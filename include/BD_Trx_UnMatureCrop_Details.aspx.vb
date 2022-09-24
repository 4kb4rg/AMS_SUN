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


Public Class BD_UnMatureCrop_Det : Inherits Page

    Protected WithEvents dgUnMatureCropDet As DataGrid
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblOvrMsg As Label
    Protected WithEvents lbtn_Recalc As Button
    Protected WithEvents lblNoRecord As Label
    Protected WithEvents lblBlkTag As Label
    Protected WithEvents lblBlkCode As Label
    Protected WithEvents lblLocTag As Label
    Protected WithEvents lblTotalAreaFig As Label
    Protected WithEvents lblYearPlanted As Label

    Dim objBD As New agri.BD.clsTrx()
    Dim objBDSetup As New agri.BD.clsSetup()
    Dim objGLSetup As New agri.GL.clsSetup()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Dim strOppCd_UnMatureCrop_Format_GET As String = "BD_CLSTRX_UNMATURECROP_FORMAT_GET"
    Dim strOppCd_UnMatureCropSetup_GET As String = "BD_CLSSETUP_UNMATURECROP_FORMAT_GET"
    Dim strOppCd_UnMatureCrop_ADD As String = "BD_CLSTRX_UNMATURECROP_ADD"
    Dim strOppCd_UnMatureCrop_UPD As String = "BD_CLSTRX_UNMATURECROP_UPD"
    Dim strOppCd_UnMatureCrop_CostPerArea_SUM As String = "BD_CLSTRX_MATURECROP_BLKTOTALAREA_GET"
    Dim strOppCd_UnMatureCrop_CostPerWeight_SUM As String = ""
    Dim strOpCd_Formula_GET As String = "BD_CLSTRX_CALCFORMULA_GET"

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
            lblNoRecord.Visible = False
            onload_GetLangCap()

            If Not Page.IsPostBack Then
                BindGrid()
                GetAreaStmtTotalArea()
                lblBlkCode.Text = Request.QueryString("blk")
                lblYearPlanted.Text = Request.QueryString("yr")
                Recalc_Formula()

            End If

        End If
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblLocTag.Text = GetCaption(objLangCap.EnumLangCap.Location)
        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
            lblBlkTag.Text = GetCaption(objLangCap.EnumLangCap.Block) & " Code"
        Else
            lblBlkTag.Text = GetCaption(objLangCap.EnumLangCap.SubBlock) & " Code"
        End If
        dgUnMatureCropDet.Columns(1).HeaderText = GetCaption(objLangCap.EnumLangCap.Account)
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
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_UNMATURECROP_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_UnMatureCrop_Details.aspx")
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





    Sub BindGrid()
        Dim PageNo As Integer
        Dim Period As String

        dgUnMatureCropDet.DataSource = LoadData()
        dgUnMatureCropDet.DataBind()
        lblLocCode.Text = strLocation
        GetActivePeriod(Period)
        lblBgtPeriod.Text = Period
    End Sub

    Protected Function LoadData() As DataSet
        Dim Period As String

        strParam = "|" & strLocation & "|" & GetActivePeriod("") & "|" & Request.QueryString("blk") & "||MCS.DispSeq ASC"
        Try
            intErrNo = objBD.mtdGetUnMatureCrop(strOppCd_UnMatureCrop_Format_GET, strParam, objDataSet)
        Catch Exp As Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_UNMATURECROP_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_UnMatureCrop_Details.aspx")
        End Try

        Return objDataSet
    End Function

    Sub GetAreaStmtTotalArea()

        Dim dsTotalArea As New DataSet()
        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
            strOppCd_UnMatureCrop_CostPerArea_SUM = "BD_CLSTRX_MATURECROP_BLKTOTALAREA_GET"
            strParam = objGLSetup.EnumBlockType.InMatureField & "|" & objGLSetup.EnumBlockStatus.Active & "|" & strLocation & "|" & Request.QueryString("blk").Trim & "|"
        Else
            strOppCd_UnMatureCrop_CostPerArea_SUM = "BD_CLSTRX_MATURECROP_SUBBLKTOTALAREA_GET"
            strParam = objGLSetup.EnumBlockType.InMatureField & "|" & objGLSetup.EnumSubBlockStatus.Active & "|" & strLocation & "|" & Request.QueryString("blk").Trim & "|"
        End If

        Try
            intErrNo = objBD.mtdGetMatureCropTotalArea(strOppCd_UnMatureCrop_CostPerArea_SUM, strParam, dsTotalArea)
        Catch Exp As Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_AREASTMT_TOTALAREA&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_UnMatureCrop_Details.aspx")
        End Try

        lblTotalAreaFig.Text = FormatNumber(Trim(dsTotalArea.Tables(0).Rows(0).Item("AreaSize")), 2)

    End Sub


    Protected Function GetActivePeriod(ByRef BGTPeriod As String) As String
        Dim strOppCd_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"
        Dim dsperiod As New DataSet()

        strParam = "|||||" & objBDSetup.EnumPeriodStatus.Active & "|" & strLocation & "|"

        Try
            intErrNo = objBDSetup.mtdGetPeriodList(strOppCd_GET, strParam, dsperiod)
        Catch Exp As Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_UnMatureCrop_Details.aspx")
        End Try
        If dsperiod.Tables(0).Rows.Count > 0 Then
            BGTPeriod = dsperiod.Tables(0).Rows(0).Item("BGTPeriod")
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
        Dim rv As RangeValidator

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
                    lbl = e.Item.FindControl("lblMandays")
                    lbl.Visible = False
                    lbl = e.Item.FindControl("lblOtherCost")
                    lbl.Visible = False
                    lbl = e.Item.FindControl("lblMaterialCost")
                    lbl.Visible = False
                    lbl = e.Item.FindControl("lblLabourCost")
                    lbl.Visible = False
                    lbl = e.Item.FindControl("lblCostperArea")
                    lbl.Visible = False
                    btn = e.Item.FindControl("Edit")
                    btn.Visible = False

                Case objBDSetup.EnumBudgetFormatItem.Entry
                    lbl = e.Item.FindControl("lblFreq")
                    If lbl.Text = "0.00" Then
                        lbl.Visible = False
                    End If
                    lbl = e.Item.FindControl("lblUnit")
                    If lbl.Text = "0.00" Then
                        lbl.Visible = False
                    End If
                    lbl = e.Item.FindControl("lblUnitCost")
                    If lbl.Text = "0.00" Then
                        lbl.Visible = False
                    End If
                    lbl = e.Item.FindControl("lblMandays")
                    If lbl.Text = "0.00" Then
                        lbl.Visible = False
                    End If
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
                    lbl = e.Item.FindControl("lblCostperArea")
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
                    lbl = e.Item.FindControl("lblCostperArea")
                    lbl.Font.Bold = True
                    lbl = e.Item.FindControl("lblFreq")
                    lbl.Visible = False
                    lbl = e.Item.FindControl("lblUnit")
                    lbl.Visible = False
                    lbl = e.Item.FindControl("lblUnitCost")
                    lbl.Visible = False
                    lbl = e.Item.FindControl("lblMandays")
                    lbl.Visible = False
                    btn = e.Item.FindControl("Edit")
                    btn.Visible = False

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
                    e.Item.CssClass = "mr-l"
                    e.Item.Font.Bold = True

                    lbl = e.Item.FindControl("lblDispCol")
                    rv = e.Item.FindControl("RangeFreq")
                    rv.Enabled = False
                    rv = e.Item.FindControl("RangeUnit")
                    rv.Enabled = False

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
                    txt = e.Item.FindControl("txtMandays")
                    txt.Visible = False
                Case objBDSetup.EnumBudgetFormatItem.Entry
                    lbl = e.Item.FindControl("lblDispCol")
                    rv = e.Item.FindControl("RangeFreq")
                    rv.Enabled = True
                    rv = e.Item.FindControl("RangeUnit")
                    rv.Enabled = True

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
                    lbl = e.Item.FindControl("lblCostperArea")
                    lbl.Visible = False

            End Select
        End If
    End Sub

    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)

        dgUnMatureCropDet.EditItemIndex = CInt(E.Item.ItemIndex)
        BindGrid()
    End Sub

    Sub DEDR_Update(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim EditText As TextBox
        Dim label As label
        Dim intError As Integer
        Dim intEdit As Integer

        Dim strUnMatureCropSetID As String
        Dim strDisp As String
        Dim strDispCol As String
        Dim strFreq As String
        Dim strUnit As String
        Dim strUnitCost As String
        Dim strMandays As String

        label = E.Item.FindControl("lblUnMatureCropSetID")
        strUnMatureCropSetID = label.Text
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
        EditText = E.Item.FindControl("txtMandays")
        strMandays = EditText.Text

        strParam = GetActivePeriod("") & "|" & _
                   strUnMatureCropSetID & "|" & _
                   strFreq & "|" & _
                   strUnit & "|" & _
                   strUnitCost & "|" & _
                   objBD.EnumUnMatureCropStatus.Budgeted & "|" & _
                   strDisp & "|" & _
                   strDispCol & "|" & _
                   strMandays & "|" & _
                   Request.QueryString("blk") & "|"

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
            strOppCd_UnMatureCrop_CostPerArea_SUM = "BD_CLSTRX_MATURECROP_BLKTOTALAREA_GET"
        Else
            strOppCd_UnMatureCrop_CostPerArea_SUM = "BD_CLSTRX_MATURECROP_SUBBLKTOTALAREA_GET"
        End If

        Try
            intErrNo = objBD.mtdUpdUnMatureCrop(strOppCd_UnMatureCrop_Format_GET, _
                                            strOppCd_UnMatureCrop_ADD, _
                                            strOppCd_UnMatureCropSetup_GET, _
                                            strOppCd_UnMatureCrop_UPD, _
                                            strOpCd_Formula_GET, _
                                            strOppCd_UnMatureCrop_CostPerArea_SUM, _
                                            strOppCd_UnMatureCrop_CostPerWeight_SUM, _
                                            strCompany, _
                                            strLocation, _
                                            strUserId, _
                                            strParam, _
                                            objBD.EnumOperation.Update, _
                                            intConfigsetting, _
                                            intError)
        Catch Exp As Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=UNMATURECROP_UPD&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_UnMatureCrop_Details.aspx")
        End Try

        If intError = objBD.EnumErrorType.CalculationErr Then
            lblOvrMsg.Visible = True
        ElseIf intError = objBD.EnumErrorType.NoRecord Then
            lblNoRecord.Visible = True
        Else
            For intEdit = E.Item.ItemIndex + 1 To dgUnMatureCropDet.Items.Count - 1
                label = dgUnMatureCropDet.Items.Item(CInt(intEdit)).FindControl("lblDispType")
                If label.Text.Trim <> objBDSetup.EnumBudgetFormatItem.Header Then
                    Exit For
                End If
            Next

            dgUnMatureCropDet.EditItemIndex = intEdit
            BindGrid()
        End If
    End Sub

    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        If CInt(E.Item.ItemIndex) = 0 And dgUnMatureCropDet.Items.Count = 1 And dgUnMatureCropDet.PageCount <> 1 Then
            dgUnMatureCropDet.CurrentPageIndex = dgUnMatureCropDet.PageCount - 2
        End If
        dgUnMatureCropDet.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub CallRecalc_Formula(ByVal Sender As Object, ByVal E As EventArgs) Handles lbtn_Recalc.Click
        Recalc_Formula()
    End Sub
    Sub Recalc_Formula()
        Dim intError As Integer

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
            strOppCd_UnMatureCrop_CostPerArea_SUM = "BD_CLSTRX_MATURECROP_BLKTOTALAREA_GET"
        Else
            strOppCd_UnMatureCrop_CostPerArea_SUM = "BD_CLSTRX_MATURECROP_SUBBLKTOTALAREA_GET"
        End If

        strParam = "|" & strLocation & "|" & GetActivePeriod("") & "|" & Request.QueryString("blk") & "|MC.UnMatureCropSetID ASC"
        Try
            intErrNo = objBD.mtdUpdUnMatureCropFormula(strParam, _
                                                    strOppCd_UnMatureCrop_Format_GET, _
                                                    strOppCd_UnMatureCrop_ADD, _
                                                    strOppCd_UnMatureCropSetup_GET, _
                                                    strOppCd_UnMatureCrop_UPD, _
                                                    strOpCd_Formula_GET, _
                                                    strOppCd_UnMatureCrop_CostPerArea_SUM, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    objBD.EnumOperation.Update, _
                                                    intConfigsetting, _
                                                    intError)

            If intError = objBD.EnumErrorType.CalculationErr Then
                lblOvrMsg.Visible = True
            ElseIf intError = objBD.EnumErrorType.NoRecord Then
                lblNoRecord.Visible = True
            End If

        Catch Exp As Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_OVERHEAD_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_PlantationOH_Details.aspx")
        End Try
        BindGrid()

    End Sub

End Class
