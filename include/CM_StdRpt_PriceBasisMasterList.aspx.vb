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

Public Class CM_StdRpt_PriceBasisMasterList : Inherits Page

    Protected RptSelect As UserControl

    Dim objCM As New agri.CM.clsReport()
    Dim objCMSetup As New agri.CM.clsSetup()
    Dim objCMTrx As New agri.CM.clsTrx()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objLangCap As New agri.PWSystem.clsLangCap()


    Protected WithEvents lblDate As Label
    Protected WithEvents lblDateFormat As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblTracker As Label

    Protected WithEvents lblLocation As Label
    Protected WithEvents lstStatus As DropDownList

    Protected WithEvents PrintPrev As ImageButton
    Protected WithEvents txtPriceBasisCode as TextBox
    Protected WithEvents txtName as TextBox
    Protected WithEvents txtUpdatedBy as TextBox
    Protected WithEvents txtDateFrom as TextBox
    Protected WithEvents txtDateTo as TextBox


    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim strLocType as String

    Dim objLangCapDs As New Object()

    Dim strCompany As String
    Dim strCompanyName As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strUserName As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String

    Dim intErrNo As Integer

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

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        Else

            If Not Page.IsPostBack Then
                onload_GetLangCap()
                BindStatusList()
            End If
        End If
    End Sub

    Protected Overloads Sub OnPreRender(ByVal Source As Object, ByVal E As EventArgs) Handles MyBase.PreRender
        Dim SDecimal As  HtmlTableRow 
        Dim SLocation As HtmlTableRow

        
        SDecimal  = RptSelect.FindControl("SelDecimal")
        SLocation  = RptSelect.FindControl("SelLocation")
        
        SDecimal.visible = false
        SLocation.visible = false        

    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblLocation.Text = GetCaption(objLangCap.EnumLangCap.Location)
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
            Response.Redirect("../../include/mesg/ErrorMessage.aspx?errcode=PWSYSTEM_CLSLANGCAP_BUSSTERM_GET&errmesg=" & lblErrMessage.Text & "&redirect=../en/reports/IN_StdRpt_Selection.aspx")
        End Try

    End Sub

    Function GetCaption(ByVal pv_TermCode) As String
        Dim count As Integer

        For count = 0 To objLangCapDs.Tables(0).Rows.Count - 1
            If Trim(pv_TermCode) = Trim(objLangCapDs.Tables(0).Rows(count).Item("TermCode")) Then
                If strLocType = objAdminLoc.EnumLocType.Mill then
                    Return Trim(objLangCapDs.Tables(0).Rows(count).Item("BusinessTermMW"))
                Else
                    Return Trim(objLangCapDs.Tables(0).Rows(count).Item("BusinessTerm"))
                End If
                Exit For
            End If
        Next
    End Function




    Sub BindStatusList()

        
        lstStatus.Items.Add(New ListItem(objCMSetup.mtdGetPriceBasisStatus(objCMSetup.EnumPriceBasisStatus.All), objCMSetup.EnumPriceBasisStatus.All))
        lstStatus.Items.Add(New ListItem(objCMSetup.mtdGetPriceBasisStatus(objCMSetup.EnumPriceBasisStatus.Active), objCMSetup.EnumPriceBasisStatus.Active))
        lstStatus.Items.Add(New ListItem(objCMSetup.mtdGetPriceBasisStatus(objCMSetup.EnumPriceBasisStatus.Deleted), objCMSetup.EnumPriceBasisStatus.Deleted))


    End Sub


    Sub btnPrintPrev_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim strPRNoFrom As String
        Dim strPRNoTo As String
        Dim strPRType As String
        Dim strStatus As String

        Dim strDateFrom As String
        Dim strDateTo As String
        Dim strRptID As String
        Dim strRptName As String
        Dim strUserLoc As String
        Dim strDec As String
        Dim strPriceBasisCode As String
        Dim strName As String
        Dim strTelephone As String
        Dim strUpdatedBy As String

        Dim tempDateFrom As TextBox
        Dim tempDateTo As TextBox
        Dim tempRpt As DropDownList
        Dim tempDec As DropDownList
        Dim templblUL As Label
        Dim tempUserLoc As HtmlInputHidden       

        Dim strParam As String
        Dim strDateSetting As String

        Dim objSysCfgDs As New Object()
        Dim objDateFormat As New Object()
        Dim objDateFrom As String
        Dim objDateTo As String
                
        strDateFrom = Trim(txtDateFrom.Text)        
        strDateTo = Trim(txtDateTo.Text)
        tempRpt = RptSelect.FindControl("lstRptName")
        strRptID = Trim(tempRpt.SelectedItem.Value)
        strRptName = Trim(tempRpt.SelectedItem.Text)

        strPriceBasisCode = Trim(txtPriceBasisCode.Text)
        strName = Trim(txtName.Text)
        strUpdatedBy = Trim(txtUpdatedBy.Text)

        strStatus = Trim(lstStatus.SelectedItem.Text)   

        strPriceBasisCode = Server.UrlEncode(strPriceBasisCode)
        strName = Server.UrlEncode(strName)
        strUpdatedBy = Server.UrlEncode(strUpdatedBy)
        

        






        strParam = "PWSYSTEM_CLSCONFIG_CONFIG_DATEFMT_GET"
        Try
            intErrNo = objSysCfg.mtdGetConfigInfo(strParam, _
                                                  strCompany, _
                                                  strLocation, _
                                                  strUserId, _
                                                  objSysCfgDs)
        Catch Exp As System.Exception
            Response.Redirect("../../include/mesg/ErrorMessage.aspx?errcode=WM_STDRPT_PriceBasis_MASTER_GET_CONFIG_DATE&errmesg=" & lblErrMessage.Text & "&redirect=../en/reports/WM_StdRpt_Selection.aspx")
        End Try

        strDateSetting = objSysCfg.mtdGetDateFormat(Trim(objSysCfgDs.Tables(0).Rows(0).Item("Datefmt")))

        If Not (strDateFrom = "" And strDateTo = "") Then
            If objGlobal.mtdValidInputDate(strDateSetting, strDateFrom, objDateFormat, objDateFrom) = True And objGlobal.mtdValidInputDate(strDateSetting, strDateTo, objDateFormat, objDateTo) = True Then
           Response.Write("<Script Language=""JavaScript"">window.open(""CM_StdRpt_PriceBasisMasterListPreview.aspx?Type=Print&Location=" & strUserLoc & "&RptID=" & strRptID & "&RptName=" & strRptName & "&Decimal=" & "" & _
                           "&DateFrom=" & objDateFrom & "&DateTo=" & objDateTo & "&PriceBasisCode=" & strPriceBasisCode  & "&Name=" & strName & "&Telephone=" & strTelephone & "&UpdatedBy=" & strUpdatedBy  & _
                           "&Status=" & strStatus & """,null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")
            Else
                lblDateFormat.Text = objDateFormat & "."
                lblDate.Visible = True
                lblDateFormat.Visible = True
            End If
        Else
           Response.Write("<Script Language=""JavaScript"">window.open(""CM_StdRpt_PriceBasisMasterListPreview.aspx?Type=Print&Location=" & strUserLoc & "&RptID=" & strRptID & "&RptName=" & strRptName & "&Decimal=" & "" & _
                           "&DateFrom=" & objDateFrom & "&DateTo=" & objDateTo & "&PriceBasisCode=" & strPriceBasisCode  & "&Name=" & strName & "&Telephone=" & strTelephone & "&UpdatedBy=" & strUpdatedBy  & _
                           "&Status=" & strStatus & """,null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")
        End If
    End Sub

End Class
 
