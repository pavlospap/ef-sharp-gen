using EFSharpGen.Design.Models;

namespace EFSharpGen;

/// <summary>
/// An implementation of the application context.
/// </summary>
public class AppContext : IAppContext
{
    /// <summary>
    /// The <see cref="Design.Models.Schema"/> that is used for the code
    /// generation.
    /// </summary>
    public virtual Schema Schema { get; set; } = default!;
}
