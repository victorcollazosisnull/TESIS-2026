using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PickupObject : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void OnPickUp()
    {
        rb.isKinematic = true;
        rb.useGravity = false;

        col.enabled = false; 
    }

    public void OnDrop()
    {
        rb.isKinematic = false;
        rb.useGravity = true;

        col.enabled = true; 
    }
}