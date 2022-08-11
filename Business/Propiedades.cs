using LogException;
using System;
using System.Configuration;

namespace Business
{
    public class Propiedades
    {
        public string List { get; set; }

        public string Http { get; set; }

        public string EstadoFactura { get; set; }

        public string Informacion
        {
            get { return "Información"; }
        }

        public string Advertencia
        {
            get
            {
                return "Advertencia";
            }
        }

        public string TransactionNotFound
        {
            get
            {
                return "No se encontro información de transacción: {0}";
            }
        }
        public string MensajeBanco
        {
            get
            {
                return "No se pudo obtener la lista de bancos, por favor intente más tarde";
            }
        }

        public string MensajePaymentActivation
        {
            get
            {
                return "En este momento no se puede realizar la transacción, por favor intente más tarde o comuníquese con el municipio.";
            }
        }

        public string MensajeTrx
        {
            get
            {
                return "No se pudo crear la transacción, por favor intente más tarde o comuníquese con el municipio.";
            }
        }

        public string MensajeCardInvalid
        {
            get
            {
                return "En este momento no se puede realizar la transacción, Datos de la tarjeta invalidos";           }
        }

        public string MensajeCamposObligatorios
        {
            get
            {
                return "Señor (a) ciudadano, debe completar los campos obligatorios.";
            }
        }

        public string MensajeValidarCARD
        {
            get
            {
                return "Para una transacción exitosa, Debe de ingresar los campos requeridos";
            }
        }
        public string MensajeR
        {
            get
            {
                return "En este momento no se puede visualizar el detalle de la transacción, por favor intente más tarde o comuníquese con el municipio.";
            }
        }


        string[] _msg = {
            "No se pudo crear la transacción, por favor intente ",
            "más tarde o comuníquese con el municipio. Nuestras líneas de atención al cliente al teléfono ",
            " o enviarnos sus inquietudes o sugerencias al correo " };


        public string[] messages
        {
            get { return _msg; }
        }

        public void catchException(Exception ex)
        {
            CLException objexception = new CLException();
            objexception.ExceptionLog(ex, CLException.Project.TaxPaymentsGEL);
        }

        public static string GetConfiguration(string key)
        {
            AppSettingsReader appSettingsReader = new AppSettingsReader();
            return appSettingsReader.GetValue(key, typeof(string)).ToString();
        }


    }
}
