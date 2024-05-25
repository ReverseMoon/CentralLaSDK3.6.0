
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Video.Components.AVPro;
using VRC.SDKBase;
using VRC.Udon;

namespace Kittenji
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None), RequireComponent(typeof(BoxCollider))]
    public class CentralAreaTransport : UdonSharpBehaviour
    {
        [Tooltip("El origen que desea transportar.")]
        public Transform Origin;
        [Tooltip("El transform al que el origen será transportado.")]
        public Transform Target;
        [Tooltip("Desactiva el origen cuando el jugador salga del trigger.")]
        public bool ToggleActive;
        [Tooltip("Activa la zona asignada a esta variable cuando salgas de la zona actual.")]
        public CentralAreaTransport DefaultArea;

        private void Start()
        {
            GetComponent<BoxCollider>().isTrigger = true;

            if (!Utilities.IsValid(Origin))
            {
                this.enabled = false;
                return;
            }

            if (ToggleActive && !Origin.gameObject.activeSelf)
                Origin.gameObject.SetActive(true);
        }

        public override void OnPlayerTriggerEnter(VRCPlayerApi player)
        {
            if (Utilities.IsValid(player) && player.isLocal)
            {
                OnEnterEvent();
            }
        }

        public override void OnPlayerTriggerExit(VRCPlayerApi player)
        {
            if (Utilities.IsValid(player) && player.isLocal)
            {
                OnExitEvent();
            }
        }

        public void OnEnterEvent()
        {
            if (ToggleActive && !Origin.gameObject.activeSelf)
                Origin.gameObject.SetActive(true);

            if (Utilities.IsValid(Target))
            {
                Origin.position = Target.position;
                Origin.rotation = Target.rotation;
                Origin.localScale = Target.lossyScale;
            }
        }
        public void OnExitEvent()
        {
            if (ToggleActive && Origin.gameObject.activeSelf)
                Origin.gameObject.SetActive(false);

            if (Utilities.IsValid(DefaultArea))
                DefaultArea.OnEnterEvent();
        }

#if !COMPILER_UDONSHARP && UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (Origin == null || Target == null) return;

            Renderer[] renderers = Origin.GetComponentsInChildren<Renderer>();
            Mesh[] meshes = new Mesh[renderers.Length];
            for (int i = 0; i < renderers.Length; i++)
            {
                Renderer renderer = renderers[i];
                if (!renderer.enabled) continue;
                if (renderer is SkinnedMeshRenderer r)
                {
                    meshes[i] = r.sharedMesh;
                } else {
                    MeshFilter mf = renderer.transform.GetComponent<MeshFilter>();
                    if (mf != null) meshes[i] = mf.sharedMesh;
                }
            }

            Gizmos.color = Color.grey;
            Matrix4x4 matrix = Gizmos.matrix;
            Transform _target = Target == null ? transform : Target;
            Matrix4x4 localToWorld = _target.localToWorldMatrix;
            for (int i = 0; i < meshes.Length; i++)
            {
                Mesh mesh = meshes[i];
                if (mesh == null) continue;
                Transform tr = renderers[i].transform;


                Quaternion localRotation = tr.rotation * Quaternion.Inverse(Origin.rotation);
                Vector3 scale = Origin.localScale;
                Origin.localScale = Vector3.one;
                Gizmos.matrix = localToWorld
                    * Matrix4x4.TRS(Origin.InverseTransformPoint(tr.position), localRotation, tr.lossyScale);

                Gizmos.DrawWireMesh(mesh);
                Origin.localScale = scale;
            }
            // Gizmos.color = Color.white;
            Gizmos.matrix = matrix;
            Gizmos.color = Color.cyan / 4f;

            AudioSource[] audioSources = Origin.GetComponentsInChildren<AudioSource>();
            for (int i = 0; i < audioSources.Length; i++)
            {
                AudioSource source = audioSources[i];
                Transform tr = source.transform;

                Vector3 point = tr.root.InverseTransformPoint(tr.position);
                point = _target.TransformPoint(point);

                Gizmos.DrawWireSphere(point, 0.1f);
                Gizmos.DrawWireSphere(point, source.minDistance);
                Gizmos.DrawWireSphere(point, source.maxDistance);
            }

            Gizmos.matrix = matrix;
            Gizmos.color = Color.white;
        }
#endif
    }
}