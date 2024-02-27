Imports System.ComponentModel.DataAnnotations
Imports System.Data.SqlClient
Imports System.Diagnostics.Eventing
Imports System.Diagnostics.Tracing
Imports System.Drawing
Imports System.IO
Imports System.Net.Mail
Imports System.Security.Policy
Imports iTextSharp.text

Imports iTextSharp.text.pdf
Imports Org.BouncyCastle.Crypto.Digests
Imports Org.BouncyCastle.Crypto.Modes
Imports Telerik.Web.UI

Public Class Accettazione

    Inherits System.Web.UI.Page


    Public Url As String
    Protected Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            Button2.Visible = True
        Else
            Button2.Visible = False
            Button1.Visible = False
            TextBox2.Visible = False
        End If
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Request.Cookies("CookieFacon") Is Nothing And TextBox2.Text = Session("random") Then
            Dim constr As String = ConfigurationManager.ConnectionStrings("NavConnectionString").ConnectionString
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("SELECT [E-Mail], [VAT Registration No_], [No_], Name FROM [MGS].[dbo].[MGS$Vendor] where No_ = '" & Session("Code") & "'", con)
                    Using sda As New SqlDataAdapter(cmd)
                        cmd.CommandType = CommandType.Text
                        Dim dt As New DataTable()
                        sda.Fill(dt)
                        If dt.Rows.Count > 0 Then
                            Dim CodiceAuth As Long = CLng(dt.Rows(0).Item(1).ToString) + CLng(dt.Rows(0).Item(2).ToString)
                            CodiceAuth = CodiceAuth * 1952
                            CodiceAuth = CodiceAuth.ToString.Substring(0, 9)
                            Session("NomeFacon") = Trim(dt.Rows(0).Item(3).ToString)
                            If Session("ID") = CodiceAuth Then
                                CreaCookieMaster()
                            End If
                            If Request.Cookies("CookieFacon") Is Nothing Then
                                Response.Redirect("NoAccess.aspx")
                            Else
                                'Dim aCookie As HttpCookie
                                'aCookie = Request.Cookies("CookieFacon")
                                'aCookie.Values("Code") = Session("Code")
                                'aCookie.Values("ID") = Session("ID")
                                'aCookie.Values("NomeFacon") = Session("NomeFacon")
                                Session("Code") = Request.Cookies("CookieFacon").Values("Code").ToString
                                Session("NomeFacon") = Request.Cookies("CookieFacon").Values("NomeFacon").ToString
                                Session("ID") = Request.Cookies("CookieFacon").Values("ID").ToString
                                Session("Email") = Trim(dt.Rows(0).Item(0).ToString)
                                GeneraLetteraPdf()
                                'InviaEmail("amministrazione@gransasso.it", "calcagnoli@gransasso.it", "Modulo di accettazione", "In allegato il modulo da restituire firmato. ", Server.MapPath("Accettazione" & "_" & Session("Code") & ".pdf"))
                                InviaEmail("amministrazione@gransasso.it", Trim(dt.Rows(0).Item(0).ToString), "Modulo di accettazione", "In allegato il modulo da restituire firmato. ", Server.MapPath("Accettazione" & "_" & Session("Code") & "_" & Session("DataOra") & ".pdf"))
                                Response.Redirect("Default.aspx")
                                'Url = "~/default?ID=" & Request.QueryString("ID") & "&Code=" & Request.QueryString("Code")
                            End If
                        Else
                            Response.Redirect("NoAccess.aspx")
                        End If
                    End Using
                End Using

            End Using

        Else
            Label1.Text = "Codice OTP non valido"
            Label1.Visible = True
            'Session("Code") = Request.Cookies("CookieFacon").Values("Code").ToString
            'Session("NomeFacon") = Request.Cookies("CookieFacon").Values("NomeFacon").ToString
            'Session("ID") = Request.Cookies("CookieFacon").Values("ID").ToString
        End If

    End Sub
    Public Sub CreaCookieMaster()
        Dim aCookie As New HttpCookie("CookieFacon")
        aCookie.Values("Code") = Session("Code")
        aCookie.Values("ID") = Session("ID")
        aCookie.Values("NomeFacon") = Session("NomeFacon")
        aCookie.Values("lastVisit") = DateTime.Now.ToString()
        aCookie.Expires = DateTime.Now.AddDays(720)
        Response.Cookies.Add(aCookie)
    End Sub

    Private Sub Accettazione_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        TextBox1.Text = "L'attivazione del proprio profilo permetterà la visualizzazione delle commesse assegnate e il relativo prezzo per ciascun articolo. " &
        Session("NomeFacon") & " dichiara che l'emissione del documento di rientro della merce varrà quale accettazione delle condizioni di vendita e dei prezzi indicati. " &
        "Il Maglificio Gran Sasso spa concede l'utilizzo della piattaforma ad uso esclusivo dell'utente che non dovrà in alcun modo diffondere le informazioni presenti, nè condividere " &
        "con altri soggetti i dati e le credenziali di accesso. " & vbCrLf &
        Session("NomeFacon") & " dichiara inoltre di essere a conoscenza che la piattaforma prevede l'utilizzo di cookie tecnici ed autorizza sin d'ora l'utilizzo degli stessi. " &
        Session("NomeFacon") & " autorizza il Maglificio Gran Sasso spa ad inviare tutte le comunicazioni inerenti la gestione della piattaforma all'indirizzo email: " & Session("Email").ToString &
        vbCrLf & vbCrLf & vbCrLf & vbCrLf &
        "Sant'Egidio alla Vibrata, " & Format(Date.Now, "dd/MM/yyyy") & vbCrLf & vbCrLf &
        "Maglificio Gran Sasso spa                                                     " & Session("NomeFacon")



    End Sub

    Private Sub Accettazione_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("ID") = "" Then
            Response.Redirect("NoAccess.aspx")
        End If

    End Sub
    Private Sub InviaEmail(ByVal mittente As String, destinatario As String, oggetto As String, testo As String, Optional ByVal Allegato As String = "")
        Try
            Dim Smtp_Server As New SmtpClient
            Dim e_mail As New MailMessage()
            Smtp_Server.UseDefaultCredentials = False
            Smtp_Server.Credentials = New Net.NetworkCredential("postamgs@gransasso.it", "trxadmn")
            Smtp_Server.Port = 25
            Smtp_Server.EnableSsl = False
            Smtp_Server.Host = "posta.gransasso.it"

            e_mail = New MailMessage()
            e_mail.From = New MailAddress(mittente)
            e_mail.To.Add(destinatario)
            e_mail.CC.Add("robert.botis@gransasso.it")
            e_mail.Subject = oggetto
            e_mail.IsBodyHtml = False
            Dim myMessage As String = ""
            myMessage = myMessage & testo & vbCrLf & vbNewLine
            'myMessage = myMessage + "Regards" + Environment.NewLine
            'myMessage = myMessage & "Dopo il primo accesso il portale può essere raggiunto all'indirizzo " & vbCrLf
            'myMessage = myMessage & "http://facon.gransasso.it" & vbCrLf
            myMessage = myMessage & "Maglificio Gran Sasso spa"
            e_mail.Body = myMessage
            If Allegato <> "" Then
                Dim attachment As Attachment = New Attachment(Allegato)
                e_mail.Attachments.Add(attachment)
            End If

            Smtp_Server.Send(e_mail)

        Catch error_t As Exception

        End Try

    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim NumeroCasuale As New Random
        Session("random") = Right("000000" & NumeroCasuale.Next(1000000).ToString, 6)
        'InviaEmail("amministrazione@gransasso.it", "calcagnoli@gransasso.it", "Codice OTP", "Il codice OTP da utilizzare è: " & Session("random"))
        InviaEmail("amministrazione@gransasso.it", Session("Email"), "Codice OTP", "Il codice OTP da utilizzare è: " & Session("random"))
        TextBox2.Visible = True
        Button1.Visible = True

    End Sub
    Public Sub GeneraLetteraPdf()
        Dim n As Byte()

        Dim reader As PdfReader = New PdfReader(New RandomAccessFileOrArray(Request.MapPath("CartaIntestata.pdf")), n)
        Dim Size As iTextSharp.text.Rectangle = reader.GetPageSizeWithRotation(1)
        Using outStream As Stream = Response.OutputStream
            Session("DataOra") = Format(Now(), "yyyyMMddhhmmss")

            Dim Document As Document = New Document(Size)
            Dim writer As PdfWriter = PdfWriter.GetInstance(Document, New FileStream(Server.MapPath("Accettazione" & "_" & Session("Code") & "_" & Session("DataOra") & ".pdf"), FileMode.Create))

            Document.Open()
            Try

                Dim cb As PdfContentByte = writer.DirectContent

                cb.BeginText()
                Try
                    Dim ct As ColumnText = New ColumnText(cb)
                    ct.SetSimpleColumn(New Phrase(New Chunk(TextBox1.Text, FontFactory.GetFont(FontFactory.HELVETICA, 12, iTextSharp.text.Font.NORMAL))), 46, 600, 530, 36, 25, Element.ALIGN_LEFT)
                    ct.Go()
                    'cb.SetFontAndSize(BaseFont.CreateFont(), 12)
                    'cb.SetTextMatrix(40, 400)
                    'cb.ShowText(TextBox1.Text)

                    'cb.ShowTextAligned(1, TextBox1.Text, 40, 400, 0)

                Finally

                    cb.EndText()


                    Dim Page As PdfImportedPage = writer.GetImportedPage(reader, 1)
                    cb.AddTemplate(Page, 0, 0)

                End Try
            Finally

                Document.Close()
                writer.Close()
                reader.Close()
            End Try
        End Using
    End Sub
End Class