using System;

namespace ProjectArchaetech.util.events {
    public class TechProgressedEvent : EventArgs {
			private readonly int availablePoints;

			public int AvailablePoints => availablePoints;

			public TechProgressedEvent(int availablePoints) {
				this.availablePoints = availablePoints;
			}
		}
}