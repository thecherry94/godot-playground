using Godot;
using System;
using System.IO;

public partial class Dude : CharacterBody2D
{
	[Export]
	Node2D Target;

	Timer navigationTimer;

	NavigationAgent2D navigationAgent2D;
	float maxSpeed = 300;
	float acceleration = 800; // How quickly we accelerate
	float deceleration = 1200; // How quickly we brake (increased for better stopping)
	float arrivalDistance = 50; // Distance at which we start braking (increased)
	float stopDistance = 5; // Distance at which we stop completely
	Vector2 direction;
	float currentSpeed = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		navigationAgent2D = GetNode<NavigationAgent2D>("NavigationAgent2D");
		navigationAgent2D.TargetPosition = Target.GlobalPosition;

		// Create and configure the navigation timer
		navigationTimer = new Timer();
		navigationTimer.WaitTime = 0.1; // Update navigation every 100ms
		navigationTimer.Autostart = true;
		navigationTimer.Timeout += OnNavTimerTimeout;
		AddChild(navigationTimer);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		// Calculate distance to final target
		float distanceToTarget = GlobalPosition.DistanceTo(navigationAgent2D.TargetPosition);
		
		// Stop completely if very close to target
		if (distanceToTarget <= stopDistance || navigationAgent2D.IsNavigationFinished())
		{
			currentSpeed = 0;
			Velocity = Vector2.Zero;
			MoveAndSlide();
			return;
		}

		// Get direction to next waypoint
		Vector2 nextPosition = navigationAgent2D.GetNextPathPosition();
		direction = (nextPosition - GlobalPosition).Normalized();
		
		// Determine target speed based on distance to target
		float targetSpeed;
		if (distanceToTarget <= arrivalDistance)
		{
			// Calculate safe speed for the remaining distance using physics
			// v = sqrt(2 * a * d) - speed needed to stop in given distance
			float safeSpeed = Mathf.Sqrt(2 * deceleration * (distanceToTarget - stopDistance));
			safeSpeed = Mathf.Min(safeSpeed, maxSpeed);
			targetSpeed = Mathf.Max(safeSpeed, 0); // Ensure non-negative
		}
		else
		{
			// Accelerate to max speed when far from target
			targetSpeed = maxSpeed;
		}
		
		// Smoothly adjust current speed towards target speed
		if (currentSpeed < targetSpeed)
		{
			// Accelerate
			currentSpeed = Mathf.MoveToward(currentSpeed, targetSpeed, acceleration * (float)delta);
		}
		else
		{
			// Decelerate
			currentSpeed = Mathf.MoveToward(currentSpeed, targetSpeed, deceleration * (float)delta);
		}
		
		// Apply velocity
		Vector2 targetVelocity = direction * currentSpeed;
		Velocity = targetVelocity;
		
		MoveAndSlide();
	}

	public void OnNavTimerTimeout() => navigationAgent2D.TargetPosition = Target.GlobalPosition;

}
