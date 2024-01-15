class_name BuildingInfo extends PanelContainer
const Building = preload("res://common/buildings/Building.cs")
const BaseBuilding = preload("res://common/buildings/BaseBuilding.cs")
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
	"local_storage": %LocalStorage,
	"transport_routes": %ListOfRoutes
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
				parsed += (job.name + ": ")
				for competency in curr.get(job).keys():
					print(competency)
					var label: String = ""
					match competency:
						0:
							label = "Novice: "
						1:
							label = "Regular: "
						2:
							label = "Expert: "
						_:
							label = "Unknown: "
					parsed += str(label, curr.get(job).get(competency).size(), ", ")
				var total: int = max.get(job)
				parsed += str("Total: ", total, "\n")
			return parsed
	)
	self.cached_buildings = {}
	self.hide()
				
func _update_data(building: Building, data: Dictionary):
	var load_data: Callable = func(info: Dictionary):
		for key in info.keys():
			self.components.get(key).update_info(info.get(key))
		
	if self.displayed_building.Data.Id != building.Data.Id:
		push_error("Loading incorrect building into Building Info panel!
			It is likely that there is an issue with signal connection.")
	else:
		if not self.cached_buildings.has(building):
			self.cached_buildings[building] = {}
		for key in data.keys():
			self.cached_buildings[building][key] = data.get(key)
		load_data.call(self.cached_buildings[building])

func open(init_info: Dictionary = {}):
	self.new_route_button.pressed.connect(func(): 
		Global.EnteringRouteConstructionMode.emit()
	)
	var pick_up: Node = Global.GetPickUp()
	if not pick_up is Building:
		push_error("Loading incorrect data into Building Info panel!")
	else:
		self.displayed_building = pick_up
		self.displayed_building.BuildingInfoUpdatedUI.connect(self._update_data)
		self._update_data(self.displayed_building, init_info)
		self.show()
	
func close():
	self.displayed_building.BuildingInfoUpdatedUI.disconnect(self._update_data)
	self.displayed_building = null
	for key in self.components:
		self.components.get(key).clear()
	self.hide()
