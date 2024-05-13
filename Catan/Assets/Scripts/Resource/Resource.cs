using UnityEngine;

namespace Catan.ResourcePhase
{

    public class Resource
    {
  
        public ResourceType type;

        public int amount;

        public Texture2D texture;

        public enum ResourceType
        {
            Any,
            Wool,
            Grain,
            Wood,
            Brick,
            Ore,
            None
        }


        public Resource(ResourceType type, int amount)
        {
            this.type = type;
            this.amount = amount;
        }

        public override string ToString()
        {
            return "(" + type.ToString() + ": " + amount + ")";
        }
    }
}
