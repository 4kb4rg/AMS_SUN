Imports System
Imports System.Data
Imports System.IO 
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic.Interaction
Imports Microsoft.VisualBasic
Imports agri.Admin.clsUom
Imports agri.GlobalHdl.clsGlobalHdl


Public Class Admin_UOMConvertionDet : Inherits Page

    Protected WithEvents ddlUOMFrom As DropDownList
    Protected WithEvents ddlUOMTo As DropDownList
    Protected WithEvents txtRate As TextBox
    Protected WithEvents lblStatus As Label
    Protected WithEvents lblDateCreated As Label
    Protected WithEvents lblLastUpdate As Label
    Protected WithEvents lblUpdatedBy As Label
    Protected WithEvents lblHiddenStatus As Label
    Protected WithEvents uomfrom As HtmlInputHidden
    Protected WithEvents uomto As HtmlInputHidden
    Protected WithEvents lblErrDup As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblErrUOMFrom As Label
    Protected WithEvents lblErrUOMTo As Label
    Protected WithEvents SaveBtn As ImageButton
    Protected WithEvents DelBtn As ImageButton
    Protected WithEvents UnDelBtn As ImageButton

    Dim objAdmin As New agri.Admin.clsUom()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objUOMDs As New Object()
    Dim objUOMConvertionDs As New Object()
 
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim intADAR As Integer

    Dim strSelectedUOMFrom As String = ""
    Dim strSelectedUOMTo As String = ""
    Dim strSortExpression As String = "UOMFrom"

    Sub Page_Load(Sender As Object, E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        intADAR = Session("SS_ADAR")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumADAccessRights.ADUOM), intADAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            lblErrUOMFrom.Visible = False
            lblErrUOMTo.Visible = False
            strSelectedUOMFrom = Trim(IIf(Request.QueryString("uomfrom") <> "", Request.QueryString("uomfrom"), Request.Form("uomfrom")))
            strSelectedUOMTo = Trim(IIf(Request.QueryString("uomto") <> "", Request.QueryString("uomto"), Request.Form("uomto")))
            If Not IsPostBack Then
                If strSelectedUOMFrom <> "" AND strSelectedUOMTo <> "" Then
                    uomfrom.Value = strSelectedUOMFrom
                    uomto.Value = strSelectedUOMTo
                    onLoad_Display()
                    BindButton()
                Else
                    newLoad_Display()
                    BindButton()
                End If
            End If
        End If
    End Sub

    Sub BindButton()
        ddlUOMFrom.Enabled = False
        ddlUOMTo.Enabled = False
        txtRate.Enabled = False
        SaveBtn.Visible = False
        DelBtn.Visible = False
        UnDelBtn.Visible = False

        Select case CInt(lblHiddenStatus.Text)
            case objAdmin.EnumUOMStatus.Active
                txtRate.Enabled = True
                SaveBtn.Visible = True
                DelBtn.Visible = True
                DelBtn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
            case objAdmin.EnumUOMStatus.Deleted
                txtRate.Enabled = False
                UnDelBtn.Visible = True
            case else
                ddlUOMFrom.Enabled = True
                ddlUOMTo.Enabled = True
                txtRate.Enabled = True
                SaveBtn.Visible = True
        End Select
    End Sub

    Sub onLoad_Display()
        Dim strOpCd_GetUOMConvertion As String = "ADMIN_CLSUOM_CONVERTION_LIST_GET"
        Dim strParam As String = ""
        Dim intErrNo As Integer
        Dim strUOMFrom As String = ""
        Dim strUOMTo As String = ""          
        Dim intSelectedUOMFromIndex As Integer = 0
        Dim intSelectedUOMToIndex As Integer = 0
        Dim intCnt As Integer
        Dim dr As DataRow

        strParam = strSelectedUOMFrom & "|" & strSelectedUOMTo & "|||" & strSortExpression & "||Upd" 

        Try
            intErrNo = objAdmin.mtdGetUOMConvertion(strOpCd_GetUOMConvertion, strParam, objUOMConvertionDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADMIN_GET_UOMConvertion&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        If objUOMConvertionDs.Tables(0).Rows.Count > 0 Then
            objUOMConvertionDs.Tables(0).Rows(0).Item(0) = Trim(objUOMConvertionDs.Tables(0).Rows(0).Item(0))
            objUOMConvertionDs.Tables(0).Rows(0).Item(1) = Trim(objUOMConvertionDs.Tables(0).Rows(0).Item(1))
            objUOMConvertionDs.Tables(0).Rows(0).Item(2) = Trim(objUOMConvertionDs.Tables(0).Rows(0).Item(2))
            objUOMConvertionDs.Tables(0).Rows(0).Item(3) = Trim(objUOMConvertionDs.Tables(0).Rows(0).Item(3))
            objUOMConvertionDs.Tables(0).Rows(0).Item(4) = Trim(objUOMConvertionDs.Tables(0).Rows(0).Item(4))
            objUOMConvertionDs.Tables(0).Rows(0).Item(5) = Trim(objUOMConvertionDs.Tables(0).Rows(0).Item(5))
            objUOMConvertionDs.Tables(0).Rows(0).Item(6) = Trim(objUOMConvertionDs.Tables(0).Rows(0).Item(6))
            objUOMConvertionDs.Tables(0).Rows(0).Item(7) = Trim(objUOMConvertionDs.Tables(0).Rows(0).Item(7))

            strUOMFrom = objUOMConvertionDs.Tables(0).Rows(0).Item(0)
            strUOMTo = objUOMConvertionDs.Tables(0).Rows(0).Item(1)
            txtRate.Text = CDbl(objUOMConvertionDs.Tables(0).Rows(0).Item(2))
            lblStatus.Text = objAdmin.mtdGetUOMStatus(objUOMConvertionDs.Tables(0).Rows(0).Item(3))
            lblHiddenStatus.Text = Trim(objUOMConvertionDs.Tables(0).Rows(0).Item(3))
            lblDateCreated.Text = objGlobal.GetLongDate(objUOMConvertionDs.Tables(0).Rows(0).Item(4))
            lblLastUpdate.Text = objGlobal.GetLongDate(objUOMConvertionDs.Tables(0).Rows(0).Item(5))
            lblUpdatedBy.Text = objUOMConvertionDs.Tables(0).Rows(0).Item(7)
        End If

        For intCnt = 0 To objUOMConvertionDs.Tables(0).Rows.Count - 1
            objUOMConvertionDs.Tables(0).Rows(intCnt).Item(0) = Trim(objUOMConvertionDs.Tables(0).Rows(intCnt).Item(0))
            objUOMConvertionDs.Tables(0).Rows(intCnt).Item(1) = Trim(objUOMConvertionDs.Tables(0).Rows(intCnt).Item(1))
            If objUOMConvertionDs.Tables(0).Rows(intCnt).Item(0) = strUOMFrom Then
                intSelectedUOMFromIndex = intCnt + 1
            End If
            If objUOMConvertionDs.Tables(0).Rows(intCnt).Item(1) = strUOMTo Then
                intSelectedUOMToIndex = intCnt + 1
            End If
        Next

        dr = objUOMConvertionDs.Tables(0).NewRow()
        dr("UOMFrom") = ""
        dr("UOMTo") = ""
        objUOMConvertionDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlUOMFrom.DataSource = objUOMConvertionDs.Tables(0)
        ddlUOMFrom.DataTextField = "UOMFrom"
        ddlUOMFrom.DataValueField = "UOMFrom"
        ddlUOMFrom.DataBind()
        ddlUOMFrom.SelectedIndex = intSelectedUOMFromIndex

        ddlUOMTo.DataSource = objUOMConvertionDs.Tables(0)
        ddlUOMTo.DataTextField = "UOMTo"
        ddlUOMTo.DataValueField = "UOMTo"
        ddlUOMTo.DataBind()
        ddlUOMTo.SelectedIndex = intSelectedUOMToIndex
    End Sub

    Sub newLoad_display()
        Dim strOpCd_GetUOM As String = "ADMIN_CLSUOM_UOM_LIST_GET"
        Dim strParam As String = ""
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim dr As DataRow

        strParam = "||||" & strSortExpression & "|"

        Try
            intErrNo = objAdmin.mtdGetUOM(strOpCd_GetUOM, strParam, objUOMDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADMIN_GET_UOM&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try
        
        For intCnt = 0 To objUOMDs.Tables(0).Rows.Count - 1
            objUOMDs.Tables(0).Rows(intCnt).Item(0) = Trim(objUOMDs.Tables(0).Rows(intCnt).Item(0))
            objUOMDs.Tables(0).Rows(intCnt).Item(1) = Trim(objUOMDs.Tables(0).Rows(intCnt).Item(1))
        Next
        
        dr = objUOMDs.Tables(0).NewRow()
        dr("UOMDesc") = "Select Unit of Measurement"
        dr("UOMCode") = ""
        objUOMDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlUOMFrom.DataSource = objUOMDs.Tables(0)
        ddlUOMFrom.DataTextField = "UOMDesc"
        ddlUOMFrom.DataValueField = "UOMCode"
        ddlUOMFrom.DataBind()

        ddlUOMTo.DataSource = objUOMDs.Tables(0)
        ddlUOMTo.DataTextField = "UOMDesc"
        ddlUOMTo.DataValueField = "UOMCode"
        ddlUOMTo.DataBind()
    End Sub

    Sub Button_Click(Sender As Object, E As ImageClickEventArgs)
        Dim strOpCd_Add As String = "ADMIN_CLSUOM_Convertion_LIST_ADD"
        Dim strOpCd_Upd As String = "ADMIN_CLSUOM_Convertion_LIST_UPD"
        Dim strOpCd_Get As String = "ADMIN_CLSUOM_Convertion_LIST_GET"
        Dim strCmdArgs As String = CType(Sender, ImageButton).CommandArgument        
        Dim intErrNo As Integer
        Dim strRate As String
        Dim strStatus As String
        Dim strParam As String = ""

        strSelectedUOMFrom = ddlUOMFrom.SelectedItem.Value
        strSelectedUOMTo = ddlUOMTo.SelectedItem.Value

        If strSelectedUOMFrom = "" Then
            lblErrUOMFrom.Visible = True
            Exit Sub
        ElseIf strSelectedUOMTo = "" Then
            lblErrUOMTo.Visible = True
            Exit Sub
        End If

        strRate = txtRate.Text

        lblErrDup.Visible = False

        Select case Trim(lblStatus.Text)
            case objAdmin.mtdGetUOMStatus(objAdmin.EnumUOMStatus.Active)
             strStatus = objAdmin.EnumUOMStatus.Active
            case objAdmin.mtdGetUOMStatus(objAdmin.EnumUOMStatus.Deleted)
             strStatus = objAdmin.EnumUOMStatus.Deleted
        End Select
                
   
        If strCmdArgs = "Save" Then
            strParam = strSelectedUOMFrom & "|" & strSelectedUOMTo & "|||" & strSortExpression & "||Add"

            Try
               intErrNo = objAdmin.mtdGetUOMConvertion(strOpCd_Get, strParam, objUOMConvertionDs)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADMIN_GET_Convertion&errmesg=" & lblErrMessage.Text & "&redirect=")
            End Try

            If objUOMConvertionDs.Tables(0).Rows.Count <> 0 Then
                If uomfrom.value = "" and uomto.value = ""
                    lblErrDup.Visible = True
                Else
                    strParam = strSelectedUOMFrom & "|" & _
                               strSelectedUOMTo & "|" & _
                               strRate & "|" & _
                               strStatus & "|"
                           
                    Try
                        intErrNo = objAdmin.mtdUpdUOMConvertion(strOpCd_Add, _
                                                                strOpCd_Upd, _
                                                                strCompany, _
                                                                strLocation, _
                                                                strUserId, _
                                                                strParam, _
                                                                True)                                           
                    Catch Exp As System.Exception
                        Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADMIN_UPD_Convertion&errmesg=" & lblErrMessage.Text & "&redirect=")
                    End Try
                End If
            Else
                strParam = strSelectedUOMFrom & "|" & _
                           strSelectedUOMTo & "|" & _
                           strRate & "|" & _
                           objAdmin.EnumUOMStatus.Active & "|"
          
                Try
                   intErrNo = objAdmin.mtdUpdUOMConvertion(strOpCd_Add, _
                                                           strOpCd_Upd, _
                                                           strCompany, _
                                                           strLocation, _
                                                           strUserId, _
                                                           strParam, _
                                                           False)
                Catch Exp As System.Exception
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADMIN_ADD_Convertion&errmesg=" & lblErrMessage.Text & "&redirect=")
                End Try
            End If

        ElseIf strCmdArgs = "Del" Then
            strParam = strSelectedUOMFrom & "|" & strSelectedUOMTo & "||" & objAdmin.EnumUOMStatus.Deleted & "||"

            Try
                intErrNo = objAdmin.mtdUpdUOMConvertion(strOpCd_Add, _
                                                        strOpCd_Upd, _
                                                        strCompany, _
                                                        strLocation, _
                                                        strUserId, _
                                                        strParam, _
                                                        True)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADMIN_DEL_Convertion&errmesg=" & lblErrMessage.Text & "&redirect=")
            End Try

        ElseIf strCmdArgs = "UnDel" Then
            strParam = strSelectedUOMFrom & "|" & strSelectedUOMTo & "||" & objAdmin.EnumUOMStatus.Active & "||"

            Try
                intErrNo = objAdmin.mtdUpdUOMConvertion(strOpCd_Add, _
                                                        strOpCd_Upd, _
                                                        strCompany, _
                                                        strLocation, _
                                                        strUserId, _
                                                        strParam, _
                                                        True)                                            
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADMIN_UNDEL_Convertion&errmesg=" & lblErrMessage.Text & "&redirect=")
            End Try
        End If

        onLoad_Display()
        BindButton()
    End Sub

    Sub BackBtn_Click(Sender As Object, E As ImageClickEventArgs)
        Response.Redirect("Admin_uom_UOMConvertionList.aspx")
    End Sub

    
End Class
