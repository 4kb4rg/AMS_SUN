Imports System
Imports System.Data
Imports System.IO 
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic.Information
Imports Microsoft.VisualBasic

Imports agri.GlobalHdl
Imports agri.PWSystem.clsLangCap
Imports agri.PWSystem.clsConfig


Public Class appmenu : Inherits Page

    Dim strUserId As String
    Dim strLangCode As String
    Dim strCompany As String
    Dim strLocation As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim intLocType As Integer
    Dim intModuleActivate As Integer
    Dim strLocTag As String

    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim objLangCapVw As New DataView

    Dim intGLAR As Integer
    Dim intBIAR As Integer
    Dim intAPAR As Integer
    Dim intCBAR As Integer
    Dim intPUAR As Integer
    Dim intINAR As Integer
    Dim intHRAR As Long
    Dim intPRAR As Long
    Dim intPDAR As Integer
    Dim intPMAR As Integer
    Dim intWMAR As Integer
    Dim intCMAR As Integer
    Dim intCTAR As Integer
    Dim intWSAR As Integer
    Dim intNUAR As Integer
    Dim intFAAR As Integer

    Const C_ADMIN = "ADMIN"

    Dim blnGLS As Boolean = False
    Dim blnINS As Boolean = False
    Dim blnHRS As Boolean = False
    Dim blnPRS As Boolean = False
    Dim blnFAS As Boolean = False
    Dim blnPDS As Boolean = False
    Dim blnNUS As Boolean = False
    Dim blnWSS As Boolean = False



#Region " Web Form Designer Generated Code "

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lnkLocation As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkLogOut As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkGL01 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkGL02 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkGL03 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkGL04 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkGL05 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkGL06 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkBill01 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkBill02 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkBill03 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkBill04 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkAP01 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkAP02 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkAP03 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkAP04 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkCB01 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkCB02 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkCB03 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkCB04 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkCB05 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkCB06 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkPU01 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkPU02 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkPU03 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkPU04 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkPU05 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkPU06 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkIN01 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkIN02 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkIN03 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkIN04 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkIN05 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkIN06 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkIN07 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkIN08 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkIN09 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkHR01 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkHR02 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkHR03 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkHR04 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkHR05 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkHR06 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkHR07 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkHR08 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkPR01 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkPR02 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkPR03 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkPR04 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkPR05 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkPR06 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkPR07 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkPR08 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkPR09 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkPR10 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkPR11 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkPR12 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkProd01 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkProd02 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkProd03 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkProd04 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkProd05 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkProd06 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkProd07 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkProd08 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkProd09 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkProd10 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkProd11 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkProd12 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkWM01 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkWM02 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkCM01 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkCM02 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkCM03 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkCT01 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkCT02 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkCT03 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkCT04 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkCT05 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkCT06 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkCT07 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkWS01 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkWS02 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkNU01 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkNU02 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkNU03 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkNU04 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkNU05 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkNU06 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkNU07 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkNU08 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkFA01 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkFA02 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkFA03 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkFA04 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpFI01 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpFI02 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpFI03 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpFI04 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpFI05 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpFI06 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpFI07 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpFI08 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpFI09 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpFI10 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpFI11 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpFI12 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpFI13 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpFI14 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpFI15 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpFI16 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpFI17 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpFI18 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpFI19 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpFI20 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpFI21 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpFI22 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpFI23 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpIN01 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpIN02 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpIN03 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpIN04 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpIN05 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpIN06 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpIN07 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpIN08 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpIN09 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpIN10 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpHR01 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpHR02 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpHR03 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpHR04 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpHR05 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpHR06 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpHR07 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpHR08 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpHR09 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpHR10 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpHR11 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpHR12 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpHR13 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpHR14 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpHR15 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpHR16 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpHR17 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpHR18 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpHR19 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpHR20 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpHR21 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpHR22 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpHR23 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpHR24 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpHR25 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpHR26 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpHR27 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPR01 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPR02 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPR03 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPR04 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPR05 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPR06 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPR07 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPR08 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPR09 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPR10 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPR11 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPR12 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPR13 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPR14 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPR15 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPR16 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPR17 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPR18 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPR19 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpFA01 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpFA02 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpFA03 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpFA04 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpFA05 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpFA06 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpFA07 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPD01 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPD02 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPD03 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPD04 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPD05 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPD06 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPD07 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPD08 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPD09 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPD10 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPD11 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPD12 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPD13 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpPD14 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpNU01 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpNU02 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpNU03 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpNU04 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpNU05 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpWS01 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpWS02 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpWS03 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpWS04 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpWS05 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpWS06 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpWS07 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpWS08 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpWS09 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpWS10 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpWS11 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpWS12 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpWS13 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkRpt1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkRpt2 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkRpt3 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkRpt4 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkRpt5 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkRpt6 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkRpt7 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkRpt8 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkRpt9 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkRpt10 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkRpt11 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkRpt12 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkRpt13 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkRpt14 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkRpt15 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkMth1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkMth2 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkMth3 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkMth4 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkMth5 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkMth6 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkMth7 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkMth8 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkMth9 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkMth10 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkMth11 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkMth12 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkAdminSetup As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkAdminDT As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkChgPwd As System.Web.UI.WebControls.HyperLink
    Protected WithEvents tblGrpTran As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSpc1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblGLHead As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblGL As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSpc2 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblBillHead As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblBill As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSpc3 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblAPHead As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblAP As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSpc4 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblCBHead As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblCB As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSpc5 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblPUHead As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblPU As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSpc6 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblINHead As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblIN As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSpc7 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblHRHead As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblHR As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSpc8 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblPRHead As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblPR As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSpc9 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblProdHead As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblProd As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSpc10 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblWMHead As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblWM As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSpc11 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblCMHead As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblCM As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSpc12 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblCTHead As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblCT As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSpc13 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblWSHead As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblWS As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSpc14 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblNUHead As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblNU As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSpc15 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblFAHead As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblFA As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSpc16 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblGrpStp As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSpc17 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblStp1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblStpFI As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSpc18 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tlbStp03 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tlbStpIN As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSpc19 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tlbStp04 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tlbStpHR As System.Web.UI.HtmlControls.HtmlTable

    Protected WithEvents tblSpc20 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tlbStp05 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tlbStpPR As System.Web.UI.HtmlControls.HtmlTable

    Protected WithEvents tblSpc21 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tlbStp06 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tlbStpFA As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSpc22 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tlbStp07 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tlbStpPD As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSpc23 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tlbStp08 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tlbStpNU As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSpc24 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tlbStp09 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tlbStpWS As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSpc25 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblGrpRpt As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tlbRpt As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSpc26 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblGrpMthEnd As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblMth As System.Web.UI.HtmlControls.HtmlTable

    Protected WithEvents tblSpc27 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblAdmin As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSpc28 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblOther As System.Web.UI.HtmlControls.HtmlTable

    Protected WithEvents tblSpc50 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tlbStp15 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tlbStpCM As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lnkStpCM01 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpCM02 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpCM03 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkStpCM04 As System.Web.UI.WebControls.HyperLink

    Protected WithEvents tblSpc51 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tlbStp16 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tlbStpWM As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lnkStpWM01 As System.Web.UI.WebControls.HyperLink

    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        InitializeComponent()

    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim strReferer As String = Request.QueryString("referer")

        strUserId = Session("SS_USERID")

        intModuleActivate = Session("SS_MODULEACTIVATE")
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strLangCode = Session("SS_LANGCODE")
        intLocType = Session("SS_LOCTYPE")
        strAccMonth = Session("SS_FAACCMONTH")
        strAccYear = Session("SS_FAACCYEAR")

        If strUserId = "" Then
            Response.Write("<Script language=""Javascript"">parent.right.location.href = '/SessionExpire.aspx'</Script>")

        ElseIf strLocation = "" Then

            If Request.QueryString("module") = C_ADMIN Then
                tblAdmin.Visible = True
            End If

            If strReferer = "" Then
                Response.Write("<Script language=""Javascript"">parent.right.location.href = 'system/user/setlocation.aspx'</Script>")
            Else
                Response.Write("<Script language=""Javascript"">parent.right.location.href = '" & strReferer & "'</Script>")
            End If
        End If

        Response.Write("<Script language=""Javascript"">parent.banner.location.href = '/banner.aspx'</Script>")

        intGLAR = Session("SS_GLAR")
        intBIAR = Session("SS_BIAR")
        intAPAR = Session("SS_APAR")
        intCBAR = Session("SS_CBAR")
        intPUAR = Session("SS_PUAR")
        intINAR = Session("SS_INAR")
        intHRAR = Session("SS_HRAR")
        intPRAR = Session("SS_PRAR")
        intPDAR = Session("SS_PDAR")
        intPMAR = Session("SS_PMAR")
        intWMAR = Session("SS_WMAR")
        intCMAR = Session("SS_CMAR")
        intCTAR = Session("SS_CTAR")
        intWSAR = Session("SS_WSAR")
        intNUAR = Session("SS_NUAR")
        intFAAR = Session("SS_FAAR")

        GetEntireLangCap()
        onLoad_Display()

        If Not IsPostBack Then
        End If
    End Sub

    Sub onLoad_Display()

        lnkAdminSetup.NavigateUrl = "/" & strLangCode & "/menu/menu_admin_page.aspx"
        lnkAdminDT.NavigateUrl = "/" & strLangCode & "/menu/menu_admindata_page.aspx"

        lnkChgPwd.NavigateUrl = "/" & strLangCode & "/changepassword.aspx?referer=/" & strLangCode & "/appmenu.aspx"


        If Session("SS_LOCATION") = "" Or Session("SS_MAXPERIOD") = 0 Then
            Call DisableAllHead()
        Else
            Call TrxGLRight()
            Call TrxARRight()
            Call TrxAPRight()
            Call TrxCBRight()
            Call TrxPURight()
            Call TrxINRight()
            Call TrxHRRight()
            Call TrxPRRight()
            Call TrxPDRight()
            Call TrxWMRight()
            Call TrxCMRight()
            Call TrxCTRight()
            Call TrxWSRight()
            Call TrxNURight()
            Call TrxFARight()
            Call StpGLRight()
            Call StpINRight()
            Call StpHRRight()
            Call StpPRRight()
            Call StpFARight()
            Call StpPDRight()
            Call StpNURight()
            Call StpWSRight()
            If blnGLS = False And _
               blnINS = False And _
               blnHRS = False And _
               blnPRS = False And _
               blnFAS = False And _
               blnPDS = False And _
               blnNUS = False And _
               blnWSS = False Then

                tblSpc16.Visible = False
                tblGrpStp.Visible = False
                tblSpc17.Visible = False

            End If

            Call ReportsRight()
            Call MonthEndRight()
        End If

    End Sub


    Private Sub DisableAllHead()
        tblGrpTran.Visible = False
        tblSpc1.Visible = False
        tblGLHead.Visible = False
        tblSpc2.Visible = False
        tblBillHead.Visible = False
        tblSpc3.Visible = False
        tblAPHead.Visible = False
        tblSpc4.Visible = False
        tblCBHead.Visible = False
        tblSpc5.Visible = False
        tblPUHead.Visible = False
        tblSpc6.Visible = False
        tblINHead.Visible = False
        tblSpc7.Visible = False
        tblHRHead.Visible = False
        tblSpc8.Visible = False
        tblPRHead.Visible = False
        tblSpc9.Visible = False
        tblProdHead.Visible = False
        tblSpc10.Visible = False
        tblWMHead.Visible = False
        tblSpc11.Visible = False
        tblCMHead.Visible = False
        tblSpc12.Visible = False
        tblCTHead.Visible = False
        tblSpc13.Visible = False
        tblWSHead.Visible = False
        tblSpc14.Visible = False
        tblNUHead.Visible = False
        tblSpc15.Visible = False
        tblFAHead.Visible = False
        tblSpc16.Visible = False

        tblGrpStp.Visible = False
        tblSpc17.Visible = False
        tblStp1.Visible = False
        tblSpc18.Visible = False
        tlbStp03.Visible = False
        tblSpc19.Visible = False
        tlbStp04.Visible = False
        tblSpc20.Visible = False
        tlbStp05.Visible = False
        tblSpc21.Visible = False
        tlbStp06.Visible = False
        tblSpc22.Visible = False
        tlbStp07.Visible = False
        tblSpc23.Visible = False
        tlbStp08.Visible = False
        tblSpc24.Visible = False
        tlbStp09.Visible = False

        tblSpc25.Visible = False
        tblGrpRpt.Visible = False

        tblSpc26.Visible = False
        tblGrpMthEnd.Visible = False

        tblSpc50.Visible = False
        tlbStp15.Visible = False
        tlbStpCM.Visible = False

        tblSpc51.Visible = False
        tlbStp16.Visible = False
        tlbStpWM.Visible = False


    End Sub

    Sub GetEntireLangCap()
        Dim objLangCapDs As New DataSet()
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

            objLangCapVw = New DataView(objLangCapDs.Tables(0))

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=MENU_FASETUP_GET_LANGCAP&errmesg=&redirect=")
        End Try

    End Sub


    Function GetCaption(ByVal pv_TermCode As Integer) As String
        Dim intIndex As Integer

        objLangCapVw.Sort = "TermCode"

        intIndex = objLangCapVw.Find(pv_TermCode)

        If intIndex = -1 Then
            Return ""
        Else
            If intLocType = 4 Then
                Return Trim(objLangCapVw(intIndex).Item("BusinessTermMW"))
            Else
                Return Trim(objLangCapVw(intIndex).Item("BusinessTerm"))
            End If
        End If

    End Function



    Private Sub TrxGLRight()
        Dim blnGL As String

        blnGL = False

        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.GeneralLedger), intModuleActivate) Then


            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumGLAccessRights.GLJournal), intGLAR) = True) Then
                tblGL.Rows(0).Visible = True
                blnGL = True
            Else
                tblGL.Rows(0).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumGLAccessRights.GLPosting), intGLAR) = True) Then
                tblGL.Rows(1).Visible = True
                blnGL = True
            Else
                tblGL.Rows(1).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumGLAccessRights.GLJournalAdj), intGLAR) = True) Then
                tblGL.Rows(2).Visible = True
                blnGL = True
            Else
                tblGL.Rows(2).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumGLAccessRights.GLVehicleUsage), intGLAR) = True) Then
                tblGL.Rows(3).Visible = True
                blnGL = True
            Else
                tblGL.Rows(3).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumGLAccessRights.GLRunHour), intGLAR) = True) Then
                tblGL.Rows(4).Visible = True
                blnGL = True
            Else
                tblGL.Rows(4).Visible = False
            End If

            If blnGL = False Then
                tblGLHead.Visible = False
                tblGL.Visible = False
                tblSpc1.Visible = False
            End If

        Else
            tblGLHead.Visible = False
            tblGL.Visible = False
            tblSpc1.Visible = False
        End If

    End Sub


    Private Sub TrxARRight()
        Dim blnAR As Boolean

        blnAR = False

        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.Billing), intModuleActivate) Then


            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumBIAccessRights.BIInvoice), intBIAR) = True) Then
                tblBill.Rows(0).Visible = True
                blnAR = True
            Else
                tblBill.Rows(0).Visible = False
            End If

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumBIAccessRights.BINote), intBIAR) = True) Then
                tblBill.Rows(1).Visible = True
                blnAR = True
            Else
                tblBill.Rows(1).Visible = False
            End If

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumBIAccessRights.BICreditNote), intBIAR) = True) Then
                tblBill.Rows(2).Visible = True
                blnAR = True
            Else
                tblBill.Rows(2).Visible = False
            End If

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumBIAccessRights.BIJournal), intBIAR) = True) Then
                tblBill.Rows(3).Visible = True
                blnAR = True
            Else
                tblBill.Rows(3).Visible = False
            End If

            If blnAR = False Then
                tblBillHead.Visible = False
                tblBill.Visible = False
                tblSpc2.Visible = False
            End If

        Else
            tblBillHead.Visible = False
            tblBill.Visible = False
            tblSpc2.Visible = False
        End If



    End Sub

    Private Sub TrxAPRight()
        Dim blnAP As Boolean
        blnAP = False

        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.AccountPayable), intModuleActivate) Then

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumAPAccessRights.APInvoiceReceive), intAPAR) = True) Then
                tblAP.Rows(0).Visible = True
                blnAP = True
            Else
                tblAP.Rows(0).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumAPAccessRights.APDebitNote), intAPAR) = True) Then
                tblAP.Rows(1).Visible = True
                blnAP = True
            Else
                tblAP.Rows(1).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumAPAccessRights.APCreditNote), intAPAR) = True) Then
                tblAP.Rows(2).Visible = True
                blnAP = True
            Else
                tblAP.Rows(2).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumAPAccessRights.APCreditorJournal), intAPAR) = True) Then
                tblAP.Rows(3).Visible = True
                blnAP = True
            Else
                tblAP.Rows(3).Visible = False
            End If

            If blnAP = False Then
                tblAPHead.Visible = False
                tblAP.Visible = False
                tblSpc3.Visible = False
            End If
        Else
            tblAPHead.Visible = False
            tblAP.Visible = False
            tblSpc3.Visible = False
        End If
    End Sub

    Private Sub TrxCBRight()
        Dim blnCB As Boolean
        blnCB = False

        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.CashAndBank), intModuleActivate) Then

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCBAccessRights.CBPayment), intCBAR) = True) Then
                tblCB.Rows(0).Visible = True
                blnCB = True
            Else
                tblCB.Rows(0).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCBAccessRights.CBReceipt), intCBAR) = True) Then
                tblCB.Rows(1).Visible = True
                blnCB = True
            Else
                tblCB.Rows(1).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCBAccessRights.CBCashBank), intCBAR) = True) Then
                tblCB.Rows(2).Visible = True
                blnCB = True
            Else
                tblCB.Rows(2).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCBAccessRights.CBDeposit), intCBAR) = True) Then
                tblCB.Rows(3).Visible = True
                blnCB = True
            Else
                tblCB.Rows(3).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCBAccessRights.CBInterAdj), intCBAR) = True) Then
                tblCB.Rows(4).Visible = True
                blnCB = True
            Else
                tblCB.Rows(4).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCBAccessRights.CBWithdrawal), intCBAR) = True) Then
                tblCB.Rows(5).Visible = True
                blnCB = True
            Else
                tblCB.Rows(5).Visible = False
            End If

            If blnCB = False Then
                tblCBHead.Visible = False
                tblCB.Visible = False
                tblSpc4.Visible = False
            End If

        Else
            tblCBHead.Visible = False
            tblCB.Visible = False
            tblSpc4.Visible = False
        End If
    End Sub


    Private Sub TrxPURight()
        Dim blnPU As Boolean

        blnPU = False

        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.Purchasing), intModuleActivate) Then


            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPUAccessRights.PURPH), intPUAR) = True) Then
                tblPU.Rows(0).Visible = True
                blnPU = True
            Else
                tblPU.Rows(0).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPUAccessRights.PUPurchaseOrder), intPUAR) = True) Then
                tblPU.Rows(1).Visible = True
                blnPU = True
            Else
                tblPU.Rows(1).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPUAccessRights.PUGoodsReceive), intPUAR) = True) Then
                tblPU.Rows(2).Visible = True
                blnPU = True
            Else
                tblPU.Rows(2).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPUAccessRights.PUGoodsReturnNote), intPUAR) = True) Then
                tblPU.Rows(3).Visible = True
                blnPU = True
            Else
                tblPU.Rows(3).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPUAccessRights.PUDispatchAdvice), intPUAR) = True) Then
                tblPU.Rows(4).Visible = True
                blnPU = True
            Else
                tblPU.Rows(4).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPUAccessRights.PUPelimpahan), intPUAR) = True) Then
                tblPU.Rows(5).Visible = True
                blnPU = True
            Else
                tblPU.Rows(5).Visible = False
            End If

            If blnPU = False Then
                tblPUHead.Visible = False
                tblPU.Visible = False
                tblSpc5.Visible = False
            End If

        Else
            tblPUHead.Visible = False
            tblPU.Visible = False
            tblSpc5.Visible = False
        End If
    End Sub

    Private Sub TrxINRight()

        Dim blnIN As Boolean
        blnIN = False

        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.Inventory), intModuleActivate) Then


            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumINAccessRights.INPurchaseRequest), intINAR) = True) Then
                tblIN.Rows(0).Visible = True
                blnIN = True
            Else
                tblIN.Rows(0).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumINAccessRights.INStockReceive), intINAR) = True) Then
                tblIN.Rows(1).Visible = True
                blnIN = True
            Else
                tblIN.Rows(1).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumINAccessRights.INStockReturnAdvice), intINAR) = True) Then
                tblIN.Rows(2).Visible = True
                blnIN = True
            Else
                tblIN.Rows(2).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumINAccessRights.INStockAdjustment), intINAR) = True) Then
                tblIN.Rows(3).Visible = True
                blnIN = True
            Else
                tblIN.Rows(3).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumINAccessRights.INStockTransfer), intINAR) = True) Then
                tblIN.Rows(4).Visible = True
                blnIN = True
            Else
                tblIN.Rows(4).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumINAccessRights.INStockIssue), intINAR) = True) Then
                tblIN.Rows(5).Visible = True
                blnIN = True
            Else
                tblIN.Rows(5).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumINAccessRights.INStockReturn), intINAR) = True) Then
                tblIN.Rows(6).Visible = True
                blnIN = True
            Else
                tblIN.Rows(6).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumINAccessRights.INFuelIssue), intINAR) = True) Then
                tblIN.Rows(7).Visible = True
                blnIN = True
            Else
                tblIN.Rows(7).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumINAccessRights.INItemToMachine), intINAR) = True) Then
                tblIN.Rows(8).Visible = True
                blnIN = True
            Else
                tblIN.Rows(8).Visible = False
            End If

            If blnIN = False Then
                tblINHead.Visible = False
                tblIN.Visible = False
                tblSpc6.Visible = False
            End If

        Else
            tblINHead.Visible = False
            tblIN.Visible = False
            tblSpc6.Visible = False
        End If
    End Sub

    Private Sub TrxHRRight()
        Dim blnHR As Boolean
        blnHR = False

        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.HumanResource), intModuleActivate) Then

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HREmployeeDetails), intHRAR) = True) Then
                tblHR.Rows(0).Visible = True
                blnHR = True
            Else
                tblHR.Rows(0).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HREmployeePayroll), intHRAR) = True) Then
                tblHR.Rows(1).Visible = True
                blnHR = True
            Else
                tblHR.Rows(1).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HREmployeeEmployement), intHRAR) = True) Then
                tblHR.Rows(2).Visible = True
                blnHR = True
            Else
                tblHR.Rows(2).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HREmployeeStatutory), intHRAR) = True) Then
                tblHR.Rows(3).Visible = True
                blnHR = True
            Else
                tblHR.Rows(3).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HREmployeeFamily), intHRAR) = True) Then
                tblHR.Rows(4).Visible = True
                blnHR = True
            Else
                tblHR.Rows(4).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HREmployeeQualification), intHRAR) = True) Then
                tblHR.Rows(5).Visible = True
                blnHR = True
            Else
                tblHR.Rows(5).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HREmployeeSkill), intHRAR) = True) Then
                tblHR.Rows(6).Visible = True
                blnHR = True
            Else
                tblHR.Rows(6).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HRCareerProgress), intHRAR) = True) Then
                tblHR.Rows(7).Visible = True
                blnHR = True
            Else
                tblHR.Rows(7).Visible = False
            End If

            If blnHR = False Then
                tblHRHead.Visible = False
                tblHR.Visible = False
                tblSpc7.Visible = False
            End If
        Else
            tblHRHead.Visible = False
            tblHR.Visible = False
            tblSpc7.Visible = False
        End If
    End Sub

    Private Sub TrxPRRight()
        Dim blnPR As Boolean
        blnPR = False

        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.Payroll), intModuleActivate) Then

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRTrxRatePayment), intPRAR) = True) Then
                tblPR.Rows(0).Visible = True
                blnPR = True
            Else
                tblPR.Rows(0).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRTrxContractPayment), intPRAR) = True) Then
                tblPR.Rows(1).Visible = True
                blnPR = True
            Else
                tblPR.Rows(1).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRTrxTrip), intPRAR) = True) Then
                tblPR.Rows(2).Visible = True
                blnPR = True
            Else
                tblPR.Rows(2).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRTrxWagesPayment), intPRAR) = True) Then
                tblPR.Rows(3).Visible = True
                blnPR = True
            Else
                tblPR.Rows(3).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRTrxAllowanceDeduction), intPRAR) = True) Then
                tblPR.Rows(4).Visible = True
                blnPR = True
            Else
                tblPR.Rows(4).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRTrxWorkPerformance), intPRAR) = True) Then
                tblPR.Rows(5).Visible = True
                blnPR = True
            Else
                tblPR.Rows(5).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRDailyAttd), intPRAR) = True) Then
                tblPR.Rows(6).Visible = True
                blnPR = True
            Else
                tblPR.Rows(6).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRHarvAttd), intPRAR) = True) Then
                tblPR.Rows(7).Visible = True
                blnPR = True
            Else
                tblPR.Rows(7).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRWeekly), intPRAR) = True) Then
                tblPR.Rows(8).Visible = True
                blnPR = True
            Else
                tblPR.Rows(8).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRTrxAttendance), intPRAR) = True) Then
                tblPR.Rows(9).Visible = True
                blnPR = True
            Else
                tblPR.Rows(9).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRTrxContractorCheckroll), intPRAR) = True) Then
                tblPR.Rows(10).Visible = True
                blnPR = True
            Else
                tblPR.Rows(10).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRTrxWPContractor), intPRAR) = True) Then
                tblPR.Rows(11).Visible = True
                blnPR = True
            Else
                tblPR.Rows(11).Visible = False
            End If
        Else
            tblPRHead.Visible = False
            tblPR.Visible = False
            tblSpc8.Visible = False
        End If

        If blnPR = False Then
            tblPRHead.Visible = False
            tblPR.Visible = False
            tblSpc8.Visible = False
        End If

    End Sub

    Private Sub TrxPDRight()
        Dim blnProd As Boolean
        Dim blnIsProd As Boolean
        blnIsProd = False

        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.Production), intModuleActivate) Then
            blnProd = True
        End If
        If Session("PW_ISMILLWARE") = True And _
            objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.MillProduction), intModuleActivate) Then
            blnProd = True
        End If

        If blnProd = True Then

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPDAccessRights.PDEstateProduction), intPDAR) = True) Then
                tblProd.Rows(0).Visible = True
                blnIsProd = True
            Else
                tblProd.Rows(0).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPMAccessRights.PMDailyProduction), intPMAR) = True) Then
                tblProd.Rows(1).Visible = True
                blnIsProd = True
            Else
                tblProd.Rows(1).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPMAccessRights.PMOilLoss), intPMAR) = True) Then
                tblProd.Rows(2).Visible = True
                blnIsProd = True
            Else
                tblProd.Rows(2).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPMAccessRights.PMOilQuality), intPMAR) = True) Then
                tblProd.Rows(3).Visible = True
                blnIsProd = True
            Else
                tblProd.Rows(3).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPMAccessRights.PMKernelQuality), intPMAR) = True) Then
                tblProd.Rows(4).Visible = True
                blnIsProd = True
            Else
                tblProd.Rows(4).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPMAccessRights.PMProducedKernel), intPMAR) = True) Then
                tblProd.Rows(5).Visible = True
                blnIsProd = True
            Else
                tblProd.Rows(5).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPMAccessRights.PMDispatchedKernel), intPMAR) = True) Then
                tblProd.Rows(6).Visible = True
                blnIsProd = True
            Else
                tblProd.Rows(6).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPMAccessRights.PMCPOStorage), intPMAR) = True) Then
                tblProd.Rows(7).Visible = True
                blnIsProd = True
            Else
                tblProd.Rows(7).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPMAccessRights.PMPKStorage), intPMAR) = True) Then
                tblProd.Rows(8).Visible = True
                blnIsProd = True
            Else
                tblProd.Rows(8).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPMAccessRights.PMKernelLoss), intPMAR) = True) Then
                tblProd.Rows(9).Visible = True
                blnIsProd = True
            Else
                tblProd.Rows(9).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPMAccessRights.PMWaterQuality), intPMAR) = True) Then
                tblProd.Rows(10).Visible = True
                blnIsProd = True
            Else
                tblProd.Rows(10).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPMAccessRights.PMWastedWaterQuality), intPMAR) = True) Then
                tblProd.Rows(11).Visible = True
                blnIsProd = True
            Else
                tblProd.Rows(11).Visible = False
            End If

            If blnIsProd = False Then
                tblProdHead.Visible = False
                tblProd.Visible = False
                tblSpc9.Visible = False
            End If

        Else
            tblProdHead.Visible = False
            tblProd.Visible = False
            tblSpc9.Visible = False
        End If

    End Sub

    Private Sub TrxWMRight()
        Dim blnWM As Boolean
        blnWM = False

        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.MillWeighing), intModuleActivate) Then

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumWMAccessRights.WMWeighBridgeTicket), intWMAR) = True) Then
                tblWM.Rows(0).Visible = True
                blnWM = True
            Else
                tblWM.Rows(0).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumWMAccessRights.WMFFBAssessment), intWMAR) = True) Then
                tblWM.Rows(1).Visible = True
                blnWM = True
            Else
                tblWM.Rows(1).Visible = False
            End If

            If blnWM = False Then
                tblWMHead.Visible = False
                tblWM.Visible = False
                tblSpc10.Visible = False

                tblSpc51.Visible = False
                tlbStp16.Visible = False
                tlbStpWM.Visible = False
            End If

        Else
            tblWMHead.Visible = False
            tblWM.Visible = False
            tblSpc10.Visible = False

            tblSpc51.Visible = False
            tlbStp16.Visible = False
            tlbStpWM.Visible = False
        End If
    End Sub

    Private Sub TrxCMRight()
        Dim blnCM As Boolean

        blnCM = False

        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.MillContract), intModuleActivate) Then


            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCMAccessRights.CMContractReg), intCMAR) = True) Then
                tblCM.Rows(0).Visible = True
                blnCM = True
            Else
                tblCM.Rows(0).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCMAccessRights.CMContractMatching), intCMAR) = True) Then
                tblCM.Rows(1).Visible = True
                blnCM = True
            Else
                tblCM.Rows(1).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCMAccessRights.CMDORegistration), intCMAR) = True) Then
                tblCM.Rows(2).Visible = True
                blnCM = True
            Else
                tblCM.Rows(2).Visible = False
            End If
            If blnCM = False Then
                tblCMHead.Visible = False
                tblCM.Visible = False
                tblSpc11.Visible = False

                tblSpc50.Visible = False
                tlbStp15.Visible = False
                tlbStpCM.Visible = False

            End If



        Else
            tblCMHead.Visible = False
            tblCM.Visible = False
            tblSpc11.Visible = False

            tblSpc50.Visible = False
            tlbStp15.Visible = False
            tlbStpCM.Visible = False

        End If
    End Sub

    Private Sub TrxCTRight()

        Dim blnCT As Boolean
        blnCT = False

        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.Canteen), intModuleActivate) Then


            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCTAccessRights.CTPurchaseRequest), intCTAR) = True) Then
                tblCT.Rows(0).Visible = True
                blnCT = True
            Else
                tblCT.Rows(0).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCTAccessRights.CTReceive), intCTAR) = True) Then
                tblCT.Rows(1).Visible = True
                blnCT = True
            Else
                tblCT.Rows(1).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCTAccessRights.CTReturnAdvice), intCTAR) = True) Then
                tblCT.Rows(2).Visible = True
                blnCT = True
            Else
                tblCT.Rows(2).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCTAccessRights.CTAdjustment), intCTAR) = True) Then
                tblCT.Rows(3).Visible = True
                blnCT = True
            Else
                tblCT.Rows(3).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCTAccessRights.CTTransfer), intCTAR) = True) Then
                tblCT.Rows(4).Visible = True
                blnCT = True
            Else
                tblCT.Rows(4).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCTAccessRights.CTIssue), intCTAR) = True) Then
                tblCT.Rows(5).Visible = True
                blnCT = True
            Else
                tblCT.Rows(5).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCTAccessRights.CTReturn), intCTAR) = True) Then
                tblCT.Rows(6).Visible = True
                blnCT = True
            Else
                tblCT.Rows(6).Visible = False
            End If
            If blnCT = False Then
                tblCTHead.Visible = False
                tblCT.Visible = False
                tblSpc12.Visible = False
            End If
        Else
            tblCTHead.Visible = False
            tblCT.Visible = False
            tblSpc12.Visible = False
        End If
    End Sub

    Private Sub TrxWSRight()

        Dim blnWS As Boolean
        blnWS = False

        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.Workshop), intModuleActivate) Then

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumWSAccessRights.WSJobRegistration), intWSAR) = True) Then
                tblWS.Rows(0).Visible = True
                blnWS = True
            Else
                tblWS.Rows(0).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumWSAccessRights.WSMechanicHour), intWSAR) = True) Then
                tblWS.Rows(1).Visible = True
                blnWS = True
            Else
                tblWS.Rows(1).Visible = False
            End If

            If blnWS = False Then
                tblWSHead.Visible = False
                tblWS.Visible = False
                tblSpc13.Visible = False
            End If

        Else
            tblWSHead.Visible = False
            tblWS.Visible = False
            tblSpc13.Visible = False
        End If
    End Sub

    Private Sub TrxNURight()
        Dim blnNU As Boolean
        blnNU = False

        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.Nursery), intModuleActivate) Then

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumNUAccessRights.NUPurchaseRequest), intNUAR) = True) Then
                tblNU.Rows(0).Visible = True
                blnNU = True
            Else
                tblNU.Rows(0).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumNUAccessRights.NUSeedRcv), intNUAR) = True) Then
                tblNU.Rows(1).Visible = True
                blnNU = True
            Else
                tblNU.Rows(1).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumNUAccessRights.NUSeedPlant), intNUAR) = True) Then
                tblNU.Rows(2).Visible = True
                blnNU = True
            Else
                tblNU.Rows(2).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumNUAccessRights.NUDblTurn), intNUAR) = True) Then
                tblNU.Rows(3).Visible = True
                blnNU = True
            Else
                tblNU.Rows(3).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumNUAccessRights.NUTransplanting), intNUAR) = True) Then
                tblNU.Rows(4).Visible = True
                blnNU = True
            Else
                tblNU.Rows(4).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumNUAccessRights.NUCulling), intNUAR) = True) Then
                tblNU.Rows(5).Visible = True
                blnNU = True
            Else
                tblNU.Rows(5).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumNUAccessRights.NUDispatch), intNUAR) = True) Then
                tblNU.Rows(6).Visible = True
                blnNU = True
            Else
                tblNU.Rows(6).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumNUAccessRights.NUSeedIssue), intNUAR) = True) Then
                tblNU.Rows(7).Visible = True
                blnNU = True
            Else
                tblNU.Rows(7).Visible = False
            End If

            If blnNU = False Then
                tblNUHead.Visible = False
                tblNU.Visible = False
                tblSpc14.Visible = False
            End If

        Else
            tblNUHead.Visible = False
            tblNU.Visible = False
            tblSpc14.Visible = False
        End If
    End Sub

    Private Sub TrxFARight()
        Dim blnFA As Boolean
        blnFA = False

        Dim strAssetAddTag As String
        Dim strAssetDeprTag As String
        Dim strAssetDispTag As String
        Dim strAssetWOTag As String

        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.FixAsset), intModuleActivate) Then

            strAssetAddTag = GetCaption(objLangCap.EnumLangCap.AssetAdd)
            strAssetDeprTag = GetCaption(objLangCap.EnumLangCap.AssetDepr)
            strAssetDispTag = GetCaption(objLangCap.EnumLangCap.AssetDisp)
            strAssetWOTag = GetCaption(objLangCap.EnumLangCap.AssetWO)

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumFAAccessRights.FAAddition), intFAAR) = True) Then
                tblFA.Rows(0).Visible = True
                lnkFA01.Text = strAssetAddTag
                blnFA = True
            Else
                tblFA.Rows(0).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumFAAccessRights.FADepreciation), intFAAR) = True) Then
                tblFA.Rows(1).Visible = True
                lnkFA02.Text = strAssetDeprTag
                blnFA = True
            Else
                tblFA.Rows(1).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumFAAccessRights.FADisposal), intFAAR) = True) Then
                tblFA.Rows(2).Visible = True
                lnkFA03.Text = strAssetDispTag
                blnFA = True
            Else
                tblFA.Rows(2).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumFAAccessRights.FAWriteOff), intFAAR) = True) Then
                tblFA.Rows(3).Visible = True
                lnkFA04.Text = strAssetWOTag
                blnFA = True
            Else
                tblFA.Rows(3).Visible = False
            End If
            If blnFA = False Then
                tblFAHead.Visible = False
                tblFA.Visible = False
                tblSpc15.Visible = False
            End If
        Else
            tblFAHead.Visible = False
            tblFA.Visible = False
            tblSpc15.Visible = False
        End If
    End Sub

    Private Sub StpGLRight()

        Dim strAccClsGrpTag As String
        Dim strAccClsTag As String
        Dim strActGrpTag As String
        Dim strActTag As String
        Dim strSubActTag As String
        Dim strExpenseCodeTag As String
        Dim strVehExpGrpCodeTag As String
        Dim strVehExpCodeTag As String
        Dim strVehTypeTag As String
        Dim strVehicleTag As String
        Dim strBlockGrpTag As String
        Dim strBlockTag As String
        Dim strSubBlockTag As String
        Dim strAccGrpTag As String
        Dim strAccountTag As String

        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.GeneralLedger), intModuleActivate) Then

            strAccClsGrpTag = GetCaption(objLangCap.EnumLangCap.AccClsGrp)
            strAccClsTag = GetCaption(objLangCap.EnumLangCap.AccClass)
            strActGrpTag = GetCaption(objLangCap.EnumLangCap.ActGrp)
            strActTag = GetCaption(objLangCap.EnumLangCap.Activity)
            strSubActTag = GetCaption(objLangCap.EnumLangCap.SubAct)
            strExpenseCodeTag = GetCaption(objLangCap.EnumLangCap.Expense) & " Code"
            strVehExpGrpCodeTag = GetCaption(objLangCap.EnumLangCap.VehExpGrp) & " Code"
            strVehExpCodeTag = GetCaption(objLangCap.EnumLangCap.VehExpense) & " Code"
            strVehTypeTag = GetCaption(objLangCap.EnumLangCap.VehType)
            strVehicleTag = GetCaption(objLangCap.EnumLangCap.Vehicle)
            strBlockGrpTag = GetCaption(objLangCap.EnumLangCap.BlockGrp)
            strBlockTag = GetCaption(objLangCap.EnumLangCap.Block)
            strSubBlockTag = GetCaption(objLangCap.EnumLangCap.SubBlock)
            strAccGrpTag = GetCaption(objLangCap.EnumLangCap.AccGrp)
            strAccountTag = GetCaption(objLangCap.EnumLangCap.Account)

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumGLAccessRights.GLAccCls), intGLAR) = True) Then
                tblStpFI.Rows(0).Visible = True
                lnkStpFI01.Text = strAccClsTag
                blnGLS = True
            Else
                tblStpFI.Rows(0).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumGLAccessRights.GLAccClsGrp), intGLAR) = True) Then
                tblStpFI.Rows(1).Visible = True
                lnkStpFI02.Text = strAccClsGrpTag
                blnGLS = True
            Else
                tblStpFI.Rows(1).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumGLAccessRights.GLActivityGrp), intGLAR) = True) Then
                tblStpFI.Rows(2).Visible = True
                lnkStpFI03.Text = strActGrpTag
                blnGLS = True
            Else
                tblStpFI.Rows(2).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumGLAccessRights.GLActivity), intGLAR) = True) Then
                tblStpFI.Rows(3).Visible = True
                lnkStpFI04.Text = strActTag
                blnGLS = True
            Else
                tblStpFI.Rows(3).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumGLAccessRights.GLSubActivity), intGLAR) = True) Then
                tblStpFI.Rows(4).Visible = True
                lnkStpFI05.Text = strSubActTag
                blnGLS = True
            Else
                tblStpFI.Rows(4).Visible = False
            End If

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumGLAccessRights.GLExpense), intGLAR) = True) Then
                tblStpFI.Rows(5).Visible = True
                lnkStpFI06.Text = strExpenseCodeTag
                blnGLS = True
            Else
                tblStpFI.Rows(5).Visible = False
            End If

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumGLAccessRights.GLVehicleExpenseGrp), intGLAR) = True) Then
                tblStpFI.Rows(6).Visible = True
                lnkStpFI07.Text = strVehExpGrpCodeTag
                blnGLS = True
            Else
                tblStpFI.Rows(6).Visible = False
            End If

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumGLAccessRights.GLVehicleExpense), intGLAR) = True) Then
                tblStpFI.Rows(7).Visible = True
                lnkStpFI08.Text = strVehExpCodeTag
                blnGLS = True
            Else
                tblStpFI.Rows(7).Visible = False
            End If

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumGLAccessRights.GLVehicle), intGLAR) = True) Then
                tblStpFI.Rows(8).Visible = True
                lnkStpFI09.Text = strVehicleTag
                blnGLS = True
            Else
                tblStpFI.Rows(8).Visible = False
            End If

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumGLAccessRights.GLVehicleType), intGLAR) = True) Then
                tblStpFI.Rows(9).Visible = True
                lnkStpFI10.Text = strVehTypeTag
                blnGLS = True
            Else
                tblStpFI.Rows(9).Visible = False
            End If

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumGLAccessRights.GLBlock), intGLAR) = True) Then
                tblStpFI.Rows(10).Visible = True
                lnkStpFI11.Text = strBlockGrpTag
                blnGLS = True
            Else
                tblStpFI.Rows(10).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumGLAccessRights.GLBlk), intGLAR) = True) Then
                tblStpFI.Rows(11).Visible = True
                lnkStpFI12.Text = strBlockTag
                blnGLS = True
            Else
                tblStpFI.Rows(11).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumGLAccessRights.GLSubBlk), intGLAR) = True) Then
                tblStpFI.Rows(12).Visible = True
                lnkStpFI13.Text = strSubBlockTag
                blnGLS = True
            Else
                tblStpFI.Rows(12).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumGLAccessRights.GLAccountGrp), intGLAR) = True) Then
                tblStpFI.Rows(13).Visible = True
                lnkStpFI14.Text = strAccGrpTag
                blnGLS = True
            Else
                tblStpFI.Rows(13).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumGLAccessRights.GLAccount), intGLAR) = True) Then
                tblStpFI.Rows(14).Visible = True
                lnkStpFI15.Text = strAccountTag
                blnGLS = True
            Else
                tblStpFI.Rows(14).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumGLAccessRights.GLEntrySetup), intGLAR) = True) Then
                tblStpFI.Rows(15).Visible = True
                blnGLS = True
            Else
                tblStpFI.Rows(15).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumGLAccessRights.GLBalSheetSetup), intGLAR) = True) Then
                tblStpFI.Rows(16).Visible = True
                blnGLS = True
            Else
                tblStpFI.Rows(16).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumGLAccessRights.GLProfLossSetup), intGLAR) = True) Then
                tblStpFI.Rows(17).Visible = True
                blnGLS = True
            Else
                tblStpFI.Rows(17).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumGLAccessRights.GLCOGS), intGLAR) = True) Then
                tblStpFI.Rows(18).Visible = True
                tblStpFI.Rows(19).Visible = True
                blnGLS = True
            Else
                tblStpFI.Rows(18).Visible = False
                tblStpFI.Rows(19).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumGLAccessRights.GLFSSETUP), intGLAR) = True) Then
                tblStpFI.Rows(20).Visible = True
                blnGLS = True
            Else
                tblStpFI.Rows(20).Visible = False
            End If

            tblStpFI.Rows(21).Visible = True
            tblStpFI.Rows(22).Visible = True
           
            If blnGLS = False Then
                 tblStp1.Visible = False
            	tblStpFI.Visible = False
            	tblSpc1.Visible = False
            End If

        Else
            tblStp1.Visible = False
            tblStpFI.Visible = False
            tblSpc1.Visible = False
        End If
    End Sub

    Private Sub StpINRight()

        Dim strProdTypeTag As String
        Dim strProdBrandTag As String
        Dim strProdModelTag As String
        Dim strProdCatTag As String
        Dim strProdMatTag As String
        Dim strStockAnaTag As String
        Dim strStockMasterTag As String
        Dim strStockItemTag As String
        Dim strDirectMasterTag As String
        Dim strDirectChargeTag As String

        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.Inventory), intModuleActivate) Then

            strProdTypeTag = GetCaption(objLangCap.EnumLangCap.ProdType)
            strProdBrandTag = GetCaption(objLangCap.EnumLangCap.ProdBrand)
            strProdModelTag = GetCaption(objLangCap.EnumLangCap.ProdModel)
            strProdCatTag = GetCaption(objLangCap.EnumLangCap.ProdCat)
            strProdMatTag = GetCaption(objLangCap.EnumLangCap.ProdMat)
            strStockAnaTag = GetCaption(objLangCap.EnumLangCap.StockAnalysis)
            strStockMasterTag = GetCaption(objLangCap.EnumLangCap.StockMaster)
            strDirectMasterTag = GetCaption(objLangCap.EnumLangCap.DirectChgMaster)
            strStockItemTag = GetCaption(objLangCap.EnumLangCap.StockItem)
            strDirectChargeTag = GetCaption(objLangCap.EnumLangCap.DirectChgItem)

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumINAccessRights.INProductSetup), intINAR) = True) Then
                tlbStpIN.Rows(0).Visible = True
                lnkStpIN01.Text = strProdTypeTag
                blnINS = True
            Else
                tlbStpIN.Rows(0).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumINAccessRights.INProductBrand), intINAR) = True) Then
                tlbStpIN.Rows(1).Visible = True
                lnkStpIN02.Text = strProdBrandTag
                blnINS = True
            Else
                tlbStpIN.Rows(1).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumINAccessRights.INProductModel), intINAR) = True) Then
                tlbStpIN.Rows(2).Visible = True
                lnkStpIN03.Text = strProdModelTag
                blnINS = True
            Else
                tlbStpIN.Rows(2).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumINAccessRights.INProductCategory), intINAR) = True) Then
                tlbStpIN.Rows(3).Visible = True
                lnkStpIN04.Text = strProdCatTag
                blnINS = True
            Else
                tlbStpIN.Rows(3).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumINAccessRights.INProductMaterial), intINAR) = True) Then
                tlbStpIN.Rows(4).Visible = True
                lnkStpIN05.Text = strProdMatTag
                blnINS = True
            Else
                tlbStpIN.Rows(4).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumINAccessRights.INAnalisisStok), intINAR) = True) Then
                tlbStpIN.Rows(5).Visible = True
                lnkStpIN06.Text = strStockAnaTag
                blnINS = True
            Else
                tlbStpIN.Rows(5).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumINAccessRights.INStockItemMaster), intINAR) = True) Then
                tlbStpIN.Rows(6).Visible = True
                lnkStpIN07.Text = strStockMasterTag
                blnINS = True
            Else
                tlbStpIN.Rows(6).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumINAccessRights.INStockItem), intINAR) = True) Then
                tlbStpIN.Rows(7).Visible = True
                lnkStpIN08.Text = strStockItemTag
                blnINS = True
            Else
                tlbStpIN.Rows(7).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumINAccessRights.INDirectChargeItem), intINAR) = True) Then
                tlbStpIN.Rows(8).Visible = True
                lnkStpIN09.Text = strDirectMasterTag
                blnINS = True
            Else
                tlbStpIN.Rows(8).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumINAccessRights.INDirectChargeItem), intINAR) = True) Then
                tlbStpIN.Rows(9).Visible = True
                lnkStpIN10.Text = strDirectChargeTag
                blnINS = True
            Else
                tlbStpIN.Rows(9).Visible = False
            End If

            If blnINS = False Then
                tlbStp03.Visible = False
                tlbStpIN.Visible = False
                tblSpc18.Visible = False
            End If
        Else
            tlbStp03.Visible = False
            tlbStpIN.Visible = False
            tblSpc18.Visible = False
        End If
    End Sub

    Private Sub StpHRRight()

        Dim strDeptCodeTag As String
        Dim strDeptTag As String
        Dim strFuncTag As String
        Dim strLevelTag As String
        Dim strReligionTag As String
        Dim strCareerProgTag As String
        Dim strJamsostekTag As String
        Dim strPOHTag As String

        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.HumanResource), intModuleActivate) Then

            strDeptCodeTag = GetCaption(objLangCap.EnumLangCap.Department) & " Code"
            strDeptTag = GetCaption(objLangCap.EnumLangCap.Department)
            strFuncTag = GetCaption(objLangCap.EnumLangCap.Func)
            strLevelTag = GetCaption(objLangCap.EnumLangCap.Level)
            strReligionTag = GetCaption(objLangCap.EnumLangCap.Religion)
            strCareerProgTag = GetCaption(objLangCap.EnumLangCap.CareerProgress)
            strJamsostekTag = GetCaption(objLangCap.EnumLangCap.Jamsostek)
            strPOHTag = GetCaption(objLangCap.EnumLangCap.POHCode)

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HRPointHired), intHRAR) = True) Then
                tlbStpHR.Rows(0).Visible = True
                lnkStpHR01.Text = strPOHTag
                blnHRS = True
            Else
                tlbStpHR.Rows(0).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HRDepartment), intHRAR) = True) Then
                tlbStpHR.Rows(1).Visible = True
                lnkStpHR02.Text = strDeptCodeTag
                blnHRS = True
            Else
                tlbStpHR.Rows(1).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HRCompany), intHRAR) = True) Then
                tlbStpHR.Rows(2).Visible = True
                lnkStpHR03.Text = strDeptTag
                blnHRS = True
            Else
                tlbStpHR.Rows(2).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HRNationality), intHRAR) = True) Then
                tlbStpHR.Rows(3).Visible = True
                blnHRS = True
            Else
                tlbStpHR.Rows(3).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HRFunction), intHRAR) = True) Then
                tlbStpHR.Rows(4).Visible = True
                lnkStpHR05.Text = strFuncTag
                blnHRS = True
            Else
                tlbStpHR.Rows(4).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HRPosition), intHRAR) = True) Then
                tlbStpHR.Rows(5).Visible = True
                blnHRS = True
            Else
                tlbStpHR.Rows(5).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HRLevel), intHRAR) = True) Then
                tlbStpHR.Rows(6).Visible = True
                lnkStpHR07.Text = strLevelTag
                blnHRS = True
            Else
                tlbStpHR.Rows(6).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HRReligion), intHRAR) = True) Then
                tlbStpHR.Rows(7).Visible = True
                lnkStpHR08.Text = strReligionTag
                blnHRS = True
            Else
                tlbStpHR.Rows(7).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HRICType), intHRAR) = True) Then
                tlbStpHR.Rows(8).Visible = True
                blnHRS = True
            Else
                tlbStpHR.Rows(8).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HRRace), intHRAR) = True) Then
                tlbStpHR.Rows(9).Visible = True
                blnHRS = True
            Else
                tlbStpHR.Rows(9).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HRSkill), intHRAR) = True) Then
                tlbStpHR.Rows(10).Visible = True
                blnHRS = True
            Else
                tlbStpHR.Rows(10).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HRShift), intHRAR) = True) Then
                tlbStpHR.Rows(11).Visible = True
                blnHRS = True
            Else
                tlbStpHR.Rows(11).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HRQualification), intHRAR) = True) Then
                tlbStpHR.Rows(12).Visible = True
                blnHRS = True
            Else
                tlbStpHR.Rows(12).Visible = False
            End If

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HRSubject), intHRAR) = True) Then
                tlbStpHR.Rows(13).Visible = True
                blnHRS = True
            Else
                tlbStpHR.Rows(13).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HREvaluation), intHRAR) = True) Then
                tlbStpHR.Rows(14).Visible = True
                blnHRS = True
            Else
                tlbStpHR.Rows(14).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HRCPCode), intHRAR) = True) Then
                tlbStpHR.Rows(15).Visible = True
                lnkStpHR16.Text = strCareerProgTag
                blnHRS = True
            Else
                tlbStpHR.Rows(15).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HRSalScheme), intHRAR) = True) Then
                tlbStpHR.Rows(16).Visible = True
                blnHRS = True
            Else
                tlbStpHR.Rows(16).Visible = False
            End If

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HRSalGrade), intHRAR) = True) Then
                tlbStpHR.Rows(17).Visible = True
                blnHRS = True
            Else
                tlbStpHR.Rows(17).Visible = False
            End If

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HRBankFormat), intHRAR) = True) Then
                tlbStpHR.Rows(18).Visible = True
                blnHRS = True
            Else
                tlbStpHR.Rows(18).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HRBank), intHRAR) = True) Then
                tlbStpHR.Rows(19).Visible = True
                blnHRS = True
            Else
                tlbStpHR.Rows(19).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HRTaxBranch), intHRAR) = True) Then
                tlbStpHR.Rows(20).Visible = True
                blnHRS = True
            Else
                tlbStpHR.Rows(20).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HRTax), intHRAR) = True) Then
                tlbStpHR.Rows(21).Visible = True
                blnHRS = True
            Else
                tlbStpHR.Rows(21).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HRJamsostek), intHRAR) = True) Then
                tlbStpHR.Rows(22).Visible = True
                lnkStpHR23.Text = strJamsostekTag
                blnHRS = True
            Else
                tlbStpHR.Rows(22).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HRPublicHoliday), intHRAR) = True) Then
                tlbStpHR.Rows(23).Visible = True
                blnHRS = True
            Else
                tlbStpHR.Rows(23).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HRHoliday), intHRAR) = True) Then
                tlbStpHR.Rows(24).Visible = True
                blnHRS = True
            Else
                tlbStpHR.Rows(24).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HRGang), intHRAR) = True) Then
                tlbStpHR.Rows(25).Visible = True
                blnHRS = True
            Else
                tlbStpHR.Rows(25).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumHRAccessRights.HRContractorSupervision), intHRAR) = True) Then
                tlbStpHR.Rows(26).Visible = True
                blnHRS = True
            Else
                tlbStpHR.Rows(26).Visible = False
            End If
            If blnHRS = False Then
                tlbStp04.Visible = False
                tlbStpHR.Visible = False
                tblSpc19.Visible = False
            End If

        Else
            tlbStp04.Visible = False
            tlbStpHR.Visible = False
            tblSpc19.Visible = False
        End If
    End Sub

    Private Sub StpPRRight()

        Dim strLoadTag As String
        Dim strRouteTag As String
        Dim strRiceTag As String
        Dim strIncentiveTag As String
        Dim strDendaTag As String
        Dim strAirBusTag As String
        Dim strMaternityTag As String
        Dim strRelocationTag As String
        Dim strMedicalTag As String
        Dim strDanaPensiunTag As String
        Dim strEmployeeEvaluationTag As String
        Dim strStandardEvaluationTag As String
        Dim strSalaryIncreaseTag As String
        Dim strTranIncTag As String

        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.Payroll), intModuleActivate) Then

            strLoadTag = GetCaption(objLangCap.EnumLangCap.Load)
            strRouteTag = GetCaption(objLangCap.EnumLangCap.Route)
            strRiceTag = GetCaption(objLangCap.EnumLangCap.RiceRation)
            strIncentiveTag = Trim(GetCaption(objLangCap.EnumLangCap.Incentive))
            strAirBusTag = GetCaption(objLangCap.EnumLangCap.AirBusTicket)
            strMaternityTag = GetCaption(objLangCap.EnumLangCap.Maternity)
            strRelocationTag = GetCaption(objLangCap.EnumLangCap.Relocation)
            strMedicalTag = GetCaption(objLangCap.EnumLangCap.Medical)
            strDanaPensiunTag = GetCaption(objLangCap.EnumLangCap.DanaPensiun)
            strEmployeeEvaluationTag = Trim(GetCaption(objLangCap.EnumLangCap.EmployeeEvaluation))
            strStandardEvaluationTag = GetCaption(objLangCap.EnumLangCap.StandardEvaluation)
            strSalaryIncreaseTag = GetCaption(objLangCap.EnumLangCap.SalaryIncrease)
            strTranIncTag = GetCaption(objLangCap.EnumLangCap.TransportIncentive)
            strDendaTag = GetCaption(objLangCap.EnumLangCap.Denda)

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRADGroup), intPRAR) = True) Then
                tlbStpPR.Rows(0).Visible = True
                blnPRS = True
            Else
                tlbStpPR.Rows(0).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRAllowanceDeduction), intPRAR) = True) Then
                tlbStpPR.Rows(1).Visible = True
                blnPRS = True
            Else
                tlbStpPR.Rows(1).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRLoad), intPRAR) = True) Then
                tlbStpPR.Rows(2).Visible = True
                lnkStpPR03.Text = strLoadTag
                blnPRS = True
            Else
                tlbStpPR.Rows(2).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRRoute), intPRAR) = True) Then
                tlbStpPR.Rows(3).Visible = True
                lnkStpPR04.Text = strRouteTag
                blnPRS = True
            Else
                tlbStpPR.Rows(3).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRSalary), intPRAR) = True) Then
                tlbStpPR.Rows(4).Visible = True
                blnPRS = True
            Else
                tlbStpPR.Rows(4).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRAirBus), intPRAR) = True) Then
                tlbStpPR.Rows(5).Visible = True
                lnkStpPR06.Text = strAirBusTag
                blnPRS = True
            Else
                tlbStpPR.Rows(5).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRDenda), intPRAR) = True) Then
                tlbStpPR.Rows(6).Visible = True
                lnkStpPR07.Text = strDendaTag
                blnPRS = True
            Else
                tlbStpPR.Rows(6).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRHarvInc), intPRAR) = True) Then
                tlbStpPR.Rows(7).Visible = True
                blnPRS = True
            Else
                tlbStpPR.Rows(7).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRContract), intPRAR) = True) Then
                tlbStpPR.Rows(8).Visible = True
                blnPRS = True
            Else
                tlbStpPR.Rows(8).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRRice), intPRAR) = True) Then
                tlbStpPR.Rows(9).Visible = True
                lnkStpPR10.Text = strRiceTag
                blnPRS = True
            Else
                tlbStpPR.Rows(9).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRPremi), intPRAR) = True) Then
                tlbStpPR.Rows(10).Visible = True
                lnkStpPR11.Text = strIncentiveTag
                blnPRS = True
            Else
                tlbStpPR.Rows(10).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRMedical), intPRAR) = True) Then
                tlbStpPR.Rows(11).Visible = True
                lnkStpPR12.Text = strMedicalTag
                blnPRS = True
            Else
                tlbStpPR.Rows(11).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRMaternity), intPRAR) = True) Then
                tlbStpPR.Rows(12).Visible = True
                lnkStpPR13.Text = strMaternityTag
                blnPRS = True
            Else
                tlbStpPR.Rows(12).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRPaySetup), intPRAR) = True) Then
                tlbStpPR.Rows(13).Visible = True
                blnPRS = True
            Else
                tlbStpPR.Rows(13).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRPensiun), intPRAR) = True) Then
                tlbStpPR.Rows(14).Visible = True
                lnkStpPR15.Text = strDanaPensiunTag
                blnPRS = True
            Else
                tlbStpPR.Rows(14).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRPensiun), intPRAR) = True) Then
                tlbStpPR.Rows(14).Visible = True
                lnkStpPR15.Text = strDanaPensiunTag
                blnPRS = True
            Else
                tlbStpPR.Rows(14).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRRelocation), intPRAR) = True) Then
                tlbStpPR.Rows(15).Visible = True
                lnkStpPR16.Text = strRelocationTag
                blnPRS = True
            Else
                tlbStpPR.Rows(15).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PREmployeeEvaluation), intPRAR) = True) Then
                tlbStpPR.Rows(16).Visible = True
                lnkStpPR17.Text = strEmployeeEvaluationTag
                blnPRS = True
            Else
                tlbStpPR.Rows(16).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRStandardEvaluation), intPRAR) = True) Then
                tlbStpPR.Rows(17).Visible = True
                lnkStpPR18.Text = strStandardEvaluationTag
                blnPRS = True
            Else
                tlbStpPR.Rows(17).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRSalaryIncrease), intPRAR) = True) Then
                tlbStpPR.Rows(18).Visible = True
                lnkStpPR19.Text = strSalaryIncreaseTag
                blnPRS = True
            Else
                tlbStpPR.Rows(18).Visible = False
            End If

            If blnPRS = False Then
                tlbStp05.Visible = False
                tlbStpPR.Visible = False
                tblSpc20.Visible = False
            End If

        Else
            tlbStp05.Visible = False
            tlbStpPR.Visible = False
            tblSpc20.Visible = False
        End If
    End Sub

    Private Sub StpFARight()

        Dim strAssetClassTag As String
        Dim strAssetGrpTag As String
        Dim strAssetRegTag As String
        Dim strAssetReglnTag As String
        Dim strAssetPermitTag As String
        Dim strAssetMasterTag As String
        Dim strAssetItemTag As String


        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.FixAsset), intModuleActivate) Then

            strAssetClassTag = GetCaption(objLangCap.EnumLangCap.AssetClass)
            strAssetGrpTag = GetCaption(objLangCap.EnumLangCap.AssetGrp)
            strAssetRegTag = GetCaption(objLangCap.EnumLangCap.AssetRegHeader)
            strAssetReglnTag = GetCaption(objLangCap.EnumLangCap.AssetRegLine)
            strAssetPermitTag = GetCaption(objLangCap.EnumLangCap.AssetPermit)
            strAssetMasterTag = GetCaption(objLangCap.EnumLangCap.AssetMaster)
            strAssetItemTag = GetCaption(objLangCap.EnumLangCap.AssetItem)

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumFAAccessRights.FAClassSetup), intFAAR) = True) Then
                tlbStpFA.Rows(0).Visible = True
                lnkStpFA01.Text = strAssetClassTag
                blnFAS = True
            Else
                tlbStpFA.Rows(0).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumFAAccessRights.FAGroupSetup), intFAAR) = True) Then
                tlbStpFA.Rows(1).Visible = True
                lnkStpFA02.Text = strAssetGrpTag
                blnFAS = True
            Else
                tlbStpFA.Rows(1).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumFAAccessRights.FARegistrationSetup), intFAAR) = True) Then
                tlbStpFA.Rows(2).Visible = True
                lnkStpFA03.Text = strAssetRegTag
                blnFAS = True
            Else
                tlbStpFA.Rows(2).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumFAAccessRights.FARegLine), intFAAR) = True) Then
                tlbStpFA.Rows(3).Visible = True
                lnkStpFA04.Text = strAssetReglnTag
                blnFAS = True
            Else
                tlbStpFA.Rows(3).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumFAAccessRights.FAPermissionSetup), intFAAR) = True) Then
                tlbStpFA.Rows(4).Visible = True
                lnkStpFA05.Text = strAssetPermitTag
                blnFAS = True
            Else
                tlbStpFA.Rows(4).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumFAAccessRights.FAMaster), intFAAR) = True) Then
                tlbStpFA.Rows(5).Visible = True
                lnkStpFA06.Text = strAssetMasterTag
                blnFAS = True
            Else
                tlbStpFA.Rows(5).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumFAAccessRights.FAItem), intFAAR) = True) Then
                tlbStpFA.Rows(6).Visible = True
                lnkStpFA07.Text = strAssetItemTag
                blnFAS = True
            Else
                tlbStpFA.Rows(6).Visible = False
            End If

            If blnFAS = False Then
                tlbStp06.Visible = False
                tlbStpFA.Visible = False
                tblSpc21.Visible = False
            End If

        Else
            tlbStp06.Visible = False
            tlbStpFA.Visible = False
            tblSpc21.Visible = False
        End If

    End Sub

    Private Sub StpPDRight()

        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.Production), intModuleActivate) Then

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPMAccessRights.PMMasterSetup), intPMAR) = True) Then
                tlbStpPD.Rows(0).Visible = True
                lnkStpPD01.Text = "Ullage - Volume Table Master"
                blnPDS = True
            Else
                tlbStpPD.Rows(0).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPMAccessRights.PMVolConvMaster), intPMAR) = True) Then
                tlbStpPD.Rows(1).Visible = True
                lnkStpPD02.Text = "Ullage - Volume Conversion Master"
                blnPDS = True
            Else
                tlbStpPD.Rows(1).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPMAccessRights.PMAvgCapConvMaster), intPMAR) = True) Then
                tlbStpPD.Rows(2).Visible = True
                lnkStpPD03.Text = "Ullage - Average Capacity Conversion Master"
                blnPDS = True
            Else
                tlbStpPD.Rows(2).Visible = False
            End If

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPMAccessRights.PMCPOPropertyMaster), intPMAR) = True) Then
                tlbStpPD.Rows(3).Visible = True
                lnkStpPD04.Text = "CPO Properties Master"
                blnPDS = True
            Else
                tlbStpPD.Rows(3).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPMAccessRights.PMStorageTypeMaster), intPMAR) = True) Then
                tlbStpPD.Rows(4).Visible = True
                lnkStpPD05.Text = "Storage Type Master"
                blnPDS = True
            Else
                tlbStpPD.Rows(4).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPMAccessRights.PMStorageAreaMaster), intPMAR) = True) Then
                tlbStpPD.Rows(5).Visible = True
                lnkStpPD06.Text = "Storage Area Master"
                blnPDS = True
            Else
                tlbStpPD.Rows(5).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPMAccessRights.PMProcessingLineMaster), intPMAR) = True) Then
                tlbStpPD.Rows(6).Visible = True
                lnkStpPD07.Text = "Processing Line Master"
                blnPDS = True
            Else
                tlbStpPD.Rows(6).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPMAccessRights.PMMachineMaster), intPMAR) = True) Then
                tlbStpPD.Rows(7).Visible = True
                lnkStpPD08.Text = "Machine Master"
                blnPDS = True
            Else
                tlbStpPD.Rows(7).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPMAccessRights.PMAcceptableOilQuality), intPMAR) = True) Then
                tlbStpPD.Rows(8).Visible = True
                lnkStpPD09.Text = "Acceptable Oil Quality"
                blnPDS = True
            Else
                tlbStpPD.Rows(8).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPMAccessRights.PMAcceptableKernelQuality), intPMAR) = True) Then
                tlbStpPD.Rows(9).Visible = True
                lnkStpPD10.Text = "Acceptable Kernel Quality"
                blnPDS = True
            Else
                tlbStpPD.Rows(9).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPMAccessRights.PMTestSample), intPMAR) = True) Then
                tlbStpPD.Rows(10).Visible = True
                lnkStpPD11.Text = "Test Sample"
                blnPDS = True
            Else
                tlbStpPD.Rows(10).Visible = False
            End If

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPMAccessRights.PMHarvestingInterval), intPMAR) = True) Then
                tlbStpPD.Rows(11).Visible = True
                lnkStpPD12.Text = "Harvesting Interval"
                blnPDS = True
            Else
                tlbStpPD.Rows(11).Visible = False
            End If

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPMAccessRights.PMMachineCriteria), intPMAR) = True) Then
                tlbStpPD.Rows(12).Visible = True
                lnkStpPD13.Text = "Machine Criteria"
                blnPDS = True
            Else
                tlbStpPD.Rows(12).Visible = False
            End If

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPMAccessRights.PMMill), intPMAR) = True) Then
                tlbStpPD.Rows(13).Visible = True
                lnkStpPD14.Text = "Mill"
                blnPDS = True
            Else
                tlbStpPD.Rows(13).Visible = False
            End If

            If blnPDS = False Then
                tlbStp07.Visible = False
                tlbStpPD.Visible = False
                tblSpc22.Visible = False
            End If

        Else
            tlbStp07.Visible = False
            tlbStpPD.Visible = False
            tblSpc22.Visible = False
        End If

    End Sub


    Private Sub StpNURight()

        Dim strNurseryBatchTag As String
        Dim strCullingTypeTag As String
        Dim strAccClassification As String

        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.Nursery), intModuleActivate) Then

            strNurseryBatchTag = GetCaption(objLangCap.EnumLangCap.NurseryBatch)
            strCullingTypeTag = GetCaption(objLangCap.EnumLangCap.CullType)
            strAccClassification = GetCaption(objLangCap.EnumLangCap.AccountDistribution)

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumNUAccessRights.NUMasterItem), intNUAR) = True) Then
                tlbStpNU.Rows(0).Visible = True
                lnkStpNU01.Text = "Nursery Master"
                blnNUS = True
            Else
                tlbStpNU.Rows(0).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumNUAccessRights.NUItem), intNUAR) = True) Then
                tlbStpNU.Rows(1).Visible = True
                lnkStpNU02.Text = "Nursery Item"
                blnNUS = True
            Else
                tlbStpNU.Rows(1).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumNUAccessRights.NUMasterSetup), intNUAR) = True) Then
                tlbStpNU.Rows(2).Visible = True
                lnkStpNU03.Text = strNurseryBatchTag
                blnNUS = True
            Else
                tlbStpNU.Rows(2).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumNUAccessRights.NUCullType), intNUAR) = True) Then
                tlbStpNU.Rows(3).Visible = True
                lnkStpNU04.Text = strCullingTypeTag
                blnNUS = True
            Else
                tlbStpNU.Rows(3).Visible = False
            End If
            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumNUAccessRights.NUWorkAccDist), intNUAR) = True) Then
                tlbStpNU.Rows(4).Visible = True
                lnkStpNU05.Text = strAccClassification
                blnNUS = True
            Else
                tlbStpNU.Rows(4).Visible = False
            End If

            If blnNUS = False Then
                tblSpc23.Visible = False
                tlbStp08.Visible = False
                tlbStpNU.Visible = False
            End If

        Else
            tblSpc23.Visible = False
            tlbStp08.Visible = False
            tlbStpNU.Visible = False
        End If

    End Sub

    Private Sub StpWSRight()

        Dim ProdTypeTag As String
        Dim ProdBrandTag As String
        Dim ProdModelTag As String
        Dim ProdCatTag As String
        Dim ProdMatTag As String
        Dim StockAnalTag As String
        Dim WorkTag As String
        Dim DirectChgMasterTag As String
        Dim DirectChgItemTag As String

        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.Workshop), intModuleActivate) Then

            ProdTypeTag = GetCaption(objLangCap.EnumLangCap.ProdType)
            ProdBrandTag = GetCaption(objLangCap.EnumLangCap.ProdBrand)
            ProdModelTag = GetCaption(objLangCap.EnumLangCap.ProdModel)
            ProdCatTag = GetCaption(objLangCap.EnumLangCap.ProdCat)
            ProdMatTag = GetCaption(objLangCap.EnumLangCap.ProdMat)
            StockAnalTag = GetCaption(objLangCap.EnumLangCap.StockAnalysis)
            WorkTag = GetCaption(objLangCap.EnumLangCap.Work)
            DirectChgMasterTag = GetCaption(objLangCap.EnumLangCap.DirectChgMaster)
            DirectChgItemTag = GetCaption(objLangCap.EnumLangCap.DirectChgItem)

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumWSAccessRights.WSProductMaster), intWSAR) = True) Then
                tlbStpWS.Rows(0).Visible = True
                lnkStpWS01.Text = ProdTypeTag
                blnWSS = True
            Else
                tlbStpWS.Rows(0).Visible = False
            End If

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumWSAccessRights.WSProductBrand), intWSAR) = True) Then
                tlbStpWS.Rows(1).Visible = True
                lnkStpWS02.Text = ProdBrandTag
                blnWSS = True
            Else
                tlbStpWS.Rows(1).Visible = False
            End If

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumWSAccessRights.WSProductModel), intWSAR) = True) Then
                tlbStpWS.Rows(2).Visible = True
                lnkStpWS03.Text = ProdModelTag
                blnWSS = True
            Else
                tlbStpWS.Rows(2).Visible = False
            End If

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumWSAccessRights.WSProductCategory), intWSAR) = True) Then
                tlbStpWS.Rows(3).Visible = True
                lnkStpWS04.Text = ProdCatTag
                blnWSS = True
            Else
                tlbStpWS.Rows(3).Visible = False
            End If

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumWSAccessRights.WSProductMaterial), intWSAR) = True) Then
                tlbStpWS.Rows(4).Visible = True
                lnkStpWS05.Text = ProdMatTag
                blnWSS = True
            Else
                tlbStpWS.Rows(4).Visible = False
            End If

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumWSAccessRights.WSAnalisisStok), intWSAR) = True) Then
                tlbStpWS.Rows(5).Visible = True
                lnkStpWS06.Text = StockAnalTag
                blnWSS = True
            Else
                tlbStpWS.Rows(5).Visible = False
            End If

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumWSAccessRights.WSWorkMaster), intWSAR) = True) Then
                tlbStpWS.Rows(6).Visible = True
                lnkStpWS07.Text = WorkTag & " Code"
                blnWSS = True
            Else
                tlbStpWS.Rows(6).Visible = False
            End If

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumWSAccessRights.WSWorkshopService), intWSAR) = True) Then
                tlbStpWS.Rows(7).Visible = True
                lnkStpWS08.Text = "Service Type"
                blnWSS = True
            Else
                tlbStpWS.Rows(7).Visible = False
            End If

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumWSAccessRights.WSDirectChargeItem), intWSAR) = True) Then
                tlbStpWS.Rows(8).Visible = True
                lnkStpWS09.Text = DirectChgItemTag
                blnWSS = True
            Else
                tlbStpWS.Rows(8).Visible = False
            End If

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumWSAccessRights.WSDirectChargeItemMaster), intWSAR) = True) Then
                tlbStpWS.Rows(9).Visible = True
                lnkStpWS10.Text = DirectChgMasterTag
                blnWSS = True
            Else
                tlbStpWS.Rows(9).Visible = False
            End If

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumWSAccessRights.WSItemPart), intWSAR) = True) Then
                tlbStpWS.Rows(10).Visible = True
                lnkStpWS11.Text = "Workshop Item Part"
                blnWSS = True
            Else
                tlbStpWS.Rows(10).Visible = False
            End If

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumWSAccessRights.WSMillProcDistr), intWSAR) = True) Then
                tlbStpWS.Rows(11).Visible = True
                lnkStpWS12.Text = "Monthly Mill Process Distribution"
                blnWSS = True
            Else
                tlbStpWS.Rows(11).Visible = False
            End If

            If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumWSAccessRights.WSMillProcDistr), intWSAR) = True) Then
                tlbStpWS.Rows(12).Visible = True
                lnkStpWS13.Text = "Calendarized Machine"
                blnWSS = True
            Else
                tlbStpWS.Rows(12).Visible = False
            End If

            If blnWSS = False Then
                tblSpc24.Visible = False
                tlbStp09.Visible = False
                tlbStpWS.Visible = False
            End If

        Else
            tblSpc24.Visible = False
            tlbStp09.Visible = False
            tlbStpWS.Visible = False
        End If

    End Sub



    Private Sub ReportsRight()

        Dim blnReports As Boolean
        blnReports = False

        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.GeneralLedger), intModuleActivate) Then
            lnkRpt1.NavigateUrl = "/" & strLangCode & "/reports/GL_StdRpt_Selection.aspx?UserLoc=" & strLocation
            tlbRpt.Rows(0).Visible = True
            blnReports = True
        Else
            lnkRpt1.NavigateUrl = ""
            tlbRpt.Rows(0).Visible = False
        End If
        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.Billing), intModuleActivate) Then
            lnkRpt2.NavigateUrl = "/" & strLangCode & "/reports/AR_StdRpt_Selection.aspx?UserLoc=" & strLocation
            tlbRpt.Rows(1).Visible = True
            blnReports = True
        Else
            lnkRpt2.NavigateUrl = ""
            tlbRpt.Rows(1).Visible = False
        End If
        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.AccountPayable), intModuleActivate) Then
            lnkRpt3.NavigateUrl = "/" & strLangCode & "/reports/AP_StdRpt_Selection.aspx?UserLoc=" & strLocation
            tlbRpt.Rows(2).Visible = True
            blnReports = True
        Else
            lnkRpt3.NavigateUrl = ""
            tlbRpt.Rows(2).Visible = False
        End If
        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.CashAndBank), intModuleActivate) Then
            lnkRpt4.NavigateUrl = "/" & strLangCode & "/reports/CB_StdRpt_Selection.aspx?UserLoc=" & strLocation
            tlbRpt.Rows(3).Visible = True
            blnReports = True
        Else
            lnkRpt4.NavigateUrl = ""
            tlbRpt.Rows(3).Visible = False
        End If
        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.Purchasing), intModuleActivate) Then
            lnkRpt5.NavigateUrl = "/" & strLangCode & "/reports/PU_StdRpt_Selection.aspx?UserLoc=" & strLocation
            tlbRpt.Rows(4).Visible = True
            blnReports = True
        Else
            lnkRpt5.NavigateUrl = ""
            tlbRpt.Rows(4).Visible = False
        End If
        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.Inventory), intModuleActivate) Then
            lnkRpt6.NavigateUrl = "/" & strLangCode & "/reports/IN_StdRpt_Selection.aspx?UserLoc=" & strLocation
            tlbRpt.Rows(5).Visible = True
            blnReports = True
        Else
            lnkRpt6.NavigateUrl = ""
            tlbRpt.Rows(5).Visible = False
        End If
        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.HumanResource), intModuleActivate) Then
            lnkRpt7.NavigateUrl = "/" & strLangCode & "/reports/HR_StdRpt_Selection.aspx?UserLoc=" & strLocation
            tlbRpt.Rows(6).Visible = True
            blnReports = True
        Else
            lnkRpt7.NavigateUrl = ""
            tlbRpt.Rows(6).Visible = False
        End If
        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.Payroll), intModuleActivate) Then
            lnkRpt8.NavigateUrl = "/" & strLangCode & "/reports/PR_StdRpt_Selection.aspx?UserLoc=" & strLocation
            tlbRpt.Rows(7).Visible = True
            blnReports = True
        Else
            lnkRpt8.NavigateUrl = ""
            tlbRpt.Rows(7).Visible = False
        End If
        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.Production), intModuleActivate) Then


            'lnkRpt9.NavigateUrl = "/" & strLangCode & "/reports/PD_StdRpt_Selection.aspx?UserLoc=" & strLocation
            If intLocType = 4 Then
                lnkRpt9.NavigateUrl = "/" & strLangCode & "/reports/PM_StdRpt_Selection.aspx?UserLoc=" & strLocation
            Else
                lnkRpt9.NavigateUrl = "/" & strLangCode & "/reports/PD_StdRpt_Selection.aspx?UserLoc=" & strLocation
            End If
            ''lnkRpt9.NavigateUrl = "/" & strLangCode & "/reports/PD_StdRpt_Selection.aspx?UserLoc=" & strLocation
            tlbRpt.Rows(8).Visible = True
            blnReports = True
        Else
            lnkRpt9.NavigateUrl = ""
            tlbRpt.Rows(8).Visible = False
        End If
        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.MillWeighing), intModuleActivate) Then
            lnkRpt10.NavigateUrl = "/" & strLangCode & "/reports/WM_StdRpt_Selection.aspx?UserLoc=" & strLocation
            tlbRpt.Rows(9).Visible = True
            blnReports = True
        Else
            lnkRpt10.NavigateUrl = ""
            tlbRpt.Rows(9).Visible = False
        End If
        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.MillContract), intModuleActivate) Then
            lnkRpt11.NavigateUrl = "/" & strLangCode & "/reports/CM_StdRpt_Selection.aspx?UserLoc=" & strLocation
            tlbRpt.Rows(10).Visible = True
            blnReports = True
        Else
            lnkRpt11.NavigateUrl = ""
            tlbRpt.Rows(10).Visible = False
        End If
        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.Canteen), intModuleActivate) Then
            lnkRpt12.NavigateUrl = "/" & strLangCode & "/reports/CT_StdRpt_Selection.aspx?UserLoc=" & strLocation
            tlbRpt.Rows(11).Visible = True
            blnReports = True
        Else
            lnkRpt12.NavigateUrl = ""
            tlbRpt.Rows(11).Visible = False
        End If
        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.Workshop), intModuleActivate) Then
            lnkRpt13.NavigateUrl = "/" & strLangCode & "/reports/WS_StdRpt_Selection.aspx?UserLoc=" & strLocation
            tlbRpt.Rows(12).Visible = True
            blnReports = True
        Else
            lnkRpt13.NavigateUrl = ""
            tlbRpt.Rows(12).Visible = False
        End If
        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.Nursery), intModuleActivate) Then
            lnkRpt14.NavigateUrl = "/" & strLangCode & "/reports/NU_StdRpt_Selection.aspx?UserLoc=" & strLocation
            tlbRpt.Rows(13).Visible = True
            blnReports = True
        Else
            lnkRpt14.NavigateUrl = ""
            tlbRpt.Rows(13).Visible = False
        End If
        If objAR.mtdHasAccessRights(objAR.mtdGetModuleActivation(objAR.EnumModuleActivation.FixAsset), intModuleActivate) Then
            lnkRpt15.NavigateUrl = "/" & strLangCode & "/reports/FA_StdRpt_Selection.aspx?UserLoc=" & strLocation
            tlbRpt.Rows(14).Visible = True
            blnReports = True
        Else
            lnkRpt15.NavigateUrl = ""
            tlbRpt.Rows(14).Visible = False
        End If

        If blnReports = False Then
            tblSpc25.Visible = False
            tblGrpRpt.Visible = False
            tlbRpt.Visible = False
        End If


    End Sub

    Private Sub MonthEndRight()

        Dim blnMthEnd As Boolean
        blnMthEnd = False

        If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumINAccessRights.INMonthEnd), intINAR) = True) Then
            lnkMth1.NavigateUrl = "/" & strLangCode & "/IN/mthend/IN_MthEnd_Process.aspx"
            tblMth.Rows(0).Visible = True
            blnMthEnd = True
        Else
            lnkMth1.NavigateUrl = ""
            tblMth.Rows(0).Visible = False
        End If

        If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumWSAccessRights.WSMonthEnd), intWSAR) = True) Then
            lnkMth2.NavigateUrl = "/" & strLangCode & "/WS/mthend/WS_MthEnd_Process.aspx"
            tblMth.Rows(1).Visible = True
            blnMthEnd = True
        Else
            lnkMth2.NavigateUrl = ""
            tblMth.Rows(1).Visible = False
        End If

        If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCTAccessRights.CTMonthEnd), intCTAR) = True) Then
            lnkMth3.NavigateUrl = "/" & strLangCode & "/CT/mthend/CT_MthEnd_Process.aspx"
            tblMth.Rows(2).Visible = True
            blnMthEnd = True
        Else
            lnkMth3.NavigateUrl = ""
            tblMth.Rows(2).Visible = False
        End If

        If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPMAccessRights.PMMonthEnd), intPMAR) = True) Or _
            (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPDAccessRights.PDMonthEnd), intPDAR) = True) Then
            lnkMth4.NavigateUrl = "/" & strLangCode & "/PD/mthend/PD_MthEnd_Process.aspx"
            tblMth.Rows(3).Visible = True
            blnMthEnd = True
        Else
            lnkMth4.NavigateUrl = ""
            tblMth.Rows(3).Visible = False
        End If

        If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumNUAccessRights.NUMonthEnd), intNUAR) = True) Then
            lnkMth5.NavigateUrl = "/" & strLangCode & "/NU/mthend/NU_MthEnd_Process.aspx"
            tblMth.Rows(4).Visible = True
            blnMthEnd = True
        Else
            lnkMth5.NavigateUrl = ""
            tblMth.Rows(4).Visible = False
        End If

        If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPUAccessRights.PUMonthEnd), intPUAR) = True) Then
            lnkMth6.NavigateUrl = "/" & strLangCode & "/PU/mthend/PU_MthEnd_Process.aspx"
            tblMth.Rows(5).Visible = True
            blnMthEnd = True
        Else
            lnkMth6.NavigateUrl = ""
            tblMth.Rows(5).Visible = False
        End If

        If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumFAAccessRights.FAGenDepreciation), intFAAR) = True) Or _
            (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumFAAccessRights.FAMonthEnd), intFAAR) = True) Then
            lnkMth7.NavigateUrl = "/" & strLangCode & "/menu/menu_FAMthEnd_page.aspx"
            tblMth.Rows(6).Visible = True
            blnMthEnd = True
        Else
            lnkMth7.NavigateUrl = ""
            tblMth.Rows(6).Visible = False
        End If

        If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumAPAccessRights.APMonthEnd), intAPAR) = True) Then
            lnkMth8.NavigateUrl = "/" & strLangCode & "/AP/mthend/AP_MthEnd_Process.aspx"
            tblMth.Rows(7).Visible = True
            blnMthEnd = True
        Else
            lnkMth8.NavigateUrl = ""
            tblMth.Rows(7).Visible = False
        End If

        If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumBIAccessRights.BIMonthEnd), intBIAR) = True) Then
            lnkMth9.NavigateUrl = "/" & strLangCode & "/BI/mthend/BI_MthEnd_Process.aspx"
            tblMth.Rows(8).Visible = True
            blnMthEnd = True
        Else
            lnkMth9.NavigateUrl = ""
            tblMth.Rows(8).Visible = False
        End If

        If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCBAccessRights.CBMthEnd), intCBAR) = True) Then
            lnkMth10.NavigateUrl = "/" & strLangCode & "/CB/mthend/CB_MthEnd_Process.aspx"
            tblMth.Rows(9).Visible = True
            blnMthEnd = True
        Else
            lnkMth10.NavigateUrl = ""
            tblMth.Rows(9).Visible = False
        End If

        If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRMonthEnd), intPRAR) = True) Or _
           (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRMthRice), intPRAR) = True) Or _
           (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRMthRapel), intPRAR) = True) Or _
           (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRMthBonus), intPRAR) = True) Or _
           (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRMthTHR), intPRAR) = True) Or _
           (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRMthDaily), intPRAR) = True) Or _
           (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRMthPayroll), intPRAR) = True) Or _
           (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumPRAccessRights.PRMthTransfer), intPRAR) = True) Then

            lnkMth11.NavigateUrl = "/" & strLangCode & "/menu/menu_PRMthEnd_page.aspx"
            tblMth.Rows(10).Visible = True
            blnMthEnd = True
        Else
            lnkMth11.NavigateUrl = ""
            tblMth.Rows(10).Visible = False
        End If

        If (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumGLAccessRights.GLMthEndGCDist), intGLAR) = True) Or _
           (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumGLAccessRights.GLJrnMonthEnd), intGLAR) = True) Or _
           (objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumGLAccessRights.GLMonthEnd), intGLAR) = True) Then

            lnkMth12.NavigateUrl = "/" & strLangCode & "/menu/menu_GLMthEnd_page.aspx"
            tblMth.Rows(11).Visible = True
            blnMthEnd = True

        Else

            lnkMth12.NavigateUrl = ""
            tblMth.Rows(11).Visible = False

        End If

        If blnMthEnd = False Then
            tblSpc26.Visible = False
            tblGrpMthEnd.Visible = False
            tblMth.Visible = False
        End If

    End Sub


End Class
