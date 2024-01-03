class_name BuildingCard extends TextureButton

@onready var icon: TextureRect = $MarginContainer/Building/IconFrame/Icon
@onready var building_name: Label = $MarginContainer/Building/Info/Name
@onready var cost: HFlowContainer = $MarginContainer/Building/Info/Cost
var building: String:
	get:
		return building

func _ready():
	self.pressed.connect(self._on_pressed)

func initialise(building: Building):
	var icon: Texture2D = building.data.icon
	var building_name: String = building.data.name
	var cost: Dictionary = building.data.cost
	self.building = building.data.id
	self.icon.set_texture(icon)
	self.building_name.set_text(building_name)
	self.cost.update_info(cost)

func _on_pressed():
	BuildingManager.add_building(self.building)
