class_name ResourcePanel extends Panel

@export var info_boxes: Dictionary = {}

func update(res: ResourceData, new_qty: float):
	get_node(self.info_boxes.get(res)).set_display(new_qty)
	
