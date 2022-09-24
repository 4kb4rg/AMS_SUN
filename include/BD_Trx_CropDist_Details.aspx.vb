
Imports System
Imports System.Data
Imports System.Math
Imports System.Collections
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic

Public Class BD_CropDist_Det : Inherits Page

    Protected WithEvents dgCropDist As DataGrid
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblOvrMsg As Label
    Protected WithEvents lblOvrMsgTop As Label
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents lblYear As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblTtlWeight As Label
    Protected WithEvents lblPercent As Label
    Protected WithEvents lblLocTag As Label
    Protected WithEvents lblTitle As Label
    Protected WithEvents lblBlockTag As Label
    Protected WithEvents lblSubBlkTag As Label 
    Protected WithEvents lblBlkCode As Label
    Protected WithEvents lblSubBlkCode As Label 
    Protected WithEvents lblCode As Label
    Protected WithEvents RowPlantingYr As HtmlTableRow
    Protected WithEvents RowSubBlk As HtmlTableRow 
    Protected WithEvents CellPercent As HtmlTableCell   
    Protected WithEvents hidBlkCode As HtmlInputHidden 
    Protected WithEvents hidDistByBlk As HtmlInputHidden 
    Protected WithEvents lblBgtStatus As Label 
    Protected WithEvents ibDist As Button  

    Dim objBDTx As New agri.BD.clsTrx()
    Dim objBD As New agri.BD.clsSetup()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objAdminLoc As New agri.Admin.clsLoc()
    

    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()

    Dim strOppCd_GET As String
    Dim strOppCd_UPD As String = "BD_CLSTRX_CROPPROD_UPD"

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
            lblOvrMsg.Visible = False
            lblOvrMsgTop.Visible = False

            If Not Page.IsPostBack Then
                lblYear.Text = Request.QueryString("yr").Trim
                lblBlkCode.Text = Request.QueryString("blk").Trim
                lblSubBlkCode.Text = Request.QueryString("subblk").Trim
                hidDistByBlk.Value = Request.QueryString("distbyblk").Trim

                If Request.QueryString("distbyblk") = True Then
                    hidBlkCode.Value = lblBlkCode.Text
                Else
                    hidBlkCode.Value = lblSubBlkCode.Text
                End If

                onload_GetLangCap()
                BindGrid()
                DisableRow()
            End If

            If lblBgtStatus.Text.Trim = objBD.EnumPeriodStatus.Addvote Then
                ibDist.Visible = False
            End If

            If SortExpression.Text = "" Then
                SortExpression.Text = "CropDistID"
                SortCol.Text = "ASC"
            End If
        End If
    End Sub

    Sub DisableRow()
        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
            RowSubBlk.Visible = False
        Else
            If hidDistByBlk.Value = True Then
                RowPlantingYr.Visible = False
                RowSubBlk.Visible = False
                CellPercent.Visible = False
                dgCropDist.Columns(2).Visible = False
            Else
                RowSubBlk.Visible = True
            End If
        End If
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()

        lblTitle.Text = "CROP DISTRIBUTION"
        lblLocTag.Text = GetCaption(objLangCap.EnumLangCap.Location) & lblCode.Text
        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
            lblBlockTag.Text = GetCaption(objLangCap.EnumLangCap.Block) & lblCode.Text
        Else
            lblBlockTag.Text = GetCaption(objLangCap.EnumLangCap.Block) & lblCode.Text
            lblSubBlkTag.Text = GetCaption(objLangCap.EnumLangCap.SubBlock) & lblCode.Text
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
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_CROPDIST_DETAILS_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropDist_Details.aspx")
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

    Sub DistBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strOppCd_GET As String
        Dim dsCropProd As DataSet

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
            strOppCd_GET = "BD_CLSTRX_CROPPROD_GET"
            strParam = strLocation & "|" & GetActivePeriod("") & "||" & " AND BD.BlkCode = '" & hidBlkCode.Value & "' |"
        Else
            If Request.QueryString("distbyblk") = True Then
                strOppCd_GET = "BD_CLSTRX_CROPPROD_SBLK_GET"
                strParam = strLocation & "|" & GetActivePeriod("") & "||" & " AND SBLK.BlkCode = '" & lblBlkCode.Text & "' |"
            Else
                strOppCd_GET = "BD_CLSTRX_CROPPROD_GET"
                strParam = strLocation & "|" & GetActivePeriod("") & "||" & " AND BD.BlkCode = '" & hidBlkCode.Value & "' |"
            End If
        End If

        Try
            intErrNo = objBDTx.mtdGetCropProd(strOppCd_GET, strParam, dsCropProd)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_CROPDIST_DET_DIST_CLICK&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropDist_Details.aspx")
        End Try

            Response.Write("<Script Language=""JavaScript"">pop_Dist=window.open(""../../BD/Trx/BD_Trx_CropDist_distribute.aspx?distbyblk=" & Request.QueryString("distbyblk") & "&blk=" & lblBlkCode.Text.Trim & _
                           "&subblk=" & lblSubBlkCode.Text & "&yr=" & lblYear.Text.Trim & """, null ,""'pop_Dist',width=400,height=500,top=100,left=250,status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");pop_Dist.focus();</Script>")
    End Sub

    Sub BindGrid()
        Dim Period As String

        dgCropDist.DataSource = LoadData()
        dgCropDist.DataBind()
        lblLocCode.Text = strLocation
        GetActivePeriod(Period)
        lblBgtPeriod.Text = Period
        LoadDistTotal()
    End Sub

    Protected Sub LoadDistTotal()
        Dim strOppCdCropDist_SUM As String
        Dim dsTotal As DataSet

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
            strOppCdCropDist_SUM = "BD_CLSTRX_CROPDIST_SUM_GET"
            strParam = strLocation & "|" & GetActivePeriod("") & "|" & hidBlkCode.Value & "||"
        Else
            If Request.QueryString("distbyblk") = True Then
                strOppCdCropDist_SUM = "BD_CLSTRX_CROPDIST_SBLK_SUM_GET"
                strParam = strLocation & "|" & GetActivePeriod("") & "|| AND SBLK.BlkCode = '" & lblBlkCode.Text & "' |"
            Else
                strOppCdCropDist_SUM = "BD_CLSTRX_CROPDIST_SUM_GET"
                strParam = strLocation & "|" & GetActivePeriod("") & "|" & hidBlkCode.Value & "||"
            End If

        End If

        Try
            intErrNo = objBDTx.mtdGetCropDist(strOppCdCropDist_SUM, strParam, dsTotal)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_CROPDIST_DET_LOADTOTAL&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropDist_Details.aspx")
        End Try

        lblTtlWeight.Text = FormatNumber(dsTotal.Tables(0).Rows(0).Item("Yield"), 2)
        lblPercent.Text = FormatNumber(Round(dsTotal.Tables(0).Rows(0).Item("Percentage")), 2)

        lblTtlWeight.Text = objGlobal.GetIDDecimalSeparator(FormatNumber(dsTotal.Tables(0).Rows(0).Item("Yield"), 0))
        lblPercent.Text = objGlobal.GetIDDecimalSeparator(FormatNumber(Round(dsTotal.Tables(0).Rows(0).Item("Percentage")), 0))


    End Sub

    Protected Function LoadData() As DataSet

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
            strOppCd_GET = "BD_CLSTRX_CROPDIST_GET"
            strParam = strLocation & "|" & GetActivePeriod("") & "|" & hidBlkCode.Value & "||" & SortExpression.Text & " " & SortCol.Text
        Else
            If Request.QueryString("distbyblk") = True Then
                strOppCd_GET = "BD_CLSTRX_CROPDIST_DISTBYBLK_GET"
                strParam = strLocation & "|" & GetActivePeriod("") & "|| AND SBLK.BlkCode = '" & hidBlkCode.Value & "' |" & SortExpression.Text & " " & SortCol.Text
            Else
                strOppCd_GET = "BD_CLSTRX_CROPDIST_GET"
                strParam = strLocation & "|" & GetActivePeriod("") & "|" & hidBlkCode.Value & "||" & SortExpression.Text & " " & SortCol.Text
            End If
        End If

        Try
            intErrNo = objBDTx.mtdGetCropDist(strOppCd_GET, strParam, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_CROPDIST_DET_GET_BUDGET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropDist_Details.aspx")
        End Try

        Return objDataSet
    End Function


    Protected Function GetActivePeriod(ByRef BGTPeriod As String) As String
        Dim strOppCd_Bgt_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"
        Dim dsperiod As New DataSet()

        strParam = "|||||" & objBD.EnumPeriodStatus.Active & "|" & strLocation & "|"
        Try
            intErrNo = objBD.mtdGetPeriodList(strOppCd_Bgt_GET, strParam, dsperiod)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_CROPDIST_DET_GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropDist_Details.aspx")
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

End Class
