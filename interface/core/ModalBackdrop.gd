class_name Modal extends CanvasLayer

@onready var _backdrop: Backdrop = $Backdrop
@onready var _windows: Dictionary = {
	"building": $Backdrop/BuildingMenu
}
var _active_window: PanelContainer

func _ready():
	self.hide()
	self._backdrop.click.connect(self.hide)
		
func on_toggle(window_name: String):
	print("toggle")
	if self.is_visible():
		self.hide()
	else:
		self._on_open(window_name)
	
func _on_open(window_name: String):
	self.show()
	self._active_window = self._windows.get(window_name)
	self._active_window.show()
	
	
