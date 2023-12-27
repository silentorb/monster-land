@tool
class_name Character
extends CharacterBody2D

@export var definition: CharacterDefinition

var sprite

func initialize_sprite():
	if not definition:
		return

	if sprite:
		sprite.frame = definition.sprite

func _ready():
	sprite = get_node("Sprite")
	initialize_sprite()

func _process(delta):
	if Engine.is_editor_hint():
		initialize_sprite()
