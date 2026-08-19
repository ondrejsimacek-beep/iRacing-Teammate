using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using Microsoft.Win32;

namespace SnailsMotorsport.IRacingTeammate
{
    [Serializable]
    public class LauncherSettings
    {
        public List<AppSetting> Apps { get; set; }
        public int DefaultDelaySeconds { get; set; }
        public string UpdateRepository { get; set; }
        public bool AutoModeEnabled { get; set; }

        public LauncherSettings()
        {
            Apps = new List<AppSetting>();
            DefaultDelaySeconds = 2;
            UpdateRepository = "";
            AutoModeEnabled = true;
        }
    }

    [Serializable]
    public class AppSetting
    {
        public string Key { get; set; }
        public string Path { get; set; }
        public bool Enabled { get; set; }
        public int DelaySeconds { get; set; }
        public bool Hidden { get; set; }

        public AppSetting()
        {
            Key = "";
            Path = "";
            Enabled = true;
            DelaySeconds = 2;
            Hidden = false;
        }
    }

    public class AppDefinition
    {
        public string Key;
        public string Name;
        public string Category;
        public string ProcessName;
        public string Initials;
        public bool EnabledByDefault;
        public string[] CandidatePaths;
    }

    public static class AppCatalog
    {
        public static List<AppDefinition> Create()
        {
            string local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string roaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string pf = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            string pfx86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);

            return new List<AppDefinition>
            {
                Def("iracing", "iRacing", "SIMULATOR", "iRacingUI", "iR", true,
                    Path.Combine(pfx86, "iRacing", "iRacingLauncher64.exe"),
                    Path.Combine(pfx86, "iRacing", "iRacingUI.exe"),
                    Path.Combine(local, "Programs", "iRacing UI", "iRacingUI.exe")),
                Def("crewchief", "Crew Chief V4", "SPOTTER", "CrewChiefV4", "CC", true,
                    Path.Combine(pfx86, "Britton IT Ltd", "CrewChiefV4", "CrewChiefV4.exe"),
                    Path.Combine(pf, "Britton IT Ltd", "CrewChiefV4", "CrewChiefV4.exe")),
                Def("tradingpaints", "Trading Paints", "LIVERIES", "Trading Paints", "TP", true,
                    Path.Combine(pfx86, "Rhinode LLC", "Trading Paints", "Trading Paints.exe"),
                    Path.Combine(pf, "Rhinode LLC", "Trading Paints", "Trading Paints.exe")),
                Def("garage61", "Garage61", "TELEMETRY", "garage61-launcher", "G61", true,
                    Path.Combine(local, "Garage61 Agent", "garage61-launcher.exe"),
                    Path.Combine(local, "Programs", "Garage61", "garage61-launcher.exe"),
                    Path.Combine(roaming, "Garage61", "garage61-launcher.exe"),
                    Path.Combine(roaming, "garage61-install", "garage61-launcher.exe")),
                Def("irdashies", "irDashies", "OPEN OVERLAYS", "irdashies", "iD", false,
                    Path.Combine(local, "irdashies", "irdashies.exe"),
                    Path.Combine(local, "Programs", "irdashies", "irdashies.exe")),
                Def("gofast", "GO Fast", "GO SETUPS", "GO Fast", "GO", false,
                    Path.Combine(local, "Programs", "GO Fast", "GO Fast.exe"),
                    Path.Combine(local, "Programs", "go-fast", "GO Fast.exe"),
                    Path.Combine(local, "GO Fast", "GO Fast.exe"),
                    Path.Combine(local, "gosetups", "GO Fast.exe")),
                Def("simhub", "SimHub", "DASHBOARDS", "SimHubWPF", "SH", false,
                    Path.Combine(pfx86, "SimHub", "SimHubWPF.exe"),
                    Path.Combine(pf, "SimHub", "SimHubWPF.exe")),
                Def("ioverlay", "iOverlay", "OVERLAY", "iOverlay", "iO", false,
                    Path.Combine(local, "Programs", "iOverlay", "iOverlay.exe"),
                    Path.Combine(local, "iOverlay", "iOverlay.exe")),
                Def("racelab", "Racelab", "OVERLAY", "RacelabApps", "RL", false,
                    Path.Combine(local, "Programs", "RacelabApps", "RacelabApps.exe"),
                    Path.Combine(local, "Programs", "racelabapps", "RacelabApps.exe")),
                Def("streamdeck", "Elgato Stream Deck", "CONTROLS", "StreamDeck", "SD", false,
                    Path.Combine(pf, "Elgato", "StreamDeck", "StreamDeck.exe")),
                Def("irsidekick", "iRSidekick", "PAINTS & TOOLS", "iRSidekick", "iRS", false,
                    Path.Combine(local, "Programs", "iRSidekick", "iRSidekick.exe"),
                    Path.Combine(local, "iRSidekick", "iRSidekick.exe"),
                    Path.Combine(pf, "iRSidekick", "iRSidekick.exe")),
                Def("vrslogger", "VRS Telemetry Logger", "TELEMETRY", "VRS_TelemetryLogger", "VRS", false,
                    Path.Combine(pfx86, "Virtual Racing School", "VRS Telemetry Logger", "VRS_TelemetryLogger.exe"),
                    Path.Combine(pf, "Virtual Racing School", "VRS Telemetry Logger", "VRS_TelemetryLogger.exe"),
                    Path.Combine(local, "Programs", "VRS Telemetry Logger", "VRS_TelemetryLogger.exe")),
                Def("kapps", "Kapps", "OVERLAY", "Kapps", "KA", false,
                    Path.Combine(local, "Programs", "Kapps", "Kapps.exe"),
                    Path.Combine(local, "Kapps", "Kapps.exe")),
                Def("jrt", "Joel Real Timing", "TIMING", "JRT", "JRT", false,
                    Path.Combine(pf, "Joel Real Timing", "JRT.exe"),
                    Path.Combine(pfx86, "Joel Real Timing", "JRT.exe"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Joel Real Timing", "JRT.exe")),
                Def("openkneeboard", "OpenKneeboard", "VR TOOL", "OpenKneeboardApp", "OKB", false,
                    Path.Combine(local, "Programs", "OpenKneeboard", "OpenKneeboardApp.exe"),
                    Path.Combine(pf, "OpenKneeboard", "OpenKneeboardApp.exe")),
                Def("maira", "Marvin's AIRA", "RACE ENGINEER", "MarvinsAIRARefactored", "MA", false,
                    Path.Combine(pf, "Marvins Awesome iRacing App - Refactored", "MarvinsAIRARefactored.exe"),
                    Path.Combine(pfx86, "Marvins Awesome iRacing App - Refactored", "MarvinsAIRARefactored.exe"))
            };
        }

        private static AppDefinition Def(string key, string name, string category, string processName,
            string initials, bool enabled, params string[] paths)
        {
            return new AppDefinition
            {
                Key = key,
                Name = name,
                Category = category,
                ProcessName = processName,
                Initials = initials,
                EnabledByDefault = enabled,
                CandidatePaths = paths.Where(delegate(string value) { return !String.IsNullOrWhiteSpace(value); }).ToArray()
            };
        }

        public static string DetectPath(AppDefinition definition)
        {
            if (definition.Key == "iracing")
            {
                string iracing = DetectIRacing();
                if (!String.IsNullOrWhiteSpace(iracing)) return iracing;
            }
            foreach (string candidate in definition.CandidatePaths)
            {
                try
                {
                    string expanded = Environment.ExpandEnvironmentVariables(candidate);
                    if (File.Exists(expanded)) return Path.GetFullPath(expanded);
                }
                catch { }
            }
            return "";
        }

        private static string DetectIRacing()
        {
            string install = FindInstalledLocation("iRacing.com Race Simulation");
            if (!String.IsNullOrWhiteSpace(install))
            {
                string[] registryCandidates =
                {
                    Path.Combine(install, "ui", "iRacingUI.exe"),
                    Path.Combine(install, "iRacingLauncher64.exe"),
                    Path.Combine(install, "iRacingUI.exe")
                };
                foreach (string candidate in registryCandidates)
                    if (File.Exists(candidate)) return candidate;
            }

            try
            {
                foreach (DriveInfo drive in DriveInfo.GetDrives())
                {
                    if (!drive.IsReady || drive.DriveType != DriveType.Fixed) continue;
                    string root = drive.RootDirectory.FullName;
                    string[] candidates =
                    {
                        Path.Combine(root, "Games", "iRacing", "ui", "iRacingUI.exe"),
                        Path.Combine(root, "Games", "iRacing", "iRacingLauncher64.exe"),
                        Path.Combine(root, "iRacing", "ui", "iRacingUI.exe"),
                        Path.Combine(root, "iRacing", "iRacingLauncher64.exe"),
                        Path.Combine(root, "SteamLibrary", "steamapps", "common", "iRacing", "ui", "iRacingUI.exe")
                    };
                    foreach (string candidate in candidates)
                        if (File.Exists(candidate)) return candidate;
                }
            }
            catch { }
            return "";
        }

        private static string FindInstalledLocation(string displayName)
        {
            RegistryKey[] roots = { Registry.LocalMachine, Registry.CurrentUser };
            string[] subkeys =
            {
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
                @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
            };
            foreach (RegistryKey root in roots)
            {
                foreach (string subkey in subkeys)
                {
                    try
                    {
                        using (RegistryKey uninstall = root.OpenSubKey(subkey))
                        {
                            if (uninstall == null) continue;
                            foreach (string name in uninstall.GetSubKeyNames())
                            {
                                using (RegistryKey item = uninstall.OpenSubKey(name))
                                {
                                    if (item == null) continue;
                                    string title = item.GetValue("DisplayName") as string;
                                    if (!String.Equals(title, displayName, StringComparison.OrdinalIgnoreCase)) continue;
                                    string location = item.GetValue("InstallLocation") as string;
                                    if (!String.IsNullOrWhiteSpace(location)) return location.Trim('"');
                                }
                            }
                        }
                    }
                    catch { }
                }
            }
            return "";
        }
    }

    public class SettingsStore
    {
        private readonly string directory;
        private readonly string filePath;

        public string DirectoryPath { get { return directory; } }

        public SettingsStore()
        {
            directory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Snails Motorsport",
                "iRacing Teammate");
            filePath = Path.Combine(directory, "settings.xml");
        }

        public LauncherSettings Load(List<AppDefinition> definitions)
        {
            LauncherSettings settings = null;
            try
            {
                if (File.Exists(filePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(LauncherSettings));
                    using (FileStream stream = File.OpenRead(filePath))
                        settings = (LauncherSettings)serializer.Deserialize(stream);
                }
            }
            catch { settings = null; }

            if (settings == null) settings = new LauncherSettings();
            HashSet<string> supportedKeys = new HashSet<string>(definitions.Select(delegate(AppDefinition item) { return item.Key; }));
            settings.Apps.RemoveAll(delegate(AppSetting item) { return !supportedKeys.Contains(item.Key); });
            foreach (AppDefinition definition in definitions)
            {
                AppSetting item = settings.Apps.FirstOrDefault(delegate(AppSetting app) { return app.Key == definition.Key; });
                if (item == null)
                {
                    item = new AppSetting
                    {
                        Key = definition.Key,
                        Enabled = definition.EnabledByDefault,
                        DelaySeconds = settings.DefaultDelaySeconds,
                        Path = AppCatalog.DetectPath(definition)
                    };
                    settings.Apps.Add(item);
                }
                else if (String.IsNullOrWhiteSpace(item.Path) || !File.Exists(item.Path))
                {
                    string detected = AppCatalog.DetectPath(definition);
                    if (!String.IsNullOrWhiteSpace(detected)) item.Path = detected;
                }
            }
            Save(settings);
            return settings;
        }

        public void Save(LauncherSettings settings)
        {
            try
            {
                Directory.CreateDirectory(directory);
                string temp = filePath + ".tmp";
                XmlSerializer serializer = new XmlSerializer(typeof(LauncherSettings));
                using (FileStream stream = File.Create(temp)) serializer.Serialize(stream, settings);
                if (File.Exists(filePath)) File.Delete(filePath);
                File.Move(temp, filePath);
            }
            catch { }
        }
    }

    public class ProcessController
    {
        private readonly Dictionary<string, Process> tracked = new Dictionary<string, Process>();
        private readonly object sync = new object();

        public static bool IsIRacingSessionRunning()
        {
            string[] sessionProcesses =
            {
                "iRacingSim64DX11",
                "iRacingSim64DX12",
                "iRacingSim64"
            };
            foreach (string processName in sessionProcesses)
            {
                try
                {
                    Process[] matches = Process.GetProcessesByName(processName);
                    bool running = matches.Length > 0;
                    foreach (Process process in matches) process.Dispose();
                    if (running) return true;
                }
                catch { }
            }
            return false;
        }

        public bool IsRunning(AppDefinition definition)
        {
            lock (sync)
            {
                Process process;
                if (tracked.TryGetValue(definition.Key, out process))
                {
                    try { if (!process.HasExited) return true; }
                    catch { }
                    tracked.Remove(definition.Key);
                }
            }
            try { return Process.GetProcessesByName(definition.ProcessName).Length > 0; }
            catch { return false; }
        }

        public bool Launch(AppDefinition definition, string path, out string error)
        {
            error = "";
            if (IsRunning(definition)) return true;
            if (String.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                error = "Executable was not found.";
                return false;
            }
            try
            {
                ProcessStartInfo info = new ProcessStartInfo(path);
                info.WorkingDirectory = Path.GetDirectoryName(path);
                info.UseShellExecute = true;
                Process process = Process.Start(info);
                if (process == null)
                {
                    error = "Windows did not return a process handle.";
                    return false;
                }
                lock (sync) tracked[definition.Key] = process;
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public bool StopTracked(AppDefinition definition)
        {
            Process process = null;
            lock (sync)
            {
                if (tracked.ContainsKey(definition.Key)) process = tracked[definition.Key];
            }
            if (process == null) return false;
            try
            {
                if (!process.HasExited)
                {
                    ProcessStartInfo taskkill = new ProcessStartInfo("taskkill.exe",
                        "/PID " + process.Id + " /T /F");
                    taskkill.CreateNoWindow = true;
                    taskkill.UseShellExecute = false;
                    Process killer = Process.Start(taskkill);
                    if (killer != null) killer.WaitForExit(5000);
                }
            }
            catch
            {
                try { if (!process.HasExited) process.Kill(); } catch { }
            }
            lock (sync) tracked.Remove(definition.Key);
            return true;
        }
    }

    public static class StartupManager
    {
        private const string RunKey = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string ValueName = "iRacing Teammate";

        private static string ShortcutPath
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup),
                    "iRacing Teammate.lnk");
            }
        }

        public static bool IsEnabled()
        {
            try
            {
                return File.Exists(ShortcutPath);
            }
            catch { return false; }
        }

        public static bool SetEnabled(bool enabled, string executablePath)
        {
            try
            {
                RemoveLegacyRunValue();
                if (!enabled)
                {
                    if (File.Exists(ShortcutPath)) File.Delete(ShortcutPath);
                    return true;
                }

                Directory.CreateDirectory(Path.GetDirectoryName(ShortcutPath));
                Type shellType = Type.GetTypeFromProgID("WScript.Shell");
                if (shellType == null) return false;
                object shell = Activator.CreateInstance(shellType);
                object shortcut = shellType.InvokeMember("CreateShortcut", BindingFlags.InvokeMethod,
                    null, shell, new object[] { ShortcutPath });
                Type shortcutType = shortcut.GetType();
                shortcutType.InvokeMember("TargetPath", BindingFlags.SetProperty, null, shortcut,
                    new object[] { executablePath });
                shortcutType.InvokeMember("WorkingDirectory", BindingFlags.SetProperty, null, shortcut,
                    new object[] { Path.GetDirectoryName(executablePath) });
                shortcutType.InvokeMember("Description", BindingFlags.SetProperty, null, shortcut,
                    new object[] { "Start iRacing Teammate with Windows" });
                shortcutType.InvokeMember("Save", BindingFlags.InvokeMethod, null, shortcut, null);
                return true;
            }
            catch { return false; }
        }

        private static void RemoveLegacyRunValue()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RunKey, true))
                    if (key != null) key.DeleteValue(ValueName, false);
            }
            catch { }
        }
    }

    public class UpdateCheckResult
    {
        public bool Configured;
        public bool UpdateAvailable;
        public string LatestVersion;
        public string ReleaseUrl;
        public string Error;
    }

    public static class UpdateChecker
    {
        public static string ResolveRepository(string configuredRepository)
        {
            if (!String.IsNullOrWhiteSpace(configuredRepository)) return configuredRepository.Trim();
            try
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("UpdateRepository"))
                using (StreamReader reader = stream == null ? null : new StreamReader(stream))
                    return reader == null ? "" : reader.ReadToEnd().Trim();
            }
            catch { return ""; }
        }

        public static UpdateCheckResult Check(string repository)
        {
            UpdateCheckResult result = new UpdateCheckResult();
            if (String.IsNullOrWhiteSpace(repository) || !repository.Contains("/"))
            {
                result.Configured = false;
                return result;
            }

            result.Configured = true;
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                string endpoint = "https://api.github.com/repos/" + repository.Trim().Trim('/') + "/releases/latest";
                string json;
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("User-Agent", "Snails-Motorsport-iRacing-Teammate");
                    client.Headers.Add("Accept", "application/vnd.github+json");
                    json = client.DownloadString(endpoint);
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> release = serializer.Deserialize<Dictionary<string, object>>(json);
                result.LatestVersion = release.ContainsKey("tag_name") ? Convert.ToString(release["tag_name"]) : "";
                result.ReleaseUrl = release.ContainsKey("html_url") ? Convert.ToString(release["html_url"]) :
                    "https://github.com/" + repository + "/releases/latest";

                string normalized = (result.LatestVersion ?? "").Trim();
                if (normalized.StartsWith("v", StringComparison.OrdinalIgnoreCase)) normalized = normalized.Substring(1);
                Version latest;
                Version current = Assembly.GetExecutingAssembly().GetName().Version;
                result.UpdateAvailable = Version.TryParse(normalized, out latest) && latest > current;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
    }
}
