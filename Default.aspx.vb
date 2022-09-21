Imports System.Data.SqlClient
Imports System.Drawing
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.xmp.impl
Imports Org.BouncyCastle.Utilities
Imports Telerik.Web.Apoc.Layout

Public Class _Default
    Inherits Page
    Public dt1 As DataTable
    Public dt2 As DataTable
    Public ContaSelezionati As Integer
    Public Url As String


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim constr As String = ConfigurationManager.ConnectionStrings("NavConnectionString").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand("SELECT [E-Mail], [VAT Registration No_], [No_] FROM [MGS].[dbo].[MGS$Vendor] where No_ = '" & Request.QueryString("Code") & "'", con)
                Using sda As New SqlDataAdapter(cmd)
                    cmd.CommandType = CommandType.Text
                    Dim dt As New DataTable()
                    sda.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        Dim CodiceAuth As Long = CInt(dt.Rows(0).Item(1).ToString) + CInt(dt.Rows(0).Item(2).ToString)
                        CodiceAuth = CodiceAuth * 1952
                        CodiceAuth = CodiceAuth.ToString.Substring(0, 9)
                        If Request.QueryString("ID") = CodiceAuth Then
                            CreaCookieMaster()
                        End If
                        If Request.Cookies("CookieFacon") Is Nothing Then
                            Response.Redirect("NoAccess.aspx")
                        Else
                            Dim aCookie As HttpCookie
                            aCookie = Request.Cookies("CookieFacon")
                            aCookie.Values("Code") = Request.QueryString("Code")
                            Session("Code") = Request.Cookies("CookieFacon").Values("Code").ToString
                            Url = "http://local:44329/default?ID=" & Request.QueryString("ID") & "&Code=" & Request.QueryString("Code")
                        End If
                    Else
                        Response.Redirect("NoAccess.aspx")
                    End If
                End Using
            End Using
        End Using
    End Sub
    Public Sub CreaCookieMaster()
        Dim aCookie As New HttpCookie("CookieFacon")
        aCookie.Values("Code") = Request.QueryString("Code")
        aCookie.Values("lastVisit") = DateTime.Now.ToString()
        aCookie.Expires = DateTime.Now.AddDays(720)
        Response.Cookies.Add(aCookie)
    End Sub

    Private Sub _Default_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        'OraConsegna.Text = ""
        'If Not Me.IsPostBack Then
        '    Dim constr As String = ConfigurationManager.ConnectionStrings("FaconConnectionString").ConnectionString
        '    Using con As New SqlConnection(constr)
        '        Using cmd As New SqlCommand("Select ID, ArticoloCliente, DescrizioneArticoloCliente, CommessaCliente, Format(Cast(Left(DataBollaCliente, 4) + '-' + Substring(Cast(DataBollaCliente as varchar(8)),5,2) + '-' + Right(DataBollaCliente,2) as date),'dd/MM/yyyy') as DataBollaCliente, NumeroBollaCliente, QT, PrezzoUnitario, Note, FlagInvioTemporaneo FROM TblDDTInviati WHERE (IDDDT IS NULL) and CodiceFornitore = '" & Session("Code") & "' order by id desc", con)
        '            Using sda As New SqlDataAdapter(cmd)
        '                cmd.CommandType = CommandType.Text
        '                Dim dt As New DataTable()
        '                sda.Fill(dt)
        '                If dt.Rows.Count > 0 Then
        '                    GridView1.DataSource = dt
        '                    GridView1.DataBind()
        '                    'necessario per far funzionare jquery
        '                    'GridView1.UseAccessibleHeader = True
        '                    'GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
        '                End If
        '            End Using
        '        End Using
        '    End Using
        'End If
        'necessario per far funzionare jquery
        'GridView1.UseAccessibleHeader = True
        'GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
        'If Session("ID") <> "" Then
        '    Dim constr As String = ConfigurationManager.ConnectionStrings("FaconConnectionString").ConnectionString
        '    Using con As New SqlConnection(constr)
        '        Using cmd As New SqlCommand("SELECT ID, ArticoloCliente, DescrizioneArticoloCliente, CommessaCliente, format(Cast(Left(DataBollaCliente,4) + '-' + Substring(Cast(DataBollaCliente as varchar(8)),5,2) + '-' + Right(DataBollaCliente,2) as date),'dd/MM/yyyy') as DataBollaCliente, NumeroBollaCliente, QT, PrezzoUnitario, Note FROM TblDDTInviati WHERE ID = " & Session("ID") & " order by ID desc", con)
        '            Using sda As New SqlDataAdapter(cmd)
        '                cmd.CommandType = CommandType.Text
        '                Dim dt As New DataTable()
        '                sda.Fill(dt)
        '                GridView1.DataSource = dt
        '                GridView1.DataBind()
        '            End Using
        '        End Using
        '    End Using
        'Else
        '    Dim constr As String = ConfigurationManager.ConnectionStrings("FaconConnectionString").ConnectionString
        '    Using con As New SqlConnection(constr)
        '        Using cmd As New SqlCommand("SELECT ID, ArticoloCliente, DescrizioneArticoloCliente, CommessaCliente, format(Cast(Left(DataBollaCliente,4) + '-' + Substring(Cast(DataBollaCliente as varchar(8)),5,2) + '-' + Right(DataBollaCliente,2) as date),'dd/MM/yyyy') as DataBollaCliente, NumeroBollaCliente, QT, PrezzoUnitario, Note FROM TblDDTInviati WHERE (IDDDT IS NULL) order by ID desc", con)
        '            Using sda As New SqlDataAdapter(cmd)
        '                cmd.CommandType = CommandType.Text
        '                Dim dt As New DataTable()
        '                sda.Fill(dt)
        '                GridView1.DataSource = dt
        '                GridView1.DataBind()
        '            End Using
        '        End Using
        '    End Using
        '    'necessario per far funzionare jquery
        '    GridView1.UseAccessibleHeader = True
        '    GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
        'End If
        If Data.Text = "" Then
            RecuperaUltimaBolla()
        End If
        If Not Me.IsPostBack Then
            OraConsegna.Text = Format(Date.Now, "HHmm")
            Data.Text = Format(Date.Now, "dd-MM-yyyy")
            LeggiFlagSelezionato()
            EvidenziaSelezionati()
        Else
            GridView1.DataBind()
            EvidenziaSelezionati()
        End If
    End Sub


    Public Sub EvidenziaSelezionati()
        For Each row As GridViewRow In GridView1.Rows
            If row.RowType = DataControlRowType.DataRow Then
                'Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("CheckBox1"), CheckBox)
                If InStr(hidden.Value.ToString, row.Cells(0).Text.ToString) > 0 Then
                    row.BackColor = Color.LightGray
                    Dim bottone As Button = CType(row.Cells(10).Controls(2), Button)
                    bottone.Text = "Deseleziona"
                End If
            End If
        Next
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Button1.Text = "STAMPA BOLLA" Then
            Button1.Text = "AGGIORNA"
            GeneraDocumento()
            hidden.Value = ""
        Else
            Button1.Text = "STAMPA BOLLA"
            Response.Redirect(Url)
        End If



    End Sub


    Public Sub GeneraDocumento()
        Dim NumeroBolla As String = ""
        Dim OraConsegnaInt As Integer
        Dim dtDati As DataTable
        If OraConsegna.Text = "" Then
            OraConsegnaInt = 0
        Else
            OraConsegnaInt = OraConsegna.Text
        End If

        NumeroBolla = NumBolla.Text.ToString

        Dim Documento As New Document(PageSize.A4, 35, 35, 35, 35)
        Dim output = New MemoryStream()
        Dim Scrittura As PdfWriter = PdfWriter.GetInstance(Documento, New FileStream(Server.MapPath("~/FileCaricati/") & "Bolla" & "_" & Session("Code") & "_" & NumeroBolla & "_" & Data.Text.Substring(6, 4) & "_" & Request.QueryString("ID") & ".pdf", FileMode.Create))
        Documento.Open()
        'Dim logo = iTextSharp.text.Image.GetInstance(Server.MapPath("Img/logs.png"))
        'logo.Alignment = iTextSharp.text.Image.ALIGN_LEFT
        'Documento.Add(logo)
        'Documento.Add(Chunk.NEWLINE)
        'Documento.Add(Chunk.NEWLINE)
        Dim titleFont = FontFactory.GetFont("Helvetica", 14, iTextSharp.text.Font.NORMAL)
        Dim FontGenerale = FontFactory.GetFont("Helvetica", 10, iTextSharp.text.Font.NORMAL)
        Dim FontPiccolo = FontFactory.GetFont("Helvetica", 8, iTextSharp.text.Font.NORMAL)
        Dim FontSottolineato = FontFactory.GetFont("Helvetica", 12, iTextSharp.text.Font.UNDERLINE)
        Dim FontGenerale12 = FontFactory.GetFont("Helvetica", 12, iTextSharp.text.Font.NORMAL)
        Dim tblIntestazione As New PdfPTable(2)
        Dim cell1 As PdfPCell
        Dim cell2 As PdfPCell
        'Dim cell3 As PdfPCell
        Dim constr As String = ConfigurationManager.ConnectionStrings("NavConnectionString").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand("SELECT [Name], [Address], [Post Code], [City], County, [VAT Registration No_]  FROM [MGS].[dbo].[MGS$Vendor] where No_ = '" & Session("Code") & "'", con)
                Using sda As New SqlDataAdapter(cmd)
                    cmd.CommandType = CommandType.Text
                    Dim dt As New DataTable()
                    sda.Fill(dt)
                    cell2 = New PdfPCell(New Phrase(Trim(dt.Rows(0).Item(0).ToString) & vbCr & Trim(dt.Rows(0).Item(1).ToString) & vbCr & Trim(dt.Rows(0).Item(2).ToString) & " " & Trim(dt.Rows(0).Item(3).ToString) & " (" & Trim(dt.Rows(0).Item(4).ToString) & ")" & vbCrLf & "partita iva: " & Trim(dt.Rows(0).Item(5).ToString) & vbCrLf, FontGenerale12)) : cell2.Border = 0
                End Using
            End Using
        End Using

        cell1 = New PdfPCell(New Phrase("")) : cell1.Border = 0
        Dim Frase1 As New Phrase("(D.D.T.) (D.P.R 472 DEL 14/08/1996)", FontPiccolo)
        Dim Frase2 As New Phrase("DOCUMENTO DI TRASPORTO", FontSottolineato)
        cell1.AddElement(Frase2)
        cell1.AddElement(Frase1)
        'cell3 = New PdfPCell(New Phrase(tabella2.Rows(0).Item("FirstName").ToString & " " & tabella2.Rows(0).Item("LastName").ToString & vbCr & tabella2.Rows(0).Item("Company").ToString & vbCr & tabella2.Rows(0).Item("Address1").ToString & vbCr & tabella2.Rows(0).Item("ZipPostalCode").ToString & " " & tabella2.Rows(0).Item("City").ToString & vbCr & tabella2.Rows(0).Item("StateProvince").ToString & vbCr & tabella2.Rows(0).Item("Country").ToString, Font)) : cell3.HorizontalAlignment = 0 : cell3.Border = 0
        tblIntestazione.AddCell(cell2) : tblIntestazione.AddCell(cell1)
        'tblIntestazione.AddCell(cell3)
        tblIntestazione.TotalWidth = 500.0F : tblIntestazione.LockedWidth = True 'impostazione lunghezza tabella(obbligatorio)

        Documento.Add(tblIntestazione)

        'Dim ParTitolo As New Paragraph("Confezioni Repo srl" & vbCr & "Viale Gran Sasso 33" & vbCr & "64010 Ancarano (TE)" & vbCrLf & "partita iva: 00775210677" & vbCrLf & "email: reposrl@yahoo.it" & vbCrLf & "tel: +39086186232", titleFont)
        'ParTitolo.Alignment = iTextSharp.text.Element.ALIGN_LEFT
        'ParTitolo.SpacingAfter = 1
        'Documento.Add(ParTitolo)
        Dim RigaVuota As New Paragraph(vbCrLf)
        Dim Frase3 As New Paragraph("Destinatario", FontSottolineato)
        Frase3.Alignment = iTextSharp.text.Element.ALIGN_RIGHT
        Documento.Add(Frase3)
        Dim Destinatario As New Paragraph("Maglificio Gran Sasso spa" & vbCrLf & "Via Isaac Newton, 2" & vbCr & " " & "64016" & " " & "Sant'Egidio alla Vibrata (TE)" & vbCr & "Partita Iva: 00061560678", FontGenerale12)
        Destinatario.Alignment = iTextSharp.text.Element.ALIGN_RIGHT
        Documento.Add(Destinatario)
        Documento.Add(RigaVuota)


        Dim DatiBolla As New Paragraph("Numero bolla: " & NumeroBolla & vbCrLf & "Data: " & Data.Text & vbCrLf & "causale trasporto: conto lavorazione " & vbCrLf & "aspetto beni: " & AspettoBeni.Text)
        Documento.Add(DatiBolla)
        Documento.Add(RigaVuota)
        Documento.Add(RigaVuota)

        Dim EtichettaLavorazioni As New Paragraph("Dettaglio dei beni")
        Documento.Add(EtichettaLavorazioni)
        Documento.Add(RigaVuota)
        Dim StringaLavorazioni As String = ""
        Dim QtLavorazioni As String = ""
        Dim StringaId As String = hidden.Value
        'StringaId = hidden.ClientID.Split(",")
        For Each row As GridViewRow In GridView1.Rows
            If row.RowType = DataControlRowType.DataRow Then

                'Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("CheckBox1"), CheckBox)
                If InStr(StringaId, row.Cells(0).Text.ToString) > 0 Then
                    dtDati = RecuperaDati(row.Cells(0).Text.ToString)
                    'If chkRow.Checked Then
                    Dim constr1 As String = ConfigurationManager.ConnectionStrings("FaconConnectionString").ConnectionString
                    Using con As New SqlConnection(constr1)
                        con.Open()
                        Dim query As String = "insert into TblDDTFacon ([CodiceFornitore],[DataBolla],[NumeroBolla],[IDProdotto],[QT],[PrezzoUnitario],[ScontoPercentuale],[ScontoUnitario],[DataConsegna],[OraConsegna],[CodiceClienteFatturazione],[CodiceClienteMerce],[Note],[AspettoBeni],[NumeroColli],[DaFatturare],[TipoSpedizione],[FlagAnnullamento]) values ('" & Session("Code") & "'," & Data.Text.Substring(6, 4) + Data.Text.Substring(3, 2) + Data.Text.Substring(0, 2) & "," & NumeroBolla & ",1," & dtDati.Rows(0).Item(0).ToString & "," & Replace(dtDati.Rows(0).Item(1).ToString, ",", ".") & ",0,0," & Data.Text.Substring(6, 4) + Data.Text.Substring(3, 2) + Data.Text.Substring(0, 2) & "," & OraConsegnaInt & ",null, null, '" & row.Cells(8).Text & "','" & AspettoBeni.SelectedValue & "'," & NumeroColli.Text & ",1,1,0)"
                        Using cmd As New SqlCommand(query)
                            cmd.Connection = con
                            cmd.ExecuteNonQuery()
                        End Using


                        Dim update As String = "update TblDDTInviati set IDDDT = " & RecuperaID(NumeroBolla, Data.Text.Substring(6, 4) + Data.Text.Substring(3, 2) + Data.Text.Substring(0, 2)) & " where id = " & row.Cells(0).Text

                        Using cmd1 As New SqlCommand(update)
                            cmd1.Connection = con
                            cmd1.ExecuteNonQuery()
                        End Using
                        Dim update1 As String = "update TblDDTFacon set Note = '' where Note = '&nbsp;'"
                        Using cmd1 As New SqlCommand(update1)
                            cmd1.Connection = con
                            cmd1.ExecuteNonQuery()
                        End Using
                        con.Close()
                    End Using
                    'StringaLavorazioni = StringaLavorazioni & row.Cells(2).Text & " " & row.Cells(3).Text & " raggr/comm: " & row.Cells(4).Text & " vs ddt:" & row.Cells(6).Text & vbCrLf
                    'QtLavorazioni = QtLavorazioni & row.Cells(7).Text & vbCrLf
                End If
            End If
        Next

        Dim tblLavorazioni As New PdfPTable(4)
        Dim cell5 As PdfPCell
        Dim cell6 As PdfPCell
        Dim cell7 As PdfPCell
        Dim cell8 As PdfPCell
        cell5 = New PdfPCell(New Phrase("Descrizione")) : cell5.Border = 1
        cell5.Colspan = 3
        cell6 = New PdfPCell(New Phrase("Quantità")) : cell6.Border = 1
        tblLavorazioni.AddCell(cell5)
        tblLavorazioni.AddCell(cell6)
        For Each row As GridViewRow In GridView1.Rows
            If row.RowType = DataControlRowType.DataRow Then
                'Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("CheckBox1"), CheckBox)
                If InStr(StringaId, row.Cells(0).Text.ToString) > 0 Then
                    dtDati = RecuperaDati(row.Cells(0).Text.ToString)
                    If dtDati.Rows(0).Item(7).ToString = "" Then
                        cell7 = New PdfPCell(New Phrase(dtDati.Rows(0).Item(2).ToString & " " & Replace(dtDati.Rows(0).Item(3).ToString, "  ", "") & " raggr/comm:" & dtDati.Rows(0).Item(4).ToString & " vs ddt:" & dtDati.Rows(0).Item(6).ToString, FontGenerale)) : cell7.Border = 1
                        cell7.Colspan = 3
                    Else
                        cell7 = New PdfPCell(New Phrase(dtDati.Rows(0).Item(2).ToString & " " & Replace(dtDati.Rows(0).Item(3).ToString, "  ", "") & " raggr/comm:" & dtDati.Rows(0).Item(4).ToString & " vs ddt:" & dtDati.Rows(0).Item(6).ToString & " note:" & dtDati.Rows(0).Item(7).ToString, FontGenerale)) : cell7.Border = 1
                        cell7.Colspan = 3
                    End If
                    tblLavorazioni.AddCell(cell7)
                    cell8 = New PdfPCell(New Phrase(dtDati.Rows(0).Item(0).ToString, FontGenerale)) : cell8.Border = 1
                    tblLavorazioni.AddCell(cell8)
                End If
            End If
        Next

        tblLavorazioni.TotalWidth = 500.0F : tblLavorazioni.LockedWidth = True 'impostazione lunghezza tabella(obbligatorio)

        Documento.Add(tblLavorazioni)

        Documento.Add(RigaVuota)
        'Dim FasiDiLavorazione As New Paragraph(StringaLavorazioni)
        'Documento.Add(FasiDiLavorazione)
        'Documento.Add(RigaVuota)
        If Note.Text.Length > 0 Then
            Dim TestoNote As New Paragraph("Note: " & Note.Text, FontSottolineato)
            Documento.Add(TestoNote)
        End If
        Documento.Add(RigaVuota)

        Dim DataConsegna As New Paragraph("Data di consegna: " & Data.Text & " ora: " & OraConsegna.Text & vbCrLf & "Trasporto a cura del destinatario")
        Documento.Add(DataConsegna)
        Documento.Add(RigaVuota)
        Dim NumeroColliPar As New Paragraph("Numero dei colli:" & NumeroColli.Text)
        Documento.Add(NumeroColliPar)
        Documento.Add(RigaVuota)
        Documento.Add(RigaVuota)
        Dim Firma As New Paragraph("Firma del conducente " & vbCr & "______________________________")
        Documento.Add(RigaVuota)
        Documento.Add(Firma)
        Documento.Add(RigaVuota)


        Documento.Close()


        'Profila(1, dt.Rows(0).Item(4).ToString & "_" & NumeroBolla & "_" & Year(Date.Now) & ".pdf", 3, 1, 2, "DDT numero " & NumeroBolla & " del " & Data.Text, Data.Text, NumeroBolla, Data.Text.Substring(3, 2), Data.Text.Substring(6, 4), "", "")


        'Me.Response.BufferOutput = True
        'Me.Response.Clear()
        'Me.Response.ClearHeaders()
        ''Me.Response.ContentType = "application/octet-stream"
        'Me.Response.ContentType = "application/pdf"
        'Me.Response.AddHeader("Content-Disposition", "attachment; filename=" & "Bolla" & "_" & Session("Code") & "_" & NumeroBolla & "_" & Data.Text.Substring(6, 4) & ".pdf")
        'Dim FilePdf As FileInfo = New FileInfo(Server.MapPath("~/FileCaricati/") & "Bolla" & "_" & Session("Code") & "_" & NumeroBolla & "_" & Data.Text.Substring(6, 4) & ".pdf")
        'Me.Response.TransmitFile(FilePdf.FullName)

        'Me.Response.Flush()

        'Me.Response.End()



    End Sub

    Public Function RecuperaID(ByVal NumeroBolla As Integer, DataBolla As Integer) As Integer
        Dim ds2 As New DataSet
        Dim constr As String = ConfigurationManager.ConnectionStrings("FaconConnectionString").ConnectionString
        Dim objConn As New SqlClient.SqlConnection(constr)
        objConn.Open()
        Dim Strsql As String = ""
        'If Session("Admin") Then
        Strsql = "SELECT max(ID) FROM Facon.[dbo].[TblDDTFacon] where NumeroBolla =" & NumeroBolla & " and DataBolla = " & DataBolla & " and CodiceFornitore = " & Session("Code")
        'Else
        'Strsql = "SELECT t1.Id, Oggetto, t2.Nome + ' ' + t2.Cognome as Nome FROM [RepoDox].[dbo].[TblProfilazione] t1 inner join [RepoDox].[dbo].[TblDipendenti] t2 on t1.CodiceFiscale = t2.CodiceFiscale where IDClasseDocumentale = 1 and t1.CodiceFiscale = '" & codice & "' ORDER BY Anno DESC, cast(Mese as int) desc, t2.Cognome asc"
        'End If
        Dim dataAdapter As New SqlClient.SqlDataAdapter(Strsql, objConn)
        ds2.EnforceConstraints = False
        dataAdapter.FillSchema(ds2, SchemaType.Source, "File")
        dataAdapter.Fill(ds2, "File")
        objConn.Close()
        Dim dt3 As New DataTable
        dt3 = ds2.Tables(0)
        Return dt3.Rows(0).Item(0).ToString
    End Function
    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        Dim constr As String = ConfigurationManager.ConnectionStrings("FaconConnectionString").ConnectionString
        Select Case e.CommandName

            'Case "Modifica"

            '    Dim indice = e.CommandArgument
            '    'Dim pippo As String = GridView1.Rows(indice).Cells(6).Text
            '    'Dim row As GridViewRow = GridView1.SelectedRow.Cells(6).Text
            '    Session("ID") = GridView1.Rows(indice).Cells(0).Text
            '    GridView1.Columns(10).Visible = False
            '    GridView1.Columns(11).Visible = True
            '    GridView1.Columns(12).Visible = True
            '    GridView1.Columns(13).Visible = True
            '    GridView1.Columns(14).Visible = True
            '    GridView1.Columns(15).Visible = True


            Case "Aggiorna"

                Dim indice = e.CommandArgument
                Dim id As Integer = GridView1.Rows(indice).Cells(0).Text
                If CType(GridView1.Rows(indice).Cells(13).Controls(1), TextBox).Text <> "" Then
                    Dim qt As Integer = CType(GridView1.Rows(indice).Cells(13).Controls(1), TextBox).Text
                    If qt <> 0 Then
                        AggiornaQtRiga(id, qt)
                    End If
                End If
                Dim noteaggiuntive As String = CType(GridView1.Rows(indice).Cells(14).Controls(1), DropDownList).SelectedValue.ToString
                If noteaggiuntive <> "" Then
                    AggiornaNoteAggiuntiveRiga(id, noteaggiuntive)
                End If
                If CType(GridView1.Rows(indice).Cells(15).Controls(1), CheckBox).Checked Then
                    AggiornaFlagRientroTemporaneo(id)
                End If
                Session("ID") = ""
                'GridView1.Columns(10).Visible = True
                'GridView1.Columns(11).Visible = False
                'GridView1.Columns(12).Visible = False
                'GridView1.Columns(13).Visible = False
                'GridView1.Columns(14).Visible = False
                'GridView1.Columns(15).Visible = False
                ''GridView1.UseAccessibleHeader = True
                ''GridView1.HeaderRow.TableSection = TableRowSection.TableHeader

            Case "Select"
                Dim indice = e.CommandArgument
                Dim bottone As Button = CType(GridView1.Rows(indice).Cells(10).Controls(2), Button)
                Dim qt = GridView1.Rows(indice).Cells(6).Text
                If bottone.Text = "Seleziona" Then
                    GridView1.Rows(indice).BackColor = Color.LightGray
                    bottone.Text = "Deseleziona"
                    hidden.Value = hidden.Value & "," & GridView1.Rows(indice).Cells(0).Text
                    ContaSelezionati = Selezionati.Text + 1
                    Selezionati.Text = ContaSelezionati
                    Using con As New SqlConnection(constr)
                        con.Open()
                        Dim update As String = "update TblDDTInviati set FlagSelezionato = 1 where id = " & GridView1.Rows(indice).Cells(0).Text & " "
                        Using cmd1 As New SqlCommand(update)
                            cmd1.Connection = con
                            cmd1.ExecuteNonQuery()
                        End Using
                        con.Close()
                    End Using
                Else
                    GridView1.Rows(indice).BackColor = Color.White
                    bottone.Text = "Seleziona"
                    hidden.Value = Replace(hidden.Value, GridView1.Rows(indice).Cells(0).Text, "")
                    ContaSelezionati = Selezionati.Text - 1
                    Selezionati.Text = ContaSelezionati
                    Using con As New SqlConnection(constr)
                        con.Open()
                        Dim update As String = "update TblDDTInviati set FlagSelezionato = null where id = " & GridView1.Rows(indice).Cells(0).Text & " "
                        Using cmd1 As New SqlCommand(update)
                            cmd1.Connection = con
                            cmd1.ExecuteNonQuery()
                        End Using
                        con.Close()
                    End Using
                End If




                'GridView1.Columns(10).Visible = True
                'GridView1.Columns(11).Visible = False
                'GridView1.Columns(12).Visible = False
                'GridView1.Columns(13).Visible = False
                'GridView1.Columns(14).Visible = False
                'GridView1.Columns(15).Visible = False
                'GridView1.UseAccessibleHeader = True
                'GridView1.HeaderRow.TableSection = TableRowSection.TableHeader

            Case "Annulla"
                Session("ID") = ""
                'GridView1.Columns(10).Visible = True
                'GridView1.Columns(11).Visible = False
                'GridView1.Columns(12).Visible = False
                'GridView1.Columns(13).Visible = False
                'GridView1.Columns(14).Visible = False
                'GridView1.Columns(15).Visible = False
                ''GridView1.UseAccessibleHeader = True
                ''GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
            Case "Update"


        End Select
    End Sub

    Public Sub AggiornaQtRiga(ByVal id As Integer, ByVal qt As Integer)
        Try


            Dim constr As String = ConfigurationManager.ConnectionStrings("FaconConnectionString").ConnectionString
            Using con As New SqlConnection(constr)
                con.Open()
                Dim update As String = "update [Facon].[dbo].[TblDDTInviati] set QT = " & qt & " where id = " & id
                Using cmd1 As New SqlCommand(update)
                    cmd1.Connection = con
                    cmd1.ExecuteNonQuery()
                End Using
                'Using cmd As New SqlCommand("SELECT ID, ArticoloCliente, DescrizioneArticoloCliente, CommessaCliente, Format(Cast(Left(DataBollaCliente, 4) + '-' + Substring(Cast(DataBollaCliente as varchar(8)),5,2) + '-' + Right(DataBollaCliente,2) as date),'dd/MM/yyyy') as DataBollaCliente, NumeroBollaCliente, QT, PrezzoUnitario, Note, FlagInvioTemporaneo FROM TblDDTInviati WHERE (IDDDT IS NULL) and CodiceFornitore = '" & Session("Code") & "' order by ID desc", con)
                '    Using sda As New SqlDataAdapter(cmd)
                '        cmd.CommandType = CommandType.Text
                '        Dim dt As New DataTable()
                '        sda.Fill(dt)
                '        If dt.Rows.Count > 0 Then
                '            GridView1.DataSource = dt
                '            GridView1.DataBind()
                '            'necessario per far funzionare jquery
                '            'GridView1.UseAccessibleHeader = True
                '            'GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
                '        End If
                '    End Using
                'End Using
                con.Close()
            End Using
        Catch ex As Exception

        End Try

    End Sub
    Public Sub AggiornaNoteAggiuntiveRiga(ByVal id As Integer, ByVal noteaggiuntive As String)
        Dim constr As String = ConfigurationManager.ConnectionStrings("FaconConnectionString").ConnectionString
        Using con As New SqlConnection(constr)
            con.Open()
            Dim update As String = "update TblDDTInviati set Note = IsNull(Note,'') + ' - ' + '" & noteaggiuntive & "' where id = " & id & " "
            Using cmd1 As New SqlCommand(update)
                cmd1.Connection = con
                cmd1.ExecuteNonQuery()
            End Using
            Using cmd As New SqlCommand("SELECT ID, ArticoloCliente, DescrizioneArticoloCliente, CommessaCliente, Format(Cast(Left(DataBollaCliente, 4) + '-' + Substring(Cast(DataBollaCliente as varchar(8)),5,2) + '-' + Right(DataBollaCliente,2) as date),'dd/MM/yyyy') as DataBollaCliente, NumeroBollaCliente, QT, PrezzoUnitario, Note, FlagInvioTemporaneo FROM TblDDTInviati WHERE (IDDDT IS NULL) and CodiceFornitore = '" & Session("Code") & "' order by ID desc", con)
                Using sda As New SqlDataAdapter(cmd)
                    cmd.CommandType = CommandType.Text
                    Dim dt As New DataTable()
                    sda.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        GridView1.DataSource = dt
                        GridView1.DataBind()
                        'necessario per far funzionare jquery
                        'GridView1.UseAccessibleHeader = True
                        'GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
                    End If
                End Using
            End Using
            con.Close()
        End Using
        'Response.Redirect("DDT.aspx")

    End Sub

    Public Sub AggiornaFlagRientroTemporaneo(ByVal id As Integer)
        Dim constr As String = ConfigurationManager.ConnectionStrings("FaconConnectionString").ConnectionString
        Using con As New SqlConnection(constr)
            con.Open()
            Dim update As String = "update TblDDTInviati set FlagInvioTemporaneo = 1 where id = " & id & " "
            Using cmd1 As New SqlCommand(update)
                cmd1.Connection = con
                cmd1.ExecuteNonQuery()
            End Using
            Using cmd As New SqlCommand("SELECT ID, ArticoloCliente, DescrizioneArticoloCliente, CommessaCliente, Format(Cast(Left(DataBollaCliente, 4) + '-' + Substring(Cast(DataBollaCliente as varchar(8)),5,2) + '-' + Right(DataBollaCliente,2) as date),'dd/MM/yyyy') as DataBollaCliente, NumeroBollaCliente, QT, PrezzoUnitario, Note, FlagInvioTemporaneo FROM TblDDTInviati WHERE (IDDDT IS NULL) and CodiceFornitore = '" & Session("Code") & "' order by ID desc", con)
                Using sda As New SqlDataAdapter(cmd)
                    cmd.CommandType = CommandType.Text
                    Dim dt As New DataTable()
                    sda.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        GridView1.DataSource = dt
                        GridView1.DataBind()
                        'necessario per far funzionare jquery
                        'GridView1.UseAccessibleHeader = True
                        'GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
                    End If
                End Using
            End Using
            con.Close()
        End Using
        'Response.Redirect("DDT.aspx")

    End Sub
    Public Sub SearchGv()
        Dim constr As String = ConfigurationManager.ConnectionStrings("FaconConnectionString").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand()
                Dim sql As String = "SELECT ID, ArticoloCliente, DescrizioneArticoloCliente, CommessaCliente, Format(Cast(Left(DataBollaCliente, 4) + '-' + Substring(Cast(DataBollaCliente as varchar(8)),5,2) + '-' + Right(DataBollaCliente,2) as date),'dd/MM/yyyy') as DataBollaCliente, NumeroBollaCliente, QT, PrezzoUnitario, Note, FlagInvioTemporaneo FROM TblDDTInviati WHERE (IDDDT IS NULL) and CodiceFornitore = '" & Session("Code") & "'"
                If Not String.IsNullOrEmpty(SearchTxt.Text.Trim()) Then
                    sql += " and ((CommessaCliente LIKE '" & SearchTxt.Text.Trim() & "%') or (NumeroBollaCliente LIKE '" & SearchTxt.Text.Trim() & "%') or (DescrizioneArticoloCliente LIKE '" & SearchTxt.Text.Trim() & "%') or (ArticoloCliente LIKE '" & SearchTxt.Text.Trim() & "%'))"
                    'cmd.Parameters.AddWithValue("@StringaRicerca", Note.Text.Trim())
                End If
                'cmd.CommandText = sql
                'cmd.Connection = con
                'Using sda As New SqlDataAdapter(cmd)
                'Dim dt As New DataTable()
                'sda.Fill(dt)
                SqlDataSource1.SelectCommand = sql
                'GridView1.DataSource = dt
                GridView1.DataBind()
                'End Using
            End Using
        End Using
        EvidenziaSelezionati()
    End Sub
    Public Sub RecuperaUltimaBolla()
        Dim ds1 As New DataSet
        Dim constr As String = ConfigurationManager.ConnectionStrings("FaconConnectionString").ConnectionString
        Dim objConn As New SqlClient.SqlConnection(constr)
        objConn.Open()
        Dim Strsql As String = ""
        'If Session("Admin") Then
        Strsql = "SELECT max(NumeroBolla) FROM [Facon].[dbo].[TblDDTFacon] where CodiceFornitore = '" & Session("Code") & "' and cast(left(DataBolla,4) as int) = " & Year(Now)
        'Else
        'Strsql = "SELECT t1.Id, Oggetto, t2.Nome + ' ' + t2.Cognome as Nome FROM [RepoDox].[dbo].[TblProfilazione] t1 inner join [RepoDox].[dbo].[TblDipendenti] t2 on t1.CodiceFiscale = t2.CodiceFiscale where IDClasseDocumentale = 1 and t1.CodiceFiscale = '" & codice & "' ORDER BY Anno DESC, cast(Mese as int) desc, t2.Cognome asc"
        'End If
        Dim dataAdapter As New SqlClient.SqlDataAdapter(Strsql, objConn)
        ds1.EnforceConstraints = False
        dataAdapter.FillSchema(ds1, SchemaType.Source, "File")
        dataAdapter.Fill(ds1, "File")
        objConn.Close()
        dt1 = ds1.Tables(0)
        NumBolla.Text = (CInt(dt1.Rows(0).Item(0)) + 1).ToString
    End Sub

    Private Sub GridView1_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles GridView1.RowEditing

    End Sub

    Private Sub GridView1_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles GridView1.RowCancelingEdit
        GridView1.DataBind()
    End Sub

    Private Sub GridView1_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles GridView1.RowUpdating
        Dim constr As String = ConfigurationManager.ConnectionStrings("FaconConnectionString").ConnectionString
        Dim id = e.Keys(0).ToString
        Dim qt = e.NewValues.Values(5)
        Using con As SqlConnection = New SqlConnection(constr)
            Using cmd As SqlCommand = New SqlCommand("update [Facon].[dbo].[TblDDTInviati] set QT = @QT where id = @ID")
                cmd.Parameters.AddWithValue("@ID ", id)
                cmd.Parameters.AddWithValue("@QT", qt)
                cmd.Connection = con
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
            End Using
        End Using

        GridView1.EditIndex = -1
        GridView1.DataBind()

    End Sub

    Private Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound

    End Sub

    Private Sub GridView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridView1.SelectedIndexChanged

    End Sub

    Private Sub GridView1_Sorting(sender As Object, e As GridViewSortEventArgs) Handles GridView1.Sorting

    End Sub


    Public Function RecuperaDati(ByVal Id As Integer)
        Dim ds1 As New DataSet
        Dim constr As String = ConfigurationManager.ConnectionStrings("FaconConnectionString").ConnectionString
        Dim objConn As New SqlClient.SqlConnection(constr)
        objConn.Open()
        Dim Strsql As String = ""
        Strsql = "SELECT QT, PrezzoUnitario, ArticoloCliente, DescrizioneArticoloCliente, CommessaCliente, DataBollaCliente, NumeroBollaCliente, Note FROM [Facon].[dbo].[TblDDTInviati] where CodiceFornitore = '" & Session("Code") & "' and ID = " & Id
        Dim dataAdapter As New SqlClient.SqlDataAdapter(Strsql, objConn)
        ds1.EnforceConstraints = False
        dataAdapter.FillSchema(ds1, SchemaType.Source, "File")
        dataAdapter.Fill(ds1, "File")
        objConn.Close()
        dt2 = ds1.Tables(0)
        Return dt2
    End Function


    Public Sub LeggiFlagSelezionato()
        Dim constr As String = ConfigurationManager.ConnectionStrings("FaconConnectionString").ConnectionString
        Using con As New SqlConnection(constr)
            con.Open()
            Using cmd As New SqlCommand("SELECT ID FROM TblDDTInviati WHERE CodiceFornitore = '" & Session("Code") & "' and FlagSelezionato = 1 and IDDDT is null", con)
                Using sda As New SqlDataAdapter(cmd)
                    cmd.CommandType = CommandType.Text
                    Dim dt As New DataTable()
                    sda.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        For Each riga As DataRow In dt.Rows
                            hidden.Value = hidden.Value & "," & riga.Item(0).ToString
                        Next
                    End If
                    Selezionati.Text = dt.Rows.Count
                End Using
            End Using
            con.Close()
        End Using

        'Response.Redirect("DDT.aspx")

    End Sub

    Private Sub GridView1_RowDeleted(sender As Object, e As GridViewDeletedEventArgs) Handles GridView1.RowDeleted

    End Sub

    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Response.Redirect(Url)
    End Sub
End Class