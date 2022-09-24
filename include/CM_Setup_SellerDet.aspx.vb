Imports System
Imports System.Data
Imports System.IO 
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports Microsoft.VisualBasic.Information
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic.Interaction

Imports agri.GlobalHdl.clsGlobalHdl



Public Class CM_Setup_SellerDet : Inherits Page

    Protected WithEvents txtSellerCode As Textbox
    Protected WithEvents txtDescription As Textbox
    Protected WithEvents txtAddress As HtmlTextArea
    Protected WithEvents txtTelNo As Textbox
    Protected WithEvents lblStatus As Label
    Protected WithEvents lblDateCreated As Label
    Protected WithEvents lblLastUpdate As Label
    Protected WithEvents lblUpdatedBy As Label

    Protected WithEvents tbcode As HtmlInputHidden
    Protected WithEvents SaveBtn As ImageButton
    Protected WithEvents DelBtn As ImageButton
    Protected WithEvents UnDelBtn As ImageButton

    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblHiddenSts As Label
    Protected WithEvents lblErrDup As Label
    Protected WithEvents lblErrAddress As Label

    Dim objCMSetup As New agri.CM.clsSetup()
    Dim objAdmin As New agri.Admin.clsUOM()
    Dim objSys As New agri.PWSystem.clsConfig()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()

    Dim objSellerDs As New Object()
    Dim objCheckDs As New Object()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim intCMAR As Integer

    Dim strSellerCode As String = ""
    Dim intStatus As Integer
    Dim intMaxLen As Integer = 0

    Sub Page_Load(Sender As Object, E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_ARACCMONTH")
        strAccYear = Session("SS_ARACCYEAR")
        intCMAR = Session("SS_CMAR")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCMAccessRights.CMMasterSetup), intCMAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            lblErrDup.Visible = False
            strSellerCode = Trim(IIf(Request.QueryString("tbcode") <> "", Request.QueryString("tbcode"), Request.Form("tbcode")))
            intStatus = CInt(lblHiddenSts.Text)
            If Not IsPostBack Then
                If strSellerCode <> "" Then
                    tbcode.Value = strSellerCode
                    onLoad_Display()
                    onLoad_BindButton()
                Else
                    onLoad_BindButton()
                End If
            End If
        End If
    End Sub


    Sub onLoad_Display()
        Dim strOpCd As String = "CM_CLSSETUP_SELLER_GET"
        Dim strParam As String        
        Dim intErrNo As Integer
        Dim strSearch As String
        
        strSearch = "and seller.SellerCode = '" & strSellerCode & "' "
        strParam = strSearch & "|" & ""

        Try
            intErrNo = objCMSetup.mtdGetMasterList(strOpCd, strParam, 0, objSellerDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_SETUP_SELLERDET_GET&errmesg=" & lblErrMessage.Text & "&redirect=cm/setup/cm_setup_sellerlist.aspx")
        End Try

        txtSellerCode.Text = Trim(objSellerDs.Tables(0).Rows(0).Item("SellerCode"))
        txtDescription.Text = Trim(objSellerDs.Tables(0).Rows(0).Item("Name"))
        txtAddress.value = Trim(objSellerDs.Tables(0).Rows(0).Item("Address"))
        txtTelNo.Text = Trim(objSellerDs.Tables(0).Rows(0).Item("TelNo"))
        intStatus = CInt(Trim(objSellerDs.Tables(0).Rows(0).Item("Status")))
        lblHiddenSts.Text = Trim(objSellerDs.Tables(0).Rows(0).Item("Status"))
        lblStatus.Text = objCMSetup.mtdGetSellerStatus(Trim(objSellerDs.Tables(0).Rows(0).Item("Status")))
        lblDateCreated.Text = objGlobal.GetLongDate(objSellerDs.Tables(0).Rows(0).Item("CreateDate"))
        lblLastUpdate.Text = objGlobal.GetLongDate(objSellerDs.Tables(0).Rows(0).Item("UpdateDate"))
        lblUpdatedBy.Text = Trim(objSellerDs.Tables(0).Rows(0).Item("UserName"))

    End Sub


    Sub onLoad_BindButton()
        txtSellerCode.Enabled = False
        txtDescription.Enabled = False
        txtAddress.Disabled = True
        txtTelNo.Enabled = False
        SaveBtn.Visible = False
        DelBtn.Visible = False
        UnDelBtn.Visible = False

        Select Case intStatus
            Case objCMSetup.EnumSellerStatus.Active
                txtDescription.Enabled = True
                txtAddress.Disabled = False
                txtTelNo.Enabled = True
                SaveBtn.Visible = True
                DelBtn.Visible = True
                DelBtn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
            Case objCMSetup.EnumSellerStatus.Deleted
                UnDelBtn.Visible = True
            Case Else
                txtSellerCode.Enabled = True
                txtDescription.Enabled = True
                txtAddress.Disabled = False
                txtTelNo.Enabled = True
                SaveBtn.Visible = True
        End Select        
    End Sub

    Sub Button_Click(Sender As Object, E As ImageClickEventArgs)
        Dim strOpCd_Upd As String = "CM_CLSSETUP_SELLER_UPD"
        Dim strOpCd_Get As String = "CM_CLSSETUP_SELLER_GET"
        Dim strOpCd_Add As String = "CM_CLSSETUP_SELLER_ADD"

        Dim strCmdArgs As String = CType(Sender, ImageButton).CommandArgument        
        Dim intErrNo As Integer
        Dim blnIsUpdate As Boolean
        Dim strParam As String = ""
        Dim strSellerName As String
        Dim strAddress As String
        Dim strTelNo As String
        Dim blnDupKey As Boolean

        If strCmdArgs = "Save" Then 
            strSellerCode = Trim(txtSellerCode.text)
            strSellerName = Trim(txtDescription.text)
            strAddress = Trim(txtAddress.value)
            strTelNo = Trim(txtTelNo.text)
    
            blnIsUpdate = IIf(intStatus = 0, False, True)
            tbcode.Value = strSellerCode  
        
            strParam = strSellerCode & Chr(9) & _
                        strSellerName & Chr(9) & _
                        strAddress & Chr(9) & _
                        strTelNo & Chr(9) & _
                        objCMSetup.EnumSellerStatus.Active
            Try
                intErrNo = objCMSetup.mtdUpdSeller(strOpCd_Get, _
                                                   strOpCd_Add, _
                                                   strOpCd_Upd, _
                                                   strCompany, _
                                                   strLocation, _
                                                   strUserId, _
                                                   strParam, _
                                                   blnDupKey, _
                                                   blnIsUpdate)

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_SETUP_SELLERDET_SAVE&errmesg=" & lblErrMessage.Text & "&redirect=cm/setup/cm_setup_sellerDet.aspx?tbcode=" & strSellerCode)
            End Try

            If blnDupKey = True Then
                lblErrDup.Visible = True
                Exit Sub
            End If

        ElseIf strCmdArgs = "Del" Then
            strParam = strSellerCode & Chr(9) & "" & Chr(9) & "" & Chr(9) & "" & Chr(9) & objCMSetup.EnumSellerStatus.Deleted
            Try
                intErrNo = objCMSetup.mtdUpdSeller(strOpCd_Get, _
                                                   strOpCd_Add, _
                                                   strOpCd_Upd, _
                                                   strCompany, _
                                                   strLocation, _
                                                   strUserId, _
                                                   strParam, _
                                                   False, _
                                                   True)

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_SETUP_SELLERDET_DEL&errmesg=" & lblErrMessage.Text & "&redirect=cm/setup/cm_setup_sellerDet.aspx?tbcode=" & strSellerCode)
            End Try
        ElseIf strCmdArgs = "UnDel" Then
            strParam = strSellerCode & Chr(9) & "" & Chr(9) & "" & Chr(9) & "" & Chr(9) & objCMSetup.EnumSellerStatus.Active
            Try
                intErrNo = objCMSetup.mtdUpdSeller(strOpCd_Get, _
                                                   strOpCd_Add, _
                                                   strOpCd_Upd, _
                                                   strCompany, _
                                                   strLocation, _
                                                   strUserId, _
                                                   strParam, _
                                                   False, _
                                                   True)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_SETUP_SELLERDET_UNDEL&errmesg=" & lblErrMessage.Text & "&redirect=cm/setup/cm_setup_sellerDet.aspx?tbcode=" & strSellerCode)
            End Try


        End If

        If strSellerCode <> "" Then
            onLoad_Display()
            onLoad_BindButton()
        End If

    End Sub

    Sub BackBtn_Click(Sender As Object, E As ImageClickEventArgs)
        Response.Redirect("CM_Setup_SellerList.aspx")
    End Sub

End Class
