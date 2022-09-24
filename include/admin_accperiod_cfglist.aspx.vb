
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



Public Class admin_accperiod_cfglist : Inherits Page

    Protected WithEvents dgResult as DataGrid
    Protected WithEvents lblTracker as Label
    Protected WithEvents SQLStatement as Label
    Protected WithEvents ddlStatusList as DropDownList
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents blnUpdate as Label
    Protected WithEvents curStatus as Label
    Protected WithEvents lblDupMsg as Label
    Protected WithEvents SortExpression as Label
    Protected WithEvents SortCol as Label
    Protected WithEvents srchAccYear as TextBox
    Protected WithEvents srchStatusList as DropDownList
    Protected WithEvents srchUpdateBy as TextBox

    Protected objAdmin As New agri.Admin.clsAccPeriod()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()

    Dim strOpCd_GET As String = "ADMIN_CLSACCPERIOD_CONFIG_GET"
    Dim strOpCd_ADD As String = "ADMIN_CLSACCPERIOD_CONFIG_ADD"
    Dim strOpCd_UPD As String = "ADMIN_CLSACCPERIOD_CONFIG_UPD"
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
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumADAccessRights.ADPeriodCfg), intADAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            If SortExpression.Text = "" Then
                SortExpression.Text = "HD.AccYear"
                SortCol.Text = "desc"
            End If
            If Not Page.IsPostBack Then
                BindSearchList() 
                BindGrid() 
                BindPageList()
            End If
        End IF
    End Sub

    Sub srchBtn_Click(sender As Object, e As System.EventArgs) 
        dgResult.CurrentPageIndex = 0
        dgResult.EditItemIndex = -1
        BindGrid() 
        BindPageList()
    End Sub 

    Sub BindGrid() 
        Dim PageNo As Integer 
        Dim PageCount As Integer
        Dim dsData As DataSet
        
        dsData = LoadData
        PageCount = objGlobal.mtdGetPageCount(dsData.Tables(0).Rows.Count, dgResult.PageSize)
        
        dgResult.DataSource = dsData
        If dgResult.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgResult.CurrentPageIndex = 0
            Else
                dgResult.CurrentPageIndex = PageCount - 1
            End If
        End If
        
        dgResult.DataBind()
        BindPageList()
        PageNo = dgResult.CurrentPageIndex + 1
        lblTracker.Text="Page " & pageno & " of " & dgResult.PageCount
    End Sub 

    Sub BindPageList() 
        Dim count as integer = 1   
        Dim arrDList As New ArrayList()

        While not count = dgResult.PageCount + 1
            arrDList.Add("Page " & count)
            Count = Count +1
        End While 
        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = dgResult.CurrentPageIndex
    End Sub 

    Sub BindddlStatusList(index as integer) 
        ddlStatusList = dgResult.Items.Item(index).FindControl("ddlStatusList")
        ddlStatusList.Items.Add(New ListItem(objAdmin.mtdGetAccPeriodCfgStatus(objAdmin.EnumAccPeriodCfg.Active), objAdmin.EnumAccPeriodCfg.Active))
    End Sub 

    Sub BindSearchList() 
        srchStatusList.Items.Add(New ListItem(objAdmin.mtdGetAccPeriodCfgStatus(objAdmin.EnumAccPeriodCfg.Active), objAdmin.EnumAccPeriodCfg.Active))
    End Sub 

    Protected Function LoadData() As DataSet
        Dim AccYear As String
        Dim Desc As String
        Dim srchStatus As String
        Dim UpdateBy As String
        Dim strParam As String
        Dim SearchStr As String = ""
        Dim SortItem As String
        Dim intCnt As Integer

        If Not srchAccYear.Text = "" Then
            SearchStr =  SearchStr & " AND HD.AccYear LIKE '" & srchAccYear.Text & "%' "
        End If

        If Not srchUpdateBy.Text = "" Then
            SearchStr = SearchStr & " AND USR.Username LIKE '" & srchUpdateBy.Text & "%' "
        End If

        SortItem = "ORDER BY " & SortExpression.Text & " " & SortCol.Text 
        strParam =  SortItem & "|" & SearchStr & "|"

        Try
           intErrNo = objAdmin.mtdGetAccPeriodCfg(strOpCd_GET, _
                                                  strCompany, _
                                                  strLocation, _
                                                  strUserId, _
                                                  strParam, _
                                                  objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADMIN_ACCPERIOD_CFG_GET&errmesg=" & Exp.ToString() & "&redirect=")
        End Try

        Return objDataSet
    End Function

    Sub btnPrevNext_Click (sender As Object, e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).commandargument
        Select Case direction
            Case "first"
                dgResult.CurrentPageIndex = 0
            Case "prev"
                dgResult.CurrentPageIndex = _
                    Math.Max(0, dgResult.CurrentPageIndex - 1)
            Case "next"
                dgResult.CurrentPageIndex = _
                    Math.Min(dgResult.PageCount - 1, dgResult.CurrentPageIndex + 1)
            Case "last"
                dgResult.CurrentPageIndex = dgResult.PageCount - 1
        End Select
        lstDropList.SelectedIndex = dgResult.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged (sender As Object, e As EventArgs)
        If Page.IsPostBack Then
            dgResult.CurrentPageIndex = lstDropList.SelectedIndex 
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(sender As Object, e As DataGridPageChangedEventArgs)
        dgResult.CurrentPageIndex=e.NewPageIndex
        BindGrid() 
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIF(SortCol.Text = "ASC", "DESC", "ASC")
        dgResult.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid() 
    End Sub
 
    Sub DEDR_Edit(Sender As Object, E As DataGridCommandEventArgs)
        Dim EditText As TextBox
        Dim List As DropDownList
        Dim updButton As LinkButton

        blnUpdate.Text = True
        dgResult.EditItemIndex = CInt(e.Item.ItemIndex)

        BindGrid() 

        If CInt(e.Item.ItemIndex) >= dgResult.Items.Count then
            dgResult.EditItemIndex = -1
            Exit Sub
        End If
        BindddlStatusList(dgResult.EditItemIndex)

        EditText = dgResult.Items.Item(CInt(e.Item.ItemIndex)).FindControl("txtStatus")
        Select Case CInt(EditText.Text) = objAdmin.EnumAccPeriodCfg.Active
            Case True
                ddlStatusList.SelectedIndex = 0
                EditText = dgResult.Items.Item(CInt(e.Item.ItemIndex)).FindControl("txtAccYear")
                EditText.ReadOnly = True
                updButton = dgResult.Items.Item(CInt(e.Item.ItemIndex)).FindControl("Update")
                updButton.Visible = True
        End Select       
    End Sub

    Sub DEDR_Update(Sender As Object, E As DataGridCommandEventArgs)
        Dim EditText As TextBox
        Dim EditLabel As Label
        Dim list As DropDownList
        Dim AccYear As String
        Dim MaxPeriod As String
        Dim Status As String
        Dim blnDupKey As Boolean = False
        Dim lblMsg as label
        Dim CreateDate As String
        Dim intINAccYear As Integer
        Dim intINAccMonth As Integer
 
        EditText = E.Item.FindControl("txtAccYear")
        AccYear = EditText.Text
        EditText = E.Item.FindControl("txtMaxPeriod")
        MaxPeriod = EditText.Text
        list = E.Item.FindControl("ddlStatusList")
        Status = list.SelectedItem.Value
        EditText = E.Item.FindControl("txtCreateDate")
        CreateDate = EditText.Text

        If Session("SS_INACCYEAR") <> "" Then
            intINAccYear = Convert.ToInt16(Session("SS_INACCYEAR"))
        Else
            intINAccYear = Convert.ToInt16(Year(Now()))
        End If

        If Session("SS_INACCMONTH") <> "" Then
            intINAccMonth = Convert.ToInt16(Session("SS_INACCMONTH"))
        Else
            intINAccMonth = Convert.ToInt16(Month(Now()))
        End If

        If Convert.ToInt16(AccYear) < intINAccYear Then
            EditLabel = E.Item.FindControl("lblErrYear")
            EditLabel.Visible = True
            Exit Sub
        ElseIf Convert.ToInt16(AccYear) = intINAccYear Then
            If Convert.ToInt16(MaxPeriod) < intINAccMonth Then
                EditLabel = E.Item.FindControl("lblErrYear")
                EditLabel.Visible = True
                Exit Sub
            End If
        End If

        strParam =  AccYear & "|" & _
                    MaxPeriod & "|" & _
                    Status & "|" & _
                    CreateDate 
        Try
            intErrNo = objAdmin.mtdUpdAccPeriodCfg(strOpCd_ADD, _
                                                strOpCd_UPD, _
                                                strOpCd_GET, _
                                                strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strParam, _
                                                blnDupKey, _
                                                blnUpdate.Text)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADMIN_ACCPERIOD_CFG_UPD&errmesg=" & Exp.ToString() & "&redirect=")
        End Try
        
        If blnDupKey Then
            lblMsg = E.Item.FindControl("lblDupMsg")
            lblMsg.Visible = True
        Else
            If AccYear = intINAccYear Then
                Session("SS_MAXPERIOD") = Convert.ToInt16(MaxPeriod)
            End If
            dgResult.EditItemIndex = -1
            BindGrid() 
        End If

    End Sub

    Sub DEDR_Cancel(Sender As Object, E As DataGridCommandEventArgs)
        dgResult.EditItemIndex = -1
        BindGrid() 
    End Sub


    Sub DEDR_Add(Sender As Object, E As ImageClickEventArgs)
        Dim DataSet As DataSet = LoadData
        Dim newRow As DataRow
        Dim updButton as LinkButton
        Dim PageCount As Integer

        blnUpdate.Text = False
        newRow = DataSet.Tables(0).NewRow()
        newRow.Item("AccYear") = ""
        newRow.Item("MaxPeriod") = ""
        newRow.Item("Status") = 1
        newRow.Item("CreateDate") = DateTime.now()
        NewRow.Item("UpdateDate") = DateTime.now()
        NewRow.Item("UserName") = ""
        DataSet.Tables(0).Rows.Add(newRow)
        
        dgResult.DataSource = dataSet
        PageCount = objGlobal.mtdGetPageCount(dataSet.Tables(0).Rows.Count, dgResult.PageSize)
        If dgResult.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgResult.CurrentPageIndex = 0
            Else
                dgResult.CurrentPageIndex = PageCount - 1
            End If
        End If
        dgResult.DataBind()
        BindPageList()

        dgResult.CurrentPageIndex = dgResult.PageCount - 1
        lblTracker.Text="Page " & (dgResult.CurrentPageIndex + 1) & " of " & dgResult.PageCount
        lstDropList.SelectedIndex = dgResult.CurrentPageIndex
        dgResult.DataBind()
        dgResult.EditItemIndex = dgResult.Items.Count -1
        dgResult.DataBind()
        BindddlStatusList(dgResult.EditItemIndex)
    End Sub

End Class
