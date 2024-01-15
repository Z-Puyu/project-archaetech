class_name Grid extends Resource

var cells: Dictionary: # [Vector2i, Cell]
	get:
		return cells

func add(new_cells: Variant):
	if new_cells is Dictionary:
		for key in new_cells.keys():
			if key is Vector2i:
				self.cells[key] = new_cells.get(key)
	elif new_cells is Array:
		for cell in new_cells:
			self.cells[cell.pos] = cell
	elif new_cells is Cell:
		self.cells[new_cells.pos] = new_cells
	
func get_cell(pos: Vector2i) -> Cell:
	return self.cells.get(pos)
