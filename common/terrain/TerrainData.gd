class_name TerrainData extends Resource

@export var type: int
@export var name: String
@export var time_to_traverse: int # Number of days to pass through the tile
@export var impassible: bool = false
@export var harmful: bool = false
@export var damage_per_day: int
@export var resource_probability: float # Probability that collectible resources is discovered
@export var discoverable_resources: Dictionary # [Resource, Weight]
