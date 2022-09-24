
Imports System
Imports System.Data
Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic.Interaction
Imports Microsoft.VisualBasic
Imports agri.Admin.clsAccPeriod
Imports agri.GlobalHdl


Public Class admin_accperiod : Inherits Page

    Protected WithEvents ddlINAccMonth As DropDownList
    Protected WithEvents ddlNUAccMonth As DropDownList
    Protected WithEvents ddlPUAccMonth As DropDownList
    Protected WithEvents ddlAPAccMonth As DropDownList
    Protected WithEvents ddlPRAccMonth As DropDownList
    Protected WithEvents ddlPDAccMonth As DropDownList
    Protected WithEvents ddlARAccMonth As DropDownList
    Protected WithEvents ddlGLAccMonth As DropDownList
    Protected WithEvents ddlFAAccMonth AS DropDownList
    Protected WithEvents ddlCBAccMonth AS DropDownList
    Protected WithEvents ddlPhyMonth As DropDownList
    Protected WithEvents txtINAccYear As TextBox
    Protected WithEvents txtNUAccYear As TextBox
    Protected WithEvents txtPUAccYear As TextBox
    Protected WithEvents txtAPAccYear As TextBox
    Protected WithEvents txtPRAccYear As TextBox
    Protected WithEvents txtPDAccYear As TextBox
    Protected WithEvents txtARAccYear As TextBox
    Protected WithEvents txtGLAccYear As TextBox
    Protected WithEvents txtFAAccYear As TextBox
    Protected WithEvents txtCBAccYear As TextBox
    Protected WithEvents txtPhyYear As TextBox
    Protected WithEvents SaveBtn As ImageButton
    Protected WithEvents lblErrAccPeriod As Label
    Protected WithEvents lblMyLocation As Label

    Protected WithEvents hidPeriodData As HtmlInputHidden

    Protected objAdmin As New agri.Admin.clsAccPeriod()
    Protected objAdminShr As New agri.Admin.clsShare()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim intADAR As Integer
    Dim intModuleActivate As Integer
    Dim intMaxPeriod As Integer
    Dim dsPeriod As New DataSet
    Dim strLocType As String

  
    Sub Page_Load(Sender As Object, E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        intADAR = Session("SS_ADAR")
        intModuleActivate = Session("SS_MODULEACTIVATE")
        intMaxPeriod = Session("SS_MAXPERIOD")
        strLocType = Session("SS_LOCTYPE")

        lblMyLocation.Text = strLocation

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumADAccessRights.ADAccPeriod), intADAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            SaveBtn.Visible = True
            lblErrAccPeriod.Visible = False
            LicCheck()
            PopulateAccountingPeriodData()
            If Not IsPostBack Then
                BindPeriod()
                onLoad_Display()

                Session("SS_LASTPHYYEAR") = txtPhyYear.Text

            End If
        End If
    End Sub

    Function BindPeriod() As Boolean
        Dim intPhyMonth As Integer
        Dim intAccMonth As Integer
        ddlPhyMonth.Items.Add(New ListItem("JAN","1"))
        ddlPhyMonth.Items.Add(New ListItem("FEB","2"))
        ddlPhyMonth.Items.Add(New ListItem("MAR","3"))
        ddlPhyMonth.Items.Add(New ListItem("APR","4"))
        ddlPhyMonth.Items.Add(New ListItem("MAY","5"))
        ddlPhyMonth.Items.Add(New ListItem("JUN","6"))
        ddlPhyMonth.Items.Add(New ListItem("JUL","7"))
        ddlPhyMonth.Items.Add(New ListItem("AUG","8"))
        ddlPhyMonth.Items.Add(New ListItem("SEP","9"))
        ddlPhyMonth.Items.Add(New ListItem("OCT","10"))
        ddlPhyMonth.Items.Add(New ListItem("NOV","11"))
        ddlPhyMonth.Items.Add(New ListItem("DEC","12"))
    End Function

    Sub ShowAccountingPeriod(ByRef ddlAccMonth As DropDownList, ByRef txtAccYear As TextBox, ByVal strAccMonth As Object, ByVal strAccYear As Object)
        Dim intCnt As Integer
        Dim intMonth As Integer = 12
        
        If IsNumeric(strAccYear) = True Then
            For intCnt = 0 To dsPeriod.Tables(0).Rows.Count - 1
                If Trim(dsPeriod.Tables(0).Rows(intCnt).Item("AccYear")) = Trim(strAccYear) Then
                    intMonth = CInt(Trim(dsPeriod.Tables(0).Rows(intCnt).Item("MaxPeriod")))
                    Exit For
                End If
            Next
        Else
            strAccYear = Year(Now())
        End If
        
        If IsNumeric(strAccMonth) = False Then
            strAccMonth = Month(Now())
        End If
        
        ddlAccMonth.Items.Clear()
        For intCnt = 1 To intMonth
            ddlAccMonth.Items.Add(New ListItem(intCnt, intCnt))
        Next
        If CInt(strAccMonth) > intMonth Then
            ddlAccMonth.Items.Add(New ListItem(CInt(strAccMonth), CInt(strAccMonth)))
            ddlAccMonth.SelectedIndex = ddlAccMonth.Items.Count - 1
        Else
            ddlAccMonth.SelectedIndex = CInt(strAccMonth) - 1
        End If
        
        txtAccYear.Text = strAccYear
    End Sub
    
    Sub PopulateAccountingPeriodData()
        Dim strOpCd_GET As String = "ADMIN_CLSACCPERIOD_CONFIG_GET"
        Dim strParam As String
        Dim intCnt As Integer
        Dim intErrNo As Integer
        
        strParam = "||"
        Try
           intErrNo = objAdmin.mtdGetAccPeriodCfg(strOpCd_GET, _
                                                  strCompany, _
                                                  strLocation, _
                                                  strUserId, _
                                                  strParam, _
                                                  dsPeriod)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADMIN_ACCPERIOD_CFG_GET&errmesg=" & Exp.ToString() & "&redirect=")
        End Try
        
        strParam = ""
        For intCnt = 0 To dsPeriod.Tables(0).Rows.Count - 1
            strParam = strParam & ";" & dsPeriod.Tables(0).Rows(intCnt).Item("MaxPeriod").Trim() & "/" & dsPeriod.Tables(0).Rows(intCnt).Item("AccYear").Trim()
        Next
        If strParam <> "" Then
            strParam = Mid(strParam, 2)
        End If
        hidPeriodData.Value = strParam
    End Sub
    Function fnGetValueFromDropDownList(ByRef ddlObject As DropDownList) As String
        If Trim(Request.Form(ddlObject.ID)) <> "" Then
            fnGetValueFromDropDownList = Trim(Request.Form(ddlObject.ID))
        Else
            fnGetValueFromDropDownList = ddlObject.SelectedItem.Value
        End If
    End Function
    
    Sub SynchronizeViewStateData()
        ShowAccountingPeriod(ddlINAccMonth, txtINAccYear, fnGetValueFromDropDownList(ddlINAccMonth), txtINAccYear.Text)
        ShowAccountingPeriod(ddlNUAccMonth, txtNUAccYear, fnGetValueFromDropDownList(ddlNUAccMonth), txtNUAccYear.Text)
        ShowAccountingPeriod(ddlPUAccMonth, txtPUAccYear, fnGetValueFromDropDownList(ddlPUAccMonth), txtPUAccYear.Text)
        ShowAccountingPeriod(ddlAPAccMonth, txtAPAccYear, fnGetValueFromDropDownList(ddlAPAccMonth), txtAPAccYear.Text)
        ShowAccountingPeriod(ddlPRAccMonth, txtPRAccYear, fnGetValueFromDropDownList(ddlPRAccMonth), txtPRAccYear.Text)
        ShowAccountingPeriod(ddlPDAccMonth, txtPDAccYear, fnGetValueFromDropDownList(ddlPDAccMonth), txtPDAccYear.Text)
        ShowAccountingPeriod(ddlFAAccMonth, txtFAAccYear, fnGetValueFromDropDownList(ddlFAAccMonth), txtFAAccYear.Text)
        ShowAccountingPeriod(ddlARAccMonth, txtARAccYear, fnGetValueFromDropDownList(ddlARAccMonth), txtARAccYear.Text)
        ShowAccountingPeriod(ddlGLAccMonth, txtGLAccYear, fnGetValueFromDropDownList(ddlGLAccMonth), txtGLAccYear.Text)
        ShowAccountingPeriod(ddlCBAccMonth, txtGLAccYear, fnGetValueFromDropDownList(ddlCBAccMonth), txtCBAccYear.Text)
    End Sub

    Sub LicCheck()
        If objAdminShr.mtdModuleCheck(intModuleActivate, objGlobal.mtdGetModuleCode(objGlobal.EnumModule.Inventory)) = False And _
            objAdminShr.mtdModuleCheck(intModuleActivate, objGlobal.mtdGetModuleCode(objGlobal.EnumModule.Canteen)) = False And _
            objAdminShr.mtdModuleCheck(intModuleActivate, objGlobal.mtdGetModuleCode(objGlobal.EnumModule.Workshop)) = False Then
            ddlINAccMonth.Enabled = False
            txtINAccYear.Enabled = False
        End If

        If objAdminShr.mtdModuleCheck(intModuleActivate, objGlobal.mtdGetModuleCode(objGlobal.EnumModule.Nursery)) = False Then
            ddlNUAccMonth.Enabled = False
            txtNUAccYear.Enabled = False
        End If

        If objAdminShr.mtdModuleCheck(intModuleActivate, objGlobal.mtdGetModuleCode(objGlobal.EnumModule.Purchasing)) = False Then
            ddlPUAccMonth.Enabled = False
            txtPUAccYear.Enabled = False
        End If

        If objAdminShr.mtdModuleCheck(intModuleActivate, objGlobal.mtdGetModuleCode(objGlobal.EnumModule.AccountPayable)) = False Then
            ddlAPAccMonth.Enabled = False
            txtAPAccYear.Enabled = False
        End If

        If objAdminShr.mtdModuleCheck(intModuleActivate, objGlobal.mtdGetModuleCode(objGlobal.EnumModule.Payroll)) = False Then
            ddlPRAccMonth.Enabled = False
            txtPRAccYear.Enabled = False
        End If

        If objAdminShr.mtdModuleCheck(intModuleActivate, objGlobal.mtdGetModuleCode(objGlobal.EnumModule.Production)) = False And _
            objAdminShr.mtdModuleCheck(intModuleActivate, objGlobal.mtdGetModuleCode(objGlobal.EnumModule.MillWeighing)) = False And _
            objAdminShr.mtdModuleCheck(intModuleActivate, objGlobal.mtdGetModuleCode(objGlobal.EnumModule.MillProduction)) = False Then
            ddlPDAccMonth.Enabled = False
            txtPDAccYear.Enabled = False
        End If

        If objAdminShr.mtdModuleCheck(intModuleActivate, objGlobal.mtdGetModuleCode(objGlobal.EnumModule.MillContract)) = False And _
            objAdminShr.mtdModuleCheck(intModuleActivate, objGlobal.mtdGetModuleCode(objGlobal.EnumModule.Billing)) = False Then
            ddlARAccMonth.Enabled = False
            txtARAccYear.Enabled = False
        End If

        If objAdminShr.mtdModuleCheck(intModuleActivate, objGlobal.mtdGetModuleCode(objGlobal.EnumModule.GeneralLedger)) = False Then
            ddlGLAccMonth.Enabled = False
            txtGLAccYear.Enabled = False
        End If

        If objAdminShr.mtdModuleCheck(intModuleActivate, objGlobal.mtdGetModuleCode(objGlobal.EnumModule.FixAsset)) = False Then
            ddlFAAccMonth.Enabled = False
            txtFAAccYear.Enabled = False
        End If
	
	If objAdminShr.mtdModuleCheck(intModuleActivate, objGlobal.mtdGetModuleCode(objGlobal.EnumModule.CashAndBank)) = False Then
            ddlCBAccMonth.Enabled = False
            txtCBAccYear.Enabled = False
        End If
    End Sub

    Sub onLoad_Display()
        Dim strOpCd As String = "ADMIN_CLSACCPERIOD_GET"
        Dim objAccPeriodDs As New Object()
        Dim intErrNo As Integer
        Dim strParam As String = ""

        If (Session("SS_INACCMONTH") = "") Then
            strParam = strCompany & "|" & strLocation
            Try
                intErrNo = objAdmin.mtdGetAccPeriod(strOpCd, _
                                                    strParam, _
                                                    objAccPeriodDs)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADMIN_GET_ACCPERIOD&errmesg=" & Exp.ToString & "&redirect=")
            End Try

            If (objAccPeriodDs.Tables(0).Rows.Count > 0) Then

                ShowAccountingPeriod(ddlINAccMonth, txtINAccYear, objAccPeriodDs.Tables(0).Rows(0).Item("INAccMonth"), objAccPeriodDs.Tables(0).Rows(0).Item("INAccYear"))
                ShowAccountingPeriod(ddlNUAccMonth, txtNUAccYear, objAccPeriodDs.Tables(0).Rows(0).Item("NUAccMonth"), objAccPeriodDs.Tables(0).Rows(0).Item("NUAccYear"))
                ShowAccountingPeriod(ddlPUAccMonth, txtPUAccYear, objAccPeriodDs.Tables(0).Rows(0).Item("PUAccMonth"), objAccPeriodDs.Tables(0).Rows(0).Item("PUAccYear"))
                ShowAccountingPeriod(ddlAPAccMonth, txtAPAccYear, objAccPeriodDs.Tables(0).Rows(0).Item("APAccMonth"), objAccPeriodDs.Tables(0).Rows(0).Item("APAccYear"))
                ShowAccountingPeriod(ddlPRAccMonth, txtPRAccYear, objAccPeriodDs.Tables(0).Rows(0).Item("PRAccMonth"), objAccPeriodDs.Tables(0).Rows(0).Item("PRAccYear"))
                If strLocType = objAdminLoc.EnumLocType.Mill Then
                    ShowAccountingPeriod(ddlPDAccMonth, txtPDAccYear, objAccPeriodDs.Tables(0).Rows(0).Item("PMAccMonth"), objAccPeriodDs.Tables(0).Rows(0).Item("PMAccYear"))
                Else
                    ShowAccountingPeriod(ddlPDAccMonth, txtPDAccYear, objAccPeriodDs.Tables(0).Rows(0).Item("PDAccMonth"), objAccPeriodDs.Tables(0).Rows(0).Item("PDAccYear"))
                End If
                ShowAccountingPeriod(ddlFAAccMonth, txtFAAccYear, objAccPeriodDs.Tables(0).Rows(0).Item("FAAccMonth"), objAccPeriodDs.Tables(0).Rows(0).Item("FAAccYear"))
                ShowAccountingPeriod(ddlARAccMonth, txtARAccYear, objAccPeriodDs.Tables(0).Rows(0).Item("ARAccMonth"), objAccPeriodDs.Tables(0).Rows(0).Item("ARAccYear"))
                ShowAccountingPeriod(ddlGLAccMonth, txtGLAccYear, objAccPeriodDs.Tables(0).Rows(0).Item("GLAccMonth"), objAccPeriodDs.Tables(0).Rows(0).Item("GLAccYear"))
                ShowAccountingPeriod(ddlCBAccMonth, txtCBAccYear, objAccPeriodDs.Tables(0).Rows(0).Item("CBAccMonth"), objAccPeriodDs.Tables(0).Rows(0).Item("CBAccYear"))
                If IsDBNull(objAccPeriodDs.Tables(0).Rows(0).Item("PhyMonth")) Then
                    ddlPhyMonth.SelectedIndex = CInt(Month(Now())) - 1
                Else
                    ddlPhyMonth.SelectedIndex = objAccPeriodDs.Tables(0).Rows(0).Item("PhyMonth") - 1
                End If
            
                If IsDBNull(objAccPeriodDs.Tables(0).Rows(0).Item("PhyYear")) Then
                    txtPhyYear.Text = Year(Now())
                Else
                    txtPhyYear.Text = objAccPeriodDs.Tables(0).Rows(0).Item("PhyYear")
                End If
            End If
        Else


	        'ShowAccountingPeriod(ddlCBAccMonth, txtCBAccYear, Session("SS_CBACCMONTH"), Session("SS_CBACCYEAR"))

            strParam = strCompany & "|" & strLocation
            Try
                intErrNo = objAdmin.mtdGetAccPeriod(strOpCd, _
                                                    strParam, _
                                                    objAccPeriodDs)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADMIN_GET_ACCPERIOD&errmesg=" & Exp.ToString & "&redirect=")
            End Try

            ShowAccountingPeriod(ddlINAccMonth, txtINAccYear, objAccPeriodDs.Tables(0).Rows(0).Item("INAccMonth"), objAccPeriodDs.Tables(0).Rows(0).Item("INAccYear"))
            ShowAccountingPeriod(ddlNUAccMonth, txtNUAccYear, objAccPeriodDs.Tables(0).Rows(0).Item("NUAccMonth"), objAccPeriodDs.Tables(0).Rows(0).Item("NUAccYear"))
            ShowAccountingPeriod(ddlPUAccMonth, txtPUAccYear, objAccPeriodDs.Tables(0).Rows(0).Item("PUAccMonth"), objAccPeriodDs.Tables(0).Rows(0).Item("PUAccYear"))
            ShowAccountingPeriod(ddlAPAccMonth, txtAPAccYear, objAccPeriodDs.Tables(0).Rows(0).Item("APAccMonth"), objAccPeriodDs.Tables(0).Rows(0).Item("APAccYear"))
            ShowAccountingPeriod(ddlPRAccMonth, txtPRAccYear, objAccPeriodDs.Tables(0).Rows(0).Item("PRAccMonth"), objAccPeriodDs.Tables(0).Rows(0).Item("PRAccYear"))
            If strLocType = objAdminLoc.EnumLocType.Mill Then
                ShowAccountingPeriod(ddlPDAccMonth, txtPDAccYear, objAccPeriodDs.Tables(0).Rows(0).Item("PMAccMonth"), objAccPeriodDs.Tables(0).Rows(0).Item("PMAccYear"))
            Else
                ShowAccountingPeriod(ddlPDAccMonth, txtPDAccYear, objAccPeriodDs.Tables(0).Rows(0).Item("PDAccMonth"), objAccPeriodDs.Tables(0).Rows(0).Item("PDAccYear"))
            End If
            ShowAccountingPeriod(ddlFAAccMonth, txtFAAccYear, objAccPeriodDs.Tables(0).Rows(0).Item("FAAccMonth"), objAccPeriodDs.Tables(0).Rows(0).Item("FAAccYear"))
            ShowAccountingPeriod(ddlARAccMonth, txtARAccYear, objAccPeriodDs.Tables(0).Rows(0).Item("ARAccMonth"), objAccPeriodDs.Tables(0).Rows(0).Item("ARAccYear"))
            ShowAccountingPeriod(ddlGLAccMonth, txtGLAccYear, objAccPeriodDs.Tables(0).Rows(0).Item("GLAccMonth"), objAccPeriodDs.Tables(0).Rows(0).Item("GLAccYear"))
            ShowAccountingPeriod(ddlCBAccMonth, txtCBAccYear, objAccPeriodDs.Tables(0).Rows(0).Item("CBAccMonth"), objAccPeriodDs.Tables(0).Rows(0).Item("CBAccYear"))
            ddlPhyMonth.SelectedIndex = CInt(Session("SS_PHYMONTH")) - 1
            txtPhyYear.Text = Session("SS_PHYYEAR")
        End If
        objAccPeriodDs = Nothing


    End Sub

    Sub SaveBtn_Click(Sender As Object, E As ImageclickEventArgs)
        Dim strOpCd_Upd As String = "ADMIN_CLSACCPERIOD_UPD"
        Dim strOpCd_GET As String = "ADMIN_CLSACCPERIOD_CONFIG_GET"
        Dim intErrNo As Integer
        Dim strParam As String = ""
        Dim blnPeriodNotFound As Boolean

        strParam = strCompany & "|" & _
                   strLocation & "|" & _
                   fnGetValueFromDropDownList(ddlINAccMonth) & "|" & _
                   txtINAccYear.Text & "|" & _
                   fnGetValueFromDropDownList(ddlNUAccMonth) & "|" & _
                   txtNUAccYear.Text & "|" & _
                   fnGetValueFromDropDownList(ddlPUAccMonth) & "|" & _
                   txtPUAccYear.Text & "|" & _
                   fnGetValueFromDropDownList(ddlAPAccMonth) & "|" & _
                   txtAPAccYear.Text & "|" & _
                   fnGetValueFromDropDownList(ddlPRAccMonth) & "|" & _
                   txtPRAccYear.Text & "|" & _
                   fnGetValueFromDropDownList(ddlPDAccMonth) & "|" & _
                   txtPDAccYear.Text & "|" & _
                   fnGetValueFromDropDownList(ddlPDAccMonth) & "|" & _
                   txtPDAccYear.Text & "|" & _
                   fnGetValueFromDropDownList(ddlARAccMonth) & "|" & _
                   txtARAccYear.Text & "|" & _
                   fnGetValueFromDropDownList(ddlGLAccMonth) & "|" & _
                   txtGLAccYear.Text & "|" & _
                   ddlPhyMonth.SelectedItem.Value.Trim() & "|" & _
                   txtPhyYear.Text & "|" & _
                   fnGetValueFromDropDownList(ddlFAAccMonth) & "|" & _
                   txtFAAccYear.Text & "|" & _
                   fnGetValueFromDropDownList(ddlCBAccMonth) & "|" & _
                   txtCBAccYear.Text
        Try
            intErrNo = objAdmin.mtdUpdAccPeriod(strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strOpCd_Upd, _
                                                strParam, _
                                                blnPeriodNotFound)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADMIN_UPD_ACCPERIOD&errmesg=" & Exp.ToString & "&redirect=")
        End Try

        If blnPeriodNotFound = True Then

            Session("SS_ACCMONTH") = fnGetValueFromDropDownList(ddlINAccMonth)
            Session("SS_ACCYEAR") = txtINAccYear.Text

            Session("SS_INACCMONTH") = fnGetValueFromDropDownList(ddlINAccMonth)
            Session("SS_INACCYEAR") = txtINAccYear.Text
            Session("SS_NUACCMONTH") = fnGetValueFromDropDownList(ddlNUAccMonth)
            Session("SS_NUACCYEAR") = txtNUAccYear.Text
            Session("SS_PUACCMONTH") = fnGetValueFromDropDownList(ddlPUAccMonth)
            Session("SS_PUACCYEAR") = txtPUAccYear.Text
            Session("SS_APACCMONTH") = fnGetValueFromDropDownList(ddlAPAccMonth)
            Session("SS_APACCYEAR") = txtAPAccYear.Text
            Session("SS_PRACCMONTH") = fnGetValueFromDropDownList(ddlPRAccMonth)
            Session("SS_PRACCYEAR") = txtPRAccYear.Text
            Session("SS_PDACCMONTH") = fnGetValueFromDropDownList(ddlPDAccMonth)
            Session("SS_PDACCYEAR") = txtPDAccYear.Text
            Session("SS_PMACCMONTH") = fnGetValueFromDropDownList(ddlPDAccMonth)
            Session("SS_PMACCYEAR") = txtPDAccYear.Text
            Session("SS_ARACCMONTH") = fnGetValueFromDropDownList(ddlARAccMonth)
            Session("SS_ARACCYEAR") = txtARAccYear.Text
            Session("SS_GLACCMONTH") = fnGetValueFromDropDownList(ddlGLAccMonth)
            Session("SS_GLACCYEAR") = txtGLAccYear.Text
            Session("SS_PHYMONTH") = ddlPhyMonth.SelectedItem.Value.Trim()
            Session("SS_PHYYEAR") = txtPhyYear.Text
            Session("SS_FAACCMONTH") = fnGetValueFromDropDownList(ddlFAAccMonth)
            Session("SS_FAACCYEAR") = txtFAAccYear.Text
            Session("SS_CBACCMONTH") = fnGetValueFromDropDownList(ddlCBAccMonth)
            Session("SS_CBACCYEAR") = txtCBAccYear.Text

            Response.Write("<Script language=""Javascript"">parent.left.location.href = '/" & strLangCode & "/appmenu.aspx'</Script>")
            onLoad_Display()
        Else
            lblErrAccPeriod.Visible = True
            SynchronizeViewStateData()
        End If
    End Sub

End Class
