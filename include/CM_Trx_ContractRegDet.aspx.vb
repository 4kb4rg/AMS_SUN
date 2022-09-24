
Imports System
Imports System.Data
Imports System.Math
Imports System.IO 
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports Microsoft.VisualBasic.Information
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic.Interaction
Imports Microsoft.VisualBasic

Public Class CM_Trx_ContractRegDet : Inherits Page
    Protected WithEvents txtContractNo As TextBox
    Protected WithEvents rdContractType As RadioButtonList
    Protected WithEvents rdContType As RadioButtonList
    Protected WithEvents txtContractDate As TextBox
    Protected WithEvents btnContractDate As Image
    Protected WithEvents lblContractDate As Label
    Protected WithEvents lblContractDateFmt As Label
    Protected WithEvents ddlProduct As DropDownList
    Protected WithEvents ddlSeller As DropDownList
    Protected WithEvents ddlBuyer As DropDownList
    Protected WithEvents txtContractQty As TextBox
    Protected WithEvents txtExtraQty As TextBox
    Protected WithEvents ddlExtraQtyType As DropDownList
    Protected WithEvents lblMatchedQty As Label
    Protected WithEvents lblBalQty As Label
    Protected WithEvents ddlCurrency As DropDownList
    Protected WithEvents txtPrice As TextBox
    Protected WithEvents ddlPriceBasis As DropDownList
    Protected WithEvents ddlAccMonth As DropDownList
    Protected WithEvents ddlAccYear As DropDownList
    Protected WithEvents taRemarks As HtmlTextArea
    Protected WithEvents ddlAccount As DropDownList
    Protected WithEvents ddlBlock As DropDownList
    Protected WithEvents lblStatus As Label
    Protected WithEvents lblDateCreated As Label
    Protected WithEvents lblLastUpdate As Label
    Protected WithEvents lblUpdatedBy As Label
    Protected WithEvents lblErrContractQty As Label
    Protected WithEvents lblActiveMatchExist As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents lblSelect As Label

    Protected WithEvents taProductQuality As HtmlTextArea
    Protected WithEvents taTermOfPayment As HtmlTextArea

    Protected WithEvents tbcode As HtmlInputHidden
    Protected WithEvents SaveBtn As ImageButton
    Protected WithEvents CloseBtn As ImageButton
    Protected WithEvents DelBtn As ImageButton
    Protected WithEvents UnDelBtn As ImageButton

    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblHiddenSts As Label
    Protected WithEvents lblPleaseSelect As Label
    Protected WithEvents lblBillParty As Label
    Protected WithEvents lblAccount As Label
    Protected WithEvents lblBlock As Label
    Protected WithEvents rfvBuyer As RequiredFieldValidator
    Protected WithEvents rfvAccount As RequiredFieldValidator
    Protected WithEvents PrintBtn As System.Web.UI.WebControls.ImageButton

    Protected WithEvents trSeller As HtmlTableRow
    Protected WithEvents trBuyer As HtmlTableRow


    Protected WithEvents trLokal As HtmlTableRow
    Protected WithEvents trExport As HtmlTableRow
    Protected WithEvents txtContNo As TextBox
    Protected WithEvents ddlContCategory As DropDownList
    Protected WithEvents ddlTerm As DropDownList
    Protected WithEvents lblBankCode As Label
    Protected WithEvents lblBankAccNo As Label

    Protected WithEvents txtDelivMonth As TextBox
    Protected WithEvents btnDelivMonth As Image
    Protected WithEvents lblDelivMonth As Label
    Protected WithEvents lblDelivMonthFmt As Label
    Protected WithEvents lblPriceBasis As Label

    Protected WithEvents lblBillPartyNo As Label
    Protected WithEvents txtBuyerNo As TextBox
    Protected WithEvents ddlCurrencyOA As DropDownList
    Protected WithEvents txtPriceOA As TextBox
    Protected WithEvents taProductClaim As HtmlTextArea
    Protected WithEvents taDeliveryTime As HtmlTextArea
    Protected WithEvents taTermOfWeighing As HtmlTextArea
    Protected WithEvents taConsignment As HtmlTextArea

    Protected WithEvents hidPPN As HtmlInputHidden
    Protected WithEvents hidTermOfWeighing As HtmlInputHidden
    Protected WithEvents hidQualityCPO As HtmlInputHidden
    Protected WithEvents hidClaimCPO As HtmlInputHidden
    Protected WithEvents hidQualityPK As HtmlInputHidden
    Protected WithEvents hidClaimPK As HtmlInputHidden
    Protected WithEvents hidTermOfPayment As HtmlInputHidden
    Protected WithEvents hidConsignment As HtmlInputHidden

    Protected WithEvents txtUnderName As TextBox
    Protected WithEvents txtUnderPost As TextBox

    Protected WithEvents cbExcel As CheckBox


    Dim objCMTrx As New agri.CM.clsTrx()
    Dim objCMSetup As New agri.CM.clsSetup()
    Dim objGLSetup As New agri.GL.clsSetup()
    Dim objGLtrx As New agri.GL.ClsTrx()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objWMTrx As New agri.WM.clsTrx()
    Dim objPUSetup As New agri.PU.clsSetup()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Protected objIN As New agri.IN.clsTrx()

    Dim objContractDs As New Object()
    Dim objMatchDs As New Object()
    Dim objCheckDs As New Object()
    Dim objSellerDs As New Object()
    Dim objBuyerDs As New Object()
    Dim objCurrencyDs As New Object()
    Dim objPriceBasisDs As New Object()
    Dim objAccDs As New Object()
    Dim objBlkDs As New Object()
    Dim objLangCapDs As New Object()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim intCMAR As Integer
    Dim strDateFMt As String
    Dim intConfigsetting As Integer

    Dim strContractNo As String = ""
    Dim intStatus As Integer
    Dim intMaxLen As Integer = 0

    Dim strPhyMonth As String
    Dim strPhyYear As String
    Dim strLastPhyYear As String
    Dim strLocType As String

    Protected WithEvents lblBankCode2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblBankAccNo2 As System.Web.UI.WebControls.Label
    Protected WithEvents txtBankAccNo2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddlBankCode2 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblBankCode1 As System.Web.UI.WebControls.Label
    Protected WithEvents ddlBankCode1 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblBankAccNo1 As System.Web.UI.WebControls.Label
    Protected WithEvents txtBankAccNo1 As System.Web.UI.WebControls.TextBox

    Private objHRSetup As New agri.HR.clsSetup
    Private dsBank As DataSet

    Dim strTermWeighing As String
    Dim strQualityCPO As String
    Dim strClaimCPO As String
    Dim strQualityPK As String
    Dim strClaimPK As String
    Dim strParamName As String
    Dim strParamValue As String


    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_ARACCMONTH")
        strAccYear = Session("SS_ARACCYEAR")
        intCMAR = Session("SS_CMAR")
        strDateFMt = Session("SS_DATEFMT")
        intConfigsetting = Session("SS_CONFIGSETTING")
        strLocType = Session("SS_LOCTYPE")

        strPhyMonth = Session("SS_PHYMONTH")
        strPhyYear = Session("SS_PHYYEAR")
        strLastPhyYear = Session("SS_LASTPHYYEAR")
        strLocType = Session("SS_LOCTYPE")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCMAccessRights.CMContractReg), intCMAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            onload_GetLangCap()
            lblContractDate.Visible = False
            lblContractDateFmt.Visible = False

            lblDelivMonth.Visible = False
            lblDelivMonthFmt.Visible = False

            lblErrContractQty.Visible = False
            strContractNo = Trim(IIf(Request.QueryString("tbcode") <> "", Request.QueryString("tbcode"), Request.Form("tbcode")))
            intStatus = CInt(lblHiddenSts.Text)
            If Not IsPostBack Then
                If strContractNo <> "" Then
                    tbcode.Value = strContractNo
                    onLoad_Display(strContractNo)
                    onLoad_BindButton()
                    CheckDisableContractQty()
                Else
                    If strContractNo = "" Then
                        txtContractDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
                        txtDelivMonth.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
                    End If
                    BindProductList("")
                    BindSellerList("")
                    BindBuyerList("")
                    BindCurrencyList("")
                    BindCurrencyOAList("")
                    BindExtraQtyTypeList("")
                    BindPriceBasisList("")
                    BindAccountList("")
                    BindBlockList("", "")
                    BindContCategory("")
                    BindTermOfDelivery("")
                    onLoad_BindButton()
                    onLoad_TableRow(2)

                    BindBankCode(ddlBankCode1, "")
                    BindBankCode(ddlBankCode2, "")
                End If
            End If
        End If
        lblBankCode1.Text = "Bank Code : "
        lblBankAccNo1.Text = "Bank Account No : "
        lblPriceBasis.Text = "Price Basis Code :*"
        ddlPriceBasis.SelectedIndex = 2

        lblBankCode2.Text = "Bank Code : "
        lblBankAccNo2.Text = "Bank Account No : "
    End Sub


    Sub BindProductList(ByVal pv_strProdCode As String)
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer

        If ddlProduct.Items.Count = 0 Then
			ddlProduct.Items.Add(New ListItem("Select Product", ""))
            ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.CPO), objWMTrx.EnumWeighBridgeTicketProduct.CPO))
            ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.PK), objWMTrx.EnumWeighBridgeTicketProduct.PK))
'            ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.EFB), objWMTrx.EnumWeighBridgeTicketProduct.EFB))
            ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.Shell), objWMTrx.EnumWeighBridgeTicketProduct.Shell))
'            ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.AbuJanjang), objWMTrx.EnumWeighBridgeTicketProduct.AbuJanjang))
           ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.Fiber), objWMTrx.EnumWeighBridgeTicketProduct.Fiber))
  '          ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.Brondolan), objWMTrx.EnumWeighBridgeTicketProduct.Brondolan))
   '         ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.Solid), objWMTrx.EnumWeighBridgeTicketProduct.Solid))
    '        ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.MinyakLimbah), objWMTrx.EnumWeighBridgeTicketProduct.MinyakLimbah))
            ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.EFB), objWMTrx.EnumWeighBridgeTicketProduct.EFB))
            ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.Others), objWMTrx.EnumWeighBridgeTicketProduct.Others))
            ddlProduct.Items.Add(New ListItem("POME", objWMTrx.EnumWeighBridgeTicketProduct.EFBOil))
            ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.MinyakKolam), objWMTrx.EnumWeighBridgeTicketProduct.MinyakKolam))
            'ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.EFBPress), objWMTrx.EnumWeighBridgeTicketProduct.EFBPress))
        End If

        If Trim(pv_strProdCode) <> "" Then
            For intCnt = 0 To ddlProduct.Items.Count - 1
                If ddlProduct.Items(intCnt).Value = pv_strProdCode Then
                    intSelectedIndex = intCnt
                End If
            Next
            ddlProduct.SelectedIndex = intSelectedIndex
        Else
            ddlProduct.SelectedIndex = 0
        End If

    End Sub

    Sub BindSellerList(ByVal pv_strSellerCode As String)
        Dim strOpCd_Get As String = "PU_CLSSETUP_SUPPLIER_GET"
        Dim strSrchStatus As String
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim dr As DataRow
        Dim intSelectedIndex As Integer

        'strSrchStatus = objPUSetup.EnumSuppStatus.Active

        'strParam = "||" & strSrchStatus & "||SupplierCode||"

        strParam = "||" & objPUSetup.EnumSuppStatus.Active & "||SupplierCode||"
        'strParam = strParam & "|" & IIf(Session("SS_COACENTRALIZED") = "1", "", " A.AccCode in (SELECT AccCode FROM GL_Account WHERE LocCode='" & Trim(strLocation) & "') ")
        strParam = strParam & "|" & IIf(Session("SS_COACENTRALIZED") = "1", "", " SupplierCode LIKE '%" & Trim(strLocation) & "%'") ', " A.AccCode in (SELECT AccCode FROM GL_Account WHERE LocCode='" & Trim(strLocation) & "') ")

        Try
            intErrNo = objPUSetup.mtdGetSupplier(strOpCd_Get, strParam, objSellerDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_SELLERLIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To objSellerDs.Tables(0).Rows.Count - 1
            objSellerDs.Tables(0).Rows(intCnt).Item("SupplierCode") = Trim(objSellerDs.Tables(0).Rows(intCnt).Item("SupplierCode"))
            objSellerDs.Tables(0).Rows(intCnt).Item("Name") = objSellerDs.Tables(0).Rows(intCnt).Item("SupplierCode") & " (" & Trim(objSellerDs.Tables(0).Rows(intCnt).Item("Name")) & ")"
            If objSellerDs.Tables(0).Rows(intCnt).Item("SupplierCode") = pv_strSellerCode Then
                intSelectedIndex = intCnt + 1
            End If
        Next

        dr = objSellerDs.Tables(0).NewRow()
        dr("SupplierCode") = ""
        dr("Name") = lblSelect.Text & "Supplier Code"
        objSellerDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlSeller.DataSource = objSellerDs.Tables(0)
        ddlSeller.DataValueField = "SupplierCode"
        ddlSeller.DataTextField = "Name"
        ddlSeller.DataBind()
        ddlSeller.SelectedIndex = intSelectedIndex

    End Sub


    Sub BindBuyerList(ByVal pv_strBuyerCode As String)
        Dim strParam As String
        Dim strOpCdGet As String = "GL_CLSSETUP_BILLPARTY_GET"
        Dim dr As DataRow
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer

        strParam = "" & "|" & _
                   "" & "|" & _
                   objGLSetup.EnumBillPartyStatus.Active & "|" & _
                   "" & "|" & _
                   "BP.Name" & "|" & _
                   "ASC" & "|"
        Try
            intErrNo = objGLSetup.mtdGetBillParty(strOpCdGet, strParam, objBuyerDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_BUYERLIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegList.aspx")
        End Try

        If objBuyerDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objBuyerDs.Tables(0).Rows.Count - 1
                objBuyerDs.Tables(0).Rows(intCnt).Item("BillPartyCode") = Trim(objBuyerDs.Tables(0).Rows(intCnt).Item("BillPartyCode"))
                'objBuyerDs.Tables(0).Rows(intCnt).Item("Name") = Trim(objBuyerDs.Tables(0).Rows(intCnt).Item("BillPartyCode")) & " (" & Trim(objBuyerDs.Tables(0).Rows(intCnt).Item("Name")) & ")"
                objBuyerDs.Tables(0).Rows(intCnt).Item("Name") = Trim(objBuyerDs.Tables(0).Rows(intCnt).Item("NamePPN")) 
                If objBuyerDs.Tables(0).Rows(intCnt).Item("BillPartyCode") = pv_strBuyerCode Then
                    intSelectedIndex = intCnt + 1
                End If
            Next
        End If

        dr = objBuyerDs.Tables(0).NewRow()
        dr("BillPartyCode") = ""
        dr("Name") = lblSelect.Text & lblBillParty.Text
        objBuyerDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlBuyer.DataSource = objBuyerDs.Tables(0)
        ddlBuyer.DataValueField = "BillPartyCode"
        ddlBuyer.DataTextField = "Name"
        ddlBuyer.DataBind()
        ddlBuyer.SelectedIndex = intSelectedIndex


    End Sub


    Sub BindExtraQtyTypeList(ByVal pv_strExtraQtyType As String)
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer

        If ddlExtraQtyType.Items.Count = 0 Then
            ddlExtraQtyType.Items.Add(New ListItem(objCMTrx.mtdGetExtraQtyType(objCMTrx.EnumExtraQtyType.Percentage), objCMTrx.EnumExtraQtyType.Percentage))
            ddlExtraQtyType.Items.Add(New ListItem(objCMTrx.mtdGetExtraQtyType(objCMTrx.EnumExtraQtyType.MetricTonnes), objCMTrx.EnumExtraQtyType.MetricTonnes))
        End If

        If Trim(pv_strExtraQtyType) <> "" Then
            For intCnt = 0 To ddlExtraQtyType.Items.Count - 1
                If ddlExtraQtyType.Items(intCnt).Value = pv_strExtraQtyType Then
                    intSelectedIndex = intCnt
                End If
            Next
            ddlExtraQtyType.SelectedIndex = intSelectedIndex
        Else
            ddlExtraQtyType.SelectedIndex = 0
        End If

    End Sub

    Sub BindCurrencyList(ByVal pv_strCurrencyCode As String)
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_CURRENCYLIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegList.aspx")
        End Try

        If pv_strCurrencyCode = "" Then
            pv_strCurrencyCode = "IDR"
            ddlCurrency.SelectedValue = pv_strCurrencyCode

            ddlCurrency.DataSource = objCurrencyDs.Tables(0)
            ddlCurrency.DataValueField = "CurrencyCode"
            ddlCurrency.DataTextField = "CurrencyCode"
            ddlCurrency.DataBind()
        Else
            If objCurrencyDs.Tables(0).Rows.Count > 0 Then
                For intCnt = 0 To objCurrencyDs.Tables(0).Rows.Count - 1
                    objCurrencyDs.Tables(0).Rows(intCnt).Item("CurrencyCode") = Trim(objCurrencyDs.Tables(0).Rows(intCnt).Item("CurrencyCode"))
                    objCurrencyDs.Tables(0).Rows(intCnt).Item("Description") = objCurrencyDs.Tables(0).Rows(intCnt).Item("CurrencyCode")
                    If objCurrencyDs.Tables(0).Rows(intCnt).Item("CurrencyCode") = pv_strCurrencyCode Then
                        intSelectedIndex = intCnt
                    End If
                Next
            End If

            ddlCurrency.DataSource = objCurrencyDs.Tables(0)
            ddlCurrency.DataValueField = "CurrencyCode"
            ddlCurrency.DataTextField = "CurrencyCode"
            ddlCurrency.DataBind()
            ddlCurrency.SelectedIndex = intSelectedIndex
        End If
    End Sub

    Sub BindCurrencyOAList(ByVal pv_strCurrencyCode As String)
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_CURRENCYLIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegList.aspx")
        End Try

        If pv_strCurrencyCode = "" Then
            pv_strCurrencyCode = "IDR"
            ddlCurrencyOA.SelectedValue = pv_strCurrencyCode

            ddlCurrencyOA.DataSource = objCurrencyDs.Tables(0)
            ddlCurrencyOA.DataValueField = "CurrencyCode"
            ddlCurrencyOA.DataTextField = "CurrencyCode"
            ddlCurrencyOA.DataBind()
        Else
            If objCurrencyDs.Tables(0).Rows.Count > 0 Then
                For intCnt = 0 To objCurrencyDs.Tables(0).Rows.Count - 1
                    objCurrencyDs.Tables(0).Rows(intCnt).Item("CurrencyCode") = Trim(objCurrencyDs.Tables(0).Rows(intCnt).Item("CurrencyCode"))
                    objCurrencyDs.Tables(0).Rows(intCnt).Item("Description") = objCurrencyDs.Tables(0).Rows(intCnt).Item("CurrencyCode")
                    If objCurrencyDs.Tables(0).Rows(intCnt).Item("CurrencyCode") = pv_strCurrencyCode Then
                        intSelectedIndex = intCnt
                    End If
                Next
            End If

            ddlCurrencyOA.DataSource = objCurrencyDs.Tables(0)
            ddlCurrencyOA.DataValueField = "CurrencyCode"
            ddlCurrencyOA.DataTextField = "CurrencyCode"
            ddlCurrencyOA.DataBind()
            ddlCurrencyOA.SelectedIndex = intSelectedIndex
        End If
    End Sub


    Sub BindPriceBasisList(ByVal pv_strPriceBasisCode As String)
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer

        If ddlPriceBasis.Items.Count = 0 Then
            ddlPriceBasis.Items.Add(New ListItem("Select Price Basis Code", ""))
            ddlPriceBasis.Items.Add(New ListItem(objCMSetup.mtdGetPriceBasisCode(objCMSetup.EnumPriceBasisCode.SPOT), objCMSetup.EnumPriceBasisCode.SPOT))
            ddlPriceBasis.Items.Add(New ListItem(objCMSetup.mtdGetPriceBasisCode(objCMSetup.EnumPriceBasisCode.MPOB), objCMSetup.EnumPriceBasisCode.MPOB))
        End If

        If Trim(pv_strPriceBasisCode) <> "" Then
            For intCnt = 0 To ddlPriceBasis.Items.Count - 1
                If ddlPriceBasis.Items(intCnt).Value = pv_strPriceBasisCode Then
                    intSelectedIndex = intCnt
                End If
            Next
            ddlPriceBasis.SelectedIndex = intSelectedIndex
        Else
            ddlPriceBasis.SelectedIndex = 0
        End If

    End Sub

    Sub BindBankCode(ByVal pv_ddl As DropDownList, ByVal pv_strCode As String)
        Dim strOpCd As String = "HR_CLSSETUP_BANK_SEARCH"
        Dim strParameter As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer = 0
        Dim strDate As String = Date_Validation(txtContractDate.Text, False)
        Dim strOpCodeBank As String

        'strParameter = "||||B.BankCode|ASC|"
        'Try
        '    If dsBank Is Nothing OrElse dsBank.Tables.Count() = 0 Then
        '        If objHRSetup.mtdGetBank(strOpCd, strParameter, dsBank, False) = 0 Then
        '            For intCnt As Integer = 0 To dsBank.Tables(0).Rows.Count - 1
        '                dsBank.Tables(0).Rows(intCnt).Item("BankCode") = Trim(dsBank.Tables(0).Rows(intCnt).Item("BankCode"))
        '                dsBank.Tables(0).Rows(intCnt).Item("Description") = Trim(dsBank.Tables(0).Rows(intCnt).Item("BankCode")) & " (" & Trim(dsBank.Tables(0).Rows(intCnt).Item("Description")) & ")"
        '            Next

        '            Dim dr As DataRow
        '            dr = dsBank.Tables(0).NewRow()
        '            dr("BankCode") = ""
        '            dr("_Description") = lblSelect.Text & "Bank Code"
        '            dsBank.Tables(0).Rows.InsertAt(dr, 0)
        '        Else
        '            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREG_BINDBANKCODE_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/trx/CM_trx_ContractRegDet.aspx")
        '        End If
        '    End If
        'Catch Exp As Exception
        '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREG_BINDBANKCODE_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/trx/CM_trx_ContractRegDet.aspx")
        'End Try

        strAccYear = Year(strDate)
        strAccMonth = Month(strDate)
        strOpCodeBank = "HR_CLSSETUP_BANK_LOCATION_GET"

        strParamName = "LOCCODE|ACCYEAR|ACCMONTH|TRXDATE|ACCCODE"
        strParamValue = strLocation & "|" & strAccYear & "|" & strAccMonth & "|" & strDate & "|"

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCodeBank, _
                                                strParamName, _
                                                strParamValue, _
                                                dsBank)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ID_GET&errmesg=" & Exp.ToString() & "&redirect=")
        End Try

        For intCnt = 0 To dsBank.Tables(0).Rows.Count - 1
            If Trim(pv_strCode) = Trim(dsBank.Tables(0).Rows(intCnt).Item("BankCode")) Then
                intSelectedIndex = intCnt + 1
            End If
        Next intCnt

        Dim dr As DataRow
        dr = dsBank.Tables(0).NewRow()
        dr("BankCode") = ""
        dr("_Description") = lblPleaseSelect.Text & " Bank" & lblCode.Text
        dsBank.Tables(0).Rows.InsertAt(dr, 0)

        pv_ddl.DataSource = dsBank.Tables(0)
        pv_ddl.DataValueField = "BankCode"
        pv_ddl.DataTextField = "_Description"
        pv_ddl.DataBind()

        Dim itm As ListItem = pv_ddl.Items.FindByValue(pv_strCode.Trim())
        If Not itm Is Nothing Then
            intSelectedIndex = pv_ddl.Items.IndexOf(itm)
        End If

        pv_ddl.SelectedIndex = intSelectedIndex
    End Sub

    Sub onSelect_Bank(ByVal Sender As System.Object, ByVal E As System.EventArgs)
        Dim intErrNo As Integer
        Dim strDate As String = Date_Validation(txtContractDate.Text, False)
        Dim strOpCodeBank As String
        Dim strBank As String
        Dim strBankAccCode As String
        Dim arrParam As Array

        If ddlBankCode1.SelectedItem.Value = "" Then
            txtBankAccNo1.Text = ""
            Exit Sub
        Else
            arrParam = Split(Trim(ddlBankCode1.SelectedItem.Value), "|")
            strBank = Trim(arrParam(0))
            strBankAccCode = Trim(arrParam(1))
        End If

        strAccYear = Year(strDate)
        strAccMonth = Month(strDate)
        strOpCodeBank = "HR_CLSSETUP_BANK_LOCATION_GET"

        strParamName = "LOCCODE|ACCYEAR|ACCMONTH|TRXDATE|ACCCODE"
        strParamValue = strLocation & "|" & strAccYear & "|" & strAccMonth & "|" & strDate & "|" & strBankAccCode

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCodeBank, _
                                                strParamName, _
                                                strParamValue, _
                                                objAccDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ID_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        If objAccDs.Tables(0).Rows.Count > 0 Then
            txtBankAccNo1.Text = Trim(objAccDs.Tables(0).Rows(0).Item("BankAccNo"))
        End If
    End Sub

    Sub BindAccountList(ByVal pv_strAccCode As String)
        Dim strOpCdGet As String = "GL_CLSSETUP_ACCOUNTCODE_LIST_GET"
        Dim strSearch As String
        Dim strSort As String
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer
        Dim dr As DataRow

        strSearch = "and AccType = '" & objGLSetup.EnumAccountType.ProfitAndLost & "' " & _
                    "and AccPurpose = '" & objGLSetup.EnumAccountPurpose.NonVehicle & "' "

        strSort = "order by AccCode"

        strParam = strSearch & "|" & strSort

        Try
            intErrNo = objCMSetup.mtdGetMasterList(strOpCdGet, strParam, 0, objAccDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_ACCOUNTLIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegList.aspx")
        End Try

        If objAccDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objAccDs.Tables(0).Rows.Count - 1
                objAccDs.Tables(0).Rows(intCnt).Item("AccCode") = Trim(objAccDs.Tables(0).Rows(intCnt).Item("AccCode"))
                objAccDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objAccDs.Tables(0).Rows(intCnt).Item("AccCode")) & "(" & Trim(objAccDs.Tables(0).Rows(intCnt).Item("Description")) & ")"
                If Trim(objAccDs.Tables(0).Rows(intCnt).Item("AccCode")) = Trim(pv_strAccCode) Then
                    intSelectedIndex = intCnt + 1
                End If
            Next
        End If

        dr = objAccDs.Tables(0).NewRow()
        dr("AccCode") = ""
        dr("Description") = lblSelect.Text & lblAccount.Text
        objAccDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlAccount.DataSource = objAccDs.Tables(0)
        ddlAccount.DataValueField = "AccCode"
        ddlAccount.DataTextField = "Description"
        ddlAccount.DataBind()
        ddlAccount.SelectedIndex = intSelectedIndex

    End Sub


    Sub BindBlockList(ByVal pv_strBlockCode As String, ByVal pv_strAccCode As String)
        Dim strOpCdGet As String
        Dim strSearch As String
        Dim strSort As String
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer
        Dim dr As DataRow

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
            strOpCdGet = "CM_CLSTRX_CONTRACTREG_BLOCK_LIST_GET"
            strSearch = "and ln.AccCode = '" & pv_strAccCode & "' " & _
                        "and blk.LocCode = '" & strLocation & "' " & _
                        "and ln.LocCode = '" & strLocation & "' " & _
                        "and blk.Status = '" & objGLSetup.EnumBlockStatus.Active & "' "
            strSort = "order by blk.BlkCode"
        Else
            strOpCdGet = "CM_CLSTRX_CONTRACTREG_SUBBLOCK_LIST_GET"
            strSearch = "and ln.AccCode = '" & pv_strAccCode & "' " & _
                        "and subblk.LocCode = '" & strLocation & "' " & _
                        "and ln.LocCode = '" & strLocation & "' " & _
                        "and subblk.Status = '" & objGLSetup.EnumSubBlockStatus.Active & "' "
            strSort = "order by subblk.SubBlkCode"
        End If

        strParam = strSearch & "|" & strSort

        Try
            intErrNo = objCMSetup.mtdGetMasterList(strOpCdGet, strParam, 0, objBlkDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_BLOCKLIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegList.aspx")
        End Try

        If objBlkDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objBlkDs.Tables(0).Rows.Count - 1
                If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
                    objBlkDs.Tables(0).Rows(intCnt).Item("BlkCode") = Trim(objBlkDs.Tables(0).Rows(intCnt).Item("BlkCode"))
                    objBlkDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objBlkDs.Tables(0).Rows(intCnt).Item("BlkCode")) & "(" & Trim(objBlkDs.Tables(0).Rows(intCnt).Item("Description")) & ")"
                    If objBlkDs.Tables(0).Rows(intCnt).Item("BlkCode") = pv_strBlockCode Then
                        intSelectedIndex = intCnt + 1
                    End If
                Else
                    objBlkDs.Tables(0).Rows(intCnt).Item("SubBlkCode") = Trim(objBlkDs.Tables(0).Rows(intCnt).Item("SubBlkCode"))
                    objBlkDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objBlkDs.Tables(0).Rows(intCnt).Item("SubBlkCode")) & "(" & Trim(objBlkDs.Tables(0).Rows(intCnt).Item("Description")) & ")"
                    If objBlkDs.Tables(0).Rows(intCnt).Item("SubBlkCode") = pv_strBlockCode Then
                        intSelectedIndex = intCnt + 1
                    End If
                End If
            Next
        End If

        dr = objBlkDs.Tables(0).NewRow()
        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
            dr("BlkCode") = ""
        Else
            dr("SubBlkCode") = ""
        End If
        dr("Description") = lblSelect.Text & lblBlock.Text
        objBlkDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlBlock.DataSource = objBlkDs.Tables(0)
        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
            ddlBlock.DataValueField = "BlkCode"
        Else
            ddlBlock.DataValueField = "SubBlkCode"
        End If
        ddlBlock.DataTextField = "Description"
        ddlBlock.DataBind()
        ddlBlock.SelectedIndex = intSelectedIndex
    End Sub

    Sub onChanged_AccCode(ByVal Sender As Object, ByVal E As EventArgs)
        Dim strAccCode As String

        strAccCode = Trim(ddlAccount.SelectedItem.Value)

        BindBlockList("", strAccCode)

    End Sub

    Function CheckDate(ByVal pv_strDate As String, ByRef pr_strDate As String) As Boolean
        Dim strDateFormatCode As String = Session("SS_DATEFMT")
        Dim strDateFormat As String

        pr_strDate = ""
        CheckDate = True
        If Not pv_strDate = "" Then
            If objGlobal.mtdValidInputDate(strDateFormatCode, pv_strDate, strDateFormat, pr_strDate) = False Then
                lblContractDateFmt.Text = strDateFormat
                lblDelivMonthFmt.Text = strDateFormat
                pr_strDate = ""
                CheckDate = False
            End If
        End If
    End Function

    Function Date_Validation(ByVal pv_strInputDate As String, ByVal pv_blnIsShortDate As Boolean) As String
        Dim objSysCfgDs As New Object
        Dim objActualDate As New Object
        Dim strDateFormat As String
        Dim strParam As String
        Dim intErrNo As Integer
        Dim strAcceptFormat As String

        strParam = "PWSYSTEM_CLSCONFIG_CONFIG_DATEFMT_GET"

        Try
            intErrNo = objSysCfg.mtdGetConfigInfo(strParam, _
                                                  strCompany, _
                                                  strLocation, _
                                                  strUserId, _
                                                  objSysCfgDs)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_DEPOSIT_GET_CONFIG&errmesg=" & Exp.Message & "&redirect=CB/trx/cb_trx_DepositList.aspx")
        End Try

        strDateFormat = objSysCfg.mtdGetDateFormat(objSysCfgDs.Tables(0).Rows(0).Item("Datefmt").Trim())

        If pv_blnIsShortDate Then
            Date_Validation = objGlobal.GetShortDate(strDateFormat, pv_strInputDate)
        Else
            If objGlobal.mtdValidInputDate(strDateFormat, _
                                           pv_strInputDate, _
                                           strAcceptFormat, _
                                           objActualDate) = True Then
                Date_Validation = objActualDate
            Else
                Date_Validation = ""
            End If
        End If
    End Function

    Sub onLoad_Display(ByVal pv_strContNo As String)
        Dim strOpCd As String = "CM_CLSTRX_CONTRACT_REG_GET_DTL"
        Dim strOpCdCheckMatchStatus As String = "CM_CLSTRX_CONTRACT_REG_GET_MATCH_STATUS"
        Dim strParam As String
        Dim strParamCheckMatchStatus As String
        Dim intErrNo As Integer
        Dim strSearch As String
        Dim strSearchMatchStatus As String
        Dim strBalQty As String

        strSearch = "and ctr.LocCode = '" & strLocation & "' and ctr.ContractNo = '" & pv_strContNo & "' "
        strParam = strSearch & "|" & ""

        strSearchMatchStatus = "where match.Status = '" & objCMTrx.EnumContractMatchStatus.Active & "' " & strSearch
        strParamCheckMatchStatus = strSearchMatchStatus & "|" & ""

        Try
            intErrNo = objCMTrx.mtdGetContract(strOpCd, strParam, 0, objContractDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegList.aspx")
        End Try

        Try
            intErrNo = objCMTrx.mtdGetContract(strOpCdCheckMatchStatus, strParamCheckMatchStatus, 0, objMatchDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_CHECK_ACTIVE_MATCH&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegList.aspx")
        End Try

        If objMatchDs.Tables(0).Rows.count > 0 Then
            lblActiveMatchExist.Text = "1"
        End If

        txtContNo.Text = Trim(objContractDs.Tables(0).Rows(0).Item("ContractNo"))
        rdContType.Items(0).Selected = IIf(CInt(Trim(objContractDs.Tables(0).Rows(0).Item("ContType"))) = objCMTrx.EnumContType.Lokal, True, False)
        rdContType.Items(1).Selected = IIf(CInt(Trim(objContractDs.Tables(0).Rows(0).Item("ContType"))) = objCMTrx.EnumContType.Export, True, False)

        rdContractType.Items(0).Selected = IIf(CInt(Trim(objContractDs.Tables(0).Rows(0).Item("ContractType"))) = objCMTrx.EnumContractType.Purchase, True, False)
        rdContractType.Items(1).Selected = IIf(CInt(Trim(objContractDs.Tables(0).Rows(0).Item("ContractType"))) = objCMTrx.EnumContractType.Sales, True, False)

        onLoad_TableRow(CInt(Trim(objContractDs.Tables(0).Rows(0).Item("ContractType"))))

        txtContractDate.Text = objGlobal.GetShortDate(strDateFMt, objContractDs.Tables(0).Rows(0).Item("ContractDate"))
        BindProductList(Trim(objContractDs.Tables(0).Rows(0).Item("ProductCode")))
        BindSellerList(Trim(objContractDs.Tables(0).Rows(0).Item("SellerCode")))
        BindBuyerList(Trim(objContractDs.Tables(0).Rows(0).Item("BuyerCode")))
        txtContractQty.Text = objContractDs.Tables(0).Rows(0).Item("ContractQty")
        txtExtraQty.Text = objContractDs.Tables(0).Rows(0).Item("ExtraQty")

        BindExtraQtyTypeList(Trim(objContractDs.Tables(0).Rows(0).Item("ExtraQtyType")))

        BindContCategory(Trim(objContractDs.Tables(0).Rows(0).Item("ContCategory")))
        BindTermOfDelivery(Trim(objContractDs.Tables(0).Rows(0).Item("TermOfDelivery")))

        lblMatchedQty.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(Trim(objContractDs.Tables(0).Rows(0).Item("MatchedQty")), CInt(Session("SS_ROUNDNO")))
        strBalQty = Trim(objContractDs.Tables(0).Rows(0).Item("ContractQty")) - Trim(objContractDs.Tables(0).Rows(0).Item("MatchedQty"))
        lblBalQty.Text = strBalQty 'ObjGlobal.GetIDDecimalSeparator_FreeDigit(strBalQty, 5)
        BindCurrencyList(Trim(objContractDs.Tables(0).Rows(0).Item("CurrencyCode")))
        'txtPrice.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(Trim(objContractDs.Tables(0).Rows(0).Item("Price")), 2), 2)
        txtPrice.Text = objContractDs.Tables(0).Rows(0).Item("Price")
        BindPriceBasisList(Trim(objContractDs.Tables(0).Rows(0).Item("PriceBasisCode")))

        txtDelivMonth.Text = objGlobal.GetShortDate(strDateFMt, objContractDs.Tables(0).Rows(0).Item("DelivMonth"))

        taRemarks.Value = Trim(objContractDs.Tables(0).Rows(0).Item("Remarks"))
        intStatus = CInt(Trim(objContractDs.Tables(0).Rows(0).Item("Status")))
        lblHiddenSts.Text = Trim(objContractDs.Tables(0).Rows(0).Item("Status"))
        lblStatus.Text = objCMTrx.mtdGetContractStatus(Trim(objContractDs.Tables(0).Rows(0).Item("Status")))
        lblDateCreated.Text = objGlobal.GetLongDate(objContractDs.Tables(0).Rows(0).Item("CreateDate"))
        lblLastUpdate.Text = objGlobal.GetLongDate(objContractDs.Tables(0).Rows(0).Item("UpdateDate"))
        lblUpdatedBy.Text = Trim(objContractDs.Tables(0).Rows(0).Item("UserName"))

        BindAccountList(Trim(objContractDs.Tables(0).Rows(0).Item("AccCode")))
        BindBlockList(Trim(objContractDs.Tables(0).Rows(0).Item("BlkCode")), Trim(objContractDs.Tables(0).Rows(0).Item("AccCode")))

        taProductQuality.Value = Trim(objContractDs.Tables(0).Rows(0).Item("ProductQuality"))
        taProductClaim.Value = Trim(objContractDs.Tables(0).Rows(0).Item("ClaimQuality"))
        taTermOfPayment.Value = Trim(objContractDs.Tables(0).Rows(0).Item("TermOfPayment"))
        taTermOfWeighing.Value = Trim(objContractDs.Tables(0).Rows(0).Item("TermOfWeighing"))
        BindBankCode(ddlBankCode1, objContractDs.Tables(0).Rows(0).Item("BankCode"))
        'BindBankCode(ddlBankCode2, objContractDs.Tables(0).Rows(0).Item("BankCode2"))
        txtBankAccNo1.Text = Trim(objContractDs.Tables(0).Rows(0).Item("BankAccNo"))
        txtBankAccNo2.Text = Trim(objContractDs.Tables(0).Rows(0).Item("BankAccNo2"))

        BindCurrencyOAList(Trim(objContractDs.Tables(0).Rows(0).Item("CurrencyCodeOA")))
        txtPriceOA.Text = Round(objContractDs.Tables(0).Rows(0).Item("PriceOA"), CInt(Session("SS_ROUNDNO")))
        taDeliveryTime.Value = objContractDs.Tables(0).Rows(0).Item("TimeOfDelivery")
        taConsignment.Value = objContractDs.Tables(0).Rows(0).Item("Consignment")
        txtBuyerNo.Text = Trim(objContractDs.Tables(0).Rows(0).Item("BuyerNo"))
        txtUnderName.Text = Trim(objContractDs.Tables(0).Rows(0).Item("UnderName"))
        txtUnderPost.Text = Trim(objContractDs.Tables(0).Rows(0).Item("UnderPost"))

    End Sub

    Sub onChange_ContractType(ByVal Sender As Object, ByVal E As EventArgs)
        Dim strContractType As String
        strContractType = rdContractType.SelectedItem.Value

        onLoad_TableRow(CInt(strContractType))

        If rdContractType.SelectedValue = "2" AndAlso _
                txtContNo.Text.Trim().Length() > 0 Then
            PrintBtn.Visible = False 'True
        Else
            PrintBtn.Visible = False
        End If
    End Sub

    Sub onLoad_TableRow(ByVal pv_intContractType As Integer)
        trSeller.Visible = False
        trBuyer.Visible = False
        If pv_intContractType = objCMTrx.EnumContractType.Sales Then
            trBuyer.Visible = True
        Else
            trSeller.Visible = True
        End If
    End Sub

    Sub onChange_ContType(ByVal Sender As Object, ByVal E As EventArgs)
        Dim strContType As String
        strContType = rdContType.SelectedItem.Value
    End Sub

    Sub onLoad_BindButton()
        txtContNo.Enabled = False
        txtContractQty.Enabled = True

        rdContractType.Enabled = False
        txtContractDate.Enabled = False
        btnContractDate.Visible = False
        ddlProduct.Enabled = False
        ddlSeller.Enabled = False
        ddlBuyer.Enabled = False
        'txtExtraQty.Enabled = False
        'ddlExtraQtyType.Enabled = False
        ddlCurrency.Enabled = False
        txtPrice.Enabled = False
        ddlPriceBasis.Enabled = False

        txtDelivMonth.Enabled = False
        btnDelivMonth.Visible = False
        lblPriceBasis.Visible = False
        ddlPriceBasis.Visible = False

        taRemarks.Disabled = True
        ddlAccount.Enabled = False
        ddlBlock.Enabled = False
        SaveBtn.Visible = False
        CloseBtn.Visible = False
        DelBtn.Visible = False
        UnDelBtn.Visible = False

        rdContType.Enabled = False
        ddlTerm.Enabled = False
        ddlContCategory.Enabled = False


        ddlBankCode1.Enabled = False
        ddlBankCode2.Enabled = False
        txtBankAccNo1.Enabled = False
        txtBankAccNo2.Enabled = False
        taProductQuality.Disabled = True
        taTermOfPayment.Disabled = True
        PrintBtn.Visible = False

        Select Case intStatus
            Case objCMTrx.EnumContractStatus.Active
                rdContractType.Enabled = True

                rdContType.Enabled = True
                ddlTerm.Enabled = True
                ddlContCategory.Enabled = True

                txtContractDate.Enabled = True
                btnContractDate.Visible = True
                ddlProduct.Enabled = True
                ddlSeller.Enabled = True
                ddlBuyer.Enabled = True
                txtExtraQty.Enabled = True
                ddlExtraQtyType.Enabled = True
                ddlCurrency.Enabled = True
                txtPrice.Enabled = True 'False
                ddlPriceBasis.Enabled = True

                txtDelivMonth.Enabled = True
                btnDelivMonth.Visible = True
                lblPriceBasis.Visible = False
                ddlPriceBasis.Visible = False

                ddlBankCode1.Enabled = True
                ddlBankCode2.Enabled = True
                txtBankAccNo1.Enabled = True
                txtBankAccNo2.Enabled = True
                taProductQuality.Disabled = False
                taTermOfPayment.Disabled = False

                txtContNo.Enabled = False
                txtContractQty.Enabled = True

                taRemarks.Disabled = False
                ddlAccount.Enabled = False
                ddlBlock.Enabled = False
                SaveBtn.Visible = True
                DelBtn.Visible = True
                DelBtn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                'If lblActiveMatchExist.Text = "0" And lblBalQty.Text <= "0" Then
                CloseBtn.Visible = True
                'End If
            Case objCMTrx.EnumContractStatus.Deleted
                UnDelBtn.Visible = True
            Case objCMTrx.EnumContractStatus.Closed
                txtContractQty.Enabled = False
            Case Else
                rdContractType.Enabled = True
                rdContractType.Items(1).Selected = True

                rdContType.Enabled = True
                ddlTerm.Enabled = True
                ddlContCategory.Enabled = True

                txtContractDate.Enabled = True
                btnContractDate.Visible = True
                ddlProduct.Enabled = True
                ddlSeller.Enabled = True
                ddlBuyer.Enabled = True
                txtContractQty.Enabled = True
                txtContractQty.Text = "0"
                'txtExtraQty.Enabled = False
                txtExtraQty.Text = "0"
                'ddlExtraQtyType.Enabled = False
                lblMatchedQty.Text = "0"
                lblBalQty.Text = "0"
                ddlCurrency.Enabled = True
                txtPrice.Enabled = True
                ddlPriceBasis.Enabled = True

                txtDelivMonth.Enabled = True
                btnDelivMonth.Visible = True
                lblPriceBasis.Visible = False
                ddlPriceBasis.Visible = False

                ddlBankCode1.Enabled = True
                ddlBankCode2.Enabled = True
                txtBankAccNo1.Enabled = True
                txtBankAccNo2.Enabled = True
                taProductQuality.Disabled = False
                taTermOfPayment.Disabled = False
                rdContType.Items(0).Selected = True

                txtContNo.Enabled = False
                txtContractQty.Enabled = True

                taRemarks.Disabled = False
                ddlAccount.Enabled = False
                ddlBlock.Enabled = False
                SaveBtn.Visible = True
        End Select

        If rdContractType.SelectedValue = "2" AndAlso _
                txtContNo.Text.Trim().Length() > 0 Then
            PrintBtn.Visible = True
        End If
    End Sub

    Sub Button_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strCmdArgs As String = CType(Sender, ImageButton).CommandArgument
        Dim strOpCd_Upd As String = "CM_CLSTRX_CONTRACT_REG_UPD"
        Dim strOpCd_Get As String = "CM_CLSTRX_CONTRACT_REG_GET"
        Dim strOpCd_Add As String = "CM_CLSTRX_CONTRACT_REG_ADD"
        Dim intErrNo As Integer
        Dim blnIsUpdate As Boolean
        Dim strParam As String = ""

        Dim strContractType As Integer
        Dim strProductCode As String
        Dim strSellerCode As String
        Dim strBuyerCode As String
        Dim decContractQty As Decimal
        Dim decExtraQty As Decimal
        Dim strExtraQtyType As String
        Dim decMatchedQty As Decimal
        Dim decBalQty As Decimal
        Dim strCurrencyCode As String
        Dim decPrice As Decimal
        Dim strPriceBasisCode As String
        Dim strDelMonth As String
        Dim strDelYear As String
        Dim strRemarks As String
        Dim strAccCode As String
        Dim strBlkCode As String
        Dim blnDupKey As Boolean

        Dim strOppCd As String = "IN_CLSTRX_PURREQ_MOVEID"
        Dim strOppCd_Back As String = "IN_CLSTRX_PURREQ_BACKID"
        Dim strOppCd_GetID As String = "IN_CLSTRX_PURREQ_GETID"
        Dim strContType As Integer
        Dim objCMID As Object
        Dim objPRDs As Object
        Dim strRunNo As Integer
        Dim strProdType As String

        Dim strNewIDFormat As String
        Dim strNewYear As String = ""
        Dim strTranPrefix As String = "CMR"
        Dim strHistYear As String = ""
        Dim objCompDs As New Object
        Dim blnIsDetail As Boolean = True
        Dim strRunNumber As String
        Dim strContCategory As String = ddlContCategory.SelectedItem.Value
        Dim strTerm As String = ddlTerm.SelectedItem.Value
        Dim strFlgContType As String = ""

        Dim strBankCode As String = ddlBankCode1.SelectedValue
        Dim strBankAccNo As String = txtBankAccNo1.Text
        Dim strBankCode2 As String = ddlBankCode2.SelectedValue
        Dim strBankAccNo2 As String = txtBankAccNo2.Text
        Dim strProductQuality As String = taProductQuality.Value
        Dim strTermOfPayment As String = taTermOfPayment.Value

        Dim strBuyerNo As String = txtBuyerNo.Text.Trim
        Dim strCurrencyOA As String = ddlCurrencyOA.SelectedItem.Value.Trim
        Dim decPriceOA As Decimal = txtPriceOA.Text
        Dim strProductClaim As String = taProductClaim.Value.Trim
        Dim strDeliveryTime As String = taDeliveryTime.Value.Trim
        Dim strTermOfWeighing As String = taTermOfWeighing.Value.Trim
        Dim strConsignment As String = taConsignment.Value.Trim

        Dim strDateContract As String = Date_Validation(txtContractDate.Text, False)
        Dim strDateDelivery As String = Date_Validation(txtDelivMonth.Text, False)
        Dim indDate As String = ""
		Dim strAccMonthRom As String

        Dim strBank As String
        'Dim strBankAccCode As String
        'Dim strBankAccNo As String
        Dim arrParam As Array

        If ddlBankCode1.SelectedItem.Value = "" Then
            strBankCode = ""
            'strBankAccCode = ""
            'strBankAccNo = ""
        Else
            arrParam = Split(Trim(ddlBankCode1.SelectedItem.Value), "|")
            strBankCode = Trim(arrParam(0))
            'strBankAccCode = Trim(arrParam(1))
            'strBankAccNo = Trim(arrParam(2))
        End If

        strContractNo = ""

        If rdContType.Items(0).Selected = True Then
            strContType = "1"
        Else
            strContType = "2"
        End If

        If rdContractType.Items(0).Selected = True Then
            strContractType = "1"
        Else
            strContractType = "2"
        End If

        If CheckDate(txtContractDate.Text.Trim(), indDate) = False Then
            lblContractDate.Visible = True
            lblContractDateFmt.Visible = True
            lblContractDate.Text = "<br>Date Entered should be in the format"
            Exit Sub
        End If

        If CheckDate(txtDelivMonth.Text.Trim(), indDate) = False Then
            lblDelivMonth.Visible = True
            lblDelivMonthFmt.Visible = True
            lblDelivMonth.Text = "<br>Date Entered should be in the format"
            Exit Sub
        End If


        strProductCode = ddlProduct.SelectedItem.Value
        strSellerCode = ddlSeller.SelectedItem.Value
        strBuyerCode = ddlBuyer.SelectedItem.Value

        If CInt(strContractType) = objCMTrx.EnumContractType.Purchase Then
            strBuyerCode = strCompany
        Else
            strSellerCode = strCompany
        End If

        If Trim(txtContractQty.Text) = "" Then
            decContractQty = 0
        Else
            decContractQty = Trim(txtContractQty.Text)
        End If

        If Trim(txtExtraQty.Text) = "" Then
            decExtraQty = 0
        Else
            decExtraQty = Trim(txtExtraQty.Text)
        End If

        strExtraQtyType = ddlExtraQtyType.SelectedItem.Value

        If Trim(lblMatchedQty.Text) = "" Then
            decMatchedQty = 0
        Else
            'decMatchedQty = Replace(Replace(Trim(lblMatchedQty.Text), ".", ""), ",", ".")
            decMatchedQty = Trim(lblMatchedQty.Text)
        End If

        If Trim(lblBalQty.Text) = "" Then
            decBalQty = 0
        Else
            'decBalQty = Replace(Replace(Trim(lblBalQty.Text), ".", ""), ",", ".")
            decBalQty = Trim(lblBalQty.Text)
        End If

        strCurrencyCode = ddlCurrency.SelectedItem.Value

        If Trim(txtPrice.Text) = "" Then
            decPrice = 0
        Else
            'decPrice = Replace(Replace(Trim(txtPrice.Text), ".", ""), ",", ".")
            decPrice = Trim(txtPrice.Text)
        End If

        If Validate_ContractQty(decContractQty, decExtraQty, CInt(strExtraQtyType), decMatchedQty) = False Then
            lblErrContractQty.Visible = True
            Exit Sub
        End If

        strPriceBasisCode = ddlPriceBasis.SelectedItem.Value

        strRemarks = taRemarks.Value
        strAccCode = ddlAccount.SelectedItem.Value
        strBlkCode = ddlBlock.SelectedItem.Value

        'If Len(strPhyMonth) = 1 Then
        '    strPhyMonth = "0" & strPhyMonth
        'End If

        'strParam = "where phyyear = '" & Right(Trim(strPhyYear), 2) & "' and tran_prefix = 'CMR'" & "|"
        'Try
        '    intErrNo = objIN.mtdGetPurchaseRequest(strOppCd_GetID, _
        '                                           strParam, _
        '                                           objIN.EnumPurReqDocType.StockPR, _
        '                                           strAccMonth, _
        '                                           strAccYear, _
        '                                           strLocation, _
        '                                           objPRDs)
        'Catch Exp As System.Exception
        '    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=PURREQ_LIST_GET_DISPLAYPR&errmesg=" & lblErrMessage.Text & "&redirect=IN/trx/IN_PurReq.aspx")
        'End Try


        'If objPRDs.Tables(0).Rows.Count > 0 Then
        '    strNewYear = ""
        '    strRunNo = Trim(objPRDs.Tables(0).Rows(0).Item("Val"))
        'Else
        '    strHistYear = Right(strLastPhyYear, 2)
        '    strNewYear = "1"
        '    strRunNo = 0
        'End If

        'If ddlProduct.SelectedItem.Value = objWMTrx.EnumWeighBridgeTicketProduct.CPO Then
        '    strProdType = objWMTrx.mtdGetProdType(objWMTrx.EnumProdType.CPO)
        'ElseIf ddlProduct.SelectedItem.Value = objWMTrx.EnumWeighBridgeTicketProduct.FFB Then
        '    strProdType = objWMTrx.mtdGetProdType(objWMTrx.EnumProdType.FFB)
        'ElseIf ddlProduct.SelectedItem.Value = objWMTrx.EnumWeighBridgeTicketProduct.PK Then
        '    strProdType = objWMTrx.mtdGetProdType(objWMTrx.EnumProdType.PK)
        'End If

		
		
        'Select Case ddlProduct.SelectedItem.Value
        '    Case objWMTrx.EnumWeighBridgeTicketProduct.CPO
        '        strProdType = "CPO"
        '    Case objWMTrx.EnumWeighBridgeTicketProduct.PK
        '        strProdType = "PK"
        '    Case objWMTrx.EnumWeighBridgeTicketProduct.FFB
        '        strProdType = "FFB"
        '    Case objWMTrx.EnumWeighBridgeTicketProduct.AbuJanjang
        '        strProdType = "ABJ"
        '    Case objWMTrx.EnumWeighBridgeTicketProduct.MinyakLimbah
        '        strProdType = "LMB"
        '    Case objWMTrx.EnumWeighBridgeTicketProduct.Shell
        '        strProdType = "CK"
        '    Case objWMTrx.EnumWeighBridgeTicketProduct.EFB
        '        strProdType = "FB"
        '    Case objWMTrx.EnumWeighBridgeTicketProduct.Others
        '        strProdType = "OTH"
        'End Select

        Select Case strProductCode
            Case objWMTrx.EnumWeighBridgeTicketProduct.FFB
                strProdType = "FFB"
            Case objWMTrx.EnumWeighBridgeTicketProduct.CPO
                strProdType = "CPO"
            Case objWMTrx.EnumWeighBridgeTicketProduct.PK
                strProdType = "PK"
            Case objWMTrx.EnumWeighBridgeTicketProduct.Others
                strProdType = "OTH"
            Case objWMTrx.EnumWeighBridgeTicketProduct.EFB
                strProdType = "JJK" '"EFB"
            Case objWMTrx.EnumWeighBridgeTicketProduct.Shell
                strProdType = "CKG"
            Case objWMTrx.EnumWeighBridgeTicketProduct.AbuJanjang
                strProdType = "ABJ"
            Case objWMTrx.EnumWeighBridgeTicketProduct.Fiber
                strProdType = "FBR"
            Case objWMTrx.EnumWeighBridgeTicketProduct.Brondolan
                strProdType = "BRD"
            Case objWMTrx.EnumWeighBridgeTicketProduct.Solid
                strProdType = "SLD"
            Case objWMTrx.EnumWeighBridgeTicketProduct.MinyakLimbah
                strProdType = "LMB"
            Case objWMTrx.EnumWeighBridgeTicketProduct.EFBOil
                strProdType = "POME"
			Case objWMTrx.EnumWeighBridgeTicketProduct.MinyakKolam
                strProdType = "MKL"
            Case objWMTrx.EnumWeighBridgeTicketProduct.EFBPress
                strProdType = "EBP"
        End Select


        strAccYear = Year(strDateContract)
        strAccMonth = Month(strDateContract)

		If strAccMonth = "1" Then
            strAccMonthRom = "I"
        ElseIf strAccMonth = "2" Then
            strAccMonthRom = "II"
        ElseIf strAccMonth = "3" Then
            strAccMonthRom = "III"
        ElseIf strAccMonth = "4" Then
            strAccMonthRom = "IV"
        ElseIf strAccMonth = "5" Then
            strAccMonthRom = "V"
        ElseIf strAccMonth = "6" Then
            strAccMonthRom = "VI"
        ElseIf strAccMonth = "7" Then
            strAccMonthRom = "VII"
        ElseIf strAccMonth = "8" Then
            strAccMonthRom = "VIII"
        ElseIf strAccMonth = "9" Then
            strAccMonthRom = "IX"
        ElseIf strAccMonth = "10" Then
            strAccMonthRom = "X"
        ElseIf strAccMonth = "11" Then
            strAccMonthRom = "XI"
        Else
            strAccMonthRom = "XII"
        End If
		
        strNewIDFormat = "/" & Trim(strCompany) & "/" & Trim(strProdType) & "/" & Trim(strAccMonthRom) & "/" & Trim(strAccYear)
        'strNewIDFormat = "/SSJA/PER-" & Trim(strProdType) & "/" & IIf(Len(Trim(strAccMonth)) = 1, "0" & strAccMonth, strAccMonth) & Mid(Trim(strAccYear), 3, 2)

        If strCmdArgs = "Save" Then

            blnIsUpdate = IIf(intStatus = 0, False, True)
            tbcode.Value = strContractNo
            strContractNo = txtContNo.Text

            strParam = strContractNo & Chr(9) & _
                       strContractType & Chr(9) & _
                       strDateContract & Chr(9) & _
                       strProductCode & Chr(9) & _
                       strSellerCode & Chr(9) & _
                       strBuyerCode & Chr(9) & _
                       decContractQty & Chr(9) & _
                       decExtraQty & Chr(9) & _
                       strExtraQtyType & Chr(9) & _
                       decMatchedQty & Chr(9) & _
                       strCurrencyCode & Chr(9) & _
                       decPrice & Chr(9) & _
                       strPriceBasisCode & Chr(9) & _
                       strDelMonth & Chr(9) & _
                       strDelYear & Chr(9) & _
                       strRemarks & Chr(9) & _
                       strAccCode & Chr(9) & _
                       strBlkCode & Chr(9) & _
                       objCMTrx.EnumContractStatus.Active & Chr(9) & _
                       strContType & Chr(9) & _
                       strNewIDFormat & Chr(9) & strNewYear & Chr(9) & strTranPrefix & Chr(9) & strHistYear & Chr(9) & Right(strPhyYear, 2) & Chr(9) & _
                       strContCategory & Chr(9) & _
                       strTerm & Chr(9) & _
                       strBankCode & Chr(9) & _
                       strBankAccNo & Chr(9) & _
                       strDateDelivery & Chr(9) & _
                       strBankCode2 & Chr(9) & _
                       strBankAccNo2 & Chr(9) & _
                       strProductQuality & Chr(9) & _
                       strTermOfPayment & Chr(9) & _
                       strCurrencyOA & Chr(9) & _
                       Round(decPriceOA, CInt(Session("SS_ROUNDNO"))) & Chr(9) & _
                       strProductClaim & Chr(9) & _
                       strDeliveryTime & Chr(9) & _
                       strTermOfWeighing & Chr(9) & _
                       strConsignment & Chr(9) & _
                       strBuyerNo & Chr(9) & _
                       strAccMonth & Chr(9) & _
                       strAccYear & Chr(9) & _
                       txtUnderName.Text.Trim & Chr(9) & _
                       txtUnderPost.Text.Trim

            Try
                intErrNo = objCMTrx.mtdUpdContract(strOpCd_Get, _
                                                   strOpCd_Add, _
                                                   strOpCd_Upd, _
                                                   strOppCd, _
                                                   strCompany, _
                                                   strLocation, _
                                                   strUserId, _
                                                   strParam, _
                                                   blnDupKey, _
                                                   objCMID, _
                                                   blnIsUpdate, _
                                                   "CMR")

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_SAVE&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegDet.aspx?tbcode=" & strContractNo)
            End Try

            If blnDupKey = True Then
                Exit Sub
            End If

            strContractNo = objCMID
            onLoad_Display(strContractNo)

        ElseIf strCmdArgs = "Close" Then
            strContractNo = txtContNo.Text

            strParam = strContractNo & Chr(9) & _
                       strContractType & Chr(9) & _
                       strDateContract & Chr(9) & _
                       strProductCode & Chr(9) & _
                       strSellerCode & Chr(9) & _
                       strBuyerCode & Chr(9) & _
                       decContractQty & Chr(9) & _
                       decExtraQty & Chr(9) & _
                       strExtraQtyType & Chr(9) & _
                       decMatchedQty & Chr(9) & _
                       strCurrencyCode & Chr(9) & _
                       decPrice & Chr(9) & _
                       strPriceBasisCode & Chr(9) & _
                       strDelMonth & Chr(9) & _
                       strDelYear & Chr(9) & _
                       strRemarks & Chr(9) & _
                       strAccCode & Chr(9) & _
                       strBlkCode & Chr(9) & _
                       objCMTrx.EnumContractStatus.Closed & Chr(9) & _
                       strContType & Chr(9) & _
                       strNewIDFormat & Chr(9) & strNewYear & Chr(9) & strTranPrefix & Chr(9) & strHistYear & Chr(9) & Right(strPhyYear, 2) & Chr(9) & _
                       strContCategory & Chr(9) & _
                       strTerm & Chr(9) & _
                       strBankCode & Chr(9) & _
                       strBankAccNo & Chr(9) & _
                       strDateDelivery & Chr(9) & _
                       strBankCode2 & Chr(9) & _
                       strBankAccNo2 & Chr(9) & _
                       strProductQuality & Chr(9) & _
                       strTermOfPayment & Chr(9) & _
                       strCurrencyOA & Chr(9) & _
                       decPriceOA & Chr(9) & _
                       strProductClaim & Chr(9) & _
                       strDeliveryTime & Chr(9) & _
                       strTermOfWeighing & Chr(9) & _
                       strConsignment & Chr(9) & _
                       strBuyerNo & Chr(9) & _
                       strAccMonth & Chr(9) & _
                       strAccYear & Chr(9) & _
                       txtUnderName.Text.Trim & Chr(9) & _
                       txtUnderPost.Text.Trim

            Try
                intErrNo = objCMTrx.mtdUpdContract(strOpCd_Get, _
                                                   strOpCd_Add, _
                                                   strOpCd_Upd, _
                                                   strOppCd, _
                                                   strCompany, _
                                                   strLocation, _
                                                   strUserId, _
                                                   strParam, _
                                                   False, _
                                                   objCMID, _
                                                   True, _
                                                   "CMR")

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_CLOSE&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegDet.aspx?tbcode=" & strContractNo)
            End Try

            strContractNo = objCMID
            onLoad_Display(strContractNo)

        ElseIf strCmdArgs = "Del" Then
            strContractNo = txtContNo.Text

            strParam = strContractNo & Chr(9) & _
                       strContractType & Chr(9) & _
                       strDateContract & Chr(9) & _
                       strProductCode & Chr(9) & _
                       strSellerCode & Chr(9) & _
                       strBuyerCode & Chr(9) & _
                       decContractQty & Chr(9) & _
                       decExtraQty & Chr(9) & _
                       strExtraQtyType & Chr(9) & _
                       decMatchedQty & Chr(9) & _
                       strCurrencyCode & Chr(9) & _
                       decPrice & Chr(9) & _
                       strPriceBasisCode & Chr(9) & _
                       strDelMonth & Chr(9) & _
                       strDelYear & Chr(9) & _
                       strRemarks & Chr(9) & _
                       strAccCode & Chr(9) & _
                       strBlkCode & Chr(9) & _
                       objCMTrx.EnumContractStatus.Deleted & Chr(9) & _
                       strContType & Chr(9) & _
                       strNewIDFormat & Chr(9) & strNewYear & Chr(9) & strTranPrefix & Chr(9) & strHistYear & Chr(9) & Right(strPhyYear, 2) & Chr(9) & _
                       strContCategory & Chr(9) & _
                       strTerm & Chr(9) & _
                       strBankCode & Chr(9) & _
                       strBankAccNo & Chr(9) & _
                       strDateDelivery & Chr(9) & _
                       strBankCode2 & Chr(9) & _
                       strBankAccNo2 & Chr(9) & _
                       strProductQuality & Chr(9) & _
                       strTermOfPayment & Chr(9) & _
                       strCurrencyOA & Chr(9) & _
                       decPriceOA & Chr(9) & _
                       strProductClaim & Chr(9) & _
                       strDeliveryTime & Chr(9) & _
                       strTermOfWeighing & Chr(9) & _
                       strConsignment & Chr(9) & _
                       strBuyerNo & Chr(9) & _
                       strAccMonth & Chr(9) & _
                       strAccYear & Chr(9) & _
                       txtUnderName.Text.Trim & Chr(9) & _
                       txtUnderPost.Text.Trim

            Try
                intErrNo = objCMTrx.mtdUpdContract(strOpCd_Get, _
                                                   strOpCd_Add, _
                                                   strOpCd_Upd, _
                                                   strOppCd, _
                                                   strCompany, _
                                                   strLocation, _
                                                   strUserId, _
                                                   strParam, _
                                                   False, _
                                                   objCMID, _
                                                   True, _
                                                   "CMR")

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_DEL&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegDet.aspx?tbcode=" & strContractNo)
            End Try

            strContractNo = objCMID
            onLoad_Display(strContractNo)

        ElseIf strCmdArgs = "UnDel" Then
            strContractNo = txtContNo.Text

            strParam = strContractNo & Chr(9) & _
                       strContractType & Chr(9) & _
                       strDateContract & Chr(9) & _
                       strProductCode & Chr(9) & _
                       strSellerCode & Chr(9) & _
                       strBuyerCode & Chr(9) & _
                       decContractQty & Chr(9) & _
                       decExtraQty & Chr(9) & _
                       strExtraQtyType & Chr(9) & _
                       decMatchedQty & Chr(9) & _
                       strCurrencyCode & Chr(9) & _
                       decPrice & Chr(9) & _
                       strPriceBasisCode & Chr(9) & _
                       strDelMonth & Chr(9) & _
                       strDelYear & Chr(9) & _
                       strRemarks & Chr(9) & _
                       strAccCode & Chr(9) & _
                       strBlkCode & Chr(9) & _
                       objCMTrx.EnumContractStatus.Active & Chr(9) & _
                       strContType & Chr(9) & _
                       strNewIDFormat & Chr(9) & strNewYear & Chr(9) & strTranPrefix & Chr(9) & strHistYear & Chr(9) & Right(strPhyYear, 2) & Chr(9) & _
                       strContCategory & Chr(9) & _
                       strTerm & Chr(9) & _
                       strBankCode & Chr(9) & _
                       strBankAccNo & Chr(9) & _
                       strDateDelivery & Chr(9) & _
                       strBankCode2 & Chr(9) & _
                       strBankAccNo2 & Chr(9) & _
                       strProductQuality & Chr(9) & _
                       strTermOfPayment & Chr(9) & _
                       strCurrencyOA & Chr(9) & _
                       decPriceOA & Chr(9) & _
                       strProductClaim & Chr(9) & _
                       strDeliveryTime & Chr(9) & _
                       strTermOfWeighing & Chr(9) & _
                       strConsignment & Chr(9) & _
                       strBuyerNo & Chr(9) & _
                       strAccMonth & Chr(9) & _
                       strAccYear & Chr(9) & _
                       txtUnderName.Text.Trim & Chr(9) & _
                       txtUnderPost.Text.Trim

            Try
                intErrNo = objCMTrx.mtdUpdContract(strOpCd_Get, _
                                                   strOpCd_Add, _
                                                   strOpCd_Upd, _
                                                   strOppCd, _
                                                   strCompany, _
                                                   strLocation, _
                                                   strUserId, _
                                                   strParam, _
                                                   False, _
                                                   objCMID, _
                                                   True, _
                                                   "CMR")
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_UNDEL&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegDet.aspx?tbcode=" & strContractNo)
            End Try

            strContractNo = objCMID
            onLoad_Display(strContractNo)

        End If


        onLoad_BindButton()

    End Sub

    Protected Function Validate_ContractQty(ByVal decContractQty As Decimal, _
                                            ByVal decExtraQty As Decimal, _
                                            ByVal intExtraQtyType As Integer, _
                                            ByVal decMatchedQty As Decimal) As Boolean

        Dim decTotalContractQty As Decimal
        Dim blnValidate As Boolean

        If intExtraQtyType = objCMTrx.EnumExtraQtyType.Percentage Then
            decTotalContractQty = decContractQty + (decContractQty * decExtraQty / 100)
        Else
            decTotalContractQty = decContractQty + decExtraQty
        End If

        If decTotalContractQty - decMatchedQty < 0 Then
            blnValidate = False
        Else
            blnValidate = True
        End If
        Return blnValidate
    End Function

    Sub PrintBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strContractNo As String = txtContNo.Text
        Dim isBuyer As String = Trim(ddlBuyer.SelectedItem.Text)
        Dim strOwner1 As String = ""
        Dim strOwner2 As String = ""
        Dim strProduct As String = Trim(ddlProduct.SelectedItem.Value)
        Dim strBillParty As String = Trim(ddlBuyer.SelectedItem.Value)
        Dim arrBuyer As Array
        Dim strBuyer As String

        arrBuyer = Split(isBuyer, "(")
        If UBound(arrBuyer) > 0 Then
            strBuyer = Trim(arrBuyer(1))
        End If
        strBuyer = Replace(strBuyer, ")", "")

        Response.Write("<Script Language=""JavaScript"">window.open(""../reports/CM_Rpt_ContractRegDet.aspx?ContractNo=" & Server.UrlEncode(strContractNo) & _
                        "&Product=" & Server.UrlEncode(strProduct) & _
                        "&BillParty=" & Server.UrlEncode(strBillParty) & _
                        "&Buyer=" & Server.UrlEncode(strBuyer) & _
                        "&Owner1=" & Server.UrlEncode(strOwner1) & _
                        "&Owner2=" & Server.UrlEncode(strOwner2) & _
                        "&strExportToExcel=" & IIf(cbExcel.Checked, "1", "0") & _
                        """, null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")
    End Sub

    Sub BackBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CM_Trx_ContractRegList.aspx")
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblBillParty.Text = GetCaption(objLangCap.EnumLangCap.BillParty) & lblCode.Text
        lblBillPartyNo.Text = GetCaption(objLangCap.EnumLangCap.BillParty)
        lblAccount.Text = GetCaption(objLangCap.EnumLangCap.Account) & lblCode.Text
        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
            lblBlock.Text = GetCaption(objLangCap.EnumLangCap.Block) & lblCode.Text
        Else
            lblBlock.Text = GetCaption(objLangCap.EnumLangCap.SubBlock) & lblCode.Text
        End If
        rfvBuyer.ErrorMessage = lblPleaseSelect.Text & lblBillParty.Text & "."
        'rfvAccount.ErrorMessage = lblPleaseSelect.Text & lblAccount.Text & "."
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREG_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=CM/Setup/CM_Trx_ContractRegList.aspx")
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

    Sub BindContCategory(ByVal pv_strContCategory As String)
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer

        If ddlContCategory.Items.Count = 0 Then
            ddlContCategory.Items.Add(New ListItem("Select Contract Category", ""))
            ddlContCategory.Items.Add(New ListItem(objCMTrx.mtdGetContCategory(objCMTrx.EnumContCategory.LTCBiasa), objCMTrx.EnumContCategory.LTCBiasa))
            ddlContCategory.Items.Add(New ListItem(objCMTrx.mtdGetContCategory(objCMTrx.EnumContCategory.LTCForward), objCMTrx.EnumContCategory.LTCForward))
            ddlContCategory.Items.Add(New ListItem(objCMTrx.mtdGetContCategory(objCMTrx.EnumContCategory.SpotLokal), objCMTrx.EnumContCategory.SpotLokal))
            ddlContCategory.Items.Add(New ListItem(objCMTrx.mtdGetContCategory(objCMTrx.EnumContCategory.SpotExport), objCMTrx.EnumContCategory.SpotExport))
            ddlContCategory.Items.Add(New ListItem(objCMTrx.mtdGetContCategory(objCMTrx.EnumContCategory.Others), objCMTrx.EnumContCategory.Others))
        End If

        If Trim(pv_strContCategory) <> "" Then
            For intCnt = 0 To ddlContCategory.Items.Count - 1
                If ddlContCategory.Items(intCnt).Value = pv_strContCategory Then
                    intSelectedIndex = intCnt
                End If
            Next
            ddlContCategory.SelectedIndex = intSelectedIndex
        Else
            ddlContCategory.SelectedIndex = 3
        End If

    End Sub

    Sub BindTermOfDelivery(ByVal pv_strTerm As String)
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer

        'If ddlTerm.Items.Count = 0 Then
        '    ddlTerm.Items.Add(New ListItem("Select Term Of Delivery", ""))
        '    ddlTerm.Items.Add(New ListItem(objCMTrx.mtdGetTermOfDelivery(objCMTrx.EnumTermOfDelivery.Franco), objCMTrx.EnumTermOfDelivery.Franco))
        '    ddlTerm.Items.Add(New ListItem(objCMTrx.mtdGetTermOfDelivery(objCMTrx.EnumTermOfDelivery.Loco), objCMTrx.EnumTermOfDelivery.Loco))
        '    ddlTerm.Items.Add(New ListItem(objCMTrx.mtdGetTermOfDelivery(objCMTrx.EnumTermOfDelivery.CIF), objCMTrx.EnumTermOfDelivery.CIF))
        '    ddlTerm.Items.Add(New ListItem(objCMTrx.mtdGetTermOfDelivery(objCMTrx.EnumTermOfDelivery.FOB), objCMTrx.EnumTermOfDelivery.FOB))
        'End If

        If Trim(pv_strTerm) <> "" Then
            For intCnt = 0 To ddlTerm.Items.Count - 1
                If ddlTerm.Items(intCnt).Value = pv_strTerm Then
                    intSelectedIndex = intCnt
                End If
            Next
            ddlTerm.SelectedIndex = intSelectedIndex
        Else
            ddlTerm.SelectedIndex = 0
        End If

    End Sub

    Sub onChanged_BillParty(ByVal Sender As Object, ByVal E As EventArgs)
        Dim strParam As String
        Dim strOpCdGet As String = "GL_CLSSETUP_BILLPARTY_GET"
        Dim dr As DataRow
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer
        Dim strBillPartyCode as String = Request.Form("ddlBuyer")

        strParam = strBillPartyCode & "|" & _
                   "" & "|" & _
                   objGLSetup.EnumBillPartyStatus.Active & "|" & _
                   "" & "|" & _
                   "BP.BillPartyCode" & "|" & _
                   "ASC" & "|"
        Try
            intErrNo = objGLSetup.mtdGetBillParty(strOpCdGet, strParam, objBuyerDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_BUYERLIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegList.aspx")
        End Try

        If objBuyerDs.Tables(0).Rows.Count > 0 Then
            hidPPN.Value = Trim(objBuyerDs.Tables(0).Rows(0).Item("PPNInit"))
            hidTermOfWeighing.Value = Trim(objBuyerDs.Tables(0).Rows(0).Item("TermOfWeighing"))
            hidQualityCPO.Value = Trim(objBuyerDs.Tables(0).Rows(0).Item("QualityCPO"))
            hidClaimCPO.Value = Trim(objBuyerDs.Tables(0).Rows(0).Item("ClaimCPO"))
            hidQualityPK.Value = Trim(objBuyerDs.Tables(0).Rows(0).Item("QualityPK"))
            hidClaimPK.Value = Trim(objBuyerDs.Tables(0).Rows(0).Item("ClaimPK"))
            hidTermOfPayment.Value = Trim(objBuyerDs.Tables(0).Rows(0).Item("TermOfPayment"))
            hidConsignment.Value = Trim(objBuyerDs.Tables(0).Rows(0).Item("LoadDest"))
            taRemarks.Value = Trim(objBuyerDs.Tables(0).Rows(0).Item("AdditionalNote"))
            taTermOfWeighing.Value = hidTermOfWeighing.Value
            taTermOfPayment.Value = hidTermOfPayment.Value
            taConsignment.Value = hidConsignment.Value
        End If

    End Sub

    Sub CheckDisableContractQty()
        Dim strOpCdGet = "CM_CLSTRX_CONTRACTMATCH_LN_GET"
        Dim strparam As String = ""
        Dim intErrNo As Integer
        Dim strSearch As String = ""
        Dim dblContQty As Double
        Dim dblDOQty As Double
        Dim strInvoiceNo As String
        Dim strContractNo As String = ""

        strContractNo = Trim(txtContNo.Text)

        strSearch = " and ctr.LocCode = '" & strLocation & "' and ln.ContractNo like '%" & strContractNo & "' "
        strSearch = strSearch & " and ln.invoiceid <> ''"
        strparam = strSearch & "|" & ""


        Try
            intErrNo = objCMTrx.mtdGetContract(strOpCdGet, strparam, 0, objContractDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegList.aspx")
        End Try

        If objContractDs.Tables(0).Rows.Count > 0 Then
            txtContractQty.Enabled = False
        Else
            txtContractQty.Enabled = True
        End If

    End Sub

    Sub onChanged_TermOfDelivery(ByVal Sender As Object, ByVal E As EventArgs)
        Dim strTerm As String = Request.Form("ddlTerm")

        'Select Case strTerm
        '    Case objCMTrx.EnumTermOfDelivery.Franco, objCMTrx.EnumTermOfDelivery.CIF
        '        taConsignment.Value = "Franco Pabrik " + Trim(ddlBuyer.SelectedItem.Text)
        '    Case objCMTrx.EnumTermOfDelivery.Loco, objCMTrx.EnumTermOfDelivery.FOB
        '        taConsignment.Value = "Di PKS " + Trim(Session("SS_COMPANYNAME"))
        '    Case Else
        '        taConsignment.Value = ""
        'End Select
    End Sub

    Sub onChanged_Product(ByVal Sender As Object, ByVal E As EventArgs)
        Dim strProduct As String = Request.Form("ddlProduct")

        Select Case strProduct
            Case objWMTrx.EnumWeighBridgeTicketProduct.CPO
                taProductQuality.Value = Trim(hidQualityCPO.Value)
                taProductClaim.Value = Trim(hidClaimCPO.Value)
            Case objWMTrx.EnumWeighBridgeTicketProduct.PK
                taProductQuality.Value = Trim(hidQualityPK.Value)
                taProductClaim.Value = Trim(hidClaimPK.Value)
            Case Else
                taProductQuality.Value = ""
                taProductClaim.Value = ""
        End Select


        '--ambil dari tbl cm_contractquality
        'Dim strParamName As String
        'Dim strParamValue As String
        'Dim strOpCdGet As String = "CM_CLSSETUP_CONTRACTQUALITY_GET"
        'Dim dsData As Object
        'Dim intErrNo As Integer
        'Dim intCnt As Integer
        'Dim intSelectedIndex As Integer
        'Dim strProduct As String = Request.Form("ddlProduct")
        'Dim strContCategory As String = Request.Form("ddlContCategory")

        'If strProduct = objWMTrx.EnumWeighBridgeTicketProduct.CPO Or strProduct = objWMTrx.EnumWeighBridgeTicketProduct.PK Then
        '    strParamName = "SEARCHSTR|SORTEXP"
        '    strParamValue = ""

        '    Try
        '        intErrNo = objGLtrx.mtdGetDataCommon(strOpCdGet, _
        '                                            strParamName, _
        '                                            strParamValue, _
        '                                            dsData)
        '    Catch Exp As System.Exception
        '        Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_BUYERLIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegList.aspx")
        '    End Try

        '    If dsData.Tables(0).Rows.Count > 0 Then
        '        Select Case strContCategory
        '            Case objCMTrx.EnumContCategory.LTCBiasa
        '                taProductQuality.Value = "FFA MAKSIMUM " & Trim(dsData.Tables(0).Rows(0).Item("LTCBiasaFFA")) & " %" & "<br>" & _
        '                                         "MI  MAKSIMUM " & Trim(dsData.Tables(0).Rows(0).Item("LTCBiasaMI")) & " %"
        '            Case objCMTrx.EnumContCategory.LTCForward
        '                taProductQuality.Value = "FFA MAKSIMUM " & Trim(dsData.Tables(0).Rows(0).Item("LTCForwardFFA")) & " %" & "<br>" & _
        '                                         "MI  MAKSIMUM " & Trim(dsData.Tables(0).Rows(0).Item("LTCForwardMI")) & " %"
        '            Case objCMTrx.EnumContCategory.SpotLokal
        '                taProductQuality.Value = "FFA MAKSIMUM " & Trim(dsData.Tables(0).Rows(0).Item("SpotLokalFFA")) & " %" & "<br>" & _
        '                                         "MI  MAKSIMUM " & Trim(dsData.Tables(0).Rows(0).Item("SpotLokalMI")) & " %" & "<br>" & _
        '                                         "DOBI MINIMUM " & Trim(dsData.Tables(0).Rows(0).Item("SpotLokalDOBI"))
        '            Case objCMTrx.EnumContCategory.SpotExport
        '                taProductQuality.Value = "FFA MAKSIMUM " & Trim(dsData.Tables(0).Rows(0).Item("SpotExportFFA")) & " %" & "<br>" & _
        '                                         "MI  MAKSIMUM " & Trim(dsData.Tables(0).Rows(0).Item("SpotExportMI")) & " %" & "<br>" & _
        '                                         "DOBI MINIMUM " & Trim(dsData.Tables(0).Rows(0).Item("SpotExportDOBI"))
        '            Case Else
        '                taProductQuality.Value = ""
        '        End Select
        '    End If

        'End If
    End Sub
	
	
	Sub NewTBBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CM_Trx_ContractRegDet.aspx")
    End Sub

End Class
