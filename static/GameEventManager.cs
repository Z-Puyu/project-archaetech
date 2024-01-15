using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private readonly HashDictionary<string, CountDown> scheduled;

        public GameEventManager() {
            this.events = new HashDictionary<string, GameEvent>();
            this.pool = new HashDictionary<Type, RandomPool<GameEvent>>();
            this.scheduled = new HashDictionary<string, CountDown>();
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

            // Random Events
            Global.EventBus.Subscribe<NewDayEvent>((sender, e) => this.PollEvent(e));
            Global.EventBus.Subscribe<NewWeekEvent>((sender, e) => this.PollEvent(e));
            Global.EventBus.Subscribe<NewFortnightEvent>((sender, e) => this.PollEvent(e));
            Global.EventBus.Subscribe<NewMonthEvent>((sender, e) => this.PollEvent(e));
            Global.EventBus.Subscribe<NewBiMonthEvent>((sender, e) => this.PollEvent(e));
            Global.EventBus.Subscribe<NewQuarterEvent>((sender, e) => this.PollEvent(e));
            Global.EventBus.Subscribe<NewHalfYearEvent>((sender, e) => this.PollEvent(e));
            Global.EventBus.Subscribe<NewYearEvent>((sender, e) => this.PollEvent(e));
        
            // On-Action Events
            Global.EventBus.Subscribe<TechUnlockedEvent>((sender, e) => this.FireEvent(e));
        }

        private void PollEvent(EventArgs globalEvent) {
            GameEvent e = default;
            C5.HashSet<RandomGameEvent> polledEvents = new C5.HashSet<RandomGameEvent>();
            RandomPool<GameEvent> pool = this.pool[globalEvent.GetType()];
            while (!polledEvents.Contains(e)) {
                e = pool.Poll();
                if (e == default || (e.IsValid() && e.CanFire())) {
                    if (e is not RandomGameEvent) {
                        GD.PushError($"{e.GetId()} is not a random event!");
                        return;
                    }
                    break;
                }
                if (e is not RandomGameEvent) {
                    GD.PushError($"{e.GetId()} is not a random event!");
                    return;
                }
                // e != default
                pool.Pop(e);
                polledEvents.Add((RandomGameEvent) e);
            }

            foreach (RandomGameEvent @event in polledEvents) {
                pool.Add(@event.Factor, @event);
                polledEvents.Remove(@event);
            }

            if (e != default) {
                string id = e.GetId();
                CountDown scheduledEvent = ((RandomGameEvent) e)
                    .Schedule(() => this.scheduled.Remove(id));
                this.scheduled.Add(id, scheduledEvent);
            }
        }

        private void FireEvent(EventArgs e) {
            C5.HashSet<GameEvent> events = this.pool[e.GetType()].Items;
            foreach (GameEvent @event in events) {
                if (e is events.@abstract.GameEvent) {
                    events.@abstract.GameEvent data = (events.@abstract.GameEvent) e;
                    @event.Customise(data.CustomTitle, data.CustomDesc);
                }
                if (@event.IsValid() && @event.CanFire()) {
                    if (@event.Mtth <= 0) {
                        @event.Fire();
                    } else {
                        string id = @event.GetId();
                        CountDown scheduledEvent = @event.Schedule(() => this.scheduled.Remove(id));
                        this.scheduled.Add(id, scheduledEvent);
                    }
                }
                
            }
        }
    }
}