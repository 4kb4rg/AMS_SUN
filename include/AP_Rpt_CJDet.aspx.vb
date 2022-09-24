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


Public Class AP_Rpt_CJDet : Inherits Page

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
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String

    Dim strSelLoc As String
    Dim strSelComp As String
    Dim strSelCJID As String


    Public Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")

        crvView.Visible = False

        strSelLoc = Trim(Request.QueryString("Location"))
        strSelComp = Trim(Request.QueryString("CompName"))
        strSelCJID = Trim(Request.QueryString("CJID"))

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


        strOpCd = "AP_STDRPT_CJ"
        strParamName = "CJID"
        strParamValue = strSelCJID


        strReportName = "AP_Rpt_CJDet.rpt"


        Try
            intErrNo = objGLRpt.mtdGetReport_KaryawanStaff(strOpCd, _
                                                strParamName, _
                                                strParamValue, _
                                                objRptDs, _
                                                objMapPath)

            objRptDs.Tables(0).TableName = "CJ_MASTER"
            objRptDs.Tables(1).TableName = "CJ_WEIGHING"
            objRptDs.Tables(2).TableName = "CJ_ADVANCE_PAYMENT"

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GET_SPK_INFO&errmesg=" & "" & "&redirect=../AP/trx/ap_trx_CJDet.aspx")
        End Try

        objRptDs1.Tables.Add(objRptDs.Tables(2).Copy())
        objRptDs1.Tables(0).TableName = "CJ_ADVANCE_PAYMENT"
        objRptDs1.Tables.Add(objRptDs.Tables(0).Copy())
        objRptDs1.Tables(1).TableName = "CJ_MASTER"
        objRptDs1.Tables.Add(objRptDs.Tables(1).Copy())
        objRptDs1.Tables(2).TableName = "CJ_WEIGHING"

        rdCrystalViewer.Load(objMapPath & "Web\EN\AP\Reports\Crystal\" & strReportName, OpenReportMethod.OpenReportByTempCopy)

        rdCrystalViewer.SetDataSource(objRptDs1)

        crvView.Visible = False
        crvView.ReportSource = rdCrystalViewer
        crvView.DataBind()

        crDiskFileDestinationOptions = New DiskFileDestinationOptions()
        crDiskFileDestinationOptions.DiskFileName = objMapPath & "web\ftp\AP_Rpt_CJDet.pdf"

        crExportOptions = rdCrystalViewer.ExportOptions
        With crExportOptions
            .DestinationOptions = crDiskFileDestinationOptions
            .ExportDestinationType = ExportDestinationType.DiskFile
            .ExportFormatType = ExportFormatType.PortableDocFormat
        End With

        rdCrystalViewer.Export()
        Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../ftp/AP_Rpt_CJDet.pdf"">")

    End Sub

    Sub PassParam()
       
    End Sub

End Class

