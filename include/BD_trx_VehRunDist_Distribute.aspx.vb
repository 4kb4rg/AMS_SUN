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
Imports System.Math


Public Class BD_VehRunDist_Dist : Inherits Page

    Protected WithEvents MonthList As DataGrid
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblPrcntErrTop As Label
    Protected WithEvents lblPrcntErr As Label
    Protected WithEvents lblFigureErrTop As Label
    Protected WithEvents lblFigureErr As Label
    Protected WithEvents lblOvrMsgTop As Label
    Protected WithEvents lblOvrMsg As Label
    Protected WithEvents lblCode As Label

    Protected WithEvents lblVehRunDistID As Label
    Protected WithEvents lblTitle As Label
    Protected WithEvents ddlDistribute As DropDownList
    Protected WithEvents lblVehTag As Label
    Protected WithEvents lblVehCode As Label
    Protected WithEvents lblLocTag As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblDistFig As Label

    Dim objBD As New agri.BD.clsTrx()
    Dim objBDSetup As New agri.BD.clsSetup()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl() 
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Dim strOppCd_VehRunDist_UPD As String = "BD_CLSTRX_VEHRUNDIST_UPD"
    Dim strOppCd_VehRunDist_AccPeriod_UPD As String = "BD_CLSTRX_VEHRUNDIST_ACCPERIOD_UPD"
    Dim strOppCd_VehRunDist_YrBudgetCost_GET As String = "BD_CLSTRX_VEHRUNDIST_YRBUDGETCOST_GET"

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

            lblOvrMsgTop.Visible = False
            lblOvrMsg.Visible = False
            lblPrcntErrTop.Visible = False
            lblPrcntErr.Visible = False
            lblFigureErrTop.Visible = False
            lblFigureErr.Visible = False

            If Not Page.IsPostBack Then
                lblVehCode.Text = Request.QueryString("vehcode")
                lblVehRunDistID.Text = Request.QueryString("id")

                lblDistFig.Text = objGlobal.GetIDDecimalSeparator(FormatNumber(Request.QueryString("cost"),0))
                onload_GetLangCap()
                BindDistList()
                BindGrid()
            End If
        End If

    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblTitle.Text = "VEHICLE RUNNING DISTRIBUTION"
        lblLocTag.Text = GetCaption(objLangCap.EnumLangCap.Location) & lblCode.Text
        lblVehTag.Text = GetCaption(objLangCap.EnumLangCap.Vehicle) & lblCode.Text
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BD_TRX_VEHRUNDIST_DIST_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_VehRunDist_Details.aspx")
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


    Sub BindDistList()
        ddlDistribute.Items.Add(New ListItem("Please select a method", ""))
        ddlDistribute.Items.Add(New ListItem(objBD.mtdGetDistMethod(objBD.EnumDistMethod.Even), objBD.EnumDistMethod.Even))
        ddlDistribute.Items.Add(New ListItem(objBD.mtdGetDistMethod(objBD.EnumDistMethod.Figure), objBD.EnumDistMethod.Figure))
        ddlDistribute.Items.Add(New ListItem(objBD.mtdGetDistMethod(objBD.EnumDistMethod.Percentage), objBD.EnumDistMethod.Percentage))

    End Sub

    Sub BindGrid()
        Dim PageNo As Integer
        Dim Period As String

        MonthList.DataSource = LoadData()
        MonthList.DataBind()
        lblLocCode.Text = strLocation
        GetActivePeriod(Period)
        lblBgtPeriod.Text = Period

    End Sub

    Protected Function LoadData() As DataSet
        Dim strOppCd_VehRunDist_AccPeriod_GET As String = "BD_CLSTRX_VEHRUNDIST_ACCPERIOD_GET"

        strParam = "|" & strLocation & "|" & GetActivePeriod("") & "|||"
        Try
            intErrNo = objBD.mtdGetVehRunningDist(strOppCd_VehRunDist_AccPeriod_GET, strParam, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_VEHRUNDIST&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_VehRunDist_Details.aspx")
        End Try

        Return objDataSet
    End Function


    Protected Function GetActivePeriod(ByRef BGTPeriod As String) As String
        Dim strOppCd_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"
        Dim dsperiod As New DataSet()


        strParam = "|||||" & objBDSetup.EnumPeriodStatus.Active & "|" & strLocation & "|"

        Try
            intErrNo = objBDSetup.mtdGetPeriodList(strOppCd_GET, strParam, dsperiod)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_VEHRUNDIST_GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_VehRunDist_Details.aspx")
        End Try

        If dsperiod.Tables(0).Rows.Count > 0 Then
            BGTPeriod = dsperiod.Tables(0).Rows(0).Item("BGTPeriod")
            Return dsperiod.Tables(0).Rows(0).Item("PeriodID")
        Else
            BGTPeriod = "No Active Period"
            Response.Redirect("../../BD/Setup/BD_setup_Periods.aspx")
        End If
    End Function

    Sub ddlDistributeSelect(ByVal Sender As Object, ByVal E As EventArgs)
        Dim txt As TextBox
        Dim decEvenFig As Decimal
        Dim decCount As Decimal
        Dim intcnt As Integer

        LoadData()
        Select Case ddlDistribute.SelectedItem.Value
            Case objBD.EnumDistMethod.Even
                decEvenFig = lblDistFig.Text / objDataSet.Tables(0).Rows.Count
                For intcnt = 0 To objDataSet.Tables(0).Rows.Count - 1
                    If intcnt = objDataSet.Tables(0).Rows.Count - 1 Then
                        decEvenFig = lblDistFig.Text - decCount
                    End If
                    txt = MonthList.Items.Item(CInt(intcnt)).FindControl("TxFig")

                    txt.Text = Round(decEvenFig, 0)
                    decCount += Round(decEvenFig, 0)
                    txt.Enabled = False
                Next
            Case objBD.EnumDistMethod.Percentage, objBD.EnumDistMethod.Figure
                For intcnt = 0 To objDataSet.Tables(0).Rows.Count - 1
                    txt = MonthList.Items.Item(CInt(intcnt)).FindControl("TxFig")
                    txt.Text = ""
                    txt.Enabled = True
                Next
        End Select
    End Sub

    Sub btnConfirm_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim strOpCd_Formula_GET As String = "BD_CLSTRX_CALCFORMULA_GET"
        Dim strOppCd_VehRunSetup_GET As String = "BD_CLSSETUP_VEHRUNNING_FORMAT_GET"
        Dim strOppCd_VehRunDist_AccPeriod_GET As String = "BD_CLSTRX_VEHRUNDIST_ACCPERIOD_GET"
        Dim strOppCd_VehRunDist_AccPeriod_List_GET As String = "BD_CLSTRX_VEHRUNDIST_ACCPERIOD_LIST_GET"
        Dim interror As Integer
        Dim strParam As String
        Dim decFig As Decimal
        Dim decFigCtrl As Decimal
        Dim intCnt As Integer

        Dim txt As TextBox
        Dim lblMonth As Label
        Dim lblYear As Label

        LoadData()

        For intCnt = 0 To objDataSet.Tables(0).Rows.Count - 1
            txt = MonthList.Items.Item(CInt(intCnt)).FindControl("TxFig")
            decFigCtrl += txt.Text
        Next

        Select Case ddlDistribute.SelectedItem.Value
            Case objBD.EnumDistMethod.Figure
                If Not decFigCtrl = CDec(lblDistFig.Text) Then
                    lblFigureErrTop.Visible = True
                    lblFigureErr.Visible = True
                    Exit Sub
                End If
            Case objBD.EnumDistMethod.Percentage
                If Not decFigCtrl = 100 Then
                    lblPrcntErrTop.Visible = True
                    lblPrcntErr.Visible = True
                    Exit Sub
                End If
        End Select

        For intCnt = 0 To objDataSet.Tables(0).Rows.Count - 1
            txt = MonthList.Items.Item(CInt(intCnt)).FindControl("TxFig")
            lblMonth = MonthList.Items.Item(CInt(intCnt)).FindControl("lblMonth")
            lblYear = MonthList.Items.Item(CInt(intCnt)).FindControl("lblYear")

            Select Case ddlDistribute.SelectedItem.Value
                Case objBD.EnumDistMethod.Even, objBD.EnumDistMethod.Figure
                    strParam = GetActivePeriod("") & "|" & lblVehRunDistID.Text & "|" & txt.Text.Trim & "||" & lblDistFig.Text.Trim & "|" & lblMonth.Text.Trim & "|" & lblYear.Text.Trim & "||"
                Case objBD.EnumDistMethod.Percentage
                    strParam = GetActivePeriod("") & "|" & lblVehRunDistID.Text & "||" & txt.Text.Trim & "|" & lblDistFig.Text.Trim & "|" & lblMonth.Text.Trim & "|" & lblYear.Text.Trim & "||"
            End Select

            Try
                intErrNo = objBD.mtdUpdVehRunningDist(strOppCd_VehRunDist_AccPeriod_UPD, _
                                                      strOppCd_VehRunDist_UPD, _
                                                      strOppCd_VehRunDist_YrBudgetCost_GET, _
                                                      strOppCd_VehRunSetup_GET, _
                                                      strOppCd_VehRunDist_AccPeriod_GET, _
                                                      strOppCd_VehRunDist_AccPeriod_List_GET, _
                                                      strOpCd_Formula_GET, _
                                                      strCompany, _
                                                      strLocation, _
                                                      strUserId, _
                                                      strParam, _
                                                      interror)
            Catch Exp As System.Exception
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_VEHRUNDIST_CONFIRM&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_VehRunDist_Details.aspx")
            End Try

            If interror = objBDSetup.EnumErrorType.Overflow Then
                lblOvrMsgTop.Visible = True
                lblOvrMsg.Visible = True
            End If
        Next

        If intErrNo = 0 Then
            Response.Write("<Script Language=""JavaScript"">opener.location.href='BD_trx_VehRunDist_Details.aspx?veh=" & lblVehCode.Text & "';window.close();</Script>")
        End If

    End Sub

    Sub btnCancel_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Response.Write("<Script Language=""JavaScript"">window.close();</Script>")
    End Sub

End Class
