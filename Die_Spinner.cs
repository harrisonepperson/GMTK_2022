using Godot;
using System;

public class Die_Spinner : Control
{
	private Spatial rotator;

	private int currentSide = 0;

	[Export]
	private int rotation_speed = 3;

	[Export]
	private PackedScene diceModel;

	public enum DieType
	{
		Action,
		DoubleMovement,
		Movement
	}

	[Export]
	private DieType diceType = DieType.Movement;

	[Export]
	private int indexOffset = 0;

	private int roll_count = 0;

	private int diceNumber = 0;

	private int cachedRemainingMoves = 0;
	private int cachedRemainingActions = 0;

	private Player player = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		int layerIndex = 19 - (GetIndex() + indexOffset);
		GetNode<Camera>("Die_Viewport/Camera").SetCullMaskBit(layerIndex, true);

		rotator = GetNode<Spatial>("Die_Viewport/Rotator");
		MeshInstance die = (MeshInstance)diceModel.Instance();
		die.SetLayerMaskBit(layerIndex, true);

		foreach (Sprite3D s in die.GetChildren())
		{
			s.SetLayerMaskBit(layerIndex, true);
		}

		roll();

		rotator.AddChild(die);

		player = GetNode<Player>("/root/Spatial/Player");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (diceType == DieType.Movement || diceType == DieType.DoubleMovement)
		{
			if (player.remainingMoves != cachedRemainingMoves)
			{
				cachedRemainingMoves = player.remainingMoves;
				var newModulate = new Color(Modulate);
				newModulate.a = cachedRemainingMoves < 1 ? 0.3F : 1F;
				Modulate = newModulate;
			}
		}
		else if (diceType == DieType.Action)
		{
			if (player.remainingActions != cachedRemainingActions)
			{
				cachedRemainingActions = player.remainingActions;
				var newModulate = new Color(Modulate);
				newModulate.a = cachedRemainingActions < 1 ? 0.3F : 1F;
				Modulate = newModulate;
			}
		}

		//Rotate the die.
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

		rotator.RotationDegrees = new Vector3(rotator.RotationDegrees.LinearInterpolate(goal, rotation_speed * delta));
	}

	private void _on_Die_Container_pressed()
	{
		if (player.remainingMoves >= 1 && !player.moveLock && !player.actionLock)
		{
			if (diceType == DieType.Movement)
			{
				handleMovementClick();
			}
			else if (diceType == DieType.DoubleMovement)
			{
				handleDoubleMovementClick();
			}
			else
			{
				handleActionClick();
			}
		}
	}

	private void handleMovementClick()
	{
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

	private void handleDoubleMovementClick()
	{
		switch (currentSide)
		{
			case 1:
				player._on_double_up_pressed();
				break;
			case 2:
				player._on_double_down_pressed();
				break;
			case 3:
				player._on_double_left_pressed();
				break;
			case 4:
				player._on_double_right_pressed();
				break;
			case 5:
				player._on_double_up_pressed();
				break;
			case 6:
				player._on_double_up_pressed();
				break;
		}
		roll();
	}

	private void handleActionClick()
	{
		switch (currentSide)
		{
			case 1:
				player._on_action_heal_pressed();
				break;
			case 2:
				player._on_action_shield_pressed();
				break;
			case 3:
				player._on_action_light_attack_pressed();
				break;
			case 4:
				player._on_action_area_attack_pressed();
				break;
			case 5:
				player._on_action_range_attack_pressed();
				break;
			case 6:
				player._on_action_light_attack_pressed();
				break;
		}
		roll();
	}

	public void roll()
	{
		roll_count++;
		Random rnd = new Random();
		int newSide = currentSide;
		while (newSide == currentSide)
		{
			newSide = rnd.Next(1, 7);
		}

		if (diceType == DieType.Action)
		{
			GD.Print("Rolled: " + newSide);
		}

		currentSide = newSide;
	}
}
