
Imports System
Imports System.Data
Imports System.IO 
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic.Interaction
Imports Microsoft.VisualBasic
Imports agri.Admin.clsLoc
Imports agri.GlobalHdl.clsGlobalHdl
Imports agri.PWSystem.clsLangCap

Public Class Admin_LocDet : Inherits Page

    Protected WithEvents txtLocCode As TextBox
    Protected WithEvents txtDescription As TextBox
    Protected WithEvents txtAddress As HtmlTextArea
    Protected WithEvents txtPostCode As TextBox
    Protected WithEvents txtState As TextBox
    Protected WithEvents txtCity As TextBox
    Protected WithEvents ddlCountry As DropDownList
    Protected WithEvents txtTelNo As TextBox
    Protected WithEvents txtFaxNo As TextBox
    Protected WithEvents txtNPWP As TextBox
    Protected WithEvents txtJamsostekNo As TextBox
    Protected WithEvents txtPPNRate As TextBox
    Protected WithEvents txtUMR As TextBox
    Protected WithEvents txtMillCapacity As TextBox
    Protected WithEvents ddlNearLocCode As DropDownList
    Protected WithEvents lblNearLocCode As Label
    Protected WithEvents ddlAccount As DropDownList
    Protected WithEvents CloseAutoDistInd_Yes As RadioButton
    Protected WithEvents CloseAutoDistInd_No As RadioButton
    Protected WithEvents AnaCtrlCost_Block As RadioButton

    Protected WithEvents LocType_Mill As RadioButton
    Protected WithEvents LocType_Estate As RadioButton

    Protected WithEvents LocLevel_Estate As RadioButton
    Protected WithEvents LocLevel_Perwakilan As RadioButton
    Protected WithEvents LocLevel_HQ As RadioButton
    Protected WithEvents LocLevel_KCP As RadioButton
    Protected WithEvents LocLevel_PowerPlant As RadioButton

    Protected WithEvents AnaCtrlCost_SubBlock As RadioButton
    Protected WithEvents AnaCtrlYield_Block As RadioButton
    Protected WithEvents AnaCtrlYield_SubBlock As RadioButton
    Protected WithEvents txtEmpPrefix As TextBox
    Protected WithEvents txtEmpCodeDigit As TextBox
    Protected WithEvents lblCurrId As Label

    Protected WithEvents SaveBtn As ImageButton
    Protected WithEvents DelBtn As ImageButton
    Protected WithEvents UnDelBtn As ImageButton
    Protected WithEvents lblStatus As Label
    Protected WithEvents lblDateCreated As Label
    Protected WithEvents lblLastUpdate As Label
    Protected WithEvents lblUpdatedBy As Label
    Protected WithEvents lblHiddenStatus As Label
    Protected WithEvents loccode As HtmlInputHidden
    Protected WithEvents btnFind As HtmlInputButton
    Protected WithEvents lblErrBlank As Label
    Protected WithEvents lblErrDup As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblErrCountry As Label
    Protected WithEvents lblErrAddress As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents lblErrEnter As Label
    Protected WithEvents lblSelect As Label
    Protected WithEvents lblTitle As Label
    Protected WithEvents lblLocation As label
    Protected WithEvents lblDesc As Label
    Protected WithEvents lblAccount As Label
    Protected WithEvents rfvCode As RequiredFieldValidator

    Protected WithEvents txtInitAccCode As TextBox

    Dim objAdmin As New agri.Admin.clsLoc()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objGLSetup As New agri.GL.clsSetup()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objSysCountry As New agri.Admin.clsCountry 
    Dim objSysComp As New agri.Admin.clsComp()


    Protected WithEvents ddlRDP As DropDownList
    Protected WithEvents lblErrRDP As Label

    Protected WithEvents ddlBankCode As DropDownList
    Protected WithEvents txtBankAccName As TextBox
    Protected WithEvents txtBankAccNo As TextBox
    Protected WithEvents ddlBankAccCode As DropDownList
    Protected WithEvents txtBankAddress As TextBox
    Protected WithEvents txtBankTown As TextBox
    Protected WithEvents txtBankState As TextBox
    Protected WithEvents ddlBankCountry As DropDownList
    Protected WithEvents lblErrBankAccCode As Label
    Protected WithEvents lblerrBankCode As Label
    Protected WithEvents lblErrBankAccName As Label
    Protected WithEvents lblerrBankAccNo As Label
    Protected WithEvents dgLineDet As DataGrid
    Protected WithEvents hidBankAccNo As HtmlInputHidden
    Protected WithEvents hidStatus As HtmlInputHidden
    Protected WithEvents txtBankRemark As TextBox

    Dim objLocCodeDs As New Object()
    Dim objAccDs As New Object()
    Dim objLangCapDs As New Object()
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim objHR As New agri.HR.clsSetup()
    Dim objGLTrx As New agri.GL.ClsTrx()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim intADAR As Integer
    Dim strLocType As String

    Protected WithEvents txtEstMgr As TextBox
    Protected WithEvents txtKasie As TextBox

    Dim strSelectedLocCode As String = ""
    Dim strSortExpression As String = "LocCode"

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        intADAR = Session("SS_ADAR")
        strLocType = Session("SS_LOCTYPE")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumADAccessRights.ADLocation), intADAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            onload_GetLangCap()
            strSelectedLocCode = Trim(IIf(Request.QueryString("loccode") <> "", Request.QueryString("loccode"), Request.Form("loccode")))
            lblErrCountry.Visible = False
            lblerrBankCode.Visible = False
            lblErrBankAccName.Visible = False
            lblerrBankAccNo.Visible = False
            lblErrBankAccCode.Visible = False

            If Not IsPostBack Then
                If strSelectedLocCode <> "" Then
                    loccode.Value = strSelectedLocCode
                    onLoad_Display()
                    onLoad_DisplayLn(txtLocCode.Text)
                Else
                    BindAccount("")
                    BindCountryList("")
                    BindNearLocList("")
                    BindRDP("")

                End If
                BindBank()
                BindButton()
            End If
        End If
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblLocation.Text = GetCaption(objLangCap.EnumLangCap.Location)
        lblNearLocCode.Text = GetCaption(objLangCap.EnumLangCap.NearestLocation)
        lblDesc.Text = GetCaption(objLangCap.EnumLangCap.LocDesc)
        lblTitle.Text = UCase(lblLocation.Text)
        lblAccount.Text = GetCaption(objLangCap.EnumLangCap.Account)
        rfvCode.ErrorMessage = lblErrEnter.Text & lblLocation.Text & lblCode.Text & "."
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
                                                 "", _
                                                 "", _
                                                 objLangCapDs, _
                                                 strParam)
        Catch Exp As Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=PU_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=admin/location/admin_location_locdet.aspx")
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
        txtLocCode.Enabled = False
        txtDescription.Enabled = False
        ddlAccount.Enabled = False
        CloseAutoDistInd_Yes.Enabled = False
        CloseAutoDistInd_No.Enabled = False

        LocType_Mill.Enabled = False
        LocType_Estate.Enabled = False

        LocLevel_Estate.Enabled = False
        LocLevel_Perwakilan.Enabled = False
        LocLevel_HQ.Enabled = False

        AnaCtrlCost_Block.Enabled = False
        AnaCtrlCost_SubBlock.Enabled = False
        AnaCtrlYield_Block.Enabled = False
        AnaCtrlYield_SubBlock.Enabled = False
        txtEmpPrefix.Enabled = False
        txtEmpCodeDigit.Enabled = False
        SaveBtn.Visible = False
        DelBtn.Visible = False
        UnDelBtn.Visible = False
        btnFind.Disabled = False
        txtEstMgr.Enabled = False
        txtKasie.Enabled = False

        Select Case CInt(lblHiddenStatus.Text)
            Case objAdmin.EnumLocStatus.Active
                txtDescription.Enabled = True
                ddlAccount.Enabled = True
                CloseAutoDistInd_Yes.Enabled = True
                CloseAutoDistInd_No.Enabled = True

                LocType_Mill.Enabled = True
                LocType_Estate.Enabled = True

                LocLevel_Estate.Enabled = True
                LocLevel_Perwakilan.Enabled = True
                LocLevel_HQ.Enabled = True
                LocLevel_KCP.Enabled = True
                LocLevel_PowerPlant.Enabled = True

                txtEstMgr.Enabled = True
                txtKasie.Enabled = True

                AnaCtrlCost_Block.Enabled = True
                AnaCtrlCost_SubBlock.Enabled = True
                AnaCtrlYield_Block.Enabled = True
                AnaCtrlYield_SubBlock.Enabled = True
                txtEmpPrefix.Enabled = True
                txtEmpCodeDigit.Enabled = True
                SaveBtn.Visible = True
                DelBtn.Visible = True
                DelBtn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
            Case objAdmin.EnumLocStatus.Deleted
                UnDelBtn.Visible = True
                btnFind.Disabled = True
            Case Else
                txtLocCode.Enabled = True
                txtDescription.Enabled = True
                ddlAccount.Enabled = True
                CloseAutoDistInd_Yes.Enabled = True
                CloseAutoDistInd_No.Enabled = True

                LocType_Mill.Enabled = True
                LocType_Estate.Enabled = True

                LocLevel_Estate.Enabled = True
                LocLevel_Perwakilan.Enabled = True
                LocLevel_HQ.Enabled = True
                LocLevel_KCP.Enabled = True
                LocLevel_PowerPlant.Enabled = True

                txtEstMgr.Enabled = True
                txtKasie.Enabled = True

                AnaCtrlCost_Block.Enabled = True
                AnaCtrlCost_SubBlock.Enabled = True
                AnaCtrlYield_Block.Enabled = True
                AnaCtrlYield_SubBlock.Enabled = True
                txtEmpPrefix.Enabled = True
                txtEmpCodeDigit.Enabled = True
                SaveBtn.Visible = True
        End Select
    End Sub

    Sub onLoad_Display()
        Dim strOpCd_Get As String = "ADMIN_CLSLOC_LOCATION_LIST_GET"
        Dim strParam As String = ""
        Dim intErrNo As Integer
        Dim intRDPindex As Integer = 0


        strParam = strSelectedLocCode & "||||" & strSortExpression & "||"

        Try
            intErrNo = objAdmin.mtdGetLocCode(strOpCd_Get, strParam, objLocCodeDs)
        Catch Exp As Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADMIN_GET_LOCATION&errmesg=" & lblErrMessage.Text & "&redirect=admin/location/admin_location_locdet.aspx")
        End Try

        If objLocCodeDs.Tables(0).Rows.Count > 0 Then
            objLocCodeDs.Tables(0).Rows(0).Item("LocCode") = objLocCodeDs.Tables(0).Rows(0).Item("LocCode").Trim()
            objLocCodeDs.Tables(0).Rows(0).Item("Description") = objLocCodeDs.Tables(0).Rows(0).Item("Description").Trim()
            objLocCodeDs.Tables(0).Rows(0).Item("CloseAutoDistInd") = objLocCodeDs.Tables(0).Rows(0).Item("CloseAutoDistInd").Trim()
            objLocCodeDs.Tables(0).Rows(0).Item("Status") = objLocCodeDs.Tables(0).Rows(0).Item("Status").Trim()
            objLocCodeDs.Tables(0).Rows(0).Item("UpdateID") = objLocCodeDs.Tables(0).Rows(0).Item("UpdateID").Trim()
            objLocCodeDs.Tables(0).Rows(0).Item("UserName") = objLocCodeDs.Tables(0).Rows(0).Item("UserName").Trim()
            objLocCodeDs.Tables(0).Rows(0).Item("AnaCtrlCost") = objLocCodeDs.Tables(0).Rows(0).Item("AnaCtrlCost").Trim()
            objLocCodeDs.Tables(0).Rows(0).Item("AnaCtrlYield") = objLocCodeDs.Tables(0).Rows(0).Item("AnaCtrlYield").Trim()

            objLocCodeDs.Tables(0).Rows(0).Item("Address") = objLocCodeDs.Tables(0).Rows(0).Item("Address").Trim()
            objLocCodeDs.Tables(0).Rows(0).Item("PostCode") = objLocCodeDs.Tables(0).Rows(0).Item("PostCode").Trim()
            objLocCodeDs.Tables(0).Rows(0).Item("State") = objLocCodeDs.Tables(0).Rows(0).Item("State").Trim()
            objLocCodeDs.Tables(0).Rows(0).Item("City") = objLocCodeDs.Tables(0).Rows(0).Item("City").Trim()
            objLocCodeDs.Tables(0).Rows(0).Item("CountryCode") = objLocCodeDs.Tables(0).Rows(0).Item("CountryCode").Trim()
            objLocCodeDs.Tables(0).Rows(0).Item("TelNo") = objLocCodeDs.Tables(0).Rows(0).Item("TelNo").Trim()
            objLocCodeDs.Tables(0).Rows(0).Item("FaxNo") = objLocCodeDs.Tables(0).Rows(0).Item("FaxNo").Trim()
            objLocCodeDs.Tables(0).Rows(0).Item("NPWP") = objLocCodeDs.Tables(0).Rows(0).Item("NPWP").Trim()
            objLocCodeDs.Tables(0).Rows(0).Item("JamsostekNo") = objLocCodeDs.Tables(0).Rows(0).Item("JamsostekNo").Trim()
            objLocCodeDs.Tables(0).Rows(0).Item("RDP") = Trim(objLocCodeDs.Tables(0).Rows(0).Item("RDP"))

            txtLocCode.Text = objLocCodeDs.Tables(0).Rows(0).Item("LocCode")
            txtDescription.Text = objLocCodeDs.Tables(0).Rows(0).Item("Description")
            txtAddress.Value = objLocCodeDs.Tables(0).Rows(0).Item("Address")
            txtPostCode.Text = objLocCodeDs.Tables(0).Rows(0).Item("PostCode")
            txtState.Text = objLocCodeDs.Tables(0).Rows(0).Item("State")
            txtCity.Text = objLocCodeDs.Tables(0).Rows(0).Item("City")
            BindCountryList(objLocCodeDs.Tables(0).Rows(0).Item("CountryCode"))
            BindNearLocList(objLocCodeDs.Tables(0).Rows(0).Item("NearLocCode"))
            txtTelNo.Text = objLocCodeDs.Tables(0).Rows(0).Item("TelNo")
            txtFaxNo.Text = objLocCodeDs.Tables(0).Rows(0).Item("FaxNo")
            txtNPWP.Text = objLocCodeDs.Tables(0).Rows(0).Item("NPWP")
            txtJamsostekNo.Text = objLocCodeDs.Tables(0).Rows(0).Item("JamsostekNo")
            txtEstMgr.Text = Trim(objLocCodeDs.Tables(0).Rows(0).Item("Manager"))
            txtKasie.Text = Trim(objLocCodeDs.Tables(0).Rows(0).Item("Kasie"))
            txtPPNRate.Text = FormatNumber(objLocCodeDs.Tables(0).Rows(0).Item("PPNRate"), 0, True, False, False)
            txtUMR.Text = FormatNumber(objLocCodeDs.Tables(0).Rows(0).Item("UMR"), 0, True, False, False)
            txtMillCapacity.Text = FormatNumber(objLocCodeDs.Tables(0).Rows(0).Item("MillCapacity"), 0, True, False, False)
            txtEmpPrefix.Text = objLocCodeDs.Tables(0).Rows(0).Item("EmpPrefix").Trim()
            txtEmpCodeDigit.Text = objLocCodeDs.Tables(0).Rows(0).Item("EmpCodeLength")
            lblCurrId.Text = objLocCodeDs.Tables(0).Rows(0).Item("EmpCodeVal")

            If objLocCodeDs.Tables(0).Rows(0).Item("CloseAutoDistInd") = objAdmin.EnumCloseAutoDistInd.Yes Then
                CloseAutoDistInd_Yes.Checked = True
            Else
                CloseAutoDistInd_No.Checked = True
            End If

            If objLocCodeDs.Tables(0).Rows(0).Item("LocType") = objAdmin.EnumLocType.Mill Then
                LocType_Mill.Checked = True
                LocType_Estate.Checked = False
            Else
                LocType_Mill.Checked = False
                LocType_Estate.Checked = True
            End If

            If objLocCodeDs.Tables(0).Rows(0).Item("LocLevel") = objAdmin.EnumLocLevel.Estate Then
                LocLevel_Estate.Checked = True
                LocLevel_Perwakilan.Checked = False
                LocLevel_HQ.Checked = False
                LocLevel_KCP.Checked = False
                LocLevel_PowerPlant.Checked = False
            ElseIf objLocCodeDs.Tables(0).Rows(0).Item("LocLevel") = objAdmin.EnumLocLevel.Perwakilan Then
                LocLevel_Estate.Checked = False
                LocLevel_Perwakilan.Checked = True
                LocLevel_HQ.Checked = False
                LocLevel_KCP.Checked = False
                LocLevel_PowerPlant.Checked = False
            ElseIf objLocCodeDs.Tables(0).Rows(0).Item("LocLevel") = objAdmin.EnumLocLevel.HQ Then
                LocLevel_Estate.Checked = False
                LocLevel_Perwakilan.Checked = False
                LocLevel_HQ.Checked = True
                LocLevel_KCP.Checked = False
                LocLevel_PowerPlant.Checked = False
            ElseIf objLocCodeDs.Tables(0).Rows(0).Item("LocLevel") = objAdmin.EnumLocLevel.KCP Then
                LocLevel_Estate.Checked = False
                LocLevel_Perwakilan.Checked = False
                LocLevel_HQ.Checked = False
                LocLevel_KCP.Checked = True
                LocLevel_PowerPlant.Checked = False
            ElseIf objLocCodeDs.Tables(0).Rows(0).Item("LocLevel") = objAdmin.EnumLocLevel.PowerPlant Then
                LocLevel_Estate.Checked = False
                LocLevel_Perwakilan.Checked = False
                LocLevel_HQ.Checked = False
                LocLevel_KCP.Checked = False
                LocLevel_PowerPlant.Checked = True
            End If

            If objLocCodeDs.Tables(0).Rows(0).Item("AnaCtrlCost") = objAdmin.EnumAnaCtrlCost.Block Then
                AnaCtrlCost_Block.Checked = True
            Else
                AnaCtrlCost_SubBlock.Checked = True
            End If
            If objLocCodeDs.Tables(0).Rows(0).Item("AnaCtrlYield") = objAdmin.EnumAnaCtrlYield.Block Then
                AnaCtrlYield_Block.Checked = True
            Else
                AnaCtrlYield_SubBlock.Checked = True
            End If

            lblStatus.Text = objAdmin.mtdGetLocStatus(objLocCodeDs.Tables(0).Rows(0).Item("Status"))
            lblHiddenStatus.Text = Trim(objLocCodeDs.Tables(0).Rows(0).Item("Status"))
            lblDateCreated.Text = objGlobal.GetLongDate(objLocCodeDs.Tables(0).Rows(0).Item("CreateDate"))
            lblLastUpdate.Text = objGlobal.GetLongDate(objLocCodeDs.Tables(0).Rows(0).Item("UpdateDate"))
            lblUpdatedBy.Text = objLocCodeDs.Tables(0).Rows(0).Item("UserName")

            BindAccount(Trim(objLocCodeDs.Tables(0).Rows(0).Item("AccCode")))

            ddlRDP.Items.Clear()
            ddlRDP.Items.Add(New ListItem("Select Running Digits Prefix", ""))
            ddlRDP.Items.Add(New ListItem(objSysComp.mtdGetRDP(objSysComp.EnumRDP.UnitUsaha), objSysComp.EnumRDP.UnitUsaha))
            ddlRDP.Items.Add(New ListItem(objSysComp.mtdGetRDP(objSysComp.EnumRDP.Jakarta), objSysComp.EnumRDP.Jakarta))
            ddlRDP.Items.Add(New ListItem(objSysComp.mtdGetRDP(objSysComp.EnumRDP.PekanBaru), objSysComp.EnumRDP.PekanBaru))
            ddlRDP.Items.Add(New ListItem(objSysComp.mtdGetRDP(objSysComp.EnumRDP.Banjarmasin), objSysComp.EnumRDP.Banjarmasin))
            ddlRDP.Items.Add(New ListItem(objSysComp.mtdGetRDP(objSysComp.EnumRDP.BalikPapan), objSysComp.EnumRDP.BalikPapan))
            ddlRDP.Items.Add(New ListItem(objSysComp.mtdGetRDP(objSysComp.EnumRDP.UjungPandang), objSysComp.EnumRDP.UjungPandang))

            If objLocCodeDs.Tables(0).Rows(0).Item("RDP") = objSysComp.EnumRDP.UnitUsaha Then
                intRDPindex = 1
            ElseIf objLocCodeDs.Tables(0).Rows(0).Item("RDP") = objSysComp.EnumRDP.Jakarta Then
                intRDPindex = 2
            ElseIf objLocCodeDs.Tables(0).Rows(0).Item("RDP") = objSysComp.EnumRDP.PekanBaru Then
                intRDPindex = 3
            ElseIf objLocCodeDs.Tables(0).Rows(0).Item("RDP") = objSysComp.EnumRDP.Banjarmasin Then
                intRDPindex = 4
            ElseIf objLocCodeDs.Tables(0).Rows(0).Item("RDP") = objSysComp.EnumRDP.BalikPapan Then
                intRDPindex = 5
            Else
                intRDPindex = 6
            End If

            ddlRDP.SelectedIndex = intRDPindex

        End If
    End Sub

    Sub BindAccount(ByVal pv_strAccCode As String)
        Dim strOpCd As String = "GL_CLSSETUP_ACCOUNTCODE_LIST_GET"
        Dim dr As DataRow
        Dim strParam As String = "Order By ACC.AccCode|And ACC.Status = '" & _
                                objGLSetup.EnumAccountCodeStatus.Active & _
                                "' And ACC.AccType = '" & objGLSetup.EnumAccountType.BalanceSheet & "'"
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer = 0

        Try
            intErrNo = objGLSetup.mtdGetMasterList(strOpCd, _
                                                   strParam, _
                                                   objGLSetup.EnumGLMasterType.AccountCode, _
                                                   objAccDs)
        Catch Exp As Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADMIN_LOCATION_DET_ACCCODE_GET&errmesg=" & lblErrMessage.Text & "&redirect=admin/location/admin_location_locdet.aspx")
        End Try

        For intCnt = 0 To objAccDs.Tables(0).Rows.Count - 1
            objAccDs.Tables(0).Rows(intCnt).Item("AccCode") = Trim(objAccDs.Tables(0).Rows(intCnt).Item("AccCode"))
            objAccDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objAccDs.Tables(0).Rows(intCnt).Item("AccCode")) & " (" & Trim(objAccDs.Tables(0).Rows(intCnt).Item("Description")) & ")"
            If objAccDs.Tables(0).Rows(intCnt).Item("AccCode") = Trim(pv_strAccCode) Then
                intSelectedIndex = intCnt + 1
            End If
        Next

        dr = objAccDs.Tables(0).NewRow()
        dr("AccCode") = ""
        dr("Description") = lblSelect.Text & lblAccount.Text
        objAccDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlAccount.DataSource = objAccDs.Tables(0)
        ddlAccount.DataValueField = "AccCode"
        ddlAccount.DataTextField = "Description"
        ddlAccount.DataBind()
        ddlAccount.SelectedIndex = intSelectedIndex

        ddlBankAccCode.DataSource = objAccDs.Tables(0)
        ddlBankAccCode.DataValueField = "AccCode"
        ddlBankAccCode.DataTextField = "Description"
        ddlBankAccCode.DataBind()
        ddlBankAccCode.SelectedIndex = 0
    End Sub

    Sub BindCountryList(ByVal pv_strCountryCode As String)
        Dim strOpCode_Country As String = "ADMIN_CLSCOUNTRY_COUNTRY_LIST_GET"
        Dim objCountryDs As New Object
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intCountryIndex As Integer = 0
        Dim dr As DataRow

        Try
            intErrNo = objSysCountry.mtdGetCountryList(strOpCode_Country, _
                                                       objCountryDs)
        Catch Exp As Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=COMPDET_ONLOAD_COUNTRYLIST&errmesg=" & lblErrMessage.Text & "&redirect=admin/location/admin_location_locdet.aspx")
        End Try

        If objCountryDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objCountryDs.Tables(0).Rows.Count - 1
                objCountryDs.Tables(0).Rows(intCnt).Item(0) = Trim(objCountryDs.Tables(0).Rows(intCnt).Item(0))
                objCountryDs.Tables(0).Rows(intCnt).Item(1) = objCountryDs.Tables(0).Rows(intCnt).Item(0) & " (" & Trim(objCountryDs.Tables(0).Rows(intCnt).Item(1)) & ")"
                If objCountryDs.Tables(0).Rows(intCnt).Item("CountryCode") = pv_strCountryCode Then 'objLocCodeDs.Tables(0).Rows(0).Item("CountryCode") Then
                    intCountryIndex = intCnt + 1
                End If
            Next intCnt
        End If

        dr = objCountryDs.Tables(0).NewRow()
        dr("CountryCode") = ""
        dr("CountryDesc") = "Select Country"
        objCountryDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlCountry.DataSource = objCountryDs.Tables(0)
        ddlCountry.DataTextField = "CountryDesc"
        ddlCountry.DataValueField = "CountryCode"
        ddlCountry.DataBind()
        ddlCountry.SelectedIndex = intCountryIndex

        ddlBankCountry.DataSource = objCountryDs.Tables(0)
        ddlBankCountry.DataTextField = "CountryDesc"
        ddlBankCountry.DataValueField = "CountryCode"
        ddlBankCountry.DataBind()
        ddlBankCountry.SelectedIndex = 0

    End Sub

    Sub Button_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCd_Add As String = "ADMIN_CLSLOC_LOCATION_LIST_ADD"
        Dim strOpCd_Upd As String = "ADMIN_CLSLOC_LOCATION_LIST_UPD"
        Dim strOpCd_Get As String = "ADMIN_CLSLOC_LOCATION_LIST_GET"
        Dim strCmdArgs As String = CType(Sender, ImageButton).CommandArgument
        Dim strAddress As String = txtAddress.Value.Trim
        Dim strPostCode As String = txtPostCode.Text.Trim
        Dim strState As String = txtState.Text.Trim
        Dim strCity As String = txtCity.Text.Trim
        Dim strCountry As String = ddlCountry.SelectedItem.Value
        Dim strTelNo As String = txtTelNo.Text.Trim
        Dim strFaxNo As String = txtFaxNo.Text.Trim
        Dim strNPWP As String = txtNPWP.Text.Trim
        Dim strJamsostekNo As String = txtJamsostekNo.Text.Trim
        Dim strPPNRate As String = FormatNumber(txtPPNRate.Text.Trim, 0, True, False, False)
        Dim strUMR As String = txtUMR.Text.Trim
        Dim strMillCapacity As String = txtMillCapacity.Text.Trim

        Dim strAccCode As String = Request.Form("ddlAccount")
        Dim strEmpPrefix As String = txtEmpPrefix.Text.Trim
        Dim intErrNo As Integer
        Dim strDescription As String
        Dim intCloseAutoDistInd As Integer

        Dim intLocType As Integer
        Dim intLocLevel As Integer

        Dim strRDP As String = ddlRDP.SelectedItem.Value
        Dim intAnaCtrlCost As Integer
        Dim intAnaCtrlYield As Integer
        Dim strStatus As String
        Dim strParam As String = ""
        Dim strNearLoc As String = IIf(Trim(ddlNearLocCode.SelectedItem.Value) <> "", Trim(ddlNearLocCode.SelectedItem.Value), "0")

        lblErrBlank.Visible = False
        lblErrDup.Visible = False

        If strCountry = "" Then
            lblErrCountry.Visible = True
            Exit Sub
        ElseIf Len(strAddress) > 512 Then
            lblErrAddress.Visible = True
            Exit Sub
        End If

        If strRDP = "" Then
            lblErrRDP.Visible = True
            Exit Sub
        Else
            lblErrRDP.Visible = False
        End If

        strSelectedLocCode = txtLocCode.Text
        strDescription = txtDescription.Text
        If CloseAutoDistInd_Yes.Checked Then
            intCloseAutoDistInd = objAdmin.EnumCloseAutoDistInd.Yes
        Else
            intCloseAutoDistInd = objAdmin.EnumCloseAutoDistInd.No
        End If

        If LocType_Mill.Checked Then
            intLocType = objAdmin.EnumLocType.Mill
        Else
            intLocType = objAdmin.EnumLocType.Estate
        End If

        If LocLevel_Estate.Checked Then
            intLocLevel = objAdmin.EnumLocLevel.Estate
        ElseIf LocLevel_Perwakilan.Checked Then
            intLocLevel = objAdmin.EnumLocLevel.Perwakilan
        Else
            intLocLevel = objAdmin.EnumLocLevel.HQ
        End If

        If AnaCtrlCost_Block.Checked Then
            intAnaCtrlCost = objAdmin.EnumAnaCtrlCost.Block
        Else
            intAnaCtrlCost = objAdmin.EnumAnaCtrlCost.SubBlock
        End If
        If AnaCtrlYield_Block.Checked Then
            intAnaCtrlYield = objAdmin.EnumAnaCtrlYield.Block
        Else
            intAnaCtrlYield = objAdmin.EnumAnaCtrlYield.SubBlock
        End If

        Select Case Trim(lblStatus.Text)
            Case objAdmin.mtdGetLocStatus(objAdmin.EnumLocStatus.Active)
                strStatus = objAdmin.EnumLocStatus.Active
            Case objAdmin.mtdGetLocStatus(objAdmin.EnumLocStatus.Deleted)
                strStatus = objAdmin.EnumLocStatus.Deleted
        End Select

        If strCmdArgs = "Save" Then
            If loccode.Value = "" Then
                If strSelectedLocCode = "" Then
                    lblErrBlank.Visible = True
                Else
                    strParam = strSelectedLocCode & "||||" & strSortExpression & "||Add"

                    Try
                        intErrNo = objAdmin.mtdGetLocCode(strOpCd_Get, strParam, objLocCodeDs)
                    Catch Exp As Exception
                        Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADMIN_GET_LOCATION&errmesg=" & lblErrMessage.Text & "&redirect=admin/location/admin_location_locdet.aspx")
                    End Try

                    If objLocCodeDs.Tables(0).Rows.Count <> 0 Then
                        lblErrDup.Visible = True
                    Else

                        strParam = strSelectedLocCode & "|" & _
                               strDescription & "|" & _
                               strAccCode & "|" & _
                               intCloseAutoDistInd & "|" & _
                               objAdmin.EnumLocStatus.Active & "|" & _
                               intAnaCtrlCost & "|" & _
                               intAnaCtrlYield & "|" & _
                               strEmpPrefix & "|" & _
                               txtEmpCodeDigit.Text & "|" & _
                               strAddress & "|" & _
                               strPostCode & "|" & _
                               strState & "|" & _
                               strCity & "|" & _
                               strCountry & "|" & _
                               strTelNo & "|" & _
                               strFaxNo & "|" & _
                               strNPWP & "|" & _
                               strJamsostekNo & "|" & _
                               strPPNRate & "|" & _
                               strUMR & "|" & _
                               strNearLoc & "|" & _
                               intLocType & "|" & _
                               intLocLevel & "|" & _
                               strRDP & "|" & _
                               strMillCapacity
                        strParam = strParam & "|" & txtEstMgr.Text & "|" & txtKasie.Text & "||"

                        Try
                            intErrNo = objAdmin.mtdUpdLocCode(strOpCd_Add, _
                                                              strOpCd_Upd, _
                                                              strCompany, _
                                                              strLocation, _
                                                              strUserId, _
                                                              strParam, _
                                                              False)
                            If Trim(strSelectedLocCode) = Trim(strLocation) Then Session("SS_PPNRATE") = strPPNRate
                        Catch Exp As Exception
                            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADMIN_GET_LOCATION&errmesg=" & lblErrMessage.Text & "&redirect=admin/location/admin_location_locdet.aspx")
                        End Try
                    End If
                End If
            Else


                strParam = strSelectedLocCode & "|" & _
                           strDescription & "|" & _
                           strAccCode & "|" & _
                           intCloseAutoDistInd & "|" & _
                           strStatus & "|" & _
                           intAnaCtrlCost & "|" & _
                           intAnaCtrlYield & "|" & _
                           strEmpPrefix & "|" & _
                           txtEmpCodeDigit.Text & "|" & _
                           strAddress & "|" & _
                           strPostCode & "|" & _
                           strState & "|" & _
                           strCity & "|" & _
                           strCountry & "|" & _
                           strTelNo & "|" & _
                           strFaxNo & "|" & _
                           strNPWP & "|" & _
                           strJamsostekNo & "|" & _
                           strPPNRate & "|" & _
                           strUMR & "|" & _
                           strNearLoc & "|" & _
                           intLocType & "|" & _
                           intLocLevel & "|" & _
                           strRDP & "|" & _
                           strMillCapacity
                strParam = strParam & "|" & txtEstMgr.Text & "|" & txtKasie.Text & "||"


                Try
                    intErrNo = objAdmin.mtdUpdLocCode(strOpCd_Add, _
                                                      strOpCd_Upd, _
                                                      strCompany, _
                                                      strLocation, _
                                                      strUserId, _
                                                      strParam, _
                                                      True)
                    If Trim(strSelectedLocCode) = Trim(strLocation) Then Session("SS_PPNRATE") = strPPNRate
                Catch Exp As Exception
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADMIN_UPD_LOCATION&errmesg=" & lblErrMessage.Text & "&redirect=admin/location/admin_location_locdet.aspx")
                End Try
            End If

        ElseIf strCmdArgs = "Del" Then
            strParam = strSelectedLocCode & "||||" & objAdmin.EnumLocStatus.Deleted & "||||||||||||||||||||||"
            Try
                intErrNo = objAdmin.mtdUpdLocCode(strOpCd_Add, _
                                                  strOpCd_Upd, _
                                                  strCompany, _
                                                  strLocation, _
                                                  strUserId, _
                                                  strParam, _
                                                  True)
            Catch Exp As Exception
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADMIN_DEL_LOCATION&errmesg=" & lblErrMessage.Text & "&redirect=admin/location/admin_location_locdet.aspx")
            End Try

        ElseIf strCmdArgs = "UnDel" Then
            strParam = strSelectedLocCode & "||||" & objAdmin.EnumLocStatus.Active & "||||||||||||||||||||||"
            Try
                intErrNo = objAdmin.mtdUpdLocCode(strOpCd_Add, _
                                                  strOpCd_Upd, _
                                                  strCompany, _
                                                  strLocation, _
                                                  strUserId, _
                                                  strParam, _
                                                  True)
            Catch Exp As Exception
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADMIN_UNDEL_LOCATION&errmesg=" & lblErrMessage.Text & "&redirect=admin/location/admin_location_locdet.aspx")
            End Try
        End If

        If strSelectedLocCode <> "" Then
            onLoad_Display()
            BindButton()
        End If
    End Sub

    Sub BackBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("Admin_location_LocList.aspx")
    End Sub

    Sub BindNearLocList(ByVal pv_strNearLocCode As String)
        Dim strOpCode_NearLoc As String = "ADMIN_CLSLOC_NEARESTLOC_LIST_GET"
        Dim objNearLocDs As New Object
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intNearLocIndex As Integer = 0
        Dim dr As DataRow

        strParam = "||" & _
                    objAdmin.EnumNearLocStatus.Active & "||" & _
                    "NearLocCode" & "||" & "get"

        Try
            intErrNo = objAdmin.mtdGetNearLocCode(strOpCode_NearLoc, strParam, objNearLocDs)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_NEAREST_LOC&errmesg=" & lblErrMessage.Text & "&redirect=Admin/Location/Admin_Location_NearestLocList.aspx")
        End Try

        If objNearLocDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objNearLocDs.Tables(0).Rows.Count - 1
                objNearLocDs.Tables(0).Rows(intCnt).Item(0) = Trim(objNearLocDs.Tables(0).Rows(intCnt).Item(0))
                objNearLocDs.Tables(0).Rows(intCnt).Item(1) = objNearLocDs.Tables(0).Rows(intCnt).Item(0) & " (" & Trim(objNearLocDs.Tables(0).Rows(intCnt).Item(1)) & ")"
                If Trim(objNearLocDs.Tables(0).Rows(intCnt).Item("NearLocCode")) = Trim(pv_strNearLocCode) Then 'objLocCodeDs.Tables(0).Rows(0).Item("CountryCode") Then
                    intNearLocIndex = intCnt + 1
                End If
            Next intCnt
        End If

        dr = objNearLocDs.Tables(0).NewRow()
        dr("NearLocCode") = ""
        dr("Description") = "Select " & lblNearLocCode.Text & " Code"
        objNearLocDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlNearLocCode.DataSource = objNearLocDs.Tables(0)
        ddlNearLocCode.DataTextField = "Description"
        ddlNearLocCode.DataValueField = "NearLocCode"
        ddlNearLocCode.DataBind()
        ddlNearLocCode.SelectedIndex = intNearLocIndex

    End Sub

    Sub BindRDP(ByVal pv_strRDP As String)
        ddlRDP.Items.Clear()
        ddlRDP.Items.Add(New ListItem("Select Running Digits Prefix", ""))
        ddlRDP.Items.Add(New ListItem(objSysComp.mtdGetRDP(objSysComp.EnumRDP.UnitUsaha), objSysComp.EnumRDP.UnitUsaha))
        ddlRDP.Items.Add(New ListItem(objSysComp.mtdGetRDP(objSysComp.EnumRDP.Jakarta), objSysComp.EnumRDP.Jakarta))
        ddlRDP.Items.Add(New ListItem(objSysComp.mtdGetRDP(objSysComp.EnumRDP.PekanBaru), objSysComp.EnumRDP.PekanBaru))
        ddlRDP.Items.Add(New ListItem(objSysComp.mtdGetRDP(objSysComp.EnumRDP.Banjarmasin), objSysComp.EnumRDP.Banjarmasin))
        ddlRDP.Items.Add(New ListItem(objSysComp.mtdGetRDP(objSysComp.EnumRDP.BalikPapan), objSysComp.EnumRDP.BalikPapan))
        ddlRDP.Items.Add(New ListItem(objSysComp.mtdGetRDP(objSysComp.EnumRDP.UjungPandang), objSysComp.EnumRDP.UjungPandang))
    End Sub

    Sub BindBank()
        Dim objBankDs As New Object()
        Dim strParam As String = ""
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim strOpCd_Bank As String = "HR_CLSSETUP_BANK_LIST_GET"
        Dim intBankIndex As Integer = 0
        Dim dr As DataRow

        strParam = "|"
        Try
            intErrNo = objHR.mtdGetMasterList(strOpCd_Bank, strParam, objHR.EnumHRMasterType.Bank, objBankDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PU_ONLOAD_BANKLIST&errmesg=" & lblErrMessage.Text & "&redirect=pu/setup/PU_setup_SuppList.aspx")
        End Try

        If objBankDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objBankDs.Tables(0).Rows.Count - 1
                objBankDs.Tables(0).Rows(intCnt).Item(0) = Trim(objBankDs.Tables(0).Rows(intCnt).Item(0))
                objBankDs.Tables(0).Rows(intCnt).Item(1) = objBankDs.Tables(0).Rows(intCnt).Item(0) & " (" & Trim(objBankDs.Tables(0).Rows(intCnt).Item(1)) & ")"
                'If objBankDs.Tables(0).Rows(intCnt).Item("BankCode") = objSuppDs.Tables(0).Rows(0).Item("BankCode") Then
                '    intBankIndex = intCnt + 1
                'End If
            Next intCnt
        End If

        dr = objBankDs.Tables(0).NewRow()
        dr("BankCode") = ""
        dr("Description") = "Select Bank Code"
        objBankDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlBankCode.DataSource = objBankDs.Tables(0)
        ddlBankCode.DataTextField = "Description"
        ddlBankCode.DataValueField = "BankCode"
        ddlBankCode.DataBind()
        ddlBankCode.SelectedIndex = intBankIndex
    End Sub

    Sub AddBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCd_Upd As String = "ADMIN_CLSLOC_LOCATION_BANK_UPDATE"
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer
        Dim objEmp As New Object()

        If txtLocCode.Text = "" Then
            lblErrDup.Visible = True
            lblErrDup.Text = "Please save location data first."
            Exit Sub
        End If
        If ddlBankCode.SelectedItem.Value = "" Then
            lblerrBankCode.Visible = True
            Exit Sub
        End If
        If txtBankAccName.Text = "" Then
            lblErrBankAccName.Visible = True
            Exit Sub
        End If
        'If txtBankAccNo.Text = "" Then
        '    lblerrBankAccNo.Visible = True
        '    Exit Sub
        'End If
        If ddlBankAccCode.SelectedItem.Value = "" Then
            lblErrBankAccCode.Visible = True
            Exit Sub
        End If

        strParamName = "LOCCODE|BANKCODE|BANKACCNAME|BANKACCNO|BANKADDRESS|BANKTOWN|BANKSTATE|BANKCOUNTRY|BANKREMARK|STATUS|ACCCODE"
        strParamValue = txtLocCode.Text & "|" & Trim(ddlBankCode.SelectedItem.Value) & "|" & UCase(Trim(txtBankAccName.Text)) & _
                       "|" & Trim(txtBankAccNo.Text) & "|" & Trim(txtBankAddress.Text) & "|" & Trim(txtBankTown.Text) & "|" & Trim(txtBankState.Text) & _
                       "|" & IIf(ddlBankCountry.SelectedItem.Value = "", "ID", Trim(ddlBankCountry.SelectedItem.Value)) & "|" & Trim(txtBankRemark.Text) & "|" & objAdmin.EnumLocStatus.Active & "|" & _
                       ddlBankAccCode.SelectedItem.Value

        Try
            intErrNo = objGLTrx.mtdInsertDataCommon(strOpCd_Upd, strParamName, strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=TX_Setup_TaxObjectDet&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        hidBankAccNo.Value = ""
        onLoad_DisplayLn(txtLocCode.Text)
    End Sub

    Sub onLoad_DisplayLn(ByVal pv_strSplCode As String)
        Dim strOpCd As String = "ADMIN_CLSLOC_LOCATION_BANK_GET"
        Dim intErrNo As Integer
        Dim strParamName As String
        Dim strParamValue As String
        Dim intCnt As Integer = 0
        Dim DelButton As LinkButton
        Dim EdtButton As LinkButton
        Dim CanButton As LinkButton
        Dim objTaxDs As New Object

        txtLocCode.Text = IIf(pv_strSplCode = "", "", pv_strSplCode)
        strParamName = "LOCCODE"
        strParamValue = txtLocCode.Text

        Try
            intErrNo = objGLTrx.mtdGetDataCommon(strOpCd, _
                                                strParamName, _
                                                strParamValue, _
                                                objTaxDs)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=TX_Setup_TaxObjectDet&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        dgLineDet.DataSource = objTaxDs.Tables(0)
        dgLineDet.DataBind()

        If objTaxDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objTaxDs.Tables(0).Rows.Count - 1
                EdtButton = dgLineDet.Items.Item(intCnt).FindControl("lbEdit")
                DelButton = dgLineDet.Items.Item(intCnt).FindControl("lbDelete")
                CanButton = dgLineDet.Items.Item(intCnt).FindControl("lbCancel")
                DelButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"

                Select Case Trim(lblStatus.Text)
                    Case objAdmin.mtdGetLocStatus(objAdmin.EnumLocStatus.Active)
                        If hidBankAccNo.Value = "" Then
                            EdtButton.Visible = True
                            DelButton.Visible = True
                            CanButton.Visible = False
                        Else
                            EdtButton.Visible = False
                            DelButton.Visible = False
                            CanButton.Visible = True
                        End If
                    Case objAdmin.mtdGetLocStatus(objAdmin.EnumLocStatus.Deleted)
                        EdtButton.Visible = False
                        DelButton.Visible = False
                        CanButton.Visible = False
                End Select
            Next intCnt
        End If
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim lbl As Label
        Dim strOpCd_Upd As String = "ADMIN_CLSLOC_LOCATION_BANK_UPDATE"
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer
        Dim lbBankCode As String
        Dim lbBankAccName As String
        Dim lbBankAddress As String
        Dim lbBankTown As String
        Dim lbBankState As String
        Dim lbBankCountry As String

        dgLineDet.EditItemIndex = CInt(E.Item.ItemIndex)

        lbl = E.Item.FindControl("lblBankCode")
        lbBankCode = lbl.Text.Trim
        lbl = E.Item.FindControl("lblBankAccNo")
        hidBankAccNo.Value = lbl.Text.Trim
        lbl = E.Item.FindControl("lblBankAccName")
        lbBankAccName = lbl.Text.Trim
        lbl = E.Item.FindControl("lblBankAddress")
        lbBankAddress = lbl.Text.Trim
        lbl = E.Item.FindControl("lblBankTown")
        lbBankTown = lbl.Text.Trim
        lbl = E.Item.FindControl("lblBankState")
        lbBankState = lbl.Text.Trim
        lbl = E.Item.FindControl("lblBankCountry")
        lbBankCountry = lbl.Text.Trim

        strParamName = "LOCCODE|BANKCODE|BANKACCNAME|BANKACCNO|BANKADDRESS|BANKTOWN|BANKSTATE|BANKCOUNTRY|BANKREMARK|STATUS|ACCCODE"
        strParamValue = txtLocCode.Text & "|" & Trim(lbBankCode) & "|" & Trim(lbBankAccName) & "|" & Trim(hidBankAccNo.Value) & _
                        "|" & Trim(lbBankAddress) & "|" & Trim(lbBankTown) & "|" & Trim(lbBankState) & _
                        "|" & Trim(lbBankCountry) & "|" & Trim(txtBankRemark.Text) & "|" & _
                        objAdmin.EnumLocStatus.Deleted & "|" & _
                        ddlBankAccCode.SelectedItem.Value

        Try
            intErrNo = objGLTrx.mtdInsertDataCommon(strOpCd_Upd, strParamName, strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=TX_Setup_TaxObjectDet&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        onLoad_DisplayLn(txtLocCode.Text)
    End Sub

    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        hidBankAccNo.Value = ""
        'onLoad_Display()
        onLoad_DisplayLn(txtLocCode.Text)
    End Sub

    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strOpCd_Bank As String = "HR_CLSSETUP_BANK_LIST_GET"
        Dim strOpCd_Country As String = "ADMIN_CLSCOUNTRY_COUNTRY_LIST_GET"
        Dim lbl As Label
        Dim btn As LinkButton
        Dim strParam As String = ""
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim dr As DataRow
        Dim intBankIndex As Integer = 0
        Dim intBankCountryIndex As Integer = 0
        Dim lbBankCode As String

        dgLineDet.EditItemIndex = CInt(E.Item.ItemIndex)

        lbl = E.Item.FindControl("lblBankCode")
        ddlBankCode.SelectedValue = lbl.Text.Trim
        lbl = E.Item.FindControl("lblBankCountry")
        ddlBankCountry.SelectedValue = lbl.Text.Trim

        lbl = E.Item.FindControl("lblBankAccNo")
        txtBankAccNo.Text = lbl.Text.Trim
        hidBankAccNo.Value = lbl.Text.Trim
        lbl = E.Item.FindControl("lblBankAccName")
        txtBankAccName.Text = lbl.Text.Trim
        lbl = E.Item.FindControl("lblBankAddress")
        txtBankAddress.Text = lbl.Text.Trim
        lbl = E.Item.FindControl("lblBankTown")
        txtBankTown.Text = lbl.Text.Trim
        lbl = E.Item.FindControl("lblBankState")
        txtBankState.Text = lbl.Text.Trim
        lbl = E.Item.FindControl("lblBankRemark")
        txtBankState.Text = lbl.Text.Trim
        lbl = E.Item.FindControl("lblBankAccCode")
        ddlBankAccCode.SelectedValue = lbl.Text.Trim

        btn = E.Item.FindControl("lbDelete")
        btn.Visible = False
        btn = E.Item.FindControl("lbEdit")
        btn.Visible = False
        btn = E.Item.FindControl("lbCancel")
        btn.Visible = True

        'txtBankAccNo.Enabled = False
        ddlBankCode.Enabled = False
        ddlBankAccCode.Enabled = False
    End Sub
End Class
