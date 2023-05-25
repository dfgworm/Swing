using Godot;
using System;

public partial class CharacterController : CharacterBody3D
{
    [Export]
    public Vector3 gravity = Vector3.Down;
    [Export]
    public float ropeForcePerStretchUnit = 1.2f;

    bool isSwinging = false;
    Vector3 ropeAttachmentPoint;
    float startingRopeLength;
    public override void _PhysicsProcess(double double_delta)
    {
        base._PhysicsProcess(double_delta);
        float delta = (float)double_delta;
        if (isSwinging)
            UpdateSwinging(delta);
        else
            UpdateRegular(delta);
       MoveAndSlide(); // You dont need arguments here, it uses Velocity
    }
    void UpdateSwinging(float delta) {
        Velocity += gravity * delta;
        
        Vector3 currentDirection = ropeAttachmentPoint - GlobalPosition;
        float currentLength = currentDirection.Length();
        Vector3 ropeForceDirection = currentDirection.Normalized();

        // Calculate the force to stretch the rope
        float ropeStretch = Mathf.Max(0, currentLength - startingRopeLength);
        float ropeForceMagnitude = ropeForcePerStretchUnit * ropeStretch;
        Vector3 ropeForce = ropeForceDirection * ropeForceMagnitude;

        // Apply the rope force
        Velocity += ropeForce * delta;
    }
    void UpdateRegular(float delta) {
        Velocity += gravity * delta;
    }
    public void StartSwinging(Vector3 attachment)
    {
        isSwinging = true;
        ropeAttachmentPoint = attachment;
        startingRopeLength = (attachment - GlobalPosition).Length();
    }

    public void StopSwinging()
    {
        isSwinging = false;
    }
}
