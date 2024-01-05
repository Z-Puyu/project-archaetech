using System.Collections.Generic;
using Godot;

namespace ProjectArchaetech {
	public partial class Global : Node {
		public enum GameMode {
			Normal,
			Paused,
			Build,
			BuildRoute
		}

		private static Queue<Node> grave;
		private static GameManager gameManager;
		private static ResourceManager resManager;
		private static PopManager popManager;
		private static BuildingManager buildManager;
		private static Events eventBus;
		private static TechManager techTree;
		private static GameMode gameState;
		private static Node pickUp;

		public static GameManager GameManager { get => gameManager; }
		public static ResourceManager ResManager { get => resManager; }
		public static PopManager PopManager { get => popManager; }
		public static BuildingManager BuildManager { get => buildManager; }
		public static Events EventBus { get => eventBus; }
		public static TechManager TechTree { get => techTree; }
		public static GameMode GameState { get => gameState; set => gameState = value; }
		public static Queue<Node> Grave { get => grave; set => grave = value; }
		public static Node PickUp { get => pickUp; set => pickUp = value; }

		public override void _Ready() {
			// Get static references to the singletons.
			grave = new Queue<Node>();
			gameManager = this.GetNode<GameManager>("GameManager");
			resManager = this.GetNode<ResourceManager>("ResourceManager");
			popManager = this.GetNode<PopManager>("PopManager");
			buildManager = this.GetNode<BuildingManager>("BuildingManager");
			eventBus = this.GetNode<Events>("Events");
			techTree = this.GetNode<TechManager>("TechManager");

			// Connect events.
			resManager.TechProgress += techTree.Research;
			eventBus.CellSelected += buildManager.SetCell;
			gameManager.NewMonth += ClearDeadObjects;
		}

		public bool IsGamePaused() => GameState == GameMode.Paused;

		public void PauseGame() {
			GameManager.GameClock.Paused = true;
			GameState = GameMode.Paused;
		}

		public void ResumeGame() {
			GameManager.GameClock.Paused = false;
			GameState = GameMode.Normal;
		}

		public static void ClearDeadObjects() {
			foreach (Node obj in Grave) {
				obj.QueueFree();
			}
		}
	}
}
