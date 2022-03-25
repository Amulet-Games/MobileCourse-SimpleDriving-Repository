using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AG
{
    public class Car : MonoBehaviour
    {
        [Header("Decision Values")]
        [ReadOnlyInspector, SerializeField] float _delta;

        [Header("Config")]
        [SerializeField] float initSpeed = 20;
        [SerializeField] AnimationCurve initSpeedCurve;

        [Space(10)]
        [SerializeField] float speedAddRate = 2f;
        [SerializeField] float turnAmt = 200f;

        [Header("Status")]
        [ReadOnlyInspector, SerializeField] bool _hasReachedInitSpeed;
        [ReadOnlyInspector, SerializeField] float _curSpeed = 0;
        [ReadOnlyInspector, SerializeField] int _curSteerDir;

        [ReadOnlyInspector, SerializeField] float _curEvaluateValue;
        [ReadOnlyInspector, SerializeField] float _curEvaluateTime;
        [ReadOnlyInspector, SerializeField] float _lastCurveKeyFrameTime; 

        [Header("Static")]
        public static Car singleton;

        #region Callbacks.
        private void Awake()
        {
            if (singleton != null)
                Destroy(gameObject);
            else
                singleton = this;
        }

        private void Start()
        {
            GetLastKeyFrameTime();
        }

        private void Update()
        {
            UpdateDelta();

            if (_hasReachedInitSpeed)
            {
                MoveCarForward_Lin();
            }
            else
            {
                MoveCarForward_Cur();
            }

            RotateCarByInput();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            /// 6 is the obstacles layer
            if (other.gameObject.layer == 6)
            {
                SceneManager.LoadScene(0);
            }
        }
        #endregion

        void UpdateDelta()
        {
            _delta = Time.deltaTime;
        }

        void MoveCarForward_Lin()
        {
            _curSpeed += speedAddRate * _delta;
            transform.Translate(Vector3.forward * _curSpeed * _delta);
        }

        void MoveCarForward_Cur()
        {
            _curEvaluateTime += _delta;
            if (_curEvaluateTime >= _lastCurveKeyFrameTime)
            {
                _hasReachedInitSpeed = true;
            }

            _curEvaluateValue = initSpeedCurve.Evaluate(_curEvaluateTime);

            _curSpeed = (initSpeed * _curEvaluateValue);
            transform.Translate(Vector3.forward * _curSpeed * _delta);
        }

        void RotateCarByInput()
        {
            transform.Rotate(0f, _curSteerDir * turnAmt * _delta, 0f);
        }

        public void Steer(int value)
        {
            _curSteerDir = value;
        }

        #region Setup.
        void GetLastKeyFrameTime()
        {
            _lastCurveKeyFrameTime = initSpeedCurve.keys[initSpeedCurve.length - 1].time;
        }
        #endregion
    }
}