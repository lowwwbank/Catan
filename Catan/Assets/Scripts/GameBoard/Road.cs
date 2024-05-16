namespace Catan.GameBoard
{
    public class Road
    {
        public int xCoord;
        public int yCoord;
        public int xDataIndex;
        public int yDataIndex;


        public int playerIndex;

        public Road(int x, int y)
        {
            xDataIndex = x;
            yDataIndex = y;
            playerIndex = -1;
        }

        public override string ToString()
        {
            return xDataIndex + ", " + yDataIndex;
        }
    }
}
