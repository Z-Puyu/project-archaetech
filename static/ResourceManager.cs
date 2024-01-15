using System;
using Godot;
using ProjectArchaetech.common;
using ProjectArchaetech.util.events;
using ProjectArchaetech.resources;

namespace ProjectArchaetech {
	[GlobalClass]
	public partial class ResourceManager : Warehouse {
		[Export]
		public ResourceData RpResource { set; get; }

		[Signal] 
		public delegate void ResourceQtyUpdatedEventHandler(ResourceData res, double newQty);

		private static readonly ProductionStartedEvent productionStartedEvent = new ProductionStartedEvent();

		public ResourceManager() : base() {
			this.RpResource = null;
		}

		public override void _Ready() {
			Global.EventBus.Subscribe<NewMonthEvent>(this.OnNewProductionCycle);
		}

		public override void Add(ResourceData res, double amount) {
			base.Add(res, amount);
			this.EmitSignal(SignalName.ResourceQtyUpdated, res, this.Resources[res]);
		}

		public override void Consume(ResourceData res, double amount) {
			base.Consume(res, amount);
			this.EmitSignal(SignalName.ResourceQtyUpdated, res, this.Resources[res]);
		}

		private void OnNewProductionCycle(object sender, EventArgs e) {
			this.Resources[RpResource] = 0;
			Global.EventBus.Publish(this, productionStartedEvent); 
		}

		public int GetRP() {
			return (int) Math.Floor(this.Resources[this.RpResource]);
		}
	}
}
