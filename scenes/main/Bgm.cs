using Godot;
using System;

public partial class Bgm : AudioStreamPlayer2D
{
	private bool backgroundMusicOn = true;

	public override void _Process(double delta)
	{
		UpdateMusicStats();
	}

	private void UpdateMusicStats()
	{
		if (backgroundMusicOn)
		{
			if (!Playing)
			{
				Play();
			}
		}
		else
		{
			Stop();
		}
	}
}
