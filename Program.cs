using HotelBookingAPI.ExceptionHandlerGlobalExceptionMiddleware;
using Hotel_Booking_App.Service.ChatService;
using HotelBookingAPI.Service.BookingService;
using HotelBookingAPI.Service.CustomerService;
using HotelBookingAPI.Service.RoomService;
using HotelBookingAPI.Service.SpecialRequestService;

var builder = WebApplication.CreateBuilder(args);

// Register services using interfaces and their implementations
builder.Services.AddSingleton<IBookingService, BookingServiceImpl>();
builder.Services.AddSingleton<ICustomerService, CustomerServiceImpl>();
builder.Services.AddSingleton<IRoomService, RoomServiceImpl>();
builder.Services.AddSingleton<ISpecialRequestService, SpecialRequestServiceImpl>();
builder.Services.AddScoped<ChatService, ChatServiceImpl>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseAuthorization();
app.MapControllers();
app.Run();
