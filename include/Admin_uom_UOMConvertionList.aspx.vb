Imports System
Imports System.Data
Imports System.Collections 
Imports System.IO
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic
Imports agri.Admin
Imports agri.GlobalHdl.clsGlobalHdl

Public Class Admin_UomConvertionList : Inherits Page

    Protected WithEvents dgUOMConvertion As DataGrid
    Protected WithEvents lblTracker As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents txtUOMFrom As TextBox
    Protected WithEvents txtUOMTo As TextBox
    Protected WithEvents ddlStatus As DropDownList
    Protected WithEvents txtLastUpdate As TextBox
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents lblErrMessage As Label

    Protected objAdmin As New agri.Admin.clsUom()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()

    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim intADAR As Integer

    Dim objUOMConvertionDs As New Object()

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
                SortExpression.Text = "UOMFrom"
            End If
            If Not Page.IsPostBack Then
                BindGrid() 
                BindPageList()
            End If
        End If
    End Sub

    Sub srchBtn_Click(sender As Object, e As System.EventArgs) 
        dgUOMConvertion.CurrentPageIndex = 0
        dgUOMConvertion.EditItemIndex = -1
        BindGrid() 
        BindPageList()
    End Sub 

    Sub BindGrid() 
        Dim intCnt As Integer
        Dim lbButton As LinkButton
        Dim lbl As Label

        Dim PageNo As Integer 
        Dim PageCount As Integer
        Dim dsData As DataSet
        
        dsData = LoadData
        PageCount = objGlobal.mtdGetPageCount(dsData.Tables(0).Rows.Count, dgUOMConvertion.PageSize)
        
        dgUOMConvertion.DataSource = dsData
        If dgUOMConvertion.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgUOMConvertion.CurrentPageIndex = 0
            Else
                dgUOMConvertion.CurrentPageIndex = PageCount - 1
            End If
        End If
        
        dgUOMConvertion.DataBind()
        BindPageList()
        PageNo = dgUOMConvertion.CurrentPageIndex + 1
        lblTracker.Text="Page " & pageno & " of " & dgUOMConvertion.PageCount


        For intCnt = 0 to dgUOMConvertion.Items.Count - 1
            lbl = dgUOMConvertion.Items.Item(intCnt).FindControl("lblStatus")
            Select Case CInt(Trim(lbl.Text))
                Case objAdmin.EnumUOMStatus.Active
                    lbButton = dgUOMConvertion.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.visible = True
                    lbButton.attributes("onclick") = "javascript:return ConfirmAction('delete');"
                Case Else
                    lbButton = dgUOMConvertion.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.visible = False
            End Select
        Next
        
    End Sub 

    Sub BindPageList() 
        Dim count As Integer = 1   
        Dim arrDList As New ArrayList()

        While not count = dgUOMConvertion.PageCount + 1
            arrDList.Add("Page " & count)
            count = count + 1
        End While 
        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = dgUOMConvertion.CurrentPageIndex
    End Sub 

    Protected Function LoadData() As DataSet
        Dim strOpCd_GET As String = "ADMIN_CLSUOM_Convertion_LIST_GET"
        Dim strSrchUOMFrom As String
        Dim strSrchUOMTo As String
        Dim strSrchStatus As String
        Dim strSrchLastUpdate As String
        Dim strSearch As String
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer

        strSrchUOMFrom = IIF(txtUOMFrom.Text = "", "", txtUOMFrom.Text)
        strSrchUOMTo = IIF(txtUOMTo.Text = "", "", txtUOMTo.Text)
        strSrchStatus = IIF(ddlStatus.SelectedItem.Value = "0", "", ddlStatus.SelectedItem.Value)
        strSrchLastUpdate = IIF(txtLastUpdate.Text = "", "", txtLastUpdate.Text)
        strParam = strSrchUOMFrom & "|" & _
                   strSrchUOMTo & "|" & _
                   strSrchStatus & "|" & _
                   strSrchLastUpdate & "|" & _
                   SortExpression.Text & "|" & _
                   SortCol.Text & "|"

        Try
            intErrNo = objAdmin.mtdGetUOMConvertion(strOpCd_GET, _
                                                    strParam, _
                                                    objUOMConvertionDs)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADMIN_GET_UOMConvertion&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To objUOMConvertionDs.Tables(0).Rows.Count - 1
            objUOMConvertionDs.Tables(0).Rows(intCnt).Item(0) = Trim(objUOMConvertionDs.Tables(0).Rows(intCnt).Item(0))
            objUOMConvertionDs.Tables(0).Rows(intCnt).Item(1) = Trim(objUOMConvertionDs.Tables(0).Rows(intCnt).Item(1))
            objUOMConvertionDs.Tables(0).Rows(intCnt).Item(5) = Trim(objUOMConvertionDs.Tables(0).Rows(intCnt).Item(5))
            objUOMConvertionDs.Tables(0).Rows(intCnt).Item(3) = Trim(objUOMConvertionDs.Tables(0).Rows(intCnt).Item(3))
            objUOMConvertionDs.Tables(0).Rows(intCnt).Item(7) = Trim(objUOMConvertionDs.Tables(0).Rows(intCnt).Item(7))
        Next

        Return objUOMConvertionDs
    End Function

    Sub btnPrevNext_Click (sender As Object, e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).commandargument
        Select Case direction
            Case "first"
                dgUOMConvertion.CurrentPageIndex = 0
            Case "prev"
                dgUOMConvertion.CurrentPageIndex = _
                    Math.Max(0, dgUOMConvertion.CurrentPageIndex - 1)
            Case "next"
                dgUOMConvertion.CurrentPageIndex = _
                    Math.Min(dgUOMConvertion.PageCount - 1, dgUOMConvertion.CurrentPageIndex + 1)
            Case "last"
                dgUOMConvertion.CurrentPageIndex = dgUOMConvertion.PageCount - 1
        End Select
        lstDropList.SelectedIndex = dgUOMConvertion.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged (sender As Object, e As EventArgs)
        If Page.IsPostBack Then
            dgUOMConvertion.CurrentPageIndex = lstDropList.SelectedIndex 
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(sender As Object, e As DataGridPageChangedEventArgs)
        dgUOMConvertion.CurrentPageIndex = e.NewPageIndex
        BindGrid() 
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIF(SortCol.Text = "ASC", "DESC", "ASC")
        dgUOMConvertion.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid() 
    End Sub

    Sub DEDR_Delete(Sender As Object, e As DataGridCommandEventArgs)
        Dim strOpCd_ADD As String = ""
        Dim strOpCd_UPD As String = "ADMIN_CLSUOM_Convertion_LIST_UPD"
        Dim strParam As String = ""
        Dim UOMFrom As LinkButton
        Dim UOMTo As LinkButton
        Dim intErrNo As Integer

        UOMFrom = dgUOMConvertion.Items.Item(CInt(e.Item.ItemIndex)).FindControl("btnUOMFrom")
        UOMTo = dgUOMConvertion.Items.Item(CInt(e.Item.ItemIndex)).FindControl("btnUOMTo")

        strParam = UOMFrom.Text & "|" & UOMTo.Text & "||" & objAdmin.EnumUOMStatus.Deleted
         
       
        Try
            intErrNo = objAdmin.mtdUpdUOMConvertion(strOpCd_ADD, _
                                                    strOpCd_UPD, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strParam, _
                                                    True)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADMIN_DEL_UOMCONVERISON&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try
      
        dgUOMConvertion.EditItemIndex = -1
        BindGrid() 
    End Sub

    Sub DEDR_Edit(Sender As Object, E As DataGridCommandEventArgs)
        Dim UOMFrom As LinkButton
        Dim UOMTo As LinkButton

        UOMFrom = dgUOMConvertion.Items.Item(CInt(e.Item.ItemIndex)).FindControl("btnUOMFrom")
        UOMTo = dgUOMConvertion.Items.Item(CInt(e.Item.ItemIndex)).FindControl("btnUOMTo")


        Response.Redirect("Admin_UOM_UOMConvertionDet.aspx?UOMFrom=" & UOMFrom.Text & "&UOMTo=" & UOMTo.Text)
    End Sub

    Sub NewUOMConvertionBtn_Click(Sender As Object, E As ImageClickEventArgs)
        Response.Redirect("Admin_UOM_UOMConvertionDet.aspx")
    End Sub

End Class
