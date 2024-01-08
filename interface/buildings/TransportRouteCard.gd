class_name TransportRouteCard extends TextureButton
const TransportRoute = preload("res://common/buildings/transport/TransportRoute.cs")

@onready var res: HFlowContainer = %TransportedResources

func initialise(route: TransportRoute):
	var icon = %Icon
	var label = %DestinationLabel
	icon.set_texture(route.To.Data.Icon)
	label.set_text("To " + route.To.Data.Name)

func display_resources(resources: Dictionary):
	for key in resources:
		self.res.update_info(key, resources.get(key))
