/*
	Copyright © Carl Emil Carlsen 2020
	http://cec.dk
*/

using System.IO;
using UnityEngine;
using UnityEditor;

namespace RenderTextureShot
{
	public static class EditorRenderTextureShot
	{
		const int jpegQuality = 98; // 1-100 %.

		[MenuItem("CONTEXT/RenderTexture/Save as PNG...")]
		public static void DoRenderTextureSavePNG( MenuCommand menuCommand )
		{
			RenderTexture renderTex = menuCommand.context as RenderTexture;
			Save( renderTex, Encoding.PNG );
		}
		
		[MenuItem("CONTEXT/RenderTexture/Save as JPG...")]
		public static void DoRenderTextureSaveJPG( MenuCommand menuCommand )
		{
			RenderTexture renderTex = menuCommand.context as RenderTexture;
			Save( renderTex, Encoding.JPG );
		}

		[MenuItem( "CONTEXT/RenderTexture/Save as TGA..." )]
		public static void DoRenderTextureSaveTGA( MenuCommand menuCommand )
		{
			RenderTexture renderTex = menuCommand.context as RenderTexture;
			Save( renderTex, Encoding.TGA );
		}

		[MenuItem( "CONTEXT/RenderTexture/Save as EXR..." )]
		public static void DoRenderTextureSaveEXR( MenuCommand menuCommand )
		{
			RenderTexture renderTex = menuCommand.context as RenderTexture;
			Save( renderTex, Encoding.EXR );
		}


		static void Save( RenderTexture rt, Encoding encoding )
		{
			// Get file path.
			string fileNameProposal = PathHelper.GetDateTimeCode() + " " + rt.name;
			string filePath = EditorUtility.SaveFilePanel( "Save Texture", "Assets/", fileNameProposal, encoding.ToString().ToLower() );
			if( string.IsNullOrEmpty( filePath ) ) return;

			// Convert.
			Texture2D tex = null;
			byte[] bytes = rt.ToBytes( ref tex, encoding, jpegQuality );
			if( tex ) Object.DestroyImmediate( tex );

			// Save to file.
			File.WriteAllBytes( filePath, bytes );

			// Log.
			Debug.Log( "Saved RenderTexture to " + encoding + ".\n" + filePath );
		}
	}
}