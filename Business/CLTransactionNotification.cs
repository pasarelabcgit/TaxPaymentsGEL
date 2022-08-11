using PaymentEntities;
using TransactionNotification.Bussines;

namespace Business
{
    public class CLTransactionNotification
    {
        public void NotificarTransaccion(DatosTransaccionEntities objDatosTransaccion)
        {
            objDatosTransaccion.Transaccion.Project = "TaxPayment";
            Notification notification = new Notification(objDatosTransaccion);
            notification.Notificar();
        }
    }
}
