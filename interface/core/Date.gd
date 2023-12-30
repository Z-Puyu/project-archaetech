class_name Date extends Label

var date: Array[int]

func _ready():
	self.date = [0, 0, 1]
	GameManager.game_clock.timeout.connect(self.next_day)

func next_day():
	self.date[2] += 1
	if self.date[2] == 31:
		self.date[1] += 1
		self.date[2] = 1
		# print(self.date[2])
		if self.date[1] == 12:
			self.date[0] += 1
			self.date[1] = 0
	var years: String = " years " if self.date[0] > 1 else " year "
	var months: String = " months " if self.date[1] > 1 else " month "
	var days: String = " days " if self.date[2] > 1 else " day "
	var displayed_date: String = str(self.date[0], years, self.date[1], months, self.date[2], days)
	self.text = displayed_date + "\nsince Awakening"
			
