Imports System
Imports System.Data
Imports System.Collections 
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic
Imports agri.GlobalHdl.clsGlobalHdl
Imports agri.GL
Imports agri.PWSystem.clsLangCap

Public Class CM_Setup_PriceBasisList : Inherits Page

    Protected WithEvents EventData as DataGrid
    Protected WithEvents ErrorMessage as Label
    Protected WithEvents lblTracker as Label
    Protected WithEvents lstDropList as DropDownList
    Protected WithEvents StatusList as DropDownList
    Protected WithEvents SortExpression as Label
    Protected WithEvents blnUpdate as Label
    Protected WithEvents lblErrMessage as Label
    Protected WithEvents lblDupMsg as Label
    Protected WithEvents sortcol as Label
    Protected WithEvents srchPriceBasisCode as TextBox
    Protected WithEvents srchDescription as TextBox
    Protected WithEvents srchUpdBy as TextBox
    Protected WithEvents srchStatus as DropDownList

    Protected objCMSetup As New agri.CM.clsSetup()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()

    Dim strOpCdGet As String = "CM_CLSSETUP_PRICEBASIS_GET"
    Dim strOpCdAdd As String = "CM_CLSSETUP_PRICEBASIS_ADD"
    Dim strOpCdUpd As String = "CM_CLSSETUP_PRICEBASIS_UPD"
    Dim objPriceBasisDs As New Object()

    Dim intErrNo As Integer
    Dim strParam As String = ""

    Dim objAR As New agri.GlobalHdl.clsAccessRights() 
    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim intCMAR As Integer

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
            If SortExpression.text = "" Then
                SortExpression.text = "PriceBasisCode"
                sortcol.text = "asc"
            End If
            If Not Page.IsPostBack Then
                BindSearchList() 
                BindGrid() 
                BindPageList()
            End If
        End IF
    End Sub

    Sub srchBtn_Click(sender As Object, e As System.EventArgs) 
        EventData.CurrentPageIndex = 0
        EventData.EditItemIndex = -1
        BindGrid() 
        BindPageList()
    End Sub 

    Sub BindGrid() 
        Dim PageNo as Integer 
        EventData.DataSource = LoadData
        EventData.DataBind()
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
        StatusList.Items.Add(New ListItem(objCMSetup.mtdGetPriceBasisStatus(objCMSetup.EnumPriceBasisStatus.Active), objCMSetup.EnumPriceBasisStatus.Active))
        StatusList.Items.Add(New ListItem(objCMSetup.mtdGetPriceBasisStatus(objCMSetup.EnumPriceBasisStatus.Deleted), objCMSetup.EnumPriceBasisStatus.Deleted))
    End Sub 

    Sub BindSearchList()
        srchStatus.Items.Add(New ListItem(objCMSetup.mtdGetPriceBasisStatus(objCMSetup.EnumPriceBasisStatus.Active), objCMSetup.EnumPriceBasisStatus.Active))
        srchStatus.Items.Add(New ListItem(objCMSetup.mtdGetPriceBasisStatus(objCMSetup.EnumPriceBasisStatus.Deleted), objCMSetup.EnumPriceBasisStatus.Deleted ))
        srchStatus.Items.Add(New ListItem(objCMSetup.mtdGetPriceBasisStatus(objCMSetup.EnumPriceBasisStatus.All), objCMSetup.EnumPriceBasisStatus.All))
        srchStatus.SelectedIndex = 0
    End Sub

    Protected Function LoadData() As DataSet
        Dim strSearch As String = ""
        Dim strSort As String = ""
        Dim strParam As String
        Dim intCnt As Integer

        If Trim(srchPriceBasisCode.text) <> "" Then
            strSearch = strSearch & "and pb.PriceBasisCode like '" & Trim(srchPriceBasisCode.text) & "%' " 
        End If
        
        If Trim(srchDescription.text) <> "" Then
            strSearch = strSearch & "and pb.Description like '" & Trim(srchDescription.text) & "%' "
        End If
        
        If srchStatus.SelectedItem.Value <> CInt(objCMSetup.EnumCurrencyStatus.All) Then
            strSearch = strSearch & "and pb.Status = '" & srchStatus.SelectedItem.Value & "' "
        End If

        If Trim(srchUpdBy.text) <> "" Then
            strSearch = strSearch & "and pb.UpdateID like '" & Trim(srchUpdBy.text) & "%' "
        End If

        strSort = "order by " & Trim(SortExpression.text) & " " & SortCol.text
        strParam = strSearch & "|" & strSort

        Try
            intErrNo = objCMSetup.mtdGetMasterList(strOpCdGet, strParam, 0, objPriceBasisDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_SETUP_PRICEBASISLIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Setup/CM_Setup_PriceBasisList.aspx")
        End Try

        If objPriceBasisDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objPriceBasisDs.Tables(0).Rows.Count - 1
                objPriceBasisDs.Tables(0).Rows(intCnt).Item("PriceBasisCode") = Trim(objPriceBasisDs.Tables(0).Rows(intCnt).Item("PriceBasisCode"))
                objPriceBasisDs.Tables(0).Rows(intCnt).Item("Description") = Trim(objPriceBasisDs.Tables(0).Rows(intCnt).Item("Description"))
                objPriceBasisDs.Tables(0).Rows(intCnt).Item("Status") = Trim(objPriceBasisDs.Tables(0).Rows(intCnt).Item("Status"))
                objPriceBasisDs.Tables(0).Rows(intCnt).Item("UpdateID") = Trim(objPriceBasisDs.Tables(0).Rows(intCnt).Item("UpdateID"))
                objPriceBasisDs.Tables(0).Rows(intCnt).Item("UserName") = Trim(objPriceBasisDs.Tables(0).Rows(intCnt).Item("UserName"))
            Next
        End If
        Return objPriceBasisDs
    End Function





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
        sortcol.text = IIF(sortcol.text = "asc", "desc", "asc")
        EventData.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid() 
    End Sub
 
    Sub DEDR_Edit(Sender As Object, e As DataGridCommandEventArgs)
        Dim txtEditText As TextBox
        Dim ddlList As Dropdownlist
        Dim lbUpdbutton As linkbutton
        
        blnUpdate.text = True
        EventData.EditItemIndex = CInt(e.Item.ItemIndex)

        BindGrid() 
        BindStatusList(EventData.EditItemIndex)

        txtEditText = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("Status")
        Select Case cint(txtEditText.text) = objCMSetup.EnumPriceBasisStatus.Active
            Case True
                Statuslist.selectedindex = 0
                txtEditText = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("PriceBasisCode")
                txtEditText.readonly = true
                lbUpdbutton = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("Delete")
                lbUpdbutton.Text = "Delete"
                lbUpdbutton.Attributes("onclick") = "javascript:return ConfirmAction('delete');" 
            Case False
                Statuslist.selectedindex = 1
                txtEditText = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("PriceBasisCode")
                txtEditText.Enabled = False
                txtEditText = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("Description")
                txtEditText.Enabled = False
                txtEditText = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("UpdateDate")
                txtEditText.Enabled = False
                txtEditText = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("UserName")
                txtEditText.Enabled = False
                ddlList = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("StatusList")
                ddlList.Enabled = False
                lbUpdbutton = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("Update")
                lbUpdbutton.Visible = False
                lbUpdbutton = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("Delete")
                lbUpdbutton.Text = "Undelete"
        End Select  
    End Sub

    Sub DEDR_Update(Sender As Object, E As DataGridCommandEventArgs)
        Dim txtEditText As TextBox
        Dim ddlList As Dropdownlist
        Dim strPriceBasisCode As String
        Dim strDescription As String
        Dim strStatus As String
        Dim blnDupKey As Boolean = False
        Dim lblMsg as label
        Dim strCreateDate As String
        Dim blnIsUpdate As Boolean

        If blnUpdate.text = "false" Then
            blnIsUpdate = False
        Else   
            blnIsUpdate = True
        End If
 
        txtEditText = E.Item.FindControl("PriceBasisCode")
        strPriceBasisCode = txtEditText.Text
        txtEditText = E.Item.FindControl("Description")
        strDescription = txtEditText.Text
        ddlList = E.Item.FindControl("StatusList")
        strStatus = ddllist.Selecteditem.Value

        strParam =  strPriceBasisCode & chr(9) & _
                    strDescription & chr(9) & _
                    strStatus 
        Try
            intErrNo = objCMSetup.mtdUpdPriceBasis(strOpCdGet, _
                                                   strOpCdAdd, _
                                                   strOpCdUpd, _
                                                   strCompany, _
                                                   strLocation, _
                                                   strUserId, _
                                                   strParam, _
                                                   blnDupKey, _
                                                   blnIsUpdate)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CM_SETUP_PRICEBASIS_SAVE&errmesg=" & lblErrMessage.Text & "&redirect=CM/Setup/CM_Setup_PriceBasisList.aspx")
        End Try
        
        If blnDupKey Then
            lblMsg = E.Item.FindControl("lblDupMsg")
            lblMsg.Visible = True
        Else
            EventData.EditItemIndex = -1
            BindGrid() 
        End If

    End Sub

    Sub DEDR_Cancel(Sender As Object, E As DataGridCommandEventArgs)
        If  CInt(e.Item.ItemIndex) = 0 and EventData.Items.Count = 1 and not EventData.CurrentPageIndex = 0 then
            EventData.CurrentPageIndex = EventData.Pagecount - 2 
            BindGrid()
            BindPageList()
        End If
        EventData.EditItemIndex = -1
        BindGrid() 
    End Sub

    Sub DEDR_Delete(Sender As Object, E As DataGridCommandEventArgs)

        Dim txtEditText As TextBox
        Dim strPriceBasisCode As String
        Dim strDescription As String
        Dim strStatus As String
        Dim blnDupKey As Boolean = False
        Dim strCreateDate As String

        txtEditText = E.Item.FindControl("PriceBasisCode")
        strPriceBasisCode = txtEditText.Text
        txtEditText = E.Item.FindControl("Description")
        strDescription = txtEditText.Text
        txtEditText = E.Item.FindControl("Status")
        strStatus = IIF(txtEditText.Text = objCMSetup.EnumPriceBasisStatus.Active, _
                        objCMSetup.EnumPriceBasisStatus.Deleted, _
                        objCMSetup.EnumPriceBasisStatus.Active )

        strParam =  strPriceBasisCode & chr(9) & _
                    "" & chr(9) & _
                    strStatus
        Try
            intErrNo = objCMSetup.mtdUpdPriceBasis(strOpCdGet, _
                                                   strOpCdAdd, _
                                                   strOpCdUpd, _
                                                   strCompany, _
                                                   strLocation, _
                                                   strUserId, _
                                                   strParam, _
                                                   False, _
                                                   True)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CM_SETUP_PRICEBASIS_DEL&errmesg=" & lblErrMessage.Text & "&redirect=CM/Setup/CM_Setup_PriceBasisList.aspx")
        End Try
      
        EventData.EditItemIndex = -1
        BindGrid() 

    End Sub

    Sub DEDR_Add(Sender As Object, E As ImageClickEventArgs)
        Dim dataSet as DataSet = LoadData
        Dim newRow as DataRow
        Dim lbUpdbutton as LinkButton
        Dim validateCode as RequiredFieldValidator
        Dim validateDesc as RequiredFieldValidator

        blnUpdate.text = "false"

        newRow = dataSet.Tables(0).NewRow()
        newRow.Item("PriceBasisCode") = ""
        newRow.Item("Description") = ""
        newRow.Item("Status") = 1
        newRow.Item("CreateDate") = DateTime.now()
        NewRow.Item("UpdateDate") = DateTime.now()
        NewRow.Item("UserName") = ""
        dataSet.Tables(0).Rows.Add(newRow)
        
        EventData.DataSource = dataSet
        EventData.DataBind()
        BindPageList()

        EventData.CurrentPageIndex = EventData.PageCount - 1
        lstDropList.SelectedIndex = EventData.CurrentPageIndex
        EventData.DataBind()
        EventData.EditItemIndex = EventData.Items.Count -1
        EventData.DataBind()
        BindStatusList(EventData.EditItemIndex)

        lbUpdbutton = EventData.Items.Item(CInt(EventData.EditItemIndex)).FindControl("Delete")
        lbUpdbutton.visible = False   

    End Sub

End Class
