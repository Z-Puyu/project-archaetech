extends Label

var curr_time: Dictionary = {
	"year": 2023,
	"month": 9,
	"day": 27
};

func next_day():
	if curr_time.day < 30:
		curr_time.day += 1
	else: 
		curr_time.day = 1
		if curr_time.month < 12:
			curr_time.month += 1
		else:
			curr_time.month = 1
			curr_time.year += 1
	self.text = str(curr_time.year, "/", curr_time.month, "/", curr_time.day)
