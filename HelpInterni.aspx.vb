Imports NPOI.SS.Formula.Functions

Public Class HelpInterni
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Admin") = "True" Then

        Else
            Response.Redirect("NoAccess.aspx")
        End If


    End Sub

End Class