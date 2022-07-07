using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", async (HttpContext http) =>   // process-image
{
    using var image = await Image.LoadAsync("wwwroot/front.png");
    int width = image.Width / 2;
    int height = image.Height / 2;
    image.Mutate(x => x.Resize(width, height));
    http.Response.Headers.CacheControl = $"public,max-age={TimeSpan.FromHours(24).TotalSeconds}";
    // ObjectDisposedException: Cannot access a disposed object. Object name: 'Image`1'.
    return Results.Stream(stream => image.SaveAsync(stream, PngFormat.Instance), "image/png");
});

app.Run();
