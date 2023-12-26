class_name PathFinder

var map: TileMap;
var nodes: Dictionary;
var adjacency: Dictionary;
var UNIT_LENGTH: float;
var visited: Array = [];

const neighbour_yeven: Array = [
	Vector2i(-1, 0),
	Vector2i(1, 0),
	Vector2i(0, 1),
	Vector2i(0, -1),
	Vector2i(-1, -1),
	Vector2i(-1, 1)
];

const neighbour_yodd: Array = [
	Vector2i(-1, 0),
	Vector2i(1, 0),
	Vector2i(0, 1),
	Vector2i(0, -1),
	Vector2i(1, 1),
	Vector2i(1, -1)
];

func _init(map: TileMap):
	self.map = map;
	self.nodes = {};
	self.adjacency = {};
	self.UNIT_LENGTH = map.map_to_local(Vector2i(0,0)).distance_to(map.map_to_local(Vector2i(0,1)));
	for each in map.land_navigable.values():
		self.add_point(each);
	print(nodes)	

func _compute_cost(from_cell: Cell, to_cell: Cell) -> float:
	return map.get_cell_tile_data(map.layers.LAND, to_cell.pos).get_custom_data("terrain").time_to_traverse;

func _estimate_cost(from_cell: Cell, to_cell: Cell) -> float:
	return map.map_to_local(from_cell.pos).distance_to(map.map_to_local(to_cell.pos)) / UNIT_LENGTH;
	
func _potential_neighbours(cell: Cell):
	var result: Array = [];
	if cell.pos.y % 2 == 0:
		for each in neighbour_yeven:
			result.append(cell.pos + each);
	else:
		for each in neighbour_yodd:
			result.append(cell.pos + each);
	return result;
	
func _add_point(cell: Cell):
	nodes[cell.pos] = cell;
	
func _connect(from: Vector2i, to: Vector2i):
	if not adjacency.has(from):
		adjacency[from] = [to]
	else:
		adjacency[from].append(to);
		
	if not adjacency.has(to):
		adjacency[to] = [from]
	else:
		adjacency[to].append(from);

func add_point(cell: Cell):
	if not nodes.has(cell.pos):
		self._add_point(cell);
		for each in _potential_neighbours(cell):
			if nodes.has(each):
				_connect(cell.pos, each);


func find(from: Cell, to: Cell):
	var pq = BST.new()
	pq.BSTinsert(0, [from.pos]);
	visited.append(from.pos);
	while not pq.is_empty():
		var curr = pq.BSTfindMin();
		pq.BSTdelete(curr.value);
		if curr.other.front() == to.pos:
			return curr.other;
		for each in adjacency[curr.other.front()]:
			if visited.has(each):
				continue;
			var path = curr.other.duplicate(true);
			path.push_front(each);
			visited.append(each);
			var cost = curr.value + _compute_cost(nodes[curr.other.front()], nodes[each]) + _estimate_cost(nodes[each], to);
			pq.BSTinsert(cost, path);
	return "fail";
