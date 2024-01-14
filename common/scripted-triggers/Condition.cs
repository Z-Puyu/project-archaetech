using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Godot;
using ProjectArchaetech.interfaces;

namespace ProjectArchaetech.triggers {
    [JsonDerivedType(typeof(Condition), typeDiscriminator: "condition")]
    [JsonDerivedType(typeof(AndCondition), typeDiscriminator: "AND")]
    [JsonDerivedType(typeof(OrCondition), typeDiscriminator: "OR")]
    [JsonDerivedType(typeof(XorCondition), typeDiscriminator: "XOR")]
    [JsonDerivedType(typeof(NotCondition), typeDiscriminator: "NOT")]
    public partial class Condition : ITestable {
        public string Name { get; set; }
        public dynamic Arg { get; set; }

        public Condition() {
            this.Name = "";
            this.Arg = null;
        }

        public Condition(string name, dynamic arg) {
            this.Name = name;
            this.Arg = arg;
        }

        public virtual bool IsTrue() {
            return (bool) Type.GetType("Triggers")
                .GetMethod(this.Name)
                .Invoke(null, [this.Arg]);
        }
    }

    public partial class AndCondition : Condition {
        public List<Condition> Conditions { get; set; }

        public AndCondition() : base() {
            this.Conditions = new List<Condition>();
        }

        public AndCondition(List<Condition> conditions) : base(default, default) {
            this.Conditions = conditions;
        }

        public override bool IsTrue() {
            foreach (Condition cond in this.Conditions) {
                if (!cond.IsTrue()) {
                    return false;
                }
            }
            return true;
        }
    }

    public partial class OrCondition : Condition {
        public List<Condition> Conditions { get; set; }

        public OrCondition() : base() {
            this.Conditions = new List<Condition>();
        }

        public OrCondition(List<Condition> conditions) : base(default, default) {
            this.Conditions = conditions;
        }

        public override bool IsTrue() {
            foreach (Condition cond in this.Conditions) {
                if (cond.IsTrue()) {
                    return true;
                };
            }
            return false;
        }
    }

    public partial class XorCondition : Condition {
        public List<Condition> Conditions { get; set; }

        public XorCondition() : base() {
            this.Conditions = new List<Condition>();
        }

        public XorCondition(List<Condition> conditions) : base(default, default) {
            this.Conditions = conditions;
        }

        public override bool IsTrue() {
            int count = 0;
            foreach (Condition cond in this.Conditions) {
                if (cond.IsTrue()) {
                    count += 1;
                };
                if (count > 1) {
                    return false;
                }
            }
            return count == 1;
        }
    }

    public partial class NotCondition : Condition {
        public List<Condition> Conditions { get; set; }

        public NotCondition() : base() {
            this.Conditions = new List<Condition>();
        }

        public NotCondition(List<Condition> conditions) : base(default, default) {
            this.Conditions = conditions;
        }

        public override bool IsTrue() {
            foreach (Condition cond in this.Conditions) {
                if (cond.IsTrue()) {
                    return false;
                };
            }
            return true;
        }
    }
}