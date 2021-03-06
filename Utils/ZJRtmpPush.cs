﻿using System;
using System.Diagnostics;
using System.IO;

namespace ZJClassTool.Utils
{
    internal class ZJRtmpPush
    {
        // ffmpeg进程
        public static Process p = new Process();

        // ffmpeg.exe实体文件路径
        private static string ffmpegPath = AppDomain.CurrentDomain.BaseDirectory + "ffmpeg\\ffmpeg.exe";

        /// <summary>
        /// 功能: 开始录制
        /// </summary>
        public static void Start(string audioDevice, string outFilePath)
        {
            if (File.Exists(outFilePath))
            {
                File.Delete(outFilePath);
            }

            /*转码，视频录制设备：gdigrab；录制对象：桌面；
             * 音频录制方式：dshow；
             * 视频编码格式：h.264；*/
            ProcessStartInfo startInfo = new ProcessStartInfo(ffmpegPath);
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            var parastr = string.Format("-f gdigrab -framerate 15 -i desktop -f dshow -i audio=\"{0}\" -vcodec libx264 -preset:v ultrafast -tune:v zerolatency -acodec libmp3lame \"{1}\"", audioDevice, outFilePath);
            startInfo.Arguments = parastr;
            p.StartInfo = startInfo;

            p.Start();
        }

        /// <summary>
        /// 功能: 开始推流
        /// </summary>
        public static void StartPush(string audioDevice, string pushUrl)
        {
            /*转码，视频录制设备：gdigrab；录制对象：桌面；
             * 音频录制方式：dshow；
             * 视频编码格式：h.264；*/
            ProcessStartInfo startInfo = new ProcessStartInfo(ffmpegPath);
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            var parastr = string.Format("-f gdigrab -framerate 15 -i desktop -f dshow -i audio=\"{0}\" -filter:v scale=w=trunc(oh*a/2)*2:h=720 -vcodec libx264 -preset:v ultrafast -acodec libmp3lame -maxrate 1000k -pix_fmt yuv422p -f flv \"{1}\"", audioDevice, pushUrl);
            startInfo.Arguments = parastr;
            p.StartInfo = startInfo;
            // Console.WriteLine("parastr:" + parastr);
            p.Start();
        }

        /// <summary>
        /// 功能: 停止录制
        /// </summary>
        public static void Stop()
        {
            p.Kill();
            p.StartInfo.Arguments = "";
        }
    }
}