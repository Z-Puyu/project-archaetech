using System;
using C5;
using Godot;
using ProjectArchaetech.events;

namespace ProjectArchaetech.common.util {
	public partial class ConstructionQueue<T> : Node where T : Node {
		private readonly HashedLinkedList<ConstructibleTask<T>> active;
		private readonly HashedLinkedList<ConstructibleTask<T>> inactive;
		private int maxActiveSize;
		private readonly HashDictionary<Cell, ConstructibleTask<T>> constructionSites;

		public ConstructionQueue(int maxActiveSize) {
			this.active = new HashedLinkedList<ConstructibleTask<T>>();
			this.inactive = new HashedLinkedList<ConstructibleTask<T>>();
			this.constructionSites = new HashDictionary<Cell, ConstructibleTask<T>>();
			this.maxActiveSize = maxActiveSize;
			Global.EventBus.Subscribe<ConstructionTaskCompletedEvent<T>>(
				this.OnTaskCompleted
			);
		}

		public int MaxActiveSize { get => maxActiveSize; set => maxActiveSize = value; }

		public int Size() {
			return this.active.Count + this.inactive.Count;
		}

		private void OnTaskCompleted(object sender, EventArgs e) {
			ConstructibleTask<T> task = (ConstructibleTask<T>) sender;
			if (this.constructionSites.Contains(task.Location)) {
				// Check if the task is really in this queue, just to be safe.
				this.Remove(task);
			}
		}

		public void Enqueue(ConstructibleTask<T> task) {
			if (this.active.Count < this.MaxActiveSize) {
				this.active.InsertFirst(task);
				task.Start();
			} else {
				this.inactive.InsertFirst(task);
			}
			this.constructionSites.Add(task.Location, task);
			this.AddChild(task);
		}

		public void Remove(ConstructibleTask<T> task) {
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

		public override string ToString() {
			string active = "{";
			string inactive = "{";
			foreach (ConstructibleTask<T> t in this.active) {
				active += (t.ToString() + " -> ");
			}
			foreach (ConstructibleTask<T> t in this.inactive) {
				inactive += (t.ToString() + " -> ");
			}
			return active + "}\n" + inactive + "}";
		}
	}
}
