using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public static BattleController Instance { get; set; }

    public Dictionary<int, List<Character>> characters = new Dictionary<int, List<Character>>();
    [SerializeField]
    private int _characterTurnIndex;
    public int CharacterTurnIndex
    {
        get
        {
            return _characterTurnIndex;
        }
    }
    [SerializeField]
    private int _actTurn;
    [SerializeField]
    private BattleSpawnPoint[] _spawnPoints;

    public Spell playerSelectedSpell;
    public bool playerIsAttacking;

    [SerializeField]
    private BattleUIController _uiController;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }

        characters.Add(0, new List<Character>());
        characters.Add(1, new List<Character>());

        FindObjectOfType<BattleLauncher>().Launch();
    }

    public Character GetRandomPlayer()
    {
        return characters[0][Random.Range(0, characters[0].Count - 1)];
    }

    public Character GetWeakestEnemy()
    {
        Character weakestEnemy = characters[1][0];
        foreach (Character character in characters[1])
        {
            if (character.Health < weakestEnemy.Health)
            {
                weakestEnemy = character;
            }
        }

        return weakestEnemy;
    }

    private void NextTurn()
    {
        _actTurn = _actTurn == 0 ? 1 : 0;
    }

    private void NextAct()
    {
        if (characters[0].Count > 0 && characters[1].Count > 0)
        {
            if (_characterTurnIndex < characters[_actTurn].Count - 1)
            {
                _characterTurnIndex++;
            }
            else
            {
                NextTurn();
                _characterTurnIndex = 0;
                Debug.Log("Turn " + _actTurn);
            }

            switch (_actTurn)
            {
                case 0:
                    _uiController.ToggleActionState(true);
                    _uiController.BuildSpellList(GetCurrentCharacter().Spells);
                    break;
                case 1:
                    StartCoroutine(PerformAct());
                    _uiController.ToggleActionState(false);
                    break;
            }
        } else
        {
            Debug.Log("Battle is over!");
        }
    }

    IEnumerator PerformAct()
    {
        yield return new WaitForSeconds(.75f);
        if (GetCurrentCharacter().Health > 0)
        {
            GetCurrentCharacter().GetComponent<Enemy>().Act();
        }
        _uiController.UpdateCharacterUI();
        yield return new WaitForSeconds(1f);
        NextAct();
    }

    public void SelectCharacter(Character character)
    {
        if (playerIsAttacking)
        {
            DoAttack(GetCurrentCharacter(), character);
        } else if (playerSelectedSpell != null)
        {
            if (GetCurrentCharacter().CastSpell(playerSelectedSpell, character))
            {
                _uiController.UpdateCharacterUI();
                NextAct();
            }
            else
            {
                Debug.LogWarning("Not enough mana to cast the spell!");
            }
        }
    }

    public void Defend()
    {
        GetCurrentCharacter().Defend();
        if (_actTurn == 0)
        {
            NextAct();
        }
    }

    public void DoAttack(Character attacker, Character target)
    {
        target.Hurt(attacker.AttackPower);
        if (_actTurn == 0)
        {
            NextAct();
        }
    }

    public void StartBattle(List<Character> players, List<Character> enemies)
    {
        Debug.Log("Start battle!");

        for (int i = 0; i < players.Count; i++)
        {
            characters[0].Add(_spawnPoints[i + 3].Spawn(players[i]));
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            characters[1].Add(_spawnPoints[i].Spawn(enemies[i]));
        }

        _uiController.UpdateCharacterUI();
    }

    public Character GetCurrentCharacter()
    {
        return characters[_actTurn][_characterTurnIndex];
    }
}
