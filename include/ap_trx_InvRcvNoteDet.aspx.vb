
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


Public Class ap_trx_InvRcvNoteDet : Inherits Page

    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents lblInvoiceRcvID As Label
    Protected WithEvents lblAccPeriod As Label
    Protected WithEvents lblStatus As Label
    Protected WithEvents txtInvoiceRcvRefNo As TextBox
    Protected WithEvents lblDateCreated As Label
    Protected WithEvents txtInvoiceRcvRefDate As TextBox
    Protected WithEvents lblLastUpdate As Label
    Protected WithEvents lblPrintDate As Label
    Protected WithEvents ddlPO As DropDownList
    Protected WithEvents lblUpdatedBy As Label

    Protected WithEvents lblErrSuppCode As Label
    Protected WithEvents txtCreditTerm As TextBox
    Protected WithEvents ddlTermType As DropDownList
    Protected WithEvents dgLineDet As DataGrid
    Protected WithEvents lblInvoiceAmount As Label
    Protected WithEvents txtRemark As TextBox
    Protected WithEvents tblSelection As HtmlTable
    Protected WithEvents validateItem As RequiredFieldValidator
    Protected WithEvents btnSelDate As Image
    Protected WithEvents Addbtn As ImageButton
    Protected WithEvents SaveBtn As ImageButton
    Protected WithEvents GenInvBtn As ImageButton
    Protected WithEvents PrintBtn As ImageButton
    Protected WithEvents CancelBtn As ImageButton
    Protected WithEvents DeleteBtn As ImageButton
    Protected WithEvents UnDeleteBtn As ImageButton
    Protected WithEvents BackBtn As ImageButton
    Protected WithEvents lblPleaseSelect As Label
    Protected WithEvents inrid As HtmlInputHidden
    Protected WithEvents idSuppCode As HtmlInputHidden
    Protected WithEvents lblErrInvRcvRefDate As Label
    Protected WithEvents lblErrUnDel As Label
    Protected WithEvents lblErrQty As Label
    Protected WithEvents lblStatusHidden As Label
    Protected WithEvents lblErrPO As Label
    Protected WithEvents lblTitle As Label
    Protected WithEvents lblInvoiceRcvIDTag As Label
    Protected WithEvents lblInvoiceRcvRefNoTag As Label
    Protected WithEvents lblInvoiceRcvRefDateTag As Label
    Protected WithEvents lblID As Label
    Protected WithEvents lblRefNo As Label
    Protected WithEvents lblRefDate As Label
    Protected WithEvents lblIDInvoiceAmount As Label
    Protected WithEvents lblCurrency1 As Label
    Protected WithEvents lblCurrency2 As Label
    Protected WithEvents hidCurrencyCode As HtmlInputHidden
    Protected WithEvents hidExchangeRate As HtmlInputHidden
    Protected WithEvents txtInvDueDate As TextBox
    Protected WithEvents lblerrInvDueDate As Label
    Protected WithEvents hidTermType As HtmlInputHidden
    Protected WithEvents txtSplInvAmt As TextBox
    Protected WithEvents NewBtn As ImageButton
    Protected WithEvents dgAPNote As DataGrid
    Protected WithEvents txtTransDate As TextBox
    Protected WithEvents lblErrTransDate As Label
    Protected WithEvents txtFakturPjkNo As TextBox
    Protected WithEvents txtFakturPjkDate As TextBox
    Protected WithEvents txtSplTaxAmt As TextBox

    Protected WithEvents txtPPNInit As TextBox
    Protected WithEvents txtPPN As TextBox
    Protected WithEvents txtSupCode As TextBox
    Protected WithEvents txtSupName As TextBox

    Protected WithEvents lblerrFakturPjkDate As Label
    Protected WithEvents rbSPO As RadioButton
    Protected WithEvents rbAdvPay As RadioButton
    Protected WithEvents rbOTE As RadioButton
    Protected WithEvents rbFFB As RadioButton
    Protected WithEvents rbTransportFee As RadioButton
    Protected WithEvents hidPOItem As HtmlInputHidden
    Protected WithEvents txtPODueDate As TextBox
    Protected WithEvents txtDueDate As TextBox
    Protected WithEvents lblErrCurrency As Label
    Protected WithEvents lblErrSplInvAmt As Label
    Protected WithEvents lblErrCreditTerm As Label
    Protected WithEvents txtAddNote As TextBox
    Protected WithEvents txtReceiveFrom As TextBox
    Protected WithEvents lblAdvancePayment As Label
    Protected WithEvents hidPOAmount As HtmlInputHidden
    Protected WithEvents hidAdvAmount As HtmlInputHidden
    Protected WithEvents lblFmtTransDate As Label
    Protected WithEvents lblFmtInvRcvRefDate As Label
    Protected WithEvents lblFmtInvDueDate As Label
    Protected WithEvents lblFmtFakturPjkDate As Label

    Protected WithEvents lblAdjAmount As Label
    Protected WithEvents hidAdjAmount As HtmlInputHidden

    Protected WithEvents ConfirmBtn As ImageButton

    Protected WithEvents rbViaBTNo As RadioButton
    Protected WithEvents rbViaBTYes As RadioButton

    Protected WithEvents lblErrFakturPjk As Label


    Dim objINSetup As New agri.IN.clsSetup()
    Dim objPUSetup As New agri.PU.clsSetup()
    Dim objPUTrx As New agri.PU.clsTrx()
    Protected objGLSetup As New agri.GL.clsSetup()
    Protected objGL As New agri.GL.clsSetup()
    Dim objAPTrx As New agri.AP.clsTrx()
    Dim objAdminShare As New agri.Admin.clsShare()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim objGLtrx As New agri.GL.ClsTrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()

    Dim objPODs As New Object()
    Dim objAccDs As New Object()
    Dim objPOLnDs As New Object()
    Dim objInvRcvLnDs As New Object()
    Dim objTermTypeDs As New Object()
    Dim objCreditTermDs As New Object()
    Dim objLangCapDs As DataSet

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intAPAR As Integer
    Dim intPPN As Decimal
    Dim intPPNAmount As Decimal
    Dim intPPHAmount As Decimal
    Dim intAmount As Decimal
    Dim intNetAmount As Decimal
    Dim strItem As String
    Dim strSelectedInvRcvId As String
    Dim strAcceptDateFormat As String
    Dim blnIsUpdated As Boolean = False
    Dim intConfig As Integer
    Dim strLocType As String
    Dim strInvRcvLnID As String
    Dim strPOID As String
    Dim strAccCode As String = ""
    Dim strTermType As String
    Dim blnIsSaved As Boolean = False
    Dim strSelAccMonth As String
    Dim strSelAccYear As String
    Dim intLevel As Integer

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
        strSelAccMonth = Session("SS_SELACCMONTH")
        strSelAccYear = Session("SS_SELACCYEAR")
        intLevel = Session("SS_USRLEVEL")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumAPAccessRights.APInvoiceReceive), intAPAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            lblErrUnDel.Visible = False
            lblErrInvRcvRefDate.Visible = False
            lblErrCurrency.Visible = False
            lblErrPO.Visible = False
            lblerrInvDueDate.Visible = False
            lblerrFakturPjkDate.Visible = False
            lblErrTransDate.Visible = False
            lblErrSplInvAmt.Visible = False
            lblErrCreditTerm.Visible = False
            lblErrTransDate.Visible = False
            lblFmtTransDate.Visible = False
            lblErrFakturPjk.Visible = False

            'to avoid double click, on aspx add this : UseSubmitBehavior="false"
            Addbtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(Addbtn).ToString())
            NewBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(NewBtn).ToString())
            SaveBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(SaveBtn).ToString())
            GenInvBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(GenInvBtn).ToString())
            PrintBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(PrintBtn).ToString())
            CancelBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(CancelBtn).ToString())
            DeleteBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(DeleteBtn).ToString())
            UnDeleteBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(UnDeleteBtn).ToString())
            BackBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(BackBtn).ToString())
            ConfirmBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(ConfirmBtn).ToString())

            strSelectedInvRcvId = Trim(IIf(Request.QueryString("inrid") = "", Request.Form("inrid"), Request.QueryString("inrid")))
            inrid.Value = strSelectedInvRcvId

            txtPPNInit.Attributes.Add("readonly", "readonly")
            txtPPN.Attributes.Add("readonly", "readonly")
            txtSupCode.Attributes.Add("readonly", "readonly")
            txtSupName.Attributes.Add("readonly", "readonly")

            onload_GetLangCap()
            If Not IsPostBack Then
                rbSPO.Text = " " & objAPTrx.mtdGetInvoiceType(objAPTrx.EnumInvoiceType.SupplierPO)
                rbAdvPay.Text = " " & objAPTrx.mtdGetInvoiceType(objAPTrx.EnumInvoiceType.AdvancePayment)
                rbOTE.Text = " " & objAPTrx.mtdGetInvoiceType(objAPTrx.EnumInvoiceType.Others)
                rbFFB.Text = " " & objAPTrx.mtdGetInvoiceType(objAPTrx.EnumInvoiceType.FFBSupplier)
                rbTransportFee.Text = " " & objAPTrx.mtdGetInvoiceType(objAPTrx.EnumInvoiceType.TransportFee)
                rbTransportFee.Visible = False

                If strSelectedInvRcvId <> "" Then
                    onLoad_Display(strSelectedInvRcvId)
                    onLoad_DisplayItem(strSelectedInvRcvId)
                    onLoad_Button()
                Else
                    txtInvoiceRcvRefDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
                    txtTransDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
                    'txtFakturPjkDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
                    txtInvDueDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
                    lblStatusHidden.Text = "0"
                    'BindSupp("")
                    BindPO(txtSupCode.Text)
                    BindTermType("")
                    BindCreditTerm("")
                    onLoad_Button()
                End If
            End If
        End If
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()

        lblTitle.Text = UCase("Invoice Reception/Tanda Terima Tagihan")
        lblInvoiceRcvIDTag.Text = "Invoice Reception" & lblID.Text
        lblInvoiceRcvRefNoTag.Text = "Invoice Reception" & lblRefNo.Text
        lblInvoiceRcvRefDateTag.Text = "Invoice Reception" & lblRefDate.Text
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_TRX_CNDET_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=ap/setup/AP_trx_CNList.aspx")
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
        UnDeleteBtn.Attributes("onclick") = "javascript:return ConfirmAction('undelete');"
        CancelBtn.Attributes("onclick") = "javascript:return ConfirmAction('cancel');"
        GenInvBtn.Attributes("onclick") = "javascript:return ConfirmAction('generate credited invoice');"

        SaveBtn.Visible = False
        GenInvBtn.Visible = False
        PrintBtn.Visible = True
        CancelBtn.Visible = False
        DeleteBtn.Visible = False
        UnDeleteBtn.Visible = False
        txtInvoiceRcvRefNo.Enabled = False
        txtInvoiceRcvRefDate.Enabled = False
        rbSPO.Enabled = False
        rbAdvPay.Enabled = False
        rbOTE.Enabled = False
        rbFFB.Enabled = False
        rbTransportFee.Enabled = False
        txtSupCode.Enabled = False
        txtRemark.Enabled = False
        txtSupCode.Enabled = False
        ddlTermType.Enabled = False
        btnSelDate.Visible = False
        tblSelection.Visible = False
        txtTransDate.Enabled = False
        'txtFakturPjkNo.Enabled = False
        'txtFakturPjkDate.Enabled = False
        txtInvDueDate.Enabled = False
        ddlPO.Enabled = False
        txtReceiveFrom.Enabled = False
        ConfirmBtn.Visible = False
        rbViaBTNo.Enabled = False
        rbViaBTYes.Enabled = False

        Select Case Trim(lblStatusHidden.Text)
            Case Trim(CStr(objAPTrx.EnumInvoiceRcvNoteStatus.Invoiced))
                CancelBtn.Visible = True
                PrintBtn.Visible = True
            Case Trim(CStr(objAPTrx.EnumInvoiceRcvNoteStatus.Deleted))
                UnDeleteBtn.Visible = True
            Case Trim(CStr(objAPTrx.EnumInvoiceRcvNoteStatus.Cancelled))
            Case Else
                SaveBtn.Visible = True
                txtInvoiceRcvRefNo.Enabled = True
                txtInvoiceRcvRefDate.Enabled = True
                txtRemark.Enabled = True
                ddlTermType.Enabled = True
                btnSelDate.Visible = True
                tblSelection.Visible = True
                txtTransDate.Enabled = True
                'txtFakturPjkNo.Enabled = True
                'txtFakturPjkDate.Enabled = True
                txtInvDueDate.Enabled = True
                rbSPO.Enabled = True
                rbAdvPay.Enabled = True
                rbOTE.Enabled = True
                rbFFB.Enabled = True
                rbTransportFee.Enabled = True
                txtSupCode.Enabled = True
                ddlTermType.Enabled = True
                btnSelDate.Visible = True
                txtReceiveFrom.Enabled = True
                rbViaBTNo.Enabled = True
                rbViaBTYes.Enabled = True

                If rbSPO.Checked = True Or rbAdvPay.Checked = True Or rbTransportFee.Checked = True Then
                    ddlPO.Enabled = True
                End If

                If Trim(lblInvoiceRcvID.Text) <> "" Then
                    txtTransDate.Enabled = False
                    If dgLineDet.Items.Count <> 0 Then
                        txtSupCode.Enabled = False
                        rbSPO.Enabled = False
                        rbAdvPay.Enabled = False
                        rbOTE.Enabled = False
                        rbFFB.Enabled = False
                        rbTransportFee.Enabled = False
                        GenInvBtn.Visible = True

                        'If rbAdvPay.checked = True Or rbOTE.checked = True Then
                        '    GenInvBtn.Visible = False
                        '    ConfirmBtn.visible = True
                        'Else
                        '    GenInvBtn.Visible = True
                        '    ConfirmBtn.visible = False
                        'End If

                    End If

                    DeleteBtn.Visible = True
                End If
        End Select


    End Sub

    Sub onLoad_Display(ByVal pv_strInvRcvId As String)
        Dim strOpCd_Get As String = "AP_CLSTRX_INVOICERECEIVENOTE_GET"
        Dim objInvRcvDs As New Object
        Dim intErrNo As Integer
        Dim intCnt As Integer = 0
        Dim strParamName As String
        Dim strParamValue As String

        strParamName = "STRSEARCH"
        strParamValue = "INV.InvoiceRcvID = '" & pv_strInvRcvId & "' AND INV.LocCode = '" & strLocation & "'"

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_Get, _
                                                strParamName, _
                                                strParamValue, _
                                                objInvRcvDs)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_GET_HEADER&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvnotelist.aspx")
        End Try

        lblInvoiceRcvID.Text = pv_strInvRcvId
        txtInvoiceRcvRefNo.Text = Trim(objInvRcvDs.Tables(0).Rows(0).Item("InvoiceRcvRefNo"))
        txtInvoiceRcvRefDate.Text = Date_Validation(objInvRcvDs.Tables(0).Rows(0).Item("InvoiceRcvRefDate"), True)
        idSuppCode.Value = Trim(objInvRcvDs.Tables(0).Rows(0).Item("SupplierCode"))
        lblAccPeriod.Text = Trim(objInvRcvDs.Tables(0).Rows(0).Item("AccMonth")) & "/" & Trim(objInvRcvDs.Tables(0).Rows(0).Item("AccYear"))
        lblStatus.Text = objAPTrx.mtdGetInvoiceRcvNoteStatus(Trim(objInvRcvDs.Tables(0).Rows(0).Item("Status")))
        lblStatusHidden.Text = Trim(objInvRcvDs.Tables(0).Rows(0).Item("Status"))
        lblDateCreated.Text = objGlobal.GetLongDate(objInvRcvDs.Tables(0).Rows(0).Item("CreateDate"))
        lblLastUpdate.Text = objGlobal.GetLongDate(objInvRcvDs.Tables(0).Rows(0).Item("UpdateDate"))
        lblPrintDate.Text = objGlobal.GetLongDate(objInvRcvDs.Tables(0).Rows(0).Item("PrintDate"))
        lblUpdatedBy.Text = Trim(objInvRcvDs.Tables(0).Rows(0).Item("UserName"))
        lblInvoiceAmount.Text = FormatNumber(objInvRcvDs.Tables(0).Rows(0).Item("TotalAmount"), CInt(Session("SS_ROUNDNO")))
        lblIDInvoiceAmount.Text = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(objInvRcvDs.Tables(0).Rows(0).Item("TotalAmount"), CInt(Session("SS_ROUNDNO"))), CInt(Session("SS_ROUNDNO")))
        txtRemark.Text = Trim(objInvRcvDs.Tables(0).Rows(0).Item("Remark"))
        txtInvDueDate.Text = Date_Validation(objInvRcvDs.Tables(0).Rows(0).Item("InvoiceDueDate"), True)
        hidTermType.Value = Trim(objInvRcvDs.Tables(0).Rows(0).Item("TermType"))
        txtTransDate.Text = Date_Validation(objInvRcvDs.Tables(0).Rows(0).Item("CreateDate"), True)
        'txtFakturPjkDate.Text = Date_Validation(objInvRcvDs.Tables(0).Rows(0).Item("FakturPajakDate"), True)
        'txtFakturPjkNo.Text = Trim(objInvRcvDs.Tables(0).Rows(0).Item("FakturPajakNo"))

        txtReceiveFrom.Text = Trim(objInvRcvDs.Tables(0).Rows(0).Item("ReceiveFrom"))
        txtSupName.Text = Trim(objInvRcvDs.Tables(0).Rows(0).Item("SupplierName"))

        If Trim(objInvRcvDs.Tables(0).Rows(0).Item("InvoiceType")) = objAPTrx.EnumInvoiceType.SupplierPO Then
            rbSPO.Checked = True
            rbAdvPay.Checked = False
            rbOTE.Checked = False
            rbFFB.Checked = False
            rbTransportFee.Checked = False
        ElseIf Trim(objInvRcvDs.Tables(0).Rows(0).Item("InvoiceType")) = objAPTrx.EnumInvoiceType.AdvancePayment Then
            rbSPO.Checked = False
            rbAdvPay.Checked = True
            rbOTE.Checked = False
            rbFFB.Checked = False
            rbTransportFee.Checked = False
        ElseIf Trim(objInvRcvDs.Tables(0).Rows(0).Item("InvoiceType")) = objAPTrx.EnumInvoiceType.Others Then
            rbSPO.Checked = False
            rbAdvPay.Checked = False
            rbOTE.Checked = True
            rbFFB.Checked = False
            rbTransportFee.Checked = False
        ElseIf Trim(objInvRcvDs.Tables(0).Rows(0).Item("InvoiceType")) = objAPTrx.EnumInvoiceType.FFBSupplier Then
            rbSPO.Checked = False
            rbAdvPay.Checked = False
            rbOTE.Checked = False
            rbFFB.Checked = True
            rbTransportFee.Checked = False
        Else
            rbSPO.Checked = False
            rbAdvPay.Checked = False
            rbOTE.Checked = False
            rbFFB.Checked = False
            rbTransportFee.Checked = True
        End If

        If objInvRcvDs.Tables(0).Rows(0).Item("ViaBankTransfer") = "1" Then
            rbViaBTYes.Checked = True
            rbViaBTNo.Checked = False
        Else
            rbViaBTYes.Checked = False
            rbViaBTNo.Checked = True
        End If

        'BindPO(Trim(objInvRcvDs.Tables(0).Rows(0).Item("POID")))
        'BindSupp(idSuppCode.Value)
        txtSupCode.Text = idSuppCode.Value
        BindTermType(Trim(objInvRcvDs.Tables(0).Rows(0).Item("TermType")))
        onLoad_DisplayAPNote(lblInvoiceRcvID.Text)
        'If rbAdvPay.Checked = True Or rbOTE.Checked = True Then
        'Else
        '    onLoad_DisplayAPNote(lblInvoiceRcvID.Text)
        'End If
    End Sub

    Sub onLoad_DisplayItem(ByVal pv_strInvoiceRcvID As String)
        Dim strOpCd_GetPOLine As String = "AP_CLSTRX_INVOICERECEIVE_POLINE_GET"
        Dim strOpCd_GetIRLine As String = "AP_CLSTRX_INVOICERECEIVENOTELN_GET"
        Dim strOpCodes As String = strOpCd_GetPOLine & "|" & strOpCd_GetIRLine
        Dim strParam As String = pv_strInvoiceRcvID
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim dr As DataRow
        Dim strParamName As String
        Dim strParamValue As String
        Dim UpdButton As LinkButton
        Dim DelButton As LinkButton
        Dim EdtButton As LinkButton
        Dim CanButton As LinkButton

        strParamName = "STRSEARCH"
        strParamValue = "INV.InvoiceRcvID = '" & pv_strInvoiceRcvID & "' AND INV.LocCode = '" & strLocation & "'"

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_GetIRLine, _
                                                strParamName, _
                                                strParamValue, _
                                                objInvRcvLnDs)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_GET_LINE&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvnotelist.aspx")
        End Try

        dgLineDet.DataSource = objInvRcvLnDs.Tables(0)
        dgLineDet.DataBind()

        For intCnt = 0 To objInvRcvLnDs.Tables(0).Rows.Count - 1
            objInvRcvLnDs.Tables(0).Rows(intCnt).Item("InvoiceRcvLnID") = Trim(objInvRcvLnDs.Tables(0).Rows(intCnt).Item("InvoiceRcvLnID"))
            objInvRcvLnDs.Tables(0).Rows(intCnt).Item("POID") = Trim(objInvRcvLnDs.Tables(0).Rows(intCnt).Item("POID"))
            objInvRcvLnDs.Tables(0).Rows(intCnt).Item("Amount") = Trim(objInvRcvLnDs.Tables(0).Rows(intCnt).Item("SupplierInvAmount"))
            strInvRcvLnID = objInvRcvLnDs.Tables(0).Rows(intCnt).Item("InvoiceRcvLnID")
            hidCurrencyCode.Value = Trim(objInvRcvLnDs.Tables(0).Rows(intCnt).Item("CurrencyCode"))
            lblCurrency1.Text = Trim(objInvRcvLnDs.Tables(0).Rows(0).Item("CurrencyCode"))

            Select Case CInt(lblStatusHidden.Text)
                Case objAPTrx.EnumInvoiceRcvNoteStatus.Invoiced
                    EdtButton = dgLineDet.Items.Item(intCnt).FindControl("lbEdit")
                    EdtButton.Visible = True
                    DelButton = dgLineDet.Items.Item(intCnt).FindControl("lbDelete")
                    DelButton.Visible = False
                    UpdButton = dgLineDet.Items.Item(intCnt).FindControl("lbUpdate")
                    UpdButton.Visible = False
                    CanButton = dgLineDet.Items.Item(intCnt).FindControl("lbCancel")
                    CanButton.Visible = False
                Case objAPTrx.EnumInvoiceRcvNoteStatus.Deleted, objAPTrx.EnumInvoiceRcvNoteStatus.Cancelled
                    EdtButton = dgLineDet.Items.Item(intCnt).FindControl("lbEdit")
                    EdtButton.Visible = False
                    DelButton = dgLineDet.Items.Item(intCnt).FindControl("lbDelete")
                    DelButton.Visible = False
                    UpdButton = dgLineDet.Items.Item(intCnt).FindControl("lbUpdate")
                    UpdButton.Visible = False
                    CanButton = dgLineDet.Items.Item(intCnt).FindControl("lbCancel")
                    CanButton.Visible = False
                Case Else
                    DelButton = dgLineDet.Items.Item(intCnt).FindControl("lbDelete")
                    DelButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                    DelButton.Visible = True
                    EdtButton = dgLineDet.Items.Item(intCnt).FindControl("lbEdit")
                    EdtButton.Visible = True
                    CanButton = dgLineDet.Items.Item(intCnt).FindControl("lbCancel")
                    CanButton.Visible = False
                    UpdButton = dgLineDet.Items.Item(intCnt).FindControl("lbUpdate")
                    UpdButton.Visible = False
            End Select
        Next intCnt



        BindPO(txtSupCode.Text)

        If objInvRcvLnDs.Tables(0).Rows.Count = 0 Then
            Dim objAPNote As Object
            Dim strOpCd_APNote As String
            Dim strAPNoteID As String

            strOpCd_APNote = "AP_CLSTRX_INVOICERCV_NOTE_DELETE"
            strParamName = "STRSEARCH"
            strParamValue = "InvoiceRcvID = '" & Trim(pv_strInvoiceRcvID) & "' "

            Try
                intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd_APNote, _
                                                        strParamName, _
                                                        strParamValue)

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=UPD_ITEM&errmesg=" & Exp.ToString() & "&redirect=pu/trx/AP_trx_InvoiceRcvList")
            End Try

            onLoad_DisplayAPNote(lblInvoiceRcvID.Text)
            'If rbAdvPay.Checked = True Or rbOTE.Checked = True Then
            'Else
            '    onLoad_DisplayAPNote(lblInvoiceRcvID.Text)
            'End If
        End If
    End Sub

    'Sub DataGrid_ItemData(ByVal Sender As Object, ByVal e As DataGridItemEventArgs) Handles dgLineDet.ItemDataBound
    '    Dim UpdButton As LinkButton
    '    Dim DelButton As LinkButton
    '    Dim EdtButton As LinkButton
    '    Dim CanButton As LinkButton

    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        Select Case CInt(lblStatusHidden.Text)
    '            Case objAPTrx.EnumInvoiceRcvNoteStatus.Invoiced
    '                EdtButton = e.Item.FindControl("lbEdit")
    '                EdtButton.Visible = True
    '                DelButton = e.Item.FindControl("lbDelete")
    '                DelButton.Visible = False
    '                UpdButton = e.Item.FindControl("lbUpdate")
    '                UpdButton.Visible = False
    '                CanButton = e.Item.FindControl("lbCancel")
    '                CanButton.Visible = False
    '            Case objAPTrx.EnumInvoiceRcvNoteStatus.Deleted, objAPTrx.EnumInvoiceRcvNoteStatus.Cancelled
    '                EdtButton = e.Item.FindControl("lbEdit")
    '                EdtButton.Visible = False
    '                DelButton = e.Item.FindControl("lbDelete")
    '                DelButton.Visible = False
    '                UpdButton = e.Item.FindControl("lbUpdate")
    '                UpdButton.Visible = False
    '                CanButton = e.Item.FindControl("lbCancel")
    '                CanButton.Visible = False
    '            Case Else
    '                DelButton = e.Item.FindControl("lbDelete")
    '                DelButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
    '                DelButton.Visible = True
    '                EdtButton = e.Item.FindControl("lbEdit")
    '                EdtButton.Visible = True
    '                CanButton = e.Item.FindControl("lbCancel")
    '                CanButton.Visible = False
    '                UpdButton = e.Item.FindControl("lbUpdate")
    '                UpdButton.Visible = False
    '        End Select
    '    End If
    'End Sub

    Sub BindPO(ByVal pv_strSuppCode As String)
        Dim strOpCd_GetPO As String = "PU_CLSTRX_PO_INVOICERECEIVE_GET"
        Dim strOpCd_GetPOLn As String = "PU_CLSTRX_PO_INVOICERECEIVE_GET_AMOUNT"
        Dim strOpCd_Get As String = "AP_CLSTRX_INVOICERECEIVE_DETAILS_GET"
        Dim strParam As String = "||||||||"
        Dim intCnt As Integer = 0
        Dim intErrNo As Integer
        Dim intSelectedIndex As Integer = 0
        Dim blnFound As Boolean = False
        Dim dr As DataRow
        Dim strPOID As String
        Dim strParamName As String
        Dim strParamValue As String

        If rbAdvPay.Checked = True Then
            strOpCd_GetPO = "PU_CLSTRX_PO_INVOICERECEIVE_GET_ADVANCE"
        End If

        'strParam = "|" & strLocation & "||" & IIf(Trim(txtSupCode.text) = "", "XXX", Trim(txtSupCode.text)) & "||||A.POID|"
        strParamName = "STRSEARCH|PPNRATE"
        If lblInvoiceRcvID.Text = "" Then
            strParamValue = "AND A.LocCode = '" & Trim(strLocation) & "' AND A.SupplierCode LIKE '" & IIf(pv_strSuppCode = "", "XXX", pv_strSuppCode) & "'" & _
			"|" & Session("SS_PPNRATE") '& _
            '" AND A.POID NOT IN " & _
            '" (Select Distinct POID From AP_InvoiceRcvNoteLn LN, AP_InvoiceRcvNote AP Where LN.InvoiceRcvID=AP.InvoiceRcvID AND Status Not In ('3','4')) "
        Else
            strParamValue = "AND A.LocCode = '" & Trim(strLocation) & "' AND A.SupplierCode LIKE '" & IIf(pv_strSuppCode = "", "XXX", pv_strSuppCode) & "'" & _
            " AND RTRIM(A.POID)+RTRIM(A.TotalAmount) NOT IN " & _
            " (Select Distinct rtrim(POID)+RTRIM(ln.SupplierInvAmount) From AP_InvoiceRcvNoteLn LN, AP_InvoiceRcvNote AP Where LN.InvoiceRcvID=AP.InvoiceRcvID AND Status Not In ('3','4')) " & _
			"|" & Session("SS_PPNRATE") '& _

            '" AND A.POID NOT IN " & _
            '" (Select Distinct POID From AP_InvoiceRcvNoteLn LN, AP_InvoiceRcvNote AP Where LN.InvoiceRcvID=AP.InvoiceRcvID AND Status Not In ('3','4')) "
        End If

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_GetPO, _
                                                strParamName, _
                                                strParamValue, _
                                                objPODs)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_GET_PODETAIL&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
        End Try


        For intCnt = 0 To objPODs.Tables(0).Rows.Count - 1
            objPODs.Tables(0).Rows(intCnt).Item("POID") = Trim(objPODs.Tables(0).Rows(intCnt).Item("POID"))
            objPODs.Tables(0).Rows(intCnt).Item("DispPOId") = Trim(objPODs.Tables(0).Rows(intCnt).Item("POId")) & ", " & _
                                                              IIf(Trim(objPODs.Tables(0).Rows(intCnt).Item("Remark")) = "", "-", Trim(objPODs.Tables(0).Rows(intCnt).Item("Remark"))) & ", " & _
                                                              Trim(objPODs.Tables(0).Rows(intCnt).Item("CurrencyCode")) & " " & _
                                                              objPODs.Tables(0).Rows(intCnt).Item("AmountToDisplay") & ", " & _
                                                              Trim(objPODs.Tables(0).Rows(intCnt).Item("ExchangeRate")) & ", " & _
                                                              "Rp. " & objPODs.Tables(0).Rows(intCnt).Item("Amount")

            'If pv_strPOID = objPODs.Tables(0).Rows(intCnt).Item("POID") Then
            '    intSelectedIndex = intCnt + 1
            '    blnFound = True
            'End If
        Next intCnt

        'If blnFound = True Then
        '    If objPODs.Tables(0).Rows.Count > 0 Then
        '        idSuppCode.Value = Trim(objPODs.Tables(0).Rows(intSelectedIndex - 1).Item("SupplierCode"))
        '        txtCreditTerm.Text = Trim(objPODs.Tables(0).Rows(intSelectedIndex - 1).Item("POCreditTerm"))
        '        hidExchangeRate.Value = Trim(objPODs.Tables(0).Rows(intSelectedIndex - 1).Item("ExchangeRate"))

        '        strParamName = "STRSEARCH"
        '        If lblInvoiceRcvID.Text <> "" Then
        '            strParamValue = "AND D.SUPPLIERCODE = '" & ddlSuppCode.SelectedItem.Value & "' " & _
        '                            " AND D.LocCode = '" & Trim(strLocation) & "'" & _
        '                            " AND D.POID NOT IN (Select POID From AP_InvoiceRcvNoteLn Where InvoiceRcvID = '" & Trim(lblInvoiceRcvID.Text) & "')" & _
        '                            " AND A.QtyReceive > 0 AND A.QtyInvoice < A.QtyReceive"
        '        Else
        '            strParamValue = "AND D.SUPPLIERCODE = '" & ddlSuppCode.SelectedItem.Value & "' " & _
        '                            " AND D.POId = '" & IIf(pv_strPOID = "", objPODs.Tables(0).Rows(intSelectedIndex - 1).Item("POID"), pv_strPOID) & "' " & _
        '                            " AND D.LocCode = '" & Trim(strLocation) & "' " & _
        '                            " AND A.QtyReceive > 0 AND A.QtyInvoice < A.QtyReceive"
        '        End If

        '        Try
        '            intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_GetPOLn, _
        '                                                strParamName, _
        '                                                strParamValue, _
        '                                                objPOLnDs)

        '        Catch Exp As System.Exception
        '            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_GET_POLINE&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
        '        End Try

        '        If objPOLnDs.Tables(0).Rows.Count > 0 Then
        '            txtSplInvAmt.Text = objPOLnDs.Tables(0).Rows(0).Item("AmountToDisplay")
        '            hidPOItem.Value = Trim(objPOLnDs.Tables(0).Rows(0).Item("POId")) & "|" & Trim(objPOLnDs.Tables(0).Rows(0).Item("CurrencyCode")) & "|" & objPOLnDs.Tables(0).Rows(0).Item("ExchangeRate") & "|" & objPOLnDs.Tables(0).Rows(0).Item("Amount") & "|" & objPOLnDs.Tables(0).Rows(0).Item("AmountToDisplay")
        '        End If
        '    End If

        'ElseIf pv_strPOID <> "" Then
        '    dr = objPODs.Tables(0).NewRow()
        '    dr("POID") = pv_strPOID
        '    objPODs.Tables(0).Rows.InsertAt(dr, objPODs.Tables(0).Rows.Count)
        '    intSelectedIndex = objPODs.Tables(0).Rows.Count
        'End If

        dr = objPODs.Tables(0).NewRow()
        dr("POId") = ""
        dr("DispPOId") = lblPleaseSelect.Text & "Purchase Order ID"
        objPODs.Tables(0).Rows.InsertAt(dr, 0)

        ddlPO.DataSource = objPODs.Tables(0)
        ddlPO.DataValueField = "POId"
        ddlPO.DataTextField = "DispPOId"
        ddlPO.DataBind()
        ddlPO.SelectedIndex = intSelectedIndex
        If ddlPO.SelectedIndex <> -1 Then
            strPOID = ddlPO.SelectedItem.Value
        End If
    End Sub

    Sub BindCreditTerm(ByVal pv_strSelectedSuppCode As String)
        'temporary being remark coz STA take credit term on PO

        'Dim strOpCd_GetCreditTerm As String = "PU_CLSSETUP_SUPPLIER_GET"
        'Dim strParam As String = Trim(pv_strSelectedSuppCode) & "||||SupplierCode||"
        'Dim intCnt As Integer = 0
        'Dim intErrNo As Integer
        'Dim intDefIndex As Integer = 0
        'Dim intSelectedIndex As Integer = 0
        'Dim crtFound As Boolean = False

        'If Trim(pv_strSelectedSuppCode) = "" Then
        '    Exit Sub
        'End If

        'Try
        '    intErrNo = objPUSetup.mtdGetSupplier(strOpCd_GetCreditTerm, strParam, objCreditTermDs)
        'Catch Exp As System.Exception
        '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PU_GET_SUPPLIER&errmesg=" & lblErrMessage.Text & "&redirect=pu/trx/pu_trx_polist.aspx")
        'End Try

        'If objCreditTermDs.Tables(0).Rows.Count > 0 Then
        '    txtCreditTerm.Text = Trim(objCreditTermDs.Tables(0).Rows(0).Item("CreditTerm"))
        'End If
    End Sub

    Sub BindTermType(ByVal pv_TermType As String)
        Dim strOpCd_GetTermType As String = "ADMIN_CLSSHARE_CREDITTERMTYPE_LIST_GET"
        Dim strParam As String = "DefaultInd=0"
        Dim intCnt As Integer = 0
        Dim intErrNo As Integer
        Dim intDefIndex As Integer = 0
        Dim intSelectedIndex As Integer = 0


        Try
            intErrNo = objAdminShare.mtdGetCreditTermType(strOpCd_GetTermType, strParam, objTermTypeDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_GET_TERMTYPELIST&errmesg=" & lblErrMessage.Text & "&redirect=pu/trx/pu_polist.aspx")
        End Try

        For intCnt = 0 To objTermTypeDs.Tables(0).Rows.Count - 1
            objTermTypeDs.Tables(0).Rows(intCnt).Item("CreditTermTypeCode") = Trim(objTermTypeDs.Tables(0).Rows(intCnt).Item("CreditTermTypeCode"))
            objTermTypeDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objTermTypeDs.Tables(0).Rows(intCnt).Item("Description"))

            If objTermTypeDs.Tables(0).Rows(intCnt).Item("CreditTermTypeCode") = pv_TermType Then
                intSelectedIndex = intCnt
            End If
        Next intCnt

        If pv_TermType = "" Then
            intSelectedIndex = 1
        End If

        ddlTermType.DataSource = objTermTypeDs.Tables(0)
        ddlTermType.DataValueField = "CreditTermTypeCode"
        ddlTermType.DataTextField = "Description"
        ddlTermType.DataBind()
        ddlTermType.SelectedIndex = intSelectedIndex
    End Sub

    Sub onSelect_Change(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim strOpCd_GetPOLn As String = "PU_CLSTRX_PO_INVOICERECEIVE_GET_AMOUNT"
        Dim intCnt As Integer = 0
        Dim intErrNo As Integer
        Dim intSelectedIndex As Integer = 0
        Dim blnFound As Boolean = False
        Dim strParamName As String
        Dim strParamValue As String
        Dim strPOCurrency As String
        Dim dblPORate As Double

        strParamName = "STRSEARCH|PPNRATE"

        If rbAdvPay.Checked = True Then
            strOpCd_GetPOLn = "PU_CLSTRX_PO_INVOICERECEIVE_GET_ADVANCE_AMOUNT"
            strParamValue = "AND D.SUPPLIERCODE = '" & txtSupCode.Text & "' " & _
                        " AND A.POId = '" & ddlPO.SelectedItem.Value & "' " & _
                        " AND D.LocCode = '" & Trim(strLocation) & "' " & _
						"|" & Session("SS_PPNRATE") '& _
        Else
            strParamValue = "AND D.SUPPLIERCODE = '" & txtSupCode.Text & "' " & _
                                   " AND D.POId = '" & ddlPO.SelectedItem.Value & "' " & _
                                   " AND D.LocCode = '" & Trim(strLocation) & "' " & _
                                   " AND A.QtyReceive > 0 AND A.QtyInvoice < A.QtyReceive" & _
								   "|" & Session("SS_PPNRATE") '& _
        End If

       
        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_GetPOLn, _
                                                strParamName, _
                                                strParamValue, _
                                                objPOLnDs)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_GET_POLINE&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
        End Try

        If objPOLnDs.Tables(0).Rows.Count > 0 Then
            txtCreditTerm.Text = objPOLnDs.Tables(0).Rows(0).Item("CreditTerm")
            If txtCreditTerm.Text = "0" Or txtCreditTerm.Text = "" Then
                txtCreditTerm.Text = "7"
            End If
            hidPOAmount.Value = objPOLnDs.Tables(0).Rows(0).Item("AmountToDisplay")
            txtSplInvAmt.Text = objPOLnDs.Tables(0).Rows(0).Item("AmountToDisplay")
            hidPOItem.Value = Trim(objPOLnDs.Tables(0).Rows(0).Item("POId")) & "|" & Trim(objPOLnDs.Tables(0).Rows(0).Item("CurrencyCode")) & "|" & objPOLnDs.Tables(0).Rows(0).Item("ExchangeRate") & "|" & objPOLnDs.Tables(0).Rows(0).Item("Amount") & "|" & objPOLnDs.Tables(0).Rows(0).Item("AmountToDisplay")
            strPOCurrency = objPOLnDs.Tables(0).Rows(0).Item("CurrencyCode")
            dblPORate = objPOLnDs.Tables(0).Rows(0).Item("ExchangeRate")
            txtSplTaxAmt.Text = objPOLnDs.Tables(0).Rows(0).Item("PPNAmount")
        End If

        Dim strInvoiceRcvId As String = strSelectedInvRcvId
        Dim strDueDate As String
        Dim strPODueDate As String
        Dim strGRDate As String
        Dim objLastGR As Object
        Dim strOpCd_LastGR As String = "AP_CLSTRX_INVOICERECEIVE_LASTGR_GET"
        Dim strDate As String = Date_Validation(txtTransDate.Text, False)
        Dim intCreditTerm As Integer = CInt(txtCreditTerm.Text)

        If ddlPO.SelectedItem.Value = "" Then
            strDueDate = strDate
        Else
            strParamName = "LOCCODE|POID"
            strParamValue = strLocation & "|" & ddlPO.SelectedItem.Value

            Try
                intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_LastGR, _
                                                    strParamName, _
                                                    strParamValue, _
                                                    objLastGR)

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_CLSTRX_INVOICERECEIVE_LASTGR_GET&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
            End Try

            If objLastGR.Tables(0).Rows.Count > 0 Then
                strGRDate = Date_Validation(objLastGR.Tables(0).Rows(0).Item("GoodsRcvRefDate"), True)
                strGRDate = Date_Validation(strGRDate, False)
            End If

            If ddlTermType.SelectedItem.Value = "11" Then
                strDueDate = DateAdd(DateInterval.Day, intCreditTerm, CDate(strDate))
            Else
                strDueDate = DateAdd(DateInterval.Day, intCreditTerm, CDate(strGRDate))
            End If

            strPODueDate = strDueDate
            Select Case True
                Case CDate(strDueDate) <= CDate(strDate)
                    strDueDate = DateAdd(DateInterval.Day, 7, CDate(strDate))
                Case DateDiff(DateInterval.Day, CDate(strDate), CDate(strDueDate)) <= 7
                    strDueDate = DateAdd(DateInterval.Day, 7, CDate(strDueDate))
            End Select

            Select Case DatePart(DateInterval.Weekday, CDate(strDueDate))
                Case vbMonday
                    strDueDate = strDueDate
                Case vbTuesday
                    strDueDate = DateAdd(DateInterval.Day, 2, CDate(strDueDate))
                Case vbWednesday
                    strDueDate = DateAdd(DateInterval.Day, 1, CDate(strDueDate))
                Case vbThursday
                    strDueDate = strDueDate
                Case vbFriday
                    strDueDate = DateAdd(DateInterval.Day, 3, CDate(strDueDate))
                Case vbSaturday
                    strDueDate = DateAdd(DateInterval.Day, 2, CDate(strDueDate))
                Case vbSunday
                    strDueDate = DateAdd(DateInterval.Day, 1, CDate(strDueDate))
            End Select
        End If

        txtPODueDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), strPODueDate)
        txtDueDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), strDueDate)

        Dim strOpCd_GetAdvPayment = "AP_CLSTRX_INVOICERECEIVE_GET_ADVANCE_PAYMENT"
        Dim strAdvAmount As String
        Dim strAdvCurrencyCode As String
        Dim dbCBExchangeRate As Double
        Dim dbAmount As Double
        Dim dbAmountToDisplay As Double
        Dim dbUsedAdvAmount As Double
        Dim dbUsedAdvAmountToDisplay As Double

        If ddlPO.SelectedItem.Value <> "" Then
            strParamName = "POID|LOCCODE"
            strParamValue = ddlPO.SelectedItem.Value & "|" & strLocation

            Try
                intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_GetAdvPayment, _
                                                    strParamName, _
                                                    strParamValue, _
                                                    objPOLnDs)

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_GET_POLINE&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
            End Try

            If objPOLnDs.Tables(0).Rows.Count > 0 Then
                For intCnt = 0 To objPOLnDs.Tables(0).Rows.Count - 1
                    strAdvCurrencyCode = Trim(objPOLnDs.Tables(0).Rows(intCnt).Item("CurrencyCode"))
                    dbCBExchangeRate = objPOLnDs.Tables(0).Rows(intCnt).Item("CBExchangeRate")

                    dbAmountToDisplay = dbAmountToDisplay + objPOLnDs.Tables(0).Rows(intCnt).Item("AmountToDisplay")
                    dbUsedAdvAmountToDisplay = dbUsedAdvAmountToDisplay + objPOLnDs.Tables(0).Rows(intCnt).Item("UsedAdvAmountToDisplay")

                    dbAmount = dbAmount + objPOLnDs.Tables(0).Rows(intCnt).Item("Amount")
                    dbUsedAdvAmount = dbUsedAdvAmount + objPOLnDs.Tables(0).Rows(intCnt).Item("UsedAdvAmount")
                Next

                If Trim(strPOCurrency) = Trim(strAdvCurrencyCode) Then
                    hidAdvAmount.Value = dbAmountToDisplay - dbUsedAdvAmountToDisplay
                    strAdvAmount = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(hidAdvAmount.Value, CInt(Session("SS_ROUNDNO"))), CInt(Session("SS_ROUNDNO")))
                    lblAdvancePayment.Text = strAdvAmount
                Else
                    If Trim(strPOCurrency) = "IDR" Then
                        hidAdvAmount.Value = (dbAmountToDisplay - dbUsedAdvAmountToDisplay) * dbCBExchangeRate
                        strAdvAmount = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(hidAdvAmount.Value, CInt(Session("SS_ROUNDNO"))), CInt(Session("SS_ROUNDNO")))
                        lblAdvancePayment.Text = strAdvAmount & " (" & Trim(strAdvCurrencyCode) & " " & objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(dbAmountToDisplay - dbUsedAdvAmountToDisplay, CInt(Session("SS_ROUNDNO"))), CInt(Session("SS_ROUNDNO"))) & ")"
                    Else
                        hidAdvAmount.Value = (dbAmount - dbUsedAdvAmount) / dblPORate
                        strAdvAmount = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(hidAdvAmount.Value, CInt(Session("SS_ROUNDNO"))), CInt(Session("SS_ROUNDNO")))
                        lblAdvancePayment.Text = strAdvAmount & " (" & Trim(strAdvCurrencyCode) & " " & objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(dbAmountToDisplay - dbUsedAdvAmountToDisplay, CInt(Session("SS_ROUNDNO"))), CInt(Session("SS_ROUNDNO"))) & ")"
                    End If
                End If

                'If Trim(strPOCurrency) = Trim(objPOLnDs.Tables(0).Rows(0).Item("CurrencyCode")) Then
                '    hidAdvAmount.Value = objPOLnDs.Tables(0).Rows(0).Item("AmountToDisplay") - objPOLnDs.Tables(0).Rows(0).Item("UsedAdvAmountToDisplay")
                '    strAdvAmount = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(hidAdvAmount.Value, CInt(Session("SS_ROUNDNO"))), CInt(Session("SS_ROUNDNO")))
                '    lblAdvancePayment.Text = strAdvAmount
                'Else
                '    If Trim(strPOCurrency) = "IDR" Then
                '        hidAdvAmount.Value = (objPOLnDs.Tables(0).Rows(0).Item("AmountToDisplay") - objPOLnDs.Tables(0).Rows(0).Item("UsedAdvAmountToDisplay")) * objPOLnDs.Tables(0).Rows(0).Item("CBExchangeRate")
                '        strAdvAmount = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(hidAdvAmount.Value, CInt(Session("SS_ROUNDNO"))), CInt(Session("SS_ROUNDNO")))
                '        lblAdvancePayment.Text = strAdvAmount & " (" & objPOLnDs.Tables(0).Rows(0).Item("CurrencyCode") & " " & objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(objPOLnDs.Tables(0).Rows(0).Item("AmountToDisplay") - objPOLnDs.Tables(0).Rows(0).Item("UsedAdvAmountToDisplay"), CInt(Session("SS_ROUNDNO"))), CInt(Session("SS_ROUNDNO"))) & ")"
                '    Else
                '        hidAdvAmount.Value = (objPOLnDs.Tables(0).Rows(0).Item("Amount") - objPOLnDs.Tables(0).Rows(0).Item("UsedAdvAmount")) / dblPORate
                '        strAdvAmount = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(hidAdvAmount.Value, CInt(Session("SS_ROUNDNO"))), CInt(Session("SS_ROUNDNO")))
                '        lblAdvancePayment.Text = strAdvAmount & " (" & objPOLnDs.Tables(0).Rows(0).Item("CurrencyCode") & " " & objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(objPOLnDs.Tables(0).Rows(0).Item("AmountToDisplay") - objPOLnDs.Tables(0).Rows(0).Item("UsedAdvAmountToDisplay"), CInt(Session("SS_ROUNDNO"))), CInt(Session("SS_ROUNDNO"))) & ")"
                '    End If
                'End If
            Else
                hidAdvAmount.Value = 0
                lblAdvancePayment.Text = 0
            End If
        Else
            txtSplInvAmt.Text = 0
            txtSplTaxAmt.Text = 0
        End If

        Dim strOpCd_GetAdjAmount = "AP_CLSTRX_INVOICERECEIVE_GET_DEBITCREDITNOTE"
        Dim strAdjAmount As String

        If ddlPO.SelectedItem.Value <> "" Then
            strParamName = "POID|SUPPLIERCODE|LOCCODE"
            strParamValue = ddlPO.SelectedItem.Value & "|" & Trim(txtSupCode.Text) & "|" & strLocation

            Try
                intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_GetAdjAmount, _
                                                    strParamName, _
                                                    strParamValue, _
                                                    objPOLnDs)

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_GET_POLINE&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
            End Try

            If objPOLnDs.Tables(0).Rows.Count > 0 Then
                For intCnt = 0 To objPOLnDs.Tables(0).Rows.Count - 1
                    hidAdjAmount.Value = CDbl(hidAdjAmount.Value) + objPOLnDs.Tables(0).Rows(intCnt).Item("TotalAmountToDisplay") - objPOLnDs.Tables(0).Rows(intCnt).Item("UsedDNCNAmountToDisplay")
                Next
                'hidAdjAmount.Value = objPOLnDs.Tables(0).Rows(0).Item("TotalAmountToDisplay") - objPOLnDs.Tables(0).Rows(0).Item("UsedDNCNAmountToDisplay")
                strAdjAmount = objGlobal.GetIDDecimalSeparator_FreeDigit(FormatNumber(hidAdjAmount.Value, CInt(Session("SS_ROUNDNO"))), CInt(Session("SS_ROUNDNO")))
                lblAdjAmount.Text = strAdjAmount
            Else
                hidAdjAmount.Value = 0
                lblAdjAmount.Text = 0
            End If
        End If

        txtSplInvAmt.Text = CDbl(txtSplInvAmt.Text) - CDbl(hidAdvAmount.Value) + CDbl(hidAdjAmount.Value)
        BindTermType("")
        onLoad_Button()
    End Sub

    Function CheckDate(ByVal pv_strDate As String, ByRef pr_strDate As String) As Boolean
        Dim strDateFormatCode As String = Session("SS_DATEFMT")
        Dim strDateFormat As String

        pr_strDate = ""
        CheckDate = True
        If Not pv_strDate = "" Then
            If objGlobal.mtdValidInputDate(strDateFormatCode, pv_strDate, strDateFormat, pr_strDate) = False Then
                lblFmtTransDate.Text = strDateFormat
                lblFmtInvRcvRefDate.Text = strDateFormat
                lblFmtInvDueDate.Text = strDateFormat
                lblFmtFakturPjkDate.Text = strDateFormat
                pr_strDate = ""
                CheckDate = False
            End If
        End If
    End Function

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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERCV_GET_CONFIG&errmesg=" & lblErrMessage.Text & "&redirect=pu/trx/pu_polist.aspx")
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

    Sub Update_InvoiceRcv(ByVal intStatus As Integer)
        Dim objInvRcvId As New Object
        Dim strInvoiceRcvId As String = strSelectedInvRcvId
        Dim strInvRcvRefNo As String = txtInvoiceRcvRefNo.Text
        Dim strInvRcvRefDate As String = Date_Validation(txtInvoiceRcvRefDate.Text, False)
        Dim strFakturPajakNo As String = "" 'txtFakturPjkNo.Text
        Dim strFakturPajakDate As String = Date_Validation("1/1/1900", False)
        Dim strSupplierCode As String = idSuppCode.Value
        Dim strCreditTerm As String = txtCreditTerm.Text
        Dim strTermType As String = ddlTermType.SelectedItem.Value
        Dim strRemark As String = Trim(Replace(txtRemark.Text, "'", "''"))
        Dim strReceiveFrom As String = Trim(Replace(txtReceiveFrom.Text, "'", "''"))
        Dim strInvType As String
        Dim dblTotalAmount As Double = 0
        Dim intCnt As Integer
        Dim intErrNo As Integer
        Dim strParamName As String
        Dim strParamValue As String
        Dim objAPNote As Object
        Dim strOpCd_APNote As String
        Dim strOpCd_GetLastNo As String
        Dim strOpCd_LastGR As String = "AP_CLSTRX_INVOICERECEIVE_LASTGR_GET"
        Dim strAPNoteID As String
        Dim dgLine As DataGridItem
        Dim dblAmount As Double
        Dim lblLineAmt As Label
        Dim strDueDate As String
        Dim strDate As String = Date_Validation(txtTransDate.Text, False)
        Dim indDate As String = ""

        If CheckDate(txtTransDate.Text.Trim(), indDate) = False Then
            lblErrTransDate.Visible = True
            lblFmtTransDate.Visible = True
            lblErrTransDate.Text = "<br>Date Entered should be in the format"
            Exit Sub
        End If

        Dim intInputPeriod As Integer = Year(strDate) * 100 + Month(strDate)
        Dim intCurPeriod As Integer = (CInt(strAccYear) * 100) + CInt(strAccMonth)
        Dim intSelPeriod As Integer = (CInt(strSelAccYear) * 100) + CInt(strSelAccMonth)

        If Session("SS_FILTERPERIOD") = "0" Then
            If intCurPeriod < intInputPeriod Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "Invalid transaction date."
                Exit Sub
            End If
        Else
            If intSelPeriod <> intInputPeriod Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "Invalid transaction date."
                Exit Sub
            End If
            If intSelPeriod < intCurPeriod And intLevel < 2 Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "This period already locked."
                Exit Sub
            End If
        End If

        txtInvoiceRcvRefDate.Text = txtTransDate.Text
        strInvRcvRefDate = txtTransDate.Text
        strInvRcvRefDate = Date_Validation(strInvRcvRefDate, False)

        If CheckDate(txtInvoiceRcvRefDate.Text.Trim(), indDate) = False Then
            lblErrInvRcvRefDate.Visible = True
            lblFmtInvRcvRefDate.Visible = True
            lblErrInvRcvRefDate.Text = "<br>Date Entered should be in the format"
            Exit Sub
        End If

        If strInvoiceRcvId <> "" Then
            Dim arrParam As Array
            arrParam = Split(lblAccPeriod.Text, "/")
            If Month(strDate) <> arrParam(0) Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "Invalid transaction date."
                Exit Sub
            ElseIf Year(strDate) <> arrParam(1) Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "Invalid transaction date."
                Exit Sub
            End If
        End If

        blnIsUpdated = False
        If Len(txtSupCode.Text) = 0 Then
            lblErrSuppCode.Visible = True
            Exit Sub
        Else
            lblErrSuppCode.Visible = False
        End If
        strSupplierCode = txtSupCode.Text

        If rbSPO.Checked = True Then
            strInvType = objAPTrx.EnumInvoiceType.SupplierPO
        ElseIf rbAdvPay.Checked = True Then
            strInvType = objAPTrx.EnumInvoiceType.AdvancePayment
        ElseIf rbOTE.Checked = True Then
            strInvType = objAPTrx.EnumInvoiceType.Others
        ElseIf rbFFB.Checked = True Then
            strInvType = objAPTrx.EnumInvoiceType.FFBSupplier
        ElseIf rbTransportFee.Checked = True Then
            strInvType = objAPTrx.EnumInvoiceType.TransportFee
        End If

        For intCnt = 0 To dgLineDet.Items.Count - 1
            dgLine = dgLineDet.Items(intCnt)
            lblLineAmt = dgLine.FindControl("lblAmount")
            dblAmount = CDbl(lblLineAmt.Text)
            dblTotalAmount += dblAmount
        Next

        If Year(strDate) * 100 + Month(strDate) < Year(strInvRcvRefDate) * 100 + Month(strInvRcvRefDate) Then
            lblErrInvRcvRefDate.Visible = True
            lblErrInvRcvRefDate.Text = "Invalid transaction date."
            Exit Sub
        End If

        If blnIsSaved = False Then
            If intStatus = objAPTrx.EnumInvoiceRcvNoteStatus.Active Then
                strDueDate = DateAdd(DateInterval.Day, CInt(txtCreditTerm.Text), CDate(strDate))
                Select Case DatePart(DateInterval.Weekday, CDate(strDueDate))
                    Case vbMonday
                        strDueDate = strDueDate
                    Case vbTuesday
                        strDueDate = DateAdd(DateInterval.Day, 2, CDate(strDueDate))
                    Case vbWednesday
                        strDueDate = DateAdd(DateInterval.Day, 1, CDate(strDueDate))
                    Case vbThursday
                        strDueDate = strDueDate
                    Case vbFriday
                        strDueDate = DateAdd(DateInterval.Day, 3, CDate(strDueDate))
                    Case vbSaturday
                        strDueDate = DateAdd(DateInterval.Day, 2, CDate(strDueDate))
                    Case vbSunday
                        strDueDate = DateAdd(DateInterval.Day, 1, CDate(strDueDate))
                End Select
            End If
        Else
            strDueDate = Date_Validation(txtInvDueDate.Text, False)
            If CDate(strDueDate) < CDate(strDate) Then
                lblerrInvDueDate.Visible = True
                lblerrInvDueDate.Text = "Invalid invoice due date."
                Exit Sub
            End If
        End If

        strAccYear = Year(strDate)
        strAccMonth = Month(strDate)

        If strInvoiceRcvId = "" Then
            strOpCd_GetLastNo = "AP_CLSTRX_INVOICERCVNOTE_GET_LASTNO"
            strParamName = "STRSEARCH"
            strParamValue = "LocCode = '" & strLocation & "' And AccMonth = '" & strAccMonth & "' And AccYear = '" & strAccYear & "' "

            Try
                intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_GetLastNo, _
                                                    strParamName, _
                                                    strParamValue, _
                                                    objAPNote)

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_CLSTRX_INVOICERCV_NOTE_GET_LASTNO&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
            End Try

            If objAPNote.Tables(0).Rows.Count > 0 Then
                strAPNoteID = Format(objAPNote.Tables(0).Rows(0).Item("NewTrxID"), "0000")
                strAPNoteID = strAPNoteID & "/" & strCompany & "/" & strLocation & "/" & IIf(Len(Trim(strAccMonth)) = 1, "0" & strAccMonth, strAccMonth) & "/" & strAccYear
            Else
                strAPNoteID = "0001" & "/" & strCompany & "/" & strLocation & "/" & IIf(Len(Trim(strAccMonth)) = 1, "0" & strAccMonth, strAccMonth) & "/" & strAccYear
            End If

            strOpCd_APNote = "AP_CLSTRX_INVOICERECEIVENOTE_ADD"
        Else
            If blnIsSaved = False Then
                strOpCd_APNote = "AP_CLSTRX_INVOICERECEIVENOTE_UPD"
            Else
                strOpCd_APNote = "AP_CLSTRX_INVOICERECEIVENOTE_SAVE"
            End If
            strAPNoteID = strInvoiceRcvId
        End If

        strParamName = "INVOICERCVID|INVOICERCVREFNO|INVOICERCVREFDATE|FAKTURPAJAKNO|FAKTURPAJAKDATE|" & _
                        "INVOICETYPE|SUPPLIERCODE|INVOICEDUEDATE|TERMTYPE|TOTALAMOUNT|REMARK|LOCCODE|" & _
                        "ACCMONTH|ACCYEAR|STATUS|CREATEDATE|UPDATEDATE|PRINTDATE|UPDATEID|RECEIVEFROM|VIABANKTRANSFER"
        strParamValue = strAPNoteID & "|" & Replace(strInvRcvRefNo, "'", "''") & "|" & strDate & "|" & strFakturPajakNo & "|" & strFakturPajakDate & "|" & _
                        strInvType & "|" & strSupplierCode & "|" & strDueDate & "|" & strTermType & "|" & dblTotalAmount & "|" & strRemark & "|" & strLocation & "|" & _
                        strAccMonth & "|" & strAccYear & "|" & intStatus & "|" & strDate & "|" & strDate & "||" & strUserId & "|" & strReceiveFrom & "|" & IIf(rbViaBTYes.Checked = True, "1", "0")

        Try
            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd_APNote, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=UPD_ITEM&errmesg=" & Exp.ToString() & "&redirect=pu/trx/AP_trx_InvoiceRcvList")
        End Try

        strInvoiceRcvId = IIf(strInvoiceRcvId = "", strAPNoteID, strInvoiceRcvId)

        inrid.Value = strInvoiceRcvId
        lblInvoiceRcvID.Text = strInvoiceRcvId
        BindTermType(ddlTermType.SelectedItem.Value)
        onLoad_Button()
        blnIsUpdated = True
        strSelectedInvRcvId = IIf(strSelectedInvRcvId = "", strInvoiceRcvId, strSelectedInvRcvId)
    End Sub

    Sub SaveBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        If strSelectedInvRcvId = "" Or lblInvoiceRcvID.Text.Trim = "" Then
            Exit Sub
        End If

        blnIsSaved = True
        Update_InvoiceRcv(objAPTrx.EnumInvoiceRcvNoteStatus.Active)
        If blnIsUpdated Then
            RefreshBtn_Click(Sender, E)
        End If
    End Sub

    Sub RefreshBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        onLoad_Display(strSelectedInvRcvId)
        onLoad_DisplayItem(strSelectedInvRcvId)
        onLoad_Button()
        txtSplInvAmt.Text = "0"
        txtSplTaxAmt.Text = "0"
        txtCreditTerm.Text = ""
        txtPODueDate.Text = ""
        txtDueDate.Text = ""
        hidAdjAmount.Value = 0
        lblAdjAmount.Text = 0
    End Sub

    Sub GenInvBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCd_APGenInv_Add As String = "AP_CLSTRX_INVOICERECEIVENOTE_GENERATE_INVOICE"
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer
        Dim strDate As String = Date_Validation(txtTransDate.Text, False)
        Dim indDate As String = ""

        If CheckDate(txtTransDate.Text.Trim(), indDate) = False Then
            lblErrTransDate.Visible = True
            lblFmtTransDate.Visible = True
            lblErrTransDate.Text = "<br>Date Entered should be in the format"
            Exit Sub
        End If

        Dim intInputPeriod As Integer = Year(strDate) * 100 + Month(strDate)
        Dim intCurPeriod As Integer = (CInt(strAccYear) * 100) + CInt(strAccMonth)
        Dim intSelPeriod As Integer = (CInt(strSelAccYear) * 100) + CInt(strSelAccMonth)

        If Session("SS_FILTERPERIOD") = "0" Then
            If intCurPeriod < intInputPeriod Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "Invalid transaction date."
                Exit Sub
            End If
        Else
            If intSelPeriod <> intInputPeriod Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "Invalid transaction date."
                Exit Sub
            End If
            If intSelPeriod < intCurPeriod And intLevel < 2 Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "This period already locked."
                Exit Sub
            End If
        End If

        'If rbAdvPay.Checked = True Or rbOTE.checked = True Then
        'Else
        strAccYear = Year(strDate)
        strAccMonth = Month(strDate)
        strParamName = "INVOICERCVID|LOCCODE|ACCMONTH|USERID|PPNRATE"
        strParamValue = strSelectedInvRcvId & "|" & strLocation & "|" & strAccMonth & "|" & strUserId & "|" & Session("SS_PPNRATE")
        Try
            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd_APGenInv_Add, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=UPD_ITEM&errmesg=" & Exp.ToString() & "&redirect=pu/trx/AP_trx_InvoiceRcvNoteList")
        End Try
        'End If

        Update_InvoiceRcv(objAPTrx.EnumInvoiceRcvNoteStatus.Invoiced)

        If blnIsUpdated Then
            RefreshBtn_Click(Sender, E)
        End If
    End Sub

    Sub GetSupplierBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        BindCreditTerm(txtSupCode.Text)
        BindPO(txtSupCode.Text)
    End Sub

    Sub ConfirmBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strDate As String = Date_Validation(txtTransDate.Text, False)
        Dim indDate As String = ""

        If CheckDate(txtTransDate.Text.Trim(), indDate) = False Then
            lblErrTransDate.Visible = True
            lblFmtTransDate.Visible = True
            lblErrTransDate.Text = "<br>Date Entered should be in the format"
            Exit Sub
        End If

        Dim intInputPeriod As Integer = Year(strDate) * 100 + Month(strDate)
        Dim intCurPeriod As Integer = (CInt(strAccYear) * 100) + CInt(strAccMonth)
        Dim intSelPeriod As Integer = (CInt(strSelAccYear) * 100) + CInt(strSelAccMonth)

        If Session("SS_FILTERPERIOD") = "0" Then
            If intCurPeriod < intInputPeriod Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "Invalid transaction date."
                Exit Sub
            End If
        Else
            If intSelPeriod <> intInputPeriod Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "Invalid transaction date."
                Exit Sub
            End If
            If intSelPeriod < intCurPeriod And intLevel < 2 Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "This period already locked."
                Exit Sub
            End If
        End If

        Update_InvoiceRcv(objAPTrx.EnumInvoiceRcvNoteStatus.Invoiced)

        If blnIsUpdated Then
            RefreshBtn_Click(Sender, E)
        End If
    End Sub

    Sub CancelBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCd_UpdateStatus As String = "AP_CLSTRX_INVOICERECEIVENOTE_UPD_STATUS"
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer
        Dim strRemark As String = Trim(Replace(txtRemark.Text, "'", "''"))
        Dim strDate As String = Date_Validation(txtTransDate.Text, False)
        Dim indDate As String = ""

        If CheckDate(txtTransDate.Text.Trim(), indDate) = False Then
            lblErrTransDate.Visible = True
            lblFmtTransDate.Visible = True
            lblErrTransDate.Text = "<br>Date Entered should be in the format"
            Exit Sub
        End If

        Dim intInputPeriod As Integer = Year(strDate) * 100 + Month(strDate)
        Dim intCurPeriod As Integer = (CInt(strAccYear) * 100) + CInt(strAccMonth)
        Dim intSelPeriod As Integer = (CInt(strSelAccYear) * 100) + CInt(strSelAccMonth)

        If Session("SS_FILTERPERIOD") = "0" Then
            If intCurPeriod < intInputPeriod Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "Invalid transaction date."
                Exit Sub
            End If
        Else
            If intSelPeriod <> intInputPeriod Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "Invalid transaction date."
                Exit Sub
            End If
            If intSelPeriod < intCurPeriod And intLevel < 2 Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "This period already locked."
                Exit Sub
            End If
        End If

        strParamName = "INVOICERCVID|STATUS|REMARK|UPDATEID"
        strParamValue = strSelectedInvRcvId & "|" & objAPTrx.EnumInvoiceRcvNoteStatus.Cancelled & "|" & strRemark & "|" & strUserId

        Try
            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd_UpdateStatus, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=UPD_ITEM&errmesg=" & Exp.ToString() & "&redirect=pu/trx/AP_trx_InvoiceRcvNoteList")
        End Try

        RefreshBtn_Click(Sender, E)
    End Sub

    Sub DeleteBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCd_UpdateStatus As String = "AP_CLSTRX_INVOICERECEIVENOTE_UPD_STATUS"
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer
        Dim strRemark As String = Trim(Replace(txtRemark.Text, "'", "''"))
        Dim strDate As String = Date_Validation(txtTransDate.Text, False)
        Dim indDate As String = ""

        If CheckDate(txtTransDate.Text.Trim(), indDate) = False Then
            lblErrTransDate.Visible = True
            lblFmtTransDate.Visible = True
            lblErrTransDate.Text = "<br>Date Entered should be in the format"
            Exit Sub
        End If

        Dim intInputPeriod As Integer = Year(strDate) * 100 + Month(strDate)
        Dim intCurPeriod As Integer = (CInt(strAccYear) * 100) + CInt(strAccMonth)
        Dim intSelPeriod As Integer = (CInt(strSelAccYear) * 100) + CInt(strSelAccMonth)

        If Session("SS_FILTERPERIOD") = "0" Then
            If intCurPeriod < intInputPeriod Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "Invalid transaction date."
                Exit Sub
            End If
        Else
            If intSelPeriod <> intInputPeriod Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "Invalid transaction date."
                Exit Sub
            End If
            If intSelPeriod < intCurPeriod And intLevel < 2 Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "This period already locked."
                Exit Sub
            End If
        End If

        strParamName = "INVOICERCVID|STATUS|REMARK|UPDATEID"
        strParamValue = strSelectedInvRcvId & "|" & objAPTrx.EnumInvoiceRcvNoteStatus.Deleted & "|" & strRemark & "|" & strUserId

        Try
            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd_UpdateStatus, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=UPD_ITEM&errmesg=" & Exp.ToString() & "&redirect=pu/trx/AP_trx_InvoiceRcvNoteList")
        End Try

        RefreshBtn_Click(Sender, E)
    End Sub

    Sub UnDeleteBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim intErr As Integer
        If intErr = -5 Then
            lblErrUnDel.Visible = True
        Else
            Dim strOpCd_UpdateStatus As String = "AP_CLSTRX_INVOICERECEIVENOTE_UPD_STATUS"
            Dim strParamName As String
            Dim strParamValue As String
            Dim intErrNo As Integer
            Dim strRemark As String = Trim(Replace(txtRemark.Text, "'", "''"))
            Dim strDate As String = Date_Validation(txtTransDate.Text, False)
            Dim indDate As String = ""

            If CheckDate(txtTransDate.Text.Trim(), indDate) = False Then
                lblErrTransDate.Visible = True
                lblFmtTransDate.Visible = True
                lblErrTransDate.Text = "<br>Date Entered should be in the format"
                Exit Sub
            End If

            Dim intInputPeriod As Integer = Year(strDate) * 10 + Month(strDate)
            Dim intCurPeriod As Integer = (CInt(strAccYear) * 10) + CInt(strAccMonth)
            Dim intSelPeriod As Integer = (CInt(strSelAccYear) * 10) + CInt(strSelAccMonth)

            If Session("SS_FILTERPERIOD") = "0" Then
                If intCurPeriod < intInputPeriod Then
                    lblErrTransDate.Visible = True
                    lblErrTransDate.Text = "Invalid transaction date."
                    Exit Sub
                End If
            Else
                If intSelPeriod <> intInputPeriod Then
                    lblErrTransDate.Visible = True
                    lblErrTransDate.Text = "Invalid transaction date."
                    Exit Sub
                End If
                If intSelPeriod < intCurPeriod And intLevel < 2 Then
                    lblErrTransDate.Visible = True
                    lblErrTransDate.Text = "This period already locked."
                    Exit Sub
                End If
            End If

            strParamName = "INVOICERCVID|STATUS|REMARK|UPDATEID"
            strParamValue = strSelectedInvRcvId & "|" & objAPTrx.EnumInvoiceRcvNoteStatus.Active & "||" & strUserId

            Try
                intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd_UpdateStatus, _
                                                        strParamName, _
                                                        strParamValue)

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=UPD_ITEM&errmesg=" & Exp.ToString() & "&redirect=pu/trx/AP_trx_InvoiceRcvNoteList")
            End Try
        End If

        RefreshBtn_Click(Sender, E)
    End Sub

    Sub AddBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCd_APNoteLn_Add As String = "AP_CLSTRX_INVOICERECEIVENOTELN_ADD"
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer
        Dim strCreditTerm As String = txtCreditTerm.Text
        Dim strPOID As String
        Dim strCurrencyCode As String
        Dim strExchangeRate As String
        Dim dblAmount As Double
        Dim dblAmountCurrency As Double
        Dim dblSplInvAmount As Double = IIf(Trim(Request.Form("txtSplInvAmt")) = "", 0, Trim(Request.Form("txtSplInvAmt")))
        Dim dblSplTaxAmount As Double = IIf(Trim(Request.Form("txtSplTaxAmt")) = "", 0, Trim(Request.Form("txtSplTaxAmt")))
        Dim arrParam As Array
        Dim strDueDate As String
        Dim strPODueDate As String
        Dim strGRDate As String
        Dim objLastGR As Object
        Dim strOpCd_LastGR As String = "AP_CLSTRX_INVOICERECEIVE_LASTGR_GET"
        Dim intCreditTerm As Integer
        Dim strAddNote As String = Trim(Replace(txtAddNote.Text, "'", "''"))
        Dim strFakturPajakNo As String = txtFakturPjkNo.Text
        Dim strFakturPajakDate As String = Date_Validation(txtFakturPjkDate.Text, False)
        Dim strDate As String = Date_Validation(txtTransDate.Text, False)
        Dim indDate As String = ""

        If CheckDate(txtTransDate.Text.Trim(), indDate) = False Then
            lblErrTransDate.Visible = True
            lblFmtTransDate.Visible = True
            lblErrTransDate.Text = "<br>Date Entered should be in the format"
            Exit Sub
        End If

        Dim intInputPeriod As Integer = Year(strDate) * 100 + Month(strDate)
        Dim intCurPeriod As Integer = (CInt(strAccYear) * 100) + CInt(strAccMonth)
        Dim intSelPeriod As Integer = (CInt(strSelAccYear) * 100) + CInt(strSelAccMonth)

        If Session("SS_FILTERPERIOD") = "0" Then
            If intCurPeriod < intInputPeriod Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "Invalid transaction date."
                Exit Sub
            End If
        Else
            If intSelPeriod <> intInputPeriod Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "Invalid transaction date."
                Exit Sub
            End If
            If intSelPeriod < intCurPeriod And intLevel < 2 Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "This period already locked."
                Exit Sub
            End If
        End If

        If rbSPO.Checked = True Or rbOTE.Checked = True Or rbFFB.Checked = True Then
            arrParam = hidPOItem.Value.Split("|")
        Else
            arrParam = Split("|||0|0", "|")
        End If

        blnIsSaved = False
        If rbSPO.Checked = True Then
            If ddlPO.SelectedIndex = 0 Then
                lblErrPO.Visible = True
                Exit Sub
            End If
            strPOID = ddlPO.SelectedItem.Value
        ElseIf rbAdvPay.Checked = True Then
            If ddlPO.SelectedIndex = 0 Then
                lblErrPO.Visible = True
                Exit Sub
            End If
            strPOID = ddlPO.SelectedItem.Value
        ElseIf rbOTE.Checked = True Then
            strPOID = ""
        ElseIf rbFFB.Checked = True Then
            strPOID = ""
        ElseIf rbTransportFee.Checked = True Then
            If ddlPO.SelectedIndex = 0 Then
                lblErrPO.Visible = True
                Exit Sub
            End If
            strPOID = ddlPO.SelectedItem.Value
        End If

        If txtSplInvAmt.Text = "" Or txtSplInvAmt.Text = "0" Then
            lblErrSplInvAmt.Visible = True
            Exit Sub
        End If

        If rbSPO.Checked = True Then
            'dblSplInvAmount = CDbl(txtSplInvAmt.Text) - CDbl(hidAdvAmount.Value) + CDbl(hidAdjAmount.Value)
            dblSplInvAmount = CDbl(txtSplInvAmt.Text)

            'If CDbl(hidAdvAmount.Value) + CDbl(txtSplInvAmt.Text) > hidPOAmount.Value Then
            '    lblErrSplInvAmt.Text = "Invalid Amount. Advance amount + supplier invoice amount are greater than PO Amount."
            '    lblErrSplInvAmt.Visible = True
            '    Exit Sub
            'Else
            '    lblErrSplInvAmt.Visible = False
            'End If
        End If

        If txtPPNInit.Text = "1" Then
            If txtFakturPjkNo.Text = "" Then
                lblErrFakturPjk.Visible = True
                lblErrFakturPjk.Text = "Tax Number cannot be empty, selected supplier has VAT/PPN"
                Exit Sub
            End If
            If CDbl(txtSplTaxAmt.Text) = 0 Then
                lblErrFakturPjk.Visible = True
                lblErrFakturPjk.Text = "Tax Amount cannot be empty, selected supplier has VAT/PPN"
                Exit Sub
            End If
        End If

        If Trim(strFakturPajakNo) <> "" Then
            If strFakturPajakDate = "" Then
                lblerrFakturPjkDate.Text = "Please input faktur pajak date"
                lblerrFakturPjkDate.Visible = True
                Exit Sub
            End If
        Else
            txtFakturPjkDate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
            strFakturPajakDate = Date_Validation(txtFakturPjkDate.Text, False)
        End If

        If txtCreditTerm.Text = "" Or txtCreditTerm.Text = "0" Then
            lblErrCreditTerm.Visible = True
            Exit Sub
        End If

        Update_InvoiceRcv(objAPTrx.EnumInvoiceRcvNoteStatus.Active)
        If lblInvoiceRcvID.Text.Trim = "" Then
            Exit Sub
        End If
        strSelectedInvRcvId = lblInvoiceRcvID.Text.Trim

        intCreditTerm = CInt(txtCreditTerm.Text)
        If strPOID = "" Then
            strDueDate = DateAdd(DateInterval.Day, intCreditTerm, CDate(strDate))
            Select Case DatePart(DateInterval.Weekday, CDate(strDueDate))
                Case vbMonday
                    strDueDate = strDueDate
                Case vbTuesday
                    strDueDate = DateAdd(DateInterval.Day, 2, CDate(strDueDate))
                Case vbWednesday
                    strDueDate = DateAdd(DateInterval.Day, 1, CDate(strDueDate))
                Case vbThursday
                    strDueDate = strDueDate
                Case vbFriday
                    strDueDate = DateAdd(DateInterval.Day, 3, CDate(strDueDate))
                Case vbSaturday
                    strDueDate = DateAdd(DateInterval.Day, 2, CDate(strDueDate))
                Case vbSunday
                    strDueDate = DateAdd(DateInterval.Day, 1, CDate(strDueDate))
            End Select
        Else
            strPOID = arrParam(0)
            strCurrencyCode = arrParam(1)
            strExchangeRate = arrParam(2)
            dblAmount = arrParam(3)
            dblAmountCurrency = arrParam(4)
            If rbAdvPay.Checked = True Then
                strPOID = ddlPO.SelectedItem.Value
            End If

            If rbSPO.Checked = True Or rbAdvPay.Checked = True Or rbTransportFee.Checked = True Then
                If hidCurrencyCode.Value <> "" Then
                    If hidCurrencyCode.Value <> strCurrencyCode Then
                        lblErrCurrency.Visible = True
                        Exit Sub
                    End If
                End If
            End If

            strParamName = "LOCCODE|POID"
            strParamValue = strLocation & "|" & ddlPO.SelectedItem.Value

            Try
                intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_LastGR, _
                                                    strParamName, _
                                                    strParamValue, _
                                                    objLastGR)

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_CLSTRX_INVOICERECEIVE_LASTGR_GET&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvlist.aspx")
            End Try

            If objLastGR.Tables(0).Rows.Count > 0 Then
                strGRDate = Date_Validation(objLastGR.Tables(0).Rows(0).Item("GoodsRcvRefDate"), True)
                strGRDate = Date_Validation(strGRDate, False)
            End If

            If ddlTermType.SelectedItem.Value = "11" Then
                strDueDate = DateAdd(DateInterval.Day, intCreditTerm, CDate(strDate))
            Else
                strDueDate = DateAdd(DateInterval.Day, intCreditTerm, CDate(strGRDate))
            End If

            strPODueDate = strDueDate
            Select Case True
                Case CDate(strDueDate) <= CDate(strDate)
                    strDueDate = DateAdd(DateInterval.Day, 7, CDate(strDate))
                Case DateDiff(DateInterval.Day, CDate(strDate), CDate(strDueDate)) <= 7
                    strDueDate = DateAdd(DateInterval.Day, 7, CDate(strDueDate))
            End Select

            Select Case DatePart(DateInterval.Weekday, CDate(strDueDate))
                Case vbMonday
                    strDueDate = strDueDate
                Case vbTuesday
                    strDueDate = DateAdd(DateInterval.Day, 2, CDate(strDueDate))
                Case vbWednesday
                    strDueDate = DateAdd(DateInterval.Day, 1, CDate(strDueDate))
                Case vbThursday
                    strDueDate = strDueDate
                Case vbFriday
                    strDueDate = DateAdd(DateInterval.Day, 3, CDate(strDueDate))
                Case vbSaturday
                    strDueDate = DateAdd(DateInterval.Day, 2, CDate(strDueDate))
                Case vbSunday
                    strDueDate = DateAdd(DateInterval.Day, 1, CDate(strDueDate))
            End Select
        End If

        strParamName = "INVOICERCVID|POID|CREDITTERM|PODUEDATE|DUEDATE|" & _
                        "AMOUNT|AMOUNTCURRENCY|CURRENCYCODE|EXCHANGERATE|SUPPLIERINVAMOUNT|ADDNOTE|FAKTURPAJAKNO|FAKTURPAJAKDATE|FAKTURAMOUNT"
        strParamValue = strSelectedInvRcvId & "|" & strPOID & "|" & strCreditTerm & "|" & strPODueDate & "|" & strDueDate & "|" & _
                        dblAmount & "|" & dblAmountCurrency & "|" & IIf(strCurrencyCode = "", "IDR", strCurrencyCode) & "|" & IIf(strExchangeRate = "", 1, strExchangeRate) & _
                        "|" & dblSplInvAmount & "|" & strAddNote & "|" & strFakturPajakNo & "|" & strFakturPajakDate & "|" & dblSplTaxAmount

        Try
            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd_APNoteLn_Add, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=UPD_ITEM&errmesg=" & Exp.ToString() & "&redirect=pu/trx/AP_trx_InvoiceRcvNoteList")
        End Try

        onLoad_Display(lblInvoiceRcvID.Text.Trim)
        onLoad_DisplayItem(lblInvoiceRcvID.Text.Trim)
        Update_InvoiceRcv(objAPTrx.EnumInvoiceRcvNoteStatus.Active)
        RefreshBtn_Click(Sender, E)
    End Sub

    Sub BackBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("ap_trx_InvRcvNoteList.aspx")
    End Sub

    Sub InvoiceType_OnCheckChange(ByVal Sender As Object, ByVal E As EventArgs)
        If txtSupCode.Text = "" Then
            Exit Sub
        End If

        If rbSPO.Checked = True Then
            ddlPO.Enabled = True
            txtSupCode.Enabled = True
            lblErrSuppCode.Visible = False
            ddlTermType.SelectedIndex = 0
            BindPO(txtSupCode.Text)
        ElseIf rbAdvPay.Checked = True Then
            ddlPO.Enabled = True
            ddlPO.SelectedIndex = 0
            txtSupCode.Enabled = True
            ddlTermType.SelectedIndex = 1
            BindPO(txtSupCode.Text)
        ElseIf rbOTE.Checked = True Then
            ddlPO.Enabled = False
            ddlPO.SelectedIndex = 0
            txtSupCode.Enabled = True
            ddlTermType.SelectedIndex = 1
        ElseIf rbFFB.Checked = True Then
            ddlPO.Enabled = False
            ddlPO.SelectedIndex = 0
            txtSupCode.Enabled = True
            ddlTermType.SelectedIndex = 1
        ElseIf rbTransportFee.Checked = True Then
            ddlPO.Enabled = True
            txtSupCode.Enabled = True
            lblErrSuppCode.Visible = False
            ddlTermType.SelectedIndex = 1
            BindPO(txtSupCode.Text)
        End If
    End Sub

    Sub PrintBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim strTRXID As String

        strTRXID = Trim(lblInvoiceRcvID.Text)
        If strTRXID = "" Then
            Exit Sub
        End If

        Response.Write("<Script Language=""JavaScript"">window.open(""../reports/AP_Rpt_InvRcvDet.aspx?Type=Print&CompName=" & strCompany & _
                        "&Location=" & strLocation & _
                        "&TrxID=" & strTRXID & _
                        "&TrxType=" & "1" & _
                        "&SupplierCode=" & "" & _
                        """,null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")

    End Sub

    Sub onLoad_DisplayAPNote(ByVal pv_strInvoiceRcvID As String)
        Dim objAPNote As Object
        Dim strOpCd_APNote As String
        Dim strAPNoteID As String
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer

        strOpCd_APNote = "AP_CLSTRX_INVOICERCV_NOTE_GET"
        strParamName = "STRSEARCH"
        strParamValue = "TrxID = '" & pv_strInvoiceRcvID & "' "

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCd_APNote, _
                                                strParamName, _
                                                strParamValue, _
                                                objAPNote)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_CLSTRX_INVOICERCV_NOTE_GET_LASTNO&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvnotelist.aspx")
        End Try

        dgAPNote.DataSource = Nothing
        dgAPNote.DataSource = objAPNote.Tables(0)
        dgAPNote.DataBind()
    End Sub

    Sub NewBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Response.Redirect("AP_trx_InvRcvNoteDet.aspx")
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs) Handles dgLineDet.DeleteCommand
        Dim strOpCode_DelLine As String = "AP_CLSTRX_INVOICERECEIVENOTELN_DEL"
        Dim lbl As Label
        Dim strIRLNId As String
        Dim strPOId As String
        Dim intErrNo As Integer
        Dim strParamName As String
        Dim strParamValue As String

        dgLineDet.EditItemIndex = CInt(E.Item.ItemIndex)
        lbl = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lblInvoiceRcvLnID")
        strIRLNId = lbl.Text

        strParamName = "INVOICERCVID|INVOICERCVLNID"
        strParamValue = strSelectedInvRcvId & "|" & strIRLNId

        Try
            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCode_DelLine, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INVOICERECEIVE_DEL_LINE&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcvnotelist.aspx")
        End Try

        blnIsSaved = True
        onLoad_Display(lblInvoiceRcvID.Text.Trim)
        onLoad_DisplayItem(lblInvoiceRcvID.Text.Trim)
        Update_InvoiceRcv(objAPTrx.EnumInvoiceRcvNoteStatus.Active)
        onLoad_Display(lblInvoiceRcvID.Text.Trim)
        onLoad_DisplayItem(lblInvoiceRcvID.Text.Trim)
        onLoad_Button()
    End Sub

    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strInvoiceRcvID As String = lblInvoiceRcvID.Text.Trim
        Dim lbl As Label
        Dim strIRLNId As String
        Dim cButton As LinkButton
        Dim FakturPajakNoText As Label
        Dim FakturPajakNo As TextBox
        Dim FakturPajakDateText As Label
        Dim FakturPajakDate As TextBox
        Dim DueDateText As Label
        Dim DueDate As TextBox
        Dim FakturAmountText As Label
        Dim FakturAmount As TextBox
        Dim strDate As String = Date_Validation(txtTransDate.Text, False)
        Dim indDate As String = ""

        If CheckDate(txtTransDate.Text.Trim(), indDate) = False Then
            lblErrTransDate.Visible = True
            lblFmtTransDate.Visible = True
            lblErrTransDate.Text = "<br>Date Entered should be in the format"
            Exit Sub
        End If

        Dim intInputPeriod As Integer = Year(strDate) * 100 + Month(strDate)
        Dim intCurPeriod As Integer = (CInt(strAccYear) * 100) + CInt(strAccMonth)
        Dim intSelPeriod As Integer = (CInt(strSelAccYear) * 100) + CInt(strSelAccMonth)

        If Session("SS_FILTERPERIOD") = "0" Then
            If intCurPeriod < intInputPeriod Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "Invalid transaction date."
                Exit Sub
            End If
        Else
            If intSelPeriod <> intInputPeriod Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "Invalid transaction date."
                Exit Sub
            End If
            If intSelPeriod < intCurPeriod And intLevel < 2 Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "This period already locked."
                Exit Sub
            End If
        End If

        dgLineDet.EditItemIndex = CInt(E.Item.ItemIndex)
        lbl = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lblInvoiceRcvLnID")
        strIRLNId = lbl.Text

        FakturPajakNoText = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lblFakturPajakNo")
        FakturPajakNoText.Visible = False
        FakturPajakNo = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lstFakturPajakNo")
        FakturPajakNo.Text = FakturPajakNoText.Text
        FakturPajakNo.Visible = True


        FakturAmountText = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lblIDFakturAmount")
        FakturAmountText.Visible = False
        FakturAmount = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lblFakturAmount")
        'FakturAmount.Text = FakturAmountText.Text
        FakturAmount.Visible = True


        FakturPajakDateText = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lblFakturPajakDate")
        FakturPajakDateText.Visible = False
        FakturPajakDate = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lstFakturPajakDate")
        FakturPajakDate.Text = IIf(FakturPajakDateText.Text <> "", Date_Validation(FakturPajakDateText.Text, True), objGlobal.GetShortDate(Session("SS_DATEFMT"), Now))
        FakturPajakDate.Visible = True

        DueDateText = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lblDueDate")
        DueDateText.Visible = False
        DueDate = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lstDueDate")
        DueDate.Text = Date_Validation(DueDateText.Text, True)
        DueDate.Visible = True

        cButton = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lbEdit")
        cButton.Visible = False
        cButton = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lbDelete")
        cButton.Visible = False
        cButton = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lbUpdate")
        cButton.Visible = True
        cButton = dgLineDet.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lbCancel")
        cButton.Visible = True
    End Sub

    Sub DEDR_Update(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strOpCd As String = "AP_CLSTRX_INVOICERECEIVENOTELN_UPD"
        Dim strInvoiceRcvID As String = lblInvoiceRcvID.Text.Trim
        Dim lbl As Label
        Dim strIRLNId As String
        Dim intErrNo As Integer
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim strFakturPajakNo As String
        Dim FakturPajakNoText As TextBox
        Dim strFakturPajakDate As String
        Dim FakturPajakDateText As TextBox
        Dim strDueDate As String
        Dim DueDateText As TextBox
        Dim strFakturAmount As Double
        Dim FakturAmountText As TextBox
        Dim strDate As String = Date_Validation(txtTransDate.Text, False)
        Dim indDate As String = ""

        If CheckDate(txtTransDate.Text.Trim(), indDate) = False Then
            lblErrTransDate.Visible = True
            lblFmtTransDate.Visible = True
            lblErrTransDate.Text = "<br>Date Entered should be in the format"
            Exit Sub
        End If

        Dim intInputPeriod As Integer = Year(strDate) * 100 + Month(strDate)
        Dim intCurPeriod As Integer = (CInt(strAccYear) * 100) + CInt(strAccMonth)
        Dim intSelPeriod As Integer = (CInt(strSelAccYear) * 100) + CInt(strSelAccMonth)

        If Session("SS_FILTERPERIOD") = "0" Then
            If intCurPeriod < intInputPeriod Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "Invalid transaction date."
                Exit Sub
            End If
        Else
            If intSelPeriod <> intInputPeriod Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "Invalid transaction date."
                Exit Sub
            End If
            If intSelPeriod < intCurPeriod And intLevel < 2 Then
                lblErrTransDate.Visible = True
                lblErrTransDate.Text = "This period already locked."
                Exit Sub
            End If
        End If

        lbl = E.Item.FindControl("lblInvoiceRcvLnID")
        strIRLNId = lbl.Text
        FakturPajakNoText = E.Item.FindControl("lstFakturPajakNo")
        strFakturPajakNo = FakturPajakNoText.Text
        FakturPajakDateText = E.Item.FindControl("lstFakturPajakDate")
        strFakturPajakDate = Date_Validation(FakturPajakDateText.Text, False)
        DueDateText = E.Item.FindControl("lstDueDate")
        strDueDate = Date_Validation(DueDateText.Text, False)

        FakturAmountText = E.Item.FindControl("lblFakturAmount")
        strFakturAmount = IIf(FakturAmountText.Text = "", 0, FakturAmountText.Text)


        strParamName = "INVOICERCVID|INVOICERCVLNID|FAKTURPAJAKNO|FAKTURPAJAKDATE|DUEDATE|UPDATEID|FAKTURAMOUNT"

        strParamValue = Trim(strInvoiceRcvID) & "|" & Trim(strIRLNId) & "|" & Trim(strFakturPajakNo) & "|" & _
                        strFakturPajakDate & "|" & strDueDate & "|" & Trim(strUserId) & "|" & strFakturAmount

        Try
            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd, _
                                                    strParamName, _
                                                    strParamValue)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADD_DETAIL&errmesg=" & lblErrMessage.Text & "&redirect=IN/trx/IN_PurReq.aspx")
        End Try

        onLoad_Display(lblInvoiceRcvID.Text.Trim)
        onLoad_DisplayItem(lblInvoiceRcvID.Text.Trim)
        onLoad_Button()
    End Sub

    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        onLoad_DisplayItem(lblInvoiceRcvID.Text.Trim)
        onLoad_Button()
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
End Class
