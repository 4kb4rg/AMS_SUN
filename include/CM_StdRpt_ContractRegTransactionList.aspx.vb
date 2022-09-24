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

Public Class CM_StdRpt_ContractRegTransactionList : Inherits Page

    Protected RptSelect As UserControl

    Dim objCM As New agri.CM.clsReport()
    Dim objCMSetup As New agri.CM.clsSetup()
    Dim objCMTrx As New agri.CM.clsTrx()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objWMTrx As New agri.WM.clsTrx()

    Dim objCurrencyDs As New Object()
    Dim objPriceBasisDs As New Object()

    Protected WithEvents lblDate As Label
    Protected WithEvents lblDateFormat As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblTracker As Label

    Protected WithEvents TrBillParty As HtmlTableRow
    Protected WithEvents TrSupplier As HtmlTableRow
    Protected WithEvents LocTag As Label
    Protected WithEvents lblAccount As Label
    Protected WithEvents lblBlock As Label
    Protected WithEvents lblBillParty As Label
    Protected WithEvents lstStatus As DropDownList
    Protected WithEvents ddlDelMonth As DropDownList
    Protected WithEvents ddlDelYear As DropDownList    
    Protected WithEvents rdContractType As RadioButtonList

    Protected WithEvents PrintPrev As ImageButton
    Protected WithEvents txtContractNo As TextBox
    Protected WithEvents txtDateFrom As TextBox
    Protected WithEvents txtDateTo As TextBox
    Protected WithEvents ddlProduct As DropDownList
    Protected WithEvents ddlCurrency As DropDownList
    Protected WithEvents ddlPriceBasis As DropDownList
    Protected WithEvents txtSupplier As TextBox
    Protected WithEvents txtBillParty As TextBox

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
                BindAccYearList()
                BindProduct()
                BindCurrencyList()
                BindPriceBasisList()
                TrSupplier.visible = False
            End If
        End If
    End Sub

    Protected Overloads Sub OnPreRender(ByVal Source As Object, ByVal E As EventArgs) Handles MyBase.PreRender
        Dim SDecimal As HtmlTableRow 
        Dim SLocation As HtmlTableRow

        SDecimal = RptSelect.FindControl("SelDecimal")
        SLocation = RptSelect.FindControl("SelLocation")
        SDecimal.Visible = True
        SLocation.Visible = True
    End Sub


    Sub onChange_ContractType(ByVal Sender As Object, ByVal E As EventArgs)
        Dim strContractType As String

        strContractType = rdContractType.SelectedItem.Value
        TrBillParty.visible = False
        TrSupplier.visible = False

        If CInt(strContractType) = objCMTrx.EnumContractType.Sales Then
            TrBillParty.visible = True
        Else
            TrSupplier.visible = True
        End If
    End Sub

    Sub BindCurrencyList()
        Dim strParam As String
        Dim strSearch As String
        Dim strSort As String
        Dim strOpCdGet As String = "CM_CLSSETUP_CURRENCY_GET"
        Dim dr As DataRow
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer

        strSearch = "and curr.Status = '" & objCMSetup.EnumCurrencyStatus.Active & "' "
        strSort = "order by curr.CurrencyCode "

        strParam = strSearch & "|" & strSort

        Try
            intErrNo = objCMSetup.mtdGetMasterList(strOpCdGet, strParam, 0, objCurrencyDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_STDRPT_CONTRACTREGLIST_CURRENCYLIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try


        If objCurrencyDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objCurrencyDs.Tables(0).Rows.Count - 1
                objCurrencyDs.Tables(0).Rows(intCnt).Item("CurrencyCode") = Trim(objCurrencyDs.Tables(0).Rows(intCnt).Item("CurrencyCode"))
                objCurrencyDs.Tables(0).Rows(intCnt).Item("Description") = objCurrencyDs.Tables(0).Rows(intCnt).Item("CurrencyCode")
            Next
        End If

        dr = objCurrencyDs.Tables(0).NewRow()
        dr("CurrencyCode") = ""
        dr("Description") = "All"
        objCurrencyDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlCurrency.DataSource = objCurrencyDs.Tables(0)
        ddlCurrency.DataValueField = "CurrencyCode"
        ddlCurrency.DataTextField = "Description"
        ddlCurrency.DataBind() 
        ddlCurrency.SelectedIndex = 0
    End Sub

    Sub BindPriceBasisList()
        ddlPriceBasis.Items.Add(New ListItem("All", ""))
        ddlPriceBasis.Items.Add(New ListItem(objCMSetup.mtdGetPriceBasisCode(objCMSetup.EnumPriceBasisCode.SPOT), objCMSetup.EnumPriceBasisCode.SPOT))
        ddlPriceBasis.Items.Add(New ListItem(objCMSetup.mtdGetPriceBasisCode(objCMSetup.EnumPriceBasisCode.MPOB), objCMSetup.EnumPriceBasisCode.MPOB))
        ddlPriceBasis.SelectedIndex = 0
    End Sub

    Sub BindProduct()
        ddlProduct.Items.Add(New ListItem("All", ""))
        ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.FFB), objWMTrx.EnumWeighBridgeTicketProduct.FFB))
        ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.CPO), objWMTrx.EnumWeighBridgeTicketProduct.CPO))
        ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.PK), objWMTrx.EnumWeighBridgeTicketProduct.PK))
    End Sub


    Sub BindAccYearList()
        Dim intCnt As Integer
        Dim CurrDate As Date
        Dim CurrYear As Integer
        Dim intCntAddYr As Integer = 1
        Dim intCntMinYr As Integer = 5
        Dim NewAddCurrYear As Integer
        Dim NewMinCurrYear As Integer
        Dim intCntddlYr As Integer = 0

        CurrDate = Today
        CurrYear = Year(CurrDate)

        While intCntMinYr <> 0
            intCntMinYr = intCntMinYr - 1
            NewMinCurrYear = CurrYear - intCntMinYr
            ddlDelYear.Items.Add(NewMinCurrYear)
        End While

        For intCntAddYr = 1 To 5
            NewAddCurrYear = CurrYear + intCntAddYr
            ddlDelYear.Items.Add(NewAddCurrYear)
        Next

        ddlDelYear.Items.Add(New ListItem("All", ""))
        ddlDelYear.SelectedIndex = ddlDelYear.Items.Count - 1
    End Sub


    Sub onload_GetLangCap()
        GetEntireLangCap()
        LocTag.Text = GetCaption(objLangCap.EnumLangCap.Location)
        lblAccount.text = GetCaption(objLangCap.EnumLangCap.Account)
        lblBlock.text = GetCaption(objLangCap.EnumLangCap.Block)
        lblBillParty.text = GetCaption(objLangCap.EnumLangCap.BillParty)
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
        lstStatus.Items.Add(New ListItem(objCMTrx.mtdGetContractStatus(objCMTrx.EnumContractStatus.All), ""))
        lstStatus.Items.Add(New ListItem(objCMTrx.mtdGetContractStatus(objCMTrx.EnumContractStatus.Active), objCMTrx.EnumContractStatus.Active))
        lstStatus.Items.Add(New ListItem(objCMTrx.mtdGetContractStatus(objCMTrx.EnumContractStatus.Deleted), objCMTrx.EnumContractStatus.Deleted))
    End Sub


    Sub btnPrintPrev_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim strRptID As String
        Dim strRptName As String
        Dim strUserLoc As String
        Dim strDec As String
        Dim strDelMonth As String
        Dim strDelYear As String

        Dim strStatus As String
        Dim strDateFrom As String
        Dim strDateTo As String
        Dim strContractNo As String
        Dim strContractType As String
        Dim strProduct As String
        Dim strSupplier As String
        Dim strBillParty As String
        Dim strSupplierOrBillParty As String
        Dim strPriceBasis As String
        Dim strCurrency As String

        Dim tempDateFrom As TextBox
        Dim tempDateTo As TextBox
        Dim tempRpt As DropDownList
        Dim tempDec As DropDownList
        Dim templblUL As Label
        Dim tempUserLoc As HtmlInputHidden

        Dim strParam As String
        Dim strDateSetting As String
        Dim objSysCfgDs As New Object()
        Dim objDateFormat As New Object()
        Dim objDateFrom As String
        Dim objDateTo As String

        Dim strLocTag As String
        Dim strAccTag As String
        Dim strBlkTag As String
        Dim strBillPartyTag As String

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

        tempRpt = RptSelect.FindControl("lstRptName")
        strRptID = Trim(tempRpt.SelectedItem.Value)
        strRptName = Trim(tempRpt.SelectedItem.Text)

        strContractNo = Trim(txtContractNo.Text)
        strContractType = Trim(rdContractType.SelectedItem.Value)
        strDateFrom = Trim(txtDateFrom.Text)
        strDateTo = Trim(txtDateTo.Text)
        strProduct = Trim(ddlProduct.SelectedItem.Value)
        strSupplier = Trim(txtSupplier.text)
        strBillParty = Trim(txtBillParty.text)
        strCurrency = Trim(ddlCurrency.SelectedItem.Value)
        strPriceBasis = Trim(ddlPriceBasis.SelectedItem.Value)
        strDelMonth = Trim(ddlDelMonth.SelectedItem.Value)
        strDelYear = Trim(ddlDelYear.SelectedItem.Value)
        strStatus = Trim(lstStatus.SelectedItem.Value)
        strLocTag = Trim(LocTag.text)
        strAccTag = Trim(lblAccount.text)
        strBlkTag = Trim(lblBlock.text)
        strBillPartyTag = Trim(lblBillParty.text)

        strContractNo = Server.UrlEncode(strContractNo)
        strProduct = Server.UrlEncode(strProduct)
        strSupplier = Server.UrlEncode(strSupplier)
        strBillParty = Server.UrlEncode(strBillParty)
        strPriceBasis = Server.UrlEncode(strPriceBasis)
        strLocTag = Server.UrlEncode(strLocTag)
        strAccTag = Server.UrlEncode(strAccTag)
        strBlkTag = Server.UrlEncode(strBlkTag)
        strBillPartyTag = Server.UrlEncode(strBillPartyTag)

        strParam = "PWSYSTEM_CLSCONFIG_CONFIG_DATEFMT_GET"
        Try
            intErrNo = objSysCfg.mtdGetConfigInfo(strParam, _
                                                  strCompany, _
                                                  strLocation, _
                                                  strUserId, _
                                                  objSysCfgDs)
        Catch Exp As System.Exception
            Response.Redirect("../../include/mesg/ErrorMessage.aspx?errcode=CM_STDRPT_CONTRACTREGLIST_CONFIG_DATE&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        strDateSetting = objSysCfg.mtdGetDateFormat(Trim(objSysCfgDs.Tables(0).Rows(0).Item("Datefmt")))

        If Not (strDateFrom = "" And strDateTo = "") Then
            If objGlobal.mtdValidInputDate(strDateSetting, strDateFrom, objDateFormat, objDateFrom) = True And objGlobal.mtdValidInputDate(strDateSetting, strDateTo, objDateFormat, objDateTo) = True Then
            Else
                lblDateFormat.Text = objDateFormat & "."
                lblDate.Visible = True
                lblDateFormat.Visible = True
            End If
        End If

        Response.Write("<Script Language=""JavaScript"">window.open(""CM_StdRpt_ContractRegTransactionListPreview.aspx?Type=Print&Location=" & strUserLoc & _
                        "&RptID=" & strRptID & _
                        "&RptName=" & strRptName & _
                        "&Decimal=" & strDec & _
                        "&ContractNo=" & strContractNo & _
                        "&ContractType=" & strContractType & _
                        "&DateFrom=" & objDateFrom & _
                        "&DateTo=" & objDateTo & _
                        "&Product=" & strProduct & _
                        "&Supplier=" & strSupplier & _
                        "&BillParty=" & strBillParty & _
                        "&PriceBasis=" & strPriceBasis & _
                        "&DelMonth=" & strDelMonth & _
                        "&DelYear=" & strDelYear & _
                        "&LocTag=" & strLocTag & _
                        "&AccTag=" & strAccTag & _
                        "&BlkTag=" & strBlkTag & _
                        "&BillPartyTag=" & strBillPartyTag & _
                        "&Currency=" & strCurrency & _
                        "&Status=" & strStatus & _
                        """,null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")


    End Sub

End Class
 
