
using System;
using ProjectArchaetech.common;

namespace ProjectArchaetech.events {
    public class TransportRouteAddedEvent : EventArgs { 
        private readonly TransportRoute route;

        public TransportRouteAddedEvent(TransportRoute route) {
            this.route = route;
        }

        public TransportRoute Route => route;
    }
}