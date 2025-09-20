using UnityEngine;

namespace ArtboxGames
{
    public class ShakeCamera : MonoBehaviour
    {
        public static ShakeCamera instance;

        private float shakeTimer;        //amount of time shake is going to last
        private float shakeAmount;  //intensity of the shake

        private Vector3 defaultPos;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        // Use this for initialization
        void Start()
        {
            defaultPos = transform.position;
        }

        // Update is called once per frame
        void Update()
        {

            if (shakeTimer >= 0)
            {
                Vector2 shakePos = Random.insideUnitCircle * shakeAmount;

                transform.position = new Vector3(transform.position.x + shakePos.x, transform.position.y + shakePos.y, transform.position.z);

                shakeTimer -= Time.deltaTime;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, defaultPos, 0.2f);
            }
        }

        public void OnShakeCamera(float shakePwr, float shakeDur)
        {
            shakeTimer = shakeDur;
            shakeAmount = shakePwr;
        }
    }
}