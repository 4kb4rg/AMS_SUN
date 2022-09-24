
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

Public Class CB_Rpt_CashBankPrint : inherits page

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
    Dim strReportName As String
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

        'If Left(Trim(strLocName), 2) = "PT" Then
        '    strCompName = strLocName
        '    strLocName = ""
        'End If

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
        strReportName = Trim(Request.QueryString("strReportName"))

        Bind_ITEM(True)

    End Sub

    Sub Bind_ITEM(ByVal blnIsPDFFormat As Boolean)
        Dim crLogonInfo As CrystalDecisions.Shared.TableLogOnInfo
        Dim objRptDs As New Dataset()
        Dim objMapPath As New Object()
        Dim strOpCd_Get As String = ""
        Dim strOpCd_GetLine As String = ""
        Dim strOpCodes As String
        Dim strParam As String = ""
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim strPDFName As String
        Dim totalrp As String
        Dim objFTPFolder As String

        If strCBType = "1" then
            strOpCd_Get = "CB_CLSTRX_CASHBANKPAY_GET_FOR_DOCRPT" & "|" & "CashBank"
            strOpCd_GetLine = "CB_CLSTRX_CASHBANKPAY_LINE_GET_FOR_DOCRPT" & "|" & "CashBankLn"
        ElseIf strCBType = "2" Then
            strOpCd_Get = "CB_CLSTRX_CASHBANKRCV_GET_FOR_DOCRPT" & "|" & "CashBank"
            strOpCd_GetLine = "CB_CLSTRX_CASHBANKRCV_LINE_GET_FOR_DOCRPT" & "|" & "CashBankLn"
        Else
            strOpCd_Get = "CB_CLSTRX_CASHBANKRCV_GET_FOR_DOCRPT" & "|" & "CashBank"
            strOpCd_GetLine = "CB_CLSTRX_CASHBANKPAY_LINE_GET_FOR_DOCRPT" & "|" & "CashBankLn"
			'strOpCd_GetLine = "CB_CLSTRX_CASHBANKRCV_LINE_GET_FOR_KWITANSI_DOCRPT" & "|" & "CashBankLn"
        End If

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
                                                 objRptDs, objMapPath, objFTPFolder)

        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=&errmesg=" & lblErrMesage.Text & "&redirect=")        
        End Try

        'Try
        '    intErrNo = objAdmin.mtdGetBasePath(objMapPath)
        'Catch Exp As Exception
        '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=&errmesg=" & lblErrMesage.Text & "&redirect=")
        'End Try

        If objRptDs.Tables(0).Rows.Count > 0 Then
            'strTotalAmount = CStr(objRptDs.Tables(0).Rows(0).Item("TotalAmount"))
            'arrTotalAmount = Split(strTotalAmount, "")
            totalrp = Trim(CStr(FormatNumber(objRptDs.Tables(0).Rows(intCnt).Item("TotalAmount"), 2)))
            strBeforeDec = Replace(Left(LTrim(LCase(objGlobal.TerbilangDesimal(Replace(totalrp, ",", ""), "Rupiah"))), 1), Left(LTrim(LCase(objGlobal.TerbilangDesimal(Replace(totalrp, ",", ""), "Rupiah"))), 1), UCase(Left(LTrim(LCase(objGlobal.TerbilangDesimal(Replace(totalrp, ",", ""), "Rupiah"))), 1))) + Mid(LTrim(LCase(objGlobal.TerbilangDesimal(Replace(totalrp, ",", ""), "Rupiah"))), 2, Len(LTrim(LCase(objGlobal.TerbilangDesimal(Replace(totalrp, ",", ""), "Rupiah")))) - 1)
            'objGlobal.TerbilangDesimal(Replace(totalrp, ",", ""), "Rupiah")
            strTotalAmount = strBeforeDec
        End If

        If objRptDs.Tables(1).Rows.Count > 4 Then
            strReportName = "CB_RPT_CashBankPrintLong"
            rdCrystalViewer.PrintOptions.PaperOrientation = PaperOrientation.Portrait
           rdCrystalViewer.PrintOptions.PaperSize = PaperSize.PaperA4
        Else
            strReportName = "CB_RPT_CashBankPrintLong"
            rdCrystalViewer.PrintOptions.PaperOrientation = PaperOrientation.Portrait
            rdCrystalViewer.PrintOptions.PaperSize = PaperSize.PaperA4
        End If

        rdCrystalViewer.Load(objMapPath & "Web\EN\CB\Reports\Crystal\" & strReportName & ".rpt", OpenReportMethod.OpenReportByTempCopy)
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


            'DiskOpts.DiskFileName = objMapPath & "web\ftp\" & strReportName & ".pdf"
            DiskOpts.DiskFileName = objFTPFolder & strReportName & ".pdf"
            rdCrystalViewer.ExportOptions.DestinationOptions = DiskOpts

            rdCrystalViewer.Export()
            rdCrystalViewer.Close()
            rdCrystalViewer.Dispose()

            Dim strUrl As String
            strUrl = objFTPFolder.Substring(InStr(objFTPFolder, "ftp") - 1, Len(objFTPFolder) - InStr(objFTPFolder, "ftp") + 1)
            strUrl = Replace(strUrl, "\", "/")
            Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../" & strUrl & strReportName & ".pdf"">")
            'Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../ftp/" & strReportName & ".pdf"">")
        End If

    End Sub

    Sub PassParam()
        Dim paramFields As New ParameterFields()
        Dim paramField1 As New ParameterField()
        Dim paramField2 As New ParameterField()
        Dim paramField3 As New ParameterField()
        Dim paramField4 As New ParameterField()
        Dim paramField5 As New ParameterField()
        Dim paramField6 As New ParameterField()
        Dim paramField7 As New ParameterField()
        Dim paramField8 As New ParameterField()
        Dim paramField9 As New ParameterField()
        Dim paramField10 As New ParameterField()


        Dim ParamDiscreteValue1 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue2 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue3 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue4 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue5 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue6 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue7 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue8 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue9 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue10 As New ParameterDiscreteValue()

        Dim crParameterValues1 As ParameterValues
        Dim crParameterValues2 As ParameterValues
        Dim crParameterValues3 As ParameterValues
        Dim crParameterValues4 As ParameterValues
        Dim crParameterValues5 As ParameterValues
        Dim crParameterValues6 As ParameterValues
        Dim crParameterValues7 As ParameterValues
        Dim crParameterValues8 As ParameterValues
        Dim crParameterValues9 As ParameterValues
        Dim crParameterValues10 As ParameterValues

        Dim crDataDef As DataDefinition
        Dim PFDefs As ParameterFieldDefinitions

        crDataDef = rdCrystalViewer.DataDefinition
        PFDefs = crDataDef.ParameterFields
        ParamFields = crvView.ParameterFieldInfo
     
        ParamField1 = ParamFields.Item("strPrintDate")  
        ParamField2 = ParamFields.Item("strPrintedBy")
        ParamField3 = ParamFields.Item("strCompName")
        ParamField4 = ParamFields.Item("strLocName")
        ParamField5 = ParamFields.Item("strStatus")
        ParamField6 = ParamFields.Item("lblAccount")
        ParamField7 = ParamFields.Item("TotalAmountSentence")
        ParamField8 = ParamFields.Item("strVehTag")
        ParamField9 = ParamFields.Item("strVehExpTag")        
        ParamField10 = ParamFields.Item("strBlockTag") 

        crParameterValues1 = ParamField1.CurrentValues
        crParameterValues2 = ParamField2.CurrentValues
        crParameterValues3 = ParamField3.CurrentValues
        crParameterValues4 = ParamField4.CurrentValues
        crParameterValues5 = ParamField5.CurrentValues
        crParameterValues6 = ParamField6.CurrentValues
        crParameterValues7 = ParamField7.CurrentValues
        crParameterValues8 = ParamField8.CurrentValues
        crParameterValues9 = ParamField9.CurrentValues
        crParameterValues10 = ParamField10.CurrentValues

        ParamDiscreteValue1.value = strPrintDate
        ParamDiscreteValue2.value = strUserName
        ParamDiscreteValue3.value = strCompName
        ParamDiscreteValue4.value = strLocName
        ParamDiscreteValue5.value = strStatus
        ParamDiscreteValue6.value = strAccountTag
        ParamDiscreteValue7.value = strTotalAmount
        ParamDiscreteValue8.value = strVehTag
        ParamDiscreteValue9.value = strVehExpTag
        ParamDiscreteValue10.value = strBlockTag

        crParameterValues1.Add(ParamDiscreteValue1)
        crParameterValues2.Add(ParamDiscreteValue2)
        crParameterValues3.Add(ParamDiscreteValue3)
        crParameterValues4.Add(ParamDiscreteValue4)
        crParameterValues5.Add(ParamDiscreteValue5)
        crParameterValues6.Add(ParamDiscreteValue6)
        crParameterValues7.Add(ParamDiscreteValue7)
        crParameterValues8.Add(ParamDiscreteValue8)
        crParameterValues9.Add(ParamDiscreteValue9)
        crParameterValues10.Add(ParamDiscreteValue10)

        PFDefs(0).ApplyCurrentValues(crParameterValues1)
        PFDefs(1).ApplyCurrentValues(crParameterValues2)
        PFDefs(2).ApplyCurrentValues(crParameterValues3)
        PFDefs(3).ApplyCurrentValues(crParameterValues4)
        PFDefs(4).ApplyCurrentValues(crParameterValues5)
        PFDefs(5).ApplyCurrentValues(crParameterValues6)
        PFDefs(6).ApplyCurrentValues(crParameterValues7)
        PFDefs(7).ApplyCurrentValues(crParameterValues8)
        PFDefs(8).ApplyCurrentValues(crParameterValues9)
        PFDefs(9).ApplyCurrentValues(crParameterValues10)

        crvView.ParameterFieldInfo = paramFields
    End Sub



End Class

