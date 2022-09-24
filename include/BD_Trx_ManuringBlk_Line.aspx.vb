
Imports System
Imports System.Data
Imports System.Collections 
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.Page
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic


Public Class BD_ManuringBlk_Line : Inherits Page

    Protected WithEvents dgManBlkLine As DataGrid
    Protected WithEvents lblCode As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblErrMessage2 As Label
    Protected WithEvents lblBlock As Label
    Protected WithEvents lblSubBlk As Label
    Protected WithEvents lblOper As Label
    Protected WithEvents hidBlkCode As HtmlInputHidden
    Protected WithEvents hidSPH As HtmlInputHidden
    Protected WithEvents hidPlantedArea As HtmlInputHidden

    Protected WithEvents lblTitle As Label
    Protected WithEvents lblLocTag As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblYearPlanted As Label
    Protected WithEvents lblBlkTag As Label
    Protected WithEvents lblBlkCode As Label
    Protected WithEvents lblSelPer As Label

    Protected WithEvents lblManuringBlkLnID As Label
    Protected WithEvents lstFert As DropDownList
    Protected WithEvents txtRates As TextBox
    Protected WithEvents lblQty As Label
    Protected WithEvents lblTotQty As Label
    Protected WithEvents lblBgtStatus As Label 
    Protected WithEvents tblAdd As HtmlTable  

    Dim objBD As New agri.BD.clsTrx()
    Dim objBDSetup As New agri.BD.clsSetup()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objAdminLoc As New agri.Admin.clsLoc()
    

    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()

    Dim objDsManBlkLn As New DataSet()
    Dim objLangCapDs As New DataSet()
    Dim dsperiod As New DataSet()
    Dim objDsFertItemCode As New DataSet()
    Dim intErrNo As Integer
    Dim strParam As String = ""
    Dim intPeriod As Integer
    Dim arr As Array
    Dim strManBlkID As String
    Dim strSelectedBlkCode As String
    Dim intError As Integer

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim intADAR As Integer
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim intConfigsetting As Integer

    Dim strOppCd_ManBlk_AccPeriod_ADD As String = "BD_CLSTRX_MANURINGACCPER_ADD"
    Dim strOppCd_ManBlkLn_ADD As String = "BD_CLSTRX_MANURINGBLKLN_ADD"
    Dim strOppCd_ManBlk_UPD As String = "BD_CLSTRX_MANURINGBLK_UPD"
    Dim strOppCd_ManBlk_AccPeriod_DEL As String = "BD_CLSTRX_MANURINGACCPER_DEL"
    Dim strOppCd_ManBlkLn_DEL As String = "BD_CLSTRX_MANURINGBLKLN_DEL"
    Dim strOppCd_ManBlkLn_SUM As String = "BD_CLSTRX_MANURINGBLKLN_SUM"
    Dim strOppCd_ManBlkLn_UPD As String = "BD_CLSTRX_MANURINGBLKLN_UPD"
    Dim strOppCd_ManBlk_GET As String

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
            arr = Split(Request.QueryString("period"), "/")
            strSelectedBlkCode = Trim(IIf(Request.QueryString("blkcode") <> "", Request.QueryString("blkcode"), Request.Form("blkcode")))
            hidSPH.Value = Request.QueryString("sph")
            hidPlantedArea.Value = Request.QueryString("plantedarea")

            If Not IsPostBack Then
                If strSelectedBlkCode <> "" Then
                    hidBlkCode.Value = strSelectedBlkCode
                    onLoad_Display()
                    onLoad_LineDisplay()
                Else
                End If

                BindGrid()
                If lblBgtStatus.Text.Trim = objBDSetup.EnumPeriodStatus.Addvote Then
                    tblAdd.Visible = False
                End If
            End If
        End If
        
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()

        lblLocTag.Text = GetCaption(objLangCap.EnumLangCap.Location) & lblCode.text
        lblBlock.Text = GetCaption(objLangCap.EnumLangCap.Block)
        lblSubBlk.Text = GetCaption(objLangCap.EnumLangCap.SubBlock)

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
            lblTitle.Text = "DETAILED MANURING SCHEDULE BY " & " " & UCase(lblBlock.Text)
            lblBlkTag.Text = lblBlock.Text & lblCode.Text
        Else
            lblTitle.Text = "DETAILED MANURING SCHEDULE BY " & " " & UCase(lblSubBlk.Text)
            lblBlkTag.Text = lblSubBlk.Text & lblCode.Text
        End If

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
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_MANURINGBLK_LINE_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_ManuringBlk_Details.aspx?yr=" & lblYearPlanted.Text)
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


    Sub BindGrid()
        Dim Period As String
        dgManBlkLine.DataSource = onLoad_LineDisplay()
        dgManBlkLine.DataBind()
        lblLocCode.Text = strLocation
        GetActivePeriod(Period)
        lblBgtPeriod.Text = Period

    End Sub

    Sub BindFertItemCode(ByVal pv_strFertItemCode As String)
        Dim strOppCd_FertItemCode_GET As String = "BD_CLSSETUP_MANURINGFERT_GET"
        Dim intCnt As Integer
        Dim intError As Integer

        strParam = "|||" & objBDSetup.EnumManuringFertStatus.Active & "|" & "MS.FertItemCode" & "|" & "AND MS.FertItemCode NOT IN ('" & pv_strFertItemCode & "')"
        Try
            intErrNo = objBDSetup.mtdGetManuringFert(strOppCd_FertItemCode_GET, _
                                                     strParam, _
                                                     objDsFertItemCode)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_MANURINGFERT_ITEMCODE&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_ManuringBlk_Details.aspx?yr=" & lblYearPlanted.Text)
        End Try

        For intCnt = 0 To objDsFertItemCode.Tables(0).Rows.Count - 1
            objDsFertItemCode.Tables(0).Rows(intCnt).Item("FertItemCode") = Trim(objDsFertItemCode.Tables(0).Rows(intCnt).Item("FertItemCode"))
            objDsFertItemCode.Tables(0).Rows(intCnt).Item("Description") = Trim(objDsFertItemCode.Tables(0).Rows(intCnt).Item("FertItemCode")) & " ( " _
                                                                & Trim(objDsFertItemCode.Tables(0).Rows(intCnt).Item("Description")) & " )"

        Next intCnt

        Dim drinsert As DataRow
        drinsert = objDsFertItemCode.Tables(0).NewRow()
        drinsert("FertItemCode") = ""
        drinsert("Description") = "Select item"
        objDsFertItemCode.Tables(0).Rows.InsertAt(drinsert, 0)

        lstFert.DataSource = objDsFertItemCode.Tables(0)
        lstFert.DataValueField = "FertItemCode"
        lstFert.DataTextField = "Description"
        lstFert.DataBind()
    End Sub

    Protected Function GetActivePeriod(ByRef BGTPeriod As String) As String
        Dim strOppCd_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"

        strParam = "|||||" & objBDSetup.EnumPeriodStatus.Active & "|" & strLocation & "|"
        Try
            intErrNo = objBDSetup.mtdGetPeriodList(strOppCd_GET, strParam, dsperiod)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_ManuringBlk_Line.aspx")
        End Try

        If dsperiod.Tables(0).Rows.Count > 0 Then
            BGTPeriod = dsperiod.Tables(0).Rows(0).Item("BGTPeriod") & " - (" & objBDSetup.mtdGetPeriodStatus(dsperiod.Tables(0).Rows(0).Item("Status")) & ")"
            lblBgtStatus.Text = dsperiod.Tables(0).Rows(0).Item("Status")
            intPeriod = dsperiod.Tables(0).Rows.Count - 1
            Return dsperiod.Tables(0).Rows(0).Item("PeriodID")
        Else
            BGTPeriod = "No Active Period"
            Response.Redirect("../../BD/Setup/BD_setup_Periods.aspx")
        End If
    End Function

    Sub DataGrid_ItemDataBound(ByVal Sender As Object, ByVal e As DataGridItemEventArgs)
        Dim lbl As Label
        Dim btn As LinkButton
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            If lblBgtStatus.Text.Trim = objBDSetup.EnumPeriodStatus.Addvote Then
                btn = e.Item.FindControl("Edit")
                btn.Visible = False
            End If
        End If
    End Sub

    Sub onLoad_Display()

        lblYearPlanted.Text = Request.QueryString("yr")
        strManBlkID = Request.QueryString("manblkid")

        lblBlkCode.Text = strSelectedBlkCode
        lblSelPer.Text = Request.QueryString("period")

    End Sub

    Protected Function onLoad_LineDisplay() As DataSet
        Dim strOpCd As String = "BD_CLSTRX_MANURINGBLKLN_GET"
        Dim intCnt As Integer
        Dim strFertItemCode As String

        strParam = strLocation & "|" & strSelectedBlkCode & "||" & Trim(arr(0)) & "|" & Trim(arr(1)) & "||"
        Try
            intErrNo = objBD.mtdGetManuringBlkLn(strOpCd, _
                                                 strParam, _
                                                 objDsManBlkLn)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_MANURINGBLKLN_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_ManuringBlk_Details.aspx?yr=" & lblYearPlanted.Text)
        End Try

        If objDsManBlkLn.Tables(0).Rows.Count > 0 Then
            LoadManuringBlkLnTotal()
            For intCnt = 0 To objDsManBlkLn.Tables(0).Rows.Count - 1
                strFertItemCode = Trim(objDsManBlkLn.Tables(0).Rows(intCnt).Item("FertItemCode")) & "','" & strFertItemCode
            Next
            strFertItemCode = strFertItemCode.Substring(0, strFertItemCode.Length - 3)
        Else
            strFertItemCode = ""
        End If

        dgManBlkLine.DataSource = objDsManBlkLn.Tables(0)
        dgManBlkLine.DataBind()

        BindFertItemCode(strFertItemCode)
        Return objDsManBlkLn
    End Function

    Sub LoadManuringBlkLnTotal()
        Dim strOppCd_ManBlkLn_SUM As String = "BD_CLSTRX_MANURINGBLKLN_SUM"

        Dim dsTotal As DataSet

        strParam = strLocation & "||" & _
                   Request.QueryString("manblkid") & "|" & _
                   Trim(arr(0)) & "|" & Trim(arr(1)) & "|" & _
                   "AND MB.PeriodID = " & GetActivePeriod("") & "|"
        Try
            intErrNo = objBD.mtdGetManuringBlkLn(strOppCd_ManBlkLn_SUM, strParam, dsTotal)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_MANURINGBLK_TOTAL&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_ManuringBlk_Details.aspx?yr=" & lblYearPlanted.Text)
        End Try


        lblTotQty.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(dsTotal.Tables(0).Rows(0).Item("TotalQty"), 5),5)

    End Sub

    Sub btnAdd_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        
        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
            strOppCd_ManBlk_GET = "BD_CLSTRX_MANURINGBLK_GET"
        Else
            strOppCd_ManBlk_GET = "BD_CLSTRX_MANURINGSUBBLK_GET"
        End If

        lblOper.Text = objBDSetup.EnumOperation.Add
        strParam = "|" & Trim(arr(0)) & "|" & Trim(arr(1)) & "|" & _
                   Trim(lstFert.SelectedItem.Value) & "|" & Trim(txtRates.Text) & "|" & _
                   lblBlkCode.Text & "|" & objBD.EnumManuringBlkStatus.Budgeted & "|" & Request.QueryString("manblkid")
        Try
            intErrNo = objBD.mtdUpdManuringBlkLine(strOppCd_ManBlk_AccPeriod_ADD, _
                                                    strOppCd_ManBlkLn_ADD, _
                                                    strOppCd_ManBlk_UPD, _
                                                    strOppCd_ManBlk_GET, _
                                                    strOppCd_ManBlkLn_SUM, _
                                                    strOppCd_ManBlkLn_UPD, _
                                                    strOppCd_ManBlk_AccPeriod_DEL, _
                                                    strOppCd_ManBlkLn_DEL, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strParam, _
                                                    lblOper.Text, _
                                                    intError, _
                                                    objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.ManuringBlkLn))
        if interrno<>0 then
            lblerrmessage2.visible=true
        else
            lblerrmessage2.visible=false
        end if
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_MANURINGBLK_LINE_ADD&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_ManuringBlk_Details.aspx?yr=" & lblYearPlanted.Text)
        End Try

        onLoad_Display()
        BindGrid()

    End Sub

    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim btn As LinkButton



        dgManBlkLine.EditItemIndex = CInt(E.Item.ItemIndex)
        BindGrid()

        btn = dgManBlkLine.Items.Item(CInt(E.Item.ItemIndex)).FindControl("Delete")
        btn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"

    End Sub

    Sub DEDR_Update(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim label As label
        Dim EditText As TextBox
        Dim strManBlkLnID As String
        Dim decRate As Decimal

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
            strOppCd_ManBlk_GET = "BD_CLSTRX_MANURINGBLK_GET"
        Else
            strOppCd_ManBlk_GET = "BD_CLSTRX_MANURINGSUBBLK_GET"
        End If

        lblOper.Text = objBDSetup.EnumOperation.Update
        label = dgManBlkLine.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lblManuringBlkLnID")
        strManBlkLnID = Trim(label.Text)
        EditText = dgManBlkLine.Items.Item(CInt(E.Item.ItemIndex)).FindControl("txtRates")
        decRate = Trim(EditText.Text)

        strParam = strManBlkLnID & "|" & Trim(arr(0)) & "|" & Trim(arr(1)) & "||" & decRate & "|" & lblBlkCode.Text & "||" & Request.QueryString("manblkid")
        Try
            intErrNo = objBD.mtdUpdManuringBlkLine(strOppCd_ManBlk_AccPeriod_ADD, _
                                                   strOppCd_ManBlkLn_ADD, _
                                                   strOppCd_ManBlk_UPD, _
                                                   strOppCd_ManBlk_GET, _
                                                   strOppCd_ManBlkLn_SUM, _
                                                   strOppCd_ManBlkLn_UPD, _
                                                   strOppCd_ManBlk_AccPeriod_DEL, _
                                                   strOppCd_ManBlkLn_DEL, _
                                                   strCompany, _
                                                   strLocation, _
                                                   strUserId, _
                                                   strParam, _
                                                   lblOper.Text, _
                                                   intError, _
                                                   "")
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_MANURINGBLK_LINE_UPD&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_ManuringBlk_Details.aspx?yr=" & lblYearPlanted.Text)
        End Try

        dgManBlkLine.EditItemIndex = -1
        onLoad_Display()
        onLoad_LineDisplay()
        BindGrid()


    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)

        Dim strManBlkLnID As String
        Dim Label As Label
        Dim intError As Integer

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
            strOppCd_ManBlk_GET = "BD_CLSTRX_MANURINGBLK_GET"
        Else
            strOppCd_ManBlk_GET = "BD_CLSTRX_MANURINGSUBBLK_GET"
        End If

        lblOper.Text = objBDSetup.EnumOperation.Delete
        Label = dgManBlkLine.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lblManuringBlkLnID")
        strManBlkLnID = Trim(Label.Text)

        strParam = strManBlkLnID & "|" & Trim(arr(0)) & "|" & Trim(arr(1)) & "|||" & lblBlkCode.Text & "||" & Request.QueryString("manblkid")
        Try
            intErrNo = objBD.mtdUpdManuringBlkLine(strOppCd_ManBlk_AccPeriod_ADD, _
                                                    strOppCd_ManBlkLn_ADD, _
                                                    strOppCd_ManBlk_UPD, _
                                                    strOppCd_ManBlk_GET, _
                                                    strOppCd_ManBlkLn_SUM, _
                                                    strOppCd_ManBlkLn_UPD, _
                                                    strOppCd_ManBlk_AccPeriod_DEL, _
                                                    strOppCd_ManBlkLn_DEL, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strParam, _
                                                    lblOper.Text, _
                                                    intError, _
                                                    "")
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_MANURINGBLK_LINE_DEL&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_ManuringBlk_Details.aspx?yr=" & lblYearPlanted.Text)
        End Try

        dgManBlkLine.EditItemIndex = -1
        onLoad_Display()
        onLoad_LineDisplay()
        BindGrid()

    End Sub

    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        dgManBlkLine.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub BackBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("BD_trx_ManuringBlk_Details.aspx?yr=" & lblYearPlanted.Text)
    End Sub
 

End Class

