
Imports System
Imports System.Data
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports Microsoft.VisualBasic.Strings


Public Class CT_trx_PrintDocs : Inherits Page

    Protected WithEvents txtFromId As TextBox
    Protected WithEvents txtToId As TextBox
    Protected WithEvents ddlIssueType As Dropdownlist
    Protected WithEvents cblDisplayCost As CheckBoxList
    Protected WithEvents ibConfirm As ImageButton
    Protected WithEvents lblDocName As Label
    Protected WithEvents lblPR As Label
    Protected WithEvents lblSRA As Label
    Protected WithEvents lblST As Label
    Protected WithEvents lblSI As Label
    Protected WithEvents lblFI As Label
    Protected WithEvents lblErrCode As Label
    Protected WithEvents lblErrMessage As Label

    Dim objCTTrx As New agri.CT.clsTrx()
    Dim objAdmin As New agri.Admin.clsShare()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()

    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objLangCapDs As New Dataset()
    Dim objRptDs As New DataSet()

    Const PR = 1    
    Const SRA = 2   
    Const ST = 3    
    Const SI = 4    
    Const FI = 5    

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
        strAccMonth = Session("SS_INACCMONTH")
        strAccYear = Session("SS_INACCYEAR")

        strLocType = Session("SS_LOCTYPE")


        If Request.QueryString("doctype") = "" Then
            intDocType = 0
        Else
            intDocType = Request.QueryString("doctype")
        End If

        lblErrCode.Visible = False

        If Page.IsPostBack Then



            Select Case CInt(intDocType)
                Case PR
                Case SRA
                Case ST
                Case SI
                    onsubmit_printSI()
                Case FI
            End Select
        Else
            Select Case CInt(intDocType)
                Case PR
                    lblDocName.Text = lblPR.Text
                Case SRA
                    lblDocName.Text = lblSRA.Text
                Case ST
                    lblDocName.Text = lblST.Text
                Case SI
                    lblDocName.Text = lblSI.Text
                    BindIssueType()
                Case FI
                    lblDocName.Text = lblFI.Text
                Case Else
                    lblDocName.Text = "Unknown document"
            End Select
        End If
    End Sub


    Sub BindIssueType()
        ddlIssueType.Items.Add(New ListItem(objCTTrx.mtdGetStockIssueType(objCTTrx.EnumStockIssueType.OwnUse), objCTTrx.EnumStockIssueType.OwnUse))
        ddlIssueType.Items.Add(New ListItem(objCTTrx.mtdGetStockIssueType(objCTTrx.EnumStockIssueType.StaffPayroll), objCTTrx.EnumStockIssueType.StaffPayroll))
        ddlIssueType.Items.Add(New ListItem(objCTTrx.mtdGetStockIssueType(objCTTrx.EnumStockIssueType.StaffDN), objCTTrx.EnumStockIssueType.StaffDN))
        ddlIssueType.Items.Add(New ListItem(objCTTrx.mtdGetStockIssueType(objCTTrx.EnumStockIssueType.External), objCTTrx.EnumStockIssueType.External))
    End Sub

    Sub onsubmit_printSI()
        Dim strOpCd_Get As String = "CT_CLSTRX_STOCKISSUE_LIST_GET" & "|" & "StockIssue"
        Dim strOpCodePrint As String = "ADMIN_SHARE_UPD_PRINTDATE"
        Dim strTable As String = "CT_STOCKISSUE"
        Dim strUpdString As String = ""
        Dim strParam As String
        Dim SearchStr As String = ""
        Dim SortStr As String = ""
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intStatus As Integer
        Dim strPrintDate As String
        Dim strStockIssueID As String
        Dim strIDRange As String = ""
        Dim strReprintedID As String = ""
        Dim strSortLine As String

        Dim strFromID As String
        Dim strToID As String
        Dim strIssueType As String
        Dim strStatus As String
        Dim strDisplayCost As String = ""

        strFromID = txtFromId.Text.Trim
        strToID = txtToId.Text.Trim
        strIssueType = Trim(ddlIssueType.SelectedItem.Value)
        strStatus = CStr(objCTTrx.EnumStockIssueStatus.Confirmed)
        If cblDisplayCost.Items(0).Selected Then
            strDisplayCost = "1"
        End If
        
        SearchStr = "and iss.Status in ('" & strStatus & "') " & _
                    "and iss.LocCode = '" & strLocation & "' " & _
                    "and iss.IssueType in ('" & strIssueType & "') " & _
                    "and iss.StockIssueID >= '" & strFromID & "' "
        If strToID <> "" Then
            SearchStr += "and iss.StockIssueID <= '" & strToID & "' "
        End If

        SortStr = "order by iss.StockIssueID"

        strParam = "||" & SearchStr & "|" & SortStr

        Try
            intErrNo = objCTTrx.mtdGetStockIssueReport(strOpCd_Get, _
                                                       strParam, _
                                                       strCompany, _
                                                       strLocation, _
                                                       strUserId, _
                                                       strAccMonth, _
                                                       strAccYear, _
                                                       objRptDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CT_TRX_PRINTDOCS_GETRANGE&errmesg=" & lblErrMessage.Text & "&redirect=")        
        End Try

        If objRptDs.Tables(0).Rows.Count > 0 Then
            For intCnt=0 To objRptDs.Tables(0).Rows.Count - 1

                intStatus = CInt(Trim(objRptDs.Tables(0).Rows(intCnt).Item("Status")))
                strPrintDate = objGlobal.GetLongDate(objRptDs.Tables(0).Rows(intCnt).Item("PrintDate"))
                strStockIssueId = Trim(objRptDs.Tables(0).Rows(intCnt).Item("StockIssueID"))
                strIDRange = strIDRange & strStockIssueId & "','"

                strUpdString = "where StockIssueID = '" & strStockIssueId & "'"
                If intStatus = objCTTrx.EnumStockIssueStatus.Confirmed Or intStatus = objCTTrx.EnumStockIssueStatus.DbNote Then
                    If strPrintDate = "" Then
                        Try
                            intErrNo = objAdmin.mtdUpdPrintDate(strOpCodePrint, _
                                                                strUpdString, _
                                                                strTable, _
                                                                strCompany, _
                                                                strLocation, _
                                                                strUserId)
                        Catch Exp As System.Exception
                            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CT_TRX_PRINTDOCS_UPDPRINTDATE&errmesg=" & lblErrMessage.Text & "&redirect=")
                        End Try
                    Else
                        strReprintedID = strReprintedID & strStockIssueId & "|"
                    End If
                End If
            Next
            strIDRange = Left(strIDRange, Len(strIDRange)-3)    
        Else
            lblErrCode.visible = True
            Exit Sub
        End If

        onload_GetLangCap()

        Response.Write("<Script Language=""JavaScript"">window.open(""../reports/CT_Rpt_StockIssueDet.aspx?strStockIssueId=" & strIDRange & _
                       "&strIssueType=" & strIssueType & _
                       "&strDisplayCost=" & strDisplayCost & _
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=IN_TRX_PRINTDOCS_GETLANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=")
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=IN_TRX_PRINTDOCS_GETENTIRELANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

    End Sub

    Function GetCaption(ByVal pv_TermCode) As String
            Dim count As Integer
            
            For count=0 To objLangCapDs.Tables(0).Rows.Count - 1
                If Trim(pv_TermCode) = Trim(objLangCapDs.Tables(0).Rows(count).Item("TermCode"))
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
