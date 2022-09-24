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


Public Class CT_PurchaseRequest : Inherits Page

    Protected WithEvents dgPRListing As DataGrid
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblTracker As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents StatusList As DropDownList
    Protected WithEvents srchStatusList As DropDownList
    Protected WithEvents SortExpression As Label
    Protected WithEvents sortcol As Label
    Protected WithEvents srchPRID As TextBox
    Protected WithEvents srchUpdateBy As TextBox
    Protected WithEvents TotalAmount As Label

    Protected objCT As New agri.CT.clsTrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()

    Dim strOppCd_GET_PRList As String = "IN_CLSTRX_PURREQ_LIST_GET"
    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim intCTAR As Integer

    Dim objDataSet As New Object()
    Dim intCnt As Integer = 0
    Dim intErrNo As Integer
    Dim intPRCount As Integer
    Dim strAccMonth As String
    Dim strAccYear As String

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
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
                SortExpression.Text = "PRID"
                sortcol.Text = "ASC"
            End If

            If Not Page.IsPostBack Then
                BindSearchList()
                BindGrid()
                BindPageList()
                CheckStatus()
            End If
        End If
    End Sub

    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        dgPRListing.CurrentPageIndex = 0
        dgPRListing.EditItemIndex = -1
        BindGrid()
        CheckStatus()
        BindPageList()
    End Sub

    Sub BindGrid()

        Dim PageNo As Integer
        Dim PageCount As Integer
        Dim dsData As DataSet

        dsData = LoadData()
        PageCount = objGlobal.mtdGetPageCount(dsData.Tables(0).Rows.Count, dgPRListing.PageSize)

        dgPRListing.DataSource = dsData
        If dgPRListing.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgPRListing.CurrentPageIndex = 0
            Else
                dgPRListing.CurrentPageIndex = PageCount - 1
            End If
        End If

        dgPRListing.DataBind()
        BindPageList()
        PageNo = dgPRListing.CurrentPageIndex + 1
        lblTracker.Text = "Page " & PageNo & " of " & dgPRListing.PageCount
    End Sub

    Sub BindPageList()
        Dim count As Integer = 1
        Dim arrDList As New ArrayList()

        While Not count = dgPRListing.PageCount + 1
            arrDList.Add("Page " & count)
            count = count + 1
        End While
        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = dgPRListing.CurrentPageIndex

    End Sub


    Sub BindSearchList()
        srchStatusList.Items.Add(New ListItem(objCT.mtdGetPurReqStatus(objCT.EnumPurReqStatus.All), objCT.EnumPurReqStatus.All))
        srchStatusList.Items.Add(New ListItem(objCT.mtdGetPurReqStatus(objCT.EnumPurReqStatus.Active), objCT.EnumPurReqStatus.Active))
        srchStatusList.Items.Add(New ListItem(objCT.mtdGetPurReqStatus(objCT.EnumPurReqStatus.Cancelled), objCT.EnumPurReqStatus.Cancelled))
        srchStatusList.Items.Add(New ListItem(objCT.mtdGetPurReqStatus(objCT.EnumPurReqStatus.Confirmed), objCT.EnumPurReqStatus.Confirmed))
        srchStatusList.Items.Add(New ListItem(objCT.mtdGetPurReqStatus(objCT.EnumPurReqStatus.Deleted), objCT.EnumPurReqStatus.Deleted))

        srchStatusList.SelectedIndex = 1
    End Sub


    Protected Function LoadData() As DataSet
        Dim srchStatus As String
        Dim strParam As String
        Dim SearchStr As String
        Dim sortitem As String

        SearchStr = "AND PR.PRType = '" & objCT.EnumPurReqDocType.CanteenPR & "' AND PR.Status like '" & IIf(Not srchStatusList.SelectedItem.Value = objCT.EnumPurReqStatus.All, _
                     srchStatusList.SelectedItem.Value, "%") & "' AND PR.AccMonth = '" & strAccMonth & "' AND PR.AccYear = '" & strAccYear & "' AND PR.LocCode = '" & strLocation & "' "

        If Not srchPRID.Text = "" Then
            SearchStr = SearchStr & " AND PR.PRID like '" & srchPRID.Text & "%' "
        End If

        If Not srchUpdateBy.Text = "" Then
            SearchStr = SearchStr & " AND Usr.Username like '" & _
                        srchUpdateBy.Text & "%' "
        End If

        sortitem = "ORDER BY " & SortExpression.Text & " " & sortcol.Text
        strParam = SearchStr & "|" & sortitem

        Try
            intErrNo = objCT.mtdGetPurchaseRequest(strOppCd_GET_PRList, _
                                                   strParam, _
                                                   objCT.EnumPurReqDocType.CanteenPR, _
                                                   strAccMonth, _
                                                   strAccYear, _
                                                   strLocation, _
                                                   objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PURREQ_LIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=CT/trx/CT_trx_PurReq.aspx")
        End Try

        intPRCount = objDataSet.Tables(0).Rows.Count

        Return objDataSet
    End Function

    Sub btnPrevNext_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).CommandArgument
        Select Case direction
            Case "first"
                dgPRListing.CurrentPageIndex = 0
            Case "prev"
                dgPRListing.CurrentPageIndex = _
                    Math.Max(0, dgPRListing.CurrentPageIndex - 1)
            Case "next"
                dgPRListing.CurrentPageIndex = _
                    Math.Min(dgPRListing.PageCount - 1, dgPRListing.CurrentPageIndex + 1)
            Case "last"
                dgPRListing.CurrentPageIndex = dgPRListing.PageCount - 1
        End Select
        lstDropList.SelectedIndex = dgPRListing.CurrentPageIndex
        BindGrid()
        CheckStatus()
    End Sub

    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            dgPRListing.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
            CheckStatus()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        dgPRListing.CurrentPageIndex = e.NewPageIndex
        BindGrid()
        CheckStatus()
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        sortcol.Text = IIf(sortcol.Text = "ASC", "DESC", "ASC")
        dgPRListing.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
        CheckStatus()
    End Sub

    Sub CheckStatus()
        Dim strOppCd_GET_PRLnList As String = "IN_CLSTRX_PURREQLN_LIST_GET"
        Dim strParam As String
        Dim strParamTemp2 As String
        Dim strParamTemp3 As String
        Dim strPRStatus As String
        Dim objPRDs As DataSet
        Dim objPRLnDs As DataSet
        Dim DelButton As LinkButton
        Dim lblPRID As Label
        Dim strPRID As String
        Dim strPRIDTemp As String
        Dim strPRIDTemp2 As String
        Dim strQtyRcv As String

        For intCnt = 0 To dgPRListing.Items.Count - 1
            lblPRID = dgPRListing.Items.Item(intCnt).FindControl("PRID")
            strPRID = lblPRID.Text
            strParamTemp2 = "And PR.PRID = '" & Trim(strPRID) & "'|" & " "
            Try
                intErrNo = objCT.mtdGetPurchaseRequest(strOppCd_GET_PRList, _
                                                       strParamTemp2, _
                                                       objCT.EnumPurReqDocType.CanteenPR, _
                                                       strAccMonth, _
                                                       strAccYear, _
                                                       strLocation, _
                                                       objPRDs)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PURREQ_LIST_GET_CHECK_STATUS&errmesg=" & lblErrMessage.Text & "&redirect=CT/trx/CT_trx_PurReq.aspx")
            End Try

            strPRIDTemp = Trim(objPRDs.Tables(0).Rows(0).Item("PRID"))
            strPRStatus = Trim(objPRDs.Tables(0).Rows(0).Item("Status"))

            strParamTemp3 = strPRIDTemp & "|" & "PRLn.PRID"
            Try
                intErrNo = objCT.mtdGetPRLnList(strOppCd_GET_PRLnList, _
                                                strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strAccMonth, _
                                                strAccYear, _
                                                strParamTemp3, _
                                                objPRLnDs)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PURREQLN_LIST_GET_CHECK_STATUS&errmesg=" & lblErrMessage.Text & "&redirect=CT/trx/CT_trx_PurReq.aspx")
            End Try

            If objPRLnDs.Tables(0).Rows.Count > 0 Then
                strQtyRcv = objPRLnDs.Tables(0).Rows(0).Item("QtyRcv")

                If strPRStatus = objCT.EnumPurReqStatus.Active And strQtyRcv <> 0 Then
                    DelButton = dgPRListing.Items.Item(intCnt).FindControl("Delete")
                    DelButton.Visible = False
                ElseIf strPRStatus = objCT.EnumPurReqStatus.Active And strQtyRcv = "" Then
                    DelButton = dgPRListing.Items.Item(intCnt).FindControl("Delete")
                    DelButton.Visible = False
                ElseIf strPRStatus = objCT.EnumPurReqStatus.Confirmed Then
                    DelButton = dgPRListing.Items.Item(intCnt).FindControl("Delete")
                    DelButton.Visible = False
                ElseIf strPRStatus = objCT.EnumPurReqStatus.Deleted Then
                    DelButton = dgPRListing.Items.Item(intCnt).FindControl("Delete")
                    DelButton.Visible = False
                    DelButton = dgPRListing.Items.Item(intCnt).FindControl("Undelete")
                    DelButton.Visible = True
                ElseIf strPRStatus = objCT.EnumPurReqStatus.Cancelled Then
                    DelButton = dgPRListing.Items.Item(intCnt).FindControl("Delete")
                    DelButton.Visible = False
                    DelButton = dgPRListing.Items.Item(intCnt).FindControl("Undelete")
                    DelButton.Visible = False
                Else
                    DelButton = dgPRListing.Items.Item(intCnt).FindControl("Delete")
                    DelButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                End If
            Else
                If strPRStatus = objCT.EnumPurReqStatus.Active Then
                    DelButton = dgPRListing.Items.Item(intCnt).FindControl("Delete")
                    DelButton.Visible = True
                    DelButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                ElseIf strPRStatus = objCT.EnumPurReqStatus.Confirmed Then
                    DelButton = dgPRListing.Items.Item(intCnt).FindControl("Delete")
                    DelButton.Visible = False
                ElseIf strPRStatus = objCT.EnumPurReqStatus.Deleted Then
                    DelButton = dgPRListing.Items.Item(intCnt).FindControl("Delete")
                    DelButton.Visible = False
                    DelButton = dgPRListing.Items.Item(intCnt).FindControl("Undelete")
                    DelButton.Visible = True
                ElseIf strPRStatus = objCT.EnumPurReqStatus.Cancelled Then
                    DelButton = dgPRListing.Items.Item(intCnt).FindControl("Delete")
                    DelButton.Visible = False
                End If
            End If
        Next intCnt
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strOppCd_UpdPQ As String = "IN_CLSTRX_PURREQ_LIST_UPD"
        Dim strOppCd_GET_PRLnList As String = "IN_CLSTRX_PURREQLN_LIST_GET"
        Dim objPRLnDs As DataSet
        Dim objPRID As Object
        Dim strPRID As String
        Dim strPRStatus As Integer = objCT.EnumPurReqStatus.Deleted
        Dim strTotalAmt As String
        Dim strRemarks As String
        Dim DelText As Label
        Dim strParamTemp As String
        Dim strParam As String


        dgPRListing.EditItemIndex = CInt(E.Item.ItemIndex)
        DelText = dgPRListing.Items.Item(CInt(E.Item.ItemIndex)).FindControl("PRID")
        strPRID = DelText.Text
        DelText = dgPRListing.Items.Item(CInt(E.Item.ItemIndex)).FindControl("Remark")
        strRemarks = DelText.Text
        DelText = dgPRListing.Items.Item(CInt(E.Item.ItemIndex)).FindControl("TotalAmount")
        strTotalAmt = DelText.Text

        strParamTemp = Trim(strPRID) & "|" & "PRLn.PRID"
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PURREQLN_LIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=CT/trx/CT_trx_PurReq.aspx")
        End Try

        If objPRLnDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objPRLnDs.Tables(0).Rows.Count - 1
                strParam = Trim(strPRID) & "|" & Trim(strRemarks) & "|" & Trim(strPRStatus) & "|" & Trim(strTotalAmt)
                Try
                    intErrNo = objCT.mtdUpdPurchaseRequest(strOppCd_UpdPQ, _
                                                           strParam, _
                                                           strCompany, _
                                                           strLocation, _
                                                           strUserId, _
                                                           objPRID, _
                                                           objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.PurchaseRequest))
                Catch Exp As System.Exception
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PURREQ_LIST_UPD_WITHOUT_PURREQLN&errmesg=" & lblErrMessage.Text & "&redirect=CT/trx/CT_trx_PurReq.aspx")
                End Try
            Next intCnt
        Else
            strParam = Trim(strPRID) & "|" & Trim(strRemarks) & "|" & Trim(strPRStatus) & "|" & Trim(strTotalAmt)
            Try
                intErrNo = objCT.mtdUpdPurchaseRequest(strOppCd_UpdPQ, _
                                                       strParam, _
                                                       strCompany, _
                                                       strLocation, _
                                                       strUserId, _
                                                       objPRID, _
                                                       objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.PurchaseRequest))
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PURREQ_LIST_UPD_WITH_PURREQLN&errmesg=" & lblErrMessage.Text & "&redirect=CT/trx/CT_trx_PurReq.aspx")
            End Try
        End If
        BindGrid()
        CheckStatus()
        BindPageList()
    End Sub

    Sub DEDR_Undelete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strOppCd_UpdPQ As String = "IN_CLSTRX_PURREQ_LIST_UPD"
        Dim strOppCd_GET_PRLnList As String = "IN_CLSTRX_PURREQLN_LIST_GET"
        Dim objPRLnDs As DataSet
        Dim objPRID As Object
        Dim strPRID As String
        Dim strPRStatus As Integer = objCT.EnumPurReqStatus.Active
        Dim strPRStatusTemp As String
        Dim strTotalAmt As String
        Dim strRemarks As String
        Dim DelText As Label
        Dim strParamTemp As String
        Dim strParam As String


        dgPRListing.EditItemIndex = CInt(E.Item.ItemIndex)
        DelText = dgPRListing.Items.Item(CInt(E.Item.ItemIndex)).FindControl("PRID")
        strPRID = DelText.Text
        DelText = dgPRListing.Items.Item(CInt(E.Item.ItemIndex)).FindControl("Remark")
        strRemarks = DelText.Text
        DelText = dgPRListing.Items.Item(CInt(E.Item.ItemIndex)).FindControl("TotalAmount")
        strTotalAmt = DelText.Text

        strParamTemp = strPRID & "|" & "PRLn.PRID"
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PURREQLN_LIST_GET_UNDELETE&errmesg=" & lblErrMessage.Text & "&redirect=CT/trx/CT_trx_PurReq.aspx")
        End Try

        If objPRLnDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objPRLnDs.Tables(0).Rows.Count - 1
                strParam = Trim(strPRID) & "|" & Trim(strRemarks) & "|" & Trim(strPRStatus) & "|" & Trim(strTotalAmt)
                Try
                    intErrNo = objCT.mtdUpdPurchaseRequest(strOppCd_UpdPQ, _
                                                           strParam, _
                                                           strCompany, _
                                                           strLocation, _
                                                           strUserId, _
                                                           objPRID, _
                                                           objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.PurchaseRequest))
                Catch Exp As System.Exception
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PURREQ_LIST_UPD_MORERECS&errmesg=" & lblErrMessage.Text & "&redirect=CT/trx/CT_trx_PurReq.aspx")
                End Try
            Next intCnt
        Else
            strParam = Trim(strPRID) & "|" & Trim(strRemarks) & "|" & Trim(strPRStatus) & "|" & Trim(strTotalAmt)
            Try
                intErrNo = objCT.mtdUpdPurchaseRequest(strOppCd_UpdPQ, _
                                                       strParam, _
                                                       strCompany, _
                                                       strLocation, _
                                                       strUserId, _
                                                       objPRID, _
                                                       objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.PurchaseRequest))
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PURREQ_LIST_UPD_1REC&errmesg=" & lblErrMessage.Text & "&redirect=CT/trx/CT_trx_PurReq.aspx")
            End Try
        End If
        BindGrid()
        CheckStatus()
        BindPageList()
    End Sub

    Sub btnNewCanteenPR_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Response.Redirect("CT_PurReq_Details.aspx?prqtype=canteen")
    End Sub

End Class
