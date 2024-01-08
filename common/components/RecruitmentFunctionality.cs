using System;
using Godot;
using Godot.Collections;
using ProjectArchaetech.interfaces;
using ProjectArchaetech.resources;
using ProjectArchaetech.util;

namespace ProjectArchaetech.common.components {
    public class RecruitmentFunctionality : IFunctionable {
        private readonly Dictionary<JobData, int> employmentCap;
		private readonly Dictionary<JobData, int> workers;

        public RecruitmentFunctionality(Dictionary<JobData, int> employmentCap, 
            Dictionary<JobData, int> workers) {
            this.employmentCap = employmentCap;
            this.workers = workers;
        }

        public void Execute(Dictionary<string, Variant> updatedUIData) {
            foreach (JobData job in this.employmentCap.Keys) {
				if (!this.workers.ContainsKey(job)) {
				    this.workers[job] = 0;
			    } 
                if (this.workers[job] < this.employmentCap[job] && 
                    Global.PopManager.NUnemployed > 0) {
                    int shortage = this.employmentCap[job] - this.workers[job];
                    int nRecruited = Math.Min(shortage, Global.PopManager.NUnemployed);
                    this.workers[job] += nRecruited;
                    Global.PopManager.NUnemployed -= nRecruited;
                }
			}
            updatedUIData["employment"] = new Pair(this.workers, this.employmentCap);
        }
    }
}