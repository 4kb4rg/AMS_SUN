
Imports System
Imports System.Data
Imports System.Web.Mail
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic.DateAndTime
Imports Microsoft.VisualBasic

Imports agri.PWSystem

Public Class change_password : Inherits Page

    Protected WithEvents tblBefore As HtmlTable
    Protected WithEvents hidReferer As HtmlInputHidden
    Protected WithEvents txtPassword As TextBox
	Protected WithEvents txtConfirmPwd As TextBox
    Protected WithEvents lblSuccess As Label
    Protected WithEvents lblErrNoUser As Label
    Protected WithEvents lblErrNoEmail As Label
    Protected WithEvents lblErrMesage As Label
    Protected WithEvents lblErrSentFail As Label
	Protected WithEvents lblPassword As Label

    Dim objSysCfg As New agri.PWSystem.clsConfig()

    Dim objSysUser As New agri.PWSystem.clsUser()
	Dim ObjOK As New agri.GL.ClsTrx
	 
    
	Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
   
    Dim strSelectedUserId As String = ""
	

    Sub Page_Load(Sender As Object, E As EventArgs)
		strCompany = Session("SS_COMPANY")
		strLocation = Session("SS_LOCATION")
		strUserId = Session("SS_USERID")
		
		If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        Else
	        lblSuccess.Visible = False
			lblErrNoUser.Visible = False
			lblPassword.Visible = False
		
			If Not IsPostBack Then
			  strSelectedUserId = strUserId
			End If
		End If
		
    End Sub

	Function getlevel(ByVal index As String) As Integer
        Dim strOpCd_GetID As String = "PWSYSTEM_CLSUSER_USERDETAILS_GET"
        Dim objNewID As New Object
		Dim intErrNo As Integer
		
        Try
            intErrNo = ObjOK.mtdGetDataCommon(strOpCd_GetID, "USERID",index, objNewID)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ALL_ALL_GETID&errmesg=" & Exp.Message & "&redirect=hr/trx/HR_trx_EmployeeList.aspx")
        End Try

        Return objNewID.Tables(0).Rows(0).Item("UsrLevel")
    End Function
		
	Function getDeptCode(ByVal index As String) As String
        Dim strOpCd_GetID As String = "PWSYSTEM_CLSUSER_USERDETAILS_GET"
        Dim objNewID As New Object
		Dim intErrNo As Integer
		
        Try
            intErrNo = ObjOK.mtdGetDataCommon(strOpCd_GetID, "USERID",index, objNewID)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ALL_ALL_GETID&errmesg=" & Exp.Message & "&redirect=hr/trx/HR_trx_EmployeeList.aspx")
        End Try

        Return objNewID.Tables(0).Rows(0).Item("DeptCode")
    End Function

    Sub OKBtn_Click(Sender As Object, E As EventArgs)
        Dim objUserDs As New Object
        Dim strOpCode As String = "PWSYSTEM_CLSUSER_USERDETAILS_UPD"
        Dim intErrNo As Integer
        Dim intPos As Integer
        Dim strParam As String
        Dim strPassword As String
		Dim intLevel As Integer
		Dim strDeptCode As String = ""
		
		intLevel = getlevel(strUserId)
		strDeptCode = getDeptCode(strUserId)
    
        
		If (txtPassword.Text <> txtConfirmPwd.Text) Then
            lblPassword.Visible = True
            Exit Sub
        End If
		
		strPassword = txtPassword.Text.trim()
	 	
		strParam = strUserId & "|" & strPassword & "|||||||" & intLevel & "|" & strDeptCode
                        
                Try
                    intErrNo = objSysUser.mtdUpdUser(strOpCode, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strParam, _
                                                    objSysUser.EnumUserUpdType.Update)
	                lblSuccess.visible = True
                Catch Exp As System.Exception
				    lblErrNoUser.visible = True
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=USERDET_SAVE_UPD_USER&errmesg=" & lblErrMesage.Text & "&redirect=system/user/userlist.aspx")
                End Try
				
    End Sub

    Sub BackBtn_Click(Sender As Object, E As EventArgs)
        Response.Redirect(Request.QueryString("referer"))
    End Sub



End Class
