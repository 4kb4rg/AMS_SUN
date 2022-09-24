
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

Imports agri.CM.clsTrx
Imports agri.Admin.clsShare
Imports agri.GlobalHdl.clsGlobalHdl


Public Class CM_Rpt_ContractFFBRegDet : Inherits page

    Protected WithEvents crvView As CrystalDecisions.Web.CrystalReportViewer
    Protected WithEvents lblErrMesage As Label

    Dim objCMRpt As New agri.CM.clsReport
    Dim objCMTrx As New agri.CM.clsTrx
    Dim objWMTrx As New agri.WM.clsTrx
    Dim objAdmin As New agri.Admin.clsShare()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()

    Dim rdCrystalViewer As New ReportDocument()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim intErrNo As Integer
    Dim strTitle As String
    Dim strContractNo As String

    Dim strExportToExcel As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim StrPricingMtd As String
    Dim strPrintedBy As String
    Dim objOk As New agri.GL.clsReport

    Public Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")

        crvView.Visible = False

        'strAccMonth = Server.UrlDecode(Request.QueryString("AccMonth").Trim())
        'strAccYear = Server.UrlDecode(Request.QueryString("AccYear").Trim())
        'StrPricingMtd = Server.UrlDecode(Request.QueryString("pricingMtd").Trim())

        Bind_ITEM(True)

    End Sub

    Sub Bind_ITEM(ByVal blnIsPDFFormat As Boolean)

        Dim objRptDs As New DataSet()
        Dim objRpt As New DataSet()
        Dim objMapPath As String
        Dim crExportOptions As ExportOptions
        Dim crDiskFileDestinationOptions As DiskFileDestinationOptions
        Dim strOpCd As String = "CM_STDRPT_CONTRACT_REG_FFB_LIST"

        Dim strParamName As String
        Dim strParamValue As String
        Dim strterbilang As String = ""
        Dim totalrp As String = ""
        Dim ReportID As String = Request.QueryString("RptID")
        Dim strExportToExcel As String = "0"
        Dim objFTPFolder As String

        Dim strYear As String
        Dim strMonth As String
        Dim strLocCode As String

        strMonth = strAccMonth
        strYear = strAccYear
        strLocCode = strLocation

        strParamName = "ACCYEAR|ACCMONTH|LOCCODE|STRSEARCH"
        strParamValue = strYear & "|" & _
                        strMonth & "|" & _
                        strLocCode & "|" & "AND ((A.PricingMtd='" & StrPricingMtd & "') OR ('" & StrPricingMtd & "'='0'))"
        Try
            intErrNo = objOk.mtdGetReportDataCommon(strOpCd, strParamName, strParamValue, objRptDs, objMapPath, objFTPFolder)
        Catch Exp As System.Exception
            Response.Redirect("../../include/mesg/ErrorMessage.aspx?errcode=CM_STDRPT_CONTRACT_REG_FFB_LIST&errmesg=" & lblErrMesage.Text & "&redirect=")
        End Try

        'rdCrystalViewer.Load(objMapPath & "web\" & strLangCode & "\reports\crystal\CM_Rpt_ContractRegFFB.rpt", OpenReportMethod.OpenReportByTempCopy)

        rdCrystalViewer.Load(objMapPath & "Web\EN\CM\Reports\Crystal\CM_Rpt_ContractRegFFB.rpt", OpenReportMethod.OpenReportByTempCopy)
        rdCrystalViewer.SetDataSource(objRptDs.Tables(0))

        crvView.Visible = False
        crvView.ReportSource = rdCrystalViewer
        crvView.DataBind()

        crDiskFileDestinationOptions = New DiskFileDestinationOptions()
        If strExportToExcel = "0" Then
            crDiskFileDestinationOptions.DiskFileName = objMapPath & "web\ftp\CM_Rpt_ContractRegFFB.pdf"
        Else
            crDiskFileDestinationOptions.DiskFileName = objMapPath & "web\ftp\CM_Rpt_ContractRegFFB.xls"
        End If

        crExportOptions = rdCrystalViewer.ExportOptions

        crExportOptions = rdCrystalViewer.ExportOptions
        With crExportOptions
            .DestinationOptions = crDiskFileDestinationOptions
            .ExportDestinationType = ExportDestinationType.DiskFile
            If strExportToExcel = "0" Then
                .ExportFormatType = ExportFormatType.PortableDocFormat
            Else
                .ExportFormatType = ExportFormatType.Excel
            End If
        End With

        rdCrystalViewer.Export()
        rdCrystalViewer.Close()
        rdCrystalViewer.Dispose()

        If strExportToExcel = "0" Then
            Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../ftp/CM_Rpt_ContractRegFFB.pdf"">")
        Else
            Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../ftp/CM_Rpt_ContractRegFFB.pdf"">")
        End If
    End Sub
    Sub PassParam()
        Dim paramFields As New ParameterFields()
        Dim paramField1 As New ParameterField()
        Dim paramField2 As New ParameterField()

        Dim ParamDiscreteValue1 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue2 As New ParameterDiscreteValue()


        Dim crParameterValues1 As ParameterValues
        Dim crParameterValues2 As ParameterValues

        Dim crDataDef As DataDefinition
        Dim PFDefs As ParameterFieldDefinitions

        crDataDef = rdCrystalViewer.DataDefinition
        PFDefs = crDataDef.ParameterFields
        paramFields = crvView.ParameterFieldInfo

        paramField1 = paramFields.Item("CompanyName")
        paramField2 = paramFields.Item("strPrintedBy")

        crParameterValues1 = paramField1.CurrentValues
        crParameterValues2 = paramField2.CurrentValues

        ParamDiscreteValue1.Value = Session("SS_COMPANYNAME")
        ParamDiscreteValue2.Value = strUserId

        crParameterValues1.Add(ParamDiscreteValue1)
        crParameterValues2.Add(ParamDiscreteValue2)

        PFDefs(0).ApplyCurrentValues(crParameterValues1)
        PFDefs(1).ApplyCurrentValues(crParameterValues2)

        crvView.ParameterFieldInfo = paramFields
    End Sub

End Class

