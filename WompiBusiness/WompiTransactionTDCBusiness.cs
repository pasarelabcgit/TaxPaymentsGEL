using Newtonsoft.Json;
using System;
using WompiTransactions;
using WompiTransactions.Entities;

namespace WompiBusiness
{
    public class WompiTransactionTDCBusiness
    {
        WompiTransactionsBusiness oWompiTransactionsBusiness = new WompiTransactionsBusiness();
        public Tuple<string, bool> CrearTransaccionCardWompi(DataTransactionWompi oDataTransaction)
        {
            bool status = false;
            string TransactionID = string.Empty;
            TransactionEntities oTransactionEntities = AsignarDatosTransaccionCard(oDataTransaction);
            TransactionResponseDetail oTransactionResponseDetail = oWompiTransactionsBusiness.CrearTransaccionTarjetaCredito(oTransactionEntities, oDataTransaction.PublicKey);
            if (oTransactionResponseDetail.Estado)
            {
                status = true;
                TransactionID = oTransactionResponseDetail.oTransactionResponse.data.id;
            }
            else
            {
                WompiTransactionBusiness.TransactionLog(oDataTransaction.IDTransaction, LogException.CLException.TipoError.Error_CrearTransaccionBotonBancolombiaWompi, string.Format("Request - {0}, Response - {1}", JsonConvert.SerializeObject(oTransactionEntities), JsonConvert.SerializeObject(oTransactionResponseDetail)));
            }

            return new Tuple<string, bool>(TransactionID, status);
        }
        private TransactionEntities AsignarDatosTransaccionCard(DataTransactionWompi oDataTransaction)
        {
           return new TransactionEntities()
            {
                acceptance_token = oDataTransaction.acceptance_token,
                amount_in_cents = oDataTransaction.amount_in_cents,
                currency = "COP",
                customer_email = oDataTransaction.customer_email,
                payment_method = new payment_method()
                {
                    type = "CARD",
                    token = oDataTransaction.token,
                    installments = oDataTransaction.installments
                },
                redirect_url = System.Configuration.ConfigurationManager.AppSettings["PPE_URL"] + "?ticketID=" + oDataTransaction.IDTransaction,
                reference = oDataTransaction.reference
            };
        }
    }
}
