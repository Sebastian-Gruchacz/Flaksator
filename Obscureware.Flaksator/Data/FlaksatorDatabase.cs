using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObscureWare.DocumentDatabase;

namespace Obscureware.Flaksator.Data
{
    class FlaksatorDatabase : BaseDocumentDatabase, IDocumentDatabase
    {
        private readonly Lazy<IStringListsRepository> _listResources;

        public FlaksatorDatabase(string dbPath) : base(dbPath)
        {
            _listResources = new Lazy<IStringListsRepository>(
                () => new LightStringListsRepository(base.Db));

        }

        public IStringListsRepository ListResources
        {
            get { return _listResources.Value; }
        }
    }
}
