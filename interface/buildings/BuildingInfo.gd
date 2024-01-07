class_name BuildingInfo extends PanelContainer
const Building = preload("res://common/buildings/Building.cs")
const BaseBuilding = preload("res://common/buildings/BaseBuilding.cs")
const LogisticBuilding = preload("res://common/buildings/LogisticBuilding.cs")
const ManableBuilding = preload("res://common/buildings/ManableBuilding.cs")
const ProductiveBuilding = preload("res://common/buildings/ProductiveBuilding.cs")
const TransportRoute = preload("res://common/buildings/transport/TransportRoute.cs")
const TransportRouteInfo = preload("res://interface/buildings/TransportRouteInfo.gd")
const Pair = preload("res://util/Pair.cs")

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
var displayed_building: Building:
	set(building):
		displayed_building = building
	get:
		return displayed_building
var cached_buildings: Dictionary:
	get:
		return cached_buildings

func _ready():
	components.get("employment").use_parser(func(text: Pair):
		if text.First is Dictionary and text.Second is Dictionary:
			var curr: Dictionary = text.First
			var max: Dictionary = text.Second
			if curr.is_empty() or max.is_empty():
				return "No employment data available"
			var parsed: String = ""
			for job in curr.keys():
				var denominator: int = max.get(job)
				var numerator: int = curr.get(job)
				parsed += str(job.name, ": ", numerator, "/", denominator, " ")
			return parsed	
	)
	Global.TransportRouteAdded.connect(self._add_route_info)
	self.cached_buildings = {}
	self.hide()
	
func _load_data(building: Building):
	if self.displayed_building.data.Id != building.data.Id:
		push_error("Loading incorrect building into Building Info panel!
			 It is likely that there is an issue with signal connection.")
	else:
		components.get("name").update_info(building.data.Name)
		components.get("icon").update_info(building.data.Icon)
		
		if building is ManableBuilding:
			components.get("employment").update_info(building.GetEmploymentData())
			components.get("local_storage").update_info({} if building is BaseBuilding 
				else building.warehouse.Resources)
		
		if building is LogisticBuilding:
			if not self.cached_buildings.has(building):
				var transport_network: Array = building.GetOutwardRoutes()
				var routes: Dictionary = {}
				for route in transport_network:
					routes[route] = self._add_route_info(route)
				self.cached_buildings[building] = routes
			else: 
				self._load(building)
				
		if building is ProductiveBuilding:
			components.get("output").update_info(building.GetOutput())

func open():
	self.new_route_button.pressed.connect(func(): 
		Global.EnteringRouteConstructionMode.emit()
	)
	var pick_up: Node = Global.GetPickUp()
	print(pick_up)
	if not pick_up is Building:
		push_error("Loading incorrect data into Building Info panel!")
	else:
		self.displayed_building = pick_up
		self.displayed_building.BuildingInfoUpdatedUI.connect(self._load_data)
		self._load_data(pick_up)
		self.show()
	
func close():
	self.displayed_building.BuildingInfoUpdatedUI.disconnect(self._load_data)
	self.displayed_building = null
	for key in self.components:
		self.components.get(key).clear()
	self.hide()

func _show_info(info: Dictionary):
	for key in info:
		if self.components.has(key):
			self.components.get(key).update_info(info.get(key))
		else:
			push_error("Non-existing building attribute \"" + key + "\"!")
			return
			
func _load(building: Building):
	var routes: Dictionary = self.cached_buildings.get(building)
	for route in routes:
		self.list_of_routes.add_child(routes.get(route))
	
func _add_route_info(route: TransportRoute) -> TransportRouteInfo:
	var new_route_card: TransportRouteInfo = CARD.instantiate()
	new_route_card.initialise(route)
	# Register this card into cache.
	self.cached_buildings.get(route.From)[route] = new_route_card
	self.list_of_routes.add_child(new_route_card)
	new_route_card.get_child(1).pressed.connect(func(): 
			self.cached_buildings.get(route.From).erase(route))
	return new_route_card
