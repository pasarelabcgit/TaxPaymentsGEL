using PaymentEntities;
using System;

namespace WompiBusiness
{
    public class DataTransactionWompi
    {
        public int IDTransaction { get; set; }
        public string number { get; set; }
        public int cvc { get; set; }
        public string exp_month { get; set; }
        public string exp_year { get; set; }
        public string card_holder { get; set; }
        public long amount_in_cents { get; set; }
        public string customer_email { get; set; }
        public int installments { get; set; }
        public string reference { get; set; }
        public string acceptance_token { get; set; }
        public string token { get; set; }
        public string PublicKey { get; set; }
        public string status { get; set; }
        public object status_message { get; set; }
        public string IDTransaccionWompi{ get; set; }
        public DateTime Transaction_date { get; set; }
        public string async_payment_url { get; set; }
        public string user_type { get; set; }
        public string user_legal_id_type { get; set; }
        public string user_legal_id { get; set; }
        public string financial_institution_code { get; set; }
        public string payment_description { get; set; }

    }
}
