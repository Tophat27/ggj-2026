using Godot;
using System;

public partial class Door : StaticBody2D
{
	[Export]
	bool open = false;
	int current = 0;
	public override void _Process(double delta)
	{
		var animator = GetNode<AnimationPlayer>("AnimationPlayer");

		if (open)
		{
			if (current == 0)
			{
				animator.Play("open");
				current = 1;
			}
			
		}

		else
		{
			if (current == 1)
			{
				animator.Play("close");
				current = 0;
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
