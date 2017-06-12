using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace com.akoimeexx.utilities.assemblyinformation {
    public partial class ObservableStack<T> :
    Notifiable,
    INotifyCollectionChanged,
    IEnumerable<T>,
    IEnumerable,
    ICollection,
    IReadOnlyCollection<T> {
#region Properties
        internal readonly Stack<T> stack = new Stack<T>();
        public int Count {
            get { return stack.Count; }
        }
        bool ICollection.IsSynchronized {
            get { return ((ICollection)stack).IsSynchronized; }
        }
        object ICollection.SyncRoot {
            get { return ((ICollection)stack).SyncRoot; }
        }
        public virtual T Top {
            get { return Peek(); }
        }
#endregion Properties
    }
    public partial class ObservableStack<T> {
#region Events
        public event NotifyCollectionChangedEventHandler CollectionChanged;
#endregion Events
    }
    public partial class ObservableStack<T> {
#region Methods
        public void Clear() {
            SendPropertyChanging("Count");
            SendPropertyChanging("Top");
            stack.Clear();
            sendCollectionChanged();
            SendPropertyChanged("Count");
            SendPropertyChanged("Top");
        }
        public bool Contains(T item) {
            return stack.Contains(item);
        }
        public void CopyTo(T[] array, int index) {
            stack.CopyTo(array, index);
        }
        void ICollection.CopyTo(Array array, int index) {
            ((ICollection)stack).CopyTo(array, index);
        }
        public IEnumerator<T> GetEnumerator() {
            return ((IEnumerable<T>)stack).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable<T>)stack).GetEnumerator();
        }
        public T Peek() {
            return (Count > 0) ? stack.Peek() : default(T);
        }
        public T Pop() {
            SendPropertyChanging("Count");
            SendPropertyChanging("Top");
            T o = stack.Pop();
            sendCollectionChanged();
            SendPropertyChanged("Count");
            SendPropertyChanged("Top");
            return o;
        }
        public void Push(T item) {
            SendPropertyChanging("Count");
            SendPropertyChanging("Top");
            stack.Push(item);
            sendCollectionChanged();
            SendPropertyChanged("Count");
            SendPropertyChanged("Top");
        }
        internal void sendCollectionChanged(
            NotifyCollectionChangedAction action = NotifyCollectionChangedAction.Reset
        ) {
            CollectionChanged?.Invoke(
                this,
                new NotifyCollectionChangedEventArgs(action)
            );
        }
        public T[] ToArray() {
            return stack.ToArray();
        }
        public void TrimExcess() {
            stack.TrimExcess();
            sendCollectionChanged();
        }
#endregion Methods
    }
    public partial class ObservableStack<T> {
#region Constructors & Destructor
        public ObservableStack() {
            stack = new Stack<T>();
        }
        public ObservableStack(IEnumerable<T> collection) {
            stack = new Stack<T>(collection);
        }
        public ObservableStack(int capacity) {
            stack = new Stack<T>(capacity);
        }
#endregion Constructors & Destructor
    }
}
