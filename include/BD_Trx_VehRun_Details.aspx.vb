
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


Public Class BD_VehRun_Det : Inherits Page

    Protected WithEvents dgVehicle As DataGrid
    Protected WithEvents ErrorMessage As Label
    Protected WithEvents SQLStatement As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents StatusList As DropDownList
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents blnUpdate As Label
    Protected WithEvents curStatus As Label
    Protected WithEvents lblVeh As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblOvrMsgTop As Label
    Protected WithEvents lblOvrMsg As Label
    Protected WithEvents lblVehType As Label
    Protected WithEvents lblCode As Label

    Protected WithEvents lblVehCode As Label
    Protected WithEvents lblDesc As Label
    Protected WithEvents lblPDate As Label
    Protected WithEvents lblHPCC As Label
    Protected WithEvents lblModel As Label
    Protected WithEvents lblUOM As Label
    Protected WithEvents lblVehTag As Label
    Protected WithEvents lblDescTag As Label
    Protected WithEvents lblLocTag As Label
    Protected WithEvents lblVRATitle As Label
    Protected WithEvents lblExpenditure As Label
    Protected WithEvents lbtn_Recalc As Button
    Protected WithEvents lblBgtStatus As Label 

    Dim objBDTx As New agri.BD.clsTrx()
    Dim objBD As New agri.BD.clsSetup()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Dim strOpCd_Format_GET As String = "BD_CLSSETUP_VEHRUNNING_FORMAT_GET"
    Dim strOpCd_VehRun_Add As String = "BD_CLSTRX_VEHRUNNING_ADD"
    Dim strOpCd_VehRun_UPD As String = "BD_CLSTRX_VEHRUNNING_UPD"
    Dim strOpCd_Formula_GET As String = "BD_CLSTRX_CALCFORMULA_GET"
    Dim strOpCd_VehRun_GET As String = "BD_CLSTRX_VEHRUNNING_GET"

    Dim objDataSet As New DataSet()
    Dim objLangCapDs As New DataSet()
    Dim intErrNo As Integer
    Dim strParam As String = ""

    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim intADAR As Integer
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strValidateCode As String
    Dim strvalidateDesc As String
    Dim strDateFMT As String
    Dim strLocType as String

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        intADAR = Session("SS_ADAR")
        strAccMonth = Session("SS_GLACCMONTH")
        strAccYear = Session("SS_GLACCYEAR")
        strDateFMT = Session("SS_DATEFMT")
        strLocType = Session("SS_LOCTYPE")
        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumADAccessRights.ADBudgeting), intADAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")

        Else
            onload_GetLangCap()
            If Not Page.IsPostBack Then
                lblVeh.Text = Request.QueryString("Veh")
                BindGrid()
                Recalc_Formula()
            End If
            If SortExpression.Text = "" Then
                SortExpression.Text = "BlkCode"
                SortCol.Text = "ASC"
            End If
        End If
        lblOvrMsgTop.Visible = False
        lblOvrMsg.Visible = False
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblVRATitle.Text = "VEHICLE RUNNING"
        lblLocTag.Text = GetCaption(objLangCap.EnumLangCap.Location) & lblCode.Text
        lblVehTag.Text = GetCaption(objLangCap.EnumLangCap.Vehicle) & lblCode.Text
        lblDescTag.Text = GetCaption(objLangCap.EnumLangCap.VehicleDesc)
        dgVehicle.Columns(1).HeaderText = GetCaption(objLangCap.EnumLangcap.VehExpense) 'lblExpenditure.Text"
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BD_TRX_VEHRUN_DETAILS_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_VehRun_List.aspx?typ=" & lblVehType.Text)
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
        dgVehicle.CurrentPageIndex = 0
        dgVehicle.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub BindGrid()
        Dim PageNo As Integer
        Dim Period As String

        dgVehicle.DataSource = LoadData()
        dgVehicle.DataBind()
        lblLocCode.Text = strLocation
        GetActivePeriod(Period)
        lblBgtPeriod.Text = Period
        LoadVehicle()
    End Sub


    Protected Function LoadData() As DataSet
        Dim Period As String

        strParam = lblVeh.Text & "||" & _
                    strLocation & "|" & _
                    GetActivePeriod("") & "||stp.DispSeq Asc"

        Try
            intErrNo = objBDTx.mtdGetVehRunning(strOpCd_VehRun_GET, strParam, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_VehRun_List.aspx?typ=" & lblVehType.Text)
        End Try
        Return objDataSet
    End Function

    Protected Sub LoadVehicle()
        Dim strOpCd As String = "GL_CLSSETUP_VEHICLE_GET_BY_VEHCODE"
        Dim intErrNo As Integer
        Dim objGLSetup As New agri.GL.clsSetup()

        Dim dsVehicle As DataSet

        Try
            intErrNo = objGLSetup.mtdGetVehicle(strOpCd, _
                                                strLocation, _
                                                lblVeh.Text.Trim, _
                                                dsVehicle, _
                                                True)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=VEHICLE_GET_BY_VEHCODE&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        lblVehCode.Text = Trim(dsVehicle.Tables(0).Rows(0).Item("VehCode"))
        lblDesc.Text = Trim(dsVehicle.Tables(0).Rows(0).Item("Description"))
        lblModel.Text = Trim(dsVehicle.Tables(0).Rows(0).Item("Model"))
        lblHPCC.Text = Trim(dsVehicle.Tables(0).Rows(0).Item("HPCC"))
        lblUOM.Text = Trim(dsVehicle.Tables(0).Rows(0).Item("UOMCode"))
        lblPDate.Text = objGlobal.GetShortDate(strDateFMT, Trim(dsVehicle.Tables(0).Rows(0).Item("PurchaseDate")))

    End Sub

    Protected Function GetActivePeriod(ByRef BGTPeriod As String) As String
        Dim strOppCd_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"
        Dim dsperiod As New DataSet()

        strParam = "|||||" & objBD.EnumPeriodStatus.Active & "|" & strLocation & "|"

        Try
            intErrNo = objBD.mtdGetPeriodList(strOppCd_GET, strParam, dsperiod)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_VehRun_List.aspx?typ=" & lblVehType.Text)
        End Try
        If dsperiod.Tables(0).Rows.Count > 0 Then
            BGTPeriod = dsperiod.Tables(0).Rows(0).Item("BGTPeriod") & " - (" & objBD.mtdGetPeriodStatus(dsperiod.Tables(0).Rows(0).Item("Status")) & ")"
            lblBgtStatus.Text = dsperiod.Tables(0).Rows(0).Item("Status") 
            Return dsperiod.Tables(0).Rows(0).Item("PeriodID")
        Else
            BGTPeriod = "No Active Period"
            Response.Redirect("../../BD/Setup/BD_setup_Periods.aspx")
        End If
    End Function

    Sub DataGrid_ItemDataBound(ByVal Sender As Object, ByVal e As DataGridItemEventArgs)
        Dim lbl As Label
        Dim btn As LinkButton
        Dim txt As TextBox
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            lbl = e.Item.FindControl("lblIdx")
            lbl.Text = e.Item.ItemIndex.ToString + 1

            lbl = e.Item.FindControl("lblDispType")
            Select Case lbl.Text
                Case objBD.EnumBudgetFormatItem.Header
                    e.Item.Cells(0).Font.Bold = True
                    e.Item.CssClass = "mr-r"

                    lbl = e.Item.FindControl("lblItem")
                    lbl.Font.Bold = True
                    lbl = e.Item.FindControl("lblQty")
                    lbl.Visible = False
                    lbl = e.Item.FindControl("lblCost")
                    lbl.Visible = False
                    btn = e.Item.FindControl("Edit")
                    btn.Visible = False

                Case objBD.EnumBudgetFormatItem.Entry
                    lbl = e.Item.FindControl("lblDispCol")
                    Select Case lbl.Text
                        Case objBD.EnumBudgetItemColumn.Unit
                            lbl = e.Item.FindControl("lblCost")
                            lbl.Visible = False
                            lbl = e.Item.FindControl("lblAddVote")
                            lbl.Visible = False
                            If lblBgtStatus.Text.Trim = objBD.EnumPeriodStatus.Addvote Then
                                btn = e.Item.FindControl("Edit")
                                btn.Visible = False
                            End If

                        Case objBD.EnumBudgetItemColumn.Cost
                            lbl = e.Item.FindControl("lblQty")
                            lbl.Visible = False
                    End Select

                Case objBD.EnumBudgetFormatItem.Formula
                    e.Item.CssClass = "mr-l"
                    e.Item.Font.Bold = True
                    lbl = e.Item.FindControl("lblItem")
                    lbl.Font.Bold = True
                    lbl = e.Item.FindControl("lblAddVote")
                    lbl.Visible = False
                    If lblBgtStatus.Text.Trim = objBD.EnumPeriodStatus.Addvote Then
                        btn = e.Item.FindControl("Edit")
                        btn.Visible = False
                    End If

                    lbl = e.Item.FindControl("lblDispCol")
                    Select Case lbl.Text
                        Case objBD.EnumBudgetItemColumn.Unit
                            lbl = e.Item.FindControl("lblCost")
                            lbl.Visible = False
                            lbl = e.Item.FindControl("lblQty")
                            lbl.Font.Bold = True
                        Case objBD.EnumBudgetItemColumn.Cost
                            lbl = e.Item.FindControl("lblQty")
                            lbl.Visible = False
                            lbl = e.Item.FindControl("lblCost")
                            lbl.Font.Bold = True
                    End Select

                Case objBD.EnumBudgetFormatItem.Total
                    e.Item.CssClass = "mr-l"
                    e.Item.Font.Bold = True
                    lbl = e.Item.FindControl("lblAddVote")  
                    lbl.Visible = False
                    lbl = e.Item.FindControl("lblItem")
                    lbl.Font.Bold = True
                    lbl = e.Item.FindControl("lblQty")
                    lbl.Font.Bold = True
                    lbl = e.Item.FindControl("lblCost")
                    lbl.Font.Bold = True
                Case Else
                    e.Item.CssClass = "mr-l"

            End Select
        ElseIf e.Item.ItemType = ListItemType.EditItem Then
            lbl = e.Item.FindControl("lblIdx")
            lbl.Text = e.Item.ItemIndex.ToString + 1

            e.Item.Cells(0).Font.Bold = True
            e.Item.Cells(1).Font.Bold = True
            e.Item.Cells(2).Font.Bold = True

            lbl = e.Item.FindControl("lblDispType")
            Select Case lbl.Text
                Case objBD.EnumBudgetFormatItem.Entry
                    lbl = e.Item.FindControl("lblItem")
                    lbl.Font.Bold = True

                    lbl = e.Item.FindControl("lblDispCol")
                    Select Case lbl.Text
                        Case objBD.EnumBudgetItemColumn.Unit
                            txt = e.Item.FindControl("txtCost")
                            txt.Visible = False
                            txt = e.Item.FindControl("txtAddVote")
                            txt.Visible = False
                        Case objBD.EnumBudgetItemColumn.Cost
                            txt = e.Item.FindControl("txtQty")
                            txt.Visible = False
                            If lblBgtStatus.Text.Trim = objBD.EnumPeriodStatus.Addvote Then
                                txt = e.Item.FindControl("txtCost")
                                txt.Visible = False
                            Else
                                txt = e.Item.FindControl("txtAddVote")
                                txt.Visible = False

                            End If

                    End Select

                Case objBD.EnumBudgetFormatItem.Formula

                    txt = e.Item.FindControl("txtCost")
                    txt.Visible = False
                    txt = e.Item.FindControl("txtQty")
                    txt.Visible = False

                    lbl = e.Item.FindControl("lblDispCol")
                    Select Case lbl.Text
                        Case objBD.EnumBudgetItemColumn.Unit
                            e.Item.Cells(3).Text = "Unit"
                            e.Item.Cells(3).Font.Bold = True
                        Case objBD.EnumBudgetItemColumn.Cost
                            e.Item.Cells(4).Text = "Cost"
                            e.Item.Cells(4).Font.Bold = True
                    End Select
                    txt = e.Item.FindControl("txtAddVote")
                    txt.Visible = False


            End Select

        ElseIf e.Item.ItemType = ListItemType.Header Then
            e.Item.HorizontalAlign = HorizontalAlign.Center
            e.Item.Font.Bold = True

        End If

    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIf(SortCol.Text = "ASC", "DESC", "ASC")
        BindGrid()
    End Sub

    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        dgVehicle.EditItemIndex = CInt(E.Item.ItemIndex)
        BindGrid()
    End Sub

    Sub DEDR_Update(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim EditText As TextBox
        Dim label As label
        Dim list As DropDownList
        Dim lblMsg As label
        Dim intError As Integer
        Dim intEdit As Integer
        Dim strSetID As String
        Dim strQty As String
        Dim strCost As String
        Dim strAddVote As String
        Dim strDisp As String
        Dim strcol As String


        label = E.Item.FindControl("lblSetID")
        strSetID = label.Text
        label = E.Item.FindControl("lblDispType")
        strDisp = label.Text
        label = E.Item.FindControl("lblDispCol")
        strcol = label.Text
        EditText = E.Item.FindControl("txtQty")
        strQty = EditText.Text
        EditText = E.Item.FindControl("txtCost")
        strCost = EditText.Text
        EditText = E.Item.FindControl("txtAddVote")
        strAddVote = EditText.Text

        strParam = lblVeh.Text & "|" & GetActivePeriod("") & "|" & strSetID & "|" & strQty & "|" & strCost & "|" & objBDTx.EnumVehRunningStatus.Budgeted & "|" & strDisp & "|" & strcol & "|" & strAddVote

        Try
            intErrNo = objBDTx.mtdUpdVehRunning(strOpCd_VehRun_GET, _
                                              strOpCd_VehRun_Add, _
                                              strOpCd_VehRun_UPD, _
                                              strOpCd_Format_GET, _
                                              strOpCd_Formula_GET, _
                                              strCompany, _
                                              strLocation, _
                                              strUserId, _
                                              strParam, _
                                              objBDTx.EnumOperation.Update, _
                                              intError)

        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=Veh_Update&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_VehRun_List.aspx?typ=" & lblVehType.Text)
        End Try

        If intError = objBDTx.EnumErrorType.CalculationErr Then
            lblOvrMsgTop.Visible = True
            lblOvrMsg.Visible = True
        Else
            For intEdit = E.Item.ItemIndex + 1 To dgVehicle.Items.Count - 1
                label = dgVehicle.Items.Item(CInt(intEdit)).FindControl("lblDispType")
                If lblBgtStatus.Text.Trim = objBD.EnumPeriodStatus.Addvote Then
                    If label.Text.Trim = objBD.EnumBudgetFormatItem.Entry Then
                        Exit For
                    End If
                Else
                    If label.Text.Trim <> objBD.EnumBudgetFormatItem.Header Then
                        Exit For
                    End If
                End If
            Next
            dgVehicle.EditItemIndex = intEdit
            BindGrid()
        End If

    End Sub

    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        If CInt(E.Item.ItemIndex) = 0 And dgVehicle.Items.Count = 1 And dgVehicle.PageCount <> 1 Then
            dgVehicle.CurrentPageIndex = dgVehicle.PageCount - 2
        End If
        dgVehicle.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub CallRecalc_Formula(ByVal Sender As Object, ByVal E As EventArgs) Handles lbtn_Recalc.Click
        Recalc_Formula()
    End Sub

    Sub Recalc_Formula()

        Dim intError As Integer

        strParam = lblVeh.Text & "||" & _
                    strLocation & "|" & _
                    GetActivePeriod("") & "||BD.VehRunSetID Asc"
        Try
            intErrNo = objBDTx.mtdUpdVehRunningFormula(strParam, _
                                                        strOpCd_VehRun_GET, _
                                                        strOpCd_VehRun_Add, _
                                                        strOpCd_VehRun_UPD, _
                                                        strOpCd_Format_GET, _
                                                        strOpCd_Formula_GET, _
                                                        strCompany, _
                                                        strLocation, _
                                                        strUserId, _
                                                        objBDTx.EnumOperation.Update, _
                                                        intError)

            If intError = objBDTx.EnumErrorType.CalculationErr Then
                lblOvrMsgTop.Visible = True
                lblOvrMsg.Visible = True
            End If

        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_OVERHEAD_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_PlantationOH_Details.aspx")
        End Try

        BindGrid()

    End Sub

End Class
