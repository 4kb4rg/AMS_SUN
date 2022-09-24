
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


Public Class BD_TitleArea : Inherits Page

    Protected WithEvents TitleData As DataGrid
    Protected WithEvents lblTracker As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents StatusList As DropDownList
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents blnUpdate As Label
    Protected WithEvents lblDupMsg As Label
    Protected WithEvents srchAreaCode As TextBox
    Protected WithEvents srchDesc As TextBox
    Protected WithEvents srchSize As TextBox
    Protected WithEvents srchUpdateBy As TextBox
    Protected WithEvents lblTitle As Label
    Protected WithEvents lblPeriodErr As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblOper As Label
    Protected WithEvents lblTotAmtFig As Label
    Protected WithEvents lblLocTag As Label
    Protected WithEvents lblBudgeting As Label
    Protected WithEvents lblTotalArea As Label
    Protected WithEvents ibNew As ImageButton

    Protected objBD As New agri.BD.clsSetup()
    Protected objBDTrx As New agri.BD.clsTrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Dim strOppCd_GET As String = "BD_CLSSETUP_TITLEAREA_GET"
    Dim strOppCd_ADD As String = "BD_CLSSETUP_TITLEAREA_ADD"
    Dim strOppCd_UPD As String = "BD_CLSSETUP_TITLEAREA_UPD"
    Dim strOppCd_SUM As String = "BD_CLSSETUP_TITLEAREA_SUM"
    Dim strOppCd_DEL As String = "BD_CLSSETUP_TITLEAREA_DEL"

    Dim objDataSet As New DataSet()
    Dim dsCheckRec As New DataSet()
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
                SortExpression.Text = "AreaCode"
                SortCol.Text = "ASC"
            End If
            If Not Page.IsPostBack Then
                BindGrid()
            End If
        End If
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblLocTag.Text = GetCaption(objLangCap.EnumLangCap.Location) & lblCode.Text
        lblTotalArea.text = GetCaption(objLangCap.EnumLangCap.TotalArea) & " :"
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_TITLEAREA_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_TitleArea.aspx")
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


    Sub CheckRecords()
        Dim strOppCd_AreaStmt_GET As String = "BD_CLSTRX_AREASTMT_GET"
        Dim EditButton As LinkButton
        Dim intCnt As Integer

        strParam = "|" & GetActivePeriod("") & "||"
        Try
            intErrNo = objBDTrx.mtdGetAreaStatement(strOppCd_AreaStmt_GET, strParam, dsCheckRec)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_TITLEAREA_AREASTMT_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_TitleArea.aspx")
        End Try

        If dsCheckRec.Tables(0).Rows.Count > 0 Then
            ibNew.Visible = False
        End If
    End Sub


    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        TitleData.CurrentPageIndex = 0
        TitleData.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub BindGrid()
        Dim PageNo As Integer
        Dim Period As String

        TitleData.DataSource = LoadData()
        TitleData.DataBind()
        lblLocCode.Text = strLocation
        GetActivePeriod(Period)
        lblBgtPeriod.Text = Period
        CheckRecords()
    End Sub

    Protected Function LoadData() As DataSet
        Dim strTtlArea As Decimal
        Dim Period As String

        
        strParam = srchAreaCode.Text & "|" & _
                    srchDesc.Text & "|" & _
                    srchSize.Text & "||" & _
                    GetActivePeriod("") & "||" & _
                    SortExpression.Text & " " & SortCol.Text & "|" & _
                    srchUpdateBy.Text
        

       
        Try
            intErrNo = objBD.mtdGetTitleArea(strOppCd_GET, strOppCd_SUM, strParam, strTtlArea, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGETPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_TitleArea.aspx")
        End Try

        lblTotAmtFig.Text = objGlobal.GetIDDecimalSeparator(Round(strTtlArea,0))
        Return objDataSet

    End Function

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
            BGTPeriod = dsperiod.Tables(0).Rows(0).Item("BGTPeriod")
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
                TitleData.CurrentPageIndex = 0
            Case "prev"
                TitleData.CurrentPageIndex = _
                    Math.Max(0, TitleData.CurrentPageIndex - 1)
            Case "next"
                TitleData.CurrentPageIndex = _
                    Math.Min(TitleData.PageCount - 1, TitleData.CurrentPageIndex + 1)
            Case "last"
                TitleData.CurrentPageIndex = TitleData.PageCount - 1
        End Select
        lstDropList.SelectedIndex = TitleData.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            TitleData.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        TitleData.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIf(SortCol.Text = "ASC", "DESC", "ASC")
        TitleData.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
    End Sub

    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim btn As LinkButton
        Dim txt As TextBox

        lblOper.Text = objBD.EnumOperation.Update
        TitleData.EditItemIndex = CInt(E.Item.ItemIndex)
        BindGrid()

        btn = TitleData.Items.Item(CInt(E.Item.ItemIndex)).FindControl("Delete")
        btn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
        txt = TitleData.Items.Item(CInt(E.Item.ItemIndex)).FindControl("txtArea")
        txt.Enabled = False
    End Sub

    Sub DEDR_Update(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim EditText As TextBox
        Dim label As label
        Dim list As DropDownList
        Dim lblMsg As label
        Dim intError As Integer
        Dim PeriodID As String
        Dim strAreaCode As String
        Dim strDesc As String
        Dim strAreaSize As String

        label = E.Item.FindControl("lblPeriodID")
        PeriodID = label.Text
        EditText = E.Item.FindControl("txtArea")
        strAreaCode = EditText.Text
        EditText = E.Item.FindControl("txtDesc")
        strDesc = EditText.Text
        EditText = E.Item.FindControl("txtSize")
        strAreaSize = EditText.Text


        strParam = strAreaCode & "|" & _
                    strDesc & "|" & _
                    strAreaSize & "|" & _
                    GetActivePeriod("") & "|"


        Try
            intErrNo = objBD.mtdUpdTitleArea(strOppCd_ADD, _
                                                strOppCd_UPD, _
                                                strOppCd_GET, _
                                                strOppCd_SUM, _
                                                strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strAccMonth, _
                                                strAccYear, _
                                                strParam, _
                                                lblOper.Text, _
                                                intError)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=UPD_BUDGETPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_TitleArea.aspx")
        End Try

        If intError = objBD.EnumErrorType.duplicateKey Then
            lblMsg = E.Item.FindControl("lblDupMsg")
            lblMsg.Visible = True
        Else
            TitleData.EditItemIndex = -1
            BindGrid()
        End If
    End Sub

    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        If CInt(E.Item.ItemIndex) = 0 And TitleData.Items.Count = 1 And TitleData.PageCount <> 1 Then
            TitleData.CurrentPageIndex = TitleData.PageCount - 2
        End If
        TitleData.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)

        Dim strParam As String
        Dim AreaCode As String
        Dim intError As Integer
        Dim Label As Label


        Label = E.Item.FindControl("lblAreaCode")
        AreaCode = Label.Text

        strParam = GetActivePeriod("") & "|" & AreaCode

        Try
            intErrNo = objBD.mtdDelTitleArea(strOppCd_DEL, _
                                                strParam, _
                                                intError)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=DEL_TITLEAREA&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_TitleArea.aspx")
        End Try
        TitleData.EditItemIndex = -1
        BindGrid()

    End Sub

    Sub DEDR_Add(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim dataSet As dataSet = LoadData()
        Dim newRow As DataRow
        Dim Updbutton As LinkButton
        Dim lbl As Label

        blnUpdate.Text = False
        newRow = dataSet.Tables(0).NewRow()
        newRow.Item("AreaCode") = ""
        newRow.Item("Description") = ""
        newRow.Item("AreaSize") = 1
        newRow.Item("PeriodID") = 0
        newRow.Item("Status") = 1
        newRow.Item("CreateDate") = DateTime.Now()
        newRow.Item("UpdateDate") = DateTime.Now()
        newRow.Item("UserName") = ""
        dataSet.Tables(0).Rows.Add(newRow)

        TitleData.DataSource = dataSet
        TitleData.DataBind()

        TitleData.CurrentPageIndex = TitleData.PageCount - 1
        TitleData.DataBind()
        TitleData.EditItemIndex = TitleData.Items.Count - 1
        TitleData.DataBind()
        lblOper.Text = objBD.EnumOperation.Add

        Updbutton = TitleData.Items.Item(CInt(TitleData.EditItemIndex)).FindControl("Delete")
        Updbutton.Visible = False

    End Sub

End Class
