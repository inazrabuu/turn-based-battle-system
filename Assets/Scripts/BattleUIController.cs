using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject _spellPanel;
    [SerializeField]
    private Button[] _actionButtons;
    [SerializeField]
    private Button _button;
    [SerializeField]
    private TextMeshProUGUI[] _characterInfos;

    private void Start()
    {
        _spellPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, ray.direction);
            if (hitInfo.collider != null && hitInfo.collider.CompareTag("Character"))
            {
                Character character = hitInfo.collider.GetComponent<Character>();
                Debug.Log(character);
                BattleController.Instance.SelectCharacter(character);
            }
        }
    }

    public void ToggleActionState(bool state)
    {
        ToggleSpellPanel(state);
        foreach (Button button in _actionButtons)
        {
            button.interactable = state;
        }
    }

    public void ToggleSpellPanel(bool state)
    {
        _spellPanel.SetActive(state);
        if (state == true)
        {
            BuildSpellList(BattleController.Instance.GetCurrentCharacter().Spells);
        }
    }

    public void BuildSpellList(List<Spell> spells)
    {
        if (_spellPanel.transform.childCount > 0)
        {
            foreach (Button button in _spellPanel.transform.GetComponentsInChildren<Button>())
            {
                Destroy(button.gameObject);
            }
        }

        foreach (Spell spell in spells)
        {
            Button spellButton = Instantiate<Button>(_button, _spellPanel.transform);
            spellButton.GetComponentInChildren<TextMeshProUGUI>().text = spell.SpellName;
            spellButton.onClick.AddListener(() => SelectSpell(spell));
        }
    }

    private void SelectSpell(Spell spell)
    {
        BattleController.Instance.playerSelectedSpell = spell;
        BattleController.Instance.playerIsAttacking = false;
    }

    public void SelectAttack()
    {
        BattleController.Instance.playerSelectedSpell = null;
        BattleController.Instance.playerIsAttacking = true;
    }

    public void Defend()
    {
        BattleController.Instance.Defend();
    }

    public void UpdateCharacterUI()
    {
        for (int i = 0; i < BattleController.Instance.characters[0].Count; i++)
        {
            Character character = BattleController.Instance.characters[0][i];
            _characterInfos[i].text = string.Format("{0} - HP: {1}/{2} MP: {3}", character.Name, character.Health, character.MaxHealth, character.ManaPoints);
        }
    }
}
