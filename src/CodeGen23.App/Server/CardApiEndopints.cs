using CodeGen23.App.Server.Services;
using CodeGen23.App.Shared;
using Microsoft.AspNetCore.Mvc;

namespace CodeGen23.App.Server;

public static class CardApiEndopints
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
            "/search",
            ([FromQuery] string query, CardsService service) =>
            {
                var cards = service.SearchCardsByQuery(query);
                return Results.Ok(cards);
            });

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
            async ([FromBody] CreateCardModel model, CardsService service) =>
            {
                try
                {
                    var cardId = await service.CreateCardAsync(model);
                    return Results.CreatedAtRoute("CardDetail", new { id = cardId }, model);
                }
                catch (InvalidOperationException)
                {
                    return Results.BadRequest();
                }
            }).RequireAuthorization();

        source.MapPut(
            "/{id}",
            async (int id, [FromBody] CardDetailModel model, CardsService service) =>
            {
                try
                {
                    await service.UpdateCardAsync(id, model);
                    return Results.NoContent();
                }
                catch (ArgumentOutOfRangeException)
                {
                    return Results.NotFound();
                }
            }).RequireAuthorization();

        source.MapPatch(
            "/{id}/status",
            async (int id, [FromBody] ChangeCardStatusModel model, CardsService service) =>
            {
                try
                {
                    await service.ChangeCardStatusAsync(id, model.Status);
                    return Results.NoContent();
                }
                catch (ArgumentOutOfRangeException)
                {
                    return Results.NotFound();
                }
            }).RequireAuthorization();

        source.MapDelete(
            "/{id}",
            async (int id, CardsService service) =>
            {
                try
                {
                    await service.DeleteCardAsync(id);
                    return Results.NoContent();
                }
                catch (ArgumentOutOfRangeException)
                {
                    return Results.NotFound();
                }
            }).RequireAuthorization();
    }
}
