using System;
using Ubik.Web.Components.Contracts;

namespace Ubik.Web.Components.Domain
{

    public class Textual : ITextualInfo
    {
        public string Subject { get; private set; }

        public string Summary { get; private set; }

        public string Body { get; private set; }

        public Textual(string subject, string summary, string description)
        {
            Subject = subject;
            Summary = summary;
            Body = description;
        }

        public Textual(string subject, string summary)
            : this(subject, summary, string.Empty)
        {
        }

        public Textual(string subject)
            : this(subject, string.Empty, string.Empty)
        {
        }

        public Textual NewSubject(string subject)
        {
            return new Textual(subject, Summary, Body);
        }

        public Textual NewSummary(string summary)
        {
            return new Textual(Subject, summary, Body);
        }

        public Textual NewDescription(string description)
        {
            return new Textual(Subject, Summary, description);
        }
    }
}