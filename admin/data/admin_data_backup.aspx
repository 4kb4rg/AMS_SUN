<%@ Page Language="vb" src="../../../include/admin_data_backup.aspx.vb"  Inherits="admin_data_backup"%>
<%@ Register TagPrefix="UserControl" Tagname="MenuAdminData" src="../../menu/menu_AdminData.ascx"%>
<%@ Register TagPrefix="Preference" Tagname="PrefHdl" src="../../include/preference/preference_handler.ascx"%>
<HTML>
	<HEAD>
		<title>admin_data_backup</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script language="javascript">
			function doKeyDown(e)
			{
				var myKeyCode = e.keyCode;

				// Shift(16), Ctrl(17), Alt(18), Del(46), Backspace(8)
				if ((myKeyCode >= 16 && myKeyCode <= 18)||(myKeyCode == 46)||(myKeyCode == 8))
					document.body.focus();
			}
		</script>
		<Preference:PrefHdl id="PrefHdl" runat="server" />
	</HEAD>
	<body>
		<form id="Form1" method="post" encType="multipart/form-data" runat="server">
			<table cellpadding="2" cellspacing="0" width="100%" border="0">
				<tr>
					<td align="center">
						<UserControl:MenuAdminData id="MenuAdminData" runat="server" />
					</td>
				</tr>
			</table>
			<div id="divBackup">
				<table cellpadding="2" cellspacing="0" width="100%" border="0">
					<tr>
						<td class="mt-h">DATABASE BACKUP</td>
					</tr>
					<tr>
						<td><hr size="1" noshade></td>
					</tr>
					<tr>
						<td>Steps</td>
					</tr>
					<tr>
						<td>1. Click "Backup" button to backup the database into backup file.</td>
					</tr>
					<tr>
						<td>2. If backup is successful, a save dialog will pop-up. Click "Save" button to save the backup file locally.</td>
					</tr>
					<tr>
						<td>Note: The entire process may take a couple of minutes.</td>
					</tr>
					<tr>
						<td>&nbsp;</td>
					</tr>
					<tr>
						<td>
							<asp:Button ID="btnBackup" Runat="server" Text="Backup"></asp:Button>
							<asp:Label ID="lblFolderNotExist" Runat="server" forecolor="red" Text="<br>Specified backup folder does not exist. Specify a valid folder and try again."></asp:Label>
							<asp:Label ID="lblUnexpectedBackupErr" Runat="server" forecolor="red" Text="<br>Backup operation encountered an unexpected error. Try again later. If problem persist, contact system administrator."></asp:Label>
							<asp:Label ID="lblNoFileGeneratedErr" Runat="server" forecolor="red" Text="<br>Backup operation encountered an unexpected error.<br>Please ensure that the Web Server and Database Server are both installed in the same physical server.<br>Try again later. If problem persist, contact system administrator."></asp:Label>
						</td>
					</tr>
					<tr><td>&nbsp;</td></tr>
<comment>
					<asp:Panel id="pnlFolderNotExist" runat="server" visible="false">
						<tr>
							<td>
								Specified backup folder does not exist. Specify a valid folder and try again.
							</td>
						</tr>
					</asp:Panel>
					<asp:Panel id="pnlUnexpectedBackupErr" runat="server" visible="false">
						<tr>
							<td>
								Backup operation encountered an unexpected error. Try again later. If problem persist, contact system administrator.
							</td>
						</tr>
					</asp:Panel>
</comment>
				</table>
			</div><br><br>
			<div id="divRestore">
				<table cellpadding="2" cellspacing="0" width="100%" border="0">
					<tr>
						<td class="mt-h">DATABASE RESTORATION</td>
					</tr>
					<tr>
						<td><hr size="1" noshade></td>
					</tr>
					<tr>
						<td>Steps</td>
					</tr>
					<tr>
						<td>1. Click "Browse" button to select your database backup file.</td>
					</tr>
					<tr>
						<td>2. Click "Upload" button to upload the selected file.</td>
					</tr>
					<tr>
						<td>3. If upload is successful, click "Restore" button to restore the database.</td>
					</tr>
					<tr>
						<td>Note: The entire process may take a couple of minutes.</td>
					</tr>
					<tr>
						<td>&nbsp;</td>
					</tr>
					<tr>
						<td>
							Database backup file:&nbsp;&nbsp;
							<input type="file" id="filUpload" runat="server" onkeypress="return false;" onpaste="return false;" onkeydown="javascript:doKeyDown(event);">&nbsp;
							<asp:Label id="lblUploaded" runat="server" forecolor="red"></asp:Label>
							<input type="hidden" id="hidRestorePath" runat="server">
						</td>
					</tr>
					<tr>
						<td>
							<asp:Button ID="btnRestore" Runat="server" Text="Restore"></asp:Button>
							<asp:Label ID="lblRestoreSuccess" Runat="server" forecolor="blue" Text="<br>Database successfully restored."></asp:Label>
							<asp:Label ID="lblUnexpectedRestoreErr" Runat="server" forecolor="red" Text="<br>Restore operation encountered an unexpected error. Try again later. If problem persist, contact system administrator."></asp:Label>
						</td>
					</tr>
					<tr><td>&nbsp;</td></tr>
<comment>
					<asp:Panel id="pnlRestoreSuccess" runat="server" visible="false">
						<tr>
							<td>
								Database successfully restored.
							</td>
						</tr>
					</asp:Panel>
					<asp:Panel id="pnlUnexpectedRestoreErr" runat="server" visible="false">
						<tr>
							<td>
								Restore operation encountered an unexpected error. Try again later. If problem persist, contact system administrator.
							</td>
						</tr>
					</asp:Panel>
</comment>
				</table>
			</div>
		</form>
	</body>
</HTML>
