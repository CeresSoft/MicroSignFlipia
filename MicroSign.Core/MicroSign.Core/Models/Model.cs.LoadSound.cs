using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MicroSign.Core.Models
{
    partial class Model
    {
        /// <summary>
        /// サンド読込結果
        /// </summary>
        public struct LoadSoundResult
        {
            /// <summary>
            /// 成功フラグ
            /// </summary>
            public readonly bool IsSuccess;

            /// <summary>
            /// メッセージ
            /// </summary>
            public readonly string? Message;

            /// <summary>
            /// サウンドフォーマット文字列
            /// </summary>
            public readonly string? FormatText;

            /// <summary>
            /// サウンドデータ(PCMの上位8ビットにしたもの)
            /// </summary>
            public readonly byte[]? Pcm8Data;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="isSuccess"></param>
            /// <param name="message"></param>
            /// <param name="formatText"></param>
            /// <param name="pcm8Data"></param>
            private LoadSoundResult(bool isSuccess, string? message, string? formatText, byte[]? pcm8Data)
            {
                this.IsSuccess = isSuccess;
                this.Message = message;
                this.FormatText = formatText;
                this.Pcm8Data = pcm8Data;
            }

            /// <summary>
            /// 失敗
            /// </summary>
            /// <param name="message"></param>
            /// <returns></returns>
            public static LoadSoundResult Failed(string message)
            {
                LoadSoundResult result = new LoadSoundResult(false, message, null, null);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <param name="formatText">サウンドファイルフォーマット文字列</param>
            /// <param name="pcm8Data">PCMの上位8ビットにしたもの</param>
            /// <returns></returns>
            public static LoadSoundResult Success(string formatText, byte[] pcm8Data)
            {
                LoadSoundResult result = new LoadSoundResult(true, null, formatText, pcm8Data);
                return result;
            }
        }

        /// <summary>
        /// 読込パス
        /// </summary>
        /// <param name="path">サウンドファイルパス</param>
        /// <returns></returns>
        public LoadSoundResult LoadSound(string path)
        {
            //サウンドファイルパス有効判定
            {
                bool isNull = string.IsNullOrEmpty(path);
                if (isNull)
                {
                    //無効の場合は失敗
                    return LoadSoundResult.Failed("サウンドファイルパスが無効です");
                }
                else
                {
                    //有効の場合は処理続行
                }
            }

            //ファイルが存在するか判定
            {
                bool isExists = System.IO.File.Exists(path);
                if (isExists)
                {
                    //存在する場合は処理続行
                }
                else
                {
                    //存在しない場合は終了
                    return LoadSoundResult.Failed("サウンドファイルが存在しません");
                }
            }

            //サウンドファイル読込
            // >> Copilot曰くサウンドファイルのフォーマットを得るには
            // >> MediaFoundationReader()を使う必要があるとのこと
            string formatText = string.Empty;
            List<byte> pcm8 = new List<byte>();
            try
            {
                LOGGER.Debug($"サウンドファイル読込 path='{path}'");
                using (NAudio.Wave.MediaFoundationReader? reader = new NAudio.Wave.MediaFoundationReader(path))
                {
                    //サウンドファイルのフォーマットを取得
                    {
                        //読込できた場合
                        NAudio.Wave.WaveFormat format = reader.WaveFormat;
                        TimeSpan ts = reader.TotalTime;
                        //秒
                        int tsSecond = (int)ts.Seconds;
                        //トータル分
                        int tsTotalMinute = (int)ts.TotalMinutes;

                        //文字列にする
                        formatText = $"{format} 長さ {tsTotalMinute} 分 {tsSecond:00} 秒";
                    }

                    //フォーマット変換
                    // >> 44,1KHz, 16bit 2Channelを設定
                    NAudio.Wave.WaveFormat targetFormat = new NAudio.Wave.WaveFormat(
                        MicroSign.Core.MicroSignConsts.Sounds.PcmFormat.SampleRate,     // サンプルレート
                        MicroSign.Core.MicroSignConsts.Sounds.PcmFormat.BitsPerSample,  // ビット数
                        MicroSign.Core.MicroSignConsts.Sounds.PcmFormat.Channels        // ステレオ
                    );

                    //PCM変換して上位8ビットのデータ保持する
                    using (NAudio.Wave.MediaFoundationResampler sampler = new NAudio.Wave.MediaFoundationResampler(reader, targetFormat))
                    {
                        //再サンプル読込バッファ生成
                        byte[] readBuff = new byte[MicroSign.Core.MicroSignConsts.Sounds.ResampleReadBuffSize];

                        //再サンプルしてPCMの上位8bitだけ得る

                        int readSize = sampler.Read(readBuff, CommonConsts.Index.First, readBuff.Length);
                        while (CommonConsts.Collection.Empty < readSize)
                        {
                            //uint16で取得
                            // >> 2チャンネルなのでLRLR...の順に得られるので、そのままの順番で設定する
                            ReadOnlySpan<byte> readBuffSpan = new ReadOnlySpan<byte>(readBuff, CommonConsts.Index.First, readSize);
                            ReadOnlySpan<UInt16> pcmBuff = MemoryMarshal.Cast<byte, UInt16>(readBuffSpan);
                            foreach (UInt16 pcm in pcmBuff)
                            {
                                //PCM(=16bit)を上位8ビットだけにする
                                int v = pcm >> CommonConsts.BitCount.BYTE;

                                //データを追加
                                pcm8.Add((byte)v);
                            }

                            //続きを読込
                            readSize = sampler.Read(readBuff, CommonConsts.Index.First, readBuff.Length);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //例外は握りつぶす
                LOGGER.WarnEx($"サウンドファイルの読込で例外発生 path='{path}'", ex);
                return LoadSoundResult.Failed($"サウンドファイル読込失敗 ({ex})");
            }

            //PCM8データが存在するか判定
            {
                int n = pcm8.Count;
                if(CommonConsts.Collection.Empty < n)
                {
                    //データが存在する場合は処理続行
                }
                else
                {
                    //データが存在しない場合は失敗にする
                    return LoadSoundResult.Failed("サウンドが空です");
                }
            }

            //ここまで来たら成功で終了
            {
                byte[] pcm8Data = pcm8.ToArray();
                return LoadSoundResult.Success(formatText, pcm8Data);
            }
        }
    }
}
