using System;

namespace ProjectArchaetech.util {
    public class Lazy<T> : ILazyBase {
        public delegate X Supplier<out X>();
        // public delegate bool Predicate<in X>(X x);
        public delegate Z Combiner<in X, in Y, out Z>(X x, Y y);
        private readonly Supplier<T> producer;
        private T value;

        public T Value => this.value ?? this.producer.Invoke();

        private Lazy(Supplier<T> producer , T value) {
            this.producer = producer;
            this.value = value;
        }

        public static Lazy<T> Of<S>(S v) where S : T {
            Lazy<T> lazy = new Lazy<T>(() => v, v);
            return lazy;
        }

        public static Lazy<T> Of<S>(Supplier<S> s) where S : class, T {
            return new Lazy<T>(s, default);
        }

        public static Lazy<T> OfValue(Supplier<T> s) {
            return new Lazy<T>(s, default);
        }

        public T Get() {
            return this.Value;
        }

        public override string ToString() {
            return this.Value?.ToString() ?? "?";
        }

        // Probably not gonna use these but I'll implement them anyways :O

        public Lazy<U> Map<S, U>(Func<T, S> f) where S : class, U {
            return Lazy<U>.Of<S>(() => f.Invoke(this.Get()));
        }

        public Lazy<U> FlatMap<S, U, V>(Func<T, V> f) where V : Lazy<S> where S: class, U {
            return Lazy<U>.Of<S>(() => f.Invoke(this.Get()).Get());
        }

        public Lazy<bool> Filter(Predicate<T> p) {    
            return Lazy<bool>.OfValue(() => p.Invoke(this.Get()));
        }

        public override bool Equals(object another) {
            return (another is ILazyBase) && (
                (this.Get() == null && ((ILazyBase) another).Get() == null) || 
                this.Get().Equals(((ILazyBase) another).Get())
            );
        }

        public Lazy<R> Combine<S, R, U>(Lazy<S> lazy, Func<T, S, U> f) where U : class, R {
            return Lazy<R>.Of(() => f.Invoke(this.Get(), lazy.Get()));
        }

        object ILazyBase.Get() {
            return this.Get();
        }
    }
}