using System;
using System.Collections.Generic;

namespace AutoForm.Analysis.Models
{
    public readonly struct ModelSpace : IEquatable<ModelSpace>
    {
        public readonly IEnumerable<Template> Templates;
        public readonly IEnumerable<Model> Models;
        public readonly IEnumerable<Control> Controls;
        private readonly String _json;
        private readonly String _string;

        private ModelSpace(IEnumerable<Model> models, IEnumerable<Control> controls, IEnumerable<Template> templates)
        {
            models.ThrowOnDuplicate(TypeIdentifierName.AutoControlModelAttribute.ToString());
            controls.ThrowOnDuplicate(TypeIdentifierName.AutoControlAttribute.ToString());
            templates.ThrowOnDuplicate(TypeIdentifierName.AutoControlTemplateAttribute.ToString());

            Models = models;
            Controls = controls;
            Templates = templates;

            _json = Json.Object(Json.KeyValuePair(nameof(Models), Models),
                                Json.KeyValuePair(nameof(Controls), Controls),
                                Json.KeyValuePair(nameof(Templates), Templates));
            _string = _json;
        }

        public static ModelSpace Create(IEnumerable<Model> models, IEnumerable<Control> controls, IEnumerable<Template> templates)
        {
            return new ModelSpace(models, controls, templates);
        }

        public override String ToString()
        {
            return _json ?? "null";
        }
        public String ToEscapedString()
        {
            return _string ?? String.Empty;
        }

        public override Boolean Equals(Object obj)
        {
            return obj is ModelSpace space && Equals(space);
        }

        public Boolean Equals(ModelSpace other)
        {
            return _json == other._json;
        }

        public override Int32 GetHashCode()
        {
            return -992964542 + EqualityComparer<String>.Default.GetHashCode(_json);
        }

        public static Boolean operator ==(ModelSpace left, ModelSpace right)
        {
            return left.Equals(right);
        }

        public static Boolean operator !=(ModelSpace left, ModelSpace right)
        {
            return !(left == right);
        }
    }
}
