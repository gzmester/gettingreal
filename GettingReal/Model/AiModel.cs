using System;
using System.Text.Json;
using GettingReal.Services;

namespace GettingReal.Model
{
    public class AiModel
    {
        public string aiModel { get; set; }
        public string id { get; set; }
        public double prisPerToken { get; set; }

        public AiModel() { }

        public AiModel(string aiModel, string id, double prisPerToken)
        {
            this.aiModel = aiModel;
            this.id = id;
            this.prisPerToken = prisPerToken;
        }

        public static AiModel GetAiModel(string id)
        {
            return Lager.Instance.HentModel(id);
        }

        public override string ToString()
        {
            return $"AI Model: {aiModel}, ID: {id}, Price per Token: {prisPerToken}";
        }
    }
}
