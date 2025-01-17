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
        public float NodeValue { get; private set; }
        public HashSet<int> NodeDomain { get; private set; }
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
        public void SetNodeGameObjectColor(Color color)
        {
            if (NodeGameObject == null) return;
            NodeGameObjectText.color = color;
        }
        public bool CompareValue(float value)
        {
            return Math.Abs(NodeValue - value) < 0.001;
        }

        public void SetNodeDomain(List<int> domain)
        {
            NodeDomain = new HashSet<int>(domain);
        }
        public void UpdateNodeDomain(int value, bool add)
        {
            if (add)
                NodeDomain.Add(value);
            else
                NodeDomain.Remove(value);
        }
        public void SetNodeData(float value, Color color)
        {
            SetValue(value);
            SetNodeGameObjectText(value);
            SetNodeGameObjectColor(color);
        }

        public void SetNodeGameObjectText(float value)
        {
            if (NodeGameObject == null) return;
            NodeGameObjectText?.SetText(value == 0 ? null : value.ToString());
        }

        #endregion
    }
}
