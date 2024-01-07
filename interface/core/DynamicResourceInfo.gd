class_name DynamicResourceInfo extends HFlowContainer
const DynamicResourceLabel = preload("res://interface/DynamicResourceLabel.gd")
const ResourceData = preload("res://common/resources/ResourceData.cs")

const LABEL = preload("res://interface/buildings/DynamicResourceLabel.tscn")
var info: Dictionary:
	set(dict):
		info = dict
	get:
		return info

func _ready():
	self.info = {}

func update_info(new_info: Dictionary):
	print(new_info)
	for key in new_info:
		if not self.info.has(key):
			add_resource(key, new_info.get(key))
		else:
			var label: DynamicResourceLabel = self.info.get(key)
			var displayed_qty: String
			var qty: float = new_info.get(key)
			if qty < 10000:
				displayed_qty = str(floor(qty) as int)
			elif qty < 1000000:
				displayed_qty = ("%.2f" % (qty / 1000)) + "k"
			else:
				displayed_qty = ("%.2f" % (qty / 1000000)) + "m"
			label.set_text(displayed_qty)
	
func add_resource(resource: ResourceData, qty: float):
	var label: DynamicResourceLabel = LABEL.instantiate()
	self.info[resource] = label
	self.add_child(label)
	label.update(resource.icon, qty)
	
func clear():
	self.info = {}
	for label in self.get_children():
		self.remove_child(label)
