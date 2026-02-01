using Godot;
using System;

public partial class Door : StaticBody2D
{
	[Export]
	public bool open = false;
	int current = 0;
	
	private AudioStreamPlayer2D openDoor;
	private AudioStreamPlayer2D closeDoor;
	
	public override void _Ready()
	{
		openDoor = GetNode<AudioStreamPlayer2D>("SFX_OpenDoor");
		closeDoor = GetNode<AudioStreamPlayer2D>("SFX_CloseDoor");
	}
	
	public override void _Process(double delta)
	{
		var animator = GetNode<AnimationPlayer>("AnimationPlayer");

		if (open)
		{
			if (current == 0)
			{
				animator.Play("open");
				current = 1;
				openDoor.Play();
			}
			
		}

		else
		{
			if (current == 1)
			{
				animator.Play("close");
				current = 0;
				closeDoor.Play();
			}
		}
	}

	public void OpenClose()
	{
		var collision = GetNode<CollisionShape2D>("CollisionShape2D");
		var sprite = GetNode<Sprite2D>("Sprite2D");

		if (current == 1)
		{
			collision.Disabled = true;
			sprite.Visible = false;
		} else
		{
			collision.Disabled = false;
			sprite.Visible = true;
		}
	}

}
