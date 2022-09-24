
Imports System
Imports System.Data
Imports System.Web.UI
Imports System.Math
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic


Public Class ap_trx_invrcv_wm_det : Inherits Page

    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblStatusHidden As Label
    Protected WithEvents txtInvID As TextBox
    Protected WithEvents reginvID As RequiredFieldValidator
    Protected WithEvents lblAccPeriod As Label
    Protected WithEvents txtRefNo As TextBox
    Protected WithEvents ReqRefNo As RequiredFieldValidator
    Protected WithEvents lblStatus As Label
    Protected WithEvents txtRefDate As TextBox
    Protected WithEvents btnSelDate As Image
    Protected WithEvents lblErrRefDate As Label
    Protected WithEvents lblLastUpdate As Label

    Protected WithEvents ddlSuppCode As DropDownList
    Protected WithEvents lblErrSuppCode As Label
    Protected WithEvents lblDateCreated As Label

    Protected WithEvents ddlAccCode As DropDownList
    Protected WithEvents lblErrAccCode As Label
    Protected WithEvents lblUpdatedBy As Label

    Protected WithEvents ddlTicket As DropDownList
    Protected WithEvents lblErrTicket As Label

    Protected WithEvents txtUnitPrice As TextBox
    Protected WithEvents lblErrUnitPrice As Label

    Protected WithEvents txtAmount As TextBox
    Protected WithEvents lblErrAmount As Label

    Protected WithEvents txtTotalWeight As TextBox

    Protected WithEvents AddDtlBtn As ImageButton
    Protected WithEvents lblConfirmErr As Label
    Protected WithEvents inrid As HtmlInputHidden

    Protected WithEvents dgLineDet As DataGrid

    Protected WithEvents lblTotAmt As Label
    Protected WithEvents SaveBtn As ImageButton
    Protected WithEvents ConfirmBtn As ImageButton
    Protected WithEvents DeleteBtn As ImageButton
    Protected WithEvents BackBtn As ImageButton
    Protected WithEvents PrintBtn As ImageButton
    Protected WithEvents tblSelection As System.Web.UI.HtmlControls.HtmlTable

    Protected WithEvents TrLink As HtmlTableRow
    Protected WithEvents lbViewJournal As LinkButton
    Protected WithEvents dgViewJournal As DataGrid
    Protected WithEvents lblTotalDB As Label
    Protected WithEvents lblTotalCR As Label
    Protected WithEvents lblTotalViewJournal As Label

    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()

    Protected objGLSetup As New agri.GL.clsSetup()
    Protected objGLTrx As New agri.GL.clsTrx()
    Protected objWMTrx As New agri.WM.clstrx()
    Protected objPUSetup As New agri.PU.clsSetup()


    Dim objAPTrx As New agri.AP.clsTrx()
    Dim objAdminShare As New agri.Admin.clsShare()

    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Dim objLangCapDs As DataSet

    Dim dsMaster As Object
    Dim dsLine As Object

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String

    Dim intAPAR As Integer
    Dim intAmount As Decimal

    Dim strTrxID As String
    Dim strAcceptDateFormat As String

    Dim intConfig As Integer
    Dim strLocType As String


    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)

        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strAccMonth = Session("SS_APACCMONTH")
        strAccYear = Session("SS_APACCYEAR")
        strLangCode = Session("SS_LANGCODE")
        intAPAR = Session("SS_APAR")
        intConfig = Session("SS_CONFIGSETTING")
        strLocType = Session("SS_LOCTYPE")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumAPAccessRights.APInvoiceReceive), intAPAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            lblErrRefDate.Visible = False
            lblErrSuppCode.Visible = False
            lblErrAccCode.Visible = False
            lblErrTicket.Visible = False
            lblConfirmErr.Visible = False
            lblErrAmount.Visible = False
            lblErrMessage.Visible = False

            strTrxID = Trim(IIf(Request.QueryString("trxID") = "", Request.Form("trxID"), Request.QueryString("trxID")))
            inrid.Value = strTrxID

            onload_GetLangCap()

            If Not IsPostBack Then

                If strTrxID <> "" Then
                    onLoad_Display(strTrxID)
                    onLoad_DisplayItem(strTrxID)
                    BindTiket()
                    onLoad_Button()
                Else
                    lblStatusHidden.Text = "0"
                    BindSupp("")
                    BindAccCode("")
                    onLoad_Button()
                End If
            End If

        End If
    End Sub


    Sub onload_GetLangCap()
        'GetEntireLangCap()


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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_TRX_CNDET_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcv_wm_list.aspx")
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


    Sub onLoad_Button()

        DeleteBtn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"

        SaveBtn.Visible = False
        ConfirmBtn.Visible = False
        DeleteBtn.Visible = False
        PrintBtn.Visible = False


        txtInvID.Enabled = False
        txtRefNo.Enabled = False
        txtRefDate.Enabled = False
        btnSelDate.Visible = False
        ddlSuppCode.Enabled = False
        ddlAccCode.Enabled = False
        tblSelection.Visible = True

        Select Case Trim(lblStatusHidden.Text)
            Case Trim(CStr(objAPTrx.EnumInvoiceRcvStatus.Confirmed))
                txtInvID.Enabled = False
                tblSelection.Visible = False
                PrintBtn.Visible = False
            Case Trim(CStr(objAPTrx.EnumInvoiceRcvStatus.Deleted))
                txtInvID.Enabled = False
                tblSelection.Visible = False
            Case Else
                SaveBtn.Visible = True
                txtInvID.Enabled = False
                txtRefNo.Enabled = True
                txtRefDate.Enabled = True
                btnSelDate.Visible = True
                ddlSuppCode.Enabled = True
                ddlAccCode.Enabled = True
                If Trim(txtInvID.Text) <> "" Then
                    txtInvID.Enabled = False
                    ddlSuppCode.Enabled = False
                    ConfirmBtn.Visible = True
                    DeleteBtn.Visible = True
                    PrintBtn.Visible = False
                End If
        End Select

    End Sub



    Sub onLoad_Display(ByVal pv_strId As String)

        Dim strOpCode As String = "AP_CLSTRX_WEIGHBRIDGE_INVOICE_GET"
        Dim intErrNo As Integer
        Dim strParamName As String = ""
        Dim strParamValue As String = ""

        strParamName = "TRXID"
        strParamValue = pv_strId

        Try
            intErrNo = objGLTrx.mtdGetDataCommon(strOpCode, _
                                                strParamName, _
                                                strParamValue, _
                                                dsMaster)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GENERATE_INVOICE_WM_GET_HEADER&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcv_wm_list.aspx")
        End Try

        txtInvID.Text = Trim(dsMaster.Tables(0).Rows(0).Item("TRXID"))
        txtRefNo.Text = Trim(dsMaster.Tables(0).Rows(0).Item("RefNo"))
        txtRefDate.Text = Date_Validation(dsMaster.Tables(0).Rows(0).Item("RefDate"), True)

        lblStatus.Text = objAPTrx.mtdGetInvoiceRcvStatus(Trim(dsMaster.Tables(0).Rows(0).Item("Status")))

        lblStatusHidden.Text = Trim(dsMaster.Tables(0).Rows(0).Item("Status"))
        lblDateCreated.Text = objGlobal.GetLongDate(dsMaster.Tables(0).Rows(0).Item("CreateDate"))
        lblLastUpdate.Text = objGlobal.GetLongDate(dsMaster.Tables(0).Rows(0).Item("UpdateDate"))
        lblUpdatedBy.Text = Trim(dsMaster.Tables(0).Rows(0).Item("UserName"))
        lblTotAmt.Text = FormatNumber(dsMaster.Tables(0).Rows(0).Item("TotalAmount"), CInt(Session("SS_ROUNDNO")))

        BindSupp(Trim(dsMaster.Tables(0).Rows(0).Item("SupplierCode")))
        BindAccCode(Trim(dsMaster.Tables(0).Rows(0).Item("AccCode")))

    End Sub

    Sub onLoad_DisplayItem(ByVal pv_strId As String)

        Dim strOpCode As String = "AP_CLSTRX_WEIGHBRIDGE_INVOICELN_GET"

        Dim intErrNo As Integer
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim intCnt As Integer

        strParamName = "TRXID"
        strParamValue = pv_strId

        Try
            intErrNo = objGLTrx.mtdGetDataCommon(strOpCode, _
                                                strParamName, _
                                                strParamValue, _
                                                dsLine)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GENERATE_INVOICE_WM_GET_HEADER&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcv_wm_list.aspx")
        End Try

        'For intCnt = 0 To dsLine.Tables(0).Rows.Count - 1

        '    dsLine.Tables(0).Rows(intCnt).Item("TicketNo") = Trim(dsLine.Tables(0).Rows(intCnt).Item("TicketNo"))
        '    dsLine.Tables(0).Rows(intCnt).Item("InDate") = Trim(dsLine.Tables(0).Rows(intCnt).Item("InDate"))
        '    dsLine.Tables(0).Rows(intCnt).Item("ProductCode") = dsLine.Tables(0).Rows(intCnt).Item("ProductCode")
        '    dsLine.Tables(0).Rows(intCnt).Item("NetWeight") = Trim(dsLine.Tables(0).Rows(intCnt).Item("NetWeight"))
        '    dsLine.Tables(0).Rows(intCnt).Item("Amount") = Trim(dsLine.Tables(0).Rows(intCnt).Item("Amount"))

        'Next intCnt

        dgLineDet.DataSource = dsLine.Tables(0)
        dgLineDet.DataBind()

        Dim lbl As Label
        For intCnt = 0 To dgLineDet.Items.Count - 1
            lbl = dgLineDet.Items.Item(intCnt).FindControl("lblNoUrut")
            If Trim(lbl.Text) = "999" Then
                lbl.Visible = False

                lbl = dgLineDet.Items.Item(intCnt).FindControl("lblInDate")
                lbl.Visible = False
                lbl = dgLineDet.Items.Item(intCnt).FindControl("lblPrice")
                lbl.Visible = False
                lbl = dgLineDet.Items.Item(intCnt).FindControl("lblProdCode")
                lbl.Visible = False
            End If
        Next
    End Sub

    Sub DataGrid_ItemData(ByVal Sender As Object, ByVal e As DataGridItemEventArgs) Handles dgLineDet.ItemDataBound
        Dim btn As LinkButton
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Select Case CInt(lblStatusHidden.Text)
                Case objAPTrx.EnumInvoiceRcvStatus.Confirmed, objAPTrx.EnumInvoiceRcvStatus.Deleted
                    btn = e.Item.FindControl("lbDelete")
                    btn.Visible = False
                Case Else
                    btn = e.Item.FindControl("lbDelete")
                    btn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
            End Select

        End If
    End Sub


    Sub BindSupp(ByVal pv_strSelectedSuppCode As String)

        Dim strOpCode As String = "PU_CLSSETUP_SUPPLIER_DDL_GET"
        Dim liTemp As ListItem
        Dim objSuppDs As New Object
        Dim strParam As String = ""
        Dim intCnt As Integer = 0
        Dim intErrNo As Integer
        Dim dr As DataRow
        Dim intSelectedIndex As Integer = 0

        'strParam = "||" & objPUSetup.EnumSuppStatus.Active & "||SupplierCode|||" & objPUSetup.EnumSupplierType.FFBSupplier
        strParam = "||" & objPUSetup.EnumSuppStatus.Active & "||SupplierCode||"
        'strParam = strParam & "|" & IIf(Session("SS_COACENTRALIZED") = "1", "", " A.AccCode in (SELECT AccCode FROM GL_Account WHERE LocCode='" & Trim(strLocation) & "') ")
        strParam = strParam & "|" & IIf(Session("SS_COACENTRALIZED") = "1", "", " SupplierCode LIKE '%" & Trim(strLocation) & "%'") ', " A.AccCode in (SELECT AccCode FROM GL_Account WHERE LocCode='" & Trim(strLocation) & "') ")
        strParam = strParam & "|" & objPUSetup.EnumSupplierType.FFBSupplier

        Try
            intErrNo = objPUSetup.mtdGetSupplier(strOpCode, strParam, objSuppDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PU_GET_SUPPLIER&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcv_wm_list")
        End Try
        dr = objSuppDs.Tables(0).NewRow()
        dr("SupplierCode") = ""
        dr("Name") = "Please Select Supplier Code"
        objSuppDs.Tables(0).Rows.InsertAt(dr, 0)
        ddlSuppCode.DataSource = objSuppDs.Tables(0)
        ddlSuppCode.DataValueField = "SupplierCode"
        ddlSuppCode.DataTextField = "Name"
        ddlSuppCode.DataBind()
        If pv_strSelectedSuppCode <> "" Then
            liTemp = ddlSuppCode.Items.FindByValue(pv_strSelectedSuppCode)
            If liTemp Is Nothing Then
                ddlSuppCode.Items.Add(New ListItem(pv_strSelectedSuppCode & " (Deleted)", pv_strSelectedSuppCode))
                intSelectedIndex = ddlSuppCode.Items.Count - 1
            Else
                intSelectedIndex = ddlSuppCode.Items.IndexOf(liTemp)
            End If
        End If

        ddlSuppCode.SelectedIndex = intSelectedIndex
    End Sub


    Sub BindTiket()

        Dim strOpCode As String = "AP_CLSTRX_WEIGHBRIDGE_INVOICE_SEARCH_BY_SUPPLIER"

        Dim dsResult As New Object
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim intErrNo As Integer
        Dim intSelectedIndex As Integer = 0
        Dim dr As DataRow
        Dim intCnt As Integer

        strParamName = "TRANSTYPE|CUSTCODE|LOCCODE"
        strParamValue = "1|" & ddlSuppCode.SelectedItem.Value & "|" & strLocation

        Try
            intErrNo = objGLTrx.mtdGetDataCommon(strOpCode, strParamName, strParamValue, dsResult)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PU_GET_SUPPLIER&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcv_wm_list")
        End Try

        For intCnt = 0 To dsResult.Tables(0).Rows.Count - 1
            dsResult.Tables(0).Rows(intCnt).Item("Descr") = dsResult.Tables(0).Rows(intCnt).Item("AccumDate") & _
                                                            " - " & dsResult.Tables(0).Rows(intCnt).Item("Weight")
                    
        Next intCnt

        dr = dsResult.Tables(0).NewRow()
        dr("TglMasuk") = ""
        dr("Descr") = "Please Select Date"
        dsResult.Tables(0).Rows.InsertAt(dr, 0)

        ddlTicket.DataSource = dsResult.Tables(0)
        ddlTicket.DataValueField = "TglMasuk"
        ddlTicket.DataTextField = "Descr"
        ddlTicket.DataBind()

        ddlTicket.SelectedIndex = intSelectedIndex

    End Sub


    Sub onSelect_Supp(ByVal sender As System.Object, ByVal e As System.EventArgs)

        BindTiket()

    End Sub

    Sub onSelect_Ticket(ByVal sender As System.Object, ByVal e As System.EventArgs)

        If Instr(ddlTicket.SelectedItem.Text, "-") > 0 Then
            txtTotalWeight.Text = Val(Mid(ddlTicket.SelectedItem.Text, Instr(ddlTicket.SelectedItem.Text, "-") + 1))
            txtAmount.Text = "0"
            txtUnitPrice.Text = "0"
        Else
            txtTotalWeight.Text = "0"
            txtAmount.Text = "0"
            txtUnitPrice.Text = "0"
        End If

    End Sub

    Function Date_Validation(ByVal pv_strInputDate As String, ByVal pv_blnIsShortDate As Boolean) As String

        Dim objSysCfgDs As New Object
        Dim objActualDate As New Object
        Dim strDateFormat As String
        Dim strParam As String
        Dim intErrNo As Integer

        strParam = "PWSYSTEM_CLSCONFIG_CONFIG_DATEFMT_GET"
        Try
            intErrNo = objSysCfg.mtdGetConfigInfo(strParam, _
                                                  strCompany, _
                                                  strLocation, _
                                                  strUserId, _
                                                  objSysCfgDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_GET_CONFIG&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcv_wm_list")
        End Try

        strDateFormat = objSysCfg.mtdGetDateFormat(Trim(objSysCfgDs.Tables(0).Rows(0).Item("Datefmt")))

        If pv_blnIsShortDate Then
            Date_Validation = objGlobal.GetShortDate(strDateFormat, pv_strInputDate)
        Else
            If objGlobal.mtdValidInputDate(strDateFormat, _
                                            pv_strInputDate, _
                                            strAcceptDateFormat, _
                                            objActualDate) = True Then
                Date_Validation = objActualDate
            Else
                Date_Validation = ""
            End If
        End If

    End Function

    Sub BindAccCode(ByVal pv_strAccCode As String)

        Dim objAccDs As New Object()
        Dim strOpCode_GetAcc As String = "GL_CLSSETUP_ACCOUNTCODE_LIST_GET"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer = 0

        Try
            strParam = "Order By ACC.AccCode|And ACC.Status = '" & _
                        objGLSetup.EnumAccountCodeStatus.Active & _
                        "' And ACC.AccType in ('" & objGLSetup.EnumAccountType.BalanceSheet & "','" & objGLSetup.EnumAccountType.ProfitandLost & "')"
            intErrNo = objGLSetup.mtdGetMasterList(strOpCode_GetAcc, strParam, objGLSetup.EnumGLMasterType.AccountCode, objAccDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CREDITNOTE_GET_SUPP2&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcv_wm_list")
        End Try

        For intCnt = 0 To objAccDs.Tables(0).Rows.Count - 1
            objAccDs.Tables(0).Rows(intCnt).Item("AccCode") = Trim(objAccDs.Tables(0).Rows(intCnt).Item("AccCode"))
            objAccDs.Tables(0).Rows(intCnt).Item("Description") = objAccDs.Tables(0).Rows(intCnt).Item("AccCode") & " (" & Trim(objAccDs.Tables(0).Rows(intCnt).Item("Description")) & ")"
            If objAccDs.Tables(0).Rows(intCnt).Item("AccCode") = Trim(pv_strAccCode) Then
                intSelectedIndex = intCnt + 1
            End If
        Next intCnt


        Dim dr As DataRow
        dr = objAccDs.Tables(0).NewRow()
        dr("AccCode") = ""
        dr("Description") = "Please Select Akun Kode"
        objAccDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlAccCode.DataSource = objAccDs.Tables(0)
        ddlAccCode.DataValueField = "AccCode"
        ddlAccCode.DataTextField = "Description"
        ddlAccCode.DataBind()
        ddlAccCode.SelectedIndex = intSelectedIndex
    End Sub


    Sub SaveBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)

        If SaveMaster = False Then
            lblErrMessage.Visible = True
            lblErrMessage.Text = "Please Check Your Data"
            Exit Sub
        End If

        'Refresh
        onLoad_Display(txtInvID.Text)
        onLoad_Button()

    End Sub


    Private Function SaveMaster() As Boolean

        Dim strOpCode As String
        Dim strRefDate As String
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim intErrNo As Integer
        Dim strNewIDFormat As String
        Dim dsData As New Object

        SaveMaster = False
        'Validate Input
        strRefDate = TRIM(txtRefDate.Text)

        If strRefDate <> "" Then
            strRefDate = Date_Validation(strRefDate, False)
            If strRefDate = "" Then
                lblErrRefDate.Visible = True
                lblErrRefDate.Text = lblErrRefDate.Text & strAcceptDateFormat
                Exit Function
            End If
        Else
            lblErrRefDate.Visible = True
            lblErrRefDate.Text = lblErrRefDate.Text & strAcceptDateFormat
            Exit Function
        End If

        If ddlSuppCode.SelectedIndex = 0 Then
            lblErrSuppCode.Visible = True
            Exit Function
        End If

        If ddlAccCode.SelectedIndex = 0 Then
            lblErrAccCode.Visible = True
            Exit Function
        End If

        strAccMonth = Month(strRefDate)
        strAccYear = Year(strRefDate)

        strNewIDFormat = "INV" & "/" & strCompany & "/" & strLocation & "/" & "W" & "/" & strAccYear & IIf(Len(Trim(strAccMonth)) = 1, "0" & strAccMonth, strAccMonth) & "/"

        strParamName = "TRXID|REFNO|REFDATE|SUPPLIERCODE|ACCCODE|STATUS|LOCCODE|USERID|NEWIDFORMAT"
        strParamValue = txtInvID.Text & "|" & txtRefNo.Text & "|" & strRefDate & "|" & _
                        ddlSuppCode.SelectedValue & "|" & ddlAccCode.SelectedValue & "|" & _
                        objAPTrx.EnumInvoiceRcvStatus.Active & "|" & _
                        strLocation & "|" & strUserId & "|" & strNewIDFormat

        If CInt(lblStatusHidden.Text) = 0 Then
            strOpCode = "AP_CLSTRX_WEIGHBRIDGE_INVOICE_ADD"
        Else
            strOpCode = "AP_CLSTRX_WEIGHBRIDGE_INVOICE_UPD"
        End If

        Try
            intErrNo = objGLTrx.mtdGetDataCommon(strOpCode, _
                                                   strParamName, _
                                                   strParamValue, dsData)

            If intErrNo <> 0 Then
                SaveMaster = False
            Else
                SaveMaster = True
            End If

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_UPD_INVOICERCV&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcv_wm_list")
        End Try

        If CInt(lblStatusHidden.Text) = 0 Then
            txtInvID.Text = dsData.Tables(0).Rows(0).Item("TrxID")
        Else
            txtInvID.Text = txtInvID.Text
        End If

    End Function

    Sub ConfirmBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)

        Dim strOpCode As String = "AP_CLSTRX_WEIGHBRIDGE_INVOICE_CONFIRM"
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim intErrNo As Integer

        strParamName = "TRXID|STATUS|USERID|LOCCODE|ACCMONTH|ACCYEAR"
        strParamValue = txtInvID.Text & "|" & objAPTrx.EnumInvoiceRcvStatus.Confirmed & _
                        "|" & strUserId & "|" & strLocation & "|" & strAccMonth & "|" & strAccYear

        Try
            intErrNo = objGLTrx.mtdInsertDataCommon(strOpCode, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GENERATE_CONFIRM&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcv_wm_list")
        End Try

        'Refresh
        onLoad_Display(txtInvID.Text)
        onLoad_DisplayItem(Trim(txtInvID.Text))
        onLoad_Button()


    End Sub


    Sub DeleteBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)

        Dim strOpCode As String
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim intErrNo As Integer

        strParamName = "TRXID|STATUS|USERID"
        strParamValue = txtInvID.Text & "|" & objAPTrx.EnumInvoiceRcvStatus.Deleted & "|" & strUserId

        strOpCode = "AP_CLSTRX_WEIGHBRIDGE_INVOICE_DEL"

        Try
            intErrNo = objGLTrx.mtdInsertDataCommon(strOpCode, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GENERATE_CONFIRM&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcv_wm_list")
        End Try

        'Refresh
        onLoad_Display(txtInvID.Text)
        onLoad_DisplayItem(Trim(txtInvID.Text))
        BindTiket()
        onLoad_Button()

    End Sub


    Sub AddDtlBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)

        Dim strOpCode As String = "AP_CLSTRX_WEIGHBRIDGE_INVOICELN_ADD"
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim intErrNo As Integer
        Dim strRefDate As String

        'Validate Input
        If ddlTicket.SelectedIndex = 0 Then
            lblErrTicket.Visible = True
            Exit Sub
        End If

        If Val(txtUnitPrice.Text) = 0 Then
            lblErrAmount.Visible = True
            Exit Sub
        End If

        'CHECK IF MASTER NOT SAVE YET  -----------------------
        If CInt(lblStatusHidden.Text) = 0 Then
            If SaveMaster = False Then
                lblErrMessage.Visible = True
                lblErrMessage.Text = "Please Check Your Data"
                Exit Sub
            End If
        End If
        '----------------------------------------------------

        '--------------- 11 Maret 2009 ----------------------------------
        'bandingkan periode bulan pada tanggal master harus sama dengan
        'tanggal bulan pada data detail
        strRefDate = TRIM(txtRefDate.Text)
        If strRefDate <> "" Then
            strRefDate = Date_Validation(strRefDate, False)
        End If

        'If Val(Mid(ddlTicket.SelectedValue, 3, CInt(Session("SS_ROUNDNO")))) <> Month(strRefDate) Then
        '    lblErrMessage.Visible = True
        '    lblErrMessage.Text = "Periode bulan tiket harus sama dengan periode pada tanggal transaksi "
        '    Exit Sub
        'End If

        strParamName = "TRXID|TICKETNO|AMOUNT|USERID|CUSTCODE|INDATE"

        strParamValue = Trim(txtInvID.Text) & "|" & ddlTicket.SelectedValue & _
                        "|" & Val(txtAmount.Text) & "|" & strUserId & "|" & ddlSuppCode.SelectedValue & "|" & _
                        ddlTicket.SelectedItem.Value

        Try
            intErrNo = objGLTrx.mtdInsertDataCommon(strOpCode, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADD_DETAIL&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcv_wm_list")
        End Try

        'Refresh
        onLoad_Display(Trim(txtInvID.Text))
        onLoad_DisplayItem(Trim(txtInvID.Text))
        onLoad_Button()

    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs) Handles dgLineDet.DeleteCommand

        Dim strOpCode As String = "AP_CLSTRX_WEIGHBRIDGE_INVOICELN_DEL"
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim strLineID As String
        Dim strAmount As String
        Dim intErrNo As Integer
        Dim lblDelText As Label
        Dim lblAmount As Label
        Dim lblWeight As Label
        Dim strWeight As String

        dgLineDet.EditItemIndex = Convert.ToInt16(E.Item.ItemIndex)
        lblDelText = dgLineDet.Items.Item(Convert.ToInt16(E.Item.ItemIndex)).FindControl("lnid")
        strLineID = lblDelText.Text
        lblAmount = dgLineDet.Items.Item(Convert.ToInt16(E.Item.ItemIndex)).FindControl("lnamount")
        strAmount = lblAmount.Text
        lblWeight = dgLineDet.Items.Item(Convert.ToInt16(E.Item.ItemIndex)).FindControl("lnweight")
        strWeight = lblWeight.Text

        strParamName = "TRXID|LINEID|AMOUNT|USERID|WEIGHT"

        strParamValue = Trim(txtInvID.Text) & "|" & strLineID & "|" & Val(strAmount) & "|" & strUserId & "|" & strWeight

        Try
            intErrNo = objGLTrx.mtdInsertDataCommon(strOpCode, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADD_DETAIL&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcv_wm_list")
        End Try

        'Refresh
        onLoad_Display(Trim(txtInvID.Text))
        onLoad_DisplayItem(Trim(txtInvID.Text))
        onLoad_Button()

    End Sub

    Sub BackBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("ap_trx_invrcv_wm_list.aspx")
    End Sub


    Sub PrintBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)

        Dim strTRXID As String

        strTRXID = Trim(txtInvID.Text)

        Response.Write("<Script Language=""JavaScript"">window.open(""../reports/AP_Rpt_InvRcv_WM_Det.aspx?Type=Print&CompName=" & strCompany & _
                       "&Location=" & strLocation & _
                       "&TRXID=" & strTRXID & _
                       """,null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")


    End Sub

    Private Sub lbViewJournal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbViewJournal.Click
        Dim intErrNo As Integer
        Dim dsResult As New Object
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim strOpCode As String = "GL_JOURNAL_PREDICTION"
        Dim arrPeriod As Array
        Dim strRefDate = Date_Validation(txtRefDate.Text, False)

        'arrPeriod = Split(lblAccPeriod.Text, "/")

        strParamName = "LOCCODE|ACCMONTH|ACCYEAR|USERID|TRXID"
        strParamValue = strLocation & "|" & Month(strRefDate) & _
                        "|" & Year(strRefDate) & "|" & _
                        Session("SS_USERID") & "|" & Trim(txtInvID.Text)

        Try
            intErrNo = objGLTrx.mtdGetDataCommon(strOpCode, _
                                                    strParamName, _
                                                    strParamValue, _
                                                    dsResult)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GL_DAYEND_PROCESS&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        If dsResult.Tables(0).Rows.Count > 0 Then

            Dim TotalDB As Double
            Dim TotalCR As Double
            Dim intCnt As Integer
            For intCnt = 0 To dsResult.Tables(0).Rows.Count - 1
                TotalDB += dsResult.Tables(0).Rows(intCnt).Item("AmountDB")
                TotalCR += dsResult.Tables(0).Rows(intCnt).Item("AmountCR")
            Next
            lblTotalDB.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(TotalDB, CInt(Session("SS_ROUNDNO"))), CInt(Session("SS_ROUNDNO")))
            lblTotalCR.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(TotalCR, CInt(Session("SS_ROUNDNO"))), CInt(Session("SS_ROUNDNO")))

            dgViewJournal.DataSource = Nothing
            dgViewJournal.DataSource = dsResult.Tables(0)
            dgViewJournal.DataBind()

            lblTotalDB.Visible = True
            lblTotalCR.Visible = True
            lblTotalViewJournal.Visible = True
            lblTotalViewJournal.Text = "Total Amount : "
        End If

        onLoad_Display(txtInvID.Text)
        onLoad_DisplayItem(txtInvID.Text)
        onLoad_Button()
    End Sub

End Class
