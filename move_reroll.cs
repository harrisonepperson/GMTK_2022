using Godot;
using System;

public class move_reroll : TextureButton
{
	private PackedScene movementDieScene;
	private PackedScene movementDieModel;
	private GridContainer movementGrid;
	private Player player = null;

	private int cachedRemainingMoves = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		movementGrid = GetNode<GridContainer>("../MovementGrid");

		player = GetNode<Player>("/root/Main/Player");
	}


	public override void _Process(float delta)
	{
		if (player.remainingMoves != cachedRemainingMoves)
		{
			cachedRemainingMoves = player.remainingMoves;

			var newModulate = new Color(Modulate);
			newModulate.a = cachedRemainingMoves < player.defaultMovesPerTurn ? 0.3F : 1F;
			Modulate = newModulate;
		}
	}

	private void _on_move_reroll_pressed()
	{
		if (cachedRemainingMoves == player.defaultMovesPerTurn)
		{
			foreach (Die_Spinner dS in movementGrid.GetChildren())
			{
				dS.roll();
				player.remainingMoves = 0;
			}
		}
	}
}
