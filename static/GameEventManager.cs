using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using C5;
using Godot;
using ProjectArchaetech.common.util;
using ProjectArchaetech.events;

namespace ProjectArchaetech {
    [GlobalClass]
    public partial class GameEventManager : Node {
        private readonly HashDictionary<string, GameEvent> events;
        private readonly HashDictionary<string, RandomPool<GameEvent>> pool;

        public GameEventManager() {
            this.events = new HashDictionary<string, GameEvent>();
            this.pool = new HashDictionary<string, RandomPool<GameEvent>>();
        }

        private void PopulatePool() {
            string onActionPath = "events/on-actions/on-actions.json";
            string onActionData = File.ReadAllText(onActionPath);
            Dictionary<string, List<string>> onActions = JsonSerializer
                .Deserialize<Dictionary<string, List<string>>>(onActionData);
            foreach (string pulse in onActions.Keys) {
                RandomPool<GameEvent> s = new RandomPool<GameEvent>(null, null);
                foreach (string id in onActions[pulse]) {
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
                this.pool[pulse] = s;
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
        }

        private void PollEvent(string pulse) {
            GameEvent e = this.pool[pulse].Poll();
            if (e != default) {
                e.Fire();
            }
        }
    }
}