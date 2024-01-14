using System.Collections.Generic;
using ProjectArchaetech.triggers;

namespace ProjectArchaetech.events {
	public partial class Option {
		public string Id { get; set; }
		public string Desc { get; set; }
		public AndCondition Potential { get; set; } // Conditions which are easy to evaluate.
		public AndCondition Triggers { get; set; } // Conditions which are expensive to evaluate.
		public List<Effect> Effects { get; set; }

		public Option() {
			this.Id = "";
			this.Desc = "";
			this.Potential = new AndCondition();
			this.Triggers = new AndCondition();
			this.Effects = new List<Effect>();
		}

		public Option(string id, string desc, AndCondition potential, AndCondition triggers, List<Effect> effects) {
			this.Id = id;
			this.Desc = desc;
			this.Potential = potential;
			this.Triggers = triggers;
			this.Effects = effects;
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
