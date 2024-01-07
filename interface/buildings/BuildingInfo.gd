class_name BuildingInfo extends PanelContainer
const Building = preload("res://common/buildings/Building.cs")
const TransportRoute = preload("res://common/buildings/TransportRoute.cs")

const CARD = preload("res://interface/buildings/TransportRouteInfo.tscn")

@onready var components: Dictionary = {
	"name": %Name,
	"icon": %Icon,
	"employment": %Employment,
	"output": %MonthlyOutput,
	"local_storage": %LocalStorage
}
@onready var new_route_button: TextureButton = %NewRouteButton
@onready var list_of_routes: VBoxContainer = %ListOfRoutes
var cached_buildings: Dictionary:
	get:
		return cached_buildings

func _ready():
	components.get("employment").use_parser(func(text: Pair):
		if text.First is Dictionary and text.Second is Dictionary:
			var curr: Dictionary = text.First
			var max: Dictionary = text.Second
			var parsed: String = ""
			for job in curr.keys():
				var denominator: int = max.get(job)
				var numerator: int = curr.get(job)
				parsed += str(job.name, ": ", numerator, "/", denominator, " ")
			return parsed	
		return "No employment data available"
	)
	Global.DisplayingBuildingUI.connect(self._open)
	self.cached_buildings = {}
	self.hide()

func _open(building: Building):
	self.new_route_button.pressed.connect(func(): 
		Global.ChangingGameMode.emit(3)
		Global.PickingUpObj.emit(building);
	)
	self._show_info(building)

func _show_info(building: Building):
	var info: Dictionary = {
		"name": building.data.name,
		"icon": building.data.icon
	}
	
	if building is ManableBuilding:
		info["employment"] = building.GetEmploymentData()
		info["local_storage"] = {} if building is BaseBuilding else building.warehouse.resources
	
	if building is LogisticBuilding:
		var transport_network: Array = building.GetOutwardRoutes()
		if not self.cached_buildings.has(building):
			var routes: Dictionary = {}
			for route in transport_network:
				routes[route] = self._add_route_info(route)
			self.cached_buildings[building] = routes
		else: 
			self._load(building)
			
	if building is ProductiveBuilding:
		info["output"] = building.GetOutput()
	for key in info:
		components.get(key).update_info(info.get(key))
	self.show()
	
func _add_route_info(route: TransportRoute) -> TransportRouteInfo:
	self.cached_buildings.get(route.From).append(route)
	var new_route_card: TransportRouteInfo = CARD.instantiate()
	new_route_card.initialise(route)
	self.list_of_routes.add_child(new_route_card)
	return new_route_card
	
func _load(building: Building):
	var routes: Dictionary = self.cached_buildings.get(building)
	for route in routes:
		self.list_of_routes.add_child(routes.get(route))
