using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectArchaetech.common.util {
    public struct Date {
        private readonly int year;
        private readonly int month;
        private readonly int day;

        public Date(int year, int month, int day) {
            this.year = year;
            this.month = month;
            this.day = day;
        }
        
        public Date(int NDays) {
            int NMonths = NDays / 30;
            this.day = NDays - NMonths * 30;
            int NYears = NMonths / 12;
            this.month = NMonths - NYears * 12;
            this.year = NYears;
        }

        public int ToDays() {
            return (this.year * 12 + this.month) * 30 + this.day;
        }

        public static bool operator ==(Date date, Date other) {
            return date.ToDays() == other.ToDays();
        }

        public static bool operator !=(Date date, Date other) {
            return date.ToDays() != other.ToDays();
        }

        public static bool operator >(Date date, Date other) {
            return date.ToDays() > other.ToDays();
        }
    
        public static bool operator <(Date date, Date other) {
            return date.ToDays() < other.ToDays();
        }
        
        public static bool operator <=(Date date, Date other) {
            return date.ToDays() <= other.ToDays();
        }

        public static bool operator >=(Date date, Date other) {
            return date.ToDays() >= other.ToDays();
        }

        public static Date operator +(Date date, int n) {
            return new Date(date.ToDays() + n);
        }

        public static Date operator +(Date date, Date other) {
            return new Date(date.ToDays() + other.ToDays());
        }

        public static Date operator -(Date date, int n) {
            if (date.ToDays() <= n) {
                throw new ArgumentException();
            }
            return new Date(date.ToDays() - n);
        }

        public static Date operator -(Date date, Date other) {
            if (date <= other) {
                throw new ArgumentException();
            }
            return new Date(date.ToDays() + other.ToDays());
        }

        public static int operator %(Date date, int n) {
            return date.ToDays() % n;
        }

        public override string ToString() {
            return $"{this.year} years {this.month} months {this.day} days";
        }
    }
}