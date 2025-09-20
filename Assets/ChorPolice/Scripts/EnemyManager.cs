using UnityEngine;

namespace ArtboxGames
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] //basic settings of car
        private float speed = 5, rotatingSpeed = 200, carTime = 30f;
        private float currentCarTime;
        private GameObject target; //ref to target to follow (Player)
        private GameObject carSign = null;//ref to sign
        private Rigidbody2D myBody;//ref to rigidbody
        private GameObject cameraObj;//ref to camera
        private AudioSource audioS;//ref to audio source

        public VariablesManager vars;

        void OnEnable()
        {
            vars = Resources.Load("VariablesContainer") as VariablesManager;
        }

        // Use this for initialization
        void Start()
        {   //we get audioSource component
            audioS = GetComponent<AudioSource>();
            audioS.clip = vars.enemyCarSound;
            //get the target and camera
            target = GameObject.FindGameObjectWithTag("Player");
            cameraObj = GameObject.FindGameObjectWithTag("MainCamera");
            //get the rigidbody
            myBody = GetComponent<Rigidbody2D>();
            //set currentMissile time to enemyCar time
            currentCarTime = carTime;
        }

        void Update()
        {   //we check if game is not started or game is over , if yes we exit from update
            if (GameManager.instance.isGameOver || UIManager.instance.GameStarted == false)
            {
                myBody.velocity = Vector3.zero;
                myBody.angularVelocity = 0f;
                return;
            }
            //check if the sign is not null
            if (carSign != null)
                SignControl();
            //check if the current time is more than zero
            if (currentCarTime > 0)//if yes we then reduce it by 1 every seconds
                currentCarTime -= Time.deltaTime;
            else if (currentCarTime <= 0)//when it become 0 or less
                Explode(ObjectDestroyed.enemyCar);//explode is called we tell which object it it
                                                  //we get distance between enemyCar and target
            float distance = Vector3.SqrMagnitude(target.transform.position - transform.position);
            //depending on distance we change the pitch of enemyCar sound
            audioS.volume = (0.5f / distance);

        }

        void FixedUpdate()
        {   //we check if game is not started or game is over , if yes we exit from update
            if (GameManager.instance.isGameOver || UIManager.instance.GameStarted == false)
                return;
            //we get the direction
            Vector2 point2Target = (Vector2)transform.position - (Vector2)target.transform.position;
            point2Target.Normalize();//normalize it
                                     //we then get the cross product on z axis
            float value = Vector3.Cross(point2Target, transform.right).z;
            #region Some Extra Code
            //if (value > 0)
            //{
            //    myBody.angularVelocity = rotatingSpeed;
            //}
            //else if (value < 0)
            //{
            //    myBody.angularVelocity = -rotatingSpeed;
            //}
            //else
            //{
            //    myBody.angularVelocity = 0;
            //}
            #endregion
            //we then rotate the enemyCar toward the target 
            myBody.angularVelocity = rotatingSpeed * value;
            //and give it speed
            myBody.velocity = transform.right * speed;
        }
        //method called when the enemyCar is spawned
        public void BasicSettings()
        {   //get ref to sign
            carSign = ObjectPooling.instance.GetEnemySign();
            //reset the current time
            currentCarTime = carTime;
        }

        void SignControl()
        {
            //on left side
            if (Mathf.Abs(transform.position.x - cameraObj.transform.position.x) > 3 &&
                Mathf.Abs(transform.position.y - cameraObj.transform.position.y) < 5 &&
                (transform.position.x - target.transform.position.x) < 0)
            {
                carSign.GetComponent<SignManager>().SetPos(target.transform.position.x - 2.7f,
                    transform.position.y);
                carSign.SetActive(true);
            }
            //on right side
            else if (Mathf.Abs(transform.position.x - cameraObj.transform.position.x) > 3 &&
                Mathf.Abs(transform.position.y - cameraObj.transform.position.y) < 5 &&
                (transform.position.x - target.transform.position.x) > 0)
            {
                carSign.GetComponent<SignManager>().SetPos(target.transform.position.x + 2.7f,
                    transform.position.y);
                carSign.SetActive(true);
            }
            //on top side
            else if (Mathf.Abs(transform.position.y - cameraObj.transform.position.y) > 5 &&
                Mathf.Abs(transform.position.x - cameraObj.transform.position.x) < 3 &&
                (transform.position.y - target.transform.position.y) > 0)
            {
                carSign.GetComponent<SignManager>().SetPos(transform.position.x,
                    target.transform.position.y + 4.7f);
                carSign.SetActive(true);
            }
            //on bottom side
            else if (Mathf.Abs(transform.position.y - cameraObj.transform.position.y) > 5 &&
                Mathf.Abs(transform.position.x - cameraObj.transform.position.x) < 3 &&
                (transform.position.y - target.transform.position.y) < 0)
            {
                carSign.GetComponent<SignManager>().SetPos(transform.position.x,
                    target.transform.position.y - 4.7f);
                carSign.SetActive(true);
            }
            //on bottom left corner
            else if (Mathf.Abs(transform.position.y - cameraObj.transform.position.y) > 5 &&
                Mathf.Abs(transform.position.x - cameraObj.transform.position.x) > 3 &&
                (transform.position.y - target.transform.position.y) < 0 &&
                (transform.position.x - target.transform.position.x) < 0)
            {
                carSign.GetComponent<SignManager>().SetPos(target.transform.position.x - 2.7f,
                    target.transform.position.y - 4.7f);
                carSign.SetActive(true);
            }
            //on bottom right corner
            else if (Mathf.Abs(transform.position.y - cameraObj.transform.position.y) > 5 &&
                Mathf.Abs(transform.position.x - cameraObj.transform.position.x) > 3 &&
                (transform.position.y - target.transform.position.y) < 0 &&
                (transform.position.x - target.transform.position.x) > 0)
            {
                carSign.GetComponent<SignManager>().SetPos(target.transform.position.x + 2.7f,
                    target.transform.position.y - 4.7f);
                carSign.SetActive(true);
            }
            //on top left corner
            else if (Mathf.Abs(transform.position.y - cameraObj.transform.position.y) > 5 &&
                Mathf.Abs(transform.position.x - cameraObj.transform.position.x) > 3 &&
                (transform.position.y - target.transform.position.y) > 0 &&
                (transform.position.x - target.transform.position.x) < 0)
            {
                carSign.GetComponent<SignManager>().SetPos(target.transform.position.x - 2.7f,
                    target.transform.position.y + 4.7f);
                carSign.SetActive(true);
            }
            //on top right corner
            else if (Mathf.Abs(transform.position.y - cameraObj.transform.position.y) > 5 &&
                Mathf.Abs(transform.position.x - cameraObj.transform.position.x) > 3 &&
                (transform.position.y - target.transform.position.y) > 0 &&
                (transform.position.x - target.transform.position.x) > 0)
            {
                carSign.GetComponent<SignManager>().SetPos(target.transform.position.x + 2.7f,
                    target.transform.position.y + 4.7f);
                carSign.SetActive(true);
            }
            //in the screen area
            else if (Mathf.Abs(transform.position.y - cameraObj.transform.position.y) < 5 &&
                Mathf.Abs(transform.position.x - cameraObj.transform.position.x) < 3)
            {
                carSign.SetActive(false);
            }
        }
        //method called when the obj is destroyed 
        void Explode(ObjectDestroyed obj)
        {   //we get the explosion
            GameObject explosion = ObjectPooling.instance.GetExplosion();
            explosion.transform.position = transform.position;//set its transform
            explosion.SetActive(true);//activate it
            explosion.GetComponent<ExplosionManager>().BasicSettings(obj);
            gameObject.SetActive(false);//deactivate this gameobject 
        }
        //identify the collider
        void OnTriggerEnter2D(Collider2D other)
        {   //if its player
            if (other.CompareTag("Player"))
            {   //deactivate the player
                other.gameObject.SetActive(false);
                //game is over
                GameManager.instance.isGameOver = true;
                //call gameover method from gui
                UIManager.instance.GameOver();
                //and do explosion
                Explode(ObjectDestroyed.car);
                Vibration.VibratePop();
            }
            else if (other.CompareTag("Enemy"))
            {
                carSign.SetActive(false);
                Explode(ObjectDestroyed.enemyCar);
            }
        }
    }
}