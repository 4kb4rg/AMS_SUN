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


Public Class BD_Blocktype : Inherits Page

    Protected WithEvents BlockCrop As DataGrid
    Protected WithEvents ErrorMessage As Label
    Protected WithEvents lblTracker As Label
    Protected WithEvents SQLStatement As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents StatusList As DropDownList
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents blnUpdate As Label
    Protected WithEvents curStatus As Label
    Protected WithEvents lblDupMsg As Label
    Protected WithEvents ddlCropType As DropDownList
    Protected WithEvents ddlBlock As DropDownList
    Protected WithEvents srchYear As TextBox
    Protected WithEvents srchBlock As TextBox
    Protected WithEvents lblTitle As Label
    Protected WithEvents lblPeriodErr As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblOper As Label
    Protected WithEvents lblTotAmtFig As Label
    Protected WithEvents lblBudgeting As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents lblLocTag As Label
    Protected WithEvents lblBlkCode As Label

    Protected objBD As New agri.BD.clsSetup()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objGLSetup As New agri.GL.clsSetup()

    Dim strOppCd_GET As String = "BD_CLSSETUP_BLOCKTYPE_GET"
    Dim strOppCd_ADD As String = "BD_CLSSETUP_BLOCKTYPE_ADD"
    Dim strOppCd_UPD As String = "BD_CLSSETUP_BLOCKTYPE_UPD"
    Dim strOppCd_DEL As String = "BD_CLSSETUP_BLOCKTYPE_DEL"

    Dim objDataSet As New DataSet()
    Dim objLangCapDs As New DataSet()
    Dim intErrNo As Integer
    Dim strParam As String = ""

    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objLoc As New agri.Admin.clsLoc()
    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim intADAR As Integer
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strValidateCode As String
    Dim strvalidateDesc As String
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
                SortCol.Text = "ASC"
            End If
            If Not Page.IsPostBack Then
                BindSrchCropTypeList()
                BindGrid()
                BindPageList()

            End If
        End If
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblLocTag.text = lblBudgeting.text & GetCaption(objLangCap.EnumLangCap.Location)
        lblBlkCode.text = GetCaption(objLangCap.EnumLangCap.Block) & lblCode.text
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_BLOCKTYPE_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_TitleArea.aspx")
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



    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        BlockCrop.CurrentPageIndex = 0
        BlockCrop.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub BindGrid()
        Dim PageNo As Integer
        Dim Period As String

        BlockCrop.DataSource = LoadData()
        BlockCrop.DataBind()
        lblLocCode.Text = strLocation
        GetActivePeriod(Period)
        lblBgtPeriod.Text = Period
        PageNo = BlockCrop.CurrentPageIndex + 1
        lblTracker.Text = "Page " & PageNo & " of " & BlockCrop.PageCount

    End Sub

    Sub BindSrchCropTypeList()
        ddlCropType.Items.Add(New ListItem(objBD.mtdGetCropType(objBD.EnumCropType.Mature), objBD.EnumCropType.Mature))
        ddlCropType.Items.Add(New ListItem(objBD.mtdGetCropType(objBD.EnumCropType.UnMature), objBD.EnumCropType.UnMature))
        ddlCropType.Items.Add(New ListItem(objBD.mtdGetCropType(objBD.EnumCropType.All), objBD.EnumCropType.All))
        ddlCropType.SelectedIndex = 2
    End Sub


    Sub BindCropTypeList(ByRef DropList As DropDownList, Optional ByVal cropType As String = "")
        DropList.Items.Add(New ListItem(objBD.mtdGetCropType(objBD.EnumCropType.Mature), objBD.EnumCropType.Mature))
        DropList.Items.Add(New ListItem(objBD.mtdGetCropType(objBD.EnumCropType.UnMature), objBD.EnumCropType.UnMature))
        Select Case cropType.Trim
            Case objBD.EnumCropType.Mature
                DropList.SelectedIndex = 0
            Case objBD.EnumCropType.UnMature
                DropList.SelectedIndex = 1
        End Select

    End Sub

    Sub BindPageList()

        Dim count As Integer = 1
        Dim arrDList As New ArrayList()

        While Not count = BlockCrop.PageCount + 1
            arrDList.Add("Page " & count)
            count = count + 1
        End While
        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = BlockCrop.CurrentPageIndex

    End Sub

    Sub BindBlockDropList(ByRef DropList As DropDownList, Optional ByVal pv_strBlkCode As String = "")
        Dim dsForDropDown As DataSet
        Dim intSelectedIndex As Integer = 0
        Dim intCnt As Integer
        Dim strOpCdBlockList_Get As String
        Dim strParam As String
        Dim dr As DataRow

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
            strOpCdBlockList_Get = "BD_CLSSETUP_BLOCKTYPE_BLOCKLIST_GET"

            strParam = "||" & objGLSetup.EnumBlockStatus.Active & "||BlkCode|"

            Try
                intErrNo = objGLSetup.mtdGetBlock(strOpCdBlockList_Get, strLocation, strParam, dsForDropDown, False)

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GL_SETUP_BLOCK_LIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
            End Try
            For intCnt = 0 To dsForDropDown.Tables(0).Rows.Count - 1
                dsForDropDown.Tables(0).Rows(intCnt).Item("BlkCode") = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("BlkCode"))
                dsForDropDown.Tables(0).Rows(intCnt).Item("Description") = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("BlkCode")) & " (" & Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("Description")) & ")"
                If Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("BlkCode")) = Trim(pv_strBlkCode) Then
                    intSelectedIndex = intCnt + 1
                End If
            Next

            dr = dsForDropDown.Tables(0).NewRow()
            dr("BlkCode") = ""
            dr("Description") = "Select Block Code"

            dsForDropDown.Tables(0).Rows.InsertAt(dr, 0)
            DropList.DataSource = dsForDropDown.Tables(0)
            DropList.DataValueField = "BlkCode"
            DropList.DataTextField = "Description"
            DropList.DataBind()
            DropList.SelectedIndex = intSelectedIndex

        Else
            strOpCdBlockList_Get = "BD_CLSSETUP_BLOCKTYPE_SUBBLOCKLIST_GET"
            strParam = "||" & objGLSetup.EnumSubBlockStatus.Active & "||SubBlkCode|"

            Try
                intErrNo = objGLSetup.mtdGetSubBlock(strOpCdBlockList_Get, strLocation, strParam, dsForDropDown, False)

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GL_SETUP_SUBBLOCK_LIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
            End Try

            For intCnt = 0 To dsForDropDown.Tables(0).Rows.Count - 1
                dsForDropDown.Tables(0).Rows(intCnt).Item("SubBlkCode") = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("SubBlkCode"))
                dsForDropDown.Tables(0).Rows(intCnt).Item("Description") = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("SubBlkCode")) & " (" & Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("Description")) & ")"
                If dsForDropDown.Tables(0).Rows(intCnt).Item("SubBlkCode") = Trim(pv_strBlkCode) Then
                    intSelectedIndex = intCnt + 1
                End If
            Next

            dr = dsForDropDown.Tables(0).NewRow()
            dr("SubBlkCode") = ""
            dr("Description") = "Select Sub Block Code"

            dsForDropDown.Tables(0).Rows.InsertAt(dr, 0)
            DropList.DataSource = dsForDropDown.Tables(0)
            DropList.DataValueField = "SubBlkCode"
            DropList.DataTextField = "Description"
            DropList.DataBind()
            DropList.SelectedIndex = intSelectedIndex

        End If

        If Not dsForDropDown Is Nothing Then
            dsForDropDown = Nothing
        End If
    End Sub

    Protected Function LoadData() As DataSet

        strParam = srchBlock.Text & "|" & srchYear.Text & "|" & ddlCropType.SelectedItem.Value & "|||" & SortExpression.Text & " " & SortCol.Text
        Try
            intErrNo = objBD.mtdGetBlockType(strOppCd_GET, strParam, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BLOCKTYPE&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_Blocktype.aspx")
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
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_Blocktype.aspx")
        End Try
        If dsperiod.Tables(0).Rows.Count > 0 Then
            BGTPeriod = dsperiod.Tables(0).Rows(0).Item("BGTPeriod")
            Return dsperiod.Tables(0).Rows(0).Item("PeriodID")
        Else
            BGTPeriod = "No Active Period"
            Return ""
        End If
    End Function

    Sub btnPrevNext_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).CommandArgument
        Select Case direction
            Case "first"
                BlockCrop.CurrentPageIndex = 0
            Case "prev"
                BlockCrop.CurrentPageIndex = _
                    Math.Max(0, BlockCrop.CurrentPageIndex - 1)
            Case "next"
                BlockCrop.CurrentPageIndex = _
                    Math.Min(BlockCrop.PageCount - 1, BlockCrop.CurrentPageIndex + 1)
            Case "last"
                BlockCrop.CurrentPageIndex = BlockCrop.PageCount - 1
        End Select
        lstDropList.SelectedIndex = BlockCrop.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            BlockCrop.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        BlockCrop.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIf(SortCol.Text = "ASC", "DESC", "ASC")
        BlockCrop.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
    End Sub

    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim Droplist As DropDownList
        Dim Label As Label
        Dim strBlk As String
        Dim strtype As String

        lblOper.Text = objBD.EnumOperation.Update
        Label = E.Item.FindControl("lblCropType")
        strtype = Label.Text
        BlockCrop.EditItemIndex = CInt(E.Item.ItemIndex)
        BindGrid()

        Droplist = BlockCrop.Items.Item(CInt(BlockCrop.EditItemIndex)).FindControl("ddlCrop")
        BindCropTypeList(Droplist, strtype)
        Droplist = BlockCrop.Items.Item(CInt(BlockCrop.EditItemIndex)).FindControl("ddlBlock")
        Droplist.Visible = False
        Label = BlockCrop.Items.Item(CInt(BlockCrop.EditItemIndex)).FindControl("lblBlkCode")
        Label.Visible = True

    End Sub

    Sub DEDR_Update(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim list As DropDownList
        Dim lbl As Label
        Dim intError As Integer
        Dim dsBlock As New DataSet()

        Dim strCroptype As String
        Dim strOpCd_Block_Get As String
        Dim strBlkCode As String
        Dim dtPlantYear As DateTime

        If lblOper.Text = objBD.EnumOperation.Update Then
            lbl = E.Item.FindControl("lblBlkCode")
            strBlkCode = lbl.Text & "|||"
        Else
            list = E.Item.FindControl("ddlBlock")
            strBlkCode = list.SelectedItem.Value & "|||"
        End If


        list = E.Item.FindControl("ddlCrop")
        strCroptype = list.SelectedItem.Value

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
            strOpCd_Block_Get = "GL_CLSSETUP_BLOCK_GET_BY_BLKCODE"

            Try
                intErrNo = objGLSetup.mtdGetBlock(strOpCd_Block_Get, _
                                                  strLocation, _
                                                  strBlkCode, _
                                                  dsBlock, _
                                                  True)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BD_UPDATEBLOCKTYPE_GETBLOCKDETAILS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_Blocktype.aspx")
            End Try

        Else
            strOpCd_Block_Get = "GL_CLSSETUP_BLOCK_GET_BY_SUBBLKCODE"

            Try
                intErrNo = objGLSetup.mtdGetSubBlock(strOpCd_Block_Get, _
                                                     strLocation, _
                                                     strBlkCode, _
                                                     dsBlock, _
                                                     True)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BD_UPDATEBLOCKTYPE_GETSUBBLOCKDETAILS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_Blocktype.aspx")
            End Try

        End If

        dtPlantYear = dsBlock.Tables(0).Rows(0).Item("PlantingDate")

        strParam = strBlkCode & "|" & _
                    dtPlantYear.Year & "|" & _
                    strCroptype & "|"


        Try
            intErrNo = objBD.mtdUpdBlocktype(strOppCd_ADD, _
                                                strOppCd_UPD, _
                                                strOppCd_GET, _
                                                strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strParam, _
                                                lblOper.Text, _
                                                intError)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=UPD_BLOCKTYPE&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_Blocktype.aspx")
        End Try

        BlockCrop.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        If CInt(E.Item.ItemIndex) = 0 And BlockCrop.Items.Count = 1 And BlockCrop.PageCount <> 1 Then
            BlockCrop.CurrentPageIndex = BlockCrop.PageCount - 2
        End If
        BlockCrop.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)

        Dim strBlkCode As String
        Dim intError As Integer
        Dim Label As Label


        Label = E.Item.FindControl("lblBlkCode")
        strBlkCode = Label.Text


        Try
            intErrNo = objBD.mtdDelBlocktype(strOppCd_DEL, _
                                                strBlkCode, _
                                                intError)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=DEL_BLOCKTYPE&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_Blocktype.aspx")
        End Try
        BlockCrop.EditItemIndex = -1
        BindGrid()

    End Sub

    Sub DEDR_Add(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim dataSet As dataSet = LoadData()
        Dim newRow As DataRow
        Dim Updbutton As LinkButton
        Dim Droplist As DropDownList
        Dim lbl As Label


        blnUpdate.Text = False
        newRow = dataSet.Tables(0).NewRow()
        newRow.Item("BlkCode") = ""
        newRow.Item("PlantingYear") = ""
        newRow.Item("CropType") = 1
        newRow.Item("Status") = 1
        newRow.Item("CreateDate") = DateTime.Now()
        newRow.Item("UpdateDate") = DateTime.Now()
        newRow.Item("UserName") = ""
        dataSet.Tables(0).Rows.Add(newRow)

        BlockCrop.DataSource = dataSet
        BlockCrop.DataBind()

        BlockCrop.CurrentPageIndex = BlockCrop.PageCount - 1
        BlockCrop.EditItemIndex = BlockCrop.Items.Count - 1
        BlockCrop.DataBind()
        lblOper.Text = objBD.EnumOperation.Add

        Droplist = BlockCrop.Items.Item(CInt(BlockCrop.EditItemIndex)).FindControl("ddlCrop")
        BindCropTypeList(Droplist)
        Droplist = BlockCrop.Items.Item(CInt(BlockCrop.EditItemIndex)).FindControl("ddlBlock")
        BindBlockDropList(Droplist)
        Updbutton = BlockCrop.Items.Item(CInt(BlockCrop.EditItemIndex)).FindControl("Delete")
        Updbutton.Visible = False

    End Sub


End Class
