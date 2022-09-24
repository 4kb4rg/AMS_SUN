Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic
Imports System.XML
Imports System.Web.Services
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web

Imports agri.GL
Imports agri.Admin.clsShare
Imports agri.GlobalHdl.clsGlobalHdl
Imports agri.PWSystem.clsLangCap


Public Class CB_Rpt_RekonsileDet : Inherits Page

    Protected WithEvents crvView As CrystalDecisions.Web.CrystalReportViewer
    Protected WithEvents dgResult As DataGrid
    Protected WithEvents lblErrMesage As Label

    Dim objGLRpt As New agri.GL.clsReport()
    Dim objAdmin As New agri.Admin.clsShare()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objLangCapDs As New Object()
    Dim rdCrystalViewer As New ReportDocument()

    Dim strCompany As String
    Dim strCompanyName As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String

    Dim strSelTRXID As String


    Public Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strCompanyName = Session("SS_COMPANYNAME")

        crvView.Visible = False

        strSelTRXID = Trim(Request.QueryString("strTrxId"))

        Bind_ITEM(True)

    End Sub

    Sub Bind_ITEM(ByVal blnIsPDFFormat As Boolean)

        Dim crLogonInfo As CrystalDecisions.Shared.TableLogOnInfo
        Dim crDiskFileDestinationOptions As DiskFileDestinationOptions
        Dim crExportOptions As ExportOptions
        Dim objRptDs As New Dataset()
        Dim objRptDs1 As New DataSet
        Dim objMapPath As String
        Dim strParam As String = ""
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim strReportName As String
        Dim strPDFName As String
        Dim strOpCd As String = ""
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim objFTPFolder As String


        strOpCd = "CB_STDRPT_REKONSILE_DET"
        strParamName = "TRXID|COMPNAME"
        strParamValue = strSelTRXID & "|" & strCompanyName



        strReportName = "CB_Rpt_Rekonsile_Det.rpt"


        Try
            intErrNo = objGLRpt.mtdGetReport_KaryawanStaff(strOpCd, _
                                                strParamName, _
                                                strParamValue, _
                                                objRptDs, _
                                                objMapPath, _
                                                objFTPFolder)

            objRptDs.Tables(0).TableName = "REKONSILE_HEADER"
            objRptDs.Tables(1).TableName = "REKONSILE_DETAIL"

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GET_REPORTINFO&errmesg=" & "" & "&redirect=../CB/trx/cb_trx_RekonsileDet.aspx")
        End Try

        objRptDs1.Tables.Add(objRptDs.Tables(1).Copy())
        objRptDs1.Tables(0).TableName = "REKONSILE_DETAIL"
        objRptDs1.Tables.Add(objRptDs.Tables(0).Copy())
        objRptDs1.Tables(1).TableName = "REKONSILE_HEADER"
        

        rdCrystalViewer.Load(objMapPath & "Web\EN\CB\Reports\Crystal\" & strReportName, OpenReportMethod.OpenReportByTempCopy)

        rdCrystalViewer.SetDataSource(objRptDs1)

        crvView.Visible = False
        crvView.ReportSource = rdCrystalViewer
        crvView.DataBind()

        crDiskFileDestinationOptions = New DiskFileDestinationOptions()
        crDiskFileDestinationOptions.DiskFileName = objFTPFolder & "CB_Rpt_Rekonsile_Det.pdf"
        'crDiskFileDestinationOptions.DiskFileName = objMapPath & "web\ftp\CB_Rpt_Rekonsile_Det.pdf"

        crExportOptions = rdCrystalViewer.ExportOptions
        With crExportOptions
            .DestinationOptions = crDiskFileDestinationOptions
            .ExportDestinationType = ExportDestinationType.DiskFile
            .ExportFormatType = ExportFormatType.PortableDocFormat
        End With

        rdCrystalViewer.Export()

        Dim strUrl As String
        strUrl = objFTPFolder.Substring(InStr(objFTPFolder, "ftp") - 1, Len(objFTPFolder) - InStr(objFTPFolder, "ftp") + 1)
        strUrl = Replace(strUrl, "\", "/")

        Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../" & strUrl & "CB_Rpt_Rekonsile_Det.pdf"">")
        'Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../ftp/CB_Rpt_Rekonsile_Det.pdf"">")

    End Sub

    Sub PassParam()

    End Sub

End Class
