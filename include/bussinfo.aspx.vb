Imports System
Imports System.Data
Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic.Interaction
Imports Microsoft.VisualBasic.DateAndTime

Imports agri.PWSystem
Imports agri.GlobalHdl

Public Class bussinfo : Inherits Page

    Protected WithEvents imgHeader As Image
    Protected WithEvents imgLogicIcon As Image
    Protected WithEvents imgLogicFont As Image
    Protected WithEvents imgDatabaseIcon As Image
    Protected WithEvents imgDatabaseFont As Image
    Protected WithEvents imgBillingFont As Image
    Protected WithEvents tdLogic As HtmlTableCell
    Protected WithEvents tdDatabase As HtmlTableCell
    Protected WithEvents tdBilling As HtmlTableCell

    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objSysCfg As New agri.PWsystem.clsConfig()


    Sub Page_Load(Sender As Object, E As EventArgs)
        If Not IsPostBack Then
            
        End If
        onload_GetHeaderImage()
    End Sub
    
    Sub onload_GetHeaderImage()




    End Sub
End Class
