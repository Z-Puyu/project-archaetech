using System;

namespace ProjectArchaetech.interfaces {
    public interface IPoolable<T> {
        public void Initialise(Action<T> @return);
        public void Return();
    }
}