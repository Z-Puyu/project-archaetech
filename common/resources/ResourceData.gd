class_name ResourceData extends Resource

@export var name: String
@export var desc: String
@export var icon: Texture
@export var type: int

func _to_string() -> String:
	return self.name
