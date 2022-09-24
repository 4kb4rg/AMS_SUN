
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


Public Class BD_VehRunDist_Det : Inherits Page

    Protected WithEvents dgVehicle As DataGrid
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblOvrMsgTop As Label
    Protected WithEvents lblOvrMsg As Label
    Protected WithEvents lblPeriod As Label
    Protected WithEvents lblBudgetingErr As Label
    Protected WithEvents lblCode As Label

    Protected WithEvents SortCol As Label
    Protected WithEvents lblLocTag As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblVeh As Label
    Protected WithEvents lblVehType As Label
    Protected WithEvents lblVRATitle As Label
    Protected WithEvents lblVehTag As Label
    Protected WithEvents lblVehCode As Label
    Protected WithEvents lblDescTag As Label
    Protected WithEvents lblDesc As Label
    Protected WithEvents lblPDate As Label
    Protected WithEvents lblHPCC As Label
    Protected WithEvents lblModel As Label
    Protected WithEvents lblUOM As Label
    Protected WithEvents lblExpenditure As Label
    Protected WithEvents lbtn_Recalc As Button
    Protected WithEvents lblBgtStatus As Label 

    Dim objBDTx As New agri.BD.clsTrx()
    Dim objBD As New agri.BD.clsSetup()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Dim strOppCd_VehRunDist_Format_GET As String = "BD_CLSTRX_VEHRUNDIST_FORMAT_GET"
    Dim strOppCd_Period_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"
    Dim strOppCd_VehRunDist_AccPeriod_GET As String = "BD_CLSTRX_VEHRUNDIST_ACCPERIOD_GET"

    Dim objDataSet As New DataSet()
    Dim objLangCapDs As New DataSet()
    Dim dsPeriod As New DataSet()

    Dim intErrNo As Integer
    Dim intError As Integer
    Dim strParam As String = ""

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim intADAR As Integer
    Dim strAccMonth As String
    Dim strAccYear As String
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
            lblOvrMsgTop.Visible = False
            lblOvrMsg.Visible = False
            lblBudgetingErr.Visible = False

            If Not Page.IsPostBack Then
                onload_GetLangCap()
                lblVeh.Text = Request.QueryString("Veh")

                BindGrid()
            End If

        End If
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblVRATitle.Text = "VEHICLE RUNNING DISTRIBUTION"
        lblLocTag.Text = GetCaption(objLangCap.EnumLangCap.Location) & lblCode.Text
        lblVehTag.Text = GetCaption(objLangCap.EnumLangCap.Vehicle) & lblCode.Text
        lblDescTag.Text = GetCaption(objLangCap.EnumLangCap.VehicleDesc)
        dgVehicle.Columns(1).HeaderText = GetCaption(objLangCap.EnumLangcap.VehExpense) 
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
            Response.Redirect("../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_VEHRUNDIST_DETAILS_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_VehRunDist_Details.aspx")
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

        InitializeBoundColumns()
        dgVehicle.DataSource = LoadData()
        dgVehicle.DataBind()
        lblLocCode.Text = strLocation
        GetActivePeriod(Period)
        lblBgtPeriod.Text = Period
        LoadVehicle()

        Formatting()


    End Sub

    Protected Function LoadData() As DataSet

        strParam = strLocation & "|" & GetActivePeriod("") & "|" & lblVeh.Text & "|||VRS.DispSeq ASC"
        Try
            intErrNo = objBDTx.mtdGetVehRunningDistFormat(strOppCd_VehRunDist_Format_GET, _
                                                          strOppCd_Period_GET, _
                                                          strParam, _
                                                          objDataSet, _
                                                          intError, _
                                                          False)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_VEHRUNDIST_DET_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_VehRunDist_Details.aspx")
        End Try

        If intError = objBDTx.EnumErrorType.NoActivePeriod Then
            lblBudgetingErr.Visible = True
            Exit Function
        Else
            Return objDataSet
        End If

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
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_VEHRUNDIST_DET_VEHICLE_GET_BY_VEHCODE&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_VehRunDist_Details.aspx")
        End Try

        lblVehCode.Text = Trim(dsVehicle.Tables(0).Rows(0).Item("VehCode"))
        lblDesc.Text = Trim(dsVehicle.Tables(0).Rows(0).Item("Description"))
        lblModel.Text = Trim(dsVehicle.Tables(0).Rows(0).Item("Model"))
        lblHPCC.Text = Trim(dsVehicle.Tables(0).Rows(0).Item("HPCC"))
        lblUOM.Text = Trim(dsVehicle.Tables(0).Rows(0).Item("UOMCode"))
        lblPDate.Text = objGlobal.GetShortDate(strDateFMT, Trim(dsVehicle.Tables(0).Rows(0).Item("PurchaseDate")))

    End Sub

    Protected Function GetActivePeriod(ByRef BGTPeriod As String) As String
        Dim dsperiod As New DataSet()

        strParam = "|||||" & objBD.EnumPeriodStatus.Active & "|" & strLocation & "|"
        Try
            intErrNo = objBD.mtdGetPeriodList(strOppCd_Period_GET, strParam, dsperiod)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_VEHRUNDIST_DET_GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_VehRunDist_Details.aspx")
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

    Sub Formatting()
        Dim EditButton As LinkButton
        Dim Label As Label
        Dim intCnt As Integer = 0

        For intCnt = 0 To dgVehicle.Items.Count - 1
            Label = dgVehicle.Items.Item(intCnt).FindControl("lblDispType")
            If Label.Text = objBD.EnumBudgetFormatItem.Header Or Label.Text = objBD.EnumBudgetFormatItem.Formula Then
                EditButton = dgVehicle.Items.Item(intCnt).FindControl("Edit")
                EditButton.Visible = False
                dgVehicle.Items.Item(intCnt).Font.Bold = True
            End If
        Next
    End Sub

    Sub DataGrid_ItemDataBound(ByVal Sender As Object, ByVal e As DataGridItemEventArgs)
        Dim intCntCell As Integer
        Dim intCntRem As Integer = 0
        Dim intCntCol As Integer = 5
        Dim intCntPeriod As Integer = 0
        Dim strColInfo As String
        Dim arrPeriod As Array
        Dim txt As TextBox
        Dim lbl As Label
        Dim btn As LinkButton

        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            lbl = e.Item.FindControl("lblIdx")
            lbl.Text = e.Item.ItemIndex.ToString + 1

            lbl = e.Item.FindControl("lblDispType")
            Select Case lbl.Text
                Case objBD.EnumBudgetFormatItem.Header
                    e.Item.Cells(0).Font.Bold = True
                    e.Item.CssClass = "mr-r"



                Case objBD.EnumBudgetFormatItem.Formula
                    e.Item.CssClass = "mr-l"
                    e.Item.Font.Bold = True
                    lbl = e.Item.FindControl("lblItem")
                    lbl.Font.Bold = True


            End Select





        ElseIf e.Item.ItemType = ListItemType.Header Then
            e.Item.HorizontalAlign = HorizontalAlign.Center
            e.Item.Font.Bold = True
        End If

        Select Case e.Item.ItemType
            Case ListItemType.Header

                arrPeriod = Split(lblPeriod.Text, "%")

                For intCntCol = 5 To e.Item.Cells.Count - 1
                    strColInfo = e.Item.Cells(intCntCol).Text
                    While intCntPeriod <= arrPeriod.GetUpperBound(0) - 1
                        e.Item.Cells(intCntCol).Text = "Period " & strColInfo '& " <BR> Cost "
                        intCntPeriod += 1
                        Exit While
                    End While
                    e.Item.Cells(intCntCol).Font.Bold = True
                Next

        End Select
    End Sub

    Private Sub InitializeBoundColumns()
        Dim intCntPeriod As Integer
        Dim strAccPeriod As String
        Dim intYear As Integer
        Dim intCntIns As Integer
        Dim strPeriod As String

        strParam = "|" & strLocation & "|" & GetActivePeriod("") & "|||"
        Try
            intErrNo = objBDTx.mtdGetVehRunningDist(strOppCd_VehRunDist_AccPeriod_GET, _
                                                    strParam, _
                                                    dsPeriod)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_VEHRUNDIST_ACCPERIOD_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_VehRunDist_Details.aspx")
        End Try

        For intCntPeriod = 0 To dsPeriod.Tables(0).Rows.Count - 1
            strAccPeriod = Trim(dsPeriod.Tables(0).Rows(intCntPeriod).Item("AccMonth")) & "/" & Trim(dsPeriod.Tables(0).Rows(intCntPeriod).Item("AccYear"))

            If intCntPeriod = 0 Then
                intCntIns = 5
            Else
                intCntIns += 1
            End If

            Dim bcInsert As TemplateColumn = New TemplateColumn()
            bcInsert.HeaderText = strAccPeriod
            bcInsert.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
            bcInsert.ItemStyle.Wrap = False
            bcInsert.ItemStyle.Width = Unit.Pixel(90)
            bcInsert.ItemStyle.HorizontalAlign = HorizontalAlign.Right
            bcInsert.ItemTemplate = New DataGridTemplate(ListItemType.Item, strAccPeriod)
            dgVehicle.Columns.AddAt(intCntIns, bcInsert)

            strPeriod = strPeriod & "%" & strAccPeriod

        Next

        If Not strPeriod = "" Then
            lblPeriod.Text = strPeriod.TrimStart("%")
            lblPeriod.Text = lblPeriod.Text & "%"
        End If

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

    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim label As label
        Dim strID As String
        Dim strYrBudgetCost As String

        label = E.Item.FindControl("lblVehRunDistID")
        strID = label.Text
        label = E.Item.FindControl("lblCost")
        strYrBudgetCost = label.Text

        BindGrid()
        Response.Write("<Script Language=""JavaScript"">pop_Dist=window.open(""../../BD/Trx/BD_Trx_VehRunDist_Distribute.aspx?id=" & strID.Trim & "&vehcode=" & lblVeh.Text.Trim & _
                       "&cost=" & strYrBudgetCost & """, null ,""'pop_Dist',width=450,height=600,top=100,left=250,status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");pop_Dist.focus();</Script>")
    End Sub

    Sub Recalc_Formula(ByVal Sender As Object, ByVal E As EventArgs) Handles lbtn_Recalc.Click
        Dim strOppCd_VehRunDist_AccPeriod_UPD As String = "BD_CLSTRX_VEHRUNDIST_ACCPERIOD_UPD"
        Dim strOppCd_VehRunDist_UPD As String = "BD_CLSTRX_VEHRUNDIST_UPD"
        Dim strOppCd_VehRunDist_GET As String = "BD_CLSTRX_VEHRUNDIST_GET"
        Dim strOppCd_VehRunDist_YrBudgetCost_GET As String = "BD_CLSTRX_VEHRUNDIST_YRBUDGETCOST_GET"
        Dim strOpCd_Formula_GET As String = "BD_CLSTRX_CALCFORMULA_GET"
        Dim strOppCd_VehRunSetup_GET As String = "BD_CLSSETUP_VEHRUNNING_FORMAT_GET"
        Dim strOppCd_VehRunDist_AccPeriod_List_GET As String = "BD_CLSTRX_VEHRUNDIST_ACCPERIOD_LIST_GET"

        strParam = "|" & strLocation & "|" & GetActivePeriod("") & "|" & lblVeh.Text.Trim & "||BD.VehRunSetID ASC"
        Try
            intErrNo = objBDTx.mtdUpdVehRunningDistFormula(strOppCd_VehRunDist_AccPeriod_UPD, _
                                                            strOppCd_VehRunDist_UPD, _
                                                            strOppCd_VehRunDist_GET, _
                                                            strOppCd_VehRunDist_YrBudgetCost_GET, _
                                                            strOppCd_VehRunSetup_GET, _
                                                            strOppCd_VehRunDist_AccPeriod_GET, _
                                                            strOppCd_VehRunDist_AccPeriod_List_GET, _
                                                            strOpCd_Formula_GET, _
                                                            strCompany, _
                                                            strLocation, _
                                                            strUserId, _
                                                            strParam, _
                                                            intError)


            If intError = objBDTx.EnumErrorType.CalculationErr Then
                lblOvrMsgTop.Visible = True
                lblOvrMsg.Visible = True
            End If

        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_VEHRUNDIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_VehRunDist_Details.aspx")
        End Try
        BindGrid()

    End Sub

End Class
