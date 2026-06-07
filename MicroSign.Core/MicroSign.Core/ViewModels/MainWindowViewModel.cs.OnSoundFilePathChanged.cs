using System;
using static MicroSign.Core.Models.Model;

namespace MicroSign.Core.ViewModels
{
    partial class MainWindowViewModel
    {
        /// <summary>
        /// サウンドファイルパス変更処理
        /// </summary>
        protected void OnSoundFilePathChanged()
        {
            try
            {
                //サウンドファイルパスを取得
                string? path = this.SoundFilePath;
                {
                    bool isNull = string.IsNullOrEmpty(path);
                    if (isNull)
                    {
                        //無効の場合は何もせずに終了
                        // >> ステータスを初期値に設定する
                        this.SoundFileStatus = MainWindowViewModel.InitializeValues.SoundFileStatus;
                        // >> PCM8データを初期値にする
                        this.SoundPcm8Data = MainWindowViewModel.InitializeValues.SoundPcm8Data;
                        return;

                    }
                    else
                    {
                        //有効の場合は処理続行
                    }
                }

                //サウンドファイル読込
                {
                    LoadSoundResult ret = this.Model.LoadSound(path!);
                    bool isSuccess = ret.IsSuccess;
                    if (isSuccess)
                    {
                        //成功の場合
                        // >> フォーマット文字列をステータスに設定
                        this.SoundFileStatus = ret.FormatText;
                        // >> PCM8データを設定
                        this.SoundPcm8Data = ret.Pcm8Data;
                    }
                    else
                    {
                        //失敗の場合
                        // >> ステータスにエラーメッセージを設定
                        this.SoundFileStatus = ret.Message;
                        // >> PCM8データを初期値にする
                        this.SoundPcm8Data = MainWindowViewModel.InitializeValues.SoundPcm8Data;
                    }
                }
           }
            catch (Exception ex)
            {
                //例外は握りつぶす
                LOGGER.WarnEx("サウンドファイルパス変更処理で例外発生", ex);
                // >> ステータスを設定する
                this.SoundFileStatus = $"例外発生 ({ex})";
            }
        }
    }
}
