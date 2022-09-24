
Imports System
Imports System.Data
Imports System.Collections 
Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic.Interaction

Imports agri.Admin.clsComp
Imports agri.GlobalHdl.clsGlobalHdl
Imports agri.PWSystem.clsLangCap

Public Class admin_comp_list : Inherits Page

    Protected WithEvents dgCoList As DataGrid
    Protected WithEvents lblTracker As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents ddlStatus As DropDownList
    Protected WithEvents txtCoCode As TextBox
    Protected WithEvents txtName As TextBox
    Protected WithEvents txtLastUpdate As TextBox
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents lblErrMesage As Label
    Protected WithEvents lblCompany As Label
    Protected WithEvents lblCompName As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents lblTitle As Label

    Protected objAdminComp As New agri.Admin.clsComp()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objAdminLoc As New agri.Admin.clsLoc()


    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim intADAR As Integer
    Dim objCompDs As New Object()
    Dim objLangCapDs As New Object()

    Dim strLocType as String

    Sub Page_Load(Sender As Object, E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        intADAR = Session("SS_ADAR")
        strLocType = Session("SS_LOCTYPE")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumADAccessRights.ADCompany), intADAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            onload_GetLangCap()
            If SortExpression.Text = "" Then
                SortExpression.Text = "CompCode"
            End If

            If Not Page.IsPostBack Then
                BindGrid() 
                BindPageList()
            End If
        End If
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        
        lblCompany.text = GetCaption(objLangCap.EnumLangCap.Company)
        lblTitle.text = UCase(lblCompany.text)
        lblCompName.text = GetCaption(objLangCap.EnumLangCap.CompName)
        dgCoList.Columns(1).headertext = lblCompany.text & lblCode.text
        dgCoList.Columns(2).headertext = lblCompName.text
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
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=PU_GET_LANGCAP&errmesg=" & lblErrMesage.Text & "&redirect=admin/company/admin_comp_list.aspx")
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



    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        dgCoList.CurrentPageIndex = 0
        dgCoList.EditItemIndex = -1
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
        PageCount = objGlobal.mtdGetPageCount(dsData.Tables(0).Rows.Count, dgCoList.PageSize)

        dgCoList.DataSource = dsData
        If dgCoList.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgCoList.CurrentPageIndex = 0
            Else
                dgCoList.CurrentPageIndex = PageCount - 1
            End If
        End If

        dgCoList.DataBind()
        BindPageList()
        PageNo = dgCoList.CurrentPageIndex + 1
        lblTracker.Text = "Page " & pageno & " of " & dgCoList.PageCount

        For intCnt = 0 To dgCoList.Items.Count - 1
            lbl = dgCoList.Items.Item(intCnt).FindControl("lblStatus")
            Select Case CInt(Trim(lbl.Text))
                Case objAdminComp.EnumCompanyStatus.Active
                    lbButton = dgCoList.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = True
                    lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                Case objAdminComp.EnumCompanyStatus.Deleted
                    lbButton = dgCoList.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False
            End Select
        Next

    End Sub

    Sub BindPageList()
        Dim count As Integer = 1
        Dim arrDList As New ArrayList

        While Not count = dgCoList.PageCount + 1
            arrDList.Add("Page " & count)
            Count = Count + 1
        End While
        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = dgCoList.CurrentPageIndex
    End Sub

    Protected Function LoadData() As DataSet
        Dim strOpCode_GetCompList As String = "ADMIN_CLSCOMP_COMPANY_LIST_GET"
        Dim strSrchCompCode As String
        Dim strSrchName As String
        Dim strSrchStatus As String
        Dim strSrchLastUpdate As String
        Dim strSearch As String
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer

        strSrchCompCode = IIf(txtCoCode.Text = "", "", txtCoCode.Text)
        strSrchName = IIf(txtName.Text = "", "", txtName.Text)
        strSrchStatus = IIf(ddlStatus.SelectedItem.Value = "0", "", ddlStatus.SelectedItem.Value)
        strSrchLastUpdate = IIf(txtLastUpdate.Text = "", "", txtLastUpdate.Text)
        strParam = strSrchCompCode & "|" & _
                   strSrchName & "|" & _
                   strSrchStatus & "|" & _
                   strSrchLastUpdate & "|" & _
                   SortExpression.Text & "|" & _
                   SortCol.Text

        Try
            intErrNo = objAdminComp.mtdGetComp(strOpCode_GetCompList, strParam, objCompDs)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=USERLIST_GET_USER&errmesg=" & lblErrMesage.Text & "&redirect=admin/company/admin_comp_list.aspx")
        End Try

        For intCnt = 0 To objCompDs.Tables(0).Rows.Count - 1
            objCompDs.Tables(0).Rows(intCnt).Item(0) = Trim(objCompDs.Tables(0).Rows(intCnt).Item(0))
            objCompDs.Tables(0).Rows(intCnt).Item(1) = Trim(objCompDs.Tables(0).Rows(intCnt).Item(1))
            objCompDs.Tables(0).Rows(intCnt).Item(2) = Trim(objCompDs.Tables(0).Rows(intCnt).Item(2))
            objCompDs.Tables(0).Rows(intCnt).Item(3) = Trim(objCompDs.Tables(0).Rows(intCnt).Item(3))
            objCompDs.Tables(0).Rows(intCnt).Item(4) = Trim(objCompDs.Tables(0).Rows(intCnt).Item(4))
        Next

        Return objCompDs
    End Function

    Sub btnPrevNext_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).commandargument
        Select Case direction
            Case "first"
                dgCoList.CurrentPageIndex = 0
            Case "prev"
                dgCoList.CurrentPageIndex = _
                Math.Max(0, dgCoList.CurrentPageIndex - 1)
            Case "next"
                dgCoList.CurrentPageIndex = _
                Math.Min(dgCoList.PageCount - 1, dgCoList.CurrentPageIndex + 1)
            Case "last"
                dgCoList.CurrentPageIndex = dgCoList.PageCount - 1
        End Select
        lstDropList.SelectedIndex = dgCoList.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            dgCoList.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        dgCoList.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIf(SortCol.Text = "ASC", "DESC", "ASC")
        dgCoList.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strOpCode_UpdComp As String = "ADMIN_CLSCOMP_COMPANY_DETAILS_UPD"
        Dim strCompParam As String = ""
        Dim CompCodeCell As TableCell = E.Item.Cells(0)
        Dim strSelectedComp As String
        Dim intErrNo As Integer

        strSelectedComp = CompCodeCell.Text
        strCompParam = strSelectedComp & "|||||||||||" & objAdminComp.EnumCompanyStatus.Deleted & "|"
        Try
            intErrNo = objAdminComp.mtdUpdComp(strOpCode_UpdComp, _
                                               strCompany, _
                                               strLocation, _
                                               strUserId, _
                                               strCompParam, _
                                               objAdminComp.EnumCompUpdType.Update)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=COMPLIST_DEL_USER&errmesg=" & lblErrMesage.Text & "&redirect=admin/company/admin_comp_list.aspx")
        End Try

        dgCoList.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub NewCoBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("admin_comp_det.aspx")
    End Sub

End Class
