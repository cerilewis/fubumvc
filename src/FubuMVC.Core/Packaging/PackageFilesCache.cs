using System;
using System.Collections.Generic;
using System.IO;
using FubuCore;
using System.Linq;
using FubuMVC.Core.Registration;

namespace FubuMVC.Core.Packaging
{
    [Singleton]
    public class PackageFilesCache : IPackageFiles
    {
        private readonly IList<string> _directories = new List<string>();


        public void AddDirectory(string directory)
        {
            _directories.Add(directory);
        }

        public void ForFiles(FileSet files, Action<string> readAction)
        {
            FindFiles(files).Each(readAction);
        }

        public IEnumerable<string> FindFiles(FileSet files)
        {
            var system = new FileSystem();
            return _directories.SelectMany(dir => system.FindFiles(dir, files));
        }

        public IEnumerable<string> Directories
        {
            get { return _directories; }
        }
    }
}