using System;
using Godot;
using Godot.Collections;
using ProjectArchaetech.events;
using ProjectArchaetech.resources;
using static ProjectArchaetech.events.EventBus;

namespace ProjectArchaetech {
	[GlobalClass]
	public partial class PopManager : Node {
		// These define how much one Pop consumes per month 
		// for each of the edible resources.
		[Export]
		public Dictionary<ResourceData, double> PrimaryFoodDemands { set; get; }
		[Export] // Currently not used but will be
		public Dictionary<ResourceData, double> SecondaryFoodDemands { set; get; } 
		[Export]
		public Array<ResourceData> PrimaryFoodChoices { set; get; }
		public int PopCount { 
			get => popCount; 
			set {
				popCount = value;
				Global.EventBus.Publish(this, new PopCountUpdatedEvent()); 
			}
		}
		public double GrowthRate { get => growthRate; set => growthRate = value; }
		public double GrowthProgress { get => growthProgress; set => growthProgress = value; }
		public int NUnemployed { 
			get => nUnemployed; 
			set {
				nUnemployed = value;
				Global.EventBus.Publish(this, new PopCountUpdatedEvent()); 
			}
		}

		private int popCount;
		private double growthRate;
		private double growthProgress;
		private int nUnemployed;
	
		[Signal]
		public delegate void PopGrewEventHandler();

		public PopManager() {
			this.PrimaryFoodDemands = new Dictionary<ResourceData, double>();
			this.SecondaryFoodDemands = new Dictionary<ResourceData, double>();
			this.PrimaryFoodChoices = new Array<ResourceData>();
		}

		public override void _Ready() {
			this.PopCount = 25;
			this.GrowthRate = 0.05;
			this.GrowthProgress = 0.0;
			this.NUnemployed = 25;
			Global.EventBus.Subscribe<NewMonthEvent>((sender, e) => this.Update());
		}

		private int ConsumeFood() {
			// The Pops will consume primary foods based on a priority ranking.
			int nUnfed = this.PopCount;
			foreach (ResourceData foodType in this.PrimaryFoodChoices) {
				if (Global.ResManager.Resources.ContainsKey(foodType)) {
					double stores = Global.ResManager.Resources[foodType];
					double perCapitaNeed = this.PrimaryFoodDemands[foodType];
					int nFed = Math.Min((int) Math.Floor(stores / perCapitaNeed), nUnfed);
					if (nFed > 0) {
						nUnfed -= nFed;
						Global.ResManager.Consume(foodType, nFed * perCapitaNeed);
					}
				}
				if (nUnfed <= 0) {
					return 0; // Everyone is fed, nice!
				}
			}
			return nUnfed; // Hungry!
		}

		public void Update() {
			// We first compute how much food is needed if all Pops only consume basic food
			int yearlyFood = this.PopCount * 12;
			double foodStorage = 0.0;
			foreach (ResourceData foodType in this.PrimaryFoodChoices) {
				if (Global.ResManager.Resources.ContainsKey(foodType)) {
					// Convert the food stores into basic food and aggregate
					double stores = Global.ResManager.Resources[foodType];
					foodStorage += stores / PrimaryFoodDemands[foodType];
				}
			}
			int nUnfed = this.ConsumeFood();
			double growthModifier = 1.0;
			if (nUnfed != 0) {
				double starvation = nUnfed / this.PopCount;
				growthModifier = -Math.Sqrt(starvation);
			} else {
				growthModifier += Math.Clamp(foodStorage / (yearlyFood * 3) - 1, 0, 0.5);
			}
			this.GrowthProgress += this.GrowthRate * growthModifier;
			int realGrowth = (int) Math.Floor(this.GrowthProgress);
			if (realGrowth != 0) {
				// This is here to simulate a progress bar...
				this.GrowthProgress -= realGrowth;
				this.PopCount += realGrowth;
			}
		}
	}
}
