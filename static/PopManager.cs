using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using C5;
using Godot;
using Godot.Collections;
using ProjectArchaetech.common;
using ProjectArchaetech.events;
using ProjectArchaetech.resources;

namespace ProjectArchaetech {
	[GlobalClass]
	public partial class PopManager : Node {
		const int MEAN_SOL = 5;
		const int MAX_EFFECTIVE_SOL = 10;
		const double EXTRA_SOL_IMPACT = 1.25;
		const double CHILDREN_FOOD_MODIFIER = 0.5;
		const double SOL_THRESHOLD = 1.25;
		// These define how much one Pop consumes per month 
		// for each of the edible resources.
		[Export]
		public Godot.Collections.Dictionary<ResourceData, double> PrimaryFoodDemands { set; get; }
		[Export] // Currently not used but will be
		public Godot.Collections.Dictionary<ResourceData, double> SecondaryFoodDemands { set; get; } 
		[Export]
		public Godot.Collections.Dictionary<ResourceData, int> PrimaryFoodChoices { set; get; } // [resource, weight]

		private readonly C5.HashSet<Pop> employedAdults;
		private readonly C5.HashSet<Pop> children;
		private readonly HashedLinkedList<Pop> unemployedAdults;
		private double growthRate;
		private double growthProgress;
		private double sol;
		private readonly RandomSelector<ResourceData> primarySelector; // Weighted
		private readonly RandomSelector<ResourceData> secondarySelector; // Unweighted
		private readonly Random randomPopSelector;

		public double GrowthRate { get => growthRate; set => growthRate = value; }
		public double GrowthProgress { get => growthProgress; set => growthProgress = value; }
	
		[Signal]
		public delegate void PopGrewEventHandler();
		[Signal]
		public delegate void PopCountUpdatedEventHandler(int labour, int total);

		public PopManager() {
			this.PrimaryFoodDemands = new Godot.Collections.Dictionary<ResourceData, double>();
			this.SecondaryFoodDemands = new Godot.Collections.Dictionary<ResourceData, double>();
			this.PrimaryFoodChoices = new Godot.Collections.Dictionary<ResourceData, int>();
			this.employedAdults = new C5.HashSet<Pop>();
			this.children = new C5.HashSet<Pop>();
			this.unemployedAdults = new HashedLinkedList<Pop>();
			this.primarySelector = new RandomSelector<ResourceData>();
			this.secondarySelector = new RandomSelector<ResourceData>();
			this.randomPopSelector = new Random(Guid.NewGuid().GetHashCode());
		}

		public override void _Ready() {
			for (int i = 0; i < 25; i += 1) {
				this.unemployedAdults.InsertFirst(new Pop(false));
			}
			for (int i = 0; i < 10; i += 1) {
				this.children.Add(new Pop(true));
			}
			this.GrowthRate = 0.05;
			this.GrowthProgress = 0.0;
			this.sol = 5;
			Global.EventBus.Subscribe<ProcessingPopsEvent>((sender, e) => this.Update());
			foreach (ResourceData res in this.SecondaryFoodDemands.Keys) {
				this.secondarySelector.Add(new C5.KeyValuePair<int, ResourceData>(1, res));
			}
			foreach (ResourceData res in this.PrimaryFoodChoices.Keys) {
				this.primarySelector.Add(new C5.KeyValuePair<int, ResourceData>(this.PrimaryFoodChoices[res], res));
			}
		}

		public int PopCount() {
			return this.unemployedAdults.Count + this.employedAdults.Count + this.children.Count;
		}

		private int ConsumeFood() {
			// The Pops will consume primary foods based on a priority ranking.
			int nUnfedAdults = this.unemployedAdults.Count + this.employedAdults.Count;
			int nUnfedChildren = this.children.Count;

			double solModifier = 1;
			double solSecondaryModifier = 1;
			if (sol > MEAN_SOL) {
				double diff = sol - MEAN_SOL;
				solModifier = Math.Sqrt(diff / (MAX_EFFECTIVE_SOL - MEAN_SOL)) * 10;
			} else if (sol < MEAN_SOL) {
				double diff = MEAN_SOL - sol;
				solModifier = Math.Sqrt(diff / MEAN_SOL) * 10;
			}
			if (sol > MEAN_SOL * SOL_THRESHOLD) {
				double diff = sol - MEAN_SOL * SOL_THRESHOLD;
				solSecondaryModifier = Math.Sqrt(diff / ((MAX_EFFECTIVE_SOL - MEAN_SOL) * SOL_THRESHOLD)) * 10;
			} else if (sol < MEAN_SOL * SOL_THRESHOLD) {
				double diff = sol - MEAN_SOL * SOL_THRESHOLD;
				solSecondaryModifier = Math.Sqrt(diff / (MEAN_SOL * SOL_THRESHOLD)) * 10;
			}

			C5.HashSet<ResourceData> invalidFoods = new C5.HashSet<ResourceData>();
			if (this.AdultsConsumeFood(nUnfedAdults, solModifier, invalidFoods) 
				&& !this.ChildrenConsumeFood(nUnfedChildren, solModifier, invalidFoods)) {
				this.ConsumeExtra(solSecondaryModifier);
			}

			foreach (ResourceData res in invalidFoods) {
				this.primarySelector.Add(new C5.KeyValuePair<int, ResourceData>(this.PrimaryFoodChoices[res], res));
			}
			
			return nUnfedAdults + nUnfedChildren;
		}

		private void ConsumeExtra(double solSecondaryModifier) {
			if (this.sol > MEAN_SOL) {
				int nUnsatisfied = 0;
				int popCount = this.PopCount();
				double modifier = solSecondaryModifier * (popCount - 0.5 * this.children.Count) 
					/ popCount * EXTRA_SOL_IMPACT;
				for (int i = 0; i < popCount; i += 1) {
					ResourceData foodType = this.secondarySelector.Select();
					double perCapitaNeed = this.SecondaryFoodDemands[foodType] * modifier;
					if (!Global.ResManager.HasEnough(foodType, perCapitaNeed)) {
						nUnsatisfied += 1;
						this.secondarySelector.Remove(foodType);
						if (this.secondarySelector.IsEmpty()) {
							return;
						}
						continue;
					}
					Global.ResManager.Consume(foodType, perCapitaNeed);
				}
				// The higher the SOL, the higher the expectation :O
				this.sol -= Math.Clamp((this.sol - MEAN_SOL) * 0.0005 * nUnsatisfied, 0, 0.2);
			} else {
				this.sol += Math.Clamp((MEAN_SOL - this.sol) * 0.05, 0.02, 0.1); // The SOL grows gradually
			}
		}

		private bool AdultsConsumeFood(int nUnfedAdults, double solModifier, 
			C5.HashSet<ResourceData> invalidFoods) {
			while (nUnfedAdults > 0) {
				ResourceData foodType = this.primarySelector.Select();
				double perCapitaNeed = this.PrimaryFoodDemands[foodType] * solModifier;
				if (!Global.ResManager.HasEnough(foodType, perCapitaNeed)) {
					if (!invalidFoods.Contains(foodType)) {
						invalidFoods.Add(foodType);
						this.primarySelector.Remove(foodType);
						this.secondarySelector.Remove(foodType);
						if (invalidFoods.Count == this.PrimaryFoodChoices.Count) {
							return false;
						}
					}
					continue;
				}
				Global.ResManager.Consume(foodType, perCapitaNeed);
				nUnfedAdults -= 1;
			}
			return true;
		}

		private bool ChildrenConsumeFood(int nUnfedChildren, double solModifier, 
			C5.HashSet<ResourceData> invalidFoods) {
			while (nUnfedChildren > 0) {
				ResourceData foodType = this.primarySelector.Select();
				double perCapitaNeed = this.PrimaryFoodDemands[foodType]
					* solModifier * CHILDREN_FOOD_MODIFIER;
				if (!Global.ResManager.HasEnough(foodType, perCapitaNeed)) {
					if (!invalidFoods.Contains(foodType)) {
						invalidFoods.Add(foodType);
						this.primarySelector.Remove(foodType);
						this.secondarySelector.Remove(foodType);
						if (invalidFoods.Count == this.PrimaryFoodChoices.Count) {
							return false;
						}
					}
					continue;
				}
				Global.ResManager.Consume(foodType, perCapitaNeed);
				nUnfedChildren -= 1;
			}
			return true;
		}

		public void Update() {
			// We first compute how much food is needed if all Pops only consume basic food
			int yearlyFood = this.PopCount() * 12;
			double foodStorage = 0.0;
			foreach (ResourceData foodType in this.PrimaryFoodChoices.Keys) {
				if (Global.ResManager.Resources.ContainsKey(foodType)) {
					// Convert the food stores into basic food and aggregate
					double stores = Global.ResManager.Resources[foodType];
					foodStorage += stores / PrimaryFoodDemands[foodType];
				}
			}
			int nUnfed = this.ConsumeFood();
			double growthModifier = 1.0;
			if (nUnfed != 0) {
				double starvation = nUnfed / this.PopCount();
				growthModifier = -Math.Sqrt(starvation);
			} else {
				growthModifier += Math.Clamp(foodStorage / (yearlyFood * 3) - 1, 0, 0.5);
			}
			this.GrowthProgress += this.GrowthRate * growthModifier;
			int realGrowth = (int) Math.Floor(this.GrowthProgress);
			while (realGrowth > 0) {
				this.PopGrow();	
				realGrowth -= 1;
				this.GrowthProgress -= 1;
			}
		}

		public List<Pop> PopFindJobs(JobData job, int n) {
			List<Pop> newRecruits = new List<Pop>(n);
			for (int i = 0; i < n; i += 1) {
				int which = this.randomPopSelector.Next(this.unemployedAdults.Count);
				Pop who = this.unemployedAdults.RemoveAt(which);
				who.acquireJob(job);
				this.employedAdults.Add(who);
				newRecruits.Add(who);
				if (this.unemployedAdults.Count == 0) {
					break;
				}
			}
			this.EmitSignal(SignalName.PopCountUpdated, this.PopCount(), this.unemployedAdults.Count);
			return newRecruits;
		}

		public int GetUnemployment() {
			return this.unemployedAdults.Count;
		}

		public void PopGrow() {
			this.children.Add(new Pop(true));
		}
	}
}
