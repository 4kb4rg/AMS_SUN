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


Public Class CB_trx_StaffAdvanceList : Inherits Page

    Protected WithEvents dgList As DataGrid
    Protected WithEvents dgTrx As DataGrid
    Protected WithEvents lblTracker As Label
    Protected WithEvents lblErrMessage As Label

    Protected WithEvents txtStaff As TextBox
    Protected WithEvents txtDocID As TextBox
    Protected WithEvents ddlDocStatus As DropDownList
    Protected WithEvents lbAccCode As LinkButton

    Protected WithEvents srchUpdateBy As TextBox
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents srchStatusList As DropDownList
    Protected WithEvents lblBlkCode As Label
    Protected WithEvents lblVehCode As Label

    Protected WithEvents lstAccMonth As DropDownList
    Protected WithEvents lstAccYear As DropDownList
    Protected WithEvents hidInit As HtmlInputHidden

    Protected WithEvents lblRealization As Label

    Protected objGLTrx As New agri.GL.ClsTrx()
    Protected objGLSetup As New agri.GL.clsSetup()
    Dim objLangCap As New agri.PWSystem.clsLangCap()

    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objPU As New agri.PU.clsTrx()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intGLAR As Integer
    Dim strLocType As String
    Dim strLocLevel As String
    Dim strSelAccMonth As String
    Dim strSelAccYear As String

    Dim objBudgetDs As New DataSet()
    Dim objLangCapDs As New Object()

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strAccMonth = Session("SS_GLACCMONTH")
        strAccYear = Session("SS_GLACCYEAR")
        strLangCode = Session("SS_LANGCODE")
        intGLAR = Session("SS_CBAR")
        strLocType = Session("SS_LOCTYPE")
        strLocLevel = Session("SS_LOCLEVEL")
        strSelAccMonth = Session("SS_SELACCMONTH")
        strSelAccYear = Session("SS_SELACCYEAR")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCBAccessRights.CBInterAdj), intGLAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            If SortExpression.Text = "" Then
                SortExpression.Text = "AccCode"
            End If

            onload_GetLangCap()

            If Not Page.IsPostBack Then
                If Session("SS_FILTERPERIOD") = "0" Then
                    lstAccMonth.SelectedValue = strAccMonth
                    BindAccYear(strAccYear)
                Else
                    lstAccMonth.SelectedValue = strSelAccMonth
                    BindAccYear(strSelAccYear)
                End If

                BindGrid()
            End If

        End If
    End Sub

    Sub dgTrx_BindGrid(ByVal sender As Object, ByVal e As DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor='lightblue'")
            If e.Item.ItemType = ListItemType.AlternatingItem Then
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='e9e9e9'")
            Else
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='f2f2f2'")
            End If
        End If
    End Sub

    Sub dgList_BindGrid(ByVal sender As Object, ByVal e As DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor='lightblue'")
            If e.Item.ItemType = ListItemType.AlternatingItem Then
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='e9e9e9'")
            Else
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='f2f2f2'")
            End If
        End If
    End Sub

    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        dgList.CurrentPageIndex = 0
        dgList.EditItemIndex = -1
        BindGrid()
    End Sub

    Sub BindGrid()
        Dim dsData As DataSet

        dsData = LoadData()
        dgList.DataSource = dsData
        dgList.DataBind()
    End Sub

    Sub srchTrxBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        dgTrx.CurrentPageIndex = 0
        dgTrx.EditItemIndex = -1
        BindGridTrx()
    End Sub

    Sub BindGridTrx()
        Dim dsData As DataSet
        Dim intCnt As Integer
        Dim lbl As Label

        dsData = LoadDataTrx()
        dgTrx.DataSource = dsData
        dgTrx.DataBind()

        For intCnt = 0 To dgTrx.Items.Count - 1
            lbl = dgTrx.Items.Item(intCnt).FindControl("lblDocID")
            If Trim(lbl.Text) = "TOTAL" Then
                lbl.Font.Bold = True
                lbl = dgTrx.Items.Item(intCnt).FindControl("lblDate")
                lbl.Visible = False
                lbl = dgTrx.Items.Item(intCnt).FindControl("lblAmount")
                lbl.Font.Bold = True
            End If
        Next

    End Sub

    Function Date_Validation(ByVal pv_strInputDate As String, ByVal pv_blnIsShortDate As Boolean) As String
        Dim objSysCfgDs As New Object
        Dim objActualDate As New Object
        Dim strDateFormat As String
        Dim strParam As String
        Dim intErrNo As Integer
        Dim strAcceptFormat As String

        strParam = "PWSYSTEM_CLSCONFIG_CONFIG_DATEFMT_GET"

        Try
            intErrNo = objSysCfg.mtdGetConfigInfo(strParam, _
                                                  strCompany, _
                                                  strLocation, _
                                                  strUserId, _
                                                  objSysCfgDs)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_DEPOSIT_GET_CONFIG&errmesg=" & Exp.Message & "&redirect=CB/trx/cb_trx_DepositList.aspx")
        End Try

        strDateFormat = objSysCfg.mtdGetDateFormat(objSysCfgDs.Tables(0).Rows(0).Item("Datefmt").Trim())

        If pv_blnIsShortDate Then
            Date_Validation = objGlobal.GetShortDate(strDateFormat, pv_strInputDate)
        Else
            If objGlobal.mtdValidInputDate(strDateFormat, _
                                           pv_strInputDate, _
                                           strAcceptFormat, _
                                           objActualDate) = True Then
                Date_Validation = objActualDate
            Else
                Date_Validation = ""
            End If
        End If
    End Function

    Protected Function LoadData() As DataSet
        Dim strOpCd As String = "HR_CLSSETUP_STAFF_GET_TRX"
        Dim dsResult As New Object
        Dim strSearch As String = " "
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer

        If lstAccMonth.SelectedItem.Value = "0" Then
            strAccMonth = "1','2','3','4','5','6','7','8','9','10','11','12"
        Else
            strAccMonth = lstAccMonth.SelectedItem.Value
        End If

        strSearch = strSearch & " AND C.AccMonth <= CAST('" & strAccMonth & "' AS INT)"

        strAccYear = lstAccYear.SelectedItem.Value
        strSearch = strSearch & " AND C.AccYear = '" & strAccYear & "'"

        If txtStaff.Text <> "" Then
            strSearch = strSearch & " AND Name LIKE '%" & Trim(txtStaff.Text) & "%' "
        End If
        If txtDocID.Text <> "" Then
            strSearch = strSearch & " AND StaffAdvDoc LIKE '%" & Trim(txtDocID.Text) & "%' "
        End If
        If ddlDocStatus.SelectedItem.Value <> "0" Then
            If ddlDocStatus.SelectedItem.Value = "1" Then
                strSearch = " AND Outstanding > 0"
            Else
                strSearch = " AND Outstanding = 0"
            End If
        End If

        If Not strSearch = "" Then
            If Right(strSearch, 4) = "AND " Then
                strSearch = Left(strSearch, Len(strSearch) - 4)
            End If
        End If

        strParamName = "LOCCODE|STRSEARCH"
        strParamValue = strLocation & "|" & strSearch

        Try
            intErrNo = objGLTrx.mtdGetDataCommon(strOpCd, _
                                                strParamName, _
                                                strParamValue, _
                                                dsResult)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BANK_BALANCE&errmesg=" & lblErrMessage.Text & "&redirect=gl/trx/gl_trx_budget_list.aspx")
        End Try

        Return dsResult

    End Function

    Protected Function LoadDataTrx() As DataSet
        Dim strOpCd As String = "HR_CLSSETUP_STAFF_GET_TRX_REALIZATION"
        Dim dsResult As New Object
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer
        Dim intCnt As Integer

        strParamName = "LOCCODE|DOCID"
        strParamValue = strLocation & "|" & hidInit.Value

        Try
            intErrNo = objGLTrx.mtdGetDataCommon(strOpCd, _
                                                strParamName, _
                                                strParamValue, _
                                                dsResult)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BANK_BALANCE&errmesg=" & lblErrMessage.Text & "&redirect=gl/trx/gl_trx_budget_list.aspx")
        End Try


        Return dsResult

    End Function

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIf(SortCol.Text = "ASC", "DESC", "ASC")
        dgList.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PU_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=gl/setup/gl_setup_subblock.aspx")
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

    Sub btnPreview_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)

        'strCompany = ddlLocation.SelectedIndex

        'Response.Write("<Script Language=""JavaScript"">window.open(""CB_trx_SaldoBankPrintDocs.aspx?doctype=1&CompName=" & strCompany & _
        '               "&tgl=" & srcTgl.Text & _
        '               "&NmBank=" & ddlBank.SelectedItem.Value & _
        '               """,null ,""status=yes, height=400, width=600, top=180, left=220, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")

    End Sub


    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim lbl As Label
        Dim intIndex As Integer = E.Item.ItemIndex
        Dim strDocID As String

        lbl = dgList.Items.Item(intIndex).FindControl("lblDocID")
        strDocID = lbl.Text
        hidInit.Value = Trim(strDocID)

        lblRealization.Text = ""
        lblRealization.Text = "Advance Realization" & " (" & hidInit.Value & ") : "
        lblRealization.Visible = True
        BindGridTrx()
    End Sub



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




End Class
