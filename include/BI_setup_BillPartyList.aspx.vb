

Imports System
Imports System.Data
Imports System.Collections 
Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic.Interaction

Imports agri.GL.clsSetup
Imports agri.GlobalHdl.clsGlobalHdl
Imports agri.PWSystem.clsLangCap

Public Class BI_setup_BillParty : Inherits Page

    Protected WithEvents dgLine As DataGrid
    Protected WithEvents lblTracker As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents ddlStatus As DropDownList
    Protected WithEvents txtBillPartyCode As TextBox
    Protected WithEvents txtName As TextBox
    Protected WithEvents txtLastUpdate As TextBox
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblBillParty As Label
    Protected WithEvents lblBillPartyName As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents lblTitle AS Label

    Protected objGLSetup As New agri.GL.clsSetup()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()

    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim intBIAR As Integer

    Dim objBPDs As New Object()
    Dim objLangCapDs As New Object()
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim strLocType as String
    Sub Page_Load(Sender As Object, E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_ARACCMONTH")
        strAccYear = Session("SS_ARACCYEAR")
        intBIAR = Session("SS_BIAR")
        strLocType = Session("SS_LOCTYPE")


        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumBIAccessRights.BIBillParty), intBIAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            onload_GetLangCap()
            If SortExpression.Text = "" Then
                SortExpression.Text = "BP.BillPartyCode"
            End If

            If Not Page.IsPostBack Then
                BindGrid() 
                BindPageList()
            End If
        End If
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblTitle.text = UCase(GetCaption(objLangCap.EnumLangCap.BillParty))
        lblBillParty.text = GetCaption(objLangCap.EnumLangCap.BillParty)
        lblBillPartyName.text = GetCaption(objLangCap.EnumLangCap.BillPartyName)
        dgLine.Columns(0).HeaderText = lblBillParty.text & lblCode.text
        dgLine.Columns(1).HeaderText = lblBillPartyName.text
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_SETUP_BILLPARTYLIST_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BI/setup/BI_setup_BillPartyList.aspx")
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


    Sub srchBtn_Click(sender As Object, e As System.EventArgs) 
        dgLine.CurrentPageIndex = 0
        dgLine.EditItemIndex = -1
        BindGrid() 
        BindPageList()
    End Sub 

    Sub BindGrid() 
        Dim intCnt As Integer
        Dim lbButton As LinkButton
        Dim lbl As Label


        Dim PageNo As Integer 
        Dim PageCount As Integer
        Dim dsData As DataSet
        
        dsData = LoadData
        PageCount = objGlobal.mtdGetPageCount(dsData.Tables(0).Rows.Count, dgLine.PageSize)
        
        dgLine.DataSource = dsData
        If dgLine.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgLine.CurrentPageIndex = 0
            Else
                dgLine.CurrentPageIndex = PageCount - 1
            End If
        End If
        
        dgLine.DataBind()
        BindPageList()
        PageNo = dgLine.CurrentPageIndex + 1
        lblTracker.Text="Page " & pageno & " of " & dgLine.PageCount

        For intCnt = 0 To dgLine.Items.Count - 1
            lbl = dgLine.Items.Item(intCnt).FindControl("lblStatus")
            Select Case CInt(Trim(lbl.Text))
                Case objGLSetup.EnumBillPartyStatus.Active
                    lbButton = dgLine.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = True
                    lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                Case objGLSetup.EnumBillPartyStatus.Deleted
                    lbButton = dgLine.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False
            End Select
        Next

    End Sub 

    Sub BindPageList() 
        Dim count as integer = 1   
        Dim arrDList As New ArrayList()

        While not count = dgLine.PageCount + 1
            arrDList.Add("Page " & count)
            Count = Count + 1
        End While 
        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = dgLine.CurrentPageIndex
    End Sub 

    Protected Function LoadData() As DataSet
        Dim strOpCd_Get As String = "GL_CLSSETUP_BILLPARTY_GET"
        Dim strSrchCode as string
        Dim strSrchName as string
        Dim strSrchStatus as string
        Dim strSrchLastUpdate as string
        Dim strSearch as string
        Dim strParam as string
        Dim intErrNo As Integer
        Dim intCnt As Integer

        strSrchCode = IIf(txtBillPartyCode.Text = "", "", txtBillPartyCode.Text)
        strSrchName = IIf(txtName.Text = "", "", txtName.Text)
        strSrchStatus = IIf(ddlStatus.SelectedItem.Value = "0", "", ddlStatus.SelectedItem.Value)
        strSrchLastUpdate = IIf(txtLastUpdate.Text = "", "", txtLastUpdate.Text)
        strParam = strSrchCode & "|" & _
                   strSrchName & "|" & _
                   strSrchStatus & "|" & _
                   strSrchLastUpdate & "|" & _
                   SortExpression.Text & "|" & _
                   SortCol.Text & "|"

        Try
            intErrNo = objGLSetup.mtdGetBillParty(strOpCd_Get, strParam, objBPDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_SETUP_BILLPARTYLIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To objBPDs.Tables(0).Rows.Count - 1
            objBPDs.Tables(0).Rows(intCnt).Item("BillPartyCode") = Trim(objBPDs.Tables(0).Rows(intCnt).Item("BillPartyCode"))
            objBPDs.Tables(0).Rows(intCnt).Item("Name") = Trim(objBPDs.Tables(0).Rows(intCnt).Item("Name"))
            objBPDs.Tables(0).Rows(intCnt).Item("ContactPerson") = Trim(objBPDs.Tables(0).Rows(intCnt).Item("ContactPerson"))
            objBPDs.Tables(0).Rows(intCnt).Item("Address") = Trim(objBPDs.Tables(0).Rows(intCnt).Item("Address"))
            objBPDs.Tables(0).Rows(intCnt).Item("Town") = Trim(objBPDs.Tables(0).Rows(intCnt).Item("Town"))
            objBPDs.Tables(0).Rows(intCnt).Item("State") = Trim(objBPDs.Tables(0).Rows(intCnt).Item("State"))
            objBPDs.Tables(0).Rows(intCnt).Item("PostCode") = Trim(objBPDs.Tables(0).Rows(intCnt).Item("PostCode"))
            objBPDs.Tables(0).Rows(intCnt).Item("CountryCode") = Trim(objBPDs.Tables(0).Rows(intCnt).Item("CountryCode"))
            objBPDs.Tables(0).Rows(intCnt).Item("TelNo") = Trim(objBPDs.Tables(0).Rows(intCnt).Item("TelNo"))
            objBPDs.Tables(0).Rows(intCnt).Item("FaxNo") = Trim(objBPDs.Tables(0).Rows(intCnt).Item("FaxNo"))
            objBPDs.Tables(0).Rows(intCnt).Item("Email") = Trim(objBPDs.Tables(0).Rows(intCnt).Item("Email"))
            objBPDs.Tables(0).Rows(intCnt).Item("AddChrg") = Trim(objBPDs.Tables(0).Rows(intCnt).Item("AddChrg"))
            objBPDs.Tables(0).Rows(intCnt).Item("AccCode") = Trim(objBPDs.Tables(0).Rows(intCnt).Item("AccCode"))
            objBPDs.Tables(0).Rows(intCnt).Item("Status") = CInt(Trim(objBPDs.Tables(0).Rows(intCnt).Item("Status")))
            objBPDs.Tables(0).Rows(intCnt).Item("UserName") = Trim(objBPDs.Tables(0).Rows(intCnt).Item("UserName"))
        Next                

        Return objBPDs
    End Function

    Sub btnPrevNext_Click (sender As Object, e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).commandargument
        Select Case direction
            Case "first"
                dgLine.CurrentPageIndex = 0
            Case "prev"
                dgLine.CurrentPageIndex = _
                Math.Max(0, dgLine.CurrentPageIndex - 1)
            Case "next"
                dgLine.CurrentPageIndex = _
                Math.Min(dgLine.PageCount - 1, dgLine.CurrentPageIndex + 1)
            Case "last"
                dgLine.CurrentPageIndex = dgLine.PageCount - 1
        End Select
        lstDropList.SelectedIndex = dgLine.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged (sender As Object, e As EventArgs)
        If Page.IsPostBack Then
            dgLine.CurrentPageIndex = lstDropList.SelectedIndex 
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(sender As Object, e As DataGridPageChangedEventArgs)
        dgLine.CurrentPageIndex=e.NewPageIndex
        BindGrid() 
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIf(SortCol.Text = "ASC", "DESC", "ASC")
        dgLine.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid() 
    End Sub
 
    Sub DEDR_Delete(Sender As Object, E As DataGridCommandEventArgs)
        Dim strOpCd_Add As String = ""
        Dim strOpCd_Upd As String = "GL_CLSSETUP_BILLPARTY_UPD"
        Dim strParam As String = ""
        Dim lblDelText As Label
        Dim strSelectedCode As String
        Dim intErrNo As Integer

        dgLine.EditItemIndex = CInt(E.Item.ItemIndex)
        lblDelText = dgLine.Items.Item(CInt(E.Item.ItemIndex)).FindControl("idBPCode")

        strSelectedCode = lblDelText.Text
        strParam = strSelectedCode & "||||||||||||||||" & objGLSetup.EnumBillPartyStatus.Deleted & "||||||||||||"
        Try
            intErrNo = objGLSetup.mtdUpdBillParty(strOpCd_Add, _
                                                strOpCd_Upd, _
                                                strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strParam, _
                                                True)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BI_SETUP_BILLPARTYLIST_DEL&errmesg=" & lblErrMessage.Text & "&redirect=bi/setup/BI_setup_BillPartyList.aspx")
        End Try

        dgLine.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub NewBillPartyBtn_Click(Sender As Object, E As ImageClickEventArgs)
        Response.Redirect("BI_setup_BillPartyDet.aspx")
    End Sub


End Class
