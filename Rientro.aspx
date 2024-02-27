<%@ Page Title="Facon V2" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site2.Master" CodeBehind="Rientro.aspx.vb" Inherits="FaconV2.RientroConfezione" MaintainScrollPositionOnPostback="True" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent2" runat="server">

    <link href="Content/bootstrap.css" rel="stylesheet" />
    
     <div >

    <div >

    <br />
    <br />
 <div>
    <asp:HiddenField ID="hidden" runat="server" />
        </div>

        <div class="row">
  <div class="col-md-4">
      <asp:Label runat="server" AssociatedControlID="SearchTxt" ></asp:Label>
     <asp:TextBox ID="SearchTxt" runat="server" Visible="true" placeholder="Ricerca...." CssClass="form-control" OnTextChanged ="SearchGv" AutoPostBack="true" onfocus="disableautocompletion(this.id);" Width="200px" ></asp:TextBox>
      <%--<input name="TextBox3" type="text" placeholder="Ricerca...." Class="form-control" onkeyup="filter2(this, '<%=GridView1.ClientID %>')" style="Width:150px" autocomplete ="off" >--%>
 </div>
 <div class="col-md-4">

     <asp:Label runat="server" AssociatedControlID="Selezionati" Visible="False" >Selezionati</asp:Label>
        <asp:TextBox ID="Selezionati" runat="server"  CssClass="form-control"  onfocus="disableautocompletion(this.id);" Width="50px" ReadOnly="True" Visible="False" >0</asp:TextBox>

 </div>

<div class="col-md-4">

     <asp:Label runat="server" AssociatedControlID="Data" >Data di rientro</asp:Label>
        <asp:TextBox ID="Data" runat="server"  CssClass="form-control" onfocus="disableautocompletion(this.id);" Width="150px"></asp:TextBox>

 </div>
    </div>

    <br />
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-hover table-responsive" AllowPaging="True" DataKeyNames="ID,QT,PrezzoUnitario" EnablePersistedSelection="True" PageSize="50" DataSourceID="SqlDataSource1" OnRowDataBound="EvidenziaApprovatiRespinti" >
             <Columns>

                <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" />
                <asp:BoundField DataField="CodiceFornitore" HeaderText="Fornitore" InsertVisible="False" ReadOnly="True" SortExpression="CodiceFornitore" />
                <asp:BoundField DataField="Name" HeaderText="Nome fornitore" InsertVisible="False" ReadOnly="True" SortExpression="Name">
                <ItemStyle Width="100px" />
                 </asp:BoundField>
                <asp:BoundField DataField="ArticoloCliente" HeaderText="Articolo" InsertVisible="False" ReadOnly="True" SortExpression="ArticoloCliente" />
                <asp:BoundField DataField="DescrizioneArticoloCliente" HeaderText="Descrizione" InsertVisible="False" ReadOnly="True" SortExpression="DescrizioneArticoloCliente">
                 <ItemStyle Width="100px" />
                 </asp:BoundField>
                <asp:BoundField DataField="CommessaCliente" HeaderText="Ragg/Comm" InsertVisible="False" ReadOnly="True" SortExpression="CommessaCliente" />
                <asp:BoundField DataField="DataBollaCliente" HeaderText="Ns data bolla" InsertVisible="False" ReadOnly="True" SortExpression="DataBollaCliente" />
                <asp:BoundField DataField="NumeroBollaCliente" HeaderText="Ns num bolla" InsertVisible="False" ReadOnly="True" SortExpression="NumeroBollaCliente" />
                <%-- <asp:TemplateField HeaderText="Data Bolla" SortExpression="DataBollaCliente">
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("DataBollaCliente") %>' Width="80px" ReadOnly="true"></asp:TextBox>
                     </EditItemTemplate>
                     <ItemTemplate>
                         <asp:Label ID="Label6" runat="server" Text='<%# Bind("DataBollaCliente") %>'></asp:Label>
                     </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Numero Bolla" SortExpression="NumeroBollaCliente">
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox7" runat="server" Text='<%# Bind("NumeroBollaCliente") %>' Width="70px" ReadOnly="true"></asp:TextBox>
                     </EditItemTemplate>
                     <ItemTemplate>
                         <asp:Label ID="Label7" runat="server" Text='<%# Bind("NumeroBollaCliente") %>'></asp:Label>
                     </ItemTemplate>
                 </asp:TemplateField>--%>
                 <asp:TemplateField HeaderText="QT" SortExpression="QT">
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("QT") %>' Width="60px" ></asp:TextBox>
                     </EditItemTemplate>
                     <ItemTemplate>
                         <asp:Label ID="Label2" runat="server" Text='<%# Bind("QT") %>'></asp:Label>
                     </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Prezzo Unitario" SortExpression="PrezzoUnitario" Visible = "false">
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox8" runat="server" Text='<%# Bind("PrezzoUnitario") %>'  Width="60px" ReadOnly="true"></asp:TextBox>
                     </EditItemTemplate>
                     <ItemTemplate>
                         <asp:Label ID="Label8" runat="server" Text='<%# Bind("PrezzoUnitario") %>'></asp:Label>
                     </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Note interne" SortExpression="Note">
                     <EditItemTemplate>
                         <%--<asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("NoteInterne") %>' Width="200px" ></asp:TextBox>--%>
                         <asp:ListBox ID="Difetti" runat="server" Width="280px" CssClass="form-control" DataTextField='<%# Bind("NoteInterne") %>'  SelectionMode="Multiple" Rows="6" Font-Size="Small" >
                            <asp:ListItem Text="" Value=""></asp:ListItem>
                            <asp:ListItem Text="MAGLIETTE COLLO" Value="MAGLIETTE COLLO"></asp:ListItem>
                            <asp:ListItem Text="MAGLIETTE SPALLE" Value="MAGLIETTE SPALLE"></asp:ListItem>
                            <asp:ListItem Text="MAGLIETTE STRISCIA" Value="MAGLIETTE STRISCIA"></asp:ListItem>
                            <asp:ListItem Text="MAGLIETTE MANICA" Value="MAGLIETTE MANICA"></asp:ListItem>
                            <asp:ListItem Text="PUNTE NON CONFORMI" Value="PUNTE NON CONFORMI"></asp:ListItem>
                            <asp:ListItem Text="PUNTI LASCIATI SOTTO LE BRACCIA" Value="PUNTI LASCIATI SOTTO LE BRACCIA"></asp:ListItem>
                            <asp:ListItem Text="PUNTI LASCIATI CHIUSURA FIANCHI" Value="PUNTI LASCIATI CHIUSURA FIANCHI"></asp:ListItem>
                            <asp:ListItem Text="TRITATO SOTTO LE BRACCIA" Value="TRITATO SOTTO LE BRACCIA"></asp:ListItem>
                            <asp:ListItem Text="BALZE NON CONFORME" Value="BALZE NON CONFORME"></asp:ListItem>
                            <asp:ListItem Text="BALZE CHE SBECCANO" Value="BALZE CHE SBECCANO"></asp:ListItem>
                            <asp:ListItem Text="CHIUSURA FIANCHI ALTA" Value="CHIUSURA FIANCHI ALTA"></asp:ListItem>
                            <asp:ListItem Text="ZIP NON CONFORMI" Value="ZIP NON CONFORMI"></asp:ListItem>
                            <asp:ListItem Text="FINISSAGGIO COLLI NON CONF" Value="FINISSAGGIO COLLI NON CONF"></asp:ListItem>
                            <asp:ListItem Text="PUNTI LASCIATI SPALLE/MANICHE" Value="PUNTI LASCIATI SPALLE/MANICHE"></asp:ListItem>
                            <asp:ListItem Text="TRITATO LISTINO DAVANTI" Value="TRITATO LISTINO DAVANTI"></asp:ListItem>
                            <asp:ListItem Text="TRAVETTE NON CONF" Value="TRAVETTE NON CONF"></asp:ListItem>
                            <asp:ListItem Text="TRAVETTE TRITATE" Value="TRAVETTE TRITATE"></asp:ListItem>
                            <asp:ListItem Text="QUADRATINO CHE SPOSTA" Value="QUADRATINO CHE SPOSTA"></asp:ListItem>

                         </asp:ListBox>
                     </EditItemTemplate>
                     <ItemTemplate>
                         <asp:Label ID="Label1" runat="server" Text='<%# Bind("NoteInterne") %>'></asp:Label>
                     </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Parziale" SortExpression="FlagInvioTemporaneo">
                     <ItemTemplate>
                         <asp:Label ID="Label9" runat="server" Text='<%# Bind("FlagInvioTemporaneo") %>'></asp:Label>
                     </ItemTemplate>
                 </asp:TemplateField>
                  <asp:BoundField DataField="CodiceOperazione" HeaderText="Operazione" InsertVisible="False" ReadOnly="True"/>
                 <asp:TemplateField HeaderText="Data rientro" SortExpression="Datarientro">
                     <ItemTemplate>
                         <asp:Label ID="Label10" runat="server" Text='<%# Bind("DataRientro") %>'></asp:Label>
                     </ItemTemplate>
                 </asp:TemplateField>
                  <asp:BoundField DataField="CapiCampionati" HeaderText="Capi da campionare" InsertVisible="False" ReadOnly="True">
                 <ItemStyle Width="100px" />
                 </asp:BoundField>
                 <asp:TemplateField HeaderText="Capi conformi" InsertVisible="False">
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("CapiConformi") %>' Width="50px"></asp:TextBox>
                     </EditItemTemplate>
                     <ItemTemplate>
                         <asp:Label ID="Label3" runat="server" Text='<%# Bind("CapiConformi") %>'></asp:Label>
                     </ItemTemplate>
                     <ItemStyle Width="50px" />
                 </asp:TemplateField>
                 <asp:BoundField DataField="CapiNonConformi" HeaderText="Capi non conformi" InsertVisible="False" ReadOnly="True">
                 <ItemStyle Width="50px" />
                 </asp:BoundField>
                 <asp:TemplateField HeaderText="Fase successiva"  InsertVisible ="False">
                     <EditItemTemplate>
                         <asp:DropDownList ID="DropDownList1" runat="server" Width="70px" CssClass="form-control" DataSourceID="SqlDataSource2" DataTextField="T1XDES" DataValueField="CODICEFASE">
<%--                             <asp:ListItem>9</asp:ListItem>
                             <asp:ListItem>12</asp:ListItem>
                             <asp:ListItem>14</asp:ListItem>--%>
                         </asp:DropDownList>
                     </EditItemTemplate>
                     <ItemTemplate>
                         <asp:Label ID="Label4" runat="server" Text='<%# Bind("FaseAvanzamentoSuccessiva") %>'></asp:Label>
                     </ItemTemplate>
                     <ItemStyle Width="50px" />
                 </asp:TemplateField>
                 <asp:CommandField ButtonType="Button" ShowEditButton="True" CausesValidation="True" >
                 <ControlStyle CssClass="btn btn-default" />
                 </asp:CommandField>
                 <asp:ButtonField ButtonType="Button" CommandName="Rientro" HeaderText="Rientro" Text="Conferma rientro"  >
                 <ControlStyle CssClass="btn btn-default" />
                  </asp:ButtonField>
                 <asp:BoundField DataField="FlagRespinta" HeaderText="" InsertVisible="False" ReadOnly="True" Visible="false" />

            </Columns>
            <PagerStyle BorderStyle="Solid" Font-Size="Large" Font-Strikeout="False" Height="50" cssClass="pagination-ys"/>

        
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:FaconConnectionString %>" UpdateCommand="Update Facon.dbo.TblDDTInviati set QT = 0 where ID = 0">
            </asp:SqlDataSource>
            <br />
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:FaconConnectionString %>" SelectCommand="select * from openquery(as400sql, 'select SUBSTR(T1KEYT,7,2) AS CODICEFASE, SUBSTR(T1KEYT,7,2) || ''-'' || T1XDES as T1XDES from applibmgs.tbord00f where substr(t1keyt,1,6) = ''CODFAM'' AND SUBSTR(T1KEYT,7,2) in (''10'', ''09'', ''56'', ''44'', ''98'',''91'',''99'',''80'',''83'',''82'',''14'',''12'',''48'',''26'',''22'',''13'',''45'') order by case SUBSTR(T1KEYT,7,2) when ''10'' then 1 when ''09'' then 2 when ''56'' then 3 when ''44'' then 4 when ''98'' then 5 when ''91'' then 6 else 99 end ')"></asp:SqlDataSource>
            <br />
   </div>

    <script type="text/javascript" src="https://code.jquery.com/jquery-3.5.1.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.24/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="https://editor.datatables.net/extensions/Editor/js/dataTables.editor.min.js"></script>
<%--    <link type="text/css" rel="stylesheet" href="https://cdn.datatables.net/1.10.24/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" href='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.4.1/css/bootstrap.min.css' media="screen" />--%>

<!-- Bootstrap -->
<!-- Bootstrap DatePicker -->

<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker.css" type="text/css"/>
<script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js" type="text/javascript"></script>

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

         <style>
               .cssPager td
        {
              padding-left: 4px;     
              padding-right: 4px;    
          }
    </style> 
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
           <script type="text/javascript">
               function filter2(phrase, _id) {
                   var words = phrase.value.toLowerCase().split(" ");
                   var table = document.getElementById(_id);
                   var ele;
                   for (var r = 1; r < table.rows.length; r++) {
                       ele = table.rows[r].innerHTML.replace(/<[^>]+>/g, "");
                       var displayStyle = 'none';
                       for (var i = 0; i < words.length; i++) {
                           if (ele.toLowerCase().indexOf(words[i]) >= 0)
                               displayStyle = '';
                           else {
                               displayStyle = 'none';
                               break;
                           }
                       }
                       table.rows[r].style.display = displayStyle;
                   }
               }
           </script>

    </div>

</asp:Content>
