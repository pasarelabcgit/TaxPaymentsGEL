using Newtonsoft.Json;
using System;
using WompiTransactions;
using WompiTransactions.Entities;

namespace WompiBusiness
{
    public class WompiTransactionBotonPSEBusiness
    {
        WompiTransactionsBusiness oWompiTransactionsBusiness = new WompiTransactionsBusiness();
        public Tuple<string, bool> CrearTransaccionButtonPSEWompi(DataTransactionWompi oDataTransaction)
        {
            bool status = false;
            string TransactionID = string.Empty;
            TransactionEntitiesPSE oTransactionEntities = AsignarDatosTransaccionPSEWompi(oDataTransaction);
            TransactionResponseDetail oTransactionResponseDetail = oWompiTransactionsBusiness.CrearTransaccionPSE(oTransactionEntities, oDataTransaction.PublicKey);
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
        private TransactionEntitiesPSE AsignarDatosTransaccionPSEWompi(DataTransactionWompi oDataTransaction)
        {
            return new TransactionEntitiesPSE()
            {
                acceptance_token = oDataTransaction.acceptance_token,
                amount_in_cents = oDataTransaction.amount_in_cents,
                currency = "COP",
                customer_email = oDataTransaction.customer_email,
                payment_method = new payment_methodPSE()
                {
                    type = "PSE",
                    user_type = oDataTransaction.user_type, // Tipo de persona, natural (0) o jurídica (1)
                    user_legal_id_type = oDataTransaction.user_legal_id_type, // Tipo de documento, CC o NIT
                    user_legal_id = oDataTransaction.user_legal_id, // Número de documento
                    financial_institution_code = oDataTransaction.financial_institution_code, // Código (`code`) de la institución financiera
                    payment_description = oDataTransaction.payment_description // Nombre de lo que se está pagando. Máximo 30 caracteres
                },
                redirect_url = System.Configuration.ConfigurationManager.AppSettings["PPE_URLTCJ"] + "?ticketID=" + oDataTransaction.IDTransaction,
                reference = oDataTransaction.reference
            };
        }
    }
}
