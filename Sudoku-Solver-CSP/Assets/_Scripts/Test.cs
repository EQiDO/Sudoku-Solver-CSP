using System.Collections.Generic;
using UnityEngine;

namespace Assets._Scripts
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private GameObject _nodeObj;
        //[SerializeField] private Vector2 _gridWorldSize;
        [SerializeField] private float _gridSizeX = 9;
        [SerializeField] private float _gridSizeY = 9;
        [SerializeField] private float _nodeRadius = 0.5f;

        private readonly List<int> _domain = new() {1, 2, 3, 4, 5, 6, 7, 8, 9};
        void Start()
        {
            var size = new Vector2(_gridSizeX, _gridSizeY);
            var grid = new Grid(_domain,_nodeObj, size, _nodeRadius);
        }
    }
}
