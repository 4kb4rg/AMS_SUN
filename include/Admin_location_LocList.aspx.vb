
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
Imports agri.PWSystem.clsLangCap

Public Class Admin_LocList : Inherits Page

    Protected WithEvents dgLocCode As DataGrid
    Protected WithEvents lblTracker As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents txtLocCode As TextBox
    Protected WithEvents txtDescription As TextBox
    Protected WithEvents ddlStatus As DropDownList
    Protected WithEvents txtLastUpdate As TextBox
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents lblTitle As Label
    Protected WithEvents lblLocation As Label
    Protected WithEvents lblDesc As Label

    Protected objAdmin As New agri.Admin.clsLoc()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim intADAR As Integer
    Dim strLocType as String

    Dim objLocCodeDs As New Object()
    Dim objLangCapDs As New Object()

    Sub Page_Load(Sender As Object, E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        intADAR = Session("SS_ADAR")
        strLocType = Session("SS_LOCTYPE")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumADAccessRights.ADLocation), intADAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            onload_GetLangCap()
            If SortExpression.Text = "" Then
                SortExpression.Text = "LocCode"
            End If
            If Not Page.IsPostBack Then
                BindGrid() 
                BindPageList()
            End If
        End If
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblLocation.text = GetCaption(objLangCap.EnumLangCap.Location)
        lblDesc.text = GetCaption(objLangCap.EnumLangCap.LocDesc)
        lblTitle.text = UCase(lblLocation.text)

        dgLocCode.Columns(0).headertext = lblLocation.text & lblCode.text
        dgLocCode.Columns(1).headertext = lblDesc.text
    End Sub

    Sub GetEntireLangCap()
        Dim strOpCode_BussTerm As String = "PWSYSTEM_CLSLANGCAP_BUSSTERM_GET"
        Dim strParam As String
        Dim intErrNo As Integer

        strParam = strLangCode
        Try
            intErrNo = objLangCap.mtdGetBussTerm(strOpCode_BussTerm, _
                                                 strCompany, _
                                                 strLocation, _
                                                 strUserId, _
                                                 "", _
                                                 "", _
                                                 objLangCapDs, _
                                                 strParam)
        Catch Exp As Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=PU_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=admin/location/admin_location_loclist.aspx")
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


    Sub srchBtn_Click(sender As Object, e As System.EventArgs) 
        dgLocCode.CurrentPageIndex = 0
        dgLocCode.EditItemIndex = -1
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
        PageCount = objGlobal.mtdGetPageCount(dsData.Tables(0).Rows.Count, dgLocCode.PageSize)
        
        dgLocCode.DataSource = dsData
        If dgLocCode.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgLocCode.CurrentPageIndex = 0
            Else
                dgLocCode.CurrentPageIndex = PageCount - 1
            End If
        End If
        
        dgLocCode.DataBind()
        BindPageList()
        PageNo = dgLocCode.CurrentPageIndex + 1
        lblTracker.Text="Page " & pageno & " of " & dgLocCode.PageCount
        
        For intCnt = 0 To dgLocCode.Items.Count - 1
            lbl = dgLocCode.Items.Item(intCnt).FindControl("lblStatus")
            Select Case CInt(Trim(lbl.Text))
                Case objAdmin.EnumLocStatus.Active
                        lbButton = dgLocCode.Items.Item(intCnt).FindControl("lbDelete")
                        lbButton.Visible = True
                        lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                Case objAdmin.EnumLocStatus.Deleted
                        lbButton = dgLocCode.Items.Item(intCnt).FindControl("lbDelete")
                        lbButton.Visible = False
            End Select
        Next

    End Sub 

    Sub BindPageList() 
        Dim count As Integer = 1   
        Dim arrDList As New ArrayList()

        While not count = dgLocCode.PageCount + 1
            arrDList.Add("Page " & count)
            count = count + 1
        End While 
        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = dgLocCode.CurrentPageIndex
    End Sub 

    Protected Function LoadData() As DataSet
        Dim strOpCd_GET As String = "ADMIN_CLSLOC_LOCATION_LIST_GET"
        Dim strSrchLocCode As String
        Dim strSrchDescription As String
        Dim strSrchStatus As String
        Dim strSrchLastUpdate As String
        Dim strSearch As String
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer

        strSrchLocCode = IIF(txtLocCode.Text = "", "", txtLocCode.Text)
        strSrchDescription = IIF(txtDescription.Text = "", "", txtDescription.Text)
        strSrchStatus = IIF(ddlStatus.SelectedItem.Value = "0", "", ddlStatus.SelectedItem.Value)
        strSrchLastUpdate = IIF(txtLastUpdate.Text = "", "", txtLastUpdate.Text)
        strParam = strSrchLocCode & "|" & _
                   strSrchDescription & "|" & _
                   strSrchStatus & "|" & _
                   strSrchLastUpdate & "|" & _
                   SortExpression.Text & "|" & _
                   SortCol.Text & "|"

        Try
            intErrNo = objAdmin.mtdGetLocCode(strOpCd_GET, strParam, objLocCodeDs)

        Catch Exp As Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADMIN_GET_LOCATION&errmesg=" & lblErrMessage.Text & "&redirect=admin/location/admin_location_loclist.aspx")
        End Try

        For intCnt = 0 To objLocCodeDs.Tables(0).Rows.Count - 1
            objLocCodeDs.Tables(0).Rows(intCnt).Item("LocCode") = objLocCodeDs.Tables(0).Rows(intCnt).Item("LocCode").Trim()
            objLocCodeDs.Tables(0).Rows(intCnt).Item("Description") = objLocCodeDs.Tables(0).Rows(intCnt).Item("Description").Trim()
            objLocCodeDs.Tables(0).Rows(intCnt).Item("Status") = objLocCodeDs.Tables(0).Rows(intCnt).Item("Status").Trim()
        Next

        Return objLocCodeDs
    End Function

    Sub btnPrevNext_Click (sender As Object, e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).commandargument
        Select Case direction
            Case "first"
                dgLocCode.CurrentPageIndex = 0
            Case "prev"
                dgLocCode.CurrentPageIndex = _
                    Math.Max(0, dgLocCode.CurrentPageIndex - 1)
            Case "next"
                dgLocCode.CurrentPageIndex = _
                    Math.Min(dgLocCode.PageCount - 1, dgLocCode.CurrentPageIndex + 1)
            Case "last"
                dgLocCode.CurrentPageIndex = dgLocCode.PageCount - 1
        End Select
        lstDropList.SelectedIndex = dgLocCode.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged (sender As Object, e As EventArgs)
        If Page.IsPostBack Then
            dgLocCode.CurrentPageIndex = lstDropList.SelectedIndex 
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(sender As Object, e As DataGridPageChangedEventArgs)
        dgLocCode.CurrentPageIndex = e.NewPageIndex
        BindGrid() 
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIF(SortCol.Text = "ASC", "DESC", "ASC")
        dgLocCode.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid() 
    End Sub

    Sub DEDR_Delete(Sender As Object, e As DataGridCommandEventArgs)
        Dim strOpCd_ADD As String = ""
        Dim strOpCd_UPD As String = "ADMIN_CLSLOC_LOCATION_LIST_UPD"
        Dim strParam As String = ""
        Dim strSelectedLocCode As String 
        Dim intErrNo As Integer
        Dim lblTemp As Label

        dgLocCode.EditItemIndex = CInt(E.Item.ItemIndex)
        lblTemp = dgLocCode.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lblLocCode")
        strSelectedLocCode = lblTemp.Text
        
        strParam = strSelectedLocCode & "||||" & objAdmin.EnumLocStatus.Deleted & "||||||||||||||||||||||||"

                

        Try
            intErrNo = objAdmin.mtdUpdLocCode(strOpCd_ADD, _
                                              strOpCd_UPD, _
                                              strCompany, _
                                              strLocation, _
                                              strUserId, _
                                              strParam, _
                                              True)
        Catch Exp As Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADMIN_DEL_LOCATION&errmesg=" & lblErrMessage.Text & "&redirect=admin/location/admin_location_loclist.aspx")
        End Try
      
        dgLocCode.EditItemIndex = -1
        BindGrid() 
    End Sub



    Sub NewLocCodeBtn_Click(Sender As Object, E As ImageClickEventArgs)
        Response.Redirect("Admin_location_LocDet.aspx")
    End Sub


End Class
