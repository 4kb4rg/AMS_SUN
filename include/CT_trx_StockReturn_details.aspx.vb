
Imports System
Imports System.Data
Imports System.Text
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic

Public Class CT_ReturnDet : Inherits Page

    Protected WithEvents dgStkTx As DataGrid
    Protected WithEvents lblStckTxID As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents Status As Label
    Protected WithEvents CreateDate As Label
    Protected WithEvents UpdateDate As Label
    Protected WithEvents UpdateBy As Label
    Protected WithEvents lblPrintDate As Label
    Protected WithEvents lblTotAmtFig As Label
    Protected WithEvents lblTotPriceFig As Label
    Protected WithEvents lblReprint As Label
    Protected WithEvents lblAccTag As Label
    Protected WithEvents txtRemarks As TextBox
    Protected WithEvents lblBlkTag As Label
    Protected WithEvents lblVehTag As Label
    Protected WithEvents lblVehExpTag As Label
    Protected WithEvents txtQty As TextBox
    Protected WithEvents lblPDateTag As Label
    Protected WithEvents tblAdd As HtmlTable
    Protected WithEvents lblError As Label
    Protected WithEvents lblStock As Label
    Protected WithEvents lblUnDel As Label
    Protected WithEvents lblQty As Label
    Protected WithEvents lblTxError As Label
    Protected WithEvents Add As ImageButton
    Protected WithEvents PRDelete As ImageButton
    Protected WithEvents Save As ImageButton
    Protected WithEvents Confirm As ImageButton
    Protected WithEvents Print As ImageButton
    Protected WithEvents cancel As ImageButton
    Protected WithEvents btnNew As ImageButton
    Protected WithEvents lblAccCode As Label
    Protected WithEvents lstItem As DropDownList
    Protected WithEvents lstStckIss As DropDownList
    Protected WithEvents lblBlock As Label
    Protected WithEvents lblVehCode As Label
    Protected WithEvents lblVehExp As Label
    Protected WithEvents lblHidStatus As Label
    Protected WithEvents lblCodeWarn As Label
    Protected WithEvents lblConfirmErr As Label
    Protected WithEvents lblStckIssErr As Label
    Protected WithEvents lblItemCodeErr As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents lblDocTitle As Label

    Protected WithEvents RowAcc As HtmlTableRow
    Protected WithEvents RowBlk As HtmlTableRow
    Protected WithEvents RowVeh As HtmlTableRow
    Protected WithEvents RowVehExp As HtmlTableRow

    Protected objCTtx As New agri.CT.clstrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objCTstp As New agri.CT.clsSetup()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objAdmin As New agri.Admin.clsShare()

    Dim strOpCdStckTxLine_GET As String = "CT_CLSTRX_STOCKRETURN_LINE_GET"
    Dim strOpCdStckTxDet_ADD As String = "CT_CLSTRX_STOCKRETURN_DETAIL_ADD"
    Dim strOpCdStckTxDet_UPD As String = "CT_CLSTRX_STOCKRETURN_DETAIL_UPD"
    Dim strOpCdItem_Details_UPD As String = "IN_CLSTRX_STOCKITEM_DETAIL_UPD"
    Dim strOpCdItem_Details_GET As String = "IN_CLSSETUP_STOCKITEM_DETAILS_GET"
    Dim strOpCdIssueLn_Det_GET As String = "CT_CLSTRX_STOCKISSUE_LINE_DETAILS_GET"
    Dim strOpCdIssueLn_Det_UPD As String = "CT_CLSTRX_STOCKISSUE_LINE_DETAILS_UPD"
    Dim strOpCdIssue_Det_GET As String = "CT_CLSTRX_STOCKISSUE_DETAIL_GET"
    Dim strOpCdItem_Details_GET2 As String = "CT_CLSSETUP_ITEM_DETAILS_GET_BY_LOCATION"

    Dim objDataSet As New DataSet()
    Dim dsGrid As New DataSet()
    Dim objLangCapDs As New DataSet()

    Dim intErrNo As Integer
    Dim strParam As String = ""

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strDateFMT As String
    Dim intCTAR As Integer
    Dim intConfigsetting As Integer

    Dim AccTag As String
    Dim VehTag As String
    Dim BlkTag As String
    Dim VehExpTag As String
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
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCTAccessRights.CTReturn), intCTAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            onload_GetLangCap()
            If Not Page.IsPostBack Then
                lblStckTxID.Text = Request.QueryString("Id")
                LoadStockTxDetails()

                If objDataSet.Tables(0).Rows.Count > 0 Then
                    DisplayFromDB()
                    DisablePage()
                    BindGrid()
                End If
                BindStockIssueDropList()
                LoadIssueDetails()
                BindItemCodeList()

                RowAcc.Visible = False
                RowBlk.Visible = False
                RowVeh.Visible = False
                RowVehExp.Visible = False

            End If

            DisablePage()
            lblError.Visible = False
            lblStock.Visible = False
            lblUnDel.Visible = False
            lblStckIssErr.Visible = False
            lblTxError.Visible = False
            lblItemCodeErr.Visible = False
        End If

    End Sub


    Sub onload_GetLangCap()
        GetEntireLangCap()

        Try
            If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
                BlkTag = GetCaption(objLangCap.EnumLangCap.Block)
            Else
                BlkTag = GetCaption(objLangCap.EnumLangCap.SubBlock)
            End If
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CT_TRX_STOCKRETURN_DETAIL_GET_LANGCAP_COSTLEVEL&errmesg=" & lblErrMessage.Text & "&redirect=ct/trx/ct_trx_StockReturn_List.aspx")
        End Try

        AccTag = GetCaption(objLangCap.EnumLangCap.Account)
        VehTag = GetCaption(objLangCap.EnumLangCap.Vehicle)
        VehExpTag = GetCaption(objLangCap.EnumLangCap.VehExpense)
        
        lblAccTag.Text = AccTag & lblCode.Text & " (CR) :* "
        lblVehTag.text = VehTag & lblCode.text & " : "
        lblBlkTag.text = BlkTag & lblCode.text & " : "
        lblVehExpTag.text = VehExpTag & lblCode.text & " : "

        dgStkTx.Columns(2).HeaderText = AccTag & lblCode.Text
        dgStkTx.Columns(3).HeaderText = BlkTag & lblCode.Text
        dgStkTx.Columns(4).HeaderText = VehTag & lblCode.Text
        dgStkTx.Columns(5).HeaderText = VehExpTag & lblCode.Text
        
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CT_TRX_STOCKRETURN_DETAIL_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=ct/trx/ct_trx_StockReturn_List.aspx")
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




    Sub DisablePage()
        txtRemarks.Enabled = False
        Save.Visible = False
        PRDelete.Visible = False
        Confirm.Visible = False
        Print.Visible = False
        cancel.Visible = False
        btnNew.Visible = False
        Select Case Trim(lblHidStatus.Text)
            Case Trim(CStr(objCTtx.EnumStockReturnStatus.Deleted))
                PRDelete.Visible = True
                btnNew.Visible = True
                PRDelete.ImageUrl = "../../images/butt_undelete.gif"
                PRDelete.AlternateText = "Undelete"
                PRDelete.Attributes("onclick") = "javascript:return ConfirmAction('undelete');"
            Case Trim(CStr(objCTtx.EnumStockReturnStatus.Confirmed))
                Print.Visible = True
                btnNew.Visible = True
            Case Else
                txtRemarks.Enabled = True
                Save.Visible = True
                If Trim(lblStckTxID.Text) <> "" Then
                    PRDelete.Visible = True
                    Confirm.Visible = True
                    Print.Visible = True
                    btnNew.Visible = True
                    PRDelete.ImageUrl = "../../images/butt_delete.gif"
                    PRDelete.AlternateText = "Delete"
                    PRDelete.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                End If
        End Select
        DisableItemTable()
    End Sub

    Sub DisableItemTable()

        If lblHidStatus.Text = CStr(objCTtx.EnumStockReturnStatus.Deleted) Or _
            lblHidStatus.Text = CStr(objCTtx.EnumStockReturnStatus.cancelled) Then
            tblAdd.Visible = False
        ElseIf lblHidStatus.Text = CStr(objCTtx.EnumStockReturnStatus.Confirmed) Then
            tblAdd.Visible = False
        Else
            tblAdd.Visible = True

        End If

    End Sub

    Sub DataGrid_ItemCreated(ByVal Sender As Object, ByVal e As DataGridItemEventArgs)
        If lblHidStatus.Text = CStr(objCTtx.EnumStockReturnStatus.Active) Then
            Select Case e.Item.ItemType
                Case ListItemType.Item, ListItemType.AlternatingItem
                    Dim DeleteButton As LinkButton
                    DeleteButton = e.Item.FindControl("Delete")
                    DeleteButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
            End Select
        Else
            Select Case e.Item.ItemType
                Case ListItemType.Item, ListItemType.AlternatingItem
                    Dim DeleteButton As LinkButton
                    DeleteButton = e.Item.FindControl("Delete")
                    DeleteButton.Visible = False
            End Select
        End If
    End Sub


    Sub DisplayFromDB()
        If objDataSet.Tables(0).Rows.Count > 0 Then
            lblHidStatus.Text = Trim(objDataSet.Tables(0).Rows(0).Item("Status"))
            Status.Text = objCTtx.mtdGetStockReturnStatus(Trim(objDataSet.Tables(0).Rows(0).Item("Status")))
            CreateDate.Text = objGlobal.GetLongDate(Trim(objDataSet.Tables(0).Rows(0).Item("CreateDate")))
            UpdateDate.Text = objGlobal.GetLongDate(Trim(objDataSet.Tables(0).Rows(0).Item("UpdateDate")))
            UpdateBy.Text = Trim(objDataSet.Tables(0).Rows(0).Item("UserName"))
            txtRemarks.Text = Trim(objDataSet.Tables(0).Rows(0).Item("Remark"))
            lblTotAmtFig.Text = ObjGlobal.GetIDDecimalSeparator(Trim(objDataSet.Tables(0).Rows(0).Item("TotalAmount")))   
            lblTotPriceFig.Text = ObjGlobal.GetIDDecimalSeparator(Trim(objDataSet.Tables(0).Rows(0).Item("TotalPrice")))   
            If Not objGlobal.mtdEmptyDate(objDataSet.Tables(0).Rows(0).Item("PrintDate")) Then
                lblPDateTag.Visible = True
                lblPrintDate.Text = objGlobal.GetLongDate(Trim(objDataSet.Tables(0).Rows(0).Item("PrintDate")))
            End If

        End If
    End Sub

    Sub BindGrid()
        Dim PageNo As Integer

        dgStkTx.DataSource = LoadDataGrid()
        dgStkTx.DataBind()
    End Sub

    Protected Function LoadDataGrid() As DataSet

        Dim strParam As String

        strParam = Trim(lblStckTxID.Text)

        Try
            intErrNo = objCTtx.mtdGetStockTransactDetails(strOpCdStckTxLine_GET, _
                                                          strCompany, _
                                                          strLocation, _
                                                          strUserId, _
                                                          strAccMonth, _
                                                          strAccYear, _
                                                          strParam, _
                                                          dsGrid)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CT_TRX_STOCKRETURN_GET&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockReturn_List.aspx")
        End Try
        Return dsGrid
    End Function

    Sub LoadStockTxDetails()

        Dim strOpCdStckTxDet_GET As String = "CT_CLSTRX_STOCKRETURN_DETAIL_GET"
        Dim StockCode As String

        strParam = Trim(lblStckTxID.Text)

        Try
            intErrNo = objCTtx.mtdGetStockTransactDetails(strOpCdStckTxDet_GET, _
                                                          strCompany, _
                                                          strLocation, _
                                                          strUserId, _
                                                          strAccMonth, _
                                                          strAccYear, _
                                                          strParam, _
                                                          objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CT_TRX_STOCKRETURNLN_GET&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockReturn_List.aspx")
        End Try

    End Sub


    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        If lblHidStatus.Text = CStr(objCTtx.EnumStockReturnStatus.Active) Then

            Dim strOpCdStckTxLine_Det_GET As String = "CT_CLSTRX_STOCKRETURNLINE_DETAILS_GET"
            Dim strOpCdStckTxLine_DEL As String = "CT_CLSTRX_STOCKRETURN_LINE_DEL"
            Dim lbl As Label
            Dim ItemCode As String
            Dim StockRtnLnID As String
            Dim strParam As String
            Dim StrTxParam As String
            Dim TxID As String
            Dim ErrorChk As Integer = objCTtx.EnumInventoryErrorType.NoError

            lbl = E.Item.FindControl("ItemCode")
            ItemCode = lbl.Text
            lbl = E.Item.FindControl("RtnLnID")
            StockRtnLnID = lbl.Text

            strParam = StockRtnLnID & "|" & ItemCode
            Try
                intErrNo = objCTtx.mtdDelReturnTransactLn(strOpCdStckTxLine_DEL, _
                                                        strOpCdStckTxLine_Det_GET, _
                                                        strOpCdItem_Details_UPD, _
                                                        strOpCdIssueLn_Det_GET, _
                                                        strOpCdIssueLn_Det_UPD, _
                                                        strCompany, _
                                                        strLocation, _
                                                        strUserId, _
                                                        strAccMonth, _
                                                        strAccYear, _
                                                        strParam)
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CT_TRX_STOCKRETURNLN_DEL&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockReturn_List.aspx")
                End If
            End Try

            Try
                StrTxParam = lblStckTxID.Text & "|||||||||||||"

                intErrNo = objCTtx.mtdUpdStockReturnDetail(strOpCdStckTxDet_ADD, _
                                                           strOpCdStckTxDet_UPD, _
                                                           strOpCdStckTxLine_GET, _
                                                           strCompany, _
                                                           strLocation, _
                                                           strUserId, _
                                                           strAccMonth, _
                                                           strAccYear, _
                                                           StrTxParam, _
                                                           objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CanteenReturn), _
                                                           ErrorChk, _
                                                           TxID)

                If ErrorChk = objCTtx.EnumInventoryErrorType.OverFlow Then
                    lblError.Visible = True
                End If

                lblStckTxID.Text = TxID
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CT_TRX_NEWSTOCKRETURN&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockReturn_List.aspx")
                End If
            End Try

            LoadStockTxDetails()
            DisplayFromDB()
            BindGrid()
            BindItemCodeList()
        End If
    End Sub

    Function CheckRequiredField() As Boolean
        If lstStckIss.SelectedItem.Value = "" Then
            lblStckIssErr.Visible = True
            Return True
        ElseIf lstItem.SelectedItem.Value = "" Then
            lblItemCodeErr.Visible = True
            Return True
        Else
            Return False
        End If
    End Function

    Sub CallLoadIssueDetails(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim arrIssueType As Array
        Dim arrIssueType2 As Array

        BindItemCodeList()

        If lstStckIss.SelectedIndex <> 0 Then
            arrIssueType = Split(lstStckIss.SelectedItem.Text.Trim, " (")
            arrIssueType2 = Split(arrIssueType(1), ")")

            If arrIssueType2(0) = objCTtx.mtdGetStockIssueType(objCTtx.EnumStockIssueType.OwnUse) Then
                RowAcc.Visible = True
                RowBlk.Visible = True
                RowVeh.Visible = True
                RowVehExp.Visible = True
            Else
                RowAcc.Visible = False
                RowBlk.Visible = False
                RowVeh.Visible = False
                RowVehExp.Visible = False
            End If
        End If
    End Sub

    Sub ShowIssuedLnDetails(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim arrIssueDet As Array

        If lstItem.SelectedIndex <> 0 Then
            arrIssueDet = Split(lstItem.SelectedItem.Value.Trim, "|")
            lblAccCode.Text = arrIssueDet(2)
            lblBlock.Text = arrIssueDet(3)
            lblVehCode.Text = arrIssueDet(4)
            lblVehExp.Text = arrIssueDet(5)
        End If
    End Sub

    Sub LoadIssueDetails()
        Dim strOpCdStckTxDet_GET As String = "CT_CLSTRX_STOCKISSUE_DETAIL_GET"
        Dim dsIssue As New DataSet()
        strParam = Trim(lstStckIss.SelectedItem.Value)

        Try
            intErrNo = objCTtx.mtdGetStockTransactDetails(strOpCdStckTxDet_GET, _
                                                          strCompany, _
                                                          strLocation, _
                                                          strUserId, _
                                                          strAccMonth, _
                                                          strAccYear, _
                                                          strParam, _
                                                          dsIssue)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CT_TRX_STOCKISSUELINE_GET&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockReturn_List.aspx")
        End Try
    End Sub

    Sub BindStockIssueDropList()

        Dim strOpCdStkIssue_Get As String = "CT_CLSTRX_STOCKISSUE_LIST_GET"
        Dim dsForDropDown As DataSet
        Dim dsForInactiveItem As DataSet
        Dim SelectedIndex As Integer = -1
        Dim intCnt As Integer
        Dim TblAlias As String
        Dim DataTextField As String
        Dim SearchStr As String
        Dim strParam As String
        Dim drinsert As DataRow

        Try
            strParam = objCTtx.EnumStockIssueStatus.Confirmed & "|" & _
                        objCTtx.EnumStockIssueType.All & "|" & _
                        strLocation & "|" & _
                        strAccMonth & "|" & _
                        strAccYear & "|stockIssueID|DESC|"

            intErrNo = objCTtx.mtdGetStockIssueListForReturn(strOpCdStkIssue_Get, strParam, dsForDropDown)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CT_TRX_ITEMTODROPDOWN&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockReturn_List.aspx")
        End Try

        For intCnt = 0 To dsForDropDown.Tables(0).Rows.Count - 1
            dsForDropDown.Tables(0).Rows(intCnt).Item(0) = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(0))
            dsForDropDown.Tables(0).Rows(intCnt).Item("IssueType") = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("StockIssueID")) & " (" & _
                                                                     objCTtx.mtdGetStockIssueType(Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("IssueType"))) & ")"
        Next intCnt

        drinsert = dsForDropDown.Tables(0).NewRow()
        drinsert(0) = ""
        drinsert(1) = "Select one Canteen Issue ID"
        dsForDropDown.Tables(0).Rows.InsertAt(drinsert, 0)

        lstStckIss.DataSource = dsForDropDown.Tables(0)
        lstStckIss.DataValueField = "StockIssueID"
        lstStckIss.DataTextField = "IssueType"
        lstStckIss.DataBind()

        If Not dsForDropDown Is Nothing Then
            dsForDropDown = Nothing
        End If
        If Not dsForInactiveItem Is Nothing Then
            dsForInactiveItem = Nothing
        End If
    End Sub

    Sub BindItemCodeList()

        Dim strOpCdItem_List_GET As String = "CT_CLSTRX_STOCKRETURN_ITEMLIST_GET"
        Dim dsItemCodeDropList As DataSet
        Dim intCnt As Integer
        Dim strparam As String

        strparam = objCTstp.EnumInventoryItemType.CanteenItem & "|" & objCTstp.EnumStockItemStatus.Active & "|" & lstStckIss.SelectedItem.Value & "|" & "itm.ItemCode" & "|" & lblStckTxID.Text
        Try
            intErrNo = objCTstp.mtdGetFilteredItemList(strOpCdItem_List_GET, _
                                                       strCompany, _
                                                       strLocation, _
                                                       strUserId, _
                                                       strAccMonth, _
                                                       strAccYear, _
                                                       strparam, _
                                                       objCTtx.EnumInventoryTransactionType.StockReturn, _
                                                       dsItemCodeDropList)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CT_TRX_ITEMTOSERVTYPEDROPDOWNLIST&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockReturn_List.aspx")
        End Try

        For intCnt = 0 To dsItemCodeDropList.Tables(0).Rows.Count - 1
            dsItemCodeDropList.Tables(0).Rows(intCnt).Item(1) = Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item("ItemCode")) & " ( " _
                                                                & Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item("Description")) & " ), " & _
                                                                FormatCurrency(Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item("cost")), 2) & ", " & _
                                                                FormatNumber(Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item("Qty")), 5) & " Unit"
            dsItemCodeDropList.Tables(0).Rows(intCnt).Item(0) = Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item("StockIssueLnID")) & "|" & _
                                                               Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item("StockIssueID")) & "|" & _
                                                               Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item("Acccode")) & "|" & _
                                                               Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item("BlkCode")) & "|" & _
                                                               Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item("Vehcode")) & "|" & _
                                                               Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item("VehExpCode")) & "||" & _
                                                               Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item("ItemCode"))
        Next intCnt
        Dim drinsert As DataRow

        If lstStckIss.SelectedItem.Value.Trim = "" Then
            dsItemCodeDropList.Tables(0).Clear()
        End If
        drinsert = dsItemCodeDropList.Tables(0).NewRow()
        drinsert(0) = ""
        drinsert(1) = "Select Item Code"
        dsItemCodeDropList.Tables(0).Rows.InsertAt(drinsert, 0)

        lstItem.DataSource = dsItemCodeDropList.Tables(0)
        lstItem.DataValueField = "ItemCode"
        lstItem.DataTextField = "Description"
        lstItem.DataBind()

        DisableItemTable()

        If Not dsItemCodeDropList Is Nothing Then
            dsItemCodeDropList = Nothing
        End If
    End Sub

    Sub btnAdd_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)

        Dim strOpCdStckTxLine_ADD As String = "CT_CLSTRX_STOCKRETURN_LINE_ADD"
        Dim strOpCdIssueLn_Det_GET As String = "CT_CLSTRX_STOCKISSUE_LINE_DETAILS_GET"
        Dim strTxLnParam As New StringBuilder()
        Dim TxID As String
        Dim StrTxParam As New StringBuilder()
        Dim ErrorChk As Integer = objCTtx.EnumInventoryErrorType.NoError
        Dim strAllowVehicle As String
        Dim PRID As String
        Dim arrIssueDet As Array
        arrIssueDet = Split(lstItem.SelectedItem.Value.Trim, "|")

        If CheckRequiredField() Then
            Exit Sub
        End If

        If lblStckTxID.Text = "" Then

            StrTxParam.Append(lblStckTxID.Text.Trim)
            StrTxParam.Append("|||||||")
            StrTxParam.Append(strLocation)
            StrTxParam.Append("|")
            StrTxParam.Append(strAccMonth)
            StrTxParam.Append("|")
            StrTxParam.Append(strAccYear)
            StrTxParam.Append("|0|")
            StrTxParam.Append(txtRemarks.Text)
            StrTxParam.Append("||")
            Try

                intErrNo = objCTtx.mtdUpdStockReturnDetail(strOpCdStckTxDet_ADD, _
                                                           strOpCdStckTxDet_UPD, _
                                                           strOpCdStckTxLine_GET, _
                                                           strCompany, _
                                                           strLocation, _
                                                           strUserId, _
                                                           strAccMonth, _
                                                           strAccYear, _
                                                           StrTxParam.ToString, _
                                                           objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CanteenReturn), _
                                                           ErrorChk, _
                                                           TxID)

                lblStckTxID.Text = TxID
                If ErrorChk = objCTtx.EnumInventoryErrorType.OverFlow Then
                    lblError.Visible = True
                End If

            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CT_TRX_NEWSTOCKRETURN&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockReturn_List.aspx")
                End If
            End Try

        End If

        If ErrorChk = objCTtx.EnumInventoryErrorType.noError And intErrNo = 0 Then

            strTxLnParam.Append(lblStckTxID.Text)
            strTxLnParam.Append("|")
            strTxLnParam.Append(arrIssueDet(7))
            strTxLnParam.Append("|")
            strTxLnParam.Append(txtQty.Text)
            strTxLnParam.Append("|")
            strTxLnParam.Append(arrIssueDet(0))
            strTxLnParam.Append("|")
            strTxLnParam.Append(lstStckIss.SelectedItem.Value)
            strTxLnParam.Append("|")
            strTxLnParam.Append(arrIssueDet(2))
            strTxLnParam.Append("|")
            strTxLnParam.Append(arrIssueDet(3))
            strTxLnParam.Append("|")
            strTxLnParam.Append(arrIssueDet(4))
            strTxLnParam.Append("|")
            strTxLnParam.Append(arrIssueDet(5))
            strTxLnParam.Append("|")
            strTxLnParam.Append(arrIssueDet(6))

            Try
                intErrNo = objCTtx.mtdAddStockReturnLn(strOpCdStckTxLine_ADD, _
                                                        strOpCdItem_Details_GET2, _
                                                        strOpCdIssueLn_Det_GET, _
                                                        strOpCdIssue_Det_GET, _
                                                        strOpCdItem_Details_UPD, _
                                                        strOpCdIssueLn_Det_UPD, _
                                                        strCompany, _
                                                        strLocation, _
                                                        strUserId, _
                                                        strAccMonth, _
                                                        strAccYear, _
                                                        objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CanteenReturnLn), _
                                                        ErrorChk, _
                                                        strTxLnParam.ToString)

            Catch Exp As System.Exception

                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CT_TRX_ADDHOURLINE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockReturn_List.aspx")
                End If

            End Try

            Select Case ErrorChk
                Case objCTtx.EnumInventoryErrorType.OverFlow
                    lblError.Visible = True
                Case objCTtx.EnumInventoryErrorType.InsufficientQty
                    lblStock.Visible = True
            End Select

        End If
        StrTxParam.Remove(0, StrTxParam.Length)
        StrTxParam.Append(lblStckTxID.Text)
        StrTxParam.Append("|||||||||||||")

        If ErrorChk = objCTtx.EnumInventoryErrorType.noError And intErrNo = 0 Then
            Try
                intErrNo = objCTtx.mtdUpdStockReturnDetail(strOpCdStckTxDet_ADD, _
                                                           strOpCdStckTxDet_UPD, _
                                                           strOpCdStckTxLine_GET, _
                                                           strCompany, _
                                                           strLocation, _
                                                           strUserId, _
                                                           strAccMonth, _
                                                           strAccYear, _
                                                           StrTxParam.ToString, _
                                                           objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CanteenReturn), _
                                                           ErrorChk, _
                                                           TxID)

                If ErrorChk = objCTtx.EnumInventoryErrorType.OverFlow Then
                    lblError.Visible = True
                End If

                lblStckTxID.Text = TxID
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CT_TRX_NEWSTOCKRETURN&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockReturn_List.aspx")
                End If
            End Try

        End If

        If Not StrTxParam Is Nothing Then
            StrTxParam = Nothing
        End If

        LoadStockTxDetails()
        DisplayFromDB()
        BindGrid()
        BindItemCodeList()
        DisablePage()
        txtQty.Text = ""

    End Sub

    Sub btnSave_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim TxID As String
        Dim StrTxParam As New StringBuilder()
        Dim ErrorChk As Integer = objCTtx.EnumInventoryErrorType.NoError
        Dim strAllowVehicle As String

        StrTxParam.Append(lblStckTxID.Text.Trim)
        StrTxParam.Append("|||||||")
        StrTxParam.Append(strLocation)
        StrTxParam.Append("|")
        StrTxParam.Append(strAccMonth)
        StrTxParam.Append("|")
        StrTxParam.Append(strAccYear)
        StrTxParam.Append("|0|")
        StrTxParam.Append(txtRemarks.Text)
        StrTxParam.Append("||")

        Try

            intErrNo = objCTtx.mtdUpdStockReturnDetail(strOpCdStckTxDet_ADD, _
                                                       strOpCdStckTxDet_UPD, _
                                                       strOpCdStckTxLine_GET, _
                                                       strCompany, _
                                                       strLocation, _
                                                       strUserId, _
                                                       strAccMonth, _
                                                       strAccYear, _
                                                       StrTxParam.ToString, _
                                                       objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CanteenReturn), _
                                                       ErrorChk, _
                                                       TxID)

            lblStckTxID.Text = TxID
            If ErrorChk = objCTtx.EnumInventoryErrorType.OverFlow Then
                lblError.Visible = True
            End If

        Catch Exp As System.Exception
            If intErrNo <> -5 Then
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CT_TRX_STOCKRETURN_UPD_DET&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockReturn_List.aspx")
            End If
        End Try


        If Not StrTxParam Is Nothing Then
            StrTxParam = Nothing
        End If

        LoadStockTxDetails()
        DisplayFromDB()
        DisablePage()

    End Sub

    Sub btnCancel_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim TxID As String
        Dim StrTxParam As String
        Dim ErrorChk As Integer = objCTtx.EnumInventoryErrorType.NoError
        If lblStckTxID.Text = "" Then

        End If
        Try
            intErrNo = objCTtx.mtdReturnInvItemLevel(strOpCdStckTxLine_GET, _
                                                     strOpCdItem_Details_UPD, _
                                                     strOpCdIssueLn_Det_GET, _
                                                     strOpCdIssueLn_Det_UPD, _
                                                     strOpCdItem_Details_GET, _
                                                     strCompany, _
                                                     strLocation, _
                                                     strUserId, _
                                                     strAccMonth, _
                                                     strAccYear, _
                                                     lblStckTxID.Text, _
                                                     objCTtx.EnumTransactionAction.Cancel, _
                                                     ErrorChk)

        Catch Exp As System.Exception
            If intErrNo <> -5 Then
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CT_TRX_CONFIRMRETURN&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockReturn_List.aspx")
            End If
        End Try
        If intErrNo = 0 And ErrorChk = objCTtx.EnumInventoryErrorType.NoError Then
            Try
                StrTxParam = lblStckTxID.Text & "|||||||||||||" & objCTtx.EnumStockReturnStatus.Cancelled

                intErrNo = objCTtx.mtdUpdStockReturnDetail(strOpCdStckTxDet_ADD, _
                                                           strOpCdStckTxDet_UPD, _
                                                           strOpCdStckTxLine_GET, _
                                                           strCompany, _
                                                           strLocation, _
                                                           strUserId, _
                                                           strAccMonth, _
                                                           strAccYear, _
                                                           StrTxParam, _
                                                           objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CanteenReturn), _
                                                           ErrorChk, _
                                                           TxID)

                If ErrorChk = objCTtx.EnumInventoryErrorType.OverFlow Then
                    lblError.Visible = True
                End If

                lblStckTxID.Text = TxID
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CT_TRX_ADDNEWSTOCKRETURN&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockReturn_List.aspx")
                End If
            End Try

        End If

        If Not StrTxParam Is Nothing Then
            StrTxParam = Nothing
        End If

        LoadStockTxDetails()
        DisplayFromDB()
        DisablePage()

    End Sub

    Sub btnConfirm_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim StrTxParam As String
        Dim TxID As String
        Dim ErrorChk As Integer = objCTtx.EnumInventoryErrorType.NoError
        Dim strOpCodeTxLnSyn As String = "CT_CLSTRX_CANTEENRETURN_LINE_SYN"
        Dim strOpCodeIssLnGet As String = "CT_CLSTRX_CANTEENISSUE_LINE_DETAILS_GET2"
        If Not LoadDataGrid.Tables(0).Rows.Count > 0 Then
            lblConfirmErr.Visible = True
            Exit Sub
        End If
        Try
            intErrNo = objCTtx.mtdConfirmReturnDoc(strOpCdStckTxLine_GET, _
                                                   strOpCdItem_Details_GET2, _
                                                   strOpCodeIssLnGet, _
                                                   strOpCdItem_Details_UPD, _
                                                   strOpCodeTxLnSyn, _
                                                   strCompany, _
                                                   strLocation, _
                                                   strUserId, _
                                                   strAccMonth, _
                                                   strAccYear, _
                                                   lblStckTxID.Text, _
                                                   ErrorChk)
        Catch Exp As System.Exception
            If intErrNo <> -5 Then
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CONFIRMRETURN&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockReturn_List.aspx")
            End If
        End Try
        Select Case ErrorChk
            Case objCTtx.EnumInventoryErrorType.OverFlow
                lblTxError.Visible = True
            Case objCTtx.EnumInventoryErrorType.InsufficientQty
                lblTxError.Visible = True
        End Select

        If intErrNo = 0 And ErrorChk = objCTtx.EnumInventoryErrorType.NoError Then
            Try
                StrTxParam = lblStckTxID.Text & "|||||||||||||" & objCTtx.EnumStockReturnStatus.Confirmed
                intErrNo = objCTtx.mtdUpdStockReturnDetail(strOpCdStckTxDet_ADD, _
                                                           strOpCdStckTxDet_UPD, _
                                                           strOpCdStckTxLine_GET, _
                                                           strCompany, _
                                                           strLocation, _
                                                           strUserId, _
                                                           strAccMonth, _
                                                           strAccYear, _
                                                           StrTxParam, _
                                                           objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CanteenReturn), _
                                                           ErrorChk, _
                                                           TxID)

                If ErrorChk = objCTtx.EnumInventoryErrorType.OverFlow Then
                    lblError.Visible = True
                End If

                lblStckTxID.Text = TxID
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CT_TRX_NEWSTOCKRETURN&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockReturn_List.aspx")
                End If
            End Try


        End If
        LoadStockTxDetails()
        DisplayFromDB()
        DisablePage()
        BindGrid()

    End Sub

    Sub btnPrint_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim StrTxParam As String
        Dim TxID As String
        Dim ErrorChk As Integer = objCTtx.EnumInventoryErrorType.NoError

        If lblPrintDate.Text = "" Then
            Try
                StrTxParam = lblStckTxID.Text & "||||||||||||" & Now() & "|"

                intErrNo = objCTtx.mtdUpdStockReturnDetail(strOpCdStckTxDet_ADD, _
                                                           strOpCdStckTxDet_UPD, _
                                                           strOpCdStckTxLine_GET, _
                                                           strCompany, _
                                                           strLocation, _
                                                           strUserId, _
                                                           strAccMonth, _
                                                           strAccYear, _
                                                           StrTxParam, _
                                                           objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CanteenReturn), _
                                                           ErrorChk, _
                                                           TxID)

                If ErrorChk = objCTtx.EnumInventoryErrorType.OverFlow Then
                    lblError.Visible = True
                End If

                lblStckTxID.Text = TxID
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CT_TRX_NEWSTOCKRETURN_PRINT&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockReturn_List.aspx")
                End If
            End Try
        Else
            lblReprint.Visible = True
        End If

        LoadStockTxDetails()
        DisplayFromDB()
        DisablePage()

    End Sub

    Sub btnDelete_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim StrTxParam As String
        Dim TxID As String
        Dim ErrorChk As Integer = objCTtx.EnumInventoryErrorType.NoError

        If lblHidStatus.Text = CStr(objCTtx.EnumStockReturnStatus.Deleted) Then
            Try

                intErrNo = objCTtx.mtdReturnInvItemLevel(strOpCdStckTxLine_GET, _
                                                         strOpCdItem_Details_UPD, _
                                                         strOpCdIssueLn_Det_GET, _
                                                         strOpCdIssueLn_Det_UPD, _
                                                         strOpCdItem_Details_GET2, _
                                                         strCompany, _
                                                         strLocation, _
                                                         strUserId, _
                                                         strAccMonth, _
                                                         strAccYear, _
                                                         lblStckTxID.Text, _
                                                         objCTtx.EnumTransactionAction.Undelete, _
                                                         ErrorChk)
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CT_TRX_UNDELETESTOCKRETURN&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockReturn_List.aspx")
                End If
            End Try
            StrTxParam = lblStckTxID.Text & "|||||||||||||" & objCTtx.EnumStockReturnStatus.Active

        ElseIf lblHidStatus.Text = CStr(objCTtx.EnumStockReturnStatus.Active) Then
            Try
                intErrNo = objCTtx.mtdReturnInvItemLevel(strOpCdStckTxLine_GET, _
                                                         strOpCdItem_Details_UPD, _
                                                         strOpCdIssueLn_Det_GET, _
                                                         strOpCdIssueLn_Det_UPD, _
                                                         strOpCdItem_Details_GET2, _
                                                         strCompany, _
                                                         strLocation, _
                                                         strUserId, _
                                                         strAccMonth, _
                                                         strAccYear, _
                                                         lblStckTxID.Text, _
                                                         objCTtx.EnumTransactionAction.Delete, _
                                                         ErrorChk)
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CT_TRX_DELETENEWSTOCKRETURN&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockReturn_List.aspx")
                End If
            End Try
            StrTxParam = lblStckTxID.Text & "|||||||||||||" & objCTtx.EnumStockReturnStatus.Deleted
        End If

        If ErrorChk = objCTtx.EnumInventoryErrorType.noError And intErrNo = 0 Then
            Try
                intErrNo = objCTtx.mtdUpdStockReturnDetail(strOpCdStckTxDet_ADD, _
                                                           strOpCdStckTxDet_UPD, _
                                                           strOpCdStckTxLine_GET, _
                                                           strCompany, _
                                                           strLocation, _
                                                           strUserId, _
                                                           strAccMonth, _
                                                           strAccYear, _
                                                           StrTxParam, _
                                                           objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CanteenReturn), _
                                                           ErrorChk, _
                                                           TxID)

                If ErrorChk = objCTtx.EnumInventoryErrorType.OverFlow Then
                    lblError.Visible = True
                End If

                lblStckTxID.Text = TxID
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CT_TRX_NEWSTOCKRETURN&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockReturn_List.aspx")
                End If
            End Try
        End If

        If ErrorChk = objCTtx.EnumInventoryErrorType.InsufficientQty Then
            lblUnDel.Visible = True
        End If
        LoadStockTxDetails()
        DisplayFromDB()
        DisablePage()
        BindGrid()
    End Sub


    Sub btnPreview_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim strOpCodePrint As String = "ADMIN_SHARE_UPD_PRINTDATE"
        Dim strUpdString As String = ""
        Dim strStatus As String
        Dim intStatus As Integer
        Dim strSortLine As String
        Dim strPrintDate As String
        Dim strTable As String
        Dim strStockReturnId As String

        strStockReturnId = Trim(lblStckTxID.Text)
        strUpdString = "where StockRtnID = '" & strStockReturnId & "'"
        strStatus = Trim(Status.Text)
        intStatus = CInt(Trim(lblHidStatus.Text))
        strPrintDate = Trim(lblPrintDate.Text)
        strTable = "CT_STOCKRTN"
        strSortLine = ""

        If intStatus = objCTtx.EnumStockReturnStatus.Confirmed Then
            If strPrintDate = "" Then
                Try
                    intErrNo = objAdmin.mtdUpdPrintDate(strOpCodePrint, _
                                                        strUpdString, _
                                                        strTable, _
                                                        strCompany, _
                                                        strLocation, _
                                                        strUserId)
                Catch Exp As System.Exception
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CT_TRX_STOCKRETURN_UPD_PRINTDATE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_trx_StockReturn_List.aspx")
                End Try
                LoadStockTxDetails()
                DisplayFromDB()
                DisablePage()
                BindGrid()
            Else
                strStatus = strStatus & " (re-printed)"
            End If
        End If

        Response.Write("<Script Language=""JavaScript"">window.open(""../reports/CT_Rpt_StockReturnDet.aspx?strStockReturnId=" & strStockReturnId & _
                       "&strPrintDate=" & strPrintDate & _
                       "&strStatus=" & strStatus & _
                       "&strSortLine=" & strSortLine & _
                       "&strAccountTag=" & AccTag & _
                       "&strBlockTag=" & BlkTag & _
                       "&strVehicleTag=" & VehTag & _
                       "&strVehExpTag=" & VehExpTag & _
                       "&strDocTitle=" & lblDocTitle.Text & _
                       """, null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")
    End Sub

    Sub btnBack_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Response.Redirect("../../CT/Trx/CT_Trx_StockReturn_List.aspx")
    End Sub

    Sub btnNew_Click(ByVal Sender As Object, ByVal e As ImageClickEventArgs)
        Response.Redirect("CT_Trx_StockReturn_Details.aspx")
    End Sub

End Class
