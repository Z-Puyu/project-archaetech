extends Node

# These define how much one Pop consumes per month 
# for each of the edible resources.
@export var primary_food_demands: Dictionary
@export var secondary_food_demands: Dictionary # Currently not used but will be
@export var primary_food_choices: Array[ResourceData]
var pop_count: int:
	set(count):
		pop_count = count
		changed.emit()
	get:
		return pop_count
var growth_rate: float:
	set(rate):
		growth_rate = rate
	get:
		return growth_rate
var pop_growth: float:
	set(progress):
		pop_growth = progress
		pop_grown.emit()
	get:
		return pop_growth
var unemployed: int:
	set(count):
		unemployed = count
		changed.emit()
	get:
		return unemployed

signal changed
signal pop_grown

func _ready():
	pop_count = 25
	growth_rate = 0.05
	pop_growth = 0.0
	unemployed = 25

func update():
	# We first compute how much food is needed if all Pops only consume basic food
	var yearly_food: int = pop_count * 12
	var food_storage: float = 0
	for food_type in primary_food_choices:
		if ResourceManager.resources.has(food_type):
			var stores: float = ResourceManager.resources.get(food_type)
			# Convert the food stores into basic food and aggregate
			food_storage += stores / primary_food_demands.get(food_type)
	consume_food() # Consume food for the month.
	# If we have lots of extra food, Pops grow faster.
	var growth_modifier: float = clamp(food_storage / (yearly_food * 3), 0, 1.5)
	pop_growth += growth_rate * growth_modifier
	var real_growth: int = floor(self.pop_growth)
	if real_growth != 0:
		# This is here to simulate a progress bar...
		self.pop_growth -= real_growth
		self.pop_count += real_growth
		
func consume_food():
	# The Pops will consume primary foods based on a priority ranking
	var unfed_pops: int = pop_count
	for food_type in primary_food_choices:
		if ResourceManager.resources.has(food_type):
			var stores: float = ResourceManager.resources.get(food_type)
			var per_capita_need: float = primary_food_demands.get(food_type)
			var num_fed: int = min(floor(stores / per_capita_need), unfed_pops)
			unfed_pops -= num_fed
			ResourceManager.consume({food_type: num_fed * per_capita_need})
		if unfed_pops <= 0:
			break
	# Then, the Pops will consume secondary foods.
	for food_type in secondary_food_demands:
		var per_capita_need: float = secondary_food_demands.get(food_type)
		if ResourceManager.has_enough(food_type, per_capita_need * pop_count):
			ResourceManager.consume({food_type: per_capita_need * pop_count})
		# else: pops become unhappy but we don't have that feature yet :O
