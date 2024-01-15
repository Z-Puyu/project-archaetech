using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectArchaetech.events.@abstract {
    public abstract class GameEvent : EventArgs {
        private string customTitle;
        private string customDesc;

        public GameEvent(string customTitle, string customDesc) {
            this.customTitle = customTitle;
            this.customDesc = customDesc;
        }

        public string CustomTitle { get => customTitle; set => customTitle = value; }
        public string CustomDesc { get => customDesc; set => customDesc = value; }
    }
}