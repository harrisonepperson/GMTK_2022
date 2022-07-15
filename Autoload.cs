using Godot;
using System;

public class Autoload : Node
{
	public int gridSize = 3;
	public bool isPlayerTurn = true;
	public int score = 0;

	public int playerHealth = 5;
	public int playerShield = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
