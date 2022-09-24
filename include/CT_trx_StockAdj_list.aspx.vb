

Imports System
Imports System.Data
Imports System.Collections 
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic


Public Class CT_StockAdjust : Inherits Page
    Protected WithEvents dgStockAdj as DataGrid
    
    Protected WithEvents lblTracker As Label
    Protected WithEvents lblErrMessage as Label
    Protected WithEvents lblOrderBy As Label
    Protected WithEvents lblOrderDir As Label
    Protected WithEvents lblActionResult As Label
    
    Protected WithEvents ddlSearchAdjType as DropDownList
    Protected WithEvents ddlSearchTransType as DropDownList
    Protected WithEvents ddlSearchStatus as DropDownList
    Protected WithEvents ddlPage as DropDownList
    
    Protected WithEvents txtSearchStockAdjID as TextBox
    Protected WithEvents txtSearchUpdateBy as TextBox
    
    Protected objCT As New agri.CT.clsTrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    
    Dim strOpCdStockAdjustmentHeader_ADD As String = "CT_CLSTRX_STOCKADJUST_DETAIL_ADD"
    Dim strOpCdStockAdjustmentHeader_GET As String = "CT_CLSTRX_STOCKADJUST_DETAIL_GET"
    Dim strOpCdStockAdjustmentHeader_UPD As String = "CT_CLSTRX_STOCKADJUST_DETAIL_UPD"
    Dim strOpCdStockAdjustmentLine_GET As String = "CT_CLSTRX_STOCKADJUST_LINE_GET"

    Dim intErrNo As Integer

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim intCTAR As Integer

    Sub Page_Load(Sender As Object, E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_INACCMONTH")
        strAccYear = Session("SS_INACCYEAR")
        intCTAR = Session("SS_CTAR")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCTAccessRights.CTAdjustment), intCTAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            If lblOrderBy.Text = "" Then
                lblOrderBy.Text = "StockAdjID"
            End If
            If lblOrderDir.Text = "" Then
                lblOrderDir.Text = "ASC"
            End If
            
            lblActionResult.Visible = False 
            
            If Not Page.IsPostBack Then
                BindSearchAdjustmentTypeDropDownList()
                BindSearchTransactionDropDownList()
                BindSearchStatusDropDownList()
                BindDataGrid()
            End If
        End IF
    End Sub

    Sub BindSearchAdjustmentTypeDropDownList() 
        ddlSearchAdjType.Items.Add(New ListItem(objCT.mtdGetAdjustmentType(objCT.EnumAdjustmentType.All), objCT.EnumAdjustmentType.All))
        ddlSearchAdjType.Items.Add(New ListItem(objCT.mtdGetAdjustmentType(objCT.EnumAdjustmentType.Quantity), objCT.EnumAdjustmentType.Quantity))
        ddlSearchAdjType.Items.Add(New ListItem(objCT.mtdGetAdjustmentType(objCT.EnumAdjustmentType.AverageCost), objCT.EnumAdjustmentType.AverageCost))
        ddlSearchAdjType.Items.Add(New ListItem(objCT.mtdGetAdjustmentType(objCT.EnumAdjustmentType.TotalCost), objCT.EnumAdjustmentType.TotalCost))
        ddlSearchAdjType.Items.Add(New ListItem(objCT.mtdGetAdjustmentType(objCT.EnumAdjustmentType.QuantityAtAverageCost), objCT.EnumAdjustmentType.QuantityAtAverageCost))
        ddlSearchAdjType.Items.Add(New ListItem(objCT.mtdGetAdjustmentType(objCT.EnumAdjustmentType.QuantityAtTotalCost), objCT.EnumAdjustmentType.QuantityAtTotalCost))
        ddlSearchAdjType.SelectedIndex = 0
    End Sub 
    
    Sub BindSearchTransactionDropDownList() 
        ddlSearchTransType.Items.Add(New ListItem(objCT.mtdGetTransactionType(objCT.EnumTransactionType.All), objCT.EnumTransactionType.All))
        ddlSearchTransType.Items.Add(New ListItem(objCT.mtdGetTransactionType(objCT.EnumTransactionType.NewValue), objCT.EnumTransactionType.NewValue))
        ddlSearchTransType.Items.Add(New ListItem(objCT.mtdGetTransactionType(objCT.EnumTransactionType.Difference), objCT.EnumTransactionType.Difference))
        ddlSearchTransType.SelectedIndex = 0
    End Sub 
    
    Sub BindSearchStatusDropDownList() 
        ddlSearchStatus.Items.Add(New ListItem(objCT.mtdGetStockAdjustStatus(objCT.EnumStockAdjustStatus.All), objCT.EnumStockAdjustStatus.All))
        ddlSearchStatus.Items.Add(New ListItem(objCT.mtdGetStockAdjustStatus(objCT.EnumStockAdjustStatus.Active), objCT.EnumStockAdjustStatus.Active))
        ddlSearchStatus.Items.Add(New ListItem(objCT.mtdGetStockAdjustStatus(objCT.EnumStockAdjustStatus.Deleted), objCT.EnumStockAdjustStatus.Deleted))
        ddlSearchStatus.Items.Add(New ListItem(objCT.mtdGetStockAdjustStatus(objCT.EnumStockAdjustStatus.Confirmed), objCT.EnumStockAdjustStatus.Confirmed))
        ddlSearchStatus.SelectedIndex = 1
    End Sub 

    Sub BindPageDropDownList() 
        Dim intCnt As Integer = 1   
        Dim arrDList As New ArrayList()

        While Not intCnt = dgStockAdj.PageCount + 1
            arrDList.Add("Page " & intCnt)
            intCnt = intCnt + 1
        End While
        ddlPage.DataSource = arrDList
        ddlPage.DataBind()
        ddlPage.SelectedIndex = dgStockAdj.CurrentPageIndex
    End Sub 
    
    Sub ddlPage_OnSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            dgStockAdj.CurrentPageIndex = ddlPage.SelectedIndex
            BindDataGrid()
        End If
    End Sub
    
    Protected Function GetStockAdjustmentListDS() As DataSet
        Dim dsList As DataSet
        Dim strWhere As String
        Dim strOrderBy As String

        strWhere = "WHERE HD.LocCode = '" & strLocation.Trim() & "' AND HD.AccMonth = '" & strAccMonth.Trim() & "' AND HD.AccYear = '" & strAccYear.Trim() & "'"
        If txtSearchStockAdjID.Text.Trim() <> "" Then
            strWhere = strWhere & " AND HD.StockAdjID LIKE '" & Replace(txtSearchStockAdjID.Text.Trim(), "'", "''") & "'"
        End If
        If ddlSearchAdjType.SelectedItem.Value <> objCT.EnumAdjustmentType.All Then
            strWhere = strWhere & " AND HD.AdjType = '" & ddlSearchAdjType.SelectedItem.Value & "'"
        End If
        If ddlSearchTransType.SelectedItem.Value <> objCT.EnumTransactionType.All Then
            strWhere = strWhere & " AND HD.TransType = '" & ddlSearchTransType.SelectedItem.Value & "'"
        End If
        If ddlSearchStatus.SelectedItem.Value <> objCT.EnumStockAdjustStatus.All Then
            strWhere = strWhere & " AND HD.Status = '" & ddlSearchStatus.SelectedItem.Value & "'"
        End If
        If txtSearchUpdateBy.Text.Trim() <> "" Then
            strWhere = strWhere & " AND USR.UserName LIKE '" & Replace(txtSearchUpdateBy.Text.Trim(), "'", "''") & "'"
        End If
        
        strOrderBy = " ORDER BY " & lblOrderBy.Text.Trim() & " " & lblOrderDir.Text.Trim()
        Try
            intErrNo = objCT.mtdGetStockAdjustment(strOpCdStockAdjustmentHeader_GET, strWhere, strOrderBy, dsList)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_STOCKTRANSFERLIST&errmesg=" & lblErrMessage.Text & "&redirect=../EN/menu/menu_CTTrx_page.aspx")
        End Try
        Return dsList
        If Not dsList Is Nothing Then
            dsList = Nothing
        End If
    End Function
    
    Sub BindDataGrid() 
        Dim PageNo As Integer 
        Dim PageCount As Integer
        Dim dsData As DataSet
        
        dsData = GetStockAdjustmentListDS()
        PageCount = objGlobal.mtdGetPageCount(dsData.Tables(0).Rows.Count, dgStockAdj.PageSize)
        
        dgStockAdj.DataSource = dsData
        If dgStockAdj.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgStockAdj.CurrentPageIndex = 0
            Else
                dgStockAdj.CurrentPageIndex = PageCount - 1
            End If
        End If
        
        dgStockAdj.DataBind()
        BindPageDropDownList()
        PageNo = dgStockAdj.CurrentPageIndex + 1
        lblTracker.Text="Page " & pageno & " of " & dgStockAdj.PageCount
    End Sub 
    
    Sub dgStockAdj_OnItemDataBound(ByVal Sender As Object, ByVal e As DataGridItemEventArgs)
        Dim lbl As Label
        Dim btn As LinkButton

        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            lbl = e.Item.FindControl("lblStatus")
            btn = e.Item.FindControl("Delete")
            Select Case CInt(lbl.Text.Trim())
                Case objCT.EnumStockAdjustStatus.Active
                    btn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                    btn.Text = "Delete"
                    btn.visible = True
                Case objCT.EnumStockAdjustStatus.Deleted
                    btn.Attributes("onclick") = "javascript:return ConfirmAction('undelete');"
                    btn.Text = "Undelete"
                    btn.visible = True
                Case Else
                    btn.visible = False
            End Select
        End If
    End Sub
    
     Sub btnSearch_OnClick(sender As Object, e As System.EventArgs) 
        dgStockAdj.CurrentPageIndex = 0
        dgStockAdj.EditItemIndex = -1
        BindDataGrid() 
        BindPageDropDownList()
    End Sub 
    
    Sub ibPrevNext_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).CommandArgument
        Select Case direction
            Case "first"
                dgStockAdj.CurrentPageIndex = 0
            Case "prev"
                dgStockAdj.CurrentPageIndex = Math.Max(0, dgStockAdj.CurrentPageIndex - 1)
            Case "next"
                dgStockAdj.CurrentPageIndex = Math.Min(dgStockAdj.PageCount - 1, dgStockAdj.CurrentPageIndex + 1)
            Case "last"
                dgStockAdj.CurrentPageIndex = dgStockAdj.PageCount - 1
        End Select
        ddlPage.SelectedIndex = dgStockAdj.CurrentPageIndex
        BindDataGrid()
    End Sub
    
    Sub ibNew_OnClick(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Response.Redirect("CT_trx_StockAdj_Details.aspx")
    End Sub
    
    Sub dgStockAdj_OnPageIndexChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        dgStockAdj.CurrentPageIndex = e.NewPageIndex
        BindDataGrid()
    End Sub

    Sub dgStockAdj_OnSortCommand(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        lblOrderBy.Text = e.SortExpression.ToString()
        lblOrderDir.Text = IIf(lblOrderDir.Text = "ASC", "DESC", "ASC")
        dgStockAdj.CurrentPageIndex = ddlPage.SelectedIndex
        BindDataGrid()
    End Sub

    Sub dgStockAdj_OnDeleteCommand(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim lblDG As Label
        Dim strStockAdjId As String
        Dim intStatus As Integer
        Dim intAction As Integer
        Dim intErrNum As Integer
        Dim strParam As String
        
        lblDG = E.Item.FindControl("lblStockAdjId")
        strStockAdjId = lblDG.Text.Trim()

        lblDG = E.Item.FindControl("lblStatus")
        If CInt(lblDG.Text.Trim()) = objCT.EnumStockAdjustStatus.Active Then
            intStatus = objCT.EnumStockAdjustStatus.Deleted
            intAction = objCT.EnumTransactionAction.Delete
        Else
            intStatus = objCT.EnumStockAdjustStatus.Active
            intAction = objCT.EnumTransactionAction.Undelete
        End If
        
        strParam = strStockAdjId & vbCrLf & "" & vbCrLf & "" & vbCrLf & "" & vbCrLf & "" & vbCrLf & _
                   "" & vbCrLf & "" & vbCrLf & intStatus & vbCrLf & "" & vbCrLf & "" & vbCrLf & _
                   "" & vbCrLf & "" & vbCrLf & ""
        Try
            intErrNo = objCT.mtdStockAdjustmentHeader_Upd(strOpCdStockAdjustmentHeader_ADD, _
                                                          strOpCdStockAdjustmentHeader_UPD, _
                                                          strOpCdStockAdjustmentLine_GET, _
                                                          strCompany, _
                                                          strLocation, _
                                                          strUserId, _
                                                          strAccMonth, _
                                                          strAccYear, _
                                                          strParam, _
                                                          intAction, _
                                                          intErrNum, _
                                                          strStockAdjId)
        Catch Exp As System.Exception
            If intErrNo <> -5 Then
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=STOCK_ADJUSTMENT_UPDATE_FAILED&errmesg=&redirect=CT/Trx/CT_trx_StockAdj_list.aspx")
            End If
        End Try
        
        If intErrNum <> objCT.EnumTransactionError.NoError Then
            lblActionResult.Text = "System failed to " & IIf(intAction = objCT.EnumTransactionAction.Delete, "delete", "undelete") & " the stock adjustment " & strStockAdjId & "."
            lblActionResult.Visible = True
        End If
        BindDataGrid()
    End Sub

End Class
