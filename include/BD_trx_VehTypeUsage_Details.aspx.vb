
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

Public Class BD_VehTypeUsage_Details : Inherits Page

    Protected WithEvents dgVehTypeUsg As DataGrid
    Protected WithEvents lblTracker As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents lblBgtStatus As Label 
    Protected WithEvents lblVehTypeCodeTag As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBudgeting As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblOper As Label
    Protected WithEvents lblLocTag As Label
    Protected WithEvents lblTotalCost As Label
    Protected WithEvents lblCode As Label

    Protected WithEvents srchCode As TextBox
    Protected WithEvents srchDesc As TextBox
    Protected WithEvents srchUpdateBy As TextBox

    Dim objBD As New agri.BD.clsTrx()
    Dim objBDSetup As New agri.BD.clsSetup()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Dim strOppCd_VehTypeDistUsg_GET As String = "BD_CLSTRX_VEHTYPEUSAGE_GET"
    Dim strOppCd_VehTypeDistUsg_ADD As String = "BD_CLSTRX_VEHTYPEUSAGE_ADD"
    Dim strOppCd_VehTypeDistUsg_UPD As String = "BD_CLSTRX_VEHTYPEUSAGE_UPD"
    Dim strOppCd_VehType_GET As String = "GL_CLSSETUP_VEHICLETYPE_LIST_GET"

    Dim objDataSet As New DataSet()
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
                SortExpression.Text = "BD.VehTypeCode"
                SortCol.Text = "ASC"
            End If

            If Not Page.IsPostBack Then
                AddVehType()
                BindGrid()
                BindPageList()
            End If
        End If
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblLocTag.Text = GetCaption(objLangCap.EnumLangCap.Location) & lblCode.Text
        lblVehTypeCodeTag.Text = GetCaption(objLangCap.EnumLangCap.VehType) & lblCode.Text
        dgVehTypeUsg.Columns(0).HeaderText = lblVehTypeCodeTag.text
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
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_VEHTYPEUSAGE_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_VehTypeUsage_Details.aspx")
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
        dgVehTypeUsg.CurrentPageIndex = 0
        dgVehTypeUsg.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub BindGrid()
        Dim Period As String

        Dim PageNo As Integer 
        Dim PageCount As Integer
        Dim dsData As DataSet
        
        dsData = LoadData
        PageCount = objGlobal.mtdGetPageCount(dsData.Tables(0).Rows.Count, dgVehTypeUsg.PageSize)
        
        dgVehTypeUsg.DataSource = dsData
        If dgVehTypeUsg.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgVehTypeUsg.CurrentPageIndex = 0
            Else
                dgVehTypeUsg.CurrentPageIndex = PageCount - 1
            End If
        End If
        
        dgVehTypeUsg.DataBind()
        BindPageList()
        PageNo = dgVehTypeUsg.CurrentPageIndex + 1
        lblTracker.Text="Page " & pageno & " of " & dgVehTypeUsg.PageCount

        lblLocCode.Text = strLocation
        GetActivePeriod(Period)
        lblBgtPeriod.Text = Period

        LoadTotal()
    End Sub

    Sub DataGrid_ItemDataBound(ByVal Sender As Object, ByVal e As DataGridItemEventArgs)
        Dim lbl As Label
        Dim btn As LinkButton
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            If lblBgtStatus.Text.Trim = objBDSetup.EnumPeriodStatus.Addvote Then
                btn = e.Item.FindControl("Edit")
                btn.Visible = False
            End If
        End If
    End Sub

    Protected Function GetActivePeriod(ByRef BGTPeriod As String) As String
        Dim strOppCd_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"
        Dim dsperiod As New DataSet()

        strParam = "|||||" & objBDSetup.EnumPeriodStatus.Active & "|" & strLocation & "|"

        Try
            intErrNo = objBDSetup.mtdGetPeriodList(strOppCd_GET, strParam, dsperiod)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_VehTypeUsage_Details.aspx")
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

    Sub BindPageList()

        Dim count As Integer = 1
        Dim arrDList As New ArrayList()

        While Not count = dgVehTypeUsg.PageCount + 1
            arrDList.Add("Page " & count)
            count = count + 1
        End While

        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = dgVehTypeUsg.CurrentPageIndex

    End Sub

    Sub AddVehType()
        Dim intError As Integer

        strParam = GetActivePeriod("") & "|"
        Try
            intErrNo = objBD.mtdUpdVehTypeDistUsage(strOppCd_VehTypeDistUsg_ADD, _
                                                    strOppCd_VehTypeDistUsg_GET, _
                                                    strOppCd_VehTypeDistUsg_UPD, _
                                                    strOppCd_VehType_GET, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strParam, _
                                                    objBDSetup.EnumOperation.Add, _
                                                    intError)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_VEHTYPEDISTUSAGE_ADD&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_VehTypeUsage_Details.aspx")
        End Try

    End Sub

    Protected Function LoadData() As DataSet

        strParam = srchCode.Text & "|" & _
                   srchDesc.Text & "|" & _
                   strLocation & "|" & _
                   GetActivePeriod("") & "|" & _
                   srchUpdateBy.Text & "||" & _
                   SortExpression.Text & " " & SortCol.Text & "|"
        Try
            intErrNo = objBD.mtdGetVehTypeDistUsage(strOppCd_VehTypeDistUsg_GET, strParam, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_VEHTYPEDISTUSAGE_DET&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_VehTypeUsage_Details.aspx")
        End Try

        Return objDataSet
    End Function

    Protected Sub LoadTotal()
        Dim dsTotal As New DataSet()
        Dim strVehTypeDistUsgCost_SUM As String = "BD_CLSTRX_VEHTYPEUSAGE_COST_SUM"

        strParam = GetActivePeriod("") & "|" & strLocation & "|"
        Try
            intErrNo = objBD.mtdGetVehTypeDistUsageTotal(strVehTypeDistUsgCost_SUM, strParam, dsTotal)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_VEHTYPEDIST_USG_TOTAL&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_VehTypeUsage_Details.aspx")
        End Try


        lblTotalCost.Text = ObjGlobal.GetIDDecimalSeparator(FormatNumber(Trim(dsTotal.Tables(0).Rows(0).Item("Total")), 0))

    End Sub

    Sub btnPrevNext_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).CommandArgument
        Select Case direction
            Case "first"
                dgVehTypeUsg.CurrentPageIndex = 0
            Case "prev"
                dgVehTypeUsg.CurrentPageIndex = _
                    Math.Max(0, dgVehTypeUsg.CurrentPageIndex - 1)
            Case "next"
                dgVehTypeUsg.CurrentPageIndex = _
                    Math.Min(dgVehTypeUsg.PageCount - 1, dgVehTypeUsg.CurrentPageIndex + 1)
            Case "last"
                dgVehTypeUsg.CurrentPageIndex = dgVehTypeUsg.PageCount - 1
        End Select
        lstDropList.SelectedIndex = dgVehTypeUsg.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            dgVehTypeUsg.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        dgVehTypeUsg.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIf(SortCol.Text = "ASC", "DESC", "ASC")
        dgVehTypeUsg.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
    End Sub

    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)

        lblOper.Text = objBD.EnumOperation.Update
        dgVehTypeUsg.EditItemIndex = CInt(E.Item.ItemIndex)

        BindGrid()
    End Sub

    Sub DEDR_Update(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim label As label
        Dim EditText As TextBox
        Dim intError As Integer
        Dim strCode As String
        Dim strUsage As String
        Dim strCost As String

        label = E.Item.FindControl("lblVehTypeCode")
        strCode = label.Text
        EditText = E.Item.FindControl("txtUsage")
        strUsage = EditText.Text
        EditText = E.Item.FindControl("txtCost")
        strCost = EditText.Text

        strParam = GetActivePeriod("") & "|" & strCode & "|" & strUsage & "|" & strCost & "|"
        Try
            intErrNo = objBD.mtdUpdVehTypeDistUsage(strOppCd_VehTypeDistUsg_ADD, _
                                                    strOppCd_VehTypeDistUsg_GET, _
                                                    strOppCd_VehTypeDistUsg_UPD, _
                                                    strOppCd_VehType_GET, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strParam, _
                                                    lblOper.Text, _
                                                    intError)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_VEHTYPEDISTUSAGE_UPD&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_VehTypeUsage_Details.aspx")
        End Try

        dgVehTypeUsg.EditItemIndex = -1
        BindGrid()

    End Sub

    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        If CInt(E.Item.ItemIndex) = 0 And dgVehTypeUsg.Items.Count = 1 And dgVehTypeUsg.PageCount <> 1 Then
            dgVehTypeUsg.CurrentPageIndex = dgVehTypeUsg.PageCount - 2
        End If
        dgVehTypeUsg.EditItemIndex = -1
        BindGrid()
    End Sub

End Class
