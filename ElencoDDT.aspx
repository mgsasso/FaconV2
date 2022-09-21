<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ElencoDDT.aspx.vb" Inherits="FaconV2.ElencoDDT" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

        <br />
    <br />
<br />
    <br />
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <asp:GridView ID="GridView1" CssClass="table table-hover"  runat="server" AutoGenerateColumns="False" >
    <Columns>
        <asp:BoundField DataField="ID" HeaderText="ID" />
        <asp:BoundField DataField="Oggetto" HeaderText="Oggetto"/>
        <asp:BoundField DataField="Numero" HeaderText="Numero" />
        <asp:BoundField DataField="Data" HeaderText="Data"/>
        <asp:ButtonField Text="Visualizza" commandname="Visualizza" />
        <asp:ButtonField Text="Elimina" commandname="Elimina" />

    </Columns>
</asp:GridView>

    <br />
    <br />
        <asp:CheckBox 
         id="CheckBox1" 
         runat="server"
         autopostback="false"
         text="Check per cancellare" />

    <script type="text/javascript" src="https://code.jquery.com/jquery-3.5.1.js"></script>
<%--    <script type="text/javascript" src="https://cdn.datatables.net/1.10.24/js/jquery.dataTables.min.js"></script>
    <link type="text/css" rel="stylesheet" href="https://cdn.datatables.net/1.10.24/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" href='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.4.1/css/bootstrap.min.css' media="screen" />--%>

            <script type="text/javascript">
                $(document).ready(function () {
                    $('[id*=GridView1]').DataTable({
                        "order": [[0, "desc"]]
                    }

                    );

                });
            </script>

</asp:Content>
