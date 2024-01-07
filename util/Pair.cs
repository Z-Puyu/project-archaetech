using Godot;

namespace ProjectArchaetech.util {
	[GlobalClass]
	public partial class Pair : RefCounted {
		private Variant first;
		private Variant second;

		public Pair(Variant first, Variant second) {
			this.first = first;
			this.second = second;
		}

		public Variant First { get => first; set => first = value; }
		public Variant Second { get => second; set => second = value; }
	}
}
