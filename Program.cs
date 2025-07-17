using Hotel_Booking_App.Service.ChatService;
using HotelBookingAPI.ExceptionHandlerGlobalExceptionMiddleware;
using HotelBookingAPI.model;
using HotelBookingAPI.Service.BookingService;
using HotelBookingAPI.Service.CustomerService;
using HotelBookingAPI.Service.RoomService;
using HotelBookingAPI.Service.SpecialRequestService;

var builder = WebApplication.CreateBuilder(args);

// --- Fix circular dependency ---
builder.Services.AddSingleton<BookingServiceImpl>();
builder.Services.AddSingleton<IBookingService>(provider =>
    provider.GetRequiredService<BookingServiceImpl>());
builder.Services.AddSingleton<IBookingReaderService>(provider =>
    provider.GetRequiredService<BookingServiceImpl>());

// --- Other services ---
builder.Services.AddSingleton<ICustomerService, CustomerServiceImpl>();
builder.Services.AddSingleton<IRoomService, RoomServiceImpl>();
builder.Services.AddSingleton<ISpecialRequestService, SpecialRequestServiceImpl>();
builder.Services.AddScoped<ChatService, ChatServiceImpl>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseRouting(); // <-- Ensure this exists
app.UseAuthorization();

app.MapControllers();
app.Run();
