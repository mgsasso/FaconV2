Imports System.Data.SqlClient
Imports System.Net.Mail

Public Class Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim UtenteLoggato As String = HttpContext.Current.User.Identity.Name
        Select Case UCase(UtenteLoggato)
            Case "GRANSASSO\LUCA"
            Case "GRANSASSO\AGOSTINI"
            Case "GRANSASSO\RASETTI"
            Case Else
                Response.Redirect("NoAccess.aspx")
        End Select
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim constr As String = ConfigurationManager.ConnectionStrings("NavConnectionString").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand("SELECT [E-Mail], [VAT Registration No_], [No_] FROM [MGS].[dbo].[MGS$Vendor] where No_ = '" & CodiceFornitore.Text.ToString & "'", con)
                Using sda As New SqlDataAdapter(cmd)
                    cmd.CommandType = CommandType.Text
                    Dim dt As New DataTable()
                    sda.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        Dim CodiceAuth As Long = CInt(dt.Rows(0).Item(1).ToString) + CInt(dt.Rows(0).Item(2).ToString)
                        CodiceAuth = CodiceAuth * 1952
                        CodiceAuth = CodiceAuth.ToString.Substring(0, 9)
                        'InviaEmail("calcagnoli@gransasso.it", dt.Rows(0).Item(0).ToString, "Link di accesso", "https://app.gransasso.it/facon?ID=" & CodiceAuth & "&Code=" & CodiceFornitore.Text.ToString)
                        InviaEmail("calcagnoli@gransasso.it", "calcagnoli@gransasso.it", "Link di accesso", "https://app.gransasso.it/facon?ID=" & CodiceAuth & "&Code=" & CodiceFornitore.Text.ToString)
                    End If

                End Using
            End Using
        End Using
    End Sub


    Private Sub InviaEmail(ByVal mittente As String, destinatario As String, oggetto As String, testo As String)
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
            e_mail.Subject = oggetto
            e_mail.IsBodyHtml = False
            Dim myMessage As String = testo + vbCrLf
            'myMessage = myMessage + "Regards" + Environment.NewLine
            myMessage = myMessage + "Maglificio Gran Sasso spa"
            e_mail.Body = myMessage
            'Dim attachment As Attachment = New Attachment(NomeFile)
            'e_mail.Attachments.Add(attachment)
            Smtp_Server.Send(e_mail)

        Catch error_t As Exception

        End Try

    End Sub
End Class