using System;
using System.Reflection;
using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;
using ProjectArchaetech.common;

namespace ProjectArchaetech.events {
	[RegisteredType(nameof(Tech), "", nameof(Resource)), GlobalClass]
	public partial class Effect : Resource {
		[Export]
		public Dictionary<string, Variant> Effects { set; get; }
		[Export]
		public string Tooltip { set; get; }
		[Export]
		public bool Visible { set; get; }

		public Effect() {
			this.Effects = new Dictionary<string, Variant>();
			this.Tooltip = "";
			this.Visible = true;
		}

		public override string ToString() {
			string str = "";
			foreach (string name in this.Effects.Keys) {
				Type type = typeof(ScriptedEffects).GetNestedType(name);
				str += type.GetMethod("ToString", BindingFlags.Static)
					.Invoke(null, [this.Effects[name]]);
				str += "\n";
			}
			return str + this.Tooltip + "\n";
		}
	}
}
