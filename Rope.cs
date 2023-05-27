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
    public bool enabled = false;
    Node3D mesh;
    Node3D jointMesh;
    public override void _Ready()
    {
        base._Ready();
        mesh = (Node3D)this.FindChild("RopeMesh");
        this.RemoveChild(mesh);
        jointMesh = (Node3D)this.FindChild("JointMesh");
        this.RemoveChild(jointMesh);
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
        if (!enabled) {
            if (this.IsAncestorOf(mesh))
                this.RemoveChild(mesh);
            if (this.IsAncestorOf(jointMesh))
                this.RemoveChild(jointMesh);
            return;
        }
        if (!this.IsAncestorOf(mesh))
            this.AddChild(mesh);
        if (!this.IsAncestorOf(jointMesh))
            this.AddChild(jointMesh);
        jointMesh.GlobalPosition = attachmentPoint;
        mesh.GlobalPosition = (GlobalPosition + attachmentPoint)/2;
        Vector3 dir = attachmentPoint - GlobalPosition;
        Vector3 cross = dir.Cross(Vector3.Down);
        if (cross == Vector3.Zero)
            cross = Vector3.Left;
        mesh.GlobalTransform = mesh.GlobalTransform.LookingAt(mesh.GlobalPosition+cross, dir);
        mesh.Scale = new Vector3(1, dir.Length(),1);
    }
    public override void _PhysicsProcess(double doubledelta)
    {
        base._PhysicsProcess(doubledelta);
        float delta = (float)doubledelta;
        if (enabled)
            UpdateRopePull(delta);
    }
    void UpdateRopePull(float delta) {
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