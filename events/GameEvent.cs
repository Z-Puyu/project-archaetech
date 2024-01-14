using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ProjectArchaetech.interfaces;

namespace ProjectArchaetech.events {
	[JsonDerivedType(typeof(GameEvent), typeDiscriminator: "Event")]
    [JsonDerivedType(typeof(RandomGameEvent), typeDiscriminator: "RandomEvent")]
	public partial class GameEvent : IEvent, IPoolable<GameEvent> {
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

		public GameEvent(string prefix, int id, string title, string desc, int mtth, 
			List<Option> options) {
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

        public void Initialise(Action<GameEvent> @return)
        {
            throw new NotImplementedException();
        }

        public void Return()
        {
            throw new NotImplementedException();
        }
    }

	public partial class RandomGameEvent : GameEvent {
		public int Factor { set; get; }

		public RandomGameEvent() : base() { 
			this.Factor = 1;
		}

		public RandomGameEvent(string prefix, int id, string title, string desc, int mtth, 
			List<Option> options, int factor) : base(prefix, id, title, desc, mtth, options) {
			this.Factor = factor;
		}
	}
}
