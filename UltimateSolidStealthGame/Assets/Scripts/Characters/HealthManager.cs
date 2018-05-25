using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Class that allows characters to register damage
*/
public class HealthManager : MonoBehaviour {

	/*
		current health of character
	*/
	[SerializeField]
	float health;
    [SerializeField]
    GameObject healthBarObject;

	/*
		reference to CharacterManager component of gameObject, either Enemy or Player
	*/
	CharacterManager manager;
    HealthBar healthBar;
    bool playerHealth;
    

	public float Health {
		get { return health; }
	}
		
	void Start () {
		manager = GetComponent<CharacterManager> ();
        if (healthBarObject) {
            Vector3 loc = transform.position;
            loc[1] += 1.5f;
            GameObject temp = Instantiate(healthBarObject, loc, Quaternion.identity);
            if (temp) {
                healthBar = temp.GetComponent<HealthBar>();
                if (healthBar) {
                    healthBar.MaxHealth = health;
                    healthBar.Target = this.gameObject;
                }
            }
        }
        if (manager.GetType() == typeof(PlayerManager)) {
            playerHealth = true;
        }
	}

	/*
		Registers bullet damage
		@param collision - info on colliding object
	*/
	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.CompareTag ("Bullet") == true) {
			Bullet bullet = collision.gameObject.GetComponent<Bullet> ();
			if (bullet != null) {
				Attack (bullet.Damage);
			}
		}
	}

	/*
		causes damage to character
		@param damage - amount of damage to take
	*/
	public void Attack(float damage) {
        if (playerHealth && PlayerPrefs.GetInt("Vibration", 1) == 1) {
            Handheld.Vibrate();
        }
		health = (health > 0.0f) ? health - damage : 0.0f;
        if (healthBar) {
            healthBar.MinusHealth(health);
        }
		if (health <= 0.0f) {
			manager.Kill ();
		}
	}
}
