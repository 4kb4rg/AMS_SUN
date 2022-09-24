
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


Public Class BD_WorkAcc_Details : Inherits Page

    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblErrMessage2 As Label
    Protected WithEvents lblHiddenSts As Label
    Protected WithEvents lblOper As Label
    Protected WithEvents HidWorkAccID As HtmlInputHidden

    Protected WithEvents lblWorkAccID As Label
    Protected WithEvents lblBgtTag As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents txtWorkAccName As TextBox
    Protected WithEvents lblStatus As Label
    Protected WithEvents lblDateCreated As Label
    Protected WithEvents lblLastUpdate As Label
    Protected WithEvents lblUpdatedBy As Label
    Protected WithEvents lblTotLabourCost As Label
    Protected WithEvents lblTotMatCost As Label
    Protected WithEvents lblTotVRACost As Label
    Protected WithEvents lblTotOHCost As Label
    Protected WithEvents lblTotAVCost As Label
    Protected WithEvents lblTot As Label

    Protected WithEvents SaveBtn As ImageButton
    Protected WithEvents DelBtn As ImageButton
    Protected WithEvents ibNew As ImageButton
    Protected WithEvents ibAlloc As ImageButton

    Protected WithEvents dgWorkAccLn As DataGrid
    Protected WithEvents lblBgtStatus As Label 

    Dim objBD As New agri.BD.clsTrx()
    Dim objBDSetup As New agri.BD.clsSetup()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl() 
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Dim objDsWorkAcc As New DataSet()
    Dim objDsWorkAccLn As New DataSet()
    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intADAR As Integer

    Dim strOppCd_ADD As String = "BD_CLSTRX_WORKACCLN_ADD"
    Dim strOppCd_UPD As String = "BD_CLSTRX_WORKACCLN_UPD"
    Dim strOppCd_WA_UPD As String = "BD_CLSTRX_WORKACC_UPD"
    Dim strOppCd_WALN_DEL As String = "BD_CLSTRX_WORKACCLN_DEL"

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
            strSelectedWorkAccID = Trim(IIf(Request.QueryString("id") <> "", Request.QueryString("id"), Request.Form("id")))
            intStatus = CInt(lblHiddenSts.Text)

            If Not IsPostBack Then
                GetActivePeriod("")
                If strSelectedWorkAccID <> "" Then
                    HidWorkAccID.Value = strSelectedWorkAccID
                    onLoad_Display()
                    onLoad_LineDisplay()
                Else
                    onLoad_BindButton()
                End If

            End If
        End If
    End Sub

    Sub DataGrid_ItemDataBound(ByVal Sender As Object, ByVal e As DataGridItemEventArgs)
        Dim lbl As Label
        Dim btn As LinkButton
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            If lblBgtStatus.Text.Trim = objBDSetup.EnumPeriodStatus.Addvote Then
                btn = e.Item.FindControl("lbEdit")
                btn.Visible = False
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
            BGTPeriod = dsperiod.Tables(0).Rows(0).Item("BGTPeriod") & " - (" & objBDSetup.mtdGetPeriodStatus(dsperiod.Tables(0).Rows(0).Item("Status")) & ")"
            lblBgtStatus.Text = dsperiod.Tables(0).Rows(0).Item("Status")
            Return dsperiod.Tables(0).Rows(0).Item("PeriodID")
        Else
            BGTPeriod = "No Active Period"
            Response.Redirect("../../BD/Setup/BD_setup_Periods.aspx")
        End If

    End Function

    Sub onLoad_BindButton()
        txtWorkAccName.Enabled = False
        SaveBtn.Visible = False
        DelBtn.Visible = False
        ibAlloc.Visible = False

        Select Case intStatus
            Case objBD.EnumWorkAccStatus.Active
                txtWorkAccName.Enabled = True
                SaveBtn.Visible = True
                DelBtn.Visible = True
                DelBtn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                ibNew.Visible = True
                If dgWorkAccLn.Items.Count > 0 Then
                    ibAlloc.Visible = True
                End If
                dgWorkAccLn.Enabled = True
            Case objBD.EnumWorkAccStatus.Budgeted
                If lblBgtStatus.Text.Trim = objBDSetup.EnumPeriodStatus.Addvote Then  
                    txtWorkAccName.Enabled = False
                    SaveBtn.Visible = False
                    DelBtn.Visible = False
                    ibNew.Visible = False
                    ibAlloc.Visible = False
                    dgWorkAccLn.Enabled = True

                Else
                    txtWorkAccName.Enabled = True
                    SaveBtn.Visible = True
                    DelBtn.Visible = True
                    DelBtn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                    ibNew.Visible = True
                    ibAlloc.Visible = True
                    dgWorkAccLn.Enabled = True

                End If
            Case Else
                txtWorkAccName.Enabled = True
                SaveBtn.Visible = True
                ibNew.Visible = False
                ibAlloc.Visible = False
                dgWorkAccLn.Enabled = True
        End Select

    End Sub

    Sub onLoad_Display()
        Dim strOppCd_GET As String = "BD_CLSTRX_WORKACC_GET"
        Dim intErrNo As Integer

        lblWorkAccID.Text = HidWorkAccID.Value
        strParam = "AND WA.WorkAccID = '" & lblWorkAccID.Text & "'"
        Try
            intErrNo = objBD.mtdGetWorkAcc(strOppCd_GET, _
                                           strCompany, _
                                           strLocation, _
                                           strUserId, _
                                           strParam, _
                                           objDsWorkAcc, _
                                           True)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_WORKACC_DET_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_WorkAcc_Details.aspx?id=" & strSelectedWorkAccID)
        End Try

        If objDsWorkAcc.Tables(0).Rows.Count > 0 Then
            txtWorkAccName.Text = Trim(objDsWorkAcc.Tables(0).Rows(0).Item("WorkAccName"))
            intStatus = CInt(Trim(objDsWorkAcc.Tables(0).Rows(0).Item("Status")))
            lblHiddenSts.Text = Trim(objDsWorkAcc.Tables(0).Rows(0).Item("Status"))
            lblStatus.Text = objBD.mtdGetWorkAccStatus(Trim(objDsWorkAcc.Tables(0).Rows(0).Item("Status")))
            lblBgtPeriod.Text = Trim(objDsWorkAcc.Tables(0).Rows(0).Item("BGTPeriod"))
            lblDateCreated.Text = objGlobal.GetLongDate(objDsWorkAcc.Tables(0).Rows(0).Item("CreateDate"))
            lblLastUpdate.Text = objGlobal.GetLongDate(objDsWorkAcc.Tables(0).Rows(0).Item("UpdateDate"))
            lblUpdatedBy.Text = Trim(objDsWorkAcc.Tables(0).Rows(0).Item("UserName"))
        End If

        onLoad_BindButton()
    End Sub

    Sub Button_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strCmdArgs As String = CType(Sender, ImageButton).CommandArgument
        Dim strOpCd_Add As String = "BD_CLSTRX_WORKACC_ADD"
        Dim strOpCd_Get As String = "BD_CLSTRX_WORKACC_GET"
        Dim strOpCd_WA_DEL As String = "BD_CLSTRX_WORKACC_DEL"
        Dim strOpCd_WAA_DEL As String = "BD_CLSTRX_WORKACCALLOC_DEL"
        Dim objWorkAccID As String
        Dim intError As Integer

        If strCmdArgs = "Save" Then
            strParam = objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.WorkingAccount) & "|" & _
                       strSelectedWorkAccID & "|" & _
                       txtWorkAccName.Text & "|" & _
                       objBD.EnumWorkAccStatus.Active & "|" & _
                       GetActivePeriod("") & "|"
            Try
                intErrNo = objBD.mtdUpdWorkAcc(strOpCd_Add, _
                                               strOppCd_WA_UPD, _
                                               strCompany, _
                                               strLocation, _
                                               strUserId, _
                                               strParam, _
                                               False, _
                                               objWorkAccID)
            Catch Exp As System.Exception
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_WORKACC_DET_SAVE&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_WorkAcc_Details.aspx?id=" & strSelectedWorkAccID)
            End Try

            strSelectedWorkAccID = objWorkAccID
            HidWorkAccID.Value = strSelectedWorkAccID

            onLoad_Display()
            onLoad_LineDisplay()
        ElseIf strCmdArgs = "Del" Then
            strParam = "WHERE WorkAccID = '" & strSelectedWorkAccID & "'"
            Try
                intErrNo = objBD.mtdDelWorkAcc(strOpCd_WA_DEL, _
                                               strOpCd_WAA_DEL, _
                                               strOppCd_WALN_DEL, _
                                               strParam, _
                                               intError)
            Catch Exp As System.Exception
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_WORKACC_DET_DEL&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_WorkAcc_Details.aspx?id=" & strSelectedWorkAccID)
            End Try

            Response.Redirect("BD_trx_WorkAcc_List.aspx")

        End If

        If strSelectedWorkAccID <> "" Then
        End If
    End Sub

    Protected Sub LoadWorkAccLnTotal()
        Dim Period As String
        Dim dsTotal As DataSet
        Dim strOppCdWorkAccLn_SUM As String = "BD_CLSTRX_WORKACCLN_SUM_GET"

        strParam = "AND WA.PeriodID = '" & GetActivePeriod("") & "' AND WA.LocCode = '" & strLocation & "' AND WALN.WorkAccID = '" & HidWorkAccID.Value & "'"
        Try
            intErrNo = objBD.mtdGetWorkAcc(strOppCdWorkAccLn_SUM, _
                                           strCompany, _
                                           strLocation, _
                                           strUserId, _
                                           strParam, _
                                           dsTotal, _
                                           True)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_WORKACCLN_TOTAL&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_WorkAcc_Details.aspx?id=" & strSelectedWorkAccID)
        End Try


        lblTotLabourCost.Text = objGlobal.GetIDDecimalSeparator(FormatNumber(dsTotal.Tables(0).Rows(0).Item("LabourTot"), 0))
        lblTotMatCost.Text = objGlobal.GetIDDecimalSeparator(FormatNumber(dsTotal.Tables(0).Rows(0).Item("MatTot"), 0))
        lblTotVRACost.Text = objGlobal.GetIDDecimalSeparator(FormatNumber(dsTotal.Tables(0).Rows(0).Item("VRATot"), 0))
        lblTotOHCost.Text = objGlobal.GetIDDecimalSeparator(FormatNumber(dsTotal.Tables(0).Rows(0).Item("OHTot"), 0))
        lblTotAVCost.Text = objGlobal.GetIDDecimalSeparator(FormatNumber(dsTotal.Tables(0).Rows(0).Item("AVTot"), 0))
        lblTot.Text = objGlobal.GetIDDecimalSeparator(FormatNumber(dsTotal.Tables(0).Rows(0).Item("Total"), 0))
    End Sub

    Protected Function onLoad_LineDisplay() As DataSet
        Dim strOpCd As String = "BD_CLSTRX_WORKACCLN_GET"
        Dim intCnt As Integer
        Dim label As label
        Dim EditText As TextBox

        strParam = "AND WA.WorkAccID = '" & HidWorkAccID.Value & "'" ' AND WA.Status = '" & objBD.EnumWorkAccLnStatus.Active & "'"
        Try
            intErrNo = objBD.mtdGetWorkAcc(strOpCd, _
                                           strCompany, _
                                           strLocation, _
                                           strUserId, _
                                           strParam, _
                                           objDsWorkAccLn, _
                                           True)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_WORKACCLN_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_WorkAcc_Details.aspx?id=" & strSelectedWorkAccID)
        End Try

        If objDsWorkAccLn.Tables(0).Rows.Count > 0 Then
            LoadWorkAccLnTotal()
        End If

        dgWorkAccLn.DataSource = objDsWorkAccLn.Tables(0)
        dgWorkAccLn.DataBind()

        onLoad_BindButton()
        Return objDsWorkAccLn
    End Function

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
    End Sub

    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim DelButton As LinkButton

        lblOper.Text = objBD.EnumOperation.Update
        dgWorkAccLn.EditItemIndex = CInt(E.Item.ItemIndex)
        BindGrid()

        DelButton = dgWorkAccLn.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lbDelete")
        DelButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"

    End Sub

    Sub BindGrid()

        dgWorkAccLn.DataSource = onLoad_LineDisplay()
        dgWorkAccLn.DataBind()

    End Sub

    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        dgWorkAccLn.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)

        Dim strWorkAccLnID As String
        Dim Label As Label
        Dim intError As Integer

        lblOper.Text = objBDSetup.EnumOperation.Delete
        Label = E.Item.FindControl("lblWorkAccLnID")
        strWorkAccLnID = Trim(Label.Text)

        strParam = "|" & strSelectedWorkAccID & "|||||||||" & "WHERE WorkAccLnID = '" & strWorkAccLnID & "'"
        Try
            intErrNo = objBD.mtdUpdWorkAccLn("", _
                                            "", _
                                            strOppCd_WA_UPD, _
                                            strOppCd_WALN_DEL, _
                                            strCompany, _
                                            strLocation, _
                                            strUserId, _
                                            strParam, _
                                            lblOper.Text, _
                                            intError)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=DEL_WORKACCLN&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_WorkAcc_Details.aspx?id=" & strSelectedWorkAccID)
        End Try

        dgWorkAccLn.EditItemIndex = -1
        onLoad_Display()
        BindGrid()

    End Sub

    Sub DEDR_Add(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim dataSet As dataSet = onLoad_LineDisplay()
        Dim newRow As DataRow
        Dim Updbutton As LinkButton

        newRow = dataSet.Tables(0).NewRow()
        newRow.Item("WorkAccLnID") = ""
        newRow.Item("Details") = ""
        newRow.Item("LabourCost") = 0
        newRow.Item("MaterialCost") = 0
        newRow.Item("VRACost") = 0
        newRow.Item("OverHeadCost") = 0
        newRow.Item("AddVote") = 0
        newRow.Item("Total") = 0
        newRow.Item("Status") = ""
        newRow.Item("CreateDate") = DateTime.Now()
        newRow.Item("UpdateDate") = DateTime.Now()
        newRow.Item("UserName") = ""
        dataSet.Tables(0).Rows.Add(newRow)

        dgWorkAccLn.DataSource = dataSet
        dgWorkAccLn.DataBind()

        dgWorkAccLn.DataBind()
        dgWorkAccLn.EditItemIndex = dgWorkAccLn.Items.Count - 1
        dgWorkAccLn.DataBind()

        Updbutton = dgWorkAccLn.Items.Item(CInt(dgWorkAccLn.EditItemIndex)).FindControl("lbDelete")
        Updbutton.Visible = False

    End Sub

    Sub DEDR_Update(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strOppCd_GET As String '= "BD_CLSTRX_WORKACC_GET"
        Dim EditText As TextBox
        Dim label As label
        Dim list As DropDownList
        Dim strWorkAccID As String
        Dim strWorkAccLnID As String
        Dim strDetails As String
        Dim strLabourCost As String
        Dim strMatCost As String
        Dim strVRACost As String
        Dim strOHCost As String
        Dim strAddVote As String
        Dim decTotal As Decimal
        Dim intError As Integer
        lblerrmessage2.visible=false
        strWorkAccID = lblWorkAccID.Text
        label = E.Item.FindControl("lblWorkAccLnID")
        strWorkAccLnID = Trim(label.Text)
        EditText = E.Item.FindControl("txtDet")
        strDetails = Trim(EditText.Text)
        EditText = E.Item.FindControl("txtLabourCost")
        strLabourCost = Trim(EditText.Text)
        EditText = E.Item.FindControl("txtMatCost")
        strMatCost = Trim(EditText.Text)
        EditText = E.Item.FindControl("txtVRACost")
        strVRACost = Trim(EditText.Text)
        EditText = E.Item.FindControl("txtOHCost")
        strOHCost = Trim(EditText.Text)
        strAddVote = 0
        decTotal = CDbl(strLabourCost) + CDbl(strMatCost) + CDbl(strVRACost) + CDbl(strOHCost) + CDbl(strAddVote)

        If strWorkAccLnID = "" Then
            lblOper.Text = objBDSetup.EnumOperation.Add
        End If

        strParam = objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.WorkingAccountLn) & "|" & _
                   strWorkAccID & "|" & _
                   strWorkAccLnID & "|" & _
                   strDetails & "|" & _
                   strLabourCost & "|" & _
                   strMatCost & "|" & _
                   strVRACost & "|" & _
                   strOHCost & "|" & _
                   strAddVote & "|" & _
                   CDbl(decTotal) & "||"

        Try
            intErrNo = objBD.mtdUpdWorkAccLn(strOppCd_ADD, _
                                            strOppCd_UPD, _
                                            strOppCd_WA_UPD, _
                                            "", _
                                            strCompany, _
                                            strLocation, _
                                            strUserId, _
                                            strParam, _
                                            lblOper.Text, _
                                            intError)
            if interrno <>0 then
                lblerrmessage2.visible=true
            end if
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=UPD_WORKACC_LN&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_WorkAcc_Details.aspx?id=" & strSelectedWorkAccID)
        End Try

        dgWorkAccLn.EditItemIndex = -1
        onLoad_Display()
        onLoad_LineDisplay()
        BindGrid()
    End Sub

    Sub BackBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("BD_trx_WorkAcc_List.aspx")
    End Sub

    Sub BtnAllocate_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim objWorkAccID As String

        strParam = HidWorkAccID.Value & "|" & objBD.EnumWorkAccStatus.Budgeted
        Try
            intErrNo = objBD.mtdUpdWorkAcc("", _
                                           strOppCd_WA_UPD, _
                                           strCompany, _
                                           strLocation, _
                                           strUserId, _
                                           strParam, _
                                           True, _
                                           objWorkAccID)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_WORKACC_DET_ALLOC&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_WorkAcc_Details.aspx?id=" & HidWorkAccID.Value)
        End Try

        Response.Redirect("BD_trx_WorkAcc_Allocate.aspx?id=" & lblWorkAccID.Text & "&Total=" & lblTot.Text)
    End Sub

End Class
