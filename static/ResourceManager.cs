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

		public ResourceManager() : base() {
			this.RpResource = null;
		}

		public void ClearRp() {
			this.Resources[RpResource] = 0;
		}

		public override void Reset() {
			base.Reset();
		}

		public void SendWarehouseInfo() {
			Global.EventBus.Publish(this, new TechProgressEvent((int) this.Resources[this.RpResource]));
			foreach (ResourceData res in this.MonthlyOutput.Keys) {
				Console.WriteLine(res + " : " + this.Resources[res]);
				this.EmitSignal(Warehouse.SignalName.ResourceQtyUpdated, res, this.Resources[res]);
			}
		}
	}
}
