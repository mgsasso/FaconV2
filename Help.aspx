<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Help.aspx.vb" Inherits="FaconV2.Help" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p>
    </p>
<%--    <div>
    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/primo_accesso_esterni.mp4" Target="_blank">Video primo accesso</asp:HyperLink>
    </div>--%>
        <div>
    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/ComeCaricareUnaBolla.pdf" Target="_blank">Come caricare una bolla</asp:HyperLink>
    </div>
        <div>
    <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/ComeTrovareUnaBollaSpecifica.pdf" Target="_blank">Come trovare una bolla specifica</asp:HyperLink>
    </div>
        <div>
    <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/ComeCancellareUnaBollaCaricata.pdf" Target="_blank">Come cancellare una bolla caricata</asp:HyperLink>
    </div>
        <div>
    <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="~/GenerareDettaglioLavorazioniMensile.pdf" Target="_blank">Generare dettaglio mensile delle lavorazioni</asp:HyperLink>
    </div>

    <br />
    

</asp:Content>
