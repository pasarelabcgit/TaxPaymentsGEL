<%@ Page Title="" Language="C#" MasterPageFile="~/SitePayments.Master" AutoEventWireup="true" CodeBehind="DetailTransactions.aspx.cs" Inherits="TaxPaymentsGEL.Pages.DetailTransactions" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../Content/css/cssjquery.css" rel="stylesheet" />
    <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
    <div class="row">
        <div class="col-xs-12 col-sm-4 col-md-2">
            <img src="../Content/img/escudo_colombia.png" alt="Republica de Colombia" class="escudo_colombia" />
            <%-- <img runat="server" id="imga" class="escudo" />--%>
        </div>
        <div class="col-xs-12 col-sm-4 col-md-7">
            <div class="logo-name" runat="server" id="LblNombreMunicipio"></div>
            <asp:Label ID="lblcode" runat="server" Visible="false" Text=""></asp:Label>
            <asp:Label ID="lblruta" runat="server" Visible="false" Text=""></asp:Label>

        </div>
        <div class="col-xs-12 col-sm-4 col-md-3 ip">
            <label>
                IP Equipo:
                <asp:Label ID="lblIP" runat="server" Text=""></asp:Label></label><br />
            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                    <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Interval="800"></asp:Timer>
                    <label>Fecha:</label>
                    <label>
                        <asp:Label ID="LblFecha" runat="server" Text=""></asp:Label>
                    </label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-2 col-md-2">
            <img src="../Content/img/Logo101P.png" class="img-responsive" />
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 titu" runat="server" id="DivIn">
            HISTORIAL DE TRANSACCIONES
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 titu" runat="server" id="color">
            Datos de búsqueda           
        </div>
    </div>
    <fieldset>
        <div class="=col-md-12">
            <div class='col-md-6'>
                <div class='form-group'>
                    <label>* Seleccionar fecha inicial</label>
                    <div class="input-group">
                        <div class="input-group-addon">
                            <i class="fa fa-calendar"></i>
                        </div>
                        <asp:TextBox ID="txtFechaInicio" runat="server" ReadOnly="true"
                            CssClass="form-control" placeholder="Seleccionar fecha inicio."
                            MaxLength="100" onfocus="OcultarDivErrores(txtFechaInicioError);" TabIndex="3"></asp:TextBox>
                        <div id="txtFechaInicioError" class="errores ocultar">
                            <asp:RequiredFieldValidator ID="txtNameRequired" runat="server"
                                ControlToValidate="txtFechaInicio" ErrorMessage="Debe seleccionar fecha inicial"
                                ValidationGroup="ValidacionFormulario"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
            <div class='col-md-6'>
                <div class='form-group'>
                    <label>* Seleccionar fecha final</label>
                    <div class="input-group">
                        <div class="input-group-addon">
                            <i class="fa fa-calendar"></i>
                        </div>
                        <asp:TextBox ID="txtFechaFinal" runat="server" ReadOnly="true"
                            CssClass="form-control" placeholder="Seleccionar fecha final."
                            MaxLength="100" onfocus="OcultarDivErrores(txtFechaFinalError);" TabIndex="3"></asp:TextBox>
                        <div id="txtFechaFinalError" class="errores ocultar">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                ControlToValidate="txtFechaFinal" ErrorMessage="Debe seleccionar fecha final"
                                ValidationGroup="ValidacionFormulario"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="=col-md-12">
            <div class='col-md-6'>
                <div class='form-group'>
                    <label>* Número de Documento</label>
                    <asp:TextBox ID="TxtNumeroDocumento" runat="server"
                        CssClass="form-control" placeholder="Recuerde ingresar sólo números" MaxLength="20"
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
                            ControlToValidate="TxtNumeroDocumento" ErrorMessage="Mínimo 7 caracteres" ValidationExpression=".{7}.*" />
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class='form-group'>
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btnConsulta" ValidationGroup="ValidacionFormulario" OnClientClick="ValidarFormulario('ValidacionFormulario');" OnClick="btnBuscar_Click" />
                </div>
            </div>
        </div>
    </fieldset>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 titu" runat="server" id="color1">
            Detalle de transacciones           
        </div>
    </div>
    <fieldset style="margin-right: 20px; margin-left: 20px;">
        <div class="row">
            <div class="=col-md-12">
                <asp:GridView ID="GridTransac" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover" runat="server">
                    <Columns>
                        <asp:BoundField DataField="str_NombreTramite" HeaderText="Nombre del tramite" SortExpression="str_NombreTramite" ReadOnly="True" />
                        <asp:BoundField DataField="dtm_FechaTransaccion" HeaderText="Fecha Transacción" SortExpression="dtm_FechaTransaccion" ReadOnly="True" />
                        <asp:BoundField DataField="str_NumeroFactura" HeaderText="Número Factura" SortExpression="str_NumeroFactura" ReadOnly="True" />
                        <asp:BoundField DataField="str_NumeroTransacionPSE" HeaderText="Numero Transación/CUS" SortExpression="str_NumeroTransacionPSE" />
                        <asp:BoundField DataField="dcm_ValorPagar" HeaderText="Valor Pagar" SortExpression="dcm_ValorPagar" />
                        <asp:BoundField DataField="Estado" HeaderText="Estado Transacción" SortExpression="Estado" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <div class="col-xs-12 col-sm-4 col-md-3 ip">
            <label>Para cancelar la transacción de clic </label>
            <asp:LinkButton ID="LinkButton2" Text="Aquí" runat="server" OnClick="LinkButton2_Click" /><br />
        </div>
    </fieldset>

    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.3/jquery.min.js"></script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.11.3/jquery-ui.min.js"></script>
    <script src="js/jquery.dataTables.min.js"></script>
    <script src="js/Lobibox.js"></script>
    <script src="js/bootstrap.js"></script>
    <script src="js/Pasarela.js"></script>
    <script>
        $(document).ready(function () {
            $.datepicker.regional['es'] = {
                closeText: 'Cerrar',
                prevText: '< Ant',
                nextText: 'Sig >',
                currentText: 'Hoy',
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
                dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
                weekHeader: 'Sm',
                dateFormat: 'dd/mm/yy',
                firstDay: 1,
                isRTL: false,
                showMonthAfterYear: false,
                yearSuffix: ''
            };
            $.datepicker.setDefaults($.datepicker.regional['es']);
            $('#<%=txtFechaInicio.ClientID%>').datepicker({
                changeMonth: true,
                changeYear: true,
                maxDate: "-0D",
                onClose: function (selectedDate) {
                    $('#<%=txtFechaFinal.ClientID%>').datepicker("option", "minDate", selectedDate);
                }
            });
            $('#<%=txtFechaFinal.ClientID%>').datepicker({
                changeMonth: true,
                changeYear: true,
                maxDate: "-0D",
            });
        });
        $(document).ready(function () {
            var table = $('#' + '<%= GridTransac.ClientID %>').DataTable({
                'columnDefs': [{
                    'targets': 0,
                    'searchable': false,
                    'orderable': false,
                    'className': 'dt-body-center',
                }],
                'order': [1, 'asc']
            });

            $('#select-all').on('click', function () {
                var rows = table.rows({ 'search': 'applied' }).nodes();
                $('input[type="checkbox"]', rows).prop('checked', this.checked);
            });

            $('#example tbody').on('change', 'input[type="checkbox"]', function () {
                if (!this.checked) {
                    var el = $('#select-all').get(0);
                    if (el && el.checked && ('indeterminate' in el)) {
                        el.indeterminate = true;
                    }
                }
            });
        });

    </script>
</asp:Content>

