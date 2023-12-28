class_name JobData extends Resource

@export var name: String
@export var desc: String
@export var icon: Texture
@export var input: Dictionary
@export var output: Dictionary

func _to_string() -> String:
	return self.name
