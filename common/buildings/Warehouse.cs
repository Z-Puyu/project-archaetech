using System;
using C5;
using Godot;
using Godot.Collections;
using ProjectArchaetech.resources;

namespace ProjectArchaetech.common {
	public partial class Warehouse : Node {
		[Export]
		public Dictionary<ResourceData, double> resources { get; set; }
		[Export]
		public Array<int> storageLimit { set; get; }
		protected HashDictionary<ResourceData, double> monthlyOutput;

		public HashDictionary<ResourceData, double> MonthlyOutput { get => monthlyOutput; }

		[Signal] 
		public delegate void QtyUpdatedEventHandler(ResourceData res, double newQty);

		public override void _Ready() {
			this.monthlyOutput = new HashDictionary<ResourceData, double>();
		}

		public bool HasEnough(ResourceData res, double benchmark) {
			return this.resources.ContainsKey(res) && this.resources[res] >= benchmark;
		}

		public void Add(ResourceData res, double amount) {
			if (res.type == ResourceType.Research) {
				resources[res] = amount;
			 } else if (!this.resources.ContainsKey(res)) {
				this.resources[res] = 0;
			} else {
				double netAmount = this.resources[res] + amount;
				this.resources[res] = Math.Min(netAmount, this.storageLimit[(int) res.type]);
			}
			this.EmitSignal(SignalName.QtyUpdated, res, this.resources[res]);
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
				this.resources[res] -= amount;
				this.EmitSignal(SignalName.QtyUpdated, res, this.resources[res]);
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
				double proportion = this.resources[res];
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
				if (this.resources[res] < 1) {
					// Use < 1 to avoid any possible floating point precision issue
					return;
				}
				input[res] *= nWorkers;
				k = Math.Min(this.resources[res] / input[res], k);
			}
			Dictionary<ResourceData, double> output = job.output.Duplicate(true);
			foreach (ResourceData res in input.Keys) {
				input[res] *= k;
			}
			foreach (ResourceData res in output.Keys) {
				if (this.MonthlyOutput.Contains(res)) {
					this.MonthlyOutput[res] += output[res] * k * nWorkers;
				} else {
					this.MonthlyOutput[res] = output[res] * k * nWorkers;
				}
			}
			this.Consume(input);
			this.Add(this.MonthlyOutput);
		}

		public virtual void Reset() {
			this.MonthlyOutput.Clear();
		}
	}
}
