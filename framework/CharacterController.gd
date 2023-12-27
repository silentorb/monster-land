class_name CharacterController
extends Node

var character: Character
var direction = Vector2(0, 0)

func _ready():
	character = get_parent()

func _physics_process(delta):
	if not character:
		return

	var speed = character.definition.speed
	if direction:
		character.velocity = direction.normalized() * speed
	else:
		character.velocity = character.velocity.move_toward(Vector2.ZERO, speed)

	character.move_and_slide()
