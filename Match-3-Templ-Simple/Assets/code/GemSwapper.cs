using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GemSwapper : MonoBehaviour {

	public Image img { get; set; }
    public RectTransform rectTrans { get; set; }
    public int srcPosX { get; set; }
	public int srcPosY { get; set; }
	public int targetPosX { get; set; }
	public int targetPosY { get; set; }
	private GemScript sourceGem, targetGem;

	// Use this for initialization
	void Start () {
		img = this.GetComponent<Image>();
		rectTrans = img.rectTransform;
		targetPosX = -9999;
		targetPosY = -9999;
        Debug.Log("Init Swapper " + this.name);

        hideIt();
    }

	// Update is called once per frame
	void Update ()
	{

	}

	public bool reachedTarget()
	{
		bool b = (rectTrans.position.x >= targetPosX-3 && 
				rectTrans.position.x <= targetPosX+3 && 
				rectTrans.position.y >= targetPosY-3 &&
				rectTrans.position.y <= targetPosY+3 );
		Debug.Log("" + (int)rectTrans.position.x + "|" + (int)rectTrans.position.y + " == " + targetPosX + "|" + targetPosY + "   " + b);
		return b;
	}

    public bool reachedSourceAgain()
	{
		return ((int)rectTrans.position.x == srcPosX && (int)rectTrans.position.y == srcPosY);
	}

    public void moveToTarget()
    {
        if ((int)rectTrans.position.x > targetPosX) {
            img.translateIUPosX(-Speed.get(0.8f));
            if (rectTrans.position.x <= targetPosX-3) img.setIUPosX(targetPosX); 
        }
        if ((int)rectTrans.position.y > targetPosY) {
            img.translateIUPosY(-Speed.get(0.8f));
            if (rectTrans.position.y <= targetPosY-3) img.setIUPosY(targetPosY); 
        }
        if ((int)rectTrans.position.x < targetPosX) {
            img.translateIUPosX(Speed.get(0.8f));
            if (rectTrans.position.x >= targetPosX+3) img.setIUPosX(targetPosX);
        }
        if ((int)rectTrans.position.y < targetPosY) {
            img.translateIUPosY(Speed.get(0.8f));
            if (rectTrans.position.y >= targetPosY+3) img.setIUPosY(targetPosY); 
        }
    }

	public void startSwap(GemScript src, GemScript target)
	{
		sourceGem = src;
		targetGem = target;
		img.sprite = src.myImg.sprite;
		img.color  = new Color(1f, 1f, 1f, 1f);
		targetPosX = (int)target.myImg.rectTransform.position.x;
		targetPosY = (int)target.myImg.rectTransform.position.y;
		srcPosX = (int)src.myImg.rectTransform.position.x;
		srcPosY = (int)src.myImg.rectTransform.position.y;
		img.setIUPos(srcPosX, srcPosY);
		Core.hideSwapPair();
	}

	public void hideIt()
	{
		img.color = new Color(1f, 1f, 1f, 0f);
		img.setIUPos(-9999, -9999);
		Debug.Log("Hiding Swapper " + this.name);
	}

}
