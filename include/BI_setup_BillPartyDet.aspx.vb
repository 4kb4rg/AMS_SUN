

Imports System
Imports System.Data
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic.Interaction
Imports Microsoft.VisualBasic

Imports agri.PWSystem.clsLangCap
Imports agri.GlobalHdl.clsGlobalHdl
Imports agri.Admin.clsCountry
Imports agri.GL.clsSetup
Imports agri.Admin.clsLoc 


Public Class BI_Setup_BillPartyDet : Inherits Page

    Protected WithEvents txtBillPartyCode As TextBox
    Protected WithEvents txtName As TextBox
    Protected WithEvents txtContactPerson As TextBox
    Protected WithEvents txtAddress As HtmlTextArea
    Protected WithEvents txtTown As TextBox
    Protected WithEvents txtState As TextBox
    Protected WithEvents txtPostCode As TextBox
    Protected WithEvents ddlCountry As DropDownList
    Protected WithEvents txtTelNo As TextBox
    Protected WithEvents txtFaxNo As TextBox
    Protected WithEvents txtEmail As TextBox
    Protected WithEvents txtCreditTerm As TextBox
    Protected WithEvents txtCreditLimit As TextBox
    Protected WithEvents ddlTermType As DropDownList
    Protected WithEvents ddlAccCode As DropDownList
    Protected WithEvents bpcode As HtmlInputHidden
    Protected WithEvents btnFind1 As HtmlInputButton
    Protected WithEvents lblHiddenStatus As Label
    Protected WithEvents lblStatus As Label
    Protected WithEvents lblDateCreated As Label
    Protected WithEvents lblLastUpdate As Label
    Protected WithEvents lblUpdateBy As Label

    Protected WithEvents lblErrAccCode As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblErrDup As Label
    Protected WithEvents lblErrAddress As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents lblErrEnter As Label
    Protected WithEvents lblErrSelect As Label
    Protected WithEvents lblSelect As Label
    Protected WithEvents lblTitle As Label
    Protected WithEvents lblBillParty As Label
    Protected WithEvents lblBillPartyName As Label
    Protected WithEvents lblAccount As Label
    Protected WithEvents rfvName As RequiredFieldValidator
    Protected WithEvents taAdditionalNote As HtmlTextArea
    Protected WithEvents lblErrCountry As Label

    Protected WithEvents txtPIC As TextBox
    Protected WithEvents txtPosition As TextBox
    Protected WithEvents taTermOfWeighing As HtmlTextArea
    Protected WithEvents taTermOfPayment As HtmlTextArea
    Protected WithEvents taProdQualityCPO As HtmlTextArea
    Protected WithEvents taProdClaimCPO As HtmlTextArea
    Protected WithEvents taProdQualityPK As HtmlTextArea
    Protected WithEvents taProdClaimPK As HtmlTextArea
    Protected WithEvents taLoadDest As HtmlTextArea
    Protected WithEvents chkPPN As CheckBox
    
    Protected WithEvents btnSave As ImageButton
    Protected WithEvents btnDelete As ImageButton
    Protected WithEvents btnUnDelete As ImageButton
    Protected WithEvents btnBack As ImageButton

    Protected WithEvents ddlAccCodeAdv As DropDownList

    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objSysCountry As New agri.Admin.clsCountry()
    Dim objGLSetup As New agri.GL.clsSetup()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim objGLTrx As New agri.GL.ClsTrx()

    Dim objBPDs As New Object()
    Dim objCountryDs As New Object()
    Dim objAccountDs As New Object()
    Dim objLangCapDs As New Object()

    Dim objHRSetup As New agri.HR.clsSetup()
    Dim objBankDs As New Object()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim intBIAR As Integer

    Protected WithEvents ddlInterLocCode As DropDownList
    Protected WithEvents lblErrInterLocCode As Label
    Protected WithEvents lblDupInterLocCode As Label

    Dim strSelectedCode As String = ""
    Dim strSortExpression As String = "BillPartyCode"

    Protected WithEvents lblBankCode As Label
    Protected WithEvents lblBankAccNo As Label
    Protected WithEvents ddlBankCode As DropDownList
    Protected WithEvents txtBankAccNo As TextBox
    Dim strLocType As String

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_ARACCMONTH")
        strAccYear = Session("SS_ARACCYEAR")
        intBIAR = Session("SS_BIAR")
        strLocType = Session("SS_LOCTYPE")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumBIAccessRights.BIBillParty), intBIAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            onload_GetLangCap()
            lblErrAccCode.Visible = False
            lblErrInterLocCode.Visible = False
            lblDupInterLocCode.Visible = False
            lblErrCountry.Visible = False

            strSelectedCode = Trim(IIf(Request.QueryString("bpcode") <> "", Request.QueryString("bpcode"), Request.Form("bpcode")))

            If Not IsPostBack Then
                If strSelectedCode <> "" Then
                    onLoad_Display()
                    BindButton()
                Else
                    onLoad_NewDisplay()
                    BindButton()
                    BindInterLocCode("")
                    BindBankCode("")
                End If
            End If

            lblBankCode.Text = "Bank Code : "
            lblBankAccNo.Text = "Bank Account No : "
        End If
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblTitle.Text = UCase(GetCaption(objLangCap.EnumLangCap.BillParty))
        lblBillParty.Text = GetCaption(objLangCap.EnumLangCap.BillParty)
        lblBillPartyName.Text = GetCaption(objLangCap.EnumLangCap.BillPartyName)
        lblAccount.Text = "Debtor " & GetCaption(objLangCap.EnumLangCap.Account)
        lblErrAccCode.Text = lblErrSelect.Text & lblAccount.Text
        rfvName.ErrorMessage = lblErrEnter.Text & lblBillPartyName.Text
        lblErrInterLocCode.Text = lblErrSelect.Text & GetCaption(objLangCap.EnumLangCap.Location) & lblCode.Text
        lblDupInterLocCode.Text = "Duplicate " & GetCaption(objLangCap.EnumLangCap.Location) & lblCode.Text & " selected."
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_SETUP_BILLPARTYDET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BI/setup/BI_setup_BillPartyDet.aspx")
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


    Sub BindButton()
        txtBillPartyCode.Enabled = False
        txtName.Enabled = False
        txtContactPerson.Enabled = False
        txtAddress.Disabled = True
        txtTown.Enabled = False
        txtState.Enabled = False
        txtPostCode.Enabled = False
        ddlCountry.Enabled = False
        txtTelNo.Enabled = False
        txtFaxNo.Enabled = False
        txtEmail.Enabled = False
        txtCreditTerm.Enabled = False
        txtCreditLimit.Enabled = False
        ddlTermType.Enabled = False
        ddlAccCode.Enabled = False
        ddlAccCodeAdv.Enabled = False
        btnSave.Visible = False
        btnDelete.Visible = False
        btnUnDelete.Visible = False
        btnFind1.Disabled = False
        taAdditionalNote.Disabled = True

        Select Case CInt(lblHiddenStatus.Text)
            Case objGLSetup.EnumBillPartyStatus.Active
                txtName.Enabled = True
                txtContactPerson.Enabled = True
                txtAddress.Disabled = False
                txtTown.Enabled = True
                txtState.Enabled = True
                txtPostCode.Enabled = True
                ddlCountry.Enabled = True
                txtTelNo.Enabled = True
                txtFaxNo.Enabled = True
                txtEmail.Enabled = True
                txtCreditTerm.Enabled = True
                txtCreditLimit.Enabled = True
                ddlAccCode.Enabled = True
                ddlAccCodeAdv.Enabled = True
                taAdditionalNote.Disabled = False
                btnSave.Visible = True
                btnDelete.Visible = True
                btnDelete.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
            Case objGLSetup.EnumBillPartyStatus.Deleted
                btnFind1.Disabled = True
                btnUnDelete.Visible = True
            Case Else
                txtBillPartyCode.Enabled = False
                txtName.Enabled = True
                txtContactPerson.Enabled = True
                txtAddress.Disabled = False
                txtTown.Enabled = True
                txtState.Enabled = True
                txtPostCode.Enabled = True
                txtCreditTerm.Enabled = True
                txtCreditLimit.Enabled = True
                ddlCountry.Enabled = True
                txtTelNo.Enabled = True
                txtFaxNo.Enabled = True
                txtEmail.Enabled = True
                ddlAccCode.Enabled = True
                ddlAccCodeAdv.Enabled = True
                taAdditionalNote.Disabled = False
                btnSave.Visible = True
        End Select
    End Sub

    Sub onLoad_Display()
        Dim strOpCd_Supplier As String = "GL_CLSSETUP_BILLPARTY_GET"
        Dim strOpCd_Country As String = "ADMIN_CLSCOUNTRY_COUNTRY_LIST_GET"
        Dim strOpCd_Account As String = "GL_CLSSETUP_ACCOUNTCODE_LIST_GET"
        Dim strParam As String = ""
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intCountryIndex As Integer = 0
        Dim intAccountIndex As Integer = 0
        Dim intAccountAdvIndex As Integer = 0

        txtBillPartyCode.Text = strSelectedCode
        bpcode.Value = strSelectedCode

        strParam = strSelectedCode & "||||" & strSortExpression & "||" & strSelectedCode
        Try
            intErrNo = objGLSetup.mtdGetBillParty(strOpCd_Supplier, strParam, objBPDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_SETUP_BILLPARTYDET_GET&errmesg=" & lblErrMessage.Text & "&redirect=BI/setup/BI_setup_BillPartyDet.aspx")
        End Try

        If objBPDs.Tables(0).Rows.Count > 0 Then
            objBPDs.Tables(0).Rows(0).Item("BillPartyCode") = Trim(objBPDs.Tables(0).Rows(0).Item("BillPartyCode"))
            objBPDs.Tables(0).Rows(0).Item("Name") = Trim(objBPDs.Tables(0).Rows(0).Item("Name"))
            objBPDs.Tables(0).Rows(0).Item("ContactPerson") = Trim(objBPDs.Tables(0).Rows(0).Item("ContactPerson"))
            objBPDs.Tables(0).Rows(0).Item("Address") = Trim(objBPDs.Tables(0).Rows(0).Item("Address"))
            objBPDs.Tables(0).Rows(0).Item("Town") = Trim(objBPDs.Tables(0).Rows(0).Item("Town"))
            objBPDs.Tables(0).Rows(0).Item("State") = Trim(objBPDs.Tables(0).Rows(0).Item("State"))
            objBPDs.Tables(0).Rows(0).Item("PostCode") = Trim(objBPDs.Tables(0).Rows(0).Item("PostCode"))
            objBPDs.Tables(0).Rows(0).Item("CountryCode") = Trim(objBPDs.Tables(0).Rows(0).Item("CountryCode"))
            objBPDs.Tables(0).Rows(0).Item("TelNo") = Trim(objBPDs.Tables(0).Rows(0).Item("TelNo"))
            objBPDs.Tables(0).Rows(0).Item("FaxNo") = Trim(objBPDs.Tables(0).Rows(0).Item("FaxNo"))
            objBPDs.Tables(0).Rows(0).Item("Email") = Trim(objBPDs.Tables(0).Rows(0).Item("Email"))
            objBPDs.Tables(0).Rows(0).Item("AddChrg") = Trim(objBPDs.Tables(0).Rows(0).Item("AddChrg"))
            objBPDs.Tables(0).Rows(0).Item("AccCode") = Trim(objBPDs.Tables(0).Rows(0).Item("AccCode"))
            objBPDs.Tables(0).Rows(0).Item("Status") = CInt(Trim(objBPDs.Tables(0).Rows(0).Item("Status")))
            objBPDs.Tables(0).Rows(0).Item("UserName") = Trim(objBPDs.Tables(0).Rows(0).Item("UserName"))

            objBPDs.Tables(0).Rows(0).Item("InterLocCode") = Trim(objBPDs.Tables(0).Rows(0).Item("InterLocCode"))
            BindInterLocCode(objBPDs.Tables(0).Rows(0).Item("InterLocCode"))
            BindBankCode(objBPDs.Tables(0).Rows(0).Item("BankCode"))
            taAdditionalNote.Value = Trim(objBPDs.Tables(0).Rows(0).Item("AdditionalNote"))

            txtPIC.Text = Trim(objBPDs.Tables(0).Rows(0).Item("PIC"))
            txtPosition.Text = Trim(objBPDs.Tables(0).Rows(0).Item("PICPos"))
            If Trim(objBPDs.Tables(0).Rows(0).Item("PPNInit")) = "0" Then
                chkPPN.Checked = False
                chkPPN.Text = "  "
            Else
                chkPPN.Checked = True
                chkPPN.Text = "  "
            End If

            taTermOfWeighing.Value = Trim(objBPDs.Tables(0).Rows(0).Item("TermOfWeighing"))
            taTermOfPayment.Value = Trim(objBPDs.Tables(0).Rows(0).Item("TermOfPayment"))
            taProdQualityCPO.Value = Trim(objBPDs.Tables(0).Rows(0).Item("QualityCPO"))
            taProdClaimCPO.Value = Trim(objBPDs.Tables(0).Rows(0).Item("ClaimCPO"))
            taProdQualityPK.Value = Trim(objBPDs.Tables(0).Rows(0).Item("QualityPK"))
            taProdClaimPK.Value = Trim(objBPDs.Tables(0).Rows(0).Item("ClaimPK"))
            taLoadDest.Value = Trim(objBPDs.Tables(0).Rows(0).Item("LoadDest"))
        End If

        Try
            intErrNo = objSysCountry.mtdGetCountryList(strOpCd_Country, objCountryDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_SETUP_BILLPARTY_COUNTRY_GET&errmesg=" & lblErrMessage.Text & "&redirect=BI/setup/BI_setup_BillPartyDet.aspx")
        End Try

        If objCountryDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objCountryDs.Tables(0).Rows.Count - 1
                objCountryDs.Tables(0).Rows(intCnt).Item(0) = Trim(objCountryDs.Tables(0).Rows(intCnt).Item(0))
                objCountryDs.Tables(0).Rows(intCnt).Item(1) = objCountryDs.Tables(0).Rows(intCnt).Item(0) & " (" & Trim(objCountryDs.Tables(0).Rows(intCnt).Item(1)) & ")"
                If objCountryDs.Tables(0).Rows(intCnt).Item("CountryCode") = objBPDs.Tables(0).Rows(0).Item("CountryCode") Then
                    intCountryIndex = intCnt + 1
                End If
            Next intCnt
        End If

        Dim dr As DataRow
        dr = objCountryDs.Tables(0).NewRow()
        dr("CountryCode") = ""
        dr("CountryDesc") = "Select Country Code"
        objCountryDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlCountry.DataSource = objCountryDs.Tables(0)
        ddlCountry.DataTextField = "CountryDesc"
        ddlCountry.DataValueField = "CountryCode"
        ddlCountry.DataBind()
        ddlCountry.SelectedIndex = intCountryIndex

        strParam = "Order By ACC.AccCode|And ACC.Status = '" & objGLSetup.EnumAccountCodeStatus.Active & "' " & _
                    " And ACC.AccType = '" & objGLSetup.EnumAccountType.BalanceSheet & "' " & _
                    " And ACC.NurseryInd = '" & objGLSetup.EnumNurseryAccount.No & "' "
        Try
            intErrNo = objGLSetup.mtdGetMasterList(strOpCd_Account, strParam, objGLSetup.EnumGLMasterType.AccountCode, objAccountDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_SETUP_BILLPARTY_ACCOUNT_GET&errmesg=" & lblErrMessage.Text & "&redirect=BI/setup/BI_setup_BillPartyDet.aspx")
        End Try


        If objAccountDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objAccountDs.Tables(0).Rows.Count - 1
                objAccountDs.Tables(0).Rows(intCnt).Item(0) = Trim(objAccountDs.Tables(0).Rows(intCnt).Item(0))
                objAccountDs.Tables(0).Rows(intCnt).Item(1) = objAccountDs.Tables(0).Rows(intCnt).Item(0) & " (" & Trim(objAccountDs.Tables(0).Rows(intCnt).Item(1)) & ")"
                If objAccountDs.Tables(0).Rows(intCnt).Item("AccCode") = objBPDs.Tables(0).Rows(0).Item("AccCode") Then
                    intAccountIndex = intCnt + 1
                End If
            Next intCnt
        End If

        dr = objAccountDs.Tables(0).NewRow()
        dr("AccCode") = ""
        dr("Description") = lblSelect.Text & lblAccount.Text
        objAccountDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlAccCode.DataSource = objAccountDs.Tables(0)
        ddlAccCode.DataTextField = "Description"
        ddlAccCode.DataValueField = "AccCode"
        ddlAccCode.DataBind()
        ddlAccCode.SelectedIndex = intAccountIndex

        If objAccountDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objAccountDs.Tables(0).Rows.Count - 1
                objAccountDs.Tables(0).Rows(intCnt).Item(0) = Trim(objAccountDs.Tables(0).Rows(intCnt).Item(0))
                objAccountDs.Tables(0).Rows(intCnt).Item(1) = objAccountDs.Tables(0).Rows(intCnt).Item(0) & " (" & Trim(objAccountDs.Tables(0).Rows(intCnt).Item(1)) & ")"
                If trim(objAccountDs.Tables(0).Rows(intCnt).Item("AccCode")) = trim(objBPDs.Tables(0).Rows(0).Item("AccCodeAdv")) Then
                    intAccountAdvIndex = intCnt + 1
                End If
            Next intCnt
        End If

        dr = objAccountDs.Tables(0).NewRow()
        dr("AccCode") = ""
        dr("Description") = "Select advance COA"
        objAccountDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlAccCodeAdv.DataSource = objAccountDs.Tables(0)
        ddlAccCodeAdv.DataTextField = "Description"
        ddlAccCodeAdv.DataValueField = "AccCode"
        ddlAccCodeAdv.DataBind()
        ddlAccCodeAdv.SelectedIndex = intAccountAdvIndex


        txtBillPartyCode.Text = strSelectedCode
        txtName.Text = objBPDs.Tables(0).Rows(0).Item("Name")
        txtContactPerson.Text = objBPDs.Tables(0).Rows(0).Item("ContactPerson")
        txtAddress.Value = objBPDs.Tables(0).Rows(0).Item("Address")
        txtTown.Text = objBPDs.Tables(0).Rows(0).Item("Town")
        txtState.Text = objBPDs.Tables(0).Rows(0).Item("State")
        txtPostCode.Text = objBPDs.Tables(0).Rows(0).Item("PostCode")
        txtTelNo.Text = objBPDs.Tables(0).Rows(0).Item("TelNo")
        txtFaxNo.Text = objBPDs.Tables(0).Rows(0).Item("FaxNo")
        txtEmail.Text = objBPDs.Tables(0).Rows(0).Item("Email")
        txtCreditTerm.Text = objBPDs.Tables(0).Rows(0).Item("CreditTerm")
        txtCreditLimit.Text = objBPDs.Tables(0).Rows(0).Item("CreditLimit")
        lblStatus.Text = objGLSetup.mtdGetBillPartyStatus(objBPDs.Tables(0).Rows(0).Item("Status"))
        lblHiddenStatus.Text = objBPDs.Tables(0).Rows(0).Item("Status")
        lblDateCreated.Text = objGlobal.GetLongDate(objBPDs.Tables(0).Rows(0).Item("CreateDate"))
        lblLastUpdate.Text = objGlobal.GetLongDate(objBPDs.Tables(0).Rows(0).Item("UpdateDate"))
        lblUpdateBy.Text = objBPDs.Tables(0).Rows(0).Item("UserName")
        txtBankAccNo.Text = Trim(objBPDs.Tables(0).Rows(0).Item("BankAccNo"))

    End Sub

    Sub onLoad_NewDisplay()
        Dim strOpCd_Country As String = "ADMIN_CLSCOUNTRY_COUNTRY_LIST_GET"
        Dim strOpCd_Account As String = "GL_CLSSETUP_ACCOUNTCODE_LIST_GET"
        Dim strParam As String = strSelectedCode
        Dim intErrNo As Integer
        Dim intCnt As Integer

        Try
            intErrNo = objSysCountry.mtdGetCountryList(strOpCd_Country, objCountryDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_SETUP_BILLPARTY_COUNTRYLIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=BI/setup/BI_setup_BillPartyDet.aspx")
        End Try

        If objCountryDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objCountryDs.Tables(0).Rows.Count - 1
                objCountryDs.Tables(0).Rows(intCnt).Item(0) = Trim(objCountryDs.Tables(0).Rows(intCnt).Item(0))
                objCountryDs.Tables(0).Rows(intCnt).Item(1) = objCountryDs.Tables(0).Rows(intCnt).Item(0) & " (" & Trim(objCountryDs.Tables(0).Rows(intCnt).Item(1)) & ")"
            Next intCnt
        End If

        Dim dr As DataRow
        dr = objCountryDs.Tables(0).NewRow()
        dr("CountryCode") = ""
        dr("CountryDesc") = "Select Country Code"
        objCountryDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlCountry.DataSource = objCountryDs.Tables(0)
        ddlCountry.DataTextField = "CountryDesc"
        ddlCountry.DataValueField = "CountryCode"
        ddlCountry.DataBind()




        strParam = "Order By ACC.AccCode|And ACC.Status = '" & _
                    objGLSetup.EnumAccountCodeStatus.Active & _
                    "' And ACC.AccType = '" & objGLSetup.EnumAccountType.BalanceSheet & "'"

        Try
            intErrNo = objGLSetup.mtdGetMasterList(strOpCd_Account, strParam, objGLSetup.EnumGLMasterType.AccountCode, objAccountDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_SETUP_BILLPARTY_ACCOUNTLIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=BI/setup/BI_setup_BillPartyDet.aspx")
        End Try

        If objAccountDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objAccountDs.Tables(0).Rows.Count - 1
                objAccountDs.Tables(0).Rows(intCnt).Item(0) = Trim(objAccountDs.Tables(0).Rows(intCnt).Item(0))
                objAccountDs.Tables(0).Rows(intCnt).Item(1) = objAccountDs.Tables(0).Rows(intCnt).Item(0) & " (" & Trim(objAccountDs.Tables(0).Rows(intCnt).Item(1)) & ")"
            Next intCnt
        End If

        dr = objAccountDs.Tables(0).NewRow()
        dr("AccCode") = ""
        dr("Description") = "Select Account Code"
        objAccountDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlAccCode.DataSource = objAccountDs.Tables(0)
        ddlAccCode.DataTextField = "Description"
        ddlAccCode.DataValueField = "AccCode"
        ddlAccCode.DataBind()

        ddlAccCodeAdv.DataSource = objAccountDs.Tables(0)
        ddlAccCodeAdv.DataTextField = "Description"
        ddlAccCodeAdv.DataValueField = "AccCode"
        ddlAccCodeAdv.DataBind()
    End Sub

    Sub btnSave_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCd_Upd As String = "GL_CLSSETUP_BILLPARTY_DETAILS_UPD"
        Dim strOpCd_Add As String = "GL_CLSSETUP_BILLPARTY_ADD_AUTO" '"GL_CLSSETUP_BILLPARTY_ADD"
        Dim strOpCd_Get As String = "GL_CLSSETUP_BILLPARTY_GET"
        Dim intErrNo As Integer
        Dim strParam As String
        Dim strSearch As String
        Dim strName As String = Replace(txtName.Text, "'", "''")
        Dim strContactPerson As String = txtContactPerson.Text
        Dim strAddress As String = Replace(txtAddress.Value, "'", "''")
        Dim strTown As String = Replace(txtTown.Text, "'", "''")
        Dim strState As String = Replace(txtState.Text, "'", "''")
        Dim strPostCode As String = txtPostCode.Text
        Dim strCountry As String = ddlCountry.SelectedItem.Value
        Dim strTelNo As String = txtTelNo.Text
        Dim strFaxNo As String = txtFaxNo.Text
        Dim strEmail As String = txtEmail.Text
        Dim strAddChrg As String = "0"
        Dim strCreditTerm As String = txtCreditTerm.Text
        Dim strTermType As String = ddlTermType.SelectedItem.Value
        Dim strCreditLimit As String = txtCreditLimit.Text
        Dim strAccCode As String = Request.Form("ddlAccCode")
        Dim strAccCodeAdv As String = Request.Form("ddlAccCodeAdv")
        Dim strInterLocCode As String = Trim(ddlInterLocCode.SelectedItem.Value)
        Dim strBankCode As String = Trim(ddlBankCode.SelectedItem.Value)
        Dim strBankAccNo As String = txtBankAccNo.Text
        Dim strAdditionalNote As String = taAdditionalNote.Value
        Dim strPIC As String = txtPIC.Text.Trim
        Dim strPICPos As String = txtPosition.Text.Trim
        Dim strPPN As String = IIf(chkPPN.Checked = True, "1", "0")
        Dim strTermOfWeighing As String = taTermOfWeighing.Value.Trim
        Dim strTermOfPayment As String = taTermOfPayment.Value.Trim
        Dim strQualityCPO As String = taProdQualityCPO.Value.Trim
        Dim strClaimCPO As String = taProdClaimCPO.Value.Trim
        Dim strQualityPK As String = taProdQualityPK.Value.Trim
        Dim strClaimPK As String = taProdClaimPK.Value.Trim
        Dim strLoadDest As String = taLoadDest.Value.Trim

        Dim strParamName As String
        Dim strParamValue As String
        Dim objSPLDs As New Object

        If Len(strAddress) > 512 Then
            lblErrAddress.Visible = True
            Exit Sub
        ElseIf strAccCode = "" Then
            lblErrAccCode.Visible = True
            Exit Sub
        ElseIf strInterLocCode = "" Then
            lblErrInterLocCode.Visible = True
            Exit Sub
        ElseIf strCountry = "" Then
            lblErrCountry.Visible = True
            Exit Sub
        End If

        'If CheckDuplicatedInterLoc(txtBillPartyCode.Text, strInterLocCode) = True Then
        '    lblDupInterLocCode.Visible = True
        '    Exit Sub
        'End If

        If strCreditLimit = "" Then
            strCreditLimit = "0"
        End If

        If txtBillPartyCode.Text = "" Then
            strParamName = "BILLPARTYCODE|NAME|CONTACTPERSON|ADDRESS|TOWN|STATE|POSTCODE|" & _
                                   "COUNTRYCODE|TELNO|FAXNO|EMAIL|ADDCHRG|CREDITTERM|TERMTYPE|CREDITLIMIT|" & _
                                   "ACCCODE|STATUS|CREATEDATE|UPDATEDATE|UPDATEID|INTERLOCCODE|BANKCODE|BANKACCNO|ADDITIONALNOTE|" & _
                                   "PIC|PICPOS|PPNINIT|TERMOFWEIGHING|QUALITYCPO|CLAIMCPO|QUALITYPK|CLAIMPK|LOADDEST|TERMOFPAYMENT|ACCCODEADV"

            strParamValue = txtBillPartyCode.Text & "|" & _
                    strName & "|" & _
                    strContactPerson & "|" & _
                    strAddress & "|" & _
                    strTown & "|" & _
                    strState & "|" & _
                    strPostCode & "|" & _
                    strCountry & "|" & _
                    strTelNo & "|" & _
                    strFaxNo & "|" & _
                    strEmail & "|" & _
                    strAddChrg & "|" & _
                    strCreditTerm & "|" & _
                    strTermType & "|" & _
                    strCreditLimit & "|" & _
                    strAccCode & "|" & _
                    objGLSetup.EnumBillPartyStatus.Active & "|" & _
                    Now() & "|" & _
                    Now() & "|" & _
                    Trim(strUserId) & "|" & _
                    strInterLocCode & "|" & _
                    strBankCode & "|" & _
                    strBankAccNo & "|" & _
                    strAdditionalNote & "|" & _
                    strPIC & "|" & _
                    strPICPos & "|" & _
                    strPPN & "|" & _
                    strTermOfWeighing & "|" & _
                    strQualityCPO & "|" & _
                    strClaimCPO & "|" & _
                    strQualityPK & "|" & _
                    strClaimPK & "|" & _
                    strLoadDest & "|" & _
                    strTermOfPayment & "|" & _
                    strAccCodeAdv

            Try
                intErrNo = objGLTrx.mtdGetDataCommon(strOpCd_Add, _
                                                    strParamName, _
                                                    strParamValue, _
                                                    objSPLDs)

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=TX_Setup_TaxObjectDet&errmesg=" & lblErrMessage.Text & "&redirect=")
            End Try

            If objSPLDs.Tables(0).Rows.Count > 0 Then
                txtBillPartyCode.Text = Trim(objSPLDs.Tables(0).Rows(0).Item("BillPartyCode"))
            End If

        Else

            strParam = txtBillPartyCode.Text & "|" & _
                    strName & "|" & _
                    strContactPerson & "|" & _
                    strAddress & "|" & _
                    strTown & "|" & _
                    strState & "|" & _
                    strPostCode & "|" & _
                    strCountry & "|" & _
                    strTelNo & "|" & _
                    strFaxNo & "|" & _
                    strEmail & "|" & _
                    strAddChrg & "|" & _
                    strCreditTerm & "|" & _
                    strTermType & "|" & _
                    strCreditLimit & "|" & _
                    strAccCode & "|" & _
                    objGLSetup.EnumBillPartyStatus.Active & "|" & _
                    strInterLocCode & "|" & _
                    strBankCode & "|" & _
                    strBankAccNo & "|" & _
                    strAdditionalNote & "|" & _
                    strPIC & "|" & _
                    strPICPos & "|" & _
                    strPPN & "|" & _
                    strTermOfWeighing & "|" & _
                    strQualityCPO & "|" & _
                    strClaimCPO & "|" & _
                    strQualityPK & "|" & _
                    strClaimPK & "|" & _
                    strLoadDest & "|" & _
                    strTermOfPayment & "|" & _
                    strAccCodeAdv

            Try
                intErrNo = objGLSetup.mtdUpdBillParty(strOpCd_Upd, _
                                                    "", _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strParam, _
                                                    False)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_SETUP_BILLPARTY_UPD&errmesg=" & lblErrMessage.Text & "&redirect=BI/setup/BI_setup_BillPartyDet.aspx")
            End Try

        End If


        strSelectedCode = txtBillPartyCode.Text

        onLoad_Display()
        BindButton()
    End Sub

    Sub btnDelete_Click(Sender As Object, E As ImageClickEventArgs)
        Dim strOpCd_Upd As String = "GL_CLSSETUP_BILLPARTY_UPD"
        Dim strOpCd_Add As String = ""
        Dim strParam As String = ""
        Dim intErrNo As Integer
        Dim strBankCode As String = ""
        Dim strBankAccNo As String = ""


        strParam = strSelectedCode & "||||||||||||||||" & objGLSetup.EnumBillPartyStatus.Deleted & "|||||||||||||||"
                 

        Try
            intErrNo = objGLSetup.mtdUpdBillParty(strOpCd_Add, _
                                                strOpCd_Upd, _
                                                strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strParam, _
                                                True)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_SETUP_BILLPARTY_DEL&errmesg=" & lblErrMessage.Text & "&redirect=BI/setup/BI_setup_BillPartyDet.aspx")
        End Try

        onLoad_Display()
        BindButton()
    End Sub


    Sub btnUnDelete_Click(Sender As Object, E As ImageClickEventArgs)
        Dim strOpCd_Upd As String = "GL_CLSSETUP_BILLPARTY_UPD"
        Dim strOpCd_Add As String = ""
        Dim strParam As String = ""
        Dim intErrNo As Integer
        Dim strBankCode As String = ""
        Dim strBankAccNo As String = ""


        strParam = strSelectedCode & "||||||||||||||||" & objGLSetup.EnumBillPartyStatus.Active & "||||||||||||||"

        Try
            intErrNo = objGLSetup.mtdUpdBillParty(strOpCd_Add, _
                                                strOpCd_Upd, _
                                                strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strParam, _
                                                True)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_SETUP_BILLPARTY_UNDEL&errmesg=" & lblErrMessage.Text & "&redirect=BI/setup/BI_setup_BillPartyDet.aspx")
        End Try

        onLoad_Display()
        BindButton()
    End Sub


    Sub btnBack_Click(Sender As Object, E As ImageClickEventArgs)
        Response.Redirect("BI_setup_BillPartyList.aspx")
    End Sub

    Sub BindInterLocCode(ByVal pv_strLocCode As String)
        Dim strOpCd As String = "ADMIN_CLSLOC_INTER_ESTATE_LOCATION_GET"
        Dim dr As DataRow
        Dim dsLoc As DataSet
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer = 0

        intSelectedIndex = 1
        Try
            strParam = objAdminLoc.EnumLocStatus.Active & "|" & _
                       Trim(Session("SS_COMPANY")) & "|" & _
                       Trim(Session("SS_LOCATION")) & "|" & _
                       Trim(Session("SS_USERID")) & "|" & _
                       objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.Billing) & "|" & _
                       "BIAR" & "|" & _
                       objAR.mtdGetAccessRights(objAR.EnumBIAccessRights.BIBillParty) & "|" & _
                       "BIAR" & "|" & _
                       objAR.mtdGetAccessRights(objAR.EnumBIAccessRights.BIBillParty)
            intErrNo = objAdminLoc.mtdGetInterEstateLoc(strOpCd, strParam, dsLoc)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AR_CLSSETUP_BP_INTER_LOC_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        If dsLoc.Tables(0).Rows.Count > 0 Then
            dr = dsLoc.Tables(0).NewRow()
            dr("LocCode") = "NONE"
            dr("Description") = "NONE (Non Inter Location Bill Party)"
            dsLoc.Tables(0).Rows.InsertAt(dr, 0)
        End If

        For intCnt = 0 To dsLoc.Tables(0).Rows.Count - 1
            dsLoc.Tables(0).Rows(intCnt).Item("LocCode") = Trim(dsLoc.Tables(0).Rows(intCnt).Item("LocCode"))
            dsLoc.Tables(0).Rows(intCnt).Item("Description") = Trim(dsLoc.Tables(0).Rows(intCnt).Item("Description"))
            If dsLoc.Tables(0).Rows(intCnt).Item("LocCode") = Trim(pv_strLocCode) Then
                intSelectedIndex = intCnt + 1
            End If
        Next

        If Trim(pv_strLocCode) <> "" And intSelectedIndex = 0 Then
            dr = dsLoc.Tables(0).NewRow()
            dr("LocCode") = Trim(pv_strLocCode)
            dr("Description") = Trim(pv_strLocCode) & " (Deleted)"
            dsLoc.Tables(0).Rows.InsertAt(dr, 0)
            intSelectedIndex = 1
        End If

        dr = dsLoc.Tables(0).NewRow()
        dr("LocCode") = ""
        dr("Description") = lblSelect.Text & GetCaption(objLangCap.EnumLangCap.Location) & lblCode.Text

        dsLoc.Tables(0).Rows.InsertAt(dr, 0)
        ddlInterLocCode.DataSource = dsLoc.Tables(0)
        ddlInterLocCode.DataValueField = "LocCode"
        ddlInterLocCode.DataTextField = "Description"
        ddlInterLocCode.DataBind()
        ddlInterLocCode.SelectedIndex = intSelectedIndex

        If Not dsLoc Is Nothing Then
            dsLoc = Nothing
        End If
    End Sub

    Function CheckDuplicatedInterLoc(ByVal pv_strBPCode As String, ByVal pv_strLocCode As String) As Boolean
        Dim strOpCd_Get As String = "GL_CLSSETUP_BILLPARTY_GET"
        Dim strSearch As String
        Dim objBPLocDs As New Object()
        Dim intErrNo As Integer

        If TRIM(pv_strLocCode) = "NONE" Then
            return False
        End If

        strSearch = " AND BP.Status = '" & objGLSetup.EnumBillPartyStatus.Active & "'" & _
                    " AND BP.InterLocCode = '" & pv_strLocCode & "'" & _
                    " AND BP.BillPartyCode NOT IN ('" & pv_strBPCode & "')"
            
        Try
            intErrNo = objGLSetup.mtdGetBPInterLoc(strOpCd_Get, strSearch, objBPLocDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_SETUP_BILLPARTY_GET&errmesg=" & lblErrMessage.Text & "&redirect=BI/setup/BI_setup_BillPartyDet.aspx")
        End Try

        If objBPLocDs.Tables(0).Rows.Count > 0 Then
            return True
        Else
            return False
        End If
    End Function

    Sub BindBankCode(ByVal pv_BankCode as String)
        Dim strOpCd_GET As String = "HR_CLSSETUP_BANK_SEARCH"
        Dim strSrchBankCode As String
        Dim strSrchDesc As String
        Dim strSrchStatus As String
        Dim strSrchLastUpdate As String
        Dim strSearch As String
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer = 0
        Dim dr As DataRow
        Dim dsBank As DataSet

        intSelectedIndex = 0

        
        strParam = "||||B.BankCode|"

        Try
            intErrNo = objHRSetup.mtdGetBank(strOpCd_GET, strParam, objBankDs, False)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=HR_SETUP_BANK_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        If objBankDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objBankDs.Tables(0).Rows.Count - 1
                objBankDs.Tables(0).Rows(intCnt).Item("BankCode") = Trim(objBankDs.Tables(0).Rows(intCnt).Item("BankCode"))
                objBankDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objBankDs.Tables(0).Rows(intCnt).Item("Description"))
                If objBankDs.Tables(0).Rows(intCnt).Item("BankCode") = Trim(pv_BankCode) Then
                    intSelectedIndex = intCnt + 1
                End If
            Next
        End If

        dr = objBankDs.Tables(0).NewRow()
        dr("BankCode") = ""
        dr("Description") = "Please Select Bank Code"
        objBankDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlBankCode.DataSource = objBankDs.Tables(0)
        ddlBankCode.DataValueField = "BankCode"
        ddlBankCode.DataTextField = "Description"
        ddlBankCode.DataBind()
        ddlBankCode.SelectedIndex = intSelectedIndex

        
    End Sub

    Sub chkPPN_Change(ByVal Sender As Object, ByVal E As EventArgs)
        If chkPPN.Checked = True Then
            chkPPN.Text = "  "
        Else
            chkPPN.Text = "  "
        End If
    End Sub
End Class
