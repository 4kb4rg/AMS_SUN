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


Public Class BD_ManuringFertUsg_List : Inherits Page

    Protected WithEvents dgCode As DataGrid
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblLocTag As Label
    Protected WithEvents lblPeriodErrTop As Label
    Protected WithEvents lblPeriodErr As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblYearPlanted As Label
    Protected WithEvents lblCode As Label

    Dim objBD As New agri.BD.clsTrx()
    Dim objBDSetup As New agri.BD.clsSetup()
    Dim objGLSetup As New agri.GL.clsSetup()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Dim objDataSet As DataSet
    Dim objLangCapDs As New DataSet()
    Dim intErrNo As Integer
    Dim strParam As String = ""

    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
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
            If Not Page.IsPostBack Then
                lblCode.Text = Request.QueryString("code")
                lblYearPlanted.Text = Request.QueryString("yr")
                BindGrid()
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BD_TRX_MANURINGFERTUSG_LIST_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_ManuringFertUsg_List.aspx?code=" & lblCode.Text)
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

        dgCode.DataSource = LoadData()
        dgCode.DataBind()
        lblLocCode.Text = strLocation
        GetActivePeriod(Period)
        lblBgtPeriod.Text = Period

    End Sub

    Protected Function LoadData() As DataSet

        Dim strOpCd_ManuringFertUsg_Item_Get As String = "BD_CLSTRX_MANURINGFERTUSG_ITEM_GET"

        strParam = lblYearPlanted.Text.Trim & "|||" & strLocation & "|" & GetActivePeriod("") & "||"
        Try
            intErrNo = objBD.mtdGetManuringFertUsg(strOpCd_ManuringFertUsg_Item_Get, _
                                                   strParam, _
                                                   objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_MANURING_FERTUSG_YEAR_CLICK&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_ManuringFertUsg_List.aspx?code=" & lblCode.Text)
        End Try
        Return objDataSet
    End Function

    Protected Function GetActivePeriod(ByRef BGTPeriod As String) As String
        Dim strOppCd_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"
        Dim dsperiod As New DataSet()

        strParam = "|||||" & objBDSetup.EnumPeriodStatus.Active & "|" & strLocation & "|"

        Try
            intErrNo = objBDSetup.mtdGetPeriodList(strOppCd_GET, strParam, dsperiod)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_ManuringFertUsg_List.aspx?code=" & lblCode.Text)
        End Try
        If dsperiod.Tables(0).Rows.Count > 0 Then
            BGTPeriod = dsperiod.Tables(0).Rows(0).Item("BGTPeriod")
            Return dsperiod.Tables(0).Rows(0).Item("PeriodID")
        Else
            BGTPeriod = "No Active Period"
            Response.Redirect("../../BD/Setup/BD_setup_Periods.aspx")
        End If
    End Function

    Sub btnFert_Click(ByVal sender As Object, ByVal e As EventArgs)

        Dim strFert As String = CType(sender, LinkButton).CommandArgument
        Dim strOpCd_ManuringFertUsg_Add As String = "BD_CLSTRX_MANURINGFERTUSG_ADD"
        Dim strOpCd_ManuringFertUsg_GET As String = "BD_CLSTRX_MANURINGFERTUSG_GET"
        Dim strOpCd_ManuringBlk_PlantArea_SUM As String '= "BD_CLSTRX_MANURINGFERTUSG_SUM"
        Dim strOpCd_ManuringFertUsgLn_ADD As String = "BD_CLSTRX_MANURINGFERTUSGLN_ADD"
        Dim strBgt_Period_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"
        Dim strCropType As String
        Dim strBlkStatus As String
        Dim intError As Integer

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
            strCropType = objGLSetup.EnumBlockType.MatureField
            strBlkStatus = objGLSetup.EnumBlockStatus.Active
            strOpCd_ManuringBlk_PlantArea_SUM = "BD_CLSTRX_MANURINGFERTUSG_BLKTOTALAREA_SUM"
        Else
            strCropType = objGLSetup.EnumSubBlockType.MatureField
            strBlkStatus = objGLSetup.EnumSubBlockStatus.Active
            strOpCd_ManuringBlk_PlantArea_SUM = "BD_CLSTRX_MANURINGFERTUSG_SUBBLKTOTALAREA_SUM"
        End If

        strParam = strFert.Trim & "|" & GetActivePeriod("") & "|" & lblYearPlanted.Text & "|" & strCropType.Trim & "|" & strBlkStatus.Trim
        Try
            intErrNo = objBD.mtdAddManuringFertUsg(strOpCd_ManuringFertUsg_Add, _
                                                   strOpCd_ManuringFertUsgLn_ADD, _
                                                   strOpCd_ManuringFertUsg_GET, _
                                                   strOpCd_ManuringBlk_PlantArea_SUM, _
                                                   strBgt_Period_GET, _
                                                   strCompany, _
                                                   strLocation, _
                                                   strUserId, _
                                                   strParam, _
                                                   intError, _
                                                   objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.ManuringUsg), _
                                                   objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.ManuringUsgLn))

        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=FERT_CLICK&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_ManuringFertUsg_List.aspx?code=" & lblCode.Text)
        End Try

        If intError = objBD.EnumErrorType.NoActivePeriod Then
            lblPeriodErrTop.Visible = True
            lblPeriodErr.Visible = True
        End If

        Response.Redirect("../../BD/Trx/BD_Trx_ManuringFertUsg_Details.aspx?fert=" & strFert.Trim & "&yr=" & lblYearPlanted.Text)
    End Sub

    Sub BackBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("BD_trx_ManuringFertUsg_YearList.aspx")
    End Sub


End Class
