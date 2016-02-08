using System;
using System.Collections.Generic;
using System.Text;

namespace SharpDevs.Helpers.Ini
{
    
    public class ChangeList<T> : List<T>
    {
        public event EventHandler OnChange;

        protected void SetChanged()
        {
            if (OnChange != null)
                OnChange(this, new EventArgs());
        }

        public new void Add(T item)
        {            
            base.Add(item);
            SetChanged();
        }

        public new void AddRange(IEnumerable<T> collection)
        {
            base.AddRange(collection);
            SetChanged();
        }

        public new void Clear()
        {
            base.Clear();
            SetChanged();
        }

        public new void Insert(int index, T item)
        {
            base.Insert(index, item);
            SetChanged();
        }

        public new void InsertRange(int index, IEnumerable<T> collection)
        {
            base.InsertRange(index, collection);
            SetChanged();
        }

        public new bool Remove(T item)
        {
            bool result = base.Remove(item);
            SetChanged();
            return result;
        }

        public new int RemoveAll(Predicate<T> match)
        {
            int result = base.RemoveAll(match);
            SetChanged();
            return result;
        }

        public new void RemoveAt(int index)
        {
            base.RemoveAt(index);
            SetChanged();
        }

        public new void RemoveRange(int index, int count)
        {
            base.RemoveRange(index, count);
            SetChanged();
        }

        public new void Reverse(int index, int count)
        {
            base.Reverse(index, count);
            SetChanged();
        }

        public new void Reverse()
        {
            base.Reverse();
            SetChanged();
        }

        public new void Sort(Comparison<T> comparison)
        {
            base.Sort(comparison);
            SetChanged();
        }

        public new void Sort()
        {
            base.Sort();
            SetChanged();
        }

        public new void Sort(int index, int count, IComparer<T> comparer)
        {
            base.Sort(index, count, comparer);
            SetChanged();
        }

        public new void Sort(IComparer<T> comparer)
        {
            base.Sort(comparer);
            SetChanged();
        }

        public new void TrimExcess()
        {
            base.TrimExcess();
            SetChanged();
        }

        //[Obsolete()]
        //public new void TrimToSize()
        //{
        //    base.TrimToSize();
        //    SetChanged();
        //}
    }
}
