using System.Dynamic;
using AlphaVisa.Application.AttachmentItems.Command;
using AlphaVisa.Application.AttachmentItems.Query;
using AlphaVisa.Web.Filters;
using Microsoft.AspNetCore.Authorization;

namespace AlphaVisa.Web.Endpoints;

public class AttachmentItems : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetAttachmentByObjectId, "{objectId:guid}")
            .MapPost(UploadAttachment, actions: new List<Action<RouteHandlerBuilder>>() {
                (route) => route.AddEndpointFilter<MultipartFormDataFilter>(),
                (route) => route.AddEndpointFilter<DisableFormValueModelBindingFilter>(),
                (route) => route.DisableAntiforgery()
            });
    }

    public async Task<object> UploadAttachment(IFormFileCollection files, ISender sender)
    {
        var uploadedFiles = new List<CreateAttachmentItemBriefDto>();

        foreach (var file in files)
        {
            var command = new CreateAttachmentItem(file);
            var fileDto = await sender.Send(command);
            uploadedFiles.Add(fileDto);
        }

        dynamic returnedValue = new ExpandoObject();
        returnedValue.type = 2;
        returnedValue.body = uploadedFiles;

        return returnedValue;
    }

    [AllowAnonymous]
    public async Task<IResult> GetAttachmentByObjectId(ISender sender, Guid objectId)
    {
        var attachmentItem = await sender.Send(new GetAttachmentItemByObjectIdQuery(objectId));

        var filePath = Path.Combine("FileStores", attachmentItem.Name!);

        if (!System.IO.File.Exists(filePath))
        {
            return Results.NotFound("File not found");
        }

        var mimeType = attachmentItem.MimeType ?? "application/octet-stream";
        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

        return Results.File(fileStream, contentType: mimeType, fileDownloadName: attachmentItem.Name);
    }
}
