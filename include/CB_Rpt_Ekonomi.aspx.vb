
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.XML
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web

Imports agri.PU.clsTrx
Imports agri.Admin.clsShare
Imports agri.GlobalHdl.clsGlobalHdl
Imports agri.PWSystem.clsLangCap

Public Class CB_Rpt_Ekonomi : Inherits page

    Protected WithEvents crvView As CrystalDecisions.Web.CrystalReportViewer
    Protected WithEvents dgResult As DataGrid
    Protected WithEvents lblErrMesage As Label

    Dim objCB As New agri.CB.clsTrx()
    Dim objAdmin As New agri.Admin.clsShare()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objLangCapDs As New Object()
    Dim rdCrystalViewer As New ReportDocument()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim strAccMonth As String
    Dim strAccYear As String

    Dim strStatus As String
    Dim strPayId As String
    Dim strPrintDate As String
    Dim strSortLine As String
    Dim strCBType As String

    Dim strCompName As String
    Dim strLocName As String
    Dim strUserName As String
    Dim strAccountTag As String
    Dim strVehTag As String
    Dim strVehExpTag As String
    Dim strBlockTag As String

    Dim strTotalAmount As String = ""
    Dim arrTotalAmount(15) As String
    Dim strBeforeDec As String = ""
    Dim strAfterDec As String = ""

    Public Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_CBACCMONTH")
        strAccYear = Session("SS_CBACCYEAR")
        strCompName = Session("SS_COMPANYNAME")
        strLocName = Session("SS_LOCATIONNAME")
        strUserName = Session("SS_USERNAME")

        crvView.Visible = False

        strStatus = Trim(Request.QueryString("strStatus"))
        strPayId = Trim(Request.QueryString("strId"))
        strCBType = Trim(Request.QueryString("CBType"))
        strPrintDate = Trim(Request.QueryString("strPrintDate"))
        strSortLine = Trim(Request.QueryString("strSortLine"))
        strAccountTag = Trim(Request.QueryString("strAccountTag"))
        strVehTag = Trim(Request.QueryString("strVehTag"))
        strVehExpTag = Trim(Request.QueryString("strVehExpTag"))
        strBlockTag = Trim(Request.QueryString("strBlockTag"))

        Bind_ITEM(True)

    End Sub

    Sub Bind_ITEM(ByVal blnIsPDFFormat As Boolean)
        Dim crLogonInfo As CrystalDecisions.Shared.TableLogOnInfo
        Dim objRptDs As New DataSet()
        Dim objMapPath As New Object()
        Dim strOpCd_Get As String = ""
        Dim strOpCd_GetLine As String = ""
        Dim strOpCodes As String
        Dim strParam As String = ""
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim strReportName As String
        Dim strPDFName As String

        If strCBType = "1" Then
            strOpCd_Get = "CB_CLSTRX_CASHBANKPAY_GET_FOR_DOCRPT" & "|" & "CashBank"
            strOpCd_GetLine = "CB_CLSTRX_CASHBANKPAY_LINE_GET_FOR_DOCRPT" & "|" & "CashBankLn"
        Else
            strOpCd_Get = "CB_CLSTRX_CASHBANKRCV_GET_FOR_DOCRPT" & "|" & "CashBank"
            strOpCd_GetLine = "CB_CLSTRX_CASHBANKRCV_LINE_GET_FOR_DOCRPT" & "|" & "CashBankLn"
        End If

        strReportName = "CB_Rpt_Ekonomi.rpt"

        strOpCodes = strOpCd_Get & Chr(9) & strOpCd_GetLine
        strParam = strPayId & "|" & strSortLine

        Try

            intErrNo = objCB.mtdGetCashBankDocRpt(strOpCodes, _
                                                 strParam, _
                                                 strCompany, _
                                                 strLocation, _
                                                 strUserId, _
                                                 strAccMonth, _
                                                 strAccYear, _
                                                 objRptDs)

        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=&errmesg=" & lblErrMesage.Text & "&redirect=")
        End Try

        Try
            intErrNo = objAdmin.mtdGetBasePath(objMapPath)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=&errmesg=" & lblErrMesage.Text & "&redirect=")
        End Try

        If objRptDs.Tables(0).Rows.Count > 0 Then
            strTotalAmount = CStr(objRptDs.Tables(0).Rows(0).Item("TotalAmount"))
            arrTotalAmount = Split(strTotalAmount, "")
            strBeforeDec = objGlobal.TerbilangDesimal((CDbl(arrTotalAmount(0))), "Rupiah")
            strTotalAmount = strBeforeDec

        End If

        rdCrystalViewer.Load(objMapPath & "Web\EN\CB\Reports\Crystal\" & strReportName, OpenReportMethod.OpenReportByTempCopy)
        rdCrystalViewer.SetDataSource(objRptDs)

        If Not blnIsPDFFormat Then
            crvView.Visible = True
            crvView.ReportSource = rdCrystalViewer
            crvView.DataBind()
            PassParam()
        Else
            crvView.Visible = False
            crvView.ReportSource = rdCrystalViewer
            crvView.DataBind()
            PassParam()
            Dim DiskOpts As CrystalDecisions.Shared.DiskFileDestinationOptions = New CrystalDecisions.Shared.DiskFileDestinationOptions()
            rdCrystalViewer.ExportOptions.ExportDestinationType = CrystalDecisions.[Shared].ExportDestinationType.DiskFile
            rdCrystalViewer.ExportOptions.ExportFormatType = CrystalDecisions.[Shared].ExportFormatType.PortableDocFormat
            DiskOpts.DiskFileName = objMapPath & "web\ftp\CB_RPT_Ekonomi.pdf"
            rdCrystalViewer.ExportOptions.DestinationOptions = DiskOpts
            rdCrystalViewer.Export()

            Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../ftp/CB_RPT_Ekonomi.pdf"">")
        End If

    End Sub

    Sub PassParam()
        Dim paramFields As New ParameterFields()
        Dim paramField1 As New ParameterField()
        Dim paramField2 As New ParameterField()
        Dim paramField3 As New ParameterField()
        Dim paramField4 As New ParameterField()

        Dim ParamDiscreteValue1 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue2 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue3 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue4 As New ParameterDiscreteValue()

        Dim crParameterValues1 As ParameterValues
        Dim crParameterValues2 As ParameterValues
        Dim crParameterValues3 As ParameterValues
        Dim crParameterValues4 As ParameterValues

        Dim crDataDef As DataDefinition
        Dim PFDefs As ParameterFieldDefinitions

        crDataDef = rdCrystalViewer.DataDefinition
        PFDefs = crDataDef.ParameterFields
        paramFields = crvView.ParameterFieldInfo

        paramField1 = paramFields.Item("strPrintedBy")
        paramField2 = paramFields.Item("strCompName")
        paramField3 = paramFields.Item("strLocName")
        paramField4 = paramFields.Item("TotalAmountSentence")

        crParameterValues1 = paramField1.CurrentValues
        crParameterValues2 = paramField2.CurrentValues
        crParameterValues3 = paramField3.CurrentValues
        crParameterValues4 = paramField4.CurrentValues
     
        ParamDiscreteValue1.Value = strUserName
        ParamDiscreteValue2.Value = strCompName
        ParamDiscreteValue3.Value = strLocName
        ParamDiscreteValue4.Value = strTotalAmount

        crParameterValues1.Add(ParamDiscreteValue1)
        crParameterValues2.Add(ParamDiscreteValue2)
        crParameterValues3.Add(ParamDiscreteValue3)
        crParameterValues4.Add(ParamDiscreteValue4)

        PFDefs(0).ApplyCurrentValues(crParameterValues1)
        PFDefs(1).ApplyCurrentValues(crParameterValues2)
        PFDefs(2).ApplyCurrentValues(crParameterValues3)
        PFDefs(3).ApplyCurrentValues(crParameterValues4)


        crvView.ParameterFieldInfo = paramFields
    End Sub



End Class

