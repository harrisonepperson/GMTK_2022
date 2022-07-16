using Godot;
using System;

public class Range_Attack_Particles : Particles
{
	private Vector3 targetPos = new Vector3(0, 2, 0);
	
	[Export]
	private int slideSpeed = 5;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		Translation = Translation.LinearInterpolate(targetPos, slideSpeed * delta);

		if (Translation.DistanceSquaredTo(targetPos) <= 0.001) {
			Translation = targetPos;
		}
	}
	
	public void castTo(Vector3 to)
	{
		Emitting = true;
		Translation = new Vector3(0, 2, 0);
		targetPos = new Vector3(to.x, 2, to.z);
	}
}
