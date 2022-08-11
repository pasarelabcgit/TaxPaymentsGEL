<%@ Page Title="" Language="C#" MasterPageFile="~/SitePayments.Master" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="TaxPaymentsGEL.Pages.Test" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="row">
        <div class="col-xs-12 col-sm-4 col-md-2">
            <img src="../content/img/escudo_colombia.png" alt="Republica de Colombia" class="escudo_colombia" />
            <img src="" runat="server" id="imga" class="escudo" />
        </div>
        <div class="col-xs-12 col-sm-4 col-md-7">
            <asp:Label ID="lblfact" runat="server" Visible="false" Text=""></asp:Label>
            <div class="logo-name" runat="server" id="LblNombreMunicipio"></div>
        </div>
        <div class="col-xs-12 col-sm-4 col-md-3 ip">
            <label>
                IP Equipo:
                <asp:Label ID="lblIP" runat="server" Text=""></asp:Label></label><br />
            <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                    <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Interval="800"></asp:Timer>
                    <label>Fecha:</label>
                    <label>
                        <asp:Label ID="LblFecha" runat="server" Text=""></asp:Label>
                        <asp:Label ID="TransationID" Visible="false" runat="server" Text=""></asp:Label>
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
<fieldset>
        <div class='row'>
            <div class='col-sm-6'>
                <div class='form-group'>
                    <label>* ID</label>
                    <asp:TextBox ID="txtID" runat="server"
                        CssClass="form-control" placeholder="Recuerde ingresar sólo números" MaxLength="12" TabIndex="2"></asp:TextBox>
                    </div>
                  <asp:ImageButton ID="BtnPagarPSE" runat="server" OnClick="BtnPagarPSE_Click" ImageUrl="../Content/img/pse1.png" />
            </div>
            <div class='col-sm-6'>
              <div class='form-group'>
                    <label>* Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"
                        CssClass="form-control"  MaxLength="20" TabIndex="2"></asp:TextBox>
                    </div>
            </div>
        </div>
    </fieldset>
</asp:Content>

