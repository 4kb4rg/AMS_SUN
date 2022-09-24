
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


Public Class BD_CapExp : Inherits Page

    Protected WithEvents CapExp As DataGrid
    Protected WithEvents lblTracker As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents StatusList As DropDownList
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents blnUpdate As Label
    Protected WithEvents curStatus As Label
    Protected WithEvents lblDupMsg As Label
    Protected WithEvents srchDetails As TextBox
    Protected WithEvents srchJust As TextBox
    Protected WithEvents srchBudget As TextBox
    Protected WithEvents srchAccount As TextBox
    Protected WithEvents srchUpdateBy As TextBox
    Protected WithEvents lblTitle As Label
    Protected WithEvents lblPeriodErr As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblOper As Label
    Protected WithEvents lblTotAmtFig As Label
    Protected WithEvents lblLocTag As Label
    Protected WithEvents lblTotalAmt As Label
    Protected WithEvents lblBgtStatus As Label 
    Protected WithEvents ibNew As ImageButton
    Dim objGLSet As New agri.GL.clsSetup()

    Dim objBDTx As New agri.BD.clsTrx()
    Dim objBD As New agri.BD.clsSetup()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Dim strOppCd_GET As String = "BD_CLSTRX_CAPEXP_GET"
    Dim strOppCd_ADD As String = "BD_CLSTRX_CAPEXP_ADD"
    Dim strOppCd_UPD As String = "BD_CLSTRX_CAPEXP_UPD"
    Dim strOppCd_DEL As String = "BD_CLSTRX_CAPEXP_DEL"

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
                SortExpression.Text = "ExpenditureID"
                SortCol.Text = "ASC"
            End If
            If Not Page.IsPostBack Then
                BindGrid()
                BindPageList()
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BD_TRX_CAPEXP_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_Capexp.aspx")
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
        CapExp.CurrentPageIndex = 0
        CapExp.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub BindGrid()
        Dim Period As String

        Dim PageNo As Integer
        Dim PageCount As Integer
        Dim dsData As DataSet

        dsData = LoadData()
        PageCount = objGlobal.mtdGetPageCount(dsData.Tables(0).Rows.Count, CapExp.PageSize)

        CapExp.DataSource = dsData
        If CapExp.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                CapExp.CurrentPageIndex = 0
            Else
                CapExp.CurrentPageIndex = PageCount - 1
            End If
        End If

        CapExp.DataBind()
        BindPageList()
        PageNo = CapExp.CurrentPageIndex + 1
        lblTracker.Text = "Page " & PageNo & " of " & CapExp.PageCount

        lblLocCode.Text = strLocation
        GetActivePeriod(Period)
        lblBgtPeriod.Text = Period
        LoadTotal()

    End Sub

    Sub BindPageList()

        Dim count As Integer = 1
        Dim arrDList As New ArrayList()

        While Not count = CapExp.PageCount + 1
            arrDList.Add("Page " & count)
            count = count + 1
        End While
        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = CapExp.CurrentPageIndex

    End Sub

    Sub BindAccCodeDropList(ByRef lstAccCode As DropDownList, Optional ByVal pv_strAccCode As String = "")

        Dim strOpCd As String = "GL_CLSSETUP_ACCOUNTCODE_LIST_GET"
        Dim dr As DataRow
        Dim strParam As String = "Order By ACC.AccCode|And ACC.Status = '" & objGLSet.EnumAccountCodeStatus.Active & "'"
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer = 0
        Dim dsForDropDown As DataSet

        Try
            intErrNo = objGLSet.mtdGetMasterList(strOpCd, _
                                                   strParam, _
                                                   objGLSet.EnumGLMasterType.AccountCode, _
                                                   dsForDropDown)
        Catch Exp As System.Exception
            Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_MATURECROP_BindAccCodeDropList&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_MatureCrop.aspx")
        End Try

        For intCnt = 0 To dsForDropDown.Tables(0).Rows.Count - 1
            dsForDropDown.Tables(0).Rows(intCnt).Item("AccCode") = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("AccCode"))
            dsForDropDown.Tables(0).Rows(intCnt).Item("Description") = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("AccCode")) & " (" & _
                                                                       Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("Description")) & ")"

            If dsForDropDown.Tables(0).Rows(intCnt).Item("AccCode") = Trim(pv_strAccCode) Then
                intSelectedIndex = intCnt + 1
            End If
        Next

        dr = dsForDropDown.Tables(0).NewRow()
        dr("AccCode") = ""
        dr("Description") = "Select Account Code"
        dsForDropDown.Tables(0).Rows.InsertAt(dr, 0)

        lstAccCode.DataSource = dsForDropDown.Tables(0)
        lstAccCode.DataValueField = "AccCode"
        lstAccCode.DataTextField = "Description"
        lstAccCode.DataBind()
        lstAccCode.SelectedIndex = intSelectedIndex

        If Not dsForDropDown Is Nothing Then
            dsForDropDown = Nothing
        End If
    End Sub


    Protected Function LoadData() As DataSet
        Dim Period As String

        strParam = srchDetails.Text & "|||||" & _
                    srchJust.Text & "|" & _
                    GetActivePeriod("") & "|" & _
                    strLocation & "||" & _
                    SortExpression.Text & " " & SortCol.Text & "|" & _
                    srchAccount.Text & "|" & srchUpdateBy.Text
        Try
            intErrNo = objBDTx.mtdGetCapExp(strOppCd_GET, strParam, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_CAPEXP_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_Capexp.aspx")
        End Try
        Return objDataSet
    End Function

    Protected Sub LoadTotal()
        Dim dsTotals As DataSet
        Dim strOppCd_SUM As String = "BD_CLSTRX_CAPEXP_SUM_GET"

        strParam = "||||||" & GetActivePeriod("") & "|" & strLocation & "||||"
        Try
            intErrNo = objBDTx.mtdGetCapExp(strOppCd_SUM, strParam, dsTotals)
        Catch Exp As System.Exception
            Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_CAPEXP_TOTAL_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_Capexp.aspx")
        End Try


        lblTotalAmt.Text = objGlobal.GetIDDecimalSeparator(FormatNumber(Round(dsTotals.Tables(0).Rows(0).Item("TotalAmount")), 0))
    End Sub

    Protected Function GetActivePeriod(ByRef BGTPeriod As String) As String
        Dim strOppCd_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"
        Dim dsperiod As New DataSet()

        strParam = "|||||" & objBD.EnumPeriodStatus.Active & "|" & strLocation & "|"
        Try
            intErrNo = objBD.mtdGetPeriodList(strOppCd_GET, strParam, dsperiod)
        Catch Exp As System.Exception
            Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_CAPEXP_GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_Capexp.aspx")
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
                CapExp.CurrentPageIndex = 0
            Case "prev"
                CapExp.CurrentPageIndex = _
                    Math.Max(0, CapExp.CurrentPageIndex - 1)
            Case "next"
                CapExp.CurrentPageIndex = _
                    Math.Min(CapExp.PageCount - 1, CapExp.CurrentPageIndex + 1)
            Case "last"
                CapExp.CurrentPageIndex = CapExp.PageCount - 1
        End Select
        lstDropList.SelectedIndex = CapExp.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            CapExp.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        CapExp.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIf(SortCol.Text = "ASC", "DESC", "ASC")
        CapExp.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
    End Sub

    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim btn As LinkButton
        Dim Droplist As DropDownList
        Dim Label As Label
        Dim strAcc As String


        lblOper.Text = objBD.EnumOperation.Update
        Label = E.Item.FindControl("lblAccCode")
        strAcc = Label.Text

        CapExp.EditItemIndex = CInt(E.Item.ItemIndex)
        BindGrid()

        Droplist = CapExp.Items.Item(CInt(E.Item.ItemIndex)).FindControl("ddlAccCode")
        BindAccCodeDropList(Droplist, strAcc)

        btn = CapExp.Items.Item(CInt(E.Item.ItemIndex)).FindControl("Delete")
        btn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"

    End Sub

    Sub DEDR_Update(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim EditText As TextBox
        Dim label As label
        Dim list As DropDownList
        Dim lblMsg As label
        Dim intError As Integer
        Dim strExpID As String
        Dim strExpdetails As String
        Dim strExisting As String
        Dim strBudgeted As String
        Dim strUnitCost As String
        Dim strJust As String
        Dim strAcc As String

        label = E.Item.FindControl("lblExpID")
        strExpID = label.Text
        EditText = E.Item.FindControl("txtDet")
        strExpdetails = EditText.Text
        EditText = E.Item.FindControl("txtExist")
        strExisting = EditText.Text
        EditText = E.Item.FindControl("txtBgt")
        strBudgeted = EditText.Text
        EditText = E.Item.FindControl("txtCost")
        strUnitCost = EditText.Text
        EditText = E.Item.FindControl("txtJust")
        strJust = EditText.Text
        list = E.Item.FindControl("ddlAccCode")
        strAcc = list.SelectedItem.Value


        strParam = strExpID & "|" & _
                    strExpdetails & "|" & _
                    strExisting & "|" & _
                    strBudgeted & "|" & _
                    strUnitCost & "|" & _
                    strJust & "|" & _
                    strLocation & "|" & _
                    GetActivePeriod("") & "||" & _
                    strAcc

        Try
            intErrNo = objBDTx.mtdUpdCapExp(strOppCd_ADD, _
                                                strOppCd_UPD, _
                                                strOppCd_GET, _
                                                strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strParam, _
                                                lblOper.Text, _
                                                intError)
        Catch Exp As System.Exception
            Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_CAPEXP_UPD&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_Capexp.aspx")
        End Try

        If intError = objBD.EnumErrorType.duplicateKey Then
            lblMsg = E.Item.FindControl("lblDupMsg")
            lblMsg.Visible = True
        Else
            CapExp.EditItemIndex = -1
            BindGrid()
        End If
    End Sub

    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        If CInt(E.Item.ItemIndex) = 0 And CapExp.Items.Count = 1 And CapExp.PageCount <> 1 Then
            CapExp.CurrentPageIndex = CapExp.PageCount - 2
        End If
        CapExp.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)

        Dim strParam As String
        Dim AreaCode As String
        Dim intError As Integer
        Dim Label As Label


        Label = E.Item.FindControl("lblExpID")
        AreaCode = Label.Text

        Try
            intErrNo = objBDTx.mtdDelCapExp(strOppCd_DEL, _
                                                AreaCode, _
                                                intError)
        Catch Exp As System.Exception
            Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_CAPEXP_DEL&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_Capexp.aspx")
        End Try
        CapExp.EditItemIndex = -1
        BindGrid()

    End Sub

    Sub DEDR_Add(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim dataSet As dataSet = LoadData()
        Dim newRow As DataRow
        Dim Updbutton As LinkButton
        Dim lbl As Label
        Dim Droplist As DropDownList

        newRow = dataSet.Tables(0).NewRow()
        newRow.Item("ExpenditureID") = 0
        newRow.Item("ExpDetails") = ""
        newRow.Item("Existing") = 0
        newRow.Item("Budgeted") = 0
        newRow.Item("UnitCost") = 0
        newRow.Item("TotalCost") = 0
        newRow.Item("Justification") = ""
        newRow.Item("LocCode") = strLocation
        newRow.Item("PeriodID") = 0
        newRow.Item("Status") = ""
        newRow.Item("CreateDate") = DateTime.Now()
        newRow.Item("UpdateDate") = DateTime.Now()
        newRow.Item("UserName") = ""
        dataSet.Tables(0).Rows.Add(newRow)

        CapExp.DataSource = dataSet
        CapExp.DataBind()

        CapExp.CurrentPageIndex = CapExp.PageCount - 1
        CapExp.DataBind()
        CapExp.EditItemIndex = CapExp.Items.Count - 1
        CapExp.DataBind()
        lblOper.Text = objBD.EnumOperation.Add

        Droplist = CapExp.Items.Item(CInt(CapExp.EditItemIndex)).FindControl("ddlAccCode")
        BindAccCodeDropList(Droplist)

        Updbutton = CapExp.Items.Item(CInt(CapExp.EditItemIndex)).FindControl("Delete")
        Updbutton.Visible = False

    End Sub
End Class
