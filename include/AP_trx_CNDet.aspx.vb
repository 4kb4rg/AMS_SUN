
Imports System
Imports System.Data
Imports System.Math
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic


Public Class ap_trx_CNDet : Inherits Page

    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblCreditNoteID As Label
    Protected WithEvents txtCNRefNo As TextBox
    Protected WithEvents txtCNRefDate As TextBox
    Protected WithEvents txtDescription As TextBox
    Protected WithEvents lblAccPeriod As Label
    Protected WithEvents lblStatus As Label

    Protected WithEvents txtSupCode As TextBox
    Protected WithEvents txtSupName As TextBox
    Protected WithEvents txtAccCode As TextBox
    Protected WithEvents txtAccName As TextBox

    Protected WithEvents lblDateCreated As Label
    Protected WithEvents lblLastUpdate As Label
    Protected WithEvents lblPrintDate As Label
    Protected WithEvents lblUpdatedBy As Label

    Protected WithEvents ddlBlock As DropDownList
    Protected WithEvents ddlVehCode As DropDownList
    Protected WithEvents ddlVehExpCode As DropDownList
    Protected WithEvents txtCreditAmount As TextBox
    Protected WithEvents tblSelection As HtmlTable
    Protected WithEvents dgLineDet As DataGrid
    Protected WithEvents lblTotalCreditAmount As Label
    Protected WithEvents lblOutPayAmount As Label
    Protected WithEvents txtRemark As TextBox
    Protected WithEvents Addbtn As ImageButton
    Protected WithEvents SaveBtn As ImageButton
    Protected WithEvents RefreshBtn As ImageButton
    Protected WithEvents ConfirmBtn As ImageButton
    Protected WithEvents PrintBtn As ImageButton
    Protected WithEvents CancelBtn As ImageButton
    Protected WithEvents DeleteBtn As ImageButton
    Protected WithEvents UnDeleteBtn As ImageButton
    Protected WithEvents BackBtn As ImageButton
    Protected WithEvents btnSelDate As Image
    Protected WithEvents cnid As HtmlInputHidden
    Protected WithEvents errReqCTAmount As Label
    Protected WithEvents lblErrSupplier As Label
    Protected WithEvents lblErrCreditAccCode As Label
    Protected WithEvents lblStatusHidden As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents lblPleaseSelect As Label
    Protected WithEvents lblPleaseSelectOneCredit As Label
    Protected WithEvents lblCredit As Label
    Protected WithEvents lblAccount As Label
    Protected WithEvents lblBlock As Label
    Protected WithEvents lblVehicle As Label
    Protected WithEvents lblVehExpense As Label
    Protected WithEvents lblErrBlock As Label
    Protected WithEvents lblErrVehicle As Label
    Protected WithEvents lblErrVehicleExp As Label
    Protected WithEvents lblVehicleOption As Label
    Protected WithEvents RowChargeLevel As HtmlTableRow
    Protected WithEvents RowPreBlk As HtmlTableRow
    Protected WithEvents RowBlk As HtmlTableRow
    Protected WithEvents lblPreBlockErr As Label
    Protected WithEvents lblPreBlkTag As Label
    Protected WithEvents ddlPreBlock As DropDownList
    Protected WithEvents ddlChargeLevel As DropDownList
    Protected WithEvents hidBlockCharge As HtmlInputHidden
    Protected WithEvents hidChargeLocCode As HtmlInputHidden

    Protected WithEvents lblInvoiceRcvRefNo As Label
	Protected WithEvents ddlInvoiceRcvRefNo As DropDownList
    Protected WithEvents lblUnqErrRefNo As Label

    Protected WithEvents lblViewTotalCreditAmount As Label
    Protected WithEvents lblViewOutPayAmount As Label

    Protected WithEvents TrLink As HtmlTableRow
    Protected WithEvents lbViewJournal As LinkButton
    Protected WithEvents dgViewJournal As DataGrid
    Protected WithEvents lblTotalDB As Label
    Protected WithEvents lblTotalCR As Label
    Protected WithEvents lblTotalViewJournal As Label

    Protected WithEvents lblFmtTransDate As Label
    Protected WithEvents lblErrTransDate As Label
    Protected WithEvents lblTxLnID As Label

    Protected WithEvents RowTax As HtmlTableRow
    Protected WithEvents RowTaxAmt As HtmlTableRow
    Protected WithEvents lstTaxObject As DropDownList
    Protected WithEvents lblTaxObjectErr As Label
    Protected WithEvents txtDPPAmount As TextBox
    Protected WithEvents hidNPWPNo As HtmlInputHidden
    Protected WithEvents hidTaxObjectRate As HtmlInputHidden
    Protected WithEvents hidCOATax As HtmlInputHidden
    Protected WithEvents hidTaxStatus As HtmlInputHidden
    Protected WithEvents hidHadCOATax As HtmlInputHidden
    Protected WithEvents lblTaxStatus As Label
    Protected WithEvents lblTaxStatusDesc As Label
    Protected WithEvents lblTaxUpdate As Label
    Protected WithEvents lblTaxUpdateDesc As Label

    Protected WithEvents RowFP As HtmlTableRow
    Protected WithEvents RowFPDate As HtmlTableRow
    Protected WithEvents txtFakturPjkNo As TextBox
    Protected WithEvents hidTaxPPN As HtmlInputHidden
    Protected WithEvents txtFakturDate As TextBox

    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()

    Dim PreBlockTag As String
    Dim BlockTag As String

    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objGLSetup As New agri.GL.clsSetup()
    Dim objPUSetup As New agri.PU.clsSetup()
    Dim objPUTrx As New agri.PU.clsTrx()
    Dim objAPTrx As New agri.AP.clsTrx()    
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim objGLtrx As New agri.GL.ClsTrx()
    Protected objPU As New agri.PU.clsTrx()

    Dim objSuppDs As New Object()
    Dim objAccDs As New Object()
    Dim objCNLnDs As New Object()
    Dim objLangCapDs As New Object()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intAPAR As Integer

    Dim strSelectedCNID As String
    Dim intCNStatus As Integer
    Dim strAcceptDateFormat As String
    Dim intConfig As Integer
    Dim strLocType As String
    Dim strSelAccMonth As String
    Dim strSelAccYear As String
    Dim intLevel As Integer

    Dim TaxLnID As String = ""
    Dim TaxRate As Double = 0
    Dim DPPAmount As Double = 0
    Dim strDateFMT As String

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strAccMonth = Session("SS_APACCMONTH")
        strAccYear = Session("SS_APACCYEAR")
        strSelAccMonth = Session("SS_SELACCMONTH")
        strSelAccYear = Session("SS_SELACCYEAR")
        strLangCode = Session("SS_LANGCODE")
        intAPAR = Session("SS_APAR")
        intConfig = Session("SS_CONFIGSETTING")
        strLocType = Session("SS_LOCTYPE")
        intLevel = Session("SS_USRLEVEL")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumAPAccessRights.APCreditNote), intAPAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else

            txtSupCode.Attributes.Add("readonly", "readonly")
            txtSupName.Attributes.Add("readonly", "readonly")
            txtAccName.Attributes.Add("readonly", "readonly")

            lblErrCreditAccCode.Visible = False
            lblPreBlockErr.Visible = False
            lblErrBlock.Visible = False
            lblErrVehicle.Visible = False
            lblErrVehicleExp.Visible = False
            lblUnqErrRefNo.Visible = False
            
            errReqCTAmount.Visible = False
            lblErrSupplier.Visible = False
            lblErrCreditAccCode.Visible = False
            lblErrTransDate.Visible = False
            lblFmtTransDate.Visible = False

            'to avoid double click, on aspx add this : UseSubmitBehavior="false"
            Addbtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(Addbtn).ToString())
            SaveBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(SaveBtn).ToString())
            RefreshBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(RefreshBtn).ToString())
            ConfirmBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(ConfirmBtn).ToString())
            'PrintBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(PrintBtn).ToString())
            CancelBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(CancelBtn).ToString())
            DeleteBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(DeleteBtn).ToString())
            UnDeleteBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(UnDeleteBtn).ToString())
            BackBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(BackBtn).ToString())


            strSelectedCNID = Trim(IIf(Request.QueryString("cnid") = "", Request.Form("cnid"), Request.QueryString("cnid")))
            cnid.Value = strSelectedCNID
            onload_GetLangCap()
            If Not IsPostBack Then
                BindChargeLevelDropDownList()
                If strSelectedCNID <> "" Then
                    onLoad_Display(strSelectedCNID)
                    onLoad_DisplayLine(strSelectedCNID)
                    onLoad_Button()
                Else
                    txtCNRefDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
                    onLoad_Button()
                    'BindSupplier("")
                    BindInvoiceRcvRefNo("", "")
                    'BindAccCode("")
                    BindPreBlock("", "")
                    BindBlock("", "")
                    BindVehicle("", "")
                    BindVehicleExpense(True, "")
                    TrLink.Visible = True
                End If
            End If
        End If
    End Sub

    Sub BindChargeLevelDropDownList()
        ddlChargeLevel.Items.Add(New ListItem(PreBlockTag, objLangCap.EnumLangCap.Block))
        ddlChargeLevel.Items.Add(New ListItem(BlockTag, objLangCap.EnumLangCap.SubBlock))
        ddlChargeLevel.selectedIndex = Session("SS_BLOCK_CHARGE_DEFAULT")
        RowChargeLevel.Visible = Session("SS_BLOCK_CHARGE_VISIBLE")
        ToggleChargeLevel()
    End Sub
    
    Sub ddlChargeLevel_OnSelectedIndexChanged(sender As Object, e As EventArgs)
        ToggleChargeLevel()
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

    Sub onload_GetLangCap()
        GetEntireLangCap()

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
        'STA Group change this into POID to link Detail Mutasi AP report
        'lblInvoiceRcvRefNo.Text = GetCaption(objLangCap.EnumLangCap.InvoiceReceive) & " Ref. No."
        lblInvoiceRcvRefNo.Text = "Reference No."

        lblErrCreditAccCode.Text = "<br>" & lblPleaseSelectOneCredit.Text & lblAccount.Text
        lblErrBlock.Text = lblPleaseSelectOneCredit.Text & lblBlock.Text
        lblErrVehicle.Text = lblPleaseSelectOneCredit.Text & lblVehicle.Text
        lblErrVehicleExp.Text = lblPleaseSelectOneCredit.Text & lblVehExpense.Text

        dgLineDet.Columns(1).HeaderText = lblCredit.Text & lblAccount.Text
        dgLineDet.Columns(3).HeaderText = lblBlock.Text
        dgLineDet.Columns(4).HeaderText = lblVehicle.Text '& <br> & lblVehExpense.Text
        
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
                    If strLocType = objAdminLoc.EnumLocType.Mill then
                        Return Trim(objLangCapDs.Tables(0).Rows(count).Item("BusinessTermMW"))
                    else
                        Return Trim(objLangCapDs.Tables(0).Rows(count).Item("BusinessTerm"))
                    end if
                    Exit For
                End If
            Next
        End Function



    Sub onLoad_Button()
        Dim intStatus As Integer

        ddlInvoiceRcvRefNo.Enabled = False
        txtRemark.Enabled = False
        txtCNRefNo.Enabled = False
        txtCNRefDate.Enabled = False
        tblSelection.Visible = False

        btnSelDate.Visible = False

        SaveBtn.Visible = False
        RefreshBtn.Visible = False
        ConfirmBtn.Visible = False
        CancelBtn.Visible = False
        DeleteBtn.Visible = False
        UnDeleteBtn.Visible = False

        If (lblStatusHidden.Text <> "") Then
            intStatus = CInt(lblStatusHidden.Text)
            Select Case intStatus
                Case objAPTrx.EnumCreditNoteStatus.Active
                    txtCNRefNo.Enabled = True
                    txtCNRefDate.Enabled = True
                    txtRemark.Enabled = True
                    tblSelection.Visible = True
                    btnSelDate.Visible = True
                    SaveBtn.Visible = True
                    RefreshBtn.Visible = True
                    ConfirmBtn.Visible = True
                    DeleteBtn.Visible = True
                    'If objCNLnDs.Tables(0).Rows.Count = 0 Then
                    ddlInvoiceRcvRefNo.Enabled = True
                    '    txtSupCode.Enabled = True
                    'End If
                Case objAPTrx.EnumCreditNoteStatus.Confirmed
                    If lblTotalCreditAmount.Text = lblOutPayAmount.Text Then
                        CancelBtn.Visible = True
                    End If
                Case objAPTrx.EnumCreditNoteStatus.Deleted
                    UnDeleteBtn.Visible = True
            End Select
        Else
            'ddlSupplier.Enabled = True
            ddlInvoiceRcvRefNo.Enabled = True
            txtCNRefNo.Enabled = True
            txtCNRefDate.Enabled = True
            txtRemark.Enabled = True
            tblSelection.Visible = True
            SaveBtn.Visible = True
            ConfirmBtn.Visible = False
            btnSelDate.Visible = True
        End If
        DeleteBtn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
        UnDeleteBtn.Attributes("onclick") = "javascript:return ConfirmAction('undelete');"
        CancelBtn.Attributes("onclick") = "javascript:return ConfirmAction('cancel');"
    End Sub


    Sub onLoad_Display(ByVal pv_strCreditNoteId As String)
        Dim strOpCd_Get As String = "AP_CLSTRX_CREDITNOTE_DETAILS_GET"
        Dim objCNDs As New Object()
        Dim intErrNo As Integer
        Dim strParam As String = pv_strCreditNoteId
        Dim intCnt As Integer = 0

        cnid.Value = pv_strCreditNoteId

        Try
            intErrNo = objAPTrx.mtdGetCreditNote(strOpCd_Get, _
                                                strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strAccMonth, _
                                                strAccYear, _
                                                strParam, _
                                                objCNDs, _
                                                True)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CREDITNOTE_GET_HEADER&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_cnlist.aspx")
        End Try

        lblCreditNoteID.Text = pv_strCreditNoteId
        txtCNRefNo.Text = Trim(objCNDs.Tables(0).Rows(0).Item("SupplierDocRefNo"))
        txtCNRefDate.Text = Date_Validation(objCNDs.Tables(0).Rows(0).Item("SupplierDocRefDate"), True)
        lblAccPeriod.Text = Trim(objCNDs.Tables(0).Rows(0).Item("AccMonth")) & "/" & Trim(objCNDs.Tables(0).Rows(0).Item("AccYear"))
        lblStatus.Text = objAPTrx.mtdGetDebitNoteStatus(Trim(objCNDs.Tables(0).Rows(0).Item("Status")))
        intCNStatus = CInt(Trim(objCNDs.Tables(0).Rows(0).Item("Status")))
        lblStatusHidden.Text = intCNStatus
        lblDateCreated.Text = objGlobal.GetLongDate(objCNDs.Tables(0).Rows(0).Item("CreateDate"))
        lblLastUpdate.Text = objGlobal.GetLongDate(objCNDs.Tables(0).Rows(0).Item("UpdateDate"))
        lblUpdatedBy.Text = Trim(objCNDs.Tables(0).Rows(0).Item("UserName"))
        lblTotalCreditAmount.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(objCNDs.Tables(0).Rows(0).Item("TotalAmount"), CInt(Session("SS_ROUNDNO"))), CInt(Session("SS_ROUNDNO")))
        lblOutPayAmount.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(objCNDs.Tables(0).Rows(0).Item("OutstandingAmount"), CInt(Session("SS_ROUNDNO"))), CInt(Session("SS_ROUNDNO")))
        lblViewTotalCreditAmount.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(objCNDs.Tables(0).Rows(0).Item("TotalAmount"), CInt(Session("SS_ROUNDNO"))), CInt(Session("SS_ROUNDNO")))
        lblViewOutPayAmount.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(objCNDs.Tables(0).Rows(0).Item("OutstandingAmount"), CInt(Session("SS_ROUNDNO"))), CInt(Session("SS_ROUNDNO")))
        txtRemark.Text = Trim(objCNDs.Tables(0).Rows(0).Item("Remark"))
        txtSupCode.Text = Trim(objCNDs.Tables(0).Rows(0).Item("SupplierCode"))
        txtSupName.Text = Trim(objCNDs.Tables(0).Rows(0).Item("SupplierName"))
        'BindSupplier(Trim(objCNDs.Tables(0).Rows(0).Item("SupplierCode")))
        BindInvoiceRcvRefNo(Trim(objCNDs.Tables(0).Rows(0).Item("SupplierCode")), Trim(objCNDs.Tables(0).Rows(0).Item("InvoiceRcvRefNo")))
        'BindAccCode("")
        BindPreBlock("", "")
        BindBlock("", "")
        BindVehicle("", "")
        BindVehicleExpense(True, "")

    End Sub


    Sub onLoad_DisplayLine(ByVal pv_strCreditNoteId As String)
        Dim strOpCd_GetLine As String = "AP_CLSTRX_CREDITNOTE_LINE_GET"
        Dim strParam As String = pv_strCreditNoteId
        Dim lbDeleteButton As LinkButton
        Dim lbEditButton As LinkButton
        Dim lbCancelButton As LinkButton
        Dim intErrNo As Integer
        Dim intCnt As Integer

        Try
            intErrNo = objAPTrx.mtdGetCreditNoteLine(strOpCd_GetLine, strParam, objCNLnDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CREDITNOTE_GET_LINE&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_cnlist.aspx")
        End Try

        For intCnt = 0 To objCNLnDs.Tables(0).Rows.Count - 1
            objCNLnDs.Tables(0).Rows(intCnt).Item("CreditNoteLnID") = Trim(objCNLnDs.Tables(0).Rows(intCnt).Item("CreditNoteLnID"))
            objCNLnDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objCNLnDs.Tables(0).Rows(intCnt).Item("Description"))
            objCNLnDs.Tables(0).Rows(intCnt).Item("AccCode") = Trim(objCNLnDs.Tables(0).Rows(intCnt).Item("AccCode"))
        Next intCnt

        dgLineDet.DataSource = objCNLnDs.Tables(0)
        dgLineDet.DataBind()

        For intCnt = 0 To objCNLnDs.Tables(0).Rows.Count - 1
            Select Case intCNStatus
                Case objAPTrx.EnumCreditNoteStatus.Active
                    lbDeleteButton = dgLineDet.Items.Item(intCnt).FindControl("lbDelete")
                    lbDeleteButton.Visible = True
                    lbDeleteButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                    lbEditButton = dgLineDet.Items.Item(intCnt).FindControl("lbEdit")
                    lbEditButton.Visible = True
                    lbCancelButton = dgLineDet.Items.Item(intCnt).FindControl("lbCancel")
                    lbCancelButton.Visible = False
                Case objAPTrx.EnumCreditNoteStatus.Confirmed
                    lbDeleteButton = dgLineDet.Items.Item(intCnt).FindControl("lbDelete")
                    lbDeleteButton.Visible = False
                    lbDeleteButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                    lbEditButton = dgLineDet.Items.Item(intCnt).FindControl("lbEdit")
                    lbEditButton.Visible = True
                    lbCancelButton = dgLineDet.Items.Item(intCnt).FindControl("lbCancel")
                    lbCancelButton.Visible = False
                Case Else
                    lbDeleteButton = dgLineDet.Items.Item(intCnt).FindControl("lbDelete")
                    lbDeleteButton.Visible = False
                    lbEditButton = dgLineDet.Items.Item(intCnt).FindControl("lbEdit")
                    lbEditButton.Visible = False
                    lbCancelButton = dgLineDet.Items.Item(intCnt).FindControl("lbCancel")
                    lbCancelButton.Visible = False
            End Select
        Next

        If objCNLnDs.Tables(0).Rows.Count > 0 Then
            TrLink.Visible = True
        Else
            TrLink.Visible = False
        End If
    End Sub

    'Sub BindSupplier(ByVal pv_strSupplierId As String)
    '    Dim strOpCode_GetSupp As String = "PU_CLSSETUP_SUPPLIER_GET"
    '    Dim strParam As String
    '    Dim intErrNo As Integer
    '    Dim intCnt As Integer
    '    Dim intSelectedSuppIndex As Integer = 0
    '    Dim dr As DataRow

    '    strParam = "||" & objPUSetup.EnumSuppStatus.Active & "||SupplierCode||"
    '    strParam = strParam & "|" & IIf(Session("SS_COACENTRALIZED") = "1", "", " A.AccCode in (SELECT AccCode FROM GL_Account WHERE LocCode='" & Trim(strLocation) & "') ")

    '    Try
    '        intErrNo = objPUSetup.mtdGetSupplier(strOpCode_GetSupp, strParam, objSuppDs)
    '    Catch Exp As System.Exception
    '        Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CREDITNOTE_GET_SUPP1&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_cnlist.aspx")
    '    End Try

    '    For intCnt = 0 To objSuppDs.Tables(0).Rows.Count - 1
    '        objSuppDs.Tables(0).Rows(intCnt).Item("SupplierCode") = Trim(objSuppDs.Tables(0).Rows(intCnt).Item("SupplierCode"))
    '        objSuppDs.Tables(0).Rows(intCnt).Item("Name") = Trim(objSuppDs.Tables(0).Rows(intCnt).Item("SupplierCode")) & " (" & Trim(objSuppDs.Tables(0).Rows(intCnt).Item("Name")) & ")"
    '        If objSuppDs.Tables(0).Rows(intCnt).Item("SupplierCode") = pv_strSupplierId Then
    '            intSelectedSuppIndex = intCnt + 1
    '        End If
    '    Next intCnt

    '    dr = objSuppDs.Tables(0).NewRow()
    '    dr("SupplierCode") = ""
    '    dr("Name") = "Please select Supplier Code"
    '    objSuppDs.Tables(0).Rows.InsertAt(dr, 0)
    '    ddlSupplier.DataSource = objSuppDs.Tables(0)
    '    ddlSupplier.DataValueField = "SupplierCode"
    '    ddlSupplier.DataTextField = "Name"
    '    ddlSupplier.DataBind()
    '    ddlSupplier.SelectedIndex = intSelectedSuppIndex
    'End Sub

    Sub BindInvoiceRcvRefNo(ByVal pv_strSupplierCode As String, ByVal pv_strInvoiceRcvRefNo As String)
        'STA Group change this into POID to link Detail Mutasi AP report
        'Dim strOpCode As String = "AP_CLSTRX_INVOICE_RECEIVE_REFNO_GET"
        'Dim strParam As String
        'Dim intErrNo As Integer
        'Dim intCnt As Integer
        'Dim intSelectedIndex As Integer = 0
        'Dim dsInvoiceRcvRefNo As DataSet
        'Dim dr As DataRow

        'Try
        '    strParam = strLocation & "|" & pv_strSupplierCode & "|" & objAPTrx.EnumInvoiceRcvStatus.Confirmed & "', '" & objAPTrx.EnumInvoiceRcvStatus.Closed & "', '" & objAPTrx.EnumInvoiceRcvStatus.Paid
        '    intErrNo = objAPTrx.mtdGetInvoiceRcvRefNo(strOpCode, strParam, dsInvoiceRcvRefNo)
        'Catch Exp As System.Exception
        '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_CLSTRX_INVOICE_RECEIVE_REFNO_GET&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_dnlist.aspx")
        'End Try

        'For intCnt = 0 To dsInvoiceRcvRefNo.Tables(0).Rows.Count - 1
        '    dsInvoiceRcvRefNo.Tables(0).Rows(intCnt).Item("InvoiceRcvRefNo") = Trim(dsInvoiceRcvRefNo.Tables(0).Rows(intCnt).Item("InvoiceRcvRefNo"))
        '    dsInvoiceRcvRefNo.Tables(0).Rows(intCnt).Item("Description") = Trim(dsInvoiceRcvRefNo.Tables(0).Rows(intCnt).Item("InvoiceRcvRefNo"))
        '    If dsInvoiceRcvRefNo.Tables(0).Rows(intCnt).Item("InvoiceRcvRefNo") = pv_strInvoiceRcvRefNo Then
        '        intSelectedIndex = intCnt + 1
        '    End If
        'Next intCnt

        'dr = dsInvoiceRcvRefNo.Tables(0).NewRow()
        'dr("InvoiceRcvRefNo") = ""
        'dr("Description") = "Please select " & lblInvoiceRcvRefNo.Text

        'dsInvoiceRcvRefNo.Tables(0).Rows.InsertAt(dr, 0)
        'ddlInvoiceRcvRefNo.DataSource = dsInvoiceRcvRefNo.Tables(0)
        'ddlInvoiceRcvRefNo.DataValueField = "InvoiceRcvRefNo"
        'ddlInvoiceRcvRefNo.DataTextField = "Description"
        'ddlInvoiceRcvRefNo.DataBind()
        'ddlInvoiceRcvRefNo.SelectedIndex = intSelectedIndex

        Dim strOpCd_GetPO As String = "PU_CLSTRX_PO_INVOICERECEIVE_GET_DNCN"
        Dim objPODs As New Object()
        Dim strParam As String = "||||||||"
        Dim intCnt As Integer = 0
        Dim intErrNo As Integer
        Dim intSelectedIndex As Integer = 0
        Dim blnFound As Boolean = False
        Dim dr As DataRow
        Dim strPOID As String
        Dim strParamName As String
        Dim strParamValue As String

        strParamName = "STRSEARCH"
        strParamName = "STRSEARCH"

        If Len(txtSupCode.Text.Trim) > 0 Then
            If pv_strInvoiceRcvRefNo = "" Then
                strParamValue = "AND A.LocCode = '" & Trim(strLocation) & "' AND A.SupplierCode LIKE '" & IIf(pv_strSupplierCode = "", "XXX", pv_strSupplierCode) & "'" & _
                                " --AND A.POID NOT IN " & _
                                " --(Select Distinct InvoiceRcvRefNo From AP_CreditNote AP Where Status In ('1')) "
            Else
                strParamValue = "AND A.LocCode = '" & Trim(strLocation) & "' AND A.SupplierCode LIKE '" & IIf(pv_strSupplierCode = "", "XXX", pv_strSupplierCode) & "'"
            End If

            Try
                intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_GetPO, _
                                                    strParamName, _
                                                    strParamValue, _
                                                    objPODs)

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_GET_PODETAIL&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
            End Try

            For intCnt = 0 To objPODs.Tables(0).Rows.Count - 1
                objPODs.Tables(0).Rows(intCnt).Item("POID") = Trim(objPODs.Tables(0).Rows(intCnt).Item("POID"))
                objPODs.Tables(0).Rows(intCnt).Item("DispPOId") = Trim(objPODs.Tables(0).Rows(intCnt).Item("POId")) & ", " & _
                                                                  Trim(objPODs.Tables(0).Rows(intCnt).Item("CurrencyCode")) & ", " & _
                                                                  Trim(objPODs.Tables(0).Rows(intCnt).Item("ExchangeRate")) & ", " & _
                                                                  "Rp. " & objPODs.Tables(0).Rows(intCnt).Item("Amount")

                If Trim(objPODs.Tables(0).Rows(intCnt).Item("POID")) = Trim(pv_strInvoiceRcvRefNo) Then
                    intSelectedIndex = intCnt + 1
                End If
            Next intCnt

            If pv_strSupplierCode <> "" Then 'objPODs.Tables(0).Rows.Count > 0 Then
                hidNPWPNo.Value = Trim(objPODs.Tables(0).Rows(0).Item("NPWPNo"))
            End If

            dr = objPODs.Tables(0).NewRow()
            dr("POId") = ""
            dr("DispPOId") = lblPleaseSelect.Text & "reference no."
            objPODs.Tables(0).Rows.InsertAt(dr, 0)

            ddlInvoiceRcvRefNo.DataSource = objPODs.Tables(0)
            ddlInvoiceRcvRefNo.DataValueField = "POId"
            ddlInvoiceRcvRefNo.DataTextField = "DispPOId"
            ddlInvoiceRcvRefNo.DataBind()

        Else
            ddlInvoiceRcvRefNo.Items.Add("Please Select reference no.")
        End If

        ddlInvoiceRcvRefNo.SelectedIndex = intSelectedIndex
        If ddlInvoiceRcvRefNo.SelectedIndex <> -1 Then
            strPOID = ddlInvoiceRcvRefNo.SelectedItem.Value
        End If
    End Sub

    'Sub BindAccCode(ByVal pv_strAccCode As String)
    '    Dim strOpCode_GetAcc As String = "GL_CLSSETUP_ACCOUNTCODE_LIST_GET"
    '    Dim strParam As String
    '    Dim intErrNo As Integer
    '    Dim intCnt As Integer
    '    Dim intSelectedIndex As Integer = 0

    '    Try
    '        strParam = "Order By ACC.AccCode|And ACC.Status = '" & _
    '                    objGLSetup.EnumAccountCodeStatus.Active & _
    '                    "' And ACC.AccType in ('" & objGLSetup.EnumAccountType.BalanceSheet & "','" & objGLSetup.EnumAccountType.ProfitAndLost & "')"

    '        strParam = strParam & IIf(Session("SS_COACENTRALIZED") = "1", "", " AND LocCode = '" & Trim(strLocation) & "' ")

    '        intErrNo = objGLSetup.mtdGetMasterList(strOpCode_GetAcc, strParam, objGLSetup.EnumGLMasterType.AccountCode, objAccDs)
    '    Catch Exp As System.Exception
    '        Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CREDITNOTE_GET_SUPP2&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_cnlist.aspx")
    '    End Try

    '    For intCnt = 0 To objAccDs.Tables(0).Rows.Count - 1
    '        objAccDs.Tables(0).Rows(intCnt).Item("AccCode") = Trim(objAccDs.Tables(0).Rows(intCnt).Item("AccCode"))
    '        objAccDs.Tables(0).Rows(intCnt).Item("Description") = objAccDs.Tables(0).Rows(intCnt).Item("AccCode") & " (" & Trim(objAccDs.Tables(0).Rows(intCnt).Item("Description")) & ")"
    '        If objAccDs.Tables(0).Rows(intCnt).Item("AccCode") = pv_strAccCode Then
    '            intSelectedIndex = intCnt + 1
    '        End If
    '    Next intCnt

    '    Dim dr As DataRow
    '    dr = objAccDs.Tables(0).NewRow()
    '    dr("AccCode") = ""
    '    dr("Description") = lblPleaseSelect.Text & lblAccount.Text
    '    objAccDs.Tables(0).Rows.InsertAt(dr, 0)

    '    ddlCreditAccCode.DataSource = objAccDs.Tables(0)
    '    ddlCreditAccCode.DataValueField = "AccCode"
    '    ddlCreditAccCode.DataTextField = "Description"
    '    ddlCreditAccCode.DataBind()
    '    ddlCreditAccCode.SelectedIndex = intSelectedIndex
    'End Sub

    Sub GetAccountDetails(ByVal pv_strAccCode As String, _
                        ByRef pr_IsBalanceSheet As Boolean, _
                        ByRef pr_blnIsNurseryInd As Boolean, _
                        ByRef pr_IsBlockRequire As Boolean, _
                        ByRef pr_IsVehicleRequire As Boolean, _
                        ByRef pr_IsOthers As Boolean)

        Dim _objAccDs As New Object()
        Dim strOpCd As String = "GL_CLSSETUP_CHARTOFACCOUNT_GET_BY_ACCCODE"
        Dim strParam As String = pv_strAccCode
        Dim intErrNo As Integer

        Try
            pr_IsBalanceSheet = False
            pr_blnIsNurseryInd = False
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
            If CInt(_objAccDs.Tables(0).Rows(0).Item("AccType")) = objGLSetup.EnumAccountType.BalanceSheet Then
                pr_IsBalanceSheet = True
                If CInt(_objAccDs.Tables(0).Rows(0).Item("NurseryInd")) = objGLSetup.EnumNurseryAccount.Yes Then
                    pr_blnIsNurseryInd = True
                End If
            End If
            If CInt(_objAccDs.Tables(0).Rows(0).Item("AccPurpose")) = objGLSetup.EnumAccountPurpose.NonVehicle Then
                pr_IsBlockRequire = True
            ElseIf CInt(_objAccDs.Tables(0).Rows(0).Item("AccPurpose")) = objGLSetup.EnumAccountPurpose.VehicleDistribution Then
                pr_IsVehicleRequire = True
            ElseIf CInt(_objAccDs.Tables(0).Rows(0).Item("AccPurpose")) = objGLSetup.EnumAccountPurpose.Others Then
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

        GetAccountDetails(txtAccCode.Text, blnIsBalanceSheet, blnIsNurseryInd, blnIsBlockRequire, blnIsVehicleRequire, blnIsOthers)

        If Not blnIsBalanceSheet Then
            If blnIsBlockRequire Then
                BindPreBlock(txtAccCode.Text, Request.Form("ddlPreBlock"))
                BindBlock(txtAccCode.Text, Request.Form("ddlBlock"))
                BindVehicle("", strVeh)
                BindVehicleExpense(True, strVehExp)
            Else
                BindPreBlock("", Request.Form("ddlPreBlock"))
                BindBlock("", Request.Form("ddlBlock"))
                BindVehicle("", strVeh)
                BindVehicleExpense(True, strVehExp)
            End If

            If blnIsVehicleRequire Then
                BindVehicle(txtAccCode.Text, strVeh)
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
            BindPreBlock(txtAccCode.Text, Request.Form("ddlPreBlock"))
            BindBlock(txtAccCode.Text, Request.Form("ddlBlock"))
            BindVehicle("", strVeh)
            BindVehicleExpense(True, strVehExp)
        Else
            BindPreBlock("", Request.Form("ddlPreBlock"))
            BindBlock("", Request.Form("ddlBlock"))
            BindVehicle("", strVeh)
            BindVehicleExpense(True, strVehExp)
        End If

        CheckCOATax()
    End Sub

    Sub CheckCOATax()
        Dim strParamName As String
        Dim strParamValue As String
        Dim objTaxDs As New Object
        Dim intErrNo As Integer
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
        Else
            RowTax.Visible = False
            RowTaxAmt.Visible = False
            hidCOATax.Value = 0
            RowFP.Visible = False
            RowFPDate.Visible = False
            hidTaxPPN.Value = 0
            'txtDebitAmount.ReadOnly = True
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=TX_Setup_TaxObjectDet&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To dsForDropDown.Tables(0).Rows.Count - 1
            If Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("TaxLnID")) = Trim(pv_strTaxLnID) Then
                intSelectedIndex = intCnt + 1
            End If
        Next intCnt

        dr = dsForDropDown.Tables(0).NewRow()
        dr("TaxLnID") = ""
        dr("Descr") = "Please Select Tax Object"
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
            txtCreditAmount.ReadOnly = False
            txtCreditAmount.ReadOnly = False
        Else
            hidTaxObjectRate.Value = CDbl(Replace(arrParam(1), "%", ""))
            If txtCreditAmount.Text <> "" Then
                DRAmt = CDbl(IIf(txtDPPAmount.Text = "", 0, txtDPPAmount.Text)) * (hidTaxObjectRate.Value / 100)
                DRAmt = Math.Floor(DRAmt)  'ROUNDDOWN
                'DRAmt = Math.Floor(DRAmt + 0.5)
                txtCreditAmount.Text = DRAmt
            End If

            txtCreditAmount.ReadOnly = True
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
            txtAccCode.Text = objCOADs.Tables(0).Rows(0).Item("AccCode")
            txtAccName.Text = objCOADs.Tables(0).Rows(0).Item("Description")
        Else
            txtAccCode.Text = ""
            txtAccName.Text = ""
        End If
    End Sub

    Sub onSelect_Change(ByVal Sender As System.Object, ByVal E As System.EventArgs)
        BindInvoiceRcvRefNo(txtSupCode.Text, "")
    End Sub

    Sub Update_CreditNote(ByVal pv_intStatus As Integer, ByRef pr_objNewCNID As Object, ByRef pr_objIsValid As Object)
        Dim strCNRefNo As String = txtCNRefNo.Text
        Dim strCNRefDate As String = txtCNRefDate.Text
        Dim strSupplierCode As String = txtSupCode.Text
        Dim strRemark As String = txtRemark.Text
        Dim strOpCd_AddCreditNote As String = "AP_CLSTRX_CREDITNOTE_ADD"
        Dim strOpCd_UpdCreditNote As String = "AP_CLSTRX_CREDITNOTE_UPD"
        Dim strOpCodes As String = strOpCd_AddCreditNote & "|" & _
                                   strOpCd_UpdCreditNote
        Dim intErrNo As Integer
        Dim strParam As String = ""
        Dim strNewIDFormat As String
        Dim objChkRef As Object
        Dim intErrNoRef As Integer
        Dim strParamRef As String = ""
        Dim strParamID As String = ""
        Dim strOpCd_RefNo As String = "AP_CLSTRX_CHK_REF_NO"
        Dim strDate As String = Date_Validation(txtCNRefDate.Text, False)
        Dim indDate As String = ""

        If CheckDate(txtCNRefDate.Text.Trim(), indDate) = False Then
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

        pr_objIsValid = False
        If (strSupplierCode = "") Then
            lblErrSupplier.Visible = True
            Exit Sub
        ElseIf IsDBNull(strCNRefDate) Or strCNRefDate = "" Then
            strCNRefDate = ""
        Else
            strCNRefDate = Date_Validation(strCNRefDate, False)
            If strCNRefDate = "" Then
                lblErrTransDate.Visible = True
                lblFmtTransDate.Visible = True
                lblErrTransDate.Text = "<br>Date Entered should be in the format"
                pr_objIsValid = False
                Exit Sub
            End If
        End If

        If ddlInvoiceRcvRefNo.SelectedItem.Value.Trim = "" Then
            lblErrMessage.Text = "Please select reference no."
            lblErrMessage.Visible = True
            Exit Sub
        End If

        If strSelectedCNID <> "" Then
            strParamID = "AND CreditNoteID NOT LIKE '" & strSelectedCNID & "'"
        Else
            strParamID = ""
        End If

        strParamRef = "AP_CREDITNOTE|SupplierDocRefNo|" & _
                      strParamID & "|" & strCNRefNo & "|" & _
                      strSupplierCode

        Try
            intErrNoRef = objAPTrx.mtdChkRefNo(strOpCd_RefNo, _
                                              strParamRef, _
                                              objChkRef)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_CHK_REF_NO&errmesg=" & Exp.ToString() & "&redirect=pu/trx/ap/trx/ap_trx_invrcvlist.aspx")
        End Try

        If objChkRef.Tables(0).Rows.Count > 0 And strCNRefNo <> "" Then
            lblUnqErrRefNo.Visible = True
            Exit Sub
        End If

        'If Month(strCNRefDate) < strAccMonth And Year(strCNRefDate) <= strAccYear Then
        '    lblErrDate.Visible = True
        '    lblErrDate.Text = "Invalid transaction date."
        '    Exit Sub
        'End If
        strAccYear = Year(strDate)
        strAccMonth = Month(strDate)

        strNewIDFormat = "SCN" & "/" & strCompany & "/" & strLocation & "/" & IIf(Len(Trim(strAccMonth)) = 1, "0" & strAccMonth, strAccMonth) & "/" & strAccYear & "/"

        strParam = strParam & objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.APCreditNote) & "|" & _
                                strSelectedCNID & "|" & _
                                strCNRefNo & "|" & _
                                strDate & "|" & _
                                strSupplierCode & "|" & _
                                strRemark & "|" & _
                                pv_intStatus & "|" & _
                                ddlInvoiceRcvRefNo.SelectedItem.Value.Trim & "|" & _
                                strNewIDFormat

        Try
            intErrNo = objAPTrx.mtdUpdCreditNote(strOpCodes, _
                                                strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strAccMonth, _
                                                strAccYear, _
                                                strParam, _
                                                pr_objNewCNID)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CREDITNOTE_UPD_DATA&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_cnlist.aspx")
        End Try

        pr_objNewCNID = IIf(strSelectedCNID = "", pr_objNewCNID, strSelectedCNID)
        pr_objIsValid = True
    End Sub


    Function Date_Validation(ByVal pv_strInputDate As String, ByVal pv_blnIsShortDate As Boolean) As String
        Dim objSysCfgDs As New Object()
        Dim objActualDate As New Object()
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

    Sub onSelect_StrAccCode(ByVal sender As System.Object, ByVal e As System.EventArgs)
        GetCOADetail(txtAccCode.Text.Trim)
        onSelect_Account(sender, e)
    End Sub

    Sub GetSupplierBtn_Click(ByVal sender As Object, ByVal e As EventArgs)
        BindInvoiceRcvRefNo(txtSupCode.Text, "")
    End Sub

    Sub AddBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCodeGLSubBlkByBlk As String = "GL_CLSSETUP_SUBBLOCK_BY_BLOCK_GET"
        Dim strParamList As String
        Dim objCNID As New Object()
        Dim blnIsValidDate As Boolean
        Dim strDescription As String = Trim(txtDescription.Text)
        Dim strAccCode As String = Request.Form("txtAccCode").Trim
        Dim strBlkCode As String
        Dim strVehCode As String = Request.Form("ddlVehCode").Trim
        Dim strVehExpenseCode As String = Request.Form("ddlVehExpCode").Trim
        Dim strCreditAmount As String = IIf(Trim(Request.Form("txtCreditAmount")) = "", 0, Trim(Request.Form("txtCreditAmount")))
        Dim strDPPAmount As String = IIf(Trim(Request.Form("txtDPPAmount")) = "", 0, Trim(Request.Form("txtDPPAmount")))

        Dim dblAmount As Double = 0
        Dim strOpCode_AddLine As String = "AP_CLSTRX_CREDITNOTE_LINE_ADD"
        Dim strOpCode_UpdLine As String = "AP_CLSTRX_CREDITNOTE_LINE_UPD"
        Dim strOpCode_GetSumAmount As String = "AP_CLSTRX_CREDITNOTE_SUM_LINEAMOUNT_GET"
        Dim strOpCode_UpdTotalAmount As String = "AP_CLSTRX_CREDITNOTE_TOTALAMOUNT_UPD"
        Dim strOpCodes As String = strOpCode_AddLine & "|" & strOpCode_GetSumAmount & "|" & strOpCode_UpdTotalAmount
        Dim intErrNo As Integer

        Dim blnIsBalanceSheet As Boolean
        Dim blnIsNurseryInd As Boolean
        Dim blnIsBlockRequire As Boolean
        Dim blnIsVehicleRequire As Boolean
        Dim blnIsOthers As Boolean

        Dim strFakturNo As String = Trim(Request.Form("txtFakturPjkNo"))
        Dim strFPDate As String = Date_Validation(Request.Form("txtFakturDate"), False)

        Dim strDate As String = Date_Validation(txtCNRefDate.Text, False)
        Dim indDate As String = ""

        If CheckDate(txtCNRefDate.Text.Trim(), indDate) = False Then
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

        If ddlChargeLevel.SelectedIndex = 1 Then
            strBlkCode = Request.Form("ddlBlock")
        Else
            strBlkCode = Request.Form("ddlPreBlock")
        End If
        GetAccountDetails(strAccCode, blnIsBalanceSheet, blnIsNurseryInd, blnIsBlockRequire, blnIsVehicleRequire, blnIsOthers)

        If Not blnIsBalanceSheet Then
            If strAccCode = "" Then
                lblErrCreditAccCode.Visible = True
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
                lblErrCreditAccCode.Visible = True
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

        If hidCOATax.Value = 1 Then
            Dim dblTotalDPP As Double = 0

            TaxLnID = lstTaxObject.SelectedItem.Value
            TaxRate = hidTaxObjectRate.Value

            If Trim(txtSupCode.Text) = "" Then
                lblErrMessage.Visible = True
                lblErrMessage.Text = "This transaction has tax, supplier has to registered first"
                Exit Sub
            End If

            If CDbl(strDPPAmount) <> 0 Then
                If TaxLnID = "" And hidTaxPPN.Value = 0 Then
                    lblTaxObjectErr.Visible = True
                    lblTaxObjectErr.Text = "Please select Tax Object"
                    Exit Sub
                Else
                End If
            End If
            If CDbl(strDPPAmount) <> 0 Then
                DPPAmount = CDbl(strDPPAmount)
            End If
        Else
            TaxLnID = ""
            TaxRate = 0
            DPPAmount = 0
        End If

        If CDbl(strCreditAmount) = 0  And strSelectedCNID <> "" Then
            errReqCTAmount.Visible = True
            onLoad_Button()
            Exit Sub
        Else
            dblAmount = CDbl(strCreditAmount) 
        End If

        If strSelectedCNID = "" Then
            Update_CreditNote(objAPTrx.EnumCreditNoteStatus.Active, objCNID, blnIsValidDate)
            If blnIsValidDate = False Then
                onLoad_Button()
                Exit Sub
            Else
                strSelectedCNID = objCNID
            End If
        End If

        Dim strParam As String = objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.APCreditNoteLn) & "|" & _
                                 strSelectedCNID & "|" & _
                                 strDescription & "|" & _
                                 strAccCode & "|" & _
                                 strBlkCode & "|" & _
                                 strVehCode & "|" & _
                                 strVehExpenseCode & "|" & _
                                 dblAmount & "|" & _
                                 TaxLnID & "|" & _
                                 TaxRate & "|" & _
                                 DPPAmount & "|" & _
                                 Trim(strFakturNo) & "|" & _
                                 strFPDate

        Try
            If lblTxLnID.Text = "" Then
                If ddlChargeLevel.SelectedIndex = 0 And RowPreBlk.Visible = True Then
                    strParamList = Session("SS_LOCATION") & "|" & _
                                           txtAccCode.Text.Trim & "|" & _
                                           ddlPreBlock.SelectedItem.Value.Trim & "|" & _
                                           objGLSetup.EnumBlockStatus.Active & "|" & _
                                           strAccMonth & "|" & strAccYear

                    intErrNo = objAPTrx.mtdUpdCreditNoteLineByBlock(strOpCodeGLSubBlkByBlk, _
                                                                    strParamList, _
                                                                    strOpCodes, _
                                                                    strCompany, _
                                                                    strLocation, _
                                                                    strUserId, _
                                                                    strAccMonth, _
                                                                    strAccYear, _
                                                                    strParam, _
                                                                    strLocType)
                Else
                    intErrNo = objAPTrx.mtdUpdCreditNoteLine(strOpCodes, _
                                                             strCompany, _
                                                             strLocation, _
                                                             strUserId, _
                                                             strAccMonth, _
                                                             strAccYear, _
                                                             strParam)
                End If
            Else
                Dim strParamName As String
                Dim strParamValue As String

                strParamName = "CREDITNOTEID|CREDITNOTELNID|DESCRIPTION|ACCCODE|BLKCODE|VEHCODE|VEHEXPENSECODE|AMOUNT|USERID|TAXLNID|TAXRATE|DPPAMOUNT"
                strParamValue = strSelectedCNID & "|" & lblTxLnID.Text & "|" & strDescription & "|" & _
                                strAccCode & "|" & _
                                strBlkCode & "|" & _
                                strVehCode & "|" & _
                                strVehExpenseCode & "|" & _
                                dblAmount & "|" & _
                                strUserId & "|" & _
                                TaxLnID & "|" & _
                                TaxRate & "|" & _
                                DPPAmount 

                Try
                    intErrNo = objGLtrx.mtdInsertDataCommon(strOpCode_UpdLine, _
                                                            strParamName, _
                                                            strParamValue)

                Catch Exp As System.Exception
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=UPD_ITEM&errmesg=" & Exp.ToString() & "&redirect=pu/trx/AP_trx_InvoiceRcvList")
                End Try
                
                '--ppn/pph
                If hidCOATax.Value = 1 Then
                    Dim strOpCd As String
                    strOpCd = "AP_CLSTRX_CREDITNOTE_TAX_UPDATE"
                    strParamName = "LOCCODE|UPDATEID|CREDITNOTE|CREDITNOTELNID|ACCMONTH|ACCYEAR|SUPPLIERCODE|TAXID|TAXLNID|DPPAMOUNT|TAXAMOUNT|KPPINIT|STATUS|ACCCODE|TAXDATE"
                    strParamValue = strLocation & "|" & strUserId & "|" & Trim(strSelectedCNID) & "|" & Trim(lblTxLnID.Text) & "|" & strAccMonth & "|" & strAccYear & "|" & _
                                    Trim(txtSupCode.Text) & "|" & _
                                    Trim(txtFakturPjkNo.Text) & "|" & _
                                    TaxLnID & "|" & _
                                    DPPAmount & "|" & dblAmount & "|" & _
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
                dgLineDet.EditItemIndex = -1

            End If

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CREDITNOTE_ADD_LINEA&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_cnlist.aspx")
        End Try

        RowTax.Visible = False
        RowTaxAmt.Visible = False
        RowFP.Visible = False
        RowFPDate.Visible = False
        txtCreditAmount.Text = 0

        onLoad_Display(strSelectedCNID)
        onLoad_DisplayLine(strSelectedCNID)
        onLoad_Button()
    End Sub

    Sub NewDNBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("ap_trx_CNDet.aspx")
    End Sub

    Sub SaveBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim objCNID As String
        Dim blnIsValidDate As Boolean

        Update_CreditNote(objAPTrx.EnumCreditNoteStatus.Active, objCNID, blnIsValidDate)
        If blnIsValidDate = True Then
            onLoad_Display(objCNID)
            onLoad_DisplayLine(objCNID)
            onLoad_Button()
        Else
            onLoad_Button()
        End If
    End Sub

    Sub RefreshBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        onLoad_Display(strSelectedCNID)
        onLoad_DisplayLine(strSelectedCNID)
        onLoad_Button()
    End Sub

    Sub ConfirmBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim objCNID As New Object()
        Dim blnIsValidDate As Boolean

        Update_CreditNote(objAPTrx.EnumCreditNoteStatus.Confirmed, objCNID, blnIsValidDate)
        If blnIsValidDate = True Then
            onLoad_Display(objCNID)
            onLoad_DisplayLine(objCNID)
            onLoad_Button()
        End If
    End Sub

    Sub CancelBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim objCNID As New Object()
        Dim blnIsValidDate As Boolean

        Update_CreditNote(objAPTrx.EnumCreditNoteStatus.Cancelled, objCNID, blnIsValidDate)
        If blnIsValidDate = True Then
            onLoad_Display(objCNID)
            onLoad_DisplayLine(objCNID)
            onLoad_Button()
        End If
    End Sub

    Sub DeleteBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim objCNID As New Object()
        Dim blnIsValidDate As Boolean

        Update_CreditNote(objAPTrx.EnumCreditNoteStatus.Deleted, objCNID, blnIsValidDate)
        If blnIsValidDate = True Then
            onLoad_Display(objCNID)
            onLoad_DisplayLine(objCNID)
            onLoad_Button()
        End If
    End Sub

    Sub UnDeleteBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim objCNID As New Object()
        Dim blnIsValidDate As Boolean

        Update_CreditNote(objAPTrx.EnumCreditNoteStatus.Active, objCNID, blnIsValidDate)
        If blnIsValidDate = True Then
            onLoad_Display(objCNID)
            onLoad_DisplayLine(objCNID)
            onLoad_Button()
        End If
    End Sub


    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strOpCode_DelLine As String = "AP_CLSTRX_CREDITNOTE_LINE_DEL"
        Dim strOpCode_GetSumAmount As String = "AP_CLSTRX_CREDITNOTE_SUM_LINEAMOUNT_GET"
        Dim strOpCode_UpdTotalAmount As String = "AP_CLSTRX_CREDITNOTE_TOTALAMOUNT_UPD"
        Dim strOpCodes = strOpCode_DelLine & "|" & strOpCode_GetSumAmount & "|" & strOpCode_UpdTotalAmount
        Dim strParam As String
        Dim lblDelText As Label
        Dim strCNLNId As String
        Dim intErrNo As Integer
        Dim strDate As String = Date_Validation(txtCNRefDate.Text, False)
        Dim indDate As String = ""

        If CheckDate(txtCNRefDate.Text.Trim(), indDate) = False Then
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
        lblDelText = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("cnlnid")
        strCNLNId = lblDelText.Text

        Try
            strParam = strCNLNId & "|" & strSelectedCNID
            intErrNo = objAPTrx.mtdDelCreditNoteLine(strOpCodes, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strAccMonth, _
                                                    strAccYear, _
                                                    strParam)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CREDITNOTE_DEL_LINE&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_cnlist.aspx")
        End Try

        onLoad_Display(strSelectedCNID)
        onLoad_DisplayLine(strSelectedCNID)
        onLoad_Button()
    End Sub

    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim cButton As LinkButton
        Dim lbl As Label
        Dim strAccCode As String
        Dim strVeh As String
        Dim strVehExp As String
        Dim strBlkCode As String
        Dim blnIsBalanceSheet As Boolean
        Dim blnIsNurseryInd As Boolean
        Dim blnIsBlockRequire As Boolean
        Dim blnIsVehicleRequire As Boolean
        Dim blnIsOthers As Boolean
        Dim Delbutton As LinkButton
        Dim strDate As String = Date_Validation(txtCNRefDate.Text, False)
        Dim indDate As String = ""

        If CheckDate(txtCNRefDate.Text.Trim(), indDate) = False Then
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

        'edit confirmed
        If lblStatusHidden.Text = CStr(objAPTrx.EnumCreditNoteStatus.Confirmed) Then
            tblSelection.Visible = True
            txtAccCode.Enabled = True
            'btnFind1.Visible = True
            ddlChargeLevel.Enabled = True
            ddlBlock.Enabled = True
            ddlVehCode.Enabled = True
            ddlVehExpCode.Enabled = True
            txtCreditAmount.Enabled = False
            Delbutton = E.Item.FindControl("lbEdit")
            Delbutton.Visible = False
            Delbutton = E.Item.FindControl("lbCancel")
            Delbutton.Visible = True
        Else
            Delbutton = E.Item.FindControl("lbEdit")
            Delbutton.Visible = False
            Delbutton = E.Item.FindControl("lbDelete")
            Delbutton.Visible = False
            Delbutton = E.Item.FindControl("lbCancel")
            Delbutton.Visible = True
        End If

        dgLineDet.EditItemIndex = CInt(E.Item.ItemIndex)
        lbl = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("cnlnid")
        lblTxLnID.Text = lbl.Text.Trim
        lbl = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lblAmount")
        txtCreditAmount.Text = FormatNumber(Abs(CDbl(lbl.Text.Trim)), 2, True, False, False)

        lbl = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lblDescription")
        txtDescription.Text = lbl.Text
        lbl = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lblAccCode")
        strAccCode = lbl.Text
        lbl = E.Item.FindControl("lblBlkCode")
        strBlkCode = lbl.Text.Trim
        lbl = E.Item.FindControl("lblVehCode")
        strVeh = lbl.Text.Trim
        lbl = E.Item.FindControl("lblVehExpCode")
        strVehExp = lbl.Text.Trim

        txtAccCode.Text = strAccCode

        GetAccountDetails(strAccCode, blnIsBalanceSheet, blnIsNurseryInd, blnIsBlockRequire, blnIsVehicleRequire, blnIsOthers)

        If Not blnIsBalanceSheet Then
            If blnIsBlockRequire Then
                BindPreBlock(txtAccCode.Text, Request.Form("ddlPreBlock"))
                BindBlock(txtAccCode.Text, Request.Form("ddlBlock"))
                BindVehicle("", strVeh)
                BindVehicleExpense(True, strVehExp)
            Else
                BindPreBlock("", Request.Form("ddlPreBlock"))
                BindBlock("", Request.Form("ddlBlock"))
                BindVehicle("", strVeh)
                BindVehicleExpense(True, strVehExp)
            End If

            If blnIsVehicleRequire Then
                BindVehicle(txtAccCode.Text, strVeh)
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
            BindPreBlock(txtAccCode.Text, Request.Form("ddlPreBlock"))
            BindBlock(txtAccCode.Text, Request.Form("ddlBlock"))
            BindVehicle("", strVeh)
            BindVehicleExpense(True, strVehExp)
        Else
            BindPreBlock("", Request.Form("ddlPreBlock"))
            BindBlock("", Request.Form("ddlBlock"))
            BindVehicle("", strVeh)
            BindVehicleExpense(True, strVehExp)
        End If

        CheckCOATax()
        If hidCOATax.Value = 1 Then
            hidCOATax.Value = 1
            lbl = E.Item.FindControl("lblTaxLnID")
            BindTaxObjectList(strAccCode, lbl.Text.Trim)
            lbl = E.Item.FindControl("lblDPPAmount")
            txtDPPAmount.Text = FormatNumber(Abs(CDbl(lbl.Text.Trim)), 2, True, False, False)
            
            lbl = E.Item.FindControl("lblTaxRate")
            hidTaxObjectRate.Value = CDbl(lbl.Text.Trim)
            lstTaxObject_OnSelectedIndexChanged(Sender, E)

            lbl = E.Item.FindControl("lblSPLFaktur")
            txtFakturPjkNo.Text = Trim(lbl.Text)
            lbl = E.Item.FindControl("lblSPLFakturDate")
            txtFakturDate.Text = objGlobal.GetShortDate(strDateFMT, Trim(lbl.Text))

            If Cdbl(txtDPPAmount.Text) <> 0 Then
                txtCreditAmount.ReadOnly = True
            End If
        Else
            hidCOATax.Value = 0
            RowTax.Visible = False
            RowTaxAmt.Visible = False
            'txtCreditAmount.ReadOnly = False
        End If

        cButton = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lbEdit")
        cButton.Visible = False
        cButton = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lbDelete")
        cButton.Visible = False
        cButton = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lbCancel")
        cButton.Visible = True
    End Sub

    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        lblTxLnID.Text = ""
        txtCreditAmount.Text = ""
        txtDescription.Text = ""
        txtAccCode.Text = ""
        ddlBlock.SelectedIndex = 0
        ddlVehCode.SelectedIndex = 0
        ddlVehExpCode.SelectedIndex = 0
        ddlChargeLevel.SelectedIndex = Session("SS_BLOCK_CHARGE_DEFAULT")
        RowChargeLevel.Visible = Session("SS_BLOCK_CHARGE_VISIBLE")
        ToggleChargeLevel()
        onLoad_DisplayLine(strSelectedCNID)
        onLoad_Button()
    End Sub

    Sub BackBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("ap_trx_CNList.aspx")
    End Sub

    Function CheckDate(ByVal pv_strDate As String, ByRef pr_strDate As String) As Boolean
        Dim strDateFormatCode As String = Session("SS_DATEFMT")
        Dim strDateFormat As String

        pr_strDate = ""
        CheckDate = True
        If Not pv_strDate = "" Then
            If objGlobal.mtdValidInputDate(strDateFormatCode, pv_strDate, strDateFormat, pr_strDate) = False Then
                lblFmtTransDate.Text = strDateFormat
                pr_strDate = ""
                CheckDate = False
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
                        Session("SS_USERID") & "|" & Trim(lblCreditNoteID.Text)

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

        strSelectedCNID = Trim(lblCreditNoteID.Text)
        onLoad_Display(strSelectedCNID)
        onLoad_DisplayLine(strSelectedCNID)
        onLoad_Button()
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

