using System;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace ObscureWare.DocumentDatabase
{
    public abstract class BaseDocumentDatabase: IDisposable
    {
        private readonly string _dbPath;
        private readonly LiteDatabase _db;
        //private readonly Lazy<IDictionariesRepository> _dictionariesRepository;

        protected BaseDocumentDatabase(string dbPath)
        {
            if (String.IsNullOrWhiteSpace(dbPath))
                throw new ArgumentException("Argument is null or whitespace", nameof(dbPath));

            _dbPath = dbPath;
            _db = new LiteDatabase(_dbPath);

            //_dictionariesRepository = new Lazy<IDictionariesRepository>(() => new LiteDictionariesRepository(_db));
        }

        public void Dispose() // TODO: full implementation
        {
            //if (_dictionariesRepository.IsValueCreated)
            //    _dictionariesRepository.Value.Dispose();

            _db?.Dispose();
        }

        public string DbPath
        {
            get { return _dbPath; }
        }

        protected LiteDatabase Db
        {
            get { return _db; }
        }

        //public Version GetLibraryVersion(string libraryName, Version currentLibraryVersion)
        //{
        //    var versioning = _db.GetCollection<VersionInfo>(@"VERSIONS");
        //    VersionInfo v = versioning.FindOne(version => version.Target == libraryName);
        //    if (v == null)
        //    {
        //        v = new VersionInfo(libraryName, currentLibraryVersion.ToString());
        //        versioning.Insert(v);
        //    }
        //    return Version.Parse(v.Version);
        //}

        //public IEnumerable<GameLanguage> GetGameLanguages(string libraryName)
        //{
        //    return _db.GetCollection<GameLanguage>(typeof(GameLanguage).Name + "_" + libraryName).FindAll();
        //}

        //public void UpdateGameLanguages(string libraryName, IEnumerable<GameLanguage> languages)
        //{
        //    var collection = _db.GetCollection<GameLanguage>(typeof(GameLanguage).Name + "_" + libraryName);
        //    foreach (var lang in collection.FindAll())
        //    {
        //        collection.Delete((gl) => gl.Id == lang.Id);
        //    }

        //    collection.Insert(languages);
        //}

        //public IDictionariesRepository Dictionaries
        //{
        //    get { return _dictionariesRepository.Value; }
        //}
    }
}
