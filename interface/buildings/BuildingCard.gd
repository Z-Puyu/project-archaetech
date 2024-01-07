class_name BuildingCard extends TextureButton
const Building = preload("res://common/buildings/Building.cs")

@onready var icon: TextureRect = $MarginContainer/Building/IconFrame/Icon
@onready var building_name: Label = $MarginContainer/Building/Info/Name
@onready var cost: HFlowContainer = $MarginContainer/Building/Info/Cost
var building: String:
	get:
		return building

func _ready():
	self.pressed.connect(func(): Global.AddingBuilding.emit(self.building))

func initialise(building: Building):
	var icon: Texture2D = building.data.Icon
	var building_name: String = building.data.Name
	var cost: Dictionary = building.data.Cost
	self.building = building.data.Id
	self.icon.set_texture(icon)
	self.building_name.set_text(building_name)
	self.cost.update_info(cost)
