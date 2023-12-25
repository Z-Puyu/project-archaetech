class_name ResourceInfo extends HBoxContainer

var _label: Label

func _ready():
	self._label = self.get_child(1)

func set_display(qty: float):
	var displayed_qty: String
	if qty < 1000:
		displayed_qty = str(floor(qty) as int)
	elif qty < 1000000:
		displayed_qty = ("%.2f" % (qty / 1000)) + "k"
	else:
		displayed_qty = ("%.2f" % (qty / 1000000)) + "m"
	self._label.text = displayed_qty
