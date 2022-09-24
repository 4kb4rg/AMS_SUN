
Imports System
Imports System.Data


Public Class cb_trx_RekonsileList : Inherits Page

    Protected WithEvents lblErrMessage As Label
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents dgRekonsile As DataGrid
    Protected WithEvents lblTracker As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents txtRekonsileID As TextBox
    Protected WithEvents txtBankCode As TextBox
    Protected WithEvents txtLastUpdate As TextBox
    Protected WithEvents lstAccMonth As DropDownList
    Protected WithEvents lstAccYear As DropDownList

    Protected objGLTrx As New agri.GL.clsTrx()
    Protected objAPTrx As New agri.AP.clsTrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intCBAR As Integer
    Dim objRekonsileDs As New Object()
    Dim strLocType As String

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
                SortExpression.Text = "REK.RekonsileID"
            End If

            If Not Page.IsPostBack Then
                lstAccMonth.SelectedValue = strAccMonth
                BindAccYear(strAccYear)
                BindGrid()
                BindPageList()
            End If
        End If
    End Sub

    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        dgRekonsile.CurrentPageIndex = 0
        dgRekonsile.EditItemIndex = -1
        BindGrid()
        BindPageList()
    End Sub


    Sub BindGrid()
        Dim strOpCd As String = "CB_CLSTRX_REKONSILE_LIST_GET"
        Dim PageNo As Integer
        Dim PageCount As Integer
        Dim lbButton As LinkButton
        Dim strSrchRekonsileID As String
        Dim strSrchBankCode As String
        Dim strSrchAccMonth As String
        Dim strSrchAccYear As String
        Dim strSearch As String
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim lbl As Label


        strSrchRekonsileID = IIf(Trim(txtRekonsileID.Text) = "", "", " AND  REK.RekonsileID LIKE '" & txtRekonsileID.Text & "'")
        strSrchBankCode = IIf(Trim(txtBankCode.Text) = "", "", " AND  REK.BankCode = '" & txtBankCode.Text & "'")

        If lstAccMonth.SelectedItem.Value = "0" Then
            strSrchAccMonth = ""
        Else
            strSrchAccMonth = " AND REK.AccMonth = '" & lstAccMonth.SelectedItem.Value & "'"
        End If

        strSrchAccYear = " AND REK.AccYear = '" & lstAccYear.SelectedItem.Value & "'"

        strSearch = strSrchRekonsileID & strSrchBankCode & strSrchAccMonth & strSrchAccYear & _
                    " AND  REK.LocCode = '" & strLocation & "'"

        'strSearch = " WHERE " & MID(Trim(strSearch), 6)

        strSearch = strSearch & " ORDER BY " & SortExpression.Text & " " & SortCol.Text


        strParamName = "STRSEARCH"
        strParamValue = strSearch
        Try
            intErrNo = objGLTrx.mtdGetDataCommon(strOpCd, strParamName, strParamValue, objRekonsileDs)

        Catch Exp As Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=REKONSILELIST_GET_LIST&errmesg=" & lblErrMessage.Text & "&redirect=CB/trx/CB_trx_Rekonsilelist.aspx")
        End Try

        PageCount = objGlobal.mtdGetPageCount(objRekonsileDs.Tables(0).Rows.Count, dgRekonsile.PageSize)
        dgRekonsile.DataSource = objRekonsileDs
        If dgRekonsile.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgRekonsile.CurrentPageIndex = 0
            Else
                dgRekonsile.CurrentPageIndex = PageCount - 1
            End If
        End If
        dgRekonsile.DataBind()
        BindPageList()

        For intCnt = 0 To dgRekonsile.Items.Count - 1
            lbl = dgRekonsile.Items.Item(intCnt).FindControl("lblStatus")
            lbButton = dgRekonsile.Items.Item(intCnt).FindControl("lbDelete")
            lbButton.Visible = True
            lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
        Next

        PageNo = dgRekonsile.CurrentPageIndex + 1
        lblTracker.Text = "Page " & PageNo & " of " & dgRekonsile.PageCount
    End Sub

    Sub BindPageList()
        Dim count As Integer = 1
        Dim arrDList As New ArrayList()

        While Not count = dgRekonsile.PageCount + 1
            arrDList.Add("Page " & count)
            count = count + 1
        End While
        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = dgRekonsile.CurrentPageIndex
    End Sub


    Sub Update_Status(ByVal pv_strRekonsileId As String, _
                      ByVal pv_intPaymentSts As Integer)


        Dim strOpCode As String
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim intErrNo As Integer

        strParamName = "REKONSILEID"
        strParamValue = pv_strRekonsileId

        strOpCode = "CB_CLSTRX_REKONSILE_DEL"

        Try
            intErrNo = objGLTrx.mtdInsertDataCommon(strOpCode, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GENERATE_CONFIRM&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcv_wm_list")
        End Try

        

    End Sub


    Sub btnPrevNext_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).CommandArgument
        Select Case direction
            Case "first"
                dgRekonsile.CurrentPageIndex = 0
            Case "prev"
                dgRekonsile.CurrentPageIndex = _
                Math.Max(0, dgRekonsile.CurrentPageIndex - 1)
            Case "next"
                dgRekonsile.CurrentPageIndex = _
                Math.Min(dgRekonsile.PageCount - 1, dgRekonsile.CurrentPageIndex + 1)
            Case "last"
                dgRekonsile.CurrentPageIndex = dgRekonsile.PageCount - 1
        End Select
        lstDropList.SelectedIndex = dgRekonsile.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            dgRekonsile.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        dgRekonsile.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIf(SortCol.Text = "ASC", "DESC", "ASC")
        dgRekonsile.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim lblDelText As Label
        Dim strRekonsileId As String

        dgRekonsile.EditItemIndex = CInt(E.Item.ItemIndex)
        lblDelText = dgRekonsile.Items.Item(CInt(E.Item.ItemIndex)).FindControl("idRekonsileId")
        strRekonsileId = lblDelText.Text

        Update_Status(strRekonsileId, "0")

        BindGrid()
        BindPageList()
    End Sub


    Sub NewRekonsileBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CB_trx_RekonsileDet.aspx")
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
End Class
