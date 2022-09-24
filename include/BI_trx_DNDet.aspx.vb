
Imports System
Imports System.Data
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic
Imports System.Math

Public Class BI_trx_DNDet : Inherits Page

    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblDebitNoteID As Label
    Protected WithEvents lblDocType As Label
    Protected WithEvents lblDocTypeValue As Label
    Protected WithEvents lblAccPeriod As Label
    Protected WithEvents lblStatus As Label
    Protected WithEvents lblDateCreated As Label
    Protected WithEvents txtDateCreated As TextBox
    Protected WithEvents btnDateCreated As Image
    Protected WithEvents lblDate As Label
    Protected WithEvents lblFmtDate As Label
    Protected WithEvents lblLastUpdate As Label
    Protected WithEvents lblPrintDate As Label
    Protected WithEvents lblUpdatedBy As Label
    Protected WithEvents ddlBillParty As DropDownList
    'Protected WithEvents ddlAccount As DropDownList

    Protected WithEvents txtAccCode As TextBox
    Protected WithEvents txtAccName As TextBox

    Protected WithEvents ddlBlock As DropDownList
    Protected WithEvents ddlVehCode As DropDownList
    Protected WithEvents ddlVehExpCode As DropDownList
    Protected WithEvents ddlDocType As DropDownList
    Protected WithEvents txtDescription As TextBox
    Protected WithEvents txtTotalUnits As TextBox
    Protected WithEvents txtRate As TextBox
    Protected WithEvents txtAmount As TextBox
    Protected WithEvents tblSelection As HtmlTable
    Protected WithEvents dgLineDet As DataGrid
    Protected WithEvents lblTotalAmount As Label
    Protected WithEvents txtRemark As TextBox
    Protected WithEvents txtCustRef As TextBox
    Protected WithEvents txtDevRef As TextBox
    Protected WithEvents SaveBtn As ImageButton
    Protected WithEvents ConfirmBtn As ImageButton
    Protected WithEvents PrintBtn As ImageButton
    Protected WithEvents CancelBtn As ImageButton
    Protected WithEvents DeleteBtn As ImageButton
    Protected WithEvents UnDeleteBtn As ImageButton
    Protected WithEvents BackBtn As ImageButton
    Protected WithEvents dbnid As HtmlInputHidden
    Protected WithEvents lblStatusHidden As Label
    Protected WithEvents lblDocTypeHidden As Label
    Protected WithEvents lblVehicleOption As Label
    Protected WithEvents lblErrBillParty As Label
    Protected WithEvents lblErrAccount As Label
    Protected WithEvents lblErrBlock As Label
    Protected WithEvents lblErrVehicle As Label
    Protected WithEvents lblErrVehicleExp As Label
    Protected WithEvents lblErrDesc As Label
    Protected WithEvents lblErrTotalUnits As Label
    Protected WithEvents lblErrRate As Label
    Protected WithEvents lblErrAmount As Label
    Protected WithEvents lblErrTotal As Label
    Protected WithEvents lblReferer As Label
    Protected WithEvents lblBillParty As Label
    Protected WithEvents lblAccount As Label
    Protected WithEvents lblBlock As Label
    Protected WithEvents lblVehicle As Label
    Protected WithEvents lblVehExpense As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents lblPleaseSelect As Label
    Protected WithEvents lblSelect As Label
    Protected WithEvents RowChargeLevel As HtmlTableRow
    Protected WithEvents RowPreBlk As HtmlTableRow
    Protected WithEvents RowBlk As HtmlTableRow
    Protected WithEvents lblPreBlockErr As Label
    Protected WithEvents lblPreBlkTag As Label
    Protected WithEvents ddlPreBlock As DropDownList
    Protected WithEvents ddlChargeLevel As DropDownList
    Protected WithEvents hidBlockCharge As HtmlInputHidden
    Protected WithEvents hidChargeLocCode As HtmlInputHidden

    Protected WithEvents lblCRErrRefNo As Label
    Protected WithEvents lblDLErrRefNo As Label

    Protected WithEvents TrLink As HtmlTableRow
    Protected WithEvents lbViewJournal As LinkButton
    Protected WithEvents dgViewJournal As DataGrid
    Protected WithEvents lblTotalDB As Label
    Protected WithEvents lblTotalCR As Label
    Protected WithEvents lblTotalViewJournal As Label


    Dim PreBlockTag As String
    Dim BlockTag As String

    Protected WithEvents lblViewTotalAmount as Label
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()

    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objGLSetup As New agri.GL.clsSetup()
    Dim objBITrx As New agri.BI.clsTrx()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objAdmin As New agri.Admin.clsShare()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Protected objGLTrx As New agri.GL.ClsTrx()


    Dim objDNDs As New Object()
    Dim objDNLnDs As New Object()
    Dim objBPDs As New Object()
    Dim objAccDs As New Object()
    Dim objBlkDs As New Object()
    Dim objVehDs As New Object()
    Dim objVehExpDs As New Object()
    Dim objLangCapDs As New Object()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intBIAR As Integer
    Dim intConfig As Integer

    Dim strSelectedDNID As String
    Dim intDNStatus As Integer
    Dim strAcceptDateFormat As String
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
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumBIAccessRights.BINote), intBIAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            onload_GetLangCap()
            lblErrBillParty.Visible = False
            lblErrAccount.Visible = False
            lblPreBlockErr.Visible = False

            lblCRErrRefNo.Visible = False
            lblDLErrRefNo.Visible = False
            lblErrBlock.Visible = False
            lblErrVehicle.Visible = False
            lblErrVehicleExp.Visible = False
            lblErrDesc.Visible = False
            lblErrTotalUnits.Visible = False
            lblErrRate.Visible = False
            lblErrAmount.Visible = False
            lblErrTotal.Visible = False
            lblReferer.Text = Request.QueryString("referer")
            strSelectedDNID = Trim(IIf(Request.QueryString("dbnid") = "", Request.Form("dbnid"), Request.QueryString("dbnid")))
            dbnid.Value = strSelectedDNID

            If Not IsPostBack Then
                BindChargeLevelDropDownList()
                If strSelectedDNID <> "" Then
                    onLoad_Display(strSelectedDNID)
                    onLoad_DisplayLine(strSelectedDNID)
                    onLoad_Button()
                Else
                    BindDocList("")
                    BindBillParty("")
                    'BindAccount("")
                    BindPreBlock("", "")
                    BindBlock("", "")
                    BindVehicle("", "")
                    BindVehicleExpense(True, "")
                    onLoad_Button()
					TrLink.Visible = False
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_DNDET_GET_LANGCAP_COSTLEVEL&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/BI_trx_DNList.aspx")
        End Try

        lblBillParty.Text = GetCaption(objLangCap.EnumLangCap.BillParty) & lblCode.Text
        lblAccount.Text = GetCaption(objLangCap.EnumLangCap.Account) & lblCode.Text
        lblVehicle.Text = GetCaption(objLangCap.EnumLangCap.Vehicle) & lblCode.Text
        lblVehExpense.Text = GetCaption(objLangCap.EnumLangCap.VehExpense) & lblCode.Text

        dgLineDet.Columns(0).HeaderText = lblAccount.Text
        dgLineDet.Columns(1).HeaderText = lblBlock.Text
        dgLineDet.Columns(2).HeaderText = GetCaption(objLangCap.EnumLangCap.BatchNo)
        dgLineDet.Columns(3).HeaderText = lblVehicle.Text
        dgLineDet.Columns(4).HeaderText = lblVehExpense.Text

        lblErrBillParty.Text = lblPleaseSelect.Text & lblBillParty.Text
        lblErrAccount.Text = "<br>" & lblPleaseSelect.Text & lblAccount.Text
        lblErrBlock.Text = lblPleaseSelect.Text & lblBlock.Text
        lblErrVehicle.Text = lblPleaseSelect.Text & lblVehicle.Text
        lblErrVehicleExp.Text = lblPleaseSelect.Text & lblVehExpense.Text
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_DNDET_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/BI_trx_DNList.aspx")
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
        ddlBillParty.Enabled = False
        txtRemark.Enabled = False
        txtCustRef.Enabled = False
        txtDevRef.Enabled = False
        ddlDocType.Enabled = False
        tblSelection.Visible = False
        SaveBtn.Visible = False
        ConfirmBtn.Visible = False
        CancelBtn.Visible = False
        DeleteBtn.Visible = False
        UnDeleteBtn.Visible = False
        lblDateCreated.Visible = False
        txtDateCreated.Enabled = False
        btnDateCreated.Visible = False
        If (lblStatusHidden.Text <> "") Then
            intStatus = CInt(lblStatusHidden.Text)
            Select Case intStatus
                Case objBITrx.EnumDebitNoteStatus.Active
                    ddlBillParty.Enabled = True
                    ddlDocType.Enabled = True
                    txtDateCreated.Enabled = True
                    btnDateCreated.Visible = True
                    txtRemark.Enabled = True
                    txtCustRef.Enabled = True
                    txtDevRef.Enabled = True
                    tblSelection.Visible = True
                    SaveBtn.Visible = True
                    ConfirmBtn.Visible = True
                    DeleteBtn.Visible = True
                    DeleteBtn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                Case objBITrx.EnumDebitNoteStatus.Deleted
                    UnDeleteBtn.Attributes("onclick") = "javascript:return ConfirmAction('undelete');"
                    UnDeleteBtn.Visible = True
                Case objBITrx.EnumDebitNoteStatus.Confirmed
                Case Else
            End Select
            lblDateCreated.Visible = True
        Else
            ddlBillParty.Enabled = True
            txtRemark.Enabled = True
            txtCustRef.Enabled = True
            txtDevRef.Enabled = True
            ddlDocType.Enabled = True
            tblSelection.Visible = True
            SaveBtn.Visible = True
            txtDateCreated.Enabled = True
            btnDateCreated.Visible = True
        End If
        If lblDebitNoteID.Text.Trim() = "" Then
            txtDateCreated.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
        End If
    End Sub

    Sub BindDocList(ByVal pv_strDoctype As String)
        ddlDocType.Items.Clear()
        ddlDocType.Items.Add(New ListItem(objBITrx.mtdGetDebitNoteDocType(objBITrx.EnumDebitNoteDocType.Manual), objBITrx.EnumDebitNoteDocType.Manual))
        ddlDocType.Items.Add(New ListItem(objBITrx.mtdGetDebitNoteDocType(objBITrx.EnumDebitNoteDocType.Manual_Millware), objBITrx.EnumDebitNoteDocType.Manual_Millware))
        ddlDocType.Items.Add(New ListItem(objBITrx.mtdGetDebitNoteDocType(objBITrx.EnumDebitNoteDocType.Auto_Millware), objBITrx.EnumDebitNoteDocType.Auto_Millware))
        Select Case Trim(pv_strDoctype)
            Case objBITrx.EnumDebitNoteDocType.Manual
                ddlDocType.SelectedIndex = 0
            Case objBITrx.EnumDebitNoteDocType.Manual_Millware
                ddlDocType.SelectedIndex = 1
            Case objBITrx.EnumDebitNoteDocType.Auto_Millware
                ddlDocType.SelectedIndex = 2
            Case Else
                If strSelectedDNID <> "" Then
                    ddlDocType.Items.Add(New ListItem(objBITrx.mtdGetDebitNoteDocType(pv_strDoctype), pv_strDoctype))
                    ddlDocType.SelectedIndex = 2
                End If
        End Select
    End Sub

    Sub onLoad_Display(ByVal pv_strDebitNoteId As String)
        Dim strOpCd_Get As String = "BI_CLSTRX_DEBITNOTE_DETAILS_GET"
        Dim objDNDs As New Object()
        Dim intErrNo As Integer
        Dim strParam As String = pv_strDebitNoteId
        Dim intCnt As Integer = 0
        Dim nBillPartyCode As String = ""

        dbnid.Value = pv_strDebitNoteId

        Try
            intErrNo = objBITrx.mtdGetDebitNote(strOpCd_Get, _
                                                strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strAccMonth, _
                                                strAccYear, _
                                                strParam, _
                                                objDNDs, _
                                                True)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_DEBITNOTEDET_GET_HEADER&errmesg=" & Exp.ToString & "&redirect=BI/trx/BI_trx_DNList.aspx")
        End Try

        lblDebitNoteID.Text = pv_strDebitNoteId
        If objDNDs.Tables(0).Rows.Count > 0 Then
            lblDocTypeHidden.Text = Trim(objDNDs.Tables(0).Rows(0).Item("DocType"))
            lblAccPeriod.Text = Trim(objDNDs.Tables(0).Rows(0).Item("AccMonth")) & "/" & Trim(objDNDs.Tables(0).Rows(0).Item("AccYear"))
            lblStatus.Text = objBITrx.mtdGetDebitNoteStatus(Trim(objDNDs.Tables(0).Rows(0).Item("Status")))
            intDNStatus = CInt(Trim(objDNDs.Tables(0).Rows(0).Item("Status")))
            lblStatusHidden.Text = intDNStatus
            lblDateCreated.Text = objGlobal.GetLongDate(objDNDs.Tables(0).Rows(0).Item("CreateDate"))
            txtDateCreated.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), objDNDs.Tables(0).Rows(0).Item("CreateDate"))
            lblLastUpdate.Text = objGlobal.GetLongDate(objDNDs.Tables(0).Rows(0).Item("UpdateDate"))
            lblPrintDate.Text = objGlobal.GetLongDate(objDNDs.Tables(0).Rows(0).Item("PrintDate"))
            lblUpdatedBy.Text = Trim(objDNDs.Tables(0).Rows(0).Item("UserName"))
            lblTotalAmount.Text = FormatNumber(objDNDs.Tables(0).Rows(0).Item("TotalAmount"), CInt(Session("SS_ROUNDNO")))
            lblViewTotalAmount.Text = objGlobal.GetIDDecimalSeparator(FormatNumber(objDNDs.Tables(0).Rows(0).Item("TotalAmount"), 0))
            txtRemark.Text = Trim(objDNDs.Tables(0).Rows(0).Item("Remark"))
            txtCustRef.Text = Trim(objDNDs.Tables(0).Rows(0).Item("CustRef"))
            txtDevRef.Text = Trim(objDNDs.Tables(0).Rows(0).Item("DeliveryRef"))
            nBillPartyCode = Trim(objDNDs.Tables(0).Rows(0).Item("BillPartyCode"))
            BindDocList(Trim(objDNDs.Tables(0).Rows(0).Item("Doctype")))
            'BindAccount("")
            BindPreBlock("", "")
            BindBlock("", "")
            BindVehicle("", "")
            BindVehicleExpense(True, "")



            Select Case CInt(objDNDs.Tables(0).Rows(0).Item("DocType"))
                Case objBITrx.EnumDebitNoteDocType.Manual, objBITrx.EnumDebitNoteDocType.Manual_Millware
                    BindBillParty(Trim(objDNDs.Tables(0).Rows(0).Item("BillPartyCode")))
                Case Else
                    ddlBillParty.Items.Add(New ListItem(Trim(objDNDs.Tables(0).Rows(0).Item("BillPartyCode")), Trim(objDNDs.Tables(0).Rows(0).Item("BillPartyCode"))))
            End Select
        Else
            BindBillParty("")
        End If
    End Sub


    Sub onLoad_DisplayLine(ByVal pv_strDebitNoteId As String)
        Dim strOpCd_GetLine As String = "BI_CLSTRX_DEBITNOTE_LINE_GET"
        Dim strParam As String = pv_strDebitNoteId
        Dim lbButton As LinkButton
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim dblDebit As Double = 0
        Dim dblCredit As Double = 0
        Dim bDispBatchNo As Boolean = False

        Try
            intErrNo = objBITrx.mtdGetDebitNoteLine(strOpCd_GetLine, strParam, objDNLnDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_DEBITNOTEDET_GET_LINE&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/BI_trx_DNList.aspx")
        End Try

        For intCnt = 0 To objDNLnDs.Tables(0).Rows.Count - 1
            objDNLnDs.Tables(0).Rows(intCnt).Item("DebitNoteLnID") = Trim(objDNLnDs.Tables(0).Rows(intCnt).Item("DebitNoteLnID"))
            objDNLnDs.Tables(0).Rows(intCnt).Item("AccCode") = Trim(objDNLnDs.Tables(0).Rows(intCnt).Item("AccCode"))
            objDNLnDs.Tables(0).Rows(intCnt).Item("BlkCode") = Trim(objDNLnDs.Tables(0).Rows(intCnt).Item("BlkCode"))
            objDNLnDs.Tables(0).Rows(intCnt).Item("VehCode") = Trim(objDNLnDs.Tables(0).Rows(intCnt).Item("VehCode"))
            objDNLnDs.Tables(0).Rows(intCnt).Item("VehExpenseCode") = Trim(objDNLnDs.Tables(0).Rows(intCnt).Item("VehExpenseCode"))
            objDNLnDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objDNLnDs.Tables(0).Rows(intCnt).Item("Description"))
        Next intCnt

        dgLineDet.DataSource = objDNLnDs.Tables(0)
        dgLineDet.DataBind()

        For intCnt = 0 To objDNLnDs.Tables(0).Rows.Count - 1
            Select Case intDNStatus
                Case objBITrx.EnumDebitNoteStatus.Active
                    lbButton = dgLineDet.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = True
                    lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                Case Else
                    lbButton = dgLineDet.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False
            End Select
            If Not objDNLnDs.Tables(0).Rows(intCnt).Item("BatchNo") = -1 Then
                bDispBatchNo = True
            End If
        Next

        dgLineDet.Columns(2).Visible = bDispBatchNo

        If objDNLnDs.Tables(0).Rows.Count > 0 Then
            TrLink.Visible = True
        Else
            TrLink.Visible = False
        End If
    End Sub


    Sub BindBillParty(ByVal pv_strCode As String)
        Dim strOpCd As String = "GL_CLSSETUP_BILLPARTY_GET"
        Dim dr As DataRow
        Dim strParam As String = "||1||BP.BillPartyCode|ASC|"
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer = 0

        Try
            intErrNo = objGLSetup.mtdGetBillParty(strOpCd, strParam, objBPDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_DEBITNOTEDET_BILLPARTY_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
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
        dr("Name") = lblPleaseSelect.Text & lblBillParty.Text
        objBPDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlBillParty.DataSource = objBPDs.Tables(0)
        ddlBillParty.DataValueField = "BillPartyCode"
        ddlBillParty.DataTextField = "Name"
        ddlBillParty.DataBind()
        ddlBillParty.SelectedIndex = intSelectedIndex
    End Sub


    Sub BindAccount(ByVal pv_strAccCode As String)
        'Dim strOpCd As String = "GL_CLSSETUP_ACCOUNTCODE_LIST_GET"
        'Dim dr As DataRow
        'Dim strParam As String = "Order By ACC.AccCode|And ACC.Status = '" & objGLSetup.EnumAccountCodeStatus.Active & "'"
        'Dim intErrNo As Integer
        'Dim intCnt As Integer
        'Dim intSelectedIndex As Integer = 0

        'Try
        '    intErrNo = objGLSetup.mtdGetMasterList(strOpCd, _
        '                                           strParam, _
        '                                           objGLSetup.EnumGLMasterType.AccountCode, _
        '                                           objAccDs)
        'Catch Exp As System.Exception
        '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_DEBITNOTEDET_ACCCODE_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        'End Try

        'For intCnt = 0 To objAccDs.Tables(0).Rows.Count - 1
        '    objAccDs.Tables(0).Rows(intCnt).Item("AccCode") = Trim(objAccDs.Tables(0).Rows(intCnt).Item("AccCode"))
        '    objAccDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objAccDs.Tables(0).Rows(intCnt).Item("AccCode")) & " (" & Trim(objAccDs.Tables(0).Rows(intCnt).Item("Description")) & ")"
        '    If objAccDs.Tables(0).Rows(intCnt).Item("AccCode") = Trim(pv_strAccCode) Then
        '        intSelectedIndex = intCnt + 1
        '    End If
        'Next

        'dr = objAccDs.Tables(0).NewRow()
        'dr("AccCode") = ""
        'dr("Description") = lblPleaseSelect.Text & lblAccount.Text
        'objAccDs.Tables(0).Rows.InsertAt(dr, 0)

        'ddlAccount.DataSource = objAccDs.Tables(0)
        'ddlAccount.DataValueField = "AccCode"
        'ddlAccount.DataTextField = "Description"
        'ddlAccount.DataBind()
        'ddlAccount.SelectedIndex = intSelectedIndex
        'ddlAccount.AutoPostBack = True
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_DEBITNOTEDET_GET_ACCOUNT_DETAILS&errmesg=" & lblErrMessage.Text & "&redirect=")
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
        Dim strAcc As String = Request.Form("txtAccCode")
        Dim strBlk As String = Request.Form("ddlBlock")
        Dim strPreBlk As String = Request.Form("ddlPreBlock")
        Dim strVeh As String = Request.Form("ddlVehCode")
        Dim strVehExp As String = Request.Form("ddlVehExpCode")

        GetAccountDetails(strAcc, blnIsBalanceSheet, blnIsNurseryInd, blnIsBlockRequire, blnIsVehicleRequire, blnIsOthers)

        If Not blnIsBalanceSheet Then
            If blnIsBlockRequire Then
                BindPreBlock(strAcc, strPreBlk)
                BindBlock(strAcc, strBlk)
                BindVehicle("", strVeh)
                BindVehicleExpense(True, strVehExp)
            Else
                BindPreBlock("", strPreBlk)
                BindBlock("", strBlk)
                BindVehicle("", strVeh)
                BindVehicleExpense(True, strVehExp)
            End If

            If blnIsVehicleRequire Then
                BindVehicle(strAcc, strVeh)
                BindVehicleExpense(False, strVehExp)
            Else
                BindVehicle("", strVeh)
                BindVehicleExpense(True, strVehExp)
            End If

            If blnIsOthers Then
                lblVehicleOption.Text = True
                BindVehicle("%", strVeh)
                BindVehicleExpense(False, strVehExp)
            Else
                lblVehicleOption.Text = False
            End If
        ElseIf blnIsNurseryInd = True Then
            BindPreBlock(strAcc, strPreBlk)
            BindBlock(strAcc, strBlk)
            BindVehicle("", strVeh)
            BindVehicleExpense(True, strVehExp)
        Else
            BindPreBlock("", strPreBlk)
            BindBlock("", strBlk)
            BindVehicle("", strVeh)
            BindVehicleExpense(True, strVehExp)
        End If
    End Sub

    Sub onSelect_StrAccCode(ByVal sender As System.Object, ByVal e As System.EventArgs)
        GetCOADetail(txtAccCode.Text.Trim)
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
            txtAccCode.Text = ""
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_DEBITNOTEDET_BLOCK_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_DEBITNOTEDET_VEH_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
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
        Dim strParam As String = "Order By VehExpenseCode ASC| And Veh.Status = '" & objGLSetup.EnumVehExpenseStatus.Active & "' "
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer = 0
        Try
            If pv_IsBlankList Or ddlVehCode.Items.Count = 1 Then
                strParam += "And Veh.VehExpensecode = ''"
            End If
            intErrNo = objGLSetup.mtdGetMasterList(strOpCd, _
                                                   strParam, _
                                                   objGLSetup.EnumGLMasterType.VehicleExpense, _
                                                   objVehExpDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_DEBITNOTEDET_VEHEXPENSE_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
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

    Sub Update_DebitNote(ByVal pv_intStatus As Integer, ByRef pr_objNewDNID As Object, ByRef pr_intSuccess As Integer)
        Dim strRemark As String = txtRemark.Text

        Dim strOpCd_AddDebitNote As String = "BI_CLSTRX_DEBITNOTE_ADD"
        Dim strOpCd_UpdDebitNote As String = "BI_CLSTRX_DEBITNOTE_UPD"
        Dim strOpCodes As String = strOpCd_AddDebitNote & "|" & _
                                   strOpCd_UpdDebitNote
        Dim intErrNo As Integer
        Dim strParam As String = ""
        Dim objChkRef As Object
        Dim intErrNoRef As Integer
        Dim strParamRef As String = ""
        Dim strOpCd_RefNo As String = "BI_CLSTRX_CHK_REF_NO"
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

        pr_intSuccess = 1

        lblDate.Visible = False
        lblFmtDate.Visible = False
        If strSelectedDNID = "" Then
            If txtDateCreated.Text.Trim() = "" Then
                lblFmtDate.Text = "Please enter Date Created"
                lblFmtDate.Visible = True
                pr_intSuccess = 0
                Exit Sub
            ElseIf CheckDate(txtDateCreated.Text.Trim(), strDate) = False Then
                lblDate.Visible = True
                lblFmtDate.Visible = True
                pr_intSuccess = 0
                Exit Sub
            End If
        End If
        If ddlBillParty.SelectedItem.Value = "" Then
            lblErrBillParty.Visible = True
            pr_intSuccess = 0
            Exit Sub
        End If

        strAccYear = Year(strDate)
        strAccMonth = Month(strDate)

        strParamRef = "BI_DEBITNOTE|CustRef|DebitNoteID|" & strSelectedDNID & "|" & _
                      ddlBillParty.SelectedItem.Value & "|" & _
                      txtCustRef.Text

        Try
            intErrNoRef = objBITrx.mtdChkRefNo(strOpCd_RefNo, _
                                              strParamRef, _
                                              objChkRef)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_INVOICEDET_CHK_REF&errmesg=" & Exp.ToString() & "&redirect=BI/trx/BI_trx_InvoiceList.aspx")
        End Try

        If objChkRef.Tables(0).Rows.Count > 0 And txtCustRef.Text <> "" Then
            lblCRErrRefNo.Visible = True
            pr_intSuccess = 0
            Exit Sub
        End If

        strParamRef = "BI_DEBITNOTE|DeliveryRef|DebitNoteID|" & strSelectedDNID & "|" & _
                      ddlBillParty.SelectedItem.Value & "|" & _
                      txtDevRef.Text

        Try
            intErrNoRef = objBITrx.mtdChkRefNo(strOpCd_RefNo, _
                                              strParamRef, _
                                              objChkRef)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_INVOICEDET_CHK_REF&errmesg=" & Exp.ToString() & "&redirect=BI/trx/BI_trx_InvoiceList.aspx")
        End Try

        If objChkRef.Tables(0).Rows.Count > 0 And txtDevRef.Text <> "" Then
            lblDLErrRefNo.Visible = True
            pr_intSuccess = 0
            Exit Sub
        End If

        strNewIDFormat = "CDN" & "/" & strCompany & "/" & strLocation & "/" & strAccYear & IIf(Len(Trim(strAccMonth)) = 1, "0" & strAccMonth, strAccMonth) & "/"

        strParam = strParam & objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.BIDebitNote) & "|" & _
                              strSelectedDNID & "|" & _
                              ddlBillParty.SelectedItem.Value & "|" & _
                              strRemark & "|" & _
                              ddlDocType.SelectedItem.Value & "|" & _
                              pv_intStatus & "|" & _
                              txtCustRef.Text & "|" & _
                              txtDevRef.Text & "|||" & _
                              strDate & "|" & _
                              strNewIDFormat

        Try
            intErrNo = objBITrx.mtdUpdDebitNote(strOpCodes, _
                                                strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strAccMonth, _
                                                strAccYear, _
                                                strParam, _
                                                pr_objNewDNID)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_DEBITNOTEDET_UPD_DATA&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/BI_trx_DNList.aspx")
        End Try

        pr_objNewDNID = IIf(strSelectedDNID = "", pr_objNewDNID, strSelectedDNID)
    End Sub


    Sub AddBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCodeGLSubBlkByBlk As String = "GL_CLSSETUP_SUBBLOCK_BY_BLOCK_GET"
        Dim strParamList As String
        Dim objDNID As New Object()
        Dim strAccCode As String = Request.Form("txtAccCode")
        Dim strBlkCode As String
        Dim strVehCode As String = Request.Form("ddlVehCode")
        Dim strVehExpenseCode As String = Request.Form("ddlVehExpCode")
        Dim strDescription As String = Trim(txtDescription.Text)
        Dim dblTotalUnit As Double
        Dim dblRate As Double
        Dim dblAmount As Double
        Dim strOpCode_AddLine As String = "BI_CLSTRX_DEBITNOTE_LINE_ADD"
        Dim strOpCode_GetSumAmount As String = "BI_CLSTRX_DEBITNOTE_SUM_LINEAMOUNT_GET"
        Dim strOpCode_UpdTotalAmount As String = "BI_CLSTRX_DEBITNOTE_UPD"
        Dim strOpCodes As String = strOpCode_AddLine & "|" & strOpCode_GetSumAmount & "|" & strOpCode_UpdTotalAmount
        Dim intErrNo As Integer
        Dim intSuccess As Integer
        Dim blnIsBalanceSheet As Boolean
        Dim blnIsNurseryInd As Boolean
        Dim blnIsBlockRequire As Boolean
        Dim blnIsVehicleRequire As Boolean
        Dim blnIsOthers As Boolean
        Dim strDate As String = Date_Validation(txtDateCreated.Text, False)

        strAccYear = Year(strDate)
        strAccMonth = Month(strDate)

        If ddlChargeLevel.SelectedIndex = 1 Then
            strBlkCode = Request.Form("ddlBlock")
        Else
            strBlkCode = Request.Form("ddlPreBlock")
        End If

        GetAccountDetails(strAccCode, blnIsBalanceSheet, blnIsNurseryInd, blnIsBlockRequire, blnIsVehicleRequire, blnIsOthers)


        If Not blnIsBalanceSheet Then
            If strAccCode = "" Then
                lblErrAccount.Visible = True
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
                lblErrAccount.Visible = True
                Exit Sub
            ElseIf strBlkCode = "" And blnIsBlockRequire = True Then
                If ddlChargeLevel.SelectedIndex = 1 Then
                    lblErrBlock.Visible = True
                Else
                    lblPreBlockErr.Visible = True
                End If
                Exit Sub
            End If
        End If

        If txtDescription.Text = "" Then
            lblErrDesc.Visible = True
            Exit Sub
        End If

        If Trim(txtTotalUnits.Text) = "" Then
            lblErrTotalUnits.Visible = True
            Exit Sub
        Else
            dblTotalUnit = CDbl(txtTotalUnits.Text)
        End If

        If Trim(txtRate.Text) = "" Then
            lblErrRate.Visible = True
            Exit Sub
        Else
            dblRate = CDbl(txtRate.Text)
        End If

        If Trim(txtAmount.Text) = "" Then
            lblErrAmount.Visible = True
            Exit Sub
        Else
            'dblAmount = CDbl(txtAmount.Text)
            dblAmount = Round(dblTotalUnit * dblRate, CInt(Session("SS_ROUNDNO")))
            txtAmount.Text = dblAmount
        End If

        If strSelectedDNID = "" Then
            Update_DebitNote(objBITrx.EnumDebitNoteStatus.Active, objDNID, intSuccess)
            If intSuccess = 1 Then
                If UCase(TypeName(objDNID)) = "OBJECT" Then
                    Exit Sub
                Else
                    strSelectedDNID = objDNID
                End If
            Else
                Exit Sub
            End If
        End If


        Dim strParam As String = objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.BIDebitNoteLn) & "|" & _
                                 strSelectedDNID & "|" & _
                                 strAccCode & "|" & _
                                 strBlkCode & "|" & _
                                 strVehCode & "|" & _
                                 strVehExpenseCode & "|" & _
                                 txtDescription.Text & "|" & _
                                 dblTotalUnit & "|" & _
                                 dblRate & "|" & _
                                 dblAmount

        Try
            If ddlChargeLevel.SelectedIndex = 0 And RowPreBlk.Visible = True Then
                strParamList = Session("SS_LOCATION") & "|" & _
                                       txtAccCode.Text.Trim & "|" & _
                                       ddlPreBlock.SelectedItem.Value.Trim & "|" & _
                                       objGLSetup.EnumBlockStatus.Active & "|" & _
                                       strAccMonth & "|" & strAccYear

                intErrNo = objBITrx.mtdUpdDebitNoteLineByBlock(strOpCodeGLSubBlkByBlk, _
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
                intErrNo = objBITrx.mtdUpdDebitNoteLine(strOpCodes, _
                                                        strCompany, _
                                                        strLocation, _
                                                        strUserId, _
                                                        strAccMonth, _
                                                        strAccYear, _
                                                        strParam)
            End If
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_DEBITNOTEDET_ADD_LINEA&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/BI_trx_DNList.aspx")
        End Try

        onLoad_Display(strSelectedDNID)
        onLoad_DisplayLine(strSelectedDNID)
        onLoad_Button()
    End Sub

    Sub SaveBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim objDNID As String
        Dim intSuccess As Integer

        Update_DebitNote(objBITrx.EnumDebitNoteStatus.Active, objDNID, intSuccess)
        If intSuccess = 1 Then
            onLoad_Display(objDNID)
            onLoad_DisplayLine(objDNID)
            onLoad_Button()
        Else
            Exit Sub
        End If
    End Sub

    Sub ConfirmBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim objDNID As New Object()
        Dim intSuccess As Integer

        If CDbl(lblTotalAmount.Text) <= 0 Then
            lblErrTotal.Visible = True
        Else
            Update_DebitNote(objBITrx.EnumDebitNoteStatus.Confirmed, objDNID, intSuccess)
            If intSuccess = 1 Then
                onLoad_Display(objDNID)
                onLoad_DisplayLine(objDNID)
                onLoad_Button()
            Else
                Exit Sub
            End If
        End If
    End Sub

    Sub CancelBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
    End Sub

    Sub DeleteBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim objDNID As New Object()
        Dim intSuccess As Integer

        Update_DebitNote(objBITrx.EnumDebitNoteStatus.Deleted, objDNID, intSuccess)
        If intSuccess = 1 Then
            onLoad_Display(objDNID)
            onLoad_DisplayLine(objDNID)
            onLoad_Button()
        Else
            Exit Sub
        End If
    End Sub

    Sub UnDeleteBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim objDNID As New Object()
        Dim intSuccess As Integer

        Update_DebitNote(objBITrx.EnumDebitNoteStatus.Active, objDNID, intSuccess)
        If intSuccess = 1 Then
            onLoad_Display(objDNID)
            onLoad_DisplayLine(objDNID)
            onLoad_Button()
        Else
            Exit Sub
        End If
    End Sub


    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strOpCode_DelLine As String = "BI_CLSTRX_DEBITNOTE_LINE_DEL"
        Dim strOpCode_GetSumAmount As String = "BI_CLSTRX_DEBITNOTE_SUM_LINEAMOUNT_GET"
        Dim strOpCode_UpdTotalAmount As String = "BI_CLSTRX_DEBITNOTE_TOTALAMOUNT_UPD"
        Dim strOpCodes = strOpCode_DelLine & "|" & strOpCode_GetSumAmount & "|" & strOpCode_UpdTotalAmount
        Dim strParam As String
        Dim lblDelText As Label
        Dim strDNLNId As String
        Dim intErrNo As Integer

        dgLineDet.EditItemIndex = CInt(E.Item.ItemIndex)
        lblDelText = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("dnlnid")
        strDNLNId = lblDelText.Text

        Try
            strParam = strDNLNId & "|" & strSelectedDNID
            intErrNo = objBITrx.mtdDelDebitNoteLine(strOpCodes, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strAccMonth, _
                                                    strAccYear, _
                                                    strParam)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_DEBITNOTEDET_DEL_LINE&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/BI_trx_DNList.aspx")
        End Try

        onLoad_Display(strSelectedDNID)
        onLoad_DisplayLine(strSelectedDNID)
        onLoad_Button()
    End Sub

    Sub BackBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        If lblReferer.Text = "" Then
            Response.Redirect("BI_trx_DNList.aspx")
        Else
            Response.Redirect(lblReferer.Text)
        End If

    End Sub

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
                        Session("SS_USERID") & "|" & Trim(lblDebitNoteID.Text)

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

        onLoad_Display(lblDebitNoteID.Text)
        onLoad_DisplayLine(lblDebitNoteID.Text)
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

    Sub NewCNBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("BI_trx_DNDet.aspx")
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

