
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

Public Class BD_WorkAcc_List : Inherits Page

    Protected WithEvents dgWorkAccList As DataGrid
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents lblTracker As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents lblErrMessage As Label

    Protected WithEvents lblWorkAccID As Label
    Protected WithEvents lblStatus As Label
    Protected WithEvents srchDesc As TextBox
    Protected WithEvents srchStatusList As DropDownList
    Protected WithEvents srchUpdateBy As TextBox
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblOper As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents lblLocTag As Label
    Protected WithEvents lblBgtStatus As Label 
    Protected WithEvents ibNew As ImageButton

    Protected objBD As New agri.BD.clsTrx()
    Dim objBDSetup As New agri.BD.clsSetup()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Dim strOppCd_GET As String = "BD_CLSTRX_WORKACC_GET"
    Dim strOppCd_UPD As String = "BD_CLSTRX_WORKACC_UPD"

    Dim objDsWorkAcc As New DataSet()
    Dim objLangCapDs As New DataSet()
    Dim intErrNo As Integer
    Dim strParam As String = ""

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim intADAR As Integer
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLocType as String

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        intADAR = Session("SS_ADAR")
        strLocType = Session("SS_LOCTYPE")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumADAccessRights.ADBudgeting), intADAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")

        Else
            onload_GetLangCap()

            If SortExpression.Text = "" Then
                SortExpression.Text = "WorkAccName"
                SortCol.Text = "ASC"
            End If

            If Not Page.IsPostBack Then
                BindSrchStatusList()
                BindGrid()
                BindPageList()
                If lblBgtStatus.Text.Trim = objBDSetup.EnumPeriodStatus.Addvote Then
                    ibNew.Visible = False  
                End If

            End If
        End If
    End Sub


    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblLocTag.Text = GetCaption(objLangCap.EnumLangCap.Location) & lblCode.Text
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
                                                 strAccMonth, _
                                                 strAccYear, _
                                                 objLangCapDs, _
                                                 strParam)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_WORKACCLIST_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_WorkAcc_List.aspx")
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


    Sub BindSrchStatusList()

        srchStatusList.Items.Add(New ListItem(objBD.mtdGetWorkAccStatus(objBD.EnumWorkAccStatus.All), objBD.EnumWorkAccStatus.All))
        srchStatusList.Items.Add(New ListItem(objBD.mtdGetWorkAccStatus(objBD.EnumWorkAccStatus.Active), objBD.EnumWorkAccStatus.Active))
        srchStatusList.Items.Add(New ListItem(objBD.mtdGetWorkAccStatus(objBD.EnumWorkAccStatus.Budgeted), objBD.EnumWorkAccStatus.Budgeted))

    End Sub

    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        dgWorkAccList.CurrentPageIndex = 0
        dgWorkAccList.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub BindGrid()
        Dim Period As String

        Dim PageNo As Integer 
        Dim PageCount As Integer
        Dim dsData As DataSet
        
        dsData = LoadData
        PageCount = objGlobal.mtdGetPageCount(dsData.Tables(0).Rows.Count, dgWorkAccList.PageSize)
        
        dgWorkAccList.DataSource = dsData
        If dgWorkAccList.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgWorkAccList.CurrentPageIndex = 0
            Else
                dgWorkAccList.CurrentPageIndex = PageCount - 1
            End If
        End If
        
        dgWorkAccList.DataBind()
        BindPageList()
        PageNo = dgWorkAccList.CurrentPageIndex + 1
        lblTracker.Text="Page " & pageno & " of " & dgWorkAccList.PageCount

        lblLocCode.Text = strLocation
        GetActivePeriod(Period)
        lblBgtPeriod.Text = Period

    End Sub

    Sub BindPageList()

        Dim count As Integer = 1
        Dim arrDList As New ArrayList()

        While Not count = dgWorkAccList.PageCount + 1
            arrDList.Add("Page " & count)
            count = count + 1
        End While

        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = dgWorkAccList.CurrentPageIndex

    End Sub

    Protected Function LoadData() As DataSet

        strParam = srchDesc.Text & "|" & _
                   srchStatusList.SelectedItem.Value & "|" & _
                   srchUpdateBy.Text & "|" & _
                   GetActivePeriod("") & "|" & _
                   strLocation & "|" & _
                   SortExpression.Text & " " & SortCol.Text

        Try
            intErrNo = objBD.mtdGetWorkAcc(strOppCd_GET, _
                                           strCompany, _
                                           strLocation, _
                                           strUserId, _
                                           strParam, _
                                           objDsWorkAcc, _
                                           False)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGET_WORKACC&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_WorkAcc_List.aspx")
        End Try

        Return objDsWorkAcc

    End Function

    Protected Function GetActivePeriod(ByRef BGTPeriod As String) As String
        Dim strOppCd_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"
        Dim dsperiod As New DataSet()

        strParam = "|||||" & objBDSetup.EnumPeriodStatus.Active & "|" & strLocation & "|"

        Try
            intErrNo = objBDSetup.mtdGetPeriodList(strOppCd_GET, strParam, dsperiod)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_WorkAcc_List.aspx")
        End Try
        If dsperiod.Tables(0).Rows.Count > 0 Then
            BGTPeriod = dsperiod.Tables(0).Rows(0).Item("BGTPeriod") & " - (" & objBDSetup.mtdGetPeriodStatus(dsperiod.Tables(0).Rows(0).Item("Status")) & ")"
            lblBgtStatus.Text = dsperiod.Tables(0).Rows(0).Item("Status") 
            Return dsperiod.Tables(0).Rows(0).Item("PeriodID")
        Else
            BGTPeriod = "No Active Period"
            Response.Redirect("../../BD/Setup/BD_setup_Periods.aspx")
        End If
    End Function

    Sub btnPrevNext_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).CommandArgument
        Select Case direction
            Case "first"
                dgWorkAccList.CurrentPageIndex = 0
            Case "prev"
                dgWorkAccList.CurrentPageIndex = _
                    Math.Max(0, dgWorkAccList.CurrentPageIndex - 1)
            Case "next"
                dgWorkAccList.CurrentPageIndex = _
                    Math.Min(dgWorkAccList.PageCount - 1, dgWorkAccList.CurrentPageIndex + 1)
            Case "last"
                dgWorkAccList.CurrentPageIndex = dgWorkAccList.PageCount - 1
        End Select
        lstDropList.SelectedIndex = dgWorkAccList.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            dgWorkAccList.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        dgWorkAccList.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIf(SortCol.Text = "ASC", "DESC", "ASC")
        dgWorkAccList.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
    End Sub












    Sub DEDR_Add(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("BD_Trx_WorkAcc_Details.aspx?")
    End Sub

End Class
