
Imports System
Imports System.Data
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic



Public Class ap_trx_CJDet : Inherits Page

    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblCJID As Label

    Protected WithEvents lblAccPeriod As Label
    Protected WithEvents rbVoid As RadioButton
    Protected WithEvents rbWriteOff As RadioButton
    Protected WithEvents rdAdjust As RadioButton
    Protected WithEvents rdAllocation As RadioButton
    Protected WithEvents lblStatus As Label
    Protected WithEvents lblDateCreated As Label
    Protected WithEvents lblLastUpdate As Label
    Protected WithEvents lblPrintDate As Label
    Protected WithEvents ddlAccCode As DropDownList
    Protected WithEvents lblUpdatedBy As Label
    Protected WithEvents ddlInvoiceRcv As DropDownList
    Protected WithEvents ddlDebitNote As DropDownList
    Protected WithEvents ddlCreditNote As DropDownList
    Protected WithEvents ddlPayment As DropDownList

    Protected WithEvents ddlBlock As DropDownList
    Protected WithEvents ddlVehCode As DropDownList
    Protected WithEvents ddlVehExpCode As DropDownList
    Protected WithEvents txtAmount As TextBox
    Protected WithEvents tblSelection As HtmlTable
    Protected WithEvents dgLineDet As DataGrid
    Protected WithEvents txtRemark As TextBox
    Protected WithEvents Addbtn As ImageButton
    Protected WithEvents SaveBtn As ImageButton
    Protected WithEvents RefreshBtn As ImageButton
    Protected WithEvents ConfirmBtn As ImageButton
    Protected WithEvents PrintBtn As ImageButton
    Protected WithEvents DeleteBtn As ImageButton
    Protected WithEvents UnDeleteBtn As ImageButton
    Protected WithEvents BackBtn As ImageButton
    Protected WithEvents cjid As HtmlInputHidden
    Protected WithEvents lblJrnType As Label
    Protected WithEvents lblVehicleOption As Label
    Protected WithEvents lblErrAccount As Label
    Protected WithEvents lblErrSupplier As Label
    Protected WithEvents lblErrBlock As Label
    Protected WithEvents lblErrVehicle As Label
    Protected WithEvents lblErrConfirmNotFulFil As Label
    Protected WithEvents lblErrConfirmNotFulFilText As Label
    Protected WithEvents lblErrReqAmount As Label
    Protected WithEvents lblErrNoSelectDoc As Label
    Protected WithEvents lblErrManySelectDoc As Label
    Protected WithEvents lblNoJrnType As Label
    Protected WithEvents lblStatusHidden As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents lblSelect As Label
    Protected WithEvents lblPleaseSelectOne As Label
    Protected WithEvents lblAccount As Label
    Protected WithEvents lblBlock As Label
    Protected WithEvents lblVehicle As Label
    Protected WithEvents lblErrVehicleExp As Label
    Protected WithEvents lblVehExpense As Label
    Protected WithEvents RowChargeLevel As HtmlTableRow
    Protected WithEvents RowPreBlk As HtmlTableRow
    Protected WithEvents RowBlk As HtmlTableRow
    Protected WithEvents lblPreBlockErr As Label
    Protected WithEvents lblPreBlkTag As Label
    Protected WithEvents ddlPreBlock As DropDownList
    Protected WithEvents ddlChargeLevel As DropDownList
    Protected WithEvents hidBlockCharge As HtmlInputHidden
    Protected WithEvents hidChargeLocCode As HtmlInputHidden
    Protected WithEvents lblID As Label
    Protected WithEvents lblInvoiceRcvTag As Label
    Protected WithEvents lblInvoiceRcvIDTag As Label
    Protected WithEvents lblPPHRateHidden As Label
    Protected WithEvents lblPPNHidden As Label
    Protected WithEvents lblPPNAmtHidden As Label
    Protected WithEvents lblPPHAmtHidden As Label
    Protected WithEvents lblNetAmtHidden As Label
    Protected WithEvents lblInvTypeHidden As Label
    Protected WithEvents lblCurrencyHidden As Label
    Protected WithEvents lblRateHidden As Label

    Protected WithEvents txtSupCode As TextBox
    Protected WithEvents txtSupName As TextBox
    Protected WithEvents txtAccCode As TextBox
    Protected WithEvents txtAccName As TextBox

    Protected WithEvents lblPPN As Label
    Protected WithEvents lblPPH As Label
    Protected WithEvents lblPercen As Label
    Protected WithEvents cbPPN As CheckBox
    Protected WithEvents txtPPHRate As TextBox
    Protected WithEvents btnFind1 As HtmlInputButton
    Dim PreBlockTag As String
    Dim BlockTag As String
    Protected ddlCreditorJournal As DropDownList
    Protected WithEvents ddlAccMonth As DropDownList
    Protected WithEvents ddlAccYear As DropDownList
    Protected WithEvents ibRefresh As ImageButton
    Protected WithEvents hidPeriodData As HtmlInputHidden

    Protected WithEvents TrLink As HtmlTableRow
    Protected WithEvents lbViewJournal As LinkButton
    Protected WithEvents dgViewJournal As DataGrid
    Protected WithEvents lblTotalDB As Label
    Protected WithEvents lblTotalCR As Label
    Protected WithEvents lblTotalViewJournal As Label
    Protected WithEvents txtCJRefDate As TextBox

    Protected WithEvents lblFmtTransDate As Label
    Protected WithEvents lblErrTransDate As Label

    Protected WithEvents ddlGoodsRcv As DropDownList

    Protected objAdmin As New agri.Admin.clsAccPeriod()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()

    Protected objAPTrx As New agri.AP.clsTrx()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objGLSetup As New agri.GL.clsSetup()
    Dim objPUSetup As New agri.PU.clsSetup()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim objGLtrx As New agri.GL.ClsTrx()

    Dim objSuppDs As New Object()
    Dim objInvRcvDs As New Object()
    Dim objDebitNoteDs As New Object()
    Dim objCreditNoteDs As New Object()
    Dim objPaymentDs As New Object()
    Dim objCJLnDs As New Object()
    Dim objAccDs As New Object()
    Dim objBlkDs As New Object()
    Dim objVehDs As New Object()
    Dim objVehExpDs As New Object()
    Dim objLangCapDs As New Object()
    Dim objCreditorJournalDs As New Object()
    Dim objInvPPNHDs As New Object()
    Dim objPayPPNHDs As New Object()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intAPAR As Integer
    Dim intConfig As Integer

    Dim strSelectedCJID As String
    Dim intCJStatus As Integer
    Dim intJrnType As Integer
    Dim strDocNotFulfil As String
    Dim strLocType As String
    Dim strAcceptDateFormat As String
    Dim strSelAccMonth As String
    Dim strSelAccYear As String
    Dim intLevel As Integer

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
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumAPAccessRights.APCreditorJournal), intAPAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            onload_GetLangCap()

            lblErrAccount.Visible = False
            lblPreBlockErr.Visible = False
            lblErrBlock.Visible = False
            lblErrVehicle.Visible = False
            lblErrVehicleExp.Visible = False

            'SaveBtn.Visible = False
            'RefreshBtn.Visible = False
            'ConfirmBtn.Visible = False
            'DeleteBtn.Visible = False
            'UnDeleteBtn.Visible = False
            lblErrSupplier.Visible = False
            lblErrAccount.Visible = False
            lblErrBlock.Visible = False
            lblErrVehicle.Visible = False
            lblErrConfirmNotFulFil.Visible = False
            lblErrReqAmount.Visible = False
            lblErrNoSelectDoc.Visible = False
            lblErrManySelectDoc.Visible = False
            lblNoJrnType.Visible = False
            lblErrTransDate.Visible = False
            lblFmtTransDate.Visible = False

            'to avoid double click, on aspx add this : UseSubmitBehavior="false"
            Addbtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(Addbtn).ToString())
            SaveBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(SaveBtn).ToString())
            RefreshBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(RefreshBtn).ToString())
            ConfirmBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(ConfirmBtn).ToString())
            'PrintBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(PrintBtn).ToString())
            DeleteBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(DeleteBtn).ToString())
            UnDeleteBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(UnDeleteBtn).ToString())
            BackBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(BackBtn).ToString())

            strSelectedCJID = Trim(IIf(Request.QueryString("cjid") = "", Request.Form("cjid"), Request.QueryString("cjid")))
            cjid.Value = strSelectedCJID

            txtSupCode.Attributes.Add("readonly", "readonly")
            txtSupName.Attributes.Add("readonly", "readonly")
            txtAccName.Attributes.Add("readonly", "readonly")

            If Not IsPostBack Then
                BindChargeLevelDropDownList()
                PopulateAccountingPeriodData()
                ShowAccountingPeriod(ddlAccMonth, ddlAccYear, strAccMonth, strAccYear)
                If strSelectedCJID <> "" Then
                    onLoad_Display(strSelectedCJID)
                    onLoad_DisplayLine(strSelectedCJID)
                    onLoad_Button()
                Else
                    txtCJRefDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
                    'BindSupplier("")
                    BindInvoiceRcv("")
                    BindDebitNote("")
                    BindCreditNote("")
                    BindCreditorJournal("")
                    BindPayment("")
                    'BindAccount("")
                    BindPreBlock("", "")
                    BindBlock("", "")
                    BindVehicle("", "")
                    BindVehicleExpense(True, "")
                    BindInvPPNH("")
                    BindPayPPNH("")
                    BindGoodsRcv("")
                    lblPPHRateHidden.Text = ""
                    lblPPNHidden.Text = ""
                    lblPPNAmtHidden.Text = ""
                    lblPPHAmtHidden.Text = ""
                    lblNetAmtHidden.Text = ""
                    lblInvTypeHidden.Text = ""

                    lblCurrencyHidden.Text = "IDR"
                    lblRateHidden.Text = "1"

                    onLoad_Button()
                    TrLink.Visible = True
                End If
            End If
        End If
    End Sub

    Sub ShowAccountingPeriod(ByRef ddlAccMonth As DropDownList, ByRef ddlAccYear As DropDownList, ByVal strAccMonth As String, ByVal strAccYear As String)
        Dim intCnt As Integer
        Dim intMonth As Integer = 12
        Dim arrPeriod() As String
        Dim arrMonthYear() As String
        Dim blnFound As Boolean
        
        blnFound = False
        For intCnt = 0 To ddlAccYear.Items.Count - 1
            If CInt(ddlAccYear.Items(intCnt).Value) = CInt(strAccYear) Then
                ddlAccYear.SelectedIndex = intCnt
                blnFound = True
                Exit For
            End If
        Next
        If blnFound = False Then
            ddlAccYear.Items.Add(New ListItem(CInt(strAccYear), CInt(strAccYear)))
            ddlAccYear.SelectedIndex = ddlAccYear.Items.Count - 1
        End If
        
        arrPeriod = Split(hidPeriodData.Value, ";")
        For intCnt = 0 To arrPeriod.GetUpperBound(0)
            arrMonthYear = Split(arrPeriod(intCnt), "/")
            If CInt(arrMonthYear(1)) = CInt(strAccYear) Then
                intMonth = CInt(arrMonthYear(0))
                Exit For
            End If
        Next
        
        ddlAccMonth.Items.Clear()
        For intCnt = 1 To intMonth
            ddlAccMonth.Items.Add(New ListItem(intCnt, intCnt))
        Next
        If CInt(strAccMonth) > intMonth Then
            ddlAccMonth.Items.Add(New ListItem(CInt(strAccMonth), CInt(strAccMonth)))
            ddlAccMonth.SelectedIndex = ddlAccMonth.Items.Count - 1
        Else
            ddlAccMonth.SelectedIndex = CInt(strAccMonth) - 1
        End If
    End Sub
    
    Sub PopulateAccountingPeriodData()
        Dim strOpCd_GET As String = "ADMIN_CLSACCPERIOD_CONFIG_GET"
        Dim dsPeriod As New DataSet
        Dim strParam As String
        Dim intCnt As Integer
        Dim intErrNo As Integer
        
        strParam = " ORDER BY HD.AccYear ASC ||"
        Try
           intErrNo = objAdmin.mtdGetAccPeriodCfg(strOpCd_GET, _
                                                  strCompany, _
                                                  strLocation, _
                                                  strUserId, _
                                                  strParam, _
                                                  dsPeriod)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADMIN_ACCPERIOD_CFG_GET&errmesg=" & Exp.ToString() & "&redirect=")
        End Try
        
        strParam = ""
        ddlAccYear.Items.Clear()
        For intCnt = 0 To dsPeriod.Tables(0).Rows.Count - 1
            strParam = strParam & ";" & dsPeriod.Tables(0).Rows(intCnt).Item("MaxPeriod").Trim() & "/" & dsPeriod.Tables(0).Rows(intCnt).Item("AccYear").Trim()
            ddlAccYear.Items.Add(New ListItem(CInt(dsPeriod.Tables(0).Rows(intCnt).Item("AccYear").Trim()), CInt(dsPeriod.Tables(0).Rows(intCnt).Item("AccYear").Trim())))
        Next
        If strParam <> "" Then
            strParam = Mid(strParam, 2)
        End If
        hidPeriodData.Value = strParam
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
        lblInvoiceRcvTag.Text = GetCaption(objLangCap.EnumLangCap.InvoiceReceive)
        lblInvoiceRcvIDTag.Text = GetCaption(objLangCap.EnumLangCap.InvoiceReceive) & lblID.Text
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

        lblErrAccount.Text = "<br>" & lblPleaseSelectOne.Text & lblAccount.Text
        lblErrBlock.Text = lblPleaseSelectOne.Text & lblBlock.Text
        lblErrVehicle.Text = lblPleaseSelectOne.Text & lblVehicle.Text
        lblErrVehicleExp.Text = lblPleaseSelectOne.Text & lblVehExpense.Text

        dgLineDet.Columns(2).HeaderText = lblAccount.Text
        dgLineDet.Columns(3).HeaderText = lblBlock.Text
        dgLineDet.Columns(4).HeaderText = lblVehicle.Text
        dgLineDet.Columns(5).HeaderText = lblVehExpense.Text
        PreBlockTag = GetCaption(objLangCap.EnumLangCap.Block)
        lblPreBlkTag.Text = PreBlockTag & lblCode.Text & " : "
        lblPreBlockErr.Text = lblPleaseSelectOne.Text & PreBlockTag & lblCode.Text
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_TRX_CJDET_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/AP_trx_CJDet.aspx")
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

        'ddlSupplier.Enabled = False
        txtRemark.Enabled = False
        tblSelection.Visible = False
        rbVoid.Enabled = False
        rbWriteOff.Enabled = False
        rdAdjust.Enabled = False
        rdAllocation.Enabled = False

        SaveBtn.Visible = False
        RefreshBtn.Visible = False
        ConfirmBtn.Visible = False
        DeleteBtn.Visible = False
        UnDeleteBtn.Visible = False
        txtCJRefDate.Enabled = False

        If (lblStatusHidden.Text <> "") Then
            intStatus = CInt(lblStatusHidden.Text)

            If rdAllocation.Checked = True Then
                PrintBtn.Visible = False
            End If


            Select Case intStatus
                Case objAPTrx.EnumCreditorJournalStatus.Active
                    txtCJRefDate.Enabled = True
                    txtRemark.Enabled = True
                    tblSelection.Visible = True
                    SaveBtn.Visible = True
                    RefreshBtn.Visible = True
                    ConfirmBtn.Visible = True
                    DeleteBtn.Visible = True
                    DeleteBtn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                Case objAPTrx.EnumCreditorJournalStatus.Deleted
                    UnDeleteBtn.Visible = True
                    PrintBtn.Visible = False
            End Select

        Else
            'ddlSupplier.Enabled = True
            txtRemark.Enabled = True
            tblSelection.Visible = True
            SaveBtn.Visible = True
            ConfirmBtn.Visible = True
            rbVoid.Enabled = True
            rbWriteOff.Enabled = True
            rdAdjust.Enabled = True
            rdAllocation.Enabled = True
            txtCJRefDate.Enabled = True
        End If
    End Sub


    Sub Switch_List()
        ddlAccMonth.Enabled = False
        ddlAccYear.Enabled = False
        ibRefresh.Visible = False
        ddlInvoiceRcv.Enabled = False
        ddlDebitNote.Enabled = False
        ddlCreditNote.Enabled = False
        ddlCreditorJournal.Enabled = False
        ddlPayment.Enabled = False

        btnFind1.Disabled = True
        txtAccCode.Enabled = False
        ddlChargeLevel.Enabled = False
        ddlPreBlock.Enabled = False
        ddlBlock.Enabled = False
        ddlVehCode.Enabled = False
        ddlVehExpCode.Enabled = False
        txtAmount.Enabled = False
        BindInvoiceRcv("")
        BindDebitNote("")
        BindCreditNote("")
        BindCreditorJournal("")
        BindPayment("")
        BindGoodsRcv("")
        lblPPHRateHidden.Text = ""
        lblPPNHidden.Text = ""
        lblPPNAmtHidden.Text = ""
        lblPPHAmtHidden.Text = ""
        lblNetAmtHidden.Text = ""
        lblInvTypeHidden.Text = ""
        lblCurrencyHidden.Text = "IDR"
        lblRateHidden.Text = "1"
        ddlGoodsRcv.Enabled = False

        If rbVoid.Checked Then
            intJrnType = objAPTrx.EnumCreditorJournalType.Void
            lblJrnType.Text = intJrnType
            ddlPayment.Enabled = True
            ddlAccMonth.Enabled = True
            ddlAccYear.Enabled = True
            ibRefresh.Visible = True
            lblPPN.Visible = True
            cbPPN.Enabled = False
            cbPPN.Visible = True
            lblPPH.Visible = True
            lblPercen.Visible = True
            txtPPHRate.Enabled = False
            txtPPHRate.Visible = True
        ElseIf rbWriteOff.Checked Then
            intJrnType = objAPTrx.EnumCreditorJournalType.Writeoff
            lblJrnType.Text = intJrnType
            ddlInvoiceRcv.Enabled = True
            ddlDebitNote.Enabled = True
            ddlCreditNote.Enabled = True
            ddlCreditorJournal.Enabled = True
            btnFind1.Disabled = False
            txtAccCode.Enabled = True
            ddlChargeLevel.Enabled = True
            ddlPreBlock.Enabled = True
            ddlBlock.Enabled = True
            ddlVehCode.Enabled = True
            ddlVehExpCode.Enabled = True
            ddlAccMonth.Enabled = True
            ddlAccYear.Enabled = True
            ibRefresh.Visible = True
            lblPPN.Visible = True
            cbPPN.Enabled = False
            cbPPN.Visible = True
            lblPPH.Visible = True
            lblPercen.Visible = True
            txtPPHRate.Enabled = False
            txtPPHRate.Visible = True
            txtAmount.Enabled = True
        ElseIf rdAdjust.Checked Then
            intJrnType = objAPTrx.EnumCreditorJournalType.Adjustment
            lblJrnType.Text = intJrnType
            btnFind1.Disabled = False
            txtAccCode.Enabled = True
            ddlChargeLevel.Enabled = True
            ddlPreBlock.Enabled = True
            ddlBlock.Enabled = True
            ddlVehCode.Enabled = True
            ddlVehExpCode.Enabled = True
            txtAmount.Enabled = True
            lblPPN.Visible = False
            cbPPN.Enabled = False
            cbPPN.Visible = False
            lblPPH.Visible = False
            lblPercen.Visible = False
            txtPPHRate.Enabled = False
            txtPPHRate.Visible = False
            ibRefresh.Visible = True
            ddlAccMonth.Enabled = True
            ddlAccYear.Enabled = True
            ddlGoodsRcv.Enabled = True
        ElseIf rdAllocation.Checked Then
            intJrnType = objAPTrx.EnumCreditorJournalType.Allocation
            lblJrnType.Text = intJrnType
            ddlInvoiceRcv.Enabled = True
            ddlDebitNote.Enabled = True
            ddlCreditNote.Enabled = True
            ddlPayment.Enabled = True
            ddlCreditorJournal.Enabled = True
            txtAmount.Enabled = True
            ddlAccMonth.Enabled = True
            ddlAccYear.Enabled = True
            ibRefresh.Visible = True
            lblPPN.Visible = True
            cbPPN.Enabled = False
            cbPPN.Visible = True
            lblPPH.Visible = True
            lblPercen.Visible = True
            txtPPHRate.Enabled = False
            txtPPHRate.Visible = True
        End If
    End Sub


    Sub onLoad_Display(ByVal pv_strCreditJrnId As String)
        Dim strOpCd_Get As String = "AP_CLSTRX_CREDITORJOURNAL_DETAILS_GET"
        Dim objCJDs As New Object()
        Dim intErrNo As Integer
        Dim strParam As String = pv_strCreditJrnId
        Dim intCnt As Integer = 0

        cjid.Value = pv_strCreditJrnId

        Try
            intErrNo = objAPTrx.mtdGetCreditorJournal(strOpCd_Get, _
                                                      strCompany, _
                                                      strLocation, _
                                                      strUserId, _
                                                      strAccMonth, _
                                                      strAccYear, _
                                                      strParam, _
                                                      objCJDs, _
                                                      True)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_TRX_CREDITJRNDET_GET_HEADER&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_cjlist.aspx")
        End Try

        lblCJID.Text = pv_strCreditJrnId
        lblAccPeriod.Text = Trim(objCJDs.Tables(0).Rows(0).Item("AccMonth")) & "/" & Trim(objCJDs.Tables(0).Rows(0).Item("AccYear"))
        intJrnType = CInt(Trim(objCJDs.Tables(0).Rows(0).Item("JrnType")))
        lblJrnType.Text = intJrnType
        lblStatus.Text = objAPTrx.mtdGetCreditorJournalStatus(Trim(objCJDs.Tables(0).Rows(0).Item("Status")))
        intCJStatus = CInt(Trim(objCJDs.Tables(0).Rows(0).Item("Status")))
        lblStatusHidden.Text = intCJStatus
        lblDateCreated.Text = objGlobal.GetLongDate(objCJDs.Tables(0).Rows(0).Item("CreateDate"))
        lblLastUpdate.Text = objGlobal.GetLongDate(objCJDs.Tables(0).Rows(0).Item("UpdateDate"))
        lblUpdatedBy.Text = Trim(objCJDs.Tables(0).Rows(0).Item("UserName"))
        txtRemark.Text = Trim(objCJDs.Tables(0).Rows(0).Item("Remark"))
        txtCJRefDate.Text = Date_Validation(objCJDs.Tables(0).Rows(0).Item("CreateDate"), True)

        Select Case intJrnType
            Case objAPTrx.EnumCreditorJournalType.Void
                rbVoid.Checked = True
                rbWriteOff.Checked = False
                rdAdjust.Checked = False
                rdAllocation.Checked = False
            Case objAPTrx.EnumCreditorJournalType.Writeoff
                rbVoid.Checked = False
                rbWriteOff.Checked = True
                rdAdjust.Checked = False
                rdAllocation.Checked = False
            Case objAPTrx.EnumCreditorJournalType.Adjustment
                rbVoid.Checked = False
                rbWriteOff.Checked = False
                rdAdjust.Checked = True
                rdAllocation.Checked = False
            Case objAPTrx.EnumCreditorJournalType.Allocation
                rbVoid.Checked = False
                rbWriteOff.Checked = False
                rdAdjust.Checked = False
                rdAllocation.Checked = True
        End Select

        txtSupCode.Text = Trim(objCJDs.Tables(0).Rows(0).Item("SupplierCode"))
        txtSupName.Text = Trim(objCJDs.Tables(0).Rows(0).Item("SupplierName"))
        'BindSupplier(Trim(objCJDs.Tables(0).Rows(0).Item("SupplierCode")))
        Switch_List()
        BindInvoiceRcv("")
        BindDebitNote("")
        BindCreditNote("")
        BindCreditorJournal("")
        BindPayment("")

        'BindAccount("")
        BindPreBlock("", "")
        BindBlock("", "")
        BindVehicle("", "")
        BindInvPPNH("")
        BindPayPPNH("")

        BindVehicleExpense(True, "")
        BindGoodsRcv("")

        lblPPHRateHidden.Text = ""
        lblPPNHidden.Text = ""
        lblPPNAmtHidden.Text = ""
        lblPPHAmtHidden.Text = ""
        lblNetAmtHidden.Text = ""
        lblInvTypeHidden.Text = ""
        lblCurrencyHidden.Text = "IDR"
        lblRateHidden.Text = "1"

    End Sub


    Sub onLoad_DisplayLine(ByVal pv_strCreditJrnId As String)
        Dim strOpCd_GetLine As String = "AP_CLSTRX_CREDITORJOURNAL_LINE_GET"
        Dim strParam As String = pv_strCreditJrnId
        Dim lbButton As LinkButton
        Dim lbl As Label
        Dim intErrNo As Integer
        Dim intCnt As Integer

        Try
            intErrNo = objAPTrx.mtdGetCreditorJournalLine(strOpCd_GetLine, strParam, objCJLnDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_TRX_CREDITJRNDET_GET_LINE&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_cjlist.aspx")
        End Try

        For intCnt = 0 To objCJLnDs.Tables(0).Rows.Count - 1
            objCJLnDs.Tables(0).Rows(intCnt).Item("CreditJrnLnID") = Trim(objCJLnDs.Tables(0).Rows(intCnt).Item("CreditJrnLnID"))
            objCJLnDs.Tables(0).Rows(intCnt).Item("DocID") = Trim(objCJLnDs.Tables(0).Rows(intCnt).Item("DocID"))
            objCJLnDs.Tables(0).Rows(intCnt).Item("DocType") = Trim(objCJLnDs.Tables(0).Rows(intCnt).Item("DocType"))
            objCJLnDs.Tables(0).Rows(intCnt).Item("NetAmount") = (objCJLnDs.Tables(0).Rows(intCnt).Item("NetAmount") + objCJLnDs.Tables(0).Rows(intCnt).Item("PPNAmount") + objCJLnDs.Tables(0).Rows(intCnt).Item("PPHAmount")) / objCJLnDs.Tables(0).Rows(intCnt).Item("ExchangeRate")
        Next intCnt

        dgLineDet.DataSource = objCJLnDs.Tables(0)
        dgLineDet.DataBind()

        For intCnt = 0 To objCJLnDs.Tables(0).Rows.Count - 1
            lbl = dgLineDet.Items.Item(intCnt).FindControl("lblEnumDocType")
            If Trim(lbl.Text) = objAPTrx.EnumPaymentDocType.InvoiceReceive Then
                lbl = dgLineDet.Items.Item(intCnt).FindControl("lblDocType")
                lbl.Text = lblInvoiceRcvTag.Text
            ElseIf Trim(lbl.Text) = objAPTrx.EnumPaymentDocType.AdvPayment Then
                lbl = dgLineDet.Items.Item(intCnt).FindControl("lblDocType")
                lbl.Text = ""
            End If
            Select Case intCJStatus
                Case objAPTrx.EnumCreditorJournalStatus.Active
                    lbButton = dgLineDet.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = True
                    lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                Case Else
                    lbButton = dgLineDet.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False
            End Select
        Next

        If objCJLnDs.Tables(0).Rows.Count > 0 Then
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
    '        Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_TRX_CREDITJRNDET_GET_SUPP1&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_cjlist.aspx")
    '    End Try

    '    For intCnt = 0 To objSuppDs.Tables(0).Rows.Count - 1
    '        objSuppDs.Tables(0).Rows(intCnt).Item("SupplierCode") = Trim(objSuppDs.Tables(0).Rows(intCnt).Item("SupplierCode"))
    '        objSuppDs.Tables(0).Rows(intCnt).Item("Name") = Trim(objSuppDs.Tables(0).Rows(intCnt).Item("SupplierCode")) & " (" & Trim(objSuppDs.Tables(0).Rows(intCnt).Item("Name")) & ")"
    '        If objSuppDs.Tables(0).Rows(intCnt).Item("SupplierCode") = pv_strSupplierId Then
    '            intSelectedSuppIndex = intCnt + 1
    '        End If
    '    Next intCnt

    '    dr = objSuppDS.Tables(0).NewRow()
    '    dr("SupplierCode") = ""
    '    dr("Name") = "Select Supplier Code"
    '    ObjSuppDS.Tables(0).Rows.InsertAt(dr, 0)

    '    ddlSupplier.DataSource = objSuppDs.Tables(0)
    '    ddlSupplier.DataValueField = "SupplierCode"
    '    ddlSupplier.DataTextField = "Name"
    '    ddlSupplier.DataBind()
    '    ddlSupplier.SelectedIndex = intSelectedSuppIndex
    '    ddlSupplier.AutoPostBack = True
    'End Sub



    Sub BindInvoiceRcv(ByVal pv_strInvoiceRcvId As String)
        Dim strOpCode As String = "AP_CLSTRX_PAYMENT_INVOICERCV_BYSUPP_GET"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim dr As DataRow
        Dim intSelectedIRIndex As Integer = 0
        Dim strAccMonth As String = ddlAccMonth.SelectedItem.Value
        Dim strAccYear As String = ddlAccYear.SelectedItem.Value
        strOpCode = "AP_CLSTRX_PAYMENT_INVOICERCV_BYSUPP_ACCPERIOD_GET"
        strParam = txtSupCode.Text & "|" & objAPTrx.EnumInvoiceRcvStatus.Confirmed & "','" & objAPTrx.EnumInvoiceRcvStatus.Closed & "','" & objAPTrx.EnumInvoiceRcvStatus.Paid

        Try
            intErrNo = objAPTrx.mtdGetPayment_Document(strOpCode, _
                                                       strCompany, _
                                                       strLocation, _
                                                       strUserId, _
                                                       strAccMonth, _
                                                       strAccYear, _
                                                       strParam, _
                                                       objAPTrx.EnumPaymentDocType.InvoiceReceive, _
                                                       objInvRcvDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_TRX_CREDITJRNDET_GET_INVOICERCV&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_cjlist.aspx")
        End Try

        For intCnt = 0 To objInvRcvDs.Tables(0).Rows.Count - 1
            objInvRcvDs.Tables(0).Rows(intCnt).Item("InvoiceRcvID") = Trim(objInvRcvDs.Tables(0).Rows(intCnt).Item("InvoiceRcvID"))
            objInvRcvDs.Tables(0).Rows(intCnt).Item("DispInvoiceRcvID") = objInvRcvDs.Tables(0).Rows(intCnt).Item("InvoiceRcvID") & ", " & objInvRcvDs.Tables(0).Rows(intCnt).Item("CurrencyCode") & " " & objInvRcvDs.Tables(0).Rows(intCnt).Item("OutstandingAmountToDisplay") & ", " & objInvRcvDs.Tables(0).Rows(intCnt).Item("Kurs")
            'objInvRcvDs.Tables(0).Rows(intCnt).Item("DispInvoiceRcvID") = Trim(objInvRcvDs.Tables(0).Rows(intCnt).Item("InvoiceRcvID")) & ", Rp " & objInvRcvDs.Tables(0).Rows(intCnt).Item("TotalAmount")
            If pv_strInvoiceRcvId = objInvRcvDs.Tables(0).Rows(intCnt).Item("InvoiceRcvID") Then
                intSelectedIRIndex = intCnt + 1
            End If
        Next intCnt

        dr = objInvRcvDs.Tables(0).NewRow()
        dr("InvoiceRcvId") = ""
        dr("DispInvoiceRcvID") = lblSelect.Text & lblInvoiceRcvTag.Text
        objInvRcvDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlInvoiceRcv.DataSource = objInvRcvDs.Tables(0)
        ddlInvoiceRcv.DataValueField = "InvoiceRcvID"
        ddlInvoiceRcv.DataTextField = "DispInvoiceRcvID"
        ddlInvoiceRcv.DataBind()
        ddlInvoiceRcv.SelectedIndex = intSelectedIRIndex
    End Sub


    Sub BindDebitNote(ByVal pv_strDebitNoteId As String)
        Dim strOpCode As String = "AP_CLSTRX_PAYMENT_DEBITNOTE_BYSUPP_GET"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedDNIndex As Integer = 0
        Dim dr As DataRow
        Dim strAccMonth As String = ddlAccMonth.SelectedItem.Value
        Dim strAccYear As String = ddlAccYear.SelectedItem.Value
        strOpCode = "AP_CLSTRX_PAYMENT_DEBITNOTE_BYSUPP_ACCPERIOD_GET"
        strParam = txtSupCode.Text & "|" & objAPTrx.EnumDebitNoteStatus.Confirmed & "','" & objAPTrx.EnumDebitNoteStatus.Closed & "','" & objAPTrx.EnumDebitNoteStatus.Paid

        Try
            intErrNo = objAPTrx.mtdGetPayment_Document(strOpCode, _
                                                       strCompany, _
                                                       strLocation, _
                                                       strUserId, _
                                                       strAccMonth, _
                                                       strAccYear, _
                                                       strParam, _
                                                       objAPTrx.EnumPaymentDocType.DebitNote, _
                                                       objDebitNoteDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_TRX_CREDITJRNDET_GET_DEBITNOTE&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_cjlist.aspx")
        End Try

        For intCnt = 0 To objDebitNoteDs.Tables(0).Rows.Count - 1
            objDebitNoteDs.Tables(0).Rows(intCnt).Item("DebitNoteId") = Trim(objDebitNoteDs.Tables(0).Rows(intCnt).Item("DebitNoteId"))
            objDebitNoteDs.Tables(0).Rows(intCnt).Item("DispDebitNoteId") = Trim(objDebitNoteDs.Tables(0).Rows(intCnt).Item("DebitNoteId")) & ", Rp " & objDebitNoteDs.Tables(0).Rows(intCnt).Item("TotalAmount")
            If pv_strDebitNoteId = objDebitNoteDs.Tables(0).Rows(intCnt).Item("DebitNoteId") Then
                intSelectedDNIndex = intCnt + 1
            End If
        Next intCnt

        dr = objDebitNoteDs.Tables(0).NewRow()
        dr("DebitNoteID") = ""
        dr("DispDebitNoteId") = "Select Debit Note"
        objDebitNoteDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlDebitNote.DataSource = objDebitNoteDs.Tables(0)
        ddlDebitNote.DataValueField = "DebitNoteID"
        ddlDebitNote.DataTextField = "DispDebitNoteId"
        ddlDebitNote.DataBind()
        ddlDebitNote.SelectedIndex = intSelectedDNIndex
    End Sub

    Sub BindCreditNote(ByVal pv_strCreditNoteId As String)
        Dim strOpCode As String = "AP_CLSTRX_PAYMENT_CREDITNOTE_BYSUPP_GET"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedCNIndex As Integer = 0
        Dim dr As DataRow
        Dim strAccMonth As String = ddlAccMonth.SelectedItem.Value
        Dim strAccYear As String = ddlAccYear.SelectedItem.Value
        strOpCode = "AP_CLSTRX_PAYMENT_CREDITNOTE_BYSUPP_ACCPERIOD_GET"
        strParam = txtSupCode.Text & "|" & objAPTrx.EnumCreditNoteStatus.Confirmed & "','" & objAPTrx.EnumCreditNoteStatus.Closed & "','" & objAPTrx.EnumCreditNoteStatus.Paid

        Try
            intErrNo = objAPTrx.mtdGetPayment_Document(strOpCode, _
                                                       strCompany, _
                                                       strLocation, _
                                                       strUserId, _
                                                       strAccMonth, _
                                                       strAccYear, _
                                                       strParam, _
                                                       objAPTrx.EnumPaymentDocType.CreditNote, _
                                                       objCreditNoteDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_TRX_CREDITJRNDET_GET_CREDITNOTE&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_cjlist.aspx")
        End Try

        For intCnt = 0 To objCreditNoteDs.Tables(0).Rows.Count - 1
            objCreditNoteDs.Tables(0).Rows(intCnt).Item("CreditNoteId") = Trim(objCreditNoteDs.Tables(0).Rows(intCnt).Item("CreditNoteId"))
            objCreditNoteDs.Tables(0).Rows(intCnt).Item("DispCreditNoteId") = Trim(objCreditNoteDs.Tables(0).Rows(intCnt).Item("CreditNoteId")) & ", Rp " & objCreditNoteDs.Tables(0).Rows(intCnt).Item("TotalAmount")
            If pv_strCreditNoteId = objCreditNoteDs.Tables(0).Rows(intCnt).Item("CreditNoteId") Then
                intSelectedCNIndex = intCnt + 1
            End If
        Next intCnt

        dr = objCreditNoteDs.Tables(0).NewRow()
        dr("CreditNoteId") = ""
        dr("DispCreditNoteId") = "Select Credit Note"
        objCreditNoteDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlCreditNote.DataSource = objCreditNoteDs.Tables(0)
        ddlCreditNote.DataValueField = "CreditNoteId"
        ddlCreditNote.DataTextField = "DispCreditNoteId"
        ddlCreditNote.DataBind()
        ddlCreditNote.SelectedIndex = intSelectedCNIndex
    End Sub

    Private Sub BindCreditorJournal(ByVal pv_strCreditorJournalId As String, _
                                    Optional ByVal strRadBtn As String = "")
        Dim strOpCode As String = "AP_CLSTRX_PAYMENT_CREDITORJOURNAL_BYSUPP_GET"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer = 0
        Dim dr As DataRow
        Dim strAccMonth As String = ddlAccMonth.SelectedItem.Value
        Dim strAccYear As String = ddlAccYear.SelectedItem.Value
        strOpCode = "AP_CLSTRX_PAYMENT_CREDITORJOURNAL_BYSUPP_ACCPERIOD_GET"
        If strRadBtn = "" Then
            strParam = txtSupCode.Text & "|" & CStr(objAPTrx.EnumCreditorJournalStatus.Confirmed) & "','" & _
                       CStr(objAPTrx.EnumCreditorJournalStatus.Closed) & "|" & _
                       CStr(objAPTrx.EnumCreditorJournalType.Adjustment) & "|" & _
                       "AND OutstandingAmount <> 0"
        Else
            strParam = txtSupCode.Text & "|" & CStr(objAPTrx.EnumCreditorJournalStatus.Confirmed) & "','" & _
                   CStr(objAPTrx.EnumCreditorJournalStatus.Closed) & "|" & _
                   CStr(objAPTrx.EnumCreditorJournalType.Adjustment) & "|"
        End If

        Try
            intErrNo = objAPTrx.mtdGetPayment_Document(strOpCode, strCompany, strLocation, strUserId, strAccMonth, strAccYear, _
                                                       strParam, objAPTrx.EnumPaymentDocType.CreditorJournal, objCreditorJournalDs)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_TRX_PAYMENT_GET_CREDITORJOURNAL&errmesg=" & Exp.ToString() & "&redirect=ap/trx/ap_trx_paylist.aspx")
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
        dr("DispCreditJrnID") = "Select Creditor Journal"
        objCreditorJournalDs.Tables(0).Rows.InsertAt(dr, 0)

        With ddlCreditorJournal
            .DataSource = objCreditorJournalDs.Tables(0)
            .DataValueField = "CreditJrnID"
            .DataTextField = "DispCreditJrnID"
            .DataBind()
            .SelectedIndex = intSelectedIndex
        End With
    End Sub

    Sub BindPayment(ByVal pv_strPaymentID As String)
        Dim strOpCode As String = "AP_CLSTRX_CREDITORJOURNAL_PAYMENT_BYSUPP_GET"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedPayIndex As Integer = 0
        Dim dr As DataRow

        Dim strAccMonth As String = ddlAccMonth.SelectedItem.Value
        Dim strAccYear As String = ddlAccYear.SelectedItem.Value
        strParam = txtSupCode.Text & "|" & objAPTrx.EnumPaymentStatus.Confirmed & "','" & objAPTrx.EnumPaymentStatus.Closed

        Try
            intErrNo = objAPTrx.mtdGetPayment_Document(strOpCode, _
                                                       strCompany, _
                                                       strLocation, _
                                                       strUserId, _
                                                       strAccMonth, _
                                                       strAccYear, _
                                                       strParam, _
                                                       objAPTrx.EnumPaymentDocType.Payment, _
                                                       objPaymentDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_TRX_CREDITJRNDET_GET_PAYMENT&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_cjlist.aspx")
        End Try

        For intCnt = 0 To objPaymentDs.Tables(0).Rows.Count - 1
            objPaymentDs.Tables(0).Rows(intCnt).Item("PaymentID") = Trim(objPaymentDs.Tables(0).Rows(intCnt).Item("PaymentID"))
            'objPaymentDs.Tables(0).Rows(intCnt).Item("DispPaymentId") = Trim(objPaymentDs.Tables(0).Rows(intCnt).Item("PaymentID")) & ", Rp " & objPaymentDs.Tables(0).Rows(intCnt).Item("TotalAmount")
            objPaymentDs.Tables(0).Rows(intCnt).Item("DispPaymentId") = objPaymentDs.Tables(0).Rows(intCnt).Item("PaymentID") & ", " & objPaymentDs.Tables(0).Rows(intCnt).Item("CurrencyCode") & " " & objPaymentDs.Tables(0).Rows(intCnt).Item("OutstandingAmountToDisplay") & ", " & objPaymentDs.Tables(0).Rows(intCnt).Item("Kurs")
            If pv_strPaymentID = objPaymentDs.Tables(0).Rows(intCnt).Item("PaymentID") Then
                intSelectedPayIndex = intCnt + 1
            End If
        Next intCnt

        dr = objPaymentDs.Tables(0).NewRow()
        dr("PaymentId") = ""
        dr("DispPaymentID") = "Select Payment ID"
        objPaymentDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlPayment.DataSource = objPaymentDs.Tables(0)
        ddlPayment.DataValueField = "PaymentID"
        ddlPayment.DataTextField = "DispPaymentID"
        ddlPayment.DataBind()
        ddlPayment.SelectedIndex = intSelectedPayIndex
    End Sub

    'Sub BindAccount(ByVal pv_strAccCode As String)
    '    Dim strOpCd As String = "GL_CLSSETUP_ACCOUNTCODE_LIST_GET"
    '    Dim dr As DataRow
    '    Dim strParam As String = "Order By ACC.AccCode|And ACC.Status = '" & objGLSetup.EnumAccountCodeStatus.Active & "'"
    '    Dim intErrNo As Integer
    '    Dim intCnt As Integer
    '    Dim intSelectedIndex As Integer = 0

    '    strParam = strParam & IIf(Session("SS_COACENTRALIZED") = "1", "", " AND LocCode = '" & Trim(strLocation) & "' ")

    '    Try
    '        intErrNo = objGLSetup.mtdGetMasterList(strOpCd, _
    '                                               strParam, _
    '                                               objGLSetup.EnumGLMasterType.AccountCode, _
    '                                               objAccDs)
    '    Catch Exp As System.Exception
    '        Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_TRX_CREDITJRNDET_ACCCODE_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
    '    End Try

    '    For intCnt = 0 To objAccDs.Tables(0).Rows.Count - 1
    '        objAccDs.Tables(0).Rows(intCnt).Item("AccCode") = Trim(objAccDs.Tables(0).Rows(intCnt).Item("AccCode"))
    '        objAccDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objAccDs.Tables(0).Rows(intCnt).Item("AccCode")) & " (" & Trim(objAccDs.Tables(0).Rows(intCnt).Item("Description")) & ")"
    '        If objAccDs.Tables(0).Rows(intCnt).Item("AccCode") = Trim(pv_strAccCode) Then
    '            intSelectedIndex = intCnt + 1
    '        End If
    '    Next

    '    dr = objAccDs.Tables(0).NewRow()
    '    dr("AccCode") = ""
    '    dr("Description") = lblSelect.Text & lblAccount.Text
    '    objAccDs.Tables(0).Rows.InsertAt(dr, 0)

    '    ddlAccount.DataSource = objAccDs.Tables(0)
    '    ddlAccount.DataValueField = "AccCode"
    '    ddlAccount.DataTextField = "Description"
    '    ddlAccount.DataBind()
    '    ddlAccount.SelectedIndex = intSelectedIndex
    '    ddlAccount.AutoPostBack = True
    'End Sub

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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_TRX_CREDITJRNDET_GET_ACCOUNT_DETAILS&errmesg=" & lblErrMessage.Text & "&redirect=")
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

    Sub onSelect_PayPPNH(ByVal sender As System.Object, ByVal e As System.EventArgs)
        BindPayPPNH(ddlPayment.SelectedItem.Value)
    End Sub

    Sub onSelect_InvPPNH(ByVal sender As System.Object, ByVal e As System.EventArgs)
        BindInvPPNH(ddlInvoiceRcv.SelectedItem.Value)
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
    End Sub

    Sub BindInvPPNH(ByVal pv_strSelectedInvID As String)
        Dim strOpCd_GetInvPPNH As String = "AP_CLSTRX_INVOICERECEIVE_PPNH_GET"
        Dim strOpCodes As String = strOpCd_GetInvPPNH
        Dim strParam As String = pv_strSelectedInvID
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim dblPPNAmount As Double
        Dim dblPPHAmount As Double
        Dim dblNetAmount As Double
        Dim dblTotalPPNAmount As Double
        Dim dblTotalPPHAmount As Double
        Dim dblTotalNetAmount As Double
        Dim dblRate As Double

        Dim lblLinePPNAmt As Label
        Dim lblLinePPHAmt As Label
        Dim lblLineNetAmt As Label

        Dim dbIRAmount As Double
        Dim dbTotalIRAmount As Double
        Dim dbCBAmount As Double
        Dim dbTotalCBAmount As Double
        Dim dbOutstandingAmount As Double
        Dim dbTotalOutstandingAmount As Double

        'tambah currency dan rate
        If Trim(pv_strSelectedInvID) = "" Then
            Exit Sub
        End If

        Try
            intErrNo = objAPTrx.mtdGetInvRcvPPNH(strOpCodes, _
                                                 strCompany, _
                                                 strLocation, _
                                                 strUserId, _
                                                 strAccMonth, _
                                                 strAccYear, _
                                                 strParam, _
                                                 objInvPPNHDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_GET_LINE&errmesg=" & Exp.ToString() & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
        End Try

        For intCnt = 0 To objInvPPNHDs.Tables(0).Rows.Count - 1
            dblPPNAmount = CDbl(objInvPPNHDs.Tables(0).Rows(intCnt).Item("InvPPNAmt"))
            dblTotalPPNAmount += dblPPNAmount
            dblPPHAmount = CDbl(objInvPPNHDs.Tables(0).Rows(intCnt).Item("InvPPHAmt"))
            dblTotalPPHAmount += dblPPHAmount
            dblNetAmount = CDbl(objInvPPNHDs.Tables(0).Rows(intCnt).Item("InvNetAmt"))
            dblTotalNetAmount += dblNetAmount
            If Trim(objInvPPNHDs.Tables(0).Rows(intCnt).Item("PPHRate")) <> "0" Then
                lblPPHRateHidden.Text = objInvPPNHDs.Tables(0).Rows(intCnt).Item("PPHRate")
            End If
            If Trim(objInvPPNHDs.Tables(0).Rows(intCnt).Item("PPNCheck")) <> "2" Then
                lblPPNHidden.Text = objInvPPNHDs.Tables(0).Rows(intCnt).Item("PPNCheck")
            End If
            lblInvTypeHidden.Text = objInvPPNHDs.Tables(0).Rows(intCnt).Item("InvoiceType")
            txtPPHRate.Text = Trim(objInvPPNHDs.Tables(0).Rows(0).Item("PPHRate"))
            cbPPN.Checked = IIf(objInvPPNHDs.Tables(0).Rows(0).Item("PPNCheck") = objAPTrx.EnumPPN.Yes, True, False)

            lblCurrencyHidden.Text = objInvPPNHDs.Tables(0).Rows(intCnt).Item("CurrencyCode")
            lblRateHidden.Text = objInvPPNHDs.Tables(0).Rows(intCnt).Item("ExchangeRate")

            dbIRAmount = objInvPPNHDs.Tables(0).Rows(intCnt).Item("IRAmount")
            dbTotalIRAmount += dbIRAmount
            dbCBAmount = objInvPPNHDs.Tables(0).Rows(intCnt).Item("CBAmount")
            dbTotalCBAmount += dbCBAmount
            dbOutstandingAmount = objInvPPNHDs.Tables(0).Rows(intCnt).Item("OutstandingAmount")
            dbTotalOutstandingAmount += dbOutstandingAmount
        Next

        If dbTotalIRAmount - dbTotalCBAmount <> 0 Then
            lblPPNAmtHidden.Text = dblTotalPPNAmount
            lblPPHAmtHidden.Text = dblTotalPPHAmount
            lblNetAmtHidden.Text = dblTotalNetAmount
        Else
            lblPPNAmtHidden.Text = 0
            lblPPHAmtHidden.Text = 0
            lblNetAmtHidden.Text = 0
        End If

        txtAmount.Text = dbTotalOutstandingAmount

    End Sub

    Sub BindPayPPNH(ByVal pv_strSelectedPayID As String)
        Dim strOpCd_GetPayPPNH As String = "AP_CLSTRX_PAYMENT_LINE_GET"
        Dim strParam As String = pv_strSelectedPayID
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

        If Trim(pv_strSelectedPayID) = "" Then
            Exit Sub
        End If

        Try
            intErrNo = objAPTrx.mtdGetPaymentLine(strOpCd_GetPayPPNH, strParam, objPayPPNHDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_TRX_PAYMENT_GET_LINE&errmesg=" & Exp.ToString() & "&redirect=ap/trx/ap_trx_paylist.aspx")
        End Try

        For intCnt = 0 To objPayPPNHDs.Tables(0).Rows.Count - 1
            dblPPNAmount = CDbl(objPayPPNHDs.Tables(0).Rows(intCnt).Item("PPNAmount"))
            dblTotalPPNAmount += dblPPNAmount
            dblPPHAmount = CDbl(objPayPPNHDs.Tables(0).Rows(intCnt).Item("PPHAmount"))
            dblTotalPPHAmount += dblPPHAmount
            dblNetAmount = CDbl(objPayPPNHDs.Tables(0).Rows(intCnt).Item("NetAmount"))
            dblTotalNetAmount += dblNetAmount
            If Trim(objPayPPNHDs.Tables(0).Rows(intCnt).Item("PPHRate")) <> "0" Then
                lblPPHRateHidden.Text = objPayPPNHDs.Tables(0).Rows(intCnt).Item("PPHRate")
            End If
            If Trim(objPayPPNHDs.Tables(0).Rows(intCnt).Item("PPN")) <> "2" Then
                lblPPNHidden.Text = objPayPPNHDs.Tables(0).Rows(intCnt).Item("PPN")
            End If
            lblInvTypeHidden.Text = objPayPPNHDs.Tables(0).Rows(intCnt).Item("InvoiceType")
            txtPPHRate.Text = Trim(objPayPPNHDs.Tables(0).Rows(0).Item("PPHRate"))
            cbPPN.Checked = IIf(objPayPPNHDs.Tables(0).Rows(0).Item("PPN") = objAPTrx.EnumPPN.Yes, True, False)

            lblCurrencyHidden.Text = objPayPPNHDs.Tables(0).Rows(intCnt).Item("CurrencyCode")
            lblRateHidden.Text = objPayPPNHDs.Tables(0).Rows(intCnt).Item("ExchangeRate")

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

        If objBlkDs.Tables(0).Rows.Count = 1 Then
            intSelectedIndex = 1
        End If

        dr = objBlkDs.Tables(0).NewRow()
        dr("BlkCode") = ""
        dr("Description") = lblPleaseSelectOne.Text & PreBlockTag & lblCode.Text

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
        dr("Description") = lblSelect.Text & lblBlock.Text
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
        dr("Description") = lblSelect.Text & lblVehicle.Text
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
        dr("Description") = lblSelect.Text & lblVehExpense.Text
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
        BindInvoiceRcv("")
        BindDebitNote("")
        BindCreditNote("")
        BindCreditorJournal("")
        BindPayment("")
        BindGoodsRcv("")
        onLoad_Button()
    End Sub

    Sub onCheck_Change(ByVal Sender As Object, ByVal E As EventArgs)
        Switch_List()
        onLoad_Button()
    End Sub

    Sub Update_CreditorJournal(ByVal pv_intStatus As Integer, ByRef pr_objNewCJID As Object, ByRef pr_objFailFulfil As Object)
        Dim strSupplierCode As String = txtSupCode.Text
        Dim strRemark As String = txtRemark.Text
        Dim intErrNo As Integer
        Dim strParam As String = ""
        'Dim strDate As String = Date_Validation(txtTransDate.Text, False)
        Dim strOpCd_GetLines As String = "AP_CLSTRX_CREDITORJOURNAL_LINE_GET"
        Dim strOpCd_AddPay As String = "AP_CLSTRX_CREDITORJOURNAL_ADD"
        Dim strOpCd_UpdPay As String = "AP_CLSTRX_CREDITORJOURNAL_UPD"
        Dim strOpCodes As String = strOpCd_GetLines & "|" & _
                                   strOpCd_AddPay & "|" & _
                                   strOpCd_UpdPay
        Dim strNewIDFormat As String
        Dim strDate As String = Date_Validation(txtCJRefDate.Text, False)
        Dim indDate As String = ""

        If CheckDate(txtCJRefDate.Text.Trim(), indDate) = False Then
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

        strAccYear = Year(strDate)
        strAccMonth = Month(strDate)

        strNewIDFormat = "SCJ" & "/" & strCompany & "/" & strLocation & "/" & IIf(Len(Trim(strAccMonth)) = 1, "0" & strAccMonth, strAccMonth) & "/" & strAccYear & "/"

        Try
            strParam = strParam & intJrnType & "|" & _
                                objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.APCreditJrn) & "|" & _
                                strSelectedCJID & "|" & _
                                strSupplierCode & "|" & _
                                strRemark & "|" & _
                                pv_intStatus & "|" & _
                                strDate & "|" & _
                                strNewIDFormat

            intErrNo = objAPTrx.mtdUpdCreditorJournal(strOpCodes, strCompany, strLocation, _
                                                      strUserId, strAccMonth, strAccYear, _
                                                      strParam, pr_objNewCJID, pr_objFailFulfil)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_TRX_CREDITJRNDET_UPD_DATA&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_cjlist.aspx")
        End Try

        If pr_objFailFulfil <> "" Then
            lblErrConfirmNotFulFil.Text = lblErrConfirmNotFulFilText.Text & pr_objFailFulfil & "."
            lblErrConfirmNotFulFil.Visible = True
        End If

        pr_objNewCJID = IIf(strSelectedCJID = "", pr_objNewCJID, strSelectedCJID)
    End Sub

    Sub GetSupplierBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        BindInvoiceRcv("")
        BindDebitNote("")
        BindCreditNote("")
        BindCreditorJournal("")
        BindPayment("")
        BindGoodsRcv("")
        onLoad_Button()
    End Sub

    Sub onSelect_StrAccCode(ByVal sender As System.Object, ByVal e As System.EventArgs)
        GetCOADetail(txtAccCode.Text.Trim)
        onSelect_Account(sender, e)
    End Sub


    Sub AddBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCodeGLSubBlkByBlk As String = "GL_CLSSETUP_SUBBLOCK_BY_BLOCK_GET"
        Dim strParamList As String
        Dim objUpdStatus As Integer
        Dim blnIsValid As Boolean
        Dim objCJID As String
        Dim strInvoiceRcv As String = ddlInvoiceRcv.SelectedItem.Value
        Dim strDebitNote As String = ddlDebitNote.SelectedItem.Value
        Dim strCreditNote As String = ddlCreditNote.SelectedItem.Value
        Dim strSelCreditJrn As String = ddlCreditorJournal.SelectedItem.Value
        Dim strPayment As String = ddlPayment.SelectedItem.Value
        Dim strAccCode As String = txtAccCode.Text
        Dim strBlkCode As String
        Dim strVehCode As String = ddlVehCode.SelectedItem.Value
        Dim strVehExpenseCode As String = ddlVehExpCode.SelectedItem.Value
        Dim strGoodsRcv As String = ddlGoodsRcv.SelectedItem.Value
        Dim intInvoiceRcvInd As Integer
        Dim intDebitNoteInd As Integer
        Dim intCreditNoteInd As Integer
        Dim intCreditorJournal As Integer
        Dim intPaymentInd As Integer
        Dim intGoodsRcvInd As Integer
        Dim dblAmount As Double
        Dim dblPPNAmount As Double
        Dim dblPPHAmount As Double
        Dim dblNetAmount As Double
        Dim strOpCode_AddLine As String = "AP_CLSTRX_CREDITORJOURNAL_LINE_ADD"
        Dim strOpCode_GetSumCJLine As String = "AP_CLSTRX_CREDITORJOURNAL_SUM_LINE_GET"
        Dim strOpCode_UpdStatus As String = "AP_CLSTRX_CREDITORJOURNAL_STATUS_UPD"
        Dim strOpCd_GetIRTOTAL As String = "AP_CLSTRX_PAYMENT_INVOICERECEIVE_AMOUNT_GET"
        Dim strOpCd_GetDNTOTAL As String = "AP_CLSTRX_PAYMENT_DEBITNOTE_AMOUNT_GET"
        Dim strOpCd_GetCNTOTAL As String = "AP_CLSTRX_PAYMENT_CREDITNOTE_AMOUNT_GET"
        Dim strOpCd_GetPAYTOTAL As String = "AP_CLSTRX_CREDITORJOURNAL_PAYMENT_AMOUNT_GET"

        Dim blnIsBalanceSheet As Boolean
        Dim blnIsNurseryInd As Boolean
        Dim blnIsBlockRequire As Boolean
        Dim blnIsVehicleRequire As Boolean
        Dim blnIsOthers As Boolean

        Dim intErrNo As Integer
        Dim strOpCodes As String = strOpCode_AddLine & "|" & _
                                   strOpCode_GetSumCJLine & "|" & _
                                   strOpCode_UpdStatus & "|" & _
                                   strOpCd_GetIRTOTAL & "|" & _
                                   strOpCd_GetDNTOTAL & "|" & _
                                   strOpCd_GetCNTOTAL & "|" & _
                                   strOpCd_GetPAYTOTAL
        Dim strDate As String = Date_Validation(txtCJRefDate.Text, False)
        Dim indDate As String = ""

        If CheckDate(txtCJRefDate.Text.Trim(), indDate) = False Then
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

        If (rbVoid.Checked = False) And (rbWriteOff.Checked = False) And (rdAdjust.Checked = False) And _
           (rdAllocation.Checked = False) Then
            lblNoJrnType.Visible = True
            onLoad_Button()
            Exit Sub
        Else
            intJrnType = CInt(lblJrnType.Text)
        End If

        intInvoiceRcvInd = IIf(strInvoiceRcv = "", 0, 1)
        intDebitNoteInd = IIf(strDebitNote = "", 0, 1)
        intCreditNoteInd = IIf(strCreditNote = "", 0, 1)
        intPaymentInd = IIf(strPayment = "", 0, 1)
        intCreditorJournal = IIf(strSelCreditJrn = "", 0, 1)
        intGoodsRcvInd = IIf(strGoodsRcv = "", 0, 1)

        If (intInvoiceRcvInd + intDebitNoteInd + intCreditNoteInd + intPaymentInd + intCreditorJournal + intGoodsRcvInd) = 0 Then
            If rbVoid.Checked Or rbWriteOff.Checked Or rdAllocation.Checked Then
                lblErrNoSelectDoc.Visible = True
                onLoad_Button()
                Exit Sub
            End If
        Else
            If (intInvoiceRcvInd + intDebitNoteInd + intCreditNoteInd + intPaymentInd + intCreditorJournal + intGoodsRcvInd) > 1 Then
                lblErrManySelectDoc.Visible = True
                BindInvoiceRcv(ddlInvoiceRcv.SelectedItem.Value)
                BindDebitNote(ddlDebitNote.SelectedItem.Value)
                BindCreditNote(ddlCreditNote.SelectedItem.Value)
                BindCreditorJournal(ddlCreditorJournal.SelectedItem.Value)
                BindPayment(ddlPayment.SelectedItem.Value)
                'BindAccount(ddlAccount.SelectedItem.Value)
                BindPreBlock(txtAccCode.Text, ddlPreBlock.SelectedItem.Value)
                BindBlock(txtAccCode.Text, ddlBlock.SelectedItem.Value)
                BindVehicle(txtAccCode.Text, ddlVehCode.SelectedItem.Value)
                BindVehicleExpense(True, ddlVehExpCode.SelectedItem.Value)
                BindInvPPNH(ddlInvoiceRcv.SelectedItem.Value)
                BindPayPPNH(ddlPayment.SelectedItem.Value)
                BindGoodsRcv(ddlGoodsRcv.SelectedItem.Value)
                onLoad_Button()
                Exit Sub
            End If
        End If

        If (rbVoid.Checked = True) Or (rdAllocation.Checked = True) Then
            strAccCode = ""
            strBlkCode = ""
            strVehCode = ""
            strVehExpenseCode = ""
        Else
            GetAccountDetails(strAccCode, blnIsBalanceSheet, blnIsNurseryInd, blnIsBlockRequire, blnIsVehicleRequire, blnIsOthers)
            If Not blnIsBalanceSheet Then
                If strAccCode = "" Then
                    lblErrAccount.Visible = True
                    onLoad_Button()
                    Exit Sub
                ElseIf strBlkCode = "" And blnIsBlockRequire = True Then
                    If ddlChargeLevel.SelectedIndex = 1 Then
                        lblErrBlock.Visible = True
                    Else
                        lblPreBlockErr.Visible = True
                    End If
                    onLoad_Button()
                    Exit Sub
                ElseIf strVehCode = "" And blnIsVehicleRequire = True Then
                    lblErrVehicle.Visible = True
                    onLoad_Button()
                    Exit Sub
                ElseIf strVehExpenseCode = "" And blnIsVehicleRequire = True Then
                    lblErrVehicleExp.Visible = True
                    onLoad_Button()
                    Exit Sub
                ElseIf strVehCode <> "" And strVehExpenseCode = "" And lblVehicleOption.Text = True Then
                    lblErrVehicleExp.Visible = True
                    onLoad_Button()
                    Exit Sub
                ElseIf strVehCode = "" And strVehExpenseCode <> "" And lblVehicleOption.Text = True Then
                    lblErrVehicle.Visible = True
                    onLoad_Button()
                    Exit Sub
                End If
            ElseIf blnIsNurseryInd = True Then
                If strAccCode = "" Then
                    lblErrAccount.Visible = True
                    onLoad_Button()
                    Exit Sub
                ElseIf strBlkCode = "" Then
                    If ddlChargeLevel.SelectedIndex = 1 Then
                        lblErrBlock.Visible = True
                    Else
                        lblPreBlockErr.Visible = True
                    End If

                    onLoad_Button()
                    Exit Sub
                End If
            End If
        End If

        If (intJrnType = objAPTrx.EnumCreditorJournalType.Adjustment Or _
            intJrnType = objAPTrx.EnumCreditorJournalType.Allocation) Then
            If Trim(txtAmount.Text) = "" Then
                lblErrReqAmount.Visible = True
                BindInvoiceRcv(ddlInvoiceRcv.SelectedItem.Value)
                BindDebitNote(ddlDebitNote.SelectedItem.Value)
                BindCreditNote(ddlCreditNote.SelectedItem.Value)
                BindCreditorJournal(ddlCreditorJournal.SelectedItem.Value)
                BindPayment(ddlPayment.SelectedItem.Value)
                'BindAccount(ddlAccount.SelectedItem.Value)
                BindPreBlock(txtAccCode.Text, ddlPreBlock.SelectedItem.Value)
                BindBlock(txtAccCode.Text, ddlBlock.SelectedItem.Value)
                BindVehicle(txtAccCode.Text, ddlVehCode.SelectedItem.Value)
                BindVehicleExpense(True, ddlVehExpCode.SelectedItem.Value)
                BindInvPPNH(ddlInvoiceRcv.SelectedItem.Value)
                BindPayPPNH(ddlPayment.SelectedItem.Value)
                BindGoodsRcv(ddlGoodsRcv.SelectedItem.Value)
                onLoad_Button()
                Exit Sub
            Else
                dblAmount = CDbl(txtAmount.Text)
                If dblAmount = 0 Then
                    lblErrReqAmount.Visible = True
                    BindInvoiceRcv(ddlInvoiceRcv.SelectedItem.Value)
                    BindDebitNote(ddlDebitNote.SelectedItem.Value)
                    BindCreditNote(ddlCreditNote.SelectedItem.Value)
                    BindCreditorJournal(ddlCreditorJournal.SelectedItem.Value)
                    BindPayment(ddlPayment.SelectedItem.Value)
                    'BindAccount(ddlAccount.SelectedItem.Value)
                    BindPreBlock(txtAccCode.Text, ddlPreBlock.SelectedItem.Value)
                    BindBlock(txtAccCode.Text, ddlBlock.SelectedItem.Value)
                    BindVehicle(txtAccCode.Text, ddlVehCode.SelectedItem.Value)
                    BindVehicleExpense(True, ddlVehExpCode.SelectedItem.Value)
                    BindInvPPNH(ddlInvoiceRcv.SelectedItem.Value)
                    BindPayPPNH(ddlPayment.SelectedItem.Value)
                    BindGoodsRcv(ddlGoodsRcv.SelectedItem.Value)
                    onLoad_Button()
                    Exit Sub
                End If
            End If
        End If

        If strSelectedCJID = "" Then
            Update_CreditorJournal(objAPTrx.EnumCreditorJournalStatus.Active, objCJID, strDocNotFulfil)

            If strDocNotFulfil <> "" Then
                BindInvoiceRcv(ddlInvoiceRcv.SelectedItem.Value)
                BindDebitNote(ddlDebitNote.SelectedItem.Value)
                BindCreditNote(ddlCreditNote.SelectedItem.Value)
                BindCreditorJournal(ddlCreditorJournal.SelectedItem.Value)
                BindPayment(ddlPayment.SelectedItem.Value)
                'BindAccount(ddlAccount.SelectedItem.Value)
                BindPreBlock(txtAccCode.Text, ddlPreBlock.SelectedItem.Value)
                BindBlock(txtAccCode.Text, ddlBlock.SelectedItem.Value)
                BindVehicle(txtAccCode.Text, ddlVehCode.SelectedItem.Value)
                BindVehicleExpense(True, ddlVehExpCode.SelectedItem.Value)
                BindInvPPNH(ddlInvoiceRcv.SelectedItem.Value)
                BindPayPPNH(ddlPayment.SelectedItem.Value)
                BindGoodsRcv(ddlGoodsRcv.SelectedItem.Value)
                onLoad_Button()
                Exit Sub
            End If
            strSelectedCJID = objCJID
        End If
        Dim strParam As String = intJrnType & "|" & _
                                 objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.APCreditJrnLn) & "|" & _
                                 strSelectedCJID & "|" & _
                                 strInvoiceRcv & "|" & _
                                 strDebitNote & "|" & _
                                 strCreditNote & "|" & _
                                 strPayment & "|" & _
                                 strAccCode & "|" & _
                                 strBlkCode & "|" & _
                                 strVehCode & "|" & _
                                 strVehExpenseCode & "|" & _
                                 dblAmount & "|" & _
                                 strSelCreditJrn & "|" & _
                                 IIf(lblPPHRateHidden.Text <> "", lblPPHRateHidden.Text, "0") & "|" & _
                                 IIf(lblPPNHidden.Text <> "", lblPPNHidden.Text, "2") & "|" & _
                                 IIf(lblPPNAmtHidden.Text <> "", lblPPNAmtHidden.Text, "0") & "|" & _
                                 IIf(lblPPHAmtHidden.Text <> "", lblPPHAmtHidden.Text, "0") & "|" & _
                                 IIf(lblNetAmtHidden.Text <> "", lblNetAmtHidden.Text, "0") & "|" & _
                                 IIf(lblInvTypeHidden.Text <> "", lblInvTypeHidden.Text, "0") & "|" & _
                                 IIf(lblCurrencyHidden.Text <> "", lblCurrencyHidden.Text, "IDR") & "|" & _
                                 IIf(lblRateHidden.Text <> "", lblRateHidden.Text, "1") & "|" & _
                                 strGoodsRcv

        Try
            If rbVoid.Checked = False And rdAllocation.Checked = False And _
               ddlChargeLevel.SelectedIndex = 0 And RowPreBlk.Visible = True And _
               ((blnIsBalanceSheet = True And blnIsNurseryInd = True) Or (blnIsBalanceSheet = False And blnIsBlockRequire = True)) Then
                strParamList = Session("SS_LOCATION") & "|" & _
                                       txtAccCode.Text.Trim & "|" & _
                                       ddlPreBlock.SelectedItem.Value.Trim & "|" & _
                                       objGLSetup.EnumBlockStatus.Active
                intErrNo = objAPTrx.mtdUpdCreditorJournalLineByBlock(strOpCodeGLSubBlkByBlk, _
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
                intErrNo = objAPTrx.mtdUpdCreditorJournalLine(strOpCodes, _
                                                              strCompany, _
                                                              strLocation, _
                                                              strUserId, _
                                                              strAccMonth, _
                                                              strAccYear, _
                                                              strParam, _
                                                              objUpdStatus)
            End If
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_TRX_CREDITJRNDET_ADD_LINE&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_cjlist.aspx")
        End Try

        If objUpdStatus = 2 Or objUpdStatus = 4 Then
            lblErrConfirmNotFulFil.Text = lblErrConfirmNotFulFilText.Text & strInvoiceRcv & strDebitNote & strCreditNote & strPayment & "."
            lblErrConfirmNotFulFil.Visible = True
            BindInvoiceRcv(ddlInvoiceRcv.SelectedItem.Value)
            BindDebitNote(ddlDebitNote.SelectedItem.Value)
            BindCreditNote(ddlCreditNote.SelectedItem.Value)
            BindCreditorJournal(ddlCreditorJournal.SelectedItem.Value)
            BindPayment(ddlPayment.SelectedItem.Value)
            'BindAccount(ddlAccount.SelectedItem.Value)
            BindPreBlock(txtAccCode.Text, ddlPreBlock.SelectedItem.Value)
            BindBlock(txtAccCode.Text, ddlBlock.SelectedItem.Value)
            BindVehicle(txtAccCode.Text, ddlVehCode.SelectedItem.Value)
            BindVehicleExpense(True, ddlVehExpCode.SelectedItem.Value)
            BindInvPPNH(ddlInvoiceRcv.SelectedItem.Value)
            BindPayPPNH(ddlPayment.SelectedItem.Value)
        End If

        onLoad_Display(strSelectedCJID)
        onLoad_DisplayLine(strSelectedCJID)
        onLoad_Button()
    End Sub

    Sub SaveBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim objCJID As String

        If (rbVoid.Checked = False) And (rbWriteOff.Checked = False) And (rdAdjust.Checked = False) And _
           (rdAllocation.Checked = False) Then
            lblNoJrnType.Visible = True
            onLoad_Button()
            Exit Sub
        Else
            intJrnType = CInt(lblJrnType.Text)
        End If

        Update_CreditorJournal(objAPTrx.EnumCreditorJournalStatus.Active, objCJID, strDocNotFulfil)
        onLoad_Display(objCJID)
        onLoad_DisplayLine(objCJID)
        onLoad_Button()
    End Sub

    Sub RefreshBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        onLoad_Display(strSelectedCJID)
        onLoad_DisplayLine(strSelectedCJID)
        onLoad_Button()
    End Sub

    Sub ConfirmBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim objCJID As New Object()

        If (rbVoid.Checked = False) And (rbWriteOff.Checked = False) And (rdAdjust.Checked = False) And _
           (rdAllocation.Checked = False) Then
            lblNoJrnType.Visible = True
            onLoad_Button()
            Exit Sub
        Else
            intJrnType = CInt(lblJrnType.Text)
        End If

        Update_CreditorJournal(objAPTrx.EnumCreditorJournalStatus.Confirmed, objCJID, strDocNotFulfil)
        onLoad_Display(objCJID)
        onLoad_DisplayLine(objCJID)
        onLoad_Button()
    End Sub

    Sub DeleteBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim objCJID As New Object()

        If (rbVoid.Checked = False) And (rbWriteOff.Checked = False) And (rdAdjust.Checked = False) And _
           (rdAllocation.Checked = False) Then
            lblNoJrnType.Visible = True
            onLoad_Button()
            Exit Sub
        Else
            intJrnType = CInt(lblJrnType.Text)
        End If

        Update_CreditorJournal(objAPTrx.EnumCreditorJournalStatus.Deleted, objCJID, strDocNotFulfil)
        onLoad_Display(objCJID)
        onLoad_DisplayLine(objCJID)
        onLoad_Button()
    End Sub

    Sub UnDeleteBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim objCJID As New Object()

        If (rbVoid.Checked = False) And (rbWriteOff.Checked = False) And (rdAdjust.Checked = False) And _
           (rdAllocation.Checked = False) Then
            lblNoJrnType.Visible = True
            onLoad_Button()
            Exit Sub
        Else
            intJrnType = CInt(lblJrnType.Text)
        End If

        Update_CreditorJournal(objAPTrx.EnumCreditorJournalStatus.Active, objCJID, strDocNotFulfil)
        onLoad_Display(objCJID)
        onLoad_DisplayLine(objCJID)
        onLoad_Button()
    End Sub


    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strOpCode_DelLine As String = "AP_CLSTRX_CREDITORJOURNAL_LINE_DEL"
        Dim strOpCode_UpdStatus As String = "AP_CLSTRX_CREDITORJOURNAL_STATUS_UPD"
        Dim strOpCodes As String = strOpCode_DelLine & "|" & strOpCode_UpdStatus
        Dim strParam As String
        Dim lblDelText As Label
        Dim stCJLNId As String
        Dim intErrNo As Integer
        Dim strDate As String = Date_Validation(txtCJRefDate.Text, False)
        Dim indDate As String = ""

        If CheckDate(txtCJRefDate.Text.Trim(), indDate) = False Then
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
        lblDelText = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("JrnLnId")
        stCJLNId = lblDelText.Text

        Try
            strParam = stCJLNId & "|" & strSelectedCJID
            intErrNo = objAPTrx.mtdDelCreditorJournalLine(strOpCodes, _
                                                          strCompany, _
                                                          strLocation, _
                                                          strUserId, _
                                                          strAccMonth, _
                                                          strAccYear, _
                                                          strParam)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_TRX_CREDITJRNDET_DEL_LINE&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_cjlist.aspx")
        End Try

        onLoad_Display(strSelectedCJID)
        onLoad_DisplayLine(strSelectedCJID)
        onLoad_Button()
    End Sub

    Sub BackBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("ap_trx_cjlist.aspx")
    End Sub
    Sub ibRefresh_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        BindInvoiceRcv("")
        BindDebitNote("")
        BindCreditNote("")
        BindCreditorJournal("")
        BindPayment("")
        BindGoodsRcv("")
        lblPPHRateHidden.Text = ""
        lblPPNHidden.Text = ""
        lblPPNAmtHidden.Text = ""
        lblPPHAmtHidden.Text = ""
        lblNetAmtHidden.Text = ""
        lblInvTypeHidden.Text = ""
    End Sub

    Sub PrintBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)

        Dim strCJID As String

        strCJID = Trim(lblCJID.Text)

        Response.Write("<Script Language=""JavaScript"">window.open(""../reports/AP_Rpt_CJDet.aspx?Type=Print&CompName=" & strCompany & _
                       "&Location=" & strLocation & _
                       "&CJID=" & strCJID & _
                       """,null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")


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

    Sub BindGoodsRcv(ByVal pv_strGoodsRcvId As String)
        Dim strOpCode As String = "AP_CLSTRX_CREDITORJOURNAL_GOODSRCV_GET"
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedGRIndex As Integer = 0
        Dim dr As DataRow
        Dim strAccMonth As String = ddlAccMonth.SelectedItem.Value
        Dim strAccYear As String = ddlAccYear.SelectedItem.Value

        Dim dsResult As New Object
        Dim strParamName As String = ""
        Dim strParamValue As String = ""

        strParamName = "LOCCODE|SUPPLIERCODE|STRSEARCH"
        strParamValue = strLocation & "|" & txtSupCode.Text & "|" & " AND A.AccMonth='" & strAccMonth & "' AND A.AccYear='" & strAccYear & "'"

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
                                                    strParamName, _
                                                    strParamValue, _
                                                    dsResult)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_TRX_CREDITJRNDET_GET_CREDITNOTE&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_cjlist.aspx")
        End Try

        For intCnt = 0 To dsResult.Tables(0).Rows.Count - 1
            dsResult.Tables(0).Rows(intCnt).Item("GoodsRcvID") = Trim(dsResult.Tables(0).Rows(intCnt).Item("GoodsRcvID"))
            dsResult.Tables(0).Rows(intCnt).Item("DispGoodsRcvID") = Trim(dsResult.Tables(0).Rows(intCnt).Item("GoodsRcvID")) & ", " & dsResult.Tables(0).Rows(intCnt).Item("POID")
            If pv_strGoodsRcvId = dsResult.Tables(0).Rows(intCnt).Item("GoodsRcvID") Then
                intSelectedGRIndex = intCnt + 1
            End If
        Next intCnt

        dr = dsResult.Tables(0).NewRow()
        dr("GoodsRcvID") = ""
        dr("DispGoodsRcvID") = "Select Goods Receive"
        dsResult.Tables(0).Rows.InsertAt(dr, 0)

        ddlGoodsRcv.DataSource = dsResult.Tables(0)
        ddlGoodsRcv.DataValueField = "GoodsRcvID"
        ddlGoodsRcv.DataTextField = "DispGoodsRcvID"
        ddlGoodsRcv.DataBind()
        ddlGoodsRcv.SelectedIndex = intSelectedGRIndex
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
                        Session("SS_USERID") & "|" & Trim(lblCJID.Text)

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

        strSelectedCJID = Trim(lblCJID.Text)
        onLoad_Display(strSelectedCJID)
        onLoad_DisplayLine(strSelectedCJID)
        onLoad_Button()
    End Sub
End Class

