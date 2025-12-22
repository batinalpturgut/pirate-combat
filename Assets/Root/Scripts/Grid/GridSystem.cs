using Root.Scripts.Managers.Grid;
using Root.Scripts.ScriptableObjects.Island;
using Root.Scripts.Utilities.Spawner;
using UnityEngine;

namespace Root.Scripts.Grid
{
    public class GridSystem
    {
        private readonly int _width;
        private readonly int _height;
        private static float _cellSize;
        private readonly NodeObject[,] _nodeObjectArr;
        private readonly GridManager _gridManager;
        private readonly Road[] _roads;

        public GridSystem(int width, int height, float cellSize, Road[] road, GridManager gridManager)
        {
            _roads = road;
            _gridManager = gridManager;
            _width = width;
            _height = height;
            _cellSize = cellSize;
            _nodeObjectArr = new NodeObject[_width, _height];
            for (int x = 0; x < _width; x++)
            {
                for (int z = 0; z < _height; z++)
                {
                    NodePosition nodePosition = new NodePosition(x, z);
                    _nodeObjectArr[x, z] =
                        new NodeObject(nodePosition, IsHostilePosition(nodePosition), x == 0 && z == 0);
                }
            }
        }

        private bool IsHostilePosition(NodePosition nodePosition)
        {
            foreach (var road in _roads)
            {
                if (road.Path.Contains(nodePosition))
                {
                    return true;
                }
            }

            return false;
        }

        public void CreateNodeTextObjects(Transform prefab)
        {
            for (int x = 0; x < _width; x++)
            {
                for (int z = 0; z < _height; z++)
                {
                    NodePosition nodePosition = new NodePosition(x, z);
                    Vector3 worldPosition = GetWorldPosition(nodePosition);
                    GridBottomLayer nodeText =
                        Spawner.Spawn<GridBottomLayer>(prefab, worldPosition, Quaternion.identity);
                    nodeText.NodeObject = GetNodeObjectWithNodePosition(nodePosition);
                    nodeText.transform.SetParent(_gridManager.NodeTextParent, false);
                }
            }
        }

        public NodeObject GetNodeObjectWithNodePosition(NodePosition nodePosition)
        {
            return _nodeObjectArr[(int)nodePosition.x, (int)nodePosition.z];
        }

        public NodePosition GetNodePosition(Vector3 worldPosition)
        {
            return new NodePosition(
                Mathf.RoundToInt(worldPosition.x / _cellSize),
                Mathf.RoundToInt(worldPosition.z / _cellSize)
            );
        }

        public Vector3 GetWorldPosition(NodePosition nodePosition)
        {
            return new Vector3(nodePosition.x * _cellSize, 0, nodePosition.z * _cellSize);
        }

        public bool IsValidNodePosition(NodePosition nodePosition)
        {
            return nodePosition.x >= 0 && nodePosition.z >= 0 && nodePosition.x < _width && nodePosition.z < _height;
        }
    }
}