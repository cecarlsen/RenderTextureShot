using UnityEngine;
using UnityEditor;

public class PackageTool
{
	[MenuItem("Package/Update Package")]
	static void UpdatePackage()
	{
		AssetDatabase.ExportPackage( "Assets/RenderTextureShot", "RenderTextureShot.unitypackage", ExportPackageOptions.Recurse );
		Debug.Log( "Package is build\n" );
	}
}
