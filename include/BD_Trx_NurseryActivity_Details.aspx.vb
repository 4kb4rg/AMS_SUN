
Imports System
Imports System.Data
Imports System.Collections 
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic
Imports System.Math 


Public Class BD_NurseryActivity_Det : Inherits Page

    Protected WithEvents dgNurseryActivityDet As DataGrid
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblOvrMsg As Label
    Protected WithEvents lblOvrMsgTop As Label
    Protected WithEvents lblNoRecord As Label
    Protected WithEvents lblNoRecordTop As Label
    Protected WithEvents lbtn_Recalc As Button
    Protected WithEvents lblLocTag As Label
    Protected WithEvents lblBlkTag As Label
    Protected WithEvents lblSubBlkTag As Label
    Protected WithEvents lblBlkCode As Label
    Protected WithEvents lblSubBlkCode As Label
    Protected WithEvents lblTotalSeedFig As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents RowSubBlk As HtmlTableRow
    Protected WithEvents hidBlkCode As HtmlInputHidden
    Protected WithEvents lblBgtStatus As Label 

    Dim objBD As New agri.BD.clsTrx()
    Dim objBDSetup As New agri.BD.clsSetup()
    Dim objGLSetup As New agri.GL.clsSetup()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl() 

    Dim strOppCd_NurseryActivity_Format_GET As String = "BD_CLSTRX_NURSERYACTIVITY_FORMAT_GET"
    Dim strOppCd_NurseryActivitySetup_GET As String = "BD_CLSSETUP_NURSERYACTIVITY_FORMAT_GET"
    Dim strOppCd_NurseryActivity_ADD As String = "BD_CLSTRX_NURSERYACTIVITY_ADD"
    Dim strOppCd_NurseryActivity_UPD As String = "BD_CLSTRX_NURSERYACTIVITY_UPD"
    Dim strOppCd_NurseryActivity_CostPerSeed_SUM As String
    Dim strOpCd_Formula_GET As String = "BD_CLSTRX_CALCNAFORMULA_GET"

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
    Dim intConfigsetting As Integer
    Dim strLocType as String

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        intADAR = Session("SS_ADAR")
        strAccMonth = Session("SS_GLACCMONTH")
        strAccYear = Session("SS_GLACCYEAR")
        intConfigsetting = Session("SS_CONFIGSETTING")
        strLocType = Session("SS_LOCTYPE")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumADAccessRights.ADBudgeting), intADAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")

        Else
            lblOvrMsg.Visible = False
            lblOvrMsgTop.Visible = False
            lblNoRecord.Visible = False
            lblNoRecordTop.Visible = False

            If Not Page.IsPostBack Then
                lblBlkCode.Text = Request.QueryString("blk").Trim
                lblSubBlkCode.Text = Request.QueryString("subblk").Trim

                If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
                    hidBlkCode.Value = lblBlkCode.Text
                Else
                    hidBlkCode.Value = lblSubBlkCode.Text
                End If

                onload_GetLangCap()
                BindGrid()
                GetTotalSeed()
                Recalc_Formula()
                DisableRow()

            End If
        End If
    End Sub

    Sub DisableRow()
        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
            RowSubBlk.Visible = False
        Else
            RowSubBlk.Visible = True
        End If
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()

        lblLocTag.Text = GetCaption(objLangCap.EnumLangCap.Location) & lblCode.Text
        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
            lblBlkTag.Text = GetCaption(objLangCap.EnumLangCap.Block) & lblCode.Text
        Else
            lblBlkTag.Text = GetCaption(objLangCap.EnumLangCap.Block) & lblCode.Text
            lblSubBlkTag.Text = GetCaption(objLangCap.EnumLangCap.SubBlock) & lblCode.Text
        End If

        dgNurseryActivityDet.Columns(1).HeaderText = GetCaption(objLangCap.EnumLangCap.Account) & lblCode.Text
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
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_NURSERYACTIVITY_DET_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_NurseryActivity_Details.aspx")
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


    Sub GetTotalSeed()

        Dim dsTotalSeed As New DataSet()
        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
            strOppCd_NurseryActivity_CostPerSeed_SUM = "BD_CLSTRX_NURSERYACTIVITY_COSTPERSEED_SUM"
            strParam = "|" & objGLSetup.EnumBlockStatus.Active & "|" & strLocation & "|" & hidBlkCode.Value & "|"
        Else
            strOppCd_NurseryActivity_CostPerSeed_SUM = "BD_CLSTRX_NURSERYACTIVITY_COSTPERSEED_SBLK_SUM"
            strParam = "|" & objGLSetup.EnumSubBlockStatus.Active & "|" & strLocation & "|" & hidBlkCode.Value & "|"
        End If

        Try
            intErrNo = objBD.mtdGetNurseryActivityTotalSeed(strOppCd_NurseryActivity_CostPerSeed_SUM, strParam, dsTotalSeed)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_NURSERYACTDET_GET_AREASTMT_TOTALAREA&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_NurseryActivity_Details.aspx")
        End Try

        lblTotalSeedFig.Text = FormatNumber(dsTotalSeed.Tables(0).Rows(0).Item("Qty"), 2)

        lblTotalSeedFig.Text = objGlobal.GetIDDecimalSeparator(FormatNumber(dsTotalSeed.Tables(0).Rows(0).Item("Qty"), 0))

    End Sub

    Sub BindGrid()
        Dim Period As String

        dgNurseryActivityDet.DataSource = LoadData()
        dgNurseryActivityDet.DataBind()
        lblLocCode.Text = strLocation
        GetActivePeriod(Period)
        lblBgtPeriod.Text = Period
    End Sub

    Protected Function LoadData() As DataSet

        strParam = "|" & strLocation & "|" & GetActivePeriod("") & "|" & hidBlkCode.Value & "||NAS.DispSeq ASC"
        Try
            intErrNo = objBD.mtdGetNurseryActivity(strOppCd_NurseryActivity_Format_GET, strParam, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_NURSERYACTIVITYDET_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_NurseryActivity_Details.aspx")
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
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_NURSERYACTDET_GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_NurseryActivity_Details.aspx")
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

    Sub DataGrid_ItemDataBound(ByVal Sender As Object, ByVal e As DataGridItemEventArgs)
        Dim lbl As Label
        Dim btn As LinkButton
        Dim txt As TextBox

        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            lbl = e.Item.FindControl("lblIdx")
            lbl.Text = e.Item.ItemIndex.ToString + 1

            lbl = e.Item.FindControl("lblDispType")
            Select Case lbl.Text
                Case objBDSetup.EnumBudgetFormatItem.Header
                    e.Item.Cells(0).Font.Bold = True
                    e.Item.CssClass = "mr-r"
                    lbl = e.Item.FindControl("lblAcc")
                    lbl.Font.Bold = True
                    lbl = e.Item.FindControl("lblItem")
                    lbl.Font.Bold = True
                    lbl = e.Item.FindControl("lblFreq")
                    lbl.Visible = False
                    lbl = e.Item.FindControl("lblUnit")
                    lbl.Visible = False
                    lbl = e.Item.FindControl("lblUnitCost")
                    lbl.Visible = False
                    lbl = e.Item.FindControl("lblOtherCost")
                    lbl.Visible = False
                    lbl = e.Item.FindControl("lblMaterialCost")
                    lbl.Visible = False
                    lbl = e.Item.FindControl("lblLabourCost")
                    lbl.Visible = False
                    lbl = e.Item.FindControl("lblCostperSeed")
                    lbl.Visible = False
                    btn = e.Item.FindControl("Edit")
                    btn.Visible = False
                    lbl = e.Item.FindControl("lblAddVote")
                    lbl.Visible = False

                Case objBDSetup.EnumBudgetFormatItem.Entry
                    lbl = e.Item.FindControl("lblOtherCost")
                    If lbl.Text = "0.00" Then
                        lbl.Visible = False
                    End If
                    lbl = e.Item.FindControl("lblMaterialCost")
                    If lbl.Text = "0.00" Then
                        lbl.Visible = False
                    End If
                    lbl = e.Item.FindControl("lblLabourCost")
                    If lbl.Text = "0.00" Then
                        lbl.Visible = False
                    End If
                    lbl = e.Item.FindControl("lblCostperSeed")
                    If lbl.Text = "0.00" Then
                        lbl.Visible = False
                    End If

                Case objBDSetup.EnumBudgetFormatItem.Formula, objBDSetup.EnumBudgetFormatItem.Total
                    e.Item.CssClass = "mr-l"
                    e.Item.Font.Bold = True

                    lbl = e.Item.FindControl("lblAcc")
                    lbl.Font.Bold = True
                    lbl = e.Item.FindControl("lblItem")
                    lbl.Font.Bold = True
                    lbl = e.Item.FindControl("lblOtherCost")
                    lbl.Font.Bold = True
                    lbl = e.Item.FindControl("lblMaterialCost")
                    lbl.Font.Bold = True
                    lbl = e.Item.FindControl("lblLabourCost")
                    lbl.Font.Bold = True
                    lbl = e.Item.FindControl("lblCostperSeed")
                    lbl.Font.Bold = True
                    lbl = e.Item.FindControl("lblFreq")
                    lbl.Visible = False
                    lbl = e.Item.FindControl("lblUnit")
                    lbl.Visible = False
                    lbl = e.Item.FindControl("lblUnitCost")
                    lbl.Visible = False
                    btn = e.Item.FindControl("Edit")
                    btn.Visible = False
                    lbl = e.Item.FindControl("lblAddVote")
                    lbl.Visible = False

                    lbl = e.Item.FindControl("lblDispCol")
                    Select Case lbl.Text
                        Case objBDSetup.EnumBudgetItemColumn.labour
                            lbl = e.Item.FindControl("lblMaterialCost")
                            lbl.Visible = False
                            lbl = e.Item.FindControl("lblOtherCost")
                            lbl.Visible = False
                        Case objBDSetup.EnumBudgetItemColumn.Other
                            lbl = e.Item.FindControl("lblMaterialCost")
                            lbl.Visible = False
                            lbl = e.Item.FindControl("lblLabourCost")
                            lbl.Visible = False
                        Case objBDSetup.EnumBudgetItemColumn.Material
                            lbl = e.Item.FindControl("lblOtherCost")
                            lbl.Visible = False
                            lbl = e.Item.FindControl("lblLabourCost")
                            lbl.Visible = False
                    End Select


                Case Else
                    e.Item.CssClass = "mr-l"

            End Select
        ElseIf e.Item.ItemType = ListItemType.EditItem Then
            lbl = e.Item.FindControl("lblDispType")
            Select Case lbl.Text
                Case objBDSetup.EnumBudgetFormatItem.Formula, objBDSetup.EnumBudgetFormatItem.Total
                    lbl = e.Item.FindControl("lblDispCol")
                    Select Case lbl.Text
                        Case objBDSetup.EnumBudgetItemColumn.labour
                            lbl = e.Item.FindControl("lblLabourCost")
                            lbl.Text = "Labour"
                            lbl.Visible = True
                            lbl = e.Item.FindControl("lblMaterialCost")
                            lbl.Visible = False
                            lbl = e.Item.FindControl("lblOtherCost")
                            lbl.Visible = False
                        Case objBDSetup.EnumBudgetItemColumn.Other
                            lbl = e.Item.FindControl("lblOtherCost")
                            lbl.Text = "Others"
                            lbl.Visible = True
                            lbl = e.Item.FindControl("lblMaterialCost")
                            lbl.Visible = False
                            lbl = e.Item.FindControl("lblLabourCost")
                            lbl.Visible = False
                        Case objBDSetup.EnumBudgetItemColumn.Material
                            lbl = e.Item.FindControl("lblMaterialCost")
                            lbl.Text = "Material"
                            lbl.Visible = True
                            lbl = e.Item.FindControl("lblOtherCost")
                            lbl.Visible = False
                            lbl = e.Item.FindControl("lblLabourCost")
                            lbl.Visible = False
                    End Select

                    lbl = e.Item.FindControl("lblAcc")
                    lbl.Font.Bold = True
                    lbl = e.Item.FindControl("lblItem")
                    lbl.Font.Bold = True
                    txt = e.Item.FindControl("txtFreq")
                    txt.Visible = False
                    txt = e.Item.FindControl("txtUnit")
                    txt.Visible = False
                    txt = e.Item.FindControl("txtUnitCost")
                    txt.Visible = False
                    lbl = e.Item.FindControl("lblAddVote")
                    lbl.Visible = False

                Case objBDSetup.EnumBudgetFormatItem.Entry
                    lbl = e.Item.FindControl("lblAddVote")
                    lbl.Visible = False
                    If lblBgtStatus.Text.Trim = objBDSetup.EnumPeriodStatus.Addvote Then
                        txt = e.Item.FindControl("txtAddVote")
                        txt.Visible = True
                        txt = e.Item.FindControl("txtFreq")
                        txt.Visible = False
                        txt = e.Item.FindControl("txtUnit")
                        txt.Visible = False
                        txt = e.Item.FindControl("txtUnitCost")
                        txt.Visible = False

                    End If

                    lbl = e.Item.FindControl("lblDispCol")
                    Select Case lbl.Text
                        Case objBDSetup.EnumBudgetItemColumn.labour
                            lbl = e.Item.FindControl("lblLabourCost")
                            lbl.Text = "Labour"
                            lbl.Visible = True
                            lbl = e.Item.FindControl("lblMaterialCost")
                            lbl.Visible = False
                            lbl = e.Item.FindControl("lblOtherCost")
                            lbl.Visible = False
                        Case objBDSetup.EnumBudgetItemColumn.Other
                            lbl = e.Item.FindControl("lblOtherCost")
                            lbl.Text = "Other"
                            lbl.Visible = True
                            lbl = e.Item.FindControl("lblMaterialCost")
                            lbl.Visible = False
                            lbl = e.Item.FindControl("lblLabourCost")
                            lbl.Visible = False
                        Case objBDSetup.EnumBudgetItemColumn.Material
                            lbl = e.Item.FindControl("lblMaterialCost")
                            lbl.Text = "Material"
                            lbl.Visible = True
                            lbl = e.Item.FindControl("lblOtherCost")
                            lbl.Visible = False
                            lbl = e.Item.FindControl("lblLabourCost")
                            lbl.Visible = False
                    End Select

                    lbl = e.Item.FindControl("lblCostperSeed")
                    lbl.Visible = False

            End Select
        End If
    End Sub

    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)

        dgNurseryActivityDet.EditItemIndex = CInt(E.Item.ItemIndex)
        BindGrid()
    End Sub

    Sub DEDR_Update(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim EditText As TextBox
        Dim label As label
        Dim intError As Integer
        Dim intEdit As Integer

        Dim strNurseryActID As String
        Dim strDisp As String
        Dim strDispCol As String
        Dim strFreq As String
        Dim strUnit As String
        Dim strUnitCost As String
        Dim strAddVote As String

        label = E.Item.FindControl("lblNurseryActID")
        strNurseryActID = label.Text
        label = E.Item.FindControl("lblDispType")
        strDisp = label.Text
        label = E.Item.FindControl("lblDispCol")
        strDispCol = label.Text
        EditText = E.Item.FindControl("txtFreq")
        strFreq = EditText.Text
        EditText = E.Item.FindControl("txtUnit")
        strUnit = EditText.Text
        EditText = E.Item.FindControl("txtUnitCost")
        strUnitCost = EditText.Text
        EditText = E.Item.FindControl("txtAddVote")
        strAddVote = EditText.Text

        strParam = GetActivePeriod("") & "|" & _
                   strNurseryActID & "|" & _
                   strFreq & "|" & _
                   strUnit & "|" & _
                   strUnitCost & "|" & _
                   objBD.EnumNurseryActivityStatus.Budgeted & "|" & _
                   strDisp & "|" & _
                   strDispCol & "|" & "|" & _
                   hidBlkCode.Value & "|" & _
                    strAddVote

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
            strOppCd_NurseryActivity_CostPerSeed_SUM = "BD_CLSTRX_NURSERYACTIVITY_COSTPERSEED_SUM"
        Else
            strOppCd_NurseryActivity_CostPerSeed_SUM = "BD_CLSTRX_NURSERYACTIVITY_COSTPERSEED_SBLK_SUM"
        End If

        Try
            intErrNo = objBD.mtdUpdNurseryActivity(strOppCd_NurseryActivity_Format_GET, _
                                                    strOppCd_NurseryActivity_ADD, _
                                                    strOppCd_NurseryActivitySetup_GET, _
                                                    strOppCd_NurseryActivity_UPD, _
                                                    strOpCd_Formula_GET, _
                                                    "", _
                                                    strOppCd_NurseryActivity_CostPerSeed_SUM, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strParam, _
                                                    objBD.EnumOperation.Update, _
                                                    intError, _
                                                    False)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_NURSERYACTIVITYDET_UPD&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_NurseryActivity_Details.aspx")
        End Try

        If intError = objBD.EnumErrorType.CalculationErr Then
            lblOvrMsg.Visible = True
            lblOvrMsgTop.Visible = True
        ElseIf intError = objBD.EnumErrorType.NoRecord Then
            lblNoRecord.Visible = True
            lblNoRecordTop.Visible = True
        Else
            For intEdit = E.Item.ItemIndex + 1 To dgNurseryActivityDet.Items.Count - 1
                label = dgNurseryActivityDet.Items.Item(CInt(intEdit)).FindControl("lblDispType")
                If label.Text.Trim <> objBDSetup.EnumBudgetFormatItem.Header Then
                    Exit For
                End If
            Next

            dgNurseryActivityDet.EditItemIndex = intEdit
            BindGrid()
        End If
    End Sub

    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        If CInt(E.Item.ItemIndex) = 0 And dgNurseryActivityDet.Items.Count = 1 And dgNurseryActivityDet.PageCount <> 1 Then
            dgNurseryActivityDet.CurrentPageIndex = dgNurseryActivityDet.PageCount - 2
        End If
        dgNurseryActivityDet.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub CallRecalc_Formula(ByVal Sender As Object, ByVal E As EventArgs) Handles lbtn_Recalc.Click
        Recalc_Formula()
    End Sub

    Sub Recalc_Formula()
        Dim intError As Integer

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
            strOppCd_NurseryActivity_CostPerSeed_SUM = "BD_CLSTRX_NURSERYACTIVITY_COSTPERSEED_SUM"
        Else
            strOppCd_NurseryActivity_CostPerSeed_SUM = "BD_CLSTRX_NURSERYACTIVITY_COSTPERSEED_SBLK_SUM"
        End If

        strParam = "|" & strLocation & "|" & GetActivePeriod("") & "|" & hidBlkCode.Value & "|NAS.DispSeq ASC"

        Try
            intErrNo = objBD.mtdUpdNurseryActivityFormula(strParam, _
                                                        strOppCd_NurseryActivity_Format_GET, _
                                                        strOppCd_NurseryActivity_ADD, _
                                                        strOppCd_NurseryActivitySetup_GET, _
                                                        strOppCd_NurseryActivity_UPD, _
                                                        strOpCd_Formula_GET, _
                                                        strOppCd_NurseryActivity_CostPerSeed_SUM, _
                                                        strCompany, _
                                                        strLocation, _
                                                        strUserId, _
                                                        objBD.EnumOperation.Update, _
                                                        intError)

            If intError = objBD.EnumErrorType.CalculationErr Then
                lblOvrMsg.Visible = True
                lblOvrMsgTop.Visible = True
            ElseIf intError = objBD.EnumErrorType.NoRecord Then
                lblNoRecord.Visible = True
                lblNoRecordTop.Visible = True
            End If

        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_NURSERYACTIVITY_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_NurseryActivity_Details.aspx")
        End Try
        BindGrid()

    End Sub


End Class
