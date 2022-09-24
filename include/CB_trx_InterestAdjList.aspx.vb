
Imports System
Imports System.Data

Public Class CB_trx_InterestAdjList : Inherits Page

    Protected WithEvents lblErrMesage As Label
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label

    Protected WithEvents dgInterestAdj As DataGrid
    Protected WithEvents lblTracker As Label
    Protected WithEvents lstDropList As DropDownList

    Protected WithEvents ddlStatus  As DropDownList
    Protected WithEvents txtIntCode As TextBox
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

    Dim objInterestAdjDs As New Object()
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
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCBAccessRights.CBInterAdj), intCBAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            If SortExpression.Text = "" Then
                 SortExpression.Text = "ADJ.InterestCode"
            End If

            If Not Page.IsPostBack Then
                BindStatusList()
                BindGrid()
                BindPageList()
            End If
        End If
    End Sub

    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        dgInterestAdj.CurrentPageIndex = 0
        dgInterestAdj.EditItemIndex = -1
        BindGrid()
        BindPageList()
    End Sub

    Sub BindStatusList()
        
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetInterestAdjStatus(objCBTrx.EnumInterestAdjStatus.All), objCBTrx.EnumInterestAdjStatus.All))
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetInterestAdjStatus(objCBTrx.EnumInterestAdjStatus.Active), objCBTrx.EnumInterestAdjStatus.Active))
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetInterestAdjStatus(objCBTrx.EnumInterestAdjStatus.Cancelled), objCBTrx.EnumInterestAdjStatus.Cancelled))
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetInterestAdjStatus(objCBTrx.EnumInterestAdjStatus.Confirmed), objCBTrx.EnumInterestAdjStatus.Confirmed))
        ddlStatus.Items.Add(New ListItem(objCBTrx.mtdGetInterestAdjStatus(objCBTrx.EnumInterestAdjStatus.Deleted), objCBTrx.EnumInterestAdjStatus.Deleted))
        ddlStatus.SelectedIndex = 1

    End Sub

    Sub BindGrid()
     
        Dim strOpCode As String = "CB_CLSTRX_INTERESTADJ_LIST_GET" 
        Dim PageNo As Integer
        Dim PageCount As Integer
        Dim lbButton As LinkButton
        
        Dim strSrchIntCode As String
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

        strSrchIntCode = IIf(txtIntCode.Text = "", "", txtIntCode.Text)
        strSrchDesc  = IIf(txtDesc.Text = "", "", txtDesc.Text)
        strSrchDepCode  = IIf(txtDepCode.Text = "", "", txtDepCode.Text)
        strSrchBilyetNo = IIf(txtBilyetNo.Text = "", "", txtBilyetNo.Text)
        strSrchStatus = IIf(ddlStatus.SelectedItem.Value = "0", "", ddlStatus.SelectedItem.Value)
        strSrchLastUpdate = IIf(txtLastUpdate.Text = "", "", txtLastUpdate.Text)

        strParam = strSrchIntCode & "|" & _
                   strSrchDesc & "|" & _
                   strSrchDepCode & "|" & _
                   strSrchBilyetNo & "|" & _
                   strSrchStatus & "|" & _
                   strSrchLastUpdate & "|" & _
                   SortExpression.text & "|" & _
                   SortCol.Text
          
        Try
            intErrNo = objCBTrx.mtdGetInterestAdj(strOpCode, _
                                              strCompany, _
                                              strLocation, _
                                              strUserId, _
                                              strAccMonth, _
                                              strAccYear, _
                                              strParam, _
                                              objInterestAdjDs)
        Catch Exp As Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=INTERESTADJ_GET_LIST&errmesg=" & Exp.Message & "&redirect=CB/trx/CB_trx_InterestAdjList.aspx")
        End Try

        PageCount = objGlobal.mtdGetPageCount(objInterestAdjDs.Tables(0).Rows.Count, dgInterestAdj.PageSize)
        dgInterestAdj.DataSource = objInterestAdjDs
        If dgInterestAdj.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgInterestAdj.CurrentPageIndex = 0
            Else
                dgInterestAdj.CurrentPageIndex = PageCount - 1
            End If
        End If
        dgInterestAdj.DataBind()
        BindPageList()

        For intCnt = 0 To dgInterestAdj.Items.Count - 1
            lbl = dgInterestAdj.Items.Item(intCnt).FindControl("lblStatus")
            
             Select Case CInt(Trim(lbl.Text))
                Case objCBTrx.EnumInterestAdjStatus.Active
                    lbButton = dgInterestAdj.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = True
                    lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                Case objCBTrx.EnumInterestAdjStatus.Cancelled
                    lbButton = dgInterestAdj.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False
                Case objCBTrx.EnumInterestAdjStatus.Confirmed
                    lbButton = dgInterestAdj.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False
                Case objCBTrx.EnumInterestAdjStatus.Deleted
                    lbButton = dgInterestAdj.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False
             End Select
        Next

        PageNo = dgInterestAdj.CurrentPageIndex + 1
        lblTracker.Text = "Page " & PageNo & " of " & dgInterestAdj.PageCount
        
    End Sub

    Sub BindPageList()
        Dim count As Integer = 1
        Dim arrDList As New ArrayList()

        While Not count = dgInterestAdj.PageCount + 1
            arrDList.Add("Page " & count)
            count = count + 1
        End While
        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = dgInterestAdj.CurrentPageIndex
    End Sub

    
    Sub Update_Status(ByVal pv_strInterestAdjCode As String, ByVal pv_strDepCode As String, _
                      ByVal pv_intInterestAdjSts As Integer)

        Dim strOpCode As String = "CB_CLSTRX_INTERESTADJ_STATUS_UPD"
        Dim strParam As String = pv_strInterestAdjCode & "|" & pv_intInterestAdjSts & "|" & Trim(pv_strDepCode) & "|1"
        Dim intCnt As Integer
        Dim intErrNo As Integer
        Dim objRslDs  As New Object()

        Try

            intErrNo = objCBTrx.mtdUpdInterestAdjStatus(strOpCode, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strAccMonth, _
                                                    strAccYear, _
                                                    strParam, _
                                                    objRslDs)
           
            If objRslDs.Tables(0).Rows(0).Item("errCodes") > 0 then
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INTERESTADJ_LIST_UPD_STATUS&errmesg=" & objRslDs.Tables(0).Rows(0).Item("errMsgs") & "&redirect=CB/trx/CB_trx_InterestAdjList.aspx")
                Exit Sub
            End If
        
      
        Catch Exp As Exception
             Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_CLSTRX_INTERESTADJ_UPDATE&errmesg=" & Exp.Message & "&redirect=CB/trx/cb_trx_InterestAdjList.aspx")
        End Try

    End Sub
    

    Sub btnPrevNext_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).CommandArgument
        Select Case direction
            Case "first"
                dgInterestAdj.CurrentPageIndex = 0
            Case "prev"
                dgInterestAdj.CurrentPageIndex = _
                Math.Max(0, dgInterestAdj.CurrentPageIndex - 1)
            Case "next"
                dgInterestAdj.CurrentPageIndex = _
                Math.Min(dgInterestAdj.PageCount - 1, dgInterestAdj.CurrentPageIndex + 1)
            Case "last"
                dgInterestAdj.CurrentPageIndex = dgInterestAdj.PageCount - 1
        End Select
        lstDropList.SelectedIndex = dgInterestAdj.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            dgInterestAdj.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        dgInterestAdj.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIf(SortCol.Text = "ASC", "DESC", "ASC")
        dgInterestAdj.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim lblDelText As Label
        Dim lblDepCodeText As Label
        Dim strIntCode As String
        Dim strDepCode As String

        dgInterestAdj.EditItemIndex = CInt(E.Item.ItemIndex)
        lblDelText = dgInterestAdj.Items.Item(CInt(E.Item.ItemIndex)).FindControl("idIntCode")
        lblDepCodeText = dgInterestAdj.Items.Item(CInt(E.Item.ItemIndex)).FindControl("idDepCode")
        strIntCode = lblDelText.Text
        strDepCode =  lblDepCodeText.Text

        Update_Status(strIntCode, strDepCode, objCBTrx.EnumInterestAdjStatus.Deleted)

        BindGrid()
        BindPageList()
    End Sub


    Sub NewBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CB_trx_InterestAdjDet.aspx")
    End Sub

    
End Class
