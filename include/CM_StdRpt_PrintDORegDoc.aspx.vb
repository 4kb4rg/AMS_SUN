Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports Microsoft.VisualBasic.Strings
Imports System.Xml
Imports System.Web.Services
Imports Microsoft.VisualBasic.Information
Imports Microsoft.VisualBasic.Interaction
Imports Microsoft.VisualBasic

Public Class CM_StdRpt_PrintDORegDoc : Inherits Page

    Protected RptSelect As UserControl

    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl
    Dim objSysCfg As New agri.PWSystem.clsConfig
    Dim objLangCap As New agri.PWSystem.clsLangCap
    Dim objWMTrx As New agri.WM.clsTrx
    Dim objAdminLoc As New agri.Admin.clsLoc
    Dim objCMTrx As New agri.CM.clsTrx

    Protected WithEvents lblDate As Label
    Protected WithEvents lblDateFormat As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblTracker As Label
    Protected WithEvents LocTag As Label
    Protected WithEvents lblAccount As Label
    Protected WithEvents lblBlock As Label
    Protected WithEvents lblBillParty As Label
    Protected WithEvents PrintPrev As ImageButton

    Protected WithEvents ddlDONo1 As DropDownList
    Protected WithEvents lblErrDONo1 As Label
    
    Protected WithEvents txtTTD As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtJabatan As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPacking As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPenyerahan As System.Web.UI.WebControls.TextBox
    Protected WithEvents ChkPPN As CheckBox


    Dim objLangCapDs As New Object
    Dim strCompany As String
    Dim strCompanyName As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strUserName As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intErrNo As Integer
    Protected WithEvents rfvTTD As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rfvJabatan As System.Web.UI.WebControls.RequiredFieldValidator

    Dim strLocType As String

    Dim objContractDs as New Object()

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strCompanyName = Session("SS_COMPANYNAME")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strUserName = Session("SS_USERNAME")
        strAccMonth = Session("SS_ARACCMONTH")
        strAccYear = Session("SS_ARACCYEAR")
        strLangCode = Session("SS_LANGCODE")
        strLocType = Session("SS_LOCTYPE")
        lblDate.Visible = False
        lblDateFormat.Visible = False

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        Else
            If Not Page.IsPostBack Then
                onload_GetLangCap()
                BindDONo1()
            End If
        End If
    End Sub

    Protected Overloads Sub OnPreRender(ByVal Source As Object, ByVal E As EventArgs) Handles MyBase.PreRender
        Dim SDecimal As HtmlTableRow
        Dim SLocation As HtmlTableRow

        SDecimal = RptSelect.FindControl("SelDecimal")
        SLocation = RptSelect.FindControl("SelLocation")
        SDecimal.Visible = True
        SLocation.Visible = True
    End Sub



    Sub BindDONo1()
        Dim strOpCd_GET As String = "CM_CLSTRX_DO_REG_GET"
        Dim strSearch As String = ""
        Dim strSort As String = ""
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer
        
        Dim dr As DataRow

        strSearch = "and cdo.status in ('1', '4')"
        strSort = "order by cdo.dono asc "
        strParam = strSearch & "|" & strSort

        Try
            intErrNo = objCMTrx.mtdGetDOReg(strOpCd_GET, strParam, 0, objContractDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_DOREGLIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Setup/CM_StdRpt_PrintDORegDoc.aspx")
        End Try

            For intCnt = 0 To objContractDs.Tables(0).Rows.Count - 1
                objContractDs.Tables(0).Rows(intCnt).Item("DONo") = Trim(objContractDs.Tables(0).Rows(intCnt).Item("DONo"))
            Next intCnt

            dr = objContractDs.Tables(0).NewRow()
            dr("DONo") = ""
            dr("DONo") = "Select DO Registration No"
            objContractDs.Tables(0).Rows.InsertAt(dr, 0)

            ddlDONo1.DataSource = objContractDs.Tables(0)
            ddlDONo1.DataValueField = "DONo"
            ddlDONo1.DataTextField = "DONo"
            ddlDONo1.DataBind()
            ddlDONo1.SelectedIndex = intSelectedIndex
            

    End Sub

    Sub onChanged_DONo(ByVal Sender As Object, ByVal E As EventArgs)
        Dim strOpCd As String = "CM_CLSTRX_DO_REG_GET"
        Dim strOpCd_GetMatch As String = "CM_CLSTRX_DO_REG_GET_QTYMATCH"
        Dim strParam As String
        Dim strParamCheckMatchStatus As String
        Dim intErrNo As Integer
        Dim strSearch As String
        Dim strBalQty As String
        Dim pv_strDONo as String = ddlDONo1.SelectedItem.Value

        strSearch = "and cdo.LocCode = '" & strLocation & "' and cdo.DONo = '" & pv_strDONo & "' "
        strParam = strSearch & "|" & ""

        Try
            intErrNo = objCMTrx.mtdGetDOReg(strOpCd, strParam, 0, objContractDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegList.aspx")
        End Try


        If objContractDs.Tables(0).Rows.count > 0 Then
            txtTTD.Text = Trim(objContractDs.Tables(0).Rows(0).Item("TTD"))
            txtJabatan.Text = Trim(objContractDs.Tables(0).Rows(0).Item("Jabatan"))
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
            Response.Redirect("../../include/mesg/ErrorMessage.aspx?errcode=PWSYSTEM_CLSLANGCAP_BUSSTERM_GET&errmesg=" & lblErrMessage.Text & "&redirect=../en/reports/CM_StdRpt_Selection.aspx")
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

    Sub btnPrintPrev_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim strRptID As String
        Dim strRptName As String
        Dim strUserLoc As String
        Dim strDec As String
        Dim strTTD As String
        Dim strDONo1 As String
        Dim strJabatan as String
        Dim tempRpt As DropDownList
        Dim tempDec As DropDownList
        Dim templblUL As Label
        Dim tempUserLoc As HtmlInputHidden
        Dim strPacking As String
        Dim strPenyerahan As String
        Dim strPPN As String

        tempUserLoc = RptSelect.FindControl("hidUserLoc")
        strUserLoc = Trim(tempUserLoc.Value)
        tempDec = RptSelect.FindControl("lstDecimal")
        strDec = Trim(tempDec.SelectedItem.Value)

        If strUserLoc = "" Then
            templblUL = RptSelect.FindControl("lblUserLoc")
            templblUL.Visible = True
            Exit Sub
        Else
            If Left(strUserLoc, 3) = "','" Then
                strUserLoc = Right(strUserLoc, Len(strUserLoc) - 3)
            ElseIf Right(strUserLoc, 3) = "','" Then
                strUserLoc = Left(strUserLoc, Len(strUserLoc) - 3)
            End If
        End If

        tempRpt = RptSelect.FindControl("lstRptName")
        strRptID = Trim(tempRpt.SelectedItem.Value)
        strRptName = Trim(tempRpt.SelectedItem.Text)

        strTTD = Server.UrlEncode(txtTTD.Text.Trim())
        strPPN = IIf(ChkPPN.Checked = True, "Yes", "No")
        
        strDONo1 = ddlDONo1.SelectedItem.Value
        IF strDONo1 = "Select DO Registration No" Then
            lblErrDONo1.Visible = True
            Exit Sub
        Else
            lblErrDONo1.Visible = False
        End If

        strJabatan = Server.UrlEnCode(txtJabatan.Text.Trim())
        strPacking = Server.UrlEncode(txtPacking.Text.Trim())
        strPenyerahan = Server.UrlEncode(txtPenyerahan.Text.Trim())
        


        Response.Write("<Script Language=""JavaScript"">window.open(""CM_StdRpt_PrintDORegDocPreview.aspx?Location=" & strUserLoc & _
                        "&Decimal=" & strDec & _
                        "&TTD=" & strTTD & _
                        "&DONo1=" & strDONo1 & _
                        "&DONo2=" & strDONo1 & _
                        "&Jabatan=" & strJabatan & _
                        "&Packing=" & strPacking & _
                        "&Penyerahan=" & strPenyerahan & _
                        "&PPN=" & strPPN & _
                        """,null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")

    End Sub

    Sub PPN_Type(ByVal Sender As Object, ByVal E As EventArgs)
        If ChkPPN.Checked = True Then
            ChkPPN.Text = "  Yes"
        Else
            ChkPPN.Text = "  No"
        End If
    End Sub
End Class
 
