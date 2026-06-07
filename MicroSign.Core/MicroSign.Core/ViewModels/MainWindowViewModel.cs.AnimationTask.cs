using System;
using System.Threading;

namespace MicroSign.Core.ViewModels
{
    partial class MainWindowViewModel
    {

        /// <summary>
        /// アニメーションタスク
        /// </summary>
        /// <param name="token"></param>
        /// <param name="animationImageItems"></param>
        /// <param name="startAnimationItem"></param>
        private void AnimationTask(CancellationToken token, AnimationImageItemCollection animationImageItems, AnimationImageItem startAnimationItem)
        {
            try
            {
                LOGGER.Debug("アニメーションタスク開始");

                //アニメーション画像コレクション有効判定
                if (animationImageItems == null)
                {
                    //無効の場合は即終了
                    LOGGER.Warn("アニメーション画像コレクションが無効");
                    return;
                }
                else
                {
                    //有効の場合は処理続行
                    LOGGER.Debug("アニメーション画像コレクション有効");
                }

                //アニメーション画像コレクション数取得
                int animationCount = animationImageItems.Count;
                if (CommonConsts.Collection.Empty < animationCount)
                {
                    //有効の場合は処理続行
                    LOGGER.Debug($"アニメーション画像数={animationCount}");
                }
                else
                {
                    //無効の場合は終了
                    LOGGER.Warn($"アニメーション画像無し={animationCount}");
                    return;
                }

                //開始アニメーション有効判定
                if (startAnimationItem == null)
                {
                    //無効の場合は即終了
                    LOGGER.Warn("開始アニメーション画像が無効");
                    return;
                }
                else
                {
                    //有効の場合は処理続行
                    LOGGER.Debug("開始アニメーション画像有効");
                }

                //インデックスを取得
                int index = animationImageItems.IndexOf(startAnimationItem);
                if (index < CommonConsts.Index.First)
                {
                    //無効の場合は即終了
                    LOGGER.Warn("開始アニメーション画像インデックス無効");
                    return;
                }
                else
                {
                    //有効の場合は処理続行
                    LOGGER.Debug($"開始アニメーション画像インデックス有効={index}");
                }

                //アニメーションを選択
                // >> 開始アニメーションだけ選択する
                this.SetSelectAnimationImage(startAnimationItem);

                //キャンセルハンドル取得
                WaitHandle cancelHandle = token.WaitHandle;

                using (WaitableTimes.WaitableTimer timer = new WaitableTimes.WaitableTimer())
                {
                    //カレントアニメーション画像
                    AnimationImageItem? currentItem = startAnimationItem;

                    //最初のタイマー設定
                    {
                        double period = currentItem.DisplayPeriod;
                        TimeSpan ts = TimeSpan.FromSeconds(period);
                        long ticks = ts.Ticks;
                        timer.SetSignalInterval(ticks);
                    }

                    //タイマー開始
                    timer.Start();

                    //ウエイトハンドル生成
                    WaitHandle[] handles =
                    {
                        cancelHandle,
                        timer,
                    };

                    //ループ処理
                    bool isLoop = true;
                    while(isLoop)
                    {
                        //イベント待ち
                        int id = WaitHandle.WaitAny(handles);
                        switch(id)
                        {
                            case CommonConsts.Index.First:
                                //キャンセルの場合
                                LOGGER.Debug("アニメーションタスクキャンセル検出");
                                isLoop = false;
                                break;

                            default:
                                //それ以外はアニメーション更新
                                {
                                    //インデックス更新
                                    index += CommonConsts.Index.Step;
                                    if (index < animationCount)
                                    {
                                        //範囲内の場合はそのまま
                                    }
                                    else
                                    {
                                        //範囲外になったら先頭に戻す
                                        index = CommonConsts.Index.First;
                                    }

                                    //アニメーション選択
                                    AnimationImageItem? item = animationImageItems[index];
                                    if(item == null)
                                    {
                                        //アニメーションが無効の場合は何もしない
                                    }
                                    else
                                    {
                                        //アニメーションが有効の場合選択する
                                        item.IsSelected = CommonUtils.Not(AnimationImageItem.InitializeValues.IsSelected);

                                        //ListView_SelectionChangedが発生しないので直接画像を入替
                                        this.LoadImage = item.Image;

                                        //表示時間を取得
                                        double period = item.DisplayPeriod;
                                        TimeSpan ts = TimeSpan.FromSeconds(period);
                                        long ticks = ts.Ticks;

                                        //リセットする
                                        timer.Reset(ticks);
                                    }

                                    //現在のアニメーションの選択を解除
                                    if (currentItem == null)
                                    {
                                        //アニメーションが無効の場合は何もしない
                                    }
                                    else
                                    {
                                        //アニメーションが有効の場合選択解除する
                                        currentItem.IsSelected = AnimationImageItem.InitializeValues.IsSelected;
                                    }

                                    //選択を変更する
                                    currentItem = item;
                                }

                                break;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                //例外は握りつぶす
                LOGGER.WarnEx("アニメーションタスクで例外発生", ex);
            }
            finally
            {
                LOGGER.Debug("アニメーションタスク終了");
            }
        }


    }
}
