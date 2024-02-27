<%@ Page Title="Facon V2" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ElencoDDT.aspx.vb" Inherits="FaconV2.ElencoDDT" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

        <br />
    <br />
<br />
    <br />
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <asp:GridView ID="GridView1" CssClass="table table-hover table-responsive"  runat="server" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True" PageSize="10" >
    <Columns>
        <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID"/>
        <asp:BoundField DataField="Oggetto" HeaderText="Oggetto"/>
        <asp:BoundField DataField="Numero" HeaderText="Numero" SortExpression="Numero" />
        <asp:BoundField DataField="Data" HeaderText="Data"/>
        <asp:ButtonField Text="Visualizza" commandname="Visualizza" />
        <asp:ButtonField Text="Elimina" commandname="Elimina" />

    </Columns>
              <PagerStyle BorderStyle="Solid" Font-Size="Large" Font-Strikeout="False" Height="50" cssClass="pagination-ys"/>
</asp:GridView>




    <br />
    <br />
        <asp:CheckBox 
         id="CheckBox1" 
         runat="server"
         autopostback="false"
         text="Check per cancellare un documento" />

     <br />
    <br />

       <asp:GridView ID="GridView2" CssClass="table table-hover table-responsive" runat="server" AutoGenerateColumns="false"  AllowSorting="True" AllowPaging="True" PageSize="10">
    <Columns>
        <asp:BoundField DataField="Anno" HeaderText="Anno" />
        <asp:BoundField DataField="Mese" HeaderText="Mese"/>
        <asp:BoundField DataField="capi" HeaderText="capi" />
        <asp:BoundField DataField="Valore" HeaderText="Valore"/>

    </Columns>
           <PagerStyle BorderStyle="Solid" Font-Size="Large" Font-Strikeout="False" Height="50" cssClass="pagination-ys"/>

</asp:GridView>



        <br />
    <asp:Label runat="server" AssociatedControlID="Data" CssClass="col-md-2 control-label">Mese di elaborazione</asp:Label>
    <div class="col-md-3">
        <asp:TextBox ID="Data" runat="server" CssClass="form-control" onfocus="disableautocompletion(this.id);"> </asp:TextBox>
    </div>
    <div class="col-md-3">
    <asp:Button ID="Button4" runat="server" Text="Genera dettaglio lavorazioni (xlsx)" Width="250px" CssClass="btn btn-default"  />
        <br />
    </div>
    <br />
    <br />

        <asp:Label ID="Label1" runat="server" Text="Label" Visible="False" ForeColor="Red"></asp:Label>


     <br />
    <br />

    <script type="text/javascript" src="https://code.jquery.com/jquery-3.5.1.js"></script>
<%--    <script type="text/javascript" src="https://cdn.datatables.net/1.10.24/js/jquery.dataTables.min.js"></script>
    <link type="text/css" rel="stylesheet" href="https://cdn.datatables.net/1.10.24/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" href='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.4.1/css/bootstrap.min.css' media="screen" />--%>


    <!-- Bootstrap -->
<!-- Bootstrap DatePicker -->
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker.css" type="text/css"/>
<script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js" type="text/javascript"></script>

            <script type="text/javascript">
                $(document).ready(function () {
                    $('[id*=GridView1]').DataTable({
                        "order": [[0, "desc"]]
                    }

                    );

                });
                $(function () {
                    $('[id*=Data]').datepicker({
                        changeMonth: true,
                        changeYear: true,
                        format: "mm-yyyy",
                        language: "it",
                        autoclose: true,
                        startView: "year",
                        minView: "year",
                        minViewMode: "months"
                    });
                });
                function disableautocompletion(id) {
                    var DataControl = document.getElementById(id);
                    DataControl.setAttribute("autocomplete", "off");

                }
            </script>


         <style>
             .pagination-ys {
    /*display: inline-block;*/
    padding-left: 0;
    margin: 20px 0;
    border-radius: 4px;
}

    .pagination-ys table > tbody > tr > td {
        display: inline;
    }

        .pagination-ys table > tbody > tr > td > a,
        .pagination-ys table > tbody > tr > td > span {
            position: relative;
            float: left;
            padding: 8px 12px;
            line-height: 1.42857143;
            text-decoration: none;
            color: dimgray;
            background-color: #ffffff;
            border: 1px solid #dddddd;
            margin-left: -1px;
        }

        .pagination-ys table > tbody > tr > td > span {
            position: relative;
            float: left;
            padding: 8px 12px;
            line-height: 1.42857143;
            text-decoration: none;
            margin-left: -1px;
            z-index: 2;
            color: #aea79f;
            background-color: #f5f5f5;
            border-color: #dddddd;
            cursor: default;
        }

        .pagination-ys table > tbody > tr > td:first-child > a,
        .pagination-ys table > tbody > tr > td:first-child > span {
            margin-left: 0;
            border-bottom-left-radius: 4px;
            border-top-left-radius: 4px;
        }

        .pagination-ys table > tbody > tr > td:last-child > a,
        .pagination-ys table > tbody > tr > td:last-child > span {
            border-bottom-right-radius: 4px;
            border-top-right-radius: 4px;
        }

        .pagination-ys table > tbody > tr > td > a:hover,
        .pagination-ys table > tbody > tr > td > span:hover,
        .pagination-ys table > tbody > tr > td > a:focus,
        .pagination-ys table > tbody > tr > td > span:focus {
            color: #97310e;
            background-color: #eeeeee;
            border-color: #dddddd;
        }

         </style>

         <style>
.pagination {
  display: inline-block;
}

.pagination a {
  color: black;
  float: left;
  padding: 8px 16px;
  text-decoration: none;
}

.pagination a.active {
  background-color: gray;
  color: white;
  border-radius: 5px;
}

.pagination a:hover:not(.active) {
  background-color: #ddd;
  border-radius: 5px;
}
</style>

</asp:Content>
