
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


Public Class BD_AreaStatement : Inherits Page

    Protected WithEvents DGMature As DataGrid
    Protected WithEvents DGNew As DataGrid
    Protected WithEvents DGOther As DataGrid
    Protected WithEvents lblTracker As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents StatusList As DropDownList
    Protected WithEvents srchStatusList As DropDownList
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents blnUpdate As Label
    Protected WithEvents curStatus As Label
    Protected WithEvents lblDupMsg As Label
    Protected WithEvents lblTitle As Label
    Protected WithEvents lblMatTotal As Label
    Protected WithEvents lblMatprcnt As Label
    Protected WithEvents lblNewTotal As Label
    Protected WithEvents lblNewprcnt As Label
    Protected WithEvents lblOtherTotal As Label
    Protected WithEvents lblOtherprcnt As Label
    Protected WithEvents lblTtlPlanted As Label
    Protected WithEvents lblPlantedPrcnt As Label
    Protected WithEvents lblTotalArea As Label
    Protected WithEvents lblprcntTotal As Label
    Protected WithEvents lblTitleArea As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblOper As Label
    Protected WithEvents lblSelect As Label
    Protected WithEvents lblBudgeting As Label
    Protected WithEvents TotalAreaTag As Label
    Protected WithEvents BudgLocTag As Label
    Protected WithEvents LocationTag As Label
    Protected WithEvents lblBgtStatus As Label 
    Protected WithEvents ibNew As ImageButton
    Protected WithEvents DGAdjustment As DataGrid
    Protected WithEvents TAdjAmt As Label
    Protected WithEvents lblAdjustmentTotal As Label
    Protected WithEvents lblAdjustmentprcnt As Label
    Protected WithEvents lblTtlTitleTotal As Label
    Protected WithEvents lblTtlTitlePrcnt As Label

    Protected objBD As New agri.BD.clsSetup()
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim objBDTrx As New agri.BD.clsTrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()

    Dim strOppCd_GET As String = "BD_CLSTRX_AREASTMT_GET"
    Dim strOppCd_ADD As String = "BD_CLSTRX_AREASTMT_ADD"
    Dim strOppCd_UPD As String = "BD_CLSTRX_AREASTMT_UPD"
    Dim strAreaStmt_SUM As String = "BD_CLSTRX_AREASTMT_SUM"

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
            onload_GetLangCap()
            If SortExpression.Text = "" Then
                SortExpression.Text = "BGTPeriod"
                SortCol.Text = "ASC"
            End If
            If Not Page.IsPostBack Then
                BindGrid()
                If lblBgtStatus.Text.Trim = objBD.EnumPeriodStatus.Addvote Then
                    ibNew.Visible = False  
                End If
            End If
        End If
    End Sub

    Sub DataGrid_ItemDataBound(ByVal Sender As Object, ByVal e As DataGridItemEventArgs)
        Dim lbl As Label
        Dim btn As LinkButton
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            If lblBgtStatus.Text.Trim = objBD.EnumPeriodStatus.Addvote Then
                btn = e.Item.FindControl("Edit")  
                btn.Visible = False
            End If
        End If
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()

        LocationTag.Text = GetCaption(objLangCap.EnumLangCap.Location) 
        BudgLocTag.Text = GetCaption(objLangCap.EnumLangCap.Location) & lblCode.Text
        TotalAreaTag.Text = GetCaption(objLangCap.EnumLangCap.TotalArea) & " :"
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BD_TRX_AREASTATEMENT_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_AreaStatement.aspx")
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
        DGMature.CurrentPageIndex = 0
        DGMature.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub BindGrid()
        Dim Period As String

        DGMature.DataSource = LoadData(objBDTrx.EnumAreaType.MatureArea)
        DGMature.DataBind()

        DGNew.DataSource = LoadData(objBDTrx.EnumAreaType.NewArea)
        DGNew.DataBind()

        DGOther.DataSource = LoadData(objBDTrx.EnumAreaType.UnPlantedArea)
        DGOther.DataBind()

        DGAdjustment.DataSource = LoadData(objBDTrx.EnumAreaType.Adjustment)
        DGAdjustment.DataBind()

        lblLocCode.Text = strLocation
        GetActivePeriod(Period)
        lblBgtPeriod.Text = Period
        LoadTotals()
    End Sub

    Protected Function LoadData(ByVal AreaType As String) As DataSet

        strParam = AreaType & "|" & GetActivePeriod("") & "|and Loccode ='" & strLocation & "'|"
        Try
            intErrNo = objBDTrx.mtdGetAreaStatement(strOppCd_GET, strParam, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGETPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_AreaStatement.aspx")
        End Try
        Return objDataSet


    End Function

    Protected Sub LoadTotals()
        Dim dsTotal As New DataSet()
        Dim dsTitleArea As New DataSet()

        Dim strTitleArea_GET As String = "BD_CLSSETUP_TITLEAREA_GET"
        Dim strTitleArea_SUM As String = "BD_CLSSETUP_TITLEAREA_SUM"
        Dim strParam As String
        Dim strTtlArea As String
        Dim decMatureAmt As Decimal, decMaturePct As Decimal
        Dim decNewAmt As Decimal, decNewPct As Decimal
        Dim decUnplantedAmt As Decimal, decUnplantedPct As Decimal
        Dim decAdjustmentAmt As Decimal, decAdjustmentPct As Decimal

        strParam = objBDTrx.EnumAreaType.MatureArea & "|" & GetActivePeriod("") & "|and Loccode ='" & strLocation & "'|"
        Try
            intErrNo = objBDTrx.mtdGetAreaStatement(strAreaStmt_SUM, strParam, dsTotal)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGETPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_AreaStatement.aspx")
        End Try

        decMatureAmt = FormatNumber(dsTotal.Tables(0).Rows(0).Item("TotalArea"),0)
        decMaturePct = FormatNumber(dsTotal.Tables(0).Rows(0).Item("TotalPercent"),0)

        lblMatTotal.Text = ObjGlobal.GetIDDecimalSeparator(Round(dsTotal.Tables(0).Rows(0).Item("TotalArea"),0))
        lblMatprcnt.Text = ObjGlobal.GetIDDecimalSeparator(Round(dsTotal.Tables(0).Rows(0).Item("TotalPercent"),0))

        strParam = objBDTrx.EnumAreaType.NewArea & "|" & GetActivePeriod("") & "|and Loccode ='" & strLocation & "'|"
        Try
            intErrNo = objBDTrx.mtdGetAreaStatement(strAreaStmt_SUM, strParam, dsTotal)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGETPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_AreaStatement.aspx")
        End Try
        decNewAmt = FormatNumber(dsTotal.Tables(0).Rows(0).Item("TotalArea"),0)
        decNewPct = FormatNumber(dsTotal.Tables(0).Rows(0).Item("TotalPercent"),0)


        lblNewTotal.Text = objGlobal.GetIDDecimalSeparator(Round(dsTotal.Tables(0).Rows(0).Item("TotalArea"), 0))
        lblNewprcnt.Text = objGlobal.GetIDDecimalSeparator(Round(dsTotal.Tables(0).Rows(0).Item("TotalPercent"), 0))

        strParam = objBDTrx.EnumAreaType.UnPlantedArea & "|" & GetActivePeriod("") & "|and Loccode ='" & strLocation & "'|"
        Try
            intErrNo = objBDTrx.mtdGetAreaStatement(strAreaStmt_SUM, strParam, dsTotal)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGETPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_AreaStatement.aspx")
        End Try

        decUnplantedAmt = FormatNumber(dsTotal.Tables(0).Rows(0).Item("TotalArea"),0)
        decUnplantedPct = FormatNumber(dsTotal.Tables(0).Rows(0).Item("TotalPercent"),0)

        lblOtherTotal.Text = objGlobal.GetIDDecimalSeparator(Round(dsTotal.Tables(0).Rows(0).Item("TotalArea"), 0))
        lblOtherprcnt.Text = objGlobal.GetIDDecimalSeparator(Round(dsTotal.Tables(0).Rows(0).Item("TotalPercent"), 0))

        strParam = objBDTrx.EnumAreaType.Adjustment & "|" & GetActivePeriod("") & "|and Loccode ='" & strLocation & "'|"
        Try
            intErrNo = objBDTrx.mtdGetAreaStatement(strAreaStmt_SUM, strParam, dsTotal)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGETPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_AreaStatement.aspx")
        End Try


        decAdjustmentAmt = FormatNumber(dsTotal.Tables(0).Rows(0).Item("TotalArea"),0)
        decAdjustmentPct = FormatNumber(dsTotal.Tables(0).Rows(0).Item("TotalPercent"),0)

        lblAdjustmentTotal.Text = objGlobal.GetIDDecimalSeparator(Round(dsTotal.Tables(0).Rows(0).Item("TotalArea"), 0))
        lblAdjustmentprcnt.Text = objGlobal.GetIDDecimalSeparator(Round(dsTotal.Tables(0).Rows(0).Item("TotalPercent"), 0))




        lblTtlPlanted.Text = objGlobal.GetIDDecimalSeparator(Round(decMatureAmt + decNewAmt, 0))
        lblPlantedPrcnt.Text = objGlobal.GetIDDecimalSeparator(Round(decMaturePct + decNewPct, 0))
        lblTotalArea.Text = objGlobal.GetIDDecimalSeparator(Round(decMatureAmt + decNewAmt + decUnplantedAmt, 0))
        lblprcntTotal.Text = objGlobal.GetIDDecimalSeparator(Round(decMaturePct + decNewPct + decUnplantedPct, 0))
        lblTtlTitleTotal.Text = objGlobal.GetIDDecimalSeparator(Round(decMatureAmt + decNewAmt + decUnplantedAmt + decAdjustmentAmt, 0))
        lblTtlTitlePrcnt.Text = objGlobal.GetIDDecimalSeparator(Round(decMaturePct + decNewPct + decUnplantedPct + decAdjustmentPct, 0))

        strParam = "||||" & GetActivePeriod("") & "|||"
        Try
            intErrNo = objBD.mtdGetTitleArea(strTitleArea_GET, strTitleArea_SUM, strParam, strTtlArea, dsTitleArea)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGETPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_TitleArea.aspx")
        End Try


        lblTitleArea.Text = ObjGlobal.GetIDDecimalSeparator(FormatNumber(strTtlArea,0))
    End Sub

    Protected Function GetActivePeriod(ByRef BGTPeriod As String) As String
        Dim strOppCd_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"
        Dim dsperiod As New DataSet()

        strParam = "|||||" & objBD.EnumPeriodStatus.Active & "|" & strLocation & "|"

        Try
            intErrNo = objBD.mtdGetPeriodList(strOppCd_GET, strParam, dsperiod)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_TitleArea.aspx")
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

    Sub btnPrevNext_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).CommandArgument
        Select Case direction
            Case "first"
                DGMature.CurrentPageIndex = 0
            Case "prev"
                DGMature.CurrentPageIndex = _
                    Math.Max(0, DGMature.CurrentPageIndex - 1)
            Case "next"
                DGMature.CurrentPageIndex = _
                    Math.Min(DGMature.PageCount - 1, DGMature.CurrentPageIndex + 1)
            Case "last"
                DGMature.CurrentPageIndex = DGMature.PageCount - 1
        End Select
        lstDropList.SelectedIndex = DGMature.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            DGMature.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        DGMature.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
    End Sub

    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim EditText As TextBox
        Dim List As DropDownList
        Dim lbl As Label
        Dim Updbutton As LinkButton
        Dim loccode As String
        Dim btn As LinkButton

        lblOper.Text = objBD.EnumOperation.Update
        DGNew.EditItemIndex = -1
        DGOther.EditItemIndex = -1
        DGMature.EditItemIndex = -1
        DGAdjustment.EditItemIndex = -1
        Response.Write(E.Item.ClientID)
        If InStr(E.Item.ClientID, "DGMature") > 0 Then
            DGMature.EditItemIndex = CInt(E.Item.ItemIndex)
        ElseIf InStr(E.Item.ClientID, "DGNew") > 0 Then
            DGNew.EditItemIndex = CInt(E.Item.ItemIndex)
        ElseIf InStr(E.Item.ClientID, "DGOther") > 0 Then
            DGOther.EditItemIndex = CInt(E.Item.ItemIndex)
        ElseIf InStr(E.Item.ClientID, "DGAdjustment") > 0 Then
            DGAdjustment.EditItemIndex = CInt(E.Item.ItemIndex)
        End If
        BindGrid()

        If InStr(E.Item.ClientID, "DGMature") > 0 Then
            lbl = DGMature.Items.Item(CInt(DGMature.EditItemIndex)).FindControl("lblAreaCode")
            List = DGMature.Items.Item(CInt(DGMature.EditItemIndex)).FindControl("ddlloccode")
            btn = DGMature.Items.Item(CInt(DGMature.EditItemIndex)).FindControl("Delete")
            btn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
        ElseIf InStr(E.Item.ClientID, "DGNew") > 0 Then
            lbl = DGNew.Items.Item(CInt(DGNew.EditItemIndex)).FindControl("lblAreaCode")
            List = DGNew.Items.Item(CInt(DGNew.EditItemIndex)).FindControl("ddlloccode")
            btn = DGNew.Items.Item(CInt(DGNew.EditItemIndex)).FindControl("Delete")
            btn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
        ElseIf InStr(E.Item.ClientID, "DGOther") > 0 Then
            lbl = DGOther.Items.Item(CInt(DGOther.EditItemIndex)).FindControl("lblAreaCode")
            List = DGOther.Items.Item(CInt(DGOther.EditItemIndex)).FindControl("ddlloccode")
            btn = DGOther.Items.Item(CInt(DGOther.EditItemIndex)).FindControl("Delete")
            btn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
        ElseIf InStr(E.Item.ClientID, "DGAdjustment") > 0 Then
            lbl = DGAdjustment.Items.Item(CInt(DGAdjustment.EditItemIndex)).FindControl("lblAreaCode")
            List = DGAdjustment.Items.Item(CInt(DGAdjustment.EditItemIndex)).FindControl("ddlloccode")
            btn = DGAdjustment.Items.Item(CInt(DGAdjustment.EditItemIndex)).FindControl("Delete")
            btn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
        End If
        loccode = lbl.Text

        BindLocDropList(List, loccode)


    End Sub

    Sub DEDR_Update(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim EditText As TextBox
        Dim label As label
        Dim list As DropDownList
        Dim lblMsg As label
        Dim intError As Integer
        Dim AreaID As String
        Dim AreaCode As String
        Dim strDesc As String
        Dim strsize As String
        Dim strAreatype As String
        Dim strTitleArea_GET As String = "BD_CLSSETUP_TITLEAREA_GET"
        Dim strTitleArea_SUM As String = "BD_CLSSETUP_TITLEAREA_SUM"
        Dim strMaterial As String
        Dim strAge As String
        Dim strHeight As String
        Dim strTerrain As String

        label = E.Item.FindControl("lblAreaID")
        AreaID = label.Text
        list = E.Item.FindControl("ddlLoccode")
        AreaCode = list.SelectedItem.Value
        EditText = E.Item.FindControl("txtdesc")
        strDesc = EditText.Text
        EditText = E.Item.FindControl("txtSize")
        strsize = EditText.Text
        EditText = E.Item.FindControl("txtMat")
        strMaterial = EditText.Text
        EditText = E.Item.FindControl("txtAge")
        strAge = EditText.Text
        EditText = E.Item.FindControl("txtHeight")
        strHeight = EditText.Text
        EditText = E.Item.FindControl("txtTerrain")
        strTerrain = EditText.Text

        If lblOper.Text = objBD.EnumOperation.Add Then
            list = E.Item.FindControl("ddlUsage")
            strAreatype = list.SelectedItem.Value
        Else
            label = E.Item.FindControl("lblAreaType")
            strAreatype = label.Text
        End If
        
        If strAreatype.Trim = objBDTrx.EnumAreaType.Adjustment Then
            label = E.Item.FindControl("lblAdjErrMsg")
            label.Visible = False
            If Decimal.Compare(strsize.Trim, 0) = 0 Then
                label.Visible = True
                Exit Sub
            End If
            strMaterial = ""
            strAge = ""
            strHeight = ""
            strTerrain = ""
        End If

        strParam = AreaID & "|" & _
                    AreaCode & "|" & _
                    strDesc & "|" & _
                    strsize & "|" & _
                    strAreatype & "|" & _
                    GetActivePeriod("") & "||"& _
                    strMaterial & "|" & _
                    strAge & "|" & _
                    strHeight & "|" & _
                    strTerrain 
        Try
            intErrNo = objBDTrx.mtdUpdAreaStatement(strOppCd_ADD, _
                                                    strOppCd_UPD, _
                                                    strOppCd_GET, _
                                                    strTitleArea_GET, _
                                                    strTitleArea_SUM, _
                                                    strAreaStmt_SUM, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strParam, _
                                                    lblOper.Text, _
                                                    intError)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=UPD_BUDGETPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_AreaStatement.aspx")
        End Try

        If intError = objBD.EnumErrorType.InsufficientQty Then
            lblMsg = E.Item.FindControl("lblAreaERR")
            lblMsg.Visible = True
        Else

            If InStr(E.Item.ClientID, "DGMature") > 0 Then
                DGMature.EditItemIndex = -1
            ElseIf InStr(E.Item.ClientID, "DGNew") > 0 Then
                DGNew.EditItemIndex = -1
            ElseIf InStr(E.Item.ClientID, "DGOther") > 0 Then
                DGOther.EditItemIndex = -1
            ElseIf InStr(E.Item.ClientID, "DGAdjustment") > 0 Then
                DGAdjustment.EditItemIndex = -1
            End If
            BindGrid()
        End If
    End Sub

    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        If InStr(E.Item.ClientID, "DGMature") > 0 Then
            DGMature.EditItemIndex = -1
        ElseIf InStr(E.Item.ClientID, "DGNew") > 0 Then
            DGNew.EditItemIndex = -1
        ElseIf InStr(E.Item.ClientID, "DGOther") > 0 Then
            DGOther.EditItemIndex = -1
        ElseIf InStr(E.Item.ClientID, "DGAdjustment") > 0 Then
            DGAdjustment.EditItemIndex = -1
        End If
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim Label As Label
        Dim intError As Integer

        Label = E.Item.FindControl("lblAreaID")

        Dim strOpCode_DEL As String = "BD_CLSTRX_AREASTMT_DEL"

        Try
            intErrNo = objBDTrx.mtdDelAreaStmt(strOpCode_DEL, _
                                                Label.Text, _
                                                intError)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=DEL_AREA&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_AreaStatement.aspx")
        End Try

        If InStr(E.Item.ClientID, "DGMature") > 0 Then
            DGMature.EditItemIndex = -1
        ElseIf InStr(E.Item.ClientID, "DGNew") > 0 Then
            DGNew.EditItemIndex = -1
        ElseIf InStr(E.Item.ClientID, "DGOther") > 0 Then
            DGOther.EditItemIndex = -1
        ElseIf InStr(E.Item.ClientID, "DGAdjustment") > 0 Then
            DGAdjustment.EditItemIndex = -1
        End If
        BindGrid()

    End Sub

    Sub DEDR_Add(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim dataSet As dataSet = LoadData(objBDTrx.EnumAreaType.UnplantedArea)
        Dim newRow As DataRow
        Dim Updbutton As LinkButton
        Dim lbl As Label
        Dim ddl As DropDownList

        DGMature.EditItemIndex = -1
        DGNew.EditItemIndex = -1
        DGAdjustment.EditItemIndex = -1
        BindGrid()

        lblOper.Text = objBD.EnumOperation.Add
        newRow = dataSet.Tables(0).NewRow()
        newRow.Item("AreaID") = 0
        newRow.Item("AreaCode") = ""
        newRow.Item("description") = ""
        newRow.Item("AreaSize") = 1
        newRow.Item("AreaPercentage") = 0
        newRow.Item("AreaType") = 0
        newRow.Item("PeriodID") = 0
        newRow.Item("PlantMaterial") = ""
        newRow.Item("AgeGroup") = ""
        newRow.Item("PlantHeight") = ""
        newRow.Item("AreaTerrain") = ""
        newRow.Item("Status") = 1
        newRow.Item("CreateDate") = DateTime.Now()
        newRow.Item("UpdateDate") = DateTime.Now()
        newRow.Item("UserName") = ""
        dataSet.Tables(0).Rows.Add(newRow)

        DGOther.DataSource = dataSet
        DGOther.EditItemIndex = dataSet.Tables(0).Rows.Count - 1 
        DGOther.DataBind()

        ddl = DGOther.Items.Item(CInt(DGOther.EditItemIndex)).FindControl("ddlloccode")

        BindLocDropList(ddl, "")

        Updbutton = DGOther.Items.Item(CInt(DGOther.EditItemIndex)).FindControl("Delete")
        Updbutton.Visible = False

        ddl = DGOther.Items.Item(CInt(DGOther.EditItemIndex)).FindControl("ddlUsage")
        ddl.Visible = True
        BindAreaTypeList(ddl)
    End Sub

    Sub BindLocDropList(ByRef DropList As DropDownList, ByVal LocCode As String)
        Dim strOpCdLocList_GET As String = "ADMIN_CLSLOC_SYSLOC_LIST_GET"
        Dim dsForDropDown As DataSet
        Dim dsForInactiveItem As DataSet
        Dim SelectedIndex As Integer = -1
        Dim intCnt As Integer
        Dim strParam As String
        Dim strFieldCheck As String
        Dim drinsert As DataRow
        Try
            strParam = "And SY.CompCode = '" & strCompany & "' AND LO.Status = " & objAdminLoc.EnumLocStatus.Active & "|"
            intErrNo = objAdminLoc.mtdGetCompLocList(strOpCdLocList_GET, strCompany, strLocation, strUserId, dsForDropDown, strParam)

        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ITEMTODROPDOWN&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_AreaStatement.aspx")
        End Try

        For intCnt = 0 To dsForDropDown.Tables(0).Rows.Count - 1
            strFieldCheck = dsForDropDown.Tables(0).Rows(intCnt).Item(0)
            dsForDropDown.Tables(0).Rows(intCnt).Item(0) = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(0))
            dsForDropDown.Tables(0).Rows(intCnt).Item(1) = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(0)) & " ( " & _
                                                           Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(1)) & " )"
            If strFieldCheck.Trim.ToUpper = UCase(Trim(LocCode)) Then
                SelectedIndex = intCnt + 1
            End If
        Next intCnt
        drinsert = dsForDropDown.Tables(0).NewRow()
        drinsert(0) = " "
        drinsert(1) = lblSelect.Text & LocationTag.Text
        dsForDropDown.Tables(0).Rows.InsertAt(drinsert, 0)

        DropList.DataSource = dsForDropDown.Tables(0)
        DropList.DataValueField = "LocCode"
        DropList.DataTextField = "LocDesc"
        DropList.DataBind()


        DropList.SelectedIndex = SelectedIndex
        If Not dsForDropDown Is Nothing Then
            dsForDropDown = Nothing
        End If
    End Sub

    Sub BindAreaTypeList(ByRef DropList As DropDownList)

        DropList.Items.Add(New ListItem(objBDTrx.mtdGetAreaType(objBDTrx.EnumAreaType.MatureArea), objBDTrx.EnumAreaType.MatureArea))
        DropList.Items.Add(New ListItem(objBDTrx.mtdGetAreaType(objBDTrx.EnumAreaType.NewArea), objBDTrx.EnumAreaType.NewArea))
        DropList.Items.Add(New ListItem(objBDTrx.mtdGetAreaType(objBDTrx.EnumAreaType.UnPlantedArea), objBDTrx.EnumAreaType.UnPlantedArea))
        DropList.Items.Add(New ListItem(objBDTrx.mtdGetAreaType(objBDTrx.EnumAreaType.Adjustment), objBDTrx.EnumAreaType.Adjustment))
    End Sub

    Sub ddlUsage_OnSelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim rvSize As RangeValidator
        Dim revSize As RegularExpressionValidator
        Dim ddlUsage As DropDownList
        Dim txtMaterial As TextBox
        Dim txtAgeGrp As TextBox
        Dim txtHeight As TextBox
        Dim txtTerrain As TextBox


        ddlUsage = sender.FindControl("ddlUsage")
        txtMaterial = sender.FindControl("txtMat")
        txtAgeGrp = sender.FindControl("txtAge")
        txtHeight = sender.FindControl("txtHeight")
        txtTerrain = sender.FindControl("txtTerrain")
        rvSize = sender.FindControl("rvSize")
        revSize = sender.FindControl("revSize")
        Select Case ddlUsage.SelectedItem.Value.Trim
            Case objBDTrx.EnumAreaType.Adjustment
                txtMaterial.Visible = False
                txtAgeGrp.Visible = False
                txtHeight.Visible = False
                txtTerrain.Visible = False
                rvSize.MinimumValue = "-999999999999999"
                revSize.ValidationExpression = "^(\-|)\d{1,15}(\.\d{1,5}|\.|)$"
            Case Else
                txtMaterial.Visible = True
                txtAgeGrp.Visible = True
                txtHeight.Visible = True
                txtTerrain.Visible = True
                rvSize.MinimumValue = "0"
                revSize.ValidationExpression = "\d{1,15}\.\d{1,5}|\d{1,15}"
        End Select
    End Sub

End Class
