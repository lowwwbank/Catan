using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Catan.Players;
using Catan.ResourcePhase;
using System;

namespace Catan.UI
{
    public class PlayerSettings : MonoBehaviour
    {
        public TextMeshProUGUI colorName;
        public GameObject playerNameInput;
        public GameObject playerName;
        public Image colorPanel;
        public static readonly string[] colorsNames = {
            "Blue",
            "Red",
            "Grey",
            "Orange",
            "Green",
            "Cyan"
        };

        public static readonly Color[] possibleColors = {
            new Color(100 / 255f, 100 / 255f, 255 / 255f),
            new Color(255 / 255f, 100 / 255f, 100 / 255f),
            new Color(240 / 255f, 240 / 255f, 240 / 255f),
            new Color(255 / 255f, 150 / 255f, 100 / 255f),
            new Color(205 / 255f, 255 / 255f, 12 / 255f),
            new Color(10 / 255f, 200 / 255f, 200 / 255f)
        };

        public static readonly Color[] uiColors = {
            new Color(100 / 255f, 100 / 255f, 255 / 255f),
            new Color(255 / 255f, 100 / 255f, 100 / 255f),
            new Color(240 / 255f, 240 / 255f, 240 / 255f),
            new Color(255 / 255f, 150 / 255f, 100 / 255f),
            new Color(205 / 255f, 255 / 255f, 12 / 255f),
            new Color(10 / 255f, 200 / 255f, 200 / 255f)
        };

        public static readonly Color[] secondaryColors = {
            new Color(10 / 255f, 10 / 255f, 10 / 255f),
            new Color(10 / 255f, 10 / 255f, 10 / 255f),
            new Color(10 / 255f, 10 / 255f, 10 / 255f),
            new Color(10 / 255f, 10 / 255f, 10 / 255f),
            new Color(10 / 255f, 10 / 255f, 10 / 255f),
            new Color(10 / 255f, 10 / 255f, 10 / 255f)
        };
        public int index;
        public int color;
        public string pName = "Player X";

        public void OpenNameBox()
        {
            pName = playerName.GetComponent<TextMeshProUGUI>().text;
            playerNameInput.GetComponent<TMP_InputField>().text = pName;

            playerNameInput.SetActive(true);
            playerName.SetActive(false);
        }

        public void CloseNameBox()
        {
            pName = playerNameInput.GetComponent<TMP_InputField>().text;
            playerName.GetComponent<TextMeshProUGUI>().text = pName;

            playerNameInput.SetActive(false);
            playerName.SetActive(true);
        }

        public void ToggleNameBox()
        {
            if (playerNameInput.activeInHierarchy)
            {
                CloseNameBox();
            }
            else
            {
                OpenNameBox();
            }
        }


        public void UpdateDisplayColors()
        {
            colorPanel.color = uiColors[color];
            colorName.text = colorsNames[color];
            colorPanel.color = uiColors[color];
        }

        public void ColorLeft()
        {
            color = color - 1;
            if (color < 0) { color = possibleColors.Length - 1; }
            UpdateDisplayColors();
        }

        public void ColorRight()
        {
            color = (color + 1) % possibleColors.Length;
            UpdateDisplayColors();
        }
        public Player GetPlayer()
        {
            Player player = new Player()
            {
                playerColor = possibleColors[color],
                primaryUIColor = uiColors[color],
                secondaryUIColor = secondaryColors[color],
                playerName = pName,
                playerIndex = index,
                resources = new Resource[5]
            };

            player.resources[0] = new Resource(Resource.ResourceType.Grain, 0);
            player.resources[1] = new Resource(Resource.ResourceType.Wool, 0);
            player.resources[2] = new Resource(Resource.ResourceType.Wood, 0);
            player.resources[3] = new Resource(Resource.ResourceType.Brick, 0);
            player.resources[4] = new Resource(Resource.ResourceType.Ore, 0);

            return player;

            void Start()
            {
                colorPanel.color = uiColors[color];
                playerName.GetComponent<TextMeshProUGUI>().text = "Nickname";
                UpdateDisplayColors();
            }
        }
    }
}