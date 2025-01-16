using System.Collections.Generic;
using UnityEngine;

namespace Assets._Scripts
{
    public class Grid
    {
        private readonly GameObject _nodeGameObject;
        private readonly Vector2 _gridWorldSize;
        private readonly float _nodeRadius;

        private Node[,] _grid;
        private float _nodeDiameter;
        private int _gridSizeX, _gridSizeY;

        private readonly List<int> _domain;

        public Grid(List<int> domain, GameObject nodeObj, Vector2 gridWorldSize, float nodeRadius)
        {
            _nodeGameObject = nodeObj;
            _gridWorldSize = gridWorldSize;
            _nodeRadius = nodeRadius;
            _domain = domain;
            InitializeGrid();
        }

        public Node GetNode(int x, int y)
        {
            return _grid[x, y];
        }
        private void InitializeGrid()
        {
            _nodeDiameter = _nodeRadius * 2;
            _gridSizeX = Mathf.RoundToInt(_gridWorldSize.x / _nodeDiameter);
            _gridSizeY = Mathf.RoundToInt(_gridWorldSize.y / _nodeDiameter);
            _grid = new Node[_gridSizeX, _gridSizeY];

            var worldBottomLeft = new Vector3(-_gridWorldSize.x / 2, 0, -_gridWorldSize.y / 2);
            var nodeHolder = new GameObject("Node Holder");
            for (var x = 0; x < _gridSizeX; x++)
            {
                for (var y = 0; y < _gridSizeY; y++)
                {
                    var worldPoint = worldBottomLeft + Vector3.right * (x * _nodeDiameter + _nodeRadius) + Vector3.forward * (y * _nodeDiameter + _nodeRadius);
                    var node = _grid[x, y] = new Node(
                        worldPoint, x, y
                    );

                    var nodeGameObject = InstantiateNodeGameObject(_nodeGameObject, worldPoint, nodeHolder);
                    node.SetNodeGameObject(nodeGameObject);
                    node.SetNodeData(float.NaN);
                }
            }
            AssignRandomNodes();
        }

        void AssignRandomNodes()
        {
            var assignedPositions = new HashSet<Vector2Int>();

            var fixedPercent = 0.5;
            var fixedCount = _gridSizeX * _gridSizeY * fixedPercent;

            for (var i = 0; i < fixedCount; i++)
            {
                var randomPosition = GetUniqueRandomPosition(assignedPositions);
                var randomValue = GetRandomValue();
                var node = _grid[randomPosition.x, randomPosition.y];
                if(CheckIsPossible(node, randomValue))
                    node.SetNodeData(randomValue);

            }
        }

        private bool CheckIsPossible(Node node, int value)
        {
            var x = node.Position.x;
            var y = node.Position.y;

            // Check row
            for (var i = 0; i < _gridSizeX; i++)
            {
                if(i == x) continue;
                var otherNode = _grid[i, y];
                if (otherNode.CompareValue(value))
                {
                    return false;
                }
            }

            // Check col
            for (var i = 0; i < _gridSizeY; i++)
            {
                if (i == y) continue;
                var otherNode = _grid[x, i];
                if (otherNode.CompareValue(value))
                {
                    return false;
                }
            }
            // Check 3x3 square

            // find bottom left 
            var squareX = (x / 3) * 3;
            var squareY = (y / 3) * 3;

            for (var i = 0; i < _gridSizeX / 3; i++) 
            {
                for (var j = 0; j < _gridSizeY / 3; j++)
                {
                    if (i == x && j == y) continue;
                    var otherNode = _grid[squareX + i, squareY + j];
                    if (otherNode.CompareValue(value))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private int GetRandomValue()
        {
            var randomIndex = Random.Range(0, _domain.Count);
            return _domain[randomIndex];
        }
        private Vector2Int GetUniqueRandomPosition(HashSet<Vector2Int> assignedPositions)
        {
            Vector2Int position;
            do
            {
                position = new Vector2Int(Random.Range(0, _gridSizeX), Random.Range(0, _gridSizeY));
            } while (assignedPositions.Contains(position));

            assignedPositions.Add(position);
            return position;
        }
        private GameObject InstantiateNodeGameObject(GameObject nodeObject, Vector3 position, GameObject holder)
        {
            var nodeGameObject = Object.Instantiate(nodeObject, position, Quaternion.identity);

            nodeGameObject.transform.parent = holder.transform;

            return nodeGameObject;
        }
    }
}
