using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Management;
using Newtonsoft.Json;

namespace ZJClassTool.Utils
{
    class ZJAudioModel
    {
        public string name { get; set; }
        public string id { get; set; }

        public static List<ZJAudioModel> getAudioDevice()
        {
            List<ZJAudioModel> audioList = new List<ZJAudioModel>();
            var enumerator = new NAudio.CoreAudioApi.MMDeviceEnumerator();

            //允许你在某些状态下枚举渲染设备
            var endpoints = enumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Unplugged | DeviceState.Active);
            foreach (var endpoint in endpoints)
            {
                ZJAudioModel audioModel = new ZJAudioModel();
                audioModel.name = endpoint.FriendlyName;
                audioModel.id = endpoint.ID;
                audioList.Add(audioModel);
            }
            return audioList;
        }
    }
}
