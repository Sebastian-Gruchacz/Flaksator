using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ObscureWare.Randomization
{
    public interface IRandomizer
    {
        int NextInt(int minIncludedValue, int maxExcludedValue);
    }


    public class RandomizerState<T> where T : IRandomizer
    {
        private readonly byte[] _state;

        private RandomizerState(byte[] stateArray)
        {
            _state = stateArray;
        }

        public T Restore()
        {
            var binaryFormatter = new BinaryFormatter();
            using (var temp = new MemoryStream(_state))
            {
                return (T)binaryFormatter.Deserialize(temp);
            }
        }

        public static RandomizerState<T> Save(T randomizer)
        {
            if (randomizer == null) throw new ArgumentNullException(nameof(randomizer));

            var binaryFormatter = new BinaryFormatter();
            using (var temp = new MemoryStream())
            {
                binaryFormatter.Serialize(temp, randomizer);
                return new RandomizerState<T>(temp.ToArray());
            }
        }
    }
}
