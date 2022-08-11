namespace Business
{
    public class ResponseCreateTransaction
    {
        public string ReturnCode { get; set; }
        public bool StateTransaction { get; set; }
        public string TransactionNumberPSE { get; set; }
        public string Bankurl { get; set; }
    }
}
