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

Public Class CM_StdRpt_ContractBook : Inherits Page

    Protected RptSelect As UserControl

    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl
    Dim objSysCfg As New agri.PWSystem.clsConfig
 
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblTracker As Label
    Protected WithEvents PrintPrev As ImageButton

    Protected WithEvents txtContractNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDONo As System.Web.UI.WebControls.TextBox
   

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
    Sub btnPrintPrev_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)

        Dim strUserLoc As String
        Dim strDec As String
        Dim tempDec As DropDownList
        Dim templblUL As Label
        Dim tempUserLoc As HtmlInputHidden
        Dim strContractNo As String
        Dim strDONo As String

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

        strContractNo = Server.UrlEnCode(txtContractNo.Text.Trim())
        strDONo = Server.UrlEncode(txtDONo.Text.Trim())

        If TRIM(strContractNo) = "" And TRIM(strDONo) = "" Then
            lblErrMessage.Text = "Silakan Input No Kontrak Atau No DO"
            lblErrMessage.Visible = True
            Exit Sub
        End If

        If TRIM(strContractNo) <> "" And TRIM(strDONo) <> "" Then
            lblErrMessage.Text = "Silakan Input No Kontrak Atau No DO"
            lblErrMessage.Visible = True
            Exit Sub
        End If

        Response.Write("<Script Language=""JavaScript"">window.open(""CM_StdRpt_ContractBookPreview.aspx?Location=" & strUserLoc & _
                        "&Decimal=" & strDec & _
                        "&ContractNo=" & strContractNo & _
                        "&DONo=" & strDONo & _
                        """,null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")

    End Sub
End Class
 
