using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalAudioManager : MonoBehaviour
{
    public static GlobalAudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource ambientSource; // Este lo usaremos para el viento

    [Header("Audio Clips")]
    public AudioClip mainMenuMusic;
    public AudioClip gameSoundtrack; // Tu banda sonora
    public AudioClip windSound;      // Tu sonido de viento

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Para que persista entre escenas
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // L�gica para cambiar audio seg�n la escena
        if (scene.name == "MainMenu") // Ejemplo para el men� principal
        {
            PlayMusic(mainMenuMusic);
            StopAmbient(); // Detener el viento en el men�
        }
        else if (scene.name == "Level1") // Reemplaza con el nombre de tu escena de juego
        {
            PlayMusic(gameSoundtrack); // Reproducir la banda sonora del juego
            PlayAmbient(windSound);    // Reproducir el sonido de viento
        }
        // Puedes a�adir m�s condiciones para diferentes escenas o ambientes
    }

    // M�todos auxiliares (ya incluidos en el ejemplo anterior)
    public void PlayMusic(AudioClip clip)
    {
        if (musicSource != null && clip != null && musicSource.clip != clip) // Evitar reiniciar si ya est� sonando
        {
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    public void PlayAmbient(AudioClip clip)
    {
        if (ambientSource != null && clip != null && ambientSource.clip != clip) // Evitar reiniciar si ya est� sonando
        {
            ambientSource.clip = clip;
            ambientSource.loop = true;
            ambientSource.Play();
        }
    }

    public void StopAmbient()
    {
        if (ambientSource != null)
        {
            ambientSource.Stop();
        }
    }
}