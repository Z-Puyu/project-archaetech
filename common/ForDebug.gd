extends Label

var msg: String = "";
var t_speed: int = 1;

func _process(delta) -> void:
	self.print_msg();

func print_msg() -> void:
	self.text = str(self.msg, "     time elapse speed: ", self.t_speed, "X"); 
