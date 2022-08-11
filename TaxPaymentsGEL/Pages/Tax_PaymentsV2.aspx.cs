using Business;
using LogException;
using Newtonsoft.Json;
using PaymentEntities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WompiBusiness;

namespace TaxPaymentsGEL.Pages
{
    public partial class Tax_PaymentsV2 : System.Web.UI.Page
    {
        #region INSTANCIAS
        Propiedades objPropiedades = new Propiedades();
        CLPSEPayment objPSE = new CLPSEPayment();
        EncryptBusiness oEncryptBusiness = new EncryptBusiness();
        CLPaymentBussines oCLPaymentBussines = new CLPaymentBussines();
        WompiTransactionBusiness oWompiTransactionBusiness = new WompiTransactionBusiness();
        Tuple<bool, string> TupleConsultarEstadoFactura;
        PaymentEntities.TransaccionEntities oTransaccionEntities = new PaymentEntities.TransaccionEntities();
        public long IDTransaccion { get; set; }

        #endregion

        #region METODOS_INICIO_TRANSACCION
        protected void Page_Load(object sender, System.EventArgs e)
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
                DatosTransaccionEntities objDatosTransaccion = oCLPaymentBussines.DetalleTransaccionPago(IDTransaccion);
                if (objDatosTransaccion.Transaccion != null)
                {
                    ActivePayment(objDatosTransaccion.Impuesto, objDatosTransaccion.Entidad.CodigoEntidad, objDatosTransaccion.Transaccion.FuentePago);
                    AddPropertiesForm();
                    AsignarPropiedadesDatosPago(objDatosTransaccion);
                    ConsultarEstadoFactura(objDatosTransaccion.Entidad.CodigoEntidad, (int)objDatosTransaccion.Impuesto.IDImpuesto, objDatosTransaccion.Transaccion.Factura);
                    ConsultarListaBancos(objDatosTransaccion.Entidad.CodigoEntidad);
                }
                else
                {
                    throw new Exception(string.Format(objPropiedades.TransactionNotFound, IDTransaccion));
                }
            }
            else
            {
                Redirect404();
            }
        }
        private bool ValidarParametros()
        {
            string param = string.Empty;
            string ParameterValue = string.Empty;
            try
            {
                param = Request.QueryString["params"].ToString();
                if (string.IsNullOrEmpty(param) == false)
                {
                    string paramsDecrypt = oEncryptBusiness.Decrypt(param);
                    NameValueCollection ParameterList = HttpUtility.ParseQueryString(paramsDecrypt);

                    foreach (String QueryString in ParameterList.AllKeys)
                    {
                        ParameterValue = HttpUtility.UrlDecode((ParameterList[QueryString]));

                        if (Parameters.IsDefined(typeof(Parameters), QueryString))
                        {
                            if (string.IsNullOrEmpty(ParameterValue) == false)
                            {
                                Parameters parameter = (Parameters)Enum.Parse(typeof(Parameters), QueryString);

                                if (Parameters.TransactionId == parameter)
                                {
                                    long TransactionId;
                                    if (Int64.TryParse(ParameterValue, out TransactionId))
                                    {
                                        if (TransactionId > 0)
                                        {
                                            IDTransaccion = TransactionId;
                                            TransationID.Text = IDTransaccion.ToString();
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
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                oCLPaymentBussines.TransactionLog(IDTransaccion, CLException.TipoError.Error_ValidarParametros, JsonConvert.SerializeObject(ex));
                catchException(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Llama el metodo de cargar bancos en js
        /// </summary>
        /// <param name="PublicKey">Token unico de municipio</param>
        private void ConsultarListaBancosWompi(string PublicKey)
        {
            try
            {
                ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:cargarListaBancos('" + PublicKey + "'); ", true);
            }
            catch (Exception ex)
            {
                oCLPaymentBussines.TransactionLog(IDTransaccion, CLException.TipoError.Error_ListaBancos, Newtonsoft.Json.JsonConvert.SerializeObject(ex));
                objPropiedades.catchException(ex);
            }
        }

        /// <summary>
        /// Recibe los bancos del cliente.
        /// </summary>
        /// <param name="bancos">Lista de bancos.</param>
        
        [WebMethod]
        public void RecibirBancos(string bancos)
        {
            Dictionary<string, string> banks = JsonConvert.DeserializeObject< Dictionary<string,string>>(bancos);
            if (banks.Count > 0)
            {
                dllBancosWompi.DataSource = banks;
                dllBancosWompi.DataValueField = "key";
                dllBancosWompi.DataTextField = "value";
                dllBancosWompi.DataBind();
            }
            else
            {
                oCLPaymentBussines.TransactionLog(IDTransaccion, CLException.TipoError.Error_ListaBancos, "NO SE OBTUVO LA LISTA DE BANCOS -PSE-WOMPI");
                ModalInfo(objPropiedades.Advertencia, objPropiedades.MensajeBanco, 2);
            }
        }

        private void ActivePayment(ImpuestoEntities oImpuestoEntities, string CodigoEntidad, TransaccionEntities.EFuentePago FuentePago)
        {
            bool status = false;

            if (oImpuestoEntities.bit_PaymentWompiTC == true)
            {
                if (string.IsNullOrWhiteSpace(oImpuestoEntities.PublicKeyWompi) == false)
                {

                    PageLoadCard();
                    DVTCJ.Visible = true;
                    tabPSE.Attributes.Add("class", "tab-pane ");
                    tabWompiBotonBancolombia.Attributes.Add("class", "tab-pane");
                    tabWompiTJ.Attributes.Add("class", "tab-pane active");
                    tabWompiBotonPSE.Attributes.Add("class", "tab-pane");
                    dvContentsPayment.Visible = true;
                    status = true;
                }
                else
                {
                    oCLPaymentBussines.TransactionLog(IDTransaccion, CLException.TipoError.Error_ValidarParametros, "Medio de pago: Tarjeta Credito inhabilitado, Public Key IsNullOrEmpty");
                }
            }
            if (oImpuestoEntities.bit_PaymentBancolombia == true)
            {
                tabPSE.Attributes.Add("class", "tab-pane ");
                tabWompiBotonBancolombia.Attributes.Add("class", "tab-pane active");
                tabWompiTJ.Attributes.Add("class", "tab-pane ");
                tabWompiBotonPSE.Attributes.Add("class", "tab-pane");
                DVBotonBancolombia.Visible = true;
                dvContentsPayment.Visible = true;
                status = true;
            }
            if (oImpuestoEntities.bit_PaymentBotonPSEWompi == true)
            {
                tabPSE.Attributes.Add("class", "tab-pane ");
                tabWompiBotonBancolombia.Attributes.Add("class", "tab-pane ");
                tabWompiTJ.Attributes.Add("class", "tab-pane ");
                tabWompiBotonPSE.Attributes.Add("class", "tab-pane active");
                DVPSEWompi.Visible = true;
                dvContentsPayment.Visible = true;
                status = true;
                //Aca llamamos nuestro JS
                ConsultarListaBancosWompi(oImpuestoEntities.PublicKeyWompi);
            }

            if (CodigoEntidad == "8920993243")//Villavicencio
            {
                if (oImpuestoEntities.IDImpuesto == (int)ImpuestoEntities.EImpuesto.ImpuestoPredial || oImpuestoEntities.IDImpuesto == (int)ImpuestoEntities.EImpuesto.ImpuestoICA)
                {
                    if (FuentePago == TransaccionEntities.EFuentePago.AppMovil)
                    {
                        if (oImpuestoEntities.bit_PaymentPSE == true)
                        {

                            DVtabPSE.Visible = true;
                            tabPSE.Attributes.Add("class", "tab-pane active");
                            tabWompiBotonBancolombia.Attributes.Add("class", "tab-pane");
                            tabWompiTJ.Attributes.Add("class", "tab-pane");
                            tabWompiBotonPSE.Attributes.Add("class", "tab-pane");
                            dvContentsPayment.Visible = true;
                            status = true;
                        }
                    }
                    else
                    {
                        DVtabPSE.Visible = false;
                    }
                }

            }
            else
            {
                if (oImpuestoEntities.bit_PaymentPSE == true)
                {
                    DVtabPSE.Visible = true;
                    tabPSE.Attributes.Add("class", "tab-pane active");
                    tabWompiBotonBancolombia.Attributes.Add("class", "tab-pane");
                    tabWompiTJ.Attributes.Add("class", "tab-pane");
                    tabWompiBotonPSE.Attributes.Add("class", "tab-pane");
                    dvContentsPayment.Visible = true;
                    status = true;
                }
            }

            if (status == false)
            {
                oCLPaymentBussines.TransactionLog(IDTransaccion, CLException.TipoError.Error_ValidarParametros, "Impuesto no tiene activo ningún metodo de pago");
            }
        }
        private void AsignarPropiedadesDatosPago(DatosTransaccionEntities objDatosTransaccion)
        {
            lblCodigoMunicipio.Text = objDatosTransaccion.Entidad.CodigoEntidad;
            LblMunicipio.Text = objDatosTransaccion.Entidad.Entidad;
            lblImpuesto.Text = objDatosTransaccion.Impuesto.Impuesto.ToLower();
            lblImpuestoBotonBancolombia.Text = objDatosTransaccion.Impuesto.Impuesto.ToLower();
            lblReferenciaBotonBancolombia.Text = objDatosTransaccion.Transaccion.Factura;
            lblImpuesto.Text = lblImpuesto.Text.Substring(0, 1).ToUpper() + lblImpuesto.Text.Substring(1, lblImpuesto.Text.Length - 1);
            lblNombres.Text = objDatosTransaccion.Pagador.NombrePagador;
            lblValorTotalPagar.Text = string.Format("$ {0:N}", objDatosTransaccion.Transaccion.Total);
            lblTotal.Text = string.Format("{0:N}", objDatosTransaccion.Transaccion.Total);
            lblfact.Text = objDatosTransaccion.Transaccion.Factura;
            TxtNumeroDocumento.Text = objDatosTransaccion.Pagador.IdentificacionPagador;
            TxtEmail.Text = objDatosTransaccion.Pagador.EmailPagador;
            txtPhone.Text = objDatosTransaccion.Pagador.TelefonoPagador;
            txtNombres.Text = objDatosTransaccion.Pagador.NombrePagador;
            dllTipoDocumento.SelectedIndex = -1;
            dllTipoDocumento.Items.FindByValue(objDatosTransaccion.Pagador.TipoDocumentoPagador).Selected = true;
        }
        private void ConsultarListaBancos(string CodigoEntidad)
        {
            Dictionary<string, string> banks = objPSE.GetBankList(CodigoEntidad);
            if (banks.Count > 0)
            {
                ddlBancos.DataSource = banks;
                ddlBancos.DataValueField = "key";
                ddlBancos.DataTextField = "value";
                ddlBancos.DataBind();
            }
            else
            {
                oCLPaymentBussines.TransactionLog(IDTransaccion, CLException.TipoError.Error_ListaBancos, "NO SE OBTUVO LA LISTA DE BANCOS");
                ModalInfo(objPropiedades.Advertencia, objPropiedades.MensajeBanco, 2);
            }
        }
        private Tuple<bool, string> ConsultarEstadoFactura(string CodigoEntidad, int IDImpuesto, string Factura)
        {
            StringBuilder sb = new StringBuilder();
            bool status = false;
            string EstadoFactura = string.Empty;
            string IDTransaccion = string.Empty;
            DatosTransaccionEntities oDatosTransaccionEntities = oCLPaymentBussines.ConsultaEstadoFactura(CodigoEntidad, IDImpuesto, Factura);
            if (oDatosTransaccionEntities.Transaccion.MedioPago == TransaccionEntities.EMedioPago.PSE)
            {
                IDTransaccion = oDatosTransaccionEntities.Transaccion.TransaccionPSE.CUS;
            }
            else if (oDatosTransaccionEntities.Transaccion.MedioPago == TransaccionEntities.EMedioPago.BotonBancolombia)
            {
                IDTransaccion = oDatosTransaccionEntities.Transaccion.TransaccionWompi.transferCodeBC;
            }
            else if (oDatosTransaccionEntities.Transaccion.MedioPago == TransaccionEntities.EMedioPago.TarjetaCredito)
            {
                IDTransaccion = oDatosTransaccionEntities.Transaccion.TransaccionWompi.Transaction_ID_Wompi;
            }

            if (oDatosTransaccionEntities.Transaccion.EstadoTransaccion == TransaccionEntities.EEstadoTransaccion.APROBADA)
            {
                EstadoFactura = TransaccionEntities.EEstadoTransaccion.APROBADA.ToString();
            }
            else if (oDatosTransaccionEntities.Transaccion.EstadoTransaccion == TransaccionEntities.EEstadoTransaccion.PENDIENTE)
            {
                EstadoFactura = TransaccionEntities.EEstadoTransaccion.PENDIENTE.ToString();
            }
            if (string.IsNullOrEmpty(EstadoFactura) == false)
            {
                sb.Append("En este momento su número de Referencia o Factura ")
                .Append(oDatosTransaccionEntities.Transaccion.Factura)
                .Append(" ha finalizado su proceso de pago y cuya transacción se encuentra " + EstadoFactura + " en su entidad financiera. Si desea mayor")
                .Append(" información sobre el estado de su operación puede comunicarse a nuestra lineas de atención al cliente ")
                .Append(oDatosTransaccionEntities.Entidad.TelefonoEntidad)
                .Append(", Fax ")
                .Append(oDatosTransaccionEntities.Entidad.FaxEntidad)
                .Append(" o enviar un correo electrónico a ")
                .Append(oDatosTransaccionEntities.Entidad.EmailEntidad)
                .Append(" y preguntar por el estado de la transacción: ")
                .Append(IDTransaccion);
                objPropiedades.EstadoFactura = sb.ToString();
                ModalInfo(objPropiedades.Advertencia, objPropiedades.EstadoFactura, 2);

                status = false;
            }
            else
            {
                status = true;
            }

            return Tuple.Create(status, "");
        }

        #endregion

        #region PSE_ACH
        protected void BtnPagarPSE_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlBancos.SelectedIndex > 0)
                {
                    var data = ValidationPayment();
                    if (data.Item1)
                    {
                        DatosTransaccionEntities oDatosTransaccionEntities = AsignarDatosTransaccionPSE(data.Item2);
                        CreateTransactionPSE(oDatosTransaccionEntities);
                    }
                }
            }
            catch (Exception ex)
            {
                catchException(ex);
            }
        }
        private void CreateTransactionPSE(DatosTransaccionEntities objDatosTransaccion)
        {
            ResponseCreateTransaction objResponseTransaction = objPSE.CreateTransaction(objDatosTransaccion);


            if (objResponseTransaction.StateTransaction == true)
            {
                objDatosTransaccion.Transaccion.EstadoTransaccion = TransaccionEntities.EEstadoTransaccion.PENDIENTE;
                objDatosTransaccion.Transaccion.TransaccionPSE.CUS = objResponseTransaction.TransactionNumberPSE;
                objDatosTransaccion.Transaccion.MedioPago = TransaccionEntities.EMedioPago.PSE;
                if (oCLPaymentBussines.ActualizarInicioTransaccionPSE(objDatosTransaccion))
                {
                    oCLPaymentBussines.InsertTransactionConfirmation(objDatosTransaccion.Entidad.CodigoEntidad, IDTransaccion, int.Parse(objDatosTransaccion.Transaccion.TransaccionPSE.CodigoBanco), objDatosTransaccion.Impuesto.CodigoServicio, objResponseTransaction);
                    RedirectBanco(objResponseTransaction.Bankurl);
                }
                else
                {
                    ModalInfo(objPropiedades.Advertencia, objPropiedades.MensajeTrx, 2);
                }
            }
            else
            {
                oCLPaymentBussines.TransactionLog(IDTransaccion, CLException.TipoError.Error_ValidarParametros, string.Concat(" ERROR EN EL METODO CreateTransaction()  ", "DATA ", JsonConvert.SerializeObject(objDatosTransaccion), " RESPUESTA ACH ", JsonConvert.SerializeObject(objResponseTransaction)));
                ModalInfo(objPropiedades.Advertencia, objPropiedades.MensajeTrx, 2);
            }
        }
        private DatosTransaccionEntities AsignarDatosTransaccionPSE(DatosTransaccionEntities oDatosTransaccionEntities)
        {

            oDatosTransaccionEntities.Pagador = AsignarDataPagador();
            oDatosTransaccionEntities.Transaccion.IDTransaccion = IDTransaccion;
            oDatosTransaccionEntities.Transaccion.TransaccionPSE.CodigoBanco = ddlBancos.SelectedItem.Value.ToString();
            oDatosTransaccionEntities.Transaccion.FechaTransaccion = DateTime.Now;
            oDatosTransaccionEntities.Transaccion.EstadoTransaccion = TransaccionEntities.EEstadoTransaccion.PENDIENTE;
            oDatosTransaccionEntities.Transaccion.IP = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();

            return oDatosTransaccionEntities;
        }
        #endregion

        #region BotonBancolombiaWompi
        protected void btnBotonBancolombia_Click(object sender, EventArgs e)
        {
            try
            {
                var data = ValidationPayment();
                if (data.Item1)
                {
                    DataTransactionWompi oDataTransaction = AsignarDatosButtonBancolombia((decimal)data.Item2.Transaccion.Total, data.Item2.Impuesto.PublicKeyWompi);
                    TransaccionButtonBancolombiaWompi(oDataTransaction);
                }
            }
            catch (Exception ex)
            {
                catchException(ex);
            }
        }
        private void TransaccionButtonBancolombiaWompi(DataTransactionWompi oDataTransactionWompi)
        {
            oDataTransactionWompi.acceptance_token = oWompiTransactionBusiness.TokenAcceptacionWompi(oDataTransactionWompi.PublicKey);
            if (string.IsNullOrEmpty(oDataTransactionWompi.acceptance_token) == false)
            {
                var result = oWompiTransactionBusiness.CrearTransaccionBotonBancolombia(oDataTransactionWompi);
                if (result.Item2)
                {
                    oDataTransactionWompi.IDTransaccionWompi = result.Item1;
                    TransactionWompi(oDataTransactionWompi, TransaccionEntities.EMedioPago.BotonBancolombia);
                }
                else
                {
                    throw new Exception("oDataTransactionWompi.acceptance_token IS NULL");
                }
            }
            else
            {
                throw new Exception(string.Format("TokenAcceptacion wompi IsNullOrEmpty - public key enviada {0}", oDataTransactionWompi.PublicKey));
            }
        }
        private DataTransactionWompi AsignarDatosButtonBancolombia(decimal Total, string PublicKey)
        {
            return new DataTransactionWompi()
            {
                amount_in_cents = (int)Total * 100,
                customer_email = TxtEmail.Text,
                installments = 1,
                reference = lblfact.Text,
                IDTransaction = int.Parse(TransationID.Text),
                PublicKey = PublicKey,
                payment_description = lblImpuesto.Text,
            };
        }

        #endregion

        #region PSEWompi
        protected void btnPSEWompi_Click(object sender, EventArgs e)
        {
            try
            {
                var data = ValidationPayment();
                if (data.Item1)
                {
                    DataTransactionWompi oDataTransaction = AsignarDatosButtonPSEWompi((long)data.Item2.Transaccion.Total, data.Item2.Impuesto.PublicKeyWompi);
                    ButtonPSEWompi(oDataTransaction);
                }

            }
            catch (Exception ex)
            {
                catchException(ex);
            }
        }
        private void ButtonPSEWompi(DataTransactionWompi oDataTransactionWompi)
        {

            oDataTransactionWompi.acceptance_token = oWompiTransactionBusiness.TokenAcceptacionWompi(oDataTransactionWompi.PublicKey);
            if (string.IsNullOrEmpty(oDataTransactionWompi.acceptance_token) == false)
            {
                var result = oWompiTransactionBusiness.CrearTransaccionButtonPSEWompi(oDataTransactionWompi);
                if (result.Item2)
                {
                    oDataTransactionWompi.IDTransaccionWompi = result.Item1;
                    TransactionWompi(oDataTransactionWompi, TransaccionEntities.EMedioPago.BotonPSEWompi);
                }
                else
                {
                    throw new Exception(" oDataTransactionWompi.acceptance_token IS NULL");
                }
            }
            else
            {
                throw new Exception(string.Format("TokenAcceptacion wompi IsNullOrEmpty - public key enviada {0}", oDataTransactionWompi.PublicKey));
            }
        }
        private DataTransactionWompi AsignarDatosButtonPSEWompi(long Total, string PublicKey)
        {
            return new DataTransactionWompi()
            {
                amount_in_cents = (Total * 100),
                customer_email = TxtEmail.Text,
                reference = lblfact.Text,
                IDTransaction = int.Parse(TransationID.Text),
                PublicKey = PublicKey,
                user_type = dllTipoPersonaWompi.SelectedValue,
                user_legal_id_type = dllTipoDocumento.SelectedValue,
                user_legal_id = lblCodigoMunicipio.Text,
                financial_institution_code = dllBancosWompi.SelectedValue,
                payment_description = lblImpuesto.Text,
            };
        }

        #endregion

        #region TarjetaCreditoWompi
        private void PageLoadCard()
        {
            AsignarDatosMes();
            AsignarDatosYear();
            AsignarCuotas();
        }
        protected void BtnPagarCard_Click(object sender, EventArgs e)
        {
            try
            {
                var data = ValidationPayment();
                if (data.Item1)
                {
                    DataTransactionWompi oDataTransaction = AsignarDatosCard((decimal)data.Item2.Transaccion.Total, data.Item2.Impuesto.PublicKeyWompi);
                    TransaccionCardWompi(oDataTransaction);

                }
            }
            catch (OperationCanceledException ex)
            {
                catchException(ex);
            }
            catch (Exception ex)
            {
                catchException(ex);
            }
        }
        private void TransaccionCardWompi(DataTransactionWompi oDataTransactionWompi)
        {
            oDataTransactionWompi.token = oWompiTransactionBusiness.TokenCardsWompi(oDataTransactionWompi);
            if (string.IsNullOrEmpty(oDataTransactionWompi.token) == false)
            {
                oDataTransactionWompi.acceptance_token = oWompiTransactionBusiness.TokenAcceptacionWompi(oDataTransactionWompi.PublicKey);
                if (string.IsNullOrEmpty(oDataTransactionWompi.acceptance_token) == false)
                {
                    var result = oWompiTransactionBusiness.CrearTransaccionCardWompi(oDataTransactionWompi);
                    if (result.Item2)
                    {
                        oDataTransactionWompi.IDTransaccionWompi = result.Item1;
                        TransactionWompi(oDataTransactionWompi, TransaccionEntities.EMedioPago.TarjetaCredito);
                    }
                    else
                    {
                        throw new Exception("oDataTransactionWompi.acceptance_token IsNullOrEmpty");
                    }
                }
                else
                {
                    throw new Exception("oDataTransactionWompi.token IsNullOrEmpty");
                }
            }
            else
            {
                throw new Exception("Token Cards IsNullOrEmpty");
            }
        }
        private string GetTransactionConfirmationWompi(string TransactionIDWompi, string PublicKey)
        {
            Thread.Sleep(5000);

            DataTransactionWompi oDataTransactionWompi = oWompiTransactionBusiness.DetailTransactionWompi(IDTransaccion, TransactionIDWompi, PublicKey);

            return oDataTransactionWompi.async_payment_url;

        }
        private DataTransactionWompi AsignarDatosCard(decimal Total, string Publickey)
        {
            return new DataTransactionWompi()
            {
                number = txtCardNumber.Text,
                cvc = int.Parse(txtCVV.Text),
                exp_month = dllexp_month.SelectedValue,
                exp_year = dllexp_year.SelectedValue,
                card_holder = txtcard_holder.Text,
                amount_in_cents = (int)(Total * 100),
                customer_email = TxtEmail.Text,
                installments = int.Parse(dllinstallments.SelectedValue),
                reference = lblfact.Text,
                IDTransaction = int.Parse(TransationID.Text),
                PublicKey = Publickey,
            };
        }
        private bool ValidarDatosCard()
        {
            if (string.IsNullOrEmpty(txtcard_holder.Text))
            {
                return false;
            }
            if (string.IsNullOrEmpty(txtCardNumber.Text))
            {
                return false;
            }
            if (string.IsNullOrEmpty(txtCVV.Text))
            {
                return false;
            }
            if (string.IsNullOrEmpty(lblfact.Text))
            {
                return false;
            }
            if (int.Parse(dllexp_month.SelectedValue) <= 0)
            {
                return false;
            }
            if (int.Parse(dllexp_year.SelectedValue) <= 0)
            {
                return false;
            }
            if (int.Parse(dllinstallments.SelectedValue) <= 0)
            {
                return false;
            }

            return true;
        }
        private void AsignarDatosMes()
        {

            dllexp_month.Items.Insert(0, new ListItem("MM", "O"));
            for (int i = 1; i <= 12; i++)
            {
                dllexp_month.Items.Add(new ListItem(i.ToString().PadLeft(2, '0'), i.ToString().PadLeft(2, '0')));
            }
        }
        private void AsignarDatosYear()
        {

            int year = DateTime.Now.Year;
            dllexp_year.Items.Insert(0, new ListItem("AA", "O"));


            for (int i = 0; i <= 15; i++)
            {
                dllexp_year.Items.Add(new ListItem(year.ToString(), year.ToString()));
                year = year + 1;
            }
        }
        private void AsignarCuotas()
        {
            for (int i = 1; i <= 36; i++)
            {
                dllinstallments.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }

        #endregion

        #region Generic
        Tuple<bool, DatosTransaccionEntities> ValidationPayment()
        {
            bool status = false;
            IDTransaccion = long.Parse(TransationID.Text);
            Mostrarprogreso();
            DatosTransaccionEntities objDatosTransaccion = oCLPaymentBussines.DetalleTransaccionPago(IDTransaccion);

            if (objDatosTransaccion.Transaccion != null)
            {
                InsertUsuarioDBGEL(objDatosTransaccion.Entidad.CodigoEntidad, objDatosTransaccion.Impuesto.Impuesto);
                status = ValidarEstadoFactura(objDatosTransaccion.Entidad.CodigoEntidad, (int)objDatosTransaccion.Impuesto.IDImpuesto, objDatosTransaccion.Transaccion.Factura);
            }
            else
            {
                ModalInfo(objPropiedades.Advertencia, objPropiedades.MensajeTrx, 2);
                oCLPaymentBussines.TransactionLog(IDTransaccion, CLException.TipoError.Error_ConexionBaseDatos, "NO SE OBTUVO RESPUESTA POR PARTE DE LA BASE DE DATOS");
                status = false;
            }

            return Tuple.Create(status, objDatosTransaccion);
        }
        private bool ValidarEstadoFactura(string CodigoEntidad, int IDImpuesto, string Factura)
        {
            TupleConsultarEstadoFactura = ConsultarEstadoFactura(CodigoEntidad, IDImpuesto, Factura);
            if (TupleConsultarEstadoFactura.Item1)
            {
                if (ValidarDatos())
                {
                    return true;
                }
                else
                {
                    ModalInfo(objPropiedades.Advertencia, objPropiedades.MensajeCamposObligatorios, 2);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private void TransactionWompi(DataTransactionWompi oDataTransactionWompi, TransaccionEntities.EMedioPago oMedioPago)
        {

            DatosTransaccionEntities oDatosTransaccionEntities = AsignarDataWompiV2(oDataTransactionWompi, oMedioPago);

            if (oDatosTransaccionEntities.Transaccion.TransaccionWompi != null)
            {
                ActualizarInicioTransaccionWompi(oDatosTransaccionEntities);
            }
            else
            {
                throw new Exception("Transaccion.TransaccionWompi is null");
            }
        }
        private DatosTransaccionEntities AsignarDataWompiV2(DataTransactionWompi oDataTransactionWompi, TransaccionEntities.EMedioPago oMedioPago)
        {
            return new DatosTransaccionEntities()
            {
                Transaccion = new TransaccionEntities()
                {
                    IDTransaccion = long.Parse(TransationID.Text),
                    MedioPago = oMedioPago,
                    TransaccionWompi = new TransaccionWompiTDCEntities()
                    {
                        Amount_in_cents = (int)oDataTransactionWompi.amount_in_cents,
                        Currency = "COP",
                        Customer_email = oDataTransactionWompi.customer_email,
                        Installments = oDataTransactionWompi.installments,
                        Transaction_ID_Wompi = oDataTransactionWompi.IDTransaccionWompi,
                        Transaction_date = DateTime.Now,
                        Payment_method_type = oMedioPago.ToString(),
                        Transaction_status = "OK",
                    },
                    EstadoTransaccion = TransaccionEntities.EEstadoTransaccion.PENDIENTE,
                    IP = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString()
                },
                Impuesto = new ImpuestoEntities()
                {
                    PublicKeyWompi = oDataTransactionWompi.PublicKey,
                },
                Pagador = AsignarDataPagador()
            };
        }
        private void ActualizarInicioTransaccionWompi(DatosTransaccionEntities oDatosTransaccionEntities)
        {
            if (oCLPaymentBussines.ActualizarInicioTransaccionWompi(oDatosTransaccionEntities))
            {
                if (oDatosTransaccionEntities.Transaccion.MedioPago == TransaccionEntities.EMedioPago.TarjetaCredito)
                {
                    RedirectBanco(string.Format("{0}?ticketID={1}", Propiedades.GetConfiguration("PPE_URLTCJ"), TransationID.Text));
                }
                else if (oDatosTransaccionEntities.Transaccion.MedioPago == TransaccionEntities.EMedioPago.BotonBancolombia || oDatosTransaccionEntities.Transaccion.MedioPago == TransaccionEntities.EMedioPago.BotonPSEWompi)
                {


                    string URL = GetTransactionConfirmationWompi(oDatosTransaccionEntities.Transaccion.TransaccionWompi.Transaction_ID_Wompi, oDatosTransaccionEntities.Impuesto.PublicKeyWompi);

                    if (string.IsNullOrEmpty(URL) == false)
                    {
                        RedirectBanco(URL);
                    }
                    else
                    {
                        throw new Exception("REDIRECT URL IsNullOrEmpty");
                    }
                }
            }
            else
            {
                throw new Exception("ActualizarInicioTransaccionCard is false");
            }
        }
        PagadorEntities AsignarDataPagador()
        {
            PaymentEntities.PagadorEntities oPagadorEntities = new PagadorEntities();
            oPagadorEntities.IdentificacionPagador = TxtNumeroDocumento.Text;
            oPagadorEntities.NombrePagador = txtNombres.Text;
            oPagadorEntities.EmailPagador = TxtEmail.Text;
            oPagadorEntities.TelefonoPagador = txtPhone.Text;
            int IDTipoPersona = int.Parse(DllTipoPersona.SelectedValue);
            if (Enum.IsDefined(typeof(PagadorEntities.ETipoPersona), IDTipoPersona))
            {
                oPagadorEntities.TypePerson = (PagadorEntities.ETipoPersona)Enum.Parse(typeof(PagadorEntities.ETipoPersona), IDTipoPersona.ToString());
            }
            oPagadorEntities.TipoDocumentoPagador = dllTipoDocumento.SelectedValue;

            return oPagadorEntities;
        }
        private bool ValidarDatos()
        {
            if (string.IsNullOrEmpty(txtNombres.Text))
            {
                return false;
            }
            if (string.IsNullOrEmpty(TxtEmail.Text))
            {
                return false;
            }
            if (string.IsNullOrEmpty(txtPhone.Text))
            {
                return false;
            }
            if (string.IsNullOrEmpty(TxtNumeroDocumento.Text))
            {
                return false;
            }
            return true;
        }


        #endregion

        #region Methods
        private void InsertUsuarioDBGEL(string CodigoEntidad, string Impuesto)
        {
            try
            {
                UserDBGELBusiness.InsertUserDBGEL(AsignarDataPagador(), CodigoEntidad, Impuesto);
            }
            catch (Exception ex)
            {
                objPropiedades.catchException(ex);
            }
        }
        public void Timer1_Tick(object sender, System.EventArgs e)
        {
            DateTime MyDateTime = DateTime.Now;
            LblFecha.Text = MyDateTime.ToString("MMMM dd, yyyy h:mm:ss tt").ToLower();
            LblFecha.Text = LblFecha.Text.Substring(0, 1).ToUpper() + LblFecha.Text.Substring(1, LblFecha.Text.Length - 1);
        }
        private void AddPropertiesForm()
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
            lblIP.Text = text2;

            Session["Http"] = objPropiedades.Http;
            BtnPagarPSE.Enabled = true;

        }
        private void ModalInfo(string Titulo, string Mensaje, int Tipo)
        {
            Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "MostrarMensaje", string.Format("<script type='text/javascript'>MostrarMensaje('{0}','{1}','{2}');</script>", Titulo, Mensaje, Tipo));
        }
        private void Mostrarprogreso()
        {
            Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "Mostrarprogreso", string.Format("<script type='text/javascript'>Mostrarprogreso();</script>", new object[0]));
        }
        private void RedirectBanco(string Url)
        {
            Session["IDTrans"] = 1;
            BtnPagarPSE.Visible = false;
            BtnPagarPSE.Enabled = false;
            Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "HideModal", string.Format("<script type='text/javascript'>HideModal('{0}');</script>", Url));
        }
        private void catchException(Exception ex)
        {
            oCLPaymentBussines.TransactionLog(IDTransaccion, CLException.TipoError.Log_Trazabilidad, JsonConvert.SerializeObject(ex));
            objPropiedades.catchException(ex);
            ModalInfo(objPropiedades.Advertencia, objPropiedades.MensajeTrx, 2);
        }
        public void LinkButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (long.Parse(lblCodigoMunicipio.Text) == (long)EntidadEntities.EEntidad.Giron)
                {
                    Response.Redirect(Propiedades.GetConfiguration("URL_MunicipioGiron"), false);
                }
                else if (lblCodigoMunicipio.Text == "8000947557")
                {
                    Response.Redirect("http://186.155.31.24:8080/smarttmtsuite-web-soacha/faces/site/portal_userLogin.xhtml?type=portal&entity=alcsoa&language=es&implementation=aireportal&tl=Tnco", false);
                }
            }
            catch (Exception ex)
            {
                catchException(ex);
            }
        }
        public enum Parameters { TransactionId };
        private void Redirect404()
        {
            Response.Redirect("~/ErrorPages/Oops.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        #endregion
    }
}