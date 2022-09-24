
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

Public Class BD_CropProd_Year : Inherits Page

    Protected WithEvents YearList As DataGrid
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblPeriodErrTop As Label
    Protected WithEvents lblPeriodErr As Label
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents lblTitle As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblTotAmtFig As Label
    Protected WithEvents BlockTag As Label
    Protected WithEvents SubBlockTag As Label
    Protected WithEvents lblLocTag As Label
    Protected WithEvents lblNoOf As Label
    Protected WithEvents lblCode As Label

    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objBDTx As New agri.BD.clsTrx()
    Dim objBD As New agri.BD.clsSetup()
    Dim objGL As New agri.GL.clsSetup()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Dim strOppCd_GET As String

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
                SortExpression.Text = "ExpDetails"
                SortCol.Text = "ASC"
            End If
            If Not Page.IsPostBack Then
                BindGrid()
            End If
        End If
        lblPeriodErrTop.Visible = False
        lblPeriodErr.Visible = False
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblTitle.Text = "CROP PRODUCTION"
        lblLocTag.Text = GetCaption(objLangCap.EnumLangCap.Location) & lblCode.Text

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
            BlockTag.Text = GetCaption(objLangCap.EnumLangCap.Block)
            YearList.Columns(1).HeaderText = lblNoOf.Text & BlockTag.Text
        Else
            SubBlockTag.Text = GetCaption(objLangCap.EnumLangCap.SubBlock)
            YearList.Columns(1).HeaderText = lblNoOf.Text & SubBlockTag.Text
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BD_TRX_GROPPRODYEARLIST_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropProd_YearList.aspx")
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



    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        YearList.CurrentPageIndex = 0
        YearList.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub BindGrid()
        Dim Period As String

        YearList.DataSource = LoadData()
        YearList.DataBind()
        lblLocCode.Text = strLocation
        GetActivePeriod(Period)
        lblBgtPeriod.Text = Period

    End Sub

    Protected Function LoadData() As DataSet
        Dim Period As String

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
            strOppCd_GET = "BD_CLSTRX_CROPPROD_BLOCK_YEAR_GET"
            strParam = objGL.EnumBlockType.MatureField & "|" & objGL.EnumBlockStatus.Active & "|" & strLocation & "||"
        Else
            strOppCd_GET = "BD_CLSTRX_CROPPROD_SUBBLOCK_YEAR_GET"
            strParam = objGL.EnumSubBlockType.MatureField & "|" & objGL.EnumSubBlockStatus.Active & "|" & strLocation & "||"
        End If

        Try
            intErrNo = objBDTx.mtdGetCropYear(strOppCd_GET, strParam, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropProd_YearList.aspx")
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
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropProd_YearList.aspx")
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
        Dim strOppCd_Period_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"
        Dim strOppCd_ActualYield_GET As String = "BD_CLSTRX_CROPPROD_ACTUALYIELD_GET"
        Dim strOpCd_CropProd_Get As String = "BD_CLSTRX_CROPPROD_GET"
        Dim strOpCd_CropProd_ADD As String = "BD_CLSTRX_CROPPROD_ADD"
        Dim strOpCd_Block_Get As String
        Dim strOppCd_BLK_GET As String
        Dim intError As Integer
        Dim strParam As String
        Dim strCropType As String
        Dim strBlkStatus As String
        Dim strTransBlkCode As String
        Dim strAreaBlkCode As String
        Dim blnBlkYieldLevel As Boolean
        Dim strBlockTag As String

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
            strOppCd_BLK_GET = "GL_CLSSETUP_BLOCK_GET_BY_BLKCODE"
            strOpCd_Block_Get = "BD_CLSTRX_CROPPROD_BLOCK_GET"
            strCropType = objGL.EnumBlockType.MatureField
            strBlkStatus = objGL.EnumBlockStatus.Active
            blnBlkYieldLevel = True
            strBlockTag = BlockTag.Text
        Else
            strOppCd_BLK_GET = "GL_CLSSETUP_BLOCK_GET_BY_SUBBLKCODE"
            strOpCd_Block_Get = "BD_CLSTRX_CROPPROD_SUBBLOCK_GET"
            strCropType = objGL.EnumSubBlockType.MatureField
            strBlkStatus = objGL.EnumSubBlockStatus.Active
            blnBlkYieldLevel = False
            strBlockTag = SubBlockTag.Text
        End If

        strParam = "|" & GetActivePeriod("") & "|" & stryear.Trim & "|" & strCropType.Trim & "|" & strBlkStatus.Trim & "|" & strLocation & "|"
        Try
            intErrNo = objBDTx.mtdAddCropProd(strOpCd_Block_Get, _
                                              strOpCd_CropProd_Get, _
                                              strOpCd_CropProd_ADD, _
                                              strOppCd_BLK_GET, _
                                              strOppCd_Period_GET, _
                                              strOppCd_ActualYield_GET, _
                                              strCompany, _
                                              strLocation, _
                                              strUserId, _
                                              strParam, _
                                              intError, _
                                              blnBlkYieldLevel)

        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_CROPPROD_GET_YEARLIST&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropProd_YearList.aspx")
        End Try

        If intError = objBDTx.EnumErrorType.NoActivePeriod Then
            lblPeriodErrTop.Visible = True
            lblPeriodErr.Visible = True


        Else
            Response.Redirect("../../BD/Trx/BD_Trx_CropProd_Details.aspx?yr=" & stryear) '& "&tblk=" & strTransBlkCode & "&ablk=" & strAreaBlkCode)
        End If
    End Sub

End Class
