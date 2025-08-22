using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IBookingRepository, InMemoryBookingRepository>();
var app = builder.Build();

app.MapPost("/api/bookings", async (BookingDto dto, IBookingRepository repo) =>
{
    var booking = new Booking(Guid.NewGuid(), dto.RoomId, dto.CheckIn, dto.CheckOut);
    await repo.AddAsync(booking);
    return Results.Created($"/api/bookings/{booking.Id}", booking);
});

app.MapGet("/api/bookings/{id:guid}", async (Guid id, IBookingRepository repo) =>
{
    var booking = await repo.GetAsync(id);
    return booking is null ? Results.NotFound() : Results.Ok(booking);
});

app.MapPut("/api/bookings/{id:guid}", async (Guid id, BookingDto dto, IBookingRepository repo) =>
{
    var updated = await repo.UpdateAsync(id, dto);
    return updated ? Results.NoContent() : Results.NotFound();
});

app.MapDelete("/api/bookings/{id:guid}", async (Guid id, IBookingRepository repo) =>
{
    var deleted = await repo.DeleteAsync(id);
    return deleted ? Results.NoContent() : Results.NotFound();
});

app.MapGet("/api/bookings", async (IBookingRepository repo) => Results.Ok(await repo.ListAsync()));

app.Run();

record Booking(Guid Id, int RoomId, DateTime CheckIn, DateTime CheckOut);

record BookingDto(int RoomId, DateTime CheckIn, DateTime CheckOut);

interface IBookingRepository
{
    Task AddAsync(Booking booking);
    Task<Booking?> GetAsync(Guid id);
    Task<bool> UpdateAsync(Guid id, BookingDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<IReadOnlyList<Booking>> ListAsync();
}

class InMemoryBookingRepository : IBookingRepository
{
    private readonly ConcurrentDictionary<Guid, Booking> _store = new();

    public Task AddAsync(Booking booking)
    {
        _store[booking.Id] = booking;
        return Task.CompletedTask;
    }

    public Task<Booking?> GetAsync(Guid id) =>
        Task.FromResult(_store.TryGetValue(id, out var b) ? b : null);

    public Task<bool> UpdateAsync(Guid id, BookingDto dto)
    {
        if (!_store.ContainsKey(id)) return Task.FromResult(false);
        var updated = new Booking(id, dto.RoomId, dto.CheckIn, dto.CheckOut);
        _store[id] = updated;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(Guid id) =>
        Task.FromResult(_store.TryRemove(id, out _));

    public Task<IReadOnlyList<Booking>> ListAsync() =>
        Task.FromResult((IReadOnlyList<Booking>)_store.Values.ToList());
}
