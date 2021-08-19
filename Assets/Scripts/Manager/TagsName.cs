using UnityEngine;
public class TagsName : MonoBehaviour
    {
        public string Player = "Player";
        public string DeathZone = "DeathZone";
        public string EndLine = "EndLine";

    public static TagsName instance;

    public void Awake()
    {
        instance = this;
    }
}


