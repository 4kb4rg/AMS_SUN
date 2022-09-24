
Imports System
Imports System.Data
Imports System.Collections 
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic.Interaction

Public Class BI_trx_JournalList : Inherits Page

    Protected WithEvents dgLine As DataGrid
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblTracker As Label
    Protected WithEvents lblBillPartyTag As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents txtDebJournalID As TextBox
    Protected WithEvents txtBillParty As TextBox
    Protected WithEvents ddlJournalType As DropDownList
    Protected WithEvents ddlStatus As DropDownList
    Protected WithEvents txtLastUpdate As TextBox
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents lstAccMonth As DropDownList
    Protected WithEvents lstAccYear As DropDownList

    Protected objBITrx As New agri.BI.clsTrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objGLtrx As New agri.GL.ClsTrx()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intBIAR As Integer

    Dim objJournalDs As New Object()
    Dim objLangCapDs As New Object()
    Dim strParam As String
    Dim intErrNo As Integer
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim strLocType As String
    Dim strSelAccMonth As String
    Dim strSelAccYear As String

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strAccMonth = Session("SS_ARACCMONTH")
        strAccYear = Session("SS_ARACCYEAR")
        strLangCode = Session("SS_LANGCODE")
        intBIAR = Session("SS_BIAR")
        strLocType = Session("SS_LOCTYPE")
        strSelAccMonth = Session("SS_SELACCMONTH")
        strSelAccYear = Session("SS_SELACCYEAR")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumBIAccessRights.BIJournal), intBIAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            If SortExpression.Text = "" Then
                SortExpression.Text = "DebtorJrnID"
            End If

            If Not Page.IsPostBack Then
                If Session("SS_FILTERPERIOD") = "0" Then
                    lstAccMonth.SelectedValue = strAccMonth
                    BindAccYear(strAccYear)
                Else
                    lstAccMonth.SelectedValue = strSelAccMonth
                    BindAccYear(strSelAccYear)
                End If
                onload_GetLangCap()
                BindSearchList()
                BindJournalType()
                BindGrid()
                BindPageList()
            End If
        End If
    End Sub

    Sub onload_GetLangCap()

        GetEntireLangCap()

        lblBillPartyTag.Text = GetCaption(objLangCap.EnumLangCap.BillParty) & lblCode.Text
        dgLine.Columns(1).HeaderText = lblBillPartyTag.Text
    End Sub

    Sub GetEntireLangCap()
        Dim strOpCode_BussTerm As String = "PWSYSTEM_CLSLANGCAP_BUSSTERM_GET"

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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_CLSTRX_JOURNAL_LIST_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/BI_trx_JournalList.aspx")
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


    Sub BindJournalType()

        ddlJournalType.Items.Add(New ListItem(objBITrx.mtdGetDebtorJournalType(objBITrx.EnumDebtorJournalType.All), objBITrx.EnumDebtorJournalType.All))
        ddlJournalType.Items.Add(New ListItem(objBITrx.mtdGetDebtorJournalType(objBITrx.EnumDebtorJournalType.Adjustment), objBITrx.EnumDebtorJournalType.Adjustment))
        ddlJournalType.Items.Add(New ListItem(objBITrx.mtdGetDebtorJournalType(objBITrx.EnumDebtorJournalType.Allocation), objBITrx.EnumDebtorJournalType.Allocation))
        ddlJournalType.Items.Add(New ListItem(objBITrx.mtdGetDebtorJournalType(objBITrx.EnumDebtorJournalType.Void), objBITrx.EnumDebtorJournalType.Void))
        ddlJournalType.Items.Add(New ListItem(objBITrx.mtdGetDebtorJournalType(objBITrx.EnumDebtorJournalType.WriteOff), objBITrx.EnumDebtorJournalType.WriteOff))
    End Sub

    Sub BindSearchList()

        ddlStatus.Items.Add(New ListItem(objBITrx.mtdGetDebtorJournalStatus(objBITrx.EnumDebtorJournalStatus.All), objBITrx.EnumDebtorJournalStatus.All))
        ddlStatus.Items.Add(New ListItem(objBITrx.mtdGetDebtorJournalStatus(objBITrx.EnumDebtorJournalStatus.Active), objBITrx.EnumDebtorJournalStatus.Active))
        ddlStatus.Items.Add(New ListItem(objBITrx.mtdGetDebtorJournalStatus(objBITrx.EnumDebtorJournalStatus.Closed), objBITrx.EnumDebtorJournalStatus.Closed))
        ddlStatus.Items.Add(New ListItem(objBITrx.mtdGetDebtorJournalStatus(objBITrx.EnumDebtorJournalStatus.Confirmed), objBITrx.EnumDebtorJournalStatus.Confirmed))
        ddlStatus.Items.Add(New ListItem(objBITrx.mtdGetDebtorJournalStatus(objBITrx.EnumDebtorJournalStatus.Deleted), objBITrx.EnumDebtorJournalStatus.Deleted))
        ddlStatus.Items.Add(New ListItem(objBITrx.mtdGetDebtorJournalStatus(objBITrx.EnumDebtorJournalStatus.WrittenOff), objBITrx.EnumDebtorJournalStatus.WrittenOff))

        ddlStatus.SelectedIndex = 1
    End Sub

    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        dgLine.CurrentPageIndex = 0
        dgLine.EditItemIndex = -1
        BindGrid()
        BindPageList()
    End Sub


    Sub BindGrid()
        Dim strOpCd_GetLine As String = "BI_CLSTRX_DEBTORJOURNAL_LINE_GET"
        Dim strOppCode = "BI_CLSTRX_DEBTORJOURNAL_LIST_GET"
        Dim PageNo As Integer
        Dim PageCount As Integer
        Dim lbButton As LinkButton
        Dim strSrchID As String
        Dim strSrchBillParty As String
        Dim strSrchType As String
        Dim strSrchStatus As String
        Dim strSrchLastUpdate As String
        Dim strSearch As String
        Dim intCnt As Integer
        Dim strDebtorJrnID As String
        Dim objJournalLnDs As New DataSet()
        Dim lbl As Label

        If lstAccMonth.SelectedItem.Value = "0" Then
            strAccMonth = "1','2','3','4','5','6','7','8','9','10','11','12"
        Else
            strAccMonth = lstAccMonth.SelectedItem.Value
        End If

        strAccYear = lstAccYear.SelectedItem.Value
        strSrchID = IIf(txtDebJournalID.Text = "", "", txtDebJournalID.Text)
        strSrchBillParty = IIf(txtBillParty.Text = "", "", txtBillParty.Text)
        strSrchType = IIf(ddlJournalType.SelectedItem.Value = objBITrx.EnumDebtorJournalType.All, "", ddlJournalType.SelectedItem.Value)
        strSrchStatus = IIf(ddlStatus.SelectedItem.Value = objBITrx.EnumDebtorJournalStatus.All, "", ddlStatus.SelectedItem.Value)
        strSrchLastUpdate = IIf(txtLastUpdate.Text = "", "", txtLastUpdate.Text)

        strParam = strSrchID & "|" & _
                   strSrchBillParty & "|" & _
                   strSrchType & "|" & _
                   strSrchStatus & "|" & _
                   strSrchLastUpdate & "|" & _
                   SortExpression.Text & "|" & _
                   SortCol.Text

        Try
            intErrNo = objBITrx.mtdGetDebtorJournal(strOppCode, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strAccMonth, _
                                                    strAccYear, _
                                                    strParam, _
                                                    objJournalDs, _
                                                    False)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_JOURNALIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To objJournalDs.Tables(0).Rows.Count - 1
            objJournalDs.Tables(0).Rows(intCnt).Item("DebtorJrnID") = Trim(objJournalDs.Tables(0).Rows(intCnt).Item("DebtorJrnID"))
            objJournalDs.Tables(0).Rows(intCnt).Item("Remark") = Trim(objJournalDs.Tables(0).Rows(intCnt).Item("Remark"))
            objJournalDs.Tables(0).Rows(intCnt).Item("Status") = Trim(objJournalDs.Tables(0).Rows(intCnt).Item("Status"))
            objJournalDs.Tables(0).Rows(intCnt).Item("UpdateID") = Trim(objJournalDs.Tables(0).Rows(intCnt).Item("UpdateID"))
        Next

        PageCount = objGlobal.mtdGetPageCount(objJournalDs.Tables(0).Rows.Count, dgLine.PageSize)
        dgLine.DataSource = objJournalDs
        If dgLine.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgLine.CurrentPageIndex = 0
            Else
                dgLine.CurrentPageIndex = PageCount - 1
            End If
        End If
        dgLine.DataBind()
        BindPageList()

        For intCnt = 0 To dgLine.Items.Count - 1
            strDebtorJrnID = Trim(objJournalDs.Tables(0).Rows(intCnt).Item("DebtorJrnID"))

            strParam = strDebtorJrnID
            Try
                intErrNo = objBITrx.mtdGetDebtorJournalLine(strOpCd_GetLine, strParam, objJournalLnDs)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_JournalList_GETLINE&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/BI_trx_JournalList.aspx")
            End Try

            lbl = dgLine.Items.Item(intCnt).FindControl("lblStatus")
            Select Case CInt(Trim(lbl.Text))
                Case objBITrx.EnumDebtorJournalStatus.Active
                        lbButton = dgLine.Items.Item(intCnt).FindControl("lbDelete")
                        lbButton.Visible = True
                        lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                Case objBITrx.EnumDebtorJournalStatus.Deleted
                    lbButton = dgLine.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = True
                    lbButton.Text = "Undelete"
                    lbButton.Attributes("onclick") = "javascript:return ConfirmAction('undelete');"
                Case Else                   
                    lbButton = dgLine.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False
            End Select
        Next

        PageNo = dgLine.CurrentPageIndex + 1
        lblTracker.Text = "Page " & PageNo & " of " & dgLine.PageCount
    End Sub

    Sub BindPageList()
        Dim count As Integer = 1
        Dim arrDList As New ArrayList()

        While Not count = dgLine.PageCount + 1
            arrDList.Add("Page " & count)
            count = count + 1
        End While
        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = dgLine.CurrentPageIndex
    End Sub


    Sub btnPrevNext_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).CommandArgument
        Select Case direction
            Case "first"
                dgLine.CurrentPageIndex = 0
            Case "prev"
                dgLine.CurrentPageIndex = _
                Math.Max(0, dgLine.CurrentPageIndex - 1)
            Case "next"
                dgLine.CurrentPageIndex = _
                Math.Min(dgLine.PageCount - 1, dgLine.CurrentPageIndex + 1)
            Case "last"
                dgLine.CurrentPageIndex = dgLine.PageCount - 1
        End Select
        lstDropList.SelectedIndex = dgLine.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            dgLine.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        dgLine.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIf(SortCol.Text = "ASC", "DESC", "ASC")
        dgLine.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strOpCd_Upd As String = "BI_CLSTRX_DEBTORJOURNAL_UPD"
        Dim lblDelText As Label
        Dim strDebtorJrnID As String
        Dim intJrnType As Integer
        Dim strStatus As String
        Dim strOpCodes As String
        Dim pr_objNewID As Object

        dgLine.EditItemIndex = CInt(E.Item.ItemIndex)
        lblDelText = dgLine.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lblDebtorJrnID")
        strDebtorJrnID = lblDelText.Text
        lblDelText = dgLine.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lblJournalType")
        intJrnType = lblDelText.Text
        lblDelText = dgLine.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lblStatus")
        strStatus = lblDelText.Text

        strOpCodes = "|" & strOpCd_Upd
        If strStatus.Trim = objBITrx.EnumDebtorJournalStatus.Active Then
            strParam = intJrnType & "||" & strDebtorJrnID & "|||" & objBITrx.EnumDebtorJournalStatus.Deleted
        Else
            strParam = intJrnType & "||" & strDebtorJrnID & "|||" & objBITrx.EnumDebtorJournalStatus.Active
        End If
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_trx_JournalList_DEL&errmesg=" & lblErrMessage.Text & "&redirect=BI/trx/BI_trx_JournalList.aspx")
        End Try

        BindGrid()
        BindPageList()
    End Sub


    Sub NewBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("BI_trx_JournalDet.aspx")
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GET_ACCYEAR&errmesg=" & lblErrMessage.Text & "&redirect=")
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
