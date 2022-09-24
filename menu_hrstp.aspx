<%@ Page Language="vb" src="../include/menu_hrstp.aspx.vb" Inherits="menu_hrstp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>GG-Menu</title>
    
   <link href="include/css/gopalms.css" rel="stylesheet" type="text/css" />
    <script language="javascript" src="/en/include/script/jscript.js" type="text/jscript"></script>

</head>
    <body bgcolor="white" style="padding-right: 0px; padding-left: 0px; margin-left: 0px; margin-right: 0px" >
    <form id="form1" runat="server">
         

            <table cellpadding="0" cellspacing="0" style="width: 100%">
	<tr>
		<td class="cell-left" valign="top">
		<table cellpadding="0" cellspacing="0" style="width: 254px; margin-top: 0px;">
 
			<tr>
				<td valign="top">
                    <div class="panel">
                        <table id="tlbStpHRHead"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							 
						</table>
					</div>
 
                    <div class="panel">
                        <table id="tlbSpc1"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							 
						</table>
					</div>
                    <div class="panel">
                        <table id="tlbStpPRHead"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							 
						</table>
					</div>
 

                    <div class="panel">
                        <table id="tlbStpHRHead_Estate"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							 
						</table>
					</div>

                    <button class="accordion">Human Resources</button> 
                    <div class="panel">
                        <table id="tlbStpHR_Estate"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink13" runat="server" NavigateUrl="/en/HR/setup/HR_Setup_Religion.aspx" target="middleFrame" text="Agama" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink9" runat="server" NavigateUrl="/en/HR/setup/HR_setup_ICType.aspx" target="middleFrame" text="Tipe ID-Card" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink5" runat="server" NavigateUrl="/en/HR/setup/HR_setup_EmpType_Estate.aspx" target="middleFrame" text="Tipe Karyawan" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink6" runat="server" NavigateUrl="/en/HR/setup/HR_setup_JobGroup_Estate.aspx" target="middleFrame" text="Group Jabatan" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>			
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink19" runat="server" NavigateUrl="/en/HR/setup/HR_Setup_Jabatan_Estate.aspx" target="middleFrame" text="Jabatan" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>	
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink34" runat="server" NavigateUrl="/en/GL/setup/TX_Setup_Kpp.aspx" target="middleFrame" text="Kantor Pelayanan Pajak" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>						 
						</table>
					</div>

                    <div class="panel">
                        <table id="tlbSpc3"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							 
						</table>
					</div>  
                    <div class="panel">
                        <table id="tlbStpPRHead_Estate"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							 
						</table>
					</div>   
                    
                    <button class="accordion">Payroll</button> 
                    <div class="panel">
                        <table id="tlbStpPR_Estate"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink1" runat="server" NavigateUrl="/en/PR/setup/PR_setup_DivCode_Estate.aspx" target="middleFrame" text="Divisi" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
 					        <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink40" runat="server" NavigateUrl="/en/PR/setup/PR_setup_DivAsisten_Estate.aspx" target="middleFrame" text="Divisi Asisten-Mandor 1" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink2" runat="server" NavigateUrl="/en/PR/setup/PR_setup_YearPlan_Estate.aspx" target="middleFrame" text="Tahun Tanam" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink12" runat="server" NavigateUrl="/en/PR/setup/PR_setup_BlokList_Estate.aspx" target="middleFrame" text="Blok" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink3" runat="server" NavigateUrl="/en/PR/setup/PR_setup_BlokBJRList_Estate.aspx" target="middleFrame" text="Bjr" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink8" runat="server" NavigateUrl="/en/PR/setup/PR_setup_GolList_Estate.aspx" target="middleFrame" text="Gol Bulanan" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink10" runat="server" NavigateUrl="/en/PR/setup/PR_setup_SalList_Estate.aspx" target="middleFrame" text="Upah Karyawan" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink20" runat="server" NavigateUrl="/en/PR/setup/PR_setup_SalaryList.aspx" target="middleFrame" text="Alokasi Pekerjaan" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink11" runat="server" NavigateUrl="/en/PR/setup/PR_Setup_TgCode_Estate.aspx" target="middleFrame" text="Kode Tanggungan" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink14" runat="server" NavigateUrl="/en/PR/setup/PR_setup_BerasRt_Estate.aspx" target="middleFrame" text="Harga Beras" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink32" runat="server" NavigateUrl="/en/PR/Setup/PR_setup_PrmBrsList_Estate.aspx" target="middleFrame" text="Premi Beras" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink28" runat="server" NavigateUrl="/en/PR/Setup/PR_setup_AstekList_Estate.aspx" target="middleFrame" text="Astek" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink29" runat="server" NavigateUrl="/en/PR/Setup/PR_Setup_CutOff_Estate.aspx" target="middleFrame" text="Cut-Off Gaji" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink30" runat="server" NavigateUrl="/en/PR/Setup/PR_Setup_AttCode_Estate.aspx" target="middleFrame" text="Kode Absensi" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink31" runat="server" NavigateUrl="/en/PR/Setup/PR_Setup_AttdList_Estate.aspx" target="middleFrame" text="Setting Kode Absensi" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink7a" runat="server" NavigateUrl="/en/PR/Setup/PR_Setup_JamKerja_Estate.aspx" target="middleFrame" text="Jam Kerja" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink7" runat="server" NavigateUrl="/en/PR/Setup/PR_Setup_Holiday_Estate.aspx" target="middleFrame" text="Hari Libur" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink7_Chg" runat="server" NavigateUrl="/en/PR/Setup/PR_Setup_Hari_Pengganti_Estate.aspx" target="middleFrame" text="Setting Hari Pengganti" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink33" runat="server" NavigateUrl="/en/PR/Setup/PR_setup_OTList_Estate.aspx" target="middleFrame" text="Tarif Lembur" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink4" runat="server" NavigateUrl="/en/PR/Setup/PR_setup_PrmiPanenList_Estate.aspx" target="middleFrame" text="Tabel Basis & Premi Panen" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink15" runat="server" NavigateUrl="/en/PR/Setup/PR_setup_DendaList_Estate.aspx" target="middleFrame" text="Denda Panen" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink16" runat="server" NavigateUrl="/en/PR/Setup/PR_setup_PrmBrondolList_Estate.aspx" target="middleFrame" text="Premi Brondolan" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink17" runat="server" NavigateUrl="/en/PR/Setup/PR_setup_PrmiKrjnnList_Estate.aspx" target="middleFrame" text="Premi Kerajinan" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink18" runat="server" NavigateUrl="/en/PR/Setup/PR_setup_PrmiDriver_Estate.aspx" target="middleFrame" text="Premi Supir" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink21" runat="server" NavigateUrl="/en/PR/Setup/PR_setup_PrmiMndorList_Estate.aspx" target="middleFrame" text="Premi Mandor" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink23" runat="server" NavigateUrl="/en/PR/Setup/PR_setup_PrmKaretList_Estate.aspx" target="middleFrame" text="Premi Deres" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink202" runat="server" NavigateUrl="/en/PR/Setup/PR_setup_PrmiLain_Estate.aspx" target="middleFrame" text="Premi Lain" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkStpFI09" runat="server" target="middleFrame" NavigateUrl="/en/PR/Setup/PR_Setup_Vehicle_Estate.aspx" text="Jenis Kenderaan"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink26" runat="server" NavigateUrl="/en/PR/setup/PR_Setup_Aktivitikategori_Estate.aspx" target="middleFrame" text="Kategori Aktiviti" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink27" runat="server" NavigateUrl="/en/PR/setup/PR_Setup_AktivitiSubkategori_Estate.aspx" target="middleFrame" text="Sub Kategori Aktiviti" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink22" runat="server" NavigateUrl="/en/PR/setup/PR_setup_AktivitiList_Estate.aspx" target="middleFrame" text="Aktiviti" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink24" runat="server" NavigateUrl="/en/PR/setup/PR_setup_Borongan_Estate.aspx" target="middleFrame" text="Borongan" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink25" runat="server" NavigateUrl="/en/PR/setup/PR_setup_ComponentGajiList_Estate.aspx" target="middleFrame" text="Komponen Gaji" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink25a" runat="server" NavigateUrl="/en/PR/setup/PR_setup_Tunjangan_Estate.aspx" target="middleFrame" text="Tunjangan Gaji" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink35" runat="server" NavigateUrl="/en/PR/setup/PR_setup_PPH21List_Estate.aspx" target="middleFrame" text="PPH21" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
							<tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink36" runat="server" NavigateUrl="/en/PR/setup/PR_setup_RapelList_Estate.aspx" target="middleFrame" text="Rapel" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
						</table>
					</div>
                    
                    <div class="panel">
                        <table id="Table2"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							 
						</table>
					</div>  
                    <div class="panel">
                        <table id="tlbStpHRHead_MILL"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							 
						</table>
					</div>  
        

                    <div class="panel">
                        <table id="tlbSpc4"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							 
						</table>
					</div>  
                    <div class="panel">
                        <table id="tlbStpPRHead_MILL"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							 
						</table>
					</div>  

    

        <div style="position:absolute; top:0px; width:87%; left:125px; height:1300px" >
          
                    <iframe id="Iframe1" name="middleFrame"  style="border-style: none; border-color: inherit; border-width: 0; width:100%; height:100%; background-color:white; margin-top:0px; margin-left: 80px;"
				        scrolling="auto" src="black.aspx"  ></iframe>
             
               </div>
 
					
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


        </form>
           

</body>
</html>

