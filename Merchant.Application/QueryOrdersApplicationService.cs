using CheckItOut.Payments.Domain.Queries.Projections;
using Merchant.Domain;
using Merchant.Domain.HttpContracts;
using Merchant.Domain.Interfaces;
using Merchant.Domain.ViewModels;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Merchant.Application
{
    public class QueryOrdersApplicationService : IQueryOrdersApplicationService
    {
        private IOrderRepository _orderRepository;
        private IGetToSecureHttpEndpointWithRetries _getToSecureHttpEndpointWithRetries;

        public QueryOrdersApplicationService(IOrderRepository orderRepository, IGetToSecureHttpEndpointWithRetries getToSecureHttpEndpointWithRetries)
        {
            _orderRepository = orderRepository;
            _getToSecureHttpEndpointWithRetries = getToSecureHttpEndpointWithRetries;
        }

        public async Task<MerchantOrdersViewModel> GetMerchantOrdersAsync(string merchantId) 
        {
            
            
            var orders = _orderRepository.GetAllByMerchantId(merchantId);
            if (orders == null)
                return new MerchantOrdersViewModel() { MerchantId = merchantId };

            var merchantOrders = MapFrom(merchantId, orders);

            //Get live payment paid verification (this is not needed, but is an example given the spec (allow merchant to retrieve paymentStatus and maskedCardNumber given an invoiceId), also if we did need to do this we would create a bulk request accepting many Id's to reduce Http round trips, this has been made async to speed up response times):
            List<Task> listOfTasks = new List<Task>();
            foreach (var merchantOrder in merchantOrders.MerchantOrders)
            {
                listOfTasks.Add(GetPaymentDetailsAsync(merchantOrder));
            }
            await Task.WhenAll(listOfTasks);
            return merchantOrders;
        }

        private async Task GetPaymentDetailsAsync(MerchantOrderViewModel merchantOrder)
        {
            HttpResponseMessage response = _getToSecureHttpEndpointWithRetries.Get(apiClientUrl: "https://localhost:44379/payments/"+merchantOrder.PaymentId, idServerUrl: "https://localhost:5001", "client", "secret", "CheckoutApi").Result;
            if (response.IsSuccessStatusCode)
            {
                GetPaymentResponse deserialisedResponse  = await response.Content.ReadAsAsync<GetPaymentResponse>();
                merchantOrder.MaskedCardNumber = deserialisedResponse.MaskedCardNumber;
                merchantOrder.ProviderVerifiedPaymentStatus = deserialisedResponse.Succeeded;
            }
        }

        private MerchantOrdersViewModel MapFrom(string merchantId, IEnumerable<Order> orders)
        {

            var merchantOrders = new List<MerchantOrderViewModel>();
            foreach (var order in orders)
            {
                var merchantOrderProducts = new List<MerchantOrderProductViewModel>();
                var merchantOrderViewModel = new MerchantOrderViewModel()
                {
                    InvoiceId = order.InvoiceId,
                    OrderStatus = order.Status.ToString(),
                    Amount = order.TotalAmount,
                    CurrencyCode = order.CurrencyCode,
                    PaymentId = order.PaymentId??"0",
                };

                foreach (var orderItem in order.OrderItems)
                {
                    var merchantOrderProductViewModel = new MerchantOrderProductViewModel()
                    {
                        Title = orderItem.Title,
                        Amount = orderItem.Price.ToString("N2")
                    };
                    merchantOrderProducts.Add(merchantOrderProductViewModel);
                }
                merchantOrderViewModel.MerchentOrderProductViewModels = merchantOrderProducts;
                merchantOrders.Add(merchantOrderViewModel);
            };

            var merchantOrdersViewModel = new MerchantOrdersViewModel() { MerchantId = merchantId };
            merchantOrdersViewModel.MerchantOrders = merchantOrders;
            return merchantOrdersViewModel;
        }


    }
}
