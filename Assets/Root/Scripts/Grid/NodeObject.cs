using Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile;

namespace Root.Scripts.Grid
{
    public class NodeObject
    {
        public NodePosition NodePosition { get; }
        public bool IsHostilePath { get; }
        public bool IsEntryPoint { get; }
        public bool HasDeployable => _deployable != null;
        public bool HasHostile => _hostileBlueprint != null;

        private IDeployable _deployable;
        private HostileBlueprint _hostileBlueprint;

        public NodeObject(NodePosition nodePosition, bool isHostilePath, bool isEntryPoint)
        {
            NodePosition = nodePosition;
            IsHostilePath = isHostilePath;
            IsEntryPoint = isEntryPoint;
        }

        public bool TryGetDeployable<T>(out T deployable)
        {
            if (IsHostilePath)
            {
                deployable = default;
                return false;
            }

            if (_deployable is T tDeployable)
            {
                deployable = tDeployable;
                return true;
            }

            deployable = default;
            return false;
        }


        public void AddDeployableToNode<T>(T deployable) where T : class, IDeployable
        {
            _deployable = deployable;
            _deployable.Place();
        }

        public void RemoveDeployableFromNode()
        {
            _deployable.Remove();
            _deployable = null;
        }

        public void AddHostileToNode(HostileBlueprint hostileBlueprint)
        {
            _hostileBlueprint = hostileBlueprint;
        }

        public void RemoveHostileFromNode()
        {
            _hostileBlueprint = null;
        }

        public bool TryGetHostile(out HostileBlueprint hostileBlueprint)
        {
            if (!_hostileBlueprint)
            {
                hostileBlueprint = null;
                return false;
            }

            hostileBlueprint = _hostileBlueprint;
            return true;
        }

        public override string ToString()
        {
            return NodePosition.ToString();
        }
    }
}