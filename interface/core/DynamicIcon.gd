class_name DynamicIcon extends TextureRect

func update_info(icon: Texture2D):
	self.set_texture(icon)

func clear():
	self.set_texture(null)
