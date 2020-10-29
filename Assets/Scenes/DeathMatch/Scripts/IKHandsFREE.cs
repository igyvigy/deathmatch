using UnityEngine;
using System.Collections;

namespace DeathMatchCharacterAnims{

	public class IKHandsFREE : MonoBehaviour{
		private Animator animator;
		private DeathMatchPlayerWeaponController weaponController;
		public Transform leftHandObj;
		public Transform attachLeft;
		[Range(0, 1)] public float leftHandPositionWeight;
		[Range(0, 1)] public float leftHandRotationWeight;
		Transform blendToTransform;
		
		void Awake() {
			animator = GetComponent<Animator>();
			weaponController = GetComponent<DeathMatchPlayerWeaponController>();
		}
		
		void OnAnimatorIK(int layerIndex){
			if(leftHandObj != null){
				animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandPositionWeight);
				animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandRotationWeight);
				animator.SetIKPosition(AvatarIKGoal.LeftHand, attachLeft.position);                    
				animator.SetIKRotation(AvatarIKGoal.LeftHand, attachLeft.rotation);
			}
		}
			
		public IEnumerator _BlendIK(bool blendOn, float delay, float timeToBlend, int weapon){
			if(weapon > 0){
				GetCurrentWeaponAttachPoint(weapon);
			}
			else{
				yield break;
			}
			yield return new WaitForSeconds(delay);
			float t = 0f;
			float blendTo = 0;
			float blendFrom = 0;
			if(blendOn){
				blendTo = 1;
			}
			else{
				blendFrom = 1;
			}
			while(t < 1){
				t += Time.deltaTime / timeToBlend;
				attachLeft = blendToTransform;
				leftHandPositionWeight = Mathf.Lerp(blendFrom, blendTo, t);
				leftHandRotationWeight = Mathf.Lerp(blendFrom, blendTo, t);
				yield return null;
			}
			weaponController.isSwitchingFinished = true;
			yield break;
		}

		void GetCurrentWeaponAttachPoint(int weapon){
			if(weapon == 1){
				blendToTransform = weaponController.twoHandSword.transform.GetChild(0).transform;
			}
		}
	}
}