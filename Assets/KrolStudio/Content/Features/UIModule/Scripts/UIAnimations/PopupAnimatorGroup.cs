using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KrolStudio
{
    public class PopupAnimatorGroup : MonoBehaviour
    {
        [SerializeField] private UIElementAnimator[] _elements;

        public async UniTask Show()
        {
            await UniTask.WhenAll(_elements.Select(e => e.Show()));
        }

        public async UniTask Hide()
        {
            var tasks = new List<UniTask>();

            foreach (var e in _elements)
            {
                if (e != null && e.gameObject.activeInHierarchy)
                    tasks.Add(e.Hide());
            }

            await UniTask.WhenAll(tasks);
        }
    }
}