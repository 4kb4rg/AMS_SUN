Imports System
Imports System.Data
Imports System.Collections 
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic


Public Class CM_setup_ExchangeRateDet : Inherits Page

    Protected WithEvents ddlFirstCurr As DropDownList
    Protected WithEvents ddlSecCurr As DropDownList
    Protected WithEvents txtExchangeRate As TextBox

    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblErrDup As Label
    Protected WithEvents lblHiddenSts As Label

    Protected WithEvents lblStatus As Label
    Protected WithEvents lblDateCreated As Label
    Protected WithEvents lblLastUpdate As Label
    Protected WithEvents lblUpdatedBy As Label

    Protected WithEvents SaveBtn As ImageButton
    Protected WithEvents DelBtn As ImageButton
    Protected WithEvents UnDelBtn As ImageButton

    Protected WithEvents tbcode As HtmlInputHidden

    Protected WithEvents lblDupMsg As Label
    Protected WithEvents lblFmt As Label
    Protected WithEvents lblDate As Label
    Protected WithEvents txtdate As TextBox
    Protected WithEvents btnSelDate As Image

    Protected WithEvents NewBtn As ImageButton

    Protected objCMSetup As New agri.CM.clsSetup()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objSysCfg As New agri.PWSystem.clsConfig()

    Dim objCurrDs As New Object()

    Dim intErrNo As Integer
    Dim strParam As String = ""
    Dim strCompositKey As String = ""
    Dim strCompositCode As String = ""
    Dim strCompositDate As String = ""
    Dim strOpCdGet As String = "CM_CLSSETUP_EXCHANGERATE_GET"
    Dim strOpCdUpd As String = "CM_CLSSETUP_EXCHANGERATE_UPD"
    Dim strOpCdAdd As String = "CM_CLSSETUP_EXCHANGERATE_ADD"

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim intCMAR As Integer
    Dim strAcceptFormat As String

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_ARACCMONTH")
        strAccYear = Session("SS_ARACCYEAR")
        intCMAR = Session("SS_CMAR")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCMAccessRights.CMExchangeRate), intCMAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            lblErrDup.Visible = False
            strCompositKey = Trim(IIf(Request.QueryString("tbcode") <> "", Request.QueryString("tbcode"), Request.Form("tbcode")))

            If Not IsPostBack Then
                BindCurrency()
                If strCompositKey <> "" Then
                    strCompositCode = Left(strCompositKey, 6)
                    strCompositDate = Mid(strCompositKey, 7)
                    onLoad_Display()
                    onLoad_BindButton()
                Else
                    onLoad_BindButton()
                End If
            End If
            
        End If
    End Sub

    Private Function getCurrency() AS dataset
        Dim strOppCd As String = "CM_CLSSETUP_CURRENCY_GET"
        Dim strParam As String = "and curr.Status = '" & objCMSetup.EnumCurrencyStatus.Active & "' | order by curr.CurrencyCode"
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim objCurrDs As New Object()
        Dim drCurrDs As DataRow

        Try
            intErrNo = objCMSetup.mtdGetMasterList(strOppCd, strParam, 0, objCurrDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_SETUP_EXCHANGERATE_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Setup/CM_setup_ExchangeRateDet.aspx")
        End Try

        For intCnt = 0 To objCurrDs.Tables(0).Rows.Count - 1
            objCurrDs.Tables(0).Rows(intCnt).Item("CurrencyCode") = Trim(objCurrDs.Tables(0).Rows(intCnt).Item("CurrencyCode"))
            objCurrDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objCurrDs.Tables(0).Rows(intCnt).Item("CurrencyCode")) & " (" & Trim(objCurrDs.Tables(0).Rows(intCnt).Item("Description")) & ")"
        Next

            getCurrency = objCurrDs
    End Function

    Private Sub BindCurrency()
        Dim objFirstCurrDs As New Object()
        Dim objSecCurrDs As New Object()

        Dim drFirstCurr As DataRow
        Dim drSecCurr As DataRow

        objFirstCurrDs = getCurrency()
        objSecCurrDs = getCurrency()

        drFirstCurr = objFirstCurrDs.Tables(0).NewRow()
        drFirstCurr("CurrencyCode") = ""
        drFirstCurr("Description") = "Select First Currency Code"
        objFirstCurrDs.Tables(0).Rows.InsertAt(drFirstCurr, 0)

        ddlFirstCurr.DataSource = objFirstCurrDs.Tables(0)
        ddlFirstCurr.DataValueField = "CurrencyCode"
        ddlFirstCurr.DataTextField = "Description"
        ddlFirstCurr.DataBind()

        drSecCurr = objSecCurrDs.Tables(0).NewRow()
        drSecCurr("CurrencyCode") = ""
        drSecCurr("Description") = "Select Second Currency Code"
        objSecCurrDs.Tables(0).Rows.InsertAt(drSecCurr, 0)

        ddlSecCurr.DataSource = objSecCurrDs.Tables(0)
        ddlSecCurr.DataValueField = "CurrencyCode"
        ddlSecCurr.DataTextField = "Description"
        ddlSecCurr.DataBind()
    End Sub

    Sub BackBtn_Click(Sender As Object, E As ImageClickEventArgs)
        Response.Redirect("CM_setup_ExchangeRate.aspx")
    End Sub

    Sub Button_Click(Sender As Object, E As ImageClickEventArgs)
        Dim strCmdArgs As String = CType(Sender, ImageButton).CommandArgument        
        Dim intErrNo As Integer
        Dim blnIsUpdate As Boolean
        Dim strParam As String = ""
        Dim blnDupKey As Boolean = False
        Dim strFirstCurr As String = ddlFirstCurr.SelectedValue
        Dim strSecCurr As String = ddlSecCurr.SelectedValue
        Dim strRate As String = txtExchangeRate.text
        Dim strDate As String = Date_Validation(txtdate.Text, False)

        If strCmdArgs = "Save" Then
            blnIsUpdate = IIf(Len(strCompositKey) = 0, False, True)

            strCompositCode = Left(strCompositKey, 6)
            strParam = strFirstCurr & Chr(9) & _
                       strSecCurr & Chr(9) & _
                       strRate & Chr(9) & _
                       objCMSetup.EnumExchangeRateStatus.Active & Chr(9) & _
                       strDate

            Try
                intErrNo = objCMSetup.mtdUpdExchangeRate(strOpCdGet, _
                                                        strOpCdAdd, _
                                                        strOpCdUpd, _
                                                        strCompany, _
                                                        strLocation, _
                                                        strUserId, _
                                                        strParam, _
                                                        blnDupKey, _
                                                        blnIsUpdate)

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_SETUP_EXCHANGERATE_SAVE&errmesg=" & lblErrMessage.Text & "&redirect=CM/Setup/CM_setup_ExchangeRateDet.aspx?tbcode=" & strCompositKey)
            End Try

            If blnDupKey = True Then
                lblErrDup.Text = "Exchange Rate already existed. "
                lblErrDup.Visible = True
                Exit Sub
            End If

            If strFirstCurr = strSecCurr And CDec(strRate) <> 1 Then
                lblErrDup.Text = "Invalid exchange rate. "
                lblErrDup.Visible = True
                Exit Sub
            End If

        ElseIf strCmdArgs = "Del" Then
            strParam = strFirstCurr & Chr(9) & _
                       strSecCurr & Chr(9) & _
                       strRate & Chr(9) & _
                       objCMSetup.EnumExchangeRateStatus.Deleted & Chr(9) & _
                       strDate
            Try
                intErrNo = objCMSetup.mtdUpdExchangeRate(strOpCdGet, _
                                                        strOpCdAdd, _
                                                        strOpCdUpd, _
                                                        strCompany, _
                                                        strLocation, _
                                                        strUserId, _
                                                        strParam, _
                                                        blnDupKey, _
                                                        True)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_SETUP_EXCHANGERATE_DEL&errmesg=" & lblErrMessage.Text & "&redirect=CM/Setup/CM_setup_ExchangeRateDet.aspx?tbcode=" & strCompositKey)
            End Try
            lblHiddenSts.Text = objCMSetup.EnumExchangeRateStatus.Deleted
        ElseIf strCmdArgs = "UnDel" Then

            strParam = strFirstCurr & Chr(9) & _
                       strSecCurr & Chr(9) & _
                       strRate & Chr(9) & _
                       objCMSetup.EnumExchangeRateStatus.Active & Chr(9) & _
                       strDate
            Try
                intErrNo = objCMSetup.mtdUpdExchangeRate(strOpCdGet, _
                                                        strOpCdAdd, _
                                                        strOpCdUpd, _
                                                        strCompany, _
                                                        strLocation, _
                                                        strUserId, _
                                                        strParam, _
                                                        blnDupKey, _
                                                        True)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_SETUP_EXCHANGERATE_UNDEL&errmesg=" & lblErrMessage.Text & "&redirect=CM/Setup/CM_setup_ExchangeRateDet.aspx?tbcode=" & strCompositKey)
            End Try
            lblHiddenSts.Text = objCMSetup.EnumExchangeRateStatus.Active
        End If

        strCompositCode = strFirstCurr & strSecCurr
        strCompositDate = strDate

        onLoad_Display()
        onLoad_BindButton()

    End Sub

    Sub onLoad_Display()
        Dim strParam As String        
        Dim intErrNo As Integer
        Dim strSearch As String
        Dim objCurrDs As New Object()
        
        strSearch = "and rtrim(exc.FirstCurrencyCode) + rtrim(exc.SecCurrencyCode) = '" & strCompositCode & "' and exc.TransDate = '" & strCompositDate & "' "
        strParam = strSearch & "|" & ""

        Try
            intErrNo = objCMSetup.mtdGetMasterList(strOpCdGet, strParam, 0, objCurrDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_SETUP_EXCHANGERATE_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Setup/CM_setup_ExchangeRateDet.aspx")
        End Try


        If objCurrDs.Tables(0).rows.Count > 0 Then
            ddlFirstCurr.SelectedValue = Trim(objCurrDs.Tables(0).Rows(0).Item("FirstCurrencyCode"))
            ddlSecCurr.SelectedValue = Trim(objCurrDs.Tables(0).Rows(0).Item("SecCurrencyCode"))
            txtExchangeRate.Text = objCurrDs.Tables(0).Rows(0).Item("ExchangeRate")
            lblHiddenSts.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("Status"))
            lblStatus.Text = objCMSetup.mtdGetExchangeRateStatus(Trim(objCurrDs.Tables(0).Rows(0).Item("Status")))
            lblDateCreated.Text = objGlobal.GetLongDate(objCurrDs.Tables(0).Rows(0).Item("CreateDate"))
            lblLastUpdate.Text = objGlobal.GetLongDate(objCurrDs.Tables(0).Rows(0).Item("UpdateDate"))
            lblUpdatedBy.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("UserName"))
            txtdate.Text = Date_Validation(objCurrDs.Tables(0).Rows(0).Item("TransDate"), True)
        End If


    End Sub

    Sub onLoad_BindButton()
        SaveBtn.Visible = False
        DelBtn.Visible = False
        UnDelBtn.Visible = False
      
        Select Case lblHiddenSts.text
            Case objCMSetup.EnumExchangeRateStatus.Active
                ddlFirstCurr.Enabled = False
                ddlSecCurr.Enabled = False
                SaveBtn.Visible = True
                DelBtn.Visible = True
                DelBtn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                txtdate.Enabled = False
                btnSelDate.Visible = False
            Case objCMSetup.EnumExchangeRateStatus.Deleted
                ddlFirstCurr.Enabled = False
                ddlSecCurr.Enabled = False
                UnDelBtn.Visible = True
                txtdate.Enabled = False
                btnSelDate.Visible = False
            Case Else
                ddlFirstCurr.Enabled = True
                ddlSecCurr.Enabled = True
                SaveBtn.Visible = True
                txtdate.Enabled = True
        End Select        
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

    Sub NewBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CM_setup_ExchangeRateDet.aspx")
    End Sub
End Class
