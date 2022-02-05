using UnityEngine;

namespace BP.Core
{
    public class GameEventTestListenerScript : MonoBehaviour
    {
        #region reactions
        public void BoolResponse(bool response) { Debug.Log("response:" + response); }
        public void VoidResposne() { Debug.Log("response to void event"); }
        #endregion
    }
}

