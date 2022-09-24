Imports System
Imports System.Data
Imports System.Collections 
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic

Public Class CM_Trx_Contract_Monitor : Inherits Page

    Protected WithEvents dgContractList As DataGrid
    Protected WithEvents dgDOList As DataGrid
    Protected WithEvents dgInvoiceList As DataGrid

    Protected WithEvents lblErrMessage As Label

    Protected WithEvents StatusList As DropDownList
    Protected WithEvents srchStatusList As DropDownList
    Protected WithEvents srchPRTypeList As DropDownList
    Protected WithEvents srchPRLevelList As DropDownList
    Protected WithEvents srchProductList As DropDownList
    Protected WithEvents SortExpression As Label
    Protected WithEvents sortcol As Label
    Protected WithEvents srchContractNo As TextBox
    Protected WithEvents srchDO As TextBox
    Protected WithEvents srchCust As TextBox

    Protected WithEvents TotalAmount As Label
    Protected WithEvents ibPrint As ImageButton
    Protected WithEvents lstAccMonth As DropDownList

    Protected WithEvents lstAccYear As DropDownList
    Protected WithEvents lblSearch As Label

    Protected WithEvents cbExcelContract As CheckBox
    Protected WithEvents cbExcelDO As CheckBox
    Protected WithEvents cbExcelInvoice As CheckBox

    Protected WithEvents ContractListPreview As ImageButton
    Protected WithEvents DoListPreview As ImageButton
    Protected WithEvents InvoiceListPreview As ImageButton

    Protected objIN As New agri.IN.clsTrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Protected objAdminLoc As New agri.Admin.clsLoc()
    Dim objGLtrx As New agri.GL.ClsTrx()
    Protected objWMTrx As New agri.WM.clsTrx()
    Dim objPU As New agri.PU.clsTrx()

    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim strOppCd_GET_PRList As String = "IN_CLSTRX_PURREQ_LIST_GET"
    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim intWMAR As Integer

    Dim objDataSet As New DataSet()
    Dim intCnt As Integer
    Dim intErrNo As Integer
    Dim intPRCount As Integer
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLocLevel As String
    Dim intLevel As Integer
    Dim strSelAccMonth As String
    Dim strSelAccYear As String


    'Dim BtnApproved As Button
    Dim BtnCancel As Button
    Dim APPButton As LinkButton
    Dim UpdButton As LinkButton
    Dim CancelButton As LinkButton
    Dim SaveButton As LinkButton

    Dim nCol_NoUrut As Byte = 0
    Dim nCol_PRILND As Byte = 1
    Dim nCol_PRID As Byte = 2
    Dim nCol_PRIDShow As Byte = 3
    Dim nCol_ItemCode As Byte = 4

    Dim nCol_DDLToLocCode As Byte = 9
    Dim nCol_lblLocCode As Byte = 9
    Dim nCol_DDLToUser As Byte = 10


#Region "COMPONENT"

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)

        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        intWMAR = Session("SS_WMAR")
        strAccMonth = Session("SS_INACCMONTH")
        strAccYear = Session("SS_INACCYEAR")
        strLocLevel = Session("SS_LOCLEVEL")
        intLevel = Session("SS_USRLEVEL")
        strSelAccMonth = Session("SS_SELACCMONTH")
        strSelAccYear = Session("SS_SELACCYEAR")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumWMAccessRights.WMWeighBridgeTicket), intWMAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            If Not Page.IsPostBack Then
                If Session("SS_FILTERPERIOD") = "0" Then
                    BindAccYear(strAccYear)
                Else
                    BindAccYear(strSelAccYear)
                End If

                lstAccMonth.Text = strSelAccMonth

                BindSearchProductList()
                'BindGrid()
            End If
        End If
    End Sub

    Sub BindSearchProductList()

        srchProductList.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.All), objWMTrx.EnumWeighBridgeTicketProduct.All))
        srchProductList.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.CPO), objWMTrx.EnumWeighBridgeTicketProduct.CPO))
        srchProductList.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.PK), objWMTrx.EnumWeighBridgeTicketProduct.PK))
        srchProductList.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.EFB), objWMTrx.EnumWeighBridgeTicketProduct.EFB))
        srchProductList.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.Shell), objWMTrx.EnumWeighBridgeTicketProduct.Shell))
        srchProductList.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.AbuJanjang), objWMTrx.EnumWeighBridgeTicketProduct.AbuJanjang))
        srchProductList.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.Fiber), objWMTrx.EnumWeighBridgeTicketProduct.Fiber))
        srchProductList.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.Brondolan), objWMTrx.EnumWeighBridgeTicketProduct.Brondolan))
        srchProductList.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.Solid), objWMTrx.EnumWeighBridgeTicketProduct.Solid))
        srchProductList.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.MinyakLimbah), objWMTrx.EnumWeighBridgeTicketProduct.MinyakLimbah))
        srchProductList.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.FFB), objWMTrx.EnumWeighBridgeTicketProduct.FFB))
        srchProductList.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.Others), objWMTrx.EnumWeighBridgeTicketProduct.Others))

    End Sub

    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        dgContractList.CurrentPageIndex = 0
        dgContractList.EditItemIndex = -1
        BindGrid()

        dgDOList.CurrentPageIndex = 0
        dgDOList.EditItemIndex = -1
        BindGridDO()

        dgInvoiceList.CurrentPageIndex = 0
        dgInvoiceList.EditItemIndex = -1
        BindGridInvoice()
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        Try
            dgContractList.CurrentPageIndex = e.NewPageIndex
            BindGrid()

            CheckStatus()
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        sortcol.Text = IIf(sortcol.Text = "ASC", "DESC", "ASC")
       
        BindGrid()
        CheckStatus()
    End Sub

    Sub CheckStatus()
    End Sub

    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        dgContractList.DataSource = Nothing
        dgContractList.DataBind()
        BindGrid()
    End Sub

    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        'Dim lbl As Label
        'Dim txt As TextBox
        'Dim btn As LinkButton

        'lbl = E.Item.FindControl("lblBuyerFirst")
        'lbl.Visible = False
        'txt = E.Item.FindControl("TxtBuyerFirst")
        'txt.Visible = True

        'lbl = E.Item.FindControl("lblBuyerSecond")
        'lbl.Visible = False
        'txt = E.Item.FindControl("TxtBuyerSecond")
        'txt.Visible = True


        'lbl = E.Item.FindControl("lblBuyerNet")
        'lbl.Visible = False
        'txt = E.Item.FindControl("TxtBuyerNet")
        'txt.Visible = True

        ''CType(dgItem.Cells(nCol_DDLToUser).FindControl("ddlToUser"), DropDownList).SelectedItem.Value()

        'btn = E.Item.FindControl("Edit")
        'btn.Visible = False

        'btn = E.Item.FindControl("Cancel")
        'btn.Visible = True

        'btn = E.Item.FindControl("Update")
        'btn.Visible = True

    End Sub

    Sub DEDR_Update(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        'Dim StrOpCode As String = "WM_CLSTRX_WEIGHBRIDGE_TICKET_EDIT_UPDATE_GET"
        'Dim strParamName As String = ""
        'Dim strParamValue As String = ""
        'Dim lblTicketNo As Label
        'Dim TxtBuyerFirst As TextBox
        'Dim TxtBuyerSec As TextBox
        'Dim TxtBuyerNet As TextBox
        'Dim btn As LinkButton

        'lblTicketNo = E.Item.FindControl("lblTicketNo")
        'TxtBuyerFirst = E.Item.FindControl("TxtBuyerFirst")
        'TxtBuyerSec = E.Item.FindControl("TxtBuyerSecond")
        'TxtBuyerNet = E.Item.FindControl("TxtBuyerNet")

        'strParamName = "STRSEARCH"
        'strParamValue = "SET BuyerFirstWeight=" & TxtBuyerFirst.Text & _
        '                            ",BuyerSecondWeight=" & TxtBuyerSec.Text & _
        '                            ",BuyerNetWeight=" & TxtBuyerNet.Text & _
        '                            " Where TicketNo='" & lblTicketNo.Text & "' And LocCode='" & strLocation & "'"

        'Try
        '    intErrNo = objGLtrx.mtdInsertDataCommon(StrOpCode, _
        '                                            strParamName, _
        '                                            strParamValue)

        'Catch Exp As System.Exception
        '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=UPD_ITEM&errmesg=" & Exp.ToString() & "&redirect=pu/trx/pu_trx_GRList")
        'End Try

        'TxtBuyerFirst.Visible = False
        'TxtBuyerSec.Visible = False
        'TxtBuyerNet.Visible = False

        'btn = E.Item.FindControl("Edit")
        'btn.Visible = True

        'btn = E.Item.FindControl("Cancel")
        'btn.Visible = False

        'btn = E.Item.FindControl("Update")
        'btn.Visible = False

        'BindGrid()
    End Sub

    Sub Btnrefresh_onClick(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        BindGrid()
    End Sub

    Sub ContractListPreview_onClick(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim cAccMonth As String = IIf(Len(lstAccMonth.SelectedItem.Value) = 1, "0" & Trim(lstAccYear.SelectedItem.Value), Trim(lstAccMonth.SelectedItem.Value))
        strAccYear = lstAccYear.SelectedItem.Value

        Response.Clear()
        Response.AddHeader("content-disposition", "attachment;filename=MONITOR.CONTRACT-" & Trim(strLocation) & "-" & Trim(cAccMonth) & Trim(strAccYear) & ".xls")
        Response.Charset = ""
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.ContentType = "application/vnd.xls"

        Dim stringWrite = New System.IO.StringWriter()
        Dim htmlWrite = New HtmlTextWriter(stringWrite)

        dgContractList.RenderControl(htmlWrite)
        Response.Write(stringWrite.ToString())
        Response.End()
    End Sub

    Sub DOListPreview_onClick(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim cAccMonth As String = IIf(Len(lstAccMonth.SelectedItem.Value) = 1, "0" & Trim(lstAccYear.SelectedItem.Value), Trim(lstAccMonth.SelectedItem.Value))
        strAccYear = lstAccYear.SelectedItem.Value

        Response.Clear()
        Response.AddHeader("content-disposition", "attachment;filename=MONITOR.DO-" & Trim(strLocation) & "-" & Trim(cAccMonth) & Trim(strAccYear) & ".xls")
        Response.Charset = ""
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.ContentType = "application/vnd.xls"

        Dim stringWrite = New System.IO.StringWriter()
        Dim htmlWrite = New HtmlTextWriter(stringWrite)

        dgDOList.RenderControl(htmlWrite)
        Response.Write(stringWrite.ToString())
        Response.End()
    End Sub

    Sub InvoiceListPreview_onClick(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim cAccMonth As String = IIf(Len(lstAccMonth.SelectedItem.Value) = 1, "0" & Trim(lstAccYear.SelectedItem.Value), Trim(lstAccMonth.SelectedItem.Value))
        strAccYear = lstAccYear.SelectedItem.Value

        Response.Clear()
        Response.AddHeader("content-disposition", "attachment;filename=MONITOR.INVOICE-" & Trim(strLocation) & "-" & Trim(cAccMonth) & Trim(strAccYear) & ".xls")
        Response.Charset = ""
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.ContentType = "application/vnd.xls"

        Dim stringWrite = New System.IO.StringWriter()
        Dim htmlWrite = New HtmlTextWriter(stringWrite)

        dgInvoiceList.RenderControl(htmlWrite)
        Response.Write(stringWrite.ToString())
        Response.End()
    End Sub
#End Region

#Region "PROCEDURE"
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

    Sub UserMsgBox(ByVal F As Object, ByVal sMsg As String)
        Dim sb As New StringBuilder()
        Dim oFormObject As System.Web.UI.Control = Nothing
        Try
            sMsg = sMsg.Replace("'", "\'")
            sMsg = sMsg.Replace(Chr(34), "\" & Chr(34))
            sMsg = sMsg.Replace(vbCrLf, "\n")
            sMsg = "<script language='javascript'>alert('" & sMsg & "');</script>"
            sb = New StringBuilder()
            sb.Append(sMsg)
            For Each oFormObject In F.Controls
                If TypeOf oFormObject Is HtmlForm Then
                    Exit For
                End If
            Next
            oFormObject.Controls.AddAt(oFormObject.Controls.Count, New LiteralControl(sb.ToString()))
        Catch ex As Exception

        End Try
    End Sub

    Sub BindGrid()
        Dim PageNo As Integer
        Dim PageCount As Integer
        Dim dsData As DataSet
        Dim lbl As Label

        dsData = LoadData()
        PageCount = objGlobal.mtdGetPageCount(dsData.Tables(0).Rows.Count, dgContractList.PageSize)

        dgContractList.DataSource = dsData.Tables(0)
        If dgContractList.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgContractList.CurrentPageIndex = 0
            Else
                dgContractList.CurrentPageIndex = PageCount - 1
            End If
        End If

        dgContractList.DataBind()

        For intCnt = 0 To dgContractList.Items.Count - 1
            lbl = dgContractList.Items.Item(intCnt).FindControl("lblContractNo")
            If Trim(lbl.Text) = "TOTAL" Then
                lbl.Font.Bold = True
                lbl = dgContractList.Items.Item(intCnt).FindControl("lblContractDate")
                lbl.Visible = False
                lbl = dgContractList.Items.Item(intCnt).FindControl("lblProductCode")
                lbl.Visible = False
                lbl = dgContractList.Items.Item(intCnt).FindControl("lblPrice")
                lbl.Visible = False
                lbl = dgContractList.Items.Item(intCnt).FindControl("lblQtyContract")
                lbl.Font.Bold = True
                lbl = dgContractList.Items.Item(intCnt).FindControl("lblAmount")
                lbl.Font.Bold = True
                lbl = dgContractList.Items.Item(intCnt).FindControl("lblOA")
                lbl.Visible = False
                lbl = dgContractList.Items.Item(intCnt).FindControl("lblDOQty")
                lbl.Font.Bold = True
                lbl = dgContractList.Items.Item(intCnt).FindControl("lblConDisp")
                lbl.Font.Bold = True
                lbl = dgContractList.Items.Item(intCnt).FindControl("lblConDispTD")
                lbl.Font.Bold = True
                lbl = dgContractList.Items.Item(intCnt).FindControl("lblConDispBal")
                lbl.Font.Bold = True
                lbl = dgContractList.Items.Item(intCnt).FindControl("lblBuyerQty")
                lbl.Font.Bold = True
                lbl = dgContractList.Items.Item(intCnt).FindControl("lblBuyerQtyTD")
                lbl.Font.Bold = True
                lbl = dgContractList.Items.Item(intCnt).FindControl("lblBuyerQtyBal")
                lbl.Font.Bold = True
                lbl = dgContractList.Items.Item(intCnt).FindControl("lblAmountConBI")
                lbl.Font.Bold = True
                lbl = dgContractList.Items.Item(intCnt).FindControl("lblAmountConSBI")
                lbl.Font.Bold = True
                lbl = dgContractList.Items.Item(intCnt).FindControl("lblAmountConBal")
                lbl.Font.Bold = True
                lbl = dgContractList.Items.Item(intCnt).FindControl("lblClaimQty")
                lbl.Font.Bold = True
                lbl = dgContractList.Items.Item(intCnt).FindControl("lblClaimAmount")
                lbl.Font.Bold = True
            End If

            lbl = dgContractList.Items.Item(intCnt).FindControl("lblLastDelivered")
            If Trim(lbl.Text) = "01 Jan 1901" Then
                lbl.Visible = False
            End If
            lbl = dgContractList.Items.Item(intCnt).FindControl("lblLastReceived")
            If Trim(lbl.Text) = "01 Jan 1901" Then
                lbl.Visible = False
            End If
        Next
    End Sub

    Protected Function LoadData() As DataSet
        Dim objItemDs As New Object()
        Dim strOpCode As String = "CM_CLSTRX_CONTRACT_MONITORING_GET"

        Dim strSearchLocLevel As String = ""
        Dim strParamName As String
        Dim strParamValue As String
        Dim objData As New Object
        Dim StatusLn As String = ""
        Dim PrLevel As String = ""
        Dim strSrchContractNo As String
        Dim strSrchDeliveryNo As String
        Dim strSrchDateIn As String
        Dim strSrchCustomer As String
        Dim strSrchProduct As String
        Dim strSrchStatus As String
        Dim strSearchMonth As String
        Dim strSearchYear As String
        Dim strSearch As String

        If lstAccMonth.SelectedItem.Value = "0" Then
            strSearchMonth = ""
        Else
            strSearchMonth = lstAccMonth.SelectedItem.Value
        End If

        strSrchContractNo = IIf(srchContractNo.Text = "", "", " AND c.ContractNo like '%" & srchContractNo.Text & "%' ")
        strSrchDeliveryNo = IIf(srchDO.Text = "", "", " AND DONo like '%" & srchDO.Text & "%' ")
        strSrchCustomer = IIf(srchCust.Text = "", "", " AND (bl.BillPartyCode LIKE '%" & Trim(srchCust.Text) & "%' OR bl.Name LIKE '%" & Trim(srchCust.Text) & "%') ")
        strSrchProduct = IIf(srchProductList.SelectedItem.Value = objWMTrx.EnumWeighBridgeTicketProduct.All, "", " AND c.ProductCode='" & srchProductList.SelectedItem.Value & "' ")
        strSrchStatus = IIf(srchStatusList.SelectedItem.Value = objWMTrx.EnumWeighBridgeTicketStatus.All, "", srchStatusList.SelectedItem.Value)
        strSearch = strSrchContractNo & strSrchDeliveryNo & strSrchCustomer & strSrchProduct

        strParamName = "ACCYEAR|ACCMONTH|STRSEARCH|STATUS"
        strParamValue = lstAccYear.SelectedItem.Value & "|" & strSearchMonth & "|" & _
                        Trim(strSearch) & "|" & strSrchStatus

        'strParamName = "ACCYEAR|ACCMONTH|LOC|CUST|CON|DO|STATUS|STRSEARCH"
        'strParamValue = lstAccYear.SelectedItem.Value & "|" & lstAccMonth.SelectedItem.Value & "|" & strLocation & "|" & _
        '                Trim(srchCust.Text) & "|" & Trim(srchContractNo.Text) & "|" & Trim(srchDO.Text) & "|" & srchStatusList.SelectedItem.Value & "|" & _
        '                Trim(strSearch)

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
                                                strParamName, _
                                                strParamValue, _
                                                objItemDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=IN_ITEM_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        intPRCount = objItemDs.Tables(0).Rows.Count
        If objItemDs.Tables(0).Rows.Count > 0 Then
            cbExcelContract.Visible = True
            ContractListPreview.Visible = True
        Else
            cbExcelContract.Visible = False
            ContractListPreview.Visible = False
        End If

        Return objItemDs
    End Function

    Sub BindGridDO()
        Dim PageNo As Integer
        Dim PageCount As Integer
        Dim dsData As DataSet
        Dim lblPOID As Label
        Dim lblGRID As Label
        Dim lbl As Label
        Dim btn As Button

        dsData = LoadDataDO()
        PageCount = objGlobal.mtdGetPageCount(dsData.Tables(0).Rows.Count, dgContractList.PageSize)

        dgDOList.DataSource = dsData.Tables(1)
        If dgDOList.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgDOList.CurrentPageIndex = 0
            Else
                dgDOList.CurrentPageIndex = PageCount - 1
            End If
        End If

        dgDOList.DataBind()

        For intCnt = 0 To dgDOList.Items.Count - 1
            lblPOID = dgDOList.Items.Item(intCnt).FindControl("lblPOID")
            lblGRID = dgDOList.Items.Item(intCnt).FindControl("lblGRID")
            btn = dgDOList.Items.Item(intCnt).FindControl("btnGenerate")
            btn.Attributes("onclick") = "javascript:return ConfirmAction('generate goods receive');"

            If Trim(lblGRID.Text) <> "" Then
                btn.Visible = False
                'lblPOID.Visible = True
                lblGRID.Visible = True
            Else
                btn.Visible = True
                'lblPOID.Visible = False
                lblGRID.Visible = False
            End If


            lbl = dgDOList.Items.Item(intCnt).FindControl("lblDONo")
            If Trim(lbl.Text) = "TOTAL" Then
                btn.Visible = False
                lbl.Font.Bold = True
                lbl = dgDOList.Items.Item(intCnt).FindControl("lblDODate")
                lbl.Visible = False
                lbl = dgDOList.Items.Item(intCnt).FindControl("lblProductCode")
                lbl.Visible = False
                lbl = dgDOList.Items.Item(intCnt).FindControl("lblPrice")
                lbl.Visible = False
                lbl = dgDOList.Items.Item(intCnt).FindControl("lblToleransi")
                lbl.Visible = False
                lbl = dgDOList.Items.Item(intCnt).FindControl("lblQtyContract")
                lbl.Font.Bold = True
                lbl = dgDOList.Items.Item(intCnt).FindControl("lblAmount")
                lbl.Font.Bold = True
                lbl = dgDOList.Items.Item(intCnt).FindControl("lblDOPrice")
                lbl.Visible = False
                lbl = dgDOList.Items.Item(intCnt).FindControl("lblDOQty")
                lbl.Font.Bold = True
                lbl = dgDOList.Items.Item(intCnt).FindControl("lblConDisp")
                lbl.Font.Bold = True
                lbl = dgDOList.Items.Item(intCnt).FindControl("lblConDispTD")
                lbl.Font.Bold = True
                lbl = dgDOList.Items.Item(intCnt).FindControl("lblConDispBal")
                lbl.Font.Bold = True
                lbl = dgDOList.Items.Item(intCnt).FindControl("lblBuyerQty")
                lbl.Font.Bold = True
                lbl = dgDOList.Items.Item(intCnt).FindControl("lblBuyerQtyTD")
                lbl.Font.Bold = True
                lbl = dgDOList.Items.Item(intCnt).FindControl("lblBuyerQtyBal")
                lbl.Font.Bold = True
                lbl = dgDOList.Items.Item(intCnt).FindControl("lblAmountOABI")
                lbl.Font.Bold = True
                lbl = dgDOList.Items.Item(intCnt).FindControl("lblAmountOASBI")
                lbl.Font.Bold = True
                lbl = dgDOList.Items.Item(intCnt).FindControl("lblAmountOABal")
                lbl.Font.Bold = True
            End If
        Next
    End Sub

    Protected Function LoadDataDO() As DataSet
        Dim objItemDs As New Object()
        Dim strOpCode As String = "CM_CLSTRX_CONTRACT_MONITORING_GET"

        Dim strParamName As String
        Dim strParamValue As String
        Dim objData As New Object
        Dim StatusLn As String = ""
        Dim PrLevel As String = ""
        Dim strSrchContractNo As String
        Dim strSrchDeliveryNo As String
        Dim strSrchDateIn As String
        Dim strSrchCustomer As String
        Dim strSrchProduct As String
        Dim strSrchStatus As String
        Dim strSearchMonth As String
        Dim strSearchYear As String
        Dim strSearch As String

        If lstAccMonth.SelectedItem.Value = "0" Then
            strSearchMonth = ""
        Else
            strSearchMonth = lstAccMonth.SelectedItem.Value
        End If

        strSrchContractNo = IIf(srchContractNo.Text = "", "", " AND c.ContractNo like '%" & srchContractNo.Text & "%' ")
        strSrchDeliveryNo = IIf(srchDO.Text = "", "", " AND DONo like '%" & srchDO.Text & "%' ")
        strSrchCustomer = IIf(srchCust.Text = "", "", " AND (bl.BillPartyCode LIKE '%" & Trim(srchCust.Text) & "%' OR bl.Name LIKE '%" & Trim(srchCust.Text) & "%') ")
        strSrchProduct = IIf(srchProductList.SelectedItem.Value = objWMTrx.EnumWeighBridgeTicketProduct.All, "", " AND c.ProductCode='" & srchProductList.SelectedItem.Value & "' ")
        strSrchStatus = IIf(srchStatusList.SelectedItem.Value = objWMTrx.EnumWeighBridgeTicketStatus.All, "", srchStatusList.SelectedItem.Value)
        strSearch = strSrchContractNo & strSrchDeliveryNo & strSrchCustomer & strSrchProduct

        strParamName = "ACCYEAR|ACCMONTH|STRSEARCH|STATUS"
        strParamValue = lstAccYear.SelectedItem.Value & "|" & strSearchMonth & "|" & _
                        Trim(strSearch) & "|" & strSrchStatus

        'strParamName = "ACCYEAR|ACCMONTH|LOC|CUST|CON|DO|STATUS|STRSEARCH"
        'strParamValue = lstAccYear.SelectedItem.Value & "|" & lstAccMonth.SelectedItem.Value & "|" & strLocation & "|" & _
        '                Trim(srchCust.Text) & "|" & Trim(srchContractNo.Text) & "|" & Trim(srchDO.Text) & "|" & srchStatusList.SelectedItem.Value & "|" & _
        '                Trim(strSearch)

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
                                                strParamName, _
                                                strParamValue, _
                                                objItemDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=IN_ITEM_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        intPRCount = objItemDs.Tables(0).Rows.Count
        If objItemDs.Tables(0).Rows.Count > 0 Then
            cbExcelDO.Visible = True
            DoListPreview.Visible = True
        Else
            cbExcelDO.Visible = False
            DoListPreview.Visible = False
        End If

        Return objItemDs
    End Function

    Sub BindGridInvoice()
        Dim PageNo As Integer
        Dim PageCount As Integer
        Dim dsData As DataSet
        Dim lbl As Label

        dsData = LoadDataInvoice()
        PageCount = objGlobal.mtdGetPageCount(dsData.Tables(1).Rows.Count, dgContractList.PageSize)

        dgInvoiceList.DataSource = dsData.Tables(2)
        If dgInvoiceList.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgInvoiceList.CurrentPageIndex = 0
            Else
                dgInvoiceList.CurrentPageIndex = PageCount - 1
            End If
        End If

        dgInvoiceList.DataBind()

        For intCnt = 0 To dgInvoiceList.Items.Count - 1
            lbl = dgInvoiceList.Items.Item(intCnt).FindControl("lblNoUrut")
            If Trim(lbl.Text) <> "1" Then
                lbl = dgInvoiceList.Items.Item(intCnt).FindControl("lblContractNo")
                lbl.Visible = False
                lbl = dgInvoiceList.Items.Item(intCnt).FindControl("lblContractDate")
                lbl.Visible = False
                lbl = dgInvoiceList.Items.Item(intCnt).FindControl("lblCustomer")
                lbl.Visible = False
                lbl = dgInvoiceList.Items.Item(intCnt).FindControl("lblNPWP")
                lbl.Visible = False
                lbl = dgInvoiceList.Items.Item(intCnt).FindControl("lblProductCode")
                lbl.Visible = False
                lbl = dgInvoiceList.Items.Item(intCnt).FindControl("lblQtyContract")
                lbl.Visible = False
                lbl = dgInvoiceList.Items.Item(intCnt).FindControl("lblPrice")
                lbl.Visible = False
                lbl = dgInvoiceList.Items.Item(intCnt).FindControl("lblContractAmount")
                lbl.Visible = False
            End If

            lbl = dgInvoiceList.Items.Item(intCnt).FindControl("lblInvoiceDate")
            If Trim(lbl.Text) = "01 Jan 1901" Then
                lbl.Visible = False
            End If
            lbl = dgInvoiceList.Items.Item(intCnt).FindControl("lblInvoiceTaxDate")
            If Trim(lbl.Text) = "01 Jan 1901" Then
                lbl.Visible = False
            End If
        Next
    End Sub

    Protected Function LoadDataInvoice() As DataSet
        Dim objItemDs As New Object()
        Dim strOpCode As String = "CM_CLSTRX_CONTRACT_MONITORING_GET"

        Dim strSearchLocLevel As String = ""
        Dim strParamName As String
        Dim strParamValue As String
        Dim objData As New Object
        Dim StatusLn As String = ""
        Dim PrLevel As String = ""       
        Dim strSrchContractNo As String
        Dim strSrchDeliveryNo As String
        Dim strSrchDateIn As String
        Dim strSrchCustomer As String
        Dim strSrchProduct As String
        Dim strSrchStatus As String
        Dim strSearchMonth As String
        Dim strSearchYear As String
        Dim strSearch As String

        If lstAccMonth.SelectedItem.Value = "0" Then
            strSearchMonth = ""
        Else
            strSearchMonth = lstAccMonth.SelectedItem.Value
        End If

        strSrchContractNo = IIf(srchContractNo.Text = "", "", " AND c.ContractNo like '%" & srchContractNo.Text & "%' ")
        strSrchDeliveryNo = IIf(srchDO.Text = "", "", " AND DONo like '%" & srchDO.Text & "%' ")
        strSrchCustomer = IIf(srchCust.Text = "", "", " AND (bl.BillPartyCode LIKE '%" & Trim(srchCust.Text) & "%' OR bl.Name LIKE '%" & Trim(srchCust.Text) & "%') ")
        strSrchProduct = IIf(srchProductList.SelectedItem.Value = objWMTrx.EnumWeighBridgeTicketProduct.All, "", " AND c.ProductCode='" & srchProductList.SelectedItem.Value & "' ")
        strSrchStatus = IIf(srchStatusList.SelectedItem.Value = objWMTrx.EnumWeighBridgeTicketStatus.All, "", srchStatusList.SelectedItem.Value)
        strSearch = strSrchContractNo & strSrchDeliveryNo & strSrchCustomer & strSrchProduct

        strParamName = "ACCYEAR|ACCMONTH|STRSEARCH|STATUS"
        strParamValue = lstAccYear.SelectedItem.Value & "|" & strSearchMonth & "|" & _
                        Trim(strSearch) & "|" & strSrchStatus

        'strParamName = "ACCYEAR|ACCMONTH|LOC|CUST|CON|DO|STATUS|STRSEARCH"
        'strParamValue = lstAccYear.SelectedItem.Value & "|" & lstAccMonth.SelectedItem.Value & "|" & strLocation & "|" & _
        '                Trim(srchCust.Text) & "|" & Trim(srchContractNo.Text) & "|" & Trim(srchDO.Text) & "|" & srchStatusList.SelectedItem.Value & "|" & _
        '                Trim(strSearch)

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
                                                strParamName, _
                                                strParamValue, _
                                                objItemDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=IN_ITEM_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        intPRCount = objItemDs.Tables(1).Rows.Count

        If objItemDs.Tables(1).Rows.Count > 0 Then
            cbExcelInvoice.Visible = True
            InvoiceListPreview.Visible = True
        Else
            cbExcelInvoice.Visible = False
            InvoiceListPreview.Visible = False
        End If

        Return objItemDs
    End Function

    Sub GRGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strOpCd As String = "CM_CLSTRX_CONTRACT_MONITORING_GENERATE_GR"
        Dim objItemDs As New Object()
        Dim intCnt As Integer = 0
        Dim intErrNo As Integer
        Dim intSelectedIndex As Integer = 0
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim strDONo As String = ""
        Dim strTransCode As String = ""
        Dim dbDOQty As Double
        Dim dbDOPrice As Double
        Dim dbDOAmount As Double
        Dim indDate As String = ""
        Dim bUpdate As Boolean = False


        Dim btn As Button = CType(sender, Button)
        Dim dgItem As DataGridItem = CType(btn.NamingContainer, DataGridItem)

        strDONo = CType(dgItem.Cells(0).FindControl("lblDONo"), Label).Text
        strTransCode = CType(dgItem.Cells(0).FindControl("lblTransporterCode"), Label).Text
        dbDOQty = CDbl(CType(dgItem.Cells(0).FindControl("lblBuyerQtyTD"), Label).Text)
        dbDOPrice = CDbl(CType(dgItem.Cells(0).FindControl("lblDOPrice"), Label).Text)
        dbDOAmount = CDbl(CType(dgItem.Cells(0).FindControl("lblAmountOASBI"), Label).Text)
        btn = dgDOList.Items.Item(intCnt).FindControl("btnGenerate")

        strParamName = "LOCCODE|DONO|TRANSPORTER|DOQTY|ACCMONTH|ACCYEAR|UPDATEID"
        strParamValue = strLocation & "|" & strDONo & "|" & strTransCode & "|" & dbDOQty & "|" & lstAccMonth.SelectedItem.Value & "|" & lstAccYear.SelectedItem.Value & "|" & strUserId

        bUpdate = False
        Try
            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd, _
                                                    strParamName, _
                                                    strParamValue)
            bUpdate = True
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADD_DETAIL&errmesg=" & lblErrMessage.Text & "&redirect=IN/trx/IN_PurReq.aspx")
        End Try

        BindGridDO()

        If bUpdate = True Then
            UserMsgBox(Me, "Generate GR sucsess...!!!")
            btn.Enabled = False
        End If

    End Sub

    Sub dgContractList_BindGrid(ByVal sender As Object, ByVal e As DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor='DarkSeaGreen'")
            If e.Item.ItemType = ListItemType.AlternatingItem Then
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='e9e9e9'")
            Else
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='f2f2f2'")
            End If
        End If
    End Sub

    Private Sub dgContractList_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgContractList.ItemCreated
        If e.Item.ItemType = ListItemType.Header Then
            Dim dgItem As DataGridItem
            Dim dgCell As TableCell
            dgItem = New DataGridItem(0, 0, ListItemType.Header)

            dgCell = New TableCell()
            dgCell.ColumnSpan = 7
            dgItem.Cells.Add(dgCell)
            dgCell.Text = "CONTRACT"
            dgCell.HorizontalAlign = HorizontalAlign.Center

            dgCell = New TableCell()
            dgCell.ColumnSpan = 3
            dgItem.Cells.Add(dgCell)
            dgCell.Text = "DELIVERY ORDER"
            dgCell.HorizontalAlign = HorizontalAlign.Center

            dgCell = New TableCell()
            dgCell.ColumnSpan = 2
            dgItem.Cells.Add(dgCell)
            dgCell.Text = "TERMS"
            dgCell.HorizontalAlign = HorizontalAlign.Center

            dgCell = New TableCell()
            dgCell.ColumnSpan = 3
            dgItem.Cells.Add(dgCell)
            dgCell.Text = "TONASE <BR> ON SELLER WB"
            dgCell.HorizontalAlign = HorizontalAlign.Center

            dgCell = New TableCell()
            dgCell.ColumnSpan = 3
            dgItem.Cells.Add(dgCell)
            dgCell.Text = "TONASE <BR> ON CUSTOMER WB"
            dgCell.HorizontalAlign = HorizontalAlign.Center

            dgCell = New TableCell()
            dgCell.ColumnSpan = 3
            dgItem.Cells.Add(dgCell)
            dgCell.Text = "SALES INCOME"
            dgCell.HorizontalAlign = HorizontalAlign.Center

            dgCell = New TableCell()
            dgCell.ColumnSpan = 2
            dgItem.Cells.Add(dgCell)
            dgCell.Text = "CLAIMED"
            dgCell.HorizontalAlign = HorizontalAlign.Center

            dgCell = New TableCell()
            dgCell.ColumnSpan = 2
            dgItem.Cells.Add(dgCell)
            dgCell.Text = "LAST DATE OF"
            dgCell.HorizontalAlign = HorizontalAlign.Center

            dgCell = New TableCell()
            dgCell.RowSpan = 2
            dgItem.Cells.Add(dgCell)
            dgCell.Text = "OA"
            dgCell.HorizontalAlign = HorizontalAlign.Center

            dgItem.Font.Bold = True
            dgContractList.Controls(0).Controls.AddAt(0, dgItem)
        End If
    End Sub

    Private Sub dgContractList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgContractList.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            Dim dgItem As DataGridItem

            dgItem = New DataGridItem(0, 0, ListItemType.Header)

            e.Item.Cells(25).Visible = False
        End If
    End Sub

    Sub dgDOList_BindGrid(ByVal sender As Object, ByVal e As DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor='DarkSeaGreen'")
            If e.Item.ItemType = ListItemType.AlternatingItem Then
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='e9e9e9'")
            Else
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='f2f2f2'")
            End If
        End If
    End Sub

    Private Sub dgDOList_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgDOList.ItemCreated
        If e.Item.ItemType = ListItemType.Header Then
            Dim dgItem As DataGridItem
            Dim dgCell As TableCell
            dgItem = New DataGridItem(0, 0, ListItemType.Header)

            dgCell = New TableCell()
            dgCell.ColumnSpan = 6
            dgItem.Cells.Add(dgCell)
            dgCell.Text = "DELIVERY ORDER"
            dgCell.HorizontalAlign = HorizontalAlign.Center

            dgCell = New TableCell()
            dgCell.ColumnSpan = 3
            dgItem.Cells.Add(dgCell)
            dgCell.Text = "TONASE <BR> ON SELLER WB"
            dgCell.HorizontalAlign = HorizontalAlign.Center

            dgCell = New TableCell()
            dgCell.ColumnSpan = 3
            dgItem.Cells.Add(dgCell)
            dgCell.Text = "TONASE <BR> ON CUSTOMER WB"
            dgCell.HorizontalAlign = HorizontalAlign.Center

            dgCell = New TableCell()
            dgCell.ColumnSpan = 4
            dgItem.Cells.Add(dgCell)
            dgCell.Text = "TRANSPORT FEE"
            dgCell.HorizontalAlign = HorizontalAlign.Center

            dgCell = New TableCell()
            dgCell.RowSpan = 2
            dgItem.Cells.Add(dgCell)
            dgCell.Text = ""
            dgCell.HorizontalAlign = HorizontalAlign.Center

            dgCell = New TableCell()
            dgCell.ColumnSpan = 6
            dgItem.Cells.Add(dgCell)
            dgCell.Text = "CONTRACT"
            dgCell.HorizontalAlign = HorizontalAlign.Center

            dgItem.Font.Bold = True
            dgDOList.Controls(0).Controls.AddAt(0, dgItem)
        End If
    End Sub

    Private Sub dgDOList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgDOList.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            Dim dgItem As DataGridItem

            dgItem = New DataGridItem(0, 0, ListItemType.Header)

            e.Item.Cells(16).Visible = False
            'e.Item.Cells(5).Visible = False
        End If
    End Sub

    Sub dgInvoiceList_BindGrid(ByVal sender As Object, ByVal e As DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor='DarkSeaGreen'")
            If e.Item.ItemType = ListItemType.AlternatingItem Then
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='e9e9e9'")
            Else
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='f2f2f2'")
            End If
        End If
    End Sub

    Private Sub dgInvoiceList_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgInvoiceList.ItemCreated
        If e.Item.ItemType = ListItemType.Header Then
            Dim dgItem As DataGridItem
            Dim dgCell As TableCell
            dgItem = New DataGridItem(0, 0, ListItemType.Header)

            dgCell = New TableCell()
            dgCell.ColumnSpan = 8
            dgItem.Cells.Add(dgCell)
            dgCell.Text = "CONTRACT"
            dgCell.HorizontalAlign = HorizontalAlign.Center

            dgCell = New TableCell()
            dgCell.ColumnSpan = 2
            dgItem.Cells.Add(dgCell)
            dgCell.Text = "INVOICE"
            dgCell.HorizontalAlign = HorizontalAlign.Center

            dgCell = New TableCell()
            dgCell.ColumnSpan = 4
            dgItem.Cells.Add(dgCell)
            dgCell.Text = "TAX"
            dgCell.HorizontalAlign = HorizontalAlign.Center

            dgItem.Font.Bold = True
            dgInvoiceList.Controls(0).Controls.AddAt(0, dgItem)
        End If
    End Sub

    Private Sub dgInvoiceList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgInvoiceList.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            Dim dgItem As DataGridItem

            dgItem = New DataGridItem(0, 0, ListItemType.Header)

            'e.Item.Cells(4).Visible = False
            'e.Item.Cells(5).Visible = False
        End If
    End Sub
#End Region
End Class