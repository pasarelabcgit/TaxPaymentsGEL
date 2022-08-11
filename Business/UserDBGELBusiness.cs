using LogException;
using PaymentEntities;
using System;
using UserDBGELBusiness;
using UserDBGELBusiness.Entities;

namespace Business
{
    public class UserDBGELBusiness
    {
        public static void InsertUserDBGEL(PagadorEntities oPagadorEntities, string CodigoEntidad, string Impuesto)
        {
            try
            {
                UserDBGEL oUserDBGEL = new UserDBGEL();

                CiudadanoGELEntities oCiudadanoGELEntities = new CiudadanoGELEntities()
                {
                    Identificacion = oPagadorEntities.IdentificacionPagador,
                    PrimerNombre = oPagadorEntities.NombrePagador,
                    SegundoNombre = string.Empty,
                    Direccion = string.Empty,
                    Email = oPagadorEntities.EmailPagador,
                    Telefono = oPagadorEntities.TelefonoPagador,
                    PrimerApellido = string.Empty,
                    SegundoApellido = string.Empty,
                    TipoDocumento = 1//CC

                };
                oUserDBGEL.InsertarUsuarioDBGEL(oCiudadanoGELEntities, CodigoEntidad, Impuesto);
            }
            catch (Exception ex)
            {
                CLException oCLException = new CLException();
                oCLException.ExceptionLog(ex, CLException.Project.ApiPayment);
            }

        }
    }
}
