using Godot;
using System;

public partial class SideButton : Area2D
{

	[Export]
	Door door;

	[Export]
	AnimatedSprite2D sprite;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnAreaEntered(Area2D collision)
	{
		if (collision is Bullet)
		{
			door.open = true;
			sprite.Animation = "pressed";
		}
	}
}
