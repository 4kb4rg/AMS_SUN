
Imports System
Imports System.Data

Public Class CB_trx_DepositList : Inherits Page

    Protected WithEvents lblErrMesage As Label
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label

    Protected WithEvents dgDeposit As DataGrid
    Protected WithEvents lblTracker As Label
    Protected WithEvents lstDropList As DropDownList

    Protected WithEvents ddlStatus  As DropDownList
    Protected WithEvents txtDepCode As TextBox
    Protected WithEvents txtDesc As TextBox
    Protected WithEvents txtBank As TextBox
    Protected WithEvents ddlType As DropDownList
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

    Dim objDepositDs As New Object()
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
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCBAccessRights.CBDeposit), intCBAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            If SortExpression.Text = "" Then
                 SortExpression.Text = "DEP.DepositCode"
            End If

            If Not Page.IsPostBack Then
                BindStatusList()
                BindTypeList()
                BindGrid()
                BindPageList()
            End If
        End If
    End Sub

    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        dgDeposit.CurrentPageIndex = 0
        dgDeposit.EditItemIndex = -1
        BindGrid()
        BindPageList()
    End Sub

    Sub BindStatusList()
        
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetDepositStatus(objCBTrx.EnumDepositStatus.All), objCBTrx.EnumDepositStatus.All))
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetDepositStatus(objCBTrx.EnumDepositStatus.Active), objCBTrx.EnumDepositStatus.Active))
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetDepositStatus(objCBTrx.EnumDepositStatus.Cancelled), objCBTrx.EnumDepositStatus.Cancelled))
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetDepositStatus(objCBTrx.EnumDepositStatus.Confirmed), objCBTrx.EnumDepositStatus.Confirmed))
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetDepositStatus(objCBTrx.EnumDepositStatus.Deleted), objCBTrx.EnumDepositStatus.Deleted))
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetDepositStatus(objCBTrx.EnumDepositStatus.Withdrawn), objCBTrx.EnumDepositStatus.Withdrawn))
        ddlStatus.SelectedIndex = 1

    End Sub
    
    Sub BindTypeList()
        ddlType.Items.Add(New ListItem(objCBTrx.mtdGetDepositType(objCBTrx.EnumDepositType.All), objCBTrx.EnumDepositType.All))
        ddlType.Items.Add(New ListItem(objCBTrx.mtdGetDepositType(objCBTrx.EnumDepositType.TypeI), objCBTrx.EnumDepositType.TypeI))
        ddlType.Items.Add(New ListItem(objCBTrx.mtdGetDepositType(objCBTrx.EnumDepositType.TypeII), objCBTrx.EnumDepositType.TypeII))
        ddlType.SelectedIndex = 0
    End Sub
    

    Sub BindGrid()
     
        Dim strOpCode_GetDepList As String = "CB_CLSTRX_DEPOSIT_LIST_GET" 
        Dim PageNo As Integer
        Dim PageCount As Integer
        Dim lbButton As LinkButton
        
        Dim strSrchDepCode As String
        Dim strSrchDesc As String
        Dim strSrchBankCd As String
        Dim strSrchType As String
        Dim strSrchStatus As String
        dim strSrchLastUpdate As String
        Dim strSearch As String
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim lbl As Label

        strSrchDepCode = IIf(txtDepCode.Text = "", "", txtDepCode.Text)
        strSrchDesc  = IIf(txtDesc.Text = "", "", txtDesc.Text)
        strSrchBankCd  = IIf(txtBank.Text = "", "", txtBank.Text)
        strSrchType = IIf(ddlType.SelectedItem.Value = "0", "", ddlType.SelectedItem.Value)
        strSrchStatus = IIf(ddlStatus.SelectedItem.Value = "0", "", ddlStatus.SelectedItem.Value)
        strSrchLastUpdate = IIf(txtLastUpdate.Text = "", "", txtLastUpdate.Text)

        strParam = strSrchDepCode & "|" & _
                   strSrchDesc & "|" & _
                   strSrchBankCd & "|" & _
                   strSrchType & "|" & _
                   strSrchStatus & "|" & _
                   strSrchLastUpdate & "|" & _
                   SortExpression.text & "|" & _
                   SortCol.Text
          
        Try
            intErrNo = objCBTrx.mtdGetDeposit(strOpCode_GetDepList, _
                                              strCompany, _
                                              strLocation, _
                                              strUserId, _
                                              strAccMonth, _
                                              strAccYear, _
                                              strParam, _
                                              objDepositDs)
        Catch Exp As Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=DEPOSIT_GET_LIST&errmesg=" & Exp.Message & "&redirect=CB/trx/CB_trx_DepositList.aspx")
        End Try

        PageCount = objGlobal.mtdGetPageCount(objDepositDs.Tables(0).Rows.Count, dgDeposit.PageSize)
        dgDeposit.DataSource = objDepositDs
        If dgDeposit.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgDeposit.CurrentPageIndex = 0
            Else
                dgDeposit.CurrentPageIndex = PageCount - 1
            End If
        End If
        dgDeposit.DataBind()
        BindPageList()

        For intCnt = 0 To dgDeposit.Items.Count - 1
            lbl = dgDeposit.Items.Item(intCnt).FindControl("lblStatus")
            
             Select Case CInt(Trim(lbl.Text))
                Case objCBTrx.EnumDepositStatus.Active
                    lbButton = dgDeposit.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = True
                    lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                Case objCBTrx.EnumDepositStatus.Cancelled
                    lbButton = dgDeposit.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False
                Case objCBTrx.EnumDepositStatus.Confirmed
                    lbButton = dgDeposit.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False
                Case objCBTrx.EnumDepositStatus.Deleted
                    lbButton = dgDeposit.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False
                Case objCBTrx.EnumDepositStatus.Withdrawn
                    lbButton = dgDeposit.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False
             End Select
        Next

        PageNo = dgDeposit.CurrentPageIndex + 1
        lblTracker.Text = "Page " & PageNo & " of " & dgDeposit.PageCount
        
    End Sub

    Sub BindPageList()
        Dim count As Integer = 1
        Dim arrDList As New ArrayList()

        While Not count = dgDeposit.PageCount + 1
            arrDList.Add("Page " & count)
            count = count + 1
        End While
        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = dgDeposit.CurrentPageIndex
    End Sub

    
    Sub Update_Status(ByVal pv_strDepositCode As String, _
                      ByVal pv_intDepositSts As Integer)

        Dim strOpCd As String = "CB_CLSTRX_DEPOSIT_STATUS_UPD"
        Dim strParam As String = pv_strDepositCode & "|" & pv_intDepositSts & "|1" 
        Dim intCnt As Integer
        Dim intErrNo As Integer
        Dim objRslDs  As New Object()

        Try     
            intErrNo = objCBTrx.mtdUpdDepositStatus(strOpCd, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strAccMonth, _
                                                    strAccYear, strParam, objRslDs)

            If objRslDs.Tables(0).Rows(0).Item("errCodes") > 0 then
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_CLSTRX_DEPOSIT_UPDATE&errmesg=" & objRslDs.Tables(0).Rows(0).Item("errMsgs") & "&redirect=CB/trx/cb_trx_DepositList.aspx")
                Exit Sub
            End If
            
            Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=DEPOSITLIST_UPD_STATUS&errmesg=" & Exp.Message & "&redirect=CB/trx/cb_trx_DepositList.aspx")
        End Try

    End Sub
    

    Sub btnPrevNext_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).CommandArgument
        Select Case direction
            Case "first"
                dgDeposit.CurrentPageIndex = 0
            Case "prev"
                dgDeposit.CurrentPageIndex = _
                Math.Max(0, dgDeposit.CurrentPageIndex - 1)
            Case "next"
                dgDeposit.CurrentPageIndex = _
                Math.Min(dgDeposit.PageCount - 1, dgDeposit.CurrentPageIndex + 1)
            Case "last"
                dgDeposit.CurrentPageIndex = dgDeposit.PageCount - 1
        End Select
        lstDropList.SelectedIndex = dgDeposit.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            dgDeposit.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        dgDeposit.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIf(SortCol.Text = "ASC", "DESC", "ASC")
        dgDeposit.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim lblDelText As Label
        Dim strDepCode As String

        dgDeposit.EditItemIndex = CInt(E.Item.ItemIndex)
        lblDelText = dgDeposit.Items.Item(CInt(E.Item.ItemIndex)).FindControl("idDepCode")
        strDepCode = lblDelText.Text

        Update_Status(strDepCode, objCBTrx.EnumDepositStatus.Deleted)

        BindGrid()
        BindPageList()
    End Sub


    Sub NewDepBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CB_trx_DepositDet.aspx")
    End Sub

    
End Class
