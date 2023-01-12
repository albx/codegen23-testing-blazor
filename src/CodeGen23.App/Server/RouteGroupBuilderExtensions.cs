using CodeGen23.App.Server.Services;
using CodeGen23.App.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeGen23.App.Server;

public static class RouteGroupBuilderExtensions
{
    public static void MapCardsEndpoints(this RouteGroupBuilder source)
    {
        source.MapGet(
            "/",
            (CardsService service) =>
            {
                var cards = service.GetAllCards();
                return Results.Ok(cards);
            }).RequireAuthorization();

        source.MapGet(
            "/{id}",
            (int id, CardsService service) =>
            {
                var card = service.GetCardDetail(id);
                if (card is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(card);
            }).RequireAuthorization().WithName("CardDetail");

        source.MapPost(
            "/",
            async ([FromBody] CreateCardModel model, CardsService service, ClaimsPrincipal user) =>
            {
                try
                {
                    var cardId = await service.CreateCardAsync(model, user.Identity!);
                    return Results.CreatedAtRoute("CardDetail", new { id = cardId }, model);
                }
                catch (InvalidOperationException)
                {
                    return Results.BadRequest();
                }
            }).RequireAuthorization();

        source.MapPut(
            "/{id}",
            async (int id, [FromBody] CardDetailModel model, CardsService service, ClaimsPrincipal user) =>
            {
                try
                {
                    await service.UpdateCardAsync(id, model, user.Identity!);
                    return Results.NoContent();
                }
                catch (ArgumentOutOfRangeException)
                {
                    return Results.NotFound();
                }
                catch (UnauthorizedAccessException)
                {
                    return Results.Forbid();
                }
            }).RequireAuthorization();

        source.MapDelete(
            "/{id}",
            async (int id, CardsService service, ClaimsPrincipal user) =>
            {
                try
                {
                    await service.DeleteCardAsync(id, user.Identity!);
                    return Results.NoContent();
                }
                catch (ArgumentOutOfRangeException)
                {
                    return Results.NotFound();
                }
                catch (UnauthorizedAccessException)
                {
                    return Results.Forbid();
                }
            }).RequireAuthorization();
    }
}
