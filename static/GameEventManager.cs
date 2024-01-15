using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using C5;
using Godot;
using ProjectArchaetech.common.util;
using ProjectArchaetech.events;
using ProjectArchaetech.util.events;

namespace ProjectArchaetech {
    [GlobalClass]
    public partial class GameEventManager : Node {
        private const string EVENT_NAMESPACE = "ProjectArchaetech.util.events";
        private readonly HashDictionary<string, GameEvent> events;
        private readonly HashDictionary<Type, RandomPool<GameEvent>> pool;

        public GameEventManager() {
            this.events = new HashDictionary<string, GameEvent>();
            this.pool = new HashDictionary<Type, RandomPool<GameEvent>>();
        }

        private void PopulatePool() {
            string onActionPath = "events/on-actions/on-actions.json";
            string onActionData = File.ReadAllText(onActionPath);
            Dictionary<string, OnAction> onActions = JsonSerializer
                .Deserialize<Dictionary<string, OnAction>>(onActionData);
            foreach (string pulse in onActions.Keys) {
                string eventName = onActions[pulse].Global;
                Type eventType = Type.GetType($"{EVENT_NAMESPACE}.{eventName}");
                RandomPool<GameEvent> s = new RandomPool<GameEvent>(null, null);
                foreach (string id in onActions[pulse].EventId) {
                    if (this.events.Contains(id)) {
                        GameEvent e = this.events[id];
                        if (e is RandomGameEvent) {
                            s.Add(((RandomGameEvent) e).Factor, e);
                        } else {
                            GD.PushError("Event with ID " + id + " is not a random event!");
                            this.GetTree().Quit();
                        }
                    } else {
                        GD.PushError("Event with ID " + id + " does not exist!");
                    }
                }
                this.pool[eventType] = s;
            }
        }

        public override void _Ready() {
            string eventDataPath = "events/data/events.json";
            string jsonString = File.ReadAllText(eventDataPath);
            List<GameEvent> events = JsonSerializer.Deserialize<List<GameEvent>>(jsonString);
            foreach (GameEvent e in events) {
                if (this.events.Contains(e.GetId())) {
                    GD.PushError("Duplicate event ID " + e.GetId() + "!");
                    this.GetTree().Quit();
                    break;
                }
                this.events[e.GetId()] = e;
            }
            this.PopulatePool();

            HashDictionary<string, int> testData = new HashDictionary<string, int>();
            testData["No Event"] = 0;
            for (int i = 0; i < 10000; i += 1) {
                GameEvent e = this.pool[typeof(NewDayEvent)].Poll();
                if (e == null) {
                    testData["No Event"] += 1;
                    continue;
                }
                if (testData.Contains(e.GetId())) {
                    testData[e.GetId()] += 1;
                } else {
                    testData[e.GetId()] = 1;
                }
            }
            Global.EventBus.Subscribe<NewMonthEvent>((sender, e) => this.PollEvent(e));
        }

        private void PollEvent(EventArgs globalEvent) {
            GameEvent e = this.pool[globalEvent.GetType()].Poll();
            if (e != default) {
                e.Fire();
            }
        }
    }
}