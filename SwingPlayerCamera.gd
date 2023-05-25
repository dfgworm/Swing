extends Node3D

@export
var MOUSE_SENSITIVITY :float = 0.06
@export
var ropeLength :float = 5

@onready
var cam :Camera3D = $Camera3D
@onready
var swingController :CharacterBody3D = get_parent()

func _ready():
	Input.set_mouse_mode(Input.MOUSE_MODE_HIDDEN)
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)


func _input(event):
	# This section controls your player camera. Sensitivity can be changed.
	if event is InputEventMouseMotion and Input.get_mouse_mode() == Input.MOUSE_MODE_CAPTURED:
		cam.rotate_x(deg_to_rad(event.relative.y * MOUSE_SENSITIVITY * -1))
		self.rotate_y(deg_to_rad(event.relative.x * MOUSE_SENSITIVITY * -1))

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
		var point := cam.global_position +dir.normalized()*ropeLength
		swingController.StartSwinging(point) 
	if Input.is_action_just_released("Rope"):
		swingController.StopSwinging()
