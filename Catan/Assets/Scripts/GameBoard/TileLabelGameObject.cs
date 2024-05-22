using UnityEngine;

namespace Catan.GameBoard
{
    public class TileLabelGameObject : MonoBehaviour
    {
        private void OnMouseDown()
        {
            transform.parent.parent.GetChild(0).GetComponent<TileGameObject>().OnMouseDown();
        }
    }
}

