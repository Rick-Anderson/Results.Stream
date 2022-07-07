using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", async (HttpContext http) => 
{
    // removing following // produces ObjectDisposedException
    // using
    var image = await Image.LoadAsync("wwwroot/front.png");
    int width = image.Width / 2;
    int height = image.Height / 2;
    image.Mutate(x => x.Resize(width, height));
    http.Response.Headers.CacheControl = $"public,max-age={TimeSpan.FromHours(24).TotalSeconds}";

    return Results.Stream(stream => image.SaveAsync(stream, PngFormat.Instance), "image/png");
    // when using is used in first line, ie using var image = await Image.LoadAsync("wwwroot/front.png");
    // ObjectDisposedException: Cannot access a disposed object. Object name: 'Image`1'.
});

app.Run();
