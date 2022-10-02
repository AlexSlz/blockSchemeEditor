using Microsoft.Win32;
using System;
using System.Security.Principal;
using System.Windows.Forms;

namespace blockSchemeEditor
{
    internal static class SystemActions
    {
        public static bool InitRegEdit()
        {
            if (CheckAdmin())
            {
                if (CheckRegEdit())
                {
                    CreateRegEdit();
                }
                return !CheckRegEdit();
            }
            return false;
        }
        private static bool CheckAdmin()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }
        private static bool CheckRegEdit()
        {
            return (Registry.GetValue("HKEY_CLASSES_ROOT\\.block", "", null) == null);
        }
        private static void CreateRegEdit()
        {
            string appName = "blockShemeEditor";
            string exePath = $"\"{Application.ExecutablePath}\"";
            string exeName = AppDomain.CurrentDomain.FriendlyName;
            try
            {
                Registry.ClassesRoot.CreateSubKey(".block").SetValue("", appName);
                Registry.ClassesRoot.CreateSubKey(appName + @"\DefaultIcon").SetValue("", exePath);
                Registry.ClassesRoot.CreateSubKey(appName + @"\shell\open\command").SetValue("", exePath + "\"%1\"");
                //Registry.ClassesRoot.CreateSubKey(appName + @"\shell\edit\command").SetValue("", exePath + "\"%1\"");

                Registry.ClassesRoot.CreateSubKey($@"Applications\{exeName}\shell\open\command").SetValue("", exePath + "\"%1\"");
                //Registry.ClassesRoot.CreateSubKey($@"Applications\{exeName}\Ashell\edit\command").SetValue("", exePath + "\"%1\"");
                MessageBox.Show("Regedit installed!");
            }
            catch
            {
            }
        }
        public static void DeleteRegEdit()
        {
            try
            {
                Registry.ClassesRoot.DeleteSubKeyTree("blockShemeEditor");
                Registry.ClassesRoot.DeleteSubKeyTree(".block");
                Registry.ClassesRoot.DeleteSubKeyTree($"Applications\\{AppDomain.CurrentDomain.FriendlyName}");
                MessageBox.Show("OK.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
