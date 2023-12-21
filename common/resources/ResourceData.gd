class_name ResourceData extends Resource

enum resource_types {
	COMMON_RAW,
	RARE_RAW,
	ENERGY,
	CONSUMER,
	INDUSTRIAL,
	RESEARCH,
	LUXUARY,
	HIGH_END
}

@export var name: String
@export var icon: Texture
@export var type: int

func _to_string() -> String:
	return self.name
