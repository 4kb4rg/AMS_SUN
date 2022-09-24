Imports System
Imports System.Data
Imports System.IO 
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports Microsoft.VisualBasic.Information
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic.Interaction

Public Class BD_VehTypeDist : Inherits Page

    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents lblDupMsg As Label

    Protected WithEvents lblActTag As Label
    Protected WithEvents ddlAct As DropDownList
    Protected WithEvents lblBgtTag As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblVehTypeTag As Label
    Protected WithEvents ddlVehType As DropDownList
    Protected WithEvents lblLocTag As Label
    Protected WithEvents lblLocCode As Label

    Protected WithEvents dgVehDist As DataGrid

    Dim objBD As New agri.BD.clsSetup()
    Dim objBDTrx As New agri.BD.clsTrx()
    Dim objGLSetup As New agri.GL.clsSetup()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objLoc As New agri.Admin.clsLoc()

    Dim objDsActivity As New DataSet()
    Dim objDsVehType As New DataSet()
    Dim objDsVehDist As New DataSet()
    Dim objLangCapDs As New DataSet()
    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intADAR As Integer

    Dim strOppCd_GET As String = "BD_CLSSETUP_VEHTYPEDIST_GET"
    Dim strOppCd_ADD As String = "BD_CLSSETUP_VEHTYPEDIST_ADD"
    Dim strOppCd_DEL As String = "BD_CLSSETUP_VEHTYPEDIST_DEL"

    Dim intErrNo As Integer
    Dim strParam As String
    Dim intError As Integer
    Dim strLocType as String

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        intADAR = Session("SS_ADAR")
        strLocType = Session("SS_LOCTYPE")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumADAccessRights.ADBudgeting), intADAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else

            onload_GetLangCap()
            lblDupMsg.Visible = False
            If Not IsPostBack Then
                BindActivity()
                BindVehicleType()
                BindGrid()
                onLoad_LineDisplay()
            End If
        End If
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblDupMsg.Text = lblActTag.Text & " and " & lblVehTypeTag.Text & " exists!"
        lblLocTag.Text = GetCaption(objLangCap.EnumLangCap.Location) & lblCode.Text
        lblActTag.Text = GetCaption(objLangCap.EnumLangCap.Activity) & lblCode.Text
        lblVehTypeTag.Text = GetCaption(objLangCap.EnumLangCap.VehType) & lblCode.Text
        dgVehDist.Columns(0).HeaderText = GetCaption(objLangCap.EnumLangCap.Activity)
        dgVehDist.Columns(1).HeaderText = GetCaption(objLangCap.EnumLangCap.VehType)

    End Sub

    Sub GetEntireLangCap()
        Dim strOpCode_BussTerm As String = "PWSYSTEM_CLSLANGCAP_BUSSTERM_GET"
        Dim strParam As String

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
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_VEHDIST_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_VehDist.aspx")
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


    Protected Function GetActivePeriod(ByRef BGTPeriod As String) As String
        Dim strOppCd_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"
        Dim dsperiod As New DataSet()

        strParam = "|||||" & objBD.EnumPeriodStatus.Active & "|" & strLocation & "|"
        Try
            intErrNo = objBD.mtdGetPeriodList(strOppCd_GET, strParam, dsperiod)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_setup_VehDist.aspx")
        End Try

        If dsperiod.Tables(0).Rows.Count > 0 Then
            BGTPeriod = dsperiod.Tables(0).Rows(0).Item("BGTPeriod")
            Return dsperiod.Tables(0).Rows(0).Item("PeriodID")
        Else
            BGTPeriod = "No Active Period"
            Return ""
        End If

    End Function

    Sub BindActivity()
        Dim strOppCd_Activity_GET As String = "GL_CLSSETUP_ACTIVITY_LIST_GET"
        Dim intCnt As Integer
        Dim dr As DataRow

        strParam = "||" & objGLSetup.EnumActStatus.Active & "||" & "ActCode " & "|"
        Try
            intErrNo = objGLSetup.mtdGetActivity(strOppCd_Activity_GET, _
                                                 strParam, _
                                                 objDsActivity, _
                                                 False)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_ACTIVITY_LIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_VehDist.aspx")
        End Try

        For intCnt = 0 To objDsActivity.Tables(0).Rows.Count - 1
            objDsActivity.Tables(0).Rows(intCnt).Item("ActCode") = Trim(objDsActivity.Tables(0).Rows(intCnt).Item("ActCode"))
            objDsActivity.Tables(0).Rows(intCnt).Item("Description") = Trim(objDsActivity.Tables(0).Rows(intCnt).Item("ActCode")) & " (" & Trim(objDsActivity.Tables(0).Rows(intCnt).Item("Description")) & ")"
        Next

        dr = objDsActivity.Tables(0).NewRow()
        dr("ActCode") = ""
        dr("Description") = "Select " & lblActTag.Text
        objDsActivity.Tables(0).Rows.InsertAt(dr, 0)

        ddlAct.DataSource = objDsActivity.Tables(0)
        ddlAct.DataValueField = "ActCode"
        ddlAct.DataTextField = "Description"
        ddlAct.DataBind()

    End Sub

    Sub BindVehicleType()
        Dim strOppCd_VehicleType_GET As String = "GL_CLSSETUP_VEHICLETYPE_LIST_GET"
        Dim intCnt As Integer
        Dim dr As DataRow

        strParam = "ORDER BY Veh.VehTypeCode" & "|" & "AND Veh.Status = '" & objGLSetup.EnumVehTypeStatus.Active & "'|"
        Try
            intErrNo = objGLSetup.mtdGetMasterList(strOppCd_VehicleType_GET, _
                                                   strParam, _
                                                   objGLSetup.EnumGLMasterType.VehType, _
                                                   objDsVehType)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_VEHICLETYPE_LIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_VehDist.aspx")
        End Try

        For intCnt = 0 To objDsVehType.Tables(0).Rows.Count - 1
            objDsVehType.Tables(0).Rows(intCnt).Item("VehTypeCode") = Trim(objDsVehType.Tables(0).Rows(intCnt).Item("VehTypeCode"))
            objDsVehType.Tables(0).Rows(intCnt).Item("Description") = Trim(objDsVehType.Tables(0).Rows(intCnt).Item("VehTypeCode")) & " (" & Trim(objDsVehType.Tables(0).Rows(intCnt).Item("Description")) & ")"
        Next

        dr = objDsVehType.Tables(0).NewRow()
        dr("VehTypeCode") = ""
        dr("Description") = "Select " & lblVehTypeTag.Text
        objDsVehType.Tables(0).Rows.InsertAt(dr, 0)

        ddlVehType.DataSource = objDsVehType.Tables(0)
        ddlVehType.DataValueField = "VehTypeCode"
        ddlVehType.DataTextField = "Description"
        ddlVehType.DataBind()
    End Sub

    Protected Function onLoad_LineDisplay() As DataSet
        Dim intCnt As Integer
        Dim label As label
        Dim EditText As TextBox

        strParam = "||" & strLocation & "|"
        Try
            intErrNo = objBD.mtdGetVehTypeDist(strOppCd_GET, _
                                           strParam, _
                                           objDsVehDist)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_VEHDIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_VehDist.aspx")
        End Try

        dgVehDist.DataSource = objDsVehDist.Tables(0)
        dgVehDist.DataBind()

        Return objDsVehDist
    End Function

    Sub DEDR_Add(ByVal sender As Object, ByVal e As ImageClickEventArgs)

        strParam = Trim(ddlAct.SelectedItem.Value) & "|" & _
                   Trim(ddlVehType.SelectedItem.Value) & "|"
        Try
            intErrNo = objBD.mtdAddVehTypeDist(strOppCd_ADD, _
                                                strOppCd_GET, _
                                                strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strParam, _
                                                intError)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_SETUP_VEHDIST_ADD&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_VehDist.aspx")
        End Try

        If intError = objBD.EnumErrorType.duplicateKey Then
            lblDupMsg.Visible = True
            Exit Sub
        Else
            onLoad_LineDisplay()
        End If

    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
    End Sub

    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim DelButton As LinkButton

        dgVehDist.EditItemIndex = CInt(E.Item.ItemIndex)
        BindGrid()

        DelButton = dgVehDist.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lbDelete")
        DelButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"

    End Sub

    Sub BindGrid()
        Dim Period As String

        dgVehDist.DataSource = onLoad_LineDisplay()
        dgVehDist.DataBind()
        lblLocCode.Text = strLocation
        GetActivePeriod(Period)
        lblBgtPeriod.Text = Period

    End Sub

    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        dgVehDist.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)

        Dim strActCode As String
        Dim strVehTypeCode As String
        Dim Label As Label
        Dim intError As Integer

        Label = E.Item.FindControl("lblActCode")
        strActCode = Trim(Label.Text)
        Label = E.Item.FindControl("lblVehTypeCode")
        strVehTypeCode = Trim(Label.Text)

        strParam = strActCode & "|" & strVehTypeCode & "|"
        Try
            intErrNo = objBD.mtdDelVehTypeDist(strOppCd_DEL, _
                                                strParam, _
                                                intError)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=DEL_VEHDIST&errmesg=" & lblErrMessage.Text & "&redirect=BD/Setup/BD_setup_VehDist.aspx")
        End Try

        dgVehDist.EditItemIndex = -1
        onLoad_LineDisplay()
        BindGrid()

    End Sub

End Class
