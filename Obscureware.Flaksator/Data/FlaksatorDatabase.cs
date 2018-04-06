namespace Obscureware.Flaksator.Data
{
    using System;

    using ObscureWare.DocumentDatabase;

    internal class FlaksatorDatabase : BaseDocumentDatabase, IDocumentDatabase
    {
        private readonly Lazy<IStringListsRepository> _listResources;
        private readonly Lazy<IDictionaryRepositories> _dictionaryResources;

        public FlaksatorDatabase(string dbPath) : base(dbPath)
        {
            this._listResources = new Lazy<IStringListsRepository>(
                () => new LightStringListsRepository(this.Db));
            this._dictionaryResources = new Lazy<IDictionaryRepositories>(
                () => new LightDictionaryRepositories(this.Db));
        }

        public IStringListsRepository ListResources
        {
            get { return this._listResources.Value; }
        }

        public IDictionaryRepositories DictionaryResources
        {
            get { return this._dictionaryResources.Value; }
        }
    }
}
