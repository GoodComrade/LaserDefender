using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float speed = 15.0f;
    public float padding = 0;
    public GameObject projectile;
    public float projectileSpeed;
    public float firingRate = 0.2f;
    public float health = 250;

    public AudioClip fireSound;

    float xMin;
    float xMax;
    float yMin = -4.6f;
    float yMax = -1;

    void OnTriggerEnter2D(Collider2D collider)
    {
        Projectile missile = collider.gameObject.GetComponent<Projectile>();
        if (missile)
        {
            health -= missile.GetDamage();
            missile.Hit();
            if (health <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        LevelManager man = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        man.LoadLevel("Win Screen");
        Destroy(gameObject);
    }
    void Start()
    {
        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        xMin = leftmost.x + padding;
        xMax = rightmost.x - padding;
    }

    void Fire()
    {
        GameObject beam = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
        beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed, 0);
        AudioSource.PlayClipAtPoint(fireSound, transform.position);
    }
    void Update ()
    {
    if(Input.GetKeyDown(KeyCode.Space))
        {
            InvokeRepeating("Fire", 0.000001f, firingRate);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke("Fire");
        }

    if (Input.GetKey(KeyCode.LeftArrow))
        transform.position += Vector3.left * speed * Time.deltaTime;

    else if (Input.GetKey(KeyCode.RightArrow))
        transform.position += Vector3.right * speed * Time.deltaTime;

    if (Input.GetKey(KeyCode.UpArrow))
        transform.position += Vector3.up * speed * Time.deltaTime;

    else if (Input.GetKey(KeyCode.DownArrow))
        transform.position += Vector3.down * speed * Time.deltaTime;

    float newX = Mathf.Clamp(transform.position.x, xMin, xMax);
    transform.position = new Vector3(newX, transform.position.y, transform.position.z);

    float newY = Mathf.Clamp(transform.position.y, yMin, yMax);
    transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
