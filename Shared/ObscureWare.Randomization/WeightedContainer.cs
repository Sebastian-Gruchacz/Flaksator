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
            this.RestoreElements(elements.ToArray());
        }

        public T DrawElement(IRandomizer randomizer)
        {
            if (this._revolver != null)
            {
                return this._revolver.DrawElement(randomizer);
            }
            else
            {
                int rnd = randomizer.GetNext(0, (int) this._totalScale);
                uint totalScore = 0;
                foreach (T element in this._elements)
                {
                    totalScore += element.ProbabilityWeight;
                    if (totalScore >= rnd)
                        return element;
                }

                // closing element
                return this._elements[this._elements.Count];
            }
        }

        private void RestoreElements(T[] elements)
        {
            this._elements.AddRange(elements);
            foreach (var element in elements)
            {
                this._totalScale += element.ProbabilityWeight;
            }

            if (this._elements.Count * this.MODE_BALANCE_FACTOR > this._totalScale)
            {
                this.SetRevolverMode();
            }
            else
            {
                this.SetScanningMode();
            }
        }

        private void SetScanningMode()
        {
            this._revolver = null;
        }

        private void SetRevolverMode()
        {
            this._revolver = new ProbabilityRevolver(this._totalScale, this._elements);
        }

        private class ProbabilityRevolver
        {
            private readonly uint _totalScale;
            private readonly T[] _cylinder;

            public ProbabilityRevolver(uint totalScale, IEnumerable<T> elements)
            {
                this._totalScale = totalScale;
                this._cylinder = new T[totalScale];
                int startIndex = 0;
                foreach (T element in elements)
                {
                    for (int i = startIndex; i < startIndex + element.ProbabilityWeight; i++)
                    {
                        this._cylinder[i] = element;
                    }

                    startIndex += element.ProbabilityWeight;
                }
            }

            public T DrawElement(IRandomizer randomizer)
            {
                int rnd = randomizer.GetNext(0, (int) this._totalScale);
                return this._cylinder[rnd];
            }
        }
    }

    public interface IWeightedElement
    {
        byte ProbabilityWeight { get; }
    }
}
