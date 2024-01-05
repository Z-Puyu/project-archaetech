using C5;
using Godot;

namespace ProjectArchaetech.common.util {
    public partial class ConstructionQueue<T> : Node where T : Node {
        private readonly HashedLinkedList<ConstructibleTask<T>> active;
        private readonly HashedLinkedList<ConstructibleTask<T>> inactive;
        private int maxActiveSize;
        private readonly HashDictionary<Cell, ConstructibleTask<T>> constructionSites;

        public ConstructionQueue(int maxActiveSize) {
            this.active = new HashedLinkedList<ConstructibleTask<T>>();
            this.inactive = new HashedLinkedList<ConstructibleTask<T>>();
            this.maxActiveSize = maxActiveSize;
        }

        public int MaxActiveSize { get => MaxActiveSize; set => MaxActiveSize = value; }

        public int Size() {
            return this.active.Count + this.inactive.Count;
        }

        public void Enqueue(ConstructibleTask<T> task) {
            task.TerminateEvent += this.Remove;
            if (this.active.Count < this.MaxActiveSize) {
                this.active.InsertFirst(task);
                task.Start();
            } else {
                this.inactive.InsertFirst(task);
            }
            this.constructionSites.Add(task.Location, task);
        }

        public void Remove(ConstructibleTask<T> task) {
            task.TerminateEvent -= this.Remove;
            if (this.active.Contains(task)) {
                this.active.Remove(task);
            } else {
                this.inactive.Remove(task);
            }
            Global.Grave.Enqueue(task);
            this.constructionSites.Remove(task.Location);
            this.Refresh();
        }

        public bool RemoveAt(Cell location, out ConstructibleTask<T> task) {
            return this.constructionSites.Remove(location, out task);
        }

        public void Refresh() {
            while (this.active.Count > this.MaxActiveSize) {
                ConstructibleTask<T> task = this.active.RemoveFirst();
                task.Pause();
                this.inactive.InsertLast(task);
            }
            while (this.active.Count < this.MaxActiveSize) {
                if (this.inactive.IsEmpty) {
                    break;
                }
                ConstructibleTask<T> task = this.inactive.RemoveLast();
                this.active.InsertFirst(task);
                task.Start();
            }
        }

        public void ShrinkBy(int size) {
            this.MaxActiveSize -= size;
            this.Refresh();
        }

        public void ExpandBy(int size) {
            this.MaxActiveSize += 1;
            this.Refresh();
        }
    }
}