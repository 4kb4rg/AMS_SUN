
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


Public Class CT_Receive : Inherits Page

    Protected WithEvents dgCanteenReceive As DataGrid
    Protected WithEvents lblTracker As Label
    Protected WithEvents lblUnDel As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents SQLStatement As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents StatusList As DropDownList
    Protected WithEvents srchStatusList As DropDownList
    Protected WithEvents srchCanRcvType As DropDownList
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents blnUpdate As Label
    Protected WithEvents curStatus As Label
    Protected WithEvents srchCanteenTxID As TextBox
    Protected WithEvents srchRef As TextBox
    Protected WithEvents srchUpdateBy As TextBox

    Protected objCTtx As New agri.CT.clsTrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()

    Dim strOppCd_GET As String = "CT_CLSTRX_CANTEENRECEIVE_LIST_GET"
    Dim objDataSet As New DataSet()
    Dim intErrNo As Integer
    Dim strParam As String = ""

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim intCTAR As Integer

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_INACCMONTH")
        strAccYear = Session("SS_INACCYEAR")
        intCTAR = Session("SS_CTAR")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCTAccessRights.CTReceive), intCTAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            If SortExpression.Text = "" Then
                SortExpression.Text = "CanteenRcvID"
            End If
            If Not Page.IsPostBack Then
                BindSearchList()
                BindCanRcvTypeList()
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

        strParam = strLangCode & "|" & objGlobal.mtdGetModuleCode(objGlobal.EnumModule.Canteen)
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CT_TRX_CANRCV_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=../../menu/menu_CTTrx_Page.aspx")
        End Try


    End Sub

    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        dgCanteenReceive.CurrentPageIndex = 0
        dgCanteenReceive.EditItemIndex = -1
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
        PageCount = objGlobal.mtdGetPageCount(dsData.Tables(0).Rows.Count, dgCanteenReceive.PageSize)

        dgCanteenReceive.DataSource = dsData
        If dgCanteenReceive.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgCanteenReceive.CurrentPageIndex = 0
            Else
                dgCanteenReceive.CurrentPageIndex = PageCount - 1
            End If
        End If

        dgCanteenReceive.DataBind()
        BindPageList()
        PageNo = dgCanteenReceive.CurrentPageIndex + 1
        lblTracker.Text = "Page " & pageno & " of " & dgCanteenReceive.PageCount

        For intCnt = 0 To dgCanteenReceive.Items.Count - 1
            lbl = dgCanteenReceive.Items.Item(intCnt).FindControl("lblStatus")
            btn = dgCanteenReceive.Items.Item(intCnt).FindControl("Delete")
            If CInt(lbl.Text) = objCTtx.EnumCanteenReceiveStatus.Active Then
                btn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
            ElseIf CInt(lbl.Text) = objCTtx.EnumCanteenReceiveStatus.Deleted Then
                btn.Attributes("onclick") = "javascript:return ConfirmAction('undelete');"
                btn.Text = "Undelete"
            Else
                btn.Visible = False
            End If
        Next intCnt

    End Sub

    Sub BindSearchList()
        srchStatusList.Items.Add(New ListItem(objCTtx.mtdGetCanteenReceiveStatus(objCTtx.EnumCanteenReceiveStatus.All), objCTtx.EnumCanteenReceiveStatus.All))
        srchStatusList.Items.Add(New ListItem(objCTtx.mtdGetCanteenReceiveStatus(objCTtx.EnumCanteenReceiveStatus.Active), objCTtx.EnumCanteenReceiveStatus.Active))
        srchStatusList.Items.Add(New ListItem(objCTtx.mtdGetCanteenReceiveStatus(objCTtx.EnumCanteenReceiveStatus.Confirmed), objCTtx.EnumCanteenReceiveStatus.Confirmed))
        srchStatusList.Items.Add(New ListItem(objCTtx.mtdGetCanteenReceiveStatus(objCTtx.EnumCanteenReceiveStatus.Deleted), objCTtx.EnumCanteenReceiveStatus.Deleted))

        srchStatusList.SelectedIndex = 1
    End Sub

    Sub BindCanRcvTypeList()
        srchCanRcvType.Items.Add(New ListItem(objCTtx.mtdGetCanteenReceiveDocType(objCTtx.EnumCanteenReceiveDocType.All), objCTtx.EnumCanteenReceiveDocType.All))
        srchCanRcvType.Items.Add(New ListItem(objCTtx.mtdGetCanteenReceiveDocType(objCTtx.EnumCanteenReceiveDocType.DispatchAdvice), objCTtx.EnumCanteenReceiveDocType.DispatchAdvice))
        srchCanRcvType.Items.Add(New ListItem(objCTtx.mtdGetCanteenReceiveDocType(objCTtx.EnumCanteenReceiveDocType.StockReturnAdvice), objCTtx.EnumCanteenReceiveDocType.StockReturnAdvice))
        srchCanRcvType.Items.Add(New ListItem(objCTtx.mtdGetCanteenReceiveDocType(objCTtx.EnumCanteenReceiveDocType.StockTransfer), objCTtx.EnumCanteenReceiveDocType.StockTransfer))
    End Sub


    Sub BindPageList()
        Dim count As Integer = 1
        Dim arrDList As New ArrayList()

        While Not count = dgCanteenReceive.PageCount + 1
            arrDList.Add("Page " & count)
            count = count + 1
        End While
        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = dgCanteenReceive.CurrentPageIndex
    End Sub

    Protected Function LoadData() As DataSet
        Dim CanteenCode As String
        Dim Desc As String
        Dim UpdateBy As String
        Dim srchStatus As String
        Dim strParam As String
        Dim SearchStr As String
        Dim SortItem As String

        strParam = srchCanteenTxID.Text & "|" & srchRef.Text & "|" & srchUpdateBy.Text & "|" & _
                   srchStatusList.SelectedItem.Value & "|" & SortExpression.Text & "|" & _
                   SortCol.Text & "|" & strAccMonth & "|" & strAccYear & "|" & strLocation & "|" & srchCanRcvType.SelectedItem.Value.Trim

        Try
            intErrNo = objCTtx.mtdGetCanteenReceiveList(strOppCd_GET, strParam, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_CANTEENRECEIVELIST&errmesg=" & lblErrMessage.Text & "&redirect=../EN/menu/menu_CTTrx_page.aspx")
        End Try
        Return objDataSet
    End Function

    Sub btnPrevNext_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).CommandArgument
        Select Case direction
            Case "first"
                dgCanteenReceive.CurrentPageIndex = 0
            Case "prev"
                dgCanteenReceive.CurrentPageIndex = _
                    Math.Max(0, dgCanteenReceive.CurrentPageIndex - 1)
            Case "next"
                dgCanteenReceive.CurrentPageIndex = _
                    Math.Min(dgCanteenReceive.PageCount - 1, dgCanteenReceive.CurrentPageIndex + 1)
            Case "last"
                dgCanteenReceive.CurrentPageIndex = dgCanteenReceive.PageCount - 1
        End Select
        lstDropList.SelectedIndex = dgCanteenReceive.CurrentPageIndex
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strTxParam As String
        Dim strOpCdCanteenTxDet_ADD As String = "CT_CLSTRX_CANTEENRECEIVE_DETAIL_ADD"
        Dim strOpCdCanteenTxDet_UPD As String = "CT_CLSTRX_CANTEENRECEIVE_DETAIL_UPD"
        Dim strOpCdCanteenTxLine_GET As String = "CT_CLSTRX_CANTEENRECEIVE_LINE_GET"
        Dim strOpCdItem_Details_UPD As String = "IN_CLSTRX_STOCKITEM_DETAIL_UPD"
        Dim strOpCdItem_Details_GET As String = "IN_CLSSETUP_STOCKITEM_DETAILS_GET"
        Dim strOpPRLNDet_Details_GET As String = "IN_CLSTRX_PURREQLN_DET_GET"
        Dim strOpPRLN_UPD As String = "IN_CLSTRX_PRLN_UPD"
        Dim strOpCdPR_Count_GET As String = "IN_CLSTRX_PURREQLN_FINDLIST_GET"
        Dim strOpCdPR_Details_UPD As String = "IN_CLSTRX_PURREQ_DET_UPD"

        Dim TxID As String
        Dim strTxID As String
        Dim strStatus As String
        Dim ErrorChk As Integer = objCTtx.EnumInventoryErrorType.NoError
        Dim lbl As Label

        lbl = E.Item.FindControl("lblTxID")
        strTxID = Trim(lbl.Text)
        lbl = E.Item.FindControl("lblStatus")
        strStatus = Trim(lbl.Text)

        If strStatus = objCTtx.EnumCanteenReceiveStatus.Deleted Then
            Try
                intErrNo = objCTtx.mtdReceiveInvItemLevel(strOpCdCanteenTxLine_GET, _
                                                          strOpCdItem_Details_UPD, _
                                                          strOpCdItem_Details_GET, _
                                                          strOpCdPR_Count_GET, _
                                                          strOpCdPR_Details_UPD, _
                                                          strCompany, _
                                                          strLocation, _
                                                          strUserId, _
                                                          strAccMonth, _
                                                          strAccYear, _
                                                          strTxID, _
                                                          strOpPRLNDet_Details_GET, _
                                                          strOpPRLN_UPD, _
                                                          objCTtx.EnumTransactionAction.Undelete, _
                                                          objCTtx.EnumCanteenReceiveDocType.DispatchAdvice, _
                                                          ErrorChk)
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=UNDELETE_CANTEENRECEIVE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_CanteenReceiveList.aspx")
                End If
            End Try

            strTxParam = strTxID & "|||||||||||||||" & objCTtx.EnumCanteenReceiveStatus.Active

        ElseIf strStatus = objCTtx.EnumCanteenReceiveStatus.Active Then
            Try
                intErrNo = objCTtx.mtdReceiveInvItemLevel(strOpCdCanteenTxLine_GET, _
                                                          strOpCdItem_Details_UPD, _
                                                          strOpCdItem_Details_GET, _
                                                          strOpCdPR_Count_GET, _
                                                          strOpCdPR_Details_UPD, _
                                                          strCompany, _
                                                          strLocation, _
                                                          strUserId, _
                                                          strAccMonth, _
                                                          strAccYear, _
                                                          strTxID, _
                                                          strOpPRLNDet_Details_GET, _
                                                          strOpPRLN_UPD, _
                                                          objCTtx.EnumTransactionAction.Delete, _
                                                          objCTtx.EnumCanteenReceiveDocType.DispatchAdvice, _
                                                          ErrorChk)
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=DELETE_CANTEENRECEIVE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_CanteenReceiveList.aspx")
                End If
            End Try

            strTxParam = strTxID & "|||||||||||||||" & objCTtx.EnumCanteenReceiveStatus.Deleted & "|"
        End If

        If intErrNo = 0 And Not ErrorChk = objCTtx.EnumInventoryErrorType.InsufficientQty Then
            Try
                intErrNo = objCTtx.mtdUpdCanteenReceiveDetail(strOpCdCanteenTxDet_ADD, _
                                                              strOpCdCanteenTxDet_UPD, _
                                                              strOpCdCanteenTxLine_GET, _
                                                              strCompany, _
                                                              strLocation, _
                                                              strUserId, _
                                                              strAccMonth, _
                                                              strAccYear, _
                                                              strTxParam, _
                                                              objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CTCanteenReceive), _
                                                              ErrorChk, _
                                                              TxID)
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_NEWCANTEENRECEIVE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_CanteenReceiveList.aspx")
                End If
            End Try

        End If

        If ErrorChk = objCTtx.EnumInventoryErrorType.InsufficientQty Then
            lblUnDel.Visible = True
        End If
        BindGrid()
        BindPageList()
    End Sub

    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            dgCanteenReceive.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        dgCanteenReceive.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIf(SortCol.Text = "ASC", "DESC", "ASC")
        dgCanteenReceive.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
    End Sub

    Sub btnNewItm_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Response.Redirect("../../CT/Trx/CT_Trx_CanteenReceiveDet.aspx")
    End Sub

End Class
