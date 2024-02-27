Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Security.Policy

Public Class Login
    Inherits System.Web.UI.Page
    Public CodiceAuth As Long

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("ID") = "xcvqrybsudaR56@@76ddsaZWXQUDS" Then
            CreaCookieAdmin()
            Session("Admin") = "True"
        End If
        If Request.Cookies("CookieFaconAdmin") Is Nothing Then
            Response.Redirect("NoAccess.aspx")
        Else
            Session("Admin") = "True"
        End If
        'Dim UtenteLoggato As String = HttpContext.Current.User.Identity.Name
        'Select Case UCase(UtenteLoggato)
        '    Case "GRANSASSO\LUCA"
        '    Case "GRANSASSO\AGOSTINI"
        '    Case "GRANSASSO\RASETTI"
        '    Case Else
        '        Response.Redirect("NoAccess.aspx")
        'End Select
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim constr As String = ConfigurationManager.ConnectionStrings("NavConnectionString").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand("SELECT [E-Mail], [VAT Registration No_], [No_], Name FROM [MGS].[dbo].[MGS$Vendor] where No_ = '" & CodiceFornitore.Text.ToString & "'", con)
                Using sda As New SqlDataAdapter(cmd)
                    cmd.CommandType = CommandType.Text
                    Dim dt As New DataTable()
                    sda.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        CodiceAuth = CLng(dt.Rows(0).Item(1).ToString) + CLng(dt.Rows(0).Item(2).ToString)
                        CodiceAuth = CodiceAuth * 1952
                        CodiceAuth = CodiceAuth.ToString.Substring(0, 9)
                        Destinatario.Text = RTrim(dt.Rows(0).Item(0).ToString)
                        Label1.Text = RTrim(dt.Rows(0).Item(3).ToString)
                    End If
                End Using
            End Using
        End Using
        HyperLink1.Visible = True
        HyperLink1.NavigateUrl = "~/Default?ID=" & CodiceAuth & "&Code=" & CodiceFornitore.Text.ToString
        'Url.Text = "https://facon.gransasso.it/Default?ID=" & CodiceAuth & "&Code=" & CodiceFornitore.Text.ToString
        'Url.Visible = True
        Button2.Visible = True
        LblMittente.Visible = True
        Mittente.Visible = True
        LblDestinatario.Visible = True
        Destinatario.Visible = True
        Label1.Visible = True

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
            Dim myMessage As String = testo
            'myMessage = myMessage & testo & vbCrLf
            'myMessage = myMessage + "Regards" + Environment.NewLine
            'myMessage = myMessage & "Dopo il primo accesso il portale può essere raggiunto all'indirizzo " & vbCrLf
            'myMessage = myMessage & "https://facon.gransasso.it" & vbCrLf
            'myMessage = myMessage & "Maglificio Gran Sasso spa"
            e_mail.Body = myMessage
            'Dim attachment As Attachment = New Attachment(NomeFile)
            'e_mail.Attachments.Add(attachment)
            Smtp_Server.Send(e_mail)

        Catch error_t As Exception

        End Try

    End Sub
    Public Sub CreaCookieAdmin()
        Dim aCookie As New HttpCookie("CookieFaconAdmin")
        'aCookie.Values("Code") = Session("Code")
        'aCookie.Values("ID") = Session("ID")
        'aCookie.Values("NomeFacon") = Session("NomeFacon")
        aCookie.Values("lastVisit") = DateTime.Now.ToString()
        aCookie.Expires = DateTime.Now.AddDays(3600)
        Response.Cookies.Add(aCookie)
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim testo As String = "Per abilitare la propria utenza cliccare sul link seguente:" & vbCrLf

        HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery, "/")

        testo = testo & HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery, "/") & Replace(HyperLink1.NavigateUrl.ToString, "~/", "") & vbCrLf
        'myMessage = myMessage + "Regards" + Environment.NewLine
        testo = testo & "Dopo il primo accesso il portale può essere raggiunto all'indirizzo " & vbCrLf
        testo = testo & "https://facon.gransasso.it" & vbCrLf
        testo = testo & "Maglificio Gran Sasso spa"
        'Dim link As New HyperLink
        'link.NavigateUrl = HttpContext.Current.Request.Url.Host & "/Default?ID=" & CodiceAuth & "&Code=" & CodiceFornitore.Text.ToString
        InviaEmail(Mittente.Text, Destinatario.Text, "Link di accesso", testo)
    End Sub
End Class