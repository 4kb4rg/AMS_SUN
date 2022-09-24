
Imports System
Imports System.Data
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic.Interaction
Imports Microsoft.VisualBasic

Imports agri.Admin.clsComp
Imports agri.Admin.clsLoc
Imports agri.Admin.clsCountry
Imports agri.GlobalHdl.clsGlobalHdl
Imports agri.PWSystem.clsLangCap

Public Class admin_comp_det : Inherits Page

    Protected WithEvents dgLocation As DataGrid
    Protected WithEvents txtCompCode As TextBox
    Protected WithEvents txtCompName As TextBox
    Protected WithEvents tblSelection As HtmlTable

    Protected WithEvents txtPostCode As TextBox
    Protected WithEvents txtTelNo As TextBox
    Protected WithEvents txtState As TextBox
    Protected WithEvents txtFaxNo As TextBox
    Protected WithEvents txtNPWP As TextBox
    Protected WithEvents txtCity As TextBox
    Protected WithEvents txtAddress As HtmlTextArea
    Protected WithEvents hidCompCode As HtmlInputHidden

    Protected WithEvents lblErrDelete As Label
    Protected WithEvents lblStatus As Label
    Protected WithEvents lblDateCreated As Label
    Protected WithEvents lblLastUpdate As Label
    Protected WithEvents lblUpdateBy As Label
    Protected WithEvents lblHiddenStatus As Label
    Protected WithEvents lblErrCompCode As Label
    Protected WithEvents lblErrAddress As Label
    Protected WithEvents lblErrLoc As Label
    Protected WithEvents btnAddLoc As ImageButton
    Protected WithEvents btnSave As ImageButton
    Protected WithEvents btnDelete As ImageButton
    Protected WithEvents btnUnDelete As ImageButton
    Protected WithEvents btnBack As ImageButton
    Protected WithEvents ddlCountry As DropDownList
    Protected WithEvents ddlLocation As DropDownList
    Protected WithEvents lblErrMesage As Label
    Protected WithEvents lblErrCountry As Label
    Protected WithEvents lblSelect As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents lblErrSelect As Label
    Protected WithEvents lblErrEnter As Label
    Protected WithEvents lblTitle As Label
    Protected WithEvents lblCompany As Label
    Protected WithEvents lblCompName As Label
    Protected WithEvents lblLoc1 As Label
    Protected WithEvents lblLoc2 As Label
    Protected WithEvents validateCompCode As RequiredFieldValidator
    Protected WithEvents validateCompName As RequiredFieldValidator

    Protected WithEvents ddlRDP As DropDownList
    Protected WithEvents lblErrRDP As Label

    Dim objSysComp As New agri.Admin.clsComp()
    Dim objSysLoc As New agri.Admin.clsLoc()
    Dim objSysCountry As New agri.Admin.clsCountry()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    
    Dim objCompDs As New Object()
    Dim objLocDs As New Object()
    Dim objCompLocDs As New Object()
    Dim objCountryDs As New Object()
    Dim objLangCapDs As New Object()
    Dim objAdminLoc As New agri.Admin.clsLoc()


    Dim objRDPDs As New Object()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim intADAR As Integer

    Dim strSelectedCo As String = ""
    Dim strLocType as String


    Sub Page_Load(Sender As Object, E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        intADAR = Session("SS_ADAR")
        
        strLocType = Session("SS_LOCTYPE")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumADAccessRights.ADCompany), intADAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            onload_GetLangCap()
            lblErrDelete.Visible = False
            lblErrCountry.Visible = False
            lblErrLoc.Visible = False
            strSelectedCo = Trim(IIf(Request.QueryString("compcode") <> "", Request.QueryString("compcode"), Request.Form("txtCompCode")))

            If Not IsPostBack Then
                If strSelectedCo <> "" Then
                    onLoad_Display()
                    onLoad_BindButton()
                Else
                    onLoad_NewDisplay()
                    onLoad_BindButton()
                End If
            End If
        End If
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        
        lblCompany.text = GetCaption(objLangCap.EnumLangCap.Company)
        lblTitle.text = UCase(lblCompany.text)
        lblCompName.text = GetCaption(objLangCap.EnumLangCap.CompName)
        lblLoc1.text = GetCaption(objLangCap.EnumLangCap.Location)
        lblLoc2.text = GetCaption(objLangCap.EnumLangCap.Location)
        validateCompCode.ErrorMessage = lblErrEnter.text & lblCompany.text & lblCode.text & "."
        validateCompName.ErrorMessage = lblErrEnter.text & lblCompName.text & "."
        lblErrLoc.text = lblErrSelect.text & lblLoc1.text
        dgLocation.Columns(0).headertext = lblLoc1.text & lblCode.text
        dgLocation.Columns(1).headertext = GetCaption(objLangCap.EnumLangCap.LocDesc)
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
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=PU_GET_LANGCAP&errmesg=" & lblErrMesage.Text & "&redirect=admin/company/admin_comp_det.aspx")
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


    Sub onLoad_BindButton()
        txtCompName.Enabled = False
        txtPostCode.Enabled = False
        txtTelNo.Enabled = False
        txtState.Enabled = False
        ddlCountry.Enabled = False
        txtFaxNo.Enabled = False
        txtCity.Enabled = False
        txtAddress.Disabled = True
        txtNPWP.Enabled = False
        tblSelection.Visible = False
        btnSave.Visible = False
        btnDelete.Visible = False
        btnUnDelete.Visible = False
        ddlRDP.visible = false 
        lblErrRDP.visible = false
        Select Case CInt(lblHiddenStatus.Text)
            Case objSysComp.EnumCompanyStatus.Active
                txtCompName.Enabled = True
                txtPostCode.Enabled = True
                txtTelNo.Enabled = True
                txtState.Enabled = True
                ddlCountry.Enabled = True
                txtFaxNo.Enabled = True
                txtCity.Enabled = True
                txtAddress.Disabled = False
                txtNPWP.Enabled = True
                btnSave.Visible = True
                btnDelete.Visible = True
                btnDelete.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                tblSelection.Visible = True
                ddlRDP.visible = false 
                lblErrRDP.visible = false
            Case objSysComp.EnumCompanyStatus.Deleted
                btnUnDelete.Visible = True
                ddlRDP.visible = false 
                lblErrRDP.visible = false
                btnUnDelete.Attributes("onclick") = "javascript:return ConfirmAction('undelete');"
            Case Else
                txtCompName.Enabled = True
                txtPostCode.Enabled = True
                txtTelNo.Enabled = True
                txtState.Enabled = True
                ddlCountry.Enabled = True
                txtFaxNo.Enabled = True
                txtCity.Enabled = True
                txtAddress.Disabled = False
                txtNPWP.Enabled = True
                btnSave.Visible = True
                tblSelection.Visible = True
                ddlRDP.visible = false 
                lblErrRDP.visible = false
        End Select
    End Sub

    Sub onLoad_Display()
        Dim strOpCode_Comp As String = "ADMIN_CLSCOMP_COMPANY_DETAILS_GET"
        Dim strOpCode_Country As String = "ADMIN_CLSCOUNTRY_COUNTRY_LIST_GET"
        Dim strOpCode_Loc As String = "ADMIN_CLSLOC_COMPANY_LOCATION_LIST_GET"
        Dim strOpCode_CompLoc As String = "ADMIN_CLSCOMP_COMPANY_LOCATION_LIST_GET"
        Dim blnCompDetails As Boolean = True
        Dim strParam As String = ""
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intCountryIndex As Integer = 0

        Dim intRDPindex as integer = 0

        Dim lbButton As LinkButton
        Dim dr As DataRow

        txtCompCode.Text = strSelectedCo
        hidCompCode.Value = strSelectedCo

        strParam = strSelectedCo
        Try
            intErrNo = objSysComp.mtdGetComp(strOpCode_Comp, strParam, objCompDs, blnCompDetails)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=COMPDET_ONLOAD_COMPDETAILS&errmesg=" & lblErrMesage.Text & "&redirect=admin/company/admin_comp_list.aspx")
        End Try

        If objCompDs.Tables(0).Rows.Count > 0 Then
            objCompDs.Tables(0).Rows(0).Item("CompName") = Trim(objCompDs.Tables(0).Rows(0).Item("CompName"))
            objCompDs.Tables(0).Rows(0).Item("Address") = Trim(objCompDs.Tables(0).Rows(0).Item("Address"))
            objCompDs.Tables(0).Rows(0).Item("PostCode") = Trim(objCompDs.Tables(0).Rows(0).Item("PostCode"))
            objCompDs.Tables(0).Rows(0).Item("State") = Trim(objCompDs.Tables(0).Rows(0).Item("State"))
            objCompDs.Tables(0).Rows(0).Item("City") = Trim(objCompDs.Tables(0).Rows(0).Item("City"))
            objCompDs.Tables(0).Rows(0).Item("CountryCode") = Trim(objCompDs.Tables(0).Rows(0).Item("CountryCode"))
            objCompDs.Tables(0).Rows(0).Item("TelNo") = Trim(objCompDs.Tables(0).Rows(0).Item("TelNo"))
            objCompDs.Tables(0).Rows(0).Item("FaxNo") = Trim(objCompDs.Tables(0).Rows(0).Item("FaxNo"))
            objCompDs.Tables(0).Rows(0).Item("CreateDate") = objGlobal.GetLongDate(Trim(objCompDs.Tables(0).Rows(0).Item("CreateDate")))
            objCompDs.Tables(0).Rows(0).Item("UpdateDate") = objGlobal.GetLongDate(Trim(objCompDs.Tables(0).Rows(0).Item("UpdateDate")))
            objCompDs.Tables(0).Rows(0).Item("Status") = Trim(objCompDs.Tables(0).Rows(0).Item("Status"))
            objCompDs.Tables(0).Rows(0).Item("UpdateBy") = Trim(objCompDs.Tables(0).Rows(0).Item("UpdateBy"))
            objCompDs.Tables(0).Rows(0).Item("NPWP") = Trim(objCompDs.Tables(0).Rows(0).Item("NPWP"))
            objCompDs.Tables(0).Rows(0).Item("RDP") = Trim(objCompDs.Tables(0).Rows(0).Item("RDP"))
        End If

        Try
            intErrNo = objSysCountry.mtdGetCountryList(strOpCode_Country, _
                                                       objCountryDs)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=COMPDET_ONLOAD_COUNTRYLIST&errmesg=" & lblErrMesage.Text & "&redirect=admin/company/admin_comp_list.aspx")
        End Try

        If objCountryDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objCountryDs.Tables(0).Rows.Count - 1
                objCountryDs.Tables(0).Rows(intCnt).Item(0) = Trim(objCountryDs.Tables(0).Rows(intCnt).Item(0))
                objCountryDs.Tables(0).Rows(intCnt).Item(1) = objCountryDs.Tables(0).Rows(intCnt).Item(0) & " (" & Trim(objCountryDs.Tables(0).Rows(intCnt).Item(1)) & ")"
                If objCountryDs.Tables(0).Rows(intCnt).Item("CountryCode") = objCompDs.Tables(0).Rows(0).Item("CountryCode") Then
                    intCountryIndex = intCnt + 1
                End If
            Next intCnt
        End If

        Try
            strParam = objSysLoc.EnumLocStatus.Active & "|" & strSelectedCo & "|"
            intErrNo = objSysLoc.mtdGetLocList(strOpCode_Loc, _
                                               strCompany, _
                                               strLocation, _
                                               strUserId, _
                                               objLocDs, _
                                               strParam)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=COMPDET_ONLOAD_LOCLIST&errmesg=" & lblErrMesage.Text & "&redirect=admin/company/admin_comp_list.aspx")
        End Try

        If objLocDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objLocDs.Tables(0).Rows.Count - 1
                objLocDs.Tables(0).Rows(intCnt).Item(0) = Trim(objLocDs.Tables(0).Rows(intCnt).Item(0))
                objLocDs.Tables(0).Rows(intCnt).Item(1) = objLocDs.Tables(0).Rows(intCnt).Item(0) & " (" & Trim(objLocDs.Tables(0).Rows(intCnt).Item(1)) & ")"
            Next intCnt
        End If


        Try
            strParam = strSelectedCo
            intErrNo = objSysComp.mtdGetCompLocList(strOpCode_CompLoc, _
                                                    objCompLocDs, _
                                                    strParam)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=COMPDET_ONLOAD_COMPLOCLIST&errmesg=" & lblErrMesage.Text & "&redirect=admin/company/admin_comp_list.aspx")
        End Try

        If objCompLocDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objCompLocDs.Tables(0).Rows.Count - 1
                objCompLocDs.Tables(0).Rows(intCnt).Item(0) = Trim(objCompLocDs.Tables(0).Rows(intCnt).Item(0))
                objCompLocDs.Tables(0).Rows(intCnt).Item(1) = Trim(objCompLocDs.Tables(0).Rows(intCnt).Item(1))
            Next intCnt
        End If



        txtCompCode.ReadOnly = True
        txtCompCode.Text = strSelectedCo
        txtCompName.Text = objCompDs.Tables(0).Rows(0).Item("CompName")
        txtPostCode.Text = objCompDs.Tables(0).Rows(0).Item("PostCode")
        txtTelNo.Text = objCompDs.Tables(0).Rows(0).Item("TelNo")
        txtState.Text = objCompDs.Tables(0).Rows(0).Item("State")
        txtFaxNo.Text = objCompDs.Tables(0).Rows(0).Item("FaxNo")
        txtCity.Text = objCompDs.Tables(0).Rows(0).Item("City")
        txtAddress.Value = objCompDs.Tables(0).Rows(0).Item("Address")
        txtNPWP.Text = objCompDs.Tables(0).Rows(0).Item("NPWP")
        lblStatus.Text = objSysComp.mtdGetCompanyStatus(objCompDs.Tables(0).Rows(0).Item("Status"))
        lblHiddenStatus.Text = Trim(objCompDs.Tables(0).Rows(0).Item("Status"))
        lblDateCreated.Text = objGlobal.GetLongDate(objCompDs.Tables(0).Rows(0).Item("CreateDate"))
        lblLastUpdate.Text = objGlobal.GetLongDate(objCompDs.Tables(0).Rows(0).Item("UpdateDate"))
        lblUpdateBy.Text = objCompDs.Tables(0).Rows(0).Item("UpdateBy")

        dr = objCountryDs.Tables(0).NewRow()
        dr("CountryCode") = ""
        dr("CountryDesc") = "Select Country"
        objCountryDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlCountry.DataSource = objCountryDs.Tables(0)
        ddlCountry.DataTextField = "CountryDesc"
        ddlCountry.DataValueField = "CountryCode"
        ddlCountry.DataBind()
        ddlCountry.SelectedIndex = intCountryIndex

        dr = objLocDs.Tables(0).NewRow()
        dr("LocCode") = ""
        dr("Description") = lblSelect.Text & lblLoc1.Text
        objLocDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlLocation.DataSource = objLocDs.Tables(0)
        ddlLocation.DataTextField = "Description"
        ddlLocation.DataValueField = "LocCode"
        ddlLocation.DataBind()

        'ddlRDP.SelectedIndex = intRDPindex


        dgLocation.DataSource = objCompLocDs.Tables(0)
        dgLocation.DataBind()

        For intCnt = 0 To dgLocation.Items.Count - 1
            Select Case CInt(objCompDs.Tables(0).Rows(0).Item("Status"))
                Case objSysComp.EnumCompanyStatus.Active
                    lbButton = dgLocation.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = True
                    lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                Case objSysComp.EnumCompanyStatus.Deleted
                    lbButton = dgLocation.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False
            End Select
        Next

    End Sub


    Sub onLoad_NewDisplay()
        Dim strOpCode_Country As String = "ADMIN_CLSCOUNTRY_COUNTRY_LIST_GET"
        Dim strOpCode_Loc As String = "ADMIN_CLSLOC_COMPANY_LOCATION_LIST_GET"
        Dim strParam As String = strSelectedCo
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim dr As DataRow

        Try
            intErrNo = objSysCountry.mtdGetCountryList(strOpCode_Country, _
                                                       objCountryDs)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=COMPDET_GET_COUNTRYLIST&errmesg=" & lblErrMesage.Text & "&redirect=admin/company/admin_comp_list.aspx")
        End Try

        If objCountryDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objCountryDs.Tables(0).Rows.Count - 1
                objCountryDs.Tables(0).Rows(intCnt).Item(0) = Trim(objCountryDs.Tables(0).Rows(intCnt).Item(0))
                objCountryDs.Tables(0).Rows(intCnt).Item(1) = objCountryDs.Tables(0).Rows(intCnt).Item(0) & " (" & Trim(objCountryDs.Tables(0).Rows(intCnt).Item(1)) & ")"
            Next intCnt
        End If


        Try
            strParam = objSysLoc.EnumLocStatus.Active & "|" & strSelectedCo & "|"
            intErrNo = objSysLoc.mtdGetLocList(strOpCode_Loc, _
                                               strCompany, _
                                               strLocation, _
                                               strUserId, _
                                               objLocDs, _
                                               strParam)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=COMPDET_GET_LOCLIST&errmesg=" & lblErrMesage.Text & "&redirect=admin/company/admin_comp_list.aspx")
        End Try

        If objLocDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objLocDs.Tables(0).Rows.Count - 1
                objLocDs.Tables(0).Rows(intCnt).Item(0) = Trim(objLocDs.Tables(0).Rows(intCnt).Item(0))
                objLocDs.Tables(0).Rows(intCnt).Item(1) = objLocDs.Tables(0).Rows(intCnt).Item(0) & " (" & Trim(objLocDs.Tables(0).Rows(intCnt).Item(1)) & ")"
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

        dr = objLocDs.Tables(0).NewRow()
        dr("LocCode") = ""
        dr("Description") = "Select Location"
        objLocDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlLocation.DataSource = objLocDs.Tables(0)
        ddlLocation.DataTextField = "Description"
        ddlLocation.DataValueField = "LocCode"
        ddlLocation.DataBind()



    End Sub


    Sub btnSave_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCode_UpdComp As String = "ADMIN_CLSCOMP_COMPANY_DETAILS_UPD"
        Dim strOpCode_AddComp As String = "ADMIN_CLSCOMP_COMPANY_DETAILS_ADD"
        Dim strOpCode_GetComp As String = "ADMIN_CLSCOMP_COMPANY_DETAILS_GET"
        Dim _objCompDs As New Object
        Dim blnIsDetail As Boolean = True
        Dim intErrNo As Integer
        Dim strCompParam As String
        Dim strCompName As String = Trim(txtCompName.Text)
        Dim strPostCode As String = Trim(txtPostCode.Text)
        Dim strTelNo As String = Trim(txtTelNo.Text)
        Dim strState As String = Trim(txtState.Text)
        Dim strFaxNo As String = Trim(txtFaxNo.Text)
        Dim strCity As String = Trim(txtCity.Text)
        Dim strAddress As String = Trim(txtAddress.Value)
        Dim strCountry As String = ddlCountry.SelectedItem.Value
        Dim strNPWP As String = Trim(txtNPWP.Text)

        Dim strStatus As String = "1"
        Dim strRDP As String = "0"


        If strCountry = "" Then
            lblErrCountry.Visible = True
            Exit Sub
        ElseIf Len(strAddress) > 512 Then
            lblErrAddress.Visible = True
            Exit Sub
        End If


        strCompParam = strSelectedCo & "|" & _
                        strCompName & "|" & _
                        strAddress & "|" & _
                        strPostCode & "|" & _
                        strState & "|" & _
                        strCity & "|" & _
                        strCountry & "|" & _
                        strTelNo & "|" & _
                        strFaxNo & "|" & _
                        strNPWP & "|" & _
                        strStatus & "|" & _
                        strRDP


        If Request.Form("hidCompCode") <> "" Then
            Try
                intErrNo = objSysComp.mtdUpdComp(strOpCode_UpdComp, _
                                                strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strCompParam, _
                                                objSysComp.EnumCompUpdType.Update)
            Catch Exp As System.Exception
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=COMPDET_SAVEBTN_COMP_UPD&errmesg=" & lblErrMesage.Text & "&redirect=admin/company/admin_comp_det.aspx?compcode=" & strSelectedCo)
            End Try

        Else
            Try
                intErrNo = objSysComp.mtdGetComp(strOpCode_GetComp, _
                                                strSelectedCo, _
                                                _objCompDs, _
                                                blnIsDetail)
            Catch Exp As System.Exception
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=COMPDET_SAVEBTN_COMP_GET&errmesg=" & lblErrMesage.Text & "&redirect=admin/company/admin_comp_det.aspx?compcode=" & strSelectedCo)
            End Try

            If _objCompDs.Tables(0).Rows.Count = 0 Then
                Try
                    intErrNo = objSysComp.mtdUpdComp(strOpCode_AddComp, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strCompParam, _
                                                    objSysComp.EnumCompUpdType.Add)
                Catch Exp As System.Exception
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=COMPDET_SAVEBTN_COMP_ADD&errmesg=" & lblErrMesage.Text & "&redirect=admin/company/admin_comp_det.aspx?compcode=" & strSelectedCo)
                End Try

            Else
                lblErrCompCode.Visible = True
            End If
        End If

        onLoad_Display()
        onLoad_BindButton()
    End Sub


    Sub DEDR_DeleteLoc(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim objCompSysLocDs As New DataSet
        Dim strOpCode_CompSysLoc As String = "ADMIN_CLSLOC_SYSLOC_LIST_GET"
        Dim strOpCode_CompLoc As String = "ADMIN_CLSCOMP_COMPANY_LOCATION_DEL"
        Dim strOpCode_Comp As String = "ADMIN_CLSCOMP_COMPANY_DETAILS_UPD"
        Dim strCompLocParam As String = ""
        Dim LocCodeCell As TableCell = E.Item.Cells(0)
        Dim intErrNo As Integer
        Dim strParam As String

        Try
            strParam = " AND SY.CompCode = '" & strSelectedCo & "' AND SY.LocCode = '" & LocCodeCell.Text & "'"
            intErrNo = objSysLoc.mtdGetCOMPLocList(strOpCode_CompSysLoc, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    objCompSysLocDs, _
                                                    strParam)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=COMPDET_DELETEBRN_GET_SYSLOC&errmesg=" & lblErrMesage.Text & "&redirect=admin/company/admin_comp_det.aspx?compcode=" & strSelectedCo)
        End Try

        If objCompSysLocDs.Tables(0).Rows.Count = 0 Then
            strCompLocParam = strSelectedCo & "|" & LocCodeCell.Text
            Try
                intErrNo = objSysComp.mtdUpdCompLoc(strOpCode_CompLoc, _
                                                    strOpCode_Comp, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strCompLocParam)
            Catch Exp As System.Exception
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=COMPDET_DELETEBTN_COMPLOC_UPD&errmesg=" & lblErrMesage.Text & "&redirect=admin/company/admin_comp_det.aspx?compcode=" & strSelectedCo)
            End Try
        Else
            lblErrDelete.Visible = True
        End If

        onLoad_Display()
        onLoad_BindButton()
    End Sub


    Sub btnAddLoc_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCode_CompLoc As String = "ADMIN_CLSCOMP_COMPANY_LOCATION_ADD"
        Dim strOpCode_Comp As String = "ADMIN_CLSCOMP_COMPANY_DETAILS_UPD"
        Dim strCompLocParam As String = ""
        Dim strSelectedLoc As String = ddlLocation.SelectedItem.Value
        Dim intErrNo As Integer

        If strSelectedLoc = "" Then
            lblErrLoc.Visible = True
            Exit Sub
        ElseIf hidCompCode.Value = "" Then
            btnSave_Click(Sender, E)
        End If

        strCompLocParam = strSelectedCo & "|" & ddlLocation.SelectedItem.Value

        Try
            intErrNo = objSysComp.mtdUpdCompLoc(strOpCode_CompLoc, _
                                                strOpCode_Comp, _
                                                strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strCompLocParam)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=COMPDET_ADDBTN_COMPLOC_UPD&errmesg=" & lblErrMesage.Text & "&redirect=admin/company/admin_comp_det.aspx?compcode=" & strSelectedCo)
        End Try

        onLoad_Display()
        onLoad_BindButton()
    End Sub


    Sub btnDelete_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCode_UpdComp As String = "ADMIN_CLSCOMP_COMPANY_DETAILS_UPD"
        Dim strCompParam As String = ""
        Dim intErrNo As Integer

        strCompParam = strSelectedCo & "||||||||||" & objSysComp.EnumCompanyStatus.Deleted

        Try
            intErrNo = objSysComp.mtdUpdComp(strOpCode_UpdComp, _
                                             strCompany, _
                                             strLocation, _
                                             strUserId, _
                                             strCompParam, _
                                             objSysComp.EnumCompUpdType.Update)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=COMPDET_DELETEBTN_COMP_UPD&errmesg=" & lblErrMesage.Text & "&redirect=admin/company/admin_comp_det.aspx?compcode=" & strSelectedCo)
        End Try

        onLoad_Display()
        onLoad_BindButton()
    End Sub


    Sub btnUnDelete_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCode_UpdComp As String = "ADMIN_CLSCOMP_COMPANY_DETAILS_UPD"
        Dim strCompParam As String = ""
        Dim intErrNo As Integer

        strCompParam = strSelectedCo & "||||||||||" & objSysComp.EnumCompanyStatus.Active

        Try
            intErrNo = objSysComp.mtdUpdComp(strOpCode_UpdComp, _
                                             strCompany, _
                                             strLocation, _
                                             strUserId, _
                                             strCompParam, _
                                             objSysComp.EnumCompUpdType.Update)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=COMPDET_UNDELETEBTN_COMP_UPD&errmesg=" & lblErrMesage.Text & "&redirect=admin/company/admin_comp_det.aspx?compcode=" & strSelectedCo)
        End Try

        onLoad_Display()
        onLoad_BindButton()
    End Sub


    Sub btnBack_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("admin_comp_list.aspx")
    End Sub

End Class
