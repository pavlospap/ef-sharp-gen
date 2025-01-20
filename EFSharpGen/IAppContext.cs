using EFSharpGen.Design.Models;

namespace EFSharpGen;

/// <summary>
/// The application context.
/// </summary>
public interface IAppContext
{
    /// <summary>
    /// The <see cref="Design.Models.Schema"/> that is used for the code
    /// generation.
    /// </summary>
    Schema Schema { get; set; }
}
