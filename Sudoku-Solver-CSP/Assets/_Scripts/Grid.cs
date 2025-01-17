using System.Collections.Generic;
using System.Linq;
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

        private readonly List<int> _nodeDomain;

        public IEnumerable<Node> GetAllNodes => _grid.Cast<Node>();

        public Node EmptyNodeWithSmallestDomain =>
            GetAllNodes
                .Where(node => node.CompareValue(0))
                .OrderBy(node => node.NodeDomain.Count)
                .FirstOrDefault();

        public Grid(List<int> nodeDomain, GameObject nodeObj, Vector2 gridWorldSize, float nodeRadius)
        {
            _nodeGameObject = nodeObj;
            _gridWorldSize = gridWorldSize;
            _nodeRadius = nodeRadius;
            _nodeDomain = nodeDomain;
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

            var worldUpperLeft = new Vector3(-_gridWorldSize.x / 2, 0, _gridWorldSize.y / 2);
            var nodeHolder = new GameObject("Node Holder");

            for (var x = 0; x < _gridSizeX; x++)
            {
                for (var y = 0; y < _gridSizeY; y++)
                {
                    var worldPoint = worldUpperLeft +
                                     Vector3.right * (x * _nodeDiameter + _nodeRadius) +
                                     Vector3.back * (y * _nodeDiameter + _nodeRadius);

                    var node = _grid[x, y] = new Node(worldPoint, x, y);

                    var nodeGameObject = InstantiateNodeGameObject(_nodeGameObject, worldPoint, nodeHolder);
                    node.SetNodeGameObject(nodeGameObject);
                    node.SetNodeDomain(_nodeDomain);
                }
            }
            AssignPuzzle();
        }

        private void AssignPuzzle()
        {
            var simpleGrid = new int[9, 9]
            {
                { 4, 0, 0, 7, 0, 0, 1, 5, 8 },
                { 0, 1, 0, 9, 8, 0, 0, 2, 0 },
                { 8, 0, 0, 5, 0, 1, 0, 6, 9 },
                { 0, 9, 0, 0, 3, 0, 0, 0, 0 },
                { 6, 0, 7, 1, 9, 0, 2, 8, 0 },
                { 0, 8, 0, 0, 6, 2, 3, 9, 0 },
                { 0, 0, 6, 3, 0, 0, 9, 7, 2 },
                { 9, 0, 0, 0, 1, 0, 0, 3, 5 },
                { 0, 5, 2, 6, 0, 0, 8, 0, 1 }
            };
            var hardGrid = new int[9, 9]
            {
                { 4, 0, 0, 8, 0, 2, 0, 0, 5 },
                { 0, 5, 0, 0, 0, 3, 6, 0, 0 },
                { 1, 0, 0, 0, 9, 0, 8, 3, 0 },
                { 2, 0, 0, 0, 1, 9, 0, 8, 0 },
                { 0, 0, 0, 2, 6, 0, 0, 0, 0 },
                { 0, 0, 0, 4, 0, 0, 1, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 9, 0 },
                { 7, 1, 0, 5, 0, 0, 0, 0, 0 },
                { 0, 0, 2, 0, 0, 0, 3, 4, 0 }
            };
            var veryHardGrid = new int[9, 9]
            {
                { 8, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 3, 6, 0, 0, 0, 0, 0 },
                { 0, 7, 0, 0, 9, 0, 2, 0, 0 },
                { 0, 5, 0, 0, 0, 7, 0, 0, 0 },
                { 0, 0, 0, 0, 4, 5, 7, 0, 0 },
                { 0, 0, 0, 1, 0, 0, 0, 3, 0 },
                { 0, 0, 1, 0, 0, 0, 0, 6, 8 },
                { 0, 0, 8, 5, 0, 0, 0, 1, 0 },
                { 0, 9, 0, 0, 0, 0, 4, 0, 0 },
            };
            var hardestGrid = new int[9, 9]
            {
                {0, 6, 1, 0, 0, 7, 0, 0, 3},
                {0, 9, 2, 0, 0, 3, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 8, 5, 3, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 5, 0, 4},
                {5, 0, 0, 0, 0, 8, 0, 0, 0},
                {0, 4, 0, 0, 0, 0, 0, 0, 1},
                {0, 0, 0, 1, 6, 0, 8, 0, 0},
                {6, 0, 0, 0, 0, 0, 0, 0, 0 }

            };
            for (var x = 0; x < _gridSizeX; x++)
            {
                for (var y = 0; y < _gridSizeY; y++)
                {
                    var data = hardestGrid[y, x];
                    _grid[x, y].SetNodeData(data, Color.black);
                    if(data == 0) continue;
                    UpdateDomains(_grid[x, y], data, float.NaN);
                }
            }
        }

        public bool CheckIsPossible(Node node, int value)
        {
            return node.NodeDomain.Contains(value);
        }

        public void UpdateDomains(Node node, int value, float prevValue)
        {

            var x = node.Position.x;
            var y = node.Position.y;

            var restore = value == 0;

            // Check row
            for (var i = 0; i < _gridSizeX; i++)
            {
                if (i == x  && !restore) continue;
                var otherNode = _grid[i, y];

                var domainValue = restore ? prevValue : value;
                otherNode.UpdateNodeDomain((int)domainValue, restore);
            }

            // Check col
            for (var i = 0; i < _gridSizeY; i++)
            {
                if (i == y && !restore) continue;
                var otherNode = _grid[x, i];
                var domainValue = restore ? prevValue : value;
                otherNode.UpdateNodeDomain((int)domainValue, restore);
            }

            // Check 3x3 square
            var squareX = (x / 3) * 3;
            var squareY = (y / 3) * 3;

            for (var i = 0; i < _gridSizeX / 3; i++)
            {
                for (var j = 0; j < _gridSizeY / 3; j++)
                {
                    if (squareX + i == x && squareY + j == y && !restore) continue;
                    var otherNode = _grid[squareX + i, squareY + j];
                    var domainValue = restore ? prevValue : value;
                    otherNode.UpdateNodeDomain((int)domainValue, restore);
                }
            }
        }

        private GameObject InstantiateNodeGameObject(GameObject nodeObject, Vector3 position, GameObject holder)
        {
            var nodeGameObject = Object.Instantiate(nodeObject, position, Quaternion.identity);

            nodeGameObject.transform.parent = holder.transform;

            return nodeGameObject;
        }
    }
}
