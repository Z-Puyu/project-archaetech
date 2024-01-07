class_name Backdrop extends Control

signal clicked

func _ready():
	self.hide()
	
func _gui_input(event: InputEvent):
	if self.is_visible():
		if not Global.IsBuildingTransportRoutes():
			if event.is_action_released("left_click"):
				self.close()
		elif event.is_action_pressed("right_click"):
			Global.ExitingRouteConstructionMode.emit()
			self.close()

func open():
	self.show()
	
func close():
	self.clicked.emit()
	self.hide()
