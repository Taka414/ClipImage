# ClipImage

等間隔に並んだ画像を切り出すツール

## 対象環境

* Windows10
* .NET Framework 4.7.2

## 使い方

以下の順序でコマンドラインを入寮すると設定ファイルとキャラクターマップに従い切り取り対象画像の画像を分割し、切り取り対象画像のあるフォルダにファイル名と同じ名前のサブフォルダを作成してその中に結果を全て格納します。

```txt
> ClipImage.exe ${設定ファイル} ${キャラクターマップ} ${切り取り対象画像}
```

### 設定ファイル

切り取る対象画像を以下の通りと想定する。

![加工例](https://github.com/Taka414/ClipImage/blob/main/readme/001.png?raw=true "サンプル")

```txt
// Setting.json

{
   "margin-top": 20,   // (1) 画像の上部の余白(px)
   "margin-left": 20,  // (2) 画像の左側の余白(px)
   "char-margin-horizontal": 30, // 文字の横幅(px, 等幅)
   "char-margin-vertical": 30,   // 文字の縦幅(px, 等幅)
   "char-width": 70,   // 文字と文字の横の間隔(ox)
   "char-height": 70   // 文字と文字の縦の間隔(px)
}
```

### キャラクターマップ

テキスト形式であれば何でもよい

上記例であれば以下の通り

```txt
// CharMap.txt

1 2
A B
```

## 実行例

1. 以下のように実行する。

```
> ClipImage.exe Setting.json CharMap.txt Sample.png
```

2. Sampleというフォルダが作成され以下の4つの画像が作成される。

ファイル名は16進数のAscii文字 + .png で保存される。（ただしファイル名は Ascii 文字以外を想定していない）

### 31.png

![1](https://raw.githubusercontent.com/Taka414/ClipImage/main/readme/1.png "1")

### 32.png

![2](https://raw.githubusercontent.com/Taka414/ClipImage/main/readme/2.png "2")

### 41.png

![A](https://raw.githubusercontent.com/Taka414/ClipImage/main/readme/A.png "A")

### 42.png

![B](https://raw.githubusercontent.com/Taka414/ClipImage/main/readme/B.png "B")




