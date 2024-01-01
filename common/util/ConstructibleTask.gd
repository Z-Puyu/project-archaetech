class_name ConstructibleTask extends Vertex

var days_remaining: int:
	set(days):
		days_remaining = days
	get:
		return days_remaining
var location: Cell

signal terminate(task: ConstructibleTask)

func _init(obj: Node2D, location: Cell):
	super._init(obj)
	self.days_remaining = obj.data.time_to_build
	self.location = location
	
func start():
	GameManager.game_clock.timeout.connect(self.progress)
	
func pause():
	GameManager.game_clock.timeout.disconnect(self.progress)

func progress():
	self.days_remaining -= 1
	if self.days_remaining == 0:
		if self.value() is Building:
			self.location.building = self.value()
			BuildingManager.spawn_building.emit(self.value())
		GameManager.game_clock.timeout.disconnect(self.progress)
		self.terminate.emit(self)
