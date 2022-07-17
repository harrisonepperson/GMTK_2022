using Godot;
using System;

public class LevelsCleared : Label
{
	private int cachedScore = -1;
	private Autoload autoload;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		autoload = GetNode<Autoload>("/root/Autoload");
	}

	public override void _Process(float delta)
	{
		if (autoload.score != cachedScore)
		{
			GD.Print("Score changed to: " + autoload.score);
			cachedScore = autoload.score;
			Text = "Levels Cleared: " + cachedScore;
		}
	}
}
