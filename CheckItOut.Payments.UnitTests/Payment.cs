using System;
using Xunit;

namespace CheckItOut.Payments.UnitTests
{
    public class Payment
    {
        [Fact]
        public void CanMakeValidPayment()
        {
            //Arrange:
            Domain.Payment payment = new Domain.Payment();
            
            //Act:
            payment.MakePayment();
            
            //Assert:
            Assert.True(false);
        }
    }
}
