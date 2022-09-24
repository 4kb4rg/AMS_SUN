Imports System
Imports System.Data
Imports System.Collections 
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Strings
Imports System.Xml
Imports System.Web.Services
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports agri.PWSystem.clsLangCap

Imports agri.GlobalHdl.clsGlobalHdl

Public Class AR_StdRpt_ContractInvoiceExportPreview : Inherits Page

    Dim objARRpt As New agri.BI.clsReport()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objBITrx As New agri.BI.clsTrx()
    Dim objHRSetup As New agri.HR.clsSetup()

    Protected WithEvents crvView As CrystalDecisions.Web.CrystalReportViewer
    Protected WithEvents lblNoRecord As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents EventData As DataGrid
    Protected WithEvents lblErrMesage As Label
    Dim objCompany As New agri.Admin.clsComp()
    Dim objCountryDesc As New agri.Admin.clsCountry()
    Dim objSysLoc As New agri.PWSystem.clsConfig()

    Dim rdCrystalViewer As ReportDocument
    Dim strCompany As String
    Dim strCompanyName As String
    Dim strLocation As String
    Dim strLocationName As String
    Dim strUserId As String
    Dim strPrintedBy As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intConfigsetting As Integer

    Dim strRptId As String
    Dim strRptName As String
    Dim strSelAccMonth As String
    Dim strSelAccYear As String
    Dim intSelDecimal As Integer
    Dim strSelSupress As String
    Dim strUserLoc As String
    Dim strFromInvoiceId As String
    Dim strToInvoiceId As String
    Dim strFromInvoiceDate As String
    Dim strToInvoiceDate As String
    Dim strBillPartyName As String
    Dim strInvoiceType As String
    Dim strSearchExp As String = ""
    Dim BillPartyTag As String

    Dim strUndName As String
    Dim strUndPost As String
    Dim strCompanyAddress As String
    Dim strDeliveryDate As String
    Dim strLC As String
    Dim strLCNo As String
    Dim strIssueDate As String
    Dim strBank As String
    Dim strFOB As String
    Dim strDestination As String
    Dim strShipRisk As String
    Dim strSpecification As String
    Dim strMonth As String
    Dim strInvoiceDate As String
    Dim strNotify As String
    Dim strBillPartyAddress As String


    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        crvView.Visible = False
        strCompany = Session("SS_COMPANY")
        strCompanyName = Session("SS_COMPANYNAME")
        strLocation = Session("SS_LOCATION")
        strLocationName = Session("SS_LOCATIONNAME")
        strUserId = Session("SS_USERID")
        strPrintedBy = Session("SS_USERNAME")
        strAccMonth = Session("SS_ARACCMONTH")
        strAccYear = Session("SS_ARACCYEAR")
        strLangCode = Session("SS_LANGCODE")
        intConfigsetting = Session("SS_CONFIGSETTING")
        strCompanyAddress = Session("SS_COMPANYADDRESS")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        Else
            strUserLoc = Trim(Request.QueryString("Location"))
            strRptId = Trim(Request.QueryString("RptId"))
            strRptName = Trim(Request.QueryString("RptName"))
            strSelAccMonth = Request.QueryString("SelAccMonth")
            strSelAccYear = Request.QueryString("SelAccYear")
            intSelDecimal = CInt(Request.QueryString("Decimal"))
            strSelSupress = Request.QueryString("Supp")

            strFromInvoiceId = Trim(Request.QueryString("FromInvId"))
            strToInvoiceId = Trim(Request.QueryString("ToInvId"))
            strFromInvoiceDate = Trim(Request.QueryString("FromInvDate"))
            strToInvoiceDate = Trim(Request.QueryString("ToInvDate"))
            strBillPartyName = Trim(Request.QueryString("BillPartyName"))
            strInvoiceType = Trim(Request.QueryString("InvType"))
            BillPartyTag = Trim(Request.QueryString("BillPartyTag"))
            strDeliveryDate = Trim(Request.QueryString("DeliveryDate"))
            strLC = Trim(Request.QueryString("LC"))
            strLCNo = Trim(Request.QueryString("LCNo"))
            strIssueDate = Trim(Request.QueryString("DateIssue"))
            strBank = Trim(Request.QueryString("Bank"))
            strFOB = Trim(Request.QueryString("FOB"))
            strDestination = Trim(Request.QueryString("Destination"))
            strShipRisk = Trim(Request.QueryString("ShipRisk"))
            strSpecification = Trim(Request.QueryString("Specification"))
            strUndName = Trim(Request.QueryString("UndName"))
            strUndPost = Trim(Request.QueryString("UndPost"))
            strNotify = Trim(Request.QueryString("Notify"))
            strBillPartyAddress = Trim(Request.QueryString("BillPartyAddress"))

            strSearchExp = "and inv.LocCode = '" & strUserLoc & "' " & _
                            "and inv.DocType = '" & strInvoiceType & "' " & _
                                "and inv.Status IN ('" & objBITrx.EnumInvoiceStatus.Active & "','" & objBITrx.EnumInvoiceStatus.Confirmed & "') "



            If Trim(strFromInvoiceId) <> "" Then
                strSearchExp = strSearchExp & "and inv.InvoiceId >= '" & strFromInvoiceId & "' and inv.InvoiceId <= '" & strFromInvoiceId & "' "
            End If

            'If Trim(strToInvoiceId) <> "" Then
            'strSearchExp = strSearchExp & "and inv.InvoiceId <= '" & strFromInvoiceId & "' "
            'End If


            If Trim(strFromInvoiceDate) <> "" Then
                strSearchExp = strSearchExp & "and datediff(d, '" & strFromInvoiceDate & "', inv.CreateDate) >= 0 "
            End If

            If Trim(strToInvoiceDate) <> "" Then
                strSearchExp = strSearchExp & "and datediff(d, '" & strToInvoiceDate & "', inv.CreateDate) <= 0 "
            End If

            'If Trim(strBillPartyCode) <> "" Then
            'strSearchExp = strSearchExp & "and inv.BillPartyCode like '" & strBillPartyCode & "' "
            'End If

            BindReport()
        End If
    End Sub


    Sub BindReport()
        Dim crExportOptions As ExportOptions
        Dim crDiskFileDestinationOptions As DiskFileDestinationOptions
        Dim strOpCd As String
        Dim strOpCdRslGet As String = ""
        Dim objRptDs As New DataSet()
        Dim objMapPath As String
        Dim intErrNo As Integer
        Dim strParam As String
        Dim strRptPrefix As String
        Dim intCnt As Integer
        Dim totalRp As Long

        Select Case CInt(strInvoiceType)
            Case objBITrx.EnumInvoiceDocType.Manual
                strRptPrefix = "AR_StdRpt_ManualInvoiceExport"
                strOpCd = "AR_STDRPT_MANUAL_INVOICE_GET_BILL_EXPORT" & "|" & "AR_INVOICEEXPORT"
                'Case objBITrx.EnumInvoiceDocType.Manual_Millware
                '    strRptPrefix = "AR_StdRpt_ManualMillInvoice"
                '    strOpCd = "AR_STDRPT_MANUALMILL_INVOICE_GET_SP" & "|" & "AR_INVOICE"
                'Case Else
                '    strRptPrefix = "AR_StdRpt_AutoMillInvoice"
                '    strOpCd = "AR_STDRPT_AUTOMILL_INVOICE_GET_SP" & "|" & "AR_INVOICE"
        End Select

        strParam = strSelAccMonth & "|" & _
                   strSelAccYear & "|" & _
                   strSearchExp & "|" & _
                   objBITrx.EnumInvoiceStatus.Active & "|" & _
                   objBITrx.EnumInvoiceStatus.Confirmed


        Try
            intErrNo = objARRpt.mtdGetReport_ContractInvoice(strOpCd, _
                                                             strCompany, _
                                                             strLocation, _
                                                             strUserId, _
                                                             strAccMonth, _
                                                             strAccYear, _
                                                             strParam, _
                                                             objRptDs, _
                                                             objMapPath)
        Catch Exp As System.Exception
            Response.Redirect("../../include/mesg/ErrorMessage.aspx?errcode=AR_STDRPT_CONTRACTINVOICE_GET&errmesg=" & Exp.ToString() & "&redirect=")
        End Try

        For intCnt = 0 To objRptDs.Tables(0).Rows.Count - 1
            totalRp = Trim(CStr(objRptDs.Tables(0).Rows(intCnt).Item("TotalAmount")))
            objRptDs.Tables(0).Rows(intCnt).Item("Terbilang") = objGlobal.ConvertNo2Words(totalRp)
        Next intCnt

        If objRptDs.Tables(0).Rows.Count > 0 Then
            Select Case Month(objRptDs.Tables(0).Rows(0).Item("CreateDate"))
                Case 1
                    strMonth = "January"
                Case 2
                    strMonth = "February"
                Case 3
                    strMonth = "March"
                Case 4
                    strMonth = "April"
                Case 5
                    strMonth = "May"
                Case 6
                    strMonth = "June"
                Case 7
                    strMonth = "July"
                Case 8
                    strMonth = "August"
                Case 9
                    strMonth = "September"
                Case 10
                    strMonth = "October"
                Case 11
                    strMonth = "November"
                Case 12
                    strMonth = "December"
            End Select
            strInvoiceDate = Day(objRptDs.Tables(0).Rows(0).Item("CreateDate")) & " " & strMonth & " " & Year(objRptDs.Tables(0).Rows(0).Item("CreateDate"))
        End If

        rdCrystalViewer = New ReportDocument()
        rdCrystalViewer.Load(objMapPath & "web\" & strLangCode & "\Reports\Crystal\" & strRptPrefix & ".rpt", OpenReportMethod.OpenReportByTempCopy)
        rdCrystalViewer.SetDataSource(objRptDs)

        PassParam()

        crDiskFileDestinationOptions = New DiskFileDestinationOptions()
        crDiskFileDestinationOptions.DiskFileName = objMapPath & "web\ftp\" & strRptPrefix & ".pdf"

        crExportOptions = rdCrystalViewer.ExportOptions
        With crExportOptions
            .DestinationOptions = crDiskFileDestinationOptions
            .ExportDestinationType = ExportDestinationType.DiskFile
            .ExportFormatType = ExportFormatType.PortableDocFormat
        End With

        rdCrystalViewer.Export()
        Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../ftp/" & strRptPrefix & ".pdf"">")
    End Sub

    Sub PassParam()
        Dim strOpCode_GetSysLoc As String = "PWSYSTEM_CLSCONFIG_SYSLOC_DETAILS_GET"
        Dim strOpCode_GetCountryDetails As String = "ADMIN_CLSCOUNTRY_COUNTRY_DETAILS_GET"
        Dim strOpCd_GetBankDetail As String = "HR_CLSSETUP_BANK_GET"
        Dim intErrNo As Integer
        Dim strSelectedCo As String = ""
        Dim objSysLocDs As New DataSet()
        Dim objCountryDs As Object
        Dim strParam As String
        Dim objBankDs As New Object()
        Dim strBankDescription As String
        Dim strBankAddress As String
        Dim strBankAccNo As String
        Dim strBankAccOwner As String
        Dim strBankSwift As String
        Dim strCorBankDescription As String
        Dim strCorBankAccNo As String
        Dim strCorBankSwift As String

        Dim ParamFieldDefs As ParameterFieldDefinitions
        Dim ParamFieldDef1 As ParameterFieldDefinition
        Dim ParamFieldDef2 As ParameterFieldDefinition
        Dim ParamFieldDef3 As ParameterFieldDefinition
        Dim ParamFieldDef4 As ParameterFieldDefinition
        Dim ParamFieldDef5 As ParameterFieldDefinition
        Dim ParamFieldDef6 As ParameterFieldDefinition
        Dim ParamFieldDef7 As ParameterFieldDefinition
        Dim ParamFieldDef8 As ParameterFieldDefinition
        Dim ParamFieldDef9 As ParameterFieldDefinition
        Dim ParamFieldDef10 As ParameterFieldDefinition
        Dim ParamFieldDef11 As ParameterFieldDefinition
        Dim ParamFieldDef12 As ParameterFieldDefinition
        Dim ParamFieldDef13 As ParameterFieldDefinition
        Dim ParamFieldDef14 As ParameterFieldDefinition
        Dim ParamFieldDef15 As ParameterFieldDefinition
        Dim ParamFieldDef16 As ParameterFieldDefinition
        Dim ParamFieldDef17 As ParameterFieldDefinition
        Dim ParamFieldDef18 As ParameterFieldDefinition
        Dim ParamFieldDef19 As ParameterFieldDefinition
        Dim ParamFieldDef20 As ParameterFieldDefinition
        Dim ParamFieldDef21 As ParameterFieldDefinition
        Dim ParamFieldDef22 As ParameterFieldDefinition
        Dim ParamFieldDef23 As ParameterFieldDefinition
        Dim ParamFieldDef24 As ParameterFieldDefinition
        Dim ParamFieldDef25 As ParameterFieldDefinition
        Dim ParamFieldDef26 As ParameterFieldDefinition
        Dim ParamFieldDef27 As ParameterFieldDefinition
        Dim ParamFieldDef28 As ParameterFieldDefinition
        Dim ParamFieldDef29 As ParameterFieldDefinition
        Dim ParamFieldDef30 As ParameterFieldDefinition

        Dim ParameterValues1 As New ParameterValues()
        Dim ParameterValues2 As New ParameterValues()
        Dim ParameterValues3 As New ParameterValues()
        Dim ParameterValues4 As New ParameterValues()
        Dim ParameterValues5 As New ParameterValues()
        Dim ParameterValues6 As New ParameterValues()
        Dim ParameterValues7 As New ParameterValues()
        Dim ParameterValues8 As New ParameterValues()
        Dim ParameterValues9 As New ParameterValues()
        Dim ParameterValues10 As New ParameterValues()
        Dim ParameterValues11 As New ParameterValues()
        Dim ParameterValues12 As New ParameterValues()
        Dim ParameterValues13 As New ParameterValues()
        Dim ParameterValues14 As New ParameterValues()
        Dim ParameterValues15 As New ParameterValues()
        Dim ParameterValues16 As New ParameterValues()
        Dim ParameterValues17 As New ParameterValues()
        Dim ParameterValues18 As New ParameterValues()
        Dim ParameterValues19 As New ParameterValues()
        Dim ParameterValues20 As New ParameterValues()
        Dim ParameterValues21 As New ParameterValues()
        Dim ParameterValues22 As New ParameterValues()
        Dim ParameterValues23 As New ParameterValues()
        Dim ParameterValues24 As New ParameterValues()
        Dim ParameterValues25 As New ParameterValues()
        Dim ParameterValues26 As New ParameterValues()
        Dim ParameterValues27 As New ParameterValues()
        Dim ParameterValues28 As New ParameterValues()
        Dim ParameterValues29 As New ParameterValues()
        Dim ParameterValues30 As New ParameterValues()

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
        Dim ParamDiscreteValue11 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue12 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue13 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue14 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue15 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue16 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue17 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue18 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue19 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue20 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue21 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue22 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue23 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue24 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue25 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue26 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue27 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue28 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue29 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue30 As New ParameterDiscreteValue()

        Try
            strParam = strCompany & "|" & strLocation & "|" & strUserId
            intErrNo = objSysLoc.mtdGetSysLocInfo(strOpCode_GetSysLoc, _
                                                  strCompany, _
                                                  strLocation, _
                                                  strUserId, _
                                                  objSysLocDs, _
                                                  strParam)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=SETLOC_GET_SYSLOC&errmesg=" & lblErrMesage.Text & "&redirect=system/user/setlocation.aspx")
        End Try

        'strSelectedCo = Session("SS_LOCATION")
        'Try
        '    intErrNo = objCompany.mtdGetComp(strOpCode_GetComp, _
        '                                    strSelectedCo, _
        '                                    objCompDs, _
        '                                    True)
        'Catch Exp As System.Exception
        '    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADMIN_CLSCOMP_COMPANY_DETAILS_GET&errmesg=" & lblErrMesage.Text & "&redirect=")
        'End Try

        strParam = trim(objSysLocDs.Tables(0).Rows(0).Item("CountryCode")) & "|"
        Try
            intErrNo = objCountryDesc.mtdGetCountryDetails(strOpCode_GetCountryDetails, _
                                            objCountryDs, _
                                            strParam)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADMIN_CLSCOUNTRY_COUNTRY_DETAILS_GET&errmesg=" & lblErrMesage.Text & "&redirect=")
        End Try

        strParam = strBank

        Try
            intErrNo = objHRSetup.mtdGetBank(strOpCd_GetBankDetail, _
                                             strParam, _
                                             objBankDs, _
                                             True)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=HR_SETUP_BANK_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        If objBankDs.Tables(0).Rows.Count > 0 Then
            strBankDescription = Trim(objBankDs.Tables(0).Rows(0).Item("Description"))
            strBankAddress = Trim(objBankDs.Tables(0).Rows(0).Item("Address"))
            strBankAccNo = Trim(objBankDs.Tables(0).Rows(0).Item("AccNo"))
            strBankAccOwner = Trim(objBankDs.Tables(0).Rows(0).Item("AccOwner"))
            strBankSwift = Trim(objBankDs.Tables(0).Rows(0).Item("Swift"))
            strCorBankDescription = Trim(objBankDs.Tables(0).Rows(0).Item("CorBankDescription"))
            strCorBankAccNo = Trim(objBankDs.Tables(0).Rows(0).Item("CorBankAccNo"))
            strCorBankSwift = Trim(objBankDs.Tables(0).Rows(0).Item("CorBankSwift"))
        Else
            strBankDescription = ""
            strBankAddress = ""
            strBankAccNo = ""
            strBankAccOwner = ""
            strBankSwift = ""
            strCorBankDescription = ""
            strCorBankAccNo = ""
            strCorBankSwift = ""
        End If


        ParamDiscreteValue1.Value = strCompanyName
        ParamDiscreteValue2.Value = strPrintedBy
        ParamDiscreteValue3.Value = strLocationName
        ParamDiscreteValue4.Value = strCompanyAddress
        ParamDiscreteValue5.Value = Trim(objSysLocDs.Tables(0).Rows(0).Item("LocAddress"))
        ParamDiscreteValue6.Value = Trim(objSysLocDs.Tables(0).Rows(0).Item("City")) & chr(13) & Trim(objCountryDs.Tables(0).Rows(0).Item("CountryDesc")) & " " & Trim(objSysLocDs.Tables(0).Rows(0).Item("PostCode"))
        ParamDiscreteValue7.Value = "Telp. " & Trim(objSysLocDs.Tables(0).Rows(0).Item("TelNo")) & " Fax. " & Trim(objSysLocDs.Tables(0).Rows(0).Item("FaxNo"))
        ParamDiscreteValue8.Value = strShipRisk
        ParamDiscreteValue9.Value = strDeliveryDate
        ParamDiscreteValue10.Value = strFOB
        ParamDiscreteValue11.Value = strDestination
        ParamDiscreteValue12.Value = strLC
        ParamDiscreteValue13.Value = strLCNo
        ParamDiscreteValue14.Value = strIssueDate
        ParamDiscreteValue15.Value = strBank
        ParamDiscreteValue16.Value = strSpecification
        ParamDiscreteValue17.Value = strUndName
        ParamDiscreteValue18.Value = strUndPost
        ParamDiscreteValue19.Value = strBankDescription
        ParamDiscreteValue20.Value = strBankAddress
        ParamDiscreteValue21.Value = strBankAccNo
        ParamDiscreteValue22.Value = strBankAccOwner
        ParamDiscreteValue23.Value = strBankSwift
        ParamDiscreteValue24.Value = strCorBankDescription
        ParamDiscreteValue25.Value = strCorBankAccNo
        ParamDiscreteValue26.Value = strCorBankSwift
        ParamDiscreteValue27.Value = strInvoiceDate
        ParamDiscreteValue28.Value = strNotify
        ParamDiscreteValue29.Value = strBillPartyAddress
        ParamDiscreteValue30.Value = strBillPartyName

        ParamFieldDefs = rdCrystalViewer.DataDefinition.ParameterFields
        ParamFieldDef1 = ParamFieldDefs.Item("SellerName")
        ParamFieldDef2 = ParamFieldDefs.Item("SessionUserName")
        ParamFieldDef3 = ParamFieldDefs.Item("SessionLocName")
        ParamFieldDef4 = ParamFieldDefs.Item("SessionCompAdd")
        ParamFieldDef5 = ParamFieldDefs.Item("LocAddress")
        ParamFieldDef6 = ParamFieldDefs.Item("LocCityPostcode")
        ParamFieldDef7 = ParamFieldDefs.Item("LocNumber")
        ParamFieldDef8 = ParamFieldDefs.Item("ShipRisk")
        ParamFieldDef9 = ParamFieldDefs.Item("DeliveryDate")
        ParamFieldDef10 = ParamFieldDefs.Item("FOB")
        ParamFieldDef11 = ParamFieldDefs.Item("Destination")
        ParamFieldDef12 = ParamFieldDefs.Item("LC")
        ParamFieldDef13 = ParamFieldDefs.Item("LCNo")
        ParamFieldDef14 = ParamFieldDefs.Item("IssueDate")
        ParamFieldDef15 = ParamFieldDefs.Item("BankCode")
        ParamFieldDef16 = ParamFieldDefs.Item("Specification")
        ParamFieldDef17 = ParamFieldDefs.Item("UndersignedName")
        ParamFieldDef18 = ParamFieldDefs.Item("UndersignedPost")
        ParamFieldDef19 = ParamFieldDefs.Item("BankDescription")
        ParamFieldDef20 = ParamFieldDefs.Item("BankAddress")
        ParamFieldDef21 = ParamFieldDefs.Item("BankAccNo")
        ParamFieldDef22 = ParamFieldDefs.Item("BankAccOwner")
        ParamFieldDef23 = ParamFieldDefs.Item("BankSwift")
        ParamFieldDef24 = ParamFieldDefs.Item("CorBankDescription")
        ParamFieldDef25 = ParamFieldDefs.Item("CorBankAccNo")
        ParamFieldDef26 = ParamFieldDefs.Item("CorBankSwift")
        ParamFieldDef27 = ParamFieldDefs.Item("InvoiceDate")
        ParamFieldDef28 = ParamFieldDefs.Item("Notify")
        ParamFieldDef29 = ParamFieldDefs.Item("BillPartyAddress")
        ParamFieldDef30 = ParamFieldDefs.Item("BillPartyName")

        ParameterValues1 = ParamFieldDef1.CurrentValues
        ParameterValues2 = ParamFieldDef2.CurrentValues
        ParameterValues3 = ParamFieldDef3.CurrentValues
        ParameterValues4 = ParamFieldDef4.CurrentValues
        ParameterValues5 = ParamFieldDef5.CurrentValues
        ParameterValues6 = ParamFieldDef6.CurrentValues
        ParameterValues7 = ParamFieldDef7.CurrentValues
        ParameterValues8 = ParamFieldDef8.CurrentValues
        ParameterValues9 = ParamFieldDef9.CurrentValues
        ParameterValues10 = ParamFieldDef10.CurrentValues
        ParameterValues11 = ParamFieldDef11.CurrentValues
        ParameterValues12 = ParamFieldDef12.CurrentValues
        ParameterValues13 = ParamFieldDef13.CurrentValues
        ParameterValues14 = ParamFieldDef14.CurrentValues
        ParameterValues15 = ParamFieldDef15.CurrentValues
        ParameterValues16 = ParamFieldDef16.CurrentValues
        ParameterValues17 = ParamFieldDef17.CurrentValues
        ParameterValues18 = ParamFieldDef18.CurrentValues
        ParameterValues19 = ParamFieldDef19.CurrentValues
        ParameterValues20 = ParamFieldDef20.CurrentValues
        ParameterValues21 = ParamFieldDef21.CurrentValues
        ParameterValues22 = ParamFieldDef22.CurrentValues
        ParameterValues23 = ParamFieldDef23.CurrentValues
        ParameterValues24 = ParamFieldDef24.CurrentValues
        ParameterValues25 = ParamFieldDef25.CurrentValues
        ParameterValues26 = ParamFieldDef26.CurrentValues
        ParameterValues27 = ParamFieldDef27.CurrentValues
        ParameterValues28 = ParamFieldDef28.CurrentValues
        ParameterValues29 = ParamFieldDef29.CurrentValues
        ParameterValues30 = ParamFieldDef30.CurrentValues

        ParameterValues1.Add(ParamDiscreteValue1)
        ParameterValues2.Add(ParamDiscreteValue2)
        ParameterValues3.Add(ParamDiscreteValue3)
        ParameterValues4.Add(ParamDiscreteValue4)
        ParameterValues5.Add(ParamDiscreteValue5)
        ParameterValues6.Add(ParamDiscreteValue6)
        ParameterValues7.Add(ParamDiscreteValue7)
        ParameterValues8.Add(ParamDiscreteValue8)
        ParameterValues9.Add(ParamDiscreteValue9)
        ParameterValues10.Add(ParamDiscreteValue10)
        ParameterValues11.Add(ParamDiscreteValue11)
        ParameterValues12.Add(ParamDiscreteValue12)
        ParameterValues13.Add(ParamDiscreteValue13)
        ParameterValues14.Add(ParamDiscreteValue14)
        ParameterValues15.Add(ParamDiscreteValue15)
        ParameterValues16.Add(ParamDiscreteValue16)
        ParameterValues17.Add(ParamDiscreteValue17)
        ParameterValues18.Add(ParamDiscreteValue18)
        ParameterValues19.Add(ParamDiscreteValue19)
        ParameterValues20.Add(ParamDiscreteValue20)
        ParameterValues21.Add(ParamDiscreteValue21)
        ParameterValues22.Add(ParamDiscreteValue22)
        ParameterValues23.Add(ParamDiscreteValue23)
        ParameterValues24.Add(ParamDiscreteValue24)
        ParameterValues25.Add(ParamDiscreteValue25)
        ParameterValues26.Add(ParamDiscreteValue26)
        ParameterValues27.Add(ParamDiscreteValue27)
        ParameterValues28.Add(ParamDiscreteValue28)
        ParameterValues29.Add(ParamDiscreteValue29)
        ParameterValues30.Add(ParamDiscreteValue30)

        ParamFieldDef1.ApplyCurrentValues(ParameterValues1)
        ParamFieldDef2.ApplyCurrentValues(ParameterValues2)
        ParamFieldDef3.ApplyCurrentValues(ParameterValues3)
        ParamFieldDef4.ApplyCurrentValues(ParameterValues4)
        ParamFieldDef5.ApplyCurrentValues(ParameterValues5)
        ParamFieldDef6.ApplyCurrentValues(ParameterValues6)
        ParamFieldDef7.ApplyCurrentValues(ParameterValues7)
        ParamFieldDef8.ApplyCurrentValues(ParameterValues8)
        ParamFieldDef9.ApplyCurrentValues(ParameterValues9)
        ParamFieldDef10.ApplyCurrentValues(ParameterValues10)
        ParamFieldDef11.ApplyCurrentValues(ParameterValues11)
        ParamFieldDef12.ApplyCurrentValues(ParameterValues12)
        ParamFieldDef13.ApplyCurrentValues(ParameterValues13)
        ParamFieldDef14.ApplyCurrentValues(ParameterValues14)
        ParamFieldDef15.ApplyCurrentValues(ParameterValues15)
        ParamFieldDef16.ApplyCurrentValues(ParameterValues16)
        ParamFieldDef17.ApplyCurrentValues(ParameterValues17)
        ParamFieldDef18.ApplyCurrentValues(ParameterValues18)
        ParamFieldDef19.ApplyCurrentValues(ParameterValues19)
        ParamFieldDef20.ApplyCurrentValues(ParameterValues20)
        ParamFieldDef21.ApplyCurrentValues(ParameterValues21)
        ParamFieldDef22.ApplyCurrentValues(ParameterValues22)
        ParamFieldDef23.ApplyCurrentValues(ParameterValues23)
        ParamFieldDef24.ApplyCurrentValues(ParameterValues24)
        ParamFieldDef25.ApplyCurrentValues(ParameterValues25)
        ParamFieldDef26.ApplyCurrentValues(ParameterValues26)
        ParamFieldDef27.ApplyCurrentValues(ParameterValues27)
        ParamFieldDef28.ApplyCurrentValues(ParameterValues28)
        ParamFieldDef29.ApplyCurrentValues(ParameterValues29)
        ParamFieldDef30.ApplyCurrentValues(ParameterValues30)
    End Sub

End Class

