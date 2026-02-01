using Godot;
using System;

public partial class Exit : Area2D
{
	[Export]
	Label text;
	public void OnBodyEntered(Node2D collision){
		if (collision.IsInGroup("player"))
		{
			text.LabelSettings.FontColor = new Color(text.LabelSettings.FontColor, 1f); 
		}
	}
}
