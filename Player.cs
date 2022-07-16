using Godot;
using System;

public class Player : RigidBody
{
	private Autoload autoload;
	private int gridSize;

	private Vector3 targetPos;

	private int remainingActions;
	private int remainingMoves;

	private bool moveLock = false;
	private bool actionLock = false;

	[Export]
	private int maxHealth = 5;

	[Export]
	private int slideSpeed = 5;

	[Export]
	private int defaultActionsPerTurn = 1;

	[Export]
	private int defaultMovesPerTurn = 2;

	public override void _Ready()
	{
		autoload = GetNode<Autoload>("/root/Autoload");
		gridSize = autoload.gridSize;
		targetPos = Translation;

		remainingActions = defaultActionsPerTurn;
		remainingMoves = defaultMovesPerTurn;
	}

	public override void _Process(float delta)
	{
		Label turnLabel = GetNode<Label>("Camera_Holder/Camera/FullScreen/Turn");
		if (autoload.isPlayerTurn)
		{
			turnLabel.Text = "PLAYER TURN";
		}
		else
		{
			turnLabel.Text = "ENEMY TURN";
		}

		if (moveLock)
		{
			Translation = Translation.LinearInterpolate(targetPos, slideSpeed * delta);

			if (Translation.DistanceSquaredTo(targetPos) <= 0.001)
			{
				Translation = targetPos;
				moveLock = false;
			}
		}

		if (autoload.isPlayerTurn && !actionLock && remainingActions == 0 && remainingMoves == 0)
		{
			autoload.finishPlayerTurn();
		}
	}

	public void startTurn()
	{
		remainingActions = defaultActionsPerTurn;
		remainingMoves = defaultMovesPerTurn;
	}

	private void kill()
	{
		Sleeping = false;
	}

	private void handle_move(Vector3 offset)
	{
		if (autoload.isPlayerTurn && !moveLock && !actionLock && remainingMoves > 0)
		{
			targetPos = new Vector3(Translation.x + offset.x, Translation.y, Translation.z + offset.z);
			moveLock = true;
			remainingMoves--;
		}
	}

	public void _on_up_pressed()
	{
		handle_move(new Vector3(0, 0, 0 - gridSize));
	}

	public void _on_left_pressed()
	{
		handle_move(new Vector3(0 - gridSize, 0, 0));
	}

	public void _on_down_pressed()
	{
		handle_move(new Vector3(0, 0, gridSize));
	}

	public void _on_right_pressed()
	{
		handle_move(new Vector3(gridSize, 0, 0));
	}

	public void _on_double_up_pressed()
	{
		handle_move(new Vector3(0, 0, 0 - 2 * gridSize));
	}

	public void _on_double_left_pressed()
	{
		handle_move(new Vector3(0 - 2 * gridSize, 0, 0));
	}

	public void _on_double_down_pressed()
	{
		handle_move(new Vector3(0, 0, 2 * gridSize));
	}

	public void _on_double_right_pressed()
	{
		handle_move(new Vector3(2 * gridSize, 0, 0));
	}


	private void handle_action(float waitTime = 1.5F)
	{
		Timer actionTimer = GetNode<Timer>("ActionLock");
		actionLock = true;
		actionTimer.WaitTime = waitTime;
		actionTimer.Start();
	}

	private void _on_action_heal_pressed()
	{
		if (autoload.isPlayerTurn && !moveLock && !actionLock && remainingActions > 0)
		{
			autoload.playerHealth = Math.Min(maxHealth, autoload.playerHealth + 1);
			GetNode<Particles>("Health_Particles").Emitting = true;
			handle_action();
			remainingActions--;
		}
	}

	private void _on_action_shield_pressed()
	{
		if (autoload.isPlayerTurn && !moveLock && !actionLock && remainingActions > 0)
		{
			autoload.playerShield++;
			GetNode<Particles>("Shield_Particles").Emitting = true;
			handle_action();
			remainingActions--;
		}
	}

	private void _on_action_light_attack_pressed()
	{
		if (autoload.isPlayerTurn && !moveLock && !actionLock && remainingActions > 0)
		{
			handle_action();
			remainingActions--;
			return;
		}
	}

	private void _on_action_area_attack_pressed()
	{
		if (autoload.isPlayerTurn && !moveLock && !actionLock && remainingActions > 0)
		{
			handle_action();
			remainingActions--;
			return;
		}
	}

	private void _on_action_range_attack_pressed()
	{
		if (autoload.isPlayerTurn && !moveLock && !actionLock && remainingActions > 0)
		{
			Range_Attack_Particles fireBall = GetNode<Range_Attack_Particles>("Range_Attack_Particles");

			Godot.Collections.Array enemies = GetTree().GetNodesInGroup("enemy");

			if (enemies.Count > 0)
			{
				Enemy closestEnemy = (Enemy)enemies[0];
				float closestDist = Translation.DistanceSquaredTo(closestEnemy.Translation);

				foreach (Enemy e in enemies)
				{
					if (!e.isDead)
					{
						float enemDist = Translation.DistanceSquaredTo(e.Translation);
						if (enemDist < closestDist)
						{
							closestEnemy = e;
							closestDist = enemDist;
						}
					}
				}

				if (closestDist <= 36)
				{
					fireBall.castTo(new Vector3(closestEnemy.Translation.x - Translation.x, 2, closestEnemy.Translation.z - Translation.z));
					closestEnemy.damage(1);
				}
			}

			handle_action();
			remainingActions--;
			return;
		}
	}

	private void _on_end_turn_pressed()
	{
		autoload.finishPlayerTurn();
		remainingActions = 0;
		remainingMoves = 0;
	}

	private void _on_ActionLock_timeout()
	{
		actionLock = false;
	}
}
