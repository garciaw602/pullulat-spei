using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    // Las referencias a los AudioSources deben estar aquí porque este script los controlará
    [Header("Audio Sources")]
    [Tooltip("Fuente de audio principal para efectos de un solo disparo (salto, recoger, muerte).")]
    public AudioSource oneShotAudioSource; // Arrastra el primer AudioSource aquí
    [Tooltip("Fuente de audio para sonidos de bucle (correr/pasos).")]
    public AudioSource loopingAudioSource; // Arrastra el segundo AudioSource aquí

    [Header("Audio Clips")]
    public AudioClip jumpSound;     // Sonido de salto
    public AudioClip landSound;     // Sonido de aterrizaje
    public AudioClip runSound;      // Sonido de correr/pasos
    public AudioClip collectSound;  // Sonido de recoger objeto
    public AudioClip deathSound;    // Sonido de muerte 
    public AudioClip fallSound;     //Sonido de caída al vacío


    private bool isRunningSoundPlaying = false;

    // --- Métodos Públicos para Reproducir Sonidos ---

    public void PlayJumpSound()
    {
        if (oneShotAudioSource != null && jumpSound != null)
        {
            oneShotAudioSource.PlayOneShot(jumpSound);
            Debug.Log("Playing Jump Sound"); // Debugging
        }
        else { Debug.LogWarning("Jump sound or source missing."); }
    }

    public void PlayLandSound()
    {
        if (oneShotAudioSource != null && landSound != null)
        {
            oneShotAudioSource.PlayOneShot(landSound);
            Debug.Log("Playing Land Sound"); // Debugging
        }
        else { Debug.LogWarning("Land sound or source missing."); }
    }

    public void StartRunSound()
    {
        if (loopingAudioSource != null && runSound != null && !isRunningSoundPlaying)
        {
            loopingAudioSource.clip = runSound;
            loopingAudioSource.loop = true;
            loopingAudioSource.Play();
            isRunningSoundPlaying = true;
            Debug.Log("Starting Run Sound"); // Debugging
        }
        else { Debug.LogWarning("Run sound, source or already playing."); }
    }

    public void StopRunSound()
    {
        if (loopingAudioSource != null && isRunningSoundPlaying)
        {
            loopingAudioSource.Stop();
            isRunningSoundPlaying = false;
            Debug.Log("Stopping Run Sound"); // Debugging
        }
        else { Debug.LogWarning("Run sound not playing or source missing."); }
    }

    public void PlayCollectSound()
    {
        if (oneShotAudioSource != null && collectSound != null)
        {
            oneShotAudioSource.PlayOneShot(collectSound);
            Debug.Log("Playing Collect Sound"); // Debugging
        }
        else { Debug.LogWarning("Collect sound or source missing."); }
    }

    // --- ¡NUEVO MÉTODO PARA EL SONIDO DE MUERTE! ---
    public void PlayDeathSound()
    {
        if (oneShotAudioSource != null && deathSound != null)
        {
            oneShotAudioSource.PlayOneShot(deathSound);
            Debug.Log("Playing Death Sound"); // Debugging
        }
        else
        {
            Debug.LogWarning("Death sound or its AudioSource is missing in PlayerAudioManager."); // Debugging
        }
    }

    public void PlayFallSound()
    {
        if (oneShotAudioSource != null && fallSound != null)
        {
            oneShotAudioSource.PlayOneShot(fallSound);
            Debug.Log("Playing Fall Sound"); // Debugging
        }
        else
        {
            Debug.LogWarning("Fall sound or its AudioSource is missing in PlayerAudioManager.");
        }
    }

}