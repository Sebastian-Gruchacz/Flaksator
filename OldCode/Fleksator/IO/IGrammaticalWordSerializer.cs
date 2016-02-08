using SharpDevs.Fleksator.Grammar;

namespace SharpDevs.Fleksator.IO
{
    public interface IGrammaticalWordSerializer<T> : IGrammaticalPartLoader<T>, IGrammaticalPartWriter<T> where T : GrammaticalWord
    {
    }
}
