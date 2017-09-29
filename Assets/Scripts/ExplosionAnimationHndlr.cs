using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAnimationHndlr : MonoBehaviour {

    public void OnAnimationEnd() {

        MainGame.instance.DisableExplosion();

    }
}
