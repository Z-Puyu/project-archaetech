class_name DynamicIcon extends TextureRect

func apply(icon: Texture2D):
	print(icon == null)
	self.set_texture(icon)
