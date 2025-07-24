using System.Collections.Generic;
using UnityEngine;  

namespace MyGame.Dialogues
{
    [System.Serializable]
    public class Interaction
    {
        public string id;
        public List<string> lines;
        public string npcName;
    }
}
