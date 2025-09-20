using UnityEngine;

namespace ArtboxGames
{
    public class PointManager : MonoBehaviour
    {   //ref to sign gameobject
        private GameObject pointSign = null;
        //ref to main camera and target(Player)
        private GameObject cameraObj, target;

        private SpriteRenderer signImg;

        [HideInInspector]
        public VariablesManager vars;

        void OnEnable()
        {
            vars = Resources.Load("VariablesContainer") as VariablesManager;
        }

        // Use this for initialization
        void Start()
        {
            signImg = GetComponent<SpriteRenderer>();
            signImg.sprite = vars.starSprite;

            //we get the player and camera and store them in the variables
            target = GameObject.FindGameObjectWithTag("Player");
            cameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        }

        // Update is called once per frame
        void Update()
        {   //checks if sign object is not null
            if (pointSign != null)
                SignControl();
        }
        //when point is spawned this method is called
        public void BasicSettings()
        {   //it get a sign from objectPooling and stores it
            pointSign = ObjectPooling.instance.GetPointSign();
        }
        //method which decide the position od sign depending on the distance between camera
        //and point and on which side of car the point is
        void SignControl()
        {   //the 3 and 5 are the half of screen width and height respectivily
            //on left side
            //here we see if the difference between point and camera x dis is more than 3 and 
            //difference between point and camera y dis is less than 5 and
            //if the difference between point and car x dis is less than zero
            if (Mathf.Abs(transform.position.x - cameraObj.transform.position.x) > 3 &&
                Mathf.Abs(transform.position.y - cameraObj.transform.position.y) < 5 &&
                (transform.position.x - target.transform.position.x) < 0)
            {   //then we set the sign pos whos x remain same and y changes depending on the
                //point y pos
                //we subtract 2.7 from car pos because we want sign to be at screen border
                pointSign.GetComponent<SignManager>().SetPos(target.transform.position.x - 2.7f,
                    transform.position.y);
                //we then set it active
                pointSign.SetActive(true);
            }
            //on right side
            else if (Mathf.Abs(transform.position.x - cameraObj.transform.position.x) > 3 &&
                Mathf.Abs(transform.position.y - cameraObj.transform.position.y) < 5 &&
                (transform.position.x - target.transform.position.x) > 0)
            {
                pointSign.GetComponent<SignManager>().SetPos(target.transform.position.x + 2.7f,
                    transform.position.y);
                pointSign.SetActive(true);
            }
            //on top side
            else if (Mathf.Abs(transform.position.y - cameraObj.transform.position.y) > 5 &&
                Mathf.Abs(transform.position.x - cameraObj.transform.position.x) < 3 &&
                (transform.position.y - target.transform.position.y) > 0)
            {
                pointSign.GetComponent<SignManager>().SetPos(transform.position.x,
                    target.transform.position.y + 4.7f);
                pointSign.SetActive(true);
            }
            //on bottom side
            else if (Mathf.Abs(transform.position.y - cameraObj.transform.position.y) > 5 &&
                Mathf.Abs(transform.position.x - cameraObj.transform.position.x) < 3 &&
                (transform.position.y - target.transform.position.y) < 0)
            {
                pointSign.GetComponent<SignManager>().SetPos(transform.position.x,
                    target.transform.position.y - 4.7f);
                pointSign.SetActive(true);
            }
            //on bottom left corner
            else if (Mathf.Abs(transform.position.y - cameraObj.transform.position.y) > 5 &&
                Mathf.Abs(transform.position.x - cameraObj.transform.position.x) > 3 &&
                (transform.position.y - target.transform.position.y) < 0 &&
                (transform.position.x - target.transform.position.x) < 0)
            {
                pointSign.GetComponent<SignManager>().SetPos(target.transform.position.x - 2.7f,
                    target.transform.position.y - 4.7f);
                pointSign.SetActive(true);
            }
            //on bottom right corner
            else if (Mathf.Abs(transform.position.y - cameraObj.transform.position.y) > 5 &&
                Mathf.Abs(transform.position.x - cameraObj.transform.position.x) > 3 &&
                (transform.position.y - target.transform.position.y) < 0 &&
                (transform.position.x - target.transform.position.x) > 0)
            {
                pointSign.GetComponent<SignManager>().SetPos(target.transform.position.x + 2.7f,
                    target.transform.position.y - 4.7f);
                pointSign.SetActive(true);
            }
            //on top left corner
            else if (Mathf.Abs(transform.position.y - cameraObj.transform.position.y) > 5 &&
                Mathf.Abs(transform.position.x - cameraObj.transform.position.x) > 3 &&
                (transform.position.y - target.transform.position.y) > 0 &&
                (transform.position.x - target.transform.position.x) < 0)
            {
                pointSign.GetComponent<SignManager>().SetPos(target.transform.position.x - 2.7f,
                    target.transform.position.y + 4.7f);
                pointSign.SetActive(true);
            }
            //on top right corner
            else if (Mathf.Abs(transform.position.y - cameraObj.transform.position.y) > 5 &&
                Mathf.Abs(transform.position.x - cameraObj.transform.position.x) > 3 &&
                (transform.position.y - target.transform.position.y) > 0 &&
                (transform.position.x - target.transform.position.x) > 0)
            {
                pointSign.GetComponent<SignManager>().SetPos(target.transform.position.x + 2.7f,
                    target.transform.position.y + 4.7f);
                pointSign.SetActive(true);
            }
            //in the screen area
            else if (Mathf.Abs(transform.position.y - cameraObj.transform.position.y) < 5 &&
                Mathf.Abs(transform.position.x - cameraObj.transform.position.x) < 3)
            {
                pointSign.SetActive(false);
            }
        }
    }
}