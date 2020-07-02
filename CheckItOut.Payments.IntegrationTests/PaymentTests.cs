using CheckItOut.Payments.Api;
using CheckItOut.Payments.Domain.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using CheckItOut.Payments.Domain;
using Moq;
using CheckItOut.Payments.Domain.BankSim;
using CheckItOut.Payments.Domain.BankSim.Dto;
using CheckItOut.Payments.Application.CommandHandlers;
using CheckItOut.Payments.Domain.Commands;

namespace CheckItOut.Payments.IntegrationTests
{
    public class PaymentTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private CustomWebApplicationFactory<Startup> _factory;
        private Mock<IChargeCardAdapter> _chargeCard;
        private ServiceProvider _privateServiceProvider;

        private void SetupInitialTestData(IMerchantRepository merchantRepository)
        {
            var newMerchant = new Merchant() { MerchantId = "TEST", FullName = "bob", AccountNumber = "1111111111111111", SortCode = "111111", CardNumber= "1234123412341234", Csv="234" };
            merchantRepository.Add(newMerchant).Wait();
            merchantRepository.Save().Wait();
        }

        public PaymentTests(CustomWebApplicationFactory<Startup> factory)
        {
            _chargeCard = new Mock<IChargeCardAdapter>();

            _factory = factory;
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<IChargeCardAdapter>(_chargeCard.Object);
                    _privateServiceProvider = services.BuildServiceProvider();

                    using (var scope = _privateServiceProvider.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var merchantRepository = scopedServices.GetRequiredService<IMerchantRepository>();
                        var merchant = merchantRepository.GetById("TEST").Result;
                        if (merchant == null)
                        {
                            SetupInitialTestData(merchantRepository);
                        }
                        //var db = scopedServices
                        //    .GetRequiredService<ApplicationDbContext>();
                    }
                });
            })
            .CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task MakeValidPaymentCreatesPayment()
        {
            //Arrange:
            string bankSimTransactionId = Guid.NewGuid().ToString();
            _chargeCard.Setup(x => x.Charge(It.IsAny<FinaliseTransactionRequest>())).ReturnsAsync(new FinaliseTransactionResponse { BankSimTransactionId = bankSimTransactionId, Success=true });

            var maskedCardNumber = "############4141";
            var makePaymentCommand = new MakePaymentCommand()
            {
                Amount = 1000,
                CurrencyCode = "GBP",
                SenderCardNumber = "4141414141414141",
                SenderCvv = "111",
                RecipientMerchantId = "TEST",
                InvoiceId = Guid.NewGuid().ToString()
            };

            string paymentId = string.Empty;
            using (var scope = _privateServiceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var paymentRepository = scopedServices.GetRequiredService<IPaymentRepository>();
                var queryMerchants = scopedServices.GetRequiredService<CheckItOut.Payments.Domain.Queries.IQueryMerchants>();

                //Act:
                var paymentsCommandHandler = new PaymentsCommandHandler(paymentRepository, queryMerchants, _chargeCard.Object, new Mock<Domain.MerchantContracts.INotifyMerchantPaymentSucceeded>().Object);
                paymentId = await paymentsCommandHandler.Handle(makePaymentCommand);
            }

            //Assert:
            Payment payment = null;
            using (var scope = _privateServiceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var paymentRepository = scopedServices.GetRequiredService<IPaymentRepository>();
                payment = await paymentRepository.GetById(paymentId);
            }
            Assert.Equal(makePaymentCommand.InvoiceId, payment.InvoiceId);
            Assert.Equal(payment.SenderCardNumber, maskedCardNumber);
            Assert.Equal(makePaymentCommand.Amount, payment.Amount);
            Assert.Equal(payment.BankSimTransactionId, bankSimTransactionId);
            Assert.Equal(payment.Status, PaymentStatus.Succeeded);
        }

        //[Fact]
        //public async Task FailedPaymentRequest() 
        //{ 
        //
        //}
            
     }
}
