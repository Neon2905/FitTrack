using FitTrack.Core;
using FitTrack.Database;
using Attribute = FitTrack.Database.Entities.RegisteredDevice;

namespace FitTrack.Model
{
    class RegisteredDevice : ServerBase
    {
        public string Id
        {
            get => Get(Attribute.Id);
        }

        public string HostName
        {
            get => Get(Attribute.HostName);
        }

        public string OSInformation
        {
            get => Get(Attribute.OSInformation);
        }

        private string Get(Attribute wanted_attribute) => GetById(wanted_attribute, this.Id).ToString();

        #region Create
        public static void Create()
        {
            var Id = GenerateNewDeviceId();
            var HostName = SystemInfo.DeviceName;
            var OSInformation = SystemInfo.OSInformation;
            ExecuteNonQuery($"INSERT INTO RegisteredDevice(Id,HostName,OSInformation) VALUES(" +
                                $"'{Id}', '{HostName}', '{OSInformation}');");
        }
        #endregion
    }
}
