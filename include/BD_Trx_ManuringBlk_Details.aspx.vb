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

Public Class BD_ManuringBlk_Det : Inherits Page

    Protected WithEvents dgManBlk As DataGrid
    Protected WithEvents lstPeriod As DropDownList
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents lblYear As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBgtPeriod As Label    
    Protected WithEvents lblCode As Label
    Protected WithEvents lblBlock As Label
    Protected WithEvents lblSubBlk As Label
    Protected WithEvents lblLocTag As Label
    Protected WithEvents lblTitle As Label
    Protected WithEvents lblBgtStatus As Label 

    Dim objBD As New agri.BD.clsTrx()
    Dim objBDSetup As New agri.BD.clsSetup()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Dim objDataSet As New DataSet()
    Dim objLangCapDs As New DataSet()
    Dim dsperiod As New DataSet()
    Dim intErrNo As Integer
    Dim strParam As String = ""
    Dim intPeriod As Integer
    Dim arr As Array

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
                lblYear.Text = Request.QueryString("Yr")

                BindGrid()
            End If
        End If
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()

        lblLocTag.Text = GetCaption(objLangCap.EnumLangCap.Location) & lblCode.Text
        lblBlock.Text = GetCaption(objLangCap.EnumLangCap.Block)
        lblSubBlk.Text = GetCaption(objLangCap.EnumLangCap.SubBlock)
        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
            lblTitle.Text = "DETAILED MANURING SCHEDULE BY " & UCase(lblBlock.Text)
            dgManBlk.Columns(0).HeaderText = lblBlock.Text & lblCode.Text
        Else
            lblTitle.Text = "DETAILED MANURING SCHEDULE BY " & UCase(lblSubBlk.Text)
            dgManBlk.Columns(0).HeaderText = lblSubBlk.Text & lblCode.Text
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
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_MANURINGBLK_DETAILS_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_ManuringBlk_Details.aspx")
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
        Dim PageNo As Integer
        Dim Period As String

        dgManBlk.DataSource = LoadData()
        dgManBlk.DataBind()
        lblLocCode.Text = strLocation
        GetActivePeriod(Period)
        lblBgtPeriod.Text = Period

        BindPeriod()

    End Sub

    Protected Function LoadData() As DataSet
        Dim strOppCd_GET As String

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
            strOppCd_GET = "BD_CLSTRX_MANURINGBLK_GET"
        Else
            strOppCd_GET = "BD_CLSTRX_MANURINGSUBBLK_GET"
        End If

        strParam = strLocation & "|" & _
                   GetActivePeriod("") & "|" & _
                   lblYear.Text & "||" & _
                   SortExpression.Text & " " & SortCol.Text
        Try
            intErrNo = objBD.mtdGetManuringBlk(strOppCd_GET, strParam, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BD_MANURINGBLK_DETAILS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_ManuringBlk_Details.aspx")
        End Try
        Return objDataSet
    End Function

    Sub BindPeriod()
        Dim periodStr As String
        Dim ddl As DropDownList
        Dim intCnt As Integer
        Dim intRec As Integer

        Try
            intErrNo = objBD.mtdGetPeriodRange(intPeriod, _
                                               dsperiod, _
                                               periodStr)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BGTPERIOD&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_ManuringBlk_Details.aspx")
        End Try

        periodStr = periodStr & ","
        arr = Split(periodStr, ",")

        For intRec = 0 To dgManBlk.Items.Count - 1
            For intCnt = 0 To arr.GetUpperBound(0) - 1
                lstPeriod = dgManBlk.Items.Item(intRec).FindControl("lstPeriod")
                lstPeriod.Items.Add(New ListItem(arr(intCnt), arr(intCnt)))
            Next
        Next
    End Sub

    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strManBlkID As String
        Dim strBlkCode As String
        Dim strPeriod As String
        Dim Label As Label
        Dim ddl As DropDownList
        Dim strPlantedArea As String
        Dim strSPH As String

        Label = E.Item.FindControl("lblManuringBlkID")
        strManBlkID = Trim(Label.Text)
        Label = E.Item.FindControl("lblBlkCode")
        strBlkCode = Trim(Label.Text)
        ddl = E.Item.FindControl("lstPeriod")
        strPeriod = Trim(ddl.SelectedItem.Value)
        Label = E.Item.FindControl("lblPlantedArea")
        strPlantedArea = Trim(Label.Text)
        Label = E.Item.FindControl("lblSPH")
        strSPH = Trim(Label.Text)

        Response.Redirect("BD_trx_ManuringBlk_Line.aspx?yr=" & lblYear.Text & "&manblkid=" & strManBlkID & _
                          "&blkcode=" & strBlkCode & "&period=" & strPeriod & "&sph=" & strSPH & "&plantedarea=" & strPlantedArea)

    End Sub

    Protected Function GetActivePeriod(ByRef BGTPeriod As String) As String
        Dim strOppCd_Bgt_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"

        strParam = "|||||" & objBDSetup.EnumPeriodStatus.Active & "|" & strLocation & "|"
        Try
            intErrNo = objBDSetup.mtdGetPeriodList(strOppCd_Bgt_GET, strParam, dsperiod)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_ManuringBlk_Details.aspx")
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

    Sub BackBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("BD_trx_ManuringBlk_YearList.aspx")
    End Sub

End Class
