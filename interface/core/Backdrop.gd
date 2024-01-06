class_name Backdrop extends Control

signal click

func _ready():
	self.hide()
	
func _gui_input(event: InputEvent):
	if event.is_action_released("left_click"):
		self.click.emit()
		self.hide()

func _input(event):
	if event.is_action_pressed("space_bar_pressed"):
		print("HI from " + str(self))
