using UnityEngine;

public class SimpleJumpAndMove : MonoBehaviour
{
    public GameObject BulletPrefab;
    public float speed = 5f;      // Velocidad de movimiento ajustable desde el Inspector
    public float jumpForce = 10f; // Fuerza de salto ajustable desde el Inspector
    public Transform cameraTransform; // Referencia a la cámara para que siga al personaje
    public int maxHealth = 3;      // Máxima salud del personaje
    public Vector3 startPosition;  // Posición inicial para reiniciar después del daño

    private Rigidbody2D rb;
    private bool canJump = true;  // Variable para controlar si el personaje puede saltar
    private Animator animator;
    private int currentHealth;

    // Nueva referencia a todos los enemigos Grunt
    public GruntScript[] grunts;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        startPosition = transform.position;

        // Buscar todos los Grunt en la escena
        grunts = FindObjectsOfType<GruntScript>();
    }

    void Update()
    {
        // Movimiento automático a la derecha
        rb.velocity = new Vector2(speed, rb.velocity.y);

        // Saltar cuando se presiona espacio y se puede saltar
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            Jump();
        }

        // Disparar cuando se presiona el botón derecho del ratón
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Shoot();
        }

        // Actualizar la animación de correr
        animator.SetBool("running", true);

        // Ajustar la dirección del sprite según el movimiento
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        // Hacer que la cámara siga al personaje
        cameraTransform.position = new Vector3(transform.position.x, cameraTransform.position.y, cameraTransform.position.z);
    }

    private void Jump()
    {
        // Aplicar fuerza de salto
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        canJump = false; // Deshabilitar el salto hasta que se toque el suelo
    }

    private void Shoot()
    {
        // Crear bala en la posición del personaje
        Instantiate(BulletPrefab, transform.position, Quaternion.identity);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Detectar si el personaje ha tocado el suelo
        if (collision.contacts[0].normal.y > 0.5f)
        {
            canJump = true; // Permitir saltar nuevamente
        }
    }

    // Método para recibir daño
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Respawn();  // Reiniciar si la salud llega a 0
        }

        // Reiniciar la posición de los enemigos Grunt
        ResetGruntPositions();
    }

    // Reiniciar la posición del personaje
    private void Respawn()
    {
        transform.position = startPosition;  // Volver a la posición inicial
        currentHealth = maxHealth;  // Restaurar la salud
    }

    // Método para reiniciar la posición de todos los Grunts
    private void ResetGruntPositions()
    {
        foreach (GruntScript grunt in grunts)
        {
            grunt.ResetPosition();
        }
    }
}
