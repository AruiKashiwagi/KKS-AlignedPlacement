# Aligned Placement Helper

![Screenshot](https://github.com/AruiKashiwagi/KKS-AlignedPlacement/assets/52348271/87c279fb-9a2e-4474-89e5-2281558a43ce)

※日本語説明は下の方にあります

A small plugin for Koikatsu / Koikatsu Sunshine studio, which is useful to arrange multiple objects in a row, or in a circular form.
Also contains a feature that randomizes animation speed of selected objects.

# How to install
Make sure you've already installed the following plugins:

- [BepInEx](https://github.com/BepInEx/BepInEx) (I've tested with 5.4.22)
- [Illusion Modding API (KKAPI/KKSAPI)](https://github.com/IllusionMods/IllusionModdingAPI) (Tested with 1.38)
- [BepisPlugins](https://github.com/IllusionMods/BepisPlugins) (Tested with r19.5)

(Note that this plugin doesn't use BepisPlugins, but KKAPI has a dependency on ExtensibleSaveFormat which is the part of BepisPlugins)

Download the archive appropriate to your game version, then unzip it to your game folder.
KK_AlignedPlacement.dll (or KKS_AlignedPlacement.dll) will be extracted into the ./BepInEx/plugins/ folder.

# How to use
After a successful installation, you'll see the icon on the left side of the studio screen. Click it to toggle the plugin dialog.

This plugin contains three sepearate features; Linear Increment panel, Circular Placement panel and Animation Speed Randomizer panel.
For any panel, the basic usage is 1. Select multiple target objects, 2. Input values and then 3. Hit the corresponding "Go" button.
v
## Linear Increment panel
Arranges the selected objects in a row by adding an incremental value to the position of each object. In a similar way, you can also add an incremental value to the rotation angle of each object.

The position increment and the rotation increment are two separate features. Fill the input fields you want to change (X, Y and Z values respectively) and hit the corresponding "Go" button.

Note that the first selected object is always treated as a "reference", and its position and angle never change. So you must select at least two objects before hitting "Go".

## Circular Placement panel
Arranges the selected objects on a circle, or on an arc.
Check the following options and hit the "Go" button to run. You must select at least two objects to run this feature.

### Circle axes
Determines the plane where you place objects.

Basically, if you want to put characters on the ground, choose "ZX (Horizontal)" which is default.
"XY (Vertical)" and "ZY (Vertical)" are used when you want to make a wheel-like structure.

### Use first object as center point
If checked, the first object is treated as a center point, and the rest of the objects will be arranged around them. The position and the angle of the first selected object stay unchanged.

If unchecked, all the selected objects are placed on the edge of a circle, as if there were a virtual center point inside them. In this case, the position of the center point is determined by "Radius" and "Placement start angle" settings described below, and also the first object's position.
The position of the first selected objects stays unchanged, but its angle may change depending on the "Rotate each object" checkbox described below.

### Radius
Determines the radius of a placement circle.

### Placement start angle / End angle
Determines the angles where you start and stop to put objects on a circle.
For example, if you entered "0, 180" degrees, selected objects will be placed on a semicircle. "180, 0" also gives a similar result, but in an opposite order. The "0, 90" setting makes a quadrant. You can also enter a negative value, or a value larger than 360.

### End angle is inclusive
If checked, the last selected object is placed right above the point of "End angle". Recommended when you want to arrange objects on an arc.

If unchecked, the last selected object is placed before reaching the "End angle", as if there were an extra object to put. As a result, all the objects will be arranged more closely than when the checkbox is checked.
Recommended when you want to arrange objects in 0 to 360 degrees and make a full circle.

### Rotate each object
If checked, each selected object rotates by an incremental amount.
It basically works in the same way as the Linear Increment panel, but this checkbox is much convenient because it auto-calculates the amount of degree based on how many objects you selected and what values you input in "Placement start angle / End angle" fields.

If unchecked, the rotation angles stay unchanged for all the selected objects.

#### Rotation axis
Valid only when "Rotate each object" is checked.
Determines which axis is used to rotate objects. When ZX is chosen as "Circle axes", Rotation axis should be Y to let the objects look at certain direction on a horizontal plane.
For the axes not specified by this setting, the angles of the first object will be applied.

#### Rotation offset angle
Valid only when "Rotate each object" is checked.
Determines the rotation angle of the first object on a circle. Subsequent objects also follows the same manner. With a proper setting, you can let the objects look at the center of a circle, or the opposite, or let them look at the next one's back. You'll see once you try it.

## Animation Speed Randomizer panel
Randomizes the animation speed of selected objects between the minimum and the maximum speed you specify.
Not really relevant to the linear placement feature, but I found it's convenient if you can add a slight difference to each object while they are standing in a line.

This is the only feature that works even when just one object is selected.
Note that the minimum value must be smaller than maximum value, otherwise this feature does nothing.

# How to build
There's a Visual Studio 2022 soultion file in the repository.

Before building the plugin, you must resolve the DLL dependencies. I believe most of dependencies can be fulfilled through NuGet, but apparently KKAPI.dll and KKSAPI.dll are the exception. You have to copy them to the directory ./lib/ next to the .sln file manually.

After all the dependencies are resolved, click "Build Solution" from Visual Studio 2022's menu.
Note that Koikatsu requires .NET Framework 3.5, and Koikatsu Sunshine requires 4.6.

# Notice
I don't use Discord so often. If you found any problem, open an issue on GitHub or contact me on X (Twitter).

https://twitter.com/Arui_Kashiwagi

This plugin is free software. You can redistribute or modify it without my permission.

# 日本語説明
「コイカツ！」もしくは「コイカツ！サンシャイン」のスタジオモードで、複数のオブジェクトを規則的に並べたい時に役立つプラグインです。
線形配置と円形配置の2種類の機能を搭載しています。おまけでアニメ速度ランダマイズ機能もついています。

# インストール方法
前提として次のプラグインがインストールされている必要があります。これらのインストール方法については割愛します。
- [BepInEx](https://github.com/BepInEx/BepInEx)（5.4.22で確認）
- [Illusion Modding API (KKAPI/KKSAPI)](https://github.com/IllusionMods/IllusionModdingAPI)（1.38で確認）
- [BepisPlugins](https://github.com/IllusionMods/BepisPlugins)（r19.5で確認）※

※本プラグイン自体はBepisPluginsの機能を使っていないのですが、KKAPIがBepisPluginsの中のExtensibleSaveFormatに依存しているため必要となります。

Releaseページに掲載したzipファイルのうち、お持ちのゲームに合う方をダウンロードしてゲームディレクトリに展開してください。
zip内のディレクトリ構造が維持されていれば、./BepInEx/plugins/ ディレクトリにDLLファイルが展開されます。

# 使い方
正常にインストールされていれば、スタジオ起動後の画面左端のツールバーにアイコンが1個追加されます。このアイコンをクリックするとダイアログが開閉します。

ダイアログ内は「Linear Increment（線形配置）」「Circular Placement（円形配置）」「Animation Speed Randomizer（アニメ速度ランダマイズ）の3つの独立した機能に分かれています。
すべての機能は「オブジェクト（通常は2個以上）を選択し、ダイアログに数値を入力し、対応する"Go"ボタンを押す」という手順で使用します。

## Linear Increment（線形配置）パネル
選択したオブジェクトの座標を「ひとつ前のオブジェクトの座標に一定の値を加算したもの」に変更します。これにより複数のオブジェクトを直線上に等間隔で並べることができます。

また、回転角度に対して同様の計算を適用することで、複数オブジェクトの角度を一定量ずつ回転させることもできます。
座標（Position）変更と回転角度（Rotation）変更は独立した機能ですので、いずれか使用したい方の数値欄にX・Y・Zの加算値を入力し、「Go」ボタンを押してください。

1個目のオブジェクトは基準点として扱われるため、座標・角度は変化しません。したがって2個以上のオブジェクトを選択していないとこの機能は意味を持ちません。

## Circular Placement（円形配置）パネル
選択した複数のオブジェクトを円形に配置します。
こちらも2個以上のオブジェクトを選択していないと動作しません。

### Circle axes（平面軸）
配置の基準となる円をどの平面上に描くかを選択します。
キャラを地平面の上に立たせたいときは「ZX (Horizontal)」を選ぶ、とだけ覚えておけば大抵の場合は事足りるかと思います。
水車のような縦向きの輪を作りたい時はXYかZYを選んでください。

### Use first object as center point（1個目のオブジェクトを中心点とする）
ONの場合、1個目のオブジェクトを中心点とみなし、2個目以降のオブジェクトはそれを取り囲むように配置されます。中心点となるオブジェクトの位置・角度は一切変化しません。

OFFの場合、すべての選択オブジェクトは円周上に置かれるものと想定し、1個目のオブジェクトの位置をもとに仮想的な中心点を算出します。中心点の座標は後述のRadiusおよびPlacement start angleの値にも影響を受けます。
こちらの場合も1個目のオブジェクトの座標は変化しませんが、回転角度については後述するRotate each objectチェックボックスがONになっていれば変化する可能性があります。

### Radius（半径）
配置の基準となる円の半径を設定します。

### Placement start angle（配置開始角度） - End angle（終了角度）
オブジェクトを円周上に配置する際の開始角度と終了角度を設定します。
たとえば「0, 180」を入力すれば半円を描いて配置されます。「180, 0」の場合も同じ半円状となりますが、オブジェクトは逆順で配置されます。そのほか負の値や360度を超える値も入力可能です。

### End angle is inclusive（配置に終了角度を含む）
ONの場合、最後のオブジェクトはEnd angleが表す位置の真上に配置されます。半円や扇型に配置する場合にはこちらの設定で使用することをおすすめします。

OFFの場合、最後のオブジェクトはEnd angleに到達するよりも一歩手前の位置に配置されます。そのためオブジェクト同士の間隔はONの場合よりも詰まった配置になります（あたかも配置するオブジェクトが一個多い時のような計算式になります）。
Placement start angle / End angleに「0, 360」を入力して完全な円形に配置したい場合、ON設定のままだと最初と最後のオブジェクトの位置が重なってしまうので、OFFで使用することをおすすめします。

### Rotate each object（各オブジェクトを回転させる）
ONにすると、各オブジェクトをひとつ前のオブジェクトに対して一定の角度ずつ回転させます。
これはLinear Incrementパネルで角度調整を行うのとほぼ同じ機能ですが、選択オブジェクトの個数や配置範囲に応じて増分を自動計算してくれるというメリットがあります。

OFFの場合、選択オブジェクトの回転角度は一切変化しません。

#### Rotation axis（回転軸）
「Rotate each object」チェックボックスがONの時のみ意味を持ちます。
各オブジェクトを回転させる際の軸をX, Y, Zからいずれか一つ選びます。Circle axesが「ZX」のとき、こちらで「Y」を選ぶことで地平面上での向きの変更となります。
Rotation axisで指定していない軸については、原則として1個目のオブジェクトと同じ角度の値がコピーされます。

#### Rotation offset angle（回転角度オフセット）
「Rotate each object」チェックボックスがONの時のみ意味を持ちます。
各オブジェクトを回転させるにあたり、1個目のオブジェクトの回転角度をここで入力した角度に設定します。2個目以降のオブジェクトの角度はこの値に増分を加えたものになります。
適切に設定することで、全てのキャラに円の中心を向かせたり、逆に外を向かせたり、あるいは隣のキャラの背中を向かせたりということができます。Placement start angleの値によっても効果が変わりますので各自調整してください。

## Animation Speed Randomizer（アニメ速度ランダマイザ）パネル
選択したオブジェクトのアニメーション速度を一定の範囲内でランダムに変化させます。
等間隔配置とは直接関係のない機能ですが、一列に並ばせたキャラのモーションにゆらぎを持たせるのに便利なのでつけました。
この機能のみ、選択オブジェクトが1個だけであっても動作します。

「min.」「Max.」にそれぞれ最小・最大速度を入力して「Go」を押せば、その範囲内で速度がランダムに変化します。
min. > Maxとなる数値を入力した場合はエラーとなり、Goボタンを押しても何もしません。

# ビルド方法
Visual Studio 2022用ソリューションファイルを同梱しているので、それを開いてください。

ビルドの前にあらかじめ関連DLLファイル一式を用意する必要がありますが、その大半はNuGet経由で取得できるようになっていると思います。たぶんソリューションエクスプローラーの各プロジェクト（KK, KKS）上で右クリックして「すべての依存関係ツリーの読み込み」あたりを選べば読み込まれると思うのでやってみてください。だめだったら「NuGetパッケージの管理」からどうにかしてください。

ただし KKAPI.dll および KKSAPI.dll だけはどうやら自動解決できないようなので、手動でファイルを追加する必要があります。.slnファイルと同じ階層の lib/ ディレクトリの中にこれらのdllファイルをコピーしておいてください。
もしくはソリューションエクスプローラーの「依存関係」右クリック→「アセンブリ参照の追加」→「参照」から任意のパスを直接追加することもできます。

依存関係が解決できたら「ビルド」→「ソリューションのビルド」を選べばビルドが実行されます。
コイカツ版のビルドには.NET 3.5、サンシャイン版のビルドには.NET 4.6が必要です。

# 諸注意
何か問題があればGitHubのIssuesに登録するか、もしくはX（Twitter）経由でご連絡ください。
（作者はDiscordはほとんど見ていません）

https://twitter.com/Arui_Kashiwagi

再配布・改変に制約はありません。各自の責任において自由に行ってください。