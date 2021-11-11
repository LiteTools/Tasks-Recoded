﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using ByteSizeLib;

namespace Tasks {
    public partial class frmCleanup : Form {
        public frmCleanup() { InitializeComponent(); }

        [DllImport("Shell32.dll")]
        static extern int SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleFlag dwFlags);
        enum RecycleFlag : int {
            SHERB_NOCONFIRMATION = 0x00000001,  // No confirmation, when emptying
            SHERB_NOPROGRESSUI = 0x00000001,    // No progress tracking window during the emptying of the recycle bin
            SHERB_NOSOUND = 0x00000004          // No sound when the emptying of the recycle bin is complete
        }

        private bool DeleteAllFiles(DirectoryInfo[] directoryInfos, bool condition) {
            if(condition) {
                foreach(dirInfo in directoryInfos) {
                    foreach (var file in dirInfo.GetFiles())
                        try { file.Delete(); CleanupLogsLBox.Items.Add("Deleted " + file.FullName); }
                        catch (Exception ex) { CleanupLogsLBox.Items.Add("Exception: " + ex.Message); }
                    
                    foreach (var dir in dirInfo.GetDirectories())
                        try { dir.Delete(true); CleanupLogsLBox.Items.Add("Deleted Folder " + dir.FullName); }
                        catch (Exception ex) { CleanupLogsLBox.Items.Add("Exception: " + ex.Message); }
                }
            }
            
            return condition;
        }



        private void btnCleanup_Click(object sender, EventArgs e) {
            var _up             = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            
            var chrome_ud_nodef = _up + "\\AppData\\Local\\Google\\Chrome\\User Data\\";
            var chrome_ud       = chrome_ud_nodef + "Default\\";
            var discord         = _up + "\\AppData\\Roaming\\discord";
            var edge_ud_nodef   = _up + "\\AppData\\Local\\Microsoft\\Edge\\User Data\\";
            var edge_ud         = edge_ud_nodef + "Default\\";
            
            var localappdata    = { Environment.GetEnvironmentVariable("LocalAppData") };
            var roamingappdata  = { Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) };
            var temp            = { new DirectoryInfo(Path.GetTempPath()), new DirectoryInfo("C:\\Windows\\Temp") };
            var downloads       = { new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads") };
            var prefetch        = { new DirectoryInfo("C:\\Windows\\Prefetch") };
            
            var chrome_cache    = {
                new DirectoryInfo(chrome_ud + "Cache"),
                new DirectoryInfo(chrome_ud + "Code Cache"),
                new DirectoryInfo(chrome_ud + "GPUCache"),
                new DirectoryInfo(chrome_ud_nodef + "ShaderCache"),
                new DirectoryInfo(chrome_ud + "Service Worker\\CacheStorage"),
                new DirectoryInfo(chrome_ud + "Service Worker\\ScriptCache"),
                new DirectoryInfo(chrome_ud_nodef + "GrShaderCache\\GPUCache"),
                new DirectoryInfo(chrome_ud + "File System")
            };
            var chrome_sessions = { 
                new DirectoryInfo(chrome_ud + "Sessions"),
                new DirectoryInfo(chrome_ud + "Session Storage"),
                new DirectoryInfo(chrome_ud + "Extension State")
            };
            var chrome_cookies  = {
                new DirectoryInfo(chrome_ud + "IndexedDB"),
                new DirectoryInfo(chrome_ud + "Cookies"),
                new DirectoryInfo(chrome_ud + "Cookies-journal")
            };
            var chrome_history  = {
                new DirectoryInfo(chrome_ud + "History"),
                new DirectoryInfo(chrome_ud + "History Provider Cache"),
                new DirectoryInfo(chrome_ud + "History-journal")
            };
            var discord_cache   = {
                new DirectoryInfo(discord + "Cache"),
                new DirectoryInfo(discord + "Code Cache"),
                new DirectoryInfo(discord + "GPUCache")
            };
            var discord_cookies = {
                new DirectoryInfo(discord + "Cookies"),
                new DirectoryInfo(discord + "Cookies-journal")
            };
            var edge_history    = {
                new DirectoryInfo(edge_ud + "History")
            };
            var edge_cache      = {
            };
            var edge_cookies    = {
                new DirectoryInfo(edge_ud + "Cookies"),
                new DirectoryInfo(edge_ud + "IndexedDB")
            };
            
            if (cbSystemRecycleBin.Checked) {
                SHEmptyRecycleBin(IntPtr.Zero, null, RecycleFlag.SHERB_NOSOUND | RecycleFlag.SHERB_NOCONFIRMATION);
                CleanupLogsLBox.Items.Add("Recycle Bin Cleared.");
            }
            
            if (DeleteAllFiles(downloads, cbExplorerDownloads.Checked)) CleanupLogsLBox.Items.Add("Downloads Folder Cleared.");
            if (DeleteAllFiles(temp, cbSystemTempFolders.Checked)) CleanupLogsLBox.Items.Add("Temp Folder Cleaned.");
            if (DeleteAllFiles(prefetch, cbSystemPrefetch.Checked)) CleanupLogsLBox.Items.Add("Prefetch Cleaned.");
            
            // Chrome
            if (DeleteAllFiles(chrome_cache, cbChromeCache.Checked)) CleanupLogsLBox.Items.Add("Chrome Cache cleaned.");
            if (DeleteAllFiles(chrome_sessions, cbChromeSessions.Checked)) CleanupLogsLBox.Items.Add("Chrome Sessions cleaned."); 
            if (DeleteAllFiles(chrome_cookies, cbChromeCookies.Checked)) CleanupLogsLBox.Items.Add("Chrome Cookies cleaned.");
            if (DeleteAllFiles(chrome_history, cbChromeSearchHistory.Checked)) CleanupLogsLBox.Items.Add("Chrome Search History cleaned.");
            
            // Discord
            if (DeleteAllFiles(discord_cache, cbDiscordCache.Checked)) CleanupLogsLBox.Items.Add("Discord Cache cleaned.");
            if (DeleteAllFiles(discord_cookies, cbDiscordCookies.Checked)) CleanupLogsLBox.Items.Add("Discord Cookies cleaned.");

            //Firefox

            // Firefox Cache
            if (cbFirefoxCache.Checked) {
                try {
                    var cache = (localappdata + "\\Mozilla\\Firefox\\Profiles\\");
                    foreach (string direc in Directory.EnumerateDirectories(cache)) {
                        if (direc.Contains("release") == true) {
                            var cachefile = (direc + "\\cache2");
                            foreach (string file in Directory.EnumerateFiles(cachefile)) {
                                try {
                                    File.Delete(file);
                                    CleanupLogsLBox.Items.Add("Firefox Cache Cleaned.");
                                } catch (Exception ex) { CleanupLogsLBox.Items.Add("Exception Error: " + ex); }
                            }
                            
                            foreach (string dir in Directory.EnumerateDirectories(cachefile)) {
                                try {
                                    Directory.Delete(dir, true);
                                    CleanupLogsLBox.Items.Add("Firefox Cache Cleaned.");
                                } catch (Exception ex) {
                                    CleanupLogsLBox.Items.Add("Exception Error:" + ex);
                                }
                            }
                        }
                    }
                    
                    try {
                        var profile = (roamingappdata + "\\Mozilla\\Firefox\\Profiles\\");
                        foreach (string direc in Directory.EnumerateDirectories(profile)) {
                            if (direc.Contains("release") == true) {
                                try {
                                    var shadercache = (direc + "\\shader-cache");
                                    foreach (string file in Directory.EnumerateFiles(shadercache)) {
                                        try {
                                            File.Delete(file);
                                            CleanupLogsLBox.Items.Add("Deleted File: " + file);
                                        } catch {}
                                    }
                                } catch {}
                            }
                        }
                    } catch (Exception ex) { CleanupLogsLBox.Items.Add("Error trying to delete Firefox Shader Cache. " + ex); }
                } catch (Exception ex) { CleanupLogsLBox.Items.Add("Error trying to delete firefox cache. " + ex); }
            }

            // Firefox Cookies
            if (cbFirefoxCookies.Checked) {
                try {
                    var cookies = (roamingappdata + "\\Mozilla\\Firefox\\Profiles\\");
                    foreach (string direc in Directory.EnumerateDirectories(cookies)) {
                        if (direc.Contains("release") == true) {
                            try {
                                var cookiefile = (direc + "\\cookies.sqlite");
                                File.Delete(cookiefile);
                                CleanupLogsLBox.Items.Add("Firefox Cookies Cleaned.");
                            } catch (Exception ex) {
                                CleanupLogsLBox.Items.Add("Error while trying to delete Firefox cookies! \n" + ex);
                            }
                        }
                    }
                } catch (Exception ex) { CleanupLogsLBox.Items.Add("Error while trying to delete Firefox cookies! \n" + ex); }
            }
            
            // Firefox Search History
            if (cbFirefoxSearchHistory.Checked) {
                try {
                    var cookies = (roamingappdata + "\\Mozilla\\Firefox\\Profiles\\");
                    foreach (string direc in Directory.EnumerateDirectories(cookies)) {
                        if (direc.Contains("release")) {
                            try {
                                var cookiefile = (direc + "\\places.sqlite");
                                File.Delete(cookiefile);
                                CleanupLogsLBox.Items.Add("Firefox History Cleaned.");
                            } catch (Exception ex) { CleanupLogsLBox.Items.Add("Error while trying to clean Firefox History." + ex); }
                        }
                    }
                } catch (Exception ex) { CleanupLogsLBox.Items.Add("Error when trying to delete Firefox History! \n" + ex); }
            }
            
            // DNS & ARP
            // Clear DNS
            if (cbSystemDNSCache.Checked) {
                try {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/c ipconfig /flushdns";
                    startInfo.RedirectStandardError = true;
                    process.StartInfo = startInfo;
                    process.Start();
                    CleanupLogsLBox.Items.Add("DNS Cache Cleared.");
                } catch (Exception ex) { CleanupLogsLBox.Items.Add("Error while trying to clear DNS cache!" + ex); }
            }
            
            // Clear ARP
            if (cbSystemARPCache.Checked) {
                try {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Verb = "runas"; //give cmd admin perms
                    startInfo.UseShellExecute = true;
                    startInfo.Arguments = @"/C arp -a -d";
                    process.StartInfo = startInfo;
                    process.Start();
                    CleanupLogsLBox.Items.Add("ARP Cache Cleared.");
                } catch (Exception ex) {
                    CleanupLogsLBox.Items.Add("Error while trying to clear ARP cache. " + ex);
                    MessageBox.Show(ex.ToString());
                }
            }

            // Recent Files
            if (cbExplorerRecents.Checked) {
                try {
                    CleanRecentFiles.CleanRecents.ClearAll();
                    CleanupLogsLBox.Items.Add("Recent Files Cleared.");
                } catch (Exception ex) {
                    CleanupLogsLBox.Items.Add("Error while clearing Recent Files. " + ex);
                }
            }


            if (DeleteAllFiles(edge_history, cbEdgeSearchHistory.Checked)) CleanupLogsLBox.Items.Add("Edge Search History cleaned.");
            if (DeleteAllFiles(edge_cookies, cbEdgeCookies.Checked)) CleanupLogsLBox.Items.Add("Edge Cookies Deleted.");
            if (cbEdgeCache.Checked) {
                    "\\AppData\\Local\\Microsoft\\Edge\\User Data\\Default\\Cache\\";
                    "\\AppData\\Local\\Microsoft\\Edge\\User Data\\Default\\Code Cache\\";
                    "\\AppData\\Local\\Microsoft\\Edge\\User Data\\Default\\GPUCache\\";
                    "\\AppData\\Local\\Microsoft\\Edge\\User Data\\ShaderCache\\";
                    "\\AppData\\Local\\Microsoft\\Edge\\User Data\\Default\\Service Worker\\CacheStorage\\";
                    "\\AppData\\Local\\Microsoft\\Edge\\User Data\\Default\\Service Worker\\ScriptCache\\";
                    "\\AppData\\Local\\Microsoft\\Edge\\User Data\\GrShaderCache\\GPUCache\\";
                    "\\AppData\\Local\\Microsoft\\Edge\\User Data\\Default\\Service Worker\\Database\\";
                    CleanupLogsLBox.Items.Add("Edge Cache Deleted.");
            }

            if (cbSystemEventLogs.Checked) {
                foreach (var eventLog in EventLog.GetEventLogs()) {
                    try {
                        eventLog.Clear();
                        eventLog.Dispose();
                    } catch (Exception ex) { CleanupLogsLBox.Items.Add("Error deleting Event Logs: " + ex); }
                }
            }

            if (cbEdgeSessions.Checked) {
                var directory = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Microsoft\\Edge\\User Data\\Default\\Sessions");
                var directory2 = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Microsoft\\Edge\\User Data\\Default\\Session Storage");
                var directory3 = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Microsoft\\Edge\\User Data\\Default\\Extension State");
                if (DeleteAllFiles(directory) & DeleteAllFiles(directory2) & DeleteAllFiles(directory3)) CleanupLogsLBox.Items.Add("Edge Session Deleted.");
            }

            if (cbSystemDirectXCache.Checked) {
                var directory = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\D3DSCache");
                if (DeleteAllFiles(directory)) CleanupLogsLBox.Items.Add("DirectX Shader Cache Deleted.");
            }

            if (cbSystemMemDumps.Checked) {
                var directory = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\CrashDumps\\");
                if (DeleteAllFiles(directory)) CleanupLogsLBox.Items.Add("System Memory Dumps Deleted.");
            }


            if (cbSystemErrorReporting.Checked) {
                var directory = new DirectoryInfo("C:\\ProgramData\\Microsoft\\Windows\\WER\\ReportArchive");
                if (DeleteAllFiles(directory)) CleanupLogsLBox.Items.Add("Deleted " + directory);
            }

            if (cbExplorerThumbCache.Checked) {
                var tc = (Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Microsoft\\Windows\\Explorer\\");
                foreach (string file in Directory.EnumerateFiles(tc))

                if (file.Contains("thumbcache")) {
                    try {
                        File.Delete(file);
                        CleanupLogsLBox.Items.Add("Deleted " + file);
                    } catch (Exception) {
                        CleanupLogsLBox.Items.Add("Could not delete " + file);
                    }
                }
            }

            if (cbExplorerIconCache.Checked) {
                var ic = (Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Microsoft\\Windows\\Explorer\\");
                foreach (string file in Directory.EnumerateFiles(ic))

                    if (file.Contains("iconcache") == true)
                    {
                        try
                        {
                            File.Delete(file);
                            CleanupLogsLBox.Items.Add("Deleted " + file);
                        }
                        catch (Exception)
                        {
                            CleanupLogsLBox.Items.Add("Could not delete " + file);
                        }

                    }
            }

            if (cbChromeSavedPasswords.Checked) {
                var directory = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\Login Data\\");
                if (DeleteAllFiles(directory)) CleanupLogsLBox.Items.Add("Chrome Saved Passwords Deleted.");
            }


            WriteCleanupSummary();
        }


        private void Tabs_SelectedIndexChanged(object sender, EventArgs e) {
            if (tabControl1.SelectedTab.Text == "Browser Extensions") btnCleanup.Visible = false;
            else btnCleanup.Visible = true;
        }
        
        private void frmCleanup_Load(object sender, EventArgs e) {
            tabControl1.SelectedIndexChanged += new EventHandler(Tabs_SelectedIndexChanged);
            
            DirectoryExists();
            
            // Extension Finder
            if (Directory.Exists(Dirs.chromeExtDir))
                comboBox1.Items.Add("Google Chrome");

            if (Directory.Exists(Dirs.firefoxDir))
                comboBox1.Items.Add("Mozilla Firefox");

            if (Directory.Exists(Dirs.edgeDir))
                comboBox1.Items.Add("Microsoft Edge");
        }





        private void button1_Click(object sender, EventArgs e) //DisplayDNS
        {

            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "Scripts/BatFiles/displaydns.bat";
                process.Start();
                //Directory.SetCurrentDirectory(@"/");
                //Directory.SetCurrentDirectory(@"Scripts/BatFiles");
                //Process.Start("displaydns.bat");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());

            }

        }

        private void button2_Click(object sender, EventArgs e) //DisplayARP
        {


            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "Scripts/BatFiles/displayarp.bat";
                process.Start();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());

            }



        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            var g = new Dirs();

            if (comboBox1.Text == "Google Chrome") {
                ExtensionsBox.Items.Clear();

                foreach (string ext in Directory.EnumerateDirectories(Dirs.chromeExtDir)) {
                    FileInfo fi = new FileInfo(ext);
                    ListViewItem extb = ExtensionsBox.Items.Add(fi.Name, 0);
                    DirectoryInfo fol = new DirectoryInfo(ext);
                    fol.EnumerateDirectories();
                    extb.SubItems.Add("~ " + ByteSize.FromBytes(ext.Length).ToString());

                    extb.SubItems.Add(ext);
                }
            }
            else if (comboBox1.Text == "Mozilla Firefox") {
                ExtensionsBox.Items.Clear();
                try {
                    foreach (string fol in Directory.EnumerateDirectories(Dirs.firefoxExtDir)) {
                        if (fol.Contains("-release")) {
                            string prf = (fol + "\\extensions\\");

                            try {
                                foreach (string ext in Directory.EnumerateFiles(prf)) {
                                    FileInfo fi = new FileInfo(ext);
                                    ListViewItem extb = ExtensionsBox.Items.Add(fi.Name, 0);
                                    extb.SubItems.Add("~ " + ByteSize.FromBytes(fi.Length).ToString());
                                    extb.SubItems.Add(ext);
                                }
                            }
                            catch (Exception Exc) {
                                MessageBox.Show(Exc.ToString());
                            }
                        }
                    }
                }
                catch (Exception Exc) { MessageBox.Show(Exc.ToString()); }
            } else if (comboBox1.Text == "Microsoft Edge") {
                ExtensionsBox.Items.Clear();
                foreach (string ext in Directory.EnumerateDirectories(Dirs.edgeExtDir)) {
                    FileInfo fi = new FileInfo(ext);

                    ListViewItem extb = ExtensionsBox.Items.Add(fi.Name, 0);
                    DirectoryInfo fol = new DirectoryInfo(ext);
                    fol.EnumerateDirectories();
                    extb.SubItems.Add("~ " + ByteSize.FromBytes(ext.Length).ToString());

                    extb.SubItems.Add(ext);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e) {
            if (ExtensionsBox.SelectedItems.Count >= 0) { //Check if the user selected extensions for deletion.
                /*Process process = new Process();
                process.StartInfo.FileName = "Scripts/BatFiles/killfirefox.bat";
                process.Start();
                process.WaitForExit();*/
                if (comboBox1.Text == "Google Chrome") {
                    foreach (ListViewItem eachItem in ExtensionsBox.SelectedItems) {
                        try {
                            Taskkill.Browser(1);
                            Thread.Sleep(75);
                            var item = ExtensionsBox.SelectedItems[0];
                            var subItem = item.SubItems[2].Text;
                            RemoveExt.RemoveExtension(subItem, 2);
                            ExtensionsBox.Items.Remove(eachItem);
                            CleanupLogsLBox.Items.Add("Extension Removed.");
                        } catch (Exception ex) { CleanupLogsLBox.Items.Add("Error while trying to remove extension." + ex); }
                    }
                }

                if (comboBox1.Text == "Mozilla Firefox") {
                    Taskkill.Browser(2);
                    Thread.Sleep(75); //Short threadsleep or else the extension deleter would start before firefox is fully killed for some reasons ?

                    try {
                        RemoveExt.RemoveExtension(ExtensionsBox.SelectedItems[0].SubItems[2].Text, 1);

                        foreach (ListViewItem eachItem in ExtensionsBox.SelectedItems) {
                            ExtensionsBox.Items.Remove(eachItem);
                            CleanupLogsLBox.Items.Add("Extension Removed.");
                        }
                    } catch { CleanupLogsLBox.Items.Add("Failed to remove extension"); }
                }


                if (comboBox1.Text == "Microsoft Edge") {
                    foreach (ListViewItem eachItem in ExtensionsBox.SelectedItems) {
                        try {
                            Taskkill.Browser(3);
                            Thread.Sleep(75);
                            var item = ExtensionsBox.SelectedItems[0];
                            var subItem = item.SubItems[2].Text;
                            RemoveExt.RemoveExtension(subItem, 2);
                            ExtensionsBox.Items.Remove(eachItem);
                            CleanupLogsLBox.Items.Add("Extension Removed.");
                        } catch (Exception ex) { CleanupLogsLBox.Items.Add("Error while trying to remove extension." + ex); }
                    }
                }
            } else MessageBox.Show("Please select an extension to remove.");
        }



        private void button5_Click(object sender, EventArgs e) {
            try { RunFile.RunBat("Scripts/BatFiles/byesolitaire.bat", true); }
            catch (Exception ex) { MessageBox.Show("An error occurred." + ex); }
        }

        private void button7_Click(object sender, EventArgs e) {
            try { Process.Start("powershell", "-ExecutionPolicy Bypass -File Scripts/Debloater/DisableCortana.ps1"); }
            catch (Exception ex) { MessageBox.Show("An error occurred." + ex); }
        }

        private void button6_Click(object sender, EventArgs e) {
            try { Process.Start("powershell", "-ExecutionPolicy Bypass  -File Scripts/Debloater/UninstallOneDrive.ps1"); }
            catch (Exception ex) { MessageBox.Show("An error occurred." + ex); }
        }

        private void button4_Click(object sender, EventArgs e) {
            RunFile.RunBat("removeedge.bat", true);
        }
        
        private void DirectoryExists() {
            // Todo: Check if the applications are on the computer and disable the checkboxes if it doesn't exist.
            var localappdata = Environment.GetEnvironmentVariable("LocalAppData");
            var roamingappdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            Dirs.chromeDir = localappdata + "\\Google\\Chrome\\";
            Dirs.chromeExtDir = (Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\Extensions");
            Dirs.firefoxDir = localappdata + "\\Mozilla\\Firefox\\";
            Dirs.firefoxExtDir = roamingappdata + "\\Mozilla\\Firefox\\Profiles\\";
            Dirs.edgeDir = (Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Microsoft\\Edge\\");
            Dirs.edgeExtDir = (Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Microsoft\\Edge\\User Data\\Default\\Extensions\\");
            Dirs.discordDir = localappdata + "\\Discord\\"; // Makes more sense checking appdata than program files


            if (!Directory.Exists(Dirs.chromeDir)) {
                cbChromeCache.Enabled = false;
                cbChromeCookies.Enabled = false;
                cbChromeSearchHistory.Enabled = false;
                cbChromeSessions.Enabled = false;
                cbChromeSavedPasswords.Enabled = false;
                lblChromeNotDetected.Visible = true;
            }

            if (!Directory.Exists(Dirs.firefoxDir)) {
                cbFirefoxCache.Enabled = false;
                cbFirefoxCookies.Enabled = false;
                cbFirefoxSearchHistory.Enabled = false;
                lblFirefoxNotDetected.Visible = true;
            }

            if (!Directory.Exists(Dirs.discordDir)) {
                cbDiscordCache.Enabled = false;
                cbDiscordCookies.Enabled = false;
                lblDiscordNotDetected.Visible = true;

            }

            if (!Directory.Exists(Dirs.edgeDir)) {
                cbEdgeCache.Enabled = false;
                cbEdgeCookies.Enabled = false;
                cbEdgeSearchHistory.Enabled = false;
                cbEdgeSessions.Enabled = false;
                lblEdgeNotDetected.Visible = true;
            }
        }

        private void cbEdgeCookies_CheckStateChanged(object sender, EventArgs e) {
            try { taskDialog1.Show(); }
            catch { Console.WriteLine("An error has occurred."); }
        }

        private void cbChromeCache_CheckStateChanged(object sender, EventArgs e) {}

        private void cbFirefoxCookies_CheckStateChanged(object sender, EventArgs e) {
            try { taskDialog1.Show(); }
            catch { Console.WriteLine("An error has occurred."); }
        }

        private void cbChromeCookies_CheckStateChanged(object sender, EventArgs e) {
            try { taskDialog1.Show(); }
            catch { Console.WriteLine("An error has occurred."); }
        }

        public void WriteCleanupSummary() {
            int t = (int)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);
            File.WriteAllLines(
              Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Tasks"), "Cleanup Summary") + "\\tasks-cleanup-summary-" + t + ".txt",
              CleanupLogsLBox.Items.Cast<string>().ToArray()
            );
            MessageBox.Show("Cleanup is logged at " + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "Tasks\\" + "Cleanup Summary" +"\\tasks-cleanup-summary-" + t + ".txt");
        }
    }
}
