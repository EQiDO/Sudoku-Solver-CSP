using System;
using System.Collections.Generic;
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
        public string NodeValue { get; private set; }
        public HashSet<string> NodeDomain { get; set; }
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
        public void SetValue(string value)
        {
            NodeValue = value;
        }
        public void SetNodeGameObjectColor(Color color)
        {
            if (NodeGameObject == null) return;
            NodeGameObjectText.color = color;
        }
        public bool CompareValue(string value)
        {
            return NodeValue.Equals(value);
        }
        public void SetNodeData(string value, Color color)
        {
            SetValue(value);
            SetNodeGameObjectText(value);
            SetNodeGameObjectColor(color);
        }

        public void SetNodeGameObjectText(string value)
        {
            if (NodeGameObject == null) return;
            NodeGameObjectText?.SetText(value == "0" ? null : value);
        }

        #endregion
    }
}
