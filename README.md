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

次のような様式のCSVファイルを記述してください。文字コードは ``UTF-8`` 限定です。

|Text|Intent|Status|Update_ts|(EntityName1)|(EntityName2)|(EntityName…)|
|:---|:---|:---|:---|:---|:---|:---|
|学習させる文章|LUISのIntent名|0以外だと処理済としてスキップします|このツールが処理した日時|EntityName1として学習させたい単語|EntityName2として学習させたい単語|EntityNameとして学習させたい単語…|

``Text``, ``Intent``, ``Status``, ``Update_ts`` は必須列です。
それ以外の列は、LUISのEntity名に対応させた列を任意の数定義できます。

例えば、 LUIS で「今日の東京の天気を知りたい」という文章から GetWeather という Intent を得たいとき、 luis.ai には、次のような Intent と Entity を定義することになると思います。

* Intent - GetWeather : 天気を得るという「意図」
* Entity - Day : いつ？（例: 今日、明日、12月23日）
* Entity - City : 場所や地名（例: 東京、名古屋、ニューヨーク）

このような Intent と Entity 群の定義であるとき、サンプルデータCSV は次のように記述します。

|Text|Intent|Status|Update_ts|Day|City|
|:---|:---|:---|:---|:---|:---|
|今日の東京の天気を知りたい|GetWeather|0||今日|東京|
|明日の大阪の天気は？|GetWeather|0||明日|大阪|
|ニューヨークの5月12日の天気はどうですか？|GetWeather|0||5月12日|ニューヨーク|

この CSV を渡してプログラムを実行すると、 luis.ai の [REST API - examples - Add label](https://westus.dev.cognitive.microsoft.com/docs/services/5890b47c39e2bb17b84a55ff/operations/5890b47c39e2bb052c5b9c08) を実行して、それぞれの例文と Entity をラベル付けします。

学習自体やAPIのPublishは行わないので、Train コマンドは luis.ai の Web画面や REST API から行ってください。

プログラムの処理が終わると、結果を ``path/to/examples.csv.out`` に出力します。出力されたファイルには、``Status`` が ``1`` に、 ``Update_ts`` には処理日時が格納されます。

end of contents