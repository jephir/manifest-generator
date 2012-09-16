using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Test.CommandLineParsing;

namespace ManifestGenerator
{
    class Manifest : Command
    {
        static SHA256 sha256 = new SHA256Managed();

        int version;

        public string ContentDirectory { get; set; }
        public string ContentServerUri { get; set; }
        public string OutputFile { get; set; }
        public string InstallerPath { get; set; }
        public string ApplicationPath { get; set; }
        public string ApplicationArguments { get; set; }
        public bool? IncludeManifest { get; set; }

        public override void Execute()
        {
            SetDefaults();
            LoadVersion();
            CreateManifest();
            SaveVersion();
        }

        void SetDefaults()
        {
            ContentDirectory = ContentDirectory ?? Directory.GetCurrentDirectory();
            ContentServerUri = ContentServerUri ?? "http://localhost/";
            OutputFile = OutputFile ?? Path.Combine(ContentDirectory, "manifest.xml");
        }

        void LoadVersion()
        {
            if (File.Exists("version.txt"))
            {
                version = Convert.ToInt32(File.ReadAllText("version.txt"));
            }
            else
            {
                version = 0;
            }
        }

        void SaveVersion()
        {
            File.WriteAllText("version.txt", version.ToString());
        }

        void CreateManifest()
        {
            var contentDirectory = new DirectoryInfo(ContentDirectory);
            var files = contentDirectory.GetFiles("*", SearchOption.AllDirectories);
            var document = new XDocument(new XDeclaration("1.0", Encoding.UTF8.HeaderName, String.Empty));
            var package = new XElement("package");

            package.Add(new XAttribute("version", ++version));

            if (InstallerPath != null)
            {
                var installer = new XElement("installer");
                installer.Add(new XAttribute("path", InstallerPath));
                package.Add(installer);
            }

            if (ApplicationPath != null)
            {
                var application = new XElement("application");
                application.Add(new XAttribute("path", ApplicationPath));
                if (ApplicationArguments != null) application.Add(new XAttribute("arguments", ApplicationArguments));
                package.Add(application);
            }

            foreach (var file in files)
            {
                if (file.Name.Equals(Path.GetFileName(OutputFile)) && IncludeManifest != true) continue;

                var item = new XElement("item");

                var filePath = Program.MakeRelativePath(contentDirectory.FullName + Path.DirectorySeparatorChar, file.FullName);
                var fileUri = ContentServerUri + filePath;
                var fileSize = file.Length;

                var fileBytes = File.ReadAllBytes(file.FullName);
                var fileHash = sha256.ComputeHash(fileBytes);
                var fileHashString = BitConverter.ToString(fileHash).Replace("-", String.Empty).ToLower();

                item.Add(new XAttribute("path", filePath));
                item.Add(new XAttribute("uri", fileUri));
                item.Add(new XAttribute("size", fileSize));
                item.Add(new XAttribute("sha256", fileHashString));

                package.Add(item);
            }

            document.Add(package);

            using (var writer = new XmlTextWriter(OutputFile, new UTF8Encoding(false)))
            {
                writer.Formatting = Formatting.Indented;
                document.Save(writer);
            }
        }
    }
}
