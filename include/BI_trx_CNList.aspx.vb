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


Public Class BI_trx_CNList : Inherits Page

    Protected WithEvents dgLine As DataGrid
    Protected WithEvents lblTracker As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents txtCNID As TextBox
    Protected WithEvents txtBillParty As TextBox
    Protected WithEvents ddlCreditNoteType As DropDownList
    Protected WithEvents ddlStatus As DropDownList
    Protected WithEvents txtLastUpdate As TextBox
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents lblErrMesage As Label
    Protected WithEvents lblBillParty As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents lstAccMonth As DropDownList
    Protected WithEvents lstAccYear As DropDownList

    Protected objBITrx As New agri.BI.clsTrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objGLtrx As New agri.GL.ClsTrx()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intBIAR As Integer
    Dim objCreditNoteDs As New Object()
    Dim objLangCapDs As New Object()
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim strLocType As String
    Dim strSelAccMonth As String
    Dim strSelAccYear As String

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strAccMonth = Session("SS_ARACCMONTH")
        strAccYear = Session("SS_ARACCYEAR")
        strLangCode = Session("SS_LANGCODE")
        intBIAR = Session("SS_BIAR")
        strLocType = Session("SS_LOCTYPE")
        strSelAccMonth = Session("SS_SELACCMONTH")
        strSelAccYear = Session("SS_SELACCYEAR")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumBIAccessRights.BICreditNote), intBIAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            If SortExpression.Text = "" Then
                SortExpression.Text = "CN.CreditNoteID"
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
                BindStatusList()
                BindCreditNoteType()
                BindGrid()
                BindPageList()
            End If
        End If
    End Sub

    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        dgLine.CurrentPageIndex = 0
        dgLine.EditItemIndex = -1
        BindGrid()
        BindPageList()
    End Sub

    Sub BindCreditNoteType()
        ddlCreditNoteType.Items.Add(New ListItem("All", ""))
        ddlCreditNoteType.Items.Add(New ListItem(objBITrx.mtdGetCreditNoteDocType(objBITrx.EnumCreditNoteDocType.Auto_Millware), objBITrx.EnumCreditNoteDocType.Auto_Millware))
        ddlCreditNoteType.Items.Add(New ListItem(objBITrx.mtdGetCreditNoteDocType(objBITrx.EnumCreditNoteDocType.Auto_WSJob_Staff), objBITrx.EnumCreditNoteDocType.Auto_WSJob_Staff))
        ddlCreditNoteType.Items.Add(New ListItem(objBITrx.mtdGetCreditNoteDocType(objBITrx.EnumCreditNoteDocType.Auto_WSJob_External), objBITrx.EnumCreditNoteDocType.Auto_WSJob_External))
        ddlCreditNoteType.Items.Add(New ListItem(objBITrx.mtdGetCreditNoteDocType(objBITrx.EnumCreditNoteDocType.Auto_WSJob_Internal), objBITrx.EnumCreditNoteDocType.Auto_WSJob_Internal)) 
        ddlCreditNoteType.Items.Add(New ListItem(objBITrx.mtdGetCreditNoteDocType(objBITrx.EnumCreditNoteDocType.Auto_GLJournal), objBITrx.EnumCreditNoteDocType.Auto_GLJournal)) 
        ddlCreditNoteType.Items.Add(New ListItem(objBITrx.mtdGetCreditNoteDocType(objBITrx.EnumCreditNoteDocType.Manual), objBITrx.EnumCreditNoteDocType.Manual))
        ddlCreditNoteType.Items.Add(New ListItem(objBITrx.mtdGetCreditNoteDocType(objBITrx.EnumCreditNoteDocType.Manual_Millware), objBITrx.EnumCreditNoteDocType.Manual_Millware))
    End Sub

    Sub BindStatusList()
        ddlStatus.Items.Add(New ListItem(objBITrx.mtdGetCreditNoteStatus(objBITrx.EnumCreditNoteStatus.All), objBITrx.EnumCreditNoteStatus.All))
        ddlStatus.Items.Add(New ListItem(objBITrx.mtdGetCreditNoteStatus(objBITrx.EnumCreditNoteStatus.Active), objBITrx.EnumCreditNoteStatus.Active))
        ddlStatus.Items.Add(New ListItem(objBITrx.mtdGetCreditNoteStatus(objBITrx.EnumCreditNoteStatus.Cancelled), objBITrx.EnumCreditNoteStatus.Cancelled))
        ddlStatus.Items.Add(New ListItem(objBITrx.mtdGetCreditNoteStatus(objBITrx.EnumCreditNoteStatus.Closed), objBITrx.EnumCreditNoteStatus.Closed))
        ddlStatus.Items.Add(New ListItem(objBITrx.mtdGetCreditNoteStatus(objBITrx.EnumCreditNoteStatus.Confirmed), objBITrx.EnumCreditNoteStatus.Confirmed))
        ddlStatus.Items.Add(New ListItem(objBITrx.mtdGetCreditNoteStatus(objBITrx.EnumCreditNoteStatus.Deleted), objBITrx.EnumCreditNoteStatus.Deleted))
        ddlStatus.Items.Add(New ListItem(objBITrx.mtdGetCreditNoteStatus(objBITrx.EnumCreditNoteStatus.WrittenOff), objBITrx.EnumCreditNoteStatus.WrittenOff))
        ddlStatus.SelectedIndex = 1

    End Sub

    Sub BindGrid()
        Dim PageNo As Integer
        Dim PageCount As Integer
        Dim strOpCode_GetCNList As String = "BI_CLSTRX_CREDITNOTE_LIST_GET"
        Dim lbButton As LinkButton
        Dim strSrchCNID As String
        Dim strBillParty As String
        Dim strCreditNoteType As String
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
        strSrchCNID = IIf(txtCNID.Text = "", "", txtCNID.Text)
        strBillParty = IIf(txtBillParty.Text = "", "", txtBillParty.Text)
        strCreditNoteType = IIf(ddlCreditNoteType.SelectedItem.Value = "", "", ddlCreditNoteType.SelectedItem.Value)
        strSrchStatus = IIf(ddlStatus.SelectedItem.Value = "0", "", ddlStatus.SelectedItem.Value)
        strSrchLastUpdate = IIf(txtLastUpdate.Text = "", "", txtLastUpdate.Text)
        strParam = strSrchCNID & "|" & _
                   strBillParty & "|" & _
                   strCreditNoteType & "|" & _
                   strSrchStatus & "|" & _
                   strSrchLastUpdate & "|" & _
                   SortExpression.Text & "|" & _
                   SortCol.Text

        Try
            intErrNo = objBITrx.mtdGetCreditNote(strOpCode_GetCNList, _
                                                strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strAccMonth, _
                                                strAccYear, _
                                                strParam, _
                                                objCreditNoteDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_CREDITNOTELIST_GET_LIST&errmesg=" & lblErrMesage.Text & "&redirect=")
        End Try

        For intCnt = 0 To objCreditNoteDs.Tables(0).Rows.Count - 1
            objCreditNoteDs.Tables(0).Rows(intCnt).Item("CreditNoteID") = Trim(objCreditNoteDs.Tables(0).Rows(intCnt).Item("CreditNoteID"))
            objCreditNoteDs.Tables(0).Rows(intCnt).Item("Remark") = Trim(objCreditNoteDs.Tables(0).Rows(intCnt).Item("Remark"))
            objCreditNoteDs.Tables(0).Rows(intCnt).Item("Status") = Trim(objCreditNoteDs.Tables(0).Rows(intCnt).Item("Status"))
            objCreditNoteDs.Tables(0).Rows(intCnt).Item("UpdateID") = Trim(objCreditNoteDs.Tables(0).Rows(intCnt).Item("UpdateID"))
        Next

        PageCount = objGlobal.mtdGetPageCount(objCreditNoteDs.Tables(0).Rows.Count, dgLine.PageSize)
        dgLine.DataSource = objCreditNoteDs
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
                Case objBITrx.EnumCreditNoteStatus.Active
                    lbButton = dgLine.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = True
                    lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                Case objBITrx.EnumCreditNoteStatus.Deleted
                    lbButton = dgLine.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Text = "Undelete"
                    lbButton.Visible = True
                    lbButton.Attributes("onclick") = "javascript:return ConfirmAction('undelete');"
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


    Sub Update_Status(ByVal pv_strCreditNoteId As String, _
                      ByVal pv_intCreditNoteSts As Integer)

        Dim strOpCode_CreditNote As String = "BI_CLSTRX_CREDITNOTE_STATUS_UPD"
        Dim strParam As String = pv_strCreditNoteId & "|" & pv_intCreditNoteSts
        Dim intCnt As Integer
        Dim intErrNo As Integer

        Try
            intErrNo = objBITrx.mtdUpdCreditNoteStatus(strOpCode_CreditNote, _
                                                      strCompany, _
                                                      strLocation, _
                                                      strUserId, _
                                                      strAccMonth, _
                                                      strAccYear, _
                                                      strParam)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_CREDITNOTELIST_UPD_STATUS&errmesg=" & lblErrMesage.Text & "&redirect=BI/trx/BI_trx_CNList.aspx")
        End Try
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
        Dim lbl As Label
        Dim strCreditNoteId As String
        Dim strStatus As String

        dgLine.EditItemIndex = CInt(E.Item.ItemIndex)
        lbl = dgLine.Items.Item(CInt(E.Item.ItemIndex)).FindControl("idCNId")
        strCreditNoteId = lbl.Text.Trim()
        lbl = dgLine.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lblStatus")
        strStatus = lbl.Text.Trim()

        Select Case strStatus
            Case objBITrx.EnumCreditNoteStatus.Active
                Update_Status(strCreditNoteId, objBITrx.EnumCreditNoteStatus.Deleted)
            Case objBITrx.EnumCreditNoteStatus.Deleted
                 Update_Status(strCreditNoteId, objBITrx.EnumCreditNoteStatus.Active)
        End Select 
        BindGrid()
        BindPageList()
    End Sub


    Sub NewCNBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("BI_trx_CNDet.aspx")
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblBillParty.Text = GetCaption(objLangCap.EnumLangCap.BillParty) & lblCode.Text
        dgLine.Columns(2).HeaderText = lblBillParty.Text
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_CLSTRX_CREDITNOTE_LIST_GET_LANGCAP&errmesg=" & lblErrMesage.Text & "&redirect=BI/trx/BI_trx_CNList.aspx")
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
