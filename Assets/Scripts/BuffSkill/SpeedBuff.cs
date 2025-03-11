using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBuff : BaseBuff
{
    protected PlayerController playerController;
    protected override void Start()
    {
        playerController = GameManager.Instance.Player.controller;
        base.Start();
    }
    protected override void BuffStart()
    {
        playerController.moveSpeed += increasSpeed;
    }
    protected override void BuffEnd()
    {
        playerController.moveSpeed -= increasSpeed;
    }
    public override BuffName GetBuffName()
    {
        return BuffName.SpeedUp;
    }
}
