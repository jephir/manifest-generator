using System;
using Microsoft.Test.CommandLineParsing;

namespace ManifestGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var manifest = new Manifest();

            manifest.ParseArguments(args);
            manifest.Execute();
        }

        // Modified from: http://stackoverflow.com/a/340454/238687
        public static String MakeRelativePath(String fromPath, String toPath)
        {
            if (String.IsNullOrEmpty(fromPath)) throw new ArgumentNullException("fromPath");
            if (String.IsNullOrEmpty(toPath)) throw new ArgumentNullException("toPath");

            var fromUri = new Uri(fromPath);
            var toUri = new Uri(toPath);

            var relativeUri = fromUri.MakeRelativeUri(toUri);
            var relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            return relativePath;
        }
    }
}
