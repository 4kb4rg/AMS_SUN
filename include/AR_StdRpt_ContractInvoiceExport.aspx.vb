
Imports System
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports Microsoft.VisualBasic.Strings
Imports System.Xml
Imports System.Web.Services

Public Class AR_StdRpt_ContractInvoiceExport : Inherits Page

    Protected RptSelect As UserControl

    Dim objGLSetup As New agri.GL.clsSetup()
    Dim objCMRpt As New agri.CM.clsReport()
    Dim objBITrx As New agri.BI.clsTrx()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objBPDs As New Object()
    Dim objSysCfgDs As New Object()
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim objWMTrx As New agri.WM.clsTrx()
    Dim objAdmin As New agri.Admin.clsUom()
    Private objHRSetup As New agri.HR.clsSetup

    Protected WithEvents ddlProduct As DropDownList
    Protected WithEvents ddlUOMCode As DropDownList
    Protected WithEvents ddlFromInvoiceId As DropDownList
    'Protected WithEvents ddlToInvoiceId As DropDownList
    Protected WithEvents txtFromInvoiceDate As Textbox
    Protected WithEvents txtToInvoiceDate As Textbox
    Protected WithEvents txtBuyer As Textbox
    Protected WithEvents txtBillPartyAddress As textbox
    Protected WithEvents ddlInvDocType As Dropdownlist
    Protected WithEvents lblBillParty As Label
    Protected WithEvents lblBillPartyAddress As Label
    Protected WithEvents txtRisk As TextBox
    Protected WithEvents txtDeliveryDate As TextBox
    Protected WithEvents txtFOB As TextBox
    Protected WithEvents txtDestination As TextBox
    Protected WithEvents ddlBankCode As DropDownList
    Protected WithEvents txtUndName As TextBox
    Protected WithEvents lblUndName As Label
    Protected WithEvents txtUndPost As Textbox
    Protected WithEvents lblUndPost As Label
    Protected WithEvents lblDate As Label
    Protected WithEvents lblDateFormat As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblTracker As Label
    Protected WithEvents rbSuppYes As RadioButton
    Protected WithEvents rbSuppNo As RadioButton
    Protected WithEvents PrintPrev As ImageButton
    Protected WithEvents ChkLC As CheckBox
    Protected WithEvents txtLCNo As TextBox
    Protected WithEvents txtIssueDate As TextBox
    Protected WithEvents txtAdvSwift As TextBox
    Protected WithEvents WLC1 As HtmlTableRow
    Protected WithEvents WLC2 As HtmlTableRow
    Protected WithEvents WOLC1 As HtmlTableRow
    Protected WithEvents WOLC2 As HtmlTableRow
    Protected WithEvents WOLC3 As HtmlTableRow
    Protected WithEvents taCorbank As HtmlTextArea
    Protected WithEvents txtSpecification As TextBox
    Protected WithEvents txtNotify As TextBox

    Dim TrMthYr As HtmlTableRow

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intConfigsetting As Integer
    Dim dr As DataRow
    Dim intCnt As Integer
    Dim intErrNo As Integer

    Private dsBank As DataSet
    Dim objUOMDs As New Object()
    Dim objLangCapDs As New Object()
    Dim objInvoiceDs As New Object()
    Dim strUndName As String
    Dim strUndPost As String
    Dim strLocType As String


    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)

        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strAccMonth = Session("SS_ARACCMONTH")
        strAccYear = Session("SS_ARACCYEAR")
        strLangCode = Session("SS_LANGCODE")
        intConfigsetting = Session("SS_CONFIGSETTING")
        strLocType = Session("SS_LOCTYPE")

        WLC1.Visible = False
        WLC2.Visible = False
        WOLC1.Visible = True
        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        Else
            If Not Page.IsPostBack Then
                lblUndName.Text = strUndName
                lblUndPost.Text = strUndPost
                If Trim(strUndName) <> "" Then
                    txtUndName.Text = strUndName
                End If
                If Trim(strUndPost) <> "" Then
                    txtUndPost.Text = strUndPost
                End If
                onload_GetLangCap()
                BindInvoiceType()
                BindBankCode(ddlBankCode, "")
                BindInvoice("")
            End If
        End If
        If ChkLC.Checked = True Then
            ChkLC.Text = "  Yes"
            WLC1.Visible = True
            WLC2.Visible = True
            WOLC1.Visible = False
        Else
            ChkLC.Text = "  No"
            WLC1.Visible = False
            WLC2.Visible = False
            WOLC1.Visible = True
        End If
    End Sub

    Sub BindInvoiceType()
        ddlInvDocType.Items.Add(New ListItem(objBITrx.mtdGetInvoiceDocType(objBITrx.EnumInvoiceDocType.Manual), objBITrx.EnumInvoiceDocType.Manual))
        ddlInvDocType.Items.Add(New ListItem(objBITrx.mtdGetInvoiceDocType(objBITrx.EnumInvoiceDocType.Manual_Millware), objBITrx.EnumInvoiceDocType.Manual_Millware))
        ddlInvDocType.Items.Add(New ListItem(objBITrx.mtdGetInvoiceDocType(objBITrx.EnumInvoiceDocType.Auto_Millware), objBITrx.EnumInvoiceDocType.Auto_Millware))
        ddlInvDocType.SelectedIndex = 0
    End Sub

    Protected Overloads Sub OnPreRender(ByVal Source As Object, ByVal E As EventArgs) Handles MyBase.PreRender
        Dim htmltr As HtmlTableRow

        htmltr = RptSelect.FindControl("SelLocation")
        htmltr.Visible = False

        htmltr = RptSelect.FindControl("TrMthYr")
        htmltr.Visible = True

        htmltr = RptSelect.FindControl("SelDecimal")
        htmltr.Visible = False

        If Page.IsPostBack Then
        End If
    End Sub


    Sub btnPrintPrev_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim strFromInvoiceId As String
        Dim strToInvoiceId As String
        Dim strFromInvoiceDate As String
        Dim strToInvoiceDate As String
        Dim strBillPartyName As String
        Dim strInvoiceType As String
        Dim strDec As String
        Dim strSelAccMonth As String
        Dim strSelAccYear As String
        Dim strRptId As String
        Dim strRptName As String
        Dim strSupp As String
        Dim strParam As String

        Dim ddlist As DropDownList
        Dim rdblist As RadioButtonList

        Dim strDateSetting As String
        Dim objDateFrom As String
        Dim objDateTo As String
        Dim objDateFormat As String
        Dim intCnt As Integer
        Dim strDeliveryDate As String = ""
        Dim objDateDelivery As String
        Dim strMonth As String
        Dim strLC As String


        strFromInvoiceId = ddlFromInvoiceId.Text
        strToInvoiceId = "" 'ddlToInvoiceId.Text
        strFromInvoiceDate = txtFromInvoiceDate.Text
        strToInvoiceDate = txtToInvoiceDate.Text
        strBillPartyName = txtBuyer.Text
        strInvoiceType = ddlInvDocType.SelectedItem.Value
        strLC = IIf(ChkLC.Checked = True, "Yes", "No")

        ddlist = RptSelect.FindControl("lstAccMonth")
        strSelAccMonth = Trim(ddlist.SelectedItem.Value)

        ddlist = RptSelect.FindControl("lstAccYear")
        strSelAccYear = Trim(ddlist.SelectedItem.Value)

        ddlist = RptSelect.FindControl("lstRptName")
        strRptId = Trim(ddlist.SelectedItem.Value)

        ddlist = RptSelect.FindControl("lstRptName")
        strRptName = Trim(ddlist.SelectedItem.Text)

        ddlist = RptSelect.FindControl("lstDecimal")
        strDec = Trim(ddlist.SelectedItem.Value)

        strUndName = Server.UrlEncode(txtUndName.Text.Trim())
        strUndPost = Server.UrlEncode(txtUndPost.Text.Trim())

        If rbSuppYes.Checked Then
            strSupp = rbSuppYes.Text
        ElseIf rbSuppNo.Checked Then
            strSupp = rbSuppNo.Text
        End If

        strParam = "PWSYSTEM_CLSCONFIG_CONFIG_DATEFMT_GET"
        Try
            intErrNo = objSysCfg.mtdGetConfigInfo(strParam, _
                                                  strCompany, _
                                                  strLocation, _
                                                  strUserId, _
                                                  objSysCfgDs)
        Catch Exp As System.Exception
            Response.Redirect("../../include/mesg/ErrorMessage.aspx?errcode=AR_STDRPT_GET_CONFIG_DATE&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        strDateSetting = objSysCfg.mtdGetDateFormat(Trim(objSysCfgDs.Tables(0).Rows(0).Item("Datefmt")))

        If Not (Trim(strFromInvoiceDate) = "" And Trim(strToInvoiceDate) = "") Then
            If objGlobal.mtdValidInputDate(strDateSetting, strFromInvoiceDate, objDateFormat, objDateFrom) = True And objGlobal.mtdValidInputDate(strDateSetting, strToInvoiceDate, objDateFormat, objDateTo) = True Then
            Else
                lblDateFormat.Text = objDateFormat & "."
                lblDate.Visible = True
                lblDateFormat.Visible = True
                Exit Sub
            End If
        End If

        If Not (Trim(txtDeliveryDate.Text) = "") Then
            If objGlobal.mtdValidInputDate(strDateSetting, Trim(txtDeliveryDate.Text), objDateFormat, objDateDelivery) = True Then
            Else
                lblDateFormat.Text = objDateFormat & "."
                lblDate.Visible = True
                lblDateFormat.Visible = True
                Exit Sub
            End If
        End If

        If Not (Trim(txtDeliveryDate.Text) = "") Then
            Select Case Month(objDateDelivery)
                Case 1
                    strMonth = "January"
                Case 2
                    strMonth = "February"
                Case 3
                    strMonth = "March"
                Case 4
                    strMonth = "April"
                Case 5
                    strMonth = "May"
                Case 6
                    strMonth = "June"
                Case 7
                    strMonth = "July"
                Case 8
                    strMonth = "August"
                Case 9
                    strMonth = "September"
                Case 10
                    strMonth = "October"
                Case 11
                    strMonth = "November"
                Case 12
                    strMonth = "December"
            End Select
            strDeliveryDate = Day(objDateDelivery) & " " & strMonth & " " & Year(objDateDelivery)
        End If

        Response.Write("<Script Language=""JavaScript"">window.open(""AR_StdRpt_ContractInvoiceExportPreview.aspx?Type=Print&CompName=" & strCompany & _
                       "&Location=" & strLocation & _
                       "&SelAccMonth=" & strSelAccMonth & _
                       "&SelAccYear=" & strSelAccYear & _
                       "&RptId=" & strRptId & _
                       "&RptName=" & strRptName & _
                       "&Decimal=" & strDec & _
                       "&Supp=" & strSupp & _
                       "&FromInvId=" & strFromInvoiceId & _
                       "&ToInvId=" & strToInvoiceId & _
                       "&FromInvDate=" & objDateFrom & _
                       "&ToInvDate=" & objDateTo & _
                       "&BillPartyName=" & Server.UrlEncode(strBillPartyName) & _
                       "&InvType=" & strInvoiceType & _
                       "&BillPartyTag=" & lblBillParty.Text & _
                       "&ShipRisk=" & Trim(txtRisk.Text) & _
                       "&DeliveryDate=" & strDeliveryDate & _
                       "&FOB=" & Trim(txtFOB.Text) & _
                       "&Destination=" & Trim(txtDestination.Text) & _
                       "&LC=" & strLC & _
                       "&LCNo=" & Trim(txtLCNo.Text) & _
                       "&DateIssue=" & Trim(txtIssueDate.Text) & _
                       "&Bank=" & Trim(ddlBankCode.SelectedItem.Value) & _
                       "&UndName=" & strUndName & _
                       "&UndPost=" & strUndPost & _
                       "&Specification=" & Server.UrlEncode(Trim(txtSpecification.Text)) & _
                       "&Notify=" & Server.UrlEncode(Trim(txtNotify.Text)) & _
                       "&BillPartyAddress=" & Server.UrlEncode(Trim(txtBillPartyAddress.Text)) & _
                       """,null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblBillParty.Text = GetCaption(objLangCap.EnumLangCap.BillParty)
        lblBillPartyAddress.Text = GetCaption(objLangCap.EnumLangCap.BillParty) & " Address"
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
            Response.Redirect("../../include/mesg/ErrorMessage.aspx?errcode=GL_STDRPT_TRIALBALANCE_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=../en/reports/IN_StdRpt_Selection.aspx")
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

    Sub LC_Type(ByVal Sender As Object, ByVal E As EventArgs)
        If ChkLC.Checked = True Then
            ChkLC.Text = "  Yes"
            WLC1.Visible = True
            WLC2.Visible = True
            WOLC1.Visible = False
        Else
            ChkLC.Text = "  No"
            WLC1.Visible = False
            WLC2.Visible = False
            WOLC1.Visible = True
        End If
    End Sub


    Sub BindBankCode(ByVal pv_ddl As DropDownList, ByVal pv_strCode As String)
        Dim strOpCd As String = "HR_CLSSETUP_BANK_SEARCH"
        Dim strParameter As String
        Dim intSelectedIndex As Integer = 0

        strParameter = "||||B.BankCode|ASC|"
        Try
            If dsBank Is Nothing OrElse dsBank.Tables.Count() = 0 Then
                If objHRSetup.mtdGetBank(strOpCd, strParameter, dsBank, False) = 0 Then
                    For intCnt As Integer = 0 To dsBank.Tables(0).Rows.Count - 1
                        dsBank.Tables(0).Rows(intCnt).Item("BankCode") = Trim(dsBank.Tables(0).Rows(intCnt).Item("BankCode"))
                        dsBank.Tables(0).Rows(intCnt).Item("Description") = Trim(dsBank.Tables(0).Rows(intCnt).Item("BankCode")) & " (" & Trim(dsBank.Tables(0).Rows(intCnt).Item("Description")) & ")"
                    Next

                    Dim dr As DataRow
                    dr = dsBank.Tables(0).NewRow()
                    dr("BankCode") = ""
                    dr("Description") = "Please select Bank Code"
                    dsBank.Tables(0).Rows.InsertAt(dr, 0)
                Else
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREG_BINDBANKCODE_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/trx/CM_trx_ContractRegDet.aspx")
                End If
            End If
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREG_BINDBANKCODE_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/trx/CM_trx_ContractRegDet.aspx")
        End Try

        pv_ddl.DataSource = dsBank.Tables(0)
        pv_ddl.DataValueField = "BankCode"
        pv_ddl.DataTextField = "Description"
        pv_ddl.DataBind()

        Dim itm As ListItem = pv_ddl.Items.FindByValue(pv_strCode.Trim())
        If Not itm Is Nothing Then
            intSelectedIndex = pv_ddl.Items.IndexOf(itm)
        End If

        pv_ddl.SelectedIndex = intSelectedIndex
    End Sub

    Sub BindInvoice(ByVal pv_Invoice As String)
        Dim strInvoiceType As String
        Dim strSrchStatus As String
        Dim strSortExpression As String
        Dim strSearch As String
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim strOpCdGet As String = "BI_CLSTRX_INVOICE_GET"
        Dim lbl As Label

        strInvoiceType = objBITrx.EnumInvoiceDocType.Manual
        strSrchStatus = objBITrx.EnumInvoiceStatus.Confirmed
        strSortExpression = "InvoiceID"
        strParam = "||" & _
                   strInvoiceType & "|" & _
                   strSrchStatus & "||" & strSortExpression & "|"

        Try
            intErrNo = objBITrx.mtdGetInvoice(strOpCdGet, _
                                              strCompany, _
                                              strLocation, _
                                              strUserId, _
                                              strAccMonth, _
                                              strAccYear, _
                                              strParam, _
                                              objInvoiceDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_INVOICELIST_GET_LIST&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To objInvoiceDs.Tables(0).Rows.Count - 1
            objInvoiceDs.Tables(0).Rows(intCnt).Item("InvoiceId") = Trim(objInvoiceDs.Tables(0).Rows(intCnt).Item("InvoiceId"))
        Next

        Dim dr As DataRow
        dr = objInvoiceDs.Tables(0).NewRow()
        dr("InvoiceId") = ""
        dr("InvoiceId") = "Please select Invoice No"
        objInvoiceDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlFromInvoiceId.DataSource = objInvoiceDs.Tables(0)
        ddlFromInvoiceId.DataValueField = "InvoiceId"
        ddlFromInvoiceId.DataTextField = "InvoiceId"
        ddlFromInvoiceId.DataBind()

    End Sub

    Sub InvoiceIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim strOpCd_GetInvoice As String = "BI_CLSTRX_INVOICE_DETAILS_GET"
        Dim strOpCd_GetBillParty As String = "GL_CLSSETUP_BILLPARTY_GET"
        Dim intErrNo As Integer
        Dim strParam As String = Trim(ddlFromInvoiceId.SelectedItem.Value)
        Dim intCnt As Integer = 0
        Dim strBillPartyCode As String

        Try
            intErrNo = objBITrx.mtdGetInvoice(strOpCd_GetInvoice, _
                                                strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strAccMonth, _
                                                strAccYear, _
                                                strParam, _
                                                objInvoiceDs, _
                                                True)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_INVOICE_GET_HEADER&errmesg=" & Exp.ToString & "&redirect=BI/trx/BI_trx_DNList.aspx")
        End Try

        If objInvoiceDs.Tables(0).Rows.Count > 0 Then
            strBillPartyCode = Trim(objInvoiceDs.Tables(0).Rows(0).Item("BillPartyCode"))

            strParam = strBillPartyCode & "||1||BP.BillPartyCode|ASC|"
            Try
                intErrNo = objGLSetup.mtdGetBillParty(strOpCd_GetBillParty, strParam, objBPDs)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_DEBITNOTEDET_BILLPARTY_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
            End Try

            If objBPDs.Tables(0).Rows.Count > 0 Then
                txtBuyer.Text = Trim(objBPDs.Tables(0).Rows(0).Item("Name"))
                txtBillPartyAddress.Text = Trim(objBPDs.Tables(0).Rows(0).Item("Address")) & " " & Trim(objBPDs.Tables(0).Rows(0).Item("Town")) & " " & _
                                           Trim(objBPDs.Tables(0).Rows(0).Item("State")) & " " & Trim(objBPDs.Tables(0).Rows(0).Item("CountryDesc")) & " " & Trim(objBPDs.Tables(0).Rows(0).Item("PostCode"))
            End If
        End If
    End Sub
End Class
