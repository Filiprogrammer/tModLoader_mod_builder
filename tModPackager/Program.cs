using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria.ModLoader.Core;

namespace tModPackager
{
    class Program
    {
        static IList<string> sourceExtensions = new List<string> { ".csproj", ".cs", ".sln" };

        private static bool IgnoreCompletely(string modDir, BuildProperties buildProperties, string resource)
        {
            var relPath = resource.Substring(modDir.Length + 1);
            return buildProperties.ignoreFile(relPath) ||
                relPath[0] == '.' ||
                relPath.StartsWith("bin" + Path.DirectorySeparatorChar) ||
                relPath.StartsWith("obj" + Path.DirectorySeparatorChar);
        }

        private static bool IgnoreResource(string modDir, BuildProperties buildProperties, string resource)
        {
            var relPath = resource.Substring(modDir.Length + 1);
            return IgnoreCompletely(modDir, buildProperties, resource) ||
                relPath == "build.txt" ||
                !buildProperties.includeSource && sourceExtensions.Contains(Path.GetExtension(resource)) ||
                Path.GetFileName(resource) == "Thumbs.db";
        }

        private static string DllRefPath(string modDir, string dllName)
        {
            string path = Path.Combine(modDir, "lib", dllName) + ".dll";

            if (File.Exists(path))
                return path;

            throw new Exception("Missing dll reference: " + path);
        }

        static void Main(string[] args)
        {
            if (args.Length < 2)
                Console.WriteLine("Usage: tModPackager [path to mod directory] [path to resulting .tmod file]");

            string modDir = null;

            if (args.Length >= 1)
            {
                modDir = args[0];
            }
            else
            {
                Console.Write("Path to mod directory: ");
                modDir = Console.ReadLine();
            }

            string tmodPath = null;

            if (args.Length >= 2)
            {
                tmodPath = args[1];
            }
            else
            {
                Console.Write("Path to resulting .tmod file: ");
                tmodPath = Console.ReadLine();
            }

            BuildProperties buildProperties = BuildProperties.ReadBuildFile(modDir);
            string modName = Path.GetFileName(modDir);
            TmodFile tmodfile = new TmodFile(tmodPath, modName, buildProperties.version);
            tmodfile.AddFile(modName + ".dll", File.ReadAllBytes(modDir + Path.DirectorySeparatorChar + "obj" + Path.DirectorySeparatorChar + "Debug" + Path.DirectorySeparatorChar + "net6.0" + Path.DirectorySeparatorChar + modName + ".dll"));
            tmodfile.AddFile("Info", buildProperties.ToBytes());

            List<string> resources = Directory.GetFiles(modDir, "*", SearchOption.AllDirectories).Where(res => !IgnoreResource(modDir, buildProperties, res)).ToList();

            foreach (string resource in resources)
            {
                string relPath = resource.Substring(modDir.Length + 1);
                tmodfile.AddFile(relPath.Replace(Path.DirectorySeparatorChar, '/'), File.ReadAllBytes(resource));
            }

            // add dll references from the -eac bin folder
            var libFolder = Path.Combine(modDir, "lib");
            foreach (var dllPath in buildProperties.dllReferences.Select(dllName => DllRefPath(modDir, dllName)))
                if (!dllPath.StartsWith(libFolder))
                    tmodfile.AddFile("lib/" + Path.GetFileName(dllPath), File.ReadAllBytes(dllPath));

            tmodfile.Save();
        }
    }
}
