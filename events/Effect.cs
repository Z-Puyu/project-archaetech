using System;
using System.Reflection;
using ProjectArchaetech.common;

namespace ProjectArchaetech.events {
	public partial class Effect {
		public string Name { get; set; }
		public dynamic Arg { get; set; }
		public string Tooltip { get; set; }
		public bool Visible { get; set; }

		public Effect() {
			this.Name = "";
			this.Arg = null;
			this.Tooltip = "";
			this.Visible = true;
		}

		public Effect(string name, dynamic arg, string tooltip, bool visible) {
			this.Name = name;
			this.Arg = arg;
			this.Tooltip = tooltip;
			this.Visible = visible;
		}

		public void Invoke() {
			typeof(ScriptedEffects).GetMethod(this.Name, BindingFlags.Static).Invoke(null, Arg);
		}

		public override string ToString() {
			string str = "";
			Type type = typeof(ScriptedEffects).GetNestedType(Name);
			string effectDesc = (string) type.GetMethod("ToString", BindingFlags.Static)
				.Invoke(null, [this.Arg]);
			return str + effectDesc + "\n" + this.Tooltip + "\n";
		}
	}
}
