using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace EditTTL
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string user = Environment.UserDomainName + "\\" + Environment.UserName;

                RegistrySecurity rs = new RegistrySecurity();

                rs.AddAccessRule(new RegistryAccessRule(user,
                    RegistryRights.WriteKey | RegistryRights.ChangePermissions,
                    InheritanceFlags.None, PropagationFlags.None, AccessControlType.Deny));

                RegistryKey tcpip = Registry.LocalMachine.CreateSubKey("SYSTEM\\CurrentControlSet\\Services\\Tcpip\\Parameters", RegistryKeyPermissionCheck.Default, rs);
                RegistryKey tcpip6 = Registry.LocalMachine.CreateSubKey("SYSTEM\\CurrentControlSet\\Services\\Tcpip6\\Parameters", RegistryKeyPermissionCheck.Default, rs);

                string TTL = "65";
                Console.WriteLine("Setting new TTL value - " + TTL + "...");
                tcpip.SetValue("DefaultTTL", TTL, RegistryValueKind.DWord);
                tcpip.Close();
                tcpip6.SetValue("DefaultTTL", TTL, RegistryValueKind.DWord);
                tcpip6.Close();

                Console.WriteLine("All done. Please restart your computer.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }
    }
}
