using System;
using System.Collections.Generic;
using C5;
using Godot;
using ProjectArchaetech.resources;

namespace ProjectArchaetech.common {
    public partial class Pop : Node {
        private bool isChild;
        private bool isFemale;
        private readonly Queue<JobData> workedJobs;
        private readonly HashDictionary<JobData, ValueTuple<Competency, double>> xp;

        public bool IsChild { get => isChild; set => isChild = value; }
        public bool IsFemale { get => isFemale; set => isFemale = value; }

        public Pop(bool isChild, bool isFemale, JobData job) {
            this.isChild = isChild;
            this.isFemale = isFemale;
            this.workedJobs = new Queue<JobData>(3);
            this.xp = new HashDictionary<JobData, (Competency, double)>();
        }

        public Pop() {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            this.isChild = rand.Next() % 2 == 1 ? true : false;
            this.isFemale = rand.Next() % 2 == 1 ? true : false;
            this.workedJobs = new Queue<JobData>(3);
            this.xp = new HashDictionary<JobData, (Competency, double)>();
        }

        public Pop(bool isChild) {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            this.isChild = isChild;
            this.isFemale = rand.Next() % 2 == 1 ? true : false;
            this.workedJobs = new Queue<JobData>(3);
            this.xp = new HashDictionary<JobData, (Competency, double)>();
        }

        public void GainXp(JobData job, double amount) {
            if (!this.xp.Contains(job)) {
                GD.PushError("No valid experience data found for " + job + "!");
                return;
            }
            Competency currCompetency = this.xp[job].Item1;
            if (currCompetency != Competency.Expert) {
                double currXp = this.xp[job].Item2;
                currXp += amount;
                if (currXp >= 100) {
                    this.xp.Update(job, (currCompetency + 1, 0));
                }
            }
        }

        public void acquireJob(JobData job) {
            if (!this.xp.Contains(job)) {
                while (this.xp.Count >= 3) {
                    this.xp.Remove(this.workedJobs.Dequeue());
                } 
                this.workedJobs.Enqueue(job);
                this.xp.Add(job, (Competency.Novice, 0));
            } else {
                if (this.xp[job].Item1 == Competency.Expert) {
                    this.xp.Update(job, (Competency.Regular, 0));
                } else {
                    this.xp.Update(job, (this.xp[job].Item1, this.xp[job].Item2 / 2));
                }
            }
        }

        public Competency GetCompetencyOf(JobData job) {
            return this.xp[job].Item1;
        }
    }
}