using avatarapi;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
// using FirebaseAdmin;
//importing FirebaseAdmin namespace, i.e., a container of other classes

var builder = WebApplication.CreateBuilder(args);
//builder variable declared
    //The WebApplication class configures the HTTP routes.
    //The CreateBuilder method initializes a new instance of the WebApplication class. 

//Creates an application instance with "Google Application Default Credentials"
//AppOptions controls the creation of the Firebase App

var app = builder.Build();
    //app variable declared
    //The Build method creates the application. It is a method of the TreeRouteBuilder class, which is used to create an interface to implement an HTTP router.
    //The "args" parameter will pass in the arguments of the application that our API will make calls to.

app.MapGet("Avatars/{avatarId}.json", async (string avatarId) =>
//The MapGet method adds a route endpoint ("/avatars")that can be accessed by an HTTP GET request.
 //The anonymous arrow function returns the below displays it in the browser.
{
    var result = await AvatarService.GetById(avatarId);
    //using the await method to await an HTTP response from firebase after the GET request is sent in the form of the avatar by its id.
    if (result == null) return Results.NotFound("Avatar Not Found");
    return Results.Ok(result);
});

app.Run();
    //The Run method completes execution of the middleware, i.e., terminates it. 