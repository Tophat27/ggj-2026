using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public float Speed = 300.0f;
	[Export]
	public float JumpVelocity = -400.0f;

	[Export]
	public float PushForce = 15f;
	public float MinPush = 10f;

	[Export]
	int current_sprite = 0; // 0 - player, 1 - raposa, 2 - ala ursa, 3 - ouriço
	bool isJumping = false;
	[Export]
	int totaljumps = 1;
	int currentjumps = 1;

	[Export]
	CollisionShape2D force;

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
				animatedSprite2D.Animation = "falling_" + current_sprite.ToString();
			}
		}

		if (IsOnFloor() && isJumping)
		{
			isJumping = false;
			animatedSprite2D.Animation = "landing_" + current_sprite.ToString();
			GD.Print("im here");
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && (IsOnFloor() || currentjumps > 0))
		{
			velocity.Y = JumpVelocity;
			animatedSprite2D.Animation = "jump_" + current_sprite.ToString();
			jumptimer.Start();	
			isJumping = true;	
			currentjumps -= 1;
		}


		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			
			if (IsOnFloor() && !isJumping){
				animatedSprite2D.Animation = "walk_" + current_sprite.ToString();
				currentjumps = totaljumps;
			}
			animatedSprite2D.FlipH = velocity.X < 0;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			
			if (IsOnFloor() && !isJumping){
				animatedSprite2D.Animation = "idle_" + current_sprite.ToString();
				currentjumps = totaljumps;
			}
		}

		Velocity = velocity;
		MoveAndSlide();

		if (!isJumping)
		{
			jumptimer.Stop();
		}

		getCollision();
	}
	
	
	public void getCollision(){
		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			var c = GetSlideCollision(i);
			if (c.GetCollider() is RigidBody2D rb)
			{
				float push = PushForce * Velocity.Length() / Speed + MinPush;
				rb.ApplyCentralImpulse(-c.GetNormal() * push);
			} 
		}
	}

	public void OnJumpTimerTimeout()
	{
		isJumping = false;
	}

	public void ActivateFoxMask()
	{
		current_sprite = 1;
		totaljumps = 2;
		force.Disabled = true;
	} 
	
	public void ActivateBearMask(){
		current_sprite = 2;
		totaljumps = 1;
		force.Disabled = false;
	}
}
