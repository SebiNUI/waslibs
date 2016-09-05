﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStudio.Uwp.Html
{
    public sealed class HtmlNode : HtmlFragment
    {
        public Dictionary<string, string> Attributes { get; }

        internal HtmlNode(HtmlTag openTag)
        {
            Name = openTag.Name.ToLowerInvariant();
            Attributes = openTag.Attributes;
        }
    }
}
