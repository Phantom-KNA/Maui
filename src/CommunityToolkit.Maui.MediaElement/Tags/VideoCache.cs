#if ANDROID
using Android.Content;
using Com.Google.Android.Exoplayer2.Database;
using Com.Google.Android.Exoplayer2.Upstream.Cache;
using Java.IO;
#endif
using Microsoft.Maui.Storage;
using File = Java.IO.File;

namespace CommunityToolkit.Maui.Tags;
public static class VideoCache
{
	public static SimpleCache? SimpleCache;

	public static SimpleCache GetInstance(Context? context)
	{
		var databaseProvider = new StandaloneDatabaseProvider(context);

		if (SimpleCache is null)
		{
			var file = new File(context?.CacheDir, "exoplayer");
			file.DeleteOnExit();

			SimpleCache = new SimpleCache(
				file,
				new LeastRecentlyUsedCacheEvictor(300L * 1024L * 1024L),
				databaseProvider
			);
		}

		return SimpleCache;
	}

	public static void Release()
	{
		try
		{
			var dir = System.IO.Path.Combine(FileSystem.CacheDirectory, "exoplayer");
			System.IO.Directory.Delete(dir, true);
		}
		catch
		{
			// Ignore
		}

		SimpleCache?.Release();
		SimpleCache = null;
	}
}
