
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

Public Class BD_NurseryActivity_Format : Inherits Page

    Protected WithEvents NASetup As DataGrid
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblOper As Label
    Protected WithEvents lblLocTag As Label
    Protected WithEvents lblBudgeting As Label
    Protected WithEvents lblCode As Label

    Protected objBD As New agri.BD.clsSetup()
    Protected objBDTrx As New agri.BD.clsTrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objGLSet As New agri.GL.clsSetup()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objLoc As New agri.Admin.clsLoc()

    Dim strOppCd_GET As String = "BD_CLSSETUP_NURSERYACTIVITY_FORMAT_GET"
    Dim strOppCd_ADD As String = "BD_CLSSETUP_NURSERYACTIVITY_ADD"
    Dim strOppCd_UPD As String = "BD_CLSSETUP_NURSERYACTIVITY_UPD"

    Dim objDataSet As New DataSet()
    Dim objLangCapDs As New DataSet()
    Dim intErrNo As Integer
    Dim strParam As String = ""

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim intADAR As Integer
    Dim intConfigsetting As Integer
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
        intConfigsetting = Session("SS_CONFIGSETTING")
        strAccMonth = Session("SS_GLACCMONTH")
        strAccYear = Session("SS_GLACCYEAR")
        strLocType = Session("SS_LOCTYPE")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumADAccessRights.ADBudgeting), intADAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")

        Else
            If Not Page.IsPostBack Then
                onload_GetLangCap()
                BindGrid()
            End If
        End If
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblLocTag.Text = GetCaption(objLangCap.EnumLangcap.Location) & lblCode.Text
        NASetup.Columns(2).HeaderText = GetCaption(objLangCap.EnumLangcap.Account) & lblCode.Text
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
            Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_NURSERYACTIVITY_GETLANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_NurseryActivity.aspx")
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


    Sub DataGrid_ItemDataCreated(ByVal Sender As Object, ByVal e As DataGridItemEventArgs) Handles NASetup.ItemDataBound
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
                    lbl = e.Item.FindControl("lblColName")
                    lbl.Visible = True
                Case objBD.EnumBudgetFormatItem.Header
                    lbl = e.Item.FindControl("lblForm")
                    lbl.Visible = True
                    lbl = e.Item.FindControl("lblColName")
                    lbl.Visible = False
                Case objBD.EnumBudgetFormatItem.Total
                    lbl = e.Item.FindControl("lblForm")
                    arrForm = lbl.Text.Split(Chr(9))
                    lbl = e.Item.FindControl("lblForm1")
                    lbl.Text = "Others :" & arrForm(0) & "<BR>"
                    lbl = e.Item.FindControl("lblForm2")
                    lbl.Text = "Materials :" & arrForm(1) & "<BR>"
                    lbl = e.Item.FindControl("lblForm3")
                    lbl.Text = "Labours :" & arrForm(2)
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
                    txt = e.Item.FindControl("txtFormula3")
                    txt.Text = arrForm(2)
            End Select
        End If
    End Sub

    Sub BindGrid()
        Dim PageNo As Integer
        Dim Period As String

        NASetup.DataSource = LoadData()
        NASetup.DataBind()
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
        DropList.Items.Add(New ListItem(objBD.mtdGetItemColumn(objBD.EnumBudgetItemColumn.Labour), objBD.EnumBudgetItemColumn.Labour))
        DropList.Items.Add(New ListItem(objBD.mtdGetItemColumn(objBD.EnumBudgetItemColumn.Material), objBD.EnumBudgetItemColumn.Material))
        DropList.Items.Add(New ListItem(objBD.mtdGetItemColumn(objBD.EnumBudgetItemColumn.Other), objBD.EnumBudgetItemColumn.Other))
        Select Case itemtype.Trim
            Case objBD.EnumBudgetItemColumn.Labour
                DropList.SelectedIndex = 0
            Case objBD.EnumBudgetItemColumn.Material
                DropList.SelectedIndex = 1
            Case objBD.EnumBudgetItemColumn.Other
                DropList.SelectedIndex = 2
        End Select

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
            If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
                strParam = strParam & _
                           " AND ACC.AccType = '" & objGLSet.EnumAccountType.BalanceSheet & "'" & _
                           " AND ACC.NurseryInd = '" & objGLSet.EnumNurseryAccount.Yes & "'" & _
                           " AND ACC.AccCode IN(SELECT BA.AccCode FROM GL_BLKACC BA LEFT OUTER JOIN GL_BLOCK BLK ON BLK.LocCode = BA.LocCode AND BLK.BlkCode = BA.BlkCode" & _
                           " WHERE BA.LocCode = '" & strLocation & "' AND BLK.BlkType = '" & objGLSet.EnumBlockType.Nursery & "' AND BLK.Status = '" & objGLSet.EnumBlockStatus.Active & "')"
            Else
                strParam = strParam & _
                           " AND ACC.AccType = '" & objGLSet.EnumAccountType.BalanceSheet & "'" & _
                           " AND ACC.NurseryInd = '" & objGLSet.EnumNurseryAccount.Yes & "'" & _
                           " AND ACC.AccCode IN(SELECT BA.AccCode FROM GL_SUBBLKACC BA LEFT OUTER JOIN GL_SUBBLK BLK ON BLK.LocCode = BA.LocCode AND BLK.SubBlkCode = BA.SubBlkCode" & _
                           " WHERE BA.LocCode = '" & strLocation & "' AND BLK.SubBlkType = '" & objGLSet.EnumSubBlockType.Nursery & "' AND BLK.Status = '" & objGLSet.EnumSubBlockStatus.Active & "')"
            End If
            intErrNo = objGLSet.mtdGetMasterList(strOpCd, _
                                                   strParam, _
                                                   objGLSet.EnumGLMasterType.AccountCode, _
                                                   dsForDropDown)
        Catch Exp As System.Exception
            Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_NURSERYACTIVITY_BindAccCodeDropList&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To dsForDropDown.Tables(0).Rows.Count - 1
            dsForDropDown.Tables(0).Rows(intCnt).Item("AccCode") = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("AccCode"))
            dsForDropDown.Tables(0).Rows(intCnt).Item("Description") = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("AccCode")) & " (" & Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("Description")) & ")"
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

        strParam = "|||" & strLocation & "|" & "DispSeq Asc||" & GetActivePeriod("")
        Try
            intErrNo = objBD.mtdGetNurseryActivityFormat(strOppCd_GET, strParam, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_NURSERYACTIVITY_GET_BLOCKTYPE&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_NurseryActivity.aspx")
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
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_NURSERYACTIVITY_GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_NurseryActivity.aspx")
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

        Droplist = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("ddlDispType")
        Select Case Droplist.SelectedItem.Value.Trim
            Case objBD.EnumBudgetFormatItem.Formula
                txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtFormula")
                txt.Visible = True
                txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtFormula1")
                txt.Visible = False
                txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtFormula2")
                txt.Visible = False
                txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtFormula3")
                txt.Visible = False
                txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtFormulaRef")
                txt.Visible = True
                Droplist = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("ddlAccCode")
                Droplist.SelectedIndex = 0
                Droplist.Visible = False
                Droplist = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("ddlDispCol")
                Droplist.Visible = True
            Case objBD.EnumBudgetFormatItem.Total
                txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtFormula")
                txt.Visible = False
                txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtFormula1")
                txt.Visible = True
                txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtFormula2")
                txt.Visible = True
                txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtFormula3")
                txt.Visible = True
                txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtFormulaRef")
                txt.Visible = True
                Droplist = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("ddlAccCode")
                Droplist.SelectedIndex = 0
                Droplist.Visible = False
                Droplist = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("ddlDispCol")
                Droplist.Visible = False
            Case objBD.EnumBudgetFormatItem.Entry
                txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtFormula")
                txt.Visible = False
                txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtFormula1")
                txt.Visible = False
                txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtFormula2")
                txt.Visible = False
                txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtFormula3")
                txt.Visible = False
                txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtFormulaRef")
                txt.Visible = True
                Droplist = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("ddlAccCode")
                Droplist.Visible = True
            Case objBD.EnumBudgetFormatItem.Header
                txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtFormula")
                txt.Visible = False
                txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtFormula1")
                txt.Visible = False
                txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtFormula2")
                txt.Visible = False
                txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtFormula3")
                txt.Visible = False
                txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtFormulaRef")
                txt.Visible = False
                Droplist = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("ddlAccCode")
                Droplist.SelectedIndex = 0
                Droplist.Visible = False
                Droplist = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("ddlDispCol")
                Droplist.Visible = False
        End Select
    End Sub

    Public Function RefCheck(ByVal RefCode As String, ByVal Seq As String, ByVal TxID As String) As Integer

        Dim objRowsRef() As DataRow
        Dim objRowsSeq() As DataRow
        Dim dsCheck As DataSet = LoadData()
        Dim strTx As String
        If TxID.Trim <> "0" Then
            strTx = " and NurseryActID <> '" & TxID.Trim & "'"
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
        Dim Droplist As DropDownList
        Dim Label As Label
        Dim txt As TextBox
        Dim strAcc As String
        Dim strtype As String
        Dim strCol As String
        Dim btn As LinkButton

        lblOper.Text = objBD.EnumOperation.Update
        Label = E.Item.FindControl("lblDisp")
        strtype = Label.Text
        Label = E.Item.FindControl("lblAcc")
        strAcc = Label.Text
        Label = E.Item.FindControl("lblCol")
        strCol = Label.Text
        NASetup.EditItemIndex = CInt(E.Item.ItemIndex)
        BindGrid()

        Droplist = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("ddlAccCode")
        BindAccCodeDropList(Droplist, strAcc)
        Droplist = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("ddlDispCol")
        BindItemColList(Droplist, strCol)
        Droplist = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("ddlDispType")
        BindItemTypeList(Droplist, strtype)

        Droplist = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("ddlDispType")
        Select Case Droplist.SelectedItem.Value.Trim
            Case objBD.EnumBudgetFormatItem.Formula
                txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtFormula")
                txt.Visible = True
                txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtFormulaRef")
                txt.Visible = True
                Droplist = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("ddlAccCode")
                Droplist.SelectedIndex = 0
                Droplist.Visible = False
            Case objBD.EnumBudgetFormatItem.Total
                txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtFormula1")
                txt.Visible = True
                txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtFormula2")
                txt.Visible = True
                txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtFormula3")
                txt.Visible = True
                txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtFormulaRef")
                txt.Visible = True
                Droplist = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("ddlAccCode")
                Droplist.SelectedIndex = 0
                Droplist.Visible = False
                Droplist = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("ddlDispCol")
                Droplist.Visible = False

            Case objBD.EnumBudgetFormatItem.Entry
                txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtFormula")
                txt.Visible = False
                txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtFormulaRef")
                txt.Visible = True
                Droplist = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("ddlAccCode")
                Droplist.Visible = True
            Case objBD.EnumBudgetFormatItem.Header
                txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtFormula")
                txt.Visible = False
                txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtFormulaRef")
                txt.Visible = False
                Droplist = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("ddlAccCode")
                Droplist.SelectedIndex = 0
                Droplist.Visible = False
        End Select

        btn = NASetup.Items.Item(CInt(E.Item.ItemIndex)).FindControl("Delete")
        btn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"

    End Sub

    Sub DEDR_Update(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strOppCd_NurseryActivity_Format_GET As String = "BD_CLSTRX_NURSERYACTIVITY_FORMAT_GET"
        Dim strOppCd_NurseryActivitySetup_GET As String = "BD_CLSSETUP_NURSERYACTIVITY_FORMAT_GET"
        Dim strOppCd_NurseryActivity_ADD As String = "BD_CLSTRX_NURSERYACTIVITY_ADD"
        Dim strOppCd_NurseryActivity_UPD As String = "BD_CLSTRX_NURSERYACTIVITY_UPD"
        Dim strOppCd_NurseryActivity_CostPerSeed_SUM As String = "BD_CLSTRX_NURSERYACTIVITY_COSTPERSEED_SUM"
        Dim strOpCd_Formula_GET As String = "BD_CLSTRX_CALCNAFORMULA_GET"

        Dim list As DropDownList
        Dim lbl As Label
        Dim txt As TextBox
        Dim rfv As RequiredFieldValidator
        Dim intError As Integer

        Dim strTx As String
        Dim strAccCode As String
        Dim strDesc As String
        Dim strDisp As String
        Dim strForm As String
        Dim strCol As String
        Dim strRef As String
        Dim strSeq As String
        Dim strDispCol As String
        Dim intCheck As Integer

        lbl = E.Item.FindControl("lblTxID")
        strTx = lbl.Text.Trim
        list = E.Item.FindControl("ddlAccCode")
        strAccCode = list.SelectedItem.Value
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
                strAccCode = ""
            Case objBD.EnumBudgetFormatItem.Total
                strAccCode = ""
                txt = E.Item.FindControl("txtFormula1")
                strForm = txt.Text & Chr(9)
                txt = E.Item.FindControl("txtFormula2")
                strForm = strForm & txt.Text & Chr(9)
                txt = E.Item.FindControl("txtFormula3")
                strForm = strForm & txt.Text
                strCol = objBD.EnumBudgetItemColumn.All
            Case objBD.EnumBudgetFormatItem.Header
                strRef = ""
                strAccCode = ""
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
                rfv = E.Item.FindControl("validateAcc")
                rfv.Visible = False
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
                strParam = GetActivePeriod("") & "|" & _
                           strTx & "|||||" & _
                           strDisp & "|" & _
                           strCol & "|" & strDispCol & "||"
                Try
                    intErrNo = objBDTrx.mtdUpdNurseryActivity(strOppCd_NurseryActivity_Format_GET, _
                                                            strOppCd_NurseryActivity_ADD, _
                                                            strOppCd_NurseryActivitySetup_GET, _
                                                            strOppCd_NurseryActivity_UPD, _
                                                            strOpCd_Formula_GET, _
                                                            "", _
                                                            strOppCd_NurseryActivity_CostPerSeed_SUM, _
                                                            strCompany, _
                                                            strLocation, _
                                                            strUserId, _
                                                            strParam, _
                                                            objBDTrx.EnumOperation.Update, _
                                                            intError, _
                                                            False)
                Catch Exp As System.Exception
                    Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_NURSERYACTIVITY_UPD&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_NurseryActivity.aspx")
                End Try

            End If
        End If

        strParam = strTx & "|" & _
                    strAccCode & "|" & _
                    strDesc & "|" & _
                    strDisp & "|" & _
                    strForm & "|" & _
                    strCol & "|" & _
                    strRef & "|" & _
                    GetActivePeriod("") & "|" & _
                    objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.NurseryActivity) & "|" & _
                    strSeq

        Try
            intErrNo = objBD.mtdUpdNurseryActivity(strOppCd_ADD, _
                                            strOppCd_UPD, _
                                            strCompany, _
                                            strLocation, _
                                            strUserId, _
                                            strParam, _
                                            lblOper.Text, _
                                            intError)
        Catch Exp As System.Exception
            Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_NURSERYACTIVITY_UPD_BLOCKTYPE&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_NurseryActivity.aspx")
        End Try

        NASetup.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        If CInt(E.Item.ItemIndex) = 0 And NASetup.Items.Count = 1 And NASetup.PageCount <> 1 Then
            NASetup.CurrentPageIndex = NASetup.PageCount - 2
        End If
        NASetup.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strOppCd_NurseryActivitySetup_DEL As String = "BD_CLSSETUP_NURSERYACTIVITY_DEL"
        Dim strOppCd_NurseryActivityDist_Format_GET As String = "BD_CLSTRX_NURSERYACTIVITYDIST_FORMAT_GET"
        Dim strOppCd_NurseryActivity_DEL As String = "BD_CLSTRX_NURSERYACTIVITY_DEL"
        Dim strOppCd_NurseryActivityDist_DEL As String = "BD_CLSTRX_NURSERYACTIVITYDIST_DEL"
        Dim strOppCd_NurseryActivityDistAccPeriod_DEL As String = "BD_CLSTRX_NURSERYACTIVITYDIST_ACCPERIOD_DEL"
        Dim strOppCd_BgtPeriod_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"

        Dim strID As String
        Dim strAccCode As String
        Dim intError As Integer
        Dim intCnt As Integer
        Dim label As label
        Dim dsNurseryActivityDist As New DataSet()

        label = E.Item.FindControl("lblTxID")
        strID = label.Text.Trim
        label = E.Item.FindControl("lblAcc")
        strAccCode = label.Text.Trim

        strParam = strLocation & "|" & GetActivePeriod("") & "|" & strAccCode & "||||"
        Try
            intErrNo = objBDTrx.mtdGetNurseryActivityDistFormat(strOppCd_NurseryActivityDist_Format_GET, _
                                                         strOppCd_BgtPeriod_GET, _
                                                         strParam, _
                                                         dsNurseryActivityDist, _
                                                         intError, _
                                                         False)
        Catch Exp As System.Exception
            Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_GET_NURSERYACTIVITYDIST&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_NurseryActivity.aspx")
        End Try

        strParam = strID & "|" & strLocation & "|" & GetActivePeriod("") & "|"
        Try
            intErrNo = objBD.mtdDelNurseryActivity(strOppCd_NurseryActivitySetup_DEL, _
                                            strParam, _
                                            intError)
        Catch Exp As System.Exception
            Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_NURSERYACTIVITYSETUP_DEL&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_NurseryActivity.aspx")
        End Try

        strParam = strID & "|" & strLocation & "|" & GetActivePeriod("") & "|"
        Try
            intErrNo = objBD.mtdDelNurseryActivity(strOppCd_NurseryActivity_DEL, _
                                            strParam, _
                                            intError)
        Catch Exp As System.Exception
            Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_NURSERYACTIVITY_DEL&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_NurseryActivity.aspx")
        End Try

        strParam = strAccCode & "|" & strLocation & "|" & GetActivePeriod("") & "|"
        Try
            intErrNo = objBD.mtdDelNurseryActivityDist(strOppCd_NurseryActivityDist_DEL, _
                                            strParam, _
                                            intError)
        Catch Exp As System.Exception
            Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_NURSERYACTIVITYDIST_DEL&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_NurseryActivity.aspx")
        End Try

        For intCnt = 0 To dsNurseryActivityDist.Tables(0).Rows.Count - 1
            strParam = Trim(dsNurseryActivityDist.Tables(0).Rows(intCnt).Item("NurseryActDistID")) & "|||"
            Try
                intErrNo = objBD.mtdDelNurseryActivityDist(strOppCd_NurseryActivityDistAccPeriod_DEL, _
                                                strParam, _
                                                intError)
            Catch Exp As System.Exception
                Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_NURSERYACTIVITYDISTACCPERIOD_DEL&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_NurseryActivity.aspx")
            End Try
        Next

        NASetup.EditItemIndex = -1
        BindGrid()

    End Sub

    Sub DEDR_Add(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim dataSet As dataSet = LoadData()
        Dim newRow As DataRow
        Dim Updbutton As LinkButton
        Dim txt As TextBox
        Dim Droplist As DropDownList
        Dim lbl As Label

        newRow = dataSet.Tables(0).NewRow()
        newRow.Item("NurseryActID") = 0
        newRow.Item("AccCode") = ""
        newRow.Item("DispSeq") = 0
        newRow.Item("ItemDescription") = ""
        newRow.Item("ItemDisplayType") = 1
        newRow.Item("FormulaRef") = ""
        newRow.Item("ItemCalcFormula") = ""
        newRow.Item("ItemDisplayCol") = 1
        newRow.Item("LocCode") = ""
        newRow.Item("CreateDate") = DateTime.Now()
        newRow.Item("UpdateDate") = DateTime.Now()
        newRow.Item("UserName") = ""
        dataSet.Tables(0).Rows.Add(newRow)

        NASetup.DataSource = dataSet
        NASetup.DataBind()

        NASetup.CurrentPageIndex = NASetup.PageCount - 1
        NASetup.EditItemIndex = NASetup.Items.Count - 1
        NASetup.DataBind()
        lblOper.Text = objBD.EnumOperation.Add

        Droplist = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("ddlAccCode")
        BindAccCodeDropList(Droplist)
        Droplist = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("ddlDispType")
        BindItemTypeList(Droplist)
        Droplist = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("ddlDispCol")
        BindItemColList(Droplist)
        Updbutton = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("Delete")
        Updbutton.Visible = False

        If NASetup.Items.Count > 1 Then
            lbl = NASetup.Items.Item(CInt(NASetup.EditItemIndex - 1)).FindControl("lblSeq")
            txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtDispSeq")
            txt.Text = lbl.Text + 7
        Else
            txt = NASetup.Items.Item(CInt(NASetup.EditItemIndex)).FindControl("txtDispSeq")
            txt.Text = 1
        End If
        txt.Enabled = False

    End Sub

End Class
