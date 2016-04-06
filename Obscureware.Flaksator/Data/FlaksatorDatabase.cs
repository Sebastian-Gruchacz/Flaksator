using System;
using ObscureWare.DocumentDatabase;

namespace Obscureware.Flaksator.Data
{
    internal class FlaksatorDatabase : BaseDocumentDatabase, IDocumentDatabase
    {
        private readonly Lazy<IStringListsRepository> _listResources;
        private readonly Lazy<IDictionaryRepositories> _dictionaryResources;

        public FlaksatorDatabase(string dbPath) : base(dbPath)
        {
            _listResources = new Lazy<IStringListsRepository>(
                () => new LightStringListsRepository(base.Db));
            _dictionaryResources = new Lazy<IDictionaryRepositories>(
                () => new LightDictionaryRepositories(base.Db));
        }

        public IStringListsRepository ListResources
        {
            get { return _listResources.Value; }
        }

        public IDictionaryRepositories DictionaryResources
        {
            get { return _dictionaryResources.Value; }
        }
    }
}
