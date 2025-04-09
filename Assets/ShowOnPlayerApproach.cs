using UnityEngine;

public class ShowOnPlayerApproach : MonoBehaviour
{
    public Transform player; // assign Player transform via Inspector
    public float activationDistance = 15f;

    private Renderer[] renderers;
    private Collider[] colliders;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        colliders = GetComponentsInChildren<Collider>();

        SetVisible(false);
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= activationDistance)
        {
            SetVisible(true);
        }
        else
        {
            SetVisible(false);
        }
    }

    void SetVisible(bool visible)
    {
        foreach (Renderer rend in renderers)
            rend.enabled = visible;

        foreach (Collider col in colliders)
            col.enabled = visible;
    }
}
