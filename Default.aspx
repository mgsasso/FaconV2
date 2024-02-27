<%@ Page Title="Facon V2" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="FaconV2._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <%--    <div class="jumbotron">
        <h1>&nbsp;</h1>
    </div>--%>
    <link href="Content/bootstrap.css" rel="stylesheet" />


<%--    <div class="row">
        <div class="col-md-4">
            <h2>&nbsp;</h2>
        </div>
        <div class="col-md-4">
            <p>
                &nbsp;</p>
        </div>

    </div>--%>
    <div class="form-group">
        <br />
        <br />
        <asp:Label runat="server" AssociatedControlID="NumBolla" CssClass="col-md-2 control-label">Numero bolla</asp:Label>
        <div class="col-md-3">
            <asp:TextBox ID="NumBolla" runat="server" CssClass="form-control" onfocus="disableautocompletion(this.id);"> </asp:TextBox>
        </div>
   </div>
    <div class="form-group">
        <br />
        <br />
        <asp:Label runat="server" AssociatedControlID="Data" CssClass="col-md-2 control-label">Data bolla</asp:Label>
        <div class="col-md-3">
            <asp:TextBox ID="Data" runat="server" CssClass="form-control"  onfocus="disableautocompletion(this.id);"> </asp:TextBox>
        </div>
   </div>
    <div class="form-group" >
       <br />
       <br />
           <asp:Label runat="server" AssociatedControlID="OraConsegna" CssClass="col-md-2 control-label">Ora</asp:Label>
           <div class="col-md-3">
           <asp:TextBox ID="OraConsegna" runat="server" CssClass="form-control" Width="100px" onfocus="disableautocompletion(this.id);"></asp:TextBox>
       </div>
       </div>
    <div class="form-group">
<%--    <div class="row">--%>
        <br />
       <br />
       <asp:Label runat="server" AssociatedControlID="AspettoBeni" CssClass="col-md-2 control-label">Aspetto dei beni</asp:Label>
       <div class="col-md-3">
           <asp:DropDownList ID="AspettoBeni" runat="server" CssClass="form-control" onfocus="disableautocompletion(this.id);" Width="190px">
               <asp:ListItem>carrelli e scatoloni</asp:ListItem>
               <asp:ListItem>carrelli</asp:ListItem>
               <asp:ListItem>scatoloni</asp:ListItem>
           </asp:DropDownList>
       </div>
       <asp:Label runat="server" AssociatedControlID="AspettoBeni" CssClass="col-md-2 control-label">Trasporto a cura</asp:Label>
       <div class="col-md-3">
           <asp:DropDownList ID="TrasportoACura" runat="server" CssClass="form-control" onfocus="disableautocompletion(this.id);">
               <asp:ListItem>destinatario</asp:ListItem>
               <asp:ListItem>mittente</asp:ListItem>
           </asp:DropDownList>
       </div>
 <%--  </div>--%>
  </div>
    <div class="form-group">
        <br />
       <br />
       <asp:Label runat="server" AssociatedControlID="NumeroColli" CssClass="col-md-2 control-label">Numero dei colli</asp:Label>
       <div class="col-md-3">
           <asp:TextBox ID="NumeroColli" runat="server" CssClass="form-control" Width="50px" onfocus="disableautocompletion(this.id);"> </asp:TextBox>
       </div>
   </div>


    <br />
    <br />
    <asp:Label runat="server" AssociatedControlID="Note" CssClass="col-md-2 control-label" >Note</asp:Label>
    <div class="col-md-3">
        <asp:TextBox ID="Note" runat="server" CssClass="form-control" AutoPostBack="true" onfocus="disableautocompletion(this.id);" Width="800px" ></asp:TextBox>
        <br />
    </div>


     <div>

    <br />
    <br />
    <br />
    <asp:Button ID="Button1" runat="server" Text="STAMPA BOLLA" Width="206px" CssClass="btn btn-default" BackColor="Silver"  AutoPostBack="true"/><br OnClientClick="aspnetForm.target ='_blank';"/>
        <br />
        <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="Label" Visible="False"></asp:Label>
        
        <br />

        </div>
     <br />
    <div>
    <asp:HiddenField ID="hidden" runat="server" />
        </div>
<div class="row">
  <div class="col-lg-6">
    <%--<div class="col-md-3">--%>
<%--     <asp:Label runat="server" AssociatedControlID="SearchTxt" Class="col-md-2 control-label">Ricerca</asp:Label>--%>
        <asp:TextBox ID="SearchTxt" runat="server"  Visible="False" placeholder="Ricerca...." CssClass="form-control" onkeyup="filter2(this, '<%=GridView1.ClientID %>')" OnTextChanged="SearchGv" AutoPostBack="true" onfocus="disableautocompletion(this.id);" Width="214px"  ></asp:TextBox>
        <input name="TextBox3" type="text" placeholder="Ricerca...." Class="form-control" onkeyup="filter2(this, '<%=GridView1.ClientID %>')" style="Width:150px" autocomplete ="off" >
        <asp:HyperLink ID="HplUrlPdf" runat="server" Visible="False" Target="_blank">HyperLink</asp:HyperLink>
 </div>

   <div class="col-lg-4">
 <%--   <div class="col-md-3">--%>
     <asp:Label runat="server" AssociatedControlID="Selezionati" Class="col-md-4 control-label">Selezionati</asp:Label>
        <asp:TextBox ID="Selezionati" runat="server"  CssClass="form-control" onfocus="disableautocompletion(this.id);" Width="50px" ReadOnly="True" >0</asp:TextBox>
<%--    </div>--%>
 </div>


 </div>

     <br />
     <br />
     <br />
    <div class="row">
     <div class="col-lg-6">
       <%--<asp:Label ID="LblFlagDaRientrare" runat="server" Text="Visualizza solo raggruppamenti da rientrare" Font-Bold="True"></asp:Label>--%>
      <div class="btn-group" role="group" aria-label="...">
      <asp:Button ID="BtnOn" runat="server" Class= "btn btn-success"  Text="da bollettare" width="100"/>
      <asp:Button ID="BtnOff" runat="server" Class= "btn btn-default"  Text="bollettati" width="100"/>
      </div>
      </div>
    </div>



     <div >

    <div >
    <br />
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Class="table table-hover table-responsive"  AllowPaging="False" DataKeyNames="ID,QT,PrezzoUnitario" EnablePersistedSelection="True" PageSize="10" DataSourceID="SqlDataSource1" >
             <Columns>

                <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" />
                <asp:BoundField DataField="ArticoloCliente" HeaderText="Articolo" InsertVisible="False" ReadOnly="True" SortExpression="ArticoloCliente" />
                <asp:BoundField DataField="DescrizioneArticoloCliente" HeaderText="Descrizione" InsertVisible="False" ReadOnly="True" SortExpression="DescrizioneArticoloCliente" />
                <asp:BoundField DataField="CommessaCliente" HeaderText="Commessa" InsertVisible="False" ReadOnly="True" SortExpression="CommessaCliente" />
 <%--                <asp:TemplateField HeaderText="ArticoloCliente" SortExpression="ArticoloCliente">
          
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("ArticoloCliente") %>' Width="150px" ReadOnly="true"></asp:TextBox>
                     </EditItemTemplate>
                     <ItemTemplate>
                         <asp:Label ID="Label3" runat="server" Text='<%# Bind("ArticoloCliente") %>'></asp:Label>
                     </ItemTemplate>
                 </asp:TemplateField>--%>
<%--                 <asp:TemplateField HeaderText="DescrizioneArticoloCliente" SortExpression="DescrizioneArticoloCliente">
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("DescrizioneArticoloCliente") %>' Width="200px" ReadOnly="true"></asp:TextBox>
                     </EditItemTemplate>
                     <ItemTemplate>
                         <asp:Label ID="Label4" runat="server" Text='<%# Bind("DescrizioneArticoloCliente") %>'></asp:Label>
                     </ItemTemplate>
                 </asp:TemplateField>--%>
<%--                 <asp:TemplateField HeaderText="CommessaCliente" SortExpression="CommessaCliente">
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("CommessaCliente") %>' Width="50px" ReadOnly="true"></asp:TextBox>
                     </EditItemTemplate>
                     <ItemTemplate>
                         <asp:Label ID="Label5" runat="server" Text='<%# Bind("CommessaCliente") %>'></asp:Label>
                     </ItemTemplate>
                 </asp:TemplateField>--%>
                 <asp:TemplateField HeaderText="Data Bolla" SortExpression="DataBollaCliente">
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
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="QT" SortExpression="QT">
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("QT") %>' Width="60px" ></asp:TextBox>
                     </EditItemTemplate>
                     <ItemTemplate>
                         <asp:Label ID="Label2" runat="server" Text='<%# Bind("QT") %>'></asp:Label>
                     </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Prezzo Unitario" SortExpression="PrezzoUnitario">
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox8" runat="server" Text='<%# Bind("PrezzoUnitario") %>'  Width="60px" ReadOnly="true"></asp:TextBox>
                     </EditItemTemplate>
                     <ItemTemplate>
                         <asp:Label ID="Label8" runat="server" Text='<%# Bind("PrezzoUnitario") %>'></asp:Label>
                     </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Note" SortExpression="Note">
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Note") %>' Width="200px" ></asp:TextBox>
                     </EditItemTemplate>
                     <ItemTemplate>
                         <asp:Label ID="Label1" runat="server" Text='<%# Bind("Note") %>'></asp:Label>
                     </ItemTemplate>
                 </asp:TemplateField>
<%--                 <asp:TemplateField HeaderText="Parziale" SortExpression="FlagInvioTemporaneo">
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox9" runat="server" Text='<%# Bind("FlagInvioTemporaneo") %>' Width="20px"></asp:TextBox>
                     </EditItemTemplate>
                     <ItemTemplate>
                         <asp:Label ID="Label9" runat="server" Text='<%# Bind("FlagInvioTemporaneo") %>'></asp:Label>
                     </ItemTemplate>
                 </asp:TemplateField>--%>
                 <asp:BoundField DataField="FlagInvioTemporaneo" HeaderText="Parziale" InsertVisible="False" ReadOnly="True"/>
                  <asp:BoundField DataField="CodiceOperazione" HeaderText="Operazione" InsertVisible="False" ReadOnly="True"/>
                 <asp:CommandField ButtonType="Button" ShowEditButton="True" CausesValidation="False" ControlStyle-Width="80">
                 <ControlStyle CssClass="btn btn-default" />
                 </asp:CommandField>
                 <asp:BoundField DataField="FlagOrdinamento" HeaderText="FlagOrdinamento" SortExpression="FlagOrdinamento" Visible="False" />
                 <asp:ButtonField ButtonType="Button" CommandName="Lavorato" HeaderText="Lavorato" Text="Seleziona" ControlStyle-Width="80">
                 <ControlStyle CssClass="btn btn-default" />
                 </asp:ButtonField>
                 <asp:ButtonField ButtonType="Button" HeaderText="Non lavorato" Text="Seleziona" CommandName="NonLavorato" ControlStyle-Width="80">
                 <ControlStyle CssClass="btn btn-default" />
                 </asp:ButtonField>
                 <asp:ButtonField CommandName="Visualizza" HeaderText="Doc lavorazione" Text="Visualizza" /> 
                 <asp:ButtonField ButtonType="Button" Visible="false" CommandName ="ModificaParzialiTotali" HeaderText="Inverti parz/comp" Text="Inverti" ControlStyle-Width="80" > <ControlStyle CssClass="btn btn-primary" /> </asp:ButtonField>
                 <asp:ButtonField ButtonType="Button" Visible="false" CommandName ="EliminaRiga" HeaderText="Elimina" Text="Elimina" ControlStyle-Width="80" > <ControlStyle CssClass="btn btn-primary" /> </asp:ButtonField>

             </Columns>

                <PagerStyle BorderStyle="Solid" Font-Size="Large" Font-Strikeout="False" Height="50" cssClass="pagination-ys"/>
 

        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:FaconConnectionString %>" SelectCommand="SELECT ID, ArticoloCliente, DescrizioneArticoloCliente, CommessaCliente, Format(CAST(LEFT (DataBollaCliente, 4) + '-' + SUBSTRING(CAST(DataBollaCliente AS varchar(8)), 5, 2) + '-' + RIGHT (DataBollaCliente, 2) AS date), 'dd/MM/yyyy') AS DataBollaCliente, NumeroBollaCliente, QT, PrezzoUnitario, Note, FlagInvioTemporaneo, FlagSelezionato, TipoProdotto + '-' + cast(CodiceTipoOperazione as varchar(2)) + '-' + Cast(CodiceOperazioneEsterna as varchar(2)) as CodiceOperazione FROM TblDDTInviati WHERE (IDDDT IS NULL) AND (CodiceFornitore = @CODICEFORNITORE) ORDER BY FlagSelezionato DESC, TblDDTInviati .DataBollaCliente desc, ID DESC" UpdateCommand="Update Facon.dbo.TblDDTInviati set QT = 0 where ID = 0">
            <SelectParameters>
                <asp:SessionParameter Name="CODICEFORNITORE" SessionField="Code" />
            </SelectParameters>
            </asp:SqlDataSource>
            <br />
            <asp:Timer ID="Timer1" runat="server" Enabled="False" Interval="10000">
        </asp:Timer>
            <br />

   </div>

    <script type="text/javascript" src="https://code.jquery.com/jquery-3.5.1.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.24/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="https://editor.datatables.net/extensions/Editor/js/dataTables.editor.min.js"></script>
<%--    <link type="text/css" rel="stylesheet" href="https://cdn.datatables.net/1.10.24/css/jquery.dataTables.min.css" />--%>
<%--    <link rel="stylesheet" href='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.4.1/css/bootstrap.min.css' media="screen" />--%>

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

        <script type="text/javascript">
            $(document).ready(function () {
            //    //$('[id*=GridView1]').DataTable();
            //    //var editor;
            //    var table = $('[id*=GridView1]').DataTable({
            //        "order": [[0, "desc"]],
            //        "lengthMenu": [[25, 50, -1], [25, 50, "All"]],
            //        "language": {
            //            url: "/dataTables/it.json",
            //        },

            //    });
                $('[id*=GridView1] tbody').on('click', 'tr', function () {
                    $(this).toggleClass('selected');
                    var ids = $.map(table.rows('.selected').data(), function (item) {
                        return item[0]
                    });
                    //console.log(ids);
                    $("#<%=hidden.ClientID %>").val(ids);

            //alert(table.rows('.selected').data().length + ' row(s) selected');
        });


    });
          
        </script>

         
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
