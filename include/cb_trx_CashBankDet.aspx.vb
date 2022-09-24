
Imports System
Imports System.Data
Imports System.Math
Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Strings



Public Class cb_trx_CashBankDet : Inherits Page

    'Protected WithEvents lblErrMessage As Label
    'Protected WithEvents issueType As Label
    'Protected WithEvents lblStsHid As Label
    'Protected WithEvents blnShortCut As Label
    'Protected WithEvents lblCode As Label
    'Protected WithEvents lblSelect As Label
    'Protected WithEvents lblPleaseSelect As Label
    'Protected WithEvents blnUpdate As Label
    'Protected WithEvents lblTxLnID As Label
    'Protected WithEvents lblPaymentID As Label
    'Protected WithEvents lblAccPeriod As Label
    'Protected WithEvents rblCashBankType As RadioButtonList
    'Protected WithEvents lblStatus As Label
    'Protected WithEvents ddlPayType As DropDownList

    'Protected WithEvents lblPaymentIDTag As Label
    'Protected WithEvents lblPayTypeTag As Label
    'Protected WithEvents lblPayToTag As Label

    'Protected WithEvents LblIsSKBActive As Label
    'Protected WithEvents lblSKBStartDate As Label

    'Protected WithEvents lblLastUpdate As Label
    'Protected WithEvents ddlBank As DropDownList
    'Protected WithEvents lblErrBank As Label
    'Protected WithEvents lblChequePrintDate As Label
    'Protected WithEvents txtChequeNo As TextBox
    'Protected WithEvents lblErrCheque As Label
    'Protected WithEvents lblPrintDate As Label
    'Protected WithEvents txtPaymentTo As TextBox
    'Protected WithEvents txtSupCode As TextBox

    'Protected WithEvents lblDateCreated As Label
    'Protected WithEvents txtDateCreated As TextBox
    'Protected WithEvents btnDateCreated As Image
    'Protected WithEvents lblDate As Label
    'Protected WithEvents lblFmt As Label

    'Protected WithEvents txtPPNInit As TextBox
    'Protected WithEvents txtPPN As TextBox
    'Protected WithEvents txtCreditTerm As TextBox

    'Protected WithEvents lblUpdatedBy As Label
    'Protected WithEvents txtDescLn As TextBox
    'Protected WithEvents lblDescErr As Label
    'Protected WithEvents ddlLocation As DropDownList
    'Protected WithEvents lblLocationErr As Label
    'Protected WithEvents lblAccCodeTag As Label
    'Protected WithEvents lstAccCode As DropDownList
    'Protected WithEvents lblAccCodeErr As Label
    'Protected WithEvents ddlChargeLevel As DropDownList
    'Protected WithEvents lblPreBlkTag As Label
    'Protected WithEvents ddlPreBlock As DropDownList
    'Protected WithEvents lblPreBlockErr As Label
    'Protected WithEvents lblBlkTag As Label
    'Protected WithEvents lstBlock As DropDownList
    'Protected WithEvents lblBlockErr As Label
    'Protected WithEvents lblVehTag As Label
    'Protected WithEvents lstVehCode As DropDownList
    'Protected WithEvents lblVehCodeErr As Label
    'Protected WithEvents lblVehExpTag As Label
    'Protected WithEvents lstVehExp As DropDownList
    'Protected WithEvents lblVehExpCodeErr As Label
    'Protected WithEvents txtDRTotalAmount As TextBox
    'Protected WithEvents RegularExpressionValidatorAmtDR As RegularExpressionValidator
    'Protected WithEvents RangetxtAmtDR As RangeValidator
    'Protected WithEvents lblTwoAmount As Label
    'Protected WithEvents txtCRTotalAmount As TextBox
    'Protected WithEvents RegularExpressionValidatorAmtCR As RegularExpressionValidator
    'Protected WithEvents RangetxtAmtCR As RangeValidator
    'Protected WithEvents lblerror As Label
    'Protected WithEvents lblStock As Label
    'Protected WithEvents lbleither As Label
    'Protected WithEvents AddDtlBtn As ImageButton
    'Protected WithEvents SaveDtlBtn As ImageButton
    'Protected WithEvents lblConfirmErr As Label
    'Protected WithEvents dgTx As DataGrid
    'Protected WithEvents lblTotAmtFig As Label
    'Protected WithEvents lblCtrlAmtFig As Label
    'Protected WithEvents lblLocCodeErr As Label
    'Protected WithEvents NewCBBtn As ImageButton
    'Protected WithEvents SaveBtn As ImageButton
    'Protected WithEvents ForwardBtn As ImageButton
    'Protected WithEvents ConfirmBtn As ImageButton
    'Protected WithEvents PrintBtn As ImageButton
    'Protected WithEvents PrintChequeBtn As ImageButton
    'Protected WithEvents DeleteBtn As ImageButton
    'Protected WithEvents UnDeleteBtn As ImageButton
    'Protected WithEvents BackBtn As ImageButton
    'Protected WithEvents tblSelection As HtmlTable
    'Protected WithEvents Find As HtmlInputButton
    'Protected WithEvents payid As HtmlInputHidden
    'Protected WithEvents hidBlockCharge As HtmlInputHidden
    'Protected WithEvents hidChargeLocCode As HtmlInputHidden  
    'Protected WithEvents lblPayType As Label      
 
    'Protected WithEvents RowChargeLevel As HtmlTableRow
    'Protected WithEvents RowPreBlk As HtmlTableRow
    'Protected WithEvents RowBlk As HtmlTableRow
    'Protected WithEvents RowChargeTo As HtmlTableRow
    'Protected WithEvents txtSplBankAccNo As TextBox
    'Protected WithEvents CancelBtn As ImageButton
    'Protected WithEvents PrintSlipBtn As ImageButton
    'Protected WithEvents txtRemark As TextBox
    'Protected WithEvents PrintKwitansiBtn As ImageButton
    'Protected WithEvents PrintTransferBtn As ImageButton
    'Protected WithEvents ddlBankTo As DropDownList
    'Protected WithEvents lblerrBankTo As Label
    'Protected WithEvents lblErrBankAccNo As Label
    'Protected WithEvents btnGet As ImageButton
    'Protected WithEvents lblBankFrom As Label
    'Protected WithEvents lblBankTo As Label


    'Protected WithEvents txtGiroDate As TextBox
    'Protected WithEvents lblDateGiro As Label
    'Protected WithEvents lblFmtGiro As Label
    'Protected WithEvents ddlRefNo As DropDownList
    'Protected WithEvents btnGetRef As ImageButton
    'Protected WithEvents lblStatusInput As Label
    'Protected WithEvents lblCurrentPeriod As Label
    'Protected WithEvents lblErrPayType As Label
    'Protected WithEvents EditBtn As ImageButton
    'Protected WithEvents btnGiroDate As Image
    'Protected WithEvents FindSpl As HtmlInputButton
    'Protected WithEvents VerifiedBtn As ImageButton

    'Protected WithEvents TrLink As HtmlTableRow
    'Protected WithEvents lbViewJournal As LinkButton
    'Protected WithEvents dgViewJournal As DataGrid
    'Protected WithEvents lblTotalDB As Label
    'Protected WithEvents lblTotalCR As Label
    'Protected WithEvents lblTotalViewJournal As Label

    'Protected WithEvents hidUserID As HtmlInputHidden

    'Protected WithEvents RowTax As HtmlTableRow
    'Protected WithEvents RowTaxAmt As HtmlTableRow
    'Protected WithEvents lstTaxObject As DropDownList
    'Protected WithEvents lblTaxObjectErr As Label
    'Protected WithEvents txtDPPAmountDR As TextBox
    'Protected WithEvents txtDPPAmountCR As TextBox
    'Protected WithEvents hidNPWPNo As HtmlInputHidden
    'Protected WithEvents hidTaxObjectRate As HtmlInputHidden
    'Protected WithEvents lblTwoAmountDPP As Label
    'Protected WithEvents hidCOATax As HtmlInputHidden
    'Protected WithEvents hidTaxStatus As HtmlInputHidden
    'Protected WithEvents hidHadCOATax As HtmlInputHidden
    'Protected WithEvents lblTaxStatus As Label
    'Protected WithEvents lblTaxStatusDesc As Label
    'Protected WithEvents lblTaxUpdate As Label
    'Protected WithEvents lblTaxUpdateDesc As Label

    'Protected WithEvents hidPrevID As HtmlInputHidden
    'Protected WithEvents hidPrevDate As HtmlInputHidden
    'Protected WithEvents hidNextID As HtmlInputHidden
    'Protected WithEvents hidNextDate As HtmlInputHidden

    'Protected WithEvents hidFFBSpl As HtmlInputHidden
    'Protected WithEvents ddlSplBankAccNo As DropDownList

    'Protected WithEvents txtAccCode As TextBox
    'Protected WithEvents txtAccName As TextBox

    'Protected WithEvents ddlGiroNo As DropDownList
    'Protected WithEvents lblErrGiroNo As Label

    'Protected WithEvents chkChequeCash As CheckBox
    'Protected WithEvents ddlChequeHandOver As DropDownList

    'Protected WithEvents RowFP As HtmlTableRow
    'Protected WithEvents RowFPDate As HtmlTableRow
    'Protected WithEvents txtFakturPjkNo As TextBox
    'Protected WithEvents hidTaxPPN As HtmlInputHidden
    'Protected WithEvents txtFakturDate As TextBox
    'Protected WithEvents txtGiroName As TextBox

    Dim objGLtrx As New agri.GL.ClsTrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objHRSetup As New agri.HR.clsSetup() 
    Dim objGLSetup As New agri.GL.clsSetup()
    Dim objCBTrx As New agri.CB.clsTrx()

    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objPRRpt As New agri.PR.clsReport()
    Dim objAdmShare As New agri.Admin.clsShare()
    Dim objPUSetup As New agri.PU.clsSetup()

    Dim strOpCdTxDet_ADD As String = "CB_CLSTRX_CASHBANK_DETAIL_ADD"
    Dim strOpCdTxDet_UPD As String = "CB_CLSTRX_CASHBANK_DETAIL_UPD"
    Dim strOpCdTxLine_GET As String = "CB_CLSTRX_CASHBANK_LINE_GET"
    Dim strOpCdTxLine_UPD As String = "CB_CLSTRX_CASHBANK_LINE_UPD"
    Dim strOpCdTxLine_DEL As String = "CB_CLSTRX_CASHBANK_LINE_DEL"
    Dim strOpCdTxLine_ADD As String = "CB_CLSTRX_CASHBANK_LINE_ADD"
    Dim strOpCdAccCode_GET As String = "GL_CLSSETUP_ACCOUNTCODE_LIST_GET"

    Dim objDataSet As New DataSet()
    Dim dsGrid As New DataSet()
    Dim objLangCapDs As New Object()
    Dim objAccDs As New Object()
    Dim objPODs As New Object()
    Dim objBankDs As New Object()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim intCBAR As Integer
    Dim strDateFMT As String
    Dim intConfigsetting As Integer
    Dim PreBlockTag As String
    Dim BlockTag As String
    Dim strAccountTag As String
    Dim strVehTag As String
    Dim strBlockTag As String
    Dim strVehExpTag As String
    Dim intErrNo As Integer
    Dim strParam As String = ""
    Dim intLevel As Integer
    Dim strLocType As String
    Dim strSelAccMonth As String
    Dim strSelAccYear As String
    Dim TaxLnID As String = ""
    Dim TaxRate As Double = 0
    Dim DPPAmount As Double = 0
    Dim strParamName As String
    Dim strParamValue As String


    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_CBACCMONTH")
        strAccYear = Session("SS_CBACCYEAR")
        strDateFMT = Session("SS_DATEFMT")
        intCBAR = Session("SS_CBAR")
        intConfigsetting = Session("SS_CONFIGSETTING")
        strLocType = Session("SS_LOCTYPE")
        intLevel = Session("SS_USRLEVEL")
        strSelAccMonth = Session("SS_SELACCMONTH")
        strSelAccYear = Session("SS_SELACCYEAR")

        txtDPPAmountCR.Attributes.Add("onfocus", "gotFocusDPPCR()")
        txtDPPAmountDR.Attributes.Add("onfocus", "gotFocusDPPDR()")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCBAccessRights.CBCashBank), intCBAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            onload_GetLangCap()

            lblTwoAmount.Visible = False
            lblerror.Visible = False
            lblLocationErr.Visible = False
            lblAccCodeErr.Visible = False
            lblPreBlockErr.Visible = False
            lblBlockErr.Visible = False
            lblVehCodeErr.Visible = False
            lblVehExpCodeErr.Visible = False
            lblDescErr.Visible = False
            lblLocCodeErr.Visible = False
            lblConfirmErr.Visible = False
            lblDate.Visible = False
            lblErrCheque.Visible = False
            lblErrBank.Visible = False
            lblerrBankTo.Visible = False
            lblErrBankAccNo.Visible = False
            lblDateGiro.Visible = False
            lblFmtGiro.Visible = False
            lblErrPayType.Visible = False
            lblTaxObjectErr.Visible = False
            lblTwoAmountDPP.Visible = False

            'to avoid double click, on aspx add this : UseSubmitBehavior="false"
            AddDtlBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(AddDtlBtn).ToString())
            NewCBBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(NewCBBtn).ToString())
            SaveBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(SaveBtn).ToString())
            VerifiedBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(VerifiedBtn).ToString())
            ConfirmBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(ConfirmBtn).ToString())
            ForwardBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(ForwardBtn).ToString())
            DeleteBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(DeleteBtn).ToString())
            UnDeleteBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(UnDeleteBtn).ToString())
            EditBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(EditBtn).ToString())
            CancelBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(CancelBtn).ToString())
            BackBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(BackBtn).ToString())
            PrintBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(PrintBtn).ToString())
            PrintChequeBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(PrintChequeBtn).ToString())
            PrintSlipBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(PrintSlipBtn).ToString())
            PrintTransferBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(PrintTransferBtn).ToString())
            PrintKwitansiBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(PrintKwitansiBtn).ToString())

            txtPPNInit.Attributes.Add("readonly", "readonly")
            txtPPN.Attributes.Add("readonly", "readonly")
            txtCreditTerm.Attributes.Add("readonly", "readonly")
            txtSupCode.Attributes.Add("readonly", "readonly")

            If Not Page.IsPostBack Then
                LblIsSKBActive.Text = 0
                lblSKBStartDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
                txtDateCreated.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
                txtGiroDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
                NewCBBtn.Visible = False
                ForwardBtn.Visible = False
                ConfirmBtn.Visible = False
                DeleteBtn.Visible = False
                UnDeleteBtn.Visible = False
                PrintBtn.Visible = False
                PrintChequeBtn.Visible = False
                PrintSlipBtn.Visible = False
                PrintKwitansiBtn.Visible = False
                TrLink.Visible = False

                BindLocationDropDownList(Trim(Session("SS_LOCATION")))
                RowChargeTo.Visible = Session("SS_INTER_ESTATE_CHARGING")

                BindChargeLevelDropDownList()
                BindPaymentMethod()
                BindBankCode("")
                BindBankTo("")
                BindPreBlock("", "")
                BindBlockDropList("")
                BindVehicleCodeDropList("")
                BindVehicleExpDropList(True)
                'BindAccCodeDropList()
                'BindSupplier("")
                lblPaymentID.Text = Request.QueryString("cbid")
                BindTaxObjectList("", "")
                BindSplBankAccNo("", "")
                BindGiroNo("", "")
                If Not Request.QueryString("cbid") = "" Then
                    LoadTxDetails()
                    If objDataSet.Tables(0).Rows.Count > 0 Then
                        DisplayFromDB()
                    End If
                End If
                BindRefNo("")
                onLoad_Button()
            End If
        End If
    End Sub

    Sub BindPayType(ByVal pv_strPayType As String)
        'If pv_strPayType = "1" Then
        '    ddlPayType.SelectedIndex = 1
        '    ddlBank.SelectedIndex = 0
        '    txtChequeNo.Text = ""
        'ElseIf pv_strPayType = "0" Then
        '    ddlPayType.SelectedIndex = 0
        'ElseIf pv_strPayType = "2" Then
        '    ddlPayType.SelectedIndex = 2
        '    ddlBank.SelectedIndex = 0
        '    txtChequeNo.Text = ""
        'ElseIf pv_strPayType = "3" Then
        '    ddlPayType.SelectedIndex = 3
        '    ddlBank.SelectedIndex = 0
        '    txtChequeNo.Text = ""
        'End If
        'onLoad_Button()
    End Sub

    Sub ToggleChargeLevel()
        If ddlChargeLevel.selectedIndex = 0 Then
            RowBlk.Visible = False
            RowPreBlk.Visible = True
            hidBlockCharge.value = "yes"
        Else
            RowBlk.Visible = True
            RowPreBlk.Visible = False
            hidBlockCharge.value = ""
        End If
    End Sub

    Sub CashBankType_Change(ByVal Sender As Object, ByVal E As EventArgs)
        Select Case Sender.SelectedItem.Value
            Case objCBTrx.EnumCashBankType.Payment, agri.CB.clsTrx.EnumCashBankType.Transfer
                lblPaymentIDTag.Text = "Payment ID :"
                lblPayTypeTag.Text = "Payment Type :*"
                lblPayToTag.Text = "Payment To :*"
                lblBankFrom.Text = "Bank From :"
                lblBankTo.Text = "Bank To :"
                BindRefNo("")
            Case objCBTrx.EnumCashBankType.Receipt
                lblPaymentIDTag.Text = "Receipt ID :"
                lblPayTypeTag.Text = "Receipt Type :*"
                lblPayToTag.Text = "Receipt From :*"
                lblBankFrom.Text = "Bank To :"
                lblBankTo.Text = "Bank From :"
                BindRefNoTrx("")
        End Select
    End Sub

    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim btn As LinkButton
        Dim lbl As Label
        Dim Delbutton As LinkButton
        Dim strDesc As String
        Dim strLocCode As String
        Dim strAccCode As String
        Dim strBlkCode As String
        Dim strVehCode As String
        Dim strVehExpCode As String
        Dim intAccType As Integer
        Dim intAccPurpose As Integer
        Dim intNurseryInd As Integer
        Dim blnFound As Boolean
        Dim strRefNoTrx As String
        Dim strDate As String = Date_Validation(txtDateCreated.Text, False)
        Dim indDate As String = ""

        If CheckDate(txtDateCreated.Text.Trim(), indDate) = False Then
            lblDate.Visible = True
            lblFmt.Visible = True
            lblDate.Text = "<br>Date Entered should be in the format"
            Exit Sub
        End If

        Dim intInputPeriod As Integer = Year(strDate) * 100 + Month(strDate)
        Dim intCurPeriod As Integer = (CInt(strAccYear) * 100) + CInt(strAccMonth)
        Dim intSelPeriod As Integer = (CInt(strSelAccYear) * 100) + CInt(strSelAccMonth)

        If Session("SS_FILTERPERIOD") = "0" Then
            If intCurPeriod < intInputPeriod Then
                lblDate.Visible = True
                lblDate.Text = "Invalid transaction date."
                Exit Sub
            End If
        Else
            If intSelPeriod <> intInputPeriod Then
                lblDate.Visible = True
                lblDate.Text = "Invalid transaction date."
                Exit Sub
            End If
            If intSelPeriod < intCurPeriod And intLevel < 2 Then
                lblDate.Visible = True
                lblDate.Text = "This period already locked."
                Exit Sub
            End If
        End If

        blnUpdate.Text = True
        dgTx.EditItemIndex = CInt(E.Item.ItemIndex)

        lbl = E.Item.FindControl("lblID")
        lblTxLnID.Text = lbl.Text.Trim

        lbl = E.Item.FindControl("lblDesc")
        txtDescLn.Text = lbl.Text.Trim
        lbl = E.Item.FindControl("lblAccTx")

        If lbl.Text.Trim = "DR" Then
            lbl = E.Item.FindControl("lblAmt")
            txtCRTotalAmount.Text = ""
            'txtDRTotalAmount.Text = FormatNumber(Abs(lCDbl(lbl.Text.Trim)), 2, True, True, True)
            txtDRTotalAmount.Text = Replace(Replace(ABS(lcdbl(lbl.Text.Trim)), ".", ""), ",", ".")
        ElseIf lbl.Text = "CR" Then
            lbl = E.Item.FindControl("lblAmt")
            txtDRTotalAmount.Text = ""
            'txtCRTotalAmount.Text = FormatNumber(Abs(lCDbl(lbl.Text.Trim)), 2, True, True, True)
            txtCRTotalAmount.Text = Replace(Replace(ABS(LCDBL(lbl.Text.Trim)), ".", ""), ",", ".")
        End If
        lbl = E.Item.FindControl("lblLocCode")
        strLocCode = lbl.Text.Trim
        BindLocationDropDownList(strLocCode)
        lbl = E.Item.FindControl("lblAccCode")
        strAccCode = lbl.Text.Trim
        lbl = E.Item.FindControl("lblBlkCode")
        strBlkCode = lbl.Text.Trim
        lbl = E.Item.FindControl("lblVehCode")
        strVehCode = lbl.Text.Trim
        lbl = E.Item.FindControl("lblVehExpCode")
        strVehExpCode = lbl.Text.Trim

        lbl = E.Item.FindControl("lblGiroNo")
        If lbl.Text.Trim <> "" Then
            BindGiroNo(ddlBank.SelectedItem.Value, lbl.Text.Trim)
        End If
        lbl = E.Item.FindControl("lblGiroName")
        txtGiroName.Text = lbl.Text.Trim

        lbl = E.Item.FindControl("lblRefNo")
        If rblCashBankType.SelectedValue = objCBTrx.EnumCashBankType.Payment Then
            BindRefNo(lbl.Text.Trim)
        Else
            strRefNoTrx = lbl.Text.Trim
            lbl = E.Item.FindControl("lblStaffAdvDoc")
            strRefNoTrx = Trim(strRefNoTrx) & "|" & lbl.Text.Trim
            BindRefNoTrx(strRefNoTrx)
        End If

        'If rblCashBankType.SelectedIndex = 0 Or rblCashBankType.SelectedIndex = 2 Then

        'mark 20apr16
        'lstAccCode.SelectedValue = strAccCode
        GetCOADetail(strAccCode)

        GetAccountDetails(strAccCode, intAccType, intAccPurpose, intNurseryInd, blnFound)

        If intAccType = objGLSetup.EnumAccountType.ProfitAndLost Then
            Select Case intAccPurpose
                Case objGLSetup.EnumAccountPurpose.NonVehicle
                    BindBlockDropList(strAccCode, strBlkCode)
                    BindVehicleCodeDropList("")
                    BindVehicleExpDropList(True)
                Case objGLSetup.EnumAccountPurpose.VehicleDistribution
                    BindBlockDropList("")
                    BindVehicleCodeDropList(strAccCode, strVehCode)
                    BindVehicleExpDropList(False, strVehExpCode)
                Case Else
                    BindBlockDropList(strAccCode, strBlkCode)
                    BindVehicleCodeDropList("%", strVehCode)
                    BindVehicleExpDropList(False, strVehExpCode)
            End Select
        ElseIf intAccType = objGLSetup.EnumAccountType.BalanceSheet Or intNurseryInd = objGLSetup.EnumNurseryAccount.Yes Then
            'BindBlockDropList(strAccCode, strBlkCode)
            BindBlockBalanceSheetDropList(strAccCode, strBlkCode)
            BindVehicleCodeDropList("")
            BindVehicleExpDropList(True)
        Else
            BindBlockDropList("", "")
            BindVehicleCodeDropList("")
            BindVehicleExpDropList(True)
        End If

        CheckCOATax()
        If hidCOATax.Value = 1 Then
            hidCOATax.Value = 1
            lbl = E.Item.FindControl("lblTaxLnID")
            BindTaxObjectList(strAccCode, lbl.Text.Trim)
            lbl = E.Item.FindControl("lblDPPAmount")
            If lCDbl(lbl.Text.Trim) < 0 Then
                txtDPPAmountDR.Text = ""
                'txtDPPAmountCR.Text = FormatNumber(Abs(lCDbl(lbl.Text.Trim)), 2, True, False, False)
                txtDPPAmountCR.Text = Replace(Replace(abs(lcdbl(lbl.Text.Trim)), ".", ""), ",", ".")
            ElseIf lCDbl(lbl.Text.Trim) > 0 Then
                txtDPPAmountCR.Text = ""
                'txtDPPAmountDR.Text = FormatNumber(Abs(lCDbl(lbl.Text.Trim)), 2, True, False, False)
                txtDPPAmountDR.Text = Replace(Replace(abs(lcdbl(lbl.Text.Trim)), ".", ""), ",", ".")
            End If

            lbl = E.Item.FindControl("lblTaxRate")
            hidTaxObjectRate.Value = lCDbl(lbl.Text.Trim)
            lstTaxObject_OnSelectedIndexChanged(Sender, E)

            lbl = E.Item.FindControl("lblSPLFaktur")
            txtFakturPjkNo.Text = Trim(lbl.Text)
            lbl = E.Item.FindControl("lblSPLFakturDate")
            txtFakturDate.Text = objGlobal.GetShortDate(strDateFMT, Trim(lbl.Text))

            If txtDPPAmountDR.Text <> "" Or txtDPPAmountCR.Text <> "" Then
                txtCRTotalAmount.ReadOnly = True
                txtDRTotalAmount.ReadOnly = True
            Else
                txtCRTotalAmount.ReadOnly = False
                txtDRTotalAmount.ReadOnly = False
            End If
        Else
            hidCOATax.Value = 0
            RowTax.Visible = False
            RowTaxAmt.Visible = False
            txtCRTotalAmount.ReadOnly = False
            txtDRTotalAmount.ReadOnly = False
        End If

        If lblTxLnID.Text <> "" Then
            AddDtlBtn.Visible = False
            SaveDtlBtn.Visible = True
        Else
            AddDtlBtn.Visible = True
            SaveDtlBtn.Visible = False
        End If
        RowChargeLevel.Visible = False
        ddlChargeLevel.SelectedIndex = 1
        ToggleChargeLevel()
    End Sub

    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        
        Initialize()
        lblTxLnID.Text = ""
        dgTx.EditItemIndex = -1
        BindGrid()
        RowTax.Visible = False
        RowTaxAmt.Visible = False
        RowFP.Visible = False
        RowFPDate.Visible = False

        AddDtlBtn.Visible = True
        SaveDtlBtn.Visible = False
        BindLocationDropDownList(Trim(Session("SS_LOCATION")))
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strDate As String = Date_Validation(txtDateCreated.Text, False)
        Dim indDate As String = ""

        If CheckDate(txtDateCreated.Text.Trim(), indDate) = False Then
            lblDate.Visible = True
            lblFmt.Visible = True
            lblDate.Text = "<br>Date Entered should be in the format"
            Exit Sub
        End If

        Dim intInputPeriod As Integer = Year(strDate) * 100 + Month(strDate)
        Dim intCurPeriod As Integer = (CInt(strAccYear) * 100) + CInt(strAccMonth)
        Dim intSelPeriod As Integer = (CInt(strSelAccYear) * 100) + CInt(strSelAccMonth)

        If Session("SS_FILTERPERIOD") = "0" Then
            If intCurPeriod < intInputPeriod Then
                lblDate.Visible = True
                lblDate.Text = "Invalid transaction date."
                Exit Sub
            End If
        Else
            If intSelPeriod <> intInputPeriod Then
                lblDate.Visible = True
                lblDate.Text = "Invalid transaction date."
                Exit Sub
            End If
            If intSelPeriod < intCurPeriod And intLevel < 2 Then
                lblDate.Visible = True
                lblDate.Text = "This period already locked."
                Exit Sub
            End If
        End If

        If lblStsHid.Text = CStr(objCBTrx.EnumCashBankStatus.Active) _
             Or CStr(objCBTrx.EnumCashBankStatus.Verified) Then

            Dim lbl As Label
            Dim id As String
            Dim ItemCode As String
            Dim strParam As String
            Dim StrTxParam As String
            Dim TxID As String
            Dim ErrorChk As Integer = 0

            lbl = E.Item.FindControl("lblID")
            id = lbl.Text

            strParam = id & "|" & Trim(lblPaymentID.Text)
            Try
                intErrNo = objCBTrx.mtdDelCashBankLn(strOpCdTxLine_DEL, strParam)
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=DEL_CASHBANKLINE&errmesg=" & lblErrMessage.Text & "&redirect=CB/Trx/CB_trx_CashBankList.aspx")
                End If

            End Try

            If rblCashBankType.SelectedValue = objCBTrx.EnumCashBankType.Receipt Then
                lbl = E.Item.FindControl("lblStaffAdvDoc")
                If lbl.Text.Trim <> "" Then
                    BindRefNoTrx("")
                End If
            End If


            UpdateCBLine()
            BindGrid()
            onLoad_Button()
        End If

    End Sub

    Sub AddDtlBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim strOpCodeGLSubBlkByBlk As String = "GL_CLSSETUP_SUBBLOCK_BY_BLOCK_GET"
        Dim strParamList As String
        Dim TxResultID As String
        Dim strCRTotalAmount As String = IIf(Trim(Request.Form("txtCRTotalAmount")) = "", 0, Trim(Request.Form("txtCRTotalAmount")))
        Dim strDRTotalAmount As String = IIf(Trim(Request.Form("txtDRTotalAmount")) = "", 0, Trim(Request.Form("txtDRTotalAmount")))
        Dim strCRTotalAmountDPP As String = IIf(Trim(Request.Form("txtDPPAmountCR")) = "", 0, Trim(Request.Form("txtDPPAmountCR")))
        Dim strDRTotalAmountDPP As String = IIf(Trim(Request.Form("txtDPPAmountDR")) = "", 0, Trim(Request.Form("txtDPPAmountDR")))

        Dim strTxParam As String
        Dim strTxLnParam As String
        Dim ErrorChk As Integer = 0
        Dim strAllowVehicle As String
        Dim strDocPrefix As String
        Dim strDocPrefixLn As String
        Dim strOpCodeID As String
        Dim strOpCd As String
        Dim dblQty As Double = 1
        Dim dblPrice As Double = 0
        Dim dblTotal As Double = 0
        Dim strGiroDate As String = Date_Validation(txtGiroDate.Text, False)
        Dim strNewIDFormat As String
        Dim strInOrOut As String
        Dim strRemark As String = Trim(Replace(txtRemark.Text, "'", "''"))
        Dim strInitial As String = ""
        Dim intStatus As Integer
        Dim strFakturNo As String = Trim(Request.Form("txtFakturPjkNo"))
        Dim strFPDate As String = Date_Validation(Request.Form("txtFakturDate"), False)

        Dim strDate As String = Date_Validation(txtDateCreated.Text, False)
        Dim indDate As String = ""
        
        'utk validasi apakah coa terdaftar
        Dim intCnt As Integer = 0
        Dim intErrNoCoaValidate As Integer
        Dim intSelectedIndex As Integer = 0

        Dim strOpCode As String = "GL_CLSSETUP_ACCOUNTCODE_LIST_GET"
        Dim objCOADs As New DataSet
        Dim strParamNameCoaValidate As String = ""
        Dim strParamValueCoaValidate As String = ""
        
        Dim strBank As String
        Dim strBankAccCode As String
        Dim strBankAccNo As String
        Dim arrParam As Array

        If ddlBank.SelectedItem.Value = "" Then
            strBank = ""
            strBankAccCode = ""
            strBankAccNo = ""
        Else
            arrParam = Split(Trim(ddlBank.SelectedItem.Value), "|")
            strBank = Trim(arrParam(0))
            strBankAccCode = Trim(arrParam(1))
            strBankAccNo = Trim(arrParam(2))

            If ddlPayType.SelectedItem.Value <> 2 Then
                If ddlPayType.SelectedItem.Value = 0 Or ddlPayType.SelectedItem.Value = 3 Then
                    If strBankAccNo = "" Then
                        lblErrPayType.Text = "This payment type must use Bank account"
                        lblErrPayType.Visible = True
                        Exit Sub
                    End If
                Else
                    If strBankAccNo <> "" Then
                        lblErrPayType.Text = "This payment type cannot use Bank account"
                        lblErrPayType.Visible = True
                        Exit Sub
                    End If
                    Select Case strBank
                        Case "KKL", "KKK"
                            If ddlPayType.SelectedItem.Value = 4 Then
                                lblErrPayType.Text = "This payment type cannot use Kas account"
                                lblErrPayType.Visible = True
                                Exit Sub
                            End If
                            If lblPaymentID.Text <> "" Then
                                If Left(lblPaymentID.Text, 3) <> "XXX" Then
                                    If Left(lblPaymentID.Text, 2) <> "KK" Then
                                        lblErrPayType.Text = "<br>ID transaction already set to Kas Besar."
                                        lblErrPayType.Visible = True
                                        Exit Sub
                                    End If
                                End If
                            End If

                        Case "KBR"
                            If ddlPayType.SelectedItem.Value = 4 Then
                                lblErrPayType.Text = "This payment type cannot use Kas account"
                                lblErrPayType.Visible = True
                                Exit Sub
                            End If
                            If lblPaymentID.Text <> "" Then
                                If Left(lblPaymentID.Text, 3) <> "XXX" Then
                                    If Left(lblPaymentID.Text, 2) <> "KR" Then
                                        lblErrPayType.Text = "<br>ID transaction already set to Kas Kecil."
                                        lblErrPayType.Visible = True
                                        Exit Sub
                                    End If
                                End If
                            End If

                        Case Else
                            If ddlPayType.SelectedItem.Value = 1 Then
                                lblErrPayType.Text = "This payment type cannot use RK/UM account"
                                lblErrPayType.Visible = True
                                Exit Sub
                            End If
                    End Select
                End If
            End If
        End If

        strParamNameCoaValidate = "SEARCHSTR|SORTEXP"
        strParamValueCoaValidate = " And ACC.AccCode = '" & Trim(txtAccCode.Text.Trim) & "'  " & "|Order By ACC.AccCode"

        Try
            intErrNoCoaValidate = objGLtrx.mtdGetDataCommon(strOpCode, _
                                                strParamNameCoaValidate, _
                                                strParamValueCoaValidate, _
                                                objCOADs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GENERATE_INVOICE_WM_GET_HEADER&errmesg=" & Exp.ToString() & "&redirect=ap/trx/ap_trx_invrcv_wm_list.aspx")
        End Try

        If objCOADs.Tables(0).Rows.Count = 0 Then
            'txtAccName.Text = objCOADs.Tables(0).Rows(0).Item("Description")
            lblAccCodeErr.Visible = True
            lblAccCodeErr.Text = "Account Code is not registered"
            Exit Sub
        End If

        'end of validasi coa terdaftar

        If CheckDate(txtDateCreated.Text.Trim(), indDate) = False Then
            lblDate.Visible = True
            lblFmt.Visible = True
            lblDate.Text = "<br>Date Entered should be in the format"
            Exit Sub
        End If

        Dim intInputPeriod As Integer = Year(strDate) * 100 + Month(strDate)
        Dim intCurPeriod As Integer = (CInt(strAccYear) * 100) + CInt(strAccMonth)
        Dim intSelPeriod As Integer = (CInt(strSelAccYear) * 100) + CInt(strSelAccMonth)

        If Session("SS_FILTERPERIOD") = "0" Then
            If intCurPeriod < intInputPeriod Then
                lblDate.Visible = True
                lblDate.Text = "Invalid transaction date."
                Exit Sub
            End If
        Else
            If intSelPeriod <> intInputPeriod Then
                lblDate.Visible = True
                lblDate.Text = "Invalid transaction date."
                Exit Sub
            End If
            If intSelPeriod < intCurPeriod And intLevel < 2 Then
                lblDate.Visible = True
                lblDate.Text = "This period already locked."
                Exit Sub
            End If
        End If

        If txtPaymentTo.Text = "" Then
            lblConfirmErr.Visible = True
            lblConfirmErr.Text = "Payment To/Receive From cannot be empty."
            Exit Sub
        End If

        If CheckRequiredField() Then Exit Sub

        lblCurrentPeriod.Text = IIf(lblPaymentID.Text = "", "", lblCurrentPeriod.Text)

        If ddlPayType.SelectedItem.Value = "0" Or ddlPayType.SelectedItem.Value = "3" Then
            If strBank = "" Then
                lblErrBank.Visible = True
                Exit Sub
            End If
            If txtGiroDate.Text = "" Then
                lblDateGiro.Visible = True
                lblDateGiro.Text = "Please enter Giro Date"
                Exit Sub
            End If
            If CheckDate(txtGiroDate.Text.Trim(), indDate) = False Then
                lblDateGiro.Visible = True
                lblFmtGiro.Visible = True
                lblDateGiro.Text = "<br>Date Entered should be in the format"
                Exit Sub
            End If
            'If txtChequeNo.Text = "" Then
            '    lblErrCheque.Visible = True
            '    Exit Sub
            'End If
            If lblCurrentPeriod.Text <> "" Then
                If Month(strGiroDate) >= Mid(lblCurrentPeriod.Text, 5) And Year(strGiroDate) >= Mid(lblCurrentPeriod.Text, 1, 4) Then
                Else
                    If rblCashBankType.SelectedValue = objCBTrx.EnumCashBankType.Payment Then
                        lblDateGiro.Visible = True
                        lblDateGiro.Text = "Invalid transaction date."
                        Exit Sub
                    End If
                End If
                If Year(strGiroDate) & Format(Month(strGiroDate), "00") & Format(Day(strGiroDate), "00") < Year(strDate) & Format(Month(strDate), "00") & Format(Day(strDate), "00") Then
                    If rblCashBankType.SelectedValue = objCBTrx.EnumCashBankType.Payment Then
                        lblDateGiro.Visible = True
                        lblDateGiro.Text = "Invalid transaction date."
                        Exit Sub
                    End If
                End If
            Else
                If Year(strGiroDate) & Format(Month(strGiroDate), "00") & Format(Day(strGiroDate), "00") < Year(strDate) & Format(Month(strDate), "00") & Format(Day(strDate), "00") Then
                    If rblCashBankType.SelectedValue = objCBTrx.EnumCashBankType.Payment Then
                        lblDateGiro.Visible = True
                        lblDateGiro.Text = "Invalid transaction date."
                        Exit Sub
                    End If
                End If
                If Month(strGiroDate) < strAccMonth And Year(strGiroDate) <= strAccYear Then
                    If rblCashBankType.SelectedValue = objCBTrx.EnumCashBankType.Payment Then
                        lblDateGiro.Visible = True
                        lblDateGiro.Text = "Invalid transaction date."
                        Exit Sub
                    End If
                End If
            End If
        End If

        If lblPaymentID.Text <> "" And Left(lblPaymentID.Text.Trim, 3) <> "XXX" Then
            If txtDateCreated.Enabled = True Then
                If hidPrevID.Value = "" Then
                    'If hidNextID.Value <> "" Then
                    '    If Day(strDate) > hidNextDate.Value Then
                    '        lblDate.Visible = True
                    '        lblDate.Text = "Date cannot be higher than last transaction date."
                    '        Exit Sub
                    '    End If
                    'End If
                    hidNextDate.Visible = False
                Else
                    ''remark sementara
                    'If hidPrevID.Value <> "" Then
                    '    If Day(strDate) < hidPrevDate.Value Then
                    '        lblDate.Visible = True
                    '        lblDate.Text = "Date cannot be smaller than previous transaction date."
                    '        Exit Sub
                    '    End If
                    '    If hidNextID.Value <> "" Then
                    '        'If Day(strDate) > hidNextDate.Value Then
                    '        '    lblDate.Visible = True
                    '        '    lblDate.Text = "Date cannot be higher than last transaction date."
                    '        '    Exit Sub
                    '        'End If
                    '        hidNextDate.Visible = False
                    '    End If
                    'Else
                    '    'If Day(strDate) < hidPrevDate.Value Then
                    '    '    lblDate.Visible = True
                    '    '    lblDate.Text = "Date cannot be smaller than previous transaction date."
                    '    '    Exit Sub
                    '    'End If
                    '    hidPrevDate.Visible = False
                    'End If
                End If
            End If

        ElseIf ddlPayType.SelectedItem.Value <> 2 Then
            ''remark sementara
            ''cek tanggal
            'Dim objLastID As New Object
            'Dim strSearchID As String
            'Dim intCBDate As Integer
            'Dim intPYDate As Integer
            'Dim strOpCd As String
            'Dim strParamName As String
            'Dim strParamValue As String

            'strOpCd = "CB_CLSTRX_SEARCH_LASTID"
            'strSearchID = " LocCode = '" & strLocation & "' And AccMonth = '" & Trim(strAccMonth) & "' And AccYear = '" & Trim(strAccYear) & "' And Left(LTrim(CashBankID),3) = '" & Trim(strInitial) & "' "
            'strParamName = "TABLE|FIELD|STRSEARCH"
            'strParamValue = "CB_CASHBANK" & "|" & "CASHBANKID" & "|" & strSearchID

            'Try
            '    intErrNo = objGLtrx.mtdGetDataCommon(strOpCd, _
            '                                        strParamName, _
            '                                        strParamValue, _
            '                                        objLastID)
            'Catch Exp As System.Exception
            '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ID_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
            'End Try

            'If objLastID.Tables(0).Rows.Count > 0 Then
            '    intCBDate = Day(objGlobal.GetLongDate(Trim(objLastID.Tables(0).Rows(0).Item("DocDate"))))
            'Else
            '    intCBDate = 0
            'End If

            'strSearchID = " LocCode = '" & strLocation & "' And AccMonth = '" & Trim(strAccMonth) & "' And AccYear = '" & Trim(strAccYear) & "' And Left(LTrim(PaymentID),3) = '" & Trim(strInitial) & "' "
            'strParamName = "TABLE|FIELD|STRSEARCH"
            'strParamValue = "CB_PAYMENT" & "|" & "PAYMENTID" & "|" & strSearchID
            'Try
            '    intErrNo = objGLtrx.mtdGetDataCommon(strOpCd, _
            '                                        strParamName, _
            '                                        strParamValue, _
            '                                        objLastID)
            'Catch Exp As System.Exception
            '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ID_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
            'End Try
            'If objLastID.Tables(0).Rows.Count > 0 Then
            '    intPYDate = Day(objGlobal.GetLongDate(Trim(objLastID.Tables(0).Rows(0).Item("DocDate"))))
            'Else
            '    intPYDate = 0
            'End If


            'If intCBDate >= intPYDate Then
            '    If Day(strDate) < intCBDate Then
            '        lblDate.Visible = True
            '        lblDate.Text = "Date cannot be smaller than last transaction date."
            '        Exit Sub
            '    End If
            'Else
            '    If Day(strDate) < intPYDate Then
            '        lblDate.Visible = True
            '        lblDate.Text = "Date cannot be smaller than last transaction date."
            '        Exit Sub
            '    End If
            'End If
        End If

        If hidCOATax.Value = 1 Then
            Dim dblTotalDPP As Double = 0

            TaxLnID = lstTaxObject.SelectedItem.Value
            TaxRate = hidTaxObjectRate.Value

            If Trim(txtSupCode.Text) = "" And lCDbl(strDRTotalAmountDPP) = 0 Then
                'bayar kas negara tdk ada validasi
            Else
                If Trim(txtSupCode.Text) = "" Then
                    lblConfirmErr.Visible = True
                    lblConfirmErr.Text = "This transaction has tax, supplier has to registered first"
                    Exit Sub
                End If

                If CDbl(strDRTotalAmountDPP) = 0 And lCDbl(strCRTotalAmountDPP) = 0 Then
                    If TaxLnID = "" Then
                    Else
                        lblTwoAmountDPP.Visible = True
                        Exit Sub
                    End If
                End If
                If CDbl(strDRTotalAmountDPP) <> 0 Or lCDbl(strCRTotalAmountDPP) <> 0 Then
                    If TaxLnID = "" And hidTaxPPN.Value = 0 Then
                        lblTaxObjectErr.Visible = True
                        lblTaxObjectErr.Text = "Please select Tax Object"
                        Exit Sub
                    Else
                    End If
                End If
                If CDbl(strDRTotalAmountDPP) <> 0 Then
                    DPPAmount = lCDbl(strDRTotalAmountDPP)
                End If
                If CDbl(strCRTotalAmountDPP) <> 0 Then
                    DPPAmount = lCDbl(strCRTotalAmountDPP) * -1
                End If
            End If
        Else
            TaxLnID = ""
            TaxRate = 0
            DPPAmount = 0
        End If

        If lCDbl(strCRTotalAmount) = 0 And CDbl(strDRTotalAmount) = 0 Then
            lblTwoAmount.Visible = True
            Exit Sub
        End If

        If CDbl(strDRTotalAmount) <> 0 Then
            dblTotal = lCDbl(strDRTotalAmount)
        End If
        If CDbl(strCRTotalAmount) <> 0 Then
            dblTotal = lCDbl(strCRTotalAmount) * -1
        End If

        If rblCashBankType.SelectedValue = objCBTrx.EnumCashBankType.Payment Then
            strDocPrefix = objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CBPaymentOther)
            strDocPrefixLn = objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CBPaymentOtherLn)
        Else
            strDocPrefix = objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CBReceiptOther)
            strDocPrefixLn = objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CBReceiptOtherLn)
        End If
        'If lblPaymentID.Text <> "" Then
        '    If intLevel = 0 Then
        '        If lblStatus.Text = objCBTrx.mtdGetCashBankStatus(objCBTrx.EnumCashBankStatus.Verified) Then
        '            If Left(Trim(Request.Form("txtAccCode")), 3) = "65." Or Left(Trim(Request.Form("txtAccCode")), 5) = "71.19" Or Left(Trim(Request.Form("txtAccCode")), 3) = "110" Then
        '            Else
        '                lblConfirmErr.Visible = True
        '                lblConfirmErr.Text = "You do not have permission to input this journal."
        '                Exit Sub
        '            End If
        '            If hidHadCOATax.Value > 0 Then
        '                If CInt(hidTaxStatus.Value) = objCBTrx.EnumTaxStatus.Unverified Then
        '                    lblConfirmErr.Visible = True
        '                    lblConfirmErr.Text = "This transaction contain tax account, please verify first."
        '                    Exit Sub
        '                End If
        '            End If
        '        ElseIf lblStatus.Text = objCBTrx.mtdGetCashBankStatus(objCBTrx.EnumCashBankStatus.Active) Then
        '            If Left(Trim(Request.Form("txtAccCode")), 3) = "110" And ddlPayType.SelectedItem.Value <> "2" Then
        '                If hidHadCOATax.Value > 0 Then
        '                    If CInt(hidTaxStatus.Value) = objCBTrx.EnumTaxStatus.Unverified Then
        '                        lblConfirmErr.Visible = True
        '                        lblConfirmErr.Text = "This transaction contain tax account, please verify first."
        '                        Exit Sub
        '                    End If
        '                ElseIf UCase(Trim(strUserId)) <> UCase(Trim(hidUserID.Value)) Then
        '                    lblConfirmErr.Visible = True
        '                    lblConfirmErr.Text = "You do not have permission to process this transaction."
        '                    Exit Sub
        '                End If
        '            ElseIf UCase(Trim(strUserId)) <> UCase(Trim(hidUserID.Value)) Then
        '                lblConfirmErr.Visible = True
        '                lblConfirmErr.Text = "You do not have permission to process this transaction."
        '                Exit Sub
        '            End If
        '        End If
        '    ElseIf intLevel = 1 Then
        '        If lblStatus.Text = objCBTrx.mtdGetCashBankStatus(objCBTrx.EnumCashBankStatus.Verified) Then
        '            lblConfirmErr.Visible = True
        '            lblConfirmErr.Text = "You do not have permission to input this journal."
        '            Exit Sub
        '        End If
        '        If Left(Trim(Request.Form("txtAccCode")), 3) = "110" And ddlPayType.SelectedItem.Value <> "2" Then
        '            If hidHadCOATax.Value > 0 Then
        '                If CInt(hidTaxStatus.Value) = objCBTrx.EnumTaxStatus.Unverified Then
        '                    lblConfirmErr.Visible = True
        '                    lblConfirmErr.Text = "This transaction contain tax account, please verify first."
        '                    Exit Sub
        '                End If
        '            End If
        '        End If
        '    Else
        '        If Left(Trim(Request.Form("txtAccCode")), 3) = "110" And ddlPayType.SelectedItem.Value <> "2" Then
        '            If hidHadCOATax.Value > 0 Then
        '                If CInt(hidTaxStatus.Value) = objCBTrx.EnumTaxStatus.Unverified Then
        '                    lblConfirmErr.Visible = True
        '                    lblConfirmErr.Text = "This transaction contain tax account, please verify first."
        '                    Exit Sub
        '                End If
        '            End If
        '        End If
        '    End If
        'End If

        'If lblPaymentID.Text <> "" Then
        '    If lblStatus.Text = objCBTrx.mtdGetCashBankStatus(objCBTrx.EnumCashBankStatus.Verified) Then
        '        If intLevel = 0 Then
        '            If Left(Trim(Request.Form("txtAccCode")), 3) = "65." Or Left(Trim(Request.Form("txtAccCode")), 5) = "71.19" Or Left(Trim(Request.Form("txtAccCode")), 3) = "110" Then
        '            Else
        '                lblConfirmErr.Visible = True
        '                lblConfirmErr.Text = "You dont have permission to input this journal."
        '                Exit Sub
        '            End If
        '        End If
        '    End If
        'End If

        If lblPaymentID.Text <> "" Then
            Select Case ddlPayType.SelectedItem.Value
                Case 0
                    Select Case rblCashBankType.SelectedValue
                        Case objCBTrx.EnumCashBankType.Payment
                            strInitial = "BBK"
                        Case objCBTrx.EnumCashBankType.Receipt
                            strInitial = "BBT"
                        Case objCBTrx.EnumCashBankType.Transfer
                            strInitial = "STF"
                    End Select

                Case 1
                    Select Case rblCashBankType.SelectedValue
                        Case objCBTrx.EnumCashBankType.Payment
                            Select Case Trim(strBank)
                                Case "KKL", "KKK"
                                    strInitial = "KKK"
                                Case Else
                                    strInitial = "KRK"
                            End Select

                        Case objCBTrx.EnumCashBankType.Receipt
                            Select Case Trim(strBank)
                                Case "KKL", "KKK"
                                    strInitial = "KKT"
                                Case Else
                                    strInitial = "KRT"
                            End Select

                        Case objCBTrx.EnumCashBankType.Transfer
                            strInitial = "STF"
                    End Select

                Case 2
                    strInitial = "XXX"

                Case 3
                    Select Case rblCashBankType.SelectedValue
                        Case objCBTrx.EnumCashBankType.Payment
                            strInitial = "BBK"
                        Case objCBTrx.EnumCashBankType.Receipt
                            strInitial = "BBT"
                        Case objCBTrx.EnumCashBankType.Transfer
                            strInitial = "STF"
                    End Select

                Case 4
                    strInitial = "PPL"

            End Select
        Else
            strInitial = "XXX"
        End If

        
        Select Case rblCashBankType.SelectedValue
            Case objCBTrx.EnumCashBankType.Payment
                strInOrOut = "O"
            Case objCBTrx.EnumCashBankType.Receipt
                strInOrOut = "I"
        End Select

        'If Month(strDate) < strAccMonth And Year(strDate) <= strAccYear Then
        '    lblDate.Visible = True
        '    lblDate.Text = "Invalid transaction date."
        '    Exit Sub
        'End If
        If lblCurrentPeriod.Text <> "" Then
            If Month(strDate) = Mid(lblCurrentPeriod.Text, 5) And Year(strDate) = Mid(lblCurrentPeriod.Text, 1, 4) Then
            Else
                lblDate.Visible = True
                lblDate.Text = "Invalid transaction date."
                Exit Sub
            End If
        End If

        strAccYear = Year(strDate)
        strAccMonth = Month(strDate)

        'Select Case strCompany
        '    Case "SAM", "MIL"
        '        strNewIDFormat = strDocPrefix & Right(strAccYear, CInt(Session("SS_ROUNDNO"))) & IIf(Len(Trim(strAccMonth)) = 1, "0" & strAccMonth, strAccMonth)
        '    Case Else
        '        strNewIDFormat = strInitial & "/" & strCompany & "/" & strLocation & "/" & strInOrOut & "/" & strAccYear & IIf(Len(Trim(strAccMonth)) = 1, "0" & strAccMonth, strAccMonth) & "/"
        'End Select
        'strNewIDFormat = strInitial & "/" & strCompany & "/" & strLocation & "/" & strInOrOut & "/" & strAccYear & IIf(Len(Trim(strAccMonth)) = 1, "0" & strAccMonth, strAccMonth) & "/"
        strNewIDFormat = strInitial & "/" & strCompany & "/" & strLocation & "/" & IIf(Len(Trim(strAccMonth)) = 1, "0" & strAccMonth, strAccMonth) & "/" & Trim(strAccYear) & "/"

        If lblPaymentID.Text = "" Then
            strTxParam = "|" & rblCashBankType.SelectedValue & "|" & ddlPayType.SelectedValue & _
                          "|" & strBank & "|" & IIf(Trim(txtChequeNo.Text) = "", "", Replace(txtChequeNo.Text, "'", "''")) & _
                          "|" & IIf(Trim(txtPaymentTo.Text) = "", "", Trim(txtPaymentTo.Text)) & "|" & strLocation & "|" & strAccMonth & _
                          "|" & strAccYear & "||" & strDate & "||" & strGiroDate & "|" & strUserId & "||" & strNewIDFormat & _
                          "|" & IIf(Trim(ddlSplBankAccNo.SelectedItem.Value) = "", "", Replace(ddlSplBankAccNo.SelectedItem.Value, "'", "''")) & "|" & strRemark & _
                          "|" & ddlBankTo.SelectedValue & "|" & txtSupCode.Text & "|" & objCBTrx.EnumTaxStatus.Unverified & _
                          "|" & strBankAccNo & "|" & IIf(chkChequeCash.Checked = True, "1", "2") & "|" & ddlChequeHandOver.SelectedItem.Value

            Try

                intErrNo = objCBTrx.mtdUpdCashBankDetail(strOpCdTxDet_ADD, _
                                                    strOpCdTxDet_UPD, _
                                                    strUserId, _
                                                    strTxParam, _
                                                    strDocPrefix, _
                                                    ErrorChk, _
                                                    TxResultID)
                lblPaymentID.Text = TxResultID
                LoadTxDetails()
                DisplayFromDB()

            Catch Exp As System.Exception
                If intErrNo > 0 Then
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADD_NEWCASHBANKDETAIL&errmesg=" & lblErrMessage.Text & "&redirect=CB/Trx/cb_trx_CashBankList.aspx")
                End If
            End Try

        End If

        Dim strStaffAdvID As String
        Dim strStaffAdvDoc As String

        If ddlRefNo.SelectedItem.Value = "" Then
            strStaffAdvID = ""
            strStaffAdvDoc = ""
        Else
            If rblCashBankType.SelectedValue = objCBTrx.EnumCashBankType.Payment Or rblCashBankType.SelectedValue = objCBTrx.EnumCashBankType.Transfer Then
                strStaffAdvID = ddlRefNo.SelectedItem.Value
                strStaffAdvDoc = ""
            Else
                arrParam = Split(Trim(ddlRefNo.SelectedItem.Value), "|")
                strStaffAdvID = Trim(arrParam(0))
                strStaffAdvDoc = Trim(arrParam(1))
            End If
        End If

        If lblTxLnID.Text = "" Then
            strTxLnParam = lblPaymentID.Text & "|" & IIf(Trim(txtDescLn.Text) = "", "", txtDescLn.Text) & "|" & dblQty & "|" & _
                           dblTotal & "|" & dblTotal & "|" & Request.Form("txtAccCode") & "|"

            If ddlChargeLevel.SelectedIndex = 0 Then
                strTxLnParam = strTxLnParam & Request.Form("ddlPreBlock")
            Else
                strTxLnParam = strTxLnParam & Request.Form("lstBlock")
            End If

            strTxLnParam = strTxLnParam & "||" & Request.Form("lstVehCode") & "|" & _
                           Request.Form("lstVehExp") & "|" & ddlLocation.SelectedItem.Value.Trim() & "|" & _
                           Trim(strStaffAdvID) & "|" & _
                           Trim(TaxLnID) & "|" & _
                           TaxRate & "|" & _
                           DPPAmount & "|" & _
                           Trim(ddlGiroNo.SelectedItem.Value) & "|" & _
                           Trim(strStaffAdvDoc) & "|" & _
                           Trim(strFakturNo) & "|" & _
                           strFPDate & "|" & _
                           Trim(txtGiroName.Text)

            Try
                If ddlChargeLevel.SelectedIndex = 0 And RowPreBlk.Visible = True Then
                    strParamList = Session("SS_LOCATION") & "|" & _
                                       txtAccCode.Text.Trim & "|" & _
                                       ddlPreBlock.SelectedItem.Value.Trim & "|" & _
                                       objGLSetup.EnumBlockStatus.Active & "|" & _
                                       strAccMonth & "|" & strAccYear

                    intErrNo = objCBTrx.mtdAddCashBankLnByBlock(strOpCodeGLSubBlkByBlk, _
                                                              strParamList, _
                                                              strOpCdTxLine_ADD, _
                                                              strDocPrefixLn, _
                                                              strTxLnParam, _
                                                              ErrorChk, _
                                                              strLocType)
                Else
                    intErrNo = objCBTrx.mtdAddCashBankLn(strOpCdTxLine_ADD, _
                                                       strDocPrefixLn, _
                                                       strTxLnParam, _
                                                       ErrorChk)
                End If
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADD_ADDCASHBANKLINE&errmesg=" & lblErrMessage.Text & "&redirect=CB/Trx/cb_trx_CashBankList.aspx")
                End If
            End Try
        Else
            
            strTxLnParam = lblTxLnID.Text & "|" & IIf(Trim(txtDescLn.Text) = "", "", txtDescLn.Text) & "|" & dblQty & "|" & _
                           dblTotal & "|" & dblTotal & "|" & Request.Form("txtAccCode") & "|" & _
                           Request.Form("lstBlock") & "||" & Request.Form("lstVehCode") & "|" & _
                           Request.Form("lstVehExp") & "|" & ddlLocation.SelectedItem.Value.Trim() & "|" & _
                           Trim(strStaffAdvID) & "|" & lblPaymentID.Text & "|" & _
                           Trim(TaxLnID) & "|" & _
                           TaxRate & "|" & _
                           DPPAmount & "|" & _
                           Trim(ddlGiroNo.SelectedItem.Value) & "|" & _
                           Trim(strStaffAdvDoc) & "|" & _
                           Trim(txtGiroName.Text)

            Try
                intErrNo = objCBTrx.mtdUpdCashBankLine(strOpCdTxLine_UPD, _
                                                           strTxLnParam.ToString, _
                                                           ErrorChk)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=UPD_CASHBANKLINE&errmesg=" & lblErrMessage.Text & "&redirect=CB/Trx/cb_trx_CashBankList.aspx")
            End Try

            'If Trim(TaxLnID) <> "" Then
            '    strOpCdTxLine_UPD = "CB_CLSTRX_CASHBANK_LINE_UPD_TAX"
            '    strParamName = "CASHBANKID|CASHBANKLNID|ACCCODE|DPPAMOUNT"
            '    strParamValue = Trim(lblPaymentID.Text) & "|" & Trim(lblTxLnID.Text) & "|" & Request.Form("txtAccCode") & "|" & DPPAmount

            '    Try
            '        intErrNo = objGLtrx.mtdInsertDataCommon(strOpCdTxLine_UPD, _
            '                                                strParamName, _
            '                                                strParamValue)

            '    Catch Exp As System.Exception
            '        Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_CASHBANK_UPDATE_TRXID&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_CashBankDet.aspx")
            '    End Try
            'End If

            '--ppn/pph
            If hidCOATax.Value = 1 Then
                strOpCd = "CB_CLSTRX_CASHBANK_TAX_UPDATE"
                strParamName = "LOCCODE|UPDATEID|CASHBANKID|CASHBANKLNID|ACCMONTH|ACCYEAR|SUPPLIERCODE|TAXID|TAXLNID|DPPAMOUNT|TAXAMOUNT|KPPINIT|STATUS|ACCCODE|TAXDATE"
                strParamValue = strLocation & "|" & strUserId & "|" & Trim(lblPaymentID.Text) & "|" & Trim(lblTxLnID.Text) & "|" & strAccMonth & "|" & strAccYear & "|" & _
                                Trim(txtSupCode.Text) & "|" & _
                                Trim(txtFakturPjkNo.Text) & "|" & _
                                TaxLnID & "|" & _
                                DPPAmount & "|" & dblTotal & "|" & _
                                strLocation & "|" & "1" & "|" & _
                                Request.Form("txtAccCode") & "|" & _
                                strFPDate

                Try
                    intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd, _
                                                            strParamName, _
                                                            strParamValue)

                Catch Exp As System.Exception
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=UPD_TAX&errmesg=" & Exp.ToString() & "&redirect=pu/trx/AP_trx_InvoiceRcvList")
                End Try

                lstTaxObject.SelectedIndex = 0
                txtFakturPjkNo.Text = ""
                txtFakturDate.Text = ""
            End If

            lblTxLnID.Text = ""
            dgTx.EditItemIndex = -1
        End If

        Select Case ErrorChk
            Case 1
                lblerror.Visible = True
        End Select

        'penomeran pindah ke verified
        'If (Left(Trim(lblPaymentID.Text), 3) <> "TBS" Or Left(Trim(lblPaymentID.Text), 3) <> "CPO" Or Left(Trim(lblPaymentID.Text), 3) <> "KER" Or Left(Trim(lblPaymentID.Text), 3) <> "CKG" Or Left(Trim(lblPaymentID.Text), 3) <> "OTH") Then
        '    If ddlPayType.SelectedItem.Value <> 2 And Left(Trim(lblPaymentID.Text), 3) = "XXX" Then
        '        Dim strOpCd_FindLastID As String = "ADMIN_CLSTRX_SEARCH_LASTID_CB"
        '        Dim strLastIDSearch1 As String
        '        Dim strLastIDSearch2 As String
        '        Dim strLastIDSearch3 As String
        '        Dim strNewID As String
        '        Dim objLastIDDs As New Object
        '        strOpCd = "CB_CLSTRX_CASHBANK_UPDATE_TRXID"

        '        strLastIDSearch1 = " LocCode = '" & strLocation & "' And AccMonth = '" & Trim(strAccMonth) & "' And AccYear = '" & Trim(strAccYear) & "' And Left(LTrim(CashBankID),3) = '" & Trim(strInitial) & "' "
        '        strLastIDSearch2 = " LocCode = '" & strLocation & "' And AccMonth = '" & Trim(strAccMonth) & "' And AccYear = '" & Trim(strAccYear) & "' And Left(LTrim(PaymentID),3) = '" & Trim(strInitial) & "' "
        '        strLastIDSearch3 = " LocCode = '" & strLocation & "' And AccMonth = '" & Trim(strAccMonth) & "' And AccYear = '" & Trim(strAccYear) & "' And Left(LTrim(ReceiptID),3) = '" & Trim(strInitial) & "' "
        '        strParamName = "STRSEARCH1|STRSEARCH2|STRSEARCH3"
        '        strParamValue = strLastIDSearch1 & "|" & strLastIDSearch2 & "|" & strLastIDSearch3

        '        Try
        '            intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_FindLastID, _
        '                                                strParamName, _
        '                                                strParamValue, _
        '                                                objLastIDDs)
        '        Catch Exp As System.Exception
        '            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ID_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        '        End Try

        '        If objLastIDDs.Tables(0).Rows.Count > 0 Then
        '            strNewID = Trim(objLastIDDs.Tables(0).Rows(0).Item("NewTrxID"))
        '        Else
        '            strNewID = "1"
        '        End If

        '        strNewIDFormat = strInitial & "/" & strCompany & "/" & strLocation & "/" & IIf(Len(Trim(strAccMonth)) = 1, "0" & strAccMonth, strAccMonth) & "/" & Trim(strAccYear) & "/" & Format(Val(strNewID), "0000")

        '        strParamName = "SOURCETYPE|CHEQUENO|BANKCODE|BANKTO|SPLBANKACCNO|REMARK|CHEQUEPRINTDATE|PRINTDATE|LOCCODE|CASHBANKID|CASHBANKNEWID|UPDATEID"

        '        strParamValue = Trim(ddlPayType.SelectedItem.Value) & _
        '                        "|" & Trim(txtChequeNo.Text) & _
        '                        "|" & Trim(strBank) & _
        '                        "|" & Trim(ddlBankTo.SelectedItem.Value) & _
        '                        "|" & Trim(ddlSplBankAccNo.SelectedItem.Value) & _
        '                        "|" & Trim(Replace(txtRemark.Text, "'", "''")) & _
        '                        "|" & "" & _
        '                        "|" & strGiroDate & _
        '                        "|" & Trim(strLocation) & _
        '                        "|" & Trim(lblPaymentID.Text) & _
        '                        "|" & Trim(strNewIDFormat) & _
        '                        "|" & Trim(strUserId)

        '        Try
        '            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd, _
        '                                                    strParamName, _
        '                                                    strParamValue)

        '        Catch Exp As System.Exception
        '            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_CASHBANK_UPDATE_TRXID&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_CashBankDet.aspx")
        '        End Try

        '        lblPaymentID.Text = Trim(strNewIDFormat)
        '    End If
        'End If

        SaveBtn_Click(sender, e)
        UpdateCBLine()
        Initialize()
        BindGrid()
        onLoad_Button()
        RowTax.Visible = False
        RowTaxAmt.Visible = False
        RowFP.Visible = False
        RowFPDate.Visible = False
        'ddlGiroNo.SelectedItem.Value = ""
        txtGiroName.Text = ""

        blnShortCut.Text = False

        If lblTxLnID.Text <> "" Then
            AddDtlBtn.Visible = False
            SaveDtlBtn.Visible = True
        Else
            AddDtlBtn.Visible = True
            SaveDtlBtn.Visible = False
        End If

    End Sub

    Sub Initialize()
        txtDRTotalAmount.Text = 0
        txtCRTotalAmount.Text = 0
        'lstAccCode.SelectedIndex = 0
        lstBlock.SelectedIndex = 0
        lstVehCode.SelectedIndex = 0
        lstVehExp.SelectedIndex = 0
        txtDPPAmountDR.Text = 0
        txtDPPAmountCR.Text = 0
        lstTaxObject.SelectedIndex = 0
        ddlChargeLevel.SelectedIndex = Session("SS_BLOCK_CHARGE_DEFAULT")
        RowChargeLevel.Visible = Session("SS_BLOCK_CHARGE_VISIBLE")
        ToggleChargeLevel()
        ddlRefNo.SelectedIndex = 0
    End Sub

    Sub DataGrid_ItemCreated(ByVal Sender As Object, ByVal e As DataGridItemEventArgs)
        If dgTx.Enabled = True Then
            Select Case e.Item.ItemType
                Case ListItemType.Item, ListItemType.AlternatingItem
                    Dim lbl As Label
                    Dim lblAmt As Label
                    lbl = e.Item.FindControl("lblIdx")
                    lbl.Text = e.Item.ItemIndex.ToString + 1

                    Dim DeleteButton As LinkButton
                    Dim EditButton As LinkButton

                    Select Case lblStatus.Text.Trim
                        Case objCBTrx.mtdGetCashBankStatus(objCBTrx.EnumCashBankStatus.Active)
                            DeleteButton = e.Item.FindControl("lbDelete")
                            DeleteButton.Visible = True
                            DeleteButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                            EditButton = e.Item.FindControl("lbEdit")
                            EditButton.Visible = True
                        Case objCBTrx.mtdGetCashBankStatus(objCBTrx.EnumCashBankStatus.Verified)
                            If intLevel = 0 Then
                                DeleteButton = e.Item.FindControl("lbDelete")
                                DeleteButton.Visible = False
                                DeleteButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                                EditButton = e.Item.FindControl("lbEdit")
                                EditButton.Visible = False
                            Else
                                DeleteButton = e.Item.FindControl("lbDelete")
                                DeleteButton.Visible = True
                                DeleteButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                                EditButton = e.Item.FindControl("lbEdit")
                                EditButton.Visible = True
                            End If
                        Case objCBTrx.mtdGetCashBankStatus(objCBTrx.EnumCashBankStatus.Deleted), _
                             objCBTrx.mtdGetCashBankStatus(objCBTrx.EnumCashBankStatus.Closed), _
                             objCBTrx.mtdGetCashBankStatus(objCBTrx.EnumCashBankStatus.Confirmed)
                            DeleteButton = e.Item.FindControl("lbDelete")
                            DeleteButton.Visible = False
                            EditButton = e.Item.FindControl("lbEdit")
                            EditButton.Visible = False
                    End Select

                    lblAmt = e.Item.FindControl("lblAmount")
                    lbl = e.Item.FindControl("lblAccTx")

                    If Sign(lCDbl(Replace(Replace(lblAmt.Text.Trim, ".", ""), ",", "."))) = 1 Then
                        lbl.Text = "DR"
                    ElseIf Sign(lCDbl(Replace(Replace(lblAmt.Text.Trim, ".", ""), ",", "."))) = -1 Then
                        lbl.Text = "CR"
                    End If

                Case ListItemType.EditItem
                    Dim lbl As Label
                    Dim lblAmt As Label
                    lbl = e.Item.FindControl("lblIdx")
                    lbl.Text = e.Item.ItemIndex.ToString + 1

                    lblAmt = e.Item.FindControl("lblAmount")
                    lbl = e.Item.FindControl("lblAccTx")

                    If Sign(lCDbl(Replace(Replace(lblAmt.Text.Trim, ".", ""), ",", "."))) = 1 Then
                        lbl.Text = "DR"
                    ElseIf Sign(lCDbl(Replace(Replace(lblAmt.Text.Trim, ".", ""), ",", "."))) = -1 Then
                        lbl.Text = "CR"
                    End If
            End Select

        Else
            Select Case e.Item.ItemType
                Case ListItemType.Item, ListItemType.AlternatingItem
                    Dim lbl As Label
                    lbl = e.Item.FindControl("lblIdx")
                    lbl.Text = e.Item.ItemIndex.ToString + 1

                    Dim DeleteButton As LinkButton
                    DeleteButton = e.Item.FindControl("lbDelete")
                    DeleteButton.Enabled = False
            End Select
        End If

    End Sub

    Function CheckRequiredField() As Boolean
        Dim intAccType As Integer
        Dim intAccPurpose As Integer
        Dim intNurseryInd As Integer
        Dim blnFound As Boolean
        Dim strAcc As String = Request.Form("txtAccCode")
        Dim strBlk As String
        Dim strVeh As String = Request.Form("lstVehCode")
        Dim strVehExp As String = Request.Form("lstVehExp")

        If ddlChargeLevel.SelectedIndex = 1 Then
            strBlk = Request.Form("lstBlock")
        Else
            strBlk = Request.Form("ddlPreBlock")
        End If

        If txtDescLn.Text = "" Then
            lblDescErr.Visible = True
            Return True
        End If

        If ddlLocation.SelectedIndex = 0 Then
            lblLocationErr.Visible = True
            Return True
        End If

        If strAcc = "" Then
            lblAccCodeErr.Visible = True
            Return True
        End If

        GetAccountDetails(strAcc, intAccType, intAccPurpose, intNurseryInd, blnFound)

        If blnFound = True Then
            If intAccType = objGLSetup.EnumAccountType.BalanceSheet And intNurseryInd = objGLSetup.EnumNurseryAccount.No Then
                Return False
            ElseIf intAccType = objGLSetup.EnumAccountType.BalanceSheet And intNurseryInd = objGLSetup.EnumNurseryAccount.Yes Then
                If strBlk = "" Then
                    If ddlChargeLevel.SelectedIndex = 1 Then
                        lblBlockErr.Visible = True
                    Else
                        lblPreBlockErr.Visible = True
                    End If
                    Return True
                Else
                    Return False
                End If
            ElseIf intAccType = objGLSetup.EnumAccountType.ProfitAndLost And _
                intAccPurpose = objGLSetup.EnumAccountPurpose.NonVehicle And _
                strBlk = "" Then

                If ddlChargeLevel.SelectedIndex = 1 Then
                    lblBlockErr.Visible = True
                Else
                    lblPreBlockErr.Visible = True
                End If
                Return True
            ElseIf intAccType = objGLSetup.EnumAccountType.ProfitAndLost And _
                intAccPurpose = objGLSetup.EnumAccountPurpose.VehicleDistribution And _
                strVeh = "" Then
                lblVehCodeErr.Visible = True
                Return True
            ElseIf intAccType = objGLSetup.EnumAccountType.ProfitAndLost And _
                intAccPurpose = objGLSetup.EnumAccountPurpose.VehicleDistribution And _
                strVehExp = "" Then
                lblVehExpCodeErr.Visible = True
                Return True
            ElseIf intAccType = objGLSetup.EnumAccountType.ProfitAndLost And _
                intAccPurpose = objGLSetup.EnumAccountPurpose.Others And _
                strVeh <> "" And strVehExp = "" Then
                lblVehExpCodeErr.Visible = True
                Return True
            ElseIf intAccType = objGLSetup.EnumAccountType.ProfitAndLost And _
                intAccPurpose = objGLSetup.EnumAccountPurpose.Others And _
                strBlk = "" Then
                If ddlChargeLevel.SelectedIndex = 1 Then
                    lblBlockErr.Visible = True
                Else
                    lblPreBlockErr.Visible = True
                End If
                Return True
            ElseIf intAccType = objGLSetup.EnumAccountType.ProfitAndLost And _
                intAccPurpose = objGLSetup.EnumAccountPurpose.Others And _
                strVeh = "" And strVehExp <> "" Then
                lblVehCodeErr.Visible = True
                Return True
            Else
                Return False
            End If
        End If
    End Function

    Sub DisplayFromDB()
        Dim PrevCBID As String
        Dim PrevPYID As String
        Dim PrevCBDate As Integer
        Dim PrevPYDate As Integer
        Dim NextCBID As String
        Dim NextPYID As String
        Dim NextCBDate As Integer
        Dim NextPYDate As Integer

        lblPaymentID.Text = Trim(objDataSet.Tables(0).Rows(0).Item("CashBankID"))
        rblCashBankType.SelectedIndex = CInt(objDataSet.Tables(0).Rows(0).Item("CashBankType")) - 1

        If rblCashBankType.SelectedIndex = 0 Or rblCashBankType.SelectedIndex = 2 Then
            lblPaymentIDTag.Text = "Payment ID :"
            lblPayTypeTag.Text = "Payment Type :*"
            lblPayToTag.Text = "Payment To :*"
        Else
            lblPaymentIDTag.Text = "Receipt ID :"
            lblPayTypeTag.Text = "Receipt Type :*"
            lblPayToTag.Text = "Receipt From :*"
        End If


        ddlPayType.SelectedValue = Trim(objDataSet.Tables(0).Rows(0).Item("SourceType"))
        'ddlBank.SelectedValue = Trim(objDataSet.Tables(0).Rows(0).Item("BankCode"))
        BindBankCode(Trim(objDataSet.Tables(0).Rows(0).Item("BankCode")))
        txtChequeNo.Text = Trim(objDataSet.Tables(0).Rows(0).Item("ChequeNo"))
        txtPaymentTo.Text = Trim(objDataSet.Tables(0).Rows(0).Item("FromTo"))
        lblAccPeriod.Text = Trim(objDataSet.Tables(0).Rows(0).Item("AccMonth")) & "/" & _
                             Trim(objDataSet.Tables(0).Rows(0).Item("AccYear"))
        lblStatus.Text = objCBTrx.mtdGetCashBankStatus(Trim(objDataSet.Tables(0).Rows(0).Item("Status")))
        lblStsHid.Text = objDataSet.Tables(0).Rows(0).Item("Status")
        lblLastUpdate.Text = objGlobal.GetLongDate(Trim(objDataSet.Tables(0).Rows(0).Item("UpdateDate")))
        lblChequePrintDate.Text = objGlobal.GetLongDate(objDataSet.Tables(0).Rows(0).Item("ChequePrintDate"))
        lblPrintDate.Text = objGlobal.GetLongDate(objDataSet.Tables(0).Rows(0).Item("ChequePrintDate"))
        lblDateCreated.Text = objGlobal.GetLongDate(Trim(objDataSet.Tables(0).Rows(0).Item("CreateDate")))
        txtDateCreated.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), objDataSet.Tables(0).Rows(0).Item("CreateDate"))
        lblUpdatedBy.Text = Trim(objDataSet.Tables(0).Rows(0).Item("UserName"))
        hidUserID.Value = Trim(objDataSet.Tables(0).Rows(0).Item("UpdateID"))
        txtSplBankAccNo.Text = Trim(objDataSet.Tables(0).Rows(0).Item("SplBankAccNo"))
        txtRemark.Text = Trim(objDataSet.Tables(0).Rows(0).Item("Remark"))
        'ddlBankTo.SelectedValue = objDataSet.Tables(0).Rows(0).Item("BankTo")
        BindBankTo(Trim(objDataSet.Tables(0).Rows(0).Item("BankTo")))
        txtGiroDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), objDataSet.Tables(0).Rows(0).Item("PrintDate"))
        lblCurrentPeriod.Text = Trim(objDataSet.Tables(0).Rows(0).Item("AccYear")) & Trim(objDataSet.Tables(0).Rows(0).Item("AccMonth"))
        txtSupCode.Text = objDataSet.Tables(0).Rows(0).Item("Suppliercode")
        hidTaxStatus.Value = objDataSet.Tables(0).Rows(0).Item("TaxStatus")
        hidNPWPNo.Value = Trim(objDataSet.Tables(0).Rows(0).Item("NPWPNo"))
        If ddlGiroNo.SelectedItem.Value = "" Then
            BindGiroNo(objDataSet.Tables(0).Rows(0).Item("BankCode").Trim(), "")
        End If
        ddlChequeHandOver.SelectedValue = objDataSet.Tables(0).Rows(0).Item("ChequeHandOver").Trim()

        If Trim(objDataSet.Tables(0).Rows(0).Item("ChequeCash")) = "1" Then
            chkChequeCash.Checked = True
        Else
            chkChequeCash.Checked = False
        End If

        PrevCBID = Trim(objDataSet.Tables(0).Rows(0).Item("PrevCBID"))
        PrevPYID = Trim(objDataSet.Tables(0).Rows(0).Item("PrevPYID"))
        PrevCBDate = Day(objGlobal.GetLongDate(Trim(objDataSet.Tables(0).Rows(0).Item("PrevCBDate"))))
        PrevPYDate = Day(objGlobal.GetLongDate(Trim(objDataSet.Tables(0).Rows(0).Item("PrevPYDate"))))
        NextCBID = Trim(objDataSet.Tables(0).Rows(0).Item("NextCBID"))
        NextPYID = Trim(objDataSet.Tables(0).Rows(0).Item("NextPYID"))
        NextCBDate = Day(objGlobal.GetLongDate(Trim(objDataSet.Tables(0).Rows(0).Item("NextCBDate"))))
        NextPYDate = Day(objGlobal.GetLongDate(Trim(objDataSet.Tables(0).Rows(0).Item("NextPYDate"))))

        hidFFBSpl.Value = Trim(objDataSet.Tables(0).Rows(0).Item("SuppCat"))
        BindSplBankAccNo(Trim(objDataSet.Tables(0).Rows(0).Item("Suppliercode")), Trim(objDataSet.Tables(0).Rows(0).Item("BankAccNo")))
        If Trim(objDataSet.Tables(0).Rows(0).Item("TaxUpdateID")) <> "" Then
            lblTaxUpdate.Visible = True
            lblTaxUpdateDesc.Visible = True
            lblTaxUpdateDesc.Text = Trim(objDataSet.Tables(0).Rows(0).Item("TaxUpdateID"))
        Else
            lblTaxUpdate.Visible = False
            lblTaxUpdateDesc.Visible = False
        End If


        If PrevCBID <> "" Then
            If PrevPYID <> "" Then
                If PrevCBID > PrevPYID Then
                    hidPrevID.Value = PrevCBID
                    hidPrevDate.Value = PrevCBDate
                Else
                    hidPrevID.Value = PrevPYID
                    hidPrevDate.Value = PrevPYDate
                End If
            Else
                hidPrevID.Value = PrevCBID
                hidPrevDate.Value = PrevCBDate
            End If
        Else
            If PrevPYID <> "" Then
                hidPrevID.Value = PrevPYID
                hidPrevDate.Value = PrevPYDate
            End If
        End If
        If NextCBID <> "" Then
            If NextPYID <> "" Then
                If NextCBID < NextPYID Then
                    hidNextID.Value = NextCBID
                    hidNextDate.Value = NextCBDate
                Else
                    hidNextID.Value = NextPYID
                    hidNextDate.Value = NextPYDate
                End If
            Else
                hidNextID.Value = NextCBID
                hidNextDate.Value = NextCBDate
            End If
        Else
            If NextPYID <> "" Then
                hidNextID.Value = NextPYID
                hidNextDate.Value = NextPYDate
            End If
        End If

        FillDetail()
    End Sub

    Private Sub FillDetail()
        Dim intCnt As Integer
        Dim Total As Double
        Dim TotalCtrl As Double
        Dim DeleteButton As LinkButton
        Dim EditButton As LinkButton
        Dim strAccCode As String
        Dim lbl As Label
        Dim strCOA As String

        dgTx.DataSource = objDataSet.Tables(1)
        dgTx.DataBind()
        dgTx.Columns(2).Visible = Session("SS_INTER_ESTATE_CHARGING")

        Total = 0
        hidHadCOATax.Value = 0

        For intCnt = 0 To objDataSet.Tables(1).Rows.Count - 1
            If Sign(objDataSet.Tables(1).Rows(intCnt).Item("Total")) = 1 Then
                Total += objDataSet.Tables(1).Rows(intCnt).Item("Total")
            Else
                TotalCtrl += objDataSet.Tables(1).Rows(intCnt).Item("Total")
            End If

            Select Case lblStatus.Text
                Case objCBTrx.mtdGetCashBankStatus(objCBTrx.EnumCashBankStatus.Active)
                    DeleteButton = dgTx.Items.Item(intCnt).FindControl("lbDelete")
                    DeleteButton.Visible = True
                    DeleteButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                    EditButton = dgTx.Items.Item(intCnt).FindControl("lbEdit")
                    EditButton.Visible = True
                Case objCBTrx.mtdGetCashBankStatus(objCBTrx.EnumCashBankStatus.Verified)
                    lbl = dgTx.Items.Item(intCnt).FindControl("lblAccCode")
                    strCOA = lbl.Text.Trim
                    If intLevel = 0 Then
                        DeleteButton = dgTx.Items.Item(intCnt).FindControl("lbDelete")
                        DeleteButton.Visible = False
                        DeleteButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                        EditButton = dgTx.Items.Item(intCnt).FindControl("lbEdit")
                        EditButton.Visible = False
                        If Left(Trim(strCOA), 3) = "65." Or Left(Trim(strCOA), 5) = "71.19" Or Left(Trim(strCOA), 3) = "110" Then
                            DeleteButton = dgTx.Items.Item(intCnt).FindControl("lbDelete")
                            DeleteButton.Visible = True
                            DeleteButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                            EditButton = dgTx.Items.Item(intCnt).FindControl("lbEdit")
                            EditButton.Visible = True
                        End If
                    Else
                        DeleteButton = dgTx.Items.Item(intCnt).FindControl("lbDelete")
                        DeleteButton.Visible = True
                        DeleteButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                        EditButton = dgTx.Items.Item(intCnt).FindControl("lbEdit")
                        EditButton.Visible = True
                    End If
                Case objCBTrx.mtdGetCashBankStatus(objCBTrx.EnumCashBankStatus.Deleted), _
                     objCBTrx.mtdGetCashBankStatus(objCBTrx.EnumCashBankStatus.Closed), _
                     objCBTrx.mtdGetCashBankStatus(objCBTrx.EnumCashBankStatus.Confirmed)
                    DeleteButton = dgTx.Items.Item(intCnt).FindControl("lbDelete")
                    DeleteButton.Visible = False
                    EditButton = dgTx.Items.Item(intCnt).FindControl("lbEdit")
                    EditButton.Visible = False
            End Select

            'cek coa tax
            If Trim(objDataSet.Tables(1).Rows(intCnt).Item("TaxLnID")) <> "" Then
                hidHadCOATax.Value += 1
            End If
        Next intCnt

        'display coa status
        If hidHadCOATax.Value > 0 Then
            lblTaxStatus.Visible = True
            lblTaxStatusDesc.Visible = True
            lblTaxStatusDesc.Text = objCBTrx.mtdGetTaxStatus(Trim(hidTaxStatus.Value))
        Else
            lblTaxStatus.Visible = False
            lblTaxStatusDesc.Visible = False
        End If

        lblTotAmtFig.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(Total, 2), 2) 'objGlobal.GetIDDecimalSeparator(FormatNumber(Total, 2, True, False, False))
        lblCtrlAmtFig.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(Total + TotalCtrl, 2), 2) 'objGlobal.GetIDDecimalSeparator(FormatNumber(Total + TotalCtrl, 2, True, False, False))

        If objDataSet.Tables(1).Rows.Count > 0 Then
            TrLink.Visible = True
        Else
            TrLink.Visible = False
        End If
    End Sub

    Sub BindGrid()
        LoadTxDetails()
        FillDetail()
    End Sub

    Sub LoadTxDetails()
        strParam = Trim(lblPaymentID.Text)
        Try
            intErrNo = objCBTrx.mtdGetCashBankDetail(strOpCdTxLine_GET, _
                                                     strCompany, _
                                                     strLocation, _
                                                     strUserId, _
                                                     strAccMonth, _
                                                     strAccYear, _
                                                     strParam, objDataSet, True)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GET_CASHBANKLN&errmesg=" & lblErrMessage.Text & "&redirect=CB/Trx/cb_trx_CashBankList.aspx")
        End Try

        lblUpdatedBy.Text = Trim(objDataSet.Tables(0).Rows(0).Item("UserName"))
    End Sub

    Sub GetCOADetail(ByVal pv_strCode As String)
        'Dim dr As DataRow
        Dim intCnt As Integer = 0
        Dim intErrNo As Integer
        Dim intSelectedIndex As Integer = 0


        Dim strOpCode As String = "GL_CLSSETUP_ACCOUNTCODE_LIST_GET"
        Dim objCOADs As New DataSet
        Dim strParamName As String = ""
        Dim strParamValue As String = ""

        strParamName = "SEARCHSTR|SORTEXP"
        strParamValue = " And ACC.AccCode = '" & Trim(pv_strCode) & "'  " & "|Order By ACC.AccCode"

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
                                                strParamName, _
                                                strParamValue, _
                                                objCOADs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GENERATE_INVOICE_WM_GET_HEADER&errmesg=" & Exp.ToString() & "&redirect=ap/trx/ap_trx_invrcv_wm_list.aspx")
        End Try

        If objCOADs.Tables(0).Rows.Count > 0 Then
            txtAccName.Text = objCOADs.Tables(0).Rows(0).Item("Description")
            txtAccCode.Text = objCOADs.Tables(0).Rows(0).Item("AccCode")
        Else
            txtAccName.Text = ""
            txtAccCode.Text = ""
        End If
    End Sub


    'mark 2
    'selected change dropdown coa list
    Sub CallCheckVehicleUse(ByVal sender As Object, ByVal e As System.EventArgs)

        CheckVehicleUse()

        If Left(txtAccCode.Text, 3) = "110" Then
            If rblCashBankType.SelectedValue = objCBTrx.EnumCashBankType.Payment Then
                txtCRTotalAmount.Text = Replace(Replace(lblCtrlAmtFig.Text, ".", ""), ",", ".")
            Else
                txtDRTotalAmount.Text = Replace(Replace(lblCtrlAmtFig.Text, ".", ""), ",", ".")
            End If

            hidCOATax.Value = 0
        ElseIf ddlPayType.SelectedItem.Value = "4" Then
            If rblCashBankType.SelectedValue = objCBTrx.EnumCashBankType.Payment Then
                txtCRTotalAmount.Text = Replace(Replace(lblCtrlAmtFig.Text, ".", ""), ",", ".")
            Else
                txtDRTotalAmount.Text = Replace(Replace(lblCtrlAmtFig.Text, ".", ""), ",", ".")
            End If
        Else
            CheckCOATax()
        End If
        CheckCOATax()
        GetCOADetail(txtAccCode.Text.Trim)
    End Sub

    Sub CheckCOATax()
        Dim strParamName As String
        Dim strParamValue As String
        Dim objTaxDs As New Object
        Dim strOpCd As String = "GL_CLSTRX_TAXOBJECTRATE_LIST_GET" '"TX_CLSSETUP_TAXOBJECTRATE_LIST_GET"

        strParamName = "STRSEARCH"
        strParamValue = " AND TOB.AccCode = '" & Trim(txtAccCode.Text) & "' "

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCd, _
                                                strParamName, _
                                                strParamValue, _
                                                objTaxDs)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=TX_Setup_TaxObjectDet&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        If objTaxDs.Tables(0).Rows.Count > 0 Then
            RowTax.Visible = True
            RowTaxAmt.Visible = True
            hidCOATax.Value = 1
            BindTaxObjectList(txtAccCode.Text, "")

            If Trim(objTaxDs.Tables(0).Rows(0).Item("TaxID")) <> "" Then
                RowTax.Visible = True
                RowFP.Visible = False
                RowFPDate.Visible = False
                hidTaxPPN.Value = 0
            Else
                RowTax.Visible = False
                RowFP.Visible = True
                RowFPDate.Visible = True
                hidTaxPPN.Value = 1
            End If

            'If hidFFBSpl.Value <> "2" Then --awalnya supplier tbs owner tak ada pph
            '    RowTax.Visible = True
            '    RowTaxAmt.Visible = True
            '    hidCOATax.Value = 1
            '    BindTaxObjectList(txtAccCode.Text, "")
            'Else
            '    RowTax.Visible = False
            '    RowTaxAmt.Visible = False
            '    hidCOATax.Value = 0
            '    txtCRTotalAmount.ReadOnly = False
            '    txtDRTotalAmount.ReadOnly = False
            'End If
        Else
            RowTax.Visible = False
            RowTaxAmt.Visible = False
            hidCOATax.Value = 0
            RowFP.Visible = False
            RowFPDate.Visible = False
            hidTaxPPN.Value = 0
            txtCRTotalAmount.ReadOnly = False
            txtDRTotalAmount.ReadOnly = False
        End If

        'RangetxtAmtDR.Visible = False
        'RangetxtAmtCR.Visible = False
    End Sub

    Sub GetAccountDetails(ByVal pv_strAccCode As String, _
                          ByRef pr_strAccType As Integer, _
                          ByRef pr_strAccPurpose As Integer, _
                          ByRef pr_strNurseryInd As Integer, _
                          ByRef pr_blnFound As Boolean)

        Dim _objAccDs As New DataSet()
        Dim strOpCd As String = "GL_CLSSETUP_CHARTOFACCOUNT_GET_BY_ACCCODE"
        Dim strParam As String = pv_strAccCode
        Dim intErrNo As Integer

        Try
            intErrNo = objGLSetup.mtdGetAccount(strOpCd, _
                                                strParam, _
                                                _objAccDs, _
                                                True)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GET_ACCOUNT_DETAILS&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        If _objAccDs.Tables(0).Rows.Count = 1 Then
            pr_strAccType = Convert.ToInt16(_objAccDs.Tables(0).Rows(0).Item("AccType"))
            pr_strAccPurpose = Convert.ToInt16(_objAccDs.Tables(0).Rows(0).Item("AccPurpose"))
            pr_strNurseryInd = Convert.ToInt16(_objAccDs.Tables(0).Rows(0).Item("NurseryInd"))
            pr_blnFound = True
        Else
            pr_blnFound = False
            pr_strAccType = 0
            pr_strAccPurpose = 0
            pr_strNurseryInd = 0
        End If
    End Sub

    Sub CheckVehicleUse()
        Dim intAccType As Integer
        Dim intAccPurpose As Integer
        Dim intNurseryInd As Integer
        Dim blnFound As Boolean
        Dim strAcc As String = Request.Form("txtAccCode")
        Dim strBlk As String = Request.Form("lstBlock")
        Dim strPreBlk As String = Request.Form("ddlPreBlock")
        Dim strVeh As String = Request.Form("lstVehCode")
        Dim strVehExp As String = Request.Form("lstVehExp")

        GetAccountDetails(strAcc, intAccType, intAccPurpose, intNurseryInd, blnFound)

        If intAccType = objGLSetup.EnumAccountType.ProfitAndLost Then
            Select Case intAccPurpose
                Case objGLSetup.EnumAccountPurpose.NonVehicle
                    BindPreBlock(strAcc, strPreBlk)
                    BindBlockDropList(strAcc, strBlk)
                    BindVehicleCodeDropList("")
                    BindVehicleExpDropList(True)
                Case objGLSetup.EnumAccountPurpose.VehicleDistribution
                    BindPreBlock("", "")
                    BindBlockDropList("")
                    BindVehicleCodeDropList(strAcc, strVeh)
                    BindVehicleExpDropList(False, strVehExp)
                Case Else
                    BindPreBlock(strAcc, strPreBlk)
                    BindBlockDropList(strAcc, strBlk)
                    BindVehicleCodeDropList("%", strVeh)
                    BindVehicleExpDropList(False, strVehExp)
            End Select
        ElseIf intAccType = objGLSetup.EnumAccountType.BalanceSheet Or intNurseryInd = objGLSetup.EnumNurseryAccount.Yes Then
            'BindPreBlock(strAcc, strPreBlk)
            'BindBlockDropList(strAcc, strBlk)
            BindPreBlockBalanceSheet(strAcc, strPreBlk)
            BindBlockBalanceSheetDropList(strAcc, strPreBlk)
            BindVehicleCodeDropList("")
            BindVehicleExpDropList(True)
        Else
            BindPreBlock("", "")
            BindBlockDropList("", "")
            BindVehicleCodeDropList("")
            BindVehicleExpDropList(True)
        End If
    End Sub

    Sub BindPreBlockBalanceSheet(ByVal pv_strAccCode As String, ByVal pv_strBlkCode As String)
        Dim strOpCd As String
        Dim dr As DataRow
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intMasterType As Integer
        Dim intSelectedIndex As Integer = 0


        strOpCd = "GL_CLSSETUP_ACCOUNT_PREBLOCK_BALANCESHEET_GET"

        strParamName = "ACCCODE|LOCCODE|STATUS"
        strParamValue = pv_strAccCode & "|" & ddlLocation.SelectedItem.Value.Trim() & "|" & objGLSetup.EnumBlockStatus.Active
        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCd, _
                                            strParamName, _
                                            strParamValue, _
                                            objPODs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GL_CLSSETUP_ACCOUNT_BLOCK_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To objPODs.Tables(0).Rows.Count - 1
            objPODs.Tables(0).Rows(intCnt).Item("BlkCode") = Trim(objPODs.Tables(0).Rows(intCnt).Item("BlkCode"))
            objPODs.Tables(0).Rows(intCnt).Item("Description") = Trim(objPODs.Tables(0).Rows(intCnt).Item("BlkCode")) & " (" & Trim(objPODs.Tables(0).Rows(intCnt).Item("Description")) & ")"
            If objPODs.Tables(0).Rows(intCnt).Item("BlkCode") = Trim(pv_strBlkCode) Then
                intSelectedIndex = intCnt + 1
            End If
        Next

        If objPODs.Tables(0).Rows.Count = 1 Then
            intSelectedIndex = 1
        End If

        dr = objPODs.Tables(0).NewRow()
        dr("BlkCode") = ""
        dr("Description") = lblSelect.Text & PreBlockTag & lblCode.Text

        objPODs.Tables(0).Rows.InsertAt(dr, 0)
        ddlPreBlock.DataSource = objPODs.Tables(0)
        ddlPreBlock.DataValueField = "BlkCode"
        ddlPreBlock.DataTextField = "Description"
        ddlPreBlock.DataBind()
        ddlPreBlock.SelectedIndex = intSelectedIndex

        If Not objPODs Is Nothing Then
            objPODs = Nothing
        End If
    End Sub

    Sub BindBlockBalanceSheetDropList(ByVal pv_strAccCode As String, Optional ByVal pv_strBlkCode As String = "")
        Dim dsForDropDown As DataSet
        Dim intSelectedIndex As Integer = 0
        Dim intCnt As Integer
        Dim strOpCdBlockList_Get As String
        Dim dr As DataRow

        Try

            strOpCdBlockList_Get = "GL_CLSSETUP_ACCOUNT_SUBBLOCK_BALANCE_SHEET_GET"

            strParamName = "ACCCODE|LOCCODE|STATUS"
            strParamValue = pv_strAccCode & "|" & ddlLocation.SelectedItem.Value.Trim() & "|" & objGLSetup.EnumBlockStatus.Active

            intErrNo = objGLtrx.mtdGetDataCommon(strOpCdBlockList_Get, _
                                            strParamName, _
                                            strParamValue, _
                                            objPODs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GL_CLSSETUP_ACCOUNT_BLOCK_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To objPODs.Tables(0).Rows.Count - 1
            objPODs.Tables(0).Rows(intCnt).Item("BlkCode") = Trim(objPODs.Tables(0).Rows(intCnt).Item("BlkCode"))
            objPODs.Tables(0).Rows(intCnt).Item("Description") = Trim(objPODs.Tables(0).Rows(intCnt).Item("BlkCode")) & " (" & Trim(objPODs.Tables(0).Rows(intCnt).Item("Description")) & ")"
            If objPODs.Tables(0).Rows(intCnt).Item("BlkCode") = Trim(pv_strBlkCode) Then
                intSelectedIndex = intCnt + 1
            End If
        Next

        If objPODs.Tables(0).Rows.Count = 1 Then
            intSelectedIndex = 1
        End If

        dr = objPODs.Tables(0).NewRow()
        dr("BlkCode") = ""
        dr("Description") = lblSelect.Text & strBlockTag & lblCode.Text
        objPODs.Tables(0).Rows.InsertAt(dr, 0)

        lstBlock.DataSource = objPODs.Tables(0)
        lstBlock.DataValueField = "BlkCode"
        lstBlock.DataTextField = "Description"
        lstBlock.DataBind()
        lstBlock.SelectedIndex = intSelectedIndex

        If Not objPODs Is Nothing Then
            objPODs = Nothing
        End If
    End Sub

    Sub onLoad_Button()
        Dim intStatus As Integer
        Dim strDate As String = Date_Validation(txtDateCreated.Text, False)
        Dim strBank As String
        Dim strBankAccCode As String
        Dim arrParam As Array

        If ddlBank.SelectedItem.Value = "" Then
            txtAccCode.Text = ""
            Exit Sub
        Else
            arrParam = Split(Trim(ddlBank.SelectedItem.Value), "|")
            strBank = Trim(arrParam(0))
            strBankAccCode = Trim(arrParam(1))
        End If

        ddlPayType.Enabled = False
        ddlBank.Enabled = False
        txtChequeNo.Enabled = False
        tblSelection.Visible = False
        PrintChequeBtn.Visible = False
        rblCashBankType.Enabled =True ''False
        txtPaymentTo.Enabled = False
        NewCBBtn.Visible = False
        ForwardBtn.Visible = False
        ConfirmBtn.Visible = False
        DeleteBtn.Visible = False
        UnDeleteBtn.Visible = False
        lblDateCreated.Visible = False
        txtDateCreated.Enabled = False
        btnDateCreated.Visible = False
        ddlBankTo.Enabled = False
        PrintBtn.Visible = False
        PrintSlipBtn.Visible = False
        PrintTransferBtn.Visible = False
        PrintKwitansiBtn.Visible = False
        txtRemark.Enabled = False
        btnGetRef.Visible = False
        CancelBtn.Visible = False
        txtGiroDate.Enabled = False
        btnGiroDate.Visible = False
        txtChequeNo.Enabled = False
        txtSplBankAccNo.Enabled = False
        ddlBank.Enabled = False
        ddlBankTo.Enabled = False
        EditBtn.Visible = False
        SaveBtn.Visible = False
        VerifiedBtn.Visible = False
        FindSpl.Disabled = True
        btnGet.Visible = False

        If (lblStsHid.Text <> "") Then
            intStatus = Convert.ToInt16(lblStsHid.Text)
            Select Case intStatus
                Case objCBTrx.EnumCashBankStatus.Active, objCBTrx.EnumCashBankStatus.Verified
                    ddlPayType.Enabled = True
                    txtSplBankAccNo.Enabled = False
                    ddlBank.Enabled = True
                    txtChequeNo.Enabled = True
                    ddlBankTo.Enabled = True
                    'btnGet.Visible = True
                    tblSelection.Visible = True
                    SaveBtn.Visible = True
                    ConfirmBtn.Visible = True
                    DeleteBtn.Visible = True
                    PrintBtn.Visible = True
                    NewCBBtn.Visible = True
                    txtRemark.Enabled = True
                    txtGiroDate.Enabled = True
                    btnGiroDate.Visible = True
                    txtDateCreated.Enabled = True
                    btnDateCreated.Visible = True
                    If Left(lblPaymentID.Text.Trim, 3) = "XXX" Then
                        ForwardBtn.Visible = True
                    Else
                        ForwardBtn.Visible = False
                    End If

                    Select Case intLevel
                        Case 0 'user finance
                            If UCase(Trim(strUserId)) <> UCase(Trim(hidUserID.Value)) Then
                                VerifiedBtn.Visible = False
                                ConfirmBtn.Visible = False
                                txtPaymentTo.Enabled = True
                                FindSpl.Disabled = False
                                DeleteBtn.Visible = False
                                UnDeleteBtn.Visible = False
                            Else
                                VerifiedBtn.Visible = False
                                ConfirmBtn.Visible = False
                                txtPaymentTo.Enabled = True
                                FindSpl.Disabled = False
                                DeleteBtn.Visible = True
                                UnDeleteBtn.Visible = False
                            End If
                        Case 1 'user accounting
                            VerifiedBtn.Visible = False
                            ConfirmBtn.Visible = False
                            txtPaymentTo.Enabled = True
                            FindSpl.Disabled = False
                        Case Else 'supervisor akunting and up
                            VerifiedBtn.Visible = True
                            If Left(lblPaymentID.Text.Trim, 3) = "XXX" Then
                                ConfirmBtn.Visible = False
                            Else
                                ConfirmBtn.Visible = True
                            End If
                            txtPaymentTo.Enabled = True
                            FindSpl.Disabled = False
                    End Select

                    'If intLevel = 0 Then
                    '    VerifiedBtn.Visible = False
                    '    ConfirmBtn.Visible = False
                    '    'ForwardBtn.Visible = False
                    '    txtPaymentTo.Enabled = True
                    '    FindSpl.Disabled = False
                    '    DeleteBtn.Visible = False
                    '    UnDeleteBtn.Visible = False
                    'Else
                    '    VerifiedBtn.Visible = True
                    '    ConfirmBtn.Visible = True
                    '    'ForwardBtn.Visible = True
                    '    txtPaymentTo.Enabled = True
                    '    FindSpl.Disabled = False
                    'End If

                    If Month(strDate) <= Mid(lblCurrentPeriod.Text, 5) And Year(strDate) = Mid(lblCurrentPeriod.Text, 1, 4) Then
                        txtDateCreated.Enabled = True
                        btnDateCreated.Visible = True
                    Else
                        txtDateCreated.Enabled = False
                        btnDateCreated.Visible = False
                    End If

                    'temporary ?
                    If Convert.ToInt16(lblPayType.Text) <> objCBTrx.EnumPaymentType.OtherParty _
                        And (rblCashBankType.SelectedValue = objCBTrx.EnumCashBankType.Payment Or rblCashBankType.SelectedValue = objCBTrx.EnumCashBankType.Transfer) Then
                        SaveBtn.Visible = True
                        'If ddlBank.SelectedItem.Value = ddlBankTo.SelectedItem.Value Or ddlBankTo.SelectedItem.Value = "" Then
                        If Trim(strBank) = Trim(ddlBankTo.SelectedItem.Value) Or Trim(ddlBankTo.SelectedItem.Value) = "" Then
                            PrintSlipBtn.Visible = True
                            PrintTransferBtn.Visible = False
                        Else
                            PrintSlipBtn.Visible = False
                            PrintTransferBtn.Visible = True
                        End If

                        PrintChequeBtn.Visible = True
                        txtChequeNo.Enabled = True
                    Else
                        PrintKwitansiBtn.Visible = True
                        SaveBtn.Visible = True
                    End If

                Case objCBTrx.EnumCashBankStatus.Deleted
                    If intLevel = 0 Then
                        If UCase(Trim(strUserId)) <> UCase(Trim(hidUserID.Value)) Then
                            UnDeleteBtn.Visible = False
                        Else
                            UnDeleteBtn.Visible = True
                        End If
                    Else
                        UnDeleteBtn.Visible = True
                    End If
                    PrintBtn.Visible = False
                    SaveBtn.Visible = False
                    NewCBBtn.Visible = True

                Case objCBTrx.EnumCashBankStatus.Closed
                    PrintBtn.Visible = True
                    SaveBtn.Visible = False
                    NewCBBtn.Visible = True

                Case objCBTrx.EnumCashBankStatus.Cancelled
                    PrintBtn.Visible = False
                    SaveBtn.Visible = False
                    NewCBBtn.Visible = True

                Case objCBTrx.EnumCashBankStatus.Confirmed
                    If Convert.ToInt16(lblPayType.Text) <> objCBTrx.EnumPaymentType.OtherParty _
                        And rblCashBankType.SelectedValue = objCBTrx.EnumCashBankType.Payment Then
                        'If ddlBank.SelectedItem.Value = ddlBankTo.SelectedItem.Value Or ddlBankTo.SelectedItem.Value = "" Then
                        If Trim(strBank) = Trim(ddlBankTo.SelectedItem.Value) Or Trim(ddlBankTo.SelectedItem.Value) = "" Then
                            PrintSlipBtn.Visible = True
                            PrintTransferBtn.Visible = False
                        Else
                            PrintSlipBtn.Visible = False
                            PrintTransferBtn.Visible = True
                        End If

                        PrintChequeBtn.Visible = True
                        txtChequeNo.Enabled = True
                    Else
                        PrintKwitansiBtn.Visible = True
                        SaveBtn.Visible = False
                    End If
                    PrintBtn.Visible = True
                    NewCBBtn.Visible = True
                    CancelBtn.Visible = True

                    If intLevel = 0 Then
                        CancelBtn.Visible = False
						EditBtn.Visible = False
                    Else
                        CancelBtn.Visible = True
						EditBtn.Visible = True
                    End If
            End Select
            lblDateCreated.Visible = True
        Else
            ddlPayType.Enabled = True
            ddlBank.Enabled = True
            txtChequeNo.Enabled = True
            txtSplBankAccNo.Enabled = False
            ddlBankTo.Enabled = True
            'btnGet.Visible = True
            tblSelection.Visible = True
            rblCashBankType.Enabled = True
            txtPaymentTo.Enabled = True
            SaveBtn.Visible = True
            ConfirmBtn.Visible = False
            PrintBtn.Visible = False
            NewCBBtn.Visible = False
            btnDateCreated.Visible = True
            txtDateCreated.Enabled = True
            txtRemark.Enabled = True
            PrintSlipBtn.Visible = False
            PrintTransferBtn.Visible = False
            txtGiroDate.Enabled = True
            btnGiroDate.Visible = True
            btnDateCreated.Visible = True
            FindSpl.Disabled = False
        End If

        DeleteBtn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
        UnDeleteBtn.Attributes("onclick") = "javascript:return ConfirmAction('undelete');"
        ForwardBtn.Attributes("onclick") = "javascript:return ConfirmAction('move forward this transaction into next period');"

        Select Case Trim(Replace(Replace(Replace(lblCtrlAmtFig.Text, ".", ""), ",", "."), "-", ""))
            Case "0.00", "0,00", "0", ""
                BackBtn.Attributes("onclick") = ""
                NewCBBtn.Attributes("onclick") = ""
                ForwardBtn.Attributes("onclick") = ""
                SaveBtn.Attributes("onclick") = ""
            Case Else
                BackBtn.Attributes("onclick") = "javascript:return ConfirmAction('exit while amount in current transaction is not balance. Difference = " & Trim(lblCtrlAmtFig.Text) & " ');"
                If Left(Trim(lblPaymentID.Text), 3) <> "XXX" Then
                    SaveBtn.Attributes("onclick") = "javascript:return ConfirmAction('save while amount in current transaction is not balance. Difference = " & Trim(lblCtrlAmtFig.Text) & " ');"
                End If
                NewCBBtn.Attributes("onclick") = "javascript:return ConfirmAction('create new transaction while amount in current transaction is not balance. Difference = " & Trim(lblCtrlAmtFig.Text) & " ');"
                ForwardBtn.Attributes("onclick") = "javascript:return ConfirmAction('move forward this transaction while amount is not balance. Difference = " & Trim(lblCtrlAmtFig.Text) & " ');"
                'Select Case Trim(Replace(Replace(Replace(lblTotAmtFig.Text, ".", ""), ",", "."), "-", ""))
                '    Case "0.00", "0,00", "0"
                '        BackBtn.Attributes("onclick") = "javascript:return ConfirmAction('exit while amount in current transaction is not balance');"
                '        NewCBBtn.Attributes("onclick") = "javascript:return ConfirmAction('create new transaction while amount in current transaction is not balance');"
                'End Select
        End Select


        'Dim strNowDate As String = Date_Validation(Date_Validation(Now(), True), False)
        'If lblPaymentID.Text <> "" And Left(lblPaymentID.Text.Trim, 3) <> "XXX" Then
        '    If Year(strDate) & Format(Month(strDate), "00") & Format(Day(strDate), "00") < Year(strNowDate) & Format(Month(strNowDate), "00") & Format(Day(strNowDate), "00") Then
        '        txtDateCreated.Enabled = False
        '    End If
        'End If

        If lblPaymentID.Text.Trim() = "" Then
            'txtDateCreated.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
        End If
    End Sub

    Sub BindPaymentMethod()
        rblCashBankType.Items.Add(New ListItem(objCBTrx.mtdGetCashBankType(objCBTrx.EnumCashBankType.Payment), objCBTrx.EnumCashBankType.Payment))
        rblCashBankType.Items.Add(New ListItem(objCBTrx.mtdGetCashBankType(objCBTrx.EnumCashBankType.Receipt), objCBTrx.EnumCashBankType.Receipt))
        rblCashBankType.Items.Add(New ListItem(objCBTrx.mtdGetCashBankType(objCBTrx.EnumCashBankType.Transfer), objCBTrx.EnumCashBankType.Transfer))
        rblCashBankType.SelectedIndex = 0
    End Sub

    Sub BindPreBlock(ByVal pv_strAccCode As String, ByVal pv_strBlkCode As String)
        Dim strOpCd As String
        Dim dr As DataRow
        Dim objBlkDs As DataSet
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intMasterType As Integer
        Dim intSelectedIndex As Integer = 0

        strOpCd = "GL_CLSSETUP_ACCOUNT_PREBLOCK_GET"
        intSelectedIndex = 0
        Try
            strParam = pv_strAccCode & "|" & ddlLocation.SelectedItem.Value.Trim() & "|" & objGLSetup.EnumBlockStatus.Active
            intErrNo = objGLSetup.mtdGetAccountBlock(strOpCd, _
                                                     strParam, _
                                                     objBlkDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GL_CLSSETUP_ACCOUNT_BLOCK_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        If objBlkDs.Tables(0).Rows.Count = 1 Then
            intSelectedIndex = 1
        End If

        dr = objBlkDs.Tables(0).NewRow()
        dr("BlkCode") = ""
        dr("_Description") = lblPleaseSelect.Text & PreBlockTag & lblCode.Text

        objBlkDs.Tables(0).Rows.InsertAt(dr, 0)
        ddlPreBlock.DataSource = objBlkDs.Tables(0)
        ddlPreBlock.DataValueField = "BlkCode"
        ddlPreBlock.DataTextField = "_Description"
        ddlPreBlock.DataBind()
        ddlPreBlock.SelectedIndex = intSelectedIndex

        If Not objBlkDs Is Nothing Then
            objBlkDs = Nothing
        End If
    End Sub

    Sub BindBlockDropList(ByVal pv_strAccCode As String, Optional ByVal pv_strBlkCode As String = "")
        Dim dsForDropDown As DataSet
        Dim intSelectedIndex As Integer = 0
        Dim intCnt As Integer
        Dim strOpCdBlockList_Get As String
        Dim strParam As String
        Dim dr As DataRow

        Try
            If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
                strOpCdBlockList_Get = "GL_CLSSETUP_ACCOUNT_BLOCK_GET"
                strParam = pv_strAccCode & "|" & ddlLocation.SelectedItem.Value.Trim() & "|" & objGLSetup.EnumBlockStatus.Active
            Else
                strOpCdBlockList_Get = "GL_CLSSETUP_ACCOUNT_SUBBLOCK_GET"
                strParam = pv_strAccCode & "|" & ddlLocation.SelectedItem.Value.Trim() & "|" & objGLSetup.EnumSubBlockStatus.Active
            End If
            intErrNo = objGLSetup.mtdGetAccountBlock(strOpCdBlockList_Get, _
                                                     strParam, _
                                                     dsForDropDown)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GL_CLSSETUP_ACCOUNT_BLOCK_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        If dsForDropDown.Tables(0).Rows.Count = 1 Then
            intSelectedIndex = 1
        End If

        dr = dsForDropDown.Tables(0).NewRow()
        dr("BlkCode") = ""
        dr("_Description") = lblSelect.Text & strBlockTag
        dsForDropDown.Tables(0).Rows.InsertAt(dr, 0)
        lstBlock.DataSource = dsForDropDown.Tables(0)
        lstBlock.DataValueField = "BlkCode"
        lstBlock.DataTextField = "_Description"
        lstBlock.DataBind()
        lstBlock.SelectedIndex = intSelectedIndex

        If Not dsForDropDown Is Nothing Then
            dsForDropDown = Nothing
        End If
    End Sub

    'mark 1
    Sub BindAccCodeDropList(Optional ByVal pv_strAccCode As String = "")
        Dim strOpCd As String = "GL_CLSSETUP_ACCOUNTCODE_LIST_GET"
        Dim dr As DataRow
        'Dim strParam As String = "Order By ACC.AccCode|And ACC.Status = '" & objGLSetup.EnumAccountCodeStatus.Active & "'"
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer = 0
        Dim dsForDropDown As DataSet

        If LblIsSKBActive.Text = 1 And (Format(txtDateCreated.Text, "dd/MM/yyyy") >= Format(lblSKBStartDate.Text, "dd/MM/yyyy")) Then
            strParam = "Order By ACC.AccCode|And ACC.Status = '" & objGLSetup.EnumAccountCodeStatus.Active & "' AND ACC.AccCode NOT IN (SELECT AccCode FROM dbo.TX_TAXOBJECTRATELN) "
        Else
            strParam = "Order By ACC.AccCode|And ACC.Status = '" & objGLSetup.EnumAccountCodeStatus.Active & "' "
        End If

        strParam = strParam & IIf(Session("SS_COACENTRALIZED") = "1", "", " AND LocCode = '" & Trim(strLocation) & "' ")

        Try
            intErrNo = objGLSetup.mtdGetMasterList(strOpCd, _
                                                   strParam, _
                                                   objGLSetup.EnumGLMasterType.AccountCode, _
                                                   dsForDropDown)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BindAccCodeDropList&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        dr = dsForDropDown.Tables(0).NewRow()
        dr("AccCode") = ""
        dr("_Description") = lblSelect.Text & strAccountTag
        dsForDropDown.Tables(0).Rows.InsertAt(dr, 0)

        'lstAccCode.DataSource = dsForDropDown.Tables(0)
        'lstAccCode.DataValueField = "AccCode"
        'lstAccCode.DataTextField = "_Description"
        'lstAccCode.DataBind()
        'lstAccCode.SelectedIndex = intSelectedIndex

        If Not dsForDropDown Is Nothing Then
            dsForDropDown = Nothing
        End If
    End Sub

    Sub BindVehicleCodeDropList(ByVal pv_strAccCode As String, Optional ByVal pv_strVehCode As String = "")
        Dim dsForDropDown As DataSet
        Dim strOpCd As String
        Dim drinsert As DataRow
        Dim strParam As New StringBuilder()
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intMasterType As Integer
        Dim intSelectedIndex As Integer = 0

        strParam.Append("|LocCode = '" & ddlLocation.SelectedItem.Value.Trim() & "' AND Status = '" & objGLSetup.EnumVehicleStatus.Active & "'")
        Try
            strOpCd = "GL_CLSSETUP_VEH_LIST_GET"
            intErrNo = objGLSetup.mtdGetMasterList(strOpCd, _
                                                   strParam.ToString, _
                                                   objGLSetup.EnumGLMasterType.Vehicle, _
                                                   dsForDropDown)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADD_ITEMTODROPDOWN&errmesg=" & lblErrMessage.Text & "&redirect=CB/Trx/cb_trx_CashBankList.aspx")
        End Try

        drinsert = dsForDropDown.Tables(0).NewRow()
        drinsert("VehCode") = ""
        drinsert("_Description") = lblSelect.Text & strVehTag
        dsForDropDown.Tables(0).Rows.InsertAt(drinsert, 0)

        lstVehCode.DataSource = dsForDropDown.Tables(0)
        lstVehCode.DataValueField = "VehCode"
        lstVehCode.DataTextField = "_Description"
        lstVehCode.DataBind()
        lstVehCode.SelectedIndex = intSelectedIndex

        If Not dsForDropDown Is Nothing Then
            dsForDropDown = Nothing
        End If
    End Sub

    Sub BindVehicleExpDropList(ByVal pv_IsBlankList As Boolean, Optional ByVal pv_strVehExpCode As String = "")

        Dim dsForDropDown As DataSet
        Dim strOpCd As String = "GL_CLSSETUP_VEHEXPENSE_LIST_GET"
        Dim drinsert As DataRow
        Dim strParam As String = "Order By VehExpenseCode ASC|And Veh.Status = '" & objGLSetup.EnumVehicleExpenseStatus.Active & "'"
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer = 0

        Try
            If pv_IsBlankList Then
                strParam += " And Veh.VehExpensecode = ''"
            End If
            intErrNo = objGLSetup.mtdGetMasterList(strOpCd, _
                                                   strParam, _
                                                   objGLSetup.EnumGLMasterType.VehicleExpense, _
                                                   dsForDropDown)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=JR_TRX_ADTRX_VEHEXPENSE_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To dsForDropDown.Tables(0).Rows.Count - 1
            dsForDropDown.Tables(0).Rows(intCnt).Item(0) = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(0))
            dsForDropDown.Tables(0).Rows(intCnt).Item(1) = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(0)) & " ( " & _
                                                          Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(1)) & " )"
            If dsForDropDown.Tables(0).Rows(intCnt).Item("VehExpenseCode") = Trim(pv_strVehExpCode) Then
                intSelectedIndex = intCnt + 1
            End If
        Next intCnt
        drinsert = dsForDropDown.Tables(0).NewRow()
        drinsert(0) = ""
        drinsert(1) = lblSelect.Text & strVehExpTag
        dsForDropDown.Tables(0).Rows.InsertAt(drinsert, 0)

        lstVehExp.DataSource = dsForDropDown.Tables(0)
        lstVehExp.DataValueField = "VehExpenseCode"
        lstVehExp.DataTextField = "Description"
        lstVehExp.DataBind()
        lstVehExp.SelectedIndex = intSelectedIndex

        If Not dsForDropDown Is Nothing Then
            dsForDropDown = Nothing
        End If
    End Sub

    Sub BindBankCode(ByVal pv_strBankCode As String)
        Dim strOpCode As String = "HR_CLSSETUP_BANK_LIST_GET"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim objBankDs As New DataSet()
        Dim intSelectedBankIndex As Integer = 0
        Dim strDate As String = Date_Validation(txtDateCreated.Text, False)
        Dim strOpCodeBank As String

        'strParam = "| And AccCode IN (SELECT AccCode FROM GL_ACCOUNT WHERE COALevel='2' AND AccType='1' AND AccCode NOT LIKE 'DUMMY%') "
        'strParam = strParam & IIf(Session("SS_COACENTRALIZED") = "1", "", " And A.AccCode in (SELECT AccCode FROM GL_Account WHERE LocCode='" & Trim(strLocation) & "') ")
        'strParam = strParam & " And AccCode IN (SELECT AccCode FROM SH_LOCATION_BANK WHERE LocCode='" & Trim(strLocation) & "')"

        'Try
        '    intErrNo = objHRSetup.mtdGetMasterList(strOpCode, _
        '                                           strParam, _
        '                                           objHRSetup.EnumHRMasterType.Bank, _
        '                                           objBankDs)
        'Catch Exp As Exception
        '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_GET_BANK&errmesg=" & Exp.ToString() & "&redirect=CB/Trx/cb_trx_CashBankList.aspx")
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
                                                objBankDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ID_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To objBankDs.Tables(0).Rows.Count - 1
            If Trim(objBankDs.Tables(0).Rows(intCnt).Item("BankCode")) = Trim(pv_strBankCode) Then
                intSelectedBankIndex = intCnt + 1
                Exit For
            End If
        Next intCnt


        Dim dr As DataRow
        dr = objBankDs.Tables(0).NewRow()
        dr("BankCode") = ""
        dr("_Description") = lblPleaseSelect.Text & " Bank" & lblCode.Text
        objBankDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlBank.DataSource = objBankDs.Tables(0)
        ddlBank.DataValueField = "BankCode"
        ddlBank.DataTextField = "_Description"
        ddlBank.DataBind()
        ddlBank.SelectedIndex = intSelectedBankIndex


        strParam = "|"


        Try
            intErrNo = objHRSetup.mtdGetMasterList(strOpCode, _
                                                   strParam, _
                                                   objHRSetup.EnumHRMasterType.Bank, _
                                                   objBankDs)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_GET_BANK&errmesg=" & Exp.ToString() & "&redirect=CB/Trx/cb_trx_CashBankList.aspx")
        End Try

        For intCnt = 0 To objBankDs.Tables(0).Rows.Count - 1
            If (objBankDs.Tables(0).Rows(intCnt).Item("BankCode") = Trim(pv_strBankCode)) Then
                intSelectedBankIndex = intCnt + 1
                Exit For
            End If
        Next intCnt

        dr = objBankDs.Tables(0).NewRow()
        dr("BankCode") = ""
        dr("_Description") = lblPleaseSelect.Text & " Bank" & lblCode.Text
        objBankDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlBankTo.DataSource = objBankDs.Tables(0)
        ddlBankTo.DataValueField = "BankCode"
        ddlBankTo.DataTextField = "_Description"
        ddlBankTo.DataBind()
        ddlBankTo.SelectedIndex = intSelectedBankIndex

    End Sub

    Sub BindBankTo(ByVal pv_strBankCode As String)
        Dim strOpCode As String = "HR_CLSSETUP_BANK_LIST_GET"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim objBankDs As New DataSet()
        Dim intSelectedBankIndex As Integer = 0

        strParam = "|"

        Try
            intErrNo = objHRSetup.mtdGetMasterList(strOpCode, _
                                                   strParam, _
                                                   objHRSetup.EnumHRMasterType.Bank, _
                                                   objBankDs)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_GET_BANK&errmesg=" & Exp.ToString() & "&redirect=CB/Trx/cb_trx_CashBankList.aspx")
        End Try

        For intCnt = 0 To objBankDs.Tables(0).Rows.Count - 1
            If (objBankDs.Tables(0).Rows(intCnt).Item("BankCode") = Trim(pv_strBankCode)) Then
                intSelectedBankIndex = intCnt + 1
                Exit For
            End If
        Next intCnt

        Dim dr As DataRow
        dr = objBankDs.Tables(0).NewRow()
        dr("BankCode") = ""
        dr("_Description") = lblPleaseSelect.Text & " Bank" & lblCode.Text
        objBankDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlBankTo.DataSource = objBankDs.Tables(0)
        ddlBankTo.DataValueField = "BankCode"
        ddlBankTo.DataTextField = "_Description"
        ddlBankTo.DataBind()
        ddlBankTo.SelectedIndex = intSelectedBankIndex

    End Sub

    Sub BindLocationDropDownList(ByVal pv_strLocCode As String)
        Dim strOpCd As String
        Dim dr As DataRow
        Dim dsLoc As DataSet
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer = 0

        strOpCd = "ADMIN_CLSLOC_INTER_ESTATE_LOCATION_GET"
        intSelectedIndex = 0
        Try
            strParam = objAdminLoc.EnumLocStatus.Active & "|" & _
                       Trim(Session("SS_COMPANY")) & "|" & _
                       Trim(Session("SS_LOCATION")) & "|" & _
                       Trim(Session("SS_USERID")) & "|" & _
                       objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.GeneralLedger) & "|" & _
                       "GLAR" & "|" & _
                       objAR.mtdGetAccessRights(objAR.EnumGLAccessRights.GLJournal) & "|" & _
                       "GLAR" & "|" & _
                       objAR.mtdGetAccessRights(objAR.EnumGLAccessRights.GLJournal)
            intErrNo = objAdminLoc.mtdGetInterEstateLoc(strOpCd, _
                                                        strParam, _
                                                        dsLoc)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GL_CLSSETUP_ACCOUNT_BLOCK_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To dsLoc.Tables(0).Rows.Count - 1
            dsLoc.Tables(0).Rows(intCnt).Item("LocCode") = Trim(dsLoc.Tables(0).Rows(intCnt).Item("LocCode"))
            dsLoc.Tables(0).Rows(intCnt).Item("Description") = Trim(dsLoc.Tables(0).Rows(intCnt).Item("Description"))
            If dsLoc.Tables(0).Rows(intCnt).Item("LocCode") = Trim(pv_strLocCode) Then
                intSelectedIndex = intCnt + 1
            End If
        Next

        If Trim(pv_strLocCode) <> "" And intSelectedIndex = 0 Then
            dr = dsLoc.Tables(0).NewRow()
            dr("LocCode") = Trim(pv_strLocCode)
            dr("Description") = Trim(pv_strLocCode) & " (Deleted)"
            dsLoc.Tables(0).Rows.InsertAt(dr, 0)
            intSelectedIndex = 1
        End If

        dr = dsLoc.Tables(0).NewRow()
        dr("LocCode") = ""
        dr("Description") = lblSelect.Text & GetCaption(objLangCap.EnumLangCap.Location) & lblCode.Text

        dsLoc.Tables(0).Rows.InsertAt(dr, 0)
        ddlLocation.DataSource = dsLoc.Tables(0)
        ddlLocation.DataValueField = "LocCode"
        ddlLocation.DataTextField = "Description"
        ddlLocation.DataBind()
        ddlLocation.SelectedIndex = intSelectedIndex

        If Not dsLoc Is Nothing Then
            dsLoc = Nothing
        End If
    End Sub

    Sub onSelect_Bank(ByVal Sender As System.Object, ByVal E As System.EventArgs)
        Dim intStatus As Integer
        Dim defaultAccCode As String = ""
        Dim strOpCode As String = "HR_CLSSETUP_BANK_ACCCODE_GET"
        Dim strDate As String = Date_Validation(txtDateCreated.Text, False)
        Dim strOpCodeBank As String
        Dim strBank As String
        Dim strBankAccCode As String
        Dim arrParam As Array

        If ddlBank.SelectedItem.Value = "" Then
            txtAccCode.Text = ""
            Exit Sub
        Else
            arrParam = Split(Trim(ddlBank.SelectedItem.Value), "|")
            strBank = Trim(arrParam(0))
            strBankAccCode = Trim(arrParam(1))
        End If

        'Try
        '    strParam = ddlBank.SelectedItem.Value
        '    intErrNo = objHRSetup.mtdGetAccCode(strOpCode, strParam, objAccDs)
        'Catch Exp As Exception
        '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_CASHBANK_GET_BANKACCCODE&errmesg=" & Exp.ToString() & "&redirect=CB/Trx/cb_trx_CashBankList.aspx")
        'End Try
        'If objAccDs.Tables(0).Rows.Count > 0 Then
        '    defaultAccCode = Trim(objAccDs.Tables(0).Rows(0).Item("AccCode"))
        'End If

        'objAccDs = Nothing

        'strOpCode = "GL_CLSSETUP_ACCOUNTCODE_LIST_GET"
        'strParam = "Order By ACC.AccCode|And ACC.Status = '" & objGLSetup.EnumAccountCodeStatus.Active & "' AND ACC.AccCode Like '" & Trim(defaultAccCode) & "%'"
        'strParam = strParam & IIf(Session("SS_COACENTRALIZED") = "1", "", " AND LocCode = '" & Trim(strLocation) & "' ")

        'Try
        '    intErrNo = objGLSetup.mtdGetMasterList(strOpCode, _
        '                                           strParam, _
        '                                           objGLSetup.EnumGLMasterType.AccountCode, _
        '                                           objAccDs)
        'Catch Exp As System.Exception
        '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BindAccCodeDropList&errmesg=" & lblErrMessage.Text & "&redirect=")
        'End Try

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
            txtAccCode.Text = Trim(objAccDs.Tables(0).Rows(0).Item("AccCode"))
        End If

        If Left(txtAccCode.Text, 3) = "110" Or Left(txtAccCode.Text, 6) = "112.01" Then
            If rblCashBankType.SelectedValue = objCBTrx.EnumCashBankType.Payment Or rblCashBankType.SelectedValue = objCBTrx.EnumCashBankType.Transfer Then
                txtCRTotalAmount.Text = Replace(Replace(Replace(lblCtrlAmtFig.Text, ".", ""), ",", "."), "-", "")
                Select Case txtCRTotalAmount.Text
                    Case "0.00", "0,00", "0", ""
                        txtCRTotalAmount.Text = ""
                    Case Else
                        txtCRTotalAmount.Text = txtCRTotalAmount.Text
                End Select
            Else
                txtDRTotalAmount.Text = Replace(Replace(Replace(lblCtrlAmtFig.Text, ".", ""), ",", "."), "-", "")
                Select Case txtDRTotalAmount.Text
                    Case "0.00", "0,00", "0", ""
                        txtDRTotalAmount.Text = ""
                    Case Else
                        txtDRTotalAmount.Text = txtDRTotalAmount.Text
                End Select
            End If

        Else
            txtCRTotalAmount.Text = ""
            txtDRTotalAmount.Text = ""
        End If

        BindGiroNo(ddlBank.SelectedItem.Value, "")

        If lblStsHid.Text <> "" Then
            intStatus = Convert.ToInt16(lblStsHid.Text)
            If intStatus = objCBTrx.EnumCashBankStatus.Confirmed Then
                If strBank = ddlBankTo.SelectedItem.Value Or ddlBankTo.SelectedItem.Value = "" Then
                    PrintSlipBtn.Visible = True
                    PrintTransferBtn.Visible = False
                Else
                    PrintSlipBtn.Visible = False
                    PrintTransferBtn.Visible = True
                End If
            End If
        End If
    End Sub

    Sub BindChargeLevelDropDownList()
        ddlChargeLevel.Items.Add(New ListItem(PreBlockTag, objLangCap.EnumLangCap.Block))
        ddlChargeLevel.Items.Add(New ListItem(BlockTag, objLangCap.EnumLangCap.SubBlock))
        ddlChargeLevel.SelectedIndex = Session("SS_BLOCK_CHARGE_DEFAULT")
        RowChargeLevel.Visible = Session("SS_BLOCK_CHARGE_VISIBLE")
        ToggleChargeLevel()
    End Sub

    Sub ddlChargeLevel_OnSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        ToggleChargeLevel()
    End Sub

    Sub onSelect_PayType(ByVal Sender As System.Object, ByVal E As System.EventArgs)
        'BindPayType(ddlPayType.SelectedItem.Value)
        BindGiroNo(ddlBank.SelectedItem.Value, "")
    End Sub

    Sub ddlLocation_OnSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim strAccCode As String = txtAccCode.Text.Trim()
        Dim strVehCode As String = lstVehCode.SelectedItem.Value.Trim()
        Dim strPreBlkCode As String = ddlPreBlock.SelectedItem.Value.Trim()
        Dim strBlkCode As String = lstBlock.SelectedItem.Value.Trim()
        BindVehicleCodeDropList(strAccCode, strVehCode)
        BindBlockDropList(strAccCode, strBlkCode)
        BindPreBlock(strAccCode, strPreBlkCode)
        hidChargeLocCode.Value = ddlLocation.SelectedItem.Value.Trim()
    End Sub

    Sub SaveBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim TxResultID As String
        Dim strAllowVehicle As String
        Dim ErrorChk As Integer = 0
        Dim StrTxParam As String
        Dim strDocPrefix As String
        Dim strGiroDate As String = Date_Validation(txtGiroDate.Text, False)
        Dim strNewIDFormat As String
        Dim strInOrOut As String
        Dim strRemark As String = Trim(Replace(txtRemark.Text, "'", "''"))
        Dim strInitial As String = ""
        Dim strParamName As String
        Dim strParamValue As String
        Dim strOpCd As String
        Dim intStatus As Integer
        Dim strDate As String = Date_Validation(txtDateCreated.Text, False)
        Dim indDate As String = ""
        Dim strBank As String
        Dim strBankAccCode As String
        Dim strBankAccNo As String
        Dim arrParam As Array

        If ddlBank.SelectedItem.Value = "" Then
            strBank = ""
            strBankAccCode = ""
            strBankAccNo = ""
        Else
            arrParam = Split(Trim(ddlBank.SelectedItem.Value), "|")
            strBank = Trim(arrParam(0))
            strBankAccCode = Trim(arrParam(1))
            strBankAccNo = Trim(arrParam(2))

            If ddlPayType.SelectedItem.Value = 0 Or ddlPayType.SelectedItem.Value = 3 Then
                If strBankAccNo = "" Then
                    lblErrPayType.Text = "This payment type must use Bank account"
                    lblErrPayType.Visible = True
                    Exit Sub
                End If
            Else
                If strBankAccNo <> "" Then
                    lblErrPayType.Text = "This payment type cannot use Bank account"
                    lblErrPayType.Visible = True
                    Exit Sub
                End If
                Select Case strBank
                    Case "KKL", "KKK"
                        If ddlPayType.SelectedItem.Value = 4 Then
                            lblErrPayType.Text = "This payment type cannot use Kas account"
                            lblErrPayType.Visible = True
                            Exit Sub
                        End If
                        If Left(lblPaymentID.Text, 3) <> "XXX" Then
                            If Left(lblPaymentID.Text, 2) <> "KK" Then
                                lblErrPayType.Text = "<br>ID transaction already set to Kas Besar."
                                lblErrPayType.Visible = True
                                Exit Sub
                            End If
                        End If
                        
                    Case "KBR"
                        If ddlPayType.SelectedItem.Value = 4 Then
                            lblErrPayType.Text = "This payment type cannot use Kas account"
                            lblErrPayType.Visible = True
                            Exit Sub
                        End If
                        If Left(lblPaymentID.Text, 3) <> "XXX" Then
                            If Left(lblPaymentID.Text, 2) <> "KR" Then
                                lblErrPayType.Text = "<br>ID transaction already set to Kas Kecil."
                                lblErrPayType.Visible = True
                                Exit Sub
                            End If
                        End If

                    Case Else
                        If ddlPayType.SelectedItem.Value = 1 Then
                            lblErrPayType.Text = "This payment type cannot use RK account"
                            lblErrPayType.Visible = True
                            Exit Sub
                        End If
                End Select
            End If
        End If

        If CheckDate(txtDateCreated.Text.Trim(), indDate) = False Then
            lblDate.Visible = True
            lblFmt.Visible = True
            lblDate.Text = "<br>Date Entered should be in the format"
            Exit Sub
        End If

        Dim intInputPeriod As Integer = Year(strDate) * 100 + Month(strDate)
        Dim intCurPeriod As Integer = (CInt(strAccYear) * 100) + CInt(strAccMonth)
        Dim intSelPeriod As Integer = (CInt(strSelAccYear) * 100) + CInt(strSelAccMonth)

        If Session("SS_FILTERPERIOD") = "0" Then
            If intCurPeriod < intInputPeriod Then
                lblDate.Visible = True
                lblDate.Text = "Invalid transaction date."
                Exit Sub
            End If
        Else
            If intSelPeriod <> intInputPeriod Then
                lblDate.Visible = True
                lblDate.Text = "Invalid transaction date."
                Exit Sub
            End If
            If intSelPeriod < intCurPeriod Then
                lblDate.Visible = True
                lblDate.Text = "This period already locked."
                Exit Sub
            End If
        End If

        If lblPaymentID.Text = "" Then
            Exit Sub
        End If

        If txtPaymentTo.Text = "" Then
            lblConfirmErr.Visible = True
            lblConfirmErr.Text = "Payment To/Receive From cannot be empty."
            Exit Sub
        End If

        'If intLevel = 0 Then
        '    If lblStatus.Text = objCBTrx.mtdGetCashBankStatus(objCBTrx.EnumCashBankStatus.Active) Then
        '        If ddlPayType.SelectedItem.Value <> "2" Then
        '            If rblCashBankType.SelectedValue = objCBTrx.EnumCashBankType.Payment Then
        '                If hidHadCOATax.Value > 0 Then
        '                    If CInt(hidTaxStatus.Value) = objCBTrx.EnumTaxStatus.Unverified Then
        '                        lblConfirmErr.Visible = True
        '                        lblConfirmErr.Text = "This transaction contain tax account, please verify first."
        '                        Exit Sub
        '                    End If
        '                ElseIf UCase(Trim(strUserId)) <> UCase(Trim(hidUserID.Value)) Then
        '                    lblConfirmErr.Visible = True
        '                    lblConfirmErr.Text = "You do not have permission to process this transaction."
        '                    Exit Sub
        '                End If
        '            End If
        '        ElseIf UCase(Trim(strUserId)) <> UCase(Trim(hidUserID.Value)) Then
        '            lblConfirmErr.Visible = True
        '            lblConfirmErr.Text = "You do not have permission to process this transaction."
        '            Exit Sub
        '        End If
        '    End If
        'End If
        'If ddlPayType.SelectedItem.Value <> "2" Then 'if other than Need Verification
        '    If hidHadCOATax.Value > 0 Then
        '        If CInt(hidTaxStatus.Value) = objCBTrx.EnumTaxStatus.Unverified Then
        '            lblConfirmErr.Visible = True
        '            lblConfirmErr.Text = "This transaction contain tax account, please verify first."
        '            Exit Sub
        '        End If
        '    End If
        'End If

        If ddlPayType.SelectedItem.Value = "0" Or ddlPayType.SelectedItem.Value = "3" Then
            If strBank = "" Then
                lblErrBank.Visible = True
                Exit Sub
            End If
            If txtGiroDate.Text = "" Then
                lblDateGiro.Visible = True
                lblDateGiro.Text = "Please enter Giro Date"
                Exit Sub
            End If
            If CheckDate(txtGiroDate.Text.Trim(), indDate) = False Then
                lblDateGiro.Visible = True
                lblFmtGiro.Visible = True
                lblDateGiro.Text = "<br>Date Entered should be in the format"
                Exit Sub
            End If
            'If txtChequeNo.Text = "" Then
            '    lblErrCheque.Visible = True
            '    Exit Sub
            'End If
            If lblCurrentPeriod.Text <> "" Then
                If Month(strGiroDate) >= Mid(lblCurrentPeriod.Text, 5) And Year(strGiroDate) >= Mid(lblCurrentPeriod.Text, 1, 4) Then
                Else
                    If rblCashBankType.SelectedValue = objCBTrx.EnumCashBankType.Payment Then
                        lblDateGiro.Visible = True
                        lblDateGiro.Text = "Invalid transaction date."
                        Exit Sub
                    End If
                End If
                If Year(strGiroDate) & Format(Month(strGiroDate), "00") & Format(Day(strGiroDate), "00") < Year(strDate) & Format(Month(strDate), "00") & Format(Day(strDate), "00") Then
                    If rblCashBankType.SelectedValue = objCBTrx.EnumCashBankType.Payment Then
                        lblDateGiro.Visible = True
                        lblDateGiro.Text = "Invalid transaction date."
                        Exit Sub
                    End If
                End If
            Else
                If Year(strGiroDate) & Format(Month(strGiroDate), "00") & Format(Day(strGiroDate), "00") < Year(strDate) & Format(Month(strDate), "00") & Format(Day(strDate), "00") Then
                    If rblCashBankType.SelectedValue = objCBTrx.EnumCashBankType.Payment Then
                        lblDateGiro.Visible = True
                        lblDateGiro.Text = "Invalid transaction date."
                        Exit Sub
                    End If
                End If
                If Month(strGiroDate) < strAccMonth And Year(strGiroDate) <= strAccYear Then
                    If rblCashBankType.SelectedValue = objCBTrx.EnumCashBankType.Payment Then
                        lblDateGiro.Visible = True
                        lblDateGiro.Text = "Invalid transaction date."
                        Exit Sub
                    End If
                End If
            End If
        End If

        If rblCashBankType.SelectedValue = objCBTrx.EnumCashBankType.Payment Then
            strDocPrefix = objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CBPaymentOther)
        Else
            strDocPrefix = objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CBReceiptOther)
        End If

        'If lblPaymentID.Text <> "" Then
        '    Select Case ddlPayType.SelectedItem.Value
        '        Case 0
        '            Select Case rblCashBankType.SelectedValue
        '                Case objCBTrx.EnumCashBankType.Payment
        '                    strInitial = "BBK"
        '                Case objCBTrx.EnumCashBankType.Receipt
        '                    strInitial = "BBT"
        '                Case objCBTrx.EnumCashBankType.Transfer
        '                    strInitial = "STF"
        '            End Select

        '        Case 1
        '            Select Case rblCashBankType.SelectedValue
        '                Case objCBTrx.EnumCashBankType.Payment
        '                    Select Case Trim(strBank)
        '                        Case "KKL", "KKK"
        '                            strInitial = "KKK"
        '                        Case Else
        '                            strInitial = "KRK"
        '                    End Select

        '                Case objCBTrx.EnumCashBankType.Receipt
        '                    Select Case Trim(strBank)
        '                        Case "KKL", "KKK"
        '                            strInitial = "KKT"
        '                        Case Else
        '                            strInitial = "KRT"
        '                    End Select
        '            End Select

        '        Case 2
        '            strInitial = "XXX"

        '        Case 3
        '            Select Case rblCashBankType.SelectedValue
        '                Case objCBTrx.EnumCashBankType.Payment
        '                    strInitial = "BBK"
        '                Case objCBTrx.EnumCashBankType.Receipt
        '                    strInitial = "BBT"
        '                Case objCBTrx.EnumCashBankType.Transfer
        '                    strInitial = "STF"
        '            End Select

        '        Case 4
        '            strInitial = "PPL"

        '    End Select
        'Else
        '    strInitial = "XXX"
        'End If

        strInitial = "XXX"

        Select Case rblCashBankType.SelectedValue
            Case objCBTrx.EnumCashBankType.Payment
                strInOrOut = "O"
            Case objCBTrx.EnumCashBankType.Receipt
                strInOrOut = "I"
        End Select

        If lblCurrentPeriod.Text <> "" Then
            If Month(strDate) = Mid(lblCurrentPeriod.Text, 5) And Year(strDate) = Mid(lblCurrentPeriod.Text, 1, 4) Then
            Else
                lblDate.Visible = True
                lblDate.Text = "Invalid transaction date."
                Exit Sub
            End If
        End If

        strAccYear = Year(strDate)
        strAccMonth = Month(strDate)

        If lblPaymentID.Text <> "" And Left(lblPaymentID.Text.Trim, 3) <> "XXX" Then
            If txtDateCreated.Enabled = True Then
                If hidPrevID.Value = "" Then
                    If hidNextID.Value <> "" Then
                        'If Day(strDate) > hidNextDate.Value Then
                        '    lblDate.Visible = True
                        '    lblDate.Text = "Date cannot be higher than last transaction date."
                        '    Exit Sub
                        'End If
                        hidNextDate.Visible = False
                    End If
                Else
                    ''remark sementara
                    'If hidPrevID.Value <> "" Then
                    '    If Day(strDate) < hidPrevDate.Value Then
                    '        lblDate.Visible = True
                    '        lblDate.Text = "Date cannot be smaller than previous transaction date."
                    '        Exit Sub
                    '    End If
                    '    If hidNextID.Value <> "" Then
                    '        'If Day(strDate) > hidNextDate.Value Then
                    '        '    lblDate.Visible = True
                    '        '    lblDate.Text = "Date cannot be higher than last transaction date."
                    '        '    Exit Sub
                    '        'End If
                    '        hidNextDate.Visible = False
                    '    End If
                    'Else
                    '    If Day(strDate) < hidPrevDate.Value Then
                    '        lblDate.Visible = True
                    '        lblDate.Text = "Date cannot be smaller than previous transaction date."
                    '        Exit Sub
                    '    End If
                    'End If
                End If
            End If

        ElseIf ddlPayType.SelectedItem.Value <> 2 Then
            ''remark sementara 
            ''komparasi tanggal input
            'Dim objLastID As New Object
            'Dim strSearchID As String
            'Dim strOpCdSearch As String
            'Dim intCBDate As Integer
            'Dim intPYDate As Integer
            'strOpCd = "CB_CLSTRX_SEARCH_LASTID"
            'strSearchID = " LocCode = '" & strLocation & "' And AccMonth = '" & Trim(strAccMonth) & "' And AccYear = '" & Trim(strAccYear) & "' And Left(LTrim(CashBankID),3) = '" & Trim(strInitial) & "' "
            'strParamName = "TABLE|FIELD|STRSEARCH"
            'strParamValue = "CB_CASHBANK" & "|" & "CASHBANKID" & "|" & strSearchID

            'Try
            '    intErrNo = objGLtrx.mtdGetDataCommon(strOpCd, _
            '                                        strParamName, _
            '                                        strParamValue, _
            '                                        objLastID)
            'Catch Exp As System.Exception
            '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ID_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
            'End Try

            'If objLastID.Tables(0).Rows.Count > 0 Then
            '    intCBDate = Day(objGlobal.GetLongDate(Trim(objLastID.Tables(0).Rows(0).Item("DocDate"))))
            'Else
            '    intCBDate = 0
            'End If

            'strSearchID = " LocCode = '" & strLocation & "' And AccMonth = '" & Trim(strAccMonth) & "' And AccYear = '" & Trim(strAccYear) & "' And Left(LTrim(PaymentID),3) = '" & Trim(strInitial) & "' "
            'strParamName = "TABLE|FIELD|STRSEARCH"
            'strParamValue = "CB_PAYMENT" & "|" & "PAYMENTID" & "|" & strSearchID
            'Try
            '    intErrNo = objGLtrx.mtdGetDataCommon(strOpCd, _
            '                                        strParamName, _
            '                                        strParamValue, _
            '                                        objLastID)
            'Catch Exp As System.Exception
            '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ID_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
            'End Try
            'If objLastID.Tables(0).Rows.Count > 0 Then
            '    intPYDate = Day(objGlobal.GetLongDate(Trim(objLastID.Tables(0).Rows(0).Item("DocDate"))))
            'Else
            '    intPYDate = 0
            'End If


            'If intCBDate >= intPYDate Then
            '    If Day(strDate) < intCBDate Then
            '        lblDate.Visible = True
            '        lblDate.Text = "Date cannot be smaller than last transaction date."
            '        Exit Sub
            '    End If
            'Else
            '    If Day(strDate) < intPYDate Then
            '        lblDate.Visible = True
            '        lblDate.Text = "Date cannot be smaller than last transaction date."
            '        Exit Sub
            '    End If
            'End If
        End If

        'strNewIDFormat = strInitial & "/" & strCompany & "/" & strLocation & "/" & strInOrOut & "/" & strAccYear & IIf(Len(Trim(strAccMonth)) = 1, "0" & strAccMonth, strAccMonth) & "/"
        strNewIDFormat = strInitial & "/" & strCompany & "/" & strLocation & "/" & IIf(Len(Trim(strAccMonth)) = 1, "0" & strAccMonth, strAccMonth) & "/" & Trim(strAccYear) & "/" 

        StrTxParam = lblPaymentID.Text & "|" & rblCashBankType.SelectedValue & "|" & ddlPayType.SelectedValue & _
                          "|" & strBank & "|" & IIf(Trim(txtChequeNo.Text) = "", "", Trim(txtChequeNo.Text)) & _
                          "|" & IIf(Trim(txtPaymentTo.Text) = "", "", Trim(txtPaymentTo.Text)) & "|" & strLocation & "|" & strAccMonth & _
                          "|" & strAccYear & "||" & strDate & "||" & strGiroDate & "|" & strUserId & "||" & strNewIDFormat & _
                          "|" & IIf(Trim(ddlSplBankAccNo.SelectedItem.Value) = "", "", Trim(ddlSplBankAccNo.SelectedItem.Value)) & "|" & strRemark & _
                          "|" & ddlBankTo.SelectedValue & "|" & txtSupCode.Text & "|" & CInt(hidTaxStatus.Value) & _
                          "|" & strBankAccNo & "|" & IIf(chkChequeCash.Checked = True, "1", "2") & "|" & ddlChequeHandOver.SelectedItem.Value

        Try

            intErrNo = objCBTrx.mtdUpdCashBankDetail(strOpCdTxDet_ADD, _
                                                        strOpCdTxDet_UPD, _
                                                        strUserId, _
                                                        StrTxParam, _
                                                        strDocPrefix, _
                                                        ErrorChk, _
                                                        TxResultID)

            lblPaymentID.Text = TxResultID
            If ErrorChk = 1 Then
                lblerror.Visible = True
            End If

        Catch Exp As System.Exception
            If intErrNo <> -5 Then
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADD_ADDNEWCASHBANK&errmesg=" & lblErrMessage.Text & "&redirect=/CB/trx/CB_trx_CashBankDet.aspx")
            End If
        End Try


        intStatus = Convert.ToInt16(lblStsHid.Text)
        If intStatus = objCBTrx.EnumCashBankStatus.Active Or intStatus = objCBTrx.EnumCashBankStatus.Verified Or intStatus = objCBTrx.EnumCashBankStatus.Confirmed Then
            If strBank = "" And Left(Trim(lblPaymentID.Text), 3) <> "XXX" Then
                lblErrBank.Visible = True
                Exit Sub
            End If

            If strBank <> "" Then
                strOpCd = "CB_CLSTRX_CASHBANK_DETAIL_UPD_ACCNO"
                strParamName = "BANKCODE|CASHBANKID|SOURCETYPE|BANKACCNO|LOCCODE"
                strParamValue = strBank & "|" & _
                                lblPaymentID.Text & "|" & _
                                ddlPayType.SelectedItem.Value & "|" & _
                                strBankAccNo & "|" & _
                                strLocation

                Try
                    intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd, _
                                                            strParamName, _
                                                            strParamValue)

                Catch Exp As System.Exception
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PAYMENT_UPD&errmesg=" & Exp.ToString() & "&redirect=cb/trx/cb_trx_cashbanklist")
                End Try
            End If
        End If

        'penomeran pindah ke verified
        'If (Left(Trim(lblPaymentID.Text), 3) <> "TBS" Or Left(Trim(lblPaymentID.Text), 3) <> "CPO" Or Left(Trim(lblPaymentID.Text), 3) <> "KER" Or Left(Trim(lblPaymentID.Text), 3) <> "CKG" Or Left(Trim(lblPaymentID.Text), 3) <> "OTH") Then
        '    If ddlPayType.SelectedItem.Value <> 2 And Left(Trim(lblPaymentID.Text), 3) = "XXX" Then
        '        Dim strOpCd_FindLastID As String = "ADMIN_CLSTRX_SEARCH_LASTID_CB"
        '        Dim strLastIDSearch1 As String
        '        Dim strLastIDSearch2 As String
        '        Dim strLastIDSearch3 As String
        '        Dim strNewID As String
        '        Dim objLastIDDs As New Object
        '        strOpCd = "CB_CLSTRX_CASHBANK_UPDATE_TRXID"

        '        strLastIDSearch1 = " LocCode = '" & strLocation & "' And AccMonth = '" & Trim(strAccMonth) & "' And AccYear = '" & Trim(strAccYear) & "' And Left(LTrim(CashBankID),3) = '" & Trim(strInitial) & "' "
        '        strLastIDSearch2 = " LocCode = '" & strLocation & "' And AccMonth = '" & Trim(strAccMonth) & "' And AccYear = '" & Trim(strAccYear) & "' And Left(LTrim(PaymentID),3) = '" & Trim(strInitial) & "' "
        '        strLastIDSearch3 = " LocCode = '" & strLocation & "' And AccMonth = '" & Trim(strAccMonth) & "' And AccYear = '" & Trim(strAccYear) & "' And Left(LTrim(ReceiptID),3) = '" & Trim(strInitial) & "' "
        '        strParamName = "STRSEARCH1|STRSEARCH3|STRSEARCH4"
        '        strParamValue = strLastIDSearch1 & "|" & strLastIDSearch2 & "|" & strLastIDSearch3

        '        Try
        '            intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_FindLastID, _
        '                                                strParamName, _
        '                                                strParamValue, _
        '                                                objLastIDDs)
        '        Catch Exp As System.Exception
        '            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ID_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        '        End Try

        '        If objLastIDDs.Tables(0).Rows.Count > 0 Then
        '            strNewID = Trim(objLastIDDs.Tables(0).Rows(0).Item("NewTrxID"))
        '        Else
        '            strNewID = "1"
        '        End If

        '        strNewIDFormat = strInitial & "/" & strCompany & "/" & strLocation & "/" & IIf(Len(Trim(strAccMonth)) = 1, "0" & strAccMonth, strAccMonth) & "/" & Trim(strAccYear) & "/" & Format(Val(strNewID), "0000")

        '        strParamName = "SOURCETYPE|CHEQUENO|BANKCODE|BANKTO|SPLBANKACCNO|REMARK|CHEQUEPRINTDATE|PRINTDATE|LOCCODE|CASHBANKID|CASHBANKNEWID|UPDATEID"

        '        strParamValue = Trim(ddlPayType.SelectedItem.Value) & _
        '                        "|" & Trim(txtChequeNo.Text) & _
        '                        "|" & Trim(strBank) & _
        '                        "|" & Trim(ddlBankTo.SelectedItem.Value) & _
        '                        "|" & Trim(ddlSplBankAccNo.SelectedItem.Value) & _
        '                        "|" & Trim(Replace(txtRemark.Text, "'", "''")) & _
        '                        "|" & "" & _
        '                        "|" & strGiroDate & _
        '                        "|" & Trim(strLocation) & _
        '                        "|" & Trim(lblPaymentID.Text) & _
        '                        "|" & Trim(strNewIDFormat) & _
        '                        "|" & Trim(strUserId)

        '        Try
        '            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd, _
        '                                                    strParamName, _
        '                                                    strParamValue)

        '        Catch Exp As System.Exception
        '            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_CASHBANK_UPDATE_TRXID&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_CashBankDet.aspx")
        '        End Try

        '        lblPaymentID.Text = Trim(strNewIDFormat) 'Trim(strBank) + Trim(Mid(lblPaymentID.Text, 4))
        '    End If
        'End If

        Dim strOpCdUpd As String = "CB_CLSTRX_CASHBANK_UPDATE"

        strParamName = "CHEQUENO|BANKCODE|REMARK|CHEQUEPRINTDATE|PRINTDATE|LOCCODE|CASHBANKID"

        strParamValue = Trim(txtChequeNo.Text) & _
                        "|" & Trim(strBank) & _
                        "|" & Trim(Replace(txtRemark.Text, "'", "''")) & _
                        "|" & IIf(Left(Trim(lblPaymentID.Text), 3) = "XXX", "", "") & _
                        "|" & strGiroDate & _
                        "|" & Trim(strLocation) & _
                        "|" & Trim(lblPaymentID.Text.Trim)

        Try
            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCdUpd, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_CASHBANK_PREVIEWCHEQUE_UPD&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_CashBankDet.aspx")
        End Try

        UpdateCBLine()
        LoadTxDetails()
        DisplayFromDB()
        onLoad_Button()
    End Sub

    Sub ForwardBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim TxResultID As String
        Dim strAllowVehicle As String
        Dim ErrorChk As Integer = 0
        Dim StrTxParam As String
        Dim strDocPrefix As String
        Dim new_strDate As String '= Date_Validation(txtDateCreated.Text, False)
        Dim new_strGiroDate As String '= Date_Validation(txtGiroDate.Text, False)
        Dim strNewIDFormat As String
        Dim strInOrOut As String
        Dim strRemark As String = Trim(Replace(txtRemark.Text, "'", "''"))
        Dim strInitial As String = ""
        Dim newDate As Date = Date_Validation(txtDateCreated.Text, False)
        Dim newGiroDate As Date = Date_Validation(txtGiroDate.Text, False)
        Dim strDate As String = Date_Validation(txtDateCreated.Text, False)
        Dim indDate As String = ""

        Dim strBank As String
        Dim strBankAccCode As String
        Dim strBankAccNo As String
        Dim arrParam As Array

        If ddlBank.SelectedItem.Value = "" Then
            strBank = ""
            strBankAccCode = ""
            strBankAccNo = ""
        Else
            arrParam = Split(Trim(ddlBank.SelectedItem.Value), "|")
            strBank = Trim(arrParam(0))
            strBankAccCode = Trim(arrParam(1))
            strBankAccNo = Trim(arrParam(2))
        End If

        If CheckDate(txtDateCreated.Text.Trim(), indDate) = False Then
            lblDate.Visible = True
            lblFmt.Visible = True
            lblDate.Text = "<br>Date Entered should be in the format"
            Exit Sub
        End If

        Dim intInputPeriod As Integer = Year(strDate) * 100 + Month(strDate)
        Dim intCurPeriod As Integer = (CInt(strAccYear) * 100) + CInt(strAccMonth)
        Dim intSelPeriod As Integer = (CInt(strSelAccYear) * 100) + CInt(strSelAccMonth)

        If Session("SS_FILTERPERIOD") = "0" Then
            If intSelPeriod < intInputPeriod Then
                lblDate.Visible = True
                lblDate.Text = "Invalid transaction date."
                Exit Sub
            End If
        Else
            If intSelPeriod <> intInputPeriod Then
                lblDate.Visible = True
                lblDate.Text = "Invalid transaction date."
                Exit Sub
            End If
            If intSelPeriod < intCurPeriod And intLevel < 2 Then
                lblDate.Visible = True
                lblDate.Text = "This period already locked."
                Exit Sub
            End If
        End If

        If lblPaymentID.Text = "" Then
            Exit Sub
        End If

        If ddlPayType.SelectedItem.Value = "0" Or ddlPayType.SelectedItem.Value = "3" Then
            If strBank = "" Then
                lblErrBank.Visible = True
                Exit Sub
            End If
            'If txtChequeNo.Text = "" Then
            '    lblErrCheque.Visible = True
            '    Exit Sub
            'End If
        End If

        If rblCashBankType.SelectedValue = objCBTrx.EnumCashBankType.Payment Then
            strDocPrefix = objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CBPaymentOther)
        Else
            strDocPrefix = objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CBReceiptOther)
        End If

        new_strDate = DateAdd(DateInterval.Month, 1, newDate)
        new_strGiroDate = DateAdd(DateInterval.Month, 1, newGiroDate)
        strAccYear = Year(new_strDate)
        strAccMonth = Month(new_strDate)

        StrTxParam = lblPaymentID.Text & "|" & rblCashBankType.SelectedValue & "|" & ddlPayType.SelectedValue & _
                          "|" & strBank & "|" & IIf(Trim(txtChequeNo.Text) = "", "", Trim(txtChequeNo.Text)) & _
                          "|" & IIf(Trim(txtPaymentTo.Text) = "", "", Trim(txtPaymentTo.Text)) & "|" & strLocation & "|" & strAccMonth & _
                          "|" & strAccYear & "||" & new_strDate & "||" & new_strGiroDate & "|" & strUserId & "||" & _
                          "|" & IIf(Trim(ddlSplBankAccNo.SelectedItem.Value) = "", "", Trim(ddlSplBankAccNo.SelectedItem.Value)) & "|" & strRemark & _
                          "|" & ddlBankTo.SelectedValue & "|" & txtSupCode.Text & "|" & CInt(hidTaxStatus.Value) & _
                          "|" & strBankAccNo & "|" & IIf(chkChequeCash.Checked = True, "1", "2") & "|" & ddlChequeHandOver.SelectedItem.Value

        Try

            intErrNo = objCBTrx.mtdUpdCashBankDetail(strOpCdTxDet_ADD, _
                                                        strOpCdTxDet_UPD, _
                                                        strUserId, _
                                                        StrTxParam, _
                                                        strDocPrefix, _
                                                        ErrorChk, _
                                                        TxResultID)

            lblPaymentID.Text = TxResultID
            If ErrorChk = 1 Then
                lblerror.Visible = True
            End If

        Catch Exp As System.Exception
            If intErrNo <> -5 Then
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADD_ADDNEWCASHBANK&errmesg=" & lblErrMessage.Text & "&redirect=/CB/trx/CB_trx_CashBankDet.aspx")
            End If
        End Try

        LoadTxDetails()
        DisplayFromDB()
        onLoad_Button()
    End Sub

    Sub ConfirmBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        If Left(Trim(lblPaymentID.Text), 3) = "XXX" Then
            lblErrPayType.Visible = True
            Exit Sub
        Else
            Update_Status(objCBTrx.EnumCashBankStatus.Confirmed)
            LoadTxDetails()
            DisplayFromDB()
            onLoad_Button()
        End If
    End Sub

    Sub PreviewBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim strOpCodePrint As String = "ADMIN_SHARE_UPD_PRINTDATE"
        Dim strUpdString As String = ""
        Dim strStatus As String
        Dim intStatus As Integer
        Dim strSortLine As String
        Dim strPrintDate As String
        Dim strTable As String
        Dim strCBType As String
        Dim strReportName As String
        Dim strID As String = lblPaymentID.Text.Trim
        Dim strGiroDate As String = Date_Validation(txtGiroDate.Text, False)

        Dim strBank As String
        Dim strBankAccCode As String
        Dim strBankAccNo As String
        Dim arrParam As Array

        If ddlBank.SelectedItem.Value = "" Then
            strBank = ""
            strBankAccCode = ""
            strBankAccNo = ""
        Else
            arrParam = Split(Trim(ddlBank.SelectedItem.Value), "|")
            strBank = Trim(arrParam(0))
            strBankAccCode = Trim(arrParam(1))
            strBankAccNo = Trim(arrParam(2))
        End If

        Select Case Trim(Replace(Replace(Replace(lblCtrlAmtFig.Text, ".", ""), ",", "."), "-", ""))
            Case "0.00", "0,00", "0", ""
            Case Else
                lblConfirmErr.Visible = True
                Exit Sub
        End Select

        strUpdString = "where CashBankID = '" & lblPaymentID.Text & "'"
        strStatus = Trim(lblStatus.Text)
        intStatus = Convert.ToInt16(lblStsHid.Text)
        strPrintDate = Trim(lblPrintDate.Text)
        strSortLine = "CB_CASHBANKLN.CashBankLnID"
        strTable = "CB_CASHBANK"
        strCBType = rblCashBankType.SelectedValue 
        strReportName = "CB_RPT_CashBankPrint"

        'If intStatus = objCBTrx.EnumCashBankStatus.Confirmed Then
        '    If strPrintDate = "" or strPrintDate = "01 Jan 1900" Then
        '        Try
        '            intErrNo = objAdmShare.mtdUpdPrintDate(strOpCodePrint, _
        '                                                   strUpdString, _
        '                                                   strTable, _
        '                                                   strCompany, _
        '                                                   strLocation, _
        '                                                   strUserId)
        '        Catch Exp As Exception
        '            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_CASHBANKDET_UPD_PRINTDATE&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_CashBankDet.aspx")
        '        End Try
        '    Else
        '        strStatus = strStatus & " (re-printed)"
        '    End If
        'End If

        Dim strOpCd As String = "CB_CLSTRX_CASHBANK_UPDATE"
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim intErrNo As Integer

        strParamName = "CHEQUENO|BANKCODE|REMARK|CHEQUEPRINTDATE|PRINTDATE|LOCCODE|CASHBANKID"

        strParamValue = Trim(txtChequeNo.Text) & _
                        "|" & Trim(strBank) & _
                        "|" & Trim(Replace(txtRemark.Text, "'", "''")) & _
                        "|" & IIf(Left(Trim(lblPaymentID.Text), 3) = "XXX", "", Now()) & _
                        "|" & strGiroDate & _
                        "|" & Trim(strLocation) & _
                        "|" & Trim(strID)

        Try
            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_CASHBANK_PREVIEWCHEQUE_UPD&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_CashBankDet.aspx")
        End Try

        LoadTxDetails()
        DisplayFromDB()
        onLoad_Button()

        Response.Write("<Script Language=""JavaScript"">window.open(""../reports/CB_Rpt_CashBankPrint.aspx?strId=" & lblPaymentID.Text & _
                       "&strPrintDate=" & strPrintDate & _
                       "&strStatus=" & strStatus & _
                       "&CBType=" & strCBType & _
                       "&strSortLine=" & strSortLine & _
                       "&strAccountTag=" & strAccountTag & _
                       "&strVehTag=" & strVehTag & _
                       "&strVehExpTag=" & strVehExpTag & _
                       "&strBlockTag=" & BlockTag & _
                       "&strReportName=" & strReportName & _
                       """, null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")
    End Sub

    Sub PreviewKwitansiBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim strOpCodePrint As String = "ADMIN_SHARE_UPD_PRINTDATE"
        Dim strUpdString As String = ""
        Dim strStatus As String
        Dim intStatus As Integer
        Dim strSortLine As String
        Dim strPrintDate As String
        Dim strTable As String
        Dim strCBType As String
        Dim strReportName As String
        Dim strID As String = lblPaymentID.Text.Trim
        Dim strGiroDate As String = Date_Validation(txtGiroDate.Text, False)

        Dim strBank As String
        Dim strBankAccCode As String
        Dim strBankAccNo As String
        Dim arrParam As Array

        If ddlBank.SelectedItem.Value = "" Then
            strBank = ""
            strBankAccCode = ""
            strBankAccNo = ""
        Else
            arrParam = Split(Trim(ddlBank.SelectedItem.Value), "|")
            strBank = Trim(arrParam(0))
            strBankAccCode = Trim(arrParam(1))
            strBankAccNo = Trim(arrParam(2))
        End If

        Select Case Trim(Replace(Replace(Replace(lblCtrlAmtFig.Text, ".", ""), ",", "."), "-", ""))
            Case "0.00", "0,00", "0", ""
            Case Else
                lblConfirmErr.Visible = True
                Exit Sub
        End Select

        strUpdString = "where CashBankID = '" & lblPaymentID.Text & "'"
        strStatus = Trim(lblStatus.Text)
        intStatus = Convert.ToInt16(lblStsHid.Text)
        strPrintDate = Trim(lblPrintDate.Text)
        strSortLine = "CB_CASHBANKLN.CashBankLnID"
        strTable = "CB_CASHBANK"
        strCBType = 3 'to capture Kwitansi printing --rblCashBankType.SelectedValue
        strReportName = "CB_RPT_ReceiptVoucher"

        'If intStatus = objCBTrx.EnumCashBankStatus.Confirmed Then
        '    If strPrintDate = "" Or strPrintDate = "01 Jan 1900" Then
        '        Try
        '            intErrNo = objAdmShare.mtdUpdPrintDate(strOpCodePrint, _
        '                                                   strUpdString, _
        '                                                   strTable, _
        '                                                   strCompany, _
        '                                                   strLocation, _
        '                                                   strUserId)
        '        Catch Exp As Exception
        '            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_CASHBANKDET_UPD_PRINTDATE&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_CashBankDet.aspx")
        '        End Try
        '    Else
        '        strStatus = strStatus & " (re-printed)"
        '    End If
        'End If

        Dim strOpCd As String = "CB_CLSTRX_CASHBANK_UPDATE"
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim intErrNo As Integer

        strParamName = "CHEQUENO|BANKCODE|REMARK|CHEQUEPRINTDATE|PRINTDATE|LOCCODE|CASHBANKID"

        strParamValue = Trim(txtChequeNo.Text) & _
                        "|" & Trim(strBank) & _
                        "|" & Trim(Replace(txtRemark.Text, "'", "''")) & _
                        "|" & IIf(Left(Trim(lblPaymentID.Text), 3) = "XXX", "", Now()) & _
                        "|" & IIf(Left(Trim(lblPaymentID.Text), 3) = "XXX", "", Now()) & _
                        "|" & Trim(strLocation) & _
                        "|" & Trim(strID)

        Try
            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_CASHBANK_PREVIEWCHEQUE_UPD&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_CashBankDet.aspx")
        End Try


        LoadTxDetails()
        DisplayFromDB()
        onLoad_Button()

        Response.Write("<Script Language=""JavaScript"">window.open(""../reports/CB_Rpt_CashBankPrint.aspx?strId=" & lblPaymentID.Text & _
                       "&strPrintDate=" & strPrintDate & _
                       "&strStatus=" & strStatus & _
                       "&CBType=" & strCBType & _
                       "&strSortLine=" & strSortLine & _
                       "&strAccountTag=" & strAccountTag & _
                       "&strVehTag=" & strVehTag & _
                       "&strVehExpTag=" & strVehExpTag & _
                       "&strBlockTag=" & BlockTag & _
                       "&strReportName=" & strReportName & _
                       """, null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")
    End Sub

    Sub PreviewChequeBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim dsForBankCodeDropDown As New DataSet()
        Dim strChequeNo As String = txtChequeNo.Text.Trim
        Dim strID As String = lblPaymentID.Text.Trim
        Dim strProgramPath As String = ""

        Dim strStatus As String
        Dim intStatus As Integer
        Dim strSortLine As String
        Dim strPrintDate As String
        Dim strTable As String
        Dim strCBType As String
        Dim strGiroDate As String = Date_Validation(txtGiroDate.Text, False)

        Dim strBank As String
        Dim strBankAccCode As String
        Dim strBankAccNo As String
        Dim arrParam As Array

        If ddlBank.SelectedItem.Value = "" Then
            strBank = ""
            strBankAccCode = ""
            strBankAccNo = ""
        Else
            arrParam = Split(Trim(ddlBank.SelectedItem.Value), "|")
            strBank = Trim(arrParam(0))
            strBankAccCode = Trim(arrParam(1))
            strBankAccNo = Trim(arrParam(2))
        End If

        Select Case Trim(Replace(Replace(Replace(lblCtrlAmtFig.Text, ".", ""), ",", "."), "-", ""))
            Case "0.00", "0,00", "0", ""
            Case Else
                lblConfirmErr.Visible = True
                Exit Sub
        End Select

        strStatus = Trim(lblStatus.Text)
        intStatus = Convert.ToInt16(lblStsHid.Text)
        strPrintDate = Trim(lblPrintDate.Text)
        strSortLine = "CB_CASHBANKLN.CashBankLnID"
        strTable = "CB_CASHBANK"
        strCBType = rblCashBankType.SelectedValue

        If strBank = "" Then
            lblErrBank.Visible = True
            Exit Sub
        End If

        'If txtChequeNo.Text = "" Then
        '    lblErrCheque.Visible = True
        '    Exit Sub
        'End If

        'If ddlPayType.SelectedItem.Value = "3" Then
        '    If ddlSupplier.SelectedItem.Value = "" Then
        '    Else
        '        If ddlBankTo.SelectedItem.Value = "" Then
        '            lblerrBankTo.Visible = True
        '            Exit Sub
        '        End If
        '        If txtSplBankAccNo.Text = "" Then
        '            lblErrBankAccNo.Visible = True
        '            Exit Sub
        '        End If
        '    End If
        'End If

        '-- Bank can be edited
        'If Left(Trim(strID), 3) <> Trim(strBank) Then
        '    lblConfirmErr.Text = "Invalid Bank Details! Please check your Bank Details again."
        '    lblConfirmErr.Visible = True
        '    onLoad_Button()
        '    Exit Sub
        'End If

        Dim strOpCd As String = "CB_CLSTRX_CASHBANK_UPDATE"
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim intErrNo As Integer

        strParamName = "CHEQUENO|BANKCODE|REMARK|CHEQUEPRINTDATE|PRINTDATE|LOCCODE|CASHBANKID"

        strParamValue = Trim(txtChequeNo.Text) & _
                        "|" & Trim(strBank) & _
                        "|" & Trim(Replace(txtRemark.Text, "'", "''")) & _
                        "|" & IIf(Left(Trim(lblPaymentID.Text), 3) = "XXX", "", Now()) & _
                        "|" & strGiroDate & _
                        "|" & Trim(strLocation) & _
                        "|" & Trim(strID)

        Try
            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_CASHBANK_PREVIEWCHEQUE_UPD&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_CashBankDet.aspx")
        End Try

        If ddlPayType.SelectedItem.Value = "0" Or ddlPayType.SelectedItem.Value = "1" Then
            strOpCd = "PR_STDRPT_BANK_GET_CHEQUEFMT"
            strParam = Convert.ToString(objHRSetup.EnumBankStatus.Active) & "|" & _
                       Convert.ToString(objHRSetup.EnumBankFormatStatus.Active) & "|" & _
                       Convert.ToString(objHRSetup.EnumBankFormatType.Cheque) & "|" & _
                       strBank
        ElseIf ddlPayType.SelectedItem.Value = "3" Then
            strOpCd = "PR_STDRPT_BANK_GET_BILYETGIROFMT"
            strParam = Convert.ToString(objHRSetup.EnumBankStatus.Active) & "|" & _
                       Convert.ToString(objHRSetup.EnumBankFormatStatus.Active) & "|" & _
                       Convert.ToString(objHRSetup.EnumBankFormatType.BilyetGiro) & "|" & _
                       strBank
        End If


        Try
            intErrNo = objPRRpt.mtdGetBank(strOpCd, strParam, dsForBankCodeDropDown)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_PROGPATH_PREVIEWCHEQUE_GET&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_CashBankDet.aspx")
        End Try

        If dsForBankCodeDropDown.Tables(0).Rows.Count > 0 Then
            strProgramPath = dsForBankCodeDropDown.Tables(0).Rows(0).Item("ProgramPath").Trim()

            lblChequePrintDate.Text = objGlobal.GetLongDate(Now())

            'Response.Write("<Script Language=""JavaScript"">window.open(""../reports/" & strProgramPath & ".aspx?Type=Print&CompName=" & strCompany & _
            '                "&trx=CB_trx_payment&TotalAmount=" & lblTotAmtFig.Text & _
            '                "&SupplierCode=" & "" & "&SupplierName=" & txtPaymentTo.Text & _
            '                "&objMCBPath=" & Server.UrlEncode(objMCBPath) & _
            '                """,null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")

            'If ddlBank.SelectedItem.Value.Trim <> "EKS" Then
            '    Response.Write("<Script Language=""JavaScript"">window.open(""../reports/CB_Rpt_ChequeGiroPrint.aspx?strId=" & lblPaymentID.Text & _
            '               "&strProgramPath=" & strProgramPath & _
            '               "&TRXType=" & "CASHBANK" & _
            '               "&CBType=" & "1" & _
            '               "&strSortLine=" & strSortLine & _
            '               """, null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")
            'Else
            Response.Write("<Script Language=""JavaScript"">window.open(""../reports/CB_Rpt_ChequeGiroPrint.aspx?strId=" & lblPaymentID.Text & _
                         "&strProgramPath=" & strProgramPath & _
                         "&TRXType=" & "CASHBANK" & _
                         "&CBType=" & "1" & _
                         "&strSortLine=" & strSortLine & _
                         "&strCurrencyCode=" & "IDR" & _
                         "&strExchangeRate=" & "1" & _
                         "&strBiaya=" & "0" & _
                         "&strDeduct=" & "0" & _
                         """, null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");window.close();</Script>")
            'End If
        Else

            lblConfirmErr.Text = "Cheque format not found! Please check your Bank Details again."
            lblConfirmErr.Visible = True
            onLoad_Button()

            Exit Sub
        End If
    End Sub

    Sub PreviewSlipBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim dsForBankCodeDropDown As New DataSet()
        Dim strChequeNo As String = txtChequeNo.Text.Trim
        Dim strID As String = lblPaymentID.Text.Trim
        Dim strProgramPath As String = ""
        Dim strStatus As String
        Dim intStatus As Integer
        Dim strSortLine As String
        Dim strPrintDate As String
        Dim strTable As String
        Dim strCBType As String
        Dim strGiroDate As String = Date_Validation(txtGiroDate.Text, False)

        Dim strBank As String
        Dim strBankAccCode As String
        Dim strBankAccNo As String
        Dim arrParam As Array

        If ddlBank.SelectedItem.Value = "" Then
            strBank = ""
            strBankAccCode = ""
            strBankAccNo = ""
        Else
            arrParam = Split(Trim(ddlBank.SelectedItem.Value), "|")
            strBank = Trim(arrParam(0))
            strBankAccCode = Trim(arrParam(1))
            strBankAccNo = Trim(arrParam(2))
        End If

        Select Case Trim(Replace(Replace(Replace(lblCtrlAmtFig.Text, ".", ""), ",", "."), "-", ""))
            Case "0.00", "0,00", "0", ""
            Case Else
                lblConfirmErr.Visible = True
                Exit Sub
        End Select

        strStatus = Trim(lblStatus.Text)
        intStatus = Convert.ToInt16(lblStsHid.Text)
        strPrintDate = Trim(lblPrintDate.Text)
        strSortLine = "CB_CASHBANKLN.CashBankLnID"
        strTable = "CB_CASHBANK"
        strCBType = rblCashBankType.SelectedValue

        If strBank = "" Then
            lblErrBank.Visible = True
            Exit Sub
        End If

        '-- Bank can be edited
        'If Left(Trim(strID), 3) <> Trim(ddlBank.SelectedItem.Value) Then
        '    lblConfirmErr.Text = "Invalid Bank Details! Please check your Bank Details again."
        '    lblConfirmErr.Visible = True
        '    onLoad_Button()
        '    Exit Sub
        'End If

        Dim strOpCd As String = "CB_CLSTRX_CASHBANK_UPDATE"
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim intErrNo As Integer

        strParamName = "CHEQUENO|BANKCODE|REMARK|CHEQUEPRINTDATE|PRINTDATE|LOCCODE|CASHBANKID"

        strParamValue = Trim(txtChequeNo.Text) & _
                        "|" & Trim(strBank) & _
                        "|" & Trim(Replace(txtRemark.Text, "'", "''")) & _
                        "|" & IIf(Left(Trim(lblPaymentID.Text), 3) = "XXX", "", Now()) & _
                        "|" & strGiroDate & _
                        "|" & Trim(strLocation) & _
                        "|" & Trim(strID)

        Try
            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_CASHBANK_PREVIEWCHEQUE_UPD&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_CashBankDet.aspx")
        End Try

        strOpCd = "PR_STDRPT_BANK_GET_SLIPSETORANFMT"
        strParam = Convert.ToString(objHRSetup.EnumBankStatus.Active) & "|" & _
                   Convert.ToString(objHRSetup.EnumBankFormatStatus.Active) & "|" & _
                   Convert.ToString(objHRSetup.EnumBankFormatType.SlipSetoran) & "|" & _
                   ddlBank.SelectedItem.Value.Trim

        Try
            intErrNo = objPRRpt.mtdGetBank(strOpCd, strParam, dsForBankCodeDropDown)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_PROGPATH_PREVIEWCHEQUE_GET&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_CashBankDet.aspx")
        End Try

        If dsForBankCodeDropDown.Tables(0).Rows.Count > 0 Then
            strProgramPath = dsForBankCodeDropDown.Tables(0).Rows(0).Item("ProgramPath").Trim()

            lblChequePrintDate.Text = objGlobal.GetLongDate(Now())

            'If ddlBank.SelectedItem.Value.Trim <> "EKS" Then
            '    Response.Write("<Script Language=""JavaScript"">window.open(""../reports/CB_Rpt_ChequeGiroPrint.aspx?strId=" & lblPaymentID.Text & _
            '               "&strProgramPath=" & strProgramPath & _
            '               "&TRXType=" & "CASHBANK" & _
            '               "&CBType=" & "2" & _
            '               "&strSortLine=" & strSortLine & _
            '               """, null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")
            'Else
            Response.Write("<Script Language=""JavaScript"">window.open(""../reports/CB_Rpt_ChequeGiroPrint.aspx?strId=" & lblPaymentID.Text & _
                         "&strProgramPath=" & strProgramPath & _
                         "&TRXType=" & "CASHBANK" & _
                         "&CBType=" & "2" & _
                         "&strSortLine=" & strSortLine & _
                         "&strCurrencyCode=" & "IDR" & _
                         "&strExchangeRate=" & "1" & _
                         "&strBiaya=" & "0" & _
                         "&strDeduct=" & "0" & _
                         """, null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");window.close();</Script>")
            'End If
        Else
            lblConfirmErr.Text = "Slip Setoran format not found! Please check your Bank Details again."
            lblConfirmErr.Visible = True
            onLoad_Button()

            Exit Sub
        End If
    End Sub

    Sub PreviewTransferBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim dsForBankCodeDropDown As New DataSet()
        Dim strChequeNo As String = txtChequeNo.Text.Trim
        Dim strID As String = lblPaymentID.Text.Trim
        Dim strProgramPath As String = ""

        Dim strStatus As String
        Dim intStatus As Integer
        Dim strSortLine As String
        Dim strPrintDate As String
        Dim strTable As String
        Dim strCBType As String
        Dim strGiroDate As String = Date_Validation(txtGiroDate.Text, False)

        Dim strBank As String
        Dim strBankAccCode As String
        Dim strBankAccNo As String
        Dim arrParam As Array

        If ddlBank.SelectedItem.Value = "" Then
            strBank = ""
            strBankAccCode = ""
            strBankAccNo = ""
        Else
            arrParam = Split(Trim(ddlBank.SelectedItem.Value), "|")
            strBank = Trim(arrParam(0))
            strBankAccCode = Trim(arrParam(1))
            strBankAccNo = Trim(arrParam(2))
        End If

        Select Case Trim(Replace(Replace(Replace(lblCtrlAmtFig.Text, ".", ""), ",", "."), "-", ""))
            Case "0.00", "0,00", "0", ""
            Case Else
                lblConfirmErr.Visible = True
                Exit Sub
        End Select

        strStatus = Trim(lblStatus.Text)
        intStatus = Convert.ToInt16(lblStsHid.Text)
        strPrintDate = Trim(lblPrintDate.Text)
        strSortLine = "CB_CASHBANKLN.CashBankLnID"
        strTable = "CB_CASHBANK"
        strCBType = rblCashBankType.SelectedValue

        If ddlBank.SelectedItem.Value = "" Then
            lblErrBank.Visible = True
            Exit Sub
        End If

        If ddlBankTo.SelectedItem.Value = "" Then
            lblerrBankTo.Visible = True
            Exit Sub
        End If

        '-- Bank can be edited
        'If Left(Trim(strID), 3) <> Trim(ddlBank.SelectedItem.Value) Then
        '    lblConfirmErr.Text = "Invalid Bank Details! Please check your Bank Details again."
        '    lblConfirmErr.Visible = True
        '    onLoad_Button()
        '    Exit Sub
        'End If

        Dim strOpCd As String = "CB_CLSTRX_CASHBANK_UPDATE"
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim intErrNo As Integer

        strParamName = "CHEQUENO|BANKCODE|REMARK|CHEQUEPRINTDATE|PRINTDATE|LOCCODE|CASHBANKID"

        strParamValue = Trim(txtChequeNo.Text) & _
                        "|" & Trim(strBank) & _
                        "|" & Trim(Replace(txtRemark.Text, "'", "''")) & _
                        "|" & IIf(Left(Trim(lblPaymentID.Text), 3) = "XXX", "", Now()) & _
                        "|" & strGiroDate & _
                        "|" & Trim(strLocation) & _
                        "|" & Trim(strID)

        Try
            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_CASHBANK_PREVIEWCHEQUE_UPD&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_CashBankDet.aspx")
        End Try

        strOpCd = "PR_STDRPT_BANK_GET_SLIPTRANSFERFMT"
        strParam = Convert.ToString(objHRSetup.EnumBankStatus.Active) & "|" & _
                   Convert.ToString(objHRSetup.EnumBankFormatStatus.Active) & "|" & _
                   Convert.ToString(objHRSetup.EnumBankFormatType.SlipTransfer) & "|" & _
                   ddlBank.SelectedItem.Value.Trim

        Try
            intErrNo = objPRRpt.mtdGetBank(strOpCd, strParam, dsForBankCodeDropDown)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_PROGPATH_PREVIEWCHEQUE_GET&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_CashBankDet.aspx")
        End Try

        If dsForBankCodeDropDown.Tables(0).Rows.Count > 0 Then
            strProgramPath = dsForBankCodeDropDown.Tables(0).Rows(0).Item("ProgramPath").Trim()

            lblChequePrintDate.Text = objGlobal.GetLongDate(Now())

            'If ddlBank.SelectedItem.Value.Trim <> "EKS" Then
            '    Response.Write("<Script Language=""JavaScript"">window.open(""../reports/CB_Rpt_ChequeGiroPrint.aspx?strId=" & lblPaymentID.Text & _
            '               "&strProgramPath=" & strProgramPath & _
            '               "&TRXType=" & "CASHBANK" & _
            '               "&CBType=" & "3" & _
            '               "&strSortLine=" & strSortLine & _
            '               """, null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")
            'Else
            Response.Write("<Script Language=""JavaScript"">window.open(""../reports/CB_Rpt_ChequeGiroPrint.aspx?strId=" & lblPaymentID.Text & _
                         "&strProgramPath=" & strProgramPath & _
                         "&TRXType=" & "CASHBANK" & _
                         "&CBType=" & "3" & _
                         "&strSortLine=" & strSortLine & _
                         "&strCurrencyCode=" & "IDR" & _
                         "&strExchangeRate=" & "1" & _
                         "&strBiaya=" & "0" & _
                         "&strDeduct=" & "0" & _
                         """, null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");window.close();</Script>")
            'End If
        Else

            lblConfirmErr.Text = "Slip Transfer format not found! Please check your Bank Details again."
            lblConfirmErr.Visible = True
            onLoad_Button()
            Exit Sub
        End If
    End Sub

    Sub DeleteBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Update_Status(objCBTrx.EnumCashBankStatus.Deleted)
        LoadTxDetails()
        DisplayFromDB()
        onLoad_Button()
    End Sub

    Sub UnDeleteBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Update_Status(objCBTrx.EnumCashBankStatus.Active)
        LoadTxDetails()
        DisplayFromDB()
        onLoad_Button()
    End Sub

    Sub BackBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Response.Redirect("CB_trx_CashBankList.aspx")
    End Sub

    Sub CancelBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Update_Status(objCBTrx.EnumCashBankStatus.Cancelled)
        LoadTxDetails()
        DisplayFromDB()
        onLoad_Button()
    End Sub

    Private Sub Update_Status(ByVal pv_intStatus As Integer)
        Dim strTxParam As String
        Dim strDocPrefix As String = ""
        Dim TxResultID As String
        Dim ErrorChk As Integer = 0
        Dim strRemark As String = Trim(Replace(txtRemark.Text, "'", "''"))
        Dim strDate As String = Date_Validation(txtDateCreated.Text, False)
        Dim indDate As String = ""

        Dim strBank As String
        Dim strBankAccCode As String
        Dim strBankAccNo As String
        Dim arrParam As Array

        If ddlBank.SelectedItem.Value = "" Then
            strBank = ""
            strBankAccCode = ""
            strBankAccNo = ""
        Else
            arrParam = Split(Trim(ddlBank.SelectedItem.Value), "|")
            strBank = Trim(arrParam(0))
            strBankAccCode = Trim(arrParam(1))
            strBankAccNo = Trim(arrParam(2))
        End If


        If CheckDate(txtDateCreated.Text.Trim(), indDate) = False Then
            lblDate.Visible = True
            lblFmt.Visible = True
            lblDate.Text = "<br>Date Entered should be in the format"
            Exit Sub
        End If

        Dim intInputPeriod As Integer = Year(strDate) * 100 + Month(strDate)
        Dim intCurPeriod As Integer = (CInt(strAccYear) * 100) + CInt(strAccMonth)
        Dim intSelPeriod As Integer = (CInt(strSelAccYear) * 100) + CInt(strSelAccMonth)

        If Session("SS_FILTERPERIOD") = "0" Then
            If intCurPeriod < intInputPeriod Then
                lblDate.Visible = True
                lblDate.Text = "Invalid transaction date."
                Exit Sub
            End If
        Else
            If intSelPeriod <> intInputPeriod Then
                lblDate.Visible = True
                lblDate.Text = "Invalid transaction date."
                Exit Sub
            End If
            If intSelPeriod < intCurPeriod And intLevel < 2 Then
                lblDate.Visible = True
                lblDate.Text = "This period already locked."
                Exit Sub
            End If
        End If

        If txtPaymentTo.Text = "" Then
            lblConfirmErr.Visible = True
            lblConfirmErr.Text = "Payment To/Receive From cannot be empty."
            Exit Sub
        End If

        If lblPaymentID.Text <> "" And Left(lblPaymentID.Text.Trim, 3) <> "XXX" Then
            If txtDateCreated.Enabled = True Then
                If hidPrevID.Value = "" Then
                    If hidNextID.Value <> "" Then
                        'If Day(strDate) > hidNextDate.Value Then
                        '    lblDate.Visible = True
                        '    lblDate.Text = "Date cannot be higher than last transaction date."
                        '    Exit Sub
                        'End If
                        hidNextDate.Visible = False
                    End If
                Else
                    ''remark sementara
                    'If hidPrevID.Value <> "" Then
                    '    If Day(strDate) < hidPrevDate.Value Then
                    '        lblDate.Visible = True
                    '        lblDate.Text = "Date cannot be smaller than previous transaction date."
                    '        Exit Sub
                    '    End If
                    '    If hidNextID.Value <> "" Then
                    '        'If Day(strDate) > hidNextDate.Value Then
                    '        '    lblDate.Visible = True
                    '        '    lblDate.Text = "Date cannot be higher than last transaction date."
                    '        '    Exit Sub
                    '        'End If
                    '        hidNextDate.Visible = False
                    '    End If
                    'Else
                    '    If Day(strDate) < hidPrevDate.Value Then
                    '        lblDate.Visible = True
                    '        lblDate.Text = "Date cannot be smaller than previous transaction date."
                    '        Exit Sub
                    '    End If
                    'End If
                End If
            End If
        End If

        'If lblPaymentID.Text = "" Then
        '    If txtDateCreated.Text.Trim() = "" Then
        '        lblFmt.Text = "Please enter Date Created"
        '        lblFmt.Visible = True
        '        'pr_intSuccess = 0
        '        Exit Sub
        '    ElseIf CheckDate(txtDateCreated.Text.Trim(), strDate) = False Then
        '        lblDate.Visible = True
        '        lblFmt.Visible = True
        '        lblDate.Text = "<br>Date Entered should be in the format"
        '        'pr_intSuccess = 0
        '        Exit Sub
        '    End If
        'End If

        If pv_intStatus = objCBTrx.EnumCashBankStatus.Confirmed Then
            Select Case Trim(Replace(Replace(Replace(lblCtrlAmtFig.Text, ".", ""), ",", "."), "-", ""))
                Case "0.00", "0,00", "0", ""
                Case Else
                    lblConfirmErr.Visible = True
                    Exit Sub
            End Select
        End If

        strTxParam = lblPaymentID.Text & "|||" & strBank & "|" & IIf(Trim(txtChequeNo.Text) = "", "", Replace(Trim(txtChequeNo.Text), "'", "''")) & _
                                  "|" & IIf(Trim(txtPaymentTo.Text) = "", "", Trim(txtPaymentTo.Text)) & "|" & strLocation & _
                                  "|||" & pv_intStatus & "||||" & strUserId & "||" & "" & _
                                  "|" & IIf(Trim(ddlSplBankAccNo.SelectedItem.Value) = "", "", Replace(ddlSplBankAccNo.SelectedItem.Value, "'", "''")) & "|" & strRemark & _
                                  "|" & ddlBankTo.SelectedValue & "|" & txtSupCode.Text & "|" & CInt(hidTaxStatus.Value) & _
                                  "|" & strBankAccNo & "|" & IIf(chkChequeCash.Checked = True, "1", "2") & "|" & ddlChequeHandOver.SelectedItem.Value

        Try

            intErrNo = objCBTrx.mtdUpdCashBankDetail(strOpCdTxDet_ADD, _
                                                        strOpCdTxDet_UPD, _
                                                        strUserId, _
                                                        strTxParam, _
                                                        strDocPrefix, _
                                                        ErrorChk, _
                                                        TxResultID)
            lblPaymentID.Text = TxResultID
            If ErrorChk = 1 Then
                lblerror.Visible = True
            End If

        Catch Exp As System.Exception
            If intErrNo <> -5 Then
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADD_ADDNEWCASHBANK&errmesg=" & lblErrMessage.Text & "&redirect=CB/trx/CB_trx_CashBankList.aspx")
            End If
        End Try

        UpdateCBLine()
        'LoadTxDetails()
        'DisplayFromDB()
        'onLoad_Button()

    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()

        Try
            If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
                strBlockTag = GetCaption(objLangCap.EnumLangCap.Block) & lblCode.Text
                BlockTag = GetCaption(objLangCap.EnumLangCap.Block)
            Else
                strBlockTag = GetCaption(objLangCap.EnumLangCap.SubBlock) & lblCode.Text
                BlockTag = GetCaption(objLangCap.EnumLangCap.SubBlock)
            End If

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_CASHBANK_GET_COSTLEVEL_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=CB/trx/CB_trx_CashBankDet.aspx")
        End Try

        strAccountTag = GetCaption(objLangCap.EnumLangCap.Account) & lblCode.Text
        strVehTag = GetCaption(objLangCap.EnumLangCap.Vehicle) & lblCode.Text
        strVehExpTag = GetCaption(objLangCap.EnumLangCap.VehExpense) & lblCode.Text

        lblAccCodeTag.Text = strAccountTag & " :* "
        lblBlkTag.Text = strBlockTag & " : "
        lblVehTag.Text = strVehTag & " : "
        lblVehExpTag.Text = strVehExpTag & " :"

        lblAccCodeErr.Text = "<BR>" & lblPleaseSelect.Text & strAccountTag
        lblVehCodeErr.Text = lblPleaseSelect.Text & strVehTag
        lblBlockErr.Text = lblPleaseSelect.Text & strBlockTag
        lblVehExpCodeErr.Text = lblPleaseSelect.Text & strVehExpTag

        dgTx.Columns(2).HeaderText = strAccountTag
        dgTx.Columns(4).HeaderText = "Charge To" & "<BR>" & strBlockTag
        dgTx.Columns(5).HeaderText = "Cheque/Giro No"
        'dgTx.Columns(5).HeaderText = strVehTag & "<BR>" & strVehExpTag
        'dgTx.Columns(6).HeaderText = strVehExpTag

        PreBlockTag = GetCaption(objLangCap.EnumLangCap.Block)
        lblPreBlkTag.Text = PreBlockTag & lblCode.Text & " : "
        lblPreBlockErr.Text = lblPleaseSelect.Text & PreBlockTag & lblCode.Text
        lblLocationErr.Text = lblPleaseSelect.Text & GetCaption(objLangCap.EnumLangCap.Location) & lblCode.Text
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GL_TRX_JOURNAL_DETAIL_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=CB/CB_trx_CashBankDet.aspx")
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

    Function CheckDate(ByVal pv_strDate As String, ByRef pr_strDate As String) As Boolean
        Dim strDateFormatCode As String = Session("SS_DATEFMT")
        Dim strDateFormat As String

        pr_strDate = ""
        CheckDate = True
        If Not pv_strDate = "" Then
            If objGlobal.mtdValidInputDate(strDateFormatCode, pv_strDate, strDateFormat, pr_strDate) = False Then
                lblFmt.Text = strDateFormat
                lblFmtGiro.Text = strDateFormat
                pr_strDate = ""
                CheckDate = False
            End If
        End If
    End Function

    Sub NewCBBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CB_trx_CashBankDet.aspx")
    End Sub

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

    Sub GetDataBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim strOpCode As String = "PU_CLSSETUP_SUPPLIER_GET"
        Dim intErrNo As Integer
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim strSplCode As String
        Dim dsMaster As Object

        strParamName = "STRSEARCH"
        strParamValue = " And A.SupplierCode = '" & Trim(txtSupCode.Text) & "'"

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
                                                strParamName, _
                                                strParamValue, _
                                                dsMaster)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=IN_ITEM_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        If dsMaster.Tables(0).Rows.Count > 0 Then
            txtPaymentTo.Text = Trim(dsMaster.Tables(0).Rows(0).Item("Name"))
            txtSplBankAccNo.Text = Trim(dsMaster.Tables(0).Rows(0).Item("BankAccNo"))
            hidNPWPNo.Value = Trim(dsMaster.Tables(0).Rows(0).Item("NPWPNo"))
            'BindBankTo(dsMaster.Tables(0).Rows(0).Item("BankCode"))
            hidFFBSpl.Value = Trim(dsMaster.Tables(0).Rows(0).Item("SuppCat"))
            BindSplBankAccNo(Trim(txtSupCode.Text), "")
            LblIsSKBActive.Text = Trim(dsMaster.Tables(0).Rows(0).Item("SKBIsActivation"))
            lblSKBStartDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), dsMaster.Tables(0).Rows(0).Item("SKBDate"))
            BindRefNo("")
            'BindAccCodeDropList("")
        End If
    End Sub

    Sub GetSupplierBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim strOpCode As String = "PU_CLSSETUP_SUPPLIER_GET"
        Dim intErrNo As Integer
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim dsMaster As Object
        Dim strSuplierCode As String

        strParamName = "STRSEARCH"
        If txtPaymentTo.Text = "" Then
            strParamValue = ""
        Else
            strParamValue = IIf(Session("SS_COACENTRALIZED") = "1", "", " AND A.AccCode in (SELECT AccCode FROM GL_Account WHERE LocCode='" & Trim(strLocation) & "') ")
            strParamValue = strParamValue & " And A.Name Like '" & Replace(txtPaymentTo.Text, "'", "''") & "%' And A.Status = '" & objPUSetup.EnumSuppStatus.Active & "'"
        End If

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
                                                strParamName, _
                                                strParamValue, _
                                                dsMaster)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=IN_ITEM_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        If dsMaster.Tables(0).Rows.Count = 1 Then
            txtPaymentTo.Text = Trim(dsMaster.Tables(0).Rows(0).Item("Name"))
            txtSplBankAccNo.Text = Trim(dsMaster.Tables(0).Rows(0).Item("BankAccNo"))
            strSuplierCode = Trim(dsMaster.Tables(0).Rows(0).Item("SupplierCode"))
            BindBankTo(dsMaster.Tables(0).Rows(0).Item("BankCode"))
            BindRefNo("")
            txtSupCode.Text = strSuplierCode
            'BindSupplier(strSuplierCode)
        Else
            txtSplBankAccNo.Text = ""
            txtSupCode.Text = ""
            ddlBankTo.SelectedValue = ""
        End If

    End Sub

    Sub GetSupplier(ByVal pv_supplier As String)

        Dim strOpCode As String = "PU_CLSSETUP_SUPPLIER_GET"
        Dim intErrNo As Integer
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim dsMaster As Object
        Dim strSuplierCode As String

        strParamName = "STRSEARCH"
        If txtPaymentTo.Text = "" Then
            strParamValue = ""
        Else
            strParamValue = IIf(Session("SS_COACENTRALIZED") = "1", "", " AND A.AccCode in (SELECT AccCode FROM GL_Account WHERE LocCode='" & Trim(strLocation) & "') ")
            strParamValue = strParamValue & " And A.Name Like '" & Replace(txtPaymentTo.Text, "'", "''") & "%' And A.Status = '" & objPUSetup.EnumSuppStatus.Active & "'"
        End If

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
                                                strParamName, _
                                                strParamValue, _
                                                dsMaster)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=IN_ITEM_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        If dsMaster.Tables(0).Rows.Count = 1 Then
            txtPaymentTo.Text = Trim(dsMaster.Tables(0).Rows(0).Item("Name"))
            txtSplBankAccNo.Text = Trim(dsMaster.Tables(0).Rows(0).Item("BankAccNo"))
            strSuplierCode = Trim(dsMaster.Tables(0).Rows(0).Item("SupplierCode"))
            BindBankTo(dsMaster.Tables(0).Rows(0).Item("BankCode"))
            BindRefNo("")
            txtSupCode.Text = strSuplierCode
            'BindSupplier(strSuplierCode)
        Else
            txtSplBankAccNo.Text = ""
            txtSupCode.Text = ""
            ddlBankTo.SelectedValue = ""
        End If
    End Sub

    'Sub onSelect_Supplier(ByVal Sender As System.Object, ByVal E As System.EventArgs)

    '    Dim strOpCode As String = "PU_CLSSETUP_SUPPLIER_GET"
    '    Dim intErrNo As Integer
    '    Dim strParamName As String = ""
    '    Dim strParamValue As String = ""
    '    Dim strSplCode As String
    '    Dim dsMaster As Object

    '    strParamName = "STRSEARCH"
    '    strParamValue = " And A.SupplierCode = '" & Trim(txtSupCode.Text) & "'"

    '    Try
    '        intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
    '                                            strParamName, _
    '                                            strParamValue, _
    '                                            dsMaster)
    '    Catch Exp As System.Exception
    '        Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=IN_ITEM_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
    '    End Try

    '    If dsMaster.Tables(0).Rows.Count > 0 Then
    '        txtPaymentTo.Text = Trim(dsMaster.Tables(0).Rows(0).Item("Name"))
    '        txtSplBankAccNo.Text = Trim(dsMaster.Tables(0).Rows(0).Item("BankAccNo"))
    '        hidNPWPNo.Value = Trim(dsMaster.Tables(0).Rows(0).Item("NPWPNo"))
    '        'BindBankTo(dsMaster.Tables(0).Rows(0).Item("BankCode"))
    '        hidFFBSpl.Value = Trim(dsMaster.Tables(0).Rows(0).Item("SuppCat"))
    '        BindSplBankAccNo(Trim(txtSupCode.Text), "")
    '        LblIsSKBActive.Text = Trim(dsMaster.Tables(0).Rows(0).Item("SKBIsActivation"))
    '        lblSKBStartDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), dsMaster.Tables(0).Rows(0).Item("SKBDate"))
    '        BindRefNo("")
    '        'BindAccCodeDropList("")
    '    End If

    'End Sub

    Sub BindSplBankAccNo(ByVal pv_strSplCode As String, Optional ByVal pv_strBankAccNo As String = "")
        Dim strOpCd As String = "PU_CLSSETUP_SUPPLIERBANK_GET"
        Dim intErrNo As Integer
        Dim strParamName As String
        Dim strParamValue As String
        Dim intCnt As Integer = 0
        Dim intBankAccIndex As Integer = 0
        Dim dsMaster As Object

        strParamName = "SUPPLIERCODE"
        strParamValue = Trim(pv_strSplCode)

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCd, _
                                                strParamName, _
                                                strParamValue, _
                                                dsMaster)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=TX_Setup_TaxObjectDet&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To dsMaster.Tables(0).Rows.Count - 1
            dsMaster.Tables(0).Rows(intCnt).Item("BankAccNo") = dsMaster.Tables(0).Rows(intCnt).Item("BankAccNo").Trim()
            dsMaster.Tables(0).Rows(intCnt).Item("SplBankDescr") = dsMaster.Tables(0).Rows(intCnt).Item("SplBankDescr")
            If Trim(dsMaster.Tables(0).Rows(intCnt).Item("BankAccNo")) = Trim(pv_strBankAccNo) Then
                intBankAccIndex = intCnt + 1
            End If
        Next intCnt

        If dsMaster.Tables(0).Rows.Count = 1 Then
            intBankAccIndex = 1
        End If

        Dim dr As DataRow
        dr = dsMaster.Tables(0).NewRow()
        dr("BankAccNo") = ""
        dr("SplBankDescr") = lblPleaseSelect.Text & " Bank Account Number"
        dsMaster.Tables(0).Rows.InsertAt(dr, 0)

        ddlSplBankAccNo.DataSource = dsMaster.Tables(0)
        ddlSplBankAccNo.DataValueField = "BankAccNo"
        ddlSplBankAccNo.DataTextField = "SplBankDescr"
        ddlSplBankAccNo.DataBind()
        ddlSplBankAccNo.SelectedIndex = intBankAccIndex

        If lblPaymentID.Text = "" Then
            Dim arrParam As Array
            arrParam = Split(ddlSplBankAccNo.SelectedItem.Text, " ")
            BindBankTo(arrParam(0))
        End If
    End Sub

    Sub onSelect_SplBankAccNo(ByVal Sender As System.Object, ByVal E As System.EventArgs)
        Dim arrParam As Array
        arrParam = Split(ddlSplBankAccNo.SelectedItem.Text, " ")
        BindBankTo(arrParam(0))
        'txtSplBankAccNo.Text = Replace(Trim(arrParam(1)), ")", "")
    End Sub

    'Sub BindRefNo(ByVal pv_strRefNo As String)
    '    Dim strOpCode As String = "CB_CLSTRX_CASHBANKTEMP_REFERENCENO_GET"
    '    Dim intErrNo As Integer
    '    Dim strParamName As String = ""
    '    Dim strParamValue As String = ""
    '    Dim dsMaster As Object
    '    Dim intCnt As Integer
    '    Dim intSelectedIndex As Integer = 0

    '    strParamName = "STRSEARCH"

    '    If txtSupCode.Text = "" Or txtPaymentTo.Text = "" Then
    '        strParamValue = " And SPL.AccCode IN (SELECT AccCode FROM GL_Account " & IIf(Session("SS_COACENTRALIZED") = "1", ")", "WHERE LocCode = '" & Trim(strLocation) & "') ") & _
    '                        " And CB.Status Not In ('" & objCBTrx.EnumCashBankStatus.Cancelled & "','" & objCBTrx.EnumCashBankStatus.Deleted & "')" & _
    '                        " And CB.LocCode = '" & Trim(strLocation) & "' And (CB.FromTo = '" & txtSupCode.Text & "' Or CB.FromTo Like '" & Replace(txtPaymentTo.Text, "'", "''") & "') " & _
    '                        " And CB.CashBankID Not In (Select Distinct RefNo From CB_CashBankLn ln, CB_CashBank cb Where cb.CashBankID=ln.CashBankID And cb.Status Not In ('" & objCBTrx.EnumCashBankStatus.Cancelled & "','" & objCBTrx.EnumCashBankStatus.Deleted & "'))"
    '    Else
    '        'If ddlSupplier.SelectedItem.Value = "" Or txtPaymentTo.Text = "" Then
    '        '    strParamValue = " And CB.LocCode = '" & Trim(strLocation) & "' And (CB.FromTo = '" & ddlSupplier.SelectedItem.Value & "' Or CB.FromTo Like '" & txtPaymentTo.Text & "%') "
    '        'Else
    '        'If Trim(lblPaymentID.Text) = "" Then
    '        strParamValue = " And SPL.AccCode IN (SELECT AccCode FROM GL_Account " & IIf(Session("SS_COACENTRALIZED") = "1", ")", "WHERE LocCode = '" & Trim(strLocation) & "') ") & _
    '                        " And CB.Status Not In ('" & objCBTrx.EnumCashBankStatus.Cancelled & "','" & objCBTrx.EnumCashBankStatus.Deleted & "')" & _
    '                        " And CB.LocCode = '" & Trim(strLocation) & "' And (CB.FromTo = '" & txtSupCode.Text & "' Or CB.FromTo Like '" & Replace(txtPaymentTo.Text, "'", "''") & "%') " & _
    '                        " And CB.CashBankID Not In (Select Distinct RefNo From CB_CashBankLn ln, CB_CashBank cb Where cb.CashBankID=ln.CashBankID And cb.Status Not In ('" & objCBTrx.EnumCashBankStatus.Cancelled & "','" & objCBTrx.EnumCashBankStatus.Deleted & "'))"
    '        'Else
    '        '    strParamValue = " And CB.LocCode = '" & Trim(strLocation) & "' And (CB.FromTo = '" & ddlSupplier.SelectedItem.Value & "' Or CB.FromTo Like '" & txtPaymentTo.Text & "%') " & _
    '        '                    " And CB.CashBankID Not In (Select Distinct RefNo From CB_CashBankLn ln, CB_CashBank cb Where cb.CashBankID=ln.CashBankID And cb.Status Not In ('" & objCBTrx.EnumCashBankStatus.Cancelled & "','" & objCBTrx.EnumCashBankStatus.Deleted & "') And cb.CashBankID = '" & Trim(lblPaymentID.Text) & "') "
    '        'End If

    '        'End If
    '    End If

    '    Try
    '        intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
    '                                            strParamName, _
    '                                            strParamValue, _
    '                                            dsMaster)
    '    Catch Exp As System.Exception
    '        Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=IN_ITEM_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
    '    End Try

    '    For intCnt = 0 To dsMaster.Tables(0).Rows.Count - 1
    '        dsMaster.Tables(0).Rows(intCnt).Item("CashBankID") = dsMaster.Tables(0).Rows(intCnt).Item("CashBankID").Trim()
    '        dsMaster.Tables(0).Rows(intCnt).Item("Description") = dsMaster.Tables(0).Rows(intCnt).Item("CashBankID") & " (" & dsMaster.Tables(0).Rows(intCnt).Item("Total") & ")"
    '        If dsMaster.Tables(0).Rows(intCnt).Item("CashBankID") = pv_strRefNo Then
    '            intSelectedIndex = intCnt + 1
    '            lblStatusInput.Text = "2"
    '        End If
    '    Next intCnt

    '    Dim dr As DataRow
    '    dr = dsMaster.Tables(0).NewRow()
    '    dr("CashBankID") = ""
    '    dr("Description") = lblPleaseSelect.Text & " Reference No."
    '    dsMaster.Tables(0).Rows.InsertAt(dr, 0)

    '    ddlRefNo.DataSource = dsMaster.Tables(0)
    '    ddlRefNo.DataValueField = "CashBankID"
    '    ddlRefNo.DataTextField = "Description"
    '    ddlRefNo.DataBind()
    '    ddlRefNo.SelectedIndex = intSelectedIndex
    '    ddlRefNo.AutoPostBack = True

    'End Sub

    Sub BindRefNo(ByVal pv_strRefNo As String)
        Dim strOpCode As String = "HR_CLSSETUP_STAFF_GET"
        Dim intErrNo As Integer
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim dsMaster As Object
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer = 0

        strParamName = "STRSEARCH"
        strParamValue = " AND A.Status='1' "

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
                                                strParamName, _
                                                strParamValue, _
                                                dsMaster)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=IN_ITEM_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        If pv_strRefNo <> "" Then
            For intCnt = 0 To dsMaster.Tables(0).Rows.Count - 1
                If Trim(pv_strRefNo) = Trim(dsMaster.Tables(0).Rows(intCnt).Item("StaffID")) Then
                    intSelectedIndex = intCnt + 1
                End If
            Next intCnt
        End If
        
        Dim dr As DataRow
        dr = dsMaster.Tables(0).NewRow()
        dr("StaffID") = ""
        dr("Name") = lblPleaseSelect.Text & " reference/staff advance"
        dsMaster.Tables(0).Rows.InsertAt(dr, 0)

        ddlRefNo.DataSource = dsMaster.Tables(0)
        ddlRefNo.DataValueField = "StaffID"
        ddlRefNo.DataTextField = "Name"
        ddlRefNo.DataBind()
        ddlRefNo.SelectedIndex = intSelectedIndex
        ddlRefNo.AutoPostBack = True

    End Sub

    Sub BindRefNoTrx(ByVal pv_strRefNo As String)
        Dim strOpCode As String = "HR_CLSSETUP_STAFF_GET_TRX"
        Dim intErrNo As Integer
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim dsMaster As Object
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer = 0

        strParamName = "LOCCODE|STRSEARCH"
        If pv_strRefNo = "" Then
            strParamValue = strLocation & "|" & " AND A.Status='1' AND Outstanding > 0"
        Else
            strParamValue = strLocation & "|" & " AND A.Status='1' AND Outstanding >= 0"
        End If

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
                                                strParamName, _
                                                strParamValue, _
                                                dsMaster)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=IN_ITEM_GET&errmesg=" & Exp.Message.ToString & "&redirect=")
        End Try

        For intCnt = 0 To dsMaster.Tables(0).Rows.Count - 1
            dsMaster.Tables(0).Rows(intCnt).Item("RefNo") = dsMaster.Tables(0).Rows(intCnt).Item("RefNo").Trim()
            dsMaster.Tables(0).Rows(intCnt).Item("Name") = Trim(dsMaster.Tables(0).Rows(intCnt).Item("Name")) & ", " & Trim(dsMaster.Tables(0).Rows(intCnt).Item("StaffAdvDoc")) & ", Rp " & objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(dsMaster.Tables(0).Rows(intCnt).Item("Outstanding"), CInt(Session("SS_ROUNDNO"))), CInt(Session("SS_ROUNDNO")))
            If Trim(pv_strRefNo) = Trim(dsMaster.Tables(0).Rows(intCnt).Item("RefNo")) Then
                intSelectedIndex = intCnt + 1
            End If
        Next intCnt

        Dim dr As DataRow
        dr = dsMaster.Tables(0).NewRow()
        dr("RefNo") = ""
        dr("Name") = lblPleaseSelect.Text & " reference/staff advance"
        dsMaster.Tables(0).Rows.InsertAt(dr, 0)

        ddlRefNo.DataSource = dsMaster.Tables(0)
        ddlRefNo.DataValueField = "RefNo"
        ddlRefNo.DataTextField = "Name"
        ddlRefNo.DataBind()
        ddlRefNo.SelectedIndex = intSelectedIndex
        ddlRefNo.AutoPostBack = True

    End Sub

    Sub OnSelect_RefNo(ByVal sender As Object, ByVal e As EventArgs)
        'If ddlRefNo.SelectedItem.Value = "" Then
        '    btnGetRef.Visible = False
        '    AddDtlBtn.Visible = True
        'Else
        '    btnGetRef.Visible = True
        '    AddDtlBtn.Visible = False
        'End If
    End Sub

    Sub GetRefNoBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim strParamList As String
        Dim strStatus As String
        Dim TxResultID As String
        Dim strTxParam As String
        Dim ErrorChk As Integer = 0
        Dim strDocPrefix As String
        Dim strDocPrefixLn As String
        Dim strOpCodeID As String
        Dim dblQty As Double = 1
        Dim dblPrice As Double = 0
        Dim dblTotal As Double = 0
        Dim strDate As String = Date_Validation(txtDateCreated.Text, False)
        Dim strGiroDate As String = Date_Validation(txtGiroDate.Text, False)
        Dim strNewIDFormat As String
        Dim strInOrOut As String
        Dim strRemark As String = Trim(Replace(txtRemark.Text, "'", "''"))
        Dim strInitial As String = ""

        Dim strBank As String
        Dim strBankAccCode As String
        Dim strBankAccNo As String
        Dim arrParam As Array

        If ddlBank.SelectedItem.Value = "" Then
            strBank = ""
            strBankAccCode = ""
            strBankAccNo = ""
        Else
            arrParam = Split(Trim(ddlBank.SelectedItem.Value), "|")
            strBank = Trim(arrParam(0))
            strBankAccCode = Trim(arrParam(1))
            strBankAccNo = Trim(arrParam(2))
        End If


        If ddlPayType.SelectedItem.Value = "0" Or ddlPayType.SelectedItem.Value = "3" Then
            If ddlBank.SelectedItem.Value = "" Then
                lblErrBank.Visible = True
                Exit Sub
            End If
            'If txtChequeNo.Text = "" Then
            '    lblErrCheque.Visible = True
            '    Exit Sub
            'End If
        End If

        If rblCashBankType.SelectedValue = objCBTrx.EnumCashBankType.Payment Then
            strDocPrefix = objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CBPaymentOther)
            strDocPrefixLn = objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CBPaymentOtherLn)
        Else
            strDocPrefix = objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CBReceiptOther)
            strDocPrefixLn = objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CBReceiptOtherLn)
        End If

        If lblPaymentID.Text <> "" Then
            Select Case ddlPayType.SelectedItem.Value
                Case 0
                    Select Case rblCashBankType.SelectedValue
                        Case objCBTrx.EnumCashBankType.Payment
                            strInitial = "BBK"
                        Case objCBTrx.EnumCashBankType.Receipt
                            strInitial = "BBT"
                        Case objCBTrx.EnumCashBankType.Transfer
                            strInitial = "STF"
                    End Select

                Case 1
                    Select Case rblCashBankType.SelectedValue
                        Case objCBTrx.EnumCashBankType.Payment
                            strInitial = "KKK"
                        Case objCBTrx.EnumCashBankType.Receipt
                            strInitial = "KKT"
                    End Select

                Case 2
                    strInitial = "XXX"

                Case 3
                    Select Case rblCashBankType.SelectedValue
                        Case objCBTrx.EnumCashBankType.Payment
                            strInitial = "BBK"
                        Case objCBTrx.EnumCashBankType.Receipt
                            strInitial = "BBT"
                        Case objCBTrx.EnumCashBankType.Transfer
                            strInitial = "STF"
                    End Select

                Case 4
                    strInitial = "PPL"

            End Select
        Else
            strInitial = "XXX"
        End If


        Select Case rblCashBankType.SelectedValue
            Case objCBTrx.EnumCashBankType.Payment
                strInOrOut = "O"
            Case objCBTrx.EnumCashBankType.Receipt
                strInOrOut = "I"
        End Select

        If Month(strDate) < strAccMonth And Year(strDate) <= strAccYear Then
            lblDate.Visible = True
            lblDate.Text = "Invalid transaction date."
            Exit Sub
        End If

        strAccYear = Year(strDate)
        strAccMonth = Month(strDate)
        strNewIDFormat = strInitial & "/" & strCompany & "/" & strLocation & "/" & IIf(Len(Trim(strAccMonth)) = 1, "0" & strAccMonth, strAccMonth) & "/" & strAccYear & "/"

        If lblPaymentID.Text = "" Then
            strTxParam = "|" & rblCashBankType.SelectedValue & "|" & ddlPayType.SelectedValue & _
                          "|" & strBank & "|" & IIf(Trim(txtChequeNo.Text) = "", "", Replace(txtChequeNo.Text, "'", "''")) & _
                          "|" & IIf(Trim(txtPaymentTo.Text) = "", "", Trim(txtPaymentTo.Text)) & "|" & strLocation & "|" & strAccMonth & _
                          "|" & strAccYear & "||" & strDate & "||" & strGiroDate & "|" & strUserId & "||" & strNewIDFormat & _
                          "|" & IIf(Trim(ddlSplBankAccNo.SelectedItem.Value) = "", "", Replace(ddlSplBankAccNo.SelectedItem.Value, "'", "''")) & "|" & strRemark & _
                          "|" & ddlBankTo.SelectedValue & "|" & txtSupCode.Text & "|" & objCBTrx.EnumTaxStatus.Unverified & _
                          "|" & strBankAccNo & "|" & IIf(chkChequeCash.Checked = True, "1", "2") & "|" & ddlChequeHandOver.SelectedItem.Value

            Try

                intErrNo = objCBTrx.mtdUpdCashBankDetail(strOpCdTxDet_ADD, _
                                                    strOpCdTxDet_UPD, _
                                                    strUserId, _
                                                    strTxParam, _
                                                    strDocPrefix, _
                                                    ErrorChk, _
                                                    TxResultID)
                lblPaymentID.Text = TxResultID
                LoadTxDetails()
                DisplayFromDB()

            Catch Exp As System.Exception
                If intErrNo > 0 Then
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADD_NEWCASHBANKDETAIL&errmesg=" & lblErrMessage.Text & "&redirect=CB/Trx/cb_trx_CashBankList.aspx")
                End If
            End Try
        End If

        'not in gsj
        'If ddlRefNo.SelectedItem.Value <> "" Then
        '    Dim strOpCd As String = "CB_CLSTRX_CASHBANK_LINE_ADD_FROM_TRANSFER"
        '    Dim strParamName As String = ""
        '    Dim strParamValue As String = ""

        '    strParamName = "STATUSINPUT|REFNO|CASHBANKID|NPWP|FFBSPL"

        '    strParamValue = Trim(lblStatusInput.Text) & _
        '                    "|" & Trim(ddlRefNo.SelectedItem.Value) & _
        '                    "|" & Trim(lblPaymentID.Text) & _
        '                    "|" & Trim(hidNPWPNo.Value) & _
        '                     "|" & Trim(hidFFBSpl.Value)

        '    Try
        '        intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd, _
        '                                                strParamName, _
        '                                                strParamValue)

        '    Catch Exp As System.Exception
        '        Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_CASHBANK_UPDATE_TRXID&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_CashBankDet.aspx")
        '    End Try
        'End If

        Initialize()
        BindGrid()
        BindRefNo("")
        onLoad_Button()

        blnShortCut.Text = False

        If lblTxLnID.Text <> "" Then
            AddDtlBtn.Visible = False
            SaveDtlBtn.Visible = True
        Else
            AddDtlBtn.Visible = True
            SaveDtlBtn.Visible = False
        End If
    End Sub

    Sub EditBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Update_Status(objCBTrx.EnumCashBankStatus.Active)
        LoadTxDetails()
        DisplayFromDB()
        onLoad_Button()

        txtDateCreated.Enabled = True
        btnDateCreated.Visible = True
        txtGiroDate.Enabled = True
        btnGiroDate.Visible = True
        ddlPayType.Enabled = True
        ddlBank.Enabled = True
        txtChequeNo.Enabled = True
        ddlBank.Enabled = True
        ddlBankTo.Enabled = True
        txtChequeNo.Enabled = True
        txtSplBankAccNo.Enabled = False
        txtRemark.Enabled = True
        'btnGet.Visible = True
        SaveBtn.Visible = True
        EditBtn.Visible = False
    End Sub

    Sub VerifiedBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim TxResultID As String
        Dim strAllowVehicle As String
        Dim ErrorChk As Integer = 0
        Dim StrTxParam As String
        Dim strDocPrefix As String
        Dim strGiroDate As String = Date_Validation(txtGiroDate.Text, False)
        Dim strNewIDFormat As String
        Dim strInOrOut As String
        Dim strRemark As String = Trim(Replace(txtRemark.Text, "'", "''"))
        Dim strInitial As String = ""
        Dim strParamName As String
        Dim strParamValue As String
        Dim strOpCd As String
        Dim intStatus As Integer
        Dim strDate As String = Date_Validation(txtDateCreated.Text, False)
        Dim indDate As String = ""
        Dim strBank As String
        Dim strBankAccCode As String
        Dim strBankAccNo As String
        Dim arrParam As Array

        If ddlBank.SelectedItem.Value = "" Then
            strBank = ""
            strBankAccCode = ""
            strBankAccNo = ""
            lblErrPayType.Text = "Please select Bank/Kas from"
            lblErrPayType.Visible = True
            Exit Sub
        Else
            arrParam = Split(Trim(ddlBank.SelectedItem.Value), "|")
            strBank = Trim(arrParam(0))
            strBankAccCode = Trim(arrParam(1))
            strBankAccNo = Trim(arrParam(2))

            If ddlPayType.SelectedItem.Value = 0 Or ddlPayType.SelectedItem.Value = 3 Then
                If strBankAccNo = "" Then
                    lblErrPayType.Text = "This payment type must use Bank account"
                    lblErrPayType.Visible = True
                    Exit Sub
                End If
            Else
                If strBankAccNo <> "" Then
                    lblErrPayType.Text = "This payment type cannot use Bank account"
                    lblErrPayType.Visible = True
                    Exit Sub
                End If
                Select Case strBank
                    Case "KKL", "KKK"
                        If ddlPayType.SelectedItem.Value = 4 Then
                            lblErrPayType.Text = "This payment type cannot use Kas account"
                            lblErrPayType.Visible = True
                            Exit Sub
                        End If
                        If Left(lblPaymentID.Text, 3) <> "XXX" Then
                            If Left(lblPaymentID.Text, 2) <> "KK" Then
                                lblErrPayType.Text = "<br>ID transaction already set to Kas Besar."
                                lblErrPayType.Visible = True
                                Exit Sub
                            End If
                        End If

                    Case "KBR"
                        If ddlPayType.SelectedItem.Value = 4 Then
                            lblErrPayType.Text = "This payment type cannot use Kas account"
                            lblErrPayType.Visible = True
                            Exit Sub
                        End If
                        If Left(lblPaymentID.Text, 3) <> "XXX" Then
                            If Left(lblPaymentID.Text, 2) <> "KR" Then
                                lblErrPayType.Text = "<br>ID transaction already set to Kas Kecil."
                                lblErrPayType.Visible = True
                                Exit Sub
                            End If
                        End If

                    Case Else
                        If ddlPayType.SelectedItem.Value = 1 Then
                            lblErrPayType.Text = "This payment type cannot use RK account"
                            lblErrPayType.Visible = True
                            Exit Sub
                        End If
                End Select
            End If
        End If

        If CheckDate(txtDateCreated.Text.Trim(), indDate) = False Then
            lblDate.Visible = True
            lblFmt.Visible = True
            lblDate.Text = "<br>Date Entered should be in the format"
            Exit Sub
        End If

        Dim intInputPeriod As Integer = Year(strDate) * 100 + Month(strDate)
        Dim intCurPeriod As Integer = (CInt(strAccYear) * 100) + CInt(strAccMonth)
        Dim intSelPeriod As Integer = (CInt(strSelAccYear) * 100) + CInt(strSelAccMonth)

        If Session("SS_FILTERPERIOD") = "0" Then
            If intCurPeriod < intInputPeriod Then
                lblDate.Visible = True
                lblDate.Text = "Invalid transaction date."
                Exit Sub
            End If
        Else
            If intSelPeriod <> intInputPeriod Then
                lblDate.Visible = True
                lblDate.Text = "Invalid transaction date."
                Exit Sub
            End If
            If intSelPeriod < intCurPeriod Then
                lblDate.Visible = True
                lblDate.Text = "This period already locked."
                Exit Sub
            End If
        End If

        If lblPaymentID.Text = "" Then
            Exit Sub
        End If

        If txtPaymentTo.Text = "" Then
            lblConfirmErr.Visible = True
            lblConfirmErr.Text = "Payment To/Receive From cannot be empty."
            Exit Sub
        End If

        If ddlPayType.SelectedItem.Value = "0" Or ddlPayType.SelectedItem.Value = "3" Then
            If strBank = "" Then
                lblErrBank.Visible = True
                Exit Sub
            End If
            If txtGiroDate.Text = "" Then
                lblDateGiro.Visible = True
                lblDateGiro.Text = "Please enter Giro Date"
                Exit Sub
            End If
            If CheckDate(txtGiroDate.Text.Trim(), indDate) = False Then
                lblDateGiro.Visible = True
                lblFmtGiro.Visible = True
                lblDateGiro.Text = "<br>Date Entered should be in the format"
                Exit Sub
            End If
            'If txtChequeNo.Text = "" Then
            '    lblErrCheque.Visible = True
            '    Exit Sub
            'End If
            If lblCurrentPeriod.Text <> "" Then
                If Month(strGiroDate) >= Mid(lblCurrentPeriod.Text, 5) And Year(strGiroDate) >= Mid(lblCurrentPeriod.Text, 1, 4) Then
                Else
                    If rblCashBankType.SelectedValue = objCBTrx.EnumCashBankType.Payment Then
                        lblDateGiro.Visible = True
                        lblDateGiro.Text = "Invalid transaction date."
                        Exit Sub
                    End If
                End If
                If Year(strGiroDate) & Format(Month(strGiroDate), "00") & Format(Day(strGiroDate), "00") < Year(strDate) & Format(Month(strDate), "00") & Format(Day(strDate), "00") Then
                    If rblCashBankType.SelectedValue = objCBTrx.EnumCashBankType.Payment Then
                        lblDateGiro.Visible = True
                        lblDateGiro.Text = "Invalid transaction date."
                        Exit Sub
                    End If
                End If
            Else
                If Year(strGiroDate) & Format(Month(strGiroDate), "00") & Format(Day(strGiroDate), "00") < Year(strDate) & Format(Month(strDate), "00") & Format(Day(strDate), "00") Then
                    If rblCashBankType.SelectedValue = objCBTrx.EnumCashBankType.Payment Then
                        lblDateGiro.Visible = True
                        lblDateGiro.Text = "Invalid transaction date."
                        Exit Sub
                    End If
                End If
                If Month(strGiroDate) < strAccMonth And Year(strGiroDate) <= strAccYear Then
                    If rblCashBankType.SelectedValue = objCBTrx.EnumCashBankType.Payment Then
                        lblDateGiro.Visible = True
                        lblDateGiro.Text = "Invalid transaction date."
                        Exit Sub
                    End If
                End If
            End If
        End If

        If rblCashBankType.SelectedValue = objCBTrx.EnumCashBankType.Payment Then
            strDocPrefix = objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CBPaymentOther)
        Else
            strDocPrefix = objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CBReceiptOther)
        End If

        If lblPaymentID.Text <> "" Then
            Select Case ddlPayType.SelectedItem.Value
                Case 0
                    Select Case rblCashBankType.SelectedValue
                        Case objCBTrx.EnumCashBankType.Payment
                            strInitial = "BBK"
                        Case objCBTrx.EnumCashBankType.Receipt
                            strInitial = "BBT"
                        Case objCBTrx.EnumCashBankType.Transfer
                            strInitial = "STF"
                    End Select

                Case 1
                    Select Case rblCashBankType.SelectedValue
                        Case objCBTrx.EnumCashBankType.Payment
                            Select Case Trim(strBank)
                                Case "KKL", "KKK"
                                    strInitial = "KKK"
                                Case Else
                                    strInitial = "KRK"
                            End Select

                        Case objCBTrx.EnumCashBankType.Receipt
                            Select Case Trim(strBank)
                                Case "KKL", "KKK"
                                    strInitial = "KKT"
                                Case Else
                                    strInitial = "KRT"
                            End Select

                        Case objCBTrx.EnumCashBankType.Transfer
                            strInitial = "STF"
                    End Select

                Case 2
                    strInitial = "XXX"

                Case 3
                    Select Case rblCashBankType.SelectedValue
                        Case objCBTrx.EnumCashBankType.Payment
                            strInitial = "BBK"
                        Case objCBTrx.EnumCashBankType.Receipt
                            strInitial = "BBT"
                        Case objCBTrx.EnumCashBankType.Transfer
                            strInitial = "STF"
                    End Select

                Case 4
                    strInitial = "PPL"

            End Select
        Else
            strInitial = "XXX"
        End If

        strAccYear = Year(strDate)
        strAccMonth = Month(strDate)

        strNewIDFormat = strInitial & "/" & strCompany & "/" & strLocation & "/" & IIf(Len(Trim(strAccMonth)) = 1, "0" & strAccMonth, strAccMonth) & "/" & Trim(strAccYear) & "/"

        StrTxParam = lblPaymentID.Text & "|" & rblCashBankType.SelectedValue & "|" & ddlPayType.SelectedValue & _
                          "|" & strBank & "|" & IIf(Trim(txtChequeNo.Text) = "", "", Trim(txtChequeNo.Text)) & _
                          "|" & IIf(Trim(txtPaymentTo.Text) = "", "", Trim(txtPaymentTo.Text)) & "|" & strLocation & "|" & strAccMonth & _
                          "|" & strAccYear & "||" & strDate & "||" & strGiroDate & "|" & strUserId & "||" & strNewIDFormat & _
                          "|" & IIf(Trim(ddlSplBankAccNo.SelectedItem.Value) = "", "", Trim(ddlSplBankAccNo.SelectedItem.Value)) & "|" & strRemark & _
                          "|" & ddlBankTo.SelectedValue & "|" & txtSupCode.Text & "|" & CInt(hidTaxStatus.Value) & _
                          "|" & strBankAccNo & "|" & IIf(chkChequeCash.Checked = True, "1", "2") & "|" & ddlChequeHandOver.SelectedItem.Value

        Try

            intErrNo = objCBTrx.mtdUpdCashBankDetail(strOpCdTxDet_ADD, _
                                                        strOpCdTxDet_UPD, _
                                                        strUserId, _
                                                        StrTxParam, _
                                                        strDocPrefix, _
                                                        ErrorChk, _
                                                        TxResultID)

            lblPaymentID.Text = TxResultID
            If ErrorChk = 1 Then
                lblerror.Visible = True
            End If

        Catch Exp As System.Exception
            If intErrNo <> -5 Then
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADD_ADDNEWCASHBANK&errmesg=" & lblErrMessage.Text & "&redirect=/CB/trx/CB_trx_CashBankDet.aspx")
            End If
        End Try

        intStatus = Convert.ToInt16(lblStsHid.Text)
        If intStatus = objCBTrx.EnumCashBankStatus.Active Or intStatus = objCBTrx.EnumCashBankStatus.Verified Or intStatus = objCBTrx.EnumCashBankStatus.Confirmed Then
            If strBank = "" And Left(Trim(lblPaymentID.Text), 3) <> "XXX" Then
                lblErrBank.Visible = True
                Exit Sub
            End If

            If strBank <> "" Then
                strOpCd = "CB_CLSTRX_CASHBANK_DETAIL_UPD_ACCNO"
                strParamName = "BANKCODE|CASHBANKID|SOURCETYPE|BANKACCNO|LOCCODE"
                strParamValue = strBank & "|" & _
                                lblPaymentID.Text & "|" & _
                                ddlPayType.SelectedItem.Value & "|" & _
                                strBankAccNo & "|" & _
                                strLocation

                Try
                    intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd, _
                                                            strParamName, _
                                                            strParamValue)

                Catch Exp As System.Exception
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PAYMENT_UPD&errmesg=" & Exp.ToString() & "&redirect=cb/trx/cb_trx_cashbanklist")
                End Try
            End If
        End If

        If (Left(Trim(lblPaymentID.Text), 3) <> "TBS" Or Left(Trim(lblPaymentID.Text), 3) <> "CPO" Or Left(Trim(lblPaymentID.Text), 3) <> "KER" Or Left(Trim(lblPaymentID.Text), 3) <> "CKG" Or Left(Trim(lblPaymentID.Text), 3) <> "OTH") Then
            If ddlPayType.SelectedItem.Value <> 2 And Left(Trim(lblPaymentID.Text), 3) = "XXX" Then
                Dim strOpCd_FindLastID As String = "ADMIN_CLSTRX_SEARCH_LASTID_CB"
                Dim strLastIDSearch1 As String
                Dim strLastIDSearch2 As String
                Dim strLastIDSearch3 As String
                Dim strNewID As String
                Dim objLastIDDs As New Object
                strOpCd = "CB_CLSTRX_CASHBANK_UPDATE_TRXID"

                strLastIDSearch1 = " LocCode = '" & strLocation & "' And AccMonth = '" & Trim(strAccMonth) & "' And AccYear = '" & Trim(strAccYear) & "' And Left(LTrim(CashBankID),3) = '" & Trim(strInitial) & "' "
                strLastIDSearch2 = " LocCode = '" & strLocation & "' And AccMonth = '" & Trim(strAccMonth) & "' And AccYear = '" & Trim(strAccYear) & "' And Left(LTrim(PaymentID),3) = '" & Trim(strInitial) & "' "
                strLastIDSearch3 = " LocCode = '" & strLocation & "' And AccMonth = '" & Trim(strAccMonth) & "' And AccYear = '" & Trim(strAccYear) & "' And Left(LTrim(ReceiptID),3) = '" & Trim(strInitial) & "' "
                strParamName = "STRSEARCH1|STRSEARCH2|STRSEARCH3"
                strParamValue = strLastIDSearch1 & "|" & strLastIDSearch2 & "|" & strLastIDSearch3

                Try
                    intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_FindLastID, _
                                                        strParamName, _
                                                        strParamValue, _
                                                        objLastIDDs)
                Catch Exp As System.Exception
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ID_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
                End Try

                If objLastIDDs.Tables(0).Rows.Count > 0 Then
                    strNewID = Trim(objLastIDDs.Tables(0).Rows(0).Item("NewTrxID"))
                Else
                    strNewID = "1"
                End If

                strNewIDFormat = strInitial & "/" & strCompany & "/" & strLocation & "/" & IIf(Len(Trim(strAccMonth)) = 1, "0" & strAccMonth, strAccMonth) & "/" & Trim(strAccYear) & "/" & Format(Val(strNewID), "0000")

                strParamName = "SOURCETYPE|CHEQUENO|BANKCODE|BANKTO|SPLBANKACCNO|REMARK|CHEQUEPRINTDATE|PRINTDATE|LOCCODE|CASHBANKID|CASHBANKNEWID|UPDATEID"

                strParamValue = Trim(ddlPayType.SelectedItem.Value) & _
                                "|" & Trim(txtChequeNo.Text) & _
                                "|" & Trim(strBank) & _
                                "|" & Trim(ddlBankTo.SelectedItem.Value) & _
                                "|" & Trim(ddlSplBankAccNo.SelectedItem.Value) & _
                                "|" & Trim(Replace(txtRemark.Text, "'", "''")) & _
                                "|" & "" & _
                                "|" & strGiroDate & _
                                "|" & Trim(strLocation) & _
                                "|" & Trim(lblPaymentID.Text) & _
                                "|" & Trim(strNewIDFormat) & _
                                "|" & Trim(strUserId)

                Try
                    intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd, _
                                                            strParamName, _
                                                            strParamValue)

                Catch Exp As System.Exception
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_CASHBANK_UPDATE_TRXID&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_CashBankDet.aspx")
                End Try

                lblPaymentID.Text = Trim(strNewIDFormat) 'Trim(strBank) + Trim(Mid(lblPaymentID.Text, 4))
            End If
        End If

        Dim strOpCdUpd As String = "CB_CLSTRX_CASHBANK_UPDATE"

        strParamName = "CHEQUENO|BANKCODE|REMARK|CHEQUEPRINTDATE|PRINTDATE|LOCCODE|CASHBANKID"

        strParamValue = Trim(txtChequeNo.Text) & _
                        "|" & Trim(strBank) & _
                        "|" & Trim(Replace(txtRemark.Text, "'", "''")) & _
                        "|" & IIf(Left(Trim(lblPaymentID.Text), 3) = "XXX", "", "") & _
                        "|" & strGiroDate & _
                        "|" & Trim(strLocation) & _
                        "|" & Trim(lblPaymentID.Text.Trim)

        Try
            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCdUpd, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_CASHBANK_PREVIEWCHEQUE_UPD&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_CashBankDet.aspx")
        End Try

        Update_Status(objCBTrx.EnumCashBankStatus.Verified)
        LoadTxDetails()
        DisplayFromDB()
        onLoad_Button()
    End Sub

    Private Sub UpdateCBLine()
        Dim strOpCdGetUpd As String
        Dim objFinds As New Object
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim ErrorChk As Integer = 0
        Dim strDocPrefixLn As String
        Dim dbBalance As Double
        Dim strDate As String = Date_Validation(txtDateCreated.Text, False)
        Dim dsResult As New Object
        Dim strUpdateDate As Date
        Dim strPrintDate As Date

        strParam = Trim(lblPaymentID.Text)
        strParamName = "CASHBANKID"
        strParamValue = Trim(lblPaymentID.Text)

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCdTxLine_GET, _
                                                    strParamName, _
                                                    strParamValue, _
                                                    dsResult)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GL_DAYEND_PROCESS&errmesg=" & Exp.Message.ToString & "&redirect=")
        End Try


        If dsResult.Tables(0).Rows.Count > 0 Then
            If Left(Trim(lblPaymentID.Text), 3) <> "XXX" And dsResult.Tables(0).Rows(0).Item("ChequePrintDate") > dsResult.Tables(0).Rows(0).Item("CreateDate") Then
            Else
                strAccYear = Year(strDate)
                strAccMonth = Month(strDate)
                strOpCdGetUpd = "CB_CLSTRX_CASHBANK_LINE_UPD_COABANK"
                strParamName = "LOCCODE|ACCMONTH|ACCYEAR|CASHBANKID|SOURCETYPE"
                strParamValue = Trim(strLocation) & "|" & strAccMonth & "|" & strAccYear & "|" & Trim(lblPaymentID.Text) & "|" & ddlPayType.SelectedItem.Value

                Try
                    intErrNo = objGLtrx.mtdInsertDataCommon(strOpCdGetUpd, _
                                                        strParamName, _
                                                        strParamValue)
                Catch Exp As System.Exception
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ID_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
                End Try

                '=========untuk update ID jk penomeran ikut kode bank
                'Dim strOpCd_UpdID As String = "CB_CLSTRX_CASHBANK_UPD_TRXID"
                'Dim objID As New Object
                'strParamName = "CASHBANKID|LOCCODE|USERID|CBTYPE|SOURCETYPE"
                'strParamValue = Trim(lblPaymentID.Text) & "|" & strLocation & "|" & strUserId & "|" & IIf(rblCashBankType.SelectedValue = objCBTrx.EnumCashBankType.Payment, "1", "2") & _
                '                "|" & ddlPayType.SelectedItem.Value

                'Try
                '    intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_UpdID, _
                '                                            strParamName, _
                '                                            strParamValue, _
                '                                            objID)

                'Catch Exp As System.Exception
                '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PAYMENT_UPD&errmesg=" & Exp.ToString() & "&redirect=cb/trx/cb_trx_cashbanklist")
                'End Try

                'If objID.Tables(0).Rows.Count > 0 Then
                '    lblPaymentID.Text = Trim(objID.Tables(0).Rows(0).Item("NewTrxID"))
                '    rblCashBankType.SelectedIndex = CInt(objID.Tables(0).Rows(0).Item("CashBankType")) - 1

                '    If rblCashBankType.SelectedIndex = 0 Then
                '        lblPaymentIDTag.Text = "Payment ID :"
                '        lblPayTypeTag.Text = "Payment Type :*"
                '        lblPayToTag.Text = "Payment To :*"
                '    Else
                '        lblPaymentIDTag.Text = "Receipt ID :"
                '        lblPayTypeTag.Text = "Receipt Type :*"
                '        lblPayToTag.Text = "Receipt From :*"
                '    End If
                'End If
            End If
        End If

        strOpCdGetUpd = "CB_CLSTRX_CASHBANK_DETAIL_UPD"
        strParamName = "UPDATESTR"
        strParamValue = "SET UPDATEID = '" & Trim(strUserId) & "', UPDATEDATE = GETDATE() WHERE CASHBANKID = '" & Trim(lblPaymentID.Text) & "' AND LOCCODE = '" & Trim(strLocation) & "'"

        Try
            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCdGetUpd, _
                                                strParamName, _
                                                strParamValue)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ID_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try
    End Sub

    Private Sub txtPaymentTo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPaymentTo.TextChanged
        ''txtSplBankAccNo.Text = ""
        ''ddlSupplier.SelectedValue = ""
        ''ddlBankTo.SelectedValue = ""
        'GetSupplier(txtPaymentTo.Text.Trim)
    End Sub

    Private Sub lbViewJournal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbViewJournal.Click
        Dim intErrNo As Integer
        Dim dsResult As New Object
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim strOpCode As String = "GL_JOURNAL_PREDICTION"
        Dim arrPeriod As Array

        arrPeriod = Split(lblAccPeriod.Text, "/")

        strParamName = "LOCCODE|ACCMONTH|ACCYEAR|USERID|TRXID"
        strParamValue = strLocation & "|" & arrPeriod(0) & _
                        "|" & arrPeriod(1) & "|" & _
                        Session("SS_USERID") & "|" & Trim(lblPaymentID.Text)

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
                                                    strParamName, _
                                                    strParamValue, _
                                                    dsResult)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GL_DAYEND_PROCESS&errmesg=" & Exp.Message.ToString & "&redirect=")
        End Try

        If dsResult.Tables(0).Rows.Count > 0 Then

            Dim TotalDB As Double
            Dim TotalCR As Double
            Dim intCnt As Integer
            For intCnt = 0 To dsResult.Tables(0).Rows.Count - 1
                TotalDB += dsResult.Tables(0).Rows(intCnt).Item("AmountDB")
                TotalCR += dsResult.Tables(0).Rows(intCnt).Item("AmountCR")
            Next
            lblTotalDB.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(TotalDB, CInt(Session("SS_ROUNDNO"))), CInt(Session("SS_ROUNDNO")))
            lblTotalCR.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(TotalCR, CInt(Session("SS_ROUNDNO"))), CInt(Session("SS_ROUNDNO")))

            dgViewJournal.DataSource = Nothing
            dgViewJournal.DataSource = dsResult.Tables(0)
            dgViewJournal.DataBind()

            lblTotalDB.Visible = True
            lblTotalCR.Visible = True
            lblTotalViewJournal.Visible = True
            lblTotalViewJournal.Text = "Total Amount : "
        End If

        LoadTxDetails()
        DisplayFromDB()
        onLoad_Button()
    End Sub

    Sub BindTaxObjectList(Optional ByVal pv_strAccCode As String = "", Optional ByVal pv_strTaxLnID As String = "")
        Dim strOpCd As String = "TX_CLSSETUP_TAXOBJECTRATELN_LIST_GET"
        Dim dr As DataRow
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer = 0
        Dim dsForDropDown As DataSet
        Dim intSPL As Char

        If hidNPWPNo.Value = "" Then
            intSPL = "2"
        Else
            intSPL = "1"
        End If
        strParamName = "STRSEARCH|INTSPL"
        strParamValue = "AccCode = '" & Trim(pv_strAccCode) & "' ORDER By TaxLnID ASC" & "|" & Trim(intSPL)

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCd, _
                                                strParamName, _
                                                strParamValue, _
                                                dsForDropDown)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=TX_Setup_TaxObjectDet&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To dsForDropDown.Tables(0).Rows.Count - 1
            If Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("TaxLnID")) = Trim(pv_strTaxLnID) Then
                intSelectedIndex = intCnt + 1
            End If
        Next intCnt

        dr = dsForDropDown.Tables(0).NewRow()
        dr("TaxLnID") = ""
        dr("Descr") = lblSelect.Text & "Tax Object"
        dsForDropDown.Tables(0).Rows.InsertAt(dr, 0)

        lstTaxObject.DataSource = dsForDropDown.Tables(0)
        lstTaxObject.DataValueField = "TaxLnID"
        lstTaxObject.DataTextField = "Descr"
        lstTaxObject.DataBind()
        lstTaxObject.SelectedIndex = intSelectedIndex

        If Not dsForDropDown Is Nothing Then
            dsForDropDown = Nothing
        End If
    End Sub

    Sub lstTaxObject_OnSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim arrParam As Array
        Dim CRAmt As Double
        Dim DRAmt As Double
        arrParam = Split(lstTaxObject.SelectedItem.Text, ";")

        If lstTaxObject.SelectedItem.Value = "" Then
            txtCRTotalAmount.ReadOnly = False
            txtDRTotalAmount.ReadOnly = False
        Else
            hidTaxObjectRate.Value = CDbl(Replace(arrParam(1), "%", ""))
            If txtDPPAmountDR.Text <> "" Then
                DRAmt = CDbl(IIf(txtDPPAmountDR.Text = "", 0, txtDPPAmountDR.Text)) * (hidTaxObjectRate.Value / 100)
                DRAmt = Math.Floor(DRAmt + 0.5)
                txtDRTotalAmount.Text = DRAmt
            Else
                CRAmt = CDbl(IIf(txtDPPAmountCR.Text = "", 0, txtDPPAmountCR.Text)) * (hidTaxObjectRate.Value / 100)
                CRAmt = Math.Floor(CRAmt + 0.5)
                txtCRTotalAmount.Text = CRAmt
            End If

            txtCRTotalAmount.ReadOnly = True
            txtDRTotalAmount.ReadOnly = True
        End If
    End Sub

    Sub GetSaldoBankBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim strOpCode As String = "HR_CLSSETUP_BANK_LIST_GET"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim objBankDs As New DataSet()
        Dim intSelectedBankIndex As Integer = 0
        Dim strDate As String = Date_Validation(txtDateCreated.Text, False)
        Dim strOpCodeBank As String

        strAccYear = Year(strDate)
        strAccMonth = Month(strDate)
        strOpCodeBank = "HR_CLSSETUP_BANK_LOCATION_GET"

        strParamName = "LOCCODE|ACCYEAR|ACCMONTH|TRXDATE|ACCCODE"
        strParamValue = strLocation & "|" & strAccYear & "|" & strAccMonth & "|" & strDate & "|"

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCodeBank, _
                                                strParamName, _
                                                strParamValue, _
                                                objBankDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ID_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try


        Dim dr As DataRow
        dr = objBankDs.Tables(0).NewRow()
        dr("BankCode") = ""
        dr("_Description") = lblPleaseSelect.Text & " Bank" & lblCode.Text
        objBankDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlBank.DataSource = objBankDs.Tables(0)
        ddlBank.DataValueField = "BankCode"
        ddlBank.DataTextField = "_Description"
        ddlBank.DataBind()
        ddlBank.SelectedIndex = intSelectedBankIndex

    End Sub

    Sub BindGiroNo(ByVal pv_strBankCode As String, ByVal pv_strGiroNo As String)
        Dim strOpCode As String = "HR_CLSSETUP_BANK_BILYETGIRO_GET_TRX"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedBankIndex As Integer = 0
        Dim strDate As String = Date_Validation(txtDateCreated.Text, False)
        Dim strDocType As String

        Dim strBank As String
        Dim strBankAccCode As String
        Dim strBankAccNo As String
        Dim arrParam As Array

        If pv_strBankCode = "" Then
            strBank = ""
            strBankAccCode = ""
            strBankAccNo = ""
            'Exit Sub
        Else
            arrParam = Split(Trim(pv_strBankCode), "|")
            strBank = Trim(arrParam(0))
            strBankAccCode = Trim(arrParam(1))
            strBankAccNo = Trim(arrParam(2))
        End If

        strDocType = ddlPayType.SelectedItem.Value

        strParamName = "LOCCODE|STRSEARCH|TRXID"
        strParamValue = strLocation & "|" & _
                        " AND BankCode = '" & strBank & "' AND BankAccNo = '" & strBankAccNo & "' AND DocType IN ('" & strDocType & "') " & "|" & _
                        Trim(lblPaymentID.Text)

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
                                                strParamName, _
                                                strParamValue, _
                                                objBankDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ID_GET&errmesg=" & Exp.ToString() & "&redirect=")
        End Try

        For intCnt = 0 To objBankDs.Tables(0).Rows.Count - 1
            If Trim(pv_strGiroNo) = Trim(objBankDs.Tables(0).Rows(intCnt).Item("DocNo")) Then
                intSelectedBankIndex = intCnt + 1
            End If
        Next intCnt

        Dim dr As DataRow
        dr = objBankDs.Tables(0).NewRow()
        dr("DocNo") = ""
        dr("BankDescr") = lblPleaseSelect.Text & " Giro/Cheque no."
        objBankDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlGiroNo.DataSource = objBankDs.Tables(0)
        ddlGiroNo.DataValueField = "DocNo"
        ddlGiroNo.DataTextField = "BankDescr"
        ddlGiroNo.DataBind()
        ddlGiroNo.SelectedIndex = intSelectedBankIndex
    End Sub

    Private Function lCDbl(ByVal pcVal As String) As Double
        If IsNumeric(pcVal) Then lCDbl = CDbl(pcVal) Else lCDbl = 0
    End Function


    'Private Sub txtDPPAmountDR_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDPPAmountDR.TextChanged
    '    txtDPPAmountDR.Text = Format(Val(Replace(txtDPPAmountDR.Text, ",", "")), "#,###,###")
    'End Sub

    'Private Sub txtDRTotalAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDRTotalAmount.TextChanged
    '    txtDRTotalAmount.Text = Format(Val(Replace(txtDRTotalAmount.Text, ",", "")), "#,###,###")
    'End Sub

End Class
