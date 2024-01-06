class_name Modal extends CanvasLayer

@onready var backdrop: Backdrop = $Backdrop
@onready var windows: Dictionary = {
	"construction": $Backdrop/BuildingMenu,
	"building": $Backdrop/BuildingInfo
}
var active_window: PanelContainer

func _ready():
	self.hide() # Modal is invisible by default
	self.backdrop.click.connect(self.hide)
	get_node("/root/Global/Events").ModalToggled.connect(self.on_toggle)
		
func on_toggle(window_name: String):
	if self.is_visible():
		self.backdrop.hide()
		self.hide()
	else:
		self._on_open(window_name)
	
func _on_open(window_name: String):
	self.show()
	self.backdrop.show()
	# Switch to the correct window
	if active_window != null:
		self.active_window.hide()
	self.active_window = self.windows.get(window_name)
	self.active_window.show()
	
func _input(event):
	if event.is_action_pressed("space_bar_pressed"):
		print("HI from " + str(self))
	
	
