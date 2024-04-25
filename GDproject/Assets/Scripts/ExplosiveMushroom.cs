using UnityEngine;

public class ExplosiveMushroom : Enemy
{
    [SerializeField] private int maxHealth;
    private int _health;
    [SerializeField] private ExplosiveScript explosiveScript;
    
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
            explosiveScript.Explode();
        }
    }
    
    override 
    public void Ignite()
    {
    }
}
