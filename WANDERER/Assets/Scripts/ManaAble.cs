using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ManaAble : MonoBehaviour
{
    public int Mana { get; private set; }
    public int MaxMana { get; private set; }

    public UnityEvent<int, int> manaChanged;

    [SerializeField]
    private int manaRegenRate = 5; // Mana regenerated per second

    private void Awake()
    {
        MaxMana = 100; // Set your default max mana
        Mana = MaxMana;

        if (manaChanged == null)
            manaChanged = new UnityEvent<int, int>();
    }

    private void Start()
    {
        StartCoroutine(RegenerateMana());
    }

    public bool CanCastSkill(int manaCost)
    {
        return Mana >= manaCost;
    }

    public void UseMana(int amount)
    {
        if (CanCastSkill(amount))
        {
            Mana -= amount;
            Mana = Mathf.Clamp(Mana, 0, MaxMana);
            manaChanged.Invoke(Mana, MaxMana);
        }
        else
        {
            Debug.LogError("Not enough mana to cast skill");
        }
    }

    public void RecoverMana(int amount)
    {
        Mana += amount;
        Mana = Mathf.Clamp(Mana, 0, MaxMana);
        manaChanged.Invoke(Mana, MaxMana);
    }

    private IEnumerator RegenerateMana()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            RecoverMana(manaRegenRate);
        }
    }
    public void ChangeMana(int amount)
    {
        Mana += amount;
        Mana = Mathf.Clamp(Mana, 0, MaxMana);
        manaChanged.Invoke(Mana, MaxMana);

    }
}
