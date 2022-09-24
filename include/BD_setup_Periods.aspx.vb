
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

Public Class BD_Periods : Inherits Page

    Protected WithEvents PeriodData As DataGrid
    Protected WithEvents lblTracker As Label
    Protected WithEvents lblErrCopyTemp As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblErrClose As Label
    Protected WithEvents StatusList As DropDownList
    Protected WithEvents srchStatusList As DropDownList
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents blnUpdate As Label
    Protected WithEvents lblDupMsg As Label
    Protected WithEvents srchBgtPeriod As TextBox
    Protected WithEvents srchStartMonth As TextBox
    Protected WithEvents srchStartYear As TextBox
    Protected WithEvents srchEndMonth As TextBox
    Protected WithEvents srchEndYear As TextBox
    Protected WithEvents lblTitle As Label
    Protected WithEvents lblStockAnaCode As Label
    Protected WithEvents lblDescription As Label
    Protected WithEvents lblStartMthCtrl As Label
    Protected WithEvents lblStartYrCtrl As Label
    Protected WithEvents lblEndMthCtrl As Label
    Protected WithEvents lblEndYrCtrl As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblBudgeting As Label
    Protected WithEvents lblLocTag As Label
    Protected WithEvents ibClose As ImageButton

    Protected objBD As New agri.BD.clsSetup()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Protected objAdmin As New agri.Admin.clsAccPeriod()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objLoc As New agri.Admin.clsLoc()

    Dim strOppCd_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"
    Dim strOppCd_ADD As String = "BD_CLSSETUP_BGTPERIOD_ADD"
    Dim strOppCd_UPD As String = "BD_CLSSETUP_BGTPERIOD_UPD"
    Dim strOppCd_DEL As String = "BD_CLSSETUP_BGTPERIOD_DEL"

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
        strAccMonth = Session("SS_GLACCMONTH")
        strAccYear = Session("SS_GLACCYEAR")
        strLocType = Session("SS_LOCTYPE")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumADAccessRights.ADBudgeting), intADAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")

        Else
            onload_GetLangCap()
            ibClose.Attributes("onclick") = "javascript:return ConfirmAction('close');"

            If SortExpression.Text = "" Then
                SortExpression.Text = "BGTPeriod"
                SortCol.Text = "ASC"
            End If
            If Not Page.IsPostBack Then
                BindGrid()
            End If
            lblErrClose.Visible = False
            lblErrCopyTemp.Visible = False

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
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_PERIODS_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_Periods.aspx")
        End Try

    End Sub

    Function GetCaption(ByVal pv_TermCode) As String
        Dim count As Integer

        For count = 0 To objLangCapDs.Tables(0).Rows.Count - 1
            If Trim(pv_TermCode) = Trim(objLangCapDs.Tables(0).Rows(count).Item("TermCode")) Then
                If strLocType = objLoc.EnumLocType.Mill then
                    Return Trim(objLangCapDs.Tables(0).Rows(count).Item("BusinessTermMW"))
                else
                    Return Trim(objLangCapDs.Tables(0).Rows(count).Item("BusinessTerm"))
                end if
                Exit For
            End If
        Next
    End Function



    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        PeriodData.CurrentPageIndex = 0
        PeriodData.EditItemIndex = -1
        BindGrid()

    End Sub

    Sub BindGrid()
        Dim PageNo As Integer

        PeriodData.DataSource = LoadData()
        PeriodData.DataBind()
        lblLocCode.Text = strLocation
        lblBgtPeriod.Text = GetActivePeriod()

    End Sub

    Protected Function LoadData() As DataSet



        strParam = "|||||" & objBD.EnumPeriodStatus.All & "|" & strLocation & "|"

        Try
            intErrNo = objBD.mtdGetPeriodList(strOppCd_GET, strParam, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGETPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_Periods.aspx")
        End Try

        Return objDataSet
    End Function

    Sub Bind_PeriodList(ByVal pv_strYear As String, ByRef ddlYear As dropdownlist)
        Dim dr As DataRow
        Dim intErrNo As Integer
        Dim intSelectedIndex As Integer = 0

        Dim AccYear As String
        Dim Desc As String
        Dim intCnt As Integer
        Dim dsYearlist As DataSet = GetPlantwarePeriod()

        If pv_strYear <> "" Then
            For intCnt = 0 To dsYearlist.Tables(0).Rows.Count - 1
                If dsYearlist.Tables(0).Rows(intCnt).Item("AccYear").Trim() = pv_strYear Then
                    intSelectedIndex = intCnt + 1
                    Exit For
                End If
            Next
        End If

        dr = dsYearlist.Tables(0).NewRow()
        dr("AccYear") = "Select Year"
        dsYearlist.Tables(0).Rows.InsertAt(dr, 0)

        ddlYear.DataSource = dsYearlist.Tables(0)
        ddlYear.DataValueField = "AccYear"
        ddlYear.DataTextField = "AccYear"
        ddlYear.DataBind()
        ddlYear.SelectedIndex = intSelectedIndex
    End Sub

    Protected Function GetPlantwarePeriod(Optional ByVal strYear As String = "") As DataSet
        Dim strOpCd_GET As String = "ADMIN_CLSACCPERIOD_CONFIG_GET"
        Dim strParam As String
        Dim SearchStr As String = ""
        Dim SortItem As String
        Dim dsYear As DataSet
        SortItem = "ORDER BY HD.AccYear desc"

        If strYear <> "" Then
            SearchStr = "AND HD.AccYear = '" & strYear & "'"
        End If

        strParam = SortItem & "|" & SearchStr & "|"
        Try
            intErrNo = objAdmin.mtdGetAccPeriodCfg(strOpCd_GET, _
                                                   strCompany, _
                                                   strLocation, _
                                                   strUserId, _
                                                   strParam, _
                                                   dsYear)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADMIN_ACCPERIOD_CFG_GET&errmesg=" & Exp.ToString() & "&redirect=")
        End Try
        Return dsYear
    End Function

    Protected Function GetActivePeriod() As String



        strParam = "|||||" & objBD.EnumPeriodStatus.Active & "|" & strLocation & "|"

        Try
            intErrNo = objBD.mtdGetPeriodList(strOppCd_GET, strParam, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_Periods.aspx")
        End Try
        If objDataSet.Tables(0).Rows.Count > 0 Then
            Return objDataSet.Tables(0).Rows(0).Item("BGTPeriod")

        Else
            lblBgtPeriod.ForeColor = System.Drawing.Color.Red
            lblBgtPeriod.Font.Bold = True
            Return "No Active Period"
        End If
    End Function

    Sub DataGrid_ItemDataBound(ByVal Sender As Object, ByVal e As DataGridItemEventArgs)
        Dim lbl As Label
        Dim btn As LinkButton

        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            lbl = e.Item.FindControl("lblStatus")
            If lbl.Text.Trim = objBD.EnumPeriodStatus.Future Then
                btn = e.Item.FindControl("Edit")
                btn.Visible = True
            ElseIf lbl.Text.Trim = objBD.EnumPeriodStatus.Addvote Then
                lbl = e.Item.FindControl("lblStatustxt")
                lbl.Font.Bold = True
            ElseIf lbl.Text.Trim = objBD.EnumPeriodStatus.Active And e.Item.DataSetIndex <> 0 Then
                btn = PeriodData.Items.Item(CInt(e.Item.DataSetIndex) - 1).FindControl("AddVote")
                btn.Visible = True
                lbl = PeriodData.Items.Item(CInt(e.Item.DataSetIndex) - 1).FindControl("lblPerID")
                btn.CommandName = lbl.Text.Trim
                lbl = e.Item.FindControl("lblPerID")
                btn.CommandArgument = lbl.Text.Trim
            End If

        End If
    End Sub
    Sub btnPrevNext_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).CommandArgument
        Select Case direction
            Case "first"
                PeriodData.CurrentPageIndex = 0
            Case "prev"
                PeriodData.CurrentPageIndex = _
                    Math.Max(0, PeriodData.CurrentPageIndex - 1)
            Case "next"
                PeriodData.CurrentPageIndex = _
                    Math.Min(PeriodData.PageCount - 1, PeriodData.CurrentPageIndex + 1)
            Case "last"
                PeriodData.CurrentPageIndex = PeriodData.PageCount - 1
        End Select
        lstDropList.SelectedIndex = PeriodData.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            PeriodData.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        PeriodData.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
    End Sub

    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim EditText As TextBox
        Dim List As DropDownList
        Dim lbl As Label
        Dim Updbutton As LinkButton
        Dim strStartMth As String
        Dim strEndMth As String
        Dim btn As LinkButton
        Dim Status As String

        blnUpdate.Text = True
        PeriodData.EditItemIndex = CInt(E.Item.ItemIndex)

        lbl = E.Item.FindControl("lblEndMonth")
        strEndMth = lbl.Text
        lbl = E.Item.FindControl("lblStartMonth")
        strStartMth = lbl.Text

        BindGrid()

        If E.Item.ItemIndex > 0 Then
            lbl = PeriodData.Items.Item(CInt(E.Item.ItemIndex) - 1).FindControl("lblEndMonth")
            lblStartMthCtrl.Text = lbl.Text
        Else
            lblStartMthCtrl.Text = 0
            lblStartYrCtrl.Text = 0
        End If
        lbl = E.Item.FindControl("lblEndMonth")

        If E.Item.ItemIndex < PeriodData.Items.Count - 1 Then
            lbl = PeriodData.Items.Item(CInt(E.Item.ItemIndex) + 1).FindControl("lblStartMonth")
            lblEndMthCtrl.Text = lbl.Text
            lbl = PeriodData.Items.Item(CInt(E.Item.ItemIndex) + 1).FindControl("lblStartYear")
            lblEndYrCtrl.Text = lbl.Text
        Else
            lblEndMthCtrl.Text = 0
            lblEndYrCtrl.Text = 0

        End If

        lbl = PeriodData.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lblStatus")
        Status = lbl.Text
        btn = PeriodData.Items.Item(CInt(E.Item.ItemIndex)).FindControl("Delete")

        lbl = E.Item.FindControl("lblStartYear")
        List = PeriodData.Items.Item(CInt(PeriodData.EditItemIndex)).FindControl("ddlYear")
        Bind_PeriodList(lbl.Text.Trim, List)

        Select Case Status.Trim
            Case objBD.EnumPeriodStatus.Deleted
                btn.Text = "Undelete"
            Case objBD.EnumPeriodStatus.Passed, objBD.EnumPeriodStatus.Future
                btn.Text = "Delete"
                btn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
            Case objBD.EnumPeriodStatus.Active
                btn.Visible = False
        End Select

    End Sub

    Sub DEDR_Close(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOppCd_VehRunSetup_Format_GET As String = "BD_CLSSETUP_VEHRUNNING_FORMAT_GET"
        Dim strOppCd_VehRunSetup_ADD As String = "BD_CLSSETUP_VEHRUNNING_ADD"
        Dim strOppCd_VehRunSetup_UPD As String = "BD_CLSSETUP_VEHRUNNING_UPD"
        Dim strOppCd_OverheadSetup_Format_GET As String = "BD_CLSSETUP_OVERHEAD_FORMAT_GET"
        Dim strOppCd_OverheadSetup_ADD As String = "BD_CLSSETUP_OVERHEAD_ADD"
        Dim strOppCd_OverheadSetup_UPD As String = "BD_CLSSETUP_OVERHEAD_UPD"
        Dim strOppCd_MatureCropSetup_Format_GET As String = "BD_CLSSETUP_MATURECROP_FORMAT_GET"
        Dim strOppCd_MatureCropSetup_ADD As String = "BD_CLSSETUP_MATURECROP_ADD"
        Dim strOppCd_MatureCropSetup_UPD As String = "BD_CLSSETUP_MATURECROP_UPD"
        Dim strOppCd_ImMatureCropSetup_Format_GET As String = "BD_CLSSETUP_UNMATURECROP_FORMAT_GET"
        Dim strOppCd_ImMatureCropSetup_ADD As String = "BD_CLSSETUP_UNMATURECROP_ADD"
        Dim strOppCd_ImMatureCropSetup_UPD As String = "BD_CLSSETUP_UNMATURECROP_UPD"
        Dim strOppCd_NurseryActSetup_Format_GET As String = "BD_CLSSETUP_NURSERYACTIVITY_FORMAT_GET"
        Dim strOppCd_NurseryActSetup_ADD As String = "BD_CLSSETUP_NURSERYACTIVITY_ADD"
        Dim strOppCd_NurseryActSetup_UPD As String = "BD_CLSSETUP_NURSERYACTIVITY_UPD"

        Dim lb As Label
        Dim strStatus As String
        Dim strPeriodID As String
        Dim strNextPeriodID As String
        Dim blnFlgFuture As Boolean = False
        Dim blnFlgActive As Boolean = False
        Dim blnFlgAddVote As Boolean = False
        Dim blnFlgHold As Boolean = False
        Dim intCnt As Integer
        Dim intCntAct As Integer
        Dim intCntFut As Integer
        Dim intCntAV As Integer
        Dim intCntHd As Integer
        Dim intError As Integer

        For intCnt = 0 To PeriodData.Items.Count - 1
            lb = PeriodData.Items(intCnt).FindControl("lblStatus")
            strStatus = Trim(lb.Text)

            If strStatus = objBD.EnumPeriodStatus.Future Then
                blnFlgFuture = True
                intCntFut = intCnt
            End If

            If strStatus = objBD.EnumPeriodStatus.Active Then
                blnFlgActive = True
                intCntAct = intCnt
            End If

            If strStatus = objBD.EnumPeriodStatus.AddVote Then
                blnFlgAddVote = True
                intCntAV = intCnt
            End If
            If strStatus = objBD.EnumPeriodStatus.Hold Then
                blnFlgHold = True
                intCntHd = intCnt
            End If
        Next


        If blnFlgFuture And blnFlgActive Then
            lb = PeriodData.Items(intCntAct).FindControl("lblPerID")
            strPeriodID = Trim(lb.Text)
            lb = PeriodData.Items(intCntAct + 1).FindControl("lblPerID")
            strNextPeriodID = Trim(lb.Text)

            strParam = strPeriodID & "|" & strNextPeriodID
            Try
                intErrNo = objBD.mtdCreateNewTemplate(strOppCd_VehRunSetup_Format_GET, _
                                                    strOppCd_VehRunSetup_ADD, _
                                                    strOppCd_VehRunSetup_UPD, _
                                                    strOppCd_OverheadSetup_Format_GET, _
                                                    strOppCd_OverheadSetup_ADD, _
                                                    strOppCd_OverheadSetup_UPD, _
                                                    strOppCd_MatureCropSetup_Format_GET, _
                                                    strOppCd_MatureCropSetup_ADD, _
                                                    strOppCd_MatureCropSetup_UPD, _
                                                    strOppCd_ImMatureCropSetup_Format_GET, _
                                                    strOppCd_ImMatureCropSetup_ADD, _
                                                    strOppCd_ImMatureCropSetup_UPD, _
                                                    strOppCd_NurseryActSetup_Format_GET, _
                                                    strOppCd_NurseryActSetup_ADD, _
                                                    strOppCd_NurseryActSetup_UPD, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strParam, _
                                                    intError)
            Catch Exp As System.Exception
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_CREATE_NEW_TEMPLATE&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_Periods.aspx")
            End Try

            If Not intError = objBD.EnumErrorType.AddFailed Then

                strParam = strPeriodID & "||||||" & objBD.EnumPeriodStatus.Passed & "|"
                Try
                    intErrNo = objBD.mtdUpdBGTPeriod(strOppCd_ADD, _
                                                    strOppCd_UPD, _
                                                    strOppCd_GET, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strAccMonth, _
                                                    strAccYear, _
                                                    strParam, _
                                                    intError)
                Catch Exp As System.Exception
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=UPD_BUDGETPERIODS_CLOSE&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_Periods.aspx")
                End Try


                strParam = strNextPeriodID & "||||||" & objBD.EnumPeriodStatus.Active & "|"
                Try
                    intErrNo = objBD.mtdUpdBGTPeriod(strOppCd_ADD, _
                                                    strOppCd_UPD, _
                                                    strOppCd_GET, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strAccMonth, _
                                                    strAccYear, _
                                                    strParam, _
                                                    intError)
                Catch Exp As System.Exception
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=UPD_BUDGETPERIODS_CLOSE&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_Periods.aspx")
                End Try
            Else
                lblErrCopyTemp.Visible = True
                Exit Sub
            End If

        ElseIf blnFlgAddVote And blnFlgHold Then
            lb = PeriodData.Items(intCntAct).FindControl("lblPerID")
            strPeriodID = Trim(lb.Text)
            lb = PeriodData.Items(intCntAct + 1).FindControl("lblPerID")
            strNextPeriodID = Trim(lb.Text)

            strParam = strPeriodID & "||||||" & objBD.EnumPeriodStatus.Passed & "|"
            Try
                intErrNo = objBD.mtdUpdBGTPeriod(strOppCd_ADD, _
                                                strOppCd_UPD, _
                                                strOppCd_GET, _
                                                strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strAccMonth, _
                                                strAccYear, _
                                                strParam, _
                                                intError)
            Catch Exp As System.Exception
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=UPD_BUDGETPERIODS_CLOSE&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_Periods.aspx")
            End Try


            strParam = strNextPeriodID & "||||||" & objBD.EnumPeriodStatus.Active & "|"
            Try
                intErrNo = objBD.mtdUpdBGTPeriod(strOppCd_ADD, _
                                                strOppCd_UPD, _
                                                strOppCd_GET, _
                                                strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strAccMonth, _
                                                strAccYear, _
                                                strParam, _
                                                intError)
            Catch Exp As System.Exception
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=UPD_BUDGETPERIODS_CLOSE&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_Periods.aspx")
            End Try


        ElseIf Not blnFlgActive Then


        Else
            lblErrClose.Visible = True
            Exit Sub
        End If

        BindGrid()
    End Sub

    Sub DEDR_Update(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim EditText As TextBox
        Dim label As Label
        Dim list As DropDownList
        Dim lblMsg As Label
        Dim intError As Integer
        Dim PeriodID As String
        Dim BGTPeriod As String
        Dim StartMonth As String
        Dim strBgtYear As String
        Dim EndMonth As String

        list = E.Item.FindControl("ddlYear")
        strBgtYear = list.SelectedItem.Value.Trim
        Dim intMaxperiod As Integer = GetPlantwarePeriod(strBgtYear).Tables(0).Rows(0).Item("MaxPeriod")

        label = E.Item.FindControl("lblPeriodID")
        PeriodID = label.Text
        EditText = E.Item.FindControl("txtBGTPeriod")
        BGTPeriod = EditText.Text
        EditText = E.Item.FindControl("txtStartMonth")
        StartMonth = EditText.Text
        EditText = E.Item.FindControl("txtEndMonth")
        EndMonth = EditText.Text






        strParam = PeriodID & "|" & _
                    BGTPeriod & "|" & _
                    StartMonth & "|" & _
                    strBgtYear & "|" & _
                    EndMonth & "|" & _
                    strBgtYear & "||" & _
                    intMaxperiod.ToString

        Try
            intErrNo = objBD.mtdUpdBGTPeriod(strOppCd_ADD, _
                                            strOppCd_UPD, _
                                            strOppCd_GET, _
                                            strCompany, _
                                            strLocation, _
                                            strUserId, _
                                            strAccMonth, _
                                            strAccYear, _
                                            strParam, _
                                            intError)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=UPD_BUDGETPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_Periods.aspx")
        End Try

        If intError = objBD.EnumErrorType.InsufficientQty Then
            lblMsg = E.Item.FindControl("lblPeriodErr")
            lblMsg.Visible = True
        Else
            PeriodData.EditItemIndex = -1
            BindGrid()
        End If
    End Sub

    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        If CInt(E.Item.ItemIndex) = 0 And PeriodData.Items.Count = 1 And PeriodData.PageCount <> 1 Then
            PeriodData.CurrentPageIndex = PeriodData.PageCount - 2
        End If
        PeriodData.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim intError As Integer
        Dim Label As Label
        Dim PeriodID As String
        Dim Status As String

        Label = E.Item.FindControl("lblPeriodID")
        PeriodID = Label.Text
        Label = E.Item.FindControl("lblStatus")
        Status = Label.Text

        If Status = objBD.EnumPeriodStatus.Future Then

            Try
                intErrNo = objBD.mtdDelBGTPeriod(strOppCd_DEL, _
                                                    PeriodID, _
                                                    intError)
            Catch Exp As System.Exception
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=UPD_BUDGETPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_Periods.aspx")
            End Try
        ElseIf Status = objBD.EnumPeriodStatus.Deleted Or Status = objBD.EnumPeriodStatus.Passed Then

            If Status = objBD.EnumPeriodStatus.Passed Then
                strParam = PeriodID & "||||||" & objBD.EnumPeriodStatus.Deleted & "|"
            ElseIf Status = objBD.EnumPeriodStatus.Deleted Then
                strParam = PeriodID & "||||||" & objBD.EnumPeriodStatus.Passed & "|"
            End If
            Try
                intErrNo = objBD.mtdUpdBGTPeriod(strOppCd_ADD, _
                                                        strOppCd_UPD, _
                                                        strOppCd_GET, _
                                                        strCompany, _
                                                        strLocation, _
                                                        strUserId, _
                                                        strAccMonth, _
                                                        strAccYear, _
                                                        strParam, _
                                                        intError)
            Catch Exp As System.Exception
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=UPD_BUDGETPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_Periods.aspx")
            End Try

        End If

        PeriodData.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub DEDR_Add(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim dataSet As DataSet = LoadData()
        Dim newRow As DataRow
        Dim Updbutton As LinkButton
        Dim ddl As DropDownList
        Dim lbl As Label
        Dim validator As RequiredFieldValidator

        blnUpdate.Text = False
        newRow = dataSet.Tables(0).NewRow()
        newRow.Item("PeriodID") = 0
        newRow.Item("BGTPeriod") = ""
        newRow.Item("StartMonth") = ""
        newRow.Item("StartYear") = ""
        newRow.Item("EndMonth") = ""
        newRow.Item("EndYear") = ""
        newRow.Item("Status") = 1
        newRow.Item("CreateDate") = DateTime.Now()
        newRow.Item("UpdateDate") = DateTime.Now()
        newRow.Item("UserName") = ""
        dataSet.Tables(0).Rows.Add(newRow)

        PeriodData.DataSource = dataSet
        PeriodData.DataBind()

        PeriodData.CurrentPageIndex = PeriodData.PageCount - 1
        PeriodData.DataBind()
        PeriodData.EditItemIndex = PeriodData.Items.Count - 1
        PeriodData.DataBind()

        If PeriodData.EditItemIndex > 0 Then
            lbl = PeriodData.Items.Item(CInt(PeriodData.EditItemIndex) - 1).FindControl("lblEndMonth")
            lblStartMthCtrl.Text = lbl.Text
            lblEndMthCtrl.Text = 0
        Else
            lblStartMthCtrl.Text = 0
            lblStartYrCtrl.Text = 0
            lblEndMthCtrl.Text = 0
            lblEndYrCtrl.Text = 0

        End If
        ddl = PeriodData.Items.Item(CInt(PeriodData.EditItemIndex)).FindControl("ddlYear")
        Bind_PeriodList("", ddl)

        Updbutton = PeriodData.Items.Item(CInt(PeriodData.EditItemIndex)).FindControl("Delete")
        Updbutton.Visible = False

    End Sub

    Sub btnAddVote_Click(ByVal Sender As Object, ByVal E As EventArgs)
        Dim strActiveID As String = Sender.CommandArgument
        Dim strAddVoteID As String = Sender.CommandName

        Dim intError As Integer
        strParam = strActiveID & "||||||" & objBD.EnumPeriodStatus.Hold & "|"

        Try
            intErrNo = objBD.mtdUpdBGTPeriod(strOppCd_ADD, _
                                            strOppCd_UPD, _
                                            strOppCd_GET, _
                                            strCompany, _
                                            strLocation, _
                                            strUserId, _
                                            strAccMonth, _
                                            strAccYear, _
                                            strParam, _
                                            intError)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=UPD_BUDGETPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_Periods.aspx")
        End Try

        strParam = strAddVoteID & "||||||" & objBD.EnumPeriodStatus.AddVote & "|"

        Try
            intErrNo = objBD.mtdUpdBGTPeriod(strOppCd_ADD, _
                                            strOppCd_UPD, _
                                            strOppCd_GET, _
                                            strCompany, _
                                            strLocation, _
                                            strUserId, _
                                            strAccMonth, _
                                            strAccYear, _
                                            strParam, _
                                            intError)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=UPD_BUDGETPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_Periods.aspx")
        End Try
        BindGrid()

    End Sub

End Class
