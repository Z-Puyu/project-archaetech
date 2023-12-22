extends Control

var info: Label

# Called when the node enters the scene tree for the first time.
func _ready():
	self.visible = true
	self.info = $Info

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func showInfo(_data: CellData):
	self.visible = true
	self.info.text = _data._to_string()
	
