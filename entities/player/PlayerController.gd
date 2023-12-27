extends CharacterController

func _physics_process(delta):
	var direction_x = Input.get_axis("ui_left", "ui_right")
	var direction_y = Input.get_axis("ui_up", "ui_down")
	direction = Vector2(direction_x, direction_y)
	super._physics_process(delta)
