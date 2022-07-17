using Godot;
using System;

public class Floor : Spatial
{
	private Random rnd = new Random();

	private Autoload autoload;

	[Export]
	private PackedScene enemyScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		autoload = GetNode<Autoload>("/root/Autoload");
		if (rnd.Next(0, 1 + (int) Math.Floor((double) 10 / (1 + autoload.score))) == 0) {
			Enemy enemyInstance = (Enemy) enemyScene.Instance();
			AddChild(enemyInstance);
		}
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
