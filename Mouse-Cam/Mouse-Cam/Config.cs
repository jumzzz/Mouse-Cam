using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Hand_Virtual_Mouse
{
    public class ConfigImgProcVirtualMouse 
    {
        public int Frequency { get; set; }
        public int Erosion { get; set; }
        public int Dilation { get; set; }

        public ConfigImgProcVirtualMouse()
        {
            Frequency = 50;
            Erosion = 0;
            Dilation = 1;
        }

        public void SerializeData(string path)
        {
            XmlSerializer serialize = new XmlSerializer(this.GetType());
            FileStream file = File.Create(path);
            serialize.Serialize(file, this);
        }

        public static ConfigImgProcVirtualMouse DeserializeObject(string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                XmlSerializer serializer = new
                XmlSerializer(typeof(ConfigImgProcVirtualMouse));

                ConfigImgProcVirtualMouse vmfc = (ConfigImgProcVirtualMouse)serializer.Deserialize(fileStream);

                return vmfc;
            }

        }
    }

    public class ConfigFileVirtualMouse
    {
        public string FilePathHSV { get; set; }
        public string FilePathPRM { get; set; }
        public string FilePathOPN { get; set; }
        public string FilePathLFT { get; set; }
        public string FilePathRGT { get; set; }
        public string FilePathHLT { get; set; }
        public string FilePathSDN { get; set; }
        public string FilePathSUP { get; set; }
        public string FileHistSkinHSV { get; set; }
        public string FileHistNonSkinHSV { get; set; }
        
        public ConfigFileVirtualMouse() 
        {
        }

        public void SerializeData(string path) 
        {
            XmlSerializer serialize = new XmlSerializer(this.GetType());
            FileStream file = File.Create(path);
            serialize.Serialize(file, this);
        }

        public static ConfigFileVirtualMouse DeserializeObject(string filePath) 
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                XmlSerializer serializer = new
                XmlSerializer(typeof(ConfigFileVirtualMouse));

                ConfigFileVirtualMouse vmfc = (ConfigFileVirtualMouse)serializer.Deserialize(fileStream);

                fileStream.Close();
                return vmfc;
            }
        
        }

    }
}
