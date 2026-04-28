[操作マニュアル - TOP](./microsign_manual.md) 

## ゆっくりMovie Maker 4 を使ったアニメーションの作成

ゆっくりMovie Maker 4 (YMM4) を使って表示パネル向けのアニメーションを作成する方法です

[YMM4](https://manjubox.net/ymm4/)

YMM4 でアニメーションを作成する手順は説明しません。
あくまで YMM4 で表示パネル向けのアニメーションを作成するときの
設定などについて説明します

MicroSignの操作方法は[基本操作](./microsign_manual_basic.md)を参照してください


### 動画の設定

YMM4で一番最初に行う「動画の設定」です
メニューから「ファイル」→「動画の設定」を開きます

![動画の設定メニュー](./images//microsign_ymm4_001.png)

以下のように設定してください

|項目          |設定値  |
|--------------|-------|
|画面サイズ     |「カスタム」で表示パネルのドット数(128x32など)に設定してください|
|フレームレート  |10fps,15fps,20fp,30fpsあたりを選択してください。15fpsか20fpsがおすすめです|
|音声サンプリングレート|MicroSignでは音声は扱わないのでなんでもよいです|

![動画の設定](./images//microsign_ymm4_002.png)

これで動画の作成を行ってください


### キャラクターの配置

当リポジトリにて配布しているYMM4向け素材をダウンロードします

#### 素材の一例

|            | 32×32 | 48×32 |
|------------|--------|--------|
|ケレスちゃん|![32x32](../SampleAnimations//YMM4向け素材/yukkuri_32x32_ceres-chan.png)|![48x32](../SampleAnimations//YMM4向け素材/yukkuri_48x32_ceres-chan.png)|
|ソフトくん  |![32x32](../SampleAnimations//YMM4向け素材/yukkuri_32x32_soft-kun.png)|![48x32](../SampleAnimations//YMM4向け素材/yukkuri_48x32_soft-kun.png)|

__◆すべてのYMM4向け素材は **[こちら](../SampleAnimations/YMM4向け素材/)** で公開しています__

メニューから「ファイル」→「アイテムを追加」→「画像アイテム」を開きます

![アイテムを追加](./images//microsign_ymm4_011.png)

ファイル選択画面が開くので、ダウンロードした素材から画像ファイルを選択します

![ファイル選択](./images//microsign_ymm4_012.png)

以下のように画像が追加されます

![画像追加](./images//microsign_ymm4_013.png)

画像をドラッグし、配置を調整します

![画像移動](./images//microsign_ymm4_014.png)

必要に応じてエフェクトや図形を追加します

![エフェクト再生](./images//microsign_ymm4_015.png)

### 動画出力

YMM4で動画の出力を行う画面を開きます

![動画出力メニュー](./images//microsign_ymm4_003.png)


動画出力画面が開くので以下のように設定して出力してください

|項目          |設定値           |
|--------------|----------------|
|動画出力       |連番PNG + WAV出力|
|連番PNG出力    |ON              |
|WAV出力        |OFF(ONでもよいですが使用しません)|

その他は出力したい動画に合わせてください

![動画出力](./images//microsign_ymm4_004.png)

フォルダの選択画面が開くので、連番pngを出力するフォルダを選択します

![動画出力](./images//microsign_ymm4_005.png)

以下のように動画が出力されます

![動画出力](./images//microsign_ymm4_006.png)

### MicroSignへの取り込み

MicroSignを起動し、ドット数を表示パネルのドット数にします

![MicroSign起動](./images//microsign_ymm4_007.png)

標準表示期間をYMM4で設定したFPSの表示期間に設定します
今回は15 fps で作成したので「0.066」を設定します

![MicroSign表示期間](./images//microsign_ymm4_008.png)

YMM4で出力した連番PNGをMicroSignのタイムラインにドラッグ＆ドロップして
連番PNGをフレームとして登録します

![MicroSign表示期間](./images//microsign_ymm4_009.png)

以上でYMM4で作成したアニメーションをフレームとして登録できます

![MicroSign表示期間](./images//microsign_ymm4_010.png)
