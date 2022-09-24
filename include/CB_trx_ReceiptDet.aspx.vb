
Imports System
Imports System.Data
Imports System.Math

Public Class cb_trx_ReceiptDet : Inherits Page

    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblErrBillParty As Label
    Protected WithEvents lblErrNoSelectDoc As Label
    Protected WithEvents lblErrManySelectDoc As Label
    Protected WithEvents lblErrAmount As Label
    Protected WithEvents lblErrAction As Label
    Protected WithEvents lblPleaseSelect As Label
    Protected WithEvents lblErrTotal As Label
    Protected WithEvents lblSelect As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents lblAccount As Label

    Protected WithEvents txtAccCodeOth As TextBox
    Protected WithEvents txtAccNameOth As TextBox

    Protected WithEvents txtAccCode As TextBox
    Protected WithEvents txtAccName As TextBox

    Protected WithEvents lblBillPartyTag As Label
    Protected WithEvents lblAccCodeTag As Label

    Protected WithEvents ddlChargeLevel As DropDownList
    Protected WithEvents lblPreBlkTag As Label
    Protected WithEvents ddlPreBlock As DropDownList
    Protected WithEvents lblPreBlockErr As Label
    Protected WithEvents lblBlkTag As Label
    Protected WithEvents lstBlock As DropDownList
    Protected WithEvents lblBlockErr As Label
    Protected WithEvents lblVehTag As Label
    Protected WithEvents lstVehCode As DropDownList
    Protected WithEvents lblVehCodeErr As Label
    Protected WithEvents lblVehExpTag As Label
    Protected WithEvents lstVehExp As DropDownList
    Protected WithEvents lblVehExpCodeErr As Label
    Protected WithEvents RowChargeLevel As HtmlTableRow
    Protected WithEvents RowPreBlk As HtmlTableRow
    Protected WithEvents RowBlk As HtmlTableRow
    Protected WithEvents hidBlockCharge As HtmlInputHidden    
    Dim strPreBlockTag As String
    Dim strVehTag As String
    Dim strBlockTag As String
    Dim strVehExpTag As String

    Protected WithEvents lblReceiptID As Label
    Protected WithEvents ddlBillParty As DropDownList
    Protected WithEvents ddlRecType As DropDownList
    Protected WithEvents ddlBank As DropDownList

    Protected WithEvents txtChequeNo As TextBox
    Protected WithEvents txtRemark As TextBox
    Protected WithEvents lblTotalAmount As Label

    Protected WithEvents lblAccPeriod As Label
    Protected WithEvents lblStatus As Label
    Protected WithEvents lblDateCreated As Label
    Protected WithEvents txtDateCreated As TextBox
    Protected WithEvents btnDateCreated As Image
    Protected WithEvents lblDate As Label
    Protected WithEvents lblFmt As Label
    Protected WithEvents lblLastUpdate As Label
    Protected WithEvents lblPrintDate As Label
    Protected WithEvents lblUpdatedBy As Label

    Protected WithEvents ddlInvoice As DropDownList
    Protected WithEvents ddlDebitNote As DropDownList
    Protected WithEvents ddlCreditNote As DropDownList
    Protected WithEvents ddlDebtorJrn As DropDownList
    'Protected WithEvents ddlAccount As DropDownList
    Protected WithEvents txtAmount As TextBox

    Protected WithEvents tblSelection As HtmlTable
    Protected WithEvents dgLineDet As DataGrid

    Protected WithEvents lblDocID As Label
    Protected WithEvents DocType As Label
    Protected WithEvents lblAccCode As Label
    Protected WithEvents lblLnID As Label
    Protected WithEvents lblAmount As Label

    Protected WithEvents AddBtn As ImageButton
    Protected WithEvents SaveBtn As ImageButton
    Protected WithEvents ConfirmBtn As ImageButton
    Protected WithEvents EditBtn As ImageButton
    Protected WithEvents DeleteBtn As ImageButton
    Protected WithEvents PrintBtn As ImageButton
    Protected WithEvents BackBtn As ImageButton
    Protected WithEvents btnFind1 As HtmlInputButton

    Protected WithEvents hidReceiptID As HtmlInputHidden
    Protected WithEvents hidStatus As HtmlInputHidden
    Protected WithEvents hidTotalAmt As HtmlInputHidden

    Protected WithEvents rfvAccCode As RequiredFieldValidator

    Protected WithEvents ddlCurrency As DropDownList
    Protected WithEvents txtExRate As TextBox
    Protected WithEvents hidARCurrency As HtmlInputHidden
    Protected WithEvents hidARExRate As HtmlInputHidden
    Protected WithEvents hidARAmount As HtmlInputHidden
    Protected WithEvents lblCurrency As Label

    Protected WithEvents TrLink As HtmlTableRow
    Protected WithEvents lbViewJournal As LinkButton
    Protected WithEvents dgViewJournal As DataGrid
    Protected WithEvents lblTotalDB As Label
    Protected WithEvents lblTotalCR As Label
    Protected WithEvents lblTotalViewJournal As Label

    Protected WithEvents txtAddNote As HtmlTextArea

    'Protected WithEvents ddlOther As DropDownList
    Protected WithEvents ddlContract As DropDownList
    Protected WithEvents RowTax As HtmlTableRow
    Protected WithEvents RowTaxAmt As HtmlTableRow
    Protected WithEvents lstTaxObject As DropDownList
    Protected WithEvents lblTaxObjectErr As Label
    Protected WithEvents txtDPPAmount As TextBox
    Protected WithEvents hidNPWPNo As HtmlInputHidden
    Protected WithEvents hidTaxObjectRate As HtmlInputHidden
    Protected WithEvents lblErrAmountDPP As Label
    Protected WithEvents hidCOATax As HtmlInputHidden
    Protected WithEvents hidPOPPH23 As HtmlInputHidden
    Protected WithEvents hidFindPOPPH23 As HtmlInputHidden
    Protected WithEvents lblFindINVPOPPH23 As Label
    Protected WithEvents lblFindPOPPH23 As Label
    Protected WithEvents hidTaxStatus As HtmlInputHidden
    Protected WithEvents hidHadCOATax As HtmlInputHidden
    Protected WithEvents hidInvAmount As HtmlInputHidden
    Protected WithEvents LblIsSKBActive As Label
    Protected WithEvents lblSKBStartDate As Label
    Protected WithEvents hidPPNInit As HtmlInputHidden

    Protected WithEvents lblViewTotalAmount As Label
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Protected WithEvents lblPPN As Label
    Protected WithEvents BlblPPH As Label
    Protected WithEvents lblPercen As Label
    Protected WithEvents cbPPN As CheckBox
    Protected WithEvents txtPPHRate As TextBox
    Protected WithEvents lblPPHRateHidden As Label
    Protected WithEvents lblPPNHidden As Label
    Protected WithEvents lblErrValidPPNHRate As Label

    Protected WithEvents rblReceiptTerm As RadioButtonList

    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objGLSetup As New agri.GL.clsSetup()
    Dim objHRSetup As New agri.HR.clsSetup()
    Dim objCBTrx As New agri.CB.clsTrx
    Dim objBITrx As New agri.BI.clsTrx
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objAdmin As New agri.Admin.clsShare()
    Dim objLangCap As New agri.PWSystem.clsLangCap
    Dim objCMSetup As New agri.CM.clsSetup()
    Dim objGLtrx As New agri.GL.ClsTrx()
    Dim objCMTrx As New agri.CM.clsTrx()

    Dim objReceiptDs As New DataSet()
    Dim objReceiptLnDs As New DataSet()
    Dim objBPDs As New Object()
    Dim objBankDs As New Object()
    Dim objEDitDs As New Object()
    Dim objInvDs As New Object()
    Dim objDNDs As New Object()
    Dim objCNDs As New Object()
    Dim objDJDs As New Object()
    Dim objAccDs As New Object()
    Dim objBlkDs As New Object()
    Dim objLangCapDs As New Object()

    Dim strOpCd_GetLine As String = "CB_CLSTRX_RECEIPT_LINE_GET"
    Dim strOpCode_Receipt_Upd As String = "CB_CLSTRX_RECEIPT_UPD"
    Dim strOpCode_Invoice_Upd As String = "BI_CLSTRX_INVOICE_UPD"
    Dim strOpCode_DebitNote_Upd As String = "BI_CLSTRX_DEBITNOTE_UPD"
    Dim strOpCode_CreditNote_Upd As String = "BI_CLSTRX_CREDITNOTE_UPD"
    Dim strOpCode_DebtorJrn_Upd As String = "BI_CLSTRX_DEBTORJOURNAL_UPD"
    Dim strOpCode_Invoice_OutstdAmount_Get As String = "BI_CLSTRX_INVOICE_OUTSTDAMT_GET"
    Dim strOpCode_DebitNote_OutstdAmount_Get As String = "BI_CLSTRX_DEBITNOTE_OUTSTDAMT_GET"
    Dim strOpCode_CreditNote_OutstdAmount_Get As String = "BI_CLSTRX_CREDITNOTE_OUTSTDAMT_GET"
    Dim strOpCode_DebtorJrn_OutstdAmount_Get As String = "BI_CLSTRX_DEBTORJOURNAL_OUTSTDAMT_GET"

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intCBAR As Integer
    Dim intConfig As Integer

    Dim strSelectedReceiptID As String
    Dim intReceiptStatus As Integer
    Dim strAcceptDateFormat As String
    Dim arrAmount As Array

    Dim strParam As String = ""
    Dim intErrNo As Integer = 0
    Dim intCnt As Integer = 0
    Dim dr As DataRow
    Dim strLocType As String
    Dim pv_strCurrencyCode As String
    Dim strCurrency As String
    Dim strExRate As String
    Dim strARExRate As String
    Dim strCBExRate As String
    Dim TaxLnID As String = ""
    Dim TaxRate As Double = 0
    Dim DPPAmount As Double = 0
    Dim strParamName As String
    Dim strParamValue As String

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strAccMonth = Session("SS_CBACCMONTH")
        strAccYear = Session("SS_CBACCYEAR")
        strLangCode = Session("SS_LANGCODE")
        intCBAR = Session("SS_CBAR")
        intConfig = Session("SS_CONFIGSETTING")
        strLocType = Session("SS_LOCTYPE")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCBAccessRights.CBReceipt), intCBAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            onload_GetLangCap()
            lblErrBillParty.Visible = False
            lblErrNoSelectDoc.Visible = False
            lblErrManySelectDoc.Visible = False
            lblErrAmount.Visible = False
            lblErrTotal.Visible = False
            lblErrValidPPNHRate.Visible = False
            lblErrAction.Visible = False
            lblBlockErr.Visible = False
            lblPreBlockErr.Visible = False
            lblVehCodeErr.Visible = False
            lblVehExpCodeErr.Visible = False

            'to avoid double click, on aspx add this : UseSubmitBehavior="false"
            AddBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(AddBtn).ToString())
            ConfirmBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(ConfirmBtn).ToString())

            strSelectedReceiptID = Trim(IIf(Request.QueryString("receiptid") = "", Request.Form("hidReceiptID"), Request.QueryString("receiptid")))
            hidReceiptID.Value = strSelectedReceiptID

            If Not IsPostBack Then
                LblIsSKBActive.Text = 0
                lblSKBStartDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
                BindReceiptTerm()

                If strSelectedReceiptID <> "" Then
                    'BindOtherAccCode("")
                    onLoad_Display(strSelectedReceiptID)
                    onLoad_DisplayLine(strSelectedReceiptID)
                    onLoad_Button()

                    BindChargeLevelDropDownList()
                    BindPreBlock("", "")
                    BindVehicleCodeDropList("")
                    BindVehicleExpDropList(True)
                    BindCurrencyList("")

                Else
                    txtDateCreated.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
                    BindBillParty("")
                    BindRecType("")
                    BindBankCode("")
                    ddlCreditNote.Items.Add(lblPleaseSelect.Text & "Credit Note")
                    ddlCreditNote.Items(0).Value = ""
                    ddlDebtorJrn.Items.Add(lblPleaseSelect.Text & "Debtor Journal")
                    ddlDebtorJrn.Items(0).Value = ""
                    ddlDebitNote.Items.Add(lblPleaseSelect.Text & "Debit Note")
                    ddlDebitNote.Items(0).Value = ""
                    ddlInvoice.Items.Add(lblPleaseSelect.Text & "Invoice")
                    ddlInvoice.Items(0).Value = ""
                    ddlContract.Items.Add(lblPleaseSelect.Text & "Contract No")
                    ddlContract.Items(0).Value = ""
                    'BindAccount("")
                    BindChargeLevelDropDownList()
                    BindPreBlock("", "")
                    BindBlockDropList("")
                    BindVehicleCodeDropList("")
                    BindVehicleExpDropList(True)
                    BindCurrencyList("")
                    BindPPNH("")
                    onLoad_Button()
                    'BindOtherAccCode("")
                    'BindContractNoList("", "")
                    TrLink.Visible = False
                End If
            Else

            End If
        End If
    End Sub

    Sub BindReceiptTerm()
        rblReceiptTerm.Items.Add(New ListItem("Incl. VAT", "1"))
        rblReceiptTerm.Items.Add(New ListItem("Tax Basis Only", "2"))
        rblReceiptTerm.Items.Add(New ListItem("VAT Only", "3"))
        rblReceiptTerm.SelectedIndex = 0
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()

        Try
            If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfig) = True Then
                lblBlkTag.Text = GetCaption(objLangCap.EnumLangCap.Block) & lblCode.Text
                strBlockTag = GetCaption(objLangCap.EnumLangCap.Block)
            Else
                lblBlkTag.Text = GetCaption(objLangCap.EnumLangCap.SubBlock) & lblCode.Text
                strBlockTag = GetCaption(objLangCap.EnumLangCap.SubBlock)
            End If
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_RECEIPTDET_GET_LANGCAP_COSTLEVEL&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/CB_trx_ReeciptList.aspx")
        End Try

        lblBillPartyTag.Text = GetCaption(objLangCap.EnumLangCap.BillParty) & lblCode.Text
        lblAccount.Text = GetCaption(objLangCap.EnumLangCap.Account) & lblCode.Text
        lblAccCodeTag.Text = "Debit " & lblAccount.Text

        dgLineDet.Columns(2).HeaderText = lblAccount.Text
        'dgLineDet.Columns(3).HeaderText = lblBlkTag.Text

        lblErrBillParty.Text = lblPleaseSelect.Text & lblBillPartyTag.Text
        rfvAccCode.ErrorMessage = "<BR>" & lblPleaseSelect.Text & lblAccount.Text


        strVehTag = GetCaption(objLangCap.EnumLangCap.Vehicle) & lblCode.Text
        strVehExpTag = GetCaption(objLangCap.EnumLangCap.VehExpense) & lblCode.Text
        lblBlkTag.Text = strBlockTag & " : "
        lblVehTag.Text = strVehTag & " : "
        lblVehExpTag.Text = strVehExpTag & " :"

        lblVehCodeErr.Text = lblPleaseSelect.Text & strVehTag
        lblBlockErr.Text = lblPleaseSelect.Text & strBlockTag
        lblVehExpCodeErr.Text = lblPleaseSelect.Text & strVehExpTag

        'dgLineDet.Columns(4).HeaderText = strVehTag
        'dgLineDet.Columns(5).HeaderText = strVehExpTag

        strPreBlockTag = GetCaption(objLangCap.EnumLangCap.Block)
        lblPreBlkTag.Text = strPreBlockTag & lblCode.Text & " : "
        lblPreBlockErr.Text = lblPleaseSelect.Text & strPreBlockTag & lblCode.Text


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
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_RECEIPTDET_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/CB_trx_ReceiptList.aspx")
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
        Dim intStatus As Integer

        ddlBillParty.Enabled = False
        ddlRecType.Enabled = False
        ddlBank.Enabled = False

        txtChequeNo.Enabled = False
        txtRemark.Enabled = False
        tblSelection.Visible = False
        SaveBtn.Visible = False
        EditBtn.Visible = True
        DeleteBtn.Visible = False
        ConfirmBtn.Visible = False
        btnFind1.Disabled = True
        lblDateCreated.Visible = False
        txtDateCreated.Enabled = False
        btnDateCreated.Visible = False

        If (hidStatus.Value <> "") Then
            onLoad_DisplayLine(hidReceiptID.Value.Trim)
            intStatus = CInt(hidStatus.Value)
            Select Case intStatus
                Case objCBTrx.EnumReceiptStatus.Active
                    If objReceiptLnDs.Tables(0).Rows.Count = 0 Then
                        ddlRecType.Enabled = True
                        If ddlRecType.SelectedItem.Value = "0" Then
                            ddlBank.Enabled = True
                            txtChequeNo.Enabled = True
                        End If
                        ddlBillParty.Enabled = True
                        txtRemark.Enabled = True
                        tblSelection.Visible = True
                        SaveBtn.Visible = True
                        EditBtn.Visible = False
                        DeleteBtn.Visible = True
                        btnFind1.Disabled = False
                    Else
                        ddlRecType.Enabled = True
                        If ddlRecType.SelectedItem.Value = "0" Then
                            ddlBank.Enabled = True
                            txtChequeNo.Enabled = True
                        End If

                        tblSelection.Visible = True
                        ConfirmBtn.Visible = True
                        EditBtn.Visible = False
                        txtRemark.Enabled = True
                        SaveBtn.Visible = True
                        DeleteBtn.Visible = True
                        btnFind1.Disabled = False
                    End If

                    DeleteBtn.Visible = True
                    DeleteBtn.ImageUrl = "../../images/butt_delete.gif"
                    DeleteBtn.AlternateText = "Delete"
                    DeleteBtn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"

                Case objCBTrx.EnumReceiptStatus.Deleted
                    DeleteBtn.Visible = False
                    'DeleteBtn.ImageUrl = "../../images/butt_undelete.gif"
                    'DeleteBtn.AlternateText = "Undelete"
                    'DeleteBtn.Attributes("onclick") = "javascript:return ConfirmAction('undelete');"
                Case Else
            End Select

            lblDateCreated.Visible = True
        Else
            ddlBillParty.Enabled = True
            ddlRecType.Enabled = True
            If ddlRecType.SelectedItem.Value = "0" Then
                ddlBank.Enabled = True
                txtChequeNo.Enabled = True
            End If
            txtRemark.Enabled = True
            tblSelection.Visible = True
            SaveBtn.Visible = True
            EditBtn.Visible = False
            btnFind1.Disabled = False

            txtDateCreated.Enabled = True
            btnDateCreated.Visible = True
        End If
        If lblReceiptID.Text.Trim() = "" Then
            'txtDateCreated.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
        End If
    End Sub

    Sub onLoad_Display(ByVal pv_strInvoiceId As String)
        Dim strOpCd_Get As String = "CB_CLSTRX_RECEIPT_DETAIL_GET"

        hidReceiptID.Value = pv_strInvoiceId

        strParam = "AND RC.ReceiptID = '" & pv_strInvoiceId & "'"
        Try
            intErrNo = objCBTrx.mtdGetReceipt(strOpCd_Get, _
                                              strCompany, _
                                              strLocation, _
                                              strUserId, _
                                              strAccMonth, _
                                              strAccYear, _
                                              strParam, _
                                              objReceiptDs, _
                                              True)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_RECEIPTDET_GET_HEADER&errmesg=" & Exp.ToString & "&redirect=BI/trx/CB_trx_ReceiptList.aspx")
        End Try

        lblReceiptID.Text = pv_strInvoiceId
        onSelect_BillPartyCode(Trim(objReceiptDs.Tables(0).Rows(0).Item("BillPartyCode")))
        BindRecType(objReceiptDs.Tables(0).Rows(0).Item("ReceiptType").Trim())
        txtChequeNo.Text = Trim(objReceiptDs.Tables(0).Rows(0).Item("ChequeNo"))
        txtRemark.Text = Trim(objReceiptDs.Tables(0).Rows(0).Item("Remark"))

        lblAccPeriod.Text = Trim(objReceiptDs.Tables(0).Rows(0).Item("AccMonth")) & "/" & Trim(objReceiptDs.Tables(0).Rows(0).Item("AccYear"))
        lblStatus.Text = objCBTrx.mtdGetReceiptStatus(Trim(objReceiptDs.Tables(0).Rows(0).Item("Status")))
        intReceiptStatus = CInt(Trim(objReceiptDs.Tables(0).Rows(0).Item("Status")))
        hidStatus.Value = intReceiptStatus
        lblDateCreated.Text = objGlobal.GetLongDate(objReceiptDs.Tables(0).Rows(0).Item("CreateDate"))
        txtDateCreated.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), objReceiptDs.Tables(0).Rows(0).Item("CreateDate"))
        lblLastUpdate.Text = objGlobal.GetLongDate(objReceiptDs.Tables(0).Rows(0).Item("UpdateDate"))
        lblPrintDate.Text = objGlobal.GetLongDate(objReceiptDs.Tables(0).Rows(0).Item("PrintDate"))
        lblUpdatedBy.Text = Trim(objReceiptDs.Tables(0).Rows(0).Item("UserName"))
        lblCurrency.Text = Trim(objReceiptDs.Tables(0).Rows(0).Item("CurrencyCode"))
        BindBankCode(Trim(objReceiptDs.Tables(0).Rows(0).Item("BankCode")))


        If Sign(objReceiptDs.Tables(0).Rows(0).Item("TotalAmount")) = -1 Then
            lblViewTotalAmount.Text = "(" & objGlobal.GetIDDecimalSeparator(FormatNumber(Abs(objReceiptDs.Tables(0).Rows(0).Item("TotalAmountToDisplay")), 0)) & ")"
            lblTotalAmount.Text = "(" & FormatNumber(Abs(objReceiptDs.Tables(0).Rows(0).Item("TotalAmount")), CInt(Session("SS_ROUNDNO"))) & ")"
        Else
            lblViewTotalAmount.Text = objGlobal.GetIDDecimalSeparator(FormatNumber(Trim(objReceiptDs.Tables(0).Rows(0).Item("TotalAmountToDisplay")), 0))
            lblTotalAmount.Text = FormatNumber(Trim(objReceiptDs.Tables(0).Rows(0).Item("TotalAmount")), CInt(Session("SS_ROUNDNO")))
        End If
        hidTotalAmt.Value = objReceiptDs.Tables(0).Rows(0).Item("TotalAmount")

        If Trim(objReceiptDs.Tables(0).Rows(0).Item("PPNInit")) = objCBTrx.EnumPPN.Yes Then
            cbPPN.Checked = True
            cbPPN.Enabled = False
        Else
            cbPPN.Checked = False
            cbPPN.Enabled = False
        End If

        If objReceiptDs.Tables(0).Rows.Count > 0 Then
            TrLink.Visible = True
        Else
            TrLink.Visible = False
        End If
    End Sub


    Sub onLoad_DisplayLine(ByVal pv_strInvoiceId As String)
        Dim lbButton As LinkButton
        Dim lblDocType As Label
        Dim lblAmountCtrl As Label
        Dim strDocType As String
        Dim strInvoice As String = ddlInvoice.SelectedItem.Value.Trim
        Dim strDebitNote As String = ddlDebitNote.SelectedItem.Value.Trim
        Dim strCreditNote As String = ddlCreditNote.SelectedItem.Value.Trim
        Dim strDebtorJrn As String = ddlDebtorJrn.SelectedItem.Value.Trim
        Dim strOther As String = txtAccCodeOth.Text ''ddlOther.SelectedItem.Value
        Dim strAdvReceipt As String = ddlContract.SelectedItem.Value

        Dim intInvoiceRcvInd As Integer
        Dim intDebitNoteInd As Integer
        Dim intCreditNoteInd As Integer
        Dim intDebtorJrnInd As Integer
        Dim intOther As Integer
        Dim intAdvReceipt As Integer

        intInvoiceRcvInd = IIf(strInvoice = "", 0, 1)
        intDebitNoteInd = IIf(strDebitNote = "", 0, 1)
        intCreditNoteInd = IIf(strCreditNote = "", 0, 1)
        intDebtorJrnInd = IIf(strDebtorJrn = "", 0, 1)
        intOther = IIf(strOther = "", 0, 1)
        intAdvReceipt = IIf(strAdvReceipt = "", 0, 1)

        If (intInvoiceRcvInd + intDebitNoteInd + intCreditNoteInd + intDebtorJrnInd + intOther + intAdvReceipt) = 0 Then
            'cbPPN.Checked = False
            'cbPPN.Enabled = True
            txtPPHRate.Text = "0"
            txtPPHRate.Enabled = True
        End If

        strParam = pv_strInvoiceId
        Try
            intErrNo = objCBTrx.mtdGetReceiptLine(strOpCd_GetLine, strParam, objReceiptLnDs)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_RECEIPTDET_GET_LINE&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/CB_trx_ReceiptList.aspx")
        End Try

        For intCnt = 0 To objReceiptLnDs.Tables(0).Rows.Count - 1
            objReceiptLnDs.Tables(0).Rows(intCnt).Item("ReceiptLnID") = Trim(objReceiptLnDs.Tables(0).Rows(intCnt).Item("ReceiptLnID"))
            objReceiptLnDs.Tables(0).Rows(intCnt).Item("DocID") = Trim(objReceiptLnDs.Tables(0).Rows(intCnt).Item("DocID"))
            objReceiptLnDs.Tables(0).Rows(intCnt).Item("DocType") = Trim(objReceiptLnDs.Tables(0).Rows(intCnt).Item("DocType"))
            objReceiptLnDs.Tables(0).Rows(intCnt).Item("AccCode") = Trim(objReceiptLnDs.Tables(0).Rows(intCnt).Item("AccCode"))
            objReceiptLnDs.Tables(0).Rows(intCnt).Item("BlkCode") = Trim(objReceiptLnDs.Tables(0).Rows(intCnt).Item("BlkCode"))
            objReceiptLnDs.Tables(0).Rows(intCnt).Item("VehCode") = Trim(objReceiptLnDs.Tables(0).Rows(intCnt).Item("VehCode"))
            objReceiptLnDs.Tables(0).Rows(intCnt).Item("VehExpCode") = Trim(objReceiptLnDs.Tables(0).Rows(intCnt).Item("VehExpCode"))

            lblPPHRateHidden.Text = objReceiptLnDs.Tables(0).Rows(intCnt).Item("PPHRate")
            lblPPNHidden.Text = objReceiptLnDs.Tables(0).Rows(0).Item("PPN")
        Next intCnt

        If objReceiptLnDs.Tables(0).Rows.Count = 0 Then
            lblPPHRateHidden.Text = ""
            lblPPNHidden.Text = ""
        End If

        dgLineDet.DataSource = objReceiptLnDs.Tables(0)
        dgLineDet.DataBind()

        For intCnt = 0 To objReceiptLnDs.Tables(0).Rows.Count - 1
            Select Case intReceiptStatus
                Case objCBTrx.EnumReceiptStatus.Active
                    lbButton = dgLineDet.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = True
                    lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                Case Else
                    lbButton = dgLineDet.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False
            End Select

            lblDocType = dgLineDet.Items.Item(intCnt).FindControl("lblDocType")
            strDocType = lblDocType.Text
            Select Case strDocType
                Case objCBTrx.EnumReceiptDocType.ContractInvoice 'objGlobal.EnumDocType.ContractInvoice
                    lblDocType.Text = objCBTrx.mtdGetReceiptDocType(objCBTrx.EnumReceiptDocType.ContractInvoice) '  objGlobal.mtdGetDocName(objGlobal.EnumDocType.ContractInvoice)
                Case objCBTrx.EnumReceiptDocType.DebitNote 'objGlobal.EnumDocType.BIDebitNote
                    lblDocType.Text = objCBTrx.mtdGetReceiptDocType(objCBTrx.EnumReceiptDocType.CreditNote) ' objGlobal.mtdGetDocName(objGlobal.EnumDocType.BIDebitNote)
                Case objCBTrx.EnumReceiptDocType.CreditNote 'objGlobal.EnumDocType.BICreditNote
                    lblDocType.Text = objCBTrx.mtdGetReceiptDocType(objCBTrx.EnumReceiptDocType.CreditNote) ' objGlobal.mtdGetDocName(objGlobal.EnumDocType.BICreditNote)
                    lblAmountCtrl = dgLineDet.Items.Item(intCnt).FindControl("lblAmount")
                    lblAmountCtrl.Text = "(" & lblAmountCtrl.Text & ")"
                Case objCBTrx.EnumReceiptDocType.DebtorJournal 'objGlobal.EnumDocType.ARDebtorJournal
                    lblDocType.Text = objCBTrx.mtdGetReceiptDocType(objCBTrx.EnumReceiptDocType.DebtorJournal) ' objGlobal.mtdGetDocName(objGlobal.EnumDocType.ARDebtorJournal)
                Case objCBTrx.EnumReceiptDocType.Receipt
                    lblDocType.Text = objCBTrx.mtdGetReceiptDocType(objCBTrx.EnumReceiptDocType.Receipt)
                Case objCBTrx.EnumReceiptDocType.AdvReceipt 'objGlobal.EnumDocType.ARAdvancePayment
                    lblDocType.Text = objCBTrx.mtdGetReceiptDocType(objCBTrx.EnumReceiptDocType.AdvReceipt)
                    'lblDocType.Text = "Advance Receipt"
            End Select
        Next
    End Sub


    Sub BindBillParty(ByVal pv_strCode As String)
        Dim strOpCd As String = "GL_CLSSETUP_BILLPARTY_GET"
        Dim intSelectedIndex As Integer = 0

        Dim strParam As String = "||1||BP.Name|ASC|"

        Try
            intErrNo = objGLSetup.mtdGetBillParty(strOpCd, strParam, objBPDs)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_RECEIPTDET_BINDBILLPARTY_GET&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/CB_trx_ReceiptList.aspx")
        End Try

        For intCnt = 0 To objBPDs.Tables(0).Rows.Count - 1
            objBPDs.Tables(0).Rows(intCnt).Item("BillPartyCode") = Trim(objBPDs.Tables(0).Rows(intCnt).Item("BillPartyCode"))
            'objBPDs.Tables(0).Rows(intCnt).Item("Name") = Trim(objBPDs.Tables(0).Rows(intCnt).Item("BillPartyCode")) & " (" & Trim(objBPDs.Tables(0).Rows(intCnt).Item("Name")) & ")"
            objBPDs.Tables(0).Rows(intCnt).Item("Name") = Trim(objBPDs.Tables(0).Rows(intCnt).Item("Name")) & "(" & Trim(objBPDs.Tables(0).Rows(intCnt).Item("BillPartyCode")) & ")"
            If objBPDs.Tables(0).Rows(intCnt).Item("BillPartyCode") = Trim(pv_strCode) Then
                intSelectedIndex = intCnt + 1
            End If
        Next

        dr = objBPDs.Tables(0).NewRow()
        dr("BillPartyCode") = ""
        dr("Name") = lblPleaseSelect.Text & lblBillPartyTag.Text
        objBPDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlBillParty.DataSource = objBPDs.Tables(0)
        ddlBillParty.DataValueField = "BillPartyCode"
        ddlBillParty.DataTextField = "Name"
        ddlBillParty.DataBind()
        ddlBillParty.SelectedIndex = intSelectedIndex
    End Sub


    Sub BindInvoice(ByVal pv_strCode As String)
        Dim strOpCd As String = "CB_CLSTRX_RECEIPT_INVOICE_GET"

        strParam = pv_strCode & "|" & objCBTrx.EnumInvoiceStatus.Confirmed & "','" & objCBTrx.EnumInvoiceStatus.Closed & "|"
        Try
            intErrNo = objCBTrx.mtdGetReceipt_InvDNCN(strOpCd, strParam, strLocation, objInvDs)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_RECEIPTDET_BINDINVOICE_GET&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/CB_trx_ReceiptList.aspx")
        End Try

        For intCnt = 0 To objInvDs.Tables(0).Rows.Count - 1
            objInvDs.Tables(0).Rows(intCnt).Item("InvoiceID") = Trim(objInvDs.Tables(0).Rows(intCnt).Item("InvoiceID"))
            objInvDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objInvDs.Tables(0).Rows(intCnt).Item("InvoiceID")) & ", " & objInvDs.Tables(0).Rows(intCnt).Item("CurrencyCode") & " " & Trim(objInvDs.Tables(0).Rows(intCnt).Item("OutstandingAmountToDisplay")) & ", " & objInvDs.Tables(0).Rows(intCnt).Item("Kurs") & ", " & objInvDs.Tables(0).Rows(intCnt).Item("ContractNo")
        Next

        dr = objInvDs.Tables(0).NewRow()
        dr("InvoiceID") = ""
        dr("Description") = lblPleaseSelect.Text & "Invoice"
        objInvDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlInvoice.DataSource = objInvDs.Tables(0)
        ddlInvoice.DataValueField = "InvoiceID"
        ddlInvoice.DataTextField = "Description"
        ddlInvoice.DataBind()

    End Sub

    Sub BindDebitNote(ByVal pv_strCode As String)
        Dim strOpCd As String = "CB_CLSTRX_RECEIPT_DEBITNOTE_GET"

        strParam = pv_strCode & "|" & objCBTrx.EnumDebitNoteStatus.Confirmed & "','" & objCBTrx.EnumDebitNoteStatus.Closed & "|"
        Try
            intErrNo = objCBTrx.mtdGetReceipt_InvDNCN(strOpCd, strParam, strLocation, objDNDs)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_RECEIPT_BINDDEBITNOTE_GET&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/CB_trx_ReceiptList.aspx")
        End Try

        For intCnt = 0 To objDNDs.Tables(0).Rows.Count - 1
            objDNDs.Tables(0).Rows(intCnt).Item("DebitNoteID") = Trim(objDNDs.Tables(0).Rows(intCnt).Item("DebitNoteID"))
            objDNDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objDNDs.Tables(0).Rows(intCnt).Item("DebitNoteID")) & ", Rp " & Trim(objDNDs.Tables(0).Rows(intCnt).Item("OutstandingAmount"))
        Next

        dr = objDNDs.Tables(0).NewRow()
        dr("DebitNoteID") = ""
        dr("Description") = lblPleaseSelect.Text & "Debit Note"
        objDNDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlDebitNote.DataSource = objDNDs.Tables(0)
        ddlDebitNote.DataValueField = "DebitNoteID"
        ddlDebitNote.DataTextField = "Description"
        ddlDebitNote.DataBind()
    End Sub

    Sub BindCreditNote(ByVal pv_strCode As String)
        Dim strOpCd As String = "CB_CLSTRX_RECEIPT_CREDITNOTE_GET"

        strParam = pv_strCode & "|" & objCBTrx.EnumCreditNoteStatus.Confirmed & "','" & objCBTrx.EnumCreditNoteStatus.Closed & "|"
        Try
            intErrNo = objCBTrx.mtdGetReceipt_InvDNCN(strOpCd, strParam, strLocation, objCNDs)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_RECEIPT_BINDCREDITNOTE_GET&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/CB_trx_ReceiptList.aspx")
        End Try

        For intCnt = 0 To objCNDs.Tables(0).Rows.Count - 1
            objCNDs.Tables(0).Rows(intCnt).Item("CreditNoteID") = Trim(objCNDs.Tables(0).Rows(intCnt).Item("CreditNoteID"))
            objCNDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objCNDs.Tables(0).Rows(intCnt).Item("CreditNoteID")) & ", Rp " & Trim(objCNDs.Tables(0).Rows(intCnt).Item("OutstandingAmount"))
        Next


        dr = objCNDs.Tables(0).NewRow()
        dr("CreditNoteID") = ""
        dr("Description") = lblPleaseSelect.Text & "Credit Note"
        objCNDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlCreditNote.DataSource = objCNDs.Tables(0)
        ddlCreditNote.DataValueField = "CreditNoteID"
        ddlCreditNote.DataTextField = "Description"
        ddlCreditNote.DataBind()
    End Sub

    Sub BindDebtorJournal(ByVal pv_strCode As String)
        Dim strOpCd As String = "CB_CLSTRX_RECEIPT_DEBTORJOURNAL_GET"

        strParam = pv_strCode & "|" & objCBTrx.EnumDebtorJournalStatus.Confirmed & "','" & objCBTrx.EnumDebtorJournalStatus.Closed & "|"
        Try
            intErrNo = objCBTrx.mtdGetReceipt_InvDNCN(strOpCd, strParam, strLocation, objDJDs)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_RECEIPTDET_BINDDEBTORJRN_GET&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/CB_trx_ReceiptList.aspx")
        End Try

        For intCnt = 0 To objDJDs.Tables(0).Rows.Count - 1
            objDJDs.Tables(0).Rows(intCnt).Item("DebtorJrnID") = Trim(objDJDs.Tables(0).Rows(intCnt).Item("DebtorJrnID"))
            objDJDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objDJDs.Tables(0).Rows(intCnt).Item("DebtorJrnID")) & ", Rp " & objDJDs.Tables(0).Rows(intCnt).Item("OutstandingAmount")
        Next

        dr = objDJDs.Tables(0).NewRow()
        dr("DebtorJrnID") = ""
        dr("Description") = lblPleaseSelect.Text & "Debtor Journal"
        objDJDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlDebtorJrn.DataSource = objDJDs.Tables(0)
        ddlDebtorJrn.DataValueField = "DebtorJrnID"
        ddlDebtorJrn.DataTextField = "Description"
        ddlDebtorJrn.DataBind()
    End Sub

    Sub BindPPNH(ByVal pv_strSelectedInvRcv As String)
        Dim strOpCd_GetPPNH As String = "BI_CLSTRX_INVOICE_LINE_GET"
        Dim strParam As String = pv_strSelectedInvRcv
        Dim objPPNHDs As New Object()
        Dim intErrNo As Integer

        If Trim(pv_strSelectedInvRcv) = "" Then
            Exit Sub
        End If

        Try
            intErrNo = objBITrx.mtdGetInvoiceLine(strOpCd_GetPPNH, strParam, objPPNHDs)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_DEBITNOTEDET_GET_LINE&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/CB_trx_DNList.aspx")
        End Try

        If objPPNHDs.Tables(0).Rows.Count > 0 Then
            txtPPHRate.Text = Trim(objPPNHDs.Tables(0).Rows(0).Item("PPHRate"))
            cbPPN.Checked = IIf(objPPNHDs.Tables(0).Rows(0).Item("PPN") = objCBTrx.EnumPPN.Yes, True, False)
        End If
        cbPPN.Enabled = False
        txtPPHRate.Enabled = False
    End Sub

    Sub BindDCNPPNH(ByVal pv_strSelectedDoc As String)
        cbPPN.Checked = False
        cbPPN.Enabled = False
        txtPPHRate.Text = "0"
        txtPPHRate.Enabled = False
        lblPPHRateHidden.Text = ""
        lblPPNHidden.Text = ""
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

        Dim strInvoice As String = ddlInvoice.SelectedItem.Value.Trim
        Dim strDebitNote As String = ddlDebitNote.SelectedItem.Value.Trim
        Dim strCreditNote As String = ddlCreditNote.SelectedItem.Value.Trim
        Dim strDebtorJrn As String = ddlDebtorJrn.SelectedItem.Value.Trim
        Dim strOther As String = txtAccCodeOth.Text.Trim 'ddlOther.SelectedItem.Value
        Dim strAdvReceipt As String = ddlContract.SelectedItem.Value

        Dim intInvoiceRcvInd As Integer
        Dim intDebitNoteInd As Integer
        Dim intCreditNoteInd As Integer
        Dim intDebtorJrnInd As Integer
        Dim intOther As Integer
        Dim intAdvReceipt As Integer

        intInvoiceRcvInd = IIf(strInvoice = "", 0, 1)
        intDebitNoteInd = IIf(strDebitNote = "", 0, 1)
        intCreditNoteInd = IIf(strCreditNote = "", 0, 1)
        intDebtorJrnInd = IIf(strDebtorJrn = "", 0, 1)
        intOther = IIf(strOther = "", 0, 1)
        intAdvReceipt = IIf(strAdvReceipt = "", 0, 1)

        If (intInvoiceRcvInd + intDebitNoteInd + intCreditNoteInd + intDebtorJrnInd + intOther + intAdvReceipt) = 0 Then
            'cbPPN.Enabled = True
            txtPPHRate.Enabled = True
        End If


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


    Sub BindAccount(ByVal pv_strAccCode As String)
        'Dim strOpCd As String = "GL_CLSSETUP_ACCOUNTCODE_LIST_GET"


        'strParam = "Order By ACC.AccCode|AND ACC.Status = '" & objGLSetup.EnumAccountCodeStatus.Active & "' " & vbCrLf & _
        '           "AND ACC.AccType IN ('" & objGLSetup.EnumAccountType.BalanceSheet & "','" & objGLSetup.EnumAccountType.ProfitAndLost & "')"

        'strParam = strParam & IIf(Session("SS_COACENTRALIZED") = "1", "", " AND LocCode = '" & Trim(strLocation) & "' ")

        'Try
        '    intErrNo = objGLSetup.mtdGetMasterList(strOpCd, _
        '                                           strParam, _
        '                                           objGLSetup.EnumGLMasterType.AccountCode, _
        '                                           objAccDs)
        'Catch Exp As Exception
        '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_RECEIPTDET_BindAccount_GET&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/CB_trx_ReceiptList.aspx")
        'End Try

        'For intCnt = 0 To objAccDs.Tables(0).Rows.Count - 1
        '    objAccDs.Tables(0).Rows(intCnt).Item("AccCode") = Trim(objAccDs.Tables(0).Rows(intCnt).Item("AccCode"))
        '    objAccDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objAccDs.Tables(0).Rows(intCnt).Item("AccCode")) & " (" & Trim(objAccDs.Tables(0).Rows(intCnt).Item("Description")) & ")"
        'Next

        'dr = objAccDs.Tables(0).NewRow()
        'dr("AccCode") = ""
        'dr("Description") = lblPleaseSelect.Text & lblAccount.Text
        'objAccDs.Tables(0).Rows.InsertAt(dr, 0)

        'ddlAccount.DataSource = objAccDs.Tables(0)
        'ddlAccount.DataValueField = "AccCode"
        'ddlAccount.DataTextField = "Description"
        'ddlAccount.DataBind()

    End Sub

    Sub onSelect_BillParty(ByVal Sender As Object, ByVal E As EventArgs)
        onSelect_BillPartyCode(ddlBillParty.SelectedItem.Value.Trim)
    End Sub

    Sub onSelect_BillPartyCode(ByVal pv_strCode As String)
        Dim strOpCd_Get As String = "CB_CLSTRX_RECEIPT_GET"

        If strSelectedReceiptID = "" Then
            BindInvoice(ddlBillParty.SelectedItem.Value.Trim)
            BindDebitNote(ddlBillParty.SelectedItem.Value.Trim)
            BindCreditNote(ddlBillParty.SelectedItem.Value.Trim)
            BindDebtorJournal(ddlBillParty.SelectedItem.Value.Trim)
            BindContractNoList(ddlBillParty.SelectedItem.Value.Trim, "")
        Else
            BindBillParty(pv_strCode)
            BindInvoice(pv_strCode)
            BindDebitNote(pv_strCode)
            BindCreditNote(pv_strCode)
            BindDebtorJournal(pv_strCode)
            BindAccount(pv_strCode)
            BindBlockDropList(pv_strCode, "")
            BindPPNH(pv_strCode)
            BindContractNoList(pv_strCode, "")
        End If
    End Sub

    Sub onSelect_InvRcv(ByVal sender As System.Object, ByVal e As System.EventArgs)
        BindPPNH(ddlInvoice.SelectedItem.Value)
        CheckAmount(ddlInvoice.SelectedItem.Text)
        BindCurrencyList(pv_strCurrencyCode)
    End Sub

    Sub onSelect_DN(ByVal sender As System.Object, ByVal e As System.EventArgs)
        BindDCNPPNH(ddlDebitNote.SelectedItem.Value)
        CheckAmount(ddlDebitNote.SelectedItem.Text)
    End Sub

    Sub onSelect_CN(ByVal sender As System.Object, ByVal e As System.EventArgs)
        BindDCNPPNH(ddlCreditNote.SelectedItem.Value)
        CheckAmount(ddlCreditNote.SelectedItem.Text)
    End Sub

    Sub onSelect_DJ(ByVal sender As System.Object, ByVal e As System.EventArgs)
        BindDCNPPNH(ddlDebtorJrn.SelectedItem.Value)
        CheckAmount(ddlDebtorJrn.SelectedItem.Text)
    End Sub

    Sub onSelect_StrAccCodeOther(ByVal sender As System.Object, ByVal e As System.EventArgs)
        GetCOADetailOther(txtAccCodeOth.Text.Trim)
    End Sub

    Sub GetCOADetailOther(ByVal pv_strCode As String)
        Dim dr As DataRow
        Dim intCnt As Integer = 0
        Dim intErrNo As Integer
        Dim intSelectedIndex As Integer = 0
        Dim strOpCode As String = "GL_CLSSETUP_ACCOUNTCODE_LIST_GET"
        Dim objCOADs As New DataSet
        Dim strParamName As String = ""
        Dim strParamValue As String = ""

        Dim strDate As String = Date_Validation(txtDateCreated.Text, False)


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
            txtAccCodeOth.Text = objCOADs.Tables(0).Rows(0).Item("AccCode")
            txtAccNameOth.Text = objCOADs.Tables(0).Rows(0).Item("Description")
        Else
            txtAccCodeOth.Text = ""
            txtAccNameOth.Text = ""
        End If

    End Sub

    Sub onSelect_StrAccCode(ByVal sender As System.Object, ByVal e As System.EventArgs)
        GetCOADetail(txtAccCode.Text.Trim)
    End Sub

    Sub GetCOADetail(ByVal pv_strCode As String)
        Dim dr As DataRow
        Dim intCnt As Integer = 0
        Dim intErrNo As Integer
        Dim intSelectedIndex As Integer = 0
        Dim strOpCode As String = "GL_CLSSETUP_ACCOUNTCODE_LIST_GET"
        Dim objCOADs As New DataSet
        Dim strParamName As String = ""
        Dim strParamValue As String = ""

        Dim strDate As String = Date_Validation(txtDateCreated.Text, False)


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
            txtAccCode.Text = objCOADs.Tables(0).Rows(0).Item("AccCode")
            txtAccName.Text = objCOADs.Tables(0).Rows(0).Item("Description")
        Else
            txtAccCode.Text = ""
            txtAccName.Text = ""
        End If

    End Sub

    Sub Update_Receipt(ByVal pv_intStatus As Integer, ByRef pr_objNewID As String, ByRef pr_intSuccess As Integer)

        Dim strOpCd_Add As String = "CB_CLSTRX_RECEIPT_ADD"
        Dim strOpCd_Upd As String = "CB_CLSTRX_RECEIPT_UPD"
        Dim strOpCodes As String = strOpCd_Add & "|" & strOpCd_Upd
        Dim strDate As String = Date_Validation(txtDateCreated.Text, False)
        Dim strNewIDFormat As String
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

        pr_intSuccess = 1

        If ddlBillParty.SelectedItem.Value = "" Then
            lblErrBillParty.Visible = True
            pr_intSuccess = 0
            Exit Sub
        End If

        If strSelectedReceiptID = "" Then
            strSelectedReceiptID = pr_objNewID
        End If
        lblDate.Visible = False
        lblFmt.Visible = False
        'If strSelectedReceiptID = "" Then
        '    If txtDateCreated.Text.Trim() = "" Then
        '        lblFmt.Text = "Please enter Date Created"
        '        lblFmt.Visible = True
        '        pr_intSuccess = 0
        '        Exit Sub
        '    ElseIf CheckDate(txtDateCreated.Text.Trim(), strDate) = False Then
        '        lblDate.Visible = True
        '        lblFmt.Visible = True
        '        pr_intSuccess = 0
        '        Exit Sub
        '    End If
        'End If

        If Month(strDate) < strAccMonth And Year(strDate) <= strAccYear Then
            lblDate.Visible = True
            lblDate.Text = "Invalid transaction date."
            Exit Sub
        End If


        Select Case ddlRecType.SelectedItem.Value
            Case 0
                strInitial = "BBT"
            Case 1
                strInitial = "KKT"
            Case 2
                strInitial = "PPL"
        End Select


        'Select Case ddlRecType.SelectedItem.Value
        '    Case 0
        '        strInitial = Trim(ddlBank.SelectedItem.Value)
        '    Case 1
        '        strInitial = "KKL"
        '    Case 2
        '        strInitial = "OTH"
        'End Select

        strAccYear = Year(strDate)
        strAccMonth = Month(strDate)
        'Select Case strCompany
        '    Case "SAM", "MIL"
        '        strNewIDFormat = objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.ARReceipt) & Right(strAccYear, CInt(Session("SS_ROUNDNO"))) & IIf(Len(Trim(strAccMonth)) = 1, "0" & strAccMonth, strAccMonth)
        '    Case Else
        '        strNewIDFormat = strInitial & "/" & strCompany & "/" & strLocation & "/" & "I" & "/" & strAccYear & IIf(Len(Trim(strAccMonth)) = 1, "0" & strAccMonth, strAccMonth) & "/"
        'End Select

        strNewIDFormat = strInitial & "/" & strCompany & "/" & strLocation & "/" & IIf(Len(Trim(strAccMonth)) = 1, "0" & strAccMonth, strAccMonth) & "/" & Trim(strAccYear) & "/"

        strParam = objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.ARReceipt) & "|" & _
                    strSelectedReceiptID & "|" & _
                    ddlBillParty.SelectedItem.Value.Trim & "|" & _
                    strBank & "|" & _
                    txtChequeNo.Text.Trim & "|" & _
                    txtRemark.Text.Trim & "|" & _
                    pv_intStatus & "|" & _
                    strDate & "|" & _
                    ddlRecType.SelectedItem.Value & "|" & _
                    strNewIDFormat & "|" & _
                    strBankAccNo

        Try
            intErrNo = objCBTrx.mtdUpdReceipt(strOpCodes, _
                                              strCompany, _
                                              strLocation, _
                                              strUserId, _
                                              strAccMonth, _
                                              strAccYear, _
                                              strParam, _
                                              pr_objNewID)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_RECEIPTDET_UPD_HEADER&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/CB_trx_ReceiptList.aspx")
        End Try

        pr_objNewID = IIf(strSelectedReceiptID = "", pr_objNewID, strSelectedReceiptID)

        If pr_objNewID <> "" And strBank <> "" Then
            Dim strParamName As String = ""
            Dim strParamValue As String = ""
            Dim strOpCode As String = "CB_CLSTRX_RECEIPT_UPDATE_LINE"

            strParamName = "BANKCODE|BANKACCNO|RECEIPTID"
            strParamValue = strBank & "|" & strBankAccNo & "|" & Trim(pr_objNewID)

            Try
                intErrNo = objGLtrx.mtdInsertDataCommon(strOpCode, _
                          strParamName, _
                          strParamValue)

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_RECEIPTUPDATE_TRXID&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_ReceiptDet.aspx")
            End Try

        End If
    End Sub


    Sub AddBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strBillParty As String = ddlBillParty.SelectedItem.Value.Trim
        Dim strInvoice As String = ddlInvoice.SelectedItem.Value.Trim
        Dim strDebitNote As String = ddlDebitNote.SelectedItem.Value.Trim
        Dim strCreditNote As String = ddlCreditNote.SelectedItem.Value.Trim
        Dim strDebtorJrn As String = ddlDebtorJrn.SelectedItem.Value.Trim
        Dim strAccCode As String = txtAccCode.Text.Trim ''ddlAccount.SelectedItem.Value.Trim
        Dim strOther As String = txtAccCodeOth.Text.Trim ''ddlOther.SelectedItem.Value
        Dim strAdvReceipt As String = ddlContract.SelectedItem.Value

        Dim strBlkCode As String
        If ddlChargeLevel.SelectedIndex = 0 Then
            strBlkCode = Request.Form("ddlPreBlock")
        Else
            strBlkCode = Request.Form("lstBlock")
        End If
        Dim strVehCode As String = Request.Form("lstVehCode")
        Dim strVehExp As String = Request.Form("lstVehExp")

        Dim strOpCodeGLSubBlkByBlk As String = "GL_CLSSETUP_SUBBLOCK_BY_BLOCK_GET"
        Dim strParamList As String

        Dim strOpCode_AddLine As String = "CB_CLSTRX_RECEIPT_LINE_ADD"
        Dim objReceiptID As String
        Dim strDocType As String
        Dim strDocID As String
        Dim strOpCodes As String
        Dim intSuccess As Integer
        Dim intError As Integer
        'If txtAmount.Text.Trim = 0 Then
        '    lblErrAmount.Visible = True
        '    Exit Sub
        'End If
        Dim intInvoiceRcvInd As Integer
        Dim intDebitNoteInd As Integer
        Dim intCreditNoteInd As Integer
        Dim intDebtorJrnInd As Integer
        Dim intOther As Integer
        Dim intAdvReceipt As Integer

        Dim blnIsBalanceSheet As Boolean
        Dim blnIsNurseryInd As Boolean
        Dim blnIsBlockRequire As Boolean
        Dim blnIsVehicleRequire As Boolean
        Dim blnIsOthers As Boolean
        Dim intAmount As Integer
        Dim intPPN As Integer
        Dim intPPHRate As Integer
        Dim intPPNRate As Integer
        Dim intPPNAmount As Double
        Dim intPPHAmount As Double
        Dim intNetAmount As Double
        Dim dblPPHRate As Double
        Dim dblPPNAmount As Double
        Dim dblPPHAmount As Double
        Dim dblNetAmount As Double
        Dim dblAmount As Double
        Dim strDate As String = Date_Validation(txtDateCreated.Text, False)
        Dim strReceiptTerm As String = rblReceiptTerm.SelectedValue

        strAccYear = Year(strDate)
        strAccMonth = Month(strDate)

        intInvoiceRcvInd = IIf(strInvoice = "", 0, 1)
        intDebitNoteInd = IIf(strDebitNote = "", 0, 1)
        intCreditNoteInd = IIf(strCreditNote = "", 0, 1)
        intDebtorJrnInd = IIf(strDebtorJrn = "", 0, 1)
        intOther = IIf(strOther = "", 0, 1)
        intAdvReceipt = IIf(strAdvReceipt = "", 0, 1)

        If (intInvoiceRcvInd + intDebitNoteInd + intCreditNoteInd + intDebtorJrnInd + intOther + intAdvReceipt) > 1 Then
            lblErrManySelectDoc.Visible = True
            BindInvoice(ddlBillParty.SelectedItem.Value)
            BindDebitNote(ddlBillParty.SelectedItem.Value)
            BindCreditNote(ddlBillParty.SelectedItem.Value)
            BindDebtorJournal(ddlBillParty.SelectedItem.Value)
            'BindAccount(ddlAccount.SelectedItem.Value)

            BindBlockDropList(txtAccCode.Text.Trim, lstBlock.SelectedItem.Value)
            BindContractNoList(ddlBillParty.SelectedItem.Value, ddlContract.SelectedItem.Value)

            onLoad_Button()

            Exit Sub
        End If

        If hidCOATax.Value = 1 Then
            Dim dblTotalDPP As Double = 0

            TaxLnID = lstTaxObject.SelectedItem.Value
            TaxRate = hidTaxObjectRate.Value

            If txtDPPAmount.Text = "" Then
                If TaxLnID = "" Then
                    lblTaxObjectErr.Visible = True
                    lblTaxObjectErr.Text = "Please select Tax Object"
                    Exit Sub
                Else
                    lblErrAmountDPP.Visible = True
                    Exit Sub
                End If
            End If

            If txtDPPAmount.Text <> "" Then
                If TaxLnID = "" Then
                    lblTaxObjectErr.Visible = True
                    lblTaxObjectErr.Text = "Please select Tax Object"
                    Exit Sub
                Else
                End If
            End If
            If txtDPPAmount.Text <> "" Then
                DPPAmount = CDbl(txtDPPAmount.Text)
            End If
        Else
            TaxLnID = ""
            TaxRate = 0
            DPPAmount = 0
        End If

        If strDebitNote <> "" Or strCreditNote <> "" Or strDebtorJrn <> "" Then
            intPPHRate = 0
            intPPN = 0
        Else
            intPPN = IIf(cbPPN.Checked = True, objCBTrx.EnumPPN.Yes, objCBTrx.EnumPPN.No)
            intPPNRate = IIf(cbPPN.Checked = True, Session("SS_PPNRATE"), 0)
            intPPHRate = IIf(txtPPHRate.Text <> "", txtPPHRate.Text, "0")
        End If

        If lblPPHRateHidden.Text <> "" And lblPPNHidden.Text <> "" Then
            If lblPPHRateHidden.Text <> intPPHRate Or lblPPNHidden.Text <> intPPN Then
                lblErrValidPPNHRate.Visible = True
                Exit Sub
            End If
        End If

        If CheckRequiredField() = True Then
            onLoad_Button()
            Exit Sub
        End If

        If strInvoice <> "" Then
            If strDebitNote = "" Or strCreditNote = "" Or strDebtorJrn = "" Or strOther = "" Or strAdvReceipt = "" Then
                strDocType = objCBTrx.EnumReceiptDocType.ContractInvoice ' objGlobal.EnumDocType.ContractInvoice
                strDocID = ddlInvoice.SelectedItem.Value.Trim
                strOpCodes = strOpCode_AddLine & "|" & strOpCode_Invoice_OutstdAmount_Get & "|" & strOpCode_Invoice_Upd & "|" & strOpCode_Receipt_Upd
            End If
        ElseIf strDebitNote <> "" Then
            If strInvoice = "" Or strCreditNote = "" Or strDebtorJrn = "" Or strOther = "" Or strAdvReceipt = "" Then
                strDocType = objCBTrx.EnumReceiptDocType.DebitNote 'objGlobal.EnumDocType.BIDebitNote
                strDocID = ddlDebitNote.SelectedItem.Value.Trim
                strOpCodes = strOpCode_AddLine & "|" & strOpCode_DebitNote_OutstdAmount_Get & "|" & strOpCode_DebitNote_Upd & "|" & strOpCode_Receipt_Upd
            End If
        ElseIf strCreditNote <> "" Then
            If strInvoice = "" Or strDebitNote = "" Or strDebtorJrn = "" Or strOther = "" Or strAdvReceipt = "" Then
                strDocType = objCBTrx.EnumReceiptDocType.CreditNote 'objGlobal.EnumDocType.BICreditNote
                strDocID = ddlCreditNote.SelectedItem.Value.Trim
                strOpCodes = strOpCode_AddLine & "|" & strOpCode_CreditNote_OutstdAmount_Get & "|" & strOpCode_CreditNote_Upd & "|" & strOpCode_Receipt_Upd
            End If
        ElseIf strDebtorJrn <> "" Then
            If strInvoice = "" Or strDebitNote = "" Or strCreditNote = "" Or strOther = "" Or strAdvReceipt = "" Then
                strDocType = objCBTrx.EnumReceiptDocType.DebtorJournal 'objGlobal.EnumDocType.ARDebtorJournal
                strDocID = ddlDebtorJrn.SelectedItem.Value.Trim
                strOpCodes = strOpCode_AddLine & "|" & strOpCode_DebtorJrn_OutstdAmount_Get & "|" & strOpCode_DebtorJrn_Upd & "|" & strOpCode_Receipt_Upd
            End If
        ElseIf strOther <> "" Then
            If strInvoice = "" Or strDebitNote = "" Or strCreditNote = "" Or strDebtorJrn = "" Or strAdvReceipt = "" Then
                strDocType = objCBTrx.EnumReceiptDocType.Receipt 'objGlobal.EnumDocType.ARDebtorJournal
                strDocID = txtAccCodeOth.Text.Trim ''ddlOther.SelectedItem.Value.Trim
                strOpCodes = strOpCode_AddLine & "|||" & strOpCode_Receipt_Upd
            End If
        ElseIf strAdvReceipt <> "" Then
            If strInvoice = "" Or strDebitNote = "" Or strCreditNote = "" Or strDebtorJrn = "" Or strOther = "" Then
                strDocType = objCBTrx.EnumReceiptDocType.AdvReceipt 'objGlobal.EnumDocType.ARDebtorJournal
                strDocID = ddlContract.SelectedItem.Value.Trim
                strOpCodes = strOpCode_AddLine & "|" & strOpCode_DebtorJrn_OutstdAmount_Get & "|" & strOpCode_DebtorJrn_Upd & "|" & strOpCode_Receipt_Upd
            End If
        End If

        'If strInvoice = "" And strDebitNote = "" And strCreditNote = "" And strDebtorJrn = "" Then
        '    strDocType = objGlobal.EnumDocType.ARAdvancePayment
        '    strDocID = ""
        '    strOpCodes = strOpCode_AddLine & "|||" & strOpCode_Receipt_Upd
        'End If

        If lblReceiptID.Text = "" Then

            Update_Receipt(objCBTrx.EnumReceiptStatus.Active, objReceiptID, intSuccess)
            If intSuccess = 1 Then
                If UCase(TypeName(objReceiptID)) = "OBJECT" Then
                    Exit Sub
                Else
                    lblReceiptID.Text = objReceiptID
                    strSelectedReceiptID = objReceiptID
                End If
            Else
                Exit Sub
            End If
        Else

        End If

        strCurrency = ddlCurrency.SelectedItem.Value
        strExRate = txtExRate.Text
        strARExRate = hidARExRate.Value

        If strInvoice <> "" Then
            If txtPPHRate.Text <> "" Then
                intNetAmount = ROUND((txtAmount.Text * 100) / (intPPNRate + 100 - txtPPHRate.Text.Trim),0)
                intPPHAmount = (intNetAmount * txtPPHRate.Text.Trim) / 100
            Else
                intNetAmount = ROUND((txtAmount.Text * 100) / (intPPNRate + 100),0)
                intPPNAmount = 0
            End If
            If cbPPN.Checked = True Then
                intPPNAmount = Round((intNetAmount * Session("SS_PPNRATE")) / 100, 0)
            Else
                intPPNAmount = 0
            End If
        Else
            If (intInvoiceRcvInd + intDebitNoteInd + intCreditNoteInd + intDebtorJrnInd) = 0 Then
                If txtPPHRate.Text <> "" Then
                    intNetAmount = (txtAmount.Text * 100) / (intPPNRate + 100 - txtPPHRate.Text.Trim)
                    intPPHAmount = (intNetAmount * txtPPHRate.Text.Trim) / 100
                Else
                    intNetAmount = (txtAmount.Text * 100) / (intPPNRate + 100)
                    intPPNAmount = 0
                End If
                If cbPPN.Checked = True Then
                    intPPNAmount = Round((intNetAmount * Session("SS_PPNRATE")) / 100, 0)
                Else
                    intPPNAmount = 0
                End If
            Else
                intNetAmount = txtAmount.Text
                intPPNAmount = 0
                intPPHAmount = 0
            End If
        End If

        Select Case strReceiptTerm
            Case "1"
                'Incl. VAT:
                dblAmount = Convert.ToDouble(txtAmount.Text)
                dblPPNAmount = Convert.ToDouble(intPPNAmount)
                dblPPHAmount = Convert.ToDouble(intPPHAmount)
                dblNetAmount = Convert.ToDouble(intNetAmount)
                dblPPHRate = Convert.ToDouble(intPPHRate)

            Case "2"
                'Tax Basis Only (DPP only):
                dblAmount = Convert.ToDouble(txtAmount.Text)
                dblPPNAmount = 0
                dblPPHAmount = 0
                dblNetAmount = Convert.ToDouble(txtAmount.Text)
                dblPPHRate = 0

            Case "3"
                'VAT Only (PPN only):
                dblAmount = Convert.ToDouble(txtAmount.Text)
                dblPPNAmount = 0
                dblPPHAmount = 0
                dblNetAmount = Convert.ToDouble(txtAmount.Text)
                dblPPHRate = 0
        End Select
            
        'dblAmount = Convert.ToDouble(txtAmount.Text)
        'dblPPNAmount = Convert.ToDouble(intPPNAmount)
        'dblPPHAmount = Convert.ToDouble(intPPHAmount)
        'dblNetAmount = Convert.ToDouble(intNetAmount)
        'dblPPHRate = Convert.ToDouble(intPPHRate)

        If hidARCurrency.Value <> "" Then
            If hidARCurrency.Value <> strCurrency Then
                strCBExRate = dblAmount / CDbl(hidARAmount.Value)
            Else
                strCBExRate = txtExRate.Text
            End If
        Else
            strCBExRate = "1"
        End If

        strParam = objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.ARReceiptLn) & "|" & _
                    lblReceiptID.Text & "|" & _
                    strAccCode & "|" & _
                    strDocType & "|" & _
                    strDocID & "|" & _
                    dblAmount & "|" & _
                    strBillParty & "|" & _
                    strBlkCode & "|" & _
                    dblPPHRate & "|" & _
                    intPPN & "|" & _
                    dblPPNAmount & "|" & _
                    dblPPHAmount & "|" & _
                    dblNetAmount & "|" & _
                    strVehCode & "|" & _
                    strVehExp & "|1|" & _
                    strCurrency & "|" & _
                    strExRate & "|" & _
                    strARExRate & "|" & _
                    strCBExRate & "|" & _
                    Replace(Trim(txtAddNote.Value), "'", "''") & "|" & _
                    Trim(TaxLnID) & "|" & _
                    TaxRate & "|" & _
                    DPPAmount & "|" & _
                    strReceiptTerm

        Try
            If ddlChargeLevel.SelectedIndex = 0 And RowPreBlk.Visible = True Then
                strParamList = Session("SS_LOCATION") & "|" & _
                                   txtAccCode.Text.Trim & "|" & _
                                   ddlPreBlock.SelectedItem.Value.Trim & "|" & _
                                   objGLSetup.EnumBlockStatus.Active & "|" & _
                                   strAccMonth & "|" & strAccYear

                intErrNo = objCBTrx.mtdUpdReceiptLineByBlock(strOpCodeGLSubBlkByBlk, _
                                             strParamList, _
                                              strOpCodes, _
                                              strCompany, _
                                              strLocation, _
                                              strUserId, _
                                              strAccMonth, _
                                              strAccYear, _
                                              strParam, _
                                              intError)


            Else
                intErrNo = objCBTrx.mtdUpdReceiptLine(strOpCodes, _
                                              strCompany, _
                                              strLocation, _
                                              strUserId, _
                                              strAccMonth, _
                                              strAccYear, _
                                              strParam, _
                                              intError)

            End If
        Catch Exp As System.Exception
            If intErrNo <> -5 Then
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADD_ADDCASHBANKLINE&errmesg=" & lblErrMessage.Text & "&redirect=CB/Trx/cb_trx_CashBankList.aspx")
            End If
        End Try


        If intError = objCBTrx.EnumErrorType.Exceed Then
            lblErrAmount.Visible = True
            Exit Sub
        Else
            onLoad_Display(lblReceiptID.Text)
            onLoad_DisplayLine(lblReceiptID.Text)
            onLoad_Button()
        End If

    End Sub

    Sub NewBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CB_trx_ReceiptDet.aspx")
    End Sub

    Sub SaveBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim objReceiptID As String = lblReceiptID.Text.Trim
        Dim intSuccess As Integer

        If strSelectedReceiptID = "" Then
            Exit Sub
        End If

        Update_Receipt(objCBTrx.EnumReceiptStatus.Active, objReceiptID, intSuccess)
        If intSuccess = 1 Then
            onLoad_Display(objReceiptID)
            onLoad_DisplayLine(objReceiptID)
            onLoad_Button()
        Else
            Exit Sub
        End If
    End Sub


    Sub ConfirmBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strErrMsg As String
        Dim strParam As String
        Dim strOpCodes As String

        strParam = strSelectedReceiptID
        strOpCodes = "CB_CLSTRX_RECEIPT_GET_FOR_CONFIRM" & "|" & _
                     strOpCode_Receipt_Upd & "|" & _
                     strOpCode_Invoice_Upd & "|" & _
                     strOpCode_DebitNote_Upd & "|" & _
                     strOpCode_CreditNote_Upd & "|" & _
                     strOpCode_DebtorJrn_Upd
        Try
            intErrNo = objCBTrx.mtdReceipt_Confirm(strOpCodes, _
                                                   strCompany, _
                                                   strLocation, _
                                                   strUserId, _
                                                   strAccMonth, _
                                                   strAccYear, _
                                                   strParam, _
                                                   strErrMsg)
            If strErrMsg = "" Then
                onLoad_Display(strSelectedReceiptID)
                onLoad_DisplayLine(strSelectedReceiptID)
                onLoad_Button()
            Else
                lblErrAction.Text = strErrMsg & "<br>"
                lblErrAction.Visible = True
            End If
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_RECEIPTDET_CONFIRM&errmesg=" & Server.UrlEncode(lblErrMessage.Text) & "&redirect=BI/trx/CB_trx_ReceiptList.aspx")
        End Try

    End Sub

    Sub EditBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCodes As String = "CB_CLSTRX_RECEIPT_EDIT"

        strParamName = "RCPID|LOCCODE"
        strParamValue = lblReceiptID.Text.Trim & "|" & _
                     strLocation

        Try

            Try
                intErrNo = objGLtrx.mtdInsertDataCommon(strOpCodes, _
                                                    strParamName, _
                                                    strParamValue)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ID_GET&errmesg=" & Exp.ToString() & "&redirect=")
            End Try



            onLoad_Display(lblReceiptID.Text.Trim)
            onLoad_DisplayLine(lblReceiptID.Text.Trim)
            onLoad_Button()
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_RECEIPTDET_CONFIRM&errmesg=" & Server.UrlEncode(lblErrMessage.Text) & "&redirect=BI/trx/CB_trx_ReceiptList.aspx")
        End Try

    End Sub

    Sub DeleteBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim objReceiptID As String
        Dim intSuccess As Integer
        Select Case CInt(hidStatus.Value)
            Case objCBTrx.EnumReceiptStatus.Active
                Update_Receipt(objCBTrx.EnumReceiptStatus.Deleted, objReceiptID, intSuccess)
            Case objCBTrx.EnumReceiptStatus.Deleted
                Update_Receipt(objCBTrx.EnumReceiptStatus.Active, objReceiptID, intSuccess)
            Case Else
                Exit Sub
        End Select
        onLoad_Display(strSelectedReceiptID)
        onLoad_DisplayLine(strSelectedReceiptID)
        onLoad_Button()
    End Sub


    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strOpCode_DelLine As String = "CB_CLSTRX_RECEIPT_LINE_DEL"
        Dim strOpCode_ReceiptLine_Amount_SUM As String = "CB_CLSTRX_RECEIPT_LINE_AMOUNT_SUM"
        Dim strOpCodes As String
        Dim lblDelText As Label
        Dim strLNId As String
        Dim strDocID As String
        Dim strDocType As String
        Dim strAmount As String

        dgLineDet.EditItemIndex = CInt(E.Item.ItemIndex)
        lblDelText = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lblLnID")
        strLNId = lblDelText.Text
        lblDelText = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lblDocID")
        strDocID = lblDelText.Text
        lblDelText = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lblDocTypeVal")
        strDocType = lblDelText.Text
        lblDelText = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lblAmt")
        strAmount = lblDelText.Text

        If strDocType = objCBTrx.EnumReceiptDocType.ContractInvoice Then 'objGlobal.EnumDocType.ContractInvoice Then
            strOpCodes = strOpCode_DelLine & "|" & strOpCode_Invoice_OutstdAmount_Get & "|" & _
                         strOpCode_Invoice_Upd & "|" & strOpCode_ReceiptLine_Amount_SUM & "|" & _
                         strOpCode_Receipt_Upd & "|"
        ElseIf strDocType = objCBTrx.EnumReceiptDocType.DebitNote Then 'objGlobal.EnumDocType.BIDebitNote Then
            strOpCodes = strOpCode_DelLine & "|" & strOpCode_DebitNote_OutstdAmount_Get & "|" & _
                         strOpCode_DebitNote_Upd & "|" & strOpCode_ReceiptLine_Amount_SUM & "|" & _
                         strOpCode_Receipt_Upd & "|"
        ElseIf strDocType = objCBTrx.EnumReceiptDocType.CreditNote Then 'objGlobal.EnumDocType.BICreditNote Then
            strOpCodes = strOpCode_DelLine & "|" & strOpCode_CreditNote_OutstdAmount_Get & "|" & _
                         strOpCode_CreditNote_Upd & "|" & strOpCode_ReceiptLine_Amount_SUM & "|" & _
                         strOpCode_Receipt_Upd & "|"
        ElseIf strDocType = objCBTrx.EnumReceiptDocType.DebtorJournal Then 'objGlobal.EnumDocType.ARDebtorJournal Then
            strOpCodes = strOpCode_DelLine & "|" & strOpCode_DebtorJrn_OutstdAmount_Get & "|" & _
                         strOpCode_DebtorJrn_Upd & "|" & strOpCode_ReceiptLine_Amount_SUM & "|" & _
                         strOpCode_Receipt_Upd & "|"
        ElseIf strDocType = objCBTrx.EnumReceiptDocType.Receipt Then 'objGlobal.EnumDocType.ARAdvancePayment Then
            strOpCodes = strOpCode_DelLine & "|||" & strOpCode_ReceiptLine_Amount_SUM & "|" & _
                         strOpCode_Receipt_Upd & "|"
        ElseIf strDocType = objCBTrx.EnumReceiptDocType.AdvReceipt Then 'objGlobal.EnumDocType.ARAdvancePayment Then
            strOpCodes = strOpCode_DelLine & "|||" & strOpCode_ReceiptLine_Amount_SUM & "|" & _
                         strOpCode_Receipt_Upd & "|"
        End If

        Try
            strParam = strLNId & "|" & strSelectedReceiptID & "|" & strDocID & "|" & CDbl(strAmount) & "|" & strDocType
            intErrNo = objCBTrx.mtdDelReceiptLine(strOpCodes, _
                                                  strCompany, _
                                                  strLocation, _
                                                  strUserId, _
                                                  strAccMonth, _
                                                  strAccYear, _
                                                  strParam)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_RECEIPTDET_DEL_LINE&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/CB_trx_ReceiptList.aspx")
        End Try

        onLoad_Display(strSelectedReceiptID)
        onLoad_DisplayLine(strSelectedReceiptID)
        onLoad_Button()
    End Sub


    Sub BackBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CB_trx_ReceiptList.aspx")
    End Sub

    Function CheckDate(ByVal pv_strDate As String, ByRef pr_strDate As String) As Boolean
        Dim strDateFormatCode As String = Session("SS_DATEFMT")
        Dim strDateFormat As String

        pr_strDate = ""
        CheckDate = True
        If Not pv_strDate = "" Then
            If objGlobal.mtdValidInputDate(strDateFormatCode, pv_strDate, strDateFormat, pr_strDate) = False Then
                lblFmt.Text = strDateFormat
                pr_strDate = ""
                CheckDate = False
            End If
        End If
    End Function

    Sub BindRecType(ByVal pv_strRecType As String)
        If pv_strRecType = "1" Then
            ddlRecType.SelectedIndex = 1
            ddlBank.SelectedIndex = 0
            txtChequeNo.Text = ""
        ElseIf pv_strRecType = "0" Then
            ddlRecType.SelectedIndex = 0
        ElseIf pv_strRecType = "2" Then
            ddlRecType.SelectedIndex = 2
            txtChequeNo.Text = ""
        End If
        onLoad_Button()
    End Sub

    Sub onSelect_RecType(ByVal Sender As System.Object, ByVal E As System.EventArgs)
        BindRecType(ddlRecType.SelectedItem.Value)
    End Sub

    Sub BindBankCode(ByVal pv_strBankCode As String)
        Dim strOpCode As String = "HR_CLSSETUP_BANK_LIST_GET"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedBankIndex As Integer = 0
        Dim strDate As String = Date_Validation(txtDateCreated.Text, False)
        Dim strOpCodeBank As String

        'strParam = "|"

        'Try
        '    intErrNo = objHRSetup.mtdGetMasterList(strOpCode, _
        '                                           strParam, _
        '                                           objHRSetup.EnumHRMasterType.Bank, _
        '                                           objBankDs)
        'Catch Exp As Exception
        '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_RECEIPT_GET_BANK&errmesg=" & Exp.ToString() & "&redirect=CB/trx/cb_trx_paylist.aspx")
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ID_GET&errmesg=" & Exp.ToString() & "&redirect=")
        End Try

        For intCnt = 0 To objBankDs.Tables(0).Rows.Count - 1
            'objBankDs.Tables(0).Rows(intCnt).Item("BankCode") = objBankDs.Tables(0).Rows(intCnt).Item("BankCode").Trim()
            'objBankDs.Tables(0).Rows(intCnt).Item("Description") = objBankDs.Tables(0).Rows(intCnt).Item("BankCode") & " (" & objBankDs.Tables(0).Rows(intCnt).Item("Description").Trim() & ")"
            If Trim(pv_strBankCode) = Trim(objBankDs.Tables(0).Rows(intCnt).Item("BankCode")) Then
                intSelectedBankIndex = intCnt + 1
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
    End Sub

    Sub onSelect_Bank(ByVal Sender As System.Object, ByVal E As System.EventArgs)
        Dim strOpCode As String = "HR_CLSSETUP_BANK_ACCCODE_GET"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim defaultAccCode As String

        'Try
        '    strParam = ddlBank.SelectedItem.Value
        '    intErrNo = objHRSetup.mtdGetAccCode(strOpCode, strParam, objAccDs)
        'Catch Exp As Exception
        '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_RECEIPT_GET_BANKACCCODE&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_paylist.aspx")
        'End Try

        Dim strDate As String = Date_Validation(txtDateCreated.Text, False)
        Dim strOpCodeBank As String
        Dim strBank As String
        Dim strBankAccCode As String
        Dim arrParam As Array

        If ddlBank.SelectedItem.Value = "" Then
            Exit Sub
        Else
            arrParam = Split(Trim(ddlBank.SelectedItem.Value), "|")
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ID_GET&errmesg=" & Exp.ToString() & "&redirect=")
        End Try

        For intCnt = 0 To objAccDs.Tables(0).Rows.Count - 1
            defaultAccCode = Trim(objAccDs.Tables(0).Rows(intCnt).Item("AccCode"))
        Next intCnt

        If Trim(defaultAccCode) <> "" Then
            txtAccCode.Text = Trim(defaultAccCode)
        End If
    End Sub

    Sub CheckAmount(ByVal pv_strDoc As String)
        Dim arrAmount As Array
        Dim strAmount As String
        Dim strAmount2 As String
        Dim strKurs As String

        arrAmount = Split(pv_strDoc, ",")
        If UBound(arrAmount) > 0 Then
            strAmount = Trim(arrAmount(1))

            pv_strCurrencyCode = Trim(Left(strAmount, Len(strAmount) - 3))
            strAmount2 = Trim(Right(strAmount, Len(strAmount) - 3))
            pv_strCurrencyCode = Trim(Left(Trim(strAmount), Len(Trim(strAmount)) - 3))
            'strAmount2 = Trim(Right(trim(strAmount), Len(trim(strAmount)) - 3))
            strAmount2 = Mid(Trim(strAmount), 5, Len(Trim(strAmount)))
            'strAmount2 = Left(Trim(strAmount2), Len(Trim(strAmount2)) - 3)
            hidARAmount.Value = strAmount2

            txtAmount.Text = CDbl(strAmount2) 'FormatNumber(Round(CDbl(strAmount2), CInt(Session("SS_ROUNDNO"))), CInt(Session("SS_ROUNDNO")))
            If UBound(arrAmount) > 1 Then
                strKurs = Trim(arrAmount(2))
                If InStr(strKurs, ")") > 0 Then
                    txtExRate.Text = Trim(Right(strKurs, Len(strKurs) - 7))
                    txtExRate.Text = Replace(txtExRate.Text, ")", "")
                    pv_strCurrencyCode = Mid(strKurs, 2, 3)
                Else
                    txtExRate.Text = Mid(strKurs, 4, Len(strKurs))
                    pv_strCurrencyCode = Mid(strKurs, 2, 3)
                End If
            Else
                pv_strCurrencyCode = "IDR"
                txtExRate.Text = "1"
            End If

            hidARCurrency.Value = pv_strCurrencyCode
            hidARExRate.Value = txtExRate.Text
        Else
            pv_strCurrencyCode = "IDR"
            txtAmount.Text = ""
            txtExRate.Text = "1"
        End If

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

    Sub BindChargeLevelDropDownList()
        ddlChargeLevel.Items.Add(New ListItem(strPreBlockTag, objLangCap.EnumLangCap.Block))
        ddlChargeLevel.Items.Add(New ListItem(strBlockTag, objLangCap.EnumLangCap.SubBlock))
        ddlChargeLevel.SelectedIndex = Session("SS_BLOCK_CHARGE_DEFAULT")
        RowChargeLevel.Visible = Session("SS_BLOCK_CHARGE_VISIBLE")
        ToggleChargeLevel()
    End Sub

    Sub onSelect_Account(ByVal Sender As Object, ByVal E As EventArgs)
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

        ElseIf intAccType = objGLSetup.EnumAccountType.BalanceSheet And intNurseryInd = objGLSetup.EnumNurseryAccount.Yes Then
            BindPreBlock(strAcc, strPreBlk)
            BindBlockDropList(strAcc, strBlk)
            BindVehicleCodeDropList("")
            BindVehicleExpDropList(True)
        Else
            BindPreBlock("", "")
            BindBlockDropList("", "")
            BindVehicleCodeDropList("")
            BindVehicleExpDropList(True)
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

        If pv_strAccCode <> "" Then
            strOpCd = "GL_CLSSETUP_ACCOUNT_PREBLOCK_GET"
            intSelectedIndex = 0
            Try
                strParam = pv_strAccCode & "|" & strLocation & "|" & objGLSetup.EnumBlockStatus.Active

                intErrNo = objGLSetup.mtdGetAccountBlock(strOpCd, _
                                                         strParam, _
                                                         objBlkDs)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GL_CLSSETUP_ACCOUNT_BLOCK_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
            End Try


            dr = objBlkDs.Tables(0).NewRow()
            dr("BlkCode") = ""
            dr("_Description") = lblPleaseSelect.Text & strPreBlockTag & lblCode.Text

            objBlkDs.Tables(0).Rows.InsertAt(dr, 0)
            ddlPreBlock.DataSource = objBlkDs.Tables(0)
            ddlPreBlock.DataValueField = "BlkCode"
            ddlPreBlock.DataTextField = "_Description"
            ddlPreBlock.DataBind()
            ddlPreBlock.SelectedIndex = intSelectedIndex
        Else
            ddlPreBlock.Items.Clear()
            ddlPreBlock.Items.Add(New ListItem(lblSelect.Text & strBlockTag, ""))
            ddlPreBlock.SelectedIndex = 0
        End If
       
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

        If pv_strAccCode <> "" Then
            Try
                If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfig) = True Then
                    strOpCdBlockList_Get = "GL_CLSSETUP_ACCOUNT_BLOCK_GET"
                    strParam = pv_strAccCode & "|" & strLocation & "|" & objGLSetup.EnumBlockStatus.Active
                Else
                    strOpCdBlockList_Get = "GL_CLSSETUP_ACCOUNT_SUBBLOCK_GET"
                    strParam = pv_strAccCode & "|" & strLocation & "|" & objGLSetup.EnumSubBlockStatus.Active
                End If
                intErrNo = objGLSetup.mtdGetAccountBlock(strOpCdBlockList_Get, _
                                                         strParam, _
                                                         dsForDropDown)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GL_CLSSETUP_ACCOUNT_BLOCK_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
            End Try

            dr = dsForDropDown.Tables(0).NewRow()
            dr("BlkCode") = ""
            dr("_Description") = lblSelect.Text & strBlockTag
            dsForDropDown.Tables(0).Rows.InsertAt(dr, 0)
            lstBlock.DataSource = dsForDropDown.Tables(0)
            lstBlock.DataValueField = "BlkCode"
            lstBlock.DataTextField = "_Description"
            lstBlock.DataBind()
            lstBlock.SelectedIndex = intSelectedIndex
        Else
            lstBlock.Items.Clear()
            lstBlock.Items.Add(New ListItem(lblSelect.Text & strBlockTag, ""))
            lstBlock.SelectedIndex = 0
        End If
        
        If Not dsForDropDown Is Nothing Then
            dsForDropDown = Nothing
        End If
    End Sub

    Sub BindVehicleCodeDropList(ByVal pv_strAccCode As String, Optional ByVal pv_strVehCode As String = "")
        Dim dsForDropDown As DataSet
        Dim strOpCd As String
        Dim drinsert As DataRow
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intMasterType As Integer
        Dim intSelectedIndex As Integer = 0

        If pv_strAccCode <> "" Then
            strParam = "|AccCode = '" & pv_strAccCode & "' AND LocCode = '" & strLocation & "' AND Status = '" & objGLSetup.EnumVehicleStatus.Active & "'"
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
        Else
            lstVehCode.Items.Clear()
            lstVehCode.Items.Add(New ListItem(lblSelect.Text & strVehTag, ""))
            lstVehCode.SelectedIndex = 0
        End If
        

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

        If pv_strVehExpCode <> "" Then
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
        Else
            lstVehExp.Items.Clear()
            lstVehExp.Items.Add(New ListItem(lblSelect.Text & strVehExpTag, ""))
            lstVehExp.SelectedIndex = 0
        End If
        

        If Not dsForDropDown Is Nothing Then
            dsForDropDown = Nothing
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

    Sub BindCurrencyList(ByVal pv_strCurrencyCode As String)
        Dim strParam As String
        Dim strSearch As String
        Dim strSort As String
        Dim strOpCdGet As String = "CM_CLSSETUP_CURRENCY_GET"
        Dim dr As DataRow
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer
        Dim objCurrencyDs As New Object()

        strSearch = "and curr.Status = '" & objCMSetup.EnumCurrencyStatus.Active & "' "
        strSort = "order by curr.CurrencyCode "

        strParam = strSearch & "|" & strSort

        Try
            intErrNo = objCMSetup.mtdGetMasterList(strOpCdGet, strParam, 0, objCurrencyDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_CURRENCYLIST_GET&errmesg=" & Exp.ToString() & "&redirect=CM/Trx/CM_Trx_ContractRegList.aspx")
        End Try

        'intSelectedIndex = 1
        If pv_strCurrencyCode = "" Then
            pv_strCurrencyCode = "IDR"
        End If
        If objCurrencyDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objCurrencyDs.Tables(0).Rows.Count - 1
                objCurrencyDs.Tables(0).Rows(intCnt).Item("CurrencyCode") = Trim(objCurrencyDs.Tables(0).Rows(intCnt).Item("CurrencyCode"))
                objCurrencyDs.Tables(0).Rows(intCnt).Item("Description") = objCurrencyDs.Tables(0).Rows(intCnt).Item("CurrencyCode")
                If objCurrencyDs.Tables(0).Rows(intCnt).Item("CurrencyCode") = pv_strCurrencyCode Then
                    intSelectedIndex = intCnt '+1
                End If
            Next
        End If


        ddlCurrency.DataSource = objCurrencyDs.Tables(0)
        ddlCurrency.DataValueField = "CurrencyCode"
        ddlCurrency.DataTextField = "Description"
        ddlCurrency.DataBind()
        ddlCurrency.SelectedIndex = intSelectedIndex
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
                        Session("SS_USERID") & "|" & Trim(lblReceiptID.Text)

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

        strSelectedReceiptID = Trim(lblReceiptID.Text)
        onLoad_Display(strSelectedReceiptID)
        onLoad_DisplayLine(strSelectedReceiptID)
        onLoad_Button()
    End Sub

    Sub BindOtherAccCode(ByVal pv_strAccCode As String)
        'Dim strOpCode_GetAcc As String = "GL_CLSSETUP_ACCOUNTCODE_LIST_GET"
        'Dim strParam As String
        'Dim intErrNo As Integer
        'Dim intCnt As Integer
        'Dim intSelectedIndex As Integer = 0
        'Dim objAccOthDs As New Object()

        'Try

        '    If LblIsSKBActive.Text = 1 And (Format(txtDateCreated.Text, "dd/MM/yyyy") >= Format(lblSKBStartDate.Text, "dd/MM/yyyy")) Then
        '        strParam = "Order By ACC.AccCode|And ACC.Status = '" & objGLSetup.EnumAccountCodeStatus.Active & "' AND ACC.AccCode NOT IN (SELECT AccCode FROM dbo.TX_TAXOBJECTRATELN) "
        '    Else
        '        strParam = "Order By ACC.AccCode|And ACC.Status = '" & objGLSetup.EnumAccountCodeStatus.Active & "' "
        '    End If

        '    strParam = strParam & IIf(Session("SS_COACENTRALIZED") = "1", "", " AND LocCode = '" & Trim(strLocation) & "' ")
        '    intErrNo = objGLSetup.mtdGetMasterList(strOpCode_GetAcc, strParam, objGLSetup.EnumGLMasterType.AccountCode, objAccOthDs)

        'Catch Exp As Exception
        '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_GET_SUPP2&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_paylist.aspx")
        'End Try

        'For intCnt = 0 To objAccOthDs.Tables(0).Rows.Count - 1
        '    If objAccOthDs.Tables(0).Rows(intCnt).Item("AccCode") = pv_strAccCode Then
        '        intSelectedIndex = intCnt + 1
        '        Exit For
        '    End If
        'Next intCnt


        'Dim dr As DataRow
        'dr = objAccOthDs.Tables(0).NewRow()
        'dr("AccCode") = ""
        'dr("_Description") = lblPleaseSelect.Text & "Other Cost"
        'objAccOthDs.Tables(0).Rows.InsertAt(dr, 0)
        ''ddlOther
        'ddlOther.DataSource = objAccOthDs.Tables(0)
        'ddlOther.DataValueField = "AccCode"
        'ddlOther.DataTextField = "_Description"
        'ddlOther.DataBind()
        'ddlOther.SelectedIndex = intSelectedIndex

        'onLoad_Button()
    End Sub

    Sub BindContractNoList(ByVal pv_strBuyer As String, ByVal pv_strContNo As String)
        Dim strParam As String
        Dim strOpCdGet As String = "CM_CLSTRX_CONTRACT_REG_GET"
        Dim dr As DataRow
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer
        Dim strSearch As String
        Dim objContractDs As New Object()

        strSearch = "and ctr.LocCode = '" & strLocation & "' and ctr.BuyerCode like '%" & pv_strBuyer & "' and ctr.status in ('1', '4') "
        strParam = strSearch & "|" & ""


        Try
            intErrNo = objCMTrx.mtdGetContract(strOpCdGet, strParam, 0, objContractDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegList.aspx")
        End Try

        If objContractDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objContractDs.Tables(0).Rows.Count - 1
                objContractDs.Tables(0).Rows(intCnt).Item("ContractNo") = Trim(objContractDs.Tables(0).Rows(intCnt).Item("ContractNo"))
                If objContractDs.Tables(0).Rows(intCnt).Item("ContractNo") = pv_strContNo Then
                    intSelectedIndex = intCnt + 1
                End If
            Next

            hidPPNInit.Value = objContractDs.Tables(0).Rows(0).Item("PPNInit")
            If Trim(objContractDs.Tables(0).Rows(0).Item("PPNInit")) = "0" Then
                cbPPN.Checked = False
                cbPPN.Text = "  No"
            Else
                cbPPN.Checked = True
                cbPPN.Text = "  Yes"
            End If
            cbPPN.Enabled = False
        End If

        dr = objContractDs.Tables(0).NewRow()
        dr("ContractNo") = ""
        dr("ContractDescr") = "Please Select Contract No"
        objContractDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlContract.DataSource = objContractDs.Tables(0)
        ddlContract.DataValueField = "ContractNo"
        ddlContract.DataTextField = "ContractDescr"
        ddlContract.DataBind()
        ddlContract.SelectedIndex = intSelectedIndex
    End Sub

    Sub onSelect_Contract(ByVal sender As System.Object, ByVal e As System.EventArgs)
        CheckAmount(ddlContract.SelectedItem.Text)
        'BindCurrencyList(pv_strCurrencyCode)
        'ddlCurrency.SelectedValue = Trim(pv_strCurrencyCode)

        If ddlContract.SelectedIndex <> 0 And ddlBank.SelectedIndex = 0 Then
            BindAccount("DUMMY")
        Else
            BindAccount(ddlBank.SelectedItem.Value)
        End If
    End Sub

    Sub onSelect_Other(ByVal sender As System.Object, ByVal e As System.EventArgs)
        CheckCOATax()
    End Sub

    Sub CheckCOATax()
        Dim strParamName As String
        Dim strParamValue As String
        Dim objTaxDs As New Object
        Dim intErrNo As Integer
        Dim strOpCd As String = "TX_CLSSETUP_TAXOBJECTRATE_LIST_GET"

        strParamName = "STRSEARCH"
        strParamValue = " AND TOB.AccCode = '" & Trim(txtAccCodeOth.Text.Trim) & "' "

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCd, _
                                                strParamName, _
                                                strParamValue, _
                                                objTaxDs)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=TX_Setup_TaxObjectDet&errmesg=" & Exp.Message.ToString & "&redirect=")
        End Try

        If objTaxDs.Tables(0).Rows.Count > 0 Then
            RowTax.Visible = True
            RowTaxAmt.Visible = True
            hidCOATax.Value = 1
            txtDPPAmount.Text = IIf(CDbl(hidInvAmount.Value) = 0, 0, hidInvAmount.Value * -1)
            txtAmount.Text = 0
            BindAccount("DUMMY")
            BindTaxObjectList(txtAccCodeOth.Text.Trim, "")
        Else
            RowTax.Visible = False
            RowTaxAmt.Visible = False
            hidCOATax.Value = 0
            txtAmount.ReadOnly = False
        End If
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=TX_Setup_TaxObjectDet&errmesg=" & Exp.ToString() & "&redirect=")
        End Try

        For intCnt = 0 To dsForDropDown.Tables(0).Rows.Count - 1
            If Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("TaxLnID")) = Trim(pv_strTaxLnID) Then
                intSelectedIndex = intCnt + 1
            End If
        Next intCnt

        dr = dsForDropDown.Tables(0).NewRow()
        dr("TaxLnID") = ""
        dr("Descr") = lblPleaseSelect.Text & "Tax Object"
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
        Dim Amt As Double
        arrParam = Split(lstTaxObject.SelectedItem.Text, "-")

        If lstTaxObject.SelectedItem.Value = "" Then
            txtAmount.ReadOnly = False
        Else
            hidTaxObjectRate.Value = CDbl(Replace(arrParam(1), "%", ""))
            Amt = CDbl(IIf(txtDPPAmount.Text = "", 0, txtDPPAmount.Text)) * (hidTaxObjectRate.Value / 100)
            Amt = Math.Floor(Amt + 0.5)
            txtAmount.Text = Amt
            txtAmount.ReadOnly = True
        End If
    End Sub

    Sub btnPreview_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim strOpCodePrint As String = "ADMIN_SHARE_UPD_PRINTDATE"
        Dim strUpdString As String = ""
        Dim strStatus As String
        Dim intStatus As Integer
        Dim strSortLine As String
        Dim strPrintDate As String
        Dim strTable As String
        Dim strAccountTag As String
        Dim strID As String = hidReceiptID.Value
        
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

        hidReceiptID.Value = lblReceiptID.Text.Trim

        strAccountTag = lblAccount.Text
        strUpdString = "where PaymentID = '" & hidReceiptID.Value & "'"
        strStatus = Trim(lblStatus.Text)
        intStatus = Convert.ToInt16(hidStatus.Value)
        strPrintDate = Trim(lblPrintDate.Text)
        strSortLine = "CB_RECEIPTLN.ReceiptLnID"
        strTable = "CB_RECEIPT"

        Response.Write("<Script Language=""JavaScript"">window.open(""../reports/CB_Rpt_ReceiptDet.aspx?strPayId=" & hidReceiptID.Value & _
                       "&strPrintDate=" & strPrintDate & _
                       "&strStatus=" & strStatus & _
                       "&strSortLine=" & strSortLine & _
                       "&strAccountTag=" & strAccountTag & _
                       """, null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")
    End Sub

    Sub dgLine_BindGrid(ByVal sender As Object, ByVal e As DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor='lightblue'")
            If e.Item.ItemType = ListItemType.AlternatingItem Then
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='e9e9e9'")
            Else
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='f2f2f2'")
            End If
        End If
    End Sub

End Class

