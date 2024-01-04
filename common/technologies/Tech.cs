using Godot;
using Godot.Collections;

namespace ProjectArchaetech {
    [GlobalClass]
    public partial class Tech : Resource {
        [Export] public string name;
        [Export] public string desc;
        [Export] public int type;
        [Export] public Texture2D icon;
        [Export] public Array<Tech> prerequisites;
        [Export] public Array<Tech> children;
        [Export] public int cost;
        [Export] public int progress;
        [Export] public int weight;
        [Export] public Array<Variant> rewards;
    }
}