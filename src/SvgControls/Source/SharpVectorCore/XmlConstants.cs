﻿using System;

namespace SharpVectors.Dom
{
    /// <summary>
    /// Contains common XML constants.
    /// </summary>
    public static class XmlConstants
    {
        // Namespace URIs
        public const string XmlNamespaceUri       = "http://www.w3.org/XML/1998/namespace";
        public const string XmlnsNamespaceUri     = "http://www.w3.org/2000/xmlns/";
        public const string XlinkNamespaceUri     = "http://www.w3.org/1999/xlink";
        public const string XmlEventsNamespaceUri = "http://www.w3.org/2001/xml-events";

        // Namespace prefixes
        public const string XmlPrefix          = "xml";
        public const string XmlnsPrefix        = "xmlns";
        public const string XlinkPrefix        = "xlink";
        /// <summary>
        /// Specifies a namespace prefix.
        /// </summary>
        public static readonly string XmlNamespacePrefix = XmlnsPrefix + ':' + XmlPrefix;  // xmlns:xml

        // xml:{base, id, lang, space} and XML Events attributes
        public const string BaseAttribute  = "base";
        public const string IdAttribute    = "id";
        public const string LangAttribute  = "lang";
        public const string SpaceAttribute = "space";

        public static readonly string BaseQname   = XmlPrefix + ':' + BaseAttribute;    // xml:base
        public static readonly string IdQname     = XmlPrefix + ':' + IdAttribute;      // xml:id
        public static readonly string LangQname   = XmlPrefix + ':' + LangAttribute;    // xml:lang
        public static readonly string SpaceQname  = XmlPrefix + ':' + SpaceAttribute;   // xml:space

        public const string DefaultValue          = "default";
        public const string PreserveValue         = "preserve";
        public const string EventsEventAttribute  = "event";

        // XLink attributes
        public const string XlinkHash                = "#";
        public const string XlinkHrefAttribute       = "href";
        public static readonly string XlinkHrefQname = XlinkPrefix + ':' + XlinkHrefAttribute;  // xlink:href
    }
}
