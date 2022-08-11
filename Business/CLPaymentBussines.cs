using LogException;
using Newtonsoft.Json;
using PaymentBusiness;
using PaymentEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Business
{
    public class CLPaymentBussines
    {

        Transaction oTransaction = new Transaction();
        TransactionPSEBusiness oTransactionPSEBusiness = new TransactionPSEBusiness();
        TransactionWompiBusiness oTransactionWompiBusiness = new TransactionWompiBusiness();
        DatosTransaccionEntities oDatosTransaccionEntities = new DatosTransaccionEntities();
        CLException objCLException = new CLException();
        public CLPaymentBussines(DatosTransaccionEntities oTransaccionEntities)
        {
            DatosTransaccionEntities oDatosTransaccionEntities = oTransaccionEntities;
        }
        public CLPaymentBussines()
        {
           
        }
        public DatosTransaccionEntities DetalleTransaccionPago(long IDTransaccion)
        {

           return oTransaction.DetalleTransaccion(IDTransaccion);

        }
        public DatosTransaccionEntities DetalleTransaccionPago(string IDTransaccionWompi)
        {
            return oTransactionWompiBusiness.DetalleTransaccionPagoWompi(IDTransaccionWompi);
 
        }
        public DatosTransaccionEntities ConsultaEstadoFactura(string CodigoMunicipio, int IDTramite, string Factura)
        {
            return  oTransaction.ConsultaEstadoFactura(CodigoMunicipio, IDTramite, Factura);
        }
        public List<DatosTransaccionEntities> ConsultaDetalleTransacciones(string Document, string MunicipalityCode, DateTime StartDate, DateTime EndDate)
        {
            return oTransaction.ConsultaDetalleTransacciones(Document, MunicipalityCode, StartDate, EndDate);
        }
        public bool ActualizarInicioTransaccionPSE(DatosTransaccionEntities objDatosTransaccion)
        {
            
            bool status = oTransactionPSEBusiness.ActualizarInicioTransaccionPSE(objDatosTransaccion);

            if (status == false)
            {
                TransactionLog((long)objDatosTransaccion.Transaccion.IDTransaccion, CLException.TipoError.Error_ActualizarInicioTransaccion, JsonConvert.SerializeObject(objDatosTransaccion));
            }

            return status;
        }
        public bool ActualizarInicioTransaccionWompi(DatosTransaccionEntities objDatosTransaccion)
        {
            bool status = false;
            try
            {
                status = oTransactionWompiBusiness.ActualizarInicioTransaccionWompi(objDatosTransaccion);

                if (status == false)
                {
                    TransactionLog((long)objDatosTransaccion.Transaccion.IDTransaccion, CLException.TipoError.Error_ActualizarInicioTransaccion, JsonConvert.SerializeObject(objDatosTransaccion));
                }

            }
            catch (Exception ex)
            {
                
                objCLException.TransactionLog(CLException.TipoError.Error, (long)objDatosTransaccion.Transaccion.IDTransaccion, string.Format("Error {0} - request {1} ", Newtonsoft.Json.JsonConvert.SerializeObject(ex), Newtonsoft.Json.JsonConvert.SerializeObject(objDatosTransaccion)));
            }
           
            return status;
        }
        public void InsertTransactionConfirmation(string CodigoMunicipio, long IDtransaccion, int CodBanco, string ServiceCode, ResponseCreateTransaction oResponseCreateTransaction)
        {
            try
            {
                TransactionConfirmationEntities oTransactionConfirmationEntities = new TransactionConfirmationEntities()
                {
                    CodigoMunicipio = CodigoMunicipio,
                    IDTransaccion = IDtransaccion,
                    CodBanco = CodBanco,
                    ServiceCode = ServiceCode,
                    ReturnCode = oResponseCreateTransaction.ReturnCode,
                    TransactionNumberPSE = oResponseCreateTransaction.TransactionNumberPSE

                };
                if (oTransactionPSEBusiness.ConfirmarTransaccionPSE(oTransactionConfirmationEntities) == false)
                {
                    // log no se registro la transaccion en estado de verificacion
                }
            }
            catch (Exception)
            {
                // log no se registro la transaccion en estado de verificacion
            }
        }
        public bool ActualizaTerminaTransaccionPSE(long TransactionID, TransaccionEntities.EEstadoTransaccion EstadoTransaccion, int CicloTransaccion, DateTime FechaTransaccionBanco)
        {
            return oTransactionPSEBusiness.ActualizaTerminaTransaccionPSE(TransactionID, EstadoTransaccion, CicloTransaccion, FechaTransaccionBanco);
        }
        public bool ActualizaTerminaTransaccionWompi(long TransactionID, TransaccionEntities.EEstadoTransaccion EstadoTransaccion, string TransactionStatus, DateTime FechaTransaccionBanco)
        {
            return oTransactionWompiBusiness.ActualizaTerminaTransaccionWompi(TransactionID, EstadoTransaccion, TransactionStatus, FechaTransaccionBanco);
        }
        public EntidadEntities GetInformationMunicipality(string CodigoMunicipio)
        {
            EntidadEntities oEntidadEntities =  oTransaction.DetalleMunicipio(CodigoMunicipio);

            return oEntidadEntities;
        }
        public void TransactionLog(long IDTransaccion, CLException.TipoError TipoError, string Log)
        {
            CLException objCLException = new CLException();
            objCLException.TransactionLog(TipoError, IDTransaccion, Log);
        }
     

    }
}