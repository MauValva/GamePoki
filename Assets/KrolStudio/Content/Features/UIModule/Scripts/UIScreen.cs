using Cysharp.Threading.Tasks;
using UnityEngine;

namespace KrolStudio
{
    public abstract class UIScreen : MonoBehaviour
    {
        public abstract UniTask Show();
        public abstract UniTask Hide();
    }
}