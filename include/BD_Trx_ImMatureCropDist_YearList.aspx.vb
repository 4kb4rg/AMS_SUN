
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

Public Class BD_ImMatureCropDist_Year : Inherits Page

    Protected WithEvents BlockList As DataGrid
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents lblTitle As Label
    Protected WithEvents lblPeriodErr As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblTotAmtFig As Label
    Protected WithEvents BlockTag As Label
    Protected WithEvents lblLocTag As Label
    Protected WithEvents lblNoOf As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents lblBgtStatus As Label 

    Dim objBDTrx As New agri.BD.clsTrx()
    Dim objBDSetup As New agri.BD.clsSetup()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Dim strOppCd_Period_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"

    Dim objDataSet As DataSet
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
                SortExpression.Text = "OriBlkCode"
                SortCol.Text = "ASC"
            End If

            If Not Page.IsPostBack Then
                BindGrid()
            End If
        End If
        lblPeriodErr.Visible = False
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()

        lblTitle.Text = "IMMATURE CROP ACTIVITY CALENDERISATION"
        lblLocTag.Text = GetCaption(objLangCap.EnumLangCap.Location) & lblCode.Text
        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
            BlockTag.Text = GetCaption(objLangCap.EnumLangCap.Block) & lblCode.Text
            BlockList.Columns(0).Visible = False
            BlockList.Columns(1).HeaderText = BlockTag.Text
        Else
            BlockTag.Text = GetCaption(objLangCap.EnumLangCap.SubBlock) & lblCode.Text
            BlockList.Columns(0).HeaderText = GetCaption(objLangCap.EnumLangCap.Block) & lblCode.Text
            BlockList.Columns(1).HeaderText = BlockTag.Text
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BD_TRX_IMMATURECROPDIST_BlockList_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_ImMatureCropDist_YearList.aspx")
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

        BlockList.DataSource = LoadData()
        BlockList.DataBind()
        lblLocCode.Text = strLocation
        GetActivePeriod(Period)
        lblBgtPeriod.Text = Period
        If lblBgtStatus.Text.Trim = objBDSetup.EnumPeriodStatus.Addvote Then 
            BlockList.Columns(0).Visible = False
        End If

    End Sub

    Protected Function LoadData() As DataSet
        Dim strOppCd_YearPlanted_GET As String

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
            strOppCd_YearPlanted_GET = "BD_CLSTRX_UNMATURECROPDIST_YEARPLANTED_BLK_GET"
        Else
            strOppCd_YearPlanted_GET = "BD_CLSTRX_UNMATURECROPDIST_YEARPLANTED_SBLK_GET"
        End If

        strParam = "|" & strLocation & "|" & GetActivePeriod("") & "|||"
        Try
            intErrNo = objBDTrx.mtdGetUnMatureCrop(strOppCd_YearPlanted_GET, strParam, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_IMMATURECROPDIST_BlockList_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_ImMatureCropDist_YearList.aspx")
        End Try

        Return objDataSet
    End Function

    Protected Function GetActivePeriod(ByRef BGTPeriod As String) As String
        Dim dsperiod As New DataSet()

        strParam = "|||||" & objBDSetup.EnumPeriodStatus.Active & "|" & strLocation & "|"
        Try
            intErrNo = objBDSetup.mtdGetPeriodList(strOppCd_Period_GET, strParam, dsperiod)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_IMMATURECROPDIST_GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_ImMatureCropDist_YearList.aspx")
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

    Sub btnBlockYear_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim stryear As String = CType(sender, LinkButton).CommandArgument
        Dim strBlkCode As String = CType(sender, LinkButton).Text
        Dim strSubBlkCode As String = CType(sender, LinkButton).CommandName

        Dim strOppCd_UnMatureCropDist_GET As String = "BD_CLSTRX_UNMATURECROPDIST_GET"
        Dim strOppCd_UnMatureCropDist_ADD As String = "BD_CLSTRX_UNMATURECROPDIST_ADD"
        Dim strOppCd_UnMatureCropSetup_AccCode_GET As String = "BD_CLSTRX_UNMATURECROPSETUP_ACCCODE_GET"
        Dim strOppCd_UnMatureCropDist_YrBudgetCost_SUM As String = "BD_CLSTRX_UNMATURECROPDIST_YRBUDGETCOST_SUM"
        Dim strOppCd_UnMatureCropDist_AccPeriod_ADD As String = "BD_CLSTRX_UNMATURECROPDIST_ACCPERIOD_ADD"
        Dim strOppCd_UnMatureCropDist_AccPeriod_UPD As String = "BD_CLSTRX_UNMATURECROPDIST_ACCPERIOD_UPD"
        Dim strOppCd_UnMatureCropDist_UPD As String = "BD_CLSTRX_UNMATURECROPDIST_UPD"
        Dim strOppCd_SubBlock_Get = "BD_CLSTRX_UNMATURECROP_DISTBYBLOCK_SUBBLOCK_GET"
        Dim intError As Integer
        Dim strParam As String

        strParam = GetActivePeriod("") & "|" & stryear.Trim & "|" & strSubBlkCode.Trim & "|" & strBlkCode.Trim & "|"
        Try
            intErrNo = objBDTrx.mtdAddUnMatureCropDist(strOppCd_UnMatureCropDist_GET, _
                                                       strOppCd_UnMatureCropDist_ADD, _
                                                       strOppCd_UnMatureCropSetup_AccCode_GET, _
                                                       strOppCd_UnMatureCropDist_YrBudgetCost_SUM, _
                                                       strOppCd_UnMatureCropDist_AccPeriod_ADD, _
                                                       strOppCd_UnMatureCropDist_AccPeriod_UPD, _
                                                       strOppCd_UnMatureCropDist_UPD, _
                                                       strOppCd_Period_GET, _
                                                       strOppCd_SubBlock_Get, _
                                                       strCompany, _
                                                       strLocation, _
                                                       strUserId, _
                                                       strParam, _
                                                       intError, _
                                                       True)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_IMMATURECROPDIST_BLK_ADD&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_ImMatureCropDist_YearList.aspx")
        End Try

        If intError = objBDTrx.EnumErrorType.NoActivePeriod Then
            lblPeriodErr.Visible = True
        Else
            Response.Redirect("../../BD/Trx/BD_Trx_ImMatureCropDist_Details.aspx?distbyblk=true&blkcode=" & strBlkCode & "&subblkcode=" & strSubBlkCode.Trim & "&yr=" & stryear.Trim)

        End If
    End Sub


    Sub btnSubBlockYear_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim stryear As String = CType(sender, LinkButton).CommandArgument
        Dim strBlkCode As String = CType(sender, LinkButton).CommandName
        Dim strSubBlkCode As String = CType(sender, LinkButton).Text

        Dim strOppCd_UnMatureCropDist_GET As String = "BD_CLSTRX_UNMATURECROPDIST_GET"
        Dim strOppCd_UnMatureCropDist_ADD As String = "BD_CLSTRX_UNMATURECROPDIST_ADD"
        Dim strOppCd_UnMatureCropSetup_AccCode_GET As String = "BD_CLSTRX_UNMATURECROPSETUP_ACCCODE_GET"
        Dim strOppCd_UnMatureCropDist_YrBudgetCost_SUM As String = "BD_CLSTRX_UNMATURECROPDIST_YRBUDGETCOST_SUM"
        Dim strOppCd_UnMatureCropDist_AccPeriod_ADD As String = "BD_CLSTRX_UNMATURECROPDIST_ACCPERIOD_ADD"
        Dim strOppCd_UnMatureCropDist_AccPeriod_UPD As String = "BD_CLSTRX_UNMATURECROPDIST_ACCPERIOD_UPD"
        Dim strOppCd_UnMatureCropDist_UPD As String = "BD_CLSTRX_UNMATURECROPDIST_UPD"
        Dim strOppCd_SubBlock_Get As String = "BD_CLSTRX_CROPPROD_SUBBLOCK_GET"
        Dim intError As Integer
        Dim strParam As String

        strParam = GetActivePeriod("") & "|" & stryear.Trim & "|" & strSubBlkCode.Trim
        Try
            intErrNo = objBDTrx.mtdAddUnMatureCropDist(strOppCd_UnMatureCropDist_GET, _
                                                       strOppCd_UnMatureCropDist_ADD, _
                                                       strOppCd_UnMatureCropSetup_AccCode_GET, _
                                                       strOppCd_UnMatureCropDist_YrBudgetCost_SUM, _
                                                       strOppCd_UnMatureCropDist_AccPeriod_ADD, _
                                                       strOppCd_UnMatureCropDist_AccPeriod_UPD, _
                                                       strOppCd_UnMatureCropDist_UPD, _
                                                       strOppCd_Period_GET, _
                                                       strOppCd_SubBlock_Get, _
                                                       strCompany, _
                                                       strLocation, _
                                                       strUserId, _
                                                       strParam, _
                                                       intError, _
                                                       False)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_IMMATURECROPDIST_SBLK_ADD&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_ImMatureCropDist_YearList.aspx")
        End Try

        If intError = objBDTrx.EnumErrorType.NoActivePeriod Then
            lblPeriodErr.Visible = True
        Else
            Response.Redirect("../../BD/Trx/BD_Trx_ImMatureCropDist_Details.aspx?distbyblk=false&blkcode=" & strBlkCode & "&subblkcode=" & strSubBlkCode.Trim & "&yr=" & stryear.Trim)
        End If
    End Sub

End Class
