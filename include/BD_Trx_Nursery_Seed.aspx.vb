Imports System
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic


Public Class BD_Nursery_Seed : Inherits Page

    Protected WithEvents EventData As DataGrid
    Protected WithEvents ErrorMessage As Label
    Protected WithEvents lblTracker As Label
    Protected WithEvents SQLStatement As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents BlkCodeList As DropDownList
    Protected WithEvents lstBlkCode As DropDownList
    Protected WithEvents srchBlkCodeList As TextBox
    Protected WithEvents SortExpression As Label
    Protected WithEvents blnUpdate As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblDupMsg As Label
    Protected WithEvents sortcol As Label
    Protected WithEvents SeedID As TextBox
    Protected WithEvents BlkCode As TextBox
    Protected WithEvents txtQty As TextBox
    Protected WithEvents CreateDate As TextBox
    Protected WithEvents UpdateDate As TextBox
    Protected WithEvents lblTitle As Label
    Protected WithEvents lblMonthYear As Label
    Protected WithEvents lblSeedID As Label
    Protected WithEvents lblBlkCode As Label
    Protected WithEvents lblQty As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblStatus As Label
    Protected WithEvents lblPleaeEnter As Label
    Protected WithEvents lblList As Label
    Protected WithEvents lblUserID As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents lblErrMsg As Label
    Protected WithEvents lblPleaseSelect As Label

    Protected objBD As New agri.BD.clsTrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objGL As New agri.GL.clsSetup()
    Dim objBDTrx As New agri.BD.clstrx()
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Dim strOppCd_GET As String = "BD_CLSTRX_NURSERYSEED_LIST_GET"
    Dim strOppCd_ADD As String = "BD_CLSTRX_NURSERYSEED_LIST_ADD"
    Dim strOppCd_UPD As String = "BD_CLSTRX_NURSERYSEED_LIST_UPD"
    Dim strOppCd_DEL As String = "BD_CLSTRX_NURSERYSEED_LIST_DEL"

    Dim objDataSet As New Object()
    Dim objLangCapDs As New Object()

    Dim intErrNo As Integer
    Dim strParam As String = ""
    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim intADAR As Integer
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strValidateMonthYear As String

    Dim DocTitleTag As String
    Dim SeedIDTag As String
    Dim BlkCodeTag As String
    Dim QtyTag As String
    Dim LocCodeTag As String
    Dim StatusTag As String
    Dim CreateDateTag As String
    Dim UpdateDateTag As String
    Dim UpdateIDTag As String
    Dim intConfigsetting As Integer
    Dim strLocType as String

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        intADAR = Session("SS_ADAR")
        strAccMonth = Session("SS_GLACCMONTH")
        strAccYear = Session("SS_GLACCYEAR")
        intConfigsetting = Session("SS_CONFIGSETTING")
        strLocType = Session("SS_LOCTYPE")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumADAccessRights.ADBudgeting), intADAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")

        Else
            onload_GetLangCap()
            If SortExpression.Text = "" Then
                SortExpression.Text = "BlkCode"
                sortcol.Text = "ASC"
            End If
            If Not Page.IsPostBack Then
                BindGrid() 
                BindPageList()
            End If

        End If
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblTitle.Text = "NURSERY SEED LIST"
        DocTitleTag = lblTitle.Text

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
            lblBlkCode.Text = GetCaption(objLangCap.EnumLangCap.Block) & lblcode.text
        Else
            lblBlkCode.Text = GetCaption(objLangCap.EnumLangCap.SubBlock) & lblCode.Text
        End If
        EventData.Columns(1).HeaderText = lblBlkCode.Text

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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BD_TRX_NURSERY_SEED_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_Trx_Nursery_Seed.aspx")
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

    Protected Function LoadData() As DataSet

        Dim UpdateBy As String
        Dim srchStatus As String
        Dim strParam As String
        Dim SearchStr As String
        Dim sortitem As String
        Dim intCnt As Integer


        If srchBlkCodeList.Text = "" Then
            SearchStr = "%"
        Else
            SearchStr = Trim(srchBlkCodeList.Text) & "%"
        End If

        SearchStr = " AND BD.BlkCode like '" & SearchStr & "'" & _
                    " AND BD.LocCode like '" & Trim(strLocation) & "'"

        If Not txtQty.Text = "" Then
            SearchStr = SearchStr & " AND BD.QTY = " & txtQty.Text
        End If

        sortitem = " ORDER BY " & SortExpression.Text & " " & sortcol.Text

        strParam = sortitem & "|" & SearchStr

        Try
            strOppCd_GET = "BD_CLSTRX_NURSERYSEED_LIST_GET"
            intErrNo = objBD.mtdGetNurserySeed(strOppCd_GET, strParam, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_NURSERY_SEED&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_Trx_NURSERY_SEED.aspx")
        End Try

        If objDataSet.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objDataSet.Tables(0).Rows.Count - 1
                objDataSet.Tables(0).Rows(intCnt).Item("SeedID") = Trim(objDataSet.Tables(0).Rows(intCnt).Item("SeedID"))
                objDataSet.Tables(0).Rows(intCnt).Item("BlkCode") = Trim(objDataSet.Tables(0).Rows(intCnt).Item("BlkCode"))
                objDataSet.Tables(0).Rows(intCnt).Item("Qty") = Trim(objDataSet.Tables(0).Rows(intCnt).Item("Qty"))
                objDataSet.Tables(0).Rows(intCnt).Item("Status") = Trim(objDataSet.Tables(0).Rows(intCnt).Item("Status"))
                objDataSet.Tables(0).Rows(intCnt).Item("CreateDate") = Trim(objDataSet.Tables(0).Rows(intCnt).Item("CreateDate"))
                objDataSet.Tables(0).Rows(intCnt).Item("UpdateDate") = Trim(objDataSet.Tables(0).Rows(intCnt).Item("UpdateDate"))
                objDataSet.Tables(0).Rows(intCnt).Item("UserName") = Trim(objDataSet.Tables(0).Rows(intCnt).Item("UserName"))
            Next
        End If

        Return objDataSet
    End Function


    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        EventData.CurrentPageIndex = 0
        EventData.EditItemIndex = -1
        BindGrid()
        BindPageList()
    End Sub

    Sub BindPageList()

        Dim count As Integer = 1
        Dim arrDList As New ArrayList()

        While Not count = EventData.PageCount + 1
            arrDList.Add("Page " & count)
            count = count + 1
        End While
        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = EventData.CurrentPageIndex

    End Sub

    Sub BindGrid()

        Dim PageNo As Integer
        Dim PageCount As Integer
        Dim dsData As DataSet

        dsData = LoadData()
        PageCount = objGlobal.mtdGetPageCount(dsData.Tables(0).Rows.Count, EventData.PageSize)

        EventData.DataSource = dsData
        If EventData.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                EventData.CurrentPageIndex = 0
            Else
                EventData.CurrentPageIndex = PageCount - 1
            End If
        End If

        EventData.DataBind()
        BindPageList()
        PageNo = EventData.CurrentPageIndex + 1
        lblTracker.Text = "Page " & PageNo & " of " & EventData.PageCount
    End Sub


    Protected Function BindBlkCodeList(ByVal intIndex As Integer)
        Dim intcnt As Integer
        Dim intSelIndex As Integer
        Dim lbl As Label

        lstBlkCode = EventData.Items.Item(intIndex).FindControl("lstBlkCode")
        lbl = EventData.Items.Item(intIndex).FindControl("lblBlkCode")

        objDataSet = LoadBlkCodeData()

        If objDataSet.Tables(0).Rows.Count > 0 Then
            For intcnt = 0 To objDataSet.Tables(0).Rows.Count - 1
                objDataSet.Tables(0).Rows(intcnt).Item("BlkCode") = Trim(objDataSet.Tables(0).Rows(intcnt).Item("BlkCode"))
                If Trim(objDataSet.Tables(0).Rows(intcnt).Item("BlkCode")) = lbl.Text Then
                    intSelIndex = intcnt
                End If
            Next
        End If

        lstBlkCode.DataSource = objDataSet
        lstBlkCode.DataValueField = "BlkCode"
        lstBlkCode.DataTextField = "BlkCode"
        lstBlkCode.DataBind()

        lstBlkCode.SelectedIndex = intSelIndex

    End Function

    Protected Function BindNewBlkCodeList(ByVal intIndex As Integer)
        Dim intcnt As Integer
        Dim intSelIndex As Integer
        Dim lbl As Label
        Dim drinsert As DataRow

        lstBlkCode = EventData.Items.Item(intIndex).FindControl("lstBlkCode")
        lbl = EventData.Items.Item(intIndex).FindControl("lblBlkCode")

        objDataSet = LoadBlkCodeData()

        drinsert = objDataSet.Tables(0).NewRow()
        drinsert(0) = lblPleaseSelect.Text & lblBlkCode.Text
        objDataSet.Tables(0).Rows.InsertAt(drinsert, 0)

        lstBlkCode.DataSource = objDataSet.Tables(0)
        lstBlkCode.DataValueField = "BlkCode"
        lstBlkCode.DataTextField = "BlkCode"
        lstBlkCode.DataBind()

    End Function

    Protected Function LoadBlkCodeData() As DataSet

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
            strOppCd_GET = "BD_CLSTRX_NURSERYSEED_BLOCKCODE_GET"
            strParam = objGL.EnumBlockType.Nursery & "|" & objGL.EnumBlockStatus.Active & "|" & strLocation & "|"
        Else
            strOppCd_GET = "BD_CLSTRX_NURSERYSEED_SUBBLKCODE_GET"
            strParam = objGL.EnumSubBlockType.Nursery & "|" & objGL.EnumSubBlockStatus.Active & "|" & strLocation & "|"
        End If

        Try
            intErrNo = objBDTrx.mtdGetNurserySeedBlkCode(strOppCd_GET, strParam, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_NURSERY_SEED&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_Nursery_Seed.aspx")
        End Try

        Return objDataSet

    End Function


    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            EventData.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        EventData.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        sortcol.Text = IIf(sortcol.Text = "ASC", "DESC", "ASC")
        EventData.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
    End Sub

    Sub btnPreview_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim strStatus As String
        Dim strBlkCode As String
        Dim strQty As String
        Dim strUpdateBy As String
        Dim strSortExp As String
        Dim strSortCol As String

        strBlkCode = srchBlkCodeList.Text
        strQty = txtQty.Text
        strSortExp = SortExpression.Text
        strSortCol = sortcol.Text

        Response.Write("<Script Language=""JavaScript"">window.open(""../reports/BD_Rpt_NurserySeedList.aspx?strStatus=" & strStatus & _
                       "&strBlkCode=" & strBlkCode & _
                       "&strQty=" & strQty & _
                       "&strSortExp=" & strSortExp & _
                       "&strSortCol=" & strSortCol & _
                       "&DocTitleTag=" & DocTitleTag & _
                       """, null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")
    End Sub

    Sub btnPrevNext_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).CommandArgument
        Select Case direction
            Case "first"
                EventData.CurrentPageIndex = 0
            Case "prev"
                EventData.CurrentPageIndex = _
                    Math.Max(0, EventData.CurrentPageIndex - 1)
            Case "next"
                EventData.CurrentPageIndex = _
                    Math.Min(EventData.PageCount - 1, EventData.CurrentPageIndex + 1)
            Case "last"
                EventData.CurrentPageIndex = EventData.PageCount - 1
        End Select
        lstDropList.SelectedIndex = EventData.CurrentPageIndex
        BindGrid()
    End Sub


    Sub DEDR_Edit(ByVal Sender As Object, ByVal e As DataGridCommandEventArgs)

        Dim EditText As TextBox
        Dim Updbutton As LinkButton
        Dim lst As DropDownList
        Dim lbl As Label
        Dim strBlkCode As String

        blnUpdate.Text = True
        EventData.EditItemIndex = CInt(e.Item.ItemIndex)
        BindGrid()

        EditText = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("SeedID")
        EditText.ReadOnly = True

        lbl = e.Item.FindControl("lblBlkCode")
        strBlkCode = lbl.Text
        BindBlkCodeList(e.Item.ItemIndex)
        lst = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lstBlkCode")

        EditText = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("txtQty")
        EditText.ReadOnly = False

        Updbutton = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("Delete")
        Updbutton.Text = "Delete"
        Updbutton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"

    End Sub

    Sub DEDR_Update(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)

        Dim EditText As TextBox
        Dim lstBlkCode As DropDownList
        Dim SeedID As String
        Dim BlkCode As String
        Dim Qty As String
        Dim blnDupKey As Boolean = False
        Dim lblDupMsg As Label
        Dim lblErrMsg As Label

        lblErrMsg = E.Item.FindControl("lblErrMsg")
        lblDupMsg = E.Item.FindControl("lblDupMsg")
        lblDupMsg.Visible = False
        lblErrMsg.Visible = False

        EditText = E.Item.FindControl("SeedID")
        SeedID = Trim(EditText.Text)

        lstBlkCode = E.Item.FindControl("lstBlkCode")
        BlkCode = Trim(lstBlkCode.SelectedItem.Value)

        If BlkCode = lblPleaseSelect.Text & lblBlkCode.Text Then
            lblErrMsg.Visible = True
            lblErrMsg.Text = lblPleaseSelect.Text & lblBlkCode.Text
            Exit Sub
        End If

        EditText = E.Item.FindControl("txtQty")
        Qty = Trim(EditText.Text)

        strParam = SeedID & "|" & BlkCode & "|" & Qty

        Try
            intErrNo = objBD.mtdUpdNurserySeed(strOppCd_ADD, _
                                               strOppCd_UPD, _
                                               strOppCd_GET, _
                                               strCompany, _
                                               strLocation, _
                                               strUserId, _
                                               strParam, _
                                               blnDupKey, _
                                               blnUpdate.Text)

            If intErrNo <> 0 Then
                lblDupMsg.Visible = True
                lblDupMsg.Text = lblBlkCode.Text & " already has quantity"
            End If


        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=UPDATE_NURSERY_SEED&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_Trx_NurserySeed.aspx")
        End Try

        If blnDupKey Then
        Else
            EventData.EditItemIndex = -1
            BindGrid()
        End If

    End Sub

    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        If CInt(E.Item.ItemIndex) = 0 And EventData.Items.Count = 1 And Not EventData.CurrentPageIndex = 0 Then
            EventData.CurrentPageIndex = EventData.PageCount - 2
            BindGrid()
            BindPageList()
        End If
        EventData.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)

        Dim EditText As TextBox
        Dim SeedID As String
        Dim blnDupKey As Boolean = False
        Dim lblMsg As Label
        Dim intError As Integer
        Dim delStatus As String

        EditText = E.Item.FindControl("SeedID")
        SeedID = EditText.Text

        Try
            intErrNo = objBD.mtdDelNurserySeed(strOppCd_DEL, _
                                               SeedID, _
                                               intError)

        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=DELETE_NURSERY_SEED&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_Trx_Nursery_=Seed.aspx")
        End Try

        EventData.EditItemIndex = -1
        BindGrid()

    End Sub

    Sub DEDR_Add(ByVal Sender As Object, ByVal E As ImageClickEventArgs)

        Dim dataSet As dataSet = LoadData()
        Dim newRow As DataRow
        Dim Updbutton As LinkButton

        blnUpdate.Text = False
        newRow = dataSet.Tables(0).NewRow()
        newRow.Item("SeedID") = 0
        newRow.Item("BlkCode") = ""
        newRow.Item("Qty") = 0
        newRow.Item("LocCode") = ""
        newRow.Item("Status") = 1
        newRow.Item("CreateDate") = DateTime.Now()
        newRow.Item("UpdateDate") = DateTime.Now()
        newRow.Item("UserName") = ""
        dataSet.Tables(0).Rows.Add(newRow)

        EventData.DataSource = dataSet
        EventData.DataBind()
        BindPageList()

        EventData.CurrentPageIndex = EventData.PageCount - 1
        lstDropList.SelectedIndex = EventData.CurrentPageIndex
        EventData.DataBind()
        EventData.EditItemIndex = EventData.Items.Count - 1
        EventData.DataBind()
        BindNewBlkCodeList(EventData.EditItemIndex)
        Updbutton = EventData.Items.Item(CInt(EventData.EditItemIndex)).FindControl("Delete")
        Updbutton.Visible = False

    End Sub

End Class
