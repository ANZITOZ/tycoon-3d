using System;
using UnityEngine;

public class PlayerProgression : MonoBehaviour
{
    [SerializeField] private int level = 1;
    [SerializeField] private int currentXp = 0;
    [SerializeField] private int baseXpToNextLevel = 100;
    [SerializeField] private float xpGrowth = 1.35f;

    public int Level => level;
    public int CurrentXp => currentXp;
    public int XpToNextLevel => Mathf.Max(1, Mathf.CeilToInt(baseXpToNextLevel * Mathf.Pow(xpGrowth, level - 1)));
    public float LevelProgress => XpToNextLevel <= 0 ? 0f : (float)currentXp / XpToNextLevel;

    public event Action<int> LevelChanged;
    public event Action<int, int> XPChanged;

    public void AddXP(int amount)
    {
        if (amount <= 0)
            return;

        currentXp += amount;

        while (currentXp >= XpToNextLevel)
        {
            currentXp -= XpToNextLevel;
            level++;
            LevelChanged?.Invoke(level);
        }

        XPChanged?.Invoke(currentXp, XpToNextLevel);
    }

    public void SetProgress(int newLevel, int newXp)
    {
        level = Mathf.Max(1, newLevel);
        currentXp = Mathf.Max(0, newXp);

        while (currentXp >= XpToNextLevel)
        {
            currentXp -= XpToNextLevel;
            level++;
        }

        XPChanged?.Invoke(currentXp, XpToNextLevel);
        LevelChanged?.Invoke(level);
    }
}
