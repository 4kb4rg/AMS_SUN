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


Public Class CM_setup_ContractQuality : Inherits Page

    Protected WithEvents txtLTCBiasaFFA As TextBox
    Protected WithEvents txtLTCBiasaMI As TextBox
    Protected WithEvents txtLTCForwardFFA As TextBox
    Protected WithEvents txtLTCForwardMI As TextBox
    Protected WithEvents txtSpotLokalFFA As TextBox
    Protected WithEvents txtSpotLokalMI As TextBox
    Protected WithEvents txtSpotLokalDOBI As TextBox
    Protected WithEvents txtSpotExportFFA As TextBox
    Protected WithEvents txtSpotExportMI As TextBox
    Protected WithEvents txtSpotExportDOBI As TextBox
    Protected WithEvents txtMoisture As TextBox
    Protected WithEvents txtImpurity As TextBox

    Protected WithEvents lblStatus As Label
    Protected WithEvents lblIsExist As Label
    Protected WithEvents lblDateCreated As Label
    Protected WithEvents lblLastUpdate As Label
    Protected WithEvents lblUpdatedBy As Label

    Protected WithEvents SaveBtn As ImageButton

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
    Protected WithEvents srchCurrencyCode As TextBox
    Protected WithEvents srchDescription As TextBox
    Protected WithEvents srchUpdBy As TextBox
    Protected WithEvents srchStatus As DropDownList

    Protected WithEvents lblHiddenSts As Label
    Dim strOpCdGet As String = "CM_CLSSETUP_CONTRACTQUALITY_GET"
    Dim strOpCdUpd As String = "CM_CLSSETUP_CONTRACTQUALITY_UPD"
    Dim strOpCdAdd As String = "CM_CLSSETUP_CONTRACTQUALITY_ADD"

    Protected objCMSetup As New agri.CM.clsSetup()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()

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
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCMAccessRights.CMContractQuality), intCMAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            If Not IsPostBack Then
                onLoad_Display()
            End If
            
        End If
    End Sub

    Sub BackBtn_Click(Sender As Object, E As ImageClickEventArgs)
        Response.Redirect("../../menu/menu_CMSetup_page.aspx")
    End Sub

    Sub Button_Click(Sender As Object, E As ImageClickEventArgs)
        Dim strCmdArgs As String = CType(Sender, ImageButton).CommandArgument        
        Dim intErrNo As Integer
        Dim blnIsUpdate As Boolean
        Dim strParam As String = ""
        Dim strRate As String = txtLTCBiasaFFA.text


        If strCmdArgs = "Save" Then 
            blnIsUpdate = IIf(Cint(lblIsExist.text) = 0, False, True)

            strParam = txtLTCBiasaFFA.Text & Chr(9) & _
                       txtLTCBiasaMI.Text & Chr(9) & _
                       txtLTCForwardFFA.Text & Chr(9) & _
                       txtLTCForwardMI.Text & Chr(9) & _
                       txtSpotLokalFFA.Text & Chr(9) & _
                       txtSpotLokalMI.Text & Chr(9) & _
                       txtSpotLokalDOBI.Text & Chr(9) & _
                       txtSpotExportFFA.Text & Chr(9) & _
                       txtSpotExportMI.Text & Chr(9) & _
                       txtSpotExportDOBI.Text & Chr(9) & _
                       txtMoisture.Text & Chr(9) & _
                       txtImpurity.Text & Chr(9) & _
                       objCMSetup.EnumContractQualityStatus.Active

            Try
                intErrNo = objCMSetup.mtdUpdContractQuality(strOpCdAdd, _
                                                        strOpCdUpd, _
                                                        strCompany, _
                                                        strLocation, _
                                                        strUserId, _
                                                        strParam, _
                                                        blnIsUpdate)

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_SETUP_ContractQuality_SAVE&errmesg=" & lblErrMessage.Text & "&redirect=CM/Setup/CM_setup_ContractQuality.aspx")
            End Try
        End If
        onLoad_Display()
    End Sub

    Sub onLoad_Display()
        Dim strParam As String        
        Dim intErrNo As Integer
        Dim strSearch As String
        Dim objCurrDs As New Object()
        
        strParam =  "|" 

        Try
            intErrNo = objCMSetup.mtdGetMasterList(strOpCdGet, strParam, 0, objCurrDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_SETUP_ContractQuality_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Setup/CM_setup_ContractQuality.aspx")
        End Try

        lblIsExist.text = objCurrDs.Tables(0).rows.Count
	 
        if Cint(lblIsExist.text) > 0 then 
            txtLTCBiasaFFA.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("LTCBiasaFFA"))
            txtLTCBiasaMI.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("LTCBiasaMI"))
            txtLTCForwardFFA.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("LTCForwardFFA"))
            txtLTCForwardMI.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("LTCForwardMI"))
            txtSpotLokalFFA.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("SpotLokalFFA"))
            txtSpotLokalMI.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("SpotLokalMI"))
            txtSpotLokalDOBI.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("SpotLokalDOBI"))
            txtSpotExportFFA.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("SpotExportFFA"))
            txtSpotExportMI.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("SpotExportMI")) 
            txtSpotExportDOBI.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("SpotExportDOBI")) 
            txtMoisture.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("Moisture"))
            txtImpurity.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("Impurity"))   

            lblHiddenSts.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("Status"))
            lblStatus.Text = objCMSetup.mtdGetContractQualityStatus(Trim(objCurrDs.Tables(0).Rows(0).Item("Status")))
            lblDateCreated.Text = objGlobal.GetLongDate(objCurrDs.Tables(0).Rows(0).Item("CreateDate"))
            lblLastUpdate.Text = objGlobal.GetLongDate(objCurrDs.Tables(0).Rows(0).Item("UpdateDate"))
            lblUpdatedBy.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("UserName"))        
        end if
    End Sub

End Class
