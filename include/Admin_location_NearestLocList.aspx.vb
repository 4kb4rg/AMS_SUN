
Imports System
Imports System.Data
Imports System.Collections 
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic
Imports agri.GlobalHdl.clsGlobalHdl
Imports agri.PR
Imports agri.PWSystem.clsLangCap


Public Class Admin_Location_NearestLocList : Inherits Page

    Protected WithEvents EventData as DataGrid
    Protected WithEvents ErrorMessage as Label
    Protected WithEvents lblTracker as Label
    Protected WithEvents lblErrMessage as Label
    Protected WithEvents strState as Label
    Protected WithEvents SQLStatement as Label
    Protected WithEvents lstDropList as DropDownList
    Protected WithEvents StatusList as DropDownList
    Protected WithEvents srchStatusList as DropDownList
    Protected WithEvents SortExpression as Label
    Protected WithEvents blnUpdate as Label
    Protected WithEvents curStatus as Label
    Protected WithEvents sortcol as Label
    Protected WithEvents srchNearestLoc as TextBox
    Protected WithEvents srchDesc as TextBox
    Protected WithEvents srchUpdateBy as TextBox
    Protected WithEvents lblTitle as Label
    Protected WithEvents lblNearestLoc as Label
    Protected WithEvents lblDesc as Label
    Protected WithEvents lblCode as Label
    Protected WithEvents lblPleaseEnter as Label
    Protected WithEvents lblList as Label

    Protected WithEvents txtDescription as TextBox
    Protected WithEvents txtNearestLoc as TextBox


    Protected objAdmin As New agri.Admin.clsLoc()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()

    Dim strOppCd_GET As String = "ADMIN_CLSLOC_NEARESTLOC_LIST_GET"
    Dim strOppCd_ADD As String = "ADMIN_CLSLOC_NEARESTLOC_LIST_ADD"
    Dim strOppCd_UPD As String = "ADMIN_CLSLOC_NEARESTLOC_LIST_UPD"

    Dim objDataSet As New Object()
    Dim objLangCapDs As New Object()
    Dim intErrNo As Integer
    Dim strParam As String = ""

    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim intADAR As Integer
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strValidateNearestLoc As String
    Dim strValidateDesc As String
    Dim DocTitleTag As String
    Dim NearestLocTag As String
    Dim NearestLocDescTag As String

    Dim strLocType as String

    Sub Page_Load(Sender As Object, E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        intADAR = Session("SS_ADAR")
        strAccMonth = Session("SS_PRACCMONTH")
        strAccYear = Session("SS_PRACCYEAR")
        strLocType = Session("SS_LOCTYPE")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumADAccessRights.ADNearestLocation), intADAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")

        Else
            onload_GetLangCap()
            If SortExpression.text = "" Then
                SortExpression.text = "NearLocCode"
                sortcol.text = "ASC"
            End If
            If Not Page.IsPostBack Then
                BindSearchList() 
                BindGrid() 
                BindPageList()
            End If
        End If
    End Sub
    Sub srchBtn_Click(sender As Object, e As System.EventArgs) 
        EventData.CurrentPageIndex = 0
        EventData.EditItemIndex = -1
        BindGrid() 
        BindPageList()
    End Sub 
   

    Sub btnPrevNext_Click (sender As Object, e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).commandargument
        Select Case direction
            Case "first"
                EventData.CurrentPageIndex = 0
            Case "prev"
                EventData.CurrentPageIndex = _
                    Math.Max(0, EventData.CurrentPageIndex - 1)
            Case "next"
                EventData.CurrentPageIndex = _
                    Math.Min(EventData.PageCount - 1, EventData.CurrentPageIndex + 1)
            Case "last"
                EventData.CurrentPageIndex = EventData.PageCount - 1
        End Select
        lstDropList.SelectedIndex = EventData.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged (sender As Object, e As EventArgs)
        If Page.IsPostBack Then
            EventData.CurrentPageIndex = lstDropList.SelectedIndex 
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(sender As Object, e As DataGridPageChangedEventArgs)
        EventData.CurrentPageIndex=e.NewPageIndex
        BindGrid() 
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.text = e.SortExpression.ToString()
        sortcol.text = IIF(sortcol.text = "ASC", "DESC", "ASC")
        EventData.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid() 
    End Sub
 
    Sub DEDR_Edit(Sender As Object, E As DataGridCommandEventArgs)
        Dim EditText As TextBox
        Dim List As Dropdownlist
        Dim Updbutton As linkbutton
        Dim validateDescription As RequiredFieldValidator
        Dim validateNearestLoc As RequiredFieldValidator
        Dim DataLabel As Label   

        strState.Text = "edit"
        blnUpdate.text = True
        EventData.EditItemIndex = CInt(e.Item.ItemIndex)

        BindGrid() 
        If CInt(e.Item.ItemIndex) >= EventData.Items.Count then
            EventData.EditItemIndex = -1
            Exit Sub
        End If
        BindStatusList(EventData.EditItemIndex)

        EditText = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("Status")
        Select Case cint(Edittext.text) = objAdmin.EnumNearLocStatus.Active
            Case True
                Statuslist.selectedindex = 0
                EditText = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("txtNearestLoc")
                EditText.Enabled = False
                Updbutton = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("Delete")
                Updbutton.Text = "Delete"
                Updbutton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
            Case False
                Statuslist.selectedindex = 1
                EditText = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("txtNearestLoc")
                EditText.Enabled = False
                EditText = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("UpdateDate")
                EditText.Enabled = False
                EditText = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("UserName")
                EditText.Enabled = False
                List = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("StatusList")
                List.Enabled = False 
                EditText = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("txtDescription")
                EditText.Enabled = False
                Updbutton = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("Update")
                Updbutton.Visible = False
                Updbutton = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("Delete")
                Updbutton.Text = "Undelete"
        End Select  

        validateDescription = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("validateDescription")
        validateDescription.ErrorMessage = strValidateNearestLoc 
        validateNearestLoc  = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("validateNearestLoc")
        validateNearestLoc.ErrorMessage = strValidateDesc
    End Sub

    Sub DEDR_Update(Sender As Object, E As DataGridCommandEventArgs)
        Dim EditText As TextBox
        Dim list As Dropdownlist
        Dim NearestLoc As String
        Dim blnDupKey As Boolean = False
        Dim lblMsg as label
        Dim Status As String
        Dim CreateDate As String
        Dim Desc As String
        Dim sortitem as string
        Dim intCnt as integer
        Dim blnUpd As Boolean

        EditText = E.Item.FindControl("txtNearestLoc")
        NearestLoc = EditText.Text
        If EditText.Enabled = False Then blnUpd = True
        EditText = E.Item.FindControl("txtDescription")
        Desc = EditText.Text
        list = E.Item.FindControl("StatusList")
        Status = list.Selecteditem.Value
        EditText = E.Item.FindControl("CreateDate")
        CreateDate = EditText.Text

        If Not blnUpd Then
           
            sortitem =  sortexpression.text
            strParam =  NearestLoc & "|||" & _
                        srchUpdateBy.text & "|" & _
                        sortitem & "||" & "get"

            Try
            intErrNo = objAdmin.mtdGetNearLocCode(strOppCd_GET, strParam, objDataSet)
            Catch Exp As System.Exception
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_NEAREST_LOC&errmesg=" & lblErrMessage.Text & "&redirect=Admin/Location/Admin_Location_NearestLocList.aspx")
            End Try

            If objDataSet.Tables(0).Rows.Count > 0 Then
                lblMsg = E.Item.FindControl("lblDupMsg")
                lblMsg.Visible = True
                Exit Sub
            End If
        End If

        strParam =  NearestLoc & "|" & _
                    Desc & "|" & _                    
                    Status 
        Try
        intErrNo = objAdmin.mtdUpdNearLocCode(strOppCd_ADD, _
                                                strOppCd_UPD, _                                               
                                                strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strParam, _                                                
                                                blnUpdate.text)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=UPDATE_NEARESTLOCLIST&errmesg=" & lblErrMessage.Text & "&redirect=Admin/Location/Admin_location_NearestLocList.aspx")
        End Try

            EventData.EditItemIndex = -1
            BindGrid() 
            strState.Text = ""

  End Sub

    Sub DEDR_Cancel(Sender As Object, E As DataGridCommandEventArgs)
        strState.Text = ""
        EventData.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub DEDR_Delete(Sender As Object, E As DataGridCommandEventArgs)

        Dim EditText As TextBox
        Dim list As DropDownList
        Dim NearestLoc As String
        Dim Description As String
        Dim blnDupKey As Boolean = False
        Dim Status As String
        Dim CreateDate As String
       
        Dim Desc As String


        EditText = E.Item.FindControl("txtNearestLoc")
        NearestLoc = EditText.Text
        EditText = E.Item.FindControl("Status")
        Status = IIF(EditText.Text = objAdmin.EnumNearLocStatus.Active, _
                        objAdmin.EnumNearLocStatus.Deleted, _
                        objAdmin.EnumNearLocStatus.Active )
        EditText = E.Item.FindControl("txtDescription")
        Desc = Trim(EditText.Text)
        EditText = E.Item.FindControl("CreateDate")
        CreateDate = EditText.Text
        strParam =  NearestLoc & "|" & _
                    Desc & "|" & _                    
                    Status 
        Try
        intErrNo = objAdmin.mtdUpdNearLocCode(strOppCd_ADD, _
                                                strOppCd_UPD, _                                               
                                                strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strParam, _                                                
                                                blnUpdate.text)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=UPDATE_NEARESTLOC&errmesg=" & lblErrMessage.Text & "&redirect=Admin/Location/Admin_location_NearestLocList.aspx")
        End Try
      
        EventData.EditItemIndex = -1
        
        If  CInt(e.Item.ItemIndex) = 0 and EventData.Items.Count = 1  And EventData.PageCount <> 1 And srchStatusList.selectedItem.Value <> objAdmin.EnumNearLocStatus.All then
            EventData.CurrentPageIndex = EventData.PageCount - 2 
        End If
        
        BindGrid() 
        BindPageList()
    End Sub

    Sub DEDR_Add(Sender As Object, E As ImageClickEventArgs)
        Dim dataSet As DataSet = LoadData
        Dim newRow As DataRow
        Dim Updbutton As LinkButton
        Dim txtNearestLoc As TextBox
        Dim txtDescription As TextBox
        Dim validateNearestLoc As RequiredFieldValidator
        Dim validateDescription As RequiredFieldValidator
        Dim PageCount As Integer

        strState.Text = "add"

        blnUpdate.text = False
        newRow = dataSet.Tables(0).NewRow()
        newRow.Item("NearLocCode") = ""
        newRow.Item("Description") = ""
        newRow.Item("Status") = 1
        newRow.Item("CreateDate") = DateTime.now()
        NewRow.Item("UpdateDate") = DateTime.now()
        NewRow.Item("UserName") = ""
        dataSet.Tables(0).Rows.Add(newRow)
        
        EventData.DataSource = dataSet.Tables(0)
        PageCount = objGlobal.mtdGetPageCount(dataSet.Tables(0).Rows.Count, EventData.PageSize)
        If EventData.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                EventData.CurrentPageIndex = 0
            Else
                EventData.CurrentPageIndex = PageCount - 1
            End If
        End If
        EventData.DataBind()
        BindPageList()

        EventData.CurrentPageIndex = EventData.PageCount - 1
        lblTracker.Text="Page " & (EventData.CurrentPageIndex + 1) & " of " & EventData.PageCount
        lstDropList.SelectedIndex = EventData.CurrentPageIndex
        EventData.DataBind()
        EventData.EditItemIndex = EventData.Items.Count - 1

        EventData.DataBind()
        BindStatusList(EventData.EditItemIndex)
        
        Updbutton = EventData.Items.Item(CInt(EventData.EditItemIndex)).FindControl("Delete")
        Updbutton.visible = False

        txtNearestLoc = EventData.Items.Item(CInt(EventData.EditItemIndex)).FindControl("txtNearestLoc")
        txtDescription = EventData.Items.Item(CInt(EventData.EditItemIndex)).FindControl("txtDescription")

        validateNearestLoc = EventData.Items.Item(CInt(EventData.EditItemIndex)).FindControl("validateNearestLoc")
        validateNearestLoc.ErrorMessage = strvalidateNearestLoc
        validateDescription = EventData.Items.Item(CInt(EventData.EditItemIndex)).FindControl("validateDescription")
        validateDescription.ErrorMessage = strValidateDesc

    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblTitle.text = UCase(GetCaption(objLangCap.EnumLangCap.NearestLocation))
        lblNearestLoc.text = GetCaption(objLangCap.EnumLangCap.NearestLocation) & lblCode.Text
        lblDesc.text = GetCaption(objLangCap.EnumLangCap.NearestLocDesc)

        EventData.Columns(0).HeaderText = lblNearestLoc.text
        EventData.Columns(1).HeaderText = lblDesc.text

        strvalidateNearestLoc = "<br>" & lblPleaseEnter.text & " "  & lblNearestLoc.text
        strvalidateDesc = "<br>" & lblPleaseEnter.text & " "  & lblDesc.text

        DocTitleTag = lblTitle.text & lblList.text
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=IN_SETUP_PRODMODEL_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=IN/Setup/IN_prodmodel.aspx")
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

    Sub BindGrid() 
        Dim PageNo As Integer 
        Dim PageCount As Integer
        Dim dsData As DataSet
        
        dsData = LoadData
        PageCount = objGlobal.mtdGetPageCount(dsData.Tables(0).Rows.Count, EventData.PageSize)
        
        EventData.DataSource = dsData
        If EventData.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                EventData.CurrentPageIndex = 0
            Else
                EventData.CurrentPageIndex = PageCount - 1
            End If
        End If
        
        EventData.DataBind()
        BindPageList()
        PageNo = EventData.CurrentPageIndex + 1
        lblTracker.Text="Page " & pageno & " of " & EventData.PageCount
    End Sub 

    Sub BindPageList() 

        Dim count as integer = 1   
        Dim arrDList As New ArrayList()

        While not count = EventData.PageCount + 1
            arrDList.Add("Page " & count)
            Count = Count +1
        End While 
        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = EventData.CurrentPageIndex

    End Sub 

    Sub BindStatusList(index as integer) 

        StatusList = EventData.Items.Item(index).FindControl("StatusList")
        StatusList.Items.Add(New ListItem(objAdmin.mtdGetNearLocStatus(objAdmin.EnumNearLocStatus.Active), objAdmin.EnumNearLocStatus.Active))
        StatusList.Items.Add(New ListItem(objAdmin.mtdGetNearLocStatus(objAdmin.EnumNearLocStatus.Deleted), objAdmin.EnumNearLocStatus.Deleted))

    End Sub 

    Sub BindSearchList() 

        srchStatusList.Items.Add(New ListItem(objAdmin.mtdGetNearLocStatus(objAdmin.EnumNearLocStatus.Active), objAdmin.EnumNearLocStatus.Active))
        srchStatusList.Items.Add(New ListItem(objAdmin.mtdGetNearLocStatus(objAdmin.EnumNearLocStatus.Deleted), objAdmin.EnumNearLocStatus.Deleted))
        srchStatusList.Items.Add(New ListItem(objAdmin.mtdGetNearLocStatus(objAdmin.EnumNearLocStatus.All), objAdmin.EnumNearLocStatus.All))

    End Sub 

    Protected Function LoadData() As DataSet
        
        Dim UpdateBy as string
        Dim srchStatus as string
        Dim strParam as string
        Dim SearchStr as string
        Dim sortitem as string
        Dim intCnt as integer

       
        sortitem =  sortexpression.text
        strParam =  Trim(srchNearestLoc.text) & "|" & _
                    Trim(srchDesc.Text) & "|" & _
                    IIF(Not srchStatusList.selectedItem.Value = objAdmin.EnumNearLocStatus.All,srchStatusList.selectedItem.Value, "%" ) & "|" & _
                    srchUpdateBy.text & "|" & _
                    sortitem & "||" 

        Try
        intErrNo = objAdmin.mtdGetNearLocCode(strOppCd_GET, strParam, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_NEAREST_LOC&errmesg=" & lblErrMessage.Text & "&redirect=Admin/Location/Admin_Location_NearestLocList.aspx")
        End Try

        If objDataSet.Tables(0).Rows.Count > 0 Then
            For intCnt=0 To objDataSet.Tables(0).Rows.Count - 1
                objDataSet.Tables(0).Rows(intCnt).Item("NearLocCode") = Trim(objDataSet.Tables(0).Rows(intCnt).Item("NearLocCode"))
                objDataSet.Tables(0).Rows(intCnt).Item("Description") = Trim(objDataSet.Tables(0).Rows(intCnt).Item("Description"))                
                objDataSet.Tables(0).Rows(intCnt).Item("Status") = Trim(objDataSet.Tables(0).Rows(intCnt).Item("Status"))
                objDataSet.Tables(0).Rows(intCnt).Item("CreateDate") = Trim(objDataSet.Tables(0).Rows(intCnt).Item("CreateDate"))
                objDataSet.Tables(0).Rows(intCnt).Item("UpdateDate") = Trim(objDataSet.Tables(0).Rows(intCnt).Item("UpdateDate"))
                objDataSet.Tables(0).Rows(intCnt).Item("UserName") = Trim(objDataSet.Tables(0).Rows(intCnt).Item("UserName"))
            Next
        End If

       Return objDataSet
    End Function




End Class
