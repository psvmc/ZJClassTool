using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;

namespace ZJClassTool.Utils
{
    public class ZJGenericTypeConverter : IValueConverter
    {
        /// <summary>
        /// 正向键值对字典
        /// </summary>
        private Dictionary<string, string> Alias { get; set; }

        /// <summary>
        /// 反向键值对字典
        /// </summary>
        private Dictionary<string, string> BackAlias { get; set; }

        private string aliasStrTemp = "";

        /// <summary>
        /// 解析转换规则
        /// </summary>
        /// <param name="aliasStr">规则字符串</param>
        private void ParseAliasByStr(string aliasStr)
        {
            if (aliasStrTemp == aliasStr)
                return;
            aliasStrTemp = aliasStr;
            Alias = new Dictionary<string, string>();
            BackAlias = new Dictionary<string, string>();

            string content = aliasStr;

            Alias = new Dictionary<string, string>();
            string[] arr1 = content.Split('|');
            foreach (string item in arr1)
            {
                string[] arr2 = item.Split(':');
                var key = arr2[0];
                if (key == "true")
                {
                    key = "True";
                }
                else if (key == "false")
                {
                    key = "False";
                }
                var value = arr2[1];
                if (key != "other")
                {
                    BackAlias.Add(value, key);
                }

                Alias.Add(key, value);
            }
        }

        private object ConvertCommon(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture, bool isBack)
        {
            if (value == null || string.IsNullOrWhiteSpace(parameter.ToString()))
                return null;
            object ret = value;//如果没有匹配返回传入的值

            ParseAliasByStr(parameter.ToString());
            Dictionary<string, string> alias;
            if (isBack)
                alias = BackAlias;
            else
                alias = Alias;

            //绑定的值
            string bindingValue = value.ToString();

            if (alias.ContainsKey(bindingValue))
                ret = StringToTargetType(alias[bindingValue], targetType);
            else if (alias.ContainsKey("other"))
                ret = StringToTargetType(alias["other"], targetType);
            else if (alias.ContainsKey("else"))
                ret = StringToTargetType(alias["else"], targetType);

            return ret;
        }

        /// <summary>
        /// 字符串转换成目标类型，如需添加一个目标类型只需在该方法中添加一个类型判断之后转换
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        private object StringToTargetType(string strValue, Type targetType)
        {
            object ret = null;
            //目标类型 string
            if (targetType == typeof(string) || targetType == typeof(char))
            {
                ret = strValue;
            }
            //目标类型 char
            if (targetType == typeof(char))
            {
                if (strValue.Length == 1)
                    ret = strValue;
            }
            //目标类型 int
            if (targetType == typeof(int))
            {
                int temp;
                if (int.TryParse(strValue, out temp))
                    ret = temp;
                else
                    ret = 0;
            }
            //目标类型 double
            if (targetType == typeof(double))
            {
                double temp;
                if (double.TryParse(strValue, out temp))
                    ret = temp;
                else
                    ret = 0;
            }
            //目标类型 float
            if (targetType == typeof(float))
            {
                float temp;
                if (float.TryParse(strValue, out temp))
                    ret = temp;
                else
                    ret = 0;
            }
            //目标类型 decimal
            if (targetType == typeof(decimal))
            {
                decimal temp;
                if (decimal.TryParse(strValue, out temp))
                    ret = temp;
                else
                    ret = 0;
            }
            //目标类型 bool? bool
            if (targetType == typeof(bool?) || targetType == typeof(bool))
            {
                bool temp;
                if (bool.TryParse(strValue, out temp))
                    ret = temp;
                else
                    ret = false;
            }

            //目标类型 Visibility
            if (targetType == typeof(Visibility))
            {
                switch (strValue.ToLower())
                {
                    case "collapsed":
                        ret = Visibility.Collapsed;
                        break;

                    case "hidden":
                        ret = Visibility.Hidden;
                        break;

                    case "visible":
                        ret = Visibility.Visible;
                        break;

                    default:
                        ret = Visibility.Visible;
                        break;
                }
            }
            return ret;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ConvertCommon(value, targetType, parameter, culture, false);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ConvertCommon(value, targetType, parameter, culture, true);
        }
    }
}