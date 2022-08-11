using LogException;
using Newtonsoft.Json;
using PaymentBusiness;
using System;
using System.Collections.Generic;
using WompiTransactions;
using WompiTransactions.Entities;

namespace WompiBusiness
{
    public class WompiTransactionBusiness
    {
        WompiTransactionsBusiness oWompiTransactionsBusiness = new WompiTransactionsBusiness();
       
        public Tuple<string, bool> CrearTransaccionBotonBancolombia(DataTransactionWompi oDataTransaction)
        {
            WompiTransactionBotonBancolombiaBusiness oWompiTransactionBotonBancolombiaBusiness = new WompiTransactionBotonBancolombiaBusiness();
            return oWompiTransactionBotonBancolombiaBusiness.CrearTransaccionButtonBancolombiaWompi(oDataTransaction);
        }
        public Tuple<string, bool> CrearTransaccionCardWompi(DataTransactionWompi oDataTransaction)
        {
            WompiTransactionTDCBusiness oWompiTransactionTDCBusiness = new WompiTransactionTDCBusiness();
            return oWompiTransactionTDCBusiness.CrearTransaccionCardWompi(oDataTransaction);
        }
        public Tuple<string, bool> CrearTransaccionButtonPSEWompi(DataTransactionWompi oDataTransaction)
        {
            WompiTransactionBotonPSEBusiness oWompiTransactionBotonPSEBusiness = new WompiTransactionBotonPSEBusiness();
            return oWompiTransactionBotonPSEBusiness.CrearTransaccionButtonPSEWompi(oDataTransaction);
        }
        public string TokenCardsWompi(DataTransactionWompi oDataTransaction)
        {
            string token = string.Empty;
            TokenEntities oTokenEntities = AsignarDatosToken(oDataTransaction);
            TokenCardsResponse oTokenCardsResponse = oWompiTransactionsBusiness.TokenCards(oTokenEntities, oDataTransaction.PublicKey);
            if (oTokenCardsResponse.data != null)
            {
                token = oTokenCardsResponse.data.id;
            }

            return token;
        }
        public string TokenAcceptacionWompi(string PublicKey)
        {
            string acceptance_token = string.Empty;
            TokenAcceptacionResponse oTokenAcceptacionResponse = oWompiTransactionsBusiness.TokenAcceptacion(PublicKey);

            if (oTokenAcceptacionResponse.data != null)
            {
                acceptance_token = oTokenAcceptacionResponse.data.presigned_acceptance.acceptance_token;
            }

            return acceptance_token;

        }
        public Dictionary<string, string> financial_institutions(string PublicKey)
        {
            financial_institutionsEntities ofinancial_institutionsEntities = oWompiTransactionsBusiness.financial_institutions(PublicKey);
            Dictionary<string, string> List = new Dictionary<string, string>();

            if(ofinancial_institutionsEntities.data != null)
            {
                if (ofinancial_institutionsEntities.data.Count > 0)
                {
                    foreach (var item in ofinancial_institutionsEntities.data)
                    {
                        List.Add(item.financial_institution_code.ToString(), item.financial_institution_name);
                    }
                }
            }
            

            return List;
        }
        public string DetailTransactionWompiFinalize(string TransactionIDWompi, string PublicKey)
        {
            string resultjs = string.Empty;
            DetailTransactionResponse oTransactionResponse = new DetailTransactionResponse();
            try
            {
                oTransactionResponse = oWompiTransactionsBusiness.DetailTransaction(TransactionIDWompi, PublicKey);
                //oTransactionResponse = oWompiTransactionsBusiness.DetailTransaction(TransactionIDWompi, PublicKey);
                // return oWompiTransactionDBBusiness.UpdateFinalizeTransactionWompi(oTransactionResponse);
                resultjs = JsonConvert.SerializeObject(oTransactionResponse);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("error DetailTransactionWompi()", JsonConvert.SerializeObject(oTransactionResponse)));
            }

            return resultjs;
        }
        public DataTransactionWompi DetailTransactionWompi(long IDTransaccion,string TransactionIDWompi, string PublicKey)
        {
            
            DataTransactionWompi oDataTransactionWompi = new DataTransactionWompi();
            DetailTransactionResponse oTransactionResponse = oWompiTransactionsBusiness.DetailTransaction(TransactionIDWompi, PublicKey);
            TransactionLog(IDTransaccion, CLException.TipoError.Log_Trazabilidad, string.Format("Response Wompi {0}", Newtonsoft.Json.JsonConvert.SerializeObject(oTransactionResponse)));
            if (oTransactionResponse != null)
            {

                oDataTransactionWompi.IDTransaccionWompi = oTransactionResponse.data.id;
                oDataTransactionWompi.status = oTransactionResponse.data.status;
                oDataTransactionWompi.status_message = oTransactionResponse.data.status_message;
                oDataTransactionWompi.async_payment_url = oTransactionResponse.data.payment_method.extra.async_payment_url;
                oDataTransactionWompi.Transaction_date = oTransactionResponse.data.created_at;
            }
            else
            {
                TransactionLog(IDTransaccion, CLException.TipoError.Log_Trazabilidad, string.Format("Response Wompi {0}", Newtonsoft.Json.JsonConvert.SerializeObject(oTransactionResponse)));
            }

            return oDataTransactionWompi;
        }
        private TokenEntities AsignarDatosToken(DataTransactionWompi oDataTransaction)
        {
            return new TokenEntities()
            {
                number = oDataTransaction.number,
                cvc = oDataTransaction.cvc.ToString(),
                exp_month = oDataTransaction.exp_month.ToString(),
                exp_year = oDataTransaction.exp_year.ToString().Remove(0, 2),
                card_holder = oDataTransaction.card_holder.ToString().ToUpper(),
            };
        }
        public static void TransactionLog(long IDTransaccion, CLException.TipoError TipoError, string Log)
        {
            CLException objCLException = new CLException();
            objCLException.TransactionLog(TipoError, IDTransaccion, Log);
        }
    }
}
