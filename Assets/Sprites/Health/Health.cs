using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")] 
    [SerializeField] private int maxHP;
    public int currentHP;
    
    private Animator _animator;

    private void Awake()
    {
        currentHP = maxHP;
        _animator = GetComponent<Animator>();
    }

    public void TakeDamage(int amount)
    {
        if (currentHP - amount <= 0)
        {
            currentHP = 0;
            Die();
        }
        else
        {
            _animator.Play("hurt");
            currentHP -= amount;
        }
    }

    private void Die()
    {
        _animator.Play("die");
    }

    public void AddHealth(int amount)
    {
        currentHP = Mathf.Clamp(currentHP + amount, 0, maxHP);
    }
}