using C5;
using Godot;
using Godot.Collections;
using ProjectArchaetech.events;
using System;
using System.Collections.Generic;
using static ProjectArchaetech.events.EventBus;

namespace ProjectArchaetech {
	[GlobalClass]
	public partial class TechManager : Node {
		[Export]
		private Array<Tech> allTechnologies;
		private RandomSelector<Tech> selector;
		private C5.HashSet<Tech> researchable;
		private C5.HashSet<Tech> unlocked;
		private C5.HashSet<Tech> focus;
		private RandomSelector<Tech> focusSelector;
		private enum TechType {
			Sci,
			Engin,
			Human
		}

		public RandomSelector<Tech> Selector { get => selector; set => selector = value; }
		public C5.HashSet<Tech> Researchable { get => researchable; set => researchable = value; }
		public C5.HashSet<Tech> Unlocked { get => unlocked; set => unlocked = value; }
		public C5.HashSet<Tech> Focus { get => focus; set => focus = value; }
		public RandomSelector<Tech> FocusSelector { get => focusSelector; set => focusSelector = value; }
		public Array<Tech> AllTechnologies { get => allTechnologies; set => allTechnologies = value; }

		public override void _Ready() {
			this.Researchable = new C5.HashSet<Tech>();
			foreach (Tech tech in this.AllTechnologies) {
				this.Researchable.Add(tech);
			}
			this.Unlocked = new C5.HashSet<Tech>();
			this.Focus = new C5.HashSet<Tech>();
			this.Selector = new RandomSelector<Tech>();
			this.FocusSelector = new RandomSelector<Tech>();
			foreach (Tech tech in this.Researchable) {
				this.Selector.Add(new C5.KeyValuePair<int, Tech>(tech.weight, tech));
			}
			this.GetNode<EventBus>("/root/EventBus").Subscribe<TechProgressEvent>((sender, e) => 
				this.Research(((TechProgressEvent) e).AvailablePoints));
		}

		public void Unlock(Tech tech) {
			if (!this.Researchable.Contains(tech)) {
				GD.PushError("Nonexistent Tech!");
				GetTree().Quit();
			} else {
				this.Unlocked.Add(tech);
				this.Researchable.Remove(tech);
				this.Selector.Remove(tech);
				if (this.focus.Remove(tech)) {
					this.FocusSelector.Remove(tech);
				}
				foreach (Tech newTech in tech.children) {
					if (this.Unlocked.ContainsAll(newTech.prerequisites)) {
						this.Researchable.Add(newTech);
						this.Selector.Add(new C5.KeyValuePair<int, Tech>(newTech.weight, newTech));
					}
				}
			}
		}

		private void Research(int availablePoints) {
			HashDictionary<Tech, int> progress = new HashDictionary<Tech, int>();
			if (this.researchable.IsEmpty) {
				return;
			}
			if (!this.Focus.IsEmpty) {
				int focusPoints = (int) Math.Floor(0.33 * availablePoints);
				for (int i = 0; i < focusPoints; i += 1) {
					Tech target = this.FocusSelector.Select();
					if (progress.Contains(target)) {
						progress[target] += 1;
					} else {
						progress[target] = 1;
					}
				}
				availablePoints -= focusPoints;
			}
			for (int i = 0; i < availablePoints; i += 1) {
				Tech target = this.Selector.Select();
				if (progress.Contains(target)) {
					progress[target] += 1;
				} else {
					progress[target] = 1;
				}
			}
			List<Tech> unlocking = new List<Tech>();
			foreach (Tech tech in this.Researchable) {
				tech.progress = Math.Min(tech.progress + progress[tech], tech.cost);
				Console.WriteLine(tech.name + " gets " + Math.Min(progress[tech], tech.cost - tech.progress + progress[tech]) + " progress");
				if (tech.progress == tech.cost) {
					unlocking.Add(tech);
				}
			}
			foreach (Tech tech in unlocking) {
				this.Unlock(tech);
				Console.WriteLine("unlocked " + tech.name + "!");
			}
		}
	}
}
