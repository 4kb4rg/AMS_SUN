

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

Public Class CT_CRADetails : Inherits Page
    Protected WithEvents dgStkTx As DataGrid
    Protected WithEvents lblStckTxID As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents Status As Label
    Protected WithEvents CreateDate As Label
    Protected WithEvents UpdateDate As Label
    Protected WithEvents UpdateBy As Label
    Protected WithEvents lblPrintDate As Label
    Protected WithEvents lblTotAmtFig As Label
    Protected WithEvents lblReprint As Label
    Protected WithEvents lblDate As Label
    Protected WithEvents txtQty As TextBox
    Protected WithEvents lblPDateTag As Label
    Protected WithEvents tblAdd As HtmlTable
    Protected WithEvents lblError As Label
    Protected WithEvents lblStock As Label
    Protected WithEvents lblUnDel As Label
    Protected WithEvents lblDoc As Label
    Protected WithEvents lblPR As Label
    Protected WithEvents lblTxError As Label
    Protected WithEvents lblStatusHid As Label
    Protected WithEvents lblConfirmErr As Label
    Protected WithEvents lblItemCodeErr As Label
    Protected WithEvents txtRemarks As TextBox
    Protected WithEvents Add As ImageButton
    Protected WithEvents PRDelete As ImageButton
    Protected WithEvents Save As ImageButton
    Protected WithEvents Confirm As ImageButton
    Protected WithEvents Print As ImageButton
    Protected WithEvents btnNew As ImageButton
    Protected WithEvents lstItem As DropDownList
    Protected WithEvents lstDoc As DropDownList
    Protected WithEvents FindCT As HtmlInputButton
    Protected WithEvents lblDocTitle As Label

    Protected objCT As New agri.CT.clstrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objCTstp As New agri.CT.clsSetup()
    Dim objGLset As New agri.GL.clsSetup()
    Dim objGLtrx As New agri.GL.clsTrx()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objAdmin As New agri.Admin.clsShare()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()

    Dim strOpCdStckTxDet_ADD As String = "IN_CLSTRX_ITEMRETURNADVICE_ADD"
    Dim strOpCdStckTxDet_UPD As String = "IN_CLSTRX_ITEMRETURNADVICE_SELECTIVE_UPD"
    Dim strOpCdStckTxLine_GET As String = "IN_CLSTRX_ITEMRETURNADVICE_LINE_GET"
    Dim strOpCdItem_Details_GET As String = "IN_CLSSETUP_STOCKITEM_DETAILS_GET"
    Dim strOpCdItem_Details_UPD As String = "IN_CLSTRX_STOCKITEM_DETAIL_UPD"
    Dim strOpCdStckRecvLine_Det_GET As String = "CT_CLSTRX_CANTEENRECEIVELINE_DETAILS_GET"
    Dim strOpCdItem_Details_GET2 As String = "CT_CLSSETUP_ITEM_DETAILS_GET_BY_LOCATION"
    Dim strOpCdStckRecvLine_Det_UPD As String = "CT_CLSTRX_CANTEENRECEIVE_LINE_UPD"

    Dim objDataSet As New DataSet()
    Dim dsGrid As New DataSet()
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

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCTAccessRights.CTReturnAdvice), intCTAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            If Not Page.IsPostBack Then
                lblStckTxID.Text = Request.QueryString("Id")
                LoadStockTxDetails()
                If objDataSet.Tables(0).Rows.Count > 0 Then
                    DisplayFromDB()
                    DisablePage()
                    BindGrid()
                End If
                BindDocList()
                BindItemCodeList()
            End If
            lblError.Visible = False
            lblStock.Visible = False
            lblUnDel.Visible = False
            lblTxError.Visible = False
            lblConfirmErr.Visible = False
            lblItemCodeErr.Visible = False
            lblPR.Visible = False

            DisablePage()
        End If

    End Sub

    Sub DisablePage()
        txtRemarks.Enabled = False
        Save.Visible = False
        Confirm.Visible = False
        PRDelete.Visible = False
        Print.Visible = False
        btnNew.Visible = False
        Select Case Trim(lblStatusHid.Text)
            Case Trim(CStr(objCT.EnumCanRetAdvStatus.Deleted))
                PRDelete.Visible = True
                btnNew.Visible = True
                PRDelete.ImageUrl = "../../images/butt_undelete.gif"
                PRDelete.AlternateText = "Undelete"
                PRDelete.Attributes("onclick") = "javascript:return ConfirmAction('undelete');"
            Case Trim(CStr(objCT.EnumCanRetAdvStatus.Confirmed))
                btnNew.Visible = True
                Print.Visible = True
            Case Else
                txtRemarks.Enabled = True
                Save.Visible = True
                If Trim(lblStckTxID.Text) <> "" Then
                    Confirm.Visible = True
                    PRDelete.Visible = True
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

        If lblStatusHid.Text = CStr(objCT.EnumCanRetAdvStatus.Deleted) Then
            tblAdd.Visible = False
        ElseIf lblStatusHid.Text = CStr(objCT.EnumCanRetAdvStatus.Confirmed) Then
            tblAdd.Visible = False
        Else
            tblAdd.Visible = True

        End If

    End Sub

    Sub DataGrid_ItemCreated(ByVal Sender As Object, ByVal e As DataGridItemEventArgs)

        Dim DeleteButton As LinkButton
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                DeleteButton = e.Item.FindControl("Delete")
                If Trim(lblStatusHid.Text) = Trim(CStr(objCT.EnumCanRetAdvStatus.Active)) Then
                    DeleteButton.Visible = True
                    DeleteButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                Else
                    DeleteButton.Visible = False
                End If
 
        End Select

    End Sub


    Sub DisplayFromDB()
        Status.Text = objCT.mtdGetCanRetAdvStatus(Trim(objDataSet.Tables(0).Rows(0).Item("Status")))
        lblStatusHid.Text = Trim(objDataSet.Tables(0).Rows(0).Item("Status"))
        CreateDate.Text = objGlobal.GetLongDate(Trim(objDataSet.Tables(0).Rows(0).Item("CreateDate")))
        UpdateDate.Text = objGlobal.GetLongDate(Trim(objDataSet.Tables(0).Rows(0).Item("UpdateDate")))
        UpdateBy.Text = Trim(objDataSet.Tables(0).Rows(0).Item("UserName"))
        txtRemarks.Text = Trim(objDataSet.Tables(0).Rows(0).Item("Remark"))
        lblTotAmtFig.Text = ObjGlobal.GetIDDecimalSeparator(Round(objDataSet.Tables(0).Rows(0).Item("TotalAmount"),0))
        If Not objGlobal.mtdEmptyDate(objDataSet.Tables(0).Rows(0).Item("PrintDate")) Then
            lblPDateTag.Visible = True
            lblPrintDate.Text = objGlobal.GetLongDate(Trim(objDataSet.Tables(0).Rows(0).Item("PrintDate")))
        End If
    End Sub

    Sub BindGrid()
        Dim PageNo As Integer

        dgStkTx.DataSource = LoadDataGrid()
        dgStkTx.DataBind()
    End Sub

    Protected Function LoadDataGrid() As DataSet

        Dim strParam As String
        Dim intCnt As Integer

        strParam = Trim(lblStckTxID.Text)

        Try
            intErrNo = objCT.mtdGetStockTransactDetails(strOpCdStckTxLine_GET, _
                                                          strCompany, _
                                                          strLocation, _
                                                          strUserId, _
                                                          strAccMonth, _
                                                          strAccYear, _
                                                          strParam, _
                                                          dsGrid)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_STOCKRETURNADVICE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_CRAList.aspx")
        End Try

        For intCnt = 0 To dsGrid.Tables(0).Rows.Count - 1
            dsGrid.Tables(0).Rows(intCnt).Item("Description") = Trim(dsGrid.Tables(0).Rows(intCnt).Item("ItemCode")) & " (" & _
                                                                Trim(dsGrid.Tables(0).Rows(intCnt).Item("Description")) & ")"
        Next intCnt

        Return dsGrid
    End Function

    Sub LoadStockTxDetails()

        Dim strOpCdStckTxDet_GET As String = "IN_CLSTRX_ITEMRETURNADVICE_DETAIL_GET"
        Dim StockCode As String
        strParam = Trim(lblStckTxID.Text)
        Try
            intErrNo = objCT.mtdGetStockTransactDetails(strOpCdStckTxDet_GET, _
                                                          strCompany, _
                                                          strLocation, _
                                                          strUserId, _
                                                          strAccMonth, _
                                                          strAccYear, _
                                                          strParam, _
                                                          objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_STOCKRETURNADVICELINE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_CRAList.aspx")
        End Try

    End Sub


    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        If lblStatusHid.Text = CStr(objCT.EnumCanRetAdvStatus.Active) Then

            Dim strOpCdStckTxLine_Det_GET As String = "IN_CLSTRX_ITEMRETURNADVICELN_GET"
            Dim strOpCdStckTxLine_DEL As String = "IN_CLSTRX_ITEMRETURNADVICELN_DEL"
            Dim lbl As Label
            Dim ItemCode As String
            Dim StockRetAdvLnID As String
            Dim strParam As String
            Dim StrTxParam As String
            Dim TxID As String
            Dim ErrorChk As Integer = objCT.EnumInventoryErrorType.NoError

            lbl = E.Item.FindControl("ItemCode")
            ItemCode = lbl.Text
            lbl = E.Item.FindControl("RtnAdvLnID")
            StockRetAdvLnID = lbl.Text

            strParam = StockRetAdvLnID & "|" & ItemCode
            Try
                intErrNo = objCT.mtdDelRtnAdvTransactLn(strOpCdStckTxLine_DEL, _
                                                        strOpCdStckTxLine_Det_GET, _
                                                        strOpCdItem_Details_UPD, _
                                                        strOpCdStckRecvLine_Det_GET, _
                                                        strOpCdStckRecvLine_Det_UPD, _
                                                        strCompany, _
                                                        strLocation, _
                                                        strUserId, _
                                                        strAccMonth, _
                                                        strAccYear, _
                                                        strParam)
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=DEL_STOCKRETURNADVICELINE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_CRAList.aspx")
                End If
            End Try
            StrTxParam = lblStckTxID.Text & "|||||||||||||"
            Try
                intErrNo = objCT.mtdUpdStockRtnAdvDetail(strOpCdStckTxDet_ADD, _
                                                            strOpCdStckTxDet_UPD, _
                                                            strOpCdStckTxLine_GET, _
                                                            strCompany, _
                                                            strLocation, _
                                                            strUserId, _
                                                            strAccMonth, _
                                                            strAccYear, _
                                                            StrTxParam, _
                                                            objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CanteenReturnAdvice), _
                                                            ErrorChk, _
                                                            TxID)
                lblStckTxID.Text = TxID
                If ErrorChk = objCT.EnumInventoryErrorType.OverFlow Then
                    lblError.Visible = True
                End If

            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ADDNEWSTOCKRETURNADVICE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_CRAList.aspx")
                End If
            End Try

            LoadStockTxDetails()
            DisplayFromDB()
            BindGrid()
            BindItemCodeList()
        End If
    End Sub

    Function CheckRequiredField() As Boolean
        If lstItem.SelectedItem.Value = "" Then
            lblItemCodeErr.Visible = True
            Return True
        Else
            Return False
        End If
    End Function

    Sub RebindItemList(ByVal sender As Object, ByVal e As System.EventArgs)
        BindItemCodeList()
        If lstDoc.SelectedItem.Value = "NoDoc|" Then
            FindCT.Visible = True
        Else
            FindCT.Visible = False
        End If

    End Sub

    Sub BindItemCodeList()
        Dim strOpCdItem_List_GET As String
        Dim dsItemCodeDropList As DataSet
        Dim intCnt As Integer
        Dim strparam As String
        Dim DocID As Array
        Dim strRecvID As String
        Dim strItemCode As String
        Dim drinsert As DataRow

        If lstDoc.SelectedItem.Value = "NoDoc|" Then
            strOpCdItem_List_GET = "IN_CLSSETUP_STOCKITEM_LIST_GET"

        Else
            strOpCdItem_List_GET = "CT_CLSTRX_STOCKRTNADV_ITEMLIST_GET"
        End If


        DocID = Split(lstDoc.SelectedItem.Value.Trim, "|")
        strparam = objCTstp.EnumInventoryItemType.CanteenItem & _
                    "|" & objCTstp.EnumStockItemStatus.Active & "|" & lblStckTxID.Text & "|" & "itm.ItemCode" & "|" & DocID(0)

        Try
            intErrNo = objCTstp.mtdGetFilteredItemList(strOpCdItem_List_GET, _
                                                       strCompany, _
                                                       strLocation, _
                                                       strUserId, _
                                                       strAccMonth, _
                                                       strAccYear, _
                                                       strparam, _
                                                       objCT.EnumInventoryTransactionType.StockReturnAdvice, _
                                                       dsItemCodeDropList)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ITEMTOSERVTYPEDROPDOWNLIST&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_CRAList.aspx")
        End Try
        If lstDoc.SelectedItem.Value = "NoDoc|" Then
            For intCnt = 0 To dsItemCodeDropList.Tables(0).Rows.Count - 1
                strItemCode = Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item(0))

                dsItemCodeDropList.Tables(0).Rows(intCnt).Item(0) = strItemCode
                dsItemCodeDropList.Tables(0).Rows(intCnt).Item(1) = strItemCode & " ( " & Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item("Description")) & " ), " & _
                                                                    FormatCurrency(Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item("AverageCost")), 2) & ", " & _
                                                                    FormatNumber(Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item("QtyOnHand")), 5) & " Unit"
            Next intCnt

        Else
            For intCnt = 0 To dsItemCodeDropList.Tables(0).Rows.Count - 1
                strItemCode = Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item(0))
                strRecvID = Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item(4))
                dsItemCodeDropList.Tables(0).Rows(intCnt).Item(0) = strItemCode & "|" & strRecvID
                dsItemCodeDropList.Tables(0).Rows(intCnt).Item(1) = strItemCode & " ( " & Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item("Description")) & " ), " & _
                                                                    FormatCurrency(Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item("AverageCost")), 2) & ", " & _
                                                                    FormatNumber(Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item("QtyOnHand")), 5) & " Unit"
            Next intCnt
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

    Sub BindDocList()

        Dim strOpCdPR_List_GET As String = "CT_CLSTRX_STOCKRETURN_STKRECEIVE_LIST_GET"
        Dim dsDropList As DataSet
        Dim drinsert As DataRow
        Dim intCnt As Integer
        Dim strparam As String
        Dim strRecvID As String
        Dim strDAID As String

        strparam = objCT.EnumCanteenReceiveDocType.DispatchAdvice & "|" & objCT.EnumCanteenReceiveStatus.Confirmed & "|" & strLocation & "|" & "tx.CanteenRcvID|DESC"
        Try
            intErrNo = objCT.mtdStockReceiveForReturnGet(strOpCdPR_List_GET, strparam, dsDropList)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ITEMTOPRDROPDOWNLIST&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_CRAList.aspx")
        End Try

        For intCnt = 0 To dsDropList.Tables(0).Rows.Count - 1
            strRecvID = Trim(dsDropList.Tables(0).Rows(intCnt).Item(0))
            strDAID = Trim(dsDropList.Tables(0).Rows(intCnt).Item(1))

            dsDropList.Tables(0).Rows(intCnt).Item(0) = strRecvID & "|" & strDAID
            dsDropList.Tables(0).Rows(intCnt).Item(1) = strRecvID & " (" & strDAID & ")"
        Next intCnt

        drinsert = dsDropList.Tables(0).NewRow()
        drinsert(0) = "NoDoc|"
        drinsert(1) = "Select Canteen Receive ID or leave blank"


        dsDropList.Tables(0).Rows.InsertAt(drinsert, 0)
        lstDoc.DataSource = dsDropList.Tables(0)
        lstDoc.DataValueField = "CanteenRcvID"
        lstDoc.DataTextField = "CanteenRefNo"
        lstDoc.DataBind()

        If Not dsDropList Is Nothing Then
            dsDropList = Nothing
        End If
    End Sub

    Sub btnAdd_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)

        Dim strOpCdStckTxLine_ADD As String = "IN_CLSTRX_ITEMRETURNADVICELN_ADD"
        Dim strTxLnParam As String
        Dim TxID As String
        Dim StrTxParam As New StringBuilder()
        Dim ErrorChk As Integer = objCT.EnumInventoryErrorType.NoError
        Dim strAllowVehicle As String
        Dim PRID As String

        If CheckRequiredField() Then
            Exit Sub
        End If

        If lblStckTxID.Text = "" Then

            StrTxParam.Append(lblStckTxID.Text)
            StrTxParam.Append("|")
            StrTxParam.Append(objGlobal.EnumDocType.CanteenReturnAdvice)
            StrTxParam.Append("||||||")
            StrTxParam.Append(strLocation)
            StrTxParam.Append("|")
            StrTxParam.Append(strAccMonth)
            StrTxParam.Append("|")
            StrTxParam.Append(strAccYear)
            StrTxParam.Append("|0|")
            StrTxParam.Append(txtRemarks.Text)
            StrTxParam.Append("||")
            Try

                intErrNo = objCT.mtdUpdStockRtnAdvDetail(strOpCdStckTxDet_ADD, _
                                                            strOpCdStckTxDet_UPD, _
                                                            strOpCdStckTxLine_GET, _
                                                            strCompany, _
                                                            strLocation, _
                                                            strUserId, _
                                                            strAccMonth, _
                                                            strAccYear, _
                                                            StrTxParam.ToString, _
                                                            objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CanteenReturnAdvice), _
                                                            ErrorChk, _
                                                            TxID)

                lblStckTxID.Text = TxID
                If ErrorChk = objCT.EnumInventoryErrorType.OverFlow Then
                    lblError.Visible = True
                End If

            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ADDNEWSTOCKRETURNADVICE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_CRAList.aspx")
                End If
            End Try

        End If 

        If ErrorChk = objCT.EnumInventoryErrorType.noError And intErrNo = 0 Then
            Dim DocID As Array
            DocID = lstDoc.SelectedItem.Value.Trim.Split("|")
            Dim ItemCode As Array
            If lstDoc.SelectedItem.Value = "NoDoc|" Then
                strTxLnParam = lblStckTxID.Text & "|" & DocID(0) & "|" & txtQty.Text & "||" & DocID(1) & "|" & Request.Form("lstItem")
            Else
                ItemCode = lstItem.SelectedItem.Value.Trim.Split("|")
                strTxLnParam = lblStckTxID.Text & "|" & DocID(0) & "|" & txtQty.Text & "|" & ItemCode(1) & "|" & DocID(1) & "|" & ItemCode(0)
            End If

            strTxLnParam = strTxLnParam & "||||"

            Try
                intErrNo = objCT.mtdAddStockRtnAdvLn(strOpCdStckTxLine_ADD, _
                                                     strOpCdItem_Details_GET2, _
                                                     strOpCdItem_Details_UPD, _
                                                     strOpCdStckRecvLine_Det_GET, _
                                                     strOpCdStckRecvLine_Det_UPD, _
                                                     strCompany, _
                                                     strLocation, _
                                                     strUserId, _
                                                     strAccMonth, _
                                                     strAccYear, _
                                                     objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CanteenReturnAdviceLn), _
                                                     ErrorChk, _
                                                     strTxLnParam)
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ADDHOURLINE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_CRAList.aspx")
                End If
            End Try
            Select Case ErrorChk
                Case objCT.EnumInventoryErrorType.OverFlow
                    lblError.Visible = True
                Case objCT.EnumInventoryErrorType.InsufficientQty
                    lblStock.Visible = True
                Case objCT.EnumInventoryErrorType.PRNotfound
                    lblPR.Visible = True
            End Select

        End If


        StrTxParam.Remove(0, StrTxParam.Length)
        StrTxParam.Append(lblStckTxID.Text)
        StrTxParam.Append("|||||||||||||")

        If ErrorChk = objCT.EnumInventoryErrorType.noError And intErrNo = 0 Then
            Try
                intErrNo = objCT.mtdUpdStockRtnAdvDetail(strOpCdStckTxDet_ADD, _
                                                            strOpCdStckTxDet_UPD, _
                                                            strOpCdStckTxLine_GET, _
                                                            strCompany, _
                                                            strLocation, _
                                                            strUserId, _
                                                            strAccMonth, _
                                                            strAccYear, _
                                                            StrTxParam.ToString, _
                                                            objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CanteenReturnAdvice), _
                                                            ErrorChk, _
                                                            TxID)

                If ErrorChk = objCT.EnumInventoryErrorType.OverFlow Then
                    lblError.Visible = True
                End If

                lblStckTxID.Text = TxID
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ADDNEWSTOCKRETURNADVICE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_CRAList.aspx")
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
        Dim ErrorChk As Integer = objCT.EnumInventoryErrorType.NoError
        Dim strAllowVehicle As String

        If lblStckTxID.Text = "" Then
            StrTxParam.Append(lblStckTxID.Text)
            StrTxParam.Append("|")
            StrTxParam.Append(objGlobal.EnumDocType.CanteenReturnAdvice)
            StrTxParam.Append("||||||")
            StrTxParam.Append(strLocation)
            StrTxParam.Append("|")
            StrTxParam.Append(strAccMonth)
            StrTxParam.Append("|")
            StrTxParam.Append(strAccYear)
            StrTxParam.Append("|0|")
            StrTxParam.Append(txtRemarks.Text)
            StrTxParam.Append("||")
        Else

            StrTxParam.Append(lblStckTxID.Text)
            StrTxParam.Append("|")
            StrTxParam.Append(objGlobal.EnumDocType.CanteenReturnAdvice)
            StrTxParam.Append("||||||")
            StrTxParam.Append(strLocation)
            StrTxParam.Append("|")
            StrTxParam.Append(strAccMonth)
            StrTxParam.Append("|")
            StrTxParam.Append(strAccYear)
            StrTxParam.Append("||")
            StrTxParam.Append(txtRemarks.Text)
            StrTxParam.Append("||")
        End If

        Try
            intErrNo = objCT.mtdUpdStockRtnAdvDetail(strOpCdStckTxDet_ADD, _
                                                        strOpCdStckTxDet_UPD, _
                                                        strOpCdStckTxLine_GET, _
                                                        strCompany, _
                                                        strLocation, _
                                                        strUserId, _
                                                        strAccMonth, _
                                                        strAccYear, _
                                                        StrTxParam.ToString, _
                                                        objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CanteenReturnAdvice), _
                                                        ErrorChk, _
                                                        TxID)
            lblStckTxID.Text = TxID
            If ErrorChk = objCT.EnumInventoryErrorType.OverFlow Then
                lblError.Visible = True
            End If

        Catch Exp As System.Exception
            If intErrNo <> -5 Then
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ADDNEWSTOCKRETURNADVICE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_CRAList.aspx")
            End If
        End Try

        If Not StrTxParam Is Nothing Then
            StrTxParam = Nothing
        End If


        LoadStockTxDetails()
        DisplayFromDB()
        DisablePage()
        BindGrid()

    End Sub

    Sub btnConfirm_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim StrTxParam As String
        Dim TxID As String
        Dim ErrorChk As Integer = objCT.EnumInventoryErrorType.NoError
        Dim strItemCodeList As String
        Dim strOpCdStckTxLine_ADD As String = "IN_CLSTRX_ITEMRETURNADVICELN_ADD"
        Dim strOpCdTxLnList_GET As String = "IN_CLSTRX_ITEMRETURNADVICE_LINE_GET"
        Dim strOpCdTxLnList_UPD As String = "IN_CLSTRX_ITEMRETURNADVICELN_UPD"
        Dim strOpCdRecvLine_GET As String = "CT_CLSTRX_ITEMRETURNADVICELN_CANTEENRCVLN_GET"
        If Not LoadDataGrid.Tables(0).Rows.Count > 0 Then
            lblConfirmErr.Visible = True
            Exit Sub
        End If

        Try
            intErrNo = objCT.mtdConfirmReturnAdviceDoc(strOpCdTxLnList_GET, _
                                                       strOpCdStckTxLine_ADD, _
                                                       strOpCdTxLnList_UPD, _
                                                       strOpCdRecvLine_GET, _
                                                       strOpCdItem_Details_GET2, _
                                                       strOpCdItem_Details_UPD, _
                                                       strCompany, _
                                                       strLocation, _
                                                       strUserId, _
                                                       strAccMonth, _
                                                       strAccYear, _
                                                       objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CanteenReturnAdviceLn), _
                                                       lblStckTxID.Text, _
                                                       strItemCodeList, _
                                                       ErrorChk)
        Catch Exp As System.Exception
            If intErrNo <> -5 Then
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CONFIRMRETURNADVICE&errmesg=" & lblErrMessage.Text & "&redirect=IN/Trx/IN_Trx_StockRetAdv_List.aspx")
            End If
        End Try

        If ErrorChk <> objCT.EnumInventoryErrorType.NoError Then
            lblTxError.Visible = True
        End If

        If intErrNo = 0 And ErrorChk = objCT.EnumInventoryErrorType.NoError Then
            StrTxParam = lblStckTxID.Text & "|||||||||||||" & objCT.EnumCanRetAdvStatus.Confirmed
            Try
                intErrNo = objCT.mtdUpdStockRtnAdvDetail(strOpCdStckTxDet_ADD, _
                                                        strOpCdStckTxDet_UPD, _
                                                        strOpCdStckTxLine_GET, _
                                                        strCompany, _
                                                        strLocation, _
                                                        strUserId, _
                                                        strAccMonth, _
                                                        strAccYear, _
                                                        StrTxParam.ToString, _
                                                        objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CanteenReturnAdvice), _
                                                        ErrorChk, _
                                                        TxID)
                lblStckTxID.Text = TxID
                If ErrorChk = objCT.EnumInventoryErrorType.OverFlow Then
                    lblError.Visible = True
                End If

            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ADDNEWSTOCKRETURNADVICE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_CRAList.aspx")
                End If
            End Try
        End If
        LoadStockTxDetails()
        DisplayFromDB()
        DisablePage()
        BindGrid()
        If strItemCodeList <> "" Then
            If InStr(strItemCodeList, ",") = 0 Then
                Response.write("<Script Language=""JavaScript"">window.alert(""As a result of confirming the document,\nthe average cost for this item had been set to zero.\nPlease carry out necessary stock adjustment.\n\n" & strItemCodeList & """);</Script>")
            Else
                Response.write("<Script Language=""JavaScript"">window.alert(""As a result of confirming the document,\nthe average cost for these items had been set to zero.\nPlease carry out necessary stock adjustment.\n\n" & strItemCodeList & """);</Script>")
            End If
        End If
    End Sub








    Sub btnDelete_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim StrTxParam As String
        Dim TxID As String
        Dim ErrorChk As Integer = objCT.EnumInventoryErrorType.NoError

        If lblStatusHid.Text = CStr(objCT.EnumCanRetAdvStatus.Deleted) Then
            Try
                intErrNo = objCT.mtdReturnAdvInvItemLevel(strOpCdStckTxLine_GET, _
                                                            strOpCdStckRecvLine_Det_GET, _
                                                            strOpCdStckRecvLine_Det_UPD, _
                                                            strCompany, _
                                                            strLocation, _
                                                            strUserId, _
                                                            strAccMonth, _
                                                            strAccYear, _
                                                            strOpCdItem_Details_UPD, _
                                                            strOpCdItem_Details_GET2, _
                                                            lblStckTxID.Text, _
                                                            objCT.EnumTransactionAction.Undelete, _
                                                            ErrorChk)
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=UNDELETESTOCKRETURNADVICE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_CRAList.aspx")
                End If
            End Try
            StrTxParam = lblStckTxID.Text & "|||||||||||||" & objCT.EnumCanRetAdvStatus.Active

        ElseIf lblStatusHid.Text = CStr(objCT.EnumCanRetAdvStatus.Active) Then
            Try
                intErrNo = objCT.mtdReturnAdvInvItemLevel(strOpCdStckTxLine_GET, _
                                                            strOpCdStckRecvLine_Det_GET, _
                                                            strOpCdStckRecvLine_Det_UPD, _
                                                            strCompany, _
                                                            strLocation, _
                                                            strUserId, _
                                                            strAccMonth, _
                                                            strAccYear, _
                                                            strOpCdItem_Details_UPD, _
                                                            strOpCdItem_Details_GET2, _
                                                            lblStckTxID.Text, _
                                                            objCT.EnumTransactionAction.Delete, _
                                                            ErrorChk)
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=DELETENEWSTOCKRETURNADVICE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_CRAList.aspx")
                End If
            End Try
            StrTxParam = lblStckTxID.Text & "|||||||||||||" & objCT.EnumCanRetAdvStatus.Deleted & "|"
        End If
        If intErrNo = 0 And Not ErrorChk = objCT.EnumInventoryErrorType.InsufficientQty Then
            Try
                intErrNo = objCT.mtdUpdStockRtnAdvDetail(strOpCdStckTxDet_ADD, _
                                                           strOpCdStckTxDet_UPD, _
                                                           strOpCdStckTxLine_GET, _
                                                           strCompany, _
                                                           strLocation, _
                                                           strUserId, _
                                                           strAccMonth, _
                                                           strAccYear, _
                                                           StrTxParam, _
                                                           objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CanteenReturnAdvice), _
                                                           ErrorChk, _
                                                           TxID)
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ADDNEWSTOCKRETURNADVICE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_CRAList.aspx")
                End If
            End Try
        End If

        If ErrorChk = objCT.EnumInventoryErrorType.InsufficientQty Then
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
        Dim strStockTxId As String

        strStockTxId = Trim(lblStckTxID.Text)

        strUpdString = "where ItemRetAdvID = '" & strStockTxId & "'"
        strStatus = Trim(Status.Text)
        intStatus = CInt(Trim(lblStatusHid.Text))
        strPrintDate = Trim(lblPrintDate.Text)
        strTable = "IN_ITEMRETADV"
        strSortLine = "order by ItemRetAdvLnID"


        If intStatus = objCT.EnumCanRetAdvStatus.Confirmed Then
            If strPrintDate = "" Then
                Try
                    intErrNo = objAdmin.mtdUpdPrintDate(strOpCodePrint, _
                                                        strUpdString, _
                                                        strTable, _
                                                        strCompany, _
                                                        strLocation, _
                                                        strUserId)
                Catch Exp As System.Exception
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CT_TXT_CRADETAILS_PRINTDATE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_CRAList.aspx")
                End Try
                LoadStockTxDetails()
                DisplayFromDB()
                DisablePage()
            Else
                strStatus = strStatus & " (re-printed)"
            End If
        End If

        Response.Write("<Script Language=""JavaScript"">window.open(""../../IN/reports/IN_Rpt_StockRetAdvDet.aspx?strStockTxId=" & strStockTxId & _
                       "&strPrintDate=" & strPrintDate & _
                       "&strStatus=" & strStatus & _
                       "&strSortLine=" & strSortLine & _
                       "&strDocTitle=" & lblDocTitle.Text & _
                       """, null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")
    End Sub

    Sub btnBack_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CT_trx_CRAList.aspx")
    End Sub

    Sub btnNew_Click(ByVal Sender As Object, ByVal e As ImageClickEventArgs)
        Response.Redirect("CT_Trx_CRADetails.aspx")
    End Sub

End Class
