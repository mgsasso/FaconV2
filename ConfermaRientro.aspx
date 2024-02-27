<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ConfermaRientro.aspx.vb" Inherits="FaconV2.ConfermaRientro" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


        <p>
        <br />
    </p>
    <p>
    </p>
    <link href="Content/bootstrap.css" rel="stylesheet" />

        <div class="form-group" >

        <br />
        <br />
        <asp:Label runat="server" AssociatedControlID="Data" CssClass="col-md-2 control-label">Data rientro</asp:Label>
        <div class="col-md-3">
            <asp:TextBox ID="Data" runat="server" CssClass="form-control"  onfocus="disableautocompletion(this.id);"> </asp:TextBox>
        </div>
   </div>
       <br />

  

        <div class="col-md-offset-2 col-md-10">
    <br />
    <asp:Button ID="Button1" runat="server" Text="CONFERMA RIENTRO" Width="206px" CssClass="btn btn-default" BackColor="Silver" />
        </div>
   <br />
     <br />
     <br />

    <script type="text/javascript">
    $(function () {
        $('[id*=Data]').datepicker({
            changeMonth: true,
            changeYear: true,
            format: "dd/mm/yyyy",
            language: "it-IT",
            autoclose: true,
            weekStart: 1,
            todayHighlight: true,
            startDate: '-60d'
            
        });
    });
    function disableautocompletion(id) {
        var DataControl = document.getElementById(id);
        DataControl.setAttribute("autocomplete", "off");
        
    }
    
    </script>
</asp:Content>
