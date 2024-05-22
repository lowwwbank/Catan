using System;
using UnityEngine;
using Catan.ResourcePhase;
using System.Collections.Generic;
using Catan.GameManagement;
using Catan.Camera;
using Catan.Settings;
using Catan.Util;


namespace Catan.GameBoard
{
  
    public class BoardInitializer : MonoBehaviour
    {
   
        public Tile.TileType[] tileTypes;
    
        public int[] tileAmount;
      
        public int[] boardShape;
       
        public int[] diceValues;
       
        public Resource.ResourceType[] ports;
       
        public int[] portAmounts;

        
        public static readonly Dictionary<(bool, bool, bool, bool), float> PORT_ANGLES = new Dictionary<(bool, bool, bool, bool), float>()
        {
            { (false, false, false, false),     0f },
            { (false, false, false, true ),   120f },
            { (false, false, true , false),  -120f },
            { (false, false, true , true ),   180f },
            { (false, true , false, false),  -180f },
            { (false, true , false, true ),  -240f },
            { (false, true , true , false),   240f },
            { (false, true , true , true ),   -90f },
            { (true , false, false, false),     0f },
            { (true , false, false, true ),    60f },
            { (true , false, true , false),   -60f },
            { (true , false, true , true ),   -90f },
            { (true , true , false, false),   -90f },
            { (true , true , false, true ),   -90f },
            { (true , true , true , false),   -90f },
            { (true , true , true , true ),   -90f }
        };

        public Board board;

        public void Initialize()
        {
            LoadPreset();

            board = GameObject.Find("Board").GetComponent(typeof(Board)) as Board;
            board.ClearTiles();
            board.tiles = Randomize();
            board.vertices = InitializeVertices(board.tiles);
            board.roads = InitializeRoads(board.vertices, board.tiles);
            board.PlaceTiles();
            board.PlaceVertices();
            board.PlaceRoads();
            InitializePorts(board.vertices, board.tiles);
            board.PlacePorts();

            CameraControl cam = GameObject.Find("Camera Rig").GetComponent<CameraControl>();
            cam.xBound0 = 0;
            cam.xBound1 = GameObject.Find("Vertex(" + (board.tiles.Length - 1) + "," + 0 + ")").transform.position.x + 6;

            Tile[] widest = board.tiles.SelectMax(t => t.Length);
            int widestLevel = widest[0].xDataIndex;
            cam.zBound0 = GameObject.Find("Vertex(" + widestLevel + "," + 0 + ")").transform.position.z - 2;
            cam.zBound1 = -cam.zBound0;
        }

   
        public void Reinitialize()
        {
            board = GameObject.Find("Board").GetComponent(typeof(Board)) as Board;
            board.ClearTiles();
            board.ResetVertices();
            board.ResetRoads();
            board.tiles = Randomize();
            board.PlaceTiles();
            InitializePorts(board.vertices, board.tiles);
            board.PlacePorts();
        }

        public Tile[][] Randomize()
        {
            int boardHeight = boardShape.Length;
            Tile[][] tileArray = new Tile[boardHeight][];

            if (tileAmount.Length < tileTypes.Length)
            {
                Debug.LogError("Amount not specified for some tiles");
                return null;
            }

            int[] tAmounts = (int[])tileAmount.Clone();
            int[] dValues = (int[])diceValues.Clone();

            bool placedRobber = false;
            for (int i = 0; i < boardHeight; i++)
            {
                tileArray[i] = new Tile[boardShape[i]];
                for (int j = 0; j < tileArray[i].Length; j++)
                {
                    tileArray[i][j] = new Tile(i, j, GetRandomTileType(tAmounts, tileTypes));
                    if (tileArray[i][j].type != Tile.TileType.Desert)
                    {
                        tileArray[i][j].diceValue = GetRandomDiceValue(dValues);
                    }
                    else if (!placedRobber)
                    {
                        placedRobber = true;
                        tileArray[i][j].robber = true;
                        GameObject.Find("Game Manager").GetComponent<GameManager>().robberLocation = (i, j);
                    }
                }
            }
            return tileArray;
        }

       
        public TileVertex[][] InitializeVertices(Tile[][] tiles)
        {
            if (tiles == null || tiles[0] == null)
            {
                Debug.LogError("No tiles were found.");
                return null;
            }

            int boardHeight = boardShape.Length;
            TileVertex[][] vertices = new TileVertex[boardHeight + 1][];

            for (int i = 0; i < boardHeight + 1; i++)
            {
                int aboveCount = boardShape[Math.Clamp(i - 1, 0, boardHeight - 1)];
                int belowCount = boardShape[Math.Clamp(i, 0, boardHeight - 1)];

                int heightstagger = 1;
                int remainder = 1;

                if (i - 1 >= 0 && i < boardHeight && boardShape[i] == boardShape[i - 1])
                {
                    remainder = 2;
                    if (i % 2 == 0)
                    {
                        heightstagger = 0;
                    }
                }

                if (i - 1 >= 0 && i < boardHeight && boardShape[i] < boardShape[i - 1] || i == boardHeight)
                {
                    heightstagger = 0;
                }

                vertices[i] = new TileVertex[Math.Max(aboveCount, belowCount) * 2 + remainder];

                for (int j = 0; j < vertices[i].Length; j++)
                {
                    bool up = (j + heightstagger) % 2 == 1;
                    vertices[i][j] = new TileVertex(up);
                }
            }

            return vertices;
        }

     
        public void InitializePorts(TileVertex[][] vertices, Tile[][] tiles)
        {
            int[] pAmounts = (int[])portAmounts.Clone();

            int max = 0;
            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i].Length > max)
                {
                    max = tiles[i].Length;
                }
            }
            max += 2;

            (int, int)[] portPoints = BuildPortPerimeter(vertices);

            int iteration = 0;
            (int, int) prevpoint = (-1, -1);
            bool isSecond = false;
            Resource.ResourceType t = Resource.ResourceType.Any;
            foreach ((int, int) point in portPoints)
            {
     
                if (point == prevpoint)
                {
                    continue;
                }
                prevpoint = point;

               
                iteration = ((iteration + 1) % 10);
                if (iteration == 3 || iteration == 6 || iteration == 9 || iteration == 0)
                {
                    continue;
                }

                int i = point.Item1;
                int j = point.Item2;

                bool above = vertices.TileAboveVertex(tiles, i, j, max) != (-1, -1);
                bool below = vertices.TileBelowVertex(tiles, i, j, max) != (-1, -1);
                bool left = vertices.TileLeftOfVertex(tiles, i, j) != (-1, -1);
                bool right = vertices.TileRightOfVertex(tiles, i, j) != (-1, -1);

                if (!isSecond)
                {
                    try
                    {
                        t = GetRandomPortType(pAmounts, ports);
                    }
                    catch
                    {
                        return;
                    }
                }
                

                vertices[i][j].port = new Port();
                vertices[i][j].port.type = t;
                float direction = PORT_ANGLES[(above, below, left, right)];

                if (direction == 180 && !vertices[point.Item1][point.Item2].up) direction = 0;
                if (direction == 120 && !vertices[point.Item1][point.Item2].up) direction = 60;
                if (direction == -120 && !vertices[point.Item1][point.Item2].up) direction = -60;

                vertices[i][j].port.direction = direction + (isSecond ? -20 : 20);
                isSecond = !isSecond;
            }
        }

 
        public (int, int)[] BuildPortPerimeter(TileVertex[][] vertices)
        {
            List<(int, int)> points = new List<(int, int)>();

            for (int j = 0; j < vertices[0].Length; j++)
            {
                points.Add((0, j));
            }

            int x = 0;
            int y = vertices[x].Length - 1;
            bool forward = true;
            while (x < vertices.Length - 1)
            {
                points.Add((x, y));
                if (vertices.VertexRightOfVertex(x, y).Valid() && forward)
                {
                    (x, y) = vertices.VertexRightOfVertex(x, y);
                    continue;
                }
                if (vertices.VertexBelowVertex(x, y).Valid())
                {
                    (x, y) = vertices.VertexBelowVertex(x, y);
                    forward = true;
                    continue;
                }
                if (vertices.VertexLeftOfVertex(x, y).Valid())
                {
                    (x, y) = vertices.VertexLeftOfVertex(x, y);
                    forward = false;
                    continue;
                }
                break;
            }

            x = vertices.Length - 1;
            for (int j = vertices[vertices.Length - 1].Length - 1; j >= 0; j--)
            {
                points.Add((x, j));
            }

            x = vertices.Length - 1;
            y = 0;
            bool backward = true;
            while (x > 0)
            {
                points.Add((x, y));
                if (vertices.VertexLeftOfVertex(x, y).Valid() && backward)
                {
                    (x, y) = vertices.VertexLeftOfVertex(x, y);
                    continue;
                }
                if (vertices.VertexAboveVertex(x, y).Valid())
                {
                    (x, y) = vertices.VertexAboveVertex(x, y); 
                    backward = true;
                    continue;
                }
                if (vertices.VertexRightOfVertex(x, y).Valid())
                {
                    (x, y) = vertices.VertexRightOfVertex(x, y);
                    backward = false;
                    continue;
                }
                break;
            }

            return points.ToArray();
        }


        public Road[][] InitializeRoads(TileVertex[][] vertices, Tile[][] tiles)
        {
            if (vertices == null || vertices[0] == null)
            {
                Debug.LogError("No vertices were found.");
                return null;
            }

            int boardHeight = boardShape.Length;
            Road[][] roads = new Road[boardHeight * 2 + 1][];

            for (int i = 0; i < boardHeight * 2 + 1; i++)
            {

                if (i % 2 == 0)
                {
                    roads[i] = new Road[vertices[i / 2].Length - 1];
                }

                else
                {
                    roads[i] = new Road[tiles[(i - 1) / 2].Length + 1];
                }

                for (int j = 0; j < roads[i].Length; j++)
                {
                    roads[i][j] = new Road(i, j);
                }
            }

            return roads;
        }

    
        public Tile.TileType GetRandomTileType(int[] tAmounts, Tile.TileType[] tTypes)
        {
            int rand = UnityEngine.Random.Range(0, tTypes.Length);
            for (int i = rand; i < rand + tTypes.Length; i++)
            {
                if (tAmounts[i % tTypes.Length] == 0)
                {
                    continue;
                }
                else
                {
                    tAmounts[i % tTypes.Length]--;
                    return tTypes[i % tTypes.Length];
                }
            }

           
            return Tile.TileType.Desert;
        }


        public int GetRandomDiceValue(int[] dValues)
        {
            int rand = UnityEngine.Random.Range(0, dValues.Length);
            for (int i = rand; i < rand + dValues.Length; i++)
            {
                if (dValues[i % dValues.Length] == 0)
                {
                    continue;
                }
                else
                {
                    dValues[i % dValues.Length]--;
                    return i % dValues.Length;
                }
            }

  
            return 0;
        }

     
        public Resource.ResourceType GetRandomPortType(int[] rAmounts, Resource.ResourceType[] rTypes)
        {
            int rand = UnityEngine.Random.Range(0, rTypes.Length);
            for (int i = rand; i < rand + rTypes.Length; i++)
            {
                if (rAmounts[i % rTypes.Length] <= 0)
                {
                    continue;
                }
                else
                {
                    rAmounts[i % rTypes.Length]--;
                    return rTypes[i % rTypes.Length];
                }
            }

            throw new Exception("None");
        }

        public void LoadPreset()
        {
            BoardPreset preset = GameSettings.presets[GameSettings.chosenPreset];
            tileTypes = preset.tileTypes;
            tileAmount = preset.tileAmounts;
            boardShape = preset.boardShape;
            diceValues = preset.diceValues;
            ports = preset.portTypes;
            portAmounts = preset.portAmounts;
        }
    }
}
