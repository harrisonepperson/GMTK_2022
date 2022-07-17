using Godot;
using System;

public class Enemy : RigidBody
{
	public bool isEnemyTurn = false;
	
	private Autoload autoload;
	private int gridSize;
	
	private Vector3 targetPos;
	
	private int remainingActions;
	private int remainingMoves;
	
	private bool moveLock = false;
	private bool actionLock = false;
	
	private Random rnd = new Random();
	
	private int health;
	public bool isDead = false;

	[Export]
	private int maxHealth = 1;
	
	[Export]
	private int slideSpeed = 5;

	[Export]
	private int defaultActionsPerTurn = 1;
	
	[Export]
	private int defaultMovesPerTurn = 2;
	
	[Export]
	private int sightDistance = 10;

	public override void _Ready()
	{
		autoload = GetNode<Autoload>("/root/Autoload");
		gridSize = autoload.gridSize;
		targetPos = Translation;
		
		remainingActions = defaultActionsPerTurn;
		remainingMoves = defaultMovesPerTurn;
		
		health = maxHealth;
	}

	public override void _Process(float delta)
	{
		if(isEnemyTurn && !isDead) {
			if (moveLock) {
				Translation = Translation.LinearInterpolate(targetPos, slideSpeed * delta);

				if (Translation.DistanceSquaredTo(targetPos) <= 0.001 ) {
					Translation = targetPos;
					moveLock = false;
				}
			}

			if (actionLock) {
				actionLock = false;
			}

			if (!moveLock && !actionLock) {
				if (remainingMoves + remainingActions > 0) {
					GD.Print(targetPos);
					handleTurn();
				} else {
					isEnemyTurn = false;
				}
			}
		}
	}
	
	public void damage(int hits)
	{
		health -= hits;
		
		if (health <= 0) {
			isDead = true;
			
			GetNode<Spatial>("skeleton").Visible = false;
			GetNode<Spatial>("gravestone").Visible = true;
			
//			Sleeping = false;
//			ApplyImpulse(Vector3.Forward, new Vector3(0, 2, 0));
		}
	}

	public void handleMove(Vector2 offset)
	{
//		GD.Print("Handling Enemy Move");
		targetPos = new Vector3(Translation.x + (offset.x * gridSize), Translation.y, Translation.z + (offset.y * gridSize));
		moveLock = true;
		remainingMoves --;
	}
	
	public void handleAttack()
	{
//		GD.Print("Handling Enemy Attack");
		actionLock = true;
		remainingActions --;
	}
	
	public void startTurn()
	{
		remainingActions = defaultActionsPerTurn;
		remainingMoves = defaultMovesPerTurn;
		isEnemyTurn = true;
		handleTurn();
	}
	
	public void handleTurn()
	{
//		GD.Print("Handling Enemy Turn");
		RigidBody player = GetNode<RigidBody>("/root/Main/Player");
		Vector3 playerPos = player.Translation;
		Vector2 posDelta = new Vector2(Translation.x - playerPos.x, Translation.z - playerPos.z);
		Vector2 sitePath = posDelta.Normalized();
		
		RayCast eyes = GetNode<RayCast>("Eyes");
		eyes.CastTo = new Vector3(sitePath.x * -10, 1, sitePath.y * -10);
		eyes.ForceRaycastUpdate();
		
		if (eyes.IsColliding())
		{
			Node collision = (Node) eyes.GetCollider();
			if (collision.IsInGroup("player"))
			{
				GD.Print("See player", posDelta);
				isEnemyTurn = true;

				if (remainingMoves > 0) {
					if (Math.Abs(posDelta.x) > gridSize && Math.Abs(posDelta.y) > gridSize) {
//						GD.Print("Too far too attack");
						if (rnd.Next(0, 2) == 1) {
//							GD.Print("Moving along X");
							handleMove(new Vector2(Math.Abs(posDelta.x) / -posDelta.x, 0));
						} else {
//							GD.Print("Moving along Y");
							handleMove(new Vector2(0, Math.Abs(posDelta.y) / -posDelta.y));
						}
					} else if (Math.Abs(posDelta.x) > gridSize) {
//						GD.Print("Player on X: Moving");
						handleMove(new Vector2(Math.Abs(posDelta.x) / -posDelta.x, 0));
					} else if (Math.Abs(posDelta.y) > gridSize) {
//						GD.Print("Player on Y: Moving");
						handleMove(new Vector2(0, Math.Abs(posDelta.y) / -posDelta.y));
					} else {
						remainingMoves = 0;
					}
				}

				if (remainingActions > 0) {
					if (Math.Abs(posDelta.x) == gridSize) {
//						GD.Print("Attacking adjacent Player");
						handleAttack();
					} else if (Math.Abs(posDelta.y) == gridSize) {
//						GD.Print("Attacking adjacent Player");
						handleAttack();
					} else {
						remainingActions = 0;
					}
				}

				if (remainingMoves + remainingActions == 0) {
					isEnemyTurn = false;
				}
			} else {
				isEnemyTurn = false;
				remainingActions = 0;
				remainingMoves = 0;
			}
		} else {
			isEnemyTurn = false;
			remainingActions = 0;
			remainingMoves = 0;
		}
	}
}
