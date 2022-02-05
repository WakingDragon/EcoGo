using UnityEngine;

namespace BP.Core.Audio
{
    ///create a dedicated audio src for each track
    ///hold src relationship in an item that can be used to store state
    ///each item can hold a stack of cues ready to play? - there should be a seperate management somehow of enqueuing new tracks here
    

    public class SoundtrackModule : MonoBehaviour
    {
        private void Awake()
        {
            Assemble();
        }

        private void Assemble()
        {
            
        }
    }
}

