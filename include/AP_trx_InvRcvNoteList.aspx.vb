

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


Public Class ap_trx_InvRcvNoteList : Inherits Page

    Protected WithEvents dgInvList As DataGrid
    Protected WithEvents dgInvOst As DataGrid
    Protected WithEvents lblTracker As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents ddlStatus As DropDownList
    Protected WithEvents txtInvoiceID As TextBox
    Protected WithEvents txtSupplier As TextBox
    Protected WithEvents txtLastUpdate As TextBox
    Protected WithEvents ddlInvoiceType As DropDownList
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents lblErrMesage As Label

    Protected WithEvents lblTitle As Label
    Protected WithEvents lblInvReceiveID As Label
    Protected WithEvents lblID As Label
    Protected WithEvents lstAccMonth As DropDownList
    Protected WithEvents lstAccYear As DropDownList
    Protected WithEvents txtPOID As TextBox

    Protected WithEvents NewInvRcv As ImageButton
    Protected WithEvents ibPrint As ImageButton

    Protected objAPTrx As New agri.AP.clsTrx()
    Protected objPUTrx As New agri.PU.clsTrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim objGLtrx As New agri.GL.ClsTrx()
    Dim objOk As New agri.GL.ClsTrx()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intAPAR As Integer
    Dim objInvRcvDs As New Object()
    Dim objLangCapDs As New Object()
    Dim strLocType As String
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
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumAPAccessRights.APInvoiceReceive), intAPAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            'to avoid double click, on aspx add this : UseSubmitBehavior="false"
            NewInvRcv.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(NewInvRcv).ToString())
            ibPrint.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(ibPrint).ToString())

            If SortExpression.Text = "" Then
                SortExpression.Text = "INV.InvoiceRcvID"
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
                BindInvoiceTypeList()
                BindStatusList()
                BindGrid()
                BindGrid_OutStanding()
                BindPageList()
            End If
        End If
    End Sub

    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        dgInvList.CurrentPageIndex = 0
        dgInvList.EditItemIndex = -1
        BindGrid()
        BindGrid_OutStanding()
        BindPageList()
    End Sub

    Sub BindInvoiceTypeList()
        ddlInvoiceType.Items.Add(New ListItem(objAPTrx.mtdGetInvoiceType(objAPTrx.EnumInvoiceType.All), objAPTrx.EnumInvoiceType.All))
        ddlInvoiceType.Items.Add(New ListItem(objAPTrx.mtdGetInvoiceType(objAPTrx.EnumInvoiceType.SupplierPO), objAPTrx.EnumInvoiceType.SupplierPO))
        ddlInvoiceType.Items.Add(New ListItem(objAPTrx.mtdGetInvoiceType(objAPTrx.EnumInvoiceType.AdvancePayment), objAPTrx.EnumInvoiceType.AdvancePayment))
        ddlInvoiceType.Items.Add(New ListItem(objAPTrx.mtdGetInvoiceType(objAPTrx.EnumInvoiceType.Others), objAPTrx.EnumInvoiceType.Others))
        ddlInvoiceType.Items.Add(New ListItem(objAPTrx.mtdGetInvoiceType(objAPTrx.EnumInvoiceType.FFBSupplier), objAPTrx.EnumInvoiceType.FFBSupplier))
        ddlInvoiceType.Items.Add(New ListItem(objAPTrx.mtdGetInvoiceType(objAPTrx.EnumInvoiceType.TransportFee), objAPTrx.EnumInvoiceType.TransportFee))
        ddlInvoiceType.Items.Add(New ListItem(objAPTrx.mtdGetInvoiceType(objAPTrx.EnumInvoiceType.ContractorWorkOrder), objAPTrx.EnumInvoiceType.ContractorWorkOrder))
        ddlStatus.SelectedIndex = 0
    End Sub

    Sub BindStatusList()
        ddlStatus.Items.Add(New ListItem(objAPTrx.mtdGetInvoiceRcvNoteStatus(objAPTrx.EnumInvoiceRcvNoteStatus.All), objAPTrx.EnumInvoiceRcvNoteStatus.All))
        ddlStatus.Items.Add(New ListItem(objAPTrx.mtdGetInvoiceRcvNoteStatus(objAPTrx.EnumInvoiceRcvNoteStatus.Active), objAPTrx.EnumInvoiceRcvNoteStatus.Active))
        ddlStatus.Items.Add(New ListItem(objAPTrx.mtdGetInvoiceRcvNoteStatus(objAPTrx.EnumInvoiceRcvNoteStatus.Cancelled), objAPTrx.EnumInvoiceRcvNoteStatus.Cancelled))
        ddlStatus.Items.Add(New ListItem(objAPTrx.mtdGetInvoiceRcvNoteStatus(objAPTrx.EnumInvoiceRcvNoteStatus.Invoiced), objAPTrx.EnumInvoiceRcvNoteStatus.Invoiced))
        ddlStatus.Items.Add(New ListItem(objAPTrx.mtdGetInvoiceRcvNoteStatus(objAPTrx.EnumInvoiceRcvNoteStatus.Deleted), objAPTrx.EnumInvoiceRcvNoteStatus.Deleted))
        If intLevel = 0 Then
            ddlStatus.SelectedIndex = 1
        Else
            ddlStatus.SelectedIndex = 0
        End If
    End Sub

    Sub BindGrid_OutStanding()
        Dim PageCount As Integer
        Dim dsData As DataSet
        Dim intCnt As Integer = 0
        Dim lbl As Label

        dsData = LoadData_OutStanding()
        PageCount = objGlobal.mtdGetPageCount(dsData.Tables(0).Rows.Count, dgInvOst.PageSize)

        dgInvOst.DataSource = dsData
        If dgInvOst.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgInvOst.CurrentPageIndex = 0
            Else
                dgInvOst.CurrentPageIndex = PageCount - 1
            End If
        End If

        dgInvOst.DataBind()

        For intCnt = 0 To dgInvOst.Items.Count - 1
            lbl = dgInvOst.Items.Item(intCnt).FindControl("lblNo")
            lbl.Text = intCnt + 1
        Next
    End Sub

    Protected Function LoadData_OutStanding() As DataSet
        Dim strParamName As String
        Dim strParamValue As String
        Dim objOst As New DataSet()


        Dim strOppCode_Get As String = "PU_CLSTRX_PO_INVOICERECEIVE_OUTSTANDING_GET"
        Dim intErrNo As Integer
 
        strParamName = "STRSEARCH"
        strParamValue = "AND SUPP.Name LIKE '%" & txtSupplier.Text & _
                    "%' AND A.POID LIKE '%" & txtPOID.Text & _
                    "%'  AND ((GR.AccMonth='" & lstAccMonth.SelectedItem.Value & "')OR ('" & lstAccMonth.SelectedItem.Value & "'=0)) AND GR.AccYear='" & lstAccYear.SelectedItem.Value & "'"

        Try
            intErrNo = objOk.mtdGetDataCommon(strOppCode_Get, strParamName, strParamValue, objOst)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=wm_ticket&errmesg=" & Exp.Message & "&redirect=")
        End Try

        dgInvOst.DataSource = objOst
        dgInvOst.DataBind()

        Return objOst
    End Function

    Sub BindGrid()
        Dim strOpCode_GetInvRcvList As String = "AP_CLSTRX_INVOICERECEIVENOTE_LIST_GET"
        Dim PageNo As Integer
        Dim PageCount As Integer
        Dim strSrchInvID As String
        Dim strSrchSuppCode As String
        Dim strSrchInvoiceType As String
        Dim strSrchStatus As String
        Dim strSrchLastUpdate As String
        Dim strSearch As String
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim lbl As Label
        Dim lbButton As LinkButton
        Dim strPOID As String
        Dim strParamName As String
        Dim strParamValue As String
        'Dim strSearch As String

        If lstAccMonth.SelectedItem.Value = "0" Then
            strAccMonth = "1','2','3','4','5','6','7','8','9','10','11','12"
        Else
            strAccMonth = lstAccMonth.SelectedItem.Value
        End If

        strAccYear = lstAccYear.SelectedItem.Value
        strSrchInvID = IIf(txtInvoiceID.Text = "", "", txtInvoiceID.Text)
        strSrchSuppCode = IIf(txtSupplier.Text = "", "", txtSupplier.Text)
        strSrchInvoiceType = IIf(ddlInvoiceType.SelectedIndex = 0, "", ddlInvoiceType.SelectedItem.Value)
        strSrchStatus = IIf(ddlStatus.SelectedItem.Value = "0", "", ddlStatus.SelectedItem.Value)
        strSrchLastUpdate = IIf(txtLastUpdate.Text = "", "", txtLastUpdate.Text)
        strPOID = IIf(txtPOID.Text = "", "", txtPOID.Text) 

        'strParam = strSrchInvID & "|" & _
        '           strSrchSuppCode & "|" & _
        '           strSrchStatus & "|" & _
        '           strSrchLastUpdate & "|" & _
        '           SortExpression.Text & "|" & _
        '           SortCol.Text & "|" & _
        '           strSrchInvoiceType & "|" & _
        '           strPOID

        'Try
        '    intErrNo = objAPTrx.mtdGetInvoiceRcv(strOpCode_GetInvRcvList, _
        '                                         strCompany, _
        '                                         strLocation, _
        '                                         strUserId, _
        '                                         strAccMonth, _
        '                                         strAccYear, _
        '                                         strParam, _
        '                                         objInvRcvDs)
        'Catch Exp As System.Exception
        '    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=INVRCVLIST_GET_LIST&errmesg=" & lblErrMesage.Text & "&redirect=AP/trx/ap_trx_InvRcvList.aspx")
        'End Try


        strSearch = IIf(Trim(txtInvoiceID.Text) = "", "", "INV.InvoiceRcvID Like '%" & Trim(txtInvoiceID.Text) & "%' AND ")
        strSearch = strSearch + IIf(ddlInvoiceType.SelectedIndex = 0, "", "INV.InvoiceType = '" & ddlInvoiceType.SelectedIndex & "' AND ")
        strSearch = strSearch + IIf(Trim(txtSupplier.Text) = "", "", "(SUPP.SupplierCode LIKE '" & Trim(txtSupplier.Text) & "%' or SUPP.Name LIKE '%" & Trim(txtSupplier.Text) & "%') AND ")
        strSearch = strSearch + IIf(Trim(txtPOID.Text) = "", "", "LN.POID Like '%" & Trim(txtPOID.Text) & "%' AND ")
        strSearch = strSearch + IIf(ddlStatus.SelectedItem.Value = "0", "", "INV.Status = '" & Trim(ddlStatus.SelectedItem.Value) & "' AND ")
        strSearch = strSearch + IIf(Trim(txtLastUpdate.Text) = "", "", "USR.UserName LIKE '" & Trim(txtLastUpdate.Text) & "%' AND ")
        
        If (Trim(strSearch) <> "") Then
            strSearch = " AND " & Mid(strSearch, 1, Len(strSearch) - 4) & " "
        End If

        strSearch = strSearch + IIf(Trim(SortExpression.Text) = "", "", "Order By " & Trim(SortExpression.Text) & " ")
        strSearch = strSearch + IIf(Trim(SortCol.Text) = "", "ASC ", Trim(SortCol.Text))

        strParamName = "LOCCODE|ACCMONTH|ACCYEAR|STRSEARCH"
        strParamValue = strLocation & "|" & strAccMonth & "|" & strAccYear & "|" & strSearch

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCode_GetInvRcvList, _
                                                strParamName, _
                                                strParamValue, _
                                                objInvRcvDs)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GET_ACCYEAR&errmesg=" & lblErrMesage.Text & "&redirect=")
        End Try

        PageCount = objGlobal.mtdGetPageCount(objInvRcvDs.Tables(0).Rows.Count, dgInvList.PageSize)
        dgInvList.DataSource = objInvRcvDs
        If dgInvList.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgInvList.CurrentPageIndex = 0
            Else
                dgInvList.CurrentPageIndex = PageCount - 1
            End If
        End If
        dgInvList.DataBind()
        BindPageList()

        For intCnt = 0 To dgInvList.Items.Count - 1
            lbl = dgInvList.Items.Item(intCnt).FindControl("lblStatus")

            Select Case CInt(Trim(lbl.Text))
                Case objAPTrx.EnumInvoiceRcvNoteStatus.Active
                    lbButton = dgInvList.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False 'True
                    lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                Case objAPTrx.EnumInvoiceRcvNoteStatus.Invoiced, _
                     objAPTrx.EnumInvoiceRcvNoteStatus.Deleted
                    lbButton = dgInvList.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False
            End Select
        Next

        PageNo = dgInvList.CurrentPageIndex + 1
        lblTracker.Text = "Page " & PageNo & " of " & dgInvList.PageCount
    End Sub

    Sub BindPageList()
        Dim count As Integer = 1
        Dim arrDList As New ArrayList()

        While Not count = dgInvList.PageCount + 1
            arrDList.Add("Page " & count)
            count = count + 1
        End While
        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = dgInvList.CurrentPageIndex
    End Sub

    Sub Update_Status(ByVal pv_strInvoiceRcvId As String, _
                      ByVal pv_intInvoiceRcvSts As Integer)

        Dim strOpCode_InvoiceRcv As String = "AP_CLSTRX_INVOICERECEIVENOTE_STATUS_UPD"
        Dim intErrNo As Integer
        Dim strParamName As String
        Dim strParamValue As String

        strParamName = "STATUS|UPDATEDATE|UPDATEID|INVOICERCVID"
        strParamValue = pv_intInvoiceRcvSts & "|" & Now() & "|" & strUserId & "|" & Trim(pv_strInvoiceRcvId)

        Try
            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCode_InvoiceRcv, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=INVRCVLIST_UPD_STATUS&errmesg=" & lblErrMesage.Text & "&redirect=AP/trx/ap_trx_InvRcvNoteList.aspx")
        End Try
    End Sub

    Sub btnPrevNext_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).CommandArgument
        Select Case direction
            Case "first"
                dgInvList.CurrentPageIndex = 0
            Case "prev"
                dgInvList.CurrentPageIndex = _
                Math.Max(0, dgInvList.CurrentPageIndex - 1)
            Case "next"
                dgInvList.CurrentPageIndex = _
                Math.Min(dgInvList.PageCount - 1, dgInvList.CurrentPageIndex + 1)
            Case "last"
                dgInvList.CurrentPageIndex = dgInvList.PageCount - 1
        End Select
        lstDropList.SelectedIndex = dgInvList.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            dgInvList.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        dgInvList.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIf(SortCol.Text = "ASC", "DESC", "ASC")
        dgInvList.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim lblDelText As Label
        Dim strInvoiceRcvId As String
        Dim strPOID As String

        dgInvList.EditItemIndex = CInt(E.Item.ItemIndex)
        lblDelText = dgInvList.Items.Item(CInt(E.Item.ItemIndex)).FindControl("idInvoiceRcvId")
        strInvoiceRcvId = lblDelText.Text

        Update_Status(strInvoiceRcvId, objAPTrx.EnumInvoiceRcvNoteStatus.Deleted)

        BindGrid()
        BindPageList()
    End Sub

    Sub EmpLink_Click(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Session("VIEW") = ""
        Session("VIEW") = "LOOKUP"
        If Not E.CommandName.ToString = "Sort" And Not E.CommandName.ToString = "Delete" Then
            Dim lbl As Label
            Dim intIndex As Integer = E.Item.ItemIndex
            Dim strSuppCode As String = ""
            Dim strItemCode As String = ""
            Dim strService As String = ""


            lbl = dgInvOst.Items.Item(intIndex).FindControl("lblEmpCode")
            Response.Write("<Script Language=""JavaScript"">pop_Att=window.open(""../../PU/trx/PU_trx_PODet.aspx?redirect=attm&Poid=" & lbl.Text.Trim & """, null ,""'pop_Att',width=1000,height=500,top=100,left=100,status=yes, resizable=no, scrollbars=yes, toolbar=no, location=no"");pop_Att.focus();</Script>")

            'Response.Write("<Script Language=""JavaScript"">window.open(""../../PU/reports/PU_Rpt_POPFHistoryDet.aspx?Type=Print&CompName=" & strCompany & _
            '                       "&Location=" & strLocation & _
            '                       "&RptId=" & "" & _
            '                       "&RptName=" & "" & _
            '                       "&Decimal=" & 0 & _
            '                       "&PONoFrom=" & lbl.Text & _
            '                       "&PONoTo=" & "" & _
            '                       "&PeriodeFrom=" & "" & _
            '                       "&PeriodeTo=" & "" & _
            '                       "&PIC1=" & _
            '                       "&Jabatan1=" & _
            '                       "&PIC2=" & _
            '                       "&Jabatan2=" & _
            '                       "&Catatan=" & _
            '                       "&Lokasi=" & _
            '                       "&SuppCode=" & strSuppCode & _
            '                       "&ItemCode=" & strItemCode & _
            '                       "&Service=" & strService & _
            '                       "&SentPeriod=" & _
            '                       """,null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")
        Else
            Dim lbl2 As Label
            Dim intIndex2 As Integer = E.Item.ItemIndex

            lbl2 = dgInvOst.Items.Item(intIndex2).FindControl("lblGRID")
            Response.Write("<Script Language=""JavaScript"">pop_Att=window.open(""../../PU/trx/PU_trx_GRDet.aspx?redirect=attm&GoodsRcvId=" & lbl2.Text.Trim & """, null ,""'pop_Att',width=1000,height=500,top=100,left=100,status=yes, resizable=no, scrollbars=yes, toolbar=no, location=no"");pop_Att.focus();</Script>")
        End If
    End Sub

    Sub NewInvRcv_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("ap_trx_InvRcvNoteDet.aspx")
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblTitle.Text = UCase("Invoice Reception/Tanda Terima Tagihan")
        lblInvReceiveID.Text = "Invoice Reception" & lblID.Text
        dgInvList.Columns(0).HeaderText = "Invoice Reception" & lblID.Text
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_TRX_INVRCV_LANGCAP&errmesg=" & lblErrMesage.Text & "&redirect=")
        End Try

    End Sub

    Function GetCaption(ByVal pv_TermCode) As String
        Dim count As Integer

        For count = 0 To objLangCapDs.Tables(0).Rows.Count - 1
            If Trim(pv_TermCode) = Trim(objLangCapDs.Tables(0).Rows(count).Item("TermCode")) Then
                If strLocType = objAdminLoc.EnumLocType.Mill Then
                    Return Trim(objLangCapDs.Tables(0).Rows(count).Item("BusinessTermMW"))
                Else
                    Return Trim(objLangCapDs.Tables(0).Rows(count).Item("BusinessTerm"))
                End If
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
