class_name DynamicLabel extends Label

var parser: Callable

func use_parser(parser: Callable):
	self.parser = parser
	
func _ready():
	self.parser = func(text: Variant):
		var parsed: String
		if text is Array[String]:
			parsed = str(text)
		elif text is Dictionary:
			parsed = ""
			for key in text:
				parsed += str(key, ": ", text.get(key), ", ")
		else:
			parsed = str(text)
		return parsed

func update_info(text: Variant, alt_parser: Callable = Callable()):
	if alt_parser.is_null():
		self.set_text(self.parser.call(text))
	else:
		self.set_text(alt_parser.call(text))
