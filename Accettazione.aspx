<%@ Page Title="Facon V2" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Accettazione.aspx.vb" Inherits="FaconV2.Accettazione" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Content/bootstrap.css" rel="stylesheet" />
        <p>
        </p>
    <p>
    <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" Width="1135px" Height="480px" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
    <p>
    </p>
    <p>
        <asp:CheckBox ID="CheckBox1" runat="server" Text="Ho preso visione ed accetto le condizioni" AutoPostBack="True" />
    </p>
    <p>
    </p>
    <p>

        <asp:Button ID="Button2" runat="server" CssClass="btn btn-default" Text="INVIA CODICE OTP" Visible="False" Width="152px" />
    </p>
    <p>

        <asp:TextBox ID="TextBox2" Placeholder="Inserire il codice OTP" CssClass="form-control" onfocus="disableautocompletion(this.id);" runat="server" Visible="False" Width="225px"></asp:TextBox>
    </p>
    <p>

        <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Red" Text="Label" Visible="False"></asp:Label>
    </p>
    <p>

        <asp:Button ID="Button1" runat="server" CssClass="btn btn-default" Text="INVIA" Visible="False" Width="152px" />
    </p>
    <p>

        &nbsp;</p>
    <p>
    </p>

     <script type="text/javascript">
    
    function disableautocompletion(id) {
        var DataControl = document.getElementById(id);
        DataControl.setAttribute("autocomplete", "off");
        
    }
    
     </script>


</asp:Content>
