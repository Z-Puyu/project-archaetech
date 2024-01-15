using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ProjectArchaetech.common.util;
using ProjectArchaetech.interfaces;
using ProjectArchaetech.triggers;
using ProjectArchaetech.util.events;

namespace ProjectArchaetech.events {
	[JsonDerivedType(typeof(GameEvent), typeDiscriminator: "Event")]
	[JsonDerivedType(typeof(RandomGameEvent), typeDiscriminator: "RandomEvent")]
	public partial class GameEvent : IEvent, IPoolable<GameEvent> {
		public string Prefix { get; set; }
		public int Id { get; set; }
		public string Title { get; set; }
		public string Desc { get; set; }
		public bool Visible { get; set; }
		public AndCondition Potential { get; set; }
		public AndCondition Triggers { get; set; }
		public int Mtth { get; set; }
		public List<Effect> ImmediateEffects { get; set; }
		public List<Option> Options { get; set; }

		public GameEvent() { 
			this.Prefix = "";
			this.Id = 0;
			this.Title = "";
			this.Desc = "";
			this.Visible = true;
			this.Potential = new AndCondition();
			this.Triggers = new AndCondition();
			this.Mtth = 0;
			this.ImmediateEffects = new List<Effect>();
			this.Options = new List<Option>();
		}

		public GameEvent(string prefix, int id, string title, string desc, bool visible, 
			AndCondition potential, AndCondition triggers, int mtth, List<Effect> immediateEffects,
			List<Option> options) {
			this.Prefix = prefix;
			this.Id = id;
			this.Title = title;
			this.Desc = desc;
			this.Potential = potential;
			this.Triggers = triggers;
			this.Mtth = mtth;
			this.ImmediateEffects = immediateEffects;
			this.Options = options;
		}

		public string GetId() {
			return this.Prefix + "." + this.Id;
		}

		public bool IsValid() {
			return this.Potential.IsTrue();
		}

		public bool CanFire() {
			return this.Triggers.IsTrue();
		}

		public void Fire() {
			foreach (Effect e in this.ImmediateEffects) {
				e.Invoke();
			}
			Console.WriteLine("Immediate Effects fired");
			if (this.Visible) {
				Global.EventBus.Publish(this, new GameEventFiredEvent());
			}
		}

		public void Customise(string customTitle, string customDesc) {
			this.Title = customTitle;
			this.Desc = customDesc;
		}

		public CountDown Schedule(Action @return) {
			Random rand = new Random(Guid.NewGuid().GetHashCode());
			int t = (int) (this.Mtth * (0.5 + rand.NextDouble() + rand.NextDouble() + 0.5 * rand.NextDouble()));
			return new CountDown(t, this.Fire, @return);
		}

		public void Initialise(Action<GameEvent> @return) {
			throw new NotImplementedException();
		}

		public void Return() {
			throw new NotImplementedException();
		}
	}

	public partial class RandomGameEvent : GameEvent {
		public int Factor { set; get; }

		public RandomGameEvent() : base() { 
			this.Factor = 1;
		}

		public RandomGameEvent(string prefix, int id, string title, string desc, 
			bool visible, AndCondition potential, AndCondition triggers, int mtth, 
			List<Effect> immediateEffects, List<Option> options, int factor) : 
			base(prefix, id, title, desc, visible, potential, triggers, mtth, 
			immediateEffects, options) {
			this.Factor = factor;
		}
	}
}
