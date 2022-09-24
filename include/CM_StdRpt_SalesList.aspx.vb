Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports Microsoft.VisualBasic.Strings
Imports System.Xml
Imports System.Web.Services
Imports Microsoft.VisualBasic.Information
Imports Microsoft.VisualBasic.Interaction
Imports Microsoft.VisualBasic

Public Class CM_StdRpt_SalesList : Inherits Page

    Protected RptSelect As UserControl

    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl
    Dim objSysCfg As New agri.PWSystem.clsConfig
    Dim objAdmAcc As New agri.Admin.clsAccPeriod()
    Dim objGLtrx As New agri.GL.ClsTrx()
    Dim objGLSetup As New agri.GL.clsSetup()
    Dim objPUSetup As New agri.PU.clsSetup()
    Dim objWMTrx As New agri.WM.clsTrx()


    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblTracker As Label
    Protected WithEvents PrintPrev As ImageButton

    Protected WithEvents txtContractNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDONo As System.Web.UI.WebControls.TextBox

    Protected WithEvents txtBuyerCode As TextBox
    Protected WithEvents ddlSrchAccMonthFrom As DropDownList
    Protected WithEvents ddlSrchAccYearFrom As DropDownList
    Protected WithEvents ddlSrchAccMonthTo As DropDownList
    Protected WithEvents ddlSrchAccYearTo As DropDownList
    Protected WithEvents ddlBuyer As DropDownList
    Protected WithEvents ddlProduct As DropDownList
    Protected WithEvents ddlRptType As DropDownList
    Protected WithEvents cbExcel As CheckBox

    Dim objLangCapDs As New Object
    Dim strCompany As String
    Dim strCompanyName As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strUserName As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intErrNo As Integer

    Dim strLocType As String


    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strCompanyName = Session("SS_COMPANYNAME")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strUserName = Session("SS_USERNAME")
        strAccMonth = Session("SS_ARACCMONTH")
        strAccYear = Session("SS_ARACCYEAR")
        strLangCode = Session("SS_LANGCODE")
        strLocType = Session("SS_LOCTYPE")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        Else
            lblErrMessage.Visible = False
            If Not Page.IsPostBack Then
                onload_GetLangCap()
                BindAccMonthList(BindAccYearList(strLocation, strAccYear, True))
                BindAccMonthToList(BindAccYearList(strLocation, strAccYear, False))
                BindBuyer("")
                BindProduct()
            End If
        End If
    End Sub

    Protected Overloads Sub OnPreRender(ByVal Source As Object, ByVal E As EventArgs) Handles MyBase.PreRender
        Dim SDecimal As HtmlTableRow
        Dim SLocation As HtmlTableRow

        SDecimal = RptSelect.FindControl("SelDecimal")
        SLocation = RptSelect.FindControl("SelLocation")
        SDecimal.Visible = True
        SLocation.Visible = True
    End Sub

    Sub onload_GetLangCap()
        'GetEntireLangCap()
    End Sub

    Sub BindProduct()
        ddlProduct.Items.Add(New ListItem("All", ""))
        ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.FFB), objWMTrx.EnumWeighBridgeTicketProduct.FFB))
        ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.CPO), objWMTrx.EnumWeighBridgeTicketProduct.CPO))
        ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.PK), objWMTrx.EnumWeighBridgeTicketProduct.PK))
        ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.Others), objWMTrx.EnumWeighBridgeTicketProduct.Others))
        ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.EFB), objWMTrx.EnumWeighBridgeTicketProduct.EFB))
        ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.Shell), objWMTrx.EnumWeighBridgeTicketProduct.Shell))
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
        Dim objAccCfg As New Dataset()

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
        Else
            ddlSrchAccYearTo.Items.Clear()
        End If

        If objAccCfg.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objAccCfg.Tables(0).Rows.Count - 1
                If pv_blnIsFrom = True Then
                    ddlSrchAccYearFrom.Items.Add(objAccCfg.Tables(0).Rows(intCnt).Item("AccYear"))
                Else
                    ddlSrchAccYearTo.Items.Add(objAccCfg.Tables(0).Rows(intCnt).Item("AccYear"))
                End If

                If objAccCfg.Tables(0).Rows(intCnt).Item("AccYear") = pv_strAccYear Then
                    intSelIndex = intCnt
                End If
            Next

            If pv_blnIsFrom = True Then
                ddlSrchAccYearFrom.SelectedIndex = intSelIndex
                intAccYear = ddlSrchAccYearFrom.SelectedItem.Value
            Else
                ddlSrchAccYearTo.SelectedIndex = intSelIndex
                intAccYear = ddlSrchAccYearTo.SelectedItem.Value
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
            Else
                ddlSrchAccYearTo.Items.Add(strAccYear)
                ddlSrchAccYearTo.SelectedIndex = intSelIndex
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
        BindAccMonthToList(BindAccYearList(hidUserLoc.Value, ddlSrchAccYearTo.SelectedItem.Value, False))
    End Sub

    Sub BindAccMonthToList(ByVal pv_intMaxMonth As Integer)
        Dim intCnt As Integer
        Dim intSelIndex As Integer = 0

        ddlSrchAccMonthTo.Items.Clear()
        For intCnt = 1 To pv_intMaxMonth
            ddlSrchAccMonthTo.Items.Add(intCnt)
            If intCnt = Convert.ToInt16(strAccMonth) Then
                intSelIndex = intCnt - 1
            End If
        Next
        ddlSrchAccMonthTo.SelectedIndex = intSelIndex
    End Sub

    Sub BindBuyer(ByVal pv_strBuyerId As String)
        Dim strOpCdGet As String = "GL_CLSSETUP_BILLPARTY_GET"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer = 0
        Dim objBuyerDs As New Object()

        strParam = "" & "|" & _
                   "" & "|" & _
                   objGLSetup.EnumBillPartyStatus.Active & "|" & _
                   "" & "|" & _
                   "BP.BillPartyCode" & "|" & _
                   "ASC" & "|"
        Try
            intErrNo = objGLSetup.mtdGetBillParty(strOpCdGet, strParam, objBuyerDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_BUYERLIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegList.aspx")
        End Try

        If objBuyerDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objBuyerDs.Tables(0).Rows.Count - 1
                objBuyerDs.Tables(0).Rows(intCnt).Item("BillPartyCode") = Trim(objBuyerDs.Tables(0).Rows(intCnt).Item("BillPartyCode"))
                objBuyerDs.Tables(0).Rows(intCnt).Item("Name") = Trim(objBuyerDs.Tables(0).Rows(intCnt).Item("Name"))
                If objBuyerDs.Tables(0).Rows(intCnt).Item("BillPartyCode") = pv_strBuyerId Then
                    intSelectedIndex = intCnt + 1
                End If
            Next
        End If

        Dim dr As DataRow
        dr = objBuyerDs.Tables(0).NewRow()
        dr("BillPartyCode") = ""
        dr("Name") = " Please Select Buyer Code"
        objBuyerDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlBuyer.DataSource = objBuyerDs.Tables(0)
        ddlBuyer.DataValueField = "BillPartyCode"
        ddlBuyer.DataTextField = "Name"
        ddlBuyer.DataBind()
        ddlBuyer.SelectedIndex = intSelectedIndex
        ddlBuyer.AutoPostBack = True
    End Sub

    Sub onSelect_Buyer(ByVal Sender As System.Object, ByVal E As System.EventArgs)
        Dim strOpCode As String = "PU_CLSSETUP_Buyer_GET"
        Dim intErrNo As Integer
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim dsMaster As Object

        strParamName = "STRSEARCH"
        strParamValue = " And A.BuyerCode = '" & ddlBuyer.SelectedItem.Value & "'"

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
                                                strParamName, _
                                                strParamValue, _
                                                dsMaster)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=IN_ITEM_GET&errmesg=" & Exp.ToString() & "&redirect=")
        End Try

        If dsMaster.Tables(0).Rows.Count > 0 Then
            txtBuyerCode.Text = Trim(dsMaster.Tables(0).Rows(0).Item("Name"))
        End If
    End Sub

    Sub btnPrintPrev_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)

        Dim strUserLoc As String
        Dim strDec As String
        Dim tempDec As DropDownList
        Dim templblUL As Label
        Dim tempUserLoc As HtmlInputHidden
        Dim strExportToExcel As String
        Dim strSrchAccMonthFrom As String
        Dim strSrchAccYearFrom As String
        Dim strSrchAccMonthTo As String
        Dim strSrchAccYearTo As String
        Dim strSrchPeriode1 As String
        Dim strSrchPeriode2 As String
        Dim strSrchBuyer As String
        Dim strSrchProduct As String
        Dim strSrchRptType As String

        tempUserLoc = RptSelect.FindControl("hidUserLoc")
        strUserLoc = Trim(tempUserLoc.Value)
        tempDec = RptSelect.FindControl("lstDecimal")
        strDec = Trim(tempDec.SelectedItem.Value)

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

        strSrchAccMonthFrom = Server.UrlEncode(Trim(ddlSrchAccMonthFrom.SelectedItem.Value))
        strSrchAccYearFrom = Server.UrlEncode(Trim(ddlSrchAccYearFrom.SelectedItem.Value))
        strSrchAccMonthTo = Server.UrlEncode(Trim(ddlSrchAccMonthTo.SelectedItem.Value))
        strSrchAccYearTo = Server.UrlEncode(Trim(ddlSrchAccYearTo.SelectedItem.Value))

        strSrchProduct = Server.UrlEncode(Trim(ddlProduct.Text))
        strSrchBuyer = Server.UrlEncode(Trim(txtBuyerCode.Text))
        strSrchRptType = Server.UrlEncode(Trim(ddlRptType.Text))

        If Len(Trim(strSrchAccMonthFrom)) = 1 Then
            strSrchPeriode1 = strSrchAccYearFrom & "0" & strSrchAccMonthFrom
        Else
            strSrchPeriode1 = strSrchAccYearFrom & strSrchAccMonthFrom
        End If

        If Len(Trim(strSrchAccMonthTo)) = 1 Then
            strSrchPeriode2 = strSrchAccYearTo & "0" & strSrchAccMonthTo
        Else
            strSrchPeriode2 = strSrchAccYearTo & strSrchAccMonthTo
        End If

        strExportToExcel = IIf(cbExcel.Checked = True, "1", "0")

        Response.Write("<Script Language=""JavaScript"">window.open(""CM_StdRpt_SalesListPreview.aspx?Location=" & strUserLoc & _
                        "&Decimal=" & strDec & _
                        "&SrchProduct=" & strSrchProduct & _
                        "&SrchBuyer=" & strSrchBuyer & _
                        "&SrchPeriod1=" & strSrchPeriode1 & _
                        "&SrchPeriod2=" & strSrchPeriode2 & _
                        "&SrchRptType=" & strSrchRptType & _
                        "&ExportToExcel=" & strExportToExcel & _
                        """,null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")

    End Sub
End Class
 
