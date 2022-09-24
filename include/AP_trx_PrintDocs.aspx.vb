

Imports System
Imports System.Data
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic


Public Class AP_trx_PrintDocs : Inherits Page

    Protected WithEvents ddlSuppCode As DropDownList
    Protected WithEvents txtTrxID As TextBox
    Protected WithEvents lblErrTrxID As Label
    Protected WithEvents PrintBtn As ImageButton
    Protected WithEvents lblErrMessage As Label

    Protected objCBTrx As New agri.CB.clsTrx()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objPUTrx As New agri.PU.clsTrx()
    Dim objAdmin As New agri.Admin.clsShare()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim objHRSetup As New agri.HR.clsSetup()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objLangCapDs As New Dataset()
    Dim objRptDs As New DataSet()
    Dim objPRRpt As New agri.PR.clsReport()
    Dim objGLtrx As New agri.GL.ClsTrx()
    Dim objCMSetup As New agri.CM.clsSetup()
    Dim objPUSetup As New agri.PU.clsSetup()
   
    Dim strBankCode As String
    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim intConfig As Integer
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim intTrxID As String
    Dim strDocType As String
    Dim strTrxID As String
    Dim strParam As String = ""
    Dim strExchangeRate As String
    Dim intCBAR As Integer
    Dim strAccTag As String
    Dim strBlkTag As String
    Dim strVehTag As String
    Dim strVehExpCodeTag As String
    Dim strLangCode As String
    Dim strLocType As String

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        intConfig = Session("SS_CONFIGSETTING")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_CBACCMONTH")
        strAccYear = Session("SS_CBACCYEAR")
        strLocType = Session("SS_LOCTYPE")
        intCBAR = Session("SS_CBAR")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCBAccessRights.CBCashBank), intCBAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            If Request.QueryString("TrxID") = "" Then
                intTrxID = ""
            Else
                intTrxID = Request.QueryString("TrxID")
                txtTrxID.Text = intTrxID
            End If

            lblErrTrxID.Visible = False

            If Not Page.IsPostBack Then
                BindSupp("")
            End If
        End If

    End Sub

    Sub PreviewBtn_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim objAPNote As Object
        Dim strOpCd_APNote As String
        Dim strAPNoteID As String
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer
        Dim strSuppCode As String = Trim(ddlSuppCode.SelectedItem.Value)

        strTrxID = Trim(txtTrxID.Text)
       
        If strTrxID = "" And strSuppCode = "" Then
            lblErrTrxID.Visible = True
            Exit Sub
        End If

        Response.Write("<Script Language=""JavaScript"">window.open(""../reports/AP_Rpt_InvRcvDet.aspx?Type=Print&CompName=" & strCompany & _
                       "&Location=" & strLocation & _
                       "&TrxID=" & strTrxID & _
                       "&TrxType=" & "2" & _
                       "&SupplierCode=" & strSuppCode & _
                       """,null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");window.close();</Script>")

    End Sub

    Sub BindSupp(ByVal pv_strSelectedSuppCode As String)
        Dim strOpCd As String = "PU_CLSSETUP_SUPPLIER_DDL_GET"
        Dim liTemp As ListItem
        Dim objSuppDs As New Object
        Dim strParam As String = ""
        Dim intCnt As Integer = 0
        Dim intErrNo As Integer
        Dim dr As DataRow
        Dim intSelectedIndex As Integer = 0

        strParam = "||" & objPUSetup.EnumSuppStatus.Active & "||SupplierCode||"
       
        Try
            intErrNo = objPUSetup.mtdGetSupplier(strOpCd, strParam, objSuppDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PU_GET_SUPPLIER&errmesg=" & lblErrMessage.Text & "&redirect=pu/trx/pu_trx_polist.aspx")
        End Try
        dr = objSuppDs.Tables(0).NewRow()
        dr("SupplierCode") = ""
        dr("Name") = "Please Select Supplier Code"
        objSuppDs.Tables(0).Rows.InsertAt(dr, 0)
        ddlSuppCode.DataSource = objSuppDs.Tables(0)
        ddlSuppCode.DataValueField = "SupplierCode"
        ddlSuppCode.DataTextField = "Name"
        ddlSuppCode.DataBind()
        If pv_strSelectedSuppCode <> "" Then
            liTemp = ddlSuppCode.Items.FindByValue(pv_strSelectedSuppCode)
            If liTemp Is Nothing Then
                ddlSuppCode.Items.Add(New ListItem(pv_strSelectedSuppCode & " (Deleted)", pv_strSelectedSuppCode))
                intSelectedIndex = ddlSuppCode.Items.Count - 1
            Else
                intSelectedIndex = ddlSuppCode.Items.IndexOf(liTemp)
            End If
        End If

        ddlSuppCode.SelectedIndex = intSelectedIndex
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PU_TRX_PRINTDOCS_GETENTIRELANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

    End Sub

    Function GetCaption(ByVal pv_TermCode) As String
        Dim count As Integer

        For count = 0 To objLangCapDs.Tables(0).Rows.Count - 1
            If Trim(pv_TermCode) = Trim(objLangCapDs.Tables(0).Rows(count).Item("TermCode")) Then
                If strLocType = objAdminLoc.EnumLocType.Mill Then
                    Return Trim(objLangCapDs.Tables(0).Rows(count).Item("BusinessTermMW"))
                Else
                    Return Trim(objLangCapDs.Tables(0).Rows(count).Item("BusinessTerm"))
                End If
                Exit For
            End If
        Next
    End Function

    Function Date_Validation(ByVal pv_strInputDate As String, ByVal pv_blnIsShortDate As Boolean) As String
        Dim objSysCfgDs As New Object
        Dim objActualDate As New Object
        Dim strDateFormat As String
        Dim strParam As String
        Dim intErrNo As Integer
        Dim strAcceptFormat As String

        strParam = "PWSYSTEM_CLSCONFIG_CONFIG_DATEFMT_GET"

        Try
            intErrNo = objSysCfg.mtdGetConfigInfo(strParam, _
                                                  strCompany, _
                                                  strLocation, _
                                                  strUserId, _
                                                  objSysCfgDs)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_DEPOSIT_GET_CONFIG&errmesg=" & Exp.Message & "&redirect=CB/trx/cb_trx_DepositList.aspx")
        End Try

        strDateFormat = objSysCfg.mtdGetDateFormat(objSysCfgDs.Tables(0).Rows(0).Item("Datefmt").Trim())

        If pv_blnIsShortDate Then
            Date_Validation = objGlobal.GetShortDate(strDateFormat, pv_strInputDate)
        Else
            If objGlobal.mtdValidInputDate(strDateFormat, _
                                           pv_strInputDate, _
                                           strAcceptFormat, _
                                           objActualDate) = True Then
                Date_Validation = objActualDate
            Else
                Date_Validation = ""
            End If
        End If
    End Function
End Class
