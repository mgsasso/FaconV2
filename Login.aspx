<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Login.aspx.vb" Inherits="FaconV2.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        <br />
    </p>
    <p>
    </p>
    <p>
        <div class="form-group" >
       <br />
       <br />
            <asp:Label runat="server" AssociatedControlID="CodiceFornitore" CssClass="col-md-2 control-label">Codice fornitore</asp:Label>
                   <div class="col-md-3">
                    <asp:TextBox ID="CodiceFornitore" runat="server" CssClass="form-control" onfocus="disableautocompletion(this.id);"></asp:TextBox>
       </div>
       </div></p>

        <div class="col-md-offset-2 col-md-10">
    <br />
    <asp:Button ID="Button1" runat="server" Text="ABILITA" Width="206px" CssClass="btn btn-default" BackColor="Silver" />
   &nbsp;<br />
        <br />
       
        <br />
        <br />
        <br />
        </div>
</asp:Content>
