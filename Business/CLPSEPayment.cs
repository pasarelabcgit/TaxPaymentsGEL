using LogException;
using PaymentEntities;
using PSEPayment;
using PSEPayment.Entities;
using System;
using System.Collections.Generic;

namespace Business
{
    public class CLPSEPayment
    {
        PSEPaymentBussines objPSEPaymentBussines = new PSEPaymentBussines();
        CLPaymentBussines objPasarela = new CLPaymentBussines();
        public Dictionary<string, string> GetBankList(string CodigoMunicipio)
        {
            return objPSEPaymentBussines.GetBankList(CodigoMunicipio);
        }
        public ResponseCreateTransaction CreateTransaction(DatosTransaccionEntities oDatosTransaccionEntities)
        {

            ResponseTransaction objResponseTransaction = new ResponseTransaction();
            TransactionPSEEntities oTransactionPSEEntities = AsignarPropiedadesPSE(oDatosTransaccionEntities);
            if (oDatosTransaccionEntities.Entidad.MultiCreditoPSE == true)
            {
                objResponseTransaction = objPSEPaymentBussines.CreateTransactionMultiCredit(oTransactionPSEEntities);
            }
            else
            {
                objResponseTransaction = objPSEPaymentBussines.CreateTransaction(oTransactionPSEEntities);
            }

            return new ResponseCreateTransaction()
            {
                ReturnCode = objResponseTransaction.ReturnCode,
                StateTransaction = objResponseTransaction.StateTransaction,
                TransactionNumberPSE = objResponseTransaction.TransactionNumberPSE,
                Bankurl = objResponseTransaction.Bankurl
            };

        }
        private TransactionPSEEntities AsignarPropiedadesPSE(DatosTransaccionEntities oDatosTransaccionEntities)
        {
            return new TransactionPSEEntities()
            {
                financialInstitutionCode = oDatosTransaccionEntities.Transaccion.TransaccionPSE.CodigoBanco,
                TransactionValue = (decimal)oDatosTransaccionEntities.Transaccion.Total,
                ticketId = oDatosTransaccionEntities.Transaccion.Factura,
                TransactionID = oDatosTransaccionEntities.Transaccion.IDTransaccion.ToString(),
                TypePerson = (int)oDatosTransaccionEntities.Pagador.TypePerson,
                IdentificacionPagador = oDatosTransaccionEntities.Pagador.IdentificacionPagador,
                Referencia = oDatosTransaccionEntities.Transaccion.Referencia,
                paymentDescription = oDatosTransaccionEntities.Impuesto.Impuesto.ToUpper(),


                entityCode = oDatosTransaccionEntities.Entidad.CodigoEntidad,
                ServiceCode = oDatosTransaccionEntities.Impuesto.CodigoServicio,


                //MultiCredit
                MunicipalityValue = (oDatosTransaccionEntities.Transaccion.TransaccionPSE.MunicipalityValue == null) ? string.Empty : oDatosTransaccionEntities.Transaccion.TransaccionPSE.MunicipalityValue.ToString(),
                CodigoPadre = (oDatosTransaccionEntities.Transaccion.TransaccionPSE.CodigoPadre == null) ? string.Empty : oDatosTransaccionEntities.Transaccion.TransaccionPSE.CodigoPadre.ToString(),
                CodeEnt = (oDatosTransaccionEntities.Transaccion.TransaccionPSE.CodeEnt == null) ? string.Empty : oDatosTransaccionEntities.Transaccion.TransaccionPSE.CodeEnt.ToString(),
                EntityValue = (oDatosTransaccionEntities.Transaccion.TransaccionPSE.EntityValue == null) ? string.Empty : oDatosTransaccionEntities.Transaccion.TransaccionPSE.EntityValue.ToString(),
                CurrencyISOcode = "ISO",
                 
            };
        }
        

            public ResponseTransactionInformacion GetTransactionInformation(string CodigoMunicipio, long IDTransaccion, string CUS, string ServiceCode)
        {
            ResponseTransactionInformacion objResponseTransaction = new ResponseTransactionInformacion();
            ResponseGetTransactionInformation objresponse = objPSEPaymentBussines.GetTransactionInformation(CodigoMunicipio, CUS, ServiceCode);
            if (objresponse.StatusResponse)
            {
                if (objresponse.Status > 0)
                {
                    objResponseTransaction.TransactionStatus = objresponse.Status.ToString();
                    objResponseTransaction.returnCode = objresponse.returnCode;
                    objResponseTransaction.transactionState = objresponse.transactionState;
                    objResponseTransaction.transactionCycle = int.Parse(objresponse.transactionCycle);
                    objResponseTransaction.trazabilityCode = objresponse.trazabilityCode;
                    objResponseTransaction.transactionValue = objresponse.transactionValue;
                    objResponseTransaction.ticketId = objresponse.ticketId;
                    objResponseTransaction.soliciteDate = objresponse.soliciteDate;
                    objResponseTransaction.bankProcessDate = objresponse.bankProcessDate;
                    InsertResponseINPSE(CodigoMunicipio, IDTransaccion, objresponse.Status.ToString(), objresponse);
                }

            }
            else
            {
                objPasarela.TransactionLog(IDTransaccion, CLException.TipoError.Error, string.Format("ocurrio un error al consultar la informacion de la transacción, Response ACH {0}", Newtonsoft.Json.JsonConvert.SerializeObject(objresponse)));
            }


            return objResponseTransaction;
        }
        private void InsertResponseINPSE(string CodigoMunicipio, long TransactionID, string StatusTRX, ResponseGetTransactionInformation oResponseGetTransactionInformation)
        {
            try
            {
                CLFinalizeTransactionPayment oCLFinalizeTransactionPayment = new CLFinalizeTransactionPayment();

                oCLFinalizeTransactionPayment.InsertarResponseACH(CodigoMunicipio, TransactionID, StatusTRX, oResponseGetTransactionInformation);
            }
            catch (Exception ex)
            {
                objPasarela.TransactionLog(TransactionID, CLException.TipoError.Log_Trazabilidad,Newtonsoft.Json.JsonConvert.SerializeObject(ex));
            }
           
        }
        public void finalizetransaction(string TransactionNumberPSE, string CodigoMunicipio, string ServiceCode)
        {
            objPSEPaymentBussines.FinalizeTransaction(TransactionNumberPSE, "12", CodigoMunicipio, ServiceCode);
        }
    }
}
