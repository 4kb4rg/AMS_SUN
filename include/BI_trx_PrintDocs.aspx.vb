
Imports System
Imports System.Data
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic


Public Class BI_trx_PrintDocs : Inherits Page

    Protected WithEvents txtFromId As TextBox
    Protected WithEvents txtToId As TextBox
    Protected WithEvents ddlBillType As DropDownList
    Protected WithEvents ibConfirm As ImageButton
    Protected WithEvents lblDocName As Label
    Protected WithEvents lblDN As Label
    Protected WithEvents lblCN As Label
    Protected WithEvents lblErrCode As Label
    Protected WithEvents lblErrMessage As Label

    Dim objBITrx As New agri.BI.clsTrx()
    Dim objAdmin As New agri.Admin.clsShare()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()

    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objLangCapDs As New Dataset()
    Dim objRptDs As New DataSet()

    Const DN = 1    
    Const CN = 2    

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim intConfig As integer
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim intDocType As Integer

    Dim strAccTag As String
    Dim strBlkTag As String
    Dim strVehTag As String
    Dim strVehExpCodeTag As String
    Dim strLangCode As String
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim strLocType as String
    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        intConfig = Session("SS_CONFIGSETTING")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_ARACCMONTH")
        strAccYear = Session("SS_ARACCYEAR")
        strLocType = Session("SS_LOCTYPE")



        If Request.QueryString("doctype") = "" Then
            intDocType = 0
        Else
            intDocType = Request.QueryString("doctype")
        End If

        lblErrCode.Visible = False

        If Page.IsPostBack Then
            Select Case CInt(intDocType)
                Case DN
                    onsubmit_printDN()
            End Select
        Else
            Select Case CInt(intDocType)
                Case DN
                    lblDocName.Text = lblDN.Text
                    BindBillType()
            End Select
        End If
    End Sub

    
    Sub BindBillType()
        ddlBillType.Items.Add(New ListItem(objBITrx.mtdGetDebitNoteDocType(objBITrx.EnumDebitNoteDocType.Manual), objBITrx.EnumDebitNoteDocType.Manual))
        ddlBillType.Items.Add(New ListItem(objBITrx.mtdGetDebitNoteDocType(objBITrx.EnumDebitNoteDocType.Auto_INIssue_Staff), objBITrx.EnumDebitNoteDocType.Auto_INIssue_Staff))
        ddlBillType.Items.Add(New ListItem(objBITrx.mtdGetDebitNoteDocType(objBITrx.EnumDebitNoteDocType.Auto_INIssue_External), objBITrx.EnumDebitNoteDocType.Auto_INIssue_External))
        ddlBillType.Items.Add(New ListItem(objBITrx.mtdGetDebitNoteDocType(objBITrx.EnumDebitNoteDocType.Auto_INTransfer), objBITrx.EnumDebitNoteDocType.Auto_INTransfer))
        ddlBillType.Items.Add(New ListItem(objBITrx.mtdGetDebitNoteDocType(objBITrx.EnumDebitNoteDocType.Auto_CTIssue_Staff), objBITrx.EnumDebitNoteDocType.Auto_CTIssue_Staff))
        ddlBillType.Items.Add(New ListItem(objBITrx.mtdGetDebitNoteDocType(objBITrx.EnumDebitNoteDocType.Auto_CTIssue_External), objBITrx.EnumDebitNoteDocType.Auto_CTIssue_External))
        ddlBillType.Items.Add(New ListItem(objBITrx.mtdGetDebitNoteDocType(objBITrx.EnumDebitNoteDocType.Auto_CTTransfer), objBITrx.EnumDebitNoteDocType.Auto_CTTransfer))
        ddlBillType.Items.Add(New ListItem(objBITrx.mtdGetDebitNoteDocType(objBITrx.EnumDebitNoteDocType.Auto_INFuelIssue_Staff), objBITrx.EnumDebitNoteDocType.Auto_INFuelIssue_Staff))
        ddlBillType.Items.Add(New ListItem(objBITrx.mtdGetDebitNoteDocType(objBITrx.EnumDebitNoteDocType.Auto_INFuelIssue_External), objBITrx.EnumDebitNoteDocType.Auto_INFuelIssue_External))
        ddlBillType.Items.Add(New ListItem(objBITrx.mtdGetDebitNoteDocType(objBITrx.EnumDebitNoteDocType.Auto_WSJob_Staff), objBITrx.EnumDebitNoteDocType.Auto_WSJob_Staff))
        ddlBillType.Items.Add(New ListItem(objBITrx.mtdGetDebitNoteDocType(objBITrx.EnumDebitNoteDocType.Auto_WSJob_External), objBITrx.EnumDebitNoteDocType.Auto_WSJob_External))
    End Sub


    Sub onsubmit_printDN()
        Dim strOpCd_Get As String = "BI_CLSTRX_DEBITNOTE_GETRANGE" & "|" & "DebitNote"
        Dim strOpCodePrint As String = "ADMIN_SHARE_UPD_PRINTDATE"
        Dim strTable As String = "BI_DEBITNOTE"
        Dim strUpdString As String = ""
        Dim strParam As String
        Dim SearchStr As String = ""
        Dim SortStr As String = ""
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intStatus As Integer
        Dim strPrintDate As String
        Dim strDebitNoteId As String
        Dim strIDRange As String = ""
        Dim strReprintedID As String = ""
        Dim strSortLine As String

        Dim strFromID As String
        Dim strToID As String
        Dim strBillType As String
        Dim strStatus As String

        strFromID = txtFromId.Text.Trim
        strToID = txtToId.Text.Trim
        strBillType = Trim(ddlBillType.SelectedItem.value)
        strStatus = CStr(objBITrx.EnumDebitNoteStatus.Confirmed)
        
        SearchStr = "and DebitNoteID >= '" & strFromID & "' " & _
                    "and DebitNoteID <= '" & strToID & "' " & _
                    "and DocType = '" & strBillType & "' " & _
                    "and Status in ('" & strStatus & "') "

        SortStr = "order by DebitNoteID"

        strParam = "|" & SearchStr & "|" & SortStr

        Try
            intErrNo = objBITrx.mtdGetDebitNoteReport(strOpCd_Get, _
                                                      strCompany, _
                                                      strLocation, _
                                                      strUserId, _
                                                      strAccMonth, _
                                                      strAccYear, _
                                                      strParam, _
                                                      objRptDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_PRINTDOCS_GETRANGE&errmesg=" & lblErrMessage.Text & "&redirect=")        
        End Try

        If objRptDs.Tables(0).Rows.Count > 0 Then
            For intCnt=0 To objRptDs.Tables(0).Rows.Count - 1

                intStatus = CInt(Trim(objRptDs.Tables(0).Rows(intCnt).Item("Status")))
                strPrintDate = objGlobal.GetLongDate(objRptDs.Tables(0).Rows(intCnt).Item("PrintDate"))
                strDebitNoteId = Trim(objRptDs.Tables(0).Rows(intCnt).Item("DebitNoteId"))
                strIDRange = strIDRange & strDebitNoteId & "','"

                strUpdString = "where DebitNoteID = '" & strDebitNoteId & "' " & _
                               "and LocCode = '" & strLocation & "' " & _
                               "and AccMonth = '" & strAccMonth & "' " & _
                               "and AccYear = '" & strAccYear & "' "

                If intStatus = objBITrx.EnumDebitNoteStatus.Confirmed Then
                    If strPrintDate = "" Then
                        Try
                            intErrNo = objAdmin.mtdUpdPrintDate(strOpCodePrint, _
                                                                strUpdString, _
                                                                strTable, _
                                                                strCompany, _
                                                                strLocation, _
                                                                strUserId)
                        Catch Exp As System.Exception
                            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_PRINTDOCS_UPDPRINTDATE&errmesg=" & lblErrMessage.Text & "&redirect=")
                        End Try
                    Else
                        strReprintedID = strReprintedID & strDebitNoteId & "|"
                    End If
                End If

            Next
            strIDRange = Left(strIDRange, Len(strIDRange)-3)
        Else
            lblErrCode.visible = True
            Exit Sub
        End If

        onload_GetLangCap()



        Response.Write("<Script Language=""JavaScript"">window.open(""../reports/BI_Rpt_DNDet.aspx?strDebitNoteId=" & strIDRange & _
                       "&strStatus=" & strStatus & _
                       "&strDocType=" & strBillType & _
                       "&AccountTag=" & strAccTag & _
                       "&BlockTag=" & strBlkTag & _
                       "&VehicleTag=" & strVehTag & _
                       "&VehExpenseTag=" & strVehExpCodeTag & _
                       "&batchPrint=yes" & _
                       "&reprintId=" & strReprintedID & _
                       """, null,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");window.close();</Script>")

    End Sub

    Sub close_window()
        Response.Write("<Script Language=""JavaScript"">window.close();</Script>")
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        Try
            If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfig) = True Then
               strBlkTag = GetCaption(objLangCap.EnumLangCap.Block)
            Else
               strBlkTag = GetCaption(objLangCap.EnumLangCap.SubBlock)
            End If
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_PRINTDOCS_GETLANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try
        strAccTag = GetCaption(objLangCap.EnumLangCap.Account)
        strVehTag = GetCaption(objLangCap.EnumLangCap.Vehicle)
        strVehExpCodeTag = GetCaption(objLangCap.EnumLangCap.VehExpense)
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_TRX_PRINTDOCS_GETENTIRELANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

    End Sub

    Function GetCaption(ByVal pv_TermCode) As String
            Dim count As Integer

            For count = 0 To objLangCapDs.Tables(0).Rows.Count - 1
                If Trim(pv_TermCode) = Trim(objLangCapDs.Tables(0).Rows(count).Item("TermCode")) Then
                    If strLocType = objAdminLoc.EnumLocType.Mill then
                        Return Trim(objLangCapDs.Tables(0).Rows(count).Item("BusinessTermMW"))
                    else
                        Return Trim(objLangCapDs.Tables(0).Rows(count).Item("BusinessTerm"))
                    end if
                    Exit For
                End If
            Next
        End Function

End Class
