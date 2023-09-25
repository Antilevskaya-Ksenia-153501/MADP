using Microsoft.EntityFrameworkCore;
using WEB_153501_Antilevskaya.Domain.Entities;
using static System.Net.Mime.MediaTypeNames;

namespace WEB_153501_Antilevskaya.API.Data;
public class DbInitializer
{
    public static async Task SeedData(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Database.EnsureCreatedAsync();
        await context.Database.MigrateAsync();

        var webRootPath = @"C:\Users\Notebook\Desktop\MADP\Lab1\WEB_153501_Antilevskaya\WEB_153501_Antilevskaya.API\wwwroot\Images\";
        var parentRootPath = @"C:\Users\Notebook\Desktop\MADP\Lab1\WEB_153501_Antilevskaya\WEB_153501_Antilevskaya\wwwroot\Images\";

        if (!context.Category.Any())
        {
            context.Category.AddRange(new List<Category>
            {
                new Category{Name="Sculpture", NormalizedName="sculpture" },
                new Category{Name="Painting", NormalizedName="painting"}
            });
            await context.SaveChangesAsync();
        }

        if (!context.Exhibit.Any())
        {
            var baseUrl = app.Configuration["AppSettings:BaseURL"];
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new Exception("BaseURL is not configured.");
            }
            context.Exhibit.AddRange(new List<Exhibit>
            {
                new Exhibit{Title="Harmony Unveiled", Description="\"Harmony Unveiled\" is a captivating sculpture that embodies the essence of balance and unity. Crafted from gleaming bronze, the sculpture stands tall and proud, reaching a height of six feet. Its form is a harmonious composition of fluid curves and graceful lines, exuding a sense of elegance and tranquility",
                        Category=context.Category.FirstOrDefault(obj => obj.NormalizedName.Equals("sculpture")),
                        Price=100, Image=await SaveImageAndGetUrl(parentRootPath, webRootPath, "Harmony Unveiled.jpg", baseUrl)},
                new Exhibit{Id = 2, Title="Whispering Serenity", Description="\"Whispering Serenity\" is a captivating sculpture that evokes a sense of calm and tranquility. Crafted with meticulous attention to detail, the sculpture stands as a testament to the beauty of serenity in the midst of chaos.",
                        Category=context.Category.FirstOrDefault(obj => obj.NormalizedName.Equals("sculpture")),
                        Price=140, Image=await SaveImageAndGetUrl(parentRootPath, webRootPath, "Whispering Serenity.jpg", baseUrl)},
                new Exhibit{Id = 3, Title="Mystic Symphony", Description="\"Mystic Symphony\" is an enchanting painting that immerses viewers in a world of ethereal beauty and musical inspiration. The canvas comes alive with vibrant colors and flowing brushstrokes, creating a harmonious blend of tones that captivate the eye. The painting depicts a surreal landscape, where swirling melodies intertwine with nature's elements.",
                        Category=context.Category.FirstOrDefault(obj => obj.NormalizedName.Equals("painting")),
                        Price=230, Image=await SaveImageAndGetUrl(parentRootPath, webRootPath, "Mystic Symphony.jpg", baseUrl)},
                new Exhibit{Id = 4, Title="Ethereal Reverie", Description="\"Ethereal Reverie\" is a captivating painting that transports viewers into a realm of dreamlike beauty and contemplation. The canvas is a tapestry of soft, pastel hues and gentle brushstrokes, evoking a sense of serenity and tranquility. The painting portrays an otherworldly scene where reality and imagination intertwine.",
                        Category=context.Category.FirstOrDefault(obj => obj.NormalizedName.Equals("painting")),
                        Price=180, Image=await SaveImageAndGetUrl(parentRootPath, webRootPath, "Ethereal Reverie.jpg", baseUrl)},
                new Exhibit{Id = 5, Title="Tapestry of Dreams", Description="In this captivating image, \"Tapestry of Dreams,\" a mesmerizing blend of vibrant colors and intricate patterns come together to create a visual feast for the eyes. The composition showcases a kaleidoscope of various elements and forms, each uniquely interwoven to form a stunning tapestry of dreams. The picture exudes an ethereal quality, as if plucked from the realm of imagination.",
                        Category=context.Category.FirstOrDefault(obj => obj.NormalizedName.Equals("sculpture")),
                        Price=340, Image=await SaveImageAndGetUrl(parentRootPath, webRootPath, "Tapestry of Dreams.jpg", baseUrl)},
                new Exhibit{Id = 6, Title="Kaleidoscope of Imagination", Description="In the captivating image titled \" Kaleidoscope of Imagination,\" a vibrant tapestry of dreams unfolds before the viewer's eyes. The composition transports us to a realm where reality merges seamlessly with the fantastical. Within this kaleidoscope of imagination, a mesmerizing interplay of colors, shapes, and textures takes center stage. As if glimpsing into the deepest recesses of our subconscious, the image unveils a myriad of dreamscapes, each with its own unique narrative waiting to be discovered.",
                        Category=context.Category.FirstOrDefault(obj => obj.NormalizedName.Equals("sculpture")),
                        Price=250, Image=await SaveImageAndGetUrl(parentRootPath, webRootPath, "Kaleidoscope of Imagination.jpg", baseUrl)},
                new Exhibit{Id = 7, Title="Brushstroke of Dreams", Description="In the captivating painting titled \"Brushstroke of Dreams,\" an ethereal world unfolds, inviting viewers into a realm of enchantment and wonder. The canvas comes alive with an exquisite blend of colors, textures, and forms that evoke a sense of serene beauty. Within this enchanting scene, a delicate balance between reality and fantasy is delicately portrayed. Soft, gentle brushstrokes create a dreamlike atmosphere, as if the painting itself is a gateway to a realm beyond our everyday experiences. It is a place where imagination takes flight and dreams manifest in breathtaking visual poetry.",
                        Category=context.Category.FirstOrDefault(obj => obj.NormalizedName.Equals("painting")),
                        Price=470, Image=await SaveImageAndGetUrl(parentRootPath, webRootPath, "Brushstroke of Dreams.jpg", baseUrl)},
                new Exhibit{Id = 8, Title="Verdant Journey", Description="In the captivating painting titled \"Verdant Journey,\" a lush and vibrant world unfolds before our eyes, beckoning us to embark on a mesmerizing exploration. The canvas is adorned with an array of rich greens and earthy tones, immersing us in a verdant realm of enchantment. Within this captivating scene, nature's embrace takes center stage. The painting exudes a sense of vitality and growth, as if every brushstroke breathes life into the canvas. The lush foliage and delicate blooms create a tapestry of colors and textures, inviting us to wander through a dreamscape where tranquility and harmony prevail.",
                        Category=context.Category.FirstOrDefault(obj => obj.NormalizedName.Equals("painting")),
                        Price=180, Image=await SaveImageAndGetUrl(parentRootPath, webRootPath, "Verdant Journey.jpg", baseUrl)},
            });
            await context.SaveChangesAsync();
        }
    }
    private static async Task<string> SaveImageAndGetUrl(string parentRootPath,string webRootPath, string imageName, string imageBaseUrl)
    {
        if (!File.Exists(Path.Combine(webRootPath, imageName)))
        {
            var imagePath = Path.Combine(parentRootPath, imageName);
            var imageBytes = await File.ReadAllBytesAsync(imagePath);

            var savePath = Path.Combine(webRootPath, imageName);
            await File.WriteAllBytesAsync(savePath, imageBytes);
        }
        return $"{imageBaseUrl}/Images/{imageName}";
        
    }
}

