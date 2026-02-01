using Godot;

public partial class Bullet : Area2D 
{
	[Export] public float speed = 300.0f;

	public override void _Process(double delta)
	{
		Position += Transform.X.Normalized() * speed * (float)delta;
	}

	public void OnBodyEntered(Node2D collision)
	{
		if (collision.IsInGroup("box"))
		{
			QueueFree();
		}
	}

	public void Destroy()
	{
		QueueFree();
	}


}
