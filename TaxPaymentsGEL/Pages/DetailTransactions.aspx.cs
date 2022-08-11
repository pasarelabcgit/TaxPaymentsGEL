using Business;
using PaymentEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;

namespace TaxPaymentsGEL.Pages
{
    public partial class DetailTransactions : System.Web.UI.Page
    {
        CLPaymentBussines objPasarela = new CLPaymentBussines();
        DatosTransaccionEntities oDatosTransaccionEntities = new DatosTransaccionEntities();
        Propiedades objPropiedadesTax = new Propiedades();
        
        public string CodigoMunicipio { get; set; }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (ValidarParametros())
                    {
                        AddProperties();
                    }
                    else
                    {
                        Redirect404();
                    }
                }
            }
            catch (Exception ex)
            {
                catchException(ex);
            }
        }

        private bool ValidarParametros()
        {
            if (Request.QueryString["Code"] != null)
            {
                CodigoMunicipio = Request.QueryString["Code"].ToString();
                lblcode.Text = CodigoMunicipio;
            }
            else
            {
                return false;
            }
            if (Request.QueryString["Http"] != null)
            {
                objPropiedadesTax.Http = Request.QueryString["Http"].ToString();
                lblruta.Text = objPropiedadesTax.Http;
            }
            else
            {
                return false;
            }
            if (Request.QueryString["List"] != null)
            {
                objPropiedadesTax.List = Request.QueryString["List"].ToString();
                objPropiedadesTax.Http = objPropiedadesTax.Http + "&List=" + objPropiedadesTax.List;
                lblruta.Text = objPropiedadesTax.Http;
            }
            return true;
        }

        

        protected void AddProperties()
        {
            EntidadEntities oEntidadEntities = objPasarela.GetInformationMunicipality(CodigoMunicipio);

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
            lblIP.Text = text2;

            DivIn.Style.Add("background-color", "#" + oEntidadEntities.CodigoEntidad);
            color.Style.Add("background-color", "#" + oEntidadEntities.CodigoEntidad);
            color1.Style.Add("background-color", "#" + oEntidadEntities.CodigoEntidad);
            btnBuscar.Style.Add("background-color", "#" + oEntidadEntities.CodigoEntidad);
            LblNombreMunicipio.InnerText = oEntidadEntities.Entidad;

        }

        protected void btnBuscar_Click(object sender, System.EventArgs e)
        {
            try
            {
                ListTransacciones();
            }
            catch (Exception ex)
            {
                catchException(ex);
            }
        }

        private void ListTransacciones()
        {
            List<DatosTransaccionEntities> ListData = objPasarela.ConsultaDetalleTransacciones(TxtNumeroDocumento.Text, lblcode.Text, System.Convert.ToDateTime(Request.Form[txtFechaInicio.UniqueID]), System.Convert.ToDateTime(Request.Form[txtFechaFinal.UniqueID]).AddDays(1.0));
            if (ListData.Count > 0)
            {
                GridTransac.DataSource = ListData;
                GridTransac.DataBind();
                GridTransac.HeaderRow.TableSection = System.Web.UI.WebControls.TableRowSection.TableHeader;
            }
            else
            {
                GridTransac.DataSource = null;
                GridTransac.DataBind();
                ModalInfo(objPropiedadesTax.Advertencia, "En el rango seleccionado no tiene movimientos de pago", 2);
            }
        }

        public void catchException(Exception ex)
        {
            Propiedades objpropiedades = new Propiedades();
            objpropiedades.catchException(ex);
            ModalInfo(objPropiedadesTax.Advertencia, objPropiedadesTax.MensajeTrx, 2);
        }

        #region Metodos

        protected void Timer1_Tick(object sender, System.EventArgs e)
        {
            System.DateTime now = System.DateTime.Now;
            LblFecha.Text = now.ToString("MMMM dd, yyyy h:mm:ss tt").ToLower();
            LblFecha.Text = LblFecha.Text.Substring(0, 1).ToUpper() + LblFecha.Text.Substring(1, LblFecha.Text.Length - 1);
        }

        protected void ModalInfo(string Titulo, string Mensaje, int Tipo)
        {
            Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "MostrarMensaje", string.Format("<script type='text/javascript'>MostrarMensaje('{0}','{1}','{2}');</script>", Titulo, Mensaje, Tipo));
        }

        protected void LinkButton2_Click(object sender, System.EventArgs e)
        {
            Response.Redirect(lblruta.Text, false);
        }

        private void Redirect404()
        {
            Response.Redirect("~/ErrorPages/Oops.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        #endregion

    }
}
