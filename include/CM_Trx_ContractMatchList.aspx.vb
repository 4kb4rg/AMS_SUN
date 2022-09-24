
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



Public Class CM_Trx_ContractMatchList : Inherits Page

    Protected WithEvents dgLine As DataGrid
    Protected WithEvents lbDelete As LinkButton
    Protected WithEvents lblTracker As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents txtMatchingId As TextBox
    Protected WithEvents ddlProduct As DropDownList
    Protected WithEvents ddlBuyer As DropDownList
    Protected WithEvents ddlStatus As DropDownList
    Protected WithEvents txtLastUpdate As TextBox
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblBillParty As Label
    Protected WithEvents lblCode As Label

    Protected WithEvents lstAccMonth As DropDownList
    Protected WithEvents lstAccYear As DropDownList

    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Protected objCMTrx As New agri.CM.clsTrx()
    Protected objWMTrx As New agri.WM.clsTrx()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objGLSetup As New agri.GL.clsSetup()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objGLtrx As New agri.GL.ClsTrx()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim intCMAR As Integer

    Dim objMatchDs As New Object()
    Dim objPriceBasisDs As New Object()
    Dim objBPDs As New Object()
    Dim objLangCapDs As New Object()
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim strLocType As String

    Dim strSelAccMonth As String
    Dim strSelAccYear As String

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_ARACCMONTH")
        strAccYear = Session("SS_ARACCYEAR")
        intCMAR = Session("SS_CMAR")
        strLocType = Session("SS_LOCTYPE")
        strSelAccMonth = Session("SS_SELACCMONTH")
        strSelAccYear = Session("SS_SELACCYEAR")

        If strUserId = "" Then
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCMAccessRights.CMContractMatching), intCMAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            onload_GetLangCap()
            If SortExpression.Text = "" Then
                SortExpression.Text = "ma.MatchingId"
            End If
            If Not Page.IsPostBack Then
                If Session("SS_FILTERPERIOD") = "0" Then
                    lstAccMonth.SelectedValue = strAccMonth
                    BindAccYear(strAccYear)
                Else
                    lstAccMonth.SelectedValue = strSelAccMonth
                    BindAccYear(strSelAccYear)
                End If

                BindBuyerList()
                BindProductList()
                BindStatusList()
                BindGrid()
                BindPageList()
            End If
        End If
    End Sub

    Sub BindStatusList()
        ddlStatus.Items.Add(New ListItem(objCMTrx.mtdGetContractMatchStatus(objCMTrx.EnumContractMatchStatus.All), objCMTrx.EnumContractMatchStatus.All))
        ddlStatus.Items.Add(New ListItem(objCMTrx.mtdGetContractMatchStatus(objCMTrx.EnumContractMatchStatus.Active), objCMTrx.EnumContractMatchStatus.Active))
        ddlStatus.Items.Add(New ListItem(objCMTrx.mtdGetContractMatchStatus(objCMTrx.EnumContractMatchStatus.Confirmed), objCMTrx.EnumContractMatchStatus.Confirmed))
        ddlStatus.Items.Add(New ListItem(objCMTrx.mtdGetContractMatchStatus(objCMTrx.EnumContractMatchStatus.Deleted), objCMTrx.EnumContractMatchStatus.Deleted))
        ddlStatus.SelectedIndex = 1
    End Sub

    Sub BindProductList()
        ddlProduct.Items.Add(New ListItem("All", ""))
        ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.FFB), objWMTrx.EnumWeighBridgeTicketProduct.FFB))
        ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.CPO), objWMTrx.EnumWeighBridgeTicketProduct.CPO))
        ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.PK), objWMTrx.EnumWeighBridgeTicketProduct.PK))
        ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.EFB), objWMTrx.EnumWeighBridgeTicketProduct.EFB))
        ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.Shell), objWMTrx.EnumWeighBridgeTicketProduct.Shell))
        ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.AbuJanjang), objWMTrx.EnumWeighBridgeTicketProduct.AbuJanjang))
        ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.Fiber), objWMTrx.EnumWeighBridgeTicketProduct.Fiber))
        ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.Brondolan), objWMTrx.EnumWeighBridgeTicketProduct.Brondolan))
        ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.Solid), objWMTrx.EnumWeighBridgeTicketProduct.Solid))
        ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.MinyakLimbah), objWMTrx.EnumWeighBridgeTicketProduct.MinyakLimbah))
        ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.Others), objWMTrx.EnumWeighBridgeTicketProduct.Others))
    End Sub

    Sub BindBuyerList()
        Dim strOpCd_Get As String = "GL_CLSSETUP_BILLPARTY_GET"
        Dim strSrchStatus As String
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim dr As DataRow

        strSrchStatus = objGLSetup.EnumBillPartyStatus.Active

        strParam = "" & "|" & _
                   "" & "|" & _
                   strSrchStatus & "|" & _
                   "" & "|" & _
                   "BP.BillPartyCode" & "|" & _
                   "asc" & "|"

        Try
            intErrNo = objGLSetup.mtdGetBillParty(strOpCd_Get, strParam, objBPDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACT_REG_BILLPARTYLIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To objBPDs.Tables(0).Rows.Count - 1
            objBPDs.Tables(0).Rows(intCnt).Item("BillPartyCode") = Trim(objBPDs.Tables(0).Rows(intCnt).Item("BillPartyCode"))
            objBPDs.Tables(0).Rows(intCnt).Item("Name") = objBPDs.Tables(0).Rows(intCnt).Item("BillPartyCode") & " (" & Trim(objBPDs.Tables(0).Rows(intCnt).Item("Name")) & ")"
        Next

        dr = objBPDs.Tables(0).NewRow()
        dr("BillPartyCode") = ""
        dr("Name") = "All"
        objBPDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlBuyer.DataSource = objBPDs.Tables(0)
        ddlBuyer.DataValueField = "BillPartyCode"
        ddlBuyer.DataTextField = "Name"
        ddlBuyer.DataBind()

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
                Case objCMTrx.EnumContractMatchStatus.Active
                    lbButton = dgLine.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = True
                    lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                Case objCMTrx.EnumContractMatchStatus.Confirmed
                    lbButton = dgLine.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False
                Case objCMTrx.EnumContractMatchStatus.Deleted
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
        Dim strOpCd_GET As String = "CM_CLSTRX_CONTRACT_MATCH_GET"
        Dim strSearch As String = ""
        Dim strSort As String = ""
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer

        If lstAccMonth.SelectedItem.Value = "0" Then
            strAccMonth = "1','2','3','4','5','6','7','8','9','10','11','12"
        Else
            strAccMonth = lstAccMonth.SelectedItem.Value
        End If

        strAccYear = lstAccYear.SelectedItem.Value

        strSearch = strSearch & "and ma.LocCode = '" & Trim(strLocation) & "' " & _
                    "and ma.AccMonth IN ('" & strAccMonth & "') and ma.AccYear = '" & strAccYear & "' "

        If Trim(txtMatchingId.Text) <> "" Then
            strSearch = strSearch & "and ln.ContractNo like '%" & Trim(txtMatchingId.Text) & "%' "
        End If

        If ddlProduct.SelectedItem.Value <> "" Then
            strSearch = strSearch & "and ma.ProductCode = '" & ddlProduct.SelectedItem.Value & "' "
        End If

        If ddlBuyer.SelectedItem.Value <> "" Then
            strSearch = strSearch & "and ma.BuyerCode = '" & ddlBuyer.SelectedItem.Value & "' "
        End If


        If ddlStatus.SelectedItem.Value <> CInt(objCMTrx.EnumContractMatchStatus.All) Then
            strSearch = strSearch & "and ma.Status = '" & ddlStatus.SelectedItem.Value & "' "
        End If

        If Trim(txtLastUpdate.Text) <> "" Then
            strSearch = strSearch & "and usr.UserName like '" & Trim(txtLastUpdate.Text) & "%' "
        End If

        strSearch = strSearch & "and ma.AccMonth IN ('" & strAccMonth & "') AND ma.AccYear = '" & strAccYear & "' "

        strSort = "order by " & Trim(SortExpression.Text) & " " & SortCol.Text
        strParam = strSearch & "|" & strSort

        Try
            intErrNo = objCMTrx.mtdGetContractMatch(strOpCd_GET, strParam, 0, objMatchDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGLIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Setup/CM_Trx_MPOBPriceList.aspx")
        End Try

        If objMatchDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objMatchDs.Tables(0).Rows.Count - 1
                objMatchDs.Tables(0).Rows(intCnt).Item("MatchingId") = Trim(objMatchDs.Tables(0).Rows(intCnt).Item("MatchingId"))
                objMatchDs.Tables(0).Rows(intCnt).Item("ProductCode") = Trim(objMatchDs.Tables(0).Rows(intCnt).Item("ProductCode"))
                objMatchDs.Tables(0).Rows(intCnt).Item("BuyerCode") = Trim(objMatchDs.Tables(0).Rows(intCnt).Item("BuyerCode"))
                objMatchDs.Tables(0).Rows(intCnt).Item("Status") = Trim(objMatchDs.Tables(0).Rows(intCnt).Item("Status"))
                objMatchDs.Tables(0).Rows(intCnt).Item("UserName") = Trim(objMatchDs.Tables(0).Rows(intCnt).Item("UserName"))
            Next
        End If
        Return objMatchDs
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
        SortCol.Text = IIf(SortCol.Text = "asc", "desc", "asc")
        dgLine.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal e As DataGridCommandEventArgs)
        Dim strOpCd_Add As String = "CM_CLSTRX_CONTRACTMATCH_ADD"
        Dim strOpCd_AddLine As String = "CM_CLSTRX_CONTRACTMATCH_LN_ADD"
        Dim strOpCd_Upd As String = "CM_CLSTRX_CONTRACTMATCH_UPD"
        Dim strOpCd_DelLine As String = "CM_CLSTRX_CONTRACTMATCH_LN_DEL"
        Dim strOpCd_UpdContract As String = "CM_CLSTRX_CONTRACT_REG_UPD"
        Dim strOpCd_UpdTicket As String = "CM_CLSTRX_CONTRACTMATCH_UPDTICKET"
        Dim strOpCd_GetMatchLine As String = "CM_CLSTRX_CONTRACTMATCH_LN_GET"
        Dim strOpCd_UpdMatchLine As String = "CM_CLSTRX_CONTRACTMATCH_LN_UPD"

        Dim lbl As Label
        Dim strParam As String = ""
        Dim intErrNo As Integer
        Dim strMatchingId As String

        dgLine.EditItemIndex = CInt(e.Item.ItemIndex)

        lbl = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblLnId")
        strMatchingId = lbl.Text

        strParam = strLocation & Chr(9) & _
                    strMatchingId & Chr(9) & _
                    "" & Chr(9) & _
                    "" & Chr(9) & _
                    "" & Chr(9) & _
                    "" & Chr(9) & _
                    "" & Chr(9) & _
                    "" & Chr(9) & _
                    "" & Chr(9) & _
                    "" & Chr(9) & _
                    objCMTrx.EnumContractMatchStatus.Deleted & Chr(9) & _
                    ""
        Try

            intErrNo = objCMTrx.mtdProcessContractMatch(strOpCd_Add, _
                                                        strOpCd_AddLine, _
                                                        strOpCd_Upd, _
                                                        strOpCd_DelLine, _
                                                        strOpCd_UpdContract, _
                                                        strOpCd_UpdTicket, _
                                                        strOpCd_GetMatchLine, _
                                                        strOpCd_UpdMatchLine, _
                                                        strCompany, _
                                                        strLocation, _
                                                        strUserId, _
                                                        strParam, _
                                                        "", _
                                                        "", _
                                                        "", _
                                                        True)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTMATCHDET_DELETE&errmesg=" & lblErrMessage.Text & "&redirect=CM/trx/CM_Trx_ContractMatchDet.aspx?tbcode=" & strMatchingId)
        End Try



        dgLine.EditItemIndex = -1
        BindGrid()
    End Sub


    Sub NewTBBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CM_Trx_ContractMatchDet.aspx")
    End Sub






    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblBillParty.Text = GetCaption(objLangCap.EnumLangCap.BillParty) & lblCode.text
        dgLine.Columns(1).HeaderText = lblBillParty.Text
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREG_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=CM/Setup/CM_Trx_ContractRegList.aspx")
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
