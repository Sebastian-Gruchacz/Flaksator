using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObscureWare.Randomization
{
    /// <summary>
    /// Not thread safe.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WeightedContainer<T> where T : IWeightedElement
    {
        private uint _totalScale = 0;
        private readonly List<T> _elements = new List<T>();
        private int MODE_BALANCE_FACTOR = 3;
        private ProbabilityRevolver _revolver = null;

        public WeightedContainer(IEnumerable<T> elements)
        {
            RestoreElements(elements.ToArray());
        }

        public T DrawElement(IRandomizer randomizer)
        {
            if (_revolver != null)
            {
                return _revolver.DrawElement(randomizer);
            }
            else
            {
                int rnd = randomizer.GetNext(0, (int)_totalScale);
                uint totalScore = 0;
                foreach (T element in _elements)
                {
                    totalScore += element.ProbabilityWeight;
                    if (totalScore >= rnd)
                        return element;
                }

                // closing element
                return _elements[_elements.Count];
            }
        }

        private void RestoreElements(T[] elements)
        {
            _elements.AddRange(elements);
            foreach (var element in elements)
            {
                _totalScale += element.ProbabilityWeight;
            }

            if (_elements.Count * MODE_BALANCE_FACTOR > _totalScale)
            {
                SetRevolverMode();
            }
            else
            {
                SetScanningMode();
            }
        }

        private void SetScanningMode()
        {
            _revolver = null;
        }

        private void SetRevolverMode()
        {
            _revolver = new ProbabilityRevolver(_totalScale, _elements);
        }

        private class ProbabilityRevolver
        {
            private readonly uint _totalScale;
            private readonly T[] _cylinder;

            public ProbabilityRevolver(uint totalScale, IEnumerable<T> elements)
            {
                _totalScale = totalScale;
                _cylinder = new T[totalScale];
                int startIndex = 0;
                foreach (T element in elements)
                {
                    for (int i = startIndex; i < startIndex + element.ProbabilityWeight; i++)
                    {
                        _cylinder[i] = element;
                    }

                    startIndex += element.ProbabilityWeight;
                }
            }

            public T DrawElement(IRandomizer randomizer)
            {
                int rnd = randomizer.GetNext(0, (int)_totalScale);
                return _cylinder[rnd];
            }
        }
    }

    public interface IWeightedElement
    {
        byte ProbabilityWeight { get; }
    }
}
