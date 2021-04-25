using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization; //Used for serialization into an xml file and deserialization back to a Dialogue object
using System.IO; //Used for input and output

public static class SerializerXML
{
    public static void Serialize<T>(string path, T obj)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T)); //Used to transform the oject into a XML file
        StreamWriter writer = new StreamWriter(path); //This writer will write a file named as we especify in the path
        serializer.Serialize(writer.BaseStream, obj); //Serialization
        writer.Close();
    }

    public static T Deserialize<T>(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T)); //Used to transform the XML file into an oject
        StreamReader reader = new StreamReader(path); //This reader will read the file named as we especify in the path
        T obj = (T)serializer.Deserialize(reader.BaseStream);
        reader.Close();
        return obj;
    }

}
