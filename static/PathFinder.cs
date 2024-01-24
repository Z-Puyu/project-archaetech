using System;
using System.Collections.Generic;
using System.Linq;
using C5;
using Godot;
using ProjectArchaetech.common;

namespace ProjectArchaetech {
    public class PathFinder {
        private static readonly List<Vector2I> offsetsEven = [
            new Vector2I(-1, 0),
            new Vector2I(1, 0),
            new Vector2I(0, 1),
            new Vector2I(0, -1),
            new Vector2I(-1, -1),
            new Vector2I(-1, 1)
        ];

        private static readonly List<Vector2I> offsetsOdd = [
            new Vector2I(-1, 0),
            new Vector2I(1, 0),
            new Vector2I(0, 1),
            new Vector2I(0, -1),
            new Vector2I(1, 1),
            new Vector2I(1, -1)
        ];

        private readonly Map map;
        private readonly HashDictionary<Vector2I, Cell> nodes;
        private readonly HashDictionary<Vector2I, List<Vector2I>> adjacency;
        private readonly double unitLength;
        private readonly C5.HashSet<Vector2I> visited;
        private readonly TreeDictionary<int, IEnumerable<Vector2I>> pq;

        public PathFinder(Map map, HashDictionary<Vector2I, Cell> nodes) {
            this.map = map;
            this.nodes = nodes;
            this.adjacency = new HashDictionary<Vector2I, List<Vector2I>>();
            foreach (Vector2I pos in nodes.Keys) {
                this.ConnectNeighbours(pos);
            }
            this.unitLength = map.GetDistance(new Vector2I(0, 0), new Vector2I(0, 1));
            this.pq = new TreeDictionary<int, IEnumerable<Vector2I>>();
        }

        private double ComputeCost(Vector2I from, Vector2I to) {
            return ((double) (this.map.GetTerrain(from).TimeToTraverse + 
                this.map.GetTerrain(to).TimeToTraverse)) / 2.0;
        }

        private double EstimateCost(Vector2I from, Vector2I to) {
            return this.map.GetDistance(from, to) / this.unitLength;
        }

        private int CostBetween(Vector2I from, Vector2I mid, Vector2I to) {
            return (int) Math.Ceiling(this.ComputeCost(from, mid) + this.EstimateCost(mid, to));
        }

        private static List<Vector2I> NeighboursOf(Vector2I pos) {
            List<Vector2I> neighbours = new List<Vector2I>(6);
            if (pos.Y % 2 == 0) {
                foreach (Vector2I offset in offsetsEven) {
                    neighbours.Add(pos + offset);
                }
            } else {
                foreach (Vector2I offset in offsetsOdd) {
                    neighbours.Add(pos + offset);
                }
            }
            return neighbours;
        }

        private void Connect(Vector2I from, Vector2I to) {
            if (!this.adjacency.Contains(from)) {
                this.adjacency[from] = [to];
            } else {
                this.adjacency[from].Add(to);
            }

            if (!this.adjacency.Contains(to)) {
                this.adjacency[to] = [from];
            } else {
                this.adjacency[to].Add(from);
            }
        }

        private void ConnectNeighbours(Vector2I pos) {
            foreach (Vector2I u in NeighboursOf(pos)) {
                this.Connect(u, pos);
            }
        }

        private void Add(Cell cell) {
            if (this.nodes.Contains(cell.Pos)) {
                this.nodes[cell.Pos] = cell;
                this.ConnectNeighbours(cell.Pos);
            }
        }

        public IEnumerable<Vector2I> FindPath(Vector2I from, Vector2I to) {
            List<Vector2I> initPath = [from];
            this.pq.Add(0, initPath);
            this.visited.Add(from);
            
            while (!this.pq.IsEmpty) {
                C5.KeyValuePair<int, IEnumerable<Vector2I>> curr = this.pq.DeleteMin();
                Vector2I currPos = curr.Value.Last();
                if (currPos == to) {
                    return curr.Value; // Reached!
                }
                foreach (Vector2I u in this.adjacency[currPos]) {
                    if (this.visited.Contains(u)) {
                        continue;
                    }
                    this.visited.Add(u);
                    int cost = curr.Key + this.CostBetween(from, u, to);
                    this.pq.Add(cost, curr.Value.Append(u));
                }
            }

            return null; // Unreachable!
        }
    }
}