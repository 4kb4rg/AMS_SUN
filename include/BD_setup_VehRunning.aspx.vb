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


Public Class BD_VehRunning_Format : Inherits Page

    Protected WithEvents VehSetup As DataGrid
    Protected WithEvents lblTracker As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblOper As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents lblBudgeting As Label
    Protected WithEvents lblSelect As Label
    Protected WithEvents lblLocTag As Label
    Protected WithEvents lblVehExpCode As Label
    Protected WithEvents lblTitle As Label

    Protected objBD As New agri.BD.clsSetup()
    Protected objBDTrx As New agri.BD.clsTrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objGLSet As New agri.GL.clsSetup()
    Dim objLoc As New agri.Admin.clsLoc()

    Dim strOppCd_GET As String = "BD_CLSSETUP_VEHRUNNING_FORMAT_GET"
    Dim strOppCd_ADD As String = "BD_CLSSETUP_VEHRUNNING_ADD"
    Dim strOppCd_UPD As String = "BD_CLSSETUP_VEHRUNNING_UPD"

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
    Dim strLocType as String

    Private Enum EnumRefcheck
        RefFound = 1
        SeqFound = 2
        Notfound = 3
    End Enum

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
            If SortExpression.Text = "" Then
                SortExpression.Text = "BlkCode"
                SortCol.Text = "ASC"
            End If
            If Not Page.IsPostBack Then
                onload_GetLangCap()
                BindGrid()
            End If
        End If
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblLocTag.Text = GetCaption(objLangCap.EnumLangcap.Location) & lblCode.Text
        lblTitle.Text = UCase(GetCaption(objLangCap.EnumLangCap.VehUsage))
        lblVehExpCode.Text = GetCaption(objLangCap.EnumLangcap.VehExpense) & lblCode.Text
        VehSetup.Columns(2).HeaderText = GetCaption(objLangCap.EnumLangcap.VehExpense)

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
            Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_VEHUSAGE_GETLANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_VehRunning.aspx")
        End Try

    End Sub

    Function GetCaption(ByVal pv_TermCode) As String
        Dim count As Integer

        For count = 0 To objLangCapDs.Tables(0).Rows.Count - 1
            If Trim(pv_TermCode) = Trim(objLangCapDs.Tables(0).Rows(count).Item("TermCode")) Then
                If strLocType = objLoc.EnumLocType.Mill then
                    Return Trim(objLangCapDs.Tables(0).Rows(count).Item("BusinessTermMW"))
                else
                    Return Trim(objLangCapDs.Tables(0).Rows(count).Item("BusinessTerm"))
                end if
                Exit For
            End If
        Next
    End Function


    Sub DataGrid_ItemDataCreated(ByVal Sender As Object, ByVal e As DataGridItemEventArgs) Handles VehSetup.ItemDataBound

        Dim lbl As Label
        Dim txt As TextBox
        Dim arrForm As Array

        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

            lbl = e.Item.FindControl("lblIdx")
            lbl.Text = e.Item.ItemIndex.ToString + 1

            lbl = e.Item.FindControl("lblDisp")
            Select Case lbl.Text.Trim
                Case objBD.EnumBudgetFormatItem.Formula
                    lbl = e.Item.FindControl("lblForm")
                    lbl.Visible = True
                Case objBD.EnumBudgetFormatItem.Total
                    lbl = e.Item.FindControl("lblForm")
                    arrForm = lbl.Text.Split(Chr(9))
                    lbl = e.Item.FindControl("lblForm1")
                    lbl.Text = "Unit :" & arrForm(0) & "<BR>"
                    lbl = e.Item.FindControl("lblForm2")
                    lbl.Text = "Cost :" & arrForm(1)
            End Select
        ElseIf e.Item.ItemType = ListItemType.EditItem Then
            lbl = e.Item.FindControl("lblIdx")
            lbl.Text = e.Item.ItemIndex.ToString + 1

            lbl = e.Item.FindControl("lblDisp")
            Select Case lbl.Text.Trim
                Case objBD.EnumBudgetFormatItem.Formula
                    txt = e.Item.FindControl("txtFormula")
                    txt.Visible = True
                Case objBD.EnumBudgetFormatItem.Total
                    txt = e.Item.FindControl("txtFormula")
                    arrForm = txt.Text.Split(Chr(9))
                    txt = e.Item.FindControl("txtFormula1")
                    txt.Text = arrForm(0)
                    txt = e.Item.FindControl("txtFormula2")
                    txt.Text = arrForm(1)
            End Select
        End If
    End Sub

    Sub BindGrid()
        Dim PageNo As Integer
        Dim Period As String

        VehSetup.DataSource = LoadData()
        VehSetup.DataBind()
        lblLocCode.Text = strLocation
        GetActivePeriod(Period)
        lblBgtPeriod.Text = Period

    End Sub

    Sub BindItemTypeList(ByRef DropList As DropDownList, Optional ByVal itemtype As String = "")
        DropList.Items.Add(New ListItem(objBD.mtdGetFormatItem(objBD.EnumBudgetFormatItem.Entry), objBD.EnumBudgetFormatItem.Entry))
        DropList.Items.Add(New ListItem(objBD.mtdGetFormatItem(objBD.EnumBudgetFormatItem.Header), objBD.EnumBudgetFormatItem.Header))
        DropList.Items.Add(New ListItem(objBD.mtdGetFormatItem(objBD.EnumBudgetFormatItem.Formula), objBD.EnumBudgetFormatItem.Formula))
        DropList.Items.Add(New ListItem(objBD.mtdGetFormatItem(objBD.EnumBudgetFormatItem.Total), objBD.EnumBudgetFormatItem.Total))
        Select Case itemtype.Trim
            Case objBD.EnumBudgetFormatItem.Entry
                DropList.SelectedIndex = 0
            Case objBD.EnumBudgetFormatItem.Header
                DropList.SelectedIndex = 1
            Case objBD.EnumBudgetFormatItem.Formula
                DropList.SelectedIndex = 2
            Case objBD.EnumBudgetFormatItem.Total
                DropList.SelectedIndex = 3
        End Select

    End Sub

    Sub BindItemColList(ByRef DropList As DropDownList, Optional ByVal itemtype As String = "")
        DropList.Items.Add(New ListItem(objBD.mtdGetItemColumn(objBD.EnumBudgetItemColumn.Cost), objBD.EnumBudgetItemColumn.Cost))
        DropList.Items.Add(New ListItem(objBD.mtdGetItemColumn(objBD.EnumBudgetItemColumn.Unit), objBD.EnumBudgetItemColumn.Unit))
        Select Case itemtype.Trim
            Case objBD.EnumBudgetItemColumn.Cost
                DropList.SelectedIndex = 0
            Case objBD.EnumBudgetItemColumn.Unit
                DropList.SelectedIndex = 1
        End Select
    End Sub

    Sub BindVehicleExpDropList(ByRef lstVehExp As DropDownList, Optional ByVal pv_strVehExpCode As String = "")

        Dim dsForDropDown As DataSet
        Dim strOpCd As String = "GL_CLSSETUP_VEHEXPENSE_LIST_GET"
        Dim drinsert As DataRow
        Dim strParam As String = "Order By VehExpenseCode ASC|And Veh.Status = '" & objGLSet.EnumVehicleExpenseStatus.active & "'"
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer = 0

        Try
            intErrNo = objGLSet.mtdGetMasterList(strOpCd, _
                                                   strParam, _
                                                   objGLSet.EnumGLMasterType.VehicleExpense, _
                                                   dsForDropDown)
        Catch Exp As System.Exception
            Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_VEHEXPENSE_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To dsForDropDown.Tables(0).Rows.Count - 1
            dsForDropDown.Tables(0).Rows(intCnt).Item(0) = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(0))
            dsForDropDown.Tables(0).Rows(intCnt).Item(1) = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(0)) & " ( " & _
                                                           Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(1)) & " )"

            If dsForDropDown.Tables(0).Rows(intCnt).Item("VehExpenseCode") = Trim(pv_strVehExpCode) Then
                intSelectedIndex = intCnt + 1
            End If
        Next intCnt

        drinsert = dsForDropDown.Tables(0).NewRow()
        drinsert(0) = ""
        drinsert(1) = lblSelect.Text & lblVehExpCode.Text
        dsForDropDown.Tables(0).Rows.InsertAt(drinsert, 0)

        lstVehExp.DataSource = dsForDropDown.Tables(0)
        lstVehExp.DataValueField = "VehExpenseCode"
        lstVehExp.DataTextField = "Description"
        lstVehExp.DataBind()

        lstVehExp.Items.Add(New ListItem(objBD.mtdGetItemType(objBD.EnumItemType.Usage), objBD.mtdGetItemType(objBD.EnumItemType.Usage)))
        lstVehExp.Items.Add(New ListItem(objBD.mtdGetItemType(objBD.EnumItemType.Fuel), objBD.mtdGetItemType(objBD.EnumItemType.Fuel)))
        lstVehExp.Items.Add(New ListItem(objBD.mtdGetItemType(objBD.EnumItemType.Lubricant), objBD.mtdGetItemType(objBD.EnumItemType.Lubricant)))

        Select Case Trim(pv_strVehExpCode)
            Case objBD.mtdGetItemType(objBD.EnumItemType.Usage)
                intSelectedIndex = intCnt + 1
            Case objBD.mtdGetItemType(objBD.EnumItemType.Fuel)
                intSelectedIndex = intCnt + 2
            Case objBD.mtdGetItemType(objBD.EnumItemType.Lubricant)
                intSelectedIndex = intCnt + 3
        End Select

        lstVehExp.SelectedIndex = intSelectedIndex

        If Not dsForDropDown Is Nothing Then
            dsForDropDown = Nothing
        End If
    End Sub

    Protected Function LoadData() As DataSet

        strParam = "|||" & strLocation & "|" & "DispSeq Asc||" & GetActivePeriod("")
        Try
            intErrNo = objBD.mtdGetVehRunFormat(strOppCd_GET, strParam, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_VEHRUNSETUP_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_VehRunning.aspx")
        End Try
        Return objDataSet
    End Function

    Protected Function GetActivePeriod(ByRef BGTPeriod As String) As String
        Dim strOppCd_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"
        Dim dsperiod As New DataSet()

        strParam = "|||||" & objBD.EnumPeriodStatus.Active & "|" & strLocation & "|"

        Try
            intErrNo = objBD.mtdGetPeriodList(strOppCd_GET, strParam, dsperiod)
        Catch Exp As System.Exception
            Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_VEHRUNSETUP_GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_VehRunning.aspx")
        End Try
        If dsperiod.Tables(0).Rows.Count > 0 Then
            BGTPeriod = dsperiod.Tables(0).Rows(0).Item("BGTPeriod")
            Return dsperiod.Tables(0).Rows(0).Item("PeriodID")
        Else
            BGTPeriod = "No Active Period"
            Return ""
        End If
    End Function


    Sub ddlCheckType(ByVal Sender As Object, ByVal E As EventArgs)
        Dim Droplist As DropDownList
        Dim txt As TextBox

        Droplist = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("ddlDispType")
        Select Case Droplist.SelectedItem.Value.Trim
            Case objBD.EnumBudgetFormatItem.Formula
                txt = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("txtFormula")
                txt.Visible = True
                txt = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("txtFormula1")
                txt.Visible = False
                txt = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("txtFormula2")
                txt.Visible = False
                txt = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("txtFormulaRef")
                txt.Visible = True
                Droplist = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("ddlVehExp")
                Droplist.SelectedIndex = 0
                Droplist.Visible = False
                Droplist = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("ddlDispCol")
                Droplist.Visible = True
            Case objBD.EnumBudgetFormatItem.Total
                txt = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("txtFormula")
                txt.Visible = False
                txt = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("txtFormula1")
                txt.Visible = True
                txt = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("txtFormula2")
                txt.Visible = True
                txt = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("txtFormulaRef")
                txt.Visible = True
                Droplist = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("ddlVehExp")
                Droplist.SelectedIndex = 0
                Droplist.Visible = False
                Droplist = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("ddlDispCol")
                Droplist.Visible = False
            Case objBD.EnumBudgetFormatItem.Entry
                txt = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("txtFormula")
                txt.Visible = False
                txt = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("txtFormula1")
                txt.Visible = False
                txt = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("txtFormula2")
                txt.Visible = False
                txt = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("txtFormulaRef")
                txt.Visible = True
                Droplist = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("ddlVehExp")
                Droplist.Visible = True
            Case objBD.EnumBudgetFormatItem.Header
                txt = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("txtFormula")
                txt.Visible = False
                txt = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("txtFormula1")
                txt.Visible = False
                txt = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("txtFormula2")
                txt.Visible = False
                txt = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("txtFormulaRef")
                txt.Visible = False
                Droplist = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("ddlVehExp")
                Droplist.SelectedIndex = 0
                Droplist.Visible = False
                Droplist = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("ddlDispCol")
                Droplist.Visible = False
        End Select

    End Sub

    Public Function RefCheck(ByVal RefCode As String, ByVal Seq As String, ByVal TxID As String) As Integer

        Dim objRowsRef() As DataRow
        Dim objRowsSeq() As DataRow
        Dim dsCheck As DataSet = LoadData()
        Dim strTx As String

        If TxID.Trim <> "0" Then
            strTx = " and VehRunSetID <> '" & TxID.Trim & "'"
        Else
            strTx = ""
        End If

        objRowsRef = dsCheck.Tables(0).Select("FormulaRef = '" & RefCode.Trim & "' and FormulaRef <> '' " & strTx)
        objRowsSeq = dsCheck.Tables(0).Select("DispSeq = '" & Seq.Trim & "' " & strTx)

        If objRowsRef.Length <> 0 Then
            Return EnumRefcheck.RefFound
        ElseIf objRowsSeq.Length <> 0 Then
            Return EnumRefcheck.SeqFound
        Else
            Return EnumRefcheck.Notfound
        End If

    End Function

    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim validateCode As RequiredFieldValidator
        Dim Droplist As DropDownList
        Dim Label As Label
        Dim txt As TextBox
        Dim strVehExp As String
        Dim strtype As String
        Dim strCol As String
        Dim btn As LinkButton

        lblOper.Text = objBD.EnumOperation.Update
        Label = E.Item.FindControl("lblDisp")
        strtype = Label.Text
        Label = E.Item.FindControl("lblVehExpCode")
        strVehExp = Label.Text
        Label = E.Item.FindControl("lblCol")
        strCol = Label.Text
        VehSetup.EditItemIndex = CInt(E.Item.ItemIndex)
        BindGrid()

        Droplist = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("ddlDispCol")
        BindItemColList(Droplist, strCol)
        Droplist = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("ddlVehExp")
        BindVehicleExpDropList(Droplist, strVehExp)
        Droplist = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("ddlDispType")
        BindItemTypeList(Droplist, strtype)

        Droplist = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("ddlDispType")
        Select Case Droplist.SelectedItem.Value.Trim
            Case objBD.EnumBudgetFormatItem.Formula
                txt = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("txtFormula")
                txt.Visible = True
                txt = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("txtFormulaRef")
                txt.Visible = True
                Droplist = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("ddlVehExp")
                Droplist.SelectedIndex = 0
                Droplist.Visible = False
            Case objBD.EnumBudgetFormatItem.Total
                txt = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("txtFormula1")
                txt.Visible = True
                txt = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("txtFormula2")
                txt.Visible = True
                txt = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("txtFormulaRef")
                txt.Visible = True
                Droplist = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("ddlVehExp")
                Droplist.SelectedIndex = 0
                Droplist.Visible = False
                Droplist = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("ddlDispCol")
                Droplist.Visible = False
            Case objBD.EnumBudgetFormatItem.Entry
                txt = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("txtFormula")
                txt.Visible = False
                txt = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("txtFormulaRef")
                txt.Visible = True
                Droplist = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("ddlVehExp")
                Droplist.Visible = True
            Case objBD.EnumBudgetFormatItem.Header
                txt = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("txtFormula")
                txt.Visible = False
                txt = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("txtFormulaRef")
                txt.Visible = False
                Droplist = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("ddlVehExp")
                Droplist.SelectedIndex = 0
                Droplist.Visible = False
        End Select

        btn = VehSetup.Items.Item(CInt(E.Item.ItemIndex)).FindControl("Delete")
        btn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"



    End Sub

    Sub DEDR_Update(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strOpCd_VehRun_GET As String = "BD_CLSTRX_VEHRUNNING_GET"
        Dim strOpCd_VehRun_Add As String = "BD_CLSTRX_VEHRUNNING_ADD"
        Dim strOpCd_VehRun_UPD As String = "BD_CLSTRX_VEHRUNNING_UPD"
        Dim strOpCd_Format_GET As String = "BD_CLSSETUP_VEHRUNNING_FORMAT_GET"
        Dim strOpCd_Formula_GET As String = "BD_CLSTRX_CALCFORMULA_GET"

        Dim list As DropDownList
        Dim lbl As Label
        Dim txt As TextBox
        Dim intError As Integer

        Dim strTx As String
        Dim strVehExp As String
        Dim strDesc As String
        Dim strDisp As String
        Dim strForm As String
        Dim strCol As String
        Dim strRef As String
        Dim strDispCol As String
        Dim strSeq As String
        Dim intCheck As Integer

        lbl = E.Item.FindControl("lblTxID")
        strTx = lbl.Text.Trim
        list = E.Item.FindControl("ddlVehExp")
        strVehExp = list.SelectedItem.Value
        txt = E.Item.FindControl("txtItemDesc")
        strDesc = txt.Text
        list = E.Item.FindControl("ddlDispType")
        strDisp = list.SelectedItem.Value
        txt = E.Item.FindControl("txtFormula")
        strForm = txt.Text
        txt = E.Item.FindControl("txtFormulaRef")
        strRef = txt.Text
        txt = E.Item.FindControl("txtDispSeq")
        strSeq = txt.Text
        list = E.Item.FindControl("ddlDispCol")
        strCol = list.SelectedItem.Value
        lbl = E.Item.FindControl("lblCol")
        strDispCol = lbl.Text.Trim

        Select Case strDisp.Trim
            Case objBD.EnumBudgetFormatItem.Formula
                strVehExp = ""
            Case objBD.EnumBudgetFormatItem.Total
                strVehExp = ""
                txt = E.Item.FindControl("txtFormula1")
                strForm = txt.Text & Chr(9)
                txt = E.Item.FindControl("txtFormula2")
                strForm = strForm & txt.Text
                strCol = objBD.EnumBudgetItemColumn.All

            Case objBD.EnumBudgetFormatItem.Header
                strRef = ""
                strForm = ""
                strVehExp = ""
            Case objBD.EnumBudgetFormatItem.Entry
                strForm = ""
        End Select

        intCheck = RefCheck(strRef, strSeq, strTx)
        Select Case intCheck
            Case EnumRefcheck.RefFound
                lbl = E.Item.FindControl("lblRef")
                lbl.Visible = True
                lbl = E.Item.FindControl("lblSeq")
                lbl.Visible = False
                Exit Sub
            Case EnumRefcheck.SeqFound
                lbl = E.Item.FindControl("lblSeq")
                lbl.Visible = True
                lbl = E.Item.FindControl("lblRef")
                lbl.Visible = False
                Exit Sub
        End Select

        If lblOper.Text <> objBD.EnumOperation.Add Then
            If strCol <> strDispCol Then
                strParam = "|" & GetActivePeriod("") & "|" & _
                           strTx & "||||" & _
                           strDisp & "|" & _
                           strCol & "|"
                Try
                    intErrNo = objBDTrx.mtdUpdVehRunning(strOpCd_VehRun_GET, _
                                                         strOpCd_VehRun_Add, _
                                                         strOpCd_VehRun_UPD, _
                                                         strOpCd_Format_GET, _
                                                         strOpCd_Formula_GET, _
                                                         strCompany, _
                                                         strLocation, _
                                                         strUserId, _
                                                         strParam, _
                                                         objBDTrx.EnumOperation.Update, _
                                                         intError)
                Catch Exp As System.Exception
                    Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_VEHRUN_UPD_TRX&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_VehRunning.aspx")
                End Try
            End If
        End If

        strParam = strTx & "|" & _
                    strVehExp & "|" & _
                    strDesc & "|" & _
                    strDisp & "|" & _
                    strForm & "|" & _
                    strCol & "|" & _
                    strRef & "|" & _
                    GetActivePeriod("") & "|" & _
                    objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.VehicleRunningBudgeting) & "|" & _
                    strSeq

        Try
            intErrNo = objBD.mtdUpdVehRun(strOppCd_ADD, _
                                          strOppCd_UPD, _
                                          strCompany, _
                                          strLocation, _
                                          strUserId, _
                                          strParam, _
                                          lblOper.Text, _
                                          intError)
        Catch Exp As System.Exception
            Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_VEHRUN_UPD&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_VehRunning.aspx")
        End Try

        VehSetup.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        If CInt(E.Item.ItemIndex) = 0 And VehSetup.Items.Count = 1 And VehSetup.PageCount <> 1 Then
            VehSetup.CurrentPageIndex = VehSetup.PageCount - 2
        End If
        VehSetup.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strOppCd_VehRun_GET As String = "BD_CLSTRX_VEHRUNNING_GET"
        Dim strOppCd_VehRunSetup_DEL As String = "BD_CLSSETUP_VEHRUNNING_DEL"
        Dim strOppCd_VehRunDist_GET As String = "BD_CLSTRX_VEHRUNDIST_GET"
        Dim strOppCd_VehRun_DEL As String = "BD_CLSTRX_VEHRUNNING_DEL"
        Dim strOppCd_VehRunDist_DEL As String = "BD_CLSTRX_VEHRUNDIST_DEL"
        Dim strOppCd_VehRunDistAccPeriod_DEL As String = "BD_CLSTRX_VEHRUNDIST_ACCPERIOD_DEL"
        Dim strOppCd_BgtPeriod_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"

        Dim strTxID As String
        Dim strParam As String
        Dim strVehCode As String
        Dim intCnt As Integer
        Dim intCntDist As Integer
        Dim intError As Integer
        Dim label As label
        Dim dsVehRun As New DataSet()
        Dim dsVehRunDist As New DataSet()

        label = E.Item.FindControl("lblTxID")
        strTxID = label.Text.Trim

        strParam = "|" & strTxID & "|" & _
                    strLocation & "|" & _
                    GetActivePeriod("") & "||STP.DispSeq ASC"
        Try
            intErrNo = objBDTrx.mtdGetVehRunning(strOppCd_VehRun_GET, strParam, dsVehRun)
        Catch Exp As System.Exception
            Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_VEHRUN_VEHCODE_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_VehRunning.aspx")
        End Try

        For intCnt = 0 To dsVehRun.Tables(0).Rows.Count - 1
            strVehCode = Trim(dsVehRun.Tables(0).Rows(intCnt).Item("VehCode"))

            strParam = strTxID & "|" & strLocation & "|" & GetActivePeriod("") & "|" & strVehCode & "||"
            Try
                intErrNo = objBDTrx.mtdGetVehRunningDist(strOppCd_VehRunDist_GET, _
                                                         strParam, _
                                                         dsVehRunDist)
            Catch Exp As System.Exception
                Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_VEHRUNDIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_VehRunning.aspx")
            End Try

            For intCntDist = 0 To dsVehRunDist.Tables(0).Rows.Count - 1
                strParam = Trim(dsVehRunDist.Tables(0).Rows(intCntDist).Item("VehRunDistID")) & "|||"
                Try
                    intErrNo = objBD.mtdDelVehRunFormat(strOppCd_VehRunDistAccPeriod_DEL, _
                                                        strParam, _
                                                        intError)
                Catch Exp As System.Exception
                    Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_VEHRUNDISTACCPERIOD_DEL&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_VehRunning.aspx")
                End Try
            Next

        Next

        strParam = strTxID & "|" & strLocation & "|" & GetActivePeriod("") & "|"
        Try
            intErrNo = objBD.mtdDelVehRunFormat(strOppCd_VehRunDist_DEL, _
                                                strParam, _
                                                intError)
        Catch Exp As System.Exception
            Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_VEHRUNDIST_DEL&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_VehRunning.aspx")
        End Try

        strParam = strTxID & "|" & strLocation & "|" & GetActivePeriod("") & "|"
        Try
            intErrNo = objBD.mtdDelVehRunFormat(strOppCd_VehRun_DEL, _
                                                strParam, _
                                                intError)
        Catch Exp As System.Exception
            Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_VEHRUNNING_DEL&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_VehRunning.aspx")
        End Try

        strParam = strTxID & "|" & strLocation & "|" & GetActivePeriod("") & "|"
        Try
            intErrNo = objBD.mtdDelVehRunFormat(strOppCd_VehRunSetup_DEL, _
                                                strParam, _
                                                intError)
        Catch Exp As System.Exception
            Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_VEHRUNSETUP_DEL&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_VehRunning.aspx")
        End Try

        VehSetup.EditItemIndex = -1
        BindGrid()

    End Sub

    Sub DEDR_Add(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim validateCode As RequiredFieldValidator
        Dim dataSet As dataSet = LoadData()
        Dim newRow As DataRow
        Dim Updbutton As LinkButton
        Dim Droplist As DropDownList
        Dim lbl As Label
        Dim txt As TextBox

        newRow = dataSet.Tables(0).NewRow()
        newRow.Item("VehRunSetID") = 0
        newRow.Item("DispSeq") = 0
        newRow.Item("VehExpenseCode") = ""
        newRow.Item("ItemDescription") = ""
        newRow.Item("ItemDisplayType") = 1
        newRow.Item("FormulaRef") = ""
        newRow.Item("ItemCalcFormula") = ""
        newRow.Item("ItemDisplayCol") = 3
        newRow.Item("LocCode") = ""
        newRow.Item("CreateDate") = DateTime.Now()
        newRow.Item("UpdateDate") = DateTime.Now()
        newRow.Item("UserName") = ""
        dataSet.Tables(0).Rows.Add(newRow)

        VehSetup.DataSource = dataSet
        VehSetup.DataBind()

        VehSetup.CurrentPageIndex = VehSetup.PageCount - 1
        VehSetup.EditItemIndex = VehSetup.Items.Count - 1
        VehSetup.DataBind()
        lblOper.Text = objBD.EnumOperation.Add

        Droplist = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("ddlVehExp")
        BindVehicleExpDropList(Droplist)
        Droplist = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("ddlDispType")
        BindItemTypeList(Droplist)
        Droplist = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("ddlDispCol")
        BindItemColList(Droplist)
        Updbutton = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("Delete")
        Updbutton.Visible = False

        If VehSetup.Items.Count > 1 Then
            lbl = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex - 1)).FindControl("lblSeq")
            txt = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("txtDispSeq")
            txt.Text = lbl.Text + 7
        Else
            txt = VehSetup.Items.Item(CInt(VehSetup.EditItemIndex)).FindControl("txtDispSeq")
            txt.Text = 1
        End If
        txt.Enabled = False



    End Sub
End Class
