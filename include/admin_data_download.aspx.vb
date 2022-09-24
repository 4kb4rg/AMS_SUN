
Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports System.Data
Imports Microsoft.VisualBasic.Strings
Imports agri.GlobalHdl.clsAccessRights
Imports Microsoft.VisualBasic

Public Class admin_data_download : Inherits Page

    Protected WithEvents tblDownload As HtmlTable
    Protected WithEvents tblSave As HtmlTable

    Protected WithEvents cbCompany As CheckBox
    Protected WithEvents cbLocation As CheckBox
    Protected WithEvents cbUOM As CheckBox
    Protected WithEvents cbUOMCon As CheckBox
    Protected WithEvents lblErrGenerate As Label
    Protected WithEvents lblDownloadfile As Label
    Protected WithEvents lblErrMesage As Label
    Protected WithEvents btnGenerate As ImageButton
    Protected WithEvents lnkSaveTheFile As Hyperlink
    Protected WithEvents cbAllGlobal As CheckBox
    Protected WithEvents lblErrGenerateGlobal As Label
    Protected WithEvents btnGenerateGlobal As ImageButton
    Protected WithEvents lblGlobalAllLoc As Label

    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim intADAR As Integer
    Dim dsLangCap As DataSet
    Dim strLocType as String
    

    Sub Page_Load(Sender As Object, E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        intADAR = Session("SS_ADAR")
        strLocType = Session("SS_LOCTYPE")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumADAccessRights.ADAdmDataTransfer), intADAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            If Not Page.IsPostBack Then
                tblDownload.Visible = True
                tblSave.Visible = False
                LangCapHdl()
            Else
                tblDownload.Visible = True
                tblSave.Visible = False
            End If
        End If
    End Sub


    Sub btnGenerate_Click(Sender As Object, E As ImageClickEventArgs)
        Dim strParam As String = ""
        Dim strQuery As String = ""

        If cbCompany.Checked Then strParam = strParam & "cbCompany"
        If cbLocation.Checked Then strParam = strParam & "cbLocation"
        If cbUOM.Checked Then strParam = strParam & "cbUOM"
        If cbUOMCon.Checked Then strParam = strParam & "cbUOMCon"

        If strParam = "" Then
            lblErrGenerate.Visible = True
        Else
            strQuery = "company=" & cbCompany.Checked & "&" & _
                    "location=" & cbLocation.Checked & "&" & _
                    "uom=" & cbUOM.Checked & "&" & _
                    "uomcon=" & cbUOMCon.Checked
            Response.Redirect("admin_data_download_savefile.aspx?global=no&" & strQuery)
        End If
    End Sub

    Sub LangCapHdl()
        Dim strLocationTag As String
        If Trim(Session("SS_LOCATION")) = "" Then
            dsLangCap = GetLanguageCaptionDS()
            strLocationTag = GetCaption(objLangCap.EnumLangCap.Location)
            lblGlobalAllLoc.Text = "You have not login to any " & LCase(strLocationTag) & " yet. Data from all " & LCase(strLocationTag) & "s will be downloaded if it is " & LCase(strLocationTag) & "-specific."
            lblGlobalAllLoc.Visible = True
        Else
            lblGlobalAllLoc.Visible = False
        End If
    End Sub
    
    Sub btnGenerateGlobal_Click(Sender As Object, E As ImageClickEventArgs)
        Dim strLocationTag As String
        Dim strScript As String
        
        If cbAllGlobal.Checked Then
            If Trim(Session("SS_LOCATION")) = "" Then
                Response.Redirect("admin_data_download_savefile.aspx?global=yes&noloc=yes")
            Else
                Response.Redirect("admin_data_download_savefile.aspx?global=yes&noloc=no")
            End If
            lblErrGenerateGlobal.Visible = False
        Else
            lblErrGenerateGlobal.Visible = True
        End If
    End Sub
    
    Function GetLanguageCaptionDS() As DataSet
        Dim strOpCode_BussTerm As String = "PWSYSTEM_CLSLANGCAP_BUSSTERM_GET"
        Dim dsLC As DataSet
        Dim strParam As String
        Dim intErrNo As Integer
        
        strParam = strLangCode
        Try
            intErrNo = objLangCap.mtdGetBussTerm(strOpCode_BussTerm, _
                                                 strCompany, _
                                                 strLocation, _
                                                 strUserId, _
                                                 "", _
                                                 "", _
                                                 dsLC, _
                                                 strParam)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADMIN_GET_LANGCAP&errmesg=&redirect=")
        End Try
        
        Return dsLC
        If Not dsLC Is Nothing Then
            dsLC = Nothing
        End If
    End Function
    
    Function GetCaption(ByVal pv_TermCode) As String
            Dim count As Integer

            For count = 0 To dsLangCap.Tables(0).Rows.Count - 1
                If Trim(pv_TermCode) = Trim(dsLangCap.Tables(0).Rows(count).Item("TermCode")) Then
                    If strLocType = objAdminLoc.EnumLocType.Mill then
                        Return Trim(dsLangCap.Tables(0).Rows(count).Item("BusinessTermMW"))
                    else
                        Return Trim(dsLangCap.Tables(0).Rows(count).Item("BusinessTerm"))
                    end if
                    Exit For
                End If
            Next
        End Function

End Class
