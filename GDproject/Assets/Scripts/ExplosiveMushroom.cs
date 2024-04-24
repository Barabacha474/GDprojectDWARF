using UnityEngine;

public class ExplosiveMushroom : Enemy
{
    [SerializeField] private int maxHealth;
    private int _health;
    private float _explosionRadius;
    private float _explosionForce;
    
    void Start()
    {
        _health = maxHealth;
    }
    

    public override void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health < 0)
        {
            _health = maxHealth;
            Explode();
        }
    }

    private void Explode()
    {
    }
}
