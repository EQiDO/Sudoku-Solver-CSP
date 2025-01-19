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
        [SerializeField] private float _gridSizeN = 9;
        [SerializeField] private float _nodeRadius = 0.5f;

        private readonly List<string> _domain = new();

        private Grid _grid;

        private void Awake()
        {
            for (var i = 1; i < _gridSizeN + 1; i++)
            {
                _domain.Add(i.ToString());
            }
            var size = new Vector2(_gridSizeN, _gridSizeN);
            _grid = new Grid(_domain, _nodeObj, size, _nodeRadius);
            
        }
        private void Start()
        {
            var sw = new Stopwatch();
            sw.Start();
            Debug.Log(Solve() ? "Solved!" : "Unsolvable!");
            sw.Stop();
            print($"time: {sw.Elapsed.TotalSeconds:F3}");
        }

        private bool Solve()
        {
            var node = _grid.EmptyNodeWithSmallestDomain;

            if (node == null)
            {
                return true;
            }

            foreach (var value in node.NodeDomain.ToList())
            {
                node.SetNodeData(value, Color.red);
                _grid.UpdateDomains(node, value, false);
                if (Solve())
                    return true;
                node.SetNodeData("0", Color.red);
                _grid.UpdateDomains(node, value, true);
            }

            return false;

        }

    }
}
