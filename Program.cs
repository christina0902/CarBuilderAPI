using CarBuilderAPI.Models;
using CarBuilderAPI.Models.DTOs;
List<Interior> interiors = new List<Interior>
{
    new Interior()
    {
        Id = 1,
        Price = 100,
        Material = "Beige Fabric"
    },
    new Interior()
    {
        Id = 2,
        Price = 200,
        Material = "Charcoal Fabric"
    },
    new Interior()
    {
        Id = 3,
        Price = 300,
        Material = "White Leather"
    },
    new Interior()
    {
        Id = 4,
        Price = 400,
        Material = "Black Leather"
    }
};
List<Order> orders = new List<Order>
{
    new Order()
    {
        Id = 1,
        Timestamp = new DateTime(2023, 10, 8),
        WheelId = 1,
        TechnologyId = 1,
        PaintId = 1,
        InteriorId = 1,
        Fullfilled = false
    }
};
List<PaintColor> paintColors = new List<PaintColor>
{
    new PaintColor()
    {
        Id = 1,
        Price = 10.99M,
        Color = "Silver"
    },
    new PaintColor()
    {
        Id = 2,
        Price = 12.99M,
        Color = "Midnight Blue"
    },
    new PaintColor()
    {
        Id = 3,
        Price = 15.99M,
        Color = "Firebrick Red"
    },
    new PaintColor()
    {
        Id = 4,
        Price = 20.99M,
        Color = "Spring Green"
    }
};
List<Technology> technologies = new List<Technology>
{
    new Technology()
    {
        Id = 1,
        Price = 400,
        Package = "Basic Package (basic sound system)"
    },
    new Technology()
    {
        Id = 2,
        Price = 800,
        Package = "Navigation Package (includes integrated navigation controls)"
    },
    new Technology()
    {
        Id = 3,
        Price = 1200,
        Package = "Visibility Package (includes side and rear cameras)"
    },
    new Technology()
    {
        Id = 4,
        Price = 1600,
        Package = "Ultra Package (includes navigation and visibility packages)"
    }
};
List<Wheels> wheels = new List<Wheels>
{
    new Wheels()
    {
        Id = 1,
        Price = 1000,
        Style = "17-inch Pair Radial"
    },
    new Wheels()
    {
        Id = 2,
        Price = 1500,
        Style = "17-inch Pair Radial Black"
    },
    new Wheels()
    {
        Id = 3,
        Price = 2200,
        Style = "18-inch Pair Spoke Silver"
    },
    new Wheels()
    {
        Id = 4,
        Price = 3000,
        Style = "18-inch Pair Spoke Black"
    }
};
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(options =>
                {
                    options.AllowAnyOrigin();
                    options.AllowAnyMethod();
                    options.AllowAnyHeader();
                });
}

app.UseHttpsRedirection();
app.MapGet("/wheels", () =>
{
    return wheels.Select(w => new WheelsDTO
    {
        Id = w.Id,
        Price = w.Price,
        Style = w.Style
    });
});

app.MapGet("/technologies", () =>
{
    return technologies.Select(t => new TechnologyDTO
    {
        Id = t.Id,
        Price = t.Price,
        Package = t.Package
    });
});

app.MapGet("/interiors", () =>
{
    return interiors.Select(i => new InteriorDTO
    {
        Id = i.Id,
        Price = i.Price,
        Material = i.Material
    });
});

app.MapGet("/paintcolors", () =>
{
    return paintColors.Select(pc => new PaintColorDTO
    {
        Id = pc.Id,
        Price = pc.Price,
        Color = pc.Color
    });
});


app.MapGet("/orders", () =>
{
List<OrderDTO> ordersDTO = new List<OrderDTO>();

foreach (Order order in orders)
{
    Wheels wheel = wheels.FirstOrDefault(w => w.Id == order.WheelId);
    Technology technology = technologies.FirstOrDefault(t => t.Id == order.TechnologyId);
    PaintColor paintColor = paintColors.FirstOrDefault(pc => pc.Id == order.PaintId);
    Interior interior = interiors.FirstOrDefault(i => i.Id == order.InteriorId);


        OrderDTO orderDTO = new OrderDTO
        {
            Id = order.Id,
            Timestamp = order.Timestamp,
            WheelId = order.WheelId,
            Wheel = wheel == null ? null : new WheelsDTO
            {
                Id = wheel.Id,
                Price = wheel.Price,
                Style = wheel.Style
            },
            TechnologyId = order.TechnologyId,
            Technology = new TechnologyDTO
            {
                Id = technology.Id,
                Price = technology.Price,
                Package = technology.Package
            },
            PaintId = order.PaintId,
            PaintColor = new PaintColorDTO
            {
                Id = paintColor.Id,
                Price = paintColor.Price,
                Color = paintColor.Color
            },
            InteriorId = order.InteriorId,
            Interior = new InteriorDTO
            {
                Id = interior.Id,
                Price = interior.Price,
                Material = interior.Material
            }            
        };
        ordersDTO.Add(orderDTO); 
    }
    return ordersDTO;
});

app.MapPost("/orders", (Order order) =>
{
    Wheels wheel = wheels.FirstOrDefault(w => w.Id == order.WheelId);
    Technology technology = technologies.FirstOrDefault(t => t.Id == order.TechnologyId);
    PaintColor paintColor = paintColors.FirstOrDefault(pc => pc.Id == order.PaintId);
    Interior interior = interiors.FirstOrDefault(i => i.Id == order.InteriorId);

    if(wheel == null || technology == null || paintColor == null || interior == null)
    {
        return Results.BadRequest();
    }
    order.Id = orders.Max(o => o.Id) + 1;
    order.Timestamp = DateTime.Now;
    orders.Add(order);

    return Results.Created($"/orders/{order.Id}", new OrderDTO
    {
                Id = order.Id,
                Timestamp = order.Timestamp,
                WheelId = order.WheelId,
                Wheel = wheel == null ? null : new WheelsDTO
            {
                Id = wheel.Id,
                Price = wheel.Price,
                Style = wheel.Style
            },
            TechnologyId = order.TechnologyId,
            Technology = new TechnologyDTO
            {
                Id = technology.Id,
                Price = technology.Price,
                Package = technology.Package
            },
            PaintId = order.PaintId,
            PaintColor = new PaintColorDTO
            {
                Id = paintColor.Id,
                Price = paintColor.Price,
                Color = paintColor.Color
            },
            InteriorId = order.InteriorId,
            Interior = new InteriorDTO
            {
                Id = interior.Id,
                Price = interior.Price,
                Material = interior.Material
            }
    });
});

app.MapPost("/orders/{orderId}/fulfill", (int orderId) =>
{
  Order order = orders.FirstOrDefault(o => o.Id == orderId);
    order.Fullfilled = true;
});
app.Run();
