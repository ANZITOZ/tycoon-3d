using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuUI : MonoBehaviour
{
    private Canvas canvas;
    private GameObject panel;
    private Text titleText;
    private readonly List<Button> machineButtons = new List<Button>();
    private readonly List<Text> machineTexts = new List<Text>();

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();

        if (canvas == null)
            canvas = FindFirstObjectByType<Canvas>();

        CreateMenu();
        Refresh();
    }

    private void Update()
    {
        if (machineButtons.Count == 0 && panel != null)
            CreateMachineButtons();

        Refresh();
    }

    private void CreateMenu()
    {
        Button openButton = CreateButton(
            canvas.transform,
            "CONSTRUIR",
            new Vector2(125f, 135f),
            new Vector2(210f, 70f));

        openButton.onClick.AddListener(TogglePanel);

        panel = new GameObject("Build Panel", typeof(RectTransform), typeof(Image));
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.SetParent(canvas.transform, false);
        panelRect.anchorMin = new Vector2(0f, 0.5f);
        panelRect.anchorMax = new Vector2(0f, 0.5f);
        panelRect.pivot = new Vector2(0f, 0.5f);
        panelRect.anchoredPosition = new Vector2(25f, 30f);
        panelRect.sizeDelta = new Vector2(390f, 470f);

        Image panelImage = panel.GetComponent<Image>();
        panelImage.color = new Color(0.06f, 0.07f, 0.09f, 0.96f);

        titleText = CreateText(
            panel.transform,
            "CONSTRUIR",
            new Vector2(195f, -35f),
            new Vector2(350f, 55f),
            30);

        CreateMachineButtons();
        panel.SetActive(false);
    }

    private void CreateMachineButtons()
    {
        MachineUnlockManager manager = GameManager.Instance != null ? GameManager.Instance.Machines : null;

        if (manager == null)
            return;

        IReadOnlyList<MachineDefinition> definitions = manager.MachineDefinitions;

        for (int i = 0; i < definitions.Count; i++)
        {
            int index = i;
            float y = -105f - (i * 82f);

            Button button = CreateButton(
                panel.transform,
                "",
                new Vector2(195f, y),
                new Vector2(350f, 68f));

            Text label = GetButtonText(button);

            machineButtons.Add(button);
            machineTexts.Add(label);

            button.onClick.AddListener(() => OnMachineClicked(index));
        }
    }

    private void OnMachineClicked(int index)
    {
        MachineUnlockManager manager = GameManager.Instance != null ? GameManager.Instance.Machines : null;

        if (manager == null || index < 0 || index >= manager.MachineDefinitions.Count)
            return;

        MachineDefinition definition = manager.MachineDefinitions[index];

        if (!manager.IsUnlocked(definition))
            manager.TryUnlock(definition);
    }

    private void Refresh()
    {
        MachineUnlockManager manager = GameManager.Instance != null ? GameManager.Instance.Machines : null;

        if (manager == null || machineButtons.Count == 0)
            return;

        for (int i = 0; i < machineButtons.Count && i < manager.MachineDefinitions.Count; i++)
        {
            MachineDefinition definition = manager.MachineDefinitions[i];
            bool unlocked = manager.IsUnlocked(definition);
            bool canUnlock = manager.CanUnlock(definition);

            machineButtons[i].interactable = !unlocked && canUnlock;

            if (unlocked)
            {
                machineTexts[i].text = definition.machineName + "\nDESBLOQUEADA • " + definition.outputName;
            }
            else if (GameManager.Instance.Progression.Level < definition.unlockLevel)
            {
                machineTexts[i].text = definition.machineName + "\nNÍVEL " + definition.unlockLevel + " • $" + definition.unlockCost;
            }
            else
            {
                machineTexts[i].text = definition.machineName + "\nDESBLOQUEAR • $" + definition.unlockCost;
            }
        }
    }

    private void TogglePanel()
    {
        if (panel != null)
            panel.SetActive(!panel.activeSelf);
    }

    private Button CreateButton(Transform parent, string label, Vector2 position, Vector2 size)
    {
        GameObject buttonObject = new GameObject(
            "Build Button",
            typeof(RectTransform),
            typeof(Image),
            typeof(Button));

        RectTransform rect = buttonObject.GetComponent<RectTransform>();
        rect.SetParent(parent, false);
        rect.anchorMin = new Vector2(0.5f, 1f);
        rect.anchorMax = new Vector2(0.5f, 1f);
        rect.pivot = new Vector2(0.5f, 1f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        Image image = buttonObject.GetComponent<Image>();
        image.color = new Color(0.12f, 0.5f, 0.3f, 0.96f);

        Text text = CreateText(rect, label, Vector2.zero, size, 20);
        text.resizeTextForBestFit = true;
        text.resizeTextMinSize = 12;
        text.resizeTextMaxSize = 20;

        return buttonObject.GetComponent<Button>();
    }

    private Text CreateText(Transform parent, string content, Vector2 position, Vector2 size, int fontSize)
    {
        GameObject textObject = new GameObject("Text", typeof(RectTransform), typeof(Text));
        RectTransform rect = textObject.GetComponent<RectTransform>();
        rect.SetParent(parent, false);
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        Text text = textObject.GetComponent<Text>();
        text.text = content;
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = fontSize;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;

        return text;
    }

    private Text GetButtonText(Button button)
    {
        Text text = button.GetComponentInChildren<Text>();

        if (text == null)
            text = CreateText(button.transform, "", Vector2.zero, button.GetComponent<RectTransform>().sizeDelta, 18);

        return text;
    }
}
