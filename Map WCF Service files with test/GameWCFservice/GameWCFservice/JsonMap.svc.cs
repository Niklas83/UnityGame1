using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;

namespace GameWCFservice
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "GetJsonMap" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select GetJsonMap.svc or GetJsonMap.svc.cs at the Solution Explorer and start debugging.
    public class JsonMap : IJsonMap
    {
        private SqlConnection DBConn;
        private SqlCommand DBCommand;
        private readonly string _connStr = ConfigurationManager.ConnectionStrings["azureSqlConnectionString"].ConnectionString;

        public bool Sendmap(byte[] mapByteArray)    //Should prolly be deleted
        {
            //save this in DB
            var data = System.Text.Encoding.UTF8.GetString(mapByteArray);

            if (true)
            {
                return true;       //If database save succeeded AND that validation of the string is decoded to a valid json string with correct file format
            }
            else
            {
                return false;
            }
        }

        public byte[] Getmap(int MapId)
        {
            string mapDataFromDB = "";

            DBConn = new SqlConnection(_connStr);
            DBCommand = DBConn.CreateCommand();
            const string queryString = "SELECT * FROM PlayerMaps WHERE MapId = @MapId";

            using (this.DBConn)
            using (this.DBCommand)
            {
                DBCommand.CommandText = queryString;
                DBCommand.Parameters.AddWithValue("@MapId", MapId);
                DBConn.Open();

                using (SqlDataReader reader = DBCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        mapDataFromDB = (string)reader["MapData"];
                    }
                }
            }
            var dataToReturn = Encoding.UTF8.GetBytes(mapDataFromDB);
            return dataToReturn;
        }

        public byte[] GetmapsFromUser(int userId)   //TODO
        {
            var dataToReturn = Encoding.UTF8.GetBytes("RETURNING ALL MAPS IN JSON FROM USER WITH ID: " + userId + " STRING");

            return dataToReturn;
        }

        public byte[] GetMultipleMapsByMapIDs(int[] mapIds)     //TODO
        {
            StringBuilder strBuilder = new StringBuilder();
            foreach (int mapId in mapIds)
            {
                strBuilder.Append("RETURNING ALL MAPS IN JSON FROM USER WITH ID: " + mapId + " STRING");
            }
            var dataToReturn = Encoding.UTF8.GetBytes(strBuilder.ToString());

            return dataToReturn;
        }

        public byte[] GetAllMapsID()    //TODO
        {
            var dataToReturn = Encoding.UTF8.GetBytes("RETURNING ARRAY OF ALL THE MAP IDS");

            return dataToReturn;
        }


        //Just send image when finalVersion is set to 1 (done)
        public string SendmapWithImage(byte[] mapByteArray, byte[] imageByteArray, string MapName, int finalVersion, int userId)
        {
            var mapData = System.Text.Encoding.UTF8.GetString(mapByteArray);
            DateTime mapCreatedDate = DateTime.Now;
            DateTime mapChangedDate = DateTime.Now;

            //Image as varbinary
            string mapHref = "http://www.menucool.com/slider/prod/image-slider-4.jpg";      //Save to web hotel folder with all images

            if (finalVersion > 1 || finalVersion < 0)
            {
                return "Invalid map versoning";
            }

            MapName = MapName.Trim();
            MapName = MapName.ToLower();
            MapName = MapName.First().ToString().ToUpper() + String.Join("", MapName.Skip(1));

            if (!Regex.IsMatch(MapName, @"^[a-zA-Z0-9# ]+$"))
            {
                return "The map name can only contain letters, spaces and numbers, and must atleast have one character.";
            }

            string DbMapName = "";

            DBConn = new SqlConnection(_connStr);
            DBCommand = DBConn.CreateCommand();
            string queryString = "SELECT * FROM PlayerMaps WHERE MapName = @MapName";
            using (this.DBConn)
            using (this.DBCommand)
            {
                DBCommand.CommandText = queryString;
                DBCommand.Parameters.AddWithValue("@MapName", MapName);
                DBConn.Open();

                using (SqlDataReader reader = DBCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DbMapName = (string)reader["MapName"];
                    }
                }
            }

            if (DbMapName == MapName)
            {
                return "A map with this name already exists, please choose another name.";
            }
            else
            {
                //save this image in DB         //TODO (maybe not)

                //********** maybe save this on file server and keep a reference to the URL in the DB instead, that way it can easily be viewed on the web aswell?! ************
                MemoryStream ms = new MemoryStream(imageByteArray);
                Image image = Image.FromStream(ms);

                //image.Save(@"C:\Users\Nronnlun\Pictures\imageTest.png");
                //byte[] imageArray = ImageToByteArraybyImageConverter(image);
                //return imageArray;

                DBConn = new SqlConnection(_connStr);
                DBCommand = DBConn.CreateCommand();

                queryString =
                    "INSERT INTO PlayerMaps (UserId, MapData, DateCreated, DateChanged, FinalVersion, MapImageHref, MapName) " +
                    "VALUES(@userId, @mapData, @mapCreatedDate, @mapChangedDate, @finalVersion, @mapHref, @MapName )";

                using (this.DBConn)
                using (this.DBCommand)
                {
                    DBCommand.CommandText = queryString;

                    DBCommand.Parameters.AddWithValue("@userId", userId);
                    DBCommand.Parameters.AddWithValue("@mapData", mapData);
                    DBCommand.Parameters.AddWithValue("@mapCreatedDate", mapCreatedDate);
                    DBCommand.Parameters.AddWithValue("@mapChangedDate", mapChangedDate);
                    DBCommand.Parameters.AddWithValue("@finalVersion", finalVersion);
                    DBCommand.Parameters.AddWithValue("@mapHref", mapHref);
                    DBCommand.Parameters.AddWithValue("@MapName", MapName);

                    DBConn.Open();
                    DBCommand.ExecuteNonQuery();
                }
                return "Successfully sent map";       //If database save succeeded AND that validation of the string is decoded to a valid json string with correct file format
            }
        }


        public string UpdateMapInProgress(byte[] mapByteArray, byte[] imageByteArray, string mapName, int finalVersion, int mapId, int userId)
        {
            var mapData = System.Text.Encoding.UTF8.GetString(mapByteArray);
            DateTime mapChangedDate = DateTime.Now;

            //Image as varbinary
            string mapHref = "http://upload.wikimedia.org/wikipedia/commons/e/e3/Skull-Icon.svg";      //Save to web hotel folder with all images

            if (finalVersion > 1 || finalVersion < 0)
            {
                return "Invalid map versioning";
            }

            mapName = mapName.Trim();
            mapName = mapName.ToLower();
            mapName = mapName.First().ToString().ToUpper() + String.Join("", mapName.Skip(1));

            if (!Regex.IsMatch(mapName, @"^[a-zA-Z0-9# ]+$"))
            {
                return "The map name can only contain letters, spaces and numbers, and must atleast have one character.";
            }

            int DbFinalVersion = -1;

            DBConn = new SqlConnection(_connStr);
            DBCommand = DBConn.CreateCommand();
            string queryString = "SELECT * FROM PlayerMaps WHERE mapId = @mapId AND UserId = @userId";
            using (this.DBConn)
            using (this.DBCommand)
            {
                DBCommand.CommandText = queryString;
                DBCommand.Parameters.AddWithValue("@mapId", mapId);
                DBCommand.Parameters.AddWithValue("@userId", userId);
                DBConn.Open();

                using (SqlDataReader reader = DBCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DbFinalVersion = (int)reader["FinalVersion"];
                    }
                }
            }

            if (DbFinalVersion == 1)
            {
                return "This map has already been set to be final version";
            }
            else if (DbFinalVersion != 0)
            {
                return "You cannot update a map that not previously exists.";
            }
            else
            {
                //********** maybe save this on file server and keep a reference to the URL in the DB instead, that way it can easily be viewed on the web aswell?! ************
                MemoryStream ms = new MemoryStream(imageByteArray);
                Image image = Image.FromStream(ms);

                //image.Save(@"C:\Users\Nronnlun\Pictures\imageTest.png");

                DBConn = new SqlConnection(_connStr);
                DBCommand = DBConn.CreateCommand();

                queryString =
                    "UPDATE PlayerMaps " +
                    "SET MapData = @mapData, DateChanged = @mapChangedDate, FinalVersion = @finalVersion, MapImageHref = @mapHref, MapName = @MapName " +
                    "WHERE UserId = @userId AND MapId = @mapId";

                using (this.DBConn)
                using (this.DBCommand)
                {
                    DBCommand.CommandText = queryString;

                    DBCommand.Parameters.AddWithValue("@userId", userId);
                    DBCommand.Parameters.AddWithValue("@mapData", mapData);
                    DBCommand.Parameters.AddWithValue("@mapChangedDate", mapChangedDate);
                    DBCommand.Parameters.AddWithValue("@finalVersion", finalVersion);
                    DBCommand.Parameters.AddWithValue("@mapHref", mapHref);
                    DBCommand.Parameters.AddWithValue("@MapName", mapName);
                    DBCommand.Parameters.AddWithValue("@mapId", mapId);

                    DBConn.Open();
                    DBCommand.ExecuteNonQuery();
                }

                return "Successfully updated map";
            }
            
        }
    }
}