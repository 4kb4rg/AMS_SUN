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

Public Class CM_StdRpt_PrintContractDoc : Inherits Page

    Protected RptSelect As UserControl

    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl
    Dim objSysCfg As New agri.PWSystem.clsConfig
    Dim objLangCap As New agri.PWSystem.clsLangCap
    Dim objWMTrx As New agri.WM.clsTrx
    Dim objAdminLoc As New agri.Admin.clsLoc
    Dim objCMTrx As New agri.CM.clsTrx()
    Dim objContractDs As New Object()

    Protected WithEvents lblDate As Label
    Protected WithEvents lblDateFormat As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblErrContract As Label
    Protected WithEvents lblTracker As Label
    Protected WithEvents LocTag As Label
    Protected WithEvents lblAccount As Label
    Protected WithEvents lblBlock As Label
    Protected WithEvents lblBillParty As Label
    Protected WithEvents PrintPrev As ImageButton
    Protected WithEvents ddlContractNo As DropDownList
    Protected WithEvents ddlProduct As DropDownList
    Protected WithEvents txtBillParty As TextBox
    Protected WithEvents txtOwner1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBuyer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtOwner2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents ChkPPN As CheckBox
    Protected WithEvents ChkInclude As CheckBox
    Protected WithEvents Rep1 As HtmlTableRow
    Protected WithEvents Rep2 As HtmlTableRow
    Protected WithEvents txtPengiriman As TextBox
    Protected WithEvents txtAsalBarang As TextBox

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
    Protected WithEvents rfvBuyer As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rfvOwner1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rfvOwner2 As System.Web.UI.WebControls.RequiredFieldValidator
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
        lblDate.Visible = False
        lblDateFormat.Visible = False
        Rep2.Visible = False

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        Else
            If Not Page.IsPostBack Then
                onload_GetLangCap()
                BindProduct()
                BindContractNoList("")
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

    Sub BindProduct()
        ddlProduct.Items.Add(New ListItem("All", ""))
        ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.FFB), objWMTrx.EnumWeighBridgeTicketProduct.FFB))
        ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.CPO), objWMTrx.EnumWeighBridgeTicketProduct.CPO))
        ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.PK), objWMTrx.EnumWeighBridgeTicketProduct.PK))
    End Sub


    Sub BindContractNoList(ByVal pv_strContNo As String)
        Dim strParam As String
        Dim strOpCdGet As String = "CM_CLSTRX_CONTRACT_REG_GET"
        Dim dr As DataRow
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer
        Dim strSearch as String 

        strSearch = "and ctr.LocCode = '" & strLocation & "' and ctr.status in ('1', '4') "
        strParam = strSearch & "|" & ""


        Try
            intErrNo = objCMTrx.mtdGetContract(strOpCdGet, strParam, 0, objContractDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegList.aspx")
        End Try

        If objContractDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objContractDs.Tables(0).Rows.Count - 1
                objContractDs.Tables(0).Rows(intCnt).Item("ContractNo") = Trim(objContractDs.Tables(0).Rows(intCnt).Item("ContractNo"))
                If objContractDs.Tables(0).Rows(intCnt).Item("ContractNo") = pv_strContNo Then
                    intSelectedIndex = intCnt + 1
                End If
            Next
        End If

        dr = objContractDs.Tables(0).NewRow()
        dr("ContractNo") = ""
        dr("ContractNo") = "Please Select Contract No"
        objContractDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlContractNo.DataSource = objContractDs.Tables(0)
        ddlContractNo.DataValueField = "ContractNo"
        ddlContractNo.DataTextField = "ContractNo"
        ddlContractNo.DataBind()
        ddlContractNo.SelectedIndex = intSelectedIndex
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        LocTag.Text = GetCaption(objLangCap.EnumLangCap.Location)
        lblAccount.Text = GetCaption(objLangCap.EnumLangCap.Account)
        lblBlock.Text = GetCaption(objLangCap.EnumLangCap.Block)
        lblBillParty.Text = GetCaption(objLangCap.EnumLangCap.BillParty)
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
            Response.Redirect("../../include/mesg/ErrorMessage.aspx?errcode=PWSYSTEM_CLSLANGCAP_BUSSTERM_GET&errmesg=" & lblErrMessage.Text & "&redirect=../en/reports/CM_StdRpt_Selection.aspx")
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

    Sub btnPrintPrev_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim strRptID As String
        Dim strRptName As String
        Dim strUserLoc As String
        Dim strDec As String
        Dim strContractNo As String
        Dim strProduct As String
        Dim strBillParty As String
        Dim strBuyer As String
        Dim strOwner1 As String
        Dim strOwner2 As String
        Dim tempRpt As DropDownList
        Dim tempDec As DropDownList
        Dim templblUL As Label
        Dim tempUserLoc As HtmlInputHidden
        Dim strParam As String
        Dim strPPN As String
        Dim strPPNInclude As String
        Dim strPengiriman As String
        Dim strAsalBarang As String

        If ddlContractNo.SelectedIndex = 0 Then
            lblErrContract.Visible = True
            Exit Sub
        Else
            lblErrContract.Visible = False
        End If

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

        tempRpt = RptSelect.FindControl("lstRptName")
        strRptID = Trim(tempRpt.SelectedItem.Value)
        strRptName = Trim(tempRpt.SelectedItem.Text)   

        strContractNo = Server.UrlEncode(ddlContractNo.SelectedValue.Trim())
        strProduct = Server.UrlEncode(ddlProduct.SelectedValue.Trim())
        strBillParty = Server.UrlEncode(txtBillParty.Text.Trim())
        strBuyer = Server.UrlEncode(txtBuyer.Text.Trim())
        strOwner1 = Server.UrlEncode(txtOwner1.Text.Trim())
        strOwner2 = Server.UrlEncode(txtOwner2.Text.Trim())
        strPPN = IIf(ChkPPN.Checked = True, "Yes", "No")
        strPPNInclude = IIf(ChkPPN.Checked = True, IIf(ChkInclude.Checked = True, "YES", "NO"), "NO")
        strPengiriman = Trim(txtPengiriman.Text)
        strAsalBarang = Trim(txtAsalBarang.Text)


        Response.Write("<Script Language=""JavaScript"">window.open(""CM_StdRpt_PrintContractDocPreview.aspx?Location=" & strUserLoc & _
                        "&Decimal=" & strDec & _
                        "&ContractNo=" & strContractNo & _
                        "&Product=" & strProduct & _
                        "&BillParty=" & strBillParty & _
                        "&Buyer=" & strBuyer & _
                        "&Owner1=" & strOwner1 & _
                        "&Owner2=" & strOwner2 & _
                        "&PPN=" & strPPN & _
                        "&PPNInclude=" & strPPNInclude & _
                        "&Pengiriman=" & strPengiriman & _
                        "&AsalBarang=" & strAsalBarang & _
                        """,null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")
    End Sub

    Sub PPN_Type(ByVal Sender As Object, ByVal E As EventArgs)
        If ChkPPN.Checked = True Then
            ChkPPN.Text = "  Yes"
            ChkInclude.Enabled = True
        Else
            ChkPPN.Text = "  No"
            ChkInclude.Enabled = False
        End If
    End Sub

    Sub Include_Type(ByVal Sender As Object, ByVal E As EventArgs)
        If ChkInclude.Checked = True Then
            ChkInclude.Text = "  Yes"
        Else
            ChkInclude.Text = "  No"
        End If
    End Sub

    Private Sub InitializeComponent()

    End Sub
End Class
 
