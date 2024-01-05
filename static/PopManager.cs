using System;
using Godot;
using Godot.Collections;
using ProjectArchaetech.resources;

namespace ProjectArchaetech {
	public partial class PopManager : Node {
		// These define how much one Pop consumes per month 
		// for each of the edible resources.
		[Export]
		public Dictionary<ResourceData, double> primaryFoodDemands { set; get; }
		[Export] // Currently not used but will be
		public Dictionary<ResourceData, double> secondaryFoodDemands { set; get; } 
		[Export]
		public Array<ResourceData> primaryFoodChoices { set; get; }
		public int PopCount { get => popCount; set => popCount = value; }
		public double GrowthRate { get => growthRate; set => growthRate = value; }
		public double GrowthProgress { get => growthProgress; set => growthProgress = value; }
		public int NUnemployed { get => nUnemployed; set => nUnemployed = value; }

		private int popCount;
		private double growthRate;
		private double growthProgress;
		private int nUnemployed;
	
		[Signal]
		public delegate void PopCountChangedEventHandler();
		[Signal]
		public delegate void PopGrewEventHandler();

		public override void _Ready() {
			this.PopCount = 25;
			this.GrowthRate = 0.05;
			this.GrowthProgress = 0.0;
			this.NUnemployed = 25;
		}

		private int ConsumeFood(ResourceManager resManager) {
			// The Pops will consume primary foods based on a priority ranking.
			int nUnfed = this.PopCount;
			foreach (ResourceData foodType in this.primaryFoodChoices) {
				if (resManager.resources.ContainsKey(foodType)) {
					double stores = resManager.resources[foodType];
					double perCapitaNeed = this.primaryFoodDemands[foodType];
					int nFed = Math.Min((int) Math.Floor(stores / perCapitaNeed), nUnfed);
					if (nFed > 0) {
						nUnfed -= nFed;
						resManager.Consume(foodType, nFed * perCapitaNeed);
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
			ResourceManager resManager = this.GetNode<ResourceManager>("/root/ResourceManager");
			foreach (ResourceData foodType in this.primaryFoodChoices) {
				if (resManager.resources.ContainsKey(foodType)) {
					// Convert the food stores into basic food and aggregate
					double stores = resManager.resources[foodType];
					foodStorage += stores / primaryFoodDemands[foodType];
				}
			}
			int nUnfed = this.ConsumeFood(resManager);
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
