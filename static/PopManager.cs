using System;
using System.Collections.Generic;
using C5;
using Godot;
using ProjectArchaetech.common;
using ProjectArchaetech.util.events;
using ProjectArchaetech.resources;
using System.Linq;
using System.Numerics;

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
		private readonly HashDictionary<Competency, HashedLinkedList<Pop>> unemployedAdults;
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
			this.unemployedAdults = new HashDictionary<Competency, HashedLinkedList<Pop>>();
			this.primarySelector = new RandomSelector<ResourceData>();
			this.secondarySelector = new RandomSelector<ResourceData>();
			this.randomPopSelector = new Random(Guid.NewGuid().GetHashCode());
		}

		public override void _Ready() {
			for (Competency c = Competency.Novice; c <= Competency.Expert; c += 1) {
				this.unemployedAdults.Add(c, new HashedLinkedList<Pop>());
			}
			for (int i = 0; i < 25; i += 1) {
				this.unemployedAdults[Competency.Novice].InsertFirst(new Pop(false));
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

		public int CountUnemployed() {
			int n = 0;
			foreach (HashedLinkedList<Pop> list in this.unemployedAdults.Values) {
				n += list.Count;
			}
			return n;
		}

		public int PopCount() {
			return this.CountUnemployed() + this.employedAdults.Count + this.children.Count;
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
					foodType.Use(perCapitaNeed);
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
				foodType.Use(perCapitaNeed);
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
				foodType.Use(perCapitaNeed);
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

		private bool TryFindVeteran(JobData job, out Pop pop) {
			pop = null;
			IEnumerable<double> xp = [];
			IEnumerable<Pop> targets = this.unemployedAdults[Competency.Expert]
				.Where(pop => pop.GetCompetencyOf(job, ref xp)).ToList();
			if (targets.Any()) {
				// Find any expert.
				pop = targets.First();
				this.unemployedAdults[Competency.Expert].Remove(pop);
				return true;
			}
			xp = [];
			targets = this.unemployedAdults[Competency.Regular]
				.Where(pop => pop.GetCompetencyOf(job, ref xp));
			if (this.FindMostExperienced(targets, xp, ref pop)) {
				return true;
			}
			xp = [];
			targets = this.unemployedAdults[Competency.Novice]
				.Where(pop => pop.GetCompetencyOf(job, ref xp));
			if (this.FindMostExperienced(targets, xp, ref pop)) {
				return true;
			}
			return false;
		}

		private bool FindMostExperienced(IEnumerable<Pop> pops, IEnumerable<double> xp, 
			ref Pop pop) {
			if (pops.Any()) {
				// Find a regular with greatest xp.
				double max = double.MinValue;
				int idx = -1;
				foreach (double val in xp) {
					if (val > max) {
						max = val;
					}
					idx += 1;
				}
				pop = pops.ToList()[idx];
				this.unemployedAdults[Competency.Regular].Remove(pop);
				return true;
			}
			return false;
		}

		public List<Pop> PopFindJobs(JobData job, int n) {
			List<Pop> newRecruits = new List<Pop>(n);
			for (int i = 0; i < n; i += 1) {
				if (!this.TryFindVeteran(job, out Pop who)) {
					int idx = this.randomPopSelector.Next(
						this.unemployedAdults[Competency.Novice].Count
					);
					who = this.unemployedAdults[Competency.Novice].RemoveAt(idx);
				}
				who.AcquireJob(job);
				this.employedAdults.Add(who);
				newRecruits.Add(who);
				if (this.CountUnemployed() == 0) {
					break;
				}
			}
			this.EmitSignal(SignalName.PopCountUpdated, this.PopCount(), this.CountUnemployed());
			return newRecruits;
		}

		public List<Pop> RecruitPops(int n) {
			List<Pop> recruits = new List<Pop>(n);
			if (this.CountUnemployed() >= n) {
				foreach (Competency c in this.unemployedAdults.Keys) {
					while (n > 0 && !this.unemployedAdults[c].IsEmpty) {
						recruits.Add(this.unemployedAdults[c].Remove());
						n -= 1;
					}
				}
			}
			return recruits;
		}

		public void PopGrow() {
			this.children.Add(new Pop(true));
		}
	}
}
