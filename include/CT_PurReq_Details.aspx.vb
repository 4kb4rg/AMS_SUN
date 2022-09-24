
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
Imports System.Web.UI.HtmlControls
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic

Public Class CT_PurReqDetails : Inherits Page

    Protected WithEvents dgPRLn As DataGrid
    Protected WithEvents PRLnTable As HtmlTable
    Protected WithEvents tblLine As HtmlTable
    Protected WithEvents hidPQID As HtmlInputHidden
    Protected WithEvents dsForDropDown As DataSet
    Protected WithEvents lstCanteen As DropDownList

    Protected WithEvents lblErrMesage As Label
    Protected WithEvents SortExpression As Label

    Protected WithEvents QtyReq As TextBox
    Protected WithEvents UnitCost As TextBox
    Protected WithEvents txtRemarks As TextBox

    Protected WithEvents Status As Label
    Protected WithEvents CreateDate As Label
    Protected WithEvents UpdateDate As Label
    Protected WithEvents UpdateBy As Label
    Protected WithEvents lblPurReqID As Label
    Protected WithEvents lblTotAmtFig As Label
    Protected WithEvents lblTotAmtFigDisplay As Label
    Protected WithEvents lblQtyReq As Label
    Protected WithEvents lblQtyRcv As Label
    Protected WithEvents lblQtyOutstanding As Label
    Protected WithEvents lblUnitCost As Label
    Protected WithEvents lblAmount As Label
    Protected WithEvents hidStatus As Label
    Protected WithEvents lblStatus As Label
    Protected WithEvents lblPrintDate As Label

    Protected WithEvents FindCT As HtmlInputButton
    Protected WithEvents Undelete As ImageButton
    Protected WithEvents PRDelete As ImageButton
    Protected WithEvents Save As ImageButton
    Protected WithEvents Confirm As ImageButton
    Protected WithEvents Cancel As ImageButton
    Protected WithEvents Add As ImageButton
    Protected WithEvents Print As ImageButton

    Protected objCT As New agri.CT.clsTrx()
    Protected objCTSetup As New agri.CT.clsSetup()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objAdmin As New agri.Admin.clsShare()

    Dim strOppCd_Item_GET_NOPRID As String = "IN_CLSTRX_PURREQ_ITEMLIST_WITHOUTPRID_GET"
    Dim strOppCd_Item_GET As String = "IN_CLSTRX_PURREQ_ITEMLIST_WITHPRID_GET"
    Dim strOppCd_GET_PRList As String = "IN_CLSTRX_PURREQ_LIST_GET"
    Dim strOppCd_GET_PRLnList As String = "IN_CLSTRX_PURREQLN_LIST_GET"
    Dim strOppCd_UpdPQ As String = "IN_CLSTRX_PURREQ_LIST_UPD"

    Dim objDataSet As New DataSet()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim intCTAR As Integer
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim intCnt As Integer
    Dim intErrNo As Integer
    Dim strPRID As String
    Dim strPRQType As String
    Dim dsStkDCItem As DataSet
    Dim pv_strItemCode As String
    Dim intPRLnCount As Integer

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        Dim prqtype As String

        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        intCTAR = Session("SS_CTAR")
        strAccMonth = Session("SS_INACCMONTH")
        strAccYear = Session("SS_INACCYEAR")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCTAccessRights.CTPurchaseRequest), intCTAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            If SortExpression.Text = "" Then
                SortExpression.Text = "ItemCode"
            End If

            strPRID = IIf(Request.QueryString("prid") = "", Request.Form("hidPRID"), Request.QueryString("prid"))
            strPRQType = Request.QueryString("prqtype")

            If Not IsPostBack Then
                If strPRQType = "canteen" Then
                    If strPRID <> "" Then
                        hidPQID.Value = strPRID
                    End If
                    BindStkDCList(strPRID)

                    If strPRID <> "" Then
                        onProcess_Load(strPRID)
                    End If
                End If

                If strPRID <> "" Then
                    onProcess_Load(strPRID)
                    BindStkDCList(strPRID)
                Else
                    Print.Visible = False
                    Undelete.Visible = False
                    PRDelete.Visible = False
                    Confirm.Visible = False
                    PRLnTable.Visible = False
                End If
            End If
        End If
    End Sub

    Sub onProcess_Load(ByVal pv_strPRID As String)
        onLoad_DisplayPR(pv_strPRID)
        onLoad_DisplayPRLn(pv_strPRID)
        DisableItemTable()
    End Sub

    Sub DisableItemTable()
        If Status.Text = objCT.mtdGetPurReqStatus(objCT.EnumPurReqStatus.Confirmed) Then
            tblLine.Visible = False
            Save.Visible = False
            Confirm.Visible = False
            Undelete.Visible = False
            txtRemarks.Enabled = False
            PRDelete.Visible = False
            Cancel.Visible = True
        ElseIf Status.Text = objCT.mtdGetPurReqStatus(objCT.EnumPurReqStatus.Active) Then
            tblLine.Visible = True
            Save.Visible = True
            Confirm.Visible = True
            PRDelete.Visible = True
            PRDelete.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
            Undelete.Visible = False
            txtRemarks.Enabled = True
            Cancel.Visible = False
        ElseIf Status.Text = objCT.mtdGetPurReqStatus(objCT.EnumPurReqStatus.Deleted) Then
            tblLine.Visible = False
            Save.Visible = False
            Confirm.Visible = False
            PRDelete.Visible = False
            Undelete.Visible = True
            txtRemarks.Enabled = False
            Cancel.Visible = False
        ElseIf Status.Text = objCT.mtdGetPurReqStatus(objCT.EnumPurReqStatus.Cancelled) Then
            tblLine.Visible = False
            Save.Visible = False
            Confirm.Visible = False
            PRDelete.Visible = False
            Undelete.Visible = False
            txtRemarks.Enabled = False
            Cancel.Visible = False
        End If
    End Sub

    Sub onLoad_DisplayPR(ByVal pv_strPRID As String)
        Dim intErrNo As Integer
        Dim objPRDs As New Data.DataSet()
        Dim strParam As String
        Dim TempStatus As String
        Dim prqtype As String
        Dim TotAmtFigTemp As Decimal

        PRLnTable.Visible = True
        strParam = "And PR.PRID = '" & pv_strPRID & "' AND PR.AccMonth = '" & strAccMonth & "' AND PR.AccYear = '" & strAccYear & "' AND Pr.LocCode = '" & strLocation & "'|" & " "
        Try
            intErrNo = objCT.mtdGetPurchaseRequest(strOppCd_GET_PRList, _
                                                   strParam, _
                                                   objCT.EnumPurReqDocType.StockPR, _
                                                   strAccMonth, _
                                                   strAccYear, _
                                                   strLocation, _
                                                   objPRDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CT_TRX_PURREQ_LIST_GET_DISPLAY&errmesg=" & lblErrMesage.Text & "&redirect=CT/trx/CT_PurReq.aspx")
        End Try

        lblPurReqID.Text = pv_strPRID
        Status.Text = objCT.mtdGetPurReqStatus(Trim(objPRDs.Tables(0).Rows(0).Item("Status")))
        lblStatus.Text = objPRDs.Tables(0).Rows(0).Item("Status")
        lblPrintDate.Text = objGlobal.GetLongDate(Trim(objPRDs.Tables(0).Rows(0).Item("PrintDate")))
        CreateDate.Text = objGlobal.GetLongDate(Trim(objPRDs.Tables(0).Rows(0).Item("CreateDate")))
        UpdateDate.Text = objGlobal.GetLongDate(Trim(objPRDs.Tables(0).Rows(0).Item("UpdateDate")))
        UpdateBy.Text = Trim(objPRDs.Tables(0).Rows(0).Item("UserName"))
        TotAmtFigTemp = Trim(objPRDs.Tables(0).Rows(0).Item("TotalAmount"))
        lblTotAmtFig.Text = Trim(FormatNumber(TotAmtFigTemp, 2))
        lblTotAmtFigDisplay.Text = ObjGlobal.GetIDDecimalSeparator(Round(TotAmtFigTemp, 0))
        txtRemarks.Text = Trim(objPRDs.Tables(0).Rows(0).Item("Remark"))

    End Sub


    Sub onLoad_DisplayPRLn(ByVal pv_strPRID As String)
        Dim PageNo As Integer
        Dim UpdButton As LinkButton
        Dim lblStatus As Label
        Dim lblQtyReqText As Label
        Dim lblQtyRcvText As Label
        Dim lblQtyOutText As Label
        Dim strhidStatus As String
        Dim strQtyReq As String
        Dim strQtyRcv As String
        Dim strQtyOut As String

        dgPRLn.DataSource = LoadPRData(pv_strPRID)
        dgPRLn.DataBind()

        If objDataSet.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To intPRLnCount - 1
                lblStatus = dgPRLn.Items.Item(intCnt).FindControl("hidStatus")
                strhidStatus = lblStatus.Text
                lblQtyReqText = dgPRLn.Items.Item(intCnt).FindControl("lblQtyReq")
                strQtyReq = lblQtyReqText.Text
                lblQtyRcvText = dgPRLn.Items.Item(intCnt).FindControl("lblQtyRcv")
                strQtyRcv = lblQtyRcvText.Text
                lblQtyOutText = dgPRLn.Items.Item(intCnt).FindControl("lblQtyOutstanding")
                strQtyOut = lblQtyOutText.Text

                If Status.Text = objCT.mtdGetPurReqStatus(objCT.EnumPurReqStatus.Active) And strQtyRcv = 0 Then
                    UpdButton = dgPRLn.Items.Item(intCnt).FindControl("Cancel")
                    UpdButton.Visible = False
                    UpdButton = dgPRLn.Items.Item(intCnt).FindControl("Delete")
                    UpdButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                ElseIf Status.Text = objCT.mtdGetPurReqStatus(objCT.EnumPurReqStatus.Active) And strQtyRcv <> 0 Then
                    UpdButton = dgPRLn.Items.Item(intCnt).FindControl("Cancel")
                    UpdButton.Visible = False
                    UpdButton = dgPRLn.Items.Item(intCnt).FindControl("Delete")
                    UpdButton.Visible = False
                ElseIf Status.Text = objCT.mtdGetPurReqStatus(objCT.EnumPurReqStatus.Confirmed) And strhidStatus = objCT.mtdGetPurReqStatus(objCT.EnumPurReqStatus.Active) And strQtyReq <> strQtyRcv And strQtyOut <> 0 Then
                    UpdButton = dgPRLn.Items.Item(intCnt).FindControl("Delete")
                    UpdButton.Visible = False
                ElseIf Status.Text = objCT.mtdGetPurReqStatus(objCT.EnumPurReqStatus.Confirmed) And strhidStatus = objCT.mtdGetPurReqStatus(objCT.EnumPurReqStatus.Active) And strQtyReq = strQtyRcv And strQtyOut = 0 Then
                    UpdButton = dgPRLn.Items.Item(intCnt).FindControl("Cancel")
                    UpdButton.Visible = False
                    UpdButton = dgPRLn.Items.Item(intCnt).FindControl("Delete")
                    UpdButton.Visible = False
                ElseIf Status.Text = objCT.mtdGetPurReqStatus(objCT.EnumPurReqStatus.Confirmed) And strhidStatus = objCT.mtdGetPurReqStatus(objCT.EnumPurReqStatus.Active) And strQtyReq <> strQtyRcv And strQtyOut = 0 Then
                    UpdButton = dgPRLn.Items.Item(intCnt).FindControl("Delete")
                    UpdButton.Visible = False
                ElseIf Status.Text = objCT.mtdGetPurReqStatus(objCT.EnumPurReqStatus.Confirmed) And strhidStatus = objCT.mtdGetPurReqLnStatus(objCT.EnumPurReqLnStatus.Cancel) Then
                    UpdButton = dgPRLn.Items.Item(intCnt).FindControl("Cancel")
                    UpdButton.Visible = False
                    UpdButton = dgPRLn.Items.Item(intCnt).FindControl("Delete")
                    UpdButton.Visible = False
                ElseIf Status.Text = objCT.mtdGetPurReqStatus(objCT.EnumPurReqStatus.Deleted) Then
                    UpdButton = dgPRLn.Items.Item(intCnt).FindControl("Cancel")
                    UpdButton.Visible = False
                    UpdButton = dgPRLn.Items.Item(intCnt).FindControl("Delete")
                    UpdButton.Visible = False
                ElseIf Status.Text = objCT.mtdGetPurReqStatus(objCT.EnumPurReqStatus.Cancelled) Then
                    UpdButton = dgPRLn.Items.Item(intCnt).FindControl("Cancel")
                    UpdButton.Visible = False
                    UpdButton = dgPRLn.Items.Item(intCnt).FindControl("Delete")
                    UpdButton.Visible = False
                End If
            Next intCnt
        Else
            PRLnTable.Visible = False
            Confirm.Visible = False
        End If
    End Sub

    Protected Function LoadPRData(ByVal pv_strPRID As String) As DataSet

        Dim strParam As String = pv_strPRID & "|" & "PRln.PRID"
        Dim edittext As TextBox

        Try
            intErrNo = objCT.mtdGetPRLnList(strOppCd_GET_PRLnList, _
                                            strCompany, _
                                            strLocation, _
                                            strUserId, _
                                            strAccMonth, _
                                            strAccYear, _
                                            strParam, _
                                            objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CT_TRX_PURREQLN_LIST_GET_LOADPRDATA&errmesg=" & lblErrMesage.Text & "&redirect=CT/trx/CT_PurReq.aspx")
        End Try

        For intCnt = 0 To objDataSet.Tables(0).Rows.Count - 1
            objDataSet.Tables(0).Rows(intCnt).Item(0) = Trim(objDataSet.Tables(0).Rows(intCnt).Item(0))
            objDataSet.Tables(0).Rows(intCnt).Item(1) = Trim(objDataSet.Tables(0).Rows(intCnt).Item(1))
            objDataSet.Tables(0).Rows(intCnt).Item(2) = Trim(objDataSet.Tables(0).Rows(intCnt).Item(2))
            objDataSet.Tables(0).Rows(intCnt).Item(3) = Trim(objDataSet.Tables(0).Rows(intCnt).Item(3))
            objDataSet.Tables(0).Rows(intCnt).Item(4) = Trim(objDataSet.Tables(0).Rows(intCnt).Item(4))
            objDataSet.Tables(0).Rows(intCnt).Item(5) = Trim(objDataSet.Tables(0).Rows(intCnt).Item(5))
            objDataSet.Tables(0).Rows(intCnt).Item(6) = Trim(objDataSet.Tables(0).Rows(intCnt).Item(6))
            objDataSet.Tables(0).Rows(intCnt).Item(7) = Trim(objDataSet.Tables(0).Rows(intCnt).Item(7))
        Next intCnt

        intPRLnCount = objDataSet.Tables(0).Rows.Count

        Return objDataSet
    End Function


    Sub BindStkDCList(ByVal pv_strPRID As String)
        Dim strItemType As String
        Dim strParam As String

        strItemType = "'" & objCTSetup.EnumInventoryItemType.CanteenItem & "','" & objCTSetup.EnumInventoryItemType.MiscItem & "'"
        strParam = pv_strPRID & "|" & strItemType & "|" & objCTSetup.EnumStockItemStatus.Active

        Try
            intErrNo = objCT.mtdGetItemList(strOppCd_Item_GET, _
                                            strOppCd_Item_GET_NOPRID, _
                                            strCompany, _
                                            strLocation, _
                                            strUserId, _
                                            strAccMonth, _
                                            strAccYear, _
                                            strParam, _
                                            dsForDropDown)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CT_TRX_PR_GET_ITEM_LIST_BINDSTKDCLIST&errmesg=" & lblErrMesage.Text & "&redirect=CT/trx/CT_PurReq.aspx")
        End Try

        For intCnt = 0 To dsForDropDown.Tables(0).Rows.Count - 1
            dsForDropDown.Tables(0).Rows(intCnt).Item(0) = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(0))
            dsForDropDown.Tables(0).Rows(intCnt).Item(1) = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(0)) & " ( " & _
                                                           Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(1)) & " ), " & _
                                                           FormatNumber(Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(2)), 2) & ", " & _
                                                           Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(3))
        Next intCnt

        Dim drinsert As DataRow
        drinsert = dsForDropDown.Tables(0).NewRow()
        drinsert(0) = " "
        drinsert(1) = "Select Item Code"
        dsForDropDown.Tables(0).Rows.InsertAt(drinsert, 0)

        lstCanteen.DataSource = dsForDropDown.Tables(0)
        lstCanteen.DataValueField = "ItemCode"
        lstCanteen.DataTextField = "Description"
        lstCanteen.DataBind()
    End Sub

    Sub btnAdd_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOppCd_NewPQ As String = "IN_CLSTRX_PURREQ_ADD"
        Dim strOppCd_NewPQLn As String = "IN_CLSTRX_PURREQLN_ADD"
        Dim strOppCd_Item As String = "IN_CLSSETUP_STOCKITEM_DETAILS_GET"

        Dim strItemCode As String = Trim(Request.Form("lstCanteen"))
        Dim strQtyReq As String = Trim(QtyReq.Text)
        Dim strUnitCost As String = Trim(UnitCost.Text)
        Dim strRemarks As String
        Dim strPrintDate As String
        Dim strStatus As String = objCT.EnumPurReqStatus.Active
        Dim objPRID As Object
        Dim strParam As String
        Dim strPRID As String

        If lblPurReqID.Text = "" Then
            strParam = "|" & strItemCode & "|" & strQtyReq & "|" & strUnitCost & "|" & Trim(strRemarks) & "|" & strStatus & "|" & Trim(strPrintDate) & "|" & "PRLn." & SortExpression.Text
            Try
                intErrNo = objCT.mtdNewPurchaseRequestLn(strOppCd_NewPQ, _
                                                         strOppCd_NewPQLn, _
                                                         strOppCd_Item, _
                                                         strOppCd_GET_PRLnList, _
                                                         strOppCd_UpdPQ, _
                                                         strParam, _
                                                         strCompany, _
                                                         strLocation, _
                                                         strUserId, _
                                                         strAccMonth, _
                                                         strAccYear, _
                                                         objPRID, _
                                                         objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.PurchaseRequest))
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=NEW_CT_PURREQLN_WITHOUT_PRID&errmesg=" & lblErrMesage.Text & "&redirect=CT/trx/CT_PurReq.aspx")
            End Try

            hidPQID.Value = objPRID
            onProcess_Load(hidPQID.Value)

        Else
            strPRID = Trim(lblPurReqID.Text)
            strRemarks = Trim(txtRemarks.Text)
            strParam = strPRID & "|" & strItemCode & "|" & strQtyReq & "|" & strUnitCost & "|" & strRemarks & "|" & strStatus & "|" & Trim(strPrintDate) & "|" & "PRLn." & SortExpression.Text
            Try
                intErrNo = objCT.mtdNewPurchaseRequestLn(strOppCd_NewPQ, _
                                                         strOppCd_NewPQLn, _
                                                         strOppCd_Item, _
                                                         strOppCd_GET_PRLnList, _
                                                         strOppCd_UpdPQ, _
                                                         strParam, _
                                                         strCompany, _
                                                         strLocation, _
                                                         strUserId, _
                                                         strAccMonth, _
                                                         strAccYear, _
                                                         objPRID, _
                                                         objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.PurchaseRequest))
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=NEW_CT_PURREQLN_WITH_PRID&errmesg=" & lblErrMesage.Text & "&redirect=CT/trx/CT_PurReq.aspx")
            End Try

            onProcess_Load(strPRID)
        End If
        BindStkDCList(objPRID)
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strOppCd_PurReqLn_DEL As String = "IN_CLSTRX_PURREQLN_DEL"

        Dim objPRID As Object
        Dim strPRID As String = Trim(lblPurReqID.Text)
        Dim strItemCode As String
        Dim DelText As Label
        Dim strRemarks As String = Trim(txtRemarks.Text)
        Dim strPRStatus As String = objCT.EnumPurReqStatus.Active
        Dim strParam As String

        dgPRLn.EditItemIndex = CInt(E.Item.ItemIndex)

        DelText = dgPRLn.Items.Item(CInt(E.Item.ItemIndex)).FindControl("ItemCode")
        strItemCode = DelText.Text

        strParam = strPRID & "|" & strItemCode & "|" & strPRStatus & "|" & strRemarks & "|" & "PRLn.ItemCode"
        Try
            intErrNo = objCT.mtdDelPurchaseRequestLn(strOppCd_PurReqLn_DEL, _
                                                     strOppCd_GET_PRLnList, _
                                                     strOppCd_UpdPQ, _
                                                     strParam, _
                                                     strCompany, _
                                                     strLocation, _
                                                     strUserId, _
                                                     objPRID, _
                                                     objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.PurchaseRequest))
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=DEL_CT_PURREQLN&errmesg=" & lblErrMesage.Text & "&redirect=CT/trx/CT_PurReq.aspx")
        End Try

        onProcess_Load(strPRID)
        BindStkDCList(objPRID)

    End Sub

    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strOppCd_PurReqLn_UPD As String = "IN_CLSTRX_PURREQLN_LIST_UPD"
        Dim objPRID As Object
        Dim strPRID As String = Trim(lblPurReqID.Text)

        Dim CancelText As Label
        Dim Updbutton As LinkButton

        Dim strRemarks As String = Trim(txtRemarks.Text)
        Dim strItemCode As String
        Dim strQtyReq As String
        Dim strQtyRcv As String
        Dim strQtyOutstanding As String
        Dim strCost As String
        Dim strStatus As String
        Dim strParam As String

        dgPRLn.EditItemIndex = CInt(E.Item.ItemIndex)
        CancelText = dgPRLn.Items.Item(CInt(E.Item.ItemIndex)).FindControl("ItemCode")
        strItemCode = CancelText.Text
        CancelText = dgPRLn.Items.Item(CInt(E.Item.ItemIndex)).FindControl("hidStatus")
        strStatus = CancelText.Text
        CancelText = dgPRLn.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lblQtyReq")
        strQtyReq = CancelText.Text
        CancelText = dgPRLn.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lblQtyRcv")
        strQtyRcv = CancelText.Text
        CancelText = dgPRLn.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lblQtyOutstanding")
        strQtyOutstanding = CancelText.Text
        CancelText = dgPRLn.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lblUnitCost")
        strCost = CancelText.Text

        strParam = strPRID & "|" & strItemCode & "|" & strQtyReq & "|" & strQtyRcv & "|" & strQtyOutstanding & "|" & strCost & "|" & strStatus & "|" & strRemarks & "|" & "PRLn.ItemCode"
        Try
            intErrNo = objCT.mtdCancelPurchaseRequestLn(strOppCd_PurReqLn_UPD, _
                                                        strOppCd_GET_PRLnList, _
                                                        strOppCd_UpdPQ, _
                                                        strParam, _
                                                        strCompany, _
                                                        strLocation, _
                                                        strUserId, _
                                                        objPRID, _
                                                        objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.PurchaseRequest))
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CANCEL_CT_PURREQLN&errmesg=" & lblErrMesage.Text & "&redirect=CT/trx/CT_PurReq.aspx")
        End Try
        onProcess_Load(strPRID)
    End Sub

    Sub btnSave_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOppCd_NewPQ As String = "IN_CLSTRX_PURREQ_ADD"
        Dim strRemarks As String = Trim(txtRemarks.Text)
        Dim strPrintDate As String
        Dim strPRStatus As String = objCT.EnumPurReqStatus.Active
        Dim objPRID As Object
        Dim strPRID As String = Trim(lblPurReqID.Text)
        Dim strParam As String

        If lblPurReqID.Text = "" Then
            strParam = "|" & strRemarks & "|" & lblTotAmtFig.Text & "|" & strPRStatus & "|" & Trim(strPrintDate)
            Try
                intErrNo = objCT.mtdNewPurchaseRequest(strOppCd_NewPQ, _
                                                       strOppCd_UpdPQ, _
                                                       strParam, _
                                                       strCompany, _
                                                       strLocation, _
                                                       strUserId, _
                                                       strAccMonth, _
                                                       strAccYear, _
                                                       objPRID, _
                                                       objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.PurchaseRequest))
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=NEW_CT_PURREQ_WITHOUT_PRID&errmesg=" & lblErrMesage.Text & "&redirect=CT/trx/CT_PurReq.aspx")
            End Try

            hidPQID.Value = objPRID
            onProcess_Load(hidPQID.Value)
        Else
            strParam = strPRID & "|" & strRemarks & "|" & lblTotAmtFig.Text & "|" & strPRStatus & "|" & Trim(strPrintDate)
            Try
                intErrNo = objCT.mtdNewPurchaseRequest(strOppCd_NewPQ, _
                                                       strOppCd_UpdPQ, _
                                                       strParam, _
                                                       strCompany, _
                                                       strLocation, _
                                                       strUserId, _
                                                       strAccMonth, _
                                                       strAccYear, _
                                                       objPRID, _
                                                       objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.PurchaseRequest))
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=NEW_PURREQLN_WITH_PRID&errmesg=" & lblErrMesage.Text & "&redirect=CT/trx/CT_PurReq.aspx")
            End Try

            onProcess_Load(strPRID)
        End If
    End Sub


    Sub btnConfirm_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strPRID As String = Trim(lblPurReqID.Text)
        Dim strStatus As Integer = objCT.EnumPurReqStatus.Confirmed
        Dim objPRDs As DataSet
        Dim objPRID As Object
        Dim strTotalAmt As String = Trim(lblTotAmtFig.Text)
        Dim lblRemarks As Label
        Dim strRemarks As String
        Dim strParam As String
        Dim strParamTemp As String

        strParam = "And PR.PRID = '" & strPRID & "' AND PR.AccMonth = '" & strAccMonth & "' AND PR.AccYear = '" & strAccYear & "' AND PR.LocCode = '" & strLocation & "'|" & " "
        Try
            intErrNo = objCT.mtdGetPurchaseRequest(strOppCd_GET_PRList, _
                                                   strParam, _
                                                   objCT.EnumPurReqDocType.StockPR, _
                                                   strAccMonth, _
                                                   strAccYear, _
                                                   strLocation, _
                                                   objPRDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PURREQ_CT_LIST_GET_CONFIRM&errmesg=" & lblErrMesage.Text & "&redirect=CT/trx/CT_PurReq.aspx")
        End Try

        strRemarks = Trim(objPRDs.Tables(0).Rows(0).Item("Remark"))
        strParamTemp = strPRID & "|" & strRemarks & "|" & strStatus & "|" & strTotalAmt
        Try
            intErrNo = objCT.mtdUpdPurchaseRequest(strOppCd_UpdPQ, _
                                                   strParamTemp, _
                                                   strCompany, _
                                                   strLocation, _
                                                   strUserId, _
                                                   objPRID, _
                                                   objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.PurchaseRequest))

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PURREQ_CT_LIST_UPD_CONFIRM&errmesg=" & lblErrMesage.Text & "&redirect=CT/trx/CT_PurReq.aspx")
        End Try
        onProcess_Load(strPRID)
    End Sub

    Sub btnPRDelete_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim objPRDs As DataSet
        Dim objPRLnDs As DataSet
        Dim objPRID As Object
        Dim strPRID As String = Trim(lblPurReqID.Text)
        Dim strPRStatus As Integer = objCT.EnumPurReqStatus.Deleted
        Dim strRemarks As String
        Dim strTotalAmt As String = Trim(lblTotAmtFig.Text)
        Dim lblRemarks As Label
        Dim strParamTemp As String
        Dim strParam As String

        strParam = "And PR.PRID = '" & strPRID & "' AND PR.AccMonth = '" & strAccMonth & "' AND PR.AccYear = '" & strAccYear & "' AND PR.LocCode = '" & strLocation & "'|" & " "
        Try
            intErrNo = objCT.mtdGetPurchaseRequest(strOppCd_GET_PRList, _
                                                   strParam, _
                                                   objCT.EnumPurReqDocType.StockPR, _
                                                   strAccMonth, _
                                                   strAccYear, _
                                                   strLocation, _
                                                   objPRDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PURREQ_CT_LIST_GET_DELETE&errmesg=" & lblErrMesage.Text & "&redirect=CT/trx/CT_PurReq.aspx")
        End Try

        strRemarks = Trim(objPRDs.Tables(0).Rows(0).Item("Remark"))
        strParamTemp = strPRID & "|" & "PRln.PRID"
        Try
            intErrNo = objCT.mtdGetPRLnList(strOppCd_GET_PRLnList, _
                                            strCompany, _
                                            strLocation, _
                                            strUserId, _
                                            strAccMonth, _
                                            strAccYear, _
                                            strParamTemp, _
                                            objPRLnDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PURREQLN_LIST_GET_DELETE&errmesg=" & lblErrMesage.Text & "&redirect=CT/trx/CT_PurReq.aspx")
        End Try

        If objPRLnDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objPRLnDs.Tables(0).Rows.Count - 1
                strParam = strPRID & "|" & strRemarks & "|" & strPRStatus & "|" & strTotalAmt
                Try
                    intErrNo = objCT.mtdUpdPurchaseRequest(strOppCd_UpdPQ, _
                                                           strParam, _
                                                           strCompany, _
                                                           strLocation, _
                                                           strUserId, _
                                                           objPRID, _
                                                           objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.PurchaseRequest))
                Catch Exp As System.Exception
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PURREQ_LIST_UPD_DELETE_MORERECS&errmesg=" & lblErrMesage.Text & "&redirect=CT/trx/CT_PurReq.aspx")
                End Try
            Next intCnt
        Else
            strParam = strPRID & "|" & strRemarks & "|" & strPRStatus & "|" & strTotalAmt
            Try
                intErrNo = objCT.mtdUpdPurchaseRequest(strOppCd_UpdPQ, _
                                                       strParam, _
                                                       strCompany, _
                                                       strLocation, _
                                                       strUserId, _
                                                       objPRID, _
                                                       objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.PurchaseRequest))
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PURREQ_CT_LIST_UPD_DELETE_1RECS&errmesg=" & lblErrMesage.Text & "&redirect=CT/trx/CT_PurReq.aspx")
            End Try
        End If
        onProcess_Load(strPRID)
    End Sub

    Sub btnPRUnDelete_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim objPRDs As DataSet
        Dim strPRID As String = Trim(lblPurReqID.Text)
        Dim strPRStatus As Integer = objCT.EnumPurReqStatus.Active
        Dim objPRID As Object
        Dim strTotalAmt As String = Trim(lblTotAmtFig.Text)
        Dim strRemarks As String = Trim(txtRemarks.Text)
        Dim strParam As String


        strParam = strPRID & "|" & strRemarks & "|" & strPRStatus & "|" & strTotalAmt
        Try
            intErrNo = objCT.mtdUpdPurchaseRequest(strOppCd_UpdPQ, _
                                                   strParam, _
                                                   strCompany, _
                                                   strLocation, _
                                                   strUserId, _
                                                   objPRID, _
                                                   objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.PurchaseRequest))
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PURREQ_CT_LIST_UPD_UNDELETE&errmesg=" & lblErrMesage.Text & "&redirect=CT/trx/CT_PurReq.aspx")
        End Try
        onProcess_Load(strPRID)
    End Sub

    Sub btnCancel_click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim objPRDs As DataSet
        Dim strPRID As String = Trim(lblPurReqID.Text)
        Dim strPRStatus As Integer = objCT.EnumPurReqStatus.Cancelled
        Dim objPRID As Object
        Dim strTotalAmt As String = Trim(lblTotAmtFig.Text)
        Dim strRemarks As String = Trim(txtRemarks.Text)
        Dim strParam As String

        strParam = strPRID & "|" & strRemarks & "|" & strPRStatus & "|" & strTotalAmt
        Try
            intErrNo = objCT.mtdUpdPurchaseRequest(strOppCd_UpdPQ, _
                                                   strParam, _
                                                   strCompany, _
                                                   strLocation, _
                                                   strUserId, _
                                                   objPRID, _
                                                   objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.PurchaseRequest))
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PURREQ_CT_LIST_UPD_CANCEL&errmesg=" & lblErrMesage.Text & "&redirect=CT/trx/CT_PurReq.aspx")
        End Try

        onProcess_Load(strPRID)
    End Sub


    Sub btnPreview_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim strOpCodePrint As String = "ADMIN_SHARE_UPD_PRINTDATE"
        Dim strUpdString As String = ""
        Dim strStatus As String
        Dim intStatus As Integer
        Dim strSortLine As String
        Dim strPrintDate As String
        Dim strTable As String

        strUpdString = "where PRID = '" & strPRID & "'"
        strStatus = Trim(Status.Text)
        intStatus = CInt(Trim(lblStatus.Text))
        strPrintDate = Trim(lblPrintDate.Text)
        strSortLine = "PRln.PRID"
        strTable = "IN_PR"

        If intStatus = objCT.EnumPurReqStatus.Confirmed Then
            If strPrintDate = "" Then
                Try
                    intErrNo = objAdmin.mtdUpdPrintDate(strOpCodePrint, _
                                                        strUpdString, _
                                                        strTable, _
                                                        strCompany, _
                                                        strLocation, _
                                                        strUserId)
                Catch Exp As System.Exception
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CT_CT_PURREQ_DETAILS_UPD_PRINTDATE&errmesg=" & lblErrMesage.Text & "&redirect=ct/trx/ct_purreq_details.aspx")
                End Try
                onProcess_Load(strPRID)
            Else
                strStatus = strStatus & " (re-printed)"
            End If
        End If

        Response.Write("<Script Language=""JavaScript"">window.open(""../../IN/reports/IN_Rpt_PurReqDet.aspx?strPRID=" & strPRID & _
                       "&strPrintDate=" & strPrintDate & "&strStatus=" & strStatus & "&strSortLine=" & strSortLine & """, null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")
    End Sub

    Sub btnBack_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CT_PurReq.aspx")
    End Sub

End Class
