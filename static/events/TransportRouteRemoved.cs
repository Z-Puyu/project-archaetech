
using System;
using ProjectArchaetech.common;

namespace ProjectArchaetech.util.events {
    public class TransportRouteRemovedEvent : EventArgs { 
        private readonly TransportRoute route;

        public TransportRouteRemovedEvent(TransportRoute route) {
            this.route = route;
        }

        public TransportRoute Route => route;
    }
}