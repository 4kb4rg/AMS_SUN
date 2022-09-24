Imports System
Imports System.Data
Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.Page
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic.Interaction
Imports Microsoft.VisualBasic.FileSystem

Imports System.Data.SqlClient
Imports Microsoft.VisualBasic

Imports agri.PWSystem.clsConfig
Imports agri.Admin.clsShare
Imports agri.GlobalHdl.clsGlobalHdl
Imports agri.GlobalHdl.clsAccessRights
Imports agri.BI.clsData



Public Class BI_data_trx_savefile : inherits page

    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objAdmShare As New agri.Admin.clsShare()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objBIData As New agri.BI.clsData()
    Dim objDownloadDs As New DataSet()
    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intBIAR As Integer

    Sub Page_Load(Sender As Object, E As EventArgs)
        Dim intAccMonth As Integer
        Dim intAccYear As Integer

        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strAccMonth = Session("SS_ARACCMONTH")
        strAccYear = Session("SS_ARACCYEAR")
        strLangCode = Session("SS_LANGCODE")
        intBIAR = Session("SS_BIAR")

        intAccMonth = CInt(strAccMonth)
        intAccYear = CInt(strAccYear)
        intAccMonth -= 1
        If intAccMonth = 0 Then
            intAccMonth = 12
            intAccYear -= 1
        End If
        strAccMonth = intAccMonth
        strAccYear = intAccYear

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumBIAccessRights.BIDownload), intBIAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            SaveFile()
        End If
    End Sub

    Sub SaveFile()
        Dim objStreamReader As StreamReader
        Dim strOpCd_MthEndTrx As String = "BI_CLSDATA_MTHENDTRX_DOWNLOAD"
        Dim strOpCd_DN As String = "BI_CLSDATA_DEBITNOTE_DOWNLOAD"
        Dim strOpCd_DNL As String = "BI_CLSDATA_DEBITNOTE_LINE_DOWNLOAD"
        Dim strOpCd_CN As String = "BI_CLSDATA_CREDITNOTE_DOWNLOAD"
        Dim strOpCd_CNL As String = "BI_CLSDATA_CREDITNOTE_LINE_DOWNLOAD"
        Dim strFtpPath As String
        Dim strXmlPath As String = ""
        Dim strXmlFileName As String = objGlobal.mtdGenerateDownloadFile(objGlobal.EnumMthEnd.BIMthEnd, False)
        Dim strZipFileName As String = Mid(strXmlFileName, 1, Len(strXmlFileName) - 3) & "zip"
        Dim strParam As String = ""
        Dim intErrNo As Integer
        Dim intPosted As Integer
        Dim strXmlb4Encrypt As String = ""
        Dim ObjXmlEncrypted As New Object()
        Dim ObjXmlDecrypted As New Object()

        Try
            intErrNo = objSysCfg.mtdGetFtpPath(strFtpPath)
            strXmlPath = strFtpPath & strXmlFileName
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BIDATA_TRX_GET_FTPPATH&errmesg=&redirect=")
        End Try

        Try
            strParam = strParam & strOpCd_MthEndTrx & "|" & objBIData.mtdGetDownloadTable(objBIData.EnumDownloadType.BIMthEndTrx) & Chr(9) & _
                                    strOpCd_DN & "|" & objBIData.mtdGetDownloadTable(objBIData.EnumDownloadType.BIDebitNote) & Chr(9) & _
                                    strOpCd_DNL & "|" & objBIData.mtdGetDownloadTable(objBIData.EnumDownloadType.BIDebitNoteLn) & Chr(9) & _
                                    strOpCd_CN & "|" & objBIData.mtdGetDownloadTable(objBIData.EnumDownloadType.BICreditNote) & Chr(9) & _
                                    strOpCd_CNL & "|" & objBIData.mtdGetDownloadTable(objBIData.EnumDownloadType.BICreditNoteLn) & Chr(9)

            intErrNo = objBIData.mtdDownloadRef(strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strAccMonth, _
                                                strAccYear, _
                                                strParam, _
                                                objDownloadDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BIDATA_TRX_DOWNLOAD_REF&errmesg=&redirect=")
        End Try

        objDownloadDs.WriteXml(strXmlPath, XmlWriteMode.WriteSchema)

        objStreamReader = File.OpenText(strXmlPath)
        strXmlb4Encrypt = objStreamReader.ReadToEnd()
        objStreamReader.Close()

        Try
            intErrNo = objSysCfg.mtdEncryptRef(strXmlb4Encrypt, objXmlEncrypted)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BIDATA_TRX_ENCRYPT_REF&errmesg=&redirect=")
        End Try

        Try
            strParam = objGlobal.EnumModule.Billing & "|" & objAdmShare.EnumMthEndPost.Yes
            intErrNo = objAdmShare.mtdUpdMonthEndPosted(strCompany, _
                                                        strLocation, _
                                                        strUserId, _
                                                        strAccMonth, _
                                                        strAccYear, _
                                                        "", _
                                                        strParam, _
                                                        intPosted)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BIDATA_TRX_POSTED_UPD&errmesg=&redirect=")
        End Try

        If intPosted = 2 Then
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BIDATA_TRX_NOT_CLOSED&errmesg=&redirect=")
        Else
            Response.Write(objXmlEncrypted)
            Response.ContentType = "application/zip"
            Response.AddHeader("Content-Disposition", _
                                "attachment; filename=""" & strZipFileName & """")
        End If
    End Sub

End Class

