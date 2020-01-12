/* WalletData wd = WalletManager.Instance.GetSelectedWalletData();  wd.address */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.ABI.Encoders;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Transactions;
using Nethereum.Signer;
using UnityEngine;
using UnityEngine.UI;


public class contractSetup {

    public static string ABI = @"[
    {
        'constant': false,
        'inputs': [
            {
                'internalType': 'string',
                'name': 'name',
                'type': 'string'
            },
            {
                'internalType': 'uint256',
                'name': 'pin',
                'type': 'uint256'
            }
        ],
        'name': 'addUser',
        'outputs': [],
        'payable': false,
        'stateMutability': 'nonpayable',
        'type': 'function'
    },
    {
        'constant': false,
        'inputs': [
            {
                'internalType': 'uint256',
                'name': 'id',
                'type': 'uint256'
            }
        ],
        'name': 'close',
        'outputs': [],
        'payable': false,
        'stateMutability': 'nonpayable',
        'type': 'function'
    },
    {
        'constant': false,
        'inputs': [
            {
                'internalType': 'uint256',
                'name': 'cap',
                'type': 'uint256'
            },
            {
                'internalType': 'uint256',
                'name': 'timer',
                'type': 'uint256'
            },
            {
                'internalType': 'uint256',
                'name': '_prn',
                'type': 'uint256'
            }
        ],
        'name': 'createRoom',
        'outputs': [
            {
                'internalType': 'uint256',
                'name': '',
                'type': 'uint256'
            }
        ],
        'payable': true,
        'stateMutability': 'payable',
        'type': 'function'
    },
    {
        'constant': false,
        'inputs': [
            {
                'internalType': 'string',
                'name': 'name',
                'type': 'string'
            },
            {
                'internalType': 'uint256',
                'name': 'pin',
                'type': 'uint256'
            },
            {
                'internalType': 'uint256',
                'name': 'newPin',
                'type': 'uint256'
            }
        ],
        'name': 'editUser',
        'outputs': [],
        'payable': false,
        'stateMutability': 'nonpayable',
        'type': 'function'
    },
    {
        'constant': false,
        'inputs': [
            {
                'internalType': 'uint256',
                'name': 'id',
                'type': 'uint256'
            },
            {
                'internalType': 'uint256',
                'name': 'prn',
                'type': 'uint256'
            }
        ],
        'name': 'joinRoom',
        'outputs': [],
        'payable': true,
        'stateMutability': 'payable',
        'type': 'function'
    },
    {
        'inputs': [
            {
                'internalType': 'uint8',
                'name': 'percent',
                'type': 'uint8'
            }
        ],
        'payable': true,
        'stateMutability': 'payable',
        'type': 'constructor'
    },
    {
        'constant': true,
        'inputs': [
            {
                'internalType': 'address',
                'name': 'x',
                'type': 'address'
            }
        ],
        'name': '_getName',
        'outputs': [
            {
                'internalType': 'string',
                'name': '',
                'type': 'string'
            }
        ],
        'payable': false,
        'stateMutability': 'view',
        'type': 'function'
    },
    {
        'constant': true,
        'inputs': [
            {
                'internalType': 'uint256',
                'name': 'pin',
                'type': 'uint256'
            }
        ],
        'name': 'askPIN',
        'outputs': [
            {
                'internalType': 'bool',
                'name': '',
                'type': 'bool'
            }
        ],
        'payable': false,
        'stateMutability': 'view',
        'type': 'function'
    },
    {
        'constant': true,
        'inputs': [
            {
                'internalType': 'uint256',
                'name': 'id',
                'type': 'uint256'
            }
        ],
        'name': 'getAccs',
        'outputs': [
            {
                'internalType': 'address[]',
                'name': '',
                'type': 'address[]'
            }
        ],
        'payable': false,
        'stateMutability': 'view',
        'type': 'function'
    },
    {
        'constant': true,
        'inputs': [],
        'name': 'getMyRoom',
        'outputs': [
            {
                'internalType': 'uint256[]',
                'name': '',
                'type': 'uint256[]'
            }
        ],
        'payable': false,
        'stateMutability': 'view',
        'type': 'function'
    },
    {
        'constant': true,
        'inputs': [],
        'name': 'getName',
        'outputs': [
            {
                'internalType': 'string',
                'name': '',
                'type': 'string'
            }
        ],
        'payable': false,
        'stateMutability': 'view',
        'type': 'function'
    },
    {
        'constant': true,
        'inputs': [],
        'name': 'getOpen',
        'outputs': [
            {
                'internalType': 'uint256[]',
                'name': '',
                'type': 'uint256[]'
            }
        ],
        'payable': false,
        'stateMutability': 'view',
        'type': 'function'
    },
    {
        'constant': true,
        'inputs': [
            {
                'internalType': 'uint256',
                'name': 'id',
                'type': 'uint256'
            }
        ],
        'name': 'getRoom',
        'outputs': [
            {
                'internalType': 'uint256',
                'name': '',
                'type': 'uint256'
            },
            {
                'internalType': 'uint256',
                'name': '',
                'type': 'uint256'
            },
            {
                'internalType': 'uint256',
                'name': '',
                'type': 'uint256'
            },
            {
                'internalType': 'uint256',
                'name': '',
                'type': 'uint256'
            },
            {
                'internalType': 'uint256',
                'name': '',
                'type': 'uint256'
            },
            {
                'internalType': 'bool',
                'name': '',
                'type': 'bool'
            }
        ],
        'payable': false,
        'stateMutability': 'view',
        'type': 'function'
    }
]";
    private static string contractAddress = "0x3dd57ecf52d13fe47509378cc2e83896c10e5fa8";

	private Contract contract;
    public contractSetup () {
        this.contract = new Contract (null, ABI, contractAddress);
    }

	// GET ALL OPEN
	public Function getOpenFunction () {
        return contract.GetFunction ("getOpen");
    }

	public CallInput createGetOpenCallInput () {
        var
        function = getOpenFunction ();
        return function.CreateCallInput ();
    }
	public List<int> decodeGetOpen (string result) {
        var
        function = getOpenFunction ();
        return function.DecodeSimpleTypeOutput<List<int>>(result);
		
    }

	//GET NAME
	public Function getNameFunction()
	{
		return contract.GetFunction("_getName");
	}

	public CallInput createGetNameCallInput(string address)
	{

		var
		function = getNameFunction();
		return function.CreateCallInput(address);
	}

	public string decodeGetName(string result)
	{
		var
		function = getNameFunction();
		return function.DecodeSimpleTypeOutput<string>(result);

	}

	//	GET ROOM DATA
	public Function getRoomFunction () {
        return contract.GetFunction ("getRoom");
    }

	 public CallInput createGetRoomCallInput (int id) {
        var
        function = getRoomFunction ();
        return function.CreateCallInput (id);
    }
	public decodGetRooms decodeGetOpenRoom (string result) {
        var
        function = getRoomFunction ();
        return function.DecodeDTOTypeOutput<decodGetRooms>(result);
		
    }

	// GET MY ROOM
	public Function getMyRoomFunction () {
        return contract.GetFunction ("getMyRoom");
    }

	 public CallInput createGetMyRoomCallInput () {
        var
        function = getMyRoomFunction ();
        return function.CreateCallInput();
    }
	public List<int> decodeGetMyRoom (string result) {
        var
        function = getMyRoomFunction ();
        return function.DecodeSimpleTypeOutput<List<int>>(result);
    }

	//JOIN ROOM
	public Function joinRoomFunction()
	{
		return contract.GetFunction("joinRoom");
	}

	public CallInput createJoinRoomInput()
	{
		var
		function = createRoomFunction();
		return function.CreateCallInput();
	}
	public TransactionInput joinRoomInput(string addressFrom, string privateKey, int id, int prn, HexBigInteger gas = null, HexBigInteger valueAmount = null)
	{
		var numberBytes = new IntTypeEncoder().Encode(id);
		var sha3 = new Nethereum.Util.Sha3Keccack();
		var hash = sha3.CalculateHashFromHex(addressFrom, numberBytes.ToHex());
		var signer = new MessageSigner();
		var signature = signer.Sign(hash.HexToByteArray(), privateKey);
		var ethEcdsa = MessageSigner.ExtractEcdsaSignature(signature);
		object[] array = new object[] { id, prn };
		var
		function = joinRoomFunction();
		return function.CreateTransactionInput(addressFrom, gas, valueAmount, array);
	}
	public Function depositFunction()
	{
		return contract.GetFunction("deposit");
	}
	public TransactionInput deposit(string addressFrom, string privateKey, HexBigInteger gas = null, HexBigInteger valueAmount = null)
	{
		var
		function = depositFunction();
		return function.CreateTransactionInput(addressFrom, gas, valueAmount);
	}

	//CREATE ROOM
	public Function createRoomFunction()
	{
		return contract.GetFunction("createRoom");
	}

	public CallInput createRoomCallInput()
	{
		var
		function = createRoomFunction();
		return function.CreateCallInput();
	}
	public List<int> decodeCreateRoom(string result)
	{
		var
		function = createRoomFunction();
		return function.DecodeSimpleTypeOutput<List<int>>(result);
	}
	public TransactionInput createRoomInput(string addressFrom, string privateKey, int maxPlayers, int Timer ,int prn, HexBigInteger gas = null, HexBigInteger valueAmount = null)
	{
		var numberBytes_1 = new IntTypeEncoder().Encode(Timer);
		var numberBytes = new IntTypeEncoder().Encode(maxPlayers);
		var sha3 = new Nethereum.Util.Sha3Keccack();
		var hash = sha3.CalculateHashFromHex(addressFrom, numberBytes.ToHex(), numberBytes_1.ToHex());
		var signer = new MessageSigner();
		var signature = signer.Sign(hash.HexToByteArray(), privateKey);
		var ethEcdsa = MessageSigner.ExtractEcdsaSignature(signature);
		object[] array = new object[] { maxPlayers, Timer, prn };
		var
		function = createRoomFunction();
		return function.CreateTransactionInput(addressFrom, gas, valueAmount, array);
	}
    //CLOSE ROOM
    public Function closeRoomFunction()
    {
        return contract.GetFunction("close");
    }

    public TransactionInput closeRoomInput(string addressFrom, string privateKey, HexBigInteger gas, int id , HexBigInteger valueAmount = null)
    {
        var numberBytes = new IntTypeEncoder().Encode(id);
        var sha3 = new Nethereum.Util.Sha3Keccack();
        var hash = sha3.CalculateHashFromHex(addressFrom, numberBytes.ToHex());
        var signer = new MessageSigner();
        var signature = signer.Sign(hash.HexToByteArray(), privateKey);
        var ethEcdsa = MessageSigner.ExtractEcdsaSignature(signature);
        object[] array = new object[] { id };
        var
        function = closeRoomFunction();
        return function.CreateTransactionInput(addressFrom, gas, valueAmount, array);
    }

    //CREATE USER
    public Function createUserFunction()
	{
		return contract.GetFunction("addUser");
	}

    public CallInput createUserCallInput()
	{
		var
		function = createUserFunction();
		return function.CreateCallInput();
	}
	public List<int> decodeCreateUser(string result)
	{
		var
		function = createUserFunction();
		return function.DecodeSimpleTypeOutput<List<int>>(result);
	}
	public TransactionInput createUserInput(string addressFrom, string privateKey, HexBigInteger gas, string name, int pin)
	{
		var stringBytes = new StringTypeEncoder().Encode(name);
		var numberBytes = new IntTypeEncoder().Encode(pin);
		var sha3 = new Nethereum.Util.Sha3Keccack();
		var hash = sha3.CalculateHashFromHex(addressFrom, numberBytes.ToHex(), stringBytes.ToHex());
		var signer = new MessageSigner();
		var signature = signer.Sign(hash.HexToByteArray(), privateKey);
		var ethEcdsa = MessageSigner.ExtractEcdsaSignature(signature);
		object[] array = new object[] { name, pin };
		var
		function = createUserFunction();
		return function.CreateTransactionInput(addressFrom, array);
	}

	//EDIT USER
	public Function editUserFunction()
	{
		return contract.GetFunction("editUser");
	}

	public CallInput editUserCallInput()
	{
		var
		function = editUserFunction();
		return function.CreateCallInput();
	}
	public List<int> decodeEditUser(string result)
	{
		var
		function = editUserFunction();
		return function.DecodeSimpleTypeOutput<List<int>>(result);
	}

}
[FunctionOutput]
public class decodGetRooms {
    [Parameter ("uint256", "", 1)]
    public BigInteger Price { get; set; }
	[Parameter ("uint256", "", 2)]
    public int Players { get; set; }
	[Parameter ("uint256", "", 3)]
    public int maxPlayers { get; set; }
	[Parameter ("uint256", "", 4)]
    public int timeCreated { get; set; }
	[Parameter ("uint256", "", 5)]
    public int timeLeft { get; set; }
	[Parameter ("bool", "", 6)]
    public bool Status { get; set; }
}