class_name InfoPanel extends Panel

func _ready():
	self.hide()
	Global.CellSelected.connect(func(data: TileData): self.show())
	Global.CellSelected.connect($CellInfo.show_info)
	
#func _on_new_unit_pressed():
#	if building_menu.visible:
#		building_menu.hide()	

#func _on_new_building_pressed():
#	if unit_menu.visible:
#		unit_menu.hide()
