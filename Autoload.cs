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
        //music
        ResourceLoader.Load("res://Assets/Sound Effects/music3.mp3");

        //events		
        ResourceLoader.Load("res://Assets/Sound Effects/level-complete2.wav");
        ResourceLoader.Load("res://Assets/Sound Effects/lost2.wav");

        //actions
        ResourceLoader.Load("res://Assets/Sound Effects/light-attack.mp3");
        ResourceLoader.Load("res://Assets/Sound Effects/heal.mp3");
        ResourceLoader.Load("res://Assets/Sound Effects/aoe-attack.wav");
        ResourceLoader.Load("res://Assets/Sound Effects/ranged-attack.wav");
        ResourceLoader.Load("res://Assets/Sound Effects/shield.mp3");
    }

    public void ResetGame()
    {
        playerHealth = 4;
        playerShield = 0;
        levelComplete = false;
        score = 0;
    }

    public override void _Process(float delta)
    {
        if (!isPlayerTurn)
        {

            Godot.Collections.Array enemies = GetTree().GetNodesInGroup("enemy");
            bool enemyStillPlaying = false;

            foreach (Enemy e in enemies)
            {
                if (!e.isDead && e.isEnemyTurn)
                {
                    enemyStillPlaying = true;
                }
            }

            if (!enemyStillPlaying)
            {
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
