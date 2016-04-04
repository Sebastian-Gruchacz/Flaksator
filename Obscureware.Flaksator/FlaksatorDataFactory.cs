using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Obscureware.Flaksator.Data;

namespace Obscureware.Flaksator
{
    public class FlaksatorDataFactory
    {
        private readonly string _dbPath;

        public FlaksatorDataFactory(string dbLocationString)
        {
            string[] parts = dbLocationString.Split('|');
            string folder = LocateParentFolder(
                Assembly.GetCallingAssembly().Location,
                parts[0]);
            _dbPath = Path.Combine(folder, parts[1]);
        }

        private string LocateParentFolder(string currentFolder, string searchedName)
        {
            FileInfo file = new FileInfo(currentFolder);
            if (file.Exists) // skip file
            {
                return LocateParentFolder(file.Directory.FullName, searchedName);
            }

            var root = Path.GetPathRoot(currentFolder);

            DirectoryInfo dir = new DirectoryInfo(currentFolder);
            var dirs = dir.GetDirectories();
            if (dirs.Select(d => d.Name).Contains(searchedName))
            {
                return currentFolder;
            }
            var files = dir.GetFiles();
            if (files.Select(f => f.Name).Contains(searchedName))
            {
                return currentFolder;
            }
            if (currentFolder == root)
            {
                throw new InvalidOperationException("Could not locate valid data folder.");
            }

            return LocateParentFolder(dir.Parent.FullName, searchedName);
        }

        public IDocumentDatabase CreateDatabase()
        {
            return new FlaksatorDatabase(_dbPath);
        }
    }
}
