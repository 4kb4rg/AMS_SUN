
Imports System
Imports System.Web.UI
Imports System.Web.UI.Page
Imports System.Web.UI.WebControls

Imports agri.PWSystem.clsUser

Public Class change_password : Inherits Page

    Protected WithEvents txtUserId As TextBox
    Protected WithEvents txtPwd As TextBox
    Protected WithEvents txtConfirmPwd As TextBox
    Protected WithEvents txtNewPwd As TextBox
    Protected WithEvents lblChangeResult As Label
    Protected WithEvents lblErrReEnter As Label
    Protected WithEvents lblErrNotChg As Label
    Protected WithEvents lblErrMesage As Label

    Dim objSysCfg As New agri.PWSystem.clsConfig()

    Dim objSysUser As New agri.PWSystem.clsUser()
    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String

    Sub Page_Load(Sender As Object, E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        lblChangeResult.Text = ""
        txtUserId.Text = strUserId
    End Sub


    Sub changeBtn_Click(Sender As Object, E As ImageClickEventArgs)
        Dim strOpCode_GetUser As String = "PWSYSTEM_CLSUSER_USERDETAILS_GET"
        Dim strOpCode_UpdUser As String = "PWSYSTEM_CLSUSER_USERPWD_UPD"
        Dim strOpCodes As String = strOpCode_GetUser & "|" & strOpCode_UpdUser
        Dim IsChange As Boolean
        Dim intErrNo As Integer
        Dim strParam As String

        If txtNewPwd.Text <> txtConfirmPwd.Text Then
            lblChangeResult.Text = lblErrReEnter.Text
        Else
            strParam = txtUserId.Text & "|" & txtPwd.Text & "|" & txtNewPwd.Text
            Try
                intErrNo = objSysUser.mtdChangePassword(strOpCodes, _
                                                        strCompany, _
                                                        strLocation, _
                                                        strUserId, _
                                                        strParam, _
                                                        IsChange)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CHGPWD_CHG_PWD&errmesg=" & lblErrMesage.Text & "&redirect=")
            End Try

            If IsChange Then
                Response.Redirect(Request.QueryString("referer"))
            Else
                lblChangeResult.Text = lblErrNotChg.Text
            End If
        End If
    End Sub

    Sub BackBtn_Click(Sender As Object, E As ImageClickEventArgs)
        Response.Redirect(Request.QueryString("referer"))
    End Sub

End Class
