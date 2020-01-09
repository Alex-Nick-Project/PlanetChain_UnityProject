using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.RPC.Eth.DTOs;
using System.Numerics;
using Nethereum.Util;
using TMPro;
using System.Linq;

// TODO: IMPORTANT! A serialization bug sometimes makes the walletcache.data broken, fix this!
// for now, always backup walletcache.data

[System.Serializable]
public class WalletData
{
    public string name;
    public string address;

    // TODO: stored for convenience, may need to remove for security
    public string cachedPassword;
    public string encryptedJson;
    public string privateKey;
}

public class WalletManager : MonoBehaviour
{

    // create class as singleton
    private static WalletManager instance;
    public static WalletManager Instance { get { return instance; } }
    public void Awake() { if (instance == null) instance = this; }
    public void OnDestroy() { if (instance == this) instance = null; }

    [Header("Config")]

    public string networkUrl;

    [Header("UI Components")]

    public PasswordInputField passwordInputField;

    public GameObject createWalletPanel;
    public GameObject loadingIndicatorPanel;
    public TMP_Dropdown walletSelectionDropdown;
    public Text EtherBalanceText;
    public Text PlayerNameText;
    public GameObject LoadingIndicator;
    public Button CopyToClipboardButton;
    public GameObject MainPanel;
    public Animator UIAnimator;
    private bool isPaused = false;
    private bool dataSaved = false;

    // events
    static UnityEvent newAccountAdded;
    static UnityEvent loadingFinished;

    private static List<WalletData> walletList = new List<WalletData>();

    // used for saving 
    private BinaryFormatter bf;
    private FileStream file;
    private string filePath;
    private const string fileName = "walletcache.data";

    /* show QR Scanner
    public void ToggleQRScannerDisplay(bool forceMain = false)
    {
        if (forceMain)
        {
            MainPanel.SetActive(true);
            QRScannerPanel.SetActive(false);
        }

        else
        {
            MainPanel.SetActive(!MainPanel.activeSelf);
            QRScannerPanel.SetActive(!QRScannerPanel.activeSelf);
        }
    }
    
    // show QR code display
    public void ToogleQRCodeDisplay()
    {
        currencyInfoScrollView.SetActive(!currencyInfoScrollView.activeSelf);
        QRPanel.gameObject.SetActive(!currencyInfoScrollView.activeSelf);
    }
    */
    // copy account address to clipboard
    public void CopyToClipboard()
    {
        CopyToClipboard(walletList[walletSelectionDropdown.value].address);
    }

    public void CopyToClipboard(string s)
    {
        TextEditor te = new TextEditor();
        te.text = s;
        te.SelectAll();
        te.Copy();
    }


    public void Start()
    {
        subscribeToEvents();
        LoadWalletsFromFile();
        RefreshTopPanelView();
    }

    private void subscribeToEvents()
    {
        newAccountAdded = new UnityEvent();
        loadingFinished = new UnityEvent();

        newAccountAdded.AddListener(RefreshWalletAccountDropdown);
        newAccountAdded.AddListener(RefreshTopPanelView);

        loadingFinished.AddListener(hideLoadingIndicatorPanel);
    }

    void LoadWalletsFromFile()
    {
        filePath = (Application.persistentDataPath + "/" + fileName);

        if (File.Exists(filePath))
        {
            bf = new BinaryFormatter();
            file = File.Open(filePath, FileMode.Open);

            walletList = (List<WalletData>)bf.Deserialize(file);

            file.Close();
        }

        RefreshWalletAccountDropdown();
    }


    public void RefreshWalletAccountDropdown()
    {
        //walletSelectionDropdown.ClearOptions();
        foreach (WalletData w in walletList)
        {

            if (!walletSelectionDropdown.options.Any(f => f.text == w.address))
            { 
                print("refreshing options");
                walletSelectionDropdown.AddOptions(new List<string> { w.address });
                walletSelectionDropdown.value = walletSelectionDropdown.options.Count;
            }

        }

        if (walletList.Count != 0 && !PlayerPrefs.HasKey("Tutorial"))
        {
            PlayerPrefs.SetInt("Tutorial", 0);
        }

        // add wallet create option
        // walletSelectionDropdown.AddOptions(new List<string> { "No Accounts Add one" });
    }

    public void RefreshTopPanelView()
    {
        //passwordInputField.resetFields();

        int index = walletSelectionDropdown.value;

        if (index <= 2)
        {
            if (index == 1)
            {
                UIAnimator.SetTrigger("CreateAccount");
            }

            EtherBalanceText.text = "Please, Select or";
            PlayerNameText.text = "Create an User";
            //CopyToClipboardButton.interactable = false;

            //currencyInfoScrollView.SetActive(true);
            //QRPanel.gameObject.SetActive(false);

        }
        else
        {
            //CopyToClipboardButton.interactable = true;

            //QRPanel.RenderQRCode(walletList[index].address);
            StartCoroutine(CheckAccountBalanceCoroutine(walletList[index-3].address, walletList[index-3].name));
        }
    }


    void SaveDataToFile()
    {

        bf = new BinaryFormatter();
        file = File.Create(filePath);

        bf.Serialize(file, walletList);
        file.Close();

        dataSaved = true;
    }

    void OnApplicationFocus(bool hasFocus)
    {
        isPaused = !hasFocus;
    }

    void OnApplicationPause(bool pauseStatus)
    {
        isPaused = pauseStatus;

        if (isPaused)
            SaveDataToFile();
        else
            dataSaved = false;
    }

    void OnApplicationQuit()
    {
        if (!dataSaved)
            SaveDataToFile();
    }


    private void disableOperationPanels()
    {
        createWalletPanel.SetActive(false);
    }

    private void hideLoadingIndicatorPanel()
    {
        loadingIndicatorPanel.SetActive(false);
    }

    private void showLoadingIndicatorPanel()
    {
        loadingIndicatorPanel.SetActive(true);
    }

    public void CreateWallet()
    {
        disableOperationPanels();
        showLoadingIndicatorPanel();

        // Here we call CreateAccount() and we send it a password to encrypt the new account
        StartCoroutine(CreateAccountCoroutine(passwordInputField.passwordString(), passwordInputField.userString()));
    }

    public IEnumerator CheckAccountBalanceCoroutine(string address, string name)
    {
        EtherBalanceText.text = "Loading Balance...";
        PlayerNameText.text = "Loading Name...";
        yield return 0; // allow UI to update

        var getBalanceRequest = new EthGetBalanceUnityRequest(networkUrl);
        string etherBalance;
        string customTokenBalance;

        yield return getBalanceRequest.SendRequest(address, BlockParameter.CreateLatest());
        if (getBalanceRequest.Exception == null)
        {
            etherBalance = UnitConversion.Convert.FromWei(getBalanceRequest.Result.Value).ToString();
        }
        else
        {
            throw new System.InvalidOperationException("Get balance request failed");
        }

        var tokenBalanceRequest = new EthCallUnityRequest(networkUrl);

        // get custom token balance (uint 256)
        yield return tokenBalanceRequest.SendRequest(TokenContractService.Instance.CreateCallInput("balanceOf", address),
            BlockParameter.CreateLatest());

        customTokenBalance = UnitConversion.Convert.FromWei(
            TokenContractService.Instance.DecodeVariable<BigInteger>("balanceOf", tokenBalanceRequest.Result),
            TokenContractService.Instance.TokenInfo.decimals).ToString();

        EtherBalanceText.text = "ETH: " + etherBalance;
        PlayerNameText.text = name;
    }

    // We create the function which will check the balance of the address and return a callback with a decimal variable
    public IEnumerator CreateAccountCoroutine(string password, string accountName)
    {
        yield return 0; // allow UI to update
                        //transictionScripts _transictionScripts = new transictionScripts();
                        //var name = StartCoroutine(_transictionScripts.createUsers(accountName));

        CreateAccount(password, (address, encryptedJson, privateKey) =>
        {
            // We just print the address and the encrypted json we just created
            Debug.Log(address);
            Debug.Log(encryptedJson);

            WalletData w = new WalletData();
            w.name = accountName;
            w.address = address;
            w.cachedPassword = password;
            w.encryptedJson = encryptedJson;
            w.privateKey = privateKey;

            walletList.Add(w);

            newAccountAdded.Invoke();
            loadingFinished.Invoke();
            Debug.Log(accountName);
            RefreshTopPanelView();
        });
    }

    // This function will just execute a callback after it creates and encrypt a new account
    public void CreateAccount(string password, System.Action<string, string, string> callback)
    {
        // We use the Nethereum.Signer to generate a new secret key
        var ecKey = Nethereum.Signer.EthECKey.GenerateKey();

        // After creating the secret key, we can get the public address and the private key with
        // ecKey.GetPublicAddress() and ecKey.GetPrivateKeyAsBytes()
        // (so it return it as bytes to be encrypted)
        var address = ecKey.GetPublicAddress();
        var privateKeyBytes = ecKey.GetPrivateKeyAsBytes();
        var privateKey = ecKey.GetPrivateKey();

        // Then we define a new KeyStore service
        var keystoreservice = new Nethereum.KeyStore.KeyStoreService();

        // And we can proceed to define encryptedJson with EncryptAndGenerateDefaultKeyStoreAsJson(),
        // and send it the password, the private key and the address to be encrypted.
        var encryptedJson = keystoreservice.EncryptAndGenerateDefaultKeyStoreAsJson(password, privateKeyBytes, address);
        // Finally we execute the callback and return our public address and the encrypted json.
        // (you will only be able to decrypt the json with the password used to encrypt it)
        callback(address, encryptedJson, privateKey);
    }

    public WalletData GetSelectedWalletData()
    {

        int index = walletSelectionDropdown.value - 3;

        if (index >= walletSelectionDropdown.options.Count - 1 || index < 0)
            return null;
        else
        {

            return walletList[index];

        }
    }



}
