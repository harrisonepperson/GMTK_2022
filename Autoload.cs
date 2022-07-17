using Godot;
using System;

public class Autoload : Node
{
	public int gridSize = 2;
	public bool isPlayerTurn = true;
	public int score = 0;

	public int playerHealth = 4;
	public int playerShield = 0;

	public bool levelComplete = false;

	[Signal]
	public delegate void playerFinishedTurnSignal();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public override void _Process(float delta)
	{
		if (!isPlayerTurn)
		{

			Godot.Collections.Array enemies = GetTree().GetNodesInGroup("enemy");
			bool enemyStillPlaying = false;

			foreach (Enemy e in enemies)
			{
				//				GD.Print("Found enemy in list", e.isEnemyTurn);
				if (!e.isDead && e.isEnemyTurn)
				{
					//					GD.Print("Enemy reported still in progress");
					enemyStillPlaying = true;
				}
			}

			if (!enemyStillPlaying)
			{
				isPlayerTurn = true;
				GetTree().CallGroup("player", "startTurn");
			}
		}
	}

	public void finishPlayerTurn()
	{
		if (isPlayerTurn)
		{
			isPlayerTurn = false;
			GetTree().CallGroup("enemy", "startTurn");
		}
	}
}
