using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace DAL.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum VendorDocuments
{
    PAN,
    Aadhar,
    GST,
    BusinessLicense
}