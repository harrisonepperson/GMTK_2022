using Godot;
using System;

public class move_reroll : TextureButton
{
	private PackedScene movementDieScene;
	private PackedScene movementDieModel;
	private GridContainer movementGrid;

	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		movementGrid = GetNode<GridContainer>("../MovementGrid");
	}
	
	private void _on_move_reroll_pressed()
	{
		foreach(Die_Spinner dS in movementGrid.GetChildren())
		{
			dS.roll();
		}
	}
}
