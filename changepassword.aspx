<%@ Page Language="vb"  src="include/changepassword.aspx.vb" Inherits="change_password"%>
<%@ Register TagPrefix="Preference" Tagname="PrefHdl" src="en/include/preference/preference_handler.ascx"%>
<html>
<head>
<title>Change Password</title>
</head>
<body>
<Preference:PrefHdl id=PrefHdl runat="server" />
<form id=frmMain runat=server>

	<table width=100% border=0 cellpadding=0 cellspacing=0>
		<tr height="25" >
		<td width="20" class="lb-ht"></td>
		<td width="0"></td>
		<td class="lb-ht"><span class="lb-ti">Change Password</span></td>
		</tr>	
	</table>

	<table border=0 width=100% cellspacing=1>
		
		<tr>
			<td></td>
			<td>
				<table width=100% id=tblBefore cellpadding=0 cellspacing="0" runat=server>
					<tr>
						<td></td>
						<td>New Password:<br>
							<asp:TextBox textmode=password id=txtPassword width=40% maxlength=8 runat=server />
						</td>
					</tr>
					<tr>
						<td></td>
						<td>Confirm Password:<br>
							<asp:TextBox textmode=password id=txtConfirmPwd width=40% maxlength=8 runat=server />
							<asp:Label id=lblPassword Text="Please re-enter your password." forecolor=red runat=server />
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
							<asp:Label id=lblSuccess text="Your password was changed,trying login again klik button back." runat=server />
							<asp:Label id=lblErrNoUser visible=false text="change password failed " forecolor=red runat=server />
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
