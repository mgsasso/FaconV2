Public Class ConfermaRientro
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub ConfermaRientro_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Data.Text = Format(Date.Now, "dd/MM/yyyy")
    End Sub
End Class