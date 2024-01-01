class_name ResourcePanel extends Panel

@export var info_boxes: Dictionary = {}

func update(res: ResourceData, new_qty: float):
	var displayed_qty: String
	if new_qty < 10000:
		displayed_qty = str(floor(new_qty) as int)
	elif new_qty < 1000000:
		displayed_qty = ("%.2f" % (new_qty / 1000)) + "k"
	else:
		displayed_qty = ("%.2f" % (new_qty / 1000000)) + "m"
	get_node(self.info_boxes.get(res)).set_text(displayed_qty)
	
