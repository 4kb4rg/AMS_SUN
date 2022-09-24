
Imports System
Imports System.Data

Public Class cb_trx_WithdrawalDet : Inherits Page

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
    Protected WithEvents lblWdrCode As Label
    Protected WithEvents lblStatus As Label
    Protected WithEvents txtDescription As TextBox
    Protected WithEvents rfvDescription As RequiredFieldValidator
    Protected WithEvents lblCreateDate As Label
    Protected WithEvents ddlDepCode As DropDownList
    Protected WithEvents rfvDepCode As RequiredFieldValidator
    Protected WithEvents lblLastUpdate As Label
    Protected WithEvents lblBilyetNo As Label
    Protected WithEvents lblUpdateBy As Label
    Protected WithEvents lblAccountNo As Label
    Protected WithEvents lblAmount As Label
    Protected WithEvents txtRate As TextBox
    Protected WithEvents rfvRate As RequiredFieldValidator
    Protected WithEvents cvRate As CompareValidator
    Protected WithEvents revRate As RegularExpressionValidator
    Protected WithEvents lblErrRate As Label
    Protected WithEvents txtRemarks As TextBox
    Protected WithEvents SaveBtn As ImageButton
    Protected WithEvents ConfirmBtn As ImageButton
    Protected WithEvents CancelledBtn As ImageButton
    Protected WithEvents UndeleteBtn As ImageButton
    Protected WithEvents DeleteBtn As ImageButton
    Protected WithEvents BackBtn As ImageButton

    Protected WithEvents lbhStatus As HtmlInputHidden

    Dim strWdrCode As String
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
    Dim intConfigSetting As Integer
    
    Dim objDepDs As New Object()
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

        intConfigSetting = Session("SS_CONFIGSETTING")
        
        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCBAccessRights.CBWithdrawal), intCBAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            lblErrRate.Visible = False
            lblErrMessage.Visible = False

            onload_GetLangCap()

            If Not Page.IsPostBack Then
                If Not Request.QueryString("WithdrawalCode") = "" Then
                    lblWdrCode.Text = Request.QueryString("WithdrawalCode")
                    ViewState.Item("WithdrawalCode") = Request.QueryString("WithdrawalCode")
                End If

                If Not lblWdrCode.Text = "" Then
                    DisplayData()
                Else
                    BindDeposit("")
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_WITHDRAWALDET_GET_LANGCAP&errmesg=" & Exp.Message & "&redirect=CB/trx/CB_trx_WithdrawalList.aspx")
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
       
        Dim strOpCode_GetWdrList As String = "CB_CLSTRX_WITHDRAWAL_LIST_GET" 
        
        strParam = lblWdrCode.Text & "|||||||"
       
        Try
             intErrNo = objCBTrx.mtdGetWithdrawal(strOpCode_GetWdrList, _
                                              strCompany, _
                                              strLocation, _
                                              strUserId, _
                                              strAccMonth, _
                                              strAccYear, _
                                              strParam, _
                                              objDataSet)
        
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_WITHDRAWAL_LIST_GET&errmesg=" & Exp.Message & "&redirect=CB/trx/CB_trx_WithdrawalList.aspx")
        End Try

        Return objDataSet
    End Function

    Sub DisableControl(ByVal intStatus as Integer)
        Dim blnView As Boolean
        
        SELECT CASE intStatus
        CASE objCBTrx.EnumWithdrawalStatus.Active, 0
            blnView = True
            If intStatus = 0 Then
                ddlDepCode.Enabled = True
            Else
                ddlDepCode.Enabled = False
            End If
        CASE objCBTrx.EnumWithdrawalStatus.Cancelled,  objCBTrx.EnumWithdrawalStatus.Confirmed, _
            objCBTrx.EnumWithdrawalStatus.Deleted   
            blnView = False
            ddlDepCode.Enabled = False
        END SELECT
      
        txtDescription.Enabled = blnView
       
        txtRate.Enabled = blnView
        txtRemarks.Enabled = blnView
    End Sub

    Sub DisplayData()
       
        Dim dsTx As DataSet = LoadData()
        
        If dsTx.Tables(0).Rows.Count > 0 Then
            
            lblWdrCode.Text = Trim(dsTx.Tables(0).Rows(0).Item("WithdrawalCode"))
            lblStatus.Text = objCBTrx.mtdGetDepositStatus(Trim(dsTx.Tables(0).Rows(0).Item("Status")))
            lbhStatus.Value = Trim(dsTx.Tables(0).Rows(0).Item("Status"))
            txtDescription.Text = Trim(dsTx.Tables(0).Rows(0).Item("Description"))
            lblCreateDate.Text = Trim(objGlobal.GetLongDate(dsTx.Tables(0).Rows(0).Item("CreateDate")))
            lblLastUpdate.Text = Trim(objGlobal.GetLongDate(dsTx.Tables(0).Rows(0).Item("UpdateDate")))
            lblBilyetNo.Text = Trim(dsTx.Tables(0).Rows(0).Item("BilyetNo"))
            lblAccountNo.Text = Trim(dsTx.Tables(0).Rows(0).Item("AccountNo"))
            lblAmount.Text = Trim(dsTx.Tables(0).Rows(0).Item("Amount"))
            txtRate.Text = Trim(dsTx.Tables(0).Rows(0).Item("WithdrawalRate"))
            txtRemarks.Text =  Trim(dsTx.Tables(0).Rows(0).Item("Remarks"))
           
            BindDeposit(Trim(dsTx.Tables(0).Rows(0).Item("DepositCode")), True)

            lblUpdateBy.Text = Trim(dsTx.Tables(0).Rows(0).Item("UserName"))

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
        Case objCBTrx.EnumWithdrawalStatus.Active 
             SaveBtn.Visible = True
             ConfirmBtn.Visible = True
             DeleteBtn.Visible = True
             DeleteBtn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
             BackBtn.Visible = True
             CancelledBtn.Visible = False
             UndeleteBtn.Visible = False
        Case objCBTrx.EnumWithdrawalStatus.Cancelled 
             SaveBtn.Visible = False
             ConfirmBtn.Visible = False
             DeleteBtn.Visible = False
             BackBtn.Visible = True
             CancelledBtn.Visible = False
             UndeleteBtn.Visible = False
        Case objCBTrx.EnumWithdrawalStatus.Confirmed 
             SaveBtn.Visible = False
             ConfirmBtn.Visible = False
             DeleteBtn.Visible = False
             BackBtn.Visible = True
             CancelledBtn.Visible = True
             UndeleteBtn.Visible = False
        Case objCBTrx.EnumWithdrawalStatus.Deleted 
             SaveBtn.Visible = False
             ConfirmBtn.Visible = False
             DeleteBtn.Visible = False
             BackBtn.Visible = True
             CancelledBtn.Visible = False
             UndeleteBtn.Visible = True
        End Select

    End Sub
   
    Sub BindDeposit(ByVal pv_strDepCode As String, Optional Byval pv_blnView As Boolean = False)
        Dim strOpCode As String = "CB_CLSTRX_DEPOSITLIST"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer = 0
        Dim intSelectedCurrIndex As Integer = 0
       
        If pv_blnView = False Then
            strParam =  " STATUS = '" & objCBTrx.EnumDepositStatus.Confirmed & "' AND LocCode = '" & strLocation & "'"
        Else
            strParam =  " DEPOSITCODE = '" & pv_strDepCode & "'"
        End If

        Try
            intErrNo = objCBTrx.mtdGetDepositList(strOpCode, _
                                                   strParam, _
                                                   objDepDs)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_DEPOSIT_LIST&errmesg=" & Exp.Message & "&redirect=CB/trx/cb_trx_WithdrawalList.aspx")
        End Try
        
        lblBilyetNo.Text = ""
        lblAccountNo.Text = ""
        lblAmount.Text = ""
        For intCnt = 0 To objDepDs.Tables(0).Rows.Count - 1
            objDepDs.Tables(0).Rows(intCnt).Item("DepositCode") = objDepDs.Tables(0).Rows(intCnt).Item("DepositCode").Trim()
            
            If Trim(pv_strDepCode) = Trim(objDepDs.Tables(0).Rows(intCnt).Item("DepositCode")) Then
                intSelectedCurrIndex = intCnt + 1
                lblBilyetNo.Text = objDepDs.Tables(0).Rows(intCnt).Item("BilyetNo")
                lblAccountNo.Text = objDepDs.Tables(0).Rows(intCnt).Item("AccountNo")
                lblAmount.Text = objDepDs.Tables(0).Rows(intCnt).Item("Amount")
                Exit For
            End If
        Next intCnt

        Dim dr As DataRow
        dr = objDepDs.Tables(0).NewRow()
        dr("DepositCode") = ""
        dr("Description") = "Please Select Deposit Code" 
        objDepDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlDepCode.DataSource = objDepDs.Tables(0)
        ddlDepCode.DataValueField = "DepositCode"
        ddlDepCode.DataTextField = "Description"
        ddlDepCode.DataBind()
        ddlDepCode.SelectedIndex = intSelectedCurrIndex
    End Sub
    
    Sub UpdateStatus(Byval vstrStatus As String)
        Dim strOpCd As String = "CB_CLSTRX_WITHDRAWAL_STATUS_UPD"
        Dim strParam  As String
        Dim intErrNo As Integer
        Dim objRslDs  As New Object()
        
        strParam = lblWdrCode.Text & "|" & vstrStatus & "|" & Trim(ddlDepCode.SelectedItem.Value) &  "|" & lbhStatus.Value

        Try     
            intErrNo = objCBTrx.mtdUpdWithdrawalStatus(strOpCd, _
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_CLSTRX_WITHDRAWAL_UPDATE&errmesg=" & Exp.Message & "&redirect=CB/trx/cb_trx_WithdrawalList.aspx")
        End Try

        DisplayData()

    End Sub

    Sub UpdateData()
        Dim strOpCd As String 
        Dim strParam  As String
        Dim intErrNo As Integer
        Dim strCodeResult  As String
        Dim strDocCode As String
        
        If Trim(lblWdrCode.Text) = "" THEN 
            strOpCd = "CB_CLSTRX_WITHDRAWAL_ADD"
        Else
            strOpCd = "CB_CLSTRX_WITHDRAWAL_UPD"
        End If
        
        strDocCode = objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CBWithdrawal)

        strParam =  strDocCode & "|" & Trim(lblWdrCode.Text) & "|" & Trim(txtDescription.text) & _
                   "|" & ddlDepCode.SelectedItem.Value & "|" & Trim(txtRate.Text) & _
                   "|" & Trim(txtRemarks.Text) & "|" & objCBTrx.EnumWithdrawalStatus.Active

        Try     
            intErrNo = objCBTrx.mtdUpdWithdrawal(strOpCd, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strAccMonth, _
                                                    strAccYear, _
                                                    strParam, _
                                                    strCodeResult)
            Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_CLSTRX_WITHDRAWAL_UPDATE&errmesg=" & Exp.Message & "&redirect=CB/trx/cb_trx_WithdrawalList.aspx")
        End Try
        
        If Trim(strCodeResult) <> "" THEN  lblWdrCode.Text = strCodeResult
            DisplayData()
    End Sub

    Sub ConfirmBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        UpdateStatus(objCBTrx.EnumWithdrawalStatus.Confirmed)
    End Sub

    Sub DeleteBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        UpdateStatus(objCBTrx.EnumWithdrawalStatus.Deleted)
    End Sub
    
    Sub SaveBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Call UpdateData()
    End Sub
  
    Sub BackBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CB_trx_WithdrawalList.aspx")
    End Sub
    
    Sub CancelledBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        UpdateStatus(objCBTrx.EnumWithdrawalStatus.Cancelled)
    End Sub  
  
    Sub UndeleteBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        UpdateStatus(objCBTrx.EnumWithdrawalStatus.Active)
    End Sub   

    Sub onDepCode_Change(ByVal Sender As System.Object, ByVal E As System.EventArgs)
        BindDeposit(ddlDepCode.SelectedItem.Value)
    End Sub
    
    
End Class
