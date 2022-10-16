using Newtonsoft.Json;

namespace Device.Tv.Livingroom
{
    internal class DeviceItem
    {
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; } = null!;

        [JsonProperty("connectionString")]
        public string ConnectionString { get; set; } = null!;

        [JsonProperty("deviceName")]
        public string DeviceName { get; set; } = null!;

        [JsonProperty("deviceType")]
        public string DeviceType { get; set; } = null!;

        [JsonProperty("location")]
        public string Location { get; set; } = null!;

        [JsonProperty("interval")]
        public int Interval { get; set; } = 10000;

        [JsonProperty("deviceState")]
        public bool DeviceState { get; set; } = false;
    }
}
