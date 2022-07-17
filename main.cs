using Godot;
using System;

public class main : Spatial
{
	private Random rnd = new Random();

	private Autoload autoload;

	private int gridSize;
	private int roomSize = 5;
	
	private enum tiles
	{
		Enterance,
		Exit,
		Empty,
		Floor
	}

	[Export]
	private PackedScene entranceScene;

	[Export]
	private PackedScene exitScene;

	[Export]
	private PackedScene floorScene;

	[Export]
	private PackedScene emptyScene;

	// [Export]
	// private int rows = 5;

	// [Export]
	// private int cols = 8;
	
	private int[,] roomMap;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		autoload = GetNode<Autoload>("/root/Autoload");
		gridSize = autoload.gridSize;

		chooseRoom();
		populateWithTiles();
		// buildRoom();
	}

	public void chooseRoom()
	{
		int[,] option = new int[,] {
			{ 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0 }
		};

		switch (rnd.Next(0, 4)) {
			case 0:
				option = new int[,] {
					{ 0, 0, 0, 0, 0, 0, 0, 0 },
					{ 0, 0, 0, 1, 1, 1, 0, 0 },
					{ 0, 1, 1, 1, 0, 1, 1, 0 },
					{ 0, 1, 0, 1, 0, 0, 1, 3 },
					{ 0, 2, 0, 0, 0, 0, 0, 0 }
				};
				break;
			case 1:
				option = new int[,] {
					{ 0, 3, 1, 0, 0, 0, 0, 0 },
					{ 0, 0, 1, 1, 0, 0, 0, 0 },
					{ 2, 1, 0, 1, 0, 0, 0, 0 },
					{ 0, 1, 1, 1, 0, 0, 0, 0 },
					{ 0, 0, 0, 0, 0, 0, 0, 0 }
				};
				break;
			case 2:
				option = new int[,] {
					{ 0, 0, 0, 2, 1, 1, 0, 0 },
					{ 0, 0, 0, 0, 0, 1, 0, 0 },
					{ 0, 0, 1, 1, 1, 1, 0, 0 },
					{ 0, 0, 1, 0, 0, 0, 0, 0 },
					{ 0, 0, 1, 3, 0, 0, 0, 0 }
				};
				break;
			case 3:
				option = new int[,] {
					{ 0, 0, 0, 1, 0, 0, 0, 0 },
					{ 0, 1, 1, 1, 0, 0, 0, 0 },
					{ 0, 1, 0, 1, 1, 1, 1, 0 },
					{ 0, 1, 1, 0, 1, 0, 1, 2 },
					{ 0, 3, 0, 0, 1, 0, 0, 0 }
				};
				break;
			// case 4:
			// 	option = new int[,] {
			// 		{ 0, 0, 0, 0, 0, 0, 0, 0 },
			// 		{ 0, 0, 0, 0, 0, 0, 0, 0 },
			// 		{ 0, 0, 0, 0, 0, 0, 0, 0 },
			// 		{ 0, 0, 0, 0, 0, 0, 0, 0 },
			// 		{ 0, 0, 0, 0, 0, 0, 0, 0 }
			// 	};
			// 	break;
		}

		roomMap = option;
	}
	
	public void populateWithTiles()
	{
		for (int r = 0; r < roomMap.GetLength(0); r++) {
			for (int c = 0; c < roomMap.GetLength(1); c++) {
				PackedScene toPlace = emptyScene;

				switch (roomMap[r, c]) {
					case 0: // Empty
						break;
					case 1: // Floor
						toPlace = floorScene;
						break;
					case 2: // Entrance
						toPlace = entranceScene;
						break;
					case 3: // Exit
						toPlace = exitScene;
						break;
				}

				Spatial toPlaceInstance = (Spatial) toPlace.Instance();
				toPlaceInstance.Translation = new Vector3(c * gridSize * roomSize, 0, r * gridSize * roomSize);
				AddChild(toPlaceInstance);
			}
		}
	}

	// public void buildRoom()
	// {	
	// 	roomMap = new tiles[ rows, cols ];
		
	// 	for (int r = 0; r < rows; r++) {
	// 		for (int c = 0; c < cols; c++) {
	// 			roomMap[r, c] = tiles.Empty;
	// 		}
	// 	}
		
	// 	// NESW
	// 	int entranceWall = rnd.Next(0, 4);
	// 	int exitWall = rnd.Next(0, 4);
		
	// 	while(exitWall == entranceWall) {
	// 		exitWall = rnd.Next(0, 4);
	// 	}
		
	// 	Vector2 entrancePos = Vector2.Zero;
	// 	Vector2 exitPos = Vector2.Zero;
		
	// 	switch (entranceWall) {
	// 		case 0:
	// 			entrancePos.x = rnd.Next(0, cols - 1);
	// 			entrancePos.y = 0;
	// 			break;
	// 		case 1:
	// 			entrancePos.x = cols - 1;
	// 			entrancePos.y = rnd.Next(0, rows - 1);
	// 			break;
	// 		case 2:
	// 			entrancePos.x = rnd.Next(1, cols);
	// 			entrancePos.y = rows - 1;
	// 			break;
	// 		case 3:
	// 			entrancePos.x = 0;
	// 			entrancePos.y = rnd.Next(1, rows);
	// 			break;
	// 	}
		
	// 	switch (exitWall) {
	// 		case 0:
	// 			exitPos.x = rnd.Next(1, cols - 1);
	// 			exitPos.y = 0;
	// 			break;
	// 		case 1:
	// 			exitPos.x = cols - 1;
	// 			exitPos.y = rnd.Next(0, rows - 1);
	// 			break;
	// 		case 2:
	// 			exitPos.x = rnd.Next(1, cols);
	// 			exitPos.y = rows - 1;
	// 			break;
	// 		case 3:
	// 			exitPos.x = 0;
	// 			exitPos.y = rnd.Next(1, rows);
	// 			break;
	// 	}

	// 	roomMap[(int) entrancePos.y, (int) entrancePos.x] = tiles.Enterance;
	// 	roomMap[(int) exitPos.y, (int) exitPos.x] = tiles.Exit;
		
	// 	Vector2 portalDelta = new Vector2(exitPos.x - entrancePos.x, exitPos.y - entrancePos.y);
		
	// 	recursiveStep(entrancePos, exitPos);
		
	// 	printRoom(roomMap);
	//}
	
	// private int checkNeighbors(Vector2 pos)
	// {
	// 	int neighbors = 0;
	// 	if (pos.y > 1) { // Check North
	// 		if (roomMap[(int) pos.y - 1, (int) pos.x] != tiles.Empty) {
	// 			neighbors += 1;
	// 		}
	// 	}
	// 	if (pos.x < rows - 1) { // Check East
	// 		if (roomMap[(int) pos.y, (int) pos.x + 1] != tiles.Empty) {
	// 			neighbors += 10;
	// 		}
	// 	}
	// 	if (pos.y < cols - 1) { // Check South
	// 		if (roomMap[(int) pos.y + 1, (int) pos.x] != tiles.Empty) {
	// 			neighbors += 100;
	// 		}
	// 	}
	// 	if (pos.x > 1) { // Check West
	// 		if (roomMap[(int) pos.y, (int) pos.x - 1] != tiles.Empty) {
	// 			neighbors += 1000;
	// 		}
	// 	}
		
	// 	return neighbors;
	// }

	// private void chooseEmptyNeighbor(Vector2 pos)
	// {
	// 	int neighbors = checkNeighbors(pos);
	// }
	
	// private void recursiveStep(Vector2 start, Vector2 end){
	// 	int i = 0;

	// 	// . . . . . . . . 
	// 	// . . . . . . . . 
	// 	// . . . . . . . O 
	// 	// . . . . . . . . 
	// 	// X . . . . . . . 

	// 	while (checkNeighbors(end) == 0 && i < 10) {
			
			
	// 		i ++;
	// 	}
	// }
	
	private void printRoom(tiles[,] room)
	{
		for (int r = 0; r < 5; r++) {
			string cS = "";
			
			for (int c = 0; c < 8; c++) {
				switch (room[r, c]) {
					case tiles.Enterance:
						cS += "O ";
						break;
					case tiles.Exit:
						cS += "X ";
						break;
					case tiles.Floor:
						cS += "W ";
						break;
					case tiles.Empty:
						cS += ". ";
						break;
				}
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
