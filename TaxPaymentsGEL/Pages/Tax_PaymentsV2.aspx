<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tax_PaymentsV2.aspx.cs" Inherits="TaxPaymentsGEL.Pages.Tax_PaymentsV2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript" id="www-widgetapi-script" src="https://s.ytimg.com/yts/jsbin/www-widgetapi-vflBH_DEu/www-widgetapi.js" async=""></script>
    <script type="text/javascript" id="www-widgetapi-script" src="https://s.ytimg.com/yts/jsbin/www-widgetapi-vflBH_DEu/www-widgetapi.js" async=""></script>
    <script type="text/javascript" id="www-widgetapi-script" src="https://s.ytimg.com/yts/jsbin/www-widgetapi-vflBH_DEu/www-widgetapi.js" async=""></script>
    <script src="https://www.youtube.com/iframe_api?v=3.2.10" id="YTAPI"></script>
    <meta charset="UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Pasarela de Pagos 1CERO1 Software</title>
    <link rel="shortcut icon" type="image/png" href="~/Content/img/favicon.ico" />
    <link rel="icon" href="img/favicon.png" type="image/png" sizes="16x16" />
    <link href="https://fonts.googleapis.com/css?family=Montserrat:400,500,600,700%7COpen+Sans&amp;display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="~/assetsPayment/css/bootstrap.min.css" />
    <link rel="stylesheet" href="~/assetsPayment/css/magnific-popup.css" />
    <link rel="stylesheet" href="~/assetsPayment/css/themify-icons.css" />
    <link rel="stylesheet" href="~/assetsPayment/css/animate.min.css" />
    <link rel="stylesheet" href="~/assetsPayment/css/jquery.mb.YTPlayer.min.css" />
    <link rel="stylesheet" href="~/assetsPayment/css/owl.carousel.min.css" />
    <link rel="stylesheet" href="~/assetsPayment/css/owl.theme.default.min.css" />
    <link rel="stylesheet" href="~/assetsPayment/css/style.css" />
    <link rel="stylesheet" href="~/assetsPayment/css/responsive.css" />
    <link href="~/Content/css/pasarela.css" rel="stylesheet" />
    <style>
        .offer-tag {
            background: #0704ad;
            padding: 15px;
            border-radius: 10px 60px;
            position: inherit;
            left: 0;
            top: 0%;
            box-shadow: 0 4px 20px 0 rgba(0,0,0,.15);
            color: #ffffff;
            margin-top: 20px;
        }

        .input-icon {
            height: calc(3.4rem + 2px);
            position: absolute;
            z-index: 2;
            display: block;
            width: 0px;
            top: 0;
            bottom: 0;
            left: 0;
        }

        .background-img:before {
            position: absolute;
            content: '';
            background-image: linear-gradient(to right, rgb(1 12 115 / 50%), rgb(11 2 97 / 50%), rgb(4 9 103 / 50%), rgb(13 3 105 / 50%), rgb(12 2 165 / 50%));
            width: 100%;
            height: 100%;
            top: 0;
            left: 0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" ID="ScriptManager2"></asp:ScriptManager>
        <asp:Label ID="TransationID" Visible="false" runat="server" Text=""></asp:Label>
        <asp:Label runat="server" ID="lblCodigoMunicipio" Visible="false"></asp:Label>
        <header class="header">
        </header>
        <div class="main">
            <section class="hero-section pt-50 background-img" style="background: url('https://www.electronicpaymentsinternational.com/wp-content/uploads/sites/4/2019/11/Top-influencers-in-payments-tech-Q3-2019-770x578.jpg')no-repeat center center / cover">
                <div class="container">
                    <div class="row align-items-center justify-content-center">
                        <div class="col-md-3 col-lg-3">
                            <img src="https://1cero1pay.com/PaymentGateway/content/img/escudo_colombia.png" />
                        </div>
                        <div class="col-md-5 col-lg-5">
                        </div>
                        <div class="col-md-4 col-lg-4" style="text-align: center;">
                            <b>
                                <p style="color: white;">
                                    IP Equipo:
                                    <asp:Label ID="lblIP" runat="server"></asp:Label>
                                </p>
                                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                    <ContentTemplate>
                                        <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Interval="800"></asp:Timer>
                                        <p style="color: white;">
                                            Fecha:
                                            <asp:Label ID="LblFecha" runat="server"></asp:Label>
                                        </p>
                                        <label>
                                            <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                                            <asp:Label ID="Label2" Visible="false" runat="server" Text=""></asp:Label>
                                        </label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </b>
                        </div>
                    </div>
                    <div class="row align-items-center justify-content-center">
                        <div class="col-md-12 col-lg-12">
                            <div class="page-header-content text-white text-center pt-sm-5 pt-md-5 pt-lg-0">
                                <h1 class="text-white mb-0">
                                    <asp:Label ID="LblMunicipio" runat="server"></asp:Label></h1>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <ul class="list-inline counter-wrap">
                            <li class="list-inline-item">
                                <div class="single-counter text-center">
                                    <span>Impuesto:</span>
                                    <h6>
                                        <asp:Label ID="lblImpuesto" Style="color: black; font-size: 20px;" runat="server"></asp:Label></h6>
                                </div>
                            </li>
                            <li class="list-inline-item">
                                <div class="single-counter text-center">
                                    <span>Factura:</span>
                                    <h6>
                                        <asp:Label ID="lblfact" Style="color: black; font-size: 20px;" runat="server"></asp:Label></h6>
                                </div>
                            </li>
                            <li class="list-inline-item">
                                <div class="single-counter text-center">
                                    <span>Total:</span>
                                    <h6>
                                        <asp:Label ID="lblTotal" Style="color: black; font-size: 20px;" runat="server"></asp:Label></h6>
                                </div>
                            </li>
                            <li class="list-inline-item">
                                <div class="single-counter text-center">
                                    <span>Nombre Completo:</span>
                                    <h6>
                                        <asp:Label ID="lblNombres" Style="color: black; font-size: 20px;" runat="server"></asp:Label></h6>
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </section>
            <section id="pricing" class="package-section ptb-100">
                <div class="container">
                    <div class="row justify-content-center">
                        <div class="col-lg-5 col-md">
                            <div class="card text-center single-pricing-pack" style="padding: 58px 0;">
                                <div class="card-header py-5 border-0 pricing-header">
                                    <div class="h4 text-center mb-0"><span class="price font-weight-bolder">DATOS DEL PAGADOR</span></div>
                                    <span class="h6 text-muted">Señor (a) ciudadano para una transacción exitosa ingresa los datos de forma correcta.</span>
                                </div>
                                <div class="card-body">
                                    <div class="login-signup-form">
                                        <div class="row">
                                            <div class="col-sm-6 col-12">
                                                <div class="form-group">
                                                    <label class="pb-1">Tipo Documento</label>
                                                    <div class="input-group input-group-merge">
                                                        <div class="input-icon">
                                                            <span class="ti-user color-primary"></span>
                                                        </div>
                                                        <asp:DropDownList ID="dllTipoDocumento" class="form-control" runat="server" TabIndex="3">
                                                            <asp:ListItem Value="CC">Cédula de Ciudadanía</asp:ListItem>
                                                            <asp:ListItem Value="CE">Cédula de Extranjería</asp:ListItem>
                                                            <asp:ListItem Value="NIT">Nit</asp:ListItem>
                                                            <asp:ListItem Value="TI">Tarjeta de Identidad</asp:ListItem>
                                                            <asp:ListItem Value="PP">Pasaporte</asp:ListItem>
                                                            <asp:ListItem Value="IDC">ID’s</asp:ListItem>
                                                            <asp:ListItem Value="CEL">CEL</asp:ListItem>
                                                            <asp:ListItem Value="RC">Registro Civil de Nacimiento</asp:ListItem>
                                                            <asp:ListItem Value="DE">Documento de Identificación Extranjero</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-6 col-12">
                                                <div class="form-group">
                                                    <label class="pb-1">Identificación</label>
                                                    <div class="input-group input-group-merge">
                                                        <div class="input-group input-group-merge">
                                                            <div class="input-icon">
                                                                <span class="ti-id-badge color-primary"></span>
                                                            </div>
                                                            <asp:TextBox ID="TxtNumeroDocumento" runat="server"
                                                                CssClass="form-control" placeholder="Recuerde ingresar sólo números" MaxLength="12"
                                                                onfocus="OcultarDivErrores(TxtNumeroDocumentoError);" TabIndex="2"></asp:TextBox>
                                                            <div id="TxtNumeroDocumentoError" class="errores ocultar">
                                                                <asp:RequiredFieldValidator ID="TxtNumeroDocumentoRequired" runat="server"
                                                                    ControlToValidate="TxtNumeroDocumento" CssClass="tooltips"
                                                                    ErrorMessage="Debe ingresar su número de identificación"
                                                                    ValidationGroup="ValidacionFormulario"></asp:RequiredFieldValidator>
                                                                <ajax:FilteredTextBoxExtender ID="TxtNumeroDocumentoFiltro" runat="server"
                                                                    FilterType="Numbers" TargetControlID="TxtNumeroDocumento" />
                                                                <asp:RegularExpressionValidator ValidationGroup="ValidacionFormulario"
                                                                    ID="TxtNumeroDocumentoRegularExpression" SetFocusOnError="false" runat="server"
                                                                    ControlToValidate="TxtNumeroDocumento" ErrorMessage="Mínimo 5 caracteres" ValidationExpression=".{5}.*" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-12 col-12">
                                                <div class="form-group">
                                                    <label class="pb-1">Nombre Completo</label>
                                                    <div class="input-group input-group-merge">
                                                        <div class="input-group input-group-merge">
                                                            <div class="input-icon">
                                                                <span class="ti-user color-primary"></span>
                                                            </div>
                                                            <asp:TextBox ID="txtNombres" runat="server"
                                                                CssClass="form-control" placeholder="Sólo letras, no se puede ingresar números ni símbolos."
                                                                MaxLength="100" onfocus="OcultarDivErrores(txtNameError);" TabIndex="4"></asp:TextBox>
                                                            <div id="txtNameError" class="errores ocultar">
                                                                <asp:RequiredFieldValidator ID="txtNameRequired" runat="server"
                                                                    ControlToValidate="txtNombres" ErrorMessage="Debe ingresar su primer nombre"
                                                                    ValidationGroup="ValidacionFormulario"></asp:RequiredFieldValidator>
                                                                <ajax:FilteredTextBoxExtender ID="txtNameFilteredTextBoxExtender" runat="server" TargetControlID="txtNombres"
                                                                    FilterType="Custom,LowercaseLetters, uppercaseLetters" ValidChars=" ñÑáÁéÉíÍóÓúÚ" />
                                                                <asp:RegularExpressionValidator ValidationGroup="ValidacionFormulario"
                                                                    ID="txtNameRegularExpression" SetFocusOnError="false" runat="server"
                                                                    ControlToValidate="txtNombres" ErrorMessage="Mínimo 3 caracteres" ValidationExpression=".{3}.*" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6 col-12">
                                                <div class="form-group">
                                                    <label class="pb-1">Email</label>
                                                    <div class="input-group input-group-merge">
                                                        <div class="input-group input-group-merge">
                                                            <div class="input-icon">
                                                                <span class="ti-email color-primary"></span>
                                                            </div>
                                                            <asp:TextBox ID="TxtEmail"
                                                                runat="server" name="email" MaxLength="100" CssClass="form-control"
                                                                placeholder="Recuerde hacerlo en el formato correcto. Ejemplo: ciudadano@mail.co"
                                                                onfocus="OcultarDivErrores(TxtEmailError);" TabIndex="5"></asp:TextBox>
                                                            <div id="TxtEmailError" class="errores ocultar">
                                                                <asp:RequiredFieldValidator ID="TxtEmailRequiredFieldValidator" runat="server" ControlToValidate="TxtEmail"
                                                                    CssClass="tooltips" ErrorMessage="Debe ingresar un correo electrónico"
                                                                    ValidationGroup="ValidacionFormulario"></asp:RequiredFieldValidator>
                                                                <asp:RegularExpressionValidator
                                                                    ID="TxtEmailRegularExpressionValidator" runat="server" ErrorMessage="Correo electrónico incorrecto"
                                                                    ControlToValidate="TxtEmail" ValidationExpression="^\s*(([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+([;.](([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+)*\s*$"
                                                                    ValidationGroup="ValidacionFormulario"></asp:RegularExpressionValidator>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-6 col-12">
                                                <div class="form-group">
                                                    <label class="pb-1">Teléfono</label>
                                                    <div class="input-group input-group-merge">
                                                        <div class="input-group input-group-merge">
                                                            <div class="input-icon">
                                                                <span class="ti-mobile color-primary"></span>
                                                            </div>
                                                            <asp:TextBox ID="txtPhone" runat="server"
                                                                CssClass="form-control" placeholder="Recuerde ingresar sólo números" MaxLength="12"
                                                                onfocus="OcultarDivErrores(txtPhoneError);" TabIndex="6"></asp:TextBox>
                                                            <div id="txtPhoneError" class="errores ocultar">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                                    ControlToValidate="txtPhone" CssClass="tooltips"
                                                                    ErrorMessage="Debe ingresar su número de teléfono"
                                                                    ValidationGroup="ValidacionFormulario"></asp:RequiredFieldValidator>
                                                                <ajax:FilteredTextBoxExtender ID="txtPhoneFilteredTextBoxExtender" runat="server"
                                                                    FilterType="Numbers" TargetControlID="txtPhone" />
                                                                <asp:RegularExpressionValidator ValidationGroup="ValidacionFormulario" ID="txtPhoneRegularExpressionValidator"
                                                                    SetFocusOnError="false" runat="server" ControlToValidate="txtPhone"
                                                                    ErrorMessage="Mínimo 7 caracteres" ValidationExpression=".{7}.*" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- Password -->
                                        <!-- Submit -->
                                        <div class="row">
                                            <div class="col-sm-12 mt-3">
                                                <div class="offer-tag">
                                                    <div class="ribbon-2">
                                                        <span>Total a Pagar</span>
                                                    </div>
                                                    <div class="offer-price" style="font-size: 40px;">
                                                        <asp:Label ID="lblValorTotalPagar" Style="font-size: 40px; top: 7px;" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <%-- <div class="mt-5 text-center">
                                            <p class="mb-2">
                                                Para cancelar la transacción de clic <a href="#" class="color-secondary">Aqui</a>
                                            </p>

                                        </div>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-7 col-md">
                            <div class="card text-center single-pricing-pack">
                                <div class="card-body">
                                    <div class="col-md-12">
                                        <div class="feature-contents section-heading">
                                            <div class="feature-content-wrap">
                                                <div class="h4 text-center mb-0"><span class="price font-weight-bolder">Medios de Pago:</span></div>
                                                <br />
                                                <ul class="row justify-content-center nav nav-tabs feature-tab" data-tabs="tabs">
                                                    <li class="nav-item" style="width: 22%;" id="DVtabPSE" visible="false" runat="server">
                                                        <a class="nav-link h6 active" href="#tabPSE" data-toggle="tab">
                                                            <img src="../Content/img/pse1.png" alt="pricing img" class="img-fluid" style="width: 53%;" />
                                                            <span style="font-size: 14px;">Pago por PSE</span>
                                                        </a>
                                                    </li>
                                                    <li class="nav-item" style="width: 25%;" visible="false" id="DVBotonBancolombia" runat="server">
                                                        <a class="nav-link h6" href="#tabWompiBotonBancolombia" data-toggle="tab">
                                                            <img src="../Content/img/bancolombia_button.png" alt="pricing img" class="img-fluid" style="width: 53%;" />
                                                            <span style="font-size: 14px;">Bancolombia</span>
                                                        </a>
                                                    </li>
                                                    <li class="nav-item" style="width: 40%;">
                                                        <a class="nav-link h6" href="#tabWompiTJ" data-toggle="tab" id="DVTCJ" visible="false" runat="server">
                                                            <img src="../Content/img/TC.png" alt="pricing img" class="img-fluid" style="width: 100%;" />
                                                            <span style="font-size: 14px;">Tarjeta Crédito</span>
                                                        </a>
                                                    </li>
                                                     <li class="nav-item" style="width: 25%;" visible="false" id="DVPSEWompi" runat="server">
                                                        <a class="nav-link h6" href="#tabWompiBotonPSE" data-toggle="tab">
                                                            <img src="../Content/img/pse1.png" alt="pricing img" class="img-fluid" style="width: 53%;" />
                                                            <span style="font-size: 14px;">Botón PSE</span>
                                                        </a>
                                                    </li>
                                                </ul>
                                                <div class="tab-content feature-tab-content" id="dvContentsPayment" visible="false" runat="server">
                                                    <div class="tab-pane active" id="tabPSE" runat="server">
                                                        <div class="col-md-12 col-lg-12" style="padding: 0;">
                                                            <div class="card login-signup-card shadow-lg mb-0">
                                                                <div class="card-body px-md-5 py-5" style="padding: 82px 30px !important;">
                                                                    <div class="row justify-content-center">
                                                                        <div class="col-md-12">
                                                                            <div class="section-heading text-center mb-5">
                                                                                <h4 class="text-center mb-1" style="padding-bottom: 10px">Medio de pago selecccionado:<br>
                                                                                    Botón PSE
                                                                                </h4>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="login-signup-form">
                                                                        <div class="row">
                                                                            <div class="col-sm-12 col-12">
                                                                                <div class="form-group">
                                                                                    <div class="form-group">
                                                                                        <label class="pb-1">Tipo Persona</label>
                                                                                        <div class="input-group input-group-merge">
                                                                                            <div class="input-icon">
                                                                                                <span class="ti-user color-primary"></span>
                                                                                            </div>
                                                                                            <asp:DropDownList ID="DllTipoPersona" class="form-control" TabIndex="1" runat="server">
                                                                                                <asp:ListItem Value="0">Natural</asp:ListItem>
                                                                                                <asp:ListItem Value="1">Jurídica</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-sm-12 col-12">
                                                                                <div class="form-group">
                                                                                    <div class="form-group">
                                                                                        <div class="form-group">
                                                                                            <label class="pb-1">Banco</label>
                                                                                            <div class="input-group input-group-merge">
                                                                                                <div class="input-icon">
                                                                                                    <span class="ti-home color-primary"></span>
                                                                                                </div>
                                                                                                <asp:DropDownList class="form-control" onfocus="OcultarDivErrores(ddlbancoError);" ID="ddlBancos" runat="server" TabIndex="7">
                                                                                                </asp:DropDownList>
                                                                                                <div id="ddlbancoError" class="errores ocultar">
                                                                                                    <asp:RequiredFieldValidator ID="RFVddlBancos" ControlToValidate="ddlBancos" CssClass="tooltips" ValidationGroup="ValidacionFormulario" ErrorMessage="Debe seleccionar un banco" InitialValue="0" runat="server" />
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-sm-12 col-md-4 mt-3" style="text-align: end;">
                                                                                <img src="../Content/img/pse1.png" alt="pricing img" class="img-fluid " style="width: 56px !important; padding-right: 0;" />
                                                                            </div>
                                                                           
                                                                           <asp:Button ID="BtnPagarPSE" CssClass="btn google-play-btn col-sm-12 col-md-6 mt-3" OnClientClick="javascript: return  ValidarFormulario('ValidacionFormulario'); "  OnClick="BtnPagarPSE_Click" Style="background: #0058b1; border-color: #0058b1; color: #ffffff !important; border-radius: 30px;" runat="server" Text="Pagar en linea" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="tab-pane " id="tabWompiBotonBancolombia" runat="server">
                                                        <div class="col-md-12 col-lg-12">
                                                            <div class="card login-signup-card shadow-lg mb-0">
                                                                <div class="card-body px-md-5 py-5" style="padding: 132px 0 !important;">
                                                                    <div class="row justify-content-center">
                                                                        <div class="col-md-12">
                                                                            <div class="section-heading text-center mb-5">
                                                                                <h4 class="text-center mb-1">Medio de pago selecccionado:
                                                                                <br>
                                                                                    Botón Bancolombia
                                                                                </h4>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="login-signup-form">
                                                                        <div class="row">
                                                                            <div class="col-sm-6 col-12">
                                                                                <div class="form-group">
                                                                                    <label class="pb-1">Impuesto:</label><br>
                                                                                    <span class="price font-weight-bolder" style="font-size: 20px;">
                                                                                        <asp:Label ID="lblImpuestoBotonBancolombia" runat="server"></asp:Label></span>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-sm-6 col-12">
                                                                                <div class="form-group">
                                                                                    <label class="pb-1">Referencia:</label><br>
                                                                                    <span class="price font-weight-bolder" style="font-size: 20px;"> <asp:Label ID="lblReferenciaBotonBancolombia" runat="server"></asp:Label></span>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-sm-12 col-md-4 mt-3" style="text-align: end;">
                                                                                <img src="../Content/img/bancolombia_button.png" alt="pricing img" class="img-fluid " style="width: 56px !important; padding-right: 0;" />
                                                                            </div>
                                                                            <asp:Label ID="Label3" runat="server"></asp:Label>
                                                                            <asp:Button ID="btnBotonBancolombia" CssClass="btn google-play-btn col-sm-12 col-md-6 mt-3" OnClick="btnBotonBancolombia_Click" Style="background: #0058b1; border-color: #0058b1; color: #ffffff !important; border-radius: 30px;" runat="server" Text="Pagar en linea" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="tab-pane" id="tabWompiTJ" runat="server">
                                                        <div class="col-md-12 col-lg-12">
                                                            <div class="card login-signup-card shadow-lg mb-0">
                                                                <div class="card-body px-md-5 py-5">
                                                                    <div class="row justify-content-center">
                                                                        <div class="col-md-12">
                                                                            <div class="section-heading text-center mb-5">
                                                                                <h4 class="text-center mb-1">Medio de pago selecccionado:<br>
                                                                                    Tarjeta crédito
                                                                                </h4>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="login-signup-form">
                                                                        <div class="row">
                                                                            <div class="col-sm-6 col-12">
                                                                                <div class="form-group">
                                                                                    <label class="pb-1">Titular</label>
                                                                                    <div class="input-group input-group-merge">
                                                                                        <div class="input-icon">
                                                                                            <span class="ti-credit-card color-primary"></span>
                                                                                        </div>
                                                                                        <asp:TextBox ID="txtcard_holder" CssClass="form-control" runat="server"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-sm-6 col-12">
                                                                                <div class="form-group">
                                                                                    <label class="pb-1">Número de tarjeta</label>
                                                                                    <div class="input-group input-group-merge">
                                                                                        <div class="input-icon">
                                                                                            <span class="ti-credit-card color-primary"></span>
                                                                                        </div>
                                                                                        <asp:TextBox ID="txtCardNumber" CssClass="form-control" runat="server" placeholder="xxxx-xxxx-xxxx-xxxx"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-sm-6 col-12">
                                                                                <div class="form-group">
                                                                                    <label class="pb-1">Fecha Vencimiento (MM)</label>
                                                                                    <div class="input-group input-group-merge">
                                                                                        <div class="input-icon">
                                                                                            <span class="ti-credit-card color-primary"></span>
                                                                                        </div>
                                                                                        <asp:DropDownList ID="dllexp_month" CssClass="form-control" runat="server">
                                                                                        </asp:DropDownList>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-sm-6 col-12">
                                                                                <div class="form-group">
                                                                                    <label class="pb-1">Fecha Vencimiento (YYYY)</label>
                                                                                    <div class="input-group input-group-merge">
                                                                                        <div class="input-icon">
                                                                                            <span class="ti-credit-card color-primary"></span>
                                                                                        </div>
                                                                                        <asp:DropDownList ID="dllexp_year" CssClass="form-control" runat="server">
                                                                                        </asp:DropDownList>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-sm-6 col-12">
                                                                                <div class="form-group">
                                                                                    <label class="pb-1">CVV</label>
                                                                                    <div class="input-group input-group-merge">
                                                                                        <div class="input-icon">
                                                                                            <span class="ti-credit-card color-primary"></span>
                                                                                        </div>
                                                                                        <asp:TextBox ID="txtCVV" MaxLength="3" CssClass="form-control" placeholder="•••" TextMode="Password" runat="server"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-sm-6 col-12">
                                                                                <div class="form-group">
                                                                                    <label class="pb-1">Cuotas</label>
                                                                                    <div class="input-group input-group-merge">
                                                                                        <div class="input-icon">
                                                                                            <span class="ti-credit-card color-primary"></span>
                                                                                        </div>
                                                                                        <asp:DropDownList ID="dllinstallments" CssClass="form-control" runat="server">
                                                                                        </asp:DropDownList>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <img src="../Content/img/TC.png" alt="pricing img" class="img-fluid col-sm-12 col-md-6 mt-3" style="width: 28%; padding-right: 0;" />
                                                                            <asp:Button ID="btnPagarCard" CssClass="btn google-play-btn col-sm-12 col-md-6 mt-3" OnClick="BtnPagarCard_Click" Style="background: #0058b1; border-color: #0058b1; color: #ffffff !important; border-radius: 30px;" runat="server" Text="Pagar en linea" />
                                                                            <asp:Label ID="lblSEG" Visible="false"  runat="server"></asp:Label>
                                                                            <asp:Label ID="lblToken" Visible="false" runat="server"></asp:Label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="tab-pane " id="tabWompiBotonPSE" runat="server">
                                                        <div class="col-md-12 col-lg-12" style="padding: 0;">
                                                            <div class="card login-signup-card shadow-lg mb-0">
                                                                <div class="card-body px-md-5 py-5" style="padding: 82px 30px !important;">
                                                                    <div class="row justify-content-center">
                                                                        <div class="col-md-12">
                                                                            <div class="section-heading text-center mb-5">
                                                                                <h4 class="text-center mb-1" style="padding-bottom: 10px">Medio de pago selecccionado:<br>
                                                                                    Botón PSE
                                                                                </h4>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="login-signup-form">
                                                                        <div class="row">
                                                                            <div class="col-sm-12 col-12">
                                                                                <div class="form-group">
                                                                                    <div class="form-group">
                                                                                        <label class="pb-1">Tipo Persona</label>
                                                                                        <div class="input-group input-group-merge">
                                                                                            <div class="input-icon">
                                                                                                <span class="ti-user color-primary"></span>
                                                                                            </div>
                                                                                            <asp:DropDownList ID="dllTipoPersonaWompi" class="form-control" TabIndex="1" runat="server">
                                                                                                <asp:ListItem Value="0">Natural</asp:ListItem>
                                                                                                <asp:ListItem Value="1">Jurídica</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-sm-12 col-12">
                                                                                <div class="form-group">
                                                                                    <div class="form-group">
                                                                                        <div class="form-group">
                                                                                            <label class="pb-1">Banco</label>
                                                                                            <div class="input-group input-group-merge">
                                                                                                <div class="input-icon">
                                                                                                    <span class="ti-home color-primary"></span>
                                                                                                </div>
                                                                                                <asp:DropDownList class="form-control"  ID="dllBancosWompi" runat="server" TabIndex="7">
                                                                                                </asp:DropDownList>
                                                                                                
                                                                                            </div>
                                                                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="dllBancosWompi" ValidationGroup="ValidacionFormularioPSEWompi" ErrorMessage="Debe seleccionar un banco" InitialValue="0" runat="server" />
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-sm-12 col-md-4 mt-3" style="text-align: end;">
                                                                                <img src="../Content/img/pse1.png" alt="pricing img" class="img-fluid " style="width: 56px !important; padding-right: 0;" />
                                                                            </div>
                                                                           
                                                                           <asp:Button ID="btnPSEWompi"  CssClass="btn google-play-btn col-sm-12 col-md-6 mt-3"  OnClick="btnPSEWompi_Click"  Style="background: #0058b1; border-color: #0058b1; color: #ffffff !important; border-radius: 30px;" runat="server" Text="Pagar en linea" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
        <div class="modal modal-static fade" id="processing-modal" role="dialog">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="text-center">
                            <img src="../Content/img/loading.gif" />
                            <h5><span class="modal-text">Transacción en proceso, Espere por favor... </span></h5>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="myModal" role="dialog">
            <div class="modal-dialog modal-md">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title"><span id="spantitulomensaje">[Titulo]</span></h4>
                    </div>
                    <div class="modal-body">
                        <img id="ImagenMensaje" src="" width="30px" height="30px" />
                        <span id="spanmensaje">[Mensaje]</span>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                    </div>
                </div>
            </div>
        </div>
        <footer class="footer-section">
            <div class="footer-bottom gray-light-bg pt-4 pb-4">
                <div class="container">
                    <div class="row text-center justify-content-center">
                        <div class="col-md-12 col-lg-12">
                            <p class="copyright-text pb-0 mb-0">
                                Politicas de Privacidad y Condiciones de Uso , todos los derechos reservados.<br>
                                Para una correcta visualización y navegación en el sitio, se recomienda usar las últimas versiones de los siguientes navegadores: Internet Explorer, Mozilla FireFox , Google Chrome. Si su equipo no cuenta con esta versión, por favor realice la actualización.
                <br>
                                Asesorado, diseñado y desarrollado por:
                                <br>
                                <a href="http://www.1cero1.com/" target="_blank">
                                    <img src="../Content/img/101.png" alt="1Cero1 S.A.S" title="1Cero1 S.A.S">
                                </a>
                                <br>
                                Copyright © 2020 101 S.A.S
                            
                            </p>
                        </div>
                    </div>
                </div>
            </div>
            <!--footer copyright end-->
        </footer>
        <script src="../assetsPayment/js/jquery-3.5.0.min.js"></script>
        <script src="../assetsPayment/js/bootstrap.min.js"></script>
        <script src="../Content/js/Pasarela.js"></script>
          <script src="../Content/js/APIWompi.js"></script>
    </form>
</body>
</html>

