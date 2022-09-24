<%@ Page Language="vb" src="include/login.aspx.vb" Inherits="login"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>GPS - Login</title>
    <link href="en/include/css/StyleSheetTrial.css" rel="stylesheet" type="text/css" />

    <Script Language="JavaScript">
	function onload_setfocus() {
		var doc = document.frmLogin;
		if (doc.txtUserId.value == '')
			doc.txtUserId.focus();
		else
			doc.txtPassword.focus();
	}
    </Script>

</head>
<body onload="javascript:onload_setfocus();">
    <form id="frmLogin" runat="server">
        <div class="view" id="view">
        
        <div style="left: 300px; position: absolute; top: 200px; z-index: 400; height:200px" >
            <div>
                <p><img src="en/images/thumbs/logintrial.jpg" alt="" width="409" height="237" />&nbsp;</p>
            </div>
            
                <div class="login" id="login" style="left: 100px; position: absolute; top: 70px; height:200px">
                   <table border="0" width="100%" cellspacing="3">
		            <tr style="height:25px">
			            <td style="width: 7px"></td>
			            <td>User ID:<br><asp:TextBox id="txtUserId"  maxlength="8" runat="server" Width="150px" /></td>
		            </tr>
		            <tr style="height:25px">
			            <td style="width: 7px"></td>
			            <td>Password:<br><asp:TextBox textmode="password" id="txtPassword"  maxlength="8" runat="server" Width="150px" /></td>
		            </tr>
		            <tr style="height:25px">
			            <td style="width: 7px"></td>
			            <td>
				            <asp:Button text="  Login  "  onclick="loginBtn_Click" id="Button1" runat="server"/>
				            <asp:Button text="  Continue  "  onclick="continueBtn_Click" id="continueBtn" Visible=false runat="server"/>
			            </td>
		            </tr>
			    <tr>  
			   <td style="width: 7px">&nbsp;</td>
			   <td>&nbsp;</td>
			   </tr>
		            <tr style="height:25px">
			            <td style="width: 7px"></td>
			            <td>
				            <asp:Label id="lblLoginResult" forecolor="Red" runat="server" />
				            <asp:RequiredFieldValidator id="validateUserID" display="dynamic" runat="server" 
					            ErrorMessage="Please enter your User ID." 
					            ControlToValidate="txtUserId" />					
				            <asp:RequiredFieldValidator id="validatePassword" display="dynamic" runat="server" 
					            ErrorMessage="Please enter your Password." 
					            ControlToValidate="txtPassword" />					
			            </td>
		            </tr>
		            <tr style="height:25px">
			            <td style="width: 7px">&nbsp;</td>
			            <td align="left">
                            &nbsp;<a class="lb-mt" href="forgetpassword.aspx?referer=login.aspx">Forget password?</a>
                             </td>
		            </tr>
		            <tr style="height:25px">
			            <td style="width: 7px">&nbsp;</td>
			            <td align="center" class="lb-m">
				            <asp:Label id="lblErrLoginFail" visible="false" text="Login failed." runat="server" />
				            <asp:Label id="lblErrAccInactive" visible="false" text="Your account is inactive." runat="server" />
				            <asp:Label id="lblErrAccDeleted" visible="false" text="Your account has deleted." runat="server" />
				            <asp:Label id="lblErrMesage" visible="false" Text="Error while initiating component." runat="server" />
				            <asp:Label id="lblErrExpire" visible="false" Text="days to expire date. <br>Please contact IT Administrator. <br> To Continue please input password and click Continue." runat="server" />
				            <asp:Label id="lblErrExpired" visible="false" Text="Green Golden is already expired. <br>Please contact IT Administrator." runat="server" />
				            <asp:Label id="lblErrLicense" visible="false" Text="" runat="server" />
			            </td>
		            </tr>
	            </table>
              </div>
        </div>
              
        <div class="BackgroundTopCorner"></div>
        </div> 
    </form>
</body>
</html>
