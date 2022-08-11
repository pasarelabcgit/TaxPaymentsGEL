using System;

namespace TaxPaymentsGEL.Pages
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnPagarPSE_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtPassword.Text) && txtPassword.Text.Trim() == "1Cero12018$/*")
                {
                    Response.Redirect("~/PasarelaPagos.aspx?TransactionId=" + txtID.Text + "&Http=http://www.sanjacinto-bolivar.gov.co/Ciudadanos/Paginas/DetallePago.aspx?TicketID=HDYU6G&List=LicenciaConstruccion", false);
                }
            }
            catch
            {
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            LblFecha.Text = now.ToString("MMMM dd, yyyy h:mm:ss tt").ToLower();
            LblFecha.Text = LblFecha.Text.Substring(0, 1).ToUpper() + LblFecha.Text.Substring(1, LblFecha.Text.Length - 1);
        }
    }
}