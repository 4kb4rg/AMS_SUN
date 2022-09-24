<%@ Page Language="vb" src="include/banner.aspx.vb" Inherits="banner" %>

<HTML>
	<HEAD>
		<TITLE>banner</TITLE>
	</HEAD>
	<body class="baner" bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" background="en\images\bg_hdr.gif">
		<form id="form1" runat="server">
		<table width="100%" >
			<tr valign="top" HEIGHT: "35px">
				<td align="left"><asp:label id="lblComp" runat="server" Font-Size="15pt" Font-Bold="True" ForeColor="black"></asp:label></td>
			</tr>
			<tr valign="top" HEIGHT: "25px">
				<td align="right"><asp:label id="lblLoc" runat="server" Font-Size="9pt" Font-Bold="True" ForeColor="blue"></asp:label></td>
			</tr>
			<tr HEIGHT: "25px">
				<td align="right"><asp:label id="lblUser" Font-Size="9pt" Font-Bold="True" ForeColor="blue" runat="server"></asp:label></td>
			</tr>
		</table>
	    </form>
	</body>
</HTML>