using Merchant.Ui.Web;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;
using Merchant.Domain.HttpContracts;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using System.Linq;
using Merchant.Domain.Interfaces;
using Moq;
using Merchant.Domain.ViewModels;
using Merchant.Domain;

namespace Merchant.IntegrationTests
{
    public class PciDssCheckoutTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private CustomWebApplicationFactory<Startup> _factory;

        public PciDssCheckoutTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ValidPciDssCheckoutCallsPreparePaymentServiceCreatesOrder()
        {
            //Arrange:
            //application dependency injection overides
            var mockQueryCheckoutApplicationService = new Mock<IQueryCheckoutApplicationService>();
            var mockPostToSecureHttpEndpointWithRetries = new Mock<IPostToSecureHttpEndpointWithRetries>();

            ServiceProvider privateServiceProvider = null;
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<IQueryCheckoutApplicationService>(mockQueryCheckoutApplicationService.Object);
                    services.AddSingleton<IPostToSecureHttpEndpointWithRetries>(mockPostToSecureHttpEndpointWithRetries.Object);
                    privateServiceProvider = services.BuildServiceProvider();
                    //using (var scope = privateServiceProvider.CreateScope())
                    //{
                    //    var scopedServices = scope.ServiceProvider;
                    //    orderRepository = scopedServices.GetRequiredService<IOrderRepository>();
                    //}
                });
            })
            .CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var subjectIdFromContext = "2b837f52-becd-4938-8a35-0906d8c7d591";
            var merchantId = "TEST";
            var invoiceId = Guid.NewGuid().ToString();
            var testCheckoutBasket = new Domain.ViewModels.CheckoutViewModel()
                {
                    InvoiceId = invoiceId,
                    UserId = subjectIdFromContext,
                    SellerMerchantId = merchantId,
                    SellerName = "SellIt",
                    CurrencyCode = "GBP",
                    CheckoutProductViewModels = new List<CheckoutProductViewModel>()
                        {
                            new CheckoutProductViewModel{ Title="Classic White T Shirt Size M", Price=24.99m, Delivery=0.00m},
                            new CheckoutProductViewModel{ Title="Classic Oxford Shirt Size M", Price=24.99m, Delivery=0.00m}
                        }
                };
            mockQueryCheckoutApplicationService.Setup(x=>x.GetCheckoutFromBasket(It.IsAny<string>()))
                .Returns(testCheckoutBasket);
            var paymentRequestId = Guid.NewGuid().ToString();
            var responseObject = paymentRequestId;
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage() { Content= new StringContent(JsonConvert.SerializeObject(responseObject), Encoding.UTF8, "application/json"), StatusCode= HttpStatusCode.Created};
            mockPostToSecureHttpEndpointWithRetries.Setup(x => x.Post(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>())).Returns(httpResponseMessage);
            var dto = new { invoiceId, userId = subjectIdFromContext };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            
            //Act
            var checkoutResponse = await client.PostAsync("https://localhost:44388/Checkout/Post?invoiceId="+invoiceId, content);

            //Assert:
            Order order = null;
            using (var scope = privateServiceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var orderRepository = scopedServices.GetRequiredService<IOrderRepository>();
                order = orderRepository.GetByInvoiceId(invoiceId);
            }
            Assert.Equal(HttpStatusCode.Found, checkoutResponse.StatusCode);
            Assert.True(order.InvoiceId == invoiceId
                && order.MerchantId == testCheckoutBasket.SellerMerchantId
                && order.OrderItems.Count() == testCheckoutBasket.CheckoutProductViewModels.Count()
                && order.TotalAmount == testCheckoutBasket.TotalCost);
            Assert.True(order.Status == OrderStatus.Ordered);
        }

        [Fact]
        public async Task IdempotentTest_MultipleDuplicatePostToPciDssCheckoutCreatesOnlyOneOrder()
        {
            //Arrange:
            //application dependency injection overides
            var mockQueryCheckoutApplicationService = new Mock<IQueryCheckoutApplicationService>();
            var mockPostToSecureHttpEndpointWithRetries = new Mock<IPostToSecureHttpEndpointWithRetries>();

            ServiceProvider privateServiceProvider = null;
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<IQueryCheckoutApplicationService>(mockQueryCheckoutApplicationService.Object);
                    services.AddSingleton<IPostToSecureHttpEndpointWithRetries>(mockPostToSecureHttpEndpointWithRetries.Object);
                    privateServiceProvider = services.BuildServiceProvider();
                    //using (var scope = privateServiceProvider.CreateScope())
                    //{
                    //    var scopedServices = scope.ServiceProvider;
                    //    orderRepository = scopedServices.GetRequiredService<IOrderRepository>();
                    //}
                });
            })
            .CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var subjectIdFromContext = "2b837f52-becd-4938-8a35-0906d8c7d591";
            var merchantId = "TEST";
            var invoiceId = Guid.NewGuid().ToString();
            var testCheckoutBasket = new Domain.ViewModels.CheckoutViewModel()
            {
                InvoiceId = invoiceId,
                UserId = subjectIdFromContext,
                SellerMerchantId = merchantId,
                SellerName = "SellIt",
                CurrencyCode = "GBP",
                CheckoutProductViewModels = new List<CheckoutProductViewModel>()
                        {
                            new CheckoutProductViewModel{ Title="Classic White T Shirt Size M", Price=24.99m, Delivery=0.00m},
                            new CheckoutProductViewModel{ Title="Classic Oxford Shirt Size M", Price=24.99m, Delivery=0.00m}
                        }
            };
            mockQueryCheckoutApplicationService.Setup(x => x.GetCheckoutFromBasket(It.IsAny<string>()))
                .Returns(testCheckoutBasket);
            var paymentRequestId = Guid.NewGuid().ToString();
            var responseObject = paymentRequestId;
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(responseObject), Encoding.UTF8, "application/json"), StatusCode = HttpStatusCode.Created };
            mockPostToSecureHttpEndpointWithRetries.Setup(x => x.Post(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>())).Returns(httpResponseMessage);
            var dto = new { invoiceId, userId = subjectIdFromContext };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            //Act
            var checkoutResponse = await client.PostAsync("https://localhost:44388/Checkout/Post?invoiceId=" + invoiceId, content);
            var checkoutResponse2 = await client.PostAsync("https://localhost:44388/Checkout/Post?invoiceId=" + invoiceId, content);

            //Assert:
            Order order = null;
            using (var scope = privateServiceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var orderRepository = scopedServices.GetRequiredService<IOrderRepository>();
                order = orderRepository.GetByInvoiceId(invoiceId);
            }
            Assert.NotEqual(checkoutResponse2.StatusCode, checkoutResponse.StatusCode);
            Assert.True(order.InvoiceId == invoiceId
                && order.MerchantId == testCheckoutBasket.SellerMerchantId
                && order.OrderItems.Count() == testCheckoutBasket.CheckoutProductViewModels.Count()
                && order.TotalAmount == testCheckoutBasket.TotalCost);
            Assert.True(order.Status == OrderStatus.Ordered);
        }

    }
}
