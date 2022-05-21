using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace PrimalEditor.Utiltiies
{
    public static class Serializer
    {
        /* used to write to file */
        public static void ToFile<T>(T instance, string path)
        {
            try
            {
                using var fs = new FileStream(path, FileMode.Create);
                var Serializer = new DataContractSerializer(typeof(T));
                Serializer.WriteObject(fs, instance);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Log(MessageType.Error, $"Failed to serialize {instance} to {path}");
                throw;
            }
        }

        internal static T FromFile<T>(string path)
        {
            try
            {
                using var fs = new FileStream(path, FileMode.Open);
                var Serializer = new DataContractSerializer(typeof(T));
                T instance = (T)Serializer.ReadObject(fs);
                return instance;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Log(MessageType.Error, $"Failed to derserialize {path}");
                throw;
            }
        }
    }
}
