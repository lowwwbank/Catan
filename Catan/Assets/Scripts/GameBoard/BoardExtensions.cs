using Catan.GameBoard;
using Catan.Players;
using Catan.ResourcePhase;
using Catan.Util;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Catan.GameBoard
{
   
    public static class BoardExtensions
    {
   
        public static (int, int) VertexAboveVertex(this TileVertex[][] vertices, int i, int j)
        {
            if (vertices[i][j].up)
            {
                return (-1, -1);
            }

            int v = vertices.ConvertVertical(j, i, i - 1);

            if (v == -1)
            {
                return (-1, -1);
            }

            try
            {
                TileVertex test = vertices[i - 1][v];
                if (test != null)
                {
                    return (i - 1, v);
                }
            }
            catch { }

            return (-1, -1);
        }

        public static (int, int) VertexBelowVertex(this TileVertex[][] vertices, int i, int j)
        {
            if (!vertices[i][j].up)
            {
                return (-1, -1);
            }
            
            int v = vertices.ConvertVertical(j, i, i + 1);

            if (v == -1)
            {
                return (-1, -1);
            }

            try
            {
                TileVertex test = vertices[i + 1][v];
                if (test != null)
                {
                    return (i + 1, v);
                }
            }
            catch { }

            return (-1, -1);
        }


        public static (int, int) VertexLeftOfVertex(this TileVertex[][] vertices, int i, int j)
        {
            try
            {
                TileVertex test = vertices[i][j - 1];
                if (test != null)
                {
                    return (i, j - 1);
                }
            }
            catch { }

            return (-1, -1);
        }


        public static (int, int) VertexRightOfVertex(this TileVertex[][] vertices, int i, int j)
        {
            try
            {
                TileVertex test = vertices[i][j + 1];
                if (test != null)
                {
                    return (i, j + 1);
                }
            }
            catch { }

            return (-1, -1);
        }


        public static int ConvertVertical(this TileVertex[][] vertices, int x, int i0, int i1)
        {
            int ycoord = vertices[i0][x].yCoord;

            try
            {
                if (vertices[i1].Count() < 1)
                {
                    return -1;
                }
            }
            catch
            {
                return -1;
            }

            TileVertex shifted = vertices[i1].Where(v => v.yCoord == ycoord).FirstOrDefault();
            if (shifted == null)
            {
                return -1;
            }

            return shifted.yDataIndex;
        }
        

        public static (int, int) TileAboveVertex(this TileVertex[][] vertices, Tile[][] tiles, int i, int j)
        {
            int max = 0;
            for (int k = 0; k < tiles.Length; k++)
            {
                if (tiles[k].Length > max)
                {
                    max = tiles[k].Length;
                }
            }
            max += 2;

            return vertices.TileAboveVertex(tiles, i, j, max);
        }

        public static (int, int) TileBelowVertex(this TileVertex[][] vertices, Tile[][] tiles, int i, int j)
        {
            int max = 0;
            for (int k = 0; k < tiles.Length; k++)
            {
                if (tiles[k].Length > max)
                {
                    max = tiles[k].Length;
                }
            }
            max += 2;

            return vertices.TileBelowVertex(tiles, i, j, max);
        }

        public static (int, int) TileAboveVertex(this TileVertex[][] vertices, Tile[][] tiles, int i, int j, int max)
        {
            if (!vertices[i][j].up) return (-1, -1);

            int xc = vertices[i][j].xCoord;
            int yc = vertices[i][j].yCoord;

            int smallOffset = (max % 2 == 1 ? (i + 1) % 2 : (i) % 2);

            int x = xc - 1;
            int y = (int)(yc / 2) - 1 - smallOffset;

            (int xout, int yout) = tiles.GetTileDataCoord(x, y);

            return (xout, yout);
        }


        public static (int, int) TileBelowVertex(this TileVertex[][] vertices, Tile[][] tiles, int i, int j, int max)
        {
            if (vertices[i][j].up) return (-1, -1);

            int xc = vertices[i][j].xCoord;
            int yc = vertices[i][j].yCoord;

            
            int smallOffset = (max % 2 == 0 ? (i + 1) % 2 : (i) % 2);

            int x = xc;
            int y = (int)(yc / 2) - 1 - smallOffset;

            (int xout, int yout) = tiles.GetTileDataCoord(x, y);
            return (xout, yout);
        }


        public static (int, int) TileRightOfVertex(this TileVertex[][] vertices, Tile[][] tiles, int i, int j)
        {
            int xc = vertices[i][j].xCoord;
            int yc = vertices[i][j].yCoord;

            int x = xc - (vertices[i][j].up ? 0 : 1);
            int y = (int)(yc / 2) - 1;

            (int xout, int yout) = tiles.GetTileDataCoord(x, y);
            return (xout, yout);
        }


        public static (int, int) TileLeftOfVertex(this TileVertex[][] vertices, Tile[][] tiles, int i, int j)
        {
            int xc = vertices[i][j].xCoord;
            int yc = vertices[i][j].yCoord;

            int x = xc - (vertices[i][j].up ? 0 : 1);
            int y = (int)(yc / 2) - 2;

            (int xout, int yout) = tiles.GetTileDataCoord(x, y);
            return (xout, yout);
        }


        public static (int, int) RoadAboveVertex(this TileVertex[][] vertices, Road[][] roads, int i, int j)
        {
            if (vertices[i][j].up) return (-1, -1);

            int xc = vertices[i][j].xCoord;
            int yc = vertices[i][j].yCoord;

            int x = xc * 2 - 1;
            int y = yc;

            (int xout, int yout) = roads.GetRoadDataCoord(x, y);
            return (xout, yout);
        }


        public static (int, int) RoadBelowVertex(this TileVertex[][] vertices, Road[][] roads, int i, int j)
        {
            if (!vertices[i][j].up) return (-1, -1);

            int xc = vertices[i][j].xCoord;
            int yc = vertices[i][j].yCoord;

            int x = xc * 2 + 1;
            int y = yc;

            (int xout, int yout) = roads.GetRoadDataCoord(x, y);
            return (xout, yout);
        }


        public static (int, int) RoadRightOfVertex(this TileVertex[][] vertices, Road[][] roads, int i, int j)
        {
            if (j >= roads[i * 2].Length)
            {
                return (-1, -1);
            }

            if (roads[i * 2][j] == null)
            {
                return (-1, -1);
            }

            return (i * 2, j);
        }

        public static (int, int) RoadLeftOfVertex(this TileVertex[][] vertices, Road[][] roads, int i, int j)
        {
            if (j <= 0)
            {
                return (-1, -1);
            }

            if (roads[i * 2][j - 1] == null)
            {
                return (-1, -1);
            }

            return (i * 2, j - 1);
        }

        public static (int, int)[] AdjacentRoadsToVertex(this TileVertex[][] vertices, Road[][] roads, (int, int) vertex)
        {
            (int, int) above = vertices.RoadAboveVertex(roads, vertex.Item1, vertex.Item2);
            (int, int) below = vertices.RoadBelowVertex(roads, vertex.Item1, vertex.Item2);
            (int, int) left = vertices.RoadLeftOfVertex(roads, vertex.Item1, vertex.Item2);
            (int, int) right = vertices.RoadRightOfVertex(roads, vertex.Item1, vertex.Item2);

            List<(int, int)> output = new List<(int, int)>();
            if (above != (-1, -1)) output.Add(above);
            if (below != (-1, -1)) output.Add(below);
            if (left != (-1, -1)) output.Add(left);
            if (right != (-1, -1)) output.Add(right);

            return output.ToArray();
        }

        public static (int, int)[] AdjacentVerticesToRoad(this Road[][] roads, TileVertex[][] vertices, (int, int) road)
        {
            int i = road.Item1;
            int j = road.Item2;
            (int, int)[] arr = new (int, int)[2];

            if (j >= vertices[i / 2].Length)
            {
                arr[0] = (-1, -1);
                arr[1] = (-1, -1);
            }

            if (vertices[i / 2][j] == null)
            {
                arr[0] = (-1, -1);
                arr[1] = (-1, -1);
            }

 
            if (i % 2 == 0)
            {
                arr[0] = (i / 2, j);
                arr[1] = (i / 2, j + 1);
            }

            else
            {
                arr[0] = (-1, -1);
                arr[1] = (-1, -1);
                for (int k = 0; k < vertices[i / 2].Length; k++)
                {
                    if (vertices.RoadBelowVertex(roads, i/2, k) == road)
                    {
                        arr[0] = (i / 2, k);
                    }
                }

                for (int k = 0; k < vertices[i / 2 + 1].Length; k++)
                {
                    if (vertices.RoadAboveVertex(roads, i / 2 + 1, k) == road)
                    {
                        arr[1] = (i / 2 + 1, k);
                    }
                }
            }

            return arr;
        }

        public static (int, int) GetTileDataCoord(this Tile[][] tiles, int i, int j)
        {
            int y;
            try
            {
                y = tiles[i].Where(k => k.yCoord == j).First().yDataIndex;
            }
            catch
            {
                return (-1, -1);
            }
            return (i, y);
        }

    
        public static (int, int) GetVertexDataCoord(this TileVertex[][] vertices, int i, int j)
        {
            int y;
            try
            {
                y = vertices[i].Where(k => k.yCoord == j).First().yDataIndex;
            }
            catch
            {
                return (-1, -1);
            }
            return (i, y);
        }

  
        public static (int, int) GetRoadDataCoord(this Road[][] roads, int i, int j)
        {
            int y;
            try
            {
                y = roads[i].Where(k => k.yCoord == j).First().yDataIndex;
            }
            catch
            {
                return (-1, -1);
            }
            return (i, y);
        }

     
        public static (int, int)[] GetSurroundingVertices(this Tile[][] tiles, TileVertex[][] vertices, int i, int j)
        {
            int maxIsEven = (tiles.SelectMax(tArr => tArr.Length).Length + 1) % 2;

            int x0 = tiles[i][j].xCoord;
            int y0 = (int)(tiles[i][j].yCoord + 1) * 2 + i % 2 + maxIsEven * (int)Mathf.Pow(-1, i);

            (int, int) top = vertices.GetVertexDataCoord(x0, y0);
            (int, int) bottom = vertices.GetVertexDataCoord(x0 + 1, y0);
            (int, int)[] output = 
            {
                (top.Item1, top.Item2),
                (top.Item1, top.Item2 + 1),
                (top.Item1, top.Item2 + 2),
                (bottom.Item1, bottom.Item2),
                (bottom.Item1, bottom.Item2 + 1),
                (bottom.Item1, bottom.Item2 + 2)
            };

            return output;
        }

    
        public static Vector3 PolarToCartesian(float theta, float r)
        {
            r = Mathf.Deg2Rad * r;
            return new Vector3(r * Mathf.Cos(theta), 0, r * Mathf.Sin(theta));
        }

 
        public static bool Valid(this (int, int) point)
        {
            if (point.Item1 < 0 || point.Item2 < 0) { return false; }
            return true;
        }

        public static Resource.ResourceType[] GetPlayerPorts(this TileVertex[][] vertices, Player player)
        {
            List<Resource.ResourceType> ports = new List<Resource.ResourceType>();
            for (int i = 0; i < vertices.Length; i++)
            {
                for (int j = 0; j < vertices[i].Length; j++)
                {
                    if (vertices[i][j].playerIndex == player.playerIndex && vertices[i][j].port != null)
                    {
                        ports.Add(vertices[i][j].port.type);
                    }
                }
            }
            return ports.ToArray();
        }

        public static bool HasPort(this TileVertex[][] vertices, Player player, Resource.ResourceType resource)
        {
            Resource.ResourceType[] ports = GetPlayerPorts(vertices, player);

            foreach (Resource.ResourceType port in ports)
            {
                if (port == resource)
                {
                    return true;
                }
            }

            return false;
        }  
    }
}
