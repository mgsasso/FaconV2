Imports System.Data.SqlClient
Imports System.Drawing
Imports System.IO
Imports System.Net
Imports System.Security.Cryptography
Imports Ionic.Zip
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.pdf.qrcode
Imports iTextSharp.xmp.impl
Imports Microsoft.Win32
Imports NPOI.POIFS.FileSystem
Imports NPOI.XWPF.UserModel
Imports Org.BouncyCastle.Crypto.Modes



'Imports NPOI.HSSF.Util.HSSFColor
'Imports NPOI.SS.Formula.Functions
'Imports NPOI.SS.UserModel
Imports Org.BouncyCastle.Utilities
Imports Telerik.Web.Apoc.Layout
Imports Telerik.Web.UI

Public Class _Default
    Inherits Page
    Public dt1 As DataTable
    Public dt2 As DataTable
    Public ContaSelezionati As Integer
    Public Url As String
    Public UrlPdf As String
    Public FlagSoloDaBollettare As Integer


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Request.Cookies("CookieFaconAdmin") Is Nothing Then

        Else
            Session("Admin") = "True"
        End If
        If Request.Cookies("CookieFacon") Is Nothing Then
            Dim constr As String = ConfigurationManager.ConnectionStrings("NavConnectionString").ConnectionString
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("SELECT [E-Mail], [VAT Registration No_], [No_], Name FROM [MGS].[dbo].[MGS$Vendor] where No_ = '" & Request.QueryString("Code") & "'", con)
                    Using sda As New SqlDataAdapter(cmd)
                        cmd.CommandType = CommandType.Text
                        Dim dt As New DataTable()
                        sda.Fill(dt)
                        If dt.Rows.Count > 0 Then
                            Dim CodiceAuth As Long = CLng(dt.Rows(0).Item(1).ToString) + CLng(dt.Rows(0).Item(2).ToString)
                            CodiceAuth = CodiceAuth * 1952
                            CodiceAuth = CodiceAuth.ToString.Substring(0, 9)
                            Session("NomeFacon") = Trim(dt.Rows(0).Item(3).ToString)
                            If Request.QueryString("ID") = CodiceAuth Then
                                Session("ID") = Request.QueryString("ID")
                                Session("Code") = Request.QueryString("Code")
                                Session("Email") = Trim(dt.Rows(0).Item(0).ToString)
                                If Session("Admin") = "True" Then
                                    CreaCookieMaster()
                                Else
                                    Response.Redirect("Accettazione.aspx")
                                End If
                            End If
                            If Request.Cookies("CookieFacon") Is Nothing Then
                                Response.Redirect("NoAccess.aspx")
                            Else
                                'Dim aCookie As HttpCookie
                                'aCookie = Request.Cookies("CookieFacon")
                                'aCookie.Values("Code") = Request.QueryString("Code")
                                'aCookie.Values("ID") = Request.QueryString("ID")
                                'aCookie.Values("NomeFacon") = Session("NomeFacon")
                                Session("Code") = Request.Cookies("CookieFacon").Values("Code").ToString
                                Session("ID") = Request.QueryString("ID")
                                Url = "~/default?ID=" & Request.QueryString("ID") & "&Code=" & Request.QueryString("Code")
                            End If
                        Else
                            Response.Redirect("NoAccess.aspx")
                        End If
                    End Using
                End Using
            End Using
        Else
            If Request.QueryString("Code") <> "" Then
                Dim constr As String = ConfigurationManager.ConnectionStrings("NavConnectionString").ConnectionString
                Using con As New SqlConnection(constr)
                    Using cmd As New SqlCommand("SELECT [E-Mail], [VAT Registration No_], [No_], Name FROM [MGS].[dbo].[MGS$Vendor] where No_ = '" & Request.QueryString("Code") & "'", con)
                        Using sda As New SqlDataAdapter(cmd)
                            cmd.CommandType = CommandType.Text
                            Dim dt As New DataTable()
                            sda.Fill(dt)
                            If dt.Rows.Count > 0 Then
                                Dim CodiceAuth As Long = CLng(dt.Rows(0).Item(1).ToString) + CLng(dt.Rows(0).Item(2).ToString)
                                CodiceAuth = CodiceAuth * 1952
                                CodiceAuth = CodiceAuth.ToString.Substring(0, 9)
                                Session("NomeFacon") = Trim(dt.Rows(0).Item(3).ToString)
                                If Request.QueryString("ID") = CodiceAuth Then
                                    Session("ID") = Request.QueryString("ID")
                                    Session("Code") = Request.QueryString("Code")
                                    Session("Email") = Trim(dt.Rows(0).Item(0).ToString)
                                    Session("NomeFacon") = Trim(dt.Rows(0).Item(3).ToString)
                                End If
                                If Request.Cookies("CookieFacon") Is Nothing Then
                                    Response.Redirect("NoAccess.aspx")
                                Else
                                    Dim aCookie As HttpCookie
                                    aCookie = Request.Cookies("CookieFacon")
                                    aCookie.Values("Code") = Session("Code")
                                    aCookie.Values("ID") = Session("ID")
                                    aCookie.Values("NomeFacon") = Session("NomeFacon")
                                    Response.Cookies.Add(aCookie)

                                    'Session("Code") = Request.Cookies("CookieFacon").Values("Code").ToString
                                    'Session("ID") = Request.QueryString("ID")
                                    Url = "~/default?ID=" & Request.QueryString("ID") & "&Code=" & Request.QueryString("Code")
                                End If
                            Else
                                Response.Redirect("NoAccess.aspx")
                            End If
                        End Using
                    End Using
                End Using
            Else
                Session("Code") = Request.Cookies("CookieFacon").Values("Code").ToString
                Session("NomeFacon") = Request.Cookies("CookieFacon").Values("NomeFacon").ToString
                Session("ID") = Request.Cookies("CookieFacon").Values("ID").ToString
            End If
        End If
        If Session("Admin") = "True" Then
            GridView1.Columns(16).Visible = True
            GridView1.Columns(17).Visible = True
        End If
    End Sub
    Public Sub CreaCookieMaster()
        Dim aCookie As New HttpCookie("CookieFacon")
        aCookie.Values("Code") = Request.QueryString("Code")
        aCookie.Values("ID") = Request.QueryString("ID")
        aCookie.Values("NomeFacon") = Session("NomeFacon")
        aCookie.Values("lastVisit") = DateTime.Now.ToString()
        aCookie.Expires = DateTime.Now.AddDays(720)
        Response.Cookies.Add(aCookie)
    End Sub

    Private Sub _Default_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete

        If Data.Text = "" Then
            RecuperaUltimaBolla()
        End If
        If Not Me.IsPostBack Then
            OraConsegna.Text = Format(Date.Now, "HHmm")
            Data.Text = Format(Date.Now, "dd/MM/yyyy")
            LeggiFlagSelezionato()
            EvidenziaSelezionati()
        Else
            'GridView1.DataBind()
            EvidenziaSelezionati()
        End If
    End Sub


    Public Sub EvidenziaSelezionati()
        For Each row As GridViewRow In GridView1.Rows
            If row.RowType = DataControlRowType.DataRow Then
                'Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("CheckBox1"), CheckBox)
                If InStr(hidden.Value.ToString, row.Cells(0).Text.ToString) > 0 Then
                    Dim dtDati As DataTable
                    dtDati = RecuperaDati(row.Cells(0).Text.ToString)
                    If dtDati.Rows(0).Item(8).ToString = "1" Then
                        row.BackColor = Color.LightGray
                        Dim bottone As Button = CType(row.Cells(13).Controls(0), Button)
                        bottone.Text = "Deseleziona"
                    Else
                        row.BackColor = Color.LightCoral
                        Dim bottone As Button = CType(row.Cells(14).Controls(0), Button)
                        bottone.Text = "Deseleziona"
                    End If

                End If
            End If
        Next
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Button1.Text = "STAMPA BOLLA" Then
            If NumeroColli.Text = "" Then
                Label1.Visible = True
                Label1.Text = "Inserire il numero dei colli"
                Exit Sub
            Else
                Label1.Visible = False
                Label1.Text = ""
            End If
            'Button1.Text = "AGGIORNA"
            GeneraDocumento()
            hidden.Value = ""
            'HplUrlPdf.Text = "Vedi bolla"
            'HplUrlPdf.NavigateUrl = UrlPdf
            'HplUrlPdf.Visible = True

            RecuperaUltimaBolla()
            OraConsegna.Text = Format(Date.Now, "HHmm")
            Data.Text = Format(Date.Now, "dd/MM/yyyy")
            NumeroColli.Text = ""

            GridView1.DataBind()
            'Response.Redirect(UrlPdf, 0)
            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", "window.open(" & UrlPdf & ",'_blank');", True)
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "openModal", "window.open('" + Replace(UrlPdf, "~/", "") + "','_blank');", True)
            'System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openModal", "window.open('NewQuote.aspx?val= " + this.txtQuotationNo.Text + "' ,'_blank');", True);

        Else
            'Button1.Text = "STAMPA BOLLA"
            'Response.Redirect(Url)
        End If




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

    Public Sub AggiornaPrezzoLavorazioniParzialiPrimaFase(ByVal Indice As Integer)
        Dim constr1 As String = ConfigurationManager.ConnectionStrings("FaconConnectionString").ConnectionString
        Dim update As String = ""
        Using con As New SqlConnection(constr1)
            con.Open()
            update = "update t1 set PrezzoUnitario = Round(t2.PrezzoUnitario * 0.8,2) from [Facon].[dbo].[TblDDTFacon] t1 " &
            "inner join [Facon].[dbo].[TblDDTInviati] t2 " &
            "on t1.id = t2.IDDDT " &
            "where t2.FlagInvioTemporaneo = '1' and t1.DaFatturare = 1 and t2.id = " & Indice & " "
            Using cmd As New SqlCommand(update)
                cmd.Connection = con
                cmd.ExecuteNonQuery()
            End Using
            con.Close()
        End Using

    End Sub

    Public Sub AggiornaPrezzoLavorazioniParzialiSecondaFase(ByVal Indice As Integer)
        Dim constr1 As String = ConfigurationManager.ConnectionStrings("FaconConnectionString").ConnectionString
        Dim update As String = ""
        Using con As New SqlConnection(constr1)
            con.Open()
            update = "update t1 set PrezzoUnitario = Round(t2.PrezzoUnitario * 0.8,2) from [Facon].[dbo].[TblDDTFacon] t1 " &
            "inner join [Facon].[dbo].[TblDDTInviati] t2 " &
            "on t1.id = t2.IDDDT " &
            "where t2.FlagInvioTemporaneo = '1' and t2.id = " & Indice & " "
            Using cmd As New SqlCommand(update)
                cmd.Connection = con
                cmd.ExecuteNonQuery()
            End Using
            update = "update t1 set PrezzoUnitario = Round(t2.PrezzoUnitario * 0.2,2) from [Facon].[dbo].[TblDDTFacon] t1 " &
            "inner join [Facon].[dbo].[TblDDTInviati] t2 " &
            "on t1.id = t2.IDDDT " &
            "where CharIndex('/R', t2.NumeroBollaCliente) > 0 and t2.id = " & Indice & " "
            Using cmd As New SqlCommand(update)
                cmd.Connection = con
                cmd.ExecuteNonQuery()
            End Using
            con.Close()
        End Using

    End Sub

    Public Sub AggiornaPrezzoResoSenzaLavorazione(ByVal Indice As Integer)
        Dim constr1 As String = ConfigurationManager.ConnectionStrings("FaconConnectionString").ConnectionString
        Dim update As String = ""
        Using con As New SqlConnection(constr1)
            con.Open()
            update = "update t1 set PrezzoUnitario = 0, DaFatturare = 0 from [Facon].[dbo].[TblDDTFacon] t1 " &
            "inner join [Facon].[dbo].[TblDDTInviati] t2 " &
            "on t1.id = t2.IDDDT " &
            "where t2.CausaleLavorazione = 2 and t2.id = " & Indice & " "
            Using cmd As New SqlCommand(update)
                cmd.Connection = con
                cmd.ExecuteNonQuery()
            End Using
            con.Close()
        End Using

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

        Dim Documento As New iTextSharp.text.Document(PageSize.A4, 35, 35, 35, 35)
        Dim output = New MemoryStream()
        Dim Scrittura As PdfWriter = PdfWriter.GetInstance(Documento, New FileStream(Server.MapPath("~/FileCaricati/") & "Bolla" & "_" & Session("Code") & "_" & NumeroBolla & "_" & Data.Text.Substring(6, 4) & Data.Text.Substring(3, 2) & Data.Text.Substring(0, 2) & ".pdf", FileMode.Create))
        Documento.Open()
        'Dim logo = iTextSharp.text.Image.GetInstance(Server.MapPath("Img/logs.png"))
        'logo.Alignment = iTextSharp.text.Image.ALIGN_LEFT
        'Documento.Add(logo)
        'Documento.Add(Chunk.NEWLINE)
        'Documento.Add(Chunk.NEWLINE)
        Dim titleFont = FontFactory.GetFont("Helvetica", 14, iTextSharp.text.Font.NORMAL)
        Dim FontGenerale = FontFactory.GetFont("Helvetica", 10, iTextSharp.text.Font.NORMAL)
        Dim FontGeneraleBold = FontFactory.GetFont("Helvetica", 10, iTextSharp.text.Font.BOLD)
        Dim FontGeneraleBoldRosso = FontFactory.GetFont("Helvetica", 10, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.RED)
        Dim FontPiccolo = FontFactory.GetFont("Helvetica", 8, iTextSharp.text.Font.NORMAL)
        Dim FontSottolineato = FontFactory.GetFont("Helvetica", 12, iTextSharp.text.Font.UNDERLINE)
        Dim FontSottolineato10 = FontFactory.GetFont("Helvetica", 10, iTextSharp.text.Font.UNDERLINE)
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
                    'cell2.Width = 300.0F

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
        'tblIntestazione.TotalWidth = 500.0F
        'tblIntestazione.TotalWidth = 100
        tblIntestazione.WidthPercentage = 100
        'tblIntestazione.LockedWidth = True 'impostazione larghezza tabella(obbligatorio)

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


        Dim DatiBolla As New Paragraph("Numero bolla: " & NumeroBolla & "/MGS" & vbCrLf & "Data: " & Data.Text & vbCrLf & "causale trasporto: conto lavorazione " & vbCrLf & "aspetto beni: " & AspettoBeni.Text)
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


                        Dim update As String = "update TblDDTInviati set IDDDT = " & RecuperaID(NumeroBolla, Data.Text.Substring(6, 4) + Data.Text.Substring(3, 2) + Data.Text.Substring(0, 2)) & ", FlagSelezionato = null where id = " & row.Cells(0).Text
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

                        AggiornaCapiDaCampionare(row.Cells(0).Text)
                        'AggiornaPrezzoResoSenzaLavorazione(row.Cells(0).Text)
                        'AggiornaPrezzoLavorazioniParzialiPrimaFase(row.Cells(0).Text)
                        'AggiornaPrezzoLavorazioniParzialiSecondaFase(row.Cells(0).Text)
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
                    If dtDati.Rows(0).Item(8).ToString = "1" Then
                        cell7 = New PdfPCell(New Phrase(dtDati.Rows(0).Item(2).ToString & " " & Replace(dtDati.Rows(0).Item(3).ToString, "  ", "") & " raggr/comm:" & dtDati.Rows(0).Item(4).ToString & " vs ddt:" & dtDati.Rows(0).Item(6).ToString, FontGenerale)) : cell7.Border = 1
                        cell7.Colspan = 3
                    Else
                        Dim Frase = New Phrase()

                        Frase.Add(New Chunk(dtDati.Rows(0).Item(2).ToString & " " & Replace(dtDati.Rows(0).Item(3).ToString, "  ", "") & " raggr/comm:" & dtDati.Rows(0).Item(4).ToString & " vs ddt:" & dtDati.Rows(0).Item(6).ToString, FontGenerale))
                        Frase.Add(New Chunk(" note: RESO NON LAVORATO " & dtDati.Rows(0).Item(7).ToString, FontGeneraleBoldRosso))
                        'cell7 = New PdfPCell(New Phrase(dtDati.Rows(0).Item(2).ToString & " " & Replace(dtDati.Rows(0).Item(3).ToString, "  ", "") & " raggr/comm:" & dtDati.Rows(0).Item(4).ToString & " vs ddt:" & dtDati.Rows(0).Item(6).ToString & " note: RESO NON LAVORATO " & dtDati.Rows(0).Item(7).ToString, FontGenerale)) : cell7.Border = 1
                        cell7 = New PdfPCell(Frase) : cell7.Border = 1
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

        Dim DataConsegna As New Paragraph("Data di consegna: " & Data.Text & " ora: " & OraConsegna.Text & vbCrLf & "Trasporto a cura del " & TrasportoACura.Text)
        Documento.Add(DataConsegna)
        Documento.Add(RigaVuota)
        Dim NumeroColliPar As New Paragraph("Numero dei colli: " & NumeroColli.Text)
        Documento.Add(NumeroColliPar)
        Documento.Add(RigaVuota)
        Documento.Add(RigaVuota)
        Dim Firma As New Paragraph("Firma del conducente " & vbCr & "______________________________")
        Documento.Add(RigaVuota)
        Documento.Add(Firma)
        Documento.Add(RigaVuota)
        Dim strBarCodeValue As String = "http://facon.gransasso.it/ConfermaRientro?ID=" & NumeroBolla.ToString & "&Code=" & Session("Code")
        Dim paramQR = New Dictionary(Of EncodeHintType, Object)
        paramQR.Add(EncodeHintType.CHARACTER_SET, CharacterSetECI.GetCharacterSetECIByName("UTF-8"))
        Dim BarCode As BarcodeQRCode = New BarcodeQRCode(strBarCodeValue, 100, 100, paramQR)

        Documento.Add(BarCode.GetImage())
        Documento.Close()

        File.Copy(Server.MapPath("~/FileCaricati/") & "Bolla" & "_" & Session("Code") & "_" & NumeroBolla & "_" & Data.Text.Substring(6, 4) & Data.Text.Substring(3, 2) & Data.Text.Substring(0, 2) & ".pdf", "\\serverdoc\hotfolder\DDTRicevuti\" & "Bolla" & "_" & Session("Code") & "_" & NumeroBolla & "_" & Data.Text.Substring(6, 4) & Data.Text.Substring(3, 2) & Data.Text.Substring(0, 2) & ".pdf")

        UrlPdf = "~/FileCaricati/" & "Bolla" & "_" & Session("Code") & "_" & NumeroBolla & "_" & Data.Text.Substring(6, 4) & Data.Text.Substring(3, 2) & Data.Text.Substring(0, 2) & ".pdf"

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
        'Response.Redirect(Url)



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


            Case "Lavorato"
                Dim indice = e.CommandArgument
                Dim bottone As Button = CType(GridView1.Rows(indice).Cells(13).Controls(0), Button)
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
                SearchTxt.Text = ""
                Response.Redirect("Default")
                'GridView1.DataBind()
            Case "NonLavorato"
                Dim indice = e.CommandArgument
                Dim bottone As Button = CType(GridView1.Rows(indice).Cells(14).Controls(0), Button)
                Dim qt = GridView1.Rows(indice).Cells(6).Text
                If bottone.Text = "Seleziona" Then
                    GridView1.Rows(indice).BackColor = Color.LightCoral
                    bottone.Text = "Deseleziona"
                    hidden.Value = hidden.Value & "," & GridView1.Rows(indice).Cells(0).Text
                    ContaSelezionati = Selezionati.Text + 1
                    Selezionati.Text = ContaSelezionati
                    Using con As New SqlConnection(constr)
                        con.Open()
                        Dim update As String = "update TblDDTInviati set FlagSelezionato = 1, CausaleLavorazione = 2 where id = " & GridView1.Rows(indice).Cells(0).Text & " "
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
                        Dim update As String = "update TblDDTInviati set FlagSelezionato = null, CausaleLavorazione = 1 where id = " & GridView1.Rows(indice).Cells(0).Text & " "
                        Using cmd1 As New SqlCommand(update)
                            cmd1.Connection = con
                            cmd1.ExecuteNonQuery()
                        End Using
                        con.Close()
                    End Using
                End If

                SearchTxt.Text = ""
                Response.Redirect("Default")
                'GridView1.DataBind()
            Case "Annulla"
                'GridView1.Columns(10).Visible = True
                'GridView1.Columns(11).Visible = False
                'GridView1.Columns(12).Visible = False
                'GridView1.Columns(13).Visible = False
                'GridView1.Columns(14).Visible = False
                'GridView1.Columns(15).Visible = False
                ''GridView1.UseAccessibleHeader = True
                ''GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
            Case "Visualizza"
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                'Dim row As GridViewRow = GridView1.Rows(index)
                Dim row As GridViewRow = GridView1.Rows(index)
                Dim item As String
                item = Server.HtmlDecode(row.Cells(1).Text.Substring(4, 1) & row.Cells(1).Text.Substring(0, 4) & "_" & Replace(row.Cells(3).Text, "/", "_") & "_CONFEZIONE.pdf")
                'item = "P2023_793_1_CONFEZIONE.pdf"
                'VisualizzaPdf(item, "\\storage\STAMPE\PRODUZIONE_DOCUMENTI_DI_LAVORAZIONE\Stampa_invio_documenti\Confezione_scorta\")
                VisualizzaPdf(item, Server.MapPath("/FileCaricati/"))
                If File.Exists(Server.MapPath("/FileCaricati/") & item) Then
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "openModal", "window.open('" + Replace("~/FileCaricati/" & item, "~/", "") + "','_blank');", True)
                End If
                'Response.Redirect("Default")
                'GridView1.DataBind()

            Case "ModificaParzialiTotali"

                Using con As New SqlConnection(constr)
                    Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                    Dim row As GridViewRow = GridView1.Rows(index)
                    Dim item As String = ""
                    Dim id = Server.HtmlDecode(row.Cells(0).Text)
                    Dim Stagione = Server.HtmlDecode(row.Cells(1).Text.Substring(4, 1))
                    Dim AnnoStagione = Server.HtmlDecode(row.Cells(1).Text.Substring(0, 4))
                    Dim Raggruppamento = Server.HtmlDecode(row.Cells(3).Text.Substring(0, row.Cells(3).Text.IndexOf("/")))
                    Dim Commessa = Server.HtmlDecode(row.Cells(3).Text.Substring(row.Cells(3).Text.IndexOf("/") + 1, Len(row.Cells(3).Text) - (row.Cells(3).Text.IndexOf("/") + 1)))

                    Dim Strsql As String = ""
                    Dim NumeroBollaFaconista As String = ""
                    Dim FlagInvioTemporaneo As String = ""
                    Strsql = "SELECT case when t1.NumeroBolla is null then 0 else t1.NumeroBolla end as NumeroBolla, IsNull(t2.FlagInvioTemporaneo,'') as FlagInvioTemporaneo, case when IDDDT is null then 0 else IDDDT end as IDDDT  FROM Facon.dbo.[TblDDTInviati] t2 left join Facon.[dbo].[TblDDTFacon] t1 on t1.id = t2.IDDDT where t2.id = " & id
                    con.Open()

                    Using cmd As New SqlCommand(Strsql)
                        cmd.Connection = con
                        Dim dr As SqlDataReader
                        dr = cmd.ExecuteReader
                        If dr.Read() Then
                            FlagInvioTemporaneo = Replace(dr.Item(1), " ", "")
                            If FlagInvioTemporaneo = "" Then FlagInvioTemporaneo = "0"
                            NumeroBollaFaconista = dr.Item(0)
                        End If
                        cmd.Dispose()
                        dr.Close()
                    End Using


                    If NumeroBollaFaconista <> 0 Then
                        Dim cmd1 As SqlCommand = con.CreateCommand
                        cmd1.CommandType = CommandType.StoredProcedure
                        cmd1.CommandTimeout = 30
                        cmd1.Parameters.Add(New SqlParameter("@CodiceFornitore", Session("Code")))
                        cmd1.Parameters.Add(New SqlParameter("@Raggruppamento", Raggruppamento))
                        cmd1.Parameters.Add(New SqlParameter("@Commessa", Commessa))
                        cmd1.Parameters.Add(New SqlParameter("@Stagione", Stagione))
                        cmd1.Parameters.Add(New SqlParameter("@AnnoStagione", AnnoStagione))
                        cmd1.Parameters.Add(New SqlParameter("@NumeroBollaFaconista", NumeroBollaFaconista))
                        cmd1.Parameters.Add(New SqlParameter("@FlagAttuale", FlagInvioTemporaneo))
                        cmd1.CommandText = "CorreggiLavorazioniParzialiErrate"
                        cmd1.ExecuteNonQuery()
                        cmd1.Dispose()
                    Else
                        Using con1 As New SqlConnection(constr)
                            con1.Open()
                            Dim update As String = ""
                            If FlagInvioTemporaneo = "0" Then
                                update = "update TblDDTInviati set FlagInvioTemporaneo = 1 where id = " & id & " "
                            Else
                                update = "update TblDDTInviati set FlagInvioTemporaneo = null where id = " & id & " "
                            End If

                            Using cmd1 As New SqlCommand(update)
                                cmd1.Connection = con1
                                cmd1.ExecuteNonQuery()
                                update = "execute('update applibmgs.teric00f set jofla3 = case jofla3 when '''' then ''1'' else '''' end where jocfor = ''" & Session("Code") & "'' and jocst1 = " & AnnoStagione & " and jocst2 = ''" & Stagione & "'' and jonrag = " & Raggruppamento & " and joncom = " & Commessa & " and jofann = ''''') at as400sql"
                                cmd1.CommandText = update
                                cmd1.ExecuteNonQuery()
                                cmd1.Dispose()
                            End Using
                            con1.Close()
                        End Using
                    End If

                    con.Close()
                End Using
                Response.Redirect("Default")

            Case "EliminaRiga"
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim row As GridViewRow = GridView1.Rows(index)
                Dim item As String = ""
                Dim id = Server.HtmlDecode(row.Cells(0).Text)
                Using con2 As New SqlConnection(constr)
                    con2.Open()
                    Dim update As String = "delete from TblDDTInviati where id = " & id & " "
                    Using cmd1 As New SqlCommand(update)
                        cmd1.Connection = con2
                        cmd1.ExecuteNonQuery()
                    End Using
                    con2.Close()
                End Using


                Response.Redirect("Default")
        End Select
        EvidenziaSelezionati()
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
                Dim sql As String = "SELECT ID, ArticoloCliente, DescrizioneArticoloCliente, CommessaCliente, Format(Cast(Left(DataBollaCliente, 4) + '-' + Substring(Cast(DataBollaCliente as varchar(8)),5,2) + '-' + Right(DataBollaCliente,2) as date),'dd/MM/yyyy') as DataBollaCliente, NumeroBollaCliente, QT, PrezzoUnitario, Note, FlagInvioTemporaneo, TipoProdotto + '-' + cast(CodiceTipoOperazione as varchar(2)) + '-' + Cast(CodiceOperazioneEsterna as varchar(2)) as CodiceOperazione  FROM TblDDTInviati "
                If FlagSoloDaBollettare = 1 Then
                    sql += " WHERE (IDDDT IS NULL) and CodiceFornitore = '" & Session("Code") & "'"
                End If
                If FlagSoloDaBollettare = 0 Then
                    sql += " WHERE (IDDDT IS NOT NULL) and CodiceFornitore = '" & Session("Code") & "' and cast(Left(DataBollaCliente, 4) as int) >= Year(GetDate()) - 1 "
                End If

                If Not String.IsNullOrEmpty(SearchTxt.Text.Trim()) Then
                    sql += " and ((CommessaCliente LIKE '" & SearchTxt.Text.Trim() & "%') or (NumeroBollaCliente LIKE '" & SearchTxt.Text.Trim() & "%') or (DescrizioneArticoloCliente LIKE '" & SearchTxt.Text.Trim() & "%') or (ArticoloCliente LIKE '" & SearchTxt.Text.Trim() & "%')) ORDER BY FlagSelezionato DESC, TblDDTInviati .DataBollaCliente desc, ID DESC"
                    'cmd.Parameters.AddWithValue("@StringaRicerca", Note.Text.Trim())
                Else
                    sql += " ORDER BY FlagSelezionato DESC, TblDDTInviati .DataBollaCliente desc, ID DESC"
                End If
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
        Strsql = "SELECT IsNull(max(NumeroBolla),0) FROM [Facon].[dbo].[TblDDTFacon] where CodiceFornitore = '" & Session("Code") & "' and cast(left(DataBolla,4) as int) = " & Year(Now)
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
        'SearchGv()

    End Sub

    Private Sub GridView1_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles GridView1.RowCancelingEdit
        GridView1.DataBind()
    End Sub

    Private Sub GridView1_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles GridView1.RowUpdating


        Dim constr As String = ConfigurationManager.ConnectionStrings("FaconConnectionString").ConnectionString
        Dim id = e.Keys(0).ToString
        Dim qt = e.NewValues.Values(2)
        Dim note = e.NewValues.Values(4)
        Dim FlagInvioTemporaneo As String = ""
        If note Is Nothing Then
            note = ""
        End If
        'Dim indice = e.CommandArgument
        'Dim id As Integer = GridView1.Rows(indice).Cells(0).Text
        'Dim OldQt As Integer = CType(GridView1.Rows(id).Cells(5).Controls(0), TextBox).Text

        Using con As New SqlConnection(constr)
            con.Open()
            Dim strsql As String = "SELECT case when FlagInvioTemporaneo is null then '' else FlagInvioTemporaneo end as FlagInvioTemporaneo FROM [Facon].[dbo].[TblDDTInviati] where ID = " & id
            Using cmd1 As New SqlCommand(strsql)
                cmd1.Connection = con
                'cmd1.ExecuteNonQuery()
                Dim dr As SqlDataReader
                dr = cmd1.ExecuteReader
                If dr.Read() Then
                    FlagInvioTemporaneo = dr.GetString(0)
                End If
            End Using
            con.Close()
        End Using
        If FlagInvioTemporaneo = "1" Then
            Response.Write("<script language=javascript>alert('Non è possibile modificare le quantità per le lavorazioni parziali')</script>")

            Exit Sub
        End If

        If qt > CInt(e.Keys(1).ToString) Then
            Response.Write("<script language=javascript>alert('La quantità inserita deve essere inferiore a quella modificata')</script>")

            Exit Sub
        End If
        Using con As SqlConnection = New SqlConnection(constr)
            con.Open()
            Using cmd1 As SqlCommand = New SqlCommand("  insert into [Facon].[dbo].[TblDDTInviati] select [CodiceFornitore] ,[ArticoloCliente] ,[DescrizioneArticoloCliente] " &
                                                      ", [CommessaCliente],[DataBollaCliente] ,[NumeroBollaCliente],[CausaleLavorazione],[QT] - @QT,[PrezzoUnitario] " &
                                                      ", @Note,[IDDDT],[FlagInvioTemporaneo] ,[FlagSelezionato] ,[TipoProdotto],[CodiceTipoOperazione],[CodiceOperazioneEsterna],[DataRientro],[FlagCaricatoSuAs400],[QTRettificata],[NoteInterne],[CapiCampionati] " &
                                                      ", [CapiConformi],[CapiNonConformi],[FlagRespinta],[FaseAvanzamentoSuccessiva],[FlagCaricatoAvanzamento]  from [Facon].[dbo].[TblDDTInviati] where id = @ID ")
                cmd1.Parameters.AddWithValue("@ID ", id)
                cmd1.Parameters.AddWithValue("@QT", qt)
                cmd1.Parameters.AddWithValue("@Note", note)
                cmd1.Connection = con
                cmd1.ExecuteNonQuery()

            End Using
            Using cmd As SqlCommand = New SqlCommand("update [Facon].[dbo].[TblDDTInviati] set QT = @QT, Note = @Note where id = @ID")
                cmd.Parameters.AddWithValue("@ID ", id)
                cmd.Parameters.AddWithValue("@QT", qt)
                cmd.Parameters.AddWithValue("@Note", note)
                cmd.Connection = con
                cmd.ExecuteNonQuery()
            End Using

            con.Close()
        End Using



        GridView1.EditIndex = -1
        GridView1.DataBind()

    End Sub


    Public Function RecuperaDati(ByVal Id As Integer)
        Dim ds1 As New DataSet
        Dim constr As String = ConfigurationManager.ConnectionStrings("FaconConnectionString").ConnectionString
        Dim objConn As New SqlClient.SqlConnection(constr)
        objConn.Open()
        Dim Strsql As String = ""
        Strsql = "SELECT QT, PrezzoUnitario, ArticoloCliente, DescrizioneArticoloCliente, CommessaCliente, DataBollaCliente, NumeroBollaCliente, Note, CausaleLavorazione FROM [Facon].[dbo].[TblDDTInviati] where CodiceFornitore = '" & Session("Code") & "' and ID = " & Id
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


    Public Sub VisualizzaPdf(ByVal FileName As String, ByVal Path As String, Optional ByVal NuovoNome As String = "")
        Dim dr As DataRow
        dr = RecuperaPercorsoFileArxivar(FileName)
        If dr IsNot Nothing Then


            Dim ZipToUnpack As String = Replace(dr.Item("PATH"), "E:\", "\\serverdoc\") & "\" & dr.Item("FILENAME")
            Dim UnpackDirectory As String = Path
            Using zip1 As ZipFile = ZipFile.Read(ZipToUnpack)
                zip1.Password = "ARX_gransasso"
                Dim e As ZipEntry
                ' here, we extract every entry, but we could extract conditionally,
                ' based on entry name, size, date, checkbox status, etc.   
                For Each e In zip1
                    e.Extract(UnpackDirectory, ExtractExistingFileAction.OverwriteSilently)
                Next

                Dim document As New iTextSharp.text.Document(PageSize.A3.Rotate)
                'Use a memory string so we don't need to write to disk
                Using outputStream As New MemoryStream()
                    'Associate the PDF with the stream
                    Dim w = PdfWriter.GetInstance(document, outputStream)

                    'Open the PDF for writing'
                    document.Open()

                    'Do PDF stuff Here'
                    Dim bgReader As PdfReader = New PdfReader(Path & FileName)
                    Dim bg As PdfImportedPage
                    Dim cb As PdfContentByte = w.DirectContent
                    For i = 1 To bgReader.NumberOfPages
                        document.NewPage()
                        bg = w.GetImportedPage(bgReader, i)


                        '' add the template beneath content
                        cb.AddTemplate(bg, 0, 0)
                        'w.DirectContentUnder.AddTemplate(bg, 0, 0)

                        'Close the PDF'
                    Next


                    If File.Exists(Path & Replace(FileName, "CONFEZIONE", "FREGIO")) Then
                        bgReader = New PdfReader(Path & Replace(FileName, "CONFEZIONE", "FREGIO"))
                        Dim bg1 As PdfImportedPage
                        Dim cb1 As PdfContentByte = w.DirectContent
                        For i = 1 To bgReader.NumberOfPages
                            document.NewPage()
                            bg1 = w.GetImportedPage(bgReader, i)
                            '' add the template beneath content
                            cb1.AddTemplate(bg1, 0, 0)
                            'w.DirectContentUnder.AddTemplate(bg, 0, 0)

                            'Close the PDF'
                        Next


                        'bgReader1.Close()
                    End If
                    document.NewPage()
                    Dim logo As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(Server.MapPath(".") + "\P20246112150001099A.jpg")
                    logo.ScaleAbsolute(280, 60)
                    logo.SetAbsolutePosition(115, 725)
                    logo.Alignment = 1
                    Dim cb2 As PdfContentByte = w.DirectContent
                    'document.NewPage()
                    cb2.AddImage(logo, True)


                    'document.Add(logo)


                    document.Close()





                End Using


            End Using
        End If
    End Sub

    Public Function RecuperaPercorsoFileArxivar(ByVal NomeFile As String) As DataRow
        Dim ds1 As New DataSet
        Dim objConn As New SqlClient.SqlConnection("Server=DBSERVER;Database=ARCHDB;User Id=sa;Password=1234qwerasdfZXCV")
        objConn.Open()
        Dim dataAdapter As New SqlClient.SqlDataAdapter("Select PATH, FILENAME, DOCNAME, DOCNUMBER FROM DM_PROFILE WHERE ORIGINALE = '" & NomeFile & "' ", objConn)
        ds1.EnforceConstraints = False
        dataAdapter.FillSchema(ds1, SchemaType.Source, "PathFileArxivar")
        dataAdapter.Fill(ds1, "PathFileArxivar")
        objConn.Close()
        If ds1.Tables(0).Rows.Count > 0 Then
            Return ds1.Tables(0).Rows(0)
        End If

    End Function
    Protected Sub BtnOn_Click(sender As Object, e As EventArgs) Handles BtnOn.Click
        BtnOn.CssClass = "btn btn-success"
        BtnOff.CssClass = "btn btn-default"
        FlagSoloDaBollettare = 1
        SearchGv()
        'GridView1.Columns(17).Visible = True


    End Sub
    Protected Sub BtnOff_Click(sender As Object, e As EventArgs) Handles BtnOff.Click
        BtnOn.CssClass = "btn btn-default"
        BtnOff.CssClass = "btn btn-danger"
        FlagSoloDaBollettare = 0
        SearchGv()
        'GridView1.Columns(17).Visible = False
    End Sub


End Class