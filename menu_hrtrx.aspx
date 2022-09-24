<%@ Page Language="vb" src="../include/menu_hrtrx.aspx.vb" Inherits="menu_hrtrx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>GG-Menu</title>
    
    <link href="include/css/gopalms.css" rel="stylesheet" type="text/css" />
</head>
<body style="margin: 0">
    <form id="form1" runat="server">

<table cellpadding="0" cellspacing="0" style="width: 100%">
	<tr>
		<td class="cell-left" valign="top">
		<table cellpadding="0" cellspacing="0" style="width: 254px">
 
			<tr>
				<td valign="top">
                    <div class="panel">
                        <table id="tblHRHead"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
                        </table>
                    </div>
                    
				    <%--<button class="accordion">Human Resources</button>--%>					
					<%--<div class="panel">
                        <table id="tblHR"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							<tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkHR01" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_EmployeeList.aspx?redirect=empdet" target="middleFrame" text="Employee Details"></asp:hyperlink>
                                </div></a></td>
							</tr>
							<tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkHR02" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_EmployeeList.aspx?redirect=emppay" target="middleFrame" text="Employee Payroll"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkHR03" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_EmployeeList.aspx?redirect=empemp" target="middleFrame" text="Employee Employment"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkHR04" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_EmployeeList.aspx?redirect=empstat" target="middleFrame" text="Employee Statutory"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkHR05" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_EmployeeList.aspx?redirect=empfam" target="middleFrame" text="Employee Family"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkHR06" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_EmployeeList.aspx?redirect=empqlf" target="middleFrame" text="Employee Qualification"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkHR07" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_EmployeeList.aspx?redirect=empskill" target="middleFrame" text="Employee Skill"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkHR08" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_EmployeeList.aspx?redirect=empcp" target="middleFrame" text="Career Progress"></asp:hyperlink>
                                </div></a></td>
							</tr>
						</table>
					</div>--%>
					
                    <div class="panel">
                        <table id="tblSpc1"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
                        </table>
                    </div>

                    <div class="panel">
                        <table id="tblPRHead"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
                        </table>
                    </div>

                    <%--<button class="accordion">Payroll</button>--%>					
					<%--<div class="panel">
                        <table id="tblPR"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							<tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkPR01" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_PieceRatePayList.aspx" target="middleFrame" text="Harvester Production Payment"></asp:hyperlink>
                                </div></a></td>
							</tr>
							<tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkPR02" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_ContractPayList.aspx" target="middleFrame" text="Contract Payment"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkPR03" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_TripList.aspx" target="middleFrame" text="Trip"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkPR04" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_WagesList.aspx"  target="middleFrame" text="Wages Payment"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkPR06" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_WPList.aspx"  target="middleFrame" text="Work Performance"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkPR07" runat="server" NavigateUrl="/en/PR/Trx/PR_trx_DailyAttd.aspx"  target="middleFrame" text="Daily Attendance"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkPR08" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_PieceAttd.aspx"  target="middleFrame" text="Harvester Production Attendance"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkPR10" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_AttdList.aspx"  target="middleFrame" text="Attendance Checkroll"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkPR11" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_ContractCheckrollList.aspx"  target="middleFrame" text="Contractor Checkroll"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkPR12" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_WPContractorList.aspx"  target="middleFrame" text="Work Performance Contractor"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkPR13" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_GenerateAttendance.aspx"  target="middleFrame" text="Generate Daily Attendance"></asp:hyperlink>
                                </div></a></td>
							</tr>
						</table>
					</div>--%>

                    <div class="panel">
                        <table id="tblSpc2"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
                        </table>
                    </div>
                    <div class="panel">
                        <table id="TblHR_EstateHead"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
                        </table>
                    </div>

                    <button class="accordion">Human Resources</button>					
					<div class="panel">
                        <table id="tblHR_Estate"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							<tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="Hyperlink1" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_EmployeeList_Estate.aspx?redirect=empdet" target="middleFrame" text="Data Karyawan"></asp:hyperlink>
                                </div></a></td>
							</tr>
							<tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="Hyperlink5" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_MandoranList_Estate.aspx" target="middleFrame" text="KeMandoran"></asp:hyperlink>
                                </div></a></td>
							</tr>
                    <%--        <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="Hyperlink3" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_EmployeeList_Estate.aspx?redirect=empdet" target="middleFrame" text="Data Karyawan"></asp:hyperlink>
                                </div></a></td>
							</tr>--%>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="Hyperlink10" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_ProDemosiList_Estate.aspx" target="middleFrame" text="Promosi / Demosi"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="Hyperlink8" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_MutasiList_Estate.aspx" target="middleFrame" text="Mutasi"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="Hyperlink9" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_ResignList_Estate.aspx" target="middleFrame" text="Karyawan Berhenti"></asp:hyperlink>
                                </div></a></td>
							</tr>
						</table>
					</div>

                    <div class="panel">
                        <table id="tblSpc3"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
                        </table>
                    </div>
                    <div class="panel">
                        <table id="TblPR_EstateHead"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
                        </table>
                    </div>

                    <button class="accordion">Payroll</button>					
					<div class="panel">
                        <table id="tblPR_Estate"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							<tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="Hyperlink2" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_AttdCheckRolllist_Estate.aspx"  target="middleFrame" text="Absensi " ></asp:hyperlink>
                                </div></a></td>
							</tr>
							<tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="Hyperlink6B" runat="server" NavigateUrl="/en/PR/Trx/PR_trx_RKBList.aspx"  target="middleFrame" text="Rencana Kerja Bulanan " ></asp:hyperlink>
                                </div></a></td>
							</tr>
							<tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="Hyperlink4" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_BKMList_Estate.aspx"  target="middleFrame" text="Buku Kerja " ></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="Hyperlink77" runat="server" NavigateUrl="/en/PR/Trx/PR_trx_KTNList_Estate.aspx"  target="middleFrame" text="Buku Kerja SPK" ></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="Hyperlink777" runat="server" NavigateUrl="/en/PR/Trx/PR_trx_BKMApproval_Estate.aspx"  target="middleFrame" text="Buku Kerja Approval" ></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="Hyperlink18" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_OvertimeList_Estate.aspx"  target="middleFrame" text="Lembur" ></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="Hyperlink6" runat="server" NavigateUrl="/en/PR/Trx/PR_trx_AngsuranList_Estate.aspx"  target="middleFrame" text="Angsuran" ></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="Hyperlink7" runat="server" NavigateUrl="/en/PR/Trx/PR_trx_BKMEmp_Estate.aspx"  target="middleFrame" text="Hasil Kerja Karyawan" ></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="Hyperlink20" runat="server" NavigateUrl="/en/PR/Trx/PR_trx_JrnPayroll_Estate.aspx"  target="middleFrame" text="Analisa Jurnal Payroll" ></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="Hyperlink21" runat="server" NavigateUrl="/en/PR/Trx/PR_trx_RincianAngsuran_Estate.aspx"  target="middleFrame" text="Daftar Rincian Angsuran" ></asp:hyperlink>
                                </div></a></td>
							</tr>
						</table>
					</div>

                    <div class="panel">
                        <table id="TblHR_MILLHead"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
                        </table>
                    </div>
                    <%--<button class="accordion">Payroll-Estate</button>					
					<div class="panel">
                        <table id="tblHR_MILL"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							<tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="HyperlinkML1" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_EmployeeList_Estate.aspx?redirect=empdet" target="middleFrame" text="Data Karyawan"></asp:hyperlink>
                                </div></a></td>
							</tr>
							<tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="HyperlinkML5" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_MandoranList_Estate.aspx" target="middleFrame" text="KeMandoran"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="HyperlinkML10" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_ProDemosiList_Estate.aspx" target="middleFrame" text="Promosi / Demosi"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="HyperlinkML8" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_MutasiList_Estate.aspx" target="middleFrame" text="Mutasi"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="HyperlinkML9" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_ResignList_Estate.aspx" target="middleFrame" text="Karyawan Berhenti"></asp:hyperlink>
                                </div></a></td>
							</tr>
						</table>
					</div>--%>

                    <div class="panel">
                        <table id="tblSpc4"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
                        </table>
                    </div>
                    <div class="panel">
                        <table id="TblPR_MILLHead"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
                        </table>
                    </div>
                    <%--<button class="accordion">Payroll-HO/MILL</button>--%>					
					<%--<div class="panel">
                        <table id="tblPR_MILL"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							<tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="HyperlinkML2" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_AttdCheckRolllist_Estate.aspx"  target="middleFrame" text="Absensi " ></asp:hyperlink>
                                </div></a></td>
							</tr>
						    <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="HyperlinkML6" runat="server" NavigateUrl="/en/PR/Trx/PR_trx_RKHList_Estate.aspx"  target="middleFrame" text="Rencana Kerja " ></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="HyperlinkML7" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_BKMList_Estate.aspx"  target="middleFrame" text="Buku Kerja " ></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="Hyperlink11" runat="server" NavigateUrl="/en/PR/Trx/PR_trx_BKMApproval_Estate.aspx"  target="middleFrame" text="Buku Kerja Approval" ></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="HyperlinkML18" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_OvertimeList_Estate.aspx"  target="middleFrame" text="Lembur" ></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="HyperlinkML3" runat="server" NavigateUrl="/en/PR/Trx/PR_trx_AngsuranList_Estate.aspx"  target="middleFrame" text="Angsuran" ></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="HyperlinkML4" runat="server" NavigateUrl="/en/PR/Trx/PR_trx_BKMEmp_Estate.aspx"  target="middleFrame" text="Hasil Kerja Karyawan" ></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="HyperlinkML20" runat="server" NavigateUrl="/en/PR/Trx/PR_trx_JrnPayroll_Estate.aspx"  target="middleFrame" text="Analisa Jurnal Payroll" ></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="HyperlinkML21" runat="server" NavigateUrl="/en/PR/Trx/PR_trx_RincianAngsuran_Estate.aspx"  target="middleFrame" text="Daftar Rincian Angsuran" ></asp:hyperlink>
                                </div></a></td>
							</tr>
						</table>
					</div>--%>

        <div style="position:absolute; top:0px; width:87%; left:125px; height:1000px" >
          
                    <iframe id="Iframe1" name="middleFrame"  style="border-style: none; border-color: inherit; border-width: 0; width:100%; height:100%; background-color:white; margin-top:0px; margin-left: 80px;"
				        scrolling="auto" src="black.aspx"  ></iframe>
             
               </div>
<%--
                    <button class="accordion">Testing</button>
					<div class="panel">
						<table cellpadding="0" cellspacing="1" style="width: 254px">
							<tr>
								<td><a href="#"><div class="fathermenu">Data Collection</div></a></td>
							</tr>
							<tr>
								<td><a href="#"><div class="fathermenu">Processing</div></a></td>
							</tr>
							 
						</table>
					</div>--%>
					
					<script>
					    var acc = document.getElementsByClassName("accordion");
					    var i;


					    for (i = 0; i < acc.length; i++) {
					        acc[i].onclick = function () {
					            this.classList.toggle("active");
					            this.nextElementSibling.classList.toggle("hide");
					        }
					    }
					</script>				

				</td>
			</tr>
		</table>

		</td>
	</tr>
</table>



         <%--  <div id="Nav" style="position:absolute; width:20%; top:0px; left:0px; height:1000px">
            	
            <table>
			    <tr height="20">
			    <td width="20"></td>
			</tr>
			</table> 

			 <table id="tblHRHead" cellSpacing="0" cellPadding="0" width="100%" runat="server">
							<tr height="20" >
								<td width="20"></td>
								<td width="14"><IMG src="images/arow.gif" border="0" align="left"></td>
								<td class="lb-hti"><A class="lb-tti" href="javascript:togglebox(tblHR);">Human Resources</A></td>
							</tr>
						</table>
						<table id="tblHR" style="VISIBILITY: hidden; POSITION: absolute" cellSpacing="0" cellPadding="0"
							width="100%" border="0" runat="server">
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkHR01" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_EmployeeList.aspx?redirect=empdet" target="middleFrame" text="Employee Details"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkHR02" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_EmployeeList.aspx?redirect=emppay" target="middleFrame" text="Employee Payroll"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkHR03" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_EmployeeList.aspx?redirect=empemp" target="middleFrame" text="Employee Employment"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkHR04" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_EmployeeList.aspx?redirect=empstat" target="middleFrame" text="Employee Statutory"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkHR05" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_EmployeeList.aspx?redirect=empfam" target="middleFrame" text="Employee Family"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkHR06" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_EmployeeList.aspx?redirect=empqlf" target="middleFrame" text="Employee Qualification"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkHR07" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_EmployeeList.aspx?redirect=empskill" target="middleFrame" text="Employee Skill"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkHR08" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_EmployeeList.aspx?redirect=empcp" target="middleFrame" text="Career Progress"></asp:hyperlink></td>
							</tr>
						</table>
						

						<table id="tblSpc1" cellSpacing="0" cellPadding="0" width="100%" runat="server">
							<tr>
								<td colSpan="2"><IMG height="0" src="images/spacer.gif" width="5" border="0"></td>
							</tr>
						</table>

		    	<table id="tblPRHead" cellSpacing="0" cellPadding="0" width="100%" runat="server">
							<tr height="20" >
								<td width="20"></td>
								<td width="14"><IMG src="images/arow.gif" border="0" align="left"></td>
								<td class="lb-hti"><A class="lb-tti" href="javascript:togglebox(tblPR);">Payroll</A></td>
							</tr>
						</table>
						<table id="tblPR" style="VISIBILITY: hidden; POSITION: absolute" cellSpacing="0" cellPadding="0"
							width="100%" border="0" runat="server">
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkPR01" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_PieceRatePayList.aspx" target="middleFrame" text="Harvester Production Payment"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkPR02" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_ContractPayList.aspx" target="middleFrame" text="Contract Payment"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkPR03" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_TripList.aspx" target="middleFrame" text="Trip"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkPR04" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_WagesList.aspx"  target="middleFrame" text="Wages Payment"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkPR05" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_ADList.aspx"  target="middleFrame" text="Allowance and Deduction"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkPR06" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_WPList.aspx"  target="middleFrame" text="Work Performance"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkPR07" runat="server" NavigateUrl="/en/PR/Trx/PR_trx_DailyAttd.aspx"  target="middleFrame" text="Daily Attendance"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkPR08" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_PieceAttd.aspx"  target="middleFrame" text="Harvester Production Attendance"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkPR09" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_WeeklyAttd.aspx"  target="middleFrame" text="Weekly Attendance"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkPR10" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_AttdList.aspx"  target="middleFrame" text="Attendance Checkroll"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkPR11" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_ContractCheckrollList.aspx"  target="middleFrame" text="Contractor Checkroll"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkPR12" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_WPContractorList.aspx"  target="middleFrame" text="Work Performance Contractor"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkPR13" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_GenerateAttendance.aspx"  target="middleFrame" text="Generate Daily Attendance"></asp:hyperlink></td>
							</tr>
						</table>
						
							<table id="tblSpc2" cellSpacing="0" cellPadding="0" width="100%" runat="server">
							<tr>
								<td colSpan="2"><IMG height="0" src="images/spacer.gif" width="5" border="0"></td>
							</tr>
						</table>

	         	<table id="TblHR_EstateHead" cellSpacing="0" cellPadding="0" width="100%" runat="server">
							<tr height="20" >
								<td width="20"></td>
								<td width="14"><IMG src="images/arow.gif" border="0" align="left"></td>
								<td class="lb-hti"><A class="lb-tti" href="javascript:togglebox(tblHR_Estate);javascript:togglebox1(tblPR_Estate);">Human Resources-Estate</A></td>
							</tr>
						</table>
						
						<table id="tblHR_Estate" style="VISIBILITY: hidden; POSITION: absolute" cellSpacing="0" cellPadding="0"
							width="100%" border="0" runat="server">
							<tr height="20">
								<td style="width: 20px"></td>
								<td width="14">&nbsp;<IMG src="images/dot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="Hyperlink1" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_EmployeeList_Estate.aspx?redirect=empdet" target="middleFrame" text="Data Karyawan"></asp:hyperlink></td>
							</tr>
							
							<tr height="20">
								<td style="width: 20px"></td>
								<td width="14">&nbsp;<IMG src="images/dot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="Hyperlink5" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_MandoranList_Estate.aspx" target="middleFrame" text="KeMandoran"></asp:hyperlink></td>
							</tr>
							
							<tr height="20">
								<td style="width: 20px"></td>
								<td width="14">&nbsp;<IMG src="images/dot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="Hyperlink10" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_ProDemosiList_Estate.aspx" target="middleFrame" text="Promosi / Demosi"></asp:hyperlink></td>
							</tr>
							
							<tr height="20">
								<td style="width: 20px"></td>
								<td width="14">&nbsp;<IMG src="images/dot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="Hyperlink8" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_MutasiList_Estate.aspx" target="middleFrame" text="Mutasi"></asp:hyperlink></td>
							</tr>
							
							<tr height="20">
								<td style="width: 20px"></td>
								<td width="14">&nbsp;<IMG src="images/dot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="Hyperlink9" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_ResignList_Estate.aspx" target="middleFrame" text="Karyawan Berhenti"></asp:hyperlink></td>
							</tr>
							
						</table>
						
						
						<table id="tblSpc3" cellSpacing="0" cellPadding="0" width="100%" runat="server">
							<tr>
								<td colSpan="2"><IMG height="0" src="images/spacer.gif" width="5" border="0"></td>
							</tr>
						</table>
    
                  <table id="TblPR_EstateHead" cellSpacing="0" cellPadding="0" width="100%" runat="server">
							<tr height="20" >
								<td width="20"></td>
								<td width="14"><IMG src="images/arow.gif" border="0" align="left"></td>
								<td class="lb-hti"><A class="lb-tti" href="javascript:togglebox1(tblHR_Estate);javascript:togglebox(tblPR_Estate);">Payroll-Estate</A></td>
							</tr>
						</table>
						<table id="tblPR_Estate" style="VISIBILITY: hidden; POSITION: absolute" cellSpacing="0" cellPadding="0"
							width="100%" border="0" runat="server">
							
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/dot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="Hyperlink2" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_AttdCheckRolllist_Estate.aspx"  target="middleFrame" text="Absensi " ></asp:hyperlink></td>
							</tr>
							
							 
									
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/dot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="Hyperlink7" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_BKMList_Estate.aspx"  target="middleFrame" text="Buku Kerja " ></asp:hyperlink></td>
							</tr>
							
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/dot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="Hyperlink77" runat="server" NavigateUrl="/en/PR/Trx/PR_trx_KTNList_Estate.aspx"  target="middleFrame" text="Buku Kerja SPK" ></asp:hyperlink></td>
							</tr>
							
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/dot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="Hyperlink777" runat="server" NavigateUrl="/en/PR/Trx/PR_trx_BKMApproval_Estate.aspx"  target="middleFrame" text="Buku Kerja Approval" ></asp:hyperlink></td>
							</tr>
							
							
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/dot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="Hyperlink18" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_OvertimeList_Estate.aspx"  target="middleFrame" text="Lembur" ></asp:hyperlink></td>
							</tr>
							
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/dot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="Hyperlink3" runat="server" NavigateUrl="/en/PR/Trx/PR_trx_AngsuranList_Estate.aspx"  target="middleFrame" text="Angsuran" ></asp:hyperlink></td>
							</tr>
							
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/dot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="Hyperlink4" runat="server" NavigateUrl="/en/PR/Trx/PR_trx_BKMEmp_Estate.aspx"  target="middleFrame" text="Hasil Kerja Karyawan" ></asp:hyperlink></td>
							</tr>
				 
							
							
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/dot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="Hyperlink20" runat="server" NavigateUrl="/en/PR/Trx/PR_trx_JrnPayroll_Estate.aspx"  target="middleFrame" text="Analisa Jurnal Payroll" ></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/dot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="Hyperlink21" runat="server" NavigateUrl="/en/PR/Trx/PR_trx_RincianAngsuran_Estate.aspx"  target="middleFrame" text="Daftar Rincian Angsuran" ></asp:hyperlink></td>
							</tr>
						</table>
						
						<table id="TblHR_MILLHead" cellSpacing="0" cellPadding="0" width="100%" runat="server">
							<tr height="20" >
								<td width="20"></td>
								<td width="14"><IMG src="images/arow.gif" border="0" align="left"></td>
								<td class="lb-hti"><A class="lb-tti" href="javascript:togglebox(tblHR_MILL);javascript:togglebox1(tblPR_MILL);">Human Resources-HO/MILL</A></td>
							</tr>
						</table>
						
						<table id="tblHR_MILL" style="VISIBILITY: hidden; POSITION: absolute" cellSpacing="0" cellPadding="0"
							width="100%" border="0" runat="server">
							<tr height="20">
								<td style="width: 20px"></td>
								<td width="14">&nbsp;<IMG src="images/dot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="HyperlinkML1" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_EmployeeList_Estate.aspx?redirect=empdet" target="middleFrame" text="Data Karyawan"></asp:hyperlink></td>
							</tr>
							
							<tr height="20">
								<td style="width: 20px"></td>
								<td width="14">&nbsp;<IMG src="images/dot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="HyperlinkML5" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_MandoranList_Estate.aspx" target="middleFrame" text="KeMandoran"></asp:hyperlink></td>
							</tr>
							
							<tr height="20">
								<td style="width: 20px"></td>
								<td width="14">&nbsp;<IMG src="images/dot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="HyperlinkML10" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_ProDemosiList_Estate.aspx" target="middleFrame" text="Promosi / Demosi"></asp:hyperlink></td>
							</tr>
							
							<tr height="20">
								<td style="width: 20px"></td>
								<td width="14">&nbsp;<IMG src="images/dot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="HyperlinkML8" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_MutasiList_Estate.aspx" target="middleFrame" text="Mutasi"></asp:hyperlink></td>
							</tr>
							
							<tr height="20">
								<td style="width: 20px"></td>
								<td width="14">&nbsp;<IMG src="images/dot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="HyperlinkML9" runat="server" NavigateUrl="/en/HR/trx/HR_Trx_ResignList_Estate.aspx" target="middleFrame" text="Karyawan Berhenti"></asp:hyperlink></td>
							</tr>
							
						</table>
						
						
						<table id="tblSpc4" cellSpacing="0" cellPadding="0" width="100%" runat="server">
							<tr>
								<td colSpan="2"><IMG height="0" src="images/spacer.gif" width="5" border="0"></td>
							</tr>
						</table>
    
                  <table id="TblPR_MILLHead" cellSpacing="0" cellPadding="0" width="100%" runat="server">
							<tr height="20" >
								<td width="20"></td>
								<td width="14"><IMG src="images/arow.gif" border="0" align="left"></td>
								<td class="lb-hti"><A class="lb-tti" href="javascript:togglebox1(tblHR_MILL);javascript:togglebox(tblPR_MILL);">Payroll-HO/MILL</A></td>
							</tr>
						</table>
						<table id="tblPR_MILL" style="VISIBILITY: hidden; POSITION: absolute" cellSpacing="0" cellPadding="0"
							width="100%" border="0" runat="server">
							
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/dot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="HyperlinkML2" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_AttdCheckRolllist_Estate.aspx"  target="middleFrame" text="Absensi " ></asp:hyperlink></td>
							</tr>
							
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/dot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="HyperlinkML6" runat="server" NavigateUrl="/en/PR/Trx/PR_trx_RKHList_Estate.aspx"  target="middleFrame" text="Rencana Kerja " ></asp:hyperlink></td>
							</tr>
							
									
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/dot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="HyperlinkML7" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_BKMList_Estate.aspx"  target="middleFrame" text="Buku Kerja " ></asp:hyperlink></td>
							</tr>
							
							
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/dot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="Hyperlink6" runat="server" NavigateUrl="/en/PR/Trx/PR_trx_BKMApproval_Estate.aspx"  target="middleFrame" text="Buku Kerja Approval" ></asp:hyperlink></td>
							</tr>
							
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/dot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="HyperlinkML18" runat="server" NavigateUrl="/en/PR/Trx/PR_Trx_OvertimeList_Estate.aspx"  target="middleFrame" text="Lembur" ></asp:hyperlink></td>
							</tr>
							
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/dot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="HyperlinkML3" runat="server" NavigateUrl="/en/PR/Trx/PR_trx_AngsuranList_Estate.aspx"  target="middleFrame" text="Angsuran" ></asp:hyperlink></td>
							</tr>
							
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/dot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="HyperlinkML4" runat="server" NavigateUrl="/en/PR/Trx/PR_trx_BKMEmp_Estate.aspx"  target="middleFrame" text="Hasil Kerja Karyawan" ></asp:hyperlink></td>
							</tr>
												
							
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/dot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="HyperlinkML20" runat="server" NavigateUrl="/en/PR/Trx/PR_trx_JrnPayroll_Estate.aspx"  target="middleFrame" text="Analisa Jurnal Payroll" ></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/dot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="HyperlinkML21" runat="server" NavigateUrl="/en/PR/Trx/PR_trx_RincianAngsuran_Estate.aspx"  target="middleFrame" text="Daftar Rincian Angsuran" ></asp:hyperlink></td>
							</tr>
						</table>


            </div>
         
             <div style="position:absolute; top:0px; width:85%; left:179px; height:1000px" >
          
              	<iframe id="Iframe1" name="middleFrame"  style="width:100%; height:100%; background-color:Black"
				 scrolling="auto" src="black.aspx"></iframe>
             
               </div>

           <div class="BackgroundTopCorner"></div>
           --%>
    </form>
</body>
</html>

