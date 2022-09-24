
Imports System
Imports System.Data
Imports System.Collections 
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic
Imports agri.GlobalHdl.clsGlobalHdl
Imports agri.PWSystem.clsLangCap


Public Class CB_trx_FundingList : Inherits Page

    Protected WithEvents dgLine As DataGrid
    Protected WithEvents lbDelete As LinkButton
    Protected WithEvents lblTracker As Label
    Protected WithEvents ddlTrxType As DropDownList
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents srchTrxID As TextBox
    Protected WithEvents srchDescription As TextBox
    Protected WithEvents srchStatus As DropDownList
    Protected WithEvents lstAccYear As DropDownList
    Protected WithEvents lstAccMonth As DropDownList
    Protected WithEvents txtLastUpdate As TextBox
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents lblErrMesage As Label

    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Protected objGLTrx As New agri.GL.ClsTrx()
    Protected objCBTrx As New agri.CB.clsTrx()

    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objSys As New agri.PWSystem.clsConfig()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim intCBAR As Integer

    Dim ObjTaxDs As New Object()
    Dim objConfigDs As New Object()
    Dim objLangCapDs As New Object()

    Dim strTrxIDTag As String
    Dim strDescTag As String
    Dim strActCodeTag As String
    Dim strTitleTag As String
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim strLocType As String
    Dim strSelAccMonth As String
    Dim strSelAccYear As String

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_CBACCMONTH")
        strAccYear = Session("SS_CBACCYEAR")
        intCBAR = Session("SS_CBAR")
        strLocType = Session("SS_LOCTYPE")
        strSelAccMonth = Session("SS_SELACCMONTH")
        strSelAccYear = Session("SS_SELACCYEAR")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCBAccessRights.CBPayment), intCBAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            If SortExpression.Text = "" Then
                SortExpression.Text = "A.TrxID"
            End If

            lblErrMesage.Visible = False
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
                'BindPageList()
            End If
        End If
    End Sub

    Sub BindStatusList()
        srchStatus.Items.Add(New ListItem(objGLTrx.mtdGetFundingStatus(objGLTrx.EnumFundingStatus.All), objGLTrx.EnumFundingStatus.All))
        srchStatus.Items.Add(New ListItem(objGLTrx.mtdGetFundingStatus(objGLTrx.EnumFundingStatus.Active), objGLTrx.EnumFundingStatus.Active))
        srchStatus.Items.Add(New ListItem(objGLTrx.mtdGetFundingStatus(objGLTrx.EnumFundingStatus.Confirmed), objGLTrx.EnumFundingStatus.Confirmed))
        srchStatus.Items.Add(New ListItem(objGLTrx.mtdGetFundingStatus(objGLTrx.EnumFundingStatus.Deleted), objGLTrx.EnumFundingStatus.Deleted))
        srchStatus.Items.Add(New ListItem(objGLTrx.mtdGetFundingStatus(objGLTrx.EnumFundingStatus.Cancelled), objGLTrx.EnumFundingStatus.Cancelled))
        srchStatus.Items.Add(New ListItem(objGLTrx.mtdGetFundingStatus(objGLTrx.EnumFundingStatus.Closed), objGLTrx.EnumFundingStatus.Closed))
        srchStatus.SelectedIndex = 0
    End Sub

    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        dgLine.CurrentPageIndex = 0
        dgLine.EditItemIndex = -1
        BindGrid()
        BindPageList()
    End Sub

    Sub BindGrid()
        Dim intCnt As Integer
        Dim lbButton As LinkButton
        Dim lbl As Label
        Dim PageNo As Integer
        Dim PageCount As Integer
        Dim dsData As DataSet

        dsData = LoadData()
        PageCount = objGlobal.mtdGetPageCount(dsData.Tables(0).Rows.Count, dgLine.PageSize)

        dgLine.DataSource = dsData
        If dgLine.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgLine.CurrentPageIndex = 0
            Else
                dgLine.CurrentPageIndex = PageCount - 1
            End If
        End If

        dgLine.DataBind()
        BindPageList()
        PageNo = dgLine.CurrentPageIndex + 1
        lblTracker.Text = "Page " & PageNo & " of " & dgLine.PageCount

        For intCnt = 0 To dgLine.Items.Count - 1
            lbl = dgLine.Items.Item(intCnt).FindControl("lblStatus")
            Select Case CInt(Trim(lbl.Text))
                Case objGLTrx.EnumFundingStatus.Active
                    lbButton = dgLine.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False
                    lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                Case objGLTrx.EnumFundingStatus.Deleted
                    lbButton = dgLine.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False
            End Select
        Next


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

    Protected Function LoadData() As DataSet
        Dim strOpCd_GET As String = "CB_CLSTRX_FUNDING_LIST_GET"
        Dim strSearch As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim strParamName As String
        Dim strParamValue As String

        strSearch = IIf(Trim(srchTrxID.Text) = "", "", "A.TrxID Like '%" & Trim(srchTrxID.Text) & "%' AND ")
        strSearch = strSearch + IIf(ddlTrxType.SelectedIndex = 0, "", "A.TrxType = '" & ddlTrxType.SelectedIndex & "' AND ")
        strSearch = strSearch + IIf(Trim(srchDescription.Text) = "", "", "A.Description LIKE '" & Trim(srchDescription.Text) & "%' AND ")
        strSearch = strSearch + IIf(srchStatus.SelectedItem.Value = "0", "A.Status IN ('1','2','4','5') AND ", "A.Status IN ('" & Trim(srchStatus.SelectedItem.Value) & "') AND ")
        strSearch = strSearch + IIf(Trim(txtLastUpdate.Text) = "", "", "USR.UserName LIKE '" & Trim(txtLastUpdate.Text) & "%' AND ")

        If (Trim(strSearch) <> "") Then
            strSearch = " AND " & Mid(strSearch, 1, Len(strSearch) - 4) & " "
        End If

        strSearch = strSearch + IIf(Trim(SortExpression.Text) = "", "", "Order By " & Trim(SortExpression.Text) & " ")
        strSearch = strSearch + IIf(Trim(SortCol.Text) = "", "ASC ", Trim(SortCol.Text))

        strParamName = "STRSEARCH"
        strParamValue = strSearch

        Try
            intErrNo = objGLTrx.mtdGetDataCommon(strOpCd_GET, _
                                                strParamName, _
                                                strParamValue, _
                                                ObjTaxDs)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=FA_Trx_LeaseFinancingList_GET&errmesg=" & lblErrMesage.Text & "&redirect=")
        End Try

        Return ObjTaxDs
    End Function

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

    Sub DEDR_Delete(ByVal Sender As Object, ByVal e As DataGridCommandEventArgs)
        Dim lblDelText As Label
        Dim strOpCd_UPD As String = "CB_CLSTRX_FUNDING_UPDATE"
        Dim strParam As String = ""
        Dim strSelTrxID As String
        Dim strSelTrxType As String
        Dim strSelStatus As String
        Dim intErrNo As Integer
        Dim strParamName As String
        Dim strParamValue As String

        dgLine.EditItemIndex = CInt(e.Item.ItemIndex)
        lblDelText = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblTrxID")
        strSelTrxID = lblDelText.Text
        lblDelText = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblTrxType")
        strSelTrxType = lblDelText.Text
        lblDelText = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblStatus")
        strSelStatus = lblDelText.Text

        strParamName = "STRUPDATE|STRSEARCH"
        strParamValue = "SET STATUS = '" & IIf(strSelStatus = objGLTrx.EnumFundingStatus.Active, objGLTrx.EnumFundingStatus.Deleted, objGLTrx.EnumFundingStatus.Active) & "'" & _
                        "|" & "WHERE TrxID = '" & strSelTrxID & "' AND TrxType = '" & strSelTrxType & "'"

        Try
            intErrNo = objGLTrx.mtdInsertDataCommon(strOpCd_UPD, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=FA_Trx_LeaseFinancingList_DEL&errmesg=" & lblErrMesage.Text & "&redirect=gl/setup/FA_Trx_LeaseFinancingList.aspx")
        End Try

        dgLine.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub NewBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CB_Trx_FundingDet.aspx")
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
            intErrNo = objGLTrx.mtdGetDataCommon(strOpCd, _
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
End Class
