extends CanvasLayer

@onready var calendar: Label = $Calendar

func next_turn():
	self.calendar.next_day()
