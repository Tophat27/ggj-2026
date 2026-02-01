using Godot;
using System;

public partial class SideButton : Area2D
{

	[Export]
	Door door;

	[Export]
	AnimatedSprite2D sprite;
	
	private AudioStreamPlayer2D sideButtonPress;
	
	public override void _Ready()
	{
		sideButtonPress = GetNode<AudioStreamPlayer2D>("SFX_SideButton");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnAreaEntered(Area2D collision)
	{
		if (collision is Bullet && sprite.Animation != "pressed")
		{
			door.open = true;
			sprite.Animation = "pressed";
			sideButtonPress.Play();
		}
	}
}
