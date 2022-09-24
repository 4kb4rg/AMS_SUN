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


Public Class BD_MatureCrop_Dist : Inherits Page

    Protected WithEvents MonthList As DataGrid
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblPrcntErrTop As Label
    Protected WithEvents lblPrcntErr As Label
    Protected WithEvents lblFigureErrTop As Label
    Protected WithEvents lblFigureErr As Label
    Protected WithEvents lblOvrMsgTop As Label
    Protected WithEvents lblOvrMsg As Label
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label

    Protected WithEvents lblTitle As Label
    Protected WithEvents ddlDistribute As DropDownList
    Protected WithEvents lblMatureCropDistID As Label
    Protected WithEvents lblAccCode As Label
    Protected WithEvents lblLocTag As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblBlockTag As Label
    Protected WithEvents lblSubBlkTag As Label
    Protected WithEvents lblDistFig As Label
    Protected WithEvents lblBlkCode As Label
    Protected WithEvents lblSubBlkCode As Label
    Protected WithEvents lblYearPlanted As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents RowSubBlk As HtmlTableRow
    Protected WithEvents RowPlantYr As HtmlTableRow
    Protected WithEvents hidDistByBlk As HtmlInputHidden
    Protected WithEvents hidBlkCode As HtmlInputHidden

    Dim objBD As New agri.BD.clsTrx()
    Dim objBDSetup As New agri.BD.clsSetup()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl() 
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Dim strOppCd_MatureCropDist_UPD As String = "BD_CLSTRX_MATURECROPDIST_UPD"
    Dim strOppCd_MatureCropDist_AccPeriod_UPD As String = "BD_CLSTRX_MATURECROPDIST_ACCPERIOD_UPD"

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
    Private strSqlConn As String
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

            lblOvrMsgTop.Visible = False
            lblOvrMsg.Visible = False
            lblPrcntErrTop.Visible = False
            lblPrcntErr.Visible = False
            lblFigureErrTop.Visible = False
            lblFigureErr.Visible = False

            lblBlkCode.Text = Request.QueryString("blkcode").Trim
            lblSubBlkCode.Text = Request.QueryString("subblkcode").Trim
            lblMatureCropDistID.Text = Request.QueryString("id").Trim
            lblAccCode.Text = Request.QueryString("acccode").Trim
            lblYearPlanted.Text = Request.QueryString("yr").Trim
            hidDistByBlk.Value = Request.QueryString("DistByBlk").Trim

            If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
                hidBlkCode.Value = lblBlkCode.Text
            Else
                hidBlkCode.Value = lblSubBlkCode.Text
            End If

            If Not Page.IsPostBack Then
                onload_GetLangCap()
                BindDistList()
                BindGrid()
                DisableRow()
            End If

            If SortExpression.Text = "" Then
                SortExpression.Text = "AccCode"
                SortCol.Text = "ASC"
            End If
        End If

    End Sub

    Sub DisableRow()
        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
            RowSubBlk.Visible = False
        Else
            If hidDistByBlk.Value = True Then
                RowSubBlk.Visible = False
                RowPlantYr.Visible = False
            Else
                RowSubBlk.Visible = True
            End If
        End If
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()

        lblTitle.Text = "MATURE CROP ACTIVITY CALENDERISATION"
        lblLocTag.Text = GetCaption(objLangCap.EnumLangCap.Location) & lblCode.Text
        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
            lblBlockTag.Text = GetCaption(objLangCap.EnumLangCap.Block) & lblCode.Text
        Else
            lblBlockTag.Text = GetCaption(objLangCap.EnumLangCap.Block) & lblCode.Text
            lblSubBlkTag.Text = GetCaption(objLangCap.EnumLangCap.SubBlock) & lblCode.Text
        End If
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BD_TRX_MATURECROPDIST_DISTRIBUTE_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_MatureCropDist_Details.aspx")
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
        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
            ddlDistribute.Items.Add(New ListItem(objBD.mtdGetDistMethod(objBD.EnumDistMethod.Figure), objBD.EnumDistMethod.Figure))
        Else
            If hidDistByBlk.Value = False Then
                ddlDistribute.Items.Add(New ListItem(objBD.mtdGetDistMethod(objBD.EnumDistMethod.Figure), objBD.EnumDistMethod.Figure))
            End If
        End If
        ddlDistribute.Items.Add(New ListItem(objBD.mtdGetDistMethod(objBD.EnumDistMethod.Percentage), objBD.EnumDistMethod.Percentage))

    End Sub

    Sub BindGrid()
        Dim Period As String

        MonthList.DataSource = LoadData()
        MonthList.DataBind()
        lblLocCode.Text = strLocation
        GetActivePeriod(Period)
        lblBgtPeriod.Text = Period
        LoadTotal()
    End Sub

    Protected Function LoadTotal() As Decimal
        Dim strOppCd_MatureCropDist_YrBudgetCost_SUM As String
        Dim dsTotals As DataSet
        Dim decTotalAddVote As Decimal
        Dim strLoadTotalCond As String

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
            strOppCd_MatureCropDist_YrBudgetCost_SUM = "BD_CLSTRX_MATURECROPDIST_YRBUDGETCOST_SUM"
            strLoadTotalCond = "|" & strLocation & "|" & GetActivePeriod("") & "|" & hidBlkCode.Value & "|AND MCS.AccCode = '" & lblAccCode.Text & "'|"
        Else
            If hidDistByBlk.Value = True Then
                strOppCd_MatureCropDist_YrBudgetCost_SUM = "BD_CLSTRX_MATURECROPDIST_YRBUDGETCOST_SUM_SBLK"
                strLoadTotalCond = "|" & strLocation & "|" & GetActivePeriod("") & "||AND MCS.Acccode = '" & lblAccCode.Text & "' AND SBLK.BlkCode = '" & lblBlkCode.Text & "'|"
            Else
                strOppCd_MatureCropDist_YrBudgetCost_SUM = "BD_CLSTRX_MATURECROPDIST_YRBUDGETCOST_SUM"
                strLoadTotalCond = "|" & strLocation & "|" & GetActivePeriod("") & "|" & hidBlkCode.Value & "|AND MCS.AccCode = '" & lblAccCode.Text & "'|"
            End If
        End If
        
        If GetBudgetPeriod() = 6 Then 
            Dim objSqlConn As SqlConnection
            GetDBStringProp()
            objSqlConn = New SqlConnection(strSqlConn)

            decTotalAddVote = RunSPReturnValue(objSqlConn, "BD_CLSTRX_MATURECROPDIST_SUM_ADDVOTE_GET", _
                                                "@AccCode", lblAccCode.Text, _
                                                "@PeriodID", GetActivePeriod(""), _
                                                "@BlkCode", hidBlkCode.Value, _
                                                "@LocCode", strLocation)

             
            lblDistFig.Text = objGlobal.GetIDDecimalSeparator(FormatNumber(Trim(decTotalAddVote), 0))
            Return Trim(decTotalAddVote)
        Else
            Try
                intErrNo = objBD.mtdGetMatureCrop(strOppCd_MatureCropDist_YrBudgetCost_SUM, strLoadTotalCond, dsTotals)
            Catch Exp As System.Exception
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_MATURECROPDIST_LOADTOTALS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_MatureCropDist_Details.aspx")
            End Try


            lblDistFig.Text = objGlobal.GetIDDecimalSeparator(FormatNumber(Trim(dsTotals.Tables(0).Rows(0).Item("YrBudgetCost")), 0))
            Return Trim(dsTotals.Tables(0).Rows(0).Item("YrBudgetCost"))
        End If
    End Function

    Protected Function LoadData() As DataSet
        Dim strOppCd_MatureCropDist_AccPeriod_GET As String = "BD_CLSTRX_MATURECROPDIST_ACCPERIOD_GET"

        strParam = "|" & strLocation & "|" & GetActivePeriod("") & "||||"
        Try
            intErrNo = objBD.mtdGetMatureCropDist(strOppCd_MatureCropDist_AccPeriod_GET, strParam, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_MATURECROPDIST_LOADDATA&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_MatureCropDist_Details.aspx")
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
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_MATURECROPDIST_GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_MatureCropDist_Details.aspx")
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
                decEvenFig = LoadTotal() / objDataSet.Tables(0).Rows.Count
                For intcnt = 0 To objDataSet.Tables(0).Rows.Count - 1
                    If intcnt = objDataSet.Tables(0).Rows.Count - 1 Then
                        decEvenFig = LoadTotal() - decCount
                    End If
                    txt = MonthList.Items.Item(CInt(intcnt)).FindControl("TxFig")

                    txt.Text = objGlobal.GetIDDecimalSeparator(Round(decEvenFig, 0))
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
        Dim strOppCd_MatureCropDist_YrBudgetCost_SUM As String
        Dim strOppCd_MatureCropDist_ID_GET As String = "BD_CLSTRX_MATURECROPDIST_ID_GET"
        Dim strOppCd_MatureCropDist_AccPeriod_GET As String = "BD_CLSTRX_MATURECROPDIST_ACCPERIOD_GET"
        Dim interror As Integer
        Dim strParam As String
        Dim decFig As Decimal
        Dim decFigCtrl As Decimal
        Dim intCnt As Integer
        Dim intBgtPeriod As Integer

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
            intBgtPeriod = GetBudgetPeriod()
            txt = MonthList.Items.Item(CInt(intCnt)).FindControl("TxFig")
            lblMonth = MonthList.Items.Item(CInt(intCnt)).FindControl("lblMonth")
            lblYear = MonthList.Items.Item(CInt(intCnt)).FindControl("lblYear")

            Select Case ddlDistribute.SelectedItem.Value
                Case objBD.EnumDistMethod.Even
                    If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
                        strOppCd_MatureCropDist_YrBudgetCost_SUM = "BD_CLSTRX_MATURECROPDIST_YRBUDGETCOST_SUM"
                        strParam = GetActivePeriod("") & "|" & lblMatureCropDistID.Text & "|" & txt.Text.Trim & "||" & lblAccCode.Text.Trim & "|" & lblMonth.Text.Trim & "|" & lblYear.Text.Trim & "|" & hidBlkCode.Value & "|"
                    Else
                        If hidDistByBlk.Value = True Then
                            strOppCd_MatureCropDist_YrBudgetCost_SUM = "BD_CLSTRX_MATURECROPDIST_YRBUDGETCOST_SUM_SBLK"
                            strParam = GetActivePeriod("") & "||||" & lblAccCode.Text.Trim & "|" & lblMonth.Text.Trim & "|" & lblYear.Text.Trim & "|" & lblBlkCode.Text.Trim & "|"
                        Else
                            strOppCd_MatureCropDist_YrBudgetCost_SUM = "BD_CLSTRX_MATURECROPDIST_YRBUDGETCOST_SUM"
                            strParam = GetActivePeriod("") & "|" & lblMatureCropDistID.Text & "|" & txt.Text.Trim & "||" & lblAccCode.Text.Trim & "|" & lblMonth.Text.Trim & "|" & lblYear.Text.Trim & "|" & hidBlkCode.Value & "|"
                        End If
                    End If
                Case objBD.EnumDistMethod.Figure
                    strParam = GetActivePeriod("") & "|" & lblMatureCropDistID.Text & "|" & txt.Text.Trim & "||" & lblAccCode.Text.Trim & "|" & lblMonth.Text.Trim & "|" & lblYear.Text.Trim & "|" & hidBlkCode.Value & "|"
                    strOppCd_MatureCropDist_YrBudgetCost_SUM = "BD_CLSTRX_MATURECROPDIST_YRBUDGETCOST_SUM"
                Case objBD.EnumDistMethod.Percentage
                    If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
                        strOppCd_MatureCropDist_YrBudgetCost_SUM = "BD_CLSTRX_MATURECROPDIST_YRBUDGETCOST_SUM"
                        strParam = GetActivePeriod("") & "|" & lblMatureCropDistID.Text & "||" & txt.Text.Trim & "|" & lblAccCode.Text.Trim & "|" & lblMonth.Text.Trim & "|" & lblYear.Text.Trim & "|" & hidBlkCode.Value & "|"
                    Else
                        If hidDistByBlk.Value = True Then
                            strOppCd_MatureCropDist_YrBudgetCost_SUM = "BD_CLSTRX_MATURECROPDIST_YRBUDGETCOST_SUM_SBLK"
                            strParam = GetActivePeriod("") & "|||" & txt.Text.Trim & "|" & lblAccCode.Text.Trim & "|" & lblMonth.Text.Trim & "|" & lblYear.Text.Trim & "|" & lblBlkCode.Text.Trim & "|"
                        Else
                            strOppCd_MatureCropDist_YrBudgetCost_SUM = "BD_CLSTRX_MATURECROPDIST_YRBUDGETCOST_SUM"
                            strParam = GetActivePeriod("") & "|" & lblMatureCropDistID.Text & "||" & txt.Text.Trim & "|" & lblAccCode.Text.Trim & "|" & lblMonth.Text.Trim & "|" & lblYear.Text.Trim & "|" & hidBlkCode.Value & "|"
                        End If
                    End If
            End Select

            Try
                intErrNo = objBD.mtdUpdMatureCropDist(strOppCd_MatureCropDist_AccPeriod_UPD, _
                                                      strOppCd_MatureCropDist_UPD, _
                                                      strOppCd_MatureCropDist_YrBudgetCost_SUM, _
                                                      strOppCd_MatureCropDist_ID_GET, _
                                                      strOppCd_MatureCropDist_AccPeriod_GET, _
                                                      strCompany, _
                                                      strLocation, _
                                                      strUserId, _
                                                      strParam, _
                                                      interror, _
                                                      hidDistByBlk.Value, _
                                                      intBgtPeriod)
            Catch Exp As System.Exception
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_MATURECROPDIST_CONFIRM&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_MatureCropDist_Details.aspx")
            End Try

            If interror = objBDSetup.EnumErrorType.Overflow Then
                lblOvrMsgTop.Visible = True
                lblOvrMsg.Visible = True
            End If
        Next

        If intErrNo = 0 Then
            Response.Write("<Script Language=""JavaScript"">opener.location.href='BD_trx_MatureCropDist_Details.aspx?DistByBlk=" & hidDistByBlk.Value & "&blkcode=" & lblBlkCode.Text & "&subblkcode=" & lblSubBlkCode.Text & "&acccode=" & lblAccCode.Text & "&yr=" & lblYearPlanted.Text & "';window.close();</Script>")
        End If
    End Sub

    Private Function GetBudgetPeriod() As Integer
        Dim intReturnValue As Integer

        Dim objSqlConn As SqlConnection
        GetDBStringProp()
        objSqlConn = New SqlConnection(strSqlConn)

        intReturnValue = RunSPReturnValue(objSqlConn, "BD_CLSTRX_PERIOD_STATUS_GET", _
                                          "@PeriodID", GetActivePeriod(""), _
                                          "@LocCode", strLocation)
        objSqlConn = Nothing
        Return intReturnValue
    End Function

    Sub btnCancel_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Response.Write("<Script Language=""JavaScript"">window.close();</Script>")
    End Sub

#Region " -- DBHelper -- "
    Private Sub GetDBStringProp()
        Dim objConn As New agri.Admin.clsBackupRestore
        Dim strServer As String
        Dim strDatabase As String
        Dim strUserID As String
        Dim strPassword As String

        objConn.GetDBStringProp(strServer, strDatabase, strUserID, strPassword)
        objConn = Nothing

        Me.strSqlConn = "server=" & strServer & ";User ID=" & strUserID & ";Password=" & _
                        strPassword & ";database=" & strDatabase & ";Connection Reset=FALSE"
    End Sub

    Private Function RunSPReturnValue(ByRef Connection As SqlConnection, ByVal sProcName As String, _
                                            ByVal ParamArray args As Object()) As Object
        Dim dtRow As DataRow = RunSPReturnDR(Connection, sProcName, args)
        If Not (dtRow Is Nothing) Then Return dtRow.Item(0)
    End Function

    Private Function RunSPReturnDR(ByRef Connection As SqlConnection, ByVal sProcName As String, _
                                         ByVal ParamArray args As Object()) As DataRow
        Dim dtTable As DataTable = RunSPReturnDT(Connection, sProcName, args)
        If (dtTable.Rows.Count > 0) Then Return dtTable.Rows(0)
    End Function
    
    Private Function RunSPReturnDT(ByRef Connection As SqlConnection, ByVal sProcName As String, _
                                         ByVal ParamArray args As Object()) As DataTable
        Dim dsSet As DataSet = RunSPReturnDS(Connection, sProcName, args)
        If (dsSet.Tables.Count > 0) Then Return dsSet.Tables(0)
    End Function

    Private Function RunSPReturnDS(ByRef oConnection As SqlConnection, ByVal sProcName As String, _
                                         ByVal ParamArray args As Object()) As DataSet
        Dim cnConn As SqlConnection
        Dim cmdSql As SqlCommand
        Dim dsSet As DataSet = New DataSet

        Try
            cnConn = oConnection
            cmdSql = GetCommandSP(oConnection, sProcName, args)

            Dim adpSql As SqlDataAdapter = New SqlDataAdapter(cmdSql)
            adpSql.Fill(dsSet)
        Catch exc As System.Exception
            Throw exc
        End Try
        Return dsSet
    End Function

    Private Function GetCommandSP(ByRef cnConn As SqlConnection, _
                                  ByVal sProcName As String, ByVal ParamArray args As Object()) As SqlCommand

        Dim cmdSql As SqlCommand = New SqlCommand(sProcName, cnConn)
        cmdSql.CommandType = CommandType.StoredProcedure

        Dim i As Integer = 0
        While (i < args.Length)
            If ((i + 1) >= args.Length) Then Exit While
            Dim sParamName As String = CType(args(i), String)
            Dim oVal As Object = args(i + 1)
            If IsDBNull(oVal) Then
                cmdSql.Parameters.Add(New SqlParameter(sParamName, oVal))
            Else
                If Len(oVal) >= 4000 Then
                    Dim NewParameter As New SqlParameter
                    With NewParameter
                        .ParameterName = sParamName
                        .SqlDbType = SqlDbType.Text
                        .Direction = ParameterDirection.Input
                        .Value = oVal
                    End With
                    cmdSql.Parameters.Add(NewParameter)
                Else
                    cmdSql.Parameters.Add(New SqlParameter(sParamName, oVal))
                End If
            End If
            i += 2
        End While
        Return cmdSql
    End Function
#End Region

End Class
