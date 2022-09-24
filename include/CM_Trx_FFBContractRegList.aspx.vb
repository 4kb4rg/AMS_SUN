
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

Public Class CM_Trx_FFBContractRegList : Inherits Page

    Protected WithEvents dgLine As DataGrid
    Protected WithEvents lbDelete As LinkButton
    Protected WithEvents lblTracker As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents txtContractNo As TextBox
    Protected WithEvents txtSeller As TextBox

    Protected WithEvents ddlPricingMtd As DropDownList
    Protected WithEvents ddlProduct As DropDownList
    Protected WithEvents ddlStatus As DropDownList
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblErrCtrDate As Label
    Protected WithEvents lblDateFormat As Label
    Protected WithEvents lblErrDelvPeriod As Label
    Protected WithEvents lblBillParty As Label

    Protected WithEvents lstAccMonth As DropDownList
    Protected WithEvents lstAccYear As DropDownList

    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Protected objCMTrx As New agri.CM.clsTrx()
    Protected objCMSetup As New agri.CM.clsSetup()

    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objGLSetup As New agri.GL.clsSetup()
    Protected objWMTrx As New agri.WM.clsTrx()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objPUSetup As New agri.PU.clsSetup()
    Dim objGLtrx As New agri.GL.ClsTrx()

    Dim objContractDs As New Object()
    Dim objPriceBasisDs As New Object()
    Dim objBPDs As New Object()
    Dim objSysCfgDs As New Object()
    Dim objSellerDs As New Object()
    Dim objMatchDs As New Object()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim intCMAR As Integer
    Dim strDateSetting As String

    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim strLocType As String

    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objLangCapDs As New Object()

    Protected WithEvents btnCtrDateFrom As System.Web.UI.WebControls.Image
    Protected WithEvents btnCtrDateTo As System.Web.UI.WebControls.Image
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Protected WithEvents btnPrev As System.Web.UI.WebControls.ImageButton
    Protected WithEvents btnNext As System.Web.UI.WebControls.ImageButton
    Protected WithEvents NewTBBtn As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ibPrint As System.Web.UI.WebControls.ImageButton
    Dim strSelAccMonth As String
    Dim strSelAccYear As String
    Dim strParamName As String
    Dim strParamValue As String

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_ARACCMONTH")
        strAccYear = Session("SS_ARACCYEAR")
        intCMAR = Session("SS_CMAR")
        strDateSetting = Session("SS_DATEFMT")
        strLocType = Session("SS_LOCTYPE")
        strSelAccMonth = Session("SS_SELACCMONTH")
        strSelAccYear = Session("SS_SELACCYEAR")


        If strUserId = "" Then
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCMAccessRights.CMContractReg), intCMAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            onload_GetLangCap()
            lblErrCtrDate.Visible = False
            lblErrDelvPeriod.Visible = False
            lblDateFormat.Visible = False
            If SortExpression.Text = "" Then
                SortExpression.Text = "ctr.ContractDate"
                SortCol.Text = "desc"
            End If
            If Not Page.IsPostBack Then

                If Session("SS_FILTERPERIOD") = "0" Then
                    lstAccMonth.SelectedValue = strAccMonth
                    BindAccYear(strAccYear)
                Else
                    lstAccMonth.SelectedValue = strSelAccMonth
                    BindAccYear(strSelAccYear)
                End If

                'BindSellerList()
                BindProductList()
                BindStatusList()
                BindPriceingMtd()
                BindGrid()
                BindPageList()
            End If
        End If
    End Sub

    Sub BindPriceingMtd()
        ddlPricingMtd.Items.Add("All")
        ddlPricingMtd.Items.Add("Harga Papan")
        ddlPricingMtd.Items.Add("Harga Disbun")
        ddlPricingMtd.Items.Add("Harga Tetap")
        ddlPricingMtd.Items.Add("Harga Range")
        ddlPricingMtd.Items.Add("Harga Rendement")

        ddlStatus.SelectedIndex = 0
    End Sub

    Sub BindStatusList()
        ddlStatus.Items.Add(New ListItem(objCMTrx.mtdGetContractStatus(objCMTrx.EnumContractStatus.All), objCMTrx.EnumContractStatus.All))
        ddlStatus.Items.Add(New ListItem(objCMTrx.mtdGetContractStatus(objCMTrx.EnumContractStatus.Active), objCMTrx.EnumContractStatus.Active))
        ddlStatus.Items.Add(New ListItem(objCMTrx.mtdGetContractStatus(objCMTrx.EnumContractStatus.Confirmed), objCMTrx.EnumContractStatus.Confirmed))
        ddlStatus.Items.Add(New ListItem(objCMTrx.mtdGetContractStatus(objCMTrx.EnumContractStatus.Closed), objCMTrx.EnumContractStatus.Closed))
        ddlStatus.Items.Add(New ListItem(objCMTrx.mtdGetContractStatus(objCMTrx.EnumContractStatus.Deleted), objCMTrx.EnumContractStatus.Deleted))
        ddlStatus.SelectedIndex = 1
    End Sub

    Sub BindProductList()
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
        ddlProduct.Items.Add(New ListItem("All", ""))
        ddlProduct.SelectedIndex = 0
        ddlProduct.Enabled = False
    End Sub

    'Sub BindSellerList()
    '    Dim strOpCd_Get As String = "PU_CLSSETUP_SUPPLIER_GET"
    '    Dim strSrchStatus As String
    '    Dim strParam As String
    '    Dim intErrNo As Integer
    '    Dim intCnt As Integer
    '    Dim dr As DataRow

    '    'strSrchStatus = objPUSetup.EnumSuppStatus.Active

    '    'strParam = "||" & strSrchStatus & "||SupplierCode||"

    '    strParam = "||" & objPUSetup.EnumSuppStatus.Active & "||SupplierCode||"
    '    'strParam = strParam & "|" & IIf(Session("SS_COACENTRALIZED") = "1", "", " A.AccCode in (SELECT AccCode FROM GL_Account WHERE LocCode='" & Trim(strLocation) & "') ")
    '    strParam = strParam & "|" & IIf(Session("SS_COACENTRALIZED") = "1", "", " SupplierCode LIKE '%" & Trim(strLocation) & "%'") ', " A.AccCode in (SELECT AccCode FROM GL_Account WHERE LocCode='" & Trim(strLocation) & "') ")


    '    Try
    '        intErrNo = objPUSetup.mtdGetSupplier(strOpCd_Get, strParam, objSellerDs)
    '    Catch Exp As System.Exception
    '        Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACT_REG_SELLERLIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
    '    End Try

    '    For intCnt = 0 To objSellerDs.Tables(0).Rows.Count - 1
    '        objSellerDs.Tables(0).Rows(intCnt).Item("SupplierCode") = Trim(objSellerDs.Tables(0).Rows(intCnt).Item("SupplierCode"))
    '        objSellerDs.Tables(0).Rows(intCnt).Item("Name") = objSellerDs.Tables(0).Rows(intCnt).Item("SupplierCode") & " (" & Trim(objSellerDs.Tables(0).Rows(intCnt).Item("Name")) & ")"
    '    Next

    '    dr = objSellerDs.Tables(0).NewRow()
    '    dr("SupplierCode") = ""
    '    dr("Name") = "All"
    '    objSellerDs.Tables(0).Rows.InsertAt(dr, 0)

    '    ddlSeller.DataSource = objSellerDs.Tables(0)
    '    ddlSeller.DataValueField = "SupplierCode"
    '    ddlSeller.DataTextField = "Name"
    '    ddlSeller.DataBind()

    'End Sub

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
        Dim lbl2 As Label
        Dim lbl3 As Label
        Dim lbl4 As Label

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
            lbButton = dgLine.Items.Item(intCnt).FindControl("lbDelete")
            lbButton.Visible = False

            Select Case CInt(Trim(lbl.Text))
                Case objCMTrx.EnumContractStatus.Active
                    'lbButton = dgLine.Items.Item(intCnt).FindControl("lbDelete")
                    'lbButton.Visible = True
                    lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                    lbl2 = dgLine.Items.Item(intCnt).FindControl("lblContractQty")
                    'lbl3 = dgLine.Items.Item(intCnt).FindControl("lblMatchedQty")
                    'lbl4 = dgLine.Items.Item(intCnt).FindControl("lblActiveMatch")
                    'lbButton = dgLine.Items.Item(intCnt).FindControl("lbClose")
                    'If Trim(lbl4.Text) = "" And CDbl(lbl2.Text) - CDbl(lbl3.Text) <= 0 Then
                    '    lbButton.Visible = True
                    'Else
                    '    lbButton.Visible = False
                    'End If
                    lbButton = dgLine.Items.Item(intCnt).FindControl("lbClose")
                    lbButton.Visible = False
                Case objCMTrx.EnumContractStatus.Confirmed
                    lbButton = dgLine.Items.Item(intCnt).FindControl("lbClose")
                    lbButton.Visible = True

                    'lbButton = dgLine.Items.Item(intCnt).FindControl("lbDelete")
                    'lbButton.Visible = False
                    'lbButton = dgLine.Items.Item(intCnt).FindControl("lbClose")
                    'lbButton.Visible = False
                    lbButton.Attributes("onclick") = "javascript:return ConfirmAction('Close FFB Contract');"
                Case objCMTrx.EnumContractStatus.Closed
                    lbButton = dgLine.Items.Item(intCnt).FindControl("lbClose")
                    lbButton.Visible = False

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

    Sub GetDelvPeriod(ByVal pv_strInputDate As String, ByRef pr_strDelMonth As String, ByRef pr_strDelYear As String)
        Dim arrPeriod As Array

        arrPeriod = Split(pv_strInputDate, " ")
        Select Case Trim(LCase(arrPeriod(0)))
            Case "jan"
                pr_strDelMonth = "1"
            Case "feb"
                pr_strDelMonth = "2"
            Case "mar"
                pr_strDelMonth = "3"
            Case "apr"
                pr_strDelMonth = "4"
            Case "may"
                pr_strDelMonth = "5"
            Case "jun"
                pr_strDelMonth = "6"
            Case "jul"
                pr_strDelMonth = "7"
            Case "aug"
                pr_strDelMonth = "8"
            Case "sep"
                pr_strDelMonth = "9"
            Case "oct"
                pr_strDelMonth = "10"
            Case "nov"
                pr_strDelMonth = "11"
            Case "dec"
                pr_strDelMonth = "12"
        End Select
        pr_strDelYear = Trim(arrPeriod(1))
    End Sub

    Protected Function FormatShortDate(ByVal pv_strInputDate As String) As String
        Dim strDate As String
        Dim strMonth As String
        Dim strYear As String
        Dim strFormatDate As String
        Dim arrDate As Array

        arrDate = Split(pv_strInputDate, "/")
        strDate = Trim(arrDate(0))
        strMonth = Trim(arrDate(1))
        strYear = Trim(arrDate(2))

        strFormatDate = strMonth & "/" & strDate & "/" & strYear
        Return strFormatDate
    End Function

    Protected Function LoadData() As DataSet
        Dim strOpCd_GET As String = "CM_CLSTRX_CONTRACT_FFB_GET"
        Dim strSearch As String = ""
        Dim strSort As String = ""
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim strValidDate As String
        Dim strDateFormat As String
        Dim strDMY As String
        Dim strDelMonth As String
        Dim strDelYear As String

        If lstAccMonth.SelectedItem.Value = "0" Then
            strAccMonth = "1','2','3','4','5','6','7','8','9','10','11','12"
        Else
            strAccMonth = lstAccMonth.SelectedItem.Value
        End If

        strAccYear = lstAccYear.SelectedItem.Value

        'strDMY = objSysCfg.mtdGetDateFormat(objSysCfg.EnumDateFormat.DMY)

        strSearch = strSearch & "and A.LocCode = '" & Trim(strLocation) & "' "
        If Trim(txtContractNo.Text) <> "" Then
            strSearch = strSearch & "and A.ContractNo like '" & Trim(txtContractNo.Text) & "%' "
        End If
        If txtSeller.Text <> "" Then
            strSearch = strSearch & "and (A.SupplierCode LIKE '%" & txtSeller.Text & "%' OR B.Name LIKE '%" & txtSeller.Text & "%')"
        End If
        'If ddlProduct.SelectedItem.Value <> "" Then
        '    strSearch = strSearch & "and A.ProductCode = '" & ddlProduct.SelectedItem.Value & "' "
        'End If
        If ddlStatus.SelectedItem.Value <> CInt(objCMTrx.EnumContractStatus.All) Then
            strSearch = strSearch & "and A.Status = '" & ddlStatus.SelectedItem.Value & "' "
        End If

        strSearch = strSearch & "and A.AccMonth IN ('" & strAccMonth & "') AND A.AccYear = '" & strAccYear & "' AND ((A.PricingMtd='" & ddlPricingMtd.SelectedIndex & "') OR ('" & ddlPricingMtd.SelectedIndex & "'='0'))"

        'strSort = "order by " & Trim(SortExpression.Text) & " " & SortCol.Text
        'strParam = strSearch & "|" & strSort

        'Try
        '    intErrNo = objCMTrx.mtdGetContract(strOpCd_GET, strParam, 0, objContractDs)
        'Catch Exp As System.Exception
        '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGLIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Setup/CM_Trx_ContractRegList.aspx")
        'End Try

        strParamName = "STRSEARCH"
        strParamValue = strSearch

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_GET, _
                                                   strParamName, _
                                                   strParamValue, objContractDs)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGLIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Setup/CM_Trx_ContractRegList.aspx")
        End Try
        Return objContractDs
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

    Sub DEDR_Close(ByVal Sender As Object, ByVal e As DataGridCommandEventArgs)
        'Dim strCmdArgs As String = CType(Sender, ImageButton).CommandArgument
        Dim strOpCd_Upd As String = "CM_CLSTRX_CONTRACT_FFB_CODING_UPD"
        Dim objCMID As Object
        Dim intErrNo As Integer
        Dim lbl As Label

        dgLine.EditItemIndex = CInt(e.Item.ItemIndex)
        lbl = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblContractNo")

        strParamName = "STRSEARCH"
        strParamValue = "SET Status=4 Where ContractNo='" & lbl.Text & "'"


        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_Upd, _
                                                   strParamName, _
                                                   strParamValue, objCMID)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_CLOSE&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegDet.aspx?tbcode=" & lbl.Text)
        End Try

        dgLine.EditItemIndex = -1
        BindGrid()

        

        dgLine.EditItemIndex = -1
        BindGrid()
    End Sub
    Sub stBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strAccMonth As String = ""
        Dim strAccYear As String = ""

        strAccMonth = lstAccMonth.SelectedItem.Value
        strAccYear = lstAccYear.SelectedItem.Value

        Response.Write("<Script Language=""JavaScript"">window.open(""../reports/CM_Rpt_ContractFFBRegDet.aspx?accmonth=" & Server.UrlEncode(strAccMonth) & _
                        "&accyear=" & Server.UrlEncode(strAccYear) & _
                        "&pricingMtd=" & Server.UrlEncode(ddlPricingMtd.SelectedIndex) & _
                        """, null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal e As DataGridCommandEventArgs)
        Dim lbl As Label
        Dim strOpCd_GET As String = ""
        Dim strOpCd_ADD As String = ""
        Dim strOpCd_UPD As String = "CM_CLSTRX_CONTRACT_REG_UPD"
        Dim strParam As String = ""
        Dim intErrNo As Integer
        Dim strContractNo As String
        Dim strContractType As String
        Dim dtContractDate As Date
        Dim strProductCode As String
        Dim strSellerCode As String
        Dim strBuyerCode As String
        Dim decContractQty As String
        Dim decExtraQty As String
        Dim strExtraQtyType As String
        Dim decMatchedQty As String
        Dim strCurrencyCode As String
        Dim strPrice As String
        Dim strPriceBasisCode As String
        Dim strDelMonth As String
        Dim strDelYear As String
        Dim strRemarks As String
        Dim strAccCode As String
        Dim strBlkCode As String

        Dim strOppCd As String = "IN_CLSTRX_PURREQ_MOVEID"
        Dim strOppCd_Back As String = "IN_CLSTRX_PURREQ_BACKID"
        Dim strOppCd_GetID As String = "IN_CLSTRX_PURREQ_GETID"
        Dim objCMID As Object
        Dim strRunNo As Integer
        Dim strProdType As String
        Dim strLocType As String

        Dim strCMIDFormat As String
        Dim strNewYear As String = ""
        Dim strTranPrefix As String = "CMR"
        Dim strHistYear As String = ""
        Dim objCompDs As New Object
        Dim blnIsDetail As Boolean = True

        Dim strContType As String = ""
        Dim strContCategory As String = ""
        Dim strTerm As String = ""
        Dim strBankCode As String = ""
        Dim strBankAccNo As String = ""
        Dim strDelivMonth As String = ""


        Dim strBankCode2 As String
        Dim strBankAccNo2 As String
        Dim strProductQuality As String
        Dim strTermOfPayment As String

        dgLine.EditItemIndex = CInt(e.Item.ItemIndex)

        lbl = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblLnId")
        strContractNo = lbl.Text
        lbl = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblContractType")
        strContractType = lbl.Text
        lbl = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblContractDate")
        dtContractDate = lbl.Text
        lbl = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblProductCode")
        strProductCode = lbl.Text
        lbl = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblSellerCode")
        strSellerCode = lbl.Text
        lbl = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblBuyerCode")
        strBuyerCode = lbl.Text
        lbl = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblContractQty")
        decContractQty = lbl.Text
        lbl = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblExtraQty")
        decExtraQty = lbl.Text
        lbl = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblExtraQtyType")
        strExtraQtyType = lbl.Text
        lbl = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblMatchedQty")
        decMatchedQty = lbl.Text
        lbl = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblCurrencyCode")
        strCurrencyCode = lbl.Text
        lbl = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblPrice")
        strPrice = lbl.Text
        lbl = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblPriceBasisCode")
        strPriceBasisCode = lbl.Text
        lbl = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblDelMonth")
        strDelMonth = lbl.Text
        lbl = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblDelYear")
        strDelYear = lbl.Text
        lbl = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblRemarks")
        strRemarks = lbl.Text
        lbl = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblAccCode")
        strAccCode = lbl.Text
        lbl = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblBlkCode")
        strBlkCode = lbl.Text

        lbl = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblContType")
        strContType = lbl.Text
        lbl = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblContCategory")
        strContCategory = lbl.Text
        lbl = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblTerm")
        strTerm = lbl.Text
        lbl = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblBankCode")
        strBankCode = lbl.Text
        lbl = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblBankAccNo")
        strBankAccNo = lbl.Text
        lbl = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblDelivMonth")
        strDelivMonth = lbl.Text

        lbl = dgLine.Items.Item(e.Item.ItemIndex).FindControl("lblBankCode2")
        strBankCode2 = lbl.Text
        lbl = dgLine.Items.Item(e.Item.ItemIndex).FindControl("lblBankAccNo2")
        strBankAccNo2 = lbl.Text
        lbl = dgLine.Items.Item(e.Item.ItemIndex).FindControl("lblProductQuality")
        strProductQuality = lbl.Text
        lbl = dgLine.Items.Item(e.Item.ItemIndex).FindControl("lblTermOfPayment")
        strTermOfPayment = lbl.Text


        strParam = strContractNo & Chr(9) & _
                   strContractType & Chr(9) & _
                   dtContractDate & Chr(9) & _
                   strProductCode & Chr(9) & _
                   strSellerCode & Chr(9) & _
                   strBuyerCode & Chr(9) & _
                   decContractQty & Chr(9) & _
                   decExtraQty & Chr(9) & _
                   strExtraQtyType & Chr(9) & _
                   decMatchedQty & Chr(9) & _
                   strCurrencyCode & Chr(9) & _
                   strPrice & Chr(9) & _
                   strPriceBasisCode & Chr(9) & _
                   strDelMonth & Chr(9) & _
                   strDelYear & Chr(9) & _
                   strRemarks & Chr(9) & _
                   strAccCode & Chr(9) & _
                   strBlkCode & Chr(9) & _
                   objCMTrx.EnumContractStatus.Deleted & Chr(9) & _
                   strContType & Chr(9) & _
                   "" & Chr(9) & "" & Chr(9) & "" & Chr(9) & "" & Chr(9) & "" & Chr(9) & _
                   strContCategory & Chr(9) & _
                   strTerm & Chr(9) & _
                   strBankCode & Chr(9) & _
                   strBankAccNo & Chr(9) & _
                   strDelivMonth & Chr(9) & strBankCode2 & Chr(9) & strBankAccNo2 & Chr(9) & strProductQuality & Chr(9) & strTermOfPayment

        Try
            intErrNo = objCMTrx.mtdUpdContract(strOpCd_GET, _
                                               strOpCd_ADD, _
                                               strOpCd_UPD, _
                                               strCompany, _
                                               strOppCd, _
                                               strLocation, _
                                               strUserId, _
                                               strParam, _
                                               False, _
                                               objCMID, _
                                               True, _
                                               "CMR")

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_DEL&errmesg=" & lblErrMessage.Text & "&redirect=CM/Setup/CM_Trx_ContractRegList.aspx")
        End Try
        dgLine.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub NewTBBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CM_Trx_FFBContractRegDet.aspx")
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
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


    Sub PrintBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
 
        Dim strOwner1 As String = ""
        Dim strOwner2 As String = ""

        Dim arrBuyer As Array
        Dim strBuyer As String 

        strBuyer = Replace(strBuyer, ")", "")

        Response.Write("<Script Language=""JavaScript"">window.open(""../reports/CM_Rpt_ContractFFBRegDet.aspx?ContractNo=" & Server.UrlEncode(strBuyer) & _
                        "&Product=" & "FFB" & _
                        "&BillParty=" & Server.UrlEncode(strBuyer) & _
                        "&Buyer=" & Server.UrlEncode(strBuyer) & _
                        "&Owner1=" & Server.UrlEncode(strOwner1) & _
                        "&Owner2=" & Server.UrlEncode(strOwner2) & _
                        """, null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")
    End Sub

End Class
