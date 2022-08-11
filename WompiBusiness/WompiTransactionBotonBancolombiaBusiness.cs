using Newtonsoft.Json;
using System;
using WompiTransactions;
using WompiTransactions.Entities;

namespace WompiBusiness
{
    public class WompiTransactionBotonBancolombiaBusiness
    {
        WompiTransactionsBusiness oWompiTransactionsBusiness = new WompiTransactionsBusiness();
        
        public Tuple<string, bool> CrearTransaccionButtonBancolombiaWompi(DataTransactionWompi oDataTransaction)
        {
            bool status = false;
            string TransactionID = string.Empty;

            TransactionEntitiesBotonBancolombia oTransactionEntities = AsignarDatosTransaccionButtonBancolombia(oDataTransaction);
            TransactionResponseDetail oTransactionResponseDetail = oWompiTransactionsBusiness.CrearTransaccionBotonBancolombia(oTransactionEntities, oDataTransaction.PublicKey);
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
        private TransactionEntitiesBotonBancolombia AsignarDatosTransaccionButtonBancolombia(DataTransactionWompi oDataTransaction)
        {
            Random rd = new Random();

            return new TransactionEntitiesBotonBancolombia()
            {
                acceptance_token = oDataTransaction.acceptance_token,
                amount_in_cents = oDataTransaction.amount_in_cents,
                currency = "COP",
                customer_email = oDataTransaction.customer_email,
                payment_method = new payment_methodBotonBancolombia()
                {
                    payment_description = oDataTransaction.payment_description,
                    sandbox_status = "APPROVED",
                    type = "BANCOLOMBIA_TRANSFER",
                    user_type = "PERSON",
                    installments = 1
                },
                redirect_url = System.Configuration.ConfigurationManager.AppSettings["PPE_URLTCJ"] + "?ticketID=" + oDataTransaction.IDTransaction,
                reference = oDataTransaction.reference
            };
        }
    }
}
