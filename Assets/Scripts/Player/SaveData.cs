using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;

using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using System;
using System.Runtime.Serialization;
using System.Reflection;
using System.Security.Cryptography;


[System.Serializable]
public class SaveData : ISerializable {

	public int level;

	public SaveData() {}
	
	public SaveData(SerializationInfo info, StreamingContext context) 
	{
		level = (int)info.GetValue("level", typeof(int));
	}

	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue("level", level);
	}
	
	private static string strEncrypt = "*#4$%^.++q~!cfr0(_!#$@$!&#&#*&@(7cy9rn8r265&$@&*E^184t44tq2cr9o3r6329";
	private static byte[] dv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
	
	public void Save() 
	{
		// using(Stream stream = File.Open("save01.sav", FileMode.Create)) // 1. create backing storage stream. In your case a file stream
			
		using(Stream stream = File.Open("save01.sav", FileMode.Create)) {
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Binder = new VersionDeserializationBinder(); 
			
			byte[] byKey = Encoding.UTF8.GetBytes(strEncrypt.Substring(0, 8));
			using(DESCryptoServiceProvider des = new DESCryptoServiceProvider()) {
				using(Stream cryptoStream = new CryptoStream(stream, des.CreateEncryptor(byKey, dv), CryptoStreamMode.Write)) // 2. create a CryptoStream in write mode
				{
					Log.debug("Save", "Saving...");
					using(cryptoStream)
						binaryFormatter.Serialize(cryptoStream, this); // 3. write to the cryptoStream
					cryptoStream.Close();
				}
			}
			stream.Close();
		}
		Log.debug("Save", "Saved");
	}
	
	public static SaveData Load()
	{
		if (!File.Exists("save01.sav"))
			return null;
			
		SaveData data = null;
		using(Stream stream = File.Open("save01.sav", FileMode.Open)) {
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Binder = new VersionDeserializationBinder(); 
			
			byte[] byKey = Encoding.UTF8.GetBytes(strEncrypt.Substring(0, 8));
			using (DESCryptoServiceProvider des = new DESCryptoServiceProvider()) {
				using (Stream cryptoStream = new CryptoStream(stream, des.CreateDecryptor(byKey, dv), CryptoStreamMode.Read))
				{
					Log.debug("Save", "Loading...");
					try {
						data = (SaveData) binaryFormatter.Deserialize(cryptoStream);
					} catch (Exception e) {
						Log.debug("Save", ""+e);
						return new SaveData();
					}
					Log.debug("Save", "Loaded (level={0})", data.level);
					cryptoStream.Close();
				}
			}
			stream.Close();
		}
		return data;
	}
}

public sealed class VersionDeserializationBinder : SerializationBinder 
{ 
    public override Type BindToType(string assemblyName, string typeName)
    { 
        if (!string.IsNullOrEmpty(assemblyName) && !string.IsNullOrEmpty(typeName)) 
        { 
            Type typeToDeserialize = null;
            assemblyName = Assembly.GetExecutingAssembly().FullName;
            typeToDeserialize = Type.GetType(String.Format("{0}, {1}", typeName, assemblyName)); // The following line of code returns the type. 
            return typeToDeserialize; 
        } 
        return null; 
    } 
}