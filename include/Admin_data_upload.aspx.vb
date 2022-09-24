
Imports System
Imports System.Data
Imports System.IO 
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic.Information
Imports Microsoft.VisualBasic.FileSystem
Imports Microsoft.VisualBasic
Imports agri.PWSystem.clsConfig
Imports agri.GlobalHdl.clsGlobalHdl
Imports agri.GlobalHdl.clsAccessRights
Imports agri.IN.clsData


Public Class Admin_data_upload : Inherits Page

    Protected WithEvents tblBefore As HtmlTable
    Protected WithEvents tblAfter As HtmlTable
    Protected WithEvents flUpload As HtmlInputFile
    Protected WithEvents lblErrUpload As Label
    Protected WithEvents lblErrMesage As Label
    Protected WithEvents lblErrNoFile As Label
    Protected WithEvents lblErrNoFileGlobal As Label
    Protected WithEvents tblBeforeGlobal As HtmlTable
    Protected WithEvents tblAfterGlobal As HtmlTable
    Protected WithEvents flUploadGlobal As HtmlInputFile
    Protected WithEvents lblErrUploadGlobal As Label

    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAdminData As New agri.Admin.clsData()
    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim intADAR As Integer

    Sub Page_Load(Sender As Object, E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        intADAR = Session("SS_ADAR")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumADAccessRights.ADAdmDataTransfer), intADAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            lblErrNoFile.Visible = False
            lblErrNoFileGlobal.Visible = False
            If Not Page.IsPostBack Then
                tblBefore.Visible = True
                tblAfter.Visible = False
                tblBeforeGlobal.Visible = True
                tblAfterGlobal.Visible = False
            End If
        End If
    End Sub

    Sub UploadBtn_Click(Sender As Object, E As ImageClickEventArgs)
        Dim objStreamReader As StreamReader
        Dim strZipPath As String = ""
        Dim strXmlPath As String = ""

        Dim arrZipPath As Array
        Dim strZipName As String = ""
        Dim strParam As String = ""
        Dim intErrNo As Integer
        Dim intFreeFile As Integer
        Dim strFtpPath As String
        Dim strXmlEncrypted As String = ""
        Dim objXmlDecrypted As New Object()
        Dim strErrMsg As String
        
        If Trim(flUpload.Value) = "" Then
            lblErrNoFile.Text = "Please select a file before clicking Upload button."
            lblErrNoFile.Visible = True
            Exit Sub
        ElseIf flUpload.PostedFile.ContentLength = 0 Then
            lblErrNoFile.Text = "The selected data transfer file is either not found or is corrupted."
            lblErrNoFile.Visible = True
            Exit Sub
        End If
        Try
            intErrNo = objSysCfg.mtdGetFtpPath(strFtpPath)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADMIN_GET_FTPPATH&errmesg=" & lblErrMesage.Text & "&redirect=admin/data/admin_data_upload.aspx")
        End Try        

        strZipPath = flUpload.PostedFile.FileName
        arrZipPath = Split(strZipPath, "\")
        strZipName = arrZipPath(UBound(arrZipPath))
        strZipPath = strFtpPath & strZipName
        If objGlobal.mtdValidateUploadFileName(strZipName, objGlobal.EnumDataTransferFileType.AD_AdministrationReferenceData, strErrMsg) = False Then
            lblErrNoFile.Text = "<br>" & strErrMsg
            lblErrNoFile.Visible = True
            Exit Sub
        End If
        strXmlPath = strFtpPath & Mid(strZipName, 1, Len(strZipName) - 3) & "xml"

        Dim Xmlfile As New FileInfo(strXmlPath)

        If Xmlfile.Exists Then
            File.Delete(strXmlPath)
        End If

        Try
            flUpload.PostedFile.SaveAs(strZipPath)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADMIN_SAVEAS&errmesg=" & lblErrUpload.Text & "&redirect=admin/data/admin_data_upload.aspx")
        End Try


        objStreamReader = File.OpenText(strZipPath)
        strXmlEncrypted = objStreamReader.ReadToEnd()
        objStreamReader.Close()

        Try
            intErrNo = objSysCfg.mtdDecryptRef(strXmlEncrypted, objXmlDecrypted)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADMIN_DECRYPT_REF&errmesg=" & lblErrMesage.Text & "&redirect=admin/data/admin_data_upload.aspx")
        End Try

        intFreeFile = FreeFile()
        FileOpen(intFreeFile, strXmlPath, 8)  
        Print(intFreeFile, objXmlDecrypted)
        FileClose(intFreeFile)

        Try
            intErrNo = objAdminData.mtdUploadRef(strXmlPath)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADMIN_UPLOAD_REF&errmesg=" & lblErrMesage.Text & "&redirect=admin/data/admin_data_upload.aspx")
        End Try
        tblBefore.Visible = False
        tblAfter.Visible = True
    End Sub

    Sub UploadGlobalBtn_Click(Sender As Object, E As ImageClickEventArgs)
        Dim objStreamReader As StreamReader
        Dim strZipPath As String = ""
        Dim strXmlPath As String = ""

        Dim arrZipPath As Array
        Dim strZipName As String = ""
        Dim strParam As String = ""
        Dim intErrNo As Integer
        Dim intFreeFile As Integer
        Dim strFtpPath As String
        Dim strXmlEncrypted As String = ""
        Dim objXmlDecrypted As New Object()
        Dim strErrMsg As String
        If Trim(flUploadGlobal.Value) = "" Then
            lblErrNoFileGlobal.Text = "Please select a file before clicking Upload button."
            lblErrNoFileGlobal.Visible = True
            Exit Sub
        ElseIf flUploadGlobal.PostedFile.ContentLength = 0 Then
            lblErrNoFileGlobal.Text = "The selected data transfer file is either not found or is corrupted."
            lblErrNoFileGlobal.Visible = True
            Exit Sub
        End If
        Try
            intErrNo = objSysCfg.mtdGetFtpPath(strFtpPath)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADMIN_GET_FTPPATH&errmesg=" & lblErrMesage.Text & "&redirect=admin/data/admin_data_upload.aspx")
        End Try        

        strZipPath = flUploadGlobal.PostedFile.FileName
        arrZipPath = Split(strZipPath, "\")
        strZipName = arrZipPath(UBound(arrZipPath))
        strZipPath = strFtpPath & strZipName
        If objGlobal.mtdValidateUploadFileName(strZipName, objGlobal.EnumDataTransferFileType.AD_GlobalReferenceData, strErrMsg) = False Then
            lblErrNoFileGlobal.Text = "<br>" & strErrMsg
            lblErrNoFileGlobal.Visible = True
            Exit Sub
        End If
        strXmlPath = strFtpPath & Mid(strZipName, 1, Len(strZipName) - 3) & "xml"

        Dim Xmlfile As New FileInfo(strXmlPath)

        If Xmlfile.Exists Then
            File.Delete(strXmlPath)
        End If

        Try
            flUploadGlobal.PostedFile.SaveAs(strZipPath)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADMIN_SAVEAS&errmesg=" & lblErrUpload.Text & "&redirect=admin/data/admin_data_upload.aspx")
        End Try


        objStreamReader = File.OpenText(strZipPath)
        strXmlEncrypted = objStreamReader.ReadToEnd()
        objStreamReader.Close()

        Try
            intErrNo = objSysCfg.mtdDecryptRef(strXmlEncrypted, objXmlDecrypted)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADMIN_DECRYPT_REF&errmesg=" & lblErrMesage.Text & "&redirect=admin/data/admin_data_upload.aspx")
        End Try

        intFreeFile = FreeFile()
        FileOpen(intFreeFile, strXmlPath, 8)  
        Print(intFreeFile, objXmlDecrypted)
        FileClose(intFreeFile)

        Try
            intErrNo = objAdminData.mtdUploadGlobalRef(strXmlPath, strErrMsg)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADMIN_UPLOAD_REF&errmesg=" & lblErrMesage.Text & "&redirect=admin/data/admin_data_upload.aspx")
        End Try

        If strErrMsg = "" Then
            tblBeforeGlobal.Visible = False
            tblAfterGlobal.Visible = True
            lblErrUploadGlobal.Visible = False
        Else
            lblErrUploadGlobal.Text = strErrMsg
            lblErrUploadGlobal.Visible = True
        End If
    End Sub
    
End Class
