using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IRequestRepository, InMemoryRequestRepository>();
var app = builder.Build();

app.MapPost("/api/requests", async (SpecialRequestDto dto, IRequestRepository repo) =>
{
    var req = new SpecialRequest(Guid.NewGuid(), dto.BookingId, dto.Note, dto.Priority);
    await repo.AddAsync(req);
    return Results.Created($"/api/requests/{req.Id}", req);
});

app.MapGet("/api/requests/{id:guid}", async (Guid id, IRequestRepository repo) =>
{
    var req = await repo.GetAsync(id);
    return req is null ? Results.NotFound() : Results.Ok(req);
});

app.MapPut("/api/requests/{id:guid}", async (Guid id, SpecialRequestDto dto, IRequestRepository repo) =>
{
    var updated = await repo.UpdateAsync(id, dto);
    return updated ? Results.NoContent() : Results.NotFound();
});

app.MapDelete("/api/requests/{id:guid}", async (Guid id, IRequestRepository repo) =>
{
    var deleted = await repo.DeleteAsync(id);
    return deleted ? Results.NoContent() : Results.NotFound();
});

app.MapGet("/api/requests", async (IRequestRepository repo) => Results.Ok(await repo.ListAsync()));

app.MapGet("/api/requests/by-booking/{bookingId:guid}", async (Guid bookingId, IRequestRepository repo) =>
    Results.Ok(await repo.ListByBookingAsync(bookingId)));

app.Run();

record SpecialRequest(Guid Id, Guid BookingId, string Note, int Priority);

record SpecialRequestDto(Guid BookingId, string Note, int Priority);

interface IRequestRepository
{
    Task AddAsync(SpecialRequest req);
    Task<SpecialRequest?> GetAsync(Guid id);
    Task<bool> UpdateAsync(Guid id, SpecialRequestDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<IReadOnlyList<SpecialRequest>> ListAsync();
    Task<IReadOnlyList<SpecialRequest>> ListByBookingAsync(Guid bookingId);
}

class InMemoryRequestRepository : IRequestRepository
{
    private readonly ConcurrentDictionary<Guid, SpecialRequest> _store = new();

    public Task AddAsync(SpecialRequest req)
    {
        _store[req.Id] = req;
        return Task.CompletedTask;
    }

    public Task<SpecialRequest?> GetAsync(Guid id) =>
        Task.FromResult(_store.TryGetValue(id, out var r) ? r : null);

    public Task<bool> UpdateAsync(Guid id, SpecialRequestDto dto)
    {
        if (!_store.ContainsKey(id)) return Task.FromResult(false);
        var updated = new SpecialRequest(id, dto.BookingId, dto.Note, dto.Priority);
        _store[id] = updated;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(Guid id) =>
        Task.FromResult(_store.TryRemove(id, out _));

    public Task<IReadOnlyList<SpecialRequest>> ListAsync() =>
        Task.FromResult((IReadOnlyList<SpecialRequest>)_store.Values.ToList());

    public Task<IReadOnlyList<SpecialRequest>> ListByBookingAsync(Guid bookingId) =>
        Task.FromResult((IReadOnlyList<SpecialRequest>)_store.Values.Where(r => r.BookingId == bookingId).ToList());
}
