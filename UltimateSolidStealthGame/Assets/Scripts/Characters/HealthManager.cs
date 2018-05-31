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
    [SerializeField]
    float healthBarYOffset;

	/*
		reference to CharacterManager component of gameObject, either Enemy or Player
	*/
	CharacterManager manager;
    HealthBar healthBar;
    float maxHealth;
    bool playerHealth;
    

	public float Health {
		get { return health; }
	}
    public float HealthRatio {
        get { return (health / maxHealth); }
    }
		
	void Awake () {
		manager = (transform.parent != null) ? transform.parent.GetComponentInChildren<CharacterManager> () : GetComponent<CharacterManager>();
        maxHealth = health;
        if (healthBarObject) {
            Vector3 loc = transform.position;
            loc[1] += healthBarYOffset;
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
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet != null) {
            if (enabled) {
                Attack(bullet.Damage);
            } else {
                manager.OnTakeDamage();
            }
        }
	}

	/*
		causes damage to character
		@param damage - amount of damage to take
	*/
	public void Attack(float damage) {
        if (enabled) {
            if (playerHealth && PlayerPrefs.GetInt("Vibration", 1) == 1) {
                Handheld.Vibrate();
            }
            health = (health > 0.0f) ? health - damage : 0.0f;
            if (healthBar) {
                healthBar.MinusHealth(health);
            }
            manager.OnTakeDamage();
            if (health <= 0.0f) {
                manager.Kill();
                enabled = false;
            }
        }
	}

    private void OnDisable() {
        if (healthBar && healthBar.gameObject) {
            healthBar.gameObject.SetActive(false);
        }
    }

    private void OnEnable() {
        if (healthBar && healthBar.gameObject) {
            healthBar.gameObject.SetActive(true);
        }
    }
}
