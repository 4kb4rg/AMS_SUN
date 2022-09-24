Imports System
Imports System.Data

Public Class cb_trx_InterestAdjDet : Inherits Page

    Dim objCBTrx As New agri.CB.clsTrx()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objAdminLoc As New agri.Admin.clsLoc()
    
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblIntCode As Label
    Protected WithEvents lblStatus As Label
    Protected WithEvents txtDescription As TextBox
    Protected WithEvents rfvDescription As RequiredFieldValidator
    Protected WithEvents lblCreateDate As Label
    Protected WithEvents ddlDepCode As DropDownList
    Protected WithEvents rfvDepCode As RequiredFieldValidator
    Protected WithEvents lblLastUpdate As Label
    Protected WithEvents lblAmount As Label
    Protected WithEvents lblUpdateBy As Label
    Protected WithEvents lblTotalInt As Label
    Protected WithEvents txtAmountAdj As TextBox
    Protected WithEvents rfvAmountAdj As RequiredFieldValidator
    Protected WithEvents cvAmountAdj As CompareValidator
    Protected WithEvents revAmountAdj As RegularExpressionValidator
    Protected WithEvents lblErrAmountAdj As Label
    Protected WithEvents txtRemarks As TextBox
    Protected WithEvents SaveBtn As ImageButton
    Protected WithEvents ConfirmBtn As ImageButton
    Protected WithEvents CancelledBtn As ImageButton
    Protected WithEvents UndeleteBtn As ImageButton
    Protected WithEvents DeleteBtn As ImageButton
    Protected WithEvents BackBtn As ImageButton
    Protected WithEvents lbhStatus As HtmlInputHidden
    
    Dim strIntCode As String
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
    
    Dim objIntDs As New Object()
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
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCBAccessRights.CBInterAdj), intCBAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            lblErrAmountAdj.visible = False
            lblErrMessage.Visible = False

            onload_GetLangCap()

            If Not Page.IsPostBack Then
                If Not Request.QueryString("InterestCode") = "" Then
                    lblIntCode.Text = Request.QueryString("InterestCode")
                    ViewState.Item("InterestCode") = Request.QueryString("InterestCode")
                End If

                If Not lblIntCode.Text = "" Then
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_INTERESTADJDET_GET_LANGCAP&errmesg=" & Exp.Message & "&redirect=CB/trx/CB_trx_InterestAdjList.aspx")
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
       
        Dim strOpCode As String = "CB_CLSTRX_INTERESTADJ_LIST_GET" 
        
        strParam = lblIntCode.Text & "|||||||"
       
        Try
             intErrNo = objCBTrx.mtdGetInterestAdj(strOpCode, _
                                              strCompany, _
                                              strLocation, _
                                              strUserId, _
                                              strAccMonth, _
                                              strAccYear, _
                                              strParam, _
                                              objDataSet)
        
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_INTERESTADJ_LIST_GET&errmesg=" & Exp.Message & "&redirect=CB/trx/CB_trx_InterestAdjList.aspx")
        End Try

        Return objDataSet
    End Function

    Sub DisableControl(ByVal intStatus as Integer)
        Dim blnView As Boolean
        
        SELECT CASE intStatus
        CASE objCBTrx.EnumInterestAdjStatus.Active, 0
              blnView = True
              If intStatus = 0 Then
                ddlDepCode.Enabled = True
              Else
                ddlDepCode.Enabled = False
              End If
         CASE objCBTrx.EnumInterestAdjStatus.Cancelled, objCBTrx.EnumInterestAdjStatus.Confirmed, _
              objCBTrx.EnumInterestAdjStatus.Deleted 
              blnView = False
              ddlDepCode.Enabled = False
        END SELECT
        
        txtDescription.Enabled = blnView
        txtAmountAdj.Enabled = blnView
        txtRemarks.Enabled = blnView

    End Sub

    Sub DisplayData()
       
        Dim dsTx As DataSet = LoadData()
        
        If dsTx.Tables(0).Rows.Count > 0 Then
            
            lblIntCode.Text = Trim(dsTx.Tables(0).Rows(0).Item("InterestCode"))
            lblStatus.Text = objCBTrx.mtdGetDepositStatus(Trim(dsTx.Tables(0).Rows(0).Item("Status")))
            lbhStatus.Value = Trim(dsTx.Tables(0).Rows(0).Item("Status"))
            txtDescription.Text = Trim(dsTx.Tables(0).Rows(0).Item("Description"))
            lblCreateDate.Text = Trim(objGlobal.GetLongDate(dsTx.Tables(0).Rows(0).Item("CreateDate")))
            lblLastUpdate.Text = Trim(objGlobal.GetLongDate(dsTx.Tables(0).Rows(0).Item("UpdateDate")))

            lblAmount.Text = Trim(dsTx.Tables(0).Rows(0).Item("Amount"))
            lblTotalInt.Text = Trim(dsTx.Tables(0).Rows(0).Item("Interest"))

            txtAmountAdj.Text = Trim(dsTx.Tables(0).Rows(0).Item("Adjusted"))
            txtRemarks.Text =  Trim(dsTx.Tables(0).Rows(0).Item("Remarks"))

            lblUpdateBy.Text = Trim(dsTx.Tables(0).Rows(0).Item("UserName"))

            BindDeposit(Trim(dsTx.Tables(0).Rows(0).Item("DepositCode")), True)
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
        Case objCBTrx.EnumInterestAdjStatus.Active 
             SaveBtn.Visible = True
             ConfirmBtn.Visible = True
             DeleteBtn.Visible = True
             DeleteBtn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
             BackBtn.Visible = True
             CancelledBtn.Visible = False
             UndeleteBtn.Visible = False
        Case objCBTrx.EnumInterestAdjStatus.Cancelled  
             SaveBtn.Visible = False
             ConfirmBtn.Visible = False
             DeleteBtn.Visible = False
             BackBtn.Visible = True
             CancelledBtn.Visible = False
             UndeleteBtn.Visible = False
        Case objCBTrx.EnumInterestAdjStatus.Confirmed 
             SaveBtn.Visible = False
             ConfirmBtn.Visible = False
             DeleteBtn.Visible = False
             BackBtn.Visible = True
             CancelledBtn.Visible = True
             UndeleteBtn.Visible = False
        Case objCBTrx.EnumInterestAdjStatus.Deleted  
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
            strParam =  " STATUS = '" & objCBTrx.EnumDepositStatus.Withdrawn & "' AND LocCode = '" & strLocation & "'"
        Else
            strParam =  " DEPOSITCODE = '" & pv_strDepCode & "'"
        End If

        Try
            intErrNo = objCBTrx.mtdGetDepositList(strOpCode, _
                                                   strParam, _
                                                   objIntDs)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_DEPOSIT_LIST&errmesg=" & Exp.Message & "&redirect=CB/trx/cb_trx_WithdrawalList.aspx")
        End Try

        lblAmount.Text = ""
        lblTotalInt.Text = ""
        For intCnt = 0 To objIntDs.Tables(0).Rows.Count - 1
            objIntDs.Tables(0).Rows(intCnt).Item("DepositCode") = objIntDs.Tables(0).Rows(intCnt).Item("DepositCode").Trim()
            
            If Trim(pv_strDepCode) = Trim(objIntDs.Tables(0).Rows(intCnt).Item("DepositCode")) Then
                intSelectedCurrIndex = intCnt + 1
                lblAmount.Text =   objIntDs.Tables(0).Rows(intCnt).Item("Amount")
                lblTotalInt.Text = objIntDs.Tables(0).Rows(intCnt).Item("Interest")


                Exit For
            End If
        Next intCnt

        Dim dr As DataRow
        dr = objIntDs.Tables(0).NewRow()
        dr("DepositCode") = ""
        dr("Description") = "Please Select Deposit Code" 
        objIntDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlDepCode.DataSource = objIntDs.Tables(0)
        ddlDepCode.DataValueField = "DepositCode"
        ddlDepCode.DataTextField = "Description"
        ddlDepCode.DataBind()
        ddlDepCode.SelectedIndex = intSelectedCurrIndex
    End Sub
    
    Sub UpdateStatus(Byval vstrStatus As String)
        Dim strOpCd As String = "CB_CLSTRX_INTERESTADJ_STATUS_UPD"
        Dim strParam  As String
        Dim intErrNo As Integer
        Dim objRslDs  As New Object()
        
        strParam = lblIntCode.Text & "|" & vstrStatus & "|" & Trim(ddlDepCode.SelectedItem.Value) & "|" &  lbhStatus.Value
        
        Try     
            intErrNo = objCBTrx.mtdUpdInterestAdjStatus(strOpCd, _
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_CLSTRX_INTERESTADJ_UPDATE&errmesg=" & Exp.Message & "&redirect=CB/trx/cb_trx_InterestAdjList.aspx")
        End Try

        DisplayData()

    End Sub

    Sub UpdateData()
        Dim strOpCd As String 
        Dim strParam  As String
        Dim intErrNo As Integer
        Dim strCodeResult  As String
        Dim strDocCode As String

        If Trim(lblIntCode.Text) = "" THEN 
            strOpCd = "CB_CLSTRX_INTERESTADJ_ADD"
        Else
            strOpCd = "CB_CLSTRX_INTERESTADJ_UPD"
        End If

        strDocCode = objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CBIntAdjustment)
       
        strParam =  strDocCode & "|" & Trim(lblIntCode.Text) & "|" & Trim(txtDescription.text) & _
                   "|" & ddlDepCode.SelectedItem.Value & "|" & Trim(txtAmountAdj.Text) & _
                   "|" & Trim(txtRemarks.Text) & "|" & objCBTrx.EnumInterestAdjStatus.Active

        Try     
            intErrNo = objCBTrx.mtdUpdInterestAdj(strOpCd, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strAccMonth, _
                                                    strAccYear, _
                                                    strParam, _
                                                    strCodeResult)
            Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_CLSTRX_INTERESTADJ_UPDATE&errmesg=" & Exp.Message & "&redirect=CB/trx/cb_trx_InterestAdjList.aspx")
        End Try
        
        If Trim(strCodeResult) <> "" THEN  lblIntCode.Text = strCodeResult
            DisplayData()
    End Sub

    Sub ConfirmBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        UpdateStatus(objCBTrx.EnumInterestAdjStatus.Confirmed)
    End Sub

    Sub DeleteBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        UpdateStatus(objCBTrx.EnumInterestAdjStatus.Deleted)
    End Sub
    
    Sub SaveBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Call UpdateData()
    End Sub
  
    Sub BackBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CB_trx_InterestAdjList.aspx")
    End Sub
    
    Sub CancelledBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        UpdateStatus(objCBTrx.EnumInterestAdjStatus.Cancelled)
    End Sub  
  
    Sub UndeleteBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        UpdateStatus(objCBTrx.EnumInterestAdjStatus.Active)
    End Sub   

    Sub onDepCode_Change(ByVal Sender As System.Object, ByVal E As System.EventArgs)
            BindDeposit(ddlDepCode.SelectedItem.Value)
    End Sub
    
    
End Class
