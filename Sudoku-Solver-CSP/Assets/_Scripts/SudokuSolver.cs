using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets._Scripts
{
    public class SudokuSolver : MonoBehaviour
    {
        [SerializeField] private GameObject _nodeObj;
        [SerializeField] private float _gridSizeX = 9;
        [SerializeField] private float _gridSizeY = 9;
        [SerializeField] private float _nodeRadius = 0.5f;

        private readonly List<int> _domain = new() {1, 2, 3, 4, 5, 6, 7, 8, 9};

        private Grid _grid;

        private void Awake()
        {
            var size = new Vector2(_gridSizeX, _gridSizeY);
            _grid = new Grid(_domain, _nodeObj, size, _nodeRadius);
        }
        private void Start()
        {
            var sw = new Stopwatch();
            sw.Start();
            Debug.Log(Solve() ? "Solved!" : "Unsolvable!");
            sw.Stop();
            print(sw.ElapsedMilliseconds);
        }

        private bool Solve()
        {
            var node = _grid.EmptyNodeWithSmallestDomain;

            if (node == null)
            {
                return true;
            }

            var row = node.Position.x;
            var col = node.Position.y;

            foreach (var value in node.NodeDomain.ToList().Where(value => _grid.CheckIsPossible(node, value)))
            {
                node.SetNodeData(value, Color.red);
                _grid.UpdateDomains(node, value, float.NaN);
                if (Solve())
                    return true;
                var backTrackNode = _grid.GetNode(row, col);
                var prevValue = backTrackNode.NodeValue;
                backTrackNode.SetNodeData(0, Color.red);
                _grid.UpdateDomains(backTrackNode, 0, prevValue);
            }

            return false;

        }

    }
}
