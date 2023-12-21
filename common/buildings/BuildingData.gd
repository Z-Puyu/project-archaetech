class_name BuildingData extends Resource

@export var name: String
# @export var built_texture: Texture
# @export var unbuilt_texture: Texture
@export var width: int = 1
@export var height: int = 1 # The actual height will be 2h - 1 for aesthetic purposes
@export var cost: Dictionary
@export var time_to_build: int
@export var production_methods: Array[ProductionMethod]
@export var activated_production_method: ProductionMethod
