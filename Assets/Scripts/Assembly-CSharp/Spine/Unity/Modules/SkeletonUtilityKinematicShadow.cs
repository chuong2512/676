using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity.Modules
{
	public class SkeletonUtilityKinematicShadow : MonoBehaviour
	{
		private struct TransformPair
		{
			public Transform dest;

			public Transform src;
		}

		public enum PhysicsSystem
		{
			Physics2D,
			Physics3D
		}

		[Tooltip("If checked, the hinge chain can inherit your root transform's velocity or position/rotation changes.")]
		public bool detachedShadow;

		public Transform parent;

		public bool hideShadow = true;

		public PhysicsSystem physicsSystem = PhysicsSystem.Physics3D;

		private GameObject shadowRoot;

		private readonly List<TransformPair> shadowTable = new List<TransformPair>();

		private void Start()
		{
			shadowRoot = UnityEngine.Object.Instantiate(base.gameObject);
			UnityEngine.Object.Destroy(shadowRoot.GetComponent<SkeletonUtilityKinematicShadow>());
			Transform transform = shadowRoot.transform;
			transform.position = base.transform.position;
			transform.rotation = base.transform.rotation;
			Vector3 b = base.transform.TransformPoint(Vector3.right);
			float num = Vector3.Distance(base.transform.position, b);
			transform.localScale = Vector3.one;
			if (!detachedShadow)
			{
				if (parent == null)
				{
					transform.parent = base.transform.root;
				}
				else
				{
					transform.parent = parent;
				}
			}
			if (hideShadow)
			{
				shadowRoot.hideFlags = HideFlags.HideInHierarchy;
			}
			Joint[] componentsInChildren = shadowRoot.GetComponentsInChildren<Joint>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].connectedAnchor *= num;
			}
			SkeletonUtilityBone[] componentsInChildren2 = GetComponentsInChildren<SkeletonUtilityBone>();
			SkeletonUtilityBone[] componentsInChildren3 = shadowRoot.GetComponentsInChildren<SkeletonUtilityBone>();
			SkeletonUtilityBone[] array = componentsInChildren2;
			foreach (SkeletonUtilityBone skeletonUtilityBone in array)
			{
				if (skeletonUtilityBone.gameObject == base.gameObject)
				{
					continue;
				}
				Type type = ((physicsSystem == PhysicsSystem.Physics2D) ? typeof(Rigidbody2D) : typeof(Rigidbody));
				SkeletonUtilityBone[] array2 = componentsInChildren3;
				foreach (SkeletonUtilityBone skeletonUtilityBone2 in array2)
				{
					if (skeletonUtilityBone2.GetComponent(type) != null && skeletonUtilityBone2.boneName == skeletonUtilityBone.boneName)
					{
						shadowTable.Add(new TransformPair
						{
							dest = skeletonUtilityBone.transform,
							src = skeletonUtilityBone2.transform
						});
						break;
					}
				}
			}
			Component[] components = componentsInChildren3;
			DestroyComponents(components);
			components = GetComponentsInChildren<Joint>();
			DestroyComponents(components);
			components = GetComponentsInChildren<Rigidbody>();
			DestroyComponents(components);
			components = GetComponentsInChildren<Collider>();
			DestroyComponents(components);
		}

		private static void DestroyComponents(Component[] components)
		{
			int i = 0;
			for (int num = components.Length; i < num; i++)
			{
				UnityEngine.Object.Destroy(components[i]);
			}
		}

		private void FixedUpdate()
		{
			if (physicsSystem == PhysicsSystem.Physics2D)
			{
				Rigidbody2D component = shadowRoot.GetComponent<Rigidbody2D>();
				component.MovePosition(base.transform.position);
				component.MoveRotation(base.transform.rotation.eulerAngles.z);
			}
			else
			{
				Rigidbody component2 = shadowRoot.GetComponent<Rigidbody>();
				component2.MovePosition(base.transform.position);
				component2.MoveRotation(base.transform.rotation);
			}
			int i = 0;
			for (int count = shadowTable.Count; i < count; i++)
			{
				TransformPair transformPair = shadowTable[i];
				transformPair.dest.localPosition = transformPair.src.localPosition;
				transformPair.dest.localRotation = transformPair.src.localRotation;
			}
		}
	}
}
