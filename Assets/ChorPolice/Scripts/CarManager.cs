using UnityEngine;
using CnControls;

namespace ArtboxGames
{
    public class CarManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject shieldChildObj;
        //ref to rigidbody
        private Rigidbody2D myBody;
        [SerializeField]//move speed
        private float moveSpeed = 2f, speedPickUpBoost = 1f, speedBoostTime = 2.5f;
        [SerializeField]
        private AudioSource audioFX;//ref for fx
        private float defaultSpeed, currentSpeedBostTime;
        private AudioSource audioS;//ref to audio source
        Vector3 direction;//direction of movement
        float angle;//ganel to rotate
        float lastX, lastY;//this is for spawning cloud
        private bool speedBoostTimerOn = false;

        [HideInInspector]
        public VariablesManager vars;

        void OnEnable()
        {
            vars = Resources.Load("VariablesContainer") as VariablesManager;
        }

        // Use this for initialization
        void Start()
        {
            defaultSpeed = moveSpeed;
            //getting the component
            myBody = GetComponent<Rigidbody2D>();
            audioS = GetComponent<AudioSource>();
            audioS.clip = vars.carSound;
            lastX = lastY = 0;
            for (int i = 0; i < 3; i++)
            {
                SpawnCloud();
            }
        }

        // Update is called once per frame
        void Update()
        {    //we check if game is not started or game is over , if yes we exit from update
            if (GameManager.instance.isGameOver || UIManager.instance.GameStarted == false)
                return;

            if (speedBoostTimerOn)
                SpeedBoostTime();

            //when mouse is click
            if (Input.GetMouseButton(0))
            {   //we get the direction depending on mouse pos
                direction = new Vector3(CnInputManager.GetAxis("Horizontal"),
                CnInputManager.GetAxis("Vertical"), 0f);
                //Get joystick x position
                var x = CnInputManager.GetAxis("Horizontal");
                //Get joystick y position
                var y = CnInputManager.GetAxis("Vertical");
                //get the cangle from x and y values
                angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
            }
            //play audio if its not playing
            if (!audioS.isPlaying)
                audioS.Play();

            Movement();

            if (Mathf.Abs(lastX - transform.position.x) >= 2f)
            {
                SpawnCloud();
                lastX = transform.position.x;
            }
            else if (Mathf.Abs(lastY - transform.position.y) >= 3f)
            {
                SpawnCloud();
                lastY = transform.position.y;
            }
        }

        void FixedUpdate()
        {   //we check if game is not started or game is over , if yes we exit from update
            if (GameManager.instance.isGameOver || UIManager.instance.GameStarted == false)
                return;
            //move the car
            myBody.linearVelocity = direction * moveSpeed;
        }

        void Movement()
        {   //rotate the car         
            transform.rotation = Quaternion.AngleAxis(angle - 90, new Vector3(0, 0, 1));
        }
        //when collided
        void OnTriggerEnter2D(Collider2D other)
        {   //check if its score
            if (other.CompareTag("Score"))
            {   //if yes we incres the points
                GameManager.instance.currentScore += 2;
                GameManager.instance.currentStars++;
                GameManager.instance.coins++;
                GameManager.instance.Save();
                audioFX.PlayOneShot(vars.pickUpSound);
                //update the score text
                //**********Change
                UIManager.instance.coinCountText.text = "" + GameManager.instance.currentStars;
                UIManager.instance.timeText.text = "" + GameManager.instance.currentScore;
                other.gameObject.SetActive(false); //set the other game object deactive
                Spawner.instance.currentPoints--;
            }
            else if (other.CompareTag("SpeedPickUp"))
            {
                audioFX.PlayOneShot(vars.pickUpSound);
                moveSpeed = moveSpeed + speedPickUpBoost;
                speedBoostTimerOn = true;
                currentSpeedBostTime = speedBoostTime;
                other.gameObject.SetActive(false); //set the other game object deactive
            }
            else if (other.CompareTag("ShieldPickUp"))
            {
                audioFX.PlayOneShot(vars.pickUpSound);
                shieldChildObj.SetActive(true);
                other.gameObject.SetActive(false); //set the other game object deactive
            }
        }

        void SpawnCloud()
        {
            GameObject cloud = ObjectPooling.instance.GetClouds();
            Vector3 pos = new Vector3(randXPos(), randYPos(), 0f);
            cloud.transform.position = pos;//set its position
            cloud.SetActive(true);//actiavte it
        }

        //random x value to spawn cloud
        float randXPos()
        {   //we sapwn cloud in an area between 5 unit from car on y axis
            float x = 0;
            x = Random.Range(transform.position.x - 8f, transform.position.x + 8f);
            //if the spawn x value in the screen space we recalcualte it
            while (Mathf.Abs(transform.position.x - x) < 3f)
                x = Random.Range(transform.position.x - 8f, transform.position.x + 8f);

            return x;
        }
        //random y value to spawn cloud
        float randYPos()
        {
            float y = 0;
            y = Random.Range(transform.position.y - 10f, transform.position.y + 10f);

            while (Mathf.Abs(transform.position.y - y) < 5f)
                y = Random.Range(transform.position.y - 10f, transform.position.y + 10f);

            return y;
        }

        void SpeedBoostTime()
        {
            if (currentSpeedBostTime > 0)
            {
                currentSpeedBostTime -= Time.deltaTime;
                return;
            }

            speedBoostTimerOn = false;
            moveSpeed = defaultSpeed;
        }
    }
}