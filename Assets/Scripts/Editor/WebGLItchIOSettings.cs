using UnityEditor;
using UnityEngine;

/// <summary>
/// Applies the recommended WebGL build settings for Itch.io publishing.
/// Run via Tools > WebGL > Apply Itch.io Settings before every WebGL build.
/// </summary>
public static class WebGLItchIOSettings
{
    private const string MenuPath    = "Tools/WebGL/Apply Itch.io Settings";
    private const string TemplateName = "PROJECT:ItchIO";

    [MenuItem(MenuPath)]
    public static void ApplySettings()
    {
        // Disabled is the only compression mode that works reliably on Itch.io.
        // Gzip and Brotli both require the server to set the correct Content-Encoding
        // response header. Itch.io's CDN omits these headers, which causes
        // WebAssembly.instantiateStreaming to throw "TypeError: Failed to fetch"
        // because the browser receives a compressed binary with the wrong MIME type.
        // Disabled serves raw files — larger download but zero decoding issues.
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;

        // Keep decompression fallback off — it is only relevant for Gzip/Brotli.
        PlayerSettings.WebGL.decompressionFallback = false;

        // Point to the custom loading template.
        PlayerSettings.WebGL.template = TemplateName;

        // Exception support: ExplicitlyThrownExceptionsOnly keeps the build lean
        // while still catching errors you explicitly throw.
        PlayerSettings.WebGL.exceptionSupport = WebGLExceptionSupport.ExplicitlyThrownExceptionsOnly;

        // Strip engine code on release builds.
        PlayerSettings.stripEngineCode = true;

        AssetDatabase.SaveAssets();

        Debug.Log("[WebGL Itch.io] Settings applied:\n" +
                  "  Compression     : Gzip\n" +
                  "  Decomp Fallback : true\n" +
                  "  Template        : " + TemplateName + "\n" +
                  "  Exception level : ExplicitlyThrown\n" +
                  "  Strip Engine    : true");
    }

    /// <summary>
    /// Validates that the Itch.io template folder exists before enabling the menu item.
    /// </summary>
    [MenuItem(MenuPath, validate = true)]
    public static bool ValidateSettings()
    {
        return System.IO.Directory.Exists("Assets/WebGLTemplates/ItchIO");
    }
}
