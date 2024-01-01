class_name Backdrop extends Control

signal click
	
func _unhandled_input(event: InputEvent):
	if event.is_action_released("left_click"):
		self.click.emit()
	#if event is InputEventMouseMotion:
		#print("Mouse moved")
