using System;
using ProjectArchaetech.events;
using ProjectArchaetech.util.events;

namespace ProjectArchaetech.common.util {
    public class CountDown { 
        private int delayTime;
        private readonly Action onTriggered;
        private readonly Action onDestructed;
        
        public CountDown(int delayTime, Action onTriggered, Action onDestructed) {
            this.delayTime = delayTime;
            this.onTriggered = onTriggered;
            this.onDestructed = onDestructed;
            Global.EventBus.Subscribe<NewDayEvent>(this.Progress);
        }

        private void Progress(object sender, EventArgs e) {
            this.delayTime -= 1;
            if (this.delayTime == 0) {
                this.onTriggered.Invoke();
                Global.EventBus.Unsubscribe<NewDayEvent>(this.Progress);
                this.onDestructed.Invoke();
            }
        }
    }
}