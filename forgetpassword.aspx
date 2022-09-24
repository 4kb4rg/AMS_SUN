<%@ Page Language="vb"  src="include/forgetpassword.aspx.vb" Inherits="forget_password"%>
<%@ Register TagPrefix="Preference" Tagname="PrefHdl" src="en/include/preference/preference_handler.ascx"%>
<html>
<head>
<Preference:PrefHdl id=PrefHdl runat="server" />
<title>Forget Password</title>
</head>

<body class="menu" topmargin="0" leftmargin="0">

<form id=frmForgetPwd runat=server>

	<table width=100% border=0 cellpadding=0 cellspacing=0>
		<tr height="25" >
		<td width="20" class="lb-ht"></td>
		<td width="2"></td>
		<td class="lb-ht"><span class="lb-ti">Forget Password</span></td>
		</tr>	
	</table>

	<table border=0 width=100% cellspacing=1>
		
		<tr>
			<td></td>
			<td>
				<table width=100% id=tblBefore cellpadding=0 cellspacing="0" runat=server>
					<tr>
						<td></td>
						<td>User ID:<br>
							<asp:TextBox id=txtUserId width=100% maxlength=8 runat=server />
							<asp:RequiredFieldValidator id=rfvCode display=dynamic runat=server 
								ErrorMessage="Please enter User ID." 
								ControlToValidate=txtUserId />
							<asp:RegularExpressionValidator id=revUserId 
								ControlToValidate="txtUserId"
								ValidationExpression="[a-zA-Z0-9\-]{1,8}"
								Display="Dynamic"
								text="<br>Alphanumeric without any space in between only."
								runat="server"/>
						</td>
					</tr>
					<tr height="5">
						<td colspan="3"></td>
					</tr>
					<tr>
						<td></td>
						<td>
							<asp:Button text="  OK  " onclick=OKBtn_Click id=OKBtn runat=server /> 
							<asp:Button text=" Back " CausesValidation=False onclick=BackBtn_Click id=BackBtn runat=server />
						</td>
					</tr>
					<tr class="lb-m">
						<td></td>
						<td>
							<asp:Label id=lblSuccess text="Your password will be sent to you shortly. If you still have not received your password, wait for a while before trying again." runat=server />
							<asp:Label id=lblErrNoUser visible=false text="Unknown User ID. " forecolor=red runat=server />
							<asp:Label id=lblErrNoEmail visible=false text="User email address not found." forecolor=red runat=server />
							<asp:Label id=lblErrSentFail visible=false text="The SMTP mail server connection failed." forecolor=red runat=server />
						</td>
					</tr>
				</table>				
			</td>
		</tr>
	</table>
	<input type=hidden id=hidReferer runat=server />
	<asp:Label id=lblErrMesage visible=false Text="Error while initiating component." runat=server />
</form>

</body>

</html>
