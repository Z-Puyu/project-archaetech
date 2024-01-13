using System;

namespace ProjectArchaetech.common {
    public abstract class Effect<T> {
        private readonly string name;
        protected readonly Action<T> action;

        public Effect(string name, Action<T> action) {
            this.name = name;
            this.action = action;
        }

        public abstract void Fire(in T arg);

        public static string ToString(in T arg) {
            return "";
        }
    }
}