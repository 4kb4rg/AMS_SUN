Imports System
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic


Public Class BD_trx_WSMechHour : Inherits Page

    Protected WithEvents EventData As DataGrid
    Protected WithEvents lblTracker As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents BlkCodeList As DropDownList
    Protected WithEvents lstEmpCode As DropDownList
    Protected WithEvents lstWorkCode As DropDownList
    Protected WithEvents txtSrchEmpCode As TextBox
    Protected WithEvents SortExpression As Label
    Protected hidIsUpdate As HtmlInputHidden
    Protected hidMechHourOriValue As HtmlInputHidden
    Protected hidAddVoteOriValue As HtmlInputHidden
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblDupMsg As Label
    Protected WithEvents sortcol As Label
    Protected WithEvents SeedID As TextBox
    Protected WithEvents BlkCode As TextBox
    Protected WithEvents txtSrchWorkCode As TextBox
    Protected WithEvents CreateDate As TextBox
    Protected WithEvents UpdateDate As TextBox
    Protected WithEvents lblTitle As Label
    Protected WithEvents lblMonthYear As Label
    Protected WithEvents lblSeedID As Label
    Protected WithEvents lblEmpCode As Label
    Protected WithEvents lblWorkCode As Label
    Protected WithEvents lblLocCode As Label
    Protected WithEvents lblStatus As Label
    Protected WithEvents lblPleaeEnter As Label
    Protected WithEvents lblUserID As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents lblErrMsg As Label
    Protected ibNew As ImageButton
    Protected lblBgtStatus As Label
    Protected lblPeriodID As Label

    Protected objBD As New agri.BD.clsTrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objGL As New agri.GL.clsSetup()
    Dim objBDTrx As New agri.BD.clstrx()
    Dim objWSSetup As New agri.WS.clsSetup()
    Dim objBDSetup As New agri.BD.clsSetup()
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Dim strOppCd_GET As String = "BD_CLSTRX_WSMECHHOUR_LIST_GET"
    Dim strOppCd_ADD As String = "BD_CLSTRX_WSMECHHOUR_LIST_ADD"
    Dim strOppCd_UPD As String = "BD_CLSTRX_WSMECHHOUR_LIST_UPD"
    Dim strOppCd_DEL As String = "BD_CLSTRX_WSMECHHOUR_LIST_DEL"

    Dim objDataSet As New Object()
    Dim objLangCapDs As New Object()

    Dim intErrNo As Integer
    Dim strParam As String = ""
    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim intADAR As Integer
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strValidateMonthYear As String

    Dim DocTitleTag As String
    Dim SeedIDTag As String
    Dim BlkCodeTag As String
    Dim QtyTag As String
    Dim LocCodeTag As String
    Dim StatusTag As String
    Dim CreateDateTag As String
    Dim UpdateDateTag As String
    Dim UpdateIDTag As String
    Dim intConfigsetting As Integer
    Dim strSqlConn As String
    Dim strLocType as String

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        intADAR = Session("SS_ADAR")
        strAccMonth = Session("SS_GLACCMONTH")
        strAccYear = Session("SS_GLACCYEAR")
        intConfigsetting = Session("SS_CONFIGSETTING")
        strLocType = Session("SS_LOCTYPE")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumADAccessRights.ADBudgeting), intADAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")

        Else
            onload_GetLangCap()
            GetActivePeriod()

            If SortExpression.Text = "" Then
                SortExpression.Text = "EmpCode"
                sortcol.Text = "ASC"
            End If

            If Not Page.IsPostBack Then
                BindGrid()
                BindPageList()
            End If
            
            If CInt(lblBgtStatus.Text) = objBDSetup.EnumPeriodStatus.AddVote Then ibNew.Visible = False
        End If
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblTitle.Text = "WORKSHOP MECHANIC HOUR LIST"
        DocTitleTag = lblTitle.Text
    End Sub

    Sub GetEntireLangCap()
        Dim strOpCode_BussTerm As String = "PWSYSTEM_CLSLANGCAP_BUSSTERM_GET"
        Dim strParam As String
        Dim intErrNo As Integer

        strParam = strLangCode
        Try
            intErrNo = objLangCap.mtdGetBussTerm(strOpCode_BussTerm, strCompany, strLocation, strUserId, _
                                                 strAccMonth, strAccYear, objLangCapDs, strParam)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BD_TRX_NURSERY_SEED_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_Trx_Nursery_Seed.aspx")
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

    Protected Function LoadData() As DataSet
        Dim UpdateBy As String
        Dim strSrchEmpCode, strSrchWorkCode As String
        Dim strParam As String
        Dim SearchStr As String
        Dim sortitem As String
        Dim intCnt As Integer

        If txtSrchEmpCode.Text = "" Then
            strSrchEmpCode = "%"
        Else
            strSrchEmpCode = Trim(txtSrchEmpCode.Text) & "%"
        End If

        If txtSrchWorkCode.Text = "" Then
            strSrchWorkCode = "%"
        Else
            strSrchWorkCode = Trim(txtSrchWorkCode.Text) & "%"
        End If

        SearchStr = " AND BD.LocCode LIKE '" & Trim(strLocation) & "'" & _
                    " AND BD.EmpCode LIKE '" & strSrchEmpCode & "'" & _
                    " AND BD.WorkCode LIKE '" & strSrchWorkCode & "'" & _
                    " AND BD.PeriodID = " & CInt(lblPeriodID.Text)

        sortitem = " ORDER BY " & SortExpression.Text & " " & sortcol.Text
        strParam = sortitem & "|" & SearchStr

        Try
            strOppCd_GET = "BD_CLSTRX_WSMECHHOUR_LIST_GET"
            intErrNo = objBD.mtdGetMechanicHour(strOppCd_GET, strParam, objDataSet)

        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_NURSERY_SEED&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_Trx_NURSERY_SEED.aspx")
        End Try

        If objDataSet.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objDataSet.Tables(0).Rows.Count - 1
                With objDataSet.Tables(0).Rows(intCnt)
                    .Item("LocCode") = Trim(.Item("LocCode"))
                    .Item("PeriodID") = Trim(.Item("PeriodID"))
                    .Item("EmpCode") = Trim(.Item("EmpCode"))
                    .Item("WorkCode") = Trim(.Item("WorkCode"))
                    .Item("MechHour") = Trim(.Item("MechHour"))
                    .Item("AddVote") = Trim(.Item("AddVote"))
                    .Item("CreateDate") = Trim(.Item("CreateDate"))
                    .Item("UpdateDate") = Trim(.Item("UpdateDate"))
                    .Item("UserName") = Trim(.Item("UserName"))
                End With
            Next
        End If

        Return objDataSet
    End Function


    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        EventData.CurrentPageIndex = 0
        EventData.EditItemIndex = -1
        BindGrid()
        BindPageList()
    End Sub

    Sub BindPageList()
        Dim count As Integer = 1
        Dim arrDList As New ArrayList()

        While Not count = EventData.PageCount + 1
            arrDList.Add("Page " & count)
            count = count + 1
        End While

        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = EventData.CurrentPageIndex
    End Sub

    Sub BindGrid()
        Dim PageNo As Integer
        Dim PageCount As Integer
        Dim dsData As DataSet

        dsData = LoadData()
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
        lblTracker.Text = "Page " & PageNo & " of " & EventData.PageCount
    End Sub


    Protected Function BindEmpCodeList(ByVal intIndex As Integer)
        Dim intcnt As Integer
        Dim intSelIndex As Integer
        Dim lbl As Label

        lstEmpCode = EventData.Items.Item(intIndex).FindControl("lstEmpCode")
        lbl = EventData.Items.Item(intIndex).FindControl("lblEmpCode")

        objDataSet = LoadEmpCodeData()

        If objDataSet.Tables(0).Rows.Count > 0 Then
            For intcnt = 0 To objDataSet.Tables(0).Rows.Count - 1
                objDataSet.Tables(0).Rows(intcnt).Item("BlkCode") = Trim(objDataSet.Tables(0).Rows(intcnt).Item("BlkCode"))
                If Trim(objDataSet.Tables(0).Rows(intcnt).Item("BlkCode")) = lbl.Text Then
                    intSelIndex = intcnt
                End If
            Next
        End If

        lstEmpCode.DataSource = objDataSet
        lstEmpCode.DataValueField = "BlkCode"
        lstEmpCode.DataTextField = "BlkCode"
        lstEmpCode.DataBind()
        lstEmpCode.SelectedIndex = intSelIndex
    End Function

    Private Function BindNewEmpCodeList(ByVal intIndex As Integer)
        Dim intcnt As Integer
        Dim intSelIndex As Integer
        Dim lbl As Label
        Dim drinsert As DataRow

        lstEmpCode = EventData.Items.Item(intIndex).FindControl("lstEmpCode")
        lbl = EventData.Items.Item(intIndex).FindControl("lblEmpCode")

        objDataSet = LoadEmpCodeData()

        drinsert = objDataSet.Tables(0).NewRow()
        drinsert(0) = "Please select Employee Code" 
        objDataSet.Tables(0).Rows.InsertAt(drinsert, 0)

        lstEmpCode.DataSource = objDataSet.Tables(0)
        lstEmpCode.DataValueField = "EmpCode"
        lstEmpCode.DataTextField = "EmpCode"
        lstEmpCode.DataBind()
    End Function

    Private Function BindNewWorkCodeList(ByVal intIndex As Integer)
        Dim intcnt As Integer
        Dim intSelIndex As Integer
        Dim lbl As Label
        Dim drinsert As DataRow

        lstWorkCode = EventData.Items.Item(intIndex).FindControl("lstWorkCode")
        lbl = EventData.Items.Item(intIndex).FindControl("lblWorkCode")

        objDataSet = LoadWorkCodeData()

        drinsert = objDataSet.Tables(0).NewRow()
        drinsert(0) = "Please select Work Code" 
        objDataSet.Tables(0).Rows.InsertAt(drinsert, 0)

        lstWorkCode.DataSource = objDataSet.Tables(0)
        lstWorkCode.DataValueField = "WorkCode"
        lstWorkCode.DataTextField = "WorkCode"
        lstWorkCode.DataBind()
    End Function

    Private Function LoadEmpCodeData() As DataSet
        strOppCd_GET = "BD_CLSTRX_WSMECHHOUR_EMPCODE_GET"
        
        Try
            intErrNo = objBDTrx.mtdGetMechanicEmpCode(strOppCd_GET, strLocation, objDataSet)

        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_NURSERY_SEED&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_Nursery_Seed.aspx")
        End Try

        Return objDataSet
    End Function

    Private Function LoadWorkCodeData() As DataSet
        strOppCd_GET = "BD_CLSTRX_WSMECHHOUR_WORKCODE_GET"
        
        Try
            intErrNo = objBDTrx.mtdGetMechanicWorkCode(strOppCd_GET, objWSSetup.EnumWorkCodeStatus.Active, objDataSet)

        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_NURSERY_SEED&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_Nursery_Seed.aspx")
        End Try

        Return objDataSet
    End Function

    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            EventData.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        EventData.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        sortcol.Text = IIf(sortcol.Text = "ASC", "DESC", "ASC")
        EventData.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
    End Sub

    Sub btnPreview_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim strSrchEmpCode As String
        Dim strSrchWorkCode As String
        Dim strPeriodID As String = lblPeriodID.Text

        If txtSrchEmpCode.Text = "" Then
            strSrchEmpCode = "%"
        Else
            strSrchEmpCode = Trim(txtSrchEmpCode.Text) & "%"
        End If

        If txtSrchWorkCode.Text = "" Then
            strSrchWorkCode = "%"
        Else
            strSrchWorkCode = Trim(txtSrchWorkCode.Text) & "%"
        End If

        Response.Write("<Script Language=""JavaScript"">window.open(""../reports/BD_Rpt_WSMechHourList.aspx" & _
                       "?EmpCode=" & strSrchEmpCode & _
                       "&WorkCode=" & strSrchWorkCode & _
                       "&PeriodID=" & strPeriodID & _
                       "&DocTitleTag=WORKSHOP MECHANIC HOUR LIST" & _   
                       """, null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")
    End Sub

    Sub btnPrevNext_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).CommandArgument
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


    Sub DEDR_Edit(ByVal Sender As Object, ByVal e As DataGridCommandEventArgs)
        Dim txtEmpCode, txtWorkCode, txtMechHour, txtAddVote As TextBox
        Dim lstEmpCode, lstWorkCode As DropDownList
        Dim Updbutton As LinkButton
        Dim lst As DropDownList
        Dim lbl As Label
        Dim strBlkCode As String

        hidIsUpdate.Value = "True"
        EventData.EditItemIndex = CInt(e.Item.ItemIndex)
        BindGrid()

        With EventData.Items.Item(CInt(e.Item.ItemIndex))
            txtEmpCode = .FindControl("txtEmpCode")
            txtWorkCode = .FindControl("txtWorkCode")
            txtMechHour = .FindControl("txtMechHour")
            txtAddVote = .FindControl("txtAddVote")
            lstEmpCode = .FindControl("lstEmpCode")
            lstWorkCode = .FindControl("lstWorkCode")
        End With
        
        txtEmpCode.ReadOnly = True
        txtWorkCode.ReadOnly = True
        lstEmpCode.Visible = False
        lstWorkCode.Visible = False
        
        If CInt(lblBgtStatus.Text) = objBDSetup.EnumPeriodStatus.AddVote Then
            txtMechHour.ReadOnly = True
            txtAddVote.ReadOnly = False
            hidAddVoteOriValue.Value = txtAddVote.Text
        Else
            txtMechHour.ReadOnly = False
            txtAddVote.ReadOnly = True
            hidMechHourOriValue.Value = txtMechHour.Text
        End If
        
        Updbutton = EventData.Items.Item(CInt(e.Item.ItemIndex)).FindControl("Delete")
        Updbutton.Text = "Delete"
        Updbutton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"

    End Sub

    Sub DEDR_Update(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim txtEmpCode, txtWorkCode, txtMechHour, txtAddVote As TextBox
        Dim lstEmpCode, lstWorkCode As DropDownList
        Dim strEmpCode, strWorkCode, strMechHour, strAddVote As String
        Dim lblDupMsg As Label
        Dim lblErrMsg As Label
        Dim blnIsUpdate As Boolean
        Dim strOpCodeResetMechHour As String = "BD_CLSTRX_WSMECHHOUR_RESET_MECHHOUR"
        Dim strOpCodeResetAddVote As String = "BD_CLSTRX_WSMECHHOUR_RESET_ADDVOTE"

        If hidIsUpdate.Value = "False" Then
            lstEmpCode = E.Item.FindControl("lstEmpCode")
            strEmpCode = Trim(lstEmpCode.SelectedItem.Value)    
        Else
            txtEmpCode = E.Item.FindControl("txtEmpCode")
            strEmpCode = Trim(txtEmpCode.Text)
        End If
        
        If strEmpCode = "Please select Employee Code" Then
            Response.Write("<script language='javascript'>alert('Please select Employee Code')</script>")
            Exit Sub
        End If

        If hidIsUpdate.Value = "False" Then
            lstWorkCode = E.Item.FindControl("lstWorkCode")
            strWorkCode = Trim(lstWorkCode.SelectedItem.Value)
        Else
            txtWorkCode = E.Item.FindControl("txtWorkCode")
            strWorkCode = Trim(txtWorkCode.Text)
        End If

        If strWorkCode = "Please select Work Code" Then
            Response.Write("<script language='javascript'>alert('Please select Work Code')</script>")
            Exit Sub
        End If

        txtMechHour = E.Item.FindControl("txtMechHour")
        strMechHour = Trim(txtMechHour.Text)
        
        If strMechHour = "" Then
            Response.Write("<script language='javascript'>alert('Please enter Mechanic Hour')</script>")
            Exit Sub
        Else
            If Not IsNumeric(strMechHour) Then
                Response.Write("<script language='javascript'>alert('Mechanic Hour must of numeric value')</script>")
                Exit Sub
            Else
                If CDbl(strMechHour) <= 0 Then
                    Response.Write("<script language='javascript'>alert('Mechanic Hour value must be more than zero')</script>")
                    Exit Sub 
                End If
            End If
        End If

        txtAddVote = E.Item.FindControl("txtAddVote")
        strAddVote = Trim(txtAddVote.Text)

        If CInt(lblBgtStatus.Text) = objBDSetup.EnumPeriodStatus.AddVote Then
            If strAddVote = "" Then
                Response.Write("<script language='javascript'>alert('Please enter Add Vote')</script>")
                Exit Sub
            Else
                If Not IsNumeric(strAddVote) Then
                    Response.Write("<script language='javascript'>alert('Add Vote must of numeric value')</script>")
                    Exit Sub
                Else
                    If CDbl(strAddVote) <= 0 Then
                        Response.Write("<script language='javascript'>alert('Add Vote value must be more than zero')</script>")
                        Exit Sub 
                    End If
                End If
            End If 
        End If

        blnIsUpdate = IIf(hidIsUpdate.Value = "True", True, False)
        strParam = strEmpCode & "|" & strWorkCode & "|" & strMechHour & "|" & GetPeriodID() & "|" & strAddVote
        
        Try
            intErrNo = objBD.mtdUpdWSMechHour(strOppCd_ADD, strOppCd_UPD, strOppCd_GET, strCompany, _
                                              strLocation, strUserId, strParam, blnIsUpdate)

            If intErrNo <> 0 Then
                Response.Write("<script language='javascript'>alert('This Employee Code and Work Code combination already exist')</script>")
                Exit Sub
            End If

        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=UPDATE_NURSERY_SEED&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_Trx_NurserySeed.aspx")
        End Try

        If blnIsUpdate = True Then
            If CInt(lblBgtStatus.Text) = objBDSetup.EnumPeriodStatus.AddVote Then
                If CDec(hidAddVoteOriValue.Value) <> CDec(strAddVote) Then
                    intErrNo = objBD.mtdResetWSMechHourDist(strOpCodeResetAddVote, strLocation, GetPeriodID(), _
                                                            strEmpCode, strWorkCode, strUserId)
                End If
            Else
                If CDec(hidMechHourOriValue.Value) <> CDec(strMechHour) Then
                    intErrNo = objBD.mtdResetWSMechHourDist(strOpCodeResetMechHour, strLocation, GetPeriodID(), _
                                                            strEmpCode, strWorkCode, strUserId)
                End If
            End If
        End If

        EventData.EditItemIndex = -1
        BindGrid()
    End Sub

    Private Function GetPeriodID() As String
        Dim dsPeriod As New DataSet()
        strParam = "|||||" & objBDSetup.EnumPeriodStatus.Active & "|" & strLocation & "|"

        Try
            intErrNo = objBDSetup.mtdGetPeriodList("BD_CLSSETUP_BGTPERIOD_GET", strParam, dsPeriod)

        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_MATURECROPDIST_GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/trx/BD_trx_MatureCropDist_Details.aspx")
        End Try

        If dsPeriod.Tables(0).Rows.Count > 0 Then
            Return dsPeriod.Tables(0).Rows(0).Item("PeriodID").ToString
        Else
            Response.Redirect("../../BD/Setup/BD_setup_Periods.aspx")
        End If
    End Function

    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        If CInt(E.Item.ItemIndex) = 0 And EventData.Items.Count = 1 And Not EventData.CurrentPageIndex = 0 Then
            EventData.CurrentPageIndex = EventData.PageCount - 2
            BindGrid()
            BindPageList()
        End If
        EventData.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim txtEmpCode, txtWorkCode As TextBox
        Dim intError As Integer

        txtEmpCode = E.Item.FindControl("txtEmpCode")
        txtWorkCode = E.Item.FindControl("txtWorkCode")

        Try
            intErrNo = objBD.mtdDelWSMechHour(strOppCd_DEL, Trim(txtEmpCode.Text), Trim(txtWorkCode.Text))

        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=DELETE_NURSERY_SEED&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_Trx_Nursery_=Seed.aspx")
        End Try

        EventData.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub DEDR_Add(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim dataSet As dataSet = LoadData()
        Dim newRow As DataRow
        Dim Updbutton As LinkButton
        Dim txtEmpCode, txtWorkCode, txtAddVote As TextBox

        hidIsUpdate.Value = "False"
        newRow = dataSet.Tables(0).NewRow()
        newRow.Item("LocCode") = ""
        newRow.Item("PeriodID") = 0
        newRow.Item("EmpCode") = ""
        newRow.Item("WorkCode") = ""
        newRow.Item("MechHour") = 0
        newRow.Item("AddVote") = 0
        newRow.Item("CreateDate") = DateTime.Now()
        newRow.Item("UpdateDate") = DateTime.Now()
        newRow.Item("UserName") = ""
        dataSet.Tables(0).Rows.Add(newRow)

        EventData.DataSource = dataSet
        EventData.DataBind()
        BindPageList()

        EventData.CurrentPageIndex = EventData.PageCount - 1
        lstDropList.SelectedIndex = EventData.CurrentPageIndex
        EventData.DataBind()
        EventData.EditItemIndex = EventData.Items.Count - 1
        EventData.DataBind()
        BindNewEmpCodeList(EventData.EditItemIndex)
        BindNewWorkCodeList(EventData.EditItemIndex)

        Updbutton = EventData.Items.Item(CInt(EventData.EditItemIndex)).FindControl("Delete")
        Updbutton.Visible = False
        
        With EventData.Items.Item(CInt(EventData.EditItemIndex))
            txtEmpCode = .FindControl("txtEmpCode")
            txtWorkCode = .FindControl("txtWorkCode")
            txtAddVote = .FindControl("txtAddVote")
        End With
        
        txtEmpCode.Visible = False
        txtWorkCode.Visible = False
        txtAddVote.ReadOnly = True
    End Sub

    
    Private Sub GetActivePeriod()
        Dim strOppCd_GET As String = "BD_CLSSETUP_BGTPERIOD_GET"
        Dim dsperiod As New DataSet()

        strParam = "|||||" & objBDSetup.EnumPeriodStatus.Active & "|" & strLocation & "|"

        Try
            intErrNo = objBDSetup.mtdGetPeriodList(strOppCd_GET, strParam, dsperiod)

        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=BD_TRX_MATURECROP_YEARLIST_GET_BUDGETACTIVEPERIODS&errmesg=" & lblErrMessage.Text & "&redirect=BD/Trx/BD_trx_MatureCrop_YearList.aspx")
        End Try

        If dsperiod.Tables(0).Rows.Count > 0 Then
            lblBgtStatus.Text = dsperiod.Tables(0).Rows(0).Item("Status") 
            lblPeriodID.Text = dsperiod.Tables(0).Rows(0).Item("PeriodID")
        Else
            Response.Redirect("../../BD/Setup/BD_setup_Periods.aspx")
        End If
    End Sub


End Class
