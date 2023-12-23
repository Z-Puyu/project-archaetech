class_name ConstructibleTask extends Vertex

var _days_remaining: int
var _pos: Vector2i

signal terminate(task: ConstructibleTask)

func _init(obj: Node2D, pos: Vector2i):
	super._init(obj)
	self._days_remaining = obj.data.time_to_build
	self._pos = pos
	
func start():
	GameManager.game_clock.timeout.connect(self.progress)
	
func pause():
	GameManager.game_clock.timeout.disconnect(self.progress)

func progress():
	self._days_remaining -= 1
	if self._days_remaining == 0:
		BuildingManager.spawn_building.emit(self.value(), self._pos)
		GameManager.game_clock.timeout.disconnect(self.progress)
		self.terminate.emit(self)
