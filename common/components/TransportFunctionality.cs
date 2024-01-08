using C5;
using Godot;
using Godot.Collections;
using ProjectArchaetech.interfaces;

namespace ProjectArchaetech.common.components {
    public class TransportFunctionality : IFunctionable {
        private readonly HashDictionary<Building, TransportRoute> outPaths;
        private int maxNOut;

        public TransportFunctionality(HashDictionary<Building, TransportRoute> outPaths, 
            int maxNOut) {
            this.outPaths = outPaths;
            this.maxNOut = maxNOut;
        }

        public int MaxNOut { get => maxNOut; set => maxNOut = value; }

        public void Execute(Dictionary<string, Variant> updatedUIData) {
            foreach (TransportRoute route in this.outPaths.Values) {
                route.Work(ref updatedUIData);
            }
        }
    }
}