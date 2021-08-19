using UnityEngine;

    public class LayersName : MonoBehaviour
    {
        public string Target = "Target";
        public string Obstacle = "Obstacle";
        public string InstantDeathObstacle = "InstantDeathObstacle";

    public static LayersName instance;

    public void Awake()
    {
        instance = this;
    }
}


