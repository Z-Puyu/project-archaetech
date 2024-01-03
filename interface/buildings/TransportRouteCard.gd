class_name TransportRouteCard extends TextureButton

@onready var res: HFlowContainer = $MarginContainer/HBoxContainer/VBoxContainer/TransportedResources

func initialise(data: TransportRoute):
	var icon = $MarginContainer/HBoxContainer/Icon
	var label = $MarginContainer/HBoxContainer/VBoxContainer/DestinationLabel
	icon.set_texture(data.to.data.icon)
	label.set_text("To " + data.to.data.name)

func display_resources(resources: Dictionary):
	for key in resources:
		self.res.update_info(key, resources.get(key))
