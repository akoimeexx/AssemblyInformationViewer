using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace com.akoimeexx.utilities.assemblyinformation {
    public sealed partial class ObservableSortedSet<TIn> : 
    ISet<TIn>, 
    INotifyCollectionChanged, 
    INotifyPropertyChanging, 
    INotifyPropertyChanged {
#region Properties
        internal readonly SortedSet<TIn> items = new SortedSet<TIn>();
        public IComparer<TIn> Comparer {
            get { return items.Comparer; }
        }
        public int Count {
            get { return items.Count; }
        }
        bool ICollection<TIn>.IsReadOnly {
            get { return ((ICollection<TIn>)items).IsReadOnly; }
        }
        public TIn Max {
            get { return items.Max; }
        }
        public TIn Min {
            get { return items.Min; }
        }
#endregion Properties
    }
    public sealed partial class ObservableSortedSet<TIn> {
#region Events
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;
#endregion Events
    }
    public sealed partial class ObservableSortedSet<TIn> {
#region Methods
        public bool Add(TIn item) {
            sendPropertyChanging("Count");
            sendPropertyChanging("Max");
            sendPropertyChanging("Min");
            bool b = items.Add(item);
            sendCollectionChanged(NotifyCollectionChangedAction.Add);
            sendPropertyChanged("Count");
            sendPropertyChanged("Max");
            sendPropertyChanged("Min");
            return b;
        }
        void ICollection<TIn>.Add(TIn item) {
            Add(item);
        }
        public void Clear() {
            sendPropertyChanging("Count");
            sendPropertyChanging("Max");
            sendPropertyChanging("Min");
            items.Clear();
            sendCollectionChanged(NotifyCollectionChangedAction.Reset);
            sendPropertyChanged("Count");
            sendPropertyChanged("Max");
            sendPropertyChanged("Min");
        }
        public bool Contains(TIn item) {
            return items.Contains(item);
        }
        public void CopyTo(TIn[] array) {
            items.CopyTo(array);
        }
        public void CopyTo(TIn[] array, int index) {
            items.CopyTo(array, index);
        }
        public void CopyTo(TIn[] array, int index, int count) {
            items.CopyTo(array, index, count);
        }
        public override bool Equals(object obj) {
            return items.Equals(obj);
        }
        public void ExceptWith(IEnumerable<TIn> other) {
            sendPropertyChanging("Count");
            sendPropertyChanging("Max");
            sendPropertyChanging("Min");
            items.ExceptWith(other);
            sendCollectionChanged(NotifyCollectionChangedAction.Remove);
            sendPropertyChanged("Count");
            sendPropertyChanged("Max");
            sendPropertyChanged("Min");
        }
        public IEnumerator<TIn> GetEnumerator() {
            return items.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
        public override int GetHashCode() {
            return items.GetHashCode();
        }
        public ObservableSortedSet<TIn> GetViewBetween(
            TIn lowerValue, 
            TIn upperValue
        ) {
            return new ObservableSortedSet<TIn>(
                items.GetViewBetween(lowerValue, upperValue)
            );
        }
        public void IntersectWith(IEnumerable<TIn> other) {
            sendPropertyChanging("Count");
            sendPropertyChanging("Max");
            sendPropertyChanging("Min");
            items.IntersectWith(other);
            sendCollectionChanged(NotifyCollectionChangedAction.Remove);
            sendPropertyChanged("Count");
            sendPropertyChanged("Max");
            sendPropertyChanged("Min");
        }
        public bool IsProperSubsetOf(IEnumerable<TIn> other) {
            return items.IsProperSubsetOf(other);
        }
        public bool IsProperSupersetOf(IEnumerable<TIn> other) {
            return items.IsProperSupersetOf(other);
        }
        public bool IsSubsetOf(IEnumerable<TIn> other) {
            return items.IsSubsetOf(other);
        }
        public bool IsSupersetOf(IEnumerable<TIn> other) {
            return items.IsSupersetOf(other);
        }
        public bool Overlaps(IEnumerable<TIn> other) {
            return items.Overlaps(other);
        }
        public bool Remove(TIn item) {
            bool b = false;
            sendPropertyChanging("Count");
            sendPropertyChanging("Max");
            sendPropertyChanging("Min");
            b = items.Remove(item);
            sendCollectionChanged(NotifyCollectionChangedAction.Remove);
            sendPropertyChanged("Count");
            sendPropertyChanged("Max");
            sendPropertyChanged("Min");
            return b;
        }
        public int RemoveWhere(Predicate<TIn> match) {
            int i = default(int);
            sendPropertyChanging("Count");
            sendPropertyChanging("Max");
            sendPropertyChanging("Min");
            i = items.RemoveWhere(match);
            sendCollectionChanged(NotifyCollectionChangedAction.Remove);
            sendPropertyChanged("Count");
            sendPropertyChanged("Max");
            sendPropertyChanged("Min");
            return i;
        }
        public IEnumerable<TIn> Reverse() {
            return items.Reverse();
        }
        internal void sendCollectionChanged(
            NotifyCollectionChangedAction action = NotifyCollectionChangedAction.Reset
        ) {
            sendCollectionChanged(new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Reset
            ));
        }
        internal void sendCollectionChanged(
            NotifyCollectionChangedEventArgs collectionChangedArgs
        ) {
            CollectionChanged?.Invoke(
                this,
                collectionChangedArgs
            );
        }
        internal void sendPropertyChanging(
            [CallerMemberName] string property = null
        ) {
            PropertyChanging?.Invoke(
                this, 
                new PropertyChangingEventArgs(property)
            );
        }
        internal void sendPropertyChanged(
            [CallerMemberName] string property = null
        ) {
            PropertyChanged?.Invoke(
                this, 
                new PropertyChangedEventArgs(property)
            );
        }
        internal bool Set<T>(
            ref T field, 
            T value, 
            [CallerMemberName] string property = null
        ) {
            bool b = false;
            try {
                sendPropertyChanging(property);
                field = value;
                sendPropertyChanged(property);
            } catch (Exception e) {
                throw e;
            } finally { b = Equals(field, value); }
            return b;
        }
        public bool SetEquals(IEnumerable<TIn> other) {
            return items.SetEquals(other);
        }
        public void SymmetricExceptWith(IEnumerable<TIn> other) {
            sendPropertyChanging("Count");
            sendPropertyChanging("Max");
            sendPropertyChanging("Min");
            items.SymmetricExceptWith(other);
            sendCollectionChanged(NotifyCollectionChangedAction.Remove);
            sendPropertyChanged("Count");
            sendPropertyChanged("Max");
            sendPropertyChanged("Min");
        }
        public void UnionWith(IEnumerable<TIn> other) {
            sendPropertyChanging("Count");
            sendPropertyChanging("Max");
            sendPropertyChanging("Min");
            items.UnionWith(other);
            sendCollectionChanged(NotifyCollectionChangedAction.Add);
            sendPropertyChanged("Count");
            sendPropertyChanged("Max");
            sendPropertyChanged("Min");
        }
#endregion Methods
    }
    public sealed partial class ObservableSortedSet<TIn> {
#region Constructors & Destructor
        public ObservableSortedSet() : this(new TIn[] { }) { }
        public ObservableSortedSet(
            IComparer<TIn> comparer
        ) : this(default(TIn[]), comparer) { }
        public ObservableSortedSet(
            IEnumerable<TIn> collection
        ) : this(collection, default(IComparer<TIn>)) { }
        public ObservableSortedSet(
            IEnumerable<TIn> collection,
            IComparer<TIn> comparer
        ) {
            items = new SortedSet<TIn>(collection, comparer);
        }
#endregion Constructors & Destructor
    }
}
