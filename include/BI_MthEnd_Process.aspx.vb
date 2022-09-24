Imports System
Imports System.Data
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.Page
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic.Information

Imports agri.GlobalHdl.clsGlobalHdl
Imports agri.GlobalHdl.clsAccessRights
Imports agri.BI.clsMthEnd
Imports agri.Admin.clsShare
Imports agri.PWSystem.clsConfig


Public Class BI_mthend_Process : Inherits Page

    Protected WithEvents lblAccPeriod As Label
    Protected WithEvents lblStatus As Label
    Protected WithEvents lblLastProcessDate As Label
    Protected WithEvents btnProceed As ImageButton
    Protected WithEvents lblErrNotClose As Label
    Protected WithEvents lblErrProcess As Label
    Protected WithEvents lblErrInvoice As Label
    Protected WithEvents lblErrDebitNote As Label
    Protected WithEvents lblErrCreditNote As Label
    Protected WithEvents lblErrReceipt As Label
    Protected WithEvents lblErrDebtorJournal As Label
    Protected WithEvents cbIV As CheckBox
    Protected WithEvents cbCN As CheckBox
    Protected WithEvents cbDN As CheckBox
    Protected WithEvents cbRC As CheckBox
    Protected WithEvents cbDJ As CheckBox

    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAdminShare As New agri.Admin.clsShare()
    Dim objBIMthEnd As New agri.BI.clsMthEnd()
    Dim objSysCfg As New agri.PWSystem.clsConfig()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim intBIAR As Integer

    Sub Page_Load(Sender As Object, E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_ARACCMONTH")
        strAccYear = Session("SS_ARACCYEAR")
        intBIAR = Session("SS_BIAR")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumBIAccessRights.BIMonthEnd), intBIAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            lblErrNotClose.Visible = False
            lblErrProcess.Visible = False
            lblErrInvoice.Visible = False
            lblErrDebitNote.Visible = False
            lblErrCreditNote.Visible = False
            lblErrReceipt.Visible = False
            lblErrDebtorJournal.Visible = False

            If Not Page.IsPostBack Then
                onLoad_Display()
            End If
        End If
    End Sub

    Sub onLoad_Display()
        Dim intErrNo As Integer
        Dim strOpCd As String = "ADMIN_CLSSHARE_MTHEND_GET"
        Dim strParam As String = objGlobal.EnumModule.Billing
        Dim objResult As New Object()

        Try 
            intErrNo = objAdminShare.mtdMonthEnd(strCompany, _
                                                 strLocation, _
                                                 strUserId, _
                                                 strAccMonth, _
                                                 strAccYear, _
                                                 strOpCd, _
                                                 strParam, _
                                                 True, _
                                                 objResult)
        Catch Exp As System.Exception 
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_MTHEND_GET&errmesg=" & Exp.ToString & "&redirect=")
        End Try

        If objResult.Tables(0).Rows.Count > 0 Then
            lblLastProcessDate.Text = objGlobal.GetLongDate(objResult.Tables(0).Rows(0).Item("LastProcessDate"))
            lblStatus.Text = objAdminShare.mtdGetMtdEndClose(CInt(objResult.Tables(0).Rows(0).Item("CloseInd")))
            lblAccPeriod.Text = objResult.Tables(0).Rows(0).Item("CurrAccMonth").Trim() & "/" & objResult.Tables(0).Rows(0).Item("CurrAccYear").Trim()
            If (CInt(objResult.Tables(0).Rows(0).Item("CurrAccMonth").Trim()) = CInt(strAccMonth)) And _
               (CInt(objResult.Tables(0).Rows(0).Item("CurrAccYear").Trim()) = CInt(strAccYear)) And _
               (CInt(objResult.Tables(0).Rows(0).Item("CloseInd")) = objAdminShare.EnumMthEndClose.No) Then
                btnProceed.Visible = True
            Else
                btnProceed.Visible = False
            End If
        Else
            btnProceed.Visible = True
            lblStatus.Text = ""
            lblLastProcessDate.Text = ""
            lblAccPeriod.Text = strAccMonth & "/" & strAccYear
        End If        
    End Sub

    Sub btnProceed_Click(Sender As Object, E As ImageClickEventArgs)
        Dim intErrNo As Integer
        Dim strOpCd As String = "ADMIN_CLSSHARE_MTHEND_GET"
        Dim strOpCode_GetSysLoc As String = "PWSYSTEM_CLSCONFIG_SYSLOC_DETAILS_GET"
        Dim objSysLocDs As New Dataset()
        Dim strParam As String
        Dim objResult As Integer
        Try 
            strParam = objGlobal.EnumModule.Billing & "|" & _
                        cbIV.Checked & Chr(9) & _
                        cbDN.Checked & Chr(9) & _
                        cbCN.Checked & Chr(9) & _
                        cbRC.Checked & Chr(9) & _
                        cbDJ.Checked

            intErrNo = objBIMthEnd.mtdMonthEndProcess(strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strAccMonth, _
                                                    strAccYear, _
                                                    strOpCd, _
                                                    strParam, _
                                                    Session("SS_PPNRATE"), _
                                                    objResult)
        Catch Exp As System.Exception 
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_MTHEND_PROCESS&errmesg=" & Exp.ToString & "&redirect=")
        End Try

        If objResult = 1 Then 
            strParam = strCompany & "|" & strLocation & "|" & strUserId
            Try
                intErrNo = objSysCfg.mtdGetSysLocInfo(strOpCode_GetSysLoc, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    objSysLocDs, _
                                                    strParam)
            Catch Exp As System.Exception 
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BIMTHEND_GET_SYSLOC&errmesg=" & Exp.ToString() & "&redirect=BI/mthend/BI_MthEnd_Process.aspx")
            End Try

            Session("SS_ARACCMONTH") = objSysLocDs.Tables(0).Rows(0).Item("ARAccMonth").Trim()
            Session("SS_ARACCYEAR") = objSysLocDs.Tables(0).Rows(0).Item("ARAccYear").Trim()
            strAccMonth = objSysLocDs.Tables(0).Rows(0).Item("ARAccMonth").Trim()
            strAccYear = objSysLocDs.Tables(0).Rows(0).Item("ARAccYear").Trim()
            onLoad_Display()
        Else
            Select Case objResult
                Case 0
                    lblErrNotClose.Visible = True
                Case 2
                    lblErrProcess.Visible = True
                Case 9
                    lblErrInvoice.Visible = True
                Case 10
                    lblErrDebitNote.Visible = True
                Case 11
                    lblErrCreditNote.Visible = True
                Case 12
                    lblErrReceipt.Visible = True
                Case 13
                    lblErrDebtorJournal.Visible = True
            End Select
        End If
    End Sub


End Class
