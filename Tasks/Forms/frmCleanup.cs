/*
    (c) LiteTools 2022 (https://github.com/LiteTools)
    All rights reserved under the GNU General Public License v3.0.
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

// TODO: More Cleaning Support!!!
// TODO: Output message shows file not generic error 
// TODO: Even out all messages in app
namespace Tasks
{
    public partial class frmCleanup : Form
    {
        public bool isCleanup;
        public frmCleanup() { InitializeComponent(); CheckTheme(); isCleanup = false; }

        /* 
         CheckTheme checks what theme is currently set.
        */
        public void CheckTheme()
        {
            if (Properties.Settings.Default.Theme == "light")
            {
                pictureBox2.Image = Tasks.Properties.Resources.QuickClean_Black;
                this.BackColor = Color.FromArgb(250, 250, 250);
                cbChromeCache.ForeColor = Color.Black;
                cbChromeCookies.ForeColor = Color.Black;
                cbChromeSavedPasswords.ForeColor = Color.Black;
                cbChromeSearchHistory.ForeColor = Color.Black;
                cbChromeSessions.ForeColor = Color.Black;
                cbDiscord.ForeColor = Color.Black;
                cbEdgeCache.ForeColor = Color.Black;
                cbEdgeCookies.ForeColor = Color.Black;
                cbEdgeSearchHistory.ForeColor = Color.Black;
                cbEdgeSessions.ForeColor = Color.Black;
                cbExplorerDownloads.ForeColor = Color.Black;
                cbExplorerIconCache.ForeColor = Color.Black;
                cbExplorerRecents.ForeColor = Color.Black;
                cbExplorerThumbCache.ForeColor = Color.Black;
                cbFirefoxCache.ForeColor = Color.Black;
                cbFirefoxCookies.ForeColor = Color.Black;
                cbFirefoxSearchHistory.ForeColor = Color.Black;
                cbSystemARPCache.ForeColor = Color.Black;
                cbSystemDirectXCache.ForeColor = Color.Black;
                cbSystemDNSCache.ForeColor = Color.Black;
                cbSystemErrorReporting.ForeColor = Color.Black;
                cbSystemEventLogs.ForeColor = Color.Black;
                cbSystemMemDumps.ForeColor = Color.Black;
                cbSystemPrefetch.ForeColor = Color.Black;
                cbSystemRecycleBin.ForeColor = Color.Black;
                cbSystemTempFolders.ForeColor = Color.Black;
                cbWindowsLogFiles.ForeColor = Color.Black;
                cbIECache.ForeColor = Color.Black;
                cbOneDriveCache.ForeColor = Color.Black;
                cbVLCCache.ForeColor = Color.Black;
                cbSpotifyCache.ForeColor = Color.Black;

                tabPage1.BackColor = Color.White;
                tabPage7.BackColor = Color.White;
                tabPage3.BackColor = Color.White;
                tabControl2.BackColor = Color.White;
                tabControl1.BackColor = Color.White;
                tabPage5.BackColor = Color.White;
                tabPage6.BackColor = Color.White;
                label1.ForeColor = Color.Black;
                label2.ForeColor = Color.Black;
                label3.ForeColor = Color.Black;
                label4.ForeColor = Color.Black;
                label6.ForeColor = Color.Black;
                label9.ForeColor = Color.Black;
                label10.ForeColor = Color.Black;
                label11.ForeColor = Color.Black;
                label15.ForeColor = Color.Black;
                label17.ForeColor = Color.DarkRed;
                label18.ForeColor = Color.Black;

                label9.ForeColor = Color.Black;
                comboBox1.BackColor = Color.WhiteSmoke;
                comboBox1.ForeColor = Color.Black;
                ExtensionsBox.BackColor = Color.White;
                ExtensionsBox.ForeColor = Color.Black;
            }
        }

         /* 
         DeleteAllFiles runs a loop in the cleanup scripts to remove bloat files.
        */
        private bool DeleteAllFiles(DirectoryInfo directoryInfo)
        {

            foreach (var file in directoryInfo.GetFiles())
            {
                try
                {
                // Deletes all files in directory.
                    file.Delete();
                    CleanupLogsLBox.Items.Add(LogSuccess + file.FullName);
                }
                catch (Exception ex)
                {
                    CleanupLogsLBox.Items.Add(LogError + ex.Message);
                }

            }
            foreach (var dir in directoryInfo.GetDirectories())
            {
                try
                {
                // Deletes all directories in a directory.
                    dir.Delete(true);
                    CleanupLogsLBox.Items.Add(LogSuccess + dir.FullName);
                }
                catch (Exception ex)
                {
                    CleanupLogsLBox.Items.Add(LogError + ex.Message);
                }

            }

            return true;
        }
        
        public static string LogError = "Error while trying to delete ";
        public static string LogSuccess = "Deleted ";
        private void button8_Click(object sender, EventArgs e)
        {
            var localappdata = Environment.GetEnvironmentVariable("LocalAppData");
            var roamingappdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var windowstemp = new DirectoryInfo("C:\\Windows\\Temp\\");
            var usertemp = new DirectoryInfo(Path.GetTempPath());
            var downloads = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads\\");


            if (cbExplorerDownloads.Checked)
                try
                {
                    if (DeleteAllFiles(downloads)) CleanupLogsLBox.Items.Add("Downloads Folder Cleared.");
                }
                catch (Exception ex)
                {
                    CleanupLogsLBox.Items.Add("Error deleting the Downloads Folder. " + ex);
                }

            if (cbSystemRecycleBin.Checked)
                try
                {
                    Core.Utils.CleanupUtils.SHEmptyRecycleBin(IntPtr.Zero, null, Core.Utils.CleanupUtils.RecycleFlag.SHERB_NOSOUND | Core.Utils.CleanupUtils.RecycleFlag.SHERB_NOCONFIRMATION);
                    CleanupLogsLBox.Items.Add("Recycle Bin Cleared.");
                }
                catch (Exception ex)
                {
                    CleanupLogsLBox.Items.Add("Error deleting the Recycle Bin. " + ex);
                }


            if (cbSystemTempFolders.Checked)
            {
                try
                {
                    if (DeleteAllFiles(windowstemp)) CleanupLogsLBox.Items.Add("System Temp Folder Deleted.");
                    if (DeleteAllFiles(usertemp)) CleanupLogsLBox.Items.Add("User Temp Folder Deleted.");
                }
                catch (Exception ex)
                {
                    CleanupLogsLBox.Items.Add("Error while deleting temp folders. " + ex);
                }

            }

            if (cbSystemPrefetch.Checked)
            {
                try
                {
                    var directory = new DirectoryInfo("C:\\Windows\\Prefetch");
                    if (DeleteAllFiles(directory)) CleanupLogsLBox.Items.Add("Prefetch Deleted.");
                }
                catch (Exception ex)
                {
                    CleanupLogsLBox.Items.Add("Error while deleting Prefetch. " + ex);
                }
            }


            if (cbChromeCache.Checked)
            {
                try
                {
                    string mainSubdirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Google\\Chrome\\User Data\\";
                    string[] userDataCacheDirs = { "Default\\Cache\\", "Default\\Code Cache\\", "Default\\GPUCache\\", "ShaderCache\\", "Default\\Service Worker\\CacheStorage\\", "Default\\Service Worker\\ScriptCache\\", "GrShaderCache\\GPUCache\\", "Default\\File System\\", "Default\\JumpListIconsMostVisited\\", "Default\\JumpListIconsRecentClosed\\", "Default\\Service Worker\\Database\\" };
                    List<DirectoryInfo> directoryInfos = new List<DirectoryInfo>();

                    foreach (string subdir in userDataCacheDirs)
                    {
                        // Make a new DirectoryInfo with the info of that subdirectory and then add it into the directoryInfos array
                        directoryInfos.Add(new DirectoryInfo(mainSubdirectory + subdir + "\\"));
                    }

                    bool isDeleted = true;
                    // For each DirectoryInfo inside of the directoryInfos array
                    foreach (DirectoryInfo d in directoryInfos)
                    {
                        try
                        {
                            DeleteAllFiles(d);
                        }
                        catch (Exception ex)
                        {
                            CleanupLogsLBox.Items.Add("Error deleting Chrome Cache. " + ex);
                        }
                        // If DeleteAllFiles returns false, set the isDeleted value to false
                        if (!DeleteAllFiles(d))
                            isDeleted = false;
                    }
                    if (isDeleted)
                        CleanupLogsLBox.Items.Add("Chrome Cache Deleted.");
                }
                catch (Exception)
                {

                }
            }

            if (cbChromeSessions.Checked)
            {
                try
                {
                    var directory = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\Sessions\\");
                    var directory2 = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\Session Storage\\");
                    var directory3 = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\Extension State\\");
                    if (DeleteAllFiles(directory) & DeleteAllFiles(directory2) & DeleteAllFiles(directory3)) CleanupLogsLBox.Items.Add("Chrome Sessions Deleted.");
                }
                catch
                {
                    CleanupLogsLBox.Items.Add("Unable to delete Chrome Sessions.");
                }
            }

            if (cbChromeCookies.Checked)
            {
                try
                {
                    var directory = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\IndexedDB\\");
                    File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\Cookies");
                    File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\Cookies-journal");
                    var directory2 = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\Network\\");
                    if (DeleteAllFiles(directory) & DeleteAllFiles(directory2)) CleanupLogsLBox.Items.Add("Chrome Cookies Deleted.");
                }
                catch
                {
                    CleanupLogsLBox.Items.Add("Unable to delete Chrome Cookies.");
                }

            }


            if (cbChromeSearchHistory.Checked)
            {
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\History");
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\History Provider Cache");
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\History-journal");
                CleanupLogsLBox.Items.Add("Chrome Search History Deleted.");
            }


            if (cbDiscord.Checked)
            {
                try
                {
                    File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Roaming\\discord\\Cookies");
                    File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Roaming\\discord\\Cookies-journal");
                    CleanupLogsLBox.Items.Add("Discord Cookies Deleted.");

                    var directory = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Roaming\\discord\\Cache");
                    var directory2 = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Roaming\\discord\\Code Cache");
                    var directory3 = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Roaming\\discord\\GPUCache");
                    if (DeleteAllFiles(directory) & DeleteAllFiles(directory2) & DeleteAllFiles(directory3)) CleanupLogsLBox.Items.Add("Discord Cache Deleted.");
                }
                catch (Exception ex)
                {
                    CleanupLogsLBox.Items.Add("Error deleting Discord Cache." + ex);
                }

            }

            // TODO: Improve the Firefox Cleanups so it takes up less code + works better.
            if (cbFirefoxCache.Checked)
            {
                try
                {
                    var cache = (localappdata + "\\Mozilla\\Firefox\\Profiles\\");
                    foreach (string direc in Directory.EnumerateDirectories(cache))
                    {
                        if (direc.Contains("release") == true)
                        {
                            var cachefile = (direc + "\\cache2");
                            foreach (string file in Directory.EnumerateFiles(cachefile))
                            {
                                try
                                {
                                    File.Delete(file);
                                    CleanupLogsLBox.Items.Add("Firefox Cache Deleted.");
                                }
                                catch (Exception ex)
                                {
                                    CleanupLogsLBox.Items.Add("Exception Error: " + ex);
                                }

                            }
                            foreach (string dir in Directory.EnumerateDirectories(cachefile))
                            {
                                try
                                {
                                    Directory.Delete(dir, true);
                                    CleanupLogsLBox.Items.Add("Firefox Cache Deleted.");
                                }
                                catch (Exception ex)
                                {
                                    CleanupLogsLBox.Items.Add("Exception Error:" + ex);
                                }

                            }
                        }
                    }

                    try
                    {

                        var profile = (roamingappdata + "\\Mozilla\\Firefox\\Profiles\\");
                        foreach (string direc in Directory.EnumerateDirectories(profile))
                        {
                            if (direc.Contains("release") == true)
                            {
                                try
                                {
                                    var shadercache = (direc + "\\shader-cache");
                                    foreach (string file in Directory.EnumerateFiles(shadercache))
                                    {
                                        try
                                        {
                                            File.Delete(file);
                                            CleanupLogsLBox.Items.Add("Deleted File: " + file);
                                        }
                                        catch
                                        {
                                            //do nothing
                                        }

                                    }

                                }
                                catch
                                {
                                    //do nothing
                                }

                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        CleanupLogsLBox.Items.Add("Error trying to delete Firefox Shader Cache. " + ex);
                    }

                }
                catch (Exception ex)
                {
                    CleanupLogsLBox.Items.Add("Error trying to delete Firefox cache. " + ex);
                }

            }

            if (cbFirefoxCookies.Checked)
            {
                try
                {
                    var cookies = (roamingappdata + "\\Mozilla\\Firefox\\Profiles\\");
                    foreach (string direc in Directory.EnumerateDirectories(cookies))
                    {
                        if (direc.Contains("release") == true)
                        {
                            try
                            {
                                var cookiefile = (direc + "\\cookies.sqlite");
                                File.Delete(cookiefile);
                                CleanupLogsLBox.Items.Add("Firefox Cookies Deleted.");

                            }
                            catch (Exception ex)
                            {

                                CleanupLogsLBox.Items.Add("Error while trying to delete Firefox cookies. " + ex);
                            }

                        }
                    }

                }
                catch (Exception ex)
                {
                    CleanupLogsLBox.Items.Add("Error while trying to delete Firefox cookies. " + ex);
                }


            }
            if (cbFirefoxSearchHistory.Checked)
            {
                try
                {

                    var cookies = (roamingappdata + "\\Mozilla\\Firefox\\Profiles\\");
                    foreach (string direc in Directory.EnumerateDirectories(cookies))
                    {
                        if (direc.Contains("release") == true)
                        {
                            try
                            {
                                var cookiefile = (direc + "\\places.sqlite");
                                File.Delete(cookiefile);
                                CleanupLogsLBox.Items.Add("Firefox History Deleted.");

                            }
                            catch (Exception ex)
                            {
                                CleanupLogsLBox.Items.Add("Error while trying to delete Firefox History." + ex);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    CleanupLogsLBox.Items.Add("Error when trying to delete Firefox History. " + ex);
                }
            }

           
            if (cbSystemDNSCache.Checked)
            {
                StringBuilder sb = new StringBuilder();
                try
                {
                    Process process = new Process();
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/c ipconfig /flushdns";
                    startInfo.RedirectStandardError = true;
                    process.StartInfo = startInfo;
                    process.OutputDataReceived += (sender, args) => sb.AppendLine(args.Data);

                    CleanupLogsLBox.Items.Add("DNS Cache Cleared.");
                }
                catch (Exception ex)
                {
                    CleanupLogsLBox.Items.Add("Error while trying to clear DNS cache." + ex);
                }
            }
            if (cbSystemARPCache.Checked)
            {
                try
                {
                    Process process = new Process();
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Verb = "runas"; //give cmd admin perms
                    startInfo.UseShellExecute = true;
                    startInfo.Arguments = @"/C arp -a -d";
                    process.StartInfo = startInfo;
                    process.Start();
                    CleanupLogsLBox.Items.Add("ARP Cache Cleared.");
                }
                catch (Exception ex)
                {
                    CleanupLogsLBox.Items.Add("Error while trying to clear ARP cache. " + ex);
                    MessageBox.Show(ex.ToString());
                }
            }


            if (cbExplorerRecents.Checked)
            {
                try
                {
                    Core.Utils.CleanupUtils.SHAddToRecentDocs(Core.Utils.CleanupUtils.ShellAddToRecentDocsFlags.Pidl, null);
                    CleanupLogsLBox.Items.Add("Recent Files Cleared.");
                }
                catch (Exception ex)
                {
                    CleanupLogsLBox.Items.Add("Error while clearing Recent Files. " + ex);
                }
            }


            if (cbEdgeSearchHistory.Checked)
            {
                try
                {
                    File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Microsoft\\Edge\\User Data\\Default\\History\\");
                    CleanupLogsLBox.Items.Add("Edge Search History Deleted.");
                }
                catch
                {
                    CleanupLogsLBox.Items.Add("Error while deleting Edge Search History.");
                }

            }


            if (cbEdgeCookies.Checked)
            {
                try
                {
                    File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Microsoft\\Edge\\User Data\\Default\\Cookies\\");
                    var directory = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Microsoft\\Edge\\User Data\\Default\\IndexedDB\\");
                    if (DeleteAllFiles(directory)) CleanupLogsLBox.Items.Add("Edge Cookies Deleted.");
                }
                catch
                {
                    CleanupLogsLBox.Items.Add("Error while deleting Edge Cookies.");
                }

            }



            if (cbEdgeCache.Checked)
            {
                try
                {
                    var directory = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Microsoft\\Edge\\User Data\\Default\\Cache\\");
                    var directory2 = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Microsoft\\Edge\\User Data\\Default\\Code Cache\\");
                    var directory3 = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Microsoft\\Edge\\User Data\\Default\\GPUCache\\");
                    var directory4 = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Microsoft\\Edge\\User Data\\ShaderCache\\");
                    var directory5 = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Microsoft\\Edge\\User Data\\Default\\Service Worker\\CacheStorage\\");
                    var directory6 = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Microsoft\\Edge\\User Data\\Default\\Service Worker\\ScriptCache\\");
                    var directory7 = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Microsoft\\Edge\\User Data\\GrShaderCache\\GPUCache\\");
                    var directory8 = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Microsoft\\Edge\\User Data\\Default\\Service Worker\\Database\\");
                    if (DeleteAllFiles(directory) & DeleteAllFiles(directory2) & DeleteAllFiles(directory3) & DeleteAllFiles(directory4) & DeleteAllFiles(directory5) & DeleteAllFiles(directory6) & DeleteAllFiles(directory7) & DeleteAllFiles(directory8)) CleanupLogsLBox.Items.Add("Edge Cache Deleted.");
                }
                catch
                {
                    CleanupLogsLBox.Items.Add("Error while deleting Edge Cache.");
                }
            }

            if (cbSystemEventLogs.Checked)
            {
                try
                {
                    foreach (var eventLog in EventLog.GetEventLogs())
                    {
                        eventLog.Clear();
                        CleanupLogsLBox.Items.Add("Event Logs Deleted.");
                    }
                }
                catch (Exception ex)
                {
                    CleanupLogsLBox.Items.Add("Error deleting Event Logs. " + ex);
                }


            }

            if (cbEdgeSessions.Checked)
            {
                try
                {
                    var directory = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Microsoft\\Edge\\User Data\\Default\\Sessions\\");
                    var directory2 = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Microsoft\\Edge\\User Data\\Default\\Session Storage\\");
                    var directory3 = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Microsoft\\Edge\\User Data\\Default\\Extension State\\");
                    if (DeleteAllFiles(directory) & DeleteAllFiles(directory2) & DeleteAllFiles(directory3)) CleanupLogsLBox.Items.Add("Edge Session Deleted.");
                }

                catch
                {
                    CleanupLogsLBox.Items.Add("Unable to delete Edge Sessions.");
                }

            }

            if (cbSystemDirectXCache.Checked)
            {
				try
				{
                	var directory = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\D3DSCache\\");
                	if (DeleteAllFiles(directory)) CleanupLogsLBox.Items.Add("DirectX Shader Cache Deleted.");
				}
				catch
				{
					CleanupLogsLBox.Items.Add("Unable to delete DirectX Shader Cache.");
				}
            }

            if (cbSystemMemDumps.Checked)
            {
                try
                {
                    var directory = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\CrashDumps\\");
                    var directory2 = new DirectoryInfo("C:\\Windows\\MiniDump\\");
                    var file1 = new DirectoryInfo("C:\\Windows\\MEMORY.DMP");
                    if (DeleteAllFiles(directory) & DeleteAllFiles(directory2) & DeleteAllFiles(file1)) CleanupLogsLBox.Items.Add("System Memory Dumps Deleted.");
                }
                catch
                {
                    CleanupLogsLBox.Items.Add("Error while deleting System Memory Dumps.");
                }
            }


            if (cbSystemErrorReporting.Checked)
            {
                var directory = new DirectoryInfo("C:\\ProgramData\\Microsoft\\Windows\\WER\\ReportArchive\\");
                if (DeleteAllFiles(directory)) CleanupLogsLBox.Items.Add("Deleted " + directory);
            }

            if (cbExplorerThumbCache.Checked)
            {

                var tc = (Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Microsoft\\Windows\\Explorer\\");
                foreach (string file in Directory.EnumerateFiles(tc))

                    if (file.Contains("thumbcache") == true)
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

            if (cbExplorerIconCache.Checked)
            {
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

            if (cbChromeSavedPasswords.Checked)
            {
                try
                {
                    var directory = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\Login Data\\");
                    if (DeleteAllFiles(directory)) CleanupLogsLBox.Items.Add("Chrome Saved Passwords Deleted.");
                }
                catch
                {
                    CleanupLogsLBox.Items.Add("Error while deleting Chrome Saved Passwords.");
                }

            }

            if (cbIECache.Checked)
            {
                try
                {
                    var directory = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Microsoft\\Windows\\INetCache\\IE\\");
                    var directory2 = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Microsoft\\Windows\\WebCache.old\\");
                    if (DeleteAllFiles(directory) & DeleteAllFiles(directory2)) CleanupLogsLBox.Items.Add("Internet Explorer Cache Deleted.");
                }
                catch
                {
                    CleanupLogsLBox.Items.Add("Error while deleting Internet Explorer Cache.");
                }

            }

            if (cbWindowsLogFiles.Checked)
            {
                try
                {
                    var directory = new DirectoryInfo("C:\\WINDOWS\\Logs\\MeasuredBoot\\");
                    var directory2 = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Microsoft\\CLR_v4.0\\UsageLogs\\");
                    if (DeleteAllFiles(directory) & DeleteAllFiles(directory2)) CleanupLogsLBox.Items.Add("Windows Log Files Deleted.");
                }
                catch
                {
                    CleanupLogsLBox.Items.Add("Error while deleting Windows Log Files.");
                }

            }


            if (cbOneDriveCache.Checked)
            {
                try
                {
                    var directory = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Microsoft\\OneDrive\\setup\\logs");
                    if (DeleteAllFiles(directory)) CleanupLogsLBox.Items.Add("OneDrive Cache Deleted.");
                }
                catch
                {
                    CleanupLogsLBox.Items.Add("Error while deleting OneDrive cache.");
                }

            }

            if (cbVLCCache.Checked)
            {
                try
                {
                    var directory = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Roaming\\vlc\\art\\");
                    var directory2 = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Roaming\\vlc\\crashdump\\");
                    if (DeleteAllFiles(directory) & DeleteAllFiles(directory2)) CleanupLogsLBox.Items.Add("VLC Cache Deleted.");
                }
                catch
                {
                    CleanupLogsLBox.Items.Add("Error while deleting VLC cache.");
                }

            }

            if (cbSpotifyCache.Checked)
            {
                try
                {
                    var directory = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Packages\\SpotifyAB.SpotifyMusic_zpdnekdrzrea0\\LocalCache\\Spotify\\Data\\");
                    if (DeleteAllFiles(directory)) CleanupLogsLBox.Items.Add("Spotify Cache Deleted.");
                }
                catch
                {
                    CleanupLogsLBox.Items.Add("Error while deleting Spotify Cache.");
                }
            }

            // END OF CLEANUP.
            Core.Utils.CleanupUtils.SaveCleanupLog();
            label8.Text = "" + Core.Utils.CleanupUtils.filesDeleted;

        }

        private void Tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            isCleanup = false;
            // i hate C#, i gotta do this dumb s**t just to make it hide on both forms.
            // java makes this easier
            if (tabControl1.SelectedTab.Text == "Browser Extensions" )
            {
                isCleanup = false;
            }

            if (tabControl1.SelectedTab.Text == "Quick Clean")
            {
                isCleanup = false;
            }

            if (tabControl1.SelectedTab.Text == "Cleanup")
            {
                isCleanup = true;
            }



            if (isCleanup == false)
            {
                btnCleanup.Visible = false;
            }
            else
            {
            if (!btnCleanup.Visible)
                {
                    btnCleanup.Visible = true;
                }
            }

         
        }


        private void frmCleanup_Load(object sender, EventArgs e)
        {
            tabControl1.SelectedIndexChanged += new EventHandler(Tabs_SelectedIndexChanged); // what is this?
            DirectoryExists();
            CheckTheme();
        }

        private void GetExtensionList(DirectoryInfo directoryInfo)
        {
            foreach (var ext in directoryInfo.GetDirectories())
            {
                try
                {
                    FileInfo fi = new FileInfo(ext.ToString());
                    ListViewItem extb = ExtensionsBox.Items.Add(fi.Name, 0);

                    long dirSize = DirSize(new DirectoryInfo(ext.ToString()));
                    double dirsizeMB = ConvertBytesToMegabytes(dirSize);
                    extb.SubItems.Add(dirsizeMB + "MB");
                    extb.SubItems.Add(ext.ToString());
                }
                catch
                {
                    MessageBox.Show("An error has occurred.");
                }

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var g = new Dirs();
            ExtensionsBox.Items.Clear();
            if (comboBox1.Text == "Google Chrome")
            {
                GetExtensionList(new DirectoryInfo(Dirs.chromeExtDir));
            }

            else if (comboBox1.Text == "Mozilla Firefox")
            {
                ExtensionsBox.Items.Clear();
                try
                {
                    foreach (string fol in Directory.EnumerateDirectories(Dirs.firefoxExtDir))
                    {
                        if (fol.Contains("-release"))
                        {
                            string prf = (fol + "\\extensions\\");
                            try
                            {
                                GetExtensionList(new DirectoryInfo(Dirs.firefoxExtDir));

                            }
                            catch (Exception Exc)
                            {
                                MessageBox.Show(Exc.ToString());
                            }
                        }
                    }

                }
                catch (Exception Exc)
                {
                    MessageBox.Show(Exc.ToString());
                }

            }

            else if (comboBox1.Text == "Microsoft Edge")
            {
                ExtensionsBox.Items.Clear();
                try
                {
                    GetExtensionList(new DirectoryInfo(Dirs.edgeExtDir));
                }
                catch
                {
                    MessageBox.Show("Unable to get extensions from Edge.");
                }
                
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (ExtensionsBox.SelectedItems.Count >= 0)
            {
                if (comboBox1.Text == "Google Chrome")
                {
                    Process.Start("taskkill", "/f /im chrome.exe");
                    foreach (ListViewItem eachItem in ExtensionsBox.SelectedItems)
                    {
                        try
                        {
                            var item = ExtensionsBox.SelectedItems[0];
                            var subItem = item.SubItems[2].Text;
                            Thread.Sleep(75);
                            Directory.Delete(subItem, true);
                            ExtensionsBox.Items.Remove(eachItem);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error while trying to remove extension." + ex);
                        }
                    }
                }

                if (comboBox1.Text == "Mozilla Firefox")
                {
                    Process.Start("taskkill", "/f /im firefox.exe");
                    try
                    {
                        Thread.Sleep(75);
                        File.Delete(ExtensionsBox.SelectedItems[0].SubItems[2].Text);

                        foreach (ListViewItem eachItem in ExtensionsBox.SelectedItems)
                        {
                            ExtensionsBox.Items.Remove(eachItem);
                            CleanupLogsLBox.Items.Add("Extension Removed.");
                        }
                    }
                    catch
                    {
                        CleanupLogsLBox.Items.Add("Failed to remove extension.");
                    }

                }


                if (comboBox1.Text == "Microsoft Edge")
                {
                    Process.Start("taskkill", "/f /im msedge.exe");
                    foreach (ListViewItem eachItem in ExtensionsBox.SelectedItems)
                    {
                        try
                        {
                            var item = ExtensionsBox.SelectedItems[0];
                            var subItem = item.SubItems[2].Text;
                            Thread.Sleep(75);
                            Directory.Delete(subItem, true);
                            ExtensionsBox.Items.Remove(eachItem);
                            CleanupLogsLBox.Items.Add("Extension Removed.");

                        }
                        catch (Exception ex)
                        {
                            CleanupLogsLBox.Items.Add("Error while trying to remove extension." + ex);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an extension to remove.");
            }

        }

        private void DirectoryExists()
        {
            var localappdata = Environment.GetEnvironmentVariable("LocalAppData");
            var roamingappdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            Dirs.chromeDir = localappdata + "\\Google\\Chrome\\";
            Dirs.chromeExtDir = (Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\Extensions\\");
            Dirs.firefoxDir = localappdata + "\\Mozilla\\Firefox\\";
            Dirs.firefoxExtDir = roamingappdata + "\\Mozilla\\Firefox\\Profiles\\";
            Dirs.edgeDir = (Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Microsoft\\Edge\\");
            Dirs.edgeExtDir = (Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Microsoft\\Edge\\User Data\\Default\\Extensions\\");
            Dirs.discordDir = localappdata + "\\Discord\\";

            if (!Directory.Exists(Dirs.chromeDir))
            {
                cbChromeCache.Enabled = false;
                cbChromeCookies.Enabled = false;
                cbChromeSearchHistory.Enabled = false;
                cbChromeSessions.Enabled = false;
                cbChromeSavedPasswords.Enabled = false;
                label3.Text = "Google Chrome (Not Found)";
            }

            if (!Directory.Exists(Dirs.firefoxDir))
            {
                cbFirefoxCache.Enabled = false;
                cbFirefoxCookies.Enabled = false;
                cbFirefoxSearchHistory.Enabled = false;
                label10.Text = "Mozilla Firefox (Not Found)";
            }

            if (!Directory.Exists(Dirs.discordDir))
            {
                cbDiscord.Enabled = false;
                cbDiscord.Text = "Discord (Not Found)";
            }

            if (!Directory.Exists(Dirs.edgeDir))
            {
                cbEdgeCache.Enabled = false;
                cbEdgeCookies.Enabled = false;
                cbEdgeSearchHistory.Enabled = false;
                cbEdgeSessions.Enabled = false;
                label18.Text = "Microsoft Edge (Not Found)";
            }

            if (Directory.Exists(Dirs.chromeExtDir))
            {
                comboBox1.Items.Add("Google Chrome");
            }

            if (Directory.Exists(Dirs.firefoxDir))
            {
                comboBox1.Items.Add("Mozilla Firefox");
            }

            if (Directory.Exists(Dirs.edgeDir))
            {
                comboBox1.Items.Add("Microsoft Edge");
            }
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            try
            {
                label11.Visible = true;
                button9.Visible = true;
                long size1 = DirSize(new DirectoryInfo("C:\\Windows\\Temp\\"));
                long size2 = DirSize(new DirectoryInfo(Path.GetTempPath()));
                long size4 = DirSize(new DirectoryInfo(("C:\\ProgramData\\Microsoft\\Windows\\WER\\ReportArchive\\")));
                long size5 = DirSize(new DirectoryInfo("C:\\WINDOWS\\Logs\\MeasuredBoot\\"));

                var temp1 = new DirectoryInfo("C:\\Windows\\Temp\\");
                var temp2 = new DirectoryInfo(Path.GetTempPath());

                foreach (var file in temp1.GetFiles())
                {
                    listBox1.Items.Add(file.Name);
                }
                foreach (var file in temp2.GetFiles())
                {
                    listBox1.Items.Add(file.Name);
                }
                long allsize = size1 + size2 + size4 + size5;
                double allsizeMB = ConvertBytesToMegabytes(allsize);
                label11.Text = allsizeMB + "MB can be cleaned.";
            }
            catch
            {
                label11.Text = "There was an error. Please try again.";
            }
        }

        public static long DirSize(DirectoryInfo d)
        {

            long size = 0;
            // Add file sizes.
            try
            {
                FileInfo[] fis = d.GetFiles();
                foreach (FileInfo fi in fis)
                {
                    size += fi.Length;
                }
                // Add subdirectory sizes.
                DirectoryInfo[] dis = d.GetDirectories();
                foreach (DirectoryInfo di in dis)
                {
                    size += DirSize(di);
                }
                return size;
            }
            catch
            {
                return size;
            }
        }

        static double ConvertBytesToMegabytes(long bytes)
        {
            double ConvertedByte = Math.Round(bytes / 1024f / 1024f, 2);
            return (ConvertedByte);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var windowstemp = new DirectoryInfo("C:\\Windows\\Temp\\");
            var usertemp = new DirectoryInfo(Path.GetTempPath());
            var windowsReport = new DirectoryInfo(("C:\\ProgramData\\Microsoft\\Windows\\WER\\ReportArchive\\"));
            var windowsLog = new DirectoryInfo("C:\\WINDOWS\\Logs\\MeasuredBoot\\");

            try
            {
                if (DeleteAllFiles(windowstemp)) Debug.Print("Null.");
                if (DeleteAllFiles(usertemp)) Debug.Print("Null.");
                if (DeleteAllFiles(windowsReport)) Debug.Print("Null.");
                if (DeleteAllFiles(windowsLog)) Debug.Print("Null.");
            }
            catch
            {
                // Needs advanced catch method.
                Debug.Print("Quick Clean was unable to clean everything.");
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Originally written in .bat format by Solirs.
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Normal;
                startInfo.FileName = "ipconfig";
                startInfo.Arguments = "/displaydns";
                startInfo.RedirectStandardError = true;
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
            }
            catch (Exception)
            {
                MessageBox.Show("Error while trying to show DNS Cache.");
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Originally written in .bat format by Solirs.
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Normal;
                startInfo.FileName = "arp";
                startInfo.Arguments = "-a";
                startInfo.RedirectStandardError = true;
                process.StartInfo = startInfo;
                process.Start();

            }
            catch (Exception)
            {
                MessageBox.Show("Error while trying to show ARP Cache.");
            }
        }

        private void tabPage7_Click(object sender, EventArgs e)
        {

        }

        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            // Analyze
        }
    }
}
