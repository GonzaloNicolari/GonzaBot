using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using MyEchoBot.Entities;
using Microsoft.Bot.Builder;

namespace MyEchoBot.Dialogs
{
    public class BotReviewDialog : ComponentDialog
    {
        private readonly IStatePropertyAccessor<BotReview> _BotReviewAccessor;

        public BotReviewDialog() : base(nameof(BotReviewDialog))
        {
            AddDialog(new ConfirmPrompt("isHelpFullPrompt"));
            AddDialog(new ConfirmPrompt("isEasyToDealPrompt"));
            AddDialog(new NumberPrompt<int>("PunctuatioPrompt"));
            AddDialog(new TextPrompt("ThingsToImprovePrompt"));
            AddDialog(new ConfirmPrompt("SummaryPrompt"));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                IsHelpFullStepAsync,
                IsEasyToDealStepAsync,
                PunctuationStepAsync,
                ThingsToImproveStepAsync,
                SummaryStepAsync
            }));


            InitialDialogId = nameof(WaterfallDialog);

        }

        private static async Task<DialogTurnResult> IsHelpFullStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationtoken)
        {
            //Create an object to collect user bot review
            stepContext.Values["BotReview"] = new BotReview();

            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Was the bot helpfull for you?")};

            //Ask the user if the bot was helpfull or not
            return await stepContext.PromptAsync("isHelpFullPrompt", promptOptions, cancellationtoken);

        }
        private static async Task<DialogTurnResult> IsEasyToDealStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationtoken)
        {
            //collect bot review
            var botReview = stepContext.Values["BotReview"] as BotReview;
            botReview.isHelpFull = (bool)stepContext.Result;

            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("The bot was easy to deal with?")};
            
            //ask the user if the bot was easy to deal with
            return await stepContext.PromptAsync("isEasyToDealPrompt", promptOptions, cancellationtoken);
        }
        private static async Task<DialogTurnResult> PunctuationStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationtoken)
        {
            var botReview = stepContext.Values["BotReview"] as BotReview;
            botReview.isEasyToDeal = (bool)stepContext.Result;
            
            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("In a scale of 1 to 5, how was the bot service?") };
            //ask the user to give a punctuation about the bot service
            return await stepContext.PromptAsync("PunctuatioPrompt", promptOptions, cancellationtoken);
        }
        private static async Task<DialogTurnResult> ThingsToImproveStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationtoken)
        {
            var botReview = stepContext.Values["BotReview"] as BotReview;
            botReview.Punctuation = (int)stepContext.Result;
            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("What things do you think we should improve to give a better service?") };
            //ask the user to give a feedback of things to improve
            return await stepContext.PromptAsync("ThingsToImprovePrompt", promptOptions, cancellationtoken);

        }
        private static async Task<DialogTurnResult> SummaryStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationtoken)
        {
            var botReview = stepContext.Values["BotReview"] as BotReview;
            botReview.ThingsToImprove = (string)stepContext.Result;
            
            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Thank you for your time and feedback!") };
            //final step
            return await stepContext.EndDialogAsync(botReview, cancellationtoken);
        }
    }
}
