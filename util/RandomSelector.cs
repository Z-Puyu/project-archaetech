using C5;
using System;

namespace ProjectArchaetech {
    public class RandomSelector<T> {
        private readonly TreeDictionary<int, T> items;
        private readonly HashDictionary<T, int> lookUp;
        private int totalWeight;
        private readonly Random rand;

        public int TotalWeight => totalWeight;


        public RandomSelector() {
            this.items = new TreeDictionary<int, T>();
            this.lookUp = new HashDictionary<T, int>();
            this.totalWeight = 0;
            this.rand = new Random(Guid.NewGuid().GetHashCode());
        }

        public bool IsEmpty() {
            return this.items.IsEmpty;
        }

        public T Select() {
            double r = this.rand.NextDouble() * this.TotalWeight;
            return this.items.WeakPredecessor((int) r).Value;
        }

        public void Add(params KeyValuePair<int, T>[] items) {
            foreach (KeyValuePair<int, T> item in items) {
                this.items.Add(this.TotalWeight, item.Value);
                this.lookUp.Add(item.Value, this.TotalWeight);
                this.totalWeight += item.Key;
            }
        }

        public void Remove(T item) {
            if (this.lookUp.Contains(item)) {
                int key = this.lookUp[item]; // Get the cumulative weight of this item.
                this.items.Remove(key); // Remove this item.
                this.lookUp.Remove(item);
                if (this.items.TrySuccessor(key, out KeyValuePair<int, T> next)) {
                    // If the item has at least one successor, 
                    // We calculate by how much should the successors be shifted down.
                    int diff = next.Key - key;
                    
                    this.totalWeight -= diff;
                    do {
                        this.items.Remove(next.Key);
                        this.items.Add(next.Key - diff, next.Value);
                        key = next.Key - diff;
                        this.lookUp[next.Value] = key;
                    } while (this.items.TrySuccessor(key, out next));
                } else {
                    this.totalWeight = key;
                }
            }
        }

        public override string ToString() {
            string str = "";
            foreach (KeyValuePair<int, T> pair in this.items) {

                str += pair.Value == null ? $"{pair.Key}: Null\n" : $"{pair.Key}: {pair.Value}\n";
            }
            return str;
        }
    }
}