Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic
Imports System.Xml
Imports System.Web.Services

Public Class AP_StdRpt_Supplier_AgeingList : Inherits Page

    Protected RptSelect As UserControl
    Protected WithEvents lblTracker As Label


    Protected WithEvents txtSupplierCode As TextBox

    Protected WithEvents ddlSrchAccMonthFrom As DropDownList
    Protected WithEvents ddlSrchAccYearFrom As DropDownList
    'Protected WithEvents ddlSrchAccMonthTo As DropDownList
    'Protected WithEvents ddlSrchAccYearTo As DropDownList
    Protected WithEvents ddlSupplier As DropDownList

    Protected WithEvents PrintPrev As ImageButton
    Protected WithEvents cbExcel As CheckBox


    Protected WithEvents txtFromAge1 As TextBox
    Protected WithEvents txtToAge1 As TextBox
    Protected WithEvents txtFromAge2 As TextBox
    Protected WithEvents txtToAge2 As TextBox
    Protected WithEvents txtFromAge3 As TextBox
    Protected WithEvents txtToAge3 As TextBox
    Protected WithEvents txtFromAge4 As TextBox
    Protected WithEvents txtToAge4 As TextBox
    Protected WithEvents txtFromAge5 As TextBox

    Protected WithEvents lblToAge1 As Label
    Protected WithEvents lblFromAge2 As Label
    Protected WithEvents lblToAge2 As Label
    Protected WithEvents lblFromAge3 As Label
    Protected WithEvents lblToAge3 As Label
    Protected WithEvents lblFromAge4 As Label
    Protected WithEvents lblToAge4 As Label
    Protected WithEvents lblFromAge5 As Label

    Protected WithEvents lblErrToAge1 As Label
    Protected WithEvents lblErrFromAge2 As Label
    Protected WithEvents lblErrToAge2 As Label
    Protected WithEvents lblErrFromAge3 As Label
    Protected WithEvents lblErrToAge3 As Label
    Protected WithEvents lblErrFromAge4 As Label
    Protected WithEvents lblErrToAge4 As Label
    Protected WithEvents lblErrFromAge5 As Label

    Dim TrMthYr As HtmlTableRow

    Dim objGL As New agri.GL.clsReport()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objGLSetup As New agri.GL.clsSetup()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objLangCapDs As New Object()
    Dim objAdmAcc As New agri.Admin.clsAccPeriod()
    Dim objGLtrx As New agri.GL.ClsTrx()
    Dim objPUSetup As New agri.PU.clsSetup()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intConfigsetting As Integer

    Dim dr As DataRow
    Dim intErrNo As Integer
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim strLocType As String


    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strAccMonth = Session("SS_GLACCMONTH")
        strAccYear = Session("SS_GLACCYEAR")
        strLangCode = Session("SS_LANGCODE")
        intConfigsetting = Session("SS_CONFIGSETTING")

        strLocType = Session("SS_LOCTYPE")
        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        Else
            onload_GetLangCap()
            If Not Page.IsPostBack Then
                BindAccMonthList(BindAccYearList(strLocation, strAccYear, True))
                BindAccMonthToList(BindAccYearList(strLocation, strAccYear, False))
                BindSupplier("")
            End If
        End If
    End Sub

    Protected Overloads Sub OnPreRender(ByVal Source As Object, ByVal E As EventArgs) Handles MyBase.PreRender
        Dim htmltr As HtmlTableRow

        htmltr = RptSelect.FindControl("TrMthYr")
        htmltr.Visible = False
    End Sub

    Sub BindAccMonthList(ByVal pv_intMaxMonth As Integer)
        Dim intCnt As Integer
        Dim intSelIndex As Integer = 0

        ddlSrchAccMonthFrom.Items.Clear()
        For intCnt = 1 To pv_intMaxMonth
            ddlSrchAccMonthFrom.Items.Add(intCnt)
            If intCnt = Convert.ToInt16(strAccMonth) Then
                intSelIndex = intCnt - 1
            End If
        Next
        ddlSrchAccMonthFrom.SelectedIndex = intSelIndex
    End Sub

    Function BindAccYearList(ByVal pv_strLocation As String, _
                             ByVal pv_strAccYear As String, _
                             ByVal pv_blnIsFrom As Boolean) As Integer
        Dim strOpCd_Max_Get As String = "ADMIN_CLSACCPERIOD_CONFIG_ALLLOC_MAXPERIOD_GET"
        Dim strOpCd_Dist_Get As String = "ADMIN_CLSACCPERIOD_CONFIG_ACCYEAR_DISTINCT_GET"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intAccYear As Integer
        Dim intMaxPeriod As Integer
        Dim intCnt As Integer
        Dim intSelIndex As Integer
        Dim objAccCfg As New DataSet()

        If pv_strLocation = "" Then
            pv_strLocation = strLocation
        Else
            If Left(pv_strLocation, 3) = "','" Then
                pv_strLocation = Right(pv_strLocation, Len(pv_strLocation) - 3)
            ElseIf Right(pv_strLocation, 3) = "','" Then
                pv_strLocation = Left(pv_strLocation, Len(pv_strLocation) - 3)
            ElseIf Left(pv_strLocation, 1) = "," Then
                pv_strLocation = Right(pv_strLocation, Len(pv_strLocation) - 1)
            ElseIf Right(pv_strLocation, 1) = "," Then
                pv_strLocation = Left(pv_strLocation, Len(pv_strLocation) - 1)
            End If
        End If

        Try
            strParam = "||"
            intErrNo = objAdmAcc.mtdGetAccPeriodCfg(strOpCd_Dist_Get, _
                                                    strCompany, _
                                                    pv_strLocation, _
                                                    strUserId, _
                                                    strParam, _
                                                    objAccCfg)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CT_STDRPT_CTRL_ACCCFG_DIST_GET&errmesg=" & Exp.ToString() & "&redirect=")
        End Try

        intSelIndex = 0
        If pv_blnIsFrom = True Then
            ddlSrchAccYearFrom.Items.Clear()
            'Else
            '    ddlSrchAccYearTo.Items.Clear()
        End If

        If objAccCfg.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objAccCfg.Tables(0).Rows.Count - 1
                If pv_blnIsFrom = True Then
                    ddlSrchAccYearFrom.Items.Add(objAccCfg.Tables(0).Rows(intCnt).Item("AccYear"))
                    'Else
                    '    ddlSrchAccYearTo.Items.Add(objAccCfg.Tables(0).Rows(intCnt).Item("AccYear"))
                End If

                If objAccCfg.Tables(0).Rows(intCnt).Item("AccYear") = pv_strAccYear Then
                    intSelIndex = intCnt
                End If
            Next

            If pv_blnIsFrom = True Then
                ddlSrchAccYearFrom.SelectedIndex = intSelIndex
                intAccYear = ddlSrchAccYearFrom.SelectedItem.Value
                'Else
                '    ddlSrchAccYearTo.SelectedIndex = intSelIndex
                '    intAccYear = ddlSrchAccYearTo.SelectedItem.Value
            End If

            Try
                strParam = "||" & intAccYear
                intErrNo = objAdmAcc.mtdGetAccPeriodCfg(strOpCd_Max_Get, _
                                                        strCompany, _
                                                        pv_strLocation, _
                                                        strUserId, _
                                                        strParam, _
                                                        objAccCfg)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CT_STDRPT_CTRL_ACCCFG_MAX_GET&errmesg=" & Exp.ToString() & "&redirect=")
            End Try

            Try
                intMaxPeriod = Convert.ToInt16(objAccCfg.Tables(0).Rows(0).Item("MaxPeriod"))
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CT_STDRPT_CTLR_ACCCFG_MAXPERIOD&errmesg=System required period configuration to process your request. Please set period configuration for the year of " & Convert.ToString(intAccYear) & "&redirect=")
            End Try

        Else
            If pv_blnIsFrom = True Then
                ddlSrchAccYearFrom.Items.Add(strAccYear)
                ddlSrchAccYearFrom.SelectedIndex = intSelIndex
                'Else
                '    ddlSrchAccYearTo.Items.Add(strAccYear)
                '    ddlSrchAccYearTo.SelectedIndex = intSelIndex
            End If
            intMaxPeriod = Convert.ToInt16(strAccMonth)
        End If

        objAccCfg = Nothing
        Return intMaxPeriod
    End Function


    Sub OnIndexChage_FromAccPeriod(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim hidUserLoc As HtmlInputHidden

        hidUserLoc = RptSelect.FindControl("hidUserLoc")
        BindAccMonthList(BindAccYearList(hidUserLoc.Value, ddlSrchAccYearFrom.SelectedItem.Value, True))
    End Sub

    Sub OnIndexChage_ToAccPeriod(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim hidUserLoc As HtmlInputHidden

        hidUserLoc = RptSelect.FindControl("hidUserLoc")
        'BindAccMonthToList(BindAccYearList(hidUserLoc.Value, ddlSrchAccYearTo.SelectedItem.Value, False))
    End Sub

    Sub BindAccMonthToList(ByVal pv_intMaxMonth As Integer)
        Dim intCnt As Integer
        Dim intSelIndex As Integer = 0

        'ddlSrchAccMonthTo.Items.Clear()
        For intCnt = 1 To pv_intMaxMonth
            'ddlSrchAccMonthTo.Items.Add(intCnt)
            If intCnt = Convert.ToInt16(strAccMonth) Then
                intSelIndex = intCnt - 1
            End If
        Next
        'ddlSrchAccMonthTo.SelectedIndex = intSelIndex
    End Sub


    Sub btnPrintPrev_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)

        Dim strddlAccMth As String
        Dim strddlAccYr As String
        Dim strRptId As String
        Dim strRptName As String
        Dim strUserLoc As String
        Dim strDec As String

        Dim strSrchSupplier As String
        Dim strSrchAccMonthFrom As String
        Dim strSrchAccYearFrom As String
        Dim strSrchAccMonthTo As String
        Dim strSrchAccYearTo As String
        Dim strSrchPeriode1 As String
        Dim strSrchPeriode2 As String

        Dim objSysCfgDs As New Object()

        Dim ddlist As DropDownList

        Dim IntAgeFrom1 As Integer
        Dim intToAge1 As Integer
        Dim intFromAge2 As Integer
        Dim intToAge2 As Integer
        Dim intFromAge3 As Integer
        Dim intToAge3 As Integer
        Dim intFromAge4 As Integer
        Dim intToAge4 As Integer
        Dim intFromAge5 As Integer

        Dim tempUserLoc As HtmlInputHidden
        Dim templblUL As Label
        Dim strExportToExcel As String

        ddlist = RptSelect.FindControl("lstRptName")
        strRptId = Trim(ddlist.SelectedItem.Value)

        ddlist = RptSelect.FindControl("lstRptName")
        strRptName = Trim(ddlist.SelectedItem.Text)

        ddlist = RptSelect.FindControl("lstDecimal")
        strDec = Trim(ddlist.SelectedItem.Value)

        tempUserLoc = RptSelect.FindControl("hidUserLoc")
        strUserLoc = Trim(tempUserLoc.Value)

        If strUserLoc = "" Then
            templblUL = RptSelect.FindControl("lblUserLoc")
            templblUL.Visible = True
            Exit Sub
        Else
            If Left(strUserLoc, 3) = "','" Then
                strUserLoc = Right(strUserLoc, Len(strUserLoc) - 3)
            ElseIf Right(strUserLoc, 3) = "','" Then
                strUserLoc = Left(strUserLoc, Len(strUserLoc) - 3)
            End If
        End If
        IntAgeFrom1 = txtFromAge1.Text
        intToAge1 = txtToAge1.Text
        intFromAge2 = txtFromAge2.Text
        intToAge2 = txtToAge2.Text
        intFromAge3 = txtFromAge3.Text
        intToAge3 = txtToAge3.Text
        intFromAge4 = txtFromAge4.Text
        intToAge4 = txtToAge4.Text
        intFromAge5 = txtFromAge5.Text

        If intToAge1 - 1 < 0 Then
            lblErrToAge1.Visible = True
            Exit Sub
        End If

        If intFromAge2 - intToAge1 <> 1 Then
            lblErrFromAge2.Visible = True
            Exit Sub
        End If

        If intToAge2 - intFromAge2 < 0 Then
            lblErrToAge2.Visible = True
            Exit Sub
        End If

        If intFromAge3 - intToAge2 <> 1 Then
            lblErrFromAge3.Visible = True
            Exit Sub
        End If

        If intToAge3 - intFromAge3 < 0 Then
            lblErrToAge3.Visible = True
            Exit Sub
        End If

        If intFromAge4 - intToAge3 <> 1 Then
            lblErrFromAge4.Visible = True
            Exit Sub
        End If

        If intToAge4 - intFromAge4 < 0 Then
            lblErrToAge4.Visible = True
            Exit Sub
        End If

        If intFromAge5 - intToAge4 <> 1 Then
            lblErrFromAge5.Visible = True
            Exit Sub
        End If

        strSrchSupplier = Server.UrlEncode(Trim(ddlSupplier.SelectedItem.Value))

        strSrchAccMonthFrom = Server.UrlEncode(Trim(ddlSrchAccMonthFrom.SelectedItem.Value))
        strSrchAccYearFrom = Server.UrlEncode(Trim(ddlSrchAccYearFrom.SelectedItem.Value))
        'strSrchAccMonthTo = Server.UrlEncode(Trim(ddlSrchAccMonthTo.SelectedItem.Value))
        'strSrchAccYearTo = Server.UrlEncode(Trim(ddlSrchAccYearTo.SelectedItem.Value))


        If Len(Trim(strSrchAccMonthFrom)) = 1 Then
            strSrchPeriode1 = strSrchAccYearFrom & "0" & strSrchAccMonthFrom
        Else
            strSrchPeriode1 = strSrchAccYearFrom & strSrchAccMonthFrom
        End If


        'If Len(Trim(strSrchAccMonthTo)) = 1 Then
        '    strSrchPeriode2 = strSrchAccYearTo & "0" & strSrchAccMonthTo
        'Else
        '    strSrchPeriode2 = strSrchAccYearTo & strSrchAccMonthTo
        'End If

        strExportToExcel = IIf(cbExcel.Checked = True, "1", "0")

        Response.Write("<Script Language=""JavaScript"">window.open(""AP_StdRpt_Supplier_AgeingListPreview.aspx?Type=Print&CompName=" & strCompany & _
                       "&SelLocation=" & strUserLoc & _
                       "&DDLAccMth=" & strddlAccMth & _
                       "&DDLAccYr=" & strddlAccYr & _
                       "&RptId=" & strRptId & _
                       "&RptName=" & strRptName & _
                       "&Decimal=" & strDec & _
                       "&SrchLocation=" & strUserLoc & _
                       "&SrchSupplier=" & strSrchSupplier & _
                       "&SrchPeriod1=" & strSrchPeriode1 & _
                       "&SrchPeriod2=" & strSrchPeriode1 & _
                       "&FromAge1=" & txtFromAge1.Text & _
                       "&ToAge1=" & txtToAge1.Text & _
                       "&FromAge2=" & txtFromAge2.Text & _
                       "&ToAge2=" & txtToAge2.Text & _
                       "&FromAge3=" & txtFromAge3.Text & _
                       "&ToAge3=" & txtToAge3.Text & _
                       "&FromAge4=" & txtFromAge4.Text & _
                       "&ToAge4=" & txtToAge4.Text & _
                       "&FromAge5=" & txtFromAge5.Text & _
                       "&ExportToExcel=" & strExportToExcel & _
                       """,null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")

    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()

        'lblAccCode.text = GetCaption(objLangCap.EnumLangCap.Account) & lblCode.text
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
            Response.Redirect("../../include/mesg/ErrorMessage.aspx?errcode=GL_REPORTS_DETACCLEDGER_GET_LANGCAP&errmesg=" & Exp.ToString() & "&redirect=../en/reports/GL_StdRpt_Selection.aspx")
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

    Sub BindSupplier(ByVal pv_strSupplierId As String)
        Dim strOpCode_GetSupp As String = "PU_CLSSETUP_SUPPLIER_GET"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedSuppIndex As Integer = 0
        Dim objSuppDs As New Object()

        strParam = "||" & objPUSetup.EnumSuppStatus.Active & "||SupplierCode||"
        strParam = strParam & "|" & IIf(Session("SS_COACENTRALIZED") = "1", "", " A.AccCode in (SELECT AccCode FROM GL_Account WHERE LocCode='" & Trim(strLocation) & "') ")

        Try
            intErrNo = objPUSetup.mtdGetSupplier(strOpCode_GetSupp, strParam, objSuppDs)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_PAYMENT_GET_SUPPCODE&errmesg=" & Exp.ToString() & "&redirect=CB/trx/cb_trx_paylist.aspx")
        End Try

        For intCnt = 0 To objSuppDs.Tables(0).Rows.Count - 1
            objSuppDs.Tables(0).Rows(intCnt).Item("SupplierCode") = objSuppDs.Tables(0).Rows(intCnt).Item("SupplierCode").Trim()
            objSuppDs.Tables(0).Rows(intCnt).Item("Name") = objSuppDs.Tables(0).Rows(intCnt).Item("SupplierCode") & " (" & objSuppDs.Tables(0).Rows(intCnt).Item("Name").Trim() & ")"
            If objSuppDs.Tables(0).Rows(intCnt).Item("SupplierCode") = pv_strSupplierId Then
                intSelectedSuppIndex = intCnt + 1
            End If
        Next intCnt

        Dim dr As DataRow
        dr = objSuppDs.Tables(0).NewRow()
        dr("SupplierCode") = ""
        dr("Name") = " Please Select Supplier Code"
        objSuppDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlSupplier.DataSource = objSuppDs.Tables(0)
        ddlSupplier.DataValueField = "SupplierCode"
        ddlSupplier.DataTextField = "Name"
        ddlSupplier.DataBind()
        ddlSupplier.SelectedIndex = intSelectedSuppIndex
        ddlSupplier.AutoPostBack = True
    End Sub

    Sub onSelect_Supplier(ByVal Sender As System.Object, ByVal E As System.EventArgs)
        Dim strOpCode As String = "PU_CLSSETUP_SUPPLIER_GET"
        Dim intErrNo As Integer
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim dsMaster As Object

        strParamName = "STRSEARCH"
        strParamValue = " And A.SupplierCode = '" & ddlSupplier.SelectedItem.Value & "'"

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
                                                strParamName, _
                                                strParamValue, _
                                                dsMaster)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=IN_ITEM_GET&errmesg=" & Exp.ToString() & "&redirect=")
        End Try

        If dsMaster.Tables(0).Rows.Count > 0 Then
            txtSupplierCode.Text = Trim(dsMaster.Tables(0).Rows(0).Item("Name"))
        End If
    End Sub
End Class
