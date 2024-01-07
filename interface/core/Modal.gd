class_name Modal extends CanvasLayer
const Backdrop = preload("res://interface/core/Backdrop.gd")

@onready var backdrop: Backdrop = $Backdrop
@onready var windows: Dictionary = {
	"construction": $Backdrop/BuildingMenu,
	"building": $Backdrop/BuildingInfo
}
var active_window: PanelContainer

func _ready():
	self.hide() # Modal is invisible by default
	self.backdrop.clicked.connect(self._on_backdrop_clicked)
	Global.OpeningModalUI.connect(self.open)
	
func open(window_name: String):
	if not self.windows.has(window_name):
		push_error("Modal window with name \"" + window_name + "\" does not exist!")
		return
	var new_window: PanelContainer = self.windows.get(window_name)
	if self.active_window == new_window:
		push_warning("Attempting to open modal window \"" + window_name
			+ "\", but it is already open.")
		return
	if self.active_window != null:
		self.active_window.close()
		
	# Signalling to Global to set correct game mode.
	match window_name:
		"construction":
			Global.OpeningBuildingMenu.emit()
		"building":
			pass
		_:
			push_warning("Attempting to open a non-existing window. 
				(If you see this, something is wrong with modal window name validation.)")
		
	# Switch to the correct window
	self.active_window = new_window
	self.active_window.open()
	self.backdrop.open()
	self.show()
	Global.ClosingModalUI.connect(self.close)
	
func close():
	self.hide()
	if self.active_window != null:
		self.active_window.close()
		self.active_window = null
		Global.ClosingModalUI.disconnect(self.close)
	else:
		push_warning("Attempting to close the modal when none of its windows is active.")

func _on_backdrop_clicked():
	if self.is_visible():
		self.close()
