namespace BankSim.Application.Api.Dtos
{
    public  class FinaliseTransactionResponse
    {
        public string BankSimTransactionId { get; set; }
        public bool Success { get; set; }
    }
}
