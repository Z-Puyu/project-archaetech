class_name DynamicLabel extends Label

func apply(text: Variant):
	print(text)
	var parsed: String
	if text is Array[String]:
		parsed = str(text)
	elif text is Dictionary:
		parsed = ""
		for key in text:
			print(key)
			parsed += str(key, ": ", text.get(key), ", ")
	else:
		parsed = text
	self.set_text(parsed)
