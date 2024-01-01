class_name DynamicResourceInfo extends HFlowContainer

var _info: Dictionary

func _ready():
	self._info = {}
		
func initialise(cost: Dictionary):
	var res_label: PackedScene = load("res://interface/buildings/DynamicResourceLabel.tscn")
	for res in cost:
		print("initialised " + str(res))
		var label: DynamicResourceLabel = res_label.instantiate()
		self._info[res] = label
		self.add_child(label)
		label.update(res.icon, cost.get(res))

func update_info(key: ResourceData, qty: float):
	var label: DynamicResourceLabel = self._info.get(key)
	var displayed_qty: String
	if qty < 10000:
		displayed_qty = str(floor(qty) as int)
	elif qty < 1000000:
		displayed_qty = ("%.2f" % (qty / 1000)) + "k"
	else:
		displayed_qty = ("%.2f" % (qty / 1000000)) + "m"
	label.set_text(displayed_qty)
