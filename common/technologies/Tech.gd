class_name Tech extends Resource

@export var name: String
@export var desc: String
@export var type: int
@export var icon: Texture
@export var prerequisites: Array[Tech]
@export var children: Array[Tech]
@export var cost: int
@export var progress: int
@export var weight: int
@export var rewards: Array[Variant]
