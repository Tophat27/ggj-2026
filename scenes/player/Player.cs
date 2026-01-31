using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	bool isJumping = false;

	public override void _PhysicsProcess(double delta)
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		var jumptimer = GetNode<Timer>("JumpTimer");
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
			if (!isJumping)
			{
				animatedSprite2D.Animation = "falling";
			}
		}

		if (IsOnFloor() && isJumping)
		{
			isJumping = false;
			animatedSprite2D.Animation = "landing";
			GD.Print("im here");
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
			animatedSprite2D.Animation = "jump";
			jumptimer.Start();	
			isJumping = true;	
		}


		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			
			if (IsOnFloor() && !isJumping){
				animatedSprite2D.Animation = "walk";
			}
			animatedSprite2D.FlipH = velocity.X < 0;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			
			if (IsOnFloor() && !isJumping){
				animatedSprite2D.Animation = "idle";
			}
		}

		Velocity = velocity;
		MoveAndSlide();
		GD.Print(animatedSprite2D.Animation);

		if (!isJumping)
		{
			jumptimer.Stop();
		}
	}

	public void OnJumpTimerTimeout()
	{
		isJumping = false;
	}

}
