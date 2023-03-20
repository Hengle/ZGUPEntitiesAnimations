using UnityEngine;

namespace ZG
{
    public struct MeshInstanceAnimatorAudioEventHandler : IMeshInstanceHybridAnimatorEventHandler
    {
        public void Dispatch(Transform root, float weight, int state)
        {
            var audioComponent = root.GetComponentInChildren<AudioComponent>();
            if (audioComponent == null)
                return;

            audioComponent.Play(state);
        }
    }

#if UNITY_EDITOR
    [CreateAssetMenu(menuName = "ZG/Mesh Instance/Audio/Animator Event Config", fileName = "Mesh Instance Animator Audio Event Config")]
    public class MeshInstanceAnimatorAudioEventConfig : MeshInstanceHybridAnimatorEventConfig<MeshInstanceAnimatorAudioEventHandler>
    {
        public override int GetState(AnimationEvent animationEvent) => animationEvent.intParameter;
    }
#endif
}