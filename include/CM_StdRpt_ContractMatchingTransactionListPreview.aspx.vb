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

Public Class CM_StdRpt_ContractMatchingTransactionListPreview : Inherits Page

    Protected WithEvents crvView As CrystalDecisions.Web.CrystalReportViewer
    Protected WithEvents tblCrystal As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents EventData As DataGrid

    Dim objCM As New agri.CM.clsReport()
    Dim objCMSetup As New agri.CM.clsSetup()   
    Dim objCMTrx As New agri.CM.clsTrx()
    Dim objAdmin As New agri.Admin.clsShare()

    Dim strCompany As String
    Dim strCompanyName As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strUserName As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim strUserLoc As String
    Dim strMonth As String
    Dim strYear As String
    Dim strOrderBy As String
    Dim lblOrderBy As String

    Dim tempLoc As String
    Dim strDecimal As String

    Dim rdCrystalViewer As New ReportDocument()
    Dim intErrNo As Integer

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        crvView.Visible = False

        If Right(Request.QueryString("Location"), 1) = "," Then
            strUserLoc = Left(Request.QueryString("Location"), Len(Request.QueryString("Location")) - 1)
        Else
            strUserLoc = Trim(Request.QueryString("Location"))
        End If

        tempLoc = Request.QueryString("Location")
        If Right(tempLoc, 1) = "," Then
            Session("SS_LOC") = Left(tempLoc, Len(tempLoc) - 1)
        Else
            Session("SS_LOC") = tempLoc.Replace("','", ", ")
        End If


        strMonth = Request.QueryString("Month")
        strYear =  Request.QueryString("Year")

        strDecimal = Request.QueryString("Decimal")
        strCompany = Session("SS_COMPANY")
        strCompanyName = Session("SS_COMPANYNAME")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strUserName = Session("SS_USERNAME")
        strAccMonth = Session("SS_ARACCMONTH")
        strAccYear = Session("SS_ARACCYEAR")
        strLangCode = Session("SS_LANGCODE")
        strOrderBy = Trim(Request.QueryString("OrderBy"))


        Session("SS_Decimal") = Request.QueryString("Decimal")
        Session("SS_RptID") = Request.QueryString("RptID")
        Session("SS_RptName") = Request.QueryString("RptName")
        Session("SS_DateFrom") = Request.QueryString("DateFrom")
        Session("SS_DateTo") = Request.QueryString("DateTo")
        Session("SS_PRStatus") = Request.QueryString("Status")    
        Session("SS_Status") = Request.QueryString("Status")
        Session("SS_UpdatedBy") = Request.QueryString("UpdatedBy")
        Session("SS_Month") = Request.QueryString("Month")
        Session("SS_Year") = Request.QueryString("Year")
        Session("SS_LblLocation") = Request.QueryString("LblLocation")

        Session("SS_ContractIDFrom") = Request.QueryString("ContractIDFrom")
        Session("SS_ContractIDTo") = Request.QueryString("ContractIDTo")
        Session("SS_Product") = Request.QueryString("Product")
        Session("SS_Buyer") = Request.QueryString("Buyer")
        Session("SS_ContractNo") = Request.QueryString("ContractNo")
        Session("SS_InvoiceNo") = Request.QueryString("InvoiceNo")
        Session("SS_DbCrID") = Request.QueryString("DbCrID")
        Session("SS_DbCrDateTo") = Request.QueryString("DbCrDateTo")
        Session("SS_DbCrDateFrom") = Request.QueryString("DbCrDateFrom")
        Session("SS_PriceChangedInd") = Request.QueryString("PriceChangedInd")
        Session("SS_LblBillPary") = Request.QueryString("lblBillParty")


        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        Else
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

        Dim strOpCdPR_GET As String = "CM_STDRPT_CONTRACT_MATCHING_TRANSACTION_LIST"


        If Not (Request.QueryString("DateFrom") = "" And Request.QueryString("DateTo") = "") Then
            SearchStr = searchStr & " AND (DateDiff(Day, '" & Session("SS_DATEFROM") & "', CMCLN.InvoiceDate) >= 0) And (DateDiff(Day, '" & Session("SS_DATETO") & "', CMCLN.InvoiceDate) <= 0) "
        End If

        If Request.QueryString("Status") = "All" Then
            SearchStr = SearchStr & "AND CMC.Status LIKE '%' "
        ElseIf Request.QueryString("Status") = "Confirmed" Then
            SearchStr = SearchStr & "AND CMC.Status = '" & objCMTrx.EnumContractMatchStatus.Confirmed & "'"
        ElseIf Request.QueryString("Status") = "Deleted" Then
            SearchStr = SearchStr & "AND CMC.Status = '" & objCMTrx.EnumContractMatchStatus.Deleted & "'"
        ElseIf Request.QueryString("Status") = "Active" Then
            SearchStr = SearchStr & "AND CMC.Status = '" & objCMTrx.EnumContractMatchStatus.Active & "'"
        End If

        If Not Request.QueryString("UpdatedBy") = "" Then
            SearchStr = SearchStr & " AND CMC.UpdateID LIKE '" & Request.QueryString("UpdatedBy") & "'"
        End If

        If Not (Request.QueryString("ContractIDFrom") = "" AND Request.QueryString("ContractIDTo")= "" )Then
            SearchStr = SearchStr & " AND (CMC.MatchingID >= '" & Request.QueryString("ContractIDFrom") & "' AND CMC.MatchingID <= '" & Request.QueryString("ContractIDTo") & "')"
        End If

        If Not Request.QueryString("Product") = "All" Then
            SearchStr = SearchStr & " AND CMC.ProductCode LIKE '" & Request.QueryString("Product") & "'"
        End If

        If Not Request.QueryString("Buyer") = "" Then
            SearchStr = SearchStr & " AND CMC.BuyerCode LIKE '" & Request.QueryString("Buyer") & "'"
        End If

        If Not Request.QueryString("ContractNo") = "" Then
            SearchStr = SearchStr & " AND CMCLN.ContractNo LIKE '" & Request.QueryString("ContractNo") & "'"
        End If

        If Not Request.QueryString("InvoiceNo") = "" Then
            SearchStr = SearchStr & " AND CMCLN.InvoiceId LIKE '" & Request.QueryString("InvoiceNo") & "'"
        End If

        If Not Request.QueryString("DbCrID") = "" Then
            SearchStr = SearchStr & " AND CMCLN.DnCnId LIKE '" & Request.QueryString("DbCrID") & "'"
        End If

        If Not Request.QueryString("DbCrID") = "" Then
            SearchStr = SearchStr & " AND CMCLN.DnCnId LIKE '" & Request.QueryString("DbCrID") & "'"
        End If

        If Not (Request.QueryString("DbCrDateFrom") = "" And Request.QueryString("DbCrDateTo") = "") Then
            SearchStr = searchStr & " AND (DateDiff(Day, '" & Session("SS_DbCrDateFrom") & "', CMCLN.DnCnDate) >= 0) And (DateDiff(Day, '" & Session("SS_DBCRDATETO") & "', CMCLN.DnCnDate) <= 0) "
        End If

        If Not Request.QueryString("PriceChangedInd") = "" Then
            SearchStr = SearchStr & " AND CMCLN.PriceChangedInd LIKE '" & Request.QueryString("PriceChangedInd") & "'"
        End If

        If strOrderBy = "1" Then
            strOrderBy = " ORDER BY CMC.MatchingId "
            lblOrderBy = "Contract Matching ID"
        Else
            strOrderBy = " ORDER BY CMC.BuyerCode "
            lblOrderBy = Session("SS_LblBillPary") & " Code"
        End If

            strParam = strUserLoc & "|" & strMonth  & "|" & strYear & " ||" & SearchStr & strOrderBy

        Try
            intErrNo = objCM.mtdGetReport_ContractMatchingTransactionList(strOpCdPR_GET, strParam, objRptDs, objMapPath)
        Catch Exp As System.Exception
            Response.Redirect("../../include/mesg/ErrorMessage.aspx?errcode=GET_WM_ContractMatching_Transaction_LIST_REPORT&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try



        rdCrystalViewer.Load(objMapPath & "web\" & strLangCode & "\Reports\Crystal\CM_StdRpt_Contract_Matching_TransactionList.rpt", OpenReportMethod.OpenReportByTempCopy)
        rdCrystalViewer.SetDataSource(objRptDs.Tables(0))

        rdCrystalViewer.PrintOptions.PaperOrientation = PaperOrientation.Landscape
        rdCrystalViewer.PrintOptions.PaperSize = PaperSize.PaperA3

        crvView.Visible = False                         
        crvView.ReportSource = rdCrystalViewer          
        crvView.DataBind()                              

        PassParam()

        crDiskFileDestinationOptions = New DiskFileDestinationOptions()
        crDiskFileDestinationOptions.DiskFileName = objMapPath & "web\ftp\CM_StdRpt_Contract_Matching_TransactionList.pdf"

        crExportOptions = rdCrystalViewer.ExportOptions
        With crExportOptions
            .DestinationOptions = crDiskFileDestinationOptions
            .ExportDestinationType = ExportDestinationType.DiskFile
            .ExportFormatType = ExportFormatType.PortableDocFormat
        End With

        rdCrystalViewer.Export()
        Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../ftp/CM_StdRpt_Contract_Matching_TransactionList.pdf"">")
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
        Dim paramField22 As New ParameterField()
        Dim paramField23 As New ParameterField()
        Dim paramField24 As New ParameterField()
        Dim paramField25 As New ParameterField()


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
        Dim crParameterValues21 As ParameterValues
        Dim crParameterValues22 As ParameterValues
        Dim crParameterValues23 As ParameterValues
        Dim crParameterValues24 As ParameterValues
        Dim crParameterValues25 As ParameterValues

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
        paramField9 = paramFields.Item("ParamContractIDFrom")
        paramField10 = paramFields.Item("ParamContractIDTo")
        paramField11 = paramFields.Item("ParamProduct")
        paramField12 = paramFields.Item("ParamBuyer")
        paramField13 = paramFields.Item("ParamUpdatedBy")
        paramField14 = paramFields.Item("ParamRptID")
        paramField15 = paramFields.Item("ParamAccMth")
        paramField16 = paramFields.Item("ParamAccYear")
        paramField17 = paramFields.Item("ParamLblLocation")
        paramField18 = paramFields.Item("ParamContractNo")
        paramField19 = paramFields.Item("ParamInvoiceNo")
        paramField20 = paramFields.Item("ParamDbCrID")
        paramField21 = paramFields.Item("ParamDbCrDateTo")
        paramField22 = paramFields.Item("ParamDbCrDateFrom")
        paramField23 = paramFields.Item("ParamPriceChangedInd")
        paramField24 = paramFields.Item("ParamLblBillParty")
        paramField25 = paramFields.Item("ParamOrderBy")

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
        crParameterValues22 = paramField22.CurrentValues
        crParameterValues23 = paramField23.CurrentValues
        crParameterValues24 = paramField24.CurrentValues
        crParameterValues25 = paramField25.CurrentValues

        ParamDiscreteValue1.Value = Session("SS_COMPANYNAME")
        ParamDiscreteValue2.Value = Session("SS_LOC")
        ParamDiscreteValue3.Value = Session("SS_USERNAME")
        ParamDiscreteValue4.Value = Session("SS_DECIMAL")
        ParamDiscreteValue5.Value = Session("SS_DATEFROM")
        ParamDiscreteValue6.Value = Session("SS_DATETO")
        ParamDiscreteValue7.Value = Session("SS_RPTNAME")
        ParamDiscreteValue8.Value = Session("SS_STATUS")
        ParamDiscreteValue9.Value = Session("SS_CONTRACTIDFROM")
        ParamDiscreteValue10.Value = Session("SS_CONTRACTIDTO")
        ParamDiscreteValue11.Value = Session("SS_PRODUCT")
        ParamDiscreteValue12.Value = Session("SS_BUYER")
        ParamDiscreteValue13.Value = Session("SS_UPDATEDBY")
        ParamDiscreteValue14.Value = Session("SS_RPTID")
        ParamDiscreteValue15.Value = strAccMonth
        ParamDiscreteValue16.Value = strAccYear
        ParamDiscreteValue17.Value = Session("SS_LblLocation") 
        ParamDiscreteValue18.Value = Session("SS_CONTRACTNO") 
        ParamDiscreteValue19.Value = Session("SS_INVOICENO") 
        ParamDiscreteValue20.Value = Session("SS_DBCRID") 
        ParamDiscreteValue21.Value = Session("SS_DBCRDATETO") 
        ParamDiscreteValue22.Value = Session("SS_DBCRDATEFROM") 
        ParamDiscreteValue23.Value = Session("SS_PRICECHANGEDIND") 
        ParamDiscreteValue24.Value = Session("SS_LblBillPary") 
        ParamDiscreteValue25.Value = lblOrderBy 


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
        crParameterValues22.Add(ParamDiscreteValue22)
        crParameterValues23.Add(ParamDiscreteValue23)
        crParameterValues24.Add(ParamDiscreteValue24)
        crParameterValues25.Add(ParamDiscreteValue25)


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
        PFDefs(21).ApplyCurrentValues(crParameterValues22)
        PFDefs(22).ApplyCurrentValues(crParameterValues23)
        PFDefs(23).ApplyCurrentValues(crParameterValues24)
        PFDefs(24).ApplyCurrentValues(crParameterValues25)


        crvView.ParameterFieldInfo = paramFields
    End Sub
End Class
