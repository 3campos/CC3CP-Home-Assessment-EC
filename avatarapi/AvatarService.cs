using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace avatarapi
{
    
    public static class AvatarService
    //this class houses the http routes for creating an avatar or adding one to the firebase database.
    {

        static string firebaseDatabaseUrl = "https://cc3d-assessment-default-rtdb.firebaseio.com/";
        //this is the url for the firebase database
        static string firebaseDatabaseDocument = "Avatars";
        //this is where the avatars are held
        static readonly HttpClient client = new HttpClient();
        //create an httpClient object for purposes of making HTTP requests to our database in firebase.

        //GET METHOD
        public static async Task<Avatar> GetById(string id)
        //using the GetById method to obtain the child of the object based on the id provided
        {
            string url = $"{firebaseDatabaseUrl}" + $"{firebaseDatabaseDocument}/" + $"{id}.json";
            //this is the url where this specific avatar, based on its id, is stored on the firebase database
            var httpResponseMessage = await client.GetAsync(url);
            //using the await method to await an HTTP response from firebase after the GET request is sent.

            if (httpResponseMessage.IsSuccessStatusCode)
            //if function; if the http response is successful, then the following should be return and render. Otherwise, the application should return "null".
            {
                var contentStream = await httpResponseMessage.Content.ReadAsStringAsync();
                //await method used to await the http response in stringified data
                if (contentStream != null && contentStream != "null")
                //as long as the http response is not null then perform the following
                {
                    var result = JsonSerializer.Deserialize<Avatar>(contentStream);
                    //converts the response into a json object
                    return result;
                    //returns the result, which is the avatar (json) object
                }               
            }

            return null;

        }

        public static async Task<Avatar> Add(Avatar avatar)
        //An Avatar parameter is passed in and the Add method (create method) is utilized to add a new avatar object.
        {
            avatar.Id = Guid.NewGuid().ToString("N");
            //a GUID, a unique type of Id is generated for that avatar.
            string avatarJsonString = JsonSerializer.Serialize(avatar);
            //use json serializer method to convert the Json into string, which in that form can be used for an HTTP request
            var payload = new StringContent(avatarJsonString, Encoding.UTF8, "application/json");
            //take the stringified Json, creates a new instance of it after converting it to binary string with the UTF8 method

            string url = $"{firebaseDatabaseUrl}" + $"{firebaseDatabaseDocument}/" + $"{avatar.Id}.json";
            //specify the url that will represent the http request for this avatar based on its id in javascript object notation format

            var httpResponseMessage = await client.PutAsync(url, payload);
            //using the await method to await an HTTP response from firebase after the PUT request is sent.
            
            if(httpResponseMessage.IsSuccessStatusCode)
            //if function; if the http response is successful, then the following should be return and render. Otherwise, the application should return "null".
            {
                var contentStream = await httpResponseMessage.Content.ReadAsStringAsync();
                //await method used to await the http response in stringified data
                var result = JsonSerializer.Deserialize<Avatar>(contentStream);
                //converts the response into a json object
                return result;
                //returns the result, which is the avatar (json) object
            }
            return null;
        }

        //UPDATE METHOD
        public static async Task<Avatar> Update(Avatar avatar, string id)
        //using the Update method to update the child of the object based on the id provided
        {
            avatar.Id = id;
            string avatarJsonString = JsonSerializer.Serialize(avatar);
            //use json serializer method to convert the Json into string, which in that form can be used for an HTTP request

            var payload = new StringContent(avatarJsonString, Encoding.UTF8, "application/json");
            //take the stringified Json, creates a new instance of it after converting it to binary string with the UTF8 method
            string url = $"{firebaseDatabaseUrl}" +
                        $"{firebaseDatabaseDocument}/" +
                        $"{id}.json";
            //this is the url where this specific avatar, based on its id, is stored on the firebase database
            var httpResponseMessage = await client.PutAsync(url, payload);
            //using the await method to await an HTTP response from firebase after the PUT request is sent.

            if (httpResponseMessage.IsSuccessStatusCode)
            //if function; if the http response is successful, then the following should be return and render. Otherwise, the application should return "null".
            {
                var contentStream = await httpResponseMessage.Content.ReadAsStringAsync();
                //await method used to await the http response in stringified data
                if (contentStream != null && contentStream != "null")
                //as long as the http response is not null then perform the following
                {
                    var result = JsonSerializer.Deserialize<Avatar>(contentStream);
                    //converts the response into a json object
                    return result;
                    //returns the result, which is the avatar (json) object
                }         
            }

            return null;

        }

        //DELETE METHOD
        public static async Task<string> DeleteById(string id)
        //using the DeleteById method to delete the child of the object based on the id provided
        {
            string url = $"{firebaseDatabaseUrl}" +
                        $"{firebaseDatabaseDocument}" +
                        $"{id}.json";
            //this is the url where this specific avatar, based on its id, is stored on the firebase database

            var httpResponseMessage = await client.DeleteAsync(url);
            //using the await method to await an HTTP response from firebase after the PUT request is sent.
            if (httpResponseMessage.IsSuccessStatusCode)
            //if function; if the http response is successful, then the following should be return and render. Otherwise, the application should return "null".
            {
                var contentStream = await httpResponseMessage.Content.ReadAsStringAsync();
                //await method used to await the http response in stringified data
                if(contentStream == "null")
                //if the http response is indeed null, then perform the following
                {
                    return "Deleted";
                    //return the above string if the http response is deleted
                }
            }

            return null;
        }
    }

    // MODEL
    //The below model creates an Avatar class for data that the application will manage.
    //The Avatar class will represent avatars in a database. For each instance of an Avatar, a row will be created and each property (Id, Name, IsComplete) will pertain to a column in a data table.
    public class Avatar
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public string Eye_Color { get; set; }
        public string Hair_Color { get; set; }
    }
}