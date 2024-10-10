using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntScript : MonoBehaviour
{
    public Transform John;
    public GameObject BulletPrefab;
    public float Speed = 2.0f;
    public float VisionRange = 5.0f;
    public int DamageToJohn = 1;

    private int Health = 999999999;
    private float LastShoot;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (John == null) return;

        float distance = Vector3.Distance(transform.position, John.position);

        if (distance <= VisionRange)
        {
            Vector3 direction = (John.position - transform.position).normalized;

            if (direction.x >= 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            else transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);

            transform.position += direction * Speed * Time.deltaTime;

            if (distance < 1.0f && Time.time > LastShoot + 0.25f)
            {
                Shoot();
                LastShoot = Time.time;
            }
        }
    }

    private void Shoot()
    {
        Vector3 direction = new Vector3(transform.localScale.x, 0.0f, 0.0f);
        GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.1f, Quaternion.identity);
        bullet.GetComponent<BulletScript>().SetDirection(direction);
    }

    public void Hit(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Debug.Log("Grunt derrotado");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BulletScript bullet = collision.GetComponent<BulletScript>();
        if (bullet != null)
        {
            Hit(10);
            bullet.DestroyBullet();
        }

        JohnMovement john = collision.GetComponent<JohnMovement>();
        if (john != null)
        {
            john.TakeDamage(DamageToJohn);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SimpleJumpAndMove john = collision.collider.GetComponent<SimpleJumpAndMove>();

        if (john != null)
        {
            john.TakeDamage(DamageToJohn);
        }
    }

    public void ResetPosition()
    {
        transform.position = startPosition;
    }
}
