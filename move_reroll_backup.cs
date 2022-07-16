//using Godot;
//using System;
//
//public class move_reroll : TextureButton
//{
//	private PackedScene movementDieScene;
//	private PackedScene movementDieModel;
//	private GridContainer movementGrid;
//
//	// Called when the node enters the scene tree for the first time.
//	public override async void _Ready()
//	{
//		yield(get_tree(), "idle_frame");
//		movementDieScene = (PackedScene) ResourceLoader.Load("res://Die_Spinner.tscn");
//		movementDieModel = (PackedScene) ResourceLoader.Load("res://MovementDie.tscn");
//
//		movementGrid = GetNode<GridContainer>("MovementGrid");
//
//		Godot.Collections.Array panels = movementGrid.GetChildren();
//
//		GD.Print(panels, panels.Count);
//
//		for (int i = 0; i < panels.Count; i++)
//		{
//			Panel p = (Panel) panels[i];
//			Die_Spinner dieInstance = (Die_Spinner) movementDieScene.Instance();
//			dieInstance.setDice(movementDieModel, Die_Spinner.DieType.Movement, 20 - i);
//			p.AddChild(dieInstance);
//		}
//	}
//
//	private void _on_move_reroll_pressed()
//	{
//		GD.Print('Pressed reroll');
//		foreach(Panel p in movementGrid.GetChildren())
//		{
//			((Die_Spinner) p.GetChild(0)).roll();
//		}
//	}
//}
//
//
//
