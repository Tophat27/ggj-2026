using Godot;
using System;

public partial class PressedButton : Area2D
{

	[Export]
	Door door;

	[Export]
	AnimatedSprite2D sprite;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnBodyEntered(Node2D collision)
	{
		if (collision.IsInGroup("box") || collision.IsInGroup("player"))
		{
			door.open = true;
			sprite.Animation = "pressed";
		}
	}

	public void OnAreaExited(Node2D collision)
	{
		door.open = false;
		sprite.Animation = "idle";
	}
}
