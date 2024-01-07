using System;
using C5;
using Godot;
using Godot.Collections;
using ProjectArchaetech.resources;

namespace ProjectArchaetech.common {
	[GlobalClass]
	public partial class Warehouse : Node {
		[Export]
		public Dictionary<ResourceData, double> Resources { get; set; }
		[Export]
		public Array<int> StorageLimit { set; get; }
		protected Dictionary<ResourceData, double> monthlyOutput;

		public Dictionary<ResourceData, double> MonthlyOutput { get => monthlyOutput; }

		[Signal] 
		public delegate void ResourceQtyUpdatedEventHandler(ResourceData res, double newQty);

		public Warehouse() {
			this.Resources = new Dictionary<ResourceData, double>();
			this.StorageLimit = new Array<int>();
			this.monthlyOutput = new Dictionary<ResourceData, double>();
		}

		public bool HasEnough(ResourceData res, double benchmark) {
			return this.Resources.ContainsKey(res) && this.Resources[res] >= benchmark;
		}

		public void Add(ResourceData res, double amount) {
			if (!this.Resources.ContainsKey(res)) {
				this.Resources[res] = 0;
			} else {
				double netAmount = this.Resources[res] + amount;
				this.Resources[res] = Math.Min(netAmount, this.StorageLimit[(int) res.type]);
			}
		}

		public void Add(Dictionary<ResourceData, double> affectedResources) {
			foreach (ResourceData res in affectedResources.Keys) {
				this.Add(res, affectedResources[res]);
			}
		}

		public void Add(Dictionary<ResourceData, int> affectedResources) {
			foreach (ResourceData res in affectedResources.Keys) {
				this.Add(res, affectedResources[res]);
			}
		}

		public void Add(HashDictionary<ResourceData, double> affectedResources) {
			foreach (KeyValuePair<ResourceData, double> res in affectedResources) {
				this.Add(res.Key, res.Value);
			}
		}

		public void Consume(ResourceData res, double amount) {
			if (this.HasEnough(res, amount)) {
				this.Resources[res] -= amount;
			}
		}

		public void Consume(Dictionary<ResourceData, double> affectedResources) {
			foreach (ResourceData res in affectedResources.Keys) {
				this.Consume(res, affectedResources[res]);
			}
		}

		public void Consume(Dictionary<ResourceData, int> affectedResources) {
			foreach (ResourceData res in affectedResources.Keys) {
				this.Consume(res, affectedResources[res]);
			}
		}

		public Dictionary<ResourceData, double> TakeAway(Dictionary<ResourceData, double> resources) {
			Dictionary<ResourceData, double> taken = new Dictionary<ResourceData, double>();
			foreach (ResourceData res in resources.Keys) {
				double proportion = this.Resources[res];
				double amountTaken = this.MonthlyOutput[res] * proportion;
				if (taken.ContainsKey(res)) {
					taken[res] += amountTaken;
				} else {
					taken[res] = amountTaken;
				}
			}
			this.Consume(taken);
			return taken;
		}

		public void Supply(JobData job, int nWorkers) {
			if (nWorkers <= 0) {
				return;
			}
			double k = 1.0;
			Dictionary<ResourceData, double> input = job.input.Duplicate(true);
			foreach (ResourceData res in input.Keys) {
				if (this.Resources[res] < 1) {
					// Use < 1 to avoid any possible floating point precision issue
					return;
				}
				input[res] *= nWorkers;
				k = Math.Min(this.Resources[res] / input[res], k);
			}
			Dictionary<ResourceData, double> output = job.output.Duplicate(true);
			foreach (ResourceData res in input.Keys) {
				input[res] *= k;
			}
			foreach (ResourceData res in output.Keys) {
				if (!this.MonthlyOutput.ContainsKey(res)) {
					this.MonthlyOutput[res] = 0;
				}
				if (res.type == ResourceType.Research) {
					// Research points do not need to be transported
					// so they will be directly credited into global warehouse.
					Global.ResManager.Add(res, output[res] * k * nWorkers);
				} else {
					this.MonthlyOutput[res] += output[res] * k * nWorkers;
				}
			}
			this.Consume(input);
			this.Add(this.MonthlyOutput);
		}

		public virtual void Reset() {
			foreach (ResourceData res in this.MonthlyOutput.Keys) {
				this.EmitSignal(SignalName.ResourceQtyUpdated, res, this.Resources[res]);
			}
			this.MonthlyOutput.Clear();
		}
	}
}
