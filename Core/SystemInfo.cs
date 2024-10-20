using System;
using System.IO;
using System.Management;
using System.Net.NetworkInformation;

namespace FitTrack.Core
{
    /// <summary>
    /// Provides system information including device name, OS version, and IP address.
    /// </summary>
    public class SystemInfo
    {
        /// <summary>
        /// Gets the device name and current user's name.
        /// </summary>
        /// <value>
        /// A string representing the device name.
        /// </value>
        public static string DeviceName => Environment.MachineName;

        /// <summary>
        /// Gets operating system information
        /// </summary>
        /// <value>
        /// A string representing the operating system caption, version, and build number.
        /// Example format: "Windows 10 Pro (Version 10.0, Build 19042)".
        /// </value>
        public static string OSInformation => GetOperatingSystemVersion();

        /// <summary>
        /// Gets the IP address of the system.
        /// </summary>
        /// <value>
        /// A string representing the IP address of the system. Returns an empty string if no IP address is found.
        /// </value>
        public static string IPAddress => GetIPAddress();

        /// <summary>
        /// Gets the directory path of the project.
        /// </summary>
        /// <value>
        /// The directory path up to the project directory (e.g., FitTrack).
        /// </value>
        /// <returns>
        /// A string representing the path to the project directory.
        /// </returns>
        /// <example>
        /// The following example demonstrates how to use the ProjectDirectory property:
        /// <code>
        /// string projectPath = ProjectDirectory;
        /// Console.WriteLine(projectPath);
        /// </code>
        /// </example>
        public static string ProjectDirectory
        {
            get
            {
                // Get the base directory of the current domain
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                // Get the directory info of the base directory
                DirectoryInfo directoryInfo = new DirectoryInfo(baseDirectory);

                // Navigate up to the project directory (two levels up from bin\Debug\)
                string projectDirectory = directoryInfo.Parent.Parent.FullName;

                return projectDirectory;
            }
        }

        /// <summary>
        /// Retrieves the operating system version including the name, version, and build number.
        /// </summary>
        /// <returns>
        /// A string representing the operating system caption, version, and build number.
        /// </returns>
        private static string GetOperatingSystemVersion()
        {
            string version = "";
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Caption, Version, BuildNumber FROM Win32_OperatingSystem");
                foreach (ManagementObject os in searcher.Get())
                {
                    version = $"{os["Caption"]} (Version {os["Version"]}, Build {os["BuildNumber"]})";
                }
            }
            catch (Exception)
            {
                version = "Unknown";
            }

            return version;
        }

        /// <summary>
        /// Retrieves IP address of the system.
        /// </summary>
        /// <returns>
        /// A string representing the IP address of the system.
        /// </returns>
        private static string GetIPAddress()
        {
            string ipAddress = "";
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            ipAddress = ip.Address.ToString();
                            break;
                        }
                    }
                }
            }
            return string.IsNullOrEmpty(ipAddress) ? "Unknown" : ipAddress;
        }
    }
}
