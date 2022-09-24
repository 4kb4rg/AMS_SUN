
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


Public Class ap_trx_CJList : Inherits Page

    Protected WithEvents dgCreditorJournal As DataGrid
    Protected WithEvents lblTracker As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents ddlStatus As DropDownList
    Protected WithEvents txtCJID As TextBox
    Protected WithEvents txtSupplier As TextBox
    Protected WithEvents txtLastUpdate As TextBox
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents lblErrMesage As Label
    Protected WithEvents lstAccMonth As DropDownList
    Protected WithEvents lstAccYear As DropDownList
    Protected WithEvents NewCJBtn As ImageButton
    Protected WithEvents ibPrint As ImageButton

    Protected objAPTrx As New agri.AP.clsTrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objGLtrx As New agri.GL.ClsTrx()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intAPAR As Integer
    Dim strSelAccMonth As String
    Dim strSelAccYear As String
    Dim intLevel As Integer
    Dim objCreditorJournalDs As New Object()

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
        intLevel = Session("SS_USRLEVEL")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumAPAccessRights.APCreditorJournal), intAPAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            'to avoid double click, on aspx add this : UseSubmitBehavior="false"
            NewCJBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(NewCJBtn).ToString())
            'ibPrint.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(ibPrint).ToString())

            If SortExpression.Text = "" Then
                SortExpression.Text = "CJ.CreditJrnID"
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
        dgCreditorJournal.CurrentPageIndex = 0
        dgCreditorJournal.EditItemIndex = -1
        BindGrid()
        BindPageList()
    End Sub

    Sub BindStatusList()
        ddlStatus.Items.Add(New ListItem(objAPTrx.mtdGetCreditorJournalStatus(objAPTrx.EnumCreditorJournalStatus.All), objAPTrx.EnumCreditorJournalStatus.All))
        ddlStatus.Items.Add(New ListItem(objAPTrx.mtdGetCreditorJournalStatus(objAPTrx.EnumCreditorJournalStatus.Active), objAPTrx.EnumCreditorJournalStatus.Active))
        ddlStatus.Items.Add(New ListItem(objAPTrx.mtdGetCreditorJournalStatus(objAPTrx.EnumCreditorJournalStatus.Cancelled), objAPTrx.EnumCreditorJournalStatus.Cancelled))
        ddlStatus.Items.Add(New ListItem(objAPTrx.mtdGetCreditorJournalStatus(objAPTrx.EnumCreditorJournalStatus.Closed), objAPTrx.EnumCreditorJournalStatus.Closed))
        ddlStatus.Items.Add(New ListItem(objAPTrx.mtdGetCreditorJournalStatus(objAPTrx.EnumCreditorJournalStatus.Confirmed), objAPTrx.EnumCreditorJournalStatus.Confirmed))
        ddlStatus.Items.Add(New ListItem(objAPTrx.mtdGetCreditorJournalStatus(objAPTrx.EnumCreditorJournalStatus.Deleted), objAPTrx.EnumCreditorJournalStatus.Deleted))

        If intLevel = 0 Then
            ddlStatus.SelectedIndex = 1
        Else
            ddlStatus.SelectedIndex = 0
        End If

    End Sub

    Sub BindGrid()
        Dim strOpCode_GetCJList As String = "AP_CLSTRX_CREDITORJOURNAL_LIST_GET" 
        Dim PageNo As Integer
        Dim PageCount As Integer
        Dim lbButton As LinkButton
        Dim strSrchCJID As String
        Dim strSrchSuppCode As String
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
        strSrchCJID = IIf(txtCJID.Text = "", "", txtCJID.Text)
        strSrchSuppCode = IIf(txtSupplier.Text = "", "", txtSupplier.Text)
        strSrchStatus = IIf(ddlStatus.SelectedItem.Value = "0", "", ddlStatus.SelectedItem.Value)
        strSrchLastUpdate = IIf(txtLastUpdate.Text = "", "", txtLastUpdate.Text)
        strParam = strSrchCJID & "|" & _
                   strSrchSuppCode & "|" & _
                   strSrchStatus & "|" & _
                   strSrchLastUpdate & "|" & _
                   SortExpression.Text & "|" & _
                   SortCol.Text

        Try
            intErrNo = objAPTrx.mtdGetCreditorJournal(strOpCode_GetCJList, _
                                                      strCompany, _
                                                      strLocation, _
                                                      strUserId, _
                                                      strAccMonth, _
                                                      strAccYear, _
                                                      strParam, _
                                                      objCreditorJournalDs)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CREDITJRNLIST_GET_LIST&errmesg=" & lblErrMesage.Text & "&redirect=ap/trx/ap_trx_cjlist.aspx")
        End Try

        PageCount = objGlobal.mtdGetPageCount(objCreditorJournalDs.Tables(0).Rows.Count, dgCreditorJournal.PageSize)
        dgCreditorJournal.DataSource = objCreditorJournalDs
        If dgCreditorJournal.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgCreditorJournal.CurrentPageIndex = 0
            Else
                dgCreditorJournal.CurrentPageIndex = PageCount - 1
            End If
        End If
        dgCreditorJournal.DataBind()
        BindPageList()

        For intCnt = 0 To dgCreditorJournal.Items.Count - 1
            lbl = dgCreditorJournal.Items.Item(intCnt).FindControl("lblStatus")
            Select Case CInt(Trim(lbl.Text))
                Case objAPTrx.EnumCreditorJournalStatus.Active
                    lbButton = dgCreditorJournal.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = True
                    lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                Case objAPTrx.EnumCreditorJournalStatus.Confirmed, objAPTrx.EnumCreditorJournalStatus.Cancelled
                    lbButton = dgCreditorJournal.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False
                Case objAPTrx.EnumCreditorJournalStatus.Deleted
                    lbButton = dgCreditorJournal.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False
            End Select
        Next

        PageNo = dgCreditorJournal.CurrentPageIndex + 1
        lblTracker.Text = "Page " & PageNo & " of " & dgCreditorJournal.PageCount
    End Sub

    Sub BindPageList()
        Dim count As Integer = 1
        Dim arrDList As New ArrayList()

        While Not count = dgCreditorJournal.PageCount + 1
            arrDList.Add("Page " & count)
            count = count + 1
        End While
        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = dgCreditorJournal.CurrentPageIndex
    End Sub


    Sub Update_Status(ByVal pv_strCreditJrnId As String, _
                      ByVal pv_intCreditJrnSts As Integer)

        Dim strOpCode_CreditJrn As String = "AP_CLSTRX_CREDITORJOURNAL_STATUS_UPD"
        Dim strParam As String = pv_strCreditJrnId & "|" & pv_intCreditJrnSts & "|0" 
        Dim intCnt As Integer
        Dim intErrNo As Integer

        Try
            intErrNo = objAPTrx.mtdUpdCreditorJournalStatus(strOpCode_CreditJrn, _
                                                            strCompany, _
                                                            strLocation, _
                                                            strUserId, _
                                                            strAccMonth, _
                                                            strAccYear, _
                                                            strParam)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CREDITJRNLIST_UPD_STATUS&errmesg=" & lblErrMesage.Text & "&redirect=ap/trx/ap_trx_cjlist.aspx")
        End Try
    End Sub


    Sub btnPrevNext_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).CommandArgument
        Select Case direction
            Case "first"
                dgCreditorJournal.CurrentPageIndex = 0
            Case "prev"
                dgCreditorJournal.CurrentPageIndex = _
                Math.Max(0, dgCreditorJournal.CurrentPageIndex - 1)
            Case "next"
                dgCreditorJournal.CurrentPageIndex = _
                Math.Min(dgCreditorJournal.PageCount - 1, dgCreditorJournal.CurrentPageIndex + 1)
            Case "last"
                dgCreditorJournal.CurrentPageIndex = dgCreditorJournal.PageCount - 1
        End Select
        lstDropList.SelectedIndex = dgCreditorJournal.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            dgCreditorJournal.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        dgCreditorJournal.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIf(SortCol.Text = "ASC", "DESC", "ASC")
        dgCreditorJournal.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim lblDelText As Label
        Dim strCreditNoteId As String

        dgCreditorJournal.EditItemIndex = CInt(E.Item.ItemIndex)
        lblDelText = dgCreditorJournal.Items.Item(CInt(E.Item.ItemIndex)).FindControl("idCJId")
        strCreditNoteId = lblDelText.Text

        Update_Status(strCreditNoteId, objAPTrx.EnumCreditorJournalStatus.Deleted)

        BindGrid()
        BindPageList()
    End Sub


    Sub NewCJBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("ap_trx_CJDet.aspx")
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GET_ACCYEAR&errmesg=" & lblErrMesage.Text)
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
End Class
