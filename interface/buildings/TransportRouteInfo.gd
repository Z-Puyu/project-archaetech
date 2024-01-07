class_name TransportRouteInfo extends HBoxContainer
const TransportRoute = preload("res://common/buildings/transport/TransportRoute.cs")
var route: TransportRoute

func _ready():
	$DeleteButton.pressed.connect(self.delete)
	
func initialise(data: TransportRoute):
	self.route = data
	%TransportRouteCard.initialise(data)
	%TransportRouteCard/%OptionButton.item_selected.connect(func(lvl): self.route.Manpower = lvl)

func delete():
	self.route.Disable()
	Global.DeletedGameObj.emit(self)
