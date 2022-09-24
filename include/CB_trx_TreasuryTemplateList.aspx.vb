
Imports System
Imports System.Data

Public Class cb_trx_TreasuryTemplateList : Inherits Page

    Protected WithEvents lblErrMesage As Label
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label

    Protected WithEvents dgTreasury As DataGrid
    Protected WithEvents lblTracker As Label
    Protected WithEvents lstDropList As DropDownList 

    Protected objCBTrx As New agri.CB.clsTrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    
    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intCBAR As Integer

    Dim objTreasuryDs As New Object()

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strAccMonth = Session("SS_CBACCMONTH")
        strAccYear = Session("SS_CBACCYEAR")
        strLangCode = Session("SS_LANGCODE")
        intCBAR = Session("SS_CBAR")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCBAccessRights.CBCashFlow), intCBAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            If SortExpression.Text = "" Then
                 SortExpression.Text = "ReportCode"
            End If

            If Not Page.IsPostBack Then
                BindGrid()
                BindPageList()
            End If
        End If
    End Sub

    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        dgTreasury.CurrentPageIndex = 0
        dgTreasury.EditItemIndex = -1
        BindGrid()
        BindPageList()
    End Sub

    Sub BindGrid()
     
        Dim strOpCode As String = "CB_CLSTRX_TREASURYTEMPLATE_LIST_GET" 
        Dim PageNo As Integer
        Dim PageCount As Integer
        Dim lbButton As LinkButton
        
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim lbl As Label

        strParam = " | " & SortExpression.text & " " & SortCol.Text
          
        Try
            intErrNo = objCBTrx.mtdGetTreasuryTemplateList(strOpCode, _
                                              strCompany, _
                                              strLocation, _
                                              strUserId, _
                                              strParam, _
                                              objTreasuryDs)
        Catch Exp As Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=TREASURYTEMPLATE_LIST_GET&errmesg=" & Exp.Message & "&redirect=CB/trx/cb_trx_TreasuryTemplateList.aspx")
        End Try

        PageCount = objGlobal.mtdGetPageCount(objTreasuryDs.Tables(0).Rows.Count, dgTreasury.PageSize)
        dgTreasury.DataSource = objTreasuryDs
        If dgTreasury.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgTreasury.CurrentPageIndex = 0
            Else
                dgTreasury.CurrentPageIndex = PageCount - 1
            End If
        End If
        dgTreasury.DataBind()
        BindPageList()

        For intCnt = 0 To dgTreasury.Items.Count - 1
            lbButton = dgTreasury.Items.Item(intCnt).FindControl("lbDelete")
            lbButton.Visible = True
            lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
        Next

        PageNo = dgTreasury.CurrentPageIndex + 1
        lblTracker.Text = "Page " & PageNo & " of " & dgTreasury.PageCount
        
    End Sub

    Sub BindPageList()
        Dim count As Integer = 1
        Dim arrDList As New ArrayList()

        While Not count = dgTreasury.PageCount + 1
            arrDList.Add("Page " & count)
            count = count + 1
        End While
        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = dgTreasury.CurrentPageIndex
    End Sub

    Sub btnPrevNext_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).CommandArgument
        Select Case direction
            Case "first"
                dgTreasury.CurrentPageIndex = 0
            Case "prev"
                dgTreasury.CurrentPageIndex = _
                Math.Max(0, dgTreasury.CurrentPageIndex - 1)
            Case "next"
                dgTreasury.CurrentPageIndex = _
                Math.Min(dgTreasury.PageCount - 1, dgTreasury.CurrentPageIndex + 1)
            Case "last"
                dgTreasury.CurrentPageIndex = dgTreasury.PageCount - 1
        End Select
        lstDropList.SelectedIndex = dgTreasury.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            dgTreasury.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        dgTreasury.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIf(SortCol.Text = "ASC", "DESC", "ASC")
        dgTreasury.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim lblDelText As Label      
        Dim strOpCode As String = "CB_CLSTRX_TREASURYTEMPLATE_DELETED"
        Dim strParam As String = ""
        Dim intCnt As Integer
        Dim intErrNo As Integer

        dgTreasury.EditItemIndex = CInt(E.Item.ItemIndex)
        lblDelText = dgTreasury.Items.Item(CInt(E.Item.ItemIndex)).FindControl("idStrCode")
        
        strParam = lblDelText.Text
            
        Try
            intErrNo = objCBTrx.mtdDelTreasuryTemplate(strOpCode, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strParam)
 
        Catch Exp As Exception
             Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_CLSTRX_TREASURYTEMPLATE_DELETED&errmesg=" & Exp.Message & "&redirect=CB/trx/cb_trx_TreasuryTemplateList.aspx")
        End Try

        BindGrid()
        BindPageList()
    End Sub


    Sub NewBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("cb_trx_TreasuryTemplate.aspx")
    End Sub
    

End Class
