using Godot;
using System;


public partial class Rope : Node3D
{
    [Export]
    public CharacterBody3D affectedBody;
    [Export]
    public Vector3 attachmentPoint;
    [Export]
    public float baseLength;
    [Export]
    public float forcePerStretchUnit = 10;
    [Export]
    public bool enabled = true;
    public override void _PhysicsProcess(double doubledelta)
    {
        base._PhysicsProcess(doubledelta);
        float delta = (float)doubledelta;
        if (enabled)
            UpdateSwinging(delta);
    }
    void UpdateSwinging(float delta) {
        Vector3 currentDirection = attachmentPoint - GlobalPosition;
        float currentLength = currentDirection.Length();
        Vector3 ropeForceDirection = currentDirection.Normalized();

        // Calculate the force to stretch the rope
        float ropeStretch = Mathf.Max(0, currentLength - baseLength);
        float ropeForceMagnitude = forcePerStretchUnit * ropeStretch;
        Vector3 ropeForce = ropeForceDirection * ropeForceMagnitude;

        // Apply the rope force
        affectedBody.Velocity += ropeForce * delta;
    }
    public void AttachTo(Vector3 point) {
        attachmentPoint = point;
        enabled = true;
    }
    public void Sever() {
        enabled = false;
    }
}