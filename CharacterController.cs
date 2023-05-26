using Godot;
using System;

public partial class CharacterController : CharacterBody3D
{
    [Export]
    public Vector3 gravity = Vector3.Down;

    public override void _PhysicsProcess(double double_delta)
    {
        base._PhysicsProcess(double_delta);
        float delta = (float)double_delta;
        Velocity += gravity * delta;
        MoveAndSlide();
    }
}
