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

Public Class BD_CropProd_Det_DistByBlock : Inherits Page

    Protected WithEvents dgCropProdByBlk As DataGrid
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents lblYear As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblOvrMsg As Label
    Protected WithEvents lblForecast As Label
    Protected WithEvents lblBGT As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents lblBlock As Label
    Protected WithEvents lblLocTag As Label
    Protected WithEvents lblTitle As Label
    Protected WithEvents ibDistByBlock As ImageButton

    Dim objBDTx As New agri.BD.clsTrx()
    Dim objBD As New agri.BD.clsSetup()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Dim strOppCd_Bgt_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"
    Dim strOppCd_GET As String

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
            If Not Page.IsPostBack Then
                onload_GetLangCap()
                lblYear.Text = Request.QueryString("Yr")

                BindGrid()
            End If

            If SortExpression.Text = "" Then
                SortExpression.Text = "BlkCode"
                SortCol.Text = "ASC"
            End If
        End If
        lblOvrMsg.Visible = False
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()

        lblLocTag.Text = GetCaption(objLangCap.EnumLangCap.Location) & lblCode.Text
        lblBlock.Text = GetCaption(objLangCap.EnumLangCap.Block)
        dgCropProdByBlk.Columns(0).HeaderText = lblBlock.Text & lblCode.Text
        lblTitle.Text = "CROP PRODUCTION - DISTRIBUTE BY " & UCase(lblBlock.Text)

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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BD_TRX_CROPPROD_DETAILS_DISTBYBLK_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropProd_YearList.aspx")
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
        dgCropProdByBlk.CurrentPageIndex = 0
        dgCropProdByBlk.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub BindGrid()
        Dim PageNo As Integer
        Dim Period As String

        dgCropProdByBlk.DataSource = LoadData()
        dgCropProdByBlk.DataBind()
        lblLocCode.Text = strLocation
        GetActivePeriod(Period)
        lblBgtPeriod.Text = Period
        LoadTotal()
    End Sub


    Protected Sub LoadTotal()
        Dim dsTotals As DataSet
        Dim strOppCd_SUM As String = "BD_CLSTRX_CROPPROD_SUM_DISTBYBLOCK_GET"

        strParam = lblYear.Text & "|" & strLocation & "|" & Request.QueryString("periodid") & "|"
        Try
            intErrNo = objBDTx.mtdGetCropProd_DistByBlock(strOppCd_SUM, strParam, dsTotals)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_CROPPROD_DISTBYBLOCK_TOTAL_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropProd_Details.aspx")
        End Try

        lblForecast.Text = FormatNumber(Trim(dsTotals.Tables(0).Rows(0).Item("ForecastYield")), 2)
        lblBGT.Text = FormatNumber(Trim(dsTotals.Tables(0).Rows(0).Item("YieldPerArea")), 2)

    End Sub

    Protected Function LoadData() As DataSet
        Dim Period As String

        strOppCd_GET = "BD_CLSTRX_CROPPROD_DISTBYBLOCK_BLK_GET"

        strParam = lblYear.Text & "|" & strLocation & "|" & Request.QueryString("periodid") & "|"

        Try
            intErrNo = objBDTx.mtdGetCropProd_DistByBlock(strOppCd_GET, strParam, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_CROPPROD_DET_DISTBYBLK_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropProd_YearList.aspx")
        End Try

        Return objDataSet
    End Function

    Protected Function GetActivePeriod(ByRef BGTPeriod As String) As String
        Dim dsperiod As New DataSet()

        strParam = "|||||" & objBD.EnumPeriodStatus.Active & "|" & strLocation & "|"
        Try
            intErrNo = objBD.mtdGetPeriodList(strOppCd_Bgt_GET, strParam, dsperiod)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_DET_DISTBYBLK_GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropProd_YearList.aspx")
        End Try

        If dsperiod.Tables(0).Rows.Count > 0 Then
            BGTPeriod = dsperiod.Tables(0).Rows(0).Item("BGTPeriod")
            Return dsperiod.Tables(0).Rows(0).Item("PeriodID")
        Else
            BGTPeriod = "No Active Period"
            Response.Redirect("../../BD/Setup/BD_setup_Periods.aspx")
        End If
    End Function

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIf(SortCol.Text = "ASC", "DESC", "ASC")
        BindGrid()
    End Sub

    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        dgCropProdByBlk.EditItemIndex = CInt(E.Item.ItemIndex)
        BindGrid()
    End Sub

    Sub DEDR_Update(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strOppCd_UPD As String = "BD_CLSTRX_CROPPROD_UPD"
        Dim strOppCd_SubBlock_Get As String = "BD_CLSTRX_CROPPROD_SUBBLOCK_GET"
        Dim strOppCd_CropProd_Get As String = "BD_CLSTRX_CROPPROD_GET"
        Dim EditText As TextBox
        Dim label As label
        Dim intError As Integer
        Dim strBlkCode As String
        Dim strBlkTotalArea As String
        Dim strCurrBgt As String
        Dim strNextBgt As String

        label = E.Item.FindControl("lblBlkCode")
        strBlkCode = label.Text
        label = E.Item.FindControl("lblBlkTotalArea")
        strBlkTotalArea = label.Text
        EditText = E.Item.FindControl("txtCurr")
        strCurrBgt = EditText.Text
        EditText = E.Item.FindControl("txtNextYield")
        strNextBgt = EditText.Text

        strParam = GetActivePeriod("") & "|" & _
                   lblYear.Text & "|" & _
                   strBlkCode & "|" & _
                   strCurrBgt & "|" & _
                   strNextBgt & "|" & _
                   strBlkTotalArea & "|"
        Try
            intErrNo = objBDTx.mtdUpdCropProd_DistByBlock(strOppCd_UPD, _
                                                          strOppCd_SubBlock_Get, _
                                                          strOppCd_CropProd_Get, _
                                                          strOppCd_Bgt_GET, _
                                                          strCompany, _
                                                          strLocation, _
                                                          strUserId, _
                                                          strParam, _
                                                          intError)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_CROPPROD_DET_DISTBYBLOCK_UPD&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropProd_YearList.aspx")
        End Try

        If intError = objBD.EnumErrorType.Overflow Then
            lblOvrMsg.Visible = True
        Else
            dgCropProdByBlk.EditItemIndex = E.Item.ItemIndex + 1
            BindGrid()
        End If
    End Sub

    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        If CInt(E.Item.ItemIndex) = 0 And dgCropProdByBlk.Items.Count = 1 And dgCropProdByBlk.PageCount <> 1 Then
            dgCropProdByBlk.CurrentPageIndex = dgCropProdByBlk.PageCount - 2
        End If

        dgCropProdByBlk.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub BackBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Write("<Script Language=""JavaScript"">opener.location.href='BD_trx_CropProd_Details.aspx?yr=" & lblYear.Text & "';window.close();</Script>")
    End Sub

End Class
