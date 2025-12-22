using Root.Scripts.Extensions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Abstractions;
using Root.Scripts.Gameplay.TowerDefense.Entities.Hostile.Abstractions;
using UnityEngine;

namespace Root.Scripts.Gameplay.TowerDefense.Entities.Defenders.Ship
{
    public static class ShipBehaviours
    {
        public static void RotateToTarget(IShooter shooter)
        {
            Vector3 direction = (shooter.Target.transform.position - shooter.Transform.position).normalized;
            float rotateAngle = Vector3.SignedAngle(shooter.ShootPosition, direction, Vector3.up);
            shooter.Transform.Rotate(Vector3.up, rotateAngle * Time.deltaTime * shooter.ShooterRotateSpeed);
        }

        public static bool InShootingLimits(IShooter shooter)
        {
            Vector3 rightLimit = Quaternion.AngleAxis(shooter.Angle * 0.5f, Vector3.up) * shooter.ShootPosition;
            Vector3 leftLimit = Quaternion.AngleAxis(-shooter.Angle * 0.5f, Vector3.up) * shooter.ShootPosition;

            Vector3 direction = (shooter.Target.transform.position - shooter.Transform.position).normalized;

            // Direction sol limitin saginda ise ve sag limitin solunda ise menzil icindedir.
            bool rightCondition = Vector3.Cross(rightLimit, direction).y < 0f;
            bool leftCondition = Vector3.Cross(leftLimit, direction).y > 0f;

            Debug.DrawLine(shooter.Transform.position, shooter.Transform.position + leftLimit * shooter.Range,
                Color.blue);
            Debug.DrawLine(shooter.Transform.position, shooter.Transform.position + rightLimit * shooter.Range,
                Color.blue);

            if (leftCondition && rightCondition)
            {
                Debug.DrawLine(shooter.Transform.position, shooter.Target.transform.position, Color.green);
                return true;
            }

            Debug.DrawLine(shooter.Transform.position, shooter.Target.transform.position, Color.red);
            return false;
        }
        
        public static void PlaySwimEffect(IFloatable floatable)
        {
            float wave = Mathf.Sin((Time.time + floatable.WaveOffset ) * floatable.WaveSpeed);
            float positionWave = wave.Remap(-1, 1, -0.03f, 0.03f) * Time.deltaTime;
            float rotationWave = wave.Remap(-1, 1, -5f, 5f) * Time.deltaTime;
            
            Vector3 localPosition = floatable.Transform.localPosition;
            localPosition = new Vector3(
                localPosition.x,
                localPosition.y + positionWave,
                localPosition.z
            );
            floatable.Transform.localPosition = localPosition;

            Quaternion localRotation = floatable.Transform.localRotation;
            localRotation = Quaternion.Euler(
                localRotation.eulerAngles.x + rotationWave,
                localRotation.eulerAngles.y,
                localRotation.eulerAngles.z + rotationWave * floatable.DirectionMultiplier);
            floatable.Transform.localRotation = localRotation;
        }
    }
}