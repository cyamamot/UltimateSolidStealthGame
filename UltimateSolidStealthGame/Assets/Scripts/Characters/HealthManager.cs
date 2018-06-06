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
    protected float health;
    [SerializeField]
    protected GameObject healthBarObject;
    [SerializeField]
    protected float healthBarYOffset;

    /*
		reference to CharacterManager component of gameObject, either Enemy or Player
	*/
    protected CharacterManager manager;
    protected HealthBar healthBar;
    protected float maxHealth;
    protected bool playerHealth;


    public float Health {
        get { return health; }
    }
    public float HealthRatio {
        get { return (health / maxHealth); }
    }

    protected virtual void Awake() {
        manager = (transform.parent != null) ? transform.parent.GetComponentInChildren<CharacterManager>() : GetComponent<CharacterManager>();
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
    protected void OnCollisionEnter(Collision collision) {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet != null) {
            if (enabled) {
                Attack(bullet.Damage);
            }
            else {
                manager.OnTakeDamage(0.0f);
            }
        }
    }

    /*
		causes damage to character
		@param damage - amount of damage to take
	*/
    public virtual void Attack(float damage, string damager = "Normal") {
        if (enabled) {
            if (playerHealth && PlayerPrefs.GetInt("Vibration", 1) == 1) {
                Handheld.Vibrate();
            }
            damage = (damage > health) ? health : damage;
            health = (health > 0.0f) ? health - damage : 0.0f;
            if (healthBar) {
                healthBar.MinusHealth(health);
            }
            manager.OnTakeDamage(damage);
            if (health <= 0.0f) {
                manager.Kill();
                enabled = false;
            }
        }
    }

    void OnDisable() {
        if (healthBar && healthBar.gameObject) {
            healthBar.gameObject.SetActive(false);
        }
    }

    void OnEnable() {
        if (healthBar && healthBar.gameObject) {
            healthBar.gameObject.SetActive(true);
        }
    }
}