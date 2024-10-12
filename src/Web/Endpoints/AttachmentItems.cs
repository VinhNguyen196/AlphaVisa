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

        return uploadedFiles;
    }

    [AllowAnonymous]
    public async Task<IResult> GetAttachmentByObjectId(ISender sender, Guid objectId, bool isThumbnail = false)
    {
        var attachmentItem = await sender.Send(new GetAttachmentItemByObjectIdQuery(objectId));

        var filePath = Path.Combine("filestores", attachmentItem.ObjectId.GetValueOrDefault().ToString());
        if (isThumbnail)
        {
            filePath += "_thumbnail";
        }

        if (!File.Exists(filePath))
        {
            return Results.NotFound("File not found");
        }

        var mimeType = isThumbnail ? "image/jpeg" : (attachmentItem.MimeType ?? "application/octet-stream");
        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

        return Results.File(fileStream, contentType: mimeType, fileDownloadName: attachmentItem.Name);
    }
}
