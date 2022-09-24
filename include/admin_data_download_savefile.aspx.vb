
Imports System
Imports System.Data
Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.Page
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic.Interaction
Imports Microsoft.VisualBasic.FileSystem
Imports Microsoft.VisualBasic
Imports agri.PWSystem.clsConfig
Imports agri.GlobalHdl.clsGlobalHdl
Imports agri.GlobalHdl.clsAccessRights
Imports agri.Admin.clsData
Imports agri.IN.clsData
Imports agri.CT.clsData
Imports agri.WS.clsData
Imports agri.NU.clsData
Imports agri.PU.clsData
Imports agri.HR.clsData
Imports agri.PR.clsData
Imports agri.BI.clsData
Imports agri.WM.clsData
Imports agri.CM.clsData
Imports agri.GL.clsData

Public Class admin_data_download_savefile : inherits page

    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAdminData As New agri.Admin.clsData()
    Dim objINData As New agri.IN.clsData
    Dim objCTData As New agri.CT.clsData
    Dim objWSData As New agri.WS.clsData
    Dim objNUData As New agri.NU.clsData
    Dim objPUData As New agri.PU.clsData
    Dim objHRData As New agri.HR.clsData
    Dim objPRData As New agri.PR.clsData
    Dim objPDData As New agri.PD.clsData
    Dim objBIData As New agri.BI.clsData
    Dim objWMData As New agri.WM.clsData
    Dim objCMData As New agri.CM.clsData
    Dim objGLData As New agri.GL.clsData
    Dim objDownloadDs As New DataSet()
    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim intADAR As Integer

    Sub Page_Load(Sender As Object, E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        intADAR = Session("SS_ADAR")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumADAccessRights.ADAdmDataTransfer), intADAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            If LCase(Trim(Request.QueryString("global"))) = "yes" Then
                SaveGlobalFile()
            Else
                SaveFile()
            End If
        End If
    End Sub


    Sub SaveFile()
        Dim objStreamReader As StreamReader

        Dim strComp As String = lcase(Request.QueryString("company"))
        Dim strLoc As String = lcase(Request.QueryString("location"))
        Dim strUOM As String = lcase(Request.QueryString("uom"))
        Dim strUOMcon As String = lcase(Request.QueryString("uomcon"))

        Dim strOpCode_Co As String = "ADMIN_CLSDATA_COMPANY_DOWNLOAD"
        Dim strOpCode_Loc As String = "ADMIN_CLSDATA_LOCATION_DOWNLOAD"
        Dim strOpCode_UOM As String = "ADMIN_CLSDATA_UOM_DOWNLOAD"
        Dim strOpCode_UOMCon As String = "ADMIN_CLSDATA_UOMCON_DOWNLOAD"

        Dim strFtpPath As String
        Dim strXmlPath As String = ""
        Dim strXmlFileName As String = objGlobal.mtdGenerateDownloadFile(objGlobal.EnumDataTransferFileType.AD_AdministrationReferenceData, False)
        Dim strZipFileName As String = Mid(strXmlFileName, 1, Len(strXmlFileName) - 3) & "zip"
        Dim strParam As String = ""
        Dim intErrNo As Integer
        Dim strXmlb4Encrypt As String = ""
        Dim ObjXmlEncrypted As New Object()
        Dim ObjXmlDecrypted As New Object()

        Try
            intErrNo = objSysCfg.mtdGetFtpPath(strFtpPath)
            strXmlPath = strFtpPath & strXmlFileName
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADMIN_GET_FTPPATH&errmesg=&redirect=")
        End Try

        If strComp = "true" Then strParam = strParam & strOpCode_Co & "|" & _
                                    objAdminData.mtdGetDownloadTable(objAdminData.EnumDownloadType.Company) & Chr(9)
        If strLoc = "true" Then strParam = strParam & strOpCode_Loc & "|" & _
                                    objAdminData.mtdGetDownloadTable(objAdminData.EnumDownloadType.Location) & Chr(9)
        If strUOM = "true" Then strParam = strParam & strOpCode_UOM & "|" & _
                                    objAdminData.mtdGetDownloadTable(objAdminData.EnumDownloadType.UOM) & Chr(9)
        If strUOMcon = "true" Then strParam = strParam & strOpCode_UOMCon & "|" & _
                                    objAdminData.mtdGetDownloadTable(objAdminData.EnumDownloadType.UOMConvertion) & Chr(9)

        If strParam <> "" Then
            strParam = Mid(strParam, 1, Len(strParam) - 1)
            Try
                intErrNo = objAdminData.mtdDownloadRef(strParam, _
                                                       objDownloadDs)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADMIN_DOWNLOAD_REF&errmesg=&redirect=")
            End Try


            objDownloadDs.WriteXml(strXmlPath, XmlWriteMode.WriteSchema)

            objStreamReader = File.OpenText(strXmlPath)
            strXmlb4Encrypt = objStreamReader.ReadToEnd()
            objStreamReader.Close()

            Try
                intErrNo = objSysCfg.mtdEncryptRef(strXmlb4Encrypt, objXmlEncrypted)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADMIN_DOWNLOAD_ENCRYPT_REF&errmesg=&redirect=")
            End Try

            Response.Write(objXmlEncrypted)
            Response.ContentType = "application/zip"
            Response.AddHeader("Content-Disposition", _
                                "attachment; filename=""" & strZipFileName & """")
        End If
    End Sub

    Sub SaveGlobalFile()
        Const DOWNLOAD_IN_REFERENCE_FILE As Boolean = True
        Const DOWNLOAD_CT_REFERENCE_FILE As Boolean = True
        Const DOWNLOAD_WS_REFERENCE_FILE As Boolean = True
        Const DOWNLOAD_NU_REFERENCE_FILE As Boolean = True
        Const DOWNLOAD_PU_REFERENCE_FILE As Boolean = True
        Const DOWNLOAD_AP_REFERENCE_FILE As Boolean = False
        Const DOWNLOAD_HR_REFERENCE_FILE As Boolean = True
        Const DOWNLOAD_PR_REFERENCE_FILE As Boolean = True
        Const DOWNLOAD_PD_REFERENCE_FILE As Boolean = False
        Const DOWNLOAD_PM_REFERENCE_FILE As Boolean = False
        Const DOWNLOAD_AR_REFERENCE_FILE As Boolean = True
        Const DOWNLOAD_FA_REFERENCE_FILE As Boolean = False
        Const DOWNLOAD_WM_REFERENCE_FILE As Boolean = True
        Const DOWNLOAD_CM_REFERENCE_FILE As Boolean = True
        Const DOWNLOAD_GL_REFERENCE_FILE As Boolean = True
        Const DOWNLOAD_BD_REFERENCE_FILE As Boolean = False
        Const DOWNLOAD_ADMIN_REFERENCE_FILE As Boolean = True
        Const DOWNLOAD_MISC_FILE As Boolean = True
        
        Dim blnNoLoc As Boolean
        Dim objStreamReader As StreamReader
        Dim strFtpPath As String
        Dim strXmlPath As String = ""
        Dim strXmlFileName As String = objGlobal.mtdGenerateDownloadFile(objGlobal.EnumDataTransferFileType.AD_GlobalReferenceData, False)
        Dim strZipFileName As String = Mid(strXmlFileName, 1, Len(strXmlFileName) - 3) & "zip"
        Dim intErrNo As Integer
        Dim strXmlb4Encrypt As String = ""
        Dim ObjXmlEncrypted As New Object()
        Dim ObjXmlDecrypted As New Object()
        
        Dim strAccMonthIN As String = Session("SS_INACCMONTH")
        Dim strAccMonthCT As String = Session("SS_INACCMONTH")
        Dim strAccMonthWS As String = Session("SS_INACCMONTH")
        Dim strAccMonthNU As String = Session("SS_NUACCMONTH")
        Dim strAccMonthPU As String = Session("SS_PUACCMONTH")
        Dim strAccMonthHR As String = Session("SS_PRACCMONTH")
        Dim strAccMonthPR As String = Session("SS_PRACCMONTH")
        Dim strAccMonthAR As String = Session("SS_ARACCMONTH")
        Dim strAccMonthWM As String = Session("SS_PDACCMONTH")
        Dim strAccMonthCM As String = Session("SS_ARACCMONTH")
        Dim strAccMonthGL As String = Session("SS_GLACCMONTH")
        
        Dim strAccYearIN As String = Session("SS_INACCYEAR")
        Dim strAccYearCT As String = Session("SS_INACCYEAR")
        Dim strAccYearWS As String = Session("SS_INACCYEAR")
        Dim strAccYearNU As String = Session("SS_NUACCYEAR")
        Dim strAccYearPU As String = Session("SS_PUACCYEAR")
        Dim strAccYearHR As String = Session("SS_PRACCYEAR")
        Dim strAccYearPR As String = Session("SS_PRACCYEAR")
        Dim strAccYearAR As String = Session("SS_ARACCYEAR")
        Dim strAccYearWM As String = Session("SS_PDACCYEAR")
        Dim strAccYearCM As String = Session("SS_ARACCYEAR")
        Dim strAccYearGL As String = Session("SS_GLACCYEAR")

        Dim strParamIN As String = ""
        Dim strParamCT As String = ""
        Dim strParamWS As String = ""
        Dim strParamNU As String = ""
        Dim strParamPU As String = ""
        Dim strParamHR As String = ""
        Dim strParamPR As String = ""
        Dim strParamAR As String = ""
        Dim strParamWM As String = ""
        Dim strParamCM As String = ""
        Dim strParamGL As String = ""
        Dim strParamAdmin As String = ""
        Dim strParamMisc As String = ""
        Dim strParam As String
        
        Dim dsIN As New DataSet()
        Dim dsCT As New DataSet()
        Dim dsWS As New DataSet()
        Dim dsNU As New DataSet()
        Dim dsPU As New DataSet()
        Dim dsHR As New DataSet()
        Dim dsPR As New DataSet()
        Dim dsAR As New DataSet()
        Dim dsWM As New DataSet()
        Dim dsCM As New DataSet()
        Dim dsGL As New DataSet()
        Dim dsAdmin As New DataSet()
        Dim dsMisc As New DataSet()
        
        If LCase(Trim(Request.QueryString("noloc"))) = "yes" Then
            blnNoLoc = True 
        Else
            blnNoLoc = False    
        End If

        objDownloadDs.Clear()
        objDownloadDs.EnforceConstraints = False
        
        If DOWNLOAD_IN_REFERENCE_FILE = True Then
            strParamIN = "IN_CLSDATA_PRODTYPE_DOWNLOAD" & "|" & objInData.mtdGetDownloadTable(objInData.EnumDownloadType.ProdType) & Chr(9) & _
                        "IN_CLSDATA_PRODBRAND_DOWNLOAD" & "|" & objInData.mtdGetDownloadTable(objInData.EnumDownloadType.ProdBrand) & Chr(9) & _
                        "IN_CLSDATA_PRODMODEL_DOWNLOAD" & "|" & objInData.mtdGetDownloadTable(objInData.EnumDownloadType.ProdModel) & Chr(9) & _
                        "IN_CLSDATA_PRODCAT_DOWNLOAD" & "|" & objInData.mtdGetDownloadTable(objInData.EnumDownloadType.ProdCat) & Chr(9) & _
                        "IN_CLSDATA_PRODMAT_DOWNLOAD" & "|" & objInData.mtdGetDownloadTable(objInData.EnumDownloadType.ProdMat) & Chr(9) & _
                        "IN_CLSDATA_STOCKANALYSIS_DOWNLOAD" & "|" & objInData.mtdGetDownloadTable(objInData.EnumDownloadType.StockAnalysis) & Chr(9) & _
                        "IN_CLSDATA_STOCKMASTER_DOWNLOAD" & "|" & objInData.mtdGetDownloadTable(objInData.EnumDownloadType.StockItem) & Chr(9) & _
                        "IN_CLSDATA_DIRECTCHARGEMASTER_DOWNLOAD" & "|" & objInData.mtdGetDownloadTable(objInData.EnumDownloadType.DirectChargeItem)
            Try
                intErrNo = objINData.mtdDownloadRef(strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strAccMonthIN, _
                                                    strAccYearIN, _
                                                    strParamIN, _
                                                    dsIN)
                objDownloadDs.Merge(dsIN, False, System.Data.MissingSchemaAction.Add)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=INDATA_DOWNLOAD_REF&errmesg=&redirect=")
            End Try
        End If
        
        If DOWNLOAD_CT_REFERENCE_FILE = True Then
            strParamCT = "CT_CLSDATA_CANTEENMASTER_DOWNLOAD" & "|" & objCTData.mtdGetDownloadTable(objCTData.EnumDownloadType.CanteenItem)
            Try
                intErrNo = objCTData.mtdDownloadRef(strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strAccMonthCT, _
                                                    strAccYearCT, _
                                                    strParamCT, _
                                                    dsCT)
                objDownloadDs.Merge(dsCT, False, System.Data.MissingSchemaAction.Add)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CTDATA_DOWNLOAD_REF&errmesg=&redirect=")
            End Try
        End If
        
        If DOWNLOAD_WS_REFERENCE_FILE = True Then
            strParamWS = "IN_CLSDATA_PRODTYPE_DOWNLOAD" & "|" & objWSData.mtdGetDownloadTable(objWSData.EnumDownloadType.ProdType) & Chr(9) & _
                        "IN_CLSDATA_PRODBRAND_DOWNLOAD" & "|" & objWSData.mtdGetDownloadTable(objWSData.EnumDownloadType.ProdBrand) & Chr(9) & _
                        "IN_CLSDATA_PRODMODEL_DOWNLOAD" & "|" & objWSData.mtdGetDownloadTable(objWSData.EnumDownloadType.ProdModel) & Chr(9) & _
                        "IN_CLSDATA_PRODCAT_DOWNLOAD" & "|" & objWSData.mtdGetDownloadTable(objWSData.EnumDownloadType.ProdCat) & Chr(9) & _
                        "IN_CLSDATA_PRODMAT_DOWNLOAD" & "|" & objWSData.mtdGetDownloadTable(objWSData.EnumDownloadType.ProdMat) & Chr(9) & _
                        "IN_CLSDATA_STOCKANALYSIS_DOWNLOAD" & "|" & objWSData.mtdGetDownloadTable(objWSData.EnumDownloadType.StockAnalysis) & Chr(9) & _
                        "WS_CLSDATA_WORKSHOPMASTER_DOWNLOAD" & "|" & objWSData.mtdGetDownloadTable(objWSData.EnumDownloadType.WorkshopItem) & Chr(9) & _
                        "IN_CLSDATA_DIRECTCHARGEMASTER_DOWNLOAD" & "|" & objWSData.mtdGetDownloadTable(objWSData.EnumDownloadType.DirectChargeItem) & Chr(9) & _
                        "WS_CLSDATA_WORKCODE_DOWNLOAD" & "|" & objWSData.mtdGetDownloadTable(objWSData.EnumDownloadType.WorkCode) & Chr(9) & _
                        "WS_CLSDATA_WORKSHOP_DOWNLOAD" & "|" & objWSData.mtdGetDownloadTable(objWSData.EnumDownloadType.WorkShop) & Chr(9) & _
                        "WS_CLSDATA_WORKSHOP_LINE_DOWNLOAD" & "|" & objWSData.mtdGetDownloadTable(objWSData.EnumDownloadType.WorkShopLn) & Chr(9) & _
                        "IN_CLSDATA_ITEMPART_DOWNLOAD" & "|" & objWSData.mtdGetDownloadTable(objWSData.EnumDownloadType.WSItemPart)
            Try
                intErrNo = objWSData.mtdDownloadRef(strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strAccMonthWS, _
                                                    strAccYearWS, _
                                                    strParamWS, _
                                                    dsWS)
                objDownloadDs.Merge(dsWS, False, System.Data.MissingSchemaAction.Add)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=WSDATA_DOWNLOAD_REF&errmesg=&redirect=")
            End Try
        End If
        
        If DOWNLOAD_NU_REFERENCE_FILE = True Then
            strParamNU = "NU_CLSDATA_CULLTYPE_DOWNLOAD" & "|" & objNUData.mtdGetDownloadTable(objNUData.EnumDownloadType.CullType) & Chr(9) & _
                         "NU_CLSDATA_ACCCLS_DOWNLOAD" & "|" & objNUData.mtdGetDownloadTable(objNUData.EnumDownloadType.AccCls)
            Try
                intErrNo = objNUData.mtdDownloadRef(strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strAccMonthNU, _
                                                    strAccYearNU, _
                                                    strParamNU, _
                                                    dsNU)
                objDownloadDs.Merge(dsNU, False, System.Data.MissingSchemaAction.Add)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=NUDATA_DOWNLOAD_REF&errmesg=&redirect=")
            End Try
        End If
        
        If DOWNLOAD_PU_REFERENCE_FILE = True Then
            strParamPU = "PU_CLSDATA_SUPPLIER_DOWNLOAD" & "|" & objPUData.mtdGetDownloadTable(objPUData.EnumDownloadType.Supplier)
            Try
                intErrNo = objPUData.mtdDownloadRef(strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strAccMonthPU, _
                                                    strAccYearPU, _
                                                    strParamPU, _
                                                    dsPU)
                objDownloadDs.Merge(dsPU, False, System.Data.MissingSchemaAction.Add)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CTDATA_DOWNLOAD_REF&errmesg=&redirect=")
            End Try
        End If
        
        If DOWNLOAD_AP_REFERENCE_FILE = True Then
        End If


        If DOWNLOAD_HR_REFERENCE_FILE = True Then
            strParamHR = "HR_CLSDATA_DEPTCODE_DOWNLOAD" & "|" & objHRData.mtdGetDownloadTable(objHRData.EnumDownloadType.DeptCode) & Chr(9) & _
                        "HR_CLSDATA_DEPT_DOWNLOAD" & "|" & objHRData.mtdGetDownloadTable(objHRData.EnumDownloadType.Dept) & Chr(9) & _
                        "HR_CLSDATA_NATIONALITY_DOWNLOAD" & "|" & objHRData.mtdGetDownloadTable(objHRData.EnumDownloadType.Nationality) & Chr(9) & _
                        "HR_CLSDATA_FUNCTION_DOWNLOAD" & "|" & objHRData.mtdGetDownloadTable(objHRData.EnumDownloadType.Func) & Chr(9) & _
                        "HR_CLSDATA_POSITION_DOWNLOAD" & "|" & objHRData.mtdGetDownloadTable(objHRData.EnumDownloadType.Position) & Chr(9) & _
                        "HR_CLSDATA_LEVEL_DOWNLOAD" & "|" & objHRData.mtdGetDownloadTable(objHRData.EnumDownloadType.Level) & Chr(9) & _
                        "HR_CLSDATA_RELIGION_DOWNLOAD" & "|" & objHRData.mtdGetDownloadTable(objHRData.EnumDownloadType.Religion) & Chr(9) & _
                        "HR_CLSDATA_ICTYPE_DOWNLOAD" & "|" & objHRData.mtdGetDownloadTable(objHRData.EnumDownloadType.ICType) & Chr(9) & _
                        "HR_CLSDATA_RACE_DOWNLOAD" & "|" & objHRData.mtdGetDownloadTable(objHRData.EnumDownloadType.Race) & Chr(9) & _
                        "HR_CLSDATA_SKILL_DOWNLOAD" & "|" & objHRData.mtdGetDownloadTable(objHRData.EnumDownloadType.Skill) & Chr(9) & _
                        "HR_CLSDATA_SHIFT_DOWNLOAD" & "|" & objHRData.mtdGetDownloadTable(objHRData.EnumDownloadType.Shift) & Chr(9) & _
                        "HR_CLSDATA_SHIFTLN_DOWNLOAD" & "|" & objHRData.mtdGetDownloadTable(objHRData.EnumDownloadType.ShiftLn) & Chr(9) & _
                        "HR_CLSDATA_QUALIFICATION_DOWNLOAD" & "|" & objHRData.mtdGetDownloadTable(objHRData.EnumDownloadType.Qualification) & Chr(9) & _
                        "HR_CLSDATA_SUBJECT_DOWNLOAD" & "|" & objHRData.mtdGetDownloadTable(objHRData.EnumDownloadType.Subject) & Chr(9) & _
                        "HR_CLSDATA_EVALUATION_DOWNLOAD" & "|" & objHRData.mtdGetDownloadTable(objHRData.EnumDownloadType.Evaluation) & Chr(9) & _
                        "HR_CLSDATA_CAREERPROGRESS_DOWNLOAD" & "|" & objHRData.mtdGetDownloadTable(objHRData.EnumDownloadType.CareerProgress) & Chr(9) & _
                        "HR_CLSDATA_SALSCHEME_DOWNLOAD" & "|" & objHRData.mtdGetDownloadTable(objHRData.EnumDownloadType.SalScheme) & Chr(9) & _
                        "HR_CLSDATA_SALGRADE_DOWNLOAD" & "|" & objHRData.mtdGetDownloadTable(objHRData.EnumDownloadType.SalGrade) & Chr(9) & _
                        "HR_CLSDATA_BANKFORMAT_DOWNLOAD" & "|" & objHRData.mtdGetDownloadTable(objHRData.EnumDownloadType.BankFormat) & Chr(9) & _
                        "HR_CLSDATA_BANK_DOWNLOAD" & "|" & objHRData.mtdGetDownloadTable(objHRData.EnumDownloadType.Bank) & Chr(9) & _
                        "HR_CLSDATA_EPF_DOWNLOAD" & "|" & objHRData.mtdGetDownloadTable(objHRData.EnumDownloadType.EPF) & Chr(9) & _
                        "HR_CLSDATA_TAXBRANCH_DOWNLOAD" & "|" & objHRData.mtdGetDownloadTable(objHRData.EnumDownloadType.TaxBranch) & Chr(9) & _
                        "HR_CLSDATA_TAX_DOWNLOAD" & "|" & objHRData.mtdGetDownloadTable(objHRData.EnumDownloadType.Tax) & Chr(9) & _
                        "HR_CLSDATA_SOCSO_DOWNLOAD" & "|" & objHRData.mtdGetDownloadTable(objHRData.EnumDownloadType.Socso) & Chr(9) & _
                        "HR_CLSDATA_SOCSOLN_DOWNLOAD" & "|" & objHRData.mtdGetDownloadTable(objHRData.EnumDownloadType.SocsoLn) & Chr(9) & _
                        "HR_CLSDATA_GPH_DOWNLOAD" & "|" & objHRData.mtdGetDownloadTable(objHRData.EnumDownloadType.GPH) & Chr(9) & _
                        "HR_CLSDATA_HS_DOWNLOAD" & "|" & objHRData.mtdGetDownloadTable(objHRData.EnumDownloadType.HS) & Chr(9) & _
                        "HR_CLSDATA_HSLN_DOWNLOAD" & "|" & objHRData.mtdGetDownloadTable(objHRData.EnumDownloadType.HSLn) & Chr(9) & _
                        "HR_CLSDATA_MPOBPRICE_DOWNLOAD" & "|" & objHRData.mtdGetDownloadTable(objHRData.EnumDownloadType.MPOBPrice) & Chr(9) & _
                        "HR_CLSDATA_MPOBPRICELN_DOWNLOAD" & "|" & objHRData.mtdGetDownloadTable(objHRData.EnumDownloadType.MPOBPriceLn)
            Try
                intErrNo = objHRData.mtdDownloadRef(strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strAccMonthHR, _
                                                    strAccYearHR, _
                                                    strParamHR, _
                                                    "", _
                                                    "", _
                                                    dsHR, _
                                                    False)
                objDownloadDs.Merge(dsHR, False, System.Data.MissingSchemaAction.Add)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=HRDATA_DOWNLOAD_REF&errmesg=&redirect=")
            End Try
        End If
        
        If DOWNLOAD_PR_REFERENCE_FILE = True Then
            strParamPR = "PR_CLSDATA_ADGROUP_DOWNLOAD" & "|" & objPRData.mtdGetDownloadTable(objPRData.EnumDownloadType.ADGroup) & Chr(9) & _
                        "PR_CLSDATA_AD_DOWNLOAD" & "|" & objPRData.mtdGetDownloadTable(objPRData.EnumDownloadType.AD) & Chr(9) & _
                        "PR_CLSDATA_CONTRACTOR_DOWNLOAD" & "|" & objPRData.mtdGetDownloadTable(objPRData.EnumDownloadType.Contractor) & Chr(9) & _
                        "PR_CLSDATA_ATTENDANCE_DOWNLOAD" & "|" & objPRData.mtdGetDownloadTable(objPRData.EnumDownloadType.Attendance) & Chr(9) & _
                        "PR_CLSDATA_ATTENDANCE_INCSCHEME_DOWNLOAD" & "|" & objPRData.mtdGetDownloadTable(objPRData.EnumDownloadType.AttdIncentive) & Chr(9) & _
                        "PR_CLSDATA_HARVESTING_INCSCHEME_DOWNLOAD" & "|" & objPRData.mtdGetDownloadTable(objPRData.EnumDownloadType.HarvIncentive) & Chr(9) & _
                        "PR_CLSDATA_DIFFERENTIALS_DOWNLOAD" & "|" & objPRData.mtdGetDownloadTable(objPRData.EnumDownloadType.Diff) & Chr(9) & _
                        "PR_CLSDATA_DIFFERENTIALSLN_DOWNLOAD" & "|" & objPRData.mtdGetDownloadTable(objPRData.EnumDownloadType.DiffLn) & Chr(9) & _
                        "PR_CLSDATA_PAYDIVISION_DOWNLOAD" & "|" & objPRData.mtdGetDownloadTable(objPRData.EnumDownloadType.PayDivision) & Chr(9) & _
                        "PR_CLSDATA_MAC_DOWNLOAD" & "|" & objPRData.mtdGetDownloadTable(objPRData.EnumDownloadType.MAC)
            Try
                intErrNo = objPRData.mtdDownloadRef(strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strAccMonthPR, _
                                                    strAccYearPR, _
                                                    strParamPR, _
                                                    dsPR)
                objDownloadDs.Merge(dsPR, False, System.Data.MissingSchemaAction.Add)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PRDATA_DOWNLOAD_REF&errmesg=&redirect=")
            End Try
        End If
        
        If DOWNLOAD_PD_REFERENCE_FILE = True Then
        End If
        If DOWNLOAD_PM_REFERENCE_FILE = True Then
        End If

        If DOWNLOAD_AR_REFERENCE_FILE = True Then
            strParamAR = "BI_CLSDATA_SUPPLIER_DOWNLOAD" & "|" & objBIData.mtdGetDownloadTable(objBIData.EnumDownloadType.BillParty)
            Try
                intErrNo = objBIData.mtdDownloadRef(strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strAccMonthAR, _
                                                    strAccYearAR, _
                                                    strParamAR, _
                                                    dsAR)
                objDownloadDs.Merge(dsAR, False, System.Data.MissingSchemaAction.Add)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BIDATA_DOWNLOAD_REF&errmesg=&redirect=")
            End Try
        End If
        
        If DOWNLOAD_FA_REFERENCE_FILE = True Then
        End If
        
        If DOWNLOAD_WM_REFERENCE_FILE = True Then
            strParamWM = "WM_CLSDATA_TRANSPORTER_DOWNLOAD" & "|" & objWMData.mtdGetDownloadTable(objWMData.EnumDownloadType.Transporter)
            Try
                intErrNo = objWMData.mtdDownloadRef(strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strAccMonthWM, _
                                                    strAccYearWM, _
                                                    strParamWM, _
                                                    dsWM)
                objDownloadDs.Merge(dsWM, False, System.Data.MissingSchemaAction.Add)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=WMDATA_DOWNLOAD_REF&errmesg=&redirect=")
            End Try
        End If
        
        If DOWNLOAD_CM_REFERENCE_FILE = True Then
            strParamCM = "CM_CLSDATA_CURRENCY_DOWNLOAD" & "|" & objCMData.mtdGetDownloadTable(objCMData.EnumDownloadType.Currency)
            Try
                intErrNo = objCMData.mtdDownloadRef(strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strAccMonthCM, _
                                                    strAccYearCM, _
                                                    strParamCM, _
                                                    dsCM)
                objDownloadDs.Merge(dsCM, False, System.Data.MissingSchemaAction.Add)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CMDATA_DOWNLOAD_REF&errmesg=&redirect=")
            End Try
        End If
        
        If DOWNLOAD_GL_REFERENCE_FILE = True Then
            If blnNoLoc= True Then  
                strParamGL = "GL_CLSDATA_ACCCLSGRP_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.AccountClassGroup) & Chr(9) & _
                             "GL_CLSDATA_ACCCLS_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.AccountClass) & Chr(9) & _
                             "GL_CLSDATA_ACTGRP_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.ActivityGroup) & Chr(9) & _
                             "GL_CLSDATA_ACT_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.Activity) & Chr(9) & _
                             "GL_CLSDATA_SUBACT_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.SubActivity) & Chr(9) & _
                             "GL_CLSDATA_EXP_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.Expense) & Chr(9) & _
                             "GL_CLSDATA_VEHEXPGRP_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.VehicleExpenseGrp) & Chr(9) & _
                             "GL_CLSDATA_VEHEXP_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.VehicleExpense) & Chr(9) & _
                             "GL_CLSDATA_VEHTYPE_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.VehicleType) & Chr(9) & _
                             "GL_CLSDATA_VEH_DOWNLOAD_ALL" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.Vehicle) & Chr(9) & _
                             "GL_CLSDATA_BLKGRP_DOWNLOAD_ALL" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.BlockGroup) & Chr(9) & _
                             "GL_CLSDATA_BLK_DOWNLOAD_ALL" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.Block) & Chr(9) & _
                             "GL_CLSDATA_BLKACC_DOWNLOAD_ALL" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.BlockAccount) & Chr(9) & _
                             "GL_CLSDATA_SUBBLK_DOWNLOAD_ALL" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.SubBlock) & Chr(9) & _
                             "GL_CLSDATA_SUBBLKACC_DOWNLOAD_ALL" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.SubBlockAccount) & Chr(9) & _
                             "GL_CLSDATA_ACCGRP_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.AccountGroup) & Chr(9) & _
                             "GL_CLSDATA_ACC_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.Account)
            Else
                strParamGL = "GL_CLSDATA_ACCCLSGRP_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.AccountClassGroup) & Chr(9) & _
                             "GL_CLSDATA_ACCCLS_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.AccountClass) & Chr(9) & _
                             "GL_CLSDATA_ACTGRP_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.ActivityGroup) & Chr(9) & _
                             "GL_CLSDATA_ACT_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.Activity) & Chr(9) & _
                             "GL_CLSDATA_SUBACT_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.SubActivity) & Chr(9) & _
                             "GL_CLSDATA_EXP_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.Expense) & Chr(9) & _
                             "GL_CLSDATA_VEHEXPGRP_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.VehicleExpenseGrp) & Chr(9) & _
                             "GL_CLSDATA_VEHEXP_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.VehicleExpense) & Chr(9) & _
                             "GL_CLSDATA_VEHTYPE_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.VehicleType) & Chr(9) & _
                             "GL_CLSDATA_VEH_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.Vehicle) & Chr(9) & _
                             "GL_CLSDATA_BLKGRP_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.BlockGroup) & Chr(9) & _
                             "GL_CLSDATA_BLK_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.Block) & Chr(9) & _
                             "GL_CLSDATA_BLKACC_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.BlockAccount) & Chr(9) & _
                             "GL_CLSDATA_SUBBLK_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.SubBlock) & Chr(9) & _
                             "GL_CLSDATA_SUBBLKACC_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.SubBlockAccount) & Chr(9) & _
                             "GL_CLSDATA_ACCGRP_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.AccountGroup) & Chr(9) & _
                             "GL_CLSDATA_ACC_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.Account)
            End If
            Try
                intErrNo = objGLData.mtdDownloadRef(strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strAccMonthGL, _
                                                    strAccYearGL, _
                                                    strParamGL, _
                                                    dsGL)
                objDownloadDs.Merge(dsGL, False, System.Data.MissingSchemaAction.Add)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GLDATA_DOWNLOAD_REF&errmesg=&redirect=")
            End Try
        End If
        
        If DOWNLOAD_BD_REFERENCE_FILE = True Then
        End If
        
        If DOWNLOAD_ADMIN_REFERENCE_FILE = True Then
            strParamAdmin = "ADMIN_CLSDATA_COMPANY_DOWNLOAD" & "|" & objAdminData.mtdGetDownloadTable(objAdminData.EnumDownloadType.Company) & Chr(9) & _
                            "ADMIN_CLSDATA_LOCATION_DOWNLOAD" & "|" & objAdminData.mtdGetDownloadTable(objAdminData.EnumDownloadType.Location) & Chr(9) & _
                            "ADMIN_CLSDATA_UOM_DOWNLOAD" & "|" & objAdminData.mtdGetDownloadTable(objAdminData.EnumDownloadType.UOM) & Chr(9) & _
                            "ADMIN_CLSDATA_UOMCON_DOWNLOAD" & "|" & objAdminData.mtdGetDownloadTable(objAdminData.EnumDownloadType.UOMConvertion)
            Try
                intErrNo = objAdminData.mtdDownloadRef(strParamAdmin, dsAdmin)
                objDownloadDs.Merge(dsAdmin, False, System.Data.MissingSchemaAction.Add)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADMIN_DOWNLOAD_REF&errmesg=&redirect=")
            End Try
        End If

        If DOWNLOAD_MISC_FILE = True Then
            dsMisc.Clear()
            If blnNoLoc= True Then
                strParamMisc = "PR_CLSDATA_WAGES_DOWNLOAD_ALL" & "|" & objPRData.mtdGetDownloadTable(objPRData.EnumDownloadType.Wages)
            Else
                strParamMisc = "PR_CLSDATA_WAGES_DOWNLOAD" & "|" & objPRData.mtdGetDownloadTable(objPRData.EnumDownloadType.Wages)
            End If
            Try
                intErrNo = objPRData.mtdDownloadRef(strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strAccMonthPR, _
                                                    strAccYearPR, _
                                                    strParamMisc, _
                                                    dsMisc)
                objDownloadDs.Merge(dsMisc, False, System.Data.MissingSchemaAction.Add)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PRDATA_DOWNLOAD_WAGES&errmesg=&redirect=")
            End Try
            
            dsMisc.Clear()
            If blnNoLoc= True Then
                strParamMisc = "WM_CLSDATA_DISPATCH_GET_ALL" & "|" & objWMData.mtdGetDownloadTable(objWMData.EnumDownloadType.Ticket) & Chr(9) & _
                               "BI_CLSDATA_SUPPLIER_DOWNLOAD" & "|" & objBIData.mtdGetDownloadTable(objBIData.EnumDownloadType.BillParty) & Chr(9) & _
                               "WM_CLSDATA_TRANSPORTER_DOWNLOAD" & "|" & objWMData.mtdGetDownloadTable(objWMData.EnumDownloadType.Transporter)
            Else
                strParamMisc = "WM_CLSDATA_DISPATCH_GET" & "|" & objWMData.mtdGetDownloadTable(objWMData.EnumDownloadType.Ticket) & Chr(9) & _
                               "BI_CLSDATA_SUPPLIER_DOWNLOAD" & "|" & objBIData.mtdGetDownloadTable(objBIData.EnumDownloadType.BillParty) & Chr(9) & _
                               "WM_CLSDATA_TRANSPORTER_DOWNLOAD" & "|" & objWMData.mtdGetDownloadTable(objWMData.EnumDownloadType.Transporter)
            End If
            Try
                intErrNo = objWMData.mtdDownloadDispatch("WM_CLSDATA_TICKET_BATCHNO_UPD_ALL", _
                                                         strCompany, _
                                                         strLocation, _
                                                         strUserId, _
                                                         strAccMonthWM, _
                                                         strAccYearWM, _
                                                         strParamMisc, _
                                                         " AND DownloadBatchNo = '' ", _
                                                         True, _
                                                         dsMisc)
                objDownloadDs.Merge(dsMisc, False, System.Data.MissingSchemaAction.Add)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=WMDATA_DOWNLOAD_DISPATCH&errmesg=&redirect=")
            End Try
            
            dsMisc.Clear()
            If blnNoLoc= True Then
                strParamMisc = "GL_CLSDATA_SHMTHENDTRX_DOWNLOAD_ALL" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.SHMthEndTrxTmp) & Chr(9) & _
                               "GL_CLSDATA_BLK_DOWNLOAD_ALL" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.Block) & Chr(9) & _
                               "GL_CLSDATA_SUBBLK_DOWNLOAD_ALL" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.SubBlock) & Chr(9) & _
                               "GL_CLSDATA_VEH_DOWNLOAD_ALL" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.Vehicle) & Chr(9) & _
                               "GL_CLSDATA_VEHUSAGE_DOWNLOAD_ALL" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.VehUsage) & Chr(9) & _
                               "GL_CLSDATA_VEHUSAGE_LINE_DOWNLOAD_ALL" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.VehUsageLn) & Chr(9) & _
                               "GL_CLSDATA_OILPALM_DOWNLOAD_ALL" & "|" & objPDData.mtdGetDownloadTable(objPDData.EnumDownloadType.PDEstYield)
                Try
                    intErrNo = objGLData.mtdDownloadGLTrx("GL_CLSDATA_SHMTHENDTRXTMP_DEL_ALL", _
                                                          "GL_CLSDATA_SHMTHENDTRXTMP_ADD_ALL", _
                                                          strCompany, _
                                                          strLocation, _
                                                          strUserId, _
                                                          strAccMonthGL, _
                                                          strAccYearGL, _
                                                          strParamMisc, _
                                                          dsMisc)
                    objDownloadDs.Merge(dsMisc, False, System.Data.MissingSchemaAction.Add)
                Catch Exp As System.Exception
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GLDATA_DOWNLOAD_TRX&errmesg=&redirect=")
                End Try
            Else
                strParamMisc = "GL_CLSDATA_SHMTHENDTRX_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.SHMthEndTrxTmp) & Chr(9) & _
                               "GL_CLSDATA_BLK_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.Block) & Chr(9) & _
                               "GL_CLSDATA_SUBBLK_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.SubBlock) & Chr(9) & _
                               "GL_CLSDATA_VEH_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.Vehicle) & Chr(9) & _
                               "GL_CLSDATA_VEHUSAGE_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.VehUsage) & Chr(9) & _
                               "GL_CLSDATA_VEHUSAGE_LINE_DOWNLOAD" & "|" & objGLData.mtdGetDownloadTable(objGLData.EnumDownloadType.VehUsageLn) & Chr(9) & _
                               "GL_CLSDATA_OILPALM_DOWNLOAD" & "|" & objPDData.mtdGetDownloadTable(objPDData.EnumDownloadType.PDEstYield)
                Try
                    intErrNo = objGLData.mtdDownloadGLTrx("GL_CLSDATA_SHMTHENDTRXTMP_DEL", _
                                                          "GL_CLSDATA_SHMTHENDTRXTMP_ADD", _
                                                          strCompany, _
                                                          strLocation, _
                                                          strUserId, _
                                                          strAccMonthGL, _
                                                          strAccYearGL, _
                                                          strParamMisc, _
                                                          dsMisc)
                    objDownloadDs.Merge(dsMisc, False, System.Data.MissingSchemaAction.Add)
                Catch Exp As System.Exception
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GLDATA_DOWNLOAD_TRX&errmesg=&redirect=")
                End Try
            End If
        End If
        
        Try
            intErrNo = objSysCfg.mtdGetFtpPath(strFtpPath)
            strXmlPath = strFtpPath & strXmlFileName
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADMIN_GET_FTPPATH&errmesg=&redirect=")
        End Try
        
        objDownloadDs.WriteXml(strXmlPath, XmlWriteMode.WriteSchema)

        objStreamReader = File.OpenText(strXmlPath)
        strXmlb4Encrypt = objStreamReader.ReadToEnd()
        objStreamReader.Close()

        Try
            intErrNo = objSysCfg.mtdEncryptRef(strXmlb4Encrypt, objXmlEncrypted)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADMIN_DOWNLOAD_ENCRYPT_REF&errmesg=&redirect=")
        End Try

        Response.Write(objXmlEncrypted)
        Response.ContentType = "application/zip"
        Response.AddHeader("Content-Disposition", "attachment; filename=""" & strZipFileName & """")
    End Sub

End Class

