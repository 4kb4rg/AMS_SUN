
Imports System
Imports System.Data
Imports System.Collections 
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic


Public Class CM_setup_ExchangeRate : Inherits Page

    Protected WithEvents EventData As DataGrid
    Protected WithEvents ErrorMessage As Label
    Protected WithEvents lblTracker As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents StatusList As DropDownList
    Protected WithEvents SortExpression As Label
    Protected WithEvents blnUpdate As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblDupMsg As Label
    Protected WithEvents sortcol As Label
    Protected WithEvents srch1CurrencyCode As TextBox
    Protected WithEvents srch2CurrencyCode As TextBox
    Protected WithEvents srchUpdBy As TextBox
    Protected WithEvents srchStatus As DropDownList
    Protected WithEvents srch1TransDate As TextBox
    Protected WithEvents lblErrDate As Label
    Protected WithEvents lblErrDateMsg As Label

    Protected objCMSetup As New agri.CM.clsSetup()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objSysCfg As New agri.PWSystem.clsConfig()

    Dim strOpCdGet As String = "CM_CLSSETUP_EXCHANGERATE_GET"
    Dim strOpCdAdd As String = "CM_CLSSETUP_EXCHANGERATE_ADD"
    Dim strOpCdUpd As String = "CM_CLSSETUP_EXCHANGERATE_UPD"
    Dim objCurrDs As New Object()

    Dim intErrNo As Integer
    Dim strParam As String = ""

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
            lblErrDateMsg.Visible = False
            lblErrDate.Visible = False

            If SortExpression.Text = "" Then
                SortExpression.Text = "TransDate desc, FirstCurrencyCode"
                sortcol.Text = "asc"
            End If
            If Not Page.IsPostBack Then
                BindSearchList()
                BindGrid()
                BindPageList()
            End If
        End If
    End Sub

    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim indDate As String = ""

        If CheckDate(srch1TransDate.Text.Trim(), indDate) = False Then
            lblErrDateMsg.Visible = True
            lblErrDate.Visible = True
            lblErrDateMsg.Text = "<br>Date Entered should be in the format"
            Exit Sub
        End If

        EventData.CurrentPageIndex = 0
        EventData.EditItemIndex = -1
        BindGrid()
        BindPageList()
    End Sub

    Sub BindGrid() 
        Dim PageNo As Integer
        Dim PageCount As Integer
        Dim dsData As DataSet
         
        Dim intCnt As Integer
        Dim lbl As Label
        Dim lbButton As LinkButton

        dsData = LoadData()
        PageCount = objGlobal.mtdGetPageCount(dsData.Tables(0).Rows.Count, EventData.PageSize)

        EventData.DataSource = dsData
        If EventData.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                EventData.CurrentPageIndex = 0
            Else
                EventData.CurrentPageIndex = PageCount - 1
            End If
        End If

        EventData.DataBind()
        BindPageList()
        PageNo = EventData.CurrentPageIndex + 1
        lblTracker.Text = "Page " & PageNo & " of " & EventData.PageCount
        For intCnt = 0 to EventData.Items.Count - 1
            lbl = EventData.Items.Item(intCnt).FindControl("lblStatus")
            Select Case CInt(Trim(lbl.Text))
                Case objCMSetup.EnumExchangeRateStatus.Active
                    lbButton = EventData.Items.Item(intCnt).FindControl("lblDelete")
                    lbButton.visible = True
                    lbButton.attributes("onclick") = "javascript:return ConfirmAction('delete');"
                Case Else
                    lbButton = EventData.Items.Item(intCnt).FindControl("lblDelete")
                    lbButton.visible = False
            End Select
        Next
    End Sub

    Sub BindPageList()
        Dim count As Integer = 1
        Dim arrDList As New ArrayList()

        While Not count = EventData.PageCount + 1
            arrDList.Add("Page " & count)
            count = count + 1
        End While
        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = EventData.CurrentPageIndex
    End Sub

    Sub BindStatusList(ByVal index As Integer)
        StatusList = EventData.Items.Item(index).FindControl("StatusList")
        StatusList.Items.Add(New ListItem(objCMSetup.mtdGetExchangeRateStatus(objCMSetup.EnumExchangeRateStatus.Active), objCMSetup.EnumExchangeRateStatus.Active))
        StatusList.Items.Add(New ListItem(objCMSetup.mtdGetExchangeRateStatus(objCMSetup.EnumExchangeRateStatus.Deleted), objCMSetup.EnumExchangeRateStatus.Deleted))
    End Sub

    Sub BindSearchList()
        srchStatus.Items.Add(New ListItem(objCMSetup.mtdGetExchangeRateStatus(objCMSetup.EnumExchangeRateStatus.All), objCMSetup.EnumExchangeRateStatus.All))
        srchStatus.Items.Add(New ListItem(objCMSetup.mtdGetExchangeRateStatus(objCMSetup.EnumExchangeRateStatus.Active), objCMSetup.EnumExchangeRateStatus.Active))
        srchStatus.Items.Add(New ListItem(objCMSetup.mtdGetExchangeRateStatus(objCMSetup.EnumExchangeRateStatus.Deleted), objCMSetup.EnumExchangeRateStatus.Deleted))
        srchStatus.SelectedIndex = 1
    End Sub

    Protected Function LoadData() As DataSet
        Dim strSearch As String = ""
        Dim strSort As String = ""
        Dim strParam As String
        Dim intCnt as Integer

        If Trim(srch1TransDate.Text) <> "" Then
            strSearch = strSearch & "and exc.TransDate = '" & Date_Validation(srch1TransDate.Text, False) & "' "
        End If

        If Trim(srch1CurrencyCode.Text) <> "" Then
            strSearch = strSearch & "and exc.FirstCurrencyCode like '" & Trim(srch1CurrencyCode.Text) & "%' "
        End If

        If Trim(srch2CurrencyCode.Text) <> "" Then
            strSearch = strSearch & "and exc.SecCurrencyCode like '" & Trim(srch2CurrencyCode.Text) & "%' "
        End If

        If srchStatus.SelectedItem.Value <> CInt(objCMSetup.EnumExchangeRateStatus.All) Then
            strSearch = strSearch & "and exc.Status = " & srchStatus.SelectedItem.Value '" & srchStatus.SelectedItem.Value & "%' " 
        End If

        If Trim(srchUpdBy.Text) <> "" Then
            strSearch = strSearch & "and usr.UserName like '" & Trim(srchUpdBy.Text) & "%' "
        End If

        strSort = "order by " & Trim(SortExpression.Text) & " " & sortcol.Text
        strParam = strSearch & "|" & strSort

        Try
            intErrNo = objCMSetup.mtdGetMasterList(strOpCdGet, strParam, 0, objCurrDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_SETUP_EXCHANGERATE_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Setup/CM_setup_ExchangeRate.aspx")
        End Try

        If objCurrDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objCurrDs.Tables(0).Rows.Count - 1
                objCurrDs.Tables(0).Rows(intCnt).Item("FirstCurrencyCode") = Trim(objCurrDs.Tables(0).Rows(intCnt).Item("FirstCurrencyCode"))
                objCurrDs.Tables(0).Rows(intCnt).Item("SecCurrencyCode") = Trim(objCurrDs.Tables(0).Rows(intCnt).Item("SecCurrencyCode"))
                objCurrDs.Tables(0).Rows(intCnt).Item("ExchangeRate") = Trim(objCurrDs.Tables(0).Rows(intCnt).Item("ExchangeRate"))
                objCurrDs.Tables(0).Rows(intCnt).Item("Status") = Trim(objCurrDs.Tables(0).Rows(intCnt).Item("Status"))
                objCurrDs.Tables(0).Rows(intCnt).Item("UpdateID") = Trim(objCurrDs.Tables(0).Rows(intCnt).Item("UpdateID"))
                objCurrDs.Tables(0).Rows(intCnt).Item("UserName") = Trim(objCurrDs.Tables(0).Rows(intCnt).Item("UserName"))
            Next
        End If

        Return objCurrDs
    End Function

    Sub btnPrevNext_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).CommandArgument
        Select Case direction
            Case "first"
                EventData.CurrentPageIndex = 0
            Case "prev"
                EventData.CurrentPageIndex = _
                    Math.Max(0, EventData.CurrentPageIndex - 1)
            Case "next"
                EventData.CurrentPageIndex = _
                    Math.Min(EventData.PageCount - 1, EventData.CurrentPageIndex + 1)
            Case "last"
                EventData.CurrentPageIndex = EventData.PageCount - 1
        End Select
        lstDropList.SelectedIndex = EventData.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            EventData.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        EventData.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        sortcol.Text = IIf(sortcol.Text = "asc", "desc", "asc")
        EventData.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
    End Sub

    
    Sub DEDR_Add(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        response.redirect("CM_setup_ExchangeRateDet.aspx")
    End Sub

 Sub DEDR_Edit(Sender As Object, E As DataGridCommandEventArgs)
        Dim FirstCurr As LinkButton
        Dim SecCurr As LinkButton
        Dim TransDate As LinkButton

        FirstCurr = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("btnFirstCurrency")
        SecCurr = EventData.Items.Item(CInt(E.Item.ItemIndex)).FindControl("btnSecCurrency")
        TransDate = EventData.Items.Item(CInt(E.Item.ItemIndex)).FindControl("btnTransDate")

        Response.Redirect("CM_setup_ExchangeRateDet.aspx?tbCode=" & FirstCurr.Text.Trim & SecCurr.Text.Trim & objGlobal.GetLongDate(TransDate.Text.Trim))
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal e As DataGridCommandEventArgs)
        Dim blnDupKey As Boolean = False
        Dim FirstCurr As LinkButton
        Dim SecCurr As LinkButton
        Dim ExchangeRate as LinkButton
        Dim TransDate As LinkButton

        FirstCurr = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("btnFirstCurrency")
        SecCurr= EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("btnSecCurrency")
        TransDate = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("btnTransDate")

        strParam = FirstCurr.Text.Trim & Chr(9) & _
                   SecCurr.Text.Trim & Chr(9) & Chr(9) & _
                   objCMSetup.EnumExchangeRateStatus.Deleted & Chr(9) & _
                   TransDate.Text.Trim

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
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_SETUP_EXCHANGERATE_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Setup/CM_setup_ExchangeRate.aspx")
            End Try

        EventData.EditItemIndex = -1
        BindGrid() 

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

    Function CheckDate(ByVal pv_strDate As String, ByRef pr_strDate As String) As Boolean
        Dim strDateFormatCode As String = Session("SS_DATEFMT")
        Dim strDateFormat As String

        pr_strDate = ""
        CheckDate = True
        If Not pv_strDate = "" Then
            If objGlobal.mtdValidInputDate(strDateFormatCode, pv_strDate, strDateFormat, pr_strDate) = False Then
                lblErrDate.Text = strDateFormat
                pr_strDate = ""
                CheckDate = False
            End If
        End If
    End Function
End Class
