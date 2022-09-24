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


Public Class BD_VehRunDist_List : Inherits Page

    Protected WithEvents VehList As DataGrid
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblTitle As Label
    Protected WithEvents lblLocTag As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblVehType As Label
    Protected WithEvents lblVehCode As Label
    Protected WithEvents lblVehDesc As Label
    Protected WithEvents lblCode As Label

    Dim strOppCd_BgtPeriod_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"

    Dim objBDSetup As New agri.BD.clsSetup()
    Dim objBDTrx As New agri.BD.clsTrx()
    Dim objGLSetup As New agri.GL.clsSetup()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objAdminLoc As New agri.Admin.clsLoc()

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
    Dim strLocType as String

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        intADAR = Session("SS_ADAR")
        strAccMonth = Session("SS_GLACCMONTH")
        strAccYear = Session("SS_GLACCYEAR")
        strLocType = Session("SS_LOCTYPE")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumADAccessRights.ADBudgeting), intADAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")

        Else
            If Not Page.IsPostBack Then
                lblVehType.Text = Request.QueryString("typ")

                onload_GetLangCap()
                BindGrid()
            End If
        End If
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblLocTag.Text = GetCaption(objLangCap.EnumLangCap.Location) & lblCode.Text
        lblVehCode.Text = GetCaption(objLangCap.EnumLangCap.Vehicle) & lblCode.Text
        lblVehDesc.Text = GetCaption(objLangCap.EnumLangCap.VehicleDesc)
        lblTitle.Text = UCase(GetCaption(objLangCap.EnumLangCap.Vehicle))
        VehList.Columns(0).HeaderText = lblVehCode.Text
        VehList.Columns(1).HeaderText = lblVehDesc.Text
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
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_VEHRUNDIST_LIST_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_VehRunDist_List.aspx?typ=" & lblVehType.Text)
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

        VehList.DataSource = LoadData()
        VehList.DataBind()
        lblLocCode.Text = strLocation
        GetActivePeriod(Period)
        lblBgtPeriod.Text = Period

    End Sub

    Protected Function LoadData() As DataSet

        Dim strOppCd_GET As String = "GL_CLSSETUP_VEHICLE_LIST_GET"
        Dim strParam As String
        Dim SearchStr As String
        Dim sortItem As String


        SearchStr = " AND VEH.Status = '" & objGLSetup.EnumVehicleStatus.Active & "' AND VEH.VehTypeCode = '" & lblVehType.Text & "' AND Veh.LocCode = '" & strLocation & "'"

        sortItem = "ORDER BY VEH.VehCode ASC"
        strParam = sortItem & "|" & SearchStr

        Try
            intErrNo = objGLSetup.mtdGetMasterList(strOppCd_GET, strParam, objGLSetup.EnumGLMasterType.Vehicle, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_VEHTYPEDIST_VEHICLELIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_VehRunDist_List.aspx?typ=" & lblVehType.Text)
        End Try

        Return objDataSet
    End Function

    Protected Function GetActivePeriod(ByRef BGTPeriod As String) As String

        Dim dsperiod As New DataSet()

        strParam = "|||||" & objBDSetup.EnumPeriodStatus.Active & "|" & strLocation & "|"
        Try
            intErrNo = objBDSetup.mtdGetPeriodList(strOppCd_BgtPeriod_GET, strParam, dsperiod)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_VehRunDist_List.aspx?typ=" & lblVehType.Text)
        End Try

        If dsperiod.Tables(0).Rows.Count > 0 Then
            BGTPeriod = dsperiod.Tables(0).Rows(0).Item("BGTPeriod")
            Return dsperiod.Tables(0).Rows(0).Item("PeriodID")
        Else
            BGTPeriod = "No Active Period"
            Response.Redirect("../../BD/Setup/BD_setup_Periods.aspx")
        End If
    End Function

    Sub btnVeh_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim strVeh As String = CType(sender, LinkButton).CommandArgument
        Dim intError As Integer = objBDTrx.EnumErrorType.NoError
        Dim strOppCd_VehRunSetup_GET As String = "BD_CLSSETUP_VEHRUNNING_FORMAT_GET"
        Dim strOpCd_VehRunDist_Add As String = "BD_CLSTRX_VEHRUNDIST_ADD"
        Dim strOpCd_VehRunDist_UPD As String = "BD_CLSTRX_VEHRUNDIST_UPD"
        Dim strOpCd_Formula_GET As String = "BD_CLSTRX_CALCFORMULA_GET"
        Dim strOpCd_VehRunDist_GET As String = "BD_CLSTRX_VEHRUNDIST_GET"
        Dim strOppCd_VehRunDist_AccPeriod_ADD As String = "BD_CLSTRX_VEHRUNDIST_ACCPERIOD_ADD"
        Dim strOppCd_VehRunDist_AccPeriod_UPD As String = "BD_CLSTRX_VEHRUNDIST_ACCPERIOD_UPD"
        Dim strOppCd_VehRunDist_AccPeriod_GET As String = "BD_CLSTRX_VEHRUNDIST_ACCPERIOD_GET"
        Dim strOppCd_VehRunDist_YrBudgetCost_GET As String = "BD_CLSTRX_VEHRUNDIST_YRBUDGETCOST_GET"
        Dim strOppCd_VehRunDist_AccPeriod_List_GET As String = "BD_CLSTRX_VEHRUNDIST_ACCPERIOD_LIST_GET"

        strParam = GetActivePeriod("") & "|" & strVeh.Trim & "|"
        Try
            intErrNo = objBDTrx.mtdAddVehRunningDist(strOpCd_VehRunDist_GET, _
                                                     strOpCd_VehRunDist_Add, _
                                                     strOpCd_VehRunDist_UPD, _
                                                     strOppCd_VehRunSetup_GET, _
                                                     strOppCd_VehRunDist_AccPeriod_ADD, _
                                                     strOppCd_VehRunDist_AccPeriod_UPD, _
                                                     strOppCd_VehRunDist_AccPeriod_GET, _
                                                     strOppCd_VehRunDist_AccPeriod_List_GET, _
                                                     strOppCd_VehRunDist_YrBudgetCost_GET, _
                                                     strOppCd_BgtPeriod_GET, _
                                                     strOpCd_Formula_GET, _
                                                     strCompany, _
                                                     strLocation, _
                                                     strUserId, _
                                                     strParam, _
                                                     intError)

        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_VEHRUNDIST_VEH_CLICK&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_VehRunDist_List.aspx?typ=" & lblVehType.Text)
        End Try
        If intError = objBDTrx.EnumErrorType.NoError Then
            Response.Redirect("../../BD/Trx/BD_Trx_VehRunDist_Details.aspx?veh=" & strVeh.Trim)
        Else
        End If
    End Sub


End Class
