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

Public Class BI_trx_JournalDet : Inherits Page

    Protected WithEvents lblErrConfirmNotFulFil As Label
    Protected WithEvents lblErrConfirmNotFulFilText As Label
    Protected WithEvents lblErrConfirmAllocType As Label
    Protected WithEvents lblErrConfirmTotalAmt As Label
    Protected WithEvents lblErrNoDocID As Label
    Protected WithEvents lblErrDupDoc As Label
    Protected WithEvents lblErrNoRec As Label
    Protected WithEvents lblErrExceed As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblErrNoSelectDoc As Label
    Protected WithEvents lblErrManySelectDoc As Label
    Protected WithEvents lblErrReceipt As Label
    Protected WithEvents lblErrAccount As Label
    Protected WithEvents lblErrBlock As Label
    Protected WithEvents lblErrVehicle As Label
    Protected WithEvents lblErrReqAmount As Label
    Protected WithEvents lblErrAmountZero As Label
    Protected WithEvents lblErrAmount As Label
    Protected WithEvents lblPleaseSelect As Label
    Protected WithEvents lblErrTotal As Label
    Protected WithEvents lblErrAction As Label
    Protected WithEvents lblNoJrnType As Label
    Protected WithEvents lblSelect As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents lblAccount As Label
    Protected WithEvents lblJrnType As Label
    Protected WithEvents lblVehicleOption As Label

    Protected WithEvents lblBillPartyTag As Label
    Protected WithEvents lblJrnTypeTag As Label
    Protected WithEvents lblAccCodeTag As Label
    Protected WithEvents lblBlkCodeTag As Label

    Protected WithEvents lblDebtorJrnID As Label
    Protected WithEvents ddlBillParty As DropDownList
    Protected WithEvents rbVoid As RadioButton
    Protected WithEvents rbWriteOff As RadioButton
    Protected WithEvents rbAdjust As RadioButton
    Protected WithEvents rbAlloc As RadioButton
    Protected WithEvents txtRemark As TextBox
    Protected WithEvents lblTotalAmount As Label

    Protected WithEvents lblAccPeriod As Label
    Protected WithEvents lblStatus As Label
    Protected WithEvents lblDateCreated As Label
    Protected WithEvents lblLastUpdate As Label
    Protected WithEvents lblUpdatedBy As Label

    Protected WithEvents ddlAccMonth As DropDownList
    Protected WithEvents ddlAccYear As DropDownList
    Protected WithEvents ddlInvoice As DropDownList
    Protected WithEvents ddlDebitNote As DropDownList
    Protected WithEvents ddlCreditNote As DropDownList
    Protected WithEvents ddlReceipt As DropDownList
    Protected WithEvents ddlDebtorJrn As DropDownList
    Protected WithEvents ddlAccount As DropDownList
    Protected WithEvents ddlBlock As DropDownList
    Protected WithEvents txtAmount As TextBox
    Protected WithEvents lblPPHRateHidden As Label
    Protected WithEvents lblPPNHidden As Label
    Protected WithEvents lblPPNAmtHidden As Label
    Protected WithEvents lblPPHAmtHidden As Label
    Protected WithEvents lblNetAmtHidden As Label
    Protected WithEvents lblPPN As Label
    Protected WithEvents lblPPH As Label
    Protected WithEvents lblPercen As Label
    Protected WithEvents cbPPN As CheckBox
    Protected WithEvents txtPPHRate As TextBox

    Protected WithEvents dgLineDet As DataGrid

    Protected WithEvents rfvBillParty As RequiredFieldValidator

    Protected WithEvents SaveBtn As ImageButton
    Protected WithEvents RefreshBtn As ImageButton
    Protected WithEvents ConfirmBtn As ImageButton
    Protected WithEvents DeleteBtn As ImageButton
    Protected WithEvents BackBtn As ImageButton
    Protected WithEvents btnFind1 As HtmlInputButton

    Protected WithEvents hidDebtorJrnID As HtmlInputHidden
    Protected WithEvents hidStatus As HtmlInputHidden
    Protected WithEvents hidTotalAmt As HtmlInputHidden

    Protected WithEvents tblSelection As HtmlTable
    Protected WithEvents RowChargeLevel As HtmlTableRow
    Protected WithEvents RowPreBlk As HtmlTableRow
    Protected WithEvents RowBlk As HtmlTableRow
    Protected WithEvents lblPreBlockErr As Label
    Protected WithEvents lblPreBlkTag As Label
    Protected WithEvents ddlPreBlock As DropDownList
    Protected WithEvents ddlChargeLevel As DropDownList
    Protected WithEvents hidBlockCharge As HtmlInputHidden
    Protected WithEvents hidChargeLocCode As HtmlInputHidden
    Protected WithEvents lblViewTotalAmount As Label

    Protected WithEvents TrLink As HtmlTableRow
    Protected WithEvents lbViewJournal As LinkButton
    Protected WithEvents dgViewJournal As DataGrid
    Protected WithEvents lblTotalDB As Label
    Protected WithEvents lblTotalCR As Label
    Protected WithEvents lblTotalViewJournal As Label

    Protected WithEvents txtDateCreated As TextBox
    Protected WithEvents btnDateCreated As Image
    Protected WithEvents lblFmtDate As Label
    Protected WithEvents lblDate As Label

    Dim PreBlockTag As String
    Dim BlockTag As String

    Protected objBITrx As New agri.BI.clsTrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objGLSetup As New agri.GL.clsSetup()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Protected objGLTrx As New agri.GL.ClsTrx()

    Dim objJournalDs As New DataSet()
    Dim objJournalLnDs As New DataSet()
    Dim objBPDs As New Object()
    Dim objRctDs As New Object()
    Dim objInvDs As New Object()
    Dim objDNDs As New Object()
    Dim objCNDs As New Object()
    Dim objDJDs As New Object()
    Dim objAccDs As New Object()
    Dim objBlkDs As New Object()
    Dim objVehDs As New Object()
    Dim objVehExpDs As New Object()
    Dim objLangCapDs As New Object()

    Dim strOpCd_GetLine As String = "BI_CLSTRX_DEBTORJOURNAL_LINE_GET"

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intBIAR As Integer
    Dim intConfig As Integer

    Dim strddlAccMonth As String
    Dim strddlAccYear As String
    Dim strSelectedDebtorJrnID As String
    Dim intDJStatus As Integer
    Dim intJrnType As Integer

    Dim strParam As String = ""
    Dim intErrNo As Integer = 0
    Dim intCnt As Integer = 0
    Dim dr As DataRow
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim strLocType As String
    Dim strSelAccMonth As String
    Dim strSelAccYear As String
    Dim intLevel As Integer

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strAccMonth = Session("SS_ARACCMONTH")
        strAccYear = Session("SS_ARACCYEAR")
        strLangCode = Session("SS_LANGCODE")
        intBIAR = Session("SS_BIAR")
        intConfig = Session("SS_CONFIGSETTING")
        strLocType = Session("SS_LOCTYPE")
        strSelAccMonth = Session("SS_SELACCMONTH")
        strSelAccYear = Session("SS_SELACCYEAR")
        intLevel = Session("SS_USRLEVEL")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumBIAccessRights.BIJournal), intBIAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            onload_GetLangCap()

            lblErrConfirmNotFulFil.Visible = False
            lblErrConfirmAllocType.Visible = False
            lblErrConfirmTotalAmt.Visible = False
            lblErrNoDocID.Visible = False
            lblErrDupDoc.Visible = False
            lblErrNoRec.Visible = False
            lblErrExceed.Visible = False
            lblErrNoSelectDoc.Visible = False
            lblErrManySelectDoc.Visible = False
            lblErrReceipt.Visible = False
            lblErrAccount.Visible = False
            lblPreBlockErr.Visible = False
            lblErrBlock.Visible = False
            lblErrAmount.Visible = False
            lblErrReqAmount.Visible = False
            lblErrAmountZero.Visible = False
            lblNoJrnType.Visible = False
            lblErrAction.Visible = False
            lblDate.Visible = False
            lblFmtDate.Visible = False

            strSelectedDebtorJrnID = Trim(IIf(Request.QueryString("jrnid") = "", Request.Form("hidDebtorJrnID"), Request.QueryString("jrnid")))
            hidDebtorJrnID.Value = strSelectedDebtorJrnID

            If Not IsPostBack Then
                BindChargeLevelDropDownList()
                BindAccMonthList()
                BindAccYearList()
                strddlAccMonth = ddlAccMonth.SelectedItem.Value.Trim
                strddlAccYear = ddlAccYear.SelectedItem.Value.Trim

                If strSelectedDebtorJrnID <> "" Then
                    onLoad_Display(strSelectedDebtorJrnID)
                    onLoad_DisplayLine(strSelectedDebtorJrnID)
                    onLoad_Button()
                Else
                    txtDateCreated.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
                    BindBillParty("")
                    BindInvoice("")
                    BindDebitNote("")
                    BindCreditNote("")
                    BindDebtorJournal("")
                    BindReceipt("")
                    BindAccount("")
                    BindPreBlock("", "")
                    BindBlock("", "")
                    BindInvPPNH("")
                    BindRecPPNH("")
                    lblPPHRateHidden.Text = ""
                    lblPPNHidden.Text = ""
                    lblPPNAmtHidden.Text = ""
                    lblPPHAmtHidden.Text = ""
                    lblNetAmtHidden.Text = ""
                    onLoad_Button()
                    TrLink.Visible = False
                End If

            End If
            strddlAccMonth = ddlAccMonth.SelectedItem.Value.Trim
            strddlAccYear = ddlAccYear.SelectedItem.Value.Trim

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
                lblBlkCodeTag.Text = GetCaption(objLangCap.EnumLangCap.Block) & lblCode.Text
                BlockTag = GetCaption(objLangCap.EnumLangCap.Block)
            Else
                lblBlkCodeTag.Text = GetCaption(objLangCap.EnumLangCap.SubBlock) & lblCode.Text
                BlockTag = GetCaption(objLangCap.EnumLangCap.SubBlock)
            End If
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_JRNDET_GET_LANGCAP_COSTLEVEL&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/BI_trx_JournalList.aspx")
        End Try

        lblBillPartyTag.Text = GetCaption(objLangCap.EnumLangCap.BillParty) & lblCode.Text
        lblAccCodeTag.Text = GetCaption(objLangCap.EnumLangCap.Account) & lblCode.Text

        dgLineDet.Columns(2).HeaderText = lblAccCodeTag.Text
        dgLineDet.Columns(3).HeaderText = lblBlkCodeTag.Text

        rfvBillParty.ErrorMessage = lblPleaseSelect.Text & lblBillPartyTag.Text
        lblErrAccount.Text = lblPleaseSelect.Text & lblAccCodeTag.Text
        lblErrBlock.Text = lblPleaseSelect.Text & lblBlkCodeTag.Text
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_JRNDET_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/BI_trx_JournalList.aspx")
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

        SaveBtn.Visible = False
        ConfirmBtn.Visible = False
        DeleteBtn.Visible = False
        ddlBillParty.Enabled = False
        txtRemark.Enabled = False
        tblSelection.Visible = False
        rbVoid.Enabled = False
        rbWriteOff.Enabled = False
        rbAdjust.Enabled = False
        rbAlloc.Enabled = False
        txtAmount.Text = 0
        dgLineDet.Columns(8).Visible = False
        txtDateCreated.Enabled = False
        btnDateCreated.Visible = False
        If (hidStatus.Value <> "") Then
            onLoad_DisplayLine(hidDebtorJrnID.Value.Trim)
            intStatus = CInt(hidStatus.Value)
            Select Case intStatus
                Case objBITrx.EnumDebtorJournalStatus.Active
                    If objJournalLnDs.Tables(0).Rows.Count = 0 Then
                        Switch_List()
                        txtDateCreated.Enabled = True
                        btnDateCreated.Visible = True
                        ddlBillParty.Enabled = True
                        tblSelection.Visible = True
                        rbVoid.Enabled = True
                        rbWriteOff.Enabled = True
                        rbAdjust.Enabled = True
                        rbAlloc.Enabled = True
                        txtRemark.Enabled = True
                        SaveBtn.Visible = True
                        DeleteBtn.Visible = True
                        DeleteBtn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                    Else
                        onLoad_Display(hidDebtorJrnID.Value.Trim)
                        Switch_List()
                        txtRemark.Enabled = True
                        tblSelection.Visible = True
                        SaveBtn.Visible = True
                        ConfirmBtn.Visible = True
                    End If
                    DeleteBtn.Visible = True
                    DeleteBtn.ImageUrl = "../../images/butt_delete.gif"
                    DeleteBtn.AlternateText = "Delete"
                    DeleteBtn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                    dgLineDet.Columns(8).Visible = True

                Case objBITrx.EnumDebtorJournalStatus.Deleted
                    DeleteBtn.Visible = True
                    DeleteBtn.ImageUrl = "../../images/butt_undelete.gif"
                    DeleteBtn.AlternateText = "Undelete"
                    DeleteBtn.Attributes("onclick") = "javascript:return ConfirmAction('undelete');"
            End Select
        Else
            txtDateCreated.Enabled = True
            btnDateCreated.Visible = True
            ddlBillParty.Enabled = True
            txtRemark.Enabled = True
            tblSelection.Visible = True
            SaveBtn.Visible = True
            rbVoid.Enabled = True
            rbWriteOff.Enabled = True
            rbAdjust.Enabled = True
            rbAlloc.Enabled = True
        End If

    End Sub

    Sub Switch_List()
        ddlAccMonth.Enabled = False
        ddlAccYear.Enabled = False
        RefreshBtn.Visible = False
        ddlInvoice.Enabled = False
        ddlDebitNote.Enabled = False
        ddlCreditNote.Enabled = False
        ddlReceipt.Enabled = False
        ddlDebtorJrn.Enabled = False
        ddlAccount.Enabled = False
        btnFind1.Disabled = True
        ddlChargeLevel.Enabled = False
        ddlPreBlock.Enabled = False
        ddlBlock.Enabled = False
        txtAmount.Enabled = False
        lblPPHRateHidden.Text = ""
        lblPPNHidden.Text = ""
        lblPPNAmtHidden.Text = ""
        lblPPHAmtHidden.Text = ""
        lblNetAmtHidden.Text = ""
        If rbVoid.Checked Then
            intJrnType = objBITrx.EnumDebtorJournalType.Void
            lblJrnType.Text = intJrnType
            ddlAccMonth.Enabled = True
            ddlAccYear.Enabled = True
            RefreshBtn.Visible = True
            ddlReceipt.Enabled = True
            lblPPN.Visible = True
            cbPPN.Enabled = False
            cbPPN.Visible = True
            lblPPH.Visible = True
            lblPercen.Visible = True
            txtPPHRate.Enabled = False
            txtPPHRate.Visible = True
        ElseIf rbWriteOff.Checked Then
            intJrnType = objBITrx.EnumDebtorJournalType.Writeoff
            lblJrnType.Text = intJrnType
            ddlAccMonth.Enabled = True
            ddlAccYear.Enabled = True
            RefreshBtn.Visible = True
            ddlInvoice.Enabled = True
            ddlDebitNote.Enabled = True
            ddlCreditNote.Enabled = True
            ddlDebtorJrn.Enabled = True
            ddlAccount.Enabled = True
            ddlChargeLevel.Enabled = True
            ddlPreBlock.Enabled = True
            ddlBlock.Enabled = True
            btnFind1.Disabled = False
            lblPPN.Visible = True
            cbPPN.Enabled = False
            cbPPN.Visible = True
            lblPPH.Visible = True
            lblPercen.Visible = True
            txtPPHRate.Enabled = False
            txtPPHRate.Visible = True
        ElseIf rbAdjust.Checked Then
            intJrnType = objBITrx.EnumDebtorJournalType.Adjustment
            lblJrnType.Text = intJrnType
            ddlAccount.Enabled = True
            ddlChargeLevel.Enabled = True
            ddlPreBlock.Enabled = True
            ddlBlock.Enabled = True
            btnFind1.Disabled = False
            txtAmount.Enabled = True
            lblPPN.Visible = False
            cbPPN.Enabled = False
            cbPPN.Visible = False
            lblPPH.Visible = False
            lblPercen.Visible = False
            txtPPHRate.Enabled = False
            txtPPHRate.Visible = False
        ElseIf rbAlloc.Checked Then
            intJrnType = objBITrx.EnumDebtorJournalType.Allocation
            lblJrnType.Text = intJrnType
            ddlAccMonth.Enabled = True
            ddlAccYear.Enabled = True
            RefreshBtn.Visible = True
            ddlInvoice.Enabled = True
            ddlDebitNote.Enabled = True
            ddlCreditNote.Enabled = True
            ddlDebtorJrn.Enabled = True
            ddlReceipt.Enabled = True
            txtAmount.Enabled = True
            lblPPN.Visible = True
            cbPPN.Enabled = False
            cbPPN.Visible = True
            lblPPH.Visible = True
            lblPercen.Visible = True
            txtPPHRate.Enabled = False
            txtPPHRate.Visible = True
        End If
        BindInvoice(ddlBillParty.SelectedItem.Value.Trim)
        BindDebitNote(ddlBillParty.SelectedItem.Value.Trim)
        BindCreditNote(ddlBillParty.SelectedItem.Value.Trim)
        BindReceipt(ddlBillParty.SelectedItem.Value.Trim)
        BindDebtorJournal(ddlBillParty.SelectedItem.Value.Trim)
    End Sub

    Sub onLoad_Display(ByVal pv_strDebtorJrnId As String)
        Dim strOpCd_Get As String = "BI_CLSTRX_DEBTORJOURNAL_GET"

        hidDebtorJrnID.Value = pv_strDebtorJrnId
        If pv_strDebtorJrnId <> "" Then
            strParam = "AND DJ.DebtorJrnID = '" & pv_strDebtorJrnId & "'"
            Try
                intErrNo = objBITrx.mtdGetDebtorJournal(strOpCd_Get, _
                                                        strCompany, _
                                                        strLocation, _
                                                        strUserId, _
                                                        strAccMonth, _
                                                        strAccYear, _
                                                        strParam, _
                                                        objJournalDs, _
                                                        True)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_JRNDET_GET_HEADER&errmesg=" & Exp.ToString & "&redirect=BI/trx/BI_trx_JournalList.aspx")
            End Try

            lblDebtorJrnID.Text = pv_strDebtorJrnId
            txtRemark.Text = Trim(objJournalDs.Tables(0).Rows(0).Item("Remark"))
            intJrnType = CInt(Trim(objJournalDs.Tables(0).Rows(0).Item("JrnType")))
            lblJrnType.Text = intJrnType
            lblAccPeriod.Text = Trim(objJournalDs.Tables(0).Rows(0).Item("AccMonth")) & "/" & Trim(objJournalDs.Tables(0).Rows(0).Item("AccYear"))
            lblStatus.Text = objBITrx.mtdGetDebtorJournalStatus(Trim(objJournalDs.Tables(0).Rows(0).Item("Status")))
            intDJStatus = CInt(Trim(objJournalDs.Tables(0).Rows(0).Item("Status")))
            hidStatus.Value = intDJStatus
            lblDateCreated.Text = objGlobal.GetLongDate(objJournalDs.Tables(0).Rows(0).Item("CreateDate"))
            txtDateCreated.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), objJournalDs.Tables(0).Rows(0).Item("CreateDate"))
            lblLastUpdate.Text = objGlobal.GetLongDate(objJournalDs.Tables(0).Rows(0).Item("UpdateDate"))
            lblUpdatedBy.Text = Trim(objJournalDs.Tables(0).Rows(0).Item("UserName"))

            Select Case intJrnType
                Case objBITrx.EnumDebtorJournalType.Void
                    rbVoid.Checked = True
                    rbWriteOff.Checked = False
                    rbAdjust.Checked = False
                    rbAlloc.Checked = False
                Case objBITrx.EnumDebtorJournalType.Writeoff
                    rbVoid.Checked = False
                    rbWriteOff.Checked = True
                    rbAdjust.Checked = False
                    rbAlloc.Checked = False
                Case objBITrx.EnumDebtorJournalType.Adjustment
                    rbVoid.Checked = False
                    rbWriteOff.Checked = False
                    rbAdjust.Checked = True
                    rbAlloc.Checked = False
                Case objBITrx.EnumDebtorJournalType.Allocation
                    rbVoid.Checked = False
                    rbWriteOff.Checked = False
                    rbAdjust.Checked = False
                    rbAlloc.Checked = True
            End Select

            BindBillParty(Trim(objJournalDs.Tables(0).Rows(0).Item("BillPartyCode")))
            BindInvoice(Trim(objJournalDs.Tables(0).Rows(0).Item("BillPartyCode")))
            BindDebitNote(Trim(objJournalDs.Tables(0).Rows(0).Item("BillPartyCode")))
            BindCreditNote(Trim(objJournalDs.Tables(0).Rows(0).Item("BillPartyCode")))
            BindDebtorJournal(Trim(objJournalDs.Tables(0).Rows(0).Item("BillPartyCode")))
            BindReceipt(Trim(objJournalDs.Tables(0).Rows(0).Item("BillPartyCode")))
        Else
            BindBillParty(ddlBillParty.SelectedItem.Value.Trim)
            Switch_List()
            BindInvoice(ddlBillParty.SelectedItem.Value.Trim)
            BindDebitNote(ddlBillParty.SelectedItem.Value.Trim)
            BindCreditNote(ddlBillParty.SelectedItem.Value.Trim)
            BindDebtorJournal(ddlBillParty.SelectedItem.Value.Trim)
        End If
        BindAccount("")
        BindPreBlock("", "")
        BindBlock("", "")
        BindInvPPNH("")
        BindRecPPNH("")
        lblPPHRateHidden.Text = ""
        lblPPNHidden.Text = ""
        lblPPNAmtHidden.Text = ""
        lblPPHAmtHidden.Text = ""
        lblNetAmtHidden.Text = ""
    End Sub

    Sub onLoad_DisplayLine(ByVal pv_strDebtorJrnId As String)
        Dim lbButton As LinkButton
        Dim label As label
        Dim strDocType As String
        Dim decTotalAmount As Decimal = 0

        strParam = pv_strDebtorJrnId
        Try
            intErrNo = objBITrx.mtdGetDebtorJournalLine(strOpCd_GetLine, strParam, objJournalLnDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_JRNDET_GET_LINE&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/BI_trx_JournalList.aspx")
        End Try

        For intCnt = 0 To objJournalLnDs.Tables(0).Rows.Count - 1
            objJournalLnDs.Tables(0).Rows(intCnt).Item("DebtorJrnLnId") = Trim(objJournalLnDs.Tables(0).Rows(intCnt).Item("DebtorJrnLnId"))
            objJournalLnDs.Tables(0).Rows(intCnt).Item("DocID") = Trim(objJournalLnDs.Tables(0).Rows(intCnt).Item("DocID"))
            objJournalLnDs.Tables(0).Rows(intCnt).Item("DocType") = Trim(objJournalLnDs.Tables(0).Rows(intCnt).Item("DocType"))
            objJournalLnDs.Tables(0).Rows(intCnt).Item("AccCode") = Trim(objJournalLnDs.Tables(0).Rows(intCnt).Item("AccCode"))
            objJournalLnDs.Tables(0).Rows(intCnt).Item("BlkCode") = Trim(objJournalLnDs.Tables(0).Rows(intCnt).Item("BlkCode"))

        Next intCnt

        dgLineDet.DataSource = objJournalLnDs.Tables(0)
        dgLineDet.DataBind()

        For intCnt = 0 To objJournalLnDs.Tables(0).Rows.Count - 1
            lbButton = dgLineDet.Items.Item(intCnt).FindControl("Delete")
            lbButton.Visible = True
            lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
            label = dgLineDet.Items.Item(intCnt).FindControl("lblDocType")
            strDocType = label.Text

            Select Case strDocType
                Case objGlobal.EnumDocType.ContractInvoice
                    label.Text = objGlobal.mtdGetDocName(objGlobal.EnumDocType.ContractInvoice)
                Case objGlobal.EnumDocType.BIDebitNote
                    label.Text = objGlobal.mtdGetDocName(objGlobal.EnumDocType.BIDebitNote)
                Case objGlobal.EnumDocType.BICreditNote
                    label.Text = objGlobal.mtdGetDocName(objGlobal.EnumDocType.BICreditNote)
                Case objGlobal.EnumDocType.ARReceipt
                    label.Text = objGlobal.mtdGetDocName(objGlobal.enumdoctype.ARReceipt)
                Case objGlobal.EnumDocType.ARDebtorJournal
                    label.Text = objGlobal.mtdGetDocName(objGlobal.enumdoctype.ARDebtorJournal)
            End Select

            label = dgLineDet.Items.Item(intCnt).FindControl("lblAmount")
            Select Case lblJrnType.Text.Trim
                Case objBITrx.EnumDebtorJournalType.WriteOff
                    If strDocType = objGlobal.EnumDocType.BICreditNote Or Sign(objJournalLnDs.Tables(0).Rows(intCnt).Item("Amount")) = -1 Then
                        label.Text = "(" & FormatNumber(Abs(objJournalLnDs.Tables(0).Rows(intCnt).Item("Amount")), CInt(Session("SS_ROUNDNO"))) & ")"
                        decTotalAmount = Decimal.Subtract(decTotalAmount, Abs(objJournalLnDs.Tables(0).Rows(intCnt).Item("Amount")))
                    Else
                        label.Text = FormatNumber(Trim(objJournalLnDs.Tables(0).Rows(intCnt).Item("Amount")), CInt(Session("SS_ROUNDNO")))
                        decTotalAmount = Decimal.Add(decTotalAmount, Abs(objJournalLnDs.Tables(0).Rows(intCnt).Item("Amount")))
                    End If

                Case objBITrx.EnumDebtorJournalType.Allocation
                    If strDocType = objGlobal.EnumDocType.BICreditNote Or strDocType = objGlobal.EnumDocType.ARReceipt Then
                        label.Text = "(" & FormatNumber(Abs(objJournalLnDs.Tables(0).Rows(intCnt).Item("Amount")), CInt(Session("SS_ROUNDNO"))) & ")"
                        decTotalAmount = Decimal.Subtract(decTotalAmount, Abs(objJournalLnDs.Tables(0).Rows(intCnt).Item("Amount")))
                    Else
                        label.Text = FormatNumber(Trim(objJournalLnDs.Tables(0).Rows(intCnt).Item("Amount")), CInt(Session("SS_ROUNDNO")))
                        decTotalAmount = Decimal.Add(decTotalAmount, Abs(objJournalLnDs.Tables(0).Rows(intCnt).Item("Amount")))
                    End If

                Case Else
                    If Sign(objJournalLnDs.Tables(0).Rows(intCnt).Item("Amount")) = -1 Then
                        label.Text = "(" & FormatNumber(Abs(objJournalLnDs.Tables(0).Rows(intCnt).Item("Amount")), CInt(Session("SS_ROUNDNO"))) & ")"
                        decTotalAmount = Decimal.Subtract(decTotalAmount, Abs(objJournalLnDs.Tables(0).Rows(intCnt).Item("Amount")))
                    Else
                        label.Text = FormatNumber(Trim(objJournalLnDs.Tables(0).Rows(intCnt).Item("Amount")), CInt(Session("SS_ROUNDNO")))
                        decTotalAmount = Decimal.Add(decTotalAmount, Abs(objJournalLnDs.Tables(0).Rows(intCnt).Item("Amount")))
                    End If

            End Select
        Next
        If Sign(decTotalAmount) = -1 Then
            lblViewTotalAmount.Text = "(" & objGlobal.GetIDDecimalSeparator(FormatNumber(Abs(decTotalAmount), 0)) & ")"
            lblTotalAmount.Text = "(" & FormatNumber(Abs(decTotalAmount), CInt(Session("SS_ROUNDNO"))) & ")"
        Else
            lblViewTotalAmount.Text = objGlobal.GetIDDecimalSeparator(FormatNumber(decTotalAmount, 0))
            lblTotalAmount.Text = FormatNumber(decTotalAmount, CInt(Session("SS_ROUNDNO")))
        End If

        If objJournalLnDs.Tables(0).Rows.Count > 0 Then
            TrLink.Visible = True
        Else
            TrLink.Visible = False
        End If
    End Sub

    Sub BindAccMonthList()
        If strAccMonth = 1 Then
            ddlAccMonth.SelectedIndex = 11
        Else
            ddlAccMonth.SelectedIndex = strAccMonth - 1
        End If

    End Sub

    Sub BindAccYearList()
        Dim CurrDate As Date
        Dim CurrYear As Integer
        Dim intCntAddYr As Integer = 1
        Dim intCntMinYr As Integer = 5
        Dim NewAddCurrYear As Integer
        Dim NewMinCurrYear As Integer
        Dim intCntddlYr As Integer = 0

        CurrDate = Today
        CurrYear = CInt(Session("SS_ARACCYEAR"))

        While intCntMinYr <> 0
            intCntMinYr = intCntMinYr - 1
            NewMinCurrYear = CurrYear - intCntMinYr
            ddlAccYear.Items.Add(NewMinCurrYear)
        End While


        For intCntddlYr = 0 To ddlAccYear.Items.Count - 1
            If strAccMonth = 1 Then
                If ddlAccYear.Items(intCntddlYr).Text = strAccYear - 1 Then
                    ddlAccYear.SelectedIndex = intCntddlYr
                End If
            Else
                If ddlAccYear.Items(intCntddlYr).Text = strAccYear Then
                    ddlAccYear.SelectedIndex = intCntddlYr
                End If
            End If
        Next

    End Sub

    Sub onSelect_Change(ByVal Sender As System.Object, ByVal E As System.EventArgs)
        BindBillParty(ddlBillParty.SelectedItem.Value.Trim)
        BindInvoice(ddlBillParty.SelectedItem.Value.Trim)
        BindDebitNote(ddlBillParty.SelectedItem.Value.Trim)
        BindCreditNote(ddlBillParty.SelectedItem.Value.Trim)
        BindDebtorJournal(ddlBillParty.SelectedItem.Value.Trim)
        BindReceipt(ddlBillParty.SelectedItem.Value.Trim)
        onLoad_Button()
    End Sub

    Sub BindBillParty(ByVal pv_strCode As String)
        Dim strOpCd As String = "GL_CLSSETUP_BILLPARTY_GET"
        Dim intSelectedIndex As Integer = 0

        strParam = "||1||BP.BillPartyCode|ASC|"
        Try
            intErrNo = objGLSetup.mtdGetBillParty(strOpCd, strParam, objBPDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_JRNDET_BINDBILLPARTY_GET&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/BI_trx_JournalList.aspx")
        End Try

        For intCnt = 0 To objBPDs.Tables(0).Rows.Count - 1
            objBPDs.Tables(0).Rows(intCnt).Item("BillPartyCode") = Trim(objBPDs.Tables(0).Rows(intCnt).Item("BillPartyCode"))
            objBPDs.Tables(0).Rows(intCnt).Item("Name") = Trim(objBPDs.Tables(0).Rows(intCnt).Item("BillPartyCode")) & " (" & Trim(objBPDs.Tables(0).Rows(intCnt).Item("Name")) & ")"
            If objBPDs.Tables(0).Rows(intCnt).Item("BillPartyCode") = Trim(pv_strCode) Then
                intSelectedIndex = intCnt + 1
            End If
        Next

        dr = objBPDs.Tables(0).NewRow()
        dr("BillPartyCode") = ""
        dr("Name") = lblSelect.Text & lblBillPartyTag.Text
        objBPDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlBillParty.DataSource = objBPDs.Tables(0)
        ddlBillParty.DataValueField = "BillPartyCode"
        ddlBillParty.DataTextField = "Name"
        ddlBillParty.DataBind()
        ddlBillParty.SelectedIndex = intSelectedIndex
    End Sub

    Sub BindInvoice(ByVal pv_strCode As String)
        Dim strOpCd As String = "BI_CLSTRX_FLEXIBLE_INVOICE_GET"
        Dim strBal As String = ""
        Dim strBalGet As String = ", BI.OutstandingAmount"
        Dim strBalChk As String = ""

        If IsNumeric(lblJrnType.Text.Trim()) Then
            Select Case lblJrnType.Text.Trim()

                Case objBITrx.EnumDebtorJournalType.Allocation
                    strBal = "(BI.OutstandingAmount - (SELECT COALESCE(SUM(JL.Amount), 0) FROM BI_DEBTORJOURNAL DJ LEFT OUTER JOIN BI_DEBTORJOURNALLN JL ON DJ.DebtorJrnID = JL.DebtorJrnID WHERE DJ.Status = '" & objBITrx.EnumDebtorJournalStatus.Active & "' AND DJ.JrnType='" & lblJrnType.Text & "' AND JL.DocID = BI.InvoiceID))"
                    strBalGet = ", " & strBal & " AS OutstandingAmount"
                    strBalChk = " AND " & strBal & " > 0 "

                Case objBITrx.EnumDebtorJournalType.WriteOff
                    strBal = "(BI.OutstandingAmount - (SELECT COALESCE(SUM(JL.Amount), 0) FROM BI_DEBTORJOURNAL DJ LEFT OUTER JOIN BI_DEBTORJOURNALLN JL ON DJ.DebtorJrnID = JL.DebtorJrnID WHERE DJ.Status = '" & objBITrx.EnumDebtorJournalStatus.Active & "' AND DJ.JrnType='" & lblJrnType.Text & "' AND JL.DocID = BI.InvoiceID))"
                    strBalGet = ", " & strBal & " AS OutstandingAmount"
                    strBalChk = " AND " & strBal & " > 0 "
            End Select
        End If

        strParam = strBalGet & "|" & _
                   "" & "|" & _
                   "BI.LocCode = '" & strLocation & "' AND BI.AccMonth = '" & strddlAccMonth & "' AND BI.AccYear = '" & strddlAccYear & "' AND BI.BillPartyCode = '" & pv_strCode & "' AND BI.Status IN('" & objBITrx.EnumInvoiceStatus.Confirmed & "', '" & objBITrx.EnumInvoiceStatus.Closed & "') " & strBalChk & "|" & _
                   ""
        Try
            intErrNo = objBITrx.mtdGetIV_DN_CN_RT_DJ(strOpCd, strParam, objInvDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_RECEIPTDET_BIND_INVOICE_GET&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/BI_trx_ReceiptList.aspx")
        End Try

        For intCnt = 0 To objInvDs.Tables(0).Rows.Count - 1
            objInvDs.Tables(0).Rows(intCnt).Item("InvoiceID") = Trim(objInvDs.Tables(0).Rows(intCnt).Item("InvoiceID"))
            objInvDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objInvDs.Tables(0).Rows(intCnt).Item("InvoiceID")) & ", Rp " & objInvDs.Tables(0).Rows(intCnt).Item("OutstandingAmount")
        Next

        dr = objInvDs.Tables(0).NewRow()
        dr("InvoiceID") = ""
        dr("Description") = lblSelect.Text & "Invoice"
        objInvDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlInvoice.DataSource = objInvDs.Tables(0)
        ddlInvoice.DataValueField = "InvoiceID"
        ddlInvoice.DataTextField = "Description"
        ddlInvoice.DataBind()

    End Sub

    Sub BindDebitNote(ByVal pv_strCode As String)
        Dim strOpCd As String = "BI_CLSTRX_FLEXIBLE_DEBIT_NOTE_GET"
        Dim strBal As String = ""
        Dim strBalGet As String = ", BI.OutstandingAmount"
        Dim strBalChk As String = ""

        If IsNumeric(lblJrnType.Text.Trim()) Then
            Select Case lblJrnType.Text.Trim()

                Case objBITrx.EnumDebtorJournalType.Allocation
                    strBal = "(BI.OutstandingAmount - (SELECT COALESCE(SUM(JL.Amount), 0) FROM BI_DEBTORJOURNAL DJ LEFT OUTER JOIN BI_DEBTORJOURNALLN JL ON DJ.DebtorJrnID = JL.DebtorJrnID WHERE DJ.Status = '" & objBITrx.EnumDebtorJournalStatus.Active & "' AND DJ.JrnType='" & lblJrnType.Text & "' AND JL.DocID = BI.DebitNoteID))"
                    strBalGet = ", " & strBal & " AS OutstandingAmount"
                    strBalChk = " AND " & strBal & " > 0 "

                Case objBITrx.EnumDebtorJournalType.WriteOff
                    strBal = "(BI.OutstandingAmount - (SELECT COALESCE(SUM(JL.Amount), 0) FROM BI_DEBTORJOURNAL DJ LEFT OUTER JOIN BI_DEBTORJOURNALLN JL ON DJ.DebtorJrnID = JL.DebtorJrnID WHERE DJ.Status = '" & objBITrx.EnumDebtorJournalStatus.Active & "' AND DJ.JrnType='" & lblJrnType.Text & "' AND JL.DocID = BI.DebitNoteID))"
                    strBalGet = ", " & strBal & " AS OutstandingAmount"
                    strBalChk = " AND " & strBal & " > 0 "
            End Select
        End If

        strParam = strBalGet & "|" & _
                   "" & "|" & _
                   "BI.LocCode = '" & strLocation & "' AND BI.AccMonth = '" & strddlAccMonth & "' AND BI.AccYear = '" & strddlAccYear & "' AND BI.BillPartyCode = '" & pv_strCode & "' AND BI.Status IN('" & objBITrx.EnumDebitNoteStatus.Confirmed & "', '" & objBITrx.EnumDebitNoteStatus.Closed & "') " & strBalChk & "|" & _
                   ""
        Try
            intErrNo = objBITrx.mtdGetIV_DN_CN_RT_DJ(strOpCd, strParam, objDNDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_RECEIPTDET_BIND_DEBIT_NOTE_GET&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/BI_trx_ReceiptList.aspx")
        End Try
        For intCnt = 0 To objDNDs.Tables(0).Rows.Count - 1
            objDNDs.Tables(0).Rows(intCnt).Item("DebitNoteID") = Trim(objDNDs.Tables(0).Rows(intCnt).Item("DebitNoteID"))
            objDNDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objDNDs.Tables(0).Rows(intCnt).Item("DebitNoteID")) & ", Rp " & objDNDs.Tables(0).Rows(intCnt).Item("OutstandingAmount")
        Next

        dr = objDNDs.Tables(0).NewRow()
        dr("DebitNoteID") = ""
        dr("Description") = lblSelect.Text & "Debit Note"
        objDNDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlDebitNote.DataSource = objDNDs.Tables(0)
        ddlDebitNote.DataValueField = "DebitNoteID"
        ddlDebitNote.DataTextField = "Description"
        ddlDebitNote.DataBind()

    End Sub

    Sub BindCreditNote(ByVal pv_strCode As String)
        Dim strOpCd As String = "BI_CLSTRX_FLEXIBLE_CREDIT_NOTE_GET"
        Dim strBal As String = ""
        Dim strBalGet As String = ", BI.OutstandingAmount"
        Dim strBalChk As String = ""

        If IsNumeric(lblJrnType.Text.Trim()) Then
            Select Case lblJrnType.Text.Trim()

                Case objBITrx.EnumDebtorJournalType.Allocation
                    strBal = "(BI.OutstandingAmount - (SELECT COALESCE(SUM(JL.Amount), 0) FROM BI_DEBTORJOURNAL DJ LEFT OUTER JOIN BI_DEBTORJOURNALLN JL ON DJ.DebtorJrnID = JL.DebtorJrnID WHERE DJ.Status = '" & objBITrx.EnumDebtorJournalStatus.Active & "' AND DJ.JrnType='" & lblJrnType.Text & "' AND JL.DocID = BI.CreditNoteID))"
                    strBalGet = ", " & strBal & " AS OutstandingAmount"
                    strBalChk = " AND " & strBal & " > 0 "

                Case objBITrx.EnumDebtorJournalType.WriteOff
                    strBal = "(BI.OutstandingAmount - (SELECT COALESCE(SUM(JL.Amount), 0) FROM BI_DEBTORJOURNAL DJ LEFT OUTER JOIN BI_DEBTORJOURNALLN JL ON DJ.DebtorJrnID = JL.DebtorJrnID WHERE DJ.Status = '" & objBITrx.EnumDebtorJournalStatus.Active & "' AND DJ.JrnType='" & lblJrnType.Text & "' AND JL.DocID = BI.CreditNoteID))"
                    strBalGet = ", " & strBal & " AS OutstandingAmount"
                    strBalChk = " AND " & strBal & " > 0 "
            End Select
        End If

        strParam = strBalGet & "|" & _
                   "" & "|" & _
                   "BI.LocCode = '" & strLocation & "' AND BI.AccMonth = '" & strddlAccMonth & "' AND BI.AccYear = '" & strddlAccYear & "' AND BI.BillPartyCode = '" & pv_strCode & "' AND BI.Status IN('" & objBITrx.EnumCreditNoteStatus.Confirmed & "', '" & objBITrx.EnumCreditNoteStatus.Closed & "') " & strBalChk & "|" & _
                   ""
        Try
            intErrNo = objBITrx.mtdGetIV_DN_CN_RT_DJ(strOpCd, strParam, objCNDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_RECEIPTDET_BIND_CREDIT_NOTE_GET&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/BI_trx_ReceiptList.aspx")
        End Try
        For intCnt = 0 To objCNDs.Tables(0).Rows.Count - 1
            objCNDs.Tables(0).Rows(intCnt).Item("CreditNoteID") = Trim(objCNDs.Tables(0).Rows(intCnt).Item("CreditNoteID"))
            objCNDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objCNDs.Tables(0).Rows(intCnt).Item("CreditNoteID")) & ", Rp " & objCNDs.Tables(0).Rows(intCnt).Item("OutstandingAmount")
        Next

        dr = objCNDs.Tables(0).NewRow()
        dr("CreditNoteID") = ""
        dr("Description") = lblSelect.Text & "Credit Note"
        objCNDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlCreditNote.DataSource = objCNDs.Tables(0)
        ddlCreditNote.DataValueField = "CreditNoteID"
        ddlCreditNote.DataTextField = "Description"
        ddlCreditNote.DataBind()

    End Sub

    Sub BindReceipt(ByVal pv_strBillPartyCode As String)
        Dim strOpCd As String = "BI_CLSTRX_FLEXIBLE_RECEIPT_GET"
        Dim strBal As String = ""
        Dim strBalGet As String = ", BI.OutstandingAmount, BI.TotalAmount"
        Dim strBalChk As String = ""

        If IsNumeric(lblJrnType.Text.Trim()) Then
            Select Case lblJrnType.Text.Trim()

                Case objBITrx.EnumDebtorJournalType.Allocation
                    strBal = "(BI.OutstandingAmount - (SELECT COALESCE(SUM(JL.Amount), 0) FROM BI_DEBTORJOURNAL DJ LEFT OUTER JOIN BI_DEBTORJOURNALLN JL ON DJ.DebtorJrnID = JL.DebtorJrnID WHERE DJ.Status = '" & objBITrx.EnumDebtorJournalStatus.Active & "' AND DJ.JrnType='" & lblJrnType.Text & "' AND JL.DocID = BI.ReceiptID))"
                    strBalGet = ", " & strBal & " AS OutstandingAmount, BI.TotalAmount"
                    strBalChk = " AND " & strBal & " > 0 "
                Case objBITrx.EnumDebtorJournalType.Void
                    strBal = ""
                    strBalGet = ", BI.OutstandingAmount, BI.TotalAmount"
                    strBalChk = " AND BI.ReceiptID NOT IN ( SELECT COALESCE(DocID, '') FROM BI_DEBTORJOURNAL DJ LEFT OUTER JOIN BI_DEBTORJOURNALLN JL ON DJ.DebtorJrnID = JL.DebtorJrnID WHERE DJ.Status = '" & objBITrx.EnumDebtorJournalStatus.Active & "' AND DJ.JrnType='" & lblJrnType.Text & "') "

            End Select
        End If

        strParam = strBalGet & "|" & _
                   "" & "|" & _
                   "BI.LocCode = '" & strLocation & "' AND BI.AccMonth = '" & strddlAccMonth & "' AND BI.AccYear = '" & strddlAccYear & "' AND BI.BillPartyCode = '" & pv_strBillPartyCode & "' AND BI.Status IN('" & objBITrx.EnumDebtorJournalStatus.Confirmed & "', '" & objBITrx.EnumDebtorJournalStatus.Closed & "') " & strBalChk & "|" & _
                   ""
        Try
            intErrNo = objBITrx.mtdGetIV_DN_CN_RT_DJ(strOpCd, strParam, objRctDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_RECEIPTDET_BINDINVOICE_GET&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/BI_trx_ReceiptList.aspx")
        End Try
        For intCnt = 0 To objRctDs.Tables(0).Rows.Count - 1
            objRctDs.Tables(0).Rows(intCnt).Item("ReceiptID") = Trim(objRctDs.Tables(0).Rows(intCnt).Item("ReceiptID"))
            If rbVoid.Checked = True Then
                objRctDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objRctDs.Tables(0).Rows(intCnt).Item("ReceiptID")) & ", Rp " & objRctDs.Tables(0).Rows(intCnt).Item("TotalAmount")
            ElseIf rbAlloc.Checked = True Then
                objRctDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objRctDs.Tables(0).Rows(intCnt).Item("ReceiptID")) & ", Rp " & objRctDs.Tables(0).Rows(intCnt).Item("OutstandingAmount")
            End If
        Next

        dr = objRctDs.Tables(0).NewRow()
        dr("ReceiptID") = ""
        dr("Description") = lblSelect.Text & "Receipt"
        objRctDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlReceipt.DataSource = objRctDs.Tables(0)
        ddlReceipt.DataValueField = "ReceiptID"
        ddlReceipt.DataTextField = "Description"
        ddlReceipt.DataBind()

    End Sub

    Sub BindDebtorJournal(ByVal pv_strCode As String)
        Dim strOpCd As String = "BI_CLSTRX_FLEXIBLE_DEBTOR_JOURNAL_GET"
        Dim strBal As String = ""
        Dim strBalGet As String = ", BI.OutstandingAmount"
        Dim strBalChk As String = ""

        If IsNumeric(lblJrnType.Text.Trim()) Then
            Select Case lblJrnType.Text.Trim()

                Case objBITrx.EnumDebtorJournalType.Allocation
                    strBal = "(BI.OutstandingAmount - (SELECT COALESCE(SUM(JL.Amount), 0) FROM BI_DEBTORJOURNAL DJ LEFT OUTER JOIN BI_DEBTORJOURNALLN JL ON DJ.DebtorJrnID = JL.DebtorJrnID WHERE DJ.Status = '" & objBITrx.EnumDebtorJournalStatus.Active & "' AND DJ.JrnType='" & lblJrnType.Text & "' AND JL.DocID = BI.DebtorJrnID))"
                    strBalGet = ", " & strBal & " AS OutstandingAmount"
                    strBalChk = " AND " & strBal & " <> 0 "

                Case objBITrx.EnumDebtorJournalType.WriteOff
                    strBal = "(BI.OutstandingAmount - (SELECT COALESCE(SUM(JL.Amount), 0) FROM BI_DEBTORJOURNAL DJ LEFT OUTER JOIN BI_DEBTORJOURNALLN JL ON DJ.DebtorJrnID = JL.DebtorJrnID WHERE DJ.Status = '" & objBITrx.EnumDebtorJournalStatus.Active & "' AND DJ.JrnType='" & lblJrnType.Text & "' AND JL.DocID = BI.DebtorJrnID))"
                    strBalGet = ", " & strBal & " AS OutstandingAmount"
                    strBalChk = " AND " & strBal & " <> 0  AND BI.JrnType='" & objBITrx.EnumDebtorJournalType.Adjustment & "'"
            End Select
        End If

        strParam = strBalGet & "|" & _
                   "" & "|" & _
                   "BI.LocCode = '" & strLocation & "' AND BI.AccMonth = '" & strddlAccMonth & "' AND BI.AccYear = '" & strddlAccYear & "' AND BI.BillPartyCode = '" & pv_strCode & "' AND BI.Status IN('" & objBITrx.EnumDebtorJournalStatus.Confirmed & "', '" & objBITrx.EnumDebtorJournalStatus.Closed & "') " & strBalChk & "|" & _
                   ""
        Try
            intErrNo = objBITrx.mtdGetIV_DN_CN_RT_DJ(strOpCd, strParam, objDJDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_RECEIPTDET_BINDINVOICE_GET&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/BI_trx_ReceiptList.aspx")
        End Try
        For intCnt = 0 To objDJDs.Tables(0).Rows.Count - 1
            objDJDs.Tables(0).Rows(intCnt).Item("DebtorJrnID") = Trim(objDJDs.Tables(0).Rows(intCnt).Item("DebtorJrnID"))
            objDJDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objDJDs.Tables(0).Rows(intCnt).Item("DebtorJrnID")) & ", Rp " & objDJDs.Tables(0).Rows(intCnt).Item("OutstandingAmount")
        Next

        dr = objDJDs.Tables(0).NewRow()
        dr("DebtorJrnID") = ""
        dr("Description") = lblSelect.Text & "Debtor Journal"
        objDJDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlDebtorJrn.DataSource = objDJDs.Tables(0)
        ddlDebtorJrn.DataValueField = "DebtorJrnID"
        ddlDebtorJrn.DataTextField = "Description"
        ddlDebtorJrn.DataBind()

    End Sub

    Sub BindAccount(ByVal pv_strAccCode As String)
        Dim strOpCd As String = "GL_CLSSETUP_ACCOUNTCODE_LIST_GET"

        strParam = "Order By ACC.AccCode|AND ACC.Status = '" & objGLSetup.EnumAccountCodeStatus.Active & "' " & vbCrLf & _
                   "AND ACC.AccType IN ('" & objGLSetup.EnumAccountType.BalanceSheet & "','" & objGLSetup.EnumAccountType.ProfitAndLost & "')" & vbCrLf & _
                   "AND ACC.AccPurpose = '" & objGLSetup.EnumAccountPurpose.NonVehicle & "'"
        Try
            intErrNo = objGLSetup.mtdGetMasterList(strOpCd, _
                                                   strParam, _
                                                   objGLSetup.EnumGLMasterType.AccountCode, _
                                                   objAccDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_JRNDET_BINDACCCODE_GET&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/BI_trx_JournalList.aspx")
        End Try

        For intCnt = 0 To objAccDs.Tables(0).Rows.Count - 1
            objAccDs.Tables(0).Rows(intCnt).Item("AccCode") = Trim(objAccDs.Tables(0).Rows(intCnt).Item("AccCode"))
            objAccDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objAccDs.Tables(0).Rows(intCnt).Item("AccCode")) & " (" & Trim(objAccDs.Tables(0).Rows(intCnt).Item("Description")) & ")"
        Next

        dr = objAccDs.Tables(0).NewRow()
        dr("AccCode") = ""
        dr("Description") = lblPleaseSelect.Text & lblAccCodeTag.Text
        objAccDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlAccount.DataSource = objAccDs.Tables(0)
        ddlAccount.DataValueField = "AccCode"
        ddlAccount.DataTextField = "Description"
        ddlAccount.DataBind()

    End Sub

    Sub GetAccountDetails(ByVal pv_strAccCode As String, _
                            ByRef pr_IsBalanceSheet As Boolean, _
                            ByRef pr_IsNurseryInd As Boolean, _
                            ByRef pr_IsBlockRequire As Boolean, _
                            ByRef pr_IsVehicleRequire As Boolean, _
                            ByRef pr_IsOthers As Boolean)

        Dim _objAccDs As New Object()
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_JRNDET_GET_ACCOUNT_DETAILS&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/BI_trx_JournalList.aspx")
        End Try

        If _objAccDs.Tables(0).Rows.Count = 1 Then
            If CInt(_objAccDs.Tables(0).Rows(0).Item("AccType")) = objGLSetup.EnumAccountType.BalanceSheet Then
                pr_IsBalanceSheet = True
                If CInt(_objAccDs.Tables(0).Rows(0).Item("NurseryInd")) = objGLSetup.EnumNurseryAccount.Yes Then
                    pr_IsNurseryInd = True
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

        GetAccountDetails(ddlAccount.SelectedItem.Value, blnIsBalanceSheet, blnIsNurseryInd, blnIsBlockRequire, blnIsVehicleRequire, blnIsOthers)

        If Not blnIsBalanceSheet Then
            If blnIsBlockRequire Then
                BindPreBlock(ddlAccount.SelectedItem.Value, Request.Form("ddlPreBlock"))
                BindBlock(ddlAccount.SelectedItem.Value, Request.Form("ddlBlock"))
            Else
                BindPreBlock("", Request.Form("ddlPreBlock"))
                BindBlock("", Request.Form("ddlBlock"))
            End If
            If blnIsVehicleRequire Then
            End If
            If blnIsOthers Then
            End If
        ElseIf blnIsNurseryInd = True Then
            BindPreBlock(ddlAccount.SelectedItem.Value, Request.Form("ddlPreBlock"))
            BindBlock(ddlAccount.SelectedItem.Value, Request.Form("ddlBlock"))
        Else
            BindPreBlock("", Request.Form("ddlPreBlock"))
            BindBlock("", Request.Form("ddlBlock"))
        End If
    End Sub

    Sub onSelect_RecPPNH(ByVal sender As System.Object, ByVal e As System.EventArgs)
        BindRecPPNH(ddlReceipt.SelectedItem.Value)
    End Sub

    Sub onSelect_InvPPNH(ByVal sender As System.Object, ByVal e As System.EventArgs)
        BindInvPPNH(ddlInvoice.SelectedItem.Value)
    End Sub

    Sub BindInvPPNH(ByVal pv_strSelectedInvID As String)
        Dim strOpCd_GetInvPPNH As String = "BI_CLSTRX_INVOICE_LINE_GET"
        Dim strParam As String = pv_strSelectedInvID
        Dim objInvPPNHDs As New Object()
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim dblPPNAmount As Double
        Dim dblPPHAmount As Double
        Dim dblNetAmount As Double
        Dim dblTotalPPNAmount As Double
        Dim dblTotalPPHAmount As Double
        Dim dblTotalNetAmount As Double
        Dim lblLinePPNAmt As Label
        Dim lblLinePPHAmt As Label
        Dim lblLineNetAmt As Label

        If Trim(pv_strSelectedInvID) = "" Then
            Exit Sub
        End If

        Try
            intErrNo = objBITrx.mtdGetInvoiceLine(strOpCd_GetInvPPNH, strParam, objInvPPNHDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_DEBITNOTEDET_GET_LINE&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/BI_trx_DNList.aspx")
        End Try

        For intCnt = 0 To objInvPPNHDs.Tables(0).Rows.Count - 1
            dblPPNAmount = CDbl(objInvPPNHDs.Tables(0).Rows(intCnt).Item("PPNAmount"))
            dblTotalPPNAmount += dblPPNAmount
            dblPPHAmount = CDbl(objInvPPNHDs.Tables(0).Rows(intCnt).Item("PPHAmount"))
            dblTotalPPHAmount += dblPPHAmount
            dblNetAmount = CDbl(objInvPPNHDs.Tables(0).Rows(intCnt).Item("NetAmount"))
            dblTotalNetAmount += dblNetAmount
            If Trim(objInvPPNHDs.Tables(0).Rows(intCnt).Item("PPHRate")) <> "0" Then
                lblPPHRateHidden.Text = objInvPPNHDs.Tables(0).Rows(intCnt).Item("PPHRate")
            End If
            If Trim(objInvPPNHDs.Tables(0).Rows(intCnt).Item("PPN")) <> "2" Then
                lblPPNHidden.Text = objInvPPNHDs.Tables(0).Rows(intCnt).Item("PPN")
            End If
            txtPPHRate.Text = Trim(objInvPPNHDs.Tables(0).Rows(0).Item("PPHRate"))
            cbPPN.Checked = IIf(objInvPPNHDs.Tables(0).Rows(0).Item("PPN") = objBITrx.EnumPPN.Yes, True, False)
        Next

        lblPPNAmtHidden.Text = dblTotalPPNAmount
        lblPPHAmtHidden.Text = dblTotalPPHAmount
        lblNetAmtHidden.Text = dblTotalNetAmount
    End Sub

    Sub BindRecPPNH(ByVal pv_strSelectedRecID As String)
        Dim strOpCd_GetRecPPNH As String = "BI_CLSTRX_RECEIPT_LINE_GET"
        Dim strParam As String = pv_strSelectedRecID
        Dim objRecPPNHDs As New Object()
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim dblPPNAmount As Double
        Dim dblPPHAmount As Double
        Dim dblNetAmount As Double
        Dim dblTotalPPNAmount As Double
        Dim dblTotalPPHAmount As Double
        Dim dblTotalNetAmount As Double
        Dim lblLinePPNAmt As Label
        Dim lblLinePPHAmt As Label
        Dim lblLineNetAmt As Label

        If Trim(pv_strSelectedRecID) = "" Then
            Exit Sub
        End If

        Try
            intErrNo = objBITrx.mtdGetReceiptLine(strOpCd_GetRecPPNH, strParam, objRecPPNHDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_RECEIPTDET_GET_LINE&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/BI_trx_ReceiptList.aspx")
        End Try

        For intCnt = 0 To objRecPPNHDs.Tables(0).Rows.Count - 1
            dblPPNAmount = CDbl(objRecPPNHDs.Tables(0).Rows(intCnt).Item("PPNAmount"))
            dblTotalPPNAmount += dblPPNAmount
            dblPPHAmount = CDbl(objRecPPNHDs.Tables(0).Rows(intCnt).Item("PPHAmount"))
            dblTotalPPHAmount += dblPPHAmount
            dblNetAmount = CDbl(objRecPPNHDs.Tables(0).Rows(intCnt).Item("NetAmount"))
            dblTotalNetAmount += dblNetAmount
            If Trim(objRecPPNHDs.Tables(0).Rows(intCnt).Item("PPHRate")) <> "0" Then
                lblPPHRateHidden.Text = objRecPPNHDs.Tables(0).Rows(intCnt).Item("PPHRate")
            End If
            If Trim(objRecPPNHDs.Tables(0).Rows(intCnt).Item("PPN")) <> "2" Then
                lblPPNHidden.Text = objRecPPNHDs.Tables(0).Rows(intCnt).Item("PPN")
            End If
            txtPPHRate.Text = Trim(objRecPPNHDs.Tables(0).Rows(0).Item("PPHRate"))
            cbPPN.Checked = IIf(objRecPPNHDs.Tables(0).Rows(0).Item("PPN") = objBITrx.EnumPPN.Yes, True, False)
        Next

        lblPPNAmtHidden.Text = dblTotalPPNAmount
        lblPPHAmtHidden.Text = dblTotalPPHAmount
        lblNetAmtHidden.Text = dblTotalNetAmount
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
        Dim intSelectedIndex As Integer

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfig) = True Then
            strOpCd = "GL_CLSSETUP_ACCOUNT_BLOCK_GET"
            strParam = pv_strAccCode & "|" & Session("SS_LOCATION") & "|" & objGLSetup.EnumBlockStatus.Active
        Else
            strOpCd = "GL_CLSSETUP_ACCOUNT_SUBBLOCK_GET"
            strParam = pv_strAccCode & "|" & Session("SS_LOCATION") & "|" & objGLSetup.EnumSubBlockStatus.Active
        End If
        Try
            intErrNo = objGLSetup.mtdGetAccountBlock(strOpCd, _
                                                     strParam, _
                                                     objBlkDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_JRNDET_BLOCK_GET&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/BI_trx_JournalList.aspx")
        End Try

        For intCnt = 0 To objBlkDs.Tables(0).Rows.Count - 1
            objBlkDs.Tables(0).Rows(intCnt).Item("BlkCode") = Trim(objBlkDs.Tables(0).Rows(intCnt).Item("BlkCode"))
            objBlkDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objBlkDs.Tables(0).Rows(intCnt).Item("BlkCode")) & " (" & Trim(objBlkDs.Tables(0).Rows(intCnt).Item("Description")) & ")"
            If objBlkDs.Tables(0).Rows(intCnt).Item("BlkCode") = Trim(pv_strBlkCode) Then
                intSelectedIndex = intCnt + 1
            End If
        Next

        dr = objBlkDs.Tables(0).NewRow()
        dr("BlkCode") = ""
        dr("Description") = lblPleaseSelect.Text & lblBlkCodeTag.Text
        objBlkDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlBlock.DataSource = objBlkDs.Tables(0)
        ddlBlock.DataValueField = "BlkCode"
        ddlBlock.DataTextField = "Description"
        ddlBlock.DataBind()
        ddlBlock.SelectedIndex = intSelectedIndex
    End Sub











    Sub onCheck_Change(ByVal Sender As Object, ByVal E As EventArgs)
        Switch_List()
        BindInvoice(ddlBillParty.SelectedItem.Value.Trim)
        BindDebitNote(ddlBillParty.SelectedItem.Value.Trim)
        BindCreditNote(ddlBillParty.SelectedItem.Value.Trim)
        BindDebtorJournal(ddlBillParty.SelectedItem.Value.Trim)
        BindReceipt(ddlBillParty.SelectedItem.Value.Trim)
        onLoad_Button()
    End Sub

    Sub Update_DebtorJournal(ByVal pv_intStatus As Integer, ByRef pr_objNewID As Object)
        Dim strBillPartyCode As String = ddlBillParty.SelectedItem.Value.Trim
        Dim strRemark As String = txtRemark.Text.Trim
        Dim strOpCd_Add As String = "BI_CLSTRX_DEBTORJOURNAL_ADD"
        Dim strOpCd_Upd As String = "BI_CLSTRX_DEBTORJOURNAL_UPD"
        Dim strOpCodes As String = strOpCd_Add & "|" & _
                                   strOpCd_Upd
        Dim strDate As String = Date_Validation(txtDateCreated.Text, False)
        Dim strNewIDFormat As String
        Dim indDate As String = ""

        If CheckDate(txtDateCreated.Text.Trim(), indDate) = False Then
            lblDate.Visible = True
            lblFmtDate.Visible = True
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

        strAccYear = Year(strDate)
        strAccMonth = Month(strDate)

        strNewIDFormat = "DJO" & "/" & strCompany & "/" & strLocation & "/" & strAccYear & IIf(Len(Trim(strAccMonth)) = 1, "0" & strAccMonth, strAccMonth) & "/"

        strParam = lblJrnType.Text & "|" & _
                   objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.ARDebtorJournal) & "|" & _
                   lblDebtorJrnID.Text.Trim & "|" & _
                   strBillPartyCode & "|" & _
                   strRemark & "|" & _
                   pv_intStatus & "|" & _
                   strDate & "|" & _
                   strNewIDFormat

        Try
            intErrNo = objBITrx.mtdUpdDebtorJournal(strOpCodes, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strAccMonth, _
                                                    strAccYear, _
                                                    strParam, _
                                                    pr_objNewID)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_JRNDET_UPD&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/BI_trx_JournalList.aspx")
        End Try


        pr_objNewID = IIf(strSelectedDebtorJrnID = "", pr_objNewID, lblDebtorJrnID.Text.Trim)
    End Sub

    Sub dgLineDet_OnDeleteCommand(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strOpCodes As String
        Dim strParam As String
        Dim lblDG As Label
        Dim strDebtorJrnID As String
        Dim strDebtorJrnLnID As String

        If hidStatus.Value.Trim = Trim(CStr(objBITrx.EnumDebtorJournalStatus.Active)) Then
            strDebtorJrnID = lblDebtorJrnID.Text.Trim()
            lblDG = E.Item.FindControl("lblLnID")
            strDebtorJrnLnID = lblDG.Text.Trim()
            strParam = strDebtorJrnID & "|" & _
                       strDebtorJrnLnID & "|" & _
                       lblJrnType.Text
            strOpCodes = "BI_CLSTRX_DEBTORJOURNAL_LINE_DEL" & "|" & _
                         "BI_CLSTRX_DEBTORJOURNAL_SUM_LINE_GET" & "|" & _
                         "BI_CLSTRX_DEBTORJOURNAL_UPD"
            Try
                intErrNo = objBITrx.mtdDelDebtorJournalLine(strOpCodes, _
                                                            strCompany, _
                                                            strLocation, _
                                                            strUserId, _
                                                            strAccMonth, _
                                                            strAccYear, _
                                                            strParam)
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_JRNDET_DELETE_LINE&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/BI_trx_JournalList.aspx")
                End If
            End Try

            onLoad_Display(strSelectedDebtorJrnID)
            onLoad_DisplayLine(strSelectedDebtorJrnID)
            onLoad_Button()
        End If
    End Sub

    Sub AddBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCodeGLSubBlkByBlk As String = "GL_CLSSETUP_SUBBLOCK_BY_BLOCK_GET"
        Dim strParamList As String
        Dim objUpdStatus As Integer
        Dim objDebtorJrnID As String
        Dim blnIsBalanceSheet As Boolean
        Dim blnIsNurseryInd As Boolean
        Dim blnIsBlockRequire As Boolean
        Dim blnIsVehicleRequire As Boolean
        Dim blnIsOthers As Boolean
        Dim strInvoice As String = ddlInvoice.SelectedItem.Value.Trim
        Dim strDebitNote As String = ddlDebitNote.SelectedItem.Value.Trim
        Dim strCreditNote As String = ddlCreditNote.SelectedItem.Value.Trim
        Dim strDebtorJrn As String = ddlDebtorJrn.SelectedItem.Value.Trim
        Dim strReceipt As String = ddlReceipt.SelectedItem.Value.Trim
        Dim strAccCode As String = Request.Form("ddlAccount")
        Dim strPreBlkCode As String = Request.Form("ddlPreBlock")
        Dim strBlkCode As String = Request.Form("ddlBlock")
        Dim dblAmount As Double
        Dim strOpCode_AddLine As String = "BI_CLSTRX_DEBTORJOURNAL_LINE_ADD"
        Dim strOpCode_GetSumDJLine As String = "BI_CLSTRX_DEBTORJOURNAL_SUM_LINE_GET"
        Dim strOpCode_DebtorJrn_Upd As String = "BI_CLSTRX_DEBTORJOURNAL_UPD"
        Dim strOpCode_GetInvTotal As String = "BI_CLSTRX_INVOICE_OUTSTDAMT_GET"
        Dim strOpCode_GetDNTotal As String = "BI_CLSTRX_DEBITNOTE_OUTSTDAMT_GET"
        Dim strOpCode_GetCNTotal As String = "BI_CLSTRX_CREDITNOTE_OUTSTDAMT_GET"
        Dim strOpCode_GetRctTotal As String = "BI_CLSTRX_RECEIPT_OUTSTDAMT_GET"
        Dim strOpCode_GetDJTotal As String = "BI_CLSTRX_DEBTORJOURNAL_OUTSTDAMT_GET"
        Dim strOpCode_Invoice_Upd As String = "BI_CLSTRX_INVOICE_UPD"
        Dim strOpCode_DebitNote_Upd As String = "BI_CLSTRX_DEBITNOTE_UPD"
        Dim strOpCode_CreditNote_Upd As String = "BI_CLSTRX_CREDITNOTE_UPD"
        Dim strOpCode_Receipt_Upd As String = "BI_CLSTRX_RECEIPT_UPD"
        Dim strOpCodes As String = strOpCode_AddLine & "|" & _
                                   strOpCode_GetSumDJLine & "|" & _
                                   strOpCode_DebtorJrn_Upd & "|" & _
                                   strOpCode_GetInvTotal & "|" & _
                                   strOpCode_GetDNTotal & "|" & _
                                   strOpCode_GetCNTotal & "|" & _
                                   strOpCode_GetRctTotal & "|" & _
                                   strOpCode_Invoice_Upd & "|" & _
                                   strOpCode_DebitNote_Upd & "|" & _
                                   strOpCode_CreditNote_Upd & "|" & _
                                   strOpCode_Receipt_Upd & "|" & _
                                   strOpCode_GetDJTotal

        If (rbVoid.Checked = False) And (rbWriteOff.Checked = False) And (rbAdjust.Checked = False) And (rbAlloc.Checked = False) Then
            lblNoJrnType.Visible = True
            onLoad_Button()
            Exit Sub
        Else
            intJrnType = CInt(lblJrnType.Text)
        End If

        GetAccountDetails(strAccCode, blnIsBalanceSheet, blnIsNurseryInd, blnIsBlockRequire, blnIsVehicleRequire, blnIsOthers)
        If ddlChargeLevel.SelectedIndex = 0 Then
            strBlkCode = strPreBlkCode
        End If
        If (rbVoid.Checked = False) And (rbAlloc.Checked = False) Then
            If blnIsBalanceSheet = False Then
                If strBlkCode = "" Then
                    If ddlChargeLevel.SelectedIndex = 0 Then
                        lblPreBlockErr.Visible = True
                    Else
                        lblErrBlock.Visible = True
                    End If
                    BindAccount(strAccCode)
                    onLoad_Button()
                    Exit Sub
                End If
            ElseIf blnIsNurseryInd = True Then
                If strBlkCode = "" Then
                    If ddlChargeLevel.SelectedIndex = 0 Then
                        lblPreBlockErr.Visible = True
                    Else
                        lblErrBlock.Visible = True
                    End If
                    BindAccount(strAccCode)
                    onLoad_Button()
                    Exit Sub
                End If
            End If
        End If


        If rbVoid.Checked = True Or rbWriteOff.Checked = True Or rbAlloc.Checked = True Then
            If CheckDocs() = False Then
                Exit Sub
            End If
        End If

        If rbWriteOff.Checked = True Or rbAdjust.Checked = True Then
            If strAccCode = "" Then
                lblErrAccount.Visible = True
                BindInvoice(ddlInvoice.SelectedItem.Value)
                BindDebitNote(ddlDebitNote.SelectedItem.Value)
                BindCreditNote(ddlCreditNote.SelectedItem.Value)
                BindDebtorJournal(ddlCreditNote.SelectedItem.Value)
                BindReceipt(ddlReceipt.SelectedItem.Value)
                BindAccount(ddlAccount.SelectedItem.Value)
                BindPreBlock(ddlAccount.SelectedItem.Value, ddlPreBlock.SelectedItem.Value)
                BindBlock(ddlAccount.SelectedItem.Value, ddlBlock.SelectedItem.Value)
                onLoad_Button()
                Exit Sub
            End If
        End If

        If (intJrnType = objBITrx.EnumDebtorJournalType.Adjustment) Or (intJrnType = objBITrx.EnumDebtorJournalType.Allocation) Then
            If Trim(txtAmount.Text) = "" Then
                lblErrReqAmount.Visible = True
                BindInvoice(ddlInvoice.SelectedItem.Value)
                BindDebitNote(ddlDebitNote.SelectedItem.Value)
                BindCreditNote(ddlCreditNote.SelectedItem.Value)
                BindDebtorJournal(ddlCreditNote.SelectedItem.Value)
                BindReceipt(ddlReceipt.SelectedItem.Value)
                BindAccount(ddlAccount.SelectedItem.Value)
                BindPreBlock(ddlAccount.SelectedItem.Value, ddlPreBlock.SelectedItem.Value)
                BindBlock(ddlAccount.SelectedItem.Value, ddlBlock.SelectedItem.Value)
                BindInvPPNH(ddlInvoice.SelectedItem.Value)
                BindRecPPNH(ddlReceipt.SelectedItem.Value)
                onLoad_Button()
                Exit Sub
            Else
                dblAmount = CDbl(txtAmount.Text)
                If dblAmount = 0 Then
                    lblErrAmountZero.Visible = True
                    BindInvoice(ddlInvoice.SelectedItem.Value)
                    BindDebitNote(ddlDebitNote.SelectedItem.Value)
                    BindCreditNote(ddlCreditNote.SelectedItem.Value)
                    BindDebtorJournal(ddlCreditNote.SelectedItem.Value)
                    BindReceipt(ddlReceipt.SelectedItem.Value)
                    BindAccount(ddlAccount.SelectedItem.Value)
                    BindPreBlock(ddlAccount.SelectedItem.Value, ddlPreBlock.SelectedItem.Value)
                    BindBlock(ddlAccount.SelectedItem.Value, ddlBlock.SelectedItem.Value)
                    BindInvPPNH(ddlInvoice.SelectedItem.Value)
                    BindRecPPNH(ddlReceipt.SelectedItem.Value)
                    onLoad_Button()
                    Exit Sub
                ElseIf dblAmount < 0 And (intJrnType = objBITrx.EnumDebtorJournalType.Allocation) And strCreditNote.Trim <> "" Then
                    lblErrAmount.Visible = True
                    BindInvoice(ddlInvoice.SelectedItem.Value)
                    BindDebitNote(ddlDebitNote.SelectedItem.Value)
                    BindCreditNote(ddlCreditNote.SelectedItem.Value)
                    BindDebtorJournal(ddlCreditNote.SelectedItem.Value)
                    BindReceipt(ddlReceipt.SelectedItem.Value)
                    BindAccount(ddlAccount.SelectedItem.Value)
                    BindPreBlock(ddlAccount.SelectedItem.Value, ddlPreBlock.SelectedItem.Value)
                    BindBlock(ddlAccount.SelectedItem.Value, ddlBlock.SelectedItem.Value)
                    BindInvPPNH(ddlInvoice.SelectedItem.Value)
                    BindRecPPNH(ddlReceipt.SelectedItem.Value)
                    onLoad_Button()
                    Exit Sub
                End If
            End If
        End If

        If lblDebtorJrnID.Text = "" Then
            Update_DebtorJournal(objBITrx.EnumDebtorJournalStatus.Active, objDebtorJrnID)
            strSelectedDebtorJrnID = objDebtorJrnID
            lblDebtorJrnID.Text = objDebtorJrnID
        End If

        strParam = intJrnType & "|" & _
                    objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.ARDebtorJournalLn) & "|" & _
                    lblDebtorJrnID.Text & "|" & _
                    strInvoice & "|" & _
                    strDebitNote & "|" & _
                    strCreditNote & "|" & _
                    strDebtorJrn & "|" & _
                    strReceipt & "|" & _
                    strAccCode & "|" & _
                    strBlkCode & "|" & _
                    dblAmount & "|" & _
                    IIf(lblPPHRateHidden.Text <> "", lblPPHRateHidden.Text, "0") & "|" & _
                    IIf(lblPPNHidden.Text <> "", lblPPNHidden.Text, "2") & "|" & _
                    IIf(lblPPNAmtHidden.Text <> "", lblPPNAmtHidden.Text, "0") & "|" & _
                    IIf(lblPPHAmtHidden.Text <> "", lblPPHAmtHidden.Text, "0") & "|" & _
                    IIf(lblNetAmtHidden.Text <> "", lblNetAmtHidden.Text, "0")

        Try
            If rbVoid.Checked = False And rbAlloc.Checked = False And _
               ddlChargeLevel.SelectedIndex = 0 And RowPreBlk.Visible = True And _
               ((blnIsBalanceSheet = True And blnIsNurseryInd = True) Or (blnIsBalanceSheet = False And blnIsBlockRequire = True)) Then
                strParamList = Session("SS_LOCATION") & "|" & _
                               ddlAccount.SelectedItem.Value.Trim & "|" & _
                               ddlPreBlock.SelectedItem.Value.Trim & "|" & _
                               objGLSetup.EnumBlockStatus.Active & "|" & _
                               strAccMonth & "|" & strAccYear

                intErrNo = objBITrx.mtdUpdDebtorJournalLineByBlock(strOpCodeGLSubBlkByBlk, _
                                                                   strParamList, _
                                                                   strOpCodes, _
                                                                   strCompany, _
                                                                   strLocation, _
                                                                   strUserId, _
                                                                   strAccMonth, _
                                                                   strAccYear, _
                                                                   strParam, _
                                                                   objUpdStatus, _
                                                                   strLocType)
            Else
                intErrNo = objBITrx.mtdUpdDebtorJournalLine(strOpCodes, _
                                                            strCompany, _
                                                            strLocation, _
                                                            strUserId, _
                                                            strAccMonth, _
                                                            strAccYear, _
                                                            strParam, _
                                                            True, _
                                                            objUpdStatus)
            End If
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_JRNDET_ADD_LINE&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/BI_trx_JournalList.aspx")
        End Try


        If objUpdStatus = 2 Then
            lblErrExceed.Visible = True
        ElseIf objUpdStatus = 3 Then
            lblErrNoDocID.Visible = True
        ElseIf objUpdStatus = 4 Then
            lblErrDupDoc.Visible = True
        ElseIf objUpdStatus = 5 Then
            lblErrNoRec.Visible = True
        End If

        If objUpdStatus = 2 Or objUpdStatus = 3 Or objUpdStatus = 4 Or objUpdStatus = 5 Then
            BindInvoice(ddlInvoice.SelectedItem.Value)
            BindDebitNote(ddlDebitNote.SelectedItem.Value)
            BindCreditNote(ddlCreditNote.SelectedItem.Value)
            BindDebtorJournal(ddlCreditNote.SelectedItem.Value)
            BindReceipt(ddlReceipt.SelectedItem.Value)
            BindAccount(ddlAccount.SelectedItem.Value)
            BindPreBlock(ddlAccount.SelectedItem.Value, ddlPreBlock.SelectedItem.Value)
            BindBlock(ddlAccount.SelectedItem.Value, ddlBlock.SelectedItem.Value)
            BindInvPPNH(ddlInvoice.SelectedItem.Value)
            BindRecPPNH(ddlReceipt.SelectedItem.Value)
        End If

        onLoad_Display(strSelectedDebtorJrnID)
        onLoad_DisplayLine(strSelectedDebtorJrnID)
        onLoad_Button()
    End Sub

    Function CheckDocs() As Boolean
        Dim strInvoice As String = ddlInvoice.SelectedItem.Value.Trim
        Dim strDebitNote As String = ddlDebitNote.SelectedItem.Value.Trim
        Dim strCreditNote As String = ddlCreditNote.SelectedItem.Value.Trim
        Dim strDebtorJrn As String = ddlDebtorJrn.SelectedItem.Value.Trim
        Dim strReceipt As String = ddlReceipt.SelectedItem.Value.Trim
        Dim intInvoiceRcvInd As Integer
        Dim intDebitNoteInd As Integer
        Dim intCreditNoteInd As Integer
        Dim intDebtorJrnInd As Integer
        Dim intReceiptInd As Integer

        intInvoiceRcvInd = IIf(strInvoice = "", 0, 1)
        intDebitNoteInd = IIf(strDebitNote = "", 0, 1)
        intCreditNoteInd = IIf(strCreditNote = "", 0, 1)
        intDebtorJrnInd = IIf(strDebtorJrn = "", 0, 1)
        intReceiptInd = IIf(strReceipt = "", 0, 1)

        If (intInvoiceRcvInd + intDebitNoteInd + intCreditNoteInd + intDebtorJrnInd + intReceiptInd) = 0 Then
            lblErrNoSelectDoc.Visible = True
            onLoad_Button()
            CheckDocs = False
        ElseIf (intInvoiceRcvInd + intDebitNoteInd + intCreditNoteInd + intDebtorJrnInd + intReceiptInd) > 1 Then
            lblErrManySelectDoc.Visible = True
            BindInvoice(ddlInvoice.SelectedItem.Value)
            BindDebitNote(ddlDebitNote.SelectedItem.Value)
            BindCreditNote(ddlCreditNote.SelectedItem.Value)
            BindDebtorJournal(ddlCreditNote.SelectedItem.Value)
            BindReceipt(ddlReceipt.SelectedItem.Value)
            BindAccount(ddlAccount.SelectedItem.Value)
            BindPreBlock(ddlAccount.SelectedItem.Value, ddlPreBlock.SelectedItem.Value)
            BindBlock(ddlAccount.SelectedItem.Value, ddlBlock.SelectedItem.Value)
            BindInvPPNH(ddlInvoice.SelectedItem.Value)
            BindRecPPNH(ddlReceipt.SelectedItem.Value)
            onLoad_Button()
            CheckDocs = False
        Else
            CheckDocs = True
        End If

    End Function

    Sub SaveBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim objDebtorJrnID As String

        If (rbVoid.Checked = False) And (rbWriteOff.Checked = False) And (rbAdjust.Checked = False) And (rbAlloc.Checked = False) Then
            lblNoJrnType.Visible = True
            onLoad_Button()
            Exit Sub
        Else
            intJrnType = CInt(lblJrnType.Text)
        End If

        Update_DebtorJournal(objBITrx.EnumDebtorJournalStatus.Active, objDebtorJrnID)
        onLoad_Display(objDebtorJrnID)
        onLoad_DisplayLine(objDebtorJrnID)
        onLoad_Button()

    End Sub

    Sub RefreshBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        onLoad_Display(strSelectedDebtorJrnID)
        onLoad_DisplayLine(strSelectedDebtorJrnID)
        onLoad_Button()
    End Sub

    Sub ConfirmBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strDebtorJrnID = lblDebtorJrnID.Text.Trim
        Dim strParam As String
        Dim strOpCodes As String
        Dim strErrMsg As String
        strParam = strDebtorJrnID
        strOpCodes = "BI_CLSTRX_DEBTOR_JOURNAL_GET_FOR_CONFIRM" & "|" & _
                     "BI_CLSTRX_RECEIPT_GET_FOR_CONFIRM" & "|" & _
                     "BI_CLSTRX_INVOICE_UPD" & "|" & _
                     "BI_CLSTRX_DEBITNOTE_UPD" & "|" & _
                     "BI_CLSTRX_CREDITNOTE_UPD" & "|" & _
                     "BI_CLSTRX_RECEIPT_UPD" & "|" & _
                     "BI_CLSTRX_DEBTORJOURNAL_UPD"

        Try
            intErrNo = objBITrx.mtdDebtorJournal_Confirm(strOpCodes, _
                                                strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strAccMonth, _
                                                strAccYear, _
                                                strParam, _
                                                strErrMsg)
            If strErrMsg <> "" Then
                lblErrAction.Text = strErrMsg
                lblErrAction.Visible = True
            Else
                onLoad_Display(strDebtorJrnID)
                onLoad_DisplayLine(strDebtorJrnID)
                onLoad_Button()
            End If
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_JURNAL_DETAIL_CONFIRM&errmesg=" & Server.UrlEncode(lblErrMessage.Text) & "&redirect=BI/trx/BI_trx_JournalList.aspx")
        End Try
    End Sub

    Sub DeleteBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim objDebtorJrnID As String

        intJrnType = CInt(lblJrnType.Text)
        Select Case CInt(hidStatus.Value)
            Case objBITrx.EnumDebtorJournalStatus.Active
                Update_DebtorJournal(objBITrx.EnumDebtorJournalStatus.Deleted, objDebtorJrnID)
            Case objBITrx.EnumDebtorJournalStatus.Deleted
                Update_DebtorJournal(objBITrx.EnumDebtorJournalStatus.Active, objDebtorJrnID)
            Case Else
                Exit Sub
        End Select
        onLoad_Display(objDebtorJrnID)
        onLoad_DisplayLine(objDebtorJrnID)
        onLoad_Button()
    End Sub

    Sub BackBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("BI_trx_JournalList.aspx")
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
                        Session("SS_USERID") & "|" & Trim(lblDebtorJrnID.Text)

        Try
            intErrNo = objGLTrx.mtdGetDataCommon(strOpCode, _
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

        onLoad_Display(lblDebtorJrnID.Text)
        onLoad_DisplayLine(lblDebtorJrnID.Text)
        onLoad_Button()
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

    Function CheckDate(ByVal pv_strDate As String, ByRef pr_strDate As String) As Boolean
        Dim strDateFormatCode As String = Session("SS_DATEFMT")
        Dim strDateFormat As String

        pr_strDate = ""
        CheckDate = True
        If Not pv_strDate = "" Then
            If objGlobal.mtdValidInputDate(strDateFormatCode, pv_strDate, strDateFormat, pr_strDate) = False Then
                lblFmtDate.Text = strDateFormat
                pr_strDate = ""
                CheckDate = False
            End If
        End If
    End Function

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

