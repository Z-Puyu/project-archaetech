class_name TransportRouteUI extends HBoxContainer
const TransportRoute: CSharpScript = preload("res://common/buildings/TransportRoute.cs")
var route: CSharpScript

func _ready():
	$DeleteButton.pressed.connect(self.delete)
	
func initialise(data: CSharpScript):
	self.route = data
	$TransportRouteCard.initialise(data)
	var option_button: OptionButton = $TransportRouteCard/MarginContainer/HBoxContainer/VBoxContainer/OptionButton
	option_button.item_selected.connect(func(level): self.route.manpower = level)

func delete():
	self.route.get_parent().transport_network.erase(route)
	self.route.queue_free()
	self.queue_free()
