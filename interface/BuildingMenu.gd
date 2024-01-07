class_name BuildingMenu extends PanelContainer
const Building = preload("res://common/buildings/Building.cs")
const BuildingManager = preload("res://static/BuildingManager.cs")

@onready var _tabs: Dictionary = {
	"all": $TabContainer/All/ScrollContainer/VBoxContainer,
	"production": $TabContainer/Production/ScrollContainer/VBoxContainer
}

func _ready():
	var building_manager: BuildingManager = get_node("/root/Global/BuildingManager")
	for uid in building_manager.AvailableBuildings.keys():
		var building: Building = building_manager.AvailableBuildings.get(uid).instantiate()
		self.add(building, self._tabs.get("all"))
		if building is ProductiveBuilding:
			self.add(building, self._tabs.get("production"))
		building.queue_free()
	self.hide()
		
func add(building: Building, tab: VBoxContainer):
	var card: BuildingCard = load("res://interface/buildings/BuildingCard.tscn").instantiate()
	tab.add_child(card)
	card.initialise(building)
	
func _gui_input(event: InputEvent):
	if event.is_action_released("left_click") or event is InputEventMouseMotion:
		accept_event()
	
	
func toggle():
	if self.is_visible():
		self.hide()
	else:
		self.set_visible(true)
