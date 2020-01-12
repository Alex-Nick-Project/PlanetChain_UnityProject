using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sample : MonoBehaviour {

    private void Start()
    {
        set_texture(WalletManager.Instance.walletSelectionDropdown.itemText.text);
    }

    public void set_texture(string address)
    {      
        Identicon icon = new Identicon(address, 8);
        Texture2D texture = icon.GetBitmap(16);
        GetComponent<Renderer>().material.mainTexture = texture;
        texture.Apply();
    }
    
}
