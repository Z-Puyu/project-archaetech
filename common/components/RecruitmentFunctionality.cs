using System;
using System.Collections.Generic;
using Godot.Collections;
using ProjectArchaetech.interfaces;
using ProjectArchaetech.resources;
using ProjectArchaetech.util;

namespace ProjectArchaetech.common.components {
    public class RecruitmentFunctionality : IFunctionable {
        private readonly Godot.Collections.Dictionary<JobData, int> employmentCap;
		private readonly Godot.Collections.Dictionary<JobData, 
            Godot.Collections.Dictionary<int, Array<Pop>>> workers;

        public RecruitmentFunctionality(Godot.Collections.Dictionary<JobData, int> employmentCap, 
            Godot.Collections.Dictionary<JobData, 
            Godot.Collections.Dictionary<int, Array<Pop>>> workers) {
            this.employmentCap = employmentCap;
            this.workers = workers;
        }

        public event IFunctionable.ObjectUIDataUpdatedEventHandler ObjectUIDataUpdatedEvent;

        public void Execute() {
            foreach (JobData job in this.employmentCap.Keys) {
				if (!this.workers.ContainsKey(job)) {
				    Godot.Collections.Dictionary<int, Array<Pop>> workersList = 
                        new Godot.Collections.Dictionary<int, Array<Pop>>();
                    for (int i = 0; i < 3; i += 1) {
                        workersList[i] = new Array<Pop>();
                    }
                    workers[job] = workersList;
			    } 
                int currNWorkers = 0;
                for (Competency competency = Competency.Novice; competency <= Competency.Expert; 
                    competency += 1) {
                    currNWorkers += this.workers[job][(int) competency].Count;
                }
                if (currNWorkers < this.employmentCap[job] && 
                    Global.PopManager.CountUnemployed() > 0) {
                    int shortage = this.employmentCap[job] - currNWorkers;
                    int nRecruited = Math.Min(shortage, Global.PopManager.CountUnemployed());
                    List<Pop> newRecruits = Global.PopManager.PopFindJobs(job, nRecruited);
                    foreach (Pop pop in newRecruits) {
                        if (pop.GetCompetencyOf(job, out Competency c)) {
                             this.workers[job][(int) c].Add(pop);
                        }
                    }
                }
			}
            this.ObjectUIDataUpdatedEvent.Invoke("employment", new Pair(workers, employmentCap));
            //updatedUIData["employment"] = new Pair(this.workers, this.employmentCap);
        }
    }
}