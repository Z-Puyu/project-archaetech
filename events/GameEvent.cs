using System;
using System.Collections.Generic;
using Godot;

namespace ProjectArchaetech.events {
	public partial class GameEvent : IEvent {
		public string Prefix { get; set; }
		public int Id { get; set; }
		public string Title { get; set; }
		public string Desc { get; set; }
		public int Mtth { get; set; }
		public List<Option> Options { get; set; }

		public GameEvent() { 
			this.Prefix = "";
			this.Id = 0;
			this.Title = "";
			this.Desc = "";
			this.Mtth = 0;
			this.Options = new List<Option>();
		}

		public GameEvent(string prefix, int id, string title, string desc, int mtth, List<Option> options) {
			this.Prefix = prefix;
			this.Id = id;
			this.Title = title;
			this.Desc = desc;
			this.Mtth = mtth;
			this.Options = options;
		}

		public string GetId() {
			return this.Prefix + "." + this.Id;
		}

		public void Fire() {
			Console.WriteLine("");
		}
	}
}
