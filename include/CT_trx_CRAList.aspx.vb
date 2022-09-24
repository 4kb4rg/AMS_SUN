
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


Public Class CT_CanRetAdvList : Inherits Page

    Protected WithEvents dgStockTx As DataGrid
    Protected WithEvents lblTracker As Label
    Protected WithEvents lblUnDel As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents StatusList As DropDownList
    Protected WithEvents srchStatusList As DropDownList
    Protected WithEvents SortExpression As Label
    Protected WithEvents sortcol As Label
    Protected WithEvents blnUpdate As Label
    Protected WithEvents curStatus As Label
    Protected WithEvents srchStockTxID As TextBox
    Protected WithEvents srchRef As TextBox
    Protected WithEvents srchUpdateBy As TextBox

    Protected objCT As New agri.CT.clsTrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()

    Dim strOppCd_GET As String = "IN_CLSTRX_STOCKRETADV_LIST_GET"
    Dim objDataSet As New DataSet()
    Dim intErrNo As Integer
    Dim strParam As String = ""

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intCTAR As Integer

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strAccMonth = Session("SS_INACCMONTH")
        strAccYear = Session("SS_INACCYEAR")
        strLangCode = Session("SS_LANGCODE")
        intCTAR = Session("SS_CTAR")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCTAccessRights.CTReturnAdvice), intCTAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else

            If SortExpression.Text = "" Then
                SortExpression.Text = "ItemRetAdvID"
            End If
            If Not Page.IsPostBack Then
                BindSearchList()
                BindGrid()
                BindPageList()
            End If
        End If
    End Sub

    Sub onload_GetLangCap()
        Dim strOpCode_BussTerm As String = "PWSYSTEM_CLSLANGCAP_BUSSTERM_GET"
        Dim objLangCapDs As New DataSet()
        Dim strParam As String
        Dim intErrNo As Integer

        strParam = strLangCode & "|" & objGlobal.mtdGetModuleCode(objGlobal.EnumModule.Inventory)
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=WS_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=../../menu/menu_INTrx_Page.aspx")
        End Try


    End Sub

    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        dgStockTx.CurrentPageIndex = 0
        dgStockTx.EditItemIndex = -1
        BindGrid()
        BindPageList()
    End Sub

    Sub BindGrid()
        Dim intCnt As Integer
        Dim lbl As Label
        Dim btn As LinkButton

        Dim PageNo As Integer 
        Dim PageCount As Integer
        Dim dsData As DataSet
        
        dsData = LoadData
        PageCount = objGlobal.mtdGetPageCount(dsData.Tables(0).Rows.Count, dgStockTx.PageSize)
        
        dgStockTx.DataSource = dsData
        If dgStockTx.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgStockTx.CurrentPageIndex = 0
            Else
                dgStockTx.CurrentPageIndex = PageCount - 1
            End If
        End If
        
        dgStockTx.DataBind()
        BindPageList()
        PageNo = dgStockTx.CurrentPageIndex + 1
        lblTracker.Text="Page " & pageno & " of " & dgStockTx.PageCount

        For intCnt = 0 To dgStockTx.Items.Count - 1
            lbl = dgStockTx.Items.Item(intCnt).FindControl("lblStatus")
            btn = dgStockTx.Items.Item(intCnt).FindControl("Delete")
            If CInt(lbl.Text) = objCT.EnumStockTransferStatus.Active Then
                btn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
            ElseIf CInt(lbl.Text) = objCT.EnumStockTransferStatus.Deleted Then
                btn.Attributes("onclick") = "javascript:return ConfirmAction('undelete');"
                btn.Text = "Undelete"
            Else
                btn.Visible = False
            End If
        Next intCnt

    End Sub
    Sub BindSearchList()

        srchStatusList.Items.Add(New ListItem(objCT.mtdGetCanRetAdvStatus(objCT.EnumCanRetAdvStatus.All), objCT.EnumCanRetAdvStatus.All))
        srchStatusList.Items.Add(New ListItem(objCT.mtdGetCanRetAdvStatus(objCT.EnumCanRetAdvStatus.Active), objCT.EnumCanRetAdvStatus.Active))
        srchStatusList.Items.Add(New ListItem(objCT.mtdGetCanRetAdvStatus(objCT.EnumCanRetAdvStatus.Confirmed), objCT.EnumCanRetAdvStatus.Confirmed))
        srchStatusList.Items.Add(New ListItem(objCT.mtdGetCanRetAdvStatus(objCT.EnumCanRetAdvStatus.Deleted), objCT.EnumCanRetAdvStatus.Deleted))
        srchStatusList.SelectedIndex = 1
    End Sub
    Sub BindPageList()

        Dim count As Integer = 1
        Dim arrDList As New ArrayList()

        While Not count = dgStockTx.PageCount + 1
            arrDList.Add("Page " & count)
            count = count + 1
        End While
        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = dgStockTx.CurrentPageIndex

    End Sub

    Protected Function LoadData() As DataSet

        Dim StockCode As String
        Dim Desc As String
        Dim UpdateBy As String
        Dim srchStatus As String
        Dim strParam As String
        Dim SearchStr As String
        Dim sortitem As String

        strParam = srchStockTxID.Text & "|" & objGlobal.EnumDocType.CanteenReturnAdvice & "|" & srchUpdateBy.Text & "|" & _
                    srchStatusList.SelectedItem.Value & "|" & SortExpression.Text & "|" & sortcol.Text _
                    & "|" & strAccMonth & "|" & strAccYear & "|" & strLocation

        Try
            intErrNo = objCT.mtdGetStockRetAdvList(strOppCd_GET, strParam, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_STOCKRETADVLIST&errmesg=" & lblErrMessage.Text & "&redirect=../EN/menu/menu_INTrx_page.aspx")
        End Try
        Return objDataSet
    End Function

    Sub btnPrevNext_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).CommandArgument
        Select Case direction
            Case "first"
                dgStockTx.CurrentPageIndex = 0
            Case "prev"
                dgStockTx.CurrentPageIndex = _
                    Math.Max(0, dgStockTx.CurrentPageIndex - 1)
            Case "next"
                dgStockTx.CurrentPageIndex = _
                    Math.Min(dgStockTx.PageCount - 1, dgStockTx.CurrentPageIndex + 1)
            Case "last"
                dgStockTx.CurrentPageIndex = dgStockTx.PageCount - 1
        End Select
        lstDropList.SelectedIndex = dgStockTx.CurrentPageIndex
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)

        Dim StrTxParam As String

        Dim strOpCdStckTxDet_ADD As String = "IN_CLSTRX_ITEMRETURNADVICE_ADD"
        Dim strOpCdStckTxDet_UPD As String = "IN_CLSTRX_ITEMRETURNADVICE_SELECTIVE_UPD"
        Dim strOpCdStckTxLine_GET As String = "IN_CLSTRX_ITEMRETURNADVICE_LINE_GET"
        Dim strOpCdItem_Details_GET As String = "IN_CLSSETUP_STOCKITEM_DETAILS_GET"
        Dim strOpCdItem_Details_UPD As String = "IN_CLSTRX_STOCKITEM_DETAIL_UPD"
        Dim strOpCdStckRecvLine_Det_GET As String = "CT_CLSTRX_CANTEENRECEIVELINE_DETAILS_GET" 
        Dim strOpCdStckRecvLine_Det_UPD As String = "CT_CLSTRX_CANTEENRECEIVE_LINE_UPD"
        Dim TxID As String
        Dim strTxID As String
        Dim strstatus As String
        Dim ErrorChk As Integer = objCT.EnumInventoryErrorType.NoError
        Dim lbl As Label

        lbl = E.Item.FindControl("lblTxID")
        strTxID = Trim(lbl.Text)
        lbl = E.Item.FindControl("lblStatus")
        strstatus = Trim(lbl.Text)
        If strstatus = objCT.EnumCanRetAdvStatus.Deleted Then
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
                                                            strOpCdItem_Details_GET, _
                                                            strTxID, _
                                                            objCT.EnumTransactionAction.Undelete, _
                                                            ErrorChk)
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=UNDELETESTOCKRETURNADVICE&errmesg=" & lblErrMessage.Text & "&redirect=IN/Trx/IN_Trx_StockRetAdv_List.aspx")
                End If
            End Try
            StrTxParam = strTxID & "|||||||||||||" & objCT.EnumCanRetAdvStatus.Active

        ElseIf strstatus = objCT.EnumCanRetAdvStatus.Active Then
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
                                                            strOpCdItem_Details_GET, _
                                                            strTxID, _
                                                            objCT.EnumTransactionAction.Delete, _
                                                            ErrorChk)
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=DELETENEWSTOCKRETURNADVICE&errmesg=" & lblErrMessage.Text & "&redirect=IN/Trx/IN_Trx_StockRetAdv_List.aspx")
                End If
            End Try
            StrTxParam = strTxID & "|||||||||||||" & objCT.EnumCanRetAdvStatus.Deleted & "|"
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
                                                           objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.StockReturnAdvice), _
                                                           ErrorChk, _
                                                           TxID)
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ADDNEWSTOCKRETURNADVICE&errmesg=" & lblErrMessage.Text & "&redirect=IN/Trx/IN_Trx_StockRetAdv_List.aspx")
                End If
            End Try
        End If
        If ErrorChk = objCT.EnumInventoryErrorType.InsufficientQty Then
            lblUnDel.Visible = True
        End If
        BindGrid()
        BindPageList()
    End Sub

    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            dgStockTx.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        dgStockTx.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        sortcol.Text = IIf(sortcol.Text = "ASC", "DESC", "ASC")
        dgStockTx.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
    End Sub

    Sub btnNewItm_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Response.Redirect("../../CT/Trx/CT_Trx_CRADetails.aspx")
    End Sub

End Class
