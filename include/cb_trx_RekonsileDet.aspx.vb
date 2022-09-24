
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


Public Class cb_trx_RekonsileDet : Inherits Page

    Protected WithEvents lblErrMessage As Label

    Protected WithEvents lblRekonsileID As Label
    Protected WithEvents lblAmountHidden As Label
    Protected WithEvents lstAccMonth As DropDownList
    Protected WithEvents lstAccYear As DropDownList
    Protected WithEvents lblLastUpdate As Label
    Protected WithEvents lblUpdatedBy As Label
    Protected WithEvents ddlBank As DropDownList
    Protected WithEvents lblErrBank As Label
    Protected WithEvents lblDateCreated As Label
    Protected WithEvents txtFromDate As TextBox
    Protected WithEvents btnSelDateFrom As Image
    Protected WithEvents lblErrFromDate As Label
    Protected WithEvents txtToDate As TextBox
    Protected WithEvents txtSaldoBank As TextBox

    Protected WithEvents btnSelDateTo As Image
    Protected WithEvents lblErrToDate As Label
    Protected WithEvents lblTotAmt As Label
    Protected WithEvents lblSaldoAwal As Label
    Protected WithEvents RefreshBtn As ImageButton
    Protected WithEvents inrid As HtmlInputHidden

    Protected WithEvents dgLineDet As DataGrid


    Protected WithEvents txtAddDate1 As TextBox
    Protected WithEvents btnAddDate1 As Image
    Protected WithEvents lblErrAddDate1 As Label
    Protected WithEvents txtAddDescr1 As TextBox
    Protected WithEvents txtAddAmount1 As TextBox

    Protected WithEvents txtAddDate2 As TextBox
    Protected WithEvents btnAddDate2 As Image
    Protected WithEvents lblErrAddDate2 As Label
    Protected WithEvents txtAddDescr2 As TextBox
    Protected WithEvents txtAddAmount2 As TextBox

    Protected WithEvents txtAddDate3 As TextBox
    Protected WithEvents btnAddDate3 As Image
    Protected WithEvents lblErrAddDate3 As Label
    Protected WithEvents txtAddDescr3 As TextBox
    Protected WithEvents txtAddAmount3 As TextBox

    Protected WithEvents txtDedDate1 As TextBox
    Protected WithEvents btnDedDate1 As Image
    Protected WithEvents lblErrDedDate1 As Label
    Protected WithEvents txtDedDescr1 As TextBox
    Protected WithEvents txtDedAmount1 As TextBox

    Protected WithEvents txtDedDate2 As TextBox
    Protected WithEvents btnDedDate2 As Image
    Protected WithEvents lblErrDedDate2 As Label
    Protected WithEvents txtDedDescr2 As TextBox
    Protected WithEvents txtDedAmount2 As TextBox

    Protected WithEvents txtDedDate3 As TextBox
    Protected WithEvents btnDedDate3 As Image
    Protected WithEvents lblErrDedDate3 As Label
    Protected WithEvents txtDedDescr3 As TextBox
    Protected WithEvents txtDedAmount3 As TextBox


    Protected WithEvents SaveBtn As ImageButton
    Protected WithEvents DeleteBtn As ImageButton
    Protected WithEvents PrintBtn As ImageButton
    Protected WithEvents chkSelect As CheckBox


    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Protected objHRSetup As New agri.HR.clsSetup()

    Protected objGLSetup As New agri.GL.clsSetup()
    Protected objGLTrx As New agri.GL.clsTrx()
  
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

    Dim intCBAR As Integer
 
    Dim strAcceptDateFormat As String

    Dim intConfig As Integer
    Dim strLocType As String
    Dim strRekonsileId As String


    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)

        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strAccMonth = Session("SS_CBACCMONTH")
        strAccYear = Session("SS_CBACCYEAR")
        strLangCode = Session("SS_LANGCODE")
        intCBAR = Session("SS_CBAR")
        intConfig = Session("SS_CONFIGSETTING")
        strLocType = Session("SS_LOCTYPE")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCBAccessRights.CBDeposit), intCBAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            lblErrBank.Visible = False
            lblErrFromDate.Visible = False
            lblErrToDate.Visible = False
            lblErrMessage.Visible = False
            lblErrMessage.Visible = False

            lblErrAddDate1.Visible = False
            lblErrAddDate2.Visible = False
            lblErrAddDate3.Visible = False

            lblErrDedDate1.Visible = False
            lblErrDedDate2.Visible = False
            lblErrDedDate3.Visible = False

            strRekonsileId = Trim(IIf(Request.QueryString("Rekonsileid") = "", Request.Form("Rekonsileid"), Request.QueryString("Rekonsileid")))
            inrid.Value = strRekonsileId

            onload_GetLangCap()

            If Not IsPostBack Then

                If strRekonsileId <> "" Then
                    onLoad_Display(strRekonsileId)
                    onLoad_DisplayItem(strRekonsileId)
                    onLoad_Button(True)
                Else
                    BindAccYear(strAccYear)
                    lstAccMonth.SelectedIndex = val(strAccMonth) - 1
                    BindBankCode("")
                    onLoad_Button(False)
                End If
            End If

        End If
    End Sub

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

    Sub BindBankCode(ByVal pv_strBankCode As String)
        Dim strOpCode As String = "HR_CLSSETUP_BANK_LIST_GET"
        Dim objBankDs As New Object()
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedBankIndex As Integer = 0

        'strParam = "| And left(acccode,3)='110'"
		strParam = "|"

        Try
            intErrNo = objHRSetup.mtdGetMasterList(strOpCode, _
                                                   strParam, _
                                                   objHRSetup.EnumHRMasterType.Bank, _
                                                   objBankDs)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_GET_BANK&errmesg=" & Exp.ToString() & "&redirect=CB/trx/cb_trx_paylist.aspx")
        End Try

        For intCnt = 0 To objBankDs.Tables(0).Rows.Count - 1
            objBankDs.Tables(0).Rows(intCnt).Item("BankCode") = objBankDs.Tables(0).Rows(intCnt).Item("BankCode").Trim()
            objBankDs.Tables(0).Rows(intCnt).Item("Description") = objBankDs.Tables(0).Rows(intCnt).Item("BankCode") & " (" & objBankDs.Tables(0).Rows(intCnt).Item("Description").Trim() & ")"
            If pv_strBankCode = objBankDs.Tables(0).Rows(intCnt).Item("BankCode") Then
                intSelectedBankIndex = intCnt + 1
            End If
        Next intCnt

        Dim dr As DataRow
        dr = objBankDs.Tables(0).NewRow()
        dr("BankCode") = ""
        dr("Description") = "Please Select Bank Code"
        objBankDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlBank.DataSource = objBankDs.Tables(0)
        ddlBank.DataValueField = "BankCode"
        ddlBank.DataTextField = "Description"
        ddlBank.DataBind()
        ddlBank.SelectedIndex = intSelectedBankIndex

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


    Sub onLoad_Button(ByVal vblnHide As Boolean)

        DeleteBtn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"

        If vblnHide = True Then
            SaveBtn.Visible = True
            DeleteBtn.Visible = True
            PrintBtn.Visible = True
            RefreshBtn.Visible = True
            lstAccMonth.Enabled = False
            lstAccYear.Enabled = False
            ddlBank.Enabled = False
            txtFromDate.Enabled = False
            btnSelDateFrom.Enabled = False
            txtToDate.Enabled = False
            btnSelDateTo.Enabled = False
        Else
            SaveBtn.Visible = True
            DeleteBtn.Visible = False
            PrintBtn.Visible = False
            RefreshBtn.Visible = True
            lstAccMonth.Enabled = True
            lstAccYear.Enabled = True
            ddlBank.Enabled = True
            txtFromDate.Enabled = True
            btnSelDateFrom.Enabled = True
            txtToDate.Enabled = True
            btnSelDateTo.Enabled = True
        End If

    End Sub


    Sub onLoad_Display(ByVal pv_strRekonsileID As String)

        Dim strOpCode As String = "CB_CLSTRX_REKONSILE_GET"
        Dim intErrNo As Integer
        Dim strParamName As String = ""
        Dim strParamValue As String = ""

        strParamName = "REKONSILEID"
        strParamValue = pv_strRekonsileID

        Try
            intErrNo = objGLTrx.mtdGetDataCommon(strOpCode, _
                                                strParamName, _
                                                strParamValue, _
                                                dsMaster)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=REKONSILE__GET_HEADER&errmesg=" & lblErrMessage.Text & "&redirect=CB/trx/CB_trx_Rekonsilelist.aspx")
        End Try

        lblRekonsileID.Text = Trim(dsMaster.Tables(0).Rows(0).Item("RekonsileID"))
        'lstAccMonth.SelectedItem.Value = Val(Trim(dsMaster.Tables(0).Rows(0).Item("AccMonth")))
        BindAccYear(Trim(dsMaster.Tables(0).Rows(0).Item("AccYear")))
        lstAccMonth.SelectedIndex = Val(Trim(dsMaster.Tables(0).Rows(0).Item("AccMonth"))) - 1

        BindBankCode(Trim(dsMaster.Tables(0).Rows(0).Item("BankCode")))
        txtFromDate.Text = Date_Validation(dsMaster.Tables(0).Rows(0).Item("FromDate"), True)
        txtToDate.Text = Date_Validation(dsMaster.Tables(0).Rows(0).Item("ToDate"), True)
        lblDateCreated.Text = objGlobal.GetLongDate(dsMaster.Tables(0).Rows(0).Item("CreateDate"))
        lblLastUpdate.Text = objGlobal.GetLongDate(dsMaster.Tables(0).Rows(0).Item("UpdateDate"))
        lblUpdatedBy.Text = Trim(dsMaster.Tables(0).Rows(0).Item("UserName"))
        lblTotAmt.Text = FormatNumber(dsMaster.Tables(0).Rows(0).Item("SaldoBalance"), 2)
        lblSaldoAwal.Text = FormatNumber(dsMaster.Tables(0).Rows(0).Item("SaldoAwal"), 2)
        lblAmountHidden.Text = dsMaster.Tables(0).Rows(0).Item("SaldoAwal")
        txtSaldoBank.Text = dsMaster.Tables(0).Rows(0).Item("SaldoBank")

        txtAddDate1.Text = Date_Validation(dsMaster.Tables(0).Rows(0).Item("AddDate1"), True)
        txtAddDescr1.Text = Trim(dsMaster.Tables(0).Rows(0).Item("AddDescr1"))
        txtAddAmount1.Text = dsMaster.Tables(0).Rows(0).Item("AddAmount1")
        txtAddDate2.Text = Date_Validation(dsMaster.Tables(0).Rows(0).Item("AddDate2"), True)
        txtAddDescr2.Text = Trim(dsMaster.Tables(0).Rows(0).Item("AddDescr2"))
        txtAddAmount2.Text = dsMaster.Tables(0).Rows(0).Item("AddAmount2")
        txtAddDate3.Text = Date_Validation(dsMaster.Tables(0).Rows(0).Item("AddDate3"), True)
        txtAddDescr3.Text = Trim(dsMaster.Tables(0).Rows(0).Item("AddDescr3"))
        txtAddAmount3.Text = dsMaster.Tables(0).Rows(0).Item("AddAmount3")

        txtDedDate1.Text = Date_Validation(dsMaster.Tables(0).Rows(0).Item("DedDate1"), True)
        txtDedDescr1.Text = Trim(dsMaster.Tables(0).Rows(0).Item("DedDescr1"))
        txtDedAmount1.Text = dsMaster.Tables(0).Rows(0).Item("DedAmount1")
        txtDedDate2.Text = Date_Validation(dsMaster.Tables(0).Rows(0).Item("DedDate2"), True)
        txtDedDescr2.Text = Trim(dsMaster.Tables(0).Rows(0).Item("DedDescr2"))
        txtDedAmount2.Text = dsMaster.Tables(0).Rows(0).Item("DedAmount2")
        txtDedDate3.Text = Date_Validation(dsMaster.Tables(0).Rows(0).Item("DedDate3"), True)
        txtDedDescr3.Text = Trim(dsMaster.Tables(0).Rows(0).Item("DedDescr3"))
        txtDedAmount3.Text = dsMaster.Tables(0).Rows(0).Item("DedAmount3")

      
    End Sub

    Sub onLoad_DisplayItem(ByVal pv_strRekonsileID As String)

        Dim strOpCode As String = "CB_CLSTRX_REKONSILELN_GET"

        Dim intErrNo As Integer
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim intCnt As Integer

        strParamName = "REKONSILEID"
        strParamValue = pv_strRekonsileID

        Try
            intErrNo = objGLTrx.mtdGetDataCommon(strOpCode, _
                                                strParamName, _
                                                strParamValue, _
                                                dsLine)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_CLSTRX_REKONSILELN_GET&errmesg=" & lblErrMessage.Text & "&redirect=CB/trx/CB_trx_Rekonsilelist.aspx")
        End Try

        dgLineDet.DataSource = dsLine.Tables(0)
        dgLineDet.DataBind()

    End Sub

  
    Sub SaveBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)

        If SaveMaster = False Then
            lblErrMessage.Visible = True
            lblErrMessage.Text = "Please Check Your Data"
            Exit Sub
        End If

        'Refresh
        onLoad_Display(lblRekonsileID.Text)
        onLoad_DisplayItem(lblRekonsileID.Text)
        onLoad_Button(True)

    End Sub


    Private Function SaveMaster() As Boolean

        Dim strOpCode As String
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim intErrNo As Integer
        Dim strRekonsileID As String = ""
        Dim strFromDate As String = ""
        Dim strToDate As String = ""

        Dim dtmFromDate As Date
        Dim dtmToDate As Date

        Dim strAddDate1 As String = ""
        Dim strAddDate2 As String = ""
        Dim strAddDate3 As String = ""
        Dim strDedDate1 As String = ""
        Dim strDedDate2 As String = ""
        Dim strDedDate3 As String = ""

        SaveMaster = False
        'Validate Input
        If ddlBank.SelectedItem.Value = "" Then
            lblErrBank.Visible = True
            Exit Function
        End If

        strFromDate = TRIM(txtFromDate.Text)
        If strFromDate <> "" Then
            strFromDate = Date_Validation(strFromDate, False)
            If strFromDate = "" Then
                lblErrFromDate.Visible = True
                Exit Function
            End If


            dtmFromDate = strFromDate
            If month(dtmFromDate) <> val(lstAccMonth.SelectedItem.Value) Or _
               year(dtmFromDate) <> val(lstAccYear.SelectedItem.Value) Then
                lblErrFromDate.Visible = True
                Exit Function
            End If

        Else
            lblErrFromDate.Visible = True
            Exit Function
        End If

        strToDate = TRIM(txtToDate.Text)
        If strToDate <> "" Then
            strToDate = Date_Validation(strToDate, False)
            If strToDate = "" Then
                lblErrToDate.Visible = True
                Exit Function
            End If

            dtmToDate = strToDate
            If month(dtmToDate) <> val(lstAccMonth.SelectedItem.Value) Or _
               year(dtmToDate) <> val(lstAccYear.SelectedItem.Value) Then
                lblErrToDate.Visible = True
                Exit Function
            End If

        Else
            lblErrToDate.Visible = True
            Exit Function
        End If

        'validasi penambahan dan pengurangan
        If Val(txtAddAmount1.Text) <> 0 Then
            strAddDate1 = TRIM(txtAddDate1.Text)
            If strAddDate1 <> "" Then
                strAddDate1 = Date_Validation(strAddDate1, False)
                If strAddDate1 = "" Then
                    lblErrAddDate1.Visible = True
                    Exit Function
                End If
            Else
                lblErrAddDate1.Visible = True
                Exit Function
            End If
        Else
            strAddDate1 = ""
        End If

        If Val(txtAddAmount2.Text) <> 0 Then
            strAddDate2 = TRIM(txtAddDate2.Text)
            If strAddDate2 <> "" Then
                strAddDate2 = Date_Validation(strAddDate2, False)
                If strAddDate2 = "" Then
                    lblErrAddDate2.Visible = True
                    Exit Function
                End If
            Else
                lblErrAddDate2.Visible = True
                Exit Function
            End If
        Else
            strAddDate2 = ""
        End If

        If Val(txtAddAmount3.Text) <> 0 Then
            strAddDate3 = TRIM(txtAddDate3.Text)
            If strAddDate3 <> "" Then
                strAddDate3 = Date_Validation(strAddDate3, False)
                If strAddDate3 = "" Then
                    lblErrAddDate3.Visible = True
                    Exit Function
                End If
            Else
                lblErrAddDate3.Visible = True
                Exit Function
            End If
        Else
            strAddDate3 = ""
        End If

        If Val(txtDedAmount1.Text) <> 0 Then
            strDedDate1 = TRIM(txtDedDate1.Text)
            If strDedDate1 <> "" Then
                strDedDate1 = Date_Validation(strDedDate1, False)
                If strDedDate1 = "" Then
                    lblErrDedDate1.Visible = True
                    Exit Function
                End If
            Else
                lblErrDedDate1.Visible = True
                Exit Function
            End If
        Else
            strDedDate1 = ""
        End If

        If Val(txtDedAmount2.Text) <> 0 Then
            strDedDate2 = TRIM(txtDedDate2.Text)
            If strDedDate2 <> "" Then
                strDedDate2 = Date_Validation(strDedDate2, False)
                If strDedDate2 = "" Then
                    lblErrDedDate2.Visible = True
                    Exit Function
                End If
            Else
                lblErrDedDate2.Visible = True
                Exit Function
            End If
        Else
            strDedDate2 = ""
        End If

        If Val(txtDedAmount3.Text) <> 0 Then
            strDedDate3 = TRIM(txtDedDate3.Text)
            If strDedDate3 <> "" Then
                strDedDate3 = Date_Validation(strDedDate3, False)
                If strDedDate3 = "" Then
                    lblErrDedDate3.Visible = True
                    Exit Function
                End If
            Else
                lblErrDedDate3.Visible = True
                Exit Function
            End If
        Else
            strDedDate3 = ""
        End If





        strRekonsileID = lblRekonsileID.Text

        strParamName = "REKONSILEID|BANKCODE|SALDOAWAL|SALDOBALANCE|ACCMONTH|ACCYEAR|FROMDATE|TODATE|LOCCODE|USERID|SALDOBANK|" & _
                       "ADDDATE1|ADDDESCR1|ADDAMOUNT1|" & _
                       "ADDDATE2|ADDDESCR2|ADDAMOUNT2|" & _
                       "ADDDATE3|ADDDESCR3|ADDAMOUNT3|" & _
                       "DEDDATE1|DEDDESCR1|DEDAMOUNT1|" & _
                       "DEDDATE2|DEDDESCR2|DEDAMOUNT2|" & _
                       "DEDDATE3|DEDDESCR3|DEDAMOUNT3"

        strParamValue = strRekonsileID & "|" & ddlBank.SelectedValue & "|" & lblAmountHidden.Text & "|" & _
                        "0|" & _
                        lstAccMonth.SelectedItem.Value & "|" & lstAccYear.SelectedItem.Value & "|" & _
                        strFromDate & "|" & strToDate & "|" & strLocation & "|" & strUserId & "|" & txtSaldoBank.Text & "|" & _
                        strAddDate1 & "|" & txtAddDescr1.Text & "|" & txtAddAmount1.Text & "|" & _
                        strAddDate2 & "|" & txtAddDescr2.Text & "|" & txtAddAmount2.Text & "|" & _
                        strAddDate3 & "|" & txtAddDescr3.Text & "|" & txtAddAmount3.Text & "|" & _
                        strDedDate1 & "|" & txtDedDescr1.Text & "|" & txtDedAmount1.Text & "|" & _
                        strDedDate2 & "|" & txtDedDescr2.Text & "|" & txtDedAmount2.Text & "|" & _
                        strDedDate3 & "|" & txtDedDescr3.Text & "|" & txtDedAmount3.Text

        If Trim(strRekonsileID) = "" Then
            strOpCode = "CB_CLSTRX_REKONSILEHDR_ADD"
            Try
                intErrNo = objGLTrx.mtdGetDataCommon(strOpCode, _
                                                    strParamName, _
                                                    strParamValue, _
                                                    dsMaster)

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CP_CLSTRX_REKONSILEHDR_SAVE&errmesg=" & lblErrMessage.Text & "&redirect=CB/trx/CB_trx_Rekonsilelist.aspx")
            End Try

            strRekonsileID = Trim(dsMaster.Tables(0).Rows(0).Item("RekonsileID"))


            If intErrNo <> 0 Then

                SaveMaster = False
                Exit Function
            End If

        Else
            strOpCode = "CB_CLSTRX_REKONSILEHDR_UPD"
            Try
                intErrNo = objGLTrx.mtdInsertDataCommon(strOpCode, _
                                                      strParamName, _
                                                      strParamValue)

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CP_CLSTRX_REKONSILEHDR_SAVE&errmesg=" & lblErrMessage.Text & "&redirect=CB/trx/CB_trx_Rekonsilelist")
            End Try

            If intErrNo <> 0 Then

                SaveMaster = False
                Exit Function
            End If

        End If


        'save detail
        Dim rowCount As Integer = 0
        Dim strTrxID As String
        Dim strSource As String
        Dim strAmount As String
        Dim blnCheck As Integer

        Dim dgLineDeItem As DataGridItem
        Dim lbl As label

        strOpCode = "CB_CLSTRX_REKONSILEDTL_ADD"

        For Each dgLineDeItem In dgLineDet.Items

            Dim myCheckbox As CheckBox = CType(dgLineDeItem.Cells(7).Controls(1), CheckBox)
            If myCheckbox.Checked = True Then
                blnCheck = 1
            Else
                blnCheck = 0
            End If

            'If myCheckbox.Checked = False Then
            'trx id
            Dim myLabel As label = CType(dgLineDeItem.Cells(3).Controls(1), Label)
            strTrxID = myLabel.text
            'source
            Dim myLabel1 As label = CType(dgLineDeItem.Cells(8).Controls(1), Label)
            strSource = myLabel1.text
            'amount
            Dim myLabel2 As label = CType(dgLineDeItem.Cells(9).Controls(1), Label)
            strAmount = myLabel2.text

            strParamName = "REKONSILEID|TRXID|SOURCE|AMOUNT|ISSELECT"
            strParamValue = strRekonsileID & "|" & strTrxID & "|" & strSource & "|" & strAmount & "|" & blnCheck

            Try
                intErrNo = objGLTrx.mtdInsertDataCommon(strOpCode, _
                                                      strParamName, _
                                                      strParamValue)


            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CP_CLSTRX_REKONSILEDTL_ADD&errmesg=" & lblErrMessage.Text & "&redirect=CB/trx/CB_trx_Rekonsilelist")
            End Try

            'End If
        Next


        'update total mutasi
        strOpCode = "CB_CLSTRX_REKONSILEMUTASI_UPD"
        strParamName = "REKONSILEID"
        strParamValue = strRekonsileID

        Try
            intErrNo = objGLTrx.mtdInsertDataCommon(strOpCode, _
                                                  strParamName, _
                                                  strParamValue)


        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CP_CLSTRX_REKONSILEDTL_ADD&errmesg=" & lblErrMessage.Text & "&redirect=CB/trx/CB_trx_Rekonsilelist")
        End Try

        lblRekonsileID.Text = strRekonsileID


        SaveMaster = True
    End Function

   
    Sub DeleteBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)

        Dim strOpCode As String
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim intErrNo As Integer

        strParamName = "REKONSILEID"
        strParamValue = lblRekonsileID.Text

        strOpCode = "CB_CLSTRX_REKONSILE_DEL"

        Try
            intErrNo = objGLTrx.mtdInsertDataCommon(strOpCode, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GENERATE_CONFIRM&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcv_wm_list")
        End Try

        'Back
        Response.Redirect("CB_trx_Rekonsilelist.aspx")

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


    Sub RefreshBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)

        Dim strOpCode As String = "CB_CLSTRX_REKONSILEDTL_GET"


        Dim intErrNo As Integer
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim strFromDate As String = ""
        Dim strToDate As String = ""


        If ddlBank.SelectedItem.Value = "" Then
            lblErrBank.Visible = True
            Exit Sub
        End If

        strFromDate = TRIM(txtFromDate.Text)

        If strFromDate <> "" Then
            strFromDate = Date_Validation(strFromDate, False)
            If strFromDate = "" Then
                lblErrFromDate.Visible = True
                Exit Sub
            End If
        Else
            lblErrFromDate.Visible = True
            Exit Sub
        End If

        strToDate = TRIM(txtToDate.Text)
        If strToDate <> "" Then
            strToDate = Date_Validation(strToDate, False)
            If strToDate = "" Then
                lblErrToDate.Visible = True
                Exit Sub
            End If
        Else
            lblErrToDate.Visible = True
            Exit Sub
        End If



        strParamName = "BANKCODE|ACCMONTH|ACCYEAR|FROMDATE|TODATE"
        strParamValue = ddlBank.SelectedItem.Value & "|" & lstAccMonth.SelectedItem.Value & "|" & _
                        lstAccYear.SelectedItem.Value & "|" & strFromDate & "|" & strToDate



        Try
            intErrNo = objGLTrx.mtdGetDataCommon(strOpCode, _
                                                strParamName, _
                                                strParamValue, _
                                                dsLine)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_CLSTRX_REKONSILELN_GET&errmesg=" & lblErrMessage.Text & "&redirect=CB/trx/CB_trx_Rekonsilelist.aspx")
        End Try

        dgLineDet.DataSource = dsLine.Tables(0)
        dgLineDet.DataBind()


        lblSaldoAwal.Text = FormatNumber(dsLine.Tables(1).Rows(0).Item("SaldoAwal"), 2)
        lblAmountHidden.Text = dsLine.Tables(1).Rows(0).Item("SaldoAwal")

        'lblTotAmt.Text = FormatNumber(dsLine.Tables(2).Rows(0).Item("Total"), 2)

    End Sub



    Sub PrintBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)

        Dim strTRXID As String

        strTRXID = TRIM(lblRekonsileID.Text)

        Response.Write("<Script Language=""JavaScript"">window.open(""../reports/CB_Rpt_RekonsileDet.aspx?strTrxId=" & strTRXID & _
                         """, null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")


     
    End Sub



End Class
