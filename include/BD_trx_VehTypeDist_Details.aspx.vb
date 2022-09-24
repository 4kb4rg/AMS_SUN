
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


Public Class BD_VehTypeDist_Details : Inherits Page

    Protected WithEvents dgVehTypeDist As DataGrid
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblErrMessage2 As Label
    Protected WithEvents lblErrUsage As Label
    Protected WithEvents lblYrPlant As Label
    Protected WithEvents lblBlkTotalArea As Label
    Protected WithEvents lblTotalFFB As Label
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents lblCode As Label

    Protected WithEvents lblVehTypeCodeTag As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBudgeting As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblOper As Label
    Protected WithEvents lblLocTag As Label
    Protected WithEvents lblTotalCost As Label
    Protected WithEvents lblBgtStatus As Label 
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl() 

    Dim objBD As New agri.BD.clsTrx()
    Dim objBDSetup As New agri.BD.clsSetup()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objGLSetup As New agri.GL.clsSetup()
    Dim objAdminLoc As New agri.Admin.clsLoc()
    

    Dim strOppCd_VehTypeDist_GET As String = "BD_CLSTRX_VEHTYPEDIST_GET"
    Dim strOppCd_VehTypeDist_ADD As String = "BD_CLSTRX_VEHTYPEDIST_ADD"
    Dim strOppCd_VehTypeDist_UPD As String = "BD_CLSTRX_VEHTYPEDIST_UPD"
    Dim strOppCd_VehTypeDist_Setup_GET As String = "BD_CLSSETUP_VEHTYPEDIST_GET"
    Dim strOppCd_YrPlanted_GET As String
    Dim strOppCd_VehTypeDistUsg_GET As String = "BD_CLSTRX_VEHTYPEUSAGE_GET"
    Dim strOppCd_VehTypeDistUsgCost_SUM As String = "BD_CLSTRX_VEHTYPEDIST_USGCOST_SUM"
    Dim strOppCd_VehTypeDistParam_ADD As String = "BD_CLSTRX_VEHTYPEDISTPARAM_ADD"
    Dim strOppCd_VehTypeDistParam_GET As String = "BD_CLSTRX_VEHTYPEDISTPARAM_GET"
    Dim strOppCd_VehTypeDistParam_UPD As String = "BD_CLSTRX_VEHTYPEDISTPARAM_UPD"
    Dim objDataSet As New DataSet()
    Dim dsYrPlanted As New DataSet()
    Dim dsTotalArea As New DataSet()
    Dim objLangCapDs As New DataSet()
    Dim dr As DataRow

    Dim intErrNo As Integer
    Dim strParam As String = ""
    Dim strBlkCode As String
    Dim strYears As String
    Dim arrParam As Array

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
        intConfigsetting = Session("SS_CONFIGSETTING")
        strLocType = Session("SS_LOCTYPE")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumADAccessRights.ADBudgeting), intADAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")

        Else

            lblErrUsage.Visible = False
            onload_GetLangCap()
            If SortExpression.Text = "" Then
                SortExpression.Text = "ZA.VehTypeDistSetID"
                SortCol.Text = "ASC"
            End If

            If Not Page.IsPostBack Then
                AddVehDist()
                BindGrid()
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
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_VEHTYPEDIST_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_trx_VehTypeDist_Details.aspx")
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
        Dim Period As String
        InitializeBoundColumns()
        dgVehTypeDist.DataSource = LoadData()
        LoadTotal()
        dgVehTypeDist.DataBind()
        lblLocCode.Text = strLocation
        GetActivePeriod(Period)
        lblBgtPeriod.Text = Period
        Formatting()
    End Sub

    Sub Formatting()
        Dim EditButton As LinkButton
        Dim label As label
        Dim intCntCell As Integer = 4

        EditButton = dgVehTypeDist.Items.Item(CInt(dgVehTypeDist.Items.Count - 1)).FindControl("Edit")
        EditButton.Visible = False
        label = dgVehTypeDist.Items.Item(CInt(dgVehTypeDist.Items.Count - 1)).FindControl("lblParam")
        label.Visible = False

        While intCntCell < dgVehTypeDist.Columns.Count - 1
            dgVehTypeDist.Items.Item(CInt(dgVehTypeDist.Items.Count - 1)).Cells(intCntCell).Text = ""
            intCntCell += 2
        End While

    End Sub

    Protected Function GetActivePeriod(ByRef BGTPeriod As String) As String
        Dim strOppCd_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"
        Dim dsperiod As New DataSet()

        strParam = "|||||" & objBDSetup.EnumPeriodStatus.Active & "|" & strLocation & "|"

        Try
            intErrNo = objBDSetup.mtdGetPeriodList(strOppCd_GET, strParam, dsperiod)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_VehTypeDist_Details.aspx")
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

    Sub AddVehDist()
        Dim intError As Integer
         
        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
            strOppCd_YrPlanted_GET = "BD_CLSTRX_VEHTYPEDIST_BLOCK_YEAR_GET"
            strParam = GetActivePeriod("") & "|" & objGLSetup.EnumBlockType.MatureField & "','" & _
                       objGLSetup.EnumBlockType.InMatureField & "|" & objGLSetup.EnumBlockStatus.Active & "||||||"
        Else
            strOppCd_YrPlanted_GET = "BD_CLSTRX_VEHTYPEDIST_SUBBLOCK_YEAR_GET"
            strParam = GetActivePeriod("") & "|" & objGLSetup.EnumSubBlockType.MatureField & "','" & _
                       objGLSetup.EnumSubBlockType.InMatureField & "|" & objGLSetup.EnumSubBlockStatus.Active & "||||||"
        End If

        Try
            intErrNo = objBD.mtdUpdVehTypeDist(strOppCd_VehTypeDist_ADD, _
                                                strOppCd_VehTypeDist_GET, _
                                                strOppCd_VehTypeDist_Setup_GET, _
                                                strOppCd_VehTypeDist_UPD, _
                                                strOppCd_YrPlanted_GET, _
                                                strOppCd_VehTypeDistUsg_GET, _
                                                strOppCd_VehTypeDistUsgCost_SUM, _
                                                strOppCd_VehTypeDistParam_ADD, _
                                                strOppCd_VehTypeDistParam_GET, _
                                                strOppCd_VehTypeDistParam_UPD, _
                                                strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strParam, _
                                                objBDSetup.EnumOperation.Add, _
                                                intError)
            
        if intErrno <> 0 then
            lblErrMessage2.visible=true
        end if
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_VEHTYPEDIST_ADD&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_VehTypeDist_Details.aspx")
        End Try

    End Sub

    Sub DataGrid_ItemDataBound(ByVal Sender As Object, ByVal E As DataGridItemEventArgs)
        Dim intCntCell As Integer
        Dim intCntRem As Integer = 0
        Dim intCntCol As Integer = 4
        Dim intCntTotAreaFFB As Integer = 0
        Dim strColInfo As String
        Dim label As label
        Dim EditButton As LinkButton
        Dim arrBlkTotArea As Array
        Dim arrTotFFB As Array
        Select Case E.Item.ItemType
            Case ListItemType.Header
                While intCntCell <= dgVehTypeDist.Columns.Count - 1 - intCntRem
                    If intCntCell = 0 Then
                        intCntCell = 5
                    Else
                        intCntCell += 1
                    End If
                    E.Item.Cells.RemoveAt(intCntCell)
                    E.Item.Cells(intCntCell - 1).ColumnSpan = 2
                    intCntRem += 1
                End While

                BlockTotalAreaFFB()
                arrBlkTotArea = Split(lblBlkTotalArea.Text, "%")
                arrTotFFB = Split(lblTotalFFB.Text, "%")

                For intCntCol = 4 To E.Item.Cells.Count - 1
                    strColInfo = E.Item.Cells(intCntCol).Text
                    While intCntTotAreaFFB <= arrBlkTotArea.GetUpperBound(0) - 1

                        E.Item.Cells(intCntCol).Text = strColInfo & " <BR> Total Area : " & "<B>" & objGlobal.GetIDDecimalSeparator(FormatNumber(arrBlkTotArea(intCntTotAreaFFB), 0)) & "</B>"
                        E.Item.Cells(intCntCol).Text = E.Item.Cells(intCntCol).Text & " <BR> Total Yield : " & "<B>" & objGlobal.GetIDDecimalSeparator(FormatNumber(arrTotFFB(intCntTotAreaFFB), 0)) & "</B>"
                        
                        E.Item.Cells(intCntCol).Text = E.Item.Cells(intCntCol).Text & " <BR> Hrs/Km/Mt &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Cost"
                        intCntTotAreaFFB += 1
                        Exit While
                    End While
                Next
            Case ListItemType.Item, ListItemType.AlternatingItem
                If objDataSet.Tables(0).Rows.Count - 1 = E.Item.ItemIndex Then
                    E.Item.CssClass = "mr-h"
                    E.Item.Font.Bold = True
                End If
                If lblBgtStatus.Text.Trim = objBDSetup.EnumPeriodStatus.Addvote Then 
                    EditButton = E.Item.FindControl("Edit")
                    EditButton.Visible = False
                End If

        End Select

    End Sub

    Private Sub InitializeBoundColumns()
        Dim intCntYr As Integer
        Dim intCntIns As Integer
        Dim strInsert As String
        Dim strBlk As String





        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
            strOppCd_YrPlanted_GET = "BD_CLSTRX_VEHTYPEDIST_BLOCK_YEAR_GET"
            strParam = objGLSetup.EnumBlockType.MatureField & "','" & objGLSetup.EnumBlockType.InMatureField & "|" & _
                       objGLSetup.EnumBlockStatus.Active & "|" & strLocation
        Else
            strOppCd_YrPlanted_GET = "BD_CLSTRX_VEHTYPEDIST_SUBBLOCK_YEAR_GET"
            strParam = objGLSetup.EnumSubBlockType.MatureField & "','" & objGLSetup.EnumSubBlockType.InMatureField & "|" & _
                       objGLSetup.EnumSubBlockStatus.Active & "|" & strLocation
        End If

        Try
            intErrNo = objBD.mtdGetVehTypeDistYear(strOppCd_YrPlanted_GET, strParam, dsYrPlanted)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_VEHTYPEDIST_YEAR_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_VehTypeDist_Details.aspx")
        End Try

        For intCntYr = 0 To dsYrPlanted.Tables(0).Rows.Count - 1
            strBlk = Trim(dsYrPlanted.Tables(0).Rows(intCntYr).Item("BlkCode"))
            strInsert = Trim(dsYrPlanted.Tables(0).Rows(intCntYr).Item("BlkCode"))

            If intCntYr = 0 Then
                intCntIns = 4 
            Else
                intCntIns += 1
            End If

            Dim bcInsert As TemplateColumn = New TemplateColumn()
            bcInsert.HeaderText = strInsert
            bcInsert.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
            bcInsert.ItemStyle.Wrap = False
            bcInsert.ItemStyle.Width = Unit.Pixel(80)
            bcInsert.ItemStyle.HorizontalAlign = HorizontalAlign.Right
            bcInsert.ItemTemplate = New DataGridTemplate(ListItemType.Item, strInsert)
            dgVehTypeDist.Columns.AddAt(intCntIns, bcInsert)

            Dim bcInsert2 As TemplateColumn = New TemplateColumn()
            bcInsert2.HeaderText = strInsert & "a"
            bcInsert2.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
            bcInsert2.ItemStyle.Wrap = False
            bcInsert2.ItemStyle.Width = Unit.Pixel(80)
            bcInsert2.ItemStyle.HorizontalAlign = HorizontalAlign.Right
            bcInsert2.ItemTemplate = New DataGridTemplate(ListItemType.Item, strInsert & "a")
            intCntIns += 1
            dgVehTypeDist.Columns.AddAt(intCntIns, bcInsert2)

            strBlkCode = strBlkCode & "%" & strBlk
        Next

        lblYrPlant.Text = strBlkCode.TrimStart("%")
        lblYrPlant.Text = lblYrPlant.Text & "%"

        intCntIns += 1
        Dim gtUsg As TemplateColumn = New TemplateColumn()
        gtUsg.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
        gtUsg.HeaderText = "Grand Total"
        gtUsg.ItemStyle.HorizontalAlign = HorizontalAlign.Right
        gtUsg.ItemStyle.Wrap = False
        gtUsg.ItemStyle.Width = Unit.Pixel(80)
        gtUsg.ItemTemplate = New DataGridTemplate(ListItemType.Item, "GrandTotalUsage")
        dgVehTypeDist.Columns.AddAt(intCntIns, gtUsg)

        intCntIns += 1
        Dim gtCost As TemplateColumn = New TemplateColumn()
        gtCost.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
        gtCost.HeaderText = "Grand Total"
        gtCost.ItemStyle.HorizontalAlign = HorizontalAlign.Right
        gtCost.ItemStyle.Wrap = False
        gtCost.ItemStyle.Width = Unit.Pixel(80)
        gtCost.ItemTemplate = New DataGridTemplate(ListItemType.Item, "GrandTotalCost")
        dgVehTypeDist.Columns.AddAt(intCntIns, gtCost)

    End Sub

    Private Class DataGridTemplate
        Implements ITemplate
        Dim templateType As ListItemType
        Dim colname As String
        Dim colID As String

        Sub New(ByVal type As ListItemType, ByVal pv_colname As String)
            templateType = type
            colname = pv_colname
        End Sub

        Sub InstantiateIn(ByVal container As Control) Implements ITemplate.InstantiateIn
            Dim lc As New Literal()
            Dim lb As Label

            Select Case templateType
                Case ListItemType.Item
                    AddHandler lc.DataBinding, AddressOf TemplateControl_DataBinding
                    container.Controls.Add(lc)

            End Select

        End Sub

        Private Sub TemplateControl_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            Dim container As DataGridItem
            Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl() 

            lc = CType(sender, Literal)
            container = CType(lc.NamingContainer, DataGridItem)

            lc.Text = objGlobal.GetIDDecimalSeparator(FormatNumber(DataBinder.Eval(container.DataItem, colname), 0))
        End Sub

    End Class


    Protected Function LoadData() As DataSet

        strParam = "|" & _
                   strLocation & "|" & _
                   GetActivePeriod("") & "|AND ZA.LocCode *= VP.LocCode|" & _
                   SortExpression.Text & " " & SortCol.Text & "|" & _
                   lblYrPlant.Text & "|"
        Try
            intErrNo = objBD.mtdGetVehTypeDist(strOppCd_VehTypeDist_GET, strParam, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_VEHTYPEDISTUSAGE_DET&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_VehTypeDist_Details.aspx")
        End Try


        Return objDataSet

    End Function

    Sub BlockTotalAreaFFB()
        Dim strOppCd_VehTypeDist_TotalArea As String
        Dim strOppCd_VehTypeDist_TotalFFB As String = "BD_CLSTRX_VEHTYPEDIST_TOTALFFB"
        Dim intCntYr As Integer = 0
        Dim intCntCol As Integer = 0
        Dim strParamFFB As String
        Dim dsTotalFFB As New DataSet()
        Dim dsGrandTotalArea As New DataSet()
        Dim dsGrandTotalFFB As New DataSet()


        arrParam = Split(lblYrPlant.Text, "%")
        For intCntYr = 0 To arrParam.GetUpperBound(0) - 1

            If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
                strOppCd_VehTypeDist_TotalArea = "BD_CLSTRX_VEHTYPEDIST_BLKTOTALAREA"
                strParam = objGLSetup.EnumBlockType.MatureField & "','" & objGLSetup.EnumBlockType.InMatureField & "|" & _
                            objGLSetup.EnumBlockStatus.Active & "|" & strLocation & "|" & arrParam(intCntYr) & "|"
            Else
                strOppCd_VehTypeDist_TotalArea = "BD_CLSTRX_VEHTYPEDIST_SUBBLKTOTALAREA"
                strParam = objGLSetup.EnumSubBlockType.MatureField & "','" & objGLSetup.EnumSubBlockType.InMatureField & "|" & _
                            objGLSetup.EnumSubBlockStatus.Active & "|" & strLocation & "|" & arrParam(intCntYr) & "|"
            End If

            Try
                intErrNo = objBD.mtdGetBlockTotalArea(strOppCd_VehTypeDist_TotalArea, strParam, dsTotalArea)
            Catch Exp As System.Exception
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BLKTOTALAREA&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_VehTypeDist_Details.aspx")
            End Try

            strParamFFB = arrParam(intCntYr) & "|AND LocCode = '" & strLocation & "' AND PeriodID = '" & GetActivePeriod("") & "'"
            Try
                intErrNo = objBD.mtdGetVehTypeDist_TotalFFB(strOppCd_VehTypeDist_TotalFFB, strParamFFB, dsTotalFFB)
            Catch Exp As System.Exception
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BLKTOTALFFB&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_VehTypeDist_Details.aspx")
            End Try


            lblBlkTotalArea.Text = lblBlkTotalArea.Text & "%" & dsTotalArea.Tables(0).Rows(0).Item("TotalArea")
            lblTotalFFB.Text = lblTotalFFB.Text & "%" & dsTotalFFB.Tables(0).Rows(0).Item("TotalFFB")
        Next

        strYears = Replace(lblYrPlant.Text, "%", "','")
        strYears = Left(strYears, Len(strYears) - 3)

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
            strOppCd_VehTypeDist_TotalArea = "BD_CLSTRX_VEHTYPEDIST_BLKTOTALAREA"
            strParam = objGLSetup.EnumBlockType.MatureField & "','" & objGLSetup.EnumBlockType.InMatureField & "|" & _
                       objGLSetup.EnumBlockStatus.Active & "|" & strLocation & "|" & strYears & "|"
        Else
            strOppCd_VehTypeDist_TotalArea = "BD_CLSTRX_VEHTYPEDIST_SUBBLKTOTALAREA"
            strParam = objGLSetup.EnumSubBlockType.MatureField & "','" & objGLSetup.EnumSubBlockType.InMatureField & "|" & _
                       objGLSetup.EnumSubBlockStatus.Active & "|" & strLocation & "|" & strYears & "|"
        End If

        Try
            intErrNo = objBD.mtdGetBlockTotalArea(strOppCd_VehTypeDist_TotalArea, strParam, dsGrandTotalArea)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BLKGTOTALAREA&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_VehTypeDist_Details.aspx")
        End Try

        strParamFFB = strYears & "|AND LocCode = '" & strLocation & "' And PeriodID = " & GetActivePeriod("")
        Try
            intErrNo = objBD.mtdGetVehTypeDist_TotalFFB(strOppCd_VehTypeDist_TotalFFB, strParamFFB, dsGrandTotalFFB)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BLKGTOTALFFB&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_VehTypeDist_Details.aspx")
        End Try

        lblBlkTotalArea.Text = lblBlkTotalArea.Text.TrimStart("%")
        lblBlkTotalArea.Text = lblBlkTotalArea.Text & "%" & dsGrandTotalArea.Tables(0).Rows(0).Item("TotalArea") & "%"

        lblTotalFFB.Text = lblTotalFFB.Text.TrimStart("%")
        lblTotalFFB.Text = lblTotalFFB.Text & "%" & dsGrandTotalFFB.Tables(0).Rows(0).Item("TotalFFB") & "%"


    End Sub

    Protected Sub LoadTotal()
        Dim strOppCd_VehTypeDistParam_GTCost_SUM As String = "BD_CLSTRX_VEHTYPEDISTPARAM_GTCOST_SUM"
        Dim dsTotalCost As New DataSet()
        Dim dsGTCost As New DataSet()
        Dim intCntYr As Integer = 0
        Dim strParamCost As String
        Dim strParamGTCost As String

        dr = objDataSet.Tables(0).NewRow()
        dr("VehTypeDistSetID") = 0
        dr("Activity") = "TOTAL"
        dr("VehType") = ""
        dr("Parameter") = 0

        arrParam = Split(lblYrPlant.Text, "%")
        For intCntYr = 0 To arrParam.GetUpperBound(0) - 1

            strParamCost = "|" & strLocation & "|" & GetActivePeriod("") & "||WHERE BD.BlkCode = '" & arrParam(intCntYr) & "'|"
            Try
                intErrNo = objBD.mtdGetVehTypeDistParam(strOppCd_VehTypeDistUsgCost_SUM, strParamCost, dsTotalCost)
            Catch Exp As System.Exception
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_VEHTYPEDIST_DET_TOTALCOST_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_VehTypeDist_Details.aspx")
            End Try

            dr(arrParam(intCntYr)) = 0
            dr(arrParam(intCntYr) & "a") = dsTotalCost.Tables(0).Rows(0).Item("Cost")

        Next
        strParamGTCost = "|" & strLocation & "|" & GetActivePeriod("") & "|||"
        Try
            intErrNo = objBD.mtdGetVehTypeDistParam(strOppCd_VehTypeDistParam_GTCost_SUM, strParamGTCost, dsGTCost)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_VEHTYPEDISTPARAM_GTCOST_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_VehTypeDist_Details.aspx")
        End Try

        dr("GrandTotalUsage") = 0
        dr("GrandTotalCost") = dsGTCost.Tables(0).Rows(0).Item("GrandTotalCost")
        dr("Status") = ""
        dr("PeriodID") = 0
        dr("LocCode") = 0
        dr("CreateDate") = "01-Jan-1900"
        dr("UpdateDate") = "01-Jan-1900"
        dr("UserName") = ""
        objDataSet.Tables(0).Rows.InsertAt(dr, objDataSet.Tables(0).Rows.Count + 1)
        
    End Sub

    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        lblerrmessage2.visible=false
        lblOper.Text = objBD.EnumOperation.Update
        dgVehTypeDist.EditItemIndex = CInt(E.Item.ItemIndex)

        BindGrid()
    End Sub

    Sub DEDR_Update(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim label As label
        Dim EditText As TextBox
        Dim intError As Integer
        dim ErrorMsg as string 
        Dim strVehTypeDistID As String
        Dim strVehTypeCode As String
        Dim strParameter As String
        Dim dsTotalArea As New DataSet()
        Dim arrYrs As Array
        Dim intCntYrs As Integer
        Dim strOppCd_VehTypeDist_TotalArea As String

        arrYrs = Split(lblYrPlant.Text, "%")

        label = E.Item.FindControl("lblID")
        strVehTypeDistID = label.Text
        label = E.Item.FindControl("lblVehTypeCode")
        strVehTypeCode = label.Text
        EditText = E.Item.FindControl("txtParam")
        strParameter = EditText.Text

        For intCntYrs = 0 To arrYrs.GetUpperBound(0) - 1
            If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
                strOppCd_VehTypeDist_TotalArea = "BD_CLSTRX_VEHTYPEDIST_BLKTOTALAREA"
                strParam = objGLSetup.EnumBlockType.MatureField & "','" & objGLSetup.EnumBlockType.InMatureField & "|" & _
                           objGLSetup.EnumBlockStatus.Active & "|" & strLocation & "|" & arrYrs(intCntYrs) & "|"
            Else
                strOppCd_VehTypeDist_TotalArea = "BD_CLSTRX_VEHTYPEDIST_SUBBLKTOTALAREA"
                strParam = objGLSetup.EnumSubBlockType.MatureField & "','" & objGLSetup.EnumSubBlockType.InMatureField & "|" & _
                           objGLSetup.EnumSubBlockStatus.Active & "|" & strLocation & "|" & arrYrs(intCntYrs) & "|"
            End If

            Try
                intErrNo = objBD.mtdGetBlockTotalArea(strOppCd_VehTypeDist_TotalArea, strParam, dsTotalArea)
            Catch Exp As System.Exception
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BLKTOTALAREA_YR&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_VehTypeDist_Details.aspx")
            End Try

            strParam = GetActivePeriod("") & "||" & objBD.EnumVehTypeDistStatus.Budgeted & "|" & strVehTypeDistID & "|" & _
                       Trim(strParameter) & "|" & Trim(dsTotalArea.Tables(0).Rows(0).Item("TotalArea")) & "|" & _
                       strVehTypeCode & "|" & arrYrs(intCntYrs) & "|"
            Try
                intErrNo = objBD.mtdUpdVehTypeDist(strOppCd_VehTypeDist_ADD, _
                                                   strOppCd_VehTypeDist_GET, _
                                                   strOppCd_VehTypeDist_Setup_GET, _
                                                   strOppCd_VehTypeDist_UPD, _
                                                   strOppCd_YrPlanted_GET, _
                                                   strOppCd_VehTypeDistUsg_GET, _
                                                   strOppCd_VehTypeDistUsgCost_SUM, _
                                                   strOppCd_VehTypeDistParam_ADD, _
                                                   strOppCd_VehTypeDistParam_GET, _
                                                   strOppCd_VehTypeDistParam_UPD, _
                                                   strCompany, _
                                                   strLocation, _
                                                   strUserId, _
                                                   strParam, _
                                                   lblOper.Text, _
                                                   intError)
                                                   
            if intErrno <> 0 then
                lblErrMessage2.visible=true
            end if
            Catch Exp As System.Exception
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_VEHTYPEDIST_UPD&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_VehTypeDist_Details.aspx")
            End Try
        Next

        If intError = objBD.EnumErrorType.CalculationErr Then
            lblErrUsage.Visible = True
        Else

            If E.Item.ItemIndex + 1 = dgVehTypeDist.Items.Count - 1 Then
                dgVehTypeDist.EditItemIndex = -1
            Else
                dgVehTypeDist.EditItemIndex = E.Item.ItemIndex + 1
            End If
        End If
        BindGrid()

    End Sub

    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        If CInt(E.Item.ItemIndex) = 0 And dgVehTypeDist.Items.Count = 1 And dgVehTypeDist.PageCount <> 1 Then
            dgVehTypeDist.CurrentPageIndex = dgVehTypeDist.PageCount - 2
        End If
        dgVehTypeDist.EditItemIndex = -1
        BindGrid()
    End Sub

End Class
