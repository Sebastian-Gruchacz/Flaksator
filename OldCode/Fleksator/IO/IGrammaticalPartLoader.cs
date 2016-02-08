using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using SharpDevs.Fleksator.Grammar;

namespace SharpDevs.Fleksator.IO
{
    public interface IGrammaticalPartLoader<T> where T : GrammaticalWord
    {
        T Load(string input);
    }
}
