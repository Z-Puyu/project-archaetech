class_name InGameUI extends Control
const Cell = preload("res://common/map/Cell.cs")

@onready var _modal: CanvasLayer = %Modal

func _ready():
	Global.GameForcePaused.connect(func(): %Calendar/%PauseButton.button_pressed = false)
	Global.GameForceResumed.connect(func(): %Calendar/%PauseButton.button_pressed = true)
	%Calendar/%PauseButton.toggled.connect(func(toggled: bool): 
		Global.ResumeTime() if toggled else Global.PauseTime()
	)
	%NewBuildingButton.pressed.connect(func(): self._on_toggle_modal("construction"))
	Global.CellSelected.connect(func(cell: Cell, data: TileData): %InfoPanel.show())
	Global.CellSelected.connect(%InfoPanel/%CellInfo.show_info)

func _on_toggle_modal(window_name: String):
	Global.RestoringNormalMode.emit()
	if self._modal.is_visible():
		self._modal.close()
	else:
		self._modal.open(window_name)
