using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField]
    private string _spellName;
    public string SpellName
    {
        get
        {
            return _spellName;
        }
        set
        {
            _spellName = value;
        }
    }
    [SerializeField]
    private int _spellPower;
    [SerializeField]
    private int _manaCost;
    public int ManaCost
    {
        get { return _manaCost; }
        set { _manaCost = value; }
    }
    public enum SpellType
    {
        Attack,
        Heal
    }
    [SerializeField]
    public SpellType spellType;

    private Vector3 _targetPosition;

    private void Update()
    {
        if (_targetPosition != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, 0.15f);
            if (Vector3.Distance(transform.position, _targetPosition) < 0.25f)
            {
                Destroy(this.gameObject, 1f);
            }
        } else
        {
            Destroy(this.gameObject);
        }
    }

    public void Cast(Character target)
    {
        _targetPosition = target.transform.position;
        Debug.Log("Cast " + _spellName + " is casted to " + target.name);

        if (spellType == SpellType.Attack)
        {
            target.Hurt(_spellPower);
        } else if (spellType == SpellType.Heal)
        {
            target.Heal(_spellPower);
        }
    }
 }
