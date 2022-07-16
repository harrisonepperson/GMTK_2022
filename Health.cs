using Godot;
using System;

public class Health : Control
{

	private Autoload autoload;
	private TextureRect shield1;
	private TextureRect heart1;
	private TextureRect heart2;
	private TextureRect heart3;
	private TextureRect heart4;
	private TextureRect heart1Disabled;
	private TextureRect heart2Disabled;
	private TextureRect heart3Disabled;
	private TextureRect heart4Disabled;

	private int cachedPlayerHealth = 0;
	private int cachedPlayerShield = 1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		autoload = GetNode<Autoload>("/root/Autoload");
		shield1 = GetNode<TextureRect>("shield1");

		heart1 = GetNode<TextureRect>("heart1");
		heart2 = GetNode<TextureRect>("heart2");
		heart3 = GetNode<TextureRect>("heart3");
		heart4 = GetNode<TextureRect>("heart4");

		heart1Disabled = GetNode<TextureRect>("heart1-disabled");
		heart2Disabled = GetNode<TextureRect>("heart2-disabled");
		heart3Disabled = GetNode<TextureRect>("heart3-disabled");
		heart4Disabled = GetNode<TextureRect>("heart4-disabled");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (autoload.playerShield != cachedPlayerShield)
		{
			cachedPlayerShield = autoload.playerShield;

			shield1.Visible = cachedPlayerShield >= 1;
		}

		//If the health has changed, update the UI.
		if (autoload.playerHealth != cachedPlayerHealth)
		{
			cachedPlayerHealth = autoload.playerHealth;
			heart1.Visible = cachedPlayerHealth >= 1;
			heart1Disabled.Visible = !heart1.Visible;
			heart2.Visible = cachedPlayerHealth >= 2;
			heart2Disabled.Visible = !heart2.Visible;
			heart3.Visible = cachedPlayerHealth >= 3;
			heart3Disabled.Visible = !heart3.Visible;
			heart4.Visible = cachedPlayerHealth >= 4;
			heart4Disabled.Visible = !heart4.Visible;
		}

	}
}
