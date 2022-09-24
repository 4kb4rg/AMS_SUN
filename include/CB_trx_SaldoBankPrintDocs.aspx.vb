
Imports System
Imports System.Data
Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Strings
Imports System.Globalization
Imports System.Math 


Public Class CB_trx_SaldoBankPrintDocs : Inherits Page

    Dim objGLTrx As New agri.GL.clsTrx()
    Dim objGLSetup As New agri.GL.clsSetup()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objAdminLoc As New agri.Admin.clsLoc()


    Protected WithEvents lblErrMessage As Label
    Protected WithEvents txtTgl As TextBox
    Protected WithEvents txtBank As TextBox
    Protected WithEvents cbPT As DropDownList
    Protected WithEvents rbLok As RadioButton
    Protected WithEvents rbBank As RadioButton

   
    Protected WithEvents ibConfirm As ImageButton
   
    Protected WithEvents hidTrxID As HtmlInputHidden

    Dim objAccDs As New Dataset()
    Dim objBlkDs As New Dataset()
    Dim objVehDs As New Dataset()

    Dim strParam As String
    Dim objDataSet As New DataSet()
    Dim objLangCapDs As New Object()
    Dim intErrNo As Integer

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intGLAR As Integer

    Dim intConfigSetting As Integer

    Dim strBdgYear As String
    Dim strBdgAcc As String
    Dim strLocType As String
    Dim BlockTag As String

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)

        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_PUACCMONTH")
        strAccYear = Session("SS_PUACCYEAR")
        strLocType = Session("SS_LOCTYPE")


        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        End If

        If Not IsPostBack Then
            txtTgl.Text = Request.QueryString("Tgl")
            cbPT.SelectedIndex = Request.QueryString("CompName")
            txtBank.Text = Request.QueryString("NmBank")
            'txtTgl.Enabled = False
            'cbPT.Enabled = False
            'txtBank.Enabled = False
        End If
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()

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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=FA_CLSTRX_AssetAddDETAILS_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=FA/trx/FA_trx_AssetAddDetails.aspx")
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

    Sub btnPreview_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim strGroup As String


        If cbPT.SelectedIndex = 0 Then
            strCompany = ""
        Else
            strCompany = cbPT.SelectedItem.Value
        End If
        If rbBank.Checked Then
            strGroup = "0"
        Else
            strGroup = "1"
        End If

        Response.Write("<Script Language=""JavaScript"">window.open(""../reports/CB_Rpt_SaldoBank_listPrev.aspx?doctype=1&strPT=" & strCompany & _
                       "&Location=" & strLocation & _
                       "&group=" & strGroup & _
                       "&tgl=" & txtTgl.Text & _
                       "&NmBank=" & txtBank.Text & _
                       """,null ,""status=yes, height=400, width=600, top=180, left=220, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")

    End Sub

  

    

End Class
