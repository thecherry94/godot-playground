using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 750.0f;
	float dt = 0;

	public override void _Input(InputEvent @event)
	{
		if (@event.IsAction("ui_up"))
		{
			this.Position = new Vector2(this.Position.X, this.Position.Y - Speed * dt);
		}
		if (@event.IsAction("ui_down"))
		{
			this.Position = new Vector2(this.Position.X, this.Position.Y + Speed * dt);
		}
		if (@event.IsAction("ui_left"))
		{
			this.Position = new Vector2(this.Position.X - Speed * dt, this.Position.Y);
		}
		if (@event.IsAction("ui_right"))
		{
			this.Position = new Vector2(this.Position.X + Speed * dt, this.Position.Y);
		}

		base._Input(@event);
	}

	public override void _PhysicsProcess(double delta)
	{
		dt = (float)delta;
    }

}
