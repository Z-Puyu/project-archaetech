using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectArchaetech.events {
    public class OnAction {
        public string Global { set; get; }
        public List<string> EventId { set; get; }

        public OnAction() {
            this.Global = "";
            this.EventId = new List<string>();
        }
    }
}