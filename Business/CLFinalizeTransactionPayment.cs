using FinalizeTransactionPayment;
using PSEPayment.Entities;
using System;

namespace Business
{

    public class CLFinalizeTransactionPayment
    {
        FinalizeTransactionBusiness oFinalizeTransactionBusiness = new FinalizeTransactionBusiness();
        public void InsertarResponseACH(string CodigoMunicipio, long TransactionID, string StatusTRX,ResponseGetTransactionInformation oResponseGetTransactionInformation)
        {
            try
            {
                oFinalizeTransactionBusiness.InsertarResponseACH(CodigoMunicipio, TransactionID, StatusTRX, "TaxPaymentsGEL", oResponseGetTransactionInformation);
            }
            catch (Exception ex)
            {
                Propiedades objpropiedades = new Propiedades();
                objpropiedades.catchException(ex);
            }

        }
    }
}
