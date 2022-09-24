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


Public Class BD_trx_WSMechHourDist_Details : Inherits Page

    Protected WithEvents dgWSMechHourDist As DataGrid
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblBudgetingErr As Label
    Protected WithEvents lblPeriod As Label
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label

    Protected WithEvents lblLocTag As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBudgeting As Label
    Protected lblBgtStatus As Label
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
    Private decLastPeriodTotal As Decimal
    Private decLastPeriodAddVote As Decimal
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


            If Not Page.IsPostBack Then BindGrid()
        End If
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblLocTag.Text = GetCaption(objLangCap.EnumLangCap.Location) & lblCode.text
    End Sub

    Sub GetEntireLangCap()
        Dim strOpCode_BussTerm As String = "PWSYSTEM_CLSLANGCAP_BUSSTERM_GET"
        Dim strParam As String = strLangCode
        Dim intErrNo As Integer

        Try
            intErrNo = objLangCap.mtdGetBussTerm(strOpCode_BussTerm, strCompany, strLocation, strUserId, _
                                                 strAccMonth, strAccYear, objLangCapDs, strParam)
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
        dgWSMechHourDist.DataSource = LoadData()
        LoadTotal()
        dgWSMechHourDist.DataBind()
        lblLocCode.Text = strLocation
        GetActivePeriod(Period)
        lblBgtPeriod.Text = Period
        Formatting()

        dgWSMechHourDist.Items.Item(dgWSMechHourDist.Items.Count - 1).Font.Bold = True
    End Sub

    Sub Formatting()
        Dim EditButton As LinkButton
        Dim intCntCell As Integer = 3

        EditButton = dgWSMechHourDist.Items.Item(CInt(dgWSMechHourDist.Items.Count - 1)).FindControl("Edit")
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
            With dsperiod.Tables(0).Rows(0)
                BGTPeriod = .Item("BGTPeriod") & " - (" & objBDSetup.mtdGetPeriodStatus(.Item("Status")) & ")"
                lblBgtStatus.Text = .Item("Status")
                Return .Item("PeriodID")
            End With
        Else
            BGTPeriod = "No Active Period"
            Response.Redirect("../../BD/Setup/BD_setup_Periods.aspx")
        End If
    End Function

    Private Sub InitializeBoundColumns()
        Dim intCntPeriod As Integer
        Dim strAccPeriod As String
        Dim intYear As Integer
        Dim intCntIns As Integer
        Dim strPeriod As String
        Dim intAddVoteCol As Integer

        strParam = "|" & strLocation & "|" & GetActivePeriod("") & "||||"
        CheckAccPeriodTable("BD_CLSTRX_WSMECHHOURDIST_ACCPERIOD_GET", strParam)
        
        Try
            intErrNo = objBD.mtdGetWSMechHourDist("BD_CLSTRX_WSMECHHOURDIST_ACCPERIOD_GET", strParam, dsPeriod)

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
                .HeaderText = "Hour" 
                .HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                .ItemStyle.Wrap = False
                .ItemStyle.Width = Unit.Pixel(70)
                .ItemStyle.HorizontalAlign = HorizontalAlign.Right
                .ItemTemplate = New DataGridTemplate(ListItemType.Item, strAccPeriod)
            End With
          
            dgWSMechHourDist.Columns.AddAt(intCntIns, bcInsert)
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
           
            dgWSMechHourDist.Columns.AddAt(intAddVoteCol, tmpcolAddVote)
        Next

        If Not strPeriod = "" Then
            lblPeriod.Text = strPeriod.TrimStart("%")
            lblPeriod.Text = lblPeriod.Text & "%"
        End If
    End Sub

    Private Sub CheckAccPeriodTable(ByVal strOpCode As String, ByVal strParam As String)
        intErrNo = objBD.mtdGetWSMechHourDist(strOpCode, strParam, dsPeriod) 
        
        If dsPeriod.Tables(0).Rows.Count > 0 Then
            Exit Sub
        Else
            intErrNo = objBD.mtdAddWSMechHourDist(strLocation, GetActivePeriod(""), strUserId)
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
        strParam = strLocation & "|" & GetActivePeriod("") & "||||" & _
                   SortExpression.Text & " " & SortCol.Text

        Try
            intErrNo = objBD.mtdGetWSMechHourDistFormat("BD_CLSTRX_WSMECHHOURDIST_FORMAT_GET", _
                                                        strOppCd_Period_GET, strParam, _
                                                        objDataSet, intError, False)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_OVERHEADDIST_DET&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_PlantationOHDist_Details.aspx")
        End Try

        If intError = objBD.EnumErrorType.NoActivePeriod Then
            lblBudgetingErr.Visible = True
            Exit Function
        Else
            ModifyLastPeriodFigure(objDataSet) 
            AddTotalAddVoteCol(objDataSet) 
            Return objDataSet
        End If
    End Function

    Private Sub ModifyLastPeriodFigure(ByRef objDS As DataSet)
        Dim rowCnt As Integer
        Dim colCnt As Integer
        Dim i As Integer
        Dim strAccPeriod As String
        Dim decMechHour As Decimal
       
        With objDS.Tables(0)
            If .Rows.Count > 0 Then
                For rowCnt = 0 To .Rows.Count - 1
                    decMechHour = CDec(.Rows(rowCnt).Item("MechHour"))

                    If decMechHour > 0 Then
                        For i = 0 To dsPeriod.Tables(0).Rows.Count - 1
                            strAccPeriod = Trim(dsPeriod.Tables(0).Rows(i).Item("AccMonth")) & "/" & Trim(dsPeriod.Tables(0).Rows(i).Item("AccYear"))
                            
                            For colCnt = 0 To .Columns.Count - 1
                                If strAccPeriod = .Columns.Item(colCnt).ColumnName() Then
                                    If CDec(.Rows(rowCnt).Item(strAccPeriod)) > 0 Then
                                        If i = dsPeriod.Tables(0).Rows.Count - 1 Then 
                                            .Rows(rowCnt).Item(strAccPeriod) = RoundNumber(decMechHour, 2)
                                            decLastPeriodTotal = decLastPeriodTotal + RoundNumber(decMechHour, 2)
                                        Else
                                            decMechHour = decMechHour - RoundNumber(CDec(.Rows(rowCnt).Item(strAccPeriod)), 2)
                                        End If
                                    End If
                                    Exit For
                                End If
                            Next
                        Next
                    End If
                Next
            End If
        End With
    End Sub

    Private Function RoundNumber(ByVal d As Decimal, Optional ByVal decimals As Decimal = 0) As Decimal
        RoundNumber = Decimal.Divide(Int(Decimal.Add(Decimal.Multiply(d, 10.0 ^ decimals), 0.5)), 10.0 ^ decimals)
    End Function

    Private Sub LoadTotal()
        Dim strOppCd_WSMechHourDist_Total_SUM As String = "BD_CLSTRX_WSMECHHOURDIST_TOTAL_SUM"
        Dim dsTotalCost As New DataSet()
        Dim intCnt As Integer
        Dim intDRCnt As Integer

        dr = objDataSet.Tables(0).NewRow()
        dr("EmpCode") = "TOTAL"
        dr("WorkCode") = ""

        strParam = strLocation & "|" & GetActivePeriod("") & "||||"

        Try
            intErrNo = objBD.mtdGetWSMechHourDistFormat(strOppCd_WSMechHourDist_Total_SUM, strOppCd_Period_GET, _
                                                        strParam, dsTotalCost, intError, True)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_OVERHEADDIST_DET_LOADTOTAL_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_PlantationOHDist_Details.aspx")
        End Try

        With dsTotalCost.Tables(0)
            .Rows(0).Item(.Columns.Count - 3) = decLastPeriodTotal
        End With

        For intCnt = 0 To dsTotalCost.Tables(0).Columns.Count - 2
            intDRCnt = intCnt + 4
            dr(intDRCnt) = dsTotalCost.Tables(0).Rows(0).Item(intCnt) 
        Next

        
        dr("MechHour") = FormatNumber(Trim(dsTotalCost.Tables(0).Rows(0).Item("TotalYrBudgetCost")), 2)
        
        dr("TotalAddVote") = decAddVoteFooterTotal
        dr("LocCode") = ""
        dr("PeriodID") = 0
        dr("CreateDate") = "01-Jan-1900"
        dr("UpdateDate") = "01-Jan-1900"
        dr("UserName") = ""
        objDataSet.Tables(0).Rows.InsertAt(dr, objDataSet.Tables(0).Rows.Count + 1)
    End Sub

    Private Sub AddTotalAddVoteCol(ByRef objDS As DataSet)
        Dim i As Integer
        
        Dim objSqlConn As SqlConnection
        GetDBStringProp()
        objSqlConn = New SqlConnection(strSqlConn)

        objDS.Tables(0).Columns.Add("TotalAddVote", Type.GetType("System.Decimal"))

        For i = 0 To objDS.Tables(0).Rows.Count - 1
            With objDS.Tables(0).Rows(i)
                .Item("TotalAddVote") = RunSPReturnValue(objSqlConn, "BD_CLSTRX_WSMECHHOURDIST_SUM_ADDVOTE_GET", _
                                                         "@EmpCode", .Item("EmpCode"), _
                                                         "@WorkCode", .Item("WorkCode"), _
                                                         "@PeriodID", GetActivePeriod(""), _
                                                         "@LocCode", strLocation)

                decAddVoteFooterTotal = decAddVoteFooterTotal + .Item("TotalAddVote")
            End With
        Next
    End Sub
    
    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim lblTemp As Label
        Dim strEmpCode As String
        Dim strWorkCode As String

        lblTemp = E.Item.FindControl("lblEmpCode")
        strEmpCode = Trim(lblTemp.Text)
        lblTemp = E.Item.FindControl("lblWorkCode")
        strWorkCode = Trim(lblTemp.Text)

        BindGrid()
        Response.Write("<Script Language=""JavaScript"">pop_Dist=window.open(""../../BD/Trx/BD_Trx_WSMechHourDist_Distribute.aspx?" & _
                       "EmpCode=" & strEmpCode & "&WorkCode=" & strWorkCode & _
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
