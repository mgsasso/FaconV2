Imports System.ComponentModel.DataAnnotations
Imports System.Data.SqlClient
Imports System.Drawing
Imports NPOI.SS.Formula.Functions
Imports Telerik.Web.UI.ImageEditor

Public Class RientroConfezione
    Inherits System.Web.UI.Page
    Public ContaSelezionati As Integer
    Public dt2 As DataTable
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.QueryString("ID") = "xcvqrybsudaR5676ddsaZWXQUDS" Then
            CreaCookieFaconInterno()
        End If
        If Request.Cookies("CookieFaconInterno") Is Nothing Then
            Response.Redirect("NoAccess.aspx")
        End If

        Dim strSelect As String = ""
        Dim WhereCondition As String = ""


        If Request.QueryString("Tipo") = "Confezione" And Request.QueryString("TipoProdotto") = "1" Then
            'WhereCondition = " and  ((CodiceTipoOperazione = 21) or (CodiceTipoOperazione = 22) or (CodiceTipoOperazione = 25) or (CodiceTipoOperazione = 60)) "
            WhereCondition = " and  ((CodiceTipoOperazione = 21) or (CodiceTipoOperazione = 22) or (CodiceTipoOperazione = 60)) and FlagCaricatoAvanzamento is null and TipoProdotto = 1 " &
                             " and DataBollaCliente > Format(GetDate() - 120,'yyyyMMdd')"
        End If

        If Request.QueryString("Tipo") = "Stiro" And Request.QueryString("TipoProdotto") = "1" Then
            WhereCondition = " and  ((CodiceTipoOperazione = 40) or (CodiceTipoOperazione = 41) or (CodiceTipoOperazione = 45) or (CodiceTipoOperazione = 46)) and FlagCaricatoAvanzamento is null and TipoProdotto = 1 " &
                             " and DataBollaCliente > Format(GetDate() - 60,'yyyyMMdd')"
        End If
        If Request.QueryString("Tipo") = "Confezione" And Request.QueryString("TipoProdotto") = "2" Then
            WhereCondition = " and FlagCaricatoAvanzamento is null and TipoProdotto = 2 and DataBollaCliente > Format(GetDate() - 60,'yyyyMMdd')"
        End If

        strSelect = "Select TblDDTInviati.ID, CodiceFornitore, UPPER(Name) As Name, ArticoloCliente, DescrizioneArticoloCliente, CommessaCliente, " &
        "Format(CAST(LEFT (DataBollaCliente, 4) + '-' + SUBSTRING(CAST(DataBollaCliente AS varchar(8)), 5, 2) + '-' + RIGHT (DataBollaCliente, 2) AS date), 'dd/MM/yyyy') AS DataBollaCliente, " &
        "Format(CAST(LEFT (DataRientro, 4) + '-' + SUBSTRING(CAST(DataRientro AS varchar(8)), 5, 2) + '-' + RIGHT (DataRientro, 2) AS date), 'dd/MM/yyyy') AS DataRientro, " &
        "NumeroBollaCliente, QT, PrezzoUnitario, NoteInterne, FlagInvioTemporaneo, FlagSelezionato, " &
        "TipoProdotto + '-' + cast(CodiceTipoOperazione as varchar(2)) + '-' + Cast(CodiceOperazioneEsterna as varchar(2)) as CodiceOperazione, " &
        "CapiCampionati, CapiConformi, CapiNonConformi, FaseAvanzamentoSuccessiva, FlagRespinta FROM TblDDTInviati " &
        "inner join DBSERVER2.MGS.dbo.[MGS$Vendor] on CodiceFornitore = [No_] collate Latin1_General_100_CI_AS " &
        "WHERE IDDDT Is Not NULL and ((DataRientro Is null) Or (DataRientro > Format(GetDate() - 20,'yyyyMMdd'))) " &
        WhereCondition &
        "ORDER BY case when CapiConformi is null then 99 else FlagRespinta end desc,  DataRientro desc, TblDDTInviati.DataBollaCliente desc"

        If Not Request.QueryString("Tipo") Is Nothing Then
            SqlDataSource1.SelectCommand = strSelect
        End If


    End Sub
    Public Sub CreaCookieFaconInterno()
        Dim aCookie As New HttpCookie("CookieFaconInterno")
        'aCookie.Values("Code") = Session("Code")
        'aCookie.Values("ID") = Session("ID")
        'aCookie.Values("NomeFacon") = Session("NomeFacon")
        aCookie.Values("lastVisit") = DateTime.Now.ToString()
        aCookie.Expires = DateTime.Now.AddDays(1800)
        Response.Cookies.Add(aCookie)
    End Sub
    Public Sub SearchGv()
        Dim constr As String = ConfigurationManager.ConnectionStrings("FaconConnectionString").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand()
                Dim WhereCondition As String = ""
                Dim sql As String = "Select TblDDTInviati.ID, CodiceFornitore, Upper(Name) As Name, ArticoloCliente, DescrizioneArticoloCliente, CommessaCliente, Format(Cast(Left(DataBollaCliente, 4) + '-' + Substring(Cast(DataBollaCliente as varchar(8)),5,2) + '-' + Right(DataBollaCliente,2) as date),'dd/MM/yyyy') as DataBollaCliente, Format(Cast(Left(DataRientro, 4) + '-' + Substring(Cast(DataRientro as varchar(8)),5,2) + '-' + Right(DataRientro,2) as date),'dd/MM/yyyy') as DataRientro, NumeroBollaCliente, QT, PrezzoUnitario, NoteInterne, FlagInvioTemporaneo, TipoProdotto + '-' + cast(CodiceTipoOperazione as varchar(2)) + '-' + Cast(CodiceOperazioneEsterna as varchar(2)) as CodiceOperazione, CapiCampionati, CapiConformi, CapiNonConformi,FaseAvanzamentoSuccessiva  FROM TblDDTInviati inner join DBSERVER2.MGS.dbo.[MGS$Vendor] on CodiceFornitore = [No_] collate Latin1_General_100_CI_AS WHERE IDDDT IS NOT NULL and ((DataRientro is null) or (DataRientro > Format(GetDate() - 20,'yyyyMMdd'))) "
                If Not String.IsNullOrEmpty(SearchTxt.Text.Trim()) Then
                    sql += " and ((CommessaCliente LIKE '" & SearchTxt.Text.Trim() & "%') or (NumeroBollaCliente LIKE '" & SearchTxt.Text.Trim() & "%') or (DescrizioneArticoloCliente LIKE '" & SearchTxt.Text.Trim() & "%') or (ArticoloCliente LIKE '" & SearchTxt.Text.Trim() & "%') or (Name LIKE '%" & SearchTxt.Text.Trim() & "%')) "
                    'cmd.Parameters.AddWithValue("@StringaRicerca", Note.Text.Trim())
                End If
                If Request.QueryString("Tipo") = "Confezione" And Request.QueryString("TipoProdotto") = "1" Then
                    WhereCondition = " and  ((CodiceTipoOperazione = 21) or (CodiceTipoOperazione = 22) or (CodiceTipoOperazione = 60)) and FlagCaricatoAvanzamento is null and TipoProdotto = 1 "
                End If

                If Request.QueryString("Tipo") = "Stiro" And Request.QueryString("TipoProdotto") = "1" Then
                    WhereCondition = " and  ((CodiceTipoOperazione = 40) or (CodiceTipoOperazione = 41) or (CodiceTipoOperazione = 45) or (CodiceTipoOperazione = 46)) and FlagCaricatoAvanzamento is null and TipoProdotto = 1 "
                End If
                If Request.QueryString("Tipo") = "Confezione" And Request.QueryString("TipoProdotto") = "2" Then
                    WhereCondition = " and FlagCaricatoAvanzamento is null and TipoProdotto = 2 "
                End If
                sql += " " & WhereCondition
                sql += " ORDER BY case when CapiConformi is null then 99 else FlagRespinta end desc, DataRientro desc, TblDDTInviati.DataBollaCliente desc"

                If Not Request.QueryString("Tipo") Is Nothing Then
                    SqlDataSource1.SelectCommand = sql
                    GridView1.DataBind()
                    'EvidenziaApprovatiRespinti()
                End If
            End Using
        End Using
    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        Dim constr As String = ConfigurationManager.ConnectionStrings("FaconConnectionString").ConnectionString

        Select Case e.CommandName


            Case "Rientro"
                Dim indice = e.CommandArgument
                Dim bottone As Button = CType(GridView1.Rows(indice).Cells(19).Controls(0), Button)
                'Dim qt = GridView1.Rows(indice).Cells(6).Text
                If bottone.Text = "Conferma rientro" Then
                    GridView1.Rows(indice).BackColor = Color.LightGray
                    bottone.Text = "Annulla rientro"
                    hidden.Value = hidden.Value & "," & GridView1.Rows(indice).Cells(0).Text
                    ContaSelezionati = Selezionati.Text + 1
                    Selezionati.Text = ContaSelezionati
                    Dim TestoRientro As Label = CType(GridView1.Rows(indice).Cells(13).Controls(1), Label)
                    TestoRientro.Text = Data.Text
                    Using con As New SqlConnection(constr)
                        con.Open()
                        Dim DataRientro As Integer = Data.Text.Substring(6, 4) & Data.Text.Substring(3, 2) & Data.Text.Substring(0, 2)
                        Dim update As String = "update TblDDTInviati set DataRientro = " & DataRientro & " where id = " & GridView1.Rows(indice).Cells(0).Text & " "
                        Using cmd1 As New SqlCommand(update)
                            cmd1.Connection = con
                            cmd1.ExecuteNonQuery()
                            'aggiorno la tabella as400 con la data di rientro
                            Dim TipoOperazione = GridView1.Rows(indice).Cells(12).Text.Split("-")
                            Dim DataNsBolla As String = GridView1.Rows(indice).Cells(6).Text.Substring(6, 4) & GridView1.Rows(indice).Cells(6).Text.Substring(3, 2) & GridView1.Rows(indice).Cells(6).Text.Substring(0, 2)
                            Select Case TipoOperazione(1).ToString
                                Case "21", "22", "25", "60"
                                    update = "execute('update applibmgs.teces00f set JMDRME = " & DataRientro & " where JMCFOR = ''" & GridView1.Rows(indice).Cells(1).Text & "'' and JMNRAG || ''/'' || JMNCOM = ''" & GridView1.Rows(indice).Cells(5).Text & "'' and JMNBOL || ''/H'' = ''" & GridView1.Rows(indice).Cells(7).Text & "'' and JMDBOL = " & DataNsBolla & "') at as400sql"
                                Case "40", "41", "45", "46"
                                    update = "execute('update applibmgs.tersp00f set J£DRME = " & DataRientro & " where J£CFOR = ''" & GridView1.Rows(indice).Cells(1).Text & "'' and J£NRAG || ''/'' || J£NCOM = ''" & GridView1.Rows(indice).Cells(5).Text & "'' and J£NBOL || ''/H'' = ''" & GridView1.Rows(indice).Cells(7).Text & "'' and J£DBOL = " & DataNsBolla & "') at as400sql"
                            End Select
                            cmd1.CommandText = update
                            cmd1.ExecuteNonQuery()
                        End Using
                        con.Close()
                    End Using
                Else
                    GridView1.Rows(indice).BackColor = Color.White
                    bottone.Text = "Conferma rientro"
                    hidden.Value = hidden.Value.Replace(GridView1.Rows(indice).Cells(0).Text, "")
                    ContaSelezionati = Selezionati.Text - 1
                    Selezionati.Text = ContaSelezionati
                    Dim TestoRientro As Label = CType(GridView1.Rows(indice).Cells(13).Controls(1), Label)
                    TestoRientro.Text = ""
                    Using con As New SqlConnection(constr)
                        con.Open()
                        Dim update As String = "update TblDDTInviati set DataRientro = null where id = " & GridView1.Rows(indice).Cells(0).Text & " "
                        Using cmd1 As New SqlCommand(update)
                            cmd1.Connection = con
                            cmd1.ExecuteNonQuery()
                            'aggiorno la tabella as400 azzerando la data di rientro
                            Dim TipoOperazione = GridView1.Rows(indice).Cells(12).Text.Split("-")
                            Dim DataNsBolla As String = GridView1.Rows(indice).Cells(6).Text.Substring(6, 4) & GridView1.Rows(indice).Cells(6).Text.Substring(3, 2) & GridView1.Rows(indice).Cells(6).Text.Substring(0, 2)
                            Select Case TipoOperazione(1).ToString
                                Case "21", "22", "25", "60"
                                    update = "execute('update applibmgs.teces00f set JMDRME = 0 where JMCFOR = ''" & GridView1.Rows(indice).Cells(1).Text & "'' and JMNRAG || ''/'' || JMNCOM = ''" & GridView1.Rows(indice).Cells(5).Text & "'' and JMNBOL || ''/H'' = ''" & GridView1.Rows(indice).Cells(7).Text & "'' and JMDBOL = " & DataNsBolla & "') at as400sql"
                                Case "40", "41", "45", "46"
                                    update = "execute('update applibmgs.tersp00f set J£DRME = 0 where J£CFOR = ''" & GridView1.Rows(indice).Cells(1).Text & "'' and J£NRAG || ''/'' || J£NCOM = ''" & GridView1.Rows(indice).Cells(5).Text & "'' and J£NBOL || ''/H'' = ''" & GridView1.Rows(indice).Cells(7).Text & "'' and J£DBOL = " & DataNsBolla & "') at as400sql"
                            End Select
                            cmd1.CommandText = update
                            cmd1.ExecuteNonQuery()
                        End Using
                        con.Close()
                    End Using

                End If
        End Select
        'SearchGv()
        'GridView1.DataBind()
        'EvidenziaApprovatiRespinti()
    End Sub

    Private Sub RientroConfezione_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Data.Text = "" Then
            Data.Text = Format(Date.Now, "dd/MM/yyyy")
        End If


        For Each row As GridViewRow In GridView1.Rows
            If row.RowType = DataControlRowType.DataRow Then
                'Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("CheckBox1"), CheckBox)
                Dim TestoRientro As Label = CType(row.Cells(13).Controls(1), Label)
                If TestoRientro.Text <> "" Then
                    row.BackColor = Color.LightGray
                    Dim bottone As Button = CType(row.Cells(19).Controls(0), Button)
                    bottone.Text = "Annulla rientro"
                    'Else
                    '    row.BackColor = Color.LightCoral
                    '    Dim bottone As Button = CType(row.Cells(15).Controls(0), Button)
                    '    bottone.Text = "Deseleziona"
                End If

            End If
            If Request.QueryString("Tipo") = "Stiro" Then
                Dim bottone1 As Button = CType(row.Cells(18).Controls(0), Button)
                bottone1.Visible = False
            End If

        Next
        'SearchGv()
        EvidenziaApprovatiRespinti()
    End Sub

    Private Sub GridView1_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles GridView1.RowUpdating
        Dim constr As String = ConfigurationManager.ConnectionStrings("FaconConnectionString").ConnectionString
        Dim id = e.Keys(0).ToString
        Dim qt = e.NewValues.Values(0)
        'Dim note = e.NewValues.Values(1)
        Dim ListboxNote = CType(GridView1.Rows(e.RowIndex).Cells(10).Controls(1), ListBox)
        Dim ComboBoxFase = CType(GridView1.Rows(e.RowIndex).Cells(17).Controls(1), DropDownList)
        Dim note As String = ""
        For Each item As ListItem In ListboxNote.Items
            If item.Selected = True Then
                If note = "" Then
                    note = item.ToString
                Else
                    note = note & "," & item.ToString
                End If

            End If

        Next
        Dim FaseSuccessiva As String = ComboBoxFase.SelectedValue
        Dim CapiConformi = e.NewValues(4)
        Dim DataRientro As Label = CType(GridView1.Rows(e.RowIndex).Cells(13).Controls(1), Label)
        Dim RaggruppamentoCommessa As String = CType(GridView1.Rows(e.RowIndex).Cells(5).Text, String)
        Dim Raggruppamento As Integer = RaggruppamentoCommessa.ToString.Substring(0, InStr(RaggruppamentoCommessa.ToString, "/") - 1)

        If note Is Nothing Then
            note = ""
        End If

        If qt > CInt(e.Keys(1).ToString) Then
            Response.Write("<script language=javascript>alert('La quantità inserita deve essere inferiore a quella modificata')</script>")

            Exit Sub
        End If
        If DataRientro.Text = "" Then
            Response.Write("<script language=javascript>alert('Inserire la data di rientro')</script>")

            Exit Sub
        End If



        Using con As SqlConnection = New SqlConnection(constr)
            con.Open()
            If CapiConformi Is Nothing Then
                Using cmd As SqlCommand = New SqlCommand("update [Facon].[dbo].[TblDDTInviati] set QT = @QT, NoteInterne = @NoteInterne, FaseAvanzamentoSuccessiva = @FaseSuccessiva where id = @ID")
                    cmd.Parameters.AddWithValue("@ID ", id)
                    cmd.Parameters.AddWithValue("@QT", qt)
                    cmd.Parameters.AddWithValue("@NoteInterne", note)
                    cmd.Parameters.AddWithValue("@FaseSuccessiva", FaseSuccessiva)
                    cmd.Connection = con
                    cmd.ExecuteNonQuery()
                End Using
            Else

                Using cmd As SqlCommand = New SqlCommand("update [Facon].[dbo].[TblDDTInviati] set QT = @QT, NoteInterne = @NoteInterne, CapiConformi = @CapiConformi, CapiNonConformi = Case when (CapiCampionati - @CapiConformi) >= 0 then CapiCampionati - @CapiConformi when (CapiCampionati - @CapiConformi) < 0 then 0 end, FaseAvanzamentoSuccessiva = @FaseSuccessiva where id = @ID")
                    cmd.Parameters.AddWithValue("@ID ", id)
                    cmd.Parameters.AddWithValue("@QT", qt)
                    cmd.Parameters.AddWithValue("@NoteInterne", note)
                    cmd.Parameters.AddWithValue("@CapiConformi", CapiConformi)
                    cmd.Parameters.AddWithValue("@FaseSuccessiva", FaseSuccessiva)
                    cmd.Connection = con
                    cmd.ExecuteNonQuery()
                End Using
            End If
            If Raggruppamento >= 9000 Then
                Using cmd As SqlCommand = New SqlCommand("update [Facon].[dbo].[TblDDTInviati] set [FlagCaricatoAvanzamento] = 2 where id = @ID")
                    cmd.Parameters.AddWithValue("@ID ", id)
                    cmd.Connection = con
                    cmd.ExecuteNonQuery()
                End Using
            End If
            con.Close()
        End Using

        'AggiornaCapiDaCampionare(id)
        If Not CapiConformi Is Nothing Then
            AggiornaEsito(id)
        End If


        'GridView1.EditIndex = -1
        GridView1.DataBind()
        SearchGv()
        'EvidenziaApprovatiRespinti()

    End Sub
    Public Sub AggiornaCapiDaCampionare(ByVal Indice As Integer)
        Dim constr1 As String = ConfigurationManager.ConnectionStrings("FaconConnectionString").ConnectionString
        Using con As New SqlConnection(constr1)
            con.Open()
            Dim update As String = "update [Facon].[dbo].[TblDDTInviati] Set CapiCampionati = case when QT " &
            "between 0 And 24 then 1 when QT between 25 And 50 then 5 when QT between 51 And 90 then 8 when QT between 91 And 150 then 13 " &
            "when QT between 151 And 280 then 20 when QT between 281 And 500 then 32 when QT between 501 And 1200 then 50 " &
            "when QT between 1201 And 3200 then 80 end where id = " & Indice
            Using cmd As New SqlCommand(update)
                cmd.Connection = con
                cmd.ExecuteNonQuery()
            End Using
            con.Close()
        End Using
        'StringaLavo
    End Sub

    Public Sub AggiornaEsito(ByVal Indice As Integer)
        Dim constr1 As String = ConfigurationManager.ConnectionStrings("FaconConnectionString").ConnectionString
        Using con As New SqlConnection(constr1)
            con.Open()
            'Dim update As String = "  update [Facon].[dbo].[TblDDTInviati] set FlagRespinta = ( " &
            '"Select case when (case when QT between 0 And 24 then 0 when QT between 25 And 50 then 2 when QT between 51 And 90 then 2 when QT between 91 And 150 then 3 " &
            '"when QT between 151 And 280 then 4 when QT between 281 And 500 then 5 when QT between 501 And 1200 then 7 " &
            '"when QT between 1201 And 3200 then 9 end - CapiNonConformi) >= 0 then 0 else 1 end as FlagRespinta from [Facon].[dbo].[TblDDTInviati] where id = " & Indice & ") " &
            '"where id = " & Indice
            Dim update As String = "  update [Facon].[dbo].[TblDDTInviati] set FlagRespinta = ( " &
            "Select case when (CapiConformi - CapiNonConformi) >= 0 then 0 else 1 end as FlagRespinta from [Facon].[dbo].[TblDDTInviati] where id = " & Indice & ") " &
            "where id = " & Indice
            Using cmd As New SqlCommand(update)
                cmd.Connection = con
                cmd.ExecuteNonQuery()
            End Using
            con.Close()
        End Using




    End Sub


    Public Sub EvidenziaApprovatiRespinti()
        'GridView1.DataBind()
        For Each row As GridViewRow In GridView1.Rows
            If row.RowType = DataControlRowType.DataRow Then
                Dim bottone2 As Button = CType(row.Cells(18).Controls(0), Button)
                If bottone2.Text <> "Aggiorna" Then

                    'Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("CheckBox1"), CheckBox)
                    Dim TestoRientro As Label = CType(row.Cells(13).Controls(1), Label)
                    If TestoRientro.Text <> "" Then
                        row.BackColor = Color.LightGray
                        Dim bottone As Button = CType(row.Cells(19).Controls(0), Button)
                        bottone.Text = "Annulla rientro"
                        'Else
                        '    row.BackColor = Color.LightCoral
                        '    Dim bottone As Button = CType(row.Cells(15).Controls(0), Button)
                        '    bottone.Text = "Deseleziona"
                    End If

                    If Request.QueryString("Tipo") = "Stiro" Then
                        Dim bottone1 As Button = CType(row.Cells(18).Controls(0), Button)
                        bottone1.Visible = False
                    End If



                    'Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("CheckBox1"), CheckBox)
                    Dim dtDati As DataTable
                    dtDati = RecuperaDati(row.Cells(0).Text.ToString)
                    Select Case dtDati.Rows(0).Item(0).ToString
                        Case True
                            row.BackColor = Color.LightCoral
                            Dim bottone As Button = CType(row.Cells(19).Controls(0), Button)
                            'bottone.Text = "Respingi"
                            bottone.Visible = False
                            Dim bottone1 As Button = CType(row.Cells(18).Controls(0), Button)
                            bottone1.Text = "Respingi"
                            bottone1.Visible = True
                            bottone1.CommandName = "Respingi"
                        Case False
                            row.BackColor = Color.LightGreen
                            Dim bottone As Button = CType(row.Cells(19).Controls(0), Button)
                            'bottone.Text = "Avanza"
                            bottone.Visible = False
                            Dim bottone1 As Button = CType(row.Cells(18).Controls(0), Button)
                            bottone1.Text = "Avanza"
                            bottone1.Visible = True
                            bottone1.CommandName = "Avanza"

                    End Select
                Else
                    row.BackColor = Color.Orange

                End If



            End If
        Next
        'SearchGv()
        'GridView1.Sort("FlagRespinta, DataBollaCliente", SortDirection.Descending)
    End Sub

    Public Function RecuperaDati(ByVal Id As Integer)
        Dim ds1 As New DataSet
        Dim constr As String = ConfigurationManager.ConnectionStrings("FaconConnectionString").ConnectionString
        Dim objConn As New SqlClient.SqlConnection(constr)
        objConn.Open()
        Dim Strsql As String = ""
        Strsql = "SELECT FlagRespinta FROM [Facon].[dbo].[TblDDTInviati] where ID = " & Id
        Dim dataAdapter As New SqlClient.SqlDataAdapter(Strsql, objConn)
        ds1.EnforceConstraints = False
        dataAdapter.FillSchema(ds1, SchemaType.Source, "File")
        dataAdapter.Fill(ds1, "File")
        objConn.Close()
        dt2 = ds1.Tables(0)
        Return dt2
    End Function

    Private Sub GridView1_PageIndexChanged(sender As Object, e As EventArgs) Handles GridView1.PageIndexChanged


        For Each row As GridViewRow In GridView1.Rows
            If row.RowType = DataControlRowType.DataRow Then
                'Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("CheckBox1"), CheckBox)
                Dim TestoRientro As Label = CType(row.Cells(13).Controls(1), Label)
                If TestoRientro.Text <> "" Then
                    row.BackColor = Color.LightGray
                    Dim bottone As Button = CType(row.Cells(19).Controls(0), Button)
                    bottone.Text = "Annulla rientro"
                    'Else
                    '    row.BackColor = Color.LightCoral
                    '    Dim bottone As Button = CType(row.Cells(15).Controls(0), Button)
                    '    bottone.Text = "Deseleziona"
                End If

            End If
            If Request.QueryString("Tipo") = "Stiro" Then
                Dim bottone1 As Button = CType(row.Cells(18).Controls(0), Button)
                bottone1.Visible = False
            End If

        Next
        'SearchGv()

    End Sub

    Private Sub GridView1_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles GridView1.RowEditing
        'Dim indice = e.NewEditIndex
        ''Dim TipoProdotto = GridView1.Rows(indice).Cells(12).Text.Split("-")(0)
        ''If TipoProdotto = "1" Then
        'SqlDataSource2.SelectCommand = "select * from openquery(as400sql, 'select SUBSTR(T1KEYT,7,2) AS CODICEFASE, SUBSTR(T1KEYT,7,2) || ''-'' || T1XDES as T1XDES from applibmgs.tbord00f " &
        '                                    "where substr(t1keyt,1,6) = ''CODFAM'' AND SUBSTR(T1KEYT,7,2) in (''10'', ''09'', ''56'', ''44'', ''98'',''91'')" &
        '                                    "order by case SUBSTR(T1KEYT,7,2) when ''10'' then 1 when ''09'' then 2 when ''56'' then 3 when ''44'' then 4 when ''98'' then 5 when ''91'' then 6 else 99 end " &
        '                                    "')"
        ''End If
        ''If TipoProdotto = "2" Then
        ''    SqlDataSource2.SelectCommand = "select * from openquery(as400sql, 'select SUBSTR(T1KEYT,7,2) AS CODICEFASE, SUBSTR(T1KEYT,7,2) || ''-'' || T1XDES as T1XDES from applibmgs.tbord00f " &
        ''                                    "where substr(t1keyt,1,6) = ''CODFAC'' " &
        ''                                    "order by case SUBSTR(T1KEYT,7,2) when ''08'' then 1 when ''10'' then 2 when ''99'' then 3 else 4 end " &
        ''                                    "')"
        ''End If

        'Dim ListboxNote = CType(GridView1.Rows(indice).Cells(10).Controls(1), ListBox)
        'Dim ComboBoxFase = CType(GridView1.Rows(indice).Cells(17).Controls(1), DropDownList)
        'Dim note As String = ""
        'For Each item As ListItem In ListboxNote.Items
        '    If item.Selected = True Then
        '        If note = "" Then
        '            note = item.ToString
        '        Else
        '            note = note & "," & item.ToString
        '        End If

        '    End If

        'Next
        SearchGv()

    End Sub
End Class