extends Control

var info: Label

# Called when the node enters the scene tree for the first time.
func _ready():
	self.visible = false
	self.info = $Info

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func showInfo(_data: Cell):
	self.visible = true
	## print(UnitManager.units.keys())
	## print(_data.pos)
	## print(UnitManager.units.keys().has(_data.pos))
	if UnitManager.units.keys().has(_data.pos):
		self.info.text = str(UnitManager.units[_data.pos])
	#self.info.text = _data._to_string()
	
