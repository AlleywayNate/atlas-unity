using UnityEngine;

public class FootstepController : MonoBehaviour
{
    public AudioClip grassFootsteps;
    public AudioClip rockFootsteps;
    private AudioSource audioSource;
    private TerrainType currentTerrainType;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        HandleFootsteps();
    }

    private void HandleFootsteps()
    {
        if (IsRunning() && IsOnGround())
        {
            DetectTerrainType();

            if (!audioSource.isPlaying)
            {
                PlayFootstepSound();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    private bool IsRunning()
    {
        return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    }

    private bool IsOnGround()
    {
        RaycastHit hit;
        // Raycast downwards to check if player is on the ground
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f))
        {
            return true;
        }
        return false;
    }

    private void DetectTerrainType()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f))
        {
            // Check the material of the surface
            if (hit.collider.material != null)
            {
                if (hit.collider.material.name.Contains("Grass"))
                {
                    currentTerrainType = TerrainType.Grass;
                }
                else if (hit.collider.material.name.Contains("Rock"))
                {
                    currentTerrainType = TerrainType.Rock;
                }
            }
        }
    }

    private void PlayFootstepSound()
    {
        switch (currentTerrainType)
        {
            case TerrainType.Grass:
                audioSource.clip = grassFootsteps;
                break;
            case TerrainType.Rock:
                audioSource.clip = rockFootsteps;
                break;
        }
        audioSource.Play();
    }

    private enum TerrainType
    {
        Grass,
        Rock
    }
}