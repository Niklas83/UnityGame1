using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication1.UserService;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            //***************************************************   MAP SERVICE   *************************************************************//

            MapService.JsonMapClient mapClient = new MapService.JsonMapClient();         //TestClient is ServiceReference to GameWCFservice

            //Send map to DB/backend
            var mapToPost = Encoding.UTF8.GetBytes("{ \"test\":\"some data\" *ALOT OF JSON RETRNED THAT IS USED TO CREATE MAP*}");
            var sendMapReturnValueAndTEST = mapClient.Sendmap(mapToPost);



    //TODO: Create class containing map name, mapId, Mapdata, userId, finalVersion etc
    //Make this ^ the case for all services, (if this works within unity)
            //Get single map from DB/Backend
            var getMapUsingIDvalueAndTEST = mapClient.Getmap(7);
            var getMapbyteArrayTransformedToString = System.Text.Encoding.UTF8.GetString(getMapUsingIDvalueAndTEST);


            //Get all maps from a user
            var getAllMapsFromUser = mapClient.GetmapsFromUser(5756);
            var getAllMapsFromUserTransformedToString = System.Text.Encoding.UTF8.GetString(getAllMapsFromUser);


            //Get multiple maps from DB/Backend using multiple map ID's
            int[] mapIds = new[] {354, 1, 34, 23, 45};
            var getMultipleMapsByMapIds = mapClient.GetMultipleMapsByMapIDs(mapIds);
            var getMultipleMapsByMapIdsTransformedToString = System.Text.Encoding.UTF8.GetString(getMultipleMapsByMapIds);


            //Get all maps ID and maybe some image?  (TODO: need to add some additional filter thingy to this)
            var getAllMapIDs = mapClient.GetAllMapsID();
            var getAllMapIDsTransformedToString = System.Text.Encoding.UTF8.GetString(getAllMapIDs);


            //Send map WITH image to DB/backend
            var mapDataToPost = Encoding.UTF8.GetBytes("{ \"test\":\"some data\" *ALOT OF JSON RETRNED THAT IS USED TO CREATE MAP*}");

            byte[] imageArray = File.ReadAllBytes("c:\\theimage.png");      //MUST CHANGE THIS FILE PATH TO TEST*******************!!!

            var mapName = "before the upd";

            int finalVersion = 0;       // 0 = work in progress,  1 = final map

            int userId = 22;

            //Needed to change binding sizes in bouth App.config (this client) AND Web.config (at the service)
            //Can with current configurations send ~ 3.5MB
            string sendMapWithImageReturnValueAndTEST = mapClient.SendmapWithImage(mapDataToPost, imageArray, mapName, finalVersion, userId);
            //byte[] imageArray2 = File.ReadAllBytes(@"C:\Users\Nronnlun\Pictures\imageTest.png");




            //Send map WITH image to DB/backend
            var UPDATEmapDataToPost = Encoding.UTF8.GetBytes("{UPDATED \"test\":\"some data\" *ALOT OF JSON RETRNED THAT IS USED TO CREATE MAP*}");

            byte[] UPDATEimageArray = File.ReadAllBytes("c:\\theimage.png");      //MUST CHANGE THIS FILE PATH TO TEST*******************!!!

            var UPDATEmapName = "UPDATED MAP NAME";

            int UPDATEfinalVersion = 1;       // 0 = work in progress,  1 = final map

            int UPDATEuserId = 22;

            int mapId = 19;
            
            string updateExistingMap = mapClient.UpdateMapInProgress(UPDATEmapDataToPost, UPDATEimageArray, UPDATEmapName, UPDATEfinalVersion, mapId, UPDATEuserId);




            //***************************************************   USER SERVICE   *************************************************************//


            //Register user mock
            UserService.UserServiceClient userServClient = new UserServiceClient();

            var userRegistrationThatFailsDueUsedEmail = userServClient.UserRegister("nicksdrf", "notnick", "sdgsd@dgsSDFSDsf.com");

            var userRegistrationThatFailsDueTakenUserName = userServClient.UserRegister("nick", "nick", "sdgsd@dgssf.com");

            var userRegistrationThatSucceeds = userServClient.UserRegister("BAANewNi67ck422r5t", "NewNick", "BAANEW761@NEW345422.com");


            //Get user info based on if its you own or other person
            UserInformation userInfoOwn = userServClient.GetUser(423543, 423543);

            UserInformation userInfoOther = userServClient.GetUser(2234, 456423);

            

            var test = "adsrfas";





        } 
    }
}
