using Godot;
using System;

public class GreenButton : TextureButton
{
	private Sprite bg;
	private Sprite bgHover;

	[Export]
	private PackedScene nextScene;
	private Autoload autoload;

	[Export]
	private bool reloadCurrentScene = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		autoload = GetNode<Autoload>("/root/Autoload");
		bg = GetNode<Sprite>("bg");
		bgHover = GetNode<Sprite>("bg-hover");
	}

	private void _on_GreenButton_mouse_entered()
	{
		bg.Visible = false;
		bgHover.Visible = true;
	}

	private void _on_GreenButton_mouse_exited()
	{
		bg.Visible = true;
		bgHover.Visible = false;
	}

	private void _on_GreenButton_pressed()
	{
		if (reloadCurrentScene)
		{
			GetTree().ReloadCurrentScene();
		}
		else
		{
			if (nextScene != null)
			{
				GetTree().ChangeSceneTo(nextScene);
				autoload.ResetGame();
			}
			else
			{
				GetTree().ChangeScene("res://main.tscn");
				autoload.ResetGame();
			}
		}
	}
}
