using Business;
using LogException;
using PaymentEntities;
using SelectPdf;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Web.UI;

namespace TaxPaymentsGEL.Pages
{
    public partial class ResponsePaymentV2 : System.Web.UI.Page
    {
        Propiedades objPropiedades = new Propiedades();
        CLPaymentBussines oCLPaymentBussines = new CLPaymentBussines();
        NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    PageLoad();
                }
            }
            catch (Exception ex)
            {
                catchException(ex);
            }
        }

        private void PageLoad()
        {
            if (ValidarParametros())
            {
                DatosTransaccionEntities objDatosTransaccion = oCLPaymentBussines.DetalleTransaccionPago(long.Parse(TransationID.Text));
                if (objDatosTransaccion.Transaccion.IDTransaccion > 0)
                {
                    objDatosTransaccion = GetTransactionInformationPSE(objDatosTransaccion);
                    ActualizarDefTransaccion(objDatosTransaccion);
                    PropiedadesDetallePago(objDatosTransaccion);
                    AddPropertiesForm(objDatosTransaccion.Entidad.CodigoEntidad);             
                }
                else
                {
                    Redirect404();
                }
            }
            else
            {
                Redirect404();
            }
        }

        private bool ValidarParametros()
        {
            long TransactionId = 0;
            if (Request.QueryString["ticketID"] != null)
            {
                string param = Request.QueryString["ticketID"].ToString();

                if (string.IsNullOrEmpty(param) == false)
                {
                    if (Int64.TryParse(param, out TransactionId))
                    {
                        TransationID.Text = TransactionId.ToString();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                oCLPaymentBussines.TransactionLog(TransactionId, CLException.TipoError.Error_ValidarParametros, "No se encontro el parametro QueryString[ticketID]-");
                return false;
            }
        }

        protected DatosTransaccionEntities GetTransactionInformationPSE(DatosTransaccionEntities objDatosTransaccion)
        {
            CLPSEPayment objPSEPaymentBussines = new CLPSEPayment();
            ResponseTransactionInformacion objResponseTransaction = objPSEPaymentBussines.GetTransactionInformation(objDatosTransaccion.Entidad.CodigoEntidad, (long)objDatosTransaccion.Transaccion.IDTransaccion, objDatosTransaccion.Transaccion.TransaccionPSE.CUS, objDatosTransaccion.Impuesto.CodigoServicio);
            if (objResponseTransaction != null)
            {
                objDatosTransaccion.Transaccion.EstadoTransaccion = (TransaccionEntities.EEstadoTransaccion)Enum.Parse(typeof(TransaccionEntities.EEstadoTransaccion), objResponseTransaction.TransactionStatus.ToString());
                objDatosTransaccion.Transaccion.TransaccionPSE.ResponseReturnCode = objResponseTransaction.returnCode;
                objDatosTransaccion.Transaccion.TransaccionPSE.ResponseTransactionStateCode = objResponseTransaction.transactionState;
                objDatosTransaccion.Transaccion.TransaccionPSE.CicloTransaccion = objResponseTransaction.transactionCycle;
                objDatosTransaccion.Transaccion.TransaccionPSE.CUS = objResponseTransaction.trazabilityCode;
                objDatosTransaccion.Transaccion.Total = objResponseTransaction.transactionValue;
                objDatosTransaccion.Transaccion.Factura = objResponseTransaction.ticketId;
                objDatosTransaccion.Transaccion.FechaTransaccion = objResponseTransaction.soliciteDate;
                objDatosTransaccion.Transaccion.FechaTransaccionBanco = objResponseTransaction.bankProcessDate;
            }
            else
            {
                oCLPaymentBussines.TransactionLog((long)objDatosTransaccion.Transaccion.IDTransaccion, CLException.TipoError.Error_CrearTransaccionACH, "NO SE OBTUVO RESPUESTA POR PARTE DE ACH");
            }


            return objDatosTransaccion;
        }

        protected void PropiedadesDetallePago(DatosTransaccionEntities objDatosTransaccion)
        {
            if (objDatosTransaccion.Transaccion.EstadoTransaccion == TransaccionEntities.EEstadoTransaccion.PENDIENTE)
            {
                ModalInfo(objPropiedades.Advertencia, "Por favor verificar si el débito fue realizado en el Banco", 2);
            }
            LblMunicipio.Text = objDatosTransaccion.Entidad.Entidad;
            LblNombreMunicipio.Text = objDatosTransaccion.Entidad.Entidad;
            LblNit.Text = objDatosTransaccion.Entidad.CodigoEntidad;
            LblTelefono.Text = objDatosTransaccion.Entidad.TelefonoEntidad;
            lblEmailMunicipio.Text = objDatosTransaccion.Entidad.EmailEntidad;
            LblNombre.Text = objDatosTransaccion.Pagador.NombrePagador;
            LblIdent.Text = objDatosTransaccion.Pagador.IdentificacionPagador;
            LblPhone.Text = objDatosTransaccion.Pagador.TelefonoPagador;
            LblEmail.Text = objDatosTransaccion.Pagador.EmailPagador;
            LblIP.Text = objDatosTransaccion.Transaccion.IP;
            LblTransactionState.Text = objDatosTransaccion.Transaccion.EstadoTransaccion.ToString().ToLower();
            LblTransactionState.Text = LblTransactionState.Text.Substring(0, 1).ToUpper() + LblTransactionState.Text.Substring(1, LblTransactionState.Text.Length - 1);
            LbltrazabilityCode.Text = objDatosTransaccion.Transaccion.TransaccionPSE.CUS;
            LblTransactionValue.Text = ((decimal)objDatosTransaccion.Transaccion.Total).ToString("C", nfi);
            LblTickedID.Text = objDatosTransaccion.Transaccion.Factura;
            //LblTickedID.Text = objDatosTransaccion.Factura.PadLeft(8, '0');
            LblSolicitDate.Text = ((DateTime)objDatosTransaccion.Transaccion.FechaTransaccion).ToString("dd/MM/yyyy");
            LblBankProcessingDate.Text = ((DateTime)objDatosTransaccion.Transaccion.FechaTransaccionBanco).ToString("dd/MM/yyyy");
            LblTransactionCycle.Text = objDatosTransaccion.Transaccion.TransaccionPSE.CicloTransaccion.ToString();
            LblVATAmount.Text = objDatosTransaccion.Transaccion.TransaccionPSE.IVATAmount.ToString();
            lblBanco.Text = objDatosTransaccion.Transaccion.TransaccionPSE.NombreBanco;
            lblImpuesto.Text = objDatosTransaccion.Impuesto.Impuesto.ToLower();
            lblImpuesto.Text = lblImpuesto.Text.Substring(0, 1).ToUpper() + lblImpuesto.Text.Substring(1, lblImpuesto.Text.Length - 1);
            if (LblCodigoMunicipio.Text == "8000947557")
            {
                btnFinalizar.Visible = true;
            }
        }

        protected void ActualizarDefTransaccion(DatosTransaccionEntities objDatosTransaccion)
        {
            CLTransactionNotification objnotification = new CLTransactionNotification();
            if (oCLPaymentBussines.ActualizaTerminaTransaccionPSE((long)objDatosTransaccion.Transaccion.IDTransaccion, objDatosTransaccion.Transaccion.EstadoTransaccion, (int)objDatosTransaccion.Transaccion.TransaccionPSE.CicloTransaccion, (DateTime)objDatosTransaccion.Transaccion.FechaTransaccionBanco))
            {
                objnotification.NotificarTransaccion(objDatosTransaccion);
            }
            else
            {
                oCLPaymentBussines.TransactionLog((long)objDatosTransaccion.Transaccion.IDTransaccion, CLException.TipoError.Error_CrearTransaccionACH, "ActualizaTerminaTransaccion IS FALSE");
            }
        }

        protected void Imprimir_Click(object sender, EventArgs e)
        {
            try
            {
                ComprobanteTransaccionPDF();
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
            catch (Exception ex)
            {
                catchException(ex);
            }
        }

        private void ComprobanteTransaccionPDF()
        {
            PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), "A4", true);
            PdfPageOrientation pdfOrientation = (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation), "Portrait", true);
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.WebPageWidth = 1024;
            converter.Options.WebPageHeight = 0;
            PdfDocument doc = converter.ConvertHtmlString(CuerpoCorreoDetalleTransaccion(), "");
            doc.Save(Response, true, "DetalleTransaccion.pdf");
            doc.Close();
        }

        protected string CuerpoCorreoDetalleTransaccion()
        {

            string body = string.Empty;
            string archivo = Server.MapPath("/") + "\\PaymentGateway\\Content\\template\\DetalleTransaccion.html";
            FileStream Archivo = System.IO.File.Open(archivo, FileMode.Open);
            using (StreamReader reader = new StreamReader(Archivo))
            {
                body = reader.ReadToEnd();
            }
            string Municipio = LblMunicipio.Text;
            Municipio = Municipio.ToUpper();
            body = body.Replace("{NombreMunicipio}", Municipio);
            body = body.Replace("{EstadoTransaccion}", LblTransactionState.Text);
            body = body.Replace("{CUS}", LbltrazabilityCode.Text);
            body = body.Replace("{Banco}", lblBanco.Text);
            body = body.Replace("{Descripcion}", lblImpuesto.Text);
            body = body.Replace("{ValorTransaccion}", LblTransactionValue.Text);
            body = body.Replace("{NumeroFactura}", LblTickedID.Text);
            body = body.Replace("{FechaSolicitud}", LblSolicitDate.Text);
            body = body.Replace("{FechaRespuesta}", LblBankProcessingDate.Text);
            body = body.Replace("{CicloTransaccion}", LblTransactionCycle.Text);
            body = body.Replace("{ValorIva}", LblVATAmount.Text);
            body = body.Replace("{Nombre}", LblNombre.Text);
            body = body.Replace("{Identificacion}", LblIdent.Text);
            body = body.Replace("{Telefono}", LblPhone.Text);
            body = body.Replace("{Email}", LblEmail.Text);
            body = body.Replace("{IP}", LblIP.Text);
            body = body.Replace("{NameMunicipio}", LblMunicipio.Text);
            body = body.Replace("{Nit}", LblNit.Text);
            body = body.Replace("{TelefonoMunicipio}", LblTelefono.Text);

            return body;

        }

        #region Methods

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            DateTime MyDateTime = DateTime.Now;
            LblFecha.Text = MyDateTime.ToString("MMMM dd, yyyy h:mm:ss tt").ToLower();
            LblFecha.Text = LblFecha.Text.Substring(0, 1).ToUpper() + LblFecha.Text.Substring(1, LblFecha.Text.Length - 1);
        }

        protected void AddPropertiesForm(string CodigoEntidad)
        {

            System.Web.HttpContext current = System.Web.HttpContext.Current;
            string text = current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            string text2;
            if (string.IsNullOrEmpty(text) == false)
            {
                string[] array = text.Split(new char[]
                {
                        ','
                });
                if (array.Length != 0)
                {
                    text2 = array[0];
                }
            }
            text2 = current.Request.ServerVariables["REMOTE_ADDR"];
            lblIPp.Text = text2;
            Session["IDTrans"] = null;

            if (CodigoEntidad == "891180009")
            {
                //LblRuta.Text = "http://www.alcaldiasoacha.gov.co/";
            }
            else
            {
                //LblRuta.Text = Session["Http"].ToString();
            }
        }

        protected void ModalInfo(string Titulo, string Mensaje, int Tipo)
        {
            Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "MostrarMensaje",
            string.Format("<script type='text/javascript'>MostrarMensaje('{0}','{1}','{2}');</script>",
            Titulo, Mensaje, Tipo));
        }

        private void catchException(Exception ex)
        {
            Propiedades objpropiedades = new Propiedades();
            objpropiedades.catchException(ex);
        }

        protected void btnFin_Click1(object sender, EventArgs e)
        {
            try
            {
                if (LblCodigoMunicipio.Text == "8000947557")
                {
                    btnFinalizar.Visible = true;
                    RedirectSoacha();
                }

                //string sruta = LblRuta.Text;
                //if (LblCodigoMunicipio.Text == "8902048026")
                //{
                //    sruta = lblURLDevuelva.Text;
                //}

                //else if (!sruta.Contains("TicketID"))
                //{
                //    if (sruta.Contains("?"))
                //        sruta = string.Concat(sruta, "&");
                //    else
                //        sruta = string.Concat(sruta, "?");

                //    sruta += string.Concat("TicketID=", LblTickedID.Text);
                //}
                //Response.Redirect(sruta, false);
                //Session.RemoveAll();
            }
            catch (Exception ex)
            {
                catchException(ex);
            }
        }

        private void Redirect404()
        {
            Response.Redirect("~/ErrorPages/Oops.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void RedirectSoacha()
        {
            try
            {
                NameValueCollection collections = new NameValueCollection();
                collections.Add("estado_pol", LblTransactionState.Text);
                collections.Add("ref_venta", LblTickedID.Text);
                collections.Add("ref_pol", TransationID.Text);
                collections.Add("valor", LblTransactionValue.Text);
                collections.Add("cus", LbltrazabilityCode.Text);
                collections.Add("email_Comprador", LblEmail.Text);
                collections.Add("fecha_transaccion", LblFecha.Text);
                collections.Add("medio_pago", "PSE");
                string remoteUrl = "http://186.155.31.24:8080/smarttmtsuite-web-soacha/faces/site/resumenPagos.xhtml";

                string html = "<html><head>";
                html += "</head><body onload='document.forms[0].submit()'>";
                html += string.Format("<form style='display:none' name='PostForm' method='POST' action='{0}'>", remoteUrl);
                foreach (string key in collections.Keys)
                {
                    html += string.Format("<input name='{0}' type='text' value='{1}'>", key, collections[key]);
                }
                html += "</form></body></html>";
                Response.Clear();
                Response.ContentEncoding = Encoding.GetEncoding("ISO-8859-1");
                Response.HeaderEncoding = Encoding.GetEncoding("ISO-8859-1");
                Response.Charset = "ISO-8859-1";
                Response.Write(html);
                Response.End();
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
            catch (Exception ex)
            {
                catchException(ex);
            }

        }
        #endregion
    }
}