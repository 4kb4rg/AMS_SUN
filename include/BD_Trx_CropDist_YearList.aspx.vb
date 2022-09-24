
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

Public Class BD_CropDist_Year : Inherits Page

    Protected WithEvents YearList As DataGrid
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblProdErrTop As Label
    Protected WithEvents lblProdErr As Label
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents lblTitle As Label
    Protected WithEvents lblPeriodErr As Label
    Protected WithEvents lblPeriodErrTop As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblLocTag As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents BlockTag As Label
    Protected WithEvents lblBgtStatus As Label 

    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objBDTx As New agri.BD.clsTrx()
    Dim objBD As New agri.BD.clsSetup()
    Dim objGL As New agri.GL.clsSetup()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
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
            If SortExpression.Text = "" Then
                SortExpression.Text = "OriBlkCode"
                SortCol.Text = "ASC"
            End If

            If Not Page.IsPostBack Then
                onload_GetLangCap()
                BindGrid()
            End If
        End If
        lblPeriodErr.Visible = False
        lblPeriodErrTop.Visible = False
        lblProdErrTop.Visible = False
        lblProdErr.Visible = False
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblTitle.Text = "CROP DISTRIBUTION"

        lblLocTag.Text = GetCaption(objLangCap.EnumLangCap.Location) & lblCode.Text
        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
            BlockTag.Text = GetCaption(objLangCap.EnumLangCap.Block) & lblCode.Text
            YearList.Columns(0).Visible = False
            YearList.Columns(1).HeaderText = BlockTag.Text
        Else
            BlockTag.Text = GetCaption(objLangCap.EnumLangCap.SubBlock) & lblCode.Text
            YearList.Columns(0).HeaderText = GetCaption(objLangCap.EnumLangCap.Block) & lblCode.Text
            YearList.Columns(1).HeaderText = BlockTag.Text
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
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_CLSTRX_CROPDIST_YEARLIST_CLSLANGCAP_BUSSTERM_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropDist_YearList.aspx")
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

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
            strOppCd_GET = "BD_CLSTRX_CROPDIST_BLOCK_GET"
            strParam = objGL.EnumBlockType.MatureField & "|" & objGL.EnumBlockStatus.Active & "|" & strLocation & "||" & GetActivePeriod("")
        Else
            strOppCd_GET = "BD_CLSTRX_CROPDIST_SUBBLOCK_YEARLIST_GET"
            strParam = objGL.EnumSubBlockType.MatureField & "|" & objGL.EnumSubBlockStatus.Active & "|" & strLocation & "||" & GetActivePeriod("")
        End If

        Try
            intErrNo = objBDTx.mtdGetCropYear(strOppCd_GET, strParam, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_CLSTRX_CROPDIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropDist_YearList.aspx")
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
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_CLSTRX_CROPDIST_GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropDist_YearList.aspx")
        End Try

        If dsperiod.Tables(0).Rows.Count > 0 Then
            BGTPeriod = dsperiod.Tables(0).Rows(0).Item("BGTPeriod") & " - (" & objBD.mtdGetPeriodStatus(dsperiod.Tables(0).Rows(0).Item("Status")) & ")"
            lblBgtStatus.Text = dsperiod.Tables(0).Rows(0).Item("Status")
            Return dsperiod.Tables(0).Rows(0).Item("PeriodID")
        Else
            BGTPeriod = "No Active Period"
            Response.Redirect("../../BD/Setup/BD_setup_Periods.aspx")
        End If
    End Function

    Sub btnBlockYear_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim strYear As String = CType(sender, LinkButton).CommandArgument
        Dim strSubBlk As String = CType(sender, LinkButton).CommandName
        Dim strBlk As String = CType(sender, LinkButton).Text

        Dim strOppCd_Period_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"
        Dim strOppCd_CropDist_ADD As String = "BD_CLSTRX_CROPDIST_ADD"
        Dim strOppCd_CropDist_UPD As String = "BD_CLSTRX_CROPDIST_UPD"
        Dim strOppCd_SubBlock_Get As String = "BD_CLSTRX_CROPPROD_SUBBLOCK_GET"
        Dim strOppCd_Block_GET As String = "BD_CLSTRX_CROPDIST_BLOCK_CLICK_SBLK_GET"
        Dim strOppCd_CropDist_List_GET As String = "BD_CLSTRX_CROPDIST_GET"
        Dim strOppCd_CropDist_GET As String
        Dim strOppCd_CropDist_SUM As String = "BD_CLSTRX_CROPDIST_SUM_GET"
        Dim strOppCd_CropProd_GET As String
        Dim intError As Integer
        Dim strParam As String
        Dim strBlkCode As String

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
            strOppCd_CropProd_GET = "BD_CLSTRX_CROPPROD_GET"
            strOppCd_CropDist_GET = "BD_CLSTRX_CROPDIST_BLOCK_CLICK_BLK_GET"
        Else
            strOppCd_CropProd_GET = "BD_CLSTRX_CROPPROD_SBLK_GET"
            strOppCd_CropDist_GET = "BD_CLSTRX_CROPDIST_BLOCK_CLICK_SBLK_GET"

        End If

        strParam = GetActivePeriod("") & "|" & strYear.Trim & "|" & strSubBlk.Trim & "|" & strBlk.Trim & "|"
        Try
            intErrNo = objBDTx.mtdAddCropDist(strOppCd_CropDist_ADD, _
                                              strOppCd_Period_GET, _
                                              strOppCd_CropDist_GET, _
                                              strOppCd_CropDist_UPD, _
                                              strOppCd_CropDist_SUM, _
                                              strOppCd_CropProd_GET, _
                                              strOppCd_SubBlock_Get, _
                                              strOppCd_Block_GET, _
                                              strOppCd_CropDist_List_GET, _
                                              strCompany, _
                                              strLocation, _
                                              strUserId, _
                                              strParam, _
                                              intError, _
                                              True, _
                                              objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting), _
                                              strBlkCode)

        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_CLSTRX_CROPDIST_SBLK_CLICK&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropDist_YearList.aspx")
        End Try

        If intError = objBDTx.EnumErrorType.NoActivePeriod Then
            lblPeriodErr.Visible = True
            lblPeriodErrTop.Visible = True

        Else
            Response.Redirect("../../BD/Trx/BD_Trx_CropDist_Details.aspx?distbyblk=true&blk=" & strBlk.Trim & "&subblk=" & strSubBlk.Trim & "&yr=" & strYear.Trim)
        End If

    End Sub

    Sub btnSubBlockYear_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim stryear As String = CType(sender, LinkButton).CommandArgument
        Dim strBlk As String = CType(sender, LinkButton).CommandName
        Dim strSubBlk As String = CType(sender, LinkButton).Text

        Dim strOppCd_Period_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"
        Dim strOppCd_CropDist_GET As String = "BD_CLSTRX_CROPDIST_GET"
        Dim strOppCd_CropDist_ADD As String = "BD_CLSTRX_CROPDIST_ADD"
        Dim strOppCd_CropDist_UPD As String = "BD_CLSTRX_CROPDIST_UPD"
        Dim strOppCd_SubBlock_Get As String = "BD_CLSTRX_CROPPROD_SUBBLOCK_GET"
        Dim strOppCd_Block_GET As String = "BD_CLSTRX_CROPDIST_BLOCK_CLICK_SBLK_GET"
        Dim strOppCd_CropDist_List_GET As String = "BD_CLSTRX_CROPDIST_GET"
        Dim strOppCd_CropProd_GET As String
        Dim strOppCd_CropDist_SUM As String
        Dim intError As Integer
        Dim strParam As String
        Dim strBlkCode As String

        strParam = GetActivePeriod("") & "|" & stryear.Trim & "|" & strSubBlk.Trim & "||"
        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
            strOppCd_CropDist_SUM = "BD_CLSTRX_CROPDIST_SUM_GET"
            strOppCd_CropProd_GET = "BD_CLSTRX_CROPPROD_GET"
        Else
            strOppCd_CropDist_SUM = "BD_CLSTRX_CROPDIST_SBLK_SUM_GET"
            strOppCd_CropProd_GET = "BD_CLSTRX_CROPPROD_SBLK_GET"
        End If

        Try
            intErrNo = objBDTx.mtdAddCropDist(strOppCd_CropDist_ADD, _
                                              strOppCd_Period_GET, _
                                              strOppCd_CropDist_GET, _
                                              strOppCd_CropDist_UPD, _
                                              strOppCd_CropDist_SUM, _
                                              strOppCd_CropProd_GET, _
                                              strOppCd_SubBlock_Get, _
                                              strOppCd_Block_GET, _
                                              strOppCd_CropDist_List_GET, _
                                              strCompany, _
                                              strLocation, _
                                              strUserId, _
                                              strParam, _
                                              intError, _
                                              False, _
                                              objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting), _
                                              strBlkCode)

        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_CLSTRX_CROPDIST_SBLK_CLICK&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropDist_YearList.aspx")
        End Try

        If intError = objBDTx.EnumErrorType.NoActivePeriod Then
            lblPeriodErr.Visible = True
            lblPeriodErrTop.Visible = True
        ElseIf intError = objBDTx.EnumErrorType.CalculationErr Then
            lblProdErrTop.Text = "Production Budgeting not complete for " & BlockTag.Text & " - " & strBlkCode
            lblProdErr.Text = "Production Budgeting not complete for " & BlockTag.Text & " - " & strBlkCode

            lblProdErrTop.Visible = True
            lblProdErr.Visible = True
        Else
            Response.Redirect("../../BD/Trx/BD_Trx_CropDist_Details.aspx?distbyblk=false&blk=" & strBlk.Trim & "&subblk=" & strSubBlk.Trim & "&yr=" & stryear.Trim)
        End If
    End Sub

End Class
