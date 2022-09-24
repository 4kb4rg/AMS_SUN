<%@ Page src="include/default.aspx.vb" Inherits="defaultPage" %>

<HTML>
<head>
<title>GOPalms - Trial</title>
<script>
if (window!= top)
	top.location.href=location.href
</script>
</head>
<frameset border="0" framespacing="0" frameborder="0">
<frame id="Frame1" name="right" src="/logintrial.aspx"  runat="server" />
</frameset>
</HTML>
