using Catan.GameBoard;
using Catan.Players;
using Catan.ResourcePhase;


namespace Catan.Settings
{
    public static class GameSettings
    {
        public static int maxTurns = 1000;
        public static int maxTradeAttempts;
        public static int maxBuildAttempts;

        public static int vpWinCondition = 10;
        public static bool animations;
        public static bool testing = true;

        public static Player[] players;
        public static int chosenPreset = 0;
        public static BoardPreset[] presets = new BoardPreset[]
        {
   
            new BoardPreset()
            {
                name = "Classic",
                desc = "The classic Catan experience. 3-4 players recommended.",
                tileTypes = new Tile.TileType[]
                {
                    Tile.TileType.Pasture,
                    Tile.TileType.Field,
                    Tile.TileType.Forest,
                    Tile.TileType.Hills,
                    Tile.TileType.Mountains,
                    Tile.TileType.Desert
                },
                tileAmounts = new int[]
                {
                    4,
                    4,
                    4,
                    3,
                    3,
                    1
                },
                boardShape = new int[]
                {
                    3,
                    4,
                    5,
                    4,
                    3
                },
                diceValues = new int[]
                {
                    0,
                    0,
                    1,
                    2,
                    2,
                    2,
                    2,
                    0,
                    2,
                    2,
                    2,
                    2,
                    1
                },
                portTypes = new Resource.ResourceType[]
                {
                    Resource.ResourceType.Any,
                    Resource.ResourceType.Any,
                    Resource.ResourceType.Any,
                    Resource.ResourceType.Any,
                    Resource.ResourceType.Wool,
                    Resource.ResourceType.Grain,
                    Resource.ResourceType.Wood,
                    Resource.ResourceType.Brick,
                    Resource.ResourceType.Ore,
                },
                portAmounts = new int[]
                {
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1
                }
            }
            
            
        };
    }
}
