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
Imports agri.GlobalHdl.clsGlobalHdl
Imports agri.Admin

Public Class Admin_UOMList : Inherits Page

    Protected WithEvents dgUOM as DataGrid
    Protected WithEvents ErrorMessage as Label
    Protected WithEvents lblTracker as Label
    Protected WithEvents SQLStatement as Label
    Protected WithEvents StatusList as DropDownList
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents blnUpdate as Label
    Protected WithEvents curStatus as Label
    Protected WithEvents lblErrMessage as Label
    Protected WithEvents lblDupMsg as Label
    Protected WithEvents SortExpression as Label
    Protected WithEvents SortCol as Label
    Protected WithEvents srchUOMCode as TextBox
    Protected WithEvents srchDesc as TextBox
    Protected WithEvents srchStatusList as DropDownList
    Protected WithEvents srchUpdateBy as TextBox

    Protected objAdmin As New agri.Admin.clsUom()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()

    Dim strOpCd_GET As String = "ADMIN_CLSUOM_UOM_LIST_GET"
    Dim strOpCd_ADD As String = "ADMIN_CLSUOM_UOM_LIST_ADD"
    Dim strOpCd_UPD As String = "ADMIN_CLSUOM_UOM_LIST_UPD"
    Dim objDataSet As New Object()
    Dim intErrNo As Integer
    Dim strParam As String = ""

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim intADAR As Integer

    Sub Page_Load(Sender As Object, E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        intADAR = Session("SS_ADAR")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumADAccessRights.ADUOM), intADAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            If SortExpression.Text = "" Then
                SortExpression.Text = "UOMCode"
                SortCol.Text = "ASC"
            End If
            If Not Page.IsPostBack Then
                BindSearchList() 
                BindGrid() 
                BindPageList()
            End If
        End IF
    End Sub

    Sub srchBtn_Click(sender As Object, e As System.EventArgs) 
        dgUOM.CurrentPageIndex = 0
        dgUOM.EditItemIndex = -1
        BindGrid() 
        BindPageList()
    End Sub 

    Sub BindGrid() 

        Dim PageNo As Integer 
        Dim PageCount As Integer
        Dim dsData As DataSet
        
        dsData = LoadData
        PageCount = objGlobal.mtdGetPageCount(dsData.Tables(0).Rows.Count, dgUOM.PageSize)
        
        dgUOM.DataSource = dsData
        If dgUOM.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgUOM.CurrentPageIndex = 0
            Else
                dgUOM.CurrentPageIndex = PageCount - 1
            End If
        End If
        
        dgUOM.DataBind()
        BindPageList()
        PageNo = dgUOM.CurrentPageIndex + 1
        lblTracker.Text="Page " & pageno & " of " & dgUOM.PageCount
    End Sub 

    Sub BindPageList() 
        Dim count as integer = 1   
        Dim arrDList As New ArrayList()

        While not count = dgUOM.PageCount + 1
            arrDList.Add("Page " & count)
            Count = Count +1
        End While 
        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = dgUOM.CurrentPageIndex
    End Sub 

    Sub BindStatusList(index as integer) 
        StatusList = dgUOM.Items.Item(index).FindControl("StatusList")
        StatusList.Items.Add(New ListItem(objAdmin.mtdGetUOMStatus(objAdmin.EnumUOMStatus.Active), objAdmin.EnumUOMStatus.Active))
        StatusList.Items.Add(New ListItem(objAdmin.mtdGetUOMStatus(objAdmin.EnumUOMStatus.Deleted), objAdmin.EnumUOMStatus.Deleted))
    End Sub 

    Sub BindSearchList() 
        srchStatusList.Items.Add(New ListItem(objAdmin.mtdGetUOMStatus(objAdmin.EnumUOMStatus.Active), objAdmin.EnumUOMStatus.Active))
        srchStatusList.Items.Add(New ListItem(objAdmin.mtdGetUOMStatus(objAdmin.EnumUOMStatus.Deleted), objAdmin.EnumUOMStatus.Deleted))
        srchStatusList.Items.Add(New ListItem(objAdmin.mtdGetUOMStatus(objAdmin.EnumUOMStatus.All), objAdmin.EnumUOMStatus.All))
    End Sub 

    Protected Function LoadData() As DataSet
        Dim UOMCode As String
        Dim Desc As String
        Dim srchStatus As String
        Dim UpdateBy As String
        Dim strParam As String
        Dim SearchStr As String
        Dim SortItem As String
        Dim intCnt As Integer

        SearchStr =  " AND UOM.Status LIKE '" & IIF(Not srchStatusList.SelectedItem.Value = objAdmin.EnumUOMStatus.All, _
                       srchStatusList.SelectedItem.Value, "%" ) & "' "
        If NOT srchUOMCode.Text = "" Then
            SearchStr =  SearchStr & " AND UOM.UOMCode LIKE '" & srchUOMCode.Text & "%' "
        End If
        If NOT srchDesc.Text = "" Then
            SearchStr = SearchStr & " AND UOM.UOMDesc LIKE '" & srchDesc.Text & "%' "
        End If
        If NOT srchUpdateBy.Text = "" Then
            SearchStr = SearchStr & " AND USR.Username LIKE '" & srchUpdateBy.Text & "%' "
        End If

        SortItem = "ORDER BY " & SortExpression.Text & " " & SortCol.Text 
        strParam =  SortItem & "|" & SearchStr

        Try
           intErrNo = objAdmin.mtdGetUOM(strOpCd_GET, strParam, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADMIN_GET_UOM&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To objDataSet.Tables(0).Rows.Count - 1
            objDataSet.Tables(0).Rows(intCnt).Item("UOMCode") = objDataSet.Tables(0).Rows(intCnt).Item("UOMCode").Trim()
            objDataSet.Tables(0).Rows(intCnt).Item("UOMDesc") = objDataSet.Tables(0).Rows(intCnt).Item("UOMDesc").Trim()
        Next

        Return objDataSet
    End Function

    Sub btnPrevNext_Click (sender As Object, e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).commandargument
        Select Case direction
            Case "first"
                dgUOM.CurrentPageIndex = 0
            Case "prev"
                dgUOM.CurrentPageIndex = _
                    Math.Max(0, dgUOM.CurrentPageIndex - 1)
            Case "next"
                dgUOM.CurrentPageIndex = _
                    Math.Min(dgUOM.PageCount - 1, dgUOM.CurrentPageIndex + 1)
            Case "last"
                dgUOM.CurrentPageIndex = dgUOM.PageCount - 1
        End Select
        lstDropList.SelectedIndex = dgUOM.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged (sender As Object, e As EventArgs)
        If Page.IsPostBack Then
            dgUOM.CurrentPageIndex = lstDropList.SelectedIndex 
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(sender As Object, e As DataGridPageChangedEventArgs)
        dgUOM.CurrentPageIndex=e.NewPageIndex
        BindGrid() 
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIF(SortCol.Text = "ASC", "DESC", "ASC")
        dgUOM.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid() 
    End Sub
 
    Sub DEDR_Edit(Sender As Object, E As DataGridCommandEventArgs)
        Dim EditText As TextBox
        Dim List As DropDownList
        Dim updButton As LinkButton

        blnUpdate.Text = True
        dgUOM.EditItemIndex = CInt(e.Item.ItemIndex)

        BindGrid() 
        If CInt(e.Item.ItemIndex) >= dgUOM.Items.Count then
            dgUOM.EditItemIndex = -1
            Exit Sub
        End If
        BindStatusList(dgUOM.EditItemIndex)

        EditText = dgUOM.Items.Item(CInt(e.Item.ItemIndex)).FindControl("Status")
        Select Case CInt(EditText.Text) = objAdmin.EnumUOMStatus.Active
            Case True
                StatusList.SelectedIndex = 0
                EditText = dgUOM.Items.Item(CInt(e.Item.ItemIndex)).FindControl("UOMCode")
                EditText.ReadOnly = True
                updButton = dgUOM.Items.Item(CInt(e.Item.ItemIndex)).FindControl("Delete")
                updButton.Text = "Delete"
                updButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
            Case False
                Statuslist.SelectedIndex = 1
                EditText = dgUOM.Items.Item(CInt(e.Item.ItemIndex)).FindControl("UOMCode")
                EditText.Enabled = False
                EditText = dgUOM.Items.Item(CInt(e.Item.ItemIndex)).FindControl("UOMDesc")
                EditText.Enabled = False
                EditText = dgUOM.Items.Item(CInt(e.Item.ItemIndex)).FindControl("UpdateDate")
                EditText.Enabled = False
                EditText = dgUOM.Items.Item(CInt(e.Item.ItemIndex)).FindControl("UserName")
                EditText.Enabled = False
                List = dgUOM.Items.Item(CInt(e.Item.ItemIndex)).FindControl("StatusList")
                List.Enabled = False
                updButton = dgUOM.Items.Item(CInt(e.Item.ItemIndex)).FindControl("Update")
                updButton.Visible = False
                updButton = dgUOM.Items.Item(CInt(e.Item.ItemIndex)).FindControl("Delete")
                updButton.Text = "Undelete"
       End Select       
    End Sub

    Sub DEDR_Update(Sender As Object, E As DataGridCommandEventArgs)
        Dim EditText As TextBox
        Dim list As DropDownList
        Dim UOMCode As String
        Dim UOMDesc As String
        Dim Status As String
        Dim blnDupKey As Boolean = False
        Dim lblMsg as label
        Dim CreateDate As String
 
        EditText = E.Item.FindControl("UOMCode")
        UOMCode = EditText.Text
        EditText = E.Item.FindControl("UOMDesc")
        UOMDesc = EditText.Text
        list = E.Item.FindControl("StatusList")
        Status = list.SelectedItem.Value
        EditText = E.Item.FindControl("CreateDate")
        CreateDate = EditText.Text
        strParam =  UOMCode & "|" & _
                    UOMDesc & "|" & _
                    Status & "|" & _
                    CreateDate 

        Try
            intErrNo = objAdmin.mtdUpdUOM(strOpCd_ADD, _
                                          strOpCd_UPD, _
                                          strOpCd_GET, _
                                          strCompany, _
                                          strLocation, _
                                          strUserId, _
                                          strParam, _
                                          blnDupKey, _
                                          blnUpdate.Text)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADMIN_UPD_UOM&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try
        
        If blnDupKey Then
            lblMsg = E.Item.FindControl("lblDupMsg")
            lblMsg.Visible = True
        Else
            dgUOM.EditItemIndex = -1
            BindGrid() 
        End If

    End Sub

    Sub DEDR_Cancel(Sender As Object, E As DataGridCommandEventArgs)
        dgUOM.EditItemIndex = -1
        BindGrid() 
    End Sub

    Sub DEDR_Delete(Sender As Object, E As DataGridCommandEventArgs)

        Dim EditText As TextBox
        Dim UOMCode As String
        Dim UOMDesc As String
        Dim Status As String
        Dim blnDupKey As Boolean = False
        Dim CreateDate As String
 
        EditText = E.Item.FindControl("UOMCode")
        UOMCode = EditText.Text
        EditText = E.Item.FindControl("UOMDesc")
        UOMDesc = EditText.Text
        EditText = E.Item.FindControl("Status")
        Status = IIF(EditText.Text = objAdmin.EnumUOMStatus.Active, _
                                     objAdmin.EnumUOMStatus.Deleted, _
                                     objAdmin.EnumUOMStatus.Active )
        EditText = E.Item.FindControl("CreateDate")
        CreateDate = EditText.Text
        strParam = UOMCode & "|" & _
                   UOMDesc & "|" & _
                   Status & "|" & _
                   CreateDate 
        Try
            intErrNo = objAdmin.mtdUpdUOM(strOpCd_ADD, _
                                          strOpCd_UPD, _
                                          strOpCd_GET, _
                                          strCompany, _
                                          strLocation, _
                                          strUserId, _
                                          strParam, _
                                          blnDupKey, _
                                          blnUpdate.Text)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADMIN_UPD_UOM&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try
      
        dgUOM.EditItemIndex = -1
        BindGrid() 

    End Sub

    Sub DEDR_Add(Sender As Object, E As ImageClickEventArgs)
        Dim DataSet As DataSet = LoadData
        Dim newRow As DataRow
        Dim updButton as LinkButton
        Dim PageCount As Integer

        blnUpdate.Text = False
        newRow = DataSet.Tables(0).NewRow()
        newRow.Item("UOMCode") = ""
        newRow.Item("UOMDesc") = ""
        newRow.Item("Status") = 1
        newRow.Item("CreateDate") = DateTime.now()
        NewRow.Item("UpdateDate") = DateTime.now()
        NewRow.Item("UserName") = ""
        DataSet.Tables(0).Rows.Add(newRow)
        
        dgUOM.DataSource = dataSet
        PageCount = objGlobal.mtdGetPageCount(dataSet.Tables(0).Rows.Count, dgUOM.PageSize)
        If dgUOM.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgUOM.CurrentPageIndex = 0
            Else
                dgUOM.CurrentPageIndex = PageCount - 1
            End If
        End If
        dgUOM.DataBind()
        BindPageList()

        dgUOM.CurrentPageIndex = dgUOM.PageCount - 1
        lblTracker.Text="Page " & (dgUOM.CurrentPageIndex + 1) & " of " & dgUOM.PageCount
        lstDropList.SelectedIndex = dgUOM.CurrentPageIndex
        dgUOM.DataBind()
        dgUOM.EditItemIndex = dgUOM.Items.Count -1
        dgUOM.DataBind()
        BindStatusList(dgUOM.EditItemIndex)

        updButton = dgUOM.Items.Item(CInt(dgUOM.EditItemIndex)).FindControl("Delete")
        updButton.visible = False
    End Sub


End Class
