using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace ProjectArchaetech.events {
    [RegisteredType(nameof(Tech), "", nameof(Resource)), GlobalClass]
    public abstract partial class EventData : Resource {
        [Export]
        public string Namespace { set; get; }
        [Export]
        public int Index { set; get; }
        [Export]
        public EventType Type { set; get; }
        [Export]
        public string Title { set; get; }
        [Export]
        public string Desc { set; get; }
        [Export]
        public int MeanTimeToHappen { set; get; }
        [Export]
        public Array<Option> Options { set; get; }

        public EventData() {
            this.Namespace = "new-namespace";
            this.Index = 0;
            this.Type = EventType.Random;
            this.Title = "Event Name";
            this.Desc = "Event Description";
            this.MeanTimeToHappen = -1;
            this.Options = new Array<Option>();
        }

        public EventData(string @namespace, int index, EventType eventType, string title, string desc, int mtth, params Option[] options) {
            this.Namespace = @namespace;
            this.Index = index;
            this.Type = eventType;
            this.Title = title;
            this.Desc = desc;
            this.MeanTimeToHappen = mtth;
            this.Options = [.. options];
        }
    }
}