using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Spine;
using Spine.Unity;

public class CPopupGo : MonoBehaviour {

	//List<Sprite> go_images;
	//Image go;
	[SerializeField] private List<AnimationReferenceAsset> animationReferences = new List<AnimationReferenceAsset>();
	public SkeletonGraphic anim;
	void Awake()
	{
		//this.go_images = new List<Sprite>();
/*		animationReferences = new List<AnimationReferenceAsset>();

		for (int i = 1; i <= 9; ++i)
		{
			Sprite spr = CSpriteManager.Instance.get_sprite(string.Format("go_{0:D2}", i));
			this.go_images.Add(spr);
		}
*/
		//this.go = transform.Find("image").GetComponent<Image>();
	}


	public void refresh(int howmany_go)
	{
		if (howmany_go <= 0 || howmany_go >= 10)
		{
			return;
		}

		anim.skeletonDataAsset = animationReferences[howmany_go - 1].SkeletonDataAsset;
		anim.AnimationState.SetAnimation(0, animationReferences[howmany_go - 1], false);
			//this.go.sprite = this.go_images[howmany_go - 1];
	}
}
