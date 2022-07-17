using Godot;
using System;

public class main : Spatial
{
	private Random rnd = new Random();
	
	private enum tiles
	{
		Enterance,
		Exit,
		Empty,
		Floor
	}

	[Export]
	private int rows = 5;

	[Export]
	private int cols = 8;
	
	private tiles[,] roomMap;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		buildRoom();
	}
	
	public void buildRoom()
	{	
		roomMap = new tiles[ rows, cols ];
		
		for (int r = 0; r < rows; r++) {
			for (int c = 0; c < cols; c++) {
				roomMap[r, c] = tiles.Empty;
			}
		}
		
		// NESW
		int entranceWall = rnd.Next(0, 4);
		int exitWall = rnd.Next(0, 4);
		
		while(exitWall == entranceWall) {
			exitWall = rnd.Next(0, 4);
		}
		
		Vector2 entrancePos = Vector2.Zero;
		Vector2 exitPos = Vector2.Zero;
		
		switch (entranceWall) {
			case 0:
				entrancePos.x = rnd.Next(0, cols - 1);
				entrancePos.y = 0;
				break;
			case 1:
				entrancePos.x = cols - 1;
				entrancePos.y = rnd.Next(0, rows - 1);
				break;
			case 2:
				entrancePos.x = rnd.Next(1, cols);
				entrancePos.y = rows - 1;
				break;
			case 3:
				entrancePos.x = 0;
				entrancePos.y = rnd.Next(1, rows);
				break;
		}
		
		switch (exitWall) {
			case 0:
				exitPos.x = rnd.Next(1, cols - 1);
				exitPos.y = 0;
				break;
			case 1:
				exitPos.x = cols - 1;
				exitPos.y = rnd.Next(0, rows - 1);
				break;
			case 2:
				exitPos.x = rnd.Next(1, cols);
				exitPos.y = rows - 1;
				break;
			case 3:
				exitPos.x = 0;
				exitPos.y = rnd.Next(1, rows);
				break;
		}

		roomMap[(int) entrancePos.y, (int) entrancePos.x] = tiles.Enterance;
		roomMap[(int) exitPos.y, (int) exitPos.x] = tiles.Exit;

		GD.Print(entrancePos, exitPos);
		printRoom(roomMap);
	}
	
	private void printRoom(tiles[,] room)
	{
		for (int r = 0; r < rows; r++) {
			string cS = "";
			
			for (int c = 0; c < cols; c++) {
				cS += room[r, c] + ", ";
			}
			
			GD.Print(cS);
		}
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
