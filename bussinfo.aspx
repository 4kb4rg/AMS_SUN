<%@ Page Language="vb" src="include/bussinfo.aspx.vb" Inherits="bussinfo"%>
<%@ Register TagPrefix="Preference" Tagname="PrefHdl" src="en/include/preference/preference_handler.ascx"%>

<script runat="server">

Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)

End Sub
</script>

<html>
<head>
<Preference:PrefHdl id=PrefHdl runat="server" />
<title>Business Information</title>
</head>
	<body>
		<table cellSpacing="10" cellPadding="5" width="100%" align="center" border="0">
			<tr>
				<td style="HEIGHT: 62px" colSpan="2"><asp:label id="Label9" runat="server" Font-Size="Larger" Font-Bold="True">A Complete Computing Solution for Plantation Business Management</asp:label></td>
			</tr>
			<TR height="1">
				<TD background="en/images/hr.gif" colSpan="2"></TD>
			</TR>
			<TR>
				<TD><IMG alt="" src="en/images/timeicon.gif" width="6" height="9">
					<asp:label id="Label1" runat="server" ForeColor="#C00000" Font-Bold="True">FLEXIBILITY</asp:label></TD>
				<TD></TD>
			</TR>
			<tr>
				<td style="WIDTH: 169px" vAlign="middle" align="center"><IMG alt="" src="en/images/bus01.gif" width="55" height="55"></td>
				<td vAlign="top"><asp:label id="Label3" runat="server" ForeColor="DimGray" Width="100%" height="100%">Green Golden is a totally web based plantation management system that has the flexibility to cater to the way your operation are run. It provides well-thought out business and accounting options in its setups, various security access levels, and can be deployed centrally or de-centrally. These are only some examples of its flexible design.</asp:label></td>
			</tr>
			<TR height="1">
				<TD></TD>
				<TD background="en/images/hr.gif"></TD>
			</TR>
			<TR>
				<TD><IMG alt="" src="en/images/timeicon.gif" width="6" height="9">&nbsp;<asp:label id="Label2" runat="server" ForeColor="#C00000" Font-Bold="True">FUNCTIONALITY</asp:label></TD>
				<TD></TD>
			</TR>
			<tr>
				<td style="WIDTH: 169px" vAlign="middle" align="center">&nbsp; <IMG alt="" src="en/images/bus02.gif" width="55" height="55">
				</td>
				<td vAlign="top"><asp:label id="Label4" runat="server" ForeColor="DimGray">The functionality of Green Golden makes operations less tedious whilst allowing your business capabilities to expand. Search features, user friendly screen, automatic cost distributions, criteria driven report filtering, multi-company, multi-estate, multi-semantic features and so on, all add values in facilitating your growing business.</asp:label></td>
			</tr>
			<TR height="1">
				<TD></TD>
				<TD background="en/images/hr.gif"></TD>
			</TR>
			<TR>
				<TD><IMG alt="" src="en/images/timeicon.gif" width="6" height="9">&nbsp;<asp:label id="Label7" runat="server" ForeColor="#C00000" Font-Bold="True">SCALABILITY</asp:label></TD>
				<TD></TD>
			</TR>
			<tr>
				<td style="WIDTH: 169px" vAlign="middle" align="center">&nbsp;<IMG alt="" src="en/images/bus03.gif" width="55" height="55"></td>
				<td vAlign="top"><asp:label id="Label8" runat="server" ForeColor="DimGray">The scalability of Green Golden makes it a system that grows with your business operations. From business logic, data, to networks; Green Golden was designed to allow you to scale up each of these aspects as the need arises.</asp:label></td>
			</tr>
			<TR height="1">
				<TD></TD>
				<TD background="en/images/hr.gif"></TD>
			</TR>
			<TR>
				<TD><IMG alt="" src="en/images/timeicon.gif" width="6" height="9">
					<asp:label id="Label12" runat="server" ForeColor="#C00000" Font-Bold="True">AFFORDABILITY</asp:label></TD>
				<TD></TD>
			</TR>
			<tr>
				<td style="WIDTH: 169px" vAlign="middle" align="center">&nbsp; <IMG alt="" src="en/images/bus04.gif" width="55" height="55">
				</td>
				<td vAlign="top"><asp:label id="Label6" runat="server" ForeColor="DimGray">Green Golden's affordability disguises the richness of its flexibility, functionality and scalability.</asp:label></td>
			</tr>
		</table>
	</body>
</html>
