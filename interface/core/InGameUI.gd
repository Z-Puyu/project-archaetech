class_name InGameUI extends Control

func _ready():
	$ModalButtonGroup/NewBuildingButton.toggle.connect($Modal.on_toggle)	
