extends VBoxContainer

func create_entries(_data: CellData):
	for i in self.get_children():
		self.remove_child(i);
		i.queue_free();
	var buttons: Array[Button] = [];
	for i in len(_data.units):
		var new_button = Button.new();
		new_button.set_size(Vector2(60, 60));
		new_button.text = _data.units[i].name;
		add_child(new_button);
