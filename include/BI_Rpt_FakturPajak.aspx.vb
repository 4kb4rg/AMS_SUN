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

Imports agri.IN.clsTrx
Imports agri.Admin.clsShare
Imports agri.Admin.clsComp
Imports agri.Admin.clsLoc
Imports agri.GlobalHdl.clsGlobalHdl
Imports agri.GL

Public Class BI_Rpt_FakturPajak : Inherits Page

    Protected WithEvents crvView As CrystalDecisions.Web.CrystalReportViewer
    Protected WithEvents dgResult As DataGrid
    Protected WithEvents lblErrMesage As Label

    Dim objBI As New agri.BI.clsTrx()
    Dim objAdmin As New agri.Admin.clsShare()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim rdCrystalViewer As New ReportDocument()
    Dim objGLRpt As New agri.GL.clsReport()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim strAccMonth As String
    Dim strAccYear As String

    
    Dim strInvoiceId As String
    Dim strContractNo As String
    Dim strPrintDate As String
    Dim strProduct As String
    Dim strPPN As String
    Dim strOptionNo As String
    Dim strOptionDesc As String
    Dim strFakturPajakNo As String
    Dim strFakturPajakDate As String

    Dim strCompName As String
    Dim strCompanyAddress As String
    Dim strLocName As String
    Dim strUserName As String

    Public Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_ARACCMONTH")
        strAccYear = Session("SS_ARACCYEAR")
        strCompName = Session("SS_COMPANYNAME")
        strCompanyAddress = Session("SS_COMPANYADDRESS")
        strLocName = Session("SS_LOCATIONNAME")
        strUserName = Session("SS_USERNAME")


        crvView.Visible = False

       
        strInvoiceId = Trim(Request.QueryString("InvoiceId"))
        strContractNo = Trim(Request.QueryString("ContractNo"))
        strProduct = Trim(Request.QueryString("Product"))
        strPPN = Trim(Request.QueryString("PPN"))
        strPrintDate = Now()
        strOptionNo = Trim(Request.QueryString("OptionNo"))
        strOptionDesc = Trim(Request.QueryString("OptionDesc"))
        strFakturPajakNo = Trim(Request.QueryString("FakturPajakNo"))
        strFakturPajakDate = Trim(Request.QueryString("FakturPajakDate"))

        Bind_ITEM(True)

    End Sub

    Sub Bind_ITEM(ByVal blnIsPDFFormat As Boolean)
        Dim crLogonInfo As CrystalDecisions.Shared.TableLogOnInfo
        Dim crDiskFileDestinationOptions As DiskFileDestinationOptions
        Dim crExportOptions As ExportOptions
        Dim objRptDs As New DataSet()
        Dim objMapPath As String
        Dim strOpCd As String
        Dim strParam As String = ""
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim ReportFileName As String
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim objFTPFolder As String

        strOpCd = "BI_CLSTRX_INVOICE_GET_REPORT"
        ReportFileName = "BI_Rpt_FakturPajak.rpt"
        strParamName = "INVOICEID|LOCCODE|FAKTURPAJAKNO|FAKTURPAJAKDATE"
        strParamValue = strInvoiceId & "|" & strLocation & "|" & strFakturPajakNo & "|" & strFakturPajakDate

        Try
            intErrNo = objGLRpt.mtdGetReportDataCommon(strOpCd, _
                                                strParamName, _
                                                strParamValue, _
                                                objRptDs, _
                                                objMapPath, objFTPFolder)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=&errmesg=" & lblErrMesage.Text & "&redirect=")
        End Try

        rdCrystalViewer.Load(objMapPath & "Web\EN\BI\Reports\Crystal\" & ReportFileName, OpenReportMethod.OpenReportByTempCopy)
        rdCrystalViewer.SetDataSource(objRptDs.Tables(0))

        crvView.Visible = False
        crvView.ReportSource = rdCrystalViewer
        crvView.DataBind()
        PassParam()

        crDiskFileDestinationOptions = New DiskFileDestinationOptions()

        crDiskFileDestinationOptions.DiskFileName = objFTPFolder & ReportFileName & ".pdf"
        'crDiskFileDestinationOptions.DiskFileName = objMapPath & "web\ftp\GL_Rpt_Journal_Detail.pdf"

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
        Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../" & strUrl & ReportFileName & ".pdf"">")

    End Sub

    Sub PassParam()
        Dim paramFields As New ParameterFields()
        Dim paramField1 As New ParameterField()
        Dim paramField2 As New ParameterField()
        Dim paramField3 As New ParameterField()
        Dim paramField4 As New ParameterField()
        Dim paramField5 As New ParameterField()
        Dim paramField6 As New ParameterField()

        Dim ParamDiscreteValue1 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue2 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue3 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue4 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue5 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue6 As New ParameterDiscreteValue()

        Dim crParameterValues1 As ParameterValues
        Dim crParameterValues2 As ParameterValues
        Dim crParameterValues3 As ParameterValues
        Dim crParameterValues4 As ParameterValues
        Dim crParameterValues5 As ParameterValues
        Dim crParameterValues6 As ParameterValues

        Dim crDataDef As DataDefinition
        Dim PFDefs As ParameterFieldDefinitions

        crDataDef = rdCrystalViewer.DataDefinition
        PFDefs = crDataDef.ParameterFields
        paramFields = crvView.ParameterFieldInfo

        paramField1 = paramFields.Item("CompName")
        paramField2 = paramFields.Item("Product")
        paramField3 = paramFields.Item("Contract")
        paramField4 = paramFields.Item("OptionNo")
        paramField5 = paramFields.Item("PPN")
        paramField6 = paramFields.Item("OptionDesc")

        crParameterValues1 = paramField1.CurrentValues
        crParameterValues2 = paramField2.CurrentValues
        crParameterValues3 = paramField3.CurrentValues
        crParameterValues4 = paramField4.CurrentValues
        crParameterValues5 = paramField5.CurrentValues
        crParameterValues6 = paramField6.CurrentValues

        ParamDiscreteValue1.Value = strCompName
        ParamDiscreteValue2.Value = strProduct
        ParamDiscreteValue3.Value = strContractNo
        ParamDiscreteValue4.Value = strOptionNo
        ParamDiscreteValue5.Value = strPPN
        ParamDiscreteValue6.Value = strOptionDesc

        crParameterValues1.Add(ParamDiscreteValue1)
        crParameterValues2.Add(ParamDiscreteValue2)
        crParameterValues3.Add(ParamDiscreteValue3)
        crParameterValues4.Add(ParamDiscreteValue4)
        crParameterValues5.Add(ParamDiscreteValue5)
        crParameterValues6.Add(ParamDiscreteValue6)

        PFDefs(0).ApplyCurrentValues(crParameterValues1)
        PFDefs(1).ApplyCurrentValues(crParameterValues2)
        PFDefs(2).ApplyCurrentValues(crParameterValues3)
        PFDefs(3).ApplyCurrentValues(crParameterValues4)
        PFDefs(4).ApplyCurrentValues(crParameterValues5)
        PFDefs(5).ApplyCurrentValues(crParameterValues6)

        crvView.ParameterFieldInfo = paramFields
    End Sub


End Class

