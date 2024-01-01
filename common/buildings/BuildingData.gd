class_name BuildingData extends Resource

@export var name: String
@export var id: String
@export var icon: Texture2D
@export var width: int = 1
@export var height: int = 1 # The actual height will be 2h - 1 for aesthetic purposes
@export var cost: Dictionary
@export var required_terrains: Dictionary
@export var required_resource: ResourceData
@export var time_to_build: int
@export var production_methods: Array[ProductionMethod]
@export var activated_production_method: ProductionMethod
