using System;
using Unity.Entities;
using UnityEngine;

namespace ZG
{
    [EntityComponent(typeof(MeshInstanceSkeletonData))]
    public class MeshInstanceSkeletonComponent : EntityProxyComponent, IEntityComponent
    {
        [SerializeField]
        internal MeshInstanceSkeletonDatabase _database;

        public bool isActive
        {
            get
            {
                if (enabled)
                {
                    var transform = base.transform;
                    while (transform.gameObject.activeSelf)
                    {
                        transform = transform.parent;
                        if (transform == null)
                            return true;
                    }
                }

                return false;
            }
        }

        public MeshInstanceSkeletonDatabase database => _database;

        /*[EntityComponents]
        public Type[] entityComponentTypesEx
        {
            get
            {
                if (!isActive)
                    return new Type[] { typeof(MeshInstanceSkeletonDisabled) };

                return null;
            }
        }

        protected void OnEnable()
        {
            if (gameObjectEntity.isCreated)
                this.RemoveComponent<MeshInstanceSkeletonDisabled>();
        }

        protected void OnDisable()
        {
            if (gameObjectEntity.isCreated && gameObjectEntity.world.IsCreated)
                this.AddComponent<MeshInstanceSkeletonDisabled>();
        }*/

        void IEntityComponent.Init(in Entity entity, EntityComponentAssigner assigner)
        {
            MeshInstanceSkeletonData instance;
            instance.definition = _database.definition;

#if DEBUG
            var database = GetComponent<MeshInstanceRendererComponent>().database;
            ref var rendererDefinition = ref database.definition.Value;
            ref var defintion = ref instance.definition.Value;
            int numInstances = defintion.instances.Length, numRenderers, boneCount, bindposeCount, i, j;
            for (i = 0; i < numInstances; ++i)
            {
                ref var temp = ref defintion.instances[i];
                ref var skeleton = ref defintion.skeletons[temp.skeletionIndex];
                boneCount = skeleton.bones.Length + skeleton.indirectBones.Length;

                numRenderers = temp.rendererIndices.Length;
                for (j = 0; j < numRenderers; ++j)
                {
                    bindposeCount = database.meshes[rendererDefinition.renderers[rendererDefinition.nodes[j].rendererIndex].meshIndex].bindposeCount;

                    if(bindposeCount != boneCount)
                        Debug.LogError("Bone Index Out Range Of Skeleton!", this);
                }
            }
#endif

            assigner.SetComponentData(entity, instance);
        }
    }
}