class_name ResourcePanel extends Panel
const ResourceData = preload("res://common/resources/ResourceData.cs")

@export var info_boxes: Dictionary = {}

func _ready():
	get_node("/root/Global/ResourceManager").ResourceQtyUpdated.connect(self.update)

func update(res: ResourceData, new_qty: float):
	var displayed_qty: String
	if new_qty < 10000:
		displayed_qty = str(floor(new_qty) as int)
	elif new_qty < 1000000:
		displayed_qty = ("%.2f" % (new_qty / 1000)) + "k"
	else:
		displayed_qty = ("%.2f" % (new_qty / 1000000)) + "m"
	if res.type == 5:
		get_node(self.info_boxes.get(res)).set_text("+ " + displayed_qty)
	else:
		get_node(self.info_boxes.get(res)).set_text(displayed_qty)
	
