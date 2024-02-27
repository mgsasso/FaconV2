<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="HelpInterni.aspx.vb" Inherits="FaconV2.HelpInterni" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <br />

    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/PrimoAccessoFacon.pdf" Target="_blank">Primo accesso</asp:HyperLink>
    <br />
    <br />
    <asp:HyperLink ID="HyperLink2" runat="server" Target="_blank" NavigateUrl="AbilitareFaconisti.pdf">Come abilitare i faconisti</asp:HyperLink>

</asp:Content>
