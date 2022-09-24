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

Public Class BD_ManuringFert : Inherits Page

    Protected WithEvents dgManuringFert As DataGrid
    Protected WithEvents ErrorMessage As Label
    Protected WithEvents lblTracker As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents StatusList As DropDownList
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents blnUpdate As Label
    Protected WithEvents lblDupMsg As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents ddlFertItemCode As DropDownList

    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBudgeting As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblOper As Label
    Protected WithEvents lblLocTag As Label

    Protected WithEvents srchFertItemCode As TextBox
    Protected WithEvents srchDesc As TextBox
    Protected WithEvents srchStatusList As DropDownList
    Protected WithEvents srchUpdateBy As TextBox

    Protected objBD As New agri.BD.clsSetup()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap
    Protected objINSetup As New agri.IN.clsSetup
    Dim objLoc As New agri.Admin.clsLoc()

    Dim strOppCd_GET As String = "BD_CLSSETUP_MANURINGFERT_GET"
    Dim strOppCd_ADD As String = "BD_CLSSETUP_MANURINGFERT_ADD"
    Dim strOppCd_UPD As String = "BD_CLSSETUP_MANURINGFERT_UPD"

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

    Const ITEM_PART_SEPERATOR As String = " @ "
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
            onload_GetLangCap()
            If SortExpression.Text = "" Then
                SortExpression.Text = "Description"
                SortCol.Text = "ASC"
            End If

            If Not Page.IsPostBack Then
                BindSrchStatusList()
                BindGrid()
                BindPageList()
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
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_MANURINGFERT_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_ManuringFert.aspx")
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


    Sub BindSrchStatusList()

        srchStatusList.Items.Add(New ListItem(objBD.mtdGetManuringFertStatus(objBD.EnumManuringFertStatus.All), objBD.EnumManuringFertStatus.All))
        srchStatusList.Items.Add(New ListItem(objBD.mtdGetManuringFertStatus(objBD.EnumManuringFertStatus.Active), objBD.EnumManuringFertStatus.Active))
        srchStatusList.Items.Add(New ListItem(objBD.mtdGetManuringFertStatus(objBD.EnumManuringFertStatus.Deleted), objBD.EnumManuringFertStatus.Deleted))

    End Sub

    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        dgManuringFert.CurrentPageIndex = 0
        dgManuringFert.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub BindGrid()
        Dim Period As String

        lblLocCode.Text = strLocation
        GetActivePeriod(Period)
        lblBgtPeriod.Text = Period
        Dim PageNo As Integer 
        Dim PageCount As Integer
        Dim dsData As DataSet
        
        dsData = LoadData
        PageCount = objGlobal.mtdGetPageCount(dsData.Tables(0).Rows.Count, dgManuringFert.PageSize)
        
        dgManuringFert.DataSource = dsData
        If dgManuringFert.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgManuringFert.CurrentPageIndex = 0
            Else
                dgManuringFert.CurrentPageIndex = PageCount - 1
            End If
        End If
        
        dgManuringFert.DataBind()
        BindPageList()
        PageNo = dgManuringFert.CurrentPageIndex + 1
        lblTracker.Text="Page " & pageno & " of " & dgManuringFert.PageCount
    End Sub

    Protected Function GetActivePeriod(ByRef BGTPeriod As String) As String
        Dim strOppCd_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"
        Dim dsperiod As New DataSet()

        strParam = "|||||" & objBD.EnumPeriodStatus.Active & "|" & strLocation & "|"

        Try
            intErrNo = objBD.mtdGetPeriodList(strOppCd_GET, strParam, dsperiod)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_ManuringFert.aspx")
        End Try
        If dsperiod.Tables(0).Rows.Count > 0 Then
            BGTPeriod = dsperiod.Tables(0).Rows(0).Item("BGTPeriod")
            Return dsperiod.Tables(0).Rows(0).Item("PeriodID")
        Else
            BGTPeriod = "No Active Period"
            Return ""
        End If
    End Function

    Sub BindPageList()

        Dim count As Integer = 1
        Dim arrDList As New ArrayList()

        While Not count = dgManuringFert.PageCount + 1
            arrDList.Add("Page " & count)
            count = count + 1
        End While

        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = dgManuringFert.CurrentPageIndex

    End Sub

    Sub BindStatusList(ByVal index As Integer)

        StatusList = dgManuringFert.Items.Item(index).FindControl("StatusList")
        StatusList.Items.Add(New ListItem(objBD.mtdGetManuringFertStatus(objBD.EnumManuringFertStatus.Active), objBD.EnumManuringFertStatus.Active))
        StatusList.Items.Add(New ListItem(objBD.mtdGetManuringFertStatus(objBD.EnumManuringFertStatus.Deleted), objBD.EnumManuringFertStatus.Deleted))

    End Sub

    Protected Function LoadData() As DataSet

        strParam = srchFertItemCode.Text & "|" & _
                   srchDesc.Text & "|" & _
                   srchUpdateBy.Text & "|" & _
                   srchStatusList.SelectedItem.Value & "|" & _
                   SortExpression.Text & " " & SortCol.Text & "||"
        Try
            intErrNo = objBD.mtdGetManuringFert(strOppCd_GET, strParam, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_MANURINGFERT&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_ManuringFert.aspx")
        End Try

        Return objDataSet
    End Function

    Sub btnPrevNext_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).CommandArgument
        Select Case direction
            Case "first"
                dgManuringFert.CurrentPageIndex = 0
            Case "prev"
                dgManuringFert.CurrentPageIndex = _
                    Math.Max(0, dgManuringFert.CurrentPageIndex - 1)
            Case "next"
                dgManuringFert.CurrentPageIndex = _
                    Math.Min(dgManuringFert.PageCount - 1, dgManuringFert.CurrentPageIndex + 1)
            Case "last"
                dgManuringFert.CurrentPageIndex = dgManuringFert.PageCount - 1
        End Select
        lstDropList.SelectedIndex = dgManuringFert.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            dgManuringFert.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        dgManuringFert.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIf(SortCol.Text = "ASC", "DESC", "ASC")
        dgManuringFert.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
    End Sub

    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim EditText As TextBox
        Dim List As DropDownList
        Dim Updbutton As LinkButton
        Dim ItemCode As String

        lblOper.Text = objBD.EnumOperation.Update
        dgManuringFert.EditItemIndex = CInt(E.Item.ItemIndex)

        BindGrid()
        If CInt(e.Item.ItemIndex) >= dgManuringFert.Items.Count then
            dgManuringFert.EditItemIndex = -1
            Exit Sub
        End If
        BindStatusList(dgManuringFert.EditItemIndex)

        EditText = dgManuringFert.Items.Item(CInt(E.Item.ItemIndex)).FindControl("txtItemCode")
        ItemCode = Trim(EditText.Text)
        List = dgManuringFert.Items.Item(CInt(E.Item.ItemIndex)).FindControl("ddlFertItemCode")
        List.Enabled = False

        BindINItem(List, ItemCode)

        EditText = dgManuringFert.Items.Item(CInt(E.Item.ItemIndex)).FindControl("txtStatus")
        Select Case CInt(EditText.Text) = objBD.EnumManuringFertStatus.Active
            Case True
                StatusList.SelectedIndex = 0
                EditText = dgManuringFert.Items.Item(CInt(E.Item.ItemIndex)).FindControl("txtItemCode")
                EditText.ReadOnly = True
                Updbutton = dgManuringFert.Items.Item(CInt(E.Item.ItemIndex)).FindControl("Delete")
                Updbutton.Text = "Delete"
                Updbutton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
            Case False
                StatusList.SelectedIndex = 1
                EditText = dgManuringFert.Items.Item(CInt(E.Item.ItemIndex)).FindControl("txtItemCode")
                EditText.Enabled = False
                EditText = dgManuringFert.Items.Item(CInt(E.Item.ItemIndex)).FindControl("txtDesc")
                EditText.Enabled = False
                EditText = dgManuringFert.Items.Item(CInt(E.Item.ItemIndex)).FindControl("txtUpdateDate")
                EditText.Enabled = False
                EditText = dgManuringFert.Items.Item(CInt(E.Item.ItemIndex)).FindControl("txtUserName")
                EditText.Enabled = False
                List = dgManuringFert.Items.Item(CInt(E.Item.ItemIndex)).FindControl("StatusList")
                List.Enabled = False
                Updbutton = dgManuringFert.Items.Item(CInt(E.Item.ItemIndex)).FindControl("Update")
                Updbutton.Visible = False
                Updbutton = dgManuringFert.Items.Item(CInt(E.Item.ItemIndex)).FindControl("Delete")
                Updbutton.Text = "Undelete"
        End Select
    End Sub

    Sub DEDR_Update(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim EditText As TextBox
        Dim intError As Integer
        Dim strCode As String
        Dim strDesc As String
        Dim lblMsg As Label
        Dim list As DropDownList

        list = E.Item.FindControl("ddlFertItemCode")
        strCode = list.SelectedItem.Value

        EditText = E.Item.FindControl("txtDesc")
        strDesc = EditText.Text

        strParam = strCode & "|" & strDesc & "|"
        Try
            intErrNo = objBD.mtdUpdManuringFert(strOppCd_ADD, _
                                                strOppCd_UPD, _
                                                strOppCd_GET, _
                                                strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strAccMonth, _
                                                strAccYear, _
                                                strParam, _
                                                lblOper.Text, _
                                                intError)

        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=UPD_MANURINGFERT&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_ManuringFert.aspx")
        End Try

        If intError = objBD.EnumErrorType.duplicateKey Then
            lblMsg = E.Item.FindControl("lblDupMsg")
            lblMsg.Visible = True
        Else
            dgManuringFert.EditItemIndex = -1
            BindGrid()
        End If

    End Sub

    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        dgManuringFert.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)

        Dim EditText As TextBox
        Dim strCode As String
        Dim strDesc As String
        Dim strStatus As String
        Dim blnDupKey As Boolean = False
        Dim strCreateDate As String
        Dim intError As Integer
        Dim list As DropDownList

        list = E.Item.FindControl("ddlFertItemCode")
        strCode = list.SelectedItem.Value

        EditText = E.Item.FindControl("txtStatus")
        strStatus = IIf(EditText.Text = objBD.EnumManuringFertStatus.Active, objBD.EnumManuringFertStatus.Deleted, objBD.EnumManuringFertStatus.Active)

        strParam = strCode & "||" & strStatus & "|"
        Try
            intErrNo = objBD.mtdUpdManuringFert(strOppCd_ADD, _
                                                strOppCd_UPD, _
                                                strOppCd_GET, _
                                                strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strAccMonth, _
                                                strAccYear, _
                                                strParam, _
                                                lblOper.Text, _
                                                intError)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=DEL_MANURINGFERT&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_ManuringFert.aspx")
        End Try

        dgManuringFert.EditItemIndex = -1
        BindGrid()

    End Sub

    Sub DEDR_Add(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim dataSet As dataSet = LoadData()
        Dim newRow As DataRow
        Dim Updbutton As LinkButton
        Dim PageCount As Integer
        Dim List As DropDownList

        lblOper.Text = objBD.EnumOperation.Add
        newRow = dataSet.Tables(0).NewRow()
        newRow.Item("FertItemCode") = ""
        newRow.Item("Description") = ""
        newRow.Item("Status") = 1
        newRow.Item("CreateDate") = DateTime.Now()
        newRow.Item("UpdateDate") = DateTime.Now()
        newRow.Item("UserName") = ""
        dataSet.Tables(0).Rows.Add(newRow)

        dgManuringFert.DataSource = dataSet
        PageCount = objGlobal.mtdGetPageCount(dataSet.Tables(0).Rows.Count, dgManuringFert.PageSize)
        If dgManuringFert.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgManuringFert.CurrentPageIndex = 0
            Else
                dgManuringFert.CurrentPageIndex = PageCount - 1
            End If
        End If
        dgManuringFert.DataBind()

        BindPageList()

        dgManuringFert.CurrentPageIndex = dgManuringFert.PageCount - 1
        lblTracker.Text="Page " & (dgManuringFert.CurrentPageIndex + 1) & " of " & dgManuringFert.PageCount
        lstDropList.SelectedIndex = dgManuringFert.CurrentPageIndex
        dgManuringFert.DataBind()
        dgManuringFert.EditItemIndex = dgManuringFert.Items.Count - 1
        dgManuringFert.DataBind()
        BindStatusList(dgManuringFert.EditItemIndex)

        List = dgManuringFert.Items.Item(CInt(dgManuringFert.EditItemIndex)).FindControl("ddlFertItemCode")
        List.Enabled = True
        BindINItem(List, "")

        Updbutton = dgManuringFert.Items.Item(CInt(dgManuringFert.EditItemIndex)).FindControl("Delete")
        Updbutton.Visible = False

    End Sub

    Sub BindINItem(ByRef lstItemCode As DropDownList, ByVal ItemCode As String)
        Dim strOpCd As String = "IN_CLSTRX_ITEMPART_ITEM_GET"
        Dim objINItemDs As DataSet
        Dim strParam As String = ""
        Dim dr As DataRow
        Dim SelectedIndex As Integer = -1
        Dim intCnt As Integer = 0
        Dim intErrNo As Integer

        strParam = objINSetup.EnumInventoryItemType.Stock & "|" & _
                   strLocation & "|" & _
                   objINSetup.EnumStockItemStatus.Active & "|itm.ItemCode"

        Try
            intErrNo = objBD.mtdGetItem(strOpCd, strParam, objINItemDs)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_GET_INITEM&errmesg=" & Exp.ToString() & "&redirect=BD/Setup/BD_setup_ManuringFert.aspx")
        End Try

        For intCnt = 0 To objINItemDs.Tables(0).Rows.Count - 1
            objINItemDs.Tables(0).Rows(intCnt).Item("ItemCode") = objINItemDs.Tables(0).Rows(intCnt).Item("ItemCode")

            If objINItemDs.Tables(0).Rows(intCnt).Item("PartNo") <> "" Then
                objINItemDs.Tables(0).Rows(intCnt).Item("Description") = objINItemDs.Tables(0).Rows(intCnt).Item("ItemCode") & ITEM_PART_SEPERATOR & _
                                                                         objINItemDs.Tables(0).Rows(intCnt).Item("PartNo") & " (" & _
                                                                         objINItemDs.Tables(0).Rows(intCnt).Item("Description") & ") " 
                objINItemDs.Tables(0).Rows(intCnt).Item("ItemCode") = objINItemDs.Tables(0).Rows(intCnt).Item("ItemCode") & ITEM_PART_SEPERATOR & _
                                                                      objINItemDs.Tables(0).Rows(intCnt).Item("PartNo")
            Else
                objINItemDs.Tables(0).Rows(intCnt).Item("Description") = objINItemDs.Tables(0).Rows(intCnt).Item("ItemCode") & " (" & _
                                                                         objINItemDs.Tables(0).Rows(intCnt).Item("Description") & ") " 
            End If

            If Not ItemCode = "" Then
                If UCase(Trim(objINItemDs.Tables(0).Rows(intCnt).Item("ItemCode"))) = ItemCode Then
                    SelectedIndex = intCnt + 1
                End If
            End If

        Next intCnt

        dr = objINItemDs.Tables(0).NewRow()
        dr(2) = "Please Select Item Code "
        objINItemDs.Tables(0).Rows.InsertAt(dr, 0)

        lstItemCode.DataSource = objINItemDs.Tables(0)
        lstItemCode.DataValueField = "ItemCode"
        lstItemCode.DataTextField = "Description"
        lstItemCode.DataBind()

        If Not ItemCode = "" Then
            If SelectedIndex = -1 Then
                lstItemCode.Items.Add(New ListItem(Trim(ItemCode), Trim(ItemCode)))
                SelectedIndex = lstItemCode.Items.Count - 1
            End If
            lstItemCode.SelectedIndex = SelectedIndex
        End If

        If Not objINItemDs Is Nothing Then
            objINItemDs = Nothing
        End If
    End Sub


End Class
