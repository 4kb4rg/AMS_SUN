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

Public Class CM_setup_ClaimQuality : Inherits Page


    Protected WithEvents txtFFAFrom As TextBox
    Protected WithEvents txtFFATo As TextBox
    Protected WithEvents txtFFACondition As TextBox
    Protected WithEvents txtMIFrom As TextBox
    Protected WithEvents txtMITo As TextBox
    Protected WithEvents txtMICondition As TextBox
    Protected WithEvents txtDobiFrom As TextBox
    Protected WithEvents txtDobiTo As TextBox
    Protected WithEvents txtDobiCondition As TextBox
    Protected WithEvents txtPKMIFrom As TextBox
    Protected WithEvents txtPKMITo As TextBox
    Protected WithEvents txtPKMICondition As TextBox

    Protected WithEvents txtPKImpurityFrom As TextBox
    Protected WithEvents txtPKImpurityTo As TextBox
    Protected WithEvents txtPKImpurityCondition As TextBox


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
    Dim strOpCdGet As String = "CM_CLSSETUP_CLAIMQUALITY_GET"
    Dim strOpCdUpd As String = "CM_CLSSETUP_CLAIMQUALITY_UPD"
    Dim strOpCdAdd As String = "CM_CLSSETUP_CLAIMQUALITY_ADD"

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
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCMAccessRights.CMClaimQuality), intCMAR) = False Then
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

        If strCmdArgs = "Save" Then 
            blnIsUpdate = IIf(Cint(lblIsExist.text) = 0, False, True)

            strParam = txtFFAFrom.text & Chr(9) & _
                       txtFFATo.text & Chr(9) & _
                       txtFFACondition.text & Chr(9) & _
                       txtMIFrom.text & Chr(9) & _
                       txtMITo.text & Chr(9) & _
                       txtMICondition.text & Chr(9) & _
                       txtDobiFrom.text & Chr(9) & _
                       txtDobiTo.text & Chr(9) & _
                       txtDobiCondition.text & Chr(9) & _
                       txtPKMIFrom.text & Chr(9) & _
                       txtPKMITo.text & Chr(9) & _
                       txtPKMICondition.text & Chr(9) & _
                       txtPKImpurityFrom.text & Chr(9) & _
                       txtPKImpurityTo.text & Chr(9) & _
                       txtPKImpurityCondition.text & Chr(9) & _
                       objCMSetup.EnumClaimQualityStatus.Active 

            Try
                intErrNo = objCMSetup.mtdUpdClaimQuality(strOpCdAdd, _
                                                        strOpCdUpd, _
                                                        strCompany, _
                                                        strLocation, _
                                                        strUserId, _
                                                        strParam, _
                                                        blnIsUpdate)

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_SETUP_ClaimQuality_SAVE&errmesg=" & lblErrMessage.Text & "&redirect=CM/Setup/CM_setup_ClaimQuality.aspx")
            End Try
        End If
        onLoad_Display()
    End Sub

    Sub onLoad_Display()
        Dim strParam As String        
        Dim intErrNo As Integer
        Dim objCurrDs As New Object()        
        
        strParam =  "|" 

        Try
            intErrNo = objCMSetup.mtdGetMasterList(strOpCdGet, strParam, 0, objCurrDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_SETUP_ClaimQuality_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Setup/CM_setup_ClaimQuality.aspx")
        End Try

        lblIsExist.text = objCurrDs.Tables(0).rows.Count
	 
        if Cint(lblIsExist.text) > 0 then
            txtFFAFrom.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("CPOFFAFrom"))
            txtFFATo.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("CPOFFATo"))
            txtFFACondition.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("CPOFFACondition"))
            txtMIFrom.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("CPOMIFrom"))
            txtMITo.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("CPOMITo"))
            txtMICondition.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("CPOMICondition"))    
            txtDobiFrom.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("CPODobiFrom"))
            txtDobiTo.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("CPODobiTo"))     
            txtDobiCondition.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("CPODobiCondition"))
            txtPKMIFrom.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("PKMIFrom"))   
            txtPKMITo.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("PKMITo"))   
            txtPKMICondition.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("PKMICondition")) 

            txtPKImpurityFrom.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("PKImpurityFrom")) 
            txtPKImpurityTo.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("PKImpurityTo")) 
            txtPKImpurityCondition.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("PKImpurityCondition")) 
  

            lblHiddenSts.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("Status"))
            lblStatus.Text = objCMSetup.mtdGetClaimQualityStatus(Trim(objCurrDs.Tables(0).Rows(0).Item("Status")))
            lblDateCreated.Text = objGlobal.GetLongDate(objCurrDs.Tables(0).Rows(0).Item("CreateDate"))
            lblLastUpdate.Text = objGlobal.GetLongDate(objCurrDs.Tables(0).Rows(0).Item("UpdateDate"))
            lblUpdatedBy.Text = Trim(objCurrDs.Tables(0).Rows(0).Item("UserName"))        
        end if
    End Sub

			
    Sub fnCompareDec(Byval Sender As Object, Byval strFrom as string, Byval E As EventArgs)
        response.write("123")
        response.write("<br>")
        response.write(Sender.text)
        response.write("<br>")
        response.write(strFrom)
    End Sub

End Class
