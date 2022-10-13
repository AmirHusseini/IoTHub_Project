using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApp.MVVM.Models
{
    public class WeatherResponse
    {
        public int Temperature { get; set; }

        public int Humidity { get; set; }

        private string? _weatherCondition;
        public string? WeatherCondition
        {
            get => _weatherCondition;
            set
            {
                switch (value?.ToLower())
                {
                    case "clear":
                        _weatherCondition = "\ue28f";
                        break;

                    case "clouds":
                        _weatherCondition = "\uf0c2";
                        break;

                    case "rain":
                        _weatherCondition = "\uf740";
                        break;

                    case "thunderstorm":
                        _weatherCondition = "\uf76c";
                        break;

                    default:
                        _weatherCondition = "\uf6c4";
                        break;
                }
            }
        }
    }
}
