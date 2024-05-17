using UnityEngine;
using Catan.ResourcePhase;
using System.Linq;
using System.Text;

namespace Catan.Players
{

    public class Player
    {
        public Color playerColor;
        public Color primaryUIColor;
        public Color secondaryUIColor;

        public string playerName;

        public int playerIndex;

        public int victoryPoints;

        public int longestRoadLength;

        public bool longestRoad;

        public Resource[] resources;

        public int resourceSum
        {
            get
            {
                int sum = 0;
                foreach (Resource r in resources)
                {
                    if (r != null) sum += r.amount;
                }
                return sum;
            }
        }

        public void AddResource(Resource.ResourceType resource, int amount)
        {
            if (resource == Resource.ResourceType.None || resource == Resource.ResourceType.Any)
            {
                return;
            }
            resources.Where(rs => rs.type == resource).First().amount += amount;
        }

        public Resource RandomResource()
        {
            int rand = Random.Range(0, resources.Length);
            int rIndex = rand;

            for (int i = 0; i < resources.Length; i++)
            {
                if (resources[(i + rand) % resources.Length].amount > 0)
                {
                    rIndex = (i + rand) % resources.Length;
                }
            }

            return new Resource(resources[rIndex].type, 1);
        }

        public bool HasResource(Resource.ResourceType toTest, int amount = 1)
        {
            return resources.Where(r => r.type == toTest).First().amount >= amount;
        }

        public Player()
        {
            
        }

        public override string ToString()
        {
            string sb = "";
            sb += "'" + playerName + "' with resources: ";
            foreach (Resource r in resources)
            {
                sb += r.ToString();
            }

            return sb;
        }
    }
}
