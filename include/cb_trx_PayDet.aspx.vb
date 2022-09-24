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

Public Class cb_trx_PayDet : Inherits Page

    Protected WithEvents lblErrBank As Label
    Protected WithEvents lblPayType As Label
    Protected WithEvents lblErrCheque As Label
    Protected WithEvents lblPaymentID As Label    
    Protected WithEvents txtDescription As TextBox
    Protected WithEvents lblAccPeriod As Label
    Protected WithEvents lblStatus As Label
    Protected WithEvents ddlPayType As DropDownList
    Protected WithEvents ddlBank As DropDownList
    Protected WithEvents lblDateCreated As Label
    Protected WithEvents txtDateCreated As TextBox
    Protected WithEvents btnDateCreated As Image
    Protected WithEvents lblDate As Label
    Protected WithEvents lblFmt As Label

    Protected WithEvents txtSupCode As TextBox
    Protected WithEvents txtSupName As TextBox

    Protected WithEvents txtPPN As TextBox
    Protected WithEvents txtCreditTerm As TextBox
    Protected WithEvents txtPPNInit As TextBox

    Protected WithEvents lblLastUpdate As Label
    Protected WithEvents txtChequeNo As TextBox
    Protected WithEvents lblChequePrintDate As Label
    Protected WithEvents lblPrintDate As Label
    Protected WithEvents lblUpdatedBy As Label
    Protected WithEvents ddlInvoiceRcv As DropDownList
    Protected WithEvents ddlDebitNote As DropDownList

    Protected WithEvents ddlOther As DropDownList
    Protected WithEvents ddlCreditNote As DropDownList
    Protected WithEvents ddlAccCode As DropDownList
    Protected WithEvents txtAmount As TextBox
    Protected WithEvents tblSelection As HtmlTable
    Protected WithEvents dgLineDet As DataGrid
    Protected WithEvents lblTotalPaymentAmount As Label
    Protected WithEvents txtRemark As TextBox
    Protected WithEvents SaveBtn As ImageButton
    Protected WithEvents RefreshBtn As ImageButton
    Protected WithEvents ConfirmBtn As ImageButton
    Protected WithEvents PrintBtn As ImageButton
    Protected WithEvents PrintChequeBtn As ImageButton
    Protected WithEvents DeleteBtn As ImageButton
    Protected WithEvents UnDeleteBtn As ImageButton
    Protected WithEvents BackBtn As ImageButton
    Protected WithEvents payid As HtmlInputHidden
    Protected WithEvents lblErrAccCode As Label
    Protected WithEvents lblErrConfirmNotFulFil As Label
    Protected WithEvents lblErrConfirmNotFulFilText As Label
    Protected WithEvents lblErrReqAmount As Label
    Protected lblErrNegAmt As Label
    Protected lblErrPosAmt As Label
    Protected WithEvents lblErrNoSelectDoc As Label
    Protected WithEvents lblErrManySelectDoc As Label
    Protected WithEvents lblErrOtherDoc As Label
    Protected WithEvents lblErrPrintCheque As Label
    Protected WithEvents lblStatusHidden As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents lblPleaseSelect As Label
    Protected WithEvents lblPleaseSelectOne As Label
    Protected WithEvents lblAccount As Label
    Protected WithEvents lblProgramPath As Label
    Protected WithEvents lblPPN As Label
    Protected WithEvents lblPPH As Label
    Protected WithEvents lblPercen As Label
    Protected WithEvents cbPPN As CheckBox
    Protected WithEvents LblIsSKBActive As Label
    Protected WithEvents lblSKBStartDate As Label

    Protected WithEvents txtPPHRate As TextBox
    Protected WithEvents lblInvTypeHidden As Label
    Protected WithEvents lblPPHRateHidden As Label
    Protected WithEvents lblPPNHidden As Label
    Protected WithEvents lblErrValidPPNHRate As Label
    Protected WithEvents lblID As Label
    Protected WithEvents lblInvoiceRcvTag As Label
    Protected WithEvents lblInvoiceRcvIdTag As Label
    Protected WithEvents lblViewTotalPaymentAmount As Label
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Protected WithEvents ddlCreditorJournal As DropDownList
    Protected hidCreditJrnValue As HtmlInputHidden
    Protected WithEvents ddlCurrency As DropDownList
    Protected WithEvents txtExRate As TextBox
    Protected WithEvents hidPOExRate As HtmlInputHidden
    Protected WithEvents hidInvAmount As HtmlInputHidden
    Protected WithEvents hidPOCurrency As HtmlInputHidden
    Protected WithEvents lblCurrency As Label
    Protected WithEvents txtSplBankAccNo As TextBox
    Protected WithEvents PrintSlipBtn As ImageButton
    Protected WithEvents PrintTransferBtn As ImageButton
    Protected WithEvents txtGiroDate As TextBox
    Protected WithEvents lblDateGiro As Label
    Protected WithEvents lblFmtGiro As Label
    Protected WithEvents lblShowTotalAmount As Label
    Protected WithEvents NewBtn As ImageButton
    Protected WithEvents lblErrExRate As Label
    Protected WithEvents lblCurrentPeriod As Label
    Protected WithEvents lblErrPayType As Label
    Protected WithEvents EditBtn As ImageButton
    Protected WithEvents btnGiroDate As Image
    Protected WithEvents Find As HtmlInputButton
    Protected WithEvents VerifiedBtn As ImageButton
    Protected WithEvents lblTxLnID As Label
    Protected WithEvents AddDtlBtn As ImageButton
    Protected WithEvents SaveDtlBtn As ImageButton
    Protected WithEvents ForwardBtn As ImageButton

    Protected WithEvents TrLink As HtmlTableRow
    Protected WithEvents lbViewJournal As LinkButton
    Protected WithEvents dgViewJournal As DataGrid
    Protected WithEvents lblTotalDB As Label
    Protected WithEvents lblTotalCR As Label

    Protected WithEvents lblTotalViewJournal As Label
    Protected WithEvents ddlPOID As DropDownList

    Protected WithEvents lblErrCurrencyCode As Label
    Protected WithEvents lblErrExceeded As Label
    Protected WithEvents hidOutstandingAmount As HtmlInputHidden
    Protected WithEvents hidOutstandingAmountKonversi As HtmlInputHidden

    Protected WithEvents ddlCurrCode As DropDownList
    Protected WithEvents txtExchangeRate As TextBox
    Protected WithEvents lblErrExchangeRate As Label

    Protected WithEvents hidUserID As HtmlInputHidden
    Protected WithEvents txtAddNote As HtmlTextArea

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
    Protected WithEvents lblTaxStatus As Label
    Protected WithEvents lblTaxStatusDesc As Label
    Protected WithEvents lblTaxUpdate As Label
    Protected WithEvents lblTaxUpdateDesc As Label
    Protected WithEvents hidDocID As HtmlInputHidden

    Protected WithEvents hidPrevID As HtmlInputHidden
    Protected WithEvents hidPrevDate As HtmlInputHidden
    Protected WithEvents hidNextID As HtmlInputHidden
    Protected WithEvents hidNextDate As HtmlInputHidden

    Protected WithEvents ddlSplBankAccNo As DropDownList

    Protected WithEvents hidPOPPH21 As HtmlInputHidden
    Protected WithEvents hidFindPOPPH21 As HtmlInputHidden
    Protected WithEvents lblFindINVPOPPH21 As Label
    Protected WithEvents lblFindPOPPH21 As Label

    Protected WithEvents txtAccCodeOther As TextBox
    Protected WithEvents txtAccCode As TextBox
    Protected WithEvents txtAccOtherName As TextBox
    Protected WithEvents txtAccName As TextBox
    Protected WithEvents lblAccCodeErr As Label

    Protected WithEvents ddlGiroNo As DropDownList
    Protected WithEvents lblErrGiroNo As Label

    Protected WithEvents ddlRefNo As DropDownList
    Protected WithEvents chkChequeCash As CheckBox
    Protected WithEvents ddlChequeHandOver As DropDownList

    Dim objCMSetup As New agri.CM.clsSetup()
    Protected objCBTrx As New agri.CB.clsTrx()
    Dim objGLSetup As New agri.GL.clsSetup()
    Dim objPUSetup As New agri.PU.clsSetup()
    Dim objHRSetup As New agri.HR.clsSetup()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objAdmShare As New agri.Admin.clsShare()
    Dim objPRRpt As New agri.PR.clsReport()
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objGLtrx As New agri.GL.ClsTrx()
    Dim objAPTrx As New agri.AP.clsTrx()
    Dim objPU As New agri.PU.clsTrx()

    Dim objSuppDs As New Object()
    Dim objAccDs As New Object()
    Dim objAccOthDs As New Object()
    Dim objInvRcvDs As New Object()
    Dim objDebitNoteDs As New Object()
    Dim objCreditNoteDs As New Object()
    Dim objBankDs As New Object()
    Dim objPayLnDs As New Object()
    Dim objLangCapDs As New Object()
    Dim objCreditorJournalDs As New Object()
    Dim objPPNHDs As New Object()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intCBAR As Integer
    Dim strInvType As String
    Dim intPPN As Double
    Dim intPPNAmount As Double
    Dim intPPHAmount As Double
    Dim intNetAmount As Double
    Dim strSuppName As String
    Dim strParam As String = ""
    Dim strSelectedPayID As String
    Dim intPayStatus As Integer
    Dim strDocNotFulfil As String

    Dim strPhyMonth As String
    Dim strPhyYear As String
    Dim strLocType As String

    Dim pv_strCurrencyCode As String
    Dim strCurrency As String
    Dim strExRate As String
    Dim strPOExRate As String
    Dim strCBExRate As String
    Dim intLevel As Integer
    Dim strSelectedPOId As String
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
        strAccMonth = Session("SS_CBACCMONTH")
        strAccYear = Session("SS_CBACCYEAR")
        strLangCode = Session("SS_LANGCODE")
        intCBAR = Session("SS_CBAR")

        strPhyMonth = Session("SS_PHYMONTH")
        strPhyYear = Session("SS_PHYYEAR")
        strLocType = Session("SS_LOCTYPE")
        intLevel = Session("SS_USRLEVEL")
        strSelAccMonth = Session("SS_SELACCMONTH")
        strSelAccYear = Session("SS_SELACCYEAR")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCBAccessRights.CBPayment), intCBAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else

            'lblErrAccCode.Visible = False
            lblErrConfirmNotFulFil.Visible = False
            lblErrReqAmount.Visible = False
            lblErrNegAmt.Visible = False
            lblErrPosAmt.Visible = False
            lblErrNoSelectDoc.Visible = False
            lblErrManySelectDoc.Visible = False
            lblErrOtherDoc.Visible = False
            lblErrBank.Visible = False
            lblErrCheque.Visible = False
            lblErrPrintCheque.Visible = False
            lblErrValidPPNHRate.Visible = False
            lblDateGiro.Visible = False
            lblFmtGiro.Visible = False
            lblErrExRate.Visible = False
            lblErrPayType.Visible = False
            lblDate.Visible = False
            lblErrCurrencyCode.Visible = False
            lblErrExceeded.Visible = False
            lblErrExchangeRate.Visible = False
            lblDate.Visible = False
            lblFmt.Visible = False
            lblErrAmountDPP.Visible = False
            lblTaxObjectErr.Visible = False
            lblFindPOPPH23.Visible = False
            lblFindINVPOPPH23.Visible = False
            lblFindPOPPH21.Visible = False
            lblFindINVPOPPH21.Visible = False

            'to avoid double click, on aspx add this : UseSubmitBehavior="false"
            AddDtlBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(AddDtlBtn).ToString())
            NewBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(NewBtn).ToString())
            SaveBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(SaveBtn).ToString())
            VerifiedBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(VerifiedBtn).ToString())
            ConfirmBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(ConfirmBtn).ToString())
            ForwardBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(ForwardBtn).ToString())
            DeleteBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(DeleteBtn).ToString())
            UnDeleteBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(UnDeleteBtn).ToString())
            EditBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(EditBtn).ToString())
            BackBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(BackBtn).ToString())
            PrintBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(PrintBtn).ToString())
            PrintChequeBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(PrintChequeBtn).ToString())
            PrintSlipBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(PrintSlipBtn).ToString())
            PrintTransferBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(PrintTransferBtn).ToString())

            strSelectedPayID = Trim(IIf(Request.QueryString("payid") = "", Request.Form("payid"), Request.QueryString("payid")))
            payid.Value = strSelectedPayID

            ddlCreditorJournal.Attributes.Add("onchange", "fnPosNeg(this.options[this.selectedIndex].text);")

            If Not IsPostBack Then
                onload_GetLangCap()
                SaveBtn.Visible = False
                RefreshBtn.Visible = False
                ConfirmBtn.Visible = False
                DeleteBtn.Visible = False
                UnDeleteBtn.Visible = False
                NewBtn.Visible = False
                ForwardBtn.Visible = False
                'BindSupplier("")
                BindCurrencyList("")
                BindRefNo("")
                BindGiroNo("", "")

                txtSupCode.Attributes.Add("readonly", "readonly")
                txtSupName.Attributes.Add("readonly", "readonly")
                txtPPNInit.Attributes.Add("readonly", "readonly")
                txtPPN.Attributes.Add("readonly", "readonly")
                txtCreditTerm.Attributes.Add("readonly", "readonly")
                txtAccOtherName.Attributes.Add("readonly", "readonly")
                txtAccName.Attributes.Add("readonly", "readonly")

                LblIsSKBActive.Text = 0
                lblSKBStartDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)

                If strSelectedPayID <> "" Then
                    'BindCurrencyList("")
                    'BindOtherAccCode("")
                    onLoad_Display(strSelectedPayID)
                    onLoad_DisplayLine(strSelectedPayID)
                Else
                    txtDateCreated.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
                    txtGiroDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
                    'ssBindSupplier("")
                    BindPayType("")
                    BindBankCode("")
                    'BindInvoiceRcv("")
                    'BindDebitNote("")
                    'BindCreditNote("")
                    ''BindAccCode("")
                    ''BindOtherAccCode("")
                    'BindCreditorJournal("")
                    lblInvTypeHidden.Text = ""
                    'BindPPNH("")
                    BindCurrencyList("")
                    'BindPOID("")
                    BindSplBankAccNo("", "")
                    TrLink.Visible = False
                End If

                onLoad_Button()
            End If
        End If
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblInvoiceRcvTag.Text = GetCaption(objLangCap.EnumLangCap.InvoiceReceive)
        lblInvoiceRcvIdTag.Text = GetCaption(objLangCap.EnumLangCap.InvoiceReceive) & lblID.Text
        lblAccount.Text = "COA Cash/Bank" 'GetCaption(objLangCap.EnumLangCap.Account) & lblCode.Text
        'lblErrAccCode.Text = "<br>" & lblPleaseSelectOne.Text & lblAccount.Text
        dgLineDet.Columns(2).HeaderText = lblAccount.Text
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYDET_GET_LANGCAP&errmesg=" & Exp.ToString() & "&redirect=cb/trx/cb_trx_paylist.aspx")
        End Try

    End Sub

    Function GetCaption(ByVal pv_TermCode) As String
        Dim count As Integer

        For count = 0 To objLangCapDs.Tables(0).Rows.Count - 1
            If Trim(pv_TermCode) = objLangCapDs.Tables(0).Rows(count).Item("TermCode").Trim() Then
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
        Dim strDate As String = Date_Validation(txtDateCreated.Text, False)


        ddlPayType.Enabled = False
        ddlBank.Enabled = False
        txtChequeNo.Enabled = False
        txtRemark.Enabled = False
        tblSelection.Visible = False
        PrintChequeBtn.Visible = False
        PrintSlipBtn.Visible = False
        PrintTransferBtn.Visible = False
        lblInvTypeHidden.Text = ""
        lblPPN.Visible = False
        cbPPN.Enabled = False
        cbPPN.Visible = False
        lblPPH.Visible = False
        lblPercen.Visible = False
        txtPPHRate.Enabled = False
        txtPPHRate.Visible = False
        lblDateCreated.Visible = False
        txtDateCreated.Enabled = False
        btnDateCreated.Visible = False
        txtGiroDate.Enabled = False
        btnGiroDate.Visible = False
        txtChequeNo.Enabled = False
        txtSplBankAccNo.Enabled = False
        NewBtn.Visible = False
        EditBtn.Visible = False
        Find.Disabled = True
        SaveBtn.Visible = False
        VerifiedBtn.Visible = False
        ForwardBtn.Visible = False
        RowTax.Visible = False
        RowTaxAmt.Visible = False
        hidCOATax.Value = 0
        txtDPPAmount.Text = 0
        txtAmount.Text = 0
        ddlRefNo.Enabled = False

        If (lblStatusHidden.Text <> "") Then
            intStatus = Convert.ToInt16(lblStatusHidden.Text)
            Select Case intStatus
                Case objCBTrx.EnumPaymentStatus.Active, objCBTrx.EnumPaymentStatus.Verified
                    ddlPayType.Enabled = True
                    ddlBank.Enabled = True
                    txtChequeNo.Enabled = True
                    ddlBank.Enabled = True
                    txtChequeNo.Enabled = True
                    txtSplBankAccNo.Enabled = True
                    txtGiroDate.Enabled = True
                    btnGiroDate.Visible = True
                    txtRemark.Enabled = True
                    tblSelection.Visible = True
                    SaveBtn.Visible = True
                    RefreshBtn.Visible = True
                    ConfirmBtn.Visible = True
                    DeleteBtn.Visible = True
                    UnDeleteBtn.Visible = False
                    NewBtn.Visible = True
                    PrintBtn.Visible = True
                    Find.Disabled = True
                    ddlRefNo.Enabled = True
                    txtDateCreated.Enabled = True

                    If Left(lblPaymentID.Text.Trim, 3) = "XXX" Then
                        ForwardBtn.Visible = True
                    Else
                        ForwardBtn.Visible = False
                    End If

                    Select Case intLevel
                        Case 0 'user finance
                            VerifiedBtn.Visible = False
                            ConfirmBtn.Visible = False
                            DeleteBtn.Visible = False
                            UnDeleteBtn.Visible = False
                        Case 1 'user accounting
                            VerifiedBtn.Visible = False
                            ConfirmBtn.Visible = False
                        Case Else  'supervisor akunting and up
                            VerifiedBtn.Visible = True
                            If Left(lblPaymentID.Text.Trim, 3) = "XXX" Then
                                ConfirmBtn.Visible = False
                            Else
                                ConfirmBtn.Visible = True
                            End If
                    End Select

                    If dgLineDet.Items.Count = 0 Then
                        Find.Disabled = False
                    End If

                    If Month(strDate) <= Mid(lblCurrentPeriod.Text, 5) And Year(strDate) = Mid(lblCurrentPeriod.Text, 1, 4) Then
                        txtDateCreated.Enabled = True
                        btnDateCreated.Visible = True
                    Else
                        txtDateCreated.Enabled = False
                        btnDateCreated.Visible = False
                    End If

                    'temporary ?
                    If Convert.ToInt16(lblPayType.Text) <> objCBTrx.EnumPaymentType.OtherParty Then
                        Dim strOpCode As String = "CB_CLSTRX_PAYMENT_GET_FOR_DOCRPT_CHEQUEGIRO"
                        Dim dsMaster As Object
                        Dim intErrNo As Integer
                        Dim strBankName As String
                        Dim strSuppBankName As String

                        strParamName = "PAYMENTID"
                        strParamValue = Trim(lblPaymentID.Text)

                        Try
                            intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
                                                                strParamName, _
                                                                strParamValue, _
                                                                dsMaster)
                        Catch Exp As System.Exception
                            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GENERATE_INVOICE_WM_GET_HEADER&errmesg=" & Exp.ToString() & "&redirect=ap/trx/ap_trx_invrcv_wm_list.aspx")
                        End Try

                        If dsMaster.Tables(0).Rows.Count > 0 Then
                            strBankName = Trim(dsMaster.Tables(0).Rows(0).Item("BankName"))
                            strSuppBankName = Trim(dsMaster.Tables(0).Rows(0).Item("SuppBankName"))

                            If strBankName = strSuppBankName Or strSuppBankName = "" Then
                                PrintSlipBtn.Visible = True
                            Else
                                PrintSlipBtn.Visible = False
                            End If
                            PrintTransferBtn.Visible = TRUE
                            PrintChequeBtn.Visible = True
                            txtChequeNo.Enabled = True
                        Else
                            PrintSlipBtn.Visible = True
                        End If
                    End If

                Case objCBTrx.EnumPaymentStatus.Deleted
                    DeleteBtn.Visible = False
                    If intLevel = 0 Then
                        UnDeleteBtn.Visible = False
                    Else
                        UnDeleteBtn.Visible = True
                    End If
                    NewBtn.Visible = True
                    SaveBtn.Visible = False
                    ConfirmBtn.Visible = False
                    PrintBtn.Visible = False
                    VerifiedBtn.Visible = False
                    ForwardBtn.Visible = False
                    ddlCurrCode.Enabled = False
                    txtExchangeRate.Enabled = False
                    ddlRefNo.Enabled = False

                Case objCBTrx.EnumPaymentStatus.Confirmed
                    If Convert.ToInt16(lblPayType.Text) <> objCBTrx.EnumPaymentType.OtherParty Then
                        Dim strOpCode As String = "CB_CLSTRX_PAYMENT_GET_FOR_DOCRPT_CHEQUEGIRO"
                        Dim dsMaster As Object
                        Dim intErrNo As Integer
                        Dim strBankName As String
                        Dim strSuppBankName As String

                        strParamName = "PAYMENTID"
                        strParamValue = Trim(lblPaymentID.Text)

                        Try
                            intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
                                                                strParamName, _
                                                                strParamValue, _
                                                                dsMaster)
                        Catch Exp As System.Exception
                            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GENERATE_INVOICE_WM_GET_HEADER&errmesg=" & Exp.ToString() & "&redirect=ap/trx/ap_trx_invrcv_wm_list.aspx")
                        End Try

                        If dsMaster.Tables(0).Rows.Count > 0 Then
                            strBankName = Trim(dsMaster.Tables(0).Rows(0).Item("BankName"))
                            strSuppBankName = Trim(dsMaster.Tables(0).Rows(0).Item("SuppBankName"))

                            If strBankName = strSuppBankName Or strSuppBankName = "" Then
                                PrintSlipBtn.Visible = True
                            Else
                                PrintSlipBtn.Visible = False
                            End If
                            PrintTransferBtn.Visible = True
                            PrintChequeBtn.Visible = True
                        Else
                            PrintSlipBtn.Visible = True
                        End If
                    End If
                    txtSplBankAccNo.Enabled = False
                    SaveBtn.Visible = False
                    NewBtn.Visible = True
                    EditBtn.Visible = True
                    DeleteBtn.Visible = False
                    UnDeleteBtn.Visible = False
                    ConfirmBtn.Visible = False
                    VerifiedBtn.Visible = False
                    ForwardBtn.Visible = False
                    ddlCurrCode.Enabled = False
                    txtExchangeRate.Enabled = False
                    ddlRefNo.Enabled = False
            End Select
            lblDateCreated.Visible = True
        Else
            ddlPayType.Enabled = True
            ddlBank.Enabled = True
            txtChequeNo.Enabled = True
            txtSplBankAccNo.Enabled = True
            txtRemark.Enabled = True
            tblSelection.Visible = True
            SaveBtn.Visible = True
            ConfirmBtn.Visible = False
            btnDateCreated.Visible = True
            txtDateCreated.Enabled = True
            NewBtn.Visible = False
            txtGiroDate.Enabled = True
            btnGiroDate.Visible = True
            Find.Disabled = False
            ForwardBtn.Visible = False
            ddlRefNo.Enabled = True
        End If

        DeleteBtn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
        UnDeleteBtn.Attributes("onclick") = "javascript:return ConfirmAction('undelete');"
        ForwardBtn.Attributes("onclick") = "javascript:return ConfirmAction('move forward this transaction into next period');"

        Select Case hidPOPPH23.Value
            Case 0
                BackBtn.Attributes("onclick") = ""
                NewBtn.Attributes("onclick") = ""
                ForwardBtn.Attributes("onclick") = ""
                SaveBtn.Attributes("onclick") = ""
            Case Else
                If hidPOPPH23.Value <> hidHadCOATax.Value Then
                    BackBtn.Attributes("onclick") = "javascript:return ConfirmAction('exit while current transaction has PPh 23 on PO document');"
                    NewBtn.Attributes("onclick") = "javascript:return ConfirmAction('create new transaction while current transaction has PPh 23 on PO document');"
                    ForwardBtn.Attributes("onclick") = "javascript:return ConfirmAction('move forward while current transaction has PPh 23 on PO document');"
                    VerifiedBtn.Attributes("onclick") = "javascript:return ConfirmAction('verified while current transaction has PPh 23 on PO document');"
                End If
        End Select
        Select Case hidPOPPH21.Value
            Case 0
                BackBtn.Attributes("onclick") = ""
                NewBtn.Attributes("onclick") = ""
                ForwardBtn.Attributes("onclick") = ""
                SaveBtn.Attributes("onclick") = ""
            Case Else
                If hidPOPPH21.Value <> hidHadCOATax.Value Then
                    BackBtn.Attributes("onclick") = "javascript:return ConfirmAction('exit while current transaction has PPh 21 on PO document');"
                    NewBtn.Attributes("onclick") = "javascript:return ConfirmAction('create new transaction while current transaction has PPh 21 on PO document');"
                    ForwardBtn.Attributes("onclick") = "javascript:return ConfirmAction('move forward while current transaction has PPh 21 on PO document');"
                    VerifiedBtn.Attributes("onclick") = "javascript:return ConfirmAction('verified while current transaction has PPh 21 on PO document');"
                End If
        End Select

        'Dim strNowDate As String = Date_Validation(Date_Validation(Now(), True), False)
        'If lblPaymentID.Text <> "" And Left(lblPaymentID.Text.Trim, 3) <> "XXX" Then
        '    If Year(strDate) & Format(Month(strDate), "00") & Format(Day(strDate), "00") < Year(strNowDate) & Format(Month(strNowDate), "00") & Format(Day(strNowDate), "00") Then
        '        txtDateCreated.Enabled = False
        '    End If
        'End If

        


        If lblPaymentID.Text.Trim() <> "" Then
            'ddlPayType.Enabled = False
            'txtDateCreated.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
        End If
    End Sub

    Sub onLoad_Display(ByVal pv_strPaymentId As String)
        Dim strOpCd_Get As String = "CB_CLSTRX_PAYMENT_DETAILS_GET"
        Dim objPayDs As New Object()
        Dim intErrNo As Integer
        Dim strParam As String = pv_strPaymentId
        Dim intCnt As Integer = 0
        Dim PrevCBID As String
        Dim PrevPYID As String
        Dim PrevCBDate As Integer
        Dim PrevPYDate As Integer
        Dim NextCBID As String
        Dim NextPYID As String
        Dim NextCBDate As Integer
        Dim NextPYDate As Integer

        payid.Value = pv_strPaymentId

        Try
            intErrNo = objCBTrx.mtdGetPayment(strOpCd_Get, _
                                              strCompany, _
                                              strLocation, _
                                              strUserId, _
                                              strAccMonth, _
                                              strAccYear, _
                                              strParam, _
                                              objPayDs, _
                                              True)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_GET_HEADER&errmesg=" & Exp.ToString() & "&redirect=ap/trx/cb_trx_paylist.aspx")
        End Try

        lblPaymentID.Text = pv_strPaymentId
        strSelectedPayID = lblPaymentID.Text
        txtChequeNo.Text = objPayDs.Tables(0).Rows(0).Item("ChequeNo").Trim()
        lblAccPeriod.Text = objPayDs.Tables(0).Rows(0).Item("AccMonth") & "/" & objPayDs.Tables(0).Rows(0).Item("AccYear")
        'MsgBox(Err.Description)
        lblStatus.Text = objCBTrx.mtdGetPaymentStatus(Trim(objPayDs.Tables(0).Rows(0).Item("Status")))
        intPayStatus = Convert.ToInt16(objPayDs.Tables(0).Rows(0).Item("Status"))

        lblStatusHidden.Text = intPayStatus

        lblDateCreated.Text = objGlobal.GetLongDate(objPayDs.Tables(0).Rows(0).Item("CreateDate"))
        txtDateCreated.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), objPayDs.Tables(0).Rows(0).Item("CreateDate"))
        lblLastUpdate.Text = objGlobal.GetLongDate(objPayDs.Tables(0).Rows(0).Item("UpdateDate"))
        lblChequePrintDate.Text = objGlobal.GetLongDate(objPayDs.Tables(0).Rows(0).Item("ChequePrintDate"))
        lblPrintDate.Text = objGlobal.GetLongDate(objPayDs.Tables(0).Rows(0).Item("ChequePrintDate"))
        lblUpdatedBy.Text = objPayDs.Tables(0).Rows(0).Item("UserName")

        lblTotalPaymentAmount.Text = FormatNumber(objPayDs.Tables(0).Rows(0).Item("TotalAmount"), 2, True, False, False)
        lblCurrency.Text = Trim(objPayDs.Tables(0).Rows(0).Item("CurrencyCode"))
        lblViewTotalPaymentAmount.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(objPayDs.Tables(0).Rows(0).Item("TotalAmountToDisplay"), CInt(Session("SS_ROUNDNO"))), CInt(Session("SS_ROUNDNO"))) 'objGlobal.GetIDDecimalSeparator(FormatNumber(objPayDs.Tables(0).Rows(0).Item("TotalAmountToDisplay"), 2, True, False, False))
        txtRemark.Text = objPayDs.Tables(0).Rows(0).Item("Remark").Trim()
        lblPayType.Text = objPayDs.Tables(0).Rows(0).Item("PaymentType").Trim()
        strSuppName = objPayDs.Tables(0).Rows(0).Item("SupplierName").Trim()
        txtSplBankAccNo.Text = objPayDs.Tables(0).Rows(0).Item("SplBankAccNo").Trim()
        txtGiroDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), objPayDs.Tables(0).Rows(0).Item("PrintDate"))
        lblShowTotalAmount.Text = "( Rp." & objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(objPayDs.Tables(0).Rows(0).Item("TotalAmount"), CInt(Session("SS_ROUNDNO"))), CInt(Session("SS_ROUNDNO"))) & " )" ' "( Rp." & objGlobal.GetIDDecimalSeparator(FormatNumber(objPayDs.Tables(0).Rows(0).Item("TotalAmount"), 2, True, False, False)) & " )"
        ddlPayType.SelectedValue = objPayDs.Tables(0).Rows(0).Item("PaymentType").Trim()
        lblCurrentPeriod.Text = Trim(objPayDs.Tables(0).Rows(0).Item("AccYear")) & Trim(objPayDs.Tables(0).Rows(0).Item("AccMonth"))
        hidUserID.Value = Trim(objPayDs.Tables(0).Rows(0).Item("UpdateID"))
        hidTaxStatus.Value = objPayDs.Tables(0).Rows(0).Item("TaxStatus")
        hidNPWPNo.Value = Trim(objPayDs.Tables(0).Rows(0).Item("NPWPNo"))
        BindSplBankAccNo(Trim(objPayDs.Tables(0).Rows(0).Item("Suppliercode")), Trim(objPayDs.Tables(0).Rows(0).Item("SplBankAccNo")))
        BindRefNo(Trim(objPayDs.Tables(0).Rows(0).Item("StaffAdvRefNo")))
        BindGiroNo(objPayDs.Tables(0).Rows(0).Item("BankCode").Trim(), "")
        ddlChequeHandOver.SelectedValue = objPayDs.Tables(0).Rows(0).Item("ChequeHandOver").Trim()

        If Trim(objPayDs.Tables(0).Rows(0).Item("ChequeCash")) = "1" Then
            chkChequeCash.Checked = True
        Else
            chkChequeCash.Checked = False
        End If

        If Trim(objPayDs.Tables(0).Rows(0).Item("TaxUpdateID")) <> "" Then
            lblTaxUpdate.Visible = True
            lblTaxUpdateDesc.Visible = True
            lblTaxUpdateDesc.Text = Trim(objPayDs.Tables(0).Rows(0).Item("TaxUpdateID"))
        Else
            lblTaxUpdate.Visible = False
            lblTaxUpdateDesc.Visible = False
        End If

        PrevCBID = Trim(objPayDs.Tables(0).Rows(0).Item("PrevCBID"))
        PrevPYID = Trim(objPayDs.Tables(0).Rows(0).Item("PrevPYID"))
        PrevCBDate = Day(objGlobal.GetLongDate(Trim(objPayDs.Tables(0).Rows(0).Item("PrevCBDate"))))
        PrevPYDate = Day(objGlobal.GetLongDate(Trim(objPayDs.Tables(0).Rows(0).Item("PrevPYDate"))))
        NextCBID = Trim(objPayDs.Tables(0).Rows(0).Item("NextCBID"))
        NextPYID = Trim(objPayDs.Tables(0).Rows(0).Item("NextPYID"))
        NextCBDate = Day(objGlobal.GetLongDate(Trim(objPayDs.Tables(0).Rows(0).Item("NextCBDate"))))
        NextPYDate = Day(objGlobal.GetLongDate(Trim(objPayDs.Tables(0).Rows(0).Item("NextPYDate"))))

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

        If UCase(Trim(objPayDs.Tables(0).Rows(0).Item("CurrencyCode"))) <> "IDR" Then
            lblShowTotalAmount.Visible = True
        Else
            lblShowTotalAmount.Visible = False
        End If

        'BindSupplier(objPayDs.Tables(0).Rows(0).Item("SupplierCode").Trim())
        txtSupCode.Text = objPayDs.Tables(0).Rows(0).Item("SupplierCode").Trim()
        txtSupName.Text = objPayDs.Tables(0).Rows(0).Item("SupplierName").Trim()
        BindPayType(objPayDs.Tables(0).Rows(0).Item("PaymentType").Trim())
        BindBankCode(objPayDs.Tables(0).Rows(0).Item("BankCode").Trim())
        BindInvoiceRcv("")
        BindDebitNote("")
        BindCreditNote("")
        BindCreditorJournal("")
        'BindAccCode("")
        BindPOID("")
        BindTaxObjectList("", "")
    End Sub

    Sub onLoad_DisplayLine(ByVal pv_strPaymentId As String)
        Dim strOpCd_GetLine As String = "CB_CLSTRX_PAYMENT_LINE_GET"
        Dim strParam As String = pv_strPaymentId
        Dim lbButton As LinkButton
        Dim lbEditButton As LinkButton
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim lbl As Label
        Dim strCOA As String
        Dim strDocType As String

        Try
            intErrNo = objCBTrx.mtdGetPaymentLine(strOpCd_GetLine, strParam, objPayLnDs)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_GET_LINE&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_paylist.aspx")
        End Try

        dgLineDet.DataSource = objPayLnDs.Tables(0)
        dgLineDet.DataBind()

        hidHadCOATax.Value = 0
        hidPOPPH23.Value = 0
        hidPOPPH21.Value = 0

        For intCnt = 0 To objPayLnDs.Tables(0).Rows.Count - 1
            lbl = dgLineDet.Items.Item(intCnt).FindControl("lblEnumDocType")
            strDocType = Trim(lbl.Text)
            If Trim(lbl.Text) = objCBTrx.EnumPaymentDocType.InvoiceReceive Then
                lbl = dgLineDet.Items.Item(intCnt).FindControl("lblDocType")
                lbl.Text = lblInvoiceRcvTag.Text
            End If
            lblPPHRateHidden.Text = objPayLnDs.Tables(0).Rows(intCnt).Item("PPHRate")
            lblPPNHidden.Text = objPayLnDs.Tables(0).Rows(0).Item("PPN")
            Select Case intPayStatus
                Case objCBTrx.EnumPaymentStatus.Active
                    lbButton = dgLineDet.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = True
                    lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                    lbEditButton = dgLineDet.Items.Item(intCnt).FindControl("lbEdit")
                    lbEditButton.Visible = True
                Case objCBTrx.EnumPaymentStatus.Verified
                    lbl = dgLineDet.Items.Item(intCnt).FindControl("lblAccCode")
                    strCOA = lbl.Text.Trim
                    If intLevel = 0 Then
                        lbButton = dgLineDet.Items.Item(intCnt).FindControl("lbDelete")
                        lbButton.Visible = False
                        lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                        lbEditButton = dgLineDet.Items.Item(intCnt).FindControl("lbEdit")
                        lbEditButton.Visible = False
                        If strDocType = objCBTrx.EnumPaymentDocType.Payment Then
                            If Left(Trim(strCOA), 3) = "65." Or Left(Trim(strCOA), 5) = "71.19" Or Left(Trim(strCOA), 3) = "110" Then 'can edit/delete by FIN user
                                lbButton = dgLineDet.Items.Item(intCnt).FindControl("lbDelete")
                                lbButton.Visible = True
                                lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                                lbEditButton = dgLineDet.Items.Item(intCnt).FindControl("lbEdit")
                                lbEditButton.Visible = True
                            End If
                        End If
                    Else
                        lbButton = dgLineDet.Items.Item(intCnt).FindControl("lbDelete")
                        lbButton.Visible = True
                        lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                        lbEditButton = dgLineDet.Items.Item(intCnt).FindControl("lbEdit")
                        lbEditButton.Visible = True
                    End If
                Case Else
                    lbButton = dgLineDet.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False
            End Select

            'cek coa tax
            lbl = dgLineDet.Items.Item(intCnt).FindControl("lblTaxLnID")
            If lbl.Text.Trim <> "" Then
                hidHadCOATax.Value += 1
            End If

            'cek payment contain PO PPH23
            lbl = dgLineDet.Items.Item(intCnt).FindControl("lblPO_PPH23")
            If CDbl(lbl.Text) <> 0 Then
                hidPOPPH23.Value += 1
            End If
            'cek payment contain PO PPH21
            lbl = dgLineDet.Items.Item(intCnt).FindControl("lblPO_PPH21")
            If CDbl(lbl.Text) <> 0 Then
                hidPOPPH21.Value += 1
            End If
        Next

        'display coa status
        If hidHadCOATax.Value > 0 Then
            lblTaxStatus.Visible = True
            lblTaxStatusDesc.Text = objCBTrx.mtdGetTaxStatus(Trim(hidTaxStatus.Value))
        Else
            lblTaxStatus.Visible = False
            lblTaxStatusDesc.Visible = False
        End If

        If objPayLnDs.Tables(0).Rows.Count = 0 Then
            lblPPHRateHidden.Text = ""
            lblPPNHidden.Text = ""
        End If

        If objPayLnDs.Tables(0).Rows.Count > 0 Then
            TrLink.Visible = True
        Else
            TrLink.Visible = False
        End If

        If objPayLnDs.Tables(0).Rows.Count > 0 Then
            'BindCurrCodeList(objPayLnDs.Tables(0).Rows(0).Item("CBCurrencyCode"))
            ddlCurrCode.SelectedValue = Trim(objPayLnDs.Tables(0).Rows(0).Item("CBCurrencyCode"))
            txtExchangeRate.Text = objPayLnDs.Tables(0).Rows(0).Item("CBExchangeRate")
        End If
    End Sub

    'Sub BindSupplier(ByVal pv_strSupplierId As String)
    '    Dim strOpCode_GetSupp As String = "PU_CLSSETUP_SUPPLIER_GET"
    '    Dim strParam As String
    '    Dim intErrNo As Integer
    '    Dim intCnt As Integer
    '    Dim intSelectedSuppIndex As Integer = 0

    '    strParam = "||" & objPUSetup.EnumSuppStatus.Active & "||SupplierCode||"
    '    'strParam = strParam & "|" & IIf(Session("SS_COACENTRALIZED") = "1", "", " A.AccCode in (SELECT AccCode FROM GL_Account WHERE LocCode='" & Trim(strLocation) & "') ")
    '    strParam = strParam & "|" & IIf(Session("SS_COACENTRALIZED") = "1", "", " SupplierCode LIKE '%" & Trim(strLocation) & "%'") ', " A.AccCode in (SELECT AccCode FROM GL_Account WHERE LocCode='" & Trim(strLocation) & "') ")

    '    Try
    '        intErrNo = objPUSetup.mtdGetSupplier(strOpCode_GetSupp, strParam, objSuppDs)
    '    Catch Exp As Exception
    '        Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_GET_SUPPCODE&errmesg=" & Exp.ToString() & "&redirect=CB/trx/cb_trx_paylist.aspx")
    '    End Try

    '    'For intCnt = 0 To objSuppDs.Tables(0).Rows.Count - 1
    '    '    objSuppDs.Tables(0).Rows(intCnt).Item("SupplierCode") = objSuppDs.Tables(0).Rows(intCnt).Item("SupplierCode").Trim()
    '    '    objSuppDs.Tables(0).Rows(intCnt).Item("Name") = objSuppDs.Tables(0).Rows(intCnt).Item("SupplierCode") & " (" & objSuppDs.Tables(0).Rows(intCnt).Item("Name").Trim() & ")"
    '    '    If objSuppDs.Tables(0).Rows(intCnt).Item("SupplierCode") = pv_strSupplierId Then
    '    '        intSelectedSuppIndex = intCnt + 1
    '    '    End If
    '    'Next intCnt

    '    Dim dr As DataRow
    '    dr = objSuppDs.Tables(0).NewRow()
    '    dr("SupplierCode") = ""
    '    dr("CodeDescr") = lblPleaseSelect.Text & " Supplier" & lblCode.Text
    '    objSuppDs.Tables(0).Rows.InsertAt(dr, 0)

    '    ddlSupplier.DataSource = objSuppDs.Tables(0)
    '    ddlSupplier.DataValueField = "SupplierCode"
    '    ddlSupplier.DataTextField = "CodeDescr"
    '    ddlSupplier.DataBind()
    '    'ddlSupplier.SelectedIndex = intSelectedSuppIndex
    '    ddlSupplier.AutoPostBack = True
    'End Sub

    Sub BindSplBankAccNo(ByVal pv_strSplCode As String, Optional ByVal pv_strSplBankAccNo As String = "")
        Dim strOpCd As String = "PU_CLSSETUP_SUPPLIERBANK_GET"
        Dim intErrNo As Integer
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=TX_Setup_TaxObjectDet&errmesg=" & Exp.ToString() & "&redirect=")
        End Try

        For intCnt = 0 To dsMaster.Tables(0).Rows.Count - 1
            dsMaster.Tables(0).Rows(intCnt).Item("BankAccNo") = dsMaster.Tables(0).Rows(intCnt).Item("BankAccNo").Trim()
            dsMaster.Tables(0).Rows(intCnt).Item("SplBankDescr") = dsMaster.Tables(0).Rows(intCnt).Item("SplBankDescr")
            If Trim(dsMaster.Tables(0).Rows(intCnt).Item("BankAccNo")) = Trim(pv_strSplBankAccNo) Then
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
    End Sub

    Sub onSelect_SplBankAccNo(ByVal Sender As System.Object, ByVal E As System.EventArgs)
        Dim arrParam As Array
        arrParam = Split(ddlSplBankAccNo.SelectedItem.Text, "(")

        'txtSplBankAccNo.Text = Replace(Trim(arrParam(1)), ")", "")
    End Sub

    Sub BindPayType(ByVal pv_strPayType As String)
        'If pv_strPayType = "1" Then
        '    ddlPayType.SelectedIndex = 1
        '    ddlBank.SelectedIndex = 0
        '    ddlBank.Enabled = False
        '    txtChequeNo.Text = ""
        '    txtChequeNo.Enabled = False
        'ElseIf pv_strPayType = "0" Then
        '    ddlPayType.SelectedIndex = 0
        '    ddlBank.Enabled = True
        '    txtChequeNo.Enabled = True
        'ElseIf pv_strPayType = "2" Then
        '    ddlPayType.SelectedIndex = 2
        '    ddlBank.Enabled = True
        '    txtChequeNo.Enabled = True
        'ElseIf pv_strPayType = "3" Then
        '    ddlPayType.SelectedIndex = 3
        '    ddlBank.Enabled = True
        '    txtChequeNo.Enabled = True
        'End If
        'onLoad_Button()
    End Sub

    Sub BindBankCode(ByVal pv_strBankCode As String)
        Dim strOpCode As String = "HR_CLSSETUP_BANK_LIST_GET"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedBankIndex As Integer = 0
        Dim strDate As String = Date_Validation(txtDateCreated.Text, False)
        Dim strOpCodeBank As String

        ''strParam = "| And (left(rtrim(acccode),3)='110' or left(rtrim(acccode),6)='111.31')"
        'strParam = "| And AccCode IN (SELECT AccCode FROM GL_ACCOUNT WHERE COALevel='2' AND AccType='1' AND AccCode NOT LIKE 'DUMMY%') "
        'strParam = strParam & IIf(Session("SS_COACENTRALIZED") = "1", "", " And AccCode in (SELECT AccCode FROM GL_Account WHERE LocCode='" & Trim(strLocation) & "') ")

        'Try
        '    intErrNo = objHRSetup.mtdGetMasterList(strOpCode, _
        '                                           strParam, _
        '                                           objHRSetup.EnumHRMasterType.Bank, _
        '                                           objBankDs)
        'Catch Exp As Exception
        '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_GET_BANK&errmesg=" & Exp.ToString() & "&redirect=CB/trx/cb_trx_paylist.aspx")
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

    Sub BindInvoiceRcv(ByVal pv_strInvoiceRcvId As String)
        Dim strOpCode As String = "CB_CLSTRX_PAYMENT_INVOICERCV_BYSUPP_GET"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim dr As DataRow
        Dim intSelectedIRIndex As Integer = 0

        strParam = txtSupCode.Text & "|" & objCBTrx.EnumInvoiceRcvStatus.Confirmed & "','" & objCBTrx.EnumInvoiceRcvStatus.Closed & "','" & objCBTrx.EnumInvoiceRcvStatus.Paid

        Try
            intErrNo = objCBTrx.mtdGetPayment_Document(strOpCode, _
                                                       strCompany, _
                                                       strLocation, _
                                                       strUserId, _
                                                       strAccMonth, _
                                                       strAccYear, _
                                                       strParam, _
                                                       objCBTrx.EnumPaymentDocType.InvoiceReceive, _
                                                       objInvRcvDs)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_GET_INVOICERCV&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_paylist.aspx")
        End Try

        For intCnt = 0 To objInvRcvDs.Tables(0).Rows.Count - 1
            If CDbl(objInvRcvDs.Tables(0).Rows(intCnt).Item("OutstandingAmountToDisplay")) <> 0 Then
                objInvRcvDs.Tables(0).Rows(intCnt).Item("InvoiceRcvID") = objInvRcvDs.Tables(0).Rows(intCnt).Item("InvoiceRcvID").Trim()
                objInvRcvDs.Tables(0).Rows(intCnt).Item("DispInvoiceRcvID") = objInvRcvDs.Tables(0).Rows(intCnt).Item("InvoiceRcvID") & ", " & objInvRcvDs.Tables(0).Rows(intCnt).Item("CurrencyCode") & " " & objInvRcvDs.Tables(0).Rows(intCnt).Item("OutstandingAmountToDisplay") & ", " & objInvRcvDs.Tables(0).Rows(intCnt).Item("Kurs")

                If pv_strInvoiceRcvId = objInvRcvDs.Tables(0).Rows(intCnt).Item("InvoiceRcvID") Then
                    intSelectedIRIndex = intCnt + 1
                End If
            End If
        Next intCnt

        dr = objInvRcvDs.Tables(0).NewRow()
        dr("InvoiceRcvId") = ""
        dr("DispInvoiceRcvID") = lblPleaseSelect.Text & lblInvoiceRcvTag.Text
        objInvRcvDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlInvoiceRcv.DataSource = objInvRcvDs.Tables(0)
        ddlInvoiceRcv.DataValueField = "InvoiceRcvID"
        ddlInvoiceRcv.DataTextField = "DispInvoiceRcvID"
        ddlInvoiceRcv.DataBind()
        ddlInvoiceRcv.SelectedIndex = intSelectedIRIndex
    End Sub

    Sub BindDebitNote(ByVal pv_strDebitNoteId As String)
        Dim strOpCode As String = "CB_CLSTRX_PAYMENT_DEBITNOTE_BYSUPP_GET"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedDNIndex As Integer = 0
        Dim dr As DataRow

        strParam = txtSupCode.Text & "|" & Convert.ToString(objCBTrx.EnumDebitNoteStatus.Confirmed) & "','" & Convert.ToString(objCBTrx.EnumDebitNoteStatus.Closed) & "','" & Convert.ToString(objCBTrx.EnumDebitNoteStatus.Paid)

        Try
            intErrNo = objCBTrx.mtdGetPayment_Document(strOpCode, _
                                                       strCompany, _
                                                       strLocation, _
                                                       strUserId, _
                                                       strAccMonth, _
                                                       strAccYear, _
                                                       strParam, _
                                                       objCBTrx.EnumPaymentDocType.DebitNote, _
                                                       objDebitNoteDs)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_GET_DEBITNOTE&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_paylist.aspx")
        End Try

        For intCnt = 0 To objDebitNoteDs.Tables(0).Rows.Count - 1
            objDebitNoteDs.Tables(0).Rows(intCnt).Item("DebitNoteId") = objDebitNoteDs.Tables(0).Rows(intCnt).Item("DebitNoteId").Trim()
            objDebitNoteDs.Tables(0).Rows(intCnt).Item("DispDebitNoteId") = objDebitNoteDs.Tables(0).Rows(intCnt).Item("DebitNoteId") & ", Rp " & objDebitNoteDs.Tables(0).Rows(intCnt).Item("OutstandingAmount")
            If pv_strDebitNoteId = objDebitNoteDs.Tables(0).Rows(intCnt).Item("DebitNoteId") Then
                intSelectedDNIndex = intCnt + 1
            End If
        Next intCnt

        dr = objDebitNoteDs.Tables(0).NewRow()
        dr("DebitNoteID") = ""
        dr("DispDebitNoteId") = lblPleaseSelect.Text & " Debit Note ID"
        objDebitNoteDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlDebitNote.DataSource = objDebitNoteDs.Tables(0)
        ddlDebitNote.DataValueField = "DebitNoteID"
        ddlDebitNote.DataTextField = "DispDebitNoteId"
        ddlDebitNote.DataBind()
        ddlDebitNote.SelectedIndex = intSelectedDNIndex
    End Sub

    Sub BindCreditNote(ByVal pv_strCreditNoteId As String)
        Dim strOpCode As String = "CB_CLSTRX_PAYMENT_CREDITNOTE_BYSUPP_GET"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedCNIndex As Integer = 0
        Dim dr As DataRow

        strParam = txtSupCode.Text & "|" & Convert.ToString(objCBTrx.EnumCreditNoteStatus.Confirmed) & "','" & Convert.ToString(objCBTrx.EnumCreditNoteStatus.Closed) & "','" & Convert.ToString(objCBTrx.EnumCreditNoteStatus.Paid)

        Try
            intErrNo = objCBTrx.mtdGetPayment_Document(strOpCode, _
                                                       strCompany, _
                                                       strLocation, _
                                                       strUserId, _
                                                       strAccMonth, _
                                                       strAccYear, _
                                                       strParam, _
                                                       objCBTrx.EnumPaymentDocType.CreditNote, _
                                                       objCreditNoteDs)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_GET_CREDITNOTE&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_paylist.aspx")
        End Try

        For intCnt = 0 To objCreditNoteDs.Tables(0).Rows.Count - 1
            objCreditNoteDs.Tables(0).Rows(intCnt).Item("CreditNoteId") = objCreditNoteDs.Tables(0).Rows(intCnt).Item("CreditNoteId").Trim()
            objCreditNoteDs.Tables(0).Rows(intCnt).Item("DispCreditNoteId") = objCreditNoteDs.Tables(0).Rows(intCnt).Item("CreditNoteId") & ", Rp " & objCreditNoteDs.Tables(0).Rows(intCnt).Item("OutstandingAmount")
            If pv_strCreditNoteId = objCreditNoteDs.Tables(0).Rows(intCnt).Item("CreditNoteId") Then
                intSelectedCNIndex = intCnt + 1
            End If
        Next intCnt

        dr = objCreditNoteDs.Tables(0).NewRow()
        dr("CreditNoteId") = ""
        dr("DispCreditNoteId") = lblPleaseSelect.Text & " Credit Note ID"
        objCreditNoteDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlCreditNote.DataSource = objCreditNoteDs.Tables(0)
        ddlCreditNote.DataValueField = "CreditNoteId"
        ddlCreditNote.DataTextField = "DispCreditNoteId"
        ddlCreditNote.DataBind()
        ddlCreditNote.SelectedIndex = intSelectedCNIndex
    End Sub

    Private Sub BindCreditorJournal(ByVal pv_strCreditorJournalId As String)
        Dim strOpCode As String = "CB_CLSTRX_PAYMENT_CREDITORJOURNAL_BYSUPP_GET"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer = 0
        Dim dr As DataRow

        strParam = txtSupCode.Text & "|" & CStr(objCBTrx.EnumCreditorJournalStatus.Confirmed) & "','" & _
                   CStr(objCBTrx.EnumCreditorJournalStatus.Closed) & "|" & _
                   CStr(objCBTrx.EnumCreditorJournalType.Adjustment) & "|" & _
                   "AND OutstandingAmount <> 0"

        Try
            intErrNo = objCBTrx.mtdGetPayment_Document(strOpCode, strCompany, strLocation, strUserId, strAccMonth, strAccYear, _
                                                       strParam, objCBTrx.EnumPaymentDocType.CreditorJournal, objCreditorJournalDs)

        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_GET_CREDITORJOURNAL&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_paylist.aspx")
        End Try

        With objCreditorJournalDs.Tables(0)
            For intCnt = 0 To .Rows.Count - 1
                .Rows(intCnt).Item("CreditJrnID") = .Rows(intCnt).Item("CreditJrnID").Trim()
                .Rows(intCnt).Item("DispCreditJrnID") = .Rows(intCnt).Item("CreditJrnID") & ", Rp " & .Rows(intCnt).Item("OutstandingAmount")
                If pv_strCreditorJournalId = .Rows(intCnt).Item("CreditJrnID") Then
                    intSelectedIndex = intCnt + 1
                End If
            Next intCnt
        End With

        dr = objCreditorJournalDs.Tables(0).NewRow()
        dr("CreditJrnID") = ""
        dr("DispCreditJrnID") = lblPleaseSelect.Text & " Creditor Journal ID"
        objCreditorJournalDs.Tables(0).Rows.InsertAt(dr, 0)

        With ddlCreditorJournal
            .DataSource = objCreditorJournalDs.Tables(0)
            .DataValueField = "CreditJrnID"
            .DataTextField = "DispCreditJrnID"
            .DataBind()
            .SelectedIndex = intSelectedIndex
        End With
    End Sub

    'Sub BindOtherAccCode(ByVal pv_strAccCode As String)
    '    Dim strOpCode_GetAcc As String = "GL_CLSSETUP_ACCOUNTCODE_LIST_GET"
    '    Dim strParam As String
    '    Dim intErrNo As Integer
    '    Dim intCnt As Integer
    '    Dim intSelectedIndex As Integer = 0



    '    Try

    '        If LblIsSKBActive.Text = 1 And (Format(txtDateCreated.Text, "dd/MM/yyyy") >= Format(lblSKBStartDate.Text, "dd/MM/yyyy")) Then
    '            strParam = "Order By ACC.AccCode|And ACC.Status = '" & objGLSetup.EnumAccountCodeStatus.Active & "' AND ACC.AccCode NOT IN (SELECT AccCode FROM dbo.TX_TAXOBJECTRATELN) "
    '        Else
    '            strParam = "Order By ACC.AccCode|And ACC.Status = '" & objGLSetup.EnumAccountCodeStatus.Active & "' "
    '        End If

    '        strParam = strParam & IIf(Session("SS_COACENTRALIZED") = "1", "", " AND LocCode = '" & Trim(strLocation) & "' ")
    '        intErrNo = objGLSetup.mtdGetMasterList(strOpCode_GetAcc, strParam, objGLSetup.EnumGLMasterType.AccountCode, objAccOthDs)

    '    Catch Exp As Exception
    '        Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_GET_SUPP2&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_paylist.aspx")
    '    End Try

    '    For intCnt = 0 To objAccOthDs.Tables(0).Rows.Count - 1
    '        If objAccOthDs.Tables(0).Rows(intCnt).Item("AccCode") = pv_strAccCode Then
    '            intSelectedIndex = intCnt + 1
    '            Exit For
    '        End If
    '    Next intCnt


    '    Dim dr As DataRow
    '    dr = objAccOthDs.Tables(0).NewRow()
    '    dr("AccCode") = ""
    '    dr("_Description") = lblPleaseSelect.Text & "Other Cost"
    '    objAccOthDs.Tables(0).Rows.InsertAt(dr, 0)
    '    ddlOther()
    '    ddlOther.DataSource = objAccOthDs.Tables(0)
    '    ddlOther.DataValueField = "AccCode"
    '    ddlOther.DataTextField = "_Description"
    '    ddlOther.DataBind()
    '    ddlOther.SelectedIndex = intSelectedIndex

    '    onLoad_Button()
    'End Sub

    Sub BindBankAccCode(ByVal pv_strAccCode As String)
        'Dim strOpCode_GetAcc As String = "GL_CLSSETUP_ACCOUNTCODE_LIST_GET"
        Dim strOpCode As String = "HR_CLSSETUP_BANK_ACCCODE_GET"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer = 0
        Dim defaultAccCode As String

        Try
            strParam = ddlBank.SelectedItem.Value
            intErrNo = objHRSetup.mtdGetAccCode(strOpCode, strParam, objAccDs)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_GET_BANKACCCODE&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_paylist.aspx")
        End Try

        For intCnt = 0 To objAccDs.Tables(0).Rows.Count - 1
            defaultAccCode = Trim(objAccDs.Tables(0).Rows(intCnt).Item("AccCode"))
        Next intCnt

        objAccDs = Nothing
        intCnt = 0

        'Try
        '    strParam = "Order By ACC.AccCode|And ACC.Status = '" & objGLSetup.EnumAccountCodeStatus.Active & "' " & _
        '                " And ACC.AccType = '" & objGLSetup.EnumAccountType.BalanceSheet & "' " & _
        '               " And ACC.NurseryInd = '" & objGLSetup.EnumNurseryAccount.No & "' "
        '
        '    strParam = strParam & IIf(Session("SS_COACENTRALIZED") = "1", "", " AND LocCode = '" & Trim(strLocation) & "' ")
        '
        '   intErrNo = objGLSetup.mtdGetMasterList(strOpCode_GetAcc, strParam, objGLSetup.EnumGLMasterType.AccountCode, objAccDs)
        'Catch Exp As Exception
        '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_GET_SUPP2&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_paylist.aspx")
        'End Try

        'For intCnt = 0 To objAccDs.Tables(0).Rows.Count - 1
        '    If (objAccDs.Tables(0).Rows(intCnt).Item("AccCode") = pv_strAccCode Or objAccDs.Tables(0).Rows(intCnt).Item("AccCode") = defaultAccCode) Then
        '        intSelectedIndex = intCnt + 1
        '        Exit For
        '    End If
        'Next intCnt

        GetCOADetail(defaultAccCode)

        'Dim dr As DataRow
        'dr = objAccDs.Tables(0).NewRow()
        'dr("AccCode") = ""
        'dr("_Description") = lblPleaseSelect.Text & lblAccount.Text
        'objAccDs.Tables(0).Rows.InsertAt(dr, 0)

        'ddlAccCode.DataSource = objAccDs.Tables(0)
        'ddlAccCode.DataValueField = "AccCode"
        'ddlAccCode.DataTextField = "_Description"
        'ddlAccCode.DataBind()

        'ddlAccCode.SelectedIndex = intSelectedIndex

        'onLoad_Button()
    End Sub

    'Sub BindAccCode(ByVal pv_strAccCode As String)
    '    Dim strOpCode_GetAcc As String = "GL_CLSSETUP_ACCOUNTCODE_LIST_GET"
    '    Dim strOpCode As String = "HR_CLSSETUP_BANK_ACCCODE_GET"
    '    Dim strParam As String
    '    Dim intErrNo As Integer
    '    Dim intCnt As Integer
    '    Dim intSelectedIndex As Integer = 0
    '    Dim defaultAccCode As String

    '    Try
    '        strParam = ddlBank.SelectedItem.Value
    '        intErrNo = objHRSetup.mtdGetAccCode(strOpCode, strParam, objAccDs)
    '    Catch Exp As Exception
    '        Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_GET_BANKACCCODE&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_paylist.aspx")
    '    End Try

    '    For intCnt = 0 To objAccDs.Tables(0).Rows.Count - 1
    '        defaultAccCode = Trim(objAccDs.Tables(0).Rows(intCnt).Item("AccCode"))
    '    Next intCnt

    '    objAccDs = Nothing
    '    intCnt = 0

    '    Try
    '        strParam = "Order By ACC.AccCode|And ACC.Status = '" & objGLSetup.EnumAccountCodeStatus.Active & "' " & _
    '                    " And ACC.AccType = '" & objGLSetup.EnumAccountType.BalanceSheet & "' " & _
    '                    " And ACC.NurseryInd = '" & objGLSetup.EnumNurseryAccount.No & "' "

    '        strParam = strParam & IIf(Session("SS_COACENTRALIZED") = "1", "", " AND LocCode = '" & Trim(strLocation) & "' ")

    '        intErrNo = objGLSetup.mtdGetMasterList(strOpCode_GetAcc, strParam, objGLSetup.EnumGLMasterType.AccountCode, objAccDs)
    '    Catch Exp As Exception
    '        Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_GET_SUPP2&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_paylist.aspx")
    '    End Try

    '    For intCnt = 0 To objAccDs.Tables(0).Rows.Count - 1
    '        If (objAccDs.Tables(0).Rows(intCnt).Item("AccCode") = pv_strAccCode Or objAccDs.Tables(0).Rows(intCnt).Item("AccCode") = defaultAccCode) Then
    '            intSelectedIndex = intCnt + 1
    '            Exit For
    '        End If
    '    Next intCnt

    '    Dim dr As DataRow
    '    dr = objAccDs.Tables(0).NewRow()
    '    dr("AccCode") = ""
    '    dr("_Description") = lblPleaseSelect.Text & lblAccount.Text
    '    objAccDs.Tables(0).Rows.InsertAt(dr, 0)

    '    'ddlAccCode.DataSource = objAccDs.Tables(0)
    '    'ddlAccCode.DataValueField = "AccCode"
    '    'ddlAccCode.DataTextField = "_Description"
    '    'ddlAccCode.DataBind()

    '    'ddlAccCode.SelectedIndex = intSelectedIndex

    '    'onLoad_Button()
    'End Sub

    Sub BindPPNH(ByVal pv_strSelectedInvRcv As String)
        Dim strOpCd_GetPPNH As String = "AP_CLSTRX_INVOICERECEIVE_PPNH_GET"
        Dim strOpCodes As String = strOpCd_GetPPNH
        Dim strParam As String = pv_strSelectedInvRcv
        Dim intErrNo As Integer

        If Trim(pv_strSelectedInvRcv) = "" Then
            Exit Sub
        End If

        Try
            intErrNo = objCBTrx.mtdGetInvRcvPPNH(strOpCodes, _
                                                 strCompany, _
                                                 strLocation, _
                                                 strUserId, _
                                                 strAccMonth, _
                                                 strAccYear, _
                                                 strParam, _
                                                 objPPNHDs)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/E rrorMessage.aspx?errcode=INVOICERCV_GET_LINE&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_invrcvlist.aspx")
        End Try

        If objPPNHDs.Tables(0).Rows.Count > 0 Then
            txtPPHRate.Text = Trim(objPPNHDs.Tables(0).Rows(0).Item("PPHRate"))
            cbPPN.Checked = IIf(objPPNHDs.Tables(0).Rows(0).Item("PPNCheck") = objCBTrx.EnumPPN.Yes, True, False)
            lblInvTypeHidden.Text = objPPNHDs.Tables(0).Rows(0).Item("InvoiceType").Trim()
            hidFindPOPPH23.Value = objPPNHDs.Tables(0).Rows(0).Item("PO_PPH23")
            hidFindPOPPH21.Value = objPPNHDs.Tables(0).Rows(0).Item("PO_PPH21")

            If CDbl(hidFindPOPPH23.Value) = 0 Then
                lblFindINVPOPPH23.Visible = False
            Else
                lblFindINVPOPPH23.Visible = True
            End If
            If CDbl(hidFindPOPPH21.Value) = 0 Then
                lblFindINVPOPPH21.Visible = False
            Else
                lblFindINVPOPPH21.Visible = True
            End If

            If (lblInvTypeHidden.Text <> "") Then
                If lblInvTypeHidden.Text = objCBTrx.EnumInvoiceType.SupplierPO Then
                    lblPPN.Visible = True
                    cbPPN.Enabled = False
                    cbPPN.Visible = True
                    lblPPH.Visible = False
                    lblPercen.Visible = False
                    txtPPHRate.Enabled = False
                    txtPPHRate.Visible = False
                Else
                    lblPPN.Visible = True
                    cbPPN.Enabled = False
                    cbPPN.Visible = True
                    lblPPH.Visible = True
                    lblPercen.Visible = True
                    txtPPHRate.Enabled = False
                    txtPPHRate.Visible = True
                End If
            Else
                lblPPN.Visible = False
                cbPPN.Enabled = False
                cbPPN.Visible = False
                lblPPH.Visible = False
                lblPercen.Visible = False
                txtPPHRate.Enabled = False
                txtPPHRate.Visible = False
            End If
        End If
    End Sub

    Sub onSelect_PayType(ByVal Sender As System.Object, ByVal E As System.EventArgs)
        'BindPayType(ddlPayType.SelectedItem.Value)
        BindGiroNo(ddlBank.SelectedItem.Value, "")
    End Sub

    Sub onSelect_Change(ByVal Sender As System.Object, ByVal E As System.EventArgs)

        BindInvoiceRcv("")
        BindDebitNote("")
        BindCreditNote("")
        'BindAccCode("")
        BindCreditorJournal("")
        BindPOID("")

        Dim strOpCode As String = "PU_CLSSETUP_SUPPLIER_GET"
        Dim intErrNo As Integer
        Dim dsMaster As Object

        strParamName = "STRSEARCH"
        If txtSupCode.Text = "" Then
            strParamValue = ""
        Else
            strParamValue = " And A.SupplierCode Like '" & Trim(txtSupCode.Text) & "%'"
        End If

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
                                                strParamName, _
                                                strParamValue, _
                                                dsMaster)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_GET_SUPPCODE&errmesg=" & Exp.ToString() & "&redirect=CB/trx/cb_trx_paylist.aspx")
        End Try

        If dsMaster.Tables(0).Rows.Count > 0 Then
            txtSplBankAccNo.Text = Trim(dsMaster.Tables(0).Rows(0).Item("BankAccNo"))
            hidNPWPNo.Value = Trim(dsMaster.Tables(0).Rows(0).Item("NPWPNo"))
            LblIsSKBActive.Text = Trim(dsMaster.Tables(0).Rows(0).Item("SKBIsActivation"))
            lblSKBStartDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), dsMaster.Tables(0).Rows(0).Item("SKBDate"))
            'lblSKBStartDate.Text = dsMaster.Tables(0).Rows(0).Item("SKBDate")
            BindSplBankAccNo(Trim(txtSupCode.Text), "")
        End If
        'BindOtherAccCode("")
        onLoad_Button()

    End Sub

    Sub onSelect_InvRcv(ByVal sender As System.Object, ByVal e As System.EventArgs)
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

        BindPPNH(ddlInvoiceRcv.SelectedItem.Value)
        CheckAmount(ddlInvoiceRcv.SelectedItem.Text)
        'BindCurrencyList(pv_strCurrencyCode)
        ddlCurrency.SelectedValue = Trim(pv_strCurrencyCode)

        If ddlInvoiceRcv.SelectedIndex <> 0 And ddlBank.SelectedIndex = 0 Then
            'BindAccCode("DUMMY")
            GetCOADetail("DUMMY")
        Else
            'BindAccCode(ddlBank.SelectedItem.Value)
            BindBankAccCode(strBank)
        End If
    End Sub

    Sub onSelect_DbtNote(ByVal sender As System.Object, ByVal e As System.EventArgs)
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

        CheckAmount(ddlDebitNote.SelectedItem.Text)
        If ddlDebitNote.SelectedIndex <> 0 And ddlBank.SelectedIndex = 0 Then
            'BindAccCode("DUMMY")
            GetCOADetail("DUMMY")
        Else
            'BindAccCode(ddlBank.SelectedItem.Value)
            BindBankAccCode(strBank)
        End If
    End Sub

    Sub onSelect_CrNote(ByVal sender As System.Object, ByVal e As System.EventArgs)
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

        CheckAmount(ddlCreditNote.SelectedItem.Text)
        If ddlCreditNote.SelectedIndex <> 0 And ddlBank.SelectedIndex = 0 Then
            'BindAccCode("DUMMY")
            GetCOADetail("DUMMY")
        Else
            'BindAccCode(ddlBank.SelectedItem.Value)
            BindBankAccCode(strBank)
        End If
    End Sub

    Sub onSelect_CrJrn(ByVal sender As System.Object, ByVal e As System.EventArgs)
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

        CheckAmount(ddlCreditorJournal.SelectedItem.Text)
        If ddlCreditorJournal.SelectedIndex <> 0 And ddlBank.SelectedIndex = 0 Then
            'BindAccCode("DUMMY")
            GetCOADetail("DUMMY")
        Else
            'BindAccCode(ddlBank.SelectedItem.Value)
            BindBankAccCode(strBank)
        End If
    End Sub

    Sub onSelect_Bank(ByVal Sender As System.Object, ByVal E As System.EventArgs)
        'BindAccCode(ddlBank.SelectedItem.Value)
        BindBankAccCode(ddlBank.SelectedItem.Value)
        BindGiroNo(ddlBank.SelectedItem.Value, "")
    End Sub

    Sub GetSupplierBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        BindInvoiceRcv("")
        BindDebitNote("")
        BindCreditNote("")
        'BindAccCode("")
        BindCreditorJournal("")
        BindPOID("")

        Dim strOpCode As String = "PU_CLSSETUP_SUPPLIER_GET"
        Dim intErrNo As Integer
        Dim dsMaster As Object

        strParamName = "STRSEARCH"
        If txtSupCode.Text = "" Then
            strParamValue = ""
        Else
            strParamValue = " And A.SupplierCode Like '" & Trim(txtSupCode.Text) & "%'"
        End If

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
                                                strParamName, _
                                                strParamValue, _
                                                dsMaster)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_GET_SUPPCODE&errmesg=" & Exp.ToString() & "&redirect=CB/trx/cb_trx_paylist.aspx")
        End Try

        If dsMaster.Tables(0).Rows.Count > 0 Then
            txtSplBankAccNo.Text = Trim(dsMaster.Tables(0).Rows(0).Item("BankAccNo"))
            hidNPWPNo.Value = Trim(dsMaster.Tables(0).Rows(0).Item("NPWPNo"))
            LblIsSKBActive.Text = Trim(dsMaster.Tables(0).Rows(0).Item("SKBIsActivation"))
            lblSKBStartDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), dsMaster.Tables(0).Rows(0).Item("SKBDate"))
            'lblSKBStartDate.Text = dsMaster.Tables(0).Rows(0).Item("SKBDate")
            BindSplBankAccNo(Trim(txtSupCode.Text), "")
        End If
        'BindOtherAccCode("")
        onLoad_Button()

    End Sub

    Sub Update_Payment(ByVal pv_intStatus As Integer, ByRef pr_objNewPayID As Object, _
                       ByRef pr_objFailFulfil As Object)

        Dim strSupplierCode As String = txtSupCode.Text
        Dim strPaymentType As String = ddlPayType.SelectedItem.Value
        Dim strBankCode As String = ddlBank.SelectedItem.Value
        Dim strChequeNo As String = txtChequeNo.Text
        Dim strRemark As String = txtRemark.Text

        Dim intErrNo As Integer
        Dim strParam As String = ""
        Dim strOpCd_GetLines As String = "CB_CLSTRX_PAYMENT_LINE_GET"
        Dim strOpCd_GetIRTOTAL As String = "CB_CLSTRX_PAYMENT_INVOICERECEIVE_AMOUNT_GET"
        Dim strOpCd_UpdIROutstand As String = "CB_CLSTRX_PAYMENT_INVOICERECEIVE_OUTSTANDING_UPD"
        Dim strOpCd_GetDNTOTAL As String = "CB_CLSTRX_PAYMENT_DEBITNOTE_AMOUNT_GET"
        Dim strOpCd_UpdDNOutstand As String = "CB_CLSTRX_PAYMENT_DEBITNOTE_OUTSTANDING_UPD"
        Dim strOpCd_GetCNTOTAL As String = "CB_CLSTRX_PAYMENT_CREDITNOTE_AMOUNT_GET"
        Dim strOpCd_UpdCNOutstand As String = "CB_CLSTRX_PAYMENT_CREDITNOTE_OUTSTANDING_UPD"
        Dim strOpCd_AddPay As String = "CB_CLSTRX_PAYMENT_ADD"
        Dim strOpCd_UpdPay As String = "CB_CLSTRX_PAYMENT_UPD"
        Dim strOpCd_GetCJTOTAL As String = "CB_CLSTRX_PAYMENT_CREDITORJOURNAL_AMOUNT_GET"
        Dim strOpCd_UpdCJOutstand As String = "CB_CLSTRX_PAYMENT_CREDITORJOURNAL_OUTSTANDING_UPD"
        Dim strGiroDate As String = Date_Validation(txtGiroDate.Text, False)
        Dim strNewIDFormat As String
        Dim strInitial As String = ""
        Dim strSplBankAccNo As String = txtSplBankAccNo.Text
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

        If pv_intStatus = objCBTrx.EnumPaymentStatus.Confirmed Then
            If lblTotalPaymentAmount.Text <= 0 Then
                lblErrConfirmNotFulFil.Text = "Cannot confirm payment if total payment amount is equals to or less than zero."
                lblErrConfirmNotFulFil.Visible = True
                pr_objNewPayID = IIf(strSelectedPayID = "", pr_objNewPayID, strSelectedPayID)
                Exit Sub
            End If
        End If

        'If Month(strDate) < strAccMonth And Year(strDate) <= strAccYear Then
        '    lblDate.Visible = True
        '    lblDate.Text = "Invalid transaction date."
        '    pr_objFailFulfil = lblDate.Text
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

        If ddlPayType.SelectedItem.Value = "0" Or ddlPayType.SelectedItem.Value = "3" Then
            If ddlBank.SelectedItem.Value = "" Then
                lblErrBank.Visible = True
                Exit Sub
            End If
            'If txtGiroDate.Text = "" Then
            '    lblDateGiro.Visible = True
            '    lblDateGiro.Text = "Please enter Giro Date"
            '    Exit Sub
            'End If
            'If CheckDate(txtGiroDate.Text.Trim(), indDate) = False Then
            '    lblDateGiro.Visible = True
            '    lblFmtGiro.Visible = True
            '    lblDateGiro.Text = "<br>Date Entered should be in the format"
            '    Exit Sub
            'End If
            'If txtChequeNo.Text = "" Then
            '    lblErrCheque.Visible = True
            '    Exit Sub
            'End If

            If lblCurrentPeriod.Text <> "" Then
                If Month(strGiroDate) >= Mid(lblCurrentPeriod.Text, 5) And Year(strGiroDate) = Mid(lblCurrentPeriod.Text, 1, 4) Then
                Else
                    lblDateGiro.Visible = True
                    lblDateGiro.Text = "Invalid transaction date."
                    Exit Sub
                End If

                If Year(strGiroDate) & Format(Month(strGiroDate), "00") & Format(Day(strGiroDate), "00") < Year(strDate) & Format(Month(strDate), "00") & Format(Day(strDate), "00") Then
                    lblDateGiro.Visible = True
                    lblDateGiro.Text = "Invalid transaction date."
                    Exit Sub
                End If
            Else
                If Year(strGiroDate) & Format(Month(strGiroDate), "00") & Format(Day(strGiroDate), "00") < Year(strDate) & Format(Month(strDate), "00") & Format(Day(strDate), "00") Then
                    lblDateGiro.Visible = True
                    lblDateGiro.Text = "Invalid transaction date."
                    Exit Sub
                End If
                If Month(strGiroDate) < strAccMonth And Year(strGiroDate) <= strAccYear Then
                    lblDateGiro.Visible = True
                    lblDateGiro.Text = "Invalid transaction date."
                    Exit Sub
                End If
            End If
        End If

        If lblPaymentID.Text <> "" Then
            If pv_intStatus = objCBTrx.EnumPaymentStatus.Verified Then
                Select Case ddlPayType.SelectedItem.Value
                    Case 0
                        strInitial = "BBK"
                    Case 1
                        Select Case Trim(strBank)
                            Case "KKL", "KKK"
                                strInitial = "KKK"
                            Case Else
                                strInitial = "KRK"
                        End Select

                    Case 2
                        strInitial = "XXX"
                    Case 3
                        strInitial = "BBK"
                    Case 4
                        Select Case Trim(strBank)
                            Case "UMP"
                                strInitial = "UMP"
                            Case Else
                                strInitial = "PPL"
                        End Select
                End Select
            Else
                strInitial = "XXX"
            End If
        Else
            strInitial = "XXX"
        End If


        strAccYear = Year(strDate)
        strAccMonth = Month(strDate)

        If lblPaymentID.Text <> "" And Left(lblPaymentID.Text.Trim, 3) <> "XXX" Then
            ''remark sementara
            'If txtDateCreated.Enabled = True Then
            '    If hidPrevID.Value = "" Then
            '        If hidNextID.Value <> "" Then
            '            If Day(strDate) > hidNextDate.Value Then
            '                lblDate.Visible = True
            '                lblDate.Text = "Date cannot be higher than last transaction date."
            '                Exit Sub
            '            End If
            '        End If
            '    Else
            '        If hidPrevID.Value <> "" Then
            '            If Day(strDate) < hidPrevDate.Value Then
            '                lblDate.Visible = True
            '                lblDate.Text = "Date cannot be smaller than previous transaction date."
            '                Exit Sub
            '            End If
            '            If hidNextID.Value <> "" Then
            '                If Day(strDate) > hidNextDate.Value Then
            '                    lblDate.Visible = True
            '                    lblDate.Text = "Date cannot be higher than last transaction date."
            '                    Exit Sub
            '                End If
            '            End If
            '        Else
            '            If Day(strDate) < hidPrevDate.Value Then
            '                lblDate.Visible = True
            '                lblDate.Text = "Date cannot be smaller than previous transaction date."
            '                Exit Sub
            '            End If
            '        End If
            '    End If
            'End If

        ElseIf ddlPayType.SelectedItem.Value <> 2 Then
            ''remark sementara
            ''cek tanggal
            'Dim objLastID As New Object
            'Dim strSearchID As String
            'Dim strOpCdSearch As String
            'Dim intCBDate As Integer
            'Dim intPYDate As Integer

            'strOpCdSearch = "CB_CLSTRX_SEARCH_LASTID"
            'strSearchID = " LocCode = '" & strLocation & "' And AccMonth = '" & Trim(strAccMonth) & "' And AccYear = '" & Trim(strAccYear) & "' And Left(LTrim(PaymentID),3) = '" & Trim(strInitial) & "' "
            'strParamName = "TABLE|FIELD|STRSEARCH"
            'strParamValue = "CB_PAYMENT" & "|" & "PAYMENTID" & "|" & strSearchID

            'Try
            '    intErrNo = objGLtrx.mtdGetDataCommon(strOpCdSearch, _
            '                                        strParamName, _
            '                                        strParamValue, _
            '                                        objLastID)
            'Catch Exp As System.Exception
            '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ID_GET&errmesg=" & Exp.ToString() & "&redirect=")
            'End Try

            'If objLastID.Tables(0).Rows.Count > 0 Then
            '    intPYDate = Day(objGlobal.GetLongDate(Trim(objLastID.Tables(0).Rows(0).Item("DocDate"))))
            'Else
            '    intPYDate = 0
            'End If

            'strSearchID = " LocCode = '" & strLocation & "' And AccMonth = '" & Trim(strAccMonth) & "' And AccYear = '" & Trim(strAccYear) & "' And Left(LTrim(CashBankID),3) = '" & Trim(strInitial) & "' "
            'strParamName = "TABLE|FIELD|STRSEARCH"
            'strParamValue = "CB_CASHBANK" & "|" & "CASHBANKID" & "|" & strSearchID

            'Try
            '    intErrNo = objGLtrx.mtdGetDataCommon(strOpCdSearch, _
            '                                        strParamName, _
            '                                        strParamValue, _
            '                                        objLastID)
            'Catch Exp As System.Exception
            '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ID_GET&errmesg=" & Exp.ToString() & "&redirect=")
            'End Try

            'If objLastID.Tables(0).Rows.Count > 0 Then
            '    intCBDate = Day(objGlobal.GetLongDate(Trim(objLastID.Tables(0).Rows(0).Item("DocDate"))))
            'Else
            '    intCBDate = 0
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

        Dim strStaffAdvID As String
        Dim strStaffAdvDoc As String
        
        If ddlRefNo.SelectedItem.Value = "" Then
            strStaffAdvID = ""
            strStaffAdvDoc = ""
        Else
            arrParam = Split(Trim(ddlRefNo.SelectedItem.Value), "|")
            strStaffAdvID = Trim(arrParam(0))
            strStaffAdvDoc = Trim(arrParam(1))
        End If


        'strNewIDFormat = strInitial & "/" & strCompany & "/" & strLocation & "/" & "O" & "/" & strAccYear & IIf(Len(Trim(strAccMonth)) = 1, "0" & strAccMonth, strAccMonth) & "/"
        strNewIDFormat = strInitial & "/" & strCompany & "/" & strLocation & "/" & IIf(Len(Trim(strAccMonth)) = 1, "0" & strAccMonth, strAccMonth) & "/" & Trim(strAccYear) & "/"
        strSelectedPayID = lblPaymentID.Text

        Dim strOpCodes As String = strOpCd_GetLines & "|" & _
                                   strOpCd_GetIRTOTAL & "|" & _
                                   strOpCd_UpdIROutstand & "|" & _
                                   strOpCd_GetDNTOTAL & "|" & _
                                   strOpCd_UpdDNOutstand & "|" & _
                                   strOpCd_GetCNTOTAL & "|" & _
                                   strOpCd_UpdCNOutstand & "|" & _
                                   strOpCd_GetCJTOTAL & "|" & _
                                   strOpCd_UpdCJOutstand & "|" & _
                                   strOpCd_AddPay & "|" & _
                                   strOpCd_UpdPay

        strParam = strParam & objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CBPayment) & "|" & _
                                strSelectedPayID & "|" & _
                                strSupplierCode & "|" & _
                                strPaymentType & "|" & _
                                strBank & "|" & _
                                strChequeNo & "|" & _
                                strRemark & "|" & _
                                pv_intStatus & "|" & _
                                strDate & "|" & _
                                strNewIDFormat & "|" & _
                                ddlSplBankAccNo.SelectedItem.Value & "|" & _
                                strGiroDate & "|" & _
                                CInt(hidTaxStatus.Value) & "|" & _
                                strBankAccNo & "|" & _
                                Trim(strStaffAdvID) & "|" & _
                                Trim(strStaffAdvDoc) & "|" & _
                                IIf(chkChequeCash.Checked = True, "1", "2") & "|" & _
                                ddlChequeHandOver.SelectedItem.Value

        Try
            intErrNo = objCBTrx.mtdUpdPayment(strOpCodes, strCompany, strLocation, strUserId, _
                                              strAccMonth, strAccYear, strParam, _
                                              pr_objNewPayID, pr_objFailFulfil)

        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_UPD_DATA&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_paylist.aspx")
        End Try

        If pr_objFailFulfil <> "" Then
            lblErrConfirmNotFulFil.Text = lblErrConfirmNotFulFilText.Text & pr_objFailFulfil & "."
            lblErrConfirmNotFulFil.Visible = True
        End If

        pr_objNewPayID = IIf(strSelectedPayID = "", pr_objNewPayID, strSelectedPayID)

        If (Left(Trim(lblPaymentID.Text), 3) <> "TBS" Or Left(Trim(lblPaymentID.Text), 3) <> "CPO" Or Left(Trim(lblPaymentID.Text), 3) <> "KER" Or Left(Trim(lblPaymentID.Text), 3) <> "CKG" Or Left(Trim(lblPaymentID.Text), 3) <> "OTH") Then
            If ddlPayType.SelectedItem.Value <> 2 And Left(Trim(lblPaymentID.Text), 3) = "XXX" And strSelectedPayID <> "" Then
                If pv_intStatus = objCBTrx.EnumPaymentStatus.Verified Then
                    Dim strOpCd As String = "CB_CLSTRX_PAYMENT_UPDATE_TRXID"
                    Dim strOpCd_FindLastID As String = "ADMIN_CLSTRX_SEARCH_LASTID_CB"
                    Dim strLastIDSearch1 As String
                    Dim strLastIDSearch2 As String
                    Dim strLastIDSearch3 As String
                    Dim strNewID As String
                    Dim objLastIDDs As New Object

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
                        Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ID_GET&errmesg=" & Exp.ToString() & "&redirect=")
                    End Try

                    If objLastIDDs.Tables(0).Rows.Count > 0 Then
                        strNewID = Trim(objLastIDDs.Tables(0).Rows(0).Item("NewTrxID"))
                    Else
                        strNewID = "1"
                    End If

                    strNewIDFormat = strInitial & "/" & strCompany & "/" & strLocation & "/" & IIf(Len(Trim(strAccMonth)) = 1, "0" & strAccMonth, strAccMonth) & "/" & Trim(strAccYear) & "/" & Format(Val(strNewID), "0000")


                    strParamName = "PAYMENTTYPE|CHEQUENO|BANKCODE|SPLBANKACCNO|REMARK|CHEQUEPRINTDATE|PRINTDATE|LOCCODE|PAYMENTID|PAYMENTNEWID|UPDATEID|BANKACCNO|STAFFADVID|STAFFADVDOC|CHEQUECASH|CHEQUEHANDOVER"
                    strParamValue = Trim(ddlPayType.SelectedItem.Value) & _
                                    "|" & Trim(txtChequeNo.Text) & _
                                    "|" & Trim(strBank) & _
                                    "|" & Trim(ddlSplBankAccNo.SelectedItem.Value) & _
                                    "|" & Replace(txtRemark.Text, "'", "''") & _
                                    "|" & Now() & _
                                    "|" & strGiroDate & _
                                    "|" & Trim(strLocation) & _
                                    "|" & Trim(lblPaymentID.Text) & _
                                    "|" & Trim(strNewIDFormat) & _
                                    "|" & Trim(strUserId) & _
                                    "|" & Trim(strBankAccNo) & _
                                    "|" & Trim(strStaffAdvID) & _
                                    "|" & Trim(strStaffAdvDoc) & _
                                    "|" & IIf(chkChequeCash.Checked = True, "1", "2") & _
									"|" & ddlChequeHandOver.SelectedItem.Value



                    Try
                        intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd, _
                                                                strParamName, _
                                                                strParamValue)

                    Catch Exp As System.Exception
                        Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_CASHBANK_UPDATE_TRXID&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_CashBankDet.aspx")
                    End Try

                    pr_objNewPayID = Trim(strNewIDFormat)
                    strSelectedPayID = Trim(strNewIDFormat)
                End If
                
            End If
        End If

        If Trim(ddlCurrCode.SelectedItem.Value) <> "IDR" Then
            'If CDbl(txtExchangeRate.Text) = 1 Or CDbl(txtExchangeRate.Text) < 1 Then
            '    lblErrExchangeRate.Text = "Rate in " & Trim(ddlCurrCode.SelectedItem.Value) & " cannot equal or less than Rp. 1,00"
            '    lblErrExchangeRate.Visible = True
            '    Exit Sub
            'End If

            Dim strOpCd As String = "CB_CLSTRX_PAYMENT_UPDATE_RATE"

            strParamName = "CURRCODE|EXCHANGERATE|PAYMENTID"
            strParamValue = Trim(ddlCurrCode.SelectedItem.Value) & "|" & txtExchangeRate.Text & "|" & Trim(pr_objNewPayID)

            Try
                intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd, _
                                                        strParamName, _
                                                        strParamValue)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=UPDATE_PAYMENT&errmesg=" & Exp.ToString() & "&redirect=")
            End Try
        End If
    End Sub

    Sub AddBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim objUpdStatus As Integer
        Dim objPAYID As String
        Dim strInvoiceRcv As String = ddlInvoiceRcv.SelectedItem.Value
        Dim strDebitNote As String = ddlDebitNote.SelectedItem.Value
        Dim strCreditNote As String = ddlCreditNote.SelectedItem.Value
        'Dim strAccCode As String = ddlAccCode.SelectedItem.Value
        'isi variable strAccCode
        Dim strAccCode As String = txtAccCode.Text
        Dim strCreditorJournal As String = ddlCreditorJournal.SelectedItem.Value
        'Dim strOther As String = ddlOther.SelectedItem.Value
        'isi variable strOther
        Dim strOther As String = txtAccCodeOther.Text
        Dim strPOID As String = ddlPOID.SelectedItem.Value
        Dim intInvoiceRcvInd As Integer
        Dim intDebitNoteInd As Integer
        Dim intCreditNoteInd As Integer
        Dim intCreditorJournal As Integer
        Dim intOther
        Dim intPOID As Integer
        Dim dblAmount As Double
        Dim intSelectedDoc As Integer
        Dim intPPNRate As Integer
        Dim intPPHRate As Integer
        Dim dblPPNAmount As Double
        Dim dblPPHAmount As Double
        Dim dblNetAmount As Double
        Dim dblPPHRate As Double
        Dim strTotalAmount As String = Trim(Request.Form("txtAmount"))
        Dim strOpCode_AddLine As String = "CB_CLSTRX_PAYMENT_LINE_ADD"
        Dim strOpCode_GetPayLine As String = "CB_CLSTRX_PAYMENT_LINE_GET"
        Dim strOpCode_GetSumPayLine As String = "CB_CLSTRX_PAYMENT_SUM_LINE_GET"
        Dim strOpCode_UpdTotalAmount As String = "CB_CLSTRX_PAYMENT_TOTALAMOUNT_UPD"
        Dim strOpCd_GetIRTOTAL As String = "CB_CLSTRX_PAYMENT_INVOICERECEIVE_AMOUNT_GET"
        Dim strOpCd_GetDNTOTAL As String = "CB_CLSTRX_PAYMENT_DEBITNOTE_AMOUNT_GET"
        Dim strOpCd_GetCNTOTAL As String = "CB_CLSTRX_PAYMENT_CREDITNOTE_AMOUNT_GET"
        Dim intErrNo As Integer
        Dim strOpCd_GetCJTOTAL As String = "CB_CLSTRX_PAYMENT_CREDITORJOURNAL_AMOUNT_GET"
        Dim strGiroDate As String = Date_Validation(txtGiroDate.Text, False)
        Dim strParam As String

        Dim strOpCodes As String = strOpCode_AddLine & "|" & _
                                   strOpCode_GetPayLine & "|" & _
                                   strOpCode_GetSumPayLine & "|" & _
                                   strOpCode_UpdTotalAmount & "|" & _
                                   strOpCd_GetIRTOTAL & "|" & _
                                   strOpCd_GetDNTOTAL & "|" & _
                                   strOpCd_GetCNTOTAL & "|" & _
                                   strOpCd_GetCJTOTAL

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

        intInvoiceRcvInd = IIf(strInvoiceRcv = "", 0, 1)
        intDebitNoteInd = IIf(strDebitNote = "", 0, 1)
        intCreditNoteInd = IIf(strCreditNote = "", 0, 1)
        intCreditorJournal = IIf(strCreditorJournal = "", 0, 1)
        intOther = IIf(strOther = "", 0, 1)
        intPOID = IIf(strPOID = "", 0, 1)

        intSelectedDoc = intInvoiceRcvInd + intDebitNoteInd + intCreditNoteInd + _
                          intCreditorJournal + intOther + intPOID

        If CheckDate(txtDateCreated.Text.Trim(), indDate) = False Then
            lblDate.Visible = True
            lblFmt.Visible = True
            lblDate.Text = "<br>Date Entered should be in the format"
            Exit Sub
        End If

        If intSelectedDoc > 1 Then
            lblErrManySelectDoc.Visible = True
            Exit Sub
        ElseIf intSelectedDoc = 0 Then
            lblErrNoSelectDoc.Visible = True
            Exit Sub
        End If

        If intOther > 0 Then
            If lblTotalPaymentAmount.Text <> "" Then
                If ddlPayType.SelectedItem.Value <> "4" Then
                    If CDbl(lblTotalPaymentAmount.Text) <= 0 Then
                        lblErrOtherDoc.Visible = True
                        Exit Sub
                    End If
                End If
            Else
                lblErrOtherDoc.Visible = True
                Exit Sub
            End If
        End If


        strCurrency = ddlCurrency.SelectedItem.Value
        strExRate = txtExRate.Text
        strPOExRate = hidPOExRate.Value
        If strCurrency = "IDR" Then
            If CDbl(strExRate) > 1 Or CDbl(strExRate) < 1 Then
                lblErrExRate.Text = "Rate in IDR cannot more than Rp. 1,00"
                lblErrExRate.Visible = True
                Exit Sub
            End If
        Else
            If CDbl(strExRate) = 1 Or CDbl(strExRate) < 1 Then
                lblErrExRate.Text = "Rate in " & Trim(strCurrency) & " cannot equal or less than Rp. 1,00"
                lblErrExRate.Visible = True
                Exit Sub
            End If
        End If

        If (lblInvTypeHidden.Text = "") Or (lblInvTypeHidden.Text = "0") Then
            intPPHRate = 0
            intPPN = 0
        Else
            intPPN = IIf(cbPPN.Checked = True, objCBTrx.EnumPPN.Yes, objCBTrx.EnumPPN.No)
            If strCurrency = "IDR" Then
                intPPNRate = IIf(cbPPN.Checked = True, Session("SS_PPNRATE"), 0)
                intPPHRate = IIf(txtPPHRate.Text <> "", txtPPHRate.Text, "0")
            Else
                intPPNRate = 0
                intPPHRate = 0
                intPPN = 0
            End If
        End If

        If intSelectedDoc > 0 Then
            If lblPPHRateHidden.Text <> "" And lblPPNHidden.Text <> "" Then
                If Val(lblPPHRateHidden.Text) <> Val(intPPHRate) Then 'Or lblPPNHidden.Text <> intPPN Then
                    lblErrValidPPNHRate.Visible = True
                    Exit Sub
                End If
            End If
        End If

        If strAccCode = "" Then
            lblErrAccCode.Visible = True
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

        If lblPaymentID.Text <> "" Then
            If intLevel = 0 Then
                If lblStatus.Text = objCBTrx.mtdGetPaymentStatus(objCBTrx.EnumPaymentStatus.Verified) Then
                    'If Left(Trim(Request.Form("ddlOther")), 3) = "65." Or Left(Trim(Request.Form("ddlOther")), 5) = "71.19" Then
                    If Left(Trim(Request.Form("txtAccCodeOther")), 3) = "65." Or Left(Trim(Request.Form("txtAccCodeOther")), 5) = "71.19" Then
                    Else
                        lblErrConfirmNotFulFil.Visible = True
                        lblErrConfirmNotFulFil.Text = "You do not have permission to input this journal."
                        Exit Sub
                    End If
                ElseIf lblStatus.Text = objCBTrx.mtdGetPaymentStatus(objCBTrx.EnumPaymentStatus.Active) And UCase(Trim(strUserId)) <> UCase(Trim(hidUserID.Value)) Then
                    lblErrConfirmNotFulFil.Visible = True
                    lblErrConfirmNotFulFil.Text = "You do not have permission to process this transaction."
                    Exit Sub
                End If
            End If
        End If

        If lblPaymentID.Text <> "" And Left(lblPaymentID.Text.Trim, 3) <> "XXX" Then
            ''remark sementara
            'If txtDateCreated.Enabled = True Then
            '    If hidPrevID.Value = "" Then
            '        If hidNextID.Value <> "" Then
            '            If Day(strDate) > hidNextDate.Value Then
            '                lblDate.Visible = True
            '                lblDate.Text = "Date cannot be higher than last transaction date."
            '                Exit Sub
            '            End If
            '        End If
            '    Else
            '        If hidPrevID.Value <> "" Then
            '            If Day(strDate) < hidPrevDate.Value Then
            '                lblDate.Visible = True
            '                lblDate.Text = "Date cannot be smaller than previous transaction date."
            '                Exit Sub
            '            End If
            '            If hidNextID.Value <> "" Then
            '                If Day(strDate) > hidNextDate.Value Then
            '                    lblDate.Visible = True
            '                    lblDate.Text = "Date cannot be higher than last transaction date."
            '                    Exit Sub
            '                End If
            '            End If
            '        Else
            '            If Day(strDate) < hidPrevDate.Value Then
            '                lblDate.Visible = True
            '                lblDate.Text = "Date cannot be smaller than previous transaction date."
            '                Exit Sub
            '            End If
            '        End If
            '    End If
            'End If
        End If

        If Trim(strTotalAmount) = "" Then
            lblErrReqAmount.Visible = True
            Exit Sub
        Else
            dblAmount = objAPTrx.RoundNumber(Convert.ToDouble(strTotalAmount), CInt(Session("SS_ROUNDNO")))
            If dblAmount = 0 Then
                lblErrReqAmount.Visible = True
                Exit Sub
            Else
                If strCreditorJournal <> "" Then
                    If dblAmount > 0 And hidCreditJrnValue.Value = "-" Then
                        lblErrNegAmt.Visible = True
                        Exit Sub
                    Else
                        If dblAmount < 0 And hidCreditJrnValue.Value <> "-" Then
                            lblErrPosAmt.Visible = True
                            Exit Sub
                        End If
                    End If
                End If
            End If
        End If

        If strPOID <> "" Then
            'Advance payment can accept different currency
            'If strCurrency <> hidPOCurrency.Value Then
            '    lblErrCurrencyCode.Visible = True
            '    lblErrCurrencyCode.Text = "Advance payment currency has to be identically as PO Currency."
            '    Exit Sub
            'End If

            If hidPOCurrency.Value <> strCurrency Then
                If CDbl(strTotalAmount) * CDbl(strExRate) > CDbl(hidOutstandingAmountKonversi.Value) Then
                    lblErrExceeded.Visible = True
                    Exit Sub
                End If
            Else
                If CDbl(strTotalAmount) > CDbl(hidOutstandingAmount.Value) Then
                    lblErrExceeded.Visible = True
                    Exit Sub
                End If
            End If
        End If

        If hidPOCurrency.Value <> "" Then
            'If hidPOCurrency.Value <> strCurrency Then
            '    If hidInvAmount.Value <> 1 Then
            '        strCBExRate = dblAmount / CDbl(hidInvAmount.Value)
            '    Else
            '        strCBExRate = txtExRate.Text
            '    End If
            'Else
            '    strCBExRate = txtExRate.Text
            'End If
            strCBExRate = objAPTrx.RoundNumber(txtExRate.Text, CInt(Session("SS_ROUNDNO")))
        Else
            strCBExRate = "1"
        End If

        If intOther > 0 Then
            strCurrency = ddlCurrency.SelectedItem.Value
            strExRate = txtExRate.Text
            strPOExRate = txtExRate.Text
            strCBExRate = txtExRate.Text
        End If

        If strSelectedPayID = "" Then

            Update_Payment(objCBTrx.EnumPaymentStatus.Active, objPAYID, strDocNotFulfil)

            If strDocNotFulfil <> "" Then
                BindInvoiceRcv(ddlInvoiceRcv.SelectedItem.Value)
                BindDebitNote(ddlDebitNote.SelectedItem.Value)
                BindCreditNote(ddlCreditNote.SelectedItem.Value)
                'BindAccCode(ddlAccCode.SelectedItem.Value)
                GetCOADetail(ddlAccCode.SelectedItem.Value)
                BindCreditorJournal(ddlCreditorJournal.SelectedItem.Value)
                BindPOID(ddlPOID.SelectedItem.Value)
                onLoad_Button()
                Exit Sub
            End If

            strSelectedPayID = objPAYID
        Else
            If dgLineDet.Items.Count = 0 Then
                Update_Payment(objCBTrx.EnumPaymentStatus.Active, objPAYID, strDocNotFulfil)
            End If

            If lblCurrentPeriod.Text <> "" Then
                If Month(strDate) = Mid(lblCurrentPeriod.Text, 5) And Year(strDate) = Mid(lblCurrentPeriod.Text, 1, 4) Then
                Else
                    lblDate.Visible = True
                    lblDate.Text = "Invalid transaction date."
                    Exit Sub
                End If
            End If

            If ddlPayType.SelectedItem.Value = "0" Or ddlPayType.SelectedItem.Value = "3" Then
                If ddlBank.SelectedItem.Value = "" Then
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
                    If Month(strGiroDate) = Mid(lblCurrentPeriod.Text, 5) And Year(strGiroDate) = Mid(lblCurrentPeriod.Text, 1, 4) Then
                    Else
                        lblDateGiro.Visible = True
                        lblDateGiro.Text = "Invalid transaction date."
                        Exit Sub
                    End If
                    If Year(strGiroDate) & Format(Month(strGiroDate), "00") & Format(Day(strGiroDate), "00") < Year(strDate) & Format(Month(strDate), "00") & Format(Day(strDate), "00") Then
                        lblDateGiro.Visible = True
                        lblDateGiro.Text = "Invalid transaction date."
                        Exit Sub
                    End If
                End If
            End If
        End If

        If (lblInvTypeHidden.Text <> "") Then
            If lblInvTypeHidden.Text = objCBTrx.EnumInvoiceType.SupplierPO Then
                intNetAmount = objAPTrx.RoundNumber((strTotalAmount * 100) / (intPPNRate + 100), CInt(Session("SS_ROUNDNO")))
                If cbPPN.Checked = True Then
                    intPPNAmount = objAPTrx.RoundNumber((intNetAmount * intPPNRate) / 100, 0)
                Else
                    intPPNAmount = 0
                End If
                intPPHAmount = 0
            Else
                If lblInvTypeHidden.Text = objCBTrx.EnumInvoiceType.ContractorWorkOrder Or lblInvTypeHidden.Text = objCBTrx.EnumInvoiceType.Others Then
                    If txtPPHRate.Text <> "" Then
                        intNetAmount = objAPTrx.RoundNumber((strTotalAmount * 100) / (intPPNRate + 100 - txtPPHRate.Text.Trim), CInt(Session("SS_ROUNDNO")))
                        intPPHAmount = objAPTrx.RoundNumber(((intNetAmount * txtPPHRate.Text.Trim) / 100), 0)
                    Else
                        intNetAmount = objAPTrx.RoundNumber((strTotalAmount * 100) / (intPPNRate + 100), 0)
                        intPPNAmount = 0
                    End If
                    If cbPPN.Checked = True Then
                        intPPNAmount = IIf(strCurrency = "IDR", objAPTrx.RoundNumber(((intNetAmount * intPPNRate) / 100), 0), 0)
                    Else
                        intPPNAmount = 0
                    End If
                Else
                    intNetAmount = objAPTrx.RoundNumber(strTotalAmount, CInt(Session("SS_ROUNDNO")))
                    intPPNAmount = 0
                    intPPHAmount = 0
                End If
            End If
        Else
            intNetAmount = objAPTrx.RoundNumber(strTotalAmount, CInt(Session("SS_ROUNDNO")))
            intPPNAmount = 0
            intPPHAmount = 0
        End If
        dblPPNAmount = Convert.ToDouble(intPPNAmount)
        dblPPHAmount = Convert.ToDouble(intPPHAmount)
        dblNetAmount = Convert.ToDouble(intNetAmount)
        dblPPHRate = Convert.ToDouble(intPPHRate)

        If lblTxLnID.Text = "" Then
            strParam = objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CBPaymentLn) & "|" & _
                     strSelectedPayID & "|" & _
                     strInvoiceRcv & "|" & _
                     strDebitNote & "|" & _
                     strCreditNote & "|" & _
                     strCreditorJournal & "|" & _
                     strAccCode & "|" & _
                     dblAmount & "|" & _
                     dblPPHRate & "|" & _
                     intPPN & "|" & _
                     dblPPNAmount & "|" & _
                     dblPPHAmount & "|" & _
                     dblNetAmount & "|" & _
                     IIf(lblInvTypeHidden.Text <> "", lblInvTypeHidden.Text, "0") & "|" & _
                     strCurrency & "|" & _
                     strExRate & "|" & _
                     strPOExRate & "|" & _
                     strCBExRate & "|" & _
                     strOther & "|" & _
                     strPOID & "|" & _
                     Replace(Trim(txtAddNote.Value), "'", "''") & "|" & _
                     Trim(TaxLnID) & "|" & _
                     TaxRate & "|" & _
                     DPPAmount & "|" & _
                     Trim(ddlGiroNo.SelectedItem.Value)

            Try
                intErrNo = objCBTrx.mtdUpdPaymentLine(strOpCodes, strCompany, strLocation, strUserId, _
                                                      strAccMonth, strAccYear, strParam, objUpdStatus)

            Catch Exp As Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_ADD_LINE&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_paylist.aspx")
            End Try

        Else
            Dim strOpCodes_DelLine As String
            Dim strOpCode_DelLine As String = "CB_CLSTRX_PAYMENT_LINE_DEL"
            strOpCode_GetPayLine = "CB_CLSTRX_PAYMENT_LINE_GET"
            strOpCode_UpdTotalAmount = "CB_CLSTRX_PAYMENT_TOTALAMOUNT_UPD"
            strOpCodes_DelLine = strOpCode_DelLine & "|" & strOpCode_GetPayLine & "|" & strOpCode_UpdTotalAmount

            Try
                strParam = lblTxLnID.Text & "|" & strSelectedPayID
                intErrNo = objCBTrx.mtdDelPaymentLine(strOpCodes_DelLine, _
                                                      strCompany, _
                                                      strLocation, _
                                                      strUserId, _
                                                      strAccMonth, _
                                                      strAccYear, _
                                                      strParam)
            Catch Exp As Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_DEL_LINE&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_paylist.aspx")
            End Try

            strParam = objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CBPaymentLn) & "|" & _
                     strSelectedPayID & "|" & _
                     strInvoiceRcv & "|" & _
                     strDebitNote & "|" & _
                     strCreditNote & "|" & _
                     strCreditorJournal & "|" & _
                     strAccCode & "|" & _
                     dblAmount & "|" & _
                     dblPPHRate & "|" & _
                     intPPN & "|" & _
                     dblPPNAmount & "|" & _
                     dblPPHAmount & "|" & _
                     dblNetAmount & "|" & _
                     IIf(lblInvTypeHidden.Text <> "", lblInvTypeHidden.Text, "0") & "|" & _
                     strCurrency & "|" & _
                     strExRate & "|" & _
                     strPOExRate & "|" & _
                     strCBExRate & "|" & _
                     strOther & "|" & _
                     strPOID & "|" & _
                     Replace(Trim(txtAddNote.Value), "'", "''") & "|" & _
                     Trim(TaxLnID) & "|" & _
                     TaxRate & "|" & _
                     DPPAmount & "|" & _
                     Trim(ddlGiroNo.SelectedItem.Value)

            Try
                intErrNo = objCBTrx.mtdUpdPaymentLine(strOpCodes, strCompany, strLocation, strUserId, _
                                                      strAccMonth, strAccYear, strParam, objUpdStatus)

            Catch Exp As Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_ADD_LINE&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_paylist.aspx")
            End Try

            '    strOpCodes = "CB_CLSTRX_PAYMENT_LINE_UPD"
            '    strParamName = "PAYMENTTYPE|BANKCODE|CHEQUENO|REMARK|STATUS|CREATEDATE|UPDATEDATE|UPDATEID|BANKACCNO|PAYMENTID"
            '    strParamValue = ddlPayType.SelectedItem.Value & "|" & _
            '                    ddlBank.SelectedItem.Value & "|" & _
            '                    txtChequeNo.Text & "|" & _
            '                    txtRemark.Text & "|" & _
            '                    objCBTrx.EnumPaymentStatus.Confirmed & "|" & _
            '                    strDate & "|" & _
            '                    Now() & "|" & _
            '                    strUserId & "|" & _
            '                    txtSplBankAccNo.Text & "|" & _
            '                    strSelectedPayID

            '    Try
            '        intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd, _
            '                                                strParamName, _
            '                                                strParamValue)

            '    Catch Exp As System.Exception
            '        Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PAYMENTLN_UPD&errmesg=" & Exp.ToString() & "&redirect=pu/trx/AP_trx_InvoiceRcvList")
            '    End Try
            '    lblTxLnID.Text = ""
            '    dgLineDet.EditItemIndex = -1

        End If

        If objUpdStatus = 2 Then
            lblErrConfirmNotFulFil.Text = lblErrConfirmNotFulFilText.Text & strInvoiceRcv & strDebitNote & strCreditNote & ".xxx"
            lblErrConfirmNotFulFil.Visible = True
            BindInvoiceRcv(ddlInvoiceRcv.SelectedItem.Value)
            BindDebitNote(ddlDebitNote.SelectedItem.Value)
            BindCreditNote(ddlCreditNote.SelectedItem.Value)
            'BindAccCode(ddlAccCode.SelectedItem.Value)
            GetCOADetail(txtAccCode.Text)
            BindCreditorJournal(ddlCreditorJournal.SelectedItem.Value)
            BindPOID(ddlPOID.SelectedItem.Value)
        Else
            'ddlOther.SelectedIndex = 0
            GetCOAOtherDetail("")
            onLoad_Display(strSelectedPayID)
            onLoad_DisplayLine(strSelectedPayID)
        End If

        onLoad_Button()
    End Sub

    Sub SaveBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim objPAYID As String
        Dim intErrNo As Integer
        Dim strOpCd As String = "CB_CLSTRX_PAYMENT_UPD"
        Dim strGiroDate As String = Date_Validation(txtGiroDate.Text, False)
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

        If strSelectedPayID = "" Then
            Exit Sub
        End If

        strCurrency = ddlCurrency.SelectedItem.Value
        strExRate = txtExRate.Text
        strPOExRate = hidPOExRate.Value
        If strCurrency = "IDR" Then
            If CDbl(strExRate) > 1 Or CDbl(strExRate) < 1 Then
                lblErrExRate.Text = "Rate in IDR cannot more than Rp. 1,00"
                lblErrExRate.Visible = True
                Exit Sub
            End If
        Else
            If CDbl(strExRate) = 1 Or CDbl(strExRate) < 1 Then
                lblErrExRate.Text = "Rate in " & Trim(strCurrency) & " cannot equal or less than Rp. 1,00"
                lblErrExRate.Visible = True
                Exit Sub
            End If
        End If

        intStatus = Convert.ToInt16(lblStatusHidden.Text)
        Select Case intStatus
            Case objCBTrx.EnumPaymentStatus.Active
                If intLevel = 0 Then
                    If ddlPayType.SelectedItem.Value <> "2" Then 'if other than Need Verification
                        If hidHadCOATax.Value > 0 Then
                            If CInt(hidTaxStatus.Value) = objCBTrx.EnumTaxStatus.Unverified Then
                                lblErrConfirmNotFulFil.Visible = True
                                lblErrConfirmNotFulFil.Text = "This transaction contain tax account, please verify first."
                                Exit Sub
                            End If
                        Else
                            If UCase(Trim(strUserId)) <> UCase(Trim(hidUserID.Value)) Then
                                lblErrConfirmNotFulFil.Visible = True
                                lblErrConfirmNotFulFil.Text = "You do not have permission to process this transaction."
                                Exit Sub
                            End If
                        End If
                    Else
                        If UCase(Trim(strUserId)) <> UCase(Trim(hidUserID.Value)) Then
                            lblErrConfirmNotFulFil.Visible = True
                            lblErrConfirmNotFulFil.Text = "You do not have permission to process this transaction."
                            Exit Sub
                        End If
                    End If
                End If

                Update_Payment(objCBTrx.EnumPaymentStatus.Active, objPAYID, strDocNotFulfil)
                strSelectedPayID = objPAYID
                'strSelectedPayID = lblPaymentID.Text
            Case objCBTrx.EnumPaymentStatus.Verified
                If ddlPayType.SelectedItem.Value <> "2" Then 'if other than Need Verification
                    If hidHadCOATax.Value > 0 Then
                        If CInt(hidTaxStatus.Value) = objCBTrx.EnumTaxStatus.Unverified Then
                            lblErrConfirmNotFulFil.Visible = True
                            lblErrConfirmNotFulFil.Text = "This transaction contain tax account, please verify first."
                            Exit Sub
                        End If
                    End If
                End If
                Update_Payment(objCBTrx.EnumPaymentStatus.Verified, objPAYID, strDocNotFulfil)
                strSelectedPayID = objPAYID
                'strSelectedPayID = lblPaymentID.Text
            Case objCBTrx.EnumPaymentStatus.Confirmed
                If lblCurrentPeriod.Text <> "" Then
                    If Month(strDate) = Mid(lblCurrentPeriod.Text, 5) And Year(strDate) = Mid(lblCurrentPeriod.Text, 1, 4) Then
                    Else
                        lblDate.Visible = True
                        lblDate.Text = "Invalid transaction date."
                        Exit Sub
                    End If
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
                        If Month(strGiroDate) = Mid(lblCurrentPeriod.Text, 5) And Year(strGiroDate) = Mid(lblCurrentPeriod.Text, 1, 4) Then
                        Else
                            lblDateGiro.Visible = True
                            lblDateGiro.Text = "Invalid transaction date."
                            Exit Sub
                        End If
                        If Year(strGiroDate) & Format(Month(strGiroDate), "00") & Format(Day(strGiroDate), "00") < Year(strDate) & Format(Month(strDate), "00") & Format(Day(strDate), "00") Then
                            lblDateGiro.Visible = True
                            lblDateGiro.Text = "Invalid transaction date."
                            Exit Sub
                        End If
                    End If
                End If

                Dim strStaffAdvID As String
                Dim strStaffAdvDoc As String

                If ddlRefNo.SelectedItem.Value = "" Then
                    strStaffAdvID = ""
                    strBankAccCode = ""
                Else
                    arrParam = Split(Trim(ddlRefNo.SelectedItem.Value), "|")
                    strStaffAdvID = Trim(arrParam(0))
                    strStaffAdvDoc = Trim(arrParam(1))
                End If

                strParamName = "SUPPLIERCODE|PAYMENTTYPE|BANKCODE|CHEQUENO|REMARK|STATUS|CREATEDATE|PRINTDATE|UPDATEDATE|UPDATEID|SPLBANKACCNO|TAXSTATUS|PAYMENTID|BANKACCNO|STAFFADVID|STAFFADVDOC|CHEQUECASH|CHEQUEHANDOVER"
                strParamValue = txtSupCode.Text & "|" & _
                                ddlPayType.SelectedItem.Value & "|" & _
                                strBank & "|" & _
                                txtChequeNo.Text & "|" & _
                                Replace(txtRemark.Text, "'", "''") & "|" & _
                                objCBTrx.EnumPaymentStatus.Confirmed & "|" & _
                                strDate & "|" & _
                                strGiroDate & "|" & _
                                Now() & "|" & _
                                strUserId & "|" & _
                                ddlSplBankAccNo.SelectedItem.Value & "|" & _
                                CInt(hidTaxStatus.Value) & "|" & _
                                strSelectedPayID & "|" & _
                                strBankAccNo & "|" & _
                                Trim(strStaffAdvID) & "|" & _
                                Trim(strStaffAdvDoc) & "|" & _
                                IIf(chkChequeCash.Checked = True, "1", "2") & "|" & _
                                ddlChequeHandOver.SelectedItem.Value

                Try
                    intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd, _
                                                            strParamName, _
                                                            strParamValue)

                Catch Exp As System.Exception
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PAYMENT_UPD&errmesg=" & Exp.ToString() & "&redirect=pu/trx/AP_trx_InvoiceRcvList")
                End Try

                strSelectedPayID = strSelectedPayID
        End Select

        If strSelectedPayID <> "" Then
            RefreshBtn_Click(Sender, E)
        End If
    End Sub

    Sub RefreshBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        onLoad_Display(strSelectedPayID)
        onLoad_DisplayLine(strSelectedPayID)
        onLoad_Button()
    End Sub

    Sub ConfirmBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim objPAYID As String

        If Left(Trim(lblPaymentID.Text), 3) = "XXX" Then
            lblErrPayType.Visible = True
            Exit Sub
        Else
            strSelectedPayID = Trim(lblPaymentID.Text)
            Update_Payment(objCBTrx.EnumPaymentStatus.Confirmed, objPAYID, strDocNotFulfil)

            strSelectedPayID = objPAYID
            If strSelectedPayID <> "" Then
                RefreshBtn_Click(Sender, E)
            End If
        End If
    End Sub

    Sub DeleteBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim objPAYID As New Object()

        strSelectedPayID = Trim(lblPaymentID.Text)
        Update_Payment(objCBTrx.EnumPaymentStatus.Deleted, objPAYID, strDocNotFulfil)

        strSelectedPayID = objPAYID
        If strSelectedPayID <> "" Then
            RefreshBtn_Click(Sender, E)
        End If
    End Sub

    Sub UnDeleteBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim objPAYID As New Object()

        Update_Payment(objCBTrx.EnumPaymentStatus.Active, objPAYID, strDocNotFulfil)

        strSelectedPayID = objPAYID
        If strSelectedPayID <> "" Then
            RefreshBtn_Click(Sender, E)
        End If
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strOpCode_DelLine As String = "CB_CLSTRX_PAYMENT_LINE_DEL"
        Dim strOpCode_GetPayLine As String = "CB_CLSTRX_PAYMENT_LINE_GET"
        Dim strOpCode_UpdTotalAmount As String = "CB_CLSTRX_PAYMENT_TOTALAMOUNT_UPD"
        Dim strOpCodes = strOpCode_DelLine & "|" & strOpCode_GetPayLine & "|" & strOpCode_UpdTotalAmount
        Dim strParam As String
        Dim lblDelText As Label
        Dim strPayLNId As String
        Dim intErrNo As Integer
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

        strSelectedPayID = lblPaymentID.Text.Trim

        dgLineDet.EditItemIndex = Convert.ToInt16(E.Item.ItemIndex)
        lblDelText = dgLineDet.Items.Item(Convert.ToInt16(E.Item.ItemIndex)).FindControl("cnlnid")
        strPayLNId = lblDelText.Text


        Try
            strParam = strPayLNId & "|" & strSelectedPayID
            intErrNo = objCBTrx.mtdDelPaymentLine(strOpCodes, _
                                                  strCompany, _
                                                  strLocation, _
                                                  strUserId, _
                                                  strAccMonth, _
                                                  strAccYear, _
                                                  strParam)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_DEL_LINE&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_paylist.aspx")
        End Try

        onLoad_Display(strSelectedPayID)
        onLoad_DisplayLine(strSelectedPayID)
        onLoad_Button()
    End Sub

    Sub btnPreviewCheque_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim dsForBankCodeDropDown As New DataSet()
        Dim strChequeNo As String = txtChequeNo.Text.Trim
        Dim strOpCd_UpdPay As String = "CB_CLSTRX_PAYMENT_PRINTCHEQUE_UPD"
        Dim strSortLine = "CB_PAYMENTLN.PaymentLnID"
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

        strSelectedPayID = lblPaymentID.Text.Trim

        If strBank = "" Then
            lblErrBank.Visible = True
            Exit Sub
        End If

        '-- Bank can be edited
        'If Left(Trim(strSelectedPayID), 3) <> Trim(strBank) Then
        '    lblErrPrintCheque.Text = "Invalid Bank Details! Please check your Bank Details again."
        '    lblErrPrintCheque.Visible = True
        '    onLoad_Button()
        '    Exit Sub
        'End If

        Dim strOpCd As String = "CB_CLSTRX_PAYMENT_UPDATE"
        Dim intErrNo As Integer

        strParamName = "CHEQUENO|BANKCODE|REMARK|CHEQUEPRINTDATE|PRINTDATE|LOCCODE|PAYMENTID|CHEQUECASH|CHEQUEHANDOVER"

        strParamValue = Trim(txtChequeNo.Text) & _
                        "|" & Trim(strBank) & _
                        "|" & Replace(txtRemark.Text, "'", "''") & _
                        "|" & Now() & _
                        "|" & strGiroDate & _
                        "|" & Trim(strLocation) & _
                        "|" & Trim(strSelectedPayID) & _
                        "|" & IIf(chkChequeCash.Checked = True, "1", "2") & _
                        "|" & ddlChequeHandOver.SelectedItem.Value

        Try
            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_PREVIEWCHEQUE_UPD&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_paydet.aspx")
        End Try

        If ddlPayType.SelectedItem.Value = "0" Or ddlPayType.SelectedItem.Value = "1" Then
            strOpCd = "PR_STDRPT_BANK_GET_CHEQUEFMT"
            strParam = Convert.ToString(objHRSetup.EnumBankStatus.Active) & "|" & _
                       Convert.ToString(objHRSetup.EnumBankFormatStatus.Active) & "|" & _
                       Convert.ToString(objHRSetup.EnumBankFormatType.Cheque) & "|" & _
                       strBank.Trim
        ElseIf ddlPayType.SelectedItem.Value = "3" Then
            strOpCd = "PR_STDRPT_BANK_GET_BILYETGIROFMT"
            strParam = Convert.ToString(objHRSetup.EnumBankStatus.Active) & "|" & _
                       Convert.ToString(objHRSetup.EnumBankFormatStatus.Active) & "|" & _
                       Convert.ToString(objHRSetup.EnumBankFormatType.BilyetGiro) & "|" & _
                       strBank.Trim
        End If

        Try
            intErrNo = objPRRpt.mtdGetBank(strOpCd, strParam, dsForBankCodeDropDown)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_PROGPATH_PREVIEWCHEQUE_GET&errmesg=" & Exp.ToString() & "&redirect=../../en/CB/trx/CB_trx_paydet.aspx")
        End Try

        If dsForBankCodeDropDown.Tables(0).Rows.Count > 0 Then
            lblProgramPath.Text = dsForBankCodeDropDown.Tables(0).Rows(0).Item("ProgramPath").Trim()

            'onLoad_Display(strSelectedPayID)
            'Response.Write("<Script Language=""JavaScript"">window.open(""../../reports/" & lblProgramPath.Text & "?Type=Print&CompName=" & strCompany & _
            '                "&trx=CB_trx_payment&TotalAmount=" & lblTotalPaymentAmount.Text & _
            '                "&SupplierCode=" & ddlSupplier.SelectedItem.Value.Trim & "&SupplierName=" & strSuppName & _
            '                "&objMCBPath=" & Server.UrlEncode(objMCBPath) & _
            '                """,null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")

            Response.Write("<Script Language=""JavaScript"">window.open(""../reports/CB_Rpt_ChequeGiroPrint.aspx?strId=" & strSelectedPayID & _
                  "&strProgramPath=" & lblProgramPath.Text & _
                  "&TRXType=" & "PAYMENT" & _
                  "&CBType=" & "1" & _
                  "&strSortLine=" & strSortLine & _
                  "&strCurrencyCode=" & lblCurrency.Text & _
                  "&strExchangeRate=" & "1" & _
                  "&strBiaya=" & "0" & _
                  """, null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");window.close();</Script>")
        Else
            lblErrPrintCheque.Visible = True
            onLoad_Display(strSelectedPayID)
            Exit Sub
        End If

    End Sub

    Sub btnPreviewSlip_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim dsForBankCodeDropDown As New DataSet()
        Dim strChequeNo As String = txtChequeNo.Text.Trim
        Dim strOpCd_UpdPay As String = "CB_CLSTRX_PAYMENT_PRINTCHEQUE_UPD"
        Dim strSortLine = "CB_PAYMENTLN.PaymentLnID"
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

        strSelectedPayID = lblPaymentID.Text.Trim

        If strBank = "" Then
            lblErrBank.Visible = True
            Exit Sub
        End If

        '-- Bank can be edited
        'If Left(Trim(strSelectedPayID), 3) <> Trim(strBank) Then
        '    lblErrPrintCheque.Text = "Invalid Bank Details! Please check your Bank Details again."
        '    lblErrPrintCheque.Visible = True
        '    onLoad_Button()
        '    Exit Sub
        'End If

        Dim strOpCd As String = "CB_CLSTRX_PAYMENT_UPDATE"
        Dim intErrNo As Integer

        strParamName = "CHEQUENO|BANKCODE|REMARK|CHEQUEPRINTDATE|PRINTDATE|LOCCODE|PAYMENTID|CHEQUECASH"

        strParamValue = Trim(txtChequeNo.Text) & _
                        "|" & Trim(strBank) & _
                        "|" & Trim(txtRemark.Text) & _
                        "|" & Now() & _
                        "|" & strGiroDate & _
                        "|" & Trim(strLocation) & _
                        "|" & Trim(strSelectedPayID) & _
                        "|" & IIf(chkChequeCash.Checked = True, "1", "2")

        Try
            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_PREVIEWCHEQUE_UPD&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_paydet.aspx")
        End Try

        strOpCd = "PR_STDRPT_BANK_GET_SLIPSETORANFMT"
        strParam = Convert.ToString(objHRSetup.EnumBankStatus.Active) & "|" & _
                   Convert.ToString(objHRSetup.EnumBankFormatStatus.Active) & "|" & _
                   Convert.ToString(objHRSetup.EnumBankFormatType.SlipSetoran) & "|" & _
                   strBank.Trim


        Try
            intErrNo = objPRRpt.mtdGetBank(strOpCd, strParam, dsForBankCodeDropDown)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_PROGPATH_PREVIEWCHEQUE_GET&errmesg=" & Exp.ToString() & "&redirect=../../en/CB/trx/CB_trx_paydet.aspx")
        End Try

        If dsForBankCodeDropDown.Tables(0).Rows.Count > 0 Then
            lblProgramPath.Text = dsForBankCodeDropDown.Tables(0).Rows(0).Item("ProgramPath").Trim()

            'onLoad_Display(strSelectedPayID)
            'Response.Write("<Script Language=""JavaScript"">window.open(""../../reports/" & lblProgramPath.Text & "?Type=Print&CompName=" & strCompany & _
            '                "&trx=CB_trx_payment&TotalAmount=" & lblTotalPaymentAmount.Text & _
            '                "&SupplierCode=" & ddlSupplier.SelectedItem.Value.Trim & "&SupplierName=" & strSuppName & _
            '                "&objMCBPath=" & Server.UrlEncode(objMCBPath) & _
            '                """,null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")

            'If UCase(Trim(lblCurrency.Text)) = "IDR" Then
            '    Response.Write("<Script Language=""JavaScript"">window.open(""../reports/CB_Rpt_ChequeGiroPrint.aspx?strId=" & strSelectedPayID & _
            '               "&strProgramPath=" & lblProgramPath.Text & _
            '               "&TRXType=" & "PAYMENT" & _
            '               "&CBType=" & "2" & _
            '               "&strSortLine=" & strSortLine & _
            '               """, null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")
            'Else
            Response.Write("<Script Language=""JavaScript"">window.open(""../reports/CB_Rpt_ChequeGiroPrint.aspx?strId=" & strSelectedPayID & _
                  "&strProgramPath=" & lblProgramPath.Text & _
                  "&TRXType=" & "PAYMENT" & _
                  "&CBType=" & "2" & _
                  "&strSortLine=" & strSortLine & _
                  "&strCurrencyCode=" & lblCurrency.Text & _
                  "&strExchangeRate=" & "1" & _
                  "&strBiaya=" & "0" & _
                  """, null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");window.close();</Script>")
            'End If
        Else
            lblErrPrintCheque.Visible = True
            onLoad_Display(strSelectedPayID)
            Exit Sub
        End If
    End Sub

    Sub btnPreviewTransfer_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim dsForBankCodeDropDown As New DataSet()
        Dim strChequeNo As String = txtChequeNo.Text.Trim
        Dim strOpCd_UpdPay As String = "CB_CLSTRX_PAYMENT_PRINTCHEQUE_UPD"
        Dim strSortLine = "CB_PAYMENTLN.PaymentLnID"
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

        strSelectedPayID = lblPaymentID.Text.Trim

        If strBank = "" Then
            lblErrBank.Visible = True
            Exit Sub
        End If

        '-- Bank can be edited
        'If Left(Trim(strSelectedPayID), 3) <> Trim(strBank) Then
        '    lblErrPrintCheque.Text = "Invalid Bank Details! Please check your Bank Details again."
        '    lblErrPrintCheque.Visible = True
        '    onLoad_Button()
        '    Exit Sub
        'End If

        Dim strOpCd As String = "CB_CLSTRX_PAYMENT_UPDATE"
        Dim intErrNo As Integer

        strParamName = "CHEQUENO|BANKCODE|REMARK|CHEQUEPRINTDATE|PRINTDATE|LOCCODE|PAYMENTID|CHEQUECASH"

        strParamValue = Trim(txtChequeNo.Text) & _
                        "|" & Trim(strBank) & _
                        "|" & Replace(txtRemark.Text, "'", "''") & _
                        "|" & Now() & _
                        "|" & strGiroDate & _
                        "|" & Trim(strLocation) & _
                        "|" & Trim(strSelectedPayID) & _
                        "|" & IIf(chkChequeCash.Checked = True, "1", "2")

        Try
            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_PREVIEWCHEQUE_UPD&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_paydet.aspx")
        End Try

        strOpCd = "PR_STDRPT_BANK_GET_SLIPTRANSFERFMT"
        strParam = Convert.ToString(objHRSetup.EnumBankStatus.Active) & "|" & _
                   Convert.ToString(objHRSetup.EnumBankFormatStatus.Active) & "|" & _
                   Convert.ToString(objHRSetup.EnumBankFormatType.SlipTransfer) & "|" & _
                   strBank.Trim


        Try
            intErrNo = objPRRpt.mtdGetBank(strOpCd, strParam, dsForBankCodeDropDown)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_PROGPATH_PREVIEWCHEQUE_GET&errmesg=" & Exp.ToString() & "&redirect=../../en/CB/trx/CB_trx_paydet.aspx")
        End Try

        If dsForBankCodeDropDown.Tables(0).Rows.Count > 0 Then
            lblProgramPath.Text = dsForBankCodeDropDown.Tables(0).Rows(0).Item("ProgramPath").Trim()

            'onLoad_Display(strSelectedPayID)
            'Response.Write("<Script Language=""JavaScript"">window.open(""../../reports/" & lblProgramPath.Text & "?Type=Print&CompName=" & strCompany & _
            '                "&trx=CB_trx_payment&TotalAmount=" & lblTotalPaymentAmount.Text & _
            '                "&SupplierCode=" & ddlSupplier.SelectedItem.Value.Trim & "&SupplierName=" & strSuppName & _
            '                "&objMCBPath=" & Server.UrlEncode(objMCBPath) & _
            '                """,null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")

            'If UCase(Trim(lblCurrency.Text)) = "IDR" Then
            '    Response.Write("<Script Language=""JavaScript"">window.open(""../reports/CB_Rpt_ChequeGiroPrint.aspx?strId=" & strSelectedPayID & _
            '               "&strProgramPath=" & lblProgramPath.Text & _
            '               "&TRXType=" & "PAYMENT" & _
            '               "&CBType=" & "3" & _
            '               "&strSortLine=" & strSortLine & _
            '               """, null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")
            'Else
            Response.Write("<Script Language=""JavaScript"">window.open(""../reports/CB_Rpt_ChequeGiroPrint.aspx?strId=" & strSelectedPayID & _
                  "&strProgramPath=" & lblProgramPath.Text & _
                  "&TRXType=" & "PAYMENT" & _
                  "&CBType=" & "3" & _
                  "&strSortLine=" & strSortLine & _
                  "&strCurrencyCode=" & lblCurrency.Text & _
                  "&strExchangeRate=" & "1" & _
                  "&strBiaya=" & "0" & _
                  """, null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");window.close();</Script>")
            'End If
        Else
            lblErrPrintCheque.Visible = True
            onLoad_Display(strSelectedPayID)
            Exit Sub
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
        Dim strID As String = strSelectedPayID
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

        strSelectedPayID = lblPaymentID.Text.Trim

        strAccountTag = lblAccount.Text
        strUpdString = "where PaymentID = '" & strSelectedPayID & "'"
        strStatus = Trim(lblStatus.Text)
        intStatus = Convert.ToInt16(lblStatusHidden.Text)
        strPrintDate = Trim(lblPrintDate.Text)
        strSortLine = "CB_PAYMENTLN.PaymentLnID"
        strTable = "CB_PAYMENT"

        'If intStatus = objCBTrx.EnumPaymentStatus.Confirmed Then
        '    If strPrintDate = "" Then
        '        Try
        '            intErrNo = objAdmShare.mtdUpdPrintDate(strOpCodePrint, _
        '                                                   strUpdString, _
        '                                                   strTable, _
        '                                                   strCompany, _
        '                                                   strLocation, _
        '                                                   strUserId)
        '        Catch Exp As Exception
        '            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYDET_UPD_PRINTDATE&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_paylist.aspx")
        '        End Try
        '    Else
        '        strStatus = strStatus & " (re-printed)"
        '    End If
        'End If

        Dim strOpCd As String = "CB_CLSTRX_PAYMENT_UPDATE"
        Dim intErrNo As Integer

        strParamName = "CHEQUENO|BANKCODE|REMARK|CHEQUEPRINTDATE|PRINTDATE|LOCCODE|PAYMENTID|CHEQUECASH"

        strParamValue = Trim(txtChequeNo.Text) & _
                        "|" & Trim(strBank) & _
                        "|" & Replace(txtRemark.Text, "'", "''") & _
                        "|" & Now() & _
                        "|" & strGiroDate & _
                        "|" & Trim(strLocation) & _
                        "|" & Trim(strSelectedPayID) & _
                        "|" & IIf(chkChequeCash.Checked = True, "1", "2")

        Try
            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_PREVIEWCHEQUE_UPD&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_paydet.aspx")
        End Try

        onLoad_Display(strSelectedPayID)
        onLoad_DisplayLine(strSelectedPayID)
        onLoad_Button()

        Response.Write("<Script Language=""JavaScript"">window.open(""../reports/CB_Rpt_PaymentDet.aspx?strPayId=" & strSelectedPayID & _
                       "&strPrintDate=" & strPrintDate & _
                       "&strStatus=" & strStatus & _
                       "&strSortLine=" & strSortLine & _
                       "&strAccountTag=" & strAccountTag & _
                       """, null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")
    End Sub

    Sub BackBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CB_trx_paylist.aspx")
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
            'strAmount2 = Trim(Right(strAmount,Len(strAmount)- 2))
            strAmount2 = Trim(Right(strAmount, Len(strAmount) - 3))
            hidInvAmount.Value = strAmount2

            'txtAmount.Text = Trim(Left(strAmount2,Len(strAmount2)- 3))
            txtAmount.Text = strAmount2
            If UBound(arrAmount) > 1 Then
                strKurs = Trim(arrAmount(2))
                txtExRate.Text = Trim(Right(strKurs, Len(strKurs) - 7))
                txtExRate.Text = Replace(txtExRate.Text, ")", "")
                pv_strCurrencyCode = Mid(strKurs, 2, 3)
            ElseIf UBound(arrAmount) > 2 Then
                hidFindPOPPH23.Value = arrAmount(3)
                If CDbl(hidFindPOPPH23.Value) = 0 Then
                    lblFindPOPPH23.Visible = False
                Else
                    lblFindPOPPH23.Visible = True
                End If
                hidFindPOPPH21.Value = arrAmount(4)
                If CDbl(hidFindPOPPH21.Value) = 0 Then
                    lblFindPOPPH21.Visible = False
                Else
                    lblFindPOPPH21.Visible = True
                End If
            Else
                pv_strCurrencyCode = "IDR"
                txtExRate.Text = "1"
            End If

            hidDocID.Value = Trim(arrAmount(0))
            hidPOCurrency.Value = pv_strCurrencyCode
            hidPOExRate.Value = txtExRate.Text
            hidOutstandingAmount.Value = txtAmount.Text

            If hidPOCurrency.Value <> "IDR" Then
                hidOutstandingAmountKonversi.Value = hidOutstandingAmount.Value * hidPOExRate.Value
            Else
                hidOutstandingAmountKonversi.Value = hidOutstandingAmount.Value
            End If
        Else
            pv_strCurrencyCode = "IDR"
            txtAmount.Text = "0"
            txtExRate.Text = "1"
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
            ddlCurrency.SelectedValue = Trim(pv_strCurrencyCode)
            ddlCurrCode.SelectedValue = Trim(pv_strCurrencyCode)
        End If
        'If objCurrencyDs.Tables(0).Rows.Count > 0 Then
        '    For intCnt = 0 To objCurrencyDs.Tables(0).Rows.Count - 1
        '        objCurrencyDs.Tables(0).Rows(intCnt).Item("CurrencyCode") = Trim(objCurrencyDs.Tables(0).Rows(intCnt).Item("CurrencyCode"))
        '        objCurrencyDs.Tables(0).Rows(intCnt).Item("Description") = objCurrencyDs.Tables(0).Rows(intCnt).Item("CurrencyCode")
        '        If Trim(objCurrencyDs.Tables(0).Rows(intCnt).Item("CurrencyCode")) = Trim(pv_strCurrencyCode) Then
        '            intSelectedIndex = intCnt
        '        End If
        '    Next
        'End If

        ddlCurrency.DataSource = objCurrencyDs.Tables(0)
        ddlCurrency.DataValueField = "CurrencyCode"
        ddlCurrency.DataTextField = "CodeDescr"
        ddlCurrency.DataBind()
        'ddlCurrency.SelectedIndex = intSelectedIndex

        ddlCurrCode.DataSource = objCurrencyDs.Tables(0)
        ddlCurrCode.DataValueField = "CurrencyCode"
        ddlCurrCode.DataTextField = "CodeDescr"
        ddlCurrCode.DataBind()
        'ddlCurrCode.SelectedIndex = intSelectedIndex

    End Sub

    Sub BindCurrCodeList(ByVal pv_strCurrencyCode As String)
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
            ddlCurrCode.SelectedValue = Trim(pv_strCurrencyCode)
        End If
        'If objCurrencyDs.Tables(0).Rows.Count > 0 Then
        '    For intCnt = 0 To objCurrencyDs.Tables(0).Rows.Count - 1
        '        objCurrencyDs.Tables(0).Rows(intCnt).Item("CurrencyCode") = Trim(objCurrencyDs.Tables(0).Rows(intCnt).Item("CurrencyCode"))
        '        objCurrencyDs.Tables(0).Rows(intCnt).Item("Description") = objCurrencyDs.Tables(0).Rows(intCnt).Item("CurrencyCode")
        '        If Trim(objCurrencyDs.Tables(0).Rows(intCnt).Item("CurrencyCode")) = Trim(pv_strCurrencyCode) Then
        '            intSelectedIndex = intCnt
        '        End If
        '    Next
        'End If

        ddlCurrCode.DataSource = objCurrencyDs.Tables(0)
        ddlCurrCode.DataValueField = "CurrencyCode"
        ddlCurrCode.DataTextField = "CodeDescr"
        ddlCurrCode.DataBind()
        'ddlCurrCode.SelectedIndex = intSelectedIndex

    End Sub

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

    Sub NewBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CB_trx_PayDet.aspx")
    End Sub

    Sub CurrencyChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim strParam As String
        Dim strSearch As String
        Dim strSort As String
        Dim strOpCdGet As String = "CM_CLSSETUP_EXCHANGERATE_GET"
        Dim dr As DataRow
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer
        Dim objCurrencyDs As New Object()
        Dim strDate As String = Date_Validation(txtDateCreated.Text, False)

        strSearch = "and exc.SecCurrencyCode = 'IDR' and exc.FirstCurrencyCode = '" & Trim(ddlCurrency.SelectedItem.Value) & "' and exc.Status = '" & objCMSetup.EnumExchangeRateStatus.Active & "' and exc.TransDate = '" & strDate & "' "
        strSort = "order by exc.FirstCurrencyCode "

        strParam = strSearch & "|" & strSort

        Try
            intErrNo = objCMSetup.mtdGetMasterList(strOpCdGet, strParam, 0, objCurrencyDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_CURRENCYLIST_GET&errmesg=" & Exp.Message & "&redirect=CM/Trx/CM_Trx_ContractRegList.aspx")
        End Try

        If objCurrencyDs.Tables(0).Rows.Count > 0 Then
            txtExRate.Text = objCurrencyDs.Tables(0).Rows(0).Item("ExchangeRate")
        Else
            If Trim(ddlCurrency.SelectedItem.Value) <> "IDR" Then
                lblErrExRate.Text = "Exchange rate for this date has not been created."
                lblErrExRate.Visible = True
                Exit Sub
            End If
        End If
    End Sub

    Sub CurrCodeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim strParam As String
        Dim strSearch As String
        Dim strSort As String
        Dim strOpCdGet As String = "CM_CLSSETUP_EXCHANGERATE_GET"
        Dim dr As DataRow
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer
        Dim objCurrencyDs As New Object()
        Dim strDate As String = Date_Validation(txtDateCreated.Text, False)

        strSearch = "and exc.SecCurrencyCode = 'IDR' and exc.FirstCurrencyCode = '" & Trim(ddlCurrCode.SelectedItem.Value) & "' and exc.Status = '" & objCMSetup.EnumExchangeRateStatus.Active & "' and exc.TransDate = '" & strDate & "' "
        strSort = "order by exc.FirstCurrencyCode "

        strParam = strSearch & "|" & strSort

        Try
            intErrNo = objCMSetup.mtdGetMasterList(strOpCdGet, strParam, 0, objCurrencyDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_CURRENCYLIST_GET&errmesg=" & Exp.Message & "&redirect=CM/Trx/CM_Trx_ContractRegList.aspx")
        End Try

        If objCurrencyDs.Tables(0).Rows.Count > 0 Then
            txtExchangeRate.Text = objCurrencyDs.Tables(0).Rows(0).Item("ExchangeRate")
        Else
            If Trim(ddlCurrCode.SelectedItem.Value) <> "IDR" Then
                lblErrExchangeRate.Text = "Exchange rate for this date has not been created."
                lblErrExchangeRate.Visible = True
                Exit Sub
            End If
        End If
    End Sub

    Sub EditBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        txtDateCreated.Enabled = True
        btnDateCreated.Visible = True
        txtGiroDate.Enabled = True
        btnGiroDate.Visible = True
        ddlPayType.Enabled = True
        ddlBank.Enabled = True
        txtChequeNo.Enabled = True
        ddlBank.Enabled = True
        txtChequeNo.Enabled = True
        txtSplBankAccNo.Enabled = True
        txtRemark.Enabled = True
        SaveBtn.Visible = True
        EditBtn.Visible = False
    End Sub

    Sub VerifiedBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim objPAYID As String

        If strSelectedPayID = "" Then
            Exit Sub
        End If
        strSelectedPayID = Trim(lblPaymentID.Text)
        Update_Payment(objCBTrx.EnumPaymentStatus.Verified, objPAYID, strDocNotFulfil)

        strSelectedPayID = objPAYID
        If strSelectedPayID <> "" Then
            RefreshBtn_Click(Sender, E)
        End If
    End Sub

    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim lbl As Label
        Dim strAccCode As String
        Dim intDocType As Integer
        Dim strDocID As String
        Dim strCurrCode As String
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

        dgLineDet.EditItemIndex = CInt(E.Item.ItemIndex)

        lbl = E.Item.FindControl("cnlnid")
        lblTxLnID.Text = lbl.Text.Trim

        lbl = E.Item.FindControl("lblEnumDocType")
        intDocType = lbl.Text.Trim

        lbl = E.Item.FindControl("lblDocId")
        strDocID = lbl.Text.Trim

        BindDocID(intDocType, strDocID)

        lbl = E.Item.FindControl("lblAccCode")
        strAccCode = lbl.Text.Trim
        'BindAccCode(strAccCode)
        GetCOADetail(strAccCode)

        lbl = E.Item.FindControl("lblEnumDocType")
        If lbl.Text.Trim = objCBTrx.EnumPaymentDocType.Payment Then
            'BindOtherAccCode(strDocID)
            GetCOAOtherDetail(strDocID)
            ddlInvoiceRcv.SelectedValue = ""
            ddlDebitNote.SelectedValue = ""
            ddlCreditNote.SelectedValue = ""
            ddlCreditorJournal.SelectedValue = ""
        Else
            'BindOtherAccCode("")
        End If

        lbl = E.Item.FindControl("lblAddNote")
        txtAddNote.Value = lbl.Text.Trim

        lbl = E.Item.FindControl("lblCurrCode")
        strCurrCode = lbl.Text.Trim
        'BindCurrencyList(strCurrCode)
        ddlCurrency.SelectedValue = Trim(strCurrCode)

        lbl = E.Item.FindControl("lblExRate")
        txtExRate.Text = lbl.Text.Trim

        lbl = E.Item.FindControl("lblGiroNo")
        If lbl.Text.Trim <> "" Then
            BindGiroNo(ddlBank.SelectedItem.Value, lbl.Text.Trim)
        End If

        CheckCOATax()
        If hidCOATax.Value = 1 Then
            hidCOATax.Value = 1
            lbl = E.Item.FindControl("lblTaxLnID")
            BindTaxObjectList(strDocID, lbl.Text.Trim)
            lbl = E.Item.FindControl("lblDPPAmount")
            txtDPPAmount.Text = CDbl(lbl.Text.Trim) 'FormatNumber(CDbl(lbl.Text.Trim), 2, True, False, False)
            lbl = E.Item.FindControl("lblTaxRate")
            hidTaxObjectRate.Value = CDbl(lbl.Text.Trim)
            lstTaxObject_OnSelectedIndexChanged(Sender, E)
            txtAmount.ReadOnly = True
        Else
            RowTax.Visible = False
            RowTaxAmt.Visible = False
            hidCOATax.Value = 0
            txtAmount.ReadOnly = False
        End If

        lbl = E.Item.FindControl("lblAmountToDisplay")
        txtAmount.Text = CDbl(lbl.Text.Trim) 'FormatNumber(CDbl(lbl.Text.Trim), 2, True, False, False)
    End Sub

    Sub BindDocID(ByVal pv_IntDocType As String, ByVal pv_DocId As String)
        Dim strOpCode As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim dr As DataRow
        Dim dsMaster As Object
        Dim intSelectedIndex As Integer = 0

        strOpCode = "CB_CLSTRX_PAYMENT_DOCID_BYSUPP_GET"
        strParamName = "INTDOCTYPE|DOCID"
        strParamValue = pv_IntDocType & "|" & Trim(pv_DocId)
        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
                                                strParamName, _
                                                strParamValue, _
                                                dsMaster)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GENERATE_INVOICE_WM_GET_HEADER&errmesg=" & Exp.ToString() & "&redirect=ap/trx/ap_trx_invrcv_wm_list.aspx")
        End Try


        Select Case pv_IntDocType
            Case objCBTrx.EnumPaymentDocType.InvoiceReceive
                For intCnt = 0 To dsMaster.Tables(0).Rows.Count - 1
                    dsMaster.Tables(0).Rows(intCnt).Item("InvoiceRcvID") = dsMaster.Tables(0).Rows(intCnt).Item("InvoiceRcvID").Trim()
                    dsMaster.Tables(0).Rows(intCnt).Item("DispInvoiceRcvID") = dsMaster.Tables(0).Rows(intCnt).Item("InvoiceRcvID") & ", " & dsMaster.Tables(0).Rows(intCnt).Item("CurrencyCode") & " " & dsMaster.Tables(0).Rows(intCnt).Item("OutstandingAmountToDisplay") & ", " & dsMaster.Tables(0).Rows(intCnt).Item("Kurs")
                    If pv_DocId = dsMaster.Tables(0).Rows(intCnt).Item("InvoiceRcvID") Then
                        intSelectedIndex = intCnt + 1
                    End If
                Next

                dr = dsMaster.Tables(0).NewRow()
                dr("InvoiceRcvId") = ""
                dr("DispInvoiceRcvID") = lblPleaseSelect.Text & lblInvoiceRcvTag.Text
                dsMaster.Tables(0).Rows.InsertAt(dr, 0)

                ddlInvoiceRcv.DataSource = dsMaster.Tables(0)
                ddlInvoiceRcv.DataValueField = "InvoiceRcvID"
                ddlInvoiceRcv.DataTextField = "DispInvoiceRcvID"
                ddlInvoiceRcv.DataBind()
                ddlInvoiceRcv.SelectedIndex = intSelectedIndex

                CheckAmount(ddlInvoiceRcv.SelectedItem.Text)

            Case objCBTrx.EnumPaymentDocType.DebitNote
                For intCnt = 0 To dsMaster.Tables(0).Rows.Count - 1
                    dsMaster.Tables(0).Rows(intCnt).Item("DebitNoteId") = dsMaster.Tables(0).Rows(intCnt).Item("DebitNoteId").Trim()
                    dsMaster.Tables(0).Rows(intCnt).Item("DispDebitNoteId") = dsMaster.Tables(0).Rows(intCnt).Item("DebitNoteId") & ", Rp " & dsMaster.Tables(0).Rows(intCnt).Item("OutstandingAmountToDisplay")
                    If pv_DocId = dsMaster.Tables(0).Rows(intCnt).Item("DebitNoteId") Then
                        intSelectedIndex = intCnt + 1
                    End If
                Next

                dr = dsMaster.Tables(0).NewRow()
                dr("DebitNoteID") = ""
                dr("DispDebitNoteId") = lblPleaseSelect.Text & " Debit Note ID"
                dsMaster.Tables(0).Rows.InsertAt(dr, 0)

                ddlDebitNote.DataSource = dsMaster.Tables(0)
                ddlDebitNote.DataValueField = "DebitNoteID"
                ddlDebitNote.DataTextField = "DispDebitNoteId"
                ddlDebitNote.DataBind()
                ddlDebitNote.SelectedIndex = intSelectedIndex

                CheckAmount(ddlDebitNote.SelectedItem.Text)

            Case objCBTrx.EnumPaymentDocType.CreditNote
                For intCnt = 0 To dsMaster.Tables(0).Rows.Count - 1
                    dsMaster.Tables(0).Rows(intCnt).Item("CreditNoteId") = dsMaster.Tables(0).Rows(intCnt).Item("CreditNoteId").Trim()
                    dsMaster.Tables(0).Rows(intCnt).Item("DispCreditNoteId") = dsMaster.Tables(0).Rows(intCnt).Item("CreditNoteId") & ", Rp " & dsMaster.Tables(0).Rows(intCnt).Item("OutstandingAmountToDisplay")
                    If pv_DocId = dsMaster.Tables(0).Rows(intCnt).Item("CreditNoteId") Then
                        intSelectedIndex = intCnt + 1
                    End If
                Next

                dr = dsMaster.Tables(0).NewRow()
                dr("CreditNoteId") = ""
                dr("DispCreditNoteId") = lblPleaseSelect.Text & " Credit Note ID"
                dsMaster.Tables(0).Rows.InsertAt(dr, 0)

                ddlCreditNote.DataSource = dsMaster.Tables(0)
                ddlCreditNote.DataValueField = "CreditNoteId"
                ddlCreditNote.DataTextField = "DispCreditNoteId"
                ddlCreditNote.DataBind()
                ddlCreditNote.SelectedIndex = intSelectedIndex

                CheckAmount(ddlCreditNote.SelectedItem.Text)

            Case objCBTrx.EnumPaymentDocType.Payment
                For intCnt = 0 To dsMaster.Tables(0).Rows.Count - 1
                    If pv_DocId = dsMaster.Tables(0).Rows(intCnt).Item("AccCode") Then
                        intSelectedIndex = intCnt + 1
                    End If
                Next

                dr = dsMaster.Tables(0).NewRow()
                dr("AccCode") = ""
                dr("_Description") = lblPleaseSelect.Text & "Other Cost"
                dsMaster.Tables(0).Rows.InsertAt(dr, 0)

                'ddlOther.DataSource = dsMaster.Tables(0)
                'ddlOther.DataValueField = "AccCode"
                'ddlOther.DataTextField = "_Description"
                'ddlOther.DataBind()
                'ddlOther.SelectedIndex = intSelectedIndex

            Case objCBTrx.EnumPaymentDocType.CreditorJournal
                For intCnt = 0 To dsMaster.Tables(0).Rows.Count - 1
                    dsMaster.Tables(0).Rows(intCnt).Item("CreditJrnID") = dsMaster.Tables(0).Rows(intCnt).Item("CreditJrnID").Trim()
                    dsMaster.Tables(0).Rows(intCnt).Item("DispCreditJrnID") = dsMaster.Tables(0).Rows(intCnt).Item("CreditJrnID") & ", Rp " & dsMaster.Tables(0).Rows(intCnt).Item("OutstandingAmountToDisplay")
                    If pv_DocId = dsMaster.Tables(0).Rows(intCnt).Item("CreditJrnID") Then
                        intSelectedIndex = intCnt + 1
                    End If
                Next

                dr = dsMaster.Tables(0).NewRow()
                dr("CreditJrnID") = ""
                dr("DispCreditJrnID") = lblPleaseSelect.Text & " Creditor Journal ID"
                dsMaster.Tables(0).Rows.InsertAt(dr, 0)

                ddlCreditorJournal.DataSource = dsMaster.Tables(0)
                ddlCreditorJournal.DataValueField = "CreditJrnID"
                ddlCreditorJournal.DataTextField = "DispCreditJrnID"
                ddlCreditorJournal.DataBind()
                ddlCreditorJournal.SelectedIndex = intSelectedIndex

                CheckAmount(ddlCreditorJournal.SelectedItem.Text)

            Case objCBTrx.EnumPaymentDocType.AdvPayment
                For intCnt = 0 To dsMaster.Tables(0).Rows.Count - 1
                    dsMaster.Tables(0).Rows(intCnt).Item("POID") = dsMaster.Tables(0).Rows(intCnt).Item("POID").Trim()
                    dsMaster.Tables(0).Rows(intCnt).Item("DispPOID") = dsMaster.Tables(0).Rows(intCnt).Item("POID").Trim() & ", " & dsMaster.Tables(0).Rows(intCnt).Item("CurrencyCode") & " " & dsMaster.Tables(0).Rows(intCnt).Item("OutstandingAmountToDisplay") & ", " & dsMaster.Tables(0).Rows(intCnt).Item("Kurs")
                    If pv_DocId = Trim(dsMaster.Tables(0).Rows(intCnt).Item("POID")) Then
                        intSelectedIndex = intCnt + 1
                    End If
                Next
                'Response.Write(pv_DocId & "|" & Trim(dsMaster.Tables(0).Rows(0).Item("POID")))
                dr = dsMaster.Tables(0).NewRow()
                dr("POID") = ""
                dr("DispPOID") = lblPleaseSelect.Text & lblInvoiceRcvTag.Text
                dsMaster.Tables(0).Rows.InsertAt(dr, 0)

                ddlPOID.DataSource = dsMaster.Tables(0)
                ddlPOID.DataValueField = "POID"
                ddlPOID.DataTextField = "DispPOID"
                ddlPOID.DataBind()
                ddlPOID.SelectedIndex = intSelectedIndex

                CheckAmount(ddlPOID.SelectedItem.Text)

        End Select
    End Sub

    Sub ForwardBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim intErrNo As Integer
        Dim strOpCd As String = "CB_CLSTRX_PAYMENT_MOVE"
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

        If strSelectedPayID = "" Then
            Exit Sub
        End If

        strParamName = "PAYMENTID|USERID"
        strParamValue = strSelectedPayID & "|" & strUserId

        Try
            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PAYMENT_UPD&errmesg=" & Exp.ToString() & "&redirect=pu/trx/AP_trx_InvoiceRcvList")
        End Try

        onLoad_Display(strSelectedPayID)
        onLoad_Button()
    End Sub

    Private Sub lbViewJournal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbViewJournal.Click
        Dim intErrNo As Integer
        Dim dsResult As New Object
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

        strSelectedPayID = Trim(lblPaymentID.Text)
        onLoad_Display(strSelectedPayID)
        onLoad_DisplayLine(strSelectedPayID)
        onLoad_Button()
    End Sub

    Sub BindPOID(ByVal pv_strSuppCode As String)
        Dim strOpCd As String = "PU_CLSTRX_PO_GET_ADVANCEPAYMENT"
        Dim objPODs As New Object()
        Dim strParam As String
        Dim intCnt As Integer = 0
        Dim intErrNo As Integer
        Dim dr As DataRow
        Dim intSelectedIndex As Integer = 0

        strParamName = "STRSEARCH"
        strParamValue = "AND A.LocCode = '" & strLocation & "' AND (B.SupplierCode LIKE '" & txtSupCode.Text & "' OR B.Name LIKE '%" & txtSupCode.Text & "%') AND A.Status IN ('" & objPU.EnumPOStatus.Confirmed & "')  " & _
                        " AND A.POID NOT IN (Select POID From PU_GoodsRcv Where Status Not In ('" & objPU.EnumGRStatus.Cancelled & "','" & objPU.EnumGRStatus.Deleted & "')) "

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCd, _
                                                strParamName, _
                                                strParamValue, _
                                                objPODs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PU_GET_PO&errmesg=" & Exp.ToString() & "&redirect=pu/trx/pu_trx_GRList.aspx")
        End Try

        For intCnt = 0 To objPODs.Tables(0).Rows.Count - 1
            objPODs.Tables(0).Rows(intCnt).Item("POId") = objPODs.Tables(0).Rows(intCnt).Item("POId").Trim()
            objPODs.Tables(0).Rows(intCnt).Item("DispPOId") = objPODs.Tables(0).Rows(intCnt).Item("POId").Trim() & "," & objPODs.Tables(0).Rows(intCnt).Item("CurrencyCode").Trim() & " " & objPODs.Tables(0).Rows(intCnt).Item("OutstandingAmountCurrency") & ", " & objPODs.Tables(0).Rows(intCnt).Item("Kurs") & ", " & objPODs.Tables(0).Rows(intCnt).Item("PO_PPH23") & ", " & objPODs.Tables(0).Rows(intCnt).Item("PO_PPH21")

            If objPODs.Tables(0).Rows(intCnt).Item("POId") = strSelectedPOId Then
                intSelectedIndex = intCnt + 1
            End If
        Next intCnt

        If Trim(txtSupCode.Text) = "" Then
            objPODs.Tables(0).Clear()
            intSelectedIndex = 0
        End If

        dr = objPODs.Tables(0).NewRow()
        dr("POId") = ""
        dr("DispPOId") = "Please select Purchase Order"
        objPODs.Tables(0).Rows.InsertAt(dr, 0)

        ddlPOID.DataSource = objPODs.Tables(0)
        ddlPOID.DataValueField = "POId"
        ddlPOID.DataTextField = "DispPOId"
        ddlPOID.DataBind()
        ddlPOID.SelectedIndex = intSelectedIndex
        If ddlPOID.SelectedIndex <> -1 Then
            strSelectedPOId = ddlPOID.SelectedItem.Value
        End If
    End Sub

    Sub onSelect_PO(ByVal sender As System.Object, ByVal e As System.EventArgs)
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

        CheckAmount(ddlPOID.SelectedItem.Text)
        'BindCurrencyList(pv_strCurrencyCode)
        ddlCurrency.SelectedValue = Trim(pv_strCurrencyCode)

        If ddlPOID.SelectedIndex <> 0 And ddlBank.SelectedIndex = 0 Then
            'BindAccCode("DUMMY")
            GetCOADetail("DUMMY")
        Else
            'BindAccCode(strBank)
            GetCOADetail(strBank)
        End If
    End Sub

    Sub GetCOADetail(ByVal pv_strCode As String)
        'Dim dr As DataRow
        Dim intCnt As Integer = 0
        Dim intErrNo As Integer
        Dim intSelectedIndex As Integer = 0
        Dim strOpCode As String = "GL_CLSSETUP_ACCOUNTCODE_LIST_GET"
        Dim objCOADs As New DataSet

        Dim strDate As String = Date_Validation(txtDateCreated.Text, False)
        Dim strOpCodeBank As String
        Dim strBank As String
        Dim strBankAccCode As String
        Dim arrParam As Array

        If ddlBank.SelectedItem.Value = "" Then
            txtAccCode.Text = "DUMMY"
            txtAccName.Text = "DUMMY"
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

        'strParamName = "SEARCHSTR|SORTEXP"
        'strParamValue = " And ACC.AccCode = '" & Trim(pv_strCode) & "'  " & "|Order By ACC.AccCode"

        'Try
        '    intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
        '                                        strParamName, _
        '                                        strParamValue, _
        '                                        objCOADs)
        'Catch Exp As System.Exception
        '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GENERATE_INVOICE_WM_GET_HEADER&errmesg=" & Exp.ToString() & "&redirect=ap/trx/ap_trx_invrcv_wm_list.aspx")
        'End Try

        If objAccDs.Tables(0).Rows.Count > 0 Then
            txtAccCode.Text = objAccDs.Tables(0).Rows(0).Item("AccCode")
            txtAccName.Text = objAccDs.Tables(0).Rows(0).Item("Description")
        Else
            txtAccCode.Text = ""
            txtAccName.Text = ""
        End If
    End Sub

    Sub GetCOAOtherDetail(ByVal pv_strCode As String)
        'Dim dr As DataRow
        Dim intCnt As Integer = 0
        Dim intErrNo As Integer
        Dim intSelectedIndex As Integer = 0
        Dim strOpCode As String = "GL_CLSSETUP_ACCOUNTCODE_LIST_GET"
        Dim objCOADs As New DataSet

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
            txtAccCodeOther.Text = objCOADs.Tables(0).Rows(0).Item("AccCode")
            txtAccOtherName.Text = objCOADs.Tables(0).Rows(0).Item("Description")
        Else
            txtAccCodeOther.Text = ""
            txtAccOtherName.Text = ""
        End If
    End Sub

    Sub onSelect_StrAccCode(ByVal sender As System.Object, ByVal e As System.EventArgs)
        GetCOADetail(txtAccCode.Text.Trim)
    End Sub

    'mark 1
    Sub onSelect_StrAccCodeOther(ByVal sender As System.Object, ByVal e As System.EventArgs)
        CheckCOATax()
        GetCOAOtherDetail(txtAccCodeOther.Text.Trim)
    End Sub

    Sub CheckCOATax()
        Dim objTaxDs As New Object
        Dim intErrNo As Integer
        Dim strOpCd As String = "TX_CLSSETUP_TAXOBJECTRATE_LIST_GET"

        strParamName = "STRSEARCH"
        'strParamValue = " AND TOB.AccCode = '" & Trim(ddlOther.SelectedItem.Value) & "' "
        strParamValue = " AND TOB.AccCode = '" & Trim(txtAccCodeOther.Text) & "' "

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
            'BindAccCode("DUMMY")
            GetCOADetail("DUMMY")
            'BindTaxObjectList(ddlOther.SelectedItem.Value, "")
            BindTaxObjectList(txtAccCodeOther.Text, "")
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
        arrParam = Split(lstTaxObject.SelectedItem.Text, ";")

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

    Sub BindRefNo(ByVal pv_strRefNo As String)
        Dim strOpCode As String = "HR_CLSSETUP_STAFF_GET_TRX"
        Dim intErrNo As Integer
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim dsMaster As Object
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer = 0

        strParamName = "LOCCODE|STRSEARCH"
        strParamValue = strLocation & "|" & " AND A.Status='1' AND Outstanding > 0 "

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

        If pv_strGiroNo <> "" Then
            For intCnt = 0 To objBankDs.Tables(0).Rows.Count - 1
                If Trim(pv_strGiroNo) = Trim(objBankDs.Tables(0).Rows(intCnt).Item("DocNo")) Then
                    intSelectedBankIndex = intCnt + 1
                End If
            Next intCnt
        End If

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

