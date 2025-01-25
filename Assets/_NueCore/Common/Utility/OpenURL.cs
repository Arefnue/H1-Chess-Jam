using UnityEngine;

namespace _NueCore.Common.Utility
{
    public class OpenURL : MonoBehaviour
    {
        [SerializeField,TextArea(3,5)] private string defaultURL;

        public void OpenDefault()
        {
            Open(defaultURL);
        }
        public void Open(string url)
        {
            Application.OpenURL(url);
        }
    }
}