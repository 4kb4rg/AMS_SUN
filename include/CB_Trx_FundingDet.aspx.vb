
Imports System
Imports System.Data
Imports System.IO 
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports Microsoft.VisualBasic.Information
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic.Interaction

Imports agri.Admin
Imports agri.HR
Imports agri.GlobalHdl.clsGlobalHdl
Imports agri.PWSystem.clsLangCap

Public Class CB_Trx_FundingDet : Inherits Page

    Protected WithEvents lblCode As Label
    Protected WithEvents lblSelect As Label
    Protected WithEvents lblPleaseSelect As Label

    Protected WithEvents lblTrxID As Label
    Protected WithEvents ddlTrxType As DropDownList
    Protected WithEvents ddlAccCode As DropDownList
    Protected WithEvents txtLoanNo As TextBox
    Protected WithEvents txtDescription As TextBox
    Protected WithEvents txtOtherID As TextBox
    Protected WithEvents txtTransDate As TextBox
    Protected WithEvents txtAmount As TextBox
    Protected WithEvents txtMonthlyAmount As TextBox
    Protected WithEvents txtInstallDate As TextBox
    Protected WithEvents ddlDuration As DropDownList
    Protected WithEvents ddlPeriodic As DropDownList
    Protected WithEvents txtTotDays As TextBox
    Protected WithEvents txtRate As TextBox
    Protected WithEvents txtMaturityDate As TextBox
    Protected WithEvents txtRemark As TextBox

    Protected WithEvents lblAccPeriod As Label
    Protected WithEvents lblStatus As Label
    Protected WithEvents lblDateCreated As Label
    Protected WithEvents lblLastUpdate As Label
    Protected WithEvents lblUpdatedBy As Label
    Protected WithEvents trxid As HtmlInputHidden
    Protected WithEvents trxlnid As HtmlInputHidden
    Protected WithEvents NewBtn As ImageButton
    Protected WithEvents SaveBtn As ImageButton
    Protected WithEvents DelBtn As ImageButton
    Protected WithEvents UnDelBtn As ImageButton
    Protected WithEvents ConfirmBtn As ImageButton
    Protected WithEvents CancelBtn As ImageButton
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblHiddenSts As Label
    Protected WithEvents lblErrDup As Label
    Protected WithEvents lblErrAmount As Label
    Protected WithEvents lblErrRate As Label
    Protected WithEvents lblErrTrxType As Label
    Protected WithEvents dgLineDet As DataGrid
    Protected WithEvents lblDateTrans As Label
    Protected WithEvents lblFmtTrans As Label
    Protected WithEvents lblDateInstall As Label
    Protected WithEvents lblFmtInstall As Label
    Protected WithEvents lblDateMaturity As Label
    Protected WithEvents lblFmtMaturity As Label
    Protected WithEvents lblErrMonthlyAmount As Label
    Protected WithEvents lblErrTotDays As Label
    Protected WithEvents lblErrAccCode As Label
    Protected WithEvents lblErrLoanNo As Label

    Protected WithEvents lblAmount As TextBox
    Protected WithEvents lblMonthlyAmount As TextBox
    Protected WithEvents hidAccCode As HtmlInputHidden
    Protected WithEvents hidStatEdit As HtmlInputHidden
    Protected WithEvents hidStatEditLn As HtmlInputHidden
    Protected WithEvents hidRowID As HtmlInputHidden
    Protected WithEvents hidDescr As HtmlInputHidden

    Dim objGLSetup As New agri.GL.clsSetup()
    Dim objGLTrx As New agri.GL.ClsTrx()
    Dim objAdmin As New agri.Admin.clsUom()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objSys As New agri.PWSystem.clsConfig()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Protected objCBTrx As New agri.CB.clsTrx()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objHRSetup As New agri.HR.clsSetup()

    Dim objTaxDs As New Object()
    Dim objActDs As New Object()
    Dim objUOMDs As New Object()
    Dim objConfigDs As New Object()
    Dim objLangCapDs As New Object()

    Dim intConfigsetting As Integer
    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim intCBAR As Integer
    Dim strTrxID As String = ""
    Dim intStatus As Integer
    Dim intMaxLen As Integer = 0
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim strAccountTag As String
    Dim strLocType As String
    Dim strAcceptDateFormat As String
    Dim strSelAccMonth As String
    Dim strSelAccYear As String
    Dim intLevel As Integer
    

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_GLACCMONTH")
        strAccYear = Session("SS_GLACCYEAR")
        intCBAR = Session("SS_CBAR")
        strLocType = Session("SS_LOCTYPE")
        intConfigsetting = Session("SS_CONFIGSETTING")
        intLevel = Session("SS_USRLEVEL")
        strSelAccMonth = Session("SS_SELACCMONTH")
        strSelAccYear = Session("SS_SELACCYEAR")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCBAccessRights.CBPayment), intCBAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            onload_GetLangCap()
            strTrxID = Trim(IIf(Request.QueryString("trxid") <> "", Request.QueryString("trxid"), Request.Form("trxid")))

            'to avoid double click, on aspx add this : UseSubmitBehavior="false"
            SaveBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(SaveBtn).ToString())
            ConfirmBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(ConfirmBtn).ToString())
            DelBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(DelBtn).ToString())
            UnDelBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(UnDelBtn).ToString())
            CancelBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(CancelBtn).ToString())

            intStatus = CInt(lblHiddenSts.Text)
            lblErrAmount.Visible = False
            lblErrRate.Visible = False
            lblErrTrxType.Visible = False
            lblDateTrans.Visible = False
            lblFmtTrans.Visible = False
            lblDateInstall.Visible = False
            lblFmtInstall.Visible = False
            lblDateMaturity.Visible = False
            lblFmtMaturity.Visible = False
            lblErrAccCode.Visible = False
            lblErrLoanNo.Visible = False
            lblErrMonthlyAmount.Visible = False
            lblErrTotDays.Visible = False

            If Not IsPostBack Then
                txtTransDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
                txtInstallDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
                txtMaturityDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
                hidRowID.Value = 0
                BindAccCode("")

                If strTrxID <> "" Then
                    trxid.Value = strTrxID
                    onLoad_Display(trxid.Value)
                    onLoad_DisplayLn(trxid.Value)
                Else
                    onLoad_BindButton()
                End If
            End If

        End If
    End Sub

    Sub BindAccCode(ByVal pv_strAccCode As String)
        Dim strOpCd_Account As String = "GL_CLSSETUP_ACCOUNTCODE_LIST_GET"
        Dim strParam As String = ""
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim dr As DataRow
        Dim intAccountIndex As Integer = 0
        Dim objAccountDs As New Object()

        strParam = "Order By ACC.AccCode|And ACC.Status = '" & objGLSetup.EnumAccountCodeStatus.Active & "' " & _
                       " And ACC.AccType = '" & objGLSetup.EnumAccountType.BalanceSheet & "' " & _
                       " And ACC.AccGrpCode = '34' "
        strParam = strParam & IIf(Session("SS_COACENTRALIZED") = "1", "", " AND LocCode = '" & Trim(strLocation) & "' ")

        Try
            intErrNo = objGLSetup.mtdGetMasterList(strOpCd_Account, strParam, objGLSetup.EnumGLMasterType.AccountCode, objAccountDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PU_ONLOAD_ACCOUNTLIST&errmesg=" & lblErrMessage.Text & "&redirect=pu/setup/PU_setup_SuppList.aspx")
        End Try

        If objAccountDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objAccountDs.Tables(0).Rows.Count - 1
                If objAccountDs.Tables(0).Rows(intCnt).Item("AccCode") = Trim(pv_strAccCode) Then
                    intAccountIndex = intCnt + 1
                End If
            Next intCnt
        End If

        dr = objAccountDs.Tables(0).NewRow()
        dr("AccCode") = ""
        dr("_Description") = lblPleaseSelect.Text & " Account" & lblCode.Text
        objAccountDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlAccCode.DataSource = objAccountDs.Tables(0)
        ddlAccCode.DataTextField = "_Description"
        ddlAccCode.DataValueField = "AccCode"
        ddlAccCode.DataBind()
        ddlAccCode.SelectedIndex = intAccountIndex
    End Sub

    Sub onLoad_Display(ByVal pv_strTrxID As String)
        Dim strOpCd As String = "CB_CLSTRX_FUNDING_LIST_GET"
        Dim intErrNo As Integer
        Dim strParamName As String
        Dim strParamValue As String

        strParamName = "STRSEARCH"
        strParamValue = " AND A.TrxID = '" & Trim(pv_strTrxID) & "' "

        Try
            intErrNo = objGLTrx.mtdGetDataCommon(strOpCd, _
                                                strParamName, _
                                                strParamValue, _
                                                objTaxDs)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=TX_Setup_TaxObjectDet&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        lblTrxID.Text = Trim(objTaxDs.Tables(0).Rows(0).Item("TrxID"))
        txtDescription.Text = Trim(objTaxDs.Tables(0).Rows(0).Item("Description"))
        ddlTrxType.SelectedIndex = CInt(objTaxDs.Tables(0).Rows(0).Item("TrxType"))
        txtTransDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), objTaxDs.Tables(0).Rows(0).Item("TrxDate"))

        txtAmount.Text = objTaxDs.Tables(0).Rows(0).Item("Amount")
        txtMonthlyAmount.Text = 0 'objTaxDs.Tables(0).Rows(0).Item("M_Amount")
        txtRate.Text = 0 'objTaxDs.Tables(0).Rows(0).Item("Rate")
        ddlDuration.SelectedIndex = CInt(objTaxDs.Tables(0).Rows(0).Item("Duration")) - 1
        ddlPeriodic.SelectedIndex = CInt(objTaxDs.Tables(0).Rows(0).Item("Periodic"))
        txtTotDays.Text = objTaxDs.Tables(0).Rows(0).Item("TotDays")
        txtInstallDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), objTaxDs.Tables(0).Rows(0).Item("InstallDate"))
        txtMaturityDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), objTaxDs.Tables(0).Rows(0).Item("MaturityDate"))
        txtRemark.Text = Trim(objTaxDs.Tables(0).Rows(0).Item("Remark"))
        hidAccCode.Value = objTaxDs.Tables(0).Rows(0).Item("AccCode").Trim()
        BindAccCode(hidAccCode.Value)
        hidDescr.Value = Trim(objTaxDs.Tables(0).Rows(0).Item("AccDescr"))

        lblAmount.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(txtAmount.Text, 2), 2)
        lblMonthlyAmount.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(txtMonthlyAmount.Text, 2), 2)

        intStatus = CInt(Trim(objTaxDs.Tables(0).Rows(0).Item("Status")))
        lblHiddenSts.Text = Trim(objTaxDs.Tables(0).Rows(0).Item("Status"))
        lblAccPeriod.Text = Trim(Trim(objTaxDs.Tables(0).Rows(0).Item("AccMonth"))) & "/" & Trim(Trim(objTaxDs.Tables(0).Rows(0).Item("AccYear")))
        lblStatus.Text = objGLTrx.mtdGetLeaseFinanceStatus(Trim(objTaxDs.Tables(0).Rows(0).Item("Status")))
        lblDateCreated.Text = objGlobal.GetLongDate(objTaxDs.Tables(0).Rows(0).Item("CreateDate"))
        lblLastUpdate.Text = objGlobal.GetLongDate(objTaxDs.Tables(0).Rows(0).Item("UpdateDate"))
        lblUpdatedBy.Text = Trim(objTaxDs.Tables(0).Rows(0).Item("UserName"))
        hidStatEdit.Value = Trim(objTaxDs.Tables(0).Rows(0).Item("StatusEdit"))

        onLoad_BindButton()
    End Sub

    Sub onLoad_DisplayLn(ByVal pv_strTrxID As String)
        Dim strOpCd As String = "CB_CLSTRX_FUNDING_LINE_GET"
        Dim intErrNo As Integer
        Dim strParamName As String
        Dim strParamValue As String
        Dim intCnt As Integer = 0
        Dim lbl As Label
        Dim lb As LinkButton

        strParamName = "STRSEARCH"
        strParamValue = " AND A.TrxID = '" & Trim(pv_strTrxID) & "' "

        Try
            intErrNo = objGLTrx.mtdGetDataCommon(strOpCd, _
                                                strParamName, _
                                                strParamValue, _
                                                objTaxDs)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=TX_Setup_TaxObjectDet&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        If objTaxDs.Tables(0).Rows.Count > 0 Then
            dgLineDet.DataSource = objTaxDs.Tables(0)
            dgLineDet.DataBind()

            For intCnt = 0 To objTaxDs.Tables(0).Rows.Count - 1
                lbl = dgLineDet.Items.Item(intCnt).FindControl("lblRowID")
                If lbl.Text = "99999" Then
                    lbl.Visible = True
                    lbl.Text = "TOTAL"
                    lbl.Font.Bold = True
                    lbl = dgLineDet.Items.Item(intCnt).FindControl("lblM_Amount")
                    lbl.Font.Bold = True
                    lbl = dgLineDet.Items.Item(intCnt).FindControl("lblM_Rate")
                    lbl.Visible = False
                    lbl = dgLineDet.Items.Item(intCnt).FindControl("lblM_BalanceAmount")
                    lbl.Visible = False
                    lbl = dgLineDet.Items.Item(intCnt).FindControl("lblM_Accrued")
                    lbl.Visible = False
                    lbl = dgLineDet.Items.Item(intCnt).FindControl("lblM_Interest")
                    lbl.Visible = False
                    lbl = dgLineDet.Items.Item(intCnt).FindControl("lblTotDays")
                    lbl.Visible = False
                    lb = dgLineDet.Items.Item(intCnt).FindControl("lbEdit")
                    lb.Visible = False
                    dgLineDet.Items.Item(intCnt).BackColor = Drawing.Color.lightblue
                Else
                    lbl = dgLineDet.Items.Item(intCnt).FindControl("lblPayDate")
                    If (Year(lbl.Text) * 100) + Month(lbl.Text) = (strSelAccYear * 100) + strSelAccMonth Then
                        dgLineDet.Items.Item(intCnt).BackColor = Drawing.Color.LightGray
                    End If
                End If
            Next
        End If
    End Sub

    Sub onLoad_BindButton()
        txtDescription.Enabled = False
        ddlTrxType.Enabled = False
        txtTransDate.Enabled = False
        NewBtn.Visible = True
        SaveBtn.Visible = False
        DelBtn.Visible = False
        UnDelBtn.Visible = False
        ConfirmBtn.Visible = False
        CancelBtn.Visible = False

        Select Case intStatus
            Case objGLTrx.EnumLeaseFinancingStatus.Active
                txtDescription.Enabled = True
                ddlTrxType.Enabled = False
                txtTransDate.Enabled = True
                SaveBtn.Visible = True
                DelBtn.Visible = True
                DelBtn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                ConfirmBtn.Visible = True

                If hidStatEditLn.Value = "" Or hidStatEditLn.Value = "0" Then 'belum ada edit
                    SaveBtn.Attributes("onclick") = "javascript:return ConfirmAction('save and re/calculate this loan');"
                End If

            Case objGLTrx.EnumLeaseFinancingStatus.Deleted
                UnDelBtn.Visible = True
                txtDescription.Enabled = False
                ddlTrxType.Enabled = False
                txtTransDate.Enabled = False
                txtAmount.ReadOnly = True
                txtRate.ReadOnly = True
                ddlDuration.Enabled = False
                txtInstallDate.Enabled = False
                txtMaturityDate.Enabled = False
                txtRemark.Enabled = False

            Case objGLTrx.EnumLeaseFinancingStatus.Confirmed
                CancelBtn.Visible = True
                txtDescription.Enabled = False
                ddlTrxType.Enabled = False
                txtTransDate.Enabled = False
                txtAmount.ReadOnly = True
                txtRate.ReadOnly = True
                ddlDuration.Enabled = False
                txtInstallDate.Enabled = False
                txtMaturityDate.Enabled = False
                txtRemark.Enabled = False

            Case objGLTrx.EnumLeaseFinancingStatus.Cancelled
                CancelBtn.Visible = True
                txtDescription.Enabled = False
                ddlTrxType.Enabled = False
                txtTransDate.Enabled = False
                txtAmount.ReadOnly = True
                txtRate.ReadOnly = True
                ddlDuration.Enabled = False
                txtInstallDate.Enabled = False
                txtMaturityDate.Enabled = False
                txtRemark.Enabled = False

            Case Else
                txtTransDate.Enabled = True
                txtDescription.Enabled = True
                ddlTrxType.Enabled = True
                SaveBtn.Visible = True
        End Select
    End Sub

    Sub Button_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCd_Upd As String = "CB_CLSTRX_FUNDING_UPDATE"
        Dim strCmdArgs As String = CType(Sender, ImageButton).CommandArgument
        Dim intErrNo As Integer
        Dim blnIsUpdate As Boolean
        Dim strParamName As String
        Dim strParamValue As String
        Dim strDate As String = Date_Validation(txtTransDate.Text, False)
        Dim strDateInstall As String = Date_Validation(txtInstallDate.Text, False)
        Dim strDateMaturity As String = Date_Validation(txtMaturityDate.Text, False)
        Dim strAccCode As String = Request.Form("ddlAccCode")
        Dim indDate As String = ""

        If CheckDate(txtTransDate.Text.Trim(), indDate) = False Then
            lblDateTrans.Visible = True
            lblFmtTrans.Visible = True
            lblDateTrans.Text = "<br>Date Entered should be in the format"
            Exit Sub
        End If

        'Dim intInputPeriod As Integer = Year(strDate) * 100 + Month(strDate)
        'Dim intCurPeriod As Integer = (CInt(strAccYear) * 100) + CInt(strAccMonth)
        'Dim intSelPeriod As Integer = (CInt(strSelAccYear) * 100) + CInt(strSelAccMonth)

        'If Session("SS_FILTERPERIOD") = "0" Then
        '    If intCurPeriod < intInputPeriod Then
        '        lblDateTrans.Visible = True
        '        lblDateTrans.Text = "Invalid transaction date."
        '        Exit Sub
        '    End If
        'Else
        '    If intSelPeriod <> intInputPeriod Then
        '        lblDateTrans.Visible = True
        '        lblDateTrans.Text = "Invalid transaction date."
        '        Exit Sub
        '    End If
        '    If intSelPeriod < intCurPeriod And intLevel < 2 Then
        '        lblDateTrans.Visible = True
        '        lblDateTrans.Text = "This period already locked."
        '        Exit Sub
        '    End If
        'End If

        If ddlTrxType.SelectedItem.Value = "0" Then
            lblErrTrxType.Visible = True
            Exit Sub
        End If

        If strAccCode = "" Then
            lblErrAccCode.Visible = True
            Exit Sub
        End If

        strAccYear = Year(strDate)
        strAccMonth = Month(strDate)

        If strCmdArgs = "Save" Then
            trxid.Value = strTrxID

            If txtMonthlyAmount.Text = "" Then
                lblErrMonthlyAmount.Visible = True
                Exit Sub
            End If
            If txtTotDays.Text = "" Then
                lblErrTotDays.Visible = True
                Exit Sub
            End If
            If CDbl(txtRate.Text) = 0 Or txtRate.Text = "" Then
                lblErrRate.Visible = True
                Exit Sub
            End If

            strParamName = "TRXID|TRXTYPE|TRXDATE|ACCCODE|LOANNO|DESCRIPTION|OTHERID|AMOUNT|M_AMOUNT|INSTALLDATE|MATURITYDATE|RATE|DURATION|PERIODIC|TOTDAYS|" & _
                            "ACCMONTH|ACCYEAR|LOCCODE|REMARK|STATUS|UPDATEID|STATEDIT|STATEDITLN|ROWID"
            strParamValue = strTrxID & "|" & _
                            ddlTrxType.SelectedItem.Value & "|" & _
                            strDate & "|" & _
                            Trim(strAccCode) & "|" & _
                            Trim(txtLoanNo.Text) & "|" & _
                            Trim(txtDescription.Text) & "|" & _
                            Trim(txtOtherID.Text) & "|" & _
                            txtAmount.Text & "|" & _
                            txtMonthlyAmount.Text & "|" & _
                            strDateInstall & "|" & _
                            strDateMaturity & "|" & _
                            txtRate.Text & "|" & _
                            ddlDuration.SelectedItem.Value & "|" & _
                            ddlPeriodic.SelectedItem.Value & "|" & _
                            txtTotDays.Text & "|" & _
                            strAccMonth & "|" & _
                            strAccYear & "|" & _
                            strLocation & "|" & _
                            Trim(txtRemark.Text) & "|" & _
                            objGLTrx.EnumFundingStatus.Active & "|" & _
                            Trim(strUserId) & "|" & _
                            hidStatEdit.Value & "|" & _
                            hidStatEditLn.Value & "|" & _
                            IIf(hidRowID.Value = "", 0, hidRowID.Value)

        ElseIf strCmdArgs = "Del" Then
            strParamName = "TRXID|TRXTYPE|TRXDATE|ACCCODE|LOANNO|DESCRIPTION|OTHERID|AMOUNT|M_AMOUNT|INSTALLDATE|MATURITYDATE|RATE|DURATION|PERIODIC|TOTDAYS|" & _
                            "ACCMONTH|ACCYEAR|LOCCODE|REMARK|STATUS|UPDATEID|STATEDIT|STATEDITLN|ROWID"
            strParamValue = strTrxID & "|" & _
                            ddlTrxType.SelectedItem.Value & "|" & _
                            strDate & "|" & _
                            Trim(ddlAccCode.SelectedItem.Value) & "|" & _
                            Trim(txtLoanNo.Text) & "|" & _
                            Trim(txtDescription.Text) & "|" & _
                            Trim(txtOtherID.Text) & "|" & _
                            txtAmount.Text & "|" & _
                            txtMonthlyAmount.Text & "|" & _
                            strDateInstall & "|" & _
                            strDateMaturity & "|" & _
                            txtRate.Text & "|" & _
                            ddlDuration.SelectedItem.Value & "|" & _
                            ddlPeriodic.SelectedItem.Value & "|" & _
                            txtTotDays.Text & "|" & _
                            strAccMonth & "|" & _
                            strAccYear & "|" & _
                            strLocation & "|" & _
                            Trim(txtRemark.Text) & "|" & _
                            objGLTrx.EnumFundingStatus.Deleted & "|" & _
                            Trim(strUserId) & "|" & _
                            hidStatEdit.Value & "|" & _
                            hidStatEditLn.Value & "|" & _
                            IIf(hidRowID.Value = "", 0, hidRowID.Value)

        ElseIf strCmdArgs = "UnDel" Then
            strParamName = "TRXID|TRXTYPE|TRXDATE|ACCCODE|LOANNO|DESCRIPTION|OTHERID|AMOUNT|M_AMOUNT|INSTALLDATE|MATURITYDATE|RATE|DURATION|PERIODIC|TOTDAYS|" & _
                            "ACCMONTH|ACCYEAR|LOCCODE|REMARK|STATUS|UPDATEID|STATEDIT|STATEDITLN|ROWID"
            strParamValue = strTrxID & "|" & _
                            ddlTrxType.SelectedItem.Value & "|" & _
                            strDate & "|" & _
                            Trim(ddlAccCode.SelectedItem.Value) & "|" & _
                            Trim(txtLoanNo.Text) & "|" & _
                            Trim(txtDescription.Text) & "|" & _
                            Trim(txtOtherID.Text) & "|" & _
                            txtAmount.Text & "|" & _
                            txtMonthlyAmount.Text & "|" & _
                            strDateInstall & "|" & _
                            strDateMaturity & "|" & _
                            txtRate.Text & "|" & _
                            ddlDuration.SelectedItem.Value & "|" & _
                            ddlPeriodic.SelectedItem.Value & "|" & _
                            txtTotDays.Text & "|" & _
                            strAccMonth & "|" & _
                            strAccYear & "|" & _
                            strLocation & "|" & _
                            Trim(txtRemark.Text) & "|" & _
                            objGLTrx.EnumFundingStatus.Active & "|" & _
                            Trim(strUserId) & "|" & _
                            hidStatEdit.Value & "|" & _
                            hidStatEditLn.Value & "|" & _
                            IIf(hidRowID.Value = "", 0, hidRowID.Value)

        ElseIf strCmdArgs = "Confirm" Then
            trxid.Value = strTrxID

            strParamName = "TRXID|TRXTYPE|TRXDATE|ACCCODE|LOANNO|DESCRIPTION|OTHERID|AMOUNT|M_AMOUNT|INSTALLDATE|MATURITYDATE|RATE|DURATION|PERIODIC|TOTDAYS|" & _
                            "ACCMONTH|ACCYEAR|LOCCODE|REMARK|STATUS|UPDATEID|STATEDIT|STATEDITLN|ROWID"
            strParamValue = strTrxID & "|" & _
                            ddlTrxType.SelectedItem.Value & "|" & _
                            strDate & "|" & _
                            Trim(ddlAccCode.SelectedItem.Value) & "|" & _
                            Trim(txtLoanNo.Text) & "|" & _
                            Trim(txtDescription.Text) & "|" & _
                            Trim(txtOtherID.Text) & "|" & _
                            txtAmount.Text & "|" & _
                            txtMonthlyAmount.Text & "|" & _
                            strDateInstall & "|" & _
                            strDateMaturity & "|" & _
                            txtRate.Text & "|" & _
                            ddlDuration.SelectedItem.Value & "|" & _
                            ddlPeriodic.SelectedItem.Value & "|" & _
                            txtTotDays.Text & "|" & _
                            strAccMonth & "|" & _
                            strAccYear & "|" & _
                            strLocation & "|" & _
                            Trim(txtRemark.Text) & "|" & _
                            objGLTrx.EnumFundingStatus.Confirmed & "|" & _
                            Trim(strUserId) & "|" & _
                            hidStatEdit.Value & "|" & _
                            hidStatEditLn.Value & "|" & _
                            IIf(hidRowID.Value = "", 0, hidRowID.Value)

        ElseIf strCmdArgs = "Cancel" Then
            trxid.Value = strTrxID

            strParamName = "TRXID|TRXTYPE|TRXDATE|ACCCODE|LOANNO|DESCRIPTION|OTHERID|AMOUNT|M_AMOUNT|INSTALLDATE|MATURITYDATE|RATE|DURATION|PERIODIC|TOTDAYS|" & _
                            "ACCMONTH|ACCYEAR|LOCCODE|REMARK|STATUS|UPDATEID|STATEDIT|STATEDITLN|ROWID"
            strParamValue = strTrxID & "|" & _
                            ddlTrxType.SelectedItem.Value & "|" & _
                            strDate & "|" & _
                            Trim(ddlAccCode.SelectedItem.Value) & "|" & _
                            Trim(txtLoanNo.Text) & "|" & _
                            Trim(txtDescription.Text) & "|" & _
                            Trim(txtOtherID.Text) & "|" & _
                            txtAmount.Text & "|" & _
                            txtMonthlyAmount.Text & "|" & _
                            strDateInstall & "|" & _
                            strDateMaturity & "|" & _
                            txtRate.Text & "|" & _
                            ddlDuration.SelectedItem.Value & "|" & _
                            ddlPeriodic.SelectedItem.Value & "|" & _
                            txtTotDays.Text & "|" & _
                            strAccMonth & "|" & _
                            strAccYear & "|" & _
                            strLocation & "|" & _
                            Trim(txtRemark.Text) & "|" & _
                            objGLTrx.EnumFundingStatus.Cancelled & "|" & _
                            Trim(strUserId) & "|" & _
                            hidStatEdit.Value & "|" & _
                            hidStatEditLn.Value & "|" & _
                            IIf(hidRowID.Value = "", 0, hidRowID.Value)
        End If

        Try
            intErrNo = objGLTrx.mtdGetDataCommon(strOpCd_Upd, strParamName, strParamValue, objTaxDs)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=TX_Setup_TaxObjectDet&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        If objTaxDs.Tables(0).Rows.Count > 0 Then
            trxid.Value = Trim(objTaxDs.Tables(0).Rows(0).Item("TrxID"))
        End If
        If trxid.Value <> "" Then
            onLoad_Display(trxid.Value)
            onLoad_DisplayLn(trxid.Value)
        End If
    End Sub

    Sub BackBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CB_Trx_FundingList.aspx")
    End Sub

    Sub NewBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CB_Trx_FundingDet.aspx")
    End Sub

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

    Sub onload_GetLangCap()
        GetEntireLangCap()

        strAccountTag = GetCaption(objLangCap.EnumLangCap.Account) & lblCode.Text
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GL_TRX_JOURNAL_DETAIL_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=FA/FA_trx_CashBankDet.aspx")
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
                lblFmtTrans.Text = strDateFormat
                lblFmtTrans.Text = strDateFormat
                pr_strDate = ""
                CheckDate = False
            End If
        End If
    End Function

    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim lbl As Label
        
        hidStatEditLn.Value = "1"
        dgLineDet.EditItemIndex = CInt(E.Item.ItemIndex)

        lbl = E.Item.FindControl("lblRowID")
        hidRowID.Value = lbl.Text.Trim
        lbl = E.Item.FindControl("lblIDM_Amount")
        txtMonthlyAmount.Text = lbl.Text.Trim
        lblMonthlyAmount.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(txtMonthlyAmount.Text, 2), 2)
        lbl = E.Item.FindControl("lblIDM_Interest")
        txtRate.Text = lbl.Text.Trim
        lbl = E.Item.FindControl("lblIDPayDate")
        txtInstallDate.Text = Date_Validation(lbl.Text.Trim, True)
        lbl = E.Item.FindControl("lblIDM_Periodic")
        ddlPeriodic.SelectedValue = lbl.Text.Trim

        DelBtn.Visible = False
        ConfirmBtn.Visible = False
    End Sub

    Sub DownloadBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim intCnt As Integer
        Dim lb As LinkButton

        Dim TrxID As String = Trim(lblTrxID.Text)

        For intCnt = 0 To dgLineDet.Items.Count - 1
            lb = dgLineDet.Items.Item(intCnt).FindControl("lbEdit")
            lb.Visible = False
        Next

        Response.Clear()
        Response.AddHeader("content-disposition", "attachment;filename=" & UCase(Trim(hidDescr.Value)) & "-" & Trim(TrxID) & ".xls")
        Response.Charset = ""
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.ContentType = "application/vnd.xls"

        Dim stringWrite = New System.IO.StringWriter()
        Dim htmlWrite = New HtmlTextWriter(stringWrite)

        dgLineDet.RenderControl(htmlWrite)
        Response.Write(stringWrite.ToString())
        Response.End()
    End Sub
End Class
