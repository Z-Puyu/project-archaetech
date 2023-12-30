extends Control

var info: Label

@onready var unit_menu = $NewUnit/UnitMenu
@onready var building_menu = $NewBuilding/BuildingMenu

# Called when the node enters the scene tree for the first time.
func _ready():
	self.visible = false
	self.info = $Info

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func showInfo(_data: Cell):
	self.visible = true
	#print(UnitManager.units.keys())
	#print(_data.pos)
	#print(UnitManager.units.keys().has(_data.pos))
	if UnitManager.units.keys().has(_data.pos):
		self.info.text = str(UnitManager.units[_data.pos])
	#self.info.text = _data._to_string()
	
func _on_new_unit_pressed():
	if building_menu.visible:
		building_menu.hide()	

func _on_new_building_pressed():
	if unit_menu.visible:
		unit_menu.hide()
