using UnityEngine;
public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private int damage;

    private Rigidbody2D _rb;
    private PlayerMovement _playerMovement;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("WeakPoint"))
        {
            other.gameObject.GetComponentInParent<Health>().TakeDamage(damage);
            _playerMovement.JumpAction();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Cherry"))
        {
            CherryController.Instance.AddCherry(1);
            Destroy(other.gameObject);
        }
    }
}