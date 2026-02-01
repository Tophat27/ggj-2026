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

	[Export]
	bool canpush = false;
	bool ispushing = false;

	[Export]
	bool canShoot = false;
	bool isShooting = false;


private AudioStreamPlayer2D[] walkingSounds;
private int currentWalkIndex = 0;
private float walkTimer = 0.3f;
private float walkDelay = 0.3f;
private AudioStreamPlayer2D playerJumping;
private AudioStreamPlayer2D playerShooting;
private AudioStreamPlayer2D playerTotem;

private float pushTimer = 0f;
private float pushDelay = 0.5f;

public override void _Ready()
{
	walkingSounds = new AudioStreamPlayer2D[8];
	for (int i = 1; i <= 8; i++)
	{
		walkingSounds[i - 1] = GetNode<AudioStreamPlayer2D>($"SFX_WalkingList/SFX_Walking_0{i}");
		walkingSounds[i-1].VolumeDb = -15;
	}
	
	playerJumping = GetNode<AudioStreamPlayer2D>("SFX_Jump");
	playerShooting = GetNode<AudioStreamPlayer2D>("SFX_Shoot");
	playerTotem = GetNode<AudioStreamPlayer2D>("SFX_Totem");
}

	// animations

	public override void _PhysicsProcess(double delta)
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		var jumptimer = GetNode<Timer>("JumpTimer");
		var shoottimer = GetNode<Timer>("ShootTimer");
		var marker = GetNode<Marker2D>("Marker2D");
		var instance = GD.Load<PackedScene>("res://scenes/bullet/bullet.tscn");
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
			isShooting = false;	
			currentjumps -= 1;
			playerJumping.Play();
			walkTimer = 0;
			foreach (var sound in walkingSounds )
			{
				sound.Stop();
			}
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
				walkTimer -= (float)delta;
				 if (walkTimer <= 0)
				 {
					walkingSounds[currentWalkIndex].Play();
					currentWalkIndex = (currentWalkIndex + 1) % walkingSounds.Length;
					walkTimer = walkDelay;
				 }
			}
			animatedSprite2D.FlipH = velocity.X < 0;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			
			if (IsOnFloor() && !isJumping && !isShooting){
				animatedSprite2D.Animation = "idle_" + current_sprite.ToString();
				currentjumps = totaljumps;
				walkTimer = 0;
				foreach (var sound in walkingSounds)
				{
					sound.Stop();
				}
			}
		}

		Velocity = velocity;
		MoveAndSlide();

		if (!isJumping)
		{
			jumptimer.Stop();
		}
		
		if (!isShooting)
		{
			jumptimer.Stop();
		}

		if(ispushing){
			animatedSprite2D.Animation = "push";
		}

		if (canpush && IsOnFloor()){
			getCollision(animatedSprite2D);
		}

		if (canShoot && Input.IsActionJustPressed("shoot") && !isShooting){
			isShooting = true;
			shoottimer.Start();
			playerShooting.Play();
			animatedSprite2D.Animation = "shooting";
			var bullet = (Bullet)instance.Instantiate();

			Vector2 mark = marker.GlobalPosition;

			if (animatedSprite2D.FlipH){
				bullet.speed = -bullet.speed;
				float deslocation = marker.Position.X;
				mark.X = GlobalPosition.X - deslocation;
			}

			bullet.GlobalPosition = mark;

			GetTree().CurrentScene.AddChild(bullet);
			
		}
		// coisa da caixa lol
		if (pushTimer > 0)
		{
			pushTimer -= (float)delta;
		}
	}
	
	
public void getCollision(AnimatedSprite2D animatedSprite2D)
{
	for (int i = 0; i < GetSlideCollisionCount(); i++)
	{
		var c = GetSlideCollision(i);
		if (c.GetCollider() is RigidBody2D rb)
		{
			Vector2 normal = c.GetNormal();
			if (Mathf.Abs(normal.X) > 0.5f)
			{
				ispushing = true;
				float push = PushForce * Velocity.Length() / Speed + MinPush;
				rb.ApplyCentralImpulse(-normal * push);
				
				if (rb.LinearVelocity.Length() > 5)
				{
					if (pushTimer <= 0)
					{
						var pushSound = rb.GetNodeOrNull<AudioStreamPlayer2D>("SFX_Push");
						if (pushSound != null)
						{
							pushSound.Play();
							pushTimer = pushDelay;
						}
					}
				}
			}
		}
		else {
			ispushing = false;
		}
	}
}

	public void OnJumpTimerTimeout()
	{
		isJumping = false;
	}

	public void OnShootTimerTimeout(){
		isShooting = false;
	}

	public void ActivateFoxMask()
	{
		playerTotem.Play();
		current_sprite = 1;
		totaljumps = 2;
		canpush = false;
		canShoot = false;
	}
	
	public void ActivateBearMask(){
		playerTotem.Play();
		current_sprite = 2;
		totaljumps = 1;
		canpush = true;
		canShoot = false;
	}

	public void ActivateHogMask(){
		playerTotem.Play();
		current_sprite = 3;
		totaljumps = 1;
		canpush = false;
		canShoot = true;

	}
}
