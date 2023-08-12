using System.Reflection;

namespace SortingVisualizer.Misc;

public static class UriHelpers
{
    /// <summary>
    /// Reads all the data as a byte array from some URI.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method can also read embedded resources using the netres scheme.
    /// A netres URI looks something like:
    /// </para>
    /// <code>
    /// netres://Example.Assembly.Net/Assets/MyImage.png
    /// </code>
    /// <para>
    /// The above URI will search for the resource <c>Example.Assembly.Net.Assets.MyImage.png</c>
    /// in the assembly <c>Example.Assembly.Net</c>.
    /// </para>
    /// </remarks>
    /// <param name="uri">the URI.</param>
    /// <returns>A byte array with all the data</returns>
    /// <exception cref="ArgumentException">If the URI scheme is not supported.</exception>
    public static async Task<byte[]> ReadAllBytesAsync(this Uri uri)
    {
        switch (uri.Scheme)
        {
            case "netres":
            {
                var assembly = uri.Authority == "" ? Assembly.GetCallingAssembly() : 
                    AppDomain.CurrentDomain.GetAssemblies().Single(a => a.FullName != null && a.GetName().Name!.Equals(uri.Authority, StringComparison.InvariantCultureIgnoreCase));
                return await ReadAllFromResourceAsync(assembly, uri.AbsolutePath);
            }
            case "http" or "https" or "ftp":
            {
                using var http = new HttpClient();
                using var resp = await http.GetAsync(uri);
                return await resp.Content.ReadAsByteArrayAsync();
            }
            case "file":
            {
                return await File.ReadAllBytesAsync(uri.LocalPath);
            }
            default:
            {
                throw new ArgumentException("Unsupported URI scheme!");
            }
        }
    }

    private static async Task<byte[]> ReadAllFromResourceAsync(Assembly assembly, string path)
    {
        var resName = assembly.GetName().Name! + path.Replace('/', '.');
        await using var stream = assembly.GetManifestResourceStream(resName) ?? 
            throw new FileNotFoundException($"Resource \"{path}\" in \"{assembly.FullName}\" is not accessible");
        await using var memStream = new MemoryStream();
        
        await stream.CopyToAsync(memStream);
        memStream.Write(new ReadOnlySpan<byte>(0));
        return memStream.ToArray();
    }

    public static byte[] ReadAllBytes(this Uri uri)
    {
        return ReadAllBytesAsync(uri).Result;
    }
    
    
}