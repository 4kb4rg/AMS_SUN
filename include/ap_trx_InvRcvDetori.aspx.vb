
Imports System
Imports System.Data
Imports System.Web.UI
Imports System.Math
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic


Public Class ap_trx_InvRcvDet : Inherits Page

    Protected WithEvents lblErrMessage As Label    
    Protected WithEvents lblCode As Label
    Protected WithEvents lblInvoiceRcvID As Label
    Protected WithEvents lblAccPeriod As Label
    Protected WithEvents lblStatus As Label
    Protected WithEvents txtInvoiceRcvRefNo As TextBox
    Protected WithEvents lblDateCreated As Label
    Protected WithEvents txtInvoiceRcvRefDate As TextBox
    Protected WithEvents lblLastUpdate As Label
    Protected WithEvents lblPrintDate As Label
    Protected WithEvents ddlPO As DropDownList
    Protected WithEvents lblUpdatedBy As Label
    Protected WithEvents rbSPO As RadioButton
    Protected WithEvents rbCWO As RadioButton
    Protected WithEvents rbOTE As RadioButton
    Protected WithEvents rbFFB As RadioButton
    Protected WithEvents ddlSuppCode As DropDownList
    Protected WithEvents lblErrInvoiceRcvRefNo As Label
    Protected WithEvents lblErrSuppCode As Label
    Protected WithEvents txtCreditTerm As TextBox
    Protected WithEvents ddlTermType As DropDownList
    Protected WithEvents dgLineDet As DataGrid
    Protected WithEvents lblInvoiceAmount As Label
    Protected WithEvents lblOutPayAmount As Label
    Protected WithEvents txtRemark As TextBox
    Protected WithEvents txtQty As TextBox
    Protected WithEvents txtCost As TextBox
    Protected WithEvents txtDONo As TextBox
    Protected WithEvents txtOtherInvNo As TextBox
    Protected WithEvents cbPPN As CheckBox
    Protected WithEvents txtPPHRate As TextBox
    Protected WithEvents tblSelection As HtmlTable
    Protected WithEvents validateItem As RequiredFieldValidator
    Protected WithEvents btnSelDate As Image
    Protected WithEvents Addbtn As ImageButton
    Protected WithEvents SaveBtn As ImageButton
    Protected WithEvents RefreshBtn As ImageButton
    Protected WithEvents ConfirmBtn As ImageButton
    Protected WithEvents PrintBtn As ImageButton
    Protected WithEvents CancelBtn As ImageButton
    Protected WithEvents DeleteBtn As ImageButton
    Protected WithEvents UnDeleteBtn As ImageButton
    Protected WithEvents BackBtn As ImageButton
    Protected WithEvents lblPleaseSelect As Label
    Protected WithEvents inrid As HtmlInputHidden
    Protected WithEvents idSuppCode As HtmlInputHidden
    Protected WithEvents lblErrInvRcvRefDate As Label
    Protected WithEvents lblErrUnDel As Label
    Protected WithEvents lblErrQty As Label
    Protected WithEvents lblStatusHidden As Label
    Protected WithEvents lblAccount As Label
    Protected WithEvents lblBlock As Label
    Protected WithEvents lblVehicle As Label
    Protected WithEvents lblVehExpense As Label
    Protected WithEvents lblErrBlock As Label
    Protected WithEvents lblErrVehicle As Label
    Protected WithEvents lblErrVehicleExp As Label
    Protected WithEvents lblVehicleOption As Label
    Protected WithEvents lblErrAccCode As Label
    Protected WithEvents lblErrPO As Label
    Protected WithEvents lblPPN As Label
    Protected WithEvents lblPPH As Label
    Protected WithEvents lblPercen As Label
    Protected WithEvents errOverQty As Label
    Protected WithEvents lstItem As DropDownList
    Protected WithEvents ddlAccCode As DropDownList
    Protected WithEvents ddlBlock As DropDownList
    Protected WithEvents ddlVehCode As DropDownList
    Protected WithEvents ddlVehExpCode As DropDownList
    Protected WithEvents RowChargeLevel As HtmlTableRow
    Protected WithEvents RowPreBlk As HtmlTableRow
    Protected WithEvents RowBlk As HtmlTableRow
    Protected WithEvents lblPreBlockErr As Label
    Protected WithEvents lblPreBlkTag As Label
    Protected WithEvents ddlPreBlock As DropDownList
    Protected WithEvents ddlChargeLevel As DropDownList
    Protected WithEvents hidBlockCharge As HtmlInputHidden
    Protected WithEvents hidChargeLocCode As HtmlInputHidden
    Protected WithEvents errUnmatchCost As Label
    Protected WithEvents lblTitle As Label
    Protected WithEvents lblInvoiceRcvIDTag As Label
    Protected WithEvents lblInvoiceRcvRefNoTag As Label
    Protected WithEvents lblInvoiceRcvRefDateTag As Label
    Protected WithEvents lblID As Label
    Protected WithEvents lblRefNo As Label
    Protected WithEvents lblRefDate As Label
    Protected WithEvents lblUnqErrInvRcvRefNo As Label
    Protected WithEvents lblIDInvoiceAmount As Label
    Protected WithEvents lblIDOutPayAmount As Label
    Protected WithEvents lblCurrency1 As Label
    Protected WithEvents lblCurrency2 As Label
    Protected WithEvents hidCurrencyCode As HtmlInputHidden
    Protected WithEvents hidExchangeRate As HtmlInputHidden
    Protected WithEvents rbTransportFee As RadioButton
    Protected WithEvents txtPBBKB As TextBox
    Protected WithEvents btnAddAllItem As ImageButton
    Protected WithEvents txtInvDueDate As TextBox
    Protected WithEvents lblerrInvDueDate As Label
    Protected WithEvents hidTermType As HtmlInputHidden
    Protected WithEvents PrintDocBtn As ImageButton
    Protected WithEvents txtSplInvAmt As TextBox
    Protected WithEvents dgAPNote As DataGrid
    Protected WithEvents NewBtn As ImageButton
    Protected WithEvents ddlTTRefNo As DropDownList
    Protected WithEvents txtTransDate As TextBox
    Protected WithEvents lblErrTransDate As Label
    Protected WithEvents EditBtn As ImageButton

    Protected WithEvents TrLink As HtmlTableRow
    Protected WithEvents lbViewJournal As LinkButton
    Protected WithEvents dgViewJournal As DataGrid
    Protected WithEvents lblTotalDB As Label
    Protected WithEvents lblTotalCR As Label
    Protected WithEvents lblTotalViewJournal As Label
    Protected WithEvents lblUsedAdvPayAmount As Label
    Protected WithEvents lblIDUsedAdvPayAmount As Label
    Protected WithEvents lblCurrency3 As Label
    Protected WithEvents hidPaymentID As HtmlInputHidden
    Protected WithEvents hidAdvAmount As HtmlInputHidden
    Protected WithEvents hidUsedAdvAmount As HtmlInputHidden
    Protected WithEvents hidInvAmount As HtmlInputHidden
    Protected WithEvents hidAdvExchangeRate As HtmlInputHidden
    Protected WithEvents hidAdvPayment As HtmlInputHidden
    Protected WithEvents lblAdvPaymentAmount As Label
    Protected WithEvents lblCurrency4 As Label
    Protected WithEvents hidPOCurrencyCode As HtmlInputHidden
    Protected WithEvents hidAdvCurrencyCode As HtmlInputHidden
    Protected WithEvents lblFmtTransDate As Label
    Protected WithEvents lblFmtInvRcvRefDate As Label
    Protected WithEvents lblFmtInvDueDate As Label
    Protected WithEvents lblAmtTransportFee As Label
    Protected WithEvents txtAmtTransportFee As TextBox

    Protected WithEvents errReqCost As Label
    Protected WithEvents errReqQty As Label
    Protected WithEvents hidUnitCost As HtmlInputHidden

    Protected WithEvents lblCurrency5 As Label
    Protected WithEvents lblCurrency6 As Label
    Protected WithEvents lblDNCNAmount As Label
    Protected WithEvents lblUsedAdjAmount As Label
    Protected WithEvents txtUsedDNCNAmount As TextBox
    Protected WithEvents txtOutPayAmount As TextBox
    Protected WithEvents hidOutPayAmount As HtmlInputHidden
    Protected WithEvents hidDNCNID As HtmlInputHidden
    Protected WithEvents TrAdj As HtmlTableRow
    Protected WithEvents dgAPCJ As DataGrid
    Protected WithEvents hidAPCJID As HtmlInputHidden
    Protected WithEvents hidDNCNAmount As HtmlInputHidden
    Protected WithEvents hidHasPaymentID As HtmlInputHidden

    Dim PreBlockTag As String
    Dim BlockTag As String

    Dim objINSetup As New agri.IN.clsSetup()
    Dim objPUSetup As New agri.PU.clsSetup()
    Dim objPUTrx As New agri.PU.clsTrx()
    Protected objGLSetup As New agri.GL.clsSetup()
    Protected objGL As New agri.GL.clsSetup()
    Dim objAPTrx As New agri.AP.clsTrx()
    Dim objAdminShare As New agri.Admin.clsShare()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim objGLtrx As New agri.GL.ClsTrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()

    Dim objPODs As New Object()
    Dim objAccDs As New Object()
    Dim objPOLnDs As New Object()
    Dim objInvRcvLnDs As New Object()
    Dim objTermTypeDs As New Object()
    Dim objCreditTermDs As New Object()
    Dim objLangCapDs As DataSet

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intAPAR As Integer
    Dim intPPN As Decimal
    Dim intCost As Decimal
    Dim intPPNAmount As Decimal
    Dim intPPHAmount As Decimal
    Dim intAmount As Decimal
    Dim intNetAmount As Decimal
    Dim strItem As String
    Dim strSelectedInvRcvId As String
    Dim strAcceptDateFormat As String
    Dim blnIsUpdated As Boolean = False
    Dim intConfig As Integer
    Dim strLocType As String
    Dim strInvRcvLnID As String
    Dim strPOID As String
    Dim strAccCode As String = ""
    Dim strTermType As String
    Dim strSelAccMonth As String
    Dim strSelAccYear As String
    Dim intLevel As Integer

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strAccMonth = Session("SS_APACCMONTH")
        strAccYear = Session("SS_APACCYEAR")
        strLangCode = Session("SS_LANGCODE")
        intAPAR = Session("SS_APAR")
        intConfig = Session("SS_CONFIGSETTING")
        strLocType = Session("SS_LOCTYPE")
        strSelAccMonth = Session("SS_SELACCMONTH")
        strSelAccYear = Session("SS_SELACCYEAR")
        intLevel = Session("SS_USRLEVEL")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumAPAccessRights.APInvoiceReceive), intAPAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            lblErrUnDel.Visible = False
            lblErrInvRcvRefDate.Visible = False
            lblErrQty.Visible = False
            lblErrPO.Visible = False
            lblerrInvDueDate.Visible = False
            lblErrTransDate.Visible = False
            errReqQty.Visible = False
            errReqCost.Visible = False
            lblErrTransDate.Visible = False
            lblFmtTransDate.Visible = False
            errUnmatchCost.Visible = False

            'to avoid double click, on aspx add this : UseSubmitBehavior="false"
            Addbtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(Addbtn).ToString())
            NewBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(NewBtn).ToString())
            SaveBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(SaveBtn).ToString())
            RefreshBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(RefreshBtn).ToString())
            ConfirmBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(ConfirmBtn).ToString())
            PrintBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(PrintBtn).ToString())
            CancelBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(CancelBtn).ToString())
            DeleteBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(DeleteBtn).ToString())
            UnDeleteBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(UnDeleteBtn).ToString())
            EditBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(EditBtn).ToString())
            PrintDocBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(PrintDocBtn).ToString())
            BackBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(BackBtn).ToString())
            btnAddAllItem.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(btnAddAllItem).ToString())

            strSelectedInvRcvId = Trim(IIf(Request.QueryString("inrid") = "", Request.Form("inrid"), Request.QueryString("inrid")))
            inrid.Value = strSelectedInvRcvId

            onload_GetLangCap()
            If Not IsPostBack Then
                BindChargeLevelDropDownList()
                rbSPO.Text = " " & objAPTrx.mtdGetInvoiceType(objAPTrx.EnumInvoiceType.SupplierPO)
                rbCWO.Text = " " & "Advance Payment" 'objAPTrx.mtdGetInvoiceType(objAPTrx.EnumInvoiceType.ContractorWorkOrder)
                rbOTE.Text = " " & objAPTrx.mtdGetInvoiceType(objAPTrx.EnumInvoiceType.Others)
                rbFFB.Text = " " & objAPTrx.mtdGetInvoiceType(objAPTrx.EnumInvoiceType.FFBSupplier)
                rbTransportFee.Text = " " & objAPTrx.mtdGetInvoiceType(objAPTrx.EnumInvoiceType.TransportFee)

                If strSelectedInvRcvId <> "" Then
                    onLoad_Display(strSelectedInvRcvId)
                    onLoad_DisplayItem(strSelectedInvRcvId)
                    onLoad_Button()
                    BindAccCode("")
                    BindPreBlock("", "")
                    BindBlock("", "")
                    BindVehicle("", "")
                    BindVehicleExpense(True, "")
                Else
                    txtInvoiceRcvRefDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
                    txtTransDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
                    lblStatusHidden.Text = "0"
                    BindPO("")
                    BindSupp("")
                    BindTermType("")
                    BindCreditTerm("")
                    BindAccCode("")
                    BindPreBlock("", "")
                    BindBlock("", "")

                    BindVehicle("", "")
                    BindVehicleExpense(True, "")
                    BindTTRefNo("")
                    onLoad_Button()
                    TrLink.Visible = True
                End If
            End If
            errOverQty.Visible = False
            lblErrAccCode.Visible = False
            lblPreBlockErr.Visible = False
            lblErrBlock.Visible = False
            lblErrVehicle.Visible = False
            lblErrVehicleExp.Visible = False

            lblUnqErrInvRcvRefNo.Visible = False
        End If
    End Sub

    Sub BindChargeLevelDropDownList()
        ddlChargeLevel.Items.Add(New ListItem(PreBlockTag, objLangCap.EnumLangCap.Block))
        ddlChargeLevel.Items.Add(New ListItem(BlockTag, objLangCap.EnumLangCap.SubBlock))
        ddlChargeLevel.SelectedIndex = Session("SS_BLOCK_CHARGE_DEFAULT")
        ToggleChargeLevel()
    End Sub

    Sub ddlChargeLevel_OnSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        ToggleChargeLevel()
    End Sub

    Sub ToggleChargeLevel()
        If ddlChargeLevel.SelectedIndex = 0 Then
            RowBlk.Visible = False
            RowPreBlk.Visible = True
            hidBlockCharge.Value = "yes"
        Else
            RowBlk.Visible = True
            RowPreBlk.Visible = False
            hidBlockCharge.Value = ""
        End If
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()

        lblTitle.Text = UCase(GetCaption(objLangCap.EnumLangCap.InvoiceReceive))
        lblInvoiceRcvIDTag.Text = GetCaption(objLangCap.EnumLangCap.InvoiceReceive) & lblID.Text
        lblInvoiceRcvRefNoTag.Text = GetCaption(objLangCap.EnumLangCap.InvoiceReceive) & lblRefNo.Text
        lblInvoiceRcvRefDateTag.Text = GetCaption(objLangCap.EnumLangCap.InvoiceReceive) & lblRefDate.Text

        Try
            If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfig) = True Then
                lblBlock.Text = GetCaption(objLangCap.EnumLangCap.Block) & lblCode.Text
                BlockTag = GetCaption(objLangCap.EnumLangCap.Block)
            Else
                lblBlock.Text = GetCaption(objLangCap.EnumLangCap.SubBlock) & lblCode.Text
                BlockTag = GetCaption(objLangCap.EnumLangCap.SubBlock)
            End If
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_TRX_CJDET_GET_LANGCAP_COSTLEVEL&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/AP_trx_CJDet.aspx")
        End Try

        lblAccount.Text = GetCaption(objLangCap.EnumLangCap.Account) & lblCode.Text
        lblVehicle.Text = GetCaption(objLangCap.EnumLangCap.Vehicle) & lblCode.Text
        lblVehExpense.Text = GetCaption(objLangCap.EnumLangCap.VehExpense) & lblCode.Text

        lblErrAccCode.Text = "<br>" & lblPleaseSelect.Text & lblAccount.Text
        lblErrBlock.Text = lblPleaseSelect.Text & lblBlock.Text
        lblErrVehicle.Text = lblPleaseSelect.Text & lblVehicle.Text
        lblErrVehicleExp.Text = lblPleaseSelect.Text & lblVehExpense.Text

        dgLineDet.Columns(1).HeaderText = lblAccount.Text
        dgLineDet.Columns(2).HeaderText = lblBlock.Text
        dgLineDet.Columns(3).HeaderText = lblVehicle.Text
        dgLineDet.Columns(4).HeaderText = lblVehExpense.Text
        PreBlockTag = GetCaption(objLangCap.EnumLangCap.Block)
        lblPreBlkTag.Text = PreBlockTag & lblCode.Text & " : "
        lblPreBlockErr.Text = lblPleaseSelect.Text & PreBlockTag & lblCode.Text
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_TRX_CNDET_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=ap/setup/AP_trx_CNList.aspx")
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


    Sub onLoad_Button()
        DeleteBtn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
        UnDeleteBtn.Attributes("onclick") = "javascript:return ConfirmAction('undelete');"
        CancelBtn.Attributes("onclick") = "javascript:return ConfirmAction('cancel');"

        SaveBtn.Visible = False
        RefreshBtn.Visible = False
        ConfirmBtn.Visible = False
        PrintBtn.Visible = False
        CancelBtn.Visible = False
        DeleteBtn.Visible = False
        UnDeleteBtn.Visible = False
        txtInvoiceRcvRefNo.Enabled = False
        txtInvoiceRcvRefDate.Enabled = False
        txtCreditTerm.Enabled = False
        txtRemark.Enabled = False
        rbSPO.Enabled = False
        rbCWO.Enabled = False
        rbOTE.Enabled = False
        rbFFB.Enabled = False
        rbTransportFee.Enabled = False
        ddlSuppCode.Enabled = False
        ddlPO.Enabled = False
        ddlTermType.Enabled = False
        btnSelDate.Visible = False
        tblSelection.Visible = False
        RowChargeLevel.Visible = False
        RowPreBlk.Visible = False
        validateItem.Enabled = False
        txtTransDate.Enabled = False
        ' ddlTTRefNo.Visible = False
        btnAddAllItem.Enabled = False
        lblPPH.Visible = True
        txtPPHRate.Visible = True
        lblPercen.Visible = True
        txtUsedDNCNAmount.ReadOnly = True
        EditBtn.Visible = False

        'If IIf(lblDNCNAmount.Text = "", 0, CDbl(lblDNCNAmount.Text)) <> 0 Then
        If CDbl(hidDNCNAmount.Value) <> 0 Then
            TrAdj.Visible = True
            txtUsedDNCNAmount.Visible = True
            txtOutPayAmount.Visible = True
            lblIDOutPayAmount.Visible = False
        Else
            TrAdj.Visible = False
            txtUsedDNCNAmount.Visible = False
            txtOutPayAmount.Visible = False
            lblIDOutPayAmount.Visible = True
        End If

        Select Case Trim(lblStatusHidden.Text)
            Case Trim(CStr(objAPTrx.EnumInvoiceRcvStatus.Confirmed))
                If CDbl(lblInvoiceAmount.Text) = CDbl(lblOutPayAmount.Text) Then
                    CancelBtn.Visible = True
                ElseIf CDbl(lblUsedAdvPayAmount.Text) > 0 And CDbl(lblOutPayAmount.Text) <> 0 Then
                    CancelBtn.Visible = True
                ElseIf CDbl(hidDNCNAmount.Value) <> 0 And CDbl(lblOutPayAmount.Text) <> 0 Then
                    CancelBtn.Visible = True
                End If
                If hidHasPaymentID.Value <> "" Then
                    CancelBtn.Visible = False
                End If
                txtInvDueDate.Enabled = False
                txtSplInvAmt.Enabled = False
                ddlTTRefNo.Enabled = False
                onLoad_DisplayAPCJ(lblInvoiceRcvID.Text)
            Case Trim(CStr(objAPTrx.EnumInvoiceRcvStatus.Deleted))
                UnDeleteBtn.Visible = True
            Case Trim(CStr(objAPTrx.EnumInvoiceRcvStatus.Cancelled))
            Case Trim(CStr(objAPTrx.EnumInvoiceRcvStatus.Writeoff))
            Case Trim(CStr(objAPTrx.EnumInvoiceRcvStatus.Closed))
            Case Trim(CStr(objAPTrx.EnumInvoiceRcvStatus.Paid))
            Case Else
                SaveBtn.Visible = True
                txtInvoiceRcvRefNo.Enabled = True
                txtInvoiceRcvRefDate.Enabled = True
                txtCreditTerm.Enabled = True
                txtRemark.Enabled = True
                rbSPO.Enabled = True
                rbCWO.Enabled = True
                rbOTE.Enabled = True
                rbFFB.Enabled = True
                rbTransportFee.Enabled = True
                txtTransDate.Enabled = True
                txtUsedDNCNAmount.ReadOnly = False
                If rbSPO.Checked = True Then
                    dgLineDet.Columns(13).Visible = False
                    ddlPO.Enabled = True
                    lblPPN.Visible = True
                    cbPPN.Enabled = False
                    cbPPN.Visible = True
                    ddlSuppCode.Enabled = False
                    lstItem.Enabled = True
                    validateItem.Enabled = True
                    btnAddAllItem.Enabled = True

                ElseIf rbCWO.Checked = True Then
                    dgLineDet.Columns(0).Visible = False
                    ddlPO.Enabled = False
                    ddlSuppCode.Enabled = True
                    lstItem.Enabled = False
                    validateItem.Enabled = False

                ElseIf rbFFB.Checked = True Then
                    txtInvoiceRcvRefNo.Enabled = False
                    lstItem.Enabled = True
                    validateItem.Enabled = True

                ElseIf rbTransportFee.Checked = True Then
                    dgLineDet.Columns(13).Visible = False
                    ddlPO.Enabled = True
                    lblPPN.Visible = False
                    cbPPN.Enabled = False
                    cbPPN.Visible = False
                    ddlSuppCode.Enabled = False
                    lstItem.Enabled = False
                    validateItem.Enabled = False
                    btnAddAllItem.Enabled = True
                Else
                    ddlPO.Enabled = False
                    ddlSuppCode.Enabled = True
                    lstItem.Enabled = False
                    validateItem.Enabled = False
                End If

                ddlTermType.Enabled = True
                btnSelDate.Visible = True
                tblSelection.Visible = True
                RowChargeLevel.Visible = Session("SS_BLOCK_CHARGE_VISIBLE")
                ToggleChargeLevel()
                If Trim(lblInvoiceRcvID.Text) <> "" Then
                    If dgLineDet.Items.Count <> 0 Then
                        rbSPO.Enabled = False
                        rbCWO.Enabled = False
                        rbOTE.Enabled = False
                        rbFFB.Enabled = False
                        rbTransportFee.Enabled = False
                        ddlPO.Enabled = False
                        ddlSuppCode.Enabled = False
                        txtTransDate.Enabled = False
                    Else
                        lblOutPayAmount.Text = 0
                        lblUsedAdvPayAmount.Text = 0
                    End If
                    RefreshBtn.Visible = True
                    ConfirmBtn.Visible = True
                    DeleteBtn.Visible = True
                End If
        End Select

        If CDbl(hidDNCNAmount.Value) <> 0 And CDbl(Replace(Replace(txtUsedDNCNAmount.Text.Trim, ".", ""), ",", ".")) <> 0 Then
            ConfirmBtn.Attributes("onclick") = "javascript:return ConfirmAction('confirm with " & Trim(lblCurrency6.Text) & " " & Trim(txtUsedDNCNAmount.Text) & " as creditor journal allocation amount');"
        End If

        'btnAddAllItem.Attributes("onclick") = "javascript:return ConfirmAction('create this transaction without Tanda Terima Tagihan Ref. No.');"
    End Sub

    Sub onLoad_Display(ByVal pv_strInvRcvId As String)
        Dim strOpCd_Get As String = "AP_CLSTRX_INVOICERECEIVE_DETAILS_GET"
        Dim objInvRcvDs As New Object
        Dim intErrNo As Integer
        Dim strParam As String = pv_strInvRcvId & "|"
        Dim intCnt As Integer = 0
        Dim strAdvAmount As String
        Dim strUsedAdvAmount As String
        Dim dblAdvAmount As Double
        Dim dblUsedAdvAmount As Double

        Try
            intErrNo = objAPTrx.mtdGetInvoiceRcv(strOpCd_Get, _
                                                 strCompany, _
                                                 strLocation, _
                                                 strUserId, _
                                                 strAccMonth, _
                                                 strAccYear, _
                                                 strParam, _
                                                 objInvRcvDs, _
                                                 True)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_GET_HEADER&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
        End Try

        lblInvoiceRcvID.Text = pv_strInvRcvId
        txtInvoiceRcvRefNo.Text = Trim(objInvRcvDs.Tables(0).Rows(0).Item("InvoiceRcvRefNo"))
        txtInvoiceRcvRefDate.Text = Date_Validation(objInvRcvDs.Tables(0).Rows(0).Item("InvoiceRcvRefDate"), True)
        idSuppCode.Value = Trim(objInvRcvDs.Tables(0).Rows(0).Item("SupplierCode"))
        lblAccPeriod.Text = Trim(objInvRcvDs.Tables(0).Rows(0).Item("AccMonth")) & "/" & Trim(objInvRcvDs.Tables(0).Rows(0).Item("AccYear"))
        lblStatus.Text = objAPTrx.mtdGetInvoiceRcvStatus(Trim(objInvRcvDs.Tables(0).Rows(0).Item("Status")))
        lblStatusHidden.Text = Trim(objInvRcvDs.Tables(0).Rows(0).Item("Status"))
        lblDateCreated.Text = objGlobal.GetLongDate(objInvRcvDs.Tables(0).Rows(0).Item("CreateDate"))
        lblLastUpdate.Text = objGlobal.GetLongDate(objInvRcvDs.Tables(0).Rows(0).Item("UpdateDate"))
        lblPrintDate.Text = objGlobal.GetLongDate(objInvRcvDs.Tables(0).Rows(0).Item("PrintDate"))
        lblUpdatedBy.Text = Trim(objInvRcvDs.Tables(0).Rows(0).Item("UserName"))
        lblInvoiceAmount.Text = FormatNumber(objInvRcvDs.Tables(0).Rows(0).Item("TotalAmountToDisplay"), 2)
        hidInvAmount.Value = objInvRcvDs.Tables(0).Rows(0).Item("TotalAmountToDisplay")
        lblCurrency1.Text = Trim(objInvRcvDs.Tables(0).Rows(0).Item("CurrencyCode"))
        lblIDInvoiceAmount.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(objInvRcvDs.Tables(0).Rows(0).Item("TotalAmountToDisplay"), 2), 2)
        lblOutPayAmount.Text = FormatNumber(objInvRcvDs.Tables(0).Rows(0).Item("OutstandingAmountToDisplay"), 2)
        lblCurrency2.Text = Trim(objInvRcvDs.Tables(0).Rows(0).Item("CurrencyCode"))
        lblIDOutPayAmount.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(objInvRcvDs.Tables(0).Rows(0).Item("OutstandingAmountToDisplay"), 2), 2)
        txtRemark.Text = Trim(objInvRcvDs.Tables(0).Rows(0).Item("Remark"))
        hidCurrencyCode.Value = Trim(objInvRcvDs.Tables(0).Rows(0).Item("CurrencyCode"))
        hidExchangeRate.Value = FormatNumber(objInvRcvDs.Tables(0).Rows(0).Item("ExchangeRate"), 2)
        strPOID = Trim(objInvRcvDs.Tables(0).Rows(0).Item("POID"))
        txtInvDueDate.Text = Date_Validation(objInvRcvDs.Tables(0).Rows(0).Item("DueDate"), True)
        hidTermType.Value = Trim(objInvRcvDs.Tables(0).Rows(0).Item("TermType"))
        hidHasPaymentID.Value = Trim(objInvRcvDs.Tables(0).Rows(0).Item("HasPaymentID"))

        If objInvRcvDs.Tables(0).Rows(0).Item("AdvAmount") <> 0 Then
            'txtSplInvAmt.Text = objInvRcvDs.Tables(0).Rows(0).Item("SupplierInvAmount") / IIf(Trim(objInvRcvDs.Tables(0).Rows(0).Item("AdvCurrencyCode")) = "IDR", objInvRcvDs.Tables(0).Rows(0).Item("ExchangeRate"), objInvRcvDs.Tables(0).Rows(0).Item("AdvExchangeRate"))
            txtSplInvAmt.Text = objInvRcvDs.Tables(0).Rows(0).Item("SupplierInvAmount")
        Else
            txtSplInvAmt.Text = objInvRcvDs.Tables(0).Rows(0).Item("SupplierInvAmount")
        End If

        txtTransDate.Text = Date_Validation(objInvRcvDs.Tables(0).Rows(0).Item("CreateDate"), True)


        lblCurrency3.Text = Trim(objInvRcvDs.Tables(0).Rows(0).Item("CurrencyCode"))
        lblCurrency4.Text = Trim(objInvRcvDs.Tables(0).Rows(0).Item("CurrencyCode"))
        hidPaymentID.Value = Trim(objInvRcvDs.Tables(0).Rows(0).Item("PaymentID"))
        hidAdvExchangeRate.Value = objInvRcvDs.Tables(0).Rows(0).Item("AdvExchangeRate")


        If Trim(objInvRcvDs.Tables(0).Rows(0).Item("AdvCurrencyCode")) = Trim(objInvRcvDs.Tables(0).Rows(0).Item("CurrencyCode")) Then
            dblUsedAdvAmount = objInvRcvDs.Tables(0).Rows(0).Item("UsedAdvAmount") '/ objInvRcvDs.Tables(0).Rows(0).Item("AdvExchangeRate")
            strUsedAdvAmount = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(dblUsedAdvAmount, 2), 2)
            lblUsedAdvPayAmount.Text = FormatNumber(dblUsedAdvAmount, 2)
            lblIDUsedAdvPayAmount.Text = strUsedAdvAmount
            hidUsedAdvAmount.Value = dblUsedAdvAmount

            'dblAdvAmount = objInvRcvDs.Tables(0).Rows(0).Item("AdvAmount") / objInvRcvDs.Tables(0).Rows(0).Item("AdvExchangeRate")
            dblAdvAmount = objInvRcvDs.Tables(0).Rows(0).Item("TotalAdvAmount")
            strAdvAmount = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(dblAdvAmount, 2), 2)
            lblAdvPaymentAmount.Text = strAdvAmount
            hidAdvPayment.Value = dblAdvAmount
        Else
            If Trim(objInvRcvDs.Tables(0).Rows(0).Item("CurrencyCode")) = "IDR" Then
                dblUsedAdvAmount = (objInvRcvDs.Tables(0).Rows(0).Item("UsedAdvAmount"))
                strUsedAdvAmount = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(dblUsedAdvAmount, 2), 2)
                lblUsedAdvPayAmount.Text = FormatNumber(dblUsedAdvAmount, 2)
                lblIDUsedAdvPayAmount.Text = strUsedAdvAmount
                hidUsedAdvAmount.Value = dblUsedAdvAmount

                dblAdvAmount = objInvRcvDs.Tables(0).Rows(0).Item("AdvAmount")
                strAdvAmount = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(dblAdvAmount, 2), 2)
                lblAdvPaymentAmount.Text = strAdvAmount & " (" & objInvRcvDs.Tables(0).Rows(0).Item("AdvCurrencyCode") & " " & objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(dblAdvAmount / objInvRcvDs.Tables(0).Rows(0).Item("AdvExchangeRate"), 2), 2) & ")"
                hidAdvPayment.Value = dblAdvAmount
            Else
                dblUsedAdvAmount = (objInvRcvDs.Tables(0).Rows(0).Item("UsedAdvAmount")) '/ objInvRcvDs.Tables(0).Rows(0).Item("ExchangeRate")
                strUsedAdvAmount = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(dblUsedAdvAmount, 2), 2)
                lblUsedAdvPayAmount.Text = FormatNumber(dblUsedAdvAmount, 2)
                lblIDUsedAdvPayAmount.Text = strUsedAdvAmount
                hidUsedAdvAmount.Value = dblUsedAdvAmount

                dblAdvAmount = objInvRcvDs.Tables(0).Rows(0).Item("AdvAmount") / objInvRcvDs.Tables(0).Rows(0).Item("ExchangeRate")
                strAdvAmount = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(dblAdvAmount, 2), 2)
                lblAdvPaymentAmount.Text = strAdvAmount
                lblAdvPaymentAmount.Text = strAdvAmount & " (" & objInvRcvDs.Tables(0).Rows(0).Item("AdvCurrencyCode") & " " & objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(objInvRcvDs.Tables(0).Rows(0).Item("AdvAmount"), 2), 2) & ")"
                hidAdvPayment.Value = dblAdvAmount
            End If
        End If

        hidDNCNID.Value = Trim(objInvRcvDs.Tables(0).Rows(0).Item("DNCNID"))
        lblCurrency5.Text = Trim(objInvRcvDs.Tables(0).Rows(0).Item("CurrencyCode"))
        lblCurrency6.Text = Trim(objInvRcvDs.Tables(0).Rows(0).Item("CurrencyCode"))
        lblDNCNAmount.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(objInvRcvDs.Tables(0).Rows(0).Item("DNCNAmount"), 2), 2)
        hidDNCNAmount.Value = objInvRcvDs.Tables(0).Rows(0).Item("DNCNAmount")
        txtUsedDNCNAmount.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(objInvRcvDs.Tables(0).Rows(0).Item("UsedDNCNAmount"), 2), 2)
        If CDbl(hidDNCNAmount.Value) <> 0 Then
            TrAdj.Visible = True
            txtUsedDNCNAmount.Visible = True
            txtOutPayAmount.Visible = True
            lblIDOutPayAmount.Visible = False
            ConfirmBtn.Attributes("onclick") = "javascript:return ConfirmAction('confirm with " & Trim(lblCurrency6.Text) & " " & Trim(txtUsedDNCNAmount.Text) & " as creditor journal allocation amount');"
        Else
            TrAdj.Visible = False
            txtUsedDNCNAmount.Visible = False
            txtOutPayAmount.Visible = False
            lblIDOutPayAmount.Visible = True
        End If

        If Trim(objInvRcvDs.Tables(0).Rows(0).Item("InvoiceType")) = objAPTrx.EnumInvoiceType.SupplierPO Then
            rbSPO.Checked = True
            rbCWO.Checked = False
            rbOTE.Checked = False
            rbFFB.Checked = False
            rbTransportFee.Checked = False
            If lblInvoiceRcvID.Text = "" Then
                onLoad_DisplayAdvancePayment(strPOID)
                onLoad_DisplayDNCNAmount(strPOID)
            Else
                'lblIDOutPayAmount.Text = hidInvAmount.Value - (objInvRcvDs.Tables(0).Rows(0).Item("UsedAdvAmount") / IIf(Trim(lblStatusHidden.Text) = Trim(CStr(objAPTrx.EnumInvoiceRcvStatus.Confirmed)), objInvRcvDs.Tables(0).Rows(0).Item("ExchangeRate"), objInvRcvDs.Tables(0).Rows(0).Item("AdvExchangeRate"))) ' hidAdvPayment.Value
                'lblIDOutPayAmount.Text = hidInvAmount.Value - (objInvRcvDs.Tables(0).Rows(0).Item("UsedAdvAmount") / IIf(Trim(objInvRcvDs.Tables(0).Rows(0).Item("AdvCurrencyCode")) = "IDR", objInvRcvDs.Tables(0).Rows(0).Item("ExchangeRate"), objInvRcvDs.Tables(0).Rows(0).Item("AdvExchangeRate")))
                lblIDOutPayAmount.Text = hidInvAmount.Value - hidUsedAdvAmount.Value + objInvRcvDs.Tables(0).Rows(0).Item("UsedDNCNAmount")
                If lblIDOutPayAmount.Text < 0 Then
                    lblIDOutPayAmount.Text = 0
                End If
                lblIDOutPayAmount.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(lblIDOutPayAmount.Text, 2), 2)
                txtOutPayAmount.Text = lblIDOutPayAmount.Text
                hidOutPayAmount.Value = hidInvAmount.Value - hidUsedAdvAmount.Value
                'lblIDUsedAdvPayAmount.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber((objInvRcvDs.Tables(0).Rows(0).Item("UsedAdvAmount") / IIf(Trim(lblStatusHidden.Text) = Trim(CStr(objAPTrx.EnumInvoiceRcvStatus.Confirmed)), objInvRcvDs.Tables(0).Rows(0).Item("ExchangeRate"), objInvRcvDs.Tables(0).Rows(0).Item("AdvExchangeRate"))), 2), 2)
                'lblIDUsedAdvPayAmount.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber((objInvRcvDs.Tables(0).Rows(0).Item("UsedAdvAmount") / IIf(Trim(objInvRcvDs.Tables(0).Rows(0).Item("AdvCurrencyCode")) = "IDR", objInvRcvDs.Tables(0).Rows(0).Item("ExchangeRate"), objInvRcvDs.Tables(0).Rows(0).Item("AdvExchangeRate"))), 2), 2)
            End If
        ElseIf Trim(objInvRcvDs.Tables(0).Rows(0).Item("InvoiceType")) = objAPTrx.EnumInvoiceType.ContractorWorkOrder Then
            rbSPO.Checked = False
            rbCWO.Checked = True
            rbOTE.Checked = False
            rbFFB.Checked = False
            rbTransportFee.Checked = False
        ElseIf Trim(objInvRcvDs.Tables(0).Rows(0).Item("InvoiceType")) = objAPTrx.EnumInvoiceType.Others Then
            rbSPO.Checked = False
            rbCWO.Checked = False
            rbOTE.Checked = True
            rbFFB.Checked = False
            rbTransportFee.Checked = False
        ElseIf Trim(objInvRcvDs.Tables(0).Rows(0).Item("InvoiceType")) = objAPTrx.EnumInvoiceType.FFBSupplier Then
            rbSPO.Checked = False
            rbCWO.Checked = False
            rbOTE.Checked = False
            rbFFB.Checked = True
            rbTransportFee.Checked = False
        Else
            rbSPO.Checked = False
            rbCWO.Checked = False
            rbOTE.Checked = False
            rbFFB.Checked = False
            rbTransportFee.Checked = True
        End If

        If rbSPO.Checked = True Then
            dgLineDet.Columns(13).Visible = False
            ddlPO.Enabled = rbSPO.Enabled
            lblPPN.Visible = True
            cbPPN.Enabled = False
            cbPPN.Visible = True
            ddlSuppCode.Enabled = False
            ddlSuppCode.SelectedIndex = 0
            lblErrSuppCode.Visible = False
            lstItem.Enabled = True
            validateItem.Enabled = True
            lblPPH.Visible = True
            lblPercen.Visible = True
            txtPPHRate.Enabled = False
            txtPPHRate.Visible = True
        Else
            If rbCWO.Checked = True Then
                dgLineDet.Columns(0).Visible = False
                ddlPO.Enabled = False
                ddlPO.SelectedIndex = 0
                lblPPN.Visible = True
                cbPPN.Enabled = True
                cbPPN.Visible = True
                ddlSuppCode.Enabled = rbCWO.Enabled
                lstItem.Enabled = False
                lblPPH.Visible = True
                lblPercen.Visible = True
                txtPPHRate.Enabled = True
                txtPPHRate.Visible = True
            Else
                If rbOTE.Checked = True Then
                    ddlPO.Enabled = False
                    ddlPO.SelectedIndex = 0
                    lblPPN.Visible = True
                    cbPPN.Enabled = True
                    cbPPN.Visible = True
                    ddlSuppCode.Enabled = rbOTE.Enabled
                    lstItem.Enabled = True
                    validateItem.Enabled = True
                    lblPPH.Visible = True
                    lblPercen.Visible = True
                    txtPPHRate.Enabled = True
                    txtPPHRate.Visible = True
                Else
                    dgLineDet.Columns(13).Visible = False
                    ddlPO.Enabled = rbSPO.Enabled
                    lblPPN.Visible = True
                    cbPPN.Enabled = False
                    cbPPN.Visible = True
                    ddlSuppCode.Enabled = False
                    ddlSuppCode.SelectedIndex = 0
                    lblErrSuppCode.Visible = False
                    lstItem.Enabled = True
                    validateItem.Enabled = True
                    lblPPH.Visible = True
                    lblPercen.Visible = True
                    txtPPHRate.Enabled = False
                    txtPPHRate.Visible = True
                End If
            End If
        End If

        'BindPO(Trim(objInvRcvDs.Tables(0).Rows(0).Item("POID")))
        BindSupp(idSuppCode.Value)
        BindTermType(Trim(objInvRcvDs.Tables(0).Rows(0).Item("TermType")))
        txtCreditTerm.Text = Trim(objInvRcvDs.Tables(0).Rows(0).Item("CreditTerm"))
        'onLoad_DisplayAPNote(lblInvoiceRcvID.Text)
    End Sub

    Sub onLoad_DisplayItem(ByVal pv_strInvoiceRcvID As String)
        Dim strOpCd_GetPOLine As String = "AP_CLSTRX_INVOICERECEIVE_POLINE_GET"
        Dim strOpCd_GetIRLine As String = "AP_CLSTRX_INVOICERECEIVE_LINE_GET"
        Dim strOpCodes As String = strOpCd_GetPOLine & "|" & strOpCd_GetIRLine
        Dim strParam As String = pv_strInvoiceRcvID & "|" & strLocation
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim dr As DataRow

        Dim strParamName As String
        Dim strParamValue As String

        strParamName = "INVOICERCVID|LOCCODE"
        strParamValue = Trim(pv_strInvoiceRcvID) & "|" & strLocation

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_GetIRLine, _
                                                strParamName, _
                                                strParamValue, _
                                                objInvRcvLnDs)

            'Try
            '    intErrNo = objAPTrx.mtdGetInvoiceRcvLine(strOpCodes, _
            '                                             strCompany, _
            '                                             strLocation, _
            '                                             strUserId, _
            '                                             strAccMonth, _
            '                                             strAccYear, _
            '                                             strParam, _
            '                                             objInvRcvLnDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_GET_LINE&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
        End Try

        For intCnt = 0 To objInvRcvLnDs.Tables(0).Rows.Count - 1
            objInvRcvLnDs.Tables(0).Rows(intCnt).Item("InvoiceRcvLnID") = Trim(objInvRcvLnDs.Tables(0).Rows(intCnt).Item("InvoiceRcvLnID"))
            objInvRcvLnDs.Tables(0).Rows(intCnt).Item("ItemCode") = Trim(objInvRcvLnDs.Tables(0).Rows(intCnt).Item("ItemCode"))
            objInvRcvLnDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objInvRcvLnDs.Tables(0).Rows(intCnt).Item("Description"))
            objInvRcvLnDs.Tables(0).Rows(intCnt).Item("UOMCode") = Trim(objInvRcvLnDs.Tables(0).Rows(intCnt).Item("UOMCode"))
            objInvRcvLnDs.Tables(0).Rows(intCnt).Item("UOMDesc") = Trim(objInvRcvLnDs.Tables(0).Rows(intCnt).Item("UOMDesc"))
            objInvRcvLnDs.Tables(0).Rows(intCnt).Item("Cost") = objInvRcvLnDs.Tables(0).Rows(intCnt).Item("Cost")
            strInvRcvLnID = objInvRcvLnDs.Tables(0).Rows(intCnt).Item("InvoiceRcvLnID")
            txtPPHRate.Text = objInvRcvLnDs.Tables(0).Rows(intCnt).Item("PPHRate")
            cbPPN.Checked = IIf(objInvRcvLnDs.Tables(0).Rows(intCnt).Item("PPN") = objAPTrx.EnumPPN.Yes, True, False)
        Next intCnt

        dgLineDet.DataSource = objInvRcvLnDs.Tables(0)
        dgLineDet.DataBind()
        If rbSPO.Checked = True Then
            lblPPN.Visible = True
            cbPPN.Enabled = False
            cbPPN.Visible = True
            lblPPH.Visible = True
            lblPercen.Visible = True
            txtPPHRate.Text = "0"
            txtPPHRate.Enabled = False
            txtPPHRate.Visible = True
        Else
            If strInvRcvLnID <> "" Then
                cbPPN.Enabled = False
                txtPPHRate.Enabled = False
            Else
                cbPPN.Checked = False
                cbPPN.Enabled = True
                txtPPHRate.Text = "0"
                txtPPHRate.Enabled = True
            End If
        End If

        BindPO(strPOID)

        If objInvRcvLnDs.Tables(0).Rows.Count > 0 Then
            If rbOTE.Checked = True Then
                TrLink.Visible = False
            Else
                TrLink.Visible = True
            End If
        Else
            Dim strOpCd_UpdAdvPayment = "AP_CLSTRX_INVOICERECEIVE_UPD_DNCNAMOUNT"
           
            strParamName = "LOCCODE|INVOICERCVID|DNCNID|DNCNAMOUNT|USEDDNCNAMOUNT"
            strParamValue = strLocation & "|" & _
                            lblInvoiceRcvID.Text & "|" & _
                            "" & "|" & _
                            0 & "|" & _
                            0

            Try
                intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd_UpdAdvPayment, _
                                                        strParamName, _
                                                        strParamValue)

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_UPD_POSTATUS&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
            End Try

            onLoad_DisplayAdvancePayment(strPOID)
            onLoad_DisplayDNCNAmount(strPOID)
            TrLink.Visible = False
        End If

        'If objInvRcvLnDs.Tables(0).Rows.Count = 0 Then
        '    Dim objAPNote As Object
        '    Dim strOpCd_APNote As String
        '    Dim strAPNoteID As String
        '    Dim strParamName As String
        '    Dim strParamValue As String

        '    strOpCd_APNote = "AP_CLSTRX_INVOICERCV_NOTE_DELETE"
        '    strParamName = "STRSEARCH"
        '    strParamValue = "InvoiceRcvID = '" & Trim(pv_strInvoiceRcvID) & "' "

        '    Try
        '        intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd_APNote, _
        '                                                strParamName, _
        '                                                strParamValue)

        '    Catch Exp As System.Exception
        '        Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=UPD_ITEM&errmesg=" & Exp.ToString() & "&redirect=pu/trx/AP_trx_InvoiceRcvList")
        '    End Try
        '    onLoad_DisplayAPNote(lblInvoiceRcvID.Text)
        'End If
    End Sub



    Sub DataGrid_ItemData(ByVal Sender As Object, ByVal e As DataGridItemEventArgs) Handles dgLineDet.ItemDataBound
        Dim btn As LinkButton
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Select Case CInt(lblStatusHidden.Text)
                Case objAPTrx.EnumInvoiceRcvStatus.Confirmed, objAPTrx.EnumInvoiceRcvStatus.Deleted, objAPTrx.EnumInvoiceRcvStatus.Cancelled, objAPTrx.EnumInvoiceRcvStatus.Writeoff, objAPTrx.EnumInvoiceRcvStatus.Paid
                    btn = e.Item.FindControl("lbDelete")
                    btn.Visible = False
                Case Else
                    'simon 
                    If rbFFB.Checked = False Then
                        btn = e.Item.FindControl("lbDelete")
                        btn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                    Else
                        btn = e.Item.FindControl("lbDelete")
                        btn.Visible = False
                    End If
            End Select
        End If
    End Sub

    Sub BindPO(ByVal pv_strPOID As String)
        Dim strOpCd_GetPO As String = "PU_CLSTRX_PO_INVOICERECEIVE_GET"
        Dim strOpCd_GetPOLn As String = "PU_CLSTRX_PO_LINE_GET"
        Dim strOpCd_GetPO_TransportFee As String = "PU_CLSTRX_PO_INVOICERECEIVE_GET_TRANSPORTFEE"
        Dim strOpCd_Get As String = "AP_CLSTRX_INVOICERECEIVE_DETAILS_GET"
        Dim strParam As String = "||||||||"
        Dim intCnt As Integer = 0
        Dim intErrNo As Integer
        Dim intSelectedIndex As Integer = 0
        Dim blnFound As Boolean = False
        Dim dr As DataRow
        Dim strPOID As String

        lstItem.Items.Clear()
        lstItem.Items.Add(New ListItem(lblPleaseSelect.Text & "Item Code", ""))

        Dim strOpCd_ItemGet As String = "IN_CLSSETUP_INVITEM_LIST_GET"
        Dim objItemDs As DataSet

        If rbOTE.Checked = True Or rbFFB.Checked = True Then
            ddlPO.Items.Clear()
            ddlPO.Items.Add(New ListItem(lblPleaseSelect.Text & "Purchase Order ID", ""))

            strParam = "Order By ItemCode | AND LocCode = '" & strLocation & "' AND itm.Status = '" & objINSetup.EnumStockItemStatus.Active & "'"
            intErrNo = objINSetup.mtdGetMasterList(strOpCd_ItemGet, strParam, 0, objItemDs)
            For intCnt = 0 To objItemDs.Tables(0).Rows.Count - 1
                objItemDs.Tables(0).Rows(intCnt).Item("Description") = objItemDs.Tables(0).Rows(intCnt).Item("ItemCode") & " (" & _
                                                                            Trim(objItemDs.Tables(0).Rows(intCnt).Item("Description")) & "), " & _
                                                                        "Rp. " & objItemDs.Tables(0).Rows(intCnt).Item("LatestCost") & ", " & _
                                                                        objGlobal.GetIDDecimalSeparator_FreeDigit(objItemDs.Tables(0).Rows(intCnt).Item("QtyOnHand"), 5) & ", " & _
                                                                        Trim(objItemDs.Tables(0).Rows(intCnt).Item("UOMCode"))



                objItemDs.Tables(0).Rows(intCnt).Item("ItemCode") = "|" & Trim(objItemDs.Tables(0).Rows(intCnt).Item("ItemCode")) & "|"
            Next
            dr = objItemDs.Tables(0).NewRow()
            dr("ItemCode") = ""
            dr("Description") = lblPleaseSelect.Text & "Item Code"
            objItemDs.Tables(0).Rows.InsertAt(dr, 0)
            lstItem.DataSource = objItemDs.Tables(0)
            lstItem.DataValueField = "ItemCode"
            lstItem.DataTextField = "Description"
            lstItem.DataBind()
            Exit Sub
        Else
            strParam = "|" & strLocation & "||||||A.POID|"
        End If


        Try
            intErrNo = objPUTrx.mtdGetPO_New(IIf(rbSPO.Checked = True, strOpCd_GetPO, strOpCd_GetPO_TransportFee), strAccMonth, strAccYear, strParam, objPODs)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_GET_PODETAIL&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
        End Try

        For intCnt = 0 To objPODs.Tables(0).Rows.Count - 1
            objPODs.Tables(0).Rows(intCnt).Item("POID") = Trim(objPODs.Tables(0).Rows(intCnt).Item("POID"))
            'objPODs.Tables(0).Rows(intCnt).Item("DispPOId") = Trim(objPODs.Tables(0).Rows(intCnt).Item("POId")) & ", " & Trim(objPODs.Tables(0).Rows(intCnt).Item("Remark"))
            objPODs.Tables(0).Rows(intCnt).Item("DispPOId") = Trim(objPODs.Tables(0).Rows(intCnt).Item("POId")) & ", " & _
                                                              Trim(objPODs.Tables(0).Rows(intCnt).Item("CurrencyCode")) & ", " & _
                                                              Trim(objPODs.Tables(0).Rows(intCnt).Item("ExchangeRate")) & ", " & _
                                                              "Rp. " & objPODs.Tables(0).Rows(intCnt).Item("Amount")
            If pv_strPOID = objPODs.Tables(0).Rows(intCnt).Item("POID") Then
                intSelectedIndex = intCnt + 1
                blnFound = True
            End If
        Next intCnt

        If blnFound = True Then
            If objPODs.Tables(0).Rows.Count > 0 Then
                idSuppCode.Value = Trim(objPODs.Tables(0).Rows(intSelectedIndex - 1).Item("SupplierCode"))
                txtCreditTerm.Text = Trim(objPODs.Tables(0).Rows(intSelectedIndex - 1).Item("POCreditTerm"))
                hidExchangeRate.Value = Trim(objPODs.Tables(0).Rows(intSelectedIndex - 1).Item("ExchangeRate"))
                BindSupp(idSuppCode.Value)

                'strParam = IIf(pv_strPOID = "", _
                '            objPODs.Tables(0).Rows(intSelectedIndex - 1).Item("POID") & "||||||", _
                '            pv_strPOID & "||||||")
                'Try
                '    intErrNo = objPUTrx.mtdGetPOLn(strOpCd_GetPOLn, _
                '                                   strCompany, _
                '                                   strLocation, _
                '                                   strUserId, _
                '                                   strAccMonth, _
                '                                   strAccYear, _
                '                                   strParam, _
                '                                   objPOLnDs)
                'Catch Exp As System.Exception
                '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_GET_POLINE&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
                'End Try

                Dim strParamName As String
                Dim strParamValue As String

                strParamName = "STRSEARCH"
                'If lblInvoiceRcvID.Text <> "" Then
                '    strParamValue = "AND A.POId = '" & IIf(pv_strPOID = "", objPODs.Tables(0).Rows(intSelectedIndex - 1).Item("POID"), pv_strPOID) & "' " & _
                '                    " AND D.LocCode = '" & Trim(strLocation) & "'" & _
                '                    " AND A.QtyReceive > 0 " & _
                '                    " AND RTrim(A.ItemCode)+RTrim(A.POLNID)+Rtrim(QtyReceive) NOT IN (Select RTrim(ItemCode)+RTrim(POLNID)+RTrim(QtyInvoice) From AP_InvoiceRcvLn A, AP_InvoiceRcv B Where A.InvoiceRcvID=B.InvoiceRcvID AND Status NOT IN ('3','4') AND A.InvoiceRcvID = '" & Trim(lblInvoiceRcvID.Text) & "' AND LocCode = '" & Trim(strLocation) & "')"
                'Else
                strParamValue = "AND A.POId = '" & IIf(pv_strPOID = "", objPODs.Tables(0).Rows(intSelectedIndex - 1).Item("POID"), pv_strPOID) & "' " & _
                                " AND D.LocCode = '" & Trim(strLocation) & "' " & _
                                " AND A.QtyReceive > 0 " & _
                                " AND RTrim(A.ItemCode)+RTrim(A.POLNID)+Rtrim(QtyReceive) NOT IN (Select RTrim(ItemCode)+RTrim(POLNID)+RTrim(QtyInvoice) From AP_InvoiceRcvLn A, AP_InvoiceRcv B Where A.InvoiceRcvID=B.InvoiceRcvID AND Status NOT IN ('3','4') AND LocCode = '" & Trim(strLocation) & "')"
                '
                'End If

                Try
                    intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_GetPOLn, _
                                                        strParamName, _
                                                        strParamValue, _
                                                        objPOLnDs)

                Catch Exp As System.Exception
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_GET_POLINE&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
                End Try

                For intCnt = 0 To objPOLnDs.Tables(0).Rows.Count - 1
                    objPOLnDs.Tables(0).Rows(intCnt).Item("POLnId") = objPOLnDs.Tables(0).Rows(intCnt).Item("POLnId") & "|" & objPOLnDs.Tables(0).Rows(intCnt).Item("ItemCode") & "|" & objPOLnDs.Tables(0).Rows(intCnt).Item("QtyReceive") - objPOLnDs.Tables(0).Rows(intCnt).Item("QtyInvoice") & "|" & objPOLnDs.Tables(0).Rows(intCnt).Item("POId")

                    If rbTransportFee.Checked = True Then
                        cbPPN.Checked = False
                    Else
                        cbPPN.Checked = IIf(objPOLnDs.Tables(0).Rows(intCnt).Item("PPNCheck") = objAPTrx.EnumPPN.Yes, True, False)
                    End If


                    If objPOLnDs.Tables(0).Rows(intCnt).Item("PartNo") <> "" Then


                        objPOLnDs.Tables(0).Rows(intCnt).Item("Description") = objPOLnDs.Tables(0).Rows(intCnt).Item("ItemCode") & " @ " & _
                                                                               objPOLnDs.Tables(0).Rows(intCnt).Item("PartNo") & " (" & _
                                                                               objPOLnDs.Tables(0).Rows(intCnt).Item("Description") & ") - " & _
                                                                               "Rp. " & objPOLnDs.Tables(0).Rows(intCnt).Item("CostAftDisc") & ", " & _
                                                                               objGlobal.GetIDDecimalSeparator_FreeDigit(objPOLnDs.Tables(0).Rows(intCnt).Item("QtyReceive") - objPOLnDs.Tables(0).Rows(intCnt).Item("QtyInvoice"), 5) & ", " & _
                                                                               Trim(objPOLnDs.Tables(0).Rows(intCnt).Item("UOMCode"))
                    Else

                        objPOLnDs.Tables(0).Rows(intCnt).Item("Description") = objPOLnDs.Tables(0).Rows(intCnt).Item("ItemCode") & " (" & _
                                                                               objPOLnDs.Tables(0).Rows(intCnt).Item("Description") & ") - " & _
                                                                               "Rp. " & objPOLnDs.Tables(0).Rows(intCnt).Item("CostAftDisc") & ", " & _
                                                                               objGlobal.GetIDDecimalSeparator_FreeDigit(objPOLnDs.Tables(0).Rows(intCnt).Item("QtyReceive") - objPOLnDs.Tables(0).Rows(intCnt).Item("QtyInvoice"), 5) & ", " & _
                                                                               Trim(objPOLnDs.Tables(0).Rows(intCnt).Item("UOMCode"))
                    End If

                Next

                dr = objPOLnDs.Tables(0).NewRow()
                dr("POLnId") = ""
                dr("Description") = lblPleaseSelect.Text & "Item Code"
                objPOLnDs.Tables(0).Rows.InsertAt(dr, 0)

                lstItem.DataSource = objPOLnDs.Tables(0)
                lstItem.DataValueField = "POLnId"
                lstItem.DataTextField = "Description"
                lstItem.DataBind()
            End If

        ElseIf pv_strPOID <> "" Then
            dr = objPODs.Tables(0).NewRow()
            dr("POID") = pv_strPOID
            objPODs.Tables(0).Rows.InsertAt(dr, objPODs.Tables(0).Rows.Count)
            intSelectedIndex = objPODs.Tables(0).Rows.Count
        End If

        If lblInvoiceRcvID.Text <> "" And strInvRcvLnID <> "" Then
            strParam = lblInvoiceRcvID.Text & "|"
            Try
                intErrNo = objAPTrx.mtdGetInvoiceRcv(strOpCd_Get, _
                                                     strCompany, _
                                                     strLocation, _
                                                     strUserId, _
                                                     strAccMonth, _
                                                     strAccYear, _
                                                     strParam, _
                                                     objPODs, _
                                                     True)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_GET_HEADER&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
            End Try

            For intCnt = 0 To objPODs.Tables(0).Rows.Count - 1
                objPODs.Tables(0).Rows(intCnt).Item("POID") = Trim(objPODs.Tables(0).Rows(intCnt).Item("POID"))
                objPODs.Tables(0).Rows(intCnt).Item("DispPOId") = Trim(objPODs.Tables(0).Rows(intCnt).Item("POId")) & ", " & Trim(objPODs.Tables(0).Rows(intCnt).Item("PORemark"))

                If objPODs.Tables(0).Rows(intCnt).Item("POID") = Trim(pv_strPOID) Then
                    intSelectedIndex = intCnt + 1
                End If
            Next intCnt
        End If

        dr = objPODs.Tables(0).NewRow()
        dr("POId") = ""
        dr("DispPOId") = lblPleaseSelect.Text & "Purchase Order ID"
        objPODs.Tables(0).Rows.InsertAt(dr, 0)

        ddlPO.DataSource = objPODs.Tables(0)
        ddlPO.DataValueField = "POId"
        ddlPO.DataTextField = "DispPOId"
        ddlPO.DataBind()
        ddlPO.SelectedIndex = intSelectedIndex
        If ddlPO.SelectedIndex <> -1 Then
            strPOID = ddlPO.SelectedItem.Value
        End If
    End Sub

    Sub BindSupp(ByVal pv_strSelectedSuppCode As String)
        Dim strOpCd As String = "PU_CLSSETUP_SUPPLIER_DDL_GET"
        Dim liTemp As ListItem
        Dim objSuppDs As New Object
        Dim strParam As String = ""
        Dim intCnt As Integer = 0
        Dim intErrNo As Integer
        Dim dr As DataRow
        Dim intSelectedIndex As Integer = 0

        If rbFFB.Checked = True Then
            strParam = "||" & objPUSetup.EnumSuppStatus.Active & "||SupplierCode||"
            strParam = strParam & "|" & IIf(Session("SS_COACENTRALIZED") = "1", "", " A.AccCode in (SELECT AccCode FROM GL_Account WHERE LocCode='" & Trim(strLocation) & "') ")
            strParam = strParam & "|" & objPUSetup.EnumSupplierType.FFBSupplier
        Else
            strParam = "||" & objPUSetup.EnumSuppStatus.Active & "||SupplierCode||"
            strParam = strParam & "|" & IIf(Session("SS_COACENTRALIZED") = "1", "", " A.AccCode in (SELECT AccCode FROM GL_Account WHERE LocCode='" & Trim(strLocation) & "') ")
        End If

        Try
            intErrNo = objPUSetup.mtdGetSupplier(strOpCd, strParam, objSuppDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PU_GET_SUPPLIER&errmesg=" & lblErrMessage.Text & "&redirect=pu/trx/pu_trx_polist.aspx")
        End Try
        dr = objSuppDs.Tables(0).NewRow()
        dr("SupplierCode") = ""
        dr("Name") = "Please Select Supplier Code"
        objSuppDs.Tables(0).Rows.InsertAt(dr, 0)
        ddlSuppCode.DataSource = objSuppDs.Tables(0)
        ddlSuppCode.DataValueField = "SupplierCode"
        ddlSuppCode.DataTextField = "Name"
        ddlSuppCode.DataBind()
        If pv_strSelectedSuppCode <> "" Then
            liTemp = ddlSuppCode.Items.FindByValue(pv_strSelectedSuppCode)
            If liTemp Is Nothing Then
                ddlSuppCode.Items.Add(New ListItem(pv_strSelectedSuppCode & " (Deleted)", pv_strSelectedSuppCode))
                intSelectedIndex = ddlSuppCode.Items.Count - 1
            Else
                intSelectedIndex = ddlSuppCode.Items.IndexOf(liTemp)
            End If
        End If

        ddlSuppCode.SelectedIndex = intSelectedIndex
    End Sub

    Sub BindCreditTerm(ByVal pv_strSelectedSuppCode As String)
        'temporary being remark coz STA take credit term on PO

        'Dim strOpCd_GetCreditTerm As String = "PU_CLSSETUP_SUPPLIER_GET"
        'Dim strParam As String = Trim(pv_strSelectedSuppCode) & "||||SupplierCode||"
        'Dim intCnt As Integer = 0
        'Dim intErrNo As Integer
        'Dim intDefIndex As Integer = 0
        'Dim intSelectedIndex As Integer = 0
        'Dim crtFound As Boolean = False

        'If Trim(pv_strSelectedSuppCode) = "" Then
        '    Exit Sub
        'End If

        'Try
        '    intErrNo = objPUSetup.mtdGetSupplier(strOpCd_GetCreditTerm, strParam, objCreditTermDs)
        'Catch Exp As System.Exception
        '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PU_GET_SUPPLIER&errmesg=" & lblErrMessage.Text & "&redirect=pu/trx/pu_trx_polist.aspx")
        'End Try

        'If objCreditTermDs.Tables(0).Rows.Count > 0 Then
        '    txtCreditTerm.Text = Trim(objCreditTermDs.Tables(0).Rows(0).Item("CreditTerm"))
        'End If
    End Sub

    Sub BindTermType(ByVal pv_TermType As String)
        Dim strOpCd_GetTermType As String = "ADMIN_CLSSHARE_CREDITTERMTYPE_LIST_GET"
        Dim strParam As String = "DefaultInd=0"
        Dim intCnt As Integer = 0
        Dim intErrNo As Integer
        Dim intDefIndex As Integer = 0
        Dim intSelectedIndex As Integer = 0


        Try
            intErrNo = objAdminShare.mtdGetCreditTermType(strOpCd_GetTermType, strParam, objTermTypeDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_GET_TERMTYPELIST&errmesg=" & lblErrMessage.Text & "&redirect=pu/trx/pu_polist.aspx")
        End Try

        For intCnt = 0 To objTermTypeDs.Tables(0).Rows.Count - 1
            objTermTypeDs.Tables(0).Rows(intCnt).Item("CreditTermTypeCode") = Trim(objTermTypeDs.Tables(0).Rows(intCnt).Item("CreditTermTypeCode"))
            objTermTypeDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objTermTypeDs.Tables(0).Rows(intCnt).Item("Description"))

            If objTermTypeDs.Tables(0).Rows(intCnt).Item("CreditTermTypeCode") = pv_TermType Then
                intSelectedIndex = intCnt
            End If

            'If objTermTypeDs.Tables(0).Rows(intCnt).Item("DefaultInd") = "0" Then
            '    intDefIndex = intCnt
            'End If
        Next intCnt

        'If intSelectedIndex = 0 Then intSelectedIndex = intDefIndex

        ddlTermType.DataSource = objTermTypeDs.Tables(0)
        ddlTermType.DataValueField = "CreditTermTypeCode"
        ddlTermType.DataTextField = "Description"
        ddlTermType.DataBind()
        ddlTermType.SelectedIndex = intSelectedIndex
    End Sub

    Sub onSelect_Change(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim strOpCdGetAcc As String = "GL_CLSSETUP_ENTRYSETUP_TYPE_GET"
        Dim intErrNo As Integer
        Dim strParam As String = ""
        Dim objGLDs As New DataSet()
        Dim objTemp As New DataSet()
        Dim SelectedAcc As String = ""

        BindPO(ddlPO.SelectedItem.Value)
        BindTermType("")
        onLoad_Button()

        strParam = objGlobal.EnumModule.Purchasing & "|" & objGL.EnumEntryType.PUDRGoodsReceive & "|" & strLocation

        Try
            intErrNo = objGL.mtdGetEntrySetupDet(strOpCdGetAcc, _
                                                 strParam, _
                                                 objTemp)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GL_SETUP_ENTRYSETUP_SAVE&errmesg=" & Exp.ToString & "&redirect=")
        End Try

        If objTemp.Tables(0).Rows.Count > 0 Then
            strAccCode = objTemp.Tables(0).Rows(0).Item("CRAccCode")
            BindAccCode(strAccCode)
        Else
            BindAccCode("")
        End If


        Dim strInvoiceRcvId As String = strSelectedInvRcvId
        Dim strDueDate As String
        Dim strPODueDate As String
        Dim strGRDate As String
        Dim objLastGR As Object
        Dim strOpCd_LastGR As String = "AP_CLSTRX_INVOICERECEIVE_LASTGR_GET"
        Dim strParamName As String
        Dim strParamValue As String
        Dim strDate As String = Date_Validation(txtTransDate.Text, False)
        Dim intCreditTerm As Integer = CInt(txtCreditTerm.Text)

        If ddlPO.SelectedItem.Value = "" Then
            strDueDate = strDate
        Else
            If strInvoiceRcvId = "" Then
                strParamName = "LOCCODE|POID"
                strParamValue = strLocation & "|" & Replace(ddlPO.SelectedItem.Value, "-ADD", "")

                Try
                    intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_LastGR, _
                                                        strParamName, _
                                                        strParamValue, _
                                                        objLastGR)

                Catch Exp As System.Exception
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_CLSTRX_INVOICERECEIVE_LASTGR_GET&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
                End Try

                If objLastGR.Tables(0).Rows.Count > 0 Then
                    strGRDate = Date_Validation(objLastGR.Tables(0).Rows(0).Item("GoodsRcvRefDate"), True)
                    strGRDate = Date_Validation(strGRDate, False)
                End If

                If ddlTermType.SelectedItem.Value = "11" Then
                    strDueDate = DateAdd(DateInterval.Day, intCreditTerm, CDate(strDate))
                Else
                    strDueDate = DateAdd(DateInterval.Day, intCreditTerm, CDate(strGRDate))
                End If

                strPODueDate = strDueDate
                Select Case True
                    Case CDate(strDueDate) <= CDate(strDate)
                        strDueDate = DateAdd(DateInterval.Day, 7, CDate(strDate))
                    Case DateDiff(DateInterval.Day, CDate(strDate), CDate(strDueDate)) <= 7
                        strDueDate = DateAdd(DateInterval.Day, 7, CDate(strDueDate))
                End Select

                Select Case DatePart(DateInterval.Weekday, CDate(strDueDate))
                    Case vbMonday
                        strDueDate = strDueDate
                    Case vbTuesday
                        strDueDate = DateAdd(DateInterval.Day, 2, CDate(strDueDate))
                    Case vbWednesday
                        strDueDate = DateAdd(DateInterval.Day, 1, CDate(strDueDate))
                    Case vbThursday
                        strDueDate = strDueDate
                    Case vbFriday
                        strDueDate = DateAdd(DateInterval.Day, 3, CDate(strDueDate))
                    Case vbSaturday
                        strDueDate = DateAdd(DateInterval.Day, 2, CDate(strDueDate))
                    Case vbSunday
                        strDueDate = DateAdd(DateInterval.Day, 1, CDate(strDueDate))
                End Select
            Else
                If hidTermType.Value <> ddlTermType.SelectedItem.Value Then
                    strParamName = "LOCCODE|POID"
                    strParamValue = strLocation & "|" & ddlPO.SelectedItem.Value

                    Try
                        intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_LastGR, _
                                                            strParamName, _
                                                            strParamValue, _
                                                            objLastGR)

                    Catch Exp As System.Exception
                        Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_CLSTRX_INVOICERECEIVE_LASTGR_GET&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
                    End Try

                    If objLastGR.Tables(0).Rows.Count > 0 Then
                        strGRDate = Date_Validation(objLastGR.Tables(0).Rows(0).Item("GoodsRcvRefDate"), True)
                        strGRDate = Date_Validation(strGRDate, False)
                    End If

                    If ddlTermType.SelectedItem.Value = "11" Then
                        strDueDate = DateAdd(DateInterval.Day, intCreditTerm, CDate(strDate))
                    Else
                        strDueDate = DateAdd(DateInterval.Day, intCreditTerm, CDate(strGRDate))
                    End If

                    strPODueDate = strDueDate
                    Select Case True
                        Case CDate(strDueDate) <= CDate(strDate)
                            strDueDate = DateAdd(DateInterval.Day, 7, CDate(strDate))
                        Case DateDiff(DateInterval.Day, CDate(strDate), CDate(strDueDate)) <= 7
                            strDueDate = DateAdd(DateInterval.Day, 7, CDate(strDueDate))
                    End Select

                    Select Case DatePart(DateInterval.Weekday, CDate(strDueDate))
                        Case vbMonday
                            strDueDate = strDueDate
                        Case vbTuesday
                            strDueDate = DateAdd(DateInterval.Day, 2, CDate(strDueDate))
                        Case vbWednesday
                            strDueDate = DateAdd(DateInterval.Day, 1, CDate(strDueDate))
                        Case vbThursday
                            strDueDate = strDueDate
                        Case vbFriday
                            strDueDate = DateAdd(DateInterval.Day, 3, CDate(strDueDate))
                        Case vbSaturday
                            strDueDate = DateAdd(DateInterval.Day, 2, CDate(strDueDate))
                        Case vbSunday
                            strDueDate = DateAdd(DateInterval.Day, 1, CDate(strDueDate))
                    End Select
                Else
                    strDueDate = Date_Validation(txtInvDueDate.Text, False)
                    If CDate(strDueDate) < CDate(strDate) Then
                        lblerrInvDueDate.Visible = True
                        lblerrInvDueDate.Text = "Invalid invoice due date."
                        Exit Sub
                    End If

                    strParamName = "LOCCODE|POID"
                    strParamValue = strLocation & "|" & ddlPO.SelectedItem.Value

                    Try
                        intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_LastGR, _
                                                            strParamName, _
                                                            strParamValue, _
                                                            objLastGR)

                    Catch Exp As System.Exception
                        Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_CLSTRX_INVOICERECEIVE_LASTGR_GET&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
                    End Try

                    If objLastGR.Tables(0).Rows.Count > 0 Then
                        strGRDate = Date_Validation(objLastGR.Tables(0).Rows(0).Item("GoodsRcvRefDate"), True)
                        strGRDate = Date_Validation(strGRDate, False)
                    End If

                    If ddlTermType.SelectedItem.Value = "11" Then
                        strPODueDate = DateAdd(DateInterval.Day, intCreditTerm, CDate(strDate))
                    Else
                        strPODueDate = DateAdd(DateInterval.Day, intCreditTerm, CDate(strGRDate))
                    End If
                End If
            End If
        End If

        txtInvDueDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), strDueDate)
        BindTTRefNo("")
        If dgLineDet.Items.Count = 0 Then
            onLoad_DisplayAdvancePayment(ddlPO.SelectedItem.Value)
            onLoad_DisplayDNCNAmount(ddlPO.SelectedItem.Value)
        End If
    End Sub

    Sub onSelect_Supp(ByVal sender As System.Object, ByVal e As System.EventArgs)
        BindCreditTerm(ddlSuppCode.SelectedItem.Value)
    End Sub

    Function CheckDate(ByVal pv_strDate As String, ByRef pr_strDate As String) As Boolean
        Dim strDateFormatCode As String = Session("SS_DATEFMT")
        Dim strDateFormat As String

        pr_strDate = ""
        CheckDate = True
        If Not pv_strDate = "" Then
            If objGlobal.mtdValidInputDate(strDateFormatCode, pv_strDate, strDateFormat, pr_strDate) = False Then
                lblFmtTransDate.Text = strDateFormat
                lblFmtInvRcvRefDate.Text = strDateFormat
                lblFmtInvDueDate.Text = strDateFormat
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

        strParam = "PWSYSTEM_CLSCONFIG_CONFIG_DATEFMT_GET"
        Try
            intErrNo = objSysCfg.mtdGetConfigInfo(strParam, _
                                                  strCompany, _
                                                  strLocation, _
                                                  strUserId, _
                                                  objSysCfgDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_GET_CONFIG&errmesg=" & lblErrMessage.Text & "&redirect=pu/trx/pu_polist.aspx")
        End Try

        strDateFormat = objSysCfg.mtdGetDateFormat(Trim(objSysCfgDs.Tables(0).Rows(0).Item("Datefmt")))

        If pv_blnIsShortDate Then
            Date_Validation = objGlobal.GetShortDate(strDateFormat, pv_strInputDate)
        Else
            If objGlobal.mtdValidInputDate(strDateFormat, _
                                            pv_strInputDate, _
                                            strAcceptDateFormat, _
                                            objActualDate) = True Then
                Date_Validation = objActualDate
            Else
                Date_Validation = ""
            End If
        End If
    End Function

    Sub BindAccCode(ByVal pv_strAccCode As String)
        Dim strOpCode_GetAcc As String = "GL_CLSSETUP_ACCOUNTCODE_LIST_GET"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer = 0

        Try
            strParam = "Order By ACC.AccCode|And ACC.Status = '" & _
                        objGLSetup.EnumAccountCodeStatus.Active & _
                        "' And ACC.AccType in ('" & objGLSetup.EnumAccountType.BalanceSheet & "','" & objGLSetup.EnumAccountType.ProfitAndLost & "')"

            strParam = strParam & IIf(Session("SS_COACENTRALIZED") = "1", "", " AND LocCode = '" & Trim(strLocation) & "' ")

            intErrNo = objGLSetup.mtdGetMasterList(strOpCode_GetAcc, strParam, objGLSetup.EnumGLMasterType.AccountCode, objAccDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CREDITNOTE_GET_SUPP2&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_cnlist.aspx")
        End Try

        For intCnt = 0 To objAccDs.Tables(0).Rows.Count - 1
            objAccDs.Tables(0).Rows(intCnt).Item("AccCode") = Trim(objAccDs.Tables(0).Rows(intCnt).Item("AccCode"))
            objAccDs.Tables(0).Rows(intCnt).Item("Description") = objAccDs.Tables(0).Rows(intCnt).Item("AccCode") & " (" & Trim(objAccDs.Tables(0).Rows(intCnt).Item("Description")) & ")"
            If objAccDs.Tables(0).Rows(intCnt).Item("AccCode") = Trim(pv_strAccCode) Then
                intSelectedIndex = intCnt + 1
            End If
        Next intCnt


        Dim dr As DataRow
        dr = objAccDs.Tables(0).NewRow()
        dr("AccCode") = ""
        dr("Description") = lblPleaseSelect.Text & lblAccount.Text
        objAccDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlAccCode.DataSource = objAccDs.Tables(0)
        ddlAccCode.DataValueField = "AccCode"
        ddlAccCode.DataTextField = "Description"
        ddlAccCode.DataBind()
        ddlAccCode.SelectedIndex = intSelectedIndex
    End Sub

    Sub GetAccountDetails(ByVal pv_strAccCode As String, _
                      ByRef pr_IsBalanceSheet As Boolean, _
                      ByRef pr_IsNurseryInd As Boolean, _
                      ByRef pr_IsBlockRequire As Boolean, _
                      ByRef pr_IsVehicleRequire As Boolean, _
                      ByRef pr_IsOthers As Boolean)

        Dim _objAccDs As New Object
        Dim strOpCd As String = "GL_CLSSETUP_CHARTOFACCOUNT_GET_BY_ACCCODE"
        Dim strParam As String = pv_strAccCode
        Dim intErrNo As Integer

        Try
            pr_IsBalanceSheet = False
            pr_IsNurseryInd = False
            pr_IsBlockRequire = False
            pr_IsVehicleRequire = False
            pr_IsOthers = False
            intErrNo = objGLSetup.mtdGetAccount(strOpCd, _
                                                strParam, _
                                                _objAccDs, _
                                                True)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_TRX_CREDITJRNDET_GET_ACCOUNT_DETAILS&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        If _objAccDs.Tables(0).Rows.Count = 1 Then
            If Convert.ToInt16(_objAccDs.Tables(0).Rows(0).Item("AccType")) = objGLSetup.EnumAccountType.BalanceSheet Then
                pr_IsBalanceSheet = True
                If Convert.ToInt16(_objAccDs.Tables(0).Rows(0).Item("NurseryInd")) = objGLSetup.EnumNurseryAccount.Yes Then
                    pr_IsNurseryInd = True
                End If
            End If
            If Convert.ToInt16(_objAccDs.Tables(0).Rows(0).Item("AccPurpose")) = objGLSetup.EnumAccountPurpose.NonVehicle Then
                pr_IsBlockRequire = True
            ElseIf Convert.ToInt16(_objAccDs.Tables(0).Rows(0).Item("AccPurpose")) = objGLSetup.EnumAccountPurpose.VehicleDistribution Then
                pr_IsVehicleRequire = True
            ElseIf Convert.ToInt16(_objAccDs.Tables(0).Rows(0).Item("AccPurpose")) = objGLSetup.EnumAccountPurpose.Others Then
                pr_IsBlockRequire = True
                pr_IsOthers = True
            End If
        End If
    End Sub


    Sub onSelect_Account(ByVal Sender As Object, ByVal E As EventArgs)
        Dim blnIsBalanceSheet As Boolean
        Dim blnIsNurseryInd As Boolean
        Dim blnIsBlockRequire As Boolean
        Dim blnIsVehicleRequire As Boolean
        Dim blnIsOthers As Boolean
        Dim strVeh As String = Request.Form("ddlVehCode")
        Dim strVehExp As String = Request.Form("ddlVehExpCode")

        GetAccountDetails(ddlAccCode.SelectedItem.Value, blnIsBalanceSheet, blnIsNurseryInd, blnIsBlockRequire, blnIsVehicleRequire, blnIsOthers)

        If Not blnIsBalanceSheet Then
            If blnIsBlockRequire Then
                BindPreBlock(ddlAccCode.SelectedItem.Value, Request.Form("ddlPreBlock"))
                BindBlock(ddlAccCode.SelectedItem.Value, Request.Form("ddlBlock"))
                BindVehicle("", strVeh)
                BindVehicleExpense(True, strVehExp)
            Else
                BindPreBlock("", Request.Form("ddlPreBlock"))
                BindBlock("", Request.Form("ddlBlock"))
                BindVehicle("", strVeh)
                BindVehicleExpense(True, strVehExp)
            End If

            If blnIsVehicleRequire Then
                BindVehicle(ddlAccCode.SelectedItem.Value, strVeh)
                BindVehicleExpense(False, strVehExp)
            End If

            If blnIsOthers Then
                lblVehicleOption.Text = True
                BindVehicle("%", strVeh)
                BindVehicleExpense(False, strVehExp)
            Else
                lblVehicleOption.Text = False
            End If
        ElseIf blnIsNurseryInd = True Then
            BindPreBlock(ddlAccCode.SelectedItem.Value, Request.Form("ddlPreBlock"))
            BindBlock(ddlAccCode.SelectedItem.Value, Request.Form("ddlBlock"))

            BindVehicle("", strVeh)
            BindVehicleExpense(True, strVehExp)
        Else
            BindPreBlock("", Request.Form("ddlPreBlock"))
            BindBlock("", Request.Form("ddlBlock"))
            BindVehicle("", strVeh)
            BindVehicleExpense(True, strVehExp)
        End If
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
            strParam = pv_strAccCode & "|" & Session("SS_LOCATION") & "|" & objGLSetup.EnumBlockStatus.Active
            intErrNo = objGLSetup.mtdGetAccountBlock(strOpCd, _
                                                     strParam, _
                                                     objBlkDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GL_CLSSETUP_ACCOUNT_BLOCK_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To objBlkDs.Tables(0).Rows.Count - 1
            objBlkDs.Tables(0).Rows(intCnt).Item("BlkCode") = Trim(objBlkDs.Tables(0).Rows(intCnt).Item("BlkCode"))
            objBlkDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objBlkDs.Tables(0).Rows(intCnt).Item("BlkCode")) & " (" & Trim(objBlkDs.Tables(0).Rows(intCnt).Item("Description")) & ")"
            If objBlkDs.Tables(0).Rows(intCnt).Item("BlkCode") = Trim(pv_strBlkCode) Then
                intSelectedIndex = intCnt + 1
            End If
        Next

        If objBlkDs.Tables(0).Rows.Count = 1 Then
            intSelectedIndex = 1
        End If

        dr = objBlkDs.Tables(0).NewRow()
        dr("BlkCode") = ""
        dr("Description") = lblPleaseSelect.Text & PreBlockTag & lblCode.Text

        objBlkDs.Tables(0).Rows.InsertAt(dr, 0)
        ddlPreBlock.DataSource = objBlkDs.Tables(0)
        ddlPreBlock.DataValueField = "BlkCode"
        ddlPreBlock.DataTextField = "Description"
        ddlPreBlock.DataBind()
        ddlPreBlock.SelectedIndex = intSelectedIndex

        If Not objBlkDs Is Nothing Then
            objBlkDs = Nothing
        End If
    End Sub

    Sub BindBlock(ByVal pv_strAccCode As String, ByVal pv_strBlkCode As String)
        Dim strOpCd As String
        Dim dr As DataRow
        Dim objBlkDs As DataSet
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intMasterType As Integer
        Dim intSelectedIndex As Integer = 0

        Try
            If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfig) = True Then
                strOpCd = "GL_CLSSETUP_ACCOUNT_BLOCK_GET"
                strParam = pv_strAccCode & "|" & Session("SS_LOCATION") & "|" & objGLSetup.EnumBlockStatus.Active
            Else
                strOpCd = "GL_CLSSETUP_ACCOUNT_SUBBLOCK_GET"
                strParam = pv_strAccCode & "|" & Session("SS_LOCATION") & "|" & objGLSetup.EnumSubBlockStatus.Active
            End If
            intErrNo = objGLSetup.mtdGetAccountBlock(strOpCd, _
                                                     strParam, _
                                                     objBlkDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_TRX_CREDITJRNDET_BLOCK_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To objBlkDs.Tables(0).Rows.Count - 1
            objBlkDs.Tables(0).Rows(intCnt).Item("BlkCode") = Trim(objBlkDs.Tables(0).Rows(intCnt).Item("BlkCode"))
            objBlkDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objBlkDs.Tables(0).Rows(intCnt).Item("BlkCode")) & " (" & Trim(objBlkDs.Tables(0).Rows(intCnt).Item("Description")) & ")"
            If objBlkDs.Tables(0).Rows(intCnt).Item("BlkCode") = Trim(pv_strBlkCode) Then
                intSelectedIndex = intCnt + 1
            End If
        Next

        If objBlkDs.Tables(0).Rows.Count = 1 Then
            intSelectedIndex = 1
        End If

        dr = objBlkDs.Tables(0).NewRow()
        dr("BlkCode") = ""
        dr("Description") = lblPleaseSelect.Text & lblBlock.Text
        objBlkDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlBlock.DataSource = objBlkDs.Tables(0)
        ddlBlock.DataValueField = "BlkCode"
        ddlBlock.DataTextField = "Description"
        ddlBlock.DataBind()
        ddlBlock.SelectedIndex = intSelectedIndex
    End Sub

    Sub BindVehicle(ByVal pv_strAccCode As String, ByVal pv_strVehCode As String)
        Dim strOpCd As String
        Dim dr As DataRow
        Dim objVehDs As DataSet
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intMasterType As Integer
        Dim intSelectedIndex As Integer = 0

        Try
            strOpCd = "GL_CLSSETUP_VEH_LIST_GET"
            strParam = "|AccCode = '" & pv_strAccCode & "' AND LocCode = '" & Session("SS_LOCATION") & "' AND Status = '" & objGLSetup.EnumVehicleStatus.Active & "'"
            intErrNo = objGLSetup.mtdGetMasterList(strOpCd, _
                                                   strParam, _
                                                   objGLSetup.EnumGLMasterType.Vehicle, _
                                                   objVehDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_TRX_CREDITJRNDET_VEH_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To objVehDs.Tables(0).Rows.Count - 1
            objVehDs.Tables(0).Rows(intCnt).Item("VehCode") = Trim(objVehDs.Tables(0).Rows(intCnt).Item("VehCode"))
            objVehDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objVehDs.Tables(0).Rows(intCnt).Item("VehCode")) & " (" & Trim(objVehDs.Tables(0).Rows(intCnt).Item("Description")) & ")"
            If objVehDs.Tables(0).Rows(intCnt).Item("VehCode") = Trim(pv_strVehCode) Then
                intSelectedIndex = intCnt + 1
            End If
        Next

        dr = objVehDs.Tables(0).NewRow()
        dr("VehCode") = ""
        dr("Description") = lblPleaseSelect.Text & lblVehicle.Text
        objVehDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlVehCode.DataSource = objVehDs.Tables(0)
        ddlVehCode.DataValueField = "VehCode"
        ddlVehCode.DataTextField = "Description"
        ddlVehCode.DataBind()
        ddlVehCode.SelectedIndex = intSelectedIndex
    End Sub

    Sub BindVehicleExpense(ByVal pv_IsBlankList As Boolean, ByVal pv_strVehExpCode As String)
        Dim strOpCd As String = "GL_CLSSETUP_VEHEXPENSE_LIST_GET"
        Dim dr As DataRow
        Dim objVehExpDs As DataSet
        Dim strParam As String = "Order By VehExpenseCode ASC| And Veh.Status = '" & objGLSetup.EnumVehExpenseStatus.Active & "' "
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer = 0

        Try
            If pv_IsBlankList Then
                strParam += "And Veh.VehExpensecode = ''"
            End If
            intErrNo = objGLSetup.mtdGetMasterList(strOpCd, _
                                                   strParam, _
                                                   objGLSetup.EnumGLMasterType.VehicleExpense, _
                                                   objVehExpDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_TRX_CREDITJRNDET_VEHEXPENSE_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To objVehExpDs.Tables(0).Rows.Count - 1
            objVehExpDs.Tables(0).Rows(intCnt).Item("VehExpenseCode") = Trim(objVehExpDs.Tables(0).Rows(intCnt).Item("VehExpenseCode"))
            objVehExpDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objVehExpDs.Tables(0).Rows(intCnt).Item("VehExpenseCode")) & " (" & Trim(objVehExpDs.Tables(0).Rows(intCnt).Item("Description")) & ")"
            If objVehExpDs.Tables(0).Rows(intCnt).Item("VehExpenseCode") = Trim(pv_strVehExpCode) Then
                intSelectedIndex = intCnt + 1
            End If
        Next

        dr = objVehExpDs.Tables(0).NewRow()
        dr("VehExpenseCode") = ""
        dr("Description") = lblPleaseSelect.Text & lblVehExpense.Text
        objVehExpDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlVehExpCode.DataSource = objVehExpDs.Tables(0)
        ddlVehExpCode.DataValueField = "VehExpenseCode"
        ddlVehExpCode.DataTextField = "Description"
        ddlVehExpCode.DataBind()
        ddlVehExpCode.SelectedIndex = intSelectedIndex
    End Sub

    Sub Update_InvoiceRcv(ByVal intStatus As Integer)
        Dim objInvRcvId As New Object
        Dim strInvoiceRcvId As String = strSelectedInvRcvId
        Dim strInvRcvRefNo As String = IIf(txtInvoiceRcvRefNo.Text = "", "-", txtInvoiceRcvRefNo.Text)
        Dim strInvRcvRefDate As String = txtInvoiceRcvRefDate.Text
        Dim strPOID As String
        Dim strInvType As String
        Dim strSupplierCode As String = idSuppCode.Value
        Dim strCreditTerm As String = txtCreditTerm.Text
        Dim strTermType As String = ddlTermType.SelectedItem.Value
        Dim strRemark As String = Trim(Replace(txtRemark.Text, "'", "''"))
        Dim lblInvoiceLnID As Label
        Dim lblItemCode As Label
        Dim lblQtyRcv As Label
        Dim txtQtyInv As Label
        Dim txtUnitPrice As Label
        Dim dgLine As DataGridItem
        Dim dblAmount As Double
        Dim dblPPNRate As Double
        Dim dblPPNAmount As Double
        Dim dblPPHAmount As Double
        Dim dblNetAmount As Double
        Dim dblTotalAmount As Double = 0
        Dim strParamLn As String = ""
        Dim intCnt As Integer

        Dim strOpCd_AddInvoiceRcv As String = "AP_CLSTRX_INVOICERECEIVE_ADD"
        Dim strOpCd_UpdInvoiceRcv As String = "AP_CLSTRX_INVOICERECEIVE_UPD"
        Dim strOpCd_AddInvoiceRcvLn As String = "AP_CLSTRX_INVOICERECEIVELN_ADD"
        Dim strOpCd_UpdInvoiceRcvLn As String = "AP_CLSTRX_INVOICERECEIVELN_UPD"
        Dim strOpCodes As String = strOpCd_AddInvoiceRcv & "|" & _
                                   strOpCd_UpdInvoiceRcv & "|" & _
                                   strOpCd_AddInvoiceRcvLn & "|" & _
                                   strOpCd_UpdInvoiceRcvLn
        Dim intErrNo As Integer
        Dim strParam As String = ""

        Dim objChkRef As Object
        Dim intErrNoRef As Integer
        Dim strParamRef As String = ""
        Dim strParamID As String = ""
        Dim strOpCd_RefNo As String = "AP_CLSTRX_CHK_REF_NO"

        Dim lblLineAmt As Label
        Dim lblLinePPNAmt As Label
        Dim lblLinePPHAmt As Label
        Dim lblLineNetAmt As Label
        Dim strNewIDFormat As String
        Dim intCreditTerm As Integer = CInt(txtCreditTerm.Text)
        Dim strDueDate As String
        Dim strPODueDate As String
        Dim strGRDate As String
        Dim objLastGR As Object
        Dim strOpCd_LastGR As String = "AP_CLSTRX_INVOICERECEIVE_LASTGR_GET"
        Dim strParamName As String
        Dim strParamValue As String
        Dim objAPNote As Object
        Dim strOpCd_APNote As String
        Dim strAPNoteID As String
        Dim strDate As String = Date_Validation(txtTransDate.Text, False)
        Dim indDate As String = ""

        If CheckDate(txtTransDate.Text.Trim(), indDate) = False Then
            lblErrTransDate.Visible = True
            lblFmtTransDate.Visible = True
            lblErrTransDate.Text = "<br>Date Entered should be in the format"
            Exit Sub
        End If

        Dim intInputPeriod As Integer = Year(strDate) * 100 + Month(strDate)
        Dim intCurPeriod As Integer = (CInt(strAccYear) * 100) + CInt(strAccMonth)
        Dim intSelPeriod As Integer = (CInt(strSelAccYear) * 100) + CInt(strSelAccMonth)

        If Session("SS_FILTERPERIOD") = "0" Then
            If intCurPeriod < intInputPeriod Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "Invalid transaction date."
                Exit Sub
            End If
        Else
            If intSelPeriod <> intInputPeriod Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "Invalid transaction date."
                Exit Sub
            End If
            If intSelPeriod < intCurPeriod And intLevel < 2 Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "This period already locked."
                Exit Sub
            End If
        End If

        If CheckDate(txtInvoiceRcvRefDate.Text.Trim(), indDate) = False Then
            lblErrInvRcvRefDate.Visible = True
            lblFmtInvRcvRefDate.Visible = True
            lblErrInvRcvRefDate.Text = "<br>Date Entered should be in the format"
            Exit Sub
        End If

        If strInvoiceRcvId <> "" Then
            Dim arrParam As Array
            arrParam = Split(lblAccPeriod.Text, "/")
            If Month(strDate) <> arrParam(0) Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "Invalid transaction date."
                Exit Sub
            ElseIf Year(strDate) <> arrParam(1) Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "Invalid transaction date."
                Exit Sub
            End If
        End If

        blnIsUpdated = False

        If Trim(strInvRcvRefNo) = "" Then
            lblErrInvoiceRcvRefNo.Visible = True
            Exit Sub
        Else
            lblErrInvoiceRcvRefNo.Visible = False
        End If

        If strInvRcvRefDate <> "" Then
            strInvRcvRefDate = Date_Validation(strInvRcvRefDate, False)
            If strInvRcvRefDate = "" Then
                lblErrInvRcvRefDate.Visible = True
                lblErrInvRcvRefDate.Text = lblErrInvRcvRefDate.Text & strAcceptDateFormat
                Exit Sub
            End If
        End If

        If rbSPO.Checked = True Then
            strInvType = objAPTrx.EnumInvoiceType.SupplierPO
            If ddlPO.SelectedIndex = 0 Then
                lblErrPO.Visible = True
                Exit Sub
            End If
            strPOID = ddlPO.SelectedItem.Value
        ElseIf rbCWO.Checked = True Then
            strInvType = objAPTrx.EnumInvoiceType.ContractorWorkOrder
            If ddlSuppCode.SelectedIndex = 0 Then
                lblErrSuppCode.Visible = True
                Exit Sub
            Else
                lblErrSuppCode.Visible = False
            End If
            strPOID = ""
            strSupplierCode = ddlSuppCode.SelectedItem.Value
        ElseIf rbOTE.Checked = True Then
            strInvType = objAPTrx.EnumInvoiceType.Others
            If ddlSuppCode.SelectedIndex = 0 Then
                lblErrSuppCode.Visible = True
                Exit Sub
            Else
                lblErrSuppCode.Visible = False
            End If
            strPOID = ""
            strSupplierCode = ddlSuppCode.SelectedItem.Value
        ElseIf rbFFB.Checked = True Then
            strInvType = objAPTrx.EnumInvoiceType.FFBSupplier
            If ddlSuppCode.SelectedIndex = 0 Then
                lblErrSuppCode.Visible = True
                Exit Sub
            Else
                lblErrSuppCode.Visible = False
            End If
            strPOID = ""
            strSupplierCode = ddlSuppCode.SelectedItem.Value
        ElseIf rbTransportFee.Checked = True Then
            strInvType = objAPTrx.EnumInvoiceType.TransportFee
            If ddlPO.SelectedIndex = 0 Then
                lblErrPO.Visible = True
                Exit Sub
            End If
            strPOID = ddlPO.SelectedItem.Value
        End If

        For intCnt = 0 To dgLineDet.Items.Count - 1
            dgLine = dgLineDet.Items(intCnt)
            lblInvoiceLnID = dgLine.FindControl("lblInvoiceLnID")
            lblItemCode = dgLine.FindControl("lblItemCode")
            lblQtyRcv = dgLine.FindControl("lblQtyReceived")
            txtQtyInv = dgLine.FindControl("txtQtyInvoiced")
            txtUnitPrice = dgLine.FindControl("txtUnitPrice")

            If lblQtyRcv.Text <> "" Then
                If CDbl(txtQtyInv.Text) > CDbl(lblQtyRcv.Text) Then
                    lblErrQty.Visible = True
                    Exit Sub
                End If
            End If
            lblLineAmt = dgLine.FindControl("lblAmount")
            dblAmount = CDbl(lblLineAmt.Text)

            lblLinePPNAmt = dgLine.FindControl("lblPPNAmount")
            dblPPNAmount = CDbl(lblLinePPNAmt.Text)
            lblLinePPHAmt = dgLine.FindControl("lblPPHAmount")
            dblPPHAmount = CDbl(lblLinePPHAmt.Text)
            lblLineNetAmt = dgLine.FindControl("lblNetAmount")
            dblNetAmount = CDbl(lblLineNetAmt.Text)
            dblTotalAmount += dblAmount
        Next

        If strPOID = "" Then
            strDueDate = strDate
        Else
            If strInvoiceRcvId = "" Then
                strParamName = "LOCCODE|POID"
                strParamValue = strLocation & "|" & Replace(ddlPO.SelectedItem.Value, "-ADD", "")

                Try
                    intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_LastGR, _
                                                        strParamName, _
                                                        strParamValue, _
                                                        objLastGR)

                Catch Exp As System.Exception
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_CLSTRX_INVOICERECEIVE_LASTGR_GET&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
                End Try

                If objLastGR.Tables(0).Rows.Count > 0 Then
                    strGRDate = Date_Validation(objLastGR.Tables(0).Rows(0).Item("GoodsRcvRefDate"), True)
                    strGRDate = Date_Validation(strGRDate, False)
                End If

                If ddlTermType.SelectedItem.Value = "11" Then
                    strDueDate = DateAdd(DateInterval.Day, intCreditTerm, CDate(strDate))
                Else
                    strDueDate = DateAdd(DateInterval.Day, intCreditTerm, CDate(strGRDate))
                End If

                strPODueDate = strDueDate
                Select Case True
                    Case CDate(strDueDate) <= CDate(strDate)
                        strDueDate = DateAdd(DateInterval.Day, 7, CDate(strDate))
                    Case DateDiff(DateInterval.Day, CDate(strDate), CDate(strDueDate)) <= 7
                        strDueDate = DateAdd(DateInterval.Day, 7, CDate(strDueDate))
                End Select

                Select Case DatePart(DateInterval.Weekday, CDate(strDueDate))
                    Case vbMonday
                        strDueDate = strDueDate
                    Case vbTuesday
                        strDueDate = DateAdd(DateInterval.Day, 2, CDate(strDueDate))
                    Case vbWednesday
                        strDueDate = DateAdd(DateInterval.Day, 1, CDate(strDueDate))
                    Case vbThursday
                        strDueDate = strDueDate
                    Case vbFriday
                        strDueDate = DateAdd(DateInterval.Day, 3, CDate(strDueDate))
                    Case vbSaturday
                        strDueDate = DateAdd(DateInterval.Day, 2, CDate(strDueDate))
                    Case vbSunday
                        strDueDate = DateAdd(DateInterval.Day, 1, CDate(strDueDate))
                End Select
            Else
                If hidTermType.Value <> ddlTermType.SelectedItem.Value Then
                    strParamName = "LOCCODE|POID"
                    strParamValue = strLocation & "|" & Replace(ddlPO.SelectedItem.Value, "-ADD", "")

                    Try
                        intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_LastGR, _
                                                            strParamName, _
                                                            strParamValue, _
                                                            objLastGR)

                    Catch Exp As System.Exception
                        Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_CLSTRX_INVOICERECEIVE_LASTGR_GET&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
                    End Try

                    If objLastGR.Tables(0).Rows.Count > 0 Then
                        strGRDate = Date_Validation(objLastGR.Tables(0).Rows(0).Item("GoodsRcvRefDate"), True)
                        strGRDate = Date_Validation(strGRDate, False)
                    End If

                    If ddlTermType.SelectedItem.Value = "11" Then
                        strDueDate = DateAdd(DateInterval.Day, intCreditTerm, CDate(strDate))
                    Else
                        strDueDate = DateAdd(DateInterval.Day, intCreditTerm, CDate(strGRDate))
                    End If

                    strPODueDate = strDueDate
                    Select Case True
                        Case CDate(strDueDate) <= CDate(strDate)
                            strDueDate = DateAdd(DateInterval.Day, 7, CDate(strDate))
                        Case DateDiff(DateInterval.Day, CDate(strDate), CDate(strDueDate)) <= 7
                            strDueDate = DateAdd(DateInterval.Day, 7, CDate(strDueDate))
                    End Select

                    Select Case DatePart(DateInterval.Weekday, CDate(strDueDate))
                        Case vbMonday
                            strDueDate = strDueDate
                        Case vbTuesday
                            strDueDate = DateAdd(DateInterval.Day, 2, CDate(strDueDate))
                        Case vbWednesday
                            strDueDate = DateAdd(DateInterval.Day, 1, CDate(strDueDate))
                        Case vbThursday
                            strDueDate = strDueDate
                        Case vbFriday
                            strDueDate = DateAdd(DateInterval.Day, 3, CDate(strDueDate))
                        Case vbSaturday
                            strDueDate = DateAdd(DateInterval.Day, 2, CDate(strDueDate))
                        Case vbSunday
                            strDueDate = DateAdd(DateInterval.Day, 1, CDate(strDueDate))
                    End Select
                Else
                    strDueDate = Date_Validation(txtInvDueDate.Text, False)
                    If CDate(strDueDate) < CDate(strDate) Then
                        lblerrInvDueDate.Visible = True
                        lblerrInvDueDate.Text = "Invalid invoice due date."
                        Exit Sub
                    End If

                    strParamName = "LOCCODE|POID"
                    strParamValue = strLocation & "|" & ddlPO.SelectedItem.Value

                    Try
                        intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_LastGR, _
                                                            strParamName, _
                                                            strParamValue, _
                                                            objLastGR)

                    Catch Exp As System.Exception
                        Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_CLSTRX_INVOICERECEIVE_LASTGR_GET&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
                    End Try

                    If objLastGR.Tables(0).Rows.Count > 0 Then
                        strGRDate = Date_Validation(objLastGR.Tables(0).Rows(0).Item("GoodsRcvRefDate"), True)
                        strGRDate = Date_Validation(strGRDate, False)
                    End If

                    If ddlTermType.SelectedItem.Value = "11" Then
                        strPODueDate = DateAdd(DateInterval.Day, intCreditTerm, CDate(strDate))
                    Else
                        strPODueDate = DateAdd(DateInterval.Day, intCreditTerm, CDate(strGRDate))
                    End If
                End If
            End If
        End If

        'If Month(strDate) < strAccMonth And Year(strDate) <= strAccYear Then
        '    lblErrTransDate.Visible = True
        '    lblErrTransDate.Text = "Invalid transaction date."
        '    Exit Sub
        'End If

        strAccYear = Year(strDate)
        strAccMonth = Month(strDate)
        'Select Case strCompany
        '    Case "SAM", "MIL"
        '        strNewIDFormat = objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.InvoiceReceive) & Right(strAccYear, 2) & IIf(Len(Trim(strAccMonth)) = 1, "0" & strAccMonth, strAccMonth)
        '    Case Else
        '        strNewIDFormat = "INV" & "/" & strCompany & "/" & strLocation & "/" & "O" & "/" & strAccYear & IIf(Len(Trim(strAccMonth)) = 1, "0" & strAccMonth, strAccMonth) & "/"
        'End Select
        strNewIDFormat = "INV" & "/" & strCompany & "/" & strLocation & "/" & "O" & "/" & strAccYear & IIf(Len(Trim(strAccMonth)) = 1, "0" & strAccMonth, strAccMonth) & "/"

        If strParamLn <> "" Then
            strParamLn = Mid(strParamLn, 1, Len(strParamLn) - 1)
        End If

        strParam = strParam & objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.InvoiceReceiveLn) & "|" & _
                                objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.InvoiceReceive) & "|" & _
                                strInvoiceRcvId & "|" & _
                                Replace(strInvRcvRefNo, "'", "''") & "|" & _
                                strInvRcvRefDate & "|" & _
                                strPOID & "|" & _
                                strSupplierCode & "|" & _
                                strCreditTerm & "|" & _
                                strTermType & "|" & _
                                dblTotalAmount & "|" & _
                                strRemark & "|" & _
                                intStatus & "|" & _
                                strInvType & "|" & _
                                strNewIDFormat & "|" & _
                                strDueDate & "|" & _
                                IIf(strPODueDate = "", "", strPODueDate) & "|" & _
                                IIf(txtSplInvAmt.Text = "", 0, txtSplInvAmt.Text) & "|" & _
                                strDate
        'hidPaymentID.Value & "|" & _
        'hidUsedAdvAmount.Value * hidAdvExchangeRate.Value

        Try
            intErrNo = objAPTrx.mtdUpdInvoiceRcv(strOpCodes, _
                                                strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strAccMonth, _
                                                strAccYear, _
                                                strParam, _
                                                strParamLn, _
                                                objInvRcvId)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_UPD_INVOICERCV&errmesg=" & lblErrMessage.Text & "&redirect=pu/trx/pu_polist.aspx")
        End Try

        strInvoiceRcvId = IIf(strInvoiceRcvId = "", objInvRcvId, strInvoiceRcvId)

        'If strPOID <> "" Then
        '    If ddlTTRefNo.SelectedItem.Value = "" Then
        '        strOpCd_APNote = "AP_CLSTRX_INVOICERCV_NOTE_GET_LASTNO"
        '        strParamName = "STRSEARCH"
        '        strParamValue = "AccMonth = '" & strAccMonth & "' And AccYear = '" & strAccYear & "' "

        '        Try
        '            intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_APNote, _
        '                                                strParamName, _
        '                                                strParamValue, _
        '                                                objAPNote)

        '        Catch Exp As System.Exception
        '            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_CLSTRX_INVOICERCV_NOTE_GET_LASTNO&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
        '        End Try

        '        If objAPNote.Tables(0).Rows.Count > 0 Then
        '            strAPNoteID = Format(objAPNote.Tables(0).Rows(0).Item("NewTrxID"), "0000")

        '            strOpCd_APNote = "AP_CLSTRX_INVOICERCV_NOTE_GET_LASTNO"
        '            strParamName = "STRSEARCH"
        '            strParamValue = "AccMonth = '" & strAccMonth & "' And AccYear = '" & strAccYear & "' And InvoiceRcvRefNo = '" & Trim(strInvRcvRefNo) & "'"

        '            Try
        '                intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_APNote, _
        '                                                    strParamName, _
        '                                                    strParamValue, _
        '                                                    objAPNote)

        '            Catch Exp As System.Exception
        '                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_CLSTRX_INVOICERCV_NOTE_GET_LASTNO&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
        '            End Try

        '            If objAPNote.Tables(0).Rows.Count > 0 Then
        '                strAPNoteID = objAPNote.Tables(0).Rows(0).Item("TrxID")
        '            Else
        '                strAPNoteID = strAPNoteID & "/" & strCompany & "/" & strLocation & "/" & IIf(Len(Trim(strAccMonth)) = 1, "0" & strAccMonth, strAccMonth) & "/" & strAccYear
        '            End If
        '        Else
        '            strAPNoteID = "0001" & "/" & strCompany & "/" & strLocation & "/" & IIf(Len(Trim(strAccMonth)) = 1, "0" & strAccMonth, strAccMonth) & "/" & strAccYear
        '        End If

        '        strOpCd_APNote = "AP_CLSTRX_INVOICERCV_NOTE_ADD"
        '        strParamName = "TRXID|SUPPLIERCODE|LOCCODE|ACCMONTH|ACCYEAR|POID|INVOICERCVREFNO|INVOICERCVID|PAYMENTID"
        '        strParamValue = strAPNoteID & "|" & strSupplierCode & "|" & strLocation & "|" & strAccMonth & "|" & strAccYear & "|" & _
        '                        strPOID & "|" & strInvRcvRefNo & "|" & strInvoiceRcvId & "|"

        '        Try
        '            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd_APNote, _
        '                                                    strParamName, _
        '                                                    strParamValue)

        '        Catch Exp As System.Exception
        '            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=UPD_ITEM&errmesg=" & Exp.ToString() & "&redirect=pu/trx/AP_trx_InvoiceRcvList")
        '        End Try
        '    Else
        '        strAPNoteID = ddlTTRefNo.SelectedItem.Value
        '        strOpCd_APNote = "AP_CLSTRX_INVOICERCV_NOTE_ADD"
        '        strParamName = "TRXID|SUPPLIERCODE|LOCCODE|ACCMONTH|ACCYEAR|POID|INVOICERCVREFNO|INVOICERCVID|PAYMENTID"
        '        strParamValue = strAPNoteID & "|" & strSupplierCode & "|" & strLocation & "|" & strAccMonth & "|" & strAccYear & "|" & _
        '                        strPOID & "|" & strInvRcvRefNo & "|" & strInvoiceRcvId & "|"

        '        Try
        '            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd_APNote, _
        '                                                    strParamName, _
        '                                                    strParamValue)

        '        Catch Exp As System.Exception
        '            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=UPD_ITEM&errmesg=" & Exp.ToString() & "&redirect=pu/trx/AP_trx_InvoiceRcvList")
        '        End Try
        '    End If
        'End If



        inrid.Value = strInvoiceRcvId
        lblInvoiceRcvID.Text = strInvoiceRcvId
        If rbSPO.Checked = True Then
            'BindPO(ddlPO.SelectedItem.Value)
            txtCreditTerm.Text = strCreditTerm
        End If
        BindTermType(ddlTermType.SelectedItem.Value)
        onLoad_Button()
        blnIsUpdated = True
        strSelectedInvRcvId = IIf(strSelectedInvRcvId = "", strInvoiceRcvId, strSelectedInvRcvId)
    End Sub

    Sub Update_POLine(ByVal pv_intInvRcvStatus As String, Optional ByRef pr_intError As Integer = 0)

        Dim strOpCd_POLn_UPD As String = "PU_CLSTRX_PO_LINE_UPD"
        Dim strOpCd_GetIRPOLine As String = "AP_CLSTRX_INVOICERECEIVE_POLINE_GET"
        Dim strOpCd_GetIRLine As String = "AP_CLSTRX_INVOICERECEIVE_LINE_GET"
        Dim strOpCodes As String = strOpCd_GetIRPOLine & "|" & strOpCd_GetIRLine
        Dim strOpCd_GetPOLine As String = "PU_CLSTRX_PO_LINE_GET"
        Dim strParam As String
        Dim intErrNo As Integer
        If rbCWO.Checked = True Or rbOTE.Checked = True Or rbFFB.Checked = True Then
            Exit Sub
        End If
        Try
            strParam = lblInvoiceRcvID.Text.Trim & "|" & ddlPO.SelectedItem.Value.Trim

            intErrNo = objAPTrx.mtdAdjustInvoiceReceivePOLine(strOpCd_POLn_UPD, _
                                                                strOpCodes, _
                                                                strOpCd_GetPOLine, _
                                                                strCompany, _
                                                                strLocation, _
                                                                strUserId, _
                                                                strAccMonth, _
                                                                strAccYear, _
                                                                pv_intInvRcvStatus, _
                                                                strParam)
            pr_intError = intErrNo
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=UPDATE_POLINE&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
        End Try

    End Sub

    Sub Update_POTable(ByVal pv_intInvRcvStatus As String)
        Dim objPOId As New Object
        Dim strOpCd_AddPO As String = "PU_CLSTRX_PO_ADD"
        Dim strOpCd_UpdPo As String = "PU_CLSTRX_PO_UPD"
        Dim strOppCd As String = "PU_CLSTRX_PO_MOVEID"
        Dim strOppCd_Back As String = "PU_CLSTRX_PO_BACKID"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim strPOStatus As String
        Dim intItmCnt As Integer
        Dim arrParam As Array
        Dim intPOItmCnt As Double = 0
        If rbCWO.Checked = True Or rbOTE.Checked = True Or rbFFB.Checked = True Then
            Exit Sub
        End If
        For intItmCnt = 1 To lstItem.Items.Count - 1
            arrParam = lstItem.Items(intItmCnt).Value.Split("|")
            intPOItmCnt = intPOItmCnt + CDbl(arrParam(2).trim)
        Next

        Select Case pv_intInvRcvStatus
            Case objAPTrx.EnumInvoiceRcvStatus.Confirmed
                If intPOItmCnt = 0 Then
                    strPOStatus = objPUTrx.EnumPOStatus.Invoiced
                    'Try
                    '    strParam = ddlPO.SelectedItem.Value & "|0|||" & strPOStatus & "||" & strAccMonth & "|" & strAccYear & "|||||||||" & hidCurrencyCode.Value & "|" & hidExchangeRate.Value & "||||"
                    '    intErrNo = objPUTrx.mtdUpdPO(strOpCd_AddPO, _
                    '                                 strOpCd_UpdPo, _
                    '                                 strOppCd, _
                    '                                 strOppCd_Back, _
                    '                                 strCompany, _
                    '                                 strLocation, _
                    '                                 strUserId, _
                    '                                 strParam, _
                    '                                 objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.PurchaseOrder), _
                    '                                 objPOId)
                    'Catch Exp As System.Exception
                    '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_UPD_POSTATUS&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
                    'End Try

                    Dim strParamName As String = ""
                    Dim strParamValue As String = ""

                    strParamName = "STRUPDATE"
                    strParamValue = "Set Status = '" & strPOStatus & "' Where POID = '" & Trim(ddlPO.SelectedItem.Value) & "' "

                    Try
                        intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd_UpdPo, _
                                                                strParamName, _
                                                                strParamValue)

                    Catch Exp As System.Exception
                        Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_UPD_POSTATUS&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
                    End Try
                End If
            Case objAPTrx.EnumInvoiceRcvStatus.Cancelled, objAPTrx.EnumInvoiceRcvStatus.Deleted, objAPTrx.EnumInvoiceRcvStatus.Active
                If intPOItmCnt <> 0 Then
                    strPOStatus = objPUTrx.EnumPOStatus.Confirmed
                    'Try
                    '    strParam = ddlPO.SelectedItem.Value & "||||" & strPOStatus & "||" & strAccMonth & "|" & strAccYear & "|||||||||" & hidCurrencyCode.Value & "|" & hidExchangeRate.Value & "||||"
                    '    intErrNo = objPUTrx.mtdUpdPO(strOpCd_AddPO, _
                    '                                 strOpCd_UpdPo, _
                    '                                 strOppCd, _
                    '                                 strOppCd_Back, _
                    '                                 strCompany, _
                    '                                 strLocation, _
                    '                                 strUserId, _
                    '                                 strParam, _
                    '                                 objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.PurchaseOrder), _
                    '                                 objPOId)
                    'Catch Exp As System.Exception
                    '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_UPD_POSTATUS&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
                    'End Try
                    Dim strParamName As String = ""
                    Dim strParamValue As String = ""

                    strParamName = "STRUPDATE"
                    strParamValue = "Set Status = '" & strPOStatus & "' Where POID = '" & Trim(ddlPO.SelectedItem.Value) & "' "

                    Try
                        intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd_UpdPo, _
                                                                strParamName, _
                                                                strParamValue)


                    Catch Exp As System.Exception
                        Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_UPD_POSTATUS&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
                    End Try
                End If
        End Select


    End Sub

    Sub SaveBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        If strSelectedInvRcvId = "" Or lblInvoiceRcvID.Text.Trim = "" Then
            Exit Sub
        End If

        Update_InvoiceRcv(objAPTrx.EnumInvoiceRcvStatus.Active)
        Update_DNCNAmount(objAPTrx.EnumInvoiceRcvStatus.Active)
        If blnIsUpdated Then
            RefreshBtn_Click(Sender, E)
        End If
    End Sub

    Sub RefreshBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        onLoad_Display(strSelectedInvRcvId)
        onLoad_DisplayItem(strSelectedInvRcvId)
        onLoad_Button()
    End Sub

    Sub ConfirmBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Update_InvoiceRcv(objAPTrx.EnumInvoiceRcvStatus.Confirmed)
        Update_AdvPayment(objAPTrx.EnumInvoiceRcvStatus.Confirmed)
        Update_DNCNAmount(objAPTrx.EnumInvoiceRcvStatus.Confirmed)
        Update_CJAllocation(objAPTrx.EnumInvoiceRcvStatus.Confirmed)
        If blnIsUpdated Then
            Update_POTable(objAPTrx.EnumInvoiceRcvStatus.Confirmed)
            RefreshBtn_Click(Sender, E)
        End If
    End Sub

    Sub CancelBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Update_InvoiceRcv(objAPTrx.EnumInvoiceRcvStatus.Cancelled)
        Update_AdvPayment(objAPTrx.EnumInvoiceRcvStatus.Cancelled)
        Update_DNCNAmount(objAPTrx.EnumInvoiceRcvStatus.Cancelled)
        Update_CJAllocation(objAPTrx.EnumInvoiceRcvStatus.Cancelled)
        If blnIsUpdated Then
            Update_POLine(objAPTrx.EnumInvoiceRcvStatus.Cancelled)
            Update_POTable(objAPTrx.EnumInvoiceRcvStatus.Cancelled)

            Dim strOpCd_APNote As String
            Dim strParamName As String
            Dim strParamValue As String
            Dim interrno As Integer

            strOpCd_APNote = "AP_CLSTRX_INVOICERCV_NOTE_DELETE"
            strParamName = "STRSEARCH"
            strParamValue = "InvoiceRcvID = '" & Trim(lblInvoiceRcvID.Text) & "' "

            Try
                interrno = objGLtrx.mtdInsertDataCommon(strOpCd_APNote, _
                                                        strParamName, _
                                                        strParamValue)

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=UPD_ITEM&errmesg=" & Exp.ToString() & "&redirect=pu/trx/AP_trx_InvoiceRcvList")
            End Try

            RefreshBtn_Click(Sender, E)
        End If
    End Sub

    Sub DeleteBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Update_InvoiceRcv(objAPTrx.EnumInvoiceRcvStatus.Deleted)
        If blnIsUpdated Then
            Update_POLine(objAPTrx.EnumInvoiceRcvStatus.Deleted)
            Update_POTable(objAPTrx.EnumInvoiceRcvStatus.Deleted)
            RefreshBtn_Click(Sender, E)
        End If
    End Sub

    Sub UnDeleteBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim intErr As Integer
        Dim strDate As String = Date_Validation(txtTransDate.Text, False)
        Dim indDate As String = ""

        If CheckDate(txtTransDate.Text.Trim(), indDate) = False Then
            lblErrTransDate.Visible = True
            lblFmtTransDate.Visible = True
            lblErrTransDate.Text = "<br>Date Entered should be in the format"
            Exit Sub
        End If

        Dim intInputPeriod As Integer = Year(strDate) * 10 + Month(strDate)
        Dim intCurPeriod As Integer = (CInt(strAccYear) * 10) + CInt(strAccMonth)
        Dim intSelPeriod As Integer = (CInt(strSelAccYear) * 10) + CInt(strSelAccMonth)

        If Session("SS_FILTERPERIOD") = "0" Then
            If intCurPeriod < intInputPeriod Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "Invalid transaction date."
                Exit Sub
            End If
        Else
            If intSelPeriod <> intInputPeriod Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "Invalid transaction date."
                Exit Sub
            End If
            If intSelPeriod < intCurPeriod And intLevel < 2 Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "This period already locked."
                Exit Sub
            End If
        End If

        Update_POLine(objAPTrx.EnumInvoiceRcvStatus.Active, intErr)
        If intErr = -5 Then
            lblErrUnDel.Visible = True
        Else
            Update_POTable(objAPTrx.EnumInvoiceRcvStatus.Active)
            Update_InvoiceRcv(objAPTrx.EnumInvoiceRcvStatus.Active)
        End If
        RefreshBtn_Click(Sender, E)
    End Sub

    Sub AddBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCodeGLSubBlkByBlk As String = "GL_CLSSETUP_SUBBLOCK_BY_BLOCK_GET"
        Dim strParamList As String
        Dim strCreditTerm As String = txtCreditTerm.Text
        Dim blnIsValidDate As Boolean
        Dim strAccCode As String = Request.Form("ddlAccCode").Trim
        Dim strBlkCode As String
        If ddlChargeLevel.SelectedIndex = 1 Then
            strBlkCode = Request.Form("ddlBlock").Trim
        Else
            strBlkCode = Request.Form("ddlPreBlock").Trim
        End If
        Dim strVehCode As String = Request.Form("ddlVehCode").Trim
        Dim strVehExpenseCode As String = Request.Form("ddlVehExpCode").Trim

        Dim dblAmount As Double
        Dim intPPHRate As Double
        Dim dblPPNAmount As Double
        Dim dblPPHAmount As Double
        Dim dblNetAmount As Double
        Dim arrParam As Array
        Dim strParamID As String
        If rbSPO.Checked = True Or rbFFB.Checked = True Then 'Or rbOTE.Checked = True 
            arrParam = lstItem.SelectedItem.Value.Split("|")
            strParamID = arrParam(0) & "#" & arrParam(3)
        Else
            arrParam = Split("||0", "|")
            strParamID = arrParam(0)
        End If

        Dim strOpCd_AddInvoiceRcvLn As String = "AP_CLSTRX_INVOICERECEIVELN_ADD"
        Dim strOpCd_POLn_UPD As String = "PU_CLSTRX_PO_LINE_UPD"
        Dim intErrNo As Integer
        Dim objID As Object

        Dim blnIsBalanceSheet As Boolean
        Dim blnIsNurseryInd As Boolean
        Dim blnIsBlockRequire As Boolean
        Dim blnIsVehicleRequire As Boolean
        Dim blnIsOthers As Boolean
        Dim dblPBBKBAmount As Double
        Dim strDate As String = Date_Validation(txtTransDate.Text, False)
        Dim indDate As String = ""

        If CheckDate(txtTransDate.Text.Trim(), indDate) = False Then
            lblErrTransDate.Visible = True
            lblFmtTransDate.Visible = True
            lblErrTransDate.Text = "<br>Date Entered should be in the format"
            Exit Sub
        End If

        Dim intInputPeriod As Integer = Year(strDate) * 100 + Month(strDate)
        Dim intCurPeriod As Integer = (CInt(strAccYear) * 100) + CInt(strAccMonth)
        Dim intSelPeriod As Integer = (CInt(strSelAccYear) * 100) + CInt(strSelAccMonth)

        If Session("SS_FILTERPERIOD") = "0" Then
            If intCurPeriod < intInputPeriod Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "Invalid transaction date."
                Exit Sub
            End If
        Else
            If intSelPeriod <> intInputPeriod Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "Invalid transaction date."
                Exit Sub
            End If
            If intSelPeriod < intCurPeriod And intLevel < 2 Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "This period already locked."
                Exit Sub
            End If
        End If

        If txtCost.Text <= 0 Then
            errReqCost.Visible = True
            Exit Sub
        ElseIf CDbl(txtCost.Text) <> CDbl(hidUnitCost.Value) Then
            errUnmatchCost.Visible = True
            Exit Sub
        Else
            If txtQty.Text <= 0 Then
                errReqQty.Visible = True
                Exit Sub
            End If
        End If

        Update_InvoiceRcv(objAPTrx.EnumInvoiceRcvStatus.Active)

        If lblInvoiceRcvID.Text.Trim = "" Then
            Exit Sub
        End If
        strSelectedInvRcvId = lblInvoiceRcvID.Text.Trim

        If Trim(strAccCode) = "" Then
            lblErrAccCode.Visible = True
            Exit Sub
        End If

        If Not blnIsBalanceSheet Then
            If strAccCode = "" And rbOTE.Checked = False Then
                lblErrAccCode.Visible = True
                Exit Sub
            ElseIf strBlkCode = "" And blnIsBlockRequire = True Then
                If ddlChargeLevel.SelectedIndex = 1 Then
                    lblErrBlock.Visible = True
                Else
                    lblPreBlockErr.Visible = True
                End If
                Exit Sub
            ElseIf strVehCode = "" And blnIsVehicleRequire = True Then
                lblErrVehicle.Visible = True
                Exit Sub
            ElseIf strVehExpenseCode = "" And blnIsVehicleRequire = True Then
                lblErrVehicleExp.Visible = True
                Exit Sub
            ElseIf strVehCode <> "" And strVehExpenseCode = "" And lblVehicleOption.Text = True Then
                lblErrVehicleExp.Visible = True
                Exit Sub
            ElseIf strVehCode = "" And strVehExpenseCode <> "" And lblVehicleOption.Text = True Then
                lblErrVehicle.Visible = True
                Exit Sub
            End If
        ElseIf blnIsNurseryInd = True Then
            If strAccCode = "" Then
                lblErrAccCode.Visible = True
                Exit Sub
            ElseIf strBlkCode = "" Then
                If ddlChargeLevel.SelectedIndex = 1 Then
                    lblErrBlock.Visible = True
                Else
                    lblPreBlockErr.Visible = True
                End If
                Exit Sub
            End If
        End If

        If rbSPO.Checked = True Then
            If CDec(arrParam(2)) < CDec(txtQty.Text.Trim) Then
                errOverQty.Visible = True
                Exit Sub
            End If
        End If
        If UBound(arrParam) >= 1 Then
            strItem = arrParam(1)
        End If

        intPPN = IIf(cbPPN.Checked = True, objAPTrx.EnumPPN.Yes, objAPTrx.EnumPPN.No)
        intPPHRate = IIf(txtPPHRate.Text.Trim <> "", txtPPHRate.Text.Trim, "0")

        'Get amount base on qty on GR
        'Dim strOpCd_GetAmount = "AP_CLSTRX_INVOICERECEIVE_GET_POAMOUNT"
        'Dim strParamName As String
        'Dim strParamValue As String
        'Dim strPOLnID As String
        'Dim strItemCode As String

        'strItemCode = lstItem.SelectedItem.Value
        'arrParam = lstItem.SelectedItem.Value.Split("|")

        'If UBound(arrParam) >= 1 Then
        '    strPOLnID = arrParam(0)
        '    strItemCode = arrParam(1)
        'End If

        'strParamName = "STRSEARCH"

        'strParamValue = " AND D.POId = '" & ddlPO.SelectedItem.Value & "' " & _
        '                " AND A.POLNID = '" & strPOLnID & "' " & _
        '                " AND A.ItemCode = '" & strItemCode & "' " & _
        '                " AND D.LocCode = '" & Trim(strLocation) & "' "

        'Try
        '    intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_GetAmount, _
        '                                        strParamName, _
        '                                        strParamValue, _
        '                                        objPOLnDs)

        'Catch Exp As System.Exception
        '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_GET_POLINE&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
        'End Try

        'If objPOLnDs.Tables(0).Rows.Count > 0 Then
        '    intCost = objPOLnDs.Tables(0).Rows(0).Item("Cost")
        '    intNetAmount = objPOLnDs.Tables(0).Rows(0).Item("NetAmount")
        '    intPPNAmount = objPOLnDs.Tables(0).Rows(0).Item("PPNAmount")
        'End If

        dblPBBKBAmount = IIf(txtPBBKB.Text.Trim = "", 0, txtPBBKB.Text.Trim)
        intPPHAmount = objAPTrx.RoundNumber((txtQty.Text.Trim * txtCost.Text.Trim) * (intPPHRate / 100), 0)

        If cbPPN.Checked = True Then
            'intPPNAmount = intPPNAmount '
            If txtAmtTransportFee.Visible = True Then
                intPPNAmount = (objAPTrx.RoundNumber((txtQty.Text.Trim * txtCost.Text.Trim) * hidExchangeRate.Value * (Session("SS_PPNRATE") / 100), 0)) + (objAPTrx.RoundNumber(txtAmtTransportFee.Text * (Session("SS_PPNRATE") / 100), 0))
                intNetAmount = (txtQty.Text.Trim * txtCost.Text.Trim * hidExchangeRate.Value) + CDbl(txtAmtTransportFee.Text)
            Else
                intPPNAmount = (objAPTrx.RoundNumber((txtQty.Text.Trim * txtCost.Text.Trim) * hidExchangeRate.Value * (Session("SS_PPNRATE") / 100), 0))
                intNetAmount = (txtQty.Text.Trim * txtCost.Text.Trim * hidExchangeRate.Value)
            End If
        Else
            If txtAmtTransportFee.Visible = True Then
                intNetAmount = (txtQty.Text.Trim * txtCost.Text.Trim * hidExchangeRate.Value) + CDbl(txtAmtTransportFee.Text)
            Else
                intNetAmount = (txtQty.Text.Trim * txtCost.Text.Trim * hidExchangeRate.Value)
            End If
            intPPNAmount = 0
        End If

        If rbSPO.Checked = True Then
            'intAmount = intNetAmount + intPPNAmount + dblPBBKBAmount
            intAmount = intNetAmount + intPPNAmount + dblPBBKBAmount + intPPHAmount 'capture pph 22 solar
            intPPHAmount = 0 'capture pph 22 solar
            intAmount = IIf(hidExchangeRate.Value = 1, objAPTrx.RoundNumber(intAmount, 2), objAPTrx.RoundNumber(intAmount, 2))
        Else
            intAmount = intNetAmount + intPPNAmount - intPPHAmount
            intAmount = IIf(hidExchangeRate.Value = 1, objAPTrx.RoundNumber(intAmount, 2), objAPTrx.RoundNumber(intAmount, 2))
        End If

        Dim strParam As String = "|" & _
                                 lblInvoiceRcvID.Text.Trim & "|" & _
                                 strItem & "|" & _
                                 strAccCode & "|" & _
                                 strBlkCode & "|" & _
                                 strVehCode & "|" & _
                                 strVehExpenseCode & "|" & _
                                 txtQty.Text.Trim & "|" & _
                                 txtCost.Text.Trim * hidExchangeRate.Value & "|" & _
                                 strParamID & "|" & _
                                 txtDONo.Text.Trim & "|" & _
                                 intAmount & "|" & _
                                 txtOtherInvNo.Text.Trim & "|" & _
                                 intPPHRate & "|" & _
                                 intPPN & "|" & _
                                 intPPNAmount & "|" & _
                                 intPPHAmount & "|" & _
                                 intNetAmount & "|" & _
                                 dblPBBKBAmount

        Try
            If ddlChargeLevel.SelectedIndex = 0 And RowPreBlk.Visible = True Then
                strParamList = Session("SS_LOCATION") & "|" & _
                                       ddlAccCode.SelectedItem.Value.Trim & "|" & _
                                       ddlPreBlock.SelectedItem.Value.Trim & "|" & _
                                       objGLSetup.EnumBlockStatus.Active
                intErrNo = objAPTrx.mtdAddInvoiceReceiveLnByBlock(strOpCodeGLSubBlkByBlk, _
                                                                  strParamList, _
                                                                  strOpCd_AddInvoiceRcvLn, _
                                                                  strOpCd_POLn_UPD, _
                                                                  strCompany, _
                                                                  strLocation, _
                                                                  strUserId, _
                                                                  strAccMonth, _
                                                                  strAccYear, _
                                                                  objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.InvoiceReceive), _
                                                                  objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.InvoiceReceiveLn), _
                                                                  strParam, _
                                                                  objID, _
                                                                  strLocType)
            Else
                intErrNo = objAPTrx.mtdAddInvoiceReceiveLn(strOpCd_AddInvoiceRcvLn, _
                                                            strOpCd_POLn_UPD, _
                                                            strCompany, _
                                                            strLocation, _
                                                            strUserId, _
                                                            strAccMonth, _
                                                            strAccYear, _
                                                            objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.InvoiceReceive), _
                                                            objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.InvoiceReceiveLn), _
                                                            strParam, _
                                                            objID)
            End If
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CREDITNOTE_ADD_LINEA&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_cnlist.aspx")
        End Try

        Update_AdvPayment(objAPTrx.EnumInvoiceRcvStatus.Active)
        Update_DNCNAmount(objAPTrx.EnumInvoiceRcvStatus.Active)
        onLoad_Display(lblInvoiceRcvID.Text.Trim)
        onLoad_DisplayItem(lblInvoiceRcvID.Text.Trim)
        Update_InvoiceRcv(objAPTrx.EnumInvoiceRcvStatus.Active)
        RefreshBtn_Click(Sender, E)

        If rbSPO.Checked = True Then
            'BindPO(ddlPO.SelectedItem.Value)
            txtCreditTerm.Text = strCreditTerm
        End If

        txtQty.Text = 0
        txtCost.Text = 0
        txtPBBKB.Text = 0
        txtAmtTransportFee.Text = 0
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs) Handles dgLineDet.DeleteCommand
        Dim strOpCode_DelLine As String = "AP_CLSTRX_INVOICERECEIVE_LINE_DEL"
        Dim strOpCd_POLn_UPD As String = "PU_CLSTRX_PO_LINE_UPD"
        Dim strOpCodes = strOpCode_DelLine & "|" & strOpCd_POLn_UPD
        Dim strCreditTerm As String = txtCreditTerm.Text
        Dim strParam As String
        Dim lbl As Label
        Dim strIRLNId As String
        Dim strPOLNId As String
        Dim strQty As String
        Dim intErrNo As Integer
        Dim strDate As String = Date_Validation(txtTransDate.Text, False)
        Dim indDate As String = ""

        If CheckDate(txtTransDate.Text.Trim(), indDate) = False Then
            lblErrTransDate.Visible = True
            lblFmtTransDate.Visible = True
            lblErrTransDate.Text = "<br>Date Entered should be in the format"
            Exit Sub
        End If

        Dim intInputPeriod As Integer = Year(strDate) * 100 + Month(strDate)
        Dim intCurPeriod As Integer = (CInt(strAccYear) * 100) + CInt(strAccMonth)
        Dim intSelPeriod As Integer = (CInt(strSelAccYear) * 100) + CInt(strSelAccMonth)

        If Session("SS_FILTERPERIOD") = "0" Then
            If intCurPeriod < intInputPeriod Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "Invalid transaction date."
                Exit Sub
            End If
        Else
            If intSelPeriod <> intInputPeriod Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "Invalid transaction date."
                Exit Sub
            End If
            If intSelPeriod < intCurPeriod And intLevel < 2 Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "This period already locked."
                Exit Sub
            End If
        End If

        dgLineDet.EditItemIndex = CInt(E.Item.ItemIndex)
        lbl = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lblInvoiceLnID")
        strIRLNId = lbl.Text
        lbl = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("POlnid")
        strPOLNId = lbl.Text & "#" & Trim(ddlPO.SelectedItem.Value)
        lbl = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("txtQtyInvoiced")
        strQty = lbl.Text

        Try
            strParam = strIRLNId & "|" & strPOLNId & "|" & strQty & "|" & lblInvoiceRcvID.Text.Trim
            intErrNo = objAPTrx.mtdDelInvoiceReceiveLn(strOpCodes, _
                                                        strCompany, _
                                                        strLocation, _
                                                        strUserId, _
                                                        strAccMonth, _
                                                        strAccYear, _
                                                        strParam)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERECEIVE_DEL_LINE&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_INVRCVlist.aspx")
        End Try

        onLoad_Display(lblInvoiceRcvID.Text.Trim)
        onLoad_DisplayItem(lblInvoiceRcvID.Text.Trim)
        Update_InvoiceRcv(objAPTrx.EnumInvoiceRcvStatus.Active)
        Update_AdvPayment(objAPTrx.EnumInvoiceRcvStatus.Active)
        onLoad_Display(lblInvoiceRcvID.Text.Trim)
        onLoad_DisplayItem(lblInvoiceRcvID.Text.Trim)
        onLoad_Button()
        If rbSPO.Checked = True Then
            BindPO(ddlPO.SelectedItem.Value)
            txtCreditTerm.Text = strCreditTerm
        End If
    End Sub

    'Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs) Handles dgLineDet.EditCommand
    '    Dim lbl As Label
    '    Dim strIRLNId As String
    '    Dim strPOLNId As String
    '    Dim strQty As String
    '    Dim strDate As String = Date_Validation(txtTransDate.Text, False)
    '    Dim indDate As String = ""

    '    If CheckDate(txtTransDate.Text.Trim(), indDate) = False Then
    '        lblErrTransDate.Visible = True
    '        lblFmtTransDate.Visible = True
    '        lblErrTransDate.Text = "<br>Date Entered should be in the format"
    '        Exit Sub
    '    End If

    '    Dim intInputPeriod As Integer = Year(strDate) * 100 + Month(strDate)
    '    Dim intCurPeriod As Integer = (CInt(strAccYear) * 100) + CInt(strAccMonth)
    '    Dim intSelPeriod As Integer = (CInt(strSelAccYear) * 100) + CInt(strSelAccMonth)

    '    If Session("SS_FILTERPERIOD") = "0" Then
    '        If intCurPeriod < intInputPeriod Then
    '            lblErrTransDate.Visible = True
    '            lblErrTransDate.Text = "Invalid transaction date."
    '            Exit Sub
    '        End If
    '    Else
    '        If intSelPeriod <> intInputPeriod Then
    '            lblErrTransDate.Visible = True
    '            lblErrTransDate.Text = "Invalid transaction date."
    '            Exit Sub
    '        End If
    '        If intSelPeriod < intCurPeriod And intLevel < 2 Then
    '            lblErrTransDate.Visible = True
    '            lblErrTransDate.Text = "This period already locked."
    '            Exit Sub
    '        End If
    '    End If

    '    dgLineDet.EditItemIndex = CInt(e.Item.ItemIndex)
    '    lbl = dgLineDet.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblInvoiceLnID")
    '    strIRLNId = lbl.Text
    '    lbl = dgLineDet.Items.Item(CInt(e.Item.ItemIndex)).FindControl("POlnid")
    '    strPOLNId = lbl.Text & "#" & Trim(ddlPO.SelectedItem.Value)
    '    lbl = dgLineDet.Items.Item(CInt(e.Item.ItemIndex)).FindControl("txtQtyInvoiced")
    '    txtQty.Text = lbl.Text
    '    lbl = dgLineDet.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblItemCode")
    '    GetItem(lbl.Text)
    '    lbl = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("txtUnitPrice")
    '    txtCost.Text = lbl.Text

    '    lbl = E.Item.FindControl("lblAccCode")
    '    strAccCode = lbl.Text.Trim
    '    BindAccCode(strAccCode)

    'End Sub


    Sub InvoiceType_OnCheckChange(ByVal Sender As Object, ByVal E As EventArgs)
        If rbSPO.Checked = True Then
            dgLineDet.Columns(13).Visible = False
            ddlPO.Enabled = True
            validateItem.Enabled = True
            lstItem.Enabled = True
            lblPPN.Visible = True
            cbPPN.Enabled = False
            cbPPN.Visible = True
            ddlSuppCode.Enabled = False
            lblErrSuppCode.Visible = False
            ddlSuppCode.SelectedIndex = 0
            lblPPH.Visible = True
            lblPercen.Visible = True
            txtPPHRate.Enabled = False
            txtPPHRate.Visible = True
            btnAddAllItem.Visible = True
            ddlTermType.SelectedIndex = 0
            BindPO("")
        ElseIf rbCWO.Checked = True Then
            dgLineDet.Columns(12).Visible = True
            dgLineDet.Columns(13).Visible = True
            dgLineDet.Columns(0).Visible = False
            ddlPO.Enabled = False
            ddlPO.SelectedIndex = 0
            validateItem.Enabled = False
            lstItem.Enabled = False
            lblPPN.Visible = True
            cbPPN.Enabled = True
            cbPPN.Visible = True
            ddlSuppCode.Enabled = True
            lblPPH.Visible = True
            lblPercen.Visible = True
            txtPPHRate.Enabled = True
            txtPPHRate.Visible = True
            btnAddAllItem.Visible = False
            ddlTermType.SelectedIndex = 1
            BindPO("")
        ElseIf rbOTE.Checked = True Then
            dgLineDet.Columns(12).Visible = True
            dgLineDet.Columns(13).Visible = True
            ddlPO.Enabled = False
            ddlPO.SelectedIndex = 0
            validateItem.Enabled = False
            lstItem.Enabled = False
            lblPPN.Visible = True
            cbPPN.Enabled = False
            txtPPHRate.Enabled = False
            cbPPN.Visible = True
            ddlSuppCode.Enabled = True
            lblPPH.Visible = True
            lblPercen.Visible = True
            txtPPHRate.Visible = True
            btnAddAllItem.Visible = False
            ddlTermType.SelectedIndex = 1
            BindPO("")
        ElseIf rbFFB.Checked = True Then
            dgLineDet.Columns(12).Visible = True
            dgLineDet.Columns(13).Visible = True
            ddlPO.Enabled = False
            ddlPO.SelectedIndex = 0
            validateItem.Enabled = True
            lstItem.Enabled = True
            lblPPN.Visible = True
            cbPPN.Enabled = False
            txtPPHRate.Enabled = False
            cbPPN.Visible = True
            ddlSuppCode.Enabled = True
            lblPPH.Visible = True
            lblPercen.Visible = True
            txtPPHRate.Visible = True
            btnAddAllItem.Visible = False
            ddlTermType.SelectedIndex = 1
            BindPO("")
            BindSupp("")
        ElseIf rbTransportFee.Checked = True Then
            dgLineDet.Columns(13).Visible = False
            ddlPO.Enabled = True
            validateItem.Enabled = False
            lstItem.Enabled = False
            lblPPN.Visible = False
            cbPPN.Enabled = False
            cbPPN.Visible = False
            ddlSuppCode.Enabled = False
            lblErrSuppCode.Visible = False
            ddlSuppCode.SelectedIndex = 0
            lblPPH.Visible = True
            lblPercen.Visible = True
            txtPPHRate.Enabled = False
            txtPPHRate.Visible = True
            btnAddAllItem.Visible = True
            ddlTermType.SelectedIndex = 1
            BindPO("")
        End If
    End Sub

    Sub BackBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("ap_trx_InvRcvList.aspx")
    End Sub

    Sub ItemIndexChanged(ByVal Sender As System.Object, ByVal e As System.EventArgs)
        Dim strOpCd_GetPOLn As String = "PU_CLSTRX_PO_LINE_GET_DETAIL_ITEM"
        Dim intCnt As Integer = 0
        Dim intErrNo As Integer
        Dim intSelectedIndex As Integer = 0
        Dim blnFound As Boolean = False
        Dim dr As DataRow
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim intItmCnt As Integer
        Dim arrParam As Array
        Dim strItemCode As String
        Dim strPOLnID As String

        strItemCode = lstItem.SelectedItem.Value
        onSelect_Change(Sender, e)
        'For intItmCnt = 1 To lstItem.Items.Count - 1
        '    arrParam = lstItem.Items(intItmCnt).Value.Split("|")
        '    strItemCode = arrParam(1).trim
        'Next
        lstItem.SelectedValue = strItemCode
        lstItem.SelectedItem.Value = lstItem.SelectedValue
        arrParam = lstItem.SelectedItem.Value.Split("|")

        If UBound(arrParam) >= 1 Then
            strPOLnID = arrParam(0)
            strItemCode = arrParam(1)
        End If

        strParamName = "POID|POLNID|ITEMCODE|LOCCODE"
        strParamValue = ddlPO.SelectedItem.Value & _
                        "|" & strPOLnID & _
                        "|" & strItemCode & _
                        "|" & strLocation

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_GetPOLn, _
                                                strParamName, _
                                                strParamValue, _
                                                objPODs)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_GET_POLINE&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
        End Try

        If objPODs.Tables(0).Rows.Count > 0 Then
            txtQty.Text = objPODs.Tables(0).Rows(0).Item("QtyOutstanding") 'objPODs.Tables(0).Rows(0).Item("QtyReceive") - objPODs.Tables(0).Rows(0).Item("QtyInvoice")
            txtCost.Text = Round(objPODs.Tables(0).Rows(0).Item("CostToDisplay"), 2) 'Round(objPODs.Tables(0).Rows(0).Item("CostAftDisc"), 2)
            'txtCost.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(objPODs.Tables(0).Rows(0).Item("CostToDisplay"), 2), 2)

            txtPBBKB.Text = Round(objPODs.Tables(0).Rows(0).Item("PBBKB"), 2)
            txtPPHRate.Text = objPODs.Tables(0).Rows(0).Item("PPN22")
            txtAmtTransportFee.Text = Round(objPODs.Tables(0).Rows(0).Item("NetAmtTransportFee"), 2)
            'txtAmtTransportFee.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(objPODs.Tables(0).Rows(0).Item("AmtTransportFee"), 2), 2)
            txtAmtTransportFee.Enabled = False
            hidUnitCost.Value = objPODs.Tables(0).Rows(0).Item("CostToDisplay")
            If objPODs.Tables(0).Rows(0).Item("NetAmtTransportFee") = 0 Then
                txtAmtTransportFee.Visible = False
                lblAmtTransportFee.Visible = False
            Else
                txtAmtTransportFee.Visible = True
                lblAmtTransportFee.Visible = True
            End If
        Else
            txtQty.Text = 0
            txtCost.Text = 0
            txtPBBKB.Text = 0
            txtAmtTransportFee.Text = 0
            txtAmtTransportFee.Visible = False
            lblAmtTransportFee.Visible = False
        End If
    End Sub

    Sub BtnAddAllItem_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim strOpCodeGLSubBlkByBlk As String = "GL_CLSSETUP_SUBBLOCK_BY_BLOCK_GET"
        Dim strParamList As String
        Dim strCreditTerm As String = txtCreditTerm.Text
        Dim blnIsValidDate As Boolean
        Dim strAccCode As String = Request.Form("ddlAccCode").Trim
        Dim strBlkCode As String
        If ddlChargeLevel.SelectedIndex = 1 Then
            strBlkCode = Request.Form("ddlBlock").Trim
        Else
            strBlkCode = Request.Form("ddlPreBlock").Trim
        End If
        Dim strVehCode As String = Request.Form("ddlVehCode").Trim
        Dim strVehExpenseCode As String = Request.Form("ddlVehExpCode").Trim

        Dim dblQty As Double
        Dim dblCost As Double
        Dim dblAmount As Double
        Dim dblPPHRate As Double
        Dim dblPPN As Double
        Dim dblPPNAmount As Double
        Dim dblPPHAmount As Double
        Dim dblNetAmount As Double
        Dim dblPBBKBAmount As Double
        Dim arrParam As Array
        Dim strParamID As String
        Dim strOpCd_AddInvoiceRcvLn As String = "AP_CLSTRX_INVOICERECEIVELN_ADD"
        Dim strOpCd_POLn_UPD As String = "PU_CLSTRX_PO_LINE_UPD"
        Dim intErrNo As Integer
        Dim objID As Object
        Dim blnIsBalanceSheet As Boolean
        Dim blnIsNurseryInd As Boolean
        Dim blnIsBlockRequire As Boolean
        Dim blnIsVehicleRequire As Boolean
        Dim blnIsOthers As Boolean
        Dim intCnt As Integer
        Dim strParamName As String
        Dim strParamValue As String
        Dim objPODs As Object
        Dim strOpCd_GetPOLine As String = "PU_CLSTRX_PO_LINE_GET"
        Dim strOpCdGetAcc As String = "GL_CLSSETUP_ENTRYSETUP_TYPE_GET"
        Dim objTemp As New DataSet()
        Dim strParam As String

        If ddlPO.SelectedItem.Value = "" Then
            lblErrPO.Visible = True
            Exit Sub
        End If
        Update_InvoiceRcv(objAPTrx.EnumInvoiceRcvStatus.Active)

        If lblInvoiceRcvID.Text.Trim = "" Then
            Exit Sub
        End If
        strSelectedInvRcvId = lblInvoiceRcvID.Text.Trim

        strParamName = "STRSEARCH"
        strParamValue = "AND A.POId = '" & Trim(ddlPO.SelectedItem.Value) & "' " & _
                                            " AND D.LocCode = '" & Trim(strLocation) & "' " & _
                                            " AND A.QtyReceive > 0 "

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_GetPOLine, _
                                                strParamName, _
                                                strParamValue, _
                                                objPODs)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PU_CLSTRX_PO_LINE_GET&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
        End Try

        For intCnt = 0 To objPODs.Tables(0).Rows.Count - 1
            strParam = objGlobal.EnumModule.Purchasing & "|" & objGL.EnumEntryType.PUDRGoodsReceive & "|" & strLocation
            Try
                intErrNo = objGL.mtdGetEntrySetupDet(strOpCdGetAcc, _
                                                     strParam, _
                                                     objTemp)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GL_SETUP_ENTRYSETUP_SAVE&errmesg=" & Exp.ToString & "&redirect=")
            End Try

            If objTemp.Tables(0).Rows.Count > 0 Then
                strAccCode = objTemp.Tables(0).Rows(0).Item("CRAccCode")
            End If

            If rbSPO.Checked = True Or rbOTE.Checked = True Or rbFFB.Checked = True Then
                strParamID = objPODs.Tables(0).Rows(intCnt).Item("POLnID") & "#" & objPODs.Tables(0).Rows(intCnt).Item("POID")
            Else
                arrParam = Split("||0", "|")
                strParamID = arrParam(0)
            End If
            strItem = objPODs.Tables(0).Rows(intCnt).Item("ItemCode")
            strBlkCode = ""
            strVehCode = ""
            strVehExpenseCode = ""
            dblQty = objPODs.Tables(0).Rows(intCnt).Item("QtyReceive") - objPODs.Tables(0).Rows(intCnt).Item("QtyInvoice")
            dblCost = Round(objPODs.Tables(0).Rows(intCnt).Item("Cost"), 2)
            dblPPN = objPODs.Tables(0).Rows(intCnt).Item("PPNCheck")
            If dblPPN = 1 Then
                dblPPNAmount = Round(dblQty * dblCost * 0.1, 0) 'Round(objPODs.Tables(0).Rows(intCnt).Item("PPNAmount"), 2)
            Else
                dblPPNAmount = Round(dblQty * dblCost * 0, 0) 'Round(objPODs.Tables(0).Rows(intCnt).Item("PPNAmount"), 2)
            End If
            dblPPHRate = "0"
            dblPPHAmount = "0"
            dblNetAmount = dblQty * dblCost 'Round(objPODs.Tables(0).Rows(intCnt).Item("NetAmount"), 2)
            dblPBBKBAmount = Round(dblQty * Round(dblCost * (0.5) * (objPODs.Tables(0).Rows(intCnt).Item("PBBKB") / 100), 2), 0) 'Round(objPODs.Tables(0).Rows(intCnt).Item("PBBKB"), 2)
            dblAmount = Round(dblNetAmount + dblPPNAmount + dblPBBKBAmount, 2) 'Round(objPODs.Tables(0).Rows(intCnt).Item("Amount"), 2)

            If dblQty > 0 Then
                strParam = "|" & _
                        lblInvoiceRcvID.Text.Trim & "|" & _
                        strItem & "|" & _
                        strAccCode & "|" & _
                        strBlkCode & "|" & _
                        strVehCode & "|" & _
                        strVehExpenseCode & "|" & _
                        dblQty & "|" & _
                        dblCost & "|" & _
                        strParamID & "|" & _
                        "|" & _
                        dblAmount & "|" & _
                        "|" & _
                        dblPPHRate & "|" & _
                        dblPPN & "|" & _
                        dblPPNAmount & "|" & _
                        dblPPHAmount & "|" & _
                        dblNetAmount & "|" & _
                        dblPBBKBAmount

                Try
                    If ddlChargeLevel.SelectedIndex = 0 And RowPreBlk.Visible = True Then
                        strParamList = Session("SS_LOCATION") & "|" & _
                                               ddlAccCode.SelectedItem.Value.Trim & "|" & _
                                               ddlPreBlock.SelectedItem.Value.Trim & "|" & _
                                               objGLSetup.EnumBlockStatus.Active
                        intErrNo = objAPTrx.mtdAddInvoiceReceiveLnByBlock(strOpCodeGLSubBlkByBlk, _
                                                                          strParamList, _
                                                                          strOpCd_AddInvoiceRcvLn, _
                                                                          strOpCd_POLn_UPD, _
                                                                          strCompany, _
                                                                          strLocation, _
                                                                          strUserId, _
                                                                          strAccMonth, _
                                                                          strAccYear, _
                                                                          objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.InvoiceReceive), _
                                                                          objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.InvoiceReceiveLn), _
                                                                          strParam, _
                                                                          objID, _
                                                                          strLocType)
                    Else
                        intErrNo = objAPTrx.mtdAddInvoiceReceiveLn(strOpCd_AddInvoiceRcvLn, _
                                                                    strOpCd_POLn_UPD, _
                                                                    strCompany, _
                                                                    strLocation, _
                                                                    strUserId, _
                                                                    strAccMonth, _
                                                                    strAccYear, _
                                                                    objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.InvoiceReceive), _
                                                                    objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.InvoiceReceiveLn), _
                                                                    strParam, _
                                                                    objID)
                    End If
                Catch Exp As System.Exception
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CREDITNOTE_ADD_LINEA&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_cnlist.aspx")
                End Try
            End If
        Next

        Update_AdvPayment(objAPTrx.EnumInvoiceRcvStatus.Active)
        Update_DNCNAmount(objAPTrx.EnumInvoiceRcvStatus.Active)
        onLoad_Display(lblInvoiceRcvID.Text.Trim)
        onLoad_DisplayItem(lblInvoiceRcvID.Text.Trim)
        Update_InvoiceRcv(objAPTrx.EnumInvoiceRcvStatus.Active)
        RefreshBtn_Click(sender, e)

        If rbSPO.Checked = True Then
            'BindPO(ddlPO.SelectedItem.Value)
            txtCreditTerm.Text = strCreditTerm
        End If
    End Sub

    Sub PrintDocBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim strTRXID As String

        strTRXID = Trim(lblInvoiceRcvID.Text)

        Response.Write("<Script Language=""JavaScript"">window.open(""../reports/AP_Rpt_InvRcvDet.aspx?Type=Print&CompName=" & strCompany & _
                        "&Location=" & strLocation & _
                        "&TrxID=" & strTRXID & _
                        "&TrxType=" & "1" & _
                        "&SupplierCode=" & "" & _
                        """,null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")

    End Sub

    Sub onLoad_DisplayAPNote(ByVal pv_strInvoiceRcvID As String)
        Dim objAPNote As Object
        Dim strOpCd_APNote As String
        Dim strAPNoteID As String
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer

        strOpCd_APNote = "AP_CLSTRX_INVOICERCV_NOTE_GET"
        strParamName = "STRSEARCH"
        strParamValue = "InvoiceRcvID = '" & pv_strInvoiceRcvID & "' "

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_APNote, _
                                                strParamName, _
                                                strParamValue, _
                                                objAPNote)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_CLSTRX_INVOICERCV_NOTE_GET_LASTNO&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
        End Try

        dgAPNote.DataSource = Nothing
        dgAPNote.DataSource = objAPNote.Tables(0)
        dgAPNote.DataBind()

        If objAPNote.Tables(0).Rows.Count > 0 Then
            BindTTRefNo(objAPNote.Tables(0).Rows(0).Item("TrxID").Trim())
        End If
    End Sub

    Sub onLoad_DisplayAdvancePayment(ByVal pv_strPOID As String)
        Dim strOpCd_GetAdvPayment = "AP_CLSTRX_INVOICERECEIVE_GET_ADVANCE_PAYMENT"
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer
        Dim dblPORate As Double
        Dim strPOCurrencyCode As String
        Dim strAdvAmount As String
        Dim strAdvCurrencyCode As String
        Dim dbCBExchangeRate As Double
        Dim dbAmount As Double
        Dim dbAmountToDisplay As Double
        Dim dbUsedAdvAmount As Double
        Dim dbUsedAdvAmountToDisplay As Double
        Dim intCnt As Integer = 0

        strParamName = "POID|LOCCODE"
        strParamValue = pv_strPOID & "|" & strLocation

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_GetAdvPayment, _
                                                strParamName, _
                                                strParamValue, _
                                                objPOLnDs)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_GET_POLINE&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
        End Try

        If objPOLnDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objPOLnDs.Tables(0).Rows.Count - 1
                strPOCurrencyCode = Trim(objPOLnDs.Tables(0).Rows(intCnt).Item("POCurrencyCode"))
                strAdvCurrencyCode = Trim(objPOLnDs.Tables(0).Rows(intCnt).Item("CurrencyCode"))
                dbCBExchangeRate = objPOLnDs.Tables(0).Rows(intCnt).Item("CBExchangeRate")
                dblPORate = objPOLnDs.Tables(0).Rows(intCnt).Item("POExchangeRate")
                hidPaymentID.Value = objPOLnDs.Tables(0).Rows(intCnt).Item("PaymentID")
                lblCurrency3.Text = objPOLnDs.Tables(0).Rows(intCnt).Item("POCurrencyCode")
                hidAdvExchangeRate.Value = objPOLnDs.Tables(0).Rows(intCnt).Item("ExchangeRate")
                hidPOCurrencyCode.Value = Trim(objPOLnDs.Tables(0).Rows(intCnt).Item("POCurrencyCode"))
                hidAdvCurrencyCode.Value = Trim(objPOLnDs.Tables(0).Rows(intCnt).Item("CurrencyCode"))

                dbAmountToDisplay = dbAmountToDisplay + objPOLnDs.Tables(0).Rows(intCnt).Item("AmountToDisplay")
                dbUsedAdvAmountToDisplay = dbUsedAdvAmountToDisplay + objPOLnDs.Tables(0).Rows(intCnt).Item("UsedAdvAmountToDisplay")

                dbAmount = dbAmount + objPOLnDs.Tables(0).Rows(intCnt).Item("Amount")
                dbUsedAdvAmount = dbUsedAdvAmount + objPOLnDs.Tables(0).Rows(intCnt).Item("UsedAdvAmount")
            Next

            If Trim(strAdvCurrencyCode) = Trim(strPOCurrencyCode) Then
                hidAdvAmount.Value = dbUsedAdvAmountToDisplay
                strAdvAmount = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(hidAdvAmount.Value, 2), 2)
                lblIDUsedAdvPayAmount.Text = strAdvAmount
            Else
                If Trim(strPOCurrencyCode) = "IDR" Then
                    hidAdvAmount.Value = dbUsedAdvAmountToDisplay * dbCBExchangeRate
                    strAdvAmount = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(hidAdvAmount.Value, 2), 2)
                    lblIDUsedAdvPayAmount.Text = strAdvAmount
                Else
                    hidAdvAmount.Value = dbUsedAdvAmount / dblPORate
                    strAdvAmount = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(hidAdvAmount.Value, 2), 2)
                    lblIDUsedAdvPayAmount.Text = strAdvAmount
                End If
            End If

            If lblInvoiceRcvID.Text = "" Or dgLineDet.Items.Count = 0 Then
                lblCurrency4.Text = Trim(strPOCurrencyCode)
                If Trim(strAdvCurrencyCode) = Trim(strPOCurrencyCode) Then
                    hidAdvPayment.Value = dbAmountToDisplay - dbUsedAdvAmountToDisplay
                    strAdvAmount = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(hidAdvPayment.Value, 2), 2)
                    lblAdvPaymentAmount.Text = strAdvAmount
                Else
                    If Trim(strPOCurrencyCode) = "IDR" Then
                        hidAdvPayment.Value = (dbAmountToDisplay - dbUsedAdvAmountToDisplay) * dbCBExchangeRate
                        strAdvAmount = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(hidAdvPayment.Value, 2), 2)
                        lblAdvPaymentAmount.Text = strAdvAmount & " (" & strAdvCurrencyCode & " " & objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(dbAmountToDisplay - dbUsedAdvAmountToDisplay, 2), 2) & ")"
                    Else
                        hidAdvPayment.Value = (dbAmount - dbUsedAdvAmount) / dblPORate
                        strAdvAmount = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(hidAdvPayment.Value, 2), 2)
                        lblAdvPaymentAmount.Text = strAdvAmount & " (" & strAdvCurrencyCode & " " & objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(dbAmountToDisplay - dbUsedAdvAmountToDisplay, 2), 2) & ")"
                    End If
                End If
            End If

            'dblPORate = objPOLnDs.Tables(0).Rows(0).Item("POExchangeRate")
            'hidPaymentID.Value = objPOLnDs.Tables(0).Rows(0).Item("PaymentID")
            'lblCurrency3.Text = objPOLnDs.Tables(0).Rows(0).Item("POCurrencyCode")
            'hidAdvExchangeRate.Value = objPOLnDs.Tables(0).Rows(0).Item("ExchangeRate")
            'hidPOCurrencyCode.Value = Trim(objPOLnDs.Tables(0).Rows(0).Item("POCurrencyCode"))
            'hidAdvCurrencyCode.Value = Trim(objPOLnDs.Tables(0).Rows(0).Item("CurrencyCode"))

            'If Trim(objPOLnDs.Tables(0).Rows(0).Item("CurrencyCode")) = Trim(objPOLnDs.Tables(0).Rows(0).Item("POCurrencyCode")) Then
            '    hidAdvAmount.Value = objPOLnDs.Tables(0).Rows(0).Item("UsedAdvAmountToDisplay")
            '    strAdvAmount = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(hidAdvAmount.Value, 2), 2)
            '    lblIDUsedAdvPayAmount.Text = strAdvAmount
            'Else
            '    If Trim(objPOLnDs.Tables(0).Rows(0).Item("POCurrencyCode")) = "IDR" Then
            '        hidAdvAmount.Value = (objPOLnDs.Tables(0).Rows(0).Item("UsedAdvAmountToDisplay")) * objPOLnDs.Tables(0).Rows(0).Item("CBExchangeRate")
            '        strAdvAmount = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(hidAdvAmount.Value, 2), 2)
            '        lblIDUsedAdvPayAmount.Text = strAdvAmount
            '    Else
            '        hidAdvAmount.Value = (objPOLnDs.Tables(0).Rows(0).Item("UsedAdvAmount")) / dblPORate
            '        strAdvAmount = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(hidAdvAmount.Value, 2), 2)
            '        lblIDUsedAdvPayAmount.Text = strAdvAmount
            '    End If
            'End If

            'If lblInvoiceRcvID.Text = "" Or dgLineDet.Items.Count = 0 Then
            '    lblCurrency4.Text = objPOLnDs.Tables(0).Rows(0).Item("POCurrencyCode")
            '    If Trim(objPOLnDs.Tables(0).Rows(0).Item("CurrencyCode")) = Trim(objPOLnDs.Tables(0).Rows(0).Item("POCurrencyCode")) Then
            '        hidAdvPayment.Value = objPOLnDs.Tables(0).Rows(0).Item("AmountToDisplay") - objPOLnDs.Tables(0).Rows(0).Item("UsedAdvAmountToDisplay")
            '        strAdvAmount = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(hidAdvPayment.Value, 2), 2)
            '        lblAdvPaymentAmount.Text = strAdvAmount
            '    Else
            '        If Trim(objPOLnDs.Tables(0).Rows(0).Item("POCurrencyCode")) = "IDR" Then
            '            hidAdvPayment.Value = (objPOLnDs.Tables(0).Rows(0).Item("AmountToDisplay") - objPOLnDs.Tables(0).Rows(0).Item("UsedAdvAmountToDisplay")) * objPOLnDs.Tables(0).Rows(0).Item("CBExchangeRate")
            '            strAdvAmount = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(hidAdvPayment.Value, 2), 2)
            '            lblAdvPaymentAmount.Text = strAdvAmount & " (" & objPOLnDs.Tables(0).Rows(0).Item("CurrencyCode") & " " & objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(objPOLnDs.Tables(0).Rows(0).Item("AmountToDisplay") - objPOLnDs.Tables(0).Rows(0).Item("UsedAdvAmountToDisplay"), 2), 2) & ")"
            '        Else
            '            hidAdvPayment.Value = (objPOLnDs.Tables(0).Rows(0).Item("Amount") - objPOLnDs.Tables(0).Rows(0).Item("UsedAdvAmount")) / dblPORate
            '            strAdvAmount = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(hidAdvPayment.Value, 2), 2)
            '            lblAdvPaymentAmount.Text = strAdvAmount & " (" & objPOLnDs.Tables(0).Rows(0).Item("CurrencyCode") & " " & objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(objPOLnDs.Tables(0).Rows(0).Item("AmountToDisplay") - objPOLnDs.Tables(0).Rows(0).Item("UsedAdvAmountToDisplay"), 2), 2) & ")"
            '        End If
            '    End If
            'End If
        End If
    End Sub

    Sub Update_AdvPayment(ByVal intStatus As Integer)
        Dim strOpCd_UpdAdvPayment = "AP_CLSTRX_INVOICERECEIVE_UPD_ADVPAYMENT"
        Dim strOpCd_GetIRLine As String = "AP_CLSTRX_INVOICERECEIVE_LINE_GET"
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer
        Dim dblTotalAmount As Double = 0
        Dim dblAmount As Double = 0
        Dim dblUsedAdvAmount As Double
        Dim dblOutAdvAmount As Double
        Dim intCnt As Integer

        strParamName = "INVOICERCVID|LOCCODE"
        strParamValue = lblInvoiceRcvID.Text & "|" & strLocation

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_GetIRLine, _
                                                strParamName, _
                                                strParamValue, _
                                                objInvRcvLnDs)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_GET_POLINE&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
        End Try

        For intCnt = 0 To objInvRcvLnDs.Tables(0).Rows.Count - 1
            dblAmount = objInvRcvLnDs.Tables(0).Rows(intCnt).Item("Amount")
            dblTotalAmount += dblAmount
        Next

        If dblTotalAmount > 0 Then
        Else
            dblTotalAmount = 0
            hidPaymentID.Value = ""
        End If

        If hidAdvPayment.Value > 0 Then
            dblOutAdvAmount = CDbl(hidAdvPayment.Value) - CDbl(hidAdvAmount.Value)
            Select Case CDbl(dblTotalAmount)
                Case Is < CDbl(dblOutAdvAmount)
                    hidUsedAdvAmount.Value = dblTotalAmount
                    dblUsedAdvAmount = dblTotalAmount
                Case Is > CDbl(dblOutAdvAmount)
                    hidUsedAdvAmount.Value = dblOutAdvAmount
                    dblUsedAdvAmount = dblOutAdvAmount
                Case Else
                    hidUsedAdvAmount.Value = dblTotalAmount
                    dblUsedAdvAmount = dblTotalAmount
            End Select
        Else
            hidUsedAdvAmount.Value = 0
            dblUsedAdvAmount = 0
        End If

        'dblUsedAdvAmount = dblUsedAdvAmount * hidExchangeRate.Value
        dblUsedAdvAmount = dblUsedAdvAmount



        strParamName = "INVOICERCVID|STATUS|OUTSTANDINGAMOUNT|USEDADVAMOUNT|PAYMENTID"
        strParamValue = lblInvoiceRcvID.Text & "|" & _
                        intStatus & "|" & _
                        dblTotalAmount & "|" & _
                        dblUsedAdvAmount & "|" & _
                        hidPaymentID.Value

        Try
            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd_UpdAdvPayment, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_UPD_POSTATUS&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
        End Try
    End Sub

    Sub onLoad_DisplayDNCNAmount(ByVal pv_strPOID As String)
        Dim strOpCd_GetDNCNAmount = "AP_CLSTRX_INVOICERECEIVE_GET_DEBITCREDITNOTE"
        Dim strDNCNAmount As String
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer
        Dim intCnt As Integer = 0
        Dim dbDNCNAmount As Double
        Dim dbUsedDNCNAmount As Double
        Dim dbUsedDNCNAmountToDisplay As Double

        If ddlPO.SelectedItem.Value <> "" Then
            strParamName = "POID|SUPPLIERCODE|LOCCODE"
            strParamValue = ddlPO.SelectedItem.Value & "|" & Trim(ddlSuppCode.SelectedItem.Value) & "|" & strLocation

            Try
                intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_GetDNCNAmount, _
                                                    strParamName, _
                                                    strParamValue, _
                                                    objPOLnDs)

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_GET_POLINE&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
            End Try

            If objPOLnDs.Tables(0).Rows.Count > 0 Then
                For intCnt = 0 To objPOLnDs.Tables(0).Rows.Count - 1
                    hidDNCNID.Value = Trim(objPOLnDs.Tables(0).Rows(intCnt).Item("DNCNID"))
                    lblCurrency5.Text = objPOLnDs.Tables(0).Rows(intCnt).Item("CurrencyCode")
                    lblCurrency6.Text = objPOLnDs.Tables(0).Rows(intCnt).Item("CurrencyCode")
                    dbDNCNAmount = dbDNCNAmount + objPOLnDs.Tables(0).Rows(intCnt).Item("TotalAmountToDisplay")
                    dbUsedDNCNAmount = dbUsedDNCNAmount + objPOLnDs.Tables(0).Rows(intCnt).Item("UsedDNCNAmountToDisplay")
                Next

                strDNCNAmount = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(dbDNCNAmount, 2), 2)
                lblDNCNAmount.Text = strDNCNAmount
                hidDNCNAmount.Value = dbDNCNAmount
                txtUsedDNCNAmount.Text = dbDNCNAmount - dbUsedDNCNAmount

                If CDbl(hidDNCNAmount.Value) <> 0 Then
                    TrAdj.Visible = True
                    txtUsedDNCNAmount.Visible = True
                    txtOutPayAmount.Visible = True
                    lblIDOutPayAmount.Visible = False
                Else
                    TrAdj.Visible = False
                    txtUsedDNCNAmount.Visible = False
                    txtOutPayAmount.Visible = False
                    lblIDOutPayAmount.Visible = True
                End If
            Else
                lblCurrency5.Text = "IDR"
                lblDNCNAmount.Text = CDbl(0)
                lblCurrency6.Text = "IDR"
                txtUsedDNCNAmount.Text = CDbl(0)
                txtUsedDNCNAmount.Visible = False
                txtOutPayAmount.Visible = False
            End If
        End If
    End Sub

    Sub Update_DNCNAmount(ByVal intStatus As Integer)
        Dim strOpCd_UpdAdvPayment = "AP_CLSTRX_INVOICERECEIVE_UPD_DNCNAMOUNT"
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer

        strParamName = "LOCCODE|INVOICERCVID|DNCNID|DNCNAMOUNT|USEDDNCNAMOUNT"
        strParamValue = strLocation & "|" & _
                        lblInvoiceRcvID.Text & "|" & _
                        Trim(hidDNCNID.Value) & "|" & _
                        CDbl(hidDNCNAmount.Value) & "|" & _
                        CDbl(Replace(Replace(txtUsedDNCNAmount.Text.Trim, ".", ""), ",", "."))

        Try
            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd_UpdAdvPayment, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_UPD_POSTATUS&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
        End Try
    End Sub

    Sub Update_CJAllocation(ByVal intStatus As Integer)
        Dim strOpCd_GenCJ = "AP_CLSTRX_INVOICERECEIVE_GENERATE_CJ"
        Dim strOpCd_UpdCJ = "AP_CLSTRX_INVOICERECEIVE_UPD_CJ"
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer
        Dim strDate As String = Date_Validation(txtTransDate.Text, False)

        strAccYear = Year(strDate)
        strAccMonth = Month(strDate)

        If CDbl(hidDNCNAmount.Value) <> 0 And CDbl(Replace(Replace(txtUsedDNCNAmount.Text.Trim, ".", ""), ",", ".")) <> 0 Then
            If intStatus = objAPTrx.EnumInvoiceRcvStatus.Confirmed Then
                strParamName = "LOCCODE|INVOICERCVID|ACCMONTH|ACCYEAR|USERID|USEDDNCNAMOUNT"
                strParamValue = strLocation & "|" & _
                                lblInvoiceRcvID.Text & "|" & _
                                strAccMonth & "|" & _
                                strAccYear & "|" & _
                                strUserId & "|" & _
                                CDbl(Replace(Replace(txtUsedDNCNAmount.Text.Trim, ".", ""), ",", "."))

                Try
                    intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd_GenCJ, _
                                                            strParamName, _
                                                            strParamValue)

                Catch Exp As System.Exception
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_UPD_POSTATUS&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
                End Try
            ElseIf intStatus = objAPTrx.EnumInvoiceRcvStatus.Cancelled Then
                strParamName = "LOCCODE|CREDITJRNID|UPDATEID|REMARK|STATUS|UPDATEDATE"
                strParamValue = strLocation & "|" & _
                                hidAPCJID.Value & "|" & _
                                strUserId & "||4|" & _
                                Now()

                Try
                    intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd_UpdCJ, _
                                                            strParamName, _
                                                            strParamValue)

                Catch Exp As System.Exception
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_UPD_POSTATUS&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
                End Try
            End If
        End If
    End Sub

    Sub onLoad_DisplayAPCJ(ByVal pv_strInvoiceRcvID As String)
        Dim objCJ As Object
        Dim strOpCd_APCJ As String = "AP_CLSTRX_INVOICERECEIVE_GET_CJ"
        Dim strAPNoteID As String
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer

        strParamName = "INVOICERCVID|LOCCODE"
        strParamValue = Trim(pv_strInvoiceRcvID) & "|" & strLocation

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_APCJ, _
                                                strParamName, _
                                                strParamValue, _
                                                objCJ)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_CLSTRX_INVOICERCV_NOTE_GET_LASTNO&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
        End Try

        dgAPCJ.DataSource = Nothing
        dgAPCJ.DataSource = objCJ.Tables(0)
        dgAPCJ.DataBind()

        If objCJ.Tables(0).Rows.Count > 0 Then
            hidAPCJID.Value = Trim(objCJ.Tables(0).Rows(0).Item("CreditJrnID"))
        End If
    End Sub

    Sub NewBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Response.Redirect("AP_trx_InvRcvDet.aspx")
    End Sub

    Sub BindTTRefNo(ByVal pv_strRefNo As String)
        Dim strOpCode As String = "AP_CLSTRX_INVOICERCV_NOTE_REFNO_GET"
        Dim intErrNo As Integer
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim dsMaster As Object
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer = 0
        Dim strSupplierCode As String = idSuppCode.Value
        Dim strDate As String = Date_Validation(txtTransDate.Text, False)

        strAccYear = Year(strDate)
        strAccMonth = Month(strDate)
        'If strPOID <> "" Then
        strParamName = "STRSEARCH"
        strParamValue = " SupplierCode = '" & strSupplierCode & "' AND AccMonth = '" & strAccMonth & "' AND AccYear = '" & strAccYear & "'"

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
                                                strParamName, _
                                                strParamValue, _
                                                dsMaster)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=IN_ITEM_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To dsMaster.Tables(0).Rows.Count - 1
            dsMaster.Tables(0).Rows(intCnt).Item("TrxID") = dsMaster.Tables(0).Rows(intCnt).Item("TrxID").Trim()
            dsMaster.Tables(0).Rows(intCnt).Item("Description") = dsMaster.Tables(0).Rows(intCnt).Item("TrxID")
            If dsMaster.Tables(0).Rows(intCnt).Item("TrxID") = pv_strRefNo Then
                intSelectedIndex = intCnt + 1
            End If
        Next intCnt

        Dim dr As DataRow
        dr = dsMaster.Tables(0).NewRow()
        dr("TrxID") = ""
        dr("Description") = lblPleaseSelect.Text & " Tanda Terima Reference No."
        dsMaster.Tables(0).Rows.InsertAt(dr, 0)

        ddlTTRefNo.DataSource = dsMaster.Tables(0)
        ddlTTRefNo.DataValueField = "TrxID"
        ddlTTRefNo.DataTextField = "Description"
        ddlTTRefNo.DataBind()
        ddlTTRefNo.SelectedIndex = intSelectedIndex
        ddlTTRefNo.AutoPostBack = True
        'End If
    End Sub

    Sub GetItem(ByVal pv_strItemCode As String)
        Dim dr As DataRow
        Dim intCnt As Integer = 0
        Dim intErrNo As Integer
        Dim intSelectedIndex As Integer = 0
        Dim strPOType As String

        Dim strOpCode As String = "IN_CLSTRX_ITEMPART_ITEM_GET"
        Dim dsMaster As Object
        Dim strParamName As String = ""
        Dim strParamValue As String = ""

        strParamName = "SEARCHSTR|SORTEXP"
        strParamValue = " AND itm.ItemCode = '" & Trim(pv_strItemCode) & "' AND itm.LocCode = '" & strLocation & "' AND itm.Status = '" & objINSetup.EnumStockItemStatus.Active & "'  " & "|itm.ItemCode"

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
                                                strParamName, _
                                                strParamValue, _
                                                dsMaster)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GENERATE_INVOICE_WM_GET_HEADER&errmesg=" & Exp.ToString() & "&redirect=ap/trx/ap_trx_invrcv_wm_list.aspx")
        End Try

        For intCnt = 0 To dsMaster.Tables(0).Rows.Count - 1
            If dsMaster.Tables(0).Rows(intCnt).Item("ItemCode") = Trim(pv_strItemCode) Then
                intSelectedIndex = intCnt + 1
            End If
        Next intCnt

        dr = dsMaster.Tables(0).NewRow()
        dr("ItemCode") = ""
        dr("Description") = "Please select Item Code"
        dsMaster.Tables(0).Rows.InsertAt(dr, 0)

        lstItem.DataSource = dsMaster.Tables(0)
        lstItem.DataValueField = "ItemCode"
        lstItem.DataTextField = "Description"
        lstItem.DataBind()
        lstItem.SelectedIndex = intSelectedIndex
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
                        Session("SS_USERID") & "|" & Trim(lblInvoiceRcvID.Text)

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
            lblTotalDB.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(TotalDB, 2), 2)
            lblTotalCR.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(TotalCR, 2), 2)

            dgViewJournal.DataSource = Nothing
            dgViewJournal.DataSource = dsResult.Tables(0)
            dgViewJournal.DataBind()

            lblTotalDB.Visible = True
            lblTotalCR.Visible = True
            lblTotalViewJournal.Visible = True
            lblTotalViewJournal.Text = "Total Amount : "
        End If

        strSelectedInvRcvId = Trim(lblInvoiceRcvID.Text)
        onLoad_Display(strSelectedInvRcvId)
        onLoad_DisplayItem(strSelectedInvRcvId)
        onLoad_Button()
    End Sub

    Sub EditBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)

    End Sub
End Class
