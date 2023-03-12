/*
	Copyright (c) 2020 Carl Emil Carlsen (http://cec.dk)

	RuntimeRenderTextureShot
	========================

	Press a specified key to save a RenderTexture to file in a folder named "Shots" on your desktop.
*/

using System.IO;
using UnityEngine;
using RenderTextureShot;

public class RuntimeRenderTextureShot : MonoBehaviour
{
	[SerializeField] RenderTexture _renderTexture = null;
	[SerializeField] KeyCode _key = KeyCode.S;
	[SerializeField] KeyCode _modKey = KeyCode.None;
	[SerializeField] Encoding _encoding = Encoding.JPG;
	
	Texture2D _texture2D;
	string _outputDirectoryPath;
		
	const int jpegQuality = 98; // 0 to 100
	const string shotsFolderName = "Shots";
	

	public RenderTexture renderTexture {
		get { return _renderTexture; }
		set { _renderTexture = value; }
	}



	void Awake()
	{
		_outputDirectoryPath = PathHelper.GetDesktopPath() + "/" + shotsFolderName;
		if( !Directory.Exists( _outputDirectoryPath ) ) Directory.CreateDirectory( _outputDirectoryPath );
	}


	void OnDestroy()
	{
		if( _texture2D ) Destroy( _texture2D );
	}


	void LateUpdate()
	{
		if( Input.GetKeyDown( _key ) && ( _modKey == KeyCode.None || Input.GetKey( _modKey ) ) ) SaveShot();
	}


	public void SaveShot()
	{
		string fileName = PathHelper.GetDateTimeCode() + " " + _renderTexture.name;
		string filePath = PathHelper.GenerateUniqueFilePath( _outputDirectoryPath, fileName, "." + _encoding.ToString().ToLower() );

		// Convert to bytes.
		byte[] bytes = _renderTexture.Encode( ref _texture2D, _encoding, jpegQuality );

		// Save to file.
		File.WriteAllBytes( filePath, bytes );

		// Log.
		Debug.Log( "Saved RenderTexture to " + _encoding + ".\n" + filePath );
	}
}