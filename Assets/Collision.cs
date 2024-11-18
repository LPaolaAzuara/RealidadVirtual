using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Collision : MonoBehaviour
{
    public GameObject losePanel; // Panel de derrota
    public Button restartButton; // Bot�n de reinicio

    public GameObject winPanel; // Panel de victoria
    public Transform finishTrigger; // Trigger del final

    public GameObject pausePanel; // Panel de pausa
    public Button pauseButton; // Bot�n de pausa
    public Button resumeButton; // Bot�n de reanudar

    public GameObject startPanel; // Panel inicial

    private bool isPaused = false; // Estado del juego (pausado o no)

    private void Start()
    {
        // Muestra el panel inicial al comienzo del juego y oculta los dem�s
        startPanel.SetActive(true);
        losePanel.SetActive(false);
        winPanel.SetActive(false);
        pausePanel.SetActive(false);

        // Pausa el tiempo al inicio para que el juego comience solo al quitar el panel inicial
        Time.timeScale = 0f;

        // Asigna la funci�n RestartGame al bot�n de reinicio
        restartButton.onClick.AddListener(RestartGame);

        // Asigna las funciones de pausa y reanudar
        pauseButton.onClick.AddListener(TogglePause);
        resumeButton.onClick.AddListener(ResumeFromStartOrPause);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que colisiona tiene el tag "Enfermero"
        if (other.CompareTag("Enfermero"))
        {
            // Activa el panel de derrota
            losePanel.SetActive(true);

            // Detiene el tiempo para que el juego se pause
            Time.timeScale = 0f;
        }

        // Verifica si el jugador alcanza el Trigger del final
        if (other.CompareTag("Player") && other.transform == finishTrigger)
        {
            // Activa el panel de victoria
            winPanel.SetActive(true);

            // Detiene el tiempo para pausar el juego
            Time.timeScale = 0f;
        }
    }

    public void RestartGame()
    {
        // Restablece el tiempo a la velocidad normal antes de reiniciar la escena
        Time.timeScale = 1f;

        // Recarga la escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator ReloadSceneWithDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // Usa tiempo real para evitar conflictos con Time.timeScale

        // Ahora recarga la escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void TogglePause()
    {
        isPaused = !isPaused; // Cambia el estado de pausa

        if (isPaused)
        {
            pausePanel.SetActive(true); // Muestra el panel de pausa
            Time.timeScale = 0f; // Pausa el tiempo
        }
        else
        {
            pausePanel.SetActive(false); // Oculta el panel de pausa
            Time.timeScale = 1f; // Reanuda el tiempo
        }
    }

    private void ResumeFromStartOrPause()
    {
        if (startPanel.activeSelf)
        {
            // Si el panel inicial est� activo, desact�valo para comenzar el juego
            startPanel.SetActive(false);
        }
        else
        {
            // Si est� en pausa, desactiva el panel de pausa
            pausePanel.SetActive(false);
        }

        // Reanuda el tiempo
        Time.timeScale = 1f;
        isPaused = false; // Aseg�rate de que el estado de pausa sea falso
    }
}
