Imports System
Imports System.Data
Imports System.Collections 
Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic.Interaction


Public Class CB_trx_SaldoBank_list : Inherits Page

    Protected WithEvents dgList As DataGrid
    Protected WithEvents dgTrx As DataGrid
    Protected WithEvents lblTracker As Label
    Protected WithEvents lblErrMessage As Label

    Protected WithEvents srcTgl As TextBox
    Protected WithEvents ddlLocation As DropDownList
    Protected WithEvents ddlDocStatus As DropDownList
    Protected WithEvents lbAccCode As LinkButton

    Protected WithEvents srchUpdateBy As TextBox
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents srchStatusList As DropDownList
    Protected WithEvents lblBlkCode As Label
    Protected WithEvents lblVehCode As Label

    Protected WithEvents txtDateFrom As TextBox
    Protected WithEvents txtDateTo As TextBox
    Protected WithEvents hidInit As HtmlInputHidden

    Protected WithEvents btnCalculate As ImageButton
    Protected WithEvents btnSave As ImageButton
    Protected WithEvents btnClear As ImageButton

    Protected objGLTrx As New agri.GL.ClsTrx()
    Protected objGLSetup As New agri.GL.clsSetup()
    Dim objLangCap As New agri.PWSystem.clsLangCap()

    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objPU As New agri.PU.clsTrx()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intGLAR As Integer
    Dim strLocType As String
    Dim strLocLevel As String
    Dim strSelAccMonth As String
    Dim strSelAccYear As String

    Dim objBudgetDs As New DataSet()
    Dim objLangCapDs As New Object()

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strAccMonth = Session("SS_GLACCMONTH")
        strAccYear = Session("SS_GLACCYEAR")
        strLangCode = Session("SS_LANGCODE")
        intGLAR = Session("SS_CBAR")
        strLocType = Session("SS_LOCTYPE")
        strLocLevel = Session("SS_LOCLEVEL")
        strSelAccMonth = Session("SS_SELACCMONTH")
        strSelAccYear = Session("SS_SELACCYEAR")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCBAccessRights.CBInterAdj), intGLAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            If SortExpression.Text = "" Then
                SortExpression.Text = "AccCode"
            End If

            onload_GetLangCap()

            If Not Page.IsPostBack Then
                srcTgl.Text = "1/" & strSelAccMonth & "/" & strSelAccYear 'Date_Validation(DateTime.Now, True)
                txtDateFrom.Text = srcTgl.Text
                txtDateTo.Text = DateAdd(DateInterval.Month, 1, CDate(strSelAccMonth & "/1/" & strSelAccYear))
                txtDateTo.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), DateAdd(DateInterval.Day, -1, CDate(txtDateTo.Text)))

                BindLocation(strLocation)
                BindGrid()
            End If

            btnSave.Attributes("onclick") = "javascript:return ConfirmAction('save these bank assignment');"
            btnClear.Attributes("onclick") = "javascript:return ConfirmAction('clear these all bank assignment');"
        End If
    End Sub

    Sub BindLocation(ByVal pv_strLocCode As String)
        Dim strOpCd As String = "ADMIN_CLSLOC_LOCATION_LIST_GET"
        Dim objLocDs As New Object()
        Dim strPRRefLocCode As String
        Dim strParam As String = ""
        Dim intCnt As Integer = 0
        Dim intErrNo As Integer
        Dim dr As DataRow
        Dim intSelectedIndex As Integer

        strPRRefLocCode = IIf(pv_strLocCode = "", "", pv_strLocCode)
        strParam = strPRRefLocCode & "|" & objAdminLoc.EnumLocStatus.Active & "|LocCode|"

        Try
            intErrNo = objPU.mtdGetLoc(strOpCd, strParam, objLocDs, "")

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PU_GET_LOC&errmesg=" & lblErrMessage.Text & "&redirect=pu/trx/pu_trx_polist.aspx")
        End Try

        For intCnt = 0 To objLocDs.Tables(0).Rows.Count - 1
            With objLocDs.Tables(0).Rows(intCnt)
                .Item("LocCode") = Trim(.Item("LocCode"))
                .Item("Description") = Trim(.Item("LocCode")) & " (" & Trim(.Item("Description")) & ")"
                If .Item("LocCode") = Trim(strPRRefLocCode) Then intSelectedIndex = intCnt + 1
            End With
        Next intCnt

        dr = objLocDs.Tables(0).NewRow()
        dr("LocCode") = ""
        dr("Description") = "-All-"
        objLocDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlLocation.DataSource = objLocDs.Tables(0)
        ddlLocation.DataValueField = "LocCode"
        ddlLocation.DataTextField = "Description"
        ddlLocation.DataBind()
        ddlLocation.SelectedIndex = intSelectedIndex
    End Sub

    Sub dgTrx_BindGrid(ByVal sender As Object, ByVal e As DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor='lightblue'")
            If e.Item.ItemType = ListItemType.AlternatingItem Then
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='e9e9e9'")
            Else
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='f2f2f2'")
            End If
        End If
    End Sub

    Sub dgList_BindGrid(ByVal sender As Object, ByVal e As DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor='lightblue'")
            If e.Item.ItemType = ListItemType.AlternatingItem Then
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='e9e9e9'")
            Else
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='f2f2f2'")
            End If
        End If
    End Sub

    Private Sub dgList_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgList.ItemCreated
        If e.Item.ItemType = ListItemType.Header Then
            Dim dgItem As DataGridItem
            Dim dgCell As TableCell
            dgItem = New DataGridItem(0, 0, ListItemType.Header)

            dgCell = New TableCell()
            dgCell.RowSpan = 2
            dgItem.Cells.Add(dgCell)
            dgCell.Text = "Bank/Cash"
            dgCell.HorizontalAlign = HorizontalAlign.Center

            dgCell = New TableCell()
            dgCell.RowSpan = 2
            dgItem.Cells.Add(dgCell)
            dgCell.Text = "Bank AC No."
            dgCell.HorizontalAlign = HorizontalAlign.Center

            dgCell = New TableCell()
            dgCell.RowSpan = 2
            dgItem.Cells.Add(dgCell)
            dgCell.Text = "COA"
            dgCell.HorizontalAlign = HorizontalAlign.Center

            dgCell = New TableCell()
            dgCell.ColumnSpan = 3
            dgItem.Cells.Add(dgCell)
            dgCell.Text = "Balance"
            dgCell.HorizontalAlign = HorizontalAlign.Center

            dgItem.Font.Bold = True
            dgList.Controls(0).Controls.AddAt(0, dgItem)
        End If
    End Sub

    Private Sub dgList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgList.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            Dim dgItem As DataGridItem

            dgItem = New DataGridItem(0, 0, ListItemType.Header)

            e.Item.Cells(0).Visible = False
            e.Item.Cells(1).Visible = False
            e.Item.Cells(2).Visible = False
        End If

    End Sub

    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        txtDateFrom.Text = srcTgl.Text
        dgList.CurrentPageIndex = 0
        dgList.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub BindGrid()
        Dim dsData As DataSet

        dsData = LoadData()
        dgList.DataSource = dsData.Tables(0)
        dgList.DataBind()
        dgTrx.DataSource = dsData.Tables(1)
        dgTrx.DataBind()
        BindBankList()
    End Sub

    Sub BindGridSaldo()
        Dim dsData As DataSet

        dsData = LoadDataSaldo()
        dgList.DataSource = dsData
        dgList.DataBind()
    End Sub

    Sub srchTrxBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        dgTrx.CurrentPageIndex = 0
        dgTrx.EditItemIndex = -1
        BindGridTrx()
    End Sub

    Sub BindGridTrx()
        Dim dsData As DataSet

        dsData = LoadDataTrx()
        dgTrx.DataSource = dsData
        dgTrx.DataBind()
        BindBankList()
    End Sub

    Sub BindBankList()
        Dim strOpCodeBank As String = "HR_CLSSETUP_BANK_LOCATION_GET_ONLY"
        Dim objBankDs As New DataSet()
        Dim ddlBankCode As DropDownList
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer
        Dim intCnt As Integer

        strParamName = "LOCCODE"
        strParamValue = strLocation

        Try
            intErrNo = objGLTrx.mtdGetDataCommon(strOpCodeBank, _
                                                strParamName, _
                                                strParamValue, _
                                                objBankDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ID_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        Dim dr As DataRow
        dr = objBankDs.Tables(0).NewRow()
        dr("BankCode") = ""
        dr("_Description") = "-All-"
        objBankDs.Tables(0).Rows.InsertAt(dr, 0)


        For intCnt = 0 To dgTrx.Items.Count - 1
            ddlBankCode = dgTrx.Items.Item(CInt(intCnt)).FindControl("ddlBankCode")
            ddlBankCode.DataSource = objBankDs.Tables(0)
            ddlBankCode.DataValueField = "BankCode"
            ddlBankCode.DataTextField = "_Description"
            ddlBankCode.DataBind()
        Next
    End Sub

    Function Date_Validation(ByVal pv_strInputDate As String, ByVal pv_blnIsShortDate As Boolean) As String
        Dim objSysCfgDs As New Object
        Dim objActualDate As New Object
        Dim strDateFormat As String
        Dim strParam As String
        Dim intErrNo As Integer
        Dim strAcceptFormat As String

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

    Protected Function LoadData() As DataSet
        Dim strOpCd As String = "HR_CLSSETUP_BANK_LOCATION_GET_BALANCE"
        Dim dsResult As New Object
        Dim strSrchNamaBank As String
        Dim strSrchLoc As String
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer
        Dim strDate As String = Date_Validation(srcTgl.Text, False)
        Dim strDateFrom As String = Date_Validation(txtDateFrom.Text, False)
        Dim strDateTo As String = Date_Validation(txtDateTo.Text, False)

        If hidInit.Value = "" Then
            strParamName = "LOCCODE|ACCYEAR|ACCMONTH|TRXDATEFROM|TRXDATETO|ACCCODE|USERID|DOCSTATUS"
            strParamValue = strLocation & "|" & strAccYear & "|" & strAccMonth & "|" & strDate & "|" & strDateTo & "||" & strUserId & "|" & ddlDocStatus.SelectedItem.Value

            Try
                intErrNo = objGLTrx.mtdGetDataCommon(strOpCd, _
                                                    strParamName, _
                                                    strParamValue, _
                                                    dsResult)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BANK_BALANCE&errmesg=" & lblErrMessage.Text & "&redirect=gl/trx/gl_trx_budget_list.aspx")
            End Try

        Else
            strOpCd = "HR_CLSSETUP_BANK_LOCATION_GET_SALDO"

            strParamName = "LOCCODE|USERID"
            strParamValue = strLocation & "|" & strUserId

            Try
                intErrNo = objGLTrx.mtdGetDataCommon(strOpCd, _
                                                    strParamName, _
                                                    strParamValue, _
                                                    dsResult)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BANK_BALANCE&errmesg=" & lblErrMessage.Text & "&redirect=gl/trx/gl_trx_budget_list.aspx")
            End Try

        End If

        Return dsResult

    End Function

    Protected Function LoadDataSaldo() As DataSet
        Dim strOpCd As String = "HR_CLSSETUP_BANK_LOCATION_GET_SALDO"
        Dim dsResult As New Object
        Dim strSrchNamaBank As String
        Dim strSrchLoc As String
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer
        Dim strDate As String = Date_Validation(srcTgl.Text, False)
        Dim strDateFrom As String = Date_Validation(txtDateFrom.Text, False)
        Dim strDateTo As String = Date_Validation(txtDateTo.Text, False)

        strParamName = "LOCCODE|USERID"
        strParamValue = strLocation & "|" & strUserId

        Try
            intErrNo = objGLTrx.mtdGetDataCommon(strOpCd, _
                                                strParamName, _
                                                strParamValue, _
                                                dsResult)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BANK_BALANCE&errmesg=" & lblErrMessage.Text & "&redirect=gl/trx/gl_trx_budget_list.aspx")
        End Try

        Return dsResult

    End Function

    Protected Function LoadDataTrx() As DataSet
        Dim strOpCd As String = "HR_CLSSETUP_BANK_LOCATION_GET_TRX"
        Dim dsResult As New Object
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim strDateFrom As String = Date_Validation(txtDateFrom.Text, False)
        Dim strDateTo As String = Date_Validation(txtDateTo.Text, False)


        strParamName = "LOCCODE|ACCYEAR|ACCMONTH|TRXDATEFROM|TRXDATETO|ACCCODE|USERID"
        strParamValue = strLocation & "|" & strAccYear & "|" & strAccMonth & "|" & strDateFrom & "|" & strDateTo & "||" & strUserId

        Try
            intErrNo = objGLTrx.mtdGetDataCommon(strOpCd, _
                                                strParamName, _
                                                strParamValue, _
                                                dsResult)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BANK_BALANCE&errmesg=" & lblErrMessage.Text & "&redirect=gl/trx/gl_trx_budget_list.aspx")
        End Try

        Return dsResult

    End Function

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIf(SortCol.Text = "ASC", "DESC", "ASC")
        dgList.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
    End Sub

    Sub onload_GetLangCap()

        GetEntireLangCap()

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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PU_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=gl/setup/gl_setup_subblock.aspx")
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

    Sub btnCalculate_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim strOpCodeClear As String = "HR_CLSSETUP_BANK_LOCATION_TRX_CLEAR"
        Dim strOpCodeBank As String = "HR_CLSSETUP_BANK_LOCATION_TRX_UPD"
        Dim objBankDs As New DataSet()
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim strDateFrom As String = Date_Validation(txtDateFrom.Text, False)
        Dim strDateTo As String = Date_Validation(txtDateTo.Text, False)

        Dim lbl As Label
        Dim ddlBankCode As DropDownList
        Dim strBank As String
        Dim strBankAccCode As String
        Dim strBankAccNo As String
        Dim arrParam As Array

        hidInit.Value = "1"


        strParamName = "LOCCODE|USERID"
        strParamValue = strLocation & "|" & Trim(strUserId)

        Try
            intErrNo = objGLTrx.mtdInsertDataCommon(strOpCodeClear, _
                                                    strParamName, _
                                                    strParamValue)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BANK_BALANCE&errmesg=" & Exp.ToString() & "&redirect=")
        End Try


        For intCnt = 0 To dgTrx.Items.Count - 1
            lbl = dgTrx.Items.Item(CInt(intCnt)).FindControl("lblDocID")
            ddlBankCode = dgTrx.Items.Item(CInt(intCnt)).FindControl("ddlBankCode")

            If Trim(ddlBankCode.SelectedItem.Value) <> "" Then
                arrParam = Split(Trim(ddlBankCode.SelectedItem.Value), "|")
                strBank = Trim(arrParam(0))
                strBankAccCode = Trim(arrParam(1))
                strBankAccNo = Trim(arrParam(2))

                strParamName = "LOCCODE|BANKCODE|BANKACCNO|DOCID|USERID"
                strParamValue = strLocation & "|" & Trim(strBank) & "|" & Trim(strBankAccNo) & "|" & Trim(lbl.Text) & "|" & Trim(strUserId)

                Try
                    intErrNo = objGLTrx.mtdInsertDataCommon(strOpCodeBank, _
                                                            strParamName, _
                                                            strParamValue)
                Catch Exp As System.Exception
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BANK_BALANCE&errmesg=" & Exp.ToString() & "&redirect=")
                End Try
            End If
        Next

        BindGridSaldo()
    End Sub

    Sub btnSave_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim strOpCodeBank As String
        Dim objBankDs As New DataSet()
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim strDateFrom As String = Date_Validation(txtDateFrom.Text, False)
        Dim strDateTo As String = Date_Validation(txtDateTo.Text, False)

        Dim lbl As Label
        Dim lblType As Label
        Dim ddlBankCode As DropDownList
        Dim strBank As String
        Dim strBankAccCode As String
        Dim strBankAccNo As String
        Dim arrParam As Array

        For intCnt = 0 To dgTrx.Items.Count - 1
            lbl = dgTrx.Items.Item(CInt(intCnt)).FindControl("lblDocID")
            lblType = dgTrx.Items.Item(CInt(intCnt)).FindControl("lblDocType")
            ddlBankCode = dgTrx.Items.Item(CInt(intCnt)).FindControl("ddlBankCode")

            If Trim(ddlBankCode.SelectedItem.Value) <> "" Then
                arrParam = Split(Trim(ddlBankCode.SelectedItem.Value), "|")
                strBank = Trim(arrParam(0))
                strBankAccCode = Trim(arrParam(1))
                strBankAccNo = Trim(arrParam(2))

                If Trim(lblType.Text) = "CASHBANK" Then
                    strOpCodeBank = "CB_CLSTRX_CASHBANK_DETAIL_UPD"
                    strParamName = "UPDATESTR"
                    strParamValue = "SET BankCode='" & Trim(strBank) & "', BankAccNo='" & Trim(strBankAccNo) & "', " & _
                                    "UpdateID='" & Trim(strUserId) & "', UpdateDate=GETDATE() " & _
                                    "WHERE LocCode='" & strLocation & "' AND CashBankID='" & Trim(lbl.Text) & "'"
                Else
                    strOpCodeBank = "CB_CLSTRX_PAYMENT_DETAIL_UPD"
                    strParamName = "UPDATESTR"
                    strParamValue = "SET BankCode='" & Trim(strBank) & "', BankAccNo='" & Trim(strBankAccNo) & "', " & _
                                    "UpdateID='" & Trim(strUserId) & "', UpdateDate=GETDATE() " & _
                                    "WHERE LocCode='" & strLocation & "' AND PaymentID='" & Trim(lbl.Text) & "'"
                End If

                Try
                    intErrNo = objGLTrx.mtdInsertDataCommon(strOpCodeBank, _
                                                            strParamName, _
                                                            strParamValue)
                Catch Exp As System.Exception
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BANK_BALANCE&errmesg=" & Exp.ToString() & "&redirect=")
                End Try
            End If
        Next
    End Sub

    Sub btnClear_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim strOpCodeClear As String = "HR_CLSSETUP_BANK_LOCATION_TRX_CLEAR"
        Dim objBankDs As New DataSet()
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim strDateFrom As String = Date_Validation(txtDateFrom.Text, False)
        Dim strDateTo As String = Date_Validation(txtDateTo.Text, False)

        Dim lbl As Label
        Dim ddlBankCode As DropDownList
        Dim strBank As String
        Dim strBankAccCode As String
        Dim strBankAccNo As String
        Dim arrParam As Array

        hidInit.Value = ""

        strParamName = "LOCCODE|USERID"
        strParamValue = strLocation & "|" & Trim(strUserId)

        Try
            intErrNo = objGLTrx.mtdInsertDataCommon(strOpCodeClear, _
                                                    strParamName, _
                                                    strParamValue)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BANK_BALANCE&errmesg=" & Exp.ToString() & "&redirect=")
        End Try

        BindGridSaldo()
    End Sub

    Sub btnPreview_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)

        'strCompany = ddlLocation.SelectedIndex

        'Response.Write("<Script Language=""JavaScript"">window.open(""CB_trx_SaldoBankPrintDocs.aspx?doctype=1&CompName=" & strCompany & _
        '               "&tgl=" & srcTgl.Text & _
        '               "&NmBank=" & ddlBank.SelectedItem.Value & _
        '               """,null ,""status=yes, height=400, width=600, top=180, left=220, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")

    End Sub


    Sub dgList_OnCommand(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim lbl As Label
        Dim intIndex As Integer = E.Item.ItemIndex
        Dim strCOA As String
        Dim strDbase As String = ""
        Dim strLoc As String

        lbl = dgList.Items.Item(intIndex).FindControl("lblAccCode")
        strCOA = lbl.Text
        lbl = dgList.Items.Item(intIndex).FindControl("lblComp")
        strCompany = lbl.Text
        lbl = dgList.Items.Item(intIndex).FindControl("lblLoc")
        strLoc = lbl.Text

        Dim strSdate As String
        strSdate = "01/" & Mid(srcTgl.Text, 4, 3) & Right(srcTgl.Text, 4)


        Response.Write("<Script Language=""JavaScript"">window.open(""../reports/CB_Rpt_SaldoBankDet_Prev.aspx?doctype=1&strDbase=" & strDbase & _
                       "&Location=" & strLoc & _
                       "&Sdate=" & Date_Validation(strSdate, False) & _
                       "&Edate=" & Date_Validation(srcTgl.Text, False) & _
                       "&COA=" & Trim(strCOA) & _
                       """,null ,""status=yes, height=400, width=600, top=180, left=220, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")

    End Sub

   




End Class
