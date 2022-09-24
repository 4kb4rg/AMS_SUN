

Imports System
Imports System.Data
Imports System.Collections 
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic.Interaction



Public Class ap_trx_DNList : Inherits Page

    Protected WithEvents dgDebitNote As DataGrid
    Protected WithEvents lblTracker As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents ddlStatus As DropDownList
    Protected WithEvents txtDNID As TextBox
    Protected WithEvents txtDNRefNoID As TextBox
    Protected WithEvents txtSupplier As TextBox
    Protected WithEvents txtLastUpdate As TextBox
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents lblErrMesage As Label
    Protected WithEvents lblInvoiceRcvRefNo As Label
    Protected WithEvents txtInvoiceRcvRefNo As TextBox
    Protected WithEvents lstAccMonth As DropDownList
    Protected WithEvents lstAccYear As DropDownList

    Protected WithEvents NewDNBtn As ImageButton
    Protected WithEvents ibPrint As ImageButton

    Protected objAPTrx As New agri.AP.clsTrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim objGLtrx As New agri.GL.ClsTrx()
    Dim objLangCapDs As New Object()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intAPAR As Integer
    Dim objDebitNoteDs As New Object()
    Dim strLocType as String
    Dim strSelAccMonth As String
    Dim strSelAccYear As String
    Dim intLevel As Integer

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strAccMonth = Session("SS_APACCMONTH")
        strAccYear = Session("SS_APACCYEAR")
        strSelAccMonth = Session("SS_SELACCMONTH")
        strSelAccYear = Session("SS_SELACCYEAR")
        strLangCode = Session("SS_LANGCODE")
        intAPAR = Session("SS_APAR")
        strLocType = Session("SS_LOCTYPE")
        intLevel = Session("SS_USRLEVEL")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumAPAccessRights.APDebitNote), intAPAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            'to avoid double click, on aspx add this : UseSubmitBehavior="false"
            NewDNBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(NewDNBtn).ToString())
            'ibPrint.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(ibPrint).ToString())

            onload_GetLangCap()
            If SortExpression.Text = "" Then
                SortExpression.Text = "DN.DebitNoteID"
            End If

            If Not Page.IsPostBack Then
                If Session("SS_FILTERPERIOD") = "0" Then
                    lstAccMonth.SelectedValue = strAccMonth
                    BindAccYear(strAccYear)
                Else
                    lstAccMonth.SelectedValue = strSelAccMonth
                    BindAccYear(strSelAccYear)
                End If

                BindStatusList()
                BindGrid()
                BindPageList()
            End If
        End If
    End Sub

    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        dgDebitNote.CurrentPageIndex = 0
        dgDebitNote.EditItemIndex = -1
        BindGrid()
        BindPageList()
    End Sub

    Sub BindStatusList()
        ddlStatus.Items.Add(New ListItem(objAPTrx.mtdGetDebitNoteStatus(objAPTrx.EnumDebitNoteStatus.All), objAPTrx.EnumDebitNoteStatus.All))
        ddlStatus.Items.Add(New ListItem(objAPTrx.mtdGetDebitNoteStatus(objAPTrx.EnumDebitNoteStatus.Active), objAPTrx.EnumDebitNoteStatus.Active))
        ddlStatus.Items.Add(New ListItem(objAPTrx.mtdGetDebitNoteStatus(objAPTrx.EnumDebitNoteStatus.Cancelled), objAPTrx.EnumDebitNoteStatus.Cancelled))
        ddlStatus.Items.Add(New ListItem(objAPTrx.mtdGetDebitNoteStatus(objAPTrx.EnumDebitNoteStatus.Closed), objAPTrx.EnumDebitNoteStatus.Closed))
        ddlStatus.Items.Add(New ListItem(objAPTrx.mtdGetDebitNoteStatus(objAPTrx.EnumDebitNoteStatus.Confirmed), objAPTrx.EnumDebitNoteStatus.Confirmed))
        ddlStatus.Items.Add(New ListItem(objAPTrx.mtdGetDebitNoteStatus(objAPTrx.EnumDebitNoteStatus.Deleted), objAPTrx.EnumDebitNoteStatus.Deleted))
        ddlStatus.Items.Add(New ListItem(objAPTrx.mtdGetDebitNoteStatus(objAPTrx.EnumDebitNoteStatus.Paid), objAPTrx.EnumDebitNoteStatus.Paid))
        ddlStatus.Items.Add(New ListItem(objAPTrx.mtdGetDebitNoteStatus(objAPTrx.EnumDebitNoteStatus.Writeoff), objAPTrx.EnumDebitNoteStatus.Writeoff))

        If intLevel = 0 Then
            ddlStatus.SelectedIndex = 1
        Else
            ddlStatus.SelectedIndex = 0
        End If

    End Sub

    Sub BindGrid()
        Dim strOpCode_GetDNList As String = "AP_CLSTRX_DEBITNOTE_LIST_GET" 
        Dim PageNo As Integer
        Dim PageCount As Integer
        Dim lbButton As LinkButton
        Dim strSrchDNID As String
        Dim strSrchDNRefDocID As String
        Dim strSrchSuppCode As String
        Dim strSrchInvoiceRcvRefNo As String
        Dim strSrchStatus As String
        Dim strSrchLastUpdate As String
        Dim strSearch As String
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim lbl As Label

        If lstAccMonth.SelectedItem.Value = "0" Then
            strAccMonth = "1','2','3','4','5','6','7','8','9','10','11','12"
        Else
            strAccMonth = lstAccMonth.SelectedItem.Value
        End If

        strAccYear = lstAccYear.SelectedItem.Value
        strSrchDNID = IIf(txtDNID.Text = "", "", txtDNID.Text)
        strSrchDNRefDocID = IIf(txtDNRefNoID.Text = "", "", txtDNRefNoID.Text)
        strSrchSuppCode = IIf(txtSupplier.Text = "", "", txtSupplier.Text)
        strSrchInvoiceRcvRefNo = IIf(txtInvoiceRcvRefNo.Text = "", "", txtInvoiceRcvRefNo.Text)
        strSrchStatus = IIf(ddlStatus.SelectedItem.Value = "0", "", ddlStatus.SelectedItem.Value)
        strSrchLastUpdate = IIf(txtLastUpdate.Text = "", "", txtLastUpdate.Text)
        strParam = strSrchDNID & "|" & _
                   strSrchDNRefDocID & "|" & _
                   strSrchSuppCode & "|" & _
                   strSrchStatus & "|" & _
                   strSrchLastUpdate & "|" & _
                   SortExpression.Text & "|" & _
                   SortCol.Text & "|" & _
                   strSrchInvoiceRcvRefNo

        Try
            intErrNo = objAPTrx.mtdGetDebitNote(strOpCode_GetDNList, _
                                                strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strAccMonth, _
                                                strAccYear, _
                                                strParam, _
                                                objDebitNoteDs)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=DEBITNOTELIST_GET_LIST&errmesg=" & lblErrMesage.Text & "&redirect=ap/trx/ap_trx_dnlist.aspx")
        End Try

        PageCount = objGlobal.mtdGetPageCount(objDebitNoteDs.Tables(0).Rows.Count, dgDebitNote.PageSize)
        dgDebitNote.DataSource = objDebitNoteDs
        If dgDebitNote.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgDebitNote.CurrentPageIndex = 0
            Else
                dgDebitNote.CurrentPageIndex = PageCount - 1
            End If
        End If
        dgDebitNote.DataBind()
        BindPageList()

        For intCnt = 0 To dgDebitNote.Items.Count - 1
            lbl = dgDebitNote.Items.Item(intCnt).FindControl("lblStatus")
            Select Case CInt(Trim(lbl.Text))
                Case objAPTrx.EnumDebitNoteStatus.Active
                    lbButton = dgDebitNote.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = True
                    lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                Case objAPTrx.EnumDebitNoteStatus.Confirmed, objAPTrx.EnumDebitNoteStatus.Cancelled
                    lbButton = dgDebitNote.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False
                Case objAPTrx.EnumDebitNoteStatus.Deleted
                    lbButton = dgDebitNote.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False
                Case objAPTrx.EnumDebitNoteStatus.Writeoff, objAPTrx.EnumDebitNoteStatus.Paid
                    lbButton = dgDebitNote.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False
            End Select
        Next

        PageNo = dgDebitNote.CurrentPageIndex + 1
        lblTracker.Text = "Page " & PageNo & " of " & dgDebitNote.PageCount
    End Sub

    Sub BindPageList()
        Dim count As Integer = 1
        Dim arrDList As New ArrayList()

        While Not count = dgDebitNote.PageCount + 1
            arrDList.Add("Page " & count)
            count = count + 1
        End While
        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = dgDebitNote.CurrentPageIndex
    End Sub


    Sub Update_Status(ByVal pv_strDebitNoteId As String, _
                      ByVal pv_intDebitNoteSts As Integer)

        Dim strOpCode_DebitNote As String = "AP_CLSTRX_DEBITNOTE_STATUS_UPD"
        Dim strParam As String = pv_strDebitNoteId & "|" & pv_intDebitNoteSts
        Dim intCnt As Integer
        Dim intErrNo As Integer

        Try
            intErrNo = objAPTrx.mtdUpdDebitNoteStatus(strOpCode_DebitNote, _
                                                      strCompany, _
                                                      strLocation, _
                                                      strUserId, _
                                                      strAccMonth, _
                                                      strAccYear, _
                                                      strParam)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=DEBITNOTELIST_UPD_STATUS&errmesg=" & lblErrMesage.Text & "&redirect=ap/trx/ap_trx_dnlist.aspx")
        End Try

    End Sub


    Sub btnPrevNext_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).CommandArgument
        Select Case direction
            Case "first"
                dgDebitNote.CurrentPageIndex = 0
            Case "prev"
                dgDebitNote.CurrentPageIndex = _
                Math.Max(0, dgDebitNote.CurrentPageIndex - 1)
            Case "next"
                dgDebitNote.CurrentPageIndex = _
                Math.Min(dgDebitNote.PageCount - 1, dgDebitNote.CurrentPageIndex + 1)
            Case "last"
                dgDebitNote.CurrentPageIndex = dgDebitNote.PageCount - 1
        End Select
        lstDropList.SelectedIndex = dgDebitNote.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            dgDebitNote.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        dgDebitNote.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIf(SortCol.Text = "ASC", "DESC", "ASC")
        dgDebitNote.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim lblDelText As Label
        Dim strDebitNoteId As String

        dgDebitNote.EditItemIndex = CInt(E.Item.ItemIndex)
        lblDelText = dgDebitNote.Items.Item(CInt(E.Item.ItemIndex)).FindControl("idDNId")
        strDebitNoteId = lblDelText.Text

        Update_Status(strDebitNoteId, objAPTrx.EnumDebitNoteStatus.Deleted)

        BindGrid()
        BindPageList()
    End Sub


    Sub NewDNBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("ap_trx_DNDet.aspx")
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        'STA Group change this into POID to link Detail Mutasi AP report
        'lblInvoiceRcvRefNo.Text = GetCaption(objLangCap.EnumLangCap.InvoiceReceive) & " Ref. No."
        lblInvoiceRcvRefNo.Text = "Purchase Order Ref. No."
        dgDebitNote.Columns(5).HeaderText = lblInvoiceRcvRefNo.Text
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_TRX_DNLIST_GET_LANGCAP&errmesg=" & lblErrMesage.Text & "&redirect=ap/setup/AP_trx_CNList.aspx")
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GET_ACCYEAR&errmesg=" & lblErrMesage.Text & "&redirect=")
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
