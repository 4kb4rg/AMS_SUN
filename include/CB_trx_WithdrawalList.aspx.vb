
Imports System
Imports System.Data

Public Class CB_trx_WithdrawalList : Inherits Page

    Protected WithEvents lblErrMesage As Label
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label

    Protected WithEvents dgWithdrawal As DataGrid
    Protected WithEvents lblTracker As Label
    Protected WithEvents lstDropList As DropDownList

    Protected WithEvents ddlStatus  As DropDownList
    Protected WithEvents txtWdrCode As TextBox
    Protected WithEvents txtDesc As TextBox
    Protected WithEvents txtDepCode As TextBox
    Protected WithEvents txtBilyetNo As TextBox
    Protected WithEvents txtLastUpdate As TextBox
	

    Protected objCBTrx As New agri.CB.clsTrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objAdminLoc As New agri.Admin.clsLoc()

    
    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intCBAR As Integer

    Dim objWithdrawalDs As New Object()
    Dim strLocType as String

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strAccMonth = Session("SS_CBACCMONTH")
        strAccYear = Session("SS_CBACCYEAR")
        strLangCode = Session("SS_LANGCODE")
        intCBAR = Session("SS_CBAR")
        strLocType = Session("SS_LOCTYPE")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCBAccessRights.CBWithdrawal), intCBAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            If SortExpression.Text = "" Then
                 SortExpression.Text = "WDR.WithdrawalCode"
            End If

            If Not Page.IsPostBack Then
                BindStatusList()
                BindGrid()
                BindPageList()
            End If
        End If
    End Sub

    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        dgWithdrawal.CurrentPageIndex = 0
        dgWithdrawal.EditItemIndex = -1
        BindGrid()
        BindPageList()
    End Sub

    Sub BindStatusList()
        
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetWithdrawalStatus(objCBTrx.EnumWithdrawalStatus.All), objCBTrx.EnumWithdrawalStatus.All))
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetWithdrawalStatus(objCBTrx.EnumWithdrawalStatus.Active), objCBTrx.EnumWithdrawalStatus.Active))
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetWithdrawalStatus(objCBTrx.EnumWithdrawalStatus.Cancelled), objCBTrx.EnumWithdrawalStatus.Cancelled))
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetWithdrawalStatus(objCBTrx.EnumWithdrawalStatus.Confirmed), objCBTrx.EnumWithdrawalStatus.Confirmed))
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetWithdrawalStatus(objCBTrx.EnumWithdrawalStatus.Deleted), objCBTrx.EnumWithdrawalStatus.Deleted))
        ddlStatus.SelectedIndex = 1

    End Sub

    Sub BindGrid()
     
        Dim strOpCode_GetWdrList As String = "CB_CLSTRX_WITHDRAWAL_LIST_GET" 
        Dim PageNo As Integer
        Dim PageCount As Integer
        Dim lbButton As LinkButton
        
        Dim strSrchWdrCode As String
        Dim strSrchDesc As String
        Dim strSrchDepCode As String
        Dim strSrchBilyetNo As String
        Dim strSrchStatus As String
        dim strSrchLastUpdate As String
        Dim strSearch As String
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim lbl As Label

        strSrchWdrCode = IIf(txtWdrCode.Text = "", "", txtWdrCode.Text)
        strSrchDesc  = IIf(txtDesc.Text = "", "", txtDesc.Text)
        strSrchDepCode  = IIf(txtDepCode.Text = "", "", txtDepCode.Text)
        strSrchBilyetNo = IIf(txtBilyetNo.Text = "", "", txtBilyetNo.Text)
        strSrchStatus = IIf(ddlStatus.SelectedItem.Value = "0", "", ddlStatus.SelectedItem.Value)
        strSrchLastUpdate = IIf(txtLastUpdate.Text = "", "", txtLastUpdate.Text)

        strParam = strSrchWdrCode & "|" & _
                   strSrchDesc & "|" & _
                   strSrchDepCode & "|" & _
                   strSrchBilyetNo & "|" & _
                   strSrchStatus & "|" & _
                   strSrchLastUpdate & "|" & _
                   SortExpression.text & "|" & _
                   SortCol.Text
          
        Try
            intErrNo = objCBTrx.mtdGetWithdrawal(strOpCode_GetWdrList, _
                                              strCompany, _
                                              strLocation, _
                                              strUserId, _
                                              strAccMonth, _
                                              strAccYear, _
                                              strParam, _
                                              objWithdrawalDs)
        Catch Exp As Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=WITHDRAWAL_GET_LIST&errmesg=" & Exp.Message & "&redirect=CB/trx/CB_trx_WithdrawalList.aspx")
        End Try

        PageCount = objGlobal.mtdGetPageCount(objWithdrawalDs.Tables(0).Rows.Count, dgWithdrawal.PageSize)
        dgWithdrawal.DataSource = objWithdrawalDs
        If dgWithdrawal.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgWithdrawal.CurrentPageIndex = 0
            Else
                dgWithdrawal.CurrentPageIndex = PageCount - 1
            End If
        End If
        dgWithdrawal.DataBind()
        BindPageList()

        For intCnt = 0 To dgWithdrawal.Items.Count - 1
            lbl = dgWithdrawal.Items.Item(intCnt).FindControl("lblStatus")
            
             Select Case CInt(Trim(lbl.Text))
                Case objCBTrx.EnumWithdrawalStatus.Active
                    lbButton = dgWithdrawal.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = True
                    lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                Case objCBTrx.EnumWithdrawalStatus.Cancelled
                    lbButton = dgWithdrawal.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False
                Case objCBTrx.EnumWithdrawalStatus.Confirmed
                    lbButton = dgWithdrawal.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False
                Case objCBTrx.EnumWithdrawalStatus.Deleted
                    lbButton = dgWithdrawal.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False
             End Select
        Next

        PageNo = dgWithdrawal.CurrentPageIndex + 1
        lblTracker.Text = "Page " & PageNo & " of " & dgWithdrawal.PageCount
        
    End Sub

    Sub BindPageList()
        Dim count As Integer = 1
        Dim arrDList As New ArrayList()

        While Not count = dgWithdrawal.PageCount + 1
            arrDList.Add("Page " & count)
            count = count + 1
        End While
        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = dgWithdrawal.CurrentPageIndex
    End Sub

    
    Sub Update_Status(ByVal pv_strWithdrawalCode As String, _
                      ByVal pv_strDepCode As String, _
                      ByVal pv_intWithdrawalSts As Integer)

        Dim strOpCode_Withdrawal As String = "CB_CLSTRX_WITHDRAWAL_STATUS_UPD"
        Dim strParam As String = pv_strWithdrawalCode  & "|" &  pv_intWithdrawalSts  & "|" & Trim(pv_strDepCode)  & "|1"
        Dim intCnt As Integer
        Dim intErrNo As Integer
        Dim objRslDs  As New Object()

        Try

            intErrNo = objCBTrx.mtdUpdWithdrawalStatus(strOpCode_Withdrawal, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strAccMonth, _
                                                    strAccYear, _
                                                    strParam, objRslDs)

            If objRslDs.Tables(0).Rows(0).Item("errCodes") > 0 then
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=WITHDRAWAL_LIST_UPD_STATUS&errmesg=" & objRslDs.Tables(0).Rows(0).Item("errMsgs") & "&redirect=CB/trx/cb_trx_WithdrawalList.aspx")
                Exit Sub
            End If

        Catch Exp As Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=WITHDRAWAL_LIST_UPD_STATUS&errmesg=" & Exp.Message & "&redirect=CB/trx/cb_trx_WithdrawalList.aspx")
        End Try
    End Sub
    

    Sub btnPrevNext_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).CommandArgument
        Select Case direction
            Case "first"
                dgWithdrawal.CurrentPageIndex = 0
            Case "prev"
                dgWithdrawal.CurrentPageIndex = _
                Math.Max(0, dgWithdrawal.CurrentPageIndex - 1)
            Case "next"
                dgWithdrawal.CurrentPageIndex = _
                Math.Min(dgWithdrawal.PageCount - 1, dgWithdrawal.CurrentPageIndex + 1)
            Case "last"
                dgWithdrawal.CurrentPageIndex = dgWithdrawal.PageCount - 1
        End Select
        lstDropList.SelectedIndex = dgWithdrawal.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            dgWithdrawal.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        dgWithdrawal.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIf(SortCol.Text = "ASC", "DESC", "ASC")
        dgWithdrawal.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim lblDelText As Label
        Dim strWdrCode As String
        Dim lblIdDepCode   As Label
        Dim strIdDepCode  As String

        dgWithdrawal.EditItemIndex = CInt(E.Item.ItemIndex)
        lblDelText = dgWithdrawal.Items.Item(CInt(E.Item.ItemIndex)).FindControl("idWdrCode")
        lblIdDepCode  = dgWithdrawal.Items.Item(CInt(E.Item.ItemIndex)).FindControl("idDepCode")
        strIdDepCode = lblIdDepCode.Text
        strWdrCode = lblDelText.Text

        Update_Status(strWdrCode, strIdDepCode, objCBTrx.EnumWithdrawalStatus.Deleted)

        BindGrid()
        BindPageList()
    End Sub


    Sub NewWdrBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CB_trx_WithdrawalDet.aspx")
    End Sub

    
End Class
