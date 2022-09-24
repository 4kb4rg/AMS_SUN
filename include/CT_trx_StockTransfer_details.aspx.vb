

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic

Public Class CT_StockTransfer : Inherits Page

    Protected WithEvents dgStkTx As DataGrid
    Protected WithEvents lblStckTxID As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents Status As Label
    Protected WithEvents CreateDate As Label
    Protected WithEvents UpdateDate As Label
    Protected WithEvents UpdateBy As Label
    Protected WithEvents lblPrintDate As Label
    Protected WithEvents lblDNNoteID As Label
    Protected WithEvents lblTotAmtFig As Label
    Protected WithEvents ToLocTag As Label
    Protected WithEvents lblAccTag As Label
    Protected WithEvents lblReprint As Label
    Protected WithEvents txtDesc As TextBox
    Protected WithEvents txtRemarks As TextBox
    Protected WithEvents txtQty As TextBox
    Protected WithEvents lblPDateTag As Label
    Protected WithEvents lblDNIDTag As Label
    Protected WithEvents tblAdd As HtmlTable
    Protected WithEvents validateQty As RequiredFieldValidator
    Protected WithEvents lblError As Label
    Protected WithEvents lblStock As Label
    Protected WithEvents lblUnDel As Label
    Protected WithEvents lblConfirmErr As Label
    Protected WithEvents Add As ImageButton
    Protected WithEvents PRDelete As ImageButton
    Protected WithEvents Save As ImageButton
    Protected WithEvents Confirm As ImageButton
    Protected WithEvents Print As ImageButton
    Protected WithEvents DebitNote As ImageButton
    Protected WithEvents Cancel As ImageButton
    Protected WithEvents btnNew As ImageButton
    Protected WithEvents lblStatusHid As Label
    Protected WithEvents lblToLocErr As Label
    Protected WithEvents lblItemCodeErr As Label
    Protected WithEvents lstToLoc As DropDownList
    Protected WithEvents lstItem As DropDownList
    Protected WithEvents FindCT As HtmlInputButton
    Protected WithEvents lblSelect As Label
    Protected WithEvents lblTo As Label
    Protected WithEvents lblLocation As Label
    Protected WithEvents lblPleaseSelectOne As Label
    Protected WithEvents lblDocTitle As Label

    Protected objCTtx As New agri.CT.clstrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objINstp As New agri.IN.clsSetup()
    Dim objCTstp As New agri.CT.clsSetup()
    Dim objGLset As New agri.GL.clsSetup()
    Dim objGLtrx As New agri.GL.clsTrx()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objAdmin As New agri.Admin.clsShare()

    Dim strOpCdStckTxDet_ADD As String = "CT_CLSTRX_STOCKTRANSFER_DETAIL_ADD"
    Dim strOpCdStckTxDet_UPD As String = "CT_CLSTRX_STOCKTRANSFER_DETAIL_UPD"
    Dim strOpCdStckTxLine_GET As String = "CT_CLSTRX_STOCKTRANSFER_LINE_GET"
    Dim strOpCdItem_Details_GET As String = "IN_CLSSETUP_STOCKITEM_DETAILS_GET"
    Dim strOpCdItem_Details_GET2 As String = "CT_CLSSETUP_ITEM_DETAILS_GET_BY_LOCATION"
    Dim strOpCdItem_Details_UPD As String = "IN_CLSTRX_STOCKITEM_DETAIL_UPD"

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
    Dim strARAccMonth As String
    Dim strARAccYear As String
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
        strARAccMonth = Session("SS_ARACCMONTH")
        strARAccYear = Session("SS_ARACCYEAR")
        intCTAR = Session("SS_CTAR")
        intConfigsetting = Session("SS_CONFIGSETTING")

        strLocType = Session("SS_LOCTYPE")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCTAccessRights.CTTransfer), intCTAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            onload_GetLangCap()
            If Not Page.IsPostBack Then
                lblStckTxID.Text = Request.QueryString("Id")
                LoadStockTxDetails()

                If objDataSet.Tables(0).Rows.Count > 0 Then
                    DisplayFromDB(False)
                    DisablePage()
                    BindGrid()
                End If
                BindItemCodeList()
                BindLocDropList()
            End If
            lblError.Visible = False
            lblStock.Visible = False
            lblUnDel.Visible = False
            lblConfirmErr.Visible = False
            lblToLocErr.Visible = False

            DisablePage()
        End If

    End Sub


    Sub onload_GetLangCap()

        GetEntireLangCap()
        
        lblLocation.text = GetCaption(objLangCap.EnumLangCap.Location)
        ToLocTag.Text = lblTo.Text & lblLocation.Text
        lblToLocErr.text = lblPleaseSelectOne.text & lblLocation.text

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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CT_TRX_STOCKTRANSFER_DETAIL_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=ct/trx/ct_trx_StockTransfer_list.aspx")
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
        txtDesc.Enabled = False
        txtRemarks.Enabled = False
        lstToLoc.Enabled = False
        validateQty.Visible = False
        Save.Visible = False
        Confirm.Visible = False
        Print.Visible = False
        DebitNote.Visible = False
        Cancel.Visible = False
        PRDelete.Visible = False
        btnNew.Visible = False
        Select Case Trim(lblStatusHid.Text)
            Case Trim(CStr(objCTtx.EnumStockTransferStatus.Deleted))
                PRDelete.Visible = True
                btnNew.Visible = True
                PRDelete.ImageUrl = "../../images/butt_undelete.gif"
                PRDelete.AlternateText = "Undelete"
                PRDelete.Attributes("onclick") = "javascript:return ConfirmAction('undelete');"
            Case Trim(CStr(objCTtx.EnumStockTransferStatus.Confirmed))
                btnNew.Visible = True
                DebitNote.Visible = True
                Print.Visible = True
            Case Trim(CStr(objCTtx.EnumStockTransferStatus.DBNote))
                btnNew.Visible = True
                Print.Visible = True
            Case Else
                txtDesc.Enabled = True
                txtRemarks.Enabled = True
                lstToLoc.Enabled = True
                validateQty.Visible = True
                Save.Visible = True
                If Trim(lblStckTxID.Text) <> "" Then
                    Confirm.Visible = True
                    Print.Visible = True
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

        If lblStatusHid.Text = CStr(objCTtx.EnumStockTransferStatus.Deleted) _
        Or lblStatusHid.Text = CStr(objCTtx.EnumStockTransferStatus.Cancelled) _
        Or lblStatusHid.Text = CStr(objCTtx.EnumStockTransferStatus.DbNote) Then
            tblAdd.Visible = False
            validateQty.Visible = False
        ElseIf lblStatusHid.Text = CStr(objCTtx.EnumStockTransferStatus.Confirmed) Then
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
                    Case objCTtx.mtdGetStocktransferStatus(objCTtx.EnumStockTransferStatus.Active)
                        DeleteButton = e.Item.FindControl("Delete")
                        DeleteButton.Visible = True
                        DeleteButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                    Case objCTtx.mtdGetStocktransferStatus(objCTtx.EnumStockTransferStatus.Confirmed), _
                        objCTtx.mtdGetStocktransferStatus(objCTtx.EnumStockTransferStatus.Deleted), _
                        objCTtx.mtdGetStocktransferStatus(objCTtx.EnumStockTransferStatus.DBNote), _
                        objCTtx.mtdGetStocktransferStatus(objCTtx.EnumStockTransferStatus.Cancelled)
                        DeleteButton = e.Item.FindControl("Delete")
                        DeleteButton.Visible = False
                End Select
        End Select

    End Sub


    Sub DisplayFromDB(ByVal pv_blnIsRedirect As Boolean)
        lblStatusHid.Text = Trim(objDataSet.Tables(0).Rows(0).Item("Status"))
        Status.Text = objCTtx.mtdGetStocktransferStatus(Trim(objDataSet.Tables(0).Rows(0).Item("Status")))
        CreateDate.Text = objGlobal.GetLongDate(Trim(objDataSet.Tables(0).Rows(0).Item("CreateDate")))
        UpdateDate.Text = objGlobal.GetLongDate(Trim(objDataSet.Tables(0).Rows(0).Item("UpdateDate")))
        UpdateBy.Text = Trim(objDataSet.Tables(0).Rows(0).Item("UserName"))
        txtDesc.Text = Trim(objDataSet.Tables(0).Rows(0).Item("Description"))
        txtRemarks.Text = Trim(objDataSet.Tables(0).Rows(0).Item("Remark"))
        lblTotAmtFig.Text = ObjGlobal.GetIDDecimalSeparator(Trim(objDataSet.Tables(0).Rows(0).Item("TotalAmount"))) 
        If Not objGlobal.mtdEmptyDate(objDataSet.Tables(0).Rows(0).Item("PrintDate")) Then
            lblPDateTag.Visible = True
            lblPrintDate.Text = objGlobal.GetLongDate(Trim(objDataSet.Tables(0).Rows(0).Item("PrintDate")))
        End If
        If Not Trim(objDataSet.Tables(0).Rows(0).Item("DNID")) = "" Then
            lblDNIDTag.Visible = True
            lblDNNoteID.Text = Trim(objDataSet.Tables(0).Rows(0).Item("DNID"))
            If pv_blnIsRedirect = True Then
               Response.Redirect("../../BI/trx/BI_trx_DNDet.aspx?dbnid=" & lblDNNoteID.Text & "&referer=" & Request.ServerVariables("SCRIPT_NAME") & "?Id=" & lblStckTxID.Text)
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
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_CANTEENTRANSFER&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_Stocktransfer_List.aspx")
        End Try
        Return dsGrid
    End Function

    Sub LoadStockTxDetails()

        Dim strOpCdStckTxDet_GET As String = "CT_CLSTRX_STOCKTRANSFER_DETAIL_GET"
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
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_CANTEENTRANSFERLINE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_Stocktransfer_List.aspx")
        End Try

    End Sub


    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        If lblStatusHid.Text = CStr(objCTtx.EnumStockTransferStatus.Active) Then

            Dim strOpCdStckTxLine_Det_GET As String = "CT_CLSTRX_STOCKTRANSFERLINE_DETAILS_GET"
            Dim strOpCdStckTxLine_DEL As String = "CT_CLSTRX_STOCKTRANSFER_LINE_DEL"
            Dim lbl As Label
            Dim ItemCode As String
            Dim strParam As String
            Dim StrTxParam As String
            Dim TxID As String
            Dim ErrorChk As Integer = objCTtx.EnumInventoryErrorType.NoError

            lbl = E.Item.FindControl("ItemCode")
            ItemCode = lbl.Text

            strParam = lblStckTxID.Text & "|" & ItemCode
            Try
                intErrNo = objCTtx.mtdDelStockTransferLn(strOpCdStckTxLine_DEL, _
                                                        strOpCdStckTxLine_Det_GET, _
                                                        strOpCdItem_Details_UPD, _
                                                        strCompany, _
                                                        strLocation, _
                                                        strUserId, _
                                                        strAccMonth, _
                                                        strAccYear, _
                                                        strParam)
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=DEL_CANTEENTRANSFERLINE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_Stocktransfer_List.aspx")
                End If
            End Try
            StrTxParam = lblStckTxID.Text & "|||||||||||||||"

            Try
                intErrNo = objCTtx.mtdUpdStockTransferDetail(strOpCdStckTxDet_ADD, _
                                                             strOpCdStckTxDet_UPD, _
                                                             strOpCdStckTxLine_GET, _
                                                             strCompany, _
                                                             strLocation, _
                                                             strUserId, _
                                                             strAccMonth, _
                                                             strAccYear, _
                                                             strARAccMonth, _
                                                             strARAccYear, _
                                                             StrTxParam, _
                                                             objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CanteenTransfer), _
                                                             ErrorChk, _
                                                             TxID)
                If ErrorChk = objCTtx.EnumInventoryErrorType.OverFlow Then
                    lblError.Visible = True
                End If

                lblStckTxID.Text = TxID
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ADDNEWCANTEENTRANSFER&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_Stocktransfer_List.aspx")
                End If
            End Try
            LoadStockTxDetails()
            DisplayFromDB(False)
            BindGrid()
            BindItemCodeList()
        End If
    End Sub

    Sub BindLocDropList()
        Dim strOpCdLocList_GET As String = "ADMIN_CLSLOC_SYSLOC_LIST_GET"
        Dim dsForDropDown As DataSet
        Dim dsForInactiveItem As DataSet
        Dim SelectedIndex As Integer = -1
        Dim intCnt As Integer
        Dim strParam As String
        Dim strFieldCheck As String
        Dim drinsert As DataRow

        Try
            strParam = "And SY.CompCode = '" & strCompany & "' AND LO.Status = " & objAdminLoc.EnumLocStatus.Active & " AND Not SY.LocCode = '" & strLocation & "'|"
            intErrNo = objAdminLoc.mtdGetCompLocList(strOpCdLocList_GET, strCompany, strLocation, strUserId, dsForDropDown, strParam)

        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ITEMTODROPDOWN&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_Stocktransfer_List.aspx")
        End Try

        For intCnt = 0 To dsForDropDown.Tables(0).Rows.Count - 1
            strFieldCheck = dsForDropDown.Tables(0).Rows(intCnt).Item(0)
            dsForDropDown.Tables(0).Rows(intCnt).Item(0) = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(0))
            dsForDropDown.Tables(0).Rows(intCnt).Item(1) = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(0)) & " ( " & _
                                                           Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(1)) & " )"
            If Not lblStckTxID.Text = "" Then
                If strFieldCheck.Trim.ToUpper = UCase(Trim(objDataSet.Tables(0).Rows(0).Item("ToLocCode"))) Then
                    SelectedIndex = intCnt + 1
                End If
            End If
        Next intCnt
        drinsert = dsForDropDown.Tables(0).NewRow()
        drinsert(0) = ""
        drinsert(1) = lblSelect.text & lblLocation.text
        dsForDropDown.Tables(0).Rows.InsertAt(drinsert, 0)


        lstToLoc.DataSource = dsForDropDown.Tables(0)
        lstToLoc.DataValueField = "LocCode"
        lstToLoc.DataTextField = "LocDesc"
        lstToLoc.DataBind()

        If SelectedIndex = -1 And Not lblStckTxID.Text = "" Then

            Try
                strParam = "And Sy.CompCode = '" & strCompany & "' AND sy.Loccode = '" & _
                                Trim(objDataSet.Tables(0).Rows(0).Item("ToLocCode")) & "'|"
                intErrNo = objAdminLoc.mtdGetCompLocList(strOpCdLocList_GET, strCompany, strLocation, strUserId, dsForInactiveItem, strParam)

            Catch Exp As System.Exception
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ITEMNOTFOUND&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_Stocktransfer_List.aspx")
            End Try

            If dsForInactiveItem.Tables(0).Rows.Count > 0 Then  
                lstToLoc.Items.Add(New ListItem(Trim(dsForInactiveItem.Tables(0).Rows(0).Item(0)) & _
                " (" & objGlobal.mtdGetItemDBStatus(objGlobal.EnumItemDBStatus.NotActive) & ") ", Trim(dsForInactiveItem.Tables(0).Rows(0).Item(0))))
            Else 
                lstToLoc.Items.Add(New ListItem(Trim(objDataSet.Tables(0).Rows(0).Item("ToLocCode")) & _
                    " (" & objGlobal.mtdGetItemDBStatus(objGlobal.EnumItemDBStatus.Unknown) & ") ", Trim(objDataSet.Tables(0).Rows(0).Item("ToLocCode"))))
            End If
            SelectedIndex = lstToLoc.Items.Count - 1
        End If

        lstToLoc.SelectedIndex = SelectedIndex
        If Not dsForDropDown Is Nothing Then
            dsForDropDown = Nothing
        End If
        If Not dsForInactiveItem Is Nothing Then
            dsForInactiveItem = Nothing
        End If
    End Sub

    Function CheckRequiredField() As Boolean
        If Request.Form("lstItem") = "" Then
            lblItemCodeErr.Visible = True
            Return True
        Else
            Return False
        End If
    End Function

    Function CheckLocField() As Boolean
        If lstToLoc.SelectedItem.Value = "" Then
            lblToLocErr.Visible = True
            Return True
        Else
            Return False
        End If
    End Function

    Sub BindItemCodeList()

        Dim strOpCdItem_List_GET As String = "IN_CLSSETUP_STOCKITEM_LIST_GET"
        Dim dsItemCodeDropList As DataSet
        Dim intCnt As Integer
        Dim strparam As String

        strparam = objINstp.EnumInventoryItemType.CanteenItem & "|" & objINstp.EnumStockItemStatus.Active & "|" & lblStckTxID.Text & "|" & "itm.ItemCode"
        Try
            intErrNo = objCTstp.mtdGetFilteredItemList(strOpCdItem_List_GET, _
                                                       strCompany, _
                                                       strLocation, _
                                                       strUserId, _
                                                       strAccMonth, _
                                                       strAccYear, _
                                                       strparam, _
                                                       objCTtx.EnumInventoryTransactionType.StockTransfer, _
                                                       dsItemCodeDropList)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ITEMTOSERVTYPEDROPDOWNLIST&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_Stocktransfer_List.aspx")
        End Try

        For intCnt = 0 To dsItemCodeDropList.Tables(0).Rows.Count - 1
            dsItemCodeDropList.Tables(0).Rows(intCnt).Item(0) = Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item(0))
            dsItemCodeDropList.Tables(0).Rows(intCnt).Item(1) = Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item("ItemCode")) & " ( " _
                                                                & Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item("Description")) & " ), " & _
                                                                FormatCurrency(Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item("AverageCost")), 2) & ", " & _
                                                                FormatNumber(Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item("QtyOnHand")), 5) & " Unit"
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

        DisableItemTable()

        If Not dsItemCodeDropList Is Nothing Then
            dsItemCodeDropList = Nothing
        End If
    End Sub



    Sub btnAdd_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)

        Dim strOpCdStckTxLine_ADD As String = "CT_CLSTRX_STOCKTRANSFER_LINE_ADD"
        Dim strTxLnParam As String
        Dim TxID As String
        Dim StrTxParam As New StringBuilder()
        Dim ErrorChk As Integer = objCTtx.EnumInventoryErrorType.NoError
        Dim strAllowVehicle As String

        If CheckRequiredField() Then
            Exit Sub
        End If

        If CheckLocField() Then
            Exit Sub
        End If

        If lblStckTxID.Text = "" Then
            StrTxParam.Append(lblStckTxID.Text)
            StrTxParam.Append("||||||")
            StrTxParam.Append(lblDNNoteID.Text)
            StrTxParam.Append("|")
            StrTxParam.Append(txtDesc.Text)
            StrTxParam.Append("|")
            StrTxParam.Append(strLocation)
            StrTxParam.Append("|")
            StrTxParam.Append(lstToLoc.SelectedItem.Value)
            StrTxParam.Append("|")
            StrTxParam.Append(strAccMonth)
            StrTxParam.Append("|")
            StrTxParam.Append(strAccYear)
            StrTxParam.Append("|0|")
            StrTxParam.Append(txtRemarks.Text)
            StrTxParam.Append("||")
            Try
                intErrNo = objCTtx.mtdUpdStockTransferDetail(strOpCdStckTxDet_ADD, _
                                                             strOpCdStckTxDet_UPD, _
                                                             strOpCdStckTxLine_GET, _
                                                             strCompany, _
                                                             strLocation, _
                                                             strUserId, _
                                                             strAccMonth, _
                                                             strAccYear, _
                                                             strARAccMonth, _
                                                             strARAccYear, _
                                                             StrTxParam.ToString, _
                                                             objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CanteenTransfer), _
                                                             ErrorChk, _
                                                             TxID)
                lblStckTxID.Text = TxID
                If ErrorChk = objCTtx.EnumInventoryErrorType.OverFlow Then
                    lblError.Visible = True
                End If

            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ADDNEWCANTEENTRANSFER&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_Stocktransfer_List.aspx")
                End If
            End Try

        End If 

        If ErrorChk = objCTtx.EnumInventoryErrorType.noError And intErrNo = 0 Then
            strTxLnParam = lblStckTxID.Text & "|" & Request.Form("lstItem") & "|" & txtQty.Text
            Try
                intErrNo = objCTtx.mtdAddStockTransferLn(strOpCdStckTxLine_ADD, _
                                                        strOpCdItem_Details_GET2, _
                                                        strOpCdItem_Details_UPD, _
                                                        strCompany, _
                                                        strLocation, _
                                                        strUserId, _
                                                        strAccMonth, _
                                                        strAccYear, _
                                                        ErrorChk, _
                                                        strTxLnParam)
                Select Case ErrorChk
                    Case objCTtx.EnumInventoryErrorType.OverFlow
                        lblError.Visible = True
                    Case objCTtx.EnumInventoryErrorType.InsufficientQty
                        lblStock.Visible = True
                End Select

            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ADDHOURLINE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_Stocktransfer_List.aspx")
                End If
            End Try

        End If

        StrTxParam.Remove(0, StrTxParam.Length)
        StrTxParam.Append(lblStckTxID.Text)
        StrTxParam.Append("|||||||||||||||")

        If ErrorChk = objCTtx.EnumInventoryErrorType.noError And intErrNo = 0 Then
            Try
                intErrNo = objCTtx.mtdUpdStockTransferDetail(strOpCdStckTxDet_ADD, _
                                                             strOpCdStckTxDet_UPD, _
                                                             strOpCdStckTxLine_GET, _
                                                             strCompany, _
                                                             strLocation, _
                                                             strUserId, _
                                                             strAccMonth, _
                                                             strAccYear, _
                                                             strARAccMonth, _
                                                             strARAccYear, _
                                                             StrTxParam.ToString, _
                                                             objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CanteenTransfer), _
                                                             ErrorChk, _
                                                            TxID)
                If ErrorChk = objCTtx.EnumInventoryErrorType.OverFlow Then
                    lblError.Visible = True
                End If

                lblStckTxID.Text = TxID
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ADDNEWCANTEENTRANSFER&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_Stocktransfer_List.aspx")
                End If
            End Try

        End If

        If Not StrTxParam Is Nothing Then
            StrTxParam = Nothing
        End If

        LoadStockTxDetails()
        DisplayFromDB(False)
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

        If CheckLocField() Then
            Exit Sub
        End If

        If lblStckTxID.Text = "" Then
            StrTxParam.Append(lblStckTxID.Text)
            StrTxParam.Append("||||||")
            StrTxParam.Append(lblDNNoteID.Text)
            StrTxParam.Append("|")
            StrTxParam.Append(txtDesc.Text)
            StrTxParam.Append("|")
            StrTxParam.Append(strLocation)
            StrTxParam.Append("|")
            StrTxParam.Append(lstToLoc.SelectedItem.Value)
            StrTxParam.Append("|")
            StrTxParam.Append(strAccMonth)
            StrTxParam.Append("|")
            StrTxParam.Append(strAccYear)
            StrTxParam.Append("|0|")
            StrTxParam.Append(txtRemarks.Text)
            StrTxParam.Append("||")
        Else

            StrTxParam.Append(lblStckTxID.Text)
            StrTxParam.Append("||||||")
            StrTxParam.Append(lblDNNoteID.Text)
            StrTxParam.Append("|")
            StrTxParam.Append(txtDesc.Text)
            StrTxParam.Append("||")
            StrTxParam.Append(lstToLoc.SelectedItem.Value)
            StrTxParam.Append("||||")
            StrTxParam.Append(txtRemarks.Text)
            StrTxParam.Append("||")

        End If
        Try
            intErrNo = objCTtx.mtdUpdStockTransferDetail(strOpCdStckTxDet_ADD, _
                                                         strOpCdStckTxDet_UPD, _
                                                         strOpCdStckTxLine_GET, _
                                                         strCompany, _
                                                         strLocation, _
                                                         strUserId, _
                                                         strAccMonth, _
                                                         strAccYear, _
                                                         strARAccMonth, _
                                                         strARAccYear, _
                                                         StrTxParam.ToString, _
                                                         objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CanteenTransfer), _
                                                         ErrorChk, _
                                                         TxID)
            lblStckTxID.Text = TxID
            If ErrorChk = objCTtx.EnumInventoryErrorType.OverFlow Then
                lblError.Visible = True
            End If

        Catch Exp As System.Exception
            If intErrNo <> -5 Then
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ADDNEWCANTEENTRANSFER&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_Stocktransfer_List.aspx")
            End If
        End Try

        If Not StrTxParam Is Nothing Then
            StrTxParam = Nothing
        End If

        LoadStockTxDetails()
        DisplayFromDB(False)
        DisablePage()
    End Sub

    Sub btnConfirm_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim StrTxParam As String
        Dim TxID As String
        Dim ErrorChk As Integer = objCTtx.EnumInventoryErrorType.NoError

        Dim strOpCodeGetAllLines As String = "CT_CLSTRX_CANTEENTRANSFER_LINE_GET_ALL"
        Dim strOpCodeUpdLine As String = "CT_CLSTRX_CANTEENTRANSFER_LINE_SYN"

        If Not LoadDataGrid.Tables(0).Rows.Count > 0 Then
            lblConfirmErr.Visible = True
            Exit Sub
        End If

        Try
            intErrNo = objCTtx.mtdSynStockTransferLn(strOpCodeGetAllLines, _
                                                     strOpCodeUpdLine, _
                                                     strOpCdItem_Details_GET2, _
                                                     strOpCdItem_Details_UPD, _
                                                     strCompany, _
                                                     strLocation, _
                                                     strUserId, _
                                                     ErrorChk, _
                                                     lblStckTxID.Text)
            Select Case ErrorChk
                Case objCTtx.EnumInventoryErrorType.OverFlow
                    lblError.Visible = True
                    Exit Sub
                Case objCTtx.EnumInventoryErrorType.InsufficientQty
                    lblStock.Visible = True
                    Exit Sub
                Case Else
                    lblError.Visible = False
                    lblStock.Visible = False
            End Select
        Catch Exp As System.Exception
            If intErrNo <> -5 Then
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=REMOVEINVENTORYITEM&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_Stocktransfer_List.aspx")
            End If
        End Try

        StrTxParam = lblStckTxID.Text & "|||||||||||||||" & objCTtx.EnumStockTransferStatus.Confirmed
        Try
            intErrNo = objCTtx.mtdUpdStockTransferDetail(strOpCdStckTxDet_ADD, _
                                                         strOpCdStckTxDet_UPD, _
                                                         strOpCdStckTxLine_GET, _
                                                         strCompany, _
                                                         strLocation, _
                                                         strUserId, _
                                                         strAccMonth, _
                                                         strAccYear, _
                                                         strARAccMonth, _
                                                         strARAccYear, _
                                                         StrTxParam, _
                                                         objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CanteenTransfer), _
                                                         ErrorChk, _
                                                         TxID)
            lblStckTxID.Text = TxID
            If ErrorChk = objCTtx.EnumInventoryErrorType.OverFlow Then
                lblError.Visible = True
            End If

        Catch Exp As System.Exception
            If intErrNo <> -5 Then
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ADDNEWCANTEENTRANSFER&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_Stocktransfer_List.aspx")
            End If
        End Try
        LoadStockTxDetails()
        DisplayFromDB(False)
        DisablePage()
        BindGrid()

    End Sub
    Sub btnDebitNote_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim StrTxParam As String
        Dim strDocTypeId As String = objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.BIDebitNote) & "|" & objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.BIDebitNoteLn)
        Dim TxID As String
        Dim ErrorChk As Integer = objCTtx.EnumInventoryErrorType.NoError

        StrTxParam = lblStckTxID.Text & "|||||||||" & lstToLoc.SelectedItem.Value & "||||||" & objCTtx.EnumStockTransferStatus.DbNote
        Try
            intErrNo = objCTtx.mtdUpdStockTransferDetail(strOpCdStckTxDet_ADD, _
                                                         strOpCdStckTxDet_UPD, _
                                                         strOpCdStckTxLine_GET, _
                                                         strCompany, _
                                                         strLocation, _
                                                         strUserId, _
                                                         strAccMonth, _
                                                         strAccYear, _
                                                         strARAccMonth, _
                                                         strARAccYear, _
                                                         StrTxParam, _
                                                         strDocTypeId, _
                                                         ErrorChk, _
                                                         TxID)
            lblStckTxID.Text = TxID
            If ErrorChk = objCTtx.EnumInventoryErrorType.OverFlow Then
                lblError.Visible = True
            End If

        Catch Exp As System.Exception
            If intErrNo <> -5 Then
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ADDNEWCANTEENTRANSFER&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_Stocktransfer_List.aspx")
            End If
        End Try
        LoadStockTxDetails()
        DisplayFromDB(True)
        DisablePage()
        BindGrid()
    End Sub







    Sub btnCancel_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim StrTxParam As String
        Dim TxID As String
        Dim ErrorChk As Integer = objCTtx.EnumInventoryErrorType.NoError

        Try
            intErrNo = objCTtx.mtdAdjustInvItemLevel(strOpCdStckTxLine_GET, _
                                                     strOpCdItem_Details_UPD, _
                                                     strOpCdItem_Details_GET2, _
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
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=REMOVEINVENTORYITEM&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_Stocktransfer_List.aspx")
            End If
        End Try
        StrTxParam = lblStckTxID.Text & "|||||||||||||||" & objCTtx.EnumStockTransferStatus.Cancelled

        If intErrNo = 0 Then
            Try
                intErrNo = objCTtx.mtdUpdStockTransferDetail(strOpCdStckTxDet_ADD, _
                                                             strOpCdStckTxDet_UPD, _
                                                             strOpCdStckTxLine_GET, _
                                                             strCompany, _
                                                             strLocation, _
                                                             strUserId, _
                                                             strAccMonth, _
                                                             strAccYear, _
                                                             strARAccMonth, _
                                                             strARAccYear, _
                                                             StrTxParam, _
                                                             objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CanteenTransfer), _
                                                             ErrorChk, _
                                                             TxID)
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CANCELCANTEENTRANSFER&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_Stocktransfer_List.aspx")
                End If
            End Try
        End If

        If ErrorChk = objCTtx.EnumInventoryErrorType.InsufficientQty Then
            lblUnDel.Visible = True
        End If
        LoadStockTxDetails()
        DisplayFromDB(False)
        DisablePage()
        BindGrid()
    End Sub

    Sub btnDelete_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim StrTxParam As String
        Dim TxID As String
        Dim ErrorChk As Integer = objCTtx.EnumInventoryErrorType.NoError

        If lblStatusHid.Text = CStr(objCTtx.EnumStockTransferStatus.Deleted) Then
            Try
                intErrNo = objCTtx.mtdAdjustInvItemLevel(strOpCdStckTxLine_GET, _
                                                         strOpCdItem_Details_UPD, _
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
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=UNDELETECANTEENTRANSFER&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_Stocktransfer_List.aspx")
                End If
            End Try
            StrTxParam = lblStckTxID.Text & "|||||||||||||||" & objCTtx.EnumStockTransferStatus.Active
        Else
            Try
                intErrNo = objCTtx.mtdAdjustInvItemLevel(strOpCdStckTxLine_GET, _
                                                         strOpCdItem_Details_UPD, _
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
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=DELETENEWCANTEENTRANSFER&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_Stocktransfer_List.aspx")
                End If
            End Try
            StrTxParam = lblStckTxID.Text & "|||||||||||||||" & objCTtx.EnumStockTransferStatus.Deleted & "|"
        End If

        If intErrNo = 0 And Not ErrorChk = objCTtx.EnumInventoryErrorType.InsufficientQty Then
            Try
                intErrNo = objCTtx.mtdUpdStockTransferDetail(strOpCdStckTxDet_ADD, _
                                                             strOpCdStckTxDet_UPD, _
                                                             strOpCdStckTxLine_GET, _
                                                             strCompany, _
                                                             strLocation, _
                                                             strUserId, _
                                                             strAccMonth, _
                                                             strAccYear, _
                                                             strARAccMonth, _
                                                             strARAccYear, _
                                                             StrTxParam, _
                                                             objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CanteenTransfer), _
                                                             ErrorChk, _
                                                             TxID)
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ADDNEWCANTEENTRANSFER&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_Stocktransfer_List.aspx")
                End If
            End Try
        End If

        If ErrorChk = objCTtx.EnumInventoryErrorType.InsufficientQty Then
            lblUnDel.Visible = True
        End If
        LoadStockTxDetails()
        DisplayFromDB(False)
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
        Dim strStockTxId As String

        strStockTxId = Trim(lblStckTxID.Text)

        strUpdString = "where StockTransferID = '" & strStockTxId & "'"
        strStatus = Trim(Status.Text)
        intStatus = CInt(Trim(lblStatusHid.Text))
        strPrintDate = Trim(lblPrintDate.Text)
        strTable = "CT_STOCKTRANSFER"
        strSortLine = ""


        If intStatus = objCTtx.EnumStockTransferStatus.Confirmed Or intStatus = objCTtx.EnumStockTransferStatus.DbNote Then
            If strPrintDate = "" Then
                Try
                    intErrNo = objAdmin.mtdUpdPrintDate(strOpCodePrint, _
                                                        strUpdString, _
                                                        strTable, _
                                                        strCompany, _
                                                        strLocation, _
                                                        strUserId)
                Catch Exp As System.Exception
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CT_TRX_STOCKTRANSFER_UPD_PRINTDATE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_Stocktransfer_List.aspx")
                End Try
                LoadStockTxDetails()
                DisplayFromDB(False)
                DisablePage()
            Else
                strStatus = strStatus & " (re-printed)"
            End If
        End If

        Response.Write("<Script Language=""JavaScript"">window.open(""../reports/CT_Rpt_StockTransferDet.aspx?strStockTxId=" & strStockTxId & _
                       "&strPrintDate=" & strPrintDate & _
                       "&strStatus=" & strStatus & _
                       "&strSortLine=" & strSortLine & _
                       "&strDocTitle=" & lblDocTitle.text & _
                       """, null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")
    End Sub

    Sub btnBack_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)

        Response.Redirect("../../CT/Trx/CT_Trx_Stocktransfer_List.aspx")
    End Sub

    Sub btnNew_Click(ByVal Sender As Object, ByVal e As ImageClickEventArgs)
        Response.Redirect("CT_Trx_StockTransfer_Details.aspx")
    End Sub

End Class
