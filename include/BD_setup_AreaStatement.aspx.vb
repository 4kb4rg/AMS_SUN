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


Public Class BD_AreaStatement : Inherits Page

    Protected WithEvents TitleData As DataGrid
    Protected WithEvents ErrorMessage As Label
    Protected WithEvents lblTracker As Label
    Protected WithEvents SQLStatement As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents StatusList As DropDownList
    Protected WithEvents srchStatusList As DropDownList
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents blnUpdate As Label
    Protected WithEvents curStatus As Label
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
    Protected WithEvents lblPeriodErr As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBgtPeriod As Label

    Protected objBD As New agri.BD.clsSetup()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()

    Dim strOppCd_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"
    Dim strOppCd_ADD As String = "BD_CLSSETUP_BGTPERIOD_ADD"
    Dim strOppCd_UPD As String = "BD_CLSSETUP_BGTPERIOD_UPD"

    Dim objDataSet As New DataSet()
    Dim objLangCapDs As New DataSet()
    Dim intErrNo As Integer
    Dim strParam As String = ""

    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objLoc As New agri.Admin.clsLoc()
    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim intADAR As Integer
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strValidateCode As String
    Dim strvalidateDesc As String
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
            If SortExpression.Text = "" Then
                SortExpression.Text = "BGTPeriod"
                SortCol.Text = "ASC"
            End If
            If Not Page.IsPostBack Then
                BindGrid()
            End If
            lblPeriodErr.Visible = False
        End If
    End Sub

    Sub onload_GetLangCap()


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
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PWSYSTEM_CLSLANGCAP_BUSSTERM_GET&errmesg=" & lblErrMessage.Text & "&redirect=IN/Setup/IN_prodtype.aspx")
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
        Session("MySessionData") = Nothing
        TitleData.CurrentPageIndex = 0
        TitleData.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub BindGrid()
        Dim PageNo As Integer

        TitleData.DataSource = LoadData()
        TitleData.DataBind()
        lblLocCode.Text = strLocation
        lblBgtPeriod.Text = GetActivePeriod.Tables(0).Rows(0).Item("BGTPeriod")
    End Sub


    Protected Function LoadData() As DataSet



        strParam = "|||||" & objBD.EnumPeriodStatus.All & "|" & strLocation & "|"

        Try
            intErrNo = objBD.mtdGetPeriodList(strOppCd_GET, strParam, objDataSet)
        Catch Exp As Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGETPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_Periods.aspx")
        End Try
        Return objDataSet
    End Function

    Protected Function GetActivePeriod() As DataSet



        strParam = "|||||" & objBD.EnumPeriodStatus.Active & "|" & strLocation & "|"

        Try
            intErrNo = objBD.mtdGetPeriodList(strOppCd_GET, strParam, objDataSet)
        Catch Exp As Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_Periods.aspx")
        End Try
        Return objDataSet
    End Function

    Sub btnPrevNext_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).CommandArgument
        Select Case direction
            Case "first"
                TitleData.CurrentPageIndex = 0
            Case "prev"
                TitleData.CurrentPageIndex = _
                    Math.Max(0, TitleData.CurrentPageIndex - 1)
            Case "next"
                TitleData.CurrentPageIndex = _
                    Math.Min(TitleData.PageCount - 1, TitleData.CurrentPageIndex + 1)
            Case "last"
                TitleData.CurrentPageIndex = TitleData.PageCount - 1
        End Select
        lstDropList.SelectedIndex = TitleData.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            TitleData.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        TitleData.CurrentPageIndex = e.NewPageIndex
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

        blnUpdate.Text = True
        TitleData.EditItemIndex = CInt(E.Item.ItemIndex)

        lbl = E.Item.FindControl("lblEndMonth")
        strEndMth = lbl.Text
        lbl = E.Item.FindControl("lblStartMonth")
        strStartMth = lbl.Text

        BindGrid()

        List = TitleData.Items.Item(CInt(E.Item.ItemIndex)).FindControl("ddlStartMonth")
        List.SelectedIndex = CInt(strStartMth) - 1
        List = TitleData.Items.Item(CInt(E.Item.ItemIndex)).FindControl("ddlEndMonth")
        List.SelectedIndex = CInt(strEndMth) - 1


        If E.Item.ItemIndex > 0 Then
            lbl = TitleData.Items.Item(CInt(E.Item.ItemIndex) - 1).FindControl("lblEndMonth")
            lblStartMthCtrl.Text = lbl.Text
            lbl = TitleData.Items.Item(CInt(E.Item.ItemIndex) - 1).FindControl("lblEndYear")
            lblStartYrCtrl.Text = lbl.Text
        Else
            lblStartMthCtrl.Text = 0
            lblStartYrCtrl.Text = 0
        End If

        If E.Item.ItemIndex < TitleData.Items.Count - 1 Then
            lbl = TitleData.Items.Item(CInt(E.Item.ItemIndex) + 1).FindControl("lblStartMonth")
            lblEndMthCtrl.Text = lbl.Text
            lbl = TitleData.Items.Item(CInt(E.Item.ItemIndex) + 1).FindControl("lblStartYear")
            lblEndYrCtrl.Text = lbl.Text
        Else
            lblEndMthCtrl.Text = 0
            lblEndYrCtrl.Text = 0

        End If
    End Sub

    Sub DEDR_Update(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim EditText As TextBox
        Dim label As label
        Dim list As DropDownList
        Dim lblMsg As label
        Dim intError As Integer
        Dim PeriodID As String
        Dim BGTPeriod As String
        Dim StartMonth As String
        Dim StartYear As String
        Dim EndMonth As String
        Dim EndYr As String

        label = E.Item.FindControl("lblPeriodID")
        PeriodID = label.Text
        EditText = E.Item.FindControl("txtBGTPeriod")
        BGTPeriod = EditText.Text
        list = E.Item.FindControl("ddlStartMonth")
        StartMonth = list.SelectedItem.Value
        EditText = E.Item.FindControl("txtStartYear")
        StartYear = EditText.Text
        list = E.Item.FindControl("ddlEndMonth")
        EndMonth = list.SelectedItem.Value
        EditText = E.Item.FindControl("txtEndYr")
        EndYr = EditText.Text


        If CInt(StartYear) < CInt(lblStartYrCtrl.Text) And CInt(lblStartYrCtrl.Text) <> 0 Then
            lblPeriodErr.Visible = True
            Exit Sub
        ElseIf CInt(StartYear) = CInt(lblStartYrCtrl.Text) Then
            If CInt(StartMonth) <= CInt(lblStartMthCtrl.Text) Then
                lblPeriodErr.Visible = True
                Exit Sub
            End If
        End If

        If CInt(EndYr) > CInt(lblEndYrCtrl.Text) And CInt(lblEndYrCtrl.Text) <> 0 Then
            lblPeriodErr.Visible = True
            Exit Sub
        ElseIf CInt(EndYr) = CInt(lblEndYrCtrl.Text) Then
            If CInt(EndMonth) >= CInt(lblEndMthCtrl.Text) Then
                lblPeriodErr.Visible = True
                Exit Sub
            End If
        End If

        If CInt(EndYr) < CInt(StartYear) Then
            lblPeriodErr.Visible = True
            Exit Sub
        ElseIf CInt(EndYr) = CInt(StartYear) Then
            If CInt(StartMonth) >= CInt(EndMonth) Then
                lblPeriodErr.Visible = True
                Exit Sub
            End If
        End If
        strParam = PeriodID & "|" & _
                    BGTPeriod & "|" & _
                    StartMonth & "|" & _
                    StartYear & "|" & _
                    EndMonth & "|" & _
                    EndYr & "|"

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
        Catch Exp As Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=UPD_BUDGETPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_Periods.aspx")
        End Try

        If intError = objBD.EnumErrorType.duplicateKey Then
            lblMsg = E.Item.FindControl("lblDupMsg")
            lblMsg.Visible = True
        Else
            TitleData.EditItemIndex = -1
            BindGrid()
        End If
    End Sub

    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        If CInt(E.Item.ItemIndex) = 0 And TitleData.Items.Count = 1 And TitleData.PageCount <> 1 Then
            TitleData.CurrentPageIndex = TitleData.PageCount - 2
        End If
        TitleData.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)




    End Sub

    Sub DEDR_Add(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim dataSet As dataSet = LoadData()
        Dim newRow As DataRow
        Dim Updbutton As LinkButton
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

        TitleData.DataSource = dataSet
        TitleData.DataBind()

        TitleData.CurrentPageIndex = TitleData.PageCount - 1
        TitleData.DataBind()
        TitleData.EditItemIndex = TitleData.Items.Count - 1
        TitleData.DataBind()

        If TitleData.EditItemIndex > 0 Then
            lbl = TitleData.Items.Item(CInt(TitleData.EditItemIndex) - 1).FindControl("lblEndMonth")
            lblStartMthCtrl.Text = lbl.Text
            lbl = TitleData.Items.Item(CInt(TitleData.EditItemIndex) - 1).FindControl("lblEndYear")
            lblStartYrCtrl.Text = lbl.Text
        Else
            lblStartMthCtrl.Text = 0
            lblStartYrCtrl.Text = 0
            lblEndMthCtrl.Text = 0
            lblEndYrCtrl.Text = 0

        End If

        Updbutton = TitleData.Items.Item(CInt(TitleData.EditItemIndex)).FindControl("Delete")
        Updbutton.Visible = False

    End Sub

End Class
