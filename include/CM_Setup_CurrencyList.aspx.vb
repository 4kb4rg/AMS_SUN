Imports System
Imports System.Data
Imports System.Collections 
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic

Public Class CM_Setup_CurrencyList : Inherits Page

    Protected WithEvents EventData As DataGrid
    Protected WithEvents ErrorMessage As Label
    Protected WithEvents lblTracker As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents StatusList As DropDownList
    Protected WithEvents SortExpression As Label
    Protected WithEvents blnUpdate As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblDupMsg As Label
    Protected WithEvents sortcol As Label
    Protected WithEvents srchCurrencyCode As TextBox
    Protected WithEvents srchDescription As TextBox
    Protected WithEvents srchUpdBy As TextBox
    Protected WithEvents srchStatus As DropDownList

    Protected objCMSetup As New agri.CM.clsSetup()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()

    Dim strOpCdGet As String = "CM_CLSSETUP_CURRENCY_GET"
    Dim strOpCdAdd As String = "CM_CLSSETUP_CURRENCY_ADD"
    Dim strOpCdUpd As String = "CM_CLSSETUP_CURRENCY_UPD"
    Dim objCurrDs As New Object()

    Dim intErrNo As Integer
    Dim strParam As String = ""

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim intCMAR As Integer

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_ARACCMONTH")
        strAccYear = Session("SS_ARACCYEAR")
        intCMAR = Session("SS_CMAR")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCMAccessRights.CMMasterSetup), intCMAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            If SortExpression.Text = "" Then
                SortExpression.Text = "CurrencyCode"
                sortcol.Text = "asc"
            End If
            If Not Page.IsPostBack Then
                BindSearchList()
                BindGrid()
                BindPageList()
            End If
        End If
    End Sub

    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        EventData.CurrentPageIndex = 0
        EventData.EditItemIndex = -1
        BindGrid()
        BindPageList()
    End Sub

    Sub BindGrid()
        Dim PageNo As Integer
        Dim PageCount As Integer
        Dim dsData As DataSet

        dsData = LoadData()
        PageCount = objGlobal.mtdGetPageCount(dsData.Tables(0).Rows.Count, EventData.PageSize)

        EventData.DataSource = dsData
        If EventData.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                EventData.CurrentPageIndex = 0
            Else
                EventData.CurrentPageIndex = PageCount - 1
            End If
        End If

        EventData.DataBind()
        BindPageList()
        PageNo = EventData.CurrentPageIndex + 1
        lblTracker.Text = "Page " & PageNo & " of " & EventData.PageCount
    End Sub

    Sub BindPageList()
        Dim count As Integer = 1
        Dim arrDList As New ArrayList()

        While Not count = EventData.PageCount + 1
            arrDList.Add("Page " & count)
            count = count + 1
        End While
        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = EventData.CurrentPageIndex
    End Sub

    Sub BindStatusList(ByVal index As Integer)
        StatusList = EventData.Items.Item(index).FindControl("StatusList")
        StatusList.Items.Add(New ListItem(objCMSetup.mtdGetCurrencyStatus(objCMSetup.EnumCurrencyStatus.Active), objCMSetup.EnumCurrencyStatus.Active))
        StatusList.Items.Add(New ListItem(objCMSetup.mtdGetCurrencyStatus(objCMSetup.EnumCurrencyStatus.Deleted), objCMSetup.EnumCurrencyStatus.Deleted))
    End Sub

    Sub BindSearchList()
        srchStatus.Items.Add(New ListItem(objCMSetup.mtdGetCurrencyStatus(objCMSetup.EnumCurrencyStatus.All), objCMSetup.EnumCurrencyStatus.All))
        srchStatus.Items.Add(New ListItem(objCMSetup.mtdGetCurrencyStatus(objCMSetup.EnumCurrencyStatus.Active), objCMSetup.EnumCurrencyStatus.Active))
        srchStatus.Items.Add(New ListItem(objCMSetup.mtdGetCurrencyStatus(objCMSetup.EnumCurrencyStatus.Deleted), objCMSetup.EnumCurrencyStatus.Deleted))
        srchStatus.SelectedIndex = 1
    End Sub

    Protected Function LoadData() As DataSet
        Dim strSearch As String = ""
        Dim strSort As String = ""
        Dim strParam As String
        Dim intCnt As Integer

        If Trim(srchCurrencyCode.Text) <> "" Then
            strSearch = strSearch & "and curr.CurrencyCode like '" & Trim(srchCurrencyCode.Text) & "%' "
        End If

        If Trim(srchDescription.Text) <> "" Then
            strSearch = strSearch & "and curr.Description like '" & Trim(srchDescription.Text) & "%' "
        End If

        If srchStatus.SelectedItem.Value <> CInt(objCMSetup.EnumCurrencyStatus.All) Then
            strSearch = strSearch & "and curr.Status = '" & srchStatus.SelectedItem.Value & "' "
        End If

        If Trim(srchUpdBy.Text) <> "" Then
            strSearch = strSearch & "and usr.UserName like '" & Trim(srchUpdBy.Text) & "%' "
        End If

        strSort = "order by " & Trim(SortExpression.Text) & " " & sortcol.Text
        strParam = strSearch & "|" & strSort

        Try
            intErrNo = objCMSetup.mtdGetMasterList(strOpCdGet, strParam, 0, objCurrDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_SETUP_CURRENCYLIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Setup/CM_Setup_CurrencyList.aspx")
        End Try

        If objCurrDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objCurrDs.Tables(0).Rows.Count - 1
                objCurrDs.Tables(0).Rows(intCnt).Item("CurrencyCode") = Trim(objCurrDs.Tables(0).Rows(intCnt).Item("CurrencyCode"))
                objCurrDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objCurrDs.Tables(0).Rows(intCnt).Item("Description"))
                objCurrDs.Tables(0).Rows(intCnt).Item("Status") = Trim(objCurrDs.Tables(0).Rows(intCnt).Item("Status"))
                objCurrDs.Tables(0).Rows(intCnt).Item("UpdateID") = Trim(objCurrDs.Tables(0).Rows(intCnt).Item("UpdateID"))
                objCurrDs.Tables(0).Rows(intCnt).Item("UserName") = Trim(objCurrDs.Tables(0).Rows(intCnt).Item("UserName"))
            Next
        End If
        Return objCurrDs
    End Function





    Sub btnPrevNext_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).CommandArgument
        Select Case direction
            Case "first"
                EventData.CurrentPageIndex = 0
            Case "prev"
                EventData.CurrentPageIndex = _
                    Math.Max(0, EventData.CurrentPageIndex - 1)
            Case "next"
                EventData.CurrentPageIndex = _
                    Math.Min(EventData.PageCount - 1, EventData.CurrentPageIndex + 1)
            Case "last"
                EventData.CurrentPageIndex = EventData.PageCount - 1
        End Select
        lstDropList.SelectedIndex = EventData.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            EventData.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        EventData.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        sortcol.Text = IIf(sortcol.Text = "asc", "desc", "asc")
        EventData.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
    End Sub

    Sub DEDR_Edit(ByVal Sender As Object, ByVal e As DataGridCommandEventArgs)
        Dim txtEditText As TextBox
        Dim ddlList As DropDownList
        Dim lbUpdbutton As LinkButton

        blnUpdate.Text = True
        EventData.EditItemIndex = CInt(e.Item.ItemIndex)

        BindGrid()
        If CInt(e.Item.ItemIndex) >= EventData.Items.Count Then
            EventData.EditItemIndex = -1
            Exit Sub
        End If
        BindStatusList(EventData.EditItemIndex)

        txtEditText = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("Status")
        Select Case CInt(txtEditText.Text) = objCMSetup.EnumCurrencyStatus.Active
            Case True
                StatusList.SelectedIndex = 0
                txtEditText = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("CurrencyCode")
                txtEditText.ReadOnly = True
                lbUpdbutton = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("Delete")
                lbUpdbutton.Text = "Delete"
                lbUpdbutton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
            Case False
                StatusList.SelectedIndex = 1
                txtEditText = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("CurrencyCode")
                txtEditText.Enabled = False
                txtEditText = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("Description")
                txtEditText.Enabled = False
                txtEditText = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("UpdateDate")
                txtEditText.Enabled = False
                txtEditText = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("UserName")
                txtEditText.Enabled = False
                ddlList = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("StatusList")
                ddlList.Enabled = False
                lbUpdbutton = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("Update")
                lbUpdbutton.Visible = False
                lbUpdbutton = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("Delete")
                lbUpdbutton.Text = "Undelete"
        End Select
    End Sub

    Sub DEDR_Update(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim txtEditText As TextBox
        Dim ddlList As DropDownList
        Dim strCurrencyCode As String
        Dim strDescription As String
        Dim strStatus As String
        Dim blnDupKey As Boolean = False
        Dim lblMsg As Label
        Dim strCreateDate As String
        Dim blnIsUpdate As Boolean

        If blnUpdate.Text = "false" Then
            blnIsUpdate = False
        Else
            blnIsUpdate = True
        End If

        txtEditText = E.Item.FindControl("CurrencyCode")
        strCurrencyCode = txtEditText.Text
        txtEditText = E.Item.FindControl("Description")
        strDescription = txtEditText.Text
        ddlList = E.Item.FindControl("StatusList")
        strStatus = ddlList.SelectedItem.Value

        strParam = strCurrencyCode & Chr(9) & _
                    strDescription & Chr(9) & _
                    strStatus
        Try
            intErrNo = objCMSetup.mtdUpdCurrency(strOpCdGet, _
                                                 strOpCdAdd, _
                                                 strOpCdUpd, _
                                                 strCompany, _
                                                 strLocation, _
                                                 strUserId, _
                                                 strParam, _
                                                 blnDupKey, _
                                                 blnIsUpdate)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CM_SETUP_CURRENCY_SAVE&errmesg=" & lblErrMessage.Text & "&redirect=CM/Setup/CM_Setup_CurrencyList.aspx")
        End Try

        If blnDupKey Then
            lblMsg = E.Item.FindControl("lblDupMsg")
            lblMsg.Visible = True
        Else
            EventData.EditItemIndex = -1
            BindGrid()
        End If

    End Sub

    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        EventData.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)

        Dim txtEditText As TextBox
        Dim strCurrencyCode As String
        Dim strDescription As String
        Dim strStatus As String
        Dim blnDupKey As Boolean = False
        Dim strCreateDate As String

        txtEditText = E.Item.FindControl("CurrencyCode")
        strCurrencyCode = txtEditText.Text
        txtEditText = E.Item.FindControl("Description")
        strDescription = txtEditText.Text
        txtEditText = E.Item.FindControl("Status")
        strStatus = IIf(txtEditText.Text = objCMSetup.EnumCurrencyStatus.Active, _
                        objCMSetup.EnumCurrencyStatus.Deleted, _
                        objCMSetup.EnumCurrencyStatus.Active)

        strParam = strCurrencyCode & Chr(9) & _
                    "" & Chr(9) & _
                    strStatus
        Try
            intErrNo = objCMSetup.mtdUpdCurrency(strOpCdGet, _
                                                 strOpCdAdd, _
                                                 strOpCdUpd, _
                                                 strCompany, _
                                                 strLocation, _
                                                 strUserId, _
                                                 strParam, _
                                                 False, _
                                                 True)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CM_SETUP_CURRENCY_DEL&errmesg=" & lblErrMessage.Text & "&redirect=CM/Setup/CM_Setup_CurrencyList.aspx")
        End Try

        EventData.EditItemIndex = -1
        BindGrid()

    End Sub

    Sub DEDR_Add(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim dataSet As dataSet = LoadData()
        Dim newRow As DataRow
        Dim lbUpdbutton As LinkButton
        Dim validateCode As RequiredFieldValidator
        Dim validateDesc As RequiredFieldValidator
        Dim PageCount As Integer

        blnUpdate.Text = "false"

        newRow = dataSet.Tables(0).NewRow()
        newRow.Item("CurrencyCode") = ""
        newRow.Item("Description") = ""
        newRow.Item("Status") = 1
        newRow.Item("CreateDate") = DateTime.Now()
        newRow.Item("UpdateDate") = DateTime.Now()
        newRow.Item("UserName") = ""
        dataSet.Tables(0).Rows.Add(newRow)

        EventData.DataSource = dataSet
        PageCount = objGlobal.mtdGetPageCount(dataSet.Tables(0).Rows.Count, EventData.PageSize)
        If EventData.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                EventData.CurrentPageIndex = 0
            Else
                EventData.CurrentPageIndex = PageCount - 1
            End If
        End If
        EventData.DataBind()
        BindPageList()

        EventData.CurrentPageIndex = EventData.PageCount - 1
        lblTracker.Text = "Page " & (EventData.CurrentPageIndex + 1) & " of " & EventData.PageCount
        lstDropList.SelectedIndex = EventData.CurrentPageIndex
        EventData.DataBind()
        EventData.EditItemIndex = EventData.Items.Count - 1
        EventData.DataBind()
        BindStatusList(EventData.EditItemIndex)

        lbUpdbutton = EventData.Items.Item(CInt(EventData.EditItemIndex)).FindControl("Delete")
        lbUpdbutton.Visible = False

    End Sub

End Class
