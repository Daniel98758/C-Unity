using UnityEngine;
using UnityEngine.SceneManagement;

public class Bandera : MonoBehaviour
{
    // Detectar si el jugador ha colisionado con la bandera
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si el objeto que colisiona es el jugador
        if (other.CompareTag("Player"))
        {
            // Cargar la siguiente escena
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

            // Verificar si la siguiente escena existe
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                Debug.Log("No hay más escenas para cargar.");
            }
        }
    }
}
