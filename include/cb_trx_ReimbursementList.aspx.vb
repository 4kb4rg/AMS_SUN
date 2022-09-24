
Imports System
Imports System.Data


Public Class CB_trx_ReimbursementList : Inherits Page

    Protected WithEvents lblErrMesage As Label
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents dgDataList As DataGrid
    Protected WithEvents lblTracker As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents ddlStatus As DropDownList
    Protected WithEvents lstAccMonth As DropDownList
    Protected WithEvents lstAccYear As DropDownList
    Protected WithEvents NewBtn As ImageButton

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
    Dim objCashBankDs As New Object()
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
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCBAccessRights.CBCashBank), intCBAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            If SortExpression.Text = "" Then
                SortExpression.Text = "LEFT(CB.CASHBANKID,3)+RIGHT(RTRIM(CB.CASHBANKID),4)"
            End If

            'to avoid double click, on aspx add this : UseSubmitBehavior="false"
            NewBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(NewBtn).ToString())
            
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
        End If
    End Sub

    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        dgDataList.CurrentPageIndex = 0
        dgDataList.EditItemIndex = -1
        BindGrid()
        BindPageList()
    End Sub

    Sub BindStatusList()
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetCashBankStatus(objCBTrx.EnumCashBankStatus.All), objCBTrx.EnumCashBankStatus.All))
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetCashBankStatus(objCBTrx.EnumCashBankStatus.Active), objCBTrx.EnumCashBankStatus.Active))
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetCashBankStatus(objCBTrx.EnumCashBankStatus.Closed), objCBTrx.EnumCashBankStatus.Closed))
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetCashBankStatus(objCBTrx.EnumCashBankStatus.Confirmed), objCBTrx.EnumCashBankStatus.Confirmed))
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetCashBankStatus(objCBTrx.EnumCashBankStatus.Deleted), objCBTrx.EnumCashBankStatus.Deleted))
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetCashBankStatus(objCBTrx.EnumCashBankStatus.Cancelled), objCBTrx.EnumCashBankStatus.Cancelled))
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetCashBankStatus(objCBTrx.EnumCashBankStatus.Verified), objCBTrx.EnumCashBankStatus.Verified))
        If intLevel = 0 Then
            ddlStatus.SelectedIndex = 6
        Else
            ddlStatus.SelectedIndex = 0
        End If
    End Sub

    Sub BindGrid()
        Dim strOpCode As String = "CB_CLSTRX_REIMBURSEMENT_GET_ALL"
        Dim PageNo As Integer
        Dim PageCount As Integer
        Dim lbButton As LinkButton
        Dim strSrchID As String
        Dim strSrchFromTo As String
        Dim strSrchStatus As String
        Dim strSrchType As String = ""
        Dim strSrchLastUpdate As String
        Dim strSearch As String
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim lbl As Label
        Dim strSearchSourceType As String
        Dim strChequeNo As String
        Dim lblBalance As Label
        Dim strParamName As String
        Dim strParamValue As String

        If lstAccMonth.SelectedItem.Value = "0" Then
            strAccMonth = "1','2','3','4','5','6','7','8','9','10','11','12"
        Else
            strAccMonth = lstAccMonth.SelectedItem.Value
        End If

        strAccYear = lstAccYear.SelectedItem.Value
        If intLevel = 0 Then
            strSrchStatus = IIf(ddlStatus.SelectedItem.Value = "0", "1','2','4','5','6','9", ddlStatus.SelectedItem.Value)
        Else
            strSrchStatus = IIf(ddlStatus.SelectedItem.Value = "0", "1','2','4','5','6','9", ddlStatus.SelectedItem.Value)
        End If

        strParamName = "LOCCODE|ACCMONTH|ACCYEAR|STATUS"
        strParamValue = strLocation & "|" & strAccMonth & "|" & strAccYear & "|" & strSrchStatus

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
                                                strParamName, _
                                                strParamValue, _
                                                objCashBankDs)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CASHBANKTLIST_GET_LIST&errmesg=" & lblErrMesage.Text & "&redirect=CB/trx/CB_trx_CashBanklist.aspx")
        End Try

        PageCount = objGlobal.mtdGetPageCount(objCashBankDs.Tables(0).Rows.Count, dgDataList.PageSize)
        dgDataList.DataSource = objCashBankDs
        If dgDataList.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgDataList.CurrentPageIndex = 0
            Else
                dgDataList.CurrentPageIndex = PageCount - 1
            End If
        End If
        dgDataList.DataBind()
        BindPageList()

        For intCnt = 0 To dgDataList.Items.Count - 1
            lbl = dgDataList.Items.Item(intCnt).FindControl("lblStatus")

            Select Case CInt(Trim(lbl.Text))
                Case objCBTrx.EnumCashBankStatus.Active, objCBTrx.EnumCashBankStatus.Verified
                    lbl = dgDataList.Items.Item(intCnt).FindControl("idCBId")
                    If Right(Trim(lbl.Text), 4) = "XXXX" Then
                        lbButton = dgDataList.Items.Item(intCnt).FindControl("lbDelete")
                        lbButton.Visible = True
                        lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                    Else
                        lbButton = dgDataList.Items.Item(intCnt).FindControl("lbDelete")
                        lbButton.Visible = False
                        lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                    End If

                Case objCBTrx.EnumCashBankStatus.Confirmed, _
                     objCBTrx.EnumCashBankStatus.Deleted, _
                     objCBTrx.EnumCashBankStatus.Closed, _
                     objCBTrx.EnumCashBankStatus.Cancelled
                    lbButton = dgDataList.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False

            End Select
        Next

        PageNo = dgDataList.CurrentPageIndex + 1
        lblTracker.Text = "Page " & PageNo & " of " & dgDataList.PageCount
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
    End Sub


    Sub Update_Status(ByVal pv_strId As String, _
                      ByVal pv_intSts As Integer)

        Dim strOpCode As String = "CB_CLSTRX_CASHBANK_STATUS_UPD"
        Dim strParam As String = pv_strId & "|" & pv_intSts
        Dim intCnt As Integer
        Dim intErrNo As Integer

        Try
            intErrNo = objCBTrx.mtdUpdCashBankDetailStatus(strOpCode, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strAccMonth, _
                                                    strAccYear, _
                                                    strParam)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CASHBANKLIST_UPD_STATUS&errmesg=" & lblErrMesage.Text & "&redirect=CB/trx/CB_trx_cashbanklist.aspx")
        End Try
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

        dgDataList.EditItemIndex = CInt(E.Item.ItemIndex)
        lblDelText = dgDataList.Items.Item(CInt(E.Item.ItemIndex)).FindControl("idCBId")
        strId = lblDelText.Text

        Update_Status(strId, objCBTrx.EnumCashBankStatus.Deleted)

        BindGrid()
        BindPageList()
    End Sub

    Sub NewBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CB_trx_ReimbursementDet.aspx")
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
