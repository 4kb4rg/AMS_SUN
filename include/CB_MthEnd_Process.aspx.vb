Imports System
Imports System.Data


Imports agri.GlobalHdl.clsGlobalHdl
Imports agri.GlobalHdl.clsAccessRights
Imports agri.CB.clsMthEnd
Imports agri.Admin.clsShare
Imports agri.PWSystem.clsConfig


Public Class CB_mthend_Process : Inherits Page

    Protected WithEvents lblAccPeriod As Label
    Protected WithEvents lblStatus As Label
    Protected WithEvents lblLastProcessDate As Label
    Protected WithEvents btnProceed As ImageButton
    Protected WithEvents lblErrNotClose As Label
    Protected WithEvents lblErrProcess As Label
    Protected WithEvents lblErrPayment As Label
    Protected WithEvents lblErrReceipt As Label
    Protected WithEvents lblErrDeposit As Label
    Protected WithEvents lblErrWithdrawal As Label    
    Protected WithEvents lblErrInterAdj As Label

    Protected WithEvents CloseInd_Final As RadioButton
    Protected WithEvents CloseInd_Temporary As RadioButton
    Protected WithEvents CloseInd_Rollback As RadioButton

    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAdminShare As New agri.Admin.clsShare()
    Dim objMthEndDs As New agri.CB.clsMthEnd()
    Dim objSysCfg As New agri.PWSystem.clsConfig()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strSelAccMonth As String
    Dim strSelAccYear As String
    Dim strClsAccMonth As String
    Dim strClsAccYear As String
    Dim intCBAR As Integer

    Sub Page_Load(Sender As Object, E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_CBACCMONTH")
        strAccYear = Session("SS_CBACCYEAR")
        intCBAR = Session("SS_CBAR")
        strSelAccMonth = Session("SS_SELACCMONTH")
        strSelAccYear = Session("SS_SELACCYEAR")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCBAccessRights.CBMthEnd), intCBAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            lblErrNotClose.Visible = False
            lblErrProcess.Visible = False
            lblErrPayment.Visible = False
            lblErrReceipt.Visible = False
            lblErrDeposit.Visible = False
            lblErrWithdrawal.Visible = False
            lblErrInterAdj.Visible = False

            If Not Page.IsPostBack Then
                onLoad_Display()
            End If
        End If
    End Sub

    Sub onLoad_Display()
        Dim intErrNo As Integer
        Dim strOpCd As String = "ADMIN_CLSSHARE_MTHEND_GET"
        Dim strParam As String = objGlobal.EnumModule.CashAndBank
        Dim objResult As New Object()

        If CloseInd_Final.Checked = True Then
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
            Catch Exp As Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_MTHEND_GET&errmesg=" & Exp.ToString & "&redirect=")
            End Try

            If objResult.Tables(0).Rows.Count > 0 Then
                lblLastProcessDate.Text = objGlobal.GetLongDate(objResult.Tables(0).Rows(0).Item("LastProcessDate"))
                lblStatus.Text = objAdminShare.mtdGetMtdEndClose(CInt(objResult.Tables(0).Rows(0).Item("CloseInd")))
                lblAccPeriod.Text = objResult.Tables(0).Rows(0).Item("CurrAccMonth").Trim() & "/" & objResult.Tables(0).Rows(0).Item("CurrAccYear").Trim()
                strClsAccMonth = objResult.Tables(0).Rows(0).Item("CurrAccMonth").Trim()
                strClsAccYear = objResult.Tables(0).Rows(0).Item("CurrAccYear").Trim()
                If (CInt(objResult.Tables(0).Rows(0).Item("CloseInd")) = objAdminShare.EnumMthEndClose.No) Then
                    btnProceed.Visible = True
                Else
                    btnProceed.Visible = False
                End If
                'If (CInt(objResult.Tables(0).Rows(0).Item("CurrAccMonth").Trim()) = CInt(strAccMonth)) And _
                '   (CInt(objResult.Tables(0).Rows(0).Item("CurrAccYear").Trim()) = CInt(strAccYear)) And _
                '   (CInt(objResult.Tables(0).Rows(0).Item("CloseInd")) = objAdminShare.EnumMthEndClose.No) Then
                '    btnProceed.Visible = True
                'Else
                '    btnProceed.Visible = False
                'End If
            Else
                btnProceed.Visible = True
                lblStatus.Text = ""
                lblLastProcessDate.Text = ""
                lblAccPeriod.Text = strAccMonth & "/" & strAccYear
            End If
        Else
            Dim strOpCode_GetSysLoc As String = "PWSYSTEM_CLSCONFIG_SYSLOC_DETAILS_GET"
            Dim objSysLocDs As New DataSet()
            strParam = strCompany & "|" & strLocation & "|" & strUserId
            Try
                intErrNo = objSysCfg.mtdGetSysLocInfo(strOpCode_GetSysLoc, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    objSysLocDs, _
                                                    strParam)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PUMTHEND_GET_SYSLOC&errmesg=" & Exp.ToString() & "&redirect=PU/mthend/PU_MthEnd_Process.aspx")
            End Try

            Session("SS_CBACCMONTH") = objSysLocDs.Tables(0).Rows(0).Item("CBAccMonth").Trim()
            Session("SS_CBACCYEAR") = objSysLocDs.Tables(0).Rows(0).Item("CBAccYear").Trim()
            strAccMonth = objSysLocDs.Tables(0).Rows(0).Item("CBAccMonth").Trim()
            strAccYear = objSysLocDs.Tables(0).Rows(0).Item("CBAccYear").Trim()

            'lblLastProcessDate.Text = objGlobal.GetLongDate(objResult.Tables(0).Rows(0).Item("LastProcessDate"))
            lblStatus.Text = objAdminShare.mtdGetMtdEndClose(objAdminShare.EnumMthEndClose.No)
            lblAccPeriod.Text = strAccMonth & "/" & strAccYear
            btnProceed.Visible = True
        End If
        
    End Sub

    Sub CloseInd_OnCheckChange(ByVal Sender As Object, ByVal E As EventArgs)
        Dim intErrNo As Integer
        Dim strOpCd As String = "ADMIN_CLSSHARE_MTHEND_GET"
        Dim strOpCode_GetSysLoc As String = "PWSYSTEM_CLSCONFIG_SYSLOC_DETAILS_GET"
        Dim objSysLocDs As New DataSet()
        Dim strParam As String = objGlobal.EnumModule.Purchasing
        Dim objResult As New Object()

        If CloseInd_Final.Checked = True Then
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
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PU_MTHEND_GET&errmesg=" & Exp.ToString & "&redirect=")
            End Try

            If objResult.Tables(0).Rows.Count > 0 Then
                lblLastProcessDate.Text = objGlobal.GetLongDate(objResult.Tables(0).Rows(0).Item("LastProcessDate"))
                lblStatus.Text = objAdminShare.mtdGetMtdEndClose(CInt(objResult.Tables(0).Rows(0).Item("CloseInd")))
                lblAccPeriod.Text = objResult.Tables(0).Rows(0).Item("CurrAccMonth").Trim() & "/" & objResult.Tables(0).Rows(0).Item("CurrAccYear").Trim()
                strClsAccMonth = objResult.Tables(0).Rows(0).Item("CurrAccMonth").Trim()
                strClsAccYear = objResult.Tables(0).Rows(0).Item("CurrAccYear").Trim()
                If (CInt(objResult.Tables(0).Rows(0).Item("CloseInd")) = objAdminShare.EnumMthEndClose.No) Then
                    btnProceed.Visible = True
                Else
                    btnProceed.Visible = False
                End If

                'If (CInt(objResult.Tables(0).Rows(0).Item("CurrAccMonth").Trim()) = CInt(strAccMonth)) And _
                '   (CInt(objResult.Tables(0).Rows(0).Item("CurrAccYear").Trim()) = CInt(strAccYear)) And _
                '   (CInt(objResult.Tables(0).Rows(0).Item("CloseInd")) = objAdminShare.EnumMthEndClose.No) Then
                '    btnProceed.Visible = True
                'Else
                '    btnProceed.Visible = False
                'End If
            Else
                btnProceed.Visible = True
                lblStatus.Text = ""
                lblLastProcessDate.Text = ""
                lblAccPeriod.Text = strAccMonth & "/" & strAccYear
            End If
        Else
            strParam = strCompany & "|" & strLocation & "|" & strUserId
            Try
                intErrNo = objSysCfg.mtdGetSysLocInfo(strOpCode_GetSysLoc, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    objSysLocDs, _
                                                    strParam)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PUMTHEND_GET_SYSLOC&errmesg=" & Exp.ToString() & "&redirect=PU/mthend/PU_MthEnd_Process.aspx")
            End Try

            Session("SS_CBACCMONTH") = objSysLocDs.Tables(0).Rows(0).Item("CBAccMonth").Trim()
            Session("SS_CBACCYEAR") = objSysLocDs.Tables(0).Rows(0).Item("CBAccYear").Trim()
            strAccMonth = objSysLocDs.Tables(0).Rows(0).Item("CBAccMonth").Trim()
            strAccYear = objSysLocDs.Tables(0).Rows(0).Item("CBAccYear").Trim()

            'lblLastProcessDate.Text = objGlobal.GetLongDate(objResult.Tables(0).Rows(0).Item("LastProcessDate"))
            lblStatus.Text = objAdminShare.mtdGetMtdEndClose(objAdminShare.EnumMthEndClose.No)
            lblAccPeriod.Text = strAccMonth & "/" & strAccYear
            btnProceed.Visible = True
        End If

    End Sub

    Sub btnProceed_Click(Sender As Object, E As ImageClickEventArgs)
        Dim intErrNo As Integer
        Dim strOpCd As String = "ADMIN_CLSSHARE_MTHEND_GET"
        Dim strOpCode_GetSysLoc As String = "PWSYSTEM_CLSCONFIG_SYSLOC_DETAILS_GET"
        Dim objSysLocDs As New Dataset()
        Dim strParam As String = objGlobal.EnumModule.CashAndBank & "|" & True
        Dim objResult As Integer
        Dim intCurPeriod As Integer = (CInt(strAccYear) * 100) + CInt(strAccMonth)
        Dim intSelPeriod As Integer = (CInt(strSelAccYear) * 100) + CInt(strSelAccMonth)
        Dim intClsPeriod As Integer = (CInt(strClsAccYear) * 100) + CInt(strClsAccMonth)

        If Session("SS_FILTERPERIOD") = "0" Then
            If intSelPeriod < intCurPeriod Then
                lblErrNotClose.Visible = True
                lblErrNotClose.Text = "Invalid Period"
                Exit Sub
            End If
        Else
            If intSelPeriod < intCurPeriod Then
                lblErrNotClose.Visible = True
                lblErrNotClose.Text = "Invalid Period"
                Exit Sub
            End If
            If CloseInd_Final.Checked = True Then
                If intSelPeriod <> intClsPeriod Then
                    lblErrNotClose.Visible = True
                    lblErrNotClose.Text = "Invalid Period"
                    Exit Sub
                End If
            End If
        End If

        If CloseInd_Final.Checked = True Then
            Try
                intErrNo = objMthEndDs.mtdMonthEndProcess(strCompany, _
                                                        strLocation, _
                                                        strUserId, _
                                                        strAccMonth, _
                                                        strAccYear, _
                                                        strOpCd, _
                                                        strParam, _
                                                        Session("SS_PPNRATE"), _
                                                        objResult)
            Catch Exp As Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_MTHEND_PROCESS&errmesg=" & Exp.ToString & "&redirect=")
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
                Catch Exp As Exception
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CBMTHEND_GET_SYSLOC&errmesg=" & Exp.ToString() & "&redirect=CB/mthend/CB_MthEnd_Process.aspx")
                End Try

                Session("SS_CBACCMONTH") = objSysLocDs.Tables(0).Rows(0).Item("CBAccMonth").Trim()
                Session("SS_CBACCYEAR") = objSysLocDs.Tables(0).Rows(0).Item("CBAccYear").Trim()
                Session("SS_PHYMONTH") = objSysLocDs.Tables(0).Rows(0).Item("PhyMonth").Trim()
                Session("SS_PHYYEAR") = objSysLocDs.Tables(0).Rows(0).Item("PhyYear").Trim()
                strAccMonth = objSysLocDs.Tables(0).Rows(0).Item("CBAccMonth").Trim()
                strAccYear = objSysLocDs.Tables(0).Rows(0).Item("CBAccYear").Trim()
                onLoad_Display()
            Else
                Select Case objResult
                    Case 0
                        lblErrNotClose.Visible = True
                    Case 2
                        lblErrProcess.Visible = True
                    Case 9
                        lblErrPayment.Visible = True
                    Case 10
                        lblErrReceipt.Visible = True
                    Case 11
                        lblErrDeposit.Visible = True
                    Case 12
                        lblErrWithdrawal.Visible = True
                    Case 13
                        lblErrInterAdj.Visible = True
                    Case 14
                        lblErrInterAdj.Text = "Please Defined Double Entry"
                        lblErrInterAdj.Visible = True
                    Case 15
                        lblErrInterAdj.Text = "There are some pending cash bank documents waiting to confirm."
                        lblErrInterAdj.Visible = True
                End Select
            End If

        Else
            'Temporary and Rollback Temporary Closing
            'Only update AccMonth/AccYear on AD_SYSCFG
            Dim strOpCodeTemp As String = "PWSYSTEM_CLSCONFIG_SYSLOC_ACCPERIOD_UPD_TEMP"
            Dim strParamName As String = ""
            Dim strParamValue As String = ""
            Dim clsAccMonth As String
            Dim clsAccyear As String
            Dim objGLtrx As New agri.GL.ClsTrx()

            strParam = strCompany & "|" & strLocation & "|" & strUserId
            Try
                intErrNo = objSysCfg.mtdGetSysLocInfo(strOpCode_GetSysLoc, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    objSysLocDs, _
                                                    strParam)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PUMTHEND_GET_SYSLOC&errmesg=" & Exp.ToString() & "&redirect=PU/mthend/PU_MthEnd_Process.aspx")
            End Try

            If CloseInd_Temporary.Checked = True Then
                clsAccyear = objSysLocDs.Tables(0).Rows(0).Item("CBAccYear").Trim()
                clsAccMonth = Val(objSysLocDs.Tables(0).Rows(0).Item("CBAccMonth").Trim()) + 1
                If clsAccMonth > 12 Then
                    clsAccMonth = 1
                    clsAccyear = Val(clsAccyear) + 1
                End If

            ElseIf CloseInd_Rollback.Checked = True Then
                clsAccyear = objSysLocDs.Tables(0).Rows(0).Item("CBAccYear").Trim()
                clsAccMonth = Val(objSysLocDs.Tables(0).Rows(0).Item("CBAccMonth").Trim()) - 1
                If clsAccMonth = 0 Then
                    clsAccMonth = 12
                    clsAccyear = Val(clsAccyear) - 1
                End If
            End If

            strParamName = "LOCCODE|COMPCODE|STRUPDATE"
            strParamValue = Trim(strLocation) & "|" & Trim(strCompany) & "|" & _
                            "CBACCMONTH='" & clsAccMonth & "', CBACCYEAR='" & clsAccyear & "' "

            Try
                intErrNo = objGLtrx.mtdInsertDataCommon(strOpCodeTemp, _
                                                        strParamName, _
                                                        strParamValue)

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PU_MTHEND_PROCESS&errmesg=" & Exp.ToString & "&redirect=")
            End Try

            strParam = strCompany & "|" & strLocation & "|" & strUserId
            Try
                intErrNo = objSysCfg.mtdGetSysLocInfo(strOpCode_GetSysLoc, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    objSysLocDs, _
                                                    strParam)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PUMTHEND_GET_SYSLOC&errmesg=" & Exp.ToString() & "&redirect=PU/mthend/PU_MthEnd_Process.aspx")
            End Try

            Session("SS_CBAccMonth") = objSysLocDs.Tables(0).Rows(0).Item("CBAccMonth").Trim()
            Session("SS_CBAccYear") = objSysLocDs.Tables(0).Rows(0).Item("CBAccYear").Trim()
            strAccMonth = objSysLocDs.Tables(0).Rows(0).Item("CBAccMonth").Trim()
            strAccYear = objSysLocDs.Tables(0).Rows(0).Item("CBAccYear").Trim()
            onLoad_Display()
        End If
        
    End Sub

End Class
