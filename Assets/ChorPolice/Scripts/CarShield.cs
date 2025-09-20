using UnityEngine;

namespace ArtboxGames
{
    public class CarShield : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                other.gameObject.SetActive(false);
                GameObject explosion = ObjectPooling.instance.GetExplosion();
                explosion.transform.position = transform.position;//set its transform
                explosion.SetActive(true);//activate it
                explosion.GetComponent<ExplosionManager>().BasicSettings(ObjectDestroyed.enemyCar);
                gameObject.SetActive(false);//deactivate this gameobject 
            }
        }
    }
}