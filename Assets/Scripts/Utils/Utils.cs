using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {
    public static string questManagerTag = "QuestManager";

    public const string player1Tag = "Player1";

    public const string player1AB1 = "ActionButton2A";
    public const string player1AB2 = "ActionButton2B";


    public const string player2Tag = "Player2";

    public const string player2AB1 = "ActionButton1A";
    public const string player2AB2 = "ActionButton1B";

    public const string flamableTreeTag = "FlamableTree";
    public const string animalToSaveTag = "AnimalToSave";

    public const string berriesTag = "Berries";

    public const int treeQuestIndex = 0;
    public const int twoPlayerQuestIndex = 1;
    public const int saveAnimalQuestIndex = 2;
    public const int rockDestroyQuestIndex = 3;
    public const int bearFeedQuestIndex = 4;

    [System.Serializable]
    public enum InventoryObjectType
    {
        Water, Food, Wood, Nails
    }

    public const int waterInventoryIndex = 0;
    public const int foodInventoryIndex = 1;
    public const int woodInventoryIndex = 2;
    public const int nailsInventoryIndex = 3;
    public const string player1InventoryTag = "Player1Inventory";
    public const string player2InventoryTag = "Player2Inventory";
}
