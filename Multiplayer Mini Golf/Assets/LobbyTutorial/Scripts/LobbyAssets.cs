using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyAssets : MonoBehaviour {



    public static LobbyAssets Instance { get; private set; }


    [SerializeField] private Sprite GrinSprite;
    [SerializeField] private Sprite CatSprite;
    [SerializeField] private Sprite PlagueSprite;
    [SerializeField] private Sprite GnomeSprite;


    private void Awake() {
        Instance = this;
    }

    public Sprite GetSprite(LobbyManager.PlayerCharacter playerCharacter) {
        switch (playerCharacter) {
            default:
            case LobbyManager.PlayerCharacter.Grin:     return GrinSprite;
            case LobbyManager.PlayerCharacter.Cat:      return CatSprite;
            case LobbyManager.PlayerCharacter.Plague:   return PlagueSprite;
            case LobbyManager.PlayerCharacter.Gnome:    return GnomeSprite;
        }
    }

}