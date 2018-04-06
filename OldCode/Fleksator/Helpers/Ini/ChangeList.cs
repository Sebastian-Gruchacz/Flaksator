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
            if (this.OnChange != null)
                this.OnChange(this, new EventArgs());
        }

        public new void Add(T item)
        {            
            base.Add(item);
            this.SetChanged();
        }

        public new void AddRange(IEnumerable<T> collection)
        {
            base.AddRange(collection);
            this.SetChanged();
        }

        public new void Clear()
        {
            base.Clear();
            this.SetChanged();
        }

        public new void Insert(int index, T item)
        {
            base.Insert(index, item);
            this.SetChanged();
        }

        public new void InsertRange(int index, IEnumerable<T> collection)
        {
            base.InsertRange(index, collection);
            this.SetChanged();
        }

        public new bool Remove(T item)
        {
            bool result = base.Remove(item);
            this.SetChanged();
            return result;
        }

        public new int RemoveAll(Predicate<T> match)
        {
            int result = base.RemoveAll(match);
            this.SetChanged();
            return result;
        }

        public new void RemoveAt(int index)
        {
            base.RemoveAt(index);
            this.SetChanged();
        }

        public new void RemoveRange(int index, int count)
        {
            base.RemoveRange(index, count);
            this.SetChanged();
        }

        public new void Reverse(int index, int count)
        {
            base.Reverse(index, count);
            this.SetChanged();
        }

        public new void Reverse()
        {
            base.Reverse();
            this.SetChanged();
        }

        public new void Sort(Comparison<T> comparison)
        {
            base.Sort(comparison);
            this.SetChanged();
        }

        public new void Sort()
        {
            base.Sort();
            this.SetChanged();
        }

        public new void Sort(int index, int count, IComparer<T> comparer)
        {
            base.Sort(index, count, comparer);
            this.SetChanged();
        }

        public new void Sort(IComparer<T> comparer)
        {
            base.Sort(comparer);
            this.SetChanged();
        }

        public new void TrimExcess()
        {
            base.TrimExcess();
            this.SetChanged();
        }

        //[Obsolete()]
        //public new void TrimToSize()
        //{
        //    base.TrimToSize();
        //    SetChanged();
        //}
    }
}
