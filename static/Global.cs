using System;
using System.Collections.Generic;
using Godot;
using ProjectArchaetech.common;
using ProjectArchaetech.events;
using static ProjectArchaetech.events.EventBus;

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

		public static GameManager GameManager { get => gameManager; }
		public static ResourceManager ResManager { get => resManager; }
		public static PopManager PopManager { get => popManager; }
		public static BuildingManager BuildManager { get => buildManager; }
		public static EventBus EventBus { get => eventBus; }
		public static TechManager TechTree { get => techTree; }
		public static GameMode GameState { get => gameState; set => gameState = value; }
		public static Queue<Node> Grave { get => grave; set => grave = value; }
		public static Node PickUp { get => pickUp; set => pickUp = value; }
		public static bool GameClockWasPaused { get => gameClockWasPaused; set => gameClockWasPaused = value; }

		// We define the following signals to communicate between GDscript and C#
		[Signal]
		public delegate void NewMonthEventHandler();
		[Signal]
		public delegate void ChangingGameModeEventHandler(int gameModeIdx);
		[Signal]
		public delegate void PickingUpObjEventHandler(Node gameObj);
		[Signal]
		public delegate void AddingBuildingEventHandler(string buildingId);
		[Signal]
		public delegate void CellSelectedEventHandler(Cell cell, TileData tileData);
		[Signal]
		public delegate void DisplayingBuildingUIEventHandler(Building building);
		[Signal]
		public delegate void ModalToggledUIEventHandler(string windowName);
		[Signal]
		public delegate void DeletedGameObjEventHandler(Node node);

		public override void _Ready() {
			// Get static references to the singletons.
			grave = new Queue<Node>();
			gameManager = this.GetNode<GameManager>("GameManager");
			resManager = this.GetNode<ResourceManager>("ResourceManager");
			popManager = this.GetNode<PopManager>("PopManager");
			buildManager = this.GetNode<BuildingManager>("BuildingManager");
			eventBus = this.GetNode<EventBus>("/root/EventBus");
			techTree = this.GetNode<TechManager>("TechManager");

			// Connect events.
			EventBus.Subscribe<NewMonthEvent>((sender, e) => ClearDeadObjects());
			
			this.CellSelected += buildManager.SetCell;
			this.ChangingGameMode += gameModeIdx => GameState = (GameMode) gameModeIdx;
			this.PickingUpObj += gameObj => PickUp = gameObj;
			this.AddingBuilding += buildingId => BuildManager.AddBuilding(buildingId);
			this.DeletedGameObj += node => Grave.Enqueue(node);
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
			foreach (Node obj in Grave) {
				obj.QueueFree();
			}
		}
	}
}
