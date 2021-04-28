namespace SharpVectors.Dom.Css
{
	/// <summary>
	/// This interface allows the DOM user to create a <see cref="ICssStyleSheet"/> outside the context of a document. 
	/// There is no way to associate the new <see cref="ICssStyleSheet"/> with a document in DOM Level 2. 
	/// </summary>
	/// <remarks>
	/// This interface should inherit from <see cref="IDomImplementation"/> but System.Xml does not have 
	/// this interface, just a class. You can not inherit from a class in an interface.
	/// </remarks>	
	public interface IDomImplementationCss : IDomImplementation
    {
		/// <summary>
		/// Creates a new <see cref="ICssStyleSheet"/>.
		/// </summary>
		/// <param name="title">The advisory title. See also the Style Sheet Interfaces section.</param>
		/// <param name="media">The comma-separated list of media associated with the new style sheet. 
		/// See also the Style Sheet Interfaces section.</param>
		/// <returns>A new CSS style sheet.</returns>
		ICssStyleSheet CreateCssStyleSheet(string title, string media);

		//bool HasFeature(string feature,string version);
		//DocumentType CreateDocumentType(string qualifiedName, string publicId, string systemId);
		//Document CreateDocument(string namespaceUri, string qualifiedName, DocumentType doctype);
	}
}
