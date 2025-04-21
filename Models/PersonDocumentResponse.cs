using System;
using System.Text.Json.Serialization;

namespace DirectorySite.Models;

public class PersonDocumentResponse
{
    [JsonPropertyName("id")]
    public int Id;

    [JsonPropertyName("personId")]
    public Guid PersonId;

    [JsonPropertyName("fileName")]
    public string? FileName;

    [JsonPropertyName("filePath")]
    public string? FilePath;

    [JsonPropertyName("filePathBack")]
    public string? FilePathBack;

    [JsonPropertyName("mimmeType")]
    public string? MimmeType;

    [JsonPropertyName("fileUrl")]
    public string? FileUrl;

    [JsonPropertyName("fileUrlBack")]
    public string? FileUrlBack;

    [JsonPropertyName("fileSize")]
    public int? FileSize;

    [JsonPropertyName("fileSizeBack")]
    public int? FileSizeBack;

    [JsonPropertyName("documentTypeId")]
    public int? DocumentTypeId;

    [JsonPropertyName("documentTypeName")]
    public string? DocumentTypeName;

    [JsonPropertyName("valid")]
    public string? Valid;

    [JsonPropertyName("folio")]
    public string? Folio;

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt;

    [JsonPropertyName("deletedAt")]
    public DateTime? DeletedAt;

    public override string ToString()
    {
        return $"Id: {Id}, PersonId: {PersonId}, FileName: {FileName}, FilePath: {FilePath}, FilePathBack: {FilePathBack}, " +
               $"MimmeType: {MimmeType}, FileUrl: {FileUrl}, FileUrlBack: {FileUrlBack}, FileSize: {FileSize}, " +
               $"FileSizeBack: {FileSizeBack}, DocumentTypeId: {DocumentTypeId}, DocumentTypeName: {DocumentTypeName}, " +
               $"Valid: {Valid}, Folio: {Folio}, CreatedAt: {CreatedAt}, DeletedAt: {DeletedAt}";
    }
}
