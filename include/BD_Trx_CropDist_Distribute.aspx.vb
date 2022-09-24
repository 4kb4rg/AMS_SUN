

Imports System
Imports System.Data
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
Imports System.Math

Public Class BD_CropDist_Dist : Inherits Page

    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblOvrMsgTop As Label
    Protected WithEvents lblOvrMsg As Label
    Protected WithEvents lblPrcntErrTop As Label
    Protected WithEvents lblPrcntErr As Label
    Protected WithEvents lblFigureErrTop As Label
    Protected WithEvents lblFigureErr As Label
    Protected WithEvents MonthList As DataGrid
    Protected WithEvents SQLStatement As Label
    Protected WithEvents ddlDistribute As DropDownList
    Protected WithEvents StatusList As DropDownList
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents curStatus As Label
    Protected WithEvents lblYear As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblForecast As Label
    Protected WithEvents lblYield As Label
    Protected WithEvents lblDistFig As Label
    Protected WithEvents lblLocTag As Label
    Protected WithEvents lblTitle As Label
    Protected WithEvents lblBlkCode As Label
    Protected WithEvents lblSubBlkCode As Label 
    Protected WithEvents lblBlockTag As Label
    Protected WithEvents lblSubBlkTag As Label 
    Protected WithEvents lblCode As Label
    Protected WithEvents RowPlantingYr As HtmlTableRow
    Protected WithEvents RowSubBlk As HtmlTableRow 
    Protected WithEvents hidDistByBlk As HtmlInputHidden 
    Protected WithEvents hidBlkCode As HtmlInputHidden 

    Dim objBDTx As New agri.BD.clsTrx()
    Dim objBD As New agri.BD.clsSetup()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Dim strOppCd_SUM As String

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
            lblOvrMsgTop.Visible = False
            lblOvrMsg.Visible = False
            lblPrcntErrTop.Visible = False
            lblPrcntErr.Visible = False
            lblFigureErrTop.Visible = False
            lblFigureErr.Visible = False

            lblYear.Text = Request.QueryString("yr").Trim
            lblBlkCode.Text = Request.QueryString("blk").Trim
            lblSubBlkCode.Text = Request.QueryString("subblk").Trim
            hidDistByBlk.Value = Request.QueryString("DistByBlk").Trim 

            If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
                hidBlkCode.Value = lblBlkCode.Text
            Else
                hidBlkCode.Value = lblSubBlkCode.Text
            End If

            If Not Page.IsPostBack Then
                onload_GetLangCap()
                BindDistList()
                BindGrid()
                DisableRow()
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
            MonthList.Columns(2).Visible = False
        Else
            If hidDistByBlk.Value = True Then
                RowPlantingYr.Visible = False
                RowSubBlk.Visible = False
                MonthList.Columns(2).Visible = True
            Else
                RowSubBlk.Visible = True
                MonthList.Columns(2).Visible = False
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
            Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_CROPDIST_DISTRIBUTE_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropProd_Distribute.aspx?distbyblk=" & hidDistByBlk.Value & "&blk=" & lblBlkCode.Text & "&subblk=" & lblSubBlkCode.Text & "&yr=" & lblYear.Text)
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

    Sub BindDistList()
        ddlDistribute.Items.Add(New ListItem("Please select a method", ""))
        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
            ddlDistribute.Items.Add(New ListItem(objBDTx.mtdGetDistMethod(objBDTx.EnumDistMethod.Even), objBDTx.EnumDistMethod.Even))
            ddlDistribute.Items.Add(New ListItem(objBDTx.mtdGetDistMethod(objBDTx.EnumDistMethod.Figure), objBDTx.EnumDistMethod.Figure))
        Else
            If hidDistByBlk.Value = True Then
                ddlDistribute.Items.Add(New ListItem(objBDTx.mtdGetDistMethod(objBDTx.EnumDistMethod.EvenPercentage), objBDTx.EnumDistMethod.EvenPercentage))
            Else
                ddlDistribute.Items.Add(New ListItem(objBDTx.mtdGetDistMethod(objBDTx.EnumDistMethod.Even), objBDTx.EnumDistMethod.Even))
                ddlDistribute.Items.Add(New ListItem(objBDTx.mtdGetDistMethod(objBDTx.EnumDistMethod.Figure), objBDTx.EnumDistMethod.Figure))
            End If
        End If
        ddlDistribute.Items.Add(New ListItem(objBDTx.mtdGetDistMethod(objBDTx.EnumDistMethod.Percentage), objBDTx.EnumDistMethod.Percentage))

    End Sub

    Sub BindGrid()
        Dim Period As String

        MonthList.DataSource = LoadData()
        MonthList.DataBind()
        lblLocCode.Text = strLocation
        GetActivePeriod(Period)
        lblBgtPeriod.Text = Period
        LoadTotal()
    End Sub


    Protected Function LoadTotal() As  decimal 
        Dim dsTotals As DataSet

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
            strOppCd_SUM = "BD_CLSTRX_CROPPROD_SUM_GET"
            strParam = strLocation & "|" & GetActivePeriod("") & "||AND BD.BlkCode ='" & hidBlkCode.Value & "'|"
        Else
            If hidDistByBlk.Value = True Then
                strOppCd_SUM = "BD_CLSTRX_CROPPROD_DISTBYBLK_SBLK_SUM_GET"
                strParam = strLocation & "|" & GetActivePeriod("") & "||AND SBLK.BlkCode ='" & lblBlkCode.Text & "'|"
            Else
                strOppCd_SUM = "BD_CLSTRX_CROPPROD_SUM_GET"
                strParam = strLocation & "|" & GetActivePeriod("") & "||AND BD.BlkCode ='" & lblSubBlkCode.Text.Trim & "'|"
            End If
        End If

        Try
            intErrNo = objBDTx.mtdGetCropProd(strOppCd_SUM, strParam, dsTotals)
        Catch Exp As System.Exception
            Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_CROPDIST_DISTRIBUTE_GET_TOTAL&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropProd_Distribute.aspx?distbyblk=" & hidDistByBlk.Value & "&blk=" & lblBlkCode.Text & "&subblk=" & lblSubBlkCode.Text & "&yr=" & lblYear.Text)
        End Try


        lblDistFig.Text = objGlobal.GetIDDecimalSeparator(FormatNumber(Trim(dsTotals.Tables(0).Rows(0).Item("BudgetYield")), 0))
        Return Trim(dsTotals.Tables(0).Rows(0).Item("BudgetYield"))
        

    End Function

    Protected Function LoadData() As DataSet
        Dim strOppCd_GET As String = "BD_CLSTRX_CROPDIST_GET"

        strParam = strLocation & "|" & GetActivePeriod("") & "|" & hidBlkCode.Value & "||" & SortExpression.Text & " " & SortCol.Text


        Try
            intErrNo = objBDTx.mtdGetCropDist(strOppCd_GET, strParam, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_CROPDIST_DISTRIBUTE_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropProd_Distribute.aspx?distbyblk=" & hidDistByBlk.Value & "&blk=" & lblBlkCode.Text & "&subblk=" & lblSubBlkCode.Text & "&yr=" & lblYear.Text)
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
            Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_CROPDIST_DISTRIBUTE_GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropProd_Distribute.aspx?distbyblk=" & hidDistByBlk.Value & "&blk=" & lblBlkCode.Text & "&subblk=" & lblSubBlkCode.Text & "&yr=" & lblYear.Text)
        End Try

        If dsperiod.Tables(0).Rows.Count > 0 Then
            BGTPeriod = dsperiod.Tables(0).Rows(0).Item("BGTPeriod")
            Return dsperiod.Tables(0).Rows(0).Item("PeriodID")
        Else
            BGTPeriod = "No Active Period"
            Response.Redirect("../../BD/Setup/BD_setup_Periods.aspx")
        End If
    End Function

    Sub DataGrid_ItemDataBound(ByVal Sender As Object, ByVal e As DataGridItemEventArgs)

    End Sub

    Sub ddlDistributeSelect(ByVal Sender As Object, ByVal E As EventArgs)
        Dim txt As TextBox
        Dim decEvenFig As Decimal
        Dim decCount As Decimal
        Dim intcnt As Integer

        LoadData()
        Select Case ddlDistribute.SelectedItem.Value
            Case objBDTx.EnumDistMethod.EvenPercentage
                decEvenFig = 100 / objDataSet.Tables(0).Rows.Count
                For intcnt = 0 To objDataSet.Tables(0).Rows.Count - 1
                    If intcnt = objDataSet.Tables(0).Rows.Count - 1 Then
                        decEvenFig = 100 - decCount
                    End If
                    txt = MonthList.Items.Item(CInt(intcnt)).FindControl("TxFig")


                    txt.Text = objGlobal.GetIDDecimalSeparator(Round(decEvenFig, 0))
                    decCount += Round(decEvenFig, 0)
    
                    txt.Enabled = False
                Next
            Case objBDTx.EnumDistMethod.Even
                decEvenFig = LoadTotal() / objDataSet.Tables(0).Rows.Count
                For intcnt = 0 To objDataSet.Tables(0).Rows.Count - 1
                    If intcnt = objDataSet.Tables(0).Rows.Count - 1 Then
                        decEvenFig = LoadTotal() - decCount
                    End If
                    txt = MonthList.Items.Item(CInt(intcnt)).FindControl("TxFig")

                    txt.Text = objGlobal.GetIDDecimalSeparator(Round(decEvenFig, 0))
                    decCount += Round(decEvenFig, 0)
                    txt.Enabled = False
                Next
            Case objBDTx.EnumDistMethod.Percentage, objBDTx.EnumDistMethod.Figure
                For intcnt = 0 To objDataSet.Tables(0).Rows.Count - 1
                    txt = MonthList.Items.Item(CInt(intcnt)).FindControl("TxFig")
                    txt.Text = ""
                    txt.Enabled = True
                Next
        End Select
    End Sub

    Sub btnConfirm_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim strOppCd_Block_GET As String = "BD_CLSTRX_CROPDIST_BLOCK_CLICK_SBLK_GET"
        Dim strOppCd_CropDist_List_GET As String = "BD_CLSTRX_CROPDIST_GET"
        Dim strOppCd_UPD As String = "BD_CLSTRX_CROPDIST_UPD"
        Dim interror As Integer
        Dim strParam As String
        Dim decFig As Decimal
        Dim decFigCtrl As Decimal
        Dim txt As TextBox
        Dim lbl As Label
        Dim intcnt As Integer
        Dim dsIncrementalWgt As DataSet
        Dim blnIsLastRec As Boolean = False

        LoadData()

        For intcnt = 0 To objDataSet.Tables(0).Rows.Count - 1
            txt = MonthList.Items.Item(CInt(intcnt)).FindControl("TxFig")
            decFigCtrl += txt.Text
        Next

        Select Case ddlDistribute.SelectedItem.Value
            Case objBDTx.EnumDistMethod.Figure
                If Not decFigCtrl = CDec(lblDistFig.Text) Then
                    lblFigureErrTop.Visible = True
                    lblFigureErr.Visible = True
                    Exit Sub
                End If
            Case objBDTx.EnumDistMethod.Percentage
                If Not decFigCtrl = 100 Then
                    lblPrcntErrTop.Visible = True
                    lblPrcntErr.Visible = True
                    Exit Sub
                End If
        End Select

        For intcnt = 0 To objDataSet.Tables(0).Rows.Count - 1
            If intcnt = objDataSet.Tables(0).Rows.Count - 1 Then blnIsLastRec = True

            txt = MonthList.Items.Item(CInt(intcnt)).FindControl("TxFig")
            lbl = MonthList.Items.Item(CInt(intcnt)).FindControl("lblCropDistID")

            Select Case ddlDistribute.SelectedItem.Value
                Case objBDTx.EnumDistMethod.Even, objBDTx.EnumDistMethod.EvenPercentage, objBDTx.EnumDistMethod.Figure
                    If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
                        strOppCd_SUM = "BD_CLSTRX_CROPPROD_SUM_GET"
                        strParam = GetActivePeriod("") & "|" & hidBlkCode.Value & "|" & txt.Text.Trim & "||" & lbl.Text.Trim
                    Else
                        If hidDistByBlk.Value = True Then
                            strOppCd_SUM = "BD_CLSTRX_CROPPROD_DISTBYBLK_SBLK_SUM_GET"
                            strParam = GetActivePeriod("") & "|" & lblBlkCode.Text & "|" & txt.Text.Trim & "||" & intcnt
                        Else
                            strOppCd_SUM = "BD_CLSTRX_CROPPROD_SUM_GET"
                            strParam = GetActivePeriod("") & "|" & hidBlkCode.Value & "|" & txt.Text.Trim & "||" & lbl.Text.Trim
                        End If
                    End If
                Case objBDTx.EnumDistMethod.Percentage
                    If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
                        strOppCd_SUM = "BD_CLSTRX_CROPPROD_SUM_GET"
                        strParam = GetActivePeriod("") & "|" & hidBlkCode.Value & "||" & txt.Text.Trim & "|" & lbl.Text.Trim
                    Else
                        If hidDistByBlk.Value = True Then
                            strOppCd_SUM = "BD_CLSTRX_CROPPROD_DISTBYBLK_SBLK_SUM_GET"
                            strParam = GetActivePeriod("") & "|" & lblBlkCode.Text & "||" & txt.Text.Trim & "|" & intcnt
                        Else
                            strOppCd_SUM = "BD_CLSTRX_CROPPROD_SUM_GET"
                            strParam = GetActivePeriod("") & "|" & hidBlkCode.Value & "||" & txt.Text.Trim & "|" & lbl.Text.Trim
                        End If
                    End If

            End Select

            Try
                intErrNo = objBDTx.mtdUpdCropDist(strOppCd_UPD, _
                                                  strOppCd_SUM, _
                                                  strOppCd_Block_GET, _
                                                  strOppCd_CropDist_List_GET, _
                                                  strCompany, _
                                                  strLocation, _
                                                  strUserId, _
                                                  strParam, _
                                                  interror, _
                                                  hidDistByBlk.Value, _
                                                  objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting), _
                                                  False, dsIncrementalWgt, blnIsLastRec)
            Catch Exp As System.Exception
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_CROPDIST_DISTRIBUTE_UPD&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropDist_Distribute.aspx?distbyblk=" & hidDistByBlk.Value & "&blk=" & lblBlkCode.Text & "&subblk=" & lblSubBlkCode.Text & "&yr=" & lblYear.Text)
            End Try

            If interror = objBD.EnumErrorType.Overflow Then
                lblOvrMsgTop.Visible = True
                lblOvrMsg.Visible = True
            End If
        Next

        If intErrNo = 0 Then
            Response.Write("<Script Language=""JavaScript"">opener.location.href='BD_trx_CropDist_Details.aspx?distbyblk=" & hidDistByBlk.Value & "&yr=" & lblYear.Text & "&blk=" & lblBlkCode.Text & "&subblk=" & lblSubBlkCode.Text & "';window.close();</Script>")
        End If

    End Sub

    Sub btnCancel_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Response.Write("<Script Language=""JavaScript"">window.close();</Script>")
    End Sub


End Class
