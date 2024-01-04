using C5;
using Godot.Collections;
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

        public void AddItems(params KeyValuePair<int, T>[] items) {
            foreach (KeyValuePair<int, T> item in items) {
                this.items.Add(this.totalWeight + item.Key, item.Value);
                this.lookUp.Add(item.Value, this.totalWeight + item.Key);
                this.totalWeight += item.Key;
            }
        }

        public void RemoveItem(T item) {
            if (this.lookUp.Contains(item)) {
                int key = this.lookUp[item]; // Get the cumulative weight of this item.
                KeyValuePair<int, T> prev;
                int weight = key;
                if (this.items.TryPredecessor(key, out prev)) {
                    // If there is a predecessor, calculate the weight.
                    weight -= prev.Key;
                }
                this.items.Remove(key); // Remove this item.
                KeyValuePair<int, T> next;
                while (this.items.TrySuccessor(key, out next)) {
                    // For every higher entry, rectify the weights.
                    this.items.Remove(key);
                    key = next.Key - weight;
                    this.items.Add(key, next.Value);
                }
            }
        }
    }
}