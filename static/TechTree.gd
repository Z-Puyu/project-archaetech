extends Node

const ALL_TECHNOLOGIES = [
	preload("res://common/technologies/TechTest.tres")
]

enum tech_types {
	SCI,
	ENGIN,
	HUMAN
}

var _technologies: Array[Tech] # This is a connected DAG :O
var _selector: RandomSelector
var _researchable: Array[Tech]
var _unlocked: Dictionary
var _focus: Tech

func _ready():
	for tech in ALL_TECHNOLOGIES:
		_technologies.append(tech)
		_researchable.append(tech)
	_unlocked = {}
	var weights: Array[int] = []
	for tech in _researchable:
		weights.append(tech.weight)
	_selector = RandomSelector.new(_researchable, weights)
	ResourceManager.tech_progress.connect(_research)
	
func _research(available_points: int):
	var progress: Dictionary = {}
	# print("Available: " + str(available_points))
	# The focused tech will guarantee to receive one third of research points
	if _focus != null:
		progress[_focus] += floor(0.33 * available_points)
		available_points -= floor(0.33 * available_points)
	# Now we distribute n identical balls to r distinct boxes :O
	for research_point in available_points:
		var to: Tech = _selector.select()
		if progress.has(to):
			progress[to] += 1
		else:
			progress[to] = 1
	for tech in _researchable:
		tech.progress = min(tech.progress + progress.get(tech), tech.cost)
		print(tech.name + " gets " + str(min(progress.get(tech), tech.cost - tech.progress + progress.get(tech))) + " progress")
		if tech.progress == tech.cost:
			_unlock(tech)
			print("unlocked " + tech.name + "!")
				
func _unlock(tech: Tech):
	_unlocked[tech] = null
	_researchable.erase(tech)
	var new: Array[Tech] = []
	var new_weights: Array[int] = []
	for new_tech in tech.children:
		if _unlocked.has_all(new_tech.prerequisites):
			_researchable.append(new_tech)
			new.append(new_tech)
			new_weights.append(new_tech.weight)
	_selector.remove_items([tech], [tech.weight])
	_selector.add_items(new, new_weights)
