using C5;
using System;

namespace ProjectArchaetech {
    public class RandomSelector<T> {
        private TreeDictionary<int, T> items;
        private HashDictionary<T, int> lookUp;
        private int totalWeight;
        private Random rand;

        public RandomSelector() {
            this.items = new TreeDictionary<int, T>();
            this.lookUp = new HashDictionary<T, int>();
            this.totalWeight = 0;
            this.rand = new Random(Guid.NewGuid().GetHashCode());
        }

        public T Select() {
            double r = this.rand.NextDouble() * this.totalWeight;
            return this.items.WeakPredecessor((int) r).Value;
        }

        public void Add(params KeyValuePair<int, T>[] items) {
            foreach (KeyValuePair<int, T> item in items) {
                this.items.Add(this.totalWeight, item.Value);
                this.lookUp.Add(item.Value, this.totalWeight);
                this.totalWeight += item.Key;
            }
        }

        public void Remove(T item) {
            if (this.lookUp.Contains(item)) {
                int key = this.lookUp[item]; // Get the cumulative weight of this item.
                this.items.Remove(key); // Remove this item.
                if (this.items.TrySuccessor(key, out KeyValuePair<int, T> next)) {
                    // If the item has at least one successor, 
                    // We calculate by how much should the successors be shifted down.
                    int diff = next.Key - key;
                    do {
                        this.items.Remove(next.Key);
                        this.items.Add(next.Key - diff, next.Value);
                    } while (this.items.TrySuccessor(key, out next));
                }
            }
        }
    }
}