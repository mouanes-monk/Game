using UnityEngine;

public class WindManager : MonoBehaviour
{
    public ParticleSystem sandstorm;
    public float windSpeed = 2f;

    void Update()
    {
        var velocity = sandstorm.velocityOverLifetime;
        velocity.x = windSpeed; // Change wind direction dynamically
    }

    public void IncreaseWind()
    {
        windSpeed += 1f; // Call this when wind should get stronger
    }

    public void DecreaseWind()
    {
        windSpeed = Mathf.Max(0, windSpeed - 1f);
    }
}
