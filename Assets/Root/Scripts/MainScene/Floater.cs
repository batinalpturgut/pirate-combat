using System;
using Root.Scripts.Managers.Tick;
using Root.Scripts.Managers.Tick.Abstractions;
using Root.Scripts.Utilities.Abstractions;
using Root.Scripts.Utilities.Logger;
using UnityEngine;

namespace Root.Scripts.MainScene
{
    public class Floater : MonoBehaviour, IInitializer<TickManager>, IStandardTickable
    {
        // Roll z, Pitch x, Yaw y
        [SerializeField]
        private float waveHeight = 0.15f; // Geminin ne kadar yukari asagi gidecegini kontrol eder.

        [SerializeField]
        private float waveSpeed = 2f; // Geminin yukari asagi hareketinin ne kadar hizli olacagini kontrol eder.

        [SerializeField]
        private float tiltRollAngle; // Geminin saga sola yatma acisi. 

        [SerializeField]
        private float tiltPitchAngle; // Geminin yukari asagi yatma acisi. 

        [SerializeField]
        private float tiltRollSpeed; // Geminin saga sola yatma hizini kontrol eder.

        [SerializeField]
        private float tiltPitchSpeed; // Geminin yukari asagi yatma hizini kontrol eder.

        [SerializeField]
        private float turningTiltRollAngle;

        [SerializeField]
        private float turningTiltYawAngle;

        [SerializeField]
        private float turningTiltPitchAngle;

        [SerializeField]
        private float turningTiltRollSpeed;

        [SerializeField]
        private float turningTiltYawSpeed;

        [SerializeField]
        private float turningTiltPitchSpeed;

        private Vector3 _initialPos;

        private Quaternion _initialRot;

        private Vector3 _operand = new Vector3();

        private Vector3 _operand2 = new Vector3();

        private EShipState _currentState = EShipState.Neutral;

        private int _direction = 1;

        private bool _isTurning;
        private float _turningX;
        private float _turningY;
        private float _turningZ;
        private float _stabilizingTurningX;
        private float _stabilizingTurningY;
        private float _stabilizingTurningZ;
        private bool _canSimulateNeutralFloating;
        private float _startTime;
        private TickManager _tickManager;
        public int ExecutionOrder => 0;

        private enum EShipState
        {
            Leaning,
            Stabilizing,
            Neutral
        }

        private void Awake()
        {
            _initialPos = transform.localPosition;
            _initialRot = transform.localRotation;
        }

        private void Start()
        {
            _tickManager.Register(this);
        }

        void IStandardTickable.Tick()
        {
            Log.Console(_currentState);
            SimulateFloating();
            HandleTurns();
        }


        private void HandleTurns2()
        {
        }


        public void SetTurningStatus2(bool isTurning, bool isRight)
        {
            _isTurning = isTurning;

            if (isRight)
            {
                _direction = -1;
            }
            else
            {
                _direction = 1;
            }
        }

        private void HandleTurns()
        {
            if (_isTurning && _currentState != EShipState.Leaning &&
                _currentState != EShipState.Stabilizing) // Hem donmesi hem de lean olmamasi gerekir.
            {
                GetReadyForLeaning();
            }

            switch (_currentState)
            {
                case EShipState.Leaning:
                    _turningX = Mathf.Lerp(_turningX, _operand2.x, Time.deltaTime * turningTiltPitchSpeed);
                    _turningY = Mathf.Lerp(_turningY, _operand2.y, Time.deltaTime * turningTiltYawSpeed);
                    _turningZ = Mathf.Lerp(_turningZ, _operand2.z, Time.deltaTime * turningTiltRollSpeed);
                    transform.localRotation = Quaternion.Euler(_turningX, _turningY, _turningZ);
                    if (!_isTurning)
                    {
                        _currentState = EShipState.Stabilizing;
                        transform.localPosition = _initialPos;
                    }

                    break;
                case EShipState.Stabilizing:
                    _turningX = Mathf.Lerp(_turningX, _initialRot.x, Time.deltaTime * (turningTiltPitchSpeed + 2));
                    _turningY = Mathf.Lerp(_turningY, _initialRot.y, Time.deltaTime * (turningTiltYawSpeed + 2));
                    _turningZ = Mathf.Lerp(_turningZ, _initialRot.z, Time.deltaTime * (turningTiltRollSpeed + 2));
                    transform.localRotation = Quaternion.Euler(_turningX, _turningY, _turningZ);

                    if (Quaternion.Angle(transform.localRotation, _initialRot) < 0.5f)
                    {
                        _currentState = EShipState.Neutral;
                    }

                    break;
                case EShipState.Neutral:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void GetReadyForLeaning()
        {
            _currentState = EShipState.Leaning;
            transform.localRotation = _initialRot;
            transform.localPosition = _initialPos;
            _operand2.Set(turningTiltPitchAngle + transform.localEulerAngles.x,
                _direction * turningTiltYawAngle + transform.localEulerAngles.y,
                _direction * turningTiltRollAngle + transform.localEulerAngles.z);
            _turningX = transform.localEulerAngles.x;
            _turningY = transform.localEulerAngles.y;
            _turningZ = transform.localEulerAngles.z;
        }

        public void SetTurningStatus(bool isTurning, bool isRight)
        {
            _isTurning = isTurning;

            if (isRight)
            {
                _direction = -1;
            }
            else
            {
                _direction = 1;
            }
        }

        private void SimulateFloating()
        {
            if (_currentState != EShipState.Neutral)
            {
                _startTime = Time.time;
                return;
            }

            float waveOffset = Mathf.Sin((Time.time - _startTime) * waveSpeed) * waveHeight;
            float tiltRollOffset = Mathf.Sin((Time.time - _startTime) * tiltRollSpeed) * tiltRollAngle;
            float tiltPitchOffset = Mathf.Sin((Time.time - _startTime) * tiltPitchSpeed) * tiltPitchAngle;
            _operand.Set(transform.localPosition.x, _initialPos.y + waveOffset, transform.localPosition.z);
            transform.localPosition = _operand;

            if (_currentState != EShipState.Neutral)
            {
                return;
            }

            _operand.Set(_initialRot.x + tiltPitchOffset, transform.localEulerAngles.y, _initialRot.z + tiltRollOffset);
            transform.localRotation = Quaternion.Euler(_operand);
        }

        public void Initialize(TickManager tickManager)
        {
            _tickManager = tickManager;
        }


        void IStandardTickable.FixedTick()
        {
        }

        void IStandardTickable.LateTick()
        {
        }
    }
}