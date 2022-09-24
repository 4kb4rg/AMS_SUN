Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic.Information
Imports System.Xml
Imports System.Web.Services
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web

Public Class CM_StdRpt_ContractRegTransactionListPreview : Inherits Page

    Protected WithEvents crvView As CrystalDecisions.Web.CrystalReportViewer
    Protected WithEvents tblCrystal As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents EventData As DataGrid

    Dim objCM As New agri.CM.clsReport()
    Dim objCMSetup As New agri.CM.clsSetup()   
    Dim objCMTrx As New agri.CM.clsTrx()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()

    Dim strCompany As String
    Dim strCompanyName As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strUserName As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim strUserLoc As String

    Dim strRptId As String
    Dim strRptName As String
    Dim strDecimal As String
    Dim strContractNo As String
    Dim strContractType As String
    Dim strDateFrom As String
    Dim strDateTo As String
    Dim strProduct As String
    Dim strSupplier As String
    Dim strBillParty As String
    Dim strCurrency As String
    Dim strPriceBasis As String
    Dim strDelMonth As String
    Dim strDelYear As String
    Dim strStatus As String
    Dim strLocTag As String
    Dim strAccTag As String
    Dim strBlkTag As String
    Dim strBillPartyTag As String

    Dim rdCrystalViewer As New ReportDocument()
    Dim intErrNo As Integer

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        crvView.Visible = False

        strCompany = Session("SS_COMPANY")
        strCompanyName = Session("SS_COMPANYNAME")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strUserName = Session("SS_USERNAME")
        strAccMonth = Session("SS_ARACCMONTH")
        strAccYear = Session("SS_ARACCYEAR")
        strLangCode = Session("SS_LANGCODE")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        Else
            If Right(Request.QueryString("Location"), 1) = "," Then
                strUserLoc = Left(Request.QueryString("Location"), Len(Request.QueryString("Location")) - 1)
            Else
                strUserLoc = Trim(Request.QueryString("Location"))
            End If
            
            strRptId = trim(Request.QueryString("RptID"))
            strRptName = trim(Request.QueryString("RptName"))
            strDecimal = trim(Request.QueryString("Decimal"))

            strContractNo = trim(Request.QueryString("ContractNo"))
            strContractType = trim(Request.QueryString("ContractType"))
            strDateFrom = trim(Request.QueryString("DateFrom"))
            strDateTo = trim(Request.QueryString("DateTo"))
            strProduct = trim(Request.QueryString("Product"))
            strSupplier = trim(Request.QueryString("Supplier"))
            strBillParty = trim(Request.QueryString("BillParty"))
            strCurrency = trim(Request.QueryString("Currency"))
            strPriceBasis = trim(Request.QueryString("PriceBasis"))
            strDelMonth = trim(Request.QueryString("DelMonth"))
            strDelYear = trim(Request.QueryString("DelYear"))
            strStatus = trim(Request.QueryString("Status"))
            strLocTag = trim(Request.QueryString("LocTag"))
            strAccTag = trim(Request.QueryString("AccTag"))
            strBlkTag = trim(Request.QueryString("BlkTag"))
            strBillPartyTag = trim(Request.QueryString("BillPartyTag"))

            BindReport()
        End If
    End Sub


    Sub BindReport()

        Dim objRptDs As New DataSet()
        Dim objMapPath As String
        Dim SearchStr As String
        Dim strParam As String
        Dim crExportOptions As ExportOptions
        Dim crDiskFileDestinationOptions As DiskFileDestinationOptions
        Dim strOpCdGet As String 

        SearchStr = "AND CMC.ContractType = '" & strContractType & "' "

        If CInt(strContractType) = objCMTrx.EnumContractType.Purchase Then
            strOpCdGet = "CM_STDRPT_CONTRACT_REG_PURCHASES_TRANSACTION_LIST"
            If Not strSupplier = "" Then
               SearchStr = SearchStr & "AND CMC.SellerCode LIKE '" & strSupplier & "' "
            End If
        Else
            strOpCdGet = "CM_STDRPT_CONTRACT_REG_SALES_TRANSACTION_LIST"
            If Not strBillParty = "" Then
               SearchStr = SearchStr & "AND CMC.BuyerCode LIKE '" & strBillParty & "' "
            End If
        End If

        If strDateFrom <> "" Then
            SearchStr = SearchStr & "AND (DateDiff(Day, '" & strDateFrom & "', CMC.ContractDate) >= 0) "
        End If

        If strDateTo <> "" Then
             SearchStr = SearchStr & "AND (DateDiff(Day, '" & strDateTo & "', CMC.ContractDate) <= 0) "
        End If

        If strStatus <> "" Then
            SearchStr = SearchStr & "AND CMC.Status = '" & strStatus & "' "
        End If
        
        If strDelMonth <> "" Then
            SearchStr = SearchStr & "AND CMC.DelMonth = '" & strDelMonth & "' "
        End If

        If strDelYear <> "" Then
            SearchStr = SearchStr & "AND CMC.DelYear = '" & strDelYear & "' "
        End If

        If strProduct <> "" Then
            SearchStr = SearchStr & "AND CMC.ProductCode like '" & strProduct & "' "
        End If

        If strPriceBasis <> "" Then
            SearchStr = SearchStr & "AND CMC.PriceBasisCode = '" & strPriceBasis & "' "
        End If

        If strContractNo <> "" Then
            SearchStr = SearchStr & "AND CMC.ContractNo LIKE '" & strContractNo & "' "
        End If

        If strCurrency <> "" Then
            SearchStr = SearchStr & "AND CMC.CurrencyCode = '" & strCurrency & "' "
        End If

        strParam = strUserLoc & "||||" & SearchStr

        Try
            intErrNo = objCM.mtdGetReport_ContractRegTransactionList(strOpCdGet, strParam, objRptDs, objMapPath)
        Catch Exp As System.Exception
            Response.Redirect("../../include/mesg/ErrorMessage.aspx?errcode=CM_STDRPT_CONTRACTREGTRANS_LIST_REPORT&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try



        rdCrystalViewer.Load(objMapPath & "web\" & strLangCode & "\Reports\Crystal\CM_StdRpt_Contract_Reg_TransactionList.rpt", OpenReportMethod.OpenReportByTempCopy)
        rdCrystalViewer.SetDataSource(objRptDs.Tables(0))

        rdCrystalViewer.PrintOptions.PaperOrientation = PaperOrientation.Landscape
        rdCrystalViewer.PrintOptions.PaperSize = PaperSize.PaperA3

        crvView.Visible = False                         
        crvView.ReportSource = rdCrystalViewer          
        crvView.DataBind()                              

        PassParam()

        crDiskFileDestinationOptions = New DiskFileDestinationOptions()
        crDiskFileDestinationOptions.DiskFileName = objMapPath & "web\ftp\CM_StdRpt_Contract_Reg_TransactionList.pdf"

        crExportOptions = rdCrystalViewer.ExportOptions
        With crExportOptions
            .DestinationOptions = crDiskFileDestinationOptions
            .ExportDestinationType = ExportDestinationType.DiskFile
            .ExportFormatType = ExportFormatType.PortableDocFormat
        End With

        rdCrystalViewer.Export()
        Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../ftp/CM_StdRpt_Contract_Reg_TransactionList.pdf"">")
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
        Dim paramField11 As New ParameterField()
        Dim paramField12 As New ParameterField()
        Dim paramField13 As New ParameterField()
        Dim paramField14 As New ParameterField()
        Dim paramField15 As New ParameterField()
        Dim paramField16 As New ParameterField()
        Dim paramField17 As New ParameterField()
        Dim paramField18 As New ParameterField()
        Dim paramField19 As New ParameterField()
        Dim paramField20 As New ParameterField()
        Dim paramField21 As New ParameterField()

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
        Dim crParameterValues11 As ParameterValues
        Dim crParameterValues12 As ParameterValues
        Dim crParameterValues13 As ParameterValues
        Dim crParameterValues14 As ParameterValues
        Dim crParameterValues15 As ParameterValues
        Dim crParameterValues16 As ParameterValues
        Dim crParameterValues17 As ParameterValues
        Dim crParameterValues18 As ParameterValues
        Dim crParameterValues19 As ParameterValues
        Dim crParameterValues20 As ParameterValues
        Dim crParameterValues21 As parameterValues

        Dim crDataDef As DataDefinition
        Dim PFDefs As ParameterFieldDefinitions

        crDataDef = rdCrystalViewer.DataDefinition
        PFDefs = crDataDef.ParameterFields
        paramFields = crvView.ParameterFieldInfo

        paramField1 = paramFields.Item("ParamCompanyName")
        paramField2 = paramFields.Item("ParamLocation")
        paramField3 = paramFields.Item("ParamUserName")
        paramField4 = paramFields.Item("ParamDecimal")
        paramField5 = paramFields.Item("ParamDateFrom")
        paramField6 = paramFields.Item("ParamDateTo")        
        paramField7 = paramFields.Item("ParamRptName")
        paramField8 = paramFields.Item("ParamStatus")
        paramField9 = paramFields.Item("ParamMonth")
        paramField10 = paramFields.Item("ParamYear")
        paramField11 = paramFields.Item("ParamProduct")
        paramField12 = paramFields.Item("ParamRptID")
        paramField13 = paramFields.Item("ParamLblLocation")
        paramField14 = paramFields.Item("ParamPriceBasis")
        paramField15 = paramFields.Item("ParamContractNo")
        paramField16 = paramFields.Item("ParamType")
        paramField17 = paramFields.Item("ParamSellerOrBillParty")
        paramField18 = paramFields.Item("ParamCurrencyCode")
        paramField19 = paramFields.Item("ParamLblSellerOrBillParty")
        paramField20 = paramFields.Item("ParamLblAccount")
        paramField21 = paramFields.Item("ParamLblBlock")

        crParameterValues1 = paramField1.CurrentValues
        crParameterValues2 = paramField2.CurrentValues
        crParameterValues3 = paramField3.CurrentValues
        crParameterValues4 = paramField4.CurrentValues
        crParameterValues5 = paramField5.CurrentValues
        crParameterValues6 = paramField6.CurrentValues
        crParameterValues7 = paramField7.CurrentValues
        crParameterValues8 = paramField8.CurrentValues
        crParameterValues9 = paramField9.CurrentValues
        crParameterValues10 = paramField10.CurrentValues
        crParameterValues11 = paramField11.CurrentValues
        crParameterValues12 = paramField12.CurrentValues
        crParameterValues13 = paramField13.CurrentValues
        crParameterValues14 = paramField14.CurrentValues
        crParameterValues15 = paramField15.CurrentValues
        crParameterValues16 = paramField16.CurrentValues
        crParameterValues17 = paramField17.CurrentValues
        crParameterValues18 = paramField18.CurrentValues
        crParameterValues19 = paramField19.CurrentValues
        crParameterValues20 = paramField20.CurrentValues
        crParameterValues21 = paramField21.CurrentValues

        If Instr(strUserLoc, "','") > 0 Then
            strUserLoc = Replace(strUserLoc, "','", ", ")
        End If

        If strStatus = "" Then
            strStatus = "All"
        Else
            strStatus = objCMTrx.mtdGetContractStatus(CInt(strStatus))
        End if

        If strDelMonth = "" Then
            strDelMonth = "All"
        Else
            strDelMonth = objGlobal.GetShortMonth(strDelMonth)
        End If

        If strDelYear = "" And strDelMonth <> "All" Then
            strDelYear = "All"
        End If

        If strCurrency = "" Then
            strCurrency = "All"
        End If

        ParamDiscreteValue1.Value = strCompanyName
        ParamDiscreteValue2.Value = strUserLoc
        ParamDiscreteValue3.Value = strUserName
        ParamDiscreteValue4.Value = strDecimal
        ParamDiscreteValue5.Value = strDateFrom
        ParamDiscreteValue6.Value = strDateTo
        ParamDiscreteValue7.Value = strRptName
        ParamDiscreteValue8.Value = strStatus
        ParamDiscreteValue9.Value = strDelMonth
        ParamDiscreteValue10.Value = strDelYear
        ParamDiscreteValue11.Value = strProduct
        ParamDiscreteValue12.Value = strRptId
        ParamDiscreteValue13.Value = strLocTag
        ParamDiscreteValue14.Value = strPriceBasis 
        ParamDiscreteValue15.Value = strContractNo
        ParamDiscreteValue16.Value = strContractType

        If CInt(strContractType) = objCMTrx.EnumContractType.Purchase Then
            ParamDiscreteValue17.Value = strSupplier
            ParamDiscreteValue19.Value = "Seller"
        Else
            ParamDiscreteValue17.Value = strBillParty
            ParamDiscreteValue19.Value = strBillPartyTag
        End If
        ParamDiscreteValue18.Value = strCurrency
        ParamDiscreteValue20.Value = strAccTag & " Code"
        ParamDiscreteValue21.Value = strBlkTag & " Code"

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
        crParameterValues11.Add(ParamDiscreteValue11)
        crParameterValues12.Add(ParamDiscreteValue12)
        crParameterValues13.Add(ParamDiscreteValue13)
        crParameterValues14.Add(ParamDiscreteValue14)
        crParameterValues15.Add(ParamDiscreteValue15)
        crParameterValues16.Add(ParamDiscreteValue16)
        crParameterValues17.Add(ParamDiscreteValue17)
        crParameterValues18.Add(ParamDiscreteValue18)
        crParameterValues19.Add(ParamDiscreteValue19)
        crParameterValues20.Add(ParamDiscreteValue20)
        crParameterValues21.Add(ParamDiscreteValue21)

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
        PFDefs(10).ApplyCurrentValues(crParameterValues11)
        PFDefs(11).ApplyCurrentValues(crParameterValues12)
        PFDefs(12).ApplyCurrentValues(crParameterValues13)
        PFDefs(13).ApplyCurrentValues(crParameterValues14)
        PFDefs(14).ApplyCurrentValues(crParameterValues15)
        PFDefs(15).ApplyCurrentValues(crParameterValues16)
        PFDefs(16).ApplyCurrentValues(crParameterValues17)
        PFDefs(17).ApplyCurrentValues(crParameterValues18)
        PFDefs(18).ApplyCurrentValues(crParameterValues19)
        PFDefs(19).ApplyCurrentValues(crParameterValues20)
        PFDefs(20).ApplyCurrentValues(crParameterValues21)

        crvView.ParameterFieldInfo = paramFields
    End Sub
End Class
