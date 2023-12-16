extends Control

var Building: Label;
var Units: Label;

# Called when the node enters the scene tree for the first time.
func _ready():
	self.visible = true;
	self.Building = $Building;
	self.Units = $Units;

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func showInfo(_data: CellData):
	self.visible = true;
	self.Building.text = str(_data.buildings);
	self.Units.text = str(_data.units);
	
