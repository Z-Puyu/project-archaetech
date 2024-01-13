using System;
using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace ProjectArchaetech.events {
	[RegisteredType(nameof(Tech), "", nameof(Resource)), GlobalClass]
	public partial class Option : Resource {
		[Export]
		public string Id { set; get; }
		[Export]
		public string Desc { set; get; }
		[Export]
		public Condition Potential { set; get; } // Conditions which are easy to evaluate.
		[Export]
		public Condition Triggers { set; get; } // Conditions which are expensive to evaluate.
		[Export]
		public Array<Effect> Effects { set; get; }

		public Option() {
			this.Id = "";
			this.Desc = "";
			this.Potential = new Condition();
			this.Triggers = new Condition();
			this.Effects = new Array<Effect>();
		}

		public override string ToString() {
			string str = "";
			foreach (Effect e in this.Effects) {
				str += e.ToString();
			}
			return str;
		}
	}
}
