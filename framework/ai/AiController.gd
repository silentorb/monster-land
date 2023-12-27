extends CharacterController

var direction_timer = 1

@export var direction_duration = 1

func new_direction():
	var angle = randf() * PI * 2
	direction = Vector2(1, 0).rotated(angle)

func _ready():
	new_direction()
	super._ready()

func update_direction(delta):
	direction_timer -= delta
	if direction_timer <= 0:
		direction_timer = direction_duration
		new_direction()

func _physics_process(delta):
	update_direction(delta)
	super._physics_process(delta)
