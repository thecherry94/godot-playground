using Godot;
using System;

public partial class Character : CharacterBody2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		bool Test = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_up"))
		{
			this.Position = new Vector2(this.Position.X, this.Position.Y - 1);
		}
		if (@event.IsActionPressed("ui_down"))
		{
			this.Position = new Vector2(this.Position.X, this.Position.Y + 1);
		}
		if (@event.IsActionPressed("ui_left"))
		{
			this.Position = new Vector2(this.Position.X - 1, this.Position.Y);
		}
		if (@event.IsActionPressed("ui_right"))
		{
			this.Position = new Vector2(this.Position.X + 1, this.Position.Y);
		}

		base._Input(@event);
    }

}
