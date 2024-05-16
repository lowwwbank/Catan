using Catan.ResourcePhase;
using UnityEngine;


namespace Catan.GameBoard
{

    public class Tile
    {

        public TileType type;
        
        public int diceValue;
   
        public int xDataIndex;

        public int yDataIndex;
 
        public int xCoord;
 
        public int yCoord;

        public bool robber;

        public Resource.ResourceType resourceType
        { 
            get
            {
                switch (type)
                {
                    case TileType.Pasture:
                        return Resource.ResourceType.Wool;
                    case TileType.Field:
                        return Resource.ResourceType.Grain;
                    case TileType.Forest:
                        return Resource.ResourceType.Wood;
                    case TileType.Hills:
                        return Resource.ResourceType.Brick;
                    case TileType.Mountains:
                        return Resource.ResourceType.Ore;
                    case TileType.Desert:
                        return Resource.ResourceType.None;
                    default:
                        return Resource.ResourceType.Any;
                }
            }
        }

        public Tile(int x, int y, TileType tileType)
        {
            xDataIndex = x;
            yDataIndex = y;
            type = tileType;
        }

 
        public enum TileType
        {
            Pasture,
            Field,
            Forest,
            Hills,
            Mountains,
            Desert
        } 
    }
}