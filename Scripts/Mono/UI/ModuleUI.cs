using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleUI : ItemUI {
    public void OnPointerEnter_SetGaragePointerOnModuleUI()
    {
        Garage._instance.pointerOnModuleUI = this;
    }

    public void OnPointerExit_SetGaragePointerOnModuleUI()
    {
        Garage._instance.pointerOnModuleUI = null;
    }

    //这个函数只拿来改变UI,不会换枪换炮
    public virtual void SetUI(ItemInfo item)
    {

    }
}
