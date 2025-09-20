using UnityEngine;

namespace ArtboxGames
{
    public class CameraFollower : MonoBehaviour
    {
        public static CameraFollower instance;

        private GameObject target;

        public GameObject Target
        {
            get { return target; }
            set { target = value; }
        }

        void Awake()
        {
            if (instance == null)
                instance = this;
        }

        // Use this for initialization
        void Start()
        {
            //target = GameObject.FindGameObjectWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {
            if (target != null)
                transform.position = new Vector3(target.transform.position.x,
                    target.transform.position.y, transform.position.z);
        }
    }
}