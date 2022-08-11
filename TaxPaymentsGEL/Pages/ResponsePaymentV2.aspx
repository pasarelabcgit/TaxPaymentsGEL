<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResponsePaymentV2.aspx.cs" Inherits="TaxPaymentsGEL.Pages.ResponsePaymentV2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript" id="www-widgetapi-script" src="https://s.ytimg.com/yts/jsbin/www-widgetapi-vflBH_DEu/www-widgetapi.js" async=""></script>
    <script type="text/javascript" id="www-widgetapi-script" src="https://s.ytimg.com/yts/jsbin/www-widgetapi-vflBH_DEu/www-widgetapi.js" async=""></script>
    <script type="text/javascript" id="www-widgetapi-script" src="https://s.ytimg.com/yts/jsbin/www-widgetapi-vflBH_DEu/www-widgetapi.js" async=""></script>
    <script src="https://www.youtube.com/iframe_api?v=3.2.10" id="YTAPI"></script>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <title>Pasarela de Pagos 1CERO1 Software</title>
     <link rel="shortcut icon" type="image/png" href="~/Content/img/favicon.ico" />
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
        .card{height: 500px;}

         .titulos{
            color: #000;
            font-weight: 800;
            font-size: 17px !important;
         }

         .card span{color:#000 !important;}

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" ID="ScriptManager2"></asp:ScriptManager>
        <asp:Label ID="TransationID" Visible="false" runat="server" Text=""></asp:Label>
         <asp:Label ID="LblCodigoMunicipio" runat="server"></asp:Label>
        <header class="header">
        </header>
        <div class="main">
            <section class="hero-section pt-50 background-img" style="background: url('https://www.electronicpaymentsinternational.com/wp-content/uploads/sites/4/2019/11/Top-influencers-in-payments-tech-Q3-2019-770x578.jpg')no-repeat center center / cover">
                <div class="container">
                    <div class="row align-items-center justify-content-center">
                        <div class="col-md-3 col-lg-3">
                            <img src="https://1cero1pay.com/PaymentGateway/content/img/escudo_colombia.png"/>
                        </div>
                        <div class="col-md-5 col-lg-5">
                        </div>
                        <div class="col-md-4 col-lg-4" style="text-align: center;">
                            <b>
                                <p style="color: white;">
                                    IP Equipo:
                                    <asp:Label ID="lblIPp" runat="server"></asp:Label>
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
                    <!--counter start-->
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
                                         <asp:Label ID="LblTickedID" runat="server"></asp:Label></h6>
                                </div>
                            </li>
                            <li class="list-inline-item">
                                <div class="single-counter text-center">
                                    <span>Total:</span>
                                    <h6>
                                        <asp:Label ID="LblTransactionValue" Style="color: black; font-size: 20px;" runat="server"></asp:Label></h6>
                                </div>
                            </li>
                            <li class="list-inline-item">
                                <div class="single-counter text-center">
                                    <span>Estado:</span>
                                    <h6>
                                        <asp:Label ID="LblTransactionState" Style="color: black; font-size: 20px;" runat="server"></asp:Label></h6>
                                </div>
                            </li>
                        </ul>
                    </div>
                    <!--counter end-->
                </div>
            </section>
            <!--hero section end-->
            <section id="pricing" class="package-section ptb-100">
                <div class="container">
                    
                    <div class="row justify-content-center">
                        <div class="col-lg-4 col-md">
                            <div class="card text-center single-pricing-pack">
                                <div class="card-header py-5 border-0 pricing-header">
                                    <div class="h4 text-center mb-0"><span class="price font-weight-bolder" style="font-size: 20PX;">DATOS DE LA TRANSACCIÓN</span></div>
                                </div>
                                <div class="card-body">
                                    <ul class="list-unstyled text-sm mb-4 pricing-feature-list">
                                        <li class="titulos">CUS:</li>
                                        <li>
                                            <asp:Label ID="LbltrazabilityCode" runat="server"></asp:Label></li>
                                        <li class="titulos">Fecha Transacción</li>
                                        <li>
                                            <asp:Label ID="LblSolicitDate" runat="server"></asp:Label></li>
                                        <li class="titulos">Ciclo Transacción</li>
                                        <li>
                                            <asp:Label ID="LblTransactionCycle" runat="server"></asp:Label></li>
                                         <li class="titulos">IVA:</li>
                                        <li>
                                            <asp:Label ID="LblVATAmount" runat="server"></asp:Label></li>
                                         <li class="titulos">Banco:</li>
                                        <li>
                                            <asp:Label ID="lblBanco" runat="server"></asp:Label></li>
                                        <li class="titulos">Fecha Respuesta Banco:</li>
                                        <li>
                                            <asp:Label ID="LblBankProcessingDate" runat="server"></asp:Label></li>
                                    </ul>

                                </div>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md">
                            <div class="card text-center single-pricing-pack">
                                <div class="card-header py-5 border-0 pricing-header">
                                    <div class="h4 text-center mb-0"><span class="price font-weight-bolder" style="font-size: 20PX;">DATOS DEL CLIENTE</span></div>
                                </div>
                                <div class="card-body">
                                    <ul class="list-unstyled text-sm mb-4 pricing-feature-list">
                                        <li class="titulos">Nombre:</li>
                                        <li>
                                            <asp:Label ID="LblNombre" runat="server"></asp:Label></li>
                                        <li class="titulos">Identificación</li>
                                        <li>
                                            <asp:Label ID="LblIdent" runat="server"></asp:Label></li>
                                        <li class="titulos">Email</li>
                                        <li>
                                            <asp:Label ID="LblEmail" runat="server"></asp:Label></li>
                                        <li class="titulos">Teléfono</li>
                                        <li>
                                            <asp:Label ID="LblPhone" runat="server"></asp:Label></li>
                                        <li class="titulos">IP</li>
                                        <li>
                                            <asp:Label ID="LblIP" runat="server"></asp:Label></li>
                                    </ul>

                                </div>
                            </div>
                        </div>

                        <div class="col-lg-4 col-md">
                            <div class="card text-center single-pricing-pack">
                                <div class="card-header py-5 border-0 pricing-header">
                                    <div class="h4 text-center mb-0"><span class="price font-weight-bolder" style="font-size: 20PX;">DATOS DEL MUNICIPIO</span></div>
                                </div>
                                <div class="card-body">
                                    <ul class="list-unstyled text-sm mb-4 pricing-feature-list">
                                        <li class="titulos">Nombre:</li>
                                        <li>
                                            <asp:Label ID="LblNombreMunicipio" runat="server"></asp:Label></li>
                                        <li class="titulos">Nit</li>
                                        <li>
                                            <asp:Label ID="LblNit" runat="server"></asp:Label></li>
                                        <li class="titulos">Email</li>
                                        <li>
                                            <asp:Label ID="lblEmailMunicipio" runat="server"></asp:Label></li>
                                        <li class="titulos">Teléfono</li>
                                        <li>
                                            <asp:Label ID="LblTelefono" runat="server"></asp:Label></li>

                                    </ul>

                                </div>
                            </div>
                        </div>

                    </div>
                    <div class="mt-5 text-center">
                        <div class="download-btn">
                            <asp:Button ID="btnImprimir" CssClass="btn solid-btn mb-3" OnClick="Imprimir_Click" runat="server" Text="Imprimir Comprobante" />
                            <asp:Button ID="btnFinalizar" CssClass="btn solid-btn mb-3" OnClick="btnFin_Click1" runat="server" Text="Finalizar" />
                        </div>

                    </div>
                </div>
            </section>
            <!--our video promo section start-->
            <!--our video promo section end-->


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
                             Asesorado, diseñado y desarrollado por: <br><a href="http://www.1cero1.com/" target="_blank">
                    <img src="../Content/img/101.png" alt="1Cero1 S.A.S" title="1Cero1 S.A.S">
                </a><br>Copyright © 2020 101 S.A.S
                            
                            </p>
                        </div>
                    </div>
                </div>
            </div>
            <!--footer copyright end-->
        </footer>
        <script src="../assetsPayment/js/jquery-3.5.0.min.js"></script>
        <script src="../assetsPayment/js/popper.min.js"></script>
        <script src="../assetsPayment/js/bootstrap.min.js"></script>
        <script src="../assetsPayment/js/jquery.magnific-popup.min.js"></script>
        <script src="../assetsPayment/js/jquery.easing.min.js"></script>
        <script src="../assetsPayment/js/jquery.mb.YTPlayer.min.js"></script>
        <script src="../assetsPayment/js/wow.min.js"></script>
        <script src="../assetsPayment/js/owl.carousel.min.js"></script>
        <script src="../assetsPayment/js/jquery.countdown.min.js"></script>
        <script src="../assetsPayment/js/scripts.js"></script>
        <script src="../Content/js/Pasarela.js"></script>
    </form>
</body>
</html>

