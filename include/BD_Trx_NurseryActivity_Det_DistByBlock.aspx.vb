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



Public Class BD_trx_NurseryActivity_Det_DistByBlock : Inherits Page

    Protected WithEvents dgNurseryActivityDetDist As DataGrid
    Protected WithEvents lblTitleNurseryAct As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblOvrMsg As Label
    Protected WithEvents lblLocTag As Label
    Protected WithEvents lblBlkTag As Label
    Protected WithEvents lblBlkCode As Label
    Protected WithEvents lblNoRecord As Label
    Protected WithEvents lblTotalSeedFig As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents hidBlkCode As HtmlInputHidden
    Protected WithEvents hidSubBlkCode As HtmlInputHidden

    Dim objBD As New agri.BD.clsTrx()
    Dim objBDSetup As New agri.BD.clsSetup()
    Dim objGLSetup As New agri.GL.clsSetup()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()

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
            lblOvrMsg.Visible = False
            lblNoRecord.Visible = False

            If Not Page.IsPostBack Then
                lblBlkCode.Text = Request.QueryString("blk")
                hidBlkCode.Value = lblBlkCode.Text
                hidSubBlkCode.Value = Request.QueryString("subblk")

                onload_GetLangCap()
                BindGrid()
                GetTotalSeed()
            End If
        End If
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()

        lblLocTag.Text = GetCaption(objLangCap.EnumLangCap.Location) & lblCode.Text
        lblBlkTag.Text = GetCaption(objLangCap.EnumLangCap.Block) & lblCode.Text
        dgNurseryActivityDetDist.Columns(1).HeaderText = GetCaption(objLangCap.EnumLangCap.Account) & lblCode.Text
        lblTitleNurseryAct.Text = lblTitleNurseryAct.Text & " DISTRIBUTE BY " & UCase(lblBlkTag.Text)
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
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_NURSERYACT_DET_DISTBYBLK_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_NurseryActivity_Det_DistByBlock.aspx?blk=" & hidBlkCode.Value)
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

        Dim strOppCd_NurseryActivity_CostPerSeed_SUM As String = "BD_CLSTRX_NURSERYACTIVITY_COSTPERSEED_BLK_SUM"
        Dim dsTotalSeed As New DataSet()

        strParam = "||" & strLocation & "|" & hidBlkCode.Value & "|"

        Try
            intErrNo = objBD.mtdGetNurseryActivityTotalSeed(strOppCd_NurseryActivity_CostPerSeed_SUM, strParam, dsTotalSeed)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_NURSERYACTDET_BLK_GET_AREASTMT_TOTALAREA&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_NurseryActivity_Det_DistByBlock.aspx?blk=" & hidBlkCode.Value)
        End Try

        lblTotalSeedFig.Text = FormatNumber(dsTotalSeed.Tables(0).Rows(0).Item("Qty"), 2)

    End Sub

    Sub BindGrid()
        Dim Period As String

        dgNurseryActivityDetDist.DataSource = LoadData()
        dgNurseryActivityDetDist.DataBind()
        lblLocCode.Text = strLocation
        GetActivePeriod(Period)
        lblBgtPeriod.Text = Period
    End Sub

    Protected Function LoadData() As DataSet
        Dim strOppCd_NurseryActivity_DistByBlk_GET As String = "BD_CLSTRX_NURSERYACTIVITY_DISTBYBLOCK_GET"
        Dim strOppCd_NurseryActivity_DistByBlk_Cost_GET As String = "BD_CLSTRX_NURSERYACTIVITY_DISTBYBLOCK_COST_GET"
        Dim strOppCd_SubBlock_Get As String = "BD_CLSTRX_NURSERYACTIVITY_SUBBLOCK_GET"
        Dim dsSubBlkList As New DataSet
        Dim dsCost As New DataSet
        Dim strSubBlkCode As String
        Dim intCnt As Integer
        Dim intCntNA As Integer
        Dim intCntCost As Integer

        strParam = objGLSetup.EnumBlockType.Nursery & "|" & _
                   objGLSetup.EnumBlockStatus.Active & "|" & _
                   strLocation & "|AND SBLK.BlkCode = '" & lblBlkCode.Text & "' "
        Try
            intErrNo = objBD.mtdGetNurseryActivityYear(strOppCd_SubBlock_Get, strParam, dsSubBlkList)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_CLSTRX_NURSERYACT_DET_DISTBYBLK_SUBBLKLIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_NurseryActivity_Det_DistByBlock.aspx?blk=" & hidBlkCode.Value)
        End Try

        For intCnt = 0 To dsSubBlkList.Tables(0).Rows.Count - 1
            strSubBlkCode += dsSubBlkList.Tables(0).Rows(intCnt).Item("BlkCode") & "','"
        Next

        strSubBlkCode = Mid(strSubBlkCode, 1, Len(strSubBlkCode) - 3) & " "

        strParam = "|" & strLocation & "|" & GetActivePeriod("") & "||" & vbCrLf & _
                   "AND NA.BlkCode IN ('" & strSubBlkCode & "') " & vbCrLf & _
                   "AND NAS.ItemDisplayType = '" & objBDSetup.EnumBudgetFormatItem.Entry & "'|"
        Try
            intErrNo = objBD.mtdGetNurseryActivity(strOppCd_NurseryActivity_DistByBlk_GET, strParam, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_NURSERYACTIVITYDET_DISTBLKBLK_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_NurseryActivity_Det_DistByBlock.aspx?blk=" & hidBlkCode.Value)
        End Try

        For intCntNA = 0 To objDataSet.Tables(0).Rows.Count - 1
            For intCnt = 0 To dsSubBlkList.Tables(0).Rows.Count - 1
                strParam = objDataSet.Tables(0).Rows(intCntNA).Item("NurseryActID") & "|" & _
                           strLocation & "|" & _
                           GetActivePeriod("") & "||" & _
                           "AND NA.BlkCode = '" & dsSubBlkList.Tables(0).Rows(intCnt).Item("SubBlkCode") & "'|"
                Try
                    intErrNo = objBD.mtdGetNurseryActivity(strOppCd_NurseryActivity_DistByBlk_Cost_GET, strParam, dsCost)
                Catch Exp As System.Exception
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_CLSTRX_NURSERYACTIVITYDET_DISTBYBLK_COST_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_NurseryActivity_Det_DistByBlock.aspx?blk=" & hidBlkCode.Value)
                End Try

                For intCntCost = 0 To dsCost.Tables(0).Rows.Count - 1
                    If dsCost.Tables(0).Rows(intCntCost).Item("Unit") <> 0 Then
                        objDataSet.Tables(0).Rows(intCntNA).Item("Unit") = dsCost.Tables(0).Rows(intCntCost).Item("Unit")
                    End If

                    If dsCost.Tables(0).Rows(intCntCost).Item("UnitCost") <> 0 Then
                        objDataSet.Tables(0).Rows(intCntNA).Item("UnitCost") = dsCost.Tables(0).Rows(intCntCost).Item("UnitCost")
                    End If
                Next
            Next
        Next

        Return objDataSet

    End Function

    Protected Function GetActivePeriod(ByRef BGTPeriod As String) As String
        Dim strOppCd_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"
        Dim dsperiod As New DataSet()


        strParam = "|||||" & objBDSetup.EnumPeriodStatus.Active & "|" & strLocation & "|"

        Try
            intErrNo = objBDSetup.mtdGetPeriodList(strOppCd_GET, strParam, dsperiod)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_CLSTRX_NURSERYACT_DET_DISTBYBLK_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_NurseryActivity_Det_DistByBlock.aspx?blk=" & hidBlkCode.Value)
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

        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            lbl = e.Item.FindControl("lblIdx")
            lbl.Text = e.Item.ItemIndex.ToString + 1

            lbl = e.Item.FindControl("lblDispType")
            Select Case lbl.Text
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
                Case Else
                    e.Item.CssClass = "mr-l"

            End Select
        ElseIf e.Item.ItemType = ListItemType.EditItem Then
            lbl = e.Item.FindControl("lblIdx")
            lbl.Text = e.Item.ItemIndex.ToString + 1
        End If
    End Sub

    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)

        dgNurseryActivityDetDist.EditItemIndex = CInt(E.Item.ItemIndex)
        BindGrid()
    End Sub

    Sub DEDR_Update(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strOppCode_SubBlock_Get = "BD_CLSTRX_NURSERYACTIVITY_SUBBLOCK_GET"
        Dim strOppCd_NurseryActivity_UPD As String = "BD_CLSTRX_NURSERYACTIVITY_UPD"
        Dim EditText As TextBox
        Dim label As label
        Dim intError As Integer
        Dim intEdit As Integer

        Dim strNurseryActSetID As String
        Dim strDisp As String
        Dim strDispCol As String
        Dim strFreq As String
        Dim strUnit As String
        Dim strUnitCost As String

        label = E.Item.FindControl("lblNurseryActID")
        strNurseryActSetID = label.Text
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

        strParam = GetActivePeriod("") & "|" & _
                   lblBlkCode.Text & "|" & _
                   lblTotalSeedFig.Text & "|" & _
                   strFreq & "|" & _
                   strUnit & "|" & _
                   strUnitCost & "|" & _
                   strDisp & "|" & _
                   strDispCol & "|" & _
                   strNurseryActSetID & "|"

        Try
            intErrNo = objBD.mtdUpdNurseryActivity_DistByBlock(strOppCd_NurseryActivity_UPD, _
                                                               strOppCode_SubBlock_Get, _
                                                               strCompany, _
                                                               strLocation, _
                                                               strUserId, _
                                                               strParam, _
                                                               intError)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_CLSTRX_NURSERYACTDET_DISTBYBLK_SBLK_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_NurseryActivity_Det_DistByBlock.aspx?blk=" & hidBlkCode.Value)
        End Try


        For intEdit = E.Item.ItemIndex + 1 To dgNurseryActivityDetDist.Items.Count - 1
            label = dgNurseryActivityDetDist.Items.Item(CInt(intEdit)).FindControl("lblDispType")
            If label.Text.Trim <> objBDSetup.EnumBudgetFormatItem.Header Then
                Exit For
            End If
        Next

        dgNurseryActivityDetDist.EditItemIndex = intEdit
        BindGrid()
    End Sub

    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        If CInt(E.Item.ItemIndex) = 0 And dgNurseryActivityDetDist.Items.Count = 1 And dgNurseryActivityDetDist.PageCount <> 1 Then
            dgNurseryActivityDetDist.CurrentPageIndex = dgNurseryActivityDetDist.PageCount - 2
        End If
        dgNurseryActivityDetDist.EditItemIndex = -1
        BindGrid()
    End Sub


End Class
