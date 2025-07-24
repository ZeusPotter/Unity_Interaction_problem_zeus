using System.Collections.Generic;
using UnityEngine;
using MyGame.Dialogues;  // para Interaction

namespace DialogueSystem
{
    [System.Serializable]
    public class DialogueScene
    {
        public List<MyGame.Dialogues.Interaction> interactions;  // aquí Interaction = MyGame.Dialogues.Interaction
    }
}