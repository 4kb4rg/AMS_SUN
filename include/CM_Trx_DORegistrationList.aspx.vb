
Imports System
Imports System.Data
Imports System.Collections 
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic

Public Class CM_Trx_DORegistrationList : Inherits Page

    Protected WithEvents dgLine As DataGrid
    Protected WithEvents lbDelete As LinkButton
    Protected WithEvents lblTracker As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents txtContractNo As TextBox
    Protected WithEvents txtDONo As TextBox
    Protected WithEvents txtLastUpdate As TextBox
    Protected WithEvents ddlStatus as DropDownList

    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblErrCtrDate As Label
    Protected WithEvents lblDateFormat As Label
    Protected WithEvents lblErrDelvPeriod As Label
    Protected WithEvents lblBillParty As Label
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label

    Protected WithEvents lstAccMonth As DropDownList
    Protected WithEvents lstAccYear As DropDownList

    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Protected objCMTrx As New agri.CM.clsTrx()
    Protected objCMSetup As New agri.CM.clsSetup()

    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objGLSetup As New agri.GL.clsSetup()
    Dim objWMTrx As New agri.WM.clsTrx()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objPUSetup As New agri.PU.clsSetup()
    Dim objGLtrx As New agri.GL.ClsTrx()

    Dim objContractDs As New Object()
    Dim objPriceBasisDs As New Object()
    Dim objBPDs As New Object()
    Dim objSysCfgDs As New Object()
    Dim objSellerDs As New Object()
    Dim objMatchDs As New Object()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim intCMAR As Integer
    Dim strDateSetting As String

    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim strLocType As String

    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objLangCapDs As New Object()
    Dim strSelAccMonth As String
    Dim strSelAccYear As String

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_ARACCMONTH")
        strAccYear = Session("SS_ARACCYEAR")
        intCMAR = Session("SS_CMAR")
        strDateSetting = Session("SS_DATEFMT")
        strLocType = Session("SS_LOCTYPE")
        strSelAccMonth = Session("SS_SELACCMONTH")
        strSelAccYear = Session("SS_SELACCYEAR")

        If strUserId = "" Then
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCMAccessRights.CMDORegistration), intCMAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            onload_GetLangCap()
            lblErrCtrDate.Visible = False
            lblErrDelvPeriod.Visible = False
            lblDateFormat.Visible = False
            If SortExpression.Text = "" Then
                SortExpression.Text = "cdo.DONo"
                SortCol.Text = "desc"
            End If
            If Not Page.IsPostBack Then
                If Session("SS_FILTERPERIOD") = "0" Then
                    lstAccMonth.SelectedValue = strAccMonth
                    BindAccYear(strAccYear)
                Else
                    lstAccMonth.SelectedValue = strSelAccMonth
                    BindAccYear(strSelAccYear)
                End If
                BindStatusList()
                BindGrid()
                BindPageList()
            End If
        End If
    End Sub

    Sub BindStatusList()
        ddlStatus.Items.Add(New ListItem(objCMTrx.mtdGetContractStatus(objCMTrx.EnumContractStatus.All), objCMTrx.EnumContractStatus.All))
        ddlStatus.Items.Add(New ListItem(objCMTrx.mtdGetContractStatus(objCMTrx.EnumContractStatus.Active), objCMTrx.EnumContractStatus.Active))
        ddlStatus.Items.Add(New ListItem(objCMTrx.mtdGetContractStatus(objCMTrx.EnumContractStatus.Closed), objCMTrx.EnumContractStatus.Closed))
        ddlStatus.Items.Add(New ListItem(objCMTrx.mtdGetContractStatus(objCMTrx.EnumContractStatus.Deleted), objCMTrx.EnumContractStatus.Deleted))
        ddlStatus.SelectedIndex = 1
    End Sub



    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        dgLine.CurrentPageIndex = 0
        dgLine.EditItemIndex = -1
        BindGrid()
        BindPageList()
    End Sub

    Sub BindGrid()
        Dim intCnt As Integer
        Dim lbButton As LinkButton
        Dim lbl As Label
        Dim lbl2 As Label
        Dim lbl3 As Label
        Dim lbl4 As Label



        Dim PageNo As Integer
        Dim PageCount As Integer
        Dim dsData As DataSet

        dsData = LoadData()
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
        lblTracker.Text = "Page " & PageNo & " of " & dgLine.PageCount

        For intCnt = 0 To dgLine.Items.Count - 1
            lbl = dgLine.Items.Item(intCnt).FindControl("lblStatus")
            Select Case CInt(Trim(lbl.Text))
                Case objCMTrx.EnumContractStatus.Active
                    lbButton = dgLine.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = True
                    lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                Case Else 
                    lbButton = dgLine.Items.Item(intCnt).FindControl("lbDelete")
                    lbButton.Visible = False
            End Select
        Next

        PageNo = dgLine.CurrentPageIndex + 1
        lblTracker.Text = "Page " & PageNo & " of " & dgLine.PageCount

    End Sub

    Sub BindPageList()
        Dim count As Integer = 1
        Dim arrDList As New ArrayList()

        While Not count = dgLine.PageCount + 1
            arrDList.Add("Page " & count)
            count = count + 1
        End While
        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = dgLine.CurrentPageIndex
    End Sub

    Sub GetDelvPeriod(ByVal pv_strInputDate As String, ByRef pr_strDelMonth As String, ByRef pr_strDelYear As String)
        Dim arrPeriod As Array

        arrPeriod = Split(pv_strInputDate, " ")
        Select Case Trim(LCase(arrPeriod(0)))
            Case "jan"
                pr_strDelMonth = "1"
            Case "feb"
                pr_strDelMonth = "2"
            Case "mar"
                pr_strDelMonth = "3"
            Case "apr"
                pr_strDelMonth = "4"
            Case "may"
                pr_strDelMonth = "5"
            Case "jun"
                pr_strDelMonth = "6"
            Case "jul"
                pr_strDelMonth = "7"
            Case "aug"
                pr_strDelMonth = "8"
            Case "sep"
                pr_strDelMonth = "9"
            Case "oct"
                pr_strDelMonth = "10"
            Case "nov"
                pr_strDelMonth = "11"
            Case "dec"
                pr_strDelMonth = "12"
        End Select
        pr_strDelYear = Trim(arrPeriod(1))
    End Sub

    Protected Function FormatShortDate(ByVal pv_strInputDate As String) As String
        Dim strDate As String
        Dim strMonth As String
        Dim strYear As String
        Dim strFormatDate As String
        Dim arrDate As Array

        arrDate = Split(pv_strInputDate, "/")
        strDate = Trim(arrDate(0))
        strMonth = Trim(arrDate(1))
        strYear = Trim(arrDate(2))

        strFormatDate = strMonth & "/" & strDate & "/" & strYear
        Return strFormatDate
    End Function

    Protected Function LoadData() As DataSet
        Dim strOpCd_GET As String = "CM_CLSTRX_DO_REG_GET"
        Dim strSearch As String = ""
        Dim strSort As String = ""
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim strDMY As String

        If lstAccMonth.SelectedItem.Value = "0" Then
            strAccMonth = "1','2','3','4','5','6','7','8','9','10','11','12"
        Else
            strAccMonth = lstAccMonth.SelectedItem.Value
        End If

        strAccYear = lstAccYear.SelectedItem.Value

        strSearch = strSearch & " cdo.LocCode = '" & Trim(strLocation) & "' and"
        If Trim(txtContractNo.Text) <> "" Then
            strSearch = strSearch & " cdo.ContractNo like '" & Trim(txtContractNo.Text) & "%' and"
        End If
        If Trim(txtDONo.Text) <> "" Then
            strSearch = strSearch & " cdo.DONo like '" & Trim(txtDONo.Text) & "%' and"
        End If
        If Trim(txtLastUpdate.Text) <> "" Then
            strSearch = strSearch & " cdo.LastUpdate like '" & Trim(txtLastUpdate.Text) & "%' and"
        End If
        If ddlStatus.SelectedItem.Value <> CInt(objCMTrx.EnumContractStatus.All) Then
            strSearch = strSearch & " cdo.Status = '" & ddlStatus.SelectedItem.Value & "' and"
        End If
        strSearch = strSearch & " cdo.AccMonth IN ('" & strAccMonth & "') AND cdo.AccYear = '" & strAccYear & "' and"

        strSearch = " where " & left(strSearch, len(strSearch) - 4)
 
        strSort = "order by " & Trim(SortExpression.Text) & " " & SortCol.Text
        strParam = strSearch & "|" & strSort

        Try
            intErrNo = objCMTrx.mtdGetDOReg(strOpCd_GET, strParam, 0, objContractDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_DOREGLIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Setup/CM_Trx_DORegistrationList.aspx")
        End Try

        Return objContractDs
    End Function


    Sub btnPrevNext_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).CommandArgument
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


    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            dgLine.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        dgLine.CurrentPageIndex = e.NewPageIndex
        BindGrid()

    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIf(SortCol.Text = "asc", "desc", "asc")
        dgLine.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
    End Sub



    Sub DEDR_Delete(ByVal Sender As Object, ByVal e As DataGridCommandEventArgs)
        Dim lbl As Label
        Dim strOpCd_GET As String = ""
        Dim strOpCd_ADD As String = ""
        Dim strOpCd_UPD As String = "CM_CLSTRX_DO_REG_UPD"
        Dim strOpCd_UPDQty as String = "CM_CLSTRX_DO_REG_REMQTY_UPD"
        Dim strParam As String = ""
        Dim intErrNo As Integer

        Dim strDONo As String = ""
        Dim strDODate As String = ""
        Dim strDOQty As String = ""
        Dim strNPWP As String = ""
        Dim strAddress As String = ""           
        Dim strContractNo As String = ""
        Dim strBillPartyCode As String = ""
        Dim strTerm As String = ""
        Dim strContractQty As String = ""
        Dim strRemQty As String = ""
        Dim dtUpdateDate As String = ""
        Dim strStatus As String = ""
        Dim strLocation As String = ""
        Dim strUpdateID As String = ""

        Dim strCMIDFormat As String
        Dim strNewYear As String = ""
        Dim strTranPrefix As String = "CMDO"
        Dim strHistYear As String = ""
        Dim strSearch As String = ""

        Dim strOppCd As String = "IN_CLSTRX_PURREQ_MOVEID"
        Dim strOppCd_Back As String = "IN_CLSTRX_PURREQ_BACKID"
        Dim strOppCd_GetID As String = "IN_CLSTRX_PURREQ_GETID"
        Dim objCMID As Object


        dgLine.EditItemIndex = CInt(e.Item.ItemIndex)

        lbl = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblDONo")
        strDONo = lbl.Text
        lbl = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblContractNo")
        strContractNo = lbl.Text
        lbl = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblUpdateDate")
        dtUpdateDate = lbl.Text
        lbl = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblStatus")
        strStatus = lbl.Text
        lbl = dgLine.Items.Item(CInt(e.Item.ItemIndex)).FindControl("lblUpdateID")
        strUpdateID = lbl.Text

        strParam = strDONo & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _            
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   dtUpdateDate & Chr(9) & _
                   objCMTrx.EnumContractStatus.Deleted & Chr(9) & _
                   strLocation & Chr(9) & _
                   strAccMonth & Chr(9) & _
                   strAccYear & Chr(9) & _
                   strUpdateID & Chr(9) & _
                   "" & Chr(9) & "" & Chr(9)& "" & Chr(9) & "" & Chr(9) & "" & Chr(9) & "" & Chr(9) & "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   ""

        Try
            intErrNo = objCMTrx.mtdUpdDOReg(strOpCd_GET, _
                                               strOpCd_ADD, _
                                               strOpCd_UPD, _
                                               strCompany, _
                                               strOppCd, _
                                               strLocation, _
                                               strUserId, _
                                               strParam, _
                                               False, _
                                               objCMID, _
                                               True,  _
                                               strTranPrefix)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_DOREGDET_DEL&errmesg=" & Exp.Message & "&redirect=CM/Setup/CM_Trx_DORegistrationList.aspx")
        End Try

        strSearch = "WHERE ContractNo like '%" & strContractNo & "'  " 

        strparam = strSearch & "|" & ""

        Try
            intErrNo = objCMTrx.mtdGetContract(strOpCd_UPDQty, strParam, 0, objContractDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_DORegistrationList.aspx")
        End Try



        dgLine.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub NewTBBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CM_Trx_DORegistrationDet.aspx")
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREG_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=CM/Setup/CM_Trx_ContractRegList.aspx")
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


    Sub BindAccYear(ByVal pv_strAccYear As String)
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer
        Dim intCnt As Integer = 0
        Dim intSelectedIndex As Integer
        Dim objAccYearDs As New Object
        Dim dr As DataRow
        Dim strOpCd As String = "ADMIN_CLSACCPERIOD_CONFIG_GET"

        strParamName = "LOCCODE|SEARCHSTR|SORTEXP"
        strParamValue = strLocation & "||Order By HD.AccYear"

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCd, _
                                                strParamName, _
                                                strParamValue, _
                                                objAccYearDs)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GET_ACCYEAR&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To objAccYearDs.Tables(0).Rows.Count - 1
            objAccYearDs.Tables(0).Rows(intCnt).Item("AccYear") = Trim(objAccYearDs.Tables(0).Rows(intCnt).Item("AccYear"))
            objAccYearDs.Tables(0).Rows(intCnt).Item("UserName") = Trim(objAccYearDs.Tables(0).Rows(intCnt).Item("AccYear"))
            If objAccYearDs.Tables(0).Rows(intCnt).Item("AccYear") = pv_strAccYear Then
                intSelectedIndex = intCnt + 1
            End If
        Next intCnt

        lstAccYear.DataSource = objAccYearDs.Tables(0)
        lstAccYear.DataValueField = "AccYear"
        lstAccYear.DataTextField = "UserName"
        lstAccYear.DataBind()
        lstAccYear.SelectedIndex = intSelectedIndex - 1
    End Sub

    Sub dgLine_BindGrid(ByVal sender As Object, ByVal e As DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor='lightblue'")
            If e.Item.ItemType = ListItemType.AlternatingItem Then
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='e9e9e9'")
            Else
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='f2f2f2'")
            End If
        End If
    End Sub

End Class
