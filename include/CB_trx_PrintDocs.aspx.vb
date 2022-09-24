

Imports System
Imports System.Data
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic


Public Class CB_trx_PrintDocs : Inherits Page

    Protected WithEvents ddlBank As DropDownList
    Protected WithEvents ddlBankTo As DropDownList
    Protected WithEvents ddlPayType As DropDownList
    Protected WithEvents txtBankAccNo As TextBox
    Protected WithEvents txtChequeNo As TextBox
    Protected WithEvents lblErrCheque As Label
    Protected WithEvents lblErrBankAccNo As Label
    Protected WithEvents lblErrBank As Label
    Protected WithEvents lblErrBankto As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblConfirmErr As Label
    Protected WithEvents btnGet As Button
    Protected WithEvents PrintChequeBtn As ImageButton
    Protected WithEvents PrintSlipBtn As ImageButton
    Protected WithEvents PrintTransferBtn As ImageButton
    Protected WithEvents lblBankFrom As Label
    Protected WithEvents lblBankTo As Label
    Protected WithEvents txtExRate As TextBox
    Protected WithEvents ddlCurrency As DropDownList
    Protected WithEvents txtBiaya As TextBox
    Protected WithEvents chkDeduct As CheckBox
    Protected WithEvents lblErrExRate As Label
    Protected WithEvents txtTtlAmount As TextBox
    Protected WithEvents txtAmtToPrint As TextBox
    Protected WithEvents btnGetCG As ImageButton
    Protected WithEvents btnGetBP As ImageButton
    Protected WithEvents txtChequeNoToPrint As TextBox
    Protected WithEvents hidSupplierCode As HtmlInputHidden

    Protected objCBTrx As New agri.CB.clsTrx()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objPUTrx As New agri.PU.clsTrx()
    Dim objAdmin As New agri.Admin.clsShare()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim objHRSetup As New agri.HR.clsSetup()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objLangCapDs As New Dataset()
    Dim objRptDs As New DataSet()
    Dim objPRRpt As New agri.PR.clsReport()
    Dim objGLtrx As New agri.GL.ClsTrx()
    Dim objCMSetup As New agri.CM.clsSetup()

    Dim strBankCode As String
    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim intConfig As Integer
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim intDocType As Integer
    Dim strDocType As String
    Dim strTrxID As String
    Dim strParam As String = ""
    Dim strExchangeRate As String
    Dim intCBAR As Integer
    Dim strAccTag As String
    Dim strBlkTag As String
    Dim strVehTag As String
    Dim strVehExpCodeTag As String
    Dim strLangCode As String
    Dim strLocType As String

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        intConfig = Session("SS_CONFIGSETTING")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_CBACCMONTH")
        strAccYear = Session("SS_CBACCYEAR")
        strLocType = Session("SS_LOCTYPE")
        intCBAR = Session("SS_CBAR")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCBAccessRights.CBCashBank), intCBAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            If Request.QueryString("doctype") = "" Then
                intDocType = 0
            Else
                intDocType = Request.QueryString("doctype")
                If intDocType = 1 Then
                    strDocType = "CASHBANK"
                Else
                    strDocType = "PAYMENT"
                End If
            End If

            lblErrCheque.Visible = False
            lblErrBankAccNo.Visible = False
            lblErrBank.Visible = False
            lblErrBankto.Visible = False
            lblConfirmErr.Visible = False
            lblErrExRate.Visible = False
            txtTtlAmount.Enabled = False

            'PrintSlipBtn.Visible = False
            'PrintTransferBtn.Visible = False
            'PrintChequeBtn.Visible = False
            'txtBankAccNo.Enabled = False
            'ddlBank.Enabled = False
            'ddlBankTo.Enabled = False

            If Not Page.IsPostBack Then
                BindBankCode("")
                BindBankTo("")
                BindCurrencyList("")
            End If
        End If
        
    End Sub

    Sub BindCurrencyList(ByVal pv_strCurrencyCode As String)
        Dim strParam As String
        Dim strSearch As String
        Dim strSort As String
        Dim strOpCdGet As String = "CM_CLSSETUP_CURRENCY_GET"
        Dim dr As DataRow
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer
        Dim objCurrencyDs As New Object()

        strSearch = "and curr.Status = '" & objCMSetup.EnumCurrencyStatus.Active & "' "
        strSort = "order by curr.CurrencyCode "

        strParam = strSearch & "|" & strSort

        Try
            intErrNo = objCMSetup.mtdGetMasterList(strOpCdGet, strParam, 0, objCurrencyDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_CURRENCYLIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegList.aspx")
        End Try

        If pv_strCurrencyCode = "" Then
            pv_strCurrencyCode = "IDR"
        End If
        If objCurrencyDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objCurrencyDs.Tables(0).Rows.Count - 1
                objCurrencyDs.Tables(0).Rows(intCnt).Item("CurrencyCode") = Trim(objCurrencyDs.Tables(0).Rows(intCnt).Item("CurrencyCode"))
                objCurrencyDs.Tables(0).Rows(intCnt).Item("Description") = objCurrencyDs.Tables(0).Rows(intCnt).Item("CurrencyCode")
                If objCurrencyDs.Tables(0).Rows(intCnt).Item("CurrencyCode") = pv_strCurrencyCode Then
                    intSelectedIndex = intCnt
                End If
            Next
        End If


        ddlCurrency.DataSource = objCurrencyDs.Tables(0)
        ddlCurrency.DataValueField = "CurrencyCode"
        ddlCurrency.DataTextField = "Description"
        ddlCurrency.DataBind()
        ddlCurrency.SelectedIndex = intSelectedIndex

    End Sub

    Sub CurrencyChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim strParam As String
        Dim strSearch As String
        Dim strSort As String
        Dim strOpCdGet As String = "CM_CLSSETUP_EXCHANGERATE_GET"
        Dim dr As DataRow
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer
        Dim objCurrencyDs As New Object()
        Dim strDate As String = Date_Validation(Now(), False)

        strSearch = "and exc.SecCurrencyCode = 'IDR' and exc.FirstCurrencyCode = '" & Trim(ddlCurrency.SelectedItem.Value) & "' and exc.Status = '" & objCMSetup.EnumExchangeRateStatus.Active & "' and exc.TransDate = '" & strDate & "' "
        strSort = "order by exc.FirstCurrencyCode "

        strParam = strSearch & "|" & strSort

        Try
            intErrNo = objCMSetup.mtdGetMasterList(strOpCdGet, strParam, 0, objCurrencyDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_CURRENCYLIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegList.aspx")
        End Try

        If objCurrencyDs.Tables(0).Rows.Count > 0 Then
            txtExRate.Text = objCurrencyDs.Tables(0).Rows(0).Item("ExchangeRate")
        Else
            If Trim(ddlCurrency.SelectedItem.Value) <> "IDR" Then
                lblErrExRate.Visible = True
                Exit Sub
            End If
        End If
    End Sub

    Sub BindBankCode(ByVal pv_strBankCode As String)
        Dim strOpCode As String = "HR_CLSSETUP_BANK_LOCATION_GET_BANKONLY"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim objBankDs As New DataSet()
        Dim intSelectedBankIndex As Integer = 0
        Dim strParamName As String = ""
        Dim strParamValue As String = ""

        strParamName = "LOCCODE"
        strParamValue = Trim(strLocation)

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
                                                    strParamName, _
                                                    strParamValue, _
                                                    objBankDs)

        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_GET_BANK&errmesg=" & Exp.ToString() & "&redirect=CB/Trx/cb_trx_CashBankList.aspx")
        End Try

        For intCnt = 0 To objBankDs.Tables(0).Rows.Count - 1
            If (objBankDs.Tables(0).Rows(intCnt).Item("BankCode") = Trim(pv_strBankCode)) Then
                intSelectedBankIndex = intCnt + 1
                Exit For
            End If
        Next intCnt

        Dim dr As DataRow
        dr = objBankDs.Tables(0).NewRow()
        dr("BankCode") = ""
        dr("_Description") = "Please Select Bank"
        objBankDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlBank.DataSource = objBankDs.Tables(0)
        ddlBank.DataValueField = "BankCode"
        ddlBank.DataTextField = "_Description"
        ddlBank.DataBind()
        ddlBank.SelectedIndex = intSelectedBankIndex

    End Sub

    Sub BindBankTo(ByVal pv_strBankCode As String)
        Dim strOpCode As String = "HR_CLSSETUP_BANK_LIST_GET"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim objBankDs As New DataSet()
        Dim intSelectedBankIndex As Integer = 0

        strParam = "|"

        Try
            intErrNo = objHRSetup.mtdGetMasterList(strOpCode, _
                                                   strParam, _
                                                   objHRSetup.EnumHRMasterType.Bank, _
                                                   objBankDs)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_GET_BANK&errmesg=" & Exp.ToString() & "&redirect=CB/Trx/cb_trx_CashBankList.aspx")
        End Try

        For intCnt = 0 To objBankDs.Tables(0).Rows.Count - 1
            If (objBankDs.Tables(0).Rows(intCnt).Item("BankCode") = Trim(pv_strBankCode)) Then
                intSelectedBankIndex = intCnt + 1
                Exit For
            End If
        Next intCnt

        Dim dr As DataRow
        dr = objBankDs.Tables(0).NewRow()
        dr("BankCode") = ""
        dr("_Description") = "Please Select Bank"
        objBankDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlBankTo.DataSource = objBankDs.Tables(0)
        ddlBankTo.DataValueField = "BankCode"
        ddlBankTo.DataTextField = "_Description"
        ddlBankTo.DataBind()
        ddlBankTo.SelectedIndex = intSelectedBankIndex

    End Sub

    Sub PreviewChequeBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim dsForBankCodeDropDown As New DataSet()
        Dim strChequeNo As String = txtChequeNo.Text.Trim
        Dim strProgramPath As String = ""

        Dim strStatus As String
        Dim intStatus As Integer
        Dim strSortLine As String
        Dim strPrintDate As String
        Dim strTable As String
        Dim strCBType As String

        strSortLine = "CB_CASHBANKLN.CashBankLnID"
        strTable = "CB_CASHBANK"
        strCBType = "1"

        If ddlBank.SelectedItem.Value = "" Then
            lblErrBank.Visible = True
            Exit Sub
        End If

        If txtChequeNo.Text = "" Then
            lblErrCheque.Visible = True
            Exit Sub
        End If

        If Trim(ddlCurrency.SelectedItem.Value) <> "IDR" And (txtExRate.Text = "1" Or txtExRate.Text = "0") Then
            lblErrExRate.Text = "Please input exchange rate for this currency code."
            lblErrExRate.Visible = True
            Exit Sub
        End If
        strExchangeRate = txtExRate.Text

        Dim strOpCd As String = "CB_CLSTRX_CBPAY_UPDATE_MULTITRX"
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim intErrNo As Integer

        strParamName = "BANKCODE|CHEQUEPRINTDATE|LOCCODE|CHEQUENO"

        strParamValue = Trim(ddlBank.SelectedItem.Value) & _
                        "|" & Now() & _
                        "|" & Trim(strLocation) & _
                        "|" & Trim(txtChequeNo.Text)

        Try
            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_CASHBANK_PREVIEWCHEQUE_UPD&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_CashBankDet.aspx")
        End Try

        If ddlPayType.SelectedItem.Value = "0" Then
            strOpCd = "PR_STDRPT_BANK_GET_CHEQUEFMT"
            strParam = Convert.ToString(objHRSetup.EnumBankStatus.Active) & "|" & _
                       Convert.ToString(objHRSetup.EnumBankFormatStatus.Active) & "|" & _
                       Convert.ToString(objHRSetup.EnumBankFormatType.Cheque) & "|" & _
                       Trim(ddlBank.SelectedItem.Value)
        ElseIf ddlPayType.SelectedItem.Value = "3" Then
            strOpCd = "PR_STDRPT_BANK_GET_BILYETGIROFMT"
            strParam = Convert.ToString(objHRSetup.EnumBankStatus.Active) & "|" & _
                       Convert.ToString(objHRSetup.EnumBankFormatStatus.Active) & "|" & _
                       Convert.ToString(objHRSetup.EnumBankFormatType.BilyetGiro) & "|" & _
                       Trim(ddlBank.SelectedItem.Value)
        End If


        Try
            intErrNo = objPRRpt.mtdGetBank(strOpCd, strParam, dsForBankCodeDropDown)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_PROGPATH_PREVIEWCHEQUE_GET&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_CashBankDet.aspx")
        End Try

        If dsForBankCodeDropDown.Tables(0).Rows.Count > 0 Then
            strProgramPath = dsForBankCodeDropDown.Tables(0).Rows(0).Item("ProgramPath").Trim()

            Response.Write("<Script Language=""JavaScript"">window.open(""../reports/CB_Rpt_ChequeGiroMultiPrint.aspx?strId=" & txtChequeNo.Text & _
                       "&strProgramPath=" & strProgramPath & _
                       "&TRXType=" & strDocType & _
                       "&CBType=" & "1" & _
                       "&strSortLine=" & strSortLine & _
                       "&strCurrencyCode=" & Trim(ddlCurrency.SelectedItem.Value) & _
                       "&strExchangeRate=" & strExchangeRate & _
                       "&strBiaya=" & IIf(txtBiaya.Text = "", "0", txtBiaya.Text) & _
                       "&strDeduct=" & IIf(chkDeduct.Checked = True, "1", "0") & _
                       "&strAmtToPrint=" & Trim(txtAmtToPrint.Text) & _
                       "&strChequeNoToPrint=" & Trim(txtChequeNoToPrint.Text) & _
                       """, null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");window.close();</Script>")
        Else
            lblConfirmErr.Text = "Cheque format not found! Please check your Bank Details again."
            lblConfirmErr.Visible = True
            Exit Sub
        End If
    End Sub

    Sub PreviewSlipBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim dsForBankCodeDropDown As New DataSet()
        Dim strChequeNo As String = txtChequeNo.Text.Trim
        Dim strProgramPath As String = ""

        Dim strStatus As String
        Dim intStatus As Integer
        Dim strSortLine As String
        Dim strPrintDate As String
        Dim strTable As String
        Dim strCBType As String

        strSortLine = "CB_CASHBANKLN.CashBankLnID"
        strTable = "CB_CASHBANK"
        strCBType = "1"

        If lblBankFrom.Text = "" Then
            lblErrBank.Visible = True
            Exit Sub
        End If

        If txtChequeNo.Text = "" Then
            lblErrCheque.Visible = True
            Exit Sub
        End If

        If Trim(ddlCurrency.SelectedItem.Value) <> "IDR" And (txtExRate.Text = "1" Or txtExRate.Text = "0") Then
            lblErrExRate.Text = "Please input exchange rate for this currency code."
            lblErrExRate.Visible = True
            Exit Sub
        End If
        strExchangeRate = txtExRate.Text

        Dim strOpCd As String = "CB_CLSTRX_CBPAY_UPDATE_MULTITRX"
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim intErrNo As Integer

        strParamName = "BANKCODE|CHEQUEPRINTDATE|LOCCODE|CHEQUENO"

        strParamValue = Trim(ddlBank.SelectedItem.Value) & _
                        "|" & Now() & _
                        "|" & Trim(strLocation) & _
                        "|" & Trim(txtChequeNo.Text)

        Try
            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_CASHBANK_PREVIEWCHEQUE_UPD&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_CashBankDet.aspx")
        End Try

        strOpCd = "PR_STDRPT_BANK_GET_SLIPSETORANFMT"
        strParam = Convert.ToString(objHRSetup.EnumBankStatus.Active) & "|" & _
                   Convert.ToString(objHRSetup.EnumBankFormatStatus.Active) & "|" & _
                   Convert.ToString(objHRSetup.EnumBankFormatType.SlipSetoran) & "|" & _
                   Trim(ddlBank.SelectedItem.Value)

        Try
            intErrNo = objPRRpt.mtdGetBank(strOpCd, strParam, dsForBankCodeDropDown)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_PROGPATH_PREVIEWCHEQUE_GET&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_CashBankDet.aspx")
        End Try

        If dsForBankCodeDropDown.Tables(0).Rows.Count > 0 Then
            strProgramPath = dsForBankCodeDropDown.Tables(0).Rows(0).Item("ProgramPath").Trim()

            Response.Write("<Script Language=""JavaScript"">window.open(""../reports/CB_Rpt_ChequeGiroMultiPrint.aspx?strId=" & txtChequeNo.Text & _
                       "&strProgramPath=" & strProgramPath & _
                       "&TRXType=" & strDocType & _
                       "&CBType=" & "2" & _
                       "&strSortLine=" & strSortLine & _
                       "&strCurrencyCode=" & Trim(ddlCurrency.SelectedItem.Value) & _
                       "&strExchangeRate=" & strExchangeRate & _
                       "&strBiaya=" & IIf(txtBiaya.Text = "", "0", txtBiaya.Text) & _
                       "&strDeduct=" & IIf(chkDeduct.Checked = True, "1", "0") & _
                       "&strAmtToPrint=" & Trim(txtAmtToPrint.Text) & _
                       "&strChequeNoToPrint=" & Trim(txtChequeNoToPrint.Text) & _
                       """, null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");window.close();</Script>")
        Else
            lblConfirmErr.Text = "Slip format not found! Please check your Bank Details again."
            lblConfirmErr.Visible = True
            Exit Sub
        End If
    End Sub

    Sub btnPreviewTransfer_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim dsForBankCodeDropDown As New DataSet()
        Dim strChequeNo As String = txtChequeNo.Text.Trim
        Dim strProgramPath As String = ""

        Dim strStatus As String
        Dim intStatus As Integer
        Dim strSortLine As String
        Dim strPrintDate As String
        Dim strTable As String
        Dim strCBType As String

        strSortLine = "CB_CASHBANKLN.CashBankLnID"
        strTable = "CB_CASHBANK"
        strCBType = "1"

        If lblBankFrom.Text = "" Then
            lblErrBank.Visible = True
            Exit Sub
        End If

        If ddlBankTo.SelectedItem.Value = "" Then
            lblErrBankto.Visible = True
            Exit Sub
        End If

        If txtChequeNo.Text = "" Then
            lblErrCheque.Visible = True
            Exit Sub
        End If

        If Trim(ddlCurrency.SelectedItem.Value) <> "IDR" And (txtExRate.Text = "1" Or txtExRate.Text = "0") Then
            lblErrExRate.Text = "Please input exchange rate for this currency code."
            lblErrExRate.Visible = True
            Exit Sub
        End If
        strExchangeRate = txtExRate.Text

        Dim strOpCd As String = "CB_CLSTRX_CBPAY_UPDATE_MULTITRX"
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim intErrNo As Integer

        strParamName = "BANKCODE|CHEQUEPRINTDATE|LOCCODE|CHEQUENO"

        strParamValue = Trim(ddlBank.SelectedItem.Value) & _
                        "|" & Now() & _
                        "|" & Trim(strLocation) & _
                        "|" & Trim(txtChequeNo.Text)

        Try
            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_CASHBANK_PREVIEWCHEQUE_UPD&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_CashBankDet.aspx")
        End Try

        strOpCd = "PR_STDRPT_BANK_GET_SLIPTRANSFERFMT"
        strParam = Convert.ToString(objHRSetup.EnumBankStatus.Active) & "|" & _
                   Convert.ToString(objHRSetup.EnumBankFormatStatus.Active) & "|" & _
                   Convert.ToString(objHRSetup.EnumBankFormatType.SlipTransfer) & "|" & _
                   Trim(ddlBank.SelectedItem.Value)

        Try
            intErrNo = objPRRpt.mtdGetBank(strOpCd, strParam, dsForBankCodeDropDown)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_PROGPATH_PREVIEWCHEQUE_GET&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_CashBankDet.aspx")
        End Try

        If dsForBankCodeDropDown.Tables(0).Rows.Count > 0 Then
            strProgramPath = dsForBankCodeDropDown.Tables(0).Rows(0).Item("ProgramPath").Trim()

            Response.Write("<Script Language=""JavaScript"">window.open(""../reports/CB_Rpt_ChequeGiroMultiPrint.aspx?strId=" & txtChequeNo.Text & _
                       "&strProgramPath=" & strProgramPath & _
                       "&TRXType=" & strDocType & _
                       "&CBType=" & "3" & _
                       "&strSortLine=" & strSortLine & _
                       "&strCurrencyCode=" & Trim(ddlCurrency.SelectedItem.Value) & _
                       "&strExchangeRate=" & strExchangeRate & _
                       "&strBiaya=" & IIf(txtBiaya.Text = "", "0", txtBiaya.Text) & _
                       "&strDeduct=" & IIf(chkDeduct.Checked = True, "1", "0") & _
                       "&strAmtToPrint=" & Trim(txtAmtToPrint.Text) & _
                       "&strChequeNoToPrint=" & Trim(txtChequeNoToPrint.Text) & _
                       """, null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");window.close();</Script>")
        Else
            lblConfirmErr.Text = "Slip format not found! Please check your Bank Details again."
            lblConfirmErr.Visible = True
            Exit Sub
        End If
    End Sub

    Sub GetCGBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim strOpCode As String = "CB_CLSTRX_CBPAY_GETDATA_FOR_DOCRPT_CHEQUEGIRO_MULTITRX"
        Dim intErrNo As Integer
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim dsMaster As Object
        Dim intCnt As Integer
        Dim dblTotal As Double

        strParamName = "CHEQUENO"
        If txtChequeNo.Text = "" Then
            lblErrCheque.Visible = False
            Exit Sub
        Else
            strParamValue = txtChequeNo.Text
        End If

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
                                                strParamName, _
                                                strParamValue, _
                                                dsMaster)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=IN_ITEM_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        If dsMaster.Tables(0).Rows.Count > 0 Then
            If dsMaster.Tables(0).Rows.Count > 1 And dsMaster.Tables(0).Rows(0).Item("SourceType") = "3" Then
                lblConfirmErr.Text = "There are different supplier code between transactions."
                lblConfirmErr.Visible = True
                Exit Sub
            End If

            For intCnt = 0 To dsMaster.Tables(0).Rows.Count - 1
                dblTotal += dsMaster.Tables(0).Rows(intCnt).Item("TotalAmount")
            Next
            dsMaster.Tables(0).Rows(0).Item("TotalAmount") = dblTotal
           
            ddlPayType.SelectedValue = Trim(dsMaster.Tables(0).Rows(0).Item("SourceType"))
            txtBankAccNo.Text = Trim(dsMaster.Tables(0).Rows(0).Item("BankAccNo"))
            txtTtlAmount.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(dsMaster.Tables(0).Rows(0).Item("TotalAmount"), 2), 2)
            txtAmtToPrint.Text = Trim(dsMaster.Tables(0).Rows(0).Item("TotalAmount"))
            txtChequeNoToPrint.Text = Trim(dsMaster.Tables(0).Rows(0).Item("ChequeNo"))
            BindBankCode(dsMaster.Tables(0).Rows(0).Item("BankCode"))
            BindBankTo(dsMaster.Tables(0).Rows(0).Item("BankTo"))
            'hidSupplierCode.Value = Trim(dsMaster.Tables(0).Rows(0).Item("SupplierCode"))

            PrintChequeBtn.Visible = True
            If dsMaster.Tables(0).Rows(0).Item("BankCode") = dsMaster.Tables(0).Rows(0).Item("BankTo") Then
                PrintSlipBtn.Visible = True
                PrintTransferBtn.Visible = False
            Else
                PrintSlipBtn.Visible = False
                PrintTransferBtn.Visible = True
            End If

            lblBankFrom.Text = ddlBank.SelectedItem.Value
            lblBankTo.Text = ddlBankTo.SelectedItem.Value
            If lblBankFrom.Text = "EKS" Then
                BindCurrencyList("USD")
            Else
                BindCurrencyList("IDR")
            End If
        End If

        ddlBank.enabled = False
        ddlBankTo.enabled = False
        txtBankAccNo.enabled = False
        txtChequeNoToPrint.enabled = False

    End Sub

    'Sub GetBPBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
    '    Dim strOpCode As String = "CB_CLSTRX_CBPAY_GETDATA_FOR_DOCRPT_CHEQUEGIRO_MULTITRX"
    '    Dim intErrNo As Integer
    '    Dim strParamName As String = ""
    '    Dim strParamValue As String = ""
    '    Dim dsMaster As Object

    '    strParamName = "CASHBANKID"
    '    If txtChequeNo.Text = "" Then
    '        lblErrCheque.Visible = False
    '        Exit Sub
    '    Else
    '        strParamValue = txtChequeNo.Text
    '    End If

    '    Try
    '        intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
    '                                            strParamName, _
    '                                            strParamValue, _
    '                                            dsMaster)
    '    Catch Exp As System.Exception
    '        Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=IN_ITEM_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
    '    End Try

    '    If dsMaster.Tables(0).Rows.Count > 0 Then
    '        ddlPayType.SelectedValue = Trim(dsMaster.Tables(0).Rows(0).Item("SourceType"))
    '        txtBankAccNo.Text = Trim(dsMaster.Tables(0).Rows(0).Item("BankAccNo"))
    '        txtTtlAmount.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(dsMaster.Tables(0).Rows(0).Item("TotalAmount"), 2), 2)
    '        txtAmtToPrint.Text = Trim(dsMaster.Tables(0).Rows(0).Item("TotalAmount"))
    '        BindBankCode(dsMaster.Tables(0).Rows(0).Item("BankCode"))
    '        BindBankTo(dsMaster.Tables(0).Rows(0).Item("BankTo"))

    '        PrintChequeBtn.Visible = True
    '        If dsMaster.Tables(0).Rows(0).Item("BankCode") = dsMaster.Tables(0).Rows(0).Item("BankTo") Then
    '            PrintSlipBtn.Visible = True
    '            PrintTransferBtn.Visible = False
    '        Else
    '            PrintSlipBtn.Visible = False
    '            PrintTransferBtn.Visible = True
    '        End If

    '        lblBankFrom.Text = ddlBank.SelectedItem.Value
    '        lblBankTo.Text = ddlBankTo.SelectedItem.Value
    '        If lblBankFrom.Text = "EKS" Then
    '            BindCurrencyList("USD")
    '        Else
    '            BindCurrencyList("IDR")
    '        End If
    '    End If
    'End Sub

    Sub onSelect_Change(ByVal Sender As System.Object, ByVal E As System.EventArgs)
        If ddlBank.SelectedItem.Value = ddlBankTo.SelectedItem.Value Then
            PrintSlipBtn.Visible = True
            PrintTransferBtn.Visible = False
        Else
            PrintSlipBtn.Visible = False
            PrintTransferBtn.Visible = True
        End If
        PrintChequeBtn.Visible = True
    End Sub

    Sub close_window()
        Response.Write("<Script Language=""JavaScript"">window.close();</Script>")
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        Try
            If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfig) = True Then
                strBlkTag = GetCaption(objLangCap.EnumLangCap.Block)
            Else
                strBlkTag = GetCaption(objLangCap.EnumLangCap.SubBlock)
            End If
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_PRINTDOCS_GETLANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try
        strAccTag = GetCaption(objLangCap.EnumLangCap.Account)
        strVehTag = GetCaption(objLangCap.EnumLangCap.Vehicle)
        strVehExpCodeTag = GetCaption(objLangCap.EnumLangCap.VehExpense)
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PU_TRX_PRINTDOCS_GETENTIRELANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=")
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

    Function Date_Validation(ByVal pv_strInputDate As String, ByVal pv_blnIsShortDate As Boolean) As String
        Dim objSysCfgDs As New Object
        Dim objActualDate As New Object
        Dim strDateFormat As String
        Dim strParam As String
        Dim intErrNo As Integer
        Dim strAcceptFormat As String

        strParam = "PWSYSTEM_CLSCONFIG_CONFIG_DATEFMT_GET"

        Try
            intErrNo = objSysCfg.mtdGetConfigInfo(strParam, _
                                                  strCompany, _
                                                  strLocation, _
                                                  strUserId, _
                                                  objSysCfgDs)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_DEPOSIT_GET_CONFIG&errmesg=" & Exp.Message & "&redirect=CB/trx/cb_trx_DepositList.aspx")
        End Try

        strDateFormat = objSysCfg.mtdGetDateFormat(objSysCfgDs.Tables(0).Rows(0).Item("Datefmt").Trim())

        If pv_blnIsShortDate Then
            Date_Validation = objGlobal.GetShortDate(strDateFormat, pv_strInputDate)
        Else
            If objGlobal.mtdValidInputDate(strDateFormat, _
                                           pv_strInputDate, _
                                           strAcceptFormat, _
                                           objActualDate) = True Then
                Date_Validation = objActualDate
            Else
                Date_Validation = ""
            End If
        End If
    End Function
End Class
