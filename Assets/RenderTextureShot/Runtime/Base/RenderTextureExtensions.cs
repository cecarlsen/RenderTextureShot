/*
	Copyright © Carl Emil Carlsen 2020-2023
	http://cec.dk
*/

using UnityEngine;

namespace RenderTextureShot
{
	public static class RenderTextureExtensions
	{

		public static byte[] Encode( this RenderTexture rt, ref Texture2D texture2D, Encoding encoding, int jpegQuality = 98 )
		{
			// For JPG, PNG and TGA we want gamma corrected values, so we need to convert to sRGB. 
			// If the RenderTexture is not already set up for this, we create a transfer texture and 
			// handle conversion using a blit pass.
			bool useTransferTexture = !rt.sRGB && encoding != Encoding.EXR;
			RenderTexture transferTexture = null;

			// Ensure recoures.
			if( !texture2D || texture2D.width != rt.width || texture2D.height != rt.height )
			{
				if( texture2D ) Object.Destroy( texture2D );

				TextureFormat format = encoding == Encoding.EXR ? TextureFormat.RGBAFloat : TextureFormat.ARGB32;
				texture2D = new Texture2D( rt.width, rt.height, format, false );
				if( useTransferTexture )
				{
					RenderTextureDescriptor rtDescp = new RenderTextureDescriptor() {
						width = rt.width,
						height = rt.height,
						dimension = UnityEngine.Rendering.TextureDimension.Tex2D,
						volumeDepth = 1,
						colorFormat = RenderTextureFormat.ARGB32,
						depthBufferBits = 0,
						useMipMap = false,
						msaaSamples = 1,
						sRGB = true,
					};
					transferTexture = RenderTexture.GetTemporary( rtDescp );
				}
			}

			// Convert linear to sRGB gamma
			if( useTransferTexture ) Graphics.Blit( rt, transferTexture );

			// Grab pixels from GPU.
			RenderTexture prevActive = RenderTexture.active;
			RenderTexture.active = useTransferTexture ? transferTexture : rt;
			texture2D.ReadPixels( new Rect( 0, 0, texture2D.width, texture2D.height ), 0, 0 );
			RenderTexture.active = prevActive;
			if( useTransferTexture ) RenderTexture.ReleaseTemporary( transferTexture );
			texture2D.Apply();

			// Save to file.
			switch( encoding )
			{
				case Encoding.JPG: return texture2D.EncodeToJPG( jpegQuality );
				case Encoding.PNG: return texture2D.EncodeToPNG();
				case Encoding.TGA: return texture2D.EncodeToTGA();
				default: return texture2D.EncodeToEXR( Texture2D.EXRFlags.None );
			}
		}
	}
}