using Godot;
using System;

public class Player : RigidBody
{
	private Autoload autoload;
	private int gridSize;

	private Vector3 targetPos;

	public int remainingActions;
	public int remainingMoves;

	public bool moveLock = false;
	public bool actionLock = false;

	[Export]
	private int maxHealth = 5;

	[Export]
	private int slideSpeed = 5;

	[Export]
	private int defaultActionsPerTurn = 1;

	[Export]
	public int defaultMovesPerTurn = 2;

	[Export]
	private int lightDamage = 2;

	[Export]
	private int aoeDamage = 1;

	[Export]
	private int rangeDamage = 1;

	public int numActions = 3;

	private bool isDead = false;

	private string deathScreen = "res://Assets/Scenes/Outro.tscn";

	private Timer actionTimer;

	AudioStreamPlayer lightAttackSound;
	AudioStreamPlayer aoeAttackSound;
	AudioStreamPlayer rangedAttackSound;
	AudioStreamPlayer healSound;
	AudioStreamPlayer shieldSound;

	public override void _Ready()
	{
		GD.Load<PackedScene>(deathScreen);
		actionTimer = GetNode<Timer>("ActionLock");

		autoload = GetNode<Autoload>("/root/Autoload");
		gridSize = autoload.gridSize;
		targetPos = Translation;

		remainingActions = defaultActionsPerTurn;
		remainingMoves = defaultMovesPerTurn;

		lightAttackSound = GetNode<AudioStreamPlayer>("LightAttackSound");
		aoeAttackSound = GetNode<AudioStreamPlayer>("AOEAttackSound");
		rangedAttackSound = GetNode<AudioStreamPlayer>("RangedAttackSound");
		healSound = GetNode<AudioStreamPlayer>("HealSound");
		shieldSound = GetNode<AudioStreamPlayer>("ShieldSound");
	}

	public override void _Process(float delta)
	{


		// if (!autoload.levelComplete)
		// {
		// 	Godot.Collections.Array enemies = GetTree().GetNodesInGroup("enemy");
		// 	var textLevelComplete = true;
		// 	foreach (Enemy e in enemies)
		// 	{
		// 		if (!e.isDead)
		// 		{
		// 			textLevelComplete = false;
		// 		}
		// 	}
		// 	if (textLevelComplete)
		// 	{
		// 		autoload.levelComplete = true;
		// 		//GetNode<Control>("CanvasLayer/FullScreen/Level Complete UI").Visible = true;
		// 	}
		// }

		Label turnLabel = GetNode<Label>("CanvasLayer/FullScreen/Turn");
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

		if (!actionLock && isDead)
		{
			GetTree().ChangeScene(deathScreen);
		}

		if (autoload.isPlayerTurn && !actionLock && remainingActions == 0 && remainingMoves == 0)
		{
			autoload.finishPlayerTurn();
		}
	}

	public void startTurn()
	{
		autoload.isPlayerTurn = true;
		remainingActions = defaultActionsPerTurn;
		remainingMoves = defaultMovesPerTurn;
	}

	public void damage(int hits)
	{
		if (autoload.playerShield > 0)
		{
			hits = Math.Abs(hits - autoload.playerShield);
			autoload.playerShield = 0;
		}

		autoload.playerHealth -= hits;

		if (autoload.playerHealth <= 0)
		{
			GetNode<Spatial>("knight").Visible = false;
			GetNode<Spatial>("gravestone").Visible = true;

			isDead = true;

			handle_action(3F);
		}
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
		actionLock = true;
		actionTimer.WaitTime = waitTime;
		actionTimer.Start();
		numActions--;
	}

	public void _on_action_heal_pressed()
	{
		if (autoload.isPlayerTurn && !moveLock && !actionLock && remainingActions > 0)
		{
			healSound.Play();
			autoload.playerHealth = Math.Min(maxHealth, autoload.playerHealth + 1);
			GetNode<Particles>("Health_Particles").Emitting = true;
			handle_action();
			remainingActions--;
		}
	}

	public void _on_action_shield_pressed()
	{
		if (autoload.isPlayerTurn && !moveLock && !actionLock && remainingActions > 0)
		{
			shieldSound.Play();
			autoload.playerShield++;
			GetNode<Particles>("Shield_Particles").Emitting = true;
			handle_action();
			remainingActions--;
		}
	}

	public void _on_action_light_attack_pressed()
	{
		if (autoload.isPlayerTurn && !moveLock && !actionLock && remainingActions > 0)
		{
			lightAttackSound.Play();
			Particles ball = GetNode<Particles>("Light_Attack_Particles");

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

				if (closestDist <= 8)
				{
					ball.Translation = new Vector3(closestEnemy.Translation.x - Translation.x, 2, closestEnemy.Translation.z - Translation.z);
					ball.Emitting = true;
					closestEnemy.damage(lightDamage);
				}
			}

			handle_action();
			remainingActions--;
			return;
		}
	}

	public void _on_action_area_attack_pressed()
	{
		if (autoload.isPlayerTurn && !moveLock && !actionLock && remainingActions > 0)
		{
			aoeAttackSound.Play();
			Particles aoe = GetNode<Particles>("AOE_Attack_Particles");

			Godot.Collections.Array enemies = GetTree().GetNodesInGroup("enemy");
			if (enemies.Count > 0)
			{
				aoe.Emitting = true;
				foreach (Enemy e in enemies)
				{
					if (!e.isDead)
					{
						float dist = Translation.DistanceSquaredTo(e.Translation);

						if (dist <= 8)
						{
							e.damage(aoeDamage);
						}
					}
				}
			}

			handle_action();
			remainingActions--;
			return;
		}
	}

	public void _on_action_range_attack_pressed()
	{
		if (autoload.isPlayerTurn && !moveLock && !actionLock && remainingActions > 0)
		{
			rangedAttackSound.Play();
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
					closestEnemy.damage(rangeDamage);
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
