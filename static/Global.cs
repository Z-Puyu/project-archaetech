using System.Collections.Generic;
using Godot;
using ProjectArchaetech.common;
using ProjectArchaetech.util.events;

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
		private static EventBus eventBus;
		private static TechManager techTree;
		private static GameMode gameState;
		private static Node pickUp;
		private static bool gameClockWasPaused;

        public static GameManager GameManager => gameManager;
        public static ResourceManager ResManager => resManager;
        public static PopManager PopManager => popManager;
        public static BuildingManager BuildManager => buildManager;
        public static EventBus EventBus => eventBus;
        public static TechManager TechTree => techTree;
        public static GameMode GameState { get => gameState; set => gameState = value; }
		public static Queue<Node> Grave { get => grave; set => grave = value; }
		public static Node PickUp { get => pickUp; set => pickUp = value; }
		public static bool GameClockWasPaused { get => gameClockWasPaused; set => gameClockWasPaused = value; }

		// We define the following signals to communicate between GDscript and C#
		// Modal-related
		[Signal]
		public delegate void OpeningModalUIEventHandler(string windowName, Godot.Collections.Dictionary<string, Variant> optionalArgs);
		[Signal]
		public delegate void ClosingModalUIEventHandler();
		[Signal]
		public delegate void OpeningBuildingMenuEventHandler();
		[Signal]
		public delegate void TransportRouteAddedEventHandler(TransportRoute route);
		[Signal]
		public delegate void TransportRouteRemovedEventHandler(TransportRoute route);

		// GameMode-related
		[Signal]
		public delegate void EnteringRouteConstructionModeEventHandler();
		[Signal]
		public delegate void RestoringNormalModeEventHandler();

		[Signal]
		public delegate void NewMonthEventHandler();
		[Signal]
		public delegate void PickingUpObjEventHandler(Node gameObj);
		[Signal]
		public delegate void AddingBuildingEventHandler(string buildingId);
		[Signal]
		public delegate void CellSelectedEventHandler(Cell cell, TileData tileData);
		
		[Signal]
		public delegate void DeletedGameObjEventHandler(Node node);

		public Global() {
			eventBus = new EventBus();
		}

		public override void _Ready() {
			// Get static references to the singletons.
			this.AddChild(eventBus);
			grave = new Queue<Node>();
			gameManager = this.GetNode<GameManager>("GameManager");
			resManager = this.GetNode<ResourceManager>("ResourceManager");
			popManager = this.GetNode<PopManager>("PopManager");
			buildManager = this.GetNode<BuildingManager>("BuildingManager");
			techTree = this.GetNode<TechManager>("TechManager");

			// Connect events.
			EventBus.Subscribe<NewMonthEvent>((sender, e) => ClearDeadObjects());
			EventBus.Subscribe<TransportRouteAddedEvent>((sender, e) => this.EmitSignal(
				SignalName.TransportRouteAdded, ((TransportRouteAddedEvent) e).Route
			));
			
			this.CellSelected += (cell, tileData) => BuildManager.SetCell(cell, tileData);
			this.PickingUpObj += gameObj => PickUp = gameObj;
			this.AddingBuilding += BuildManager.AddBuilding;
			this.DeletedGameObj += Grave.Enqueue;
			this.OpeningBuildingMenu += OnOpeningConstructionMenu;
			this.EnteringRouteConstructionMode += () => GameState = GameMode.BuildRoute;
			this.RestoringNormalMode += () => GameState = GameMode.Normal;
			this.TransportRouteRemoved += route => EventBus
				.Publish(this, new TransportRouteRemovedEvent(route));
		}

		private static void OnOpeningConstructionMenu() {
			if (PickUp is Cell) {
				GameState = GameMode.Build;
			}
		}
		public bool IsGamePaused() => GameState == GameMode.Paused;

		public void PauseGame() {
			GameClockWasPaused = GameManager.GameClock.Paused;
			GameManager.GameClock.Paused = true;
			GameState = GameMode.Paused;
		}

		public void ResumeGame() {
			GameManager.GameClock.Paused = GameManager.GameClock.Paused;
			GameState = GameMode.Normal;
		}
		
		public void PauseTime() {
			GameManager.GameClock.Paused = true;
		}
		
		public void ResumeTime() {
			GameManager.GameClock.Paused = false;
		}
		
		public void SpeedUpGame() {
			GameManager.GameClock.SpeedUp();
		}
		
		public void SlowDownGame() {
			GameManager.GameClock.SlowDown();
		}

		public static void ClearDeadObjects() {
			while (Grave.TryDequeue(out Node obj)) {
				obj.QueueFree();
			}
		}

		// The following methods should only be used by GDScript classes.
		public Node GetPickUp() {
			return PickUp;
		}

		public bool IsBuildingTransportRoutes() {
			return GameState == GameMode.BuildRoute;
		}
	}
}
