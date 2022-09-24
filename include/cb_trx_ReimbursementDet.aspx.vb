
Imports System
Imports System.Data


Public Class cb_trx_ReimbursementDet : Inherits Page

    Protected WithEvents lblErrMesage As Label
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents dgDataList As DataGrid
    Protected WithEvents lblTracker As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents lblPaymentID As Label
    Protected WithEvents lblAccPeriod As Label
    Protected WithEvents lblStatus As Label
    Protected WithEvents lblLastUpdate As Label
    Protected WithEvents lblChequePrintDate As Label
    Protected WithEvents lblPrintDate As Label
    Protected WithEvents lblDateCreated As Label
    Protected WithEvents lblUpdatedBy As Label

    Protected WithEvents txtDateCreated As TextBox
    Protected WithEvents txtGiroDate As TextBox
    Protected WithEvents ddlBank As DropDownList
    Protected WithEvents ddlPayType As DropDownList
    Protected WithEvents txtChequeNo As TextBox
    Protected WithEvents txtRemark As TextBox
    Protected WithEvents txtAmount As TextBox
    Protected WithEvents NewBtn As ImageButton
    Protected WithEvents SaveBtn As ImageButton
    Protected WithEvents DeleteBtn As ImageButton
    Protected WithEvents ConfirmBtn As ImageButton
    Protected WithEvents CancelBtn As ImageButton
    Protected WithEvents hidInit As HtmlInputHidden
    Protected WithEvents hidStatus As HtmlInputHidden

    Protected WithEvents lblDate As Label
    Protected WithEvents lblFmt As Label
    Protected WithEvents lblDateGiro As Label
    Protected WithEvents lblFmtGiro As Label
    Protected WithEvents lblErrPayType As Label

    Protected objCBTrx As New agri.CB.clsTrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim objGLtrx As New agri.GL.ClsTrx()
    Dim objHRSetup As New agri.HR.clsSetup()
    Dim objSysCfg As New agri.PWSystem.clsConfig()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intCBAR As Integer
    Dim objCashBankDs As New Object()
    Dim strLocType As String
    Dim intLevel As Integer
    Dim strSelAccMonth As String
    Dim strSelAccYear As String
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
        strLocType = Session("SS_LOCTYPE")
        intLevel = Session("SS_USRLEVEL")
        strSelAccMonth = Session("SS_SELACCMONTH")
        strSelAccYear = Session("SS_SELACCYEAR")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCBAccessRights.CBCashBank), intCBAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            If SortExpression.Text = "" Then
                SortExpression.Text = "LEFT(CB.CASHBANKID,3)+RIGHT(RTRIM(CB.CASHBANKID),4)"
            End If

            'to avoid double click, on aspx add this : UseSubmitBehavior="false"
            SaveBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(SaveBtn).ToString())
            ConfirmBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(ConfirmBtn).ToString())

            lblErrMesage.Visible = False
            lblErrPayType.Visible = False

            If Not Page.IsPostBack Then
                txtDateCreated.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
                txtGiroDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
                lblPaymentID.Text = Request.QueryString("cbid")

                BindBankCode("")
                BindGrid()
                BindPageList()
            End If

            If intLevel > 2 Then
                If lblPaymentID.Text = "" Then
                    ConfirmBtn.Visible = False
                    DeleteBtn.Visible = False
                    DeleteBtn.Attributes("onclick") = "javascript:return ConfirmAction('delete this reimbursement');"
                Else
                    If CInt(hidStatus.Value) = objCBTrx.EnumCashBankStatus.Active Then
                        ConfirmBtn.Visible = True
                        ConfirmBtn.Attributes("onclick") = "javascript:return ConfirmAction('confirm this reimbursement');"
                    End If
                End If
            Else
                ConfirmBtn.Visible = False
            End If
        End If
    End Sub

    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        dgDataList.CurrentPageIndex = 0
        dgDataList.EditItemIndex = -1
        BindGrid()
        BindPageList()
    End Sub

    Sub BindGrid()
        Dim strOpCode As String
        Dim PageNo As Integer
        Dim PageCount As Integer
        Dim lbButton As LinkButton
        Dim strSrchID As String
        Dim strSrchStatus As String
        Dim strSrchType As String = ""
        Dim strSearch As String = ""
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim lbl As Label
        Dim strChequeNo As String
        Dim lblBalance As Label
        Dim strParamName As String
        Dim strParamValue As String

        strAccMonth = strSelAccMonth
        strAccYear = strSelAccYear

        If lblPaymentID.Text <> "" Then
            hidInit.Value = "1"
            strOpCode = "CB_CLSTRX_REIMBURSEMENT_GET_DETAIL"

            strParamName = "LOCCODE|ACCMONTH|ACCYEAR|UPDATEID|PAYMENTID"
            strParamValue = strLocation & "|" & strAccMonth & "|" & strAccYear & "|" & strUserId & "|" & lblPaymentID.Text
        Else
            strOpCode = "CB_CLSTRX_REIMBURSEMENT_LIST_GET"

            strParamName = "LOCCODE|ACCMONTH|ACCYEAR|UPDATEID|STRINIT"
            strParamValue = strLocation & "|" & strAccMonth & "|" & strAccYear & "|" & strUserId & "|" & hidInit.Value
        End If

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
                                                strParamName, _
                                                strParamValue, _
                                                objCashBankDs)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=REIMBURSEMENT_GET_LIST&errmesg=" & lblErrMesage.Text & "&redirect=CB/trx/CB_trx_CashBanklist.aspx")
        End Try

        If lblPaymentID.Text <> "" Then
            PageCount = objGlobal.mtdGetPageCount(objCashBankDs.Tables(1).Rows.Count, dgDataList.PageSize)
            dgDataList.DataSource = objCashBankDs.Tables(1)
            If dgDataList.CurrentPageIndex >= PageCount Then
                If PageCount = 0 Then
                    dgDataList.CurrentPageIndex = 0
                Else
                    dgDataList.CurrentPageIndex = PageCount - 1
                End If
            End If
        Else
            PageCount = objGlobal.mtdGetPageCount(objCashBankDs.Tables(0).Rows.Count, dgDataList.PageSize)
            dgDataList.DataSource = objCashBankDs
            If dgDataList.CurrentPageIndex >= PageCount Then
                If PageCount = 0 Then
                    dgDataList.CurrentPageIndex = 0
                Else
                    dgDataList.CurrentPageIndex = PageCount - 1
                End If
            End If
        End If

        dgDataList.DataBind()
        BindPageList()

        If objCashBankDs.Tables(0).Rows.Count > 0 Then
            If lblPaymentID.Text <> "" Then
                lblPaymentID.Text = objCashBankDs.Tables(0).Rows(0).Item("paymentid")
                txtDateCreated.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), objCashBankDs.Tables(0).Rows(0).Item("PaymentDate"))            
                BindBankCode(objCashBankDs.Tables(0).Rows(0).Item("BankCode"))
                txtChequeNo.Text = Trim(objCashBankDs.Tables(0).Rows(0).Item("ChequeNo"))
                txtGiroDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), objCashBankDs.Tables(0).Rows(0).Item("ChequePrintDate"))
                txtRemark.Text = Trim(objCashBankDs.Tables(0).Rows(0).Item("Remark"))
                ddlPayType.SelectedValue = Trim(objCashBankDs.Tables(0).Rows(0).Item("PaymentType"))
                hidStatus.Value = Trim(objCashBankDs.Tables(0).Rows(0).Item("Status"))
                lblStatus.Text = objCBTrx.mtdGetCashBankStatus(Trim(objCashBankDs.Tables(0).Rows(0).Item("Status")))
                lblChequePrintDate.Text = objGlobal.GetLongDate(objCashBankDs.Tables(0).Rows(0).Item("ChequePrintDate"))
                lblDateCreated.Text = objGlobal.GetLongDate(Trim(objCashBankDs.Tables(0).Rows(0).Item("CreateDate")))
                lblLastUpdate.Text = objGlobal.GetLongDate(Trim(objCashBankDs.Tables(0).Rows(0).Item("UpdateDate")))
                lblUpdatedBy.Text = Trim(objCashBankDs.Tables(0).Rows(0).Item("UserName"))
                lblAccPeriod.Text = Trim(objCashBankDs.Tables(0).Rows(0).Item("AccYear")) & Trim(objCashBankDs.Tables(0).Rows(0).Item("AccMonth"))
                txtAmount.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(objCashBankDs.Tables(0).Rows(0).Item("TotalAmount"), CInt(Session("SS_ROUNDNO"))), CInt(Session("SS_ROUNDNO")))
            Else
                txtAmount.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(objCashBankDs.Tables(0).Rows(0).Item("TotalAmount"), CInt(Session("SS_ROUNDNO"))), CInt(Session("SS_ROUNDNO")))
            End If
        End If

        For intCnt = 0 To dgDataList.Items.Count - 1
            lbl = dgDataList.Items.Item(intCnt).FindControl("lblStatus")

            Select Case CInt(Trim(lbl.Text))
                Case objCBTrx.EnumCashBankStatus.Active, objCBTrx.EnumCashBankStatus.Verified
                    lbButton = dgDataList.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = True
                    lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"

                Case objCBTrx.EnumCashBankStatus.Confirmed, _
                     objCBTrx.EnumCashBankStatus.Deleted, _
                     objCBTrx.EnumCashBankStatus.Closed, _
                     objCBTrx.EnumCashBankStatus.Cancelled
                    lbButton = dgDataList.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False

            End Select
            lblBalance = dgDataList.Items.Item(intCnt).FindControl("lblBalance")
            If lblBalance.Text > 0 Then
                dgDataList.Items(intCnt).ForeColor = Drawing.Color.Red
            End If
        Next

        'PageNo = dgDataList.CurrentPageIndex + 1
        'lblTracker.Text = "Page " & PageNo & " of " & dgDataList.PageCount
    End Sub

    Sub BindPageList()
        Dim count As Integer = 1
        Dim arrDList As New ArrayList()

        While Not count = dgDataList.PageCount + 1
            arrDList.Add("Page " & count)
            count = count + 1
        End While
        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = dgDataList.CurrentPageIndex
        
        Select Case CInt(hidStatus.Value)
            Case objCBTrx.EnumCashBankStatus.Active
                SaveBtn.Visible = True
                CancelBtn.Visible = False
                DeleteBtn.Visible = True
                ConfirmBtn.Visible = True
            Case objCBTrx.EnumCashBankStatus.Confirmed
                SaveBtn.Visible = False
                CancelBtn.Visible = True
                DeleteBtn.Visible = False
                ConfirmBtn.Visible = False

                ddlPayType.Enabled = False
                ddlBank.Enabled = False
                txtChequeNo.Enabled = False
                txtGiroDate.Enabled = False
                txtRemark.Enabled = False
            Case Else
                SaveBtn.Visible = False
                ConfirmBtn.Visible = False
                CancelBtn.Visible = False
                DeleteBtn.Visible = False

                ddlPayType.Enabled = False
                ddlBank.Enabled = False
                txtChequeNo.Enabled = False
                txtGiroDate.Enabled = False
                txtRemark.Enabled = False
        End Select
    End Sub

    Sub btnPrevNext_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).CommandArgument
        Select Case direction
            Case "first"
                dgDataList.CurrentPageIndex = 0
            Case "prev"
                dgDataList.CurrentPageIndex = _
                Math.Max(0, dgDataList.CurrentPageIndex - 1)
            Case "next"
                dgDataList.CurrentPageIndex = _
                Math.Min(dgDataList.PageCount - 1, dgDataList.CurrentPageIndex + 1)
            Case "last"
                dgDataList.CurrentPageIndex = dgDataList.PageCount - 1
        End Select
        lstDropList.SelectedIndex = dgDataList.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            dgDataList.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        dgDataList.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIf(SortCol.Text = "ASC", "DESC", "ASC")
        dgDataList.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim lblDelText As Label
        Dim strId As String
        Dim strOpCode As String = "CB_CLSTRX_REIMBURSEMENT_LIST_DELETE"
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer

        dgDataList.EditItemIndex = CInt(E.Item.ItemIndex)
        lblDelText = dgDataList.Items.Item(CInt(E.Item.ItemIndex)).FindControl("idCBId")
        strId = lblDelText.Text

        hidInit.Value = "1"

        strParamName = "DOCID"
        strParamValue = strId

        Try
            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCode, _
                                                strParamName, _
                                                strParamValue)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=REIMBURSEMENT POSTING&errmesg=" & lblErrMesage.Text & "&redirect=CB/trx/CB_trx_CashBanklist.aspx")
        End Try

        BindGrid()
        BindPageList()
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ID_GET&errmesg=" & Exp.ToString() & "&redirect=")
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
        dr("_Description") = "Please select Bank"
        objBankDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlBank.DataSource = objBankDs.Tables(0)
        ddlBank.DataValueField = "BankCode"
        ddlBank.DataTextField = "_Description"
        ddlBank.DataBind()
        ddlBank.SelectedIndex = intSelectedBankIndex
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
        hidInit.Value = ""
        Response.Redirect("CB_trx_ReimbursementDet.aspx")
    End Sub

    Sub SaveBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCode As String = "CB_CLSTRX_REIMBURSEMENT_SAVE"
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer

        Dim strGiroDate As String = Date_Validation(txtGiroDate.Text, False)
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
                        If ddlPayType.SelectedItem.Value = 1 Then
                            lblErrPayType.Text = "This payment type cannot use Kas Kecil account"
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
            If intSelPeriod < intCurPeriod And intLevel < 2 Then
                lblDate.Visible = True
                lblDate.Text = "This period already locked."
                Exit Sub
            End If
        End If

        If ddlPayType.SelectedItem.Value <> 5 Then
            If strBank = "" Then
                lblErrMesage.Visible = True
                lblErrMesage.Text = "Please select bank"
                Exit Sub
            End If
            If txtChequeNo.Text = "" Then
                lblErrMesage.Visible = True
                lblErrMesage.Text = "Please input cheque/giro no."
                Exit Sub
            End If
        End If

        strAccYear = Year(strDate)
        strAccMonth = Month(strDate)

        strParamName = "PAYMENTID|CREATEDATE|LOCCODE|ACCMONTH|ACCYEAR|UPDATEID|BANKCODE|CHEQUENO|CHEQUEDATE|REMARK|BANKACCNO|PAYTYPE"
        strParamValue = lblPaymentID.Text & "|" & strDate & "|" & strLocation & "|" & strAccMonth & "|" & strAccYear & "|" & strUserId & "|" & _
                        Trim(strBank) & "|" & txtChequeNo.Text & "|" & strGiroDate & "|" & Trim(txtRemark.Text) & "|" & Trim(strBankAccNo) & "|" & Trim(ddlPayType.SelectedItem.Value)

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
                                                 strParamName, _
                                                 strParamValue, _
                                                 objCashBankDs)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=REIMBURSEMENT_SAVE&errmesg=" & lblErrMesage.Text & "&redirect=CB/trx/CB_trx_CashBanklist.aspx")
        End Try

        If objCashBankDs.Tables(0).Rows.Count > 0 Then
            lblPaymentID.Text = objCashBankDs.Tables(0).Rows(0).Item("paymentid")
        End If

        BindGrid()
        BindPageList()
    End Sub

    Sub DeleteBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCode As String = "CB_CLSTRX_REIMBURSEMENT_LIST_UPDATE"
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer

        Dim strGiroDate As String = Date_Validation(txtGiroDate.Text, False)
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

        If Trim(lblPaymentID.Text) <> "" Then
            strParamName = "STRUPDATE"
            strParamValue = " SET PaymentDate='" & Trim(strDate) & "', BankCode='" & Trim(strBank) & "', " & _
                            "ChequeNo='" & Trim(txtChequeNo.Text) & "', ChequePrintDate='" & Trim(strGiroDate) & "', Status='4', UpdateDate=GETDATE(), UpdateID='" & Trim(strUserId) & "' " & _
                            "WHERE PaymentID='" & Trim(lblPaymentID.Text) & "' "

            Try
                intErrNo = objGLtrx.mtdInsertDataCommon(strOpCode, _
                                                    strParamName, _
                                                    strParamValue)
            Catch Exp As Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=REIMBURSEMENT POSTING&errmesg=" & lblErrMesage.Text & "&redirect=CB/trx/CB_trx_CashBanklist.aspx")
            End Try
        End If

        BindGrid()
        BindPageList()
    End Sub

    Sub ConfirmBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCode As String
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer

        Dim strGiroDate As String = Date_Validation(txtGiroDate.Text, False)
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

        If ddlPayType.SelectedItem.Value <> 5 Then
            If strBank = "" Then
                lblErrMesage.Visible = True
                lblErrMesage.Text = "Please select bank"
                Exit Sub
            End If
            If txtChequeNo.Text = "" Then
                lblErrMesage.Visible = True
                lblErrMesage.Text = "Please input cheque/giro no."
                Exit Sub
            End If
        End If


        If Trim(lblPaymentID.Text) <> "" Then
            strAccYear = Year(strDate)
            strAccMonth = Month(strDate)

            If ddlPayType.SelectedItem.Value <> "5" Then
                strOpCode = "CB_CLSTRX_REIMBURSEMENT_CONFIRM"
                strParamName = "PAYMENTID|CREATEDATE|LOCCODE|ACCMONTH|ACCYEAR|UPDATEID|BANKCODE|CHEQUENO|CHEQUEDATE|REMARK|BANKACCNO|PAYTYPE"
                strParamValue = lblPaymentID.Text & "|" & strDate & "|" & strLocation & "|" & strAccMonth & "|" & strAccYear & "|" & strUserId & "|" & _
                                Trim(strBank) & "|" & txtChequeNo.Text & "|" & strGiroDate & "|" & Trim(txtRemark.Text) & "|" & Trim(strBankAccNo) & "|" & ddlPayType.SelectedItem.Value
            Else
                strOpCode = "CB_CLSTRX_REIMBURSEMENT_CONFIRM_JOURNAL"
                strParamName = "PAYMENTID|CREATEDATE|LOCCODE|ACCMONTH|ACCYEAR|UPDATEID|BANKCODE|CHEQUENO|CHEQUEDATE|REMARK|BANKACCNO|PAYTYPE"
                strParamValue = lblPaymentID.Text & "|" & strDate & "|" & strLocation & "|" & strAccMonth & "|" & strAccYear & "|" & strUserId & "|" & _
                                Trim(strBank) & "|" & txtChequeNo.Text & "|" & strGiroDate & "|" & Trim(txtRemark.Text) & "|" & Trim("33.5.06.001") & "|" & ddlPayType.SelectedItem.Value
            End If

            
            Try
                intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
                                                     strParamName, _
                                                     strParamValue, _
                                                     objCashBankDs)
            Catch Exp As Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=REIMBURSEMENT_SAVE&errmesg=" & lblErrMesage.Text & "&redirect=CB/trx/CB_trx_CashBanklist.aspx")
            End Try

            If objCashBankDs.Tables(0).Rows.Count > 0 Then
                lblPaymentID.Text = objCashBankDs.Tables(0).Rows(0).Item("paymentid")
            End If
        End If

        BindGrid()
        BindPageList()
    End Sub

    Sub CancelBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCode As String = "CB_CLSTRX_REIMBURSEMENT_UPDATE"
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer

        Dim strGiroDate As String = Date_Validation(txtGiroDate.Text, False)
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

        If Trim(lblPaymentID.Text) <> "" Then
            strParamName = "PAYMENTID|UPDATEID|STRUPDATE"
            strParamValue = lblPaymentID.Text & "|" & strUserId & "|" & " SET PaymentDate='" & Trim(strDate) & "', BankCode='" & Trim(ddlBank.SelectedItem.Value) & "', " & _
                            "ChequeNo='" & Trim(txtChequeNo.Text) & "', ChequePrintDate='" & Trim(strGiroDate) & "', Status='6', UpdateDate=GETDATE(), UpdateID='" & Trim(strUserId) & "' " & _
                            "WHERE PaymentID='" & Trim(lblPaymentID.Text) & "' "

            Try
                intErrNo = objGLtrx.mtdInsertDataCommon(strOpCode, _
                                                    strParamName, _
                                                    strParamValue)
            Catch Exp As Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=REIMBURSEMENT POSTING&errmesg=" & lblErrMesage.Text & "&redirect=CB/trx/CB_trx_CashBanklist.aspx")
            End Try
        End If

        BindGrid()
        BindPageList()
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
