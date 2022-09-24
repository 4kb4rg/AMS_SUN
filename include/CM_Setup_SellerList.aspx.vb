Imports System
Imports System.Data
Imports System.Collections 
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic
Imports agri.GlobalHdl.clsGlobalHdl


Public Class CM_Setup_SellerList : Inherits Page

    Protected WithEvents dgLine As DataGrid
    Protected WithEvents lbDelete As LinkButton    
    Protected WithEvents lblTracker As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents txtSellerCode As TextBox
    Protected WithEvents txtDescription As TextBox
    Protected WithEvents ddlStatus As DropDownList
    Protected WithEvents txtLastUpdate As TextBox
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents lblErrMessage As Label

    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Protected objCMSetup As New agri.CM.clsSetup()    
    Dim objAR As New agri.GlobalHdl.clsAccessRights()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim intCMAR As Integer

    Dim objSellerDs As New Object()
    Dim objConfigDs As New Object()
    Dim objLangCapDs As New Object()

    Sub Page_Load(Sender As Object, E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_ARACCMONTH")
        strAccYear = Session("SS_ARACCYEAR")
        intCMAR = Session("SS_CMAR")

        If strUserId = "" Then
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCMAccessRights.CMMasterSetup), intCMAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            If SortExpression.Text = "" Then
                SortExpression.Text = "seller.Name"
            End If
            If Not Page.IsPostBack Then
                BindGrid() 
                BindPageList()
            End If
        End If
    End Sub

    Sub srchBtn_Click(sender As Object, e As System.EventArgs) 
        dgLine.CurrentPageIndex = 0
        dgLine.EditItemIndex = -1
        BindGrid() 
        BindPageList()
    End Sub 

    Sub BindGrid() 
        Dim PageNo As Integer 
        Dim intCnt As Integer
        Dim lbButton As LinkButton
        Dim dsTemp As New DataSet()

        dsTemp = LoadData()

        If Math.Ceiling(dsTemp.Tables(0).Rows.Count / dgLine.PageSize) < dgLine.CurrentPageIndex Then
            dgLine.CurrentPageIndex = 0
            dgLine.EditItemIndex = -1
        End If

        dgLine.DataSource = dsTemp
        dgLine.DataBind()

        For intCnt = 0 To dgLine.Items.Count - 1
            Select Case CInt(dsTemp.Tables(0).Rows(intCnt).Item("Status"))
                Case objCMSetup.EnumSellerStatus.Active
                    lbButton = dgLine.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = True
                    lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                Case objCMSetup.EnumSellerStatus.Deleted
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

    Protected Function LoadData() As DataSet

        Dim strOpCd_GET As String = "CM_CLSSETUP_SELLER_GET"
        Dim strSearch As String = ""
        Dim strSort As String = ""
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer

        If Trim(txtSellerCode.text) <> "" Then
            strSearch = strSearch & "and seller.SellerCode like '" & Trim(txtSellerCode.text) & "%' " 
        End If
        
        If Trim(txtDescription.text) <> "" Then
            strSearch = strSearch & "and seller.Name like '" & Trim(txtDescription.text) & "%' "
        End If
        
        If ddlStatus.SelectedItem.Value <> 0 Then
            strSearch = strSearch & "and seller.Status = '" & ddlStatus.SelectedItem.Value & "' "
        End If

        If Trim(txtLastUpdate.text) <> "" Then
            strSearch = strSearch & "and seller.UpdateID like '" & Trim(txtLastUpdate.text) & "%' "
        End If

        strSort = "order by " & Trim(SortExpression.text) & " " & SortCol.text
        strParam = strSearch & "|" & strSort

        Try
            intErrNo = objCMSetup.mtdGetMasterList(strOpCd_GET, strParam, 0, objSellerDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_SETUP_SELLERLIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Setup/CM_Setup_SellerList.aspx")
        End Try

        If objSellerDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objSellerDs.Tables(0).Rows.Count - 1
                objSellerDs.Tables(0).Rows(intCnt).Item("SellerCode") = Trim(objSellerDs.Tables(0).Rows(intCnt).Item("SellerCode"))
                objSellerDs.Tables(0).Rows(intCnt).Item("Name") = Trim(objSellerDs.Tables(0).Rows(intCnt).Item("Name"))
                objSellerDs.Tables(0).Rows(intCnt).Item("Status") = Trim(objSellerDs.Tables(0).Rows(intCnt).Item("Status"))
                objSellerDs.Tables(0).Rows(intCnt).Item("UpdateID") = Trim(objSellerDs.Tables(0).Rows(intCnt).Item("UpdateID"))
                objSellerDs.Tables(0).Rows(intCnt).Item("UserName") = Trim(objSellerDs.Tables(0).Rows(intCnt).Item("UserName"))
            Next
        End If
        Return objSellerDs
    End Function


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
        SortCol.Text = IIf(SortCol.Text = "asc", "desc", "asc")
        dgLine.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal e As DataGridCommandEventArgs)
        Dim lblDelText As Label
        Dim strOpCd_GET As String = ""
        Dim strOpCd_ADD As String = ""
        Dim strOpCd_UPD As String = "CM_CLSSETUP_SELLER_UPD"
        Dim strParam As String = ""
        Dim strSellerCode As String
        Dim intErrNo As Integer

        dgLine.EditItemIndex = CInt(e.Item.ItemIndex)
        lblDelText = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblLnId")
        strSellerCode = lblDelText.Text

        strParam = strSellerCode & Chr(9) & "" & Chr(9) & "" & Chr(9) & "" & Chr(9) & objCMSetup.EnumSellerStatus.Deleted

        Try
            intErrNo = objCMSetup.mtdUpdSeller(strOpCd_GET, _
                                               strOpCd_ADD, _
                                               strOpCd_UPD, _
                                               strCompany, _
                                               strLocation, _
                                               strUserId, _
                                               strParam, _
                                               False, _
                                               True)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_SETUP_SELLERLIST_DEL&errmesg=" & lblErrMessage.Text & "&redirect=CM/Setup/CM_Setup_SellerList.aspx")
        End Try

        dgLine.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub NewTBBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CM_Setup_SellerDet.aspx")
    End Sub




End Class
