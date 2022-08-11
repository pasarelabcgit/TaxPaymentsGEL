using System;

namespace Business
{
    public class ResponseTransactionInformacion
    {
        public string TransactionStatus { get; set; }
        public string returnCode { get; set; }
        public string transactionState { get; set; }
        public int transactionCycle { get; set; }
        public string trazabilityCode { get; set; }
        public decimal transactionValue { get; set; }
        public string ticketId { get; set; }
        public DateTime soliciteDate { get; set; }
        public DateTime bankProcessDate { get; set; }
    }
}
