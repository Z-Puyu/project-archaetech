class_name TransportRouteInfo extends HBoxContainer
const TransportRoute = preload("res://common/buildings/transport/TransportRoute.cs")
var route: TransportRoute

signal ui_deleted(which: TransportRouteInfo);

func _ready():
	$DeleteButton.pressed.connect(self.delete)
	
func initialise(data: TransportRoute):
	self.route = data
	%TransportRouteCard.initialise(data)
	%TransportRouteCard/%OptionButton.item_selected.connect(func(lvl): self.route.Level = lvl)

func delete():
	self.route.Disable()
	Global.TransportRouteRemoved.emit(self.route)
	self.ui_deleted.emit(self)
	Global.DeletedGameObj.emit(self)
