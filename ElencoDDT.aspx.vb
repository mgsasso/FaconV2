Imports System.Data.SqlClient
Imports System.IO
Imports System.Security.Policy
Imports Ionic.Zip
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Public Class ElencoDDT
    Inherits System.Web.UI.Page
    Public Url As String
    Public dt As DataTable
    Public constrNav As String = ConfigurationManager.ConnectionStrings("NavConnectionString").ConnectionString
    Public constrArxivar As String = ConfigurationManager.ConnectionStrings("ArxivarConnectionString").ConnectionString
    Public constrFacon As String = ConfigurationManager.ConnectionStrings("FaconConnectionString").ConnectionString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Using con As New SqlConnection(constrNav)
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
                            Url = "~/elencoDDT?ID=" & Request.QueryString("ID") & "&Code=" & Request.QueryString("Code")
                        End If
                    Else
                        Response.Redirect("NoAccess.aspx")
                    End If
                End Using
            End Using
        End Using
        If Not Me.IsPostBack Then
            RecuperaElencoFile()
            GridView1.DataSource = dt
            GridView1.DataBind()

        End If
    End Sub
    Public Sub CreaCookieMaster()
        Dim aCookie As New HttpCookie("CookieFacon")
        aCookie.Values("Code") = Request.QueryString("Code")
        aCookie.Values("lastVisit") = DateTime.Now.ToString()
        aCookie.Expires = DateTime.Now.AddDays(720)
        Response.Cookies.Add(aCookie)
    End Sub

    Public Sub RecuperaElencoFile()
        Dim ds1 As New DataSet
        Dim objConn As New SqlClient.SqlConnection(constrArxivar)
        objConn.Open()
        Dim Strsql As String = ""
        Strsql = "SELECT DOCNUMBER as ID, DOCNAME as OGGETTO, NUMERO, format(DATADOC, 'dd/MM/yyyy') as DATA  FROM [ARCHDB].[dbo].[DM_PROFILE] where TIPO2 = 154 and TESTO2_4 = '" & Session("Code") & "'"
        Dim dataAdapter As New SqlClient.SqlDataAdapter(Strsql, objConn)
        ds1.EnforceConstraints = False
        dataAdapter.FillSchema(ds1, SchemaType.Source, "File")
        dataAdapter.Fill(ds1, "File")
        objConn.Close()
        dt = ds1.Tables(0)
    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand

        If e.CommandName = "Visualizza" Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)
            Dim item As Integer
            item = Server.HtmlDecode(row.Cells(0).Text)
            Session("SystemId") = item
            'Response.Redirect("OpenPdf.aspx")
            'Response.Write("<script>window.open('OpenPdf.aspx','_blank');</script>")
            'Dim pdfFolder As String = "PdfFiles"
            VisualizzaPdf(item, "\\serverdoc\hotfolder\Lul\Temp\")
        End If
        If e.CommandName = "Elimina" And CheckBox1.Checked Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = GridView1.Rows(index)
            ' Create a new ListItem object for the contact in the row.     
            Dim item As Integer
            'item = row.Cells(1).Text
            item = Server.HtmlDecode(row.Cells(0).Text)
            Using con As New SqlConnection(constrFacon)
                con.Open()
                Dim update As String = "update t1 set IDDDT = null from TblDDTInviati t1 inner join TblDDTFacon t2 on t1.IDDDT = t2.ID where NumeroBolla = " & row.Cells(2).Text & " and substring(cast(DataBolla as varchar(8)),1,4) = " & row.Cells(3).Text.Substring(6, 4) & " and CodiceFornitore = '" & Request.QueryString("Code") & "'"

                Using cmd1 As New SqlCommand(update)
                    cmd1.Connection = con
                    cmd1.ExecuteNonQuery()
                End Using
                Dim query As String = "delete from TblDDTFacon where NumeroBolla = " & row.Cells(2).Text & " and substring(cast(DataBolla as varchar(8)),1,4) = " & row.Cells(3).Text.Substring(6, 4) & " and CodiceFornitore = '" & Request.QueryString("Code") & "'"
                Using cmd As New SqlCommand(query)
                    cmd.Connection = con
                    cmd.ExecuteNonQuery()
                End Using

                con.Close()
            End Using
            Response.Redirect(Url)

        End If
    End Sub


    Public Function RecuperaPercorsoFileArxivar(ByVal docnumber As Integer) As DataRow
        Dim ds1 As New DataSet

        Dim objConn As New SqlClient.SqlConnection(constrArxivar)
        objConn.Open()
        Dim dataAdapter As New SqlClient.SqlDataAdapter("SELECT PATH, FILENAME, DOCNAME, DOCNUMBER FROM DM_PROFILE WHERE DOCNUMBER = " & docnumber & " ", objConn)
        ds1.EnforceConstraints = False
        dataAdapter.FillSchema(ds1, SchemaType.Source, "PathFileArxivar")
        dataAdapter.Fill(ds1, "PathFileArxivar")
        objConn.Close()
        Return ds1.Tables(0).Rows(0)
    End Function

    Public Sub VisualizzaPdf(ByVal docnumber As Integer, ByVal Path As String, Optional ByVal NuovoNome As String = "")
        Dim dr As DataRow
        dr = RecuperaPercorsoFileArxivar(docnumber)
        Dim ZipToUnpack As String = Replace(dr.Item("PATH"), "E:\", "\\serverdoc\") & "\" & dr.Item("FILENAME")
        Dim UnpackDirectory As String = Path
        Using zip1 As ZipFile = ZipFile.Read(ZipToUnpack)
            zip1.Password = "ARX_gransasso"
            Dim e As ZipEntry
            ' here, we extract every entry, but we could extract conditionally,
            ' based on entry name, size, date, checkbox status, etc.   
            For Each e In zip1
                e.Extract(UnpackDirectory, ExtractExistingFileAction.OverwriteSilently)

                Dim document As New Document(PageSize.A4)
                'Use a memory string so we don't need to write to disk
                Using outputStream As New MemoryStream()
                    'Associate the PDF with the stream
                    Dim w = PdfWriter.GetInstance(document, outputStream)

                    'Open the PDF for writing'
                    document.Open()

                    'Do PDF stuff Here'
                    Dim bgReader As PdfReader = New PdfReader(Path & e.FileName.ToString)
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
                    document.Close()
                    'Clear the response buffer'
                    Response.Clear()
                    'Set the output type as a PDF'
                    Response.ContentType = "application/pdf"
                    'Response.ContentType = "application/octet-stream"
                    'Disable caching'
                    Response.AddHeader("Expires", "0")
                    Response.AddHeader("Cache-Control", "")
                    'Set the filename'
                    Response.AddHeader("Content-Disposition", "inline; filename=" & UCase(e.FileName.ToString))
                    'Set the length of the file so the browser can display an accurate progress bar'
                    Response.AddHeader("Content-length", outputStream.GetBuffer().Length.ToString())
                    'Response.AddHeader(“Accept-Header”, outputStream.GetBuffer().Length.ToString())
                    'Write the contents of the memory stream'
                    Response.OutputStream.Write(outputStream.GetBuffer(), 0, outputStream.GetBuffer().Length)

                    'Close the response stream'
                    'Response.Buffer = True
                    'Response.Clear()
                    'Response.End()
                    bgReader.Close()
                    'w.Close()
                    outputStream.Close()
                    File.Delete(Path & e.FileName.ToString)

                    'HttpContext.Current.Response.ClearContent()
                    'HttpContext.Current.Response.ClearHeaders()
                    'HttpContext.Current.Response.ContentType = "application/pdf"
                    'HttpContext.Current.Response.TransmitFile(Path & e.FileName.ToString)
                    'HttpContext.Current.Response.Flush()
                    'HttpContext.Current.Response.Close()
                    'File.Delete(Path & e.FileName.ToString)


                End Using



            Next
        End Using
    End Sub

End Class