using System;
using TMPro;
using UnityEngine;

namespace Assets._Scripts
{
    public class Node
    {
        #region Properties

        #region Immutable
        public Vector3 WorldPosition { get; }
        public int GridX { get; }
        public int GridY { get; }
        public Vector2Int Position => new(GridX, GridY);
        #endregion

        #region Mutable
        public GameObject NodeGameObject { get; private set; }
        public TMP_Text NodeGameObjectText => NodeGameObject?.GetComponentInChildren<TMP_Text>();
        public float NodeValue { get; private set; }
        #endregion

        #endregion

        #region Ctor
        public Node(Vector3 worldPosition, int gridX, int gridY)
        {
            WorldPosition = worldPosition;
            GridX = gridX;
            GridY = gridY;
        }
        #endregion

        #region Public Methods


        public void SetNodeGameObject(GameObject nodeGameObject)
        {
            NodeGameObject = nodeGameObject;
        }
        public void SetValue(float value)
        {
            NodeValue = value;
        }

        public bool CompareValue(int value)
        {
            return Math.Abs(NodeValue - value) < 0.001;
        }
        public void SetNodeData(float value)
        {
            SetValue(value);
            SetNodeGameObjectText(value);
        }
        
        public void SetNodeGameObjectText(float value)
        {
            if (NodeGameObject == null) return;
            NodeGameObjectText?.SetText(float.IsNaN(value) ? null : value.ToString());
        }

        #endregion
    }
}
