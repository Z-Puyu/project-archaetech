using System;
using ProjectArchaetech.resources;

namespace ProjectArchaetech.common {
    public partial class Pop {
        private bool isChild;
        private bool isFemale;
        private JobData job;
        private Competency competency;

        public bool IsChild { get => isChild; set => isChild = value; }
        public bool IsFemale { get => isFemale; set => isFemale = value; }
        public JobData Job { get => job; set => job = value; }
        public Competency Competency { get => competency; set => competency = value; }

        public Pop(bool isChild, bool isFemale, JobData job) {
            this.isChild = isChild;
            this.isFemale = isFemale;
            this.job = job;
            this.competency = Competency.Novice;
        }

        public Pop() {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            this.isChild = rand.Next() % 2 == 1 ? true : false;
            this.isFemale = rand.Next() % 2 == 1 ? true : false;
            this.job = null;
            this.competency = Competency.Novice;
        }

        public Pop(bool isChild) {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            this.isChild = isChild;
            this.isFemale = rand.Next() % 2 == 1 ? true : false;
            this.job = null;
            this.competency = Competency.Novice;
        }
    }
}