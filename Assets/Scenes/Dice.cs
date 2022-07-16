using Godot;
using System;

public class Dice : Spatial
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	[Export]
	public int currentSide = 1;

	// [Export]
	// private NodePath dieContainerPath = null;

	public bool isSpinning = true;

	private int roll_count = 0;

	private Spatial dieContainer = null;
	private Player player = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		dieContainer = GetNode<Spatial>("die-container");
		player = GetNode<Player>("/root/Spatial/Player");
		GD.Print(dieContainer);
		roll();
	}

	public void get_current_side()
	{

	}

	private void on_click()
	{
		GD.Print("rolling...");
		//Take the selected action.
		switch (currentSide)
		{
			case 1:
				player._on_up_pressed();
				break;
			case 2:
				player._on_down_pressed();
				break;
			case 3:
				player._on_left_pressed();
				break;
			case 4:
				player._on_right_pressed();
				break;
			case 5:
				player._on_up_pressed();
				break;
			case 6:
				player._on_up_pressed();
				break;
		}
		roll();
	}

	private void roll()
	{
		//Roll for the next action.
		roll_count++;
		Random rnd = new Random();
		int newSide = currentSide;
		while (newSide == currentSide)
		{
			newSide = rnd.Next(1, 7);
		}

		currentSide = newSide;


		switch (currentSide)
		{
			case 1:
				GD.Print("Rolled a 1: up");
				break;
			case 2:
				GD.Print("Rolled a 2: down");
				break;
			case 3:
				GD.Print("Rolled a 3: left");
				break;
			case 4:
				GD.Print("Rolled a 4: right");
				break;
			case 5:
				GD.Print("Rolled a 5: up");
				break;
			case 6:
				GD.Print("Rolled a 6: up");
				break;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (isSpinning)
		{
			//    var global_pos = global_transform.origin
			// 	var player_pos = player.global_transform.origin

			// 	var wtransform = global_transform.looking_at(Vector3(player_pos.x,global_pos.y,player_pos.z),Vector3(0,1,0))
			// 	var wrotation = Quat(global_transform.basis).slerp(Quat(wtransform.basis), rotation_speed)

			// 	global_transform = Transform(Basis(wrotation), global_transform.origin)

			var rotation_speed = 1 * delta;

			Vector3 targetRotation;

			switch (currentSide)
			{
				case 1:
					targetRotation = new Vector3(0, 0, 0);
					break;
				case 2:
					targetRotation = new Vector3(90, 0, 0);
					break;
				case 3:
					targetRotation = new Vector3(180, 0, 0);
					break;
				case 4:
					targetRotation = new Vector3(270, 0, 0);
					break;
				case 5:
					targetRotation = new Vector3(0, 90, 0);
					break;
				case 6:
					targetRotation = new Vector3(0, -90, 0);
					break;
				default:
					targetRotation = new Vector3(0, 0, 0);
					break;
			}

			var addedRotation = roll_count % 2 == 0 ? 0 : 360;
			var goal = new Vector3(
				targetRotation.x + addedRotation,
				targetRotation.y + addedRotation,
				targetRotation.z + addedRotation
			);


			dieContainer.RotationDegrees = new Vector3(dieContainer.RotationDegrees.LinearInterpolate(goal, rotation_speed));

			//dieContainer.RotationDegrees = new Vector3(dieContainer.Rotation.x, dieContainer.Rotation.y + (delta * 1), dieContainer.Rotation.z);
		}
	}
}



