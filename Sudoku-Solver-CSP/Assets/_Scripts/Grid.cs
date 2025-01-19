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
        private int _gridSizeN;

        private readonly List<string> _nodeDomain;

        public IEnumerable<Node> GetAllNodes => _grid.Cast<Node>();

        public Node EmptyNodeWithSmallestDomain =>
            GetAllNodes
                .Where(node => node.CompareValue("0"))
                .OrderBy(node => node.NodeDomain.Count)
                .FirstOrDefault();

        public Grid(List<string> nodeDomain, GameObject nodeObj, Vector2 gridWorldSize, float nodeRadius)
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
            _gridSizeN = Mathf.RoundToInt(_gridWorldSize.x / _nodeDiameter);
            _grid = new Node[_gridSizeN, _gridSizeN];

            var worldUpperLeft = new Vector3(-_gridWorldSize.x / 2, 0, _gridWorldSize.y / 2);
            var nodeHolder = new GameObject("Node Holder");

            for (var x = 0; x < _gridSizeN; x++)
            {
                for (var y = 0; y < _gridSizeN; y++)
                {
                    var worldPoint = worldUpperLeft +
                                     Vector3.right * (x * _nodeDiameter + _nodeRadius) +
                                     Vector3.back * (y * _nodeDiameter + _nodeRadius);

                    var node = _grid[x, y] = new Node(worldPoint, x, y);

                    var nodeGameObject = InstantiateNodeGameObject(_nodeGameObject, worldPoint, nodeHolder);
                    node.SetNodeGameObject(nodeGameObject);
                    node.NodeDomain = new HashSet<string>(_nodeDomain);
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

            var mediumGrid = new int[9, 9]
            {
                {2, 0, 5, 1, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 1, 0, 0},
                {8, 0, 0, 0, 9, 0, 7, 2, 0},
                {7, 0, 1, 5, 2, 0, 0, 0, 8},
                {0, 0, 0, 0, 0, 0, 9, 0, 0},
                {4, 0, 6, 9, 1, 0, 0, 0, 3},
                {1, 0, 0, 0, 8, 0, 4, 6, 0},
                {0, 0, 0, 0, 0, 0, 5, 0, 0},
                {9, 0, 7, 4, 0, 0, 0, 0, 0}
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
            var largeGrid = new int[16, 16]
            {
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},

            };


            for (var x = 0; x < _gridSizeN; x++)
            {
                for (var y = 0; y < _gridSizeN; y++)
                {
                    var data = hardestGrid[y, x].ToString();
                    var node = _grid[x, y];
                    node.SetNodeData(data, Color.black);
                    if(data == "0") continue;
                    UpdateDomains(node, data, false);
                }
            }
        }

        public void UpdateDomains(Node node, string value, bool backTrack)
        {
            var x = node.Position.x;
            var y = node.Position.y;

            // row
            for (var i = 0; i < _gridSizeN; i++)
            {
                if (i == x) continue; 
                var otherNode = _grid[i, y];
                if(!backTrack)
                    otherNode.NodeDomain.Remove(value);

                else if(IsValueSafeForNode(otherNode, value))
                    otherNode.NodeDomain.Add(value);
            }
            //col
            for (var i = 0; i < _gridSizeN; i++)
            {
                if (i == y) continue;
                var otherNode = _grid[x, i];

                if (!backTrack)
                    otherNode.NodeDomain.Remove(value);

                else if (IsValueSafeForNode(otherNode, value))
                    otherNode.NodeDomain.Add(value);
            }
            //subGrid
            var subGridSize = (int)Mathf.Sqrt(_gridSizeN);
            var startX = (x / subGridSize) * subGridSize;
            var startY = (y / subGridSize) * subGridSize;

            for (var i = startX; i < startX + subGridSize; i++)
            {
                for (var j = startY; j < startY + subGridSize; j++)
                {
                    if (i == x && j == y) continue;
                    var otherNode = _grid[i, j];

                    if (!backTrack)
                        otherNode.NodeDomain.Remove(value);

                    else if (IsValueSafeForNode(otherNode, value))
                        otherNode.NodeDomain.Add(value);
                }
            }
        }

        private bool IsValueSafeForNode(Node node, string value)
        {
            return !IsValueInRow(node, value) && !IsValueInColumn(node, value) && !IsValueInSubGrid(node, value);
        }

        private bool IsValueInRow(Node node, string value)
        {
            var x = node.Position.x;
            var y = node.Position.y;

            for (var i = 0; i < _gridSizeN; i++)
            {
                if(i == x) continue;

                var otherNode = _grid[i, y];
                if (otherNode.CompareValue(value))
                    return true;
            }
            return false;
        }

        private bool IsValueInColumn(Node node, string value)
        {
            var x = node.Position.x;
            var y = node.Position.y;
            for (var i = 0; i < _gridSizeN; i++)
            {
                if (i == y) continue;

                var otherNode = _grid[x, i];
                if (otherNode.CompareValue(value))
                    return true;
            }
            return false;
        }

        private bool IsValueInSubGrid(Node node, string value)
        {
            var x = node.Position.x;
            var y = node.Position.y;

            var subGridSize = (int)Mathf.Sqrt(_gridSizeN);
            var startX = (node.Position.x / subGridSize) * subGridSize;
            var startY = (node.Position.y / subGridSize) * subGridSize;

            for (var i = startX; i < startX + subGridSize; i++)
            {
                for (var j = startY; j < startY + subGridSize; j++)
                {
                    if (i == x && j == y) continue;
                    var otherNode = _grid[i, j];
                    if (otherNode.CompareValue(value))
                        return true;
                }
            }
            return false;
        }

        private GameObject InstantiateNodeGameObject(GameObject nodeObject, Vector3 position, GameObject holder)
        {
            var nodeGameObject = Object.Instantiate(nodeObject, position, Quaternion.identity);

            nodeGameObject.transform.parent = holder.transform;

            return nodeGameObject;
        }
    }
}
