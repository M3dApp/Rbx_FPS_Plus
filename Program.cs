namespace RbxFPSPlus
{
    internal class Program
    {
        static int MaxFPS = 200;

        static string? FindFileDirectory(string searchPath, string fileName)
        {
            try
            {
                string[] files = Directory.GetFiles(searchPath, fileName);
                if (files.Length > 0)
                    return searchPath;
                string[] subDirectories = Directory.GetDirectories(searchPath);
                foreach (string subDirectory in subDirectories)
                {
                    string? directory = FindFileDirectory(subDirectory, fileName);
                    if (directory != null)
                        return directory;
                }
            }
            catch (Exception) { }
            return null;
        }

        static string? ScanForProgram(string programName)
        {
            string[] searchPaths = {
                @$"Users\{Environment.UserName}\AppData\Local",
                @"Program Files",
                @"Program Files (x86)",
            };
            foreach (DriveInfo root in DriveInfo.GetDrives())
            {
                foreach (string path in searchPaths)
                {
                    string fullPath = root.RootDirectory.FullName + path + @"\Roblox\Versions";
                    if (!Directory.Exists(fullPath))
                        continue;
                    string? programPath = FindFileDirectory(fullPath, programName);
                    if (programPath != null)
                        return programPath;
                }
            }
            return null;
        }

        static void Main()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Searching for program location...\n");
            string? gamePath = ScanForProgram("RobloxPlayerLauncher.exe");
            if (gamePath == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Program not found.\n");
                Console.ForegroundColor = ConsoleColor.White;
                Thread.Sleep(8000);
                return;
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Program found.\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Enter in Max FPS: ");
            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                MaxFPS = Math.Max(int.Parse(Console.ReadLine()), 30);
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nInvalid integer.");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            string TargetDir = gamePath + @"\ClientSettings";
            string[] Data = { "{\"DFIntTaskSchedulerTargetFps\": " + MaxFPS + "}" };
            try
            {
                Directory.CreateDirectory(TargetDir);
                File.WriteAllLines(TargetDir + @"\ClientAppSettings.json", Data);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Max FPS set to: {MaxFPS}\n");
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to write file.\nTry run program as administrator.\n");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Thread.Sleep(8000);
        }
    }
}