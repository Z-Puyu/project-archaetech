class_name DynamicLabel extends Label

func update_info(text: Variant):
	var parsed: String
	if text is Array[String]:
		parsed = str(text)
	elif text is Dictionary:
		parsed = ""
		for key in text:
			parsed += str(key, ": ", text.get(key), ", ")
	else:
		parsed = str(text)
	self.set_text(parsed)
