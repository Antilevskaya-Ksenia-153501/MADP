using WEB_153501_Antilevskaya.Domain.Models;
using WEB_153501_Antilevskaya.Extensions;
using System.Text.Json.Serialization;
using WEB_153501_Antilevskaya.Domain.Entities;

namespace WEB_153501_Antilevskaya.Services.CartService;
public class SessionCart : Cart
{
    public static Cart GetCart(IServiceProvider services)
    {
        ISession? session = services.GetRequiredService<IHttpContextAccessor>().HttpContext?.Session;
        SessionCart cart = session?.Get<SessionCart>("Cart") ?? new SessionCart();
        cart.Session= session;
        return cart;
    }

    [JsonIgnore]
    public ISession? Session { get; set; }

    public override void AddToCart(Exhibit exhibit)
    {
        base.AddToCart(exhibit);
        Session?.Set("Cart", this);
    }

    public override void RemoveItems(int id)
    {
        base.RemoveItems(id);
        Session?.Set("Cart", this);
    }

    public override void ClearAll()
    {
        base.ClearAll();
        Session?.Set("Cart", this);
    }
}
