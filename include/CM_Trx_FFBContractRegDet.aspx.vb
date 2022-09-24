
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

Public Class CM_Trx_FFBContractRegDet : Inherits Page
    Protected WithEvents txtContractNo As TextBox
    Protected WithEvents rdPricingMtd As RadioButtonList
    Protected WithEvents txtContractDate As TextBox
    Protected WithEvents btnContractDate As Image
    Protected WithEvents lblContractDate As Label
    Protected WithEvents lblContractDateFmt As Label
    'Protected WithEvents ddlSeller As DropDownList
    Protected WithEvents txtSupCode As TextBox
    Protected WithEvents txtSupName As TextBox

    Protected WithEvents FindSpl As HtmlInputButton
    Protected WithEvents txtContractQty As TextBox
    Protected WithEvents txtExtraQty As TextBox
    Protected WithEvents ddlExtraQtyType As DropDownList
    Protected WithEvents lblMatchedQty As Label
    Protected WithEvents lblBalQty As Label
    Protected WithEvents ddlCurrency As DropDownList
    Protected WithEvents ddlCurrBrd As DropDownList

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

    Protected WithEvents taAddNote As HtmlTextArea
    Protected WithEvents taProductQuality As HtmlTextArea
    Protected WithEvents taTermOfPayment As HtmlTextArea

    Protected WithEvents tbcode As HtmlInputHidden
    Protected WithEvents SaveBtn As ImageButton
    Protected WithEvents CloseBtn As ImageButton
    Protected WithEvents DelBtn As ImageButton
    Protected WithEvents UnDelBtn As ImageButton
    Protected WithEvents ConfirmBtn As ImageButton
    Protected WithEvents EditBtn As ImageButton

    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblHiddenSts As Label
    Protected WithEvents lblPleaseSelect As Label
    Protected WithEvents PrintBtn As System.Web.UI.WebControls.ImageButton

    Protected WithEvents trSeller As HtmlTableRow
    Protected WithEvents trBuyer As HtmlTableRow
    Protected WithEvents trPapan As HtmlTableRow
    Protected WithEvents trQty As HtmlTableRow
    Protected WithEvents trPrice As HtmlTableRow
    Protected WithEvents trAddFee As HtmlTableRow
    Protected WithEvents trAddFee2 As HtmlTableRow  
    Protected WithEvents trAddFee3 As HtmlTableRow
    Protected WithEvents trAddFee4 As HtmlTableRow
    Protected WithEvents trAddFeeS1 As HtmlTableRow
    Protected WithEvents trAddFeeS2 As HtmlTableRow
    Protected WithEvents trAddFeeS3 As HtmlTableRow

    Protected WithEvents trBonusFee As HtmlTableRow
    Protected WithEvents trBonus As HtmlTableRow
    Protected WithEvents trBonus1 As HtmlTableRow
    Protected WithEvents trBonus2 As HtmlTableRow
    Protected WithEvents trBonus3 As HtmlTableRow
    Protected WithEvents trBonus4 As HtmlTableRow
    Protected WithEvents trBonus5 As HtmlTableRow
    Protected WithEvents trDeduct As HtmlTableRow
    Protected WithEvents trDeduct1 As HtmlTableRow
    Protected WithEvents trDeduct2 As HtmlTableRow
    Protected WithEvents trDeduct3 As HtmlTableRow



    Protected WithEvents trLokal As HtmlTableRow
    Protected WithEvents trExport As HtmlTableRow
    Protected WithEvents txtContNo As TextBox
    Protected WithEvents ddlContCategory As DropDownList
    Protected WithEvents ddlTermOfWeighing As DropDownList
    Protected WithEvents lblBankCode As Label
    Protected WithEvents lblBankAccNo As Label
    Protected WithEvents lblPriceBasis As Label

    Protected WithEvents txtBuyerNo As TextBox

    Protected WithEvents ddlCurrAddFee As DropDownList
    Protected WithEvents ddlCurrAddFee2 As DropDownList
    Protected WithEvents ddlCurrAddFee3 As DropDownList
    Protected WithEvents ddlCurrAddFeeS1 As DropDownList
    Protected WithEvents ddlCurrAddFeeS2 As DropDownList
    Protected WithEvents ddlCurrAddFeeS3 As DropDownList

    Protected WithEvents txtAddFee As TextBox
    Protected WithEvents txtAddFee2 As TextBox
    Protected WithEvents txtAddFee3 As TextBox
    Protected WithEvents txtAddFeeS1 As TextBox
    Protected WithEvents txtAddFeeS2 As TextBox
    Protected WithEvents txtAddFeeS3 As TextBox
    Protected WithEvents txtAddFeeBrd As TextBox

    Protected WithEvents ddlCurrBonusFee As DropDownList
    Protected WithEvents txtBonusFee As TextBox

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

    Protected WithEvents rdInvoiceMtd As RadioButtonList
    Protected WithEvents chkSenin As CheckBox
    Protected WithEvents chkSelasa As CheckBox
    Protected WithEvents chkRabu As CheckBox
    Protected WithEvents chkKamis As CheckBox
    Protected WithEvents chkJumat As CheckBox

    Protected WithEvents chkPapan As CheckBox
    Protected WithEvents txtPeriod1 As TextBox
    Protected WithEvents txtPeriod2 As TextBox
    Protected WithEvents btnPeriod1 As Image
    Protected WithEvents btnPeriod2 As Image

    Protected WithEvents txt_min1 As TextBox
    Protected WithEvents txt_max1 As TextBox
    Protected WithEvents txt_fee1 As TextBox
    Protected WithEvents ddl_opsi1 As DropDownList
    Protected WithEvents txt_min2 As TextBox
    Protected WithEvents txt_max2 As TextBox
    Protected WithEvents txt_fee2 As TextBox
    Protected WithEvents ddl_opsi2 As DropDownList
    Protected WithEvents txt_min3 As TextBox
    Protected WithEvents txt_max3 As TextBox
    Protected WithEvents txt_fee3 As TextBox
    Protected WithEvents ddl_opsi3 As DropDownList
    Protected WithEvents txt_min4 As TextBox
    Protected WithEvents txt_max4 As TextBox
    Protected WithEvents txt_fee4 As TextBox
    Protected WithEvents ddl_opsi4 As DropDownList
    Protected WithEvents txt_min5 As TextBox
    Protected WithEvents txt_max5 As TextBox
    Protected WithEvents txt_fee5 As TextBox
    Protected WithEvents ddl_opsi5 As DropDownList

    Protected WithEvents rdDeductionMtd As RadioButtonList
    Protected WithEvents txtDeductionRate As TextBox
    Protected WithEvents txt_minDeduct1 As TextBox
    Protected WithEvents txt_maxDeduct1 As TextBox
    Protected WithEvents txt_rate1 As TextBox
    Protected WithEvents txt_minDeduct2 As TextBox
    Protected WithEvents txt_maxDeduct2 As TextBox
    Protected WithEvents txt_rate2 As TextBox
    Protected WithEvents txt_minDeduct3 As TextBox
    Protected WithEvents txt_maxDeduct3 As TextBox
    Protected WithEvents txt_rate3 As TextBox

    Protected WithEvents cbExcel As CheckBox

    Protected WithEvents chkOB As CheckBox
    Protected WithEvents chkOL As CheckBox

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
    Dim intLevel As Integer


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
        intLevel = Session("SS_USRLEVEL")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCMAccessRights.CMContractReg), intCMAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            onload_GetLangCap()
            lblContractDate.Visible = False
            lblContractDateFmt.Visible = False

            lblErrContractQty.Visible = False
            strContractNo = Trim(IIf(Request.QueryString("tbcode") <> "", Request.QueryString("tbcode"), Request.Form("tbcode")))
            intStatus = CInt(lblHiddenSts.Text)



            If Not IsPostBack Then
                'txtSupCode.Attributes.Add("Autopostback", "true")
                txtSupCode.Attributes.Add("readonly", "readonly")
                txtSupName.Attributes.Add("readonly", "readonly")



                If strContractNo <> "" Then
                    tbcode.Value = strContractNo
                    onLoad_Display(strContractNo)
                    onLoad_BindButton()
                    'CheckDisableContractQty()
                Else
                    If strContractNo = "" Then
                        txtContractDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
                        txtPeriod1.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
                        txtPeriod2.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), DateAdd(DateInterval.Year, 1, Now))
                    End If

                    BindSellerList("")
                    BindCurrencyList("")
                    BindCurrAddFeeList("")
                    BindCurrBonusFeeList("")
                    'BindExtraQtyTypeList("")
                    'BindPriceBasisList("")
                    'BindTermOfDelivery("")
                    onLoad_BindButton()
                    'onLoad_TableRow(2)

                    BindBankCode("", "", "")
                End If
            End If
        End If
    End Sub


    Sub BindSellerList(ByVal pv_strSellerCode As String)
        'Dim strOpCd_Get As String = "PU_CLSSETUP_SUPPLIER_GET"
        'Dim strSrchStatus As String
        'Dim strParam As String
        'Dim intErrNo As Integer
        'Dim intCnt As Integer
        'Dim dr As DataRow
        'Dim intSelectedIndex As Integer

        'strParam = "||" & objPUSetup.EnumSuppStatus.Active & "||SupplierCode||"

        'strParam = strParam & "|" & IIf(Session("SS_COACENTRALIZED") = "1", "", " SupplierCode LIKE '%" & Trim(strLocation) & "%'")
        'strParam = strParam & "|" & objPUSetup.EnumSupplierType.FFBSupplier

        'Try
        '    intErrNo = objPUSetup.mtdGetSupplier(strOpCd_Get, strParam, objSellerDs)
        'Catch Exp As System.Exception
        '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_SELLERLIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        'End Try

        'For intCnt = 0 To objSellerDs.Tables(0).Rows.Count - 1
        '    objSellerDs.Tables(0).Rows(intCnt).Item("SupplierCode") = Trim(objSellerDs.Tables(0).Rows(intCnt).Item("SupplierCode"))
        '    objSellerDs.Tables(0).Rows(intCnt).Item("Name") = objSellerDs.Tables(0).Rows(intCnt).Item("SupplierCode") & " (" & Trim(objSellerDs.Tables(0).Rows(intCnt).Item("Name")) & ")"
        '    If objSellerDs.Tables(0).Rows(intCnt).Item("SupplierCode") = pv_strSellerCode Then
        '        intSelectedIndex = intCnt + 1
        '    End If
        'Next

        'dr = objSellerDs.Tables(0).NewRow()
        'dr("SupplierCode") = ""
        'dr("Name") = lblSelect.Text & "Supplier Code"
        'objSellerDs.Tables(0).Rows.InsertAt(dr, 0)

        'ddlSeller.DataSource = objSellerDs.Tables(0)
        'ddlSeller.DataValueField = "SupplierCode"
        'ddlSeller.DataTextField = "Name"
        'ddlSeller.DataBind()
        'ddlSeller.SelectedIndex = intSelectedIndex

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
            ddlCurrAddFee.SelectedValue = pv_strCurrencyCode
            ddlCurrAddFee2.SelectedValue = pv_strCurrencyCode
            ddlCurrAddFee3.SelectedValue = pv_strCurrencyCode
            ddlCurrAddFeeS1.SelectedValue = pv_strCurrencyCode
            ddlCurrAddFeeS2.SelectedValue = pv_strCurrencyCode
            ddlCurrAddFeeS3.SelectedValue = pv_strCurrencyCode
            ddlCurrBrd.SelectedValue = pv_strCurrencyCode
            ddlCurrBonusFee.SelectedValue = pv_strCurrencyCode

            ddlCurrency.DataSource = objCurrencyDs.Tables(0)
            ddlCurrency.DataValueField = "CurrencyCode"
            ddlCurrency.DataTextField = "CurrencyCode"
            ddlCurrency.DataBind()

            ddlCurrAddFee2.DataSource = objCurrencyDs.Tables(0)
            ddlCurrAddFee2.DataValueField = "CurrencyCode"
            ddlCurrAddFee2.DataTextField = "CurrencyCode"
            ddlCurrAddFee2.DataBind()


            ddlCurrAddFee3.DataSource = objCurrencyDs.Tables(0)
            ddlCurrAddFee3.DataValueField = "CurrencyCode"
            ddlCurrAddFee3.DataTextField = "CurrencyCode"
            ddlCurrAddFee3.DataBind()

            ddlCurrAddFeeS1.DataSource = objCurrencyDs.Tables(0)
            ddlCurrAddFeeS1.DataValueField = "CurrencyCode"
            ddlCurrAddFeeS1.DataTextField = "CurrencyCode"
            ddlCurrAddFeeS1.DataBind()

            ddlCurrAddFeeS2.DataSource = objCurrencyDs.Tables(0)
            ddlCurrAddFeeS2.DataValueField = "CurrencyCode"
            ddlCurrAddFeeS2.DataTextField = "CurrencyCode"
            ddlCurrAddFeeS2.DataBind()

            ddlCurrAddFeeS3.DataSource = objCurrencyDs.Tables(0)
            ddlCurrAddFeeS3.DataValueField = "CurrencyCode"
            ddlCurrAddFeeS3.DataTextField = "CurrencyCode"
            ddlCurrAddFeeS3.DataBind()
			
			ddlCurrBrd.DataSource = objCurrencyDs.Tables(0)
            ddlCurrBrd.DataValueField = "CurrencyCode"
            ddlCurrBrd.DataTextField = "CurrencyCode"
            ddlCurrBrd.DataBind()


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

    Sub BindCurrAddFeeList(ByVal pv_strCurrencyCode As String)
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
            ddlCurrAddFee.SelectedValue = pv_strCurrencyCode

            ddlCurrAddFee.DataSource = objCurrencyDs.Tables(0)
            ddlCurrAddFee.DataValueField = "CurrencyCode"
            ddlCurrAddFee.DataTextField = "CurrencyCode"
            ddlCurrAddFee.DataBind()
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

            ddlCurrAddFee.DataSource = objCurrencyDs.Tables(0)
            ddlCurrAddFee.DataValueField = "CurrencyCode"
            ddlCurrAddFee.DataTextField = "CurrencyCode"
            ddlCurrAddFee.DataBind()
            ddlCurrAddFee.SelectedIndex = intSelectedIndex
        End If
    End Sub

    Sub BindCurrBonusFeeList(ByVal pv_strCurrencyCode As String)
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
            ddlCurrBonusFee.SelectedValue = pv_strCurrencyCode

            ddlCurrBonusFee.DataSource = objCurrencyDs.Tables(0)
            ddlCurrBonusFee.DataValueField = "CurrencyCode"
            ddlCurrBonusFee.DataTextField = "CurrencyCode"
            ddlCurrBonusFee.DataBind()
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

            ddlCurrBonusFee.DataSource = objCurrencyDs.Tables(0)
            ddlCurrBonusFee.DataValueField = "CurrencyCode"
            ddlCurrBonusFee.DataTextField = "CurrencyCode"
            ddlCurrBonusFee.DataBind()
            ddlCurrBonusFee.SelectedIndex = intSelectedIndex
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

    Sub BindBankCode(ByVal pv_splCode As String, ByVal pv_strCode1 As String, ByVal pv_strCode2 As String)
        Dim strOpCd As String = "PU_CLSSETUP_SUPPLIERBANK_GET"
        Dim strParameter As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer = 0
        Dim intSelectedIndex2 As Integer = 0

        strParamName = "SUPPLIERCODE"
        strParamValue = Trim(pv_splCode)

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCd, _
                                                strParamName, _
                                                strParamValue, _
                                                dsBank)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ID_GET&errmesg=" & Exp.ToString() & "&redirect=")
        End Try

        For intCnt = 0 To dsBank.Tables(0).Rows.Count - 1
            If Trim(pv_strCode1) = Trim(dsBank.Tables(0).Rows(intCnt).Item("BankAccNo")) Then
                intSelectedIndex = intCnt + 1
            End If
            If Trim(pv_strCode2) = Trim(dsBank.Tables(0).Rows(intCnt).Item("BankAccNo")) Then
                intSelectedIndex2 = intCnt + 1
            End If
        Next intCnt

        Dim dr As DataRow
        dr = dsBank.Tables(0).NewRow()
        dr("BankAccNo") = ""
        dr("SplBankDescr") = lblPleaseSelect.Text & " Bank" & lblCode.Text
        dsBank.Tables(0).Rows.InsertAt(dr, 0)

        ddlBankCode1.DataSource = dsBank.Tables(0)
        ddlBankCode1.DataValueField = "BankAccNo"
        ddlBankCode1.DataTextField = "SplBankDescr"
        ddlBankCode1.DataBind()
        ddlBankCode1.SelectedIndex = intSelectedIndex


        ddlBankCode2.DataSource = dsBank.Tables(0)
        ddlBankCode2.DataValueField = "BankAccNo"
        ddlBankCode2.DataTextField = "SplBankDescr"
        ddlBankCode2.DataBind()
        ddlBankCode2.SelectedIndex = intSelectedIndex2
    End Sub

    Function lCDbl(ByVal pcVal As String) As Double
        If IsNumeric(pcVal) Then lCDbl = CDbl(pcVal) Else lCDbl = 0
    End Function

    Sub onSelect_Bank1(ByVal Sender As System.Object, ByVal E As System.EventArgs)
        Dim intErrNo As Integer
        Dim strDate As String = Date_Validation(txtContractDate.Text, False)
        Dim strOpCodeBank As String
        Dim strBank As String
        Dim strBankAccCode As String
        Dim arrParam As Array

        'If ddlBankCode1.SelectedItem.Value = "" Then
        '    txtBankAccNo1.Text = ""
        '    Exit Sub
        'Else
        '    arrParam = Split(Trim(ddlBankCode1.SelectedItem.Value), "|")
        '    strBank = Trim(arrParam(0))
        '    strBankAccCode = Trim(arrParam(1))
        'End If

        strOpCodeBank = "PU_CLSSETUP_SUPPLIERBANK_GET_DETAIL"

        strParamName = "SUPPLIERCODE|BANKACCNO"
        strParamValue = txtSupCode.Text.Trim & "|" & ddlBankCode1.SelectedItem.Value

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

    Sub onSelect_Bank2(ByVal Sender As System.Object, ByVal E As System.EventArgs)
        Dim intErrNo As Integer
        Dim strDate As String = Date_Validation(txtContractDate.Text, False)
        Dim strOpCodeBank As String
        Dim strBank As String
        Dim strBankAccCode As String
        Dim arrParam As Array

        'If ddlBankCode2.SelectedItem.Value = "" Then
        '    txtBankAccNo1.Text = ""
        '    Exit Sub
        'Else
        '    arrParam = Split(Trim(ddlBankCode2.SelectedItem.Value), "|")
        '    strBank = Trim(arrParam(0))
        '    strBankAccCode = Trim(arrParam(1))
        'End If

        strOpCodeBank = "PU_CLSSETUP_SUPPLIERBANK_GET_DETAIL"

        strParamName = "SUPPLIERCODE|BANKACCNO"
        strParamValue = txtSupCode.Text.Trim & "|" & ddlBankCode2.SelectedItem.Value

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCodeBank, _
                                                strParamName, _
                                                strParamValue, _
                                                objAccDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ID_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        If objAccDs.Tables(0).Rows.Count > 0 Then
            txtBankAccNo2.Text = Trim(objAccDs.Tables(0).Rows(0).Item("BankAccNo"))
        End If
    End Sub

    Sub onSeller_Change(ByVal Sender As System.Object, ByVal E As System.EventArgs)

        BindBankCode(txtSupCode.Text.Trim, "", "")
    End Sub

    Function CheckDate(ByVal pv_strDate As String, ByRef pr_strDate As String) As Boolean
        Dim strDateFormatCode As String = Session("SS_DATEFMT")
        Dim strDateFormat As String

        pr_strDate = ""
        CheckDate = True
        If Not pv_strDate = "" Then
            If objGlobal.mtdValidInputDate(strDateFormatCode, pv_strDate, strDateFormat, pr_strDate) = False Then
                lblContractDateFmt.Text = strDateFormat
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
        Dim strOpCd As String = "CM_CLSTRX_CONTRACT_FFB_GET"
        Dim strOpCdCheckMatchStatus As String = "CM_CLSTRX_CONTRACT_REG_GET_MATCH_STATUS"
        Dim strParam As String
        Dim strParamCheckMatchStatus As String
        Dim intErrNo As Integer
        Dim strSearch As String
        Dim strSearchMatchStatus As String
        Dim strBalQty As String
        Dim intCnt As Integer

        strParamName = "STRSEARCH"
        strParamValue = "WHERE ContractNo='" & Trim(strContractNo) & "' "

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCd, _
                                                   strParamName, _
                                                   strParamValue, objContractDs)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_CHECK_ACTIVE_MATCH&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegList.aspx")
        End Try

        'If objMatchDs.Tables(0).Rows.count > 0 Then
        '    lblActiveMatchExist.Text = "1"
        'End If
        txtSupCode.Text = Trim(objContractDs.Tables(0).Rows(0).Item("SupplierCode"))
        txtSupName.Text = Trim(objContractDs.Tables(0).Rows(0).Item("Name"))
        txtContNo.Text = Trim(objContractDs.Tables(0).Rows(0).Item("ContractNo"))
        txtContractDate.Text = objGlobal.GetShortDate(strDateFMt, objContractDs.Tables(0).Rows(0).Item("ContractDate"))
        txtPeriod1.Text = objGlobal.GetShortDate(strDateFMt, objContractDs.Tables(0).Rows(0).Item("Period1"))
        txtPeriod2.Text = objGlobal.GetShortDate(strDateFMt, objContractDs.Tables(0).Rows(0).Item("Period2"))
        BindSellerList(Trim(objContractDs.Tables(0).Rows(0).Item("SupplierCode")))
        txtBuyerNo.Text = Trim(objContractDs.Tables(0).Rows(0).Item("SPNo"))
        taAddNote.Value = Trim(objContractDs.Tables(0).Rows(0).Item("AdditionalNote"))

        rdPricingMtd.Items(0).Selected = IIf(objContractDs.Tables(0).Rows(0).Item("PricingMtd") = 1, True, False)
        rdPricingMtd.Items(1).Selected = IIf(objContractDs.Tables(0).Rows(0).Item("PricingMtd") = 2, True, False)
        rdPricingMtd.Items(2).Selected = IIf(objContractDs.Tables(0).Rows(0).Item("PricingMtd") = 3, True, False)
        rdPricingMtd.Items(3).Selected = IIf(objContractDs.Tables(0).Rows(0).Item("PricingMtd") = 4, True, False)
        rdPricingMtd.Items(4).Selected = IIf(objContractDs.Tables(0).Rows(0).Item("PricingMtd") = 5, True, False)

        If rdPricingMtd.Items(1).Selected = True Then
            trPapan.Visible = True
            chkPapan.Visible = True
        End If

        txtContractQty.Text = objContractDs.Tables(0).Rows(0).Item("Qty")
        BindCurrencyList(Trim(objContractDs.Tables(0).Rows(0).Item("CurrencyCode")))
        txtPrice.Text = objContractDs.Tables(0).Rows(0).Item("Price")
        BindCurrAddFeeList(Trim(objContractDs.Tables(0).Rows(0).Item("CurrAddFee")))
        txtAddFee.Text = objContractDs.Tables(0).Rows(0).Item("AddFee")
        txtAddFee2.Text = objContractDs.Tables(0).Rows(0).Item("AddFee2")
        txtAddFee3.Text = objContractDs.Tables(0).Rows(0).Item("AddFee3")
        txtAddFeeS1.Text = objContractDs.Tables(0).Rows(0).Item("AddFeeGradeS1")
        txtAddFeeS2.Text = objContractDs.Tables(0).Rows(0).Item("AddFeeGradeS2")
        txtAddFeeS3.Text = objContractDs.Tables(0).Rows(0).Item("AddFeeGradeS3")
        txtAddFeeBrd.Text = objContractDs.Tables(0).Rows(0).Item("BrdFee")

        BindCurrBonusFeeList(Trim(objContractDs.Tables(0).Rows(0).Item("CurrBonusFee")))
        txtBonusFee.Text = objContractDs.Tables(0).Rows(0).Item("BonusFee")

        txtDeductionRate.Text = objContractDs.Tables(0).Rows(0).Item("DeductionRate")
        rdDeductionMtd.Items(0).Selected = IIf(objContractDs.Tables(0).Rows(0).Item("DeductionMtd") = 1, True, False)
        rdDeductionMtd.Items(1).Selected = IIf(objContractDs.Tables(0).Rows(0).Item("DeductionMtd") = 2, True, False)
        rdDeductionMtd.Items(2).Selected = IIf(objContractDs.Tables(0).Rows(0).Item("DeductionMtd") = 3, True, False)

        If rdDeductionMtd.Items(2).Selected = True Then
            trDeduct.Visible = False
            trDeduct1.Visible = True
            trDeduct2.Visible = True
            trDeduct3.Visible = True
        Else
            trDeduct.Visible = True
            trDeduct1.Visible = False
            trDeduct2.Visible = False
            trDeduct3.Visible = False
        End If

        Select Case rdPricingMtd.SelectedItem.Value
            Case 1
                chkPapan.Enabled = False
                ddlCurrency.Enabled = False
                txtPrice.Enabled = False
                trPapan.Visible = False
                trAddFee.Visible = True
                trAddFee2.Visible = True
                trAddFee3.Visible = True
                trAddFee4.Visible = True
                trAddFeeS1.Visible = True
                trAddFeeS2.Visible = True
                trAddFeeS3.Visible = True

                trBonusFee.Visible = False
                trBonus.Visible = True
                trBonus1.Visible = True
                trBonus2.Visible = True
                trBonus3.Visible = True
                trBonus4.Visible = True
                trBonus5.Visible = True

            Case 2
                chkPapan.Enabled = True
                ddlCurrency.Enabled = False
                txtPrice.Enabled = False
                trPapan.Visible = True
                trAddFee.Visible = True
                trAddFee2.Visible = False
                trAddFee3.Visible = False
                trAddFee4.Visible = False
                trAddFeeS1.Visible = False
                trAddFeeS2.Visible = False
                trAddFeeS3.Visible = False
                txtAddFee2.Text = 0
                txtAddFee3.Text = 0
                txtAddFeeS1.Text = 0
                txtAddFeeS2.Text = 0
                txtAddFeeS3.Text = 0
                txtAddFeeBrd.Text = 0
                trBonusFee.Visible = False
                trBonus.Visible = True
                trBonus1.Visible = True
                trBonus2.Visible = True
                trBonus3.Visible = True
                trBonus4.Visible = True
                trBonus5.Visible = True

            Case 3, 4, 5
                chkPapan.Enabled = False
                ddlCurrency.Enabled = False
                txtPrice.Enabled = True
                trPapan.Visible = False
                trAddFee2.Visible = False
                trAddFee3.Visible = False
                trAddFee4.Visible = False
                trAddFeeS1.Visible = False
                trAddFeeS2.Visible = False
                trAddFeeS3.Visible = False
                txtAddFeeBrd.Text = False
                trBonusFee.Visible = False
                trBonus.Visible = False
                trBonus1.Visible = False
                trBonus2.Visible = False
                trBonus3.Visible = False
                trBonus4.Visible = False
                trBonus5.Visible = False

        End Select

        ''lblMatchedQty.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(Trim(objContractDs.Tables(0).Rows(0).Item("MatchedQty")), CInt(Session("SS_ROUNDNO")))
        ''strBalQty = Trim(objContractDs.Tables(0).Rows(0).Item("ContractQty")) - Trim(objContractDs.Tables(0).Rows(0).Item("MatchedQty"))
        ''lblBalQty.Text = strBalQty 'ObjGlobal.GetIDDecimalSeparator_FreeDigit(strBalQty, 5)

        ddlTermOfWeighing.SelectedValue = Trim(objContractDs.Tables(0).Rows(0).Item("TermOfWeighing"))
        taRemarks.Value = Trim(objContractDs.Tables(0).Rows(0).Item("Remarks"))
        intStatus = CInt(Trim(objContractDs.Tables(0).Rows(0).Item("Status")))
        lblHiddenSts.Text = Trim(objContractDs.Tables(0).Rows(0).Item("Status"))
        lblStatus.Text = objCMTrx.mtdGetContractStatus(Trim(objContractDs.Tables(0).Rows(0).Item("Status")))
        lblDateCreated.Text = objGlobal.GetLongDate(objContractDs.Tables(0).Rows(0).Item("CreateDate"))
        lblLastUpdate.Text = objGlobal.GetLongDate(objContractDs.Tables(0).Rows(0).Item("UpdateDate"))
        lblUpdatedBy.Text = Trim(objContractDs.Tables(0).Rows(0).Item("UserName"))

        rdInvoiceMtd.Items(0).Selected = IIf(objContractDs.Tables(0).Rows(0).Item("InvoicingMtd") = 1, True, False)
        rdInvoiceMtd.Items(1).Selected = IIf(objContractDs.Tables(0).Rows(0).Item("InvoicingMtd") = 2, True, False)

        If rdInvoiceMtd.Items(1).Selected = True Then
            chkSenin.Enabled = True
            chkSelasa.Enabled = True
            chkRabu.Enabled = True
            chkKamis.Enabled = True
            chkJumat.Enabled = True
        End If

        chkSenin.Checked = IIf(objContractDs.Tables(0).Rows(0).Item("isMonday") = 1, True, False)
        chkSelasa.Checked = IIf(objContractDs.Tables(0).Rows(0).Item("isTuesday") = 1, True, False)
        chkRabu.Checked = IIf(objContractDs.Tables(0).Rows(0).Item("isWednesday") = 1, True, False)
        chkKamis.Checked = IIf(objContractDs.Tables(0).Rows(0).Item("isThursday") = 1, True, False)
        chkJumat.Checked = IIf(objContractDs.Tables(0).Rows(0).Item("isFriday") = 1, True, False)

        BindBankCode(txtSupCode.Text.Trim, objContractDs.Tables(0).Rows(0).Item("BankAccNoDPP"), objContractDs.Tables(0).Rows(0).Item("BankAccNoPPN"))
        'BindBankCode(ddlBankCode1.SelectedItem.Value, objContractDs.Tables(0).Rows(0).Item("BankCode"))
        ''BindBankCode(ddlBankCode2, objContractDs.Tables(0).Rows(0).Item("BankCode2"))
        txtBankAccNo1.Text = Trim(objContractDs.Tables(0).Rows(0).Item("BankAccNoDPP"))
        txtBankAccNo2.Text = Trim(objContractDs.Tables(0).Rows(0).Item("BankAccNoPPN"))

        txtUnderName.Text = Trim(objContractDs.Tables(0).Rows(0).Item("UnderName"))
        txtUnderPost.Text = Trim(objContractDs.Tables(0).Rows(0).Item("UnderPost"))

        If objContractDs.Tables(0).Rows(0).Item("OBInit") = 1 Then
            chkOB.Checked = True
        Else
            chkOB.Checked = False
        End If
        If objContractDs.Tables(0).Rows(0).Item("OLInit") = 1 Then
            chkOL.Checked = True
        Else
            chkOL.Checked = False
        End If

        strOpCd = "CM_CLSTRX_CONTRACT_FFB_BONUS_GET"

        strParamName = "CONTRACTNO"
        strParamValue = Trim(txtContNo.Text)

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCd, _
                                                strParamName, _
                                                strParamValue, _
                                                objContractDs)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=TX_Setup_TaxObjectDet&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        If objContractDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objContractDs.Tables(0).Rows.Count - 1
                If objContractDs.Tables(0).Rows(intCnt).Item("id") = 1 Then
                    txt_min1.Text = Trim(objContractDs.Tables(0).Rows(intCnt).Item("KG_min"))
                    txt_max1.Text = Trim(objContractDs.Tables(0).Rows(intCnt).Item("KG_max"))
                    txt_fee1.Text = Trim(objContractDs.Tables(0).Rows(intCnt).Item("fee"))
                    ddl_opsi1.SelectedValue = Trim(objContractDs.Tables(0).Rows(intCnt).Item("opsi"))
                End If

                If objContractDs.Tables(0).Rows(intCnt).Item("id") = 2 Then
                    txt_min2.Text = Trim(objContractDs.Tables(0).Rows(intCnt).Item("KG_min"))
                    txt_max2.Text = Trim(objContractDs.Tables(0).Rows(intCnt).Item("KG_max"))
                    txt_fee2.Text = Trim(objContractDs.Tables(0).Rows(intCnt).Item("fee"))
                    ddl_opsi2.SelectedValue = Trim(objContractDs.Tables(0).Rows(intCnt).Item("opsi"))
                End If

                If objContractDs.Tables(0).Rows(intCnt).Item("id") = 3 Then
                    txt_min3.Text = Trim(objContractDs.Tables(0).Rows(intCnt).Item("KG_min"))
                    txt_max3.Text = Trim(objContractDs.Tables(0).Rows(intCnt).Item("KG_max"))
                    txt_fee3.Text = Trim(objContractDs.Tables(0).Rows(intCnt).Item("fee"))
                    ddl_opsi3.SelectedValue = Trim(objContractDs.Tables(0).Rows(intCnt).Item("opsi"))
                End If

                If objContractDs.Tables(0).Rows(intCnt).Item("id") = 4 Then
                    txt_min4.Text = Trim(objContractDs.Tables(0).Rows(intCnt).Item("KG_min"))
                    txt_max4.Text = Trim(objContractDs.Tables(0).Rows(intCnt).Item("KG_max"))
                    txt_fee4.Text = Trim(objContractDs.Tables(0).Rows(intCnt).Item("fee"))
                    ddl_opsi4.SelectedValue = Trim(objContractDs.Tables(0).Rows(intCnt).Item("opsi"))
                End If
            Next
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

    Sub onChange_PricingMtd(ByVal Sender As Object, ByVal E As EventArgs)
        Select Case rdPricingMtd.SelectedItem.Value
            Case 1
                chkPapan.Enabled = False
                ddlCurrency.Enabled = False
                txtPrice.Enabled = False
                trPapan.Visible = False
                trAddFee.Visible = True
                trAddFee2.Visible = True
                trAddFee3.Visible = True
                trAddFee4.Visible = True
                trAddFeeS1.Visible = True
                trAddFeeS2.Visible = True
                trAddFeeS3.Visible = True
                trBonusFee.Visible = False
                trBonus.Visible = True
                trBonus1.Visible = True
                trBonus2.Visible = True
                trBonus3.Visible = True
                trBonus4.Visible = True
                trBonus5.Visible = True

            Case 2
                chkPapan.Enabled = True
                ddlCurrency.Enabled = False
                txtPrice.Enabled = False
                trPapan.Visible = True
                trAddFee.Visible = True
                trAddFee2.Visible = False
                trAddFee3.Visible = False
                trAddFee4.Visible = False
                trAddFeeS1.Visible = False
                trAddFeeS2.Visible = False
                trAddFeeS3.Visible = False
                txtAddFee2.Text = 0
                txtAddFee3.Text = 0
                txtAddFeeS1.Text = 0
                txtAddFeeS2.Text = 0
                txtAddFeeS3.Text = 0
                txtAddFeeBrd.Text = 0
                trBonusFee.Visible = False
                trBonus.Visible = True
                trBonus1.Visible = True
                trBonus2.Visible = True
                trBonus3.Visible = True
                trBonus4.Visible = True
                trBonus5.Visible = True

            Case 3, 4, 5
                chkPapan.Enabled = False
                ddlCurrency.Enabled = False
                txtPrice.Enabled = True
                trPapan.Visible = False
                'trAddFee.Visible = False
                trAddFee2.Visible = False
                trAddFee3.Visible = False
                trAddFee4.Visible = False
                trAddFeeS1.Visible = False
                trAddFeeS2.Visible = False
                trAddFeeS3.Visible = False
                txtAddFee2.Text = 0
                txtAddFee3.Text = 0
                txtAddFeeS1.Text = 0
                txtAddFeeS2.Text = 0
                txtAddFeeS3.Text = 0
                txtAddFeeBrd.Text = 0
                trBonusFee.Visible = False
                trBonus.Visible = False
                trBonus1.Visible = False
                trBonus2.Visible = False
                trBonus3.Visible = False
                trBonus4.Visible = False
                trBonus5.Visible = False

        End Select
    End Sub

    Sub onChange_InvoiceMtd(ByVal Sender As Object, ByVal E As EventArgs)
        If rdInvoiceMtd.SelectedItem.Value = 2 Then
            chkSenin.Enabled = True
            chkSelasa.Enabled = True
            chkRabu.Enabled = True
            chkKamis.Enabled = True
            chkJumat.Enabled = True
        Else
            chkSenin.Enabled = False
            chkSelasa.Enabled = False
            chkRabu.Enabled = False
            chkKamis.Enabled = False
            chkJumat.Enabled = False
        End If
    End Sub

    Sub onChange_DeductionMtd(ByVal Sender As Object, ByVal E As EventArgs)
        If rdDeductionMtd.SelectedItem.Value = 3 Then
            trDeduct.Visible = False
            trDeduct1.Visible = True
            trDeduct2.Visible = True
            trDeduct3.Visible = True
        Else
            trDeduct.Visible = True
            trDeduct1.Visible = False
            trDeduct2.Visible = False
            trDeduct3.Visible = False
        End If
    End Sub


    Sub onLoad_BindButton()
        trPapan.Visible = False
        txtContNo.Enabled = False
        txtSupCode.Enabled = False
        FindSpl.Disabled = True
        txtContractDate.Enabled = False
        btnContractDate.Visible = False
        txtPeriod1.Enabled = False
        txtPeriod2.Enabled = False
        btnPeriod1.Visible = False
        btnPeriod2.Visible = False
        chkPapan.Enabled = False
        txtBuyerNo.Enabled = False
        taAddNote.Disabled = True

        rdPricingMtd.Enabled = False
        txtContractQty.Enabled = False
        ddlCurrency.Enabled = False
        txtPrice.Enabled = False
        ddlCurrAddFee.Enabled = False
        ddlCurrAddFee2.Enabled = False
        ddlCurrAddFee3.Enabled = False
        ddlCurrAddFeeS1.Enabled = False
        ddlCurrAddFeeS2.Enabled = False
        ddlCurrAddFeeS3.Enabled = False
        ddlCurrBrd.Enabled = False
        txtAddFee.Enabled = False
        ddlCurrBonusFee.Enabled = False
        txtBonusFee.Enabled = False

        txt_min1.Enabled = False
        txt_max1.Enabled = False
        txt_fee1.Enabled = False
        ddl_opsi1.Enabled = False
        txt_min2.Enabled = False
        txt_max2.Enabled = False
        txt_fee2.Enabled = False
        ddl_opsi2.Enabled = False
        txt_min3.Enabled = False
        txt_max3.Enabled = False
        txt_fee3.Enabled = False
        ddl_opsi3.Enabled = False
        txt_min4.Enabled = False
        txt_max4.Enabled = False
        txt_fee4.Enabled = False
        ddl_opsi4.Enabled = False
        txt_min5.Enabled = False
        txt_max5.Enabled = False
        txt_fee5.Enabled = False
        ddl_opsi5.Enabled = False

        rdDeductionMtd.Enabled = False
        txtDeductionRate.Enabled = False
        txt_minDeduct1.Enabled = False
        txt_maxDeduct1.Enabled = False
        txt_rate1.Enabled = False
        txt_minDeduct2.Enabled = False
        txt_maxDeduct2.Enabled = False
        txt_rate2.Enabled = False
        txt_minDeduct3.Enabled = False
        txt_maxDeduct3.Enabled = False
        txt_rate3.Enabled = False

        trDeduct1.Visible = False
        trDeduct2.Visible = False
        trDeduct3.Visible = False


        ddlTermOfWeighing.Enabled = False
        taRemarks.Disabled = True

        rdInvoiceMtd.Enabled = False
        chkSenin.Enabled = False
        chkSelasa.Enabled = False
        chkRabu.Enabled = False
        chkKamis.Enabled = False
        chkJumat.Enabled = False

        taTermOfPayment.Disabled = True
        txtUnderName.Enabled = False
        txtUnderPost.Enabled = False

        ddlBankCode1.Enabled = False
        txtBankAccNo1.Enabled = False
        ddlBankCode2.Enabled = False
        txtBankAccNo2.Enabled = False

        SaveBtn.Visible = False
        CloseBtn.Visible = False
        DelBtn.Visible = False
        UnDelBtn.Visible = False
        PrintBtn.Visible = False
        ConfirmBtn.Visible = False
        EditBtn.Visible = False

        Select Case intStatus
            Case objCMTrx.EnumContractStatus.Active
                txtContNo.Enabled = False
                txtSupCode.Enabled = True
                FindSpl.Disabled = False
                txtContractDate.Enabled = True
                btnContractDate.Visible = True
                txtPeriod1.Enabled = True
                txtPeriod2.Enabled = True
                btnPeriod1.Visible = True
                btnPeriod2.Visible = True
                chkPapan.Enabled = True
                txtBuyerNo.Enabled = True
                taAddNote.Disabled = False

                rdPricingMtd.Enabled = True
                txtContractQty.Enabled = True
                ddlCurrency.Enabled = False
                txtPrice.Enabled = True
                ddlCurrAddFee.Enabled = False
                txtAddFee.Enabled = True
                ddlCurrBonusFee.Enabled = False
                txtBonusFee.Enabled = True

                txt_min1.Enabled = True
                txt_max1.Enabled = True
                txt_fee1.Enabled = True
                ddl_opsi1.Enabled = True
                txt_min2.Enabled = True
                txt_max2.Enabled = True
                txt_fee2.Enabled = True
                ddl_opsi2.Enabled = True
                txt_min3.Enabled = True
                txt_max3.Enabled = True
                txt_fee3.Enabled = True
                ddl_opsi3.Enabled = True
                txt_min4.Enabled = True
                txt_max4.Enabled = True
                txt_fee4.Enabled = True
                ddl_opsi4.Enabled = True
                txt_min5.Enabled = True
                txt_max5.Enabled = True
                txt_fee5.Enabled = True
                ddl_opsi5.Enabled = True

                rdDeductionMtd.Enabled = True
                txtDeductionRate.Enabled = True
                txt_minDeduct1.Enabled = True
                txt_maxDeduct1.Enabled = True
                txt_rate1.Enabled = True
                txt_minDeduct2.Enabled = True
                txt_maxDeduct2.Enabled = True
                txt_rate2.Enabled = True
                txt_minDeduct3.Enabled = True
                txt_maxDeduct3.Enabled = True
                txt_rate3.Enabled = True

                ddlTermOfWeighing.Enabled = True
                taRemarks.Disabled = False

                rdInvoiceMtd.Enabled = True
                chkSenin.Enabled = True
                chkSelasa.Enabled = True
                chkRabu.Enabled = True
                chkKamis.Enabled = True
                chkJumat.Enabled = True

                taTermOfPayment.Disabled = False
                txtUnderName.Enabled = True
                txtUnderPost.Enabled = True

                ddlBankCode1.Enabled = True
                txtBankAccNo1.Enabled = True
                ddlBankCode2.Enabled = True
                txtBankAccNo2.Enabled = True

                SaveBtn.Visible = True
                CloseBtn.Visible = False
                DelBtn.Visible = False
                UnDelBtn.Visible = False
                PrintBtn.Visible = False
                ConfirmBtn.Visible = False
                EditBtn.Visible = False
                DelBtn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                ConfirmBtn.Visible = True
                'If lblActiveMatchExist.Text = "0" And lblBalQty.Text <= "0" Then
                '    CloseBtn.Visible = True
                'End If

                If rdPricingMtd.SelectedItem.Value = 2 Then
                    trPapan.Visible = True
                End If

            Case objCMTrx.EnumContractStatus.Deleted
                UnDelBtn.Visible = True

            Case objCMTrx.EnumContractStatus.Confirmed
                CloseBtn.Visible = True
                PrintBtn.Visible = True
                If intLevel >= 2 Then
                    EditBtn.Visible = True
                End If

            Case objCMTrx.EnumContractStatus.Closed
                If intLevel >= 3 Then
                    EditBtn.Visible = True
                End If
            Case Else
                txtContNo.Enabled = False
                txtSupCode.Enabled = True
                FindSpl.Disabled = False
                txtContractDate.Enabled = True
                btnContractDate.Visible = True
                txtPeriod1.Enabled = True
                txtPeriod2.Enabled = True
                btnPeriod1.Visible = True
                btnPeriod2.Visible = True
                chkPapan.Enabled = True
                txtBuyerNo.Enabled = True
                taAddNote.Disabled = False

                rdPricingMtd.Enabled = True
                txtContractQty.Enabled = True
                ddlCurrency.Enabled = False
                txtPrice.Enabled = True
                ddlCurrAddFee.Enabled = False
                txtAddFee.Enabled = True
                ddlCurrBonusFee.Enabled = False
                txtBonusFee.Enabled = True

                txt_min1.Enabled = True
                txt_max1.Enabled = True
                txt_fee1.Enabled = True
                ddl_opsi1.Enabled = True
                txt_min2.Enabled = True
                txt_max2.Enabled = True
                txt_fee2.Enabled = True
                ddl_opsi2.Enabled = True
                txt_min3.Enabled = True
                txt_max3.Enabled = True
                txt_fee3.Enabled = True
                ddl_opsi3.Enabled = True
                txt_min4.Enabled = True
                txt_max4.Enabled = True
                txt_fee4.Enabled = True
                ddl_opsi4.Enabled = True
                txt_min5.Enabled = True
                txt_max5.Enabled = True
                txt_fee5.Enabled = True
                ddl_opsi5.Enabled = True

                rdDeductionMtd.Enabled = True
                txtDeductionRate.Enabled = True
                txt_minDeduct1.Enabled = True
                txt_maxDeduct1.Enabled = True
                txt_rate1.Enabled = True
                txt_minDeduct2.Enabled = True
                txt_maxDeduct2.Enabled = True
                txt_rate2.Enabled = True
                txt_minDeduct3.Enabled = True
                txt_maxDeduct3.Enabled = True
                txt_rate3.Enabled = True

                taRemarks.Disabled = False
                ddlTermOfWeighing.Enabled = True

                rdInvoiceMtd.Enabled = True
                chkSenin.Enabled = False
                chkSelasa.Enabled = False
                chkRabu.Enabled = False
                chkKamis.Enabled = False
                chkJumat.Enabled = False

                taTermOfPayment.Disabled = False
                txtUnderName.Enabled = True
                txtUnderPost.Enabled = True

                ddlBankCode1.Enabled = True
                txtBankAccNo1.Enabled = True
                ddlBankCode2.Enabled = True
                txtBankAccNo2.Enabled = True

                SaveBtn.Visible = True
                CloseBtn.Visible = False
                DelBtn.Visible = False
                UnDelBtn.Visible = False
                PrintBtn.Visible = False
                ConfirmBtn.Visible = False
                EditBtn.Visible = False
        End Select

    End Sub

    Sub Button_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strCmdArgs As String = CType(Sender, ImageButton).CommandArgument
        Dim strOpCd_Upd As String = "CM_CLSTRX_CONTRACT_FFB_UPD"
        Dim strOpCd_Get As String = "CM_CLSTRX_CONTRACT_FFB_GET"
        Dim strOpCd_Add As String = "CM_CLSTRX_CONTRACT_FFB_ADD_AUTO"
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

        Dim strContType As Integer
        Dim objCMID As Object
        Dim objPRDs As Object
        Dim strRunNo As Integer

        Dim strNewIDFormat As String
        Dim strNewYear As String = ""
        Dim strTranPrefix As String = "CMR"
        Dim strHistYear As String = ""
        Dim objCompDs As New Object
        Dim blnIsDetail As Boolean = True
        Dim strRunNumber As String
        Dim strTerm As String = ddlTermOfWeighing.SelectedItem.Value
        Dim strFlgContType As String = ""

        Dim strBankCode As String = ddlBankCode1.SelectedValue
        Dim strBankAccNo As String = txtBankAccNo1.Text
        Dim strBankCode2 As String = ddlBankCode2.SelectedValue
        Dim strBankAccNo2 As String = txtBankAccNo2.Text
        Dim strTermOfPayment As String = taTermOfPayment.Value

        Dim strBuyerNo As String = txtBuyerNo.Text.Trim
        Dim strCurrencyOA As String = ddlCurrAddFee.SelectedItem.Value.Trim
        Dim decAddFee As Decimal = txtAddFee.Text

        Dim strDateContract As String = Date_Validation(txtContractDate.Text, False)
        Dim strDatePeriod1 As String = Date_Validation(txtPeriod1.Text, False)
        Dim strDatePeriod2 As String = Date_Validation(txtPeriod2.Text, False)

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

        If CheckDate(txtContractDate.Text.Trim(), indDate) = False Then
            lblContractDate.Visible = True
            lblContractDateFmt.Visible = True
            lblContractDate.Text = "<br>Date Entered should be in the format"
            Exit Sub
        End If


        strSellerCode = txtSupCode.Text.Trim

        If Trim(txtContractQty.Text) = "" Then
            decContractQty = 0
        Else
            decContractQty = Trim(txtContractQty.Text)
        End If

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

        If rdPricingMtd.SelectedItem.Value = 0 Then
            lblErrMessage.Visible = True
            lblErrMessage.Text = "Please select pricing method"
            Exit Sub
        End If
        'If rdPricingMtd.SelectedItem.Value <> 3 Then
        '    If CDbl(txtBonusFee.Text) <> 0 Then
        '        If CDbl(txtContractQty.Text) = 0 Then
        '            lblErrContractQty.Visible = True
        '            lblErrContractQty.Text = "Contract qty cannot be zero/empty."
        '            Exit Sub
        '        End If
        '    End If
        'End If
       
        If rdInvoiceMtd.SelectedItem.Value = 0 Then
            lblErrMessage.Visible = True
            lblErrMessage.Text = "Please select invoicing method"
            Exit Sub
        ElseIf rdInvoiceMtd.SelectedItem.Value = 2 Then
            If chkSenin.Checked = False And chkSelasa.Checked = False And chkRabu.Checked = False And chkKamis.Checked = False And chkJumat.Checked = False Then
                lblErrMessage.Visible = True
                lblErrMessage.Text = "Please select min. one day"
                Exit Sub
            End If
        End If


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

        If Trim(strCompany) = "KSJ" Then
            strCompany = "KSJA"
        End If
        strNewIDFormat = "/FFB-" & Trim(strCompany) & "/PMKS/" & Trim(strAccMonthRom) & "/" & Trim(strAccYear)

        If strCmdArgs = "Save" Then

            blnIsUpdate = IIf(intStatus = 0, False, True)
            tbcode.Value = strContractNo
            strContractNo = txtContNo.Text

            strParamName = "IDFORMAT|LOCCODE|CONTRACTNO|CONTRACTDATE|SUPPLIERCODE|ADDITIONALNOTE|SPNO|PERIOD1|PERIOD2|" & _
                            "PRICINGMTD|QTY|TOLERANCELIMIT|CURRENCYCODE|PRICE|CURRADDFEE|ADDFEE|CURRBONUSFEE|BONUSFEE|" & _
                            "ISPRICEDISBUNSAME|DEDUCTIONMTD|DEDUCTIONRATE|TERMOFWEIGHING|REMARKS|" & _
                            "INVOICINGMTD|ISMONDAY|ISTUESDAY|ISWEDNESDAY|ISTHURSDAY|ISFRIDAY|" & _
                            "BANKDPP|BANKACCNODPP|BANKPPN|BANKACCNOPPN|TERMOFPAYMENT|UNDERNAME|UNDERPOST|" & _
                            "ACCYEAR|ACCMONTH|" & _
                            "CREATEDATE|UPDATEDATE|UPDATEID|STATUS|" & _
                            "OBINIT|OLINIT|ADDFEE2|ADDFEE3|ADDFEES1|ADDFEES2|ADDFEES3|BRDFEE"

            strParamValue = Trim(strNewIDFormat) & "|" & strLocation & "|" & strContractNo & "|" & strDateContract & "|" & strSellerCode & "|" & Trim(taAddNote.Value) & "|" & _
                            Trim(txtBuyerNo.Text) & "|" & strDatePeriod1 & "|" & strDatePeriod2 & "|" & _
                            rdPricingMtd.SelectedItem.Value & "|" & lCDbl(txtContractQty.Text) & "|" & lCDbl(txtExtraQty.Text) & "|" & _
                            Trim(ddlCurrency.SelectedItem.Value) & "|" & lCDbl(txtPrice.Text) & "|" & _
                            Trim(ddlCurrAddFee.SelectedItem.Value) & "|" & lCDbl(txtAddFee.Text) & "|" & _
                            Trim(ddlCurrBonusFee.SelectedItem.Value) & "|" & lCDbl(txtBonusFee.Text) & "|" & _
                            IIf(chkPapan.Checked = True, 1, 2) & "|" & rdDeductionMtd.SelectedItem.Value & "|" & lCDbl(txtDeductionRate.Text) & "|" & Trim(ddlTermOfWeighing.SelectedItem.Value) & "|" & Trim(taRemarks.Value) & "|" & _
                            rdInvoiceMtd.SelectedItem.Value & "|" & IIf(chkSenin.Checked = True, 1, 2) & "|" & IIf(chkSelasa.Checked = True, 1, 2) & "|" & _
                            IIf(chkRabu.Checked = True, 1, 2) & "|" & IIf(chkKamis.Checked = True, 1, 2) & "|" & IIf(chkJumat.Checked = True, 1, 2) & "|" & _
                            Trim(ddlBankCode1.SelectedItem.Value) & "|" & Trim(txtBankAccNo1.Text) & "|" & _
                            Trim(ddlBankCode2.SelectedItem.Value) & "|" & Trim(txtBankAccNo2.Text) & "|" & _
                            Trim(taTermOfPayment.Value) & "|" & Trim(txtUnderName.Text) & "|" & Trim(txtUnderPost.Text) & "|" & _
                            strAccYear & "|" & strAccMonth & "|" & _
                            Now() & "|" & Now() & "|" & Trim(strUserId) & "|" & objCMTrx.EnumContractStatus.Active & "|" & _
                            IIf(chkOB.Checked = True, 1, 2) & "|" & IIf(chkOL.Checked = True, 1, 2) & "|" & lCDbl(txtAddFee2.Text) & _
                            "|" & lCDbl(txtAddFee3.Text) & "|" & lCDbl(txtAddFeeS1.Text) & "|" & lCDbl(txtAddFeeS2.Text) & "|" & lCDbl(txtAddFeeS3.Text) & "|" & lCDbl(txtAddFeeBrd.Text)

            Try
                intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_Add, _
                                                       strParamName, _
                                                       strParamValue, objCMID)

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_SAVE&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegDet.aspx?tbcode=" & strContractNo)
            End Try

            If blnDupKey = True Then
                Exit Sub
            End If

            strContractNo = objCMID.Tables(0).Rows(0).Item("ContractNo")

            If txt_min1.Text.Trim = "" Then
                txt_min1.Text = "0"
            End If
            If txt_max1.Text.Trim = "" Then
                txt_max1.Text = "0"
            End If
            If txt_fee1.Text.Trim = "" Then
                txt_fee1.Text = "0"
            End If

            If txt_min2.Text.Trim = "" Then
                txt_min2.Text = "0"
            End If
            If txt_max2.Text.Trim = "" Then
                txt_max2.Text = "0"
            End If
            If txt_fee2.Text.Trim = "" Then
                txt_fee2.Text = "0"
            End If

            If txt_min3.Text.Trim = "" Then
                txt_min3.Text = "0"
            End If
            If txt_max3.Text.Trim = "" Then
                txt_max3.Text = "0"
            End If
            If txt_fee3.Text.Trim = "" Then
                txt_fee3.Text = "0"
            End If

            If txt_min4.Text.Trim = "" Then
                txt_min4.Text = "0"
            End If
            If txt_max4.Text.Trim = "" Then
                txt_max4.Text = "0"
            End If
            If txt_fee4.Text.Trim = "" Then
                txt_fee4.Text = "0"
            End If

            If txt_min5.Text.Trim = "" Then
                txt_min5.Text = "0"
            End If
            If txt_max5.Text.Trim = "" Then
                txt_max5.Text = "0"
            End If
            If txt_fee5.Text.Trim = "" Then
                txt_fee5.Text = "0"
            End If

            Dim strOpCdLn_Upd As String = "CM_CLSTRX_CONTRACT_FFB_BONUS_UPDATE"

            'Level 1
            strParamName = "ID|IMIN|IMAX|FEE|OPSI|CONTRACTNO"
            strParamValue = "1|" & _
                         txt_min1.Text & "|" & _
                         txt_max1.Text & "|" & _
                         txt_fee1.Text & "|" & _
                         ddl_opsi1.SelectedItem.Value & "|" & _
                         Trim(strContractNo)

            Try
                intErrNo = objGLtrx.mtdInsertDataCommon(strOpCdLn_Upd, strParamName, strParamValue)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PR_PR_STP_PPH21LN_UPD&errmesg=" & lblErrMessage.Text & "&redirect=")
            End Try

            'Level 2
            strParamName = "ID|IMIN|IMAX|FEE|OPSI|CONTRACTNO"
            strParamValue = "2|" & _
                         txt_min2.Text & "|" & _
                         txt_max2.Text & "|" & _
                         txt_fee2.Text & "|" & _
                         ddl_opsi2.SelectedItem.Value & "|" & _
                         Trim(strContractNo)

            Try
                intErrNo = objGLtrx.mtdInsertDataCommon(strOpCdLn_Upd, strParamName, strParamValue)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PR_PR_STP_PPH21LN_UPD&errmesg=" & lblErrMessage.Text & "&redirect=")
            End Try

            'Level 3
            strParamName = "ID|IMIN|IMAX|FEE|OPSI|CONTRACTNO"
            strParamValue = "3|" & _
                         txt_min3.Text & "|" & _
                         txt_max3.Text & "|" & _
                         txt_fee3.Text & "|" & _
                         ddl_opsi3.SelectedItem.Value & "|" & _
                         Trim(strContractNo)

            Try
                intErrNo = objGLtrx.mtdInsertDataCommon(strOpCdLn_Upd, strParamName, strParamValue)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PR_PR_STP_PPH21LN_UPD&errmesg=" & lblErrMessage.Text & "&redirect=")
            End Try

            'Level 4
            strParamName = "ID|IMIN|IMAX|FEE|OPSI|CONTRACTNO"
            strParamValue = "4|" & _
                         txt_min4.Text & "|" & _
                         txt_max4.Text & "|" & _
                         txt_fee4.Text & "|" & _
                         ddl_opsi4.SelectedItem.Value & "|" & _
                         Trim(strContractNo)

            Try
                intErrNo = objGLtrx.mtdInsertDataCommon(strOpCdLn_Upd, strParamName, strParamValue)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PR_PR_STP_PPH21LN_UPD&errmesg=" & lblErrMessage.Text & "&redirect=")
            End Try

            'Level 5
            strParamName = "ID|IMIN|IMAX|FEE|OPSI|CONTRACTNO"
            strParamValue = "5|" & _
                         txt_min5.Text & "|" & _
                         txt_max5.Text & "|" & _
                         txt_fee5.Text & "|" & _
                         ddl_opsi5.SelectedItem.Value & "|" & _
                         Trim(strContractNo)

            Try
                intErrNo = objGLtrx.mtdInsertDataCommon(strOpCdLn_Upd, strParamName, strParamValue)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PR_PR_STP_PPH21LN_UPD&errmesg=" & lblErrMessage.Text & "&redirect=")
            End Try


            If txt_minDeduct1.Text.Trim = "" Then
                txt_minDeduct1.Text = "0"
            End If
            If txt_maxDeduct1.Text.Trim = "" Then
                txt_maxDeduct1.Text = "0"
            End If
            If txt_rate1.Text.Trim = "" Then
                txt_rate1.Text = "0"
            End If

            If txt_minDeduct2.Text.Trim = "" Then
                txt_minDeduct2.Text = "0"
            End If
            If txt_maxDeduct2.Text.Trim = "" Then
                txt_maxDeduct2.Text = "0"
            End If
            If txt_rate2.Text.Trim = "" Then
                txt_rate2.Text = "0"
            End If

            If txt_minDeduct3.Text.Trim = "" Then
                txt_minDeduct3.Text = "0"
            End If
            If txt_maxDeduct3.Text.Trim = "" Then
                txt_maxDeduct3.Text = "0"
            End If
            If txt_rate3.Text.Trim = "" Then
                txt_rate3.Text = "0"
            End If

            Dim strOpCdLn_UpdDeduction As String = "CM_CLSTRX_CONTRACT_FFB_DEDUCTION_UPDATE"

            'Level 1
            strParamName = "ID|IMIN|IMAX|RATE|CONTRACTNO"
            strParamValue = "1|" & _
                         txt_minDeduct1.Text & "|" & _
                         txt_maxDeduct1.Text & "|" & _
                         txt_rate1.Text & "|" & _
                         Trim(strContractNo)

            Try
                intErrNo = objGLtrx.mtdInsertDataCommon(strOpCdLn_UpdDeduction, strParamName, strParamValue)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PR_PR_STP_PPH21LN_UPD&errmesg=" & lblErrMessage.Text & "&redirect=")
            End Try

            'Level 2
            strParamName = "ID|IMIN|IMAX|RATE|CONTRACTNO"
            strParamValue = "2|" & _
                         txt_minDeduct2.Text & "|" & _
                         txt_maxDeduct2.Text & "|" & _
                         txt_rate2.Text & "|" & _
                         Trim(strContractNo)

            Try
                intErrNo = objGLtrx.mtdInsertDataCommon(strOpCdLn_UpdDeduction, strParamName, strParamValue)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PR_PR_STP_PPH21LN_UPD&errmesg=" & lblErrMessage.Text & "&redirect=")
            End Try

            'Level 3
            strParamName = "ID|IMIN|IMAX|RATE|CONTRACTNO"
            strParamValue = "3|" & _
                         txt_minDeduct3.Text & "|" & _
                         txt_maxDeduct3.Text & "|" & _
                         txt_rate3.Text & "|" & _
                         Trim(strContractNo)

            Try
                intErrNo = objGLtrx.mtdInsertDataCommon(strOpCdLn_UpdDeduction, strParamName, strParamValue)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PR_PR_STP_PPH21LN_UPD&errmesg=" & lblErrMessage.Text & "&redirect=")
            End Try


            onLoad_Display(strContractNo)

        ElseIf strCmdArgs = "Close" Then
            strContractNo = txtContNo.Text

            strParamName = "IDFORMAT|LOCCODE|CONTRACTNO|CONTRACTDATE|SUPPLIERCODE|ADDITIONALNOTE|SPNO|PERIOD1|PERIOD2|" & _
                            "PRICINGMTD|QTY|TOLERANCELIMIT|CURRENCYCODE|PRICE|CURRADDFEE|ADDFEE|CURRBONUSFEE|BONUSFEE|" & _
                            "ISPRICEDISBUNSAME|DEDUCTIONMTD|DEDUCTIONRATE|TERMOFWEIGHING|REMARKS|" & _
                            "INVOICINGMTD|ISMONDAY|ISTUESDAY|ISWEDNESDAY|ISTHURSDAY|ISFRIDAY|" & _
                            "BANKDPP|BANKACCNODPP|BANKPPN|BANKACCNOPPN|TERMOFPAYMENT|UNDERNAME|UNDERPOST|" & _
                            "ACCYEAR|ACCMONTH|" & _
                            "CREATEDATE|UPDATEDATE|UPDATEID|STATUS|" & _
                            "OBINIT|OLINIT|ADDFEE2|ADDFEE3|ADDFEES1|ADDFEES2|ADDFEES3|SBRDFEE"

            strParamValue = Trim(strNewIDFormat) & "|" & strLocation & "|" & strContractNo & "|" & strDateContract & "|" & strSellerCode & "|" & Trim(taAddNote.Value) & "|" & _
                            Trim(txtBuyerNo.Text) & "|" & strDatePeriod1 & "|" & strDatePeriod2 & "|" & _
                            rdPricingMtd.SelectedItem.Value & "|" & lCDbl(txtContractQty.Text) & "|" & lCDbl(txtExtraQty.Text) & "|" & _
                            Trim(ddlCurrency.SelectedItem.Value) & "|" & lCDbl(txtPrice.Text) & "|" & _
                            Trim(ddlCurrAddFee.SelectedItem.Value) & "|" & lCDbl(txtAddFee.Text) & "|" & _
                            Trim(ddlCurrBonusFee.SelectedItem.Value) & "|" & lCDbl(txtBonusFee.Text) & "|" & _
                            IIf(chkPapan.Checked = True, 1, 2) & "|" & rdDeductionMtd.SelectedItem.Value & "|" & lCDbl(txtDeductionRate.Text) & "|" & Trim(ddlTermOfWeighing.SelectedItem.Value) & "|" & Trim(taRemarks.Value) & "|" & _
                            rdInvoiceMtd.SelectedItem.Value & "|" & IIf(chkSenin.Checked = True, 1, 2) & "|" & IIf(chkSelasa.Checked = True, 1, 2) & "|" & _
                            IIf(chkRabu.Checked = True, 1, 2) & "|" & IIf(chkKamis.Checked = True, 1, 2) & "|" & IIf(chkJumat.Checked = True, 1, 2) & "|" & _
                            Trim(ddlBankCode1.SelectedItem.Value) & "|" & Trim(txtBankAccNo1.Text) & "|" & _
                            Trim(ddlBankCode2.SelectedItem.Value) & "|" & Trim(txtBankAccNo2.Text) & "|" & _
                            Trim(taTermOfPayment.Value) & "|" & Trim(txtUnderName.Text) & "|" & Trim(txtUnderPost.Text) & "|" & _
                            strAccYear & "|" & strAccMonth & "|" & _
                            Now() & "|" & Now() & "|" & Trim(strUserId) & "|" & objCMTrx.EnumContractStatus.Closed & "|" & _
                            IIf(chkOB.Checked = True, 1, 2) & "|" & IIf(chkOL.Checked = True, 1, 2) & "|" & lCDbl(txtAddFee2.Text) & "|" & lCDbl(txtAddFee3.Text) & "|" & lCDbl(txtAddFeeS1.Text) & "|" & lCDbl(txtAddFeeS2.Text) & "|"  & lCDbl(txtAddFeeS3.Text) & "|" & lCDbl(txtAddFeeBrd.Text)

            Try
                intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_Add, _
                                                       strParamName, _
                                                       strParamValue, objCMID)

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_CLOSE&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegDet.aspx?tbcode=" & strContractNo)
            End Try

            strContractNo = objCMID.Tables(0).Rows(0).Item("ContractNo")
            onLoad_Display(strContractNo)

        ElseIf strCmdArgs = "Confirm" Then
            strContractNo = txtContNo.Text

            strParamName = "IDFORMAT|LOCCODE|CONTRACTNO|CONTRACTDATE|SUPPLIERCODE|ADDITIONALNOTE|SPNO|PERIOD1|PERIOD2|" & _
                            "PRICINGMTD|QTY|TOLERANCELIMIT|CURRENCYCODE|PRICE|CURRADDFEE|ADDFEE|CURRBONUSFEE|BONUSFEE|" & _
                            "ISPRICEDISBUNSAME|DEDUCTIONMTD|DEDUCTIONRATE|TERMOFWEIGHING|REMARKS|" & _
                            "INVOICINGMTD|ISMONDAY|ISTUESDAY|ISWEDNESDAY|ISTHURSDAY|ISFRIDAY|" & _
                            "BANKDPP|BANKACCNODPP|BANKPPN|BANKACCNOPPN|TERMOFPAYMENT|UNDERNAME|UNDERPOST|" & _
                            "ACCYEAR|ACCMONTH|" & _
                            "CREATEDATE|UPDATEDATE|UPDATEID|STATUS|" & _
                            "OBINIT|OLINIT|ADDFEE2|ADDFEE3|ADDFEES1|ADDFEES2|ADDFEES3|BRDFEE"

            strParamValue = Trim(strNewIDFormat) & "|" & strLocation & "|" & strContractNo & "|" & strDateContract & "|" & strSellerCode & "|" & Trim(taAddNote.Value) & "|" & _
                            Trim(txtBuyerNo.Text) & "|" & strDatePeriod1 & "|" & strDatePeriod2 & "|" & _
                            rdPricingMtd.SelectedItem.Value & "|" & lCDbl(txtContractQty.Text) & "|" & lCDbl(txtExtraQty.Text) & "|" & _
                            Trim(ddlCurrency.SelectedItem.Value) & "|" & lCDbl(txtPrice.Text) & "|" & _
                            Trim(ddlCurrAddFee.SelectedItem.Value) & "|" & lCDbl(txtAddFee.Text) & "|" & _
                            Trim(ddlCurrBonusFee.SelectedItem.Value) & "|" & lCDbl(txtBonusFee.Text) & "|" & _
                            IIf(chkPapan.Checked = True, 1, 2) & "|" & rdDeductionMtd.SelectedItem.Value & "|" & lCDbl(txtDeductionRate.Text) & "|" & Trim(ddlTermOfWeighing.SelectedItem.Value) & "|" & Trim(taRemarks.Value) & "|" & _
                            rdInvoiceMtd.SelectedItem.Value & "|" & IIf(chkSenin.Checked = True, 1, 2) & "|" & IIf(chkSelasa.Checked = True, 1, 2) & "|" & _
                            IIf(chkRabu.Checked = True, 1, 2) & "|" & IIf(chkKamis.Checked = True, 1, 2) & "|" & IIf(chkJumat.Checked = True, 1, 2) & "|" & _
                            Trim(ddlBankCode1.SelectedItem.Value) & "|" & Trim(txtBankAccNo1.Text) & "|" & _
                            Trim(ddlBankCode2.SelectedItem.Value) & "|" & Trim(txtBankAccNo2.Text) & "|" & _
                            Trim(taTermOfPayment.Value) & "|" & Trim(txtUnderName.Text) & "|" & Trim(txtUnderPost.Text) & "|" & _
                            strAccYear & "|" & strAccMonth & "|" & _
                            Now() & "|" & Now() & "|" & Trim(strUserId) & "|" & objCMTrx.EnumContractStatus.Confirmed & "|" & _
                            IIf(chkOB.Checked = True, 1, 2) & "|" & IIf(chkOL.Checked = True, 1, 2) & "|" & lCDbl(txtAddFee2.Text) & "|" & lCDbl(txtAddFee3.Text) & "|" & lCDbl(txtAddFeeS1.Text) & "|" & lCDbl(txtAddFeeS2.Text) & "|" & lCDbl(txtAddFeeS3.Text) & "|" & lCDbl(txtAddFeeBrd.Text)

            Try
                intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_Add, _
                                                       strParamName, _
                                                       strParamValue, objCMID)

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_CLOSE&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegDet.aspx?tbcode=" & strContractNo)
            End Try

            strContractNo = objCMID.Tables(0).Rows(0).Item("ContractNo")
            onLoad_Display(strContractNo)

        ElseIf strCmdArgs = "Edit" Then
            strContractNo = txtContNo.Text

            strParamName = "IDFORMAT|LOCCODE|CONTRACTNO|CONTRACTDATE|SUPPLIERCODE|ADDITIONALNOTE|SPNO|PERIOD1|PERIOD2|" & _
                            "PRICINGMTD|QTY|TOLERANCELIMIT|CURRENCYCODE|PRICE|CURRADDFEE|ADDFEE|CURRBONUSFEE|BONUSFEE|" & _
                            "ISPRICEDISBUNSAME|DEDUCTIONMTD|DEDUCTIONRATE|TERMOFWEIGHING|REMARKS|" & _
                            "INVOICINGMTD|ISMONDAY|ISTUESDAY|ISWEDNESDAY|ISTHURSDAY|ISFRIDAY|" & _
                            "BANKDPP|BANKACCNODPP|BANKPPN|BANKACCNOPPN|TERMOFPAYMENT|UNDERNAME|UNDERPOST|" & _
                            "ACCYEAR|ACCMONTH|" & _
                            "CREATEDATE|UPDATEDATE|UPDATEID|STATUS|" & _
                            "OBINIT|OLINIT|ADDFEE2|ADDFEE3|ADDFEES1|ADDFEES2|ADDFEES3|BRDFEE"

            strParamValue = Trim(strNewIDFormat) & "|" & strLocation & "|" & strContractNo & "|" & strDateContract & "|" & strSellerCode & "|" & Trim(taAddNote.Value) & "|" & _
                            Trim(txtBuyerNo.Text) & "|" & strDatePeriod1 & "|" & strDatePeriod2 & "|" & _
                            rdPricingMtd.SelectedItem.Value & "|" & lCDbl(txtContractQty.Text) & "|" & lCDbl(txtExtraQty.Text) & "|" & _
                            Trim(ddlCurrency.SelectedItem.Value) & "|" & lCDbl(txtPrice.Text) & "|" & _
                            Trim(ddlCurrAddFee.SelectedItem.Value) & "|" & lCDbl(txtAddFee.Text) & "|" & _
                            Trim(ddlCurrBonusFee.SelectedItem.Value) & "|" & lCDbl(txtBonusFee.Text) & "|" & _
                            IIf(chkPapan.Checked = True, 1, 2) & "|" & rdDeductionMtd.SelectedItem.Value & "|" & lCDbl(txtDeductionRate.Text) & "|" & Trim(ddlTermOfWeighing.SelectedItem.Value) & "|" & Trim(taRemarks.Value) & "|" & _
                            rdInvoiceMtd.SelectedItem.Value & "|" & IIf(chkSenin.Checked = True, 1, 2) & "|" & IIf(chkSelasa.Checked = True, 1, 2) & "|" & _
                            IIf(chkRabu.Checked = True, 1, 2) & "|" & IIf(chkKamis.Checked = True, 1, 2) & "|" & IIf(chkJumat.Checked = True, 1, 2) & "|" & _
                            Trim(ddlBankCode1.SelectedItem.Value) & "|" & Trim(txtBankAccNo1.Text) & "|" & _
                            Trim(ddlBankCode2.SelectedItem.Value) & "|" & Trim(txtBankAccNo2.Text) & "|" & _
                            Trim(taTermOfPayment.Value) & "|" & Trim(txtUnderName.Text) & "|" & Trim(txtUnderPost.Text) & "|" & _
                            strAccYear & "|" & strAccMonth & "|" & _
                            Now() & "|" & Now() & "|" & Trim(strUserId) & "|" & objCMTrx.EnumContractStatus.Active & "|" & _
                            IIf(chkOB.Checked = True, 1, 2) & "|" & IIf(chkOL.Checked = True, 1, 2) & "|" & lCDbl(txtAddFee2.Text) & "|" & lCDbl(txtAddFee3.Text) & "|" & lCDbl(txtAddFeeS1.Text) & "|" & lCDbl(txtAddFeeS2.Text) & "|" & lCDbl(txtAddFeeS2.Text) & "|"    & lCDbl(txtAddFeeBrd.Text)

            Try
                intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_Add, _
                                                       strParamName, _
                                                       strParamValue, objCMID)

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_CLOSE&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegDet.aspx?tbcode=" & strContractNo)
            End Try

            strContractNo = objCMID.Tables(0).Rows(0).Item("ContractNo")
            onLoad_Display(strContractNo)

        ElseIf strCmdArgs = "Del" Then
            strContractNo = txtContNo.Text

            strParamName = "IDFORMAT|LOCCODE|CONTRACTNO|CONTRACTDATE|SUPPLIERCODE|ADDITIONALNOTE|SPNO|PERIOD1|PERIOD2|" & _
                            "PRICINGMTD|QTY|TOLERANCELIMIT|CURRENCYCODE|PRICE|CURRADDFEE|ADDFEE|CURRBONUSFEE|BONUSFEE|" & _
                            "ISPRICEDISBUNSAME|DEDUCTIONMTD|DEDUCTIONRATE|TERMOFWEIGHING|REMARKS|" & _
                            "INVOICINGMTD|ISMONDAY|ISTUESDAY|ISWEDNESDAY|ISTHURSDAY|ISFRIDAY|" & _
                            "BANKDPP|BANKACCNODPP|BANKPPN|BANKACCNOPPN|TERMOFPAYMENT|UNDERNAME|UNDERPOST|" & _
                            "ACCYEAR|ACCMONTH|" & _
                            "CREATEDATE|UPDATEDATE|UPDATEID|STATUS|" & _
                            "OBINIT|OLINIT|ADDFEE2|ADDFEE3|ADDFEES1|ADDFEES2|ADDFEES3|BRDFEE"

            strParamValue = Trim(strNewIDFormat) & "|" & strLocation & "|" & strContractNo & "|" & strDateContract & "|" & strSellerCode & "|" & Trim(taAddNote.Value) & "|" & _
                            Trim(txtBuyerNo.Text) & "|" & strDatePeriod1 & "|" & strDatePeriod2 & "|" & _
                            rdPricingMtd.SelectedItem.Value & "|" & lCDbl(txtContractQty.Text) & "|" & lCDbl(txtExtraQty.Text) & "|" & _
                            Trim(ddlCurrency.SelectedItem.Value) & "|" & lCDbl(txtPrice.Text) & "|" & _
                            Trim(ddlCurrAddFee.SelectedItem.Value) & "|" & lCDbl(txtAddFee.Text) & "|" & _
                            Trim(ddlCurrBonusFee.SelectedItem.Value) & "|" & lCDbl(txtBonusFee.Text) & "|" & _
                            IIf(chkPapan.Checked = True, 1, 2) & "|" & rdDeductionMtd.SelectedItem.Value & "|" & lCDbl(txtDeductionRate.Text) & "|" & Trim(ddlTermOfWeighing.SelectedItem.Value) & "|" & Trim(taRemarks.Value) & "|" & _
                            rdInvoiceMtd.SelectedItem.Value & "|" & IIf(chkSenin.Checked = True, 1, 2) & "|" & IIf(chkSelasa.Checked = True, 1, 2) & "|" & _
                            IIf(chkRabu.Checked = True, 1, 2) & "|" & IIf(chkKamis.Checked = True, 1, 2) & "|" & IIf(chkJumat.Checked = True, 1, 2) & "|" & _
                            Trim(ddlBankCode1.SelectedItem.Value) & "|" & Trim(txtBankAccNo1.Text) & "|" & _
                            Trim(ddlBankCode2.SelectedItem.Value) & "|" & Trim(txtBankAccNo2.Text) & "|" & _
                            Trim(taTermOfPayment.Value) & "|" & Trim(txtUnderName.Text) & "|" & Trim(txtUnderPost.Text) & "|" & _
                            strAccYear & "|" & strAccMonth & "|" & _
                            Now() & "|" & Now() & "|" & Trim(strUserId) & "|" & objCMTrx.EnumContractStatus.Deleted & "|" & _
                            IIf(chkOB.Checked = True, 1, 2) & "|" & IIf(chkOL.Checked = True, 1, 2) & "|" & lCDbl(txtAddFee2.Text) & "|" & lCDbl(txtAddFee3.Text) & "|" & lCDbl(txtAddFeeS1.Text) & "|" & lCDbl(txtAddFeeS2.Text) & "|" & lCDbl(txtAddFeeS3.Text) & "|" & lCDbl(txtAddFeeBrd.Text)

            Try
                intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_Add, _
                                                       strParamName, _
                                                       strParamValue, objCMID)

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_DEL&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegDet.aspx?tbcode=" & strContractNo)
            End Try

            strContractNo = objCMID.Tables(0).Rows(0).Item("ContractNo")
            onLoad_Display(strContractNo)

        ElseIf strCmdArgs = "UnDel" Then
            strContractNo = txtContNo.Text

            strParamName = "IDFORMAT|LOCCODE|CONTRACTNO|CONTRACTDATE|SUPPLIERCODE|ADDITIONALNOTE|SPNO|PERIOD1|PERIOD2|" & _
                            "PRICINGMTD|QTY|TOLERANCELIMIT|CURRENCYCODE|PRICE|CURRADDFEE|ADDFEE|CURRBONUSFEE|BONUSFEE|" & _
                            "ISPRICEDISBUNSAME|DEDUCTIONMTD|DEDUCTIONRATE|TERMOFWEIGHING|REMARKS|" & _
                            "INVOICINGMTD|ISMONDAY|ISTUESDAY|ISWEDNESDAY|ISTHURSDAY|ISFRIDAY|" & _
                            "BANKDPP|BANKACCNODPP|BANKPPN|BANKACCNOPPN|TERMOFPAYMENT|UNDERNAME|UNDERPOST|" & _
                            "ACCYEAR|ACCMONTH|" & _
                            "CREATEDATE|UPDATEDATE|UPDATEID|STATUS|" & _
                            "OBINIT|OLINIT|ADDFEE2|ADDFEE3|ADDFEES1|ADDFEES2|ADDFEES3|BRDFEE"

            strParamValue = Trim(strNewIDFormat) & "|" & strLocation & "|" & strContractNo & "|" & strDateContract & "|" & strSellerCode & "|" & Trim(taAddNote.Value) & "|" & _
                            Trim(txtBuyerNo.Text) & "|" & strDatePeriod1 & "|" & strDatePeriod2 & "|" & _
                            rdPricingMtd.SelectedItem.Value & "|" & lCDbl(txtContractQty.Text) & "|" & lCDbl(txtExtraQty.Text) & "|" & _
                            Trim(ddlCurrency.SelectedItem.Value) & "|" & lCDbl(txtPrice.Text) & "|" & _
                            Trim(ddlCurrAddFee.SelectedItem.Value) & "|" & lCDbl(txtAddFee.Text) & "|" & _
                            Trim(ddlCurrBonusFee.SelectedItem.Value) & "|" & lCDbl(txtBonusFee.Text) & "|" & _
                            IIf(chkPapan.Checked = True, 1, 2) & "|" & rdDeductionMtd.SelectedItem.Value & "|" & lCDbl(txtDeductionRate.Text) & "|" & Trim(ddlTermOfWeighing.SelectedItem.Value) & "|" & Trim(taRemarks.Value) & "|" & _
                            rdInvoiceMtd.SelectedItem.Value & "|" & IIf(chkSenin.Checked = True, 1, 2) & "|" & IIf(chkSelasa.Checked = True, 1, 2) & "|" & _
                            IIf(chkRabu.Checked = True, 1, 2) & "|" & IIf(chkKamis.Checked = True, 1, 2) & "|" & IIf(chkJumat.Checked = True, 1, 2) & "|" & _
                            Trim(ddlBankCode1.SelectedItem.Value) & "|" & Trim(txtBankAccNo1.Text) & "|" & _
                            Trim(ddlBankCode2.SelectedItem.Value) & "|" & Trim(txtBankAccNo2.Text) & "|" & _
                            Trim(taTermOfPayment.Value) & "|" & Trim(txtUnderName.Text) & "|" & Trim(txtUnderPost.Text) & "|" & _
                            strAccYear & "|" & strAccMonth & "|" & _
                            Now() & "|" & Now() & "|" & Trim(strUserId) & "|" & objCMTrx.EnumContractStatus.Active & "|" & _
                            IIf(chkOB.Checked = True, 1, 2) & "|" & IIf(chkOL.Checked = True, 1, 2) & "|" & lCDbl(txtAddFee2.Text) & "|" & lCDbl(txtAddFee3.Text) & "|" & lCDbl(txtAddFeeS1.Text) & "|" & lCDbl(txtAddFeeS2.Text) & "|" & lCDbl(txtAddFeeS3.Text) & "|" & lCDbl(txtAddFeeBrd.Text)

            Try
                intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_Add, _
                                                       strParamName, _
                                                       strParamValue, objCMID)

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_UNDEL&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegDet.aspx?tbcode=" & strContractNo)
            End Try

            strContractNo = objCMID.Tables(0).Rows(0).Item("ContractNo")
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
        Dim isBuyer As String = Trim(txtSupCode.Text)
        Dim strOwner1 As String = ""
        Dim strOwner2 As String = ""
        Dim strBillParty As String = Trim(txtSupCode.Text)
        Dim arrBuyer As Array
        Dim strBuyer As String

        arrBuyer = Split(isBuyer, "(")
        If UBound(arrBuyer) > 0 Then
            strBuyer = Trim(arrBuyer(1))
        End If

        strBuyer = Replace(strBuyer, ")", "")
         
        Response.Write("<Script Language=""JavaScript"">window.open(""../reports/CM_Rpt_ContractFFBRegDet.aspx?ContractNo=" & Server.UrlEncode(strContractNo) & _
                        "&Product=" & "FFB" & _
                        "&BillParty=" & Server.UrlEncode(strBillParty) & _
                        "&Buyer=" & Server.UrlEncode(strBuyer) & _
                        "&Owner1=" & Server.UrlEncode(strOwner1) & _
                        "&Owner2=" & Server.UrlEncode(strOwner2) & _
                        "&strExportToExcel=" & IIf(cbExcel.Checked, "1", "0") & _
                        """, null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")
    End Sub

    Sub BackBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CM_Trx_FFBContractRegList.aspx")
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
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
                If strLocType = objAdminLoc.EnumLocType.Mill Then
                    Return Trim(objLangCapDs.Tables(0).Rows(count).Item("BusinessTermMW"))
                Else
                    Return Trim(objLangCapDs.Tables(0).Rows(count).Item("BusinessTerm"))
                End If
                Exit For
            End If
        Next
    End Function

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

    Sub NewTBBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CM_Trx_FFBContractRegDet.aspx")
    End Sub
 
End Class
