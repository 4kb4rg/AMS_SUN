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


Public Class BD_MatureCropDist_Details : Inherits Page

    Protected WithEvents dgMatureCropDist As DataGrid
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblBudgetingErr As Label
    Protected WithEvents lblPeriod As Label
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label

    Protected WithEvents lblLocTag As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBudgeting As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblBlockTag As Label
    Protected WithEvents lblSubBlkTag As Label
    Protected WithEvents lblBlkCode As Label
    Protected WithEvents lblSubBlkCode As Label
    Protected WithEvents lblYearPlanted As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents RowSubBlk As HtmlTableRow
    Protected WithEvents RowPlantingYr As HtmlTableRow
    Protected WithEvents hidBlkCode As HtmlInputHidden
    Protected trPeriodTitle As HtmlTableRow

    Dim objBD As New agri.BD.clsTrx()
    Dim objBDSetup As New agri.BD.clsSetup()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objAdminLoc As New agri.Admin.clsLoc()
    
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl() 

    Dim strOppCd_Period_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"

    Dim objDataSet As New DataSet()
    Dim dsPeriod As New DataSet()
    Dim objLangCapDs As New DataSet()
    Dim dr As DataRow

    Dim intErrNo As Integer
    Dim intError As Integer
    Dim strParam As String = ""
    Dim arrParam As Array

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim intADAR As Integer
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim intConfigsetting As Integer
    Private strSqlConn As String
    Private decAddVoteFooterTotal As Decimal
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

            onload_GetLangCap()

            lblBudgetingErr.Visible = False
            lblYearPlanted.Text = Request.QueryString("yr").Trim
            lblBlkCode.Text = Request.QueryString("blkcode").Trim
            lblSubBlkCode.Text = Request.QueryString("subblkcode").Trim

            If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
                hidBlkCode.Value = lblBlkCode.Text
            Else
                hidBlkCode.Value = lblSubBlkCode.Text
            End If

            If SortExpression.Text = "" Then
                SortExpression.Text = "MCD.AccCode"
                SortCol.Text = "ASC"
            End If

            If Not Page.IsPostBack Then
                BindGrid()
                DisableRow()
            End If
        End If
    End Sub

    Sub DisableRow()
        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
            RowSubBlk.Visible = False
        Else
            If Request.QueryString("distbyblk") = False Then
                RowSubBlk.Visible = True
            Else
                RowSubBlk.Visible = False
                RowPlantingYr.Visible = False
            End If
        End If
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()

        lblLocTag.Text = GetCaption(objLangCap.EnumLangCap.Location) & lblCode.text
        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
            lblBlockTag.Text = GetCaption(objLangCap.EnumLangCap.Block) & lblCode.Text
        Else
            lblBlockTag.Text = GetCaption(objLangCap.EnumLangCap.Block) & lblCode.Text
            lblSubBlkTag.Text = GetCaption(objLangCap.EnumLangCap.SubBlock) & lblCode.Text
        End If

        dgMatureCropDist.Columns(0).HeaderText = GetCaption(objLangCap.EnumLangCap.Account) & lblCode.Text
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
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_MATURECROPDIST_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_trx_MatureCropDist_Details.aspx")
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
        dgMatureCropDist.DataSource = LoadData()
        LoadTotal()
        dgMatureCropDist.DataBind()
        lblLocCode.Text = strLocation
        GetActivePeriod(Period)
        lblBgtPeriod.Text = Period
        Formatting()

        dgMatureCropDist.Items.Item(dgMatureCropDist.Items.Count - 1).Font.Bold = True
    End Sub

    Sub Formatting()
        Dim EditButton As LinkButton
        Dim intCntCell As Integer = 3

        EditButton = dgMatureCropDist.Items.Item(CInt(dgMatureCropDist.Items.Count - 1)).FindControl("Edit")
        EditButton.Visible = False
    End Sub

    Protected Function GetActivePeriod(ByRef BGTPeriod As String) As String
        Dim dsperiod As New DataSet()

        strParam = "|||||" & objBDSetup.EnumPeriodStatus.Active & "|" & strLocation & "|"
        Try
            intErrNo = objBDSetup.mtdGetPeriodList(strOppCd_Period_GET, strParam, dsperiod)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_MATURECROPDIST_GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_MatureCropDist_Details.aspx")
        End Try

        If dsperiod.Tables(0).Rows.Count > 0 Then
            BGTPeriod = dsperiod.Tables(0).Rows(0).Item("BGTPeriod")
            Return dsperiod.Tables(0).Rows(0).Item("PeriodID")
        Else
            BGTPeriod = "No Active Period"
            Response.Redirect("../../BD/Setup/BD_setup_Periods.aspx")
        End If
    End Function

    Private Sub InitializeBoundColumns()
        Dim strOppCd_MatureCropDist_AccPeriod_GET As String = "BD_CLSTRX_MATURECROPDIST_ACCPERIOD_GET"
        Dim intCntPeriod As Integer
        Dim strAccPeriod As String
        Dim intYear As Integer
        Dim intCntIns As Integer
        Dim strPeriod As String
        Dim intAddVoteCol As Integer

        strParam = "|" & strLocation & "|" & GetActivePeriod("") & "||||"
        Try
            intErrNo = objBD.mtdGetMatureCropDist(strOppCd_MatureCropDist_AccPeriod_GET, _
                                                  strParam, _
                                                  dsPeriod)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_MATURECROPDIST_ACCPERIOD_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_MatureCropDist_Details.aspx")
        End Try

        For intCntPeriod = 0 To dsPeriod.Tables(0).Rows.Count - 1
            strAccPeriod = Trim(dsPeriod.Tables(0).Rows(intCntPeriod).Item("AccMonth")) & "/" & Trim(dsPeriod.Tables(0).Rows(intCntPeriod).Item("AccYear"))
            
            GenPeriodTitle(intCntPeriod, strAccPeriod)

            If intCntPeriod = 0 Then
                intCntIns = 5
                intAddVoteCol = 6
            Else
                intCntIns += 2
                intAddVoteCol = intCntIns + 1
            End If

            Dim bcInsert As TemplateColumn = New TemplateColumn()
            With bcInsert
                .HeaderText = "Cost" 
                .HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                .ItemStyle.Wrap = False
                .ItemStyle.Width = Unit.Pixel(70)
                .ItemStyle.HorizontalAlign = HorizontalAlign.Right
                .ItemTemplate = New DataGridTemplate(ListItemType.Item, strAccPeriod)
            End With
          
            dgMatureCropDist.Columns.AddAt(intCntIns, bcInsert)
            strPeriod = strPeriod & "%" & strAccPeriod

            Dim tmpcolAddVote As TemplateColumn = New TemplateColumn()
            With tmpcolAddVote
                .HeaderText = "Add Vote" 
                .HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                .ItemStyle.Wrap = False
                .ItemStyle.Width = Unit.Pixel(70)
                .ItemStyle.HorizontalAlign = HorizontalAlign.Right
                .ItemTemplate = New DataGridTemplate(ListItemType.Item, strAccPeriod & "AddVote")
            End With
           
            dgMatureCropDist.Columns.AddAt(intAddVoteCol, tmpcolAddVote)
        Next

        If Not strPeriod = "" Then
            lblPeriod.Text = strPeriod.TrimStart("%")
            lblPeriod.Text = lblPeriod.Text & "%"
        End If

    End Sub

    Private Sub GenPeriodTitle(ByVal i As Integer, ByVal strAccPeriod As String)
        Dim objCell As New HtmlTableCell
        With objCell
            .Align = "Center"
            .Style.Add("width", "140px")
            .Style.Add("border-right-style", "solid")
            .Style.Add("border-right-width", "1px")
            .NoWrap = True
            .BgColor = "#CCCCCC"
            .InnerHtml = "<b>Period " & strAccPeriod & "</b>"
        End With
        trPeriodTitle.Cells.Add(objCell)
        objCell = Nothing
    End Sub

    Private Class DataGridTemplate
        Implements ITemplate
        Dim templateType As ListItemType
        Dim colname As String

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
            If IsDBNull(DataBinder.Eval(container.DataItem, colname)) Then
                lc.Text = ""
            Else

                lc.Text = objGlobal.GetIDDecimalSeparator(FormatNumber(DataBinder.Eval(container.DataItem, colname), 0))
            End If
        End Sub

    End Class


    Protected Function LoadData() As DataSet
        Dim strOppCd_MatureCropDist_Format_GET As String
        Dim strParam As String
        Dim blnIsTotal As Boolean

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
            strOppCd_MatureCropDist_Format_GET = "BD_CLSTRX_MATURECROPDIST_FORMAT_GET"
            strParam = strLocation & "|" & GetActivePeriod("") & "|||" & hidBlkCode.Value & "||" & _
                       SortExpression.Text & " " & SortCol.Text
            blnIsTotal = False
        Else
            If Request.QueryString("distbyblk") = True Then
                strOppCd_MatureCropDist_Format_GET = "BD_CLSTRX_MATURECROPDIST_FORMAT_SBLK_GET"
                strParam = strLocation & "|" & GetActivePeriod("") & "||||  AND SBLK.BlkCode = '" & lblBlkCode.Text & "'|" '& _
                blnIsTotal = True
            Else
                strOppCd_MatureCropDist_Format_GET = "BD_CLSTRX_MATURECROPDIST_FORMAT_GET"
                strParam = strLocation & "|" & GetActivePeriod("") & "|||" & hidBlkCode.Value & "||" & _
                           SortExpression.Text & " " & SortCol.Text
                blnIsTotal = False
            End If
        End If

        Try
            intErrNo = objBD.mtdGetMatureCropDistFormat(strOppCd_MatureCropDist_Format_GET, _
                                                      strOppCd_Period_GET, _
                                                      strParam, _
                                                      objDataSet, _
                                                      intError, _
                                                      blnIsTotal)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_MATURECROPDIST_DET_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_MatureCropDist_Details.aspx")
        End Try

        If intError = objBD.EnumErrorType.NoActivePeriod Then
            lblBudgetingErr.Visible = True
            Exit Function
        Else
            AddTotalAddVoteCol(objDataSet) 
            Return objDataSet
        End If

    End Function

    Protected Sub LoadTotal()
        Dim strOppCd_MatureCropDist_Total_SUM As String
        Dim dsTotalCost As New DataSet()
        Dim intCnt As Integer
        Dim intDRCnt As Integer

        If objDataSet.Tables(0).Rows.Count > 0 And Not IsNothing(objDataSet) Then
            dr = objDataSet.Tables(0).NewRow()
            dr("MatureCropDistID") = 0
            dr("BlkCode") = ""
            dr("YearPlanted") = ""
            dr("AccCode") = "TOTAL"
            dr("AccDesc") = ""

            If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
                strOppCd_MatureCropDist_Total_SUM = "BD_CLSTRX_MATURECROPDIST_TOTAL_SUM"
                strParam = strLocation & "|" & GetActivePeriod("") & "|||" & hidBlkCode.Value & "||"
            Else
                If Request.QueryString("distbyblk") = True Then
                    strParam = strLocation & "|" & GetActivePeriod("") & "|||| AND SBLK.BlkCode = '" & lblBlkCode.Text & "'|"
                    strOppCd_MatureCropDist_Total_SUM = "BD_CLSTRX_MATURECROPDIST_TOTAL_SUM_SBLK"
                Else
                    strOppCd_MatureCropDist_Total_SUM = "BD_CLSTRX_MATURECROPDIST_TOTAL_SUM"
                    strParam = strLocation & "|" & GetActivePeriod("") & "|||" & hidBlkCode.Value & "||"
                End If
            End If

            Try
                intErrNo = objBD.mtdGetMatureCropDistFormat(strOppCd_MatureCropDist_Total_SUM, _
                                                            strOppCd_Period_GET, _
                                                            strParam, _
                                                            dsTotalCost, _
                                                            intError, _
                                                            True)
            Catch Exp As System.Exception
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_MATURECROPDIST_DET_LOADTOTAL_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_MatureCropDist_Details.aspx")
            End Try

            
            For intCnt = 0 To dsTotalCost.Tables(0).Columns.Count - 2
                intDRCnt = intCnt + 6
                dr(intDRCnt) = dsTotalCost.Tables(0).Rows(0).Item(intCnt) 
            Next

            
            dr("YearBudget") = FormatNumber(Trim(dsTotalCost.Tables(0).Rows(0).Item("TotalYrBudgetCost")), 2)
            
            dr("TotalAddVote") = decAddVoteFooterTotal
            dr("Status") = ""
            dr("LocCode") = strLocation
            dr("PeriodID") = GetActivePeriod("")
            dr("CreateDate") = "01-Jan-1900"
            dr("UpdateDate") = "01-Jan-1900"
            dr("UserName") = strUserId
            objDataSet.Tables(0).Rows.InsertAt(dr, objDataSet.Tables(0).Rows.Count + 1)
        End If
    End Sub

    Private Sub AddTotalAddVoteCol(ByRef objDS As DataSet)
        Dim i As Integer
        
        Dim objSqlConn As SqlConnection
        GetDBStringProp()
        objSqlConn = New SqlConnection(strSqlConn)

        objDS.Tables(0).Columns.Add("TotalAddVote", Type.GetType("System.Decimal"))

        For i = 0 To objDS.Tables(0).Rows.Count - 1
            With objDS.Tables(0).Rows(i)
                .Item("TotalAddVote") = RunSPReturnValue(objSqlConn, "BD_CLSTRX_MATURECROPDIST_SUM_ADDVOTE_GET", _
                                                         "@AccCode", .Item("AccCode"), _
                                                         "@PeriodID", GetActivePeriod(""), _
                                                         "@BlkCode", hidBlkCode.Value, _
                                                         "@LocCode", strLocation)

                decAddVoteFooterTotal = decAddVoteFooterTotal + .Item("TotalAddVote")
            End With
        Next
    End Sub
    
    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim label As label
        Dim strID As String
        Dim strAccCode As String

        label = E.Item.FindControl("lblMatureCropDistID")
        strID = label.Text
        label = E.Item.FindControl("lblAccCode")
        strAccCode = label.Text

        BindGrid()
        Response.Write("<Script Language=""JavaScript"">pop_Dist=window.open(""../../BD/Trx/BD_Trx_MatureCropDist_Distribute.aspx?blkcode=" & lblblkcode.Text & "&subblkcode=" & lblSubBlkCode.Text & _
                       "&id=" & strID.Trim & "&DistByBlk=" & Request.QueryString("distbyblk") & "&acccode=" & strAccCode.Trim & "&yr=" & lblYearPlanted.Text & _
                       """, null ,""'pop_Dist',width=450,height=600,top=100,left=250,status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");pop_Dist.focus();</Script>")

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
