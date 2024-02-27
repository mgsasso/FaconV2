Imports System.Data.SqlClient
Imports System.IO
Imports System.Security.Policy
Imports Ionic.Zip
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports NPOI.HSSF.UserModel
Imports NPOI.SS.UserModel
Imports NPOI.XSSF.UserModel
Imports Org.BouncyCastle.Utilities.Collections
Imports Telerik.Web.UI
Imports Telerik.Web.UI.ExportInfrastructure
Imports Telerik.Web.UI.GridExcelBuilder

Public Class ElencoDDT
    Inherits System.Web.UI.Page
    Public Url As String
    Public dt As DataTable
    Public dt1 As DataTable
    Public constrNav As String = ConfigurationManager.ConnectionStrings("NavConnectionString").ConnectionString
    Public constrArxivar As String = ConfigurationManager.ConnectionStrings("ArxivarConnectionString").ConnectionString
    Public constrFacon As String = ConfigurationManager.ConnectionStrings("FaconConnectionString").ConnectionString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.Cookies("CookieFacon") Is Nothing Then
            Response.Redirect("NoAccess.aspx")
        End If
        If Not Request.Cookies("CookieFaconAdmin") Is Nothing Then
            Session("Admin") = "True"
        End If

        RecuperaElencoFile()
        RecuperaDati()

        If Not Me.IsPostBack Then

            GridView1.DataSource = dt
            GridView1.DataBind()
            GridView1.AllowPaging = True
            If GridView1.Rows.Count > 0 Then
                CheckBox1.Visible = True
            Else
                CheckBox1.Visible = False
            End If

            GridView2.DataSource = dt1
            GridView2.DataBind()


        End If
    End Sub


    Public Sub RecuperaElencoFile()
        Dim ds1 As New DataSet
        Dim objConn As New SqlClient.SqlConnection(constrArxivar)
        objConn.Open()
        Dim Strsql As String = ""
        Strsql = "SELECT DOCNUMBER as ID, DOCNAME as OGGETTO, NUMERO, format(DATADOC, 'dd/MM/yyyy') as DATA  FROM [ARCHDB].[dbo].[DM_PROFILE] where TIPO2 = 154 and STATO = 'VALIDO' and TESTO2_4 = '" & Session("Code") & "' order by DATADOC desc, NUMERO DESC"
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
            'VisualizzaPdf(item, "\\serverdoc\hotfolder\TempScansioni\")
            VisualizzaPdf(item, Server.MapPath("/FileCaricati/"))
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "openModal", "window.open('" + Replace("~/FileCaricati/" & Session("NomeFilePdfBolla"), "~/", "") + "','_blank');", True)
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
                Dim update As String = "update t1 set IDDDT = null from TblDDTInviati t1 inner join TblDDTFacon t2 on t1.IDDDT = t2.ID where NumeroBolla = " & row.Cells(2).Text & " and substring(cast(DataBolla as varchar(8)),1,4) = " & row.Cells(3).Text.Substring(6, 4) & " and t2.CodiceFornitore = '" & Session("Code") & "'"

                Using cmd1 As New SqlCommand(update)
                    cmd1.Connection = con
                    cmd1.ExecuteNonQuery()
                End Using
                Dim query As String = "delete from TblDDTFacon where NumeroBolla = " & row.Cells(2).Text & " and substring(cast(DataBolla as varchar(8)),1,4) = " & row.Cells(3).Text.Substring(6, 4) & " and CodiceFornitore = '" & Session("Code") & "'"
                Using cmd As New SqlCommand(query)
                    cmd.Connection = con
                    cmd.ExecuteNonQuery()
                End Using

                con.Close()
            End Using
            Using con As New SqlConnection(constrArxivar)
                con.Open()
                Dim update As String = "update DM_PROFILE set STATO = 'ANNULLATO' where DOCNUMBER = " & row.Cells(0).Text
                Using cmd1 As New SqlCommand(update)
                    cmd1.Connection = con
                    cmd1.ExecuteNonQuery()
                End Using
                con.Close()
            End Using
            Response.Redirect("ElencoDDT.aspx")

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

                'Dim document As New Document(PageSize.A4)
                ''Use a memory string so we don't need to write to disk
                'Using outputStream As New MemoryStream()
                '    'Associate the PDF with the stream
                '    Dim w = PdfWriter.GetInstance(document, outputStream)

                '    'Open the PDF for writing'
                '    document.Open()

                '    'Do PDF stuff Here'
                Session("NomeFilePdfBolla") = e.FileName.ToString

                '    Dim bgReader As PdfReader = New PdfReader(Path & e.FileName.ToString)
                '    Dim bg As PdfImportedPage
                '    Dim cb As PdfContentByte = w.DirectContent
                '    For i = 1 To bgReader.NumberOfPages
                '        document.NewPage()
                '        bg = w.GetImportedPage(bgReader, i)


                '        '' add the template beneath content
                '        cb.AddTemplate(bg, 0, 0)
                '        'w.DirectContentUnder.AddTemplate(bg, 0, 0)

                '        'Close the PDF'
                '    Next
                '    document.Close()
                '    'Clear the response buffer'
                '    'Response.Clear()
                '    ''Set the output type as a PDF'
                '    'Response.ContentType = "application/pdf"
                '    ''Response.ContentType = "application/octet-stream"
                '    ''Disable caching'
                '    'Response.AddHeader("Expires", "0")
                '    'Response.AddHeader("Cache-Control", "")
                '    ''Set the filename'
                '    'Response.AddHeader("Content-Disposition", "inline; filename=" & UCase(e.FileName.ToString))
                '    ''Set the length of the file so the browser can display an accurate progress bar'
                '    'Response.AddHeader("Content-length", outputStream.GetBuffer().Length.ToString())
                '    ''Response.AddHeader(“Accept-Header”, outputStream.GetBuffer().Length.ToString())
                '    ''Write the contents of the memory stream'
                '    'Response.OutputStream.Write(outputStream.GetBuffer(), 0, outputStream.GetBuffer().Length)

                '    ''Close the response stream'
                '    ''Response.Buffer = True
                '    ''Response.Clear()
                '    ''Response.End()
                '    'bgReader.Close()
                '    ''w.Close()
                '    'outputStream.Close()
                '    'File.Delete(Path & e.FileName.ToString)

                '    'HttpContext.Current.Response.ClearContent()
                '    'HttpContext.Current.Response.ClearHeaders()
                '    'HttpContext.Current.Response.ContentType = "application/pdf"
                '    'HttpContext.Current.Response.TransmitFile(Path & e.FileName.ToString)
                '    'HttpContext.Current.Response.Flush()
                '    'HttpContext.Current.Response.Close()
                '    'File.Delete(Path & e.FileName.ToString)


                'End Using



            Next
        End Using
    End Sub

    Private Sub GridView1_Sorting(sender As Object, e As GridViewSortEventArgs) Handles GridView1.Sorting

    End Sub
    Public Sub RecuperaDati()
        Dim ds1 As New DataSet
        Dim constr As String = ConfigurationManager.ConnectionStrings("FaconConnectionString").ConnectionString
        Dim objConn As New SqlClient.SqlConnection(constr)
        objConn.Open()
        Dim Strsql As String = ""
        Strsql = "SELECT Format(Sum(t1.QT),'N0', 'de-de') as capi, Format(Sum(t1.QT*t1.PrezzoUnitario), 'N','de-de') as Valore, Left(t1.DataConsegna,4) as Anno, Substring(cast(t1.DataConsegna as varchar(8)),5,2) as Mese  FROM [Facon].[dbo].[TblDDTFacon] t1  inner join [Facon].[dbo].[TblDDTInviati] t2 on t1.id = t2.idddt where t1.CodiceFornitore = '" & Session("Code") & "' and t2.CausaleLavorazione <> 2 group by Left(DataConsegna,4), Substring(cast(DataConsegna as varchar(8)),5,2)   order by Anno desc, cast(Substring(cast(DataConsegna as varchar(8)),5,2) as int) desc"
        Dim dataAdapter As New SqlClient.SqlDataAdapter(Strsql, objConn)
        ds1.EnforceConstraints = False
        dataAdapter.FillSchema(ds1, SchemaType.Source, "File")
        dataAdapter.Fill(ds1, "File")
        objConn.Close()
        dt1 = ds1.Tables(0)
    End Sub



    Protected Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If Data.Text.ToString = "" Then
            Label1.Text = "Selezionare un mese da elaborare!"
            Label1.Visible = True
        Else
            Label1.Visible = False
            Dim MeseRiferimento As String = Data.Text.ToString.Substring(3, 4) & Data.Text.ToString.Substring(0, 2)
            If Session("Admin") = "True" Then
                ExporttoExcelNPOI(MeseRiferimento, "")
            Else
                ExporttoExcelNPOI(MeseRiferimento, Session("Code"))
            End If

        End If
    End Sub
    Public Sub ExporttoExcelNPOI(ByVal MeseRiferimento As String, CodiceFornitore As String)
        Dim NomeFile As String = ""
        If Session("Admin") = "True" Then
            NomeFile = "DettaglioFacon" & "_" & MeseRiferimento
        Else
            NomeFile = "DettaglioFacon" & "_" & MeseRiferimento & "_" & Session("Code")
        End If
        Dim conString As String = ConfigurationManager.ConnectionStrings("FaconConnectionString").ConnectionString
        Using con As New SqlConnection(conString)
            Using cmd As New SqlCommand("CreaDettaglioLavorazioni")
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandTimeout = 60
                cmd.Parameters.Add(New SqlParameter("@ANNOMESE", MeseRiferimento))
                cmd.Parameters.Add(New SqlParameter("@CODICEFORNITORE", CodiceFornitore))
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        sda.Fill(dt)
                        WriteExcelWithNPOI(dt, "xlsx", NomeFile)

                    End Using
                End Using
            End Using
        End Using


    End Sub
    Public Sub WriteExcelWithNPOI(ByVal dt As DataTable, ByVal extension As String, ByVal FileName As String)

        Dim workbook As IWorkbook

        If extension = "xlsx" Then
            workbook = New XSSFWorkbook()
        ElseIf (extension = "xls") Then
            workbook = New HSSFWorkbook()
        Else
            Throw New Exception("This format is not supported")
        End If

        Dim sheet1 As ISheet = workbook.CreateSheet(FileName)

        'make a header row
        Dim row1 As IRow = sheet1.CreateRow(0)

        For j As Integer = 0 To dt.Columns.Count - 1

            Dim cell As ICell = row1.CreateCell(j)
            Dim columnName As String = dt.Columns(j).ToString()
            cell.SetCellValue(columnName)

        Next
        'loops through data
        For i As Integer = 0 To dt.Rows.Count - 1

            Dim Row As IRow = sheet1.CreateRow(i + 1)
            For j As Integer = 0 To dt.Columns.Count - 1


                Dim cell As ICell = Row.CreateCell(j)
                Dim columnName As String = dt.Columns(j).ToString()
                Select Case columnName.ToString
                    Case "PrezzoUnitario", "QT", "TotaleRiga"
                        cell.SetCellValue(CDbl(dt.Rows(i)(columnName).ToString))
                    Case Else
                        cell.SetCellValue(dt.Rows(i)(columnName).ToString)
                End Select
            Next
        Next
        Dim exportData = New MemoryStream()
        Using exportData

            Response.Clear()
            workbook.Write(exportData)
            If (extension = "xlsx") Then

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", FileName.ToString & ".xlsx"))
                Response.BinaryWrite(exportData.ToArray())

            ElseIf (extension = "xls") Then

                Response.ContentType = "application/vnd.ms-excel"
                Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", FileName.ToString & ".xls"))
                Response.BinaryWrite(exportData.GetBuffer())
            End If
            Response.End()
        End Using

    End Sub

    Private Sub GridView1_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        GridView1.DataSource = dt
        GridView1.DataBind()
    End Sub

    Private Sub GridView2_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GridView2.PageIndexChanging
        GridView2.PageIndex = e.NewPageIndex
        GridView2.DataSource = dt
        GridView2.DataBind()
    End Sub
End Class