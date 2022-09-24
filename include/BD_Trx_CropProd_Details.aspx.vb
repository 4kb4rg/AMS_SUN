
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

Public Class BD_CropProd_Det : Inherits Page

    Protected WithEvents dgCropProd As DataGrid
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents lblYear As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblBgtPeriod As Label
    Protected WithEvents lblOvrMsgTop As Label
    Protected WithEvents lblOvrMsg As Label
    Protected WithEvents lblAvrgstand As Label
    Protected WithEvents lblPlanted As Label
    Protected WithEvents lblHist1 As Label
    Protected WithEvents lblHist2 As Label
    Protected WithEvents lblCurr As Label
    Protected WithEvents lblForecast As Label
    Protected WithEvents lblYield As Label
    Protected WithEvents lblBGT As Label
    Protected WithEvents lblFFB As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents lblBlock As Label
    Protected WithEvents lblSubBlock As Label
    Protected WithEvents lblLocTag As Label
    Protected WithEvents lblTitle As Label
    Protected WithEvents btnDistByBlock As Button
    Protected WithEvents lblBgtStatus As Label 

    Dim objBDTx As New agri.BD.clsTrx()
    Dim objBD As New agri.BD.clsSetup()
    Dim objGLSetup As New agri.GL.clsSetup()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Dim strOppCd_Bgt_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"
    Dim strOppCd_ActualYield_GET As String = "BD_CLSTRX_CROPPROD_ACTUALYIELD_GET"
    Dim strOppCd_GET As String = "BD_CLSTRX_CROPPROD_GET"
    Dim strOppCd_Block_Get As String

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
            If SortExpression.Text = "" Then
                If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
                    SortExpression.Text = "BlkCode"
                Else
                    SortExpression.Text = "OriBlkCode"
                End If
                SortCol.Text = "ASC"
            End If

            If Not Page.IsPostBack Then
                onload_GetLangCap()
                lblYear.Text = Request.QueryString("Yr")
                BindGrid()
            End If
        End If

        lblOvrMsgTop.Visible = False
        lblOvrMsg.Visible = False
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()

        lblTitle.Text = "CROP PRODUCTION"
        lblLocTag.Text = GetCaption(objLangCap.EnumLangCap.Location) & lblCode.Text
        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
            lblBlock.Text = GetCaption(objLangCap.EnumLangCap.Block)

            dgCropProd.Columns(0).Visible = False
            dgCropProd.Columns(1).HeaderText = lblBlock.Text & lblCode.Text

        Else
            lblBlock.Text = GetCaption(objLangCap.EnumLangCap.Block)
            lblSubBlock.Text = GetCaption(objLangCap.EnumLangCap.SubBlock)

            dgCropProd.Columns(0).HeaderText = lblBlock.Text & lblCode.Text
            dgCropProd.Columns(1).HeaderText = lblSubBlock.Text & lblCode.Text
            btnDistByBlock.Text = btnDistByBlock.Text & lblBlock.Text
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
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_CROPPROD_DETAILS_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropProd_YearList.aspx")
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
        dgCropProd.CurrentPageIndex = 0
        dgCropProd.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub BindGrid()
        Dim PageNo As Integer
        Dim Period As String

        dgCropProd.DataSource = LoadData()
        dgCropProd.DataBind()
        lblLocCode.Text = strLocation
        GetActivePeriod(Period)
        lblBgtPeriod.Text = Period
        LoadTotal()
    End Sub

    Protected Sub LoadTotal()
        Dim dsTotals As DataSet
        Dim strOppCd_SUM As String = "BD_CLSTRX_CROPPROD_SUM_GET"


        strParam = strLocation & "|" & _
                    GetActivePeriod("") & "|" & _
                    lblYear.Text & "||"
        Try
            intErrNo = objBDTx.mtdGetCropProd(strOppCd_SUM, strParam, dsTotals)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_CROPPROD_TOTAL_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropProd_YearList.aspx")
        End Try

        With dsTotals.Tables(0).Rows(0)

            lblAvrgstand.Text = objGlobal.GetIDDecimalSeparator(Round(.Item("AvrgStand"), 0))
            lblPlanted.Text = objGlobal.GetIDDecimalSeparator(Round(.Item("PlantedArea"), 0))
            lblHist1.Text = objGlobal.GetIDDecimalSeparator(Round(.Item("YieldHist1"), 0))
            lblHist2.Text = objGlobal.GetIDDecimalSeparator(Round(.Item("YieldHist2"), 0))
            lblCurr.Text = objGlobal.GetIDDecimalSeparator(Round(.Item("YieldToMonth"), 0))
            lblForecast.Text = objGlobal.GetIDDecimalSeparator(Round(.Item("ForecastYield"), 0))
            lblYield.Text = objGlobal.GetIDDecimalSeparator(Round(.Item("YieldHist3"), 0))
            lblBGT.Text = objGlobal.GetIDDecimalSeparator(Round(.Item("YieldPerArea"), 0))
            lblFFB.Text = objGlobal.GetIDDecimalSeparator(Round(.Item("BudgetYield"), 0))
        End With
        
    End Sub

    Protected Function LoadData() As DataSet
        Dim strOppCd_GET As String
        Dim Period As String

        CheckAreaDiff()

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
            btnDistByBlock.Visible = False
            strOppCd_GET = "BD_CLSTRX_CROPPROD_GET"
        Else
            btnDistByBlock.Visible = True
            strOppCd_GET = "BD_CLSTRX_CROPPROD_SBLK_GET"
        End If

        strParam = strLocation & "|" & _
                    GetActivePeriod("") & "|" & _
                    lblYear.Text & "||" & _
                    SortExpression.Text & " " & SortCol.Text
        Try
            intErrNo = objBDTx.mtdGetCropProd(strOppCd_GET, strParam, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_CROPPROD_DET_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropProd_YearList.aspx")
        End Try



        Return objDataSet
    End Function

    Protected Function GetActivePeriod(ByRef BGTPeriod As String) As String
        Dim dsperiod As New DataSet

        strParam = "|||||" & objBD.EnumPeriodStatus.Active & "|" & strLocation & "|"
        Try
            intErrNo = objBD.mtdGetPeriodList(strOppCd_Bgt_GET, strParam, dsperiod)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropProd_YearList.aspx")
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

    Sub DataGrid_ItemDataBound(ByVal Sender As Object, ByVal e As DataGridItemEventArgs)
        Dim lbl As Label
        Dim btn As LinkButton

        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            lbl = e.Item.FindControl("lblForecastYield")
            If lbl.Text = "0.00" Then
                lbl.Visible = False
                lbl = e.Item.FindControl("lblCurrYield")
                lbl.Visible = False
            End If
            lbl = e.Item.FindControl("lblBGTYield")
            If lbl.Text = "0.00" Then
                lbl.Visible = False
                lbl = e.Item.FindControl("lblFFB")
                lbl.Visible = False
            End If
            If lblBgtStatus.Text.Trim = objBD.EnumPeriodStatus.Addvote Then
                btn = e.Item.FindControl("Edit")
                btn.Visible = False
            End If
        End If

    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIf(SortCol.Text = "ASC", "DESC", "ASC")
        BindGrid()
    End Sub

    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        dgCropProd.EditItemIndex = CInt(E.Item.ItemIndex)
        BindGrid()
    End Sub

    Sub CheckAreaDiff()
        Dim strOppCd_BLK_GET As String = "GL_CLSSETUP_BLOCK_GET_BY_BLKCODE"
        Dim strOppCd_SUBBLK_GET As String = "GL_CLSSETUP_BLOCK_GET_BY_SUBBLKCODE"
        Dim strOppCd_UPD As String = "BD_CLSTRX_CROPPROD_UPD"
        Dim dsCropProd As New DataSet
        Dim dsArea As New DataSet
        Dim blnBlkYieldLevel As Boolean
        Dim decPlantAreaSize As Decimal
        Dim decSPH As Decimal
        Dim decPlantArea As Decimal
        Dim decStdPerArea As Decimal
        Dim intCnt As Integer
        Dim intError As Integer
        Dim strBlkCode As String
        Dim strCropType As String
        Dim strBlkStatus As String

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
            strOppCd_Block_Get = "BD_CLSTRX_CROPPROD_BLOCK_GET"
            strCropType = objGLSetup.EnumBlockType.MatureField
            strBlkStatus = objGLSetup.EnumBlockStatus.Active
            blnBlkYieldLevel = True
        Else
            strOppCd_Block_Get = "BD_CLSTRX_CROPPROD_SUBBLOCK_GET"
            strCropType = objGLSetup.EnumSubBlockType.MatureField
            strBlkStatus = objGLSetup.EnumSubBlockStatus.Active
            blnBlkYieldLevel = False
        End If

        strParam = strLocation & "|" & _
                   GetActivePeriod("") & "|" & _
                   lblYear.Text & "||" & _
                   SortExpression.Text & " " & SortCol.Text
        Try
            intErrNo = objBDTx.mtdGetCropProd(strOppCd_GET, strParam, dsCropProd)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_CROPPROD_DET_CHECHAREADIFF_GET&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropProd_YearList.aspx")
        End Try

        For intCnt = 0 To dsCropProd.Tables(0).Rows.Count - 1
            strBlkCode = dsCropProd.Tables(0).Rows(intCnt).Item("BlkCode")
            decPlantAreaSize = dsCropProd.Tables(0).Rows(intCnt).Item("PlantedArea")
            decSPH = dsCropProd.Tables(0).Rows(intCnt).Item("SPH")

            strParam = strBlkCode.Trim & "|||"
            If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
                Try
                    intErrNo = objGLSetup.mtdGetBlock(strOppCd_BLK_GET, strLocation, strParam, dsArea, True)
                Catch Exp As System.Exception
                    Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_CROPPROD_DET_CHECKAREADIFF_GL_BLK&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropProd_YearList.aspx")
                End Try
            Else
                Try
                    intErrNo = objGLSetup.mtdGetSubBlock(strOppCd_SUBBLK_GET, strLocation, strParam, dsArea, True)
                Catch Exp As System.Exception
                    Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_CROPPROD_DET_CHECKAREADIFF_GL_SUBBLK&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropProd_YearList.aspx")
                End Try
            End If

            If dsArea.Tables(0).Rows.Count > 0 Then
                decPlantArea = dsArea.Tables(0).Rows(0).Item("TotalArea")
                decStdPerArea = dsArea.Tables(0).Rows(0).Item("StdPerArea")
            Else
                decPlantArea = 0
                decStdPerArea = 0
            End If

            If decPlantArea <> decPlantAreaSize Or decStdPerArea <> decSPH Then
                strParam = GetActivePeriod("") & "|" & _
                           lblYear.Text & "|" & _
                           strBlkCode.Trim & "|||" & _
                           decPlantArea & "|" & _
                           decStdPerArea & "|" & _
                           strCropType.Trim & "|" & _
                           strBlkStatus.Trim & "|" & _
                           strLocation & "|"

                Try
                    intErrNo = objBDTx.mtdUpdCropProd(strOppCd_UPD, _
                                                      strOppCd_GET, _
                                                      strOppCd_Block_Get, _
                                                      strOppCd_ActualYield_GET, _
                                                      strOppCd_Bgt_GET, _
                                                      strCompany, _
                                                      strLocation, _
                                                      strUserId, _
                                                      strParam, _
                                                      intError, _
                                                      blnBlkYieldLevel)
                Catch Exp As System.Exception
                    Response.Redirect("../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_CROPPROD_DET_CHECKAREADIFF_UPD&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropProd_YearList.aspx")
                End Try
            End If
        Next

    End Sub

    Sub DEDR_Update(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strOppCd_UPD As String = "BD_CLSTRX_CROPPROD_UPD"
        Dim EditText As TextBox
        Dim label As Label
        Dim list As DropDownList
        Dim lblMsg As Label
        Dim intError As Integer
        Dim strBlkCode As String
        Dim strCurrBgt As String
        Dim strNextBgt As String
        Dim blnBlkYieldLevel As Boolean

        label = E.Item.FindControl("lblBlkCode")
        strBlkCode = label.Text
        EditText = E.Item.FindControl("txtCurr")
        strCurrBgt = EditText.Text
        EditText = E.Item.FindControl("txtNextYield")
        strNextBgt = EditText.Text

        If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockYieldLevel), intConfigsetting) = True Then
            strOppCd_Block_Get = "BD_CLSTRX_CROPPROD_BLOCK_GET"
            blnBlkYieldLevel = True
        Else
            strOppCd_Block_Get = "BD_CLSTRX_CROPPROD_SUBBLOCK_GET"
            blnBlkYieldLevel = False
        End If

        strParam = GetActivePeriod("") & "|" & _
                    lblYear.Text & "|" & _
                    strBlkCode & "|" & _
                    strCurrBgt & "|" & _
                    strNextBgt & "||||||"
        Try
            intErrNo = objBDTx.mtdUpdCropProd(strOppCd_UPD, _
                                              strOppCd_GET, _
                                              strOppCd_Block_Get, _
                                              strOppCd_ActualYield_GET, _
                                              strOppCd_Bgt_GET, _
                                              strCompany, _
                                              strLocation, _
                                              strUserId, _
                                              strParam, _
                                              intError, _
                                              blnBlkYieldLevel)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_CROPPROD_UPD&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_CropProd_YearList.aspx")
        End Try

        If intError = objBD.EnumErrorType.Overflow Then
            lblOvrMsgTop.Visible = True
            lblOvrMsg.Visible = True
        Else
            dgCropProd.EditItemIndex = E.Item.ItemIndex + 1
            BindGrid()
        End If
    End Sub

    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        If CInt(E.Item.ItemIndex) = 0 And dgCropProd.Items.Count = 1 And dgCropProd.PageCount <> 1 Then
            dgCropProd.CurrentPageIndex = dgCropProd.PageCount - 2
        End If
        dgCropProd.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub btnDistByBlock_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Write("<Script Language=""JavaScript"">pop_DistByBlk=window.open(""../../BD/Trx/BD_Trx_CropProd_Det_DistByBlock.aspx?yr=" & lblYear.Text.Trim & "&periodid=" & GetActivePeriod("") & _
                       """, null ,""'pop_DistByBlk',width=400,height=500,top=100,left=250,status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");pop_DistByBlk.focus();</Script>")

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

End Class
