extends Node3D

@export
var MOUSE_SENSITIVITY :float = 0.2

var Rope = preload("res://Rope.cs")

@onready
var cam :Camera3D = $Camera3D
@onready
var rope :Node3D = $Rope

func _ready():
	Input.set_mouse_mode(Input.MOUSE_MODE_HIDDEN)
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
	rope.affectedBody = get_parent();


func _input(event :InputEvent):
	# This section controls your player camera. Sensitivity can be changed.
	if event is InputEventMouseMotion and Input.get_mouse_mode() == Input.MOUSE_MODE_CAPTURED:
		cam.rotate_x(event.relative.y * MOUSE_SENSITIVITY/100 * -1)
		self.rotate_y(event.relative.x * MOUSE_SENSITIVITY/100 * -1)

		var camera_rot = cam.rotation
		camera_rot.x = clampf(camera_rot.x, -1.4, 1.4)
		cam.rotation = camera_rot
	
	if Input.is_action_just_pressed("DebugMode"):
		if Input.get_mouse_mode() == Input.MOUSE_MODE_VISIBLE:
			Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
		else:
			Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
	
	if Input.is_action_just_pressed("Rope"):
		var dir := cam.global_transform.basis.y - cam.global_transform.basis.z
		var point :Vector3 = cam.global_position +dir.normalized()*rope.baseLength
		rope.AttachTo(point)
	if Input.is_action_just_released("Rope"):
		rope.Sever()
