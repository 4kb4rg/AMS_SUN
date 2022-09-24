<%@ Page Language="vb" src="include/login.aspx.vb" Inherits="login"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <title>GPS - Login</title>
    <link rel="shortcut icon" href="images/gopalm_ico.png">
    <link href="en/include/css/gopalms.css" rel="stylesheet" type="text/css" />
 
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
<body style="margin: 0; background-color: #d0d0d0; onload="javascript:onload_setfocus();">
    <form id="frmLogin" runat="server">
    
    <table cellpadding="0" cellspacing="0" style="width: 90%; height: 90%;" align="center" class="main-modul-bg">
	<tr>
		<td valign="top">
		<table cellpadding="0" cellspacing="0" style="width: 90px">
			<tr>
				<td>&nbsp;</td>
			</tr>
		</table>
		</td>
		
		<td valign="top">
		<table cellpadding="0" cellspacing="0" style="width: 360px; height: 630px">
			<tr>
				<td class="main-login" valign="top" style="height: 630px">
				<table cellpadding="0" cellspacing="0" style="width: 100%">
					<tr>
						<td class="cell-center" style="height: 250px">&nbsp;</td>
					</tr>
					<tr>
						<td class="cell-center"><span class="font16">USER LOGIN</span></td>
					</tr>
					<tr>
						<td class="cell-center">&nbsp;</td>
					</tr>
					<tr>
						<td class="cell-center">
						<table align="center" cellpadding="0" cellspacing="0" style="width: 288px">
							<tr>
								<td class="cell-center"><span class="font9"></span></td>
							</tr>
						</table>
						<br>
						 <asp:TextBox id="txtUserId" CssClass="font12Tahoma" maxlength="16" runat="server" 
                                Width="250px" Height="21px" align="center" text-align="center" placeholder="Enter your username" /></td>
					</tr>
					<tr>
						<td class="cell-center"></td>
					</tr>
					<tr>
						<td class="cell-center">
						<table align="center" cellpadding="0" cellspacing="0" style="width: 288px">
							<tr>
								<td class="cell-center"><span class="font9"></span></td>
							</tr>
						</table>
						<br>
						<strong> <asp:TextBox textmode="password" id="txtPassword" CssClass="font12Tahoma"  
                                maxlength="16" runat="server" align="center" Width="250px" Height="21px" text-align="center" placeholder="Enter your password"/></strong> </td>
					</tr>
                    <tr>
                        <td class="cell-center" style="height: 19px">
				            <asp:Label id="lblLoginResult" forecolor="Red" runat="server" /></td>
                    </tr>
					<tr>
						<td class="cell-center">&nbsp;<asp:Button text="  Login  "  onclick="loginBtn_Click" id="Button1" runat="server" CssClass="button-form"/>
				            &nbsp;<asp:Button text="  Continue  "  onclick="continueBtn_Click" id="continueBtn" Visible=false runat="server" CssClass="button-form"/></td>
					</tr>
					<tr>
						<td class="cell-center">
                            &nbsp;
                            <asp:RequiredFieldValidator id="validateUserID" display="dynamic" runat="server" 
					            ErrorMessage="Please enter your User ID." 
					            ControlToValidate="txtUserId" /></td>
					</tr>
					<tr>
						<td class="cell-center">
								<asp:RequiredFieldValidator id="validatePassword" display="dynamic" runat="server" 
					            ErrorMessage="Please enter your Password." 
					            ControlToValidate="txtPassword" />
						</td>
					</tr>
					<tr>
						<td class="cell-center">
								 &nbsp;<a class="lb-mt" href="forgetpassword.aspx?referer=login.aspx" style="color:black">Forget password?</a>
						</td>
					</tr>
					
				</table>
			</tr>
			<tr>
				<td valign="top" style="height: 68px">
				&nbsp;</td>
			</tr>
		</table>
		</td>
		
		<td class="main-bg" style="width: 100%" valign="top">
		&nbsp;</td>
	</tr>
</table>

    
        <div class="view" id="view">
        
        <div style="left: 300px; position: absolute; top: 200px; z-index: 400; height:200px" >
            <div>
                <p>&nbsp;</p>
            </div>
            
                <div class="login" id="login" style="left: 100px; position: absolute; top: 70px; height:200px">
                   <table border="0" cellspacing="3" style="width: 241%">
		            <tr style="height:25px">
			            <td style="width: 7px"></td>
			            <td><br></td>
		            </tr>
		            <tr style="height:25px">
			            <td style="width: 7px"></td>
			            <td><br></td>
		            </tr>
		            <tr style="height:25px">
			            <td style="width: 7px"></td>
			            <td>
                            &nbsp;
			            </td>
		            </tr>
			    <tr>  
			   <td style="width: 7px">&nbsp;</td>
			   <td>&nbsp;</td>
			   </tr>
		            <tr style="height:25px">
			            <td style="width: 7px"></td>
			            <td>
                            &nbsp;&nbsp;
			            </td>
		            </tr>
		            <tr style="height:25px">
			            <td style="width: 7px">&nbsp;</td>
			            <td align="left">
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
