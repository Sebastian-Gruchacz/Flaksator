namespace Obscureware.Flaksator
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Data;

    public class FlaksatorDataFactory
    {
        private readonly string _dbPath;

        public FlaksatorDataFactory(string dbLocationString)
        {
            string[] parts = dbLocationString.Split('|');
            string folder = this.LocateParentFolder(
                Assembly.GetCallingAssembly().Location,
                parts[0]);
            this._dbPath = Path.Combine(folder, parts[1]);
        }

        private string LocateParentFolder(string currentFolder, string searchedName)
        {
            FileInfo file = new FileInfo(currentFolder);
            if (file.Exists) // skip file
            {
                return this.LocateParentFolder(file.Directory.FullName, searchedName);
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

            return this.LocateParentFolder(dir.Parent.FullName, searchedName);
        }

        public IDocumentDatabase CreateDatabase()
        {
            return new FlaksatorDatabase(this._dbPath);
        }
    }
}
