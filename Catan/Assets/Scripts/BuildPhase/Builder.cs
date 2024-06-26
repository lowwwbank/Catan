using Catan.GameBoard;
using Catan.Players;
using Catan.ResourcePhase;
using System.Linq;

namespace Catan.BuildPhase
{
    public static class Builder
    {
 
        public static bool BuildVertex(this Board board, Player player, (int, int) location, TileVertexGameObject obj, bool starting = false)
        {
            (int, int)[] roads = board.vertices.AdjacentRoadsToVertex(board.roads, location);
            bool adjacent = false;
            if (!starting)
            {
                foreach ((int, int) r in roads)
                {
                    if (board.roads[r.Item1][r.Item2].playerIndex == player.playerIndex)
                    {
                        adjacent = true;
                    }
                }
            }
            else
            {
                adjacent = true;
            }

            if (!adjacent) return false;

            (int, int) vabove = board.vertices.VertexAboveVertex(location.Item1, location.Item2);
            (int, int) vbelow = board.vertices.VertexBelowVertex(location.Item1, location.Item2);
            (int, int) vleft = board.vertices.VertexLeftOfVertex(location.Item1, location.Item2);
            (int, int) vright = board.vertices.VertexRightOfVertex(location.Item1, location.Item2);

            if (board.vertices[location.Item1][location.Item2].playerIndex != player.playerIndex && board.vertices[location.Item1][location.Item2].playerIndex != -1) return false;
            if (vabove.Valid() && board.vertices[vabove.Item1][vabove.Item2].development > 0) return false;
            if (vbelow.Valid() && board.vertices[vbelow.Item1][vbelow.Item2].development > 0) return false;
            if (vleft.Valid() && board.vertices[vleft.Item1][vleft.Item2].development > 0) return false;
            if (vright.Valid() && board.vertices[vright.Item1][vright.Item2].development > 0) return false;

            if (!starting)
            {
                if (board.vertices[location.Item1][location.Item2].development == TileVertex.Development.Undeveloped)
                {
                    Resource[] resources = player.resources;
                    Resource wood = resources.Where(r => r.type == Resource.ResourceType.Wood).First();
                    Resource brick = resources.Where(r => r.type == Resource.ResourceType.Brick).First();
                    Resource wool = resources.Where(r => r.type == Resource.ResourceType.Wool).First();
                    Resource grain = resources.Where(r => r.type == Resource.ResourceType.Grain).First();
                    if (wood.amount >= 1 && brick.amount >= 1 && wool.amount >= 1 && grain.amount >= 1)
                    {
                        wood.amount -= 1;
                        brick.amount -= 1;
                    }
                    else
                    {
                        return false;
                    }
                }
                if (board.vertices[location.Item1][location.Item2].development == TileVertex.Development.Town)
                {
                    Resource[] resources = player.resources;
                    Resource ore = resources.Where(r => r.type == Resource.ResourceType.Ore).First();
                    Resource grain = resources.Where(r => r.type == Resource.ResourceType.Grain).First();
                    if (ore.amount >= 3 && grain.amount >= 2)
                    {
                        ore.amount -= 3;
                        grain.amount -= 2;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            board.vertices[location.Item1][location.Item2].playerIndex = player.playerIndex;
            obj.SetPlayer(player);
            board.vertices[location.Item1][location.Item2].AdvanceDevelopment();
            obj.UpdateMesh();

            return true;
        }

        public static bool BuildRoad(this Board board, Player player, (int, int) location, RoadGameObject obj, bool starting = false)
        {
            if (board.roads[location.Item1][location.Item2].playerIndex != -1)
            {
                return false;
            }

            (int, int)[] adjacentVertices = board.roads.AdjacentVerticesToRoad(board.vertices, location);
            bool placable = false;
            for (int i = 0; i < adjacentVertices.Length; i++)
            {
                int x = adjacentVertices[i].Item1;
                int y = adjacentVertices[i].Item2;
                if (board.vertices[x][y].playerIndex == player.playerIndex)
                {
                    placable = true;
                    break;
                }
                else if (board.vertices[x][y].playerIndex == -1)
                {
                    (int, int) above = board.vertices.RoadAboveVertex(board.roads, x, y);
                    if (above.Valid() && board.roads[above.Item1][above.Item2].playerIndex == player.playerIndex)
                    {
                        placable = true;
                        break;
                    }

                    (int, int) below = board.vertices.RoadBelowVertex(board.roads, x, y);
                    if (below.Valid() && board.roads[below.Item1][below.Item2].playerIndex == player.playerIndex)
                    {
                        placable = true;
                        break;
                    }

                    (int, int) left = board.vertices.RoadLeftOfVertex(board.roads, x, y);
                    if (left.Valid() && board.roads[left.Item1][left.Item2].playerIndex == player.playerIndex)
                    {
                        placable = true;
                        break;
                    }

                    (int, int) right = board.vertices.RoadRightOfVertex(board.roads, x, y);
                    if (right.Valid() && board.roads[right.Item1][right.Item2].playerIndex == player.playerIndex)
                    {
                        placable = true;
                        break;
                    }
                }
            }

            if (!placable)
            {
                return false;
            }

            if (!starting)
            {
                Resource[] resources = player.resources;
                Resource wood = resources.Where(r => r.type == Resource.ResourceType.Wood).First();
                Resource brick = resources.Where(r => r.type == Resource.ResourceType.Brick).First();
                if (wood.amount >= 1 && brick.amount >= 1)
                {
                    wood.amount -= 1;
                    brick.amount -= 1;
                }
                else
                {
                    return false;
                }
            }

            board.roads[location.Item1][location.Item2].playerIndex = player.playerIndex;
            obj.SetPlayer(player);
            return true;
        }
    }
}
