using SharpDevs.Fleksator.Grammar;

namespace SharpDevs.Fleksator.IO
{
    public interface IGrammaticalPartWriter<T> where T : GrammaticalWord
    {
        string Write(T word);
    }
}
