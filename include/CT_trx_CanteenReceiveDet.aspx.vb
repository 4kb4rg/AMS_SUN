

Imports System
Imports System.Data
Imports System.Math
Imports System.Text
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic


Public Class CT_CanteenReceive : Inherits Page

    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents lblPleaseSelect As Label
    Protected WithEvents lblSelect As Label
    Protected WithEvents lblError As Label
    Protected WithEvents lblstock As Label
    Protected WithEvents lblUnDel As Label
    Protected WithEvents lblQty As Label
    Protected WithEvents lblPR As Label
    Protected WithEvents lblTxError As Label
    Protected WithEvents lblStatusHid As Label
    Protected WithEvents lblErrQty As Label
    Protected WithEvents lblErrUnitCost As Label
    Protected WithEvents lblErrTotalAmt As Label

    Protected WithEvents dgCanteenTx As DataGrid
    Protected WithEvents lblCanteenTxID As Label
    Protected WithEvents Status As Label
    Protected WithEvents CreateDate As Label
    Protected WithEvents UpdateDate As Label
    Protected WithEvents UpdateBy As Label
    Protected WithEvents lblTotAmtFig As Label
    Protected WithEvents lblAccTag As Label
    Protected WithEvents lblDate As Label
    Protected WithEvents lblFmt As Label

    Protected WithEvents txtRemarks As TextBox
    Protected WithEvents lblBlkTag As Label
    Protected WithEvents lblVehTag As Label
    Protected WithEvents lblVehExpTag As Label
    Protected WithEvents txtDate As TextBox
    Protected WithEvents txtQty As TextBox
    Protected WithEvents txtCost As TextBox
    Protected WithEvents txtAmount As TextBox
    Protected WithEvents txtRefNo As TextBox
    Protected WithEvents tblAdd As HtmlTable
    Protected WithEvents tblPR As HtmlTable
    Protected WithEvents RowAcc As HtmlTableRow
    Protected WithEvents RowBlk As HtmlTableRow
    Protected WithEvents RowVeh As HtmlTableRow
    Protected WithEvents RowVehExp As HtmlTableRow
    Protected WithEvents RowFromLoc As HtmlTableRow
    Protected WithEvents Add As ImageButton
    Protected WithEvents PRDelete As ImageButton
    Protected WithEvents Save As ImageButton
    Protected WithEvents Confirm As ImageButton
    Protected WithEvents btnNew As ImageButton
    Protected WithEvents btnSelDate As Image
    Protected WithEvents lstAccCode As DropDownList
    Protected WithEvents lstItem As DropDownList
    Protected WithEvents lstBlock As DropDownList
    Protected WithEvents lstVehCode As DropDownList
    Protected WithEvents lstVehExp As DropDownList
    Protected WithEvents lstRecDoc As DropDownList
    Protected WithEvents lstFromLoc As DropDownList
    Protected WithEvents lstPR As DropDownList
    Protected WithEvents lblItemCodeErr As Label
    Protected WithEvents lblAccCodeErr As Label
    Protected WithEvents lblBlockErr As Label
    Protected WithEvents lblVehCodeErr As Label
    Protected WithEvents lblVehExpCodeErr As Label
    Protected WithEvents lblConfirmErr As Label
    Protected WithEvents FindCT As HtmlInputButton
    Protected WithEvents Find As HtmlInputButton
    Protected WithEvents rfvFromLocCode As RequiredFieldValidator
    Protected WithEvents RowChargeLevel As HtmlTableRow
    Protected WithEvents RowPreBlk As HtmlTableRow
    Protected WithEvents lblPreBlockErr As Label
    Protected WithEvents lblPreBlkTag As Label
    Protected WithEvents lstPreBlock As DropDownList
    Protected WithEvents lstChargeLevel As DropDownList
    Protected WithEvents hidBlockCharge As HtmlInputHidden
    Protected WithEvents hidChargeLocCode As HtmlInputHidden
    Dim PreBlockTag As String


    Protected objCTtx As New agri.CT.clsTrx()
    Protected objINtx As New agri.IN.clsTrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objINstp As New agri.IN.clsSetup()
    Dim objGLset As New agri.GL.clsSetup()
    Dim objGLtrx As New agri.GL.clsTrx()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objLangCap As New agri.PWSystem.clsLangCap()

    Dim strOpCdCanteenTxDet_ADD As String = "CT_CLSTRX_CANTEENRECEIVE_DETAIL_ADD"
    Dim strOpCdCanteenTxDet_UPD As String = "CT_CLSTRX_CANTEENRECEIVE_DETAIL_UPD"
    Dim strOpCdCanteenTxLine_GET As String = "CT_CLSTRX_CANTEENRECEIVE_LINE_GET"
    Dim strOpCdItem_Details_GET As String = "IN_CLSSETUP_STOCKITEM_DETAILS_GET"
    Dim strOpCdItem_Details_UPD As String = "IN_CLSTRX_STOCKITEM_DETAIL_UPD"
    Dim strOpPRLNDet_Details_GET As String = "IN_CLSTRX_PURREQLN_DET_GET"
    Dim strOpPRLN_UPD As String = "IN_CLSTRX_PRLN_UPD"
    Dim strOpCdItem_Details_GET2 As String = "CT_CLSSETUP_ITEM_DETAILS_GET_BY_LOCATION"
    Dim strOpCdPR_Count_GET As String = "IN_CLSTRX_PURREQLN_FINDLIST_GET"
    Dim strOpCdPR_Details_UPD As String = "IN_CLSTRX_PURREQ_DET_UPD"
    Dim objDataSet As New DataSet()
    Dim dsGrid As New DataSet()
    Dim objLangCapDs As New DataSet()
    Dim intErrNo As Integer
    Dim strParam As String = ""
    Dim strLocationTag As String

    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strDateFMT As String
    Dim intCTAR As Integer
    Dim intConfigsetting As Integer

    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim strLocType as String


    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_INACCMONTH")
        strAccYear = Session("SS_INACCYEAR")
        strDateFMT = Session("SS_DATEFMT")
        intCTAR = Session("SS_CTAR")
        intConfigsetting = Session("SS_CONFIGSETTING")

        strLocType = Session("SS_LOCTYPE")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCTAccessRights.CTReceive), intCTAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            onload_GetLangCap()
            If Not Page.IsPostBack Then
                BindChargeLevelDropDownList()
                lblCanteenTxID.Text = Request.QueryString("Id")
                LoadCanteenTxDetails()

                If objDataSet.Tables(0).Rows.Count > 0 Then
                    DisplayFromDB()
                    BindDocList()
                    DisablePage()
                    BindGrid()
                End If
                BindDocList()
                BindPRList()
                BindAccCodeDropList()
                BindBlockDropList("")
                BindVehicleCodeDropList("")
                BindVehicleExpDropList(True)
                BindLocDropList()
            End If
            BindItemCodeList()
            DisablePage()

            lblError.Visible = False
            lblstock.Visible = False
            lblUnDel.Visible = False
            lblQty.Visible = False
            lblDate.Visible = False
            lblFmt.Visible = False
            lblPR.Visible = False
            lblTxError.Visible = False
            lblConfirmErr.Visible = False
            lblAccCodeErr.Visible = False
            lblPreBlockErr.Visible = False
            lblBlockErr.Visible = False
            lblVehCodeErr.Visible = False
            lblVehExpCodeErr.Visible = False
            lblItemCodeErr.Visible = False
            lblErrQty.Visible = False
            lblErrTotalAmt.Visible = False
            lblErrUnitCost.Visible = False
        End If
    End Sub

    Sub BindChargeLevelDropDownList()
        lstChargeLevel.Items.Add(New ListItem(PreBlockTag, objLangCap.EnumLangCap.Block))
        lstChargeLevel.Items.Add(New ListItem(lblBlkTag.Text, objLangCap.EnumLangCap.SubBlock))
        lstChargeLevel.selectedIndex = Session("SS_BLOCK_CHARGE_DEFAULT")
        ToggleChargeLevel()
    End Sub
    
    Sub lstChargeLevel_OnSelectedIndexChanged(sender As Object, e As EventArgs)
        ToggleChargeLevel()
    End Sub
    
    Sub ToggleChargeLevel()
        If lstChargeLevel.selectedIndex = 0 Then
            RowBlk.Visible = False
            RowPreBlk.Visible = True
            hidBlockCharge.value = "yes"
        Else
            RowBlk.Visible = True
            RowPreBlk.Visible = False
            hidBlockCharge.value = ""
        End If
    End Sub

    Sub onload_GetLangCap()

        GetEntireLangCap()

        Try
            If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
                lblBlkTag.Text = GetCaption(objLangCap.EnumLangCap.Block) & lblCode.Text
            Else
                lblBlkTag.Text = GetCaption(objLangCap.EnumLangCap.SubBlock) & lblCode.Text
            End If
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CT_TRX_CANTEEN_RECEIVE_GET_LANGCAP_COSTLEVEL&errmesg=" & lblErrMessage.Text & "&redirect=ct/trx/ct_trx_canteenreceivelist.aspx")
        End Try

        lblAccTag.Text = GetCaption(objLangCap.EnumLangCap.Account) & lblCode.Text
        lblVehTag.Text = GetCaption(objLangCap.EnumLangCap.Vehicle) & lblCode.Text
        lblVehExpTag.Text = GetCaption(objLangCap.EnumLangCap.VehExpense) & lblCode.Text
        strLocationTag = GetCaption(objLangCap.EnumLangCap.Location)

        dgCanteenTx.Columns(2).HeaderText = lblAccTag.Text
        dgCanteenTx.Columns(3).HeaderText = lblBlkTag.Text
        dgCanteenTx.Columns(4).HeaderText = lblVehTag.Text
        dgCanteenTx.Columns(5).HeaderText = lblVehExpTag.Text

        lblAccCodeErr.Text = "<BR>" & lblPleaseSelect.Text & lblAccTag.Text
        lblBlockErr.Text = lblPleaseSelect.Text & lblBlkTag.Text
        lblVehCodeErr.Text = lblPleaseSelect.Text & lblVehTag.Text
        lblVehExpCodeErr.Text = lblPleaseSelect.Text & lblVehExpTag.Text
        rfvFromLocCode.ErrorMessage = "<BR>" & lblPleaseSelect.Text & strLocationTag & lblCode.Text
        PreBlockTag = GetCaption(objLangCap.EnumLangCap.Block)
        lblPreBlkTag.Text = PreBlockTag & lblCode.Text & " : "
        lblPreBlockErr.Text = lblPleaseSelect.Text & PreBlockTag & lblCode.Text
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CT_TRX_CANTEEN_RECEIVE_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=ct/trx/ct_trx_canteenreceivelist.aspx")
        End Try

    End Sub

    Function GetCaption(ByVal pv_TermCode) As String
            Dim count As Integer
            
            For count=0 To objLangCapDs.Tables(0).Rows.Count - 1
                If Trim(pv_TermCode) = Trim(objLangCapDs.Tables(0).Rows(count).Item("TermCode"))
                    If strLocType = objAdminLoc.EnumLocType.Mill then
                        Return Trim(objLangCapDs.Tables(0).Rows(count).Item("BusinessTermMW"))
                    else
                        Return Trim(objLangCapDs.Tables(0).Rows(count).Item("BusinessTerm"))
                    end if
                    Exit For
                End If
            Next
    End Function




    Protected Overloads Sub OnPreRender(ByVal Source As Object, ByVal E As EventArgs) Handles MyBase.PreRender
        ItemTypeCheck()
    End Sub

    Sub DisablePage()

        If lstRecDoc.SelectedItem.Value = objCTtx.EnumCanteenReceiveDocType.DispatchAdvice Then
            lstItem.AutoPostBack = True
            tblPR.Visible = True
            RowFromLoc.Visible = False
        ElseIf lstRecDoc.SelectedItem.Value = objCTtx.EnumCanteenReceiveDocType.StockTransfer Then
            lstItem.AutoPostBack = True
            tblPR.Visible = False
            RowFromLoc.Visible = True
        ElseIf lstRecDoc.SelectedItem.Value = objCTtx.EnumCanteenReceiveDocType.StockReturnAdvice Then
            lstItem.AutoPostBack = True
            tblPR.Visible = False
            RowFromLoc.Visible = False
        End If

        txtRefNo.Enabled = False
        txtDate.Enabled = False
        btnSelDate.Visible = False
        lstRecDoc.Enabled = False
        txtRemarks.Enabled = False
        Save.Visible = False
        Confirm.Visible = False
        btnNew.Visible = False
        PRDelete.Visible = False
        Select Case Trim(lblStatusHid.Text)
            Case Trim(CStr(objCTtx.EnumCanteenReceiveStatus.Deleted))
                tblPR.Visible = False
                btnNew.Visible = True
                PRDelete.Visible = True
                PRDelete.ImageUrl = "../../images/butt_undelete.gif"
                PRDelete.AlternateText = "Undelete"
                PRDelete.Attributes("onclick") = "javascript:return ConfirmAction('undelete');"
            Case Trim(CStr(objCTtx.EnumCanteenReceiveStatus.Confirmed))
                tblPR.Visible = False
                btnNew.Visible = True
            Case Else
                txtRefNo.Enabled = True
                txtDate.Enabled = True
                btnSelDate.Visible = True
                txtRemarks.Enabled = True
                Save.Visible = True
                If Trim(lblCanteenTxID.Text) = "" Then
                    lstRecDoc.Enabled = True
                Else
                    Confirm.Visible = True
                    PRDelete.Visible = True
                    btnNew.Visible = True
                    PRDelete.ImageUrl = "../../images/butt_delete.gif"
                    PRDelete.AlternateText = "Delete"
                    PRDelete.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                End If
        End Select 

        DisableItemTable()
    End Sub

    Sub DisableItemTable()
        If lblStatusHid.Text = CStr(objCTtx.EnumCanteenReceiveStatus.Deleted) Or _
           lblStatusHid.Text = CStr(objCTtx.EnumCanteenReceiveStatus.Confirmed) Then
            tblAdd.Visible = False
        Else
            tblAdd.Visible = True

        End If
    End Sub

    Sub DataGrid_ItemCreated(ByVal Sender As Object, ByVal e As DataGridItemEventArgs)

        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim DeleteButton As LinkButton
                Select Case Trim(Status.Text)
                    Case objCTtx.mtdGetCanteenReceiveStatus(objCTtx.EnumCanteenReceiveStatus.Active)
                        DeleteButton = e.Item.FindControl("Delete")
                        DeleteButton.Visible = True
                        DeleteButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                    Case objCTtx.mtdGetCanteenReceiveStatus(objCTtx.EnumCanteenReceiveStatus.Confirmed), _
                        objCTtx.mtdGetCanteenReceiveStatus(objCTtx.EnumCanteenReceiveStatus.Deleted)
                        DeleteButton = e.Item.FindControl("Delete")
                        DeleteButton.Visible = False
                End Select
        End Select
    End Sub


    Sub DisplayFromDB()

        Status.Text = objCTtx.mtdGetCanteenReceiveStatus(Trim(objDataSet.Tables(0).Rows(0).Item("Status")))
        lblStatusHid.Text = Trim(objDataSet.Tables(0).Rows(0).Item("Status"))
        CreateDate.Text = objGlobal.GetLongDate(Trim(objDataSet.Tables(0).Rows(0).Item("CreateDate")))
        UpdateDate.Text = objGlobal.GetLongDate(Trim(objDataSet.Tables(0).Rows(0).Item("UpdateDate")))
        UpdateBy.Text = Trim(objDataSet.Tables(0).Rows(0).Item("UserName"))
        txtRemarks.Text = Trim(objDataSet.Tables(0).Rows(0).Item("Remark"))
        lblTotAmtFig.Text = ObjGlobal.GetIDDecimalSeparator(Round(objDataSet.Tables(0).Rows(0).Item("TotalAmount"),0))
        txtDate.Text = objGlobal.GetShortDate(strDateFMT, Trim(objDataSet.Tables(0).Rows(0).Item("CanteenRefDate")))
        txtRefNo.Text = Trim(objDataSet.Tables(0).Rows(0).Item("CanteenRefNo"))
    End Sub

    Sub BindGrid()
        Dim PageNo As Integer

        dgCanteenTx.DataSource = LoadDataGrid()
        dgCanteenTx.DataBind()
    End Sub

    Protected Function LoadDataGrid() As DataSet
        Dim strParam As String
        Dim intCnt As Integer

        strParam = Trim(lblCanteenTxID.Text)

        Try
            intErrNo = objCTtx.mtdGetCanteenTransactDetails(strOpCdCanteenTxLine_GET, _
                                                            strCompany, _
                                                            strLocation, _
                                                            strUserId, _
                                                            strAccMonth, _
                                                            strAccYear, _
                                                            strParam, _
                                                            dsGrid)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_CANTEENRECEIVE&errmesg=" & lblErrMessage.Text & "&redirect=CT/trx/CT_trx_CanteenReceiveList.aspx")
        End Try

        For intCnt = 0 To dsGrid.Tables(0).Rows.Count - 1
            dsGrid.Tables(0).Rows(intCnt).Item("Description") = Trim(dsGrid.Tables(0).Rows(intCnt).Item("ItemCode")) & " (" & _
                                                                Trim(dsGrid.Tables(0).Rows(intCnt).Item("Description")) & ")"
        Next intCnt

        Return dsGrid
    End Function

    Sub LoadCanteenTxDetails()
        Dim strOpCdCanteenTxDet_GET As String = "CT_CLSTRX_CANTEENRECEIVE_DETAIL_GET"
        Dim CanteenCode As String

        strParam = Trim(lblCanteenTxID.Text)

        Try
            intErrNo = objCTtx.mtdGetCanteenTransactDetails(strOpCdCanteenTxDet_GET, _
                                                            strCompany, _
                                                            strLocation, _
                                                            strUserId, _
                                                            strAccMonth, _
                                                            strAccYear, _
                                                            strParam, _
                                                            objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_CANTEENRECEIVELINE&errmesg=" & lblErrMessage.Text & "&redirect=CT/trx/CT_trx_CanteenReceiveList.aspx")
        End Try
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        If lblStatusHid.Text = objCTtx.EnumCanteenReceiveStatus.Active Then
            Dim strOpCdCanteenTxLine_Det_GET As String = "CT_CLSTRX_CANTEEN_RECEIVE_LN_DETAIL_GET" '"CT_CLSTRX_CANTEENRECEIVELINE_DETAILS_GET"
            Dim strOpCdCanteenTxLine_DEL As String = "CT_CLSTRX_CANTEENRECEIVE_LINE_DEL"
            Dim lbl As Label
            Dim ItemCode As String
            Dim CanteenReceiveLnID As String
            Dim strParam As String
            Dim StrTxParam As String
            Dim TxID As String
            Dim ErrorChk As Integer = objCTtx.EnumInventoryErrorType.NoError

            lbl = E.Item.FindControl("ItemCode")
            ItemCode = lbl.Text
            lbl = E.Item.FindControl("RecvLnID")
            CanteenReceiveLnID = lbl.Text

            strParam = CanteenReceiveLnID & "|" & ItemCode & "|" & lstRecDoc.SelectedItem.Value.Trim
            Try
                intErrNo = objCTtx.mtdDelReceiveTransactLn(strOpCdCanteenTxLine_DEL, _
                                                           strOpCdCanteenTxLine_Det_GET, _
                                                           strOpCdItem_Details_UPD, _
                                                           strOpPRLN_UPD, _
                                                           strOpCdPR_Count_GET, _
                                                           strOpCdPR_Details_UPD, _
                                                           strOpPRLNDet_Details_GET, _
                                                           strCompany, _
                                                           strLocation, _
                                                           strUserId, _
                                                           strAccMonth, _
                                                           strAccYear, _
                                                           strParam, _
                                                           ErrorChk)
                Select Case ErrorChk
                    Case objINtx.EnumInventoryErrorType.OverFlow
                        lblError.Visible = True
                    Case objINtx.EnumInventoryErrorType.InsufficientQty
                        lblStock.Visible = True
                    Case objINtx.EnumInventoryErrorType.PRNotfound
                        lblPR.Visible = True
                End Select
                If ErrorChk <> objCTtx.EnumInventoryErrorType.NoError Then
                    Exit Sub
                End If
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=DEL_CANTEENRECEIVELINE&errmesg=" & lblErrMessage.Text & "&redirect=CT/trx/CT_trx_CanteenReceiveList.aspx")
                End If
            End Try

            StrTxParam = lblCanteenTxID.Text & "|||||||||||||||"

            Try
                intErrNo = objCTtx.mtdUpdCanteenReceiveDetail(strOpCdCanteenTxDet_ADD, _
                                                              strOpCdCanteenTxDet_UPD, _
                                                              strOpCdCanteenTxLine_GET, _
                                                              strCompany, _
                                                              strLocation, _
                                                              strUserId, _
                                                              strAccMonth, _
                                                              strAccYear, _
                                                              StrTxParam, _
                                                              objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CTCanteenReceive), _
                                                              ErrorChk, _
                                                              TxID)

                If ErrorChk = objCTtx.EnumInventoryErrorType.OverFlow Then
                    lblError.Visible = True
                End If
                lblCanteenTxID.Text = TxID
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ADDNEWCANTEENRECEIVE&errmesg=" & lblErrMessage.Text & "&redirect=CT/trx/CT_trx_CanteenReceiveList.aspx")
                End If
            End Try
            LoadCanteenTxDetails()
            DisplayFromDB()
            BindGrid()
            BindItemCodeList()
            BindLocDropList()

        End If
    End Sub

    Sub BindDocList()
        lstRecDoc.Items.Clear()
        lstRecDoc.Items.Add(New ListItem(objCTtx.mtdGetCanteenReceiveDocType(objCTtx.EnumCanteenReceiveDocType.DispatchAdvice), objCTtx.EnumCanteenReceiveDocType.DispatchAdvice))
        lstRecDoc.Items.Add(New ListItem(objCTtx.mtdGetCanteenReceiveDocType(objCTtx.EnumCanteenReceiveDocType.StockTransfer), objCTtx.EnumCanteenReceiveDocType.StockTransfer))
        lstRecDoc.Items.Add(New ListItem(objCTtx.mtdGetCanteenReceiveDocType(objCTtx.EnumCanteenReceiveDocType.StockReturnAdvice), objCTtx.EnumCanteenReceiveDocType.StockReturnAdvice))
        If Not Request.QueryString("Id") = "" Then
            Select Case Trim(objDataSet.Tables(0).Rows(0).Item("CanteenDocType"))
                Case objCTtx.EnumCanteenReceiveDocType.DispatchAdvice
                    lstRecDoc.SelectedIndex = 0
                Case objCTtx.EnumCanteenReceiveDocType.StockTransfer
                    lstRecDoc.SelectedIndex = 1
                Case objCTtx.EnumCanteenReceiveDocType.StockReturnAdvice
                    lstRecDoc.SelectedIndex = 2
            End Select
        End If
    End Sub

    Sub BindAccCodeDropList(Optional ByVal pv_strAccCode As String = "")

        Dim strOpCd As String = "GL_CLSSETUP_ACCOUNTCODE_LIST_GET"
        Dim dr As DataRow
        Dim strParam As String = "Order By ACC.AccCode|And ACC.Status = '" & objGLset.EnumAccountCodeStatus.Active & "'"
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer = 0
        Dim dsForDropDown As DataSet

        Try
            intErrNo = objGLset.mtdGetMasterList(strOpCd, _
                                                   strParam, _
                                                   objGLset.EnumGLMasterType.AccountCode, _
                                                   dsForDropDown)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BindAccCodeDropList&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To dsForDropDown.Tables(0).Rows.Count - 1
            dsForDropDown.Tables(0).Rows(intCnt).Item("AccCode") = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("AccCode"))
            dsForDropDown.Tables(0).Rows(intCnt).Item("Description") = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("AccCode")) & " (" & Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("Description")) & ")"
            If dsForDropDown.Tables(0).Rows(intCnt).Item("AccCode") = Trim(pv_strAccCode) Then
                intSelectedIndex = intCnt + 1
            End If
        Next

        dr = dsForDropDown.Tables(0).NewRow()
        dr("AccCode") = ""
        dr("Description") = lblSelect.Text & lblAccTag.Text
        dsForDropDown.Tables(0).Rows.InsertAt(dr, 0)

        lstAccCode.DataSource = dsForDropDown.Tables(0)
        lstAccCode.DataValueField = "AccCode"
        lstAccCode.DataTextField = "Description"
        lstAccCode.DataBind()
        lstAccCode.SelectedIndex = intSelectedIndex

        If Not dsForDropDown Is Nothing Then
            dsForDropDown = Nothing
        End If
    End Sub

    Sub CallCheckVehicleUse(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckVehicleUse()
    End Sub

    Sub CheckVehicleUse()
        Dim intAccType As Integer
        Dim intAccPurpose As Integer

        Dim strAcc As String = Request.Form("lstAccCode")
        Dim strBlk As String = Request.Form("lstBlock")
        Dim strVeh As String = Request.Form("lstVehCode")
        Dim strVehExp As String = Request.Form("lstVehExp")
        GetAccountDetails(strAcc, intAccType, intAccPurpose)

        If intAccType = objGLset.EnumAccountType.ProfitAndLost Then
            Select Case intAccPurpose
                Case objGLset.EnumAccountPurpose.NonVehicle
                    BindBlockDropList(strAcc, strBlk)
                    BindVehicleCodeDropList("")
                    BindVehicleExpDropList(True)
                Case objGLset.EnumAccountPurpose.VehicleDistribution
                    BindBlockDropList("")
                    BindVehicleCodeDropList(strAcc, strVeh)
                    BindVehicleExpDropList(False, strVehExp)
                Case Else
                    BindBlockDropList(strAcc, strBlk)
                    BindVehicleCodeDropList("%", strVeh)
                    BindVehicleExpDropList(False, strVehExp)
            End Select
        Else
            BindBlockDropList("")
            BindVehicleCodeDropList("")
            BindVehicleExpDropList(True)
        End If

    End Sub

    Sub GetItemDetails(ByVal pv_strItemCode As String, ByRef pr_strItemType As Integer)

        Dim _objItemDs As New DataSet()
        Dim strParam As String = pv_strItemCode & "|"
        Dim intErrNo As Integer

        Try
            intErrNo = objINstp.mtdGetMasterDetail(strOpCdItem_Details_GET, _
                                                strParam, _
                                                _objItemDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GET_ITEM_DETAILS&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        If _objItemDs.Tables(0).Rows.Count = 1 Then
            pr_strItemType = CInt(_objItemDs.Tables(0).Rows(0).Item("ItemType"))
        End If
    End Sub

    Sub GetAccountDetails(ByVal pv_strAccCode As String, _
                          ByRef pr_strAccType As Integer, _
                          ByRef pr_strAccPurpose As Integer)

        Dim _objAccDs As New DataSet()
        Dim strOpCd As String = "GL_CLSSETUP_CHARTOFACCOUNT_GET_BY_ACCCODE"
        Dim strParam As String = pv_strAccCode
        Dim intErrNo As Integer

        Try
            intErrNo = objGLset.mtdGetAccount(strOpCd, _
                                                strParam, _
                                                _objAccDs, _
                                                True)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GET_ACCOUNT_DETAILS&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        If _objAccDs.Tables(0).Rows.Count = 1 Then
            pr_strAccType = CInt(_objAccDs.Tables(0).Rows(0).Item("AccType"))
            pr_strAccPurpose = CInt(_objAccDs.Tables(0).Rows(0).Item("AccPurpose"))
        End If
    End Sub

    Function CheckRequiredField() As Boolean
        Dim intAccType As Integer
        Dim intAccPurpose As Integer
        Dim strAcc As String = Request.Form("lstAccCode")
        Dim strPreBlk As String = Request.Form("lstPreBlock")
        Dim strBlk As String = Request.Form("lstBlock")
        Dim strVeh As String = Request.Form("lstVehCode")
        Dim strVehExp As String = Request.Form("lstVehExp")
        Dim strItem As String = Request.Form("lstItem")

        GetAccountDetails(strAcc, intAccType, intAccPurpose)
        If strItem.Trim = "" Then
            lblItemCodeErr.Visible = True
            Return True
        End If

        If intAccType = objGLset.EnumAccountType.BalanceSheet Then
            Return False
        Else
            If strAcc = "" Then
                lblAccCodeErr.Visible = True
                Return True
            ElseIf strPreBlk = "" And lstChargeLevel.SelectedIndex = 0 And (Not intAccPurpose = objGLset.EnumAccountPurpose.VehicleDistribution) Then
                lblPreBlockErr.Visible = True                
                Return True
            ElseIf strBlk = "" And lstChargeLevel.SelectedIndex = 1 And (Not intAccPurpose = objGLset.EnumAccountPurpose.VehicleDistribution) Then
                lblBlockErr.Visible = True
                Return True
            ElseIf strVeh = "" And intAccPurpose = objGLset.EnumAccountPurpose.VehicleDistribution Then
                lblVehCodeErr.Visible = True
                Return True
            ElseIf strVehExp = "" And intAccPurpose = objGLset.EnumAccountPurpose.VehicleDistribution Then
                lblVehExpCodeErr.Visible = True
                Return True
            ElseIf strVeh <> "" And strVehExp = "" And intAccPurpose = objGLset.EnumAccountPurpose.others Then
                lblVehExpCodeErr.Visible = True
                Return True
            ElseIf strVeh = "" And intAccPurpose = objGLset.EnumAccountPurpose.others And strVehExp <> "" Then
                lblVehCodeErr.Visible = True
                Return True
            Else
                Return False
            End If
        End If

    End Function

    Sub BindBlockDropList(ByVal pv_strAccCode As String, Optional ByVal pv_strBlkCode As String = "")
        Dim dsForDropDown As DataSet
        Dim intSelectedIndex As Integer = 0
        Dim intCnt As Integer
        Dim strOpCdBlockList_Get As String
        Dim strParam As String
        Dim dr As DataRow

        Try
            If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
                strOpCdBlockList_Get = "GL_CLSSETUP_ACCOUNT_BLOCK_GET"
                strParam = pv_strAccCode & "|" & Session("SS_LOCATION") & "|" & objGLset.EnumBlockStatus.Active
            Else
                strOpCdBlockList_Get = "GL_CLSSETUP_ACCOUNT_SUBBLOCK_GET"
                strParam = pv_strAccCode & "|" & Session("SS_LOCATION") & "|" & objGLset.EnumSubBlockStatus.Active
            End If
            intErrNo = objGLset.mtdGetAccountBlock(strOpCdBlockList_Get, _
                                                     strParam, _
                                                     dsForDropDown)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GL_CLSSETUP_ACCOUNT_BLOCK_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To dsForDropDown.Tables(0).Rows.Count - 1
            dsForDropDown.Tables(0).Rows(intCnt).Item("BlkCode") = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("BlkCode"))
            dsForDropDown.Tables(0).Rows(intCnt).Item("Description") = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("BlkCode")) & " (" & Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("Description")) & ")"
            If dsForDropDown.Tables(0).Rows(intCnt).Item("BlkCode") = Trim(pv_strBlkCode) Then
                intSelectedIndex = intCnt + 1
            End If
        Next

        dr = dsForDropDown.Tables(0).NewRow()
        dr("BlkCode") = ""
        dr("Description") = lblSelect.Text & lblBlkTag.Text
        dsForDropDown.Tables(0).Rows.InsertAt(dr, 0)
        lstBlock.DataSource = dsForDropDown.Tables(0)
        lstBlock.DataValueField = "BlkCode"
        lstBlock.DataTextField = "Description"
        lstBlock.DataBind()
        lstBlock.SelectedIndex = intSelectedIndex

        If Not dsForDropDown Is Nothing Then
            dsForDropDown = Nothing
        End If

        strOpCdBlockList_Get = "GL_CLSSETUP_ACCOUNT_PREBLOCK_GET"
        intSelectedIndex = 0
        Try
            strParam = pv_strAccCode & "|" & Session("SS_LOCATION") & "|" & objGLset.EnumBlockStatus.Active
            intErrNo = objGLset.mtdGetAccountBlock(strOpCdBlockList_Get, _
                                                     strParam, _
                                                     dsForDropDown)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GL_CLSSETUP_ACCOUNT_BLOCK_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To dsForDropDown.Tables(0).Rows.Count - 1
            dsForDropDown.Tables(0).Rows(intCnt).Item("BlkCode") = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("BlkCode"))
            dsForDropDown.Tables(0).Rows(intCnt).Item("Description") = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("BlkCode")) & " (" & Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("Description")) & ")"
            If dsForDropDown.Tables(0).Rows(intCnt).Item("BlkCode") = Trim(pv_strBlkCode) Then
                intSelectedIndex = intCnt + 1
            End If
        Next

        dr = dsForDropDown.Tables(0).NewRow()
        dr("BlkCode") = ""
        dr("Description") = lblSelect.Text & PreBlockTag & lblCode.Text

        dsForDropDown.Tables(0).Rows.InsertAt(dr, 0)
        lstPreBlock.DataSource = dsForDropDown.Tables(0)
        lstPreBlock.DataValueField = "BlkCode"
        lstPreBlock.DataTextField = "Description"
        lstPreBlock.DataBind()
        lstPreBlock.SelectedIndex = intSelectedIndex

        If Not dsForDropDown Is Nothing Then
            dsForDropDown = Nothing
        End If
    End Sub

    Sub BindVehicleCodeDropList(ByVal pv_strAccCode As String, Optional ByVal pv_strVehCode As String = "")

        Dim dsForDropDown As DataSet
        Dim strOpCd As String
        Dim drinsert As DataRow
        Dim strParam As New StringBuilder()
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intMasterType As Integer
        Dim intSelectedIndex As Integer = 0

        strParam.Append("|AccCode = '" & pv_strAccCode & "' AND LocCode = '" & Session("SS_LOCATION") & "' AND Status = '" & objGLset.EnumVehicleStatus.Active & "'")
        Try
            strOpCd = "GL_CLSSETUP_VEH_LIST_GET"
            intErrNo = objGLset.mtdGetMasterList(strOpCd, _
                                                   strParam.ToString, _
                                                   objGLset.EnumGLMasterType.Vehicle, _
                                                   dsForDropDown)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ITEMTODROPDOWN&errmesg=" & lblErrMessage.Text & "&redirect=GL/Trx/GL_Trx_Journal_List.aspx")
        End Try




        For intCnt = 0 To dsForDropDown.Tables(0).Rows.Count - 1
            dsForDropDown.Tables(0).Rows(intCnt).Item(0) = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(0))
            dsForDropDown.Tables(0).Rows(intCnt).Item(1) = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(0)) & " ( " & _
                                                          Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(1)) & " )"
            If dsForDropDown.Tables(0).Rows(intCnt).Item("VehCode") = Trim(pv_strVehCode) Then
                intSelectedIndex = intCnt + 1
            End If
        Next intCnt

        drinsert = dsForDropDown.Tables(0).NewRow()
        drinsert(0) = ""

        drinsert(1) = lblSelect.Text & lblVehTag.Text
        dsForDropDown.Tables(0).Rows.InsertAt(drinsert, 0)

        lstVehCode.DataSource = dsForDropDown.Tables(0)
        lstVehCode.DataValueField = "VehCode"
        lstVehCode.DataTextField = "Description"
        lstVehCode.DataBind()
        lstVehCode.SelectedIndex = intSelectedIndex

        If Not dsForDropDown Is Nothing Then
            dsForDropDown = Nothing
        End If
    End Sub

    Sub BindVehicleExpDropList(ByVal pv_IsBlankList As Boolean, Optional ByVal pv_strVehExpCode As String = "")

        Dim dsForDropDown As DataSet
        Dim strOpCd As String = "GL_CLSSETUP_VEHEXPENSE_LIST_GET"
        Dim drinsert As DataRow
        Dim strParam As String = "Order By VehExpenseCode ASC|And Veh.Status = '" & objGLset.EnumVehicleExpenseStatus.active & "'"
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer = 0

        Try
            If pv_IsBlankList Then
                strParam += " And Veh.VehExpensecode = ''"
            End If
            intErrNo = objGLset.mtdGetMasterList(strOpCd, _
                                                   strParam, _
                                                   objGLset.EnumGLMasterType.VehicleExpense, _
                                                   dsForDropDown)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=JR_TRX_ADTRX_VEHEXPENSE_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To dsForDropDown.Tables(0).Rows.Count - 1
            dsForDropDown.Tables(0).Rows(intCnt).Item(0) = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(0))
            dsForDropDown.Tables(0).Rows(intCnt).Item(1) = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(0)) & " ( " & _
                                                          Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(1)) & " )"
            If dsForDropDown.Tables(0).Rows(intCnt).Item("VehExpenseCode") = Trim(pv_strVehExpCode) Then
                intSelectedIndex = intCnt + 1
            End If
        Next intCnt
        drinsert = dsForDropDown.Tables(0).NewRow()
        drinsert(0) = ""
        drinsert(1) = lblSelect.Text & lblVehExpTag.Text
        dsForDropDown.Tables(0).Rows.InsertAt(drinsert, 0)

        lstVehExp.DataSource = dsForDropDown.Tables(0)
        lstVehExp.DataValueField = "VehExpenseCode"
        lstVehExp.DataTextField = "Description"
        lstVehExp.DataBind()
        lstVehExp.SelectedIndex = intSelectedIndex


        If Not dsForDropDown Is Nothing Then
            dsForDropDown = Nothing
        End If
    End Sub

    Sub BindLocDropList()
        Dim strOpCdLocList_GET As String = "ADMIN_CLSLOC_SYSLOC_LIST_GET"
        Dim dsForDropDown As DataSet
        Dim intCnt As Integer
        Dim strParam As String
        Dim strFieldCheck As String
        Dim drinsert As DataRow
        Try
            strParam = "And SY.CompCode = '" & strCompany & "' AND LO.Status = " & objAdminLoc.EnumLocStatus.Active & " AND Not SY.LocCode = '" & strLocation & "'|"

            intErrNo = objAdminLoc.mtdGetCompLocList(strOpCdLocList_GET, strCompany, strLocation, strUserId, dsForDropDown, strParam)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BIND_FROMLOCATION&errmesg=" & lblErrMessage.Text & "&redirect=CT/trx/CT_trx_CanteenReceiveList.aspx")
        End Try

        For intCnt = 0 To dsForDropDown.Tables(0).Rows.Count - 1
            strFieldCheck = dsForDropDown.Tables(0).Rows(intCnt).Item(0)
            dsForDropDown.Tables(0).Rows(intCnt).Item(0) = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(0))
            dsForDropDown.Tables(0).Rows(intCnt).Item(1) = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(0)) & " ( " & _
                                                           Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(1)) & " )"
        Next intCnt

        drinsert = dsForDropDown.Tables(0).NewRow()
        drinsert(0) = ""
        drinsert(1) = lblSelect.Text & strLocationTag
        dsForDropDown.Tables(0).Rows.InsertAt(drinsert, 0)

        lstFromLoc.DataSource = dsForDropDown.Tables(0)
        lstFromLoc.DataValueField = "LocCode"
        lstFromLoc.DataTextField = "LocDesc"
        lstFromLoc.DataBind()

        If Not dsForDropDown Is Nothing Then
            dsForDropDown = Nothing
        End If

    End Sub

    Sub BindPRList()
        Dim strOpCdPR_List_GET As String = "IN_CLSTRX_PURREQ_LIST_GET"
        Dim dsPRDropList As DataSet
        Dim drInsert As DataRow
        Dim intCnt As Integer
        Dim strParam As String


        strParam = "||" & objINtx.EnumPurReqDocType.CanteenPR & "||" & _
                   objINtx.EnumPurReqStatus.Confirmed & "|" & strLocation & "|" & strAccMonth & "|" & strAccYear & "|PR.PRID|DESC"

        Try
            intErrNo = objINtx.mtdGetPRList(strOpCdPR_List_GET, strParam, dsPRDropList)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ITEMTOPRDROPDOWNLIST&errmesg=" & lblErrMessage.Text & "&redirect=CT/trx/CT_trx_CanteenReceiveList.aspx")
        End Try

        For intCnt = 0 To dsPRDropList.Tables(0).Rows.Count - 1
            dsPRDropList.Tables(0).Rows(intCnt).Item(0) = Trim(dsPRDropList.Tables(0).Rows(intCnt).Item(0))
            dsPRDropList.Tables(0).Rows(intCnt).Item(1) = Trim(dsPRDropList.Tables(0).Rows(intCnt).Item(0))
        Next intCnt

        drInsert = dsPRDropList.Tables(0).NewRow()
        drInsert(0) = "NoPR"
        drInsert(1) = "Select purchase requisition or leave blank"

        dsPRDropList.Tables(0).Rows.InsertAt(drInsert, 0)
        lstPR.DataSource = dsPRDropList.Tables(0)
        lstPR.DataValueField = "PRID"
        lstPR.DataTextField = "PrType"
        lstPR.DataBind()

        If Not dsPRDropList Is Nothing Then
            dsPRDropList = Nothing
        End If
    End Sub

    Sub RebindItemList(ByVal sender As Object, ByVal e As System.EventArgs)
        BindItemCodeList()
    End Sub

    Sub ItemTypeCheck()
        Dim strItemType As Integer
        GetItemDetails(Request.Form("lstItem"), strItemType)
        Select Case strItemType
            Case objINstp.EnumInventoryItemType.DirectCharge
                RowAcc.Visible = True
                RowChargeLevel.Visible = Session("SS_BLOCK_CHARGE_VISIBLE")
                ToggleChargeLevel()
                RowVeh.Visible = True
                RowVehExp.Visible = True
            Case Else
                RowAcc.Visible = False
                RowBlk.Visible = False
                RowChargeLevel.Visible = False
                RowPreBlk.Visible = False
                RowVeh.Visible = False
                RowVehExp.Visible = False
        End Select

    End Sub

    Sub BindItemCodeList()
        Dim strOpCdItem_List_GET As String
        Dim dsItemCodeDropList As DataSet
        Dim intCnt As Integer
        Dim strParam As String
        Dim strItemCode As String = Request.Form("lstItem")
        Dim intSelectedIndex As Integer

        If lstPR.SelectedItem.Value = "NoPR" Then
            strOpCdItem_List_GET = "IN_CLSSETUP_STOCKITEM_LIST_GET"
        Else
            strOpCdItem_List_GET = "IN_CLSTRX_STOCKRECEIVE_ITEMLIST_GET"
        End If

        strParam = objINstp.EnumInventoryItemType.CanteenItem & "|" & objINstp.EnumStockItemStatus.Active & "|" & lblCanteenTxID.Text & "|" & "itm.ItemCode" & "|" & lstPR.SelectedItem.Value

        Try
            intErrNo = objINstp.mtdGetFilteredItemList(strOpCdItem_List_GET, _
                                                       strCompany, _
                                                       strLocation, _
                                                       strUserId, _
                                                       strAccMonth, _
                                                       strAccYear, _
                                                       strParam, _
                                                       objINtx.EnumInventoryTransactionType.CanteenReceive, _
                                                       dsItemCodeDropList)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BIND_ITEMCODE&errmesg=" & lblErrMessage.Text & "&redirect=CT/trx/CT_trx_CanteenReceiveList.aspx")
        End Try

        For intCnt = 0 To dsItemCodeDropList.Tables(0).Rows.Count - 1
            dsItemCodeDropList.Tables(0).Rows(intCnt).Item(1) = Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item("ItemCode")) & " ( " _
                                                    & Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item("Description")) & " ), " & _
                                                    FormatCurrency(Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item("AverageCost")), 2) & ", " & _
                                                    FormatNumber(Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item("QtyOnHand")), 5) & " Unit"
            dsItemCodeDropList.Tables(0).Rows(intCnt).Item(0) = Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item("ItemCode"))

            If dsItemCodeDropList.Tables(0).Rows(intCnt).Item("ItemCode") = Trim(strItemCode) Then
                intSelectedIndex = intCnt + 1
            End If
        Next intCnt

        Dim drinsert As DataRow
        drinsert = dsItemCodeDropList.Tables(0).NewRow()
        drinsert(0) = ""
        drinsert(1) = "Select Item Code"
        dsItemCodeDropList.Tables(0).Rows.InsertAt(drinsert, 0)

        lstItem.DataSource = dsItemCodeDropList.Tables(0)
        lstItem.DataValueField = "ItemCode"
        lstItem.DataTextField = "Description"
        lstItem.DataBind()
        lstItem.SelectedIndex = intSelectedIndex

        DisableItemTable()

        If Not dsItemCodeDropList Is Nothing Then
            dsItemCodeDropList = Nothing
        End If
    End Sub

    Protected Function CheckDate() As String
        Dim strDateSetting As String
        Dim objSysCfgDs As DataSet
        Dim objDateFormat As String
        Dim strValidDate As String

        If Not txtDate.Text = "" Then
            If objGlobal.mtdValidInputDate(strDateFMT, txtDate.Text, objDateFormat, strValidDate) = True Then
                Return strValidDate
            Else
                lblFmt.Text = objDateFormat & "."
                lblDate.Visible = True
                lblFmt.Visible = True
            End If
        End If

    End Function


    Sub btnAdd_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)

        Dim strOpCdCanteenTxLine_ADD As String = "CT_CLSTRX_CANTEENRECEIVE_LINE_ADD"
        Dim strOpCodeGLSubBlkByBlk As String = "GL_CLSSETUP_SUBBLOCK_BY_BLOCK_GET"
        Dim strParamList As String
        Dim TxID As String
        Dim strTxParam As New StringBuilder()
        Dim strTxLnParam As New StringBuilder()
        Dim ErrorChk As Integer = objINtx.EnumInventoryErrorType.NoError
        Dim strAllowVehicle As String
        Dim PRID As String
        Dim strItemType As String

        GetItemDetails(Request.Form("lstItem"), strItemType)
        If strItemType.Trim = Trim(objINstp.EnumInventoryItemType.DirectCharge) Then
            If CheckRequiredField() Then
                Exit Sub
            End If
        End If

        If lstItem.SelectedIndex = 0 Then
            lblItemCodeErr.Visible = True
            Exit Sub
        ElseIf txtQty.Text = "" Then
            lblErrQty.Visible = True
            Exit Sub
        ElseIf txtCost.Text = "" Then
            lblErrUnitCost.Visible = True
            Exit Sub
        ElseIf txtAmount.Text = "" Then
            lblErrTotalAmt.Visible = True
            Exit Sub
        ElseIf Not CheckDate() = "" Then
            If lblCanteenTxID.Text = "" Then
                strTxParam.Append(lblCanteenTxID.Text)
                strTxParam.Append("||||||")
                strTxParam.Append(lstRecDoc.SelectedItem.Value.Trim)
                strTxParam.Append("|")
                strTxParam.Append(txtRefNo.Text)
                strTxParam.Append("|")
                strTxParam.Append(CheckDate())
                strTxParam.Append("|")
                strTxParam.Append(strLocation)
                strTxParam.Append("|")
                strTxParam.Append(strAccMonth)
                strTxParam.Append("|")
                strTxParam.Append(strAccYear)
                strTxParam.Append("|0|")
                strTxParam.Append(txtRemarks.Text)
                strTxParam.Append("||")
                Try
                    intErrNo = objCTtx.mtdUpdCanteenReceiveDetail(strOpCdCanteenTxDet_ADD, _
                                                                  strOpCdCanteenTxDet_UPD, _
                                                                  strOpCdCanteenTxLine_GET, _
                                                                  strCompany, _
                                                                  strLocation, _
                                                                  strUserId, _
                                                                  strAccMonth, _
                                                                  strAccYear, _
                                                                  strTxParam.ToString, _
                                                                  objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CTCanteenReceive), _
                                                                  ErrorChk, _
                                                                  TxID)

                    lblCanteenTxID.Text = TxID
                    If ErrorChk = objINtx.EnumInventoryErrorType.OverFlow Then
                        lblError.Visible = True
                    End If

                Catch Exp As System.Exception
                    If intErrNo <> -5 Then
                        Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ADDNEWCANTEENRECEIVE&errmesg=" & lblErrMessage.Text & "&redirect=CT/trx/CT_trx_CanteenReceiveList.aspx")
                    End If
                End Try
            End If

            If ErrorChk = objINtx.EnumInventoryErrorType.noError And intErrNo = 0 Then
                If lstRecDoc.SelectedItem.Value = objCTtx.EnumCanteenReceiveDocType.DispatchAdvice Then
                    PRID = lstPR.SelectedItem.Value
                Else
                    PRID = ""
                End If


                strTxLnParam.Append(lblCanteenTxID.Text)
                strTxLnParam.Append("|")
                strTxLnParam.Append(Request.Form("lstItem").Trim)
                strTxLnParam.Append("|")
                strTxLnParam.Append(txtQty.Text)
                strTxLnParam.Append("|")
                strTxLnParam.Append(txtCost.Text)
                strTxLnParam.Append("|")
                strTxLnParam.Append(lstRecDoc.SelectedItem.Value)
                strTxLnParam.Append("|")
                strTxLnParam.Append(PRID)
                strTxLnParam.Append("|")

                If strItemType.Trim = Trim(objINstp.EnumInventoryItemType.DirectCharge) Then
                    strTxLnParam.Append(Request.Form("lstAccCode").Trim)
                    strTxLnParam.Append("|")
                    If lstChargeLevel.selectedIndex = 1 Then
                        strTxLnParam.Append(Request.Form("lstBlock").Trim)
                    Else
                        strTxLnParam.Append(Request.Form("lstPreBlock").Trim)
                    End If
                    strTxLnParam.Append("|")
                    strTxLnParam.Append(Request.Form("lstVehCode").Trim)
                    strTxLnParam.Append("|")
                    strTxLnParam.Append(Request.Form("lstVehExp").Trim)
                Else
                    strTxLnParam.Append("|||")
                End If

                If lstRecDoc.SelectedItem.Value.Trim = objCTtx.EnumCanteenReceiveDocType.StockTransfer Then
                    strTxLnParam.Append("|")
                    strTxLnParam.Append(Request.Form("lstFromLoc").Trim)
                Else
                    strTxLnParam.Append("|")
                End If

                strTxLnParam.Append("|")
                strTxLnParam.Append(txtAmount.Text)

                Try
                    If lstChargeLevel.selectedIndex = 0 And RowPreBlk.Visible = True Then
                        strParamList = Session("SS_LOCATION") & "|" & _
                                       lstAccCode.SelectedItem.Value.Trim & "|" & _
                                       lstPreBlock.SelectedItem.Value.Trim & "|" & _
                                       objGLset.EnumBlockStatus.Active
                        intErrNo = objCTtx.mtdAddStockReceiveLnByBlock(strOpCodeGLSubBlkByBlk, _
                                                                       strParamList, _ 
                                                                       strOpCdCanteenTxLine_ADD, _
                                                                       strOpCdItem_Details_GET2, _
                                                                       strOpCdItem_Details_UPD, _
                                                                       strOpPRLNDet_Details_GET, _
                                                                       strOpPRLN_UPD, _
                                                                       strOpCdPR_Count_GET, _
                                                                       strOpCdPR_Details_UPD, _
                                                                       strCompany, _
                                                                       strLocation, _
                                                                       strUserId, _
                                                                       strAccMonth, _
                                                                       strAccYear, _
                                                                       objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CTCanteenReceiveLn), _
                                                                       objCTtx.EnumCanteenReceiveDocType.DispatchAdvice, _
                                                                       ErrorChk, _
                                                                       strTxLnParam.ToString)
                    Else
                        intErrNo = objCTtx.mtdAddCanteenReceiveLn(strOpCdCanteenTxLine_ADD, _
                                                                strOpCdItem_Details_GET2, _
                                                                strOpCdItem_Details_UPD, _
                                                                strOpPRLNDet_Details_GET, _
                                                                strOpPRLN_UPD, _
                                                                strOpCdPR_Count_GET, _
                                                                strOpCdPR_Details_UPD, _
                                                                strCompany, _
                                                                strLocation, _
                                                                strUserId, _
                                                                strAccMonth, _
                                                                strAccYear, _
                                                                objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CTCanteenReceiveLn), _
                                                                objCTtx.EnumCanteenReceiveDocType.DispatchAdvice, _
                                                                ErrorChk, _
                                                                strTxLnParam.ToString)
                    End If

                Catch Exp As System.Exception
                    If intErrNo <> -5 Then
                        Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ADDRECEIVELINE&errmesg=" & lblErrMessage.Text & "&redirect=CT/trx/CT_trx_CanteenReceiveList.aspx")
                    End If
                End Try
                Select Case ErrorChk
                    Case objINtx.EnumInventoryErrorType.OverFlow
                        lblError.Visible = True
                    Case objINtx.EnumInventoryErrorType.InsufficientQty
                        lblstock.Visible = True
                    Case objINtx.EnumInventoryErrorType.PRNotfound
                        lblPR.Visible = True
                End Select

            End If

            strTxParam.Remove(0, strTxParam.Length)
            strTxParam.Append(lblCanteenTxID.Text)
            strTxParam.Append("|||||||||||||||")

            If ErrorChk = objINtx.EnumInventoryErrorType.noError And intErrNo = 0 Then
                Try
                    intErrNo = objCTtx.mtdUpdCanteenReceiveDetail(strOpCdCanteenTxDet_ADD, _
                                                                  strOpCdCanteenTxDet_UPD, _
                                                                  strOpCdCanteenTxLine_GET, _
                                                                  strCompany, _
                                                                  strLocation, _
                                                                  strUserId, _
                                                                  strAccMonth, _
                                                                  strAccYear, _
                                                                  strTxParam.ToString, _
                                                                  objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CTCanteenReceive), _
                                                                  ErrorChk, _
                                                                  TxID)
                    If ErrorChk = objINtx.EnumInventoryErrorType.OverFlow Then
                        lblError.Visible = True
                    End If

                    lblCanteenTxID.Text = TxID
                Catch Exp As System.Exception
                    If intErrNo <> -5 Then
                        Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ADDNEWSTOCKRECEIVE&errmesg=" & lblErrMessage.Text & "&redirect=CT/trx/CT_trx_CanteenReceiveList.aspx")
                    End If
                End Try
            End If

            If Not strTxParam Is Nothing Then
                strTxParam = Nothing
            End If

            If Not strTxLnParam Is Nothing Then
                strTxLnParam = Nothing
            End If

            LoadCanteenTxDetails()
            DisplayFromDB()
            BindGrid()
            BindItemCodeList()
            BindLocDropList()
            DisablePage()
            txtQty.Text = ""
            txtCost.Text = ""
            txtAmount.Text = ""
        End If
    End Sub

    Sub btnSave_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        If Not CheckDate() = "" Then
            Dim TxID As String
            Dim StrTxParam As New StringBuilder()
            Dim ErrorChk As Integer = objINtx.EnumInventoryErrorType.NoError
            Dim strAllowVehicle As String
            If lblCanteenTxID.Text = "" Then
                StrTxParam.Append(lblCanteenTxID.Text)
                StrTxParam.Append("||||||")
                StrTxParam.Append(lstRecDoc.SelectedItem.Value)
                StrTxParam.Append("|")
                StrTxParam.Append(txtRefNo.Text)
                StrTxParam.Append("|")
                StrTxParam.Append(CheckDate())
                StrTxParam.Append("|")
                StrTxParam.Append(strLocation)
                StrTxParam.Append("|")
                StrTxParam.Append(strAccMonth)
                StrTxParam.Append("|")
                StrTxParam.Append(strAccYear)
                StrTxParam.Append("|0|")
                StrTxParam.Append(txtRemarks.Text)
                StrTxParam.Append("||")
            Else

                StrTxParam.Append(lblCanteenTxID.Text)
                StrTxParam.Append("||||||")
                StrTxParam.Append(lstRecDoc.SelectedItem.Value)
                StrTxParam.Append("|")
                StrTxParam.Append(txtRefNo.Text)
                StrTxParam.Append("|")
                StrTxParam.Append(CheckDate())
                StrTxParam.Append("|||||")
                StrTxParam.Append(txtRemarks.Text)
                StrTxParam.Append("||")
            End If

            Try
                intErrNo = objCTtx.mtdUpdCanteenReceiveDetail(strOpCdCanteenTxDet_ADD, _
                                                              strOpCdCanteenTxDet_UPD, _
                                                              strOpCdCanteenTxLine_GET, _
                                                              strCompany, _
                                                              strLocation, _
                                                              strUserId, _
                                                              strAccMonth, _
                                                              strAccYear, _
                                                              StrTxParam.ToString, _
                                                              objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CTCanteenReceive), _
                                                              ErrorChk, _
                                                              TxID)
                lblCanteenTxID.Text = TxID
                If ErrorChk = objINtx.EnumInventoryErrorType.OverFlow Then
                    lblError.Visible = True
                End If

            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ADDNEWSTOCKRECEIVE&errmesg=" & lblErrMessage.Text & "&redirect=CT/trx/CT_trx_CanteenReceiveList.aspx")
                End If
            End Try

            If Not StrTxParam Is Nothing Then
                StrTxParam = Nothing
            End If

            LoadCanteenTxDetails()
            DisplayFromDB()
        End If

    End Sub

    Sub btnConfirm_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim StrTxParam As String
        Dim TxID As String
        Dim ErrorChk As Integer = objINtx.EnumInventoryErrorType.NoError

        If Not LoadDataGrid.Tables(0).Rows.Count > 0 Then
            lblConfirmErr.Visible = True
            Exit Sub
        End If

        Try
            intErrNo = objCTtx.mtdReceiveInvItemLevel(strOpCdCanteenTxLine_GET, _
                                                      strOpCdItem_Details_UPD, _
                                                      strOpCdItem_Details_GET2, _
                                                      strOpCdPR_Count_GET, _
                                                      strOpCdPR_Details_UPD, _
                                                      strCompany, _
                                                      strLocation, _
                                                      strUserId, _
                                                      strAccMonth, _
                                                      strAccYear, _
                                                      lblCanteenTxID.Text, _
                                                      strOpPRLNDet_Details_GET, _
                                                      strOpPRLN_UPD, _
                                                      objCTtx.EnumTransactionAction.Confirm, _
                                                      objCTtx.EnumCanteenReceiveDocType.DispatchAdvice, _
                                                      ErrorChk)
        Catch Exp As System.Exception
            If intErrNo <> -5 Then
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CONFIRMRECEIVE&errmesg=" & lblErrMessage.Text & "&redirect=CT/trx/CT_trx_CanteenReceiveList.aspx")
            End If
        End Try

        If ErrorChk = objINtx.EnumInventoryErrorType.PRNotfound Then
            lblTxError.Visible = True
        ElseIf ErrorChk = objINtx.EnumInventoryErrorType.InsufficientQty Then
            lblTxError.Visible = True
        End If

        If intErrNo = 0 And ErrorChk = objINtx.EnumInventoryErrorType.NoError Then
            StrTxParam = lblCanteenTxID.Text & "|||||||||||||||" & objCTtx.EnumCanteenReceiveStatus.Confirmed
            Try
                intErrNo = objCTtx.mtdUpdCanteenReceiveDetail(strOpCdCanteenTxDet_ADD, _
                                                              strOpCdCanteenTxDet_UPD, _
                                                              strOpCdCanteenTxLine_GET, _
                                                              strCompany, _
                                                              strLocation, _
                                                              strUserId, _
                                                              strAccMonth, _
                                                              strAccYear, _
                                                              StrTxParam, _
                                                              objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CTCanteenReceive), _
                                                              ErrorChk, _
                                                              TxID)

                lblCanteenTxID.Text = TxID
                If ErrorChk = objINtx.EnumInventoryErrorType.OverFlow Then
                    lblError.Visible = True
                End If

            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ADDNEWSTOCKRECEIVE&errmesg=" & lblErrMessage.Text & "&redirect=CT/trx/CT_trx_CanteenReceiveList.aspx")
                End If
            End Try

        End If
        LoadCanteenTxDetails()
        DisplayFromDB()
        DisablePage()
        BindGrid()
    End Sub





    Sub btnDelete_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim StrTxParam As String
        Dim TxID As String
        Dim ErrorChk As Integer = objINtx.EnumInventoryErrorType.NoError

        If lblStatusHid.Text = objCTtx.EnumCanteenReceiveStatus.Deleted Then
            Try
                intErrNo = objCTtx.mtdReceiveInvItemLevel(strOpCdCanteenTxLine_GET, _
                                                          strOpCdItem_Details_UPD, _
                                                          strOpCdItem_Details_GET2, _
                                                          strOpCdPR_Count_GET, _
                                                          strOpCdPR_Details_UPD, _
                                                          strCompany, _
                                                          strLocation, _
                                                          strUserId, _
                                                          strAccMonth, _
                                                          strAccYear, _
                                                          lblCanteenTxID.Text, _
                                                          strOpPRLNDet_Details_GET, _
                                                          strOpPRLN_UPD, _
                                                          objCTtx.EnumTransactionAction.Undelete, _
                                                          objCTtx.EnumCanteenReceiveDocType.DispatchAdvice, _
                                                          ErrorChk)
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=UNDELETESTOCKRECEIVE&errmesg=" & lblErrMessage.Text & "&redirect=CT/trx/CT_trx_CanteenReceiveList.aspx")
                End If
            End Try
            StrTxParam = lblCanteenTxID.Text & "|||||||||||||||" & objCTtx.EnumCanteenReceiveStatus.Active

        ElseIf lblStatusHid.Text = objCTtx.EnumCanteenReceiveStatus.Active Then
            Try
                intErrNo = objCTtx.mtdReceiveInvItemLevel(strOpCdCanteenTxLine_GET, _
                                                          strOpCdItem_Details_UPD, _
                                                          strOpCdItem_Details_GET2, _
                                                          strOpCdPR_Count_GET, _
                                                          strOpCdPR_Details_UPD, _
                                                          strCompany, _
                                                          strLocation, _
                                                          strUserId, _
                                                          strAccMonth, _
                                                          strAccYear, _
                                                          lblCanteenTxID.Text, _
                                                          strOpPRLNDet_Details_GET, _
                                                          strOpPRLN_UPD, _
                                                          objCTtx.EnumTransactionAction.Delete, _
                                                          objCTtx.EnumCanteenReceiveDocType.DispatchAdvice, _
                                                          ErrorChk)
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=DELETENEWSTOCKRECEIVE&errmesg=" & lblErrMessage.Text & "&redirect=CT/trx/CT_trx_CanteenReceiveList.aspx")
                End If
            End Try
            StrTxParam = lblCanteenTxID.Text & "|||||||||||||||" & objCTtx.EnumCanteenReceiveStatus.Deleted & "|"
        End If

        If intErrNo = 0 And Not ErrorChk = objINtx.EnumInventoryErrorType.InsufficientQty Then
            Try
                intErrNo = objCTtx.mtdUpdCanteenReceiveDetail(strOpCdCanteenTxDet_ADD, _
                                                              strOpCdCanteenTxDet_UPD, _
                                                              strOpCdCanteenTxLine_GET, _
                                                              strCompany, _
                                                              strLocation, _
                                                              strUserId, _
                                                              strAccMonth, _
                                                              strAccYear, _
                                                              StrTxParam, _
                                                              objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CTCanteenReceive), _
                                                              ErrorChk, _
                                                              TxID)
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ADDNEWSTOCKRECEIVE&errmesg=" & lblErrMessage.Text & "&redirect=CT/trx/CT_trx_CanteenReceiveList.aspx")
                End If
            End Try
        End If

        If ErrorChk = objINtx.EnumInventoryErrorType.InsufficientQty Then
            lblUnDel.Visible = True
        End If
        LoadCanteenTxDetails()
        DisplayFromDB()
        DisablePage()
        BindGrid()
    End Sub

    Sub btnNew_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Response.Redirect("CT_trx_CanteenReceiveDet.aspx")
    End Sub

    Sub btnBack_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Response.Redirect("../../CT/trx/CT_trx_CanteenReceiveList.aspx")
    End Sub

End Class
