using UnityEngine;

namespace ArtboxGames
{
    public class Spawner : MonoBehaviour
    {
        public static Spawner instance;

        //ref to target
        private GameObject target;
        [SerializeField]//time to spawn 1st enemyCar and then next missiles
        private float firstMissileTime = 2f, nextMissileTime = 4f;
        private float maxPointInScene = 3;//to limit the max points
        public float currentPoints = 0;

        //for spawning pickups at specific time
        [SerializeField]
        private int pickUpSpawnIncreaseMileStone;//milestone which determine when to spawn 
        private int pickUpSpawnMileStone;


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
        {   //get the ref to target
            //target = GameObject.FindGameObjectWithTag("Player");
            pickUpSpawnMileStone = pickUpSpawnIncreaseMileStone;
        }

        public void StartSpawning()
        {
            //call the method
            InvokeRepeating(nameof(SpawnEnemy), firstMissileTime, nextMissileTime);
        }

        void Update()
        {
            if (currentPoints < maxPointInScene && GameManager.instance.isGameOver == false
                && UIManager.instance.GameStarted == true)
                SpawnPoint();

            SpawnPickUp();
        }

        //method to spawn the missiles
        void SpawnEnemy()
        {    //we check if game is not started or game is over , if yes we exit from update
            if (GameManager.instance.isGameOver || UIManager.instance.GameStarted == false)
                return;
            //get the enemyCar object
            GameObject missile = ObjectPooling.instance.GetSpawnedEnemies();
            //set its transform
            missile.transform.position = new Vector3(randXMPos(), randYMPos(), 0f);
            missile.SetActive(true); //activate it
            missile.GetComponent<EnemyManager>().BasicSettings();
        }
        //method to spawn the point
        void SpawnPoint()
        {   //get the point
            GameObject point = ObjectPooling.instance.GetPoints();
            Vector3 pos = new Vector3(randXPos(), randYPos(), 0f);
            point.transform.position = pos;//set its position
            point.SetActive(true);//actiavte it
            point.GetComponent<PointManager>().BasicSettings();
            currentPoints++;
        }
        //method to spawn the point
        void SpawnPickUp()
        {
            if (GameManager.instance.currentScore > pickUpSpawnMileStone)
            {
                pickUpSpawnMileStone += pickUpSpawnIncreaseMileStone;
                //get the pickUp
                GameObject pickUp = null;

                int r = Random.Range(0, 2);
                if (r == 0)
                {
                    pickUp = ObjectPooling.instance.GetSpeedPickUps();
                }
                else
                {
                    pickUp = ObjectPooling.instance.GetShieldPickUps();
                }
                Vector3 pos = new Vector3(randXPos(), randYPos(), 0f);
                pickUp.transform.position = pos;//set its position
                pickUp.SetActive(true);//actiavte it
                pickUp.GetComponent<PickUpManager>().BasicSettings();
            }
        }
        //random x value to spawn point
        float randXPos()
        {   //we sapwn point in an area between 5 unit from car on x axis
            float x = Random.Range(target.transform.position.x - 5f, target.transform.position.x + 5f);
            return x;
        }
        //random y value to spawn point
        float randYPos()
        {   //we sapwn point in an area between 8 unit from car on y axis
            float y = Random.Range(target.transform.position.y - 8f, target.transform.position.y + 8f);
            return y;
        }
        //random x value to spawn enemyCar
        float randXMPos()
        {   //we sapwn point in an area between 5 unit from car on y axis
            float x = 0;
            x = Random.Range(target.transform.position.x - 5f, target.transform.position.x + 5f);
            //if the spawn x value in the screen space we recalcualte it
            while (Mathf.Abs(target.transform.position.x - x) < 3f)
                x = Random.Range(target.transform.position.x - 5f, target.transform.position.x + 5f);

            return x;
        }
        //random y value to spawn enemyCar
        float randYMPos()
        {
            float y = 0;
            y = Random.Range(target.transform.position.y - 8f, target.transform.position.y + 8f);

            while (Mathf.Abs(target.transform.position.y - y) < 5f)
                y = Random.Range(target.transform.position.y - 8f, target.transform.position.y + 8f);

            return y;
        }
    }
}