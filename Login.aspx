<%@ Page Title="Login" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Login.aspx.vb" Inherits="FaconV2.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        <br />
    </p>
    <p>
    </p>

        <div class="form-group" >
       <br />
       <br />
            <asp:Label runat="server" AssociatedControlID="CodiceFornitore" CssClass="col-md-2 control-label">Codice fornitore</asp:Label>
                   <div class="col-md-3">
                    <asp:TextBox ID="CodiceFornitore" runat="server" CssClass="form-control" onfocus="disableautocompletion(this.id);"></asp:TextBox>

                       </div>


  

        <div class="col-md-offset-2 col-md-10">
    <br />
    <asp:Button ID="Button1" runat="server" Text="ABILITA" Width="206px" CssClass="btn btn-default" BackColor="Silver" />

   <br />
                                   <br />
                       <asp:Label ID="Label1" runat="server" Text="Label"  Visible="false"></asp:Label>
            <br />
              <p>
             </p>
              <p>
             </p>
            <asp:HyperLink ID="HyperLink1" runat="server" Visible="False">Link di accesso</asp:HyperLink>
              <p>
             </p>
              <p>
             </p>
              </div>
            <asp:Label ID="LblMittente" runat="server" AssociatedControlID="Mittente" CssClass="col-md-2 control-label" Visible="False">Mittente email</asp:Label>
         <div class="col-md-3">
            <asp:TextBox ID="Mittente" runat="server" CssClass="form-control" onfocus="disableautocompletion(this.id);" Visible="False">rasetti@gransasso.it</asp:TextBox>
             <br />
          </div>
             <br />
            <asp:Label ID="LblDestinatario" runat="server" AssociatedControlID="Destinatario" CssClass="col-md-2 control-label" Visible="False" >Destinatario email</asp:Label>
          <div class="col-md-3">
            <asp:TextBox ID="Destinatario" runat="server" CssClass="form-control" onfocus="disableautocompletion(this.id);" Visible="False">rasetti@gransasso.it</asp:TextBox>
             <br />
             </div>
        <p>
    </p>

    <asp:Button ID="Button2" runat="server" Text="INVIA EMAIL" Width="206px" CssClass="btn btn-default" BackColor="Silver" Visible="False" />
        <p>
    </p>

        <br />
        <br />
        <br />
    </div>

    <script type="text/javascript">
    
    function disableautocompletion(id) {
        var DataControl = document.getElementById(id);
        DataControl.setAttribute("autocomplete", "off");
        
    }
    
    </script>


</asp:Content>
