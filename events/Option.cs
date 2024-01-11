using System;
using Godot;

namespace ProjectArchaetech.events {
    [GlobalClass]
    public partial class Option : Node {
        [Export]
        public string Id { set; get; }
        [Export]
        public string Desc { set; get; }
        private readonly Predicate<object> isVisible;
        private readonly Action effect;

        public Option(Predicate<object> isVisible, Action effect) {
            this.Id = "";
            this.Desc = "";
            this.effect = effect;
            this.isVisible = isVisible;
        }

        //public bool isValid() {
        //    return this.isVisible()
        //}

        public void OnSelect() {
            this.effect.Invoke();
        }
    }
}