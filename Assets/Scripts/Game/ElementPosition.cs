using UnityEngine;


    public class ElementPosition
    {
        public Vector2 LocalPosition;
        public Vector2 GridPosition;

        public ElementPosition(Vector2 localPosition, Vector2 gridPosition)
        {
            LocalPosition = localPosition;
            GridPosition = gridPosition;
        }
    }