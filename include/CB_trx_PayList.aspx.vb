
Imports System
Imports System.Data


Public Class cb_trx_PayList : Inherits Page

    Protected WithEvents lblErrMesage As Label
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents dgPayment As DataGrid
    Protected WithEvents lblTracker As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents ddlStatus As DropDownList
    Protected WithEvents txtPayID As TextBox
    Protected WithEvents txtSupplier As TextBox
    Protected WithEvents txtLastUpdate As TextBox
    Protected WithEvents lstAccMonth As DropDownList
    Protected WithEvents ddlPayType As DropDownList
    Protected WithEvents txtChequeNo As TextBox
    Protected WithEvents lstAccYear As DropDownList
    Protected WithEvents PostingBtn As ImageButton
    Protected WithEvents txtDocId As TextBox
    Protected WithEvents NewPayBtn As ImageButton
    Protected WithEvents ibPrint As ImageButton
    Protected WithEvents ForwardBtn As ImageButton

    Protected objCBTrx As New agri.CB.clsTrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim objGLtrx As New agri.GL.ClsTrx()
    
    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intCBAR As Integer
    Dim objPaymentDs As New Object()
    Dim strLocType As String
    Dim intLevel As Integer
    Dim strSelAccMonth As String
    Dim strSelAccYear As String

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
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCBAccessRights.CBPayment), intCBAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            If SortExpression.Text = "" Then
                SortExpression.Text = "PAY.CreateDate" '"PAY.PaymentID"
            End If

            'to avoid double click, on aspx add this : UseSubmitBehavior="false"
            NewPayBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(NewPayBtn).ToString())
            ibPrint.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(ibPrint).ToString())
            PostingBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(PostingBtn).ToString())
            ForwardBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(ForwardBtn).ToString())

            lblErrMesage.Visible = False

            If Not Page.IsPostBack Then
                If Session("SS_FILTERPERIOD") = "0" Then
                    lstAccMonth.SelectedValue = strAccMonth
                    BindAccYear(strAccYear)
                Else
                    lstAccMonth.SelectedValue = strSelAccMonth
                    BindAccYear(strSelAccYear)
                End If

                BindStatusList()
                BindGrid()
                BindPageList()
            End If

            If intLevel > 1 Then
                PostingBtn.Visible = True
                PostingBtn.Attributes("onclick") = "javascript:return ConfirmAction('posting this period (" & Trim(lstAccMonth.SelectedValue) & "/" & Trim(lstAccYear.SelectedValue) & ") ');"
                ForwardBtn.Visible = True
                ForwardBtn.Attributes("onclick") = "javascript:return ConfirmAction('move forward all pending transaction');"
            Else
                ForwardBtn.Visible = False
                PostingBtn.Visible = False
            End If
        End If
    End Sub

    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        dgPayment.CurrentPageIndex = 0
        dgPayment.EditItemIndex = -1
        BindGrid()
        BindPageList()
    End Sub

    Sub BindStatusList()
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetPaymentStatus(objCBTrx.EnumPaymentStatus.All), objCBTrx.EnumPaymentStatus.All))
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetPaymentStatus(objCBTrx.EnumPaymentStatus.Active), objCBTrx.EnumPaymentStatus.Active))
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetPaymentStatus(objCBTrx.EnumPaymentStatus.Closed), objCBTrx.EnumPaymentStatus.Closed))
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetPaymentStatus(objCBTrx.EnumPaymentStatus.Confirmed), objCBTrx.EnumPaymentStatus.Confirmed))
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetPaymentStatus(objCBTrx.EnumPaymentStatus.Deleted), objCBTrx.EnumPaymentStatus.Deleted))
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetPaymentStatus(objCBTrx.EnumPaymentStatus.Void), objCBTrx.EnumPaymentStatus.Void))
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetPaymentStatus(objCBTrx.EnumPaymentStatus.Verified), objCBTrx.EnumPaymentStatus.Verified))
        ddlStatus.SelectedIndex = 0
        'If intLevel = 0 Then
        '    ddlStatus.SelectedIndex = 6
        'Else
        '    ddlStatus.SelectedIndex = 0
        'End If
    End Sub

    Sub BindGrid()
        Dim strOpCode_GetPayList As String = "CB_CLSTRX_PAYMENT_LIST_GET" 
        Dim PageNo As Integer
        Dim PageCount As Integer
        Dim lbButton As LinkButton
        Dim strSrchPayID As String
        Dim strSrchSuppCode As String
        Dim strSrchStatus As String
        Dim strSrchLastUpdate As String
        Dim strSearch As String
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim lbl As Label
        Dim strSearchSourceType As String
        Dim strChequeNo As String
        Dim strDocId As String

        If lstAccMonth.SelectedItem.Value = "0" Then
            strAccMonth = "1','2','3','4','5','6','7','8','9','10','11','12"
        Else
            strAccMonth = lstAccMonth.SelectedItem.Value
        End If

        strAccYear = lstAccYear.SelectedItem.Value
        strSrchPayID = IIf(txtPayID.Text = "", "", txtPayID.Text)
        strSrchSuppCode = IIf(txtSupplier.Text = "", "", txtSupplier.Text)
        If intLevel = 0 Then
            strSrchStatus = IIf(ddlStatus.SelectedItem.Value = "0", "1','2','3','4','5','9", ddlStatus.SelectedItem.Value)
        Else
            strSrchStatus = IIf(ddlStatus.SelectedItem.Value = "0", "1','2','3','4','5','9", ddlStatus.SelectedItem.Value)
        End If
        strSrchLastUpdate = IIf(txtLastUpdate.Text = "", "", txtLastUpdate.Text)
        strSearchSourceType = IIf(ddlPayType.SelectedItem.Value = "9", "", ddlPayType.SelectedItem.Value)
        strChequeNo = IIf(txtChequeNo.Text = "", "", txtChequeNo.Text)
        strDocId = IIf(txtDocId.Text = "", "", txtDocId.Text)

        strParam = strSrchPayID & "|" & _
                   strSrchSuppCode & "|" & _
                   strSrchStatus & "|" & _
                   strSrchLastUpdate & "|" & _
                   strSearchSourceType & "|" & _
                   strChequeNo & "|" & _
                   strDocId & "|" & _
                   SortExpression.Text & "|" & _
                   SortCol.Text
        Try
            intErrNo = objCBTrx.mtdGetPayment(strOpCode_GetPayList, _
                                              strCompany, _
                                              strLocation, _
                                              strUserId, _
                                              strAccMonth, _
                                              strAccYear, _
                                              strParam, _
                                              objPaymentDs)
        Catch Exp As Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=PAYMENTLIST_GET_LIST&errmesg=" & lblErrMesage.Text & "&redirect=CB/trx/CB_trx_paylist.aspx")
        End Try

        PageCount = objGlobal.mtdGetPageCount(objPaymentDs.Tables(0).Rows.Count, dgPayment.PageSize)
        dgPayment.DataSource = objPaymentDs
        If dgPayment.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgPayment.CurrentPageIndex = 0
            Else
                dgPayment.CurrentPageIndex = PageCount - 1
            End If
        End If
        dgPayment.DataBind()
        BindPageList()

        For intCnt = 0 To dgPayment.Items.Count - 1
            lbl = dgPayment.Items.Item(intCnt).FindControl("lblStatus")
            Select Case CInt(Trim(lbl.Text))
                Case objCBTrx.EnumPaymentStatus.Active, objCBTrx.EnumPaymentStatus.Verified
                    lbl = dgPayment.Items.Item(intCnt).FindControl("idPayId")
                    If Left(Trim(lbl.Text), 3) = "XXX" Then
                        lbButton = dgPayment.Items.Item(intCnt).FindControl("lbDelete")
                        lbButton.Visible = True
                        lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                    Else
                        lbButton = dgPayment.Items.Item(intCnt).FindControl("lbDelete")
                        lbButton.Visible = False
                        lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                    End If
                   
                Case objCBTrx.EnumPaymentStatus.Confirmed, objCBTrx.EnumPaymentStatus.Closed
                    lbButton = dgPayment.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False
                Case objCBTrx.EnumPaymentStatus.Deleted, objCBTrx.EnumPaymentStatus.Void
                    lbButton = dgPayment.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False
            End Select
           
        Next

        PageNo = dgPayment.CurrentPageIndex + 1
        lblTracker.Text = "Page " & PageNo & " of " & dgPayment.PageCount
    End Sub

    Sub BindPageList()
        Dim count As Integer = 1
        Dim arrDList As New ArrayList()

        While Not count = dgPayment.PageCount + 1
            arrDList.Add("Page " & count)
            count = count + 1
        End While
        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = dgPayment.CurrentPageIndex
    End Sub


    Sub Update_Status(ByVal pv_strPaymentId As String, _
                      ByVal pv_intPaymentSts As Integer)

        Dim strOpCode_Payment As String = "CB_CLSTRX_PAYMENT_STATUS_UPD"
        Dim strParam As String = pv_strPaymentId & "|" & pv_intPaymentSts
        Dim intCnt As Integer
        Dim intErrNo As Integer

        Try
            intErrNo = objCBTrx.mtdUpdPaymentStatus(strOpCode_Payment, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strAccMonth, _
                                                    strAccYear, _
                                                    strParam)
        Catch Exp As Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=PAYMENTLIST_UPD_STATUS&errmesg=" & lblErrMesage.Text & "&redirect=CB/trx/CB_trx_paylist.aspx")
        End Try
    End Sub


    Sub btnPrevNext_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).CommandArgument
        Select Case direction
            Case "first"
                dgPayment.CurrentPageIndex = 0
            Case "prev"
                dgPayment.CurrentPageIndex = _
                Math.Max(0, dgPayment.CurrentPageIndex - 1)
            Case "next"
                dgPayment.CurrentPageIndex = _
                Math.Min(dgPayment.PageCount - 1, dgPayment.CurrentPageIndex + 1)
            Case "last"
                dgPayment.CurrentPageIndex = dgPayment.PageCount - 1
        End Select
        lstDropList.SelectedIndex = dgPayment.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            dgPayment.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        dgPayment.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIf(SortCol.Text = "ASC", "DESC", "ASC")
        dgPayment.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim lblDelText As Label
        Dim strPaymentId As String

        dgPayment.EditItemIndex = CInt(E.Item.ItemIndex)
        lblDelText = dgPayment.Items.Item(CInt(E.Item.ItemIndex)).FindControl("idPayId")
        strPaymentId = lblDelText.Text

        Update_Status(strPaymentId, objCBTrx.EnumPaymentStatus.Deleted)

        BindGrid()
        BindPageList()
    End Sub


    Sub NewPayBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CB_trx_PayDet.aspx")
    End Sub

    Sub BindAccYear(ByVal pv_strAccYear As String)
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer
        Dim intCnt As Integer = 0
        Dim intSelectedIndex As Integer
        Dim objAccYearDs As New Object
        Dim dr As DataRow
        Dim strOpCd As String = "ADMIN_CLSACCPERIOD_CONFIG_GET"

        strParamName = "LOCCODE|SEARCHSTR|SORTEXP"
        strParamValue = strLocation & "||Order By HD.AccYear"

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCd, _
                                                strParamName, _
                                                strParamValue, _
                                                objAccYearDs)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GET_ACCYEAR&errmesg=" & lblErrMesage.Text & "&redirect=")
        End Try

        For intCnt = 0 To objAccYearDs.Tables(0).Rows.Count - 1
            objAccYearDs.Tables(0).Rows(intCnt).Item("AccYear") = Trim(objAccYearDs.Tables(0).Rows(intCnt).Item("AccYear"))
            objAccYearDs.Tables(0).Rows(intCnt).Item("UserName") = Trim(objAccYearDs.Tables(0).Rows(intCnt).Item("AccYear"))
            If objAccYearDs.Tables(0).Rows(intCnt).Item("AccYear") = pv_strAccYear Then
                intSelectedIndex = intCnt + 1
            End If
        Next intCnt

        lstAccYear.DataSource = objAccYearDs.Tables(0)
        lstAccYear.DataValueField = "AccYear"
        lstAccYear.DataTextField = "UserName"
        lstAccYear.DataBind()
        lstAccYear.SelectedIndex = intSelectedIndex - 1
    End Sub

    Sub PostingBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCd As String = "CB_CLSTRX_PAYMENT_PERIOD_POSTING"
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer
        Dim postAccMonth As String
        Dim postAccYear As String

        If lstAccMonth.SelectedItem.Value = "0" Then
            postAccMonth = "1','2','3','4','5','6','7','8','9','10','11','12"
            lblErrMesage.Text = "Please select only 1 account period to proceed this posting."
            lblErrMesage.Visible = True
            Exit Sub
        Else
            postAccMonth = lstAccMonth.SelectedItem.Value
        End If

        postAccYear = lstAccYear.SelectedItem.Value

        Dim intInputPeriod As Integer = (CInt(postAccYear) * 100) + CInt(postAccMonth)
        Dim intCurPeriod As Integer = (CInt(strAccYear) * 100) + CInt(strAccMonth)
        Dim intSelPeriod As Integer = (CInt(strSelAccYear) * 100) + CInt(strSelAccMonth)

        If Session("SS_FILTERPERIOD") = "0" Then
            If intCurPeriod < intSelPeriod Then
                lblErrMesage.Visible = True
                lblErrMesage.Text = "Invalid posting period."
                Exit Sub
            End If
        Else
            If intSelPeriod <> intInputPeriod Then
                lblErrMesage.Visible = True
                lblErrMesage.Text = "Invalid posting period."
                Exit Sub
            End If
            If intSelPeriod < intCurPeriod Then
                lblErrMesage.Visible = True
                lblErrMesage.Text = "This period already locked."
                Exit Sub
            End If
        End If

        strParamName = "ACCMONTH|ACCYEAR|LOCCODE|USERID"
        strParamValue = postAccMonth & "|" & postAccYear & "|" & strLocation & "|" & strUserId

        Try
            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PERIOD POSTING&errmesg=" & Exp.ToString() & "&redirect=pu/trx/AP_trx_InvoiceRcvList")
        End Try

        BindGrid()
    End Sub

    Sub ForwardBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCd As String = "CB_CLSTRX_PAYMENT_MOVE_ALL"
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer
        Dim postAccMonth As String
        Dim postAccYear As String

        If lstAccMonth.SelectedItem.Value = "0" Then
            postAccMonth = "1','2','3','4','5','6','7','8','9','10','11','12"
            lblErrMesage.Text = "Please select only 1 account period to move forward."
            lblErrMesage.Visible = True
            Exit Sub
        Else
            postAccMonth = lstAccMonth.SelectedItem.Value
        End If

        postAccYear = lstAccYear.SelectedItem.Value

        Dim intInputPeriod As Integer = (CInt(postAccYear) * 100) + CInt(postAccMonth)
        Dim intCurPeriod As Integer = (CInt(strAccYear) * 100) + CInt(strAccMonth)
        Dim intSelPeriod As Integer = (CInt(strSelAccYear) * 100) + CInt(strSelAccMonth)

        If Session("SS_FILTERPERIOD") = "0" Then
            If intCurPeriod < intSelPeriod Then
                lblErrMesage.Visible = True
                lblErrMesage.Text = "Invalid posting period."
                Exit Sub
            End If
        Else
            If intSelPeriod <> intInputPeriod Then
                lblErrMesage.Visible = True
                lblErrMesage.Text = "Invalid posting period."
                Exit Sub
            End If
            If intSelPeriod < intCurPeriod Then
                lblErrMesage.Visible = True
                lblErrMesage.Text = "This period already locked."
                Exit Sub
            End If
        End If

        strParamName = "ACCMONTH|ACCYEAR|LOCCODE|USERID"
        strParamValue = postAccMonth & "|" & postAccYear & "|" & strLocation & "|" & strUserId

        Try
            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PERIOD POSTING&errmesg=" & Exp.ToString() & "&redirect=pu/trx/AP_trx_InvoiceRcvList")
        End Try

        BindGrid()
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
