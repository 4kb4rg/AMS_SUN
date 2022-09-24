Imports System
Imports System.Data
Imports System.Math
Imports System.Collections
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic

Public Class BD_ManuringFertUsg_Det : Inherits Page

    Protected WithEvents dgFertUsgLine As DataGrid
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents lblCode As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblPeriodErr As Label
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblLocTag As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblYearPlanted As Label
    Protected WithEvents lblFertCode As Label

    Protected WithEvents txtPrice As TextBox
    Protected WithEvents lblAccMonth As Label
    Protected WithEvents lblAccYear As Label
    Protected WithEvents lblQty As Label
    Protected WithEvents lblPrice As Label
    Protected WithEvents lblTotalCost As Label
    Protected WithEvents lblTotalQty As Label
    Protected WithEvents lblCostPerArea As Label
    Protected WithEvents lblCostPerMT As Label
    Protected WithEvents lblBgtStatus As Label 
    Protected WithEvents Calculate As ImageButton  
    Protected WithEvents Save As ImageButton 

    Dim objBD As New agri.BD.clsTrx()
    Dim objBDSetup As New agri.BD.clsSetup()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Dim strOppCd_GET As String = "BD_CLSTRX_CROPDIST_GET"
    Dim strOppCd_UPD As String = "BD_CLSTRX_CROPPROD_UPD"
    Dim strOppCdCropDist_SUM As String = "BD_CLSTRX_CROPDIST_SUM_GET"

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
            onload_GetLangCap()
            If Not Page.IsPostBack Then
                lblYearPlanted.Text = Request.QueryString("Yr")
                lblFertCode.Text = Request.QueryString("fert")

                BindGrid()
            End If

            If SortExpression.Text = "" Then
            End If
            If lblBgtStatus.Text.Trim = objBDSetup.EnumPeriodStatus.Addvote Then
                Calculate.Visible = False
                Save.Visible = False
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BD_TRX_MANURINGFERTUSG_DETAILS_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_ManuringFertUsg_Details.aspx?code=" & lblFertCode.Text & "&yr=" & lblYearPlanted.Text)
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

        dgFertUsgLine.DataSource = LoadData()
        dgFertUsgLine.DataBind()
        lblLocCode.Text = strLocation
        GetActivePeriod(Period)
        lblBgtPeriod.Text = Period

    End Sub

    Protected Function LoadData() As DataSet
        Dim strOpCd_ManuringFertUsgLn_GET As String = "BD_CLSTRX_MANURINGFERTUSGLN_GET"
        Dim strOpCd_ManuringFertUsgLn_Qty_SUM As String = "BD_CLSTRX_MANURINGFERTUSGLN_QTY_SUM"
        Dim dsQty As New DataSet()
        Dim strParamQty As String
        Dim strMth As String
        Dim strYr As String
        Dim intCnt As Integer

        strParam = lblFertCode.Text & "|" & lblYearPlanted.Text & "|" & GetActivePeriod("") & "|||" & strLocation & "|"
        Try
            intErrNo = objBD.mtdGetManuringFertUsgLn(strOpCd_ManuringFertUsgLn_GET, _
                                                     strParam, _
                                                     objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_MANURINGFERTUSGLN&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_ManuringFertUsg_Details.aspx?code=" & lblFertCode.Text & "&yr=" & lblYearPlanted.Text)
        End Try

        For intCnt = 0 To objDataSet.Tables(0).Rows.Count - 1
            strMth = Trim(objDataSet.Tables(0).Rows(intCnt).Item("AccMonth"))
            strYr = Trim(objDataSet.Tables(0).Rows(intCnt).Item("AccYear"))

            strParamQty = lblFertCode.Text & "|" & lblYearPlanted.Text & "|" & GetActivePeriod("") & "|" & strMth & "|" & strYr & "|" & strLocation & "|"
            Try
                intErrNo = objBD.mtdGetManuringFertUsgLn(strOpCd_ManuringFertUsgLn_Qty_SUM, _
                                                         strParamQty, _
                                                         dsQty)
            Catch Exp As System.Exception
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_MANURINGFERTUSGLN_QTY_SUM&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_ManuringFertUsg_Details.aspx?code=" & lblFertCode.Text & "&yr=" & lblYearPlanted.Text)
            End Try

            objDataSet.Tables(0).Rows(intCnt).Item("Qty") = dsQty.Tables(0).Rows(0).Item("Qty")
        Next

        Return objDataSet
    End Function

    Protected Function GetActivePeriod(ByRef BGTPeriod As String) As String
        Dim strOppCd_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"

        strParam = "|||||" & objBDSetup.EnumPeriodStatus.Active & "|" & strLocation & "|"

        Try
            intErrNo = objBDSetup.mtdGetPeriodList(strOppCd_GET, strParam, dsperiod)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_ManuringFertUsg_Details.aspx?code=" & lblFertCode.Text & "&yr=" & lblYearPlanted.Text)
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

    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIf(SortCol.Text = "ASC", "DESC", "ASC")
        BindGrid()
    End Sub

    Sub Button_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim strCmdArgs As String = CType(sender, ImageButton).CommandArgument
        Dim strOpCd_ManuringFertUsg_GET As String = "BD_CLSTRX_MANURINGFERTUSG_GET"
        Dim strOpCd_ManuringFertUsg_UPD As String = "BD_CLSTRX_MANURINGFERTUSG_UPD"
        Dim strOpCd_ManuringFertUsgLn_UPD As String = "BD_CLSTRX_MANURINGFERTUSGLN_UPD"
        Dim strManuringFertUsgLnID As String
        Dim strManuringFertUsgID As String
        Dim strParamLn As String

        Dim lb As Label
        Dim intDgFert As Integer
        Dim decPrice As Decimal
        Dim decQty As Decimal
        Dim decPlantedArea As Decimal

        Dim decCost As Decimal
        Dim decTotalCost As Decimal
        Dim decTotalQty As Decimal
        Dim decCostPerArea As Decimal

        decPrice = Trim(txtPrice.Text)

        lb = dgFertUsgLine.Items(0).FindControl("lblManuringFertUsgID")
        strManuringFertUsgID = Trim(lb.Text)
        lb = dgFertUsgLine.Items(0).FindControl("lblPlantedArea")
        decPlantedArea = Trim(lb.Text)

        For intDgFert = 0 To dgFertUsgLine.Items.Count - 1
            lb = dgFertUsgLine.Items(intDgFert).FindControl("lblManuringFertUsgLnID")
            strManuringFertUsgLnID = Trim(lb.Text)

            lb = dgFertUsgLine.Items(intDgFert).FindControl("lblQty")
            decQty = Trim(lb.Text)
            lb = dgFertUsgLine.Items(intDgFert).FindControl("lblOriCost")
            

            lb.Text = objGlobal.GetIDDecimalSeparator(FormatNumber((decQty * decPrice), 0))
            decCost = decQty * decPrice
            decTotalCost += decCost
            decTotalQty += decQty

        Next

        If (decPlantedArea = 0 And decTotalCost <> 0) Or (decPlantedArea = 0 And decTotalCost = 0) Then
            decCostPerArea = 0
        Else
            decCostPerArea = decTotalCost / decPlantedArea
        End If



        lblTotalCost.Text = objGlobal.GetIDDecimalSeparator(FormatNumber(decTotalCost, 0))
        lblTotalQty.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(decTotalQty, 5),5)
        lblCostPerArea.Text = objGlobal.GetIDDecimalSeparator(FormatNumber(decCostPerArea, 0))
        lblCostPerMT.Text = objGlobal.GetIDDecimalSeparator(FormatNumber(decPrice, 0))
        If strCmdArgs = "save" Then

            For intDgFert = 0 To dgFertUsgLine.Items.Count - 1
                lb = dgFertUsgLine.Items(intDgFert).FindControl("lblManuringFertUsgLnID")
                strManuringFertUsgLnID = Trim(lb.Text)

                lb = dgFertUsgLine.Items(intDgFert).FindControl("lblQty")
                decQty = Trim(lb.Text)
                lb = dgFertUsgLine.Items(intDgFert).FindControl("lblOriCost")

                lb.Text = objGlobal.GetIDDecimalSeparator(FormatNumber((decQty * decPrice), 0))

                decCost = decQty * decPrice
                strParamLn = strManuringFertUsgLnID & "|" & decCost & "|"
                Try
                    intErrNo = objBD.mtdUpdManuringFertUsgLine(strOpCd_ManuringFertUsgLn_UPD, _
                                                               strCompany, _
                                                               strLocation, _
                                                               strUserId, _
                                                               strParamLn)

                Catch Exp As System.Exception
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=MANURINGFERTUSG_LINE_UPD&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_ManuringFertUsg_Details.aspx?code=" & lblFertCode.Text & "&yr=" & lblYearPlanted.Text)
                End Try
            Next

            strParam = strManuringFertUsgID & "|" & _
                       Trim(txtPrice.Text) & "|" & _
                       Trim(decTotalCost) & "|" & _
                       Trim(decTotalQty) & "|" & _
                       Trim(decCostPerArea) & "|" & _
                       Trim(txtPrice.Text) & "|" & _
                       objBD.EnumManuringFertUsgStatus.Budgeted & "|"
            Try
                intErrNo = objBD.mtdUpdManuringFertUsg(strOpCd_ManuringFertUsg_UPD, _
                                                       strCompany, _
                                                       strLocation, _
                                                       strUserId, _
                                                       strParam)

            Catch Exp As System.Exception
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=MANURINGFERTUSG_UPD&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_ManuringFertUsg_Details.aspx?code=" & lblFertCode.Text & "&yr=" & lblYearPlanted.Text)
            End Try

        End If
    End Sub

    Sub BackBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("BD_trx_ManuringFertUsg_List.aspx?code=" & lblFertCode.Text & "&yr=" & lblYearPlanted.Text)
    End Sub


End Class
