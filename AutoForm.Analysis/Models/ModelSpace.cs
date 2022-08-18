using System;
using System.Collections.Generic;

namespace AutoForm.Analysis.Models
{
    public readonly struct ModelSpace : IEquatable<ModelSpace>
    {
        public readonly IEnumerable<Model> Models;
        public readonly IEnumerable<FallbackControl> FallbackControls;
        public readonly IEnumerable<FallbackTemplate> FallbackTemplates;
        private readonly String _json;
        private readonly String _string;

        private ModelSpace(IEnumerable<Model> models, IEnumerable<FallbackControl> controls, IEnumerable<FallbackTemplate> templates)
        {
            models.ThrowOnDuplicate(TypeIdentifierName.ModelAttribute.ToString());
            controls.ThrowOnDuplicate(TypeIdentifierName.FallbackControlAttribute.ToString());
            templates.ThrowOnDuplicate(TypeIdentifierName.FallbackTemplateAttribute.ToString());

            Models = models;
            FallbackControls = controls;
            FallbackTemplates = templates;

            _json = Json.Object(Json.KeyValuePair(nameof(Models), Models),
                                Json.KeyValuePair(nameof(FallbackControls), FallbackControls),
                                Json.KeyValuePair(nameof(FallbackTemplates), FallbackTemplates));
            _string = _json;
        }

        public static ModelSpace Create(IEnumerable<Model> models, IEnumerable<FallbackControl> controls, IEnumerable<FallbackTemplate> templates)
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
