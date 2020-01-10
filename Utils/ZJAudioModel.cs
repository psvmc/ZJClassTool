using NAudio.CoreAudioApi;
using System.Collections.Generic;

namespace ZJClassTool.Utils
{
    public class ZJAudioModel : ZJNotifyModel
    {
        private string _name;

        public string name
        {
            get { return _name; }
            set
            {
                _name = value; OnPropertyChanged("name");
            }
        }

        public string id { get; set; }
        private bool _selected = true;

        public bool selected
        {
            get { return _selected; }
            set { _selected = value; OnPropertyChanged("selected"); }
        }

        public static List<ZJAudioModel> getAudioDevice()
        {
            List<ZJAudioModel> audioList = new List<ZJAudioModel>();
            var enumerator = new NAudio.CoreAudioApi.MMDeviceEnumerator();

            //允许你在某些状态下枚举渲染设备
            var endpoints = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
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