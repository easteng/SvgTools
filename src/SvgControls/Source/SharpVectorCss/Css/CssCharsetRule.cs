using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SharpVectors.Dom.Css
{
    /// <summary>
    /// The CSSCharsetRule interface represents a @charset rule in a CSS style sheet. The value of the 
    /// encoding attribute does not affect the encoding of text data in the DOM objects; this encoding is 
    /// always UTF-16. After a stylesheet is loaded, the value of the encoding attribute is the value found 
    /// in the @charset rule. If there was no @charset in the original document, then no CSSCharsetRule is 
    /// created. The value of the encoding attribute may also be used as a hint for the encoding used on 
    /// serialization of the style sheet.
    ///	The value of the @charset rule (and therefore of the CSSCharsetRule) may not correspond to the 
    ///	encoding the document actually came in; character encoding information e.g. in an HTTP header, has 
    ///	priority (see CSS document representation) but this is not reflected in the CSSCharsetRule.
    /// </summary>
    public sealed class CssCharsetRule : CssRule, ICssCharsetRule
    {
        #region Private Fields

        private static readonly Regex regex = new Regex(@"^@charset\s""(?<charsetencoding>[^""]+)"";");

        private string _encoding;

        #endregion

        #region Constructors

        /// <summary>
        /// The constructor for CssCharSetRule
        /// </summary>
        /// <param name="match">The Regex match that found the charset rule</param>
        /// <param name="parent">The parent rule or parent stylesheet</param>
        /// <param name="readOnly">True if this instance is readonly</param>
        /// <param name="replacedStrings">An array of strings that have been replaced in the string 
        /// used for matching. These needs to be put back use the DereplaceStrings method</param>
        /// <param name="origin">The type of CssStyleSheet</param>
        internal CssCharsetRule(Match match, object parent, bool readOnly, IList<string> replacedStrings, CssStyleSheetType origin)
            : base(parent, readOnly, replacedStrings, origin)
        {
            _encoding = DeReplaceStrings(match.Groups["charsetencoding"].Value);
        }

        #endregion

        #region Internal Static Methods

        internal static CssRule Parse(ref string css, object parent, bool readOnly,
            IList<string> replacedStrings, CssStyleSheetType origin)
        {
            Match match = regex.Match(css);
            if (match.Success)
            {
                CssCharsetRule rule = new CssCharsetRule(match, parent, readOnly, replacedStrings, origin);
                css = css.Substring(match.Length);
                return rule;
            }

            // didn't match => do nothing
            return null;
        }

        #endregion

        #region Implementation of ICssCharsetRule

        /// <summary>
        /// The encoding information used in this @charset rule
        /// </summary>
        public string Encoding
        {
            get {
                return _encoding;
            }
            set {
                /*
				 * TODO: <c>SYNTAX_ERR:</c> Raised if the specified encoding value has a syntax error and 
                 * is unparsable.
				 * */
                if (_isReadOnly)
                    throw new DomException(DomExceptionType.NoModificationAllowedErr);

                _encoding = value;
            }
        }
        #endregion

        #region Implementation of ICssRule

        /// <summary>
        /// The type of the rule. The expectation is that binding-specific casting methods can be 
        /// used to cast down from an instance of the CSSRule interface to the specific derived 
        /// interface implied by the type.
        /// </summary>
        public override CssRuleType Type
        {
            get {
                return CssRuleType.CharsetRule;
            }
        }

        #endregion
    }
}
