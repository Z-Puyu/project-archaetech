using System;
using Godot;
using ProjectArchaetech.common;
using ProjectArchaetech.events;
using ProjectArchaetech.resources;
using static ProjectArchaetech.events.EventBus;

namespace ProjectArchaetech {
	[GlobalClass]
	public partial class ResourceManager : Warehouse {
		[Export]
		public ResourceData RpResource { set; get; }

		private static readonly ProductionStartedEvent productionStartedEvent = new ProductionStartedEvent();

		public ResourceManager() : base() {
			this.RpResource = null;
		}

		public override void _Ready() {
			Global.EventBus.Subscribe<NewMonthEvent>(this.OnNewProductionCycle);
		}

		private void OnNewProductionCycle(object sender, EventArgs e) {
			this.Resources[RpResource] = 0;
			Global.EventBus.Publish(this, productionStartedEvent); 
		}

		public int GetRP() {
			return (int) Math.Floor(this.Resources[this.RpResource]);
		}

		public void SendWarehouseInfo() {
			foreach (ResourceData res in this.MonthlyOutput.Keys) {
				this.EmitSignal(Warehouse.SignalName.ResourceQtyUpdated, res, this.Resources[res]);
			}
		}
	}
}
