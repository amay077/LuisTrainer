LuisTrainer
----

## これはなに？

Microsoft Cognitive services のひとつ LUIS(Language Understanding Intelligent Service) - [luis.ai](https://www.luis.ai/) に学習させるサンプルをバッチで送信するツールです。

## どこでうごかせる？

.NET Standard 2.0 で作ったコマンドラインアプリなので、

* macOS (with .NET Core)
* Windows (with .NET Framework)
* Linux (with .NET Core)

で動きます（後者２つは試してないけど）。

## 動かし方

macOS と Linux の場合は [.NET Core](https://dotnet.github.io/) をインストールして ``dotnet`` コマンドが使えるようにしてくださいね。

次のような感じでプログラム ``LuisTrainer.dll`` を実行します。渡すパラメータは 4つ です。

```
dotnet LuisTrainer.dll "path/to/examples.csv" appid=LUISのAppID appkey=LUISのAppKey versionid=LUISAppのバージョン

実際の例:
dotnet LuisTrainer.dll "./csv/examples.csv" appid=516xxxxx-xxxx-xxxx-xxxx-xzxxxxxxxxxxx appkey=5e38xxxxxxxxxxxxxxxxxxxxxxxxxxxx versionid=0.1
```

1. ``"path/to/examples.csv"`` - 学習させるサンプルデータが格納されたCSV。後述します。
2. ``appid=LUISのAppID`` - luis.ai でアプリケーションを作成すると割り振られるAppIDです。
3. ``appkey=LUISのAppKey`` - luis.ai でアプリケーションを作成すると割り振られるAppKeyです。
4. ``versionid=LUISAppのバージョン`` - luis.ai アプリケーションのバージョンです。 ``０．１`` とか。

## サンプルデータCSV仕様

|Title|Type|Description|
|:---|:---|:---|
|table|string|テーブルを表示したい|
