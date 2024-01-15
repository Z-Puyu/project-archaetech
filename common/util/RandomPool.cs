using System;
using C5;
using Godot;
using ProjectArchaetech.interfaces;

namespace ProjectArchaetech.common.util {
    public class RandomPool<T> where T : IPoolable<T> {
        private readonly Action<T> pushObj;
        private readonly Action<T> pullObj;
        private readonly RandomSelector<T> pool;
        private readonly HashSet<T> items;

        public HashSet<T> Items => items;

        public RandomPool(Action<T> pushObj, Action<T> pullObj) {
            this.pushObj = pushObj;
            this.pullObj = pullObj;
            this.pool = new RandomSelector<T>();
            this.pool.Add(new KeyValuePair<int, T>(this.pool.TotalWeight, default));
            this.items = new HashSet<T>();
        }

        public T Poll() {
            if (this.pool.IsEmpty()) {
                return default;
            }
            return this.pool.Select();
        }

        public void Add(int weight, T t) {
            this.pool.Remove(default);
            this.pool.Add(new KeyValuePair<int, T>(weight, t));
            this.pool.Add(new KeyValuePair<int, T>(this.pool.TotalWeight, default)); 
            this.Items.Add(t);   
        }

        public void Pop(T t) {
            this.pool.Remove(t);
            this.Items.Remove(t);
        }
        
        public override string ToString() {
            return this.pool.ToString();
        }
    }
}