using SharpDevs.Fleksator.IO.Implementation;

namespace SharpDevs.Fleksator.IO
{
    public class GrammarSerializersFactory
    {
        public IGrammarSerializers GetOldSerializers()
        {
            return new OldGrammarSerializers();
        }
    }
}
