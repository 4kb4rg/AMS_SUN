Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports Microsoft.VisualBasic.Strings
Imports System.Xml
Imports System.Web.Services
Imports Microsoft.VisualBasic.Information
Imports Microsoft.VisualBasic.Interaction
Imports Microsoft.VisualBasic

Public Class CM_StdRpt_ContractMatchingTransactionList : Inherits Page

    Protected RptSelect As UserControl

    Dim objCM As New agri.CM.clsReport()
    Dim objCMSetup As New agri.CM.clsSetup()
    Dim objWMTrx As New agri.WM.clsTrx()
    Dim objCMTrx As New agri.CM.clsTrx()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objLangCap As New agri.PWSystem.clsLangCap()


    Protected WithEvents lblDate As Label
    Protected WithEvents lblDateFormat As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblTracker As Label
    Protected WithEvents lblBillParty As Label

    Protected WithEvents lblLocation As Label
    Protected WithEvents lstStatus As DropDownList

    Protected WithEvents PrintPrev As ImageButton

    Protected WithEvents txtContractIDFrom As TextBox
    Protected WithEvents txtContractIDTo As TextBox
    Protected WithEvents ddlproduct As DropDownList
    Protected WithEvents txtBuyer As TextBox
    Protected WithEvents txtContractNo As TextBox
    Protected WithEvents txtInvoiceNo As TextBox
    Protected WithEvents txtDateFrom As TextBox
    Protected WithEvents txtDateTo As TextBox
    Protected WithEvents txtDbCrID As TextBox
    Protected WithEvents txtDbCrDateTo As TextBox
    Protected WithEvents txtDbCrDateFrom As TextBox
    Protected WithEvents txtPriceChangedInd As TextBox
    Protected WithEvents TxtUpdatedBy As TextBox
    Protected WithEvents ddlOrderBy As DropDownList


    Dim objLangCapDs As New Object()

    Dim strCompany As String
    Dim strCompanyName As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strUserName As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intErrNo As Integer
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim strLocType as String

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)

        strCompany = Session("SS_COMPANY")
        strCompanyName = Session("SS_COMPANYNAME")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strUserName = Session("SS_USERNAME")
        strAccMonth = Session("SS_ARACCMONTH")
        strAccYear = Session("SS_ARACCYEAR")
        strLangCode = Session("SS_LANGCODE")
        strLocType = Session("SS_LOCTYPE")
        lblDate.Visible = False
        lblDateFormat.Visible = False

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        Else

            If Not Page.IsPostBack Then
                onload_GetLangCap()
                BindStatusList()  
                BindProductList()              
                ddlOrderBy.Items.Add(New ListItem("Contract Matching ID", "1"))
                ddlOrderBy.Items.Add(New ListItem(lblBillParty.Text & " Code", "2"))
            End If
        End If
    End Sub

    Protected Overloads Sub OnPreRender(ByVal Source As Object, ByVal E As EventArgs) Handles MyBase.PreRender
        Dim SDecimal As  HtmlTableRow 
        Dim SLocation As HtmlTableRow
        Dim SMthYear As HtmlTableRow

        
        SDecimal  = RptSelect.FindControl("SelDecimal")
        SLocation  = RptSelect.FindControl("SelLocation")
        SMthYear = RptSelect.FindControl("TrMthYr")
        
        SDecimal.visible = true
        SLocation.visible = true       
        SMthYear.visible = true 

    End Sub





    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblLocation.Text = GetCaption(objLangCap.EnumLangCap.Location)
        lblBillParty.Text = GetCaption(objLangCap.EnumLangCap.BillParty)
    End Sub

    Sub GetEntireLangCap()
        Dim strOpCode_BussTerm As String = "PWSYSTEM_CLSLANGCAP_BUSSTERM_GET"
        Dim strParam As String
        Dim intErrNo As Integer

        strParam = strLangCode
        Try
            intErrNo = objLangCap.mtdGetBussTerm(strOpCode_BussTerm, _
                                                 strCompany, _
                                                 strLocation, _
                                                 strUserId, _
                                                 strAccMonth, _
                                                 strAccYear, _
                                                 objLangCapDs, _
                                                 strParam)
        Catch Exp As System.Exception
            Response.Redirect("../../include/mesg/ErrorMessage.aspx?errcode=PWSYSTEM_CLSLANGCAP_BUSSTERM_GET&errmesg=" & lblErrMessage.Text & "&redirect=../en/reports/IN_StdRpt_Selection.aspx")
        End Try

    End Sub

    Function GetCaption(ByVal pv_TermCode) As String
            Dim count As Integer

            For count = 0 To objLangCapDs.Tables(0).Rows.Count - 1
                If Trim(pv_TermCode) = Trim(objLangCapDs.Tables(0).Rows(count).Item("TermCode")) Then
                    If strLocType = objAdminLoc.EnumLocType.Mill then
                        Return Trim(objLangCapDs.Tables(0).Rows(count).Item("BusinessTermMW"))
                    else
                        Return Trim(objLangCapDs.Tables(0).Rows(count).Item("BusinessTerm"))
                    end if
                    Exit For
                End If
            Next
        End Function






    Sub BindStatusList()

        
        lstStatus.Items.Add(New ListItem(objCMTrx.mtdGetContractMatchStatus(objCMTrx.EnumContractMatchStatus.All), objCMTrx.EnumContractMatchStatus.All))
        lstStatus.Items.Add(New ListItem(objCMTrx.mtdGetContractMatchStatus(objCMTrx.EnumContractMatchStatus.Active), objCMTrx.EnumContractMatchStatus.Active))
        lstStatus.Items.Add(New ListItem(objCMTrx.mtdGetContractMatchStatus(objCMTrx.EnumContractMatchStatus.Confirmed), objCMTrx.EnumContractMatchStatus.Confirmed))
        lstStatus.Items.Add(New ListItem(objCMTrx.mtdGetContractMatchStatus(objCMTrx.EnumContractMatchStatus.Deleted), objCMTrx.EnumContractMatchStatus.Deleted))


    End Sub


    Sub btnPrintPrev_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim strPRNoFrom As String
        Dim strPRNoTo As String
        Dim strPRType As String
        Dim strStatus As String

        Dim strRptID As String
        Dim strRptName As String
        Dim strUserLoc As String
        Dim strDec As String
        Dim strMonth As String
        Dim strYear As String

        Dim strContractIDFrom As String
        Dim strContractIDTo As String
        Dim strProduct As String
        Dim strBuyer As String
        Dim strContractNo As String
        Dim strInvoiceNo As String
        Dim strDateFrom As String
        Dim strDateTo As String
        Dim strDbCrID As String
        Dim strDbCrDateTo As String
        Dim strDbCrDateFrom As String
        Dim strPriceChangedInd As String
        Dim strUpdatedBy As String

        Dim tempDateFrom As TextBox
        Dim tempDateTo As TextBox
        Dim tempRpt As DropDownList
        Dim tempDec As DropDownList
        Dim templblUL As Label
        Dim tempUserLoc As HtmlInputHidden    
        Dim tempAccMth As DropDownList
        Dim tempAccYear As DropDownList   

        Dim strParam As String
        Dim strDateSetting As String

        Dim objSysCfgDs As New Object()
        Dim objDateFormat As New Object()
        Dim objDateFrom As String
        Dim objDateTo As String
        Dim objDbCrDateTo As String
        Dim objDbCrDateFrom As String
                
        strDateFrom = Trim(txtDateFrom.Text)        
        strDateTo = Trim(txtDateTo.Text)
        tempRpt = RptSelect.FindControl("lstRptName")
        strRptID = Trim(tempRpt.SelectedItem.Value)
        strRptName = Trim(tempRpt.SelectedItem.Text)
        tempAccMth = RptSelect.FindControl("lstAccMonth")
        strMonth = tempAccMth.SelectedItem.Text
        tempAccYear = RptSelect.FindControl("lstAccYear")
        strYear = tempAccYear.SelectedItem.Text

        strContractIDFrom = Trim(txtContractIDFrom.Text)
        strContractIDTo = Trim(txtContractIDTo.Text)
        strProduct = Trim(ddlproduct.SelectedItem.Value) 
        strBuyer = Trim(txtBuyer.Text)
        strContractNo = Trim(txtContractNo.Text)
        strInvoiceNo = Trim(txtInvoiceNo.Text)
        strDateFrom = Trim(txtDateFrom.Text)
        strDateTo = Trim(txtDateTo.Text)
        strDbCrID = Trim(txtDbCrID.Text)
        strDbCrDateTo = Trim(txtDbCrDateTo.Text)
        strDbCrDateFrom = Trim(txtDbCrDateFrom.Text)
        strPriceChangedInd = Trim(txtPriceChangedInd.Text)
        strUpdatedBy = Trim(txtUpdatedBy.Text)
        strStatus = Trim(lstStatus.SelectedItem.Text)   

        strContractIDFrom = Server.UrlEncode(strContractIDFrom)
        strContractIDTo = Server.UrlEncode(strContractIDTo)
        strProduct = Server.UrlEncode(strProduct)
        strBuyer = Server.UrlEncode(strBuyer)
        strContractNo = Server.UrlEncode(strContractNo)
        strInvoiceNo = Server.UrlEncode(strInvoiceNo)
        strDbCrID = Server.UrlEncode(strDbCrID)
        strPriceChangedInd = Server.UrlEncode(strPriceChangedInd)
        strUpdatedBy = Server.UrlEncode(strUpdatedBy)

        tempUserLoc = RptSelect.FindControl("hidUserLoc")
        strUserLoc = Trim(tempUserLoc.Value)
        tempDec = RptSelect.FindControl("lstDecimal")
        strDec = Trim(tempDec.SelectedItem.Value)

        If strUserLoc = "" Then
            templblUL = RptSelect.FindControl("lblUserLoc")
            templblUL.Visible = True
            Exit Sub
        Else
            If Left(strUserLoc, 3) = "','" Then
                strUserLoc = Right(strUserLoc, Len(strUserLoc) - 3)
            ElseIf Right(strUserLoc, 3) = "','" Then
                strUserLoc = Left(strUserLoc, Len(strUserLoc) - 3)
            End If
        End If
        






        strParam = "PWSYSTEM_CLSCONFIG_CONFIG_DATEFMT_GET"
        Try
            intErrNo = objSysCfg.mtdGetConfigInfo(strParam, _
                                                  strCompany, _
                                                  strLocation, _
                                                  strUserId, _
                                                  objSysCfgDs)
        Catch Exp As System.Exception
            Response.Redirect("../../include/mesg/ErrorMessage.aspx?errcode=WM_STDRPT_ContractReg_Transaction_GET_CONFIG_DATE&errmesg=" & lblErrMessage.Text & "&redirect=../en/reports/WM_StdRpt_Selection.aspx")
        End Try

        strDateSetting = objSysCfg.mtdGetDateFormat(Trim(objSysCfgDs.Tables(0).Rows(0).Item("Datefmt")))

        If NOT(strDateFrom = "" AND strDateTo = "") OR NOT(strDbCrDateFrom= "" AND objDbCrDateTo="" ) Then
            If ((objGlobal.mtdValidInputDate(strDateSetting, strDateFrom, objDateFormat, objDateFrom) = True And objGlobal.mtdValidInputDate(strDateSetting, strDateTo, objDateFormat, objDateTo) = True) OR (objGlobal.mtdValidInputDate(strDateSetting, strDbCrDateFrom, objDateFormat, objDbCrDateFrom) = True And objGlobal.mtdValidInputDate(strDateSetting, strDbCrDateTo, objDateFormat, objDbCrDateTo) = True)) Then
           Response.Write("<Script Language=""JavaScript"">window.open(""CM_StdRpt_ContractMatchingTransactionListPreview.aspx?Type=Print&Location=" & strUserLoc & "&RptID=" & strRptID & "&RptName=" & strRptName & "&Decimal=" & strDec & _
                           "&DateFrom=" & objDateFrom & "&DateTo=" & objDateTo & "&UpdatedBy=" & strUpdatedBy  & _
                           "&ContractIDFrom=" & strContractIDFrom & "&ContractIDTo=" & strContractIDTo & _
                           "&Product=" & strProduct & "&Buyer=" & strBuyer & "&ContractNo=" & strContractNo & _
                           "&InvoiceNo=" & strInvoiceNo & "&DbCrID=" & strDbCrID & "&DbCrDateTo=" & objDbCrDateTo & _
                           "&DbCrDateFrom=" & objDbCrDateFrom & "&PriceChangedInd=" & strPriceChangedInd & _
                           "&Month=" & strMonth & "&Year=" & strYear & "&lblLocation=" & lblLocation.Text   & _
                           "&lblBillParty=" & lblBillParty.Text & "&Status=" & strStatus & _
                           "&OrderBy=" & Trim(ddlOrderBy.SelectedItem.Text) & """,null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")
            Else
                lblDateFormat.Text = objDateFormat & "."
                lblDate.Visible = True
                lblDateFormat.Visible = True
            End If
        Else
           Response.Write("<Script Language=""JavaScript"">window.open(""CM_StdRpt_ContractMatchingTransactionListPreview.aspx?Type=Print&Location=" & strUserLoc & "&RptID=" & strRptID & "&RptName=" & strRptName & "&Decimal=" & strDec & _
                           "&DateFrom=" & objDateFrom & "&DateTo=" & objDateTo & "&UpdatedBy=" & strUpdatedBy  & _
                           "&ContractIDFrom=" & strContractIDFrom & "&ContractIDTo=" & strContractIDTo & _
                           "&Product=" & strProduct & "&Buyer=" & strBuyer & "&ContractNo=" & strContractNo & _
                           "&InvoiceNo=" & strInvoiceNo & "&DbCrID=" & strDbCrID & "&DbCrDateTo=" & objDbCrDateTo & _
                           "&DbCrDateFrom=" & objDbCrDateFrom & "&PriceChangedInd=" & strPriceChangedInd & _
                           "&Month=" & strMonth & "&Year=" & strYear & "&lblLocation=" & lblLocation.Text   & _
                           "&lblBillParty=" & lblBillParty.Text & "&Status=" & strStatus & _
                           "&OrderBy=" & Trim(ddlOrderBy.SelectedItem.Value) & """,null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")
        End If
    End Sub

    Sub BindProductList()
        Dim intCnt As Integer
        If ddlProduct.Items.Count = 0 Then
            ddlProduct.Items.Add(New ListItem("All", "All"))
            ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.CPO), objWMTrx.EnumWeighBridgeTicketProduct.CPO))
            ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.PK), objWMTrx.EnumWeighBridgeTicketProduct.PK))
            ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.FFB), objWMTrx.EnumWeighBridgeTicketProduct.FFB))
        End If
    End Sub

End Class
 
