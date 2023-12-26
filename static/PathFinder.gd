class_name PathFinder

var map: TileMap;
var nodes: Dictionary;
var adjacency: Dictionary;

const neighbour_yeven: Array = [
	Vector2(-1, 0),
	Vector2(1, 0),
	Vector2(0, 1),
	Vector2(0, -1),
	Vector2(-1, -1),
	Vector2(-1, 1)
];

const neighbour_yodd: Array = [
	Vector2(-1, 0),
	Vector2(1, 0),
	Vector2(0, 1),
	Vector2(0, -1),
	Vector2(1, 1),
	Vector2(1, -1)
];

func _init(map: TileMap):
	self.map = map;
	self.nodes = {};
	self.adjacency = {};
	for each in map.grid.values():
		self.add_point(each);

func _compute_cost(from_cell: Cell, to_cell: Cell) -> float:
	return map.get_cell_tile_data(map.layers.LAND, from_cell.pos).get_custom_data("terrain").time_to_traverse;

func _estimate_cost(from_cell: Cell, to_cell: Cell) -> float:
	return (from_cell.pos - to_cell.pos).length();
	
func _potential_neighbours(cell: Cell):
	var result: Array = [];
	if int(cell.pos.y) % 2 == 0:
		for each in neighbour_yeven:
			result.append(cell.pos + (each as Vector2i));
	else:
		for each in neighbour_yodd:
			result.append(cell.pos + (each as Vector2i));
	return result;
	
func _add_point(cell: Cell):
	nodes[cell.pos] = cell;
	
func _connect(from: Vector2, to: Vector2):
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
	while not pq.is_empty():
		var curr = pq.BSTfindMin();
		pq.BSTdelete(curr.value);
		if curr.other.front() == to.pos:
			return curr.other;
		if adjacency.has(curr.other.front()):
			for each in adjacency[curr.other.front()]:
				pq.BSTinsert(curr.value + _compute_cost(nodes[curr.other.front()], nodes[each]) 
						+ _estimate_cost(nodes[each], to), curr.other.push_front(each));
	return "fail";
