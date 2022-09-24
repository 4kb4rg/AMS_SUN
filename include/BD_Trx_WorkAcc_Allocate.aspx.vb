Imports System
Imports System.Data
Imports System.IO 
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports Microsoft.VisualBasic.Information
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic.Interaction
Imports System.Math 


Public Class BD_WorkAcc_Allocate : Inherits Page

    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblOper As Label
    Protected WithEvents lblHiddenSts As Label
    Protected WithEvents lblErrPcnt As Label

    Protected WithEvents lblWorkAccID As Label
    Protected WithEvents lblWorkAccName As Label
    Protected WithEvents lblBgtTag As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblStatus As Label
    Protected WithEvents lblDateCreated As Label
    Protected WithEvents lblLastUpdate As Label
    Protected WithEvents lblUpdatedBy As Label
    Protected WithEvents lblTotal As Label
    Protected WithEvents lblTotalPcnt As Label
    Protected WithEvents label As Label

    Protected WithEvents ibNew As ImageButton

    Protected WithEvents dgWorkAccAlloc As DataGrid

    Dim objBD As New agri.BD.clsTrx()
    Dim objBDSetup As New agri.BD.clsSetup()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()

    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Dim objDsWorkAcc As New Object()
    Dim objDsWorkAlloc As New Object()
    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intADAR As Integer

    Dim strOppCd_ADD As String = "BD_CLSTRX_WORKACCALLOC_ADD"
    Dim strOppCd_UPD As String = "BD_CLSTRX_WORKACCALLOC_UPD"
    Dim strOppCd_WA_UPD As String = "BD_CLSTRX_WORKACC_UPD"
    Dim strOppCd_DEL As String = "BD_CLSTRX_WORKACCALLOC_DEL"

    Dim strSelectedWorkAccID As String = ""
    Dim intStatus As Integer
    Dim intErrNo As Integer
    Dim strParam As String
    Dim strLocType as String

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        intADAR = Session("SS_ADAR")
        strLocType = Session("SS_LOCTYPE")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumADAccessRights.ADBudgeting), intADAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            strSelectedWorkAccID = Trim(Request.QueryString("id"))
            intStatus = CInt(lblHiddenSts.Text)
            lblErrPcnt.Visible = False

            If Not IsPostBack Then
                If strSelectedWorkAccID <> "" Then
                    onLoad_Display()
                    onLoad_LineDisplay()
                Else
                    onLoad_BindButton()
                End If
            End If
        End If
    End Sub

    Protected Function GetActivePeriod(ByRef BGTPeriod As String) As String
        Dim strOppCd_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"
        Dim dsperiod As New DataSet()

        strParam = "|||||" & objBDSetup.EnumPeriodStatus.Active & "|" & strLocation & "|"
        Try
            intErrNo = objBDSetup.mtdGetPeriodList(strOppCd_GET, strParam, dsperiod)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_WorkAcc_Details.aspx?id=" & strSelectedWorkAccID)
        End Try

        If dsperiod.Tables(0).Rows.Count > 0 Then
            BGTPeriod = dsperiod.Tables(0).Rows(0).Item("BGTPeriod")
            Return dsperiod.Tables(0).Rows(0).Item("PeriodID")
        Else
            BGTPeriod = "No Active Period"
            Response.Redirect("../../BD/Setup/BD_setup_Periods.aspx")
        End If

    End Function

    Sub onLoad_BindButton()

        Select Case intStatus
            Case objBD.EnumWorkAccStatus.Active
                ibNew.Visible = True
                dgWorkAccAlloc.Enabled = True
            Case objBD.EnumWorkAccStatus.Budgeted
                ibNew.Visible = True
                dgWorkAccAlloc.Enabled = True
            Case Else
                ibNew.Visible = False
                dgWorkAccAlloc.Enabled = True
        End Select

    End Sub

    Sub onLoad_Display()
        Dim strOppCd_GET As String = "BD_CLSTRX_WORKACC_GET"
        Dim intErrNo As Integer

        strParam = "AND WA.WorkAccID = '" & strSelectedWorkAccID & "'"
        Try
            intErrNo = objBD.mtdGetWorkAcc(strOppCd_GET, _
                                           strCompany, _
                                           strLocation, _
                                           strUserId, _
                                           strParam, _
                                           objDsWorkAcc, _
                                           True)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_WORKACC_ALLOC_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_WorkAcc_Allocate.aspx?id=" & strSelectedWorkAccID)
        End Try

        lblWorkAccID.Text = strSelectedWorkAccID
        lblWorkAccName.Text = Trim(objDsWorkAcc.Tables(0).Rows(0).Item("WorkAccName"))
        intStatus = CInt(Trim(objDsWorkAcc.Tables(0).Rows(0).Item("Status")))
        lblHiddenSts.Text = Trim(objDsWorkAcc.Tables(0).Rows(0).Item("Status"))
        lblStatus.Text = objBD.mtdGetWorkAccStatus(Trim(objDsWorkAcc.Tables(0).Rows(0).Item("Status")))
        lblBgtPeriod.Text = Trim(objDsWorkAcc.Tables(0).Rows(0).Item("BGTPeriod"))
        lblDateCreated.Text = objGlobal.GetLongDate(objDsWorkAcc.Tables(0).Rows(0).Item("CreateDate"))
        lblLastUpdate.Text = objGlobal.GetLongDate(objDsWorkAcc.Tables(0).Rows(0).Item("UpdateDate"))
        lblUpdatedBy.Text = Trim(objDsWorkAcc.Tables(0).Rows(0).Item("UserName"))
        lblTotal.Text = Request.QueryString("Total")

        onLoad_BindButton()

    End Sub

    Sub Button_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)





    End Sub

    Protected Function onLoad_LineDisplay() As DataSet
        Dim strOpCd As String = "BD_CLSTRX_WORKACCALLOC_GET"

        strParam = "AND WA.WorkAccID = '" & strSelectedWorkAccID & "'"
        Try
            intErrNo = objBD.mtdGetWorkAcc(strOpCd, _
                                           strCompany, _
                                           strLocation, _
                                           strUserId, _
                                           strParam, _
                                           objDsWorkAlloc, _
                                           True)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_WORKALLOC_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_WorkAcc_Allocate.aspx?id=" & strSelectedWorkAccID)
        End Try

        LoadWorkAccAllocTotal()

        dgWorkAccAlloc.DataSource = objDsWorkAlloc.Tables(0)
        dgWorkAccAlloc.DataBind()

        Return objDsWorkAlloc
    End Function

    Protected Sub LoadWorkAccAllocTotal()
        Dim Period As String
        Dim dsPcnt As DataSet
        Dim strOppCdWAAlloc_Pcnt_SUM As String = "BD_CLSTRX_WORKACCALLOC_SUMPCNT_GET"

        strParam = "AND WA.PeriodID = '" & GetActivePeriod("") & "' AND WA.LocCode = '" & strLocation & "' AND WAA.WorkAccID = '" & strSelectedWorkAccID & "'"
        Try
            intErrNo = objBD.mtdGetWorkAcc(strOppCdWAAlloc_Pcnt_SUM, _
                                           strCompany, _
                                           strLocation, _
                                           strUserId, _
                                           strParam, _
                                           dsPcnt, _
                                           True)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_WORKACCALLOC_SUMPCNT&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_WorkAcc_Allocate.aspx?id=" & strSelectedWorkAccID)
        End Try


        lblTotalPcnt.Text = objGlobal.GetIDDecimalSeparator(FormatNumber(dsPcnt.Tables(0).Rows(0).Item("Pcnt"), 0))

        If CDbl(lblTotalPcnt.Text) > 100 Then
            ibNew.Visible = False
            lblErrPcnt.Visible = True
            Exit Sub
        ElseIf CDbl(lblTotalPcnt.Text) = 100 Then
            ibNew.Visible = False
        Else
            ibNew.Visible = True
        End If
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
    End Sub

    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim btn As LinkButton

        lblOper.Text = objBD.EnumOperation.Update
        dgWorkAccAlloc.EditItemIndex = CInt(E.Item.ItemIndex)
        BindGrid()

        btn = dgWorkAccAlloc.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lbDelete")
        btn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"

    End Sub

    Sub BindGrid()
        dgWorkAccAlloc.DataSource = onLoad_LineDisplay()
        dgWorkAccAlloc.DataBind()
    End Sub

    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        dgWorkAccAlloc.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)

        Dim strWorkAccAllocID As String
        Dim Label As Label
        Dim intError As Integer

        lblOper.Text = objBDSetup.EnumOperation.Delete
        Label = E.Item.FindControl("lblWAAllocID")
        strWorkAccAllocID = Trim(Label.Text)

        strParam = "|" & strWorkAccAllocID & "|||||" & "WHERE WAAllocID = '" & strWorkAccAllocID & "'"
        Try
            intErrNo = objBD.mtdUpdWorkAccAlloc("", _
                                                "", _
                                                strOppCd_WA_UPD, _
                                                strOppCd_DEL, _
                                                "", _
                                                strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strParam, _
                                                lblOper.Text, _
                                                intError)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=DEL_WORKACC_ALLOC&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_WorkAcc_Allocate.aspx?id=" & strSelectedWorkAccID)
        End Try

        dgWorkAccAlloc.EditItemIndex = -1
        BindGrid()

    End Sub

    Sub DEDR_Add(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim dataSet As dataSet = onLoad_LineDisplay()
        Dim newRow As DataRow
        Dim Updbutton As LinkButton

        newRow = dataSet.Tables(0).NewRow()
        newRow.Item("WAAllocID") = ""
        newRow.Item("WAAllocDesc") = ""
        newRow.Item("WAAllocPcnt") = 0
        newRow.Item("CreateDate") = DateTime.Now()
        newRow.Item("UpdateDate") = DateTime.Now()
        newRow.Item("UserName") = ""
        dataSet.Tables(0).Rows.Add(newRow)

        dgWorkAccAlloc.DataSource = dataSet
        dgWorkAccAlloc.DataBind()

        dgWorkAccAlloc.DataBind()
        dgWorkAccAlloc.EditItemIndex = dgWorkAccAlloc.Items.Count - 1
        dgWorkAccAlloc.DataBind()

        Updbutton = dgWorkAccAlloc.Items.Item(CInt(dgWorkAccAlloc.EditItemIndex)).FindControl("lbDelete")
        Updbutton.Visible = False

    End Sub

    Sub DEDR_Update(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strOppCd_GET As String = "BD_CLSTRX_WORKACCALLOC_SUMPCNT_GET"
        Dim EditText As TextBox
        Dim label As label
        Dim strWorkAccID As String
        Dim strWorkAccAllocID As String
        Dim strDetails As String
        Dim strPcnt As String
        Dim intError As Integer

        strWorkAccID = lblWorkAccID.Text
        label = E.Item.FindControl("lblWAAllocID")
        strWorkAccAllocID = Trim(label.Text)
        EditText = E.Item.FindControl("txtAllocDesc")
        strDetails = Trim(EditText.Text)
        EditText = E.Item.FindControl("txtPcnt")
        strPcnt = Trim(EditText.Text)

        If strWorkAccAllocID = "" Then
            lblOper.Text = objBDSetup.EnumOperation.Add
        End If

        strParam = objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.WorkingAccountAlloc) & "|" & _
                   strWorkAccID & "|" & _
                   strWorkAccAllocID & "|" & _
                   strDetails & "|" & _
                   strPcnt & "|" & _
                   GetActivePeriod("")

        Try
            intErrNo = objBD.mtdUpdWorkAccAlloc(strOppCd_ADD, _
                                                strOppCd_UPD, _
                                                strOppCd_WA_UPD, _
                                                strOppCd_DEL, _
                                                strOppCd_GET, _
                                                strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strParam, _
                                                lblOper.Text, _
                                                intError)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=UPD_WORKACC_ALLOC&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_WorkAcc_Allocate.aspx?id=" & strSelectedWorkAccID)
        End Try

        If intError > 0 Then
            lblErrPcnt.Visible = True
            Exit Sub
        End If

        dgWorkAccAlloc.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub BackBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("BD_trx_WorkAcc_Details.aspx?id=" & strSelectedWorkAccID)
    End Sub

End Class
