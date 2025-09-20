using UnityEngine;

namespace ArtboxGames
{
    public class CarSpawner : MonoBehaviour
    {

        public static CarSpawner instance;

        [SerializeField] //it has to equal to number of cars
        private GameObject[] carsPrefabs; //remember to assign prefabs in a same sequence as in shop

        [HideInInspector]
        public VariablesManager vars;

        void OnEnable()
        {
            vars = Resources.Load("VariablesContainer") as VariablesManager;
        }

        // Use this for initialization
        void Awake()
        {
            if (instance == null)
                instance = this;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SpawnPlane()
        {
            GameObject plane = Instantiate(carsPrefabs[GameManager.instance.selectedSkin], transform.position,
                Quaternion.identity);
            Spawner.instance.Target = plane;
            CameraFollower.instance.Target = plane;

            Spawner.instance.StartSpawning();
        }
    }
}