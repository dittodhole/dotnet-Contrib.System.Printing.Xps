/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.Printing.Xps
{
  using global::System;
  using global::System.Runtime.Serialization;
  using global::JetBrains.Annotations;

  [Serializable]
#if CONTRIB_SYSTEM_PRINTING_XPS
  internal sealed
#else
  internal
#endif
  partial class XpsNameSerializer : IObjectReference,
                                    ISerializable
  {
    /// <summary>
    ///   Initializes a new instance of the <see cref="T:Contrib.System.Printing.Xps.XpsNameSerializer"/> class.
    /// </summary>
    /// <param name="serializationInfo"/>
    /// <param name="streamingContext"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="serializationInfo"/> is <see langword="null"/>.</exception>
    public XpsNameSerializer([NotNull] SerializationInfo serializationInfo,
                             StreamingContext streamingContext)
    {
      if (serializationInfo == null)
      {
        throw new ArgumentNullException(nameof(serializationInfo));
      }
      this.ExpandedName = serializationInfo.GetString(nameof(this.ExpandedName));
    }

    [CanBeNull]
    public string ExpandedName { get; }

    /// <inheritdoc/>
    object IObjectReference.GetRealObject(StreamingContext context)
    {
      XpsName result;
      if (this.ExpandedName == null)
      {
        result = null;
      }
      else
      {
        result = XpsName.Get(this.ExpandedName);
      }

      return result;
    }

    /// <inheritdoc/>
    void ISerializable.GetObjectData(SerializationInfo info,
                                     StreamingContext context)
    {
      throw new NotSupportedException();
    }
  }
}
