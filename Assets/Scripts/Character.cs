using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private string _name;
    public string Name
    {
        get { return _name; }
    }
    [SerializeField]
    private int _health;
    public int Health
    {
        get { return _health; }
        set { _health = value; }
    }
    [SerializeField]
    private int _maxHealth;
    public int MaxHealth
    {
        get { return _maxHealth; }
    }
    [SerializeField]
    private int _attackPower;
    public int AttackPower
    {
        get { return _attackPower; }
    }
    [SerializeField]
    private int _defensePower;
    [SerializeField]
    private int _manaPoints;
    public int ManaPoints
    {
        get { return _manaPoints; }
    }
    [SerializeField]
    protected List<Spell> _spells;
    public List<Spell> Spells
    {
        get
        {
            return _spells;
        }
    }

    public void Hurt(int amount)
    {
        //int damageAmount = Random.Range(0, 1) * (amount - _defensePower);
        int damageAmount = amount;
        _health = Mathf.Max(_health - damageAmount, 0);

        if (_health == 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        //int healAmount = Random.Range(0, 1) * (amount + (int)(_maxHealth * .33));
        int healAmount = amount;
        _health = Mathf.Min(_health + healAmount, _maxHealth);
    }

    public void Defend()
    {
        _defensePower += (int)(_defensePower * .33f);   
        Debug.Log("Defense increased");
    }

    public bool CastSpell(Spell spell, Character targetCharacter)
    {
        bool sucessfull = _manaPoints >= spell.ManaCost;

        if (sucessfull)
        {
            Spell spellToCast = Instantiate<Spell>(spell, transform.position, Quaternion.identity);
            _manaPoints -= spell.ManaCost;
            spellToCast.Cast(targetCharacter);
        }

        return sucessfull;
    }

    public virtual void Die()
    {
        Debug.LogFormat("{0} has died!", _name);
        Destroy(gameObject);
    }
}
