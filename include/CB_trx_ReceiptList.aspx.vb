
Imports System
Imports System.Data

Public Class cb_trx_ReceiptList : Inherits Page

    Protected WithEvents dgLine As DataGrid
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblTracker As Label
    Protected WithEvents lblBillPartyTag As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents txtReceiptID As TextBox
    Protected WithEvents txtBillParty As TextBox
    Protected WithEvents ddlStatus As DropDownList
    Protected WithEvents txtLastUpdate As TextBox
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents lstAccMonth As DropDownList
    Protected WithEvents lstAccYear As DropDownList
    Protected WithEvents txtDocID As TextBox

    Protected objCBTrx As New agri.CB.clsTrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim objGLtrx As New agri.GL.ClsTrx()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intCBAR As Integer

    Dim objReceiptDs As New Object()
    Dim objLangCapDs As New Object()
    Dim strParam As String
    Dim intErrNo As Integer
    Dim strLocType as String
    Dim intLevel As Integer
    Dim strSelAccMonth As String
    Dim strSelAccYear As String

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strAccMonth = Session("SS_CBACCMONTH")
        strAccYear = Session("SS_CBACCYEAR")
        strLangCode = Session("SS_LANGCODE")
        intCBAR = Session("SS_CBAR")
        strLocType = Session("SS_LOCTYPE")
        intLevel = Session("SS_USRLEVEL")
        strSelAccMonth = Session("SS_SELACCMONTH")
        strSelAccYear = Session("SS_SELACCYEAR")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCBAccessRights.CBReceipt), intCBAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            If SortExpression.Text = "" Then
                SortExpression.Text = "RC.ReceiptID"
            End If

            If Not Page.IsPostBack Then
                If Session("SS_FILTERPERIOD") = "0" Then
                    lstAccMonth.SelectedValue = strAccMonth
                    BindAccYear(strAccYear)
                Else
                    lstAccMonth.SelectedValue = strSelAccMonth
                    BindAccYear(strSelAccYear)
                End If

                onload_GetLangCap()
                BindSearchList()
                BindGrid()
                BindPageList()
            End If
        End If
    End Sub

    Sub onload_GetLangCap()

        GetEntireLangCap()

        lblBillPartyTag.Text = GetCaption(objLangCap.EnumLangCap.BillParty) & lblCode.Text
        'dgLine.Columns(1).HeaderText = lblBillPartyTag.Text
        'dgLine.Columns(2).HeaderText = GetCaption(objLangCap.EnumLangCap.BillParty) & " Name"
    End Sub

    Sub GetEntireLangCap()
        Dim strOpCode_BussTerm As String = "PWSYSTEM_CLSLANGCAP_BUSSTERM_GET"

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
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_CLSTRX_RECEIPT_LIST_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=CB/trx/CB_trx_ReceiptList.aspx")
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



    Sub BindSearchList()

        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetReceiptStatus(objCBTrx.EnumReceiptStatus.All), objCBTrx.EnumReceiptStatus.All))
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetReceiptStatus(objCBTrx.EnumReceiptStatus.Active), objCBTrx.EnumReceiptStatus.Active))
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetReceiptStatus(objCBTrx.EnumReceiptStatus.Closed), objCBTrx.EnumReceiptStatus.Closed))
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetReceiptStatus(objCBTrx.EnumReceiptStatus.Confirmed), objCBTrx.EnumReceiptStatus.Confirmed))
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetReceiptStatus(objCBTrx.EnumReceiptStatus.Deleted), objCBTrx.EnumReceiptStatus.Deleted))
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetReceiptStatus(objCBTrx.EnumReceiptStatus.Void), objCBTrx.EnumReceiptStatus.Void))

        'ddlStatus.SelectedIndex = 1
        If intLevel = 0 Then
            ddlStatus.SelectedIndex = 1
        Else
            ddlStatus.SelectedIndex = 0
        End If
    End Sub

    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        dgLine.CurrentPageIndex = 0
        dgLine.EditItemIndex = -1
        BindGrid()
        BindPageList()
    End Sub


    Sub BindGrid()
        Dim PageNo As Integer
        Dim PageCount As Integer
        Dim lbButton As LinkButton
        Dim strSrchID As String
        Dim strSrchBillParty As String
        Dim strSrchStatus As String
        Dim strSrchLastUpdate As String
        Dim strSearch As String
        Dim intCnt As Integer
        Dim strOppCode = "CB_CLSTRX_RECEIPT_GET"
        Dim lbl As Label
        Dim strDocID As String

        If lstAccMonth.SelectedItem.Value = "0" Then
            strAccMonth = "1','2','3','4','5','6','7','8','9','10','11','12"
        Else
            strAccMonth = lstAccMonth.SelectedItem.Value
        End If

        strAccYear = lstAccYear.SelectedItem.Value
        strSrchID = IIf(txtReceiptID.Text = "", "", txtReceiptID.Text)
        strSrchBillParty = IIf(txtBillParty.Text = "", "", txtBillParty.Text)
        strSrchStatus = IIf(ddlStatus.SelectedItem.Value = objCBTrx.EnumReceiptStatus.All, "", ddlStatus.SelectedItem.Value)
        strSrchLastUpdate = IIf(txtLastUpdate.Text = "", "", txtLastUpdate.Text)
        strDocID = IIf(txtDocID.Text = "", "", txtDocID.Text)

        strParam = strSrchID & "|" & _
                   strSrchBillParty & "|" & _
                   strSrchStatus & "|" & _
                   strSrchLastUpdate & "|" & _
                   strDocID & "|" & _
                   SortExpression.Text & "|" & _
                   SortCol.Text

        Try
            intErrNo = objCBTrx.mtdGetReceipt(strOppCode, _
                                              strCompany, _
                                              strLocation, _
                                              strUserId, _
                                              strAccMonth, _
                                              strAccYear, _
                                              strParam, _
                                              objReceiptDs, _
                                              False)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_RECEIPTLIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To objReceiptDs.Tables(0).Rows.Count - 1
            objReceiptDs.Tables(0).Rows(intCnt).Item("ReceiptID") = Trim(objReceiptDs.Tables(0).Rows(intCnt).Item("ReceiptID"))
            objReceiptDs.Tables(0).Rows(intCnt).Item("Remark") = Trim(objReceiptDs.Tables(0).Rows(intCnt).Item("Remark"))
            objReceiptDs.Tables(0).Rows(intCnt).Item("Status") = Trim(objReceiptDs.Tables(0).Rows(intCnt).Item("Status"))
            objReceiptDs.Tables(0).Rows(intCnt).Item("UpdateID") = Trim(objReceiptDs.Tables(0).Rows(intCnt).Item("UpdateID"))
        Next

        PageCount = objGlobal.mtdGetPageCount(objReceiptDs.Tables(0).Rows.Count, dgLine.PageSize)
        dgLine.DataSource = objReceiptDs
        If dgLine.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgLine.CurrentPageIndex = 0
            Else
                dgLine.CurrentPageIndex = PageCount - 1
            End If
        End If
        dgLine.DataBind()
        BindPageList()

        For intCnt = 0 To dgLine.Items.Count - 1
            lbl = dgLine.Items.Item(intCnt).FindControl("lblStatus") 
           	Select Case CInt(Trim(lbl.Text)) 
                Case objCBTrx.EnumReceiptStatus.Active
                    lbButton = dgLine.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = True
                    lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                Case objCBTrx.EnumReceiptStatus.Deleted
                    If lstAccMonth.SelectedItem.Value >= Session("SS_CBACCMONTH") Then
                        lbButton = dgLine.Items.Item(intCnt).FindControl("lbDelete")
                        lbButton.Text = "Undelete"
                        lbButton.Visible = True
                        lbButton.Attributes("onclick") = "javascript:return ConfirmAction('undelete');"
                    Else
                        lbButton = dgLine.Items.Item(intCnt).FindControl("lbDelete")
                        lbButton.Text = "Undelete"
                        lbButton.Visible = False
                        lbButton.Attributes("onclick") = "javascript:return ConfirmAction('undelete');"
                    End If
                Case Else
                    lbButton = dgLine.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False
            End Select
        Next

        PageNo = dgLine.CurrentPageIndex + 1
        lblTracker.Text = "Page " & PageNo & " of " & dgLine.PageCount
    End Sub

    Sub BindPageList()
        Dim count As Integer = 1
        Dim arrDList As New ArrayList()

        While Not count = dgLine.PageCount + 1
            arrDList.Add("Page " & count)
            count = count + 1
        End While
        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = dgLine.CurrentPageIndex
    End Sub


    Sub btnPrevNext_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).CommandArgument
        Select Case direction
            Case "first"
                dgLine.CurrentPageIndex = 0
            Case "prev"
                dgLine.CurrentPageIndex = _
                Math.Max(0, dgLine.CurrentPageIndex - 1)
            Case "next"
                dgLine.CurrentPageIndex = _
                Math.Min(dgLine.PageCount - 1, dgLine.CurrentPageIndex + 1)
            Case "last"
                dgLine.CurrentPageIndex = dgLine.PageCount - 1
        End Select
        lstDropList.SelectedIndex = dgLine.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            dgLine.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        dgLine.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIf(SortCol.Text = "ASC", "DESC", "ASC")
        dgLine.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strOpCd_GetLine As String = "CB_CLSTRX_RECEIPT_LINE_GET"
        Dim strOpCode_Receipt_Upd As String = "CB_CLSTRX_RECEIPT_UPD"
        Dim strOpCode_Invoice_Upd As String = "CB_CLSTRX_INVOICE_UPD"
        Dim strOpCode_DebitNote_Upd As String = "CB_CLSTRX_DEBITNOTE_UPD"
        Dim strOpCode_CreditNote_Upd As String = "CB_CLSTRX_CREDITNOTE_UPD"
        Dim strOpCode_Invoice_OutstdAmount_Get As String = "CB_CLSTRX_INVOICE_OUTSTDAMT_GET"
        Dim strOpCode_DebitNote_OutstdAmount_Get As String = "CB_CLSTRX_DEBITNOTE_OUTSTDAMT_GET"
        Dim strOpCode_CreditNote_OutstdAmount_Get As String = "CB_CLSTRX_CREDITNOTE_OUTSTDAMT_GET"
        Dim lblDelText As Label
        Dim strReceiptID As String
        Dim strStatus As String
        Dim strOpCodes As String
        Dim objReceiptLnDs As New DataSet()
        Dim dsReceipt As New DataSet()
        Dim strLnID As String
        Dim strDocID As String
        Dim strDocType As String
        Dim strAmount As String
        Dim intCnt As Integer

        dgLine.EditItemIndex = CInt(E.Item.ItemIndex)
        lblDelText = dgLine.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lblReceiptId")
        strReceiptID = lblDelText.Text
        lblDelText = dgLine.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lblStatus")
        strStatus = lblDelText.Text
        strParam = strReceiptID
        strOpCodes = "|" & strOpCode_Receipt_Upd

        If strStatus.Trim() = objCBTrx.EnumReceiptStatus.Active Then
            strParam = "|" & strReceiptID & "|||||" & objCBTrx.EnumReceiptStatus.Deleted & "||||"
        Else
            strParam = "|" & strReceiptID & "|||||" & objCBTrx.EnumReceiptStatus.Active & "||||"
        End If

        Try
            intErrNo = objCBTrx.mtdUpdReceipt(strOpCodes, _
                                              strCompany, _
                                              strLocation, _
                                              strUserId, _
                                              strAccMonth, _
                                              strAccYear, _
                                              strParam, _
                                              dsReceipt)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_RECEIPTLIST_DEL&errmesg=" & lblErrMessage.Text & "&redirect=CB/trx/CB_trx_ReceiptList.aspx")
        End Try

        BindGrid()
        BindPageList()
    End Sub


    Sub NewBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CB_trx_ReceiptDet.aspx")
    End Sub

    Sub BindAccYear(ByVal pv_strAccYear As String)
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer
        Dim intCnt As Integer = 0
        Dim intSelectedIndex As Integer
        Dim objAccYearDs As New Object
        Dim dr As DataRow
        Dim strOpCd As String = "ADMIN_CLSACCPERIOD_CONFIG_GET"

        strParamName = "LOCCODE|SEARCHSTR|SORTEXP"
        strParamValue = strLocation & "||Order By HD.AccYear"

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCd, _
                                                strParamName, _
                                                strParamValue, _
                                                objAccYearDs)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GET_ACCYEAR&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To objAccYearDs.Tables(0).Rows.Count - 1
            objAccYearDs.Tables(0).Rows(intCnt).Item("AccYear") = Trim(objAccYearDs.Tables(0).Rows(intCnt).Item("AccYear"))
            objAccYearDs.Tables(0).Rows(intCnt).Item("UserName") = Trim(objAccYearDs.Tables(0).Rows(intCnt).Item("AccYear"))
            If objAccYearDs.Tables(0).Rows(intCnt).Item("AccYear") = pv_strAccYear Then
                intSelectedIndex = intCnt + 1
            End If
        Next intCnt

        lstAccYear.DataSource = objAccYearDs.Tables(0)
        lstAccYear.DataValueField = "AccYear"
        lstAccYear.DataTextField = "UserName"
        lstAccYear.DataBind()
        lstAccYear.SelectedIndex = intSelectedIndex - 1
    End Sub


    Sub dgLine_BindGrid(ByVal sender As Object, ByVal e As DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor='lightblue'")
            If e.Item.ItemType = ListItemType.AlternatingItem Then
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='e9e9e9'")
            Else
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='f2f2f2'")
            End If
        End If
    End Sub

End Class
