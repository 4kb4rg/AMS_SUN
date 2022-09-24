
Imports System
Imports System.Data


Public Class cb_trx_DepositDet : Inherits Page

    Dim objCBTrx As New agri.CB.clsTrx()
    Dim objGLSetup As New agri.GL.clsSetup()
    Dim objHRSetup As New agri.HR.clsSetup()
    Dim objCMSetup As New agri.CM.clsSetup()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblOper As Label
    Protected WithEvents lblDepCode As Label
    Protected WithEvents lblStatus As Label
    Protected WithEvents lblCreateDate As Label
    Protected WithEvents lblLastUpdate As Label
    Protected WithEvents lblUpdateBy As Label
    Protected WithEvents lblErrAmount As Label
    Protected WithEvents lblErrRate As Label
    Protected WithEvents lblErrStartDate As Label
    Protected WithEvents lblErrEndDate As Label

    Protected WithEvents lblErrCurRate As Label
    Protected WithEvents lblConvAmount As Label
    
    Protected WithEvents txtDescription As TextBox
    Protected WithEvents txtBilyetNo As TextBox
    Protected WithEvents txtAccountNo As TextBox
    Protected WithEvents txtStartDate As TextBox
    Protected WithEvents txtEndDate As TextBox
    Protected WithEvents txtAmount As TextBox
    Protected WithEvents txtRate As TextBox
    Protected WithEvents txtCurRate As TextBox   
    Protected WithEvents txtRemarks As TextBox

    Protected WithEvents lbhStatus As HtmlInputHidden

    Protected WithEvents ddlBank As DropDownList
    Protected WithEvents ddlCurrency As DropDownList
    Protected WithEvents ddlType As DropDownList
    Protected WithEvents ddlAccCode As DropDownList

    Protected WithEvents SaveBtn As ImageButton
    Protected WithEvents ConfirmBtn As ImageButton
    Protected WithEvents DeleteBtn As ImageButton
    Protected WithEvents BackBtn As ImageButton
    Protected WithEvents CancelledBtn As ImageButton
    Protected WithEvents UndeleteBtn As ImageButton

    Protected WithEvents btnSelStartDate As Image
    Protected WithEvents btnSelEndDate As Image

    Dim strDepCode As String
    Dim strParam As String
    Dim intErrNo As Integer
    Dim objDataSet As New DataSet()
    Dim objLangCapDs As New Object()
    Dim strAcceptFormat As String

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intCBAR As Integer
    Dim strDateFormat As String
    Dim intConfigSetting As Integer
    
    Dim objBankDs As New Object()
    Dim objAccDs  As New Object()
    Dim objCurrDs  As New Object()
    Dim strLocType as String
    
  
    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strAccMonth = Session("SS_CBACCMONTH")
        strAccYear = Session("SS_CBACCYEAR")
        strLangCode = Session("SS_LANGCODE")
        intCBAR = Session("SS_CBAR")
        strLocType = Session("SS_LOCTYPE")

        strDateFormat = Session("SS_DATEFMT")
        intConfigSetting = Session("SS_CONFIGSETTING")
        
        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCBAccessRights.CBDeposit), intCBAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
           
            lblErrAmount.visible = False
            lblErrRate.visible = False
            lblErrCurRate.visible = False
            lblErrStartDate.visible = False
            lblErrEndDate.visible = False
            lblErrMessage.visible = False

            onload_GetLangCap()

            If Not Page.IsPostBack Then
                If Not Request.QueryString("DepositCode") = "" Then
                    lblDepCode.Text = Request.QueryString("DepositCode")
                    ViewState.Item("DepositCode") = Request.QueryString("DepositCode")
                End If

                If Not lblDepCode.Text = "" Then
                    DisplayData()
                Else
                    BindBankCode("")
                    BindAccCode("")
                    BindType("0")
                    BindCurrency("")
                    DeleteBtn.Visible = False
                    DisableControl(0)
                    DisableButton(0)
                End If
            End If
        End If
    End Sub

    Sub onload_GetLangCap()
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
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_DEPOSITDET_GET_LANGCAP&errmesg=" & Exp.Message & "&redirect=CB/trx/CB_trx_DepositList.aspx")
        End Try

    End Sub
    
    Function GetCaption(ByVal pv_TermCode) As String
        Dim count As Integer

        For count = 0 To objLangCapDs.Tables(0).Rows.Count - 1
            If Trim(pv_TermCode) = objLangCapDs.Tables(0).Rows(count).Item("TermCode") Then
                If strLocType = objAdminLoc.EnumLocType.Mill then
                    Return Trim(objLangCapDs.Tables(0).Rows(count).Item("BusinessTermMW"))
                else
                    Return Trim(objLangCapDs.Tables(0).Rows(count).Item("BusinessTerm"))
                end if
                Exit For
            End If
        Next
    End Function


    Protected Function LoadData() As DataSet
       
        Dim strOpCode_GetDepList As String = "CB_CLSTRX_DEPOSIT_LIST_GET" 
        
        strParam = lblDepCode.Text & "|||||||"
       
        Try
             intErrNo = objCBTrx.mtdGetDeposit(strOpCode_GetDepList, _
                                              strCompany, _
                                              strLocation, _
                                              strUserId, _
                                              strAccMonth, _
                                              strAccYear, _
                                              strParam, _
                                              objDataSet)
        
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_DEPOSITDET_GET&errmesg=" & Exp.Message & "&redirect=CB/trx/CB_trx_DepositList.aspx")
        End Try

        Return objDataSet
    End Function


    Sub DisableControl(ByVal intStatus as Integer)
        Dim blnView As Boolean
        Dim blnEdit As Boolean

        SELECT CASE intStatus
        CASE objCBTrx.EnumDepositStatus.Active, 0
            blnView = True
            blnEdit = True
        CASE objCBTrx.EnumDepositStatus.Cancelled, objCBTrx.EnumDepositStatus.Confirmed, _
             objCBTrx.EnumDepositStatus.Deleted, objCBTrx.EnumDepositStatus.Withdrawn 
            blnView = False
            blnEdit = False
        CASE 6 
            blnView = False
            blnEdit = True
        END SELECT

        txtDescription.Enabled = blnView
        txtBilyetNo.Enabled = blnView
        txtAccountNo.Enabled = blnView
        txtStartDate.Enabled = blnView
        txtAmount.Enabled = blnView
        txtRemarks.Enabled = blnView
        btnSelStartDate.Visible = blnView
        ddlBank.Enabled = blnView
        ddlType.Enabled = blnView
        ddlAccCode.Enabled = blnView
        
        txtRate.Enabled = blnEdit
        txtCurRate.Enabled = blnEdit
        ddlCurrency.Enabled = blnEdit
        txtEndDate.Enabled = blnEdit
        btnSelEndDate.Visible = blnEdit

    End Sub

    Sub DisplayData()
       
        Dim dsTx As DataSet = LoadData()
        
        If dsTx.Tables(0).Rows.Count > 0 Then
            lblDepCode.Text = Trim(dsTx.Tables(0).Rows(0).Item("DepositCode"))
            txtDescription.Text = Trim(dsTx.Tables(0).Rows(0).Item("Description"))
            txtBilyetNo.Text = Trim(dsTx.Tables(0).Rows(0).Item("BilyetNo"))
            txtAccountNo.Text = Trim(dsTx.Tables(0).Rows(0).Item("AccountNo"))
            txtStartDate.Text = Date_Validation(dsTx.Tables(0).Rows(0).Item("StartDate"), True)
            txtEndDate.Text = Date_Validation(dsTx.Tables(0).Rows(0).Item("EndDate"), True)
            txtAmount.Text = dsTx.Tables(0).Rows(0).Item("Amount")
            txtRate.Text = dsTx.Tables(0).Rows(0).Item("DepositRate")
            txtCurRate.Text = dsTx.Tables(0).Rows(0).Item("CurrencyRate")

            lblConvAmount.Text = dsTx.Tables(0).Rows(0).Item("Conversion")

            txtRemarks.Text = dsTx.Tables(0).Rows(0).Item("Remarks")
            lblStatus.Text = objCBTrx.mtdGetDepositStatus(Trim(dsTx.Tables(0).Rows(0).Item("Status")))
            lbhStatus.Value = Trim(dsTx.Tables(0).Rows(0).Item("Status"))
            lblCreateDate.Text = Trim(objGlobal.GetLongDate(dsTx.Tables(0).Rows(0).Item("CreateDate")))
            lblLastUpdate.Text = Trim(objGlobal.GetLongDate(dsTx.Tables(0).Rows(0).Item("UpdateDate")))
            lblUpdateBy.Text = Trim(dsTx.Tables(0).Rows(0).Item("Username"))
            BindBankCode(Trim(dsTx.Tables(0).Rows(0).Item("BankCode")))
            BindAccCode(Trim(dsTx.Tables(0).Rows(0).Item("AccCode")))
            BindType(Trim(dsTx.Tables(0).Rows(0).Item("Type")))
            BindCurrency(Trim(dsTx.Tables(0).Rows(0).Item("Currency")))
            DisableControl(Val(dsTx.Tables(0).Rows(0).Item("Status")))
            DisableButton(Val(dsTx.Tables(0).Rows(0).Item("Status")))
        End If
    End Sub

    Sub DisableButton(ByVal intStatus As Integer)
        
        Select Case intStatus
        case 0
             SaveBtn.Visible = True
             ConfirmBtn.Visible = False
             DeleteBtn.Visible = False
             BackBtn.Visible = True
             CancelledBtn.Visible = False
             UndeleteBtn.Visible = False
        Case objCBTrx.EnumDepositStatus.Active 
             SaveBtn.Visible = True
             ConfirmBtn.Visible = True
             DeleteBtn.Visible = True
             DeleteBtn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
             BackBtn.Visible = True
             CancelledBtn.Visible = False
             UndeleteBtn.Visible = False
        Case objCBTrx.EnumDepositStatus.Cancelled, objCBTrx.EnumDepositStatus.Withdrawn  
             SaveBtn.Visible = False
             ConfirmBtn.Visible = False
             DeleteBtn.Visible = False
             BackBtn.Visible = True
             CancelledBtn.Visible = False
             UndeleteBtn.Visible = False
        Case objCBTrx.EnumDepositStatus.Confirmed  
             SaveBtn.Visible = False
             ConfirmBtn.Visible = False
             DeleteBtn.Visible = False
             BackBtn.Visible = True
             CancelledBtn.Visible = True
             UndeleteBtn.Visible = False
        Case objCBTrx.EnumDepositStatus.Deleted  
             SaveBtn.Visible = False
             ConfirmBtn.Visible = False
             DeleteBtn.Visible = False
             BackBtn.Visible = True
             CancelledBtn.Visible = False
             UndeleteBtn.Visible = True
        End Select

    End Sub

    Sub BindType(ByVal pv_strType As String)
        ddlType.SelectedIndex = Val(pv_strType)
    End Sub

    Sub BindBankCode(ByVal pv_strBankCode As String)
        Dim strOpCode As String = "HR_CLSSETUP_BANK_LIST_GET"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedBankIndex As Integer = 0

        strParam = "|"

        Try
            intErrNo = objHRSetup.mtdGetMasterList(strOpCode, _
                                                   strParam, _
                                                   objHRSetup.EnumHRMasterType.Bank, _
                                                   objBankDs)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_DEPOSIT_GET_BANK&errmesg=" & Exp.Message & "&redirect=CB/trx/cb_trx_DepositList.aspx")
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
    
    Sub BindCurrency(ByVal pv_strCurrency As String)
        Dim strOpCode As String = "CM_CLSSETUP_CURRENCY_GET"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedCurrIndex As Integer = 0

        strParam = "|"

        Try
            intErrNo = objCMSetup.mtdGetMasterList(strOpCode, _
                                                   strParam, _
                                                   0, _
                                                   objCurrDs)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_DEPOSIT_GET_CURRENCY&errmesg=" & Exp.Message & "&redirect=CB/trx/cb_trx_DepositList.aspx")
        End Try

        For intCnt = 0 To objCurrDs.Tables(0).Rows.Count - 1
            objCurrDs.Tables(0).Rows(intCnt).Item("CurrencyCode") = objCurrDs.Tables(0).Rows(intCnt).Item("CurrencyCode").Trim()
            objCurrDs.Tables(0).Rows(intCnt).Item("Description") = objCurrDs.Tables(0).Rows(intCnt).Item("CurrencyCode") & " (" & objCurrDs.Tables(0).Rows(intCnt).Item("Description").Trim() & ")"
            If pv_strCurrency = objCurrDs.Tables(0).Rows(intCnt).Item("CurrencyCode") Then
                intSelectedCurrIndex = intCnt + 1
            End If
        Next intCnt

        Dim dr As DataRow
        dr = objCurrDs.Tables(0).NewRow()
        dr("CurrencyCode") = ""
        dr("Description") = "Please Select Currency Code" 
        objCurrDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlCurrency.DataSource = objCurrDs.Tables(0)
        ddlCurrency.DataValueField = "CurrencyCode"
        ddlCurrency.DataTextField = "Description"
        ddlCurrency.DataBind()
        ddlCurrency.SelectedIndex = intSelectedCurrIndex
    End Sub
    
    Sub BindAccCode(ByVal pv_strAccCode As String)
        Dim strOpCd As String = "GL_CLSSETUP_ACCOUNTCODE_LIST_GET"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedAccIndex As Integer = 0


        strParam = "Order By ACC.AccCode|AND ACC.Status = '" & objGLSetup.EnumAccountCodeStatus.Active & "' " & vbCrLf & "AND ACC.AccType = '" & objGLSetup.EnumAccountType.BalanceSheet & "'"
       
        Try
            intErrNo = objGLSetup.mtdGetMasterList(strOpCd, _
                                                   strParam, _
                                                   objGLSetup.EnumGLMasterType.AccountCode, _
                                                   objAccDs)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_DEPOSIT_GET_BINDACCCODE_GET&errmesg=" & Exp.Message & "&redirect=CB/trx/cb_trx_DepositList.aspx")
        End Try

        For intCnt = 0 To objAccDs.Tables(0).Rows.Count - 1
            objAccDs.Tables(0).Rows(intCnt).Item("AccCode") = Trim(objAccDs.Tables(0).Rows(intCnt).Item("AccCode"))
            objAccDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objAccDs.Tables(0).Rows(intCnt).Item("AccCode")) & " (" & Trim(objAccDs.Tables(0).Rows(intCnt).Item("Description")) & ")"
            
            If pv_strAccCode = objAccDs.Tables(0).Rows(intCnt).Item("AccCode") Then
                intSelectedAccIndex = intCnt + 1
            End If
        
        Next

        Dim dr As DataRow
        dr = objAccDs.Tables(0).NewRow()
        dr("AccCode") = ""
        dr("Description") = "Please Select Account Code" 
        objAccDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlAccCode.DataSource = objAccDs.Tables(0)
        ddlAccCode.DataValueField = "AccCode"
        ddlAccCode.DataTextField = "Description"
        ddlAccCode.DataBind()
        ddlAccCode.SelectedIndex = intSelectedAccIndex
    End Sub
    
    Sub UpdateStatus(Byval strStatus As String)
        Dim strOpCd As String = "CB_CLSTRX_DEPOSIT_STATUS_UPD"
        Dim strParam  As String
        Dim intErrNo As Integer
        Dim objRslDs  As New Object()
        
        strParam = lblDepCode.Text & "|" & strStatus & "|" & lbhStatus.Value
        
        Try     
            intErrNo = objCBTrx.mtdUpdDepositStatus(strOpCd, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strAccMonth, _
                                                    strAccYear, strParam, objRslDs)

            If objRslDs.Tables(0).Rows(0).Item("errCodes") > 0 then
                lblErrMessage.Text = objRslDs.Tables(0).Rows(0).Item("errMsgs")
                lblErrMessage.visible = true
                Exit Sub
            End If
            
            Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_CLSTRX_DEPOSIT_UPDATE&errmesg=" & Exp.Message & "&redirect=CB/trx/cb_trx_DepositList.aspx")
        End Try
        
        
        DisplayData()

    End Sub

    Sub UpdateData()
        Dim strOpCd As String 
        Dim strParam  As String
        Dim intErrNo As Integer
        Dim strCodeResult  As String
        Dim strStartDate As String = Date_Validation(txtStartDate.Text, False)
        Dim strEndDate As String = Date_Validation(txtEndDate.Text, False)
        Dim strDocCode As String
        
        If Trim(txtStartDate.Text) <> "" Then
            If Trim(strStartDate) = "" Then
                    lblErrStartDate.Visible = True
                    lblErrStartDate.Text = "Invalid date format." & strAcceptFormat
                    Exit Sub
            End If
        End If
        
        If Trim(txtEndDate.Text) <> "" Then
            If Trim(strEndDate) = "" Then
                  lblErrEndDate.Visible = True
                  lblErrEndDate.Text = "Invalid date format." & strAcceptFormat
                  Exit Sub
            End If
            If blnValidEndStartDate(Trim(strEndDate), Trim(strStartDate)) = False
                 lblErrEndDate.Visible = True
                 lblErrEndDate.Text = "Ending Date must greater then Starting Date."
                 Exit Sub
            End if 
        End If

        If Trim(lblDepCode.Text) = "" THEN 
            strOpCd = "CB_CLSTRX_DEPOSIT_ADD"
        Else
            strOpCd = "CB_CLSTRX_DEPOSIT_UPD"
        End If
       
        strDocCode = objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CBDeposit)

        strParam =  strDocCode & "|" & Trim(lblDepCode.Text) & "|" & Trim(txtDescription.text) & _
                   "|" & Trim(txtBilyetNo.Text) & "|" & Trim(txtAccountNo.Text) & _
                   "|" & ddlBank.SelectedItem.Value & "|" & strStartDate & _
                   "|" & strEndDate & "|" & Trim(txtAmount.Text) & _
                   "|" & Trim(txtRate.Text) & "|" & Trim(txtCurRate.Text) & _
                   "|" & ddlCurrency.SelectedItem.Value & "|" & ddlType.SelectedItem.Value & _
                   "|" & ddlAccCode.SelectedItem.Value & "|" & Trim(txtRemarks.Text) & "|" & objCBTrx.EnumDepositStatus.Active 

        Try     
            intErrNo = objCBTrx.mtdUpdDeposit(strOpCd, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strAccMonth, _
                                                    strAccYear, _
                                                    strParam, _
                                                    strCodeResult)
            Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_CLSTRX_DEPOSIT_UPDATE&errmesg=" & Exp.Message & "&redirect=CB/trx/cb_trx_DepositList.aspx")
        End Try
        
        If Trim(strCodeResult) <> "" THEN  lblDepCode.Text = strCodeResult
            DisplayData()
    End Sub

    Function blnValidEndStartDate(Byval pv_strEndDate As String, Byval pv_strStartDate as string) As Boolean
            blnValidEndStartDate = False
            If cDate(pv_strStartDate)< cDate(pv_strEndDate)
                blnValidEndStartDate = True
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

    Sub ConfirmBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        UpdateStatus(objCBTrx.EnumDepositStatus.Confirmed)
    End Sub

    Sub DeleteBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        UpdateStatus(objCBTrx.EnumDepositStatus.Deleted)
    End Sub
    
    Sub SaveBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Call UpdateData()
    End Sub
  
    Sub BackBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CB_trx_DepositList.aspx")
    End Sub
    
    Sub CancelledBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        UpdateStatus(objCBTrx.EnumDepositStatus.Cancelled)
    End Sub  
  
    Sub UndeleteBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        UpdateStatus(objCBTrx.EnumDepositStatus.Active)
    End Sub   

    
End Class
