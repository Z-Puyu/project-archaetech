class_name BuildingInfo extends PanelContainer

const CARD = preload("res://interface/buildings/TransportRoute.tscn")

@onready var components: Dictionary = {
	"name": $MarginContainer/VBoxContainer/Header/Name,
	"icon": $MarginContainer/VBoxContainer/Header/Icon,
	"output": $MarginContainer/VBoxContainer/InfoList/MonthlyOutput,
	"local_storage": $MarginContainer/VBoxContainer/InfoList/LocalStorage,
	"new_route_button": $MarginContainer/VBoxContainer/InfoList/ScrollContainer/VBoxContainer/NewRouteButton
}
@onready var list_of_routes: VBoxContainer = $MarginContainer/VBoxContainer/InfoList/ScrollContainer/VBoxContainer
var cached_buildings: Dictionary:
	get:
		return cached_buildings

func _ready():
	Events.show_building_info.connect(self._open)
	Events.add_route_ui.connect(self._add_route)
	self.cached_buildings = {}
	self.hide()

func _open(building: Building):
	var new_route: TextureButton = self.components.get("new_route_button")
	new_route.pressed.connect(func(): 
		GameManager.game_mode = GameManager.GAME_MODES.BUILD_ROUTE
		GameManager.pick_up = building
	)
	self._show_info(building)

func _show_info(building: Building):
	var info: Dictionary = {
		"name": building.data.name,
		"icon": building.data.icon,
		"output": building.output,
		"local_storage": {} if building is BaseBuilding else building.warehouse.resources
	}
	for key in info:
		components.get(key).update_info(info.get(key))
	var transport_network: Array[TransportRoute] = building.transport_network
	if not self.cached_buildings.has(building):
		var routes: Dictionary = {}
		for route in building.transport_network:
			routes[route] = self._add_route(route)
		self.cached_buildings[building] = routes
	else: 
		self._load(building)
	self.show()
	
func _add_route(route: TransportRoute) -> TransportRouteUI:
	var new_route_card: TransportRouteUI = CARD.instantiate()
	new_route_card.initialise(route)
	self.list_of_routes.add_child(new_route_card)
	return new_route_card
	
func _load(building: Building):
	var routes: Dictionary = self.cached_buildings.get(building)
	for route in routes:
		self.list_of_routes.add_child(routes.get(route))
