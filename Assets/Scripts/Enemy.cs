using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public void Act()
    {
        int diceRoll = Random.Range(0, 2);
        Character target = BattleController.Instance.GetRandomPlayer();
        switch (diceRoll)
        {
            case 0:
                Defend();
                break;
            case 1:
                Spell spellToCast = GetRandomSpell();
                if (spellToCast.spellType == Spell.SpellType.Heal)
                {
                    target = BattleController.Instance.GetWeakestEnemy();
                }
                if (!CastSpell(spellToCast, target))
                {
                    BattleController.Instance.DoAttack(this, target);
                }
                break;
            case 2:
                BattleController.Instance.DoAttack(this, target);
                break;
        }
    }

    Spell GetRandomSpell()
    {
        return _spells[Random.Range(0, _spells.Count - 1)];
    }

    public override void Die()
    {
        BattleController.Instance.characters[1].Remove(this);
        base.Die();
    }
}
