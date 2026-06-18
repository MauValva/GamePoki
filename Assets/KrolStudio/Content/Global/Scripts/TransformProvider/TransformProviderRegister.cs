using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class TransformProviderRegister : MonoBehaviour
    {
        private ITransformProvider _transformProvider;

        [Inject]
        public void Construct(ITransformProvider transformModel) =>
            _transformProvider = transformModel;

        private void Awake() =>
            _transformProvider.Transform = transform;
    }
}
