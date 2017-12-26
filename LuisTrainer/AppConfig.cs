using System;
using System.Linq;

namespace LuisTrainer
{
    public class AppConfig
    {
        public string Path { get; }
        public string AppId { get; }
        public string AppKey { get; }
        public string VersionId { get; }

        internal AppConfig (string path, string appId, string appKey, string versionId) 
        {
            this.Path = path;
            this.AppId = appId;
            this.AppKey = appKey;
            this.VersionId = versionId;
        }

        public override string ToString()
        {
            return $"Path={Path}" + Environment.NewLine +
                $"AppId={AppId.Substring(0, 3).PadRight(AppId.Length, '*')}" + Environment.NewLine +
                $"AppKey={AppKey.Substring(0, 3).PadRight(AppKey.Length, '*')}" + Environment.NewLine +
                $"VersionId={VersionId}";
        }
    }

    public static class AppConfigExtensions
    {
        public static AppConfig ToConfig(this string[] self)
        {
            if (self?.Length < 4) 
            {
                throw new InvalidOperationException($"This program should have 4 arguments. ex: xxx.exe /doc/examples.csv appid=xxxx appkey=xxxx versionid=0.1");
            }

            var path = self[0];
            var appId = self.FirstOrDefault(x => x.StartsWith("appid=", StringComparison.Ordinal)).Substring(6); // AppId
            var appKey = self.FirstOrDefault(x => x.StartsWith("appkey=", StringComparison.Ordinal)).Substring(7); // AppKey
            var versionId = self.FirstOrDefault(x => x.StartsWith("versionid=", StringComparison.Ordinal)).Substring(10); // VersionId

            if (string.IsNullOrEmpty(path))
            {
                throw new InvalidOperationException($"Path is empty - {path}");
            }
            if (string.IsNullOrEmpty(appId))
            {
                throw new InvalidOperationException($"AppId is empty - {appId}");
            }
            if (string.IsNullOrEmpty(appKey))
            {
                throw new InvalidOperationException($"AppKey is empty - {appKey}");
            }
            if (string.IsNullOrEmpty(versionId))
            {
                throw new InvalidOperationException($"VersionId is empty - {versionId}");
            }

            return new AppConfig(path, appId, appKey, versionId);
        }
    }
}
