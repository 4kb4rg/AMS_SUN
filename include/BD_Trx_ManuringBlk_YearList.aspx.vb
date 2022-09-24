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

Public Class BD_ManuringBlk_Year : Inherits Page

    Protected WithEvents YearList As DataGrid
    Protected WithEvents lblCode As Label
    Protected WithEvents lblPeriodErrTop As Label
    Protected WithEvents lblPeriodErr As Label
    Protected WithEvents lblBlockErrTop As Label
    Protected WithEvents lblBlockErr As Label
    Protected WithEvents lblTracker As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblTitle As Label
    Protected WithEvents lblLocTag As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBgtPeriod As Label

    Dim objBD As New agri.BD.clsTrx()
    Dim objBDSetup As New agri.BD.clsSetup()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objGLSetup As New agri.GL.clsSetup()
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Dim strOppCd_GET As String

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
            If Not Page.IsPostBack Then
                BindGrid()
            End If
        End If
        lblPeriodErrTop.Visible = False
        lblPeriodErr.Visible = False
        lblBlockErrTop.Visible = False
        lblBlockErr.Visible = False
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblLocTag.Text = GetCaption(objLangCap.EnumLangCap.Location) & lblCode.Text

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
            YearList.Columns(1).HeaderText = "No. of " & GetCaption(objLangCap.EnumLangCap.Block)
        Else
            YearList.Columns(1).HeaderText = "No. of " & GetCaption(objLangCap.EnumLangCap.SubBlock)
        End If
        lblBlockErrTop.Text = "Please check " & GetCaption(objLangCap.EnumLangCap.Block) & " Info."
        lblBlockErr.Text = "Please check " & GetCaption(objLangCap.EnumLangCap.Block) & " Info."
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
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_MANURINGBLK_YEARLIST_LANGCAP_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_ManuringBlk_YearList.aspx")
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

        YearList.DataSource = LoadData()
        YearList.DataBind()
        lblLocCode.Text = strLocation
        GetActivePeriod(Period)
        lblBgtPeriod.Text = Period

    End Sub

    Protected Function LoadData() As DataSet

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
            strOppCd_GET = "BD_CLSTRX_CROPPROD_BLOCK_YEAR_GET"
            strParam = objGLSetup.EnumBlockType.MatureField & "|" & objGLSetup.EnumBlockStatus.Active & "|" & strLocation & "|"
        Else
            strOppCd_GET = "BD_CLSTRX_CROPPROD_SUBBLOCK_YEAR_GET"
            strParam = objGLSetup.EnumSubBlockType.MatureField & "|" & objGLSetup.EnumSubBlockStatus.Active & "|" & strLocation & "|"
        End If

        Try
            intErrNo = objBD.mtdGetManuringYear(strOppCd_GET, strParam, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_MANURINGBLK_YEAR_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_ManuringBlk_YearList.aspx")
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
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_ManuringBlk_YearList.aspx")
        End Try

        If dsperiod.Tables(0).Rows.Count > 0 Then
            BGTPeriod = dsperiod.Tables(0).Rows(0).Item("BGTPeriod")
            Return dsperiod.Tables(0).Rows(0).Item("PeriodID")
        Else
            BGTPeriod = "No Active Period"
            Response.Redirect("../../BD/Setup/BD_setup_Periods.aspx")
        End If

    End Function

    Sub btnYear_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim stryear As String = CType(sender, LinkButton).CommandArgument
        Dim strOpCd_ManuringBlk_ADD As String = "BD_CLSTRX_MANURINGBLK_ADD"
        Dim strOpCd_ManuringBlk_UPD As String = "BD_CLSTRX_MANURINGBLK_UPD"
        Dim strOppCd_ManBlkLn_SUM As String = "BD_CLSTRX_MANURINGBLKLN_SUM"
        Dim strOppCd_ManBlkLn_UPD As String = "BD_CLSTRX_MANURINGBLKLN_UPD"
        Dim strOppCd_ManBlkLn_GET As String = "BD_CLSTRX_MANURINGBLKLN_GET"
        Dim strOppCd_Period_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"

        Dim strOpCd_BLOCK_GET As String
        Dim strOppCd_BLK_GET As String
        Dim strOpCd_ManuringBlk_Get As String
        Dim strCropType As String
        Dim strBlkStatus As String
        Dim intError As Integer
        Dim strParam As String
        Dim blnBlkYieldLevel As Boolean

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
            strOpCd_BLOCK_GET = "BD_CLSTRX_CROPPROD_BLOCK_GET"
            strOppCd_BLK_GET = "GL_CLSSETUP_BLOCK_GET_BY_BLKCODE"
            strOpCd_ManuringBlk_Get = "BD_CLSTRX_MANURINGBLK_GET"
            strCropType = objGLSetup.EnumBlockType.MatureField
            strBlkStatus = objGLSetup.EnumBlockStatus.Active
            blnBlkYieldLevel = True
        Else
            strOpCd_BLOCK_GET = "BD_CLSTRX_CROPPROD_SUBBLOCK_GET"
            strOppCd_BLK_GET = "GL_CLSSETUP_BLOCK_GET_BY_SUBBLKCODE"
            strOpCd_ManuringBlk_Get = "BD_CLSTRX_MANURINGSUBBLK_GET"
            strCropType = objGLSetup.EnumSubBlockType.MatureField
            strBlkStatus = objGLSetup.EnumSubBlockStatus.Active
            blnBlkYieldLevel = False
        End If

        strParam = "|" & GetActivePeriod("") & "|" & stryear.Trim & "|" & strCropType.Trim & "|" & strBlkStatus.Trim & "|" & strLocation & "|"
        Try
            intErrNo = objBD.mtdAddManuringBlk(strOpCd_BLOCK_GET, _
                                              strOpCd_ManuringBlk_Get, _
                                              strOpCd_ManuringBlk_ADD, _
                                              strOpCd_ManuringBlk_UPD, _
                                              strOppCd_BLK_GET, _
                                              strOppCd_ManBlkLn_SUM, _
                                              strOppCd_ManBlkLn_UPD, _
                                              strOppCd_ManBlkLn_GET, _
                                              strOppCd_Period_GET, _
                                              strCompany, _
                                              strLocation, _
                                              strUserId, _
                                              strParam, _
                                              blnBlkYieldLevel, _
                                              intError, _
                                              objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.ManuringBlk))
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_MANURINGBLK_YEAR_CLICK&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_ManuringBlk_YearList.aspx")
        End Try

        If intError = objBD.EnumErrorType.NoActivePeriod Then
            lblPeriodErrTop.Visible = True
            lblPeriodErr.Visible = True
            Exit Sub
        ElseIf intError = objBD.EnumErrorType.BlockErr Then
            lblBlockErrTop.Visible = True
            lblBlockErr.Visible = True
            Exit Sub
        End If

        Response.Redirect("../../BD/Trx/BD_Trx_ManuringBlk_Details.aspx?yr=" & stryear.Trim)
    End Sub

End Class
